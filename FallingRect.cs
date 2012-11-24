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
        public const int FALLING_RECTANGLE_DEPTH = 100; // Judgeクラスで輪っかの上端の座標が必要になる
        const int FALLING_RECTANGLE_NUMBER = 200;
        const int FALLING_RECTANGLE_SPEED = 5;
        const int HORIZONTAL_FLY_SPPED = 50;
        const int SCORING_WEIGHT_SPEED = 200;
        
        // メンバ変数
        MainWindow mainWindow;
        public int X;
        public int Y;
        public int width = FALLING_RECTANGLE_WIDTH;
        int weight = FALLING_RECTANGLE_WEIGHT;

        public enum COLOR
        {
            RED, GREEN, BLUE
        }
        public COLOR color;
        Color _c;

        public enum STATE
        {
            NORMAL, FALLING_AROUND_YOU, FLYING_RIGHT, FLYING_LEFT, SCORING, DISAPPEARED
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
                newX = oldX + rnd.Next(MainWindow.WIDTH) - (MainWindow.WIDTH / 2);
            } while (newX < 0 | // 輪っか左端の座標は0より大きい
                MainWindow.WIDTH < newX + width | // 輪っかの右端の座標は960より小さい
                Math.Abs(newX - oldX) < 200); // 前回の落ちた場所との差（絶対値）は200より小さい
            Debug.WriteLine(newX);
            X = newX;

            // 色を決める
            switch((int)rnd.Next(3)) {
                case 0:
                    color = COLOR.RED;
                    _c = Colors.Red;
                    break;
                case 1:
                    color = COLOR.GREEN;
                    _c = Colors.Green;
                    break;
                case 2:
                    color = COLOR.BLUE;
                    _c = Colors.Blue;
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
                case STATE.DISAPPEARED:
                    break;
                default:
                    Debug.WriteLine("state is default");
                    break;
            }
            this.drawBack();
            this.drawForward();
            //Debug.WriteLine("fallingRect.X: " + X + " Y: " + Y);
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
            line(mainWindow.fallingRectBack, _c, weight,
                X,
                Y,
                X + FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH);

            // 「─」（後ろ）
            line(mainWindow.fallingRectBack, _c, weight,
                X + FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH,
                X + width - FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH);

            // 「＼」
            line(mainWindow.fallingRectBack, _c, weight,
                X + width - FALLING_RECTANGLE_DEPTH,
                Y - FALLING_RECTANGLE_DEPTH,
                X + width,
                Y);
        }

        // 人物の手前の線分を描く
        public void drawForward()
        {
            mainWindow.fallingRectFore.Children.Clear();
            line(mainWindow.fallingRectFore, _c, weight, X, Y, X + width, Y);
        }
    }
}