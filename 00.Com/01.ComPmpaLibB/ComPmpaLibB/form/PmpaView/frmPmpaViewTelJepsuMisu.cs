using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 전화접수 부도자 자료형성
/// Author : 박병규
/// Create Date : 2017.07.18
/// </summary>
/// <history>
/// </history>
/// <seealso cref="frmTelFailedBuild.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaViewTelJepsuMisu : Form
    {
        clsSpread CS = new clsSpread();
        clsUser CU = new clsUser();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;


        public frmPmpaViewTelJepsuMisu()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            dtpDate.Text =clsPublic.GstrSysDate;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strPtno = "";
            string strDept = "";
            string strTemp = "";
            
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, DEPTCODE, DRCODE, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        CHOJAE, BI, SNAME, ";
            SQL += ComNum.VBLF + "        SEX, AGE, AMT1, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND JIN       = 'E' ";
            SQL += ComNum.VBLF + "  ORDER BY DEPTCODE, PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("전화접수 부도자가 존재하지 않음.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strPtno = Dt.Rows[i]["PANO"].ToString().Trim();
                strDept = Dt.Rows[i]["deptcode"].ToString().Trim();

                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.SUCODE, B.SUNAMEK, A.ORDERCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B ";
                SQL += ComNum.VBLF + "  WHERE 1                 = 1 ";
                SQL += ComNum.VBLF + "    AND A.PTNO            = '" + strPtno + "'";
                SQL += ComNum.VBLF + "    AND A.DEPTCODE        = '" + strDept + "'";
                SQL += ComNum.VBLF + "    AND SUBSTR(A.ORDERCODE, 1, 2) = '$$' ";
                SQL += ComNum.VBLF + "    AND A.bdate           = TO_DATE('" + dtpDate.Text + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.SUCODE          = B.SUNEXT(+) ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.Cells[i, 0].Text = "$$오더없음";

                if (DtSub.Rows.Count > 0)
                {
                    strTemp = "";
                    for (int j = 0; j < DtSub.Rows.Count; j++)
                        strTemp += DtSub.Rows[j]["SUCODE"].ToString().Trim() + ",";

                    ssList_Sheet1.Cells[i, 0].Text = strTemp;
                }

                DtSub.Dispose();
                DtSub = null;

                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["DRCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DRNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["BI"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["AGE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = String.Format("{0:#,##0}", Dt.Rows[i]["AMT1"].ToString().Trim());
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "전화접수 미수현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("작업일자 : " + dtpDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
    }
}
