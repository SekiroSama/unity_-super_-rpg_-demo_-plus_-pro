using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Button btnSetting;
    public GameObject PanelSetting;

    // Start is called before the first frame update
    void Start()
    {
        btnSetting.onClick.AddListener(OnbtnSetting);
    }

    void OnbtnSetting()
    {
        PanelSetting.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
