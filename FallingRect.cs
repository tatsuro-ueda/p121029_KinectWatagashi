using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace p121029_KinectWatagashi
{
    class FallingRect
    {
        // 定数
        const int FALLING_RECTANGLE_WIDTH = 250;
        const int FALLING_RECTANGLE_WEIGHT = 20;
        const int FALLING_RECTANGLE_DEPTH = 40;
        const int FALLING_RECTANGLE_NUMBER = 10;
        
        // メンバ変数
        MainWindow mainWindow;
        public int X;
        public int Y;
        public int width = FALLING_RECTANGLE_WIDTH;
        public int weight = FALLING_RECTANGLE_WEIGHT;
        Color clr;
        String clrName;
        int hitCounter = 0;
        bool isFlyingRight;
        bool isFallingAroundYou = false;
        int getCounter = 0;
        private int p;

        // コンストラクタ
        public FallingRect(MainWindow m, int oldX)
        {
            mainWindow = m;
            Y = 0;

            // xをランダムに決める
            int newX;
                Random rnd = new Random();
            do {
                newX = oldX + rnd.Next(200) - 100;
            } while ( newX < 0 | 440 < newX | Math.Abs(newX - oldX) < 50);
            X = newX;

            // 色を決める
            switch((int)rnd.Next(3)) {
              case 0:
                clr = Colors.Red;
                clrName = "red";
                break;
              case 1:
                clr = Colors.Green;
                clrName = "green";
                break;
              case 2:
              case 3:
                clr = Colors.Blue;
                clrName = "blue";
                break;
            }
        }

        public void drawForward() {
            mainWindow.fallingRectFore.Children.Clear();
            mainWindow.fallingRectFore.Children.Add(new Line()
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = weight,
                X1 = X, 
                Y1 = Y,
                X2 = X + width, 
                Y2 = Y
            });
            Debug.WriteLine("drawForward executed");
        }
  
        //void drawScoringVE(bool isHandUp) {
        //    if(isArduino){
        //        arduino.digitalWrite(RPIN,Arduino.LOW);
        //        arduino.digitalWrite(GPIN,Arduino.LOW);
        //        arduino.digitalWrite(BPIN,Arduino.LOW);
        //    }
        //    if ( getCounter < (18 / SLOWNESS)) {
        //        weight += SLOWNESS * 50;
        //        getCounter++;
        //        y = height;
        //    } else if (isHandUp && getCounter < (36 / SLOWNESS)) {
        //        colorBefore = clr;
        //        clr = color(255, 255, 255);
        //        weight += SLOWNESS * 50;
        //        getCounter++;
        //        y = height;
        //    } else if (isHandUp && getCounter < (54 / SLOWNESS)) {
        //        clr = colorBefore;
        //        weight += SLOWNESS * 50;
        //        getCounter++;
        //        y = height;
        //    } else {
        //        isFallingAroundYou = false;
        //        getCounter = 0;
        //        y = height + 1;
        //        println("You get " + clrName + " !");
        //    }
        //}

        internal void update()
        {
            this.drawForward();
            Y += 1;
        }
    }
}