using System;

[Serializable]
public class WeaponInstance : ItemInstance
{
    private int _reinforceLevel;

    public WeaponInstance(int id, int quantity = 0, int reinforceLevel = 0) : base(id, quantity)
    {
        _reinforceLevel = reinforceLevel;
    }

    public WeaponInstance(GenericItemDataSO itemDataSO, int quantity = 0, int reinforceLevel = 0) : base(itemDataSO, quantity)
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