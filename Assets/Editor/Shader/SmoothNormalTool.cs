using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmoothNormalTool
{
    [MenuItem("Tool/Bake Smooth Normals")]
    public static void Bake()
    {
        // 1. 获取当前选中的物体
        GameObject selectedObject = Selection.activeGameObject;
        if(selectedObject == null)
        {
            Debug.LogError("请先选中场景里的物体！");
            return;
        }

        // 2. 获取网格过滤器
        MeshFilter meshFilter = selectedObject.GetComponent<MeshFilter>();
        if(meshFilter == null)
        {
            Debug.LogError("选中的物体没有MeshFilter组件！");
            return;
        }

        // 3. 复制一份网格 (千万别直接改源文件，否则可能会改坏模型)
        Mesh mesh = Object.Instantiate(meshFilter.sharedMesh);
        mesh.name = meshFilter.sharedMesh.name + "_SmoothNormal";

        // 4. 用来把重叠顶点的法线加在一起
        Dictionary<Vector3, Vector3> averageNormals = new Dictionary<Vector3, Vector3>();

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        // A. 遍历所有顶点，累加法线
        for (int i = 0; i < vertices.Length; i++)
        {
            if(!averageNormals.ContainsKey(vertices[i]))
            {
                averageNormals[vertices[i]] = normals[i];
            }
            else
            {
                averageNormals[vertices[i]] += normals[i];
            }
        }

        // B. 归一化（取平均值）
        // 此时字典里存的就是完美的平滑法线了
        foreach (var key in new List<Vector3>(averageNormals.Keys))
        {
            averageNormals[key] = averageNormals[key].normalized;
        }

        // C. 把平滑法线存进 Tangent (切线) 通道
        // Tangent 是 float4，我们把 xyz 存法线，w 存 0
        Vector4[] tangents = new Vector4[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 smoothNormal = averageNormals[vertices[i]];
            tangents[i] = new Vector4(smoothNormal.x, smoothNormal.y, smoothNormal.z, 0f);
        }

        // 应用修改
        mesh.tangents = tangents;

        // 5. 保存为文件 (Assets目录下)
        // 确保文件名不重复
        string path = "Assets/" + mesh.name + ".asset";
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();

        // 6. 自动把新模型赋给当前物体
        meshFilter.sharedMesh = mesh;
        Debug.Log("平滑法线烘焙完成，已保存为 " + path);
    }
}
