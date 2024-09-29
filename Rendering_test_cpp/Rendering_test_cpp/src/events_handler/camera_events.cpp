#include "camera_events.h"

camera_events::camera_events()
{
	// Empty constructor
}

camera_events::~camera_events()
{
	// Empty destructor
}

void camera_events::OnMouseDown(glm::vec2 mousePt)
{


}

void camera_events::OnMouseMove(glm::vec2 mousePt)
{
    processMouseMovement(mousePt);

}

void camera_events::OnMouseUp(glm::vec2 mousePt)
{


}

void camera_events::setDefault(const int& viewType)
{
    if (viewType == 1)
    {

    }

}

glm::mat4 camera_events::getViewMatrix()
{
    glm::vec3 direction = glm::normalize(position - target);
    glm::vec3 right = glm::normalize(glm::cross(up, direction));
    glm::vec3 cameraUp = glm::cross(direction, right);

    return glm::lookAt(position, target, cameraUp);
}

void camera_events::processMouseMovement(glm::vec2 mousePt)
{
    const float sensitivity = 0.1f;
    yaw += mousePt.x * sensitivity;
    pitch += mousePt.y * sensitivity;

    // Constrain pitch to avoid screen flipping
    if (pitch > 89.0f) pitch = 89.0f;
    if (pitch < -89.0f) pitch = -89.0f;

    // Update the camera direction based on yaw and pitch
    glm::vec3 front;
    front.x = cos(glm::radians(yaw)) * cos(glm::radians(pitch));
    front.y = sin(glm::radians(pitch));
    front.z = sin(glm::radians(yaw)) * cos(glm::radians(pitch));
    target = glm::normalize(front);
}
