using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    public void OnUpdate()
    {
        //广播玩家位置给全局shader 所有shader声明了_PlayerPos变量都能用
        if (GameManager.Instance.playerController != null)
        {
            Shader.SetGlobalVector("_PlayerPos", GameManager.Instance.playerController.LookPos.position);
        }
    }
}
