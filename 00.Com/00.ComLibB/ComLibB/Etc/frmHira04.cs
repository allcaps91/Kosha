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
    /// File Name       : frmHira04
    /// Description     : 심평원자료-최대용량
    /// Author          : 이현종
    /// Create Date     : 2018-05-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= " \basic\busuga\busuga.vbp(FrmHira04) >> frmHira04.cs 폼이름 재정의" />
    public partial class frmHira04 : Form
    {
        public frmHira04()
        {
            InitializeComponent();
        }

        private void frmHira04_Load(object sender, EventArgs e)
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
                SQL = " SELECT C.SUNEXT,  C.SUNAMEK, A.GNL_NM_CD,  A.DD_MAX_QTY_FREQ,  TO_CHAR(D.DELDATE,'YYYYMMDD') DELDATE   ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.HIRA_TBJBD55 A ,    KOSMOS_PMPA.EDI_SUGA B ,  KOSMOS_PMPA.BAS_SUN C , KOSMOS_PMPA.BAS_SUT D";
                SQL = SQL + ComNum.VBLF + "   WHERE A.GNL_NM_CD =  B.SCODE";
                SQL = SQL + ComNum.VBLF + "     AND B.CODE = C.BCODE";
                SQL = SQL + ComNum.VBLF + "     AND A.ADPT_FR_DT <= '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") +  "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.ADPT_TYPE = '0' ";
                SQL = SQL + ComNum.VBLF + "     AND A.GUBUN_CD = '1' "; //'최대용량
                SQL = SQL + ComNum.VBLF + "     AND C.SUNEXT =D.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "     AND  B.CODE NOT IN  ";
                SQL = SQL + ComNum.VBLF + "          (SELECT B.MEDC_CD ";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_PMPA.HIRA_TBJBD56 B ";
                SQL = SQL + ComNum.VBLF + "          WHERE B.GNL_NM_CD = A.GNL_NM_CD)";
                SQL = SQL + ComNum.VBLF + "   ORDER BY C.SUNEXT, A.ADPT_FR_DT DESC";

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
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SuNameK"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GNL_NM_CD"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DD_MAX_QTY_FREQ"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 4].Text = ComFunc.FormatStrToDateTime(dt.Rows[i]["DelDate"].ToString().Trim(), "D");
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

            strTitle = "심평원자료- 최대용량금기 약제 LIST";

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
