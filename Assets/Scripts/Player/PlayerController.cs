using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;



public class PlayerController : Controller
{
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
        WeaponSheath
    }

    private Player player;
    [SerializeField]  private Cat cat;

    private GreatSword greatSword;
    private float walkSpeed = 4f;
    private float runSpeed = 7f;

    [SerializeField] private Transform cameraArm;
    [SerializeField] private GameObject weaponObj;

    //Pet pet
    public bool isRightAttack { get; private set; }
    public bool isRoll { get; private set; }
    public bool isCharging { get; set; }


    private float chargeTime = 0f;
    private float maxChargeTime = 3f;

    public PlayerState playerState { get; private set; }

    //½½·Ô
    private Item_Potion potion;
    private Skill_CatAttack catAttack;
    private Skill_CatHeal catHeal;

    //Äü½½·Ô
    private Slot[] QuickSlot;
    private int quickSlotIndex = 0;
    private int quickSlotCount = 0;

    public void Start()
    {
        player = GetComponent<Player>();
        greatSword = weaponObj.GetComponent<GreatSword>();
        playerState = PlayerState.Idle;
        moveSpeed = walkSpeed;
        isRoll = false;
        potion = new Item_Potion(player, 10, 10);
        catAttack = new Skill_CatAttack(cat);
        catHeal = new Skill_CatHeal(cat);
        QuickSlot = new Slot[4];
        QuickSlot[0] = potion;
        QuickSlot[1] = catAttack;
        QuickSlot[2] = catHeal;
        quickSlotCount = 3;
    }




    public void Update()
    {
        InputSend();
        player.GroundCheck();
        LookAround();
        QuickSlotIndexChange();
    }

    public void InputSend()
    {

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //±¸¸£±â
        if (Input.GetKeyDown(KeyCode.Space) && !isRoll)
        {
            isRoll = true;
            playerState = PlayerState.Roll;
            player.Roll();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isRoll = false;
        }
        //¹«±â ½ºÀ§Ä¡
        else if ((player.isArmed && Input.GetKeyDown(KeyCode.LeftShift)) ||
            (!player.isArmed && Input.GetMouseButtonDown(0)))
        {
            playerState = PlayerState.WeaponSheath;
            player.WeaponSwitch();
        }
        //´Þ¸®±â
        else if (moveInput != Vector2.zero && Input.GetKey(KeyCode.LeftShift) && !player.isArmed
            && player.SwitchDoneCheck())
        {
            playerState = PlayerState.Run;
            moveSpeed = runSpeed;
            player.Move(moveSpeed, moveInput);
        }
        //°È±â
        else if (moveInput != Vector2.zero)
        {
            playerState = PlayerState.Walk;
            moveSpeed = walkSpeed;
            player.Move(moveSpeed, moveInput);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            QuickSlot[quickSlotIndex].Activate();
        }
        //Á¤Áö
        else
        {
            playerState = PlayerState.Idle;
            player.ApplyState();
        }
        if (player.isArmed)
        {
            if (Input.GetMouseButton(0))
            {
                playerState = PlayerState.Attack;
                isRightAttack = false;
                player.ApplyState();
                chargeTime += Time.deltaTime;
                if (chargeTime > maxChargeTime)
                {
                    isCharging = false;
                    //playerState = PlayerState.Idle;
                    player.ApplyState();
                    greatSword.AttackDamageSet(isRightAttack, chargeTime);
                    chargeTime =0;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isCharging = false;
                //playerState = PlayerState.Idle;
                player.ApplyState();
                greatSword.AttackDamageSet(isRightAttack, chargeTime);
                chargeTime = 0;
            }
            if (Input.GetMouseButtonDown(1))
            {
                isRightAttack = true;
                playerState = PlayerState.Attack;
                player.ApplyState();
                greatSword.AttackDamageSet(isRightAttack, chargeTime);
            }
            if (Input.GetMouseButtonUp(1))
            {
                //playerState = PlayerState.Idle;
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

        if (scroll > 0f)
        {
            quickSlotIndex = (quickSlotIndex + 1) % (quickSlotCount);
        }
        else if(scroll < 0f)
        {
            quickSlotIndex = (quickSlotIndex + 1) % (quickSlotCount);
        }
        //Debug.Log($"ÇöÀç Äü½½·Ô Index{quickSlotIndex}");
    }


}
