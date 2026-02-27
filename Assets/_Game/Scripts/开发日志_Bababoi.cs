using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 开发日志_Bababoi
{
    void 高亮显示()
    {
        BababoiTODoList[0] = null;
        SekiroTODoList[0] = null ;
        BababoiDone[0] = null ; 
    }
    List<string> BababoiDone = new List<string>()
    {
        //"武器的刀光效果：需要在动画事件中控制TrailRenderer的开启和关闭",
        // "刀光效果更新，使用MyTrailRenderer预制体渲染，在动画事件控制生成或销毁，需要保证世界坐标为000，需要调用InitMyTrailRenderer传入首尾位置",
        //  "优化刀光接口isEmitting,可以直接MyTrailRenderer.isEmitting",
        //"计时器",
        //"跑步状态用计时器优化",
        //"跳跃和空中状态"
        //"角色躲避",
        //"冲刺斩击",
        //"主角攻击优化",
    };
    List<string> BababoiTODoList = new List<string>()
    {
       
      
        "找了个刀的命中特效，合适的话可以用上",
        "角色攻击评分系统",
        //角色评分系统可能需要一个效果，可以想一下怎么做
        //蓄力斩的意思特殊动作完成后可以进行的斩击，斩击可以增加角色攻击评分
        //蓄力斩条件: 
        //1、闪避后按攻击键，蓄力斩
        //2、格挡弹反接收到Boss的攻击，蓄力斩
        //3、攻击三次后,原地蓄力斩
    };
    List<string> SekiroTODoList = new List<string>()
    {
        "现在需要闪避的一个残影，按住Ctrl角色会向后闪避一段距离，这个是直接用的CC的Move方法进行强制移动的",
        "蓄力斩需要一个特殊的刀光，换个颜色都可以"
    };

    string Last = "冲刺斩击";
    string Doing = "角色攻击评分系统";

}
