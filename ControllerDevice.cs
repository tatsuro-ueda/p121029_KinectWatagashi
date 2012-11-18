using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace p121029_KinectWatagashi
{
    interface ControllerDevice
    {
        Point getRightTop();
        Point getLeftTop();

        void start();
    }
}
