#include "elementfixedend_list_store.h"

elementfixedend_list_store::elementfixedend_list_store()
{
	// Empty constructor

}

elementfixedend_list_store::~elementfixedend_list_store()
{
	// Empty destructor

}

void elementfixedend_list_store::init(geom_parameters* geom_param_ptr)
{
	fixed_ends.init(geom_param_ptr,false,false, true);

}

void elementfixedend_list_store::add_fixed_end(glm::vec2 fixedend_loc, double fixedend_angle)
{
    // Convert angle to radians
    double angle_rad = glm::radians(fixedend_angle);

    // Precompute cosine and sine of the angle
    double cos_angle = cos(angle_rad);
    double sin_angle = sin(angle_rad);

    // Local rectangle corners relative to the top-middle point
    glm::vec2 top_left(-fixed_end_width / 2.0, fixed_end_height/ 2.0);
    glm::vec2 top_right(fixed_end_width / 2.0, fixed_end_height/ 2.0);
    glm::vec2 bottom_left(-fixed_end_width / 2.0, -fixed_end_height/2.0);
    glm::vec2 bottom_right(fixed_end_width / 2.0, -fixed_end_height/2.0);

    // Function to apply rotation to a point
    auto rotate_point = [&](const glm::vec2& point) -> glm::vec2 {
        double x_new = point.x * cos_angle - point.y * sin_angle;
        double y_new = point.x * sin_angle + point.y * cos_angle;
        return glm::vec2(x_new, y_new);
        };

    // Rotate each corner
    glm::vec2 rotated_top_left = rotate_point(top_left);
    glm::vec2 rotated_top_right = rotate_point(top_right);
    glm::vec2 rotated_bottom_left = rotate_point(bottom_left);
    glm::vec2 rotated_bottom_right = rotate_point(bottom_right);

    // Translate rotated points by the top-middle point (fixedend_loc)
    glm::vec2 final_top_left = rotated_top_left + fixedend_loc;
    glm::vec2 final_top_right = rotated_top_right + fixedend_loc;
    glm::vec2 final_bottom_left = rotated_bottom_left + fixedend_loc;
    glm::vec2 final_bottom_right = rotated_bottom_right + fixedend_loc;


    // Create the geometry
    // Fixed end point
    fixed_ends.add_mesh_point(0, final_top_left.x, final_top_left.y);
    fixed_ends.add_mesh_point(1, final_top_right.x, final_top_right.y);
    fixed_ends.add_mesh_point(2, final_bottom_right.x, final_bottom_right.y);
    fixed_ends.add_mesh_point(3, final_bottom_left.x, final_bottom_left.y);

    // Fixed end 
    fixed_ends.add_mesh_tris(0, 0, 1, 2);
    fixed_ends.add_mesh_tris(1, 2, 3, 0);

    fixed_ends.set_buffer();
}

void elementfixedend_list_store::paint_fixed_end()
{
	// Paint the fixed ends
	fixed_ends.paint_static_mesh();
}

void elementfixedend_list_store::update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency)
{
	// update the fixed ends opengl uniforms
	fixed_ends.update_opengl_uniforms(set_modelmatrix, set_viewmatrix, set_transparency);

}
