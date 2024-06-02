using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float RunSpeed = 4f;
    public float rotationSpeed = 360f; // 초당 회전 속도 (각도)
    public float dodgeSpeed = 10f;

    [Header("Component")]
    private Rigidbody _rigidbody;
    private Animator _animator;
    [SerializeField] private Transform _modle;


    [Header("Behaviour bool")]
    [SerializeField]private bool isMoveing = false;
    [SerializeField]private bool isWalk = false;
    [SerializeField]private bool isRun = false;
    [SerializeField]private bool isDodge = false;
    [SerializeField]private bool isGetHit = false;
    [SerializeField]private bool isDead = false;
    [SerializeField]private bool isGrounded = false;
    [SerializeField]private bool isFall = false;
    [SerializeField]private bool isArmed = false;
    [SerializeField]private bool isAttacking = false;
    private bool isSwitchDone = true;





    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = _modle.GetComponent<Animator>();

        moveSpeed = walkSpeed;
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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if(movement == Vector3.zero)
        {
            isMoveing = false;
            isWalk = false;
            return;
        }
        else
        {
            isMoveing = true;
            isWalk = true;

            _rigidbody.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
            
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    } 


    private void HandleInput()
    {
        //회피
        if (Input.GetKeyDown(KeyCode.Space) && !isDodge)
        {
            isDodge = true;
            StartCoroutine(Dodge());
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


    IEnumerator Dodge()
    {
        float rollTime = 0.7f; // 앞구르기 시간
        float timer = 0f;

        while (timer < rollTime)
        {
            // 캐릭터를 앞으로 이동시킵니다.
            transform.Translate(Vector3.forward * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // 앞구르기가 끝났을 때
        isDodge = false;
    }

    private void AnimatorControll()
    {
        _animator.SetBool(PlayerAnimatorParamiter.IsDead, isDead);
        _animator.SetBool(PlayerAnimatorParamiter.IsMoveing, isMoveing);
        _animator.SetBool(PlayerAnimatorParamiter.IsWalk, isWalk);
        _animator.SetBool(PlayerAnimatorParamiter.IsDead, isDead);
        _animator.SetBool(PlayerAnimatorParamiter.IsArmed, isArmed);
        _animator.SetBool(PlayerAnimatorParamiter.IsDodge, isDodge);
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
