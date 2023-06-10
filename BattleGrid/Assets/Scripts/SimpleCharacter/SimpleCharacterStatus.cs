using System;

[Flags]
public enum SimpleCharacterStatus : short
{
    NONE = 0,
    WEAKENED = 1,
    STAGGERED = 2,
    INVIGORATED = 4,
    CRAZED = 8,
    OVERREACH = 16,
    SWAGGERRING = 32,
    DEAD = 64
}
