using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class BossBT : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    public BehaviorTree _tree;
    public Transform _player;
    private Vector3 _moveDirection;

    private Animator _animator;
    private Rigidbody _bossRb;

    private bool _rotationToPlayer = false;
    private bool _startTracking = false;
    private bool _canBreathAttack = false;

    public GameObject _nomalAtt;
    public GameObject _breathAtt;

    // 프로토타입 진행을 위한 임시 변수
    public bool isBossGetHit = false;
    public bool isBossDead = false;
    public bool _detectedPlayer = false;
    public bool _trackingPlayer = false;
    public float _perceptionTime = 0;
    public bool _canNomalWalking;
    public Vector3 _randomPosToWalk;
    public bool _isBossSturned = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _bossRb = GetComponent<Rigidbody>();
        _bossRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                // Left SubTree
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => CombatManager.Instance._bossAttackRange)
                        .Selector()
                            .Sequence()
                                .Condition("canBreathAttack", () => _canBreathAttack = SetBreathChance())
                                .StateAction("BreathAttack", () =>
                                {
                                    _trackingPlayer = false;
                                    _breathAtt.SetActive(true);
                                })
                                .Do(() =>
                                {
                                    //CombatManager.Instance.StartBreathAttack();
                                    _breathAtt.SetActive(false);
                                    Debug.Log("Breath Attack");
                                    _canBreathAttack = !_canBreathAttack;
                                    return TaskStatus.Success;
                                })
                            .End()
                            .StateAction("NomalAttack", () =>
                            {
                                _trackingPlayer = false;
                            })
                            .Do(() =>
                            {
                                Debug.Log("Nomal Attack");
                                return TaskStatus.Success;
                            })
                        .End()
                .End()

                // Midle SubTree
                .Sequence()
                    .Condition("TrackingPlayer", () => _detectedPlayer)
                        .StateAction("BattleTracking", () =>
                        {
                            isBossGetHit = false;
                            _trackingPlayer = true;
                        })
                        .Do("TrackingPlayer", () =>
                        {
                            Debug.Log("TrackingPlayer");
                            return TaskStatus.Success;
                        })
                .End()

                // Right SubTree
                .Selector()
                    .Sequence()
                        .Condition("ChanceForWalking", () => SetNomalWalkingChance())
                        .StateAction("NomalWalking", () => 
                        {
                            _trackingPlayer = false;
                            _randomPosToWalk = RandomPosForWalking();
                            _canNomalWalking = true;
                        })
                        .Do("NomalWalking", () =>
                        {
                            Debug.Log("NomalWalking");
                            _canNomalWalking = false;
                            return TaskStatus.Success;
                        })
                    .End()
                    .StateAction("Idle", () =>
                    {
                        _trackingPlayer = false;
                        Debug.Log("Idle");
                    })
                    .Do(() =>
                    {
                        return TaskStatus.Success;
                    })
                .End()
            .End()
            .Build();
    }

    private void Update()
    {
        CombatManager.Instance.isPlayerInRange(_player, gameObject.transform);

        if (CombatManager.Instance._bossVisualRange || CombatManager.Instance._isBossRecognizedPlayer)
        {
            _detectedPlayer = true;
        }

        if (_trackingPlayer)
        {
            RotationToTarget();
            MovingWalkOrTracking(_player.position, 1f);

            if (!CombatManager.Instance._bossVisualRange || !CombatManager.Instance._bossPerceptionRange)
            {
                _perceptionTime += Time.deltaTime;
            }
        }

        if (_canNomalWalking)
        {
            BossNomalWalking(_randomPosToWalk);
        }

        if (_perceptionTime >= 3)
        {
            _detectedPlayer = false;
            CombatManager.Instance._isBossRecognizedPlayer = false;
            _perceptionTime = 0;
        }

    }

    private void Start() { ActivateAi(); }

    public void ActivateAi()
    {
        StartCoroutine(ActivateAiCo());
    }

    IEnumerator ActivateAiCo()
    {
        while (true)
        {
            if (isBossGetHit)   // 추후 CombatManager._bossGetHit로 변경 예정
            {
                BossBeingShot("Hit");
                //CombatManager.Instance._isbossGetHit = false;
                CombatManager.Instance._isBossRecognizedPlayer = true;   // 임시로 넣어 둠. 추후 플레이어와의 상호작용에서 제거 예정
                _detectedPlayer = true;
            }

            if (_isBossSturned)   // 추후 CombatManager._isBossSturned로 변경 예정
            {
                BossBeingShot("Sturn");
                _isBossSturned = false;
                //CombatManager.Instance._isBossSturned = false;
            }

            if (isBossDead)   // 추후 !CombatManager._isBossDead로 변경 예정
            {
                BossBeingShot("Die");
                Debug.Log($"{transform.GetChild(0).GetChild(0).name}");
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                //CombatManager.Instance._isBossDead = true;
            }

            else
            {
                _tree.Tick();
            }

            yield return null;
        }
    }

    private void BossBeingShot(string animationName)
    {
        _trackingPlayer = false;
        _canNomalWalking = false;

        _animator.Play(animationName);
    }

    /// <summary>
    /// 보스가 타겟에게로 향합니다.
    /// </summary>
    /// <param name="targetPos"> 타겟 대상 </param>
    /// <param name="speedRate"> 속도 비율 조정; Walk 일때는 0.7, Tracking 일때는 1 </param>
    private void MovingWalkOrTracking(Vector3 targetPos, float speedRate)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        _moveDirection = new Vector3(direction.x, 0, direction.z); // y축 방향은 무시
        _bossRb.MovePosition(transform.position + _moveDirection * _moveSpeed * speedRate * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 보스가 타겟에게로 몸을 회전합니다.
    /// </summary>
    public void RotationToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _bossRb.MoveRotation(rotation);
    }

    /// <summary>
    /// 보스가 브레스 공격을 할 지 boss HP 가 30% 이하일 때 50% 확률로 정해줍니다.
    /// </summary>
    /// <returns> true이면 BreathAttack, false이면 NomalAttack </returns>
    public bool SetBreathChance()
    {
        float ran = Random.value;
        if (ran <= 0.5)   // 추후 boss HP 가 30% 이하일 조건 추가 : CombatManager._currentBossHP <= 600 && 
        {
            Debug.Log($"{ran} <= 0.5 / Boss Breath Attack");
            return _canBreathAttack = true;
        }
        else
        {
            Debug.Log($"{ran} >= 0.5 / Boss Nomal Attack");
            return _canBreathAttack = false;
        }
    }

    /// <summary>
    /// 보스가 Nomal 상황에서 걸을 지에 대하여 50% 확률로 정해줍니다.
    /// </summary>
    /// <returns> true이면 NomalWalking, false이면 IDLE </returns>
    public bool SetNomalWalkingChance()
    {
        float ran = Random.value;
        if (ran <= 0.5)
        {
            Debug.Log($"{ran} <= 0.5 / Boss Walk");
            return true;
        }
        else
        {
            Debug.Log($"{ran} >= 0.5 / Boss IDLE");
            return false;
        }
    }

    /// <summary>
    /// 보스가 Nomal상태에서 움직인다면 랜덤 방향을 결정합니다.
    /// </summary>
    /// <returns>랜덤 방향</returns>
    public Vector3 RandomPosForWalking()
    {
        float targetX = Random.Range(transform.position.x - 100, transform.position.x + 100);
        float targetZ = Random.Range(transform.position.z - 100, transform.position.z + 100);
        return new Vector3(targetX, 0, targetZ);
    }

    public void BossNomalWalking(Vector3 targetPos)
    {
        MovingWalkOrTracking(targetPos, 0.7f);
        RotationToTarget();

        if (transform.position == targetPos)
        {
            _canNomalWalking = false;
        }

    }

    void OnDrawGizmos()
    {
        if (gameObject.transform != null)
        {
            // 플레이어가 범위 내에 있을 때 빨간색으로, 아니면 녹색으로 범위를 표시
            Gizmos.color = CombatManager.Instance._bossAttackRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, 9f);

            // 보스의 시야 범위를 파란색으로 표시
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, 18f);

            // 보스의 전방 방향 표시
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 18f);
        }
    }

    /// <summary>
    /// 어떠한 애니메이션 후에 울부짖는 애니메이션을 출력할 수 있는 코루틴 실행 함수입니다.
    /// Hit의 경우 0.8초가 알맞습니다.
    /// </summary>
    /// <param name="time">이전 애니메이션이 실행되고 있는 시간</param>
    private void Roaring(float time)
    {
        StartCoroutine(CoRoar(time));
    }

    private IEnumerator CoRoar(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.Play("Roar");
    }

    /*    public void BossSound(string name)
        {
            SoundManager.Instance.Play(SoundType.Effect, name);
        }*/
}