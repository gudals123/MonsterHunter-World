using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    private List<Node> _children;

    public SelectorNode(List<Node> children)
    {
        _children = children;
    }

    public override NodeState Run()
    {
        foreach (var child in _children)
        {
            NodeState result = child.Run();
            if (result == NodeState.Success)
            {
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
