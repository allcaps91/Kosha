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
    /// File Name       : frmPmpaViewGyeMisuPrint.cs
    /// Description     : 계약처 진료비 미수 내역
    /// Author          : 박창욱
    /// Create Date     : 2017-10-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR205.FRM(FrmMisuPrint.frm) >> frmPmpaViewGyeMisuPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeMisuPrint : Form
    {
        string[] GstrGels = new string[401];

        public frmPmpaViewGyeMisuPrint()
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

            strTitle = cboYYMM.Text + " 계약처 진료비 미수 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자 : " + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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

            int nRowCnt = 0;
            int nCnt = 0;
            int nSCnt = 0;
            int nTCnt = 0;
            int nTICnt = 0;
            int nTOCnt = 0;
            double nSAmt6 = 0;
            double nTAmt6 = 0;
            double nGTIAmt6 = 0;
            double nGTOAmt6 = 0;
            string strYYMM = "";
            string strLdate = "";
            string strSqlGel = "";
            string strCoIPDOPD = "";
            string strCoGel = "";
            string strFirst = "";

            int nSeq = 0;
            double nAmt6 = 0;   //현잔액
            string strIpdOpd = "";
            string strPano = "";
            string strSName = "";
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
            strLdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strYYMM, 4).Trim()), Convert.ToInt32(VB.Mid(cboYYMM.Text, 7, 2)));
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

            #region MISU_MASTER_READ

            strSqlGel = GstrGels[cboGel.SelectedIndex];
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT M.MisuID, M.Remark, M.GelCode,";
                SQL = SQL + ComNum.VBLF + "        M.DeptCode, M.IpdOpd, M.MGRRANK,";
                SQL = SQL + ComNum.VBLF + "        To_CHAR(M.Bdate,'YYYY-MM-DD') BDate, To_CHAR(M.FromDate,'YYYY-MM-DD') FDate, To_CHAR(M.ToDate,'YYYY-MM-DD') TDate,";
                SQL = SQL + ComNum.VBLF + "        a.JanAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_Monthly a, " + ComNum.DB_PMPA + "MISU_IDMST M";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '08'";  //계약처
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt <> 0";
                if (strSqlGel != "9999")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + strSqlGel.Trim() + "'";
                }
                SQL = SQL + ComNum.VBLF + "    And a.WRTNO = M.WRTNO";
                if (VB.Left(cboWon.Text, 1) == "2")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.GELCODE IN ( SELECT RTRIM(MIACODE) FROM BAS_MIA WHERE GBWON = '*' )";
                }
                else if (VB.Left(cboWon.Text, 1) == "3")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.GELCODE NOT IN ( SELECT RTRIM(MIACODE) FROM BAS_MIA WHERE GBWON = '*')";
                }
                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode,M.MisuID,M.BDate,M.IpdOpd";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode,M.BDate,M.MisuID,M.IpdOpd";
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

                nRowCnt = dt.Rows.Count;

                for (i = 0; i < nRowCnt; i++)
                {
                    #region ADD_SPREAD

                    nCnt += 1;
                    nSCnt += 1;
                    nTCnt += 1;

                    strPano = dt.Rows[i]["MisuID"].ToString().Trim();
                    strSName = cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["SName"].ToString();
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
                        MISU_Sub_Rtn(ref nSCnt, ref nTAmt6, ref nSeq, ref strCoGel, strGelCode, ref strCoIPDOPD, strIpdOpd);
                    }

                    strGelName = cpf.GET_BAS_MIA(clsDB.DbCon, strGelCode);

                    nSeq += 1;
                    ssView_Sheet1.RowCount += 1;

                    if (nSeq != 1)
                    {
                        strGelName = "";
                    }
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = nSeq.ToString();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strDate1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = strPano;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strSName;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strDate2 + "-" + strDate3;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = strIpdOpd;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = strDeptCode;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nAmt6.ToString();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = VB.DateDiff("D", Convert.ToDateTime(strDate1), Convert.ToDateTime(strLdate)).ToString();
                    switch (dt.Rows[i]["MGRRANK"].ToString().Trim())
                    {
                        case "2":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "일부입금";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "대손처리";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "산재신청중";
                            break;
                        case "5":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "일부입금";
                            ssView.ActiveSheet.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 240, 240);
                          
                            break;
                        case "9":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "기타";
                            ssView.ActiveSheet.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 240, 240);
                          
                            break;
                    }
                    nSAmt6 += nAmt6;
                    nTAmt6 += nAmt6;

                    #endregion
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            #endregion

            if (strSqlGel == "9999")
            {
                MISU_Sub_Rtn(ref nSCnt, ref nTAmt6, ref nSeq, ref strCoGel, strGelCode, ref strCoIPDOPD, strIpdOpd);
            }

            #region MISU_Total_Rtn

            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  외 래 합 계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTOCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nGTOAmt6.ToString();
            ssView.ActiveSheet.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 240, 240);

            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  입 원 합 계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTICnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nGTIAmt6.ToString();
            ssView.ActiveSheet.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 240, 240);
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "  전 체 합 계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (nTOCnt + nTICnt) + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = (nGTOAmt6 + nGTIAmt6).ToString();
            ssView.ActiveSheet.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 240, 240);
            nTOCnt = 0;
            nGTOAmt6 = 0;
            nTICnt = 0;
            nGTIAmt6 = 0;

            #endregion

        }

        void MISU_Sub_Rtn(ref int nSCnt, ref double nTAmt6, ref int nSeq, ref string strCoGel, string strGelCode, ref string strCoIPDOPD, string strIpdOpd)
        {
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = " 소  계 ";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTAmt6.ToString();
            ssView.ActiveSheet.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(221, 255, 221);
            nTAmt6 = 0;
            nSeq = 0;
            nSCnt = 0;
            strCoGel = strGelCode;
            strCoIPDOPD = strIpdOpd;
        }

        private void frmPmpaViewGyeMisuPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboGel.Items.Add("전   체");
            GstrGels[0] = "9999";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MiaCode GelCode, MiaName";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA";
                SQL = SQL + ComNum.VBLF + "  WHERE MiaCode >= 'H001' AND MiaCode <= 'H999'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY MiaCode";

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
                    cboGel.Items.Add(dt.Rows[i]["MiaName"].ToString().Trim());
                    GstrGels[i + 1] = dt.Rows[i]["GelCode"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                cboGel.SelectedIndex = 0;

                cboWon.Items.Clear();
                cboWon.Items.Add("1.전체");
                cboWon.Items.Add("2.원무행정");
                cboWon.Items.Add("3.계약처");
                cboWon.SelectedIndex = 0;

                clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
                cboYYMM.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
