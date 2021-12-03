using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public partial class frmTablet_a : Form
    {
        public frmTablet_a()
        {
            InitializeComponent();
            Display_Secondary_Monitor();
            SetControl();
            SetEvent();
        }

        void SetEvent()
        {
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        void SetControl()
        {
            this.ShowInTaskbar = false;
        }

        void Display_Secondary_Monitor()
        {
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 1)     // Has more screen
            {
                Screen scrn = (screens[0].WorkingArea.Contains(this.Location))
                                         ? screens[1] : screens[0];
                this.Location = new Point(scrn.Bounds.Left, 0);
            }
        }
    }
}
