using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BTNode;

public class SelectorNode : BTNode
{
    List<BTNode> childNodes;

    public SelectorNode(List<BTNode> childNodes)
    {
        this.childNodes = childNodes;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        for (int i = 0; i < childNodes.Count; i++)
        {
            switch (childNodes[i].Evaluate(blackboard))
            {
                case NodeStatus.SUCCESS:
                    currentStatus = NodeStatus.SUCCESS;
                    return currentStatus;
                case NodeStatus.RUNNING:
                    currentStatus = NodeStatus.RUNNING;
                    return currentStatus;
                case NodeStatus.FAILURE:
                    currentStatus = NodeStatus.FAILURE;
                    continue;
            }
        }
        currentStatus = NodeStatus.FAILURE;
        return currentStatus;
    }
}
