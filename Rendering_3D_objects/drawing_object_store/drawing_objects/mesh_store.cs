using Rendering_3D_objects.open_tk_control.open_tk_buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering_3D_objects.drawing_object_store.drawing_objects
{
    public class point_store
    {
        public int pt_id { get; set; }  
        public float x_coord { get; set; }
        public float y_coord { get; set; }

        public float z_coord { get; set; }
    }

    public class line_store
    {
        public int line_id { get; set; }
        public int startpt_id { get; set; }
        public int endpt_id { get; set; }
    }

    public class triangle_store
    {
        public int triangle_id { get; set; }
        public int pt1_id { get; set; }
        public int pt2_id { get; set; }
        public int pt3_id { get; set; }
    }

    public class quad_store
    {
        public int quad_id { get; set; }
        public int pt1_id { get; set; }
        public int pt2_id { get; set; }
        public int pt3_id { get; set; }
        public int pt4_id { get; set; }
    }


    public class mesh_store
    {
        public List<point_store> points { get; private set; }
        public List<line_store> elines { get; private set; }
        public List<triangle_store> etris { get; private set; }
        public List<quad_store> equads { get; private set; }


        // OpenTK variables
        VertexBuffer point_VertexBufferObject;
        List<VertexBufferLayout> point_BufferLayout;
        VertexArray point_VertexArrayObject;

        // Index  buffer for the points, lines and triangles
        IndexBuffer point_ElementBufferObject;
        IndexBuffer line_ElementBufferObject;
        IndexBuffer triangle_ElementBufferObject;
        IndexBuffer quad_ElementBufferObject;


        public mesh_store()
        {
            // Empty constructor
            points = new List<point_store>();
            elines = new List<line_store>();
            etris = new List<triangle_store>();
            equads = new List<quad_store>();
        }

         public mesh_store(List<point_store> points, 
             List<line_store> elines, 
             List<triangle_store> etris, 
             List<quad_store> equads)
        {
            // Main constructor
            this.points = points;
            this.elines = elines;
            this.etris = etris;
            this.equads = equads;
        }



        public void set_openTK_objects()
        {
            

        }


    }
}
