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
using Rendering_3D_objects.drawing_object_store.drawing_objects.object_store;
using Rendering_3D_objects.global_variables;
using Rendering_3D_objects.open_tk_control;
using Rendering_3D_objects.open_tk_control.open_tk_bgdraw;
using Rendering_3D_objects.open_tk_control.open_tk_shader;

namespace Rendering_3D_objects.open_tk_control
{
    public class opentk_main_control
    {
        // Width and Height
        private int panel_width;
        private int panel_height;
        private Vector3 geom_bound_max;
        private Vector3 geom_bound_min;

        // Rotate Transformation
        private Vector3 rotation_center;
        private arcball_transformation arcball_rotate;

        // Geometry Translation
        private Vector3 geom_translation;
        private float geom_scale;
        private Matrix4 geom_model_matrix;
        private Matrix4 total_rotation;

        // Pan Translate transformation store
        private Vector3 previous_translation;
        private Vector3 current_translation;
        // Zoom Scale Transformation
        public float zoom_scale { get; private set; }

        // Shader store string
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
            
            // Create a null geometry bounds
            this.geom_bound_max = Vector3.Zero;
            this.geom_bound_min = Vector3.Zero;
            this.geom_scale = 1.0f;

            // SRT Transformation (Scale, Rotate, Translate)
            // Zoom Scale
            this.zoom_scale = 1.0f;

            // Zero Rotation (Create the arcball) 
            arcball_rotate = new arcball_transformation();
            this.rotation_center = Vector3.Zero;
            this.total_rotation = arcball_rotate.GetRotationMatrix();

            // Zero translation
            this.geom_translation = Vector3.Zero;
            this.previous_translation = Vector3.Zero;
            this.current_translation = Vector3.Zero;

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
            // Store the drawing space Width and Height
            this.panel_width = width;
            this.panel_height = height;

            // SRT Transformation (Scale, Rotate, Translate)
            // Zoom Scale
            this.zoom_scale = 1.0f;
            this.line_geom_shader.SetFloat("zoomscale", this.zoom_scale);
            this.surf_geom_shader.SetFloat("zoomscale", this.zoom_scale);

            // update the drawing area size
            arcball_rotate = new arcball_transformation();
            this.total_rotation = arcball_rotate.GetRotationMatrix();
            this.bgrd_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.line_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.surf_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);

            // Zero translation
            this.previous_translation = Vector3.Zero;
            this.current_translation = Vector3.Zero;
            translate_Transform(0.0f, 0.0f);

            // Update the graphics drawing area
            // Only applicable to Background
            if (width > height)
            {
                // translate the axis to bottom left
                float locx = 0.85f;
                float locy = (((float)height / (float)width) * 0.8f);

                // Create translation for axis, which is the bottom corner
                Matrix4 bgrd_scale_translateMatrix = Matrix4.CreateTranslation(-locx, -locy, 0);

                // Find the scale with the Maximum width 
                float bgrd_scale = 0.01f * (800f / (float)width);

                // Create the background model matrix
                Matrix4 bgrd_modelMatrix = Matrix4.CreateScale(bgrd_scale) * bgrd_scale_translateMatrix;
                bgrd_modelMatrix.Transpose();

                // Set the back ground model matrix
                this.bgrd_shader.SetMatrix4("modelMatrix", bgrd_modelMatrix);

                GL.Viewport(0, (int)(-(width - height) * 0.5), width, width);
            }
            else
            {
                // translate the axis to bottom left
                float locx = (((float)width / (float)height) * 0.8f);
                float locy = 0.85f;
                // Create translation for axis, which is the bottom corner
                Matrix4 bgrd_scale_translateMatrix = Matrix4.CreateTranslation(-locx, -locy, 0);

                // Find the scale with the Maximum width 
                float bgrd_scale = 0.01f * (800f / (float)height);

                // Create the background model matrix
                Matrix4 bgrd_modelMatrix = Matrix4.CreateScale(bgrd_scale) * bgrd_scale_translateMatrix;
                bgrd_modelMatrix.Transpose();

                // Set the back ground model matrix
                this.bgrd_shader.SetMatrix4("modelMatrix", bgrd_modelMatrix);

                GL.Viewport((int)(-(height - width) * 0.5), 0, height, height);
            }

