using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Rendering_3D_objects.open_tk_control.open_tk_buffer
{
    public class FullscreenQuad
    {
        private int vao;
        private int vbo;
        private int ebo;


        public FullscreenQuad()
        {
            // Vertices: position (x,y), texcoord (u,v)
            float[] vertices = {
            -1.0f, -1.0f, 0.0f, 0.0f,  // bottom-left
             1.0f, -1.0f, 1.0f, 0.0f,  // bottom-right
             1.0f,  1.0f, 1.0f, 1.0f,  // top-right
            -1.0f,  1.0f, 0.0f, 1.0f   // top-left
        };

            uint[] indices = { 0, 1, 2, 0, 2, 3 };

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                          vertices, BufferUsageHint.StaticDraw);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int),
                          indices, BufferUsageHint.StaticDraw);

            // Position attribute
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false,
                                    4 * sizeof(float), 0);

            // TexCoord attribute
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false,
                                    4 * sizeof(float), 2 * sizeof(float));

            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }
}
