using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽屉对象
/// </summary>
public class PoolData
{
    //用来存储抽屉中的对象
    private Stack<GameObject> dataStack;
    
    //根节点
    private GameObject rootObj; 

    public int Count=>dataStack.Count;
    /// <summary>
    /// 初始化构造函数
    /// </summary>
    /// <param name="root">缓存池父对象</param>
    /// <param name="name">缓存池父对象的名字</param>
    public PoolData(GameObject root,string name)
    {

        //将布局优化设置为一个可开启选项
        if (PoolMgr.isOpenLayout)
        {
            rootObj = new GameObject(name);
            rootObj.transform.SetParent(root.transform);
        }
    }
    public GameObject Pop()
    {
        GameObject obj = dataStack.Pop();
        //激活
        obj.SetActive(true);
        if (PoolMgr.isOpenLayout)
            //断开父子关系
            obj.transform.parent = null;

        return obj;
    }
    public void Push(GameObject obj)
    {
        //失活放入抽屉的对象
        obj.SetActive(false);
        if (PoolMgr.isOpenLayout)
            //放入抽屉的根物体中建立父子关系
            obj.transform.SetParent(rootObj.transform);
        //通过栈记录对应的对象数据
        dataStack.Push(obj);
    }
}
/// <summary>
/// 缓存池管理器
/// </summary>
public class PoolMgr : BaseManager<PoolMgr> 
{
    public Dictionary<string,PoolData> poolDic = new Dictionary<string,PoolData>();
    private PoolMgr() { }

    private GameObject poolObj;

    public static bool isOpenLayout = true;
    /// <summary>
    /// 从缓存池中取出对象，相当于创建对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
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
            obj.name = name;
        }
        return obj;
    }
    /// <summary>
    /// 往缓存池中放入对象，等同于销毁对象
    /// </summary>
    /// <param name="obj">希望放入的对象</param>
    public void PushObj(GameObject obj)
    {
        if(poolObj==null&&isOpenLayout)
        {
            poolObj = new GameObject("Pool");
        }
        //obj.SetActive(false);

        //obj.transform.SetParent(poolObj.transform);
        if(!poolDic.ContainsKey(obj.name))
            poolDic.Add(obj.name, new PoolData(poolObj, obj.name));
        poolDic[obj.name].Push(obj);
    }
    public void ClearPool()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
