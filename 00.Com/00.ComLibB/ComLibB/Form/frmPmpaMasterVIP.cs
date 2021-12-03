using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : VIP환자 등록관리
/// Author : 박병규
/// Create Date : 2017.06.20
/// </summary>
/// <history>
/// </history>

namespace ComLibB
{
    public partial class frmPmpaMasterVIP : Form
    {
        private string GstrPtNo = "";
        
        ComQuery  CQ = new ComQuery();
        ComFunc CF = new ComFunc();
        clsComSMS CS = new clsComSMS();
        clsPublic CP = new clsPublic();

        String strOldGubun = string.Empty;

        public frmPmpaMasterVIP()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaMasterVIP(string strPtNo)
        {
            InitializeComponent();
            setEvent();

            GstrPtNo = strPtNo;
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            //LostFocus 이벤트
            this.txtPtno.LostFocus += new EventHandler(eControl_LostFocus);

            //KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboVIP.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboVIP2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtCnt.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRoomCode.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        private void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (char)13) { cboVIP.Focus(); }
            if (sender == this.cboVIP && e.KeyChar == (char)13) { txtRemark.Focus(); }
            if (sender == this.cboVIP2 && e.KeyChar == (char)13) { dtpFdate.Focus(); }
            if (sender == this.dtpFdate && e.KeyChar == (char)13) { dtpTdate.Focus(); }
            if (sender == this.dtpTdate && e.KeyChar == (char)13) { txtCnt.Focus(); }
            if (sender == this.txtCnt && e.KeyChar == (char)13) { txtRoomCode.Focus(); }
            if (sender == this.txtRoomCode && e.KeyChar == (char)13) { btnSearch2.Focus(); }
        }

        private void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
            {
                if (txtPtno.Text == "") { txtSname.Text = ""; return; }
                txtPtno.Text = ComFunc.LPAD(txtPtno.Text.Trim(), 8, "0");
                txtSname.Text = CF.Read_Patient(clsDB.DbCon, txtPtno.Text, "2");
                Get_DataLoad_Record();
            }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            frmPmpaMasterVIP frm = new frmPmpaMasterVIP();
            ComFunc.Form_Center(frm);

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlTop2);
            ComFunc.SetAllControlClear(pnlBody);
            ComFunc.SetAllControlClear(pnlBody2);

            //VIP 목록
            DataTable Dt = ComQuery.Set_BaseCode_Foundation(clsDB.DbCon, "BAS_VIP_구분코드", "");
            string strCode = string.Empty;
            string strCodeName = string.Empty;

            cboVIP.Items.Add("**.전체");
            cboVIP2.Items.Add("**.전체");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strCode = Dt.Rows[i]["CODE"].ToString().Trim();
                strCodeName = Dt.Rows[i]["NAME"].ToString().Trim();

                cboVIP.Items.Add(strCode + "." + strCodeName);

