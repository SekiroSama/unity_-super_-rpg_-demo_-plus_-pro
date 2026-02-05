using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 随机选择节点 失败继续成功返回 直到全部失败 根据权重随机选择一个能跑的跑
/// </summary>
public class WeightedRandomSelectorNode : BTNode
{
    List<BTNode> childNodes;
    List<float> weights;
    private int _currentChildIndex = -1;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="childNodes">子节点</param>
    /// <param name="weights">权重,权重之和应该为1</param>
    public WeightedRandomSelectorNode(List<BTNode> childNodes, List<float> weights)
    {
        this.childNodes = childNodes;
        this.weights = weights;
    }

    float randomValue = 0f;
    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        if(_currentChildIndex != -1)
        {
            switch (childNodes[_currentChildIndex].Evaluate(blackboard))
            {
                case NodeStatus.SUCCESS:
                    currentStatus = NodeStatus.SUCCESS;
                    _currentChildIndex = -1;
                    return currentStatus;
                case NodeStatus.RUNNING:
                    currentStatus = NodeStatus.RUNNING;
                    return currentStatus;
                case NodeStatus.FAILURE:
                    currentStatus = NodeStatus.FAILURE;
                    _currentChildIndex = -1;
                    break;
            }
        }
        float totalWeight = 0f;
        randomValue = Random.Range(0f, 1f);
        for (int i = 0; i < childNodes.Count; i++)
        {
            if (randomValue > totalWeight && randomValue < (totalWeight += weights[i]))
            {
                switch (childNodes[i].Evaluate(blackboard))
                {
                    case NodeStatus.SUCCESS:
                        currentStatus = NodeStatus.SUCCESS;
                        return currentStatus;
                    case NodeStatus.RUNNING:
                        _currentChildIndex = i;
                        currentStatus = NodeStatus.RUNNING;
                        return currentStatus;
                    case NodeStatus.FAILURE:
                        currentStatus = NodeStatus.FAILURE;
                        //重新随机
                        randomValue = Random.Range(totalWeight, 1f);
                        continue;
                }
            }
        }
        _currentChildIndex = -1;
        currentStatus = NodeStatus.FAILURE;
        return currentStatus;
    }
}
