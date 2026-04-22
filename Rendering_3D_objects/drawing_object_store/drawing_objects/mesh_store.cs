using OpenTK;
using Rendering_3D_objects.open_tk_control.open_tk_buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects
{
    public class point_store
    {
        public int pt_id { get; set; }
        public float x_coord { get; set; }
        public float y_coord { get; set; }
        public float z_coord { get; set; }
    }

    public class line_store
    {
        public int line_id { get; set; }
        public int startpt_id { get; set; }
        public int endpt_id { get; set; }
    }

    public class triangle_store
    {
        public int triangle_id { get; set; }
        public int pt1_id { get; set; }
        public int pt2_id { get; set; }
        public int pt3_id { get; set; }
    }

    public class quad_store
    {
        public int quad_id { get; set; }
        public int pt1_id { get; set; }
        public int pt2_id { get; set; }
        public int pt3_id { get; set; }
        public int pt4_id { get; set; }
    }


    public class mesh_store
    {
        public List<point_store> points { get; private set; }
        public List<line_store> elines { get; private set; }

        public List<line_store> wireframe { get; private set; }

        public List<triangle_store> etris { get; private set; }
        public List<quad_store> equads { get; private set; }


        // OpenTK variables
        VertexBuffer point_VertexBufferObject;
        // List<VertexBufferLayout> point_BufferLayout;
        VertexArray point_VertexArrayObject;

        // Index  buffer for the points, lines and triangles
        IndexBuffer point_ElementBufferObject;
        IndexBuffer line_ElementBufferObject;
        IndexBuffer wireframe_ElementBufferObject;
        IndexBuffer triangle_ElementBufferObject;
        IndexBuffer quad_ElementBufferObject;


        // Geometry data for OpenTK
        Dictionary<int, int> pointIdToIndex = new Dictionary<int, int>();
        List<float> vertexData = new List<float>();
        List<float> vertexnormalData = new List<float>();
        List<uint> lineIndexData = new List<uint>();
        List<uint> wireframeIndexData = new List<uint>();
        List<uint> triangleIndexData = new List<uint>();
        List<uint> quadIndexData = new List<uint>();

        public mesh_store()
        {
            // Empty constructor
            points = new List<point_store>();
            elines = new List<line_store>();
            etris = new List<triangle_store>();
            equads = new List<quad_store>();
        }

        public mesh_store(List<point_store> points,
            List<line_store> elines,
            List<triangle_store> etris,
            List<quad_store> equads)
        {
            // Main constructor
            this.points = points;
            this.elines = elines;
            this.etris = etris;
            this.equads = equads;

            // Create the wireframe from the mesh data
            create_wireframe();

            //_________________________________________________________________________
            // Create the pointIdToIndex mapping for OpenTK geometry data
            pointIdToIndex = new Dictionary<int, int>();

            // Store normals for each point
            List<Vector3> vnormals = new List<Vector3>(points.Count);

            for (int i = 0; i < points.Count; i++)
            {
                pointIdToIndex[points[i].pt_id] = i;

                // Initialize normals to zero
                vnormals.Add(Vector3.Zero);
            }

            //_________________________________________________________________________
            // Prepare vertex data for OpenTK
            vertexData = new List<float>();

            foreach (var pt in points)
            {
                vertexData.Add(pt.x_coord);
                vertexData.Add(pt.y_coord);
                vertexData.Add(pt.z_coord);
            }

            //_________________________________________________________________________
            // Prepare line index data for OpenTK
            lineIndexData = new List<uint>();

            foreach (var line in elines)
            {
                if (pointIdToIndex.ContainsKey(line.startpt_id) && pointIdToIndex.ContainsKey(line.endpt_id))
                {
                    lineIndexData.Add((uint)pointIdToIndex[line.startpt_id]);
                    lineIndexData.Add((uint)pointIdToIndex[line.endpt_id]);
                }
            }

            //_________________________________________________________________________
            // Prepare wireframe index data for OpenTK
            wireframeIndexData = new List<uint>();

            foreach (var line in wireframe)
            {
                if (pointIdToIndex.ContainsKey(line.startpt_id) && pointIdToIndex.ContainsKey(line.endpt_id))
                {
                    wireframeIndexData.Add((uint)pointIdToIndex[line.startpt_id]);
                    wireframeIndexData.Add((uint)pointIdToIndex[line.endpt_id]);
                }
            }

            //_________________________________________________________________________
            // Prepare triangle index data for OpenTK
            triangleIndexData = new List<uint>();

            foreach (var tri in etris)
            {
                if (pointIdToIndex.ContainsKey(tri.pt1_id) && pointIdToIndex.ContainsKey(tri.pt2_id) && pointIdToIndex.ContainsKey(tri.pt3_id))
                {
                    triangleIndexData.Add((uint)pointIdToIndex[tri.pt1_id]);
                    triangleIndexData.Add((uint)pointIdToIndex[tri.pt2_id]);
                    triangleIndexData.Add((uint)pointIdToIndex[tri.pt3_id]);
                }
            }

            //_________________________________________________________________________
            // Prepare quad index data for OpenTK
            quadIndexData = new List<uint>();

            foreach (var quad in equads)
            {
                if (pointIdToIndex.ContainsKey(quad.pt1_id) && pointIdToIndex.ContainsKey(quad.pt2_id) && pointIdToIndex.ContainsKey(quad.pt3_id) && pointIdToIndex.ContainsKey(quad.pt4_id))
                {
                    // Make two triangles from the quad (pt1, pt2, pt3) and (pt1, pt3, pt4)
                    // Triangle 1
                    quadIndexData.Add((uint)pointIdToIndex[quad.pt1_id]);
                    quadIndexData.Add((uint)pointIdToIndex[quad.pt2_id]);
                    quadIndexData.Add((uint)pointIdToIndex[quad.pt3_id]);

                    // Triangle 2
                    quadIndexData.Add((uint)pointIdToIndex[quad.pt1_id]);
                    quadIndexData.Add((uint)pointIdToIndex[quad.pt3_id]);
                    quadIndexData.Add((uint)pointIdToIndex[quad.pt4_id]);
                }
            }


            //_________________________________________________________________________
            // Prepare vertex normal data for OpenTK
            Vector3 get_vertex(int index)
            {
                // Get the vertex position from vertexData using the index mapping (pointIdToIndex)
                int i3 = index * 3;
                return new Vector3(vertexData[i3], vertexData[i3 + 1], vertexData[i3 + 2]);
            }

            void accumulate_normals(List<uint> indices)
            {
                for (int i = 0; i < indices.Count; i += 3)
                {
                    int i0 = (int)indices[i];
                    int i1 = (int)indices[i + 1];
                    int i2 = (int)indices[i + 2];

                    var v0 = get_vertex(i0);
                    var v1 = get_vertex(i1);
                    var v2 = get_vertex(i2);

                    // Edge vectors
                    var e1 = v1 - v0;
                    var e2 = v2 - v0;

                    // Face normal (right-hand rule)
                    var faceNormal = Vector3.Cross(e1, e2);

                    // Accumulate (area-weighted automatically)
                    vnormals[i0] += faceNormal;
                    vnormals[i1] += faceNormal;
                    vnormals[i2] += faceNormal;
                }
            }

            // Accumulate normals from triangles and quads
            accumulate_normals(triangleIndexData);
            accumulate_normals(quadIndexData);

            vertexnormalData = new List<float>(points.Count * 3);

            for (int i = 0; i < vnormals.Count; i++)
            {
                Vector3 n = vnormals[i];

                if (n.LengthSquared > 1e-12f)
                    n = Vector3.Normalize(n);
                else
                    n = new Vector3(0, 0, 1); // fallback

                vertexnormalData.Add(n.X);
                vertexnormalData.Add(n.Y);
                vertexnormalData.Add(n.Z);
            }
            //
        }


        private void create_wireframe()
        {

            // Create the wireframe from the mesh data
            wireframe = new List<line_store>();

            // HashSet to track unique edges
            var edgeSet = new HashSet<(int, int)>();

            int wireframeLineId = 0;

            // Local function to add edge safely
            void AddEdge(int a, int b)
            {
                // Canonical ordering (undirected edge)
                var edge = a < b ? (a, b) : (b, a);

                if (edgeSet.Add(edge)) // Only adds if not duplicate
                {
                    wireframe.Add(new line_store
                    {
                        line_id = wireframeLineId++,
                        startpt_id = edge.Item1,
                        endpt_id = edge.Item2
                    });
                }
            }

            // Triangles
            foreach (var tri in etris)
            {
                AddEdge(tri.pt1_id, tri.pt2_id);
                AddEdge(tri.pt2_id, tri.pt3_id);
                AddEdge(tri.pt3_id, tri.pt1_id);
            }

            // Quads
            foreach (var quad in equads)
            {
                AddEdge(quad.pt1_id, quad.pt2_id);
                AddEdge(quad.pt2_id, quad.pt3_id);
                AddEdge(quad.pt3_id, quad.pt4_id);
                AddEdge(quad.pt4_id, quad.pt1_id);
            }
        }


        public void set_openTK_objects()
        {




        }


    }
}
