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
    private Material FX_RadialBlur_FullScreen_Material;//径向模糊材质球


    public void Init(CinemachineCollider camCollider, CinemachineFreeLook camFreeLook, Material FX_RadialBlur_FullScreen_Material)
    {
        this.camCollider = camCollider;
        this.camFreeLook = camFreeLook;
        this.FX_RadialBlur_FullScreen_Material = FX_RadialBlur_FullScreen_Material;
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
        Shader.SetGlobalVector("_PlayerPos", GameManager.Instance.playerController.LookPos.position);
    }

    int lastTimerId = -1;
    /// <summary>
    /// 开启径向模糊
    /// </summary>
    /// <param name="durTime">持续时间ms</param>
    /// <param name="BlurStrength">模糊强度 0.05左右</param>
    public void RadialBlurStart(int durTime, float BlurStrength = 0.05f)
    {
        FX_RadialBlur_FullScreen_Material.SetFloat("_BlurStrength", BlurStrength);
        if(lastTimerId != -1)
        {
            TimerMgr.Instance.RemoveTimer(lastTimerId);
        }
        lastTimerId = TimerMgr.Instance.CreateTimer(false, durTime, RadialBlurEnd);
    }

    /// <summary>
    /// 关闭径向模糊
    /// </summary>
    private void RadialBlurEnd()
    {
        FX_RadialBlur_FullScreen_Material.SetFloat("_BlurStrength", 0);
    }

    /// <summary>
    /// 切换剔除模式
    /// </summary>
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
