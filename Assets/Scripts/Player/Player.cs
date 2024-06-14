using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static PlayerController;
using static UnityEngine.UI.Image;

public class Player : Entity
{
    private PlayerController playerController;
    
    [Header("Object")]
    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraArm;


    [Header("GroundCheck")]
    [SerializeField] private float groundcheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


    [Header("Power")]
    private float rollPower = 8.5f;
    private float knockbackPower = 2.5f;


    public float currentStamina { get; private set; }
    public float maxStamina { get; private set; }
    public int damage {  get; private set; }
    public bool isArmed {  get; private set; }
    

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();
        animator = characterBody.GetComponent<Animator>();
        maxHp = 150;
        currentHp = maxHp;
        maxStamina = 100f;
        currentStamina = maxStamina;
        isArmed = false;
    }

    public void DrainStamina(float value)
    {
        currentStamina -= value;
        currentStamina = Math.Clamp(currentStamina, 0, maxStamina);
        animator.SetFloat("Stamina", currentStamina);
    }

    public void StaminerRecovery(float value)
    {
        if (currentStamina >= maxStamina)
        {
            return;
        }
        currentStamina += value;
        currentStamina = Math.Clamp(currentStamina, 0, maxStamina);
        animator.SetFloat("Stamina", currentStamina);
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DoNotDisturb"))
        {
            return;
        }
        else
        {
            AnimatorControll(playerController.playerState);
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;

            rigidbody.MovePosition(transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
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
    public void Roll()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("DoNotDisturb"))
        {
            return;
        }
        AnimatorControll(playerController.playerState);

        Vector3 rollDirection = new Vector3(animator.transform.localRotation.x, 0, animator.transform.localRotation.z);
        rigidbody.velocity = rollDirection + animator.transform.forward * rollPower;
    }


    public void WeaponSwitch()
    {
        AnimatorControll(playerController.playerState);
        isArmed = !isArmed;
    }


    public override int Attack()
    {
        int value = 10;

        return value;
    }

    public override void Hit(int damage)
    {
        if (currentHp <= 0)
        {
            //Dead;
            return;
        }
        currentHp -= damage;

    }

    public void Heal(int healingAmount)
    {
        currentHp += healingAmount;
        currentHp = Math.Clamp(currentHp , 0 , maxHp);
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossAttack"))
        {
            knockback(transform.position, other.transform.position);
            StartCoroutine(GetHit());

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

}
