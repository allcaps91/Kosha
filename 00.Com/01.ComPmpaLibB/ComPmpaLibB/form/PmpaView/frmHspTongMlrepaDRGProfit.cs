using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : HspTong
    /// File Name       : frmHspTongMlrepaDRGProfit.cs
    /// Description     : DRG 수익분석
    /// Author          : 박창욱
    /// Create Date     : 2018-11-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\tong\mlrepa\FrmDRG수익분석.frm(FrmDRG수익분석) >> frmHspTongMlrepaDRGProfit.cs 폼이름 재정의" />	
    public partial class frmHspTongMlrepaDRGProfit : Form
    {
        string FstrDrName = "";
        string FstrPANO = "";
        string FstrSName = "";
        string FstrInDate = "";
        string FstrOutDate = "";

        public frmHspTongMlrepaDRGProfit()
        {
            InitializeComponent();
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "DRG 수익 분석";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ DRG 질환 : " + cboDRG.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 작업연월 : " + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "DRG 수익 분석";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ DRG 질환 : " + cboDRG.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 의사명 : " + FstrDrName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 작업연월 : " + cboFYYMM.Text + " ~ " + cboTYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void btnPrint3_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "DRG 수익 분석";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ DRG 질환 : " + cboDRG.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 의사명 : " + FstrDrName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 환자정보 : " + FstrPANO + " " + FstrInDate + " " + FstrOutDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String(" ◈ 출력일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView3, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int nRow = 0;
            string strJob = "";
            string strNew = "";
            string strOLD = "";
            string strFDate = "";
            string strTdate = "";
            string strPano = "";
            double nTRSNO = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            double nCNT1 = 0;
            double nIlsu1 = 0;
            double[] nTotAmt1 = new double[5]; //의사별
            double nCNT2 = 0;
            double nIlsu2 = 0;
            double[] nTotAmt2 = new double[5]; //과별
            double nCNT3 = 0;
            double nIlsu3 = 0;
            double[] nTotAmt3 = new double[5]; //전체합계

            ComFunc cf = new ComFunc();

            if (cboDRG.Text.Trim() == "")
            {
                ComFunc.MsgBox("DRG 질환을 선택하세요.");
                return;
            }

            if (cboFYYMM.Text.Trim() == "")
            {
                ComFunc.MsgBox("시작 년월을 입력하세요.");
                return;
            }

            if (cboTYYMM.Text.Trim() == "")
            {
                ComFunc.MsgBox("종료 년월을 입력하세요.");
                return;
            }

            if (string.Compare(cboFYYMM.Text, cboTYYMM.Text) > 0)
            {
                ComFunc.MsgBox("시작 년월이 종료 년월보다 큼");
                return;
            }

            strFDate = cboFYYMM.Text.Trim() + "-01";
            strTdate = cf.READ_LASTDAY(clsDB.DbCon, cboTYYMM.Text.Trim() + "-01");
            strJob = VB.Left(cboDRG.Text, 1).Trim();

            //누적할 변수를 Clear
            for (i = 0; i < 5; i++)
            {
                nTotAmt1[i] = 0;
                nTotAmt2[i] = 0;
                nTotAmt3[i] = 0;
            }

            //ssView1.MaxRows(0);
            //ssView2.MaxRows(0);
           // ssView3.MaxRows(0);

           // ssView1.MaxRows(20);
           // ssView2.MaxRows(9);
            //ssView3.MaxRows(16);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //DB에서 자료를 SELECT
                SQL = "";
                SQL = "SELECT DeptCode,DrCode,Pano,TRSNO,Ilsu,Amt50,Amt65,Amt66 ";
                SQL += ComNum.VBLF + " FROM IPD_TRANS ";
                SQL += ComNum.VBLF + "WHERE OutDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND OutDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                if (strJob == "1")     //1.수정체수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='C' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Czzz' ";
                }
                else if (strJob == "2") //2.편도 및 아데노이드절제술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='D' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Dzzz' ";
                }
                else if (strJob == "3") //3.충수절제술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='G08' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='G08zz' ";
                }
                else if (strJob == "4") //4.서혜 및 대퇴부 탈장수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='G09' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='G09zz' ";
                }
                else if (strJob == "5") //5.항문수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='G10' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='G10zz' ";
                }
                else if (strJob == "6") //6.자궁적출 및 자궁.부속기 수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='N' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Nzzz' ";
                }
                else if (strJob == "7") //7.제왕절개분만
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='O' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Ozzz' ";
                }
                else if (strJob == "*") //전체
                {
                    SQL += ComNum.VBLF + " AND DrgCode IS NOT NULL ";
                }
                SQL += ComNum.VBLF + "  AND GBIPD != 'D' ";
                SQL += ComNum.VBLF + "ORDER BY DeptCode,DrCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strOLD = "";
                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nAmt1 = 0;
                    nAmt2 = 0;

                    strNew = dt.Rows[i]["DEPTCODE"].ToString().Trim() + "{}";
                    strNew += dt.Rows[i]["DRCODE"].ToString().Trim();

                    if (strOLD == "")
                    {
                        strOLD = strNew;
                    }
                    else if (strOLD != strNew)
                    {
                        nRow++;

                        if (ssView1_Sheet1.RowCount < nRow)
                        {
                            ssView1_Sheet1.RowCount = nRow;
                        }

                        ssView1_Sheet1.Cells[nRow - 1, 0].Text = " " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Pstr(strOLD, "{}", 1));
                        ssView1_Sheet1.Cells[nRow - 1, 1].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, VB.Pstr(strOLD, "{}", 2));
                        ssView1_Sheet1.Cells[nRow - 1, 2].Text = nCNT1.ToString("#,##0");
                        ssView1_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt1[1].ToString("#,##0");
                        ssView1_Sheet1.Cells[nRow - 1, 4].Text = nTotAmt1[2].ToString("#,##0");
                        ssView1_Sheet1.Cells[nRow - 1, 5].Text = nTotAmt1[3].ToString("#,##0");

                        if (nIlsu1 != 0 && nCNT1 != 0)
                        {
                            ssView1_Sheet1.Cells[nRow - 1, 7].Text = (nIlsu1 / nCNT1).ToString("#,##0.0");
                        }

                        ssView1_Sheet1.Cells[nRow - 1, 9].Text = VB.Pstr(strOLD, "{}", 2);

                        nCNT1 = 0;
                        nIlsu1 = 0;
                        nTotAmt1[1] = 0;
                        nTotAmt1[2] = 0;
                        nTotAmt1[3] = 0;
                        nTotAmt1[4] = 0;

                        //과별합계
                        if (VB.Pstr(strOLD, "{}", 1) != VB.Pstr(strNew, "{}", 1))
                        {
                            nRow++;

                            if (ssView1_Sheet1.RowCount < nRow)
                            {
                                ssView1_Sheet1.RowCount = nRow;
                            }

                            ssView1_Sheet1.Cells[nRow - 1, 0].Text = " 과별합계";
                            ssView1_Sheet1.Cells[nRow - 1, 1].Text = "";
                            ssView1_Sheet1.Cells[nRow - 1, 2].Text = nCNT2.ToString("#,##0");
                            ssView1_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt2[1].ToString("#,##0");
                            ssView1_Sheet1.Cells[nRow - 1, 4].Text = nTotAmt2[2].ToString("#,##0");
                            ssView1_Sheet1.Cells[nRow - 1, 5].Text = nTotAmt2[3].ToString("#,##0");

                            if (nIlsu2 != 0 && nCNT2 != 0)
                            {
                                ssView1_Sheet1.Cells[nRow - 1, 7].Text = (nIlsu2 / nCNT2).ToString("#,##0.0");
                            }

                            ssView1_Sheet1.Cells[nRow - 1, 9].Text = "";

                            nCNT2 = 0;
                            nIlsu2 = 0;
                            nTotAmt2[1] = 0;
                            nTotAmt2[2] = 0;
                            nTotAmt2[3] = 0;
                            nTotAmt2[4] = 0;
                        }

                        strOLD = strNew;
                    }

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    nTRSNO = VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim());

                    //DRG 차액을 구함(합산)
                    SQL = "";
                    SQL = "SELECT SUM(Amt1+Amt2) ChaAmt FROM IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "WHERE Pano='" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND TRSNO=" + nTRSNO + " ";
                    SQL += ComNum.VBLF + "  AND SuNext IN ('DRG001','DRG002') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        nAmt1 += VB.Val(dt1.Rows[j]["ChaAmt"].ToString().Trim()); 
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //행위별 총액을 구함
                    SQL = "";
                    SQL = "SELECT SUM(Amt1+Amt2) ChaAmt FROM IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "WHERE Pano='" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND TRSNO=" + nTRSNO + " ";
                    SQL += ComNum.VBLF + "  AND SuNext NOT IN ('DRG001','DRG002') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        nAmt2 += VB.Val(dt1.Rows[j]["ChaAmt"].ToString().Trim());   
                    }

                    dt1.Dispose();
                    dt1 = null;

                    nCNT1++;
                    nIlsu1 += VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());  
                    nTotAmt1[1] += VB.Val(dt.Rows[i]["AMT50"].ToString().Trim()) ;
                    nTotAmt1[2] += nAmt2;
                    nTotAmt1[3] += nAmt1;

                    //과별합계
                    nCNT2++;
                    nIlsu2 += VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    nTotAmt2[1] += VB.Val(dt.Rows[i]["AMT50"].ToString().Trim());
                    nTotAmt2[2] += nAmt2;
                    nTotAmt2[3] += nAmt1;

                    //전체합계
                    nCNT3++;
                    nIlsu3 += VB.Val(dt.Rows[i]["Ilsu"].ToString().Trim());
                    nTotAmt3[1] += VB.Val(dt.Rows[i]["AMT50"].ToString().Trim());
                    nTotAmt3[2] += nAmt2;
                    nTotAmt3[3] += nAmt1;
                }

                dt.Dispose();
                dt = null;

                //의사
                nRow++;

                if (ssView1_Sheet1.RowCount < nRow)
                {
                    ssView1_Sheet1.RowCount = nRow;
                }

                ssView1_Sheet1.Cells[nRow - 1, 0].Text = " " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Pstr(strNew, "{}", 1));
                ssView1_Sheet1.Cells[nRow - 1, 1].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, VB.Pstr(strNew, "{}", 2));
                ssView1_Sheet1.Cells[nRow - 1, 2].Text = nCNT1.ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt1[1].ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 4].Text = nTotAmt1[2].ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 5].Text = nTotAmt1[3].ToString("#,##0");

                if (nIlsu1 != 0 && nCNT1 != 0)
                {
                    ssView1_Sheet1.Cells[nRow - 1, 7].Text = (nIlsu1 / nCNT1).ToString("#,##0.0");
                }

                ssView1_Sheet1.Cells[nRow - 1, 9].Text = VB.Pstr(strOLD, "{}", 2);

                //과별합계
                nRow++;

                if (ssView1_Sheet1.RowCount < nRow)
                {
                    ssView1_Sheet1.RowCount = nRow;
                }

                ssView1_Sheet1.Cells[nRow - 1, 0].Text = " 과별합계";
                ssView1_Sheet1.Cells[nRow - 1, 1].Text = "";
                ssView1_Sheet1.Cells[nRow - 1, 2].Text = nCNT2.ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt2[1].ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 4].Text = nTotAmt2[2].ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 5].Text = nTotAmt2[3].ToString("#,##0");

                if (nIlsu2 != 0 && nCNT2 != 0)
                {
                    ssView1_Sheet1.Cells[nRow - 1, 7].Text = (nIlsu2 / nCNT2).ToString("#,##0.0");
                }

                ssView1_Sheet1.Cells[nRow - 1, 9].Text = ""; 

                //전체합계
                nRow++;

                if (ssView1_Sheet1.RowCount < nRow)
                {
                    ssView1_Sheet1.RowCount = nRow;
                }

                ssView1_Sheet1.Cells[nRow - 1, 0].Text = " 전체합계";
                ssView1_Sheet1.Cells[nRow - 1, 1].Text = "";
                ssView1_Sheet1.Cells[nRow - 1, 2].Text = nCNT3.ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt3[1].ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 4].Text = nTotAmt3[2].ToString("#,##0");
                ssView1_Sheet1.Cells[nRow - 1, 5].Text = nTotAmt3[3].ToString("#,##0");

                if (nIlsu3 != 0 && nCNT3 != 0)
                {
                    ssView1_Sheet1.Cells[nRow - 1, 7].Text = (nIlsu3 / nCNT3).ToString("#,##0.0");
                }

                ssView1_Sheet1.Cells[nRow - 1, 9].Text = "";

                ssView1_Sheet1.RowCount = nRow;
                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void frmHspTongMlrepaDRGProfit_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboFYYMM, 12, "", "0");
            clsVbfunc.SetCboDate(clsDB.DbCon, cboTYYMM, 12, "", "0");

            cboDRG.Items.Clear();
            cboDRG.Items.Add("*.전체");
            cboDRG.Items.Add("1.수정체수술");
            cboDRG.Items.Add("2.편도 및 아데노이드절제술");
            cboDRG.Items.Add("3.충수절제술");
            cboDRG.Items.Add("4.서혜 및 대퇴부 탈장수술");
            cboDRG.Items.Add("5.항문수술");
            cboDRG.Items.Add("6.자궁적출 및 자궁.부속기 수술");
            cboDRG.Items.Add("7.제왕절개분만");
            cboDRG.SelectedIndex = 0;

            ssView1_Sheet1.Columns[6].Visible = false;
            ssView2_Sheet1.Columns[8].Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView1_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView1_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            int nRow = 0;
            string strFDate = "";
            string strTDate = "";
            string strJob = "";
            string strPano = "";
            string strDrCode = "";
            double nChaAmt1 = 0;
            double nChaAmt2 = 0;
            double nTRSNO = 0;

            ComFunc cf = new ComFunc();

            FstrDrName = ssView1_Sheet1.Cells[e.Row, 1].Text.Trim();
            strDrCode = ssView1_Sheet1.Cells[e.Row, 9].Text.Trim();

            strFDate = cboFYYMM.Text.Trim() + "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, cboTYYMM.Text.Trim() + "-01");
            strJob = VB.Left(cboDRG.Text, 1).Trim();

           // ssView2.MaxRows(0);
           // ssView2.MaxRows(50);
            ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //DB에서 자료를 SELECT
                SQL = "";
                SQL = "SELECT DeptCode,Pano,TRSNO,DrgCode,Ilsu,Amt50,Amt65,Amt66, ";
                SQL += ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(OutDate,'YYYY-MM-DD') OutDate ";
                SQL += ComNum.VBLF + " FROM IPD_TRANS ";
                SQL += ComNum.VBLF + "WHERE OutDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND OutDate<=TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND DrCode='" + strDrCode + "' ";
                if (strJob == "1")     //1.수정체수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='C' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Czzz' ";
                }
                else if (strJob == "2") //2.편도 및 아데노이드절제술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='D' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Dzzz' ";
                }
                else if (strJob == "3") //3.충수절제술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='G08' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='G08zz' ";
                }
                else if (strJob == "4") //4.서혜 및 대퇴부 탈장수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='G09' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='G09zz' ";
                }
                else if (strJob == "5") //5.항문수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='G10' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='G10zz' ";
                }
                else if (strJob == "6") //6.자궁적출 및 자궁.부속기 수술
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='N' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Nzzz' ";
                }
                else if (strJob == "7") //7.제왕절개분만
                {
                    SQL += ComNum.VBLF + " AND DrgCode>='O' ";
                    SQL += ComNum.VBLF + " AND DrgCode<='Ozzz' ";
                }
                else if (strJob == "*") //*.전체
                {
                    SQL += ComNum.VBLF + " AND DrgCode IS NOT NULL ";
                }
                SQL += ComNum.VBLF + "  AND GBIPD != 'D' ";
                SQL += ComNum.VBLF + "ORDER BY DeptCode,DrCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nChaAmt1 = 0;
                    nChaAmt2 = 0;

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    nTRSNO = VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim());

                    //DRG 차액
                    SQL = "";
                    SQL = "SELECT SUM(Amt1+Amt2) ChaAmt FROM IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "WHERE Pano='" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND TRSNO=" + nTRSNO + " ";
                    SQL += ComNum.VBLF + "  AND SuNext IN ('DRG001','DRG002') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        nChaAmt1 += VB.Val(dt1.Rows[j]["ChaAmt"].ToString().Trim()); 
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //행위별 총액
                    SQL = "";
                    SQL = "SELECT SUM(Amt1+Amt2) ChaAmt FROM IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "WHERE Pano='" + strPano + "' ";
                    SQL += ComNum.VBLF + "  AND TRSNO=" + nTRSNO + " ";
                    SQL += ComNum.VBLF + "  AND SuNext NOT IN ('DRG001','DRG002') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (j = 0; j < dt1.Rows.Count; j++)
                    {
                        nChaAmt2 += VB.Val(dt1.Rows[j]["ChaAmt"].ToString().Trim());  
                    }

                    dt1.Dispose();
                    dt1 = null;

                    nRow++;

                    if (nRow > ssView2_Sheet1.RowCount)
                    {
                        ssView2_Sheet1.RowCount = nRow;
                    }

                    ssView2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 1].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano);
                    ssView2_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["InDate"].ToString().Trim(); 
                    ssView2_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Ilsu"].ToString().Trim(); 
                    ssView2_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["Amt50"].ToString().Trim()).ToString("#,##0");   
                    ssView2_Sheet1.Cells[nRow - 1, 6].Text = nChaAmt2.ToString("#,##0");
                    ssView2_Sheet1.Cells[nRow - 1, 7].Text = nChaAmt1.ToString("#,##0");
                    ssView2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["DrgCode"].ToString().Trim();
                    ssView2_Sheet1.Cells[nRow - 1, 11].Text = nTRSNO.ToString();

                }

                dt.Dispose();
                dt = null;

                ssView2_Sheet1.RowCount = nRow;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssView2_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView2_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            double nTRSNO = 0;
            string strOLD = "";
            string strNew = "";
            bool bOK = false;
            double nAmt1 = 0;
            double nAmt2 = 0;

            FstrPANO = ssView2_Sheet1.Cells[e.Row, 0].Text.Trim();
            FstrSName = ssView2_Sheet1.Cells[e.Row, 1].Text.Trim();
            FstrInDate = ssView2_Sheet1.Cells[e.Row, 2].Text.Trim();
            FstrOutDate = ssView2_Sheet1.Cells[e.Row, 3].Text.Trim();
            nTRSNO = VB.Val(ssView2_Sheet1.Cells[e.Row, 11].Text);

            ssView3_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 200;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT a.Nu,a.SuNext,b.SuNameK,SUM(a.Qty*a.Nal) Qty,";
                SQL += ComNum.VBLF + " SUM(a.Amt1) Amt1,SUM(a.Amt2) Amt2 ";
                SQL += ComNum.VBLF + " FROM IPD_NEW_SLIP a,BAS_SUN b ";
                SQL += ComNum.VBLF + "WHERE a.Pano='" + FstrPANO + "' ";
                SQL += ComNum.VBLF + "  AND a.TRSNO = " + nTRSNO + " ";
                SQL += ComNum.VBLF + "  AND a.SuCode NOT IN ('DRG001','DRG002') ";
                SQL += ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                SQL += ComNum.VBLF + "GROUP BY a.Nu,a.SuNext,b.SuNameK ";
                SQL += ComNum.VBLF + "ORDER BY a.Nu,a.SuNext ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRow = 0;
                strOLD = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    bOK = true;

                    if (VB.Val(dt.Rows[i]["Amt1"].ToString().Trim())  == 0 && VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()) == 0)
                    {
                        bOK = false;
                    }

                    if (bOK == true)
                    {
                        strNew = dt.Rows[i]["Nu"].ToString().Trim();  
                        nRow++;

                        if (nRow > ssView3_Sheet1.RowCount)
                        {
                            ssView3_Sheet1.RowCount = nRow;
                        }

                        if (strOLD != strNew)
                        {
                            ssView3_Sheet1.Cells[nRow - 1, 0].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_누적행위명", strNew);
                            strOLD = strNew;
                        }

                        ssView3_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SuNext"].ToString().Trim(); 
                        ssView3_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SuNameK"].ToString().Trim(); 
                        ssView3_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("#,##0.0");  
                        ssView3_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim()).ToString("#,##0");   
                        ssView3_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()).ToString("#,##0");  

                        nAmt1 += VB.Val(dt.Rows[i]["Amt1"].ToString().Trim()); 
                        nAmt2 += VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());  
                    }
                }

                dt.Dispose();
                dt = null;

                ssView3_Sheet1.RowCount = nRow;

                nRow++;

                if (nRow > ssView3_Sheet1.RowCount)
                {
                    ssView3_Sheet1.RowCount = nRow;
                }

                ssView3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssView3_Sheet1.Cells[nRow - 1, 2].Text = "합  계";
                ssView3_Sheet1.Cells[nRow - 1, 4].Text = nAmt1.ToString("#,##0");
                ssView3_Sheet1.Cells[nRow - 1, 5].Text = nAmt2.ToString("#,##0");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
    }
}
