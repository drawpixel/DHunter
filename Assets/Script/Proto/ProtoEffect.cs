using System.Collections;
using System.Collections.Generic;


public enum EffectExistType
{
    Once,
    Caster,
    Target,
}
public enum EffectExcludeType
{
    Replace,
    Add,
    Refuse,
}
public class ProtoEffect : ProtoBase
{
    public float Probability;
    public EffectExistType Exist;
}
public class ProtoLastDamage : ProtoEffect
{
    public float LastTime;
    public float Interval;
    public float Damage;
}
