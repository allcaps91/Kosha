using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPanoMir.cs
    /// Description     : 등록번호별 청구내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs72.frm(FrmPanoMirView.frm) >> frmPmpaViewPanoMir.cs 폼이름 재정의" />
    public partial class frmPmpaViewPanoMir : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();

        string GstrRetValue = "";

        public frmPmpaViewPanoMir()
        {
            InitializeComponent();
        }

        public frmPmpaViewPanoMir(string strRetValue)
        {
            InitializeComponent();
            GstrRetValue = strRetValue;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        void Search_Data()
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

            int nRead = 0;
            int nRow = 0;
            double nTAmt = 0;
            double nJAmt = 0;
            double nEdiTAmt = 0;
            double nEdiJAmt = 0;
            double nEdiMirNo = 0;
            string strYYMM = "";
            string strWeek = "";

            if (txtPano.Text.Length != 8)
            {
                ComFunc.MsgBox("등록번호를 입력하세요");
                txtPano.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //청구ID를 SELECT
                SQL = "";
                if (rdoInsu0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "SELECT YYMM, Bi, DeptCode1,";
                    SQL = SQL + ComNum.VBLF + "       JinDate1, TAmt, JAmt,";
                    SQL = SQL + ComNum.VBLF + "       EdiTAmt, EdiJAmt, EdiBAmt,";
                    SQL = SQL + ComNum.VBLF + "       EdiMirNo, WRTNO, UpCnt1,";
                    SQL = SQL + ComNum.VBLF + "       IODate, GBMIR GUBUN";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                }
                else if (rdoInsu1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "SELECT YYMM, '31' Bi, DeptCode DeptCode1,";
                    SQL = SQL + ComNum.VBLF + "       TO_CHAR(FrDate,'YYYYMMDD') JinDate1, 0 TAmt, JAmt,";
                    SQL = SQL + ComNum.VBLF + "       EdiTAmt, EdiTAmt EdiJAmt, EdiMirNo,";
                    SQL = SQL + ComNum.VBLF + "       WRTNO, UpCnt1, TO_CHAR(ToDate,'YYYYMMDD') IODate,";
                    SQL = SQL + ComNum.VBLF + "       MIRGBN GUBUN";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_SANID ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "SELECT YYMM, 52 Bi, DeptCode1,";
                    SQL = SQL + ComNum.VBLF + "       TO_CHAR(FrDate,'YYYYMMDD') JinDate1, 0 TAmt, JAmt,";
                    SQL = SQL + ComNum.VBLF + "       0 EdiTAmt, 0 EdiJAmt, 0 EdiMirNo,";
                    SQL = SQL + ComNum.VBLF + "       WRTNO, UpCnt1, TO_CHAR(ToDate,'YYYYMMDD') IODate,";
                    SQL = SQL + ComNum.VBLF + "       MIRGBN GUBUN";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_TAID ";
                }
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND Pano='" + txtPano.Text + "' ";
                if (rdoIO0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd='I' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND IpdOpd='O' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY YYMM DESC ";

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
                ssView_Sheet1.RowCount = nRead;

                for (i = 0; i < nRead; i++)
                {
                    nTAmt = VB.Val(dt.Rows[i]["TAmt"].ToString().Trim());
                    nJAmt = VB.Val(dt.Rows[i]["JAmt"].ToString().Trim());

                    nEdiTAmt = VB.Val(dt.Rows[i]["EdiTAmt"].ToString().Trim());
                    nEdiJAmt = VB.Val(dt.Rows[i]["EdiJAmt"].ToString().Trim());

                    if (nJAmt != 0 && nTAmt == 0)
                    {
                        nTAmt = nJAmt;
                    }
                    if (nEdiJAmt != 0 && nEdiTAmt == 0)
                    {
                        nEdiTAmt = nEdiJAmt;
                    }

                    nEdiMirNo = VB.Val(dt.Rows[i]["EdiMirNo"].ToString().Trim());
                    strYYMM = dt.Rows[i]["YYMM"].ToString().Trim();

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = strYYMM;
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["DeptCode1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["JinDate1"].ToString().Trim();
                    if (nEdiJAmt != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = nEdiJAmt.ToString("###,###,###,##0");
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = nJAmt.ToString("###,###,###,##0");
                    }

                    if (rdoInsu0.Checked == true)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Left(dt.Rows[i]["IODate"].ToString().Trim(), 8) + "~" +
                                                                VB.Right(dt.Rows[i]["IODate"].ToString().Trim(), 8);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["IODate"].ToString().Trim();
                    }

                    if (dt.Rows[i]["UpCnt1"].ToString().Trim() == "9")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = "보류";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = "";
                    }

                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                    if (nEdiMirNo != 0)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = nEdiMirNo.ToString();
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = "";
                    }


                    //EDI 접수내역을 READ
                    if (nEdiMirNo > 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate, JepNo, Week, MirGbn";
                        if (rdoInsu0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_SANJEPSU ";
                        }
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND MirNo='" + nEdiMirNo + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = dt1.Rows[0]["MirGbn"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = dt1.Rows[0]["JepDate"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = dt1.Rows[0]["JepNo"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 10].Text = dt1.Rows[0]["Week"].ToString().Trim();
                            strWeek = dt1.Rows[0]["Week"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }


                    //중간청구 여부
                    if (strWeek == "7")
                    {
                        //중간청구


                        string strLastDay = "";

                        strYYMM = cpf.DATE_YYMM_ADD(strYYMM, 1);
                        strLastDay = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Mid(strYYMM, 5, 2)));

                        //개인별 청구차액을 표시
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT BuildJAmt JOHAP, JEPJAMT ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "'";
                        SQL = SQL + ComNum.VBLF + "   AND BuildDate>=TO_DATE('" + VB.Left(strYYMM, 4) + "-" + VB.Mid(strYYMM, 5, 2) + "-01" + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BuildDate <TO_DATE('" + Convert.ToDateTime(strLastDay).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND Flag='1' "; //청구Build한 내역

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Val(dt1.Rows[0]["JOHAP"].ToString().Trim()).ToString("##,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt1.Rows[0]["JEPJAMT"].ToString().Trim()).ToString("##,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = (VB.Val(dt1.Rows[0]["JEPJAMT"].ToString().Trim()) -
                                                                      VB.Val(dt1.Rows[0]["JOHAP"].ToString().Trim())).ToString("##,###,###,##0");
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        //개인별 차액

                        //개인별 청구 차액을 표시
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT JOHAP, JEPJAMT ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                        SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "'";
                        SQL = SQL + ComNum.VBLF + "   AND YYMM ='" + strYYMM + "' ";
                        if (rdoIO0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " AND IpdOpd='I' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " AND IpdOpd='O' ";
                        }
                        SQL = SQL + ComNum.VBLF + " AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssView_Sheet1.Cells[nRow - 1, 13].Text = VB.Val(dt1.Rows[0]["JOHAP"].ToString().Trim()).ToString("##,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 14].Text = VB.Val(dt1.Rows[0]["JEPJAMT"].ToString().Trim()).ToString("##,###,###,##0");
                            ssView_Sheet1.Cells[nRow - 1, 15].Text = (VB.Val(dt1.Rows[0]["JEPJAMT"].ToString().Trim()) -
                                                                      VB.Val(dt1.Rows[0]["JOHAP"].ToString().Trim())).ToString("##,###,###,##0");
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    if (rdoInsu0.Checked == true)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 16].Text = VB.Val(dt.Rows[i]["EDIBAMT"].ToString().Trim()).ToString("##,###,###,##0");
                    }

                }
                ssView.Enabled = true;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void frmPmpaViewPanoMir_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            lblSName.Text = "";

            if (GstrRetValue != "")
            {
                if (VB.Right(GstrRetValue, 1) == "O")
                {
                    rdoIO1.Checked = true;
                }
                else
                {
                    rdoIO0.Checked = true;
                }
                txtPano.Text = VB.Left(GstrRetValue, 8);
                switch (VB.Mid(GstrRetValue, 9, 2))
                {
                    case "31":
                        rdoInsu1.Checked = true;
                        break;
                    case "52":
                        rdoInsu2.Checked = true;
                        break;
                    default:
                        rdoInsu0.Checked = true;
                        break;
                }
            }

            #region 김해수 2018-09-12 퇴원,중간청구액 차액점검 및변경 자동조회
            if (txtPano.Text != "" && GstrRetValue != "")
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                    txtPano.Focus();
                    return;
                }
                lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();

                Search_Data();
                GstrRetValue = "";
            }
            #endregion
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            GstrRetValue = "";
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
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

            GstrRetValue = VB.Left(ssView_Sheet1.Cells[e.Row, 8].ToString().Trim() + VB.Space(10), 10);     //EDI 접수일
            GstrRetValue += VB.Val(ssView_Sheet1.Cells[e.Row, 11].ToString().Trim()).ToString("0000000000");    //WRTNO
            GstrRetValue += VB.Val(ssView_Sheet1.Cells[e.Row, 4].ToString().Trim()).ToString("0000000000");     //조합부담

            this.Close();
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            lblSName.Text = "";
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
            ssView.Enabled = false;
            if (txtPano.Text != "" && GstrRetValue != "")
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                    txtPano.Focus();
                    return;
                }
                lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();

                Search_Data();
                GstrRetValue = "";
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

            if (cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows.Count == 0)
            {
                ComFunc.MsgBox("등록되지 않은 등록번호입니다.");
                txtPano.Focus();
                return;
            }
            lblSName.Text = cpf.Get_BasPatient(clsDB.DbCon, txtPano.Text).Rows[0]["SName"].ToString().Trim();
            btnSearch.Focus();
        }
    }
}
