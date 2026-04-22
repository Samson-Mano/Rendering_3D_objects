using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
// This app class structure
using Rendering_3D_objects.global_variables;
using Rendering_3D_objects.drawing_object_store.drawing_objects;

namespace Rendering_3D_objects.drawing_object_store
{
    public class geometry_store
    {
        public mesh_store mesh_data { get; private set; }

        public bool is_geometry_set { get; private set; }

        public Vector3 geom_bounds_max { get; private set; }
        public Vector3 geom_bounds_min { get; private set; }

        public geometry_store()
        {
            // Empty constructor
            mesh_data = new mesh_store();

            this.is_geometry_set = false;
        }

        public geometry_store(List<point_store> points,
             List<line_store> elines,
             List<triangle_store> etris,
             List<quad_store> equads)
        {
            // Main constructor
            mesh_data = new mesh_store(points, elines, etris, equads);

            // Set the boundary size for the geometry
            set_geometry_bounds();

            this.is_geometry_set = true;
        }

        public void set_geometry_bounds()
        {
            // Set the X,Y,Z bounds of geometry
            double min_x = double.MaxValue, max_x = double.MinValue;
            double min_y = double.MaxValue, max_y = double.MinValue;
            double min_z = double.MaxValue, max_z = double.MinValue;     

            // Loop through all nodal co-ordinates to find the geometry bounds
            foreach(var pt in this.mesh_data.points)
            {
                // X Bound
                min_x = pt.x_coord < min_x ? pt.x_coord: min_x;
                max_x = pt.x_coord > max_x ? pt.x_coord: max_x;
                
                // Y Bound
                min_y = pt.y_coord < min_y ? pt.y_coord : min_y;
                max_y = pt.y_coord > max_y ? pt.y_coord : max_y;

                // Z Bound
                min_z = pt.z_coord < min_z ? pt.z_coord : min_z;
                max_z = pt.z_coord > max_z ? pt.z_coord : max_z;
            }

            geom_bounds_max = new Vector3((float)max_x , (float)max_y, (float)max_z);
            geom_bounds_min = new Vector3((float)min_x, (float) min_y, (float)min_z);
        }



        public void set_openTK_objects()
        {
            if (is_geometry_set == false)
                return;

            // Set the openTK objects
            mesh_data.set_openTK_objects();

            //this.nodes.set_openTK_objects();
            //this.elines.set_openTK_objects();
            //this.etris.set_openTK_objects();
            //this.equads.set_openTK_objects();
        }

        public void paint_line_objects()
        {
            // Paint the lines objects
            if (is_geometry_set == false)
                return;

            GL.LineWidth(5.0f);
            this.elines.paint_all_lines();
            this.nodes.paint_all_points();

            // Paint the boundary of the mesh
            if(gvariables_static.is_paint_wiremesh == true)
            {
                GL.LineWidth(0.1f);
                this.equads.paint_all_quadrilaterals_boundary();
                this.etris.paint_all_triangles_boundary();
            }
        }


        public void paint_surf_objects()
        {
            if (is_geometry_set == false || gvariables_static.is_paint_surf == false)
                return;

            // Paint all the objects
            this.equads.paint_all_quadrilaterals();
            this.etris.paint_all_triangles();

        }



    }
}
