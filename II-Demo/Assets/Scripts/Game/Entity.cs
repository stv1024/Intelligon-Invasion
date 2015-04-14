using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public virtual Vector2 Position
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    public float TimeScale = 1;

    public float Dt
    {
        get { return GameManager.DeltaTime; }
    }

    public virtual Vector2 Velocity { get; set; }
}

public enum EntityTag
{
    Intelligon,
    Atom,
    HiEnProton,
    Wall
}