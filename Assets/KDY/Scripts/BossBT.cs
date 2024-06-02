using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;

public class BossBT : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    public BehaviorTree tree;
    public Transform player;
    private Vector3 moveDirection;

    public GameObject breath;

    private Animator animator;
    private Rigidbody bossRb;

    private bool rotationToPlayer = false;
    private bool startTracking = false;
    private Vector3 wayToGoPlayer;

    public bool canBreathAttack = false;
    
    // 프로토타입 진행을 위한 임시 변수
    public bool isBossGetHit = false;
    public bool isBossDead = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bossRb = GetComponent<Rigidbody>();
        bossRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

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
                        Debug.Log("BattleTracking");
                        return TaskStatus.Success;
                    })
                .End()

                // Right SubTree
                .Sequence()
                    .StateAction("NomalWalking", () => rotationToPlayer = false )
                    .Do("NomalWalking", () =>
                    {
                        Debug.Log("NomalWalking");
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
            if (isBossGetHit)   // 추후 CombatManager._bossGetHit로 변경 예정
            {
                animator.Play("Hit");
            }

            if (isBossDead)   // 추후 !CombatManager._isBossDead로 변경 예정
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
            Vector3 direction = (player.position - transform.position).normalized;
            moveDirection = new Vector3(direction.x, 0, direction.z); // y축 방향은 무시
            bossRb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 targetDirection = (player.position - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            bossRb.MoveRotation(rotation);
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