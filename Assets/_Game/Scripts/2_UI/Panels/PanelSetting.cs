using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CameraManager;

public class PanelSetting : MonoBehaviour
{
    public Button btnChangeOcclusionMode;
    public Button btnChangehideWeapon;
    public Button btnClose;

    private Text txt_BtnChangeOcclusionMode;
    private Text txt_BtnChangehideWeapon;


    private void Awake()
    {
        txt_BtnChangeOcclusionMode = btnChangeOcclusionMode.GetComponentInChildren<Text>();
        txt_BtnChangehideWeapon = btnChangehideWeapon.GetComponentInChildren<Text>();
    }

    //private void OnEnable()
    //{

    //}

    void Start()
    {
        btnChangeOcclusionMode.onClick.AddListener(OnbtnChangeOcclusionModeClick);
        btnChangehideWeapon.onClick.AddListener(OnbtnChangehideWeapon);

        btnClose.onClick.AddListener(OnbtnClose);

        txt_BtnChangeOcclusionMode.text = GameManager.Instance.cameraManager.currentMode.ToString();
        txt_BtnChangehideWeapon.text = GameManager.Instance.playerController.weaponController.hideWeapon.ToString();

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        btnChangeOcclusionMode.onClick.RemoveListener(OnbtnChangeOcclusionModeClick);
        btnChangehideWeapon.onClick.RemoveListener(OnbtnChangehideWeapon);

        btnClose.onClick.AddListener(OnbtnClose);
    }

    /// <summary>
    /// 遮挡剔除模式切换按钮
    /// </summary>
    void OnbtnChangeOcclusionModeClick()
    {
        GameManager.Instance.cameraManager.currentMode = 
            GameManager.Instance.cameraManager.currentMode == CameraManager.OcclusionMode.CameraCollision ?
            CameraManager.OcclusionMode.ShaderCutout : CameraManager.OcclusionMode.CameraCollision;
        txt_BtnChangeOcclusionMode.text = GameManager.Instance.cameraManager.currentMode.ToString();
    }

    /// <summary>
    /// 开关隐藏武器按钮
    /// </summary>
    void OnbtnChangehideWeapon()
    {
        GameManager.Instance.playerController.weaponController.hideWeapon = !GameManager.Instance.playerController.weaponController.hideWeapon;
        txt_BtnChangehideWeapon.text = GameManager.Instance.playerController.weaponController.hideWeapon.ToString();
    }

    /// <summary>
    /// 关闭按钮
    /// </summary>
    void OnbtnClose()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
