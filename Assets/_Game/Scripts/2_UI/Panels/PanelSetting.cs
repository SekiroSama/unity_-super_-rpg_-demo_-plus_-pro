using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CameraManager;

public class PanelSetting : MonoBehaviour
{
    public Button btnChangeOcclusionMode;
    public Button btnClose;

    private Text txt_BtnChangeOcclusionMode;
    void Start()
    {
        txt_BtnChangeOcclusionMode = btnChangeOcclusionMode.GetComponentInChildren<Text>();
        btnChangeOcclusionMode.onClick.AddListener(OnbtnChangeOcclusionModeClick);
        btnClose.onClick.AddListener(OnbtnClose);
        txt_BtnChangeOcclusionMode.text = GameManager.Instance.cameraManager.currentMode.ToString();
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        btnChangeOcclusionMode.onClick.RemoveListener(OnbtnChangeOcclusionModeClick);
        btnClose.onClick.RemoveListener(OnbtnClose);
    }

    void OnbtnChangeOcclusionModeClick()
    {
        GameManager.Instance.cameraManager.currentMode = 
            GameManager.Instance.cameraManager.currentMode == CameraManager.OcclusionMode.CameraCollision ?
            CameraManager.OcclusionMode.ShaderCutout : CameraManager.OcclusionMode.CameraCollision;
        txt_BtnChangeOcclusionMode.text = GameManager.Instance.cameraManager.currentMode.ToString();
    }

    void OnbtnClose()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
