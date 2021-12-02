using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmAfterPaymentReg
    /// File Name : frmAfterPaymentReg.cs
    /// Title or Description : 외래 후불일괄수납 대상자1 등록
    /// Author : 박창욱
    /// Create Date : 2017-06-12
    /// Update Histroy :
    /// </summary>
    /// <history>  
    /// VB\Frm후불대상등록.frm(Frm후불대상등록) -> frmAfterPaymentReg.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\Frm후불대상등록.frm(Frm후불대상등록)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\buppat\\buppat.vbp
    /// </vbp>
    public partial class frmAfterPaymentReg : Form
    {
        private string gstrJobPart = "";
        private string gstrJobName = "";

        string FstrPano = "";
        string FstrROWID = "";
        string FstrFDate = "";
        string FstrEntDate = "";

        public frmAfterPaymentReg()
        {
            InitializeComponent();
        }

        public frmAfterPaymentReg(string strJobPart, string strJobName)
        {
            InitializeComponent();
            gstrJobPart = strJobPart;
            gstrJobName = strJobName;
        }

        private void frmAfterPaymentReg_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            SCREEN_CLEAR();

            cboGbn.Items.Clear();
            cboGbn.Items.Add("1.원무과");
            cboGbn.Items.Add("2.포스코위탁");
            cboGbn.SelectedIndex = 0;

            cboGbn2.Items.Clear();
            cboGbn2.Items.Add("1.원무과");
            cboGbn2.Items.Add("2.포스코위탁");
            cboGbn2.SelectedIndex = 0;

            txtJob.Text = gstrJobName;

            txtPano2.Text = "";
            txtSName.Text = "";
        }

        private void SCREEN_CLEAR()
        {
            ssInfo_clear();
            txtPano.Text = "";

            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            txtRemark.Text = "";

            FstrPano = "";
            FstrROWID = "";

            FstrFDate = "";
            FstrEntDate = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ssInfo_clear()
        {
            int xx = 0;
            int yy = 0;

            for (xx = 0; xx < ssInfo_Sheet1.RowCount; xx++)
            {
                for (yy = 0; yy < 1; yy++)
                {
                    ssInfo_Sheet1.Cells[yy, xx].Text = "";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            //기본 마스터
            string strPano = "";
            string StrSname = "";
            string strSDate = "";    //선택시작일
            string strEDate = "";    //선택시작일
            string strRemark = "";

            string strTempPano = "";    //임시 등록번호 보관
            string strPosco = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strPano = ssInfo_Sheet1.Cells[0, 0].Text;
            strTempPano = strPano;
            StrSname = ssInfo_Sheet1.Cells[0, 1].Text;

            if (strPano == "" || StrSname == "")
            {
                ComFunc.MsgBox("환자를 선택 후 저장하십시오.");
                return;
            }

            strSDate = dtpSDate.Value.ToString();
            strEDate = dtpEDate.Value.ToString();
            strRemark = txtRemark.Text.Trim();
            strPosco = VB.Left(cboGbn.Text, 1);

            //데이터 점검
            if (FstrROWID == "" && Convert.ToDateTime(strSDate) < Convert.ToDateTime(clsPublic.GstrSysDate))
            {
                ComFunc.MsgBox("시작일자는 오늘보다 작을 수 없습니다.");
                return;
            }
            if (Convert.ToDateTime(strSDate) > Convert.ToDateTime(strEDate) && strEDate != "")
            {
                ComFunc.MsgBox("시작일자는 오늘보다 작을 수 없습니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = " SELECT ROWID FROM KOSMOS_PMPA.BAS_AUTO_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND GBPOSCO ='" + strPosco + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate ='') ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_AUTO_MST ( PANO,SNAME,GUBUN,SDATE,EDATE,DELDATE,";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN,ENTDATE,ENTDATE2,REMARK,GBPOSCO ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " '" + strPano + "','" + StrSname + "','1', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strSDate + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strEDate + "','YYYY-MM-DD'),'',";
                    SQL = SQL + ComNum.VBLF + " '" + gstrJobPart + "',SYSDATE, SYSDATE,'" + strRemark + "','" + strPosco + "') ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else if (FstrROWID != "")
                {
                    //이전내역 백업
                    SQL = "";
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_AUTO_MST_HIS  ";
                    SQL = SQL + ComNum.VBLF + " ( PANO,SNAME,GUBUN,DEPTCODE,SDATE,EDATE,DELDATE,ENTDATE,ENTDATE2,ENTSABUN,REMARK,GBPOSCO)   ";
                    SQL = SQL + ComNum.VBLF + "   SELECT PANO,SNAME,GUBUN,DEPTCODE,SDATE,EDATE,DELDATE,ENTDATE,ENTDATE2,ENTSABUN,REMARK,GBPOSCO ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.BAS_AUTO_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID ='" + FstrROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.BAS_AUTO_MST SET ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN ='" + gstrJobPart + "', ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE2 = SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + " REMARK  = '" + strRemark + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();

                txtPano.Focus();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message, SQL);
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtPano.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (FstrROWID == "")
            {
                ComFunc.MsgBox("삭제할 등록번호를 먼저 선택하십시오.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (ComFunc.MsgBoxQ("정말로 선택한 건을 삭제하시겠습니까?" + ComNum.VBLF + ComNum.VBLF
                                   + "삭제되면 후불대상에서 제외됩니다.", "삭제확인") == DialogResult.Yes)
                {
                    ComFunc.ReadSysDate(clsDB.DbCon);

                    //이전내역 백업
                    SQL = "";
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_AUTO_MST_HIS  ";
                    SQL = SQL + ComNum.VBLF + " ( PANO,SNAME,GUBUN,DEPTCODE,SDATE,EDATE,DELDATE,ENTDATE,ENTDATE2,ENTSABUN,REMARK,GBPOSCO )   ";
                    SQL = SQL + ComNum.VBLF + "   SELECT PANO,SNAME,GUBUN,DEPTCODE,SDATE,EDATE,DELDATE,ENTDATE,ENTDATE2,ENTSABUN,REMARK,GBPOSCO ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.BAS_AUTO_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID ='" + FstrROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.BAS_AUTO_MST SET ";
                    SQL = SQL + ComNum.VBLF + " DELDATE =TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ,  ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN ='" + clsPublic.GstrJobPart + "', ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE2 = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("선택한 건을 삭제하였습니다.");
                SCREEN_CLEAR();
                txtPano.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nRead = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            txtRemark.Text = "";

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            if (txtPano2.Text.Trim() == "" && txtSName.Text.Trim() == "")
            {
                if (ComFunc.MsgBoxQ("검색조건을 넣지 않으면 조회시간이 길어집니다. 계속하시겠습니까?", "확인") == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                SQL = "";
                SQL = " SELECT Pano,SName,Remark,EntSabun,ROWID,GBPOSCO, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDate,'YYYY-MM-DD') SDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EDate,'YYYY-MM-DD') EDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI') EntDate,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EntDate2,'YYYY-MM-DD HH24:MI') EntDate2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_AUTO_MST ";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='1' ";
                if (txtPano2.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND Pano ='" + txtPano2.Text.Trim() + "' ";
                }
                if (txtSName.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND SName LIKE '%" + txtSName.Text.Trim() + "%' ";
                }
                SQL = SQL + ComNum.VBLF + " AND GBPOSCO = '" + VB.Left(cboGbn2.Text, 1) + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (nRead > 0)
                {
                    for (i = 0; i < nRead - 1; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["EntDate2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["EntSabun"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            txtPano.Focus();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim() != "")
                {
                    txtPano.Text = Convert.ToInt32(txtPano.Text).ToString("00000000");
                    Pano_Info_DISP(txtPano.Text);

                    READ_BAS_AUTO_MST(txtPano.Text);
                }

                SendKeys.Send("{Tab}");
            }
        }

        private void Pano_Info_DISP(string argPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {

                //환자정보 체크
                SQL = "";
                SQL = " SELECT a.Pano,a.SName,a.Jumin1 || '-' || a.Jumin2 AS Jumin, ";
                SQL = SQL + ComNum.VBLF + "  a.Juso,a.ZipCode1 || a.ZipCode1 AS ZipCode,a.Tel,a.Hphone, ";
                SQL = SQL + ComNum.VBLF + "  a.Jumin1,a.Jumin3 ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_PATIENT a ";
                SQL = SQL + ComNum.VBLF + "   WHERE a.PANO ='" + argPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrPano = dt.Rows[0]["Pano"].ToString().Trim();
                    ssInfo_Sheet1.Cells[0, 0].Text = FstrPano;
                    ssInfo_Sheet1.Cells[0, 1].Text = dt.Rows[0]["SName"].ToString().Trim();
                    ssInfo_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Jumin1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    ssInfo_Sheet1.Cells[0, 3].Text = clsVbfunc.GetBASMail(clsDB.DbCon, dt.Rows[0]["ZipCode"].ToString().Trim() + " " + dt.Rows[0]["Juso"].ToString().Trim());
                    ssInfo_Sheet1.Cells[0, 4].Text = dt.Rows[0]["Tel"].ToString().Trim() + ComNum.VBLF + dt.Rows[0]["Hphone"].ToString().Trim();
                }
                else
                {
                    ComFunc.MsgBox("이 등록번호의 환자가 존재하지 않습니다.");
                    dt.Dispose();
                    dt = null;
                    ssInfo_clear();
                    return;
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_BAS_AUTO_MST(string argPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = " SELECT ROWID,TO_CHAR(SDate,'YYYY-MM-DD') SDate,Remark FROM KOSMOS_PMPA.BAS_AUTO_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='1' ";
                SQL = SQL + ComNum.VBLF + "  AND (DelDate IS NULL OR DelDate ='') ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                FstrROWID = "";

                if(dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    dtpSDate.Value = Convert.ToDateTime(dt.Rows[0]["SDate"].ToString().Trim());
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();

                    ComFunc.MsgBox("이미 등록되어 있습니다.");

                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                }
                else
                {
                    ComFunc.MsgBox("신규 자료입니다.");
                    btnSave.Enabled = true;
                    dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            string strIO = "";

            ComFunc CF = new ComFunc();

            if (CF.READ_BARCODE(txtPano.Text.Trim()) == true)
            {
                txtPano.Text = clsPublic.GstrBarPano;
            }
            else
            {
                txtPano.Text = Convert.ToInt32(txtPano.Text).ToString("00000000");
            }
                
                if (txtPano.Text != "")
            {
                txtPano.Text = Convert.ToInt32(txtPano.Text).ToString("00000000");
            }
        }

        private void txtPano2_Leave(object sender, EventArgs e)
        {
            if (txtPano2.Text != "")
            {
                txtPano2.Text = Convert.ToInt32(txtPano2.Text).ToString("00000000");
            }
        }

        private void dtpSDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dtpEDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{Tab}");
            }
        }
    }
}
