using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 搜索节点
/// 搜索到了返回成功 没搜索到返回失败
/// </summary>
public class SearchSomethingNode : BTNode
{
    private UnityAction _callBack;
    //private Enemy_AIBlackBoard_Config _awarenessRadiusKey;
    //private Enemy_AIBlackBoard_Config _viewAnglePosKey;
    //private Enemy_AIBlackBoard_Config _viewDistancePosKey;
    private Enemy_AIBlackBoard_Config _searchTargetPosKey;
    private Vector3 _searchTargetPos;
    //private float _awarenessRadius;
    //private float _viewAngle;
    //private float _viewDistance;
    private EnemyController _enemyController;

    /// <summary>
    /// init
    /// </summary>
    /// <param name="searchTargetPosKey"></param>
    /// <param name="callBack"></param>
    public SearchSomethingNode(Enemy_AIBlackBoard_Config searchTargetPosKey, UnityAction callBack = null)
    {
        //_awarenessRadiusKey = awarenessRadius;
        //_viewAnglePosKey = viewAngle;
        //_viewDistancePosKey = viewDistance;
        _searchTargetPosKey = searchTargetPosKey;
        _callBack = callBack;
    }


    public override NodeStatus Evaluate(Blackboard blackboard)
    {
        _enemyController = blackboard.GetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController);

        //_awarenessRadius = blackboard.GetValue<float>(_awarenessRadiusKey);
        //_viewAngle = blackboard.GetValue<float>(_viewAnglePosKey);
        //_viewDistance = blackboard.GetValue<float>(_viewDistancePosKey);
        _searchTargetPos = blackboard.GetValue<Vector3>(_searchTargetPosKey);

        if (_enemyController.SearchSomething(_enemyController.AwarenessRadius, _enemyController.ViewAngle, _enemyController.ViewDistance, _searchTargetPos))
        {
            _callBack?.Invoke();
            currentStatus = NodeStatus.SUCCESS;
            return currentStatus;
        }

        currentStatus = NodeStatus.FAILURE;
        return currentStatus;
    }
}
