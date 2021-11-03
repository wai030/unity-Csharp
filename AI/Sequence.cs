using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    protected List<Node> nodes = new List<Node>();

    public Sequence(List<Node> node)
    {
        this.nodes = node;
    }
    public override NodeState Evaluate()
    {
        bool isnoderun = false;
        foreach(var node in nodes)
        {
            switch (node.Evaluate()) 
            {
                case NodeState.running:
                    isnoderun = true;
                    break;
                case NodeState.success:
                    break;
                case NodeState.failure:
                    _nodeState = NodeState.failure;
                    return _nodeState;
                    
                default:
                    break;

            }

        }
        _nodeState = isnoderun ? NodeState.running : NodeState.success;
        return _nodeState;
    }
}
