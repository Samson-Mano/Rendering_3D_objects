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
    public class line_elements
    {
        // class to store all the lines
        public Dictionary<int, line_store> all_lines { get; private set; }

        public bool is_bndry { get; private set; }

        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> line_ids { get; private set; }

        private uint[] _line_indices = new uint[0];

        private float[] _point_vertices = new float[0];

        private uint[] _point_indices = new uint[0];

        // OpenTK variables Line Elements
        private VertexBuffer linepts_VertexBufferObject;
        private List<VertexBufferLayout> line_BufferLayout;
        private VertexArray line_VertexArrayObject;
        private IndexBuffer line_ElementBufferObject;

        public line_elements(bool is_bndry)
        {
            // Empty constructor
            // Initialize all lines
            all_lines = new Dictionary<int, line_store>();
            line_ids = new Dictionary<int, int>();
            this.is_bndry = is_bndry;
        }

        public void add_line(int ln_id, int s_id, int e_id, nodes_store nodes)
        {
            // Create a line ID as sequence (0,1,2...n)
            int seq_id = line_ids.Count == 0 ? 0 : (line_ids.Values.LastOrDefault() + 1);
            // Create a temporary line
            line_store temp_ln = new line_store(s_id, e_id, nodes.all_nodes[s_id], nodes.all_nodes[e_id]);

            // Check whether the point already exists
            if (all_lines.Values.Contains(temp_ln) == false)
            {
                // Add the Line IDs to the list as Key (also flipped)
                line_ids.Add(ln_id, seq_id);

                // Add new line
                all_lines.Add(ln_id, temp_ln);
            }
        }

        public void set_openTK_objects()
        {
            // Set the openTK objects for the points
            // Set the vertices
            int j = 0;
            this._point_vertices = new float[7 * 2 * all_lines.Count];
            this._point_indices = new uint[all_lines.Count * 2];
            this._line_indices = new uint[all_lines.Count * 2];

            foreach (var ln in all_lines)
            {
                // First point
                float[] temp_vertices_0 = ln.Value.start_pt.get_point_vertices();

                add_point_vertices(j, temp_vertices_0);
                // Add the First point indices
                this._point_indices[j] = (uint)j;
                // First index (First point)
                this._line_indices[j] = (uint)j;
                j++;

                // Second point
                float[] temp_vertices_1 = ln.Value.end_pt.get_point_vertices();

                add_point_vertices(j, temp_vertices_1);
                // Add the Second point indices
                this._point_indices[j] = (uint)j;
                // Second index (Second point)
                this._line_indices[j] = (uint)j;
                j++;
            }

            //1.  Set the vertex buffer for the line obhects
            this.linepts_VertexBufferObject = new VertexBuffer(this._point_vertices, this._point_vertices.Length * sizeof(float));

            //2. Create and add to the buffer layout
            line_BufferLayout = new List<VertexBufferLayout>();
            line_BufferLayout.Add(new VertexBufferLayout(3, 7)); // Vertex layout
            line_BufferLayout.Add(new VertexBufferLayout(4, 7)); // Color layout  

            //3. Setup the vertex Array (Add vertexBuffer binds both the vertexbuffer and vertexarray)
            line_VertexArrayObject = new VertexArray();
            line_VertexArrayObject.Add_vertexBuffer(this.linepts_VertexBufferObject, line_BufferLayout);

            // 4. Set up element buffer
            line_ElementBufferObject = new IndexBuffer(this._line_indices, this._line_indices.Length);
            line_ElementBufferObject.Bind();
        }

        public void add_point_vertices(int i, float[] temp_vertices)
        {
            // X, Y, Z Co-ordinate
            this._point_vertices[(i * 7) + 0] = temp_vertices[0];
            this._point_vertices[(i * 7) + 1] = temp_vertices[1];
            this._point_vertices[(i * 7) + 2] = temp_vertices[2];

            if (is_bndry == false)
            {
                // R, G, B, A values
                this._point_vertices[(i * 7) + 3] = temp_vertices[3];
                this._point_vertices[(i * 7) + 4] = temp_vertices[4];
                this._point_vertices[(i * 7) + 5] = temp_vertices[5];
                this._point_vertices[(i * 7) + 6] = temp_vertices[6];
            }
            else
            {
                // Make the line color as black
                this._point_vertices[(i * 7) + 3] = 0;
                this._point_vertices[(i * 7) + 4] = 0;
                this._point_vertices[(i * 7) + 5] = 0;
                this._point_vertices[(i * 7) + 6] = 1;
            }
        }

        public void paint_all_lines()
        {
            // Call set_openTK_objects()
            // Bind before painting
            line_VertexArrayObject.Add_vertexBuffer(this.linepts_VertexBufferObject, line_BufferLayout);
            line_ElementBufferObject.Bind();

            // Open GL paint Lines
            GL.DrawElements(PrimitiveType.Lines, this._line_indices.Length, DrawElementsType.UnsignedInt, 0);
        }

    }
}
