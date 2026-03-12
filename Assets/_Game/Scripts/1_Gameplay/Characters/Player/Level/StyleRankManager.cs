using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleRankManager : MonoBehaviour
{
    public static StyleRankManager Instance;

    [Header("Settings")]
    public float currentScore = 0;
    public float decayRate = 10f; // 每秒扣除的分数
    public float decayDelay = 2f; // 停止攻击后多久开始衰减

    private float _lastAttackTime;

    void Awake() => Instance = this;

    void Update()
    {
        // 处理分数衰减
        if (Time.time > _lastAttackTime + decayDelay && currentScore > 0)
        {
            currentScore -= decayRate * Time.deltaTime;
            currentScore = Mathf.Max(0, currentScore);
        }
    }

    // 怪物受伤脚本调用
    public void AddScore(float amount)
    {
        currentScore += amount;
        _lastAttackTime = Time.time;
        // 这里可以触发UI更新
    }

    public StyleGrade GetCurrentGrade()
    {
        if (currentScore < 20) return StyleGrade.D;
        if (currentScore < 50) return StyleGrade.C;
        if (currentScore < 100) return StyleGrade.B;
        if (currentScore < 200) return StyleGrade.A;
        if (currentScore < 400) return StyleGrade.S;
        if (currentScore < 600) return StyleGrade.SS;
        if (currentScore < 800) return StyleGrade.SSS;
        return StyleGrade.SSS;// 以此类推
    }
}
