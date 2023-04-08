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
using Rendering_3D_objects.drawing_object_store.drawing_objects.object_store;

namespace Rendering_3D_objects.drawing_object_store
{
    public class geometry_store
    {
        public nodes_store nodes { get; private set; }
        public line_elements elines { get; private set; }
        public tri_elements etris { get; private set; }
        public quad_elements equads { get; private set; }
        public bool is_geometry_set { get; private set; }

        public Vector3 geom_bounds_max { get; private set; }
        public Vector3 geom_bounds_min { get; private set; }

        public geometry_store()
        {
            // Empty constructor
            nodes = new nodes_store();
            elines = new line_elements(false);
            etris = new tri_elements();
            equads = new quad_elements();

            this.is_geometry_set = false;
        }

        public geometry_store(nodes_store nodes, line_elements elines, tri_elements etris, quad_elements equads)
        {
            // Main constructor
            this.nodes = nodes;
            this.elines = elines;
            this.etris = etris;
            this.equads = equads;

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
            foreach(var pt in this.nodes.all_nodes)
            {
                // X Bound
                min_x = pt.Value.d_x < min_x ? pt.Value.d_x: min_x;
                max_x = pt.Value.d_x > max_x ? pt.Value.d_x: max_x;

                // Y Bound
                min_y = pt.Value.d_y < min_y ? pt.Value.d_y : min_y;
                max_y = pt.Value.d_y > max_y ? pt.Value.d_y : max_y;

                // Z Bound
                min_z = pt.Value.d_z < min_z ? pt.Value.d_z : min_z;
                max_z = pt.Value.d_z > max_z ? pt.Value.d_z : max_z;
            }

            geom_bounds_max = new Vector3((float)max_x , (float)max_y, (float)max_z);
            geom_bounds_min = new Vector3((float)min_x, (float) min_y, (float)min_z);
        }

        public void set_openTK_objects()
        {
            if (is_geometry_set == false)
                return;

            // Set the openTK objects
            this.nodes.set_openTK_objects();
            this.elines.set_openTK_objects();
            this.etris.set_openTK_objects();
            this.equads.set_openTK_objects();
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
