using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary> 검사결과 출력 </summary>
    public partial class frmExamResultPrint : Form
    {
        string FstrPano = "";
        int FnRowExam = 0;
        string FstrSpecNo = "";
        string strDate = "";

        /// <summary> 검사결과 출력 </summary>
        public frmExamResultPrint()
        {
            InitializeComponent();
        }
        public frmExamResultPrint(string GFstrPano , string GFstrSpecNo, int GFnRowExam)
        {
            InitializeComponent();

            FstrPano = GFstrPano;
            FstrSpecNo = GFstrSpecNo;
            FnRowExam = GFnRowExam;
        }

        public frmExamResultPrint(string strPano, string strDate)
        {
            InitializeComponent();

            FstrPano = strPano;
            this.strDate  = strDate;
        }

        void frmExamResultPrint_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            FstrSpecNo = "";

            if(strDate != "")
            {
                dtpActDate.Value = Convert.ToDateTime(strDate);
                txtPano.Text = FstrPano;
                txtSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text);
                txtSex.Text = clsVbfunc.READ_SEX(clsDB.DbCon, txtPano.Text);
                GetData();
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            ScreenClear();

            GetSearchData();
        }

        void GetSearchData()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView1_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT  A.PANO, A.SNAME, A.SEX,  A.IPDOPD, NVL(A.REQCNT, 1) REQCNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_RESULTPRINT A ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.ACTDATE = TO_DATE('" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (rdoGb0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.PRINTDATE2 IS NULL ";
                }

                if (rdoGb1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.PRINTDATE2 IS NOT NULL ";
                }

                SQL = SQL + ComNum.VBLF + "  GROUP  BY A.PANO, A.SNAME, A.SEX,  A.IPDOPD, NVL(A.REQCNT, 1)  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                ssView1_Sheet1.RowCount = dt.Rows.Count;
                ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["REQCNT"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        void ScreenClear()
        {
            FnRowExam = 0;
            ssView2_Sheet1.RowCount = 0;
            txtPano.Text = "";
            txtSName.Text = "";
            txtSex.Text = "";
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("정말로 출력하시겠습니까?", "Print", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            Set_Print();
            ComFunc.Delay(100);

            Save_Print_Date();
        }

        void Save_Print_Date()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {

                SQL = " UPDATE " + ComNum.DB_MED + "EXAM_RESULTPRINT SET PRINT = PRINT +1 , ";

                if (rdoGb0.Checked)
                {
                    SQL = SQL + ComNum.VBLF + " PRINTDATE = SYSDATE, ";
                }

                SQL = SQL + ComNum.VBLF + " PRINTDATE2 = SYSDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE SPECNO IN ( " + FstrSpecNo + " )";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Set_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;


            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "임상병리검사 결과";


            strHeader = CS.setSpdPrint_String(strTitle, new Font("맑은 고딕", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록번호: " + txtPano.Text + " 성명: " + txtSName.Text + " 성별: " + txtSex.Text, new Font("맑은 고딕", 11), clsSpread.enmSpdHAlign.Left, false, false);

            strFooter = CS.setSpdPrint_String("포항성모병원", new Font("맑은 고딕", 11), clsSpread.enmSpdHAlign.Left, false, false);
            strFooter += CS.setSpdPrint_String("출력일시: " + ComQuery.CurrentDateTime(clsDB.DbCon, "S") + "     Page : /p", new Font("맑은 고딕", 11), clsSpread.enmSpdHAlign.Right, false, false);                                     

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, false, true, false, false);

            CS.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
        }


        void ExamResultDisplay(string strArg)
        {
            int i = 0;
            int J = 0;
            string strSpecNo = "";
            //string strCompare = "";
            string strRef = "";
            string strResultDate = "";   //결과일자
            string strStatus = "";   //상태
            string strResult = "";   //결과
            string strOK = "";   //Display여부
            int intCNT = 0;
            int intCnt1 = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT R.Status,R.MasterCode,R.SubCode, R.Result, R.Refer, R.Panic, ";
                SQL = SQL + ComNum.VBLF + " R.Delta, R.Unit, R.SeqNo, M.ExamName, TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL = SQL + ComNum.VBLF + " R.ResultSabun, TO_CHAR(S.ReceiveDate,'YYYY-MM-DD HH24:MI') ReceiveDate, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(S.ResultDate,'YYYY-MM-DD HH24:MI') RESULTDATE2, S.AGE, S.SEX, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(S.BDATE, 'YYYY-MM-DD') BDATE, P.KORNAME, D.DRNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED  + "Exam_ResultC R, " + ComNum.DB_MED + "Exam_Master M, ";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_MED  + "EXAM_SPECMST S, " + ComNum.DB_ERP + "INSA_MST P, ";
                SQL = SQL + ComNum.VBLF + "      " + ComNum.DB_PMPA + "BAS_DOCTOR D ";
                SQL = SQL + ComNum.VBLF + "WHERE R.SpecNo='" + strArg + "' ";
                SQL = SQL + ComNum.VBLF + "  AND R.SPECNO = S.SPECNO(+)";
                SQL = SQL + ComNum.VBLF + "  AND R.SubCode = M.MasterCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND R.RESULTSABUN = P.SABUN(+)";
                SQL = SQL + ComNum.VBLF + "  AND S.DRCODE = D.DRCODE(+)";
                SQL = SQL + ComNum.VBLF + "ORDER BY R.SeqNo ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                intCNT = dt.Rows.Count;
                //strCompare = "";
                ssView2_Sheet1.RowCount = FnRowExam;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        FnRowExam = FnRowExam + 1;
                        ssView2_Sheet1.RowCount = FnRowExam;
                        ssView2_Sheet1.Cells[FnRowExam - 1, 0].Text = "  ▶ 처방일자: " + dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView2_Sheet1.Cells[FnRowExam - 1, 1].Text = "   DR.: " + dt.Rows[i]["DRNAME"].ToString().Trim();
                    }

                    strResultDate = dt.Rows[i]["ResultDate"].ToString().Trim();
                    strStatus = dt.Rows[i]["Status"].ToString().Trim();
                    strResult = dt.Rows[i]["Result"].ToString().Trim();

                    if (strStatus == "H")
                    {
                        strOK = "OK";
                    }
                    else if (strStatus == "V")
                    {
                        strOK = "OK";

                        if (strResult == "")
                        {
                            strOK = "NO";
                        }

                        if (dt.Rows[i]["MasterCode"].ToString().Trim() == dt.Rows[i]["SubCode"].ToString().Trim())
                        {
                            strOK = "OK";
                        }
                    }
                    else
                    {
                        strOK = "OK";
                        strResult = "-< 검사중 >-";
                    }

                    //Foot Note를 READ
                    SQL = "";
                    SQL = " SELECT FootNote FROM " + ComNum.DB_MED + "Exam_ResultCf ";
                    SQL = SQL + ComNum.VBLF + "WHERE SpecNo = '" + strSpecNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SeqNo =  " + dt.Rows[i]["SeqNo"].ToString().Trim() + "  ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    intCnt1 = dt1.Rows.Count;
                    if (intCnt1 > 0)
                    {
                        strOK = "OK";
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (strOK == "OK")
                    {
                        FnRowExam += 1;
                        if (FnRowExam > ssView2_Sheet1.RowCount)
                            ssView2_Sheet1.RowCount = FnRowExam + 20;

                        ssView2_Sheet1.Cells[FnRowExam - 1, 0].Text = "    " + dt.Rows[i]["ExamName"].ToString(); //검사이름
                        ssView2_Sheet1.Cells[FnRowExam - 1, 1].Text = "    " + dt.Rows[i]["Result"].ToString(); //결과치
                        ssView2_Sheet1.Cells[FnRowExam - 1, 2].Text = dt.Rows[i]["Refer"].ToString();
                        ssView2_Sheet1.Cells[FnRowExam - 1, 3].Text = "    " + dt.Rows[i]["Unit"].ToString(); //결과단위

                        strRef = Reference(dt.Rows[i]["SubCode"].ToString().Trim(), dt.Rows[i]["AGE"].ToString().Trim(), dt.Rows[i]["SEX"].ToString().Trim());
                        ssView2_Sheet1.Cells[FnRowExam - 1, 4].Text = "    " + strRef; //참고치

                        if (dt.Rows[i]["UNIT"].ToString().Trim() != "None")
                        {
                            ssView2_Sheet1.Cells[FnRowExam - 1, 6].Text = dt.Rows[i]["KORNAME"].ToString().Trim(); //검사자
                        }
                    }

                    if (intCnt1 > 0)
                    {
                        //Foot Note를 READ
                        SQL = "";
                        SQL = " SELECT FootNote FROM " + ComNum.DB_MED + "Exam_ResultCf ";
                        SQL = SQL + ComNum.VBLF + "WHERE SpecNo = '" + strArg + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND SeqNo =  " + dt.Rows[i]["SeqNo"].ToString().Trim() + " ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            Cursor.Current = Cursors.Default;
                            dt1.Dispose();
                            dt1 = null;
                            return;
                        }

                        for (J = 0; J < intCnt1; J++)
                        {
                            FnRowExam += 1;
                            if (FnRowExam > ssView2_Sheet1.RowCount)
                                ssView2_Sheet1.RowCount = FnRowExam + 20;
                            ssView2_Sheet1.Cells[FnRowExam - 1, 0].Text = "  " + dt1.Rows[J]["FootNote"].ToString().Trim();
                            ssView2_Sheet1.Cells[FnRowExam - 1, 0].ForeColor = Color.Blue;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                dt.Dispose();
                dt = null;

                if (FnRowExam > 21)
                {
                    ssView2_Sheet1.RowCount = FnRowExam;
                }

                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        string Reference(string Code, string Age, string Sex)
        {
            string strVal = "";
            int i = 0;
            int J = 0;
            string strCode = "";
            string strNormal = "";
            string strSex = "";
            string strAgeFrom = "";
            string strAgeTo = "";
            string strRefValFrom = "";
            string strRefValTo = "";
            string strAllReference = "";
            string strReference = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = " SELECT MasterCode, Normal, Sex, AgeFrom, AgeTo, RefvalFrom, RefvalTo ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "Exam_Master_Sub ";
                SQL = SQL + ComNum.VBLF + "WHERE MasterCode = '" + Code + "' AND Gubun = '41'";   //41:Reference Value

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strCode = dt.Rows[i]["MasterCode"].ToString().Trim();
                    strNormal = dt.Rows[i]["Normal"].ToString().Trim();
                    strSex = dt.Rows[i]["Sex"].ToString();
                    strAgeFrom = dt.Rows[i]["AgeFrom"].ToString();
                    strAgeTo = dt.Rows[i]["AgeTo"].ToString().Trim();
                    strRefValFrom = dt.Rows[i]["RefvalFrom"].ToString().Trim();
                    strRefValTo = dt.Rows[i]["RefvalTo"].ToString();

                    strAllReference = strAllReference + strCode + "|" + strNormal + "|" + strSex + "|" + strAgeFrom + "|" + strAgeTo + "|" + strRefValFrom + "|" + strRefValTo + "|" + "|";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strVal;
            }

            strReference = VB.TR(strAllReference, strCode, "^");


            long i2 = VB.L(strReference, "^");

            if (i2 == 1)
            {
                return strVal;
            }

            for (J = 2; J <= i2; J++)
            {
                strNormal = VB.PP(VB.PP(strReference, "^", J), "|", 2);
                strSex = VB.PP(VB.PP(strReference, "^", J), "|", 3);
                strAgeFrom = VB.PP(VB.PP(strReference, "^", J), "|", 4);
                strAgeTo = VB.PP(VB.PP(strReference, "^", J), "|", 5);
                strRefValFrom = VB.PP(VB.PP(strReference, "^", J), "|", 6);
                strRefValTo = VB.PP(VB.PP(strReference, "^", J), "|", 7);

                if (strNormal != "")
                {
                    strVal = strNormal;
                    return strVal;
                }

                if (strSex == "" || strSex == Sex)
                {
                    if (strAgeFrom != "" && strAgeTo != "")
                    {
                        if (VB.Val(strAgeFrom) <= VB.Val(Age) && VB.Val(Age) <= VB.Val(strAgeTo))
                        {
                            strVal = strRefValFrom + " ~ " + strRefValTo;
                            return strVal;
                        }
                    }
                }
            }

            return strVal;
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ScreenClear();

            txtPano.Text = ssView1_Sheet1.Cells[e.Row, 0].Text;
            FstrPano = txtPano.Text;
            txtSName.Text = ssView1_Sheet1.Cells[e.Row, 1].Text;
            txtSex.Text = ssView1_Sheet1.Cells[e.Row, 2].Text;

            FnRowExam = 0;

            GetData();
        }

        void GetData()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {

                SQL = "";
                SQL = " SELECT SPECNO FROM " + ComNum.DB_MED + "EXAM_RESULTPRINT ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE( '" + dtpActDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + FstrPano + "'";

                if (rdoGb0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND PRINTDATE2 IS NULL";
                }

                if (rdoGb1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND PRINTDATE2 IS NOT NULL";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                FstrSpecNo = "0";

                string strSPECNO = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strSPECNO = dt.Rows[i]["SPECNO"].ToString().Trim();
                    if (i == 0)
                    {
                        FstrSpecNo = " '" + strSPECNO + "' ";
                    }
                    else
                    {
                        FstrSpecNo = FstrSpecNo + " ,'" + strSPECNO + "' ";
                    }

                    ExamResultDisplay(strSPECNO);
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void ssView1_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView1_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView1, e.Column);
                return;
            }

            ssView1_Sheet1.Cells[0, 0, ssView1_Sheet1.RowCount - 1, ssView1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView1_Sheet1.Cells[e.Row, 0, e.Row, ssView1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
        }
    }
}
