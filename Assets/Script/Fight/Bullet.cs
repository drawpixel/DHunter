using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    public delegate void DgtHit();
    public DgtHit OnHit;

    Gun m_gun;
    public Gun Gun
    {
        get { return m_gun; }
    }

    Int2D m_start;
    public Int2D Start
    {
        get { return m_start; }
    }

    Vector2 m_start_pos;
    public Vector2 StartPos
    {
        get 
        {
            return m_start_pos;
        }
    }
    
    Int2D m_end;
    public Int2D End
    {
        get { return m_end; }
    }

    Vector2 m_end_pos;
    public Vector2 EndPos
    {
        get
        {
            return m_end_pos;
        }
    }

    FightGrid.DirType m_dir_s;
    public FightGrid.DirType DirStart
    {
        get { return m_dir_s; }
    }
    FightGrid.DirType m_dir_e;
    public FightGrid.DirType DirEnd
    {
        get { return m_dir_e; }
    }

    float m_life;
    public float Life
    {
        get { return m_life; }
    }
    float m_life_counter = 0;
    public float LifeCounter
    {
        get { return m_life_counter; }
    }

    public void Create()
    {
        
    }

    public void Reset(Gun gun, FightGrid.DirType dir_s, Int2D s, FightGrid.DirType dir_e, Int2D e)
    {
        m_gun = gun;

        m_start = s;
        m_end = e;
        m_dir_s = dir_s;
        m_dir_e = dir_e;

        m_start_pos = m_gun.FCtller.FGrids[(int)dir_s].Units[s.Y, s.X].Position;
        m_end_pos = m_gun.FCtller.FGrids[(int)dir_e].Units[e.Y, e.X].Position;

        m_life = Vector2.Distance(m_start_pos, m_end_pos) / gun.Info.Proto.BulletSpeed;
        m_life_counter = 0;

        ActiveInPool = true;
    }

    public void Update(float interval)
    {
        m_life_counter += interval;
        if (m_life_counter >= m_life)
        {
            Hit();
        }
    }

    public void Hit()
    {
        Aircraft ac = m_gun.FCtller.FGrids[(int)m_dir_e].Units[m_end.Y, m_end.X].Aircraft;
        if (ac != null)
        {
            ac.TakeDamage(Gun.AC, Gun.Info.Proto.AP);
        }

        if (OnHit != null)
        {
            OnHit();
        }

        BulletPool.Instance.Free(this);
    }

    bool m_active_in_pool = true;
    public bool ActiveInPool
    {
        get { return m_active_in_pool; }
        set
        {
            m_active_in_pool = value;
        }
    }
}

public class BulletPool
{
    public static BulletPool Instance;
    public static void CreasteInstance()
    {
        Instance = new BulletPool();
    }
    public static void DestroyInstance()
    {
        Instance = null;
    }

    Dictionary<Gun, List<Bullet>> m_pool = new Dictionary<Gun, List<Bullet>>();

    public void Init()
    {
        
    }
    public Bullet Alloc(Gun gun, FightGrid.DirType dir_s, Int2D s, FightGrid.DirType dir_e, Int2D e)
    {
        if (!m_pool.ContainsKey(gun))
        {
            m_pool[gun] = new List<Bullet>();
        }

        Bullet ret = null;

        foreach (Bullet m in m_pool[gun])
        {
            if (!m.ActiveInPool)
            {
                ret = m;
                break;
            }
        }

        if (ret == null)
        {
            Bullet new_m = new Bullet();
            new_m.Create();
            m_pool[gun].Add(new_m);

            ret = new_m;
        }

        ret.Reset(gun, dir_s, s, dir_e, e);

        return ret;
    }

    public void Free(Bullet blt)
    {
        blt.ActiveInPool = false;
    }

    public void Update(float interval)
    {
        foreach (List<Bullet> blts in m_pool.Values)
        {
            foreach (Bullet m in blts)
            {
                if (m.ActiveInPool)
                {
                    m.Update(interval);
                }
            }
        }
        
    }
}