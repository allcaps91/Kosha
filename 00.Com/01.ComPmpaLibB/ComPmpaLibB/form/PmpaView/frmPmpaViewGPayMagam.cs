using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewGPayMagam.cs
    /// Description     : 감액 내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iument\ILREPA06.FRM(FrmGPayMagam.frm) >> frmPmpaViewGPayMagam.cs 폼이름 재정의" />
    public partial class frmPmpaViewGPayMagam : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();

        string StrGbGamek = "";
        string strBigo = "";
        string strPano = "";
        string strBi = "";
        string strBiGubun = "";
        string strName = "";
        string strGwa = "";
        string strGwaName = "";
        string strRoom = "";
        string strDate = "";
        string strTDate = "";
        string strAmset6 = "";
        double nAmt = 0;
        double nSubTot = 0;
        double nTot = 0;
        string strSuNext = "";
        string strBigo2 = "";
        string strBigo3 = "";


        int nSel = 0;
        int nCount = 0;

        int nSubCnt = 0;
        int nSubCut = 0;
        long nCutAmt = 0;

        public frmPmpaViewGPayMagam()
        {
            InitializeComponent();
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
            int i = 0;

            ssPrint_Sheet1.Cells[2, 0].Text = "작성자 : " + clsType.User.JobMan + "  " + "작업일자 : " + dtpDate.Value.ToString("yyyy-MM-dd");
            ssPrint_Sheet1.Cells[3, 0].Text = "출력시간 : " + VB.Now().ToString();
            ssPrint_Sheet1.RowCount = 6;
            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.RowCount += 1;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 11].ColumnSpan = 4;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 8].Text = ssView_Sheet1.Cells[i, 8].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 9].Text = ssView_Sheet1.Cells[i, 9].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 10].Text = ssView_Sheet1.Cells[i, 10].Text;
                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 11].Text = ssView_Sheet1.Cells[i, 11].Text;
            }
            ssPrint_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strTitle = "감  액    내  역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, false, false, false, false, (float)0.9);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            clsSpread cSPD = new clsSpread();

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strJumin = "";
            int nRow = 0;
            int nCut = 0;

            nCount = 0;
            nSubTot = 0;
            nTot = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.GbGamek, C.Pano, C.Bi,";
                SQL = SQL + ComNum.VBLF + "        M.Sname, C.DeptCode, M.RoomCode,";
                SQL = SQL + ComNum.VBLF + "        M.GELCODE, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, TO_CHAR(a.OutDate,'YYYY-MM-DD') OutDate,";
                SQL = SQL + ComNum.VBLF + "        C.Amt, C.Bigo, M.Amset6,";
                SQL = SQL + ComNum.VBLF + "        C.SUNEXT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH C, " + ComNum.DB_PMPA + "IPD_NEW_MASTER M, ";
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS A";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND C.ActDate = TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND C.Bun     = '92'";
                SQL = SQL + ComNum.VBLF + "    AND C.IPDNO   = M.IPDNO(+)";
                SQL = SQL + ComNum.VBLF + "    AND C.TRSNO = A.TRSNO(+) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY C.Pano";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    btnPrint.Enabled = true;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    btnPrint.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRow = dt.Rows.Count;

                for (i = 0; i < nRow; i++)
                {
                    nCount += 1;
                    ssView_Sheet1.RowCount = nCount;
                    strBigo = "";
                    if (i == 0)
                    {
                        nSel = 1;
                    }
                    StrGbGamek = dt.Rows[i]["GbGamek"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strBi = dt.Rows[i]["Bi"].ToString().Trim();
                    strName = dt.Rows[i]["SName"].ToString().Trim();
                    strGwa = dt.Rows[i]["DeptCode"].ToString().Trim();
                    strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                    strDate = dt.Rows[i]["InDate"].ToString().Trim();
                    strTDate = dt.Rows[i]["OutDate"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    strAmset6 = dt.Rows[i]["AmSet6"].ToString().Trim();
                    strSuNext = VB.Left(dt.Rows[i]["SUNEXT"].ToString().Trim(), 4);
                    strBigo3 = "";
                    strBigo3 = Get_SuNameK(dt.Rows[i]["SUNEXT"].ToString().Trim());
                    strBigo2 = dt.Rows[i]["BIGO"].ToString().Trim();


                    if (cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows.Count > 0)
                    {
                        //주민암호화
                        if (cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["Jumin3"].ToString().Trim() != "")
                        {
                            strJumin = VB.Val((cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["Jumin1"].ToString().Trim())).ToString("000000") +
                                       clsAES.DeAES(VB.Val((cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["Jumin3"].ToString().Trim())).ToString("000000"));
                        }
                        else
                        {
                            strJumin = VB.Val((cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["Jumin1"].ToString().Trim())).ToString("000000") +
                                       VB.Val((cpf.Get_BasPatient(clsDB.DbCon, strPano).Rows[0]["Jumin2"].ToString().Trim())).ToString("000000");
                        }
                    }
                    else
                    {
                        strJumin = "";
                    }

                    if (StrGbGamek == "55")
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT B.MIACODE, B.MIANAME";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_MIA B ";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND A.PANO  = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND TRUNC(INDATE) <= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "    AND GBGAMEK  = '55' ";
                        SQL = SQL + ComNum.VBLF + "    AND (OUTDATE IS NULL OR OUTDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD')) ";
                        SQL = SQL + ComNum.VBLF + "    AND Trim(A.GELCODE) = Trim(B.MIACODE) ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnPrint.Enabled = true;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strBigo = "<" + dt1.Rows[0]["MIACODE"].ToString().Trim() + ">" + dt1.Rows[0]["MIANAME"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT GamMessage";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF";
                        SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "    AND GamJumin = '" + strJumin + "'";
                        SQL = SQL + ComNum.VBLF + "    AND GamCode = '" + StrGbGamek + "'";
                        SQL = SQL + ComNum.VBLF + "    AND (GAMEND >= TO_DATE('" + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D") + "','YYYY-MM-DD') OR GAMEND IS NULL) ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            btnPrint.Enabled = true;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            strBigo = dt1.Rows[0]["GamMessage"].ToString().Trim();
                        }
                        else
                        {
                            if (strBigo2 == "")
                            {
                                strBigo = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_감액코드명", StrGbGamek);
                            }
                            else
                            {
                                strBigo = strBigo2;
                            }
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    strBiGubun = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strBi);
                    strGwaName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, strGwa);

                    nCut = (int)(nAmt % 10);        //절사
                    nAmt = (int)(nAmt / 10) * 10;

                    nSubTot += nAmt;
                    nSubCut += nCut;

                    nTot += nAmt;
                    nCutAmt += nCut;

                    if (strSuNext == "Y92-")
                    {
                        strBigo = strBigo2;
                    }

                    ssView_Sheet1.Cells[nCount - 1, 0].Text = strBigo3;
                    ssView_Sheet1.Cells[nCount - 1, 1].Text = strPano;
                    ssView_Sheet1.Cells[nCount - 1, 2].Text = strBiGubun;
                    ssView_Sheet1.Cells[nCount - 1, 3].Text = strName;
                    ssView_Sheet1.Cells[nCount - 1, 4].Text = strGwaName;
                    ssView_Sheet1.Cells[nCount - 1, 5].Text = strRoom;
                    ssView_Sheet1.Cells[nCount - 1, 6].Text = strDate;
                    ssView_Sheet1.Cells[nCount - 1, 7].Text = strTDate;
                    ssView_Sheet1.Cells[nCount - 1, 8].Text = ((int)(nAmt / 10) * 10).ToString("###,###,##0");
                    ssView_Sheet1.Cells[nCount - 1, 9].Text = nCut.ToString("##0");
                    if (strAmset6 == "*")
                    {
                        ssView_Sheet1.Cells[nCount - 1, 10].Text = "구분변경자";
                    }
                    ssView_Sheet1.Cells[nCount - 1, 11].Text = strBigo;
                }
                dt.Dispose();
                dt = null;

                nSubCnt = 0;
                nCount += 1;
                ssView_Sheet1.RowCount = nCount;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.RowHeader.Cells[nCount - 1, 0].Text = " ";
                ssView_Sheet1.Cells[nCount - 1, 1].Text = "소      계";
                ssView_Sheet1.Cells[nCount - 1, 8].Text = nSubTot.ToString("###,###,##0");
                ssView_Sheet1.Cells[nCount - 1, 9].Text = nSubCut.ToString("##,##0");
                cSPD.setSpdCellColor(ssView, nCount - 1, 0, nCount - 1, ssView.ActiveSheet.ColumnCount - 1, Color.FromArgb(192, 255, 192));
                clsSpread.gSpreadLineBoder(ssView, nCount - 1, 0, nRow - 1, ssView.ActiveSheet.ColumnCount - 1, Color.Black, 3, false, true, false, true);
                nSubTot = 0;
                nSubCut = 0;

                Cursor.Current = Cursors.Default;
                btnPrint.Enabled = true;
                btnPrint.Focus();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                btnPrint.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string Get_SuNameK(string argSuNext)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = " SELECT SUNAMEK FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + argSuNext + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["SUNAMEK"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void frmPmpaViewGPayMagam_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView.Dock = DockStyle.Fill;
            ssPrint.Visible = false;
            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }
    }
}
