#pragma once
#include <glm/vec2.hpp>
#include <glm/vec3.hpp>
#include <unordered_map>
#include "../geom_parameters.h"
#include "../geometry_buffers/gBuffers.h"
#include "../geometry_objects/obj_mesh_data.h"


class elementfixedend_list_store
{
public:
	const double fixed_end_width = 50.0f;
	const double fixed_end_height = 300.0f;

	elementfixedend_list_store();
	~elementfixedend_list_store();

	void init(geom_parameters* geom_param_ptr);
	void add_fixed_end(glm::vec2 fixedend_loc, double fixedend_angle);

	void paint_fixed_end();

	void update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency);

private:
	obj_mesh_data fixed_ends;


};