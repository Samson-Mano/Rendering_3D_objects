#pragma once
#include "elementmass_list_store.h"
#include "../geometry_objects/obj_mesh_data.h"


struct elementspring_store
{
	int spring_id = 0; // ID of the line
	glm::vec2 start_pt = glm::vec2(0);
	glm::vec2 end_pt = glm::vec2(0);

	int geom_pt_startid = 0;
	int geom_pt_endid = 0;

};



class elementspring_list_store
{
public:
	unsigned int spring_count = 0;
	std::unordered_map<int, int> springId_Map;
	std::vector<elementspring_store> spring_elements;


	elementspring_list_store();
	~elementspring_list_store();

	void init(geom_parameters* geom_param_ptr);
	void add_spring(int spring_id,glm::vec2 start_pt, glm::vec2 end_pt);
	void update_spring_displacement(int spring_id, glm::vec2 start_pt, glm::vec2 end_pt);

	void paint_spring();

	void update_opengl_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency);

private:
	obj_mesh_data fixed_ends;



};
