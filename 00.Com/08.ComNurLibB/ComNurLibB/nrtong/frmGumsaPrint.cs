using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System.Drawing.Printing;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "D:\psmh\nurse\nrtong\nrtong.vbp\nrtong16.frm >> frmGumsaPrint.cs 폼이름 재정의" />

    public partial class frmGumsaPrint : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmGumsaPrint()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            PrintDocument pd;
            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = clsPrint.gGetDefaultPrinter();
            
            pd.PrintPage += new PrintPageEventHandler(eBarBARPrint);
            pd.Print();    //프린트
        }

        private void eBarBARPrint(object sender, PrintPageEventArgs e)
        {
            Rectangle r1 = new Rectangle(30, 170, 900, 800);
            Rectangle r2 = new Rectangle(10, 650, 900, 500);

            e.Graphics.DrawString("주 사 실 및 특 수 검 사 실", new Font("맑은 고딕", 15f), Brushes.Black, 280, 100, new StringFormat());    //헤더 그려주기
            e.Graphics.DrawString("작업년월일: " + dtpDate.Value.Year + "년" + dtpDate.Value.Month + "월" + dtpDate.Value.Day +"일", new Font("맑은 고딕", 10f), Brushes.Black, 320, 140, new StringFormat());    //헤더 그려주기

            if (SS1_Sheet1.Cells[19, 10].Text.Trim() == "")
            {
                SS1_Sheet1.Cells[19, 10].Text = " ";
            }

            SS1.OwnerPrintDraw(e.Graphics, r1, 0, 1);
            SS2.OwnerPrintDraw(e.Graphics, r2, 0, 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int J = 0;
            int nJusaCol = 0; // '주사
            int nJusaRow = 0;
            int nInspectCol = 0; // '특수검사
            int nInspectRow = 0;
            string strSchedule = "";
            string strDate = "";
            string strYYMM = "";
            string[,] strName = new string[16, 11];
            int nDD = 0;
            int nCount = 0;
            int nJusaCount = 0;

            strDate = dtpDate.Text;
            strYYMM = VB.Left(strDate, 4) + VB.Mid(strDate, 6, 2);
            nDD = (int)VB.Val(VB.Right(strDate, 2));

            btnCancel.PerformClick();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                // -------------특수검사,주사실
                SQL = "";
                SQL = "SELECT A.GUBUN, A.QTY, B.NAME";
                SQL = SQL + ComNum.VBLF + " FROM NUR_JUSASIL A, NUR_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.CODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND B.GUBUN IN ('3','6')";
                SQL = SQL + ComNum.VBLF + "   AND a.QTY <> '0'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.GUBUN, A.CODE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("특수검사, 주사실 자료가 없습니다.");
                }

                nJusaRow = 1;
                nJusaCol = 8;
                nInspectRow = 1;
                nInspectCol = 1;
                nJusaCount = 0;

                if (dt.Rows.Count != 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "1")
                        {
                            SS1_Sheet1.Cells[nInspectRow - 1, nInspectCol - 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            SS1_Sheet1.Cells[nInspectRow - 1, nInspectCol].Text = dt.Rows[i]["QTY"].ToString().Trim();
                            nInspectRow++;

                            if (nInspectRow > 20)
                            {
                                nInspectRow = 1;
                                nInspectCol += 2;
                            }
                        }

                        if (dt.Rows[i]["GUBUN"].ToString().Trim() == "2")
                        {
                            SS1_Sheet1.Cells[nJusaRow - 1, nJusaCol - 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                            SS1_Sheet1.Cells[nJusaRow - 1, nJusaCol].Text = dt.Rows[i]["QTY"].ToString().Trim();
                            nJusaCount += Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString()));
                            nJusaRow++;

                            if (nJusaRow > 20)
                            {
                                nJusaRow = 1;
                                nJusaCol += 2;
                            }
                        }
                    }

                    SS1_Sheet1.Cells[nJusaRow - 1, nJusaCol - 1].Text = "**합계**";
                    SS1_Sheet1.Cells[nJusaRow - 1, nJusaCol].Text = nJusaCount.ToString();
                }

                dt.Dispose();
                dt = null;


                //--------------외래직원현황
                SQL = "";
                SQL = " SELECT SNAME, SCHEDULE FROM NUR_SCHEDULE1";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM ='" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('OPD','DOCT')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당하는 자료가 없습니다.");
                }
                else
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSchedule = ComFunc.MidH(dt.Rows[i]["Schedule"].ToString(), (4 * nDD - 3), 4);

                        if (strSchedule.Trim().Length != 0)
                        {
                            switch (strSchedule)
                            {
                                case "휴가":
                                    nCount = 1;
                                    break;
                                case "월차":
                                    nCount = 2;
                                    break;
                                case "생휴":
                                    nCount = 3;
                                    break;
                                case "경조":
                                    nCount = 4;
                                    break;
                                case "병결":
                                    nCount = 5;
                                    break;
                                case "결근":
                                    nCount = 6;
                                    break;
                                case "조퇴":
                                    nCount = 7;
                                    break;
                                case "출장":
                                    nCount = 8;
                                    break;
                                case "학회":
                                    nCount = 9;
                                    break;
                                case "교육":
                                    nCount = 10;
                                    break;
                                case "훈련":
                                    nCount = 11;
                                    break;
                                case "대학":
                                    nCount = 12;
                                    break;
                                case "연수":
                                    nCount = 13;
                                    break;
                                case "분휴":
                                    nCount = 14;
                                    break;
                                default:
                                    nCount = 15;
                                    break;
                            }

                            for (J = 1; J <= 10; J++)
                            {
                                if (strName[nCount, J] == null)
                                {
                                    strName[nCount, J] = dt.Rows[i]["Sname"].ToString().Trim();
                                    break;
                                }
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                //Display
                for (i = 1; i <= 15; i++)
                {
                    for (J = 1; J <= 10; J++)
                    {
                        if (strName[i, J] == "")
                        {
                            break;
                        }
                        SS2_Sheet1.Cells[J - 1, i - 1].Text = strName[i, J];
                    }
                }

                SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                btnPrint.Enabled = true;
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

        private void frmGumsaPrint_Load(object sender, EventArgs e)
        {
            dtpDate.Value = Convert.ToDateTime(strDTP);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 20;
            SS2_Sheet1.RowCount = 0;
            SS2_Sheet1.RowCount = 10;
        }

        private void dtpDate_Leave(object sender, EventArgs e)
        {
            DateTime dtSysDate = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            if (dtpDate.Value > dtSysDate)
            {
                dtpDate.Value = dtSysDate;
            }
        }
    }
}
