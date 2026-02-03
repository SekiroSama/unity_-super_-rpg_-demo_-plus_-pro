using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 取反节点
/// 暂未完成
/// </summary>
public class InverterNode : BTNode
{
    private BTNode _childNode;
    public InverterNode(BTNode childNode)
    {
        this._childNode = childNode;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        throw new System.NotImplementedException();
    }
}
