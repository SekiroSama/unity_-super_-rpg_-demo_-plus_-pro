using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    public float Hp = 100; 
    public float MaxHp = 100;
    public float Stamina = 100;
    public float MaxMoveSpeed = 9f;

    public bool isDead = false;//是否死亡
    public bool isDowned = false;//是否破韧
    public bool isSleeping = false;//是否睡觉
    public bool isFighting = false;//是否在战斗中
    public bool isBackAwaying = false;//是否在对峙后退中
    public bool isDragonShouTriggered = false;//是否龙吼过
    public bool isAttacking = false;//是否正在攻击

    [SerializeField]
    private float Duration;//抖动时间
    [SerializeField]
    private float JitterScale = 1f;//抖动幅度

    private SkinnedMeshRenderer _meshRenderer;
    private Material _material;
    private Coroutine _jitterCoroutine;//抖动协程引用
    private NavMeshAgent _navMeshAgent;//导航组件
    private Animator _animator;
    private float _targetSpeedRatio;//目标速度

    public Animator animator => _animator;

    public virtual void Start()
    {
        _meshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        _material = _meshRenderer.material;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();
    }

    public virtual void Update()
    {
        UpdateCurrentSpeed();
    }




    public virtual void SetTargetSpeed(float speedRatio)
    {
        _targetSpeedRatio = speedRatio;
    }

    protected virtual void UpdateCurrentSpeed()
    {
        this.animator.SetFloat(EnemyAnimationConfig.Parameters.Speed, _targetSpeedRatio, 0.1f, Time.deltaTime);
    }


    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Die()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.Die);// 播放死亡动画
        isDead = true;
    }

    /// <summary>
    /// 破韧
    /// </summary>
    public virtual void Downed()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.IsDowned);// 播放破韧动画
        isDowned = true;
    }
    
    /// <summary>
    /// 睡觉
    /// </summary>
    public virtual void Sleep()
    {
        animator.SetBool(EnemyAnimationConfig.Parameters.isSleeping, isSleeping);// 播放睡觉动画
        isSleeping = true;
    }

    /// <summary>
    /// 对峙逻辑
    /// </summary>
    public virtual void BackAway()
    {
        isBackAwaying = true;
    }
    
    /// <summary>
    /// 龙吼
    /// </summary>
    public virtual void DragonShout()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.DragonShout);// 播放龙吼动画
        isDragonShouTriggered = true;
    }

    /// <summary>
    /// 投射物攻击
    /// </summary>
    public virtual void ProjectileAttack()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.DragonShout);// 播放投射物攻击动画
        isAttacking = true;
    }

    /// <summary>
    /// 近战攻击
    /// </summary>
    public virtual void MeleeHit()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.DragonShout);// 播放近战攻击动画
        isAttacking = true;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hitPoint">受击位置，传入shader</param>
    public virtual void TakeDamage(int damage, Vector3 hitPoint)
    {
        Hp -= damage;
        _material.SetVector("_HitPos", hitPoint);

        if(_jitterCoroutine != null)
        {
            StopCoroutine(_jitterCoroutine);
        }
        _jitterCoroutine = StartCoroutine(HitJitter());
    }

    #region AI_navMeshAgent
    /// <summary>
    /// 移动到目标位置
    /// </summary>
    /// <param name="targetPos">目标位置</param>
    /// <param name="speedRatio">速度比值,为1时为MaxMoveSpeed</param>
    public virtual void MoveToTarget(Vector3 targetPos, float speedRatio)
    {
        if (!CheckisOnNavMeshAndFix()) return;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(targetPos);
        _navMeshAgent.speed = speedRatio * MaxMoveSpeed;
        SetTargetSpeed(speedRatio);
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        if (!CheckisOnNavMeshAndFix()) return;
        _navMeshAgent.isStopped = true;
        SetTargetSpeed(0f);
    }

    /// <summary>
    /// 检查是否在导航网格上，不在则尝试修复
    /// </summary>
    /// <returns></returns>
    private bool CheckisOnNavMeshAndFix()
    {
        if (!_navMeshAgent.isOnNavMesh)
        {
            NavMeshHit navMeshHit = new NavMeshHit();
            if (NavMesh.SamplePosition(transform.position, out navMeshHit, 10f, NavMesh.AllAreas))
            {
                _navMeshAgent.Warp(navMeshHit.position);
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    /// <summary>
    /// 往受击抖动shader传参开始抖动
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitJitter()
    {
        _material.SetFloat("_HitStrength", JitterScale);
        float timer = 0;
        while(timer < Duration)
        {
            timer += Time.deltaTime;
            float progress = timer / Duration;
            float val = Mathf.Lerp(JitterScale, 0, progress);
            _material.SetFloat("_HitStrength", val);
            yield return null;
        }
    }
}
