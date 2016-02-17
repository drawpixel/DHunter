using System.Collections;
using System.Collections.Generic;


public class Aircraft
{
    public delegate void DgtTakeDamage(Aircraft killer, float curt);
    public DgtTakeDamage OnTakeDamage;

    public delegate void DgtDead(Aircraft killer);
    public DgtDead OnDead;



    InfoAircraft m_info;
    public InfoAircraft Info
    {
        get { return m_info; }
    }

    FightGrid m_fg = null;
    public FightGrid FGrid
    {
        get { return m_fg; }
        set
        {
            m_fg = value;
        }
    }

    Gun[] m_guns = null;
    public Gun[] Guns
    {
        get { return m_guns; }
    }


    float m_max_hp = 0;
    public float MaxHP
    {
        get { return m_max_hp; }
    }
    float m_remain_hp = 0;
    public float RemainHP
    {
        get { return m_remain_hp; }
    }


    public enum StateType
    {
        Idle,
        Fighting,
        Death,
    }
    StateType m_state = StateType.Idle;
    public StateType State
    {
        get { return m_state; }
    }
    float m_state_counter = 0;

    

    public void Create(InfoAircraft info)
	{
        m_info = info;

        m_max_hp = m_remain_hp = info.Proto.HP;

        m_guns = new Gun[Info.Guns.Length];
        for (int i = 0; i < Info.Guns.Length; ++ i)
        {
            Gun gun = new Gun();
            gun.Create(Info.Guns[i], this);

            m_guns[i] = gun; 
        }

        Idle();
	}
    
    public void Update(float interval)
    {
        m_state_counter += interval;
        foreach (Gun g in m_guns)
        {
            g.Update(interval);
        }

        switch (State)
        {
            case StateType.Idle:
                break;
            case StateType.Fighting:
                foreach (Gun g in m_guns)
                {
                    if (g.CanShoot())
                        g.Shoot();
                }
                break;
            case StateType.Death:
                break;
        }
    }

    public void TakeDamage(Aircraft killer, float damage)
    {
        if (State == StateType.Death)
            return;

        m_remain_hp -= damage;
        if (m_remain_hp <= 0)
        {
            m_remain_hp = 0;
            Dead(killer);
        }

        if (OnTakeDamage != null)
        {
            OnTakeDamage(killer, damage);
        }
    }

    public void Idle()
    {
        m_state = StateType.Idle;
        m_state_counter = 0;
    }
    public void Fight()
    {
        m_state = StateType.Fighting;
        m_state_counter = 0;
    }
    public void Dead(Aircraft killer)
    {
        m_state = StateType.Death;
        m_state_counter = 0;

        if (OnDead != null)
        {
            OnDead(killer);
        }
    }
}
