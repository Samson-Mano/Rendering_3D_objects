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
using Rendering_3D_objects.opentk_control.opentk_buffer;
using Rendering_3D_objects.drawing_object_store.drawing_objects.object_store;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects
{
    public class tri_elements
    {
        // class to store all the triangles
        public List<triangle_store> all_tri { get; private set; }
        
        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> tri_ids { get; private set; }

        private uint[] _tri_indices = new uint[0];

        // OpenTK variables
        private VertexBuffer tripts_VertexBufferObject;
        private List<VertexBufferLayout> tri_BufferLayout;
        private VertexArray tri_VertexArrayObject;
        private IndexBuffer tri_ElementBufferObject;

        public tri_elements()
        {
            // Empty constructor
            // Initialize all points
            all_tri = new List<triangle_store>();
            tri_ids = new Dictionary<int, int>();

        }

        public void add_triangle(int tri_id, int pt_id_0, int pt_id_1, int pt_id_2)
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

            // Create a temporary point
            triangle_store temp_tri = new triangle_store(tri_id, pt_id_0, pt_id_1, pt_id_2);

            // Check whether the point already exists
            if (all_tri.Contains(temp_tri) == false)
            {
                // Add the Line IDs to the list as Key (also flipped)
                tri_ids.Add(tri_id, seq_id);

                // Add new line
                all_tri.Add(temp_tri);
            }

        }

        public void set_openTK_objects(nodes_store all_nodes)
        {

            // Set the quadrialateral indices
            int j = 0;
            this._tri_indices = new uint[all_tri.Count * 3];

            foreach (triangle_store tris in all_tri)
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
                // First index (First point)
                this._tri_indices[j] = (uint)all_nodes.node_ids[tris.pt0_id];
                j++;

                // Second index (Second point)
                this._tri_indices[j] = (uint)all_nodes.node_ids[tris.pt1_id];
                j++;

                // Third index (Third point)
                this._tri_indices[j] = (uint)all_nodes.node_ids[tris.pt2_id];
                j++;
            }

            //1.  Get the vertex buffer
            this.tripts_VertexBufferObject = all_nodes.point_VertexBufferObject;

            //2. Create and add to the buffer layout
            tri_BufferLayout = new List<VertexBufferLayout>();
            tri_BufferLayout.Add(new VertexBufferLayout(3, 7)); // Vertex layout
            tri_BufferLayout.Add(new VertexBufferLayout(4, 7)); // Color layout  

            //3. Setup the vertex Array (Add vertexBuffer binds both the vertexbuffer and vertexarray)
            tri_VertexArrayObject = new VertexArray();
            tri_VertexArrayObject.Add_vertexBuffer(this.tripts_VertexBufferObject, tri_BufferLayout);

            // 4. Set up element buffer
            tri_ElementBufferObject = new IndexBuffer(this._tri_indices, this._tri_indices.Length);
            tri_ElementBufferObject.Bind();
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
