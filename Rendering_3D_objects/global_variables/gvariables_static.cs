using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rendering_3D_objects.global_variables
{
    public static class gvariables_static
    {
        public static Color glcontrol_background_color = Color.White;

        // Garphics Control variables
        public static bool Is_panflg = false;
        public static bool Is_rotateflg = false;
        public static bool Is_cntrldown = false;
        public static Color curve_color = Color.BlueViolet;

        public static double boundary_scale = 1.0;

        public static double drawing_scale = 1.0;
        public static double drawing_tx = 0.0;
        public static double drawing_ty = 0.0;

        public static bool is_paint_shrunk_triangle = true;
        public static double triangle_shrink_factor = 0.88f;

        public static bool is_paint_wiremesh = true;
        public static bool is_paint_surf = true;


        public static Color[] standard_colors = new[] { Color.Blue, Color.BlueViolet,
            Color.Brown, Color.BurlyWood,Color.CadetBlue, Color.Chocolate,Color.Coral, Color.CornflowerBlue,
            Color.Crimson, Color.DarkBlue,Color.DarkCyan, Color.DarkGoldenrod,Color.DarkGreen, Color.DarkKhaki,
            Color.DarkMagenta, Color.DarkOliveGreen,Color.DarkOrange, Color.DarkOrchid,Color.DarkRed, Color.DarkSalmon,
            Color.DarkSeaGreen, Color.DarkSlateBlue,Color.DarkSlateGray, Color.DarkTurquoise,Color.DarkViolet, Color.DeepPink,
            Color.DeepSkyBlue, Color.DodgerBlue,Color.Firebrick, Color.ForestGreen,Color.Fuchsia, Color.Goldenrod,
            Color.Green, Color.HotPink,Color.IndianRed, Color.Indigo,Color.Khaki, Color.LightCoral,
            Color.LightSalmon, Color.LightSeaGreen,Color.LightSkyBlue, Color.LightSteelBlue,Color.LimeGreen, Color.Magenta,
            Color.Maroon, Color.MediumAquamarine,Color.MediumBlue, Color.MediumOrchid,Color.MediumPurple, Color.MediumSeaGreen,
            Color.MediumSlateBlue, Color.MediumTurquoise,Color.MediumVioletRed, Color.MidnightBlue,Color.Navy, Color.Olive,
            Color.OliveDrab, Color.Orange,Color.OrangeRed, Color.Orchid,Color.PaleVioletRed, Color.Peru,
            Color.Purple, Color.Red,Color.RosyBrown, Color.RoyalBlue,Color.SaddleBrown, Color.Salmon,
            Color.SandyBrown, Color.SeaGreen,Color.Sienna, Color.SkyBlue,Color.SlateBlue, Color.SlateGray,
            Color.SteelBlue, Color.Tan,Color.Teal, Color.Thistle,Color.Tomato, Color.Turquoise,
            Color.Violet, Color.Wheat,Color.Yellow, Color.YellowGreen
            }; // 14 x 6 = 84

        public static int RoundOff(this int i)
        {
            // Roundoff to nearest 10 (used to display zoom value)
            return ((int)Math.Round(i / 10.0)) * 10;
        }

        public static int error_tracker = 0;

        public static void Show_error_Dialog(string title, string text)
        {
            var form = new Form()
            {
                Text = title,
                Size = new Size(800, 600)
            };

            form.Controls.Add(new TextBox()
            {
                Font = new Font("Segoe UI", 12),
                Text = text,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill
            });

            form.ShowDialog();
            form.Controls.OfType<TextBox>().First().Dispose();
            form.Dispose();
        }



    }
}
