using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatFatDragon_AI : Enemy_AI
{
    DebugNode debugNodeSUCCESS = new DebugNode("Debug", BTNode.NodeStatus.SUCCESS);//DebugNode
    DebugNode debugNodeFAILURE = new DebugNode("Debug", BTNode.NodeStatus.FAILURE);//DebugNode


    protected override void InitBlackboard()
    {
        blackboard = new Blackboard();
        blackboard.SetValue<EnemyController>(Enemy_AIBlackBoard_Config.KEY_SELF_EnemyController, enemyController);
        blackboard.SetValue<PlayerController>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerController, GameManager.Instance.playerController);
        blackboard.SetValue<Vector3>(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, GameManager.Instance.playerController.transform.position);
        blackboard.SetValue<Vector3>(Enemy_AIBlackBoard_Config.KEY_EnemyController_HomePos, enemyController.HomeTransform.position);
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
        /// <param ConditionNode="isDowned">韧性小于等于0</param> 通常返回失败 韧性小于等于0返回成功
        /// <param GenericActionNode="Downed, ()=> isDowned">倒地节点</param> 通常返回ing 恢复后返回成功
        //  }
        List<BTNode> root_downedCheck_ChildNodes = new List<BTNode>();//韧性检查 子节点
        ConditionNode root_downedCheck_ConditionNode = new ConditionNode(() => enemyController.isDowned);
        root_downedCheck_ChildNodes.Add(root_downedCheck_ConditionNode);
        GenericActionNode root_downedCheck_GenericActionNode = new GenericActionNode(enemyController.Downed, () => !enemyController.isDowned);
        root_downedCheck_ChildNodes.Add(root_downedCheck_GenericActionNode);
        SequenceNode root_downedCheck = new SequenceNode(root_downedCheck_ChildNodes);

        //  逃跑_低血量 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param ConditionNode="isLowHp && haveRunChance, () => enemyController.haveRunChance = false">没有进入低血量,有无逃跑机会</param> 通常返回成功 如果没有进入低血量返回失败 如果无逃跑机会返回失败
        /// <param MoveToTargetNode="homePos, speed_run">移动到指定位置（巢穴）</param> 通常返回ing 跑完后返回成功
        /// <param GenericActionNode="Sleep, ()=> !isSleeping = _enemyController.isFighting || _enemyController.Hp == _enemyController.MaxHp">睡觉时可缓慢回血，但收到双倍伤害</param> 通常返回ing 醒了后返回成功
        //  }
        List<BTNode> root_runAway_ChildNodes = new List<BTNode>();//逃跑_低血量 子节点
        ConditionNode root_runAway_ConditionNode = new ConditionNode(() => enemyController.isLowHp && enemyController.haveRunChance, () => enemyController.RunAwayChance--);
        root_runAway_ChildNodes.Add(root_runAway_ConditionNode);
        MoveToTargetNode root_runAway_MoveToTargetNode = new MoveToTargetNode(Enemy_AIBlackBoard_Config.KEY_EnemyController_HomePos, 0f, enemyController.curentSpeedRatio);
        root_runAway_ChildNodes.Add(root_runAway_MoveToTargetNode);
        GenericActionNode root_runAway_GenericActionNode = new GenericActionNode(enemyController.Sleep, () => !enemyController.isSleeping);
        root_runAway_ChildNodes.Add(root_runAway_GenericActionNode);
        SequenceNode root_runAway = new SequenceNode(root_runAway_ChildNodes);

        //  战斗 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param ConditionNode="isFighting">检查是否触发战斗</param> 通常返回失败 战斗时返回成功
        /// <param SequenceNode="BackAway, ()=> isLowStamina">精力不足时对峙</param> 通常返回成功 精力不足返回ing 
        /// <param SequenceNode="DragonShout, ()=> isDragonShouTriggered">龙吼</param> 通常返回ing 吼过了返回成功 
        /// <param WeightedRandomSelector="childNodes, weights">攻击,选择一个满足条件的发动，多个条件满足则随机一个</param>
        //  }
        List<BTNode> root_fight_ChildNodes = new List<BTNode>();//战斗 子节点
        ConditionNode root_fight_ConditionNode = new ConditionNode(() => enemyController.isFighting);
        root_fight_ChildNodes.Add(root_fight_ConditionNode);

        //  BackAway SelectorNode成功返回失败继续
        //  {
        /// <param ConditionNode="isLowStamina">检查是否触发BackAway</param> 通常返回成功 精力不足返回失败
        /// <param GenericActionNode="BackAway, ()=> isLowStamina">精力不足时对峙</param> 通常返回ing BackAway完返回成功 
        //  }
        List<BTNode> root_fight_BackAway_ChildNodes = new List<BTNode>();//BackAway 子节点
        ConditionNode root_fight_BackAway_ConditionNode = new ConditionNode(() => !enemyController.isLowStamina);
        root_fight_BackAway_ChildNodes.Add(root_fight_BackAway_ConditionNode);
        GenericActionNode root_fight_GenericActionNode_BackAway = new GenericActionNode(enemyController.BackAway, () => enemyController.isBackAwaying);
        root_fight_BackAway_ChildNodes.Add(root_fight_GenericActionNode_BackAway);
        SelectorNode root_fight_BackAway = new SelectorNode(root_fight_BackAway_ChildNodes);//BackAway 节点
        root_fight_ChildNodes.Add(root_fight_BackAway);

        //  DragonShout SelectorNode成功返回失败继续
        //  {
        /// <param ConditionNode="isDragonShouTriggered">检查是否触发DragonShout</param> 通常返回成功 没吼过返回失败
        /// <param GenericActionNode="DragonShout, ()=> isDragonShouTriggered">龙吼</param> 通常返回ing 吼完过返回成功 
        //  }
        List<BTNode> root_fight_DragonShout_ChildNodes = new List<BTNode>();//DragonShout 子节点
        ConditionNode root_fight_DragonShout_ConditionNode = new ConditionNode(() => enemyController.isDragonShouTriggered);
        root_fight_DragonShout_ChildNodes.Add(root_fight_DragonShout_ConditionNode);
        GenericActionNode root_fight_GenericActionNode_DragonShout = new GenericActionNode(enemyController.DragonShout, () => enemyController.isDragonShouting);
        root_fight_DragonShout_ChildNodes.Add(root_fight_GenericActionNode_DragonShout);
        SelectorNode root_fight_DragonShout = new SelectorNode(root_fight_DragonShout_ChildNodes);//BackAway 节点
        root_fight_ChildNodes.Add(root_fight_DragonShout);

        //  攻击 WeightedRandomSelectorNode 如果没有执行中的节点选择一个满足条件的发动，多个条件满足则根据权重随机一个
        //  {
        /// <param SequenceNode="投射物攻击">投射物攻击</param> 通常返回ing 攻击完成后返回成功
        /// <param SequenceNode="近战攻击">近战攻击</param> 通常返回ing 攻击完成后返回成功
        //  }
        List<BTNode> root_fight_atk_ChildNodes = new List<BTNode>();//攻击 子节点
        List<float> root_fight_atk_Weights = new List<float>();//攻击 子节点权重

        //  投射物攻击 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param MoveToTargetNode="playerPos, speed">移动到player</param> 通常返回ing 到达后返回成功
        /// <param GenericActionNode="ProjectileAttack, ()=> isAttacking">投射物攻击</param> 通常返回ing 攻击完成后返回成功
        //  }
        List<BTNode> root_fight_atk_ProjectileAttack_ChildNodes = new List<BTNode>();//投射物攻击 子节点
        MoveToTargetNode root_fight_atk_ProjectileAttack_MoveToTargetNode = new MoveToTargetNode(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, enemyController.ProjectileAttackDistance, enemyController.curentSpeedRatio);
        GenericActionNode root_fight_atk_ProjectileAttack_GenericActionNode = new GenericActionNode(enemyController.ProjectileAttack, ()=>enemyController.isAttacking);
        root_fight_atk_ProjectileAttack_ChildNodes.Add(root_fight_atk_ProjectileAttack_MoveToTargetNode);
        root_fight_atk_ProjectileAttack_ChildNodes.Add(root_fight_atk_ProjectileAttack_GenericActionNode);

        //  近战攻击 SequenceNode序列节点成功继续失败返回
        //  {
        /// <param MoveToTargetNode="playerPos, speed">移动到player</param> 通常返回ing 到达后返回成功
        /// <param GenericActionNode="MeleeAttack, ()=> isAttacking">近战攻击</param> 通常返回ing 攻击完成后返回成功
        //  }
        List<BTNode> root_fight_atk_MeleeAttack_ChildNodes = new List<BTNode>();//近战攻击 子节点
        MoveToTargetNode root_fight_atk_MeleeAttack_MoveToTargetNode = new MoveToTargetNode(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos, enemyController.MeleeAttackDistance, enemyController.curentSpeedRatio);
        GenericActionNode root_fight_atk_MeleeAttack_GenericActionNode = new GenericActionNode(enemyController.MeleeAttack, () => enemyController.isAttacking);
        root_fight_atk_MeleeAttack_ChildNodes.Add(root_fight_atk_MeleeAttack_MoveToTargetNode);
        root_fight_atk_MeleeAttack_ChildNodes.Add(root_fight_atk_MeleeAttack_GenericActionNode);

        SequenceNode root_fight_atk_ProjectileAttack = new SequenceNode(root_fight_atk_ProjectileAttack_ChildNodes);//投射物攻击 节点
        SequenceNode root_fight_atk_MeleeAttack = new SequenceNode(root_fight_atk_MeleeAttack_ChildNodes);//近身攻击 节点

        root_fight_atk_ChildNodes.Add(root_fight_atk_ProjectileAttack);
        root_fight_atk_Weights.Add(blackboard.GetValue<float>(Enemy_AIBlackBoard_Config.KEY_EnemyController_AtkWeights_ProjectileAttack));
        root_fight_atk_ChildNodes.Add(root_fight_atk_MeleeAttack);
        root_fight_atk_Weights.Add(blackboard.GetValue<float>(Enemy_AIBlackBoard_Config.KEY_EnemyController_AtkWeights_MeleeAttack));


        WeightedRandomSelectorNode root_fight_atk = new WeightedRandomSelectorNode(root_fight_atk_ChildNodes, root_fight_atk_Weights);//攻击 节点
        //root_fight_ChildNodes.Add(root_fight_atk);
        SequenceNode root_fight = new SequenceNode(root_fight_ChildNodes);//战斗 节点

        //  巡逻 Patrol SelectorNode
        //  {
        /// <param SearchSomethingNode="搜索范围和回调">检查视野中player出现来触发战斗</param> 通常返回失败 搜到后返回成功
        /// <param ConditionNode="hasTakeDamage">检查是否受到攻击掉血来触发战斗</param> 通常返回失败 受伤后返回成功
        /// <param MoveToTargetNode="targetPosList, speed">在一条指定路径上来回移动</param> 通常返回ing
        //  }
        List<BTNode> root_Patrol_ChildNodes = new List<BTNode>();//巡逻 子节点
        SearchSomethingNode root_SelectorNode_SearchSomethingNode = new SearchSomethingNode(Enemy_AIBlackBoard_Config.KEY_Player_PlayerPos);
        root_Patrol_ChildNodes.Add(root_SelectorNode_SearchSomethingNode);
        ConditionNode root_SelectorNode_ConditionNode = new ConditionNode(()=> enemyController.hasTakeDamage);
        root_Patrol_ChildNodes.Add(root_SelectorNode_ConditionNode);
        MoveToTargetNode root_SelectorNode_MoveToTargetNode = new MoveToTargetNode(Enemy_AIBlackBoard_Config.KEY_EnemyController_CurrentPatrolTarget, 0f, enemyController.curentSpeedRatio);
        root_Patrol_ChildNodes.Add(root_SelectorNode_MoveToTargetNode);
        SelectorNode root_Patrol = new SelectorNode(root_Patrol_ChildNodes);//巡逻 节点

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
        root_ChildNodes.Add(root_runAway);
        root_ChildNodes.Add(root_fight);
        //root_ChildNodes.Add(root_Patrol);
        rootNode = new NoMemorySelectorNode(root_ChildNodes);//root 节点
        return rootNode;
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
