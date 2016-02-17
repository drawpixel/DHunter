using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGridView : MonoBehaviour
{
    FightCtllerView m_ctller = null;
    public FightCtllerView FCtllerView
    {
        get { return m_ctller; }
    }

    FightGrid m_fight_grid;
    public FightGrid FGrid
    {
        get { return m_fight_grid; }
    }

    public FightGrid.DirType Dir
    {
        get { return m_fight_grid.Dir; }
    }
    
    public class Unit
    {
        public Int2D Index = new Int2D(0, 0);
        public FightGridUnitView UnitView = null;

        public Unit(Int2D idx, FightGridUnitView uv)
        {
            Index = idx;
            UnitView = uv;
        }
    }
    Unit[,] m_units = null;
    public Unit[,] Units
    {
        get
        {
            return m_units;
        }
    }

    Dictionary<CreatureView, Unit> m_acs = new Dictionary<CreatureView, Unit>();
    public Dictionary<CreatureView, Unit> Aircrafts
    {
        get { return m_acs; }
    }

    public void Create(FightCtllerView ctl, FightGrid grid)
	{
        m_ctller = ctl;
        m_fight_grid = grid;

        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        m_units = new Unit[FightGrid.UnitCount.Y, FightGrid.UnitCount.X];
        for (int y = 0; y < FightGrid.UnitCount.Y; ++y)
        {
            for (int x = 0; x < FightGrid.UnitCount.X; ++x)
            {
                GameObject obj_unit = ResMgr.Instance.CreateGameObject("Fight/GridUnit", gameObject);
                obj_unit.transform.localPosition = grid.Units[y, x].Position * Launcher.SpriteScale;
                FightGridUnitView uv = obj_unit.GetComponent<FightGridUnitView>();
                uv.Create(this, grid.Units[y, x]);
                m_units[y, x] = new Unit(new Int2D(x, y), uv);

                min.x = Mathf.Min(min.x, grid.Units[y, x].Position.x - FightGrid.UnitSize / 2);
                min.y = Mathf.Min(min.y, grid.Units[y, x].Position.y - FightGrid.UnitSize / 2);

                max.x = Mathf.Max(max.x, grid.Units[y, x].Position.x + FightGrid.UnitSize / 2);
                max.y = Mathf.Max(max.y, grid.Units[y, x].Position.y + FightGrid.UnitSize / 2);
            }
        }
        min *= Launcher.SpriteScale;
        max *= Launcher.SpriteScale;
        BoxCollider cld = gameObject.AddComponent<BoxCollider>();
        cld.center = min + (max - min) / 2;
        cld.size = (max - min);

        m_fight_grid.OnAddAircraft += OnAddAircraft;
	}
    void OnDestroy()
    {
        m_fight_grid.OnAddAircraft -= OnAddAircraft;
    }

    void Update()
    {
        
    }


    Unit GetUnitView(Vector2 pt)
    {
        foreach (Unit u in m_units)
        {
            if (u.UnitView.CheckPoint(pt))
                return u;
        }
        return null;
    }

    void OnAddAircraft(Creature ac, Int2D pt)
    {
        CreatureView av = Util.NewGameObject("Aircraft", gameObject).AddComponent<CreatureView>();
        av.Create(ac);
        av.UpdateTransform(pt);
    }


    Unit m_unit_start = null;

    void OnMouseDown()
    {
        Vector3 pt = Launcher.Instance.MainCamera.ScreenToWorldPoint(InputWrap.TouchPosition);
        m_unit_start = GetUnitView(pt);

        foreach (Unit u in m_units)
        {
            u.UnitView.BodyColor = u == m_unit_start ? Color.white : Color.green;
        }
    }
    void OnMouseUp()
    {
        foreach (Unit u in m_units)
        {
            u.UnitView.BodyColor = new Color(1, 1, 1, 0);
        }
    }
    void OnMouseDrag()
    {
        //Vector3 pt = Launcher.Instance.MainCamera.ScreenToWorldPoint(InputWrap.TouchPosition);
        //Unit curt = GetUnitView(pt);
    }
}
