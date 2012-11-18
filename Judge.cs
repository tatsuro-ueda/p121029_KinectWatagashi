using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            if ( leftTop.Y <= fallingRect.X )
            {
                // 輪っかが人物の右側に引っかかっている
                // 
                //        --------------------
                //     ++++++++++
                //     ++++++++++
                //     ++++++++++
                // 
                if ( leftTop.X <= fallingRect.X & fallingRect.X <= rightTop.X )
                {
                    // 内側から体が左に押した場合
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
                    if ( fallingRect.state == FallingRect.STATE.FALLING_AROUND_YOU)
                    {
                        // 左へ飛ぶ
                        fallingRect.state = FallingRect.STATE.FLYING_LEFT;
                    }
                    // 内側から体が左に押した場合
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
                    }
                }
                // 輪っかが人物の左側に引っかかっている
                // 
                //  --------------------
                //                   ++++++++++
                //                   ++++++++++
                //                   ++++++++++
                // 
                else if ( fallingRect.X <= leftTop.X & leftTop.X <= fallingRect.X + fallingRect.width )
                {

                

      } else if (
      // fallがhRctの左側に引っかかっている
      hRct.x <= x + this.width & x + this.width <= hRct.x + hRct.width
      ) 
      {
        if (isFallingAroundYou) { // 内側から体が押した場合
          isFlyingRight = true;
          isFallingAroundYou = false;
        }
        else {isFlyingRight = false;} // 外側から体が押した場合
        hitCounter = 1; // 横へ飛び始める
      } else if (
      // 輪の中にうまく入った場合
      x <= hRct.x & hRct.x + hRct.width <= x + this.width
      ) 

        }
    }
}
