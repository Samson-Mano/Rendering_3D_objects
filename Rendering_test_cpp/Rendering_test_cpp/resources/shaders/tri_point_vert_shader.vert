#version 330 core

uniform mat4 modelMatrix;
uniform mat4 panTranslation;
uniform mat4 rotateTranslation;

uniform float zoomscale;

uniform vec3 light_position_w;
uniform vec3 ptColor; // color of the mesh
uniform float transparency; // Transparency of the mesh



layout(location = 0) in vec3 node_position;
layout(location = 1) in vec3 node_normal;



out vec3 position_w;
out vec3 normal_c;
out vec3 eye_direction_c;
out vec3 light_direction_c;

out vec3 v_Color;
out float v_Transparency;



void main()
{
	// apply zoom scaling and Rotation to model matrix
	mat4 scalingMatrix = mat4(1.0)*zoomscale;
	scalingMatrix[3][3] = 1.0f;

	mat4 viewMatrix = rotateTranslation * scalingMatrix;


	// mat4 scaledModelMatrix =  rotateTranslation * scalingMatrix * modelMatrix ;


	// Final position passed to fragment shader
    gl_Position = viewMatrix * modelMatrix  * vec4(node_position,1.0f) * panTranslation;

    //_______________________________________________________________________________________

    // 1. Vertex position in the world coord 
    position_w = (modelMatrix * vec4( node_position, 1 )).xyz;


	// 2. Direction of Eye
	vec3 vertex_pos_c = ( viewMatrix * modelMatrix * vec4( node_position, 1 ) ).xyz;
    eye_direction_c = vertex_pos_c;


	// 3. Position of light
	vec3 light_position_c = (viewMatrix * vec4(light_position_w, 1) ).xyz;
    light_direction_c = light_position_c + eye_direction_c;

	// 4. Normals in camera space
	normal_c = (viewMatrix * modelMatrix * vec4( node_normal, 0.0 )).xyz;


    v_Color = ptColor;

	v_Transparency = 1.0f;

}