using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class MsgCenter
{
    public delegate void DgtAircraftCreate(InfoCreature info, int inst_id);
    public static DgtAircraftCreate OnAircraftCreate = null;

    public delegate void DgtAircraftTakeDamage(int inst_id, float damage);
    public static DgtAircraftTakeDamage OnAircraftTakeDamage = null;

    public delegate void DgtAircraftDead(int inst_id);
    public static DgtAircraftDead OnAircraftDead = null;

    public delegate void DgtAircraftShoot(int inst_id, int idx_gun, Int2D target);
    public static DgtAircraftShoot OnAircraftShoot = null;
}

