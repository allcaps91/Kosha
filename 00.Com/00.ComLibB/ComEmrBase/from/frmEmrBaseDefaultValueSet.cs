using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrBaseDefaultValueSet : Form
    {
        string mJOBGB = "";
        string mITEMCD = "";

        public delegate void SetValue(string strValue);
        public event SetValue rSetValue;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmEmrBaseDefaultValueSet()
        {
            InitializeComponent();
        }

        public frmEmrBaseDefaultValueSet(string pJOBGB, string pITEMCD)
        {
            InitializeComponent();
            mJOBGB = pJOBGB;
            mITEMCD = pITEMCD;
        }

        private void frmEmrBaseDefaultValueSet_Load(object sender, EventArgs e)
        {
            ssValue_Sheet1.RowCount = 0;

            if (mJOBGB != "")
            {
                GetValue(mITEMCD);
            }
        }

        /// <summary>
        /// 아이템에 등록된 값을 가지고 온다
        /// </summary>
        /// <param name="strITEMCD"></param>
        private void GetValue(string strITEMCD)
        {
            string SQL = "";
            int i = 0;
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            ssValue_Sheet1.RowCount = 0;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, MAX(VALUENO) AS VALUENO, ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + mJOBGB + "'";
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "GROUP BY ITEMCD, ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "ORDER BY MAX(DSPSEQ)";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                ssValue_Sheet1.RowCount = dt.Rows.Count;
                ssValue_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssValue_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void btnSetValue1_Click(object sender, EventArgs e)
        {
            string strValue = "";

            for (int i = 0; i < ssValue_Sheet1.RowCount; i++)
            {
                if (ssValue_Sheet1.Cells[i, 0].Text == "True")
                {
                    if (i == 0)
                    {
                        strValue = strValue + ssValue_Sheet1.Cells[i, 1].Text.Trim();
                    }
                    else
                    {
                        strValue = strValue + ComNum.VBLF + ssValue_Sheet1.Cells[i, 1].Text.Trim();
                    }
                }
            }

            rSetValue(strValue);
        }

        private void btnSetValue2_Click(object sender, EventArgs e)
        {
            string strValue = txtValue.Text.Trim();
            rSetValue(txtValue.Text.Trim());
        }

    }
}
