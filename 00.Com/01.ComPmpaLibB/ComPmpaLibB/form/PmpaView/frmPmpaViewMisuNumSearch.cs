using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using ComPmpaLibB.form.PmpaView;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMisuNumSearch.cs
    /// Description     : 미수번호조회
    /// Author          : 박창욱
    /// Create Date     : 2017-11-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 2017-11-03 박창욱 : FrmMstSearch2, FrmMstSearch3 통합
    /// </history>
    /// <seealso cref= "\misu\MUMAIN11.FRM(FrmMstSearch3.frm) >> frmPmpaViewMisuNumSearch.cs 폼이름 재정의" />	
    /// <seealso cref= "\misu\MUMAIN04.FRM(FrmMstSearch2.frm) >> frmPmpaViewMisuNumSearch.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMisuNumSearch : Form
    {
        clsPmpaPb.GstrBunSu GstrBunSu = null;
        frmPmpaMisuMast1 fPMM1 = null;
        frmPmpaMisuMast2 fPMM2 = null;
        frmPmpaMisuMast fPMM = null;
        frmPmpaMisuMast2TA fPMMT = null;

        string GstrClass = "";
        //string GstrClass = "07";    //디버깅용
        double GnWRTNO = 0;
        string MisuGubun = "";

        public frmPmpaViewMisuNumSearch()
        {
            InitializeComponent();
        }

        public frmPmpaViewMisuNumSearch(string strClass)
        {
            InitializeComponent();

            GstrClass = strClass;
        }

        public frmPmpaViewMisuNumSearch(clsPmpaPb.GstrBunSu argBunSu,string argGubun) 
        {
            InitializeComponent();

            GstrBunSu = argBunSu;
            GstrClass = GstrBunSu.GstrClass;

            switch (GstrClass)
            {
                case "05": //자보
                case "07": //산재
                case "08": //계약처
                case "09": //헌혈미수
                case "11": //보훈청
                case "12": //시각장애
                case "13": //심신장애
                case "14": //장애인보장구
                case "15": //직원대납
                case "16": //노인장기요양소견서
                case "17": //방문간호지시서
                case "18": //치매검사
                    lblName.Visible = true;
                    TxtName.Visible = true;
                    break;
                default:
                    lblName.Visible = false;
                    TxtName.Visible = false;
                    break;
            }

            MisuGubun = argGubun;
     

        }//2018-12-07,  김해수

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nIAmt = 0;
            string strWrtno = "";
            string strChk = "";
            string strIpdOpd = "";
            string strGelCode = ""; 

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data)+1; i++)
                //for (i = 0; i < ssView_Sheet1.RowCount; i++) //20190729 전체 데이터 등록
                {
                    strChk = Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true ? "1" : "0";
                    strIpdOpd = ssView_Sheet1.Cells[i, 3].Text;
                    nIAmt = VB.Val(VB.Replace(ssView_Sheet1.Cells[i, 10].Text,",",""));
                    strWrtno = ssView_Sheet1.Cells[i, 12].Text;
                    strGelCode = ssView_Sheet1.Cells[i, 13].Text;

                    if (strChk == "1")
                    {
                        //Master 갱신

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "MISU_IDMST SET ";
                        SQL = SQL + ComNum.VBLF + " QTY2 = QTY2 + 1 , ";
                        SQL = SQL + ComNum.VBLF + " AMT3 = AMT3 + '" + nIAmt + "',  ";
                        SQL = SQL + ComNum.VBLF + " GBEND = '0' "; //미수입금완료
                        SQL = SQL + ComNum.VBLF + " WHERE WRTNO = '" + strWrtno + "'  ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        //Slip 등록
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO MISU_SLIP";
                        SQL = SQL + ComNum.VBLF + "        (WRTNO,MisuID,Bdate,GelCode,IpdOpd,";
                        SQL = SQL + ComNum.VBLF + "        Class,Gubun,Qty,TAmt,Amt,Remark,EntDate,EntPart)";
                        SQL = SQL + ComNum.VBLF + " VALUES";
                        SQL = SQL + ComNum.VBLF + "        ('" + strWrtno + "','" + txtMisuID.Text + "',";
                        SQL = SQL + ComNum.VBLF + "        TO_DATE('" + dtpBDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + "        '" + strGelCode + "','" + strIpdOpd + "',";
                        SQL = SQL + ComNum.VBLF + "        '" + GstrClass + "','21' ,  1 , " + nIAmt + "," + nIAmt + ",";
                        SQL = SQL + ComNum.VBLF + "        '자동입금처리',SYSDATE,";
                        SQL = SQL + ComNum.VBLF + "        '" + clsType.User.Sabun + "')";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //디버깅용
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("등록하였습니다.");
                Cursor.Current = Cursors.Default;

                Search_Data();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
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

            strTitle = txtMisuID.Text + "미수번호 조회";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!(txtMisuID.Text == ""))
            {
                txtMisuID.Text = VB.Val(txtMisuID.Text).ToString("00000000");
            }

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
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            double nMAmt = 0;
            double nIAmt = 0;
            double nSAmt = 0;
            double nSAmt2 = 0;
            double nBAmt = 0;
            string strData = "";

            //Sheet Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            Cursor.Current = Cursors.WaitCursor;
            txtIAmt.Text = "";
            nMAmt = 0;
            nIAmt = 0;
            nSAmt = 0;
            nBAmt = 0;

            try
            {
                switch (GstrClass)
                {
                    case "01":
                    case "02":
                        #region Class_11_RTN

                        //공단, 직장 Select
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT WRTNO,TO_CHAR(Bdate,'YYYY-MM-DD') Bdate,";
                        SQL = SQL + ComNum.VBLF + "        MisuID,IpdOpd,Gelcode,Bun,Amt2 MAmt,";
                        SQL = SQL + ComNum.VBLF + "        Amt3+Amt6+Amt7  IAMT, Amt4 SAMT, Amt5 BAMT, Amt8 SAmt2";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST";
                        if(txtMisuID.Text != "")
                        {
                            SQL = SQL + ComNum.VBLF + "  WHERE MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        }else
                        {
                            SQL = SQL + ComNum.VBLF + "  WHERE MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        }
                        
                        SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "'";
                        if (rdoMisu0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND GbEnd = '1'";
                        }
                        else if (rdoMisu1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND GbEnd = '0'";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate DESC, GelCode";

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

                        ssView_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) != 0)
                            {
                                ssView_Sheet1.Cells[i, 0].Locked = true;
                            }
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                            strData = BunName_Set(dt.Rows[i]["Bun"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 4].Text = strData;
                            ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 8].Text = VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 10].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) -
                                                               VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()) -
                                                               VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim())).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;

                        #endregion
                        break;

                    case "03":
                    case "04":
                        #region Class_13_RTN

                        //지역, 보호
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate, a.MisuID,";
                        SQL = SQL + ComNum.VBLF + "        a.IpdOpd, a.Bun, a.Gelcode,";
                        SQL = SQL + ComNum.VBLF + "        a.Amt2 MAmt, a.Amt3+a.Amt6+a.Amt7 IAMT, a.Amt4 SAMT,";
                        SQL = SQL + ComNum.VBLF + "        a.Amt5 BAMT, Amt8 SAmt2, b.MiaName ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.VBLF + "MISU_IDMST a," + ComNum.VBLF + "BAS_MIA b ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND MISUID =  '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "' ";

                        if (rdoMisu0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '1'";
                        }
                        else if (rdoMisu1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '0'";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND rtrim(b.MiaCode) = Rtrim(a.GelCode)";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate,GelCode DESC";


                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {

                            dt.Dispose();
                            dt = null;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        ssView_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) != 0)
                            {
                                ssView_Sheet1.Cells[i, 0].Locked = true;
                            }
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MiaName"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                            strData = BunName_Set(dt.Rows[i]["Bun"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 4].Text = strData;
                            ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            nMAmt += VB.Val(dt.Rows[i]["MAmt"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            nIAmt += VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            nSAmt += VB.Val(dt.Rows[i]["SAmt"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 8].Text = VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()).ToString("###,###,###,##0");
                            nSAmt2 += VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            nBAmt += VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 10].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) -
                                                               VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()) -
                                                               VB.Val(dt.Rows[i]["BAmt"].ToString().Trim())).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;

                        ssView_Sheet1.RowCount += 1;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "합계";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nMAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = nIAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = nSAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = nSAmt2.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nBAmt.ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = (nMAmt - nIAmt - nSAmt - nSAmt2 - nBAmt).ToString();

                        #endregion
                        break;

                    case "05":
                    case "07":
                        #region Class_31_RTN

                        //산재, 자보 Select
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate, a.GelCode,";
                        SQL = SQL + ComNum.VBLF + "        a.MisuID, a.IpdOpd, a.DeptCode,";
                        SQL = SQL + ComNum.VBLF + "        a.Amt2 MAmt, a.Amt3+a.Amt6+a.Amt7 IAMT, a.Amt4 SAMT,";
                        SQL = SQL + ComNum.VBLF + "        a.Amt5 BAMT, TO_CHAR(a.FromDate,'YY-MM-DD') Fdate, TO_CHAR(a.ToDate,'YY-MM-DD') Tdate,";
                        SQL = SQL + ComNum.VBLF + "        b.Sname";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        if(txtMisuID.Text.Trim() != "00000000")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND B.SName = '" + TxtName.Text.Trim() + "'";
                        }
                        
                        SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "'";

                        if (rdoMisu0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '1'";
                        }
                        else if (rdoMisu1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '0'";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND a.MisuID = b.Pano(+)";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate";
                         
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {

                            dt.Dispose();
                            dt = null;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        ssView_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                            strData = dt.Rows[i]["GelCode"].ToString().Trim() + ",";
                            strData += dt.Rows[i]["FDate"].ToString().Trim() + "-";
                            strData += dt.Rows[i]["TDate"].ToString().Trim() + ",";
                            strData += dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 4].Text = strData;
                            ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 10].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) -
                                                  VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["BAmt"].ToString().Trim())).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;


                        #endregion
                        break;

                    case "08":
                    case "09":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                        #region Class_ETC_RTN

                        //헌혈, 계약처
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO, TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate, a.GelCode,";
                        SQL = SQL + ComNum.VBLF + "        a.MisuID, a.IpdOpd, a.DeptCode,";
                        SQL = SQL + ComNum.VBLF + "        a.Amt2 MAmt, a.Amt3+a.Amt6+a.Amt7 IAMT, a.Amt4 SAMT,";
                        SQL = SQL + ComNum.VBLF + "        a.Amt5 BAMT, TO_CHAR(a.FromDate,'YY-MM-DD') Fdate, TO_CHAR(a.ToDate,'YY-MM-DD') Tdate,";
                        SQL = SQL + ComNum.VBLF + "        b.Sname";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        //SQL = SQL + ComNum.VBLF + "    AND MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        if (txtMisuID.Text.Trim() != "00000000")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND B.SName = '" + TxtName.Text.Trim() + "'";
                        }
                        SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "'";

                        if (rdoMisu0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '1'";
                        }
                        else if (rdoMisu1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '0'";
                        }
                        SQL = SQL + ComNum.VBLF + "    AND a.MisuID = b.Pano(+)";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY a.Bdate";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {

                            dt.Dispose();
                            dt = null;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        ssView_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) != 0)
                            {
                                ssView_Sheet1.Cells[i, 0].Locked = true;
                            }
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                            strData = dt.Rows[i]["GelCode"].ToString().Trim() + ",";
                            strData += dt.Rows[i]["FDate"].ToString().Trim() + "-";
                            strData += dt.Rows[i]["TDate"].ToString().Trim() + ",";
                            strData += dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 4].Text = strData;
                            ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 10].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) -
                                               VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["BAmt"].ToString().Trim())).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;

                        #endregion
                        break;

                    case "16":
                    case "17":
                    case "18":
                        #region Class_16_RTN

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT A.WRTNO, TO_CHAR(A.Bdate,'YYYY-MM-DD') Bdate, A.MisuID,";
                        SQL = SQL + ComNum.VBLF + "        A.IpdOpd, A.gelcode, A.Bun,";
                        SQL = SQL + ComNum.VBLF + "        A.Amt2 MAmt, A.Amt3+A.Amt6+A.Amt7  IAMT, A.Amt4 SAMT,";
                        SQL = SQL + ComNum.VBLF + "        A.Amt5 BAMT, A.Amt8 SAmt2, B.SNAME";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_IDMST A," + ComNum.DB_PMPA + "BAS_PATIENT B";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        //SQL = SQL + ComNum.VBLF + "    AND A.MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        if (txtMisuID.Text.Trim() != "00000000")
                        {
                            SQL = SQL + ComNum.VBLF + "    AND MisuID = '" + VB.Left(txtMisuID.Text + VB.Space(8), 8) + "'";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "    AND B.SName = '" + TxtName.Text.Trim() + "'";
                        }

                        SQL = SQL + ComNum.VBLF + "    AND A.Class = '" + GstrClass + "'";
                        SQL = SQL + ComNum.VBLF + "    AND A.MISUID = B.PANO";
                        if (rdoMisu0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND A.GbEnd = '1'";
                        }
                        else if (rdoMisu1.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "    AND A.GbEnd = '0'";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY A.Bdate, A.GelCode";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다"); 
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {

                            dt.Dispose();
                            dt = null;
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        ssView_Sheet1.RowCount = dt.Rows.Count;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) != 0)
                            {
                                ssView_Sheet1.Cells[i, 0].Locked = true;
                            }
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                            strData = BunName_Set(dt.Rows[i]["Bun"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 4].Text = strData;
                            ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 9].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 10].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) -
                                               VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["BAmt"].ToString().Trim())).ToString("###,###,###,##0");
                            ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["MisuID"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                            ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["GelCode"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;

                        #endregion
                        break;
                }

                ssView.Focus();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string BunName_Set(string Arg1)
        {
            string ArgReturn = "";

            switch (Arg1)
            {
                case "01":
                    ArgReturn = "내과분야";
                    break;
                case "02":
                    ArgReturn = "외과분야";
                    break;
                case "03":
                    ArgReturn = "산소아과";
                    break;
                case "04":
                    ArgReturn = "안.이비인후";
                    break;
                case "05":
                    ArgReturn = "피.비뇨기";
                    break;
                case "06":
                    ArgReturn = "치과";
                    break;
                case "07":
                    ArgReturn = "NP정액";
                    break;
                case "08":
                    ArgReturn = "장애대불";
                    break;
                case "09":
                    ArgReturn = "가정간호";
                    break;
                case "10":
                    ArgReturn = "재청구";
                    break;
                case "11":
                    ArgReturn = "이의신청";
                    break;
                case "12":
                    ArgReturn = "정산진료비";
                    break;
                case "13":
                    ArgReturn = "추가청구";
                    break;
                case "14":
                    ArgReturn = "NP장애대불";
                    break;
                case "15":
                    ArgReturn = "HD정액";
                    break;
                case "16":
                    ArgReturn = "HU호스피스";
                    break;
                case "19":
                    ArgReturn = "기타청구";
                    break;
                case "20":
                    ArgReturn = "상한대불";
                    break;
                case "21":
                    ArgReturn = "희귀지원금";
                    break;
                case "22":
                    ArgReturn = "결핵지원금";
                    break;
                case "23":
                    ArgReturn = "DRG(포괄수가)";
                    break;
                case "24":
                    ArgReturn = "100/100 미만";
                    break;
                case "25":
                    ArgReturn = "국가재난지원";
                    break;
            }
            return ArgReturn;
        }

        private void frmPmpaViewMisuNumSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

           

            if (GstrClass == "")
            {
                ComFunc.MsgBox("미수구분이 없습니다.");
                this.Close();
                return;
            }

            txtIAmt.Text = "";
            dtpBDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            ssView_Sheet1.Columns[12].Visible = false;

            if (txtMisuID.Text != "")
            {
                ssView.Focus();

            }else
            {
                txtMisuID.Select();
                txtMisuID.Focus();
            }
        }

        private void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.Column != 0)
            {
                return;
            }

            double nMAmt = 0;
            double nJAmt = 0;
            string strChk = "";

            strChk = Convert.ToBoolean(ssView_Sheet1.Cells[e.Row, 0].Value) == true ? "1" : "0";
            nJAmt = VB.Val(VB.Replace(ssView_Sheet1.Cells[e.Row, 10].Text, ",", ""));
            nMAmt = VB.Val(VB.Replace(ssView_Sheet1.Cells[e.Row, 5].Text, ",", ""));

            if (nJAmt != 0)
            {
                if (strChk == "1")
                {
                    txtIAmt.Text = (VB.Val(txtIAmt.Text) + nMAmt).ToString();
                    ssView_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(139, 189, 255);
                }
                else
                {
                    txtIAmt.Text = (VB.Val(txtIAmt.Text) - nMAmt).ToString();
                    ssView_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 255, 255);
                }
            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.Column == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            //TODO : 변수 전달
            //GnWRTNO = VB.Val(ssView_Sheet1.Cells[e.Row, 12].Text);

            GstrBunSu.GnWRTNO = Convert.ToInt64(VB.Val(ssView_Sheet1.Cells[e.Row, 12].Text));//2018-12-07,  김해수

            if (GstrBunSu.GnWRTNO != 0)
            {
                switch (MisuGubun)
                {
                    case "GITA":
                        fPMM = new frmPmpaMisuMast();
                        fPMM.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    case "SANJE":
                        fPMM2 = new frmPmpaMisuMast2();
                        fPMM2.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    case "TA":
                        fPMMT = new frmPmpaMisuMast2TA();
                        fPMMT.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    case "BOHUM":
                        fPMM1 = new frmPmpaMisuMast1();
                        fPMM1.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    default:
                        break;           
                }
            }
        }

        private void txtMisuID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!(txtMisuID.Text == ""))
                {
                    txtMisuID.Text = VB.Val(txtMisuID.Text).ToString("00000000");
                }

                btnSearch.Focus();
            }
        }
    }
}
