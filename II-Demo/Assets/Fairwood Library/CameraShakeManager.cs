using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 摄像机抖动管理器
/// </summary>
public class CameraShakeManager : MonoBehaviour
{
    #region 自动创建的单例

    private static CameraShakeManager _instance;

    public static CameraShakeManager Instance
    {
        get
        {
            TrySpawn();
            return _instance;
        }
    }

    /// <summary>
    /// 手动创建单例，不会重复的
    /// </summary>
    public static void TrySpawn()
    {
        if (!_instance)
        {
            var go = new GameObject("_CameraShakeManager");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<CameraShakeManager>(); //这时候会触发Awake()
        }
    }

    #endregion

    public Camera CameraToShake;

    public AnimationCurve SlightlyShakeCameraCurve;
    public float ShakeMagnitude = 2f;

    /// <summary>
    /// 振动摄像机，方向含振幅
    /// </summary>
    /// <param name="dir">未必归一化</param>
    public void SlightlyShakeCamera(Vector2 dir)
    {
        _shakeCameraCurves.Add(SlightlyShakeCameraCurve);
        _shakeCameraDirections.Add(dir);
        _shakeCameraCurveSamplePositions.Add(0);
    }

    private readonly List<float> _shakeCameraCurveSamplePositions = new List<float>(); //三者必须长度相同
    private readonly List<Vector2> _shakeCameraDirections = new List<Vector2>(); //方向，不是归一化的，包含振幅信息
    private readonly List<AnimationCurve> _shakeCameraCurves = new List<AnimationCurve>();
    private bool _lastFrameHasNoCurves; //上一帧就已经没有曲线了。如果这一帧仍无曲线，则不需要再动摄像机，让摄像机可以做其他运动。

    private void Update()
    {
        if (!_lastFrameHasNoCurves || _shakeCameraCurves.Count > 0)
        {
            var d = Vector2.zero;
            for (int i = 0; i < _shakeCameraCurves.Count; i++)
            {
                _shakeCameraCurveSamplePositions[i] += Time.deltaTime;
                if (_shakeCameraCurveSamplePositions[i] > _shakeCameraCurves[i][_shakeCameraCurves[i].length - 1].time)
                {
                    _shakeCameraCurveSamplePositions.RemoveAt(i);
                    _shakeCameraDirections.RemoveAt(i);
                    _shakeCameraCurves.RemoveAt(i);
                }
                else
                {
                    d += _shakeCameraDirections[i]*_shakeCameraCurves[i].Evaluate(_shakeCameraCurveSamplePositions[i])*
                         ShakeMagnitude;
                }
            }
            CameraToShake.transform.localPosition = d.ToVector3(CameraToShake.transform.localPosition.z);
        }
        _lastFrameHasNoCurves = _shakeCameraCurves.Count == 0;
    }
}