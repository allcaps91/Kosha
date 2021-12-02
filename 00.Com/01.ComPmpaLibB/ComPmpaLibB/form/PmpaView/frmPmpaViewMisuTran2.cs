using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMisuTran2.cs
    /// Description     : 월별 미수금 현황
    /// Author          : 박창욱
    /// Create Date     : 2017-08-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUM203.frm(FrmMisuTran2.frm) >> frmPmpaViewMisuTran2.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuTran2 : Form
    {
        double[,] nTotAmt = new double[21, 10];

        public frmPmpaViewMisuTran2()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int nClass = 0;
            int nGubun = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            btnSearch.Enabled = false;
            btnPrint.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            Clear_Rtn();    //Sheet 및 변수를 Clear
            GelTot_Rtn(strYYMM, ref nClass);   //전월이얼액, 당월미수 등 계산
            Slip_Rtn(strFDate, strTDate, ref nClass, ref nGubun, ref nAmt);     //청구, 입금, 반송, 과지급금, 계산착오 READ
            Display_Rtn();  //소계계산 및 Display

            Cursor.Current = Cursors.Default;
            btnSearch.Enabled = true;
            btnPrint.Enabled = true;

            ssView.Focus();
        }

        //Sheet 및 변수를 Clear
        void Clear_Rtn()
        {
            int i = 0;
            int k = 0;

            ssView_Sheet1.Cells[1, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            for (i = 1; i < 21; i++)
            {
                for (k = 1; k < 10; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }
        }

        //전월이얼액, 당월미수 등 계산
        void GelTot_Rtn(string strYYMM, ref int nClass)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class,SUM(IwolAmt) cIwolAmt,SUM(MisuAmt) cMisuAmt  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GELTOT                  ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'                           ";
                SQL = SQL + ComNum.VBLF + "    AND Class <= '07'                                      ";
                if (rdoOut.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'O'                                   ";
                }
                else if (rdoIn.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'I'                                   ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY Class                                           ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Class                                           ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nClass = (int)VB.Val(dt.Rows[i]["Class"].ToString().Trim());
                    if (nClass == 20)
                    {
                        nClass = 6;
                    }
                    if (nClass == 7)
                    {
                        nClass = 6;
                    }
                    nTotAmt[1, nClass] += VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());

                    if (nClass == 6)
                    {
                        nTotAmt[2, nClass] += VB.Val(dt.Rows[i]["cMisuAmt"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //청구, 입금, 반송, 과지급금, 계산착오 READ
        void Slip_Rtn(string strFDate, string strTDate, ref int nClass, ref int nGubun, ref double nAmt)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Class,Gubun,SUM(Amt) cAmt                          ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_SLIP                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND Gubun > '10'                                       ";
                SQL = SQL + ComNum.VBLF + "    AND Class <= '07'                                      ";
                if (rdoOut.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'O'                                   ";
                }
                else if (rdoIn.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'I'                                   ";
                }
                SQL = SQL + ComNum.VBLF + "  GROUP BY Class,Gubun                                     ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Class,Gubun                                     ";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nClass = (int)VB.Val(dt.Rows[i]["Class"].ToString().Trim());
                    nGubun = (int)VB.Val(dt.Rows[i]["Gubun"].ToString().Trim());
                    nAmt = VB.Val(dt.Rows[i]["cAmt"].ToString().Trim());
                    if (nClass == 7)
                    {
                        nClass = 6;
                    }

                    switch (nGubun)
                    {
                        case 11:
                            nTotAmt[2, nClass] += nAmt; //처음청구
                            break;
                        case 12:
                            nTotAmt[3, nClass] += nAmt; //정산청구
                            break;
                        case 13:
                        case 14:
                            nTotAmt[4, nClass] += nAmt; //재청구, 추가청구
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            nTotAmt[5, nClass] += nAmt; //기타청구
                            break;

                        case 21:
                            nTotAmt[7, nClass] += nAmt; //입금
                            break;
                        case 22:
                            nTotAmt[8, nClass] += nAmt; //정산입금
                            break;
                        case 23:
                            nTotAmt[9, nClass] += nAmt; //주민입금
                            break;
                        case 24:
                            nTotAmt[10, nClass] += nAmt; //착오분입금
                            break;
                        case 25:
                            nTotAmt[10, nClass] += nAmt; //정산입금
                            break;
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            nTotAmt[7, nClass] += nAmt; //심사중입금
                            break;

                        case 31:
                            nTotAmt[12, nClass] += nAmt;
                            break;
                        case 32:
                            nTotAmt[13, nClass] += nAmt;
                            break;
                        case 33:
                            nTotAmt[14, nClass] += nAmt;
                            break;
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                            nTotAmt[15, nClass] += nAmt;
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //소계 계산 및 Sheet에 Display
        void Display_Rtn()
        {
            int i = 0;
            int j = 0;

            //청구, 입금, 심사조정 소계 구하기
            for (i = 1; i < 16; i++)
            {
                if (i != 6 && i != 11 && i != 16)
                {
                    for (j = 1; j < 9; j++)
                    {
                        nTotAmt[i, 9] += nTotAmt[i, j]; //항목계
                    }
                }
                for (j = 1; j < 10; j++)
                {
                    switch (i)
                    {
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            nTotAmt[6, j] += nTotAmt[i, j]; //청구소계
                            nTotAmt[17, j] += nTotAmt[i, j];
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            nTotAmt[11, j] += nTotAmt[i, j];    //입금소계
                            nTotAmt[18, j] += nTotAmt[i, j];
                            break;
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                            nTotAmt[16, j] += nTotAmt[i, j];    //심사조정
                            nTotAmt[19, j] += nTotAmt[i, j];
                            break;
                    }
                }
            }

            //현재잔액을 계산
            for (i = 1; i < 10; i++)
            {
                nTotAmt[20, i] += nTotAmt[1, i] + nTotAmt[17, i];
                nTotAmt[20, i] -= nTotAmt[18, i] - nTotAmt[19, i];
            }

            //Sheet에 Display
            for (i = 1; i < 21; i++)
            {
                for (j = 1; j < 10; j++)
                {
                    if (j < 7)
                    {
                        ssView_Sheet1.Cells[i, j + 1].Text = nTotAmt[i, j].ToString("###,###,###,##0 ");
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 8].Text = nTotAmt[i, 9].ToString("###,###,###,##0 ");
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string JobDate = "";
            string PrintDate = "";
            string JobMan = "";

            btnPrint.Enabled = false;

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            JobMan = clsType.User.JobName;
            JobDate = cboYYMM.Text;

            //Print Head
            strFont1 = "/c/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/l/f1" + cboYYMM.Text + " 미수금 현황";
            if (rdoOut.Checked == true)
            {
                strHead1 += "(외래)";
            }
            else if (rdoIn.Checked == true)
            {
                strHead1 += "(입원)";
            }
            else
            {
                strHead1 += "(전체)";
            }
            strHead1 += "/n";

            strHead2 = "/l/f2" + "작성자 : " + JobMan + "/n";
            strHead2 += "/l/f2" + "출력시간 : " + PrintDate;

            //Print Body
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 20;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Both;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;
        }

        private void frmPmpaViewMisuTran2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 15, "", "1");

            cboYYMM.SelectedIndex = 0;
        }

        private void rdoOut_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch.Focus();

            if (rdoIn.Checked == true)
            {
                rdoIn.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
                rdoOut.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                rdoAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            else if (rdoOut.Checked == true)
            {
                rdoIn.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                rdoOut.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
                rdoAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
            }
            else if (rdoAll.Checked == true)
            {
                rdoIn.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                rdoOut.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                rdoAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
            }
        }
    }
}
