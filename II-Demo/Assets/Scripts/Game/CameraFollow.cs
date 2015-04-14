using Fairwood.Math;
using UnityEngine;

/// <summary>
/// Summary
/// </summary>
[AddComponentMenu("Camera-Control/Smooth Follow")]
public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Damping = 3.0f;

    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!Target)
            return;

        var pos = Vector2.Lerp(transform.position, Target.position, Damping*Time.deltaTime);

        transform.position = pos.ToVector3(transform.position.z);
    }
}