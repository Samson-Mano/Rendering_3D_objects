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

namespace Rendering_3D_objects.open_tk_control
{
    public class arcball_transformation
    {

        //private Quaternion rotation;
        //private Quaternion startRotation;
        //private Vector3 startVector;
        //private bool isDragging;
        //private float radius = 1.0f;
        //private float sensitivity = 1.2f;

        //public arcball_transformation()
        //{
        //    ResetToIsometric1();
        //}

        //public void StartDrag(Vector2 mousePosition, Size viewportSize)
        //{
        //    startRotation = rotation;
        //    startVector = ScreenToSphere(mousePosition, viewportSize);
        //    isDragging = true;
        //}

        //public void UpdateDrag(Vector2 mousePosition, Size viewportSize)
        //{
        //    if (!isDragging) return;

        //    Vector3 endVector = ScreenToSphere(mousePosition, viewportSize);

        //    // Calculate rotation from start to current position
        //    Quaternion deltaRotation = CalculateRotation(startVector, endVector);

        //    // Apply rotation relative to start rotation
        //    rotation = Quaternion.Normalize(deltaRotation * startRotation);
        //}

        //public void EndDrag()
        //{
        //    isDragging = false;
        //}

        //private Vector3 ScreenToSphere(Vector2 mousePosition, Size viewportSize)
        //{
        //    // Convert to NDC coordinates
        //    Vector2 ndc = new Vector2(
        //        (2.0f * mousePosition.X / viewportSize.Width) - 1.0f,
        //        1.0f - (2.0f * mousePosition.Y / viewportSize.Height)
        //    );

        //    // Apply sensitivity
        //    ndc *= sensitivity;

        //    return ProjectToSphere(ndc, radius);
        //}

        //private Vector3 ProjectToSphere(Vector2 point, float radius)
        //{
        //    float x = point.X;
        //    float y = point.Y;
        //    float d = x * x + y * y;

        //    if (d > 1.0f)
        //    {
        //        // Project to sphere surface
        //        float s = 1.0f / (float)Math.Sqrt(d);
        //        return new Vector3(x * s, y * s, 0.0f);
        //    }
        //    else
        //    {
        //        // Inside sphere - calculate Z
        //        float z = (float)Math.Sqrt(1.0f - d);
        //        return new Vector3(x, y, z);
        //    }
        //}

        //private Quaternion CalculateRotation(Vector3 from, Vector3 to)
        //{
        //    float dot = Vector3.Dot(from, to);

        //    if (dot > 0.99999f)
        //        return Quaternion.Identity;

        //    Vector3 axis;

        //    if (dot < -0.99999f)
        //    {
        //        // Anti-parallel - find perpendicular axis
        //        axis = Vector3.Normalize(Vector3.Cross(from, Vector3.UnitX));
        //        if (axis.Length < 0.1f)
        //            axis = Vector3.Normalize(Vector3.Cross(from, Vector3.UnitY));
        //        return Quaternion.FromAxisAngle(axis, (float)Math.PI);
        //    }

        //    axis = Vector3.Normalize(Vector3.Cross(from, to));
        //    float angle = (float)Math.Acos(dot);
        //    return Quaternion.FromAxisAngle(axis, angle);
        //}

        //public Matrix4 GetRotationMatrix()
        //{
        //    return Matrix4.CreateFromQuaternion(rotation);
        //}

        //// Preset views
        //public void ResetToIsometric1()
        //{
        //    rotation = new Quaternion(0.9330127233867831f, 0.33715310920426594f,
        //                              -0.23570224007473873f, 0.5000000418651581f);
        //}

        //public void SetFrontView()
        //{
        //    rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        //}

        //public void SetTopView()
        //{
        //    rotation = new Quaternion(0.707107f, 0.0f, 0.0f, 0.707107f);
        //}

        //public void SetRightView()
        //{
        //    rotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f);
        //}





        private Vector3 startVector;
        private Vector3 endVector;
        private Quaternion rotation;

        public void OnMouseDown(Vector2 mousePt)
        {
            startVector = ProjectToSphere(mousePt, 2.0f);
        }

