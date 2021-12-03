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
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-09-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// 2017-11-03 박창욱 : MISUS211.FRM 폼과 통합
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misuta.vbp\MISUT212.FRM(FrmMisuDiff.frm)" >> frmPmpaViewMisuDiff.cs 폼이름 재정의" />
    /// <seealso cref= D:\psmh\misu\misuta.vbp\MISUS211.FRM(FrmMisuDiff.frm)" >> frmPmpaViewMisuDiff.cs 폼이름 재정의" />

    public partial class frmPmpaViewMisuDiff : Form
    {
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewMisuDiff()
        {
            InitializeComponent();
        }

        private void frmPmpaViewMisuDiff_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int nYY = 0;
            int nMM = 0;

            nYY = (int)VB.Val(VB.Left(strdtP, 4));
            nMM = (int)VB.Val(VB.Mid(strdtP, 6, 2));

            clsVbfunc.SetCboDate(clsDB.DbCon, cboyyyy, 12, "", "1");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            double nSUM1 = 0;
            double nSUM2 = 0;
            double nSUM3 = 0;
            string strFDate = "";
            string strTDate = "";
            string strYYMM = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strYYMM = VB.Left(cboyyyy.Text, 4) + VB.Mid(cboyyyy.Text, 7, 2);
            strFDate = VB.Left(cboyyyy.Text, 4) + "-" + VB.Mid(cboyyyy.Text, 7, 2) + "-01";
            strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

            ssView_Sheet1.RowCount = 0;
            nSUM3 = 0;
            nSUM1 = 0;
            nSUM2 = 0;

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " CREATE OR REPLACE VIEW VIEW_MISU_SLIP_COM AS ";
                SQL = SQL + ComNum.VBLF + " SELECT  A.MISUID, C.SNAME, DECODE(A.IPDOPD,'I','입원','외래') IPDOPD , ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, B.MIRYYMM,  A.AMT AMT1 , 0 AMT2 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_SLIP A , " + ComNum.DB_PMPA + "MISU_IDMST B, " + ComNum.DB_PMPA + "BAS_PATIENT C";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND A.WRTNO = B.WRTNO";
                if (rdoJa.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.CLASS ='07'";
                }
                else if (rdoSan.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.TONGGBN NOT IN ('3')";
                    SQL = SQL + ComNum.VBLF + "   AND A.CLASS ='05'";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN ='11'";

                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD ='I' ";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD ='O' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND A.BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.BDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND A.MISUID = C.PANO(+)";
                SQL = SQL + ComNum.VBLF + " UNION ALL  ";
                SQL = SQL + ComNum.VBLF + " SELECT  A.MISUID, C.SNAME, DECODE(A.IPDOPD,'I','입원','외래') IPDOPD , ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, B.MIRYYMM, 0 AMT1, A.AMT AMT2 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_SLIP A , " + ComNum.DB_PMPA + "MISU_IDMST B, " + ComNum.DB_PMPA + "BAS_PATIENT C";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND A.WRTNO = B.WRTNO";
                if (rdoJa.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.CLASS ='07'";
                }
                else if (rdoSan.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND B.TONGGBN NOT IN ('3')";
                    SQL = SQL + ComNum.VBLF + "   AND A.CLASS ='05'";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.GUBUN ='11'";

                if (rdoIO1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD ='I' ";
                }
                if (rdoIO2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.IPDOPD ='O' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND B.MIRYYMM= '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.MISUID = C.PANO(+)";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT MISUID, SNAME, IPDOPD , BDATE, MIRYYMM, ";
                SQL = SQL + ComNum.VBLF + "   SUM(AMT1) AMT1, SUM(AMT2) AMT2, SUM(AMT1 -AMT2) AMT3 ";
                SQL = SQL + ComNum.VBLF + " FROM VIEW_MISU_SLIP_COM ";
                SQL = SQL + ComNum.VBLF + " GROUP BY MISUID, SNAME, IPDOPD , BDATE, MIRYYMM ";
                SQL = SQL + ComNum.VBLF + " ORDER BY MIRYYMM,BDATE,SNAME,MISUID ";

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

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MISUID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["MIRYYMM"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = VB.Format(Convert.ToDouble(dt.Rows[i]["AMT1"].ToString().Trim()), "##,###,###,###,##0 ");
                    ssView_Sheet1.Cells[i, 6].Text = VB.Format(Convert.ToDouble(dt.Rows[i]["AMT2"].ToString().Trim()), "##,###,###,###,##0 ");
                    ssView_Sheet1.Cells[i, 7].Text = VB.Format(Convert.ToDouble(dt.Rows[i]["AMT3"].ToString().Trim()), "##,###,###,###,##0 ");
                    ssView_Sheet1.Cells[i, 8].Text = " ";

                    nSUM1 = nSUM1 + VB.Val(dt.Rows[i]["AMT1"].ToString().Trim());
                    nSUM2 = nSUM2 + VB.Val(dt.Rows[i]["AMT2"].ToString().Trim());
                    nSUM3 = nSUM3 + VB.Val(dt.Rows[i]["AMT3"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "합계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = VB.Format(Convert.ToDouble(nSUM1), "##,###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = VB.Format(Convert.ToDouble(nSUM2), "##,###,###,###,##0 ");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = VB.Format(Convert.ToDouble(nSUM3), "##,###,###,###,##0 ");
                ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 255, 161);

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

                clsDB.setRollbackTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = cboyyyy.Text;

            if (rdoJa.Checked == true)
            {
                strTitle += " 자보 ";
            }
            else if (rdoSan.Checked == true)
            {
                strTitle += " 산재 ";
            }

            strTitle += "(미수월 <> 통계월) LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (rdoIO1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("입원", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else if (rdoIO2.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("외래", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            else if(rdoIO0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String("전체", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.85f);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
