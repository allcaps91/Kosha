using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMirPrint.cs
    /// Description     : 산재 월별 미수금 명부
    /// Author          : 박창욱
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUS202.FRM(FrmMirPrint.frm) >> frmPmpaViewMirPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMirPrint : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();

        public frmPmpaViewMirPrint()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = cboYY.Text + "년 " + cboMM.Text + "월 " + "산재 미수금 명부(" + VB.Mid(cboGubun.Text, 3, 4) + ")";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nREAD = 0;
            int nRow = 0;
            int nSQty = 0;
            int nTQty = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            double nAmt3 = 0;
            double nAmt4 = 0;
            double nSAmt1 = 0;
            double nSAmt2 = 0;
            double nSAmt3 = 0;
            double nSAmt4 = 0;
            double nTAmt1 = 0;
            double nTAmt2 = 0;
            double nTAmt3 = 0;
            double nTAmt4 = 0;
            string strGubun = "";
            string strNEW = "";
            string strOLD = "";
            string strMiaName = "";
            string strSName = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            nRow = 0;
            strOLD = "";
            nSQty = 0;
            nSAmt1 = 0;
            nSAmt2 = 0;
            nSAmt3 = 0;
            nSAmt4 = 0;
            nTQty = 0;
            nTAmt1 = 0;
            nTAmt2 = 0;
            nTAmt3 = 0;
            nTAmt4 = 0;

            strGubun = VB.Left(cboGubun.Text, 1);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, a.GelCode, a.IpdOpd,";
                SQL = SQL + ComNum.VBLF + "        a.JanAmt, a.SakAmt, a.MisuAmt,";
                SQL = SQL + ComNum.VBLF + "        b.MisuID, b.Remark, TO_CHAR(FromDate,'YYYY-MM-DD') Fdate,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(ToDate,'YYYY-MM-DD') Tdate, b.DeptCode, TO_CHAR(b.Bdate,'YYYY-MM-DD') Bdate,";
                SQL = SQL + ComNum.VBLF + "        b.Amt2";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_MONTHLY a, " + ComNum.DB_PMPA + "MISU_IDMST b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + cboYY.Text + cboMM.Text + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '05'";   //산재
                if (strGubun == "2")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.MisuAmt > 0";
                }
                else if (strGubun == "3")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.JanAmt > 0";
                }
                else if (strGubun == "4")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.JanAmt = 0";
                }
                SQL = SQL + ComNum.VBLF + "    AND a.WRTNO = b.WRTNO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.GelCode,b.Remark,b.MisuID,b.Bdate";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    btnPrint.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    nAmt1 = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    nAmt2 = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    nAmt4 = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());
                    nAmt3 = nAmt1 - nAmt4 - nAmt2;

                    //1명을 Display
                    strNEW = dt.Rows[i]["GelCode"].ToString().Trim();
                    if (strNEW != strOLD)
                    {
                        SubTot_Display(strNEW, ref strMiaName, strOLD, ref nRow, ref nSQty, ref nSAmt1, ref nSAmt2,
                                       ref nSAmt3, ref nSAmt4, ref nTQty, ref nTAmt1, ref nTAmt2, ref nTAmt3, ref nTAmt4);
                    }

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (strNEW != strOLD)
                    {
                        strOLD = strNEW;
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strMiaName;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["MisuID"].ToString().Trim();

                    if (cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["MisuID"].ToString().Trim()).Rows.Count == 0)
                    {
                        strSName = "-< ERROR >-";
                    }
                    else
                    {
                        strSName = cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["MisuID"].ToString().Trim()).Rows[0]["SNAME"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = strSName;
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Fdate"].ToString().Trim() + "=>" + dt.Rows[i]["Tdate"].ToString().Trim();

                    switch (dt.Rows[i]["IpdOpd"].ToString().Trim())
                    {
                        case "O":
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "외래";
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = "입원";
                            break;
                    }

                    //ssView_Sheet1.Cells[nRow - 1, 5].Text = ComFunc.LeftH(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 28).Trim();   //근무처
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Left(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 28).Trim();   //근무처
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    //진료개시일
                    //ssView_Sheet1.Cells[nRow - 1, 7].Text = ComFunc.FormatStrToDate(ComFunc.MidH(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 36, 6), "D");
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = ComFunc.FormatStrToDate(VB.Mid(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 36, 6), "D");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = nAmt1.ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = nAmt2.ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = nAmt3.ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = nAmt4.ToString("###,###,###,##0 ");

                    nSQty += 1;
                    nSAmt1 += nAmt1;
                    nSAmt2 += nAmt2;
                    nSAmt3 += nAmt3;
                    nSAmt4 += nAmt4;
                }
                dt.Dispose();
                dt = null;

                SubTot_Display(strNEW, ref strMiaName, strOLD, ref nRow, ref nSQty, ref nSAmt1, ref nSAmt2,
                                       ref nSAmt3, ref nSAmt4, ref nTQty, ref nTAmt1, ref nTAmt2, ref nTAmt3, ref nTAmt4);

                nRow += 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[nRow - 1, 5].Text = "** 합 계 **";
                ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Right(VB.Space(4) + nTQty.ToString("###0"), 4) + " 건";
                ssView_Sheet1.Cells[nRow - 1, 9].Text = nTAmt1.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 10].Text = nTAmt2.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 11].Text = nTAmt3.ToString("###,###,###,##0 ");
                ssView_Sheet1.Cells[nRow - 1, 12].Text = nTAmt4.ToString("###,###,###,##0 ");

                nTAmt1 = 0;
                nTAmt2 = 0;
                nTAmt3 = 0;
                nTAmt4 = 0;
                nTQty = 0;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        void SubTot_Display(string strNEW, ref string strMiaName, string strOLD, ref int nRow, ref int nSQty, ref double nSAmt1, ref double nSAmt2, ref double nSAmt3,
                            ref double nSAmt4, ref int nTQty, ref double nTAmt1, ref double nTAmt2, ref double nTAmt3, ref double nTAmt4)
        {
            if (cpf.GET_BAS_MIA(clsDB.DbCon, strNEW) == "")
            {
                strMiaName = "-< ERROR >-";
            }
            else
            {
                strMiaName = cpf.GET_BAS_MIA(clsDB.DbCon, strNEW);
            }

            if (strOLD == "")
            {
                return;
            }

            nRow += 1;

            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 5].Text = "** 소 계 **";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Right(VB.Space(4) + nSQty.ToString("###0"), 4) + " 건";
            ssView_Sheet1.Cells[nRow - 1, 9].Text = nSAmt1.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 10].Text = nSAmt2.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 11].Text = nSAmt3.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[nRow - 1, 12].Text = nSAmt4.ToString("###,###,###,##0 ");

            nTQty = nTQty + nSQty;
            nTAmt1 = nTAmt1 + nSAmt1;
            nTAmt2 = nTAmt2 + nSAmt2;
            nTAmt3 = nTAmt3 + nSAmt3;
            nTAmt4 = nTAmt4 + nSAmt4;

            nSAmt1 = 0;
            nSAmt2 = 0;
            nSAmt3 = 0;
            nSAmt4 = 0;
            nSQty = 0;
        }

        private void frmPmpaViewMirPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;
            int i = 0;
            string strSysDate = "";

            cboYY.Items.Clear();
            cboMM.Items.Clear();
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));

            for (i = 1; i < 37; i++)
            {
                cboYY.Items.Add(nYY.ToString("0000"));
                cboMM.Items.Add(i.ToString("00"));
                nYY -= 1;
            }

            cboYY.SelectedIndex = 0;
            cboMM.SelectedIndex = nMM - 1;

            cboGubun.Items.Add("1.전체명단");
            cboGubun.Items.Add("2.청구명단");
            cboGubun.Items.Add("3.미수명단");
            cboGubun.Items.Add("4.완불명단");
            cboGubun.SelectedIndex = 0;
        }
    }
}
