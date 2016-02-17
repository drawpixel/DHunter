using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Launcher : MonoBehaviour 
{
	public static Launcher Instance = null;
	void Awake()
	{
		Instance = this;
	}

    public static Int2D LogicDim = new Int2D(640, 1136);
    public static float SpriteScale = 0.01f;

	public Camera MainCamera;
    public GameObject RootSprite;

    public Canvas CanvasUI;


	// Use this for initialization
	void Start () 
	{
        //MainCamera.orthographicSize = 5 * (Screen.width / Screen.height) / (LogicDim.X / LogicDim.Y);

        RootSprite = Util.NewGameObject("RootSprite", null);
        //RootSprite.transform.localScale = Vector3.one * (MainCamera.orthographicSize / LogicDim.Y);

        Application.targetFrameRate = 50;

        ResMgr.CreateInstance ();
		ResMgr.Instance.Init ();
        
		FxPool.CreasteInstance ();
		FxPool.Instance.Init ();

        BulletPool.CreasteInstance();
        BulletPool.Instance.Init();

        BulletViewPool.CreasteInstance();
        BulletViewPool.Instance.Init();

        ProtoMgr.CreasteInstance();
        ProtoMgr.Instance.Init((string path) =>
            {
                TextAsset ta = ResMgr.Instance.Load(path) as TextAsset;
                return ta.text;
            });

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

        SetupFightCtller();
        
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (m_fc != null)
        {
            m_fc.Update(Time.deltaTime);
        }
        
		/*
		if (Network.peerType == NetworkPeerType.Disconnected) 
		{
			NetworkConnectionError err = Network.InitializeServer (12, 7777, false);
		}
		*/
	}

	

	int m_fps = 0;
	int m_fps_cache = 0;
	float m_fps_counter = 0;
	void OnGUI()
	{
		++ m_fps_cache;
		m_fps_counter += Time.deltaTime;
		if (m_fps_counter > 1) 
		{
			m_fps_counter -= 1;
			m_fps = m_fps_cache;
			m_fps_cache = 0;
		}
		GUI.Label (new Rect(20, 20, 100, 40), "FPS: " + m_fps.ToString());
	}


    FightCtller m_fc;
    FightCtllerView m_fcv;
    void SetupFightCtller()
    {
        ResMgr.Instance.CreateGameObject("BG/BG01", gameObject);

        m_fc = new FightCtller();
        m_fc.Create();

        GameObject ctl = Util.NewGameObject("FCtller", RootSprite);
        m_fcv = ctl.AddComponent<FightCtllerView>();
        m_fcv.Create(m_fc);

        string[] keys = null;
        Int2D[] pts = null;

        keys = new string[] { "P06", "P01", "P05"};
        pts = new Int2D[] { new Int2D(0, 0), new Int2D(0, 2), new Int2D(2, 0) };
        for (int p = 0; p < 3; ++ p)
        {
            string k = keys[p];
            InfoAircraft info = new InfoAircraft(ProtoMgr.Instance.GetByKey<ProtoAircraft>(k));
            info.Guns = new InfoGun[info.Proto.Guns.Length];
            for (int i = 0; i < info.Guns.Length; ++i)
            {
                info.Guns[i] = new InfoGun(info.Proto.ProtoGuns[i]);
            }

            Aircraft ac = new Aircraft();
            ac.Create(info);
            m_fc.FGrids[0].AddAircraft(ac, pts[p]);
        }

        keys = new string[] { "P06", "P01", "P05" };
        pts = new Int2D[] { new Int2D(2, 1), new Int2D(0, 2), new Int2D(1, 0) };
        for (int p = 0; p < 3; ++p)
        {
            string k = keys[p];
            InfoAircraft info = new InfoAircraft(ProtoMgr.Instance.GetByKey<ProtoAircraft>(k));
            info.Guns = new InfoGun[info.Proto.Guns.Length];
            for (int i = 0; i < info.Guns.Length; ++i)
            {
                info.Guns[i] = new InfoGun(info.Proto.ProtoGuns[i]);
            }

            Aircraft ac = new Aircraft();
            ac.Create(info);
            m_fc.FGrids[1].AddAircraft(ac, pts[p]);
        }

        m_fc.Fight();
    }
}
