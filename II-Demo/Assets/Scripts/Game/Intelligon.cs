using Fairwood;
using Fairwood.Math;
using UnityEngine;
using System.Collections;

public class Intelligon : BaseParticle
{
    public float StraightFlyTime = 3;
    public float AttractionAmount = 100000;
    public float VelocityMax = 200;


    public Atom DockingAtom;

    public float AngularPositionOnCurrentAtom;

    public Vector2 FreeVelocity;

    private float _straightFlyStartTime;

    public override Color MyColor
    {
        get { return new Color(1, 1, 1); }
    }
    public enum MotionStateEnum
    {
        Docking,//必须有DockingAtom
        Straight,
        Falling,
    }

    public MotionStateEnum MotionState;

    void Awake()
    {
        var graphic = new GameObject("Intelligon Graphic");
        graphic.layer = GameManager.LAYER_ID_MAIN;
        graphic.transform.ResetTransform(transform);
        MyRenderer = graphic.AddComponent<SpriteRenderer>();
        MyRenderer.sprite = GameManager.Instance.IntelligonSprite;
        MyRenderer.color = MyColor;
        //MyRenderer.sortingLayerName = "Game";
        //MyRenderer.sortingOrder = 0;

        CircleCollider = gameObject.AddComponent<CircleCollider2D>();
        CircleCollider.isTrigger = true;

        var rigid = gameObject.AddComponent<Rigidbody2D>();
        rigid.mass = 1;
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigid.isKinematic = true;
        rigid.gravityScale = 0;
    }

    void Start()
    {
        GameManager.Instance.GameCamera.GetComponent<CameraFollow>().Target = transform;
        Rebuild();
    }

    /// <summary>
	/// 允许一个物体扮演多个角色
	/// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        var atom = other.GetComponent<Atom>();
        if (atom)
        {
            if (atom.IsBadAtom)
            {
                Debug.LogWarning("DIE!");
            }
            else
            {
                MotionState = MotionStateEnum.Docking;
                DockingAtom = atom;
                atom.DockingIntelligon = this;
                var dir = transform.localPosition - DockingAtom.transform.localPosition;
                AngularPositionOnCurrentAtom = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
            }
        }

        var wall = other.GetComponent<Wall>();
        if (wall)
        {
            Debug.LogWarning("Hit Wall!");
        }


    }

	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetMouseButtonUp(0))
	    {
	        Shoot();
	    }

        //检测
        if (MotionState == MotionStateEnum.Straight)
        {
            if (GameManager.GameTime - _straightFlyStartTime >= StraightFlyTime)
            {
                MotionState = MotionStateEnum.Falling;
            }
        }

        //动力学，运动学
	    if (MotionState == MotionStateEnum.Docking)
	    {
	        AngularPositionOnCurrentAtom += DockingAtom.CircularDir*DockingAtom.OrbitSpeed*Time.deltaTime;
	        var rad = DockingAtom.OrbitRadius;
	        var posOnSS = new Vector3(rad*Mathf.Cos(AngularPositionOnCurrentAtom*Mathf.Deg2Rad),
	            rad*Mathf.Sin(AngularPositionOnCurrentAtom*Mathf.Deg2Rad), 0);
	        transform.localPosition = DockingAtom.transform.localPosition + posOnSS;
	    }
        else if (MotionState == MotionStateEnum.Straight)
        {
            transform.localPosition += FreeVelocity.ToVector3() * Time.deltaTime;
        }
        else if (MotionState == MotionStateEnum.Falling)
        {
            AtomAttraction nearestAtomAttraction = null;
            float nearestDistanceSqr = 0;
            foreach (var atomAttraction in GameManager.Instance.AtomAttractionList)
            {
                if (nearestAtomAttraction)
                {
                    var distanceSqr = Vector2.SqrMagnitude(atomAttraction.Atom.Position - Position);
                    if (distanceSqr < nearestDistanceSqr)
                    {
                        nearestAtomAttraction = atomAttraction;
                        nearestDistanceSqr = distanceSqr;
                    }
                }
                else
                {
                    nearestAtomAttraction = atomAttraction;
                    nearestDistanceSqr = Vector2.SqrMagnitude(nearestAtomAttraction.Atom.Position - Position);
                }
            }
            if (nearestAtomAttraction)
            {
                var a = AttractionAmount * nearestAtomAttraction.Amount/nearestDistanceSqr*
                        (nearestAtomAttraction.Atom.Position - Position).normalized;
                FreeVelocity += a*Time.deltaTime;
                if (FreeVelocity.sqrMagnitude > VelocityMax*VelocityMax)
                {
                    FreeVelocity = FreeVelocity.normalized*VelocityMax;
                }
            }
            transform.localPosition += FreeVelocity.ToVector3() * Time.deltaTime;
        }
	}

    public void Shoot()
    {
        if (MotionState != MotionStateEnum.Docking) return;

        var rad = AngularPositionOnCurrentAtom * Mathf.Deg2Rad;
        FreeVelocity = 100 * new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        DockingAtom.DockingIntelligon = null;
        DockingAtom = null;
        MotionState = MotionStateEnum.Straight;
        _straightFlyStartTime = GameManager.GameTime;
    }

    public void Rebuild()
    {
        CircleCollider.radius = Radius;
        var scale = Radius / 512f;
        if (MyRenderer) MyRenderer.transform.localScale = new Vector3(scale, scale, 1);

    }

    #region 编辑器用
#if UNITY_EDITOR

    public override bool SolidGizmo { get { return true; } }
    void RefreshRendererSize()
    {
        var scale = GetComponent<CircleCollider2D>().radius / 512f;
        if (MyRenderer) MyRenderer.transform.localScale = new Vector3(scale, scale, 1);
    }
#endif
    #endregion
}