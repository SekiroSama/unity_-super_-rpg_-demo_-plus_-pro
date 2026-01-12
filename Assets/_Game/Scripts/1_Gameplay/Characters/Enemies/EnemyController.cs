using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HP = 100;
    MeshRenderer meshRenderer;
    Material material;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }


    public void TakeDamage(int damage, Vector3 hitPoint)
    {
        HP -= damage;
        material.SetVector("_HitPoint", hitPoint);
        StartCoroutine("");
    }
}
