using System.Collections;

public enum UnityLayer
{
	Default = 0,
	TransparentFX = 1,
	IgnoreRayCast = 2,
	Water = 4,
	UI = 5,

	LockRaycast = 11,
	LockRaycastRude = 12,
	Destroyable = 13,
}

public enum DamageType
{
    Fire,
    Energy,
    All,
}

public class Int2D
{
    public int X = 0;
    public int Y = 0;

    public Int2D()
    {}
    public Int2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Int2D operator +(Int2D s1, Int2D s2)
    {
        return new Int2D(s1.X + s2.X, s1.Y + s2.Y);
    }
    public static Int2D operator -(Int2D s1, Int2D s2)
    {
        return new Int2D(s1.X - s2.X, s1.Y - s2.Y);
    }
}