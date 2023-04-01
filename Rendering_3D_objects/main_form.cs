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
using Rendering_3D_objects.drawing_object_store;
using Rendering_3D_objects.Text_to_mesh;

namespace Rendering_3D_objects
{
    public partial class main_form : Form
    {
        public geometry_store geom { get; private set; }

        public main_form()
        {
            InitializeComponent();
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

            if (openfiledialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string text = System.IO.File.ReadAllText(openfiledialog1.FileName);

                    txt_to_mesh txt = new txt_to_mesh(text);

                    if (txt.is_read_valid == true)
                    {
                        // Read successful add to mesh data
                        geom = new geometry_store(txt.nodes, txt.elines, txt.etris, txt.equads);
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

    }
}
