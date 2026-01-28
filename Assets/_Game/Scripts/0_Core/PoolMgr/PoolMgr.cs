using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// »ù´¡»º´æ³Ø
/// </summary>
public class PoolMgr : BaseManager<PoolMgr> 
{
    public Dictionary<string,Stack<GameObject>> poolDic = new Dictionary<string,Stack<GameObject>>();
    private PoolMgr() { }
    public GameObject GetObj(string name)
    {
        GameObject obj;
        if(poolDic.ContainsKey(name) && poolDic[name].Count>0)
        {
            obj = poolDic[name].Pop();
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        }
        return obj;
    }
    public void PushObj(string name,GameObject obj)
    {
        obj.SetActive(false);
        if(!poolDic.ContainsKey(name))
            poolDic.Add(name, new Stack<GameObject>());
        poolDic[name].Push(obj);
    }
    public void ClearPool()
    {
        poolDic.Clear();
    }
}
