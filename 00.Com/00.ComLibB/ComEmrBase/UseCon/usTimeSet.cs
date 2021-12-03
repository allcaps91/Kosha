using System;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComEmrBase
{
    public partial class usTimeSet : UserControl
    {
        //저장
        public delegate void SetTime(string strText);
        public event SetTime rSetTime;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public usTimeSet()
        {
            InitializeComponent();
            this.Font = new System.Drawing.Font("굴림", 9, System.Drawing.FontStyle.Regular);
        }

        private void pAddEventBtn()
        {
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    if (VB.Left(control.Name, 6) == "Button")
                    {
                        ((Button)control).Click += new System.EventHandler(pBtn_Click);
                    }
                }
            }
        }

        private void pBtn_Click(object sender, EventArgs e)
        {
            string strText = "";
            strText = ((Button)sender).Text.Trim();     // +":00";
            rSetTime(strText);
            //this.Dispose(); 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
            //this.Dispose(); 
        }

        private void usTimeSet_Load(object sender, EventArgs e)
        {
            pAddEventBtn();
        }
    }
}
