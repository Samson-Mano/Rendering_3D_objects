#version 330 core
precision highp float;

in vec3 v_Normal;         // Transformed normal from vertex shader
in vec3 v_FragPos;        // Transformed position in world space from vertex shader
in vec3 v_Color;          // Vertex color from vertex shader
in float v_Transparency;  // Transparency from vertex shader
in vec3 v_camPos;
in vec3 v_lightPos;

out vec4 f_Color;         // Final color output

void main() 
{

	// ambient lighting
	float ambient = 0.20f;

	// diffuse lighting
	vec3 normal = normalize(v_Normal);
	vec3 lightDirection = normalize(v_lightPos - v_FragPos);
	float diffuse = max(dot(normal, lightDirection), 0.0f);

	// specular lighting
	float specularLight = 0.50f;
	vec3 viewDirection = normalize(v_camPos - v_FragPos);
	vec3 reflectionDirection = reflect(-lightDirection, normal);
	float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 8);
	float specular = specAmount * specularLight;

	// outputs final color
	f_Color = vec4(v_Color, 1.0f) * (diffuse + ambient + specular);
  
}