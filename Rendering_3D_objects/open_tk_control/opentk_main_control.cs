using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
// OpenTK library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
// This app class structure
using Rendering_3D_objects.global_variables;
using Rendering_3D_objects.open_tk_control;
using Rendering_3D_objects.open_tk_control.open_tk_bgdraw;
using Rendering_3D_objects.open_tk_control.open_tk_shader;

namespace Rendering_3D_objects.open_tk_control
{
    public class opentk_main_control
    {
        // Shader Transformation
        private shader_transformations shader_trans;
        private arcball_transformation arcball_rotate;

        // Width and Height
        private int panel_width;
        private int panel_height;

        // Shader strore string
        private shader_store shader_str = new shader_store();

        // Shader variable
        // background shader
        private Shader bgrd_shader;
        // line geometry shader
        private Shader line_geom_shader;
        // surface geometry shader
        private Shader surf_geom_shader;
        // Text shader
        private Shader text_shader;

        draw_axis_store d_axis = new draw_axis_store();

        public opentk_main_control()
        {
            // main constructor
            // Set the Background color 
            Color clr_bg = gvariables_static.glcontrol_background_color;
            GL.ClearColor(((float)clr_bg.R / 255.0f),
                ((float)clr_bg.G / 255.0f),
                ((float)clr_bg.B / 255.0f),
                ((float)clr_bg.A / 255.0f));

            // Create the arcball
            arcball_rotate = new arcball_transformation();
            shader_trans = new shader_transformations();

            // create the shaders
            this.bgrd_shader = new Shader(shader_str.get_vertex_shader("background"),
                 shader_str.get_fragment_shader("background"));
            this.line_geom_shader = new Shader(shader_str.get_vertex_shader("linegeometry"),
                 shader_str.get_fragment_shader("linegeometry"));
            this.surf_geom_shader = new Shader(shader_str.get_vertex_shader("surfacegeometry"),
                 shader_str.get_fragment_shader("surfacegeometry"));
            //this.text_shader = new Shader(shader_str.get_vertex_shader("text"),
            //     shader_str.get_fragment_shader("text"));

            d_axis.set_openTK_objects();
        }

        public void set_opengl_shader(string s_type)
        {
            // Bind the shader
            if (s_type == "background")
            {
                this.bgrd_shader.Use();
            }
            else if (s_type == "linegeometry")
            {
                this.line_geom_shader.Use();
            }
            else if (s_type == "surfacegeometry")
            {
                this.surf_geom_shader.Use();
            }
            else if (s_type == "text")
            {

            }
        }


        public void paint_opengl_control_background()
        {

            // OPen GL works as state machine (select buffer & select the shader)
            // Vertex Buffer (Buffer memory in GPU VRAM)
            // Shader (program which runs on GPU to paint in the screen)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            d_axis.paint_axis();
        }

        public void update_drawing_area_size(int width, int height)
        {
            // update the drawing area size
            arcball_rotate = new arcball_transformation();
            this.bgrd_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
            this.line_geom_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
            this.surf_geom_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());

            // Store the drawing space Width and Height
            this.panel_width = width;
            this.panel_height = height;

            // Update the graphics drawing area
            if (width > height)
            {
                // translate the axis to bottom left
                float locx = 0.85f;
                float locy = (((float)height / (float)width) * 0.8f);

                Matrix4 translateMatrix = Matrix4.CreateTranslation(-locx, -locy, 0); // initialize as identity matrix
                this.bgrd_shader.SetMatrix4("gTranslation", translateMatrix);

                // Find the scale with the Maximum width 
                float scale1 = 0.01f * (800f / (float)width);
                this.bgrd_shader.SetFloat("g_scale", scale1);
                GL.Viewport(0, (int)(-(width - height) * 0.5), width, width);
            }
            else
            {
                // translate the axis to bottom left
                float locx = (((float)width / (float)height) * 0.8f);
                float locy = 0.85f;

                Matrix4 translateMatrix = Matrix4.CreateTranslation(-locx, -locy, 0); // initialize as identity matrix
                this.bgrd_shader.SetMatrix4("gTranslation", translateMatrix);

                // Find the scale with the Maximum width 
                float scale1 = 0.01f * (800f / (float)height);
                this.bgrd_shader.SetFloat("g_scale", scale1);
                GL.Viewport((int)(-(height - width) * 0.5), 0, height, height);
            }
        }

