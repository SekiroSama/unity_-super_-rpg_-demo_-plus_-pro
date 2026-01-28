
using System.Collections.Generic;
using UnityChan;

public class 开发日志_Sekiro
{
    void 高亮显示()
    {
        toDoList[0] = Last + Doing; 
    }

    List<string> toDoList = new List<string>()
    {
        "预制体回放在Resources_Build文件夹下",
        "武器动态生成，位置可以用现在调好的子节点位置",
        "武器的刀光效果：需要在动画事件中控制TrailRenderer的开启和关闭",
    };


    string Last = "完成了武器的刀光基本效果。";
    string Doing = "用插值顶点让刀光变圆";


}


