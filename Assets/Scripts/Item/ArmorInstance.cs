using System;

[Serializable]
public class ArmorInstance : ItemInstance
{
    private int _reinforceLevel;

    public ArmorInstance(int id, int quantity = 0, int reinforceLevel = 0) : base(id, quantity)
    {
        _reinforceLevel = reinforceLevel;
    }

    public ArmorInstance(GenericItemDataSO itemDataSO, int quantity = 0, int reinforceLevel = 0) : base(itemDataSO, quantity)
    {
        _reinforceLevel = reinforceLevel;
    }

    public int GetReinforceLevel()
    {
        return _reinforceLevel;
    }

    public int GetAdditionalAttackPower()
    {
        return _reinforceLevel;
    }

    public void Reinforce()
    {
        _reinforceLevel++;
    }
}