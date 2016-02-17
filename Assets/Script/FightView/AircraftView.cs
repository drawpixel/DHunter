using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AircraftView : MonoBehaviour
{
    Aircraft m_ac;
    public Aircraft AC
    {
        get { return m_ac; }
    }

    public InfoAircraft Info
    {
        get { return m_ac.Info; }
    }

    GameObject m_frame;

    GunView[] m_gvs = null;

    GameObject m_prog_hp;
    Image m_img_prog_hp;

    public void Create(Aircraft ac)
	{
        m_ac = ac;
        m_ac.OnTakeDamage += OnTakeDamage;
        m_ac.OnDead += OnDead;

        m_frame = ResMgr.Instance.CreateGameObject("Aircraft/" + m_ac.Info.Proto.Key, gameObject);
        m_prog_hp = ResMgr.Instance.CreateGameObject("UI/PlaneProgHP", Launcher.Instance.CanvasUI.gameObject);
        m_img_prog_hp = Util.FindGameObjectByName(m_prog_hp, "Fill").GetComponent<Image>();
        m_img_prog_hp.fillAmount = 1;

        m_gvs = new GunView[ac.Guns.Length];
        for (int i = 0; i < ac.Guns.Length; ++i)
        {
            Gun g = ac.Guns[i];
            m_gvs[i] = Util.NewGameObject(g.Info.Proto.Key, gameObject).AddComponent<GunView>();
            m_gvs[i].Create(this, g);
        }
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

        if (m_ac.FGrid.Dir == FightGrid.DirType.Up)
        {
            transform.localScale = new Vector3(1, -1, 1);
            foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.transform.localRotation = Quaternion.Euler(0, 0, 180) * ps.transform.localRotation;
            }
        }
        
        Vector3 pt_prog = transform.position;
        pt_prog += m_ac.FGrid.Dir == FightGrid.DirType.Up ? new Vector3(0, FightGrid.UnitSize * Launcher.SpriteScale / 2, 0) : new Vector3(0, -FightGrid.UnitSize * Launcher.SpriteScale / 2, 0);
        pt_prog = RectTransformUtility.WorldToScreenPoint(Launcher.Instance.MainCamera, pt_prog);
        m_prog_hp.GetComponent<RectTransform>().position = pt_prog;
    }

    void OnTakeDamage(Aircraft killer, float damage)
    {
        m_img_prog_hp.fillAmount = AC.RemainHP / AC.MaxHP;
    }
    void OnDead(Aircraft killer)
    {
        m_prog_hp.SetActive(false);
        
        StartCoroutine(Drop());
        for (int i = 0; i < 20; ++ i)
        {
            StartCoroutine(PlayExplosion(i * 0.2f));
        }
    }

    IEnumerator Drop()
    {
        Vector3 orig_scale = transform.localScale;

        float counter = 0;
        while (true)
        {
            transform.localRotation *= Quaternion.Euler(0, 0, Time.deltaTime * 90.0f);
            transform.localScale = Vector3.Lerp(orig_scale, orig_scale * 0.02f, counter * 0.25f);

            counter += Time.deltaTime;
            if (counter >= 4)
            {
                GameObject.DestroyObject(gameObject);
                break;
            }
            else
                yield return 1;
        }
        
    }
    IEnumerator PlayExplosion(float delay)
    {
        yield return new WaitForSeconds(delay);

        FxBase fb = FxPool.Instance.Alloc("Fx/Exp/Exp01", null);
        Vector3 offset = new Vector3(Random.Range(-AC.Info.Proto.Dim.X, +AC.Info.Proto.Dim.X), Random.Range(-AC.Info.Proto.Dim.Y, +AC.Info.Proto.Dim.Y), 0);
        offset *= FightGrid.UnitSize * 0.5f;
        offset *= Launcher.SpriteScale;
        fb.transform.localPosition = transform.localPosition + offset;
    }
}
