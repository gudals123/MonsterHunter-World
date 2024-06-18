using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CatBT : AIController
{
    private CatController catController;
    private Cat cat;
    private PlayerController playerController;

    private void Awake()
    {
        moveSpeed = 10;
        cat = GetComponent<Cat>();
        catController = GetComponent<CatController>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence()
                    .Condition("Hit", () => catController.catState == CatController.CatState.Hit)
                    .StateAction("Hit", () =>
                    {
                        cat.Hit(cat.damage);
                    })
                // ���� Ʈ��
                .Sequence()
                    .Condition("Attack", () => playerController.playerState == PlayerState.Attack && catController.catState == CatController.CatState.Attack)
                    .Do("Attack", () =>
                    {
                        cat.Attack(catController.target.transform);
                        return TaskStatus.Success;
                    })
                .End()
                // �÷��̾� Ʈ��ŷ Ʈ��
                .Selector()
                    .Condition("Tracking", () => catController.catState == CatController.CatState.Tracking && catController.isPlayer)
                    .Do(() =>
                    {
                        cat.Tracking();
                        return TaskStatus.Success;
                    })
                    // ���� Ʈ��ŷ Ʈ��
                    .Sequence()
                        .Condition("Tracking", () => playerController.playerState == PlayerState.Attack && catController.isBoss)
                        .Do(() =>
                        {
                            cat.Tracking();
                            return TaskStatus.Success;
                        })
                    .End()
                    // �÷��̾� ��� Ʈ��
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do("PlayerCommand", () =>
                        {
                            cat.Heal();
                            return TaskStatus.Success;
                        })
                    .End()
                .End()
                // �÷��̾� ���� Ʈ��
                .Sequence()
                    .Condition("PlayerDetect", () => catController.catState == CatController.CatState.Idle)
                    .Do("PlayerDetect", () =>
                    {
                        cat.Tracking();
                        return TaskStatus.Success;
                    })
                    // ��ų ��� Ʈ��
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do("PlayerCommand", () =>
                        {
                            cat.Heal();
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
