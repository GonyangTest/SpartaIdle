using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    Main,
    Inventory,
    Shop,
    Skill,
    Setting,
    Result,
    GameOver,
    ToastMessage,
}

public abstract class BaseUI : MonoBehaviour
{
    public UIType UIType;

    public virtual void OnOpen(OpenParam param = null) {}
    public virtual void OnOpen() {}
    public virtual void OnClose() {}
}