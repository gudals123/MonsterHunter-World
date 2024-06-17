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
                .Sequence()
                    .Condition("PlayerTracking", () => catController.catState == CatController.CatState.Detect && catController.isPlayer)
                    .Do(() =>
                    {
                        Debug.Log("Tracking");
                        //catController.PlayerTracking();
                        return TaskStatus.Success;
                    })
                    // ���� Ʈ��ŷ Ʈ��
                    .Sequence()
                        .Condition("BossTracking", () => playerController.playerState == PlayerState.Attack && catController.isBoss)
                        .Do(() =>
                        {
                            Debug.Log("BossTracking");
                            cat.BossTracking();
                            return TaskStatus.Success;
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
                    .Condition("PlayerDetect", () => catController.catState == CatController.CatState.Detect && catController.isPlayer)
                    .Do(() =>
                    {
                        Debug.Log("PlayerDetect");
                        //catController.PlayerTracking();
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

        catController.PlayerTracking();
        catController.BossTracking();

        if (catController.catState == CatController.CatState.Hit)
        {
            cat.Hit(cat.damage);
        }
    }
}
