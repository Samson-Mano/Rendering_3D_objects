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
    public class quad_elements
    {
        // class to store all the quadrilaterals
        public Dictionary<int,triangle_store> all_tri_l { get; private set; }
        public Dictionary<int,triangle_store> all_tri_h { get; private set; }

        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> quad_ids { get; private set; }

        private uint[] _tri_indices = new uint[0];

        // OpenTK variables
        private VertexBuffer tripts_VertexBufferObject;
        private List<VertexBufferLayout> tri_BufferLayout;
        private VertexArray tri_VertexArrayObject;
        private IndexBuffer tri_ElementBufferObject;

        public quad_elements()
        {
            // Empty constructor
            // Initialize all points
            all_tri_l = new Dictionary<int, triangle_store>();
            all_tri_h = new Dictionary<int, triangle_store>();
            quad_ids = new Dictionary<int, int>();

        }

        public void add_quadrilateral(int tri_id, int pt_id_0, int pt_id_1, int pt_id_2, int pt_id_3, nodes_store nodes)
        {
            /*
            1__________2
            | \        | 
            |   \   th | 
            | tl  \    |
            |       \  | 
            0__________3
            */
            // 0, 1, 3
            // 2, 3, 1

            // Sort the 3D points Lexicographically
            List<int> pt_ids = sort_pts_lexicographically(pt_id_0, pt_id_1, pt_id_2, pt_id_3, nodes);

            // Create a line ID as sequence (0,1,2...n)
            int seq_id = quad_ids.Count == 0 ? 0 : (quad_ids.Values.LastOrDefault() + 1);

            // Create a temporary triangle lower
            triangle_store temp_tri_l = new triangle_store(pt_ids[0], pt_ids[1], pt_ids[3]);
            // Create a temporary triangle upper
            triangle_store temp_tri_h = new triangle_store(pt_ids[2], pt_ids[3], pt_ids[1]);

            // Check whether the point already exists
            if (all_tri_l.Values.Contains(temp_tri_l) == false && all_tri_l.Values.Contains(temp_tri_h) == false &&
                all_tri_h.Values.Contains(temp_tri_l) == false && all_tri_h.Values.Contains(temp_tri_h) == false)
            {
                // Add the Line IDs to the list as Key (also flipped)
                quad_ids.Add(tri_id, seq_id);

                // Add new Quad (as two triangle)
                all_tri_l.Add(tri_id, temp_tri_l);
                all_tri_h.Add(tri_id, temp_tri_h);
            }
        }


        public List<int> sort_pts_lexicographically(int pt0_id,int pt1_id, int pt2_id, int pt3_id, nodes_store nodes)
        {
            // Create a list of the point IDs
            List<int> pointIds = new List<int>() { pt0_id, pt1_id, pt2_id, pt3_id };

            // Compute the centroid of the four points
            Vector3 centroid = Vector3.Zero;
            foreach (int pointId in pointIds)
            {
                centroid += nodes.all_nodes[pointId].get_point_as_vector();
            }
            centroid /= pointIds.Count;

            // Sort the points lexicographically clockwise
            pointIds.Sort((a, b) => {
                Vector3 vecA = nodes.all_nodes[a].get_point_as_vector() - centroid;
                Vector3 vecB = nodes.all_nodes[b].get_point_as_vector() - centroid;
                float angleA = (float)Math.Atan2(vecA.Z, vecA.X);
                float angleB = (float)Math.Atan2(vecB.Z, vecB.X);
                return angleA.CompareTo(angleB);
            });

            return pointIds;
        }

        public void set_openTK_objects(nodes_store nodes)
        {

            // Set the quadrialateral indices
            int j = 0;
            this._tri_indices = new uint[all_tri_l.Count * 6];

            foreach (var tris in all_tri_l)
            {
                /*
                1           
                | \          
                |   \        
                | tl  \     
                |       \    
                0__________3
                */
                // 0, 1, 3
                // First index (First point)
                this._tri_indices[j] = (uint)nodes.node_ids[tris.Value.pt0_id];
                j++;

                // Second index (Second point)
                this._tri_indices[j] = (uint)nodes.node_ids[tris.Value.pt1_id];
                j++;

                // Third index (Third point)
                this._tri_indices[j] = (uint)nodes.node_ids[tris.Value.pt2_id];
                j++;
            }

            foreach (var tris in all_tri_h)
            {
                /*
                1__________2
                  \        | 
                    \   th | 
                      \    |
                        \  | 
                           3
                */
                // 2, 3, 1
                // First index (First point)
                this._tri_indices[j] = (uint)nodes.node_ids[tris.Value.pt0_id];
                j++;

                // Second index (Second point)
                this._tri_indices[j] = (uint)nodes.node_ids[tris.Value.pt1_id];
                j++;

                // Third index (Third point)
                this._tri_indices[j] = (uint)nodes.node_ids[tris.Value.pt2_id];
                j++;
            }



            //1.  Get the vertex buffer
            this.tripts_VertexBufferObject = nodes.point_VertexBufferObject;

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

        public void paint_all_quadrilaterals()
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
