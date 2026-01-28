using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 事件中心模块  
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //用于记录对应事件 关联的 对应的逻辑
    private Dictionary<string, UnityAction> eventDic = new Dictionary<string, UnityAction>();
   
    private EventCenter() { }
     /// <summary>
     /// 触发事件
     /// </summary>
     /// <param name="eventName">事件名字</param>
    public void EventTrigger(string eventName)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName]?.Invoke();
        }
    }
    /// <summary>
    /// 添加事件监听者
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void AddEventListner(string eventName, UnityAction func)
    {
        if (eventDic.ContainsKey(eventName))
            eventDic[eventName]+=func;
        else
            eventDic.Add(eventName, new UnityAction(func)) ; 
    }
    /// <summary>
    /// 移除事件监听者
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public void RemoveEventListner(string eventName, UnityAction func)
    {
        if (eventDic.ContainsKey(eventName)) 
            eventDic[eventName] -= func;
    }
    public void Clear()
    {
        eventDic.Clear();
    }
    public void clear(string eventName)
    {
        if(eventDic.ContainsKey(eventName))
            eventDic.Remove(eventName);
    }
}
