#pragma once
#include <iostream>
#include <vector>
#include <string>
#include <sstream>
#include "../ImGui/imgui.h"
#include "../ImGui/imgui_impl_glfw.h"
#include "../ImGui/imgui_impl_opengl3.h"


class simulate_window
{
public:
	bool is_show_window = false;
	bool execute_update_model = false;

	double mass_m = 1.0;
	double stiffness_k = 100.0;
	double damping_ratio_si = 0.05;


	simulate_window();
	~simulate_window();
	void init();
	void render_window();
private:

};
