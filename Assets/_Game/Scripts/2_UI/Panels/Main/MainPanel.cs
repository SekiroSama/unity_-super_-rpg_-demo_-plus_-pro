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

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnDrag");
    //}
}
