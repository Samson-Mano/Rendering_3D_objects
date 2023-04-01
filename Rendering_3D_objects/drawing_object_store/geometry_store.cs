using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rendering_3D_objects.drawing_object_store.drawing_objects;
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Rendering_3D_objects.drawing_object_store
{
    public class geometry_store
    {
        public nodes_store nodes { get; private set; }
        public line_elements elines { get; private set; }
        public tri_elements etris { get; private set; }
        public quad_elements equads { get; private set; }

        public geometry_store(nodes_store nodes, line_elements elines, tri_elements etris, quad_elements equads)
        {
            // Main constructor
            this.nodes = nodes;
            this.elines = elines;
            this.etris = etris;
            this.equads = equads;
        }



    }
}
