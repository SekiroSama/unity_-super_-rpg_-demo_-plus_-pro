using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HP = 100;
    public float duration;//抖动时间
    public float JitterScale = 0.1f;//抖动幅度

    private SkinnedMeshRenderer meshRenderer;
    private Material material;
    private Coroutine _jitterCoroutine;//抖动协程引用

    private void Start()
    {
        meshRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        material = meshRenderer.material;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hitPoint">受击位置，传入shader</param>
    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        HP -= damage;
        material.SetVector("_HitPos", hitPoint);

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
        material.SetFloat("_HitStrength", JitterScale);
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float val = Mathf.Lerp(JitterScale, 0, progress);
            material.SetFloat("_HitStrength", val);
            yield return null;
        }
    }
}
