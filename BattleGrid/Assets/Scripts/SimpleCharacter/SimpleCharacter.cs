using System;

[Serializable]
public class SimpleCharacter
{
    public int Health { get { return _health; } }
    private int _health;

    public int Attack { get { return _attack; } }
    private int _attack;

    public int Defense { get { return _defense; } }
    private int _defense;

    public SimpleCharacter(int health, int attack, int defense)
    {
        _health = health;
        _attack = attack;
        _defense = defense;
    }
}
