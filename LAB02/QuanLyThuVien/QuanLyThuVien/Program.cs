using System;
using System.Windows.Forms;
using QuanLyThuVien.Forms;

namespace QuanLyThuVien
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FrmMain());
        }
    }
}