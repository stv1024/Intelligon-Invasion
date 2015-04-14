using UnityEngine;

/// <summary>
/// 粒子基类，可派生Atom, Intelligon, Neutron
/// </summary>
public abstract class BaseParticle : Entity
{

    [UnityEngine.Range(1, 200)]
    public float Radius = 25;

    public SpriteRenderer MyRenderer;
    public CircleCollider2D CircleCollider;

    public abstract Color MyColor { get; }
    public override Vector2 Velocity
    {
        get { return rigidbody2D.velocity; }
        set { rigidbody2D.velocity = value; }
    }

    #region 编辑器用
#if UNITY_EDITOR

    public virtual bool SolidGizmo { get { return false; } }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = MyColor;
        if (SolidGizmo)
            Gizmos.DrawSphere(transform.position, Radius);
        else
            Gizmos.DrawWireSphere(transform.position, Radius);
    }

#endif
    #endregion
}