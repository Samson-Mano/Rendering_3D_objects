#pragma once
#include "elementmass_list_store.h"
#include "../geometry_objects/obj_mesh_data.h"


//struct elementspring_store
//{
//	int spring_id = 0; // ID of the line
//	glm::vec2 start_pt = glm::vec2(0);
//	glm::vec2 end_pt = glm::vec2(0);
//
//	int geom_pt_startid = 0;
//	int geom_pt_endid = 0;
//
//};



class elementspring_list_store
{
public:
	const int turn_count = 11;

	// unsigned int spring_count = 0;
	// std::unordered_map<int, int> springId_Map;
	// std::vector<elementspring_store> spring_elements;


	elementspring_list_store();
	~elementspring_list_store();

	void init(geom_parameters* geom_param_ptr);
	void add_spring(int spring_id,glm::vec2 start_pt, glm::vec2 end_pt);
	void update_spring_displacement(int spring_id, glm::vec2 start_pt, glm::vec2 end_pt);

	void paint_spring();

	void update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency);

private:
	geom_parameters* geom_param_ptr = nullptr;

	glm::vec2 rest_start_pt = glm::vec2(0); // Assumption is only one spring is stored in this class
	glm::vec2 rest_end_pt = glm::vec2(0); // Assumption is only one spring is stored in this class
	double rest_element_length = 0.0;

	int geom_pt_id = 0;
	int geom_line_id = 0;

	obj_mesh_data spring_objs;



};
