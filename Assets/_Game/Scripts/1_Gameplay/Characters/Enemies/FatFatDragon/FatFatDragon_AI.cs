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
    /// Composites 组合节点 必须有多个子节点
    /// </summary>
    /// <param name="SelectorNode">选择节点</param> 失败继续成功返回 直到全部失败 选择一个能跑的跑
    /// <param name="SequenceNode">序列节点</param> 成功继续失败返回 直到全部成功 一步一步走完整个流程
    /// <param name="ParallelNode">并行节点</param> 成功继续失败返回 直到全部成功 同时调用所有子节点 

    /// <summary>
    /// Decorators 装饰节点 有且只有一个子节点
    /// </summary>
    /// <param name="InverterNode">取反节点</param> 暂未完成

    /// <summary>
    /// LeafNodes 叶子节点 没有子节点，是树的末端。
    /// </summary>
    /// Conditions 只做判断
    /// <param name="ConditionNode">条件节点</param> 比较数值大小 
    /// Actions 具体的行为节点
    /// <param name="WaitNode">等待节点</param>
    /// <param name="MoveToTargetNode">移动节点</param>



    /// <summary>
    /// FatFatDragon_AI行为逻辑
    /// </summary>
    //  SequenceNode 序列节点 成功继续
    //  {
    /// <param name="巡逻">在一条指定路径上巡逻</param> 发现player返回成功 否则返回ing
    /// <param name="战斗_普通">进入普通战斗状态</param> 通常返回ing 切换状态返回成功
    /// <param name="战斗_发怒">进入发怒战斗状态</param> 通常返回ing 切换状态返回成功
    /// <param name="逃跑_低血量">进入逃跑状态</param> 通常返回ing 切换状态返回成功
    //  }



    /// <summary>
    /// 巡逻行为逻辑
    /// </summary>

    /// <param name="移动">在一条指定路径上来回移动</param>

    /// <param name="搜寻player">直到视野中player出现来触发战斗</param>
    /// <param name="检查是否被攻击">检查是否受到攻击掉血来触发战斗</param>

    /// <param name="进入战斗_普通">进入战斗_普通</param>

    /// <summary>
    /// 战斗_普通行为逻辑
    /// </summary>
    /// <param name="追捕player">移动到player</param>
    /// <param name="普通攻击">发动攻击</param>
    /// <param name="被破韧">韧性小于0，破韧后受伤增加并倒地</param>
    /// <param name="进入发怒">从破韧恢复后进入发怒</param>

    /// <summary>
    /// 战斗_发怒行为逻辑
    /// </summary>
    /// <param name="更快追捕player">移动到player</param>
    /// <param name="发怒攻击">发动攻击更频繁</param>
    /// <param name="被破韧">韧性小于0，但韧性更难被降低</param>

    /// <summary>
    /// 逃跑_低血量行为逻辑
    /// </summary>
    /// <param name="移动">移动到指定位置（巢穴）</param>
    /// <param name="睡觉">睡觉时可缓慢回血，但收到双倍伤害</param>
    /// <param name="进入发怒">进入发怒战斗状态</param>

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

    /// <summary>
    /// 每帧更新Blackboard
    /// </summary>
    protected override void UpdateBlackboard()
    {
        blackboard.SetValue<Vector3>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
    }
}