                //06.개인종검 10회이상자, 08.특실을 지속적으로 사용고객만 일괄등록관리에서 관리한다.
                if (strCode == "06" || strCode == "08")
                {
                    cboVIP2.Items.Add(strCode + "." + strCodeName);
                }
            }
            cboVIP.SelectedIndex = 0;
            cboVIP2.SelectedIndex = 0;

            Dt.Dispose();
            Dt = null;

            txtSabun.Text = clsType.User.IdNumber;
            txtSabunName.Text = clsType.User.UserName;

            dtpFdate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-700);
            dtpTdate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            if (GstrPtNo != "")
            {
                txtPtno.Text = GstrPtNo;
                txtSname.Text = CF.Read_Patient(clsDB.DbCon, txtPtno.Text,"2");
                Get_DataLoad_Record();
            }
        }

        //개인별 등록관리 조회
        private void Get_DataLoad_Record()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            if (txtPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호가 공란입니다.", "확인");
                eForm_Clear_STIRecord();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SNAME, TO_CHAR(BIRTH,'YYYY-MM-DD') AS BIRTH, GBBIRTH, ";
            SQL += ComNum.VBLF + "        HPHONE, EMAIL, GB_VIP, GB_VIP_REMARK, GB_VIP_SABUN, ";
            SQL += ComNum.VBLF + "        TO_CHAR(GB_VIP_DATE,'YYYY-MM-DD HH24:MI') AS GB_VIP_DATE, ";
            SQL += ComNum.VBLF + "        TEL, JIKUP, RELIGION, GBINFOR ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + txtPtno.Text + "' ";

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

            txtInfo.Text = "사번 : " + Dt.Rows[0]["GB_VIP_SABUN"].ToString().Trim() + " [" + Dt.Rows[0]["GB_VIP_DATE"].ToString().Trim()  + "]";
            txtRemark.Text = Dt.Rows[0]["GB_VIP_REMARK"].ToString();

            if (Dt.Rows[0]["GB_VIP"].ToString().Trim() != "")
            {
                strOldGubun = Dt.Rows[0]["GB_VIP"].ToString().Trim();
                ComFunc.ComboFind(cboVIP, "L", 2, Dt.Rows[0]["GB_VIP"].ToString().Trim());
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void eForm_Clear_STIRecord()
        {
            strOldGubun = "";

            txtPtno.Text = "";
            txtSname.Text = "";
            cboVIP.SelectedIndex = 0;
            txtInfo.Text = "";
            txtRemark.Text = "";
            
            txtPtno.Focus();
        }

        private void eForm_Clear_STIBatch()
        {
            txtCnt.Text = "";
            txtRoomCode.Text = "";
            cboVIP2.SelectedIndex = 0;
            ComFunc.SetAllControlClear(pnlBody2);

            cboVIP2.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Get_DataLoad_Record();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인
            
            //개인별 등록관리
            if (STCMain.SelectedTabIndex == 0)
            {
                if ( txtPtno.Text == "" || txtSname.Text == "")
                {
                    ComFunc.MsgBox("등록번호가 공란입니다.","오류");
                    txtPtno.Focus();
                    return;
                }

                if (cboVIP.Text == "" || VB.Left(cboVIP.Text,2) == "**")
                {
                    ComFunc.MsgBox("VIP 구분을 선택하시기 바랍니다.", "오류");
                    cboVIP.Focus();
                    return;
                }

                if (txtRemark.Text == "")
                {
                    ComFunc.MsgBox("VIP 참고사항이 공란입니다.", "오류");
                    txtRemark.Focus();
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);
                

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL += ComNum.VBLF + "    SET GB_VIP_SABUN  = '" + VB.Val(txtSabun.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        GB_VIP_DATE   = SYSDATE , ";
                    SQL += ComNum.VBLF + "        GB_VIP        = '" + VB.Left(cboVIP.Text.Trim(), 2) + "', ";
                    SQL += ComNum.VBLF + "        GB_VIP_REMARK = '" + txtRemark.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "  WHERE PANO          = '" + txtPtno.Text.Trim() + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //후불대상자자동등록
                    CQ.AUTO_MASTER_INSERT(clsDB.DbCon, txtPtno.Text.Trim(), txtSname.Text.Trim(), "VIP 자동저장");

                    //신규일경우 SMS전송
                    if (strOldGubun != VB.Left(cboVIP.Text.Trim(), 2))
                    {
                        string strMsg = string.Empty;

                        strMsg = txtSname.Text.Trim() + "(" + txtPtno.Text.Trim() + ")님 " ;
                        strMsg += cboVIP.Text.Trim() + " VIP 등록됨 ";
                        strMsg += "[" + txtSabun.Text.Trim() + "." + txtSabunName.Text.Trim() + "]";

                        if (CS.SMS_Broker_Send_Ex(clsDB.DbCon, "53", txtPtno.Text.Trim(), txtSname.Text.Trim(), "010-6524-3120", "054-289-8003", strMsg, "Web Broker전송") == false)
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
                    Cursor.Current = Cursors.Default;

                    eForm_Clear_STIRecord();
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
            //일괄등록관리
            else
            {
                if (cboVIP2.Text == "" || VB.Left(cboVIP2.Text, 2) == "**")
                {
                    ComFunc.MsgBox("VIP 구분을 선택하시기 바랍니다.", "오류");
                    cboVIP2.Focus();
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);
                

                try
                {
                    for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        string strGubun = string.Empty;
                        string strPtno = string.Empty;
                        string strSname = string.Empty;

                        strGubun = VB.Left(cboVIP2.Text.Trim(), 2);
                        strPtno = ssList_Sheet1.Cells[i, 1].Text.Trim();
                        strSname = ssList_Sheet1.Cells[i, 2].Text.Trim();

                        if (ssList_Sheet1.Cells[i,0].Text.Trim() == "1")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                            SQL += ComNum.VBLF + "    SET GB_VIP_sabun  = '" + clsType.User.IdNumber + "', ";
                            SQL += ComNum.VBLF + "        GB_VIP_date   = SYSDATE , ";
                            SQL += ComNum.VBLF + "        GB_VIP        = '" + strGubun + "', ";
                            SQL += ComNum.VBLF + "        GB_VIP_REMARK = '일괄등록' ";
                            SQL += ComNum.VBLF + "  WHERE PANO          ='" + strPtno + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //후불대상자자동등록
                            CQ.AUTO_MASTER_INSERT(clsDB.DbCon, strPtno, strSname, "VIP 일괄저장");

                            ssList_Sheet1.Cells[i, 0].Text = "";
                        }
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) { return; }     //권한확인

            //개인별 등록관리
            if (STCMain.SelectedTabIndex == 0)
            {
                if (txtPtno.Text == "" || txtSname.Text == "")
                {
                    ComFunc.MsgBox("등록번호가 공란입니다.", "오류");
                    txtPtno.Focus();
                    return;
                }

                if (txtSabun.Text.Trim() == "" )
                {
                    ComFunc.MsgBox("사원번호가 공란입니다.", "오류");
                    txtSabun.Focus();
                    return;
                }
                
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);
                

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL += ComNum.VBLF + "    SET GB_VIP        = '', ";
                    SQL += ComNum.VBLF + "        GB_VIP_REMARK = ''  ";
                    SQL += ComNum.VBLF + "  WHERE PANO          = '" + txtPtno.Text.Trim() + "' ";
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
                    CQ.AUTO_MASTER_DELETE(clsDB.DbCon, txtPtno.Text.Trim(), "VIP 자동삭제");

                    clsDB.setCommitTran(clsDB.DbCon);
                    MessageBox.Show("삭제되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
                    Cursor.Current = Cursors.Default;

                    eForm_Clear_STIRecord();
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
            //일괄등록관리
            else
            {
                ComFunc.MsgBox("일괄등록관리 화면에서는 삭제버튼 기능을 사용할 수 없습니다.", "확인");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //개인별 등록관리
            if (STCMain.SelectedTabIndex == 0)
            {
                eForm_Clear_STIRecord();
            }
            //일괄등록관리
            else
            {
                eForm_Clear_STIBatch();
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            Get_DataLoad_Batch();
        }

        //일괄등록관리 데이터조회
        private void Get_DataLoad_Batch()
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인
            ComFunc.SetAllControlClear(pnlBody2);

            if (cboVIP2.Text.Trim() == "" || VB.Left(cboVIP2.Text.Trim(),2) == "**")
            {
                ComFunc.MsgBox("VIP 구분을 선택하시기 바랍니다.", "확인");
                eForm_Clear_STIBatch();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            if (VB.Left( cboVIP2.Text.Trim(),2) == "06")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.PTNO AS PANO, B.SNAME, MAX(B.LASTDATE) AS SDATE, COUNT(A.PTNO) AS CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "HEA_JEPSU A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.PTNO <> '00000000' ";
                SQL += ComNum.VBLF + "    AND A.GbSts IN ('9') ";           //판정완료
                SQL += ComNum.VBLF + "    AND B.GB_VIP IS NULL ";           //VIP 미등록자
                SQL += ComNum.VBLF + "    AND A.GjJong IN ('11', '12') ";
                SQL += ComNum.VBLF + "    AND A.Ptno = B.Pano(+) ";
                SQL += ComNum.VBLF + "  GROUP BY A.Ptno, B.SName ";
                SQL += ComNum.VBLF + " HAVING COUNT(A.Ptno) >= 10 ";

            }
            else if (VB.Left(cboVIP2.Text.Trim(), 2) == "08")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.Pano, B.SName, MAX(B.LASTDATE) AS SDATE, COUNT(A.Pano) AS CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.InDate >= TO_DATE('" + dtpFdate.Text + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.InDate <  TO_DATE('" + dtpTdate.Text + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.Gbsts NOT IN ('9') ";       //입원취소
                SQL += ComNum.VBLF + "    AND B.GB_VIP IS NULL ";           //VIP 미등록자
                SQL += ComNum.VBLF + "    AND A.IPDNO IN ( SELECT IPDNO ";
                SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "IPD_BM ";
                SQL += ComNum.VBLF + "                      WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "                        AND InDate >= TO_DATE('" + dtpFdate.Text + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "                        AND InDate <  TO_DATE('" + dtpTdate.Text + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "                        AND Gbsts NOT IN ('9') ";
                SQL += ComNum.VBLF + "                        AND RoomCode IN ( " + txtRoomCode.Text.Trim() + " ) ) ";
                SQL += ComNum.VBLF + "    AND A.Pano = B.Pano(+) ";
                SQL += ComNum.VBLF + "  GROUP BY A.Pano, B.SName ";
                SQL += ComNum.VBLF + "  HAVING COUNT(A.Pano) >= " + VB.Val(txtCnt.Text) + " ";
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
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["CNT"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Convert.ToDateTime(Dt.Rows[i]["SDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;

        }

        private void STCMain_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
        {
            //개인별 등록관리
            if (STCMain.SelectedTabIndex == 0)
            {
                btnDelete.Enabled = true;
            }
            //일괄등록관리
            else
            {
                btnDelete.Enabled = false;
            }

        }

        //컬럼헤더가 체크박스일 경우 전체선택 여부
        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            bool allChecked = false;

            if (e.ColumnHeader == false) { return; }

            if (e.Column == 0)
            {
                if (ssList.Sheets[0].ColumnHeader.Cells[0, 0].Value == null || Convert.ToBoolean(ssList.Sheets[0].ColumnHeader.Cells[0, 0].Value) == false)
                    allChecked = false;
                else
                    allChecked = true;

                if (allChecked == true)
                {
                    for (i = 0; i < ssList.Sheets[0].RowCount; i++)
                    {
                        ssList.Sheets[0].Cells[i, 0].Value = false;
                    }
                    ssList.Sheets[0].ColumnHeader.Cells[0, 0].Value = false;
                }
                else
                {
                    for (i = 0; i < ssList.Sheets[0].RowCount; i++)
                    {
                        ssList.Sheets[0].Cells[i, 0].Value = true;
                    }
                    ssList.Sheets[0].ColumnHeader.Cells[0, 0].Value = true;
                }
            }
            else
            {

            }

        }
    }
}
