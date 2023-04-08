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
using Rendering_3D_objects.drawing_object_store;
using Rendering_3D_objects.drawing_object_store.drawing_objects;
using Rendering_3D_objects.open_tk_control.open_tk_buffer;
using Rendering_3D_objects.drawing_object_store.drawing_objects.object_store;


namespace Rendering_3D_objects.open_tk_control.open_tk_bgdraw
{
    public class draw_axis_store
    {
        // Define X,Y,Z and center point as triangles
        Vector3 axis_origin = new Vector3(0, 0, 0);
        float scale_x = 1.0f;

        // Origin
        public nodes_store axis_nodes;
        public tri_elements axis_tris;

        public draw_axis_store()
        {
            // Initialize all the pre-set values
            Color node_clr = Color.DarkGray;
            Color x_axis_clr = Color.Red;
            Color y_axis_clr = Color.Blue;
            Color z_axis_clr = Color.Green;

            // Origin
            axis_nodes = new nodes_store();
            axis_nodes.add_point(1, 0.00000, 0.00000, -1.00000, node_clr);//1
            axis_nodes.add_point(2, -0.70711, 0.00000, -0.70711, node_clr);//2
            axis_nodes.add_point(3, -1.00000, 0.00000, 0.00000, node_clr);//3
            axis_nodes.add_point(4, -0.70711, 0.00000, 0.70711, node_clr);//4
            axis_nodes.add_point(5, 0.00000, 0.00000, 1.00000, node_clr);//5
            axis_nodes.add_point(6, 0.00000, 0.70711, 0.70711, node_clr);//6
            axis_nodes.add_point(7, 0.00000, 1.00000, 0.00000, node_clr);//7
            axis_nodes.add_point(8, 0.00000, 0.70711, -0.70711, node_clr);//8
            axis_nodes.add_point(9, -0.65956, 0.67021, -0.34028, node_clr);//9
            axis_nodes.add_point(10, -0.66880, 0.65834, 0.34538, node_clr);//10
            axis_nodes.add_point(11, 0.70711, 0.00000, -0.70711, node_clr);//11
            axis_nodes.add_point(12, 1.00000, 0.00000, 0.00000, node_clr);//12
            axis_nodes.add_point(13, 0.70711, 0.00000, 0.70711, node_clr);//13
            axis_nodes.add_point(14, 0.65956, 0.67021, -0.34028, node_clr);//14
            axis_nodes.add_point(15, 0.66880, 0.65834, 0.34538, node_clr);//15
            axis_nodes.add_point(16, 0.00000, -0.70711, 0.70711, node_clr);//16
            axis_nodes.add_point(17, 0.00000, -1.00000, 0.00000, node_clr);//17
            axis_nodes.add_point(18, 0.00000, -0.70711, -0.70711, node_clr);//18
            axis_nodes.add_point(19, -0.65956, -0.67021, -0.34028, node_clr);//19
            axis_nodes.add_point(20, -0.66880, -0.65834, 0.34538, node_clr);//20
            axis_nodes.add_point(21, 0.65956, -0.67021, -0.34028, node_clr);//21
            axis_nodes.add_point(22, 0.66880, -0.65834, 0.34538, node_clr);//22

            // X axis
            axis_nodes.add_point(23, 5.90000, -0.22844, -0.32835, x_axis_clr);//23
            axis_nodes.add_point(24, 10.89994, -0.34639, 0.19999, x_axis_clr);//24
            axis_nodes.add_point(25, 10.89995, -0.34632, -0.19990, x_axis_clr);//25
            axis_nodes.add_point(26, 10.89999, -0.68711, -0.39641, x_axis_clr);//26
            axis_nodes.add_point(27, 10.89999, -0.69245, 0.40035, x_axis_clr);//27
            axis_nodes.add_point(28, 13.90000, 0.00000, 0.00000, x_axis_clr);//28
            axis_nodes.add_point(29, 10.89999, 0.69245, 0.40035, x_axis_clr);//29
            axis_nodes.add_point(30, 10.89999, 0.68711, -0.39641, x_axis_clr);//30
            axis_nodes.add_point(31, 10.90002, 0.00000, -0.80006, x_axis_clr);//31
            axis_nodes.add_point(32, 10.89990, -0.00023, 0.79322, x_axis_clr);//32
            axis_nodes.add_point(33, 5.90000, 0.22844, -0.32835, x_axis_clr);//33
            axis_nodes.add_point(34, 5.89998, 0.22850, 0.32831, x_axis_clr);//34
            axis_nodes.add_point(35, 5.90000, 0.40000, -0.00009, x_axis_clr);//35
            axis_nodes.add_point(36, 5.90000, 0.00000, -0.40000, x_axis_clr);//36
            axis_nodes.add_point(37, 0.90000, 0.00000, -0.40001, x_axis_clr);//37
            axis_nodes.add_point(38, 0.90000, 0.34502, -0.19939, x_axis_clr);//38
            axis_nodes.add_point(39, 0.90000, 0.34522, 0.19931, x_axis_clr);//39
            axis_nodes.add_point(40, 0.90000, 0.00001, 0.40001, x_axis_clr);//40
            axis_nodes.add_point(41, 5.89995, 0.00000, 0.40000, x_axis_clr);//41
            axis_nodes.add_point(42, 10.89990, 0.00000, 0.39993, x_axis_clr);//42
            axis_nodes.add_point(43, 10.90000, 0.34641, 0.20000, x_axis_clr);//43
            axis_nodes.add_point(44, 10.90000, 0.34641, -0.20000, x_axis_clr);//44
            axis_nodes.add_point(45, 10.90000, 0.00000, -0.40000, x_axis_clr);//45
            axis_nodes.add_point(46, 5.89998, -0.22850, 0.32831, x_axis_clr);//46
            axis_nodes.add_point(47, 5.90000, -0.40000, -0.00009, x_axis_clr);//47
            axis_nodes.add_point(48, 0.90000, -0.34502, -0.19939, x_axis_clr);//48
            axis_nodes.add_point(49, 0.90000, -0.34522, 0.19931, x_axis_clr);//49

            // Y axis
            axis_nodes.add_point(50, 0.22844, 5.90000, -0.32835, y_axis_clr);//50
            axis_nodes.add_point(51, 0.34639, 10.89994, 0.19999, y_axis_clr);//51
            axis_nodes.add_point(52, 0.34632, 10.89995, -0.19990, y_axis_clr);//52
            axis_nodes.add_point(53, 0.68711, 10.89999, -0.39641, y_axis_clr);//53
            axis_nodes.add_point(54, 0.69245, 10.89999, 0.40035, y_axis_clr);//54
            axis_nodes.add_point(55, 0.00000, 13.90000, 0.00000, y_axis_clr);//55
            axis_nodes.add_point(56, -0.69245, 10.89999, 0.40035, y_axis_clr);//56
            axis_nodes.add_point(57, -0.68711, 10.89999, -0.39641, y_axis_clr);//57
            axis_nodes.add_point(58, 0.00000, 10.90002, -0.80006, y_axis_clr);//58
            axis_nodes.add_point(59, 0.00023, 10.89990, 0.79322, y_axis_clr);//59
            axis_nodes.add_point(60, -0.22844, 5.90000, -0.32835, y_axis_clr);//60
            axis_nodes.add_point(61, -0.22850, 5.89998, 0.32831, y_axis_clr);//61
            axis_nodes.add_point(62, -0.40000, 5.90000, -0.00009, y_axis_clr);//62
            axis_nodes.add_point(63, 0.00000, 5.90000, -0.40000, y_axis_clr);//63
            axis_nodes.add_point(64, 0.00000, 0.90000, -0.40001, y_axis_clr);//64
            axis_nodes.add_point(65, -0.34502, 0.90000, -0.19939, y_axis_clr);//65
            axis_nodes.add_point(66, -0.34522, 0.90000, 0.19931, y_axis_clr);//66
            axis_nodes.add_point(67, -0.00001, 0.90000, 0.40001, y_axis_clr);//67
            axis_nodes.add_point(68, 0.00000, 5.89995, 0.40000, y_axis_clr);//68
            axis_nodes.add_point(69, 0.00000, 10.89990, 0.39993, y_axis_clr);//69
            axis_nodes.add_point(70, -0.34641, 10.90000, 0.20000, y_axis_clr);//70
            axis_nodes.add_point(71, -0.34641, 10.90000, -0.20000, y_axis_clr);//71
            axis_nodes.add_point(72, 0.00000, 10.90000, -0.40000, y_axis_clr);//72
            axis_nodes.add_point(73, 0.22850, 5.89998, 0.32831, y_axis_clr);//73
            axis_nodes.add_point(74, 0.40000, 5.90000, -0.00009, y_axis_clr);//74
            axis_nodes.add_point(75, 0.34502, 0.90000, -0.19939, y_axis_clr);//75
            axis_nodes.add_point(76, 0.34522, 0.90000, 0.19931, y_axis_clr);//76

            // Z axis
            axis_nodes.add_point(77, 0.22844, 0.32835, 5.90000, z_axis_clr);//77
            axis_nodes.add_point(78, 0.34639, -0.19999, 10.89994, z_axis_clr);//78
            axis_nodes.add_point(79, 0.34632, 0.19990, 10.89995, z_axis_clr);//79
            axis_nodes.add_point(80, 0.68711, 0.39641, 10.89999, z_axis_clr);//80
            axis_nodes.add_point(81, 0.69245, -0.40035, 10.89999, z_axis_clr);//81
            axis_nodes.add_point(82, 0.00000, 0.00000, 13.90000, z_axis_clr);//82
            axis_nodes.add_point(83, -0.69245, -0.40035, 10.89999, z_axis_clr);//83
            axis_nodes.add_point(84, -0.68711, 0.39641, 10.89999, z_axis_clr);//84
            axis_nodes.add_point(85, 0.00000, 0.80006, 10.90002, z_axis_clr);//85
            axis_nodes.add_point(86, 0.00023, -0.79322, 10.89990, z_axis_clr);//86
            axis_nodes.add_point(87, -0.22844, 0.32835, 5.90000, z_axis_clr);//87
            axis_nodes.add_point(88, -0.22850, -0.32831, 5.89998, z_axis_clr);//88
            axis_nodes.add_point(89, -0.40000, 0.00009, 5.90000, z_axis_clr);//89
            axis_nodes.add_point(90, 0.00000, 0.40000, 5.90000, z_axis_clr);//90
            axis_nodes.add_point(91, 0.00000, 0.40001, 0.90000, z_axis_clr);//91
            axis_nodes.add_point(92, -0.34502, 0.19939, 0.90000, z_axis_clr);//92
            axis_nodes.add_point(93, -0.34522, -0.19931, 0.90000, z_axis_clr);//93
            axis_nodes.add_point(94, -0.00001, -0.40001, 0.90000, z_axis_clr);//94
            axis_nodes.add_point(95, 0.00000, -0.40000, 5.89995, z_axis_clr);//95
            axis_nodes.add_point(96, 0.00000, -0.39993, 10.89990, z_axis_clr);//96
            axis_nodes.add_point(97, -0.34641, -0.20000, 10.90000, z_axis_clr);//97
            axis_nodes.add_point(98, -0.34641, 0.20000, 10.90000, z_axis_clr);//98
            axis_nodes.add_point(99, 0.00000, 0.40000, 10.90000, z_axis_clr);//99
            axis_nodes.add_point(100, 0.22850, -0.32831, 5.89998, z_axis_clr);//100
            axis_nodes.add_point(101, 0.40000, 0.00009, 5.90000, z_axis_clr);//101
            axis_nodes.add_point(102, 0.34502, 0.19939, 0.90000, z_axis_clr);//102
            axis_nodes.add_point(103, 0.34522, -0.19931, 0.90000, z_axis_clr);//103

            // Orgin Elements
            axis_tris = new tri_elements();
            axis_tris.add_triangle(1, 4, 5, 6, axis_nodes);//1
            axis_tris.add_triangle(2, 8, 1, 2, axis_nodes);//2
            axis_tris.add_triangle(3, 7, 8, 9, axis_nodes);//3
            axis_tris.add_triangle(4, 9, 8, 2, axis_nodes);//4
            axis_tris.add_triangle(5, 9, 2, 3, axis_nodes);//5
            axis_tris.add_triangle(6, 9, 3, 10, axis_nodes);//6
            axis_tris.add_triangle(7, 10, 3, 4, axis_nodes);//7
            axis_tris.add_triangle(8, 10, 4, 6, axis_nodes);//8
            axis_tris.add_triangle(9, 6, 7, 10, axis_nodes);//9
            axis_tris.add_triangle(10, 10, 7, 9, axis_nodes);//10
            axis_tris.add_triangle(11, 13, 6, 5, axis_nodes);//11
            axis_tris.add_triangle(12, 8, 11, 1, axis_nodes);//12
            axis_tris.add_triangle(13, 7, 14, 8, axis_nodes);//13
            axis_tris.add_triangle(14, 14, 11, 8, axis_nodes);//14
            axis_tris.add_triangle(15, 14, 12, 11, axis_nodes);//15
            axis_tris.add_triangle(16, 14, 15, 12, axis_nodes);//16
            axis_tris.add_triangle(17, 15, 13, 12, axis_nodes);//17
            axis_tris.add_triangle(18, 15, 6, 13, axis_nodes);//18
            axis_tris.add_triangle(19, 6, 15, 7, axis_nodes);//19
            axis_tris.add_triangle(20, 15, 14, 7, axis_nodes);//20
            axis_tris.add_triangle(21, 4, 16, 5, axis_nodes);//21
            axis_tris.add_triangle(22, 18, 2, 1, axis_nodes);//22
            axis_tris.add_triangle(23, 17, 19, 18, axis_nodes);//23
            axis_tris.add_triangle(24, 19, 2, 18, axis_nodes);//24
            axis_tris.add_triangle(25, 19, 3, 2, axis_nodes);//25
            axis_tris.add_triangle(26, 19, 20, 3, axis_nodes);//26
            axis_tris.add_triangle(27, 20, 4, 3, axis_nodes);//27
            axis_tris.add_triangle(28, 20, 16, 4, axis_nodes);//28
            axis_tris.add_triangle(29, 16, 20, 17, axis_nodes);//29
            axis_tris.add_triangle(30, 20, 19, 17, axis_nodes);//30
            axis_tris.add_triangle(31, 13, 5, 16, axis_nodes);//31
            axis_tris.add_triangle(32, 18, 1, 11, axis_nodes);//32
            axis_tris.add_triangle(33, 17, 18, 21, axis_nodes);//33
            axis_tris.add_triangle(34, 21, 18, 11, axis_nodes);//34
            axis_tris.add_triangle(35, 21, 11, 12, axis_nodes);//35
            axis_tris.add_triangle(36, 21, 12, 22, axis_nodes);//36
            axis_tris.add_triangle(37, 22, 12, 13, axis_nodes);//37
            axis_tris.add_triangle(38, 22, 13, 16, axis_nodes);//38
            axis_tris.add_triangle(39, 16, 17, 22, axis_nodes);//39
            axis_tris.add_triangle(40, 22, 17, 21, axis_nodes);//40

            // X axis Elements
            axis_tris.add_triangle(41, 23, 37, 48, axis_nodes);//41
            axis_tris.add_triangle(42, 42, 46, 41, axis_nodes);//42
            axis_tris.add_triangle(43, 24, 47, 46, axis_nodes);//43
            axis_tris.add_triangle(44, 46, 40, 41, axis_nodes);//44
            axis_tris.add_triangle(45, 26, 31, 25, axis_nodes);//45
            axis_tris.add_triangle(46, 27, 26, 24, axis_nodes);//46
            axis_tris.add_triangle(47, 24, 42, 27, axis_nodes);//47
            axis_tris.add_triangle(48, 27, 42, 32, axis_nodes);//48
            axis_tris.add_triangle(49, 24, 26, 25, axis_nodes);//49
            axis_tris.add_triangle(50, 25, 31, 45, axis_nodes);//50
            axis_tris.add_triangle(51, 28, 31, 26, axis_nodes);//51
            axis_tris.add_triangle(52, 28, 26, 27, axis_nodes);//52
            axis_tris.add_triangle(53, 28, 27, 32, axis_nodes);//53
            axis_tris.add_triangle(54, 28, 32, 29, axis_nodes);//54
            axis_tris.add_triangle(55, 28, 29, 30, axis_nodes);//55
            axis_tris.add_triangle(56, 28, 30, 31, axis_nodes);//56
            axis_tris.add_triangle(57, 44, 45, 31, axis_nodes);//57
            axis_tris.add_triangle(58, 43, 44, 30, axis_nodes);//58
            axis_tris.add_triangle(59, 29, 32, 42, axis_nodes);//59
            axis_tris.add_triangle(60, 43, 29, 42, axis_nodes);//60
            axis_tris.add_triangle(61, 29, 43, 30, axis_nodes);//61
            axis_tris.add_triangle(62, 30, 44, 31, axis_nodes);//62
            axis_tris.add_triangle(63, 34, 41, 40, axis_nodes);//63
            axis_tris.add_triangle(64, 43, 34, 35, axis_nodes);//64
            axis_tris.add_triangle(65, 42, 41, 34, axis_nodes);//65
            axis_tris.add_triangle(66, 33, 38, 37, axis_nodes);//66
            axis_tris.add_triangle(67, 35, 39, 38, axis_nodes);//67
            axis_tris.add_triangle(68, 33, 36, 45, axis_nodes);//68
            axis_tris.add_triangle(69, 35, 33, 44, axis_nodes);//69
            axis_tris.add_triangle(70, 44, 33, 45, axis_nodes);//70
            axis_tris.add_triangle(71, 33, 35, 38, axis_nodes);//71
            axis_tris.add_triangle(72, 36, 33, 37, axis_nodes);//72
            axis_tris.add_triangle(73, 43, 42, 34, axis_nodes);//73
            axis_tris.add_triangle(74, 44, 43, 35, axis_nodes);//74
            axis_tris.add_triangle(75, 35, 34, 39, axis_nodes);//75
            axis_tris.add_triangle(76, 39, 34, 40, axis_nodes);//76
            axis_tris.add_triangle(77, 47, 48, 49, axis_nodes);//77
            axis_tris.add_triangle(78, 23, 45, 36, axis_nodes);//78
            axis_tris.add_triangle(79, 47, 25, 23, axis_nodes);//79
            axis_tris.add_triangle(80, 25, 45, 23, axis_nodes);//80
            axis_tris.add_triangle(81, 23, 48, 47, axis_nodes);//81
            axis_tris.add_triangle(82, 36, 37, 23, axis_nodes);//82
            axis_tris.add_triangle(83, 24, 46, 42, axis_nodes);//83
            axis_tris.add_triangle(84, 25, 47, 24, axis_nodes);//84
            axis_tris.add_triangle(85, 47, 49, 46, axis_nodes);//85
            axis_tris.add_triangle(86, 49, 40, 46, axis_nodes);//86

            // Y axis Elements
            axis_tris.add_triangle(87, 50, 64, 75, axis_nodes);//87
            axis_tris.add_triangle(88, 69, 73, 68, axis_nodes);//88
            axis_tris.add_triangle(89, 51, 74, 73, axis_nodes);//89
            axis_tris.add_triangle(90, 73, 67, 68, axis_nodes);//90
            axis_tris.add_triangle(91, 53, 58, 52, axis_nodes);//91
            axis_tris.add_triangle(92, 54, 53, 51, axis_nodes);//92
            axis_tris.add_triangle(93, 51, 69, 54, axis_nodes);//93
            axis_tris.add_triangle(94, 54, 69, 59, axis_nodes);//94
            axis_tris.add_triangle(95, 51, 53, 52, axis_nodes);//95
            axis_tris.add_triangle(96, 52, 58, 72, axis_nodes);//96
            axis_tris.add_triangle(97, 55, 58, 53, axis_nodes);//97
            axis_tris.add_triangle(98, 55, 53, 54, axis_nodes);//98
            axis_tris.add_triangle(99, 55, 54, 59, axis_nodes);//99
            axis_tris.add_triangle(100, 55, 59, 56, axis_nodes);//100
            axis_tris.add_triangle(101, 55, 56, 57, axis_nodes);//101
            axis_tris.add_triangle(102, 55, 57, 58, axis_nodes);//102
            axis_tris.add_triangle(103, 71, 72, 58, axis_nodes);//103
            axis_tris.add_triangle(104, 70, 71, 57, axis_nodes);//104
            axis_tris.add_triangle(105, 56, 59, 69, axis_nodes);//105
            axis_tris.add_triangle(106, 70, 56, 69, axis_nodes);//106
            axis_tris.add_triangle(107, 56, 70, 57, axis_nodes);//107
            axis_tris.add_triangle(108, 57, 71, 58, axis_nodes);//108
            axis_tris.add_triangle(109, 61, 68, 67, axis_nodes);//109
            axis_tris.add_triangle(110, 70, 61, 62, axis_nodes);//110
            axis_tris.add_triangle(111, 69, 68, 61, axis_nodes);//111
            axis_tris.add_triangle(112, 60, 65, 64, axis_nodes);//112
            axis_tris.add_triangle(113, 62, 66, 65, axis_nodes);//113
            axis_tris.add_triangle(114, 60, 63, 72, axis_nodes);//114
            axis_tris.add_triangle(115, 62, 60, 71, axis_nodes);//115
            axis_tris.add_triangle(116, 71, 60, 72, axis_nodes);//116
            axis_tris.add_triangle(117, 60, 62, 65, axis_nodes);//117
            axis_tris.add_triangle(118, 63, 60, 64, axis_nodes);//118
            axis_tris.add_triangle(119, 70, 69, 61, axis_nodes);//119
            axis_tris.add_triangle(120, 71, 70, 62, axis_nodes);//120
            axis_tris.add_triangle(121, 62, 61, 66, axis_nodes);//121
            axis_tris.add_triangle(122, 66, 61, 67, axis_nodes);//122
            axis_tris.add_triangle(123, 74, 75, 76, axis_nodes);//123
            axis_tris.add_triangle(124, 50, 72, 63, axis_nodes);//124
            axis_tris.add_triangle(125, 74, 52, 50, axis_nodes);//125
            axis_tris.add_triangle(126, 52, 72, 50, axis_nodes);//126
            axis_tris.add_triangle(127, 50, 75, 74, axis_nodes);//127
            axis_tris.add_triangle(128, 63, 64, 50, axis_nodes);//128
            axis_tris.add_triangle(129, 51, 73, 69, axis_nodes);//129
            axis_tris.add_triangle(130, 52, 74, 51, axis_nodes);//130
            axis_tris.add_triangle(131, 74, 76, 73, axis_nodes);//131
            axis_tris.add_triangle(132, 76, 67, 73, axis_nodes);//132

            // Z axis Elements
            axis_tris.add_triangle(133, 77, 91, 102, axis_nodes);//133
            axis_tris.add_triangle(134, 96, 100, 95, axis_nodes);//134
            axis_tris.add_triangle(135, 78, 101, 100, axis_nodes);//135
            axis_tris.add_triangle(136, 100, 94, 95, axis_nodes);//136
            axis_tris.add_triangle(137, 80, 85, 79, axis_nodes);//137
            axis_tris.add_triangle(138, 81, 80, 78, axis_nodes);//138
            axis_tris.add_triangle(139, 78, 96, 81, axis_nodes);//139
            axis_tris.add_triangle(140, 81, 96, 86, axis_nodes);//140
            axis_tris.add_triangle(141, 78, 80, 79, axis_nodes);//141
            axis_tris.add_triangle(142, 79, 85, 99, axis_nodes);//142
            axis_tris.add_triangle(143, 82, 85, 80, axis_nodes);//143
            axis_tris.add_triangle(144, 82, 80, 81, axis_nodes);//144
            axis_tris.add_triangle(145, 82, 81, 86, axis_nodes);//145
            axis_tris.add_triangle(146, 82, 86, 83, axis_nodes);//146
            axis_tris.add_triangle(147, 82, 83, 84, axis_nodes);//147
            axis_tris.add_triangle(148, 82, 84, 85, axis_nodes);//148
            axis_tris.add_triangle(149, 98, 99, 85, axis_nodes);//149
            axis_tris.add_triangle(150, 97, 98, 84, axis_nodes);//150
            axis_tris.add_triangle(151, 83, 86, 96, axis_nodes);//151
            axis_tris.add_triangle(152, 97, 83, 96, axis_nodes);//152
            axis_tris.add_triangle(153, 83, 97, 84, axis_nodes);//153
            axis_tris.add_triangle(154, 84, 98, 85, axis_nodes);//154
            axis_tris.add_triangle(155, 88, 95, 94, axis_nodes);//155
            axis_tris.add_triangle(156, 97, 88, 89, axis_nodes);//156
            axis_tris.add_triangle(157, 96, 95, 88, axis_nodes);//157
            axis_tris.add_triangle(158, 87, 92, 91, axis_nodes);//158
            axis_tris.add_triangle(159, 89, 93, 92, axis_nodes);//159
            axis_tris.add_triangle(160, 87, 90, 99, axis_nodes);//160
            axis_tris.add_triangle(161, 89, 87, 98, axis_nodes);//161
            axis_tris.add_triangle(162, 98, 87, 99, axis_nodes);//162
            axis_tris.add_triangle(163, 87, 89, 92, axis_nodes);//163
            axis_tris.add_triangle(164, 90, 87, 91, axis_nodes);//164
            axis_tris.add_triangle(165, 97, 96, 88, axis_nodes);//165
            axis_tris.add_triangle(166, 98, 97, 89, axis_nodes);//166
            axis_tris.add_triangle(167, 89, 88, 93, axis_nodes);//167
            axis_tris.add_triangle(168, 93, 88, 94, axis_nodes);//168
            axis_tris.add_triangle(169, 101, 102, 103, axis_nodes);//169
            axis_tris.add_triangle(170, 77, 99, 90, axis_nodes);//170
            axis_tris.add_triangle(171, 101, 79, 77, axis_nodes);//171
            axis_tris.add_triangle(172, 79, 99, 77, axis_nodes);//172
            axis_tris.add_triangle(173, 77, 102, 101, axis_nodes);//173
            axis_tris.add_triangle(174, 90, 91, 77, axis_nodes);//174
            axis_tris.add_triangle(175, 78, 100, 96, axis_nodes);//175
            axis_tris.add_triangle(176, 79, 101, 78, axis_nodes);//176
            axis_tris.add_triangle(177, 101, 103, 100, axis_nodes);//177
            axis_tris.add_triangle(178, 103, 94, 100, axis_nodes);//178

        }

        public void set_openTK_objects()
        {
            // Set the OpenTK objects
            axis_nodes.set_openTK_objects();
            axis_tris.set_openTK_objects();
        }

        public void paint_axis()
        {
                        // Paint the axis
            axis_tris.paint_all_triangles();
        }
    }
}
