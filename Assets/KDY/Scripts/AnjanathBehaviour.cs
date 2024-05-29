using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnjanathBehaviour : MonoBehaviour
{
    [SerializeField] private int AnjanathHP;
    [SerializeField] private int AnjanathNomalAttack;
    [SerializeField] private int AnjanathSpeed;

    private Node _behaviour;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _behaviour = BossTree();
    }

    private Node BossTree()
    {
        return new SelectorNode(new List<Node>
        {   
            // Left Subtree
            new SequenceNode(new List<Node>
            {
                new ConditionNode(isPlayerInAttackRange),
                new SequenceNode(new List<Node>
                {
                    new ConditionNode(canBreathAttack),
                    new ActionNode(BreathAttack)
                }),
                new ActionNode(NomalAttack)
            }),

            // Right Subtree
            new SelectorNode (new List<Node>
            {
                new SequenceNode (new List<Node>
                {
                    new ConditionNode(DetectedPlayer),
                    new ActionNode(AttackIdle),
                    new ActionNode(AttackTracking)
                }),
                new SequenceNode (new List<Node>
                {
                    new ActionNode(NomalIdle),
                    new ActionNode(NomalWalking)
                })
            })

        });
    }

    private void ChangeState(string stateName)
    {
        _animator.Play(stateName);
    }

    private void Update()
    {
        _behaviour.Run();
    }

    // 이 아래부터는 임의로 만든 조건 및 값들입니다.
    private bool isPlayerInAttackRange()
    {
        return true;
    }

    private bool canBreathAttack()
    {
        // AnjanathHP <= 50 && is30Chance
        return true;
    }

    private bool DetectedPlayer()
    {
        return true;
    }

    private void BreathAttack()
    {
        ChangeState("breathAttack");
    }

    private void NomalAttack()
    {
        ChangeState("nomalAttack");
    }

    private void AttackIdle()
    {
        ChangeState("attackIdle");
    }
    private void AttackTracking()
    {
        ChangeState("attackTracking");
    }

    private void NomalIdle()
    {
        ChangeState("nomalIdle");
    }

    private void NomalWalking()
    {
        ChangeState("nomalWalking");
    }

}
