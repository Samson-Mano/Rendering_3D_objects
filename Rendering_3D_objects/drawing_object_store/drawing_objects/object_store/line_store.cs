using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects.object_store
{
    public class line_store
    {
        public int ln_id { get; private set; }

        public int start_pt_id { get; private set; }

        public int end_pt_id { get; private set; }

        public line_store(int t_ln_id, int t_start_pt_id, int t_end_pt_id)
        {
            // Main constructor
            this.ln_id = t_ln_id;
            this.start_pt_id = t_start_pt_id;
            this.end_pt_id = t_end_pt_id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as line_store);
        }

        public bool Equals(line_store other_line)
        {
            // Check 1 (Line ids should not match)
            if (this.Equals(other_line.ln_id) == true)
            {
                return true;
            }

            // Check 2 (Whether line end points match)
            if ((this.start_pt_id.Equals(other_line.start_pt_id) && this.end_pt_id.Equals(other_line.end_pt_id)) ||
                (this.start_pt_id.Equals(other_line.end_pt_id) && this.end_pt_id.Equals(other_line.start_pt_id)))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.start_pt_id, this.end_pt_id);
        }
    }
}
