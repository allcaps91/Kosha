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
    /// File Name       : frmPmpaViewEtcMisuPrintcs.cs
    /// Description     : 기타미수 종류별 상세내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR212.FRM(FrmEtcMisuPrint.frm) >> frmPmpaViewEtcMisuPrintcs.cs 폼이름 재정의" />	
    public partial class frmPmpaViewEtcMisuPrintcs : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();

        public frmPmpaViewEtcMisuPrintcs()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = cboYYMM.Text + " 기타미수 종류별 상세내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("( " + VB.Mid(cboClass.Text, 4, 24) + " )", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "/r/f2" + "출력자 : " + clsType.User.JobName + " 인 " + VB.Space(18), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nCnt = 0;
            int nSCnt = 0;
            int nTCnt = 0;
            int nTICnt = 0;
            int nTOCnt = 0;
            int nRead = 0;
            double nSAmt6 = 0;
            double nTAmt6 = 0;
            double nGTIAmt6 = 0;
            double nGTOAmt6 = 0;
            string strYYMM = "";
            string strLdate = "";
            string strCoIPDOPD = "";
            string strCoGel = "";
            string strFirst = "";

            int nSeq = 0;
            string strIpdOpd = "";
            string strPANO = "";
            string strSname = "";
            string strGelName = "";
            string strGelCode = "";
            string strDeptCode = "";
            string strDate1 = "";
            string strDate2 = "";
            string strDate3 = "";
            //string strDate4 = "";   //사고일
            //string strDate5 = "";   //진료개시일
            double nAmt6 = 0;   //현잔액

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strLdate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));
            if (Convert.ToDateTime(strLdate) > Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")))
            {
                strLdate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            }
            ssView_Sheet1.RowCount = 0;
            nGTIAmt6 = 0;
            nGTOAmt6 = 0;
            nTICnt = 0;
            nTOCnt = 0;
            strFirst = "OK";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //MISU_SQL_PREPARE
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT M.MisuID, M.Remark, M.GelCode,";
                SQL = SQL + ComNum.VBLF + "        M.DeptCode, M.IpdOpd, To_CHAR(M.Bdate,'YYYY-MM-DD') BDate,";
                SQL = SQL + ComNum.VBLF + "        To_CHAR(M.FromDate,'YYYY-MM-DD') FDate, m.Gubun, To_CHAR(M.ToDate,'YYYY-MM-DD') TDate,";
                SQL = SQL + ComNum.VBLF + "        a.JanAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_Monthly a, " + ComNum.DB_PMPA + "MISU_IDMST M";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '" + VB.Left(cboClass.Text, 2) + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt <> 0";
                SQL = SQL + ComNum.VBLF + "    And a.WRTNO = M.WRTNO(+)";
                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode, M.MisuID, M.BDate, M.IpdOpd";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode, M.BDate, M.MisuID, M.IpdOpd";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strPANO = dt.Rows[i]["MisuID"].ToString().Trim();
                    strSname = cpf.Get_BasPatient(clsDB.DbCon, strPANO).Rows[0]["Sname"].ToString().Trim();
                    strGelCode = dt.Rows[i]["GelCode"].ToString().Trim();
                    strDeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strDate1 = dt.Rows[i]["BDate"].ToString().Trim();
                    strDate2 = dt.Rows[i]["FDate"].ToString().Trim();
                    strDate3 = dt.Rows[i]["TDate"].ToString().Trim();
                    nAmt6 = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());

                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                    {
                        strIpdOpd = "입원";
                    }
                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "O")
                    {
                        strIpdOpd = "외래";
                    }

                    if (strFirst == "OK")
                    {
                        strCoGel = strGelCode;
                        strCoIPDOPD = strIpdOpd;
                        strFirst = "NO";
                        nCnt = 0;
                        nSCnt = 0;
                        nTCnt = 0;
                    }

                    if (strGelCode != strCoGel)
                    {
                        Misu_Sub_Rtn(ref nSCnt, ref nTAmt6, ref nSeq, ref strCoGel, ref strCoIPDOPD, strGelCode, strIpdOpd);
                    }

                    nCnt += 1;
                    nSCnt += 1;
                    nTCnt += 1;

                    if (strIpdOpd == "입원")
                    {
                        nTICnt += 1;
                        nGTIAmt6 += nAmt6;
                    }
                    else
                    {
                        nTOCnt += 1;
                        nGTOAmt6 += nAmt6;
                    }

                    strGelName = cpm.READ_BAS_MIA(strGelCode.Trim());
                    nSeq += 1;
                    ssView_Sheet1.RowCount += 1;

                    if (nSeq != 1)
                    {
                        strGelName = "";
                    }

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = nSeq.ToString();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strDate1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strPANO;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strSname;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (strDate2 + "-" + strDate3).Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strIpdOpd;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = strDeptCode;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nAmt6.ToString();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = VB.DateDiff("D", Convert.ToDateTime(strDate1), Convert.ToDateTime(strLdate)).ToString();

                    if (VB.Left(cboClass.Text, 2) == "18")
                    {
                        if (dt.Rows[i]["Gubun"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = cpm.Misu_Gubun_NameChk(clsDB.DbCon, VB.Left(cboClass.Text, 2), dt.Rows[i]["Gubun"].ToString().Trim());
                        }
                    }

                    nSAmt6 += nAmt6;
                    nTAmt6 += nAmt6;
                }

                dt.Dispose();
                dt = null;

                Misu_Sub_Rtn(ref nSCnt, ref nTAmt6, ref nSeq, ref strCoGel, ref strCoIPDOPD, strGelCode, strIpdOpd);

                //Misu_Total_Rtn
                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  외 래 합 계 ";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTOCnt + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nGTOAmt6.ToString();

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  입 원 합 계 ";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTICnt + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nGTIAmt6.ToString();

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  전 체 합 계 ";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (nTOCnt + nTICnt) + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (nGTOAmt6 + nGTIAmt6).ToString();

                nTOCnt = 0;
                nGTOAmt6 = 0;
                nTICnt = 0;
                nGTIAmt6 = 0;

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Misu_Sub_Rtn(ref int nSCnt, ref double nTAmt6, ref int nSeq, ref string strCoGel, ref string strCoIPDOPD, string strGelCode, string strIpdOpd)
        {
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "소   계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTAmt6.ToString();

            nTAmt6 = 0;
            nSeq = 0;
            nSCnt = 0;
            strCoGel = strGelCode;
            strCoIPDOPD = strIpdOpd;
        }

        private void frmPmpaViewEtcMisuPrintcs_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboClass.Items.Clear();
            cboClass.Items.Add("08.계약처");
            cboClass.Items.Add("09.혈액원");
            cboClass.Items.Add("11.보훈청미수");
            cboClass.Items.Add("12.시각장애자");
            cboClass.Items.Add("13.심신장애진단비");
            cboClass.Items.Add("14.장애인보장구");
            cboClass.Items.Add("15.직원대납");
            cboClass.Items.Add("16.노인장기요양소견서");
            cboClass.Items.Add("17.방문간호지시서");
            cboClass.Items.Add("18.치매검사");
            cboClass.SelectedIndex = 0;

            ssView_Sheet1.RowCount = 0;

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            cboYYMM.SelectedIndex = 0;
        }
    }
}
