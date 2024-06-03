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
    public float rotationSpeed = 720; // �ʴ� ȸ�� �ӵ� (����)
    public float dodgeSpeed = 10f;

    [Header("Component")]
    private Rigidbody _rigidbody;
    private Animator _animator;
    [SerializeField] private Transform _modle;
    [SerializeField]private Transform cameraArm;
    private Camera _mainCamera;


    [Header("Behaviour bool")]
    private bool isMoveing = false;
    private bool isWalk = false;
    private bool isRun = false;
    private bool isDodge = false;
    private bool isGetHit = false;
    private bool isDead = false;
    private bool isGrounded = false;
    private bool isFall = false;
    private bool isArmed = false;
    private bool isAttacking = false;
    private bool isSwitchDone = true;




    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = _modle.GetComponent<Animator>();
        _mainCamera = Camera.main;  
        moveSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {
        if(isDead)
        {
            return;
        }

    }

    void Update()
    {
        DeadCheck();
        HandleInput();
        AnimatorControll();
        Move();
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
        else
        {
            isMoveing = true;
            isWalk = true;

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            _modle.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * 5f;

        }
    }
    /*private void Move()
    {
        Vector2 moverInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));



        Vector3 forward = _mainCamera.transform.forward;
        Vector3 right = _mainCamera.transform.right;


        //Vector3 movement = new Vector3(moveHorizontal * forward.x, 0.0f, moveVertical * right.z);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement == Vector3.zero)
        {
            isMoveing = false;
            isWalk = false;
            return;
        }
        else
        {
            isMoveing = true;
            isWalk = true;

            //movement = vec.normalized;
            _rigidbody.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(movement);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 
                rotationSpeed * Time.fixedDeltaTime));
            *//*
             * 
                        Quaternion targetRotation = Quaternion.LookRotation(movement);

                        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                        _modle.rotation = newRotation;
            *//*
            //Quaternion targetRotation = Quaternion.LookRotation(vec);
            //_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));


            *//*            Vector3 forward = _mainCamera.transform.forward;
                        forward.y = 0f; // y���� ȸ���� ������� �����Ƿ� 0���� ����

                        Quaternion targetRotation = Quaternion.LookRotation(forward); // ��ǥ ȸ�� ����

                        // ���� ���⿡�� ��ǥ ������� ȸ���ϱ�
                        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                        // ĳ������ ȸ�� ����
                        transform.rotation = newRotation;*//*
        }
    } */


    private void HandleInput()
    {
        //ȸ��
        if (Input.GetKeyDown(KeyCode.Space) && !isDodge)
        {
            isDodge = true;
            StartCoroutine(Dodge());
        }

        //���� ����Ī
        if ((isArmed && Input.GetKeyDown(KeyCode.LeftShift)) ||
            (!isArmed && Input.GetMouseButtonDown(0)))
        {
            isArmed = !isArmed;
        }

        //�޸���
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
        float rollTime = 0.7f; // �ձ����� �ð�
        float timer = 0f;

        while (timer < rollTime)
        {
            // ĳ���͸� ������ �̵���ŵ�ϴ�.
            transform.Translate(Vector3.forward * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // �ձ����Ⱑ ������ ��
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
