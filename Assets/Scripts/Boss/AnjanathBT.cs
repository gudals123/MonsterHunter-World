using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class AnjanathBT : BossBehaviorTree
{
    private Anjanath anjanath;
    public State anjanathState;
    AnjanathController bossController;

    // 임시 변수
    public bool SetDamage;

    private void Awake()
    {
        anjanath = GetComponent<Anjanath>();
        moveSpeed = 5;

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                // Left SubTree
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => anjanathState == State.Attack)
                        .Selector()
                            .Sequence()
                                .Condition("BreathAttack", () => anjanath.startBreathAttaking)
                                    .StateAction("BreathAttack", () => anjanath.BreathAttacking())
                            .End()
                            .Sequence()
                                .Condition("NormalAttack", () => anjanath.startNormalAttaking)
                                    .StateAction("NormalAttack", () => anjanath.NormalAttacking())
                            .End()
                        .End()
                .End()

                // Midle SubTree
                .Sequence()
                    .Condition("TrackingPlayer", () => anjanathState == State.Tracking)
                        .Do("TrackingPlayer", () =>
                        {
                            anjanath.StartTracking();
                            return TaskStatus.Success;
                        })
                .End()

                // Right SubTree
                .Selector()
                    //.Condition("IsNomalStates", () => anjanathState == State.Idle)
                    .Sequence()
                        .Condition("IsTargetSelected", () => anjanath.isSetTargetPos)
                        .Do("NomalWalking", () =>
                        {
                            anjanath.NormalMoving(2);
                            return TaskStatus.Success;
                        })
                    .End()
                    .Sequence()
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
        if (anjanathState == State.GetHit)
        {
            anjanath.Hit(bossController.getOtherAttackDamage);
        }

        else
        {
            tree.Tick();
        }
    }

}
