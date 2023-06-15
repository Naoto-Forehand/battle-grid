using System;
using UnityEngine;

[Serializable]
public class SimpleCharacter
{
    public string Name { get { return _name; } }
    [SerializeField]
    private string _name;

    public int Health { get { return _health; } }
    [SerializeField]
    private int _health;

    public int Attack { get { return _attack; } }
    [SerializeField]
    private int _attack;

    public int Defense { get { return _defense; } }
    [SerializeField]
    private int _defense;

    public SimpleCharacterStatus Status { get { return _status; } }
    private SimpleCharacterStatus _status;

    private int? _cachedDamage = null;
    private int? _cachedDefense = null;

    public SimpleCharacter(SimpleCharacterFacade simpleCharacter)
    {
        _name = simpleCharacter.Name;
        _health = simpleCharacter.Health;
        _attack = simpleCharacter.Attack;
        _defense = simpleCharacter.Defense;
        SetStatus(SimpleCharacterStatus.NONE);
    }

    public void CacheDamage(int incomingDamage)
    {
        _cachedDamage = incomingDamage;
    }

    public void CacheDefense(int defenseBoost)
    {
        _cachedDefense = defenseBoost;
    }

    public void HandleReceivingDamage()
    {
        // TODO: Calculate damage received and remove appropriately
        if (_cachedDamage.HasValue)
        {
            // TODO: More complex calculation here, for now simple
            var totalDefense = (_cachedDefense.HasValue) ? _defense + _cachedDefense.Value : _defense;
            _health -= (_cachedDamage.Value - totalDefense);
            if (_health <= 0)
            {
                SetStatus(SimpleCharacterStatus.DEAD);
            }
            _cachedDamage = null;
            _cachedDefense = null;
        }
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
