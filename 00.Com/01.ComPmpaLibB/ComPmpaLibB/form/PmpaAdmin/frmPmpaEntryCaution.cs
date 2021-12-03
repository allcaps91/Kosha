using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 감사고객 명단관리
/// Author : 박병규
/// Create Date : 2017.06.15
/// </summary>
/// <history>
/// 2017.06.15 : 김민철주임. 바코드출력 기능 사용안함
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryCaution : Form
    {
        clsUser CU = null;
        ComQuery CQ = null;
        clsPmpaQuery CPQ = null;
        clsSpread SPR = null;
        clsOrdFunction OF = null;

        string strPrintDefaultName = string.Empty;

        public frmPmpaEntryCaution()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            //Control 변경시 화면 Clear
            this.rdoGubun0.Click += new EventHandler(eForm_Clear);
            this.rdoGubun1.Click += new EventHandler(eForm_Clear);
            this.rdoGubun2.Click += new EventHandler(eForm_Clear);
        }

        private void eForm_Clear(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CU = new clsUser();
            CQ = new ComQuery();
            CPQ = new clsPmpaQuery();
            SPR = new clsSpread();
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
        }

        private void Get_DataLoad()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlBody);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.YEAR, A.PANO, A.SNAME, A.JUMSU, A.UJUMSU, B.BI, A.ROWID, ";
            SQL += ComNum.VBLF + "        B.GBSMS, A.ENTSABUN, ADMIN.FC_BAS_USER_NAME(A.ENTSABUN) AS USERNAME, ";
            SQL += ComNum.VBLF + "        A.GUBUN, B.JUMIN1, B.JUMIN2, B.JUMIN3, B.TEL, B.HPHONE, B.HPHONE2, ";
            SQL += ComNum.VBLF + "        B.BUILDNO, B.ZIPCODE1 || B.ZIPCODE2  AS ZIPCODE, B.JUSO, ";
            SQL += ComNum.VBLF + "        TO_CHAR(B.LASTDATE,'YYYY-MM-DD') AS LASTDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.ENTDATE2, 'YYYY-MM-DD') AS ENTDATE2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SVIP_MST A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE ='') ";

            if (rdoGubun1.Checked == true)
                SQL += ComNum.VBLF + "AND GUBUN = '00'     ";
            else if (rdoGubun2.Checked == true)
                SQL += ComNum.VBLF + "AND Gubun = '01'     ";

            SQL += ComNum.VBLF + "    AND A.PANO = B.PANO(+) ";
            SQL += ComNum.VBLF + "  ORDER BY A.YEAR, A.PANO ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = "";
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["YEAR"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["BI"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(Dt.Rows[i]["JUMIN3"].ToString().Trim()), 1) + "******";
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["TEL"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["HPHONE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["HPHONE2"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["LASTDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["JUMSU"].ToString().Trim();
                ssList_Sheet1.Cells[i, 11].Text = Dt.Rows[i]["UJUMSU"].ToString().Trim();

                if (Dt.Rows[i]["BUILDNO"].ToString().Trim() != "")
                    ssList_Sheet1.Cells[i, 12].Text = CQ.Read_RoadJuso(clsDB.DbCon, Dt.Rows[i]["BUILDNO"].ToString().Trim()) + " " + Dt.Rows[i]["JUSO"].ToString().Trim();
                else
                    ssList_Sheet1.Cells[i, 12].Text = CQ.Read_Juso(clsDB.DbCon, Dt.Rows[i]["ZIPCODE"].ToString().Trim()) + " " + Dt.Rows[i]["JUSO"].ToString().Trim();

                if (Dt.Rows[i]["GBSMS"].ToString().Trim() == "Y")
                    ssList_Sheet1.Cells[i, 13].Text = "SMS동의";
                else if (Dt.Rows[i]["GBSMS"].ToString().Trim() == "X")
                    ssList_Sheet1.Cells[i, 13].Text = "동의거부";

                if (Dt.Rows[i]["GUBUN"].ToString().Trim() == "00")
                    ssList_Sheet1.Cells[i, 14].Text = "기본";
                else if (Dt.Rows[i]["GBSMS"].ToString().Trim() == "01")
                    ssList_Sheet1.Cells[i, 14].Text = "추천";

                ssList_Sheet1.Cells[i, 15].Text = Dt.Rows[i]["USERNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 16].Text = Dt.Rows[i]["ENTDATE2"].ToString().Trim();
                ssList_Sheet1.Cells[i, 20].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strDel = string.Empty;
            string strPtno = string.Empty;
            string strRowid = string.Empty;

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) { return; }     //권한확인

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strDel = ssList_Sheet1.Cells[i, 0].Text.Trim();
                    strPtno = ssList_Sheet1.Cells[i, 2].Text.Trim();
                    strRowid = ssList_Sheet1.Cells[i, 20].Text.Trim();

                    if (strDel != "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_SVIP_MST";
                        SQL += ComNum.VBLF + "    SET DELDATE = SYSDATE,";
                        SQL += ComNum.VBLF + "        ENTDATE = SYSDATE,";
                        SQL += ComNum.VBLF + "        ENTSABUN = '" + clsType.User.Sabun + "'";
                        SQL += ComNum.VBLF + "  WHERE ROWID = '" + strRowid + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //후불대상자자동삭제
               CQ.AUTO_MASTER_DELETE(clsDB.DbCon, strPtno, "특별고객 임의제외");

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                Get_DataLoad();
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인
            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "특별고객 명단리스트";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            frmPmpaMasterCaution frm = new frmPmpaMasterCaution();
            frm.ShowDialog();
            OF.fn_ClearMemory(frm);

        }
    }
}
