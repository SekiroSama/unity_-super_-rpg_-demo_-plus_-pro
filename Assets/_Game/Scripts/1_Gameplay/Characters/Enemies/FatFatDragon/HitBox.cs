using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    /// <summary>
    /// Åö×²Âß¼­
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController.TakeDamage();
            print("TakeDamage");
        }
    }
}
