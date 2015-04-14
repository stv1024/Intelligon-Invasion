using UnityEngine;

/// <summary>
/// 游戏界面，角色视觉层容器，是个梯形。坐标Pivot在中下，所以x∈[-480,480],y∈[0,640]
/// </summary>
public class GameVisual
{
    ///// <summary>
    ///// 远边透视后的长度
    ///// </summary>
    //public const float FarSideWidth = 830;
    ///// <summary>
    ///// 近边透视后的长度
    ///// </summary>
    //public const float NearSideWidth = 960;
    ///// <summary>
    ///// 透视后近边到远边的距离
    ///// </summary>
    //public const float NearFarDistance = 525;

    public static int VisualY2SortingOrder(float visY)
    {
        return 320 - Mathf.RoundToInt(visY);
    }

    /// <summary>
    /// 物体、子物体的所有Renderer的2D层次调整
    /// </summary>
    /// <param name="go"></param>
    /// <param name="visY"></param>
    /// <param name="extra">额外增加多少</param>
    /// <returns>有多少个renderer</returns>
    public static int SetAllRenderers2DLayer(GameObject go, float visY, int extra = 0)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        foreach (var renderer1 in renderers)
        {
            renderer1.sortingLayerID = MainRoot.LayerID_Game;
            renderer1.sortingOrder = VisualY2SortingOrder(visY) + extra;
        }
        return renderers.Length;
    }
    /// <summary>
    /// 物体、子物体的所有Renderer的2D层次调整
    /// </summary>
    /// <param name="go"></param>
    /// <returns>有多少个renderer</returns>
    public static int SetAllRenderersSortingLayer(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        foreach (var renderer1 in renderers)
        {
            renderer1.sortingLayerID = MainRoot.LayerID_Game;
        }
        return renderers.Length;
    }
}