using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatFatDragon_AI : Enemy_AI
{
    public float stoppingDistance = 10f;

    protected override void InitBlackboard()
    {
        base.InitBlackboard();
        blackboard.SetValue<PlayerController>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerController, GameManager.Instance.playerController);
        blackboard.SetValue<Vector3>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
    }

    protected override BTNode BuildTree()
    {
        List<BTNode> root_ChildNodes = new List<BTNode>();//root节点

        //List<BTNode> root_atk_childNodes = new List<BTNode>();//攻击节点
        //root_atk_childNodes.Add(new SequenceNode());
        //List<BTNode> root_atk__childNodes = new List<BTNode>();

        root_ChildNodes.Add(new MoveToTargetNode(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, stoppingDistance));

        SelectorNode root = new SelectorNode(root_ChildNodes);
        return root;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateBlackboard()
    {
        blackboard.SetValue<Vector3>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
    }
}
