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
    private Animator animator;

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
                            //LookAtBoss();
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
                            //LookAtPlayer();
                            animator.Play("Idle");
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

    //private bool BossDetected()
    //{
    //    DetectCollider = Physics.OverlapSphere(transform.position, 4f);

    //    if (DetectCollider != null && DetectCollider.Length > 0)
    //    {
    //        player = DetectCollider[0].transform;

    //        return false;
    //    }
    //    return true;
    //}

    //private void OnDrawGizmos()

    //{
    //    DetectCollider = Physics.OverlapSphere(transform.position, 4f);
    //    Gizmos.DrawWireSphere(transform.position, 4f);

    //    for (int i = 0; i < DetectCollider.Length; ++i)
    //    {
    //        Gizmos.color = Color.white;
    //        if (DetectCollider[i].name == "Boss")
    //        {
    //            Gizmos.color = Color.red;
    //        }

    //        if (DetectCollider[i].name == "Player")
    //        {
    //            Gizmos.color = Color.green;
    //        }
    //    }

    //    var Attackcollider = Physics.OverlapSphere(transform.position, 1.5f);
    //    Gizmos.DrawWireSphere(transform.position, 1.5f);
    //    Gizmos.color = Color.blue;

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
        CatManager.IsBossInRange(boss, gameObject.transform);
        CatManager.IsPlayerInRange(player, gameObject.transform);

        catTree.Tick();
    }

    //public void LookAtPlayer()
    //{
    //    transform.LookAt(player);

    //    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
    //    Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    //    catRigidbody.MoveRotation(rotation);

    //    Vector3 targetDirection = (player.position - transform.position).normalized;

    //    if (targetDirection != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
    //        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    //        catRigidbody.MoveRotation(rotation);
    //    }
    //}

    //public void LookAtBoss()
    //{
    //    transform.LookAt(boss);

    //    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
    //    Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    //    catRigidbody.MoveRotation(rotation);

    //    Vector3 targetDirection = (boss.position - transform.position).normalized;

    //    if (targetDirection != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
    //        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    //        catRigidbody.MoveRotation(rotation);
    //    }
    //}
}