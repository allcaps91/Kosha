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
    /// File Name       : frmPmpaViewOmittedMisuPrint.cs
    /// Description     : 금일 현재 미처리 미수 명세서
    /// Author          : 박창욱
    /// Create Date     : 2017-10-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\ilrepd\ILREPD04.FRM(FrmMisuPrint.frm) >> frmPmpaViewOmittedMisuPrint.cs 폼이름 재정의" />	
    public partial class frmPmpaViewOmittedMisuPrint : Form
    {
        clsSpread CS = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsPmpaFunc cpf = new clsPmpaFunc();

        public frmPmpaViewOmittedMisuPrint()
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

            strTitle = "미처리 미수금 명세서";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

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
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nRead = 0;
            int nIDno = 0;
            double nTotBAmt = 0;
            double nTotIAmt = 0;
            double nTotMAmt = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            string strPANO = "";

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //입금완료된것 완료 Set Setting
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,SUM(DECODE(Gubun1,'1',Amt,0)) Amt1,";
                SQL = SQL + ComNum.VBLF + "        SUM(DECODE(Gubun1,'1',0,Amt)) Amt2";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE FLAG <> '*'";
                SQL = SQL + ComNum.VBLF + "  GROUP BY Pano";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                    nAmt1 = VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());   //발생액
                    nAmt2 = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());   //입금액

                    if (nAmt1 == nAmt2)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GAINSLIP SET";
                        SQL = SQL + ComNum.VBLF + "        Flag = '*'";
                        SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + strPANO + "'";
                        SQL = SQL + ComNum.VBLF + "    AND Flag <> '*'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("MISU_GAINSLIP UPDATE 중 ERROR 발생!!, 전산실로 연락바랍니다.");
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                //누적할 변수를 Clear
                nTotBAmt = 0;
                nTotIAmt = 0;
                nTotMAmt = 0;

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 1;


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Pano,Gubun1, TO_CHAR(Bdate,'yy-mm-dd') Bdate, Amt,";
                SQL = SQL + ComNum.VBLF + "        Remark, IDno";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP";
                SQL = SQL + ComNum.VBLF + "  WHERE FLAG <> '*'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Pano, Bdate, Gubun1";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead + 1;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 0].Text = strPANO;
                    ssView_Sheet1.Cells[i, 1].Text = cpf.Get_BasPatient(clsDB.DbCon, strPANO).Rows[0]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    switch (dt.Rows[i]["Gubun1"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                            nTotBAmt += VB.Val(dt.Rows[0]["Amt"].ToString().Trim());
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                            nTotIAmt += VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                            break;
                    }
                    nTotMAmt -= nTotIAmt;
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    nIDno = (int)VB.Val(dt.Rows[i]["IDno"].ToString().Trim());

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Name FROM " + ComNum.DB_PMPA + "BAS_PASS";
                    SQL = SQL + ComNum.VBLF + "  WHERE IDnumber = " + nIDno;
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        ssView_Sheet1.Cells[i, 6].Text = dt1.Rows[0]["Name"].ToString().Trim();
                    }
                    dt1.Dispose();
                    dt1 = null;
                }

                //합계액을 Display
                nTotMAmt = nTotBAmt - nTotIAmt;
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "* 합계 *";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = nTotBAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = nTotIAmt.ToString("###,###,###,##0");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = "-< 미수합계 : " + nTotMAmt.ToString("###,###,###,##0") + "원 >-";

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회 완료");
                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewOmittedMisuPrint_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            btnPrint.Enabled = false;
        }
    }
}
