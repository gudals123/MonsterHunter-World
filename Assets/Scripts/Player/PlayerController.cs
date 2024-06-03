using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Speed/Force")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float RunSpeed = 4f;
    private float rollForce = 10f; 
    private float rollDuration = 0.5f;

    [Header("Component")]
    private Rigidbody _rigidbody;
    private Animator _animator;


    [Header("Behaviour bool")]
    private bool isMoveing = false;
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGetHit = false;
    private bool isDead = false;
    private bool isGrounded = false;
    private bool isFall = false;
    private bool isArmed = false;
    private bool isAttacking = false;
    private bool isSwitchDone = true;
    private bool isRoll = false;
    private bool isRolling = false;
    

    [SerializeField] private Transform _characterBody;
    [SerializeField] private Transform _cameraArm;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    private void Start()
    {
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
        AnimatorControll();
        LookAround();
    }

    private void DeadCheck()
    {
        if (BattleManager.Instance._currentPlayerHP <= 0)
        {
            isDead = true;
        }
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (moveInput == Vector2.zero)
        {
            isMoveing = false;
            isWalk = false;
            return;
        }
        else if(isSwitchDone)
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
        //회피
        if (Input.GetKeyDown(KeyCode.Space) && !isRoll)
        {
            isRoll = true;

        }

        //무기 스위칭
        if ((isArmed && Input.GetKeyDown(KeyCode.LeftShift)) ||
            (!isArmed && Input.GetMouseButtonDown(0)))
        {
            isArmed = !isArmed;
        }

        //달리기
        if (!isArmed && Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = RunSpeed;
            isRun = true;
        }
        else if (!isArmed && Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
            isRun = false;
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.transform.CompareTag("Monster"))
        {
            isGetHit = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.transform.CompareTag("Monster"))
        {
            isGetHit = false;
        }
    }




}
