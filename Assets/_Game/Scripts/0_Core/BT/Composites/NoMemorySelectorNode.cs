using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无记忆选择节点 每次都会评估所有节点 失败继续成功返回 直到全部失败 选择一个能跑的跑
/// </summary>
public class NoMemorySelectorNode : BTNode
{
    List<BTNode> childNodes;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="childNodes">子节点</param>
    public NoMemorySelectorNode(List<BTNode> childNodes)
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
