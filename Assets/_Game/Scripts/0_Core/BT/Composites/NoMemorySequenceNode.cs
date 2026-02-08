using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无记忆序列节点 每次都会评估所有节点 遍历执行所有子节点 成功继续失败返回
/// </summary>
public class NoMemorySequenceNode : BTNode
{
    List<BTNode> childNodes;

    public NoMemorySequenceNode(List<BTNode> childNodes)
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
