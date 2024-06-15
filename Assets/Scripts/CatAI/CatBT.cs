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
        catController = GetComponent<CatController>();
        cat = GetComponent<Cat>();

        tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                // ���� Ʈ��
                .Sequence()
                    .Condition("Attack", () => catController.catState == CatController.CatState.Attack && playerController.playerState == PlayerState.Attack)
                    .Do(() =>
                    {
                        Debug.Log("Attack!");
                        //catController.catState = CatController.CatState.Attack;
                        //int damage = cat.Attack();
                        return TaskStatus.Success;
                    })
                .End()
                // �÷��̾� Ʈ��ŷ Ʈ��
                .Sequence()
                    .Condition("Tracking", () => catController.catState == CatController.CatState.Tracking && catController.isPlayer)
                    .Do(() =>
                    {
                        Debug.Log("Tracking");
                        //cat.Move(moveSpeed, catController.player.transform.position);
                        return TaskStatus.Success;
                    })
                    // ���� Ʈ��ŷ Ʈ��
                    .Sequence()
                        .Condition("BossTracking", () => playerController.playerState == PlayerState.Attack)
                        .Do(() =>
                        {
                            Debug.Log("BossTracking");
                            //cat.Move(moveSpeed, catController.boss.transform.position);
                            //catController.catState = CatController.CatState.Detect;
                            return TaskStatus.Success;
                        })

                    .End()
                    // �÷��̾� ��� Ʈ��
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do(() =>
                        {
                            Debug.Log("PlayerCommand");
                            cat.Skill();
                            //catController.catState = CatController.CatState.Skill;
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
                        //catController.catState = CatController.CatState.Idle;
                        return TaskStatus.Success;
                    })
                    // ��ų ��� Ʈ��
                    .Sequence()
                        .Condition("PlayerCommand", () => catController.catState == CatController.CatState.Skill)
                        .Do(() =>
                        {
                            Debug.Log("PlayerCommand");
                            cat.Skill();
                            //catController.catState = CatController.CatState.Skill;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossAttack"))
        {
            catController.Hit();
        }
    }
}
