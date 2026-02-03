using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    public enum OcclusionMode
    {
        CameraCollision,
        ShaderCutout,
    }

    private OcclusionMode _currentMode;
    public OcclusionMode currentMode 
    {
        get => _currentMode;
        set
        {
            if (_currentMode == value) return;
            _currentMode = value;
            GameManager.Instance.terrainManager.SetClipRadiusWork(_currentMode == OcclusionMode.ShaderCutout);
            OnChangeOcclusionMode();
        }
    }
    public CinemachineFreeLook camFreeLook;
    public CinemachineCollider camCollider;

    public void Init(CinemachineCollider camCollider, CinemachineFreeLook camFreeLook)
    {
        this.camCollider = camCollider;
        this.camFreeLook = camFreeLook;
    }


    public void OnStart()
    {
        currentMode = OcclusionMode.CameraCollision;
        GameManager.Instance.terrainManager.SetClipRadiusWork(_currentMode == OcclusionMode.ShaderCutout);//设置地形遮挡裁剪

#if UNITY_ANDROID && !UNITY_EDITOR
        camFreeLook.m_XAxis.m_InputAxisName = "";
        camFreeLook.m_YAxis.m_InputAxisName = "";
#endif
    }

    public void OnUpdate()
    {

    }

    private void OnChangeOcclusionMode()
    {
        //广播玩家位置给全局shader 所有shader声明了_PlayerPos变量都能用
        if (GameManager.Instance.playerController != null)
        {
            switch (_currentMode)
            {
                case OcclusionMode.CameraCollision:
                    camCollider.enabled = true;
                    //Shader.SetGlobalVector("_PlayerPos", new Vector3(0, -10000, 0));
                    break;
                case OcclusionMode.ShaderCutout:
                    camCollider.enabled = false;
                    Shader.SetGlobalVector("_PlayerPos", GameManager.Instance.playerController.LookPos.position);
                    break;
            }
        }
    }
}
