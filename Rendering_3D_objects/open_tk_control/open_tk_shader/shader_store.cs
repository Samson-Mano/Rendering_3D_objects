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
                    "uniform mat4 modelMatrix;\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "\r\n" +
                    "out vec4 v_Color;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "v_Color = vertexColor;\r\n" +
                        "gl_Position = modelMatrix * rotationMatrix * vec4(position,1.0);\r\n" +
                    "}\r\n";
        }

        public string line_vert_shader()
        {
            // Stores the Geometry line vertex shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "uniform mat4 modelMatrix;\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "uniform mat4 panTranslation;\r\n" +
                    "uniform float zoomscale;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "\r\n" +
                    "out vec4 v_Color;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "// apply zoom scaling and Rotation to model matrix \r\n" +
                        "mat4 scalingMatrix = mat4(1.0)*zoomscale; \r\n" +
                        "scalingMatrix[3][3] = 1.0f; \r\n" +
                        "mat4 scaledModelMatrix = scalingMatrix * modelMatrix; \r\n" +
                        "mat4 rotatedModelMatrix = rotationMatrix * scaledModelMatrix; \r\n" +
                        "mat4 translatedModelMatrix =  rotatedModelMatrix * panTranslation; \r\n" +
                        "\r\n" +
                        "// apply Translation to the final position \r\n" +
                        "vec4 finalPosition = rotatedModelMatrix * vec4(position,1.0f) * panTranslation;\r\n" +
                        "\r\n" +
                        "v_Color = vertexColor;\r\n" +
                        "gl_Position = finalPosition;\r\n" +
                    "}\r\n";
        }

        public string surface_vert_shader()
        {
            // Stores the Geometry surface vertex shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "uniform mat4 modelMatrix;\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "uniform mat4 panTranslation;\r\n" +
                    "uniform float zoomscale;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec4 vertexColor;\r\n" +
                    "layout(location = 2) in vec3 surfnormal;\r\n" +
                    "\r\n" +
                    "\r\n" +
                    "out vec3 s_normal;\r\n" +
                    "out vec4 v_Color;\r\n" +
                    "\r\n" +
                    "mat4 scaleMatrix(in float scale) \r\n" +
                    "{ \r\n" +
                        "return mat4( \r\n" +
                          "scale, 0.0, 0.0, 0.0, \r\n" +
                          "0.0, scale, 0.0, 0.0, \r\n" +
                          "0.0, 0.0, scale, 0.0, \r\n" +
                          "0.0, 0.0, 0.0, 1.0 \r\n" +
                        "); \r\n" +
                    "} \r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "\r\n" +
                        "// apply zoom scaling and Rotation to model matrix \r\n" +
                        "mat4 scalingMatrix = scaleMatrix(zoomscale); \r\n" +
                        "mat4 scaledModelMatrix = scalingMatrix * modelMatrix; \r\n" +
                        "mat4 rotatedModelMatrix = rotationMatrix * scaledModelMatrix; \r\n" +
                        "mat4 translatedModelMatrix =  rotatedModelMatrix * panTranslation; \r\n" +
                        "\r\n" +
                        "// apply Translation to the final position \r\n" +
                        "vec4 finalPosition = rotatedModelMatrix * vec4(position,1.0f) * panTranslation;\r\n" +
                        "\r\n" +
                        "vec4 finalsurfnormal = rotationMatrix * vec4(surfnormal,1.0f);\r\n" +
                        "\r\n" +
                        "s_normal = vec3(finalsurfnormal.x,finalsurfnormal.y,finalsurfnormal.z);\r\n" +
                        "v_Color = vertexColor;\r\n" +
                        "gl_Position = finalPosition;\r\n" +
                    "} \r\n";
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
            // Stores the Geometry line fragment shader
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
            // Stores the Geometry surface fragment shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec3 s_normal;\r\n" +
                    "in vec4 v_Color;\r\n" +
                    "out vec4 f_Color; // fragment's final color (out to the fragment shader)\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "vec3 viewDirection = {0.0,0.0,50.0}; \r\n" +
                        "// Normalize the interpolated normal \r\n" +
                        "vec3 normal = normalize(s_normal); \r\n" +
                        "\r\n" +
                        "// Use the constant view direction \r\n" +
                        "vec3 viewDir = {0.0,0.0,1.0}; // normalize(viewDirection); \r\n" +
                        "\r\n" +
                        "// Use the light direction slightly off the view direction \r\n" +
                        "vec3 lightDir = vec3(0, 0, 1); //normalize(viewDir + vec3(0, 0, 1)); \r\n" +
                        "\r\n" +
                        "// Set the constant light color to white \r\n" +
                        "vec4 lightColor = vec4(1.0); \r\n" +
                        "\r\n" +
                        "// Calculate the reflection vector \r\n" +
                        "vec3 reflection = reflect(-lightDir, normal); \r\n" +
                        "\r\n" +
                        "// Calculate the diffuse and specular components of the Phong model \r\n" +
                        "float diffuse = max(dot(lightDir, normal), 0.0); \r\n" +
                        "float specular = pow(max(dot(reflection, viewDir), 0.0), 32.0); \r\n" +
                        "\r\n" +
                        "// Combine the diffuse and specular components with the vertex color and light color \r\n" +
                        "vec4 diffuseColor = v_Color * diffuse; \r\n" +
                        "vec4 specularColor = lightColor * specular; \r\n" +
                        "\r\n" +
                        "// Set the fragment color as the sum of the diffuse and specular components \r\n" +
                        "f_Color = diffuseColor + specularColor; \r\n" +
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
