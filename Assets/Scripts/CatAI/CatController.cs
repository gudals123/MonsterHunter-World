using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

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

    private int attackPriority;
    public CatState catState;

    //private AIController catTree;
    [SerializeField] public Transform player;
    [SerializeField] public Transform boss;
    [SerializeField] public Transform target;

    public bool isPlayer = true;
    public bool isBoss;

    [Header("Range Info")]
    private float detectRange;
    private float interactionRange;

    public Vector3 Detect(Vector3 targetPos)
    {
        Vector3 direction = (transform.position - targetPos)/*.normalized*/;

        return direction;
    }

    public void Tracking(Transform target)
    {
        Vector3 dir = Detect(target.position);

        // 보스가 감지범위 내에 있을 때
        if (target.CompareTag("Boss") && dir.magnitude <= detectRange)
        {
            Debug.Log("Boss, dir.magnitude <= detectRange");
            transform.position += dir;
        }
        // 보스가 상호작용범위 내에 있을 때
        if (target.CompareTag("Boss") && dir.magnitude <= interactionRange)
        {
            Debug.Log("Boss, dir.magnitude <= interactionRange");
        }
        // 플레이어가 상호작용범위 내에 있을 때
        if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
        {
            Debug.Log("Player, dir.magnitude <= interactionRange");
        }
        // else 플레이어 트래킹
        else
        {
            Debug.Log("else Player");
            target = player;
        }
    }
}