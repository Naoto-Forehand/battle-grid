using System;

[Serializable]
public class SimpleCharacter
{
    public string Name { get { return _name; } }
    private string _name;

    public int Health { get { return _health; } }
    private int _health;

    public int Attack { get { return _attack; } }
    private int _attack;

    public int Defense { get { return _defense; } }
    private int _defense;

    public SimpleCharacterStatus Status { get { return _status; } }
    private SimpleCharacterStatus _status;

    public SimpleCharacter(SimpleCharacterFacade simpleCharacter)
    {
        _name = simpleCharacter.Name;
        _health = simpleCharacter.Health;
        _attack = simpleCharacter.Attack;
        _defense = simpleCharacter.Defense;
        SetStatus(SimpleCharacterStatus.NONE);
    }

    public void HandleReceivingDamage(int incomingDamage)
    {
        // TODO: Calculate damage received and remove appropriately
    }

    public void HandleDealingDamage(Action<int> damageReceiver)
    {
        // TODO: Calculate damage to send
    }

    public void SetStatus(SimpleCharacterStatus flag)
    {
        _status ^= flag;
        if ((_status & SimpleCharacterStatus.DEAD) != 0)
        {
            _status &= ~SimpleCharacterStatus.WEAKENED;
            _status &= ~SimpleCharacterStatus.STAGGERED;
            _status &= ~SimpleCharacterStatus.INVIGORATED;
            _status &= ~SimpleCharacterStatus.CRAZED;
            _status &= ~SimpleCharacterStatus.OVERREACH;
            _status &= ~SimpleCharacterStatus.SWAGGERRING;
        }
    }

    public void UnSetStatus(SimpleCharacterStatus flag)
    {
        _status &= ~ flag;
    }
}
