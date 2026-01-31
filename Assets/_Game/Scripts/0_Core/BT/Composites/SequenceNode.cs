using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : BTNode
{
    List<BTNode> childNodes;

    public SequenceNode(List<BTNode> childNodes)
    {
        this.childNodes = childNodes;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        for (int i = 0; i < childNodes.Count; i++)
        {
            switch(childNodes[i].Evaluate(blackboard))
            {
                case NodeStatus.SUCCESS:
                    continue;
                case NodeStatus.FAILURE:
                    currentStatus = NodeStatus.FAILURE;
                    return currentStatus;
                case NodeStatus.RUNNING:
                    currentStatus = NodeStatus.RUNNING;
                    return currentStatus;
            }
        }
        currentStatus = NodeStatus.SUCCESS;
        return currentStatus;
    }
}
