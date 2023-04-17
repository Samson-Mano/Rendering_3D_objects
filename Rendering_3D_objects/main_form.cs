using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// OpenTK Library
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

// This app references
using Rendering_3D_objects.global_variables;
using Rendering_3D_objects.drawing_object_store;
using Rendering_3D_objects.Text_to_mesh;
using Rendering_3D_objects.open_tk_control;

namespace Rendering_3D_objects
{
    public partial class main_form : Form
    {
        public geometry_store geom { get; private set; }

        // Variables to control openTK GLControl
        // glControl wrapper class
        private opentk_main_control g_control;

        // Cursor point on the GLControl
        private PointF click_pt;

        public main_form()
        {
            InitializeComponent();
        }

        private void main_form_Load(object sender, EventArgs e)
        {
            // Initialize the geometry object
            geom = new geometry_store();

            // Load the wrapper class to control the openTK Glcontrol
            g_control = new opentk_main_control();

            // Fill the glcontrol panel
            glControl_main_panel.BorderStyle = BorderStyle.Fixed3D;
            glControl_main_panel.Dock = DockStyle.Fill;
        }

        #region "File menu items"
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // read the mesh files as text
            // Example folder has the format
            OpenFileDialog openfiledialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\"
            openfiledialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openfiledialog1.FilterIndex = 2;
            openfiledialog1.RestoreDirectory = true;

            if (openfiledialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string text = File.ReadAllText(openfiledialog1.FileName);

                    txt_to_mesh txt = new txt_to_mesh(text);

                    if (txt.is_read_valid == true)
                    {
                        // Read successful add to mesh data
                        geom = new geometry_store(txt.nodes, txt.elines, txt.etris, txt.equads);
                        geom.set_openTK_objects();

                        // Update the geometry shader
                        g_control.update_geom_shader(geom.geom_bounds_max, geom.geom_bounds_min);

                        // Refresh the painting area
                        glControl_main_panel.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot read file from disk. Original error: " + ex.Message.ToString());
                    openfiledialog1.Dispose();
                    return;
                }
            }
            openfiledialog1.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exit the form
            this.Close();
        }
        #endregion

