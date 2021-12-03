using System.Data;
using System.Windows.Forms;
using System;
using ComLibB;
using System.Drawing;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : VIP고객 명단관리
/// Author : 박병규
/// Create Date : 2017.06.14
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryVIP : Form
    {
        clsPmpaFunc PF = null;
        clsUser CU = null;
        ComQuery CQ = null;
        clsPmpaQuery CPQ = null;
        clsSpread SPR = null;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;


        String strRowid = "";
        string strPtno = "";
        string strSname = "";
        string strVipGubun = "";
        string strVipRemark = "";

        public frmPmpaEntryVIP()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            //Control 변경시 화면 Clear
            this.dtpFdate.ValueChanged += new EventHandler(eForm_Clear);
            this.dtpTdate.ValueChanged += new EventHandler(eForm_Clear);
            this.cboVIP.SelectedIndexChanged += new EventHandler(eForm_Clear);

            //KeyPress 이벤트
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboVIP.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtFind.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboVipGubun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataSearch();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            if (txtSabun.Text == "") { ComFunc.MsgBox("사원번호가 공란입니다.", "오류"); return; }
            if (cboVipGubun.Text == "" || cboVipGubun.Text == "**.전체") { ComFunc.MsgBox("VIP 선택오류입니다.", "오류"); return; }
            if (txtRemark.Text == "") { ComFunc.MsgBox("참고사항이 공란입니다.", "오류"); return; }
            if (strRowid == "") { ComFunc.MsgBox("VIP 고객을 선택후 작업하세요", "오류"); return; }

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "    SET GB_VIP_SABUN  = " + VB.Val(txtSabun.Text.Trim()) + ", ";
                SQL += ComNum.VBLF + "        GB_VIP_DATE   = SYSDATE , ";
                SQL += ComNum.VBLF + "        GB_VIP        = '" + VB.Left(cboVipGubun.Text.Trim(), 2) + "', ";
                SQL += ComNum.VBLF + "        GB_VIP_REMARK = '" + txtRemark.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "  WHERE ROWID         = '" + strRowid + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                DataSearch();
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

        private void DataSearch()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strGubun = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            strGubun = VB.Left(cboVIP.Text,2);

            Cursor.Current = Cursors.WaitCursor;
            ComFunc.SetAllControlClear(pnlLeft);

            SQL = "";
            if (chkEtc.Checked == true)
            {
                SQL += ComNum.VBLF + " SELECT B.PANO, B.SNAME, ";
                SQL += ComNum.VBLF + "        '' AS GB_VIP,                                                                 --VIP 고객구분";
                SQL += ComNum.VBLF + "        '' AS VIPNAME,                                                                --VIP 고객구분명";
                SQL += ComNum.VBLF + "        A.GAMMESSAGE AS GB_VIP_REMARK,                                                --VIP 고객 참고사항";
                SQL += ComNum.VBLF + "        B.SEX, B.JUMIN1, B.JUMIN2, B.JUMIN3, B.ROWID  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF A, ";
                SQL += ComNum.VBLF +              ComNum.DB_PMPA + "bas_patient B ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.ENTDATE <  TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.GAMOUT IS NULL";
                SQL += ComNum.VBLF + "    AND A.GAMCODE IN ('12','13','14') ";
                SQL += ComNum.VBLF + "    AND A.GAMJUMIN = B.JUMIN1||B.JUMIN2 ";
                SQL += ComNum.VBLF + "  ORDER BY B.PANO ";
            }
            else
            {
                SQL += ComNum.VBLF + " SELECT PANO, SNAME, ";
                SQL += ComNum.VBLF + "        GB_VIP,                                                                       --VIP 고객구분";
                SQL += ComNum.VBLF + "        ADMIN.FC_BAS_BCODE_NAMEREAD('BAS_VIP_구분코드', GB_VIP)  AS VIPNAME,     --VIP 고객구분명";
                SQL += ComNum.VBLF + "        GB_VIP_REMARK,                                                                --VIP 고객 참고사항";
                SQL += ComNum.VBLF + "        SEX, Jumin1, Jumin2, Jumin3, ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GB_VIP_DATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND GB_VIP_DATE <  TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";

                if (strGubun == "**")
                    SQL += ComNum.VBLF + "  AND TRIM(GB_VIP) IN ( SELECT TRIM(CODE) FROM ADMIN.BAS_BCODE WHERE GUBUN ='BAS_VIP_구분코드' ) ";
                else
                    SQL += ComNum.VBLF + "  AND GB_VIP  = '" + strGubun + "' ";

                if (txtFind.Text.Trim() != "")
                    SQL += ComNum.VBLF + "  AND (PANO LIKE '%" + txtFind.Text.Trim() + "%' OR SNAME LIKE '%" + txtFind.Text.Trim() + "%' OR GB_VIP_REMARK LIKE '%" + txtFind.Text.Trim() + "%' )  ";

                SQL += ComNum.VBLF + "  ORDER BY GB_VIP,SNAME   ";
            }
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
                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + Dt.Rows[i]["JUMIN2"].ToString().Trim(); ;
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["GB_VIP"].ToString().Trim() + "." + Dt.Rows[i]["VIPNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["GB_VIP_REMARK"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
            }
            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            PF = new clsPmpaFunc();
            CU = new clsUser();
            CQ = new ComQuery();
            CPQ = new clsPmpaQuery();
            SPR = new clsSpread();

            DataTable Dt = new DataTable();
            string strCode = "";
            string strCodeName = "";
            
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaEntryVIP frm = new frmPmpaEntryVIP();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlLeftTop);
            ComFunc.SetAllControlClear(pnlLeft);
            ComFunc.SetAllControlClear(pnlRight);

            //PF.Read_SysDate();

            dtpFdate.Text = clsPublic.GstrSysDate;
            dtpTdate.Text = clsPublic.GstrSysDate;

            //VIP 목록
            Dt = ComQuery.Set_BaseCode_Foundation(clsDB.DbCon, "BAS_VIP_구분코드", "");

            cboVIP.Items.Add("**.전체");
            cboVipGubun.Items.Add("**.전체");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strCode = Dt.Rows[i]["CODE"].ToString().Trim();
                strCodeName = Dt.Rows[i]["NAME"].ToString().Trim();

                cboVIP.Items.Add(strCode + "." + strCodeName);
                cboVipGubun.Items.Add(strCode + "." + strCodeName);
            }
            cboVIP.SelectedIndex = 0;
            cboVipGubun.SelectedIndex = 0;

            Dt.Dispose();
            Dt = null;

            txtSabun.Text = clsType.User.Sabun;
            txtSabunName.Text = clsType.User.JobName;

        }

        private void eForm_Clear(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlLeft);
            txtInfo.Text = "";
            txtSabun.Text = "";
            txtSabunName.Text = "";
            txtRemark.Text = "";
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { cboVIP.Focus(); }
            if (sender == this.cboVIP && e.KeyChar == (Char)13) { txtFind.Focus(); }
            if (sender == this.txtFind && e.KeyChar == (Char)13) { btnSearch.Focus(); }
            if (sender == this.cboVipGubun && e.KeyChar == (Char)13) { txtRemark.Focus(); }
        }

        private void chkEtc_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtc.Checked == true)
            {
                cboVIP.Enabled = false;
                txtFind.Enabled = false;
            }
            else
            {
                cboVIP.Enabled = true;
                txtFind.Enabled = true;
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            strPtno = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            strSname = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
            strVipGubun = VB.Left( ssList_Sheet1.Cells[e.Row, 4].Text.Trim(),2);
            strVipRemark = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();
            strRowid = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();

            txtInfo.Text = "등록번호 : " + strPtno + "   성명 : " + strSname;

            for (int i = 0; i < cboVipGubun.Items.Count; i++)
            {
                cboVipGubun.SelectedIndex = i;
                if (VB.Left( cboVipGubun.Text.Trim(),2) == strVipGubun)
                    break;
                else
                    cboVipGubun.SelectedIndex = 0;
            }

            txtRemark.Text = strVipRemark;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) { return; }     //권한확인

            if (strRowid == "") { ComFunc.MsgBox("VIP 대상자를 선택후 작업하세요", "오류"); return; }

            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "    SET GB_VIP        = '', ";
                SQL += ComNum.VBLF + "        GB_VIP_REMARK = ''  ";
                SQL += ComNum.VBLF + "  WHERE ROWID         = '" + strRowid + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //후불대상자자동삭제
               CQ.AUTO_MASTER_DELETE(clsDB.DbCon, strPtno, "VIP 자동삭제");

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                //VIP 고객명단 조회
                DataSearch();
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


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strDate = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strDate = dtpFdate.Text + " ~ " + dtpTdate.Text;

                strTitle = "VIP 명부(" + cboVIP.Text + ")";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String("등록기간 : " + strDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }

        // 직원, 가족 자료 삭제후 재형성
        // 수녀님 자료는 갱신 및 처리안함
        private void btnBuild_Click(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;


            Cursor.Current = Cursors.WaitCursor;
             
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //기존 자료정리
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "    SET GB_VIP2        = '', ";
                SQL += ComNum.VBLF + "        GB_VIP2_REMARK = ''  ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GB_VIP2        = '02' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.GAMJUMIN, A.GAMJUMIN3, A.GAMNAME, A.GAMMESSAGE, ";
                SQL += ComNum.VBLF + "        C.SABUN, A.GAMCODE, A.GAMNAME AS SNAME, C.BUSE, ";
                SQL += ComNum.VBLF + "        ADMIN.FC_BAS_BUSE_NAME(C.BUSE) BUSENAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "INSA_MST C ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.GAMOUT IS NULL ";
                SQL += ComNum.VBLF + "    AND A.GAMCODE IN ('21')";
                SQL += ComNum.VBLF + "    AND C.SABUN NOT IN ( SELECT SABUN ";
                SQL += ComNum.VBLF + "                           FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL += ComNum.VBLF + "                          WHERE TOIDAY IS NOT NULL )";
                SQL += ComNum.VBLF + "    AND A.GAMJUMIN = C.JUMIN";  //인사등록된 자료만
                SQL += ComNum.VBLF + "  UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.GAMJUMIN, A.GAMJUMIN3, A.GAMNAME, A.GAMMESSAGE,";
                SQL += ComNum.VBLF + "        C.SABUN, A.GAMCODE, A.GAMNAME AS SNAME, D.BUSE, ";
                SQL += ComNum.VBLF + "        ADMIN.FC_BAS_BUSE_NAME(D.BUSE) BUSENAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "INSA_MSTB C, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "INSA_MST D ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.GAMOUT IS NULL ";
                SQL += ComNum.VBLF + "    AND A.GAMCODE IN ('23','24') ";
                SQL += ComNum.VBLF + "    AND C.SABUN NOT IN ( SELECT SABUN ";
                SQL += ComNum.VBLF + "                           FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL += ComNum.VBLF + "                          WHERE TOIDAY IS NOT NULL )";
                SQL += ComNum.VBLF + "    AND C.SABUN = D.SABUN ";
                SQL += ComNum.VBLF + "    AND A.GAMJUMIN = C.JUMIN ";  //인사등록된 자료만
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (Dt.Rows.Count > 0)
                {
                    string strJumin = string.Empty;
                    string strJumin1 = string.Empty;
                    string strJumin2 = string.Empty;
                    string strPtno = string.Empty;
                    string strConvGamcode = string.Empty;
                    string strMsg = string.Empty;
                    string strBuseName = string.Empty;

                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        strJumin = VB.Mid(clsAES.DeAES(Dt.Rows[i]["GAMJUMIN3"].ToString()),7,1);
                        strJumin1 = VB.Left(clsAES.DeAES(Dt.Rows[i]["GAMJUMIN3"].ToString()), 6);
                        strJumin2 = clsAES.AES(VB.Mid(clsAES.DeAES(Dt.Rows[i]["GAMJUMIN3"].ToString()), 7, 7));

                        strPtno = CQ.GetPtno(clsDB.DbCon, strJumin1, strJumin2);
                        
                        switch (Dt.Rows[i]["GAMCODE"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                                strConvGamcode = "04";          //수녀
                                if (strJumin == "1") { strConvGamcode = "03"; }
                                break;
                            case "21":
                                strConvGamcode = "01";          //직원
                                break;
                            default:
                                strConvGamcode = "02";          //직원존속
                                break;
                        }

                        strMsg = Dt.Rows[i]["GAMMESSAGE"].ToString().Trim();
                        strBuseName = Dt.Rows[i]["BUSENAME"].ToString().Trim();

                        //NEW UPDATE
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                        SQL += ComNum.VBLF + "    SET GB_VIP2        = '" + strConvGamcode + "', ";

                        if (strConvGamcode == "03" || strConvGamcode == "04")
                            SQL += ComNum.VBLF + "        GB_VIP2_REMARK = '" + strMsg + "'  ";
                        else
                            SQL += ComNum.VBLF + "        GB_VIP2_REMARK = '" + strMsg + "[" + strBuseName + "]'  ";

                        SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO = '" + strPtno + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                Dt.Dispose();
                Dt = null;
                                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

                DataSearch();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtInfo.Text = "";
            cboVipGubun.SelectedIndex = 0;
            txtRemark.Text = "";
        }
    }
}