           // GL.DepthRange(0.0f, 1.0f);
        }

        public void update_geom_shader(Vector3 geom_bound_max, Vector3 geom_bound_min)
        {
            // Set the geometry boundary
            this.geom_bound_max = geom_bound_max;
            this.geom_bound_min = geom_bound_min;

            Vector3 geom_bound = new Vector3(geom_bound_max.X - geom_bound_min.X, geom_bound_max.Y - geom_bound_min.Y, geom_bound_max.Z - geom_bound_min.Z);
            // Update the geometry shader's geometry scale and geometry translation
            this.geom_scale = 0.5f / Math.Max(Math.Max(geom_bound.X, geom_bound.Y), geom_bound.Z);

            // Update the Geometry translation matrix
            this.geom_translation = new Vector3(-1.0f * (geom_bound_max.X + geom_bound_min.X) * 0.5f * this.geom_scale,
                                              -1.0f * (geom_bound_max.Y + geom_bound_min.Y) * 0.5f * this.geom_scale,
                                              -1.0f * (geom_bound_max.Z + geom_bound_min.Z) * 0.5f * this.geom_scale);
            Matrix4 g_transl = Matrix4.CreateTranslation(this.geom_translation);

            // Construct the model matrix
            Matrix4 modelMatrix = Matrix4.CreateScale(this.geom_scale) * g_transl;
            modelMatrix.Transpose();

            // Store the model matrix
            this.geom_model_matrix = modelMatrix;

            this.line_geom_shader.SetMatrix4("modelMatrix", modelMatrix);
            this.surf_geom_shader.SetMatrix4("modelMatrix", modelMatrix);

            // Zoom Scale
            this.zoom_scale = 1.0f;
            this.line_geom_shader.SetFloat("zoomscale", this.zoom_scale);
            this.surf_geom_shader.SetFloat("zoomscale", this.zoom_scale);

            // Set the rotation center
            this.rotation_center = new Vector3((geom_bound_max.X + geom_bound_min.X) * 0.5f * this.geom_scale,
                                               (geom_bound_max.Y + geom_bound_min.Y) * 0.5f * this.geom_scale,
                                               (geom_bound_max.Z + geom_bound_min.Z) * 0.5f * this.geom_scale);

            /*
            //  this.rotation_center = Vector3.Zero;
            arcball_rotate = new arcball_transformation();
            this.total_rotation = arcball_rotate.GetRotationMatrix();
            this.bgrd_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.line_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.surf_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
           */


            // Update the pan translation of geometry
            // Zero translation
            this.previous_translation = Vector3.Zero;
            this.current_translation = Vector3.Zero;
            translate_Transform(0.0f, 0.0f);

            Matrix4 p_transl = Matrix4.CreateTranslation(Vector3.Zero);

            this.line_geom_shader.SetMatrix4("panTranslation", p_transl);
            this.surf_geom_shader.SetMatrix4("panTranslation", p_transl);
        }

        public void intelli_zoom_operation(double e_Delta, PointF mousePt)
        {
            // Intelli zoom all the vertex shaders

            Vector2 screen_pt_b4_scale = get_normalized_screen_pt(mousePt, this.previous_translation.X, this.previous_translation.Y);

            if (e_Delta > 0)
            {
                if (this.zoom_scale < 1000)
                {
                    this.zoom_scale = this.zoom_scale + 0.1f;
                }
            }
            else if (e_Delta < 0)
            {
                if (this.zoom_scale > 0.101)
                {
                    this.zoom_scale = this.zoom_scale - 0.1f;
                }
            }

            // Get the screen pt after scaling
            Vector2 screen_pt_a4_scale = get_normalized_screen_pt(mousePt, this.previous_translation.X, this.previous_translation.Y);

            float tx = (-1.0f) * this.zoom_scale * 0.5f * (screen_pt_b4_scale.X - screen_pt_a4_scale.X);
            float ty = (-1.0f) * this.zoom_scale * 0.5f * (screen_pt_b4_scale.Y - screen_pt_a4_scale.Y);

            //update the zoom scale
            this.line_geom_shader.SetFloat("zoomscale", this.zoom_scale);
            this.surf_geom_shader.SetFloat("zoomscale", this.zoom_scale);

            translate_Transform(tx, ty);
            pan_operation_complete();
        }


        private Vector2 get_normalized_screen_pt(PointF mousePt, float transl_x, float transl_y)
        {
            // Get the normalized screen point for zoom operation
            float mid_x = this.panel_width * 0.5f;
            float mid_y = this.panel_height * 0.5f;
            float min_size = Math.Min(this.panel_width, this.panel_height);
            float margin = 0.0f;

            float mouse_x = ((mousePt.X - mid_x) / ((min_size - margin) * 0.5f));
            float mouse_y = -1.0f * ((mousePt.Y - mid_y) / ((min_size - margin) * 0.5f));

            return (new Vector2((mouse_x - (2.0f * transl_x)) / this.zoom_scale,
                               (mouse_y - (2.0f * transl_y)) / this.zoom_scale));
        }

        public void pan_operation(PointF mousePt)
        {
            // Pan the vertex shader
            // Pan operation is in progress
            float tx = (mousePt.X / (Math.Max(this.panel_width, this.panel_height) * 0.5f));
            float ty = (mousePt.Y / (Math.Max(this.panel_width, this.panel_height) * 0.5f));

            // Translate the drawing area
            translate_Transform(tx, -1.0f * ty);
        }

        public void pan_operation_complete()
        {
            // End the pan operation saving translate
            this.previous_translation = this.current_translation;
        }

        private void translate_Transform(float trans_x, float trans_y)
        {
            // 2D Translatoin
            this.current_translation = new Vector3(trans_x + this.previous_translation.X,
                                                   trans_y + this.previous_translation.Y,
                                                    0.0f + this.previous_translation.Z);

            Matrix4 current_transformation = Matrix4.CreateTranslation(this.current_translation);

            // Update the shader translation
            this.line_geom_shader.SetMatrix4("panTranslation", current_transformation);
            this.surf_geom_shader.SetMatrix4("panTranslation", current_transformation);
        }

        public void rotate_set_center(PointF mousePt, drawing_object_store.drawing_objects.nodes_store nodes)
        {
            // Conver the screen point to openTK window point
            float maxDimension = Math.Max(this.panel_width, this.panel_height);

            // Shift the point to zero,zero at center and invert the y-axis
            Vector2 screen_shift = new Vector2(mousePt.X - (this.panel_width * 0.5f), (this.panel_height * 0.5f) - mousePt.Y);

            // Find the screen point
            Vector2 screen_pt = new Vector2((2.0f * screen_shift.X) / maxDimension, (2.0f * screen_shift.Y) / maxDimension);

            // Check all the nodes whether the screen point clicked or not
            foreach (var nd in nodes.all_nodes)
            {
                // Get the node point as vector3
                Vector3 nd_pt = nd.Value.get_point_as_vector();

                if (is_point_hit(nd_pt, screen_pt) == true)
                {
                    // Update the Geometry translation matrix
                    this.geom_translation = new Vector3(-1.0f * nd_pt.X * this.geom_scale,
                                                      -1.0f * nd_pt.Y * this.geom_scale,
                                                      -1.0f * nd_pt.Z * this.geom_scale);
                    Matrix4 g_transl = Matrix4.CreateTranslation(this.geom_translation);

                    // Construct the model matrix
                    Matrix4 modelMatrix = Matrix4.CreateScale(this.geom_scale) * g_transl;
                    modelMatrix.Transpose();

                    // Store the model matrix
                    this.geom_model_matrix = modelMatrix;

                    this.line_geom_shader.SetMatrix4("modelMatrix", modelMatrix);
                    this.surf_geom_shader.SetMatrix4("modelMatrix", modelMatrix);

                    // Set the rotation center
                    Vector3 previous_rotation_center = this.rotation_center;
                    this.rotation_center = new Vector3(nd_pt.X * this.geom_scale,
                                                       nd_pt.Y * this.geom_scale,
                                                       nd_pt.Z * this.geom_scale);

                    // Below translation prevents the model from moving to zero,zero after the new center is set
                    Vector2 transl_1 = (this.total_rotation * new Vector4((this.rotation_center - previous_rotation_center), 1.0f)).Xy*zoom_scale;
                    translate_Transform(transl_1.X, transl_1.Y);
                    pan_operation_complete();

                    // System.Windows.Forms.MessageBox.Show(nd.Key.ToString());
                }

            }
        }

        public void rotate_operation_start(PointF mousePt)
        {
            // Rotate the vertex shader
            arcball_rotate.OnMouseDown(return_screen_point(mousePt));
            this.total_rotation = arcball_rotate.GetRotationMatrix();
            this.bgrd_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.line_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.surf_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
        }

        public void rotate_operation(PointF mousePt)
        {
            // Rotate the vertex shader
            arcball_rotate.OnMouseMove(return_screen_point(mousePt));
            this.total_rotation = arcball_rotate.GetRotationMatrix();
            this.bgrd_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.line_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
            this.surf_geom_shader.SetMatrix4("rotationMatrix", this.total_rotation);
        }

        private bool is_point_hit(Vector3 node_coord, Vector2 screen_pt)
        {
            // End the rotate operation saving translate
            float hit_zone_radius = 0.02f;

            // Translate the geometry co_ordinate to the screen co_ordintate (by removing geometry scale, translation from geometric center and rotation transform)
            Vector2 node_xy_transl1 = (this.total_rotation * new Vector4(((node_coord * this.geom_scale) - this.rotation_center) , 1.0f)).Xy;

            // Remove the pan translation and Zoom scale
            Vector2 node_xy = ((node_xy_transl1 * this.zoom_scale) + this.previous_translation.Xy);

            float rad_1 = (float)(Math.Pow((node_xy - screen_pt).X, 2) + Math.Pow((node_xy - screen_pt).Y, 2));

            if (rad_1 < (hit_zone_radius * hit_zone_radius))
            {
                return true;
            }

            return false;
        }

        public Vector2 return_screen_point(PointF mounsePt)
        {
            // Set the center of rotation
            float max_size = Math.Max(this.panel_width, this.panel_height);
            float x_val = 4.0f * (mounsePt.X - (this.panel_width * 0.5f)) / max_size;
            float y_val = -1.0f * (4.0f * (mounsePt.Y - (this.panel_height * 0.5f)) / max_size);

            return new Vector2(x_val, y_val);
        }
    }
}
