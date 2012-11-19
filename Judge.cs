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
    class Judge
    {
        public Judge()
        {
        }

        public void doJudge(Point leftTop, Point rightTop, FallingRect fallingRect)
        {
            // 輪っかのy軸は人物の高さより低く、かつ
            if (leftTop.Y <= fallingRect.X)
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
        }
    }
}
