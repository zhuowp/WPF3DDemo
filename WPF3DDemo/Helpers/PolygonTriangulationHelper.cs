using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using WPF3DDemo.Models;

namespace WPF3DDemo.Helpers
{
    public static class PolygonTriangulationHelper
    {
        #region Private Methods

        /// <summary>
        /// 顶点顺序为顺时针
        /// </summary>
        private static bool IsClockwise(List<Vertex2D> polygon)
        {
            return true;
            int cw = 0;
            for (int i = 0, j = 1, k = 2; i < polygon.Count; i++, j++, k++)
            {
                if (j >= polygon.Count)
                {
                    j = j % polygon.Count;
                }

                if (k >= polygon.Count)
                {
                    k = k % polygon.Count;
                }

                Vector3D v1 = new Vector3D(polygon[j].X - polygon[i].X, polygon[j].Y - polygon[i].Y, 0);
                Vector3D v2 = new Vector3D(polygon[k].X - polygon[j].X, polygon[k].Y - polygon[j].Y, 0);
                Vector3D v3 = Vector3D.CrossProduct(v1, v2);

                if (v3.Z < 0)
                {
                    cw++;
                }
                else if (v3.Z > 0)
                {
                    cw--;
                }

                //if (Vertex2D.Cross(polygon[j] - polygon[i], polygon[k] - polygon[j]) > 0)
                //{
                //    cw++;
                //}
                //else if (Vertex2D.Cross(polygon[j] - polygon[i], polygon[k] - polygon[j]) == 0)
                //{
                //}
                //else
                //{
                //    cw--;
                //}
            }

            Console.WriteLine(cw);
            return cw > 0;
        }

        /// <summary>
        ///  测试点是否在三角形内部
        /// </summary>
        /// <param name="point">测试的点</param>
        /// <param name="triA">三角形顶点A</param>
        /// <param name="triB">三角形顶点B</param>
        /// <param name="triC">三角形顶点C</param>
        /// <returns></returns>
        private static bool TestInTriangle(Vertex2D point, Vertex2D triA, Vertex2D triB, Vertex2D triC)
        {
            Vertex2D AB = triB - triA, AC = triC - triA, BC = triC - triB, AD = point - triA;
            bool ABxAC = Vertex2D.Cross(AB, AC) >= 0;
            return (ABxAC ^ Vertex2D.Cross(AB, AD) < 0) &&
                   (Vertex2D.Cross(BC, AB) > 0 ^ Vertex2D.Cross(BC, point - triB) >= 0) &&
                   (ABxAC ^ Vertex2D.Cross(AC, AD) >= 0);
        }

        /// <summary>
        /// 当给定的点与三角形的每一个顶点都在其另外两个定点所组成的边的同一边时，点在三角形内部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="vertexA"></param>
        /// <param name="vertexB"></param>
        /// <param name="vertexC"></param>
        /// <returns></returns>
        private static bool IsPointInTriangle(Vertex2D vertexA, Vertex2D vertexB, Vertex2D vertexC, Vertex2D point)
        {
            return IsTwoPointOnTheSameSideOfLine(vertexA, vertexB, vertexC, point)
                && IsTwoPointOnTheSameSideOfLine(vertexA, vertexC, vertexB, point)
                && IsTwoPointOnTheSameSideOfLine(vertexB, vertexC, vertexA, point);
        }

        private static bool IsTwoPointOnTheSameSideOfLine(Vertex2D lineVertexA, Vertex2D lineVertexB, Vertex2D pointC, Vertex2D pointD)
        {
            Vector vectorAB = VertexToVector(lineVertexA, lineVertexB);
            Vector vectorAC = VertexToVector(lineVertexA, pointC);
            Vector vectorAD = VertexToVector(lineVertexA, pointD);

            //叉乘同号
            return !((Vector.CrossProduct(vectorAB, vectorAC) > 0) ^ (Vector.CrossProduct(vectorAB, vectorAD) >= 0));
        }

        public static Vector VertexToVector(Vertex2D vertex1, Vertex2D vertex2)
        {
            return new Vector()
            {
                X = vertex2.X - vertex1.X,
                Y = vertex2.Y - vertex1.Y
            };
        }

        private static bool IsVertexConvex(Vertex2D currentVertex, Vertex2D previeVertex, Vertex2D nextVertex)
        {
            Vector vector1 = VertexToVector(previeVertex, currentVertex);
            Vector vector2 = VertexToVector(currentVertex, nextVertex);

            return Vector.CrossProduct(vector1, vector2) <= 0;
        }

        private static void UpdateVertexStatus(LinkedListNode<Vertex2D> node)
        {
            Vertex2D current = node.Value;
            Vertex2D prev = node.Previous != null ? node.Previous.Value : node.List.Last.Value;
            Vertex2D next = node.Next != null ? node.Next.Value : node.List.First.Value;

            //首先判断该点是否是凸点
            current.IsConvex = IsVertexConvex(prev, current, next);
            if (!current.IsConvex)
            {
                current.IsSeparable = false;
                return;
            }

            //检查所有剩余点是否在此三角形内，判断是否可分离
            foreach (Vertex2D vertex in node.List)
            {
                if (vertex != current && vertex != prev && vertex != next)
                {
                    //if (TestInTriangle(vertex, current, prev, next))
                    if (IsPointInTriangle(prev, current, next, vertex))
                    {
                        // 有一个多边形的顶点在三角形內，则此三角形不可分离
                        current.IsSeparable = false;
                        return;
                    }
                }
            }

            //if(!current.IsConvex)//凹点绝对不可分
            //{
            //    current.IsSeparable = false;
            //}
            //else//凸点需要进一步判断是否右内点
            //{
            //    bool hasInnerPoint = IsTriangleHasInnerPoint(current, prev, next, node.List);
            //}
            current.IsSeparable = true;
        }

        public static bool IsTriangleHasInnerPoint(Vertex2D currentVertex, Vertex2D previewVertex, Vertex2D nextVertex, IEnumerable<Vertex2D> allVertexCollection)
        {
            bool isSeparable = true;
            foreach(Vertex2D vertex in allVertexCollection)
            {
                // 有一个多边形的顶点在三角形內，则此三角形不可分离
                if (IsPointInTriangle(previewVertex, currentVertex, nextVertex, vertex))
                {
                    isSeparable = false;
                    break;
                }
            }

            return isSeparable;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 分解多边形为三角形，返回分解后的三角形顶点在polygon中的索引
        /// </summary>
        /// <param name="polygon">输入多边形</param>
        public static List<int> ResolveToTriangles(List<Vertex2D> polygon)
        {
            if (polygon.Count < 3)
            {
                return null;
            }

            bool isClockwise = IsClockwise(polygon);
            LinkedList<Vertex2D> vertexLinkedList = new LinkedList<Vertex2D>();
            // 确保顺序为顺时针，逆时针则反向插入
            if (isClockwise)
            {
                foreach (Vertex2D vertex in polygon)
                {
                    vertexLinkedList.AddLast(vertex);
                }
            }
            else
            {
                foreach (Vertex2D vertex in polygon)
                {
                    vertexLinkedList.AddFirst(vertex);
                }
            }

            //更新点的凹凸性
            for (LinkedListNode<Vertex2D> node = vertexLinkedList.First; node != null; node = node.Next)
            {
                Vertex2D current = node.Value;
                Vertex2D prev = node.Previous != null ? node.Previous.Value : node.List.Last.Value;
                Vertex2D next = node.Next != null ? node.Next.Value : node.List.First.Value;

                if (!current.IsConvex)//凸点永远是凸点，只有凹点才有可能改变为凸点
                {
                    // 之前是凹点，则判断此次是否为凸点
                    if (Vertex2D.Cross(current - prev, next - current) >= 0)
                    {
                        current.IsConvex = true;
                        node.Value = current;
                    }
                }
            }

            // 可分离的点
            LinkedList<LinkedListNode<Vertex2D>> separableVertexList = new LinkedList<LinkedListNode<Vertex2D>>();
            for (LinkedListNode<Vertex2D> node = vertexLinkedList.First; node != null; node = node.Next)
            {
                UpdateVertexStatus(node);
                if (node.Value.IsSeparable)
                {
                    separableVertexList.AddFirst(node);
                }
            }

            // 开始分离
            List<int> tris = new List<int>();

            while (vertexLinkedList.Count >= 3)
            {
                if (separableVertexList.Count == 0)
                {
                    break;
                }

                LinkedListNode<Vertex2D> current = separableVertexList.First.Value;
                separableVertexList.RemoveFirst();

                LinkedListNode<Vertex2D> prev = current.Previous ?? current.List.Last;
                LinkedListNode<Vertex2D> next = current.Next ?? current.List.First;

                tris.Add(current.Value.Index);
                tris.Add(prev.Value.Index);
                tris.Add(next.Value.Index);

                vertexLinkedList.Remove(current);

                // 更新可分离点状态
                if (prev.Value.IsSeparable)
                {
                    UpdateVertexStatus(prev);
                    if (!prev.Value.IsSeparable)
                    {
                        separableVertexList.Remove(prev);
                    }
                }
                else
                {
                    UpdateVertexStatus(prev);
                    if (prev.Value.IsSeparable)
                    {
                        separableVertexList.AddFirst(prev);
                    }
                }

                if (next.Value.IsSeparable)
                {
                    UpdateVertexStatus(next);
                    if (!next.Value.IsSeparable) separableVertexList.Remove(next);
                }
                else
                {
                    UpdateVertexStatus(next);
                    if (next.Value.IsSeparable) separableVertexList.AddFirst(next);
                }
            }

            return tris;
        }

        #endregion

    }
}
