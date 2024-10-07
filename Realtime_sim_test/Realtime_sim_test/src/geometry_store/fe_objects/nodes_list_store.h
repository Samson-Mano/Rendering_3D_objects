#pragma once
#include <glm/vec2.hpp>
#include <glm/vec3.hpp>
#include <unordered_map>
#include "../geom_parameters.h"
#include "../geometry_buffers/gBuffers.h"
#include "../geometry_objects/point_list_store.h"
#include "../geometry_objects/label_list_store.h"

struct node_store
{
	int node_id = -1;
	double x_coord = 0.0;
	double y_coord = 0.0;

	glm::vec2 node_pt() const 
	{
		return glm::vec2(x_coord, y_coord);
	}
};


class nodes_list_store
{
public:
	unsigned int node_count = 0;
	std::unordered_map<int, node_store> nodeMap; // Create an unordered_map to store nodes with ID as key


	nodes_list_store();
	~nodes_list_store();
	void init(geom_parameters* geom_param_ptr);
	void add_node(const int& node_id,  const double& x_coord, const double& y_coord);

private:
	geom_parameters* geom_param_ptr = nullptr;

	point_list_store node_points;
	label_list_store node_id_labels;
	label_list_store node_coord_labels;


};
