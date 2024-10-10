#include "elementmass_list_store.h"

elementmass_list_store::elementmass_list_store()
{
	// Empty constructor
}

elementmass_list_store::~elementmass_list_store()
{
	// Empty destructor
}

void elementmass_list_store::init(geom_parameters* geom_param_ptr)
{
	// initialize the drawing objects
	mass_element.init(geom_param_ptr, false, false, true);

}

void elementmass_list_store::add_mass(int mass_id, glm::vec2 mass_loc)
{

	// Local rectangle corners relative to the top-middle point
	glm::vec2 top_left(-ptmass_size / 2.0, ptmass_size / 2.0);
	glm::vec2 top_right(ptmass_size / 2.0, ptmass_size / 2.0);
	glm::vec2 bottom_right(ptmass_size / 2.0, -ptmass_size / 2.0);
	glm::vec2 bottom_left(-ptmass_size / 2.0, -ptmass_size / 2.0);

	// Translate  points by the center point (mass_loc)
	glm::vec2 final_top_left = top_left + mass_loc;
	glm::vec2 final_top_right = top_right + mass_loc;
	glm::vec2 final_bottom_right = bottom_right + mass_loc;
	glm::vec2 final_bottom_left = bottom_left + mass_loc;

	// Create the geometry
	// Fixed end point
	mass_element.add_mesh_point(0, final_top_left.x, final_top_left.y);
	mass_element.add_mesh_point(1, final_top_right.x, final_top_right.y);
	mass_element.add_mesh_point(2, final_bottom_right.x, final_bottom_right.y);
	mass_element.add_mesh_point(3, final_bottom_left.x, final_bottom_left.y);

	// Fixed end 
	mass_element.add_mesh_tris(0, 0, 1, 2);
	mass_element.add_mesh_tris(1, 2, 3, 0);

	mass_element.set_buffer();

	// mesh color
	glm::vec3 point_color = glm::vec3(0.476f, 0.167f, 0.025f); // Not used
	glm::vec3 line_color = glm::vec3(0.476f, 0.167f, 0.025f); // Not used
	glm::vec3 tri_color = glm::vec3(0.7, 0.3, 0.7); // (Purple)

	mass_element.update_mesh_color(point_color, line_color, tri_color);

}

void elementmass_list_store::update_mass_displacement(int mass_id, glm::vec2 mass_loc)
{
	// Update the mass location
	// Local rectangle corners relative to the top-middle point
	glm::vec2 top_left(-ptmass_size / 2.0, ptmass_size / 2.0);
	glm::vec2 top_right(ptmass_size / 2.0, ptmass_size / 2.0);
	glm::vec2 bottom_right(ptmass_size / 2.0, -ptmass_size / 2.0);
	glm::vec2 bottom_left(-ptmass_size / 2.0, -ptmass_size / 2.0);

	// Translate  points by the center point (mass_loc)
	glm::vec2 final_top_left = top_left + mass_loc;
	glm::vec2 final_top_right = top_right + mass_loc;
	glm::vec2 final_bottom_right = bottom_right + mass_loc;
	glm::vec2 final_bottom_left = bottom_left + mass_loc;

	// Create the geometry
	// Fixed end point
	mass_element.update_mesh_point(0, final_top_left.x, final_top_left.y);
	mass_element.update_mesh_point(1, final_top_right.x, final_top_right.y);
	mass_element.update_mesh_point(2, final_bottom_right.x, final_bottom_right.y);
	mass_element.update_mesh_point(3, final_bottom_left.x, final_bottom_left.y);


}

void elementmass_list_store::paint_pointmass()
{
	// Paint the mass element
	mass_element.paint_dynamic_mesh();

}

void elementmass_list_store::update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency)
{
	// update the mass element opengl uniforms
	mass_element.update_opengl_uniforms(set_modelmatrix, set_viewmatrix, set_transparency);

}
