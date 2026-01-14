using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    List<int> whiteList = new List<int>();

    private void Start()
    {

    }

    public void OpenCollider()
    {
        this.GetComponent<Collider>().enabled = true;
        whiteList.Clear();
    }

    public void CloseCollider()
    {
        this.GetComponent<Collider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            int id = other.gameObject.GetInstanceID();
            if (!whiteList.Contains(id))
            {
                EnemyController enemyController = other.GetComponentInParent<EnemyController>();
                enemyController.TakeDamage(10, other.ClosestPoint(transform.position));
                whiteList.Add(id);
            }
        }
    }
}

