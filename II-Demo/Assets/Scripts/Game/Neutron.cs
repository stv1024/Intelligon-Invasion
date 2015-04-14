using Fairwood;
using UnityEngine;

public class Neutron : Entity
{
    [Range(1, 100)]
    public float Radius;

    public SpriteRenderer MyRenderer;

    public CircleCollider2D CircleCollider;

    public override Vector2 Velocity
    {
        get { return rigidbody2D.velocity; }
        set { rigidbody2D.velocity = value; }
    }

	void Awake ()
	{
        GameManager.Instance.NeutronList.Add(this);

        var graphic = new GameObject("Atom Graphic");
        graphic.layer = GameManager.LAYER_ID_MAIN;
        graphic.transform.ResetTransform(transform);
        MyRenderer = graphic.AddComponent<SpriteRenderer>();
        MyRenderer.sprite = GameManager.Instance.NeutronGraphic;
        MyRenderer.color = new Color32(230, 255, 0, 255);

        CircleCollider = gameObject.AddComponent<CircleCollider2D>();
        CircleCollider.isTrigger = true;
        var rigid = gameObject.AddComponent<Rigidbody2D>();
        rigid.mass = 1;
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigid.isKinematic = false;
        rigid.gravityScale = 0;
	}

    void Start()
    {
        Rebuild();
    }

    void OnDestroy()
    {
        GameManager.Instance.NeutronList.Remove(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var atomDecay = other.GetComponent<AtomDecay>();
        if (atomDecay)
        {
            atomDecay.AbsorbedNeutrons += 1;
            Die();
        }
    }

    public void Die()
    {
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
    void OnDrawGizmos()
    {
        Gizmos.color = new Color32(230, 255, 0, 255);
        Gizmos.DrawSphere(transform.position, 8);
    }

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