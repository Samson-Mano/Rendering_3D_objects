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



out vec3 vertPosition;

out vec3 vertNormal;
out vec3 viewPos;
out vec3 v_Color;
out float v_Transparency;



void main()
{
	// apply zoom scaling and Rotation to model matrix
	mat4 scalingMatrix = mat4(1.0)*zoomscale;
	scalingMatrix[3][3] = 1.0f;

	mat4 viewMatrix = transpose(panTranslation) *  rotateTranslation * scalingMatrix;


	// Vertex Normal transformed
	vertNormal = normalize(transpose(inverse(mat3(viewMatrix * modelMatrix))) * node_normal);

	viewPos =  -vec3(0, 0, 1);


    v_Color = ptColor;

	v_Transparency = 1.0f;


	// Final position passed to fragment shader
    gl_Position = viewMatrix * modelMatrix  * vec4(node_position,1.0f);


}