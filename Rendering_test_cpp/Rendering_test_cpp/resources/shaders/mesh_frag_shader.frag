#version 330 core
precision highp float;

in vec3 v_Normal;         // Transformed normal from vertex shader
in vec3 v_FragPos;        // Transformed position in world space from vertex shader
in vec3 v_Color;          // Vertex color from vertex shader
in float v_Transparency;  // Transparency from vertex shader

out vec4 f_Color;         // Final color output

void main() 
{

  // ambient lighting (global illuminance)
  vec3 ambient = vec3(0.5, 0.5, 0.5); // color - grey

  // diffuse (lambertian) lighting
  // lightColor, lightSource, normal, diffuseStrength
  vec3 normal = normalize(v_Normal.xyz);
  vec3 lightColor = vec3(1.0, 1.0, 1.0); // color - white
  vec3 lightSource = vec3(1.0, 1.0, 1.0); // coord - (1, 0, 0)
  float diffuseStrength = max(0.0, dot(lightSource, normal));
  vec3 diffuse = diffuseStrength * lightColor;

  // specular light
  // lightColor, lightSource, normal, specularStrength, viewSource
  vec3 cameraSource = vec3(0.0, 0.0, 1.0);
  vec3 viewSource = normalize(cameraSource);
  vec3 reflectSource = normalize(reflect(-lightSource, normal));
  float specularStrength = max(0.0, dot(viewSource, reflectSource));
  specularStrength = pow(specularStrength, 256.0);
  vec3 specular = specularStrength * lightColor;

  // lighting = ambient + diffuse + specular
  vec3 lighting = vec3(0.0, 0.0, 0.0); // color - black
  // lighting = ambient;
  // lighting = ambient * 0.0 + diffuse;
  // lighting = ambient * 0.0 + diffuse * 0.0 + specular;
  lighting = ambient * 0.0 + diffuse * 0.5 + specular * 0.5;

  // color = modelColor * lighting
  vec3 modelColor = vec3(0.75, 0.75, 0.75);
  vec3 color = modelColor * lighting;

  f_Color = vec4(color, v_Transparency);
  
}