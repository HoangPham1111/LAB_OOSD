using System;
using System.Windows.Forms;
using QLNS.DongA.UI;

namespace QLNS.DongA
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
