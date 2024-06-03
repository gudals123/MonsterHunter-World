using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;

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
    private Vector3 _wayToGoPlayer;

    public GameObject _nomalAtt;
    public GameObject _breathAtt;
    
    // 프로토타입 진행을 위한 임시 변수
    public bool isBossGetHit = false;
    public bool isBossDead = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _bossRb = GetComponent<Rigidbody>();
        _bossRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                // Left SubTree
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => CombatManager._isPlayerInRange)
                        .Selector()
/*                            .Sequence()
                                .Condition("canBreathAttack", () => _canBreathAttack = SetBreathChance())
                                .StateAction("BreathAttack", () => 
                                { 
                                    _bossRb.velocity = Vector3.zero;
                                    _rotationToPlayer = false;
                                    _breathAtt.SetActive(true);
                                })
                                .Do(() =>
                                {
                                    CombatManager._instance.StartBreathAttack();
                                    _breathAtt.SetActive(false);
                                    Debug.Log("Breath Attack");
                                    _canBreathAttack = !_canBreathAttack;
                                    return TaskStatus.Success;
                                })
                            .End()
*/                            .StateAction("NomalAttack", () =>
                            {
                                _bossRb.velocity = Vector3.zero;
                                _rotationToPlayer = false;
                            })
                            .Do(() =>
                            {
                                Debug.Log("Nomal Attack");
                                return TaskStatus.Success;
                            })
                        .End()
                .End()

                // Middle SubTree
                .Sequence()
                    .Condition("detectedPlayer", () => CombatManager._isPlayerInBossView )
                    .StateAction("BattleTracking", () =>
                    {
                        _rotationToPlayer = true;
                        _startTracking = true;
                    })
                    .Do("TrackingPlayer", () =>
                    {
                        Debug.Log("BattleTracking");
                        return TaskStatus.Success;
                    })
                .End()

                // Right SubTree
                .Sequence()
                    .StateAction("NomalWalking", () => _rotationToPlayer = false )
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
        CombatManager.isPlayerInRange(_player, gameObject.transform);
        _wayToGoPlayer.y = 0;

        if (_rotationToPlayer)
        {
            LookAtPlayer();
        }

        if (_startTracking)
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
                _animator.Play("Hit");
            }

            if (isBossDead)   // 추후 !CombatManager._isBossDead로 변경 예정
            {
                _animator.Play("Die");
                CombatManager._isBossDead = true;
            }

            else
            {
                _tree.Tick();
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
            Vector3 direction = (_player.position - transform.position).normalized;
            _moveDirection = new Vector3(direction.x, 0, direction.z); // y축 방향은 무시
            _bossRb.MovePosition(transform.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 targetDirection = (_player.position - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
            _bossRb.MoveRotation(rotation);
        }
    }

    public bool SetBreathChance()
    {
        float ran = UnityEngine.Random.value;
        if(ran <= 0.5) return _canBreathAttack = true;   // 추후 boss HP 가 30% 이하일 조건 추가 : CombatManager._currentBossHP <= 600 && 
        else return _canBreathAttack = false;
    }

    void OnDrawGizmos()
    {
        if (gameObject.transform != null)
        {
            // 플레이어가 범위 내에 있을 때 빨간색으로, 아니면 녹색으로 범위를 표시
            Gizmos.color = CombatManager._isPlayerInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, 9f);

            // 보스의 시야 범위를 파란색으로 표시
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(gameObject.transform.position, 18f);

            // 보스의 전방 방향 표시
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * 18f);
        }
    }

    /*    public void BossSound(string name)
        {
            SoundManager.Instance.Play(SoundType.Effect, name);
        }*/
}