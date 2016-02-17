using System.Collections;
using System.Collections.Generic;

public class ProtoAircraft : ProtoBase
{
    public int HP;
    public int Armor;
    public int GrowthArmor;
    public int FireAP;
    public int EnergyAP;
    public int Crit;
    public int AntiCrit;

    public Int2D Dim;
    public int[] Occupies;
    public int[] Pattern;

    public int[] Guns;

    ProtoGun[] m_guns = null;
    public ProtoGun[] ProtoGuns
    {
        get
        {
            return m_guns;
        }
    }

    public override void Create()
    {
        m_guns = new ProtoGun[Guns.Length];
        for (int i = 0; i < m_guns.Length; ++ i)
        {
            m_guns[i] = ProtoMgr.Instance.GetByID<ProtoGun>(Guns[i]);
        }
    }
}
