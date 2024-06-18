using CleverCrow.Fluid.BTs.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourTreeBuildersExtension1
{

    public static BehaviorTreeBuilder AgentDestination1(this BehaviorTreeBuilder builder, string name)
    {
        return builder.AddNode(new AgentDestination
        {
            Name = name,
        });
    }

}
