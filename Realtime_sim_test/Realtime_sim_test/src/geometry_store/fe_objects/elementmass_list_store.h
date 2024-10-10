#pragma once
#include <glm/vec2.hpp>
#include <glm/vec3.hpp>
#include <unordered_map>
#include "../geom_parameters.h"
#include "../geometry_buffers/gBuffers.h"
#include "../geometry_objects/label_list_store.h"
#include "../geometry_objects/obj_mesh_data.h"

//struct pointmass_data
//{
//	int node_id = 0;
//	glm::vec2 ptmass_loc = glm::vec2(0);
//	glm::vec2 ptmass_defl = glm::vec2(0);
//	double ptmass_value = 0.0;
//
//	double l_cos = 0.0;
//	double m_sin = 0.0;
//
//};


class elementmass_list_store
{
public:
	const double ptmass_size = 300.0f;

	// unsigned int ptmass_count = 0;
	// std::unordered_map<int, pointmass_data> ptmassMap;

	elementmass_list_store();
	~elementmass_list_store();

	void init(geom_parameters* geom_param_ptr);
	void add_mass(int mass_id, glm::vec2 mass_loc);
	void update_mass_displacement(int mass_id, glm::vec2 mass_loc);


	void paint_pointmass();

	void update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency);

private:
	obj_mesh_data mass_element;


};
