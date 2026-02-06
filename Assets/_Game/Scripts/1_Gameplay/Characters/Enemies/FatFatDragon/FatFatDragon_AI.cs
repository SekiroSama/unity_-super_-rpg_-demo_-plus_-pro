using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatFatDragon_AI : Enemy_AI
{
    public float stoppingDistance = 10f;

    protected override void InitBlackboard()
    {
        blackboard = new Blackboard();
        blackboard.SetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController, enemyController);
        blackboard.SetValue<PlayerController>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerController, GameManager.Instance.playerController);
        blackboard.SetValue<Vector3>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
    }

    protected override BTNode BuildTree()
    {
        //  死亡检查 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param ConditionNode="isDead">死了吗</param> 通常返回失败 如果死了返回成功
        /// <param GenericActionNode="Dead, ()=> isDead">死了</param> 通常返回ing
        //  }
        List<BTNode> root_deathCheck_ChildNodes = new List<BTNode>();//死亡检查 子节点
        ConditionNode root_deathCheck_ConditionNode = new ConditionNode(() => enemyController.isDead);
        root_deathCheck_ChildNodes.Add(root_deathCheck_ConditionNode);
        GenericActionNode root_deathCheck_GenericActionNode = new GenericActionNode(enemyController.Die, () => false);
        root_deathCheck_ChildNodes.Add(root_deathCheck_GenericActionNode);
        SequenceNode root_deathCheck = new SequenceNode(root_deathCheck_ChildNodes);

        //  韧性检查 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param ConditionNode="poise <= 0">韧性小于等于0</param> 通常返回失败 韧性小于等于0返回成功
        /// <param GenericActionNode="Downed, ()=> isDowned">倒地节点</param> 通常返回ing 恢复后返回成功
        //  }
        List<BTNode> root_downedCheck_ChildNodes = new List<BTNode>();//韧性检查 子节点
        ConditionNode root_downedCheck_ConditionNode = new ConditionNode(() => enemyController.isDowned);
        root_downedCheck_ChildNodes.Add(root_downedCheck_ConditionNode);
        GenericActionNode root_downedCheck_GenericActionNode = new GenericActionNode(enemyController.Downed, () => enemyController.isDowned);
        root_downedCheck_ChildNodes.Add(root_downedCheck_GenericActionNode);
        SequenceNode root_downedCheck = new SequenceNode(root_downedCheck_ChildNodes);

        //  逃跑_低血量 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param ConditionNode="isLowHealth && haveRunChance">没有进入低血量,有无逃跑机会</param> 通常返回成功 如果没有进入低血量返回失败 如果无逃跑机会返回失败
        /// <param MoveToTargetNode="homePos, speed_run">移动到指定位置（巢穴）</param> 通常返回ing 跑完后返回成功
        /// <param GenericActionNode="Sleep, ()=> isSleeping = _enemyController.isFighting || _enemyController.Hp == _enemyController.MaxHp">睡觉时可缓慢回血，但收到双倍伤害</param> 通常返回ing 醒了后返回成功
        //  }
        List<BTNode> root_runAway_ChildNodes = new List<BTNode>();//逃跑_低血量 子节点
        ConditionNode root_runAway_ConditionNode = new ConditionNode(() => enemyController.isLowHealth && enemyController.haveRunChance);
        root_runAway_ChildNodes.Add(root_runAway_ConditionNode);
        GenericActionNode root_runAway_GenericActionNode = new GenericActionNode(enemyController.Sleep, () => enemyController.isSleeping);
        root_runAway_ChildNodes.Add(root_runAway_GenericActionNode);
        SequenceNode root_runAway = new SequenceNode(root_runAway_ChildNodes);

        //  战斗 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param ConditionNode="isFighting">检查是否触发战斗</param> 通常返回失败 战斗时返回成功
        /// <param GenericActionNode="BackAway, ()=> isLowStamina">精力不足时对峙</param> 通常返回成功 精力不足返回ing 
        /// <param GenericActionNode="DragonShout, ()=> isDragonShouTriggered">龙吼</param> 通常返回ing 吼过了返回成功 
        /// <param WeightedRandomSelector="childNodes, weights">攻击,选择一个满足条件的发动，多个条件满足则随机一个</param>
        //  }
        List<BTNode> root_fight_ChildNodes = new List<BTNode>();//战斗 子节点
        ConditionNode root_fight_ConditionNode = new ConditionNode(() => enemyController.isFighting);
        root_fight_ChildNodes.Add(root_fight_ConditionNode);
        GenericActionNode root_fight_GenericActionNode_BackAway = new GenericActionNode(enemyController.BackAway, () => enemyController.isLowStamina);
        root_fight_ChildNodes.Add(root_fight_GenericActionNode_BackAway);
        GenericActionNode root_fight_GenericActionNode_DragonShout = new GenericActionNode(enemyController.Downed, () => enemyController.isDragonShouTriggered);
        root_fight_ChildNodes.Add(root_fight_GenericActionNode_BackAway);

        //  攻击 WeightedRandomSelector 如果没有执行中的节点选择一个满足条件的发动，多个条件满足则根据权重随机一个
        //  {
        /// <param SequenceNode="投射物攻击">投射物攻击</param> 通常返回ing 攻击完成后返回成功
        /// <param SequenceNode="近战攻击">近战攻击</param> 通常返回ing 攻击完成后返回成功
        //  }
        List<BTNode> root_fight_atk_ChildNodes = new List<BTNode>();//攻击 子节点
        //WeightedRandomSelectorNode root_fight_atk_WeightedRandomSelectorNode = new WeightedRandomSelectorNode(enemyController.Downed, () => enemyController.isDragonShouTriggered);
        //root_fight_atk_ChildNodes.Add(root_fight_atk_SequenceNode_ProjectileAttack);
        //WeightedRandomSelectorNode root_fight_atk_SequenceNode_MeleeAttack = new WeightedRandomSelectorNode(enemyController.Downed, () => enemyController.isDragonShouTriggered);
        //root_fight_atk_ChildNodes.Add(root_fight_atk_SequenceNode_MeleeAttack);
        //SequenceNode root_fight = new SequenceNode(root_fight_ChildNodes);

        /// <summary>
        /// FatFatDragon_AI行为逻辑
        /// </summary>
        //  NoMemorySelectorNode 无记忆选择节点 失败继续成功返回
        //  {
        ///    <param SequenceNode="死亡检查">死亡检查</param> 通常返回失败 如果死了返回成功 死亡后返回ing
        ///    <param SequenceNode="韧性检查">韧性检查</param> 通常返回失败 破韧了返回成功 倒地后返回ing
        ///    <param SequenceNode="逃跑_低血量">进入逃跑状态</param> 通常返回ing 如果已经跑过了或没有进入低血量返回失败
        ///    <param SequenceNode="战斗">进入战斗状态</param> 通常返回ing
        ///    <param SelectorNode="巡逻">在一条指定路径上巡逻</param> 通常返回ing 进入战斗返回成功 
        //  }
        List<BTNode> root_ChildNodes = new List<BTNode>();//root 子节点
        root_ChildNodes.Add(root_deathCheck);
        root_ChildNodes.Add(root_downedCheck);
        NoMemorySelectorNode root = new NoMemorySelectorNode(root_ChildNodes);
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
