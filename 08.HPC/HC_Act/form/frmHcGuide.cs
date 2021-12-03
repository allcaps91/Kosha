using ComBase;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcGuide.cs
/// Description     : 안내화면
/// Author          : 이상훈
/// Create Date     : 2019-08-28
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm안내화면.frm(Frm안내화면)" />

namespace HC_Act
{
    public partial class frmHcGuide : Form
    {
        int FnTime1;
        int FnTime2;

        public frmHcGuide()
        {
            InitializeComponent();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();
            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            timer1.Enabled = true;
            timer2.Enabled = false;

            FnTime1 = 0;
            FnTime2 = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            FnTime1 += 1;

            if (FnTime1 == 3)
            {
                pictureBox2.Dock = DockStyle.Fill;
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                FnTime1 = 0;
                timer1.Enabled = false;
                timer2.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            FnTime2 += 1;

            if (FnTime2 == 3)
            {
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
                FnTime2 = 0;
                timer1.Enabled = true;
                timer2.Enabled = false;
            }
        }
    }
}
