using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpSelect : Form
    {
        private clsOrderEtc OE = null;
        private clsOrderEtc.OCS_CP_RECORD OCR = new clsOrderEtc.OCS_CP_RECORD();
        string mPtNo = "";
        string mER_PATIENT_InDate = "";
        string mER_PATIENT_InTime = "";

        public delegate void SetCpInto(double pCPNO, bool pCP_SELECT, bool pCP_NEW);
        public event SetCpInto rSetCpInto;

        public frmOcsCpSelect()
        {
            InitializeComponent();
        }

        public frmOcsCpSelect(string pPtNo, string pER_PATIENT_InDate, string pER_PATIENT_InTime)
        {
            InitializeComponent();

            mPtNo = pPtNo;
            mER_PATIENT_InDate = pER_PATIENT_InDate;
            mER_PATIENT_InTime = pER_PATIENT_InTime;
        }

        private void frmOcsCpSelect_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            OE = new clsOrderEtc();
            OE.Clear_OCS_CP_RECORD(ref OCR);

            OCR.PtNo = clsOrdFunction.Pat.PtNo;
            OCR.OPD_ROWID = clsOrdFunction.Pat.Mst_ROWID;

            OE.Read_ERPat_Info(ref OCR);

            Screen_display();
        }

        private void Screen_display()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

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
                SQL = SQL + ComNum.VBLF + "     a.CPNO, a.Ptno, a.GbIO, a.DeptCode, a.Bi, ";
                SQL = SQL + ComNum.VBLF + "     a.CpCode, a.ROWID, b.SName, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.BDATE,'YYYY-MM-DD') AS BDATE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(a.InTIME,'YYYY-MM-DD HH24:MI') AS InTIME, ";
                SQL = SQL + ComNum.VBLF + "     a.WarmDate, a.WarmTime, a.WarmSabun, ";           //예비CP
                SQL = SQL + ComNum.VBLF + "     a.StartDate, a.StartTime, a.StartSabun, ";        //CP등록
                SQL = SQL + ComNum.VBLF + "     a.ActDate, a.ActTime, a.ActSabun, ";              //시술
                SQL = SQL + ComNum.VBLF + "     a.DropDate, a.DropTime, a.DropSabun, ";           //CP제외
                SQL = SQL + ComNum.VBLF + "     a.CancerDate, a.CancerTime, a.CancerSabun, ";     //CP중단
                SQL = SQL + ComNum.VBLF + "     a.CallDate, a.CallTime, a.CallSabun ";           //의사콜
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_CP_RECORD a, ";
                SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT b ";
                SQL = SQL + ComNum.VBLF + "     WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "         AND a.Ptno = b.Pano(+)";
                if (mPtNo != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND a.Ptno = '" + mPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND a.InTime = TO_DATE('" + mER_PATIENT_InDate + " " + mER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI')  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "         AND a.Ptno = '" + OCR.PtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND a.InTime = TO_DATE('" + OCR.ER_PATIENT_InDate + " " + OCR.ER_PATIENT_InTime + "','YYYY-MM-DD HH24:MI')  ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY a.CpNo ";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count + 1;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["InTIME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CPCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = OE.READ_CP_NAME(dt.Rows[i]["CPCODE"].ToString().Trim(), "ER");
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["CPNO"].ToString().Trim();
                    }

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = "신규등록";
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

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) { return; }
            
            OCR.CPNO = VB.Val(ssView_Sheet1.Cells[e.Row, 5].Text.Trim());

            if (OCR.CPNO > 0)
            {
                OCR.CP_SELECT = true;
            }
            
            if (ssView_Sheet1.Cells[e.Row, 0].Text.Trim() == "신규등록")
            {
                OCR.CP_SELECT = true;
                OCR.CP_NEW = true;
                OCR.CPNO = 0;
            }

            rSetCpInto(OCR.CPNO, OCR.CP_SELECT, OCR.CP_NEW);
        }
    }
}
