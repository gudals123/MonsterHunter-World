using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class AnjanathBT : BossBehaviorTree
{
    private Anjanath anjanath;
    public State anjanathState;
    AnjanathController bossController;

    private void Awake()
    {
        anjanath = GetComponent<Anjanath>();
        moveSpeed = 5;

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("Dead", () => anjanathState == State.Dead)
                        .Do("Roar", () =>
                        {
                            anjanath.Dead();
                            return TaskStatus.Success;
                        })
                .End()
                .Sequence()
                    .Condition("Sturn", () => anjanathState == State.Sturn)
                        .StateAction("Sturn", () =>
                        {
                            anjanath.Sturn();
                        })
                .End()
                .Sequence()
                    .Condition("Hit", () => anjanathState == State.GetHit)
                        .StateAction("Hit", () =>
                        {
                            anjanath.GetHit();
                        })
                .End()
                // Left SubTree
                .Sequence()
                    .Condition("isPlayerInAttackRange", () => anjanathState == State.Attack)
                        .Selector()
                            .Sequence()
                                .Condition("BattleIdle", () => anjanath.SetChance(0.4f))
                                    .StateAction("BattleIdle", () => { })
                            .End()
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

                .Sequence()
                    .Condition("FindPlayer", () => anjanathState == State.Finding)
                        .StateAction("BattleTracking", () => { anjanath.leaveHere = true;})
                        .Do("leaveandcome", () =>
                        {
                            anjanath.leaveHere = false;
                            return TaskStatus.Success;
                        })
                        /*
                        .Do("BattleTracking", () =>
                        {
                            anjanath.LeaveHere();
                            return TaskStatus.Success;
                        })*/
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
        tree.Tick();
    }

}
