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

    // ?๋กํ ???? ์งํ? ?? ?? ๋ณ??
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
            if (isBossGetHit)   // ์ถํ CombatManager._bossGetHit๋ก? ๋ณ?๊ฒ? ?? 
            {
                BossBeingShot("Hit");
                //CombatManager.Instance._isbossGetHit = false;
                CombatManager.Instance._isBossRecognizedPlayer = true;   // ??๋ก? ?ฃ?ด ? . ์ถํ ?? ?ด?ด???? ??ธ??ฉ?? ? ๊ฑ? ?? 
                _detectedPlayer = true;
            }

            if (_isBossSturned)   // ์ถํ CombatManager._isBossSturned๋ก? ๋ณ?๊ฒ? ?? 
            {
                BossBeingShot("Sturn");
                _isBossSturned = false;
                //CombatManager.Instance._isBossSturned = false;
            }

            if (isBossDead)   // ์ถํ !CombatManager._isBossDead๋ก? ๋ณ?๊ฒ? ?? 
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
    /// ๋ณด์ค๊ฐ? ???๊ฒ์๊ฒ๋ก ?ฅ?ฉ??ค.
    /// </summary>
    /// <param name="targetPos"> ???๊ฒ? ???? </param>
    /// <param name="speedRate"> ?? ๋น์จ ์กฐ์ ; Walk ?ผ?? 0.7, Tracking ?ผ?? 1 </param>
    private void MovingWalkOrTracking(Vector3 targetPos, float speedRate)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        _moveDirection = new Vector3(direction.x, 0, direction.z); // y์ถ? ๋ฐฉํฅ??? ๋ฌด์
        _bossRb.MovePosition(transform.position + _moveDirection * _moveSpeed * speedRate * Time.fixedDeltaTime);
    }

    /// <summary>
    /// ๋ณด์ค๊ฐ? ???๊ฒ์๊ฒ๋ก ๋ชธ์ ?? ?ฉ??ค.
    /// </summary>
    public void RotationToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _bossRb.MoveRotation(rotation);
    }

    /// <summary>
    /// ๋ณด์ค๊ฐ? ๋ธ๋ ?ค ๊ณต๊ฒฉ? ?  ์ง? boss HP ๊ฐ? 30% ?ด??ผ ? 50% ?๋ฅ ๋ก ? ?ด์ค๋?ค.
    /// </summary>
    /// <returns> true?ด๋ฉ? BreathAttack, false?ด๋ฉ? NomalAttack </returns>
    public bool SetBreathChance()
    {
        float ran = Random.value;
        if (ran <= 0.5)   // ์ถํ boss HP ๊ฐ? 30% ?ด??ผ ์กฐ๊ฑด ์ถ๊?? : CombatManager._currentBossHP <= 600 && 
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
    /// ๋ณด์ค๊ฐ? Nomal ??ฉ?? ๊ฑธ์ ์ง?? ?????ฌ 50% ?๋ฅ ๋ก ? ?ด์ค๋?ค.
    /// </summary>
    /// <returns> true?ด๋ฉ? NomalWalking, false?ด๋ฉ? IDLE </returns>
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
    /// ๋ณด์ค๊ฐ? Nomal???? ???์ง์ธ?ค๋ฉ? ??ค ๋ฐฉํฅ? ๊ฒฐ์ ?ฉ??ค.
    /// </summary>
    /// <returns>??ค ๋ฐฉํฅ</returns>
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
        //CombatManager.Instance nullถ฿ดย นฎมฆภึภฝ
        if(CombatManager.Instance != null)
        {
            // ?? ?ด?ด๊ฐ? ๋ฒ์ ?ด? ?? ? ๋นจ๊ฐ??ผ๋ก?, ??๋ฉ? ?น??ผ๋ก? ๋ฒ์๋ฅ? ??
            Gizmos.color = CombatManager.Instance._bossAttackRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, 9f);

            // ๋ณด์ค? ??ผ ๋ฒ์๋ฅ? ??????ผ๋ก? ??
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, 18f);

            // ๋ณด์ค? ? ๋ฐ? ๋ฐฉํฅ ??
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 18f);
        }

    }

    /// <summary>
    /// ?ด? ? ? ?๋ฉ์ด? ?? ?ธ๋ถ?์ง๋ ? ?๋ฉ์ด?? ์ถ๋ ฅ?  ? ?? ์ฝ๋ฃจ?ด ?ค? ?จ????ค.
    /// Hit? ๊ฒฝ์ฐ 0.8์ด๊?? ?๋ง์ต??ค.
    /// </summary>
    /// <param name="time">?ด?  ? ?๋ฉ์ด??ด ?ค??๊ณ? ?? ?๊ฐ?</param>
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