#include "nodes_list_store.h"

nodes_list_store::nodes_list_store()
{
	// Empty constructor
}

nodes_list_store::~nodes_list_store()
{
	// Empty destructor
}

void nodes_list_store::init(geom_parameters* geom_param_ptr)
{
	// Set the geometry parameters
	this->geom_param_ptr = geom_param_ptr;

	// Clear the nodes
	node_count = 0;
	nodeMap.clear();

}

void nodes_list_store::add_node(const int& node_id, const double& x_coord, const double& y_coord)
{
	// Add the node to the list
	node_store temp_node;
	temp_node.node_id = node_id;
	temp_node.x_coord = x_coord; // x coordinate
	temp_node.y_coord = y_coord; // y coordinate

	//// Check whether the node_id is already there
	//if (nodeMap.find(node_id) != nodeMap.end())
	//{
	//	// Node ID already exist (do not add)
	//	return;
	//}

	// Insert to the nodes
	nodeMap.insert({ node_id, temp_node });
	node_count++;


	//__________________________ Add the node points
	glm::vec3 temp_color = geom_param_ptr->geom_colors.node_color;
	glm::vec2 node_pt_offset = glm::vec2(0);

	node_points.add_point(node_id, node_pt, node_pt_offset, temp_color, false);

	//__________________________ Add the node labels
	std::string temp_str = std::to_string(node_id);

	node_id_labels.add_text(temp_str, node_pt, glm::vec2(0), temp_color, 0.0f, true, false);

	// Add the node coordinate label

	std::stringstream ss_x;
	ss_x << std::fixed << std::setprecision(geom_param_ptr->coord_precision) << node_pt.x;

	std::stringstream ss_y;
	ss_y << std::fixed << std::setprecision(geom_param_ptr->coord_precision) << node_pt.y;

	temp_str = "(" + ss_x.str() + ", " + ss_y.str() + ")";

	node_coord_labels.add_text(temp_str, node_pt, glm::vec2(0), temp_color, 0.0f, false, false);

}


