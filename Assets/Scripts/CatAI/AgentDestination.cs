using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using UnityEngine;
using UnityEngine.AI;

public class AgentDestination : ActionBase
{
    private Animator _catAnimator;
    public Transform target;

    protected override void OnInit()
    {
        _catAnimator = Owner.GetComponent<Animator>();
    }

    protected override TaskStatus OnUpdate()
    {

        return TaskStatus.Success;
    }
}