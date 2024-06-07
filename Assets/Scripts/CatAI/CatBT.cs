using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatBT : MonoBehaviour
{
    [SerializeField] private BehaviorTree catTree;
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;
    //[SerializeField] private SphereCollider Detectcollider;
    //[SerializeField] private SphereCollider Attackcollider;
    [SerializeField] private Animator animator;
    //private Vector3 moveDirection;
    //[SerializeField] private Rigidbody catRigidbody;
    //[SerializeField] private float rotationSpeed;

    private bool isBossInRange;
    private bool isBossAttackInRange;
    private bool isPlayerInRange;
    private bool isPlayerInDetectRange;
    private bool isPlayerAlmostDie;
    private bool isAttack;
    private bool isMove;
    private Collider[] DetectCollider;

    private void Awake()
    {
        catTree = new BehaviorTreeBuilder(gameObject)
            // 공격 트리
            .Selector()
                .Sequence()
                    .Condition("isBossAttackInRange", () => CatManager._isBossInAttackRange)
                    .Do(() =>
                    {
                        Debug.Log("Attack");
                        animator.PlayInFixedTime("Attack");
                        CatManager._isBossInAttackRange = false;
                        return TaskStatus.Success;
                    })
                .End()

                .Selector()
                    // 보스 감지 트리
                    .Sequence()
                        .Condition("isBossInRange", () => CatManager._isBossInCatView)
                        .Do(() =>
                        {
                            animator.Play("Run");
                            Debug.Log("Move To Boss");
                            CatManager._isBossInCatView = false;
                            return TaskStatus.Success;
                        })
                    .End()

                    // 플레이어 트래킹 트리
                    .Sequence()
                        .Condition("isPlayerInView", () => CatManager._isPlayerInCatView)
                        .Do(() =>
                        {
                            animator.Play("Idle");
                            Debug.Log("Move To Player");
                            CatManager._isPlayerInCatView = false;
                            return TaskStatus.Success;
                        })
                    .End()

                    // 플레이어 감지 트리
                    .Sequence()
                        .Condition("isPlayerInRange", () => CatManager._isPlayerInAttackRange)
                        .Do(() =>
                        {
                            animator.Play("Run");
                            Debug.Log("Player Near");
                            CatManager._isPlayerInAttackRange = false;
                            return TaskStatus.Success;
                        })

                        // 플레이어 힐 트리
                        .Sequence()
                            .Condition("isPlayerAlmostDie", () => CatManager.isPlayerAlmostDie)
                            .Do(() =>
                            {
                                Debug.Log("Heal To Player");
                                animator.Play("Attack");
                                CatManager.instance.PlayerHP();
                                CatManager.isPlayerAlmostDie = false;
                                return TaskStatus.Success;
                            })
                        .End()
                    .End()
                .End()
            .End()
            .Build();
    }

    private void Update()
    {
        CatManager.instance.IsBossInRange(boss, gameObject.transform);
        CatManager.instance.FollowPlayer(player, gameObject.transform);

        catTree.Tick();
    }

}