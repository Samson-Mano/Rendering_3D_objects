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
    public class nodes_store
    {
        // class to store all the nodes
        public Dictionary<int,point_store> all_nodes { get; private set; }

        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> node_ids { get; private set; }

        private float[] _point_vertices = new float[0];

        private uint[] _point_indices = new uint[0];

        // OpenTK variables
        public VertexBuffer point_VertexBufferObject { get; private set; }
        public List<VertexBufferLayout> point_BufferLayout { get; private set; }
        private VertexArray point_VertexArrayObject;
        private IndexBuffer point_ElementBufferObject;

        public nodes_store()
        {
            // Empty constructor
            // Initialize all points
            all_nodes = new Dictionary<int,point_store>();
            node_ids = new Dictionary<int, int>();
        }

        public void add_point(int pt_id, double t_x, double t_y, double t_z, Color clr)
        {
                // Create a point ID as sequence (0,1,2...n)
                int seq_id = node_ids.Count == 0 ? 0 : (node_ids.Values.LastOrDefault() + 1);

                // Create a temporary point
                point_store temp_pt = new point_store(t_x, t_y, t_z, clr);

                // Check whether the point already exists
                if (all_nodes.Values.Contains(temp_pt) == false)
                {
                    // Add the node IDs to the list as Key 
                    node_ids.Add(pt_id, seq_id);

                    // Add new point
                    all_nodes.Add(pt_id,temp_pt);
                }

        }

        public void set_openTK_objects()
        {
            // Set the openTK objects for the points
            // Set the vertices
            this._point_vertices = new float[7 * all_nodes.Count];
            this._point_indices = new uint[all_nodes.Count];

            foreach (var pts in all_nodes)
            {
                // add the point vertices
                float[] temp_vertices = pts.Value.get_point_vertices();

                int i = node_ids[pts.Key]; // Get the sequential ID to form the buffers
                // X, Y, Z Co-ordinate
                this._point_vertices[(i * 7) + 0] = temp_vertices[0];
                this._point_vertices[(i * 7) + 1] = temp_vertices[1];
                this._point_vertices[(i * 7) + 2] = temp_vertices[2];

                // R, G, B, A values
                this._point_vertices[(i * 7) + 3] = temp_vertices[3];
                this._point_vertices[(i * 7) + 4] = temp_vertices[4];
                this._point_vertices[(i * 7) + 5] = temp_vertices[5];
                this._point_vertices[(i * 7) + 6] = temp_vertices[6];

                // Add the point indices
                this._point_indices[i] = (uint)i;
                i++;
            }

            //1.  Set up vertex buffer
            point_VertexBufferObject = new VertexBuffer(this._point_vertices, this._point_vertices.Length * sizeof(float));

            //2. Create and add to the buffer layout
            point_BufferLayout = new List<VertexBufferLayout>();
            point_BufferLayout.Add(new VertexBufferLayout(3, 7)); // Vertex layout
            point_BufferLayout.Add(new VertexBufferLayout(4, 7)); // Color layout  

            //3. Setup the vertex Array (Add vertexBuffer binds both the vertexbuffer and vertexarray)
            point_VertexArrayObject = new VertexArray();
            point_VertexArrayObject.Add_vertexBuffer(point_VertexBufferObject, point_BufferLayout);

            // 4. Set up element buffer
            point_ElementBufferObject = new IndexBuffer(this._point_indices, this._point_indices.Length);
            point_ElementBufferObject.Bind();
        }


        public void paint_all_points()
        {
            // Call set_openTK_objects()
            // Bind before painting
            point_VertexArrayObject.Add_vertexBuffer(point_VertexBufferObject, point_BufferLayout);
            point_ElementBufferObject.Bind();

            // Open GL paint
            GL.PointSize(4.0f);
            GL.DrawElements(PrimitiveType.Points, this._point_indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
