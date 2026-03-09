using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public GameObject ExplosionPrefab;

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    /// <summary>
    /// 每帧向前运动
    /// </summary>
    private void Moving()
    {
        this.transform.Translate(this.transform.forward * MoveSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// 碰撞逻辑
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Terrain"))
        {
            Explosion();
        }
    }

    /// <summary>
    /// 碰到后爆炸
    /// </summary>
    private void Explosion()
    {
        GameObject explosion = Instantiate(ExplosionPrefab);
        explosion.transform.position = this.transform.position;
    }
}
