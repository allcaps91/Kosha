using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSearchUnpaidOcs
    /// File Name : frmSearchUnpaidOcs.cs
    /// Title or Description : 비급여고지항목 조정
    /// Author : 안정수
    /// Create Date : 2021-01-19
    /// Update Histroy : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso> 
    /// </seealso>
    /// <vbp>
    /// </vbp>
    public partial class frmSearchUnpaidOcs : Form
    {
        int ChkCount = 0;

        public frmSearchUnpaidOcs()
        {
            InitializeComponent();
        }

        void frmSearchUnpaidOcs_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ReadSugastrGbSelfHang();

            txtCode.Focus();

            //ssView_Sheet1.Columns[3].Visible = false;
        }

        void ReadSugastrGbSelfHang(string argSuCode = "")
        {
            int i = 0;
            string SQL = string.Empty;
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                ssView_Sheet1.RowCount = 0;
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = " SELECT A.SUGBF, B.SUNEXT, B.SUNAMEK, B.ROWID, A.BAMT, (a.BAMT*1.25) BBAMT,A.IAMT, B.BCODE , A.SUGBE, A.BUN ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT A, BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";                
                SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.SUNEXT";
                

                //if (optXray.Checked == false)
                //{
                //    SQL = SQL + ComNum.VBLF + "    AND B.GBSELFHANG = 'Y' ";
                //}

                SQL = SQL + ComNum.VBLF + "    AND ((A.BUN IN ('71','73') AND A.SUGBF = '1') OR B.GBSELFHANG = 'Y')";

                if (argSuCode == "")
                {
                    if (optYak.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('11','12','20') ";
                    }
                    else if (optDressing.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('24','25','28','29','34','36') ";
                    }
                    else if (optExam.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('47','48','50','58','60','63','64') ";
                    }
                    else if (optXray.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BUN IN ('71','73') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.SUGBF = '1'        ";
                    }
                    else if (optProof.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BUN = '75' ";
                    }
                    else if (optETC.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.BUN NOT IN ('11','12','20','24','25','28','29','34','36','47','48','50','58','60','63','64','71','73','75')";
                    }
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND (UPPER(B.SUNEXT) LIKE '%" + argSuCode + "%'";
                    SQL = SQL + ComNum.VBLF + "         OR B.SUNAMEK LIKE '%" + argSuCode + "%'";
                    SQL = SQL + ComNum.VBLF + "         OR UPPER(B.SUNAMEE) LIKE '%" + argSuCode + "%')";                    
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY A.BUN, B.SUNEXT ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0 + 1].Text = (dt.Rows[i]["SugbF"].ToString().Trim() == "1" ? "◎" : "");
                        ssView_Sheet1.Cells[i, 1 + 1].Text = READ_BUN_NAME(dt.Rows[i]["BUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 2 + 1].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3 + 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4 + 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5 + 1].Text = dt.Rows[i]["BAMT"].ToString().Trim();

                        if (dt.Rows[i]["SUGBE"].ToString().Trim() == "1")
                        {
                            ssView_Sheet1.Cells[i, 6 + 1].Text = dt.Rows[i]["BBAMT"].ToString().Trim(); //원금액 보험수가 * 25%
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 6 + 1].Text = dt.Rows[i]["BAMT"].ToString().Trim();  //원금액 보험수가 
                        }

                        ssView_Sheet1.Columns[6 + 1].BackColor = Color.FromArgb(250, 244, 192);

                        ssView_Sheet1.Cells[i, 7 + 1].Text = dt.Rows[i]["IAMT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8 + 1].Text = dt.Rows[i]["BCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9 + 1].Text = dt.Rows[i]["SUGBE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }

            Cursor.Current = Cursors.Default;
        }

        public string READ_BUN_NAME(string argBun)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + " CODE || '.' || NAME AS NAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUN";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND JONG = '1'";
            SQL += ComNum.VBLF + "  AND CODE = '" + argBun + "'";
            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt2.Rows.Count > 0)
            {
                rtnVal = dt2.Rows[0]["NAME"].ToString().Trim();
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        void opt_CheckedChanged(object sender, EventArgs e)
        {
            ReadSugastrGbSelfHang();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            ReadSugastrGbSelfHang();
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strPrintName1 = string.Empty;

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();                
            }
            catch 
            {
                clsPrint.gSetDefaultPrinter(strPrintName1);
            }

            ss1_Sheet1.PrintInfo.Printer = strPrintName1;
            //ss1_Sheet1.PrintInfo.Printer = "emr서식";
            ss1_Sheet1.PrintInfo.ShowColumnHeader = PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            ss1_Sheet1.PrintInfo.ShowBorder = false;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = false;
            ss1_Sheet1.PrintInfo.ShowShadows = true;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.Centering = Centering.Both;
            ss1_Sheet1.PrintInfo.PrintType = PrintType.All;

            int intChk = 0;
            int intChkAll = 0;

            ss1.ActiveSheet.Cells[11, 0, 15, 0].Text = string.Empty;
            ss1.ActiveSheet.Cells[11, 7, 15, 7].Text = string.Empty;

            ss1.ActiveSheet.Cells[0, 9].Value = 0;
            ss1.ActiveSheet.Cells[3, 9].Value = 0;

            ss1.ActiveSheet.Cells[0, 7].Text = clsOrdFunction.Pat.PtNo;
            ss1.ActiveSheet.Cells[2, 7].Text = clsOrdFunction.Pat.sName;
            ss1.ActiveSheet.Cells[4, 7].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, clsOrdFunction.Pat.DeptCode);

            if (clsOrdFunction.Pat.WardCode != "")
            {
                ss1.ActiveSheet.Cells[3, 9].Text = "True";
            }
            else
            {
                ss1.ActiveSheet.Cells[0, 9].Text = "True";
            }

            ss1.ActiveSheet.Cells[20, 6].Text = VB.Left(clsPublic.GstrSysDate, 4) + "년　" + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월　" + VB.Right(clsPublic.GstrSysDate, 2) + "일";

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    intChkAll++;
            }

            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                    continue;

                //비금여 항목
                ss1.ActiveSheet.Cells[intChk + 11, 0].Text = ssView_Sheet1.Cells[i, 3 + 1].Text;
                //예상비용
                ss1.ActiveSheet.Cells[intChk + 11, 7].Text = ssView_Sheet1.Cells[i, 6 + 1].Text;

                intChk++;

                if (intChk == 5 || (intChk > 0 && intChk % 5 == 0) || i == ssView_Sheet1.RowCount - 1 || intChk == intChkAll)
                {
                    ss1.PrintSheet(0);

                    ComFunc.Delay(500);
                    ss1.ActiveSheet.Cells[11, 0, 15, 0].Text = string.Empty;
                    ss1.ActiveSheet.Cells[11, 7, 15, 7].Text = string.Empty;
                }
            }

            if (intChkAll == 0)
            {
                ss1.PrintSheet(0);

                ComFunc.Delay(500);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if(txtCode.Text != "")
            {
                ReadSugastrGbSelfHang(txtCode.Text.ToUpper().Trim());
            }
        }

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnView_Click(sender, e);
            }
        }
    }
}

