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
                // 공격 트리
                .Sequence()
                    .Condition("Attack", () => playerController.playerState == PlayerState.Attack && catController.catState == CatController.CatState.Attack)
                    .Do(() =>
                    {
                        cat.Attack(catController.target.transform);
                        return TaskStatus.Success;
                    })
                .End()
                // 플레이어 트래킹 트리
                .Selector()
                    .Condition("PlayerTracking", () => catController.catState == CatController.CatState.Detect)
                    .Do(() =>
                    {
                        cat.Tracking();
                        return TaskStatus.Success;
                    })
                    // 보스 트래킹 트리
                    .Sequence()
                        .Condition("BossTracking", () => playerController.playerState == PlayerState.Attack)
                        .Do(() =>
                        {
                            return TaskStatus.Success;
                        })
                        .StateAction("Attack", () =>
                        {
                            cat.Attack(catController.boss.transform);
                        })
                    .End()
                    // 플레이어 명령 트리
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do(() =>
                        {
                            return TaskStatus.Success;
                        })
                    .End()
                .End()
                // 플레이어 감지 트리
                .Sequence()
                    .Condition("PlayerDetect", () => catController.catState == CatController.CatState.Detect)
                    .Do(() =>
                    {
                        return TaskStatus.Success;
                    })
                    // 스킬 사용 트리
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do(() =>
                        {
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

        if (catController.catState == CatController.CatState.Hit)
        {
            cat.Hit(cat.damage);
        }
    }
}
