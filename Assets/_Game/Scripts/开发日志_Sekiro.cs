
using System.Collections.Generic;
using UnityChan;

public class 开发日志_Sekiro
{
    void 高亮显示()
    {
        BababoitoDoList[0] = null;
        SekiroDoList[0] = null;
    }

    List<string> BababoitoDoList = new List<string>()
    {
        //"预制体会放在Resources_Build文件夹下",
        //"武器的刀光效果：需要在动画事件中控制TrailRenderer的开启和关闭",
        //"刀光效果更新，使用MyTrailRenderer预制体渲染，在动画事件控制生成或销毁，需要保证世界坐标为000，需要调用InitMyTrailRenderer传入首尾位置",
        "武器动态生成，位置可以用现在调好的子节点位置",
        "优化刀光接口isEmitting,可以直接MyTrailRenderer.isEmitting"
    };

    List<string> SekiroDoList = new List<string>()
    {
        //"行为树基类",
        //"行为树拓展",
        "行为树差一个完结，状态机差一点",
    };
}


// Decorators 装饰节点 有且只有一个子节点
/// <param InverterNode="InverterNode">取反节点</param> 暂未完成

// Composites 组合节点 必须有多个子节点
/// <param NoMemorySelectorNode="childNodes">无记忆选择节点</param> 每次都会评估所有节点 失败继续成功返回 直到全部失败 选择一个能跑的跑
/// <param SelectorNode="childNodes">选择节点</param> 失败继续成功返回 直到全部失败 选择一个能跑的跑
/// <param WeightedRandomSelector="childNodes, weights">随机选择节点</param> 失败继续成功返回 直到全部失败 根据权重随机选择一个能跑的跑
/// <param SequenceNode="childNodes">序列节点</param> 成功继续失败返回 直到全部成功 一步一步走完整个流程

// LeafNodes 叶子节点 没有子节点，是树的末端。
/// Conditions 只做判断
/// <param ConditionNode="判断条件和回调函数">条件节点</param> 比较数值大小 
/// Actions 具体的行为节点
/// <param GenericActionNode="Dead, ()=> isDead">死了</param> 通常返回ing
/// <param GenericActionNode="Downed, ()=> isDowned">倒地节点</param> 通常返回ing
/// <param MoveToTargetNode="homePos, speed_run">移动到指定位置（巢穴）</param> 通常返回ing 跑完后返回成功
/// <param GenericActionNode="Sleep, ()=> isSleeping = _enemyController.isFighting || _enemyController.Hp == _enemyController.MaxHp">睡觉时可缓慢回血，但收到双倍伤害</param> 通常返回ing 醒了后返回成功
/// <param GenericActionNode="BackAway, ()=> _isBackAwayTriggered">精力不足时对峙</param> 通常返回ing 精力充足成功 
/// <param GenericActionNode="DragonShout, ()=> DragonShout">龙吼</param> 通常返回ing 龙吼完成过返回成功 
/// <param GenericActionNode="ProjectileAttack, ()=> isAttacking">投射物攻击</param>
/// <param GenericActionNode="MeleeAttack, ()=> isAttacking ">近战攻击</param>
/// <param SearchSomethingNode="搜索范围和回调">检查视野中player出现来触发战斗</param> 通常返回失败 搜到后返回成功




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


//  死亡检查 SequenceNode序列节点成功继续失败返回
//  {
/// <param ConditionNode="isDead">死了吗</param> 通常返回失败 如果死了返回成功
/// <param GenericActionNode="Dead, ()=> isDead">死了</param> 通常返回ing
//  }

//  韧性检查 SequenceNode序列节点成功继续失败返回
//  {
/// <param ConditionNode="poise <= 0">韧性小于等于0</param> 通常返回失败 韧性小于等于0返回成功
/// <param GenericActionNode="Downed, ()=> isDowned">倒地节点</param> 通常返回ing 恢复后返回成功
//  }

//  逃跑_低血量 SequenceNode序列节点成功继续失败返回
//  {
/// <param ConditionNode="isLowHealth && haveRunChance">没有进入低血量,有无逃跑机会</param> 通常返回成功 如果没有进入低血量返回失败 如果无逃跑机会返回失败
/// <param MoveToTargetNode="homePos, speed_run">移动到指定位置（巢穴）</param> 通常返回ing 跑完后返回成功
/// <param GenericActionNode="Sleep, ()=> isSleeping = _enemyController.isFighting || _enemyController.Hp == _enemyController.MaxHp">睡觉时可缓慢回血，但收到双倍伤害</param> 通常返回ing 醒了后返回成功
//  }


//  战斗 SequenceNode序列节点成功继续失败返回
//  {
/// <param ConditionNode="isFighting">检查是否触发战斗</param> 通常返回失败 战斗时返回成功
/// <param GenericActionNode="BackAway, ()=> isLowStamina">精力不足时对峙</param> 通常返回成功 精力不足返回ing 
/// <param GenericActionNode="DragonShout, ()=> DragonShout">龙吼</param> 通常返回ing 吼过了返回成功 
/// <param WeightedRandomSelector="childNodes, weights">攻击,选择一个满足条件的发动，多个条件满足则随机一个</param>
//  }

//  攻击 WeightedRandomSelectorNode 如果没有执行中的节点选择一个满足条件的发动，多个条件满足则根据权重随机一个
//  {
/// <param SequenceNode="投射物攻击">投射物攻击</param> 通常返回ing 攻击完成后返回成功
/// <param SequenceNode="近战攻击">近战攻击</param> 通常返回ing 攻击完成后返回成功
//  }

//  投射物攻击 SequenceNode序列节点成功继续失败返回
//  {
/// <param MoveToTargetNode="playerPos, speed">移动到player</param> 通常返回ing 到达后返回成功
/// <param GenericActionNode="ProjectileAttack, ()=> isAttacking">投射物攻击</param> 通常返回ing
//  }

//  近战攻击 SequenceNode序列节点成功继续失败返回
//  {
/// <param MoveToTargetNode="playerPos, speed">移动到player</param> 通常返回ing 到达后返回成功
/// <param GenericActionNode="MeleeAttack, ()=> isAttacking">近战攻击</param> 通常返回ing
//  }

//  巡逻 SelectorNode
//  {
/// <param SearchSomethingNode="搜索范围和回调">检查视野中player出现来触发战斗</param> 通常返回失败 搜到后返回成功
/// <param ConditionNode="hasTakeDamage">检查是否受到攻击掉血来触发战斗</param> 通常返回失败 受伤后返回成功
/// <param MoveToTargetNode="targetPosList, speed">在一条指定路径上来回移动</param> 通常返回ing
//  }