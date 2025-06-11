using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUI : MonoBehaviour
{
    public TextMeshProUGUI RemainingTimeText;
    public Image IconImage;

    public void SetBuff(Buff buff)
    {
        gameObject.SetActive(true);
        IconImage.sprite = buff.Effect.EffectIcon;
        RemainingTimeText.text = ((int)buff.RemainingTime).ToString();
    }

    public void ClearBuff()
    {
        gameObject.SetActive(false);
        IconImage.sprite = null;
        RemainingTimeText.text = "";
    }
}
