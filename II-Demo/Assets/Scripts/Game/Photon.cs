using UnityEngine;

/// <summary>
/// 光子，仅视觉效果
/// </summary>
public class Photon : Entity
{
    public override Vector2 Velocity
    {
        get { return transform.right; }
        set { transform.right = value; }
    }

    void Start()
    {
        Destroy(gameObject, animation.clip.length);
    }
}