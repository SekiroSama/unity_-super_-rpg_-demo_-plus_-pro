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

    private void Moving()
    {
        this.transform.Translate(this.transform.forward * MoveSpeed * Time.deltaTime, Space.World);
    }
}
