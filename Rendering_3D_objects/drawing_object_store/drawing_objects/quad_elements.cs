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
    public class quad_elements
    {
        // class to store all the quadrilaterals
        public Dictionary<int,triangle_store> all_tri_l { get; private set; }
        public Dictionary<int,triangle_store> all_tri_h { get; private set; }
        public line_elements all_bndry { get; private set; }

        // Dictionary to keep track of user ID and internal ID (User ID = KEY, Internal ID = 0,1,2,..,n sequential)
        public Dictionary<int, int> quad_ids { get; private set; }

        private uint[] _tri_indices = new uint[0];

        private float[] _point_vertices = new float[0];

        private uint[] _point_indices = new uint[0];

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
            all_bndry = new line_elements(true);
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
            triangle_store temp_tri_l = new triangle_store(pt_ids[0], pt_ids[1], pt_ids[3], nodes.all_nodes[pt_ids[0]]
                , nodes.all_nodes[pt_ids[1]]
                , nodes.all_nodes[pt_ids[3]]);

            // Create a temporary triangle upper
            triangle_store temp_tri_h = new triangle_store(pt_ids[2], pt_ids[3], pt_ids[1], nodes.all_nodes[pt_ids[2]]
                , nodes.all_nodes[pt_ids[3]]
                , nodes.all_nodes[pt_ids[1]]);

            // Check whether the point already exists
            if (all_tri_l.Values.Contains(temp_tri_l) == false && all_tri_l.Values.Contains(temp_tri_h) == false &&
                all_tri_h.Values.Contains(temp_tri_l) == false && all_tri_h.Values.Contains(temp_tri_h) == false)
            {
                // Add the Line IDs to the list as Key (also flipped)
                quad_ids.Add(tri_id, seq_id);

                // Add the four boundary to the list
                all_bndry.add_line(all_bndry.all_lines.Count, pt_ids[0], pt_ids[1], nodes);
                all_bndry.add_line(all_bndry.all_lines.Count, pt_ids[1], pt_ids[2], nodes);
                all_bndry.add_line(all_bndry.all_lines.Count, pt_ids[2], pt_ids[3], nodes);
                all_bndry.add_line(all_bndry.all_lines.Count, pt_ids[3], pt_ids[0], nodes);

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

            // Compute a normal vector to the plane of the points
            Vector3 normal = Vector3.Cross(nodes.all_nodes[pt1_id].get_point_as_vector() - nodes.all_nodes[pt0_id].get_point_as_vector(),
                                            nodes.all_nodes[pt2_id].get_point_as_vector() - nodes.all_nodes[pt0_id].get_point_as_vector());

            // Sort the points lexicographically clockwise
            pointIds.Sort((a, b) => {
                Vector3 vecA = nodes.all_nodes[a].get_point_as_vector() - centroid;
                Vector3 vecB = nodes.all_nodes[b].get_point_as_vector() - centroid;

                // Determine the direction of the angle
                Vector3 cross = Vector3.Cross(vecA, vecB);
                float dot = Vector3.Dot(cross, normal);

                return Math.Sign(dot);
            });

            return pointIds;
        }

        public void set_openTK_objects()
        {

            // Set the quadrialateral indices
            int j = 0;
            this._point_vertices = new float[10 * 6 * all_tri_l.Count];
            this._point_indices = new uint[all_tri_l.Count * 6];
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
                // Add the First point indices
                this._point_indices[j] = (uint)j;
                // Second index (Second point)
                this._tri_indices[j] = (uint)j;
                j++;

                // Third point
                float[] temp_vertices_2 = tris.Value.pt2.get_point_vertices();

                add_point_vertices(j, temp_vertices_2, tris.Value.tri_normal);
                // Add the First point indices
                this._point_indices[j] = (uint)j;
                // Third index (Third point)
                this._tri_indices[j] = (uint)j;
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
                // First point
                float[] temp_vertices_0 = tris.Value.pt0.get_point_vertices();

                add_point_vertices(j, temp_vertices_0, tris.Value.tri_normal);
                // Add the First point indices
                this._point_indices[j] = (uint)j;
                // First index (First point)
                this._tri_indices[j] = (uint)j;
                j++;


                // Second point
                float[] temp_vertices_1 = tris.Value.pt1.get_point_vertices();

                add_point_vertices(j, temp_vertices_1, tris.Value.tri_normal);
                // Add the First point indices
                this._point_indices[j] = (uint)j;
                // Second index (Second point)
                this._tri_indices[j] = (uint)j;
                j++;

                // Third point
                float[] temp_vertices_2 = tris.Value.pt2.get_point_vertices();

                add_point_vertices(j, temp_vertices_2, tris.Value.tri_normal);
                // Add the First point indices
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


        public void add_point_vertices(int i, float[] temp_vertices, Vector3 tri_normal)
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


        public void paint_all_quadrilaterals_boundary()
        {
            // Paint the Quadrilaterals boundaries
            this.all_bndry.paint_all_lines();
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
