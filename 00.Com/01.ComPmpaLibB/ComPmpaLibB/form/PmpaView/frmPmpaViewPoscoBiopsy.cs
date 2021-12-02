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
using ComDbB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPoscoBiopsy
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-04-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\OPD\oiguide\oiguide.vbp >> frmPmpaViewPoscoBiopsy.cs 폼이름 재정의" />
    public partial class frmPmpaViewPoscoBiopsy : Form
    {
        public frmPmpaViewPoscoBiopsy()
        {
            InitializeComponent();
        }

        private void frmPmpaViewPoscoBiopsy_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFDate.Text = strSysDate;
            dtpTDate.Text = strSysDate;
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nRead = 0;
            int nRow = 0;
            string strOK = "";
            string strROWID = "";

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            ComFunc CF = new ComFunc();

            SS_Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(JDATE,'YYYY-MM-DD') JDATE, SNAME, JUMIN1 || JUMIN2 JUMIN ,Gubun, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT_POSCO ";
                SQL = SQL + ComNum.VBLF + " WHERE ( EXAMRES2 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND EXAMRES2 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "    OR (EXAMRES3 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND EXAMRES3 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "    OR (EXAMRES4 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND EXAMRES4 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "    OR (EXAMRES6 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND EXAMRES6 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "    OR (EXAMRES8 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND EXAMRES8 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY JDATE, SNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRead = dt.Rows.Count;

                ssView_Sheet1.RowCount = nRead;

                nRow = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strOK = "";
                        if (dt.Rows[i]["Gubun"].ToString().Trim() == "01")
                        {
                            strOK = "OK";
                        }
                        if (strOK == "OK")
                        {
                            strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                            SQL = "";
                            SQL = " SELECT Pano,SName,REMARK_BIOPSY,BIOPSY_GBN,EXAM5,Sabun,Buse,TO_CHAR(EXAMRES2,'YYYY-MM-DD') EXAMRES2,TO_CHAR(EXAMRES3,'YYYY-MM-DD') EXAMRES3,";
                            SQL = SQL + ComNum.VBLF + "  TO_CHAR(EXAMRES4,'YYYY-MM-DD') EXAMRES4,TO_CHAR(EXAMRES6,'YYYY-MM-DD') EXAMRES6,TO_CHAR(EXAMRES7,'YYYY-MM-DD')  EXAMRES7, ";
                            SQL = SQL + ComNum.VBLF + "   TO_CHAR(EXAMRES8,'YYYY-MM-DD') EXAMRES8";
                            SQL = SQL + ComNum.VBLF + "  FROM BAS_PATIENT_POSCO ";
                            SQL = SQL + ComNum.VBLF + "  WHERE ROWID ='" + strROWID + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN ='01' ";  //포스코대상
                            if (chkBiopsy.Checked == true)
                            {
                                SQL = SQL + ComNum.VBLF + "   AND EXAM5 ='Y' ";  //대상자만
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //EXAMRES8 DATE 위장조영
                            //EXAMRES2 DATE 위내시경
                            //EXAMRES3 DATE 위내시경(수면)
                            //EXAMRES4 DATE 대장경
                            //EXAMRES6 DATE 대장경(수면)

                            if (dt1.Rows.Count > 0)
                            {
                                nRow = nRow + 1;

                                //내시경
                                if (Convert.ToDateTime(dt1.Rows[0]["EXAMRES2"].ToString().Trim()) >= dtpFDate.Value && Convert.ToDateTime(dt1.Rows[0]["EXAMRES2"].ToString().Trim()) <= dtpTDate.Value )
                                {
                                    ssView_Sheet1.Cells[nRow, 0].Text = dt1.Rows[0]["EXAMRES2"].ToString().Trim();
                                }
                                //수면내시경
                                if (Convert.ToDateTime(dt1.Rows[0]["EXAMRES3"].ToString().Trim()) >= dtpFDate.Value && Convert.ToDateTime(dt1.Rows[0]["EXAMRES3"].ToString().Trim()) <= dtpTDate.Value && ssView_Sheet1.Cells[nRow, 0].Text == "")
                                {
                                    if (ssView_Sheet1.Cells[nRow, 0].Text == "")
                                    {
                                        ssView_Sheet1.Cells[nRow, 0].Text = dt1.Rows[0]["EXAMRES3"].ToString().Trim();
                                    }
                                }
                                //대장경
                                if (Convert.ToDateTime(dt1.Rows[0]["EXAMRES4"].ToString().Trim()) >= dtpFDate.Value && Convert.ToDateTime(dt1.Rows[0]["EXAMRES4"].ToString().Trim()) <= dtpTDate.Value && ssView_Sheet1.Cells[nRow, 0].Text == "")
                                {
                                    if (ssView_Sheet1.Cells[nRow, 0].Text == "")
                                    {
                                        ssView_Sheet1.Cells[nRow, 0].Text = dt1.Rows[0]["EXAMRES4"].ToString().Trim();
                                    }
                                }
                                //수면대장경
                                if (Convert.ToDateTime(dt1.Rows[0]["EXAMRES6"].ToString().Trim()) >= dtpFDate.Value && Convert.ToDateTime(dt1.Rows[0]["EXAMRES6"].ToString().Trim()) <= dtpTDate.Value && ssView_Sheet1.Cells[nRow, 0].Text == "")
                                {
                                    if (ssView_Sheet1.Cells[nRow, 0].Text == "")
                                    {
                                        ssView_Sheet1.Cells[nRow, 0].Text = dt1.Rows[0]["EXAMRES6"].ToString().Trim();
                                    }
                                }
                                //위장조영
                                if (Convert.ToDateTime(dt1.Rows[0]["EXAMRES8"].ToString().Trim()) >= dtpFDate.Value && Convert.ToDateTime(dt1.Rows[0]["EXAMRES8"].ToString().Trim()) <= dtpTDate.Value && ssView_Sheet1.Cells[nRow, 0].Text == "")
                                {
                                    if (ssView_Sheet1.Cells[nRow, 0].Text == "")
                                    {
                                        ssView_Sheet1.Cells[nRow, 0].Text = dt1.Rows[0]["EXAMRES8"].ToString().Trim();
                                    }
                                }

                                ssView_Sheet1.Cells[nRow, 1].Text = dt1.Rows[0]["Pano"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow, 2].Text = dt1.Rows[0]["SName"].ToString().Trim();

                                ssView_Sheet1.Cells[nRow, 3].Text = dt1.Rows[0]["Buse"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow, 4].Text = dt1.Rows[0]["Sabun"].ToString().Trim();

                                ssView_Sheet1.Cells[nRow, 5].Text = dt1.Rows[0]["SName"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow, 6].Text = dt1.Rows[0]["EXAM5"].ToString().Trim();

                                switch (dt1.Rows[0]["REMARK_BIOPSY"].ToString().Trim())
                                {
                                    case "1":
                                        ssView_Sheet1.Cells[nRow, 7].Text = "우편";
                                        break;
                                    case "2":
                                        ssView_Sheet1.Cells[nRow, 7].Text = "등기";
                                        break;
                                    case "3":
                                        ssView_Sheet1.Cells[nRow, 7].Text = "팩스";
                                        break;
                                }

                                ssView_Sheet1.Cells[nRow, 8].Text = dt1.Rows[0]["REMARK_BIOPSY"].ToString().Trim();
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SS_Clear()
        {
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }
    }
}
