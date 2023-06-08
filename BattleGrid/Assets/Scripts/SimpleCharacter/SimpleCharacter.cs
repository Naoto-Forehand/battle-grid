using System;

[Serializable]
public class SimpleCharacter
{
    public int Health { get { return _health; } }
    private int _health;

    public int Attack { get { return _attack; } }
    private int _attack;

    public SimpleCharacter(int health, int attack)
    {
        _health = health;
        _attack = attack;
    }
}
