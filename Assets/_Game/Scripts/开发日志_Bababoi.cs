using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 开发日志_Bababoi
{
    void 高亮显示()
    {
        toDoList[0] = Last + Doing;
    }

    List<string> toDoList = new List<string>()
    {
        //"武器的刀光效果：需要在动画事件中控制TrailRenderer的开启和关闭",
        "缓存池与计时器",
        "任务中心",
        "状态切换优化",
        "跳跃和空中状态"
    };

    string Last = "完成了武器的刀光效果：需要在动画事件中控制TrailRenderer的开启和关闭";
    string Doing = "缓存池与计时器";

}
