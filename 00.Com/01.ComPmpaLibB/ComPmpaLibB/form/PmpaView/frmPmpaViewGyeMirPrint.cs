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
    /// File Name       : frmPmpaViewGyeMirPrint.cs
    /// Description     : 계약처 진료비 청구 현황
    /// Author          : 박창욱
    /// Create Date     : 2017-10-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-10-13 박창욱 : \misu\MISUR202.FRM(FrmMirPrint2.frm) 폼을 \misu\MISUG303.FRM(FrmMirPrint1.frm) 폼에 통합
    /// </history>
    /// <seealso cref= "\misu\MISUG303.FRM(FrmMirPrint1.frm) >> frmPmpaViewGyeMirPrint.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MISUR202.FRM(FrmMirPrint2.frm) >> frmPmpaViewGyeMirPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeMirPrint : Form
    {
        clsPmpaMisu cpm = new clsPmpaMisu();
        clsPmpaFunc cpf = new clsPmpaFunc();

        double[,] nTotAmt = new double[6, 3];

        public frmPmpaViewGyeMirPrint()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            btnSearch.Enabled = true;
            btnPrint.Enabled = false;
            btnCancel.Enabled = false;
            cboYYMM.Focus();
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

            strTitle = " 계약처 진료비 청구 현황 ";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("담당자:" + clsType.User.JobName + " 인 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.8f);

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

            int k = 0;
            int nREAD = 0;
            int nRow = 0;
            double nAmt = 0;
            string strOldData = "";
            string strNewData = "";
            string strMiaName = "";
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;


            for (i = 0; i < 6; i++)
            {
                for (k = 0; k < 3; k++)
                {
                    nTotAmt[i, k] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-" + "01";
            strTDate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strYYMM, 4)), Convert.ToInt32(VB.Right(strYYMM, 2)));

            try
            {
                //해당기간 자보 청구명단 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.Class, A.GelCode, A.MisuID,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.Bdate,'YYYY-MM-DD') Bdate, A.IpdOpd, A.DeptCode,";
                SQL = SQL + ComNum.VBLF + "        A.Amt2, B.SNAME, TO_CHAR(A.FromDate,'YYYY-MM-DD') Fdate,";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.ToDate,'YYYY-MM-DD') Tdate";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND A.BDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.BDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND A.MisuID = B.PANO";
                if (VB.Left(cboClass.Text, 2) == "00")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.Class >= '08'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.Class = '" + VB.Left(cboClass.Text, 2) + "'";
                }
                if (VB.Left(cboClass.Text, 2) == "08")
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY GelCode,BDate,MisuID,FromDate";
                }
                else if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Class, MisuID, GelCode,FromDate";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Class,BDATE, GelCode,MisuID,FromDate";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY Class,SNAME, GelCode,MisuID,FromDate";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = false;
                    btnCancel.Enabled = true;
                    btnPrint.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = false;
                    btnCancel.Enabled = true;
                    btnPrint.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    #region Display_Rtn
                    //1명을 Display

                    strNewData = VB.Left(dt.Rows[i]["Class"].ToString().Trim() + VB.Space(2), 2);
                    strNewData += VB.Left(dt.Rows[i]["GelCode"].ToString().Trim() + VB.Space(8), 8);
                    if (strNewData != strOldData)
                    {
                        SubTot_Display(strOldData, strNewData, ref strMiaName, ref nRow);
                    }

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    if (VB.Left(strNewData, 2) != VB.Left(strOldData, 2))
                    {
                        strOldData = strNewData;
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = cpm.READ_MisuClass(VB.Left(strNewData, 2));
                        if (ssView_Sheet1.Cells[nRow - 1, 0].Text =="개인")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = "계약처";
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = strMiaName.Trim();
                    }
                    else if (strNewData != strOldData)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = strMiaName.Trim();
                    }

                    strOldData = strNewData;
                    nAmt = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = cpf.Get_BasPatient(clsDB.DbCon, dt.Rows[i]["MisuID"].ToString().Trim()).Rows[0]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    switch (dt.Rows[i]["IpdOpd"].ToString().Trim())
                    {
                        case "O":
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = "외래";
                            nTotAmt[4, 1] += 1;
                            nTotAmt[4, 2] += nAmt;
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = "입원";
                            nTotAmt[5, 1] += 1;
                            nTotAmt[5, 2] += nAmt;
                            break;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Fdate"].ToString().Trim() + "=>" + dt.Rows[i]["Tdate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nAmt.ToString("###,###,###,##0 ");

                    nTotAmt[1, 1] += 1;
                    nTotAmt[1, 2] += nAmt;
                    nTotAmt[2, 1] += 1;
                    nTotAmt[2, 2] += nAmt;
                    nTotAmt[3, 1] += 1;
                    nTotAmt[3, 2] += nAmt;

                    #endregion
                }
                dt.Dispose();
                dt = null;

                SubTot_Display(strOldData, strNewData, ref strMiaName, ref nRow);
                Total_Display(ref nRow);

                if (VB.Left(cboClass.Text, 2) == "00")
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = "** 전체 합계 **";
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Right(VB.Space(4) + nTotAmt[3, 1].ToString("###0"), 4) + " 건";
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[3, 2].ToString("###,###,###,##0 ");
                    nTotAmt[3, 1] = 0;
                    nTotAmt[3, 2] = 0;
                }
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = false;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = false;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SubTot_Display(string strOldData, string strNewData, ref string strMiaName, ref int nRow)
        {
            if (cpf.GET_BAS_MIA(clsDB.DbCon, VB.Right(strNewData, 8)) == "")
            {
                strMiaName = "-< ERROR >-";
            }
            else
            {
                strMiaName = cpf.GET_BAS_MIA(clsDB.DbCon, VB.Right(strNewData, 8));
            }

            if (strOldData == "")
            {
                return;
            }

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 1, 7].Text = "** 계약처 소계 **";
            ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Right(VB.Space(4) + nTotAmt[1, 1].ToString("###0"), 4) + " 건";
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[1, 2].ToString("###,###,###,##0 ");
            nTotAmt[1, 1] = 0;
            nTotAmt[1, 2] = 0;

            if (VB.Left(strOldData, 2) != VB.Left(strNewData, 2))
            {
                Total_Display(ref nRow);
            }
        }

        void Total_Display(ref int nRow)
        {
            nRow += 3;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }

            ssView_Sheet1.Cells[nRow - 3, 7].Text = "** 외 래 **";
            ssView_Sheet1.Cells[nRow - 3, 3].Text = VB.Right(VB.Space(4) + nTotAmt[4, 1].ToString("###0"), 4) + " 건";
            ssView_Sheet1.Cells[nRow - 3, 8].Text = nTotAmt[4, 2].ToString("###,###,###,##0 ");

            ssView_Sheet1.Cells[nRow - 2, 7].Text = "** 입 원 **";
            ssView_Sheet1.Cells[nRow - 2, 3].Text = VB.Right(VB.Space(4) + nTotAmt[5, 1].ToString("###0"), 4) + " 건";
            ssView_Sheet1.Cells[nRow - 2, 8].Text = nTotAmt[5, 2].ToString("###,###,###,##0 ");

            ssView_Sheet1.Cells[nRow - 1, 7].Text = "** 종류별 합계 **";
            ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Right(VB.Space(4) + nTotAmt[2, 1].ToString("###0"), 4) + " 건";
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTotAmt[2, 2].ToString("###,###,###,##0 ");

            nTotAmt[2, 1] = 0;
            nTotAmt[2, 2] = 0;
            nTotAmt[4, 1] = 0;
            nTotAmt[4, 2] = 0;
            nTotAmt[5, 1] = 0;
            nTotAmt[5, 2] = 0;
        }

        private void frmPmpaViewGyeMirPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");

            cboYYMM.SelectedIndex = 0;

            cboClass.Items.Clear();
            cboClass.Items.Add("00.전체미수");
            cboClass.Items.Add("08.계약처");
            cboClass.Items.Add("09.헌혈미수");
            cboClass.Items.Add("11.보훈청미수");
            cboClass.Items.Add("12.시각장애자");
            cboClass.Items.Add("13.심신장애진단비");
            cboClass.Items.Add("14.장애인보장구");
            cboClass.Items.Add("15.직원대납");
            cboClass.Items.Add("16.노인장기요양소견서");
            cboClass.Items.Add("17.방문간호지시서");
            cboClass.Items.Add("18.치매검사");
            cboClass.SelectedIndex = 0;
        }
    }
}