        #region "View menu items"
        private void axisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show axis option
        }

        private void originToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show orgin option
        }

        private void frontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show front view
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show back view
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show left view
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show right view
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show top view
        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show bottom view
        }

        private void isometric1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show isometric view 1
        }

        private void isometric2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show isometric view 2
        }
        #endregion

        #region "Control Main Panel Events"
        private void glControl_main_panel_SizeChanged(object sender, EventArgs e)
        {
            // GL Panel size changed
            if (g_control == null)
                return;
            // glControl size changed
            // Update the size of the drawing area
            g_control.update_drawing_area_size(glControl_main_panel.Width,
                glControl_main_panel.Height);
            g_control.update_geom_shader(geom.geom_bounds_max, geom.geom_bounds_min);


            toolStripStatusLabel_Zoom.Text = "Zoom: " + (gvariables_static.RoundOff((int)(1.0f * 100))).ToString() + "%";

            // Refresh the painting area
            glControl_main_panel.Refresh();
        }

        private void glControl_main_panel_Paint(object sender, PaintEventArgs e)
        {
            // Paint the drawing area (glControl_main)
            // Tell OpenGL to use MyGLControl
            glControl_main_panel.MakeCurrent();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Paint the background color
            g_control.set_opengl_shader("background");
            g_control.paint_opengl_control_background();



            g_control.set_opengl_shader("surfacegeometry");
            geom.paint_surf_objects();

            // Display the model using OpenGL
            GL.LineWidth(2.73f);
            g_control.set_opengl_shader("linegeometry");
            // geom.set_openTK_objects();
            geom.paint_line_objects();

            //// Display the label
            //g_control.set_opengl_shader(1);
            //g_control.update_shader_uniform_var(1, Color.Green);
            //geom_obj.paint_label();

            // OpenTK windows are what's known as "double-buffered". In essence, the window manages two buffers.
            // One is rendered to while the other is currently displayed by the window.
            // This avoids screen tearing, a visual artifact that can happen if the buffer is modified while being displayed.
            // After drawing, call this function to swap the buffers. If you don't, it won't display what you've rendered.
            glControl_main_panel.SwapBuffers();
        }

        private void glControl_main_panel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // User press hold Cntrl key and mouse wheel
            if (gvariables_static.Is_cntrldown == true)
            {
                // Zoom operation commences
                glControl_main_panel.Focus();

                g_control.intelli_zoom_operation(e.Delta, new PointF(e.X, e.Y));

                // Update the zoom value in tool strip status bar
                toolStripStatusLabel_Zoom.Text = "Zoom: " + (gvariables_static.RoundOff((int)(100f * g_control.zoom_scale))).ToString() + "%";
                // Refresh the painting area
                glControl_main_panel.Refresh();
            }
        }


        private void glControl_main_panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (gvariables_static.Is_cntrldown == true)
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Pan operation starts if Ctrl + Mouse Right Button is pressed
                    // save the current cursor point
                    click_pt = new PointF(e.X, e.Y);

                    // Set the variable to indicate pan operation begins
                    gvariables_static.Is_panflg = true;

                    glControl_main_panel.Refresh();
                }
                else if (e.Button == MouseButtons.Left)
                {
                    // Rotate operation starts if Ctrl + Mouse Left Button is pressed
                    // save the current cursor point
                    click_pt = new PointF(e.X, e.Y);
                    g_control.rotate_operation_start(new PointF(e.X, e.Y));

                    // Set the variable to indicate pan operation begins
                    gvariables_static.Is_rotateflg = true;

                    glControl_main_panel.Refresh();
                }
            }
        }

        private void glControl_main_panel_MouseEnter(object sender, EventArgs e)
        {
            // set the focus to enable zoom/ pan & zoom to fit
            glControl_main_panel.Focus();
        }

        private void glControl_main_panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get the mouse coordinates
            int mouseX = e.X;
            int mouseY = e.Y;

            // Get the OpenGL viewport dimensions
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            // Convert the mouse coordinates to OpenGL coordinates
            float glX = viewport[0];
            float glY = viewport[1];

            toolStripStatusLabel_location.Text = $"(X: {glX}, Y: {glY})";


            if (gvariables_static.Is_panflg == true)
            {
                // Pan operation in progress
                g_control.pan_operation(new PointF(e.X - click_pt.X, e.Y - click_pt.Y));

                // Refresh the painting area
                glControl_main_panel.Refresh();
            }
            else if (gvariables_static.Is_rotateflg == true)
            {
                // Rotate operation in progress
                g_control.rotate_operation(new PointF(e.X, e.Y));

                // Refresh the painting area
                glControl_main_panel.Refresh();
            }
        }

        private void glControl_main_panel_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (gvariables_static.Is_cntrldown == true && e.Button == MouseButtons.Left)
            {
                // Set the Rotation center
                g_control.rotate_set_center(new PointF(e.X, e.Y), geom.nodes);
            }
        }

        private void glControl_main_panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Pan operation ends once the Mouse Right Button is released
            if (gvariables_static.Is_panflg == true)
            {
                gvariables_static.Is_panflg = false;

                // Pan operation ends (save the translate transformation)
                g_control.pan_operation_complete();

                // Refresh the painting area
                glControl_main_panel.Refresh();
            }
            else if (gvariables_static.Is_rotateflg == true)
            {
                gvariables_static.Is_rotateflg = false;

                // Rotate operation ends

                //Refresh the painting area
                glControl_main_panel.Refresh();
            }
        }

        private void glControl_main_panel_KeyDown(object sender, KeyEventArgs e)
        {
            // Key Down
            // Keydown event
            if (e.Control == true)
            {
                // User press and hold Cntrl key
                gvariables_static.Is_cntrldown = true;

                if (e.KeyCode == Keys.F)
                {
                    // (Ctrl + F) --> Zoom to fit
                    g_control.update_geom_shader(geom.geom_bounds_max, geom.geom_bounds_min);
                    // g_control.zoom_to_fit(ref glControl_main_panel);

                    toolStripStatusLabel_Zoom.Text = "Zoom: " + (gvariables_static.RoundOff((int)(1.0f * 100))).ToString() + "%";
                    toolStripStatusLabel_Zoom.Invalidate();

                    glControl_main_panel.Refresh();
                    glControl_main_panel.Invalidate();
                }
            }
        }

        private void glControl_main_panel_KeyUp(object sender, KeyEventArgs e)
        {
            // Key Up
            gvariables_static.Is_cntrldown = false;
        }
        #endregion

    }
}
