using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>   
    /// File Name       : frmCalendar2.cs
    /// Description     : 달력
    /// Author          : 유진호
    /// Create Date     : 2018-06-01        
    /// </summary>
    public partial class frmCalendar1 : Form
    {
        string FstrCalDate = "";

        public frmCalendar1()
        {
            InitializeComponent();
        }

        private void frmCalendar1_Load(object sender, EventArgs e)
        {
            FstrCalDate = VB.Trim(clsPublic.GstrCalDate);
            if (clsPublic.GstrSysDate == "")
            {
                clsPublic.GstrSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            }
            if (string.IsNullOrEmpty(FstrCalDate))
            {
                FstrCalDate = clsPublic.GstrSysDate;
            }
            
            monthCalendarAdv1.SelectedDate = Convert.ToDateTime(FstrCalDate);
            monthCalendarAdv1.DisplayMonth = Convert.ToDateTime(FstrCalDate);
        }

        private void monthCalendarAdv1_ItemDoubleClick(object sender, MouseEventArgs e)
        {
            clsPublic.GstrCalDate = FstrCalDate;
            this.Dispose();
        }

        private void monthCalendarAdv1_DateChanged(object sender, EventArgs e)
        {
            FstrCalDate = monthCalendarAdv1.SelectedDate.ToString("yyyy-MM-dd");
        }

        private void monthCalendarAdv1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                clsPublic.GstrCalDate = FstrCalDate;
                this.Dispose();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsPublic.GstrCalDate = FstrCalDate;
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
