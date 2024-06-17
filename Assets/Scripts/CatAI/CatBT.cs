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
                // ���� Ʈ��
                .Sequence()
                    .Condition("Attack", () => playerController.playerState == PlayerState.Attack && catController.catState == CatController.CatState.Attack)
                    .Do(() =>
                    {
                        Debug.Log("Attack");
                        cat.Attack(catController.target.transform);
                        return TaskStatus.Success;
                    })
                .End()
                // �÷��̾� Ʈ��ŷ Ʈ��
                .Selector()
                    .Condition("PlayerTracking", () => catController.catState == CatController.CatState.Detect)
                    .Do(() =>
                    {
                        Debug.Log("Tracking");
                        cat.Tracking();
                        return TaskStatus.Success;
                    })
                    // ���� Ʈ��ŷ Ʈ��
                    .Sequence()
                        .Condition("BossTracking", () => playerController.playerState == PlayerState.Attack)
                        .Do(() =>
                        {
                            Debug.Log("BossTracking");
                            // cat.BossTracking();
                            return TaskStatus.Success;
                        })
                        .StateAction("Attack", () =>
                        {
                            cat.Attack(catController.boss.transform);
                        })
                    .End()
                    // �÷��̾� ��� Ʈ��
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do(() =>
                        {
                            Debug.Log("PlayerCommand");
                            //cat.Heal(); // player�� catController.Heal() ���
                            return TaskStatus.Success;
                        })
                    .End()
                .End()
                // �÷��̾� ���� Ʈ��
                .Sequence()
                    .Condition("PlayerDetect", () => catController.catState == CatController.CatState.Detect)
                    .Do(() =>
                    {
                        Debug.Log("PlayerDetect");
                        return TaskStatus.Success;
                    })
                    // ��ų ��� Ʈ��
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do(() =>
                        {
                            Debug.Log("Skill");
                            catController.Heal();
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

        catController.TargetCheck();
        catController.Tracking();

        catController.Tracking();

        if (catController.catState == CatController.CatState.Hit)
        {
            cat.Hit(cat.damage);
        }
    }
}
