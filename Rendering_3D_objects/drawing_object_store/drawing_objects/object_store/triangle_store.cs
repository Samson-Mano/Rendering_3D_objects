using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// OpenTK library
using OpenTK;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects.object_store
{
    public class triangle_store
    {
        public int pt0_id { get; private set; }

        public int pt1_id { get; private set; }

        public int pt2_id { get; private set; }

        public point_store pt0 { get; private set; }

        public point_store pt1 { get; private set; }

        public point_store pt2 { get; private set; }

        public Vector3 tri_normal { get; private set; }

        public triangle_store( int t_pt0_id, int t_pt1_id, int t_pt2_id, point_store t_pt0, point_store t_pt1, point_store t_pt2)
        {
            // Main constructor
            // IDs
            this.pt0_id = t_pt0_id;
            this.pt1_id = t_pt1_id;
            this.pt2_id = t_pt2_id;
            // Points
            this.pt0 = t_pt0;
            this.pt1 = t_pt1;
            this.pt2 = t_pt2;

            tri_normal = get_normal(t_pt0, t_pt1, t_pt2);
        }

        private Vector3 get_normal(point_store p0, point_store p1, point_store p2)
        {
            // Get the normal of vector
            Vector3 edge1 = p1.get_point_as_vector() - p0.get_point_as_vector();
            Vector3 edge2 = p2.get_point_as_vector() - p0.get_point_as_vector();

            // Compute the normal vector as the cross product of the two vectors
            return Vector3.Cross(edge1, edge2).Normalized();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as triangle_store);
        }

        public bool Equals(triangle_store other_tri)
        {
            // Check (Whether line end points match)
            if (is_point_attached(other_tri.pt0_id) == true &&
                is_point_attached(other_tri.pt1_id) == true &&
                is_point_attached(other_tri.pt2_id) == true)
            {
                return true;
            }
            return false;
        }

        private bool is_point_attached(int pt_id)
        {
            // Return whether this input point Id equals to any of this triangle point ids
            if (this.pt0_id.Equals(pt_id) ||
             this.pt1_id.Equals(pt_id) ||
             this.pt2_id.Equals(pt_id))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine( this.pt0_id, this.pt1_id, this.pt2_id);
        }
    }
}
