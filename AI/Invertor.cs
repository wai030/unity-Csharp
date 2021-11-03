using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invertor : Node
{
    protected List<Node> nodes = new List<Node>();

    public Invertor(List<Node> node)
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
                    _nodeState = NodeState.failure;
                    return _nodeState;

                case NodeState.failure:
                    _nodeState = NodeState.success;
                    break;

                default:
                    break;

            }

        }
        
        return _nodeState;
    }
}
