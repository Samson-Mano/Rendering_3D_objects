#include "geom_store.h"

geom_store::geom_store()
{
	// Empty Constructor
}

geom_store::~geom_store()
{
	// Empty Destructor
}

void geom_store::init(options_window* op_window, simulate_window* sim_window)
{
	// Initialize
	// Initialize the geometry parameters
	geom_param.init();

	// Label 
	label_simulation_data.init(&geom_param);

	// Set the basic matrices
	label_simulation_data.update_opengl_uniforms(true, false, false, false, false,false);

	//_____________________________________________________________________________________
	// Create the model boundary
	boundary_lines.init(&geom_param, true, true, false);

	
	is_geometry_set = false;


	// Add the window pointers
	this->sim_window = sim_window; // Simulate window
	this->op_window = op_window; // Option window


	initialize_model();
}


void geom_store::initialize_model()
{
	// Intialize the model
	std::vector< glm::vec3> bndry_pts_list;

	bndry_pts_list.push_back({-1000,-1000,0});
	bndry_pts_list.push_back({ -1000,1000,0 });
	bndry_pts_list.push_back({ 1000,1000,0 });
	bndry_pts_list.push_back({ 1000,-1000,0 });

	// Add the boundary points
	boundary_lines.add_mesh_point(0, -1000, -1000);
	boundary_lines.add_mesh_point(1, -1000, 1000);
	boundary_lines.add_mesh_point(2, 1000, 1000);
	boundary_lines.add_mesh_point(3, 1000, -1000);

	// Add the boundary lines
	boundary_lines.add_mesh_lines(0, 0, 1);
	boundary_lines.add_mesh_lines(1, 1, 2);
	boundary_lines.add_mesh_lines(2, 2, 3);
	boundary_lines.add_mesh_lines(3, 3, 0);



	is_geometry_set = true;



	// Set the boundary of the geometry
	std::pair<glm::vec3, glm::vec3> result = geom_parameters::findMinMaxXY(bndry_pts_list);
	this->geom_param.min_b = result.first;
	this->geom_param.max_b = result.second;
	this->geom_param.geom_bound = geom_param.max_b - geom_param.min_b;

	// Set the center of the geometry
	this->geom_param.center = geom_parameters::findGeometricCenter(bndry_pts_list);

	// Set the geometry
	update_model_matrix();
	update_model_zoomfit();


	// Set the geometry buffers
	this->boundary_lines.set_buffer();

}



void geom_store::fini()
{
	// Deinitialize
	is_geometry_set = false;
}


void geom_store::update_WindowDimension(const int& window_width, const int& window_height)
{
	// Update the window dimension
	this->geom_param.window_width = window_width;
	this->geom_param.window_height = window_height;

	if (is_geometry_set == true)
	{
		// Update the model matrix
		update_model_matrix();
		// !! Zoom to fit operation during window resize is handled in mouse event class !!
	}
}


void geom_store::update_model_matrix()
{
	// Set the model matrix for the model shader
	// Find the scale of the model (with 0.9 being the maximum used)
	int max_dim = geom_param.window_width > geom_param.window_height ? geom_param.window_width : geom_param.window_height;

	double normalized_screen_width = 1.6f * (static_cast<double>(geom_param.window_width) / static_cast<double>(max_dim));
	double normalized_screen_height = 1.6f * (static_cast<double>(geom_param.window_height) / static_cast<double>(max_dim));


	geom_param.geom_scale = std::min(normalized_screen_width / geom_param.geom_bound.x,
		normalized_screen_height / geom_param.geom_bound.y);

	// Translation
	glm::vec3 geom_translation = glm::vec3(-1.0f * (geom_param.max_b.x + geom_param.min_b.x) * 0.5f * geom_param.geom_scale,
		-1.0f * (geom_param.max_b.y + geom_param.min_b.y) * 0.5f * geom_param.geom_scale,
		0.0f);

	glm::mat4 g_transl = glm::translate(glm::mat4(1.0f), geom_translation);

	geom_param.modelMatrix = g_transl * glm::scale(glm::mat4(1.0f), glm::vec3(static_cast<float>(geom_param.geom_scale)));

	// Update the model matrix
	boundary_lines.update_opengl_uniforms(true, false, true);

	
}

void geom_store::update_model_zoomfit()
{
	if (is_geometry_set == false)
		return;

	// Set the pan translation matrix
	geom_param.panTranslation = glm::mat4(1.0f);

	// Rotation Matrix
	// geom_param.rotateTranslation = glm::mat4( glm::mat4_cast(0.4402697668541200f, 0.8215545196058330f, 0.2968766167094340f, -0.2075451231915790f));

	// Set the zoom scale
	geom_param.zoom_scale = 1.0f;

	// Update the zoom scale and pan translation
	boundary_lines.update_opengl_uniforms(false, true, false);

}

void geom_store::update_model_pan(glm::vec2& transl)
{
	if (is_geometry_set == false)
		return;

	// Pan the geometry
	geom_param.panTranslation = glm::mat4(1.0f);

	geom_param.panTranslation[0][3] = -1.0f * transl.x;
	geom_param.panTranslation[1][3] = transl.y;

	// Update the pan translation
	boundary_lines.update_opengl_uniforms(false, true, false);

	
}


void geom_store::update_model_zoom(double& z_scale)
{
	if (is_geometry_set == false)
		return;

	// Zoom the geometry
	geom_param.zoom_scale = z_scale;

	// Update the Zoom
	boundary_lines.update_opengl_uniforms(false, true, false);


}

void geom_store::update_model_transperency(bool is_transparent)
{
	if (is_geometry_set == false)
		return;

	if (is_transparent == true)
	{
		// Set the transparency value
		geom_param.geom_transparency = 0.2f;
	}
	else
	{
		// remove transparency
		geom_param.geom_transparency = 1.0f;
	}

	// Update the model transparency
	boundary_lines.update_opengl_uniforms(false, false, true);

}


void geom_store::paint_geometry()
{

	if (sim_window->is_show_window == true)
	{
		// Update the Simulation Model
		if (sim_window->execute_update_model == true)
		{
			// Load a model
			this->mass_m = sim_window->mass_m;
			this->stiff_k = sim_window->stiffness_k;
			this->modal_damp_si = sim_window->damping_ratio_si;

			sim_window->execute_update_model = false;
		}

	}


	if (is_geometry_set == false)
		return;

	// Clean the back buffer and assign the new color to it
	glClear(GL_COLOR_BUFFER_BIT);

	// Paint the model
	paint_model();

}


void geom_store::paint_model()
{

	// Paint the boundaries
	boundary_lines.paint_static_mesh();


	////______________________________________________
	//// Paint the model
	//if (op_window->is_show_modelelements == true)
	//{
	//	// Show the model elements
	//	mesh_data.paint_triangles();
	//	mesh_data.paint_quadrilaterals();

	//}

	//if (op_window->is_show_modeledeges == true)
	//{
	//	// Show the model edges
	//	mesh_data.paint_mesh_edges();
	//}


	//if (op_window->is_show_inlcondition == true)
	//{
	//	// Show the node initial condition
	//	// Initial Displacement
	//	glPointSize(geom_param.selected_point_size);
	//	glLineWidth(geom_param.selected_line_width);

	//	//node_inldispl.paint_inlcond();

	//	//// Initial Velocity
	//	//node_inlvelo.paint_inlcond();


	//	glPointSize(geom_param.point_size);
	//	glLineWidth(geom_param.line_width);
	//}


}
