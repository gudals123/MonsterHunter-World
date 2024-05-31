using CleverCrow.Fluid.BTs.Trees;
using System;

public static class BehaviorTreeBuilderExtensions
{
    public static BehaviorTreeBuilder StateAction(this BehaviorTreeBuilder builder, string statename = "Ani Name", Action? onEnterAction = null)
    {
        return builder.AddNode(new StateAction(statename, onEnterAction)
        {
            Name = statename,
        });
    }
}