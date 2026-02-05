using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy的AI基类
/// </summary>
public abstract class Enemy_AI : MonoBehaviour
{
    protected Blackboard blackboard;
    protected EnemyController enemyController;
    protected BTNode rootNode;

    protected virtual void Start()
    {
        enemyController = this.GetComponent<EnemyController>();
        Init(enemyController);
    }

    protected virtual void Update()
    {
        if (rootNode == null) return;

        UpdateBlackboard();
        rootNode.Evaluate(blackboard);
    }

    /// <summary>
    /// 初始化AI
    /// </summary>
    /// <param name="enemyController">EnemyController</param>
    public virtual void Init(EnemyController enemyController)
    {
        InitBlackboard();
        rootNode = BuildTree();
    }

    /// <summary>
    /// 初始化黑板
    /// </summary>
    protected abstract void InitBlackboard();

    /// <summary>
    /// 构建行为树
    /// </summary>
    /// <returns></returns>
    protected abstract BTNode BuildTree();

    protected virtual void UpdateBlackboard()
    {

    }
}
