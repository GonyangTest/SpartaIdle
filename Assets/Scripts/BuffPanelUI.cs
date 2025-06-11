using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPanelUI : BaseFixed
{
    private BuffUI[] _buffSlots;
    [SerializeField] private Transform _buffSlotPanel;

    private void Awake()
    {
        _buffSlots = new BuffUI[_buffSlotPanel.childCount];
        for(int i = 0; i < _buffSlotPanel.childCount; i++)
        {
            _buffSlots[i] = _buffSlotPanel.GetChild(i).GetComponent<BuffUI>();
        }

        RefreshBuffPanel(new List<Buff>());
    }

    private void OnEnable()
    {
        PlayerBuffManager.Instance.OnBuffDisplayUpdated += RefreshBuffPanel;
    }

    private void OnDisable()
    {
        PlayerBuffManager.Instance.OnBuffDisplayUpdated -= RefreshBuffPanel;
    }

    public void RefreshBuffPanel(List<Buff> buffs)
    {
        for(int i = 0; i < _buffSlots.Length; i++)
        {
            if(i < buffs.Count)
            {
                _buffSlots[i].SetBuff(buffs[i]);
            }
            else
            {
                _buffSlots[i].ClearBuff();
            }
        }
    }
}
