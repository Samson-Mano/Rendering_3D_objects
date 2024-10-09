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
	fixed_ends.init(geom_param_ptr, false, true, false);

}

void elementspring_list_store::add_spring(int spring_id, glm::vec2 start_pt, glm::vec2 end_pt)
{

	// Flat ends of the spring
	int line_id = element_lines.line_count;
	temp_color = geom_param_ptr->geom_colors.spring_line_color;
	curr_pt = geom_parameters::linear_interpolation(start_node_pt, end_node_pt, 0.25f);

	// Flat end 1
	element_lines.add_line(line_id, start_node_pt, curr_pt,
		glm::vec2(0), glm::vec2(0), temp_color, temp_color, false);

	curr_pt = geom_parameters::linear_interpolation(start_node_pt, end_node_pt, 0.75f);

	// Flat end 2
	line_id = element_lines.line_count;
	element_lines.add_line(line_id, curr_pt, end_node_pt,
		glm::vec2(0), glm::vec2(0), temp_color, temp_color, false);


	// Spring portion
	int turn_count = static_cast<int>(geom_parameters::get_remap(element_max_length, element_min_length,
		geom_param_ptr->spring_turn_max, geom_param_ptr->spring_turn_min, element_length)); // spring turn frequency

	origin_pt = geom_parameters::linear_interpolation(start_node_pt, end_node_pt, 0.25f); // origin point
	prev_pt = origin_pt;
	curr_pt = glm::vec2(0);

	double spring_width_amplitude = geom_param_ptr->spring_element_width *
		(geom_param_ptr->node_circle_radii / geom_param_ptr->geom_scale);

	// Points of springs
	for (int i = 1; i < turn_count; i++)
	{
		double param_t = i / static_cast<double>(turn_count);

		double pt_x = (param_t * element_length * 0.5f);
		double pt_y = spring_width_amplitude * ((i % 2 == 0) ? 1 : -1);

		curr_pt = glm::vec2(((l_cos * pt_x) + (m_sin * pt_y)), ((-1.0 * m_sin * pt_x) + (l_cos * pt_y)));
		curr_pt = curr_pt + origin_pt;

		line_id = element_lines.line_count;

		element_lines.add_line(line_id, prev_pt, curr_pt,
			glm::vec2(0), glm::vec2(0), temp_color, temp_color, false);

		// set the previous pt
		prev_pt = curr_pt;
	}

	// Last point
	curr_pt = geom_parameters::linear_interpolation(start_node_pt, end_node_pt, 0.75f);

	line_id = element_lines.line_count;

	element_lines.add_line(line_id, prev_pt, curr_pt,
		glm::vec2(0), glm::vec2(0), temp_color, temp_color, false);



}

void elementspring_list_store::update_spring_displacement(int spring_id, glm::vec2 start_pt, glm::vec2 end_pt)
{
}

void elementspring_list_store::paint_spring()
{
}

void elementspring_list_store::update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency)
{
}
