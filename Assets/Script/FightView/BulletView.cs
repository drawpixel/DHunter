using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    Bullet m_blt;

    FxBase m_fx_fly = null;

    public void Create()
    {
        
    }

    public void Reset(Bullet blt)
    {
        m_blt = blt;
        m_blt.OnHit += OnHit;

        m_fx_fly = FxPool.Instance.Alloc("Bullet/" + blt.Gun.Info.Proto.Key, gameObject);

        transform.localPosition = m_blt.StartPos;

        Vector2 diff = (m_blt.EndPos - m_blt.StartPos).normalized;
        float angle = Vector2.Angle(diff, Vector2.up);
        if (diff.x < 0)
        {
            angle = 360 - angle;
        }
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        ActiveInPool = true;
    }

    void Update()
    {
        transform.localPosition = Vector2.Lerp(m_blt.StartPos * Launcher.SpriteScale,
                                               m_blt.EndPos * Launcher.SpriteScale, 
                                               Mathf.Clamp01(m_blt.LifeCounter / m_blt.Life));
    }

    public void OnHit()
    {
        m_blt.OnHit -= OnHit;

        FxBase fb = FxPool.Instance.Alloc("Fx/Hit/" + m_blt.Gun.Info.Proto.Key, null);
        fb.transform.localPosition = transform.localPosition;
        if (m_blt.DirEnd == FightGrid.DirType.Up)
        {
            fb.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            fb.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        FxPool.Instance.Free(m_fx_fly);
        m_fx_fly = null;

        BulletViewPool.Instance.Free(this);
    }

    bool m_active_in_pool = true;
    public bool ActiveInPool
    {
        get { return m_active_in_pool; }
        set
        {
            m_active_in_pool = value;
            gameObject.SetActive(value);
        }
    }
}

public class BulletViewPool
{
    public static BulletViewPool Instance;
    public static void CreasteInstance()
    {
        Instance = new BulletViewPool();
    }
    public static void DestroyInstance()
    {
        Instance = null;
    }

    GameObject m_blt_root;

    Dictionary<GunView, List<BulletView>> m_pool = new Dictionary<GunView, List<BulletView>>();

    public void Init()
    {
        m_blt_root = Util.NewGameObject("BulletRoot", Launcher.Instance.gameObject);
    }
    public BulletView Alloc(GunView gv, Bullet blt)
    {
        if (!m_pool.ContainsKey(gv))
        {
            m_pool[gv] = new List<BulletView>();
        }

        BulletView ret = null;

        foreach (BulletView m in m_pool[gv])
        {
            if (!m.ActiveInPool)
            {
                ret = m;
                break;
            }
        }

        if (ret == null)
        {
            BulletView new_m = Util.NewGameObject(gv.Gun.Info.Proto.Key, m_blt_root).AddComponent<BulletView>();
            new_m.Create();
            m_pool[gv].Add(new_m);

            ret = new_m;
        }

        ret.Reset(blt);

        return ret;
    }

    public void Free(BulletView blt)
    {
        blt.ActiveInPool = false;
    }
}