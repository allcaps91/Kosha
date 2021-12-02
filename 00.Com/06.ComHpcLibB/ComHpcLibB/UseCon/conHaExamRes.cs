using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComHpcLibB.UseCon
{
    public partial class conHaExamRes :UserControl
    {
        public delegate void rMouseWheelEvnt(object sender, MouseEventArgs e);
        public event rMouseWheelEvnt eMouseWheelEvnt;

        public conHaExamRes()
        {
            InitializeComponent();
            SetEvent();
        }

        private void SetEvent()
        {
            this.MouseWheel += new MouseEventHandler(eMouseWheel);
            this.txtRes.MouseWheel += new MouseEventHandler(eMouseWheel);
            this.txtRes.TextChanged += new EventHandler(eTxtChanged);
        }

        /// <summary>
        /// 텍스트 박스 클라이언트 크기 설정하기
        /// </summary>
        private void eTxtChanged(object sender, EventArgs e)
        {
            int MARGIN_X = 0;
            int MARGIN_Y = 100;
            int nSUM_Y = 0;

            Size size = TextRenderer.MeasureText(this.txtRes.Text, this.txtRes.Font);

            if (size.Width > 389)
            {
                nSUM_Y = (int)Math.Truncate(size.Width / 389.0) * 25;
                MARGIN_Y += nSUM_Y;
            }

            this.ClientSize = new Size(size.Width + MARGIN_X, size.Height + MARGIN_Y);
        }

        private void eMouseWheel(object sender, MouseEventArgs e)
        {
            eMouseWheelEvnt(sender, e);
        }
    }
}
