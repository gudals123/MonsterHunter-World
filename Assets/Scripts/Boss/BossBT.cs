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

    // ?”„ë¡œí† ????… ì§„í–‰?„ ?œ„?•œ ?„?‹œ ë³??ˆ˜
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
            if (isBossGetHit)   // ì¶”í›„ CombatManager._bossGetHitë¡? ë³?ê²? ?˜ˆ? •
            {
                BossBeingShot("Hit");
                //CombatManager.Instance._isbossGetHit = false;
                CombatManager.Instance._isBossRecognizedPlayer = true;   // ?„?‹œë¡? ?„£?–´ ?‘ . ì¶”í›„ ?”Œ? ˆ?´?–´????˜ ?ƒ?˜¸?‘?š©?—?„œ ? œê±? ?˜ˆ? •
                _detectedPlayer = true;
            }

            if (_isBossSturned)   // ì¶”í›„ CombatManager._isBossSturnedë¡? ë³?ê²? ?˜ˆ? •
            {
                BossBeingShot("Sturn");
                _isBossSturned = false;
                //CombatManager.Instance._isBossSturned = false;
            }

            if (isBossDead)   // ì¶”í›„ !CombatManager._isBossDeadë¡? ë³?ê²? ?˜ˆ? •
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
    /// ë³´ìŠ¤ê°? ???ê²Ÿì—ê²Œë¡œ ?–¥?•©?‹ˆ?‹¤.
    /// </summary>
    /// <param name="targetPos"> ???ê²? ????ƒ </param>
    /// <param name="speedRate"> ?†?„ ë¹„ìœ¨ ì¡°ì •; Walk ?¼?•Œ?Š” 0.7, Tracking ?¼?•Œ?Š” 1 </param>
    private void MovingWalkOrTracking(Vector3 targetPos, float speedRate)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        _moveDirection = new Vector3(direction.x, 0, direction.z); // yì¶? ë°©í–¥??? ë¬´ì‹œ
        _bossRb.MovePosition(transform.position + _moveDirection * _moveSpeed * speedRate * Time.fixedDeltaTime);
    }

    /// <summary>
    /// ë³´ìŠ¤ê°? ???ê²Ÿì—ê²Œë¡œ ëª¸ì„ ?šŒ? „?•©?‹ˆ?‹¤.
    /// </summary>
    public void RotationToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        _bossRb.MoveRotation(rotation);
    }

    /// <summary>
    /// ë³´ìŠ¤ê°? ë¸Œë ˆ?Š¤ ê³µê²©?„ ?•  ì§? boss HP ê°? 30% ?´?•˜?¼ ?•Œ 50% ?™•ë¥ ë¡œ ? •?•´ì¤ë‹ˆ?‹¤.
    /// </summary>
    /// <returns> true?´ë©? BreathAttack, false?´ë©? NomalAttack </returns>
    public bool SetBreathChance()
    {
        float ran = Random.value;
        if (ran <= 0.5)   // ì¶”í›„ boss HP ê°? 30% ?´?•˜?¼ ì¡°ê±´ ì¶”ê?? : CombatManager._currentBossHP <= 600 && 
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
    /// ë³´ìŠ¤ê°? Nomal ?ƒ?™©?—?„œ ê±¸ì„ ì§??— ????•˜?—¬ 50% ?™•ë¥ ë¡œ ? •?•´ì¤ë‹ˆ?‹¤.
    /// </summary>
    /// <returns> true?´ë©? NomalWalking, false?´ë©? IDLE </returns>
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
    /// ë³´ìŠ¤ê°? Nomal?ƒ?ƒœ?—?„œ ???ì§ì¸?‹¤ë©? ?œ?¤ ë°©í–¥?„ ê²°ì •?•©?‹ˆ?‹¤.
    /// </summary>
    /// <returns>?œ?¤ ë°©í–¥</returns>
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
        //CombatManager.Instance null¶ß´Â ¹®Á¦ÀÖÀ½
        if(CombatManager.Instance != null)
        {
            // ?”Œ? ˆ?´?–´ê°? ë²”ìœ„ ?‚´?— ?ˆ?„ ?•Œ ë¹¨ê°„?ƒ‰?œ¼ë¡?, ?•„?‹ˆë©? ?…¹?ƒ‰?œ¼ë¡? ë²”ìœ„ë¥? ?‘œ?‹œ
            Gizmos.color = CombatManager.Instance._bossAttackRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, 9f);

            // ë³´ìŠ¤?˜ ?‹œ?•¼ ë²”ìœ„ë¥? ?ŒŒ????ƒ‰?œ¼ë¡? ?‘œ?‹œ
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, 18f);

            // ë³´ìŠ¤?˜ ? „ë°? ë°©í–¥ ?‘œ?‹œ
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 18f);
        }

    }

    /// <summary>
    /// ?–´?– ?•œ ?• ?‹ˆë©”ì´?…˜ ?›„?— ?š¸ë¶?ì§–ëŠ” ?• ?‹ˆë©”ì´?…˜?„ ì¶œë ¥?•  ?ˆ˜ ?ˆ?Š” ì½”ë£¨?‹´ ?‹¤?–‰ ?•¨?ˆ˜?…?‹ˆ?‹¤.
    /// Hit?˜ ê²½ìš° 0.8ì´ˆê?? ?•Œë§ìŠµ?‹ˆ?‹¤.
    /// </summary>
    /// <param name="time">?´? „ ?• ?‹ˆë©”ì´?…˜?´ ?‹¤?–‰?˜ê³? ?ˆ?Š” ?‹œê°?</param>
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