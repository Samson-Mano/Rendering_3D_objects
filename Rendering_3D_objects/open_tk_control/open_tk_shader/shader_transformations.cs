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


namespace Rendering_3D_objects.open_tk_control.open_tk_shader
{
    public class shader_transformations
    {
       public Vector3 pan_position { get; private set; }    
        
        public Vector3 drag_rotation { get; private set; }
        
        public Vector3 zoom_scale { get; private set; }


        public shader_transformations()
        {
            // Initialize the transformation
            pan_position = new Vector3(0, 0, 0);
            drag_rotation = new Vector3(0, 0, 0);
            zoom_scale = new Vector3(1.0f, 1.0f, 1.0f);
        }


    }
}
