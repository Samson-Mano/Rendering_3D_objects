#pragma once
#include "geom_parameters.h"

// File system
#include <fstream>
#include <sstream>
#include <iomanip>

// Tool Windows
#include "../tool_window/options_window.h"
#include "../tool_window/simulate_window.h"

// FE Objects
#include "fe_objects/elementfixedend_list_store.h"
#include "fe_objects/elementmass_list_store.h"
#include "fe_objects/elementspring_list_store.h"

// Geometry Objects
#include "geometry_objects/obj_mesh_data.h"
#include "geometry_objects/label_list_store.h"

// Solver
#include "../solver/shm_response_solver.h"


class geom_store
{
public: 
	const double m_pi = 3.14159265358979323846;
	bool is_geometry_set = false;

	// Main Variable to strore the geometry parameters
	geom_parameters geom_param;

	geom_store();
	~geom_store();

	void init(options_window* op_window,simulate_window* sim_window);
	void fini();


	// Functions to control the drawing area
	void update_WindowDimension(const int& window_width, const int& window_height);
	void update_model_matrix();
	void update_model_zoomfit();
	void update_model_pan(glm::vec2& transl);
	void update_model_zoom(double& z_scale);
	void update_model_transperency(bool is_transparent);


	// Functions to paint the geometry and results
	void paint_geometry();
private:

	// Declare these globally or in your class
	std::chrono::high_resolution_clock::time_point lastTime = std::chrono::high_resolution_clock::now();
	float accumulatedTime = 0.0f;
	
	const float timeStep = 0.0001f; // update rate
	double total_simulation_time = 0.0; // total simualation time


	const float displ_scale_factor = 300.0f;

	// Model status
	label_list_store label_simulation_data;
	
	// Mesh objects
	elementfixedend_list_store model_fixedends;
	elementmass_list_store model_mass;
	elementspring_list_store model_spring;
	
	// Material data
	material_data mat_data;

	// Window pointers
	simulate_window* sim_window = nullptr;
	options_window* op_window = nullptr;

	// Other geometry objects
	obj_mesh_data boundary_lines;

	// Solver
	shm_response_solver shm_solver;


	void initialize_model(); // Initialize the model


	void update_simulation(); // Update the simuation
	void paint_model(); // Paint the model


	double mass_m = 1.0;
	double stiff_k = 100.0;
	double modal_damp_si = 0.0;


};

