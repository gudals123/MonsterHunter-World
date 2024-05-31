using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using System;

public class BossBT : MonoBehaviour
{
    public float movementSpeed;
    public BehaviorTree tree;
    public Transform player;

    [SerializeField] Rigidbody bossRb;

    public Animator animator;

    public bool rotationToPlayer = false;
    public bool startTracking = false;
    Vector3 wayToGoPlayer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bossRb = GetComponent<Rigidbody>();

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
            // Left SubTree_NormalAttack
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => CombatManager._isPlayerInRange)
                    .StateAction("NomalAttack", () =>
                    {
                        bossRb.velocity = Vector3.zero;
                        rotationToPlayer = false;
                    })
                    .Do(() =>
                    {
                        Debug.Log("NomalAttack");
                        return TaskStatus.Success;
                    })
                .End()

                // Middle SubTree
                .Sequence()
                    .Condition("detectedPlayer", () => CombatManager._isPlayerInBossView )
                    .StateAction("BattleTracking", () =>
                    {
                        rotationToPlayer = true;
                        startTracking = true;
                    })
                    .Do("TrackingPlayer", () =>
                    {
                        // 시야 범위 안이면서 공격 범위 밖일 때 플레이어로 향하는 코드
                        Debug.Log("BattleTracking");
                        return TaskStatus.Success;
                    })
                .End()

                // Right SubTree
                .Sequence()
                    .StateAction("NomalWalking", () => rotationToPlayer = false )
                    .Do("IdleWalking", () =>
                    {
                        // Nomal 배회하는 코드
                        return TaskStatus.Success;
                    })
                .End()
            .End()
            .Build();
    }

    private void Update()  
    {
        CombatManager.isPlayerInRange(player, gameObject.transform);
        wayToGoPlayer.y = 0;
        if (rotationToPlayer)
        {
            LookAtPlayer();
        }

        if (startTracking)
        {
            TrackingPlayer();
        }


        /*        if (CombatManager._checkParrying)
                {
                    BossParrying();
                }
        */
    }

    private void TrackingPlayer()
    {
        if (!CombatManager._isPlayerInRange)
        {
            wayToGoPlayer = (player.transform.position - transform.position).normalized;
            bossRb.velocity = wayToGoPlayer * movementSpeed;
        }
    }

    private void Start() { ActivateAi(); }

    IEnumerator ActivateAiCo()
    {
        while (true)
        {
            /*if (CombatManager._isIdle == false)
            {
                yield return null;
                continue;
            }*/

            if (!CombatManager._isBossDead)
            {
                animator.Play("Die");
                CombatManager._isBossDead = true;
            }

            else
            {
                tree.Tick();
            }

            yield return null;
        }
    }

/*    public void BreakDefense()
    {
        if (CombatManager._isForward && CombatManager._dist <= CombatManager._attackRange)
        {
            transform.LookAt(_player.position);
            _player.GetComponent<PlayerController>().isDamage = true;
            CombatManager.ConsumeStamina(CombatManager._currentPlayerST);
        }
    }
*/
/*    void BossParrying()
    {
        _animator.Play("Hit");
        _tree.RemoveActiveTask(_tree.Root);
    }
*/

    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }


    public void LookAtPlayer()
    {
        Vector3 targetDirection = (player.position - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7.5f * Time.deltaTime);
        }
    }



    /*    public void BossSound(string name)
        {
            SoundManager.Instance.Play(SoundType.Effect, name);
        }*/
}