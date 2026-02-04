using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class LogNode : BTNode
{
    private string _message;
    public LogNode(string log)
    {
        _message = log;
    }

    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        Debug.Log(_message);
        currentStatus = NodeStatus.SUCCESS;
        return currentStatus;
    }
}
