using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Rendering_3D_objects.drawing_object_store;
using Rendering_3D_objects.drawing_object_store.drawing_objects;

namespace Rendering_3D_objects.Text_to_mesh
{
    public class txt_to_mesh
    {
        public nodes_store nodes { get; private set; }
        public line_elements elines { get; private set; }
        public tri_elements etris { get; private set; }
        public quad_elements equads { get; private set; }

        public bool is_read_valid { get; private set; }

        public txt_to_mesh(string inpt_txt)
        {
            // Convert string to mesh

            // Initialize the output variables
            nodes = new nodes_store();
            elines = new line_elements();
            etris = new tri_elements();
            equads = new quad_elements();

            foreach (string line in inpt_txt.Split('\r', '\n'))
            {
                if (line.Length > 0)
                {
                    // Node
                    if (line[0] == 'n')
                    {
                        string[] values = line.Split(',');

                        int n_id = int.Parse(values[1]);
                        double x = double.Parse(values[2]);
                        double y = double.Parse(values[3]);
                        double z = double.Parse(values[4]);

                        // Do something with the parsed values here
                        Color pt_color = Color.Red;
                        nodes.add_point(n_id, x, y, z, pt_color);
                    }

                    // Element
                    if (line[0] == 'e')
                    {
                        string[] values = line.Split(',');

                        // Line element
                        if (values[2] == "line")
                        {
                            int l_id = int.Parse(values[1]);
                            int nd1 = int.Parse(values[3]);
                            int nd2 = int.Parse(values[4]);

                            // Add to lines
                            elines.add_line(l_id, nd1, nd2);
                        }

                        // tri element
                        if (values[2] == "tri")
                        {
                            int l_id = int.Parse(values[1]);
                            int nd1 = int.Parse(values[3]);
                            int nd2 = int.Parse(values[4]);
                            int nd3 = int.Parse(values[5]);

                            // Add to lines
                            etris.add_triangle(l_id, nd1, nd2, nd3);
                        }

                        // Quad element
                        if (values[2] == "quad")
                        {
                            int l_id = int.Parse(values[1]);
                            int nd1 = int.Parse(values[3]);
                            int nd2 = int.Parse(values[4]);
                            int nd3 = int.Parse(values[5]);
                            int nd4 = int.Parse(values[6]);

                            // Add to lines
                            equads.add_quadrilateral(l_id, nd1, nd2, nd3, nd4, nodes);
                        }
                    }

                }
            }

            // reading complete, check whether data is added
            is_read_valid = false;
            if(nodes.all_nodes.Count>0)
            {
                is_read_valid = true;
            }
        }
    }
}
