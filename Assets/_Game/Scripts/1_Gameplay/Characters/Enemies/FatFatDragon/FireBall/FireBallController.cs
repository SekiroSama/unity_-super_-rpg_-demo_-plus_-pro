using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public float MoveSpeed = 10f;

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
        Explosion();
    }

    /// <summary>
    /// 碰到后爆炸
    /// </summary>
    private void Explosion()
    {

    }
}
