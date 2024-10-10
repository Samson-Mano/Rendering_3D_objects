#include "elementspring_list_store.h"

elementspring_list_store::elementspring_list_store()
{
	// Empty constructor

}

elementspring_list_store::~elementspring_list_store()
{
	// Empty destructor

}

void elementspring_list_store::init(geom_parameters* geom_param_ptr)
{
	// geometry parameter
	this->geom_param_ptr = geom_param_ptr;

	spring_objs.init(geom_param_ptr, true, true, false);

}

void elementspring_list_store::add_spring(int spring_id, glm::vec2 start_pt, glm::vec2 end_pt)
{


	// element length
	double element_length = geom_param_ptr->get_line_length(start_pt, end_pt); 

	// Store the data
	this->rest_start_pt = start_pt;
	this->rest_end_pt = end_pt;

	this->rest_element_length = element_length;


	// Spring portion l_cosine, m_sine
	double l_cos = (end_pt.x - start_pt.x) / element_length; 
	double m_sin = (start_pt.y - end_pt.y) / element_length; 

	//_____________________________________________________________________________________________

	// std::vector<glm::vec2> spring_pts;

	// Sping points Flat end
	int ptid_start = geom_pt_id;

	spring_objs.add_mesh_point(geom_pt_id, start_pt.x, start_pt.y);
	geom_pt_id++;

	
	glm::vec2 origin_pt = geom_parameters::linear_interpolation(start_pt, end_pt, 0.2f);
	glm::vec2 curr_pt = origin_pt;
	
	spring_objs.add_mesh_point(geom_pt_id, curr_pt.x, curr_pt.y);
	geom_pt_id++;
	

	// Spring portion
	//  Turn count is kept constant
	//int turn_count = static_cast<int>(geom_parameters::get_remap(element_max_length, element_min_length,
	//	geom_param_ptr->spring_turn_max, geom_param_ptr->spring_turn_min, element_length)); // spring turn frequency


	// glm::vec2 prev_pt = curr_pt;
	curr_pt = glm::vec2(0);

	double spring_width_amplitude = geom_param_ptr->spring_element_width *
		(geom_param_ptr->node_circle_radii / geom_param_ptr->geom_scale);

	// Points of springs
	for (int i = 1; i < turn_count; i++)
	{
		double param_t = i / static_cast<double>(turn_count);

		double pt_x = (param_t * element_length * 0.6f);
		double pt_y = spring_width_amplitude * ((i % 2 == 0) ? 1 : -1);

		curr_pt = glm::vec2(((l_cos * pt_x) + (m_sin * pt_y)), ((-1.0 * m_sin * pt_x) + (l_cos * pt_y)));
		curr_pt = curr_pt + origin_pt;

		// Add pt
		spring_objs.add_mesh_point(geom_pt_id, curr_pt.x, curr_pt.y);
		geom_pt_id++;
	}

	// Last spring point
	curr_pt = geom_parameters::linear_interpolation(start_pt, end_pt, 0.8f);

	spring_objs.add_mesh_point(geom_pt_id, curr_pt.x, curr_pt.y);
	geom_pt_id++;

	// End point
	spring_objs.add_mesh_point(geom_pt_id, end_pt.x, end_pt.y);
	geom_pt_id++;

	//_____________________________________________________________________________________________________

	for (int i = (ptid_start + 1); i < geom_pt_id; i++)
	{
		// Add the lines
		spring_objs.add_mesh_lines(geom_line_id, i - 1, i);
		geom_line_id++;
	}


	// Set th buffer
	spring_objs.set_buffer();


	// mesh color
	glm::vec3 point_color = glm::vec3(0.54509f, 0.0f, 0.4f); // Dark Red
	glm::vec3 line_color = glm::vec3(0.54509f, 0.0f, 0.4f); // Dark Red
	glm::vec3 tri_color = glm::vec3(0.54f, 0.06f, 0.31f); // Not used

	spring_objs.update_mesh_color(point_color, line_color, tri_color);


}

void elementspring_list_store::update_spring_displacement(int spring_id, glm::vec2 start_pt, glm::vec2 end_pt)
{


	// element length
	double element_length = geom_param_ptr->get_line_length(start_pt, end_pt);

	// Find the scale factors 
	// The idea is to keep the flat line length rigid but only change the spring portion length

	double factor1 = 0.2f * (this->rest_element_length / element_length);
	double factor2 = 1.0f - (2.0f * factor1);
	double factor3 = 1.0f - factor1;


	// Spring portion l_cosine, m_sine
	double l_cos = (end_pt.x - start_pt.x) / element_length;
	double m_sin = (start_pt.y - end_pt.y) / element_length;

	//_____________________________________________________________________________________________

	// std::vector<glm::vec2> spring_pts;

	// Sping points Flat end
	int ptid_start = 0;

	spring_objs.update_mesh_point(ptid_start, start_pt.x, start_pt.y);
	ptid_start++;


	glm::vec2 origin_pt = geom_parameters::linear_interpolation(start_pt, end_pt, factor1);
	glm::vec2 curr_pt = origin_pt;

	spring_objs.update_mesh_point(ptid_start, curr_pt.x, curr_pt.y);
	ptid_start++;


	// Spring portion
	//  Turn count is kept constant
	//int turn_count = static_cast<int>(geom_parameters::get_remap(element_max_length, element_min_length,
	//	geom_param_ptr->spring_turn_max, geom_param_ptr->spring_turn_min, element_length)); // spring turn frequency


	// glm::vec2 prev_pt = curr_pt;
	curr_pt = glm::vec2(0);

	double spring_width_amplitude = geom_param_ptr->spring_element_width *
		(geom_param_ptr->node_circle_radii / geom_param_ptr->geom_scale);

	// Points of springs
	for (int i = 1; i < turn_count; i++)
	{
		double param_t = i / static_cast<double>(turn_count);

		double pt_x = (param_t * element_length * factor2);
		double pt_y = spring_width_amplitude * ((i % 2 == 0) ? 1 : -1);

		curr_pt = glm::vec2(((l_cos * pt_x) + (m_sin * pt_y)), ((-1.0 * m_sin * pt_x) + (l_cos * pt_y)));
		curr_pt = curr_pt + origin_pt;

		// Add pt
		spring_objs.update_mesh_point(ptid_start, curr_pt.x, curr_pt.y);
		ptid_start++;
	}

	// Last spring point
	curr_pt = geom_parameters::linear_interpolation(start_pt, end_pt, factor3);

	spring_objs.update_mesh_point(ptid_start, curr_pt.x, curr_pt.y);
	ptid_start++;

	// End point
	spring_objs.update_mesh_point(ptid_start, end_pt.x, end_pt.y);
	ptid_start++;



}

void elementspring_list_store::paint_spring()
{
	spring_objs.paint_dynamic_mesh();

}

void elementspring_list_store::update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency)
{
	spring_objs.update_opengl_uniforms(set_modelmatrix, set_viewmatrix, set_transparency);

}
