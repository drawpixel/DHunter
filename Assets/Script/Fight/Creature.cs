using System.Collections;
using System.Collections.Generic;


public class Creature
{
    public delegate void DgtTakeDamage(Creature killer, float curt);
    public DgtTakeDamage OnTakeDamage;

    public delegate void DgtDead(Creature killer);
    public DgtDead OnDead;



    InfoCreature m_info;
    public InfoCreature Info
    {
        get { return m_info; }
    }
    public ProtoCreature Proto
    {
        get { return m_info.Proto; }
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

    

    public void Create(InfoCreature info)
	{
        m_info = info;

        m_max_hp = m_remain_hp = info.Proto.HP;

        Idle();
	}
    
    public void Update(float interval)
    {
        m_state_counter += interval;
        
        switch (State)
        {
            case StateType.Idle:
                break;
            case StateType.Fighting:
                break;
            case StateType.Death:
                break;
        }
    }

    public void TakeDamage(Creature killer, float damage)
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
    public void Dead(Creature killer)
    {
        m_state = StateType.Death;
        m_state_counter = 0;

        if (OnDead != null)
        {
            OnDead(killer);
        }
    }
}
