using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

// OpenTK 
using OpenTK;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects.object_store
{
    public class point_store
    {
        // Size of the int -2,147,483,648 to 2,147,483,647
        private int _x;
        private int _y;
        private int _z;

        private Color _pt_clr = Color.Red;
        private Color _highlight_pt_clr = Color.Cyan;

        private double pt_paint_x;
        private double pt_paint_y;
        private double pt_paint_z;

        public double d_x { get; private set; }

        public double d_y { get; private set; }

        public double d_z { get; private set; }

        private float[] get_vertex_coords()
        {
            float[] vertex_coord = new float[3];
            // Add vertex to list
            vertex_coord[0] = (float)pt_paint_x;
            vertex_coord[1] = (float)pt_paint_y;
            vertex_coord[2] = (float)pt_paint_z;

            return vertex_coord;
        }

        private float[] get_vertex_color()
        {
            float[] vertex_clr = new float[4];

            // Add vertex color to the list
            vertex_clr[0] = ((float)this._pt_clr.R / 255.0f);
            vertex_clr[1] = ((float)this._pt_clr.G / 255.0f);
            vertex_clr[2] = ((float)this._pt_clr.B / 255.0f);
            vertex_clr[3] = ((float)this._pt_clr.A / 255.0f);

            return vertex_clr;
        }

        private float[] get_highlight_vertex_color()
        {
            float[] vertex_clr = new float[4];

            // Add vertex color to the list
            vertex_clr[0] = ((float)this._highlight_pt_clr.R / 255.0f);
            vertex_clr[1] = ((float)this._highlight_pt_clr.G / 255.0f);
            vertex_clr[2] = ((float)this._highlight_pt_clr.B / 255.0f);
            vertex_clr[3] = ((float)this._highlight_pt_clr.A / 255.0f);

            return vertex_clr;
        }

        public void update_scale(double d_scale, double tran_tx, double tran_ty, double tran_tz)
        {
            this.pt_paint_x = (d_x - tran_tx) * d_scale;
            this.pt_paint_y = (d_y - tran_ty) * d_scale;
            this.pt_paint_z = (d_z - tran_tz) * d_scale;
        }

        public float[] get_point_vertices()
        {
            // Return the point in openGL format
            return get_vertex_coords().Concat(get_vertex_color()).ToArray(); ;
        }

        public float[] get_highlight_point_vertices()
        {
            // Return the point in openGL format
            return get_vertex_coords().Concat(get_highlight_vertex_color()).ToArray(); ;
        }

        public Vector3 get_point_as_vector()
        {
            // return points as OpenTK.Struct.Vector3
            return new Vector3((float)this.d_x,
                               (float)this.d_y,
                               (float)this.d_z);
        }

        public point_store(double t_x, double t_y, double t_z, Color clr)
        {
            // Main constructor
            this.d_x = t_x;
            this.d_y = t_y;
            this.d_z = t_z;

            // Add the input to integer to avoid floating point impressision issues
            // Easier to compare the inputs
            this._x = (int)(t_x * 100000);
            this._y = (int)(t_y * 100000);
            this._z = (int)(t_z * 100000);

            this._pt_clr = clr;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as point_store);
        }

        public bool Equals(point_store other_pt)
        {
            if (this.Equals(other_pt._x, other_pt._y, other_pt._z) == true)
            {
                return true;
            }
            return false;
        }

        public bool Equals(int other_pt_x, int other_pt_y, int other_pt_z)
        {
            // Check whether the point-coordinates are the same
            if (this._x == other_pt_x && this._y == other_pt_y && this._z == other_pt_z)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this._x, this._y, this._z);
        }
    }
}
