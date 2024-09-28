#pragma once
#include "geom_parameters.h"

// File system
#include <fstream>
#include <sstream>
#include <iomanip>

// Tool Windows
#include "../tool_window/options_window.h"
#include "../tool_window/new_model_window.h"

// FE Objects
#include "fe_objects/nodes_list_store.h"
#include "fe_objects/elementline_list_store.h"
#include "fe_objects/elementtri_list_store.h"
#include "fe_objects/elementquad_list_store.h"

// Geometry Objects
#include "geometry_objects/dcel_mesh_data.h"
#include "geometry_objects/dcel_dynmesh_data.h"
#include "geometry_objects/dynamic_selrectangle_store.h"


class geom_store
{
public: 
	const double m_pi = 3.14159265358979323846;
	bool is_geometry_set = false;

	// Main Variable to strore the geometry parameters
	geom_parameters geom_param;

	geom_store();
	~geom_store();

	void init(options_window* op_window,new_model_window* md_window);
	void fini();

	// Load the geometry
	void load_model(const int& model_type,std::vector<std::string> input_data);

	// Functions to control the drawing area
	void update_WindowDimension(const int& window_width, const int& window_height);
	void update_model_matrix();
	void update_model_zoomfit();
	void update_model_pan(glm::vec2& transl);
	void update_model_rotate(glm::mat4& rotation_m);
	void update_model_zoom(double& z_scale);
	void update_model_transperency(bool is_transparent);

	// Function to paint the selection rectangle
	void update_selection_rectangle(const glm::vec2& o_pt, const glm::vec2& c_pt,
		const bool& is_paint, const bool& is_select, const bool& is_rightbutton);

	// Functions to paint the geometry and results
	void paint_geometry();
private:
	// geometry objects
	dynamic_selrectangle_store selection_rectangle;
	dcel_mesh_data mesh_data;

	// Mesh objects
	nodes_list_store model_nodes;
	elementtri_list_store model_trielements;
	elementquad_list_store model_quadelements;

	// Material data
	material_data mat_data;

	// Window pointers
	new_model_window* md_window = nullptr;
	options_window* op_window = nullptr;

	void paint_model(); // Paint the model

};

