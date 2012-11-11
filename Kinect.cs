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

        readonly int Bgr32BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;

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
            kinect.DepthStream.Enable();
            kinect.SkeletonStream.Enable();

            kinect.AllFramesReady +=new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);

            //kinect.ColorFrameReady +=
            //    new EventHandler<ColorImageFrameReadyEventArgs>(
            //        kinect_ColorFrameReady);

            //// スケルトンを有効にして、フレーム更新イベントを登録する
            //kinect.SkeletonFrameReady +=
            //    new EventHandler<SkeletonFrameReadyEventArgs>(
            //        kinect_SkeletonFrameReady);

            kinect.Start();
        }

        public void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            try
            {
                // Kinectのインスタンスを取得する
                KinectSensor kinect = sender as KinectSensor;
                if (kinect == null)
                {
                    return;
                }

                // 背景をマスクした画像を描画する
                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                {
                    using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
                    {
                        if ((colorFrame != null) && (depthFrame != null))
                        {
                            mainWindow.imageRgb.Source = BitmapSource.Create(colorFrame.Width,
                                colorFrame.Height, 96, 96, PixelFormats.Bgr32, null,
                                BackgroundMask(kinect, colorFrame, depthFrame),
                                colorFrame.Width * colorFrame.BytesPerPixel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private byte[] BackgroundMask(KinectSensor kinect,
            ColorImageFrame colorFrame, DepthImageFrame depthFrame)
        {
            ColorImageStream colorStream = kinect.ColorStream;
            DepthImageStream depthStream = kinect.DepthStream;

            // RGBカメラのピクセルごとのデータを取得する
            byte[] colorPixel = new byte[colorFrame.PixelDataLength];
            colorFrame.CopyPixelDataTo(colorPixel);

            // 距離カメラのピクセルごとのデータを取得する
            short[] depthPixel = new short[depthFrame.PixelDataLength];
            depthFrame.CopyPixelDataTo(depthPixel);

            // 距離カメラの座標に対応するRGBカメラの座標を取得する（座標合わせ）
            ColorImagePoint[] colorPoint =
                new ColorImagePoint[depthFrame.PixelDataLength];
            kinect.MapDepthFrameToColorFrame(depthStream.Format, depthPixel,
                colorStream.Format, colorPoint);

            // 出力バッファ
            byte[] outputColor = new byte[colorPixel.Length];
            for (int i = 0; i < outputColor.Length; i += Bgr32BytesPerPixel)
            {
                outputColor[i] = 255;
                outputColor[i + 1] = 255;
                outputColor[i + 2] = 255;
            }

            for (int index = 0; index < depthPixel.Length; index++)
            {
                // プレイヤーを取得する
                int player = depthPixel[index] & DepthImageFrame.PlayerIndexBitmask;

                // 変換した結果がフレームサイズを超えることがあるため、小さい方を使う
                int x = Math.Min(colorPoint[index].X, colorStream.FrameWidth - 1);
                int y = Math.Min(colorPoint[index].Y, colorStream.FrameHeight - 1);
                int colorIndex = ((y * depthFrame.Width) + x) * Bgr32BytesPerPixel;

                // プレイヤーを検出した座標だけ、RGBカメラの画像を使う
                if (player != 0)
                {
                    outputColor[colorIndex] = colorPixel[colorIndex];
                    outputColor[colorIndex + 1] = colorPixel[colorIndex + 1];
                    outputColor[colorIndex + 2] = colorPixel[colorIndex + 2];
                }
            }
            return outputColor;
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
