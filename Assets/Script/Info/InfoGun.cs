using UnityEngine;
using System.Collections;

public class InfoGun : InfoBase
{
    ProtoGun m_proto_gun;
    public ProtoGun Proto
    {
        get { return m_proto_gun; }
        
    }

    public double Rank;
    public int Star;
    public int Quality;

    public InfoGun(ProtoBase proto) : base(proto)
    {
        m_proto_gun = (proto as ProtoGun);
    }
}

