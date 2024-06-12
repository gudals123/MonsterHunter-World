using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class OldCatBT : MonoBehaviour
{
    [SerializeField] private CleverCrow.Fluid.BTs.Trees.BehaviorTree catTree;
    [SerializeField] private Transform catTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform bossTransform;
    //[SerializeField] private SphereCollider Detectcollider;
    //[SerializeField] private SphereCollider Attackcollider;
    [SerializeField] private Animator animator;
    //private Vector3 moveDirection;
    //[SerializeField] private Rigidbody catRigidbody;
    //[SerializeField] private float rotationSpeed;

    // private bool isBossInRange;
    // private bool isBossAttackInRange;
    // private bool isPlayerInRange;
    // private bool isPlayerInDetectRange;
    // private bool isPlayerAlmostDie;
    // private bool isAttack;
    // private bool isMove;
    // private Collider[] DetectCollider;

    private CatAction catAction;

    private void Awake()
    {
        catAction = GetComponent<CatAction>();

        catTree = new BehaviorTreeBuilder(gameObject)
            // 공격 트리
            .Selector()
                .Sequence()
                    .Condition("isBossAttackInRange", () => catAction._isBossInAttackRange)
                    .Do(() =>
                    {
                        // Debug.Log("Attack!");
                        animator.PlayInFixedTime("Attack");
                        catAction._isBossInAttackRange = false;
                        return TaskStatus.Success;
                    })
                .End()

                .Selector()
                    // 보스 감지 트리
                    .Sequence()
                        .Condition("isBossInRange", () => catAction._isBossInCatView)
                        .Do(() =>
                        {
                            // Debug.Log("Move To Boss");
                            animator.Play("Run");
                            catAction._isBossInCatView = false;
                            return TaskStatus.Success;
                        })
                    .End()

                    // 플레이어 트래킹 트리
                    .Sequence()
                        .Condition("isPlayerInView", () => catAction._isPlayerInCatView)
                        .Do(() =>
                        {
                            // Debug.Log("Move To Player");
                            animator.Play("Run");
                            catAction._isPlayerInCatView = false;
                            return TaskStatus.Success;
                        })
                    .End()

                    // 플레이어 감지 트리
                    .Sequence()
                        .Condition("isPlayerInRange", () => catAction._isPlayerInAttackRange)
                        .Do(() =>
                        {
                            // Debug.Log("Player Near");
                            animator.Play("Idle");
                            catAction._isPlayerInAttackRange = false;
                            return TaskStatus.Success;
                        })
                        .Sequence()// 플레이어 힐 트리
                            .Condition("isPlayerAlmostDie", () => catAction._isPlayerAlmostDie)
                            .Do(() =>
                            {
                                // Debug.Log("Heal To Player");
                                catAction.Heal();
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
        catAction.IsBossInRange(bossTransform, catTransform);
        catAction.FollowPlayer(playerTransform, catTransform);

        if (catAction.PlayerHPCheck())
        {
            catAction._isPlayerAlmostDie = true;
        }

        catTree.Tick();
    }
}