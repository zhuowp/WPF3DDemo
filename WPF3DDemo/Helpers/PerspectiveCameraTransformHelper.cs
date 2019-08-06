using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace WPF3DDemo.Helpers
{
    public static class PerspectiveCameraTransformHelper
    {
        public static void VerticalRotateAroundCenter(this PerspectiveCamera camera, double rotateAngle, Point3D center)
        {
            //旋转中心位置向量
            Vector3D rotateCenterPosition = new Vector3D(center.X, center.Y, center.Z);

            //摄像机位置向量
            Vector3D cameraPosition = new Vector3D(camera.Position.X, camera.Position.Y, camera.Position.Z);

            //旋转轴
            Vector3D rotateAxis = Vector3D.CrossProduct(cameraPosition - rotateCenterPosition, camera.UpDirection);

            //角度旋转
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.Rotation = new AxisAngleRotation3D(rotateAxis, rotateAngle);

            //变换摄像机位置
            Matrix3D matrix = rotateTransform3D.Value;
            Point3D newCameraPosition = matrix.Transform(camera.Position);
            camera.Position = newCameraPosition;
            camera.LookDirection = new Vector3D(-(newCameraPosition.X - center.X), -(newCameraPosition.Y - center.Y), -(newCameraPosition.Z - center.Z));

            Vector3D newUpDirection = Vector3D.CrossProduct(camera.LookDirection, rotateAxis);
            newUpDirection.Normalize();
            camera.UpDirection = newUpDirection;
        }

        public static void HorizontalRotateAroundCenter(this PerspectiveCamera camera, double rotateAngle, Point3D center)
        {
            //摄像机位置向量
            Vector3D cameraPosition = new Vector3D(camera.Position.X, camera.Position.Y, camera.Position.Z);

            //旋转轴
            Vector3D rotateAxis = camera.UpDirection;

            //角度旋转
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.Rotation = new AxisAngleRotation3D(rotateAxis, -rotateAngle);

            //变换摄像机位置
            Matrix3D matrix = rotateTransform3D.Value;
            Point3D newCameraPosition = matrix.Transform(camera.Position);
            camera.Position = newCameraPosition;
            camera.LookDirection = new Vector3D(-newCameraPosition.X, -newCameraPosition.Y, -newCameraPosition.Z);
        }

        public static void ZoomIn(PerspectiveCamera camera, double scaleFactor)
        {
            Point3D oldPosition = camera.Position;
            Vector3D currentLookDirection = camera.LookDirection;
            currentLookDirection.Normalize();

            Vector3D positionChangeVector = currentLookDirection * scaleFactor;
            Point3D newPosition = oldPosition + positionChangeVector;

            Point3DAnimation positionAnimation = new Point3DAnimation();
            positionAnimation.BeginTime = new TimeSpan(0, 0, 0);
            positionAnimation.Duration = TimeSpan.FromMilliseconds(100);
            positionAnimation.From = oldPosition;
            positionAnimation.To = newPosition;
            positionAnimation.Completed += (s, e) =>
            {
                Point3D position = camera.Position;
                camera.BeginAnimation(ProjectionCamera.PositionProperty, null);
                camera.Position = position;
            };

            camera.BeginAnimation(ProjectionCamera.PositionProperty, positionAnimation, HandoffBehavior.Compose);
        }

        public static void VerticalRotateInSitu(this PerspectiveCamera camera, double rotateAngle)
        {
            //旋转轴
            Vector3D rotateAxis = Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);

            //角度旋转
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.Rotation = new AxisAngleRotation3D(rotateAxis, rotateAngle);
            Matrix3D matrix = rotateTransform3D.Value;

            //更新摄像机拍摄方向
            Point3D newCameraPosition = matrix.Transform(new Point3D(camera.LookDirection.X, camera.LookDirection.Y, camera.LookDirection.Z));
            camera.LookDirection = new Vector3D(newCameraPosition.X, newCameraPosition.Y, newCameraPosition.Z);

            //更新摄像机向上向量
            Vector3D newUpDirection = Vector3D.CrossProduct(rotateAxis, camera.LookDirection);
            newUpDirection.Normalize();
            camera.UpDirection = newUpDirection;
        }

        public static void HorizontalRotateInSitu(this PerspectiveCamera camera, double rotateAngle)
        {
            Vector3D oldLookDirection = camera.LookDirection;
            Vector3D oldUpDirection = camera.UpDirection;

            //角度旋转
            Vector3D rotateAxis = new Vector3D(0, 1, 0);
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.Rotation = new AxisAngleRotation3D(rotateAxis, rotateAngle);
            Matrix3D matrix = rotateTransform3D.Value;

            //更新摄像机拍摄方向
            Point3D newCameraPosition = matrix.Transform(new Point3D(oldLookDirection.X, oldLookDirection.Y, oldLookDirection.Z));
            camera.LookDirection = new Vector3D(newCameraPosition.X, newCameraPosition.Y, newCameraPosition.Z);

            //更新摄像机向上向量
            Point3D newUpDirection1 = matrix.Transform(new Point3D(oldUpDirection.X, oldUpDirection.Y, oldUpDirection.Z));
            camera.UpDirection = new Vector3D(newUpDirection1.X, newUpDirection1.Y, newUpDirection1.Z);
        }

        public static void ZoomInInSitu(this PerspectiveCamera camera, double factor, double minFieldOfView, double maxFieldOfView)
        {
            if (camera.FieldOfView + factor < minFieldOfView)
            {
                camera.FieldOfView = minFieldOfView;
            }
            else if (camera.FieldOfView + factor > maxFieldOfView)
            {
                camera.FieldOfView = maxFieldOfView;
            }

            camera.FieldOfView += factor;
        }
    }
}
