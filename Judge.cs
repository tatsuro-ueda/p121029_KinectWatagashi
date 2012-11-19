using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace p121029_KinectWatagashi
{
    /*
     * 衝突判定
     */
    class Judge
    {
        // 定数
        const int BOTTOM_END = -350;

        public Judge()
        {
        }

        public void doJudge(Point leftTop, Point rightTop, FallingRect fallingRect)
        {
            // 輪っかのy軸は人物の高さより低く、かつ下端より高い
            if (leftTop.Y <= fallingRect.X & fallingRect.Y >= BOTTOM_END)
            {
                // 輪っかが人物の右側に引っかかっている
                // 
                //        --------------------
                //     ++++++++++
                //     ++++++++++
                //     ++++++++++
                // 
                if (leftTop.X <= fallingRect.X & fallingRect.X <= rightTop.X)
                {
                    // 輪っかの内側から体が左に押した場合
                    // 
                    //         ++++++++++
                    //        --------------------
                    //         ++++++++++
                    //         ++++++++++
                    // 
                    //              ↓
                    // 
                    //     ++++++++++
                    //        --------------------
                    //     ++++++++++
                    //     ++++++++++
                    // 
                    if (fallingRect.state == FallingRect.STATE.FALLING_AROUND_YOU)
                    {
                        // 左へ飛ぶ
                        fallingRect.state = FallingRect.STATE.FLYING_LEFT;
                        Debug.WriteLine("Falling Rect is flying left");
                    }
                    // 輪っかの外側から体が右に押した場合
                    // 
                    //     ++++++++++
                    //     ++++++++++ --------------------
                    //     ++++++++++
                    //     ++++++++++
                    // 
                    //              ↓
                    // 
                    //         ++++++++++
                    //               --------------------
                    //         ++++++++++
                    //         ++++++++++
                    // 
                    else
                    {
                        // 右へ飛ぶ
                        fallingRect.state = FallingRect.STATE.FLYING_RIGHT;
                        Debug.WriteLine("Falling Rect is flying right");
                    }
                }
                // 輪っかが人物の左側に引っかかっている
                // 
                //  --------------------
                //                   ++++++++++
                //                   ++++++++++
                //                   ++++++++++
                // 
                else if (fallingRect.X <= leftTop.X & leftTop.X <= fallingRect.X + fallingRect.width)
                {
                    // 輪っかの内側から体が左に押した場合
                    // 
                    //        ++++++++++
                    //       --------------------
                    //        ++++++++++
                    //        ++++++++++
                    // 
                    //              ↓
                    // 
                    //     ++++++++++
                    //       --------------------
                    //     ++++++++++
                    //     ++++++++++
                    // 
                    if (fallingRect.state == FallingRect.STATE.FALLING_AROUND_YOU)
                    {
                        // 左へ飛ぶ
                        fallingRect.state = FallingRect.STATE.FLYING_LEFT;
                        Debug.WriteLine("Falling Rect is flying left");
                    }
                    // 輪っかの外側から体が右に押した場合
                    // 
                    // ++++++++++
                    // ++++++++++  --------------------
                    // ++++++++++
                    // ++++++++++
                    // 
                    //              ↓
                    // 
                    //     ++++++++++
                    //       　　　--------------------
                    //     ++++++++++
                    //     ++++++++++
                    // 
                    else
                    {
                        // 右へ飛ぶ
                        fallingRect.state = FallingRect.STATE.FLYING_RIGHT;
                        Debug.WriteLine("Falling Rect is flying right");
                    }
                }
            }
            // 輪っかの中に入っていて、輪っかが下端に達した場合
            else if ( fallingRect.state == FallingRect.STATE.FALLING_AROUND_YOU & fallingRect.Y < BOTTOM_END)
            {
                fallingRect.state = FallingRect.STATE.SCORING;
            }
        }
    }
}
