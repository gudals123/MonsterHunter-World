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
    public CatState catState;

    //private AIController catTree;
    [SerializeField] public Transform player;
    [SerializeField] public Transform boss;
    [SerializeField] public Transform target;

    public bool isPlayer = true;
    public bool isBoss;

    [Header("Range Info")]
    //[SerializeField] private Collider collider;
    public float detectRange;
    public float interactionRange;
    public Vector3 dir;

    private void Start()
    {
        catState = CatState.Tracking;
        detectRange = 8f;
        interactionRange = 1.5f;
        target = player;
    }

    public Vector3 Detect(Vector3 targetPos)
    {
        Vector3 direction = (transform.position - targetPos)/*.normalized*/;

        return direction;
    }

    public void PlayerTracking(Transform target) // 상태만 변경
    {
        //// 보스가 감지범위 내에 있을 때
        //if (target.CompareTag("Boss") && dir.magnitude <= detectRange /*&& dir.magnitude > interactionRange*/)
        //{
        //    catState = CatState.Detect;
        //    isPlayer = false;
        //    isBoss = true;
        //    //target = boss;
        //}
        //// 보스가 상호작용범위 내에 있을 때
        //if (target.CompareTag("Boss") && dir.magnitude <= interactionRange)
        //{
        //    catState = CatState.Attack;
        //    isPlayer = false;
        //    isBoss = true;
        //    //target = boss;
        //}
        // 플레이어가 감지범위 내에 있을 때
        if (target.CompareTag("Player") /*&& dir.magnitude <= detectRange*//*&& dir.magnitude > interactionRange*/)
        {
            catState = CatState.Detect;
            isPlayer = true;
            //target = player;
        }
        // 플레이어가 상호작용범위 내에 있을 때
        if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
        {
            catState = CatState.Idle;
        }
        else
        {
            this.target = player;
            catState = CatState.Detect;
        }
        // else 플레이어 트래킹
        //if (target == null)
        //{
        //    target = player;
        //    isPlayer = true;
        //    catState = CatState.PlayerTracking;
        //}
    }

    private void Update()
    {
        dir = Detect(target.position);
        PlayerTracking(target);
        Debug.Log(dir.magnitude);
        Debug.Log(target.tag);
        Debug.Log(catState);
        //OnTriggerEnter(collider);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Plane")
        {
            return;
        }
        target = other.transform;
        Debug.Log($"trigger : {target.tag}");
    }
}