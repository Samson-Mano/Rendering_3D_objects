using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering_3D_objects.open_tk_control.open_tk_shader
{
    public static class shader_store
    {
        // Store all the shader as string
        #region "Vertex shaders"
        public static string br_vert_shader()
        {
            // Stores the Background vertex shader
            return @"#version 330 core
                    
                    uniform mat4 modelMatrix;
                    uniform mat4 rotationMatrix;
                    
                    layout(location = 0) in vec3 position;
                    layout(location = 1) in vec4 vertexColor;
                    
                    
                    out vec4 v_Color;
                    
                    void main()
                    {
                        v_Color = vertexColor;
                        gl_Position = modelMatrix * rotationMatrix * vec4(position,1.0);
                    }";
        }


        public static string mesh_vert_shader()
        {
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "// Pre-computed MVP matrix on CPU for better performance\r\n" +
                    "uniform mat4 uMVP;           // Model-View-Projection matrix\r\n" +
                    "uniform mat4 uNormalMatrix;  // For normal transformation\r\n" +
                    "uniform vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 aPosition;\r\n" +
                    "layout(location = 1) in vec3 aNormal;\r\n" +
                    "\r\n" +
                    "out vec3 vNormal;\r\n" +
                    "out vec4 vColor;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                    "    gl_Position = uMVP * vec4(aPosition, 1.0);\r\n" +
                    "    vNormal = normalize(mat3(uNormalMatrix) * aNormal);\r\n" +
                    "    vColor = vertexColor;\r\n" +
                    "}\r\n";
        }




        public static string mesh_vert_shader_superseeded()
        {
            // Stores the Geometry surface vertex shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "uniform mat4 modelMatrix;\r\n" +
                    "uniform mat4 rotationMatrix;\r\n" +
                    "uniform mat4 panTranslation;\r\n" +
                    "uniform float zoomscale;\r\n" +
                    "uniform vec4 vertexColor;\r\n" +
                    "\r\n" +
                    "layout(location = 0) in vec3 position;\r\n" +
                    "layout(location = 1) in vec3 surfnormal;\r\n" +
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

        public static string txt_vert_shader()
        {
            // Stores the Text vertex shader
            return "";
        }


        public static string oit_resolution_vert_shader()
        {
            return @"
                    #version 330 core

                    layout(location = 0) in vec2 aPosition;
                    layout(location = 1) in vec2 aTexCoord;

                    out vec2 vTexCoord;

                    void main()
                    {
                        gl_Position = vec4(aPosition, 0.0, 1.0);
                        vTexCoord = aTexCoord;
                    }";
        }

        public static string mesh_vert_shader_oit()
        {
            return @"
                    #version 330 core
                    uniform mat4 uMVP;
                    layout(location = 0) in vec3 aPosition;
                    layout(location = 1) in vec3 aNormal;
                    out vec3 vNormal;
                    void main()
                    {
                        gl_Position = uMVP * vec4(aPosition, 1.0);
                        vNormal = aNormal;
                    }";
        }

        #endregion

        #region "Fragment shaders"
        public static string br_frag_shader()
        {
            // Stores the Background fragment shader
            return @"
                    #version 330 core
                    
                    in vec4 v_Color;
                    out vec4 f_Color; // fragment's final color (out to the fragment shader)
                    
                    void main()
                    {
                        f_Color = v_Color;
                    }";
        }


        public static string mesh_frag_shader_superseeded()
        {
            // Stores the Geometry surface fragment shader
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec3 vNormal;\r\n" +
                    "in vec4 vColor;\r\n" +
                    "out vec4 f_Color; // fragment's final color (out to the fragment shader)\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                        "vec3 viewDirection = {0.0,0.0,50.0}; \r\n" +
                        "// Normalize the interpolated normal \r\n" +
                        "vec3 normal = normalize(vNormal); \r\n" +
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
                        "vec4 diffuseColor = vColor * diffuse; \r\n" +
                        "vec4 specularColor = lightColor * specular; \r\n" +
                        "\r\n" +
                        "// Set the fragment color as the sum of the diffuse and specular components \r\n" +
                        "f_Color = vColor; // diffuseColor + specularColor; \r\n" +
                      "}";
        }


        //public static string mesh_frag_shader()
        //{
        //    return "#version 330 core\r\n" +
        //            "\r\n" +
        //            "in vec3 vNormal;\r\n" +
        //            "in vec4 vColor;\r\n" +
        //            "out vec4 f_Color;\r\n" +
        //            "\r\n" +
        //            "uniform vec3 uLightDir;\r\n" +
        //            "uniform float uAmbientStrength;\r\n" +
        //            "uniform bool uDoubleSided;\r\n" +
        //            "\r\n" +
        //            "void main()\r\n" +
        //            "{\r\n" +
        //            "    vec3 normal = normalize(vNormal);\r\n" +
        //            "    vec3 lightDir = normalize(uLightDir);\r\n" +
        //            "    \r\n" +
        //            "    // Calculate dot product, using absolute value for double-sided\r\n" +
        //            "    float diff = dot(normal, lightDir);\r\n" +
        //            "    if (uDoubleSided) {\r\n" +
        //            "        diff = abs(diff);  // Light both sides equally\r\n" +
        //            "    }\r\n" +
        //            "    diff = max(diff, 0.0);\r\n" +
        //            "    \r\n" +
        //            "    float ambient = uAmbientStrength;\r\n" +
        //            "    float brightness = ambient + diff;\r\n" +
        //            "    \r\n" +
        //            "    f_Color = vec4(vColor.rgb * brightness, vColor.a);\r\n" +
        //            "}\r\n";
        //}


        public static string mesh_frag_shader2()
        {
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec3 vNormal;\r\n" +
                    "in vec4 vColor;\r\n" +
                    "\r\n" +
                    "out vec4 FragColor;\r\n" +
                    "\r\n" +
                    "// Separate colors for front/back faces\r\n" +
                    "uniform vec4 uFrontColor;\r\n" +
                    "uniform vec4 uBackColor;\r\n" +
                    "\r\n" +
                    "// Simple directional light (optional but useful)\r\n" +
                    "uniform vec3 uLightDir;   // should be normalized\r\n" +
                    "uniform float uLightIntensity; // e.g. 0.2–1.0\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                    "// Normalize interpolated normal\r\n" +
                    "vec3 N = normalize(vNormal);\r\n" +
                    "\r\n" +
                    "// Basic diffuse lighting\r\n" +
                    "float NdotL = max(dot(N, normalize(uLightDir)), 0.0);\r\n" +
                    "float lighting = uLightIntensity * NdotL + 0.2; // add ambient term\r\n" +
                    "\r\n" +
                    "// Choose color based on face orientation\r\n" +
                    "vec4 baseColor = gl_FrontFacing ? uFrontColor : uBackColor;\r\n" +
                    "\r\n" +
                    "// Combine with vertex color (optional multiplier)\r\n" +
                    "vec4 finalColor = baseColor * vColor;\r\n" +
                    "\r\n" +
                    "// Apply lighting only to RGB (not alpha)\r\n" +
                    "finalColor.rgb *= lighting;\r\n" +
                    "\r\n" +
                    "FragColor = finalColor;\r\n" +
                    "}\r\n";
        }


        public static string mesh_frag_shader()
        {
            return "#version 330 core\r\n" +
                    "\r\n" +
                    "in vec3 vNormal;\r\n" +
                    "in vec4 vColor;\r\n" +
                    "out vec4 FragColor;\r\n" +
                    "\r\n" +
                    "uniform vec4 uFrontColor;\r\n" +
                    "uniform vec4 uBackColor;\r\n" +
                    "\r\n" +
                    "void main()\r\n" +
                    "{\r\n" +
                    "vec4 baseColor = gl_FrontFacing ? uFrontColor : uBackColor;\r\n" +
                    "FragColor = baseColor * vColor;\r\n" +
                    "}\r\n";
        }


        public static string txt_frag_shader()
        {
            // Stores the Text fragment shader
            return "";
        }


        public static string oit_resolution_frag_shader()
        {
            return @"
                    #version 330 core

                    in vec2 vTexCoord;
                    out vec4 f_Color;

                    uniform sampler2D uAccumulationTexture;
                    uniform sampler2D uRevealageTexture;
                    uniform vec4 uClearColor;

                    void main()
                    {
                        vec4 accum = texture(uAccumulationTexture, vTexCoord);
                        float reveal = texture(uRevealageTexture, vTexCoord).r;

                        // If nothing accumulated => output clear color
                        if (accum.a <= 1e-6)
                        {
                            f_Color = uClearColor;
                            return;
                        }

                        // Normalize accumulated color
                        vec3 color = accum.rgb / accum.a;

                        // Compute final alpha
                        float alpha = clamp(1.0 - reveal, 0.0, 1.0);

                        f_Color = vec4(color, alpha);
                    }";
        }



        public static string mesh_frag_shader_oit()
        {
            return @"
                    #version 330 core

                    in vec3 vNormal;

                    uniform vec4 uColor;

                    layout(location = 0) out vec4 accumColor;
                    layout(location = 1) out float revealColor;

                    void main()
                    {
                        vec4 color = uColor;
                        float alpha = color.a;

                        // Simple stable weight (good default)   max(0.01, min(1.0, alpha)); //
                        float weight =   max(alpha * 10.0, 0.01);

                        // Accumulation
                        accumColor.rgb = color.rgb * alpha * weight;
                        accumColor.a   = alpha * weight;
                        
                       //  accumColor = vec4(0.0,1.0,0,1.0); // vec4(uColor.rgb * weight, weight);

                        // Revealage (NO weight here) weight;
                        revealColor = alpha;
                    }";
        }
        #endregion


        public static string get_vertex_shader(string s_type)
        {
            // Returns the vector shader
            if (s_type == "background")
            {
                return br_vert_shader();
            }
            else if (s_type == "meshgeometry")
            {
                return mesh_vert_shader();
            }
            else if (s_type == "text")
            {
                return txt_vert_shader();
            }
            else if (s_type == "oit_resolution")
            {
                return oit_resolution_vert_shader();
            }

            return null;
        }

        public static string get_fragment_shader(string s_type)
        {
            // Returns the fragment shader
            if (s_type == "background")
            {
                return br_frag_shader();
            }
            else if (s_type == "meshgeometry")
            {
                return mesh_frag_shader();
            }
            else if (s_type == "text")
            {
                return txt_frag_shader();
            }
            else if (s_type == "oit_resolution")
            {
                return oit_resolution_frag_shader();
            }
            return null;

        }
    }
}
