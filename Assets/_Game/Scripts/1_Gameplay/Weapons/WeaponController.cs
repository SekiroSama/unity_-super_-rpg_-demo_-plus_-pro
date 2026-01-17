using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    List<int> whiteList = new List<int>();
    private Coroutine dissolveCoroutine;//设置武器溶解效果协程
    public bool hideWeapon = false;

    private void Start()
    {

    }

    public void OpenCollider()
    {
        this.GetComponent<Collider>().enabled = true;
        whiteList.Clear();
    }

    public void CloseCollider()
    {
        this.GetComponent<Collider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!whiteList.Contains(id))
            {
                EnemyController enemyController = other.GetComponentInParent<EnemyController>();
                enemyController.TakeDamage(10, other.ClosestPoint(transform.position));
                whiteList.Add(id);
            }
        }
    }

    /// <summary>
    /// 动画事件：武器碰撞开启
    /// </summary>
    public void WeaponOn()
    {
        this.OpenCollider();
    }

    /// <summary>
    /// 动画事件：武器碰撞关闭
    /// </summary>
    public void WeaponOff()
    {
        this.CloseCollider();
    }

    /// <summary>
    /// 动画事件：开始显示武器
    /// </summary>
    public void WeaponRedissolveValStart()
    {
        if (dissolveCoroutine != null)
            StopCoroutine(dissolveCoroutine);
        dissolveCoroutine = StartCoroutine(SetWeaponDissolveVal(0));
    }

    /// <summary>
    /// 动画事件：开始溶解隐藏武器
    /// </summary>
    public void WeaponDissolveValStart()
    {
        if (dissolveCoroutine != null)
            StopCoroutine(dissolveCoroutine);
        dissolveCoroutine = StartCoroutine(SetWeaponDissolveVal(1, 1f));
    }

    /// <summary>
    /// 设置武器溶解效果
    /// </summary>
    /// <param name="_DissolveVal">目标溶解值</param>
    /// <returns></returns>
    private IEnumerator SetWeaponDissolveVal(float targetDissolveVal, float transitionDuration = 0.1f)
    {
        if (hideWeapon)
        {
            float dissolveVal = Shader.GetGlobalFloat("_DissolveVal");
            float dissolveTimer = 0f;
            while (dissolveTimer < transitionDuration)
            {
                dissolveTimer += Time.deltaTime;
                Shader.SetGlobalFloat("_DissolveVal", Mathf.Lerp(dissolveVal, targetDissolveVal, dissolveTimer / transitionDuration));
                yield return null;
            }
            Shader.SetGlobalFloat("_DissolveVal", targetDissolveVal);
        }
        else
        {
            Shader.SetGlobalFloat("_DissolveVal", 0);
        }
    }
}

