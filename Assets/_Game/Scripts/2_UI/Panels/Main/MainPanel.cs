using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public GameObject PanelSetting;

    public Button btnSetting;
    public Button btnATK;
    public Text txtRank;
    public Vector2 txtRank_Offset;

    //public Image imgJoystickBackground;
    //public Image imgJoystickHandle;
    // Start is called before the first frame update
    void Start()
    {
        btnSetting.onClick.AddListener(OnbtnSetting);
        btnATK.onClick.AddListener(OnbtnATK);
    }

    void OnbtnSetting()
    {
        PanelSetting.SetActive(true);
    }

    void OnbtnATK()
    {
        GameManager.Instance.inputManager.uibtnAttackPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(GameManager.Instance.playerController != null)
        {
            txtRank.rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.playerController.LookPos.position);
        }

        txtRank.rectTransform.position = new Vector3(txtRank.rectTransform.position.x + txtRank_Offset.x, txtRank.rectTransform.position.y + txtRank_Offset.y, txtRank.rectTransform.position.z);
    }

    public void ChangeCCRank(string rank)
    {
        txtRank.text = rank;
    }
}
