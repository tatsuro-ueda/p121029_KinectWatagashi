using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Controls;

namespace p121029_KinectWatagashi
{
    class FallingRect
    {
        // 定数
        const int FALLING_RECTANGLE_WIDTH = 400;
        const int FALLING_RECTANGLE_WEIGHT = 20;
        const int FALLING_RECTANGLE_DEPTH = 100;
        const int FALLING_RECTANGLE_NUMBER = 200;
        const int FALLING_RECTANGLE_SPEED = 1;
        
        // メンバ変数
        MainWindow mainWindow;
        int X;
        int Y;
        int width = FALLING_RECTANGLE_WIDTH;
        int weight = FALLING_RECTANGLE_WEIGHT;
        Color clr;
        String clrName;
        int hitCounter = 0;
        bool isFlyingRight;
        bool isFallingAroundYou = false;
        int getCounter = 0;

        // コンストラクタ
        public FallingRect(MainWindow m, int oldX)
        {
            mainWindow = m;
            Y = 0;

            // xをランダムに決める
            int newX;
            Random rnd = new Random();
            do
            {
                newX = oldX + rnd.Next(1600) - 800;
            } while (newX + width / 2 < -500 | // 枠の中心の座標は-500より大きい
                500 < newX + width / 2 | // 枠の中心の座標は500より小さい
                Math.Abs(newX - oldX) < 200); // 前回の落ちた場所との差（絶対値）は200より小さい
            Debug.WriteLine(newX);
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
                clr = Colors.Blue;
                clrName = "blue";
                break;
            }
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

        // 人物の後ろ側の線分を描く
        public void drawBack()
        {
            mainWindow.fallingRectBack.Children.Clear();

            // 「／」
            line(mainWindow.fallingRectBack, clr, weight,
                X,
                Y,
                X + FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH);

            // 「─」（後ろ）
            line(mainWindow.fallingRectBack, clr, weight,
                X + FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH,
                X + width - FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH);

            // 「＼」
            line(mainWindow.fallingRectBack, clr, weight,
                X + width - FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH,
                X + width,
                Y);
        }

        // 人物の手前の線分を描く
        public void drawForward()
        {
            mainWindow.fallingRectFore.Children.Clear();
            line(mainWindow.fallingRectFore, clr, weight, X, Y, X + width, Y);
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
            this.drawBack();
            this.drawForward();
            Y += FALLING_RECTANGLE_SPEED;
        }

    }
}