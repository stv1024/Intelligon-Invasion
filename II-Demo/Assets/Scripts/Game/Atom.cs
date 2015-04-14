using Fairwood;
using UnityEngine;

public class Atom : BaseParticle
{
    [UnityEngine.Range(1, 40)]
    public float OrbitHeight = 10;
    [UnityEngine.Range(1,500)]
    public float OrbitSpeed = 180;
    /// <summary>
    /// 轨道方向，±1，+1表示角速度向上
    /// </summary>
    public int CircularDir = 1;

    public bool IsBadAtom;

    public Color GoodColor = new Color32(136, 120, 255, 255);
    public Color BadColor = new Color32(255, 47, 47, 255);

    public float OrbitRadius { get { return Radius + OrbitHeight; } }
    public override Color MyColor { get { return IsBadAtom ? BadColor : GoodColor; } }

    public Intelligon DockingIntelligon;

    void Awake ()
	{
	    GameManager.Instance.AtomList.Add(this);

        var graphic = new GameObject("Atom Graphic");
        graphic.layer = GameManager.LAYER_ID_MAIN;
        graphic.transform.ResetTransform(transform);
        MyRenderer = graphic.AddComponent<SpriteRenderer>();
        MyRenderer.sprite = GameManager.Instance.AtomSprite;
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
        Rebuild();
    }

    void OnDestroy()
    {
        GameManager.Instance.AtomList.Remove(this);
    }

    public void Die()
    {
        if (DockingIntelligon)
        {
            DockingIntelligon.Shoot();
        }
        Destroy(gameObject);
    }

    public void Rebuild()
    {
        CircleCollider.radius = Radius;
        var scale = Radius / 512f;
        if (MyRenderer) MyRenderer.transform.localScale = new Vector3(scale, scale, 1);
        
    }

    #region 编辑器用
#if UNITY_EDITOR

    [ContextMenu("Preview")]
    void Preview()
    {
        SendMessage("_Preview");
    }
    void _Preview()
    {
        if (CircleCollider) return;
        Awake();
    }

    [ContextMenu("Quit Preview")]
    void QuitPreview()
    {
        SendMessage("_RecoverInit");
    }
    void _RecoverInit()
    {
        Destroy(GetComponent<CircleCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        if (MyRenderer) Destroy(MyRenderer.gameObject);
    }

#endif
    #endregion
}
