using System.Collections;
using System.Collections.Generic;


public class Gun
{
    public delegate void DgtShoot(Bullet[] blts);
    public DgtShoot OnShoot;

    InfoGun m_info;
    public InfoGun Info
    {
        get { return m_info; }
    }

    Aircraft m_ac = null;
    public Aircraft AC
    {
        get { return m_ac; }
    }

    public FightGrid FGrid
    {
        get { return m_ac.FGrid; }
    }
    public FightCtller FCtller
    {
        get { return FGrid.FCtller; }
    }

    public void Create(InfoGun info, Aircraft ac)
	{
        m_info = info;
        m_ac = ac;
	}


    float m_shoot_counter = 0;
    public void Update(float interval)
    {
        m_shoot_counter += interval;
    }


    public bool CanShoot()
    {
        if (m_shoot_counter < Info.Proto.ShootInterval)
            return false;
        return true;
    }
    public void Shoot()
    {
        Int2D[] targets = GetTargets();
        if (targets == null || targets.Length == 0)
            return;

        m_shoot_counter = 0;

        Bullet[] blts = new Bullet[targets.Length];

        for (int i = 0; i < targets.Length; ++ i)
        {
            Int2D idx  = targets[i];
            blts[i] = BulletPool.Instance.Alloc(this, m_ac.FGrid.Dir, GetGridIndex(),
                m_ac.FGrid.Dir == FightGrid.DirType.Down ? FightGrid.DirType.Up : FightGrid.DirType.Down, idx);
        }

        if (OnShoot != null)
        {
            OnShoot(blts);
        }
    }
    public Int2D GetGridIndex()
    {
        return m_ac.FGrid.Aircrafts[m_ac].Index + Info.Proto.GridOffset;
    }
    public Int2D[] GetTargets()
    {
        Int2D idx_self = GetGridIndex();
        switch (Info.Proto.Select)
        {
            case SelectType.Front:
                {
                    for (int i = 0; i < FightGrid.UnitCount.Y; ++ i)
                    {
                        Aircraft ac = GetFaceGrid().Units[i, idx_self.X].Aircraft;
                        if (ac != null && ac.State != Aircraft.StateType.Death)
                        {
                            return new Int2D[] { new Int2D(idx_self.X, i) };
                        }
                    }
                }
                break;                
                
        }
        return null;
    }

    FightGrid GetFaceGrid()
    {
        return m_ac.FGrid.FCtller.FGrids[m_ac.FGrid.Dir == FightGrid.DirType.Down ? 1 : 0];
    }
}
