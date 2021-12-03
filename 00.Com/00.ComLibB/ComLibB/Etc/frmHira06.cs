using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmHira06
    /// Description     : 심평원자료-저함량
    /// Author          : 이현종
    /// Create Date     : 2018-05-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \basic\busuga\busuga.vbp(FrmHira06) >> frmHira06.cs 폼이름 재정의" />
    public partial class frmHira06 : Form
    {
        public frmHira06()
        {
            InitializeComponent();
        }

        private void frmHira06_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "  SELECT D.SUNEXT SUNEXTA,  B.PNAME PNAMEA, A.LOW_IQTY_MEDC_CD,   A.UNIT_CNT,     E.SUNEXT SUNEXTB , C.PNAME PNAMEB,   A.HIGH_IQTY_MEDC_CD, ";
                SQL = SQL + ComNum.VBLF + "   A.ADPT_FR_DT,  A.ADPT_TO_DT";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.HIRA_TBJBD52 A, ADMIN.EDI_SUGA B, ADMIN.EDI_SUGA C, ADMIN.BAS_SUN D, ADMIN.BAS_SUN E ";
               SQL = SQL + ComNum.VBLF + "   WHERE A.LOW_IQTY_MEDC_CD = B.CODE ";
               SQL = SQL + ComNum.VBLF + "   AND A.HIGH_IQTY_MEDC_CD = C.CODE ";
               SQL = SQL + ComNum.VBLF + "   AND A.LOW_IQTY_MEDC_CD = D.BCODE";
               SQL = SQL + ComNum.VBLF + "   AND  A.HIGH_IQTY_MEDC_CD = E.BCODE(+)";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1_Sheet1.RowCount = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNextA"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PNAMEA"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["LOW_IQTY_MEDC_CD"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["UNIT_CNT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SuNextB"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PNAMEB"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["HIGH_IQTY_MEDC_CD"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ADPT_FR_DT"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ADPT_TO_DT"].ToString().Trim();

                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            Set_Print();
        }

        void Set_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "심평원자료- 저함량 약제 LIST";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력 일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(82) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 180, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, true, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
