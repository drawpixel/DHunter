using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGridUnitView : MonoBehaviour
{
    public SpriteRenderer SrBody;


    FightGridView m_grid;
    FightGrid.Unit m_unit;

    Rect m_rc_world;

    public void Create(FightGridView grid, FightGrid.Unit unit)
	{
        m_grid = grid;
        m_unit = unit;

        m_rc_world = new Rect(m_unit.Position * Launcher.SpriteScale, Vector2.one * FightGrid.UnitSize * Launcher.SpriteScale);
        m_rc_world.position -= m_rc_world.size / 2;

        BodyColor = new Color(1, 1, 1, 0);
	}
    void OnDestroy()
    {
        
    }

    void Update()
    {
        
    }

    public bool CheckPoint(Vector2 pt)
    {
        return pt.x < m_rc_world.xMax && pt.y < m_rc_world.yMax && 
               pt.x >= m_rc_world.xMin && pt.y >= m_rc_world.yMin;
    }

    public Color BodyColor
    {
        get { return SrBody.color; }
        set
        {
            SrBody.color = value;
        }
    }
}
