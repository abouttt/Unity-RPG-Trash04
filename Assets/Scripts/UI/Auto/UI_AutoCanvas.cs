using UnityEngine;

public class UI_AutoCanvas : UI_Base
{
    protected override void Init()
    {
        Managers.UI.Register<UI_AutoCanvas>(this);
    }
}
