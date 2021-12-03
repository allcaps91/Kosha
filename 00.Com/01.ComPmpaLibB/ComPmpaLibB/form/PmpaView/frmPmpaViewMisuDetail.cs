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
    /// File Name       : frmPmpaViewMisuDetail.cs
    /// Description     : 월말현재미처리미수금명세서
    /// Author          : 박창욱
    /// Create Date     : 2017-11-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-11-01 박창욱 : FrmMisuDetail, FrmMisuDetail_1 폼 통합
    /// </history>
    /// <seealso cref= "\misu\MISUP202_1.FRM(FrmMisuDetail_1) >> frmPmpaViewMisuDetail.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MISUP202.FRM(FrmMisuDetail) >> frmPmpaViewMisuDetail.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuDetail : Form
    {
        string GstrRetValue = "";
        string strYYMM = "";

        public frmPmpaViewMisuDetail()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            txtSName.Text = "";
            txtAmt1.Text = "";
            txtAmt2.Text = "";
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

            if (rdoGubun0.Checked == true)
            {
                strTitle = VB.Left(cboYYMM.Text, 11) + " 현재 미처리 미수금 명세서";
            }
            else if (rdoGubun1.Checked == true)
            {
                strTitle = VB.Left(cboYYMM.Text, 11) + " 현재 미처리 미수금 명세서  발생년[" + cboYear.Text + "]";
            }

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("담당자:" + clsType.User.JobName, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int i = 0;
            string strROWID = "";
            string strRemark = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 1; i < ssView_Sheet1.RowCount; i++)
                {
                    strRemark = ssView_Sheet1.Cells[i - 1, 12].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i - 1, 14].Text.Trim();

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MISU_GAINSLIP SET";
                    SQL = SQL + ComNum.VBLF + "       REMARK = '" + strRemark + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //테스트용
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            int i = 0;
            int j = 0;
            int k = 0;
            int nRow = 0;
            int nMisuCnt = 0;
            int nMisuCnt2 = 0;
            int nCnt = 0;
            double nTotAmt = 0;
            double nIpgumAmt = 0;
            double nTotJAmt = 0;
            double nTotJAmt2 = 0;
            double nJAmt = 0;
            string strOldData = "";
            string strNewData = "";
            string strMisuDtl = "";
            string strRemark = "";
            string strDisp = "";  //미수등급으로 조회 표시여부
            string strJumin1 = "";
            string strJumin2 = "";
            string strMisuGbn = "";
            string strMisuGbn2 = "";
            string strSDate = "";
            string strEDate = "";
            string strJuso = "";

            clsPmpaMisu cpm = new clsPmpaMisu();

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            btnPrint.Enabled = false;

            strMisuGbn = VB.Left(cboMisuGbn.Text, 2);
            strMisuGbn2 = VB.Left(cboMisuGbn2.Text, 2);

            if (VB.Left(cboYear.Text, 4) != "****")
            {
                strSDate = VB.Left(cboYear.Text, 4) + "-01-01";
                strEDate = VB.Left(cboYear.Text, 4) + "-12-31";
            }

            try
            {
                //년도별조회 외래, 월별조회 
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT a.Pano, b.Sname, TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,";
                SQL = SQL + ComNum.VBLF + "        b.Jumin1, b.Jumin2, b.Jumin3,";
                SQL = SQL + ComNum.VBLF + "        a.MisuAmt, a.JanAmt, a.DeptCode,";
                SQL = SQL + ComNum.VBLF + "        a.Gubun, b.JiCode";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINTONG a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND a.YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "    AND a.JanAmt  <> 0";
                SQL = SQL + ComNum.VBLF + "    AND a.Pano = b.Pano";

                //년도별 조회
                if (rdoGubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.BDate >=TO_DATE('" + strSDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND a.BDate <=TO_DATE('" + strEDate + "','YYYY-MM-DD') ";
                }

                //일별 조회
                if (Ck_bdate.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.BDate >=TO_DATE('" + DtpFDate.Text + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND a.BDate <=TO_DATE('" + DtpTDate.Text + "','YYYY-MM-DD') ";
                }

                if (txtSName.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "    AND b.SName Like '%" + txtSName.Text.Trim() + "%' ";
                }

                if (VB.Val(txtAmt1.Text) > 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.MisuAmt >= " + txtAmt1.Text + " ";
                }

                if (VB.Val(txtAmt2.Text) > 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND a.MisuAmt <= " + txtAmt2.Text + " ";
                }

                if (rdoGubun0.Checked == true)
                {
                    if (rdoGbn1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Gubun NOT IN ( '11','13','14','15' )";
                    }
                    if (rdoGbn2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Gubun IN (  '11','13','14','15' )";
                    }
                }
                else if (rdoGubun1.Checked == true)
                {
                    if (rdoGbn1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Gubun NOT IN ( '11','13','14' )";
                    }
                    if (rdoGbn2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND a.Gubun IN (  '11','13','14' )";
                    }
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate,a.Pano,a.MisuAmt DESC";   //수정부분

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnSearch.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count + 1;

                strOldData = "";
                nTotJAmt = 0;
                nMisuCnt = 0;
                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strRemark = "";
                    nCnt = 0;
                    strJumin1 = "";
                    strJumin2 = "";

                    strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                    if (dt.Rows[i]["Jumin3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                    }

                    //합계금액이 0 이면 미수금표시 안 함
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT PANO cPANO, SUM(JANAMT) nJAmt";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINTONG";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY PANO";
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    nJAmt = VB.Val(dt1.Rows[0]["nJAmt"].ToString().Trim());
                    dt1.Dispose();
                    dt1 = null;


                    //미수등급별 조회 로직
                    strDisp = "N";

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT ROWID, MisuDtl ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND Pano  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND Bdate = TO_DATE('" + dt.Rows[i]["Bdate"].ToString().Trim() + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND Gubun1 = '1'";
                    SQL = SQL + ComNum.VBLF + "    AND Amt = " + dt.Rows[i]["MisuAmt"].ToString().Trim();

                    if (rdoGrade1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND (GRADE2 <> '12' OR GRADE2 IS NULL ) ";  //감액제외
                    }
                    if (rdoGrade2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND  GRADE2 =  '12' ";  //감액만
                    }
                    if (rdoGrade3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND (GRADE2 <> '13' OR GRADE2 IS NULL ) ";  //비계약처제외
                    }
                    if (rdoGrade4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND  GRADE2 =  '13' ";  //비계약처 만  "
                    }

                    if (rdoGubun0.Checked == true)
                    {
                        if (strMisuGbn != "**")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUDTL,4,2) = '" + strMisuGbn + "'";
                        }

                        if (rdoIO0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUDTL,2,2) = 'ER'";
                        }
                        else if (rdoIO1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUDTL,1,1) = 'I'";
                        }
                        else if (rdoIO2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUDTL,1,1) = 'O'";
                            SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUDTL,2,2) <> 'ER'";
                        }

                        if (strMisuGbn2 != "**")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND GRADE2 = '" + strMisuGbn2 + "'";
                        }
                    }

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strDisp = "Y";

                        if (rdoGubun1.Checked == true)
                        {
                            strDisp = "N";

                            if (VB.Left(dt1.Rows[0]["MisuDTL"].ToString().Trim(), 1) == "O")
                            {
                                strDisp = "Y";
                            }
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;


                    strJuso = clsVbfunc.GetJUSOJiCode(clsDB.DbCon, dt.Rows[i]["JiCode"].ToString().Trim());

                    //미수금액이 존재하고, 미수등급별로 조회구분을 만족할 경우만 표시
                    if (nJAmt != 0 && strDisp == "Y")
                    {
                        nRow += 1;
                        ssView_Sheet1.RowCount = nRow;
                        nTotJAmt += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());
                        nMisuCnt += 1;

                        strNewData = dt.Rows[i]["Pano"].ToString().Trim();
                        if (strOldData != strNewData)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            strOldData = strNewData;
                        }


                        ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["BDate"].ToString().Trim();


                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT MisuDtl, Remark, IDno,";
                        SQL = SQL + ComNum.VBLF + "        ROWID, GRADE, GRADE2";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND Pano  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Bdate = TO_DATE('" + dt.Rows[i]["Bdate"].ToString().Trim() + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND Gubun1 = '1'";
                        SQL = SQL + ComNum.VBLF + "    AND Amt = " + dt.Rows[i]["MisuAmt"].ToString().Trim();
                        if (rdoGubun0.Checked == true)
                        {
                            if (strMisuGbn != "**")
                            {
                                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(MISUDTL,4,2) = '" + strMisuGbn + "'";
                            }
                            if (strMisuGbn2 != "**")
                            {
                                SQL = SQL + ComNum.VBLF + "    AND GRADE2 = '" + strMisuGbn2 + "'";
                            }
                        }

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strMisuDtl = VB.Left(dt1.Rows[0]["MisuDTL"].ToString().Trim() + VB.Space(30), 30);
                            nTotAmt = VB.Val(VB.Mid(strMisuDtl, 6, 9));
                            nIpgumAmt = nTotAmt - VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());

                            if (nTotAmt != 0)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt.ToString("###,###,###,###,##0 ");
                                ssView_Sheet1.Cells[nRow - 1, 4].Text = nIpgumAmt.ToString("###,###,###,###,##0 ");
                            }
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Left(strMisuDtl, 1);
                            ssView_Sheet1.Cells[nRow - 1, 11].Text = READ_PerMisuGrade_New(dt1.Rows[0]["GRADE2"].ToString().Trim());
                            if (ssView_Sheet1.Cells[nRow - 1, 11].Text != "") { ssView.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(255, 236, 236); }
                            if (dt1.Rows[0]["GRADE2"].ToString().Trim() == "02")
                            {
                                ssView.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(221, 255, 221);
                            }
                            else if (dt1.Rows[0]["GRADE2"].ToString().Trim() == "07")
                            {
                                ssView.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(255, 244, 192);
                            }


                            ssView_Sheet1.Cells[nRow - 1, 12].Text = dt1.Rows[0]["Remark"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = dt1.Rows[0]["Remark"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 16].Text = strJumin1 + "-" + strJumin2;

                            strRemark = dt1.Rows[0]["Remark"].ToString().Trim();

                            for (j = 1; j < strRemark.Length; j++)
                            {
                                switch (VB.Mid(strRemark, j, 1))
                                {
                                    case "\n":
                                        nCnt += 1;
                                        break;
                                }
                            }

                            if (nCnt == 0)
                            {
                                nCnt = (int)(strRemark.Length / 45);
                            }

                            if (nCnt <= 1)
                            {
                                if (strRemark.Length < 45)
                                {
                                    nCnt = 1;
                                }
                                if (strRemark.Length > 45)
                                {
                                    nCnt = 2;
                                }
                            }

                            ssView_Sheet1.SetRowHeight(nRow - 1, ComNum.SPDROWHT * nCnt);

                            ssView_Sheet1.Cells[nRow - 1, 13].Text = clsVbfunc.GetPassName(clsDB.DbCon, VB.Val(dt1.Rows[0]["IDno"].ToString().Trim()).ToString("####0"));
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = dt1.Rows[0]["ROWID"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;

                        ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = (VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim())).ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = cpm.READ_PerMisuGye(dt.Rows[i]["Gubun"].ToString().Trim()).Trim();
                        //ssView_Sheet1.SetRowHeight(nRow - 1, ssView_Sheet1.GetRowHeight(nRow - 1) + 5);

                        byte[] a = System.Text.Encoding.Default.GetBytes(ssView_Sheet1.Cells[nRow - 1, 12].Text);
                        int intHeight = Convert.ToInt32(a.Length / 50);

                        ssView_Sheet1.SetRowHeight(nRow - 1, ComNum.SPDROWHT + (intHeight * 18));

                        //참고사항 Display
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT Remark, IDno, ROWID";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE Pano  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Gubun1 = '9'";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        for (k = 0; k < dt1.Rows.Count; k++)
                        {
                            strRemark = "";
                            nCnt = 0;
                            nRow += 1;
                            ssView_Sheet1.RowCount = nRow;
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = "적요계속";
                            ssView_Sheet1.Cells[nRow - 1, 12].Text = dt1.Rows[k]["Remark"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = dt1.Rows[k]["Remark"].ToString().Trim();
                            strRemark = dt1.Rows[k]["Remark"].ToString().Trim();

                            for (j = 1; j < strRemark.Length; j++)
                            {
                                switch (VB.Mid(strRemark, j, 1))
                                {
                                    case "\n":
                                        nCnt += 1;
                                        break;
                                }
                            }

                            if (nCnt == 0)
                            {
                                nCnt = 1;
                            }

                            ssView_Sheet1.SetRowHeight(nRow - 1, ssView_Sheet1.GetRowHeight(nRow - 1) * nCnt);
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = dt1.Rows[k]["ROWID"].ToString().Trim();
                            //ssView_Sheet1.SetRowHeight(nRow - 1, ssView_Sheet1.GetRowHeight(nRow - 1) + 5);

                            a = System.Text.Encoding.Default.GetBytes(ssView_Sheet1.Cells[nRow - 1, 12].Text);
                            intHeight = Convert.ToInt32(a.Length / 50);

                            ssView_Sheet1.SetRowHeight(nRow - 1, ComNum.SPDROWHT + (intHeight * 18));
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }



                ssView_Sheet1.RowCount = nRow + 1;
                if (rdoGubun0.Checked == true)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "** 합계 **";
                }
                else if (rdoGubun1.Checked == true)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "** 외래합계 **";
                }
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nMisuCnt.ToString("###,##0 ") + "건 ";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nTotJAmt.ToString("###,###,###,##0 ");







                if (rdoGubun1.Checked == true)
                {
                    //년도별 조회. 입원
                    nMisuCnt2 = nMisuCnt;
                    nTotJAmt2 = nTotJAmt;
                    nRow += 1;

                    strOldData = "";
                    nTotJAmt = 0;
                    nMisuCnt = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRemark = "";
                        nCnt = 0;

                        //합계금액이 0 이면 미수금 표시 안 함
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT PANO cPANO, SUM(JANAMT) nJAmt";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINTONG";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "    AND YYMM = '" + strYYMM + "'";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnSearch.Enabled = true;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        nJAmt = VB.Val(dt1.Rows[0]["nJAmt"].ToString().Trim());

                        dt1.Dispose();
                        dt1 = null;


                        //미수등급별 조회 로직
                        strDisp = "N";
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT ROWID, MisuDtl ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND Pano  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Bdate = TO_DATE('" + dt.Rows[i]["Bdate"].ToString().Trim() + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "    AND Gubun1 = '1'";
                        SQL = SQL + ComNum.VBLF + "    AND Amt = " + dt.Rows[i]["MisuAmt"].ToString().Trim();

                        if (rdoGrade1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " AND (GRADE2 <> '12' OR GRADE2 IS NULL ) ";  //감액제외
                        }
                        if (rdoGrade2.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " AND  GRADE2 =  '12' ";  //감액만
                        }
                        if (rdoGrade3.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " AND (GRADE2 <> '13' OR GRADE2 IS NULL ) ";  //비계약처제외
                        }
                        if (rdoGrade4.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " AND  GRADE2 =  '13' ";  //비계약처 만  "
                        }

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            strDisp = "Y";

                            if (rdoGubun1.Checked == true)
                            {
                                strDisp = "N";

                                if (VB.Left(dt1.Rows[0]["MisuDTL"].ToString().Trim(), 1) == "I")
                                {
                                    strDisp = "Y";
                                }
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;


                        strJuso = clsVbfunc.GetJUSOJiCode(clsDB.DbCon, dt.Rows[i]["JiCode"].ToString().Trim());

                        //미수금액이 존재하고, 미수등급별로 조회구분을 만족할 경우만 표시
                        if (nJAmt != 0 && strDisp == "Y")
                        {
                            nRow += 1;
                            ssView_Sheet1.RowCount = nRow;
                            nTotJAmt += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim());
                            nMisuCnt += 1;

                            strNewData = dt.Rows[i]["Pano"].ToString().Trim();
                            if (strOldData != strNewData)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                                strOldData = strNewData;
                            }


                            ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["BDate"].ToString().Trim();


                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT MisuDtl, Remark, IDno,";
                            SQL = SQL + ComNum.VBLF + "        ROWID, GRADE, GRADE2";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                            SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                            SQL = SQL + ComNum.VBLF + "    AND Pano  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "    AND Bdate = TO_DATE('" + dt.Rows[i]["Bdate"].ToString().Trim() + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "    AND Gubun1 = '1'";
                            SQL = SQL + ComNum.VBLF + "    AND Amt = " + dt.Rows[i]["MisuAmt"].ToString().Trim();

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                Cursor.Current = Cursors.Default;
                                btnSearch.Enabled = true;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                strMisuDtl = VB.Left(dt1.Rows[0]["MisuDTL"].ToString().Trim() + VB.Space(30), 30);
                                nTotAmt = VB.Val(VB.Mid(strMisuDtl, 6, 9));
                                nIpgumAmt = nTotAmt - VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim());

                                if (nTotAmt != 0)
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 3].Text = nTotAmt.ToString("###,###,###,###,##0 ");
                                    ssView_Sheet1.Cells[nRow - 1, 4].Text = nIpgumAmt.ToString("###,###,###,###,##0 ");
                                }
                                ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Left(strMisuDtl, 1);
                                ssView_Sheet1.Cells[nRow - 1, 11].Text = READ_PerMisuGrade_New(dt1.Rows[0]["GRADE2"].ToString().Trim());
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt1.Rows[0]["Remark"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 15].Text = dt1.Rows[0]["Remark"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 16].Text = strJumin1 + "-" + strJumin2;

                                strRemark = dt1.Rows[0]["Remark"].ToString().Trim();

                                for (j = 1; j < strRemark.Length; j++)
                                {
                                    switch (VB.Mid(strRemark, j, 1))
                                    {
                                        case "\n":
                                            nCnt += 1;
                                            break;
                                    }
                                }

                                if (nCnt == 0)
                                {
                                    nCnt = (int)(strRemark.Length / 45);
                                }

                                if (nCnt <= 1)
                                {
                                    if (strRemark.Length < 45)
                                    {
                                        nCnt = 1;
                                    }
                                    if (strRemark.Length > 45)
                                    {
                                        nCnt = 2;
                                    }
                                }

                                ssView_Sheet1.SetRowHeight(nRow - 1, ComNum.SPDROWHT * nCnt);

                                ssView_Sheet1.Cells[nRow - 1, 13].Text = clsVbfunc.GetPassName(clsDB.DbCon, VB.Val(dt1.Rows[0]["IDno"].ToString().Trim()).ToString("####0"));
                                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt1.Rows[0]["ROWID"].ToString().Trim();
                            }
                            dt1.Dispose();
                            dt1 = null;

                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = (VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim())).ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0 ");
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = cpm.READ_PerMisuGye(dt.Rows[i]["Gubun"].ToString().Trim()).Trim();
                            //ssView_Sheet1.SetRowHeight(nRow - 1, ssView_Sheet1.GetRowHeight(nRow - 1) + 5);

                            byte[] a = System.Text.Encoding.Default.GetBytes(ssView_Sheet1.Cells[nRow - 1, 12].Text);
                            int intHeight = Convert.ToInt32(a.Length / 50);

                            ssView_Sheet1.SetRowHeight(nRow - 1, ComNum.SPDROWHT + (intHeight * 18));

                            //참고사항 Display
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " SELECT Remark, IDno, ROWID";
                            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                            SQL = SQL + ComNum.VBLF + "  WHERE Pano  = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "    AND Gubun1 = '9'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                Cursor.Current = Cursors.Default;
                                btnSearch.Enabled = true;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = "";
                                nCnt = 0;
                                nRow += 1;
                                ssView_Sheet1.RowCount = nRow;
                                ssView_Sheet1.Cells[nRow - 1, 10].Text = "적요계속";
                                ssView_Sheet1.Cells[nRow - 1, 12].Text = dt1.Rows[k]["Remark"].ToString().Trim();
                                ssView_Sheet1.Cells[nRow - 1, 15].Text = dt1.Rows[k]["Remark"].ToString().Trim();
                                strRemark = dt1.Rows[k]["Remark"].ToString().Trim();

                                for (j = 1; j < strRemark.Length; j++)
                                {
                                    switch (VB.Mid(strRemark, j, 1))
                                    {
                                        case "\n":
                                            nCnt += 1;
                                            break;
                                    }
                                }

                                if (nCnt == 0)
                                {
                                    nCnt = 1;
                                }

                                ssView_Sheet1.SetRowHeight(nRow - 1, ssView_Sheet1.GetRowHeight(nRow - 1) * nCnt);
                                ssView_Sheet1.Cells[nRow - 1, 14].Text = dt1.Rows[k]["ROWID"].ToString().Trim();
                                //ssView_Sheet1.SetRowHeight(nRow - 1, ssView_Sheet1.GetRowHeight(nRow - 1) + 5);

                                a = System.Text.Encoding.Default.GetBytes(ssView_Sheet1.Cells[nRow - 1, 12].Text);
                                intHeight = Convert.ToInt32(a.Length / 50);

                                ssView_Sheet1.SetRowHeight(nRow - 1, ComNum.SPDROWHT + (intHeight * 18));
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }

                    }

                    ssView_Sheet1.RowCount = nRow + 1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "** 입원합계 **";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nMisuCnt.ToString("###,##0 ") + "건 ";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nTotJAmt.ToString("###,###,###,##0 ");

                    nRow += 1;

                    ssView_Sheet1.RowCount = nRow + 1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = "** 전체합계 **";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (nMisuCnt + nMisuCnt2).ToString("###,##0 ") + "건 ";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = (nTotJAmt + nTotJAmt2).ToString("###,###,###,##0 ");
                }




                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        string READ_PerMisuGrade_New(string argGrade)
        {
            string rtnVal = "";

            switch (VB.Val(argGrade).ToString("00"))
            {
                case "01":
                    rtnVal = "채권사의뢰검토";
                    break;
                case "02":
                    rtnVal = "집행불능";
                    break;
                case "03":
                    rtnVal = "문제환자";
                    break;
                case "04":
                    rtnVal = "대불접수";
                    break;
                case "05":
                    rtnVal = "대불불능";
                    break;
                case "06":
                    rtnVal = "대장관리";
                    break;
                case "07":
                    rtnVal = "소액미수";
                    break;
                case "08":
                    rtnVal = "의료분쟁";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }

        private void frmPmpaViewMisuDetail_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strYYMM = "";

            strYYMM = VB.Left(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), 4) +
                      VB.Mid(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), 6, 2);

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            cboYYMM.SelectedIndex = 1;

            clsVbfunc.SetCboDateYY(clsDB.DbCon, cboYear, 12, "2");
            cboYear.SelectedIndex = 1;

            cboMisuGbn.Items.Clear();
            cboMisuGbn.Items.Add("**.전체");
            cboMisuGbn.Items.Add("01.가퇴원미수");
            cboMisuGbn.Items.Add("02.업무착오미수");
            cboMisuGbn.Items.Add("03.탈원미수");
            cboMisuGbn.Items.Add("04.지불각서");
            cboMisuGbn.Items.Add("05.응급미수");
            cboMisuGbn.Items.Add("06.외래미수");
            cboMisuGbn.Items.Add("07.심사청구미수");
            cboMisuGbn.Items.Add("08.책임보험");
            cboMisuGbn.Items.Add("09.퇴원");
            cboMisuGbn.Items.Add("10.기타");
            cboMisuGbn.Items.Add("11.기관청구미수");
            cboMisuGbn.Items.Add("12.입원정밀");
            cboMisuGbn.Items.Add("13.필수접종국가지원");
            cboMisuGbn.Items.Add("14.회사접종");
            cboMisuGbn.Items.Add("15.금연처방");
            cboMisuGbn.SelectedIndex = 0;

            cboMisuGbn2.Items.Clear();
            cboMisuGbn2.Items.Add("**.전체");
            cboMisuGbn2.Items.Add("01.채권사의뢰검토");
            cboMisuGbn2.Items.Add("02.집행불능");
            cboMisuGbn2.Items.Add("03.문제환자");
            cboMisuGbn2.Items.Add("04.대불접수");
            cboMisuGbn2.Items.Add("05.대불불능");
            cboMisuGbn2.Items.Add("06.대장관리");
            cboMisuGbn2.Items.Add("07.소액미수");
            cboMisuGbn2.Items.Add("08.의료분쟁");
            cboMisuGbn2.SelectedIndex = 0;

            txtSName.Text = "";
            txtAmt1.Text = "";
            txtAmt2.Text = "";

            ssView_Sheet1.Columns[14].Visible = false;
            ssView_Sheet1.Columns[15].Visible = false;
            ssView_Sheet1.Columns[16].Visible = false;

            cboYear.Enabled = false;

            //if (clsType.User.Sabun == "468" || clsType.User.Sabun == "7834")
            //{
            //    ssView_Sheet1.Columns[11].Locked = true;
            //    ssView_Sheet1.Columns[12].Locked = true;
            //}

            DtpFDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString();
            DtpTDate.Text = DateTime.Parse(clsPublic.GstrSysDate).ToShortDateString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.ActiveRow.Height += ComNum.SPDROWHT;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.ActiveRow.Height >= ComNum.SPDROWHT)
            {
                ssView_Sheet1.ActiveRow.Height -= ComNum.SPDROWHT;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            GstrRetValue = "";

            //TODO:폼호출
            //FrmSMS_Misu.Show 1
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strName = "";
            string strPano = "";
            GstrRetValue = "";

            strPano = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            strName = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT HPhone, Tel";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "Bas_Patient";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + strPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    GstrRetValue = dt.Rows[0]["HPhone"].ToString().Trim();
                    if (GstrRetValue == "")
                    {
                        GstrRetValue = dt.Rows[0]["Tel"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                GstrRetValue += "^^" + strName;

                //TODO : 폼 호출
                //FrmSMS_Misu.Show 1

                GstrRetValue = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true && (e.Column == 0 || e.Column == 2 || e.Column == 12))
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAmt1.Focus();
            }
        }

        private void txtAmt1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAmt2.Focus();
            }
        }

        private void txtAmt2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }

        private void rdoGubun_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGubun0.Checked == true)
            {
                cboYear.Enabled = false;
                grbIO.Enabled = true;
                cboMisuGbn.Enabled = true;
                cboMisuGbn2.Enabled = true;
            }
            if (rdoGubun1.Checked == true)
            {
                cboYear.Enabled = true;
                grbIO.Enabled = false;
                cboMisuGbn.Enabled = false;
                cboMisuGbn2.Enabled = false;
            }
        }
    }
}
