using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

/// <summary>
/// Description : 접수현황 통합조회
/// Author : 박병규
/// Create Date : 2017.08.07
/// </summary>
/// <history>
/// </history>
/// <seealso cref="OVCHRT02_NEW.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaViewResJepsuTel : Form
    {
        public frmPmpaViewResJepsuTel()
        {
            InitializeComponent();
            setParam();
        }


        private void setParam()
        {
            this.Load += new EventHandler(eForm_Load);

            //LostFocus Event
            this.txtPtno.LostFocus += new EventHandler(eCtl_LostFocus);

            //KeyPress Event
            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            //ValueChanged Event
            this.dtpFdate.ValueChanged += new EventHandler(eForm_Clear);
            this.dtpTdate.ValueChanged += new EventHandler(eForm_Clear);
        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
            {
                txtPtno.Text = ComFunc.SetAutoZero(txtPtno.Text, 8);
                txtSname.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPtno.Text);
            }
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { dtpFdate.Focus(); }
            if (sender == this.dtpFdate && e.KeyChar == (Char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (Char)13) { btnSearch.Focus(); }
        }

        private void eForm_Clear(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad();
            DeleteView();
        }

        private void Get_DataLoad()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            ComFunc.SetAllControlClear(pnlBody);
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " CREATE OR REPLACE VIEW " + ComNum.DB_PMPA + "VIEW_TELJEPSU  ";
                SQL += ComNum.VBLF + "        (JepGbn, Pano, SName, ";
                SQL += ComNum.VBLF + "         DeptCode, DrCode, RDate, ";
                SQL += ComNum.VBLF + "         Rtime, Sabun, EntDate, ";
                SQL += ComNum.VBLF + "         DelSabun, DelDate ) AS ";
                SQL += ComNum.VBLF + " SELECT DECODE(ENTSABUN, '999999', '모바일예약','전화예약') AS JepGbn, Pano, SName, ";
                SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "        Rtime, EntSabun, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "        '', '' ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND GBFLAG = 'N' ";
                SQL += ComNum.VBLF + "    AND GUBUN IS NULL ";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT '포스코' AS JepGbn, Pano, SName, ";
                SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "        Rtime, EntSabun, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "        '', '' ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND GUBUN = '02' ";
                SQL += ComNum.VBLF + " UNION ALL  ";
                //SQL += ComNum.VBLF + " SELECT '과전용' AS JepGbn, Pano, SName, ";
                //SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ";
                //SQL += ComNum.VBLF + "        Rtime, EntSabun, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                //SQL += ComNum.VBLF + "        '', '' ";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
                //SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                //SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "    AND GBFLAG = 'Y' ";                                
                SQL += ComNum.VBLF + " SELECT ";
                SQL += ComNum.VBLF + "     DECODE(GBTODAY, '1', '당일접수', '과전용') AS JEPGBN, ";
                SQL += ComNum.VBLF + "     PANO, SNAME, DEPTCODE, DRCODE, RDATE, RTIME, ENTSABUN, ENTDATE, A, B ";
                SQL += ComNum.VBLF + " FROM ";
                SQL += ComNum.VBLF + " (SELECT Pano, SName, ";
                SQL += ComNum.VBLF + "         DeptCode, DrCode, TO_CHAR(RDATE, 'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "         Rtime, EntSabun, TO_CHAR(EntDate, 'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "         '' A, '' B, ";
                SQL += ComNum.VBLF + "         (SELECT '1' FROM ADMIN.BAS_USER B ";
                SQL += ComNum.VBLF + "                    WHERE A.ENTSABUN = B.SABUN ";
                SQL += ComNum.VBLF + "                      AND B.JOBGROUP IN('JOB015001', 'JOB015002', 'JOB015003')) GBTODAY ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_TELRESV A ";
                SQL += ComNum.VBLF + " WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "   AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND GBFLAG = 'Y')  ";                                
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT '임시대리' AS JepGbn, Pano, SName, ";
                SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "        Rtime, EntSabun, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "        '', '' ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "WORK_TEMPJEPSU ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT '333(당일)' AS JepGbn, Pano, SName, ";
                SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(BDATE,'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "        TO_CHAR(JTIME,'HH24:MI') Rtime, PART, TO_CHAR(JTIME,'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "        '', '' ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND PART = '333' ";
                SQL += ComNum.VBLF + "    AND JIN = '5' ";
                SQL += ComNum.VBLF + " UNION ALL  ";
                //SQL += ComNum.VBLF + " SELECT '전화삭제' AS JepGbn, Pano, SName, ";
                //SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ";
                //SQL += ComNum.VBLF + "        Rtime, EntSabun, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                //SQL += ComNum.VBLF + "        DELSABUN, TO_CHAR(DelDate,'YYYY-MM-DD HH24:MI') DELDate ";
                //SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV_DEL ";
                //SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                //SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";                
                SQL += ComNum.VBLF + " SELECT ";
                SQL += ComNum.VBLF + "    CASE WHEN DELSABUN = '999999' THEN '모바일삭제' ";
                SQL += ComNum.VBLF + "         WHEN GBFLAG = 'Y' THEN ";
                SQL += ComNum.VBLF + "             DECODE(GBTODAY, '1', '당일삭제', '전화삭제') ";
                SQL += ComNum.VBLF + "    ELSE ";
                SQL += ComNum.VBLF + "            '전화삭제' ";
                SQL += ComNum.VBLF + "    END AS JEPGBN, ";
                SQL += ComNum.VBLF + "    PANO, SNAME, DEPTCODE, DRCODE, RDATE, RTIME, ENTSABUN, ENTDATE, DELSABUN, DELDATE ";
                SQL += ComNum.VBLF + " FROM ";
                SQL += ComNum.VBLF + "     (SELECT Pano, SName, GBFLAG, ";
                SQL += ComNum.VBLF + "             DeptCode, DrCode, TO_CHAR(RDATE, 'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "             Rtime, EntSabun, TO_CHAR(EntDate, 'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "             DELSABUN, TO_CHAR(DelDate, 'YYYY-MM-DD HH24:MI') DELDate, ";
                SQL += ComNum.VBLF + "             (SELECT '1' FROM ADMIN.BAS_USER B ";
                SQL += ComNum.VBLF + "                        WHERE A.ENTSABUN = B.SABUN ";
                SQL += ComNum.VBLF + "                          AND B.JOBGROUP IN('JOB015001', 'JOB015002', 'JOB015003')) GBTODAY ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV_DEL A ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')) ";                
                SQL += ComNum.VBLF + " UNION ALL  ";
                SQL += ComNum.VBLF + " SELECT '대리삭제' AS JepGbn, Pano, SName, ";
                SQL += ComNum.VBLF + "        DeptCode, DrCode, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ";
                SQL += ComNum.VBLF + "        Rtime, EntSabun, TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL += ComNum.VBLF + "        DELSABUN, TO_CHAR(DelDate,'YYYY-MM-DD HH24:MI') DELDate ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "WORK_TEMPJEPSU_DELETE ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND RDATE >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND RDATE <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT DECODE(RETDATE, NULL, 'OCS예약', 'OCS삭제') AS JEPGBN, PANO, SNAME, ";
                SQL += ComNum.VBLF + "        DEPTCODE, DRCODE, TO_CHAR(DATE3, 'YYYY-MM-DD') RDATE,  ";
                SQL += ComNum.VBLF + "        TO_CHAR(DATE3, 'HH24:MI') RTIME, PART ENTSABUN, TO_CHAR(DATE1, 'YYYY-MM-DD') ENTDATE, ";
                SQL += ComNum.VBLF + "        RETPART, TO_CHAR(RETDATE, 'YYYY-MM-DD') DELDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
                SQL += ComNum.VBLF + "  WHERE DATE3 >= TO_DATE('" + dtpFdate.Text + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "    AND DATE3 <= TO_DATE('" + dtpTdate.Text + " 23:59', 'YYYY-MM-DD HH24:MI') ";

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
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            SQL = "";
            //SQL += ComNum.VBLF + " SELECT JEPGBN, PANO, SNAME, ";
            //SQL += ComNum.VBLF + "        DEPTCODE, ADMIN.FC_BAS_CLINICDEPT_DEPTNAMEK(DEPTCODE) DEPTNAMEK, ";
            //SQL += ComNum.VBLF + "        DRCODE, ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            //SQL += ComNum.VBLF + "        RDATE, RTIME, ";
            //SQL += ComNum.VBLF + "        SABUN, ADMIN.FC_BAS_USER_USERNAME(SABUN) USERNAME, ";
            //SQL += ComNum.VBLF + "        ENTDATE, DELDATE, ";
            //SQL += ComNum.VBLF + "        DELSABUN, ADMIN.FC_BAS_USER_USERNAME(DELSABUN) DELNAME ";
            SQL += ComNum.VBLF + "SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VIEW_TELJEPSU ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND RDATE >= '" + dtpFdate.Text + "'";
            SQL += ComNum.VBLF + "    AND RDATE <= '" + dtpTdate.Text + "'";

            if (txtPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND Pano  = '" + txtPtno.Text.Trim() + "' ";

            if (optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY Sname                                                          ";
            }
            else if (optSort1.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY DeptCode,Pano                                                ";
            }
            else if (optSort2.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY Pano                                                           ";
            }
            else if (optSort3.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY SABUN                                                            ";
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
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            clsPmpaFunc CPF = new clsPmpaFunc();
            string strPtno = string.Empty;

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["JepGbn"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["Pano"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Sname"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, Dt.Rows[i]["DRCODE"].ToString().Trim());
                ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["RDATE"].ToString().Trim() + " " + Dt.Rows[i]["RTIME"].ToString().Trim();
                //ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["Sabun"].ToString().Trim().Length == 0 ? " " : clsVbfunc.GetInSaName(clsDB.DbCon, Dt.Rows[i]["Sabun"].ToString().Trim());
                if (Dt.Rows[i]["Sabun"].ToString().Trim() == "999999")
                {
                    ssList_Sheet1.Cells[i, 6].Text = "모바일예약";
                }
                else if (VB.Left(Dt.Rows[i]["Sabun"].ToString().Trim(), 3) == "500")
                {
                    ssList_Sheet1.Cells[i, 6].Text = "무인수납";
                }
                else
                {
                    ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["Sabun"].ToString().Trim().Length == 0 ? " " : clsVbfunc.GetInSaName(clsDB.DbCon, Dt.Rows[i]["Sabun"].ToString().Trim());
                }
                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["ENTDATE"].ToString().Trim();

                if (Dt.Rows[i]["DELSABUN"].ToString().Trim() == "999999")
                {
                    ssList_Sheet1.Cells[i, 8].Text = "모바일삭제";
                }
                else if (VB.Left(Dt.Rows[i]["DELSABUN"].ToString().Trim(), 3) == "555")
                {
                    ssList_Sheet1.Cells[i, 8].Text = "예약자부도";
                }
                else
                {
                    ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["DELSABUN"].ToString().Trim().Length == 0 ? " " : clsVbfunc.GetInSaName(clsDB.DbCon, Dt.Rows[i]["DELSABUN"].ToString().Trim());
                }
                ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["DELDATE"].ToString().Trim();

                if (Dt.Rows[i]["JepGbn"].ToString().Trim() == "전화예약"  || Dt.Rows[i]["JepGbn"].ToString().Trim() == "모바일예약")
                    ssList_Sheet1.Cells[i, 10].Text = CPF.READ_TELRESV_NOSHOW(clsDB.DbCon, Dt.Rows[i]["Pano"].ToString().Trim(), Dt.Rows[i]["RDATE"].ToString().Trim(), Dt.Rows[i]["DeptCode"].ToString().Trim());

                ssList_Sheet1.Cells[i, 10].Text = ssList_Sheet1.Cells[i, 10].Text.Length == 0 ? " " : ssList_Sheet1.Cells[i, 10].Text;

                ssList_Sheet1.Cells[i, 11].Text = clsVbfunc.GetPatientHPhone(clsDB.DbCon, Dt.Rows[i]["Pano"].ToString().Trim());
                ssList_Sheet1.Cells[i, 12].Text = " ";
            }

            Dt.Dispose();
            Dt = null;
            CPF = null;

            Cursor.Current = Cursors.Default;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "접수현황 통합조회";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회일자 : " + dtpFdate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 12), clsSpread.enmSpdHAlign.Left, false, false);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void DeleteView()
        {
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DROP VIEW " + ComNum.DB_PMPA + "VIEW_TELJEPSU  ";
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
                Cursor.Current = Cursors.Default;

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

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if(ssList_Sheet1.RowCount == 0) return;

            if(e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);
        }
    }
}
