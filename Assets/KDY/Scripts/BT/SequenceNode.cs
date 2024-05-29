using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    private List<Node> _children;

    public SequenceNode(List<Node> children)
    {
        _children = children;
    }

    public override NodeState Run()
    {
        foreach (Node child in _children)
        {
            NodeState result = child.Run();
            if(result == NodeState.Failure)
            {
                return NodeState.Failure;
            }
        }
        return NodeState.Success;
    }
}
