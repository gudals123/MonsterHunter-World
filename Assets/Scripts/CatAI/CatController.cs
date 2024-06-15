using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Unity.VisualScripting;

public class CatController : AIController
{
    public enum CatState
    {
        Idle,
        Move,
        Hit,
        Dead,
        Detect,
        Tracking,
        Attack,
        Skill
    }

    private Transform attackPriority;
    private Cat cat;
    public CatState catState;

    [SerializeField] public Transform player;
    [SerializeField] public Transform boss;
    [SerializeField] public Transform target;

    public bool isPlayer = true;
    public bool isBoss;

    [Header("Range Info")]
    public float detectRange;
    public float interactionRange;
    public Vector3 dir;

    private void Start()
    {
        cat = GetComponent<Cat>();
        catState = CatState.Tracking;
        detectRange = 8f;
        interactionRange = 1.5f;
    }

    public void Hit()
    {
        catState = CatState.Hit;
        cat.Hit(10);
        if (cat.currentHP <= 0)
        {
            catState = CatState.Dead;
        }
    }

    public Vector3 Detect(Vector3 targetPos)
    {
        Vector3 direction = (transform.position - targetPos);

        return direction;
    }

    public void Tracking(Transform target) // ���¸� ����
    {
        // �÷��̾ �������� ���� ���� ��
        if (target.CompareTag("Player"))
        {
            catState = CatState.Detect;
            isPlayer = true;
        }
        // �÷��̾ ��ȣ�ۿ���� ���� ���� ��
        if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
        {
            catState = CatState.Idle;
        }
        else
        {
            this.target = player;
            catState = CatState.Detect;
        }
    }

    private void Update()
    {
        dir = Detect(target.position);
        Tracking(target);
        Debug.Log(dir.magnitude);
        Debug.Log(target.tag);
        Debug.Log(catState);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Plane")
        {
            return;
        }

        if (other.CompareTag("BossAttack"))
        {
            catState = CatState.Hit;
        }
        else
        {
            target = other.transform;
            Debug.Log($"trigger : {target.tag}");
        }
    }
}