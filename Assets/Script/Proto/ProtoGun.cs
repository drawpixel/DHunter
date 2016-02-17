using System.Collections;
using System.Collections.Generic;

public enum SelectType
{
    Front,
    FaceBack,
    AllRandom,
    FixPartern,
}
public enum HitActionType
{
    Once,
    Range,
    Penetrate,
    Jump,
    Stay,
}
public class ProtoGun : ProtoBase
{
    public float ShootInterval = 0.5f;
    public string[] Bullets;

    //public SelectType Select;
    //public Int2D[] Partern;

    //public HitActionType HitAction;
    //public float HitActionParam0;
    //public float HitActionParam1;
    
    
}
