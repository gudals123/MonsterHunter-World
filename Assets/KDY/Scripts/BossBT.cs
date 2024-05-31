using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class BossBT : MonoBehaviour
{
    public float movementSpeed;
    public BehaviorTree tree;
    public Transform player;

    public Animator animator;

    public bool _detectedPlayer = true;

    public bool rotationToPlayer = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
            // Left SubTree_NormalAttack
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => CombatManager._isPlayerInRange)
                    .StateAction("NomalAttack", () => rotationToPlayer = true )
                    .Do(() =>
                    {
                        Debug.Log("NomalAttack");
                        return TaskStatus.Success;
                    })
                .End()
            // Middle SubTree
/*                .Sequence()
                    .Condition("detectedPlayer", () => _detectedPlayer)
                    .StateAction("BattleTracking")
                    .Do("TrackingPlayer", () =>
                    {
                        Debug.Log("BattleTracking");
                        return TaskStatus.Success;
                    })
                .End()
*/
            // Right SubTree
                .Sequence()
                    .StateAction("NomalWalking", () => rotationToPlayer = false )
                .End()
            .End()
            .Build();
    }

    private void Update()  
    {
        CombatManager.isPlayerInRange(player, gameObject.transform);

        if (rotationToPlayer)
        {
            LookAtPlayer();
        }


        /*        if (CombatManager._checkParrying)
                {
                    BossParrying();
                }
        */
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