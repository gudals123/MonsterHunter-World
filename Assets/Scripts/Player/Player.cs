using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.Rendering.DebugUI;


public class Player : Entity
{
    private PlayerController playerController;

    [Header("Weapons")]
    [SerializeField] private GameObject _handWeapon;
    [SerializeField] private GameObject _BackWeapon;

    [Header("Object")]
    [SerializeField] private GameObject chargingEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraArm;
    public GameObject attackRange;

    [Header("GroundCheck")]
    [SerializeField] private float groundcheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    private Collider playerCollider;

    [Header("Power")]
    private float rollPower = 8.5f;
    private float knockbackPower = 2.5f;


    public float currentStamina { get; private set; }
    public float maxStamina { get; private set; }
    public int damage {  get; private set; }
    public bool isArmed {  get; private set; }
    public bool isRoll;
    public float CollisionCoolTime { get; private set; }


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        maxHp = 150;
        currentHp = maxHp;
        maxStamina = 100f;
        currentStamina = maxStamina;
        isArmed = false;
        isRoll = false;
        chargingEffect.SetActive(false);
        healEffect.SetActive(false);
        _handWeapon.SetActive(false);
        _BackWeapon.SetActive(true);
    }
    private void Update()
    {
        CollisionCoolTime += Time.deltaTime;
    }

    public void DrainStamina(float value)
    {
        currentStamina -= value;
        currentStamina = Math.Clamp(currentStamina, 0, maxStamina);
        animator.SetFloat("Stamina", currentStamina);
        UIManager.Instance.UpdateSPBar(currentStamina, maxStamina);
    }

    public void RecoveryStaminer(float value)
    {
        if (currentStamina >= maxStamina)
        {
            return;
        }
        currentStamina += value;
        currentStamina = Math.Clamp(currentStamina, 0, maxStamina);
        animator.SetFloat("Stamina", currentStamina);
        UIManager.Instance.UpdateSPBar(currentStamina, maxStamina);
    }

    public bool StaminaCheck(float cost)
    {
        if(currentStamina - cost <= 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void Move(float moveSpeed, Vector2 moveInput)
    {
        AnimatorControll(playerController.playerState);
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        characterBody.rotation = Quaternion.Slerp(characterBody.rotation, targetRotation, Time.deltaTime * moveSpeed);

        rigidbody.MovePosition(transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    public void ApplyState()
    {
        AnimatorControll(playerController.playerState);
    }

    public bool SwitchDoneCheck()
    {
        return animator.GetBool(PlayerAnimatorParamiter.IsSwitchDone);
    }

    public void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector2.down, groundcheckDistance, whatIsGround))
        {
            isGrounded =  true;
        }
        else
        {
            isGrounded = false;
        }
/*        Vector3 endPosition = transform.position + Vector3.down * groundcheckDistance;
        Debug.DrawLine(transform.position, endPosition, Color.red);*/

        animator.SetBool(PlayerAnimatorParamiter.IsGrounded, isGrounded);
    }

    public void AnimatorControll(PlayerState state)
    {
        animator.SetBool(PlayerAnimatorParamiter.IsDead, state == PlayerState.Dead);
        animator.SetBool(PlayerAnimatorParamiter.IsMoving, (state == PlayerState.Run) || (state == PlayerState.Walk) ||(state == PlayerState.Tired));
        animator.SetBool(PlayerAnimatorParamiter.IsRoll, state == PlayerState.Roll);
        animator.SetBool("IsLeftShift", state == PlayerState.Run || (state == PlayerState.Tired));
        animator.SetBool(PlayerAnimatorParamiter.IsAttacking, state == PlayerState.Attack);
        animator.SetBool(PlayerAnimatorParamiter.IsRightAttak, playerController.isRightAttack);
        animator.SetBool(PlayerAnimatorParamiter.IsArmed, isArmed);
        
    }

    public void SetAnimator(string name, bool value)
    {
        animator.SetBool(name, value);
    }
    public void Roll()
    {
        //AnimatorControll(playerController.playerState);
        attackRange.SetActive(false);
        Vector3 rollDirection = new Vector3(animator.transform.localRotation.x, 0, animator.transform.localRotation.z);
        rigidbody.velocity = rollDirection + animator.transform.forward * rollPower;
    }

    public bool DoNotDisturbCheck()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsTag("DoNotDisturb");
    }

    public void WeaponSwitch()
    {
        AnimatorControll(playerController.playerState);
        isArmed = !isArmed;
    }

    public override void Hit(int damage)
    {
        if (currentHp <= 0)
        {
            //Dead;
            return;
        }
        currentHp -= damage;
        UIManager.Instance.UpdateHPBar(currentHp, maxHp);
    }

    public void Heal(int healingAmount)
    {
        currentHp += healingAmount;
        currentHp = Math.Clamp(currentHp , 0 , maxHp);
        UIManager.Instance.UpdateHPBar(currentHp, maxHp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossAttack") && CollisionCoolTime > 1)
        {
            Debug.Log("Ãæµ¹");
            CollisionCoolTime = 0;
            WeaponSetActive();
            attackRange.SetActive(false);
            knockback(transform.position, other.transform.position);
            StartCoroutine(GetHit());
            Hit(10);

            /*            other = GetComponent<Anjanath>();
                        int damage = other.Attack();*/
            /*            other = GetComponentInParent<Test>();
                        int damage = other.Attack();*/
            //Taget = other.GetComponent<Anjanath>();

            //            Taget = other.GetComponent<Bresssssssss>();
            //            SetDamage(Taget.attackValie);
        }   
    }
    private void knockback(Vector3 playerPos, Vector3 attackColliderPos)
    {
        Vector3 direction = playerPos - attackColliderPos;
        direction = new Vector3(direction.x, 0, direction.z);
        direction.Normalize();
        rigidbody.AddForce(direction * knockbackPower, ForceMode.Impulse);
    }

    private IEnumerator GetHit()
    {
        animator.SetBool(PlayerAnimatorParamiter.IsGetHit, true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool(PlayerAnimatorParamiter.IsGetHit, false);
    }


    public void ChargingEffectPlay()
    {
        chargingEffect.SetActive(true);
        StartCoroutine(ObjectDeactivated(chargingEffect, 1.0f));
    }
    public void HealEffectPlay()
    {
        healEffect.SetActive(true);
        StartCoroutine(ObjectDeactivated(healEffect, 1.0f));
    }


    private IEnumerator ObjectDeactivated(GameObject target , float time)
    {
        yield return new WaitForSeconds(time);
        target.SetActive(false);
    }



    public IEnumerator Snag()
    {
        animator.speed = 0.01f;
        yield return new WaitForSeconds(0.1f);
        animator.speed = 1f;
    }

    public void WeaponSetActive()
    {
        if (isArmed)
        {
            _handWeapon.SetActive(true);
            _BackWeapon.SetActive(false);
        }
        else
        {
            _handWeapon.SetActive(false);
            _BackWeapon.SetActive(true);
        }
    }


}
