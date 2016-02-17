using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCtllerView : MonoBehaviour
{

    FightCtller m_ctller;
    public FightCtller Ctller
    {
        get { return m_ctller; }
    }

    FightGridView[] m_fg = new FightGridView[2];

    public FightGridView[] FGridViews
    {
        get
        {
            return m_fg;
        }

        set
        {
            m_fg = value;
        }
    }

    public void Create(FightCtller ctller)
	{
        m_ctller = ctller;

        for (int i = 0; i < m_fg.Length; ++i)
        {
            m_fg[i] = Util.NewGameObject("Grid_" + ctller.FGrids[i].Dir.ToString(), gameObject).AddComponent<FightGridView>();
            m_fg[i].Create(this, m_ctller.FGrids[i]);
        }
        
	}
    
    void Update()
    {
        
    }

    

    
}
