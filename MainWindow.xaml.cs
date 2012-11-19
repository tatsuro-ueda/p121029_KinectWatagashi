﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Diagnostics;
using System.Windows.Threading;

namespace p121029_KinectWatagashi
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        enum CONTROLLER_DEVICE
        {
            KINECT, 
            GAMEPAD
        }

        // コントローラーの設定
        //CONTROLLER_DEVICE controllerDevice = CONTROLLER_DEVICE.GAMEPAD;
        CONTROLLER_DEVICE controllerDevice = CONTROLLER_DEVICE.KINECT;

        enum PHASE
        {
            INITIALIZING, WAITING, FINDING_USER, CALIBRATING,
            BEFORE_PLAY, PLAYING, AFTER_PLAY
        }
        PHASE phase;

        ControllerDevice c;
        DispatcherTimer dispatcherTimer;
        FallingRect[] fallingRects;
        Judge j;

        public MainWindow()
        {

            /*
             * 初期化関連
             */

            phase = PHASE.INITIALIZING;

            switch(controllerDevice)
            {
                case CONTROLLER_DEVICE.KINECT:
                    c = new Kinect(this);
                    break;
                case CONTROLLER_DEVICE.GAMEPAD:
                    // ゲームパッドの初期化
                    c = new GamePad(this);
                    break;
            }

            j = new Judge();
            InitializeComponent();
            c.start();

            /*
             * テスト
             */

            FallingRect fall = new FallingRect(this, -200);
            fallingRects = new FallingRect[] { fall };

            phase = PHASE.PLAYING;

            /*
             * タイマーにイベントを登録して0.05秒ごとにイベントを実行する
             */

            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(500000);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            switch (phase)
            {
                case PHASE.PLAYING:
                    foreach (FallingRect f in fallingRects)
                    {
                        j.doJudge(c.getLeftTop, c.getRightTop, f);
                        f.update();
                    }
                    break;
            }
        }

        private void Window_Closing(object sender,
            System.ComponentModel.CancelEventArgs e)
        {
        }            
    }
}
