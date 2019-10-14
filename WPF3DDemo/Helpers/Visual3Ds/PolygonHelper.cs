using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Helpers.Visual3Ds
{

    public struct Vec
    {
        public float x, y;
        public Vec(float x, float y)
        {
            this.x = x; this.y = y;
        }
        public static float Cross(Vec v1, Vec v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }
        public static Vec operator -(Vec v1, Vec v2)
        {
            return new Vec(v1.x - v2.x, v1.y - v2.y);
        }
    }

    public static class PolygonHelper
    {

        private class PointStatus
        {

            /// <summary>
            /// 顶点位置
            /// </summary>
            public Vec point;

            /// <summary>
            /// 是凸点
            /// </summary>
            public bool isConvex = false;

            /// <summary>
            /// 可分离
            /// </summary>
            public bool isSeparable = false;

            /// <summary>
            /// 所在多边形的索引
            /// </summary>
            public int index = 0;
        }

        /// <summary>
        /// 顶点顺序为顺时针
        /// </summary>
        private static bool IsClockwise(List<Vec> polygon)
        {
            return true;
            int cw = 0;
            for (int i = 0, j = 1, k = 2; i < polygon.Count; i++, j++, k++)
            {
                if (j >= polygon.Count) j -= polygon.Count;
                if (k >= polygon.Count) k -= polygon.Count;
                if (Vec.Cross(polygon[j] - polygon[i], polygon[k] - polygon[j]) >= 0) cw++;
                else cw--;
            }
            return cw >= 0;
        }

        /// <summary>
        ///  测试点是否在三角形内部
        /// </summary>
        /// <param name="point">测试的点</param>
        /// <param name="triA">三角形顶点A</param>
        /// <param name="triB">三角形顶点B</param>
        /// <param name="triC">三角形顶点C</param>
        /// <returns></returns>
        private static bool TestInTriangle(Vec point, Vec triA, Vec triB, Vec triC)
        {
            Vec AB = triB - triA, AC = triC - triA, BC = triC - triB, AD = point - triA;
            bool ABxAC = Vec.Cross(AB, AC) >= 0;
            return (ABxAC ^ Vec.Cross(AB, AD) < 0) &&
                   (Vec.Cross(BC, AB) > 0 ^ Vec.Cross(BC, point - triB) >= 0) &&
                   (ABxAC ^ Vec.Cross(AC, AD) >= 0);
        }

        private static void UpdatePointStatus(LinkedListNode<PointStatus> node)
        {
            PointStatus current = node.Value;
            PointStatus prev = node.Previous != null ? node.Previous.Value : node.List.Last.Value;
            PointStatus next = node.Next != null ? node.Next.Value : node.List.First.Value;

            if (!current.isConvex)
            {
                // 之前是凹点，则判断此次是否为凸点
                if (Vec.Cross(current.point - prev.point, next.point - current.point) >= 0)
                    current.isConvex = true;
                else
                {
                    current.isSeparable = false; // 凹点一定不可分离
                    return;
                }
            }

            // 更新可分离状态
            foreach (PointStatus pointStaus in node.List)
            {
                if (pointStaus != current && pointStaus != prev && pointStaus != next)
                {
                    if (TestInTriangle(pointStaus.point, current.point, prev.point, next.point))
                    {
                        // 有一个多边形的顶点在三角形內，则此三角形不可分离
                        current.isSeparable = false;
                        return;
                    }
                }
            }
            current.isSeparable = true;
        }

        /// <summary>
        /// 分解多边形为三角形，返回分解后的三角形顶点在polygon中的索引
        /// </summary>
        /// <param name="polygon">输入多边形</param>
        public static List<int> Resolve(List<Vec> polygon)
        {
            if (polygon.Count < 3)
            {
                return null;
            }

            bool isCW = IsClockwise(polygon);

            List<int> tris = new List<int>();
            LinkedList<PointStatus> pointStatuses = new LinkedList<PointStatus>();
            for (int i = 0; i < polygon.Count; i++)
            {
                Vec point = polygon[i];
                PointStatus pointStatus = new PointStatus { point = point, index = i };
                // 确保顺序为顺时针，逆时针则反向插入
                if (isCW)
                {
                    pointStatuses.AddLast(pointStatus);
                }
                else
                {
                    pointStatuses.AddFirst(pointStatus);
                }
            }

            // 可分离的点
            LinkedList<LinkedListNode<PointStatus>> separablePointStatuses = new LinkedList<LinkedListNode<PointStatus>>();
            for (LinkedListNode<PointStatus> node = pointStatuses.First; node != null; node = node.Next)
            {
                UpdatePointStatus(node);
                if (node.Value.isSeparable)
                {
                    separablePointStatuses.AddFirst(node);
                }
            }

            // 开始分离
            while (pointStatuses.Count >= 3)
            {
                if (separablePointStatuses.Count == 0)
                {
                    break;
                    return null; // 分离失败，例如自身相交情况
                }

                LinkedListNode<PointStatus> current = separablePointStatuses.First.Value;
                separablePointStatuses.RemoveFirst();

                LinkedListNode<PointStatus> prev = current.Previous ?? current.List.Last;
                LinkedListNode<PointStatus> next = current.Next ?? current.List.First;

                pointStatuses.Remove(current);
                tris.Add(current.Value.index);
                tris.Add(prev.Value.index);
                tris.Add(next.Value.index);

                // 更新可分离点状态
                NewMethod(separablePointStatuses, prev);
                NewMethod(separablePointStatuses, next);
            }
            return tris;
        }

        private static void NewMethod(LinkedList<LinkedListNode<PointStatus>> separablePointStatuses, LinkedListNode<PointStatus> next)
        {
            bool oldSeparable = next.Value.isSeparable;

            UpdatePointStatus(next);
            bool newSeparable = next.Value.isSeparable;

            if (oldSeparable && !newSeparable)
            {
                separablePointStatuses.Remove(next);
            }
            else if (!oldSeparable && newSeparable)
            {
                separablePointStatuses.AddFirst(next);
            }
        }
    }
}
