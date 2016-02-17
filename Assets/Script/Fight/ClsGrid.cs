using System.Collections;
using System.Collections.Generic;

public interface ICldGridSpot
{
    Int2D GetSpotPosition();
    void OnSpotCollision(ICldGridContent content);
}
public interface ICldGridContent
{
    Int2D GetContentPosition();
    void SetContentPosition(Int2D pt);

    Int2D GetContentSize();
    int[] GetContentPattern();

    void OnContentCollision(ICldGridSpot spot);
}

public class ClsGrid
{
    public class Unit
    {
        public Int2D Index;
        public List<ICldGridSpot> Spot = new List<ICldGridSpot>();
        public List<ICldGridContent> Content = new List<ICldGridContent>();

        public Unit(int x, int y)
        {
            Index = new Int2D(x, y);
        }
    }
    Unit[,] m_units = null;
    Dictionary<ICldGridContent, Int2D[]> m_remap = new Dictionary<ICldGridContent, Int2D[]>();

	public void Create(Int2D dim, int unit_size)
	{
        m_units = new Unit[dim.X / unit_size, dim.Y / unit_size];
        for (int y = 0; y < m_units.GetLength(0); ++y)
        {
            for (int x = 0; x < m_units.GetLength(1); ++x)
            {
                m_units[y, x] = new Unit(x, y);
            }
        }
	}
    
    void Update()
    {
        
    }

    public void FillContent(ICldGridContent content)
    {
        Int2D pt = content.GetContentPosition();
        Int2D sz = content.GetContentSize();
        int[] ptn = content.GetContentPattern();

        if (!m_remap.ContainsKey(content))
        {
            m_remap[content] = new Int2D[ptn.Length];
        }
        
        for (int i = 0; i < ptn.Length; ++i)
        {
            int p = ptn[i];
            int x = p % sz.X;
            int y = p / sz.Y;
            x += pt.X;
            y += pt.Y;
            if (m_units[y, x].Content.IndexOf(content) < 0)
            {
                m_units[y, x].Content.Add(content);
            }
            m_remap[content][i] = new Int2D(x, y);
        }
    }
    public void RemoveContent(ICldGridContent content, bool del)
    {
        Int2D[] pts = null;
        if (!m_remap.TryGetValue(content, out pts))
            return;
        for (int i = 0; i < pts.Length; ++i)
        {
            System.Diagnostics.Debug.Assert(m_units[pts[i].Y, pts[i].X].Content.IndexOf(content) >= 0);
            m_units[pts[i].Y, pts[i].X].Content.Remove(content);
        }
        if (del)
        {
            m_remap.Remove(content);
        }
    }
    public void UpdateContent(ICldGridContent content)
    {
        RemoveContent(content, false);
        FillContent(content);
    }
}
