#pragma once
#include <iostream>
#include <fstream>
//____ OpenGL dependencies
#include <GL/glew.h>
#include <GLFW/glfw3.h>
//____ ImGUI dependencies
#include "ImGui/imgui.h"
#include "ImGui/imgui_impl_glfw.h"
#include "ImGui/imgui_impl_opengl3.h"
#include "ImGui/stb_implement.h"
//____ Geometry store
#include "geometry_store/geom_store.h"
//____ Mouse event handler
#include "events_handler/mouse_event_handler.h"
//---- File event handler
#include "events_handler/file_events.h"
//____ Tool Window
#include "tool_window/simulate_window.h"
#include "tool_window/options_window.h"


class app_window
{
public:
	GLFWwindow* window = nullptr;
	ImFont* imgui_font = nullptr;

	// Variable to control the windows mouse events
	mouse_event_handler mouse_Handler;
	// Variable to control the file menu events
	file_events file_menu;

	bool is_glwindow_success = false;
	static int window_width;
	static int window_height;
	static bool isWindowSizeChanging;

	// main geometry variable
	geom_store geom;

	// Tool window variable
	simulate_window sim_window;
	options_window op_window;


	app_window();
	~app_window();

	// Functions
	void init();
	void fini();
	void app_render();
	void menu_events();

private:
	double previous_time = 0.0;
	int frame_count = 0;
	double fps = 0.0;

	static void framebufferSizeCallback(GLFWwindow* window, int window_width, int window_height);
	void GLFWwindow_set_icon(GLFWwindow* window);
	void calculateFPS();

};