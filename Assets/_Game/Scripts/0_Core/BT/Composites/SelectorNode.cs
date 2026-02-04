using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BTNode;

/// <summary>
/// 选择节点 失败继续成功返回
/// </summary>
public class SelectorNode : BTNode
{
    List<BTNode> childNodes;
    private int _currentChildIndex = 0;

    public SelectorNode(List<BTNode> childNodes)
    {
        this.childNodes = childNodes;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        for (int i = _currentChildIndex; i < childNodes.Count; i++)
        {
            switch (childNodes[i].Evaluate(blackboard))
            {
                case NodeStatus.SUCCESS:
                    currentStatus = NodeStatus.SUCCESS;
                    _currentChildIndex = 0;
                    return currentStatus;
                case NodeStatus.RUNNING:
                    _currentChildIndex = i;
                    currentStatus = NodeStatus.RUNNING;
                    return currentStatus;
                case NodeStatus.FAILURE:
                    currentStatus = NodeStatus.FAILURE;
                    continue;
            }
        }
        currentStatus = NodeStatus.FAILURE;
        _currentChildIndex = 0;
        return currentStatus;
    }
}
