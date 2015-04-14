using System.Collections.Generic;
using Fairwood;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// 原子的衰变组件
/// </summary>
[RequireComponent(typeof(Atom))]
public class AtomDecay : MonoBehaviour
{
    public Atom Atom;

    public float ExciteTime = 1;
    public bool CanIntelligonExcite = true;
    public bool CanNeutronExcite;
    public float DecayTime = 4;
    public List<string> DecayProductList;
    public List<int> DecayProductCountList;
    public SpriteRenderer MyRenderer;

    public float ExciteProgress = 0;
    public float DecayProgress = 0;
    public int AbsorbedNeutrons = 0;//吸收的中子数
    private float _oriRadius;
    private float _oriOrbitSpeed;

    private DecayStateEnum _decayState = DecayStateEnum.Intact;
    enum DecayStateEnum
    {
        Intact,
        Decaying,
        Die
    }

    void Awake()
    {
        Atom = GetComponent<Atom>();
        var graphic = new GameObject("AtomDecay Graphic");
        graphic.layer = GameManager.LAYER_ID_MAIN;
        graphic.transform.ResetTransform(transform);
        MyRenderer = graphic.AddComponent<SpriteRenderer>();
        MyRenderer.sprite = GameManager.Instance.AtomDecayGraphic;
        MyRenderer.color = new Color32(255, 120, 120, 255);
    }

    void Start()
    {
        Rebuild();
    }

    void Update()
    {
        if (_decayState == DecayStateEnum.Intact)
        {
            var exciting = false;
            if (CanIntelligonExcite && Atom.DockingIntelligon)//激发中
            {
                exciting = true;
                ExciteProgress += Atom.Dt;
            }
            else if (CanNeutronExcite && AbsorbedNeutrons > 0)
            {
                exciting = true;
                ExciteProgress += Atom.Dt * AbsorbedNeutrons;
            }

            if (exciting)
            {
                Rebuild();
                if (ExciteProgress >= 1) //开始衰变
                {
                    Excite();
                }
            }
            else //退激发
            {
                if (ExciteProgress > 0)
                {
                    ExciteProgress -= Atom.Dt;
                    if (ExciteProgress < 0) ExciteProgress = 0;
                    Rebuild();
                }
            }
        }
        else if (_decayState == DecayStateEnum.Decaying)
        {
            DecayProgress += Atom.Dt;

            Atom.Radius = _oriRadius*(1 - DecayProgress);
            Atom.OrbitSpeed = _oriOrbitSpeed*(1 + DecayProgress * 2);
            Rebuild();
            Atom.Rebuild();
            if (DecayProgress >= 1)
            {
                Die();
            }
        }
        else
        {
            
        }
    }

    public void Excite()
    {
        if (_decayState != DecayStateEnum.Intact) return;
        _decayState = DecayStateEnum.Decaying;
        DecayProgress = ExciteTime * (ExciteProgress - 1) / DecayTime;
        _oriRadius = Atom.CircleCollider.radius;
        _oriOrbitSpeed = Atom.OrbitSpeed;
    }

    public void Die()
    {
        _decayState = DecayStateEnum.Die;
        Assert.AreEqual(DecayProductCountList.Count, DecayProductList.Count, "ProductList Length NOT Equal");
        for (int i = 0; i < DecayProductList.Count; i++)
        {
            for (int j = 0; j < DecayProductCountList[i]; j++)
            {
                var partile = GameManager.Instance.CreateParticle(DecayProductList[i], Atom.Position);
                partile.Velocity = Random.insideUnitCircle*100;
            }
        }
        Atom.Die();
    }

    public void Rebuild()
    {
        float s = 0;
        if (_decayState == DecayStateEnum.Intact)
        {
            s = ExciteProgress*0.8f + 0.2f;
        }
        else if (_decayState == DecayStateEnum.Decaying)
        {
            s = 1 - DecayProgress;
        }
        var scale = Atom.Radius/512f*s;
        if (MyRenderer) MyRenderer.transform.localScale = new Vector3(scale, scale, 1);
    }

    #region 编辑器用
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 120, 120, 255);
        Gizmos.DrawSphere(transform.position, 8);
    }

    void _Preview()
    {
        if (MyRenderer) return;
        Awake();
    }
    [ContextMenu("Quit Preview")]
    void QuitPreview()
    {
        SendMessage("_RecoverInit");
    }

    void _RecoverInit()
    {
        if (MyRenderer) Destroy(MyRenderer.gameObject);
    }

#endif
    #endregion
}