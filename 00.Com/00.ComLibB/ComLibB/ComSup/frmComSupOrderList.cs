using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupOrderList.cs
    /// Description     : 처방 목록
    /// Author          : 이정현
    /// Create Date     : 2018-01-15
    /// <history> 
    /// 처방 목록
    /// </history>
    /// <seealso>
    /// PSMH\drug\dradr\frmOrderList.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\dradr\dradr.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupOrderList : Form
    {
        private string GstrPANO = "";

        public delegate void SendDataHandler(string[] strSUCODE, string[] strSUNAMEK, string[] strDRBUN, 
                                string[] strEFFECT, string[] strQTY, string[] strGBDIV, string[] strDOSNAME);
        public event SendDataHandler SendEvent;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmComSupOrderList(string strPANO)
        {
            InitializeComponent();

            GstrPANO = strPANO;
        }

        private void frmComSupOrderList_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            
            GetData();
        }

        private void GetData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_DATE(BDATE, 'YYYY-MM-DD') AS BDATE, GUBUN, SUCODE, SUNAMEK, EFFECT, DRBUN, QTY, GBDIV, DOSNAME";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         A.BDATE, '입원' AS GUBUN, A.SUCODE, B.SUNAMEK, C.EFFECT, DECODE(C.DRBUN, '01', '전문약', '02','일반약','03','희귀약','') AS DRBUN,";
                SQL = SQL + ComNum.VBLF + "         A.QTY , A.GBDIV, D.DOSNAME, Sum(A.QTY * A.NAL)";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_PMPA +"BAS_SUN B, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW C, " + ComNum.DB_MED + "OCS_ODOSAGE D";
                SQL = SQL + ComNum.VBLF + "         Where A.BDATE >= TRUNC(SYSDATE - 3)";
                SQL = SQL + ComNum.VBLF + "             AND A.BDATE <= TRUNC(SYSDATE)";
                SQL = SQL + ComNum.VBLF + "             AND A.PTNO = '" + GstrPANO + "'";
                SQL = SQL + ComNum.VBLF + "             AND A.SUCODE = B.SUNEXT(+)";
                SQL = SQL + ComNum.VBLF + "             AND A.SUCODE = C.SUNEXT(+)";
                SQL = SQL + ComNum.VBLF + "             AND A.DOSCODE = D.DOSCODE(+)";
                SQL = SQL + ComNum.VBLF + "             AND A.BUN IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "     GROUP BY A.BDATE, '입원', A.SUCODE, B.SUNAMEK, C.EFFECT, DECODE(C.DRBUN, '01', '전문약', '02','일반약','03','희귀약',''), A.QTY, A.GBDIV, D.DOSNAME";
                SQL = SQL + ComNum.VBLF + "     Having Sum(A.QTY * A.NAL) > 0";
                SQL = SQL + ComNum.VBLF + "     Union All";
                SQL = SQL + ComNum.VBLF + "     SELECT";
                SQL = SQL + ComNum.VBLF + "         A.BDATE, '외래' AS GUBUN, A.SUCODE, B.SUNAMEK, C.EFFECT, DECODE(C.DRBUN, '01', '전문약', '02','일반약','03','희귀약','') AS DRBUN,";
                SQL = SQL + ComNum.VBLF + "         A.QTY, A.GBDIV, D.DOSNAME, Sum(A.QTY * A.NAL)";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_PMPA + "BAS_SUN B, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW C, " + ComNum.DB_MED + "OCS_ODOSAGE D";
                SQL = SQL + ComNum.VBLF + "         WHERE A.PTNO = '" + GstrPANO + "'";
                SQL = SQL + ComNum.VBLF + "             AND A.BDATE IN";
                SQL = SQL + ComNum.VBLF + "                     (SELECT BDATE FROM ";
                SQL = SQL + ComNum.VBLF + "                         (SELECT BDATE FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL = SQL + ComNum.VBLF + "                             WHERE PANO = '" + GstrPANO + "'";
                SQL = SQL + ComNum.VBLF + "                         ORDER BY BDATE DESC)";
                SQL = SQL + ComNum.VBLF + "                     WHERE ROWNUM <= 3)";
                SQL = SQL + ComNum.VBLF + "             AND A.SUCODE = B.SUNEXT(+)";
                SQL = SQL + ComNum.VBLF + "             AND A.SUCODE = C.SUNEXT(+)";
                SQL = SQL + ComNum.VBLF + "             AND A.DOSCODE = D.DOSCODE(+)";
                SQL = SQL + ComNum.VBLF + "             AND A.BUN IN ('11','12','20')";
                SQL = SQL + ComNum.VBLF + "     GROUP BY A.BDATE, '외래', A.SUCODE, B.SUNAMEK, C.EFFECT, DECODE(C.DRBUN, '01', '전문약', '02','일반약','03','희귀약',''), A.QTY, A.GBDIV, D.DOSNAME";
                SQL = SQL + ComNum.VBLF + "     HAVING SUM(A.QTY*A.NAL) > 0)";
                SQL = SQL + ComNum.VBLF + "GROUP BY BDATE, GUBUN, SUCODE, SUNAMEK, EFFECT, DRBUN, QTY, GBDIV, DOSNAME";
                SQL = SQL + ComNum.VBLF + "ORDER BY GUBUN, BDATE, SUCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EFFECT"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DRBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            string[] strSUCODE = new string[0];
            string[] strSUNAMEK = new string[0];
            string[] strDRBUN = new string[0];
            string[] strEFFECT = new string[0];
            string[] strQTY = new string[0];
            string[] strGBDIV = new string[0];
            string[] strDOSNAME = new string[0];
            int i = 0;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                {
                    Array.Resize<string>(ref strSUCODE, strSUCODE.Length + 1);
                    Array.Resize<string>(ref strSUNAMEK, strSUNAMEK.Length + 1);
                    Array.Resize<string>(ref strDRBUN, strDRBUN.Length + 1);
                    Array.Resize<string>(ref strEFFECT, strEFFECT.Length + 1);
                    Array.Resize<string>(ref strQTY, strQTY.Length + 1);
                    Array.Resize<string>(ref strGBDIV, strGBDIV.Length + 1);
                    Array.Resize<string>(ref strDOSNAME, strDOSNAME.Length + 1);

                    strSUCODE[strSUCODE.Length - 1] = ssView_Sheet1.Cells[i, 3].Text.Trim();
                    strSUNAMEK[strSUNAMEK.Length - 1] = ssView_Sheet1.Cells[i, 4].Text.Trim();
                    strDRBUN[strDRBUN.Length - 1] = ssView_Sheet1.Cells[i, 5].Text.Trim();
                    strEFFECT[strEFFECT.Length - 1] = ssView_Sheet1.Cells[i, 6].Text.Trim();
                    strQTY[strQTY.Length - 1] = ssView_Sheet1.Cells[i, 7].Text.Trim();
                    strGBDIV[strGBDIV.Length - 1] = ssView_Sheet1.Cells[i, 8].Text.Trim();
                    strDOSNAME[strDOSNAME.Length - 1] = ssView_Sheet1.Cells[i, 9].Text.Trim();
                }
            }

            SendEvent(strSUCODE, strSUNAMEK, strDRBUN, strEFFECT, strQTY, strGBDIV, strDOSNAME);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClosed == null)
            {
                this.Close();
            }
            else
            {
                rEventClosed();
            }
        }
    }
}
