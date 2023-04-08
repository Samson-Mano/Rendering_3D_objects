using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering_3D_objects.open_tk_control.open_tk_shader
{
    public class shader_store
    {
        // Store all the shader as string
        #region "Vertex shaders"
        public string br_vert_shader()
        {
            // Stores the Background vertex shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "uniform mat4 gTranslation;\r\n" +
                    "uniform float g_scale = 1.0f;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "\r\n" +
                    "out vec4 v_Color;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "// apply scaling \r\n" + 
                        "vec4 scaledPosition = vec4(position * g_scale, 1.0);\r\n" +
                        "// apply rotation \r\n" +
                        "vec4 rotatedPosition = rotationMatrix * scaledPosition;\r\n" +
                        "// apply translation \r\n" +
                        "vec4 translatedPosition =rotatedPosition *  gTranslation;\r\n" +
                        "\r\n" +
                        "v_Color = vertexColor;\r\n" +
                        "gl_Position = translatedPosition;\r\n" +
                    "}\r\n";
        }

        public string line_vert_shader()
        {
            // Stores the Geometry vertex shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "uniform mat4 gTranslation;\r\n" +
                    "uniform vec3 rotation_point;\r\n" +
                    "uniform float g_scale = 1.0f;\r\n" +
                    "uniform float p_scale = 1.0f;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "\r\n" +
                    "out vec4 v_Color;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "// apply scaling \r\n" +
                        "vec4 scaledPosition = vec4(position * g_scale * p_scale, 1.0);\r\n" +
                        "// apply rotation \r\n" +
                        "vec4 rotatedPosition = rotationMatrix * (scaledPosition - vec4(rotation_point,1.0));\r\n" +
                        "// apply translation \r\n" +
                        "vec4 translatedPosition = (rotatedPosition + vec4(rotation_point,1.0)) *  gTranslation;\r\n" +
                        "\r\n" +
                        "v_Color = vertexColor;\r\n" +
                        "gl_Position = translatedPosition;\r\n" +
                    "}\r\n";
        }

        public string surface_vert_shader()
        {
            // Stores the Geometry vertex shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "uniform mat4 gTranslation;\r\n" +
                    "uniform vec3 rotation_point;\r\n" +
                    "uniform float g_scale = 1.0f;\r\n" +
                    "uniform float p_scale = 1.0f;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "\r\n" +
                    "out vec4 v_Color;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "// apply scaling \r\n" +
                        "vec4 scaledPosition = vec4(position * g_scale * p_scale, 1.0);\r\n" +
                        "// apply rotation \r\n" +
                        "vec4 rotatedPosition = rotationMatrix * (scaledPosition - vec4(rotation_point,1.0));\r\n" +
                        "// apply translation \r\n" +
                        "vec4 translatedPosition = (rotatedPosition + vec4(rotation_point,1.0)) *  gTranslation;\r\n" +
                        "\r\n" +
                        "v_Color = vertexColor;\r\n" +
                        "gl_Position = translatedPosition;\r\n" +
                    "}\r\n";
        }

        public string txt_vert_shader()
        {
            // Stores the Text vertex shader
            return "";
        }
        #endregion

        #region "Fragment shaders"
        public string br_frag_shader()
        {
            // Stores the Background fragment shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec4 v_Color;\r\n" +
                    "out vec4 f_Color; // fragment's final color (out to the fragment shader)\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "f_Color = v_Color;\r\n" +
                    "}";
        }

        public string line_frag_shader()
        {
            // Stores the Geometry fragment shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec4 v_Color;\r\n" +
                    "out vec4 f_Color; // fragment's final color (out to the fragment shader)\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "f_Color = v_Color;\r\n" +
                    "}";
        }


        public string surface_frag_shader()
        {
            // Stores the Geometry fragment shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec4 v_Color;\r\n" +
                    "out vec4 f_Color; // fragment's final color (out to the fragment shader)\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "f_Color = v_Color;\r\n" +
                    "}";
        }

        public string txt_frag_shader()
        {
            // Stores the Text fragment shader
            return "";
        }
        #endregion

        public shader_store()
        {
            // Empty constructor
        }

        public string get_vertex_shader(string s_type)
        {
            // Returns the vector shader
            if (s_type == "background")
            {
                return br_vert_shader();
            }
            else if (s_type == "linegeometry")
            {
                return line_vert_shader();
            }
            else if (s_type == "surfacegeometry")
            {
                return surface_vert_shader();
            }
            else if (s_type == "text")
            {
                return txt_vert_shader();
            }

            return null;
        }

        public string get_fragment_shader(string s_type)
        {
            // Returns the fragment shader
            if (s_type == "background")
            {
                return br_frag_shader();
            }
            else if (s_type == "linegeometry")
            {
                return line_frag_shader();
            }
            else if (s_type == "surfacegeometry")
            {
                return surface_frag_shader();
            }
            else if (s_type == "text")
            {
                return txt_frag_shader();
            }

            return null;

        }
    }
}