        public void update_geom_shader(Vector3 geom_bound_max, Vector3 geom_bound_min)
        {
            Vector3 geom_bound = new Vector3(geom_bound_max.X - geom_bound_min.X, geom_bound_max.Y - geom_bound_min.Y, geom_bound_max.Z - geom_bound_min.Z);
            // Update the geometry shader's geometry scale and geometry translation
            float geom_scale = 0.5f / Math.Max(Math.Max(geom_bound.X, geom_bound.Y), geom_bound.Z);
            this.line_geom_shader.SetFloat("p_scale", geom_scale);
            this.surf_geom_shader.SetFloat("p_scale", geom_scale);

            // Update the translation matrix
            Vector3 geom_transl = new Vector3(-1.0f * (geom_bound_max.X + geom_bound_min.X) * 0.5f,
                                              -1.0f * (geom_bound_max.Y + geom_bound_min.Y) * 0.5f,
                                              -1.0f * (geom_bound_max.Z + geom_bound_min.Z) * 0.5f);
            Matrix4 g_transl = Matrix4.CreateTranslation(geom_transl.X * geom_scale, geom_transl.Y * geom_scale, geom_transl.Z * geom_scale);
            // g_transl = Matrix4.CreateTranslation(0,0, 0);
            this.line_geom_shader.SetMatrix4("gTranslation", g_transl);
            this.surf_geom_shader.SetMatrix4("gTranslation", g_transl);

            // Set the rotation center
            Vector3 rotation_center = new Vector3((geom_bound_max.X + geom_bound_min.X) * 0.5f * geom_scale,
                                                      (geom_bound_max.Y + geom_bound_min.Y) * 0.5f * geom_scale,
                                                      (geom_bound_max.Z + geom_bound_min.Z) * 0.5f * geom_scale);

            arcball_rotate = new arcball_transformation();

            this.line_geom_shader.SetVector3("rotation_point", rotation_center);
            this.surf_geom_shader.SetVector3("rotation_point", rotation_center);
        }

        public void intelli_zoom_operation(double e_Delta, PointF mousePt)
        {
            // Intelli zoom all the vertex shaders

        }

        public void pan_operation(PointF mousePt)
        {
            // Pan the vertex shader

        }

        public void pan_operation_complete()
        {
            // End the pan operation saving translate

        }

        public void zoom_to_fit(ref GLControl this_Gcntrl)
        {
            // Zoom to fit the vertex shader

        }

        public void rotate_set_center(PointF mousePt)
        {
            // Set the center of rotation

        }

        public void rotate_operation_start(PointF mousePt)
        {
            // Rotate the vertex shader
            arcball_rotate.OnMouseDown(return_screen_point(mousePt));
            this.bgrd_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
            this.line_geom_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
            this.surf_geom_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
        }

        public void rotate_operation(PointF mousePt)
        {
            // Rotate the vertex shader
            arcball_rotate.OnMouseMove(return_screen_point(mousePt));
            this.bgrd_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
            this.line_geom_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
            this.surf_geom_shader.SetMatrix4("rotationMatrix", arcball_rotate.GetRotationMatrix());
        }

        public void rotate_operation_complete()
        {
            // End the rotate operation saving translate

        }

        public PointF return_screen_point(PointF mounsePt)
        {
            // Set the center of rotation
            float max_size = Math.Max(this.panel_width, this.panel_height);
            float x_val = 2.0f * (mounsePt.X - (this.panel_width * 0.5f)) / max_size;
            float y_val = -1.0f * (2.0f * (mounsePt.Y - (this.panel_height * 0.5f)) / max_size);

            return new PointF(x_val, y_val);
        }
    }
}
