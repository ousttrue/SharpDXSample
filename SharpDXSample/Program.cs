﻿using System;
using System.Windows.Forms;


namespace SharpDXSample
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Console.WriteLine(SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects());
        }
    }
}
