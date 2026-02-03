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



    /// <summary>
    /// Actions 具体的行为节点
    /// </summary>
    /// <param name="MoveToTargetNode">移动节点</param>

    /// <summary>
    /// Composites 组合节点
    /// </summary>
    /// <param name="ConditionNode">条件节点</param> 可以比较数值大小
    /// <param name="InverterNode">取反节点</param> 暂未完成
    /// <param name="SelectorNode">选择节点</param> 失败继续成功返回
    /// <param name="SequenceNode">序列节点</param> 成功继续失败返回
    /// <param name="WaitNode">等待节点</param>

    /// <summary>
    /// Conditions 条件节点 是 Action 能够执行的前提
    /// </summary>
    /// <param name="a"></param>


    /// <summary>
    /// FatFatDragon_AI行为逻辑
    /// </summary>
    /// <param name="巡逻">在一条指定路径上巡逻，直到视野中player出现或受到攻击</param>
    /// <param name="战斗_普通">进入普通战斗状态</param>
    /// <param name="战斗_发怒">进入发怒战斗状态</param>
    /// <param name="低血量_逃跑">进入发怒战斗状态</param>


    /// <summary>
    /// 巡逻行为逻辑
    /// </summary>
    /// <param name="移动">在一条指定路径上来回移动</param>
    /// <param name="搜寻player">直到视野中player出现</param>
    /// <param name="检查是否被攻击">检查是否受到攻击掉血</param>

    /// <summary>
    /// 战斗_普通行为逻辑
    /// </summary>
    /// <param name="追捕player">移动到player</param>
    /// <param name="攻击">发动攻击</param>



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
