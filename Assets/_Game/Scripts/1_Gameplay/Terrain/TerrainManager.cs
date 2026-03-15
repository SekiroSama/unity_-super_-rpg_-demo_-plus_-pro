using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager
{
    public MeshRenderer[] meshRenderers;
    private Material[] materials;

    public void Init(MeshRenderer[] meshRenderers)
    {
        this.meshRenderers = meshRenderers;
    }

    public void onAwake()
    {
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            materials = meshRenderers[i].materials;
        }
    }

    /// <summary>
    /// set遮挡裁剪半径工作与否
    /// </summary>
    /// <param name="isWork"></param>
    public void SetClipRadiusWork(bool isWork)
    {
        foreach (Material material in materials)
        {
            material.SetFloat("_ClipRadius", isWork ? 0.5f : 0f);
        }
    }
}
