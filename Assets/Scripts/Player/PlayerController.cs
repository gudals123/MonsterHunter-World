using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Speed / Fower")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    private float knockbackPower = 2.5f;

    [Header("Component")]
    private Rigidbody _rigidbody;
    private Animator _animator;
    

    [Header("State bool")]
    private bool isMoveing = false;
    private bool isWalk = false;
    private bool isRun = false;
    private bool isDead = false;
    private bool isGrounded = false;
    private bool isArmed = false;
    private bool isRoll = false;
    private bool isAttacking = false;

    private bool isGetHit = false;


    [Header("Object")]
    [SerializeField] private Transform _characterBody;
    [SerializeField] private Transform _cameraArm;

    [Header("GroundCheck")]
    [SerializeField] private float groundcheckDistance;
    [SerializeField] protected LayerMask whatIsGround;




    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
        _animator = _characterBody.GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        if(isDead)
        {
            return;
        }

        Move();
    }

    void Update()
    {
        DeadCheck();
        HandleInput();
        GroundCheck();
        AnimatorControll();
        LookAround();

        
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, Vector2.down, groundcheckDistance, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void DeadCheck()
    {
        if (CombatManager.Instance._currentPlayerHP <= 0f)
        {
            isDead = true;
        }
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Don'tMove"))
        {
            return;
        }

        if (moveInput == Vector2.zero)
        {
            isMoveing = false;
            isWalk = false;
            return;
        }
        else
        {
            isMoveing = true;
            isWalk = true;

            Vector3 lookForward = new Vector3(_cameraArm.forward.x, 0f, _cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(_cameraArm.right.x, 0f, _cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            _characterBody.forward = moveDir;

            _rigidbody.MovePosition(transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = _cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335, 361);
        }

        _cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
   

    private void HandleInput()
    {
        //������ ����
        if (Input.GetKeyDown(KeyCode.Space) && !isRoll)
        {
            isRoll = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isRoll = false;
        }

        //���� ����Ī ����
        if ((isArmed && Input.GetKeyDown(KeyCode.LeftShift)) ||
            (!isArmed && Input.GetMouseButtonDown(0)))
        {
            isArmed = !isArmed;
        }

        //�޸��� ����
        if (!isArmed && Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
            isRun = true;
        }
        else if (!isArmed && Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
            isRun = false;
        }

        //���� ����
        if (isArmed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CombatManager.Instance._isRightAttak = false;
                isAttacking = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isAttacking = false;
            }
            if (!Input.GetMouseButton(0))
            {
                CombatManager.Instance._isCharging = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                CombatManager.Instance._isRightAttak = true;
                isAttacking = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isAttacking = false;
            }
        }

    }

    private void AnimatorControll()
    {
        _animator.SetBool(PlayerAnimatorParamiter.IsDead, isDead);
        _animator.SetBool(PlayerAnimatorParamiter.IsMoveing, isMoveing);
        _animator.SetBool(PlayerAnimatorParamiter.IsWalk, isWalk);
        _animator.SetBool(PlayerAnimatorParamiter.IsDead, isDead);
        _animator.SetBool(PlayerAnimatorParamiter.IsArmed, isArmed);
        _animator.SetBool(PlayerAnimatorParamiter.IsRoll, isRoll);
        _animator.SetBool(PlayerAnimatorParamiter.IsRun, isRun);
        _animator.SetBool(PlayerAnimatorParamiter.IsGetHit, isGetHit);
        _animator.SetBool(PlayerAnimatorParamiter.IsGrounded, isGrounded);
        _animator.SetBool(PlayerAnimatorParamiter.IsAttacking, isAttacking);
        _animator.SetBool(PlayerAnimatorParamiter.IsRightAttak, CombatManager.Instance._isRightAttak);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BossAttack"))
        {
            knockback(transform.position, other.transform.position);
            StartCoroutine(GetHit());
        }
    }

    private void knockback(Vector3 playerPos, Vector3 attackColliderPos)
    {
        Vector3 direction = playerPos - attackColliderPos;
        direction = new Vector3(direction.x, 0 , direction.z);
        direction.Normalize();
        _rigidbody.AddForce(direction * knockbackPower, ForceMode.Impulse);
    }

    private IEnumerator GetHit()
    {
        isGetHit = true;
        yield return new WaitForSeconds( 0.2f); 
        isGetHit = false;
    }


}
