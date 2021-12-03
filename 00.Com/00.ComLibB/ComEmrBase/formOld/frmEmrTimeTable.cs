using System;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// frmEmrTimeTable
    /// 
    /// \mtsEmrf\frmEmrTimeTable.frm
    /// </summary>
    public partial class frmEmrTimeTable : Form
    {
        //시간정보 전달
        public delegate void SetTime(int Row, int Column, string Time);
        public event SetTime rSendTime;

        int Row = -1;
        int Column = -1;

        public frmEmrTimeTable()
        {
            InitializeComponent();
        }

        public frmEmrTimeTable(int mRow, int mColumn)
        {
            this.Row = mRow;
            this.Column = mColumn;
            InitializeComponent();
        }

        private void btnTimeApply_Click(object sender, EventArgs e)
        {
            StringBuilder strTime = new StringBuilder();

            for(int i = 0; i < ssTime_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssTime_Sheet1.ColumnCount; j++)
                {
                    switch (j)
                    {
                        case 0:
                        case 2:
                        case 4:
                        case 6:
                        case 8:
                        case 10:
                            if (ssTime_Sheet1.Cells[i, j].Text.Trim().Equals("True"))
                            {
                                if(strTime.Length > 0)
                                {
                                    strTime.Append("/");
                                }
                                strTime.Append(ssTime_Sheet1.Cells[i, j + 1].Text.Trim());
                            }
                            break;
                    }
                }
            }

            rSendTime(Row, Column, strTime.ToString().Trim());
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