        public void OnMouseMove(Vector2 mousePt)
        {
            endVector = ProjectToSphere(mousePt, 2.0f);

            // Compute the rotation quaternion from the start and end vectors
            Quaternion q;
            float dot = Vector3.Dot(startVector, endVector);
            if (dot > 0.99999f)
            {
                // Vectors are almost parallel, no rotation is needed
                q = Quaternion.Identity;
            }
            else if (dot < -0.99999f)
            {
                // Vectors are almost opposite, rotate around any axis perpendicular to startVector
                Vector3 axis = Vector3.Normalize(Vector3.Cross(Vector3.UnitX, startVector));
                q = Quaternion.FromAxisAngle(axis, (float)Math.PI);
            }
            else
            {
                // Compute the rotation axis and angle
                Vector3 axis = Vector3.Normalize(Vector3.Cross(startVector, endVector));
                float angle = (float)Math.Acos(dot);
                q = Quaternion.FromAxisAngle(axis, angle);
            }

            // Update the rotation quaternion
            rotation = Quaternion.Normalize(rotation * q);

            // Set the end vector as the start vector for the next mouse move
            startVector = endVector;
        }


        private Vector3 ProjectToSphere(Vector2 point, float radius)
        {
            // Scale point to range [-1,1]
            float x = point.X / radius;
            float y = point.Y / radius;

            // Compute square of the length of the vector from this point to the center
            float d = (x * x) + (y * y);

            if (d > 1.0f)
            {
                // Point is outside the sphere, project onto the sphere surface
                float s = 1.0f / (float)Math.Sqrt(d);
                return new Vector3(x * s, y * s, 0.0f);
            }
            else
            {
                // Point is inside the sphere, compute z coordinate
                float z = (float)Math.Sqrt(1.0f - d);
                return new Vector3(x, y, z);
            }
        }


        public arcball_transformation()
        {
            // Create an Identity null rotation
            this.rotation = Quaternion.Identity;

            // Isometric 1 as default
            this.rotation = new Quaternion(0.9330127233867831f,
                0.33715310920426594f,
                -0.23570224007473873f,
                0.5000000418651581f);

            /*
            Top view: (0.707107f, 0.0f, 0.0f, 0.707107f)
            Bottom view: (0.707107f, 0.0f, 0.0f, -0.707107f)
            Front view: (0.0f, 0.0f, 0.0f, 1.0f)
            Back view: (0.0f, 1.0f, 0.0f, 0.0f)
            Right view: (0.5f, 0.5f, -0.5f, 0.5f)
            Left view: (0.5f, -0.5f, 0.5f, 0.5f)

            Isometric view 1: (0.377964f, 0.661438f, -0.225403f, 0.588156f)
            Isometric view 2: (0.661438f, 0.225403f, -0.588156f, 0.377964f)
            Isometric view 3: (0.225403f, -0.588156f, -0.377964f, 0.661438f)
            Isometric view 4: (-0.377964f, -0.661438f, 0.225403f, 0.588156f)
            Isometric view 5: (-0.661438f, -0.225403f, 0.588156f, 0.377964f)
            Isometric view 6: (-0.225403f, 0.588156f, 0.377964f, 0.661438f)
            */
        }


        public Matrix4 GetRotationMatrix()
        {
            return Matrix4.CreateFromQuaternion(rotation);
        }

        private Vector3 MapToSphere(PointF mousePt, Vector3 rotationPt)
        {
            // Translate the mouse point to the center of the arcball
            PointF centerPt = new PointF(0.5f, 0.5f);
            Vector3 mouseVec = new Vector3(mousePt.X - centerPt.X, centerPt.Y - mousePt.Y, 0.0f);

            // Compute the radius of the arcball
            float radius = 0.5f;

            // Compute the distance from the mouse point to the center of the arcball
            float dist = mouseVec.Length;

            // If the mouse point is outside the arcball, project it onto the surface of the arcball
            if (dist > radius)
            {
                mouseVec = Vector3.Normalize(mouseVec);
                return rotationPt + mouseVec * radius;
            }

            // Compute the z-coordinate of the point on the arcball using the Pythagorean theorem
            float z = (float)Math.Sqrt(radius * radius - dist * dist);

            return rotationPt + new Vector3(mouseVec.X, mouseVec.Y, z);
        }





    }
}
