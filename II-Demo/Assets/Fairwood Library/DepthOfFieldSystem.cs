using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 景深系统
/// </summary>
public class DepthOfFieldSystem : MonoBehaviour
{
    public float FarthestZ = 100;
    public Transform TrackTarget;
    public Vector2 Offset;
    public float Factor = 1;

    private Vector3 _lastTargetPosition;

    void Start()
    {
        ForceRefreshChildrenPosition();
    }
    void Update()
    {
        if (_lastTargetPosition != TrackTarget.localPosition)
        {
            ForceRefreshChildrenPosition();
        }
    }

    void ForceRefreshChildrenPosition()
    {
        _lastTargetPosition = TrackTarget.localPosition;
        var d = (_lastTargetPosition.ToVector2() + Offset) * Factor;
        foreach (Transform child in transform)
        {
            var pos = (1 - child.localPosition.z / FarthestZ) * d;
            child.localPosition = pos.ToVector3(child.localPosition.z);
        }
    }
}