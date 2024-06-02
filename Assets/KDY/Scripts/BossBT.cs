using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;

public class BossBT : MonoBehaviour
{
    public float movementSpeed;
    public BehaviorTree tree;
    public Transform player;

    [SerializeField] Rigidbody bossRb;
    public GameObject breath;

    public Animator animator;

    public bool rotationToPlayer = false;
    public bool startTracking = false;
    public bool canBreathAttack = false;
    Vector3 wayToGoPlayer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bossRb = GetComponent<Rigidbody>();

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                // Left SubTree
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => CombatManager._isPlayerInRange)
                        .Selector()
                            .Sequence()
                                .Condition("canBreathAttack", () => canBreathAttack = SetBreathChance())
                                .StateAction("BreathAttack", () => { breath.SetActive(true); })
                                .Do(() =>
                                {
                                    CombatManager._instance.StartBreathAttack();
                                    breath.SetActive(false);
                                    Debug.Log("Breath Attack");
                                    canBreathAttack = !canBreathAttack;
                                    return TaskStatus.Success;
                                })
                            .End()
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

    }

    private void Start() { ActivateAi(); }

    IEnumerator ActivateAiCo()
    {
        while (true)
        {
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

    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }


    private void TrackingPlayer()
    {
        if (!CombatManager._isPlayerInRange)
        {
            wayToGoPlayer = (player.transform.position - transform.position).normalized;
            bossRb.velocity = wayToGoPlayer * movementSpeed;
        }
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

    public bool SetBreathChance()
    {
        float ran = UnityEngine.Random.value;
        Debug.Log(ran);
        if(ran <= 0.5) return canBreathAttack = true;   // 추후 boss HP 가 30% 이하일 조건 추가 : CombatManager._currentBossHP <= 600 && 
        else return canBreathAttack = false;
    }



    /*    public void BossSound(string name)
        {
            SoundManager.Instance.Play(SoundType.Effect, name);
        }*/
}