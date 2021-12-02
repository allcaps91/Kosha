using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewABCsak.cs
    /// Description     : 월별 삭감비교
    /// Author          : 박창욱
    /// Create Date     : 2017-08-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MUMAIN10.frm(FrmABCsak.frm) >> frmPmpaViewABCsak.cs 폼이름 재정의" />	
    public partial class frmPmpaViewABCsak : Form
    {
        public frmPmpaViewABCsak()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strFDate = "";
            string strTDate = "";
            int nREAD = 0;
            int i = 0;

            strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 6, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(cboYYMM.Text, 4)), (int)VB.Val(VB.Mid(cboYYMM.Text, 6, 2)));


            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " CREATE OR REPLACE VIEW VIEW_MISU_SAK_SLIP AS ";
                SQL = SQL + ComNum.VBLF + "SELECT MISUID JEPNO , TO_CHAR(BDATE,'YYYY-MM-DD') TDATE , CHASU, AMT, 0 ABCAMT FROM " + ComNum.DB_PMPA + "MISU_SLIP";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND BDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND CLASS IN ('01','02','03','04')";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN ='31'";

                SQL = SQL + ComNum.VBLF + "   UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT  LTRIM(TO_CHAR(A.JEPNO,'00000000')) JEPNO , TO_CHAR(D.TDATE,'YYYY-MM-DD') TDATE , A.CHASU, 0 AMT,";
                SQL = SQL + ComNum.VBLF + "    DECODE( RTRIM(A.JCODE),";
                SQL = SQL + ComNum.VBLF + "                     'E',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  /100 + A.DJAMT, ";
                SQL = SQL + ComNum.VBLF + "                     'L',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  /100 + A.DJAMT, ";
                SQL = SQL + ComNum.VBLF + "                     'F',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  /100 + A.DJAMT , ";
                SQL = SQL + ComNum.VBLF + "                     'TE', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                     'TG', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                       (A.JAMT ) * DECODE(A.GISUL,'2', ( 100 + A.RATEGASAN)/100, 1) * DECODE(C.BOHUN, '3 ', (100 - A.RATEBON) / 100 ,1) +A.DJAMT ) ABCAMT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0203 A,  " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "EDI_F0201 D";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.JEPNO = A.JEPNO";
                SQL = SQL + ComNum.VBLF + "    AND D.CHASU = A.CHASU ";
                SQL = SQL + ComNum.VBLF + "    AND D.MUKNO = A.MUKNO";
                SQL = SQL + ComNum.VBLF + "    AND A.WRTNO = C.WRTNO";
                SQL = SQL + ComNum.VBLF + "    AND ((A.JCODE >='A' AND A.JCODE <='ZZ') AND A.JCode NOT IN ('S2')   )";  //2016-10-13

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT  LTRIM(TO_CHAR(A.JEPNO,'00000000')) JEPNO , TO_CHAR(D.TDATE,'YYYY-MM-DD') TDATE , A.CHASU, 0 AMT,";
                SQL = SQL + ComNum.VBLF + "    DECODE( RTRIM(A.JCODE), ";
                SQL = SQL + ComNum.VBLF + "                     'E',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * ( A.RATEBON)  /100, ";
                SQL = SQL + ComNum.VBLF + "                     'L',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * ( A.RATEBON)  /100, ";
                SQL = SQL + ComNum.VBLF + "                     'F',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * ( A.RATEBON)  /100, ";
                SQL = SQL + ComNum.VBLF + "                     'TE', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                     'TG', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                         (A.JAMT )* DECODE(A.GISUL,'2', ( 100 + A.RATEGASAN)/100, 1)  * ( A.RATEBON /100)  )  ABCAMT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0203 A,  " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "EDI_F0201 D";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.JEPNO = A.JEPNO";
                SQL = SQL + ComNum.VBLF + "    AND D.CHASU = A.CHASU ";
                SQL = SQL + ComNum.VBLF + "    AND D.MUKNO = A.MUKNO";
                SQL = SQL + ComNum.VBLF + "    AND A.WRTNO = C.WRTNO";
                SQL = SQL + ComNum.VBLF + "    AND ((A.JCODE >='A' AND A.JCODE <='ZZ') AND A.JCode NOT IN ('S2')   )";  //2016-10-13
                SQL = SQL + ComNum.VBLF + "    AND C.BOHUN = '3 '"; //보호장애 대불 있을 경우만

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT  LTRIM(TO_CHAR(A.JEPNO,'00000000')) JEPNO, TO_CHAR(D.TDATE,'YYYY-MM-DD') TDATE , A.CHASU,  0 AMT, ";
                SQL = SQL + ComNum.VBLF + " DECODE( RTRIM(A.JCODE),";
                SQL = SQL + ComNum.VBLF + "                  'E',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  /100 + A.DJAMT, ";
                SQL = SQL + ComNum.VBLF + "                  'L',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  /100 + A.DJAMT, ";
                SQL = SQL + ComNum.VBLF + "                  'F',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * (100 - A.RATEBON)  /100 + A.DJAMT , ";
                SQL = SQL + ComNum.VBLF + "                  'TE', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                  'TG', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                       (A.JAMT ) * DECODE(A.GISUL,'2', ( 100 + A.RATEGASAN)/100, 1) * DECODE(C.BOHUN, '3 ',(100 - A.RATEBON) /100 ,1) + a.DJAMT ) ABCAMT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0603 A, " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "EDI_F0601 D, " + ComNum.DB_PMPA + "BAS_MIA E ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.JEPNO = A.JEPNO";
                SQL = SQL + ComNum.VBLF + "    AND D.CHASU = A.CHASU ";
                SQL = SQL + ComNum.VBLF + "    AND D.MUKNO = A.MUKNO";
                SQL = SQL + ComNum.VBLF + "    AND A.WRTNO = C.WRTNO";
                SQL = SQL + ComNum.VBLF + "    AND ((A.JCODE >='A' AND A.JCODE <='ZZ') AND A.JCode NOT IN ('S2')   )";  //2016-10-13
                SQL = SQL + ComNum.VBLF + "    AND C.KIHO = E.MIACODE(+)";

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT  LTRIM(TO_CHAR(A.JEPNO,'00000000')) JEPNO, TO_CHAR(D.TDATE,'YYYY-MM-DD') TDATE , A.CHASU,  0 AMT, ";
                SQL = SQL + ComNum.VBLF + " DECODE( RTRIM(A.JCODE),";
                SQL = SQL + ComNum.VBLF + "                  'E', ( A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * ( A.RATEBON)  /100, ";
                SQL = SQL + ComNum.VBLF + "                  'L', ( A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * ( A.RATEBON)  /100, ";
                SQL = SQL + ComNum.VBLF + "                  'F',  (A.JAMT ) * DECODE(A.GISUL,'2', (100 + A.RATEGASAN)/100, 1) * ( A.RATEBON)  /100, ";
                SQL = SQL + ComNum.VBLF + "                  'TE', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                  'TG', C.EDIJAMT,  ";
                SQL = SQL + ComNum.VBLF + "                       (A.JAMT ) * DECODE(A.GISUL,'2', ( 100 + A.RATEGASAN)/100, 1) * ( A.RATEBON /100)  ) ABCAMT";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "EDI_F0603 A, " + ComNum.DB_PMPA + "MIR_INSID C, " + ComNum.DB_PMPA + "EDI_F0601 D, " + ComNum.DB_PMPA + "BAS_MIA E ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE >=TO_DATE('" + strFDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.TDATE <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "    AND D.JEPNO = A.JEPNO";
                SQL = SQL + ComNum.VBLF + "    AND D.CHASU = A.CHASU ";
                SQL = SQL + ComNum.VBLF + "    AND D.MUKNO = A.MUKNO";
                SQL = SQL + ComNum.VBLF + "    AND A.WRTNO = C.WRTNO";
                SQL = SQL + ComNum.VBLF + "    AND ((A.JCODE >='A' AND A.JCODE <='ZZ') AND A.JCode NOT IN ('S2')   )";  //2016-10-13
                SQL = SQL + ComNum.VBLF + "    AND C.BOHUN = '3 '"; //보호장애 대불 있을 경우만
                SQL = SQL + ComNum.VBLF + "    AND C.KIHO = E.MIACODE(+)";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT TDATE, JEPNO, CHASU, SUM(AMT) AMT, SUM(TRUNC(ABCAMT)) ABCAMT,  SUM(AMT - TRUNC(ABCAMT)) CHAMT ";
                SQL = SQL + ComNum.VBLF + "  FROM   VIEW_MISU_SAK_SLIP    ";
                SQL = SQL + ComNum.VBLF + "   GROUP BY TDATE , JEPNO, CHASU";
                SQL = SQL + ComNum.VBLF + "   ORDER BY 1 , 2 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);

                nREAD = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nREAD;
                for (i = 0; i < nREAD; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JEPNO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ChaSu"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["ABCAMT"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["CHAMT"].ToString().Trim()).ToString("###,###,###,##0");
                }

                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strHead3 = "";
            string PrintDate = "";
            string JName = "";

            if (ssView_Sheet1.Cells[0, 0].Text == "")
            {
                return;
            }

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            JName = clsType.User.JobName;

            //Print Head
            strFont1 = "/c/fn\"굴림체\" /fz\"15\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/l/f1" + VB.Space(30) + cboYYMM.Text + "미수와 ABC 삭감 비교";
            strHead2 = "/l/f2" + "출력시간 : " + PrintDate + "/c/f2" + strHead3 + "/r/f2" + "출력자 : " + JName + " 인 " + VB.Space(8);

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 0;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 100;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void frmPmpaViewABCsak_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 10, "", "0");
        }
    }
}
