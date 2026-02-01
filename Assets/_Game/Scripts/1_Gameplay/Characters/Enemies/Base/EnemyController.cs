using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    public int HP = 100;

    [SerializeField]
    private float Duration;//抖动时间
    [SerializeField]
    private float JitterScale = 1f;//抖动幅度

    private SkinnedMeshRenderer _meshRenderer;
    private Material _material;
    private Coroutine _jitterCoroutine;//抖动协程引用
    private NavMeshAgent _navMeshAgent;//导航组件

    public virtual void Start()
    {
        _meshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        _material = _meshRenderer.material;
        _navMeshAgent = _meshRenderer.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 移动到目标位置
    /// </summary>
    public virtual void MoveToTarget(Vector3 targetPos)
    {
        _navMeshAgent.SetDestination(targetPos);
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        _navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hitPoint">受击位置，传入shader</param>
    public virtual void TakeDamage(int damage, Vector3 hitPoint)
    {
        HP -= damage;
        _material.SetVector("_HitPos", hitPoint);

        if(_jitterCoroutine != null)
        {
            StopCoroutine(_jitterCoroutine);
        }
        _jitterCoroutine = StartCoroutine(HitJitter());
    }


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
