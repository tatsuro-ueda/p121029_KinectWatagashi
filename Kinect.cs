using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace p121029_KinectWatagashi
{
    class Kinect : ControllerDevice
    {
        MainWindow mainWindow;
        KinectSensor kinect;
        Point jointRightShoulder;
        Point jointLeftShoulder;
        CoordinateMapper coodinateMapper;

        public Kinect(MainWindow m)
        {
            mainWindow = m;

            if (KinectSensor.KinectSensors.Count == 0)
            {
                throw new Exception("Kinectを接続してください");
            }

            kinect = KinectSensor.KinectSensors[0];
            coodinateMapper = kinect.CoordinateMapper;
        }

        public void start()
        {
            kinect.ColorStream.Enable();
            kinect.ColorFrameReady +=
                new EventHandler<ColorImageFrameReadyEventArgs>(
                    kinect_ColorFrameReady);

            // スケルトンを有効にして、フレーム更新イベントを登録する
            kinect.SkeletonStream.Enable();
            kinect.SkeletonFrameReady +=
                new EventHandler<SkeletonFrameReadyEventArgs>(
                    kinect_SkeletonFrameReady);

            kinect.Start();
        }

        public void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            try
            {
                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                {
                    if (colorFrame != null)
                    {
                        // RGBカメラのフレームデータを取得する
                        byte[] colorPixel = new byte[colorFrame.PixelDataLength];
                        colorFrame.CopyPixelDataTo(colorPixel);

                        // ピクセルデータをビットマップに変換する

                        mainWindow.imageRgb.Source = BitmapSource.Create(colorFrame.Width,
                            colorFrame.Height, 96, 96, PixelFormats.Bgr32, null,
                            colorPixel, colorFrame.Width * colorFrame.BytesPerPixel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            try
            {
                using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (skeletonFrame != null)
                    {
                        DrawSkeleton(kinect, skeletonFrame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DrawSkeleton(KinectSensor kinect, SkeletonFrame skeletonFrame)
        {
            // スケルトンのデータを取得する
            Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
            skeletonFrame.CopySkeletonDataTo(skeletons);

            mainWindow.canvasSkeleton.Children.Clear();

            // トラッキングされているスケルトンのジョイントを描画する
            foreach (Skeleton skeleton in skeletons)
            {

                // スケルトンがトラッキング状態（Defaultモード）の場合は、ジョイントを描画する
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {

                    // ジョイントを描画する
                    foreach (Joint joint in skeleton.Joints)
                    {

                        // ジョイントがトラッキングされていなければ次へ
                        if (joint.TrackingState == JointTrackingState.NotTracked)
                        {
                            continue;
                        }

                        // 右肩と左肩の座標を取得する
                        if (joint.JointType == JointType.ShoulderRight)
                        {
                            jointRightShoulder = new Point(joint.Position.X, joint.Position.Y);
                        }
                        else if (joint.JointType == JointType.ShoulderLeft)
                        {
                            jointLeftShoulder = new Point(joint.Position.X, joint.Position.Y);
                        }

                        // ジョイントの座標を描く
                        DrawEllipse(kinect, joint.Position);
                    }
                }
            }
        }
        private void DrawEllipse(KinectSensor kinect, SkeletonPoint position)
        {
            const int R = 5;

            // スケルトンの座標を、RGBカメラの座標に変換する
            ColorImagePoint point = 
                coodinateMapper.MapSkeletonPointToColorPoint(position, kinect.ColorStream.Format);

            // 座標を画面のサイズに変換する
            point.X = (int)ScaleTo(point.X, kinect.ColorStream.FrameWidth,
                mainWindow.canvasSkeleton.Width);
            point.Y = (int)ScaleTo(point.Y, kinect.ColorStream.FrameHeight,
                mainWindow.canvasSkeleton.Height);

            // 円を描く
            mainWindow.canvasSkeleton.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.Red),
                Margin = new Thickness(point.X - R, point.Y - R, 0, 0),
                Width = R * 2,
                Height = R * 2,
            });
        }

        double ScaleTo(double value, double source, double dest)
        {
            return (value * dest) / source;
        }

        public Point getRightTop()
        {
            return jointRightShoulder;
        }

        public Point getLeftTop()
        {
            return jointLeftShoulder;
        }
    }
}
