using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirDtlViewNEW.cs
    /// Description     : 청구 내역 VIEW(MIR_DTL)
    /// Author          : 박성완
    /// Create Date     : 2017-12-12
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\MIRDTLVIEW.frm
    /// </vbp>
    public partial class frmComMirDtlViewNEW : Form
    {
        string gstrSuNext = "";
        string gstrPano = "";
        string gstrYYMM = "";

        /// <summary>
        /// 넘겨받은 데이터를 이용하여 조회
        /// </summary>
        /// <param name="GstrSunext_B">sunext</param>
        /// <param name="GstrPano_B">등록번호</param>
        /// <param name="GstrOutDate_B">퇴원일</param>
        public frmComMirDtlViewNEW(string GstrSunext_B, string GstrPano_B, string GstrOutDate_B)
        {
            gstrSuNext = GstrSunext_B;
            gstrPano = GstrPano_B;
            gstrYYMM = GstrOutDate_B;
            InitializeComponent();

            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                SearchData();
            }

            if (sender == btnExit)
            {
                this.Close();
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                SearchData();
            }
        }

        private void SearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strSunext = "";
            string strYYMM = "";
            string strPano = "";
            //string strSlipDate = "";
            //long nSlipNo = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSunext = gstrSuNext;
            strPano = gstrPano;

            strYYMM = Convert.ToDateTime(gstrYYMM).AddDays(-365).ToString("yyyyMM");

            SQL = "    SELECT A.WRTNO, A.YYMM,   A.PANO, A.SNAME,  A.DEPTCODE1,  A.DTNO , B.SUNEXT,  B.QTY, B.NAL, B.PRICE, B.AMT, C.SUNAMEK,  B.* ";
            SQL = SQL + ComNum.VBLF + "    FROM MIR_INSID A,  MIR_INSDTL B, BAS_SUN C ";
            SQL = SQL + ComNum.VBLF + "   WHERE A.WRTNO = B.WRTNO  ";
            SQL = SQL + ComNum.VBLF + "     AND A.YYMM >='" + strYYMM + "'  ";
            SQL = SQL + ComNum.VBLF + "     AND A.EDIMIRNO >'0' ";  //청구한내역만
            SQL = SQL + ComNum.VBLF + "     AND A.PANO = '" + strPano + "' ";
            SQL = SQL + ComNum.VBLF + "     AND B.SUNEXT = '" + strSunext + "'    ";
            SQL = SQL + ComNum.VBLF + "     AND B.SUNEXT = C.SUNEXT";
            SQL = SQL + ComNum.VBLF + " ORDER BY A.YYMM DESC , A.WRTNO     ";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                ssMain.ActiveSheet.Rows.Count = dt.Rows.Count;
                ssMain.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssMain.Sheets[0].Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 1].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 2].Text = dt.Rows[i]["DeptCode1"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 3].Text = dt.Rows[i]["Sunext"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 4].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 5].Text = dt.Rows[i]["Price"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 6].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 7].Text = dt.Rows[i]["Nal"].ToString().Trim();
                    ssMain.Sheets[0].Cells[i, 8].Text = dt.Rows[i]["Amt"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
