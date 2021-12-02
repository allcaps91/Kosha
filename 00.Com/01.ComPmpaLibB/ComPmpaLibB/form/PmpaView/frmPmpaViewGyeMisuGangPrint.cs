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
    /// File Name       : frmPmpaViewGyeMisuGangPrint.cs
    /// Description     : 계약처 심신장애자 미수내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUG310.FRM(FrmMisuGangPrint.frm) >> frmPmpaViewGyeMisuGangPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeMisuGangPrint : Form
    {
        string[] GstrGels = new string[401];

        public frmPmpaViewGyeMisuGangPrint()
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

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboYYMM.Text + " 심신 장애 미수 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자:" + clsType.User.JobName + " 인 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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
            int nRowCnt = 0;
            double nSAmt6 = 0;
            double nTAmt6 = 0;
            double nGTIAmt6 = 0;
            double nGTOAmt6 = 0;
            string strYYMM = "";
            string strLdate = "";
            string strCoIPDOPD = "";
            string strCoGel = "";
            string strFirst = "";
            string strComboText = "";

            int nSeq = 0;
            double nAmt6 = 0;   //현잔액
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

            clsPmpaFunc cpf = new clsPmpaFunc();

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strLdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(cboYYMM.Text, 4)), Convert.ToInt32(VB.Mid(cboYYMM.Text, 7, 2)));

            ssView_Sheet1.RowCount = 0;

            nGTIAmt6 = 0;
            nGTOAmt6 = 0;
            nTICnt = 0;
            nTOCnt = 0;
            strFirst = "OK";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region MISU_MASTER_READ

                strComboText = VB.Right(cboClass.Text, 6).Trim();

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT M.MisuID, M.Remark, M.GelCode,";
                SQL = SQL + ComNum.VBLF + "        M.DeptCode, M.IpdOpd, To_CHAR(M.Bdate,'YYYY-MM-DD') BDate,";
                SQL = SQL + ComNum.VBLF + "        To_CHAR(M.FromDate,'YYYY-MM-DD') FDate, To_CHAR(M.ToDate,'YYYY-MM-DD') TDate, a.JanAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_Monthly a, " + ComNum.DB_PMPA + "MISU_IDMST M";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.class = '13'";
                if (cboClass.Text.Trim() != "전체")
                {
                    if (strComboText == "00000")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.GelCode NOT IN ('47111','47113','47130','47930','47770','47230','47170') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Gelcode = '" + strComboText + "'";
                    }
                }
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt <> 0";
                SQL = SQL + ComNum.VBLF + "    And a.WRTNO = M.WRTNO";
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Class,M.Gelcode,M.MisuID,M.BDate,M.IpdOpd";
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

                nRowCnt = dt.Rows.Count;
                for (i = 0; i < nRowCnt; i++)
                {
                    #region ADD_SPREAD

                    strPANO = dt.Rows[i]["MisuID"].ToString().Trim();
                    strSname = cpf.Get_BasPatient(clsDB.DbCon, strPANO).Rows[0]["SName"].ToString().Trim();
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
                        #region Misu_Sub_Rtn

                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "소   계";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSCnt + " 건";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTAmt6.ToString();
                        nTAmt6 = 0;
                        nSeq = 0;
                        nSCnt = 0;
                        strCoGel = strGelCode;
                        strCoIPDOPD = strIpdOpd;

                        #endregion
                    }

                    strGelName = cpf.GET_BAS_MIA(clsDB.DbCon, strGelCode.Trim());

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

                    nSAmt6 += nAmt6;
                    nTAmt6 += nAmt6;

                    nCnt += 1;
                    nSCnt += 1;
                    nTCnt += 1;

                    #endregion
                }
                dt.Dispose();
                dt = null;

                #endregion

                #region Misu_Sub_Rtn

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "소   계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSCnt + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTAmt6.ToString();
                nTAmt6 = 0;
                nSeq = 0;
                nSCnt = 0;
                strCoGel = strGelCode;
                strCoIPDOPD = strIpdOpd;

                #endregion

                #region Misu_Total_Rtn

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  전 체 합 계 ";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (nTOCnt + nTICnt) + " 건";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (nGTOAmt6 + nGTIAmt6).ToString();
                nTOCnt = 0;
                nGTOAmt6 = 0;
                nTICnt = 0;
                nGTIAmt6 = 0;

                #endregion

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewGyeMisuGangPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            cboYYMM.SelectedIndex = 1;

            cboClass.Items.Clear();
            cboClass.Items.Add("전체");
            cboClass.Items.Add("포항시 남구                             47111");
            cboClass.Items.Add("포항시 북구                             47113");
            cboClass.Items.Add("경북 경주시                             47130");
            cboClass.Items.Add("경북 영천시                             47230");
            cboClass.Items.Add("경북 안동시                             47170");
            cboClass.Items.Add("경북 영덕군                             47770");
            cboClass.Items.Add("경북 울진군                             47930");
            cboClass.Items.Add("기      타                             00000");
            cboClass.SelectedIndex = 0;
        }
    }
}
