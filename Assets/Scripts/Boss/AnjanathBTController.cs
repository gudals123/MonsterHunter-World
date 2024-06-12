using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Idle,
    Walk,
    Tracking,
    Attack,
    Roar,
    SetDamage,
    Sturn,
    Dead,
}

public class AnjanathBTController : Controller
{
    private bool checkTarget;
    public GameObject Target;    // 고양이 또는 플레이어
    private Anjanath anjanath;
    public int getOtherAttackDamage;

    public bool SetDamage;

    private void Awake()
    {
        anjanath = GetComponent<Anjanath>();
        moveSpeed = 5;

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                // Left SubTree
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => anjanath.AnjanathState == State.Attack)
                        .Selector()
                            .Sequence()
                                .Condition("BreathAttack", () => anjanath.startBreathAttaking)
                                    .Do(() =>
                                    {
                                        anjanath.BreathAttacking();
                                        return TaskStatus.Success;
                                    })
                            .End()
                            .Sequence()
                                .Condition("NormalAttack", () => anjanath.startNormalAttaking)
                                    .Do(() =>
                                    {
                                        anjanath.NormalAttacking();
                                        return TaskStatus.Success;
                                    })
                            .End()
                        .End()
                .End()

                // Midle SubTree
                .Sequence()
                    .Condition("TrackingPlayer", () => anjanath.AnjanathState == State.Tracking)
                        .Do("TrackingPlayer", () =>
                        {
                            anjanath.TrackingPlayer();
                            return TaskStatus.Success;
                        })
                .End()

                // Right SubTree
                .Selector()
                    .Sequence()
                        .Condition("ChanceForWalking", () => anjanath.AnjanathState == State.Walk)
                            .Do("NomalWalking", () =>
                            {
                                anjanath.NomalMoving();
                                return TaskStatus.Success;
                            })
                    .End()
                    .Sequence()
                        .Condition("ChanceForWalking", () => anjanath.AnjanathState == State.Idle)
                            .Do(() =>
                            {
                                anjanath.Idle();
                                return TaskStatus.Success;
                            })
                    .End()
                .End()
            .End()
            .Build();
    }

    private void Update()
    {
        anjanath.isPlayerInRange(Target.transform, transform);

        if (SetDamage)   // anjanath.AnjanathState == State.SetDamage
        {
            anjanath.SetDamage(getOtherAttackDamage);
        }

        else
        {
            tree.Tick();
        }
    }

}
