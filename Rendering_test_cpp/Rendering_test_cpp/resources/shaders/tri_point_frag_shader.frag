#version 330 core

in vec3 vertPosition;
in vec3 vertNormal;
in vec3 viewPos;
in vec3 v_Color;
in float v_Transparency;


out vec4 f_Color;         // Final color output


vec3 unreal(vec3 x) 
{
  return x / (x + 0.155) * 1.019;
}



void main() 
{

	vec3 normal = normalize(vertNormal);

	vec3 uniLightDir = -1.0 * normalize(vec3(0.0, 0.0, 1.0));

	vec3 viewDir = normalize(viewPos - vertPosition);
    vec3 halfDir = normalize(viewDir + uniLightDir);
	
	vec3 copper = pow(vec3(0xb6 / 255.0, 0x71 / 255.0, 0x30 / 255.0), vec3(2.2));
    vec3 specColor = mix(copper, vec3(1, 1, 1), 0.1) * 1.5;
    vec3 diffColor = copper;
    float shineness = 40;
	
    float specular = pow(max(0, dot(halfDir, normal)), shineness);
    float diffuse = max(0, dot(uniLightDir, normal));
    float ambient = 0.05;
    vec3 finalColor = (diffuse + ambient) * diffColor + specular * specColor;



    finalColor = unreal(finalColor);
    f_Color = vec4(finalColor, v_Transparency);

}