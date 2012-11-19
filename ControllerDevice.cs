using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace p121029_KinectWatagashi
{
    /*
     * Kinectとゲームパッド両用のインターフェース
     */
    interface ControllerDevice
    {
        public Point getRightTop();
        public Point getLeftTop();

        public void start();
    }
}
