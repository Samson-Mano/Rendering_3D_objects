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
    public class line_elements
    {
        // class to store all the lines
        public Dictionary<int, line_store> all_lines { get; private set; }

        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> line_ids { get; private set; }

        private uint[] _line_indices = new uint[0];

        // OpenTK variables
        private VertexBuffer linepts_VertexBufferObject;
        private List<VertexBufferLayout> line_BufferLayout;
        private VertexArray line_VertexArrayObject;
        private IndexBuffer line_ElementBufferObject;

        public line_elements()
        {
            // Empty constructor
            // Initialize all lines
            all_lines = new Dictionary<int, line_store>();
            line_ids = new Dictionary<int, int>();
        }

        public void add_line(int ln_id, int s_id, int e_id)
        {
            // Create a line ID as sequence (0,1,2...n)
            int seq_id = line_ids.Count == 0 ? 0 : (line_ids.Values.LastOrDefault() + 1);

            // Create a temporary line
            line_store temp_ln = new line_store(s_id, e_id);

            // Check whether the point already exists
            if (all_lines.Values.Contains(temp_ln) == false)
            {
                // Add the Line IDs to the list as Key (also flipped)
                line_ids.Add(ln_id, seq_id);

                // Add new line
                all_lines.Add(ln_id, temp_ln);
            }
        }

        public void set_highlight_openTK_objects(nodes_store nodes)
        {

            // Set the line indices
            int j = 0;
            this._line_indices = new uint[all_lines.Count * 2];

            foreach (var ln in all_lines)
            {
                // First index (First point)
                this._line_indices[j] = (uint)nodes.node_ids[ln.Value.start_pt_id];
                j++;

                // Second index (Second point)
                this._line_indices[j] = (uint)nodes.node_ids[ln.Value.end_pt_id];
                j++;
            }

            //1.  Get the vertex buffer
            this.linepts_VertexBufferObject = nodes.point_VertexBufferObject;

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
