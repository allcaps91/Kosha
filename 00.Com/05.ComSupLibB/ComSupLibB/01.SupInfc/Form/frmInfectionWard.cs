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
using FarPoint.Win.Spread;


namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : SupInfc
    /// File Name       : frmInfectionWard
    /// Description     : 병원감염 현황
    /// Author          : 전상원
    /// Create Date     : 2017-03-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " PSMH\nurse\nrinfo\nrinfo.vbp(ExInfect04.frm) >> frmInfectionWard.cs 폼이름 재정의" />
    public partial class frmInfectionWard : Form
    {
        public frmInfectionWard()
        {
            InitializeComponent();
        }

        private void frmInfectionWard_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            int i = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            cboWard.Items.Clear();
            cboWard.Items.Add("MICU");
            cboWard.Items.Add("SICU");

            for (i = 0; i <= 3; i++)
            {
                cboYear.Items.Add(Convert.ToInt32(VB.Left(strSysDate, 4)) - i + " 년도");
            }

            cboYear.SelectedIndex = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT WARDCODE, WARDNAME FROM " + ComNum.DB_PMPA + "BAS_WARD";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU')";
                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"]);
                    }
                }

                cboWard.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                if (clsPublic.GstrWardCode != "")
                {
                    cboWard.Text = clsPublic.GstrWardCode;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int[,] data = new int[14, 13];
            int i = 0;
            int j = 0;
            int nRow = 0;
            string[] strSpec = new string[13];
            string strFDate = "";
            string strTDate = "";

            strFDate = VB.Left(cboYear.Text, 4) + "-01-01";
            strTDate = VB.Left(cboYear.Text, 4) + "-12-31";

            for (i = 0; i <= 12; i++)
            {
                strSpec[i] = "";
            }

            for (i = 1; i <= 13; i++)
            {
                for (j = 1; j <= 12; j++)
                {
                    data[i, j] = 0;
                }
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            //인공호흡기, 도뇨관, C-LINE 사용일수
            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(JOBDATE,'MM') JOBDATE, SUM(FOLEY) FOLEY, SUM(CLINE) CLINE, ";
                SQL = SQL + ComNum.VBLF + " SUM(VENTILATOR) VENTILATOR ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_INFECTION";
                SQL = SQL + ComNum.VBLF + " WHERE JOBDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND JOBDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND WARD = '" + cboWard.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY TO_CHAR(JOBDATE,'MM') ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = (int)VB.Val(dt.Rows[i]["JOBDATE"].ToString().Trim());

                        data[nRow, 5] = data[nRow, 5] + (int)VB.Val(dt.Rows[i]["VENTILATOR"].ToString().Trim());
                        data[nRow, 6] = data[nRow, 6] + (int)VB.Val(dt.Rows[i]["FOLEY"].ToString().Trim());
                        data[nRow, 7] = data[nRow, 7] + (int)VB.Val(dt.Rows[i]["CLINE"].ToString().Trim());

                        data[13, 5] = data[13, 5] + (int)VB.Val(dt.Rows[i]["VENTILATOR"].ToString().Trim());
                        data[13, 6] = data[13, 6] + (int)VB.Val(dt.Rows[i]["FOLEY"].ToString().Trim());
                        data[13, 7] = data[13, 7] + (int)VB.Val(dt.Rows[i]["CLINE"].ToString().Trim());
                    }
                }
                
                dt.Dispose();
                dt = null;

                //감염균류 COUNT
                SQL = "";
                SQL = " SELECT TO_CHAR(INFDATE,'MM') INFDATE, CODE, COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTDTL ";
                SQL = SQL + ComNum.VBLF + " WHERE INFDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND INFDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND WARD = '" + cboWard.Text + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(INFDATE,'MM'), CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = (int)VB.Val(dt.Rows[i]["INFDATE"].ToString().Trim());

                        if (strSpec[nRow] == "")
                        {
                            strSpec[nRow] = strSpec[nRow] + dt.Rows[i]["CODE"].ToString().Trim() + ":" + dt.Rows[i]["CNT"].ToString().Trim();
                        }
                        else
                        {
                            strSpec[nRow] = strSpec[nRow] + ", " + dt.Rows[i]["CODE"].ToString().Trim() + ":" + dt.Rows[i]["CNT"].ToString().Trim();
                        }

                        data[nRow, 3] = data[nRow, 3] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        data[13, 3] = data[13, 3] + (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                //인공호흡기, 도뇨관, C-LINE 감염건수
                SQL = "";
                SQL = " SELECT TO_CHAR(INFDATE,'MM') INFDATE, ";
                SQL = SQL + ComNum.VBLF + " SUM(DECODE(RESULT_A,'1',1,0)) RESULT_A, ";
                SQL = SQL + ComNum.VBLF + " SUM(DECODE(RESULT_B,'1',1,0)) RESULT_B, ";
                SQL = SQL + ComNum.VBLF + " SUM(DECODE(RESULT_C,'1',1,0)) RESULT_C  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECTDTL ";
                SQL = SQL + ComNum.VBLF + " WHERE INFDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND INFDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND WARD = '" + cboWard.Text + "' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(INFDATE,'MM') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = (int)VB.Val(dt.Rows[i]["INFDATE"].ToString().Trim());

                        data[nRow, 10] = data[nRow, 10] + (int)VB.Val(dt.Rows[i]["RESULT_C"].ToString().Trim());
                        data[nRow, 11] = data[nRow, 11] + (int)VB.Val(dt.Rows[i]["RESULT_B"].ToString().Trim());
                        data[nRow, 12] = data[nRow, 12] + (int)VB.Val(dt.Rows[i]["RESULT_A"].ToString().Trim());

                        data[13, 10] = data[13, 10] + (int)VB.Val(dt.Rows[i]["RESULT_C"].ToString().Trim()); //인공호흡
                        data[13, 11] = data[13, 11] + (int)VB.Val(dt.Rows[i]["RESULT_B"].ToString().Trim()); //유치도뇨
                        data[13, 12] = data[13, 12] + (int)VB.Val(dt.Rows[i]["RESULT_A"].ToString().Trim()); //중심정맥
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE,'MM') ACTDATE, SUM(QTY1 + QTY2 + QTY3 + QTY4) TCNT";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_INOUT ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + cboWard.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE IN ('03','09')";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(ACTDATE,'MM') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = (int)VB.Val(dt.Rows[i]["ACTDATE"].ToString().Trim());

                        data[nRow, 1] = data[nRow, 1] + (int)VB.Val(dt.Rows[i]["TCNT"].ToString().Trim());
                        data[13, 1] = data[13, 1] + (int)VB.Val(dt.Rows[i]["TCNT"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE,'MM') ACTDATE, SUM(QTY4) TCNT";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_INOUT ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE = '" + cboWard.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CODE IN ('01')";
                SQL = SQL + ComNum.VBLF + " GROUP BY TO_CHAR(ACTDATE,'MM') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = (int)VB.Val(dt.Rows[i]["ACTDATE"].ToString().Trim());

                        data[nRow, 2] = data[nRow, 2] + (int)VB.Val(dt.Rows[i]["TCNT"].ToString().Trim());
                        data[13, 2] = data[13, 2] + (int)VB.Val(dt.Rows[i]["TCNT"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                for (i = 1; i <= 13; i++)
                {
                    for (j = 1; j <= 12; j++)
                    {
                        if (j == 8) //퇴원자기준
                        {
                            if (data[i, 1] != 0)
                            {
                                ssView_Sheet1.Cells[i - 1, j - 1].Text = Math.Round((data[i, 3] / (double)data[i, 1] * 100), 1).ToString("###0.0") + " % ";
                            }
                        }
                        else if (j == 9) //재원자기준
                        {
                            if (data[i, 2] != 0)
                            {
                                ssView_Sheet1.Cells[i - 1, j - 1].Text = Math.Round((data[i, 3] / (double)data[i, 2] * 1000), 1).ToString("###0.0") + " ‰ ";
                            }
                        }
                        else if (j == 10 || j == 11 || j == 12)
                        {
                            if (data[i, j - 5] != 0)
                            {
                                ssView_Sheet1.Cells[i - 1, j - 1].Text = Math.Round((data[i, j] / (double)data[i, j - 5] * 1000), 1).ToString("###0.0") + " ‰ " + "(" + data[i, j] + ") ";
                            }
                        }
                        else
                        {
                            if (j != 4)
                            {
                                ssView_Sheet1.Cells[i - 1, j - 1].Text = data[i, j] + " ";
                            }
                        }
                    }
                }

                for (i = 1; i <= 12; i++)
                {
                    ssView_Sheet1.Cells[i - 1, 3].Text = strSpec[i];
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = cboWard.Text + " 병원감염 현황 ";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업년 : " + cboYear.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
