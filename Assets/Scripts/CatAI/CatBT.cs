using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBT : MonoBehaviour
{
    [SerializeField] private BehaviorTree catTree;

    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody boss;
    [SerializeField] private SphereCollider Detectcollider;
    [SerializeField] private SphereCollider Attackcollider;
    private Animator animator;

    private bool isBossInRange;
    private bool isBossAttackInRange;
    private bool isPlayerInRange;
    private bool isPlayerInDetectRange;
    private bool isPlayerAlmostDie;
    private bool isAttack;
    private bool isMove;

    private void Awake()
    {
        catTree = new BehaviorTreeBuilder(gameObject)
            // 공격 트리
            .Sequence()
                .Sequence()
                    .Condition("isBossAttackInRange", () => isBossAttackInRange)
                    .Do(() =>
                    {
                        Debug.Log("Attack");
                        isBossAttackInRange = false;
                        return TaskStatus.Success;
                    })
                .End()
            .End()

            // 보스 감지 트리
            .Selector()
                .Sequence()
                    .Condition("isBossInRange", () => BossDetected())
                    .Do(() =>
                    {
                        Debug.Log("Move To Boss");
                        isBossInRange = false;
                        return TaskStatus.Success;
                    })
                .End()

                // 플레이어 감지 트리
                .Sequence()
                    .Condition("isPlayerInRange", () => isPlayerInRange)
                    .Do(() =>
                    {
                        Debug.Log("Move To Player");
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
            .Build();
    }

    private bool BossDetected()
    {
        var DetectCollider = Physics.OverlapSphere(transform.position, 3f);

        if (DetectCollider != null && DetectCollider.Length > 0)
        {
            player = DetectCollider[0].transform;

            return false;
        }
        return true;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, 3f);

    //    for (int i = 0; i < DetectCollider.Length; ++i)
    //    {
    //        if (DetectCollider[i].name == "Boss")
    //        {
    //            isBossInRange = true;
    //        }

    //        if (DetectCollider[i].name == "Player")
    //        {
    //            isPlayerInRange = true;
    //        }
    //    }

    //    var Attackcollider = Physics.OverlapSphere(transform.position, 1.5f);
    //    Gizmos.DrawWireSphere(transform.position, 1.5f);

    //    for (int i = 0; i < Attackcollider.Length; ++i)
    //    {
    //        if (Attackcollider[i].name == "Boss")
    //        {
    //            Debug.Log($"{Attackcollider[i].name}");
    //            isBossInRange = false;
    //            isBossAttackInRange = true;
    //        }

    //        if (Attackcollider[i].name == "Player")
    //        {
    //            Debug.Log($"{Attackcollider[i].name}");
    //            isPlayerInRange = false;
    //            isPlayerInDetectRange = true;
    //        }
    //    }
    //}

    private void Update()
    {
        catTree.Tick();
    }

}
