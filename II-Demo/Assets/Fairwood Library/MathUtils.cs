using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fairwood.Math
{
    public class MathUtils
    {
        /// <summary>
        /// 抽到0的概率是0.1，抽到1的概率是0.15，抽到2的概率是0.07，则调用GetRandomIndexInDistribution([0.1,0.15,0.07])来抽取。都没抽中则返回-1。
        /// </summary>
        /// <param name="distribution"></param>
        /// <returns></returns>
        public static int GetRandomIndexInDistribution(List<float> distribution)
        {
            var ran = Random.value;
            var sum = 0f;
            for (var i = 0; i < distribution.Count; i++)
            {
                sum += distribution[i];
                if (ran < sum)
                {
                    return i;
                }
            }
            return -1;
        }

        public static string GetUnifiedWxPromotionCode(DateTime date)
        {
            var d = date.Year*10000 + date.Month*100 + date.Day;
            return ((d%9839 + 7)*(d%9791 + 7)%9973).ToString("0000");
            //var ori = SDBMHash(date.ToShortDateString()) % 1000000 / 100;
            //var ori = SDBMHash(date.ToString("MM/dd/yy")) % 100000 / 10;
            //return ori.ToString("0000");
        }
        public static int SDBMHash(string str)
        {
            int hash = 0;
            var i = 0;
            while (i < str.Length)
            {
                // equivalent to: hash = 65599*hash + (*str++);
                hash = (str[i]) + (hash << 6) + (hash << 16) - hash;
                i++;
            }

            return (hash & 0x7FFFFFFF);
        }

        /// <summary>
        /// 类似Mathf.InverseLerp但不Clamp01
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float InverseLerpWithoutClamp(float a, float b, float value)
        {
            if (a != b)
                return ((value - a) / (b - a));
            return 0.0f;
        }

        public static class Rect
        {
            /// <summary>
            /// 类似Rect.PointToNormalized但不Clamp01
            /// </summary>
            /// <param name="rectangle"></param>
            /// <param name="point"></param>
            /// <returns></returns>
            public static Vector2 PointToNormalizedWithoutClamp(UnityEngine.Rect rectangle, Vector2 point)
            {
                return new Vector2(InverseLerpWithoutClamp(rectangle.x, rectangle.xMax, point.x), InverseLerpWithoutClamp(rectangle.y, rectangle.yMax, point.y));
            }
        }

        /// <summary>
        /// 获取从from向量到to向量的夹角
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static float DeltaAngle(Vector3 from, Vector3 to, Vector3 up)
        {
            float angle = 0.0f;

            float absAngle = Vector3.Angle(from, to);
            Vector3 crossVec = Vector3.Cross(from, to);
            angle = absAngle * (Vector3.Dot(crossVec, up) > 0 ? 1.0f : -1.0f);

            return angle;
        }
    }
}