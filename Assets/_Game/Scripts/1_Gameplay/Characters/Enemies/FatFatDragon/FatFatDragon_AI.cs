using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatFatDragon_AI : Enemy_AI
{
    public float stoppingDistance = 10f;

    protected override void InitBlackboard()
    {
        base.InitBlackboard();
        blackboard.SetValue<PlayerController>(Enemy_AI_Config.KEY_Player_PlayerController, GameManager.Instance.playerController);
        blackboard.SetValue<Vector3>(Enemy_AI_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
    }

    protected override BTNode BuildTree()
    {
        List<BTNode> childNodes = new List<BTNode>();
        childNodes.Add(new MoveToTargetNode(Enemy_AI_Config.KEY_Player_PlayerPos, stoppingDistance));
        childNodes.Add(new LogNode("recive"));
        childNodes.Add(new WaitNode(5f));
        childNodes.Add(new LogNode("WaitNodeover"));
        SequenceNode root = new SequenceNode(childNodes);

        return root;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateBlackboard()
    {
        blackboard.SetValue<Vector3>(Enemy_AI_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
    }
}
