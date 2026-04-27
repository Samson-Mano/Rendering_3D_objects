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
            return @"#version 330 core
                    
                    // Pre-computed MVP matrix on CPU for better performance
                    uniform mat4 uMVP;           // Model-View-Projection matrix
                    uniform mat4 uNormalMatrix;  // For normal transformation
                    uniform vec4 vertexColor;
                    
                    layout(location = 0) in vec3 aPosition;
                    layout(location = 1) in vec3 aNormal;
                    
                    out vec3 vNormal;
                    out vec4 vColor;
                    
                    void main()
                    {
                        gl_Position = uMVP * vec4(aPosition, 1.0);
                        vNormal = normalize(mat3(uNormalMatrix) * aNormal);
                        vColor = vertexColor;
                    }";
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
            return @"#version 330 core
                    
                    in vec3 vNormal;
                    in vec4 vColor;
                    out vec4 f_Color; // fragment's final color (out to the fragment shader)
                    
                    void main()
                    {
                        vec3 viewDirection = {0.0,0.0,50.0}; 
                        // Normalize the interpolated normal 
                        vec3 normal = normalize(vNormal);
                        
                        // Use the constant view direction 
                        vec3 viewDir = {0.0,0.0,1.0}; // normalize(viewDirection); 
                        
                        // Use the light direction slightly off the view direction 
                        vec3 lightDir = vec3(0, 0, 1); //normalize(viewDir + vec3(0, 0, 1)); 
                        
                        // Set the constant light color to white 
                        vec4 lightColor = vec4(1.0); 
                        
                        // Calculate the reflection vector 
                        vec3 reflection = reflect(-lightDir, normal); 
                        
                        // Calculate the diffuse and specular components of the Phong model 
                        float diffuse = max(dot(lightDir, normal), 0.0);
                        float specular = pow(max(dot(reflection, viewDir), 0.0), 32.0); 
                        
                        // Combine the diffuse and specular components with the vertex color and light color 
                        vec4 diffuseColor = vColor * diffuse; 
                        vec4 specularColor = lightColor * specular; 
                        
                        // Set the fragment color as the sum of the diffuse and specular components 
                        f_Color = vColor; // diffuseColor + specularColor; 
                      }";
        }


        //public static string mesh_frag_shader()
        //{
        //    return @"#version 330 core
                    
        //            in vec3 vNormal;
        //            in vec4 vColor;
        //            out vec4 f_Color;
                    
        //            uniform vec3 uLightDir;
        //            uniform float uAmbientStrength;
        //            uniform bool uDoubleSided;
                    
        //            void main()
        //            {
        //                vec3 normal = normalize(vNormal);
        //                vec3 lightDir = normalize(uLightDir);
                    
        //                // Calculate dot product, using absolute value for double-sided
        //                float diff = dot(normal, lightDir);
        //                if (uDoubleSided) {
        //                    diff = abs(diff);  // Light both sides equally
        //                }
        //                diff = max(diff, 0.0);
                    
        //                float ambient = uAmbientStrength;
        //                float brightness = ambient + diff;
                    
        //                f_Color = vec4(vColor.rgb * brightness, vColor.a);
        //            }";
        //}


        public static string mesh_frag_shader2()
        {
            return @"#version 330 core
                    
                    in vec3 vNormal;
                    in vec4 vColor;
                    
                    out vec4 FragColor;
                    
                    // Separate colors for front/back faces
                    uniform vec4 uFrontColor;
                    uniform vec4 uBackColor;
                    
                    // Simple directional light (optional but useful)
                    uniform vec3 uLightDir;   // should be normalized
                    uniform float uLightIntensity; // e.g. 0.2–1.0
                    
                    void main()
                    {
                    // Normalize interpolated normal
                    vec3 N = normalize(vNormal);
                    
                    // Basic diffuse lighting
                    float NdotL = max(dot(N, normalize(uLightDir)), 0.0);
                    float lighting = uLightIntensity * NdotL + 0.2; // add ambient term
                    
                    // Choose color based on face orientation
                    vec4 baseColor = gl_FrontFacing ? uFrontColor : uBackColor;
                    
                    // Combine with vertex color (optional multiplier)
                    vec4 finalColor = baseColor * vColor;
                    
                    // Apply lighting only to RGB (not alpha)
                    finalColor.rgb *= lighting;
                    
                    FragColor = finalColor;
                    }";
        }


        public static string mesh_frag_shader()
        {
            return @"#version 330 core
                   
                    in vec3 vNormal;
                    in vec4 vColor;
                    out vec4 FragColor;
                    
                    uniform vec4 uFrontColor;
                    uniform vec4 uBackColor;
                    
                    void main()
                    {
                        vec4 baseColor = gl_FrontFacing ? uFrontColor : uBackColor;
                        FragColor = baseColor * vColor;
                    };";
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
                        float weight =  max(0.01, min(1.0, alpha)); //  max(alpha * 10.0, 0.01);

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
