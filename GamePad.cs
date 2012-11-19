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

        Color clr;
        int weight;

        public GamePad(MainWindow m)
        {
            mainWindow = m;
            int x = -100;
            int y = 0;
            width = 200;

            clr = Colors.Black;
            weight = 20;

            //DrawRect(x, y);
        }

        // 線分を描く
        public void line(Canvas canvas, Color color, int weight, int x1, int y1, int x2, int y2)
        {
            canvas.Children.Add(new Line()
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = weight,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            });
        }

        void DrawRect(int x, int y)
        {
            mainWindow.canvasGamePad.Children.Clear();
            line(mainWindow.canvasGamePad, clr, weight,
                x,
                y,
                x + width,
                y);

            mainWindow.canvasGamePad.Children.Clear();
            line(mainWindow.canvasGamePad, clr, weight,
                x,
                y,
                x,
                y - 500);

            mainWindow.canvasGamePad.Children.Clear();
            line(mainWindow.canvasGamePad, clr, weight,
                x + width,
                y,
                x + width,
                y - 500);
        }

        public Point getRightTop()
        {
            throw new NotImplementedException();
        }

        public Point getLeftTop()
        {
            throw new NotImplementedException();
        }

        public void start()
        {
        }
    }
}
