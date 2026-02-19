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
        // "刀光效果更新，使用MyTrailRenderer预制体渲染，在动画事件控制生成或销毁，需要保证世界坐标为000，需要调用InitMyTrailRenderer传入首尾位置",
        //"计时器",
        //"跑步状态用计时器优化",
        //"跳跃和空中状态"
        //"角色躲避",
        "冲刺斩击",
        "跳跃劈砍",
        "主角攻击优化",
        "角色攻击评分系统"
    };

    string Last = "角色躲避";
    string Doing = "冲刺斩击";

}
