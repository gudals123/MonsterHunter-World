using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
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



    private Vector3 moveDirection;
    [SerializeField] private Rigidbody catRigidbody;
    [SerializeField] private float rotationSpeed;

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
                            Debug.Log("Move To Boss");
                            LookAtBoss();
                            CatManager._isBossInCatView = false;
                            return TaskStatus.Success;
                        })
                    .End()

                     // 플레이어 감지 트리
                    .Sequence()
                        .Condition("isPlayerInRange", () => isPlayerInRange)
                        .Do(() =>
                        {
                            Debug.Log("Move To Player");
                            LookAtPlayer();
                            isPlayerInRange = false;
                            return TaskStatus.Success;
                        })
                        .Sequence()
                            .Condition("isPlayerAlmostDie", () => isPlayerAlmostDie)
                            .Do(() =>
                            {
                                Debug.Log("Heal To Player");
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
        CatManager.IsBossInRange(boss, gameObject.transform);
        CatManager.FollowPlayer(player, gameObject.transform);

        catTree.Tick();
    }

}