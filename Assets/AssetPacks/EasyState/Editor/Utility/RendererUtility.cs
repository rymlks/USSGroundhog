using EasyState.Core.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Utility
{
    public static class RendererUtility
    {
        public static void DrawLine(Vector3[] points, float thickness, Color color, MeshGenerationContext context)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<ushort> indices = new List<ushort>();
            for (int i = 0; i < points.Length - 1; i++)
            {
                var pointA = points[i];
                var pointB = points[i + 1];

                float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x);
                float offsetX = thickness / 2 * Mathf.Sin(angle);
                float offsetY = thickness / 2 * Mathf.Cos(angle);

                vertices.Add(new Vertex()
                {
                    position = new Vector3(pointA.x + offsetX, pointA.y - offsetY, Vertex.nearZ),
                    tint = color
                });
                vertices.Add(new Vertex()
                {
                    position = new Vector3(pointB.x + offsetX, pointB.y - offsetY, Vertex.nearZ),
                    tint = color
                });
                vertices.Add(new Vertex()
                {
                    position = new Vector3(pointB.x - offsetX, pointB.y + offsetY, Vertex.nearZ),
                    tint = color
                });
                vertices.Add(new Vertex()
                {
                    position = new Vector3(pointB.x - offsetX, pointB.y + offsetY, Vertex.nearZ),
                    tint = color
                });
                vertices.Add(new Vertex()
                {
                    position = new Vector3(pointA.x - offsetX, pointA.y + offsetY, Vertex.nearZ),
                    tint = color
                });
                vertices.Add(new Vertex()
                {
                    position = new Vector3(pointA.x + offsetX, pointA.y - offsetY, Vertex.nearZ),
                    tint = color
                });

                indices.AddIndicesForSegment(i);
            }

            var mesh = context.Allocate(vertices.Count, indices.Count);
            mesh.SetAllVertices(vertices.ToArray());
            mesh.SetAllIndices(indices.ToArray());
        }
        public class DashedLineRequest
        {
            public Vector3 start;
            public Vector3 end;
            public MeshGenerationContext context;
            public float thickness = 3f;
            public Color color = EditorColors.Green_Focus;
            public float segmentLength = 10f;
            public float spaceLength = 5f;
            public DashedLineRequest(Vector3 start, Vector3 end, MeshGenerationContext context)
            {
                this.start = start;
                this.end = end;
                this.context = context;
            }


        }
        public static void DrawCurvedDashedLine(DashedLineRequest request)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<ushort> indices = new List<ushort>();
            int segmentCount = (int)((request.end - request.start).magnitude / (request.segmentLength + request.spaceLength));
            Vector3 currentPos = request.start;
            Vector3 dir = (request.end - request.start).normalized;
            for (int i = 0; i < segmentCount; i++)
            {
                Vector3 segmentEnd = currentPos + (dir * request.segmentLength);
                DrawSegment(request.color, request.thickness, vertices, currentPos,segmentEnd);
                indices.AddIndicesForSegment(i);

                currentPos = segmentEnd + (dir * request.spaceLength);

            }
            DrawSegment(request.color, request.thickness, vertices, currentPos, request.end);
            indices.AddIndicesForSegment(segmentCount);
            var mesh = request.context.Allocate(vertices.Count, indices.Count);
            mesh.SetAllVertices(vertices.ToArray());
            mesh.SetAllIndices(indices.ToArray());
        }

        private static void DrawSegment(Color lineColor, float lineThickness, List<Vertex> vertices, Vector3 pointA, Vector3 pointB)
        {
            float angle =  Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x);
            float offsetX = lineThickness / 2 * Mathf.Sin(angle);
            float offsetY = lineThickness / 2 * Mathf.Cos(angle);

            vertices.Add(new Vertex()
            {
                position = new Vector3(pointA.x + offsetX, pointA.y - offsetY, Vertex.nearZ),
                tint = lineColor
            });
            vertices.Add(new Vertex()
            {
                position = new Vector3(pointB.x + offsetX, pointB.y - offsetY, Vertex.nearZ),
                tint = lineColor
            });
            vertices.Add(new Vertex()
            {
                position = new Vector3(pointB.x - offsetX, pointB.y + offsetY, Vertex.nearZ),
                tint = lineColor
            });
            vertices.Add(new Vertex()
            {
                position = new Vector3(pointB.x - offsetX, pointB.y + offsetY, Vertex.nearZ),
                tint = lineColor
            });
            vertices.Add(new Vertex()
            {
                position = new Vector3(pointA.x - offsetX, pointA.y + offsetY, Vertex.nearZ),
                tint = lineColor
            });
            vertices.Add(new Vertex()
            {
                position = new Vector3(pointA.x + offsetX, pointA.y - offsetY, Vertex.nearZ),
                tint = lineColor
            });

        }
        private static void AddIndicesForSegment(this List<ushort> indices, int i)
        {
            ushort indexOffset(int value) => (ushort)(value + (i * 6));
            indices.Add(indexOffset(0));
            indices.Add(indexOffset(1));
            indices.Add(indexOffset(2));
            indices.Add(indexOffset(3));
            indices.Add(indexOffset(4));
            indices.Add(indexOffset(5));
        }
    }
}