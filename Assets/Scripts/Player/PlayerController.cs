using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public enum PlayerState
{
    Idle,
    GetHit,
    Dead,
    Attack,
    Roll,
    Fall,
    Walk,
    Run,
    WeaponSheath,
    Tired

}
public class PlayerController : Controller
{
    private Player player;
    [SerializeField] private Cat cat;
    private GreatSword greatSword;
    
    private float walkSpeed = 4f;
    private float tiredSpeed = 2f;
    private float runSpeed = 7f;

    private float staminaCostRun = 1f;
    private float staminaCostRoll = 30f;
    private float staminaRecoveryCost = 30f;

    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private GameObject weaponObj;

    //Pet pet
    public bool isRightAttack { get; private set; }
    public bool isCharging { get; set; }
    public bool isAnimationPauseDone { get; set; }
    public bool isMediumCharged { get; private set; }
    public bool isMaxCharged { get; private set; }
    public bool isChargeAttackDone { get; set; }
    public bool isAttackDone { get; set; }
    public bool isInputAttack { get; set; }


    private float chargeTime;
    private float maxChargeTime = 3f;
    private float switchWaitingTime = 0;

    public PlayerState playerState { get; private set; }

    //슬롯
    private Item_Potion potion;
    private Skill_CatAttack catAttack;
    private Skill_CatHeal catHeal;

    //퀵슬롯
    private Slot[] quickSlot;



    private int quickSlotIndex = 0;
    private int quickSlotCount = 0;

    public void Start()
    {
        player = GetComponent<Player>();
        greatSword = weaponObj.GetComponent<GreatSword>();
        playerState = PlayerState.Idle;
        moveSpeed = walkSpeed;
        isAnimationPauseDone = false;
        isMediumCharged = false;
        isMaxCharged = false;
        isChargeAttackDone = true;
        isAttackDone = true;
        chargeTime = 0;
        potion = new Item_Potion(player, 10, 10);
        catAttack = new Skill_CatAttack(cat);
        catHeal = new Skill_CatHeal(cat);
        quickSlot = new Slot[4];
        quickSlot[0] = potion;
        quickSlot[1] = catAttack;
        quickSlot[2] = catHeal;
        quickSlotCount = 3;
    }




    public void Update()
    {
        InputSend();
        player.GroundCheck();
        LookAround();
        QuickSlotIndexChange();
        StaminerRecovery();
        
    }

    public void InputSend()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //구르기
        if (Input.GetKeyDown(KeyCode.Space) && !player.isRoll && player.SwitchDoneCheck())
        {
            if (player.StaminaCheck(staminaCostRoll))
            {
                
                playerState = PlayerState.Roll;
                player.SetAnimator("IsRoll", true);
                //player.ApplyState();
                //player.DrainStamina(staminaCostRoll);
                //player.Roll();
                //StartCoroutine(RollCoolTime());
            }
        }
        //무기 스위치
        else if ((player.isArmed && Input.GetKeyDown(KeyCode.LeftShift) && !player.isRoll) ||
            (!player.isArmed && Input.GetMouseButtonDown(0) && !player.isRoll) ||
            (Input.GetKeyDown(KeyCode.E) && player.isArmed && !player.isRoll))
        {
            playerState = PlayerState.WeaponSheath;
            player.WeaponSwitch();
        }
        else if (moveInput != Vector2.zero && Input.GetKey(KeyCode.LeftShift) && !player.isArmed
            && player.SwitchDoneCheck() && !player.DoNotDisturbCheck())
        {
            //지침
            if (player.currentStamina < 30f)
            {
                playerState = PlayerState.Tired;
                moveSpeed = tiredSpeed;
                player.DrainStamina(staminaCostRun * Time.deltaTime);
                player.Move(moveSpeed, moveInput);
            }
            //달리기
            else
            {
                playerState = PlayerState.Run;
                moveSpeed = runSpeed;
                player.DrainStamina(staminaCostRun * Time.deltaTime);
                player.Move(moveSpeed, moveInput);
            }
        }
        //걷기
        else if (moveInput != Vector2.zero && !player.DoNotDisturbCheck())
        {
            playerState = PlayerState.Walk;
            moveSpeed = walkSpeed;
            player.Move(moveSpeed, moveInput);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !player.isArmed)
        {
            quickSlot[quickSlotIndex].Activate();
        }
        //정지
        else
        {
            switchWaitingTime += Time.deltaTime;
            if (switchWaitingTime > 0.1)
            {
                playerState = PlayerState.Idle;
                player.ApplyState();
                switchWaitingTime =0;
            }
        }
        if (player.isArmed && player.StaminaCheck(0) && !player.isRoll)
        {
            if (Input.GetMouseButton(0) && isChargeAttackDone)
            {
                playerState = PlayerState.Attack;
                isInputAttack = true;   
                isRightAttack = false;
                player.ApplyState();
                if (isCharging)
                {
                    chargeTime += Time.deltaTime;
                    if (chargeTime >= 1f && !isMediumCharged)
                    {
                        player.ChargingEffectPlay();
                        isMediumCharged = true;
                    }
                    if (chargeTime >= 2.5f && !isMaxCharged)
                    {
                        player.ChargingEffectPlay();
                        isMaxCharged = true;
                    }
                    if (chargeTime > maxChargeTime)
                    {
                        isCharging = false;
                        isMediumCharged = false;
                        isMaxCharged = false;
                        player.ApplyState();
                        greatSword.AttackDamageSet(isRightAttack, chargeTime);
                        chargeTime = 0;
                    }
                }
            }
            else
            {
               
                isInputAttack = false;
                isCharging = false ;
                isMediumCharged = false;
                isMaxCharged = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                player.ApplyState();
                greatSword.AttackDamageSet(isRightAttack, chargeTime);
                chargeTime = 0;
            }
            if (Input.GetMouseButtonDown(1) && isAttackDone)
            {
                isRightAttack = true;
                isChargeAttackDone = false;
                greatSword.AttackDamageSet(isRightAttack, chargeTime);
                playerState = PlayerState.Attack;
                player.ApplyState();
            }
            if (Input.GetMouseButtonUp(1))
            {
                player.ApplyState();
            }
        }
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335, 361);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void QuickSlotIndexChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll < 0f)
        {
            quickSlotIndex = (quickSlotIndex + 1) % (quickSlotCount);
        }
        else if (scroll > 0f)
        {
            quickSlotIndex = (quickSlotIndex - 1 + quickSlotCount) % (quickSlotCount);
        }

        UIManager.Instance.UpdateQuickSlotIcon(quickSlot, quickSlotIndex, quickSlotCount);
        //Debug.Log($"현재 퀵슬롯 Index{quickSlotIndex}");

    }

    public void StaminerRecovery()
    {
        if((playerState != PlayerState.Run) && (!player.isRoll) && (playerState != PlayerState.Tired))
        {
            player.RecoveryStaminer(staminaRecoveryCost * Time.deltaTime);
        }
    }

    
}
