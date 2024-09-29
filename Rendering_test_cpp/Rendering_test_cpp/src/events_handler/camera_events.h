#pragma once
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include "../geometry_store/geom_parameters.h"

class camera_events
{
public:
	camera_events();
	~camera_events();

	void OnMouseDown(glm::vec2 mousePt);
	void OnMouseMove(glm::vec2 mousePt);
	void OnMouseUp(glm::vec2 mousePt);
	void setDefault(const int& viewType);

	glm::mat4 getViewMatrix();

private:
	glm::vec3 position;
	glm::vec3 target;
	glm::vec3 up;
	float yaw;
	float pitch;

	void processMouseMovement(glm::vec2 mousePt);
};