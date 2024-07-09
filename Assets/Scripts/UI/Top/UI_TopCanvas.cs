using UnityEngine;

public class UI_TopCanvas : UI_Base
{
    protected override void Init()
    {
        Managers.UI.Register<UI_TopCanvas>(this);
    }
}
