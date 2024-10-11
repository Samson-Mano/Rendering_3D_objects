#version 330 core

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;

uniform vec3 vertexColor; // color of the text
uniform float vertexTransparency; // Transparency of the text

uniform vec2 screen_topleft;

layout(location = 0) in vec2 char_position;
layout(location = 1) in vec2 textureCoord;


out vec4 v_textureColor;
out vec2 v_textureCoord;

void main()
{
	
	// Set the final position of the vertex
	vec4 final_pos = viewMatrix * modelMatrix * vec4(char_position.x, char_position.y, 0.0f, 1.0f);

	gl_Position = vec4(final_pos.x + screen_topleft.x, final_pos.y + screen_topleft.y, 0.0f, 1.0f);

	// Calculate texture coordinates for the glyph
	v_textureCoord = textureCoord;
	
	// Pass the texture color to the fragment shader
	v_textureColor = vec4(vertexColor, vertexTransparency);
}