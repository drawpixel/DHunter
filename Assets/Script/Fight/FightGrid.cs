﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGrid
{
    public static int UnitSize = 100;
    public static int FightSize = 300;
    public static Int2D UnitCount = new Int2D(5, 3);


    public delegate void DgtAddAircraft(Aircraft ac, Int2D pt);
    public DgtAddAircraft OnAddAircraft;



    FightCtller m_ctller = null;
    public FightCtller FCtller
    {
        get { return m_ctller; }
    }

    public enum DirType
    {
        Down,
        Up,
    }
    DirType m_dir = DirType.Down;
    public DirType Dir
    {
        get { return m_dir; }
    }

    public class Unit
    {
        public Int2D Index = new Int2D(0, 0);
        public Vector2 Position = Vector2.zero;
        public Aircraft Aircraft = null;

        public Unit(Int2D idx, Aircraft ac)
        {
            Index = idx;
            Aircraft = ac;
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



    Dictionary<Aircraft, Unit> m_acs = new Dictionary<Aircraft, Unit>();
    public Dictionary<Aircraft, Unit> Aircrafts
    {
        get { return m_acs; }
    }

    

    public void Create(FightCtller ctl, DirType dir)
	{
        m_dir = dir;
        m_ctller = ctl;

        m_units = new Unit[UnitCount.Y, UnitCount.X];
        for (int y = 0; y < Units.GetLength(0); ++y)
        {
            for (int x = 0; x < Units.GetLength(1); ++x)
            {
                m_units[y, x] = new Unit(new Int2D(x, y), null);
                m_units[y, x].Position = CalcUnitPosition(m_units[y, x].Index, dir);
            }
        }
	}
    
    public void Update(float interval)
    {
        foreach (Aircraft ac in m_acs.Keys)
        {
            ac.Update(interval);
        }
    }

    public void Fight()
    {
        foreach (Aircraft ac in m_acs.Keys)
        {
            ac.Fight();
        }
    }

    public bool CheckAircraft(Aircraft ac, Int2D pt)
    {
        for (int i = 0; i < ac.Info.Proto.Occupies.Length; ++i)
        {
            int idx = ac.Info.Proto.Occupies[i];
            int x = idx % ac.Info.Proto.Dim.X;
            int y = idx / ac.Info.Proto.Dim.X;
            if (x < 0 || y < 0 || x >= UnitCount.X || y >= UnitCount.Y)
                return false;
        }
        return true;
    }
    public void AddAircraft(Aircraft ac, Int2D pt)
    {
        if (!CheckAircraft(ac, pt))
            return;

        m_acs.Add(ac, new Unit(pt, ac));

        ac.FGrid = this;
        for (int i = 0; i < ac.Info.Proto.Occupies.Length; ++i)
        {
            int idx = ac.Info.Proto.Occupies[i];
            int x = idx % ac.Info.Proto.Dim.X;
            int y = idx / ac.Info.Proto.Dim.X;
            Units[pt.Y + y, pt.X + x].Aircraft = ac;
        }

        if (OnAddAircraft != null)
        {
            OnAddAircraft(ac, pt);
        }
    }
    public void RemoveAircraft(Aircraft ac)
    {
        m_acs.Remove(ac);
        for (int y = 0; y < Units.GetLength(0); ++y)
        {
            for (int x = 0; x < Units.GetLength(1); ++ x)
            {
                if (Units[y, x].Aircraft == ac)
                {
                    Units[y, x].Aircraft = null;
                }
            }
        }
    }

    public List<Unit> PickGrid(Vector2 start, Vector2 dir)
    {
        Int2D s = new Int2D();
        s.X = Mathf.RoundToInt(start.x / FightGrid.UnitSize);
        s.Y = Mathf.RoundToInt(start.y / FightGrid.UnitSize);

        dir = dir.normalized;

        List<Unit> rets = null;

        float k = dir.x / (dir.x + dir.y);

        for (int i = 0; i < (FightSize / UnitSize + UnitCount.Y) * 2; ++i)
        {
            Vector2 pt = CalcUnitPosition(s, Dir);
            pt.x = start.x + pt.y * k;
            Unit u = GetUnitByPos(pt);
            if (u != null)
            {
                rets.Add(u);
            }
            NextStep(ref s, dir);
        }

        return rets;
    }
    void NextStep(ref Int2D s, Vector2 dir)
    {
        if (dir.x < dir.y)
        {
            if (dir.y > 0)
            {
                s.Y += 1;
            }
            else
            {
                s.Y -= 1;
            }
        }
        else
        {
            if (dir.y > 0)
            {
                s.X += 1;
            }
            else
            {
                s.X -= 1;
            }
        }
    }
    public Unit GetUnitByPos(Vector2 pt)
    {
        int x = Mathf.RoundToInt(pt.x / FightGrid.UnitSize) + UnitCount.X / 2;
        int y = Mathf.RoundToInt(pt.y / FightGrid.UnitSize);
        if (Dir == DirType.Down)
        {
            y += FightSize / UnitSize;
        }
        else
        {
            y -= FightSize / UnitSize;
        }
        if (x < 0 || x >= UnitCount.X || y < 0 || y >= UnitCount.Y)
        {
            return null;
        }
        return m_units[y, x];
    }

    public static Vector2 CalcUnitPosition(Int2D idx, DirType dir)
    {
        Vector2 pos = Vector3.zero;
        pos.x = -UnitCount.X * UnitSize / 2;
        pos.x += idx.X * UnitSize;
        pos.y = -300; //-(FightCtller.Dim.Y / 2) + (UnitCount.Y * UnitSize) + 50;
        pos.y -= idx.Y * UnitSize;
        pos.x += UnitSize / 2;
        //pos.y -= UnitSize / 2;
        if (dir == DirType.Up)
        {
            pos.y = -pos.y;
        }
        return pos;
    }

    
}
