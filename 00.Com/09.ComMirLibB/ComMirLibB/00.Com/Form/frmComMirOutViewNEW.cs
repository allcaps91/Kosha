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
    public partial class frmComMirOutViewNEW : Form
    {
        private string gstrSuNext;
        private string gstrPano;
        private string gstrYYMM;

        /// <summary>
        /// 넘겨받은 데이터를 이용하여 조회
        /// </summary>
        /// <param name="GstrSunext_B">sunext</param>
        /// <param name="GstrPano_B">등록번호</param>
        /// <param name="GstrOutDate_B">퇴원일</param>
        public frmComMirOutViewNEW(string GstrSunext_B, string GstrPano_B, string GstrOutDate_B)
        {
            gstrSuNext = GstrSunext_B;
            gstrPano = GstrPano_B;
            gstrYYMM = GstrOutDate_B;

            InitializeComponent();
        }

        private void frmComMirOutViewNEW_Load(object sender, EventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
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
            string strSlipDate = "";
            long nSlipNo = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strSunext = gstrSuNext;
            strPano = gstrPano;
            strYYMM = Convert.ToDateTime(gstrYYMM).AddDays(-365).ToString("yyyyMM");

            SQL = "    SELECT A.WRTNO, A.YYMM, A.PANO, A.SNAME, A.DEPTCODE1, "+ ComNum.VBLF;
            SQL += "     B.Bun , B.Sucode, B.Qty, B.DivQty, B.Div, B.Nal, B.EdiCode, B.EdiQty, B.Multi, B.MultiRemark, B.SCODESAYU, B.SCODEREMARK, B.ROWID, B.WRTNOS, "+ ComNum.VBLF;
            SQL += "     C.GBV252, C.SLIPDATE, C.SLIPNO, D.SuNameK,E.Pname,E.Danwi1,E.Danwi2, E.Spec, D.DAICODE, F.CLASSNAME  "+ ComNum.VBLF;
            SQL += "    FROM MIR_INSID A,  MIR_OUTDRUG B, MIR_OUTDRUGMST C,  BAS_SUN D , EDI_SUGA E , BAS_CLASS F  "+ ComNum.VBLF;
            SQL += "   WHERE A.WRTNO = B.WRTNO "+ ComNum.VBLF;
            SQL += "     AND A.WRTNO = C.WRTNO "+ ComNum.VBLF;
            SQL += "     AND A.YYMM >='" + strYYMM + "'  "+ ComNum.VBLF;
            SQL += "     AND A.EDIMIRNO >'0' "+ ComNum.VBLF; //청구한내역만
            SQL += "     AND A.PANO = '" + strPano + "' "+ ComNum.VBLF;
            SQL += "     AND B.SUCODE = '" + strSunext + "'    "+ ComNum.VBLF;
            SQL += "     AND B.SUCODE = D.SUNEXT"+ ComNum.VBLF;
            SQL += "   AND (B.GBSELF IS NULL OR B.GBSELF ='0') "+ ComNum.VBLF;
            SQL += "   AND B.FLAG='Y'        "+ ComNum.VBLF;
            SQL += "   AND C.FLAG='P'        "+ ComNum.VBLF;
            SQL += "   AND B.SuCode= D.SuNext "+ ComNum.VBLF;
            SQL += "   AND B.EdiCode=E.Code(+) "+ ComNum.VBLF;
            SQL += "   AND D.DAICODE = F.CLASSCODE(+) "+ ComNum.VBLF;
            SQL += " ORDER BY A.YYMM DESC , A.WRTNO     "+ ComNum.VBLF;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                ssMain.Sheets[0].Rows.Count = dt.Rows.Count;
                ssMain.Sheets[0].SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssMain.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["YYMM"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DeptCode1"].ToString().Trim();

                    strSlipDate = dt.Rows[i]["SlipDate"].ToString().Trim().Replace("-", "");
                    nSlipNo = Convert.ToInt64(dt.Rows[i]["SlipNo"]);
                    ssMain.ActiveSheet.Cells[i, 3].Text = strSlipDate + "-" + string.Format("{0:00000}", nSlipNo);

                    ssMain.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Sucode"].ToString().Trim(); //수가 코드
                    ssMain.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Qty"].ToString().Trim();    //1일 투여
                    ssMain.ActiveSheet.Cells[i, 6].Text = string.Format("{0:#####0.00}", dt.Rows[i]["DivQty"]);   //1회투여
                    ssMain.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Div"].ToString().Trim();    //1일 투여횟수
                    ssMain.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Nal"].ToString().Trim();    //날수
                    ssMain.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["GBV252"].ToString().Trim() == "Y" ? "*" : "" + dt.Rows[i]["SuNameK"].ToString().Trim(); //수가명칭
                    ssMain.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["EdiCode"].ToString().Trim();   //표준코드
                    ssMain.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["PName"].ToString().Trim();     //표준코드명
                    ssMain.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["DAICODE"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["CLASSNAME"].ToString().Trim();

                    ssMain.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["Multi"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["MultiRemark"].ToString().Trim();

                    ssMain.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["SCODESAYU"].ToString().Trim();
                    if (dt.Rows[i]["SCODESAYU"].ToString().Trim() != "")
                    {
                        ssMain.ActiveSheet.Columns[-1].BackColor = System.Drawing.Color.FromArgb(255, 230, 230);
                    }
                    ssMain.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["SCODEREMARK"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 19].Text = dt.Rows[i]["WRTNOS"].ToString().Trim();
                    ssMain.ActiveSheet.Cells[i, 20].Text = strSlipDate + "-" + string.Format("{0:00000}", nSlipNo);
                    ssMain.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["GBV252"].ToString() == "Y" ? "★" : "";

                    if (Convert.ToInt32(dt.Rows[i]["WRTNOS"]) > 0)
                    {
                        ssMain.ActiveSheet.Columns[-1].ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                    }

                    SQL = " SELECT SUNEXT, RGB FROM KOSMOS_PMPA.MIR_COLOR_SET " + ComNum.VBLF;
                    SQL += "  WHERE SABUN = '" + clsType.User.Sabun + "' " + ComNum.VBLF;
                    SQL += "    AND SUNEXT = '" + dt.Rows[i]["Sucode"].ToString().Trim() + "' " + ComNum.VBLF;

                    DataTable dt1 = null;
                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["RGB"].ToString() != "")
                        {
                            ssMain.ActiveSheet.Columns[-1].BackColor = System.Drawing.ColorTranslator.FromWin32(Convert.ToInt32(dt1.Rows[i]["RGB"]));
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void ssMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.Close();
            }
        }
    }
}
