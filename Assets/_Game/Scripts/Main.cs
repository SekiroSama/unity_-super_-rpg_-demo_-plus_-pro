using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        ResourceManage.Instance.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        //GameObject cube = ResourceManage.Instance.LoadRes<GameObject>("test_obj", "cube");
        //Instantiate(cube);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
