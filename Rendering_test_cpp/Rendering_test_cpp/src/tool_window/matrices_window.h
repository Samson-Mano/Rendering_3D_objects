#pragma once
#include <iostream>
#include "../ImGui/imgui.h"
#include "../ImGui/imgui_impl_glfw.h"
#include "../ImGui/imgui_impl_opengl3.h"

#include "../geometry_store/geom_parameters.h"

class matrices_window
{
public:
	// Window
	bool is_show_window = false;

	matrices_window();
	~matrices_window();


	void init(geom_parameters* geom_param_ptr);
	void render_window();

private:
	geom_parameters* geom_param_ptr = nullptr;

	std::string formatMat4ToString(const glm::mat4& matrix);
};