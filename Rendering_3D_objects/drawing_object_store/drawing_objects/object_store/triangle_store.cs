using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects.object_store
{
    public class triangle_store
    {
        public int tri_id { get; private set; }

        public int pt0_id { get; private set; }

        public int pt1_id { get; private set; }

        public int pt2_id { get; private set; }

        public triangle_store(int t_tri_id, int t_pt0_id, int t_pt1_id, int t_pt2_id)
        {
            // Main constructor
            this.tri_id = t_tri_id;
            this.pt0_id = t_pt0_id;
            this.pt1_id = t_pt1_id;
            this.pt2_id = t_pt2_id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as triangle_store);
        }

        public bool Equals(triangle_store other_tri)
        {
            // Check 1 (Line ids should not match)
            if (this.tri_id ==other_tri.tri_id)
            {
                return true;
            }

            // Check 2 (Whether line end points match)
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
            return HashCode.Combine(this.tri_id, this.pt0_id, this.pt1_id, this.pt2_id);
        }


    }
}
