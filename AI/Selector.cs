using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    protected List<Node> nodes = new List<Node>();

    public Selector(List<Node> node)
    {
        this.nodes = node;
    }
    public override NodeState Evaluate()
    {
        
        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.running:
                    _nodeState = NodeState.running;
                    return _nodeState;
                case NodeState.success:
                    _nodeState = NodeState.success;
                    return _nodeState;
                    
                case NodeState.failure:
                    
                    break;

                default:
                    break;

            }

        }
        _nodeState = NodeState.failure;
        return _nodeState;
    }
}
