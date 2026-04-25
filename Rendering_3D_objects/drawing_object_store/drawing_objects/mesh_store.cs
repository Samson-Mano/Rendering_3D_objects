
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Rendering_3D_objects.global_variables;
using Rendering_3D_objects.open_tk_control.open_tk_buffer;
// This app class structure
using Rendering_3D_objects.open_tk_control.open_tk_shader;
using System;
using System.Collections.Generic;
using System.Drawing;
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


        // Shader
        Shader mesh_shader;


        // OpenTK variables
        VertexBuffer point_VertexBufferObject;
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
        List<uint> pointIndexData = new List<uint>();
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

            // Create the mesh shader
            mesh_shader = new Shader(shader_store.mesh_vert_shader(),
                 shader_store.mesh_frag_shader());


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

            // Create the mesh shader
            mesh_shader = new Shader(shader_store.mesh_vert_shader(),
                 shader_store.mesh_frag_shader());

            // Create the wireframe from the mesh data
            create_wireframe();

            //_________________________________________________________________________
            // Create the pointIdToIndex mapping for OpenTK geometry data
            pointIdToIndex = new Dictionary<int, int>();
            pointIndexData = new List<uint>(points.Count);

            // Store normals for each point
            List<Vector3> vnormals = new List<Vector3>(points.Count);

            for (int i = 0; i < points.Count; i++)
            {
                pointIdToIndex[points[i].pt_id] = i;

                // Initialize normals to zero
                vnormals.Add(Vector3.Zero);

                pointIndexData.Add((uint)i);
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

        public void paint_mesh_surface()
        {
            mesh_shader.Use();

            // Paint the mesh using OpenTK (triangles and quads)
            if (gvariables_static.is_paint_meshsurface == true)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                mesh_shader.SetVector4("vertexColor", new Vector4(0.6f, 0.6f, 0.8f, 0.6f)); // Light blue color (Transparent)


                // Paint the triangle mesh
                triangle_ElementBufferObject.Bind();
                GL.DrawElements(PrimitiveType.Triangles, triangleIndexData.Count, DrawElementsType.UnsignedInt, 0);

                // Paint the quad mesh (as triangles)
                quad_ElementBufferObject.Bind();
                GL.DrawElements(PrimitiveType.Triangles, quadIndexData.Count, DrawElementsType.UnsignedInt, 0);
            }

        }

        public void paint_mesh_linespoints()
        {
            mesh_shader.Use();

            // Paint the wireframe
            if (gvariables_static.is_paint_wiremesh == true)
            {
                GL.LineWidth(1.0f);

                mesh_shader.SetVector4("vertexColor", new Vector4(0.0f, 0.0f, 0.0f, 1.0f)); // black

                // Paint the wireframe lines
                wireframe_ElementBufferObject.Bind();
                GL.DrawElements(PrimitiveType.Lines, wireframeIndexData.Count, DrawElementsType.UnsignedInt, 0);

            }

            // Paint the line mesh
            if (gvariables_static.is_paint_lines == true)
            {
                GL.LineWidth(2.5f);

                mesh_shader.SetVector4("vertexColor", new Vector4(1f, 0f, 0f, 1f)); // red

                line_ElementBufferObject.Bind();
                GL.DrawElements(PrimitiveType.Lines, lineIndexData.Count, DrawElementsType.UnsignedInt, 0);
            }


            // Paint the points
            if (gvariables_static.is_paint_points == true)
            {
                GL.PointSize(4.0f);

                mesh_shader.SetVector4("vertexColor", new Vector4(0f, 1f, 0f, 1f)); // green

                point_ElementBufferObject.Bind();
                GL.DrawElements(PrimitiveType.Points, pointIndexData.Count, DrawElementsType.UnsignedInt, 0);
            }
            //
        }


        public void set_openTK_objects()
        {

            // Create the point vertices
            float[] pointVertices = new float[6 * points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                int vi = i * 3;
                int vo = i * 6;

                // X, Y, Z Co-ordinate
                pointVertices[vo + 0] = vertexData[vi + 0];
                pointVertices[vo + 1] = vertexData[vi + 1];
                pointVertices[vo + 2] = vertexData[vi + 2];

                // Add the triangle normal
                pointVertices[vo + 3] = vertexnormalData[vi + 0];
                pointVertices[vo + 4] = vertexnormalData[vi + 1];
                pointVertices[vo + 5] = vertexnormalData[vi + 2];
                //
            }


            //1.  Create the vertex buffer (VBO) for the points
            this.point_VertexBufferObject = new VertexBuffer(pointVertices, pointVertices.Length * sizeof(float));


            //2. Create and add to the buffer layout
            List<VertexBufferLayout> point_BufferLayout = new List<VertexBufferLayout>();
            point_BufferLayout.Add(new VertexBufferLayout(3, 6)); // Vertex layout
            point_BufferLayout.Add(new VertexBufferLayout(3, 6)); // Normal layout

            //3. Create the vertex Array VAO (Add vertexBuffer binds both the vertexbuffer and vertexarray)
            point_VertexArrayObject = new VertexArray();
            point_VertexArrayObject.Add_vertexBuffer(this.point_VertexBufferObject, point_BufferLayout);

            // 4. Create the index buffer object (IBO) for the points 
            // 4A. Create the point index data 
            point_ElementBufferObject = new IndexBuffer(pointIndexData.ToArray(), pointIndexData.Count);

            // 4B. Create the line index data 
            line_ElementBufferObject = new IndexBuffer(lineIndexData.ToArray(), lineIndexData.Count);

            // 4C. Create the wireframe index data
            wireframe_ElementBufferObject = new IndexBuffer(wireframeIndexData.ToArray(), wireframeIndexData.Count);

            // 4D. Create the triangle index data
            triangle_ElementBufferObject = new IndexBuffer(triangleIndexData.ToArray(), triangleIndexData.Count);

            // 4E. Create the quad index data (2 Triangles per quad)
            quad_ElementBufferObject = new IndexBuffer(quadIndexData.ToArray(), quadIndexData.Count);

        }


        public void update_ShaderUniforms()
        {
            // Update the shader uniforms for the mesh shader
            //mesh_shader.SetMatrix4("modelMatrix", gvariables_static.modelMatrix);
            //mesh_shader.SetMatrix4("rotationMatrix", gvariables_static.rotationMatrix);
            //mesh_shader.SetMatrix4("panTranslation", gvariables_static.panTranslationMatrix);
            //mesh_shader.SetFloat("zoomscale", gvariables_static.zoomScale);

            Matrix4 scalingMatrix = Matrix4.CreateScale((float)gvariables_static.zoomScale, 
                (float)gvariables_static.zoomScale, 
                (float)gvariables_static.zoomScale);
            
            Matrix4 viewMatrix = Matrix4.Transpose(gvariables_static.panTranslationMatrix) * scalingMatrix;


            Matrix4 mvp =gvariables_static.ProjectionMatrix() *
               viewMatrix *
               gvariables_static.rotationMatrix * 
               gvariables_static.modelMatrix;

            Matrix4 normalMatrix = Matrix4.Transpose(Matrix4.Invert(gvariables_static.rotationMatrix * 
                gvariables_static.modelMatrix));

            // Set uniforms
            mesh_shader.SetMatrix4("uMVP", mvp);
            mesh_shader.SetMatrix4("uNormalMatrix", normalMatrix);

            // Fragement shader uniforms
            mesh_shader.SetVector4("uFrontColor", new Vector4(0.2f, 0.6f, 1.0f, 0.5f)); // outside
            mesh_shader.SetVector4("uBackColor", new Vector4(1.0f, 0.3f, 0.3f, 0.3f)); // inside

            // mesh_shader.SetVector3("uLightDir", new Vector3(0.3f, 0.5f, 1.0f).Normalized());
            // mesh_shader.SetFloat("uLightIntensity", 0.8f);

        }








    }
}
