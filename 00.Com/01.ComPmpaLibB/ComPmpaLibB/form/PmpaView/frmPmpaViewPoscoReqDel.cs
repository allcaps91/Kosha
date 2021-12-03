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
    /// File Name       : frmPmpaViewPoscoReqDel
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-04-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\OPD\oiguide\oiguide.vbp >> frmPmpaViewPoscoReqDel.cs 폼이름 재정의" />
    public partial class frmPmpaViewPoscoReqDel : Form
    {
        public frmPmpaViewPoscoReqDel()
        {
            InitializeComponent();
        }

        private void frmPmpaViewPoscoReqDel_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComFunc CF = new ComFunc();

            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, strSysDate, -30);
            dtpTDate.Text = strSysDate;
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            int nRead = 0;
            int nEXAMCnt = 0;
            string strEXAM = "";
            string strEXAM1 = "";
            string strEXAM2 = "";
            string strEXAM3 = "";
            string strEXAM4 = "";
            string strEXAM5 = "";
            string strEXAM6 = "";
            string strEXAM7 = "";
            string strEXAM8 = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ComFunc CF = new ComFunc();

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(JDATE,'YYYY-MM-DD') JDATE, SNAME, TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE, DELSAYU, ";
                SQL = SQL + ComNum.VBLF + " EXAMRES1, EXAMRES2, EXAMRES3, EXAMRES4, EXAM5, EXAMRES6, EXAMRES7, EXAMRES8 ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_PATIENT_POSCO_DTL ";
                if (rdoGubun_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE JDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                }
                else if (rdoGubun_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE DELDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND DELDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                }
                else if (rdoGubun_2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE (EXAMRES1 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES1 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES2 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES2 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES3 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES3 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES4 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES4 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES6 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES6 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES7 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES7 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "    OR (EXAMRES8 >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND EXAMRES8 < TO_DATE('" + CF.DATE_ADD(clsDB.DbCon, dtpTDate.Text, 1) + "','YYYY-MM-DD')) ";
                }
                SQL = SQL + ComNum.VBLF + "    AND GUBUN ='01' ";  //포스코대상
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

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = nRead;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nEXAMCnt = 0;
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        strEXAM1 = dt.Rows[i]["EXAMRES1"].ToString().Trim();
                        if (strEXAM1 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM2 = dt.Rows[i]["EXAMRES2"].ToString().Trim();
                        if (strEXAM2 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM3 = dt.Rows[i]["EXAMRES3"].ToString().Trim();
                        if (strEXAM3 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM4 = dt.Rows[i]["EXAMRES4"].ToString().Trim();
                        if (strEXAM4 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM5 = dt.Rows[i]["EXAM5"].ToString().Trim();
                        if (strEXAM5 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM6 = dt.Rows[i]["EXAMRES6"].ToString().Trim();
                        if (strEXAM6 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM7 = dt.Rows[i]["EXAMRES7"].ToString().Trim();
                        if (strEXAM7 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM8 = dt.Rows[i]["EXAMRES8"].ToString().Trim();
                        if (strEXAM8 != "")
                        {
                            nEXAMCnt = nEXAMCnt + 1;
                        }
                        strEXAM = strEXAM1 != "" ? "※초음파 : " + Convert.ToDateTime(strEXAM1).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "";
                        strEXAM = strEXAM + (strEXAM2 != "" ? "※위내시경 : " + Convert.ToDateTime(strEXAM2).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        strEXAM = strEXAM + (strEXAM3 != "" ? "※위내시경(수면) : " + Convert.ToDateTime(strEXAM3).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        strEXAM = strEXAM + (strEXAM4 != "" ? "※대장경 : " + Convert.ToDateTime(strEXAM4).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        strEXAM = strEXAM + (strEXAM5 != "" ? "※Biopsy : " + Convert.ToDateTime(strEXAM5).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        strEXAM = strEXAM + (strEXAM6 != "" ? "※대장경(수면) : " + Convert.ToDateTime(strEXAM6).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        strEXAM = strEXAM + (strEXAM7 != "" ? "※C/T : " + Convert.ToDateTime(strEXAM7).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        strEXAM = strEXAM + (strEXAM8 != "" ? "※위장조영촬영 : " + Convert.ToDateTime(strEXAM8).ToString("yyyy-MM-dd HH:mm") + ComNum.VBLF : "");
                        ssView_Sheet1.Cells[i, 2].Text = strEXAM;
                        //ssView_Sheet1.SetRowHeight(i, ComNum.SPDROWHT * nEXAMCnt);
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DELSAYU"].ToString().Trim();
                        ssView_Sheet1.SetRowHeight(i, Convert.ToInt32(ssView_Sheet1.GetPreferredRowHeight(i)) + 10);
                    }
                }

                dt.Dispose();
                dt = null;

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
    }
}
