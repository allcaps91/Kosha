using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPanoMisuDtl.cs
    /// Description     : 개인별 미수내역 상세조회
    /// Author          : 박창욱
    /// Create Date     : 2017-10-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs82.frm(FrmPanoMisuDtl.frm) >> frmPmpaViewPanoMisuDtl.cs 폼이름 재정의" />
    public partial class frmPmpaViewPanoMisuDtl : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();
        double GnWRTNO = 0;

        public frmPmpaViewPanoMisuDtl()
        {
            InitializeComponent();
        }

        public frmPmpaViewPanoMisuDtl(double nWRTNO)
        {
            InitializeComponent();
            GnWRTNO = nWRTNO;
        }

        private void frmPmpaViewPanoMisuDtl_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;

            for (i = 1; i < 10; i++)
            {
                ssView_Sheet1.Cells[1, i - 1].Text = "";
            }
            for (i = 2; i < 10; i++)
            {
                ssView_Sheet1.Cells[4, i - 1].Text = "";
                ssView_Sheet1.Cells[5, i - 1].Text = "";
            }

            ssView2_Sheet1.RowCount = 0;
            ssView2_Sheet1.RowCount = 1;

            ssView3_Sheet1.RowCount = 0;
            ssView3_Sheet1.RowCount = 1;

            Screen_Display();
        }

        void Screen_Display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            double nJanAmt = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT MisuID,TO_CHAR(Bdate,'YYYY-MM-DD') Bdate, Class,";
                SQL = SQL + ComNum.VBLF + "       IpdOpd, Bi, GelCode,";
                SQL = SQL + ComNum.VBLF + "       Bun, TO_CHAR(FromDate,'YYYY-MM-DD') Fdate, TO_CHAR(ToDate,'YYYY-MM-DD') Tdate,";
                SQL = SQL + ComNum.VBLF + "       Ilsu, DeptCode, MgrRank,";
                SQL = SQL + ComNum.VBLF + "       Qty1, Qty2, Qty3,";
                SQL = SQL + ComNum.VBLF + "       Qty4, Amt1, Amt2,";
                SQL = SQL + ComNum.VBLF + "       Amt3, Amt4, Amt5,";
                SQL = SQL + ComNum.VBLF + "       Amt6, Amt7, JepsuNo,";
                SQL = SQL + ComNum.VBLF + "       GbEnd, Remark, ROWID,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, EntPart, TongGbn,";
                SQL = SQL + ComNum.VBLF + "       MirYYMM, EdiMirNo, ChaSu,";
                SQL = SQL + ComNum.VBLF + "       MukNo";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + GnWRTNO + " ";

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

                ssView_Sheet1.Cells[1, 0].Text = cpm.READ_MisuClass(dt.Rows[0]["Class"].ToString().Trim());
                ssView_Sheet1.Cells[1, 1].Text = cpm.READ_BAS_MIA(dt.Rows[0]["GelCode"].ToString().Trim());
                ssView_Sheet1.Cells[1, 2].Text = dt.Rows[0]["MisuID"].ToString().Trim();
                ssView_Sheet1.Cells[1, 3].Text = dt.Rows[0]["BDate"].ToString().Trim();
                ssView_Sheet1.Cells[1, 4].Text = dt.Rows[0]["FDate"].ToString().Trim();
                ssView_Sheet1.Cells[1, 5].Text = dt.Rows[0]["TDate"].ToString().Trim();
                ssView_Sheet1.Cells[1, 6].Text = cpm.READ_MisuBunName(dt.Rows[0]["Bun"].ToString().Trim());
                if (dt.Rows[0]["IpdOpd"].ToString().Trim() == "I")
                {
                    ssView_Sheet1.Cells[2, 7].Text = "입원";
                }
                else if (dt.Rows[0]["IpdOpd"].ToString().Trim() == "O")
                {
                    ssView_Sheet1.Cells[2, 7].Text = "외래";
                }
                else
                {
                    ssView_Sheet1.Cells[2, 7].Text = "";
                }

                //건수 및 금액을 Display
                ssView_Sheet1.Cells[4, 2].Text = VB.Val(dt.Rows[0]["Qty1"].ToString().Trim()).ToString("###,##0");
                ssView_Sheet1.Cells[4, 3].Text = VB.Val(dt.Rows[0]["Qty2"].ToString().Trim()).ToString("###,##0");
                ssView_Sheet1.Cells[4, 4].Text = VB.Val(dt.Rows[0]["Qty3"].ToString().Trim()).ToString("###,##0");
                ssView_Sheet1.Cells[4, 5].Text = VB.Val(dt.Rows[0]["Qty4"].ToString().Trim()).ToString("###,##0");

                ssView_Sheet1.Cells[5, 1].Text = VB.Val(dt.Rows[0]["Amt1"].ToString().Trim()).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[5, 2].Text = VB.Val(dt.Rows[0]["Amt2"].ToString().Trim()).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[5, 3].Text = VB.Val(dt.Rows[0]["Amt3"].ToString().Trim()).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[5, 4].Text = VB.Val(dt.Rows[0]["Amt4"].ToString().Trim()).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[5, 5].Text = VB.Val(dt.Rows[0]["Amt5"].ToString().Trim()).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[5, 6].Text = VB.Val(dt.Rows[0]["Amt6"].ToString().Trim()).ToString("###,###,###,##0");
                ssView_Sheet1.Cells[5, 7].Text = VB.Val(dt.Rows[0]["Amt7"].ToString().Trim()).ToString("###,###,###,##0");

                //잔액을 계산
                nJanAmt = VB.Val(dt.Rows[0]["Amt2"].ToString().Trim()) - VB.Val(dt.Rows[0]["Amt3"].ToString().Trim());
                nJanAmt -= VB.Val(dt.Rows[0]["Amt4"].ToString().Trim()) - VB.Val(dt.Rows[0]["Amt5"].ToString().Trim());
                nJanAmt -= VB.Val(dt.Rows[0]["Amt6"].ToString().Trim()) - VB.Val(dt.Rows[0]["Amt7"].ToString().Trim());
                ssView_Sheet1.Cells[5, 8].Text = nJanAmt.ToString("###,###,###,##0");

                dt.Dispose();
                dt = null;


                //MISU_SLIP Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(Bdate,'YYYY-MM-DD') Bdate, Gubun, Qty,";
                SQL = SQL + ComNum.VBLF + "       Amt, Remark, TO_CHAR(EntDate,'YY-MM-DD HH24:MI') EntDate,";
                SQL = SQL + ComNum.VBLF + "       EntPart";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + GnWRTNO + " ";
                SQL = SQL + ComNum.VBLF + " ORDER BY Bdate,Gubun ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;

                ssView2_Sheet1.RowCount = nRead;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < nRead; i++)
                {
                    ssView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 1].Text = cpm.READ_SlipBunName(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 6].Text = clsVbfunc.GetPassName(clsDB.DbCon, dt.Rows[i]["EntPart"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                //MISU_MONTHLY Display
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT YYMM, IwolAmt, MisuAmt,";
                SQL = SQL + ComNum.VBLF + "       IpgumAmt, SakAmt, BanAmt,";
                SQL = SQL + ComNum.VBLF + "       EtcAmt, JanAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_MONTHLY ";
                SQL = SQL + ComNum.VBLF + " WHERE WRTNO=" + GnWRTNO + " ";
                SQL = SQL + ComNum.VBLF + " ORDER BY YYMM ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView3_Sheet1.RowCount = nRead;
                ssView3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < nRead; i++)
                {
                    ssView3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                    ssView3_Sheet1.Cells[i, 1].Text = VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 2].Text = VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 4].Text = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 5].Text = VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView3_Sheet1.Cells[i, 7].Text = VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()).ToString("###,###,###,##0");
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
