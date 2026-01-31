using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode
{
    public enum NodeStatus
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public NodeStatus currentStatus = NodeStatus.FAILURE;

    public abstract NodeStatus Evaluate(Blackboard blackboard);
}
