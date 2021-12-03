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
    /// File Name       : frmPmpaViewGyeMisuPanoPrint.cs
    /// Description     : 계약처 진료비 개인별 미수 내역
    /// Author          : 박창욱
    /// Create Date     : 2017-09-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUR208.FRM(FrmMisuPanoPrint.frm) >> frmPmpaViewGyeMisuPanoPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeMisuPanoPrint : Form
    {
        string[] GstrGels = new string[401];

        public frmPmpaViewGyeMisuPanoPrint()
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

            strTitle = cboYYMM.Text + " 계약처 진료비 개인별 미수 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자:" + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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

            int nGelFLAG = 0;
            int nGelCnt = 0;
            int nRowCnt = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            double nAmt3 = 0;
            double nAmt4 = 0;
            double nTotAmt1 = 0;
            double nTotAmt2 = 0;
            double nTotAmt3 = 0;
            double nTotAmt4 = 0;
            double nGelTot = 0;
            string strYYMM = "";
            string strLdate = "";
            string strSqlGel = "";
            string strOldGel = "";
            string strNewGel = "";

            string strPano = "";
            string strSName = "";
            string strCarNo = "";
            string strBname = "";
            string strGelName = "";
            string strGelCode = "";
            string strDate4 = "";   //사고일
            string strDate5 = "";   //진료개시일
            string strRemark = "";

            clsPmpaFunc cpf = new clsPmpaFunc();

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strLdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strYYMM, 4).Trim()), Convert.ToInt32(VB.Mid(cboYYMM.Text, 7, 2)));

            ssView_Sheet1.RowCount = 0;
            nGelTot = 0;
            nGelFLAG = 1;
            strOldGel = "";

            strSqlGel = GstrGels[cboGel.SelectedIndex];

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT M.GelCode, M.MisuID, M.Remark,";
                SQL = SQL + ComNum.VBLF + "        SUM(M.Amt2) MisuAmt, SUM(M.Amt3+Amt5+Amt6+Amt7) IpgumAmt, SUM(M.Amt4) SakAmt";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_Monthly a, " + ComNum.DB_PMPA + "MISU_IDMST M";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.Class = '08'";  //계약처
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt > 0";
                if (strSqlGel != "9999")
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + strSqlGel.Trim() + "'";
                }
                SQL = SQL + ComNum.VBLF + "    And a.WRTNO = M.WRTNO";
                SQL = SQL + ComNum.VBLF + "  GROUP BY M.GelCode,M.MisuID,M.Remark";
                SQL = SQL + ComNum.VBLF + "  ORDER BY M.Gelcode,M.MisuID,M.Remark";

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
                    nAmt1 = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());
                    nAmt2 = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    nAmt3 = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    nAmt4 = nAmt1 - nAmt2 - nAmt3;

                    if (nAmt4 != 0)
                    {
                        #region ADD_SPREAD

                        strNewGel = dt.Rows[i]["GelCode"].ToString().Trim();
                        if (strOldGel == "")
                        {
                            strOldGel = strNewGel;
                        }

                        if (strOldGel != strNewGel)
                        {
                            Misu_Sub_Gel(ref nGelCnt, ref nTotAmt1, ref nTotAmt2, ref nTotAmt3, ref nTotAmt4);
                            strOldGel = strNewGel;
                            nGelFLAG = 1;
                        }

                        strPano = dt.Rows[i]["MisuID"].ToString().Trim();
                        strSName = cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["SName"].ToString().Trim();
                        strGelCode = dt.Rows[i]["GelCode"].ToString().Trim();
                        strRemark = VB.Left(dt.Rows[i]["Remark"].ToString().Trim() + VB.Space(40), 40);
                        strCarNo = VB.Mid(strRemark, 17, 14).Trim();
                        strBname = VB.Right(strRemark, 10).Trim();
                        strDate4 = VB.Mid(strRemark, 1, 8).Trim();
                        strDate5 = VB.Mid(strRemark, 9, 8).Trim();

                        nGelCnt += 1;
                        nGelTot += nAmt4;

                        strGelName = cpf.GET_BAS_MIA(clsDB.DbCon, strGelCode);

                        ssView_Sheet1.RowCount += 1;

                        if (nGelFLAG == 0)
                        {
                            strGelName = "";
                        }
                        if (nGelFLAG == 1)
                        {
                            nGelFLAG = 0;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strGelName;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strPano;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strSName;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = nAmt1.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nAmt2.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nAmt3.ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = nAmt4.ToString("###,###,###,##0 ");

                        nTotAmt1 += nAmt1;
                        nTotAmt2 += nAmt2;
                        nTotAmt3 += nAmt3;
                        nTotAmt4 += nAmt4;

                        #endregion
                    }
                }

                Misu_Sub_Gel(ref nGelCnt, ref nTotAmt1, ref nTotAmt2, ref nTotAmt3, ref nTotAmt4);

                dt.Dispose();
                dt = null;

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Misu_Sub_Gel(ref int nGelCnt, ref double nTotAmt1, ref double nTotAmt2, ref double nTotAmt3, ref double nTotAmt4)
        {
            ssView_Sheet1.RowCount += 1;
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "계약체계";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = nGelCnt + " 건";
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = nTotAmt1.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nTotAmt2.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nTotAmt3.ToString("###,###,###,##0 ");
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = nTotAmt4.ToString("###,###,###,##0 ");

            nGelCnt = 0;
            nTotAmt1 = 0;
            nTotAmt2 = 0;
            nTotAmt3 = 0;
            nTotAmt4 = 0;
        }

        private void frmPmpaViewGyeMisuPanoPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            GstrGels[0] = "9999";
            cboGel.Items.Add("전   체");

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
