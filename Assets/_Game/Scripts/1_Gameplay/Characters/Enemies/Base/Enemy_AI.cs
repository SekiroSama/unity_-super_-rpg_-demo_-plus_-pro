using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy的AI基类
/// </summary>
public abstract class Enemy_AI : MonoBehaviour
{
    Blackboard blackboard;
    EnemyController enemyController;
    BTNode rootNode;

    public virtual void Start()
    {
        enemyController = this.GetComponent<EnemyController>();
        Init(enemyController);
    }

    void Update()
    {
        if (rootNode == null) return;
        rootNode.Evaluate(blackboard);
    }

    /// <summary>
    /// 初始化AI
    /// </summary>
    /// <param name="enemyController">EnemyController</param>
    public virtual void Init(EnemyController enemyController)
    {
        blackboard = new Blackboard();
        blackboard.SetValue<EnemyController>(Enemy_AI_Config.KEY_SELF, enemyController);
        rootNode = BuildTree();
    }

    /// <summary>
    /// 构建行为树
    /// </summary>
    /// <returns></returns>
    protected abstract BTNode BuildTree();
}
