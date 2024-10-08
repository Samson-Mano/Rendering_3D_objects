#pragma once
#include <glm/vec2.hpp>
#include <glm/vec3.hpp>
#include <unordered_map>
#include "../geom_parameters.h"
#include "../geometry_buffers/gBuffers.h"
#include "../geometry_objects/label_list_store.h"


struct pointmass_data
{
	int node_id = 0;
	glm::vec2 ptmass_loc = glm::vec2(0);
	glm::vec2 ptmass_defl = glm::vec2(0);
	double ptmass_value = 0.0;
};


class elementmass_list_store
{
public:
	const double ptmass_quad_size = 0.04;
	unsigned int ptmass_count = 0;
	std::unordered_map<int, pointmass_data> ptmassMap;

	elementmass_list_store();
	~elementmass_list_store();


private:

};
