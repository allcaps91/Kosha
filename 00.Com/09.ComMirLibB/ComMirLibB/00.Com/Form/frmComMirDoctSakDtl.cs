using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmMirTongDoctSakChkDtl.cs
    /// Description     : 삭감내역 상세 List
    /// Author          : 박성완
    /// Create Date     : 2017-12-15
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 자식폼 - MirTong에 있는거 복사해옴
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\miretc\miretc11.frm
    public partial class frmComMirDoctSakDtl : Form
    {
        string strJong = "";

        public frmComMirDoctSakDtl(string argHelpCode, string argJepNo, string argChasu, string argMukNo)
        {
            
            InitializeComponent();
            strJong = argHelpCode;
            txtJepNo.Text = argJepNo;
            txtChasu.Text = argChasu;
            txtMukNo.Text = argMukNo;

            setEvent();
        }

        private void setEvent()
        {
            this.Load += FrmMirTongDoctSakChkDtl_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExit.Click += BtnExit_Click;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            long nAMT1 = 0;
            long nAMT2 = 0;
            long nSum1 = 0;
            long nSum2 = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            //뷰 생성
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " CREATE OR REPLACE VIEW VIEW_SAK AS " + ComNum.VBLF;

                if (strJong == "1")
                {
                    SQL += " SELECT C.PANO, TO_CHAR(A.SIMDATE,'YYYY-MM-DD') TDATE, B.JCODE, SUM(B.JAMT)  AMT1, 0 AMT2, " + ComNum.VBLF;
                    SQL += "        D.NAME " + ComNum.VBLF;
                    SQL += "  FROM KOSMOS_PMPA.EDI_JEPSU A , KOSMOS_PMPA.EDI_RESULT3 B, KOSMOS_PMPA.MIR_INSID C, KOSMOS_PMPA.EDI_LCODE D" + ComNum.VBLF;
                    SQL += " WHERE A.JEPNO = " + txtJepNo.Text + " " + ComNum.VBLF;
                    if (txtMukNo.Text != "") { SQL = SQL + "   AND A.MUKNO = " + txtMukNo.Text + " " + ComNum.VBLF; }
                    if (txtChasu.Text != "") { SQL = SQL + "   AND A.CHASU = " + txtChasu.Text + " " + ComNum.VBLF; }
                    SQL += "   AND B.JCODE NOT IN('91','36','64','38','36','46','80','84','16','50','S2')" + ComNum.VBLF;
                    SQL += "   AND A.JEPNO = B.JEPNO" + ComNum.VBLF;
                    SQL += "   AND A.MUKNO = B.MUKNO " + ComNum.VBLF;
                    SQL += "   AND A.CHASU = B.CHASU" + ComNum.VBLF;
                    SQL += "   AND B.WRTNO = C.WRTNO" + ComNum.VBLF;
                    SQL += "   AND B.JCODE = D.CODE " + ComNum.VBLF;
                    SQL += "   AND D.JONG ='B'" + ComNum.VBLF;
                    SQL += " GROUP BY C.PANO, A.SIMDATE , B.JCODE, D.NAME " + ComNum.VBLF;
                    SQL += " UNION ALL " + ComNum.VBLF;
                    SQL += " SELECT C.PANO, TO_CHAR(A.TDATE,'YYYY-MM-DD') TDATE, A.JCODE, 0 AMT1, SUM(A.JAMT) AMT2, " + ComNum.VBLF;
                    SQL += "        D.NAME " + ComNum.VBLF;
                    SQL += "  FROM KOSMOS_PMPA.SAK_SIMSADOCT A, KOSMOS_PMPA.MIR_INSID C, KOSMOS_PMPA.EDI_LCODE D " + ComNum.VBLF;
                    SQL += " WHERE A.JEPNO = " + txtJepNo.Text + " " + ComNum.VBLF;
                    if (txtMukNo.Text != "") { SQL = SQL + "   AND A.MUKNO = " + txtMukNo.Text + " " + ComNum.VBLF; }
                    if (txtChasu.Text != "") { SQL = SQL + "   AND A.CHASU = " + txtChasu.Text + " " + ComNum.VBLF; }
                    SQL += "   AND A.JOHAP IN ('1','5') " + ComNum.VBLF;
                    SQL += "   AND A.WRTNO = C.WRTNO " + ComNum.VBLF;
                    SQL += "   AND A.JCODE = D.CODE " + ComNum.VBLF;
                    SQL += "   AND D.JONG ='B'" + ComNum.VBLF;
                    SQL += " GROUP BY C.PANO, A.TDATE, A.JCODE, D.NAME " + ComNum.VBLF;
                }
                else
                {
                    SQL += " SELECT C.PANO, TO_CHAR(A.TDATE,'YYYY-MM-DD') TDATE, B.JCODE, SUM(B.JAMT)  AMT1, 0 AMT2," + ComNum.VBLF;
                    SQL += "        D.NAME " + ComNum.VBLF;
                    SQL += "  FROM KOSMOS_PMPA.EDI_SANRESULT2 A , KOSMOS_PMPA.EDI_SANRESULT3 B, KOSMOS_PMPA.MIR_SANID C, KOSMOS_PMPA.EDI_LCODE D " + ComNum.VBLF;
                    SQL += " WHERE A.JEPNO = " + txtJepNo.Text + " " + ComNum.VBLF;
                    if (txtMukNo.Text != "") { SQL = SQL + "   AND A.MUKNO = " + txtMukNo.Text + " " + ComNum.VBLF; }
                    if (txtChasu.Text != "") { SQL = SQL + "   AND A.CHASU = " + txtChasu.Text + " " + ComNum.VBLF; }
                    SQL += "   AND B.JCODE NOT IN('91','36','64','38','36','46','80','84','S2')" + ComNum.VBLF;
                    SQL += "   AND A.JEPNO = B.JEPNO" + ComNum.VBLF;
                    SQL += "   AND A.MUKNO = B.MUKNO " + ComNum.VBLF;
                    SQL += "   AND A.CHASU = B.CHASU" + ComNum.VBLF;
                    SQL += "   AND A.WRTNO = C.WRTNO" + ComNum.VBLF;
                    SQL += "   AND B.JCODE = D.CODE " + ComNum.VBLF;
                    SQL += "   AND D.JONG ='E'" + ComNum.VBLF;
                    SQL += " GROUP BY C.PANO, A.TDATE, B.JCODE" + ComNum.VBLF;
                    SQL += " UNION ALL " + ComNum.VBLF;
                    SQL += " SELECT C.PANO, TO_CHAR(A.TDATE,'YYYY-MM-DD') TDATE, A.JCODE, 0 AMT1, SUM(A.JAMT) AMT2, " + ComNum.VBLF;
                    SQL += "        D.NAME " + ComNum.VBLF;
                    SQL += "  FROM KOSMOS_PMPA.SAK_SIMSADOCT A, KOSMOS_PMPA.MIR_SANID C, KOSMOS_PMPA.EDI_LCODE D" + ComNum.VBLF;
                    SQL += " WHERE A.JEPNO = " + txtJepNo.Text + " " + ComNum.VBLF;
                    if (txtMukNo.Text != "") { SQL = SQL + "   AND A.MUKNO = " + txtMukNo.Text + " " + ComNum.VBLF; }
                    if (txtChasu.Text != "") { SQL = SQL + "   AND A.CHASU = " + txtChasu.Text + " " + ComNum.VBLF; }
                    SQL += "   AND A.JOHAP IN ('6') " + ComNum.VBLF;
                    SQL += "   AND A.WRTNO = C.WRTNO " + ComNum.VBLF;
                    SQL += "   AND A.JCODE = D.CODE " + ComNum.VBLF;
                    SQL += "   AND D.JONG ='E'" + ComNum.VBLF;
                    SQL += " GROUP BY C.PANO, A.TDATE, A.JCODE, D.NAME" + ComNum.VBLF;
                }

                clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);

                SQL = " SELECT PANO, TO_DATE(TDATE,'YYYY-MM-DD') TDATE, JCODE, NAME, SUM(AMT1) AMT1, SUM(AMT2) AMT2 " + ComNum.VBLF;
                SQL += " FROM VIEW_SAK " + ComNum.VBLF;
                SQL += " GROUP BY PANO, TDATE, JCODE, NAME " + ComNum.VBLF;

              
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                ss1_Sheet1.Rows.Count = 0;


                ss1.ActiveSheet.Rows.Count = dt.Rows.Count + 1;
                ss1.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1.ActiveSheet.Cells[i + 1, 0].Text = dt.Rows[i]["PANO"].ToString();
                    ss1.ActiveSheet.Cells[i + 1, 1].Text = dt.Rows[i]["TDATE"].ToString();
                    ss1.ActiveSheet.Cells[i + 1, 2].Text = "(" + dt.Rows[i]["JCODE"].ToString() + ")" + dt.Rows[i]["NAME"].ToString();
                    ss1.ActiveSheet.Cells[i + 1, 3].Text = string.Format("{0:###,###,###,##0}", dt.Rows[i]["AMT1"]);
                    nAMT1 = Convert.ToInt64(dt.Rows[i]["Amt1"]);
                    ss1.ActiveSheet.Cells[i + 1, 4].Text = string.Format("{0:###,###,###,##0}", dt.Rows[i]["AMT2"]);
                    nAMT2 = Convert.ToInt64(dt.Rows[i]["Amt2"]);
                    ss1.ActiveSheet.Cells[i + 1, 5].Text = string.Format("{0:###,###,###,##0}", nAMT1 - nAMT2);
                    nSum1 = nSum1 + nAMT1;
                    nSum2 = nSum2 + nAMT2;
                }

                ss1.ActiveSheet.Cells[0, 1].Text = "** 합계 **";
                ss1.ActiveSheet.Cells[0, 3].Text = string.Format("{0:##,###,###,##0}", nSum1);
                ss1.ActiveSheet.Cells[0, 4].Text = string.Format("{0:##,###,###,##0}", nSum2);
                ss1.ActiveSheet.Cells[0, 5].Text = string.Format("{0:##,###,###,##0}", nSum1 - nSum2);

                dt.Dispose();
                dt = null;

                //미수내역
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, AMT, REMARK " + ComNum.VBLF;
                SQL += "  FROM MISU_SLIP" + ComNum.VBLF;
                SQL += " WHERE MISUID = '" + txtJepNo.Text.PadLeft(8, '0') + "' " + ComNum.VBLF;
                SQL += "   AND GUBUN ='31' " + ComNum.VBLF; //삭감
                SQL += "   AND AMT <> 0 " + ComNum.VBLF;
                if (strJong == "1")
                {
                    SQL += " AND CLASS IN ('01','02','03','04')" + ComNum.VBLF;
                }
                else
                {
                    SQL += " AND CLASS IN ('05)" + ComNum.VBLF;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                long nSum3 = 0;

                ss2_Sheet1.Rows.Count = 0;
                ss2_Sheet1.Rows.Count = dt.Rows.Count + 1;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString();
                    ss2_Sheet1.Cells[i, 1].Text = string.Format("{0:###,###,###,##0}", dt.Rows[i]["AMT"]);
                    nSum3 = nSum3 + Convert.ToInt64(dt.Rows[i]["AMT"]);
                    ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["REMARK"].ToString();
                }
                dt.Dispose();
                dt = null;

                ss2_Sheet1.Cells[ss2_Sheet1.Rows.Count - 1, 0].Text = "** 합계 **";
                ss2_Sheet1.Cells[ss2_Sheet1.Rows.Count - 1, 1].Text = string.Format("{0:##,###,###,##0}", nSum3);

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void FrmMirTongDoctSakChkDtl_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SearchData();
        }
    }
}
