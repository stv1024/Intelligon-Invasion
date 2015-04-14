using UnityEngine;

/// <summary>
/// 原子吸引力
/// </summary>
[RequireComponent(typeof(Atom))]
public class AtomAttraction : MonoBehaviour
{
    public float Amount = 100;

    public Atom Atom;

    void Awake()
    {
        GameManager.Instance.AtomAttractionList.Add(this);
        Atom = GetComponent<Atom>();
    }

    void OnDestroy()
    {
        GameManager.Instance.AtomAttractionList.Remove(this);
    }
}