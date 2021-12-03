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
    /// File Name       : frmPmpaViewGyeData.cs
    /// Description     : 계약처 진료내역 현황
    /// Author          : 박창욱
    /// Create Date     : 2017-10-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MISUS210.FRM(FrmDataView.frm) >> frmPmpaViewGyeData.cs 폼이름 재정의" />	
    public partial class frmPmpaViewGyeData : Form
    {
        public frmPmpaViewGyeData()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SS_Clear();
        }

        void SS_Clear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 200;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 200;
            ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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

            if (rdoGubun0.Checked == true)
            {
                strTitle = "입원 계약처 감액 현황";
            }
            else
            {
                strTitle = "외래 계약처 감액 현황";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력자:" + clsType.User.JobName + " 인 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 70, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.8f);

            if (rdoGubun0.Checked == true)
            {
                CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
            }

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
            double nTotal = 0;
            double nToAmt = 0;
            double nToAmt1 = 0;
            double nSubAmt = 0;
            double nSubAmt1 = 0;
            double nSubAmt2 = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strActDate = "";
            string strMiaName = "";
            string strSname = "";
            string strBi = "";
            string strDrName = "";
            string strDrDept = "";
            string strJong = "";

            SS_Clear();

            nTotal = 0;
            nToAmt = 0;
            nToAmt1 = 0;
            nSubAmt1 = 0;
            nSubAmt2 = 0;
            nSubAmt = 0;

            strFDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strTDate = dtpTDate.Value.ToString("yyyy-MM-dd");

            switch (VB.Left(cboJong.Text, 1))
            {
                case "1":
                    strJong = "('11','12','13','44')";  //보험
                    break;
                case "2":
                    strJong = "('31')";  //산재
                    break;
                case "3":
                    strJong = "('33')";  //산재공상
                    break;
                case "4":
                    strJong = "('41','42','43')";  //건강보험총액
                    break;
                case "5":
                    strJong = "('51') ";  //일반
                    break;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (rdoGubun0.Checked == true)
                {
                    //입원 계약처
                    #region Ipd_TM_Select

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(T.InDate,'YYYY-MM-DD') INDATE, TO_CHAR(T.OutDate,'YYYY-MM-DD') OUTDATE, B.MIANAME,";
                    SQL = SQL + ComNum.VBLF + "        A.PANO, A.SNAME, A.DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + "        A.BI, C.DRNAME, SUM(T.AMT50) AMT50,";
                    SQL = SQL + ComNum.VBLF + "        SUM(T.AMT54) AMT54, SUM(T.AMT55) TOT, SUM(T.ILSU) ILSU";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "IPD_TRANS T,";
                    SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_MIA B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND T.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND T.ActDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = T.IPDNO";
                    SQL = SQL + ComNum.VBLF + "    AND A.GbGamek = '55'";
                    if (VB.Left(cboJong.Text, 1) != "0")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.BI IN " + strJong + "";
                    }
                    SQL = SQL + ComNum.VBLF + "    And T.AMT54 <> 0"; //감액이 있는경우만
                    SQL = SQL + ComNum.VBLF + "    AND A.GELCODE = B.MIACODE(+)";
                    SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = C.DRCODE";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY T.INDATE, T.OUTDATE, B.MIANAME, A.PANO, A.SNAME, A.DEPTCODE, A.BI, C.DRNAME";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nREAD = dt.Rows.Count;
                        ssView2_Sheet1.RowCount = nREAD + 1;
                        k = 1;

                        for (i = 0; i < nREAD; i++)
                        {
                            ssView2_Sheet1.Cells[k - 1, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 1].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 2].Text = dt.Rows[i]["MIANAME"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 3].Text = "입원";
                            ssView2_Sheet1.Cells[k - 1, 4].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 6].Text = dt.Rows[i]["BI"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView2_Sheet1.Cells[k - 1, 8].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                            nSubAmt = VB.Val(dt.Rows[i]["AMT50"].ToString().Trim());
                            nSubAmt1 = VB.Val(dt.Rows[i]["AMT54"].ToString().Trim());
                            nSubAmt2 = VB.Fix((int)VB.Val(dt.Rows[i]["TOT"].ToString().Trim()) / 10) * 10;

                            nTotal += nSubAmt;
                            nToAmt += nSubAmt1;
                            nToAmt1 += nSubAmt2;

                            ssView2_Sheet1.Cells[k - 1, 9].Text = nSubAmt.ToString("###,###,###,### ");
                            ssView2_Sheet1.Cells[k - 1, 10].Text = nSubAmt1.ToString("###,###,###,### ");
                            ssView2_Sheet1.Cells[k - 1, 11].Text = nSubAmt2.ToString("###,###,###,### ");

                            k += 1;
                        }

                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = "총   액";
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 9].Text = nTotal.ToString("###,###,###,### ");
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 10].Text = nToAmt.ToString("###,###,###,### ");
                        ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 11].Text = nToAmt1.ToString("###,###,###,### ");
                    }
                    dt.Dispose();
                    dt = null;


                    #endregion
                }
                else if (rdoGubun1.Checked == true)
                {
                    //외래 계약처
                    #region Opd_Select

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , A.PANO ,A.SNAME,";
                    SQL = SQL + ComNum.VBLF + "        A.BI,B.MIANAME, D.DRCODE, C.DRNAME, D.DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(D.Bun,'92',0,'96',0,'98',0,'99',0,D.Amt1+D.Amt2)) TAmt,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(D.Bun,'92',D.Amt1+D.Amt2,0)) Bun92,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(D.Bun,'96',D.Amt1+D.Amt2,0)) Bun96,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(D.Bun,'98',D.Amt1+D.Amt2,0)) Bun98,";
                    SQL = SQL + ComNum.VBLF + "        SUM(DECODE(D.Bun,'99',D.Amt1+D.Amt2,0)) Bun99";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_MIA B,";
                    SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_DOCTOR C, " + ComNum.DB_PMPA + "OPD_SLIP D";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND A.ActDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND A.ActDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                    if (VB.Left(cboJong.Text, 1) != "0")
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.BI IN " + strJong;
                    }
                    SQL = SQL + ComNum.VBLF + "    AND (A.GBGAMEK = '55' OR A.GBGAMEK = 'F')";
                    SQL = SQL + ComNum.VBLF + "    AND A.GELCODE = B.MIACODE";
                    SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = D.ACTDATE";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = D.PANO";
                    SQL = SQL + ComNum.VBLF + "    AND A.BI = D.BI";
                    SQL = SQL + ComNum.VBLF + "    AND D.DRCODE = C.DRCODE";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.ACTDATE, A.PANO,A.BI,A.SNAME ,B.MIANAME, D.DRCODE, C.DRNAME, D.DEPTCODE";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nREAD = dt.Rows.Count;
                        for (i = 0; i < nREAD; i++)
                        {
                            strActDate = dt.Rows[i]["ACTDATE"].ToString().Trim();
                            strPANO = dt.Rows[i]["PANO"].ToString().Trim();
                            strMiaName = dt.Rows[i]["MIANAME"].ToString().Trim();
                            strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                            strBi = dt.Rows[i]["Bi"].ToString().Trim();
                            strDrName = dt.Rows[i]["DRNAME"].ToString().Trim();
                            strDrDept = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                            nSubAmt = VB.Val(dt.Rows[i]["TAMT"].ToString().Trim());
                            nSubAmt1 = VB.Val(dt.Rows[i]["BUN92"].ToString().Trim());
                            nSubAmt2 = VB.Val(dt.Rows[i]["BUN99"].ToString().Trim());

                            nTotal += nSubAmt;
                            nToAmt += nSubAmt1;
                            nToAmt1 += nSubAmt2;

                            ssView_Sheet1.Cells[i, 0].Text = strActDate;
                            ssView_Sheet1.Cells[i, 1].Text = strMiaName;
                            ssView_Sheet1.Cells[i, 2].Text = "외래";
                            ssView_Sheet1.Cells[i, 3].Text = strPANO;
                            ssView_Sheet1.Cells[i, 4].Text = strSname;
                            ssView_Sheet1.Cells[i, 5].Text = strBi;
                            ssView_Sheet1.Cells[i, 6].Text = strDrDept;
                            ssView_Sheet1.Cells[i, 7].Text = strDrName;
                            ssView_Sheet1.Cells[i, 8].Text = nSubAmt.ToString("###,###,###,### ");
                            ssView_Sheet1.Cells[i, 9].Text = nSubAmt1.ToString("###,###,###,### ");
                            ssView_Sheet1.Cells[i, 10].Text = nSubAmt2.ToString("###,###,###,### ");
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "총   액";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nTotal.ToString("###,###,###,### ");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nToAmt.ToString("###,###,###,### ");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = nToAmt1.ToString("###,###,###,### ");

                    #endregion
                }
                else
                {
                    //재원 계약처
                    #region Ipd_Master_Select

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.InDate,'YYYY-MM-DD') INDATE, TO_CHAR(A.OutDate,'YYYY-MM-DD') OUTDATE, B.MIANAME,";
                    SQL = SQL + ComNum.VBLF + "        A.PANO, A.SNAME, A.DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + "        A.BI, C.DRNAME, SUM(T.AMT50) AMT50,";
                    SQL = SQL + ComNum.VBLF + "        SUM(T.AMT54) AMT54, SUM(T.AMT55) TOT, SUM(A.ILSU) ILSU";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "IPD_TRANS T,";
                    SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_MIA B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND A.GbGamek = '55'";
                    if (VB.Left(cboJong.Text, 1) != "0")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.BI IN " + strJong + "";
                    }
                    SQL = SQL + ComNum.VBLF + "    AND A.GELCODE = B.MIACODE(+)";
                    SQL = SQL + ComNum.VBLF + "    AND A.OUTDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = C.DRCODE";
                    SQL = SQL + ComNum.VBLF + "    AND A.LASTTRS = T.TRSNO";
                    SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = T.IPDNO";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY A.INDATE, A.OUTDATE, B.MIANAME, A.PANO, A.SNAME, A.DEPTCODE, A.BI, C.DRNAME";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nREAD = dt.Rows.Count;
                        ssView2_Sheet1.RowCount = nREAD + 1;
                        k = 1;
                        ssView2_Sheet1.Cells[k - 1, 0].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        ssView2_Sheet1.Cells[k - 1, 2].Text = dt.Rows[i]["MIANAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[k - 1, 3].Text = "입원";
                        ssView2_Sheet1.Cells[k - 1, 4].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[k - 1, 5].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView2_Sheet1.Cells[k - 1, 6].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssView2_Sheet1.Cells[k - 1, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[k - 1, 8].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        nSubAmt = VB.Val(dt.Rows[i]["AMT50"].ToString().Trim());
                        nSubAmt1 = VB.Val(dt.Rows[i]["AMT54"].ToString().Trim());
                        nSubAmt2 = VB.Fix((int)VB.Val(dt.Rows[i]["TOT"].ToString().Trim()) / 10) * 10;

                        nTotal += nSubAmt;
                        nToAmt += nSubAmt1;
                        nToAmt1 += nSubAmt2;

                        ssView2_Sheet1.Cells[k - 1, 9].Text = nSubAmt.ToString("###,###,###,### ");
                        ssView2_Sheet1.Cells[k - 1, 10].Text = nSubAmt1.ToString("###,###,###,### ");
                        ssView2_Sheet1.Cells[k - 1, 11].Text = nSubAmt2.ToString("###,###,###,### ");
                        k += 1;
                    }
                    dt.Dispose();
                    dt = null;

                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 8].Text = "총   액";
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 9].Text = nTotal.ToString("###,###,###,### ");
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 10].Text = nToAmt.ToString("###,###,###,### ");
                    ssView2_Sheet1.Cells[ssView2_Sheet1.RowCount - 1, 11].Text = nToAmt1.ToString("###,###,###,### ");

                    #endregion
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewGyeData_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-30);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-1);

            cboJong.Items.Clear();
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.산재");
            cboJong.Items.Add("3.산재공상");
            cboJong.Items.Add("4.건강보험총액");
            cboJong.Items.Add("5.일반");
            cboJong.SelectedIndex = 1;

            ssView.Dock = DockStyle.Fill;
            ssView2.Dock = DockStyle.Fill;
            ssView2.Visible = true;
            ssView.Visible = false;
            lblDate.Text = "퇴원일자";
            dtpFDate.Enabled = true;
            dtpTDate.Enabled = true;
        }

        private void rdoGubun_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGubun0.Checked == true)
            {
                ssView2.Visible = true;
                ssView.Visible = false;
                lblDate.Text = "퇴원일자";
                dtpFDate.Enabled = true;
                dtpTDate.Enabled = true;
            }
            else if (rdoGubun1.Checked == true)
            {
                ssView2.Visible = false;
                ssView.Visible = true;
                lblDate.Text = "수납일자";
                dtpFDate.Enabled = true;
                dtpTDate.Enabled = true;
            }
            else
            {
                ssView2.Visible = true;
                ssView.Visible = false;
                lblDate.Text = "입원일자";
                dtpFDate.Enabled = false;
                dtpTDate.Enabled = false;
            }
        }

        private void ssView2_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView2, e.Column);
                return;
            }

            ssView2_Sheet1.Cells[0, 0, ssView2_Sheet1.RowCount - 1, ssView2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView2_Sheet1.Cells[e.Row, 0, e.Row, ssView2_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }
    }
}
