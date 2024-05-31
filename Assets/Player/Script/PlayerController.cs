using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    private float moveSpeed;
    private float walkSpeed = 2f;
    private float RunSpeed = 4f;

    [Header("Component")]
    private Rigidbody _rigidbody;
    private Animator _animator;


    [Header("Behaviour bool")]
    [SerializeField]private bool isMove = false;
    [SerializeField] private bool isAvoidance = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isRun = false;
    [SerializeField] private bool isUnarmed = true;
    [SerializeField] private bool isAttack = false;
    [SerializeField] private bool isHit = false;
    [SerializeField] private bool isMouseLeft = false;
    [SerializeField] private bool isDead = false;

    private float chargingDurationTime = 0;
    private int comboCount = 0;

    [Header("Object")]
    [SerializeField]private GameObject _weapon;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _weapon.SetActive(false);


    }

    private void FixedUpdate()
    {
        if(isDead)
        {
            return;
        }

        if (!isHit)
        {
            Move();
        }
        
    }

    void Update()
    {
        DeadCheck();
        ArmCheck();
    }

    private void ArmCheck()
    {
        if (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            return;
        }
        if (isUnarmed && (isHit || Input.GetMouseButtonDown(0)))
        {
            isUnarmed = false;
            isAttack = true;
            _weapon.SetActive(true);
        }
        if(!isUnarmed && Input.GetKeyDown(KeyCode.LeftShift))
        {
            isUnarmed = true;
            isAttack = false;
            _weapon.SetActive(false);
        }
        

    }

    private void DeadCheck()
    {
        if(BattleManager.Instance._currentPlayerHP < 0)
        {
            isDead = true;
        }
    }


    private void Move()
    {
        if (isMove)
        {
            RunCheck();
        }

        DoAvoidance();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        if (horizontal == 0 && vertical == 0)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }

/*        Vector3 direction = new Vector3(horizontal, 0, vertical);

        Vector3 newVelocity = direction * moveSpeed * Time.deltaTime;

        _rigidbody.velocity = newVelocity;  */  
    }

    private void RunCheck()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRun = true;
            moveSpeed = RunSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            moveSpeed = walkSpeed;
        }

    }
    
    private void DoAvoidance()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isAvoidance  = true;    
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isAvoidance = false;
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.transform.CompareTag("Monster"))
        {
            isHit = true;
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
            isHit = false;
        }
    }




}
