using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    [Header("数值类")] 
    [SerializeField] private float _Hp = 100; 
    public float Hp 
    {
        get => _Hp;
        set
        {
            _Hp = value <= MaxHp? value : MaxHp;
            isDead = _Hp <= 0;
            isLowHp = _Hp <= MaxHp * LowHpPersent;
        }
    }
    public float MaxHp = 100;
    [Range(0,1)] public float LowHpPersent = 0.2f;//残血阈值
    private float _Stamina = 100;//精力值
    public float Stamina
    {
        get => _Stamina;
        set
        {
            _Stamina = value <= MaxStamina? value : MaxStamina;
            isLowStamina = _Stamina <= MaxStamina * LowStaminaPersent;
        }
    }
    public float MaxStamina = 100;//最大精力值
    [Range(0,1)] public float LowStaminaPersent = 0.2f;//低精力阈值
    [SerializeField] private float _Poise = 100;//韧性值
    public float Poise
    {
        get => _Poise;
        set
        {
            _Poise = value <= MaxPoise? value : MaxPoise;
            isDowned = _Poise <= 0;
        }
    }
    public float MaxPoise = 100;//最大韧性值
    public float HPRegenerationSpeed = 1;//睡觉回复hp速度


    [Header("移动配置类")]
    public float MaxMoveSpeed = 9f;
    public int RunAwayChance = 2; //逃跑机会
    public Vector3 PatrolStart;//巡逻起点
    public Vector3 PatrolEnd;//巡逻终点
    public Transform HomeTransform;//巢穴坐标

    [Header("战斗参数类")]
    public float ProjectileAttackDistance = 50f;//远程攻击距离
    public float MeleeAttackDistance = 5f;//近身攻击距离

    [Header("感知参数类")]
    public float AwarenessRadius = 10f;//近身感知距离
    public float ViewAngle = 120f;//视角范围
    public float ViewDistance = 25f;//视角距离

    public bool isDead = false;//是否死亡
    public bool isDowned = false;//是否破韧
    public bool isSleeping = false;//是否睡觉
    public bool isFighting = false;//是否在战斗中
    public bool isBackAwaying = false;//是否在对峙后退中
    public bool isDragonShouTriggered = false;//是否龙吼过
    public bool isDragonShouting = false;//是否正在龙吼
    public bool isLowHp = false;//是否低血量
    public bool isLowStamina = false;//是否低精力
    public bool haveRunChance//是否有逃跑机会
    {
        get => RunAwayChance > 0;
    }
    private bool _isAttacking = false;//是否正在攻击
    public bool isAttacking
    {
        get { return _isAttacking; }
        set 
        { 
            _isAttacking = value;
            if(animator != null)
            animator.SetBool(EnemyAnimationConfig.Parameters.IsAttacking, _isAttacking);// 进入atk动画层级
        }
    }
    public float curentSpeedRatio = 0f;//当前速度比值
    public bool hasTakeDamage = false;//是否受到攻击
    public Vector3 currentPatrolTarget;//当前巡逻目标点

    [SerializeField]
    private float Duration;//抖动时间
    [SerializeField]
    private float JitterScale = 1f;//抖动幅度

    private SkinnedMeshRenderer _meshRenderer;
    private Material _material;
    private Coroutine _jitterCoroutine;//抖动协程引用
    private NavMeshAgent _navMeshAgent;//导航组件
    private Animator _animator;
    private float _targetSpeedRatio;//目标速度比值，会乘以MaxMoveSpeed作为实际速度

    public Animator animator => _animator;

    public void Init(Transform HomeTransform)
    {
        this.HomeTransform = HomeTransform;
    }

    public virtual void Start()
    {
        _meshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        _material = _meshRenderer.material;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();
    }

    public virtual void Update()
    {
        if (isDead) return;

        UpdateCurrentSpeed();

        OnSleeping();
    }

    #region OnUpdate
    /// <summary>
    /// 更新当前速度，平滑过渡到目标速度
    /// </summary>
    protected virtual void UpdateCurrentSpeed()
    {
        this.animator.SetFloat(EnemyAnimationConfig.Parameters.Speed, _targetSpeedRatio, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// 正在睡觉
    /// </summary>
    public virtual void OnSleeping()
    {
        if (!isSleeping) return;
        Hp += Time.deltaTime * HPRegenerationSpeed;
        isSleeping = !isFighting && Hp < MaxHp;
        if (!isSleeping)
        {
            animator.SetBool(EnemyAnimationConfig.Parameters.isSleeping, false);// 播放睡觉动画
        }
    }
    #endregion

    #region Actions
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
        animator.SetBool(EnemyAnimationConfig.Parameters.isSleeping, true);// 播放睡觉动画
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
        isDragonShouting = true;
    }

    /// <summary>
    /// 投射物攻击
    /// </summary>
    public virtual void ProjectileAttack()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.ProjectileAttack);// 播放投射物攻击动画
        isAttacking = true;
    }

    /// <summary>
    /// 近战攻击 从近战攻击里随机一个
    /// </summary>
    public virtual void MeleeAttack()
    {
        animator.SetTrigger(EnemyAnimationConfig.Parameters.MeleeAttack);// 播放近战攻击动画
        isAttacking = true;

        int randomIndex = Random.Range(0, EnemyAnimationConfig.FatFatDragonSettings.FatFatDragonMeleeAttackList.Count);
        animator.SetTrigger(EnemyAnimationConfig.FatFatDragonSettings.FatFatDragonMeleeAttackList[randomIndex]);// 播放随机的近战攻击动画
    }

    /// <summary>
    /// 搜索目标是否在视野内
    /// </summary>
    /// <param name="awarenessRadius">查找自身范围内awarenessRadius米</param>
    /// <param name="viewAngle">扇形范围内viewAngle度</param>
    /// <param name="viewDistance">扇形范围内viewDistance米</param>
    /// <param name="searchTargetPos">搜寻目标位置</param>
    /// <returns></returns>
    public bool SearchSomething(float awarenessRadius, float viewAngle, float viewDistance, Vector3 searchTargetPos)
    {

        return false;
    }
    #endregion

    #region Combat
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hitPoint">受击位置，传入shader</param>
    public virtual void TakeDamage(float damage, float poiseDamage, Vector3 hitPoint)
    {
        Hp -= damage;
        Poise -= poiseDamage;
        _material.SetVector("_HitPos", hitPoint);
        if (_jitterCoroutine != null)
        {
            StopCoroutine(_jitterCoroutine);
        }
        _jitterCoroutine = StartCoroutine(HitJitter());
    }
    #endregion

    #region AnimationEvents
    /// <summary>
    /// 攻击结束
    /// </summary>
    private void AE_AtkOver()
    {
        isAttacking = false;
    }

    /// <summary>
    /// 破韧结束
    /// </summary>
    private void AE_DownedOver()
    {
        isDowned = false;
        Poise = MaxPoise;
    }

    /// <summary>
    /// 龙吼结束
    /// </summary>
    public virtual void AE_DragonShoutOver()
    {
        isDragonShouting = false;
    }
    #endregion

    #region AI_navMeshAgent
    /// <summary>
    /// 设置目标速度比值
    /// </summary>
    /// <param name="speedRatio"></param>
    public virtual void SetTargetSpeed(float speedRatio)
    {
        _targetSpeedRatio = speedRatio;
    }

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

    #region Shader
    /// <summary>
    /// 往受击抖动shader传参开始抖动
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitJitter()
    {
        _material.SetFloat("_HitStrength", JitterScale);
        float timer = 0;
        while (timer < Duration)
        {
            timer += Time.deltaTime;
            float progress = timer / Duration;
            float val = Mathf.Lerp(JitterScale, 0, progress);
            _material.SetFloat("_HitStrength", val);
            yield return null;
        }
    }
    #endregion
}
