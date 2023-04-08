using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
// This app class structure
using Rendering_3D_objects.open_tk_control.open_tk_buffer;
using Rendering_3D_objects.drawing_object_store.drawing_objects.object_store;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects
{
    public class tri_elements
    {
        // class to store all the triangles
        public Dictionary<int, triangle_store> all_tri { get; private set; }
        public line_elements all_bndry { get; private set; }

        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> tri_ids { get; private set; }

        private uint[] _tri_indices = new uint[0];

        private float[] _point_vertices = new float[0];

        private uint[] _point_indices = new uint[0];

        // OpenTK variables
        private VertexBuffer tripts_VertexBufferObject;
        private List<VertexBufferLayout> tri_BufferLayout;
        private VertexArray tri_VertexArrayObject;
        private IndexBuffer tri_ElementBufferObject;

        public tri_elements()
        {
            // Empty constructor
            // Initialize all points
            all_tri = new Dictionary<int, triangle_store>();
            all_bndry = new line_elements(true);
            tri_ids = new Dictionary<int, int>();

        }

        public void add_triangle(int tri_id, int pt_id_0, int pt_id_1, int pt_id_2, nodes_store nodes)
        {
            /*
            1
            | \         
            |   \       
            | t   \   
            |       \   
            0__________2
            */
            // 0, 1, 2

            // Create a line ID as sequence (0,1,2...n)
            int seq_id = tri_ids.Count == 0 ? 0 : (tri_ids.Values.LastOrDefault() + 1);

            // Create a temporary triangle
            triangle_store temp_tri = new triangle_store(pt_id_0, pt_id_1, pt_id_2, nodes.all_nodes[pt_id_0]
                , nodes.all_nodes[pt_id_1]
                , nodes.all_nodes[pt_id_2]);

            // Check whether the point already exists
            if (all_tri.Values.Contains(temp_tri) == false)
            {
                // Add the tri IDs to the list as Key
                tri_ids.Add(tri_id, seq_id);

                // Add the boundary to the list
                all_bndry.add_line(all_bndry.all_lines.Count, pt_id_0, pt_id_1, nodes);
                all_bndry.add_line(all_bndry.all_lines.Count, pt_id_1, pt_id_2, nodes);
                all_bndry.add_line(all_bndry.all_lines.Count, pt_id_2, pt_id_0, nodes);

                // Add new triangle
                all_tri.Add(tri_id, temp_tri);
            }

        }

        public void set_openTK_objects()
        {

            // Set the Triangle indices
            int j = 0;
            this._point_vertices = new float[10 * 3 * all_tri.Count];
            this._point_indices = new uint[all_tri.Count * 3];
            this._tri_indices = new uint[all_tri.Count * 3];

            foreach (var tris in all_tri)
            {
                /*
                1
                | \         
                |   \       
                | t   \   
                |       \   
                0__________2
                */
                // 0, 1, 2
                // First point
                float[] temp_vertices_0 = tris.Value.pt0.get_point_vertices();

                add_point_vertices(j, temp_vertices_0,tris.Value.tri_normal);
                // Add the First point indices
                this._point_indices[j] = (uint)j;
                // First index (First point)
                this._tri_indices[j] = (uint)j;
                j++;

                // Second point
                float[] temp_vertices_1 = tris.Value.pt1.get_point_vertices();

                add_point_vertices(j, temp_vertices_1, tris.Value.tri_normal);
                // Add the Second point indices
                this._point_indices[j] = (uint)j;
                // Second index (Second point)
                this._tri_indices[j] = (uint)j;
                j++;

                // Third point
                float[] temp_vertices_2 = tris.Value.pt2.get_point_vertices();

                add_point_vertices(j, temp_vertices_2, tris.Value.tri_normal);
                // Add the Second point indices
                this._point_indices[j] = (uint)j;
                // Third index (Third point)
                this._tri_indices[j] = (uint)j;
                j++;
            }

            //1.  Get the vertex buffer
            this.tripts_VertexBufferObject = new VertexBuffer(this._point_vertices, this._point_vertices.Length * sizeof(float));

            //2. Create and add to the buffer layout
            tri_BufferLayout = new List<VertexBufferLayout>();
            tri_BufferLayout.Add(new VertexBufferLayout(3, 10)); // Vertex layout
            tri_BufferLayout.Add(new VertexBufferLayout(4, 10)); // Color layout  
            tri_BufferLayout.Add(new VertexBufferLayout(3, 10)); // Normal layout

            //3. Setup the vertex Array (Add vertexBuffer binds both the vertexbuffer and vertexarray)
            tri_VertexArrayObject = new VertexArray();
            tri_VertexArrayObject.Add_vertexBuffer(this.tripts_VertexBufferObject, tri_BufferLayout);

            // 4. Set up element buffer
            tri_ElementBufferObject = new IndexBuffer(this._tri_indices, this._tri_indices.Length);
            tri_ElementBufferObject.Bind();

            // Set the boundary openTK objects
            this.all_bndry.set_openTK_objects();
        }

        public void add_point_vertices(int i, float[] temp_vertices,Vector3 tri_normal)
        {
            // X, Y, Z Co-ordinate
            this._point_vertices[(i * 10) + 0] = temp_vertices[0];
            this._point_vertices[(i * 10) + 1] = temp_vertices[1];
            this._point_vertices[(i * 10) + 2] = temp_vertices[2];

            // R, G, B, A values
            this._point_vertices[(i * 10) + 3] = temp_vertices[3];
            this._point_vertices[(i * 10) + 4] = temp_vertices[4];
            this._point_vertices[(i * 10) + 5] = temp_vertices[5];
            this._point_vertices[(i * 10) + 6] = temp_vertices[6];

            // Add the triangle normal
            this._point_vertices[(i * 10) + 7] = tri_normal.X;
            this._point_vertices[(i * 10) + 8] = tri_normal.Y;
            this._point_vertices[(i * 10) + 9] = tri_normal.Z;
        }

        public void paint_all_triangles_boundary()
        {
            // Paint the triangle boundaries
            this.all_bndry.paint_all_lines();
        }


        public void paint_all_triangles()
        {
            // Call set_openTK_objects()
            // Bind before painting
            tri_VertexArrayObject.Add_vertexBuffer(this.tripts_VertexBufferObject, tri_BufferLayout);
            tri_ElementBufferObject.Bind();

            // Open GL paint quadrialateral
            GL.DrawElements(PrimitiveType.Triangles, this._tri_indices.Length, DrawElementsType.UnsignedInt, 0);
        }


    }
}
