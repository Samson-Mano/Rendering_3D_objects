using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Rendering_3D_objects.drawing_object_store.drawing_objects;


namespace Rendering_3D_objects.Text_to_mesh
{
    public class txt_to_mesh
    {
        public List<point_store> points { get; private set; }
        public List<line_store> elines { get; private set; }
        public List<triangle_store> etris { get; private set; }
        public List<quad_store>  equads { get; private set; }

        public bool is_read_valid { get; private set; }

        public txt_to_mesh(string inpt_txt)
        {
            // Convert string to mesh

            // Initialize the output variables
            points = new List<point_store>();
            elines = new List<line_store>();
            etris = new List<triangle_store>();
            equads = new List<quad_store>();

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
                        points.Add(new point_store { pt_id = n_id, 
                            x_coord = (float)x, 
                            y_coord = (float)y, 
                            z_coord = (float)z });
                    }

                    // Element
                    if (line[0] == 'e')
                    {
                        string[] values = line.Split(',');

                        // Line element
                        if (values[2] == "line")
                        {
                            int l_id = int.Parse(values[1]);
                            int spt_id = int.Parse(values[3]);
                            int ept_id = int.Parse(values[4]);

                            // Add to lines
                            elines.Add(new line_store { line_id = l_id, 
                                startpt_id = spt_id, 
                                endpt_id = ept_id });
                        }

                        // tri element
                        if (values[2] == "tri")
                        {
                            int t_id = int.Parse(values[1]);
                            int nd1 = int.Parse(values[3]);
                            int nd2 = int.Parse(values[4]);
                            int nd3 = int.Parse(values[5]);

                            // Add to lines
                            etris.Add(new triangle_store { triangle_id = t_id, 
                                pt1_id = nd1, 
                                pt2_id = nd2, 
                                pt3_id = nd3 });
                        }

                        // Quad element
                        if (values[2] == "quad")
                        {
                            int q_id = int.Parse(values[1]);
                            int nd1 = int.Parse(values[3]);
                            int nd2 = int.Parse(values[4]);
                            int nd3 = int.Parse(values[5]);
                            int nd4 = int.Parse(values[6]);

                            // Add to lines
                            equads.Add(new quad_store { quad_id = q_id, 
                                pt1_id = nd1, 
                                pt2_id = nd2, 
                                pt3_id = nd3, 
                                pt4_id = nd4 });
                        }
                    }

                }
            }

            // reading complete, check whether data is added
            is_read_valid = false;
            if(points.Count > 0 && (elines.Count > 0 || etris.Count > 0 || equads.Count > 0))
            {
                is_read_valid = true;
            }
        }
    }
}
