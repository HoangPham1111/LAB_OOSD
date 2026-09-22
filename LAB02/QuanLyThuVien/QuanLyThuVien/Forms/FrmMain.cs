using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyThuVien.Forms
{
    public class FrmMain : Form
    {
        public FrmMain()
        {
            Text = "Quản lý thư viện";

            Size = new Size(900, 500);

            StartPosition =
                FormStartPosition.CenterScreen;

            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Label title = new Label();

            title.Text =
                "HỆ THỐNG QUẢN LÝ THƯ VIỆN";

            title.Font =
                new Font(
                    "Arial",
                    20,
                    FontStyle.Bold);

            title.AutoSize = true;

            title.Location =
                new Point(250, 50);


            Button btnDocGia =
                new Button();

            btnDocGia.Text =
                "ĐỘC GIẢ";

            btnDocGia.Size =
                new Size(180, 60);

            btnDocGia.Location =
                new Point(120, 150);

            btnDocGia.Click +=
                (s, e) =>
                {
                    new FrmDocGia().ShowDialog();
                };


            Button btnSach =
                new Button();

            btnSach.Text =
                "SÁCH";

            btnSach.Size =
                new Size(180, 60);

            btnSach.Location =
                new Point(360, 150);

            btnSach.Click +=
                (s, e) =>
                {
                    new FrmSach().ShowDialog();
                };


            Button btnDocTrucTuyen =
                new Button();

            btnDocTrucTuyen.Text =
                "ĐỌC TRỰC TUYẾN";

            btnDocTrucTuyen.Size =
                new Size(180, 60);

            btnDocTrucTuyen.Location =
                new Point(600, 150);

            btnDocTrucTuyen.Click +=
                (s, e) =>
                {
                    new FrmDocTrucTuyen()
                        .ShowDialog();
                };


            Controls.Add(title);
            Controls.Add(btnDocGia);
            Controls.Add(btnSach);
            Controls.Add(btnDocTrucTuyen);
        }
    }
}