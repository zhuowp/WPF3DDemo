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
        /// 判断点是否在给定定点所组成的三角形内部
        /// 当给定的点与三角形的每一个顶点都在其另外两个顶点所组成的边的同一边时，点在三角形内部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="vertexA"></param>
        /// <param name="vertexB"></param>
        /// <param name="vertexC"></param>
        /// <returns></returns>
        private static bool IsPointInTriangle(Vertex2D vertexA, Vertex2D vertexB, Vertex2D vertexC, Vertex2D point)
        {
            return IsTwoPointsOnTheSameSideOfLine(vertexA, vertexB, vertexC, point)
                && IsTwoPointsOnTheSameSideOfLine(vertexA, vertexC, vertexB, point)
                && IsTwoPointsOnTheSameSideOfLine(vertexB, vertexC, vertexA, point);
        }

        /// <summary>
        /// 判断两个点是否在给定一条直线的同一边
        /// </summary>
        /// <param name="lineVertexA"></param>
        /// <param name="lineVertexB"></param>
        /// <param name="pointC"></param>
        /// <param name="pointD"></param>
        /// <returns></returns>
        private static bool IsTwoPointsOnTheSameSideOfLine(Vertex2D lineVertexA, Vertex2D lineVertexB, Vertex2D pointC, Vertex2D pointD)
        {
            Vector vectorAB = VertexToVector(lineVertexA, lineVertexB);
            Vector vectorAC = VertexToVector(lineVertexA, pointC);
            Vector vectorAD = VertexToVector(lineVertexA, pointD);

            //叉乘同号
            return (Vector.CrossProduct(vectorAB, vectorAC) >= 0 && Vector.CrossProduct(vectorAB, vectorAD) >= 0)
                || (Vector.CrossProduct(vectorAB, vectorAC) <= 0 && Vector.CrossProduct(vectorAB, vectorAD) <= 0);
        }

        /// <summary>
        /// 判断所给定点是否是凸点
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <param name="previeVertex"></param>
        /// <param name="nextVertex"></param>
        /// <returns></returns>
        private static bool IsVertexConvex(Vertex2D currentVertex, Vertex2D previeVertex, Vertex2D nextVertex)
        {
            return CrossProductOfThreePoints(currentVertex, previeVertex, nextVertex) < 0;
        }

        /// <summary>
        /// 判断三个顶点是否共线
        /// </summary>
        /// <param name="current"></param>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private static bool IsThreeVertexInLine(Vertex2D current, Vertex2D prev, Vertex2D next)
        {
            return CrossProductOfThreePoints(current, prev, next) == 0;
        }

        /// <summary>
        /// 三点组成的两个向量的叉积，两向量分别为前点指向后点
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <param name="previeVertex"></param>
        /// <param name="nextVertex"></param>
        /// <returns></returns>
        private static double CrossProductOfThreePoints(Vertex2D currentVertex, Vertex2D previeVertex, Vertex2D nextVertex)
        {
            Vector vector1 = VertexToVector(previeVertex, currentVertex);
            Vector vector2 = VertexToVector(currentVertex, nextVertex);

            return Vector.CrossProduct(vector1, vector2);
        }

        /// <summary>
        /// 判断所给定点组成的三角形是否存在内点
        /// </summary>
        /// <param name="currentVertex"></param>
        /// <param name="previewVertex"></param>
        /// <param name="nextVertex"></param>
        /// <param name="allVertexCollection"></param>
        /// <returns></returns>
        private static bool IsTriangleHasInnerPoint(Vertex2D currentVertex, Vertex2D previewVertex, Vertex2D nextVertex, IEnumerable<Vertex2D> allVertexCollection)
        {
            bool hasInnerPoint = false;
            foreach (Vertex2D vertex in allVertexCollection)
            {
                if (vertex.Index == currentVertex.Index || vertex.Index == previewVertex.Index || vertex.Index == nextVertex.Index)
                {
                    continue;
                }

                if (IsPointInTriangle(previewVertex, currentVertex, nextVertex, vertex))
                {
                    hasInnerPoint = true;
                    break;
                }
            }

            return hasInnerPoint;
        }

        /// <summary>
        /// 将给定两个定点转为矢量
        /// </summary>
        /// <param name="vertex1"></param>
        /// <param name="vertex2"></param>
        /// <returns></returns>
        private static Vector VertexToVector(Vertex2D vertex1, Vertex2D vertex2)
        {
            return new Vector()
            {
                X = vertex2.X - vertex1.X,
                Y = vertex2.Y - vertex1.Y
            };
        }

        /// <summary>
        /// 判断顶点是否可以分割
        /// </summary>
        /// <param name="node"></param>
        private static void UpdateVertexSeparableStatus(LinkedListNode<Vertex2D> node)
        {
            Vertex2D current = node.Value;
            Vertex2D prev = node.Previous != null ? node.Previous.Value : node.List.Last.Value;
            Vertex2D next = node.Next != null ? node.Next.Value : node.List.First.Value;

            //如果当前点与前后点是同一个点，则当前点直接可分割
            if (current == prev || current == next || prev == next)//prev == next 存疑
            {
                current.IsSeparable = true;
                return;
            }

            //如果当前点与前后点在同一直线上，则当前点直接可分割
            if (IsThreeVertexInLine(current, prev, next))
            {
                current.IsSeparable = true;
                return;
            }

            //判断当前点的凹凸性
            current.IsConvex = IsVertexConvex(prev, current, next);
            if (!current.IsConvex)//凹点绝对不可分
            {
                current.IsSeparable = false;
            }
            else//凸点需要进一步判断三角形内部是否存在内点，有内点则不可分
            {
                bool triangleHasInnerPoint = IsTriangleHasInnerPoint(current, prev, next, node.List);
                current.IsSeparable = !triangleHasInnerPoint;
            }
        }

        private static void UpdateSeparableList(LinkedList<LinkedListNode<Vertex2D>> separableVertexList, LinkedListNode<Vertex2D> prev)
        {
            bool oldVertexSeparable = prev.Value.IsSeparable;
            UpdateVertexSeparableStatus(prev);
            bool newVertexSeparable = prev.Value.IsSeparable;

            if (oldVertexSeparable && !newVertexSeparable)
            {
                separableVertexList.Remove(prev);
            }
            else if (!oldVertexSeparable && newVertexSeparable)
            {
                separableVertexList.AddFirst(prev);
            }
        }

        private static LinkedList<Vertex2D> InitVertexLinkedList(List<Vertex2D> polygon)
        {
            //初始化链表数据
            LinkedList<Vertex2D> vertexLinkedList = new LinkedList<Vertex2D>();

            // 确保顺序为顺时针，逆时针则反向插入
            bool isClockwise = IsClockwise(polygon);
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

            return vertexLinkedList;
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

            //初始化链表数据
            LinkedList<Vertex2D> vertexLinkedList = InitVertexLinkedList(polygon);

            // 初始化可分离的点
            LinkedList<LinkedListNode<Vertex2D>> separableVertexList = InitAllSeparableVetextLiinkedList(vertexLinkedList);

            // 开始分离
            List<int> triangleIndices = new List<int>();
            while (vertexLinkedList.Count >= 3)
            {
                if (separableVertexList.Count == 0)
                {
                    break;
                }

                LinkedListNode<Vertex2D> currentSeparableVertex = separableVertexList.First.Value;
                LinkedListNode<Vertex2D> prev = currentSeparableVertex.Previous ?? currentSeparableVertex.List.Last;
                LinkedListNode<Vertex2D> next = currentSeparableVertex.Next ?? currentSeparableVertex.List.First;

                //添加分离三角形的顶点索引
                triangleIndices.Add(currentSeparableVertex.Value.Index);
                triangleIndices.Add(prev.Value.Index);
                triangleIndices.Add(next.Value.Index);

                //在顶点链表中移出当前顶点               
                separableVertexList.RemoveFirst();
                vertexLinkedList.Remove(currentSeparableVertex);

                // 移出当前点后，会使当前点前后两点的可分离状态发生变化，所以需要更新前后两点的状态
                UpdateSeparableList(separableVertexList, prev);
                UpdateSeparableList(separableVertexList, next);
            }

            return triangleIndices;
        }

        private static LinkedList<LinkedListNode<Vertex2D>> InitAllSeparableVetextLiinkedList(LinkedList<Vertex2D> vertexLinkedList)
        {
            LinkedList<LinkedListNode<Vertex2D>> separableVertexList = new LinkedList<LinkedListNode<Vertex2D>>();
            for (LinkedListNode<Vertex2D> node = vertexLinkedList.First; node != null; node = node.Next)
            {
                UpdateVertexSeparableStatus(node);
                if (node.Value.IsSeparable)
                {
                    separableVertexList.AddFirst(node);
                }
            }

            return separableVertexList;
        }

        /// <summary>
        /// 判断定点顺序是否为顺时针
        /// </summary>
        public static bool IsClockwise(List<Vertex2D> polygon)
        {
            if(polygon.Count <3)
            {
                return false;
            }

            //如果首位相连，则不考虑最后一个点
            int totalVertexCount = polygon.Count;
            if (polygon[0] == polygon[polygon.Count - 1])
            {
                totalVertexCount = polygon.Count - 1;
            }

            //寻找最右端的点
            double rightMostX = polygon[0].X;
            int rightMostPointIndex = 0;
            for (int i = 1; i < totalVertexCount; i++)
            {
                if (rightMostX < polygon[i].X)
                {
                    rightMostX = polygon[i].X;
                    rightMostPointIndex = i;
                }
            }

            int previewIndex = 0;
            int nextIndex = 0;
            if (rightMostPointIndex == 0)
            {
                previewIndex = totalVertexCount - 1;
            }
            else
            {
                previewIndex = rightMostPointIndex - 1;
            }

            if (rightMostPointIndex == totalVertexCount - 1)
            {
                nextIndex = 0;
            }
            else
            {
                nextIndex = rightMostPointIndex++;
            }

            Vector vector1 = VertexToVector(polygon[previewIndex], polygon[rightMostPointIndex]);
            Vector vector2 = VertexToVector(polygon[rightMostPointIndex], polygon[nextIndex]);

            if (Vector.CrossProduct(vector1, vector2) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
