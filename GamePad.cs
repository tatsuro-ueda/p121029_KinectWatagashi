using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace p121029_KinectWatagashi
{
    class GamePad : ControllerDevice
    {
        MainWindow mainWindow;
        int x, y, width;
        Point jointRightShoulder;
        Point jointLeftShoulder;

        public GamePad(MainWindow m)
        {
            mainWindow = m;
            int x = -100;
            int y = 0;
            int width = 200;

            DrawRect(x, y);
        }

        void DrawRect(int x, int y)
        {
            // Create a Rectangle
            Rectangle blueRectangle = new Rectangle();
            blueRectangle.Height = 100;
            blueRectangle.Width = 200;

            // Create a blue and a black Brush
            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.Blue;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            // Set Rectangle's width and color
            blueRectangle.StrokeThickness = 4;
            blueRectangle.Stroke = blackBrush;
            // Fill rectangle with blue color
            blueRectangle.Fill = blueBrush;

            // Add Rectangle to the Grid.
            mainWindow.canvasGamePad.Children.Add(blueRectangle);
        }

        public Point getRightTop()
        {
            throw new NotImplementedException();
        }

        public Point getLeftTop()
        {
            throw new NotImplementedException();
        }
    }
}
