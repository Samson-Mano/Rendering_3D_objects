namespace Rendering_3D_objects
{
    partial class main_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main_form));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Zoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.originToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glControl_main_panel = new OpenTK.GLControl();
            this.toolStripStatusLabel_rotation = new System.Windows.Forms.ToolStripStatusLabel();
            this.viewAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isometric1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isometric2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savedViewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Zoom,
            this.toolStripStatusLabel_rotation});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Zoom
            // 
            this.toolStripStatusLabel_Zoom.Name = "toolStripStatusLabel_Zoom";
            this.toolStripStatusLabel_Zoom.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel_Zoom.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.axisToolStripMenuItem,
            this.originToolStripMenuItem,
            this.viewAxisToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // axisToolStripMenuItem
            // 
            this.axisToolStripMenuItem.Name = "axisToolStripMenuItem";
            this.axisToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.axisToolStripMenuItem.Text = "Axis";
            // 
            // originToolStripMenuItem
            // 
            this.originToolStripMenuItem.Name = "originToolStripMenuItem";
            this.originToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.originToolStripMenuItem.Text = "Origin";
            // 
            // glControl_main_panel
            // 
            this.glControl_main_panel.BackColor = System.Drawing.Color.Black;
            this.glControl_main_panel.Location = new System.Drawing.Point(60, 92);
            this.glControl_main_panel.Margin = new System.Windows.Forms.Padding(4, 19, 4, 19);
            this.glControl_main_panel.Name = "glControl_main_panel";
            this.glControl_main_panel.Size = new System.Drawing.Size(363, 263);
            this.glControl_main_panel.TabIndex = 2;
            this.glControl_main_panel.VSync = false;
            // 
            // toolStripStatusLabel_rotation
            // 
            this.toolStripStatusLabel_rotation.Name = "toolStripStatusLabel_rotation";
            this.toolStripStatusLabel_rotation.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel_rotation.Text = "toolStripStatusLabel1";
            // 
            // viewAxisToolStripMenuItem
            // 
            this.viewAxisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frontToolStripMenuItem,
            this.backToolStripMenuItem,
            this.leftToolStripMenuItem,
            this.rightToolStripMenuItem,
            this.topToolStripMenuItem,
            this.bottomToolStripMenuItem,
            this.isometric1ToolStripMenuItem,
            this.isometric2ToolStripMenuItem,
            this.savedViewsToolStripMenuItem});
            this.viewAxisToolStripMenuItem.Name = "viewAxisToolStripMenuItem";
            this.viewAxisToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewAxisToolStripMenuItem.Text = "View Axis";
            // 
            // frontToolStripMenuItem
            // 
            this.frontToolStripMenuItem.Name = "frontToolStripMenuItem";
            this.frontToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.frontToolStripMenuItem.Text = "Front";
            // 
            // backToolStripMenuItem
            // 
            this.backToolStripMenuItem.Name = "backToolStripMenuItem";
            this.backToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.backToolStripMenuItem.Text = "Back";
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.leftToolStripMenuItem.Text = "Left";
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rightToolStripMenuItem.Text = "Right";
            // 
            // topToolStripMenuItem
            // 
            this.topToolStripMenuItem.Name = "topToolStripMenuItem";
            this.topToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.topToolStripMenuItem.Text = "Top";
            // 
            // bottomToolStripMenuItem
            // 
            this.bottomToolStripMenuItem.Name = "bottomToolStripMenuItem";
            this.bottomToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.bottomToolStripMenuItem.Text = "Bottom";
            // 
            // isometric1ToolStripMenuItem
            // 
            this.isometric1ToolStripMenuItem.Name = "isometric1ToolStripMenuItem";
            this.isometric1ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.isometric1ToolStripMenuItem.Text = "Isometric 1";
            // 
            // isometric2ToolStripMenuItem
            // 
            this.isometric2ToolStripMenuItem.Name = "isometric2ToolStripMenuItem";
            this.isometric2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.isometric2ToolStripMenuItem.Text = "Isometric 2";
            // 
            // savedViewsToolStripMenuItem
            // 
            this.savedViewsToolStripMenuItem.Name = "savedViewsToolStripMenuItem";
            this.savedViewsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.savedViewsToolStripMenuItem.Text = "Saved Views";
            // 
            // main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 84F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.glControl_main_panel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Cambria Math", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 19, 4, 19);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "main_form";
            this.Text = "3D Rendering Application using OpenGL 3.3 in C# Windows Forms";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Zoom;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem originToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_rotation;
        private OpenTK.GLControl glControl_main_panel;
        private System.Windows.Forms.ToolStripMenuItem viewAxisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bottomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isometric1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isometric2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savedViewsToolStripMenuItem;
    }
}