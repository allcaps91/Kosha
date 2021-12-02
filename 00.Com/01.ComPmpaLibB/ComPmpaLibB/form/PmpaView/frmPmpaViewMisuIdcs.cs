using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMisuIdcs.cs
    /// Description     : 개인별 미수내역 상세조회
    /// Author          : 박창욱
    /// Create Date     : 2017-10-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\MUMAIN07.FRM(FrmMisuidView.frm) >> frmPmpaViewMisuIdcs.cs 폼이름 재정의" />
    public partial class frmPmpaViewMisuIdcs : Form
    {
        clsPmpaFunc cpf = new clsPmpaFunc();
        clsPmpaMisu cpm = new clsPmpaMisu();
        double GnWRTNO = 2845;

        public frmPmpaViewMisuIdcs()
        {
            InitializeComponent();
        }

        public frmPmpaViewMisuIdcs(double nWRTNO)
        {
            InitializeComponent();
            GnWRTNO = nWRTNO;
        }

        private void frmPmpaViewMisuIdcs_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

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
                cpm.READ_MISU_IDMST((long)GnWRTNO);
                if (clsPmpaType.TMM.ROWID.Trim() == "")
                {
                    ComFunc.MsgBox("미수내역이 없습니다.");
                    return;
                }

                ssView_Sheet1.Cells[1, 0].Text = cpm.READ_MisuClass(clsPmpaType.TMM.Class.Trim());
                ssView_Sheet1.Cells[1, 1].Text = cpm.READ_BAS_MIA(clsPmpaType.TMM.GelCode.Trim());
                ssView_Sheet1.Cells[1, 2].Text = clsPmpaType.TMM.MisuID;
                ssView_Sheet1.Cells[1, 3].Text = clsPmpaType.TMM.BDate;
                ssView_Sheet1.Cells[1, 4].Text = clsPmpaType.TMM.FromDate;
                ssView_Sheet1.Cells[1, 5].Text = clsPmpaType.TMM.ToDate;
                ssView_Sheet1.Cells[1, 6].Text = cpm.READ_MisuBunya(clsPmpaType.TMM.Bun.Trim());
                ssView_Sheet1.Cells[1, 7].Text = cpm.READ_MisuIpdOpd(clsPmpaType.TMM.IpdOpd.Trim());
                if (clsPmpaType.TMM.Class.Trim() == "05" || clsPmpaType.TMM.Class.Trim() == "07")
                {
                    ssView_Sheet1.Cells[1, 9].Text = clsVbfunc.GetPatientName(clsDB.DbCon, clsPmpaType.TMM.MisuID);
                }

                //건수 및 금액을 Display
                ssView_Sheet1.Cells[4, 2].Text = clsPmpaType.TMM.Qty[1].ToString("###,##0");
                ssView_Sheet1.Cells[4, 3].Text = clsPmpaType.TMM.Qty[2].ToString("###,##0");
                ssView_Sheet1.Cells[4, 4].Text = clsPmpaType.TMM.Qty[3].ToString("###,##0");
                ssView_Sheet1.Cells[4, 5].Text = clsPmpaType.TMM.Qty[4].ToString("###,##0");

                ssView_Sheet1.Cells[5, 1].Text = clsPmpaType.TMM.Amt[1].ToString("###,###,###,##0");    //총진료비
                ssView_Sheet1.Cells[5, 2].Text = clsPmpaType.TMM.Amt[2].ToString("###,###,###,##0");    //청구금액
                ssView_Sheet1.Cells[5, 3].Text = clsPmpaType.TMM.Amt[3].ToString("###,###,###,##0");    //입금액
                ssView_Sheet1.Cells[5, 4].Text = clsPmpaType.TMM.Amt[4].ToString("###,###,###,##0");    //삭감액
                ssView_Sheet1.Cells[5, 5].Text = clsPmpaType.TMM.Amt[8].ToString("###,###,###,##0");    //반송액
                ssView_Sheet1.Cells[5, 6].Text = clsPmpaType.TMM.Amt[5].ToString("###,###,###,##0");    //삭감절산
                ssView_Sheet1.Cells[5, 7].Text = clsPmpaType.TMM.Amt[6].ToString("###,###,###,##0");    //과지급금
                ssView_Sheet1.Cells[5, 8].Text = clsPmpaType.TMM.Amt[7].ToString("###,###,###,##0");    //계산착오

                nJanAmt = clsPmpaType.TMM.Amt[2] - clsPmpaType.TMM.Amt[3] - clsPmpaType.TMM.Amt[4] - clsPmpaType.TMM.Amt[5];
                nJanAmt -= clsPmpaType.TMM.Amt[6] - clsPmpaType.TMM.Amt[7] - clsPmpaType.TMM.Amt[8];
                ssView_Sheet1.Cells[5, 9].Text = nJanAmt.ToString("###,###,###,##0");

               

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
                    ssView2_Sheet1.Cells[i, 1].Text = cpm.READ_MisuGye_TA(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("###,###,###,##0");
                    ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[i, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["EntPart"].ToString().Trim());
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
            GnWRTNO = 0;
            this.Close();
        }
    }
}
