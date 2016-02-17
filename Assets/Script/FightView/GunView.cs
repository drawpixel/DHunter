using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GunView : MonoBehaviour
{
    Aircraft m_ac = null;
    public Aircraft AC
    {
        get { return m_acv.AC; }
    }

    Gun m_gun = null;
    public Gun Gun
    {
        get { return m_gun; }
    }

    AircraftView m_acv = null;
    public AircraftView ACV
    {
        get { return m_acv; }
    }

    public InfoAircraft Info
    {
        get { return m_ac.Info; }
    }

    //GameObject m_frame = null;

    public void Create(AircraftView acv, Gun gun)
	{
        m_acv = acv;
        m_gun = gun;

        m_gun.OnShoot += OnShoot;
    }
    void OnDestroy()
    {
        m_gun.OnShoot -= OnShoot;
    }

    void Update()
    {
        
    }

    public void UpdateTransform(Int2D pt)
    {
        Vector2 s = m_ac.FGrid.Units[pt.Y, pt.X].Position;
        s.x -= FightGrid.UnitSize * 0.5f;
        s.y += FightGrid.UnitSize * 0.5f;
        s.x += m_ac.Info.Proto.Dim.X * FightGrid.UnitSize * 0.5f;
        s.y -= m_ac.Info.Proto.Dim.Y * FightGrid.UnitSize * 0.5f;
        transform.localPosition = s * Launcher.SpriteScale;
        transform.localRotation = m_ac.FGrid.Dir == FightGrid.DirType.Up ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        //transform.localScale = m_ac.FGrid.Dir == FightGrid.DirType.Up ? new Vector3(1, -1, 1) : Vector3.one;
    }

    void OnShoot(Bullet[] blts)
    {
        foreach (Bullet blt in blts)
        {
            BulletViewPool.Instance.Alloc(this, blt);
        }
    }
}
