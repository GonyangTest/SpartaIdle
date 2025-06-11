using UnityEngine;

public class BaseWindow : BaseUI
{
    public override void OnOpen(OpenParam param)
    {
            UIManager.Instance.CloseAllWindows();

        base.OnOpen(param);
    }
}