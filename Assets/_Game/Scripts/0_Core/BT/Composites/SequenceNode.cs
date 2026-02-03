using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 序列节点,遍历执行所有子节点 成功继续失败返回
/// </summary>
public class SequenceNode : BTNode
{
    List<BTNode> childNodes;
    private int _currentChildIndex = 0;

    public SequenceNode(List<BTNode> childNodes)
    {
        this.childNodes = childNodes;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        for (int i = _currentChildIndex; i < childNodes.Count; i++)
        {
            switch(childNodes[i].Evaluate(blackboard))
            {
                case NodeStatus.SUCCESS:
                    continue;
                case NodeStatus.FAILURE:
                    currentStatus = NodeStatus.FAILURE;
                    _currentChildIndex = 0;
                    return currentStatus;
                case NodeStatus.RUNNING:
                    currentStatus = NodeStatus.RUNNING;
                    _currentChildIndex = i;
                    return currentStatus;
            }
        }
        currentStatus = NodeStatus.SUCCESS;
        _currentChildIndex = 0;
        return currentStatus;
    }
}
