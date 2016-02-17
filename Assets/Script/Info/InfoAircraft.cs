using UnityEngine;
using System.Collections;

public class InfoAircraft : InfoBase
{
    ProtoAircraft m_proto_ac;
    public ProtoAircraft Proto
    {
        get { return m_proto_ac; }
        
    }

    public double Rank;
    public int Star;
    public int Quality;

    public InfoGun[] Guns;

    public InfoAircraft(ProtoBase proto) : base(proto)
    {
        m_proto_ac = (proto as ProtoAircraft);
    }
}

