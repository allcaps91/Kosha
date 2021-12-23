using ComBase;
using ComDbB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace HcAdmin
{
    public partial class FrmLicense : Form
    {
        public bool FbNew = false;

        public FrmLicense()
        {
            InitializeComponent();

            FbNew = false;

            //서버접속
            if (clsDbMySql.DBConnect("115.68.23.223", "3306", "dhson", "@thsehdgml#", "dhson") == false)
            {
                ComFunc.MsgBox("라이선스 서버에 접속할 수 없습니다");
                Application.Exit();
            }
            SS1.ActiveSheet.ColumnCount = 3;
            List_Search();
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string strChkHic1 = "N";
            string strChkHic2 = "N";
            string strChkHic3 = "N";

            bool SqlErr;

            if (dptSDate.Text == "") { ComFunc.MsgBox("발급일자가 공란입니다."); dptSDate.Focus(); return; }
            if (txtLicCnt.Text == "") { ComFunc.MsgBox("라이선스 수량이 공란입니다."); txtLicCnt.Focus(); return; }
            if (int.Parse(txtLicCnt.Text) == 0) { ComFunc.MsgBox("라이선스 수량이 공란입니다."); txtLicCnt.Focus(); return; }
            if (txtAdminpass.Text == "") { ComFunc.MsgBox("관리자 비밀번호가 공란입니다."); txtAdminpass.Focus(); return; }
            if (check건강검진.Checked == true) strChkHic1 = "Y";
            if (check보건대행.Checked == true) strChkHic2 = "Y";
            if (check작업측정.Checked == true) strChkHic3 = "Y";

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                if (FbNew == true)
                {
                    SQL += ComNum.VBLF + " INSERT INTO LICMST ";
                    SQL += ComNum.VBLF + "        (LicNo, AdminPass, SDate, EDate, LicCnt,";
                    SQL += ComNum.VBLF + "         chkHic1, chkHic2, chkHic3, Sangho,Juso,";
                    SQL += ComNum.VBLF + "         Damdang, Tel, EMail,DbConnect,Remark) ";
                    SQL += ComNum.VBLF + " VALUES ('" + txtLicno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtAdminpass.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         '" + dptEDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         " + txtLicCnt.Text.Trim() + ", ";
                    SQL += ComNum.VBLF + "         '" + strChkHic1 + "', ";
                    SQL += ComNum.VBLF + "         '" + strChkHic2 + "', ";
                    SQL += ComNum.VBLF + "         '" + strChkHic3 + "', ";
                    SQL += ComNum.VBLF + "         '" + txtSangho.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJuso.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtDamdang.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtDbConnect.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtRemark.Text.Trim() + "') ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE LICMST ";
                    SQL += ComNum.VBLF + "    SET AdminPass     = '" + txtAdminpass.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        SDate         = '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "        EDate         = '" + dptEDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "        LicCnt        = " + txtLicCnt.Text.Trim() + ", ";
                    SQL += ComNum.VBLF + "        chkHic1       = '" + strChkHic1 + "', ";
                    SQL += ComNum.VBLF + "        chkHic2       = '" + strChkHic2 + "', ";
                    SQL += ComNum.VBLF + "        chkHic3       = '" + strChkHic3 + "', ";
                    SQL += ComNum.VBLF + "        Sangho        = '" + txtSangho.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Juso          = '" + txtJuso.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Damdang       = '" + txtDamdang.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Tel           = '" + txtTel.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        EMail         = '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        DbConnect     = '" + txtDbConnect.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        Remark        = '" + txtRemark.Text.Trim() + "'  ";
                    SQL += ComNum.VBLF + "  WHERE LicNo         = '" + txtLicno.Text.Trim() + "'";
                }
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
                {
                    ComFunc.MsgBox("저장 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                OciDb_AdminPass_Update();

                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                List_Search();
                Screen_Clear();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        // 클라우드 서버에 관리자 기본정보 Update //
        private void OciDb_AdminPass_Update()
        {
            string SQL = "";
            string SqlErr = "";
            string strNewPass = "";
            string strJikmu = "YYYNNNNNNNNNNNN";
            DataTable dt = null;
            int intRowAffected = 0;

            strNewPass = clsAES.AES(txtAdminpass.Text.Trim());

            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_USERS ";
                SQL = SQL + ComNum.VBLF + "WHERE SWLICENSE = '" + txtLicno.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND USERID = '1' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count == 0)
                {
                    SQL = ComNum.VBLF + " INSERT INTO HIC_USERS ";
                    SQL += ComNum.VBLF + "        (SWLICENSE, USERID, NAME, DEPT, ROLE, INDATE, TESADATE,";
                    SQL += ComNum.VBLF + "         ISACTIVE, ISDELETED, JIKMU, PASSHASH256,CERTNO,SEQ_WORD,";
                    SQL += ComNum.VBLF + "         LTDUSER,MODIFIED, MODIFIEDUSER, CREATED, CREATEDUSER) ";
                    SQL += ComNum.VBLF + " VALUES ('" + txtLicno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '1','관리자','OSHA','NURSE', ";
                    SQL += ComNum.VBLF + "         '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += ComNum.VBLF + "         '','Y','N','" + strJikmu + "', ";
                    SQL += ComNum.VBLF + "         '" + strNewPass + "','','','',";
                    SQL += ComNum.VBLF + "         SYSDATE,'1',SYSDATE,'1') ";
                }
                else
                {
                    SQL = " UPDATE HIC_USERS ";
                    SQL += "    SET NAME          = '관리자', ";
                    SQL += "        DEPT          = 'OSHA', ";
                    SQL += "        ROLE          = 'NURSE', ";
                    SQL += "       InDate        = '" + dptSDate.Value.ToString("yyyy-MM-dd") + "', ";
                    SQL += "       TesaDate      = '', ";
                    SQL += "        MODIFIED = SYSDATE, MODIFIEDUSER = '1' ";
                    SQL += " WHERE SWLICENSE     = '" + txtLicno.Text.Trim() + "'";
                    SQL += "    AND USERID        = '1' ";
                }
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            List_Search();
        }

        private void List_Search()
        {
            string SQL = "";
            DataTable dt = null;
            int i = 0;

            SS1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "      LicNo, Sangho, SDATE ";
                SQL = SQL + ComNum.VBLF + " FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where SDATE >= '" + dtpViewSDate.Value.ToString("yyyy-MM-dd") + "' ";
                if (TxtViewSangho.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND Sangho LIKE '%" + TxtViewSangho.Text.Trim() + "%' ";
                }
                if (checkEnd.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND EDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY SDATE ";

                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.RowCount = dt.Rows.Count;
                    SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["LicNo"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sangho"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void Screen_Clear()
        {
            txtLicno.Text = "";
            txtSangho.Text = "";
            txtAdminpass.Text = "";
            dptSDate.Text = "";
            dptEDate.Text = "";
            txtLicCnt.Text = "";
            check건강검진.Checked = false;
            check보건대행.Checked = false;
            check작업측정.Checked = false;
            txtJuso.Text = "";
            txtDamdang.Text = "";
            txtEmail.Text = "";
            txtTel.Text = "";
            txtRemark.Text = "";
            txtDbConnect.Text = "";
            FbNew = false;
            삭제ToolStripMenuItem.Enabled = false;
        }

        private void FrmLicense_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void SS1_CellClick_1(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            string strLicno = SS1.ActiveSheet.Cells[e.Row, 0].Value.ToString();

            string SQL = "";

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM LICMST ";
                SQL = SQL + ComNum.VBLF + "Where Licno = '" + strLicno + "' ";

                dt = clsDbMySql.GetDataTable(SQL);

                if (dt.Rows.Count > 0)
                {

                    txtLicno.Text = strLicno;
                    txtAdminpass.Text = dt.Rows[0]["AdminPass"].ToString().Trim();
                    dptSDate.Text = dt.Rows[0]["SDate"].ToString().Trim();
                    dptEDate.Text = dt.Rows[0]["EDate"].ToString().Trim();
                    txtLicCnt.Text = dt.Rows[0]["LicCnt"].ToString().Trim();
                    if (dt.Rows[0]["chkHic1"].ToString().Trim() == "Y") check건강검진.Checked = true;
                    if (dt.Rows[0]["chkHic2"].ToString().Trim() == "Y") check보건대행.Checked = true;
                    if (dt.Rows[0]["chkHic3"].ToString().Trim() == "Y") check작업측정.Checked = true;
                    txtSangho.Text = dt.Rows[0]["Sangho"].ToString().Trim();
                    txtJuso.Text = dt.Rows[0]["Juso"].ToString().Trim();
                    txtDamdang.Text = dt.Rows[0]["Damdang"].ToString().Trim();
                    txtTel.Text = dt.Rows[0]["Tel"].ToString().Trim();
                    txtEmail.Text = dt.Rows[0]["EMail"].ToString().Trim();
                    txtDbConnect.Text = dt.Rows[0]["DbConnect"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                FbNew = false;
                삭제ToolStripMenuItem.Enabled = true;
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
                Cursor.Current = Cursors.Default;
            }

        }

        private void 닫기ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 신규발급ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //                0123456789
            string strConv = "P583E96T2M";
            int[] rank = new int[] { 11, 8, 4, 10, 1, 6, 2, 5, 3, 7, 12, 9 };
            string strTime = DateTime.Now.ToString("yyyyMMddmmss");
            string strResult = "";
            int i = 0;

            for (i = 0; i < 12; i++)
            {
                int inx = int.Parse(strTime.Substring(rank[i] - 1, 1));
                string strChar = strConv.Substring(inx, 1);
                strResult = strResult + strChar;
                if (i == 3 || i == 7) strResult += "-";
            }
            Screen_Clear();

            삭제ToolStripMenuItem.Enabled = false;
            FbNew = true;
            txtLicno.Text = strResult;
            dptSDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }

        private void 삭제ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            bool SqlErr;

            if (FbNew == true) return;

            if (ComFunc.MsgBoxQ("삭제하시겠습니까") == DialogResult.No) return;

            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE FROM LICMST ";
                SQL += ComNum.VBLF + "  WHERE LicNo = '" + txtLicno.Text.Trim() + "'";

                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
                {
                    ComFunc.MsgBox("삭제 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("삭제되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
                List_Search();
                Screen_Clear();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void pC저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strNewData = "";
            string strPcData = "";

            strNewData = txtLicno.Text.Trim() + "{}";
            strNewData += txtSangho.Text.Trim() + "{}";
            strNewData += dptEDate.Value.ToString("yyyy-MM-dd") + "{}";
            strNewData += txtAdminpass.Text.Trim() + "{}";

            strPcData = clsAES.AES(strNewData);
            System.IO.File.WriteAllText(@"C:\Windows\System32\acledit392io87.dll", strPcData);

            ComFunc.MsgBox("PC에 해당회사로 라이선스가 설정되었습니다.", "알림");
        }

        private void 헬스소프트실행ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string strNewData = "";
            string strPcData = "";

            // 라이선스 정보를 PC에 저장
            strNewData = txtLicno.Text.Trim() + "{}";
            strNewData += txtSangho.Text.Trim() + "{}";
            strNewData += dptEDate.Value.ToString("yyyy-MM-dd") + "{}";
            strNewData += txtAdminpass.Text.Trim() + "{}";

            // 프로그램 실행
            strPcData = clsAES.AES(strNewData);
            System.IO.File.WriteAllText(@"C:\Windows\System32\acledit392io87.dll", strPcData);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\HealthSoft\HSMain\HS_OSHA.exe";
            startInfo.Arguments = null;
            Process.Start(startInfo);
        }

        private void 기본정보생성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            //데모용 라이선스는 생성 불가
            if (txtLicno.Text.Trim() == "3M52-85P5-8855") { ComFunc.MsgBox("이 라이선스는 기준 Data임으로 생성 불가함."); return; }
            if (txtLicno.Text.Trim() == "") { ComFunc.MsgBox("라이선스번호가 공란입니다."); return; }

            SQL = "SELECT COUNT(*) CNT FROM HIC_CODES WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (VB.Val(dt.Rows[0]["CNT"].ToString()) == 0)
            { 
                //데모용 라이선스의 정보를 복사
                SQL = "INSERT INTO HIC_CODES (";
                SQL += " ID,CODE,GROUPCODE,GROUPCODENAME,CODENAME,SORTSEQ,EXTEND1,EXTEND2,";
                SQL += " DESCRIPTION,MODIFIED,MODIFIEDUSER,ISACTIVE,ISDELETED,PROGRAM,SWLICENSE ) ";
                SQL += "SELECT HC_CODE_ID_SEQ.NEXTVAL,CODE,GROUPCODE,GROUPCODENAME,CODENAME,SORTSEQ,EXTEND1,EXTEND2,";
                SQL += "        DESCRIPTION,MODIFIED,'1',ISACTIVE,ISDELETED,PROGRAM,'" + txtLicno.Text.Trim() + "' ";
                SQL += "  FROM HIC_CODES ";
                SQL += " WHERE SWLICENSE='3M52-85P5-8855' "; //데모용 기본자료
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            SQL = "SELECT COUNT(*) CNT FROM HIC_LTD WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (VB.Val(dt.Rows[0]["CNT"].ToString()) == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_LTD (CODE,SANGHO,NAME,TEL,FAX,EMAIL,MAILCODE,JUSO,HTEL,";
                SQL += " GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,JISA,SWLICENSE ) ";
                SQL += " VALUES (HC_LTD_SEQ.NEXTVAL,'" + txtSangho.Text.Trim() + "','";
                SQL += txtSangho.Text.Trim() + "','" + txtTel.Text.Trim() + "','','";
                SQL += txtEmail.Text.Trim() + "','','" + txtJuso.Text.Trim() + "','";
                SQL += txtTel.Text.Trim() + "','N','Y','Y','N','0719','" + txtLicno.Text.Trim() + "') ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                SQL = "SELECT CODE FROM HIC_LTD ";
                SQL += " WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
                SQL += "   AND SANGHO='" + txtSangho.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows[0]["CODE"].ToString() != "")
                {
                    SQL = "INSERT INTO HIC_OSHA_SITE (ID,ISACTIVE,TASKNAME,PARENTSITE_ID,HASCHILD,ISPARENTCHARGE,";
                    SQL += " ISQUARTERCHARGE,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE ) ";
                    SQL += " VALUES (" + dt.Rows[0]["CODE"].ToString() + ",'Y','사업장 등록',0,'N','Y',";
                    SQL += "         'N',SYSTIMESTAMP,'1',SYSTIMESTAMP,'1','" + txtLicno.Text.Trim() + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            ComFunc.MsgBox("복사 완료", "알림");
        }

        private void BtnSearch_Click_1(object sender, EventArgs e)
        {

        }
    }
}
