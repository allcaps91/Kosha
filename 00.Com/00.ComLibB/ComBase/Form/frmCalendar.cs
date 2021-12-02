using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ComBase
{
    public partial class frmCalendar : Form
    {
        string FstrCalDate = "";

        public frmCalendar()
        {
            InitializeComponent();
        }

        private void frmCalendar_Load(object sender, EventArgs e)
        {
            FstrCalDate = Strings.Trim(clsPublic.GstrCalDate);
            if (clsPublic.GstrSysDate == "")
            {
                clsPublic.GstrSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            }
            if (string.IsNullOrEmpty(FstrCalDate))
            {
                FstrCalDate = clsPublic.GstrSysDate;
            }
            TxtYear.Text = VB.Left(FstrCalDate, 4);
            UpDown1.Value = Convert.ToInt32(VB.Val(TxtYear.Text));
            Calendar.SetDate(Convert.ToDateTime(FstrCalDate));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsPublic.GstrCalDate = FstrCalDate;
            this.Dispose();
        }

        private void Calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            FstrCalDate = Strings.Format(e.Start, "yyyy-MM-dd");
        }

        private void UpDown1_Scroll(object sender, ScrollEventArgs e)
        {
            switch (e.Type)
            {
                case System.Windows.Forms.ScrollEventType.EndScroll:
                    UpDown1EnabledChanged(e.NewValue);
                    break;
            }
        }

        private void UpDown1EnabledChanged(int newScrollValue)
        {
            TxtYear.Text = Strings.Format(newScrollValue, "0000");
            FstrCalDate = TxtYear.Text + "-" + Strings.Mid(FstrCalDate, 6, 5);
            Calendar.SetDate(Convert.ToDateTime(Strings.Format(FstrCalDate, "yyyy-MM-dd")));
        }

        private void TxtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            int KeyAscii = Strings.Asc(e.KeyChar);

            int nYEAR = 0;

            if (KeyAscii == 13)
            {
                nYEAR = Convert.ToInt32(Conversion.Val(TxtYear.Text));
                if (nYEAR < 1753 | nYEAR > 9999)
                {
                    TxtYear.Text = Strings.Format(UpDown1.Value, "0000");
                    goto EventExitSub;
                }

                UpDown1.Value = nYEAR;
                TxtYear.Text = Strings.Format(nYEAR, "0000");
                FstrCalDate = TxtYear.Text + "-" + Strings.Mid(FstrCalDate, 6, 5);
                Calendar.SetDate(Convert.ToDateTime(Strings.Format(FstrCalDate, "yyyy-MM-dd")));

            }

            EventExitSub:

            e.KeyChar = Strings.Chr(KeyAscii);
            if (KeyAscii == 0)
            {
                e.Handled = true;
            }
        }
    }
}
