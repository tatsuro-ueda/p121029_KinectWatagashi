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
    /*
     * 上から落ちてくる輪っか
     */
    public class FallingRect
    {
        // 定数
        const int FALLING_RECTANGLE_WIDTH = 400;
        const int FALLING_RECTANGLE_WEIGHT = 20;
        const int FALLING_RECTANGLE_DEPTH = 100;
        const int FALLING_RECTANGLE_NUMBER = 200;
        const int FALLING_RECTANGLE_SPEED = 1;
        const int HORIZONTAL_FLY_SPPED = 10;
        const int SCORING_WEIGHT_SPEED = 5;
        
        // メンバ変数
        MainWindow mainWindow;
        public int X;
        public int Y;
        public int width = FALLING_RECTANGLE_WIDTH;
        int weight = FALLING_RECTANGLE_WEIGHT;
        Color clr;
        String clrName;
        int hitCounter = 0;
        bool isFlyingRight;
        bool isFallingAroundYou = false;
        int getCounter = 0;

        public enum STATE
        {
            NORMAL, FALLING_AROUND_YOU, FLYING_RIGHT, FLYING_LEFT, SCORING
        }
        public STATE state;

        // コンストラクタ
        public FallingRect(MainWindow m, int oldX)
        {
            mainWindow = m;
            Y = 0;
            state = STATE.NORMAL;

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

        public void update()
        {
            switch (state)
            {
                case STATE.NORMAL:
                case STATE.FALLING_AROUND_YOU:
                    Y += FALLING_RECTANGLE_SPEED;
                    break;
                case STATE.FLYING_LEFT:
                    X -= HORIZONTAL_FLY_SPPED;
                    break;
                case STATE.FLYING_RIGHT:
                    X += HORIZONTAL_FLY_SPPED;
                    break;
                case STATE.SCORING:
                    weight += SCORING_WEIGHT_SPEED;
                    break;
                default:
                    Debug.WriteLine("state is default");
                    break;
            }
            this.drawBack();
            this.drawForward();
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
    }
}