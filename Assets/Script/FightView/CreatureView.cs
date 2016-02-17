using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatureView : MonoBehaviour
{
    Creature m_ac;
    public Creature AC
    {
        get { return m_ac; }
    }

    public InfoCreature Info
    {
        get { return m_ac.Info; }
    }


    


    Animation m_anim_ctl;

    GameObject m_prog_hp;
    Image m_img_prog_hp;

    public void Create(Creature ac)
	{
        m_ac = ac;
        m_ac.OnDead += OnDead;
        m_ac.OnTakeDamage += OnTakeDamage;
        

        m_anim_ctl = ResMgr.Instance.CreateGameObject(string.Format("Creature/{0}/{1}", ac.Proto.Key, ac.Proto.Key), gameObject).GetComponent<Animation>();
        m_anim_ctl.AddClip(ResMgr.Instance.Load(string.Format("Creature/{0}/FIdle", ac.Proto.Key)) as AnimationClip, "FIdle");
        m_anim_ctl.AddClip(ResMgr.Instance.Load(string.Format("Creature/{0}/Fight", ac.Proto.Key)) as AnimationClip, "Fight");
        m_anim_ctl.AddClip(ResMgr.Instance.Load(string.Format("Creature/{0}/Die", ac.Proto.Key)) as AnimationClip, "Die");
        m_anim_ctl.AddClip(ResMgr.Instance.Load(string.Format("Creature/{0}/Walk", ac.Proto.Key)) as AnimationClip, "Walk");
        m_anim_ctl.AddClip(ResMgr.Instance.Load(string.Format("Creature/{0}/BeHit", ac.Proto.Key)) as AnimationClip, "BeHit");

        m_anim_ctl["BeHit"].layer = 2;


        m_prog_hp = ResMgr.Instance.CreateGameObject("UI/PlaneProgHP", Launcher.Instance.CanvasUI.gameObject);
        m_img_prog_hp = Util.FindGameObjectByName(m_prog_hp, "Fill").GetComponent<Image>();
        m_img_prog_hp.fillAmount = 1;

        Idle();
    }
    
    void Update()
    {
        
    }

    public void UpdateTransform(Int2D pt)
    {
        Vector3 s = m_ac.FGrid.Units[pt.Y, pt.X].Position;
        s.x -= FightGrid.UnitSize * 0.5f;
        s.z += FightGrid.UnitSize * 0.5f;
        s.x += m_ac.Info.Proto.Dim.X * FightGrid.UnitSize * 0.5f;
        s.z -= m_ac.Info.Proto.Dim.Y * FightGrid.UnitSize * 0.5f;
        transform.localPosition = s * Launcher.SpriteScale;

        Vector3 pt_prog = transform.position;
        //pt_prog += m_ac.FGrid.Dir == FightGrid.DirType.Up ? new Vector3(0, FightGrid.UnitSize * Launcher.SpriteScale / 2, 0) : new Vector3(0, -FightGrid.UnitSize * Launcher.SpriteScale / 2, 0);
        pt_prog += new Vector3(0, 2, 0);
        pt_prog = RectTransformUtility.WorldToScreenPoint(Launcher.Instance.MainCamera, pt_prog);
        m_prog_hp.GetComponent<RectTransform>().position = pt_prog;

        if (m_ac.FGrid.Dir == FightGrid.DirType.Up)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }


    void Idle()
    {
        m_anim_ctl.CrossFade("FIdle", 0.2f);
    }
    void OnTakeDamage(Creature killer, float damage)
    {
        m_img_prog_hp.fillAmount = AC.RemainHP / AC.MaxHP;

        m_anim_ctl.CrossFade("BeHit", 0.05f, PlayMode.StopSameLayer);
    }
    void OnDead(Creature killer)
    {
        m_prog_hp.SetActive(false);

        m_anim_ctl.CrossFade("Die", 0.2f);
    }

}
