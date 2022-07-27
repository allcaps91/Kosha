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
        private string strLtdno1 = "";
        private string strLtdno2 = "";

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
                    SQL += ComNum.VBLF + "         Damdang, Tel, EMail,DbConnect,SMTP_Setup,Remark) ";
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
                    SQL += ComNum.VBLF + "         '" + txtSMTP.Text.Trim() + "', ";
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
                    SQL += ComNum.VBLF + "        SMTP_Setup    = '" + txtSMTP.Text.Trim() + "', ";
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
            txtSMTP.Text = "";
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
                    txtSMTP.Text = dt.Rows[0]["SMTP_Setup"].ToString().Trim();
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
            System.IO.File.WriteAllText(@"C:\HealthSoft\acledit392io87.dll", strPcData);

            READ_Licno_Disk();

            ComFunc.MsgBox("PC에 해당회사로 라이선스가 설정되었습니다.", "알림");
        }

        private bool READ_Licno_Disk()
        {
            string strPcData = "";
            string strNewData = "";

            clsType.HosInfo.SwLicense = "";
            clsType.HosInfo.SwLicInfo = "";

            //파일형식: 라이선스번호{}회사명{}종료일자{}관리자비번{}
            string strLicFile = @"C:\HealthSoft\acledit392io87.dll";
            if (System.IO.File.Exists(strLicFile) == true)
            {
                strPcData = System.IO.File.ReadAllText(strLicFile);
                strNewData = clsAES.DeAES(strPcData);
                if (VB.L(strNewData, "{}") != 5)
                {
                    ComFunc.MsgBox("라이선스 정보가 손상되어 종료됩니다.");
                    return false;
                }

                clsType.HosInfo.SwLicense = VB.Pstr(strNewData, "{}", 1);
                clsType.HosInfo.SwLicInfo = strNewData;

                return true;
            }
            else
            {
                ComFunc.MsgBox("라이선스 정보가 손상되어 종료됩니다.");
                return false;
            }
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
            System.IO.File.WriteAllText(@"C:\HealthSoft\acledit392io87.dll", strPcData);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\헬스소프트\Debug\HEALTHSOFT.exe";
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
            if (txtLicno.Text.Trim() == "9555-88P5-8P33") { ComFunc.MsgBox("이 라이선스는 기준 Data임으로 생성 불가함."); return; }
            if (txtLicno.Text.Trim() == "") { ComFunc.MsgBox("라이선스번호가 공란입니다."); return; }

            // 1.HIC_CODES를 복사
            SQL = "SELECT COUNT(*) CNT FROM HIC_CODES WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (VB.Val(dt.Rows[0]["CNT"].ToString()) == 0)
            { 
                SQL = "INSERT INTO HIC_CODES (";
                SQL += " ID,CODE,GROUPCODE,GROUPCODENAME,CODENAME,SORTSEQ,EXTEND1,EXTEND2,";
                SQL += " DESCRIPTION,MODIFIED,MODIFIEDUSER,ISACTIVE,ISDELETED,PROGRAM,SWLICENSE ) ";
                SQL += "SELECT HC_CODE_ID_SEQ.NEXTVAL,CODE,GROUPCODE,GROUPCODENAME,CODENAME,SORTSEQ,EXTEND1,EXTEND2,";
                SQL += "        DESCRIPTION,MODIFIED,'1',ISACTIVE,ISDELETED,PROGRAM,'" + txtLicno.Text.Trim() + "' ";
                SQL += "  FROM HIC_CODES ";
                SQL += " WHERE SWLICENSE='9555-88P5-8P33' "; //대한
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 2.HIC_LTD에 연습회사, 감자상회 각각 생성
            SQL = "SELECT CODE FROM HIC_LTD WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
            SQL += " AND SANGHO='연습회사' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strLtdno1 = "";
            if (dt.Rows.Count > 0) strLtdno1 = dt.Rows[0]["CODE"].ToString();

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_LTD (CODE,SANGHO,NAME,TEL,FAX,EMAIL,MAILCODE,JUSO,HTEL,";
                SQL += " GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,JISA,SWLICENSE ) ";
                SQL += "VALUES (HC_LTD_SEQ.NEXTVAL,'연습회사','연습회사','";
                SQL += txtTel.Text.Trim() + "','','";
                SQL += txtEmail.Text.Trim() + "','','" + txtJuso.Text.Trim() + "','";
                SQL += txtTel.Text.Trim() + "','N','Y','Y','N','0719','" + txtLicno.Text.Trim() + "') ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                SQL = "SELECT CODE FROM HIC_LTD ";
                SQL += " WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
                SQL += "   AND SANGHO='연습회사' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0) strLtdno1 = dt.Rows[0]["CODE"].ToString();
                if (dt.Rows[0]["CODE"].ToString() != "")
                {
                    SQL = "INSERT INTO HIC_OSHA_SITE (ID,ISACTIVE,TASKNAME,PARENTSITE_ID,HASCHILD,ISPARENTCHARGE,";
                    SQL += " ISQUARTERCHARGE,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE ) ";
                    SQL += " VALUES (" + dt.Rows[0]["CODE"].ToString() + ",'Y','사업장 등록',0,'N','Y',";
                    SQL += "         'N',SYSTIMESTAMP,'1',SYSTIMESTAMP,'1','" + txtLicno.Text.Trim() + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            SQL = "SELECT CODE FROM HIC_LTD WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
            SQL += " AND SANGHO='감자상회' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strLtdno2 = "";
            if (dt.Rows.Count > 0) strLtdno2 = dt.Rows[0]["CODE"].ToString();

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_LTD (CODE,SANGHO,NAME,TEL,FAX,EMAIL,MAILCODE,JUSO,HTEL,";
                SQL += " GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,JISA,SWLICENSE ) ";
                SQL += "VALUES (HC_LTD_SEQ.NEXTVAL,'감자상회','감자상회','";
                SQL += txtTel.Text.Trim() + "','','";
                SQL += txtEmail.Text.Trim() + "','','" + txtJuso.Text.Trim() + "','";
                SQL += txtTel.Text.Trim() + "','N','Y','Y','N','0719','" + txtLicno.Text.Trim() + "') ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                SQL = "SELECT CODE FROM HIC_LTD ";
                SQL += " WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
                SQL += "   AND SANGHO='감자상회' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0) strLtdno2 = dt.Rows[0]["CODE"].ToString();
                if (dt.Rows[0]["CODE"].ToString() != "")
                {
                    SQL = "INSERT INTO HIC_OSHA_SITE (ID,ISACTIVE,TASKNAME,PARENTSITE_ID,HASCHILD,ISPARENTCHARGE,";
                    SQL += " ISQUARTERCHARGE,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE ) ";
                    SQL += " VALUES (" + dt.Rows[0]["CODE"].ToString() + ",'Y','사업장 등록',0,'N','Y',";
                    SQL += "         'N',SYSTIMESTAMP,'1',SYSTIMESTAMP,'1','" + txtLicno.Text.Trim() + "') ";
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                }
            }

            // 3.테트스 데이타 복사 
            copy_HIC_OSHA_CONTRACT();
            copy_HIC_OSHA_ESTIMATE();
            copy_HIC_USERS(); // 테스트 사용자 등록
            copy_HIC_SITE_WORKER(); // 근로자명단
            copy_HIC_OSHA_REPORT_DOCTOR(); // 상태보고서(의사)
            copy_HIC_OSHA_REPORT_NURSE();  // 상태보고서(간호사)
            copy_HIC_OSHA_REPORT_ENGINEER();  // 상태보고서(산업위생)
            copy_HIC_OSHA_HEALTHCHECK();      // 근로자 건강상담
            copy_HIC_OSHA_CARD19();           // 위탁업무 수행일지
            copy_HIC_LTD_RESULT2();
            copy_HIC_LTD_RESULT3();
            copy_HIC_MACROWORD();             // 상용구
            Update_Gunro_Name();  //근로자명을 설씨로 변경             
            
            ComFunc.MsgBox("기본자료 생성 완료", "알림");
        }

        // 근로자명을 설씨로 강제 변경
        private void Update_Gunro_Name()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;
            string strName = "";

            // HIC_SITE_WORKER
            SQL = "SELECT * FROM HIC_SITE_WORKER ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID IN (" + strLtdno1 + "," + strLtdno2 + ") ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            for (int i=0; i< dt.Rows.Count; i++)
            {
                strName = dt.Rows[i]["NAME"].ToString().Trim();
                strName = "설" + VB.Right(strName, VB.Len(strName) - 1);

                SQL = "UPDATE HIC_SITE_WORKER SET Name='" + strName + "' ";
                SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
                SQL += "  AND SITEID=" + dt.Rows[i]["SITEID"].ToString().Trim() + " ";
                SQL += "  AND ID='" + dt.Rows[i]["ID"].ToString().Trim() + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            dt.Dispose();
            dt = null;

            // HIC_OSHA_HEALTHCHECK
            SQL = "SELECT * FROM HIC_OSHA_HEALTHCHECK ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID IN (" + strLtdno1 + "," + strLtdno2 + ") ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strName = dt.Rows[i]["NAME"].ToString().Trim();
                strName = "설" + VB.Right(strName, VB.Len(strName) - 1);

                SQL = "UPDATE HIC_OSHA_HEALTHCHECK SET Name='" + strName + "' ";
                SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
                SQL += "  AND SITE_ID=" + dt.Rows[i]["SITE_ID"].ToString().Trim() + " ";
                SQL += "  AND ID=" + dt.Rows[i]["ID"].ToString().Trim() + " ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            dt.Dispose();
            dt = null;

            // HIC_LTD_RESULT2
            SQL = "SELECT * FROM HIC_LTD_RESULT2 ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID IN (" + strLtdno1 + "," + strLtdno2 + ") ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strName = dt.Rows[i]["NAME"].ToString().Trim();
                strName = "설" + VB.Right(strName, VB.Len(strName) - 1);

                SQL = "UPDATE HIC_LTD_RESULT2 SET Name='" + strName + "' ";
                SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
                SQL += "  AND SITEID=" + dt.Rows[i]["SITEID"].ToString().Trim() + " ";
                SQL += "  AND ID=" + dt.Rows[i]["ID"].ToString().Trim() + " ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            dt.Dispose();
            dt = null;

            // HIC_LTD_RESULT3
            SQL = "SELECT * FROM HIC_LTD_RESULT3 ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID IN (" + strLtdno1 + "," + strLtdno2 + ") ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strName = dt.Rows[i]["NAME"].ToString().Trim();
                strName = "설" + VB.Right(strName, VB.Len(strName) - 1);

                SQL = "UPDATE HIC_LTD_RESULT3 SET Name='" + strName + "' ";
                SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
                SQL += "  AND SITEID=" + dt.Rows[i]["SITEID"].ToString().Trim() + " ";
                SQL += "  AND ID=" + dt.Rows[i]["ID"].ToString().Trim() + " ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            dt.Dispose();
            dt = null;

        }

        // 상용구
        private void copy_HIC_MACROWORD()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 상용구가 있으면 다시 작업 안함
            SQL = "SELECT * FROM HIC_MACROWORD ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                SQL = "INSERT INTO HIC_MACROWORD (ID,FORMNAME,CONTROL,TITLE,DISPSEQ,SUBTITLE,";
                SQL += " CONTENT2,CONTENT,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,FORMNAME,CONTROL,TITLE,DISPSEQ,SUBTITLE,CONTENT2,CONTENT,";
                SQL += " MODIFIED,'3',CREATED,'3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_MACROWORD ";
                SQL += "WHERE SWLICENSE='9555-88P5-8P33' "; //대한
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
            dt.Dispose();
            dt = null;
        }

        // 위탁업무 수행일지
        private void copy_HIC_OSHA_CARD19()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_CARD19 ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_CARD19 (ID,SITE_ID,ESTIMATE_ID,REGDATE,CERT,NAME,CONTENT,";
                SQL += " STATUS,OPINION,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno1 + "',ESTIMATE_ID,REGDATE,CERT,NAME,CONTENT,";
                SQL += " STATUS,OPINION,MODIFIED,'3',CREATED,'3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_CARD19 ";
                SQL += "WHERE SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_CARD19 ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_CARD19 (ID,SITE_ID,ESTIMATE_ID,REGDATE,CERT,NAME,CONTENT,";
                SQL += " STATUS,OPINION,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno2 + "',ESTIMATE_ID,REGDATE,CERT,NAME,CONTENT,";
                SQL += " STATUS,OPINION,MODIFIED,'3',CREATED,'3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_CARD19 ";
                SQL += "WHERE SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        // 근로자 건강상담
        private void copy_HIC_OSHA_HEALTHCHECK()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_HEALTHCHECK ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_HEALTHCHECK (SITE_ID,ID,WORKER_ID,NAME,DEPT, ";
                SQL += " GENDER,AGE,CONTENT,SUGGESTION,BPL,BPR,CHARTDATE,";
                SQL += " CHARTTIME,ISDELETED,MODIFIED,CREATED,BST,DAN,REPORT_ID,";
                SQL += " ISDOCTOR,WEIGHT,ALCHOL,SMOKE,BMI,EXAM,BREPORT_ID,SABUN,";
                SQL += " MODIFIEDUSER,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT '" + strLtdno1 + "',ID,WORKER_ID,NAME,DEPT,";
                SQL += " GENDER,AGE,CONTENT,SUGGESTION,BPL,BPR,CHARTDATE,";
                SQL += " CHARTTIME,ISDELETED,MODIFIED,CREATED,BST,DAN,REPORT_ID,";
                SQL += " ISDOCTOR,WEIGHT,ALCHOL,SMOKE,BMI,EXAM,BREPORT_ID,SABUN,";
                SQL += " '3','3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_HEALTHCHECK ";
                SQL += "WHERE SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SQL += "  AND WORKER_ID IN (SELECT ID FROM HIC_SITE_WORKER ";
                SQL += "      WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
                SQL += "        AND SITEID=" + strLtdno1 + ") ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_HEALTHCHECK ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_HEALTHCHECK (SITE_ID,ID,WORKER_ID,NAME,DEPT, ";
                SQL += " GENDER,AGE,CONTENT,SUGGESTION,BPL,BPR,CHARTDATE,";
                SQL += " CHARTTIME,ISDELETED,MODIFIED,CREATED,BST,DAN,REPORT_ID,";
                SQL += " ISDOCTOR,WEIGHT,ALCHOL,SMOKE,BMI,EXAM,BREPORT_ID,SABUN,";
                SQL += " MODIFIEDUSER,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT '" + strLtdno2 + "',ID,WORKER_ID,NAME,DEPT,";
                SQL += " GENDER,AGE,CONTENT,SUGGESTION,BPL,BPR,CHARTDATE,";
                SQL += " CHARTTIME,ISDELETED,MODIFIED,CREATED,BST,DAN,REPORT_ID,";
                SQL += " ISDOCTOR,WEIGHT,ALCHOL,SMOKE,BMI,EXAM,BREPORT_ID,SABUN,";
                SQL += " '3','3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_HEALTHCHECK ";
                SQL += "WHERE SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SQL += "  AND WORKER_ID IN (SELECT ID FROM HIC_SITE_WORKER ";
                SQL += "      WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
                SQL += "        AND SITEID=" + strLtdno2 + ") ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            //의사 상담자는 의사 사번 변경
            SQL = "UPDATE HIC_OSHA_HEALTHCHECK SET ";
            SQL += " MODIFIEDUSER='2',CREATEDUSER='2' ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID IN ('" + strLtdno1 + "','" + strLtdno2 + "') ";
            SQL += "  AND ISDOCTOR='Y' ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

        }

        // 상태보고서(산업위생)
        private void copy_HIC_OSHA_REPORT_ENGINEER()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_REPORT_ENGINEER ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_REPORT_ENGINEER (ID,SITE_ID,ESTIMATE_ID,VISITDATE,VISITRESERVEDATE, ";
                SQL += " WORKERCOUNT,WEMDATE,WEMHARMFULFACTORS,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WORKCONTENT,OSHADATE,OSHACONTENT,";
                SQL += " EDUTARGET,EDUPERSON,EDUAN,EDUTITLE,EDUTYPEJSON,EDUMETHODJSON,";
                SQL += " ENVCHECKJSON1,ENVCHECKJSON2,ENVCHECKJSON3,ISDELETED,";
                SQL += " MODIFIED,CREATED,SITEOWENER,OPINION,WEMEXPORSURE1,";
                SQL += " WEMDATE2,WEMDATEREMARK,APPROVE,";
                SQL += " SITEMANAGERNAME,SITEMANAGERGRADE,ENGINEERNAME,";
                SQL += " MODIFIEDUSER,CREATEDUSER,SITENAME,SWLICENSE ) ";
                SQL += "SELECT ID,'" + strLtdno1 + "',ESTIMATE_ID,VISITDATE,VISITRESERVEDATE,";
                SQL += " WORKERCOUNT,WEMDATE,WEMHARMFULFACTORS,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WORKCONTENT,OSHADATE,OSHACONTENT,";
                SQL += " EDUTARGET,EDUPERSON,EDUAN,EDUTITLE,EDUTYPEJSON,EDUMETHODJSON,";
                SQL += " ENVCHECKJSON1,ENVCHECKJSON2,ENVCHECKJSON3,ISDELETED,";
                SQL += " MODIFIED,CREATED,SITEOWENER,OPINION,WEMEXPORSURE1,";
                SQL += " WEMDATE2,WEMDATEREMARK,APPROVE,";
                SQL += " '강감찬','안전관리자','박위생',";
                SQL += " '4','4','연습회사','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_REPORT_ENGINEER ";
                SQL += "WHERE SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_REPORT_ENGINEER ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_REPORT_ENGINEER (ID,SITE_ID,ESTIMATE_ID,VISITDATE,VISITRESERVEDATE, ";
                SQL += " WORKERCOUNT,WEMDATE,WEMHARMFULFACTORS,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WORKCONTENT,OSHADATE,OSHACONTENT,";
                SQL += " EDUTARGET,EDUPERSON,EDUAN,EDUTITLE,EDUTYPEJSON,EDUMETHODJSON,";
                SQL += " ENVCHECKJSON1,ENVCHECKJSON2,ENVCHECKJSON3,ISDELETED,";
                SQL += " MODIFIED,CREATED,SITEOWENER,OPINION,WEMEXPORSURE1,";
                SQL += " WEMDATE2,WEMDATEREMARK,APPROVE,";
                SQL += " SITEMANAGERNAME,SITEMANAGERGRADE,ENGINEERNAME,";
                SQL += " MODIFIEDUSER,CREATEDUSER,SITENAME,SWLICENSE ) ";
                SQL += "SELECT ID,'" + strLtdno2 + "',ESTIMATE_ID,VISITDATE,VISITRESERVEDATE,";
                SQL += " WORKERCOUNT,WEMDATE,WEMHARMFULFACTORS,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WORKCONTENT,OSHADATE,OSHACONTENT,";
                SQL += " EDUTARGET,EDUPERSON,EDUAN,EDUTITLE,EDUTYPEJSON,EDUMETHODJSON,";
                SQL += " ENVCHECKJSON1,ENVCHECKJSON2,ENVCHECKJSON3,ISDELETED,";
                SQL += " MODIFIED,CREATED,SITEOWENER,OPINION,WEMEXPORSURE1,";
                SQL += " WEMDATE2,WEMDATEREMARK,APPROVE,";
                SQL += " '강감찬','안전관리자','박위생',";
                SQL += " '4','4','감자상회','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_REPORT_ENGINEER ";
                SQL += "WHERE SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        // 상태보고서(간호사)
        private void copy_HIC_OSHA_REPORT_NURSE()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_REPORT_NURSE ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_REPORT_NURSE (ID,SITE_ID,ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,SPECIALC2COUNT,";
                SQL += " SPECIALDNCOUNT,SPECIALCNCOUNT,WEMDATE,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WEMHARMFULFACTORS,PERFORMCONTENT,";
                SQL += " ISDELETED,EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,OPINION,APPROVE,";
                SQL += " SITEMANAGERNAME,SITEMANAGERGRADE,NURSENAME,SITENAME,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno1 + "',ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,SPECIALC2COUNT,";
                SQL += " SPECIALDNCOUNT,SPECIALCNCOUNT,WEMDATE,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WEMHARMFULFACTORS,PERFORMCONTENT,";
                SQL += " ISDELETED,EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,OPINION,APPROVE,";
                SQL += " '강감찬','안전관리자','황간호','연습회사',";
                SQL += " MODIFIED,'3',CREATED,'3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_REPORT_NURSE ";
                SQL += "WHERE SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_REPORT_NURSE ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_REPORT_NURSE (ID,SITE_ID,ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,SPECIALC2COUNT,";
                SQL += " SPECIALDNCOUNT,SPECIALCNCOUNT,WEMDATE,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WEMHARMFULFACTORS,PERFORMCONTENT,";
                SQL += " ISDELETED,EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,OPINION,APPROVE,";
                SQL += " SITEMANAGERNAME,SITEMANAGERGRADE,NURSENAME,SITENAME,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno2 + "',ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,SPECIALC2COUNT,";
                SQL += " SPECIALDNCOUNT,SPECIALCNCOUNT,WEMDATE,WEMEXPORSURE,";
                SQL += " WEMEXPORSUREREMARK,WEMHARMFULFACTORS,PERFORMCONTENT,";
                SQL += " ISDELETED,EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,OPINION,APPROVE,";
                SQL += " '강감찬','안전관리자','황간호','감자상회',";
                SQL += " MODIFIED,'3',CREATED,'3','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_REPORT_NURSE ";
                SQL += "WHERE SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        // 상태보고서(의사)
        private void copy_HIC_OSHA_REPORT_DOCTOR()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_REPORT_DOCTOR ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_REPORT_DOCTOR (ID,SITE_ID,ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,";
                SQL += " SPECIALC2COUNT,SPECIALDNCOUNT,SPECIALCNCOUNT,";
                SQL += " WEMDATE,WEMEXPORSURE,WEMEXPORSUREREMARK,";
                SQL += " WEMHARMFULFACTORS,PERFORMCONTENT,SITEMANAGERNAME,";
                SQL += " SITEMANAGERGRADE,DOCTORNAME,ISDELETED,OPINION,";
                SQL += " EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,APPROVE,SITENAME,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno1 + "',ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,";
                SQL += " SPECIALC2COUNT,SPECIALDNCOUNT,SPECIALCNCOUNT,";
                SQL += " WEMDATE,WEMEXPORSURE,WEMEXPORSUREREMARK,";
                SQL += " WEMHARMFULFACTORS,PERFORMCONTENT,'강감찬',";
                SQL += " '안전관리자','박의사',ISDELETED,OPINION,";
                SQL += " EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,APPROVE,'연습회사',";
                SQL += " MODIFIED,'2',CREATED,'2','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_REPORT_DOCTOR ";
                SQL += "WHERE SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_REPORT_DOCTOR ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_REPORT_DOCTOR (ID,SITE_ID,ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,";
                SQL += " SPECIALC2COUNT,SPECIALDNCOUNT,SPECIALCNCOUNT,";
                SQL += " WEMDATE,WEMEXPORSURE,WEMEXPORSUREREMARK,";
                SQL += " WEMHARMFULFACTORS,PERFORMCONTENT,SITEMANAGERNAME,";
                SQL += " SITEMANAGERGRADE,DOCTORNAME,ISDELETED,OPINION,";
                SQL += " EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,APPROVE,SITENAME,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno2 + "',ESTIMATE_ID,CURRENTWORKERCOUNT,VISITDATE,";
                SQL += " VISITRESERVEDATE,NEWWORKERCOUNT,RETIREWORKERCOUNT,";
                SQL += " CHANGEWORKERCOUNT,ACCIDENTWORKERCOUNT,DEADWORKERCOUNT,";
                SQL += " INJURYWORKERCOUNT,BIZDISEASEWORKERCOUNT,GENERALHEALTHCHECKDATE,";
                SQL += " SPECIALHEALTHCHECKDATE,GENERALD2COUNT,GENERALC2COUNT,";
                SQL += " SPECIALD1COUNT,SPECIALC1COUNT,SPECIALD2COUNT,";
                SQL += " SPECIALC2COUNT,SPECIALDNCOUNT,SPECIALCNCOUNT,";
                SQL += " WEMDATE,WEMEXPORSURE,WEMEXPORSUREREMARK,";
                SQL += " WEMHARMFULFACTORS,PERFORMCONTENT,'강감찬',";
                SQL += " '안전관리자','박의사',ISDELETED,OPINION,";
                SQL += " EXTDATA,WEMEXPORSURE2,GENERALTOTALCOUNT,";
                SQL += " SPECIALTOTALCOUNT,DEPTNAME,APPROVE,'감자상회',";
                SQL += " MODIFIED,'2',CREATED,'2','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_REPORT_DOCTOR ";
                SQL += "WHERE SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        // 상담을 한번이상 한 근로자만 가져옴
        private void copy_HIC_SITE_WORKER()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_SITE_WORKER ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_SITE_WORKER (SITEID,NAME,WORKER_ROLE,DEPT,TEL,HP,EMAIL,END_DATE,";
                SQL += " ISRETIRE,ISDELETED,JUMIN,PTNO,IPSADATE,PANO,ID,ISMANAGEOSHA,";
                SQL += " SABUN,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT '" + strLtdno1 + "',NAME,WORKER_ROLE,DEPT,'054-123-1234','010-000-0000','test#naver.com',END_DATE,";
                SQL += " ISRETIRE,ISDELETED,JUMIN,PTNO,IPSADATE,PANO,ID,ISMANAGEOSHA,";
                SQL += " SABUN,MODIFIED,'1',CREATED,'1','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_SITE_WORKER ";
                SQL += "WHERE SITEID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SQL += "  AND ID IN (SELECT WORKER_ID FROM HIC_OSHA_HEALTHCHECK ";
                SQL += "      WHERE SITEID=44873 "; //선안
                SQL += "        AND ISDELETED='N' ";
                SQL += "        AND (NAME LIKE '김%' OR NAME LIKE '박%') ";
                //SQL += "        AND ISDOCTOR='Y' ";
                SQL += "        AND SWLICENSE='9555-88P5-8P33') "; //대한

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_SITE_WORKER ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_SITE_WORKER (SITEID,NAME,WORKER_ROLE,DEPT,TEL,HP,EMAIL,END_DATE,";
                SQL += " ISRETIRE,ISDELETED,JUMIN,PTNO,IPSADATE,PANO,ID,ISMANAGEOSHA,";
                SQL += " SABUN,MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,SWLICENSE) ";
                SQL += "SELECT '" + strLtdno2 + "',NAME,WORKER_ROLE,DEPT,'054-123-1234','010-000-0000','test#naver.com',END_DATE,";
                SQL += " ISRETIRE,ISDELETED,JUMIN,PTNO,IPSADATE,PANO,ID,ISMANAGEOSHA,";
                SQL += " SABUN,MODIFIED,'1',CREATED,'1','" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_SITE_WORKER ";
                SQL += "WHERE SITEID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SQL += "  AND ID IN (SELECT WORKER_ID FROM HIC_OSHA_HEALTHCHECK ";
                SQL += "      WHERE SITEID=44909 "; //선화이엔지
                SQL += "        AND ISDELETED='N' ";
                SQL += "        AND (NAME LIKE '김%' OR NAME LIKE '박%') ";
                //SQL += "        AND ISDOCTOR='Y' ";
                SQL += "        AND SWLICENSE='9555-88P5-8P33') "; //대한
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        // 테스트 사용자 등록
        private void copy_HIC_USERS()
        {
            string SQL = "";
            string SqlErr = "";
            string strNewPass = "";
            string strJikmu = "YYYNNNNNNNNNNNN";
            DataTable dt = null;
            int intRowAffected = 0;

            string strList = "2;박의사;DOCTOR{}3;황간호;NURSE{}4;박위생;ENGINEER";
            string strUser = "";

            strNewPass = clsAES.AES("123aq!");

            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    strUser = VB.Pstr(strList, "{}", i);

                    SQL = "SELECT * FROM HIC_USERS ";
                    SQL = SQL + ComNum.VBLF + "WHERE SWLICENSE = '" + txtLicno.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND USERID = '" + VB.Pstr(strUser, ";", 1) + "' ";
                    
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (dt.Rows.Count == 0)
                    {
                        SQL =  " INSERT INTO HIC_USERS ";
                        SQL += " (SWLICENSE, USERID, NAME, DEPT, ROLE, INDATE, TESADATE,";
                        SQL += "  ISACTIVE, ISDELETED, JIKMU, PASSHASH256,CERTNO,SEQ_WORD,";
                        SQL += "  LTDUSER,MODIFIED, MODIFIEDUSER, CREATED, CREATEDUSER) ";
                        SQL += " VALUES ('" + txtLicno.Text.Trim() + "','";
                        SQL += VB.Pstr(strUser, ";", 1) + "','" + VB.Pstr(strUser, ";", 2) + "',";
                        SQL += "'OSHA','" + VB.Pstr(strUser, ";", 3) + "','";
                        SQL += dptSDate.Value.ToString("yyyy-MM-dd") + "','','Y','N','" + strJikmu + "','";
                        SQL += strNewPass + "','','','',SYSDATE,'1',SYSDATE,'1') ";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                }
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

        private void copy_HIC_OSHA_ESTIMATE()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_ESTIMATE ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND OSHA_SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_ESTIMATE (ID,OSHA_SITE_ID,ESTIMATEDATE,STARTDATE,WORKERTOTALCOUNT,";
                SQL += " OFFICIALFEE,SITEFEE,MONTHLYFEE,FEETYPE,PRINTDATE,SENDMAILDATE,";
                SQL += " REMARK,EXCELPATH,ISDELETED,MODIFIED,MODIFIEDUSER,";
                SQL += " CREATED,CREATEDUSER,BLUEMALE,BLUEFEMALE,";
                SQL += " WHITEMALE,WHITEFEMALE,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno1 + "',ESTIMATEDATE,STARTDATE,WORKERTOTALCOUNT,";
                SQL += " OFFICIALFEE,SITEFEE,MONTHLYFEE,FEETYPE,PRINTDATE,SENDMAILDATE,";
                SQL += " REMARK,EXCELPATH,ISDELETED,MODIFIED,'1',";
                SQL += " CREATED,'1',BLUEMALE,BLUEFEMALE,";
                SQL += " WHITEMALE,WHITEFEMALE,'" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_ESTIMATE ";
                SQL += "WHERE OSHA_SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_ESTIMATE ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND OSHA_SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_ESTIMATE (ID,OSHA_SITE_ID,ESTIMATEDATE,STARTDATE,WORKERTOTALCOUNT,";
                SQL += " OFFICIALFEE,SITEFEE,MONTHLYFEE,FEETYPE,PRINTDATE,SENDMAILDATE,";
                SQL += " REMARK,EXCELPATH,ISDELETED,MODIFIED,MODIFIEDUSER,";
                SQL += " CREATED,CREATEDUSER,BLUEMALE,BLUEFEMALE,";
                SQL += " WHITEMALE,WHITEFEMALE,SWLICENSE) ";
                SQL += "SELECT ID,'" + strLtdno2 + "',ESTIMATEDATE,STARTDATE,WORKERTOTALCOUNT,";
                SQL += " OFFICIALFEE,SITEFEE,MONTHLYFEE,FEETYPE,PRINTDATE,SENDMAILDATE,";
                SQL += " REMARK,EXCELPATH,ISDELETED,MODIFIED,'1',";
                SQL += " CREATED,'1',BLUEMALE,BLUEFEMALE,";
                SQL += " WHITEMALE,WHITEFEMALE,'" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_ESTIMATE ";
                SQL += "WHERE OSHA_SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        private void copy_HIC_OSHA_CONTRACT()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            // 연습회사
            SQL = "SELECT * FROM HIC_OSHA_CONTRACT ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND OSHA_SITE_ID=" + strLtdno1 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_CONTRACT (ESTIMATE_ID,OSHA_SITE_ID,CONTRACTDATE,SPECIALCONTRACT,WORKERTOTALCOUNT,";
                SQL += " WORKERWHITEMALECOUNT,WORKERWHITEFEMALECOUNT,WORKERBLUEMALECOUNT,";
                SQL += " WORKERBLUEFEMALECOUNT,MANAGEWORKERCOUNT,";
                SQL += " MANAGEDOCTOR,MANAGEDOCTORSTARTDATE,MANAGEDOCTORCOUNT,";
                SQL += " MANAGENURSE,MANAGENURSESTARTDATE,MANAGENURSECOUNT,";
                SQL += " MANAGEENGINEER,MANAGEENGINEERSTARTDATE,MANAGEENGINEERCOUNT,";
                SQL += " VISITWEEK,VISITDAY,COMMISSION,DECLAREDAY,CONTRACTSTARTDATE,";
                SQL += " CONTRACTENDDATE,POSITION,ISROTATION,ISPRODUCTTYPE,";
                SQL += " ISLABOR,BUILDINGTYPE,WORKSTARTTIME,WORKENDTIME,WORKMEETTIME,";
                SQL += " WORKROTATIONTIME,WORKLUANCHTIME,WORKRESTTIME,WORKEDUTIME,";
                SQL += " WORKETCTIME,ISCONTRACT,ISWEM,ISWEMDATA,ISCOMMITTEE,ISSKELETON,";
                SQL += " ISSKELETONDATE,ISSPACEPROGRAM,ISSPACEPROGRAMDATE,ISEARPROGRAM,";
                SQL += " ISEARPROGRAMDATE,ISSTRESS,ISSTRESSDATE,ISBRAINTEST,";
                SQL += " ISBRAINTESTDATE,ISSPECIAL,ISSPECIALDATA,ISDELETED,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,REMARK,TERMINATEDATE,";
                SQL += " VISITPLACE,SWLICENSE) ";
                SQL += "SELECT ESTIMATE_ID,'" + strLtdno1 + "',CONTRACTDATE,SPECIALCONTRACT,WORKERTOTALCOUNT,";
                SQL += " WORKERWHITEMALECOUNT,WORKERWHITEFEMALECOUNT,WORKERBLUEMALECOUNT,";
                SQL += " WORKERBLUEFEMALECOUNT,MANAGEWORKERCOUNT,";
                SQL += " '2',MANAGEDOCTORSTARTDATE,MANAGEDOCTORCOUNT,";
                SQL += " '3',MANAGENURSESTARTDATE,MANAGENURSECOUNT,";
                SQL += " '4',MANAGEENGINEERSTARTDATE,MANAGEENGINEERCOUNT,";
                SQL += " VISITWEEK,VISITDAY,COMMISSION,DECLAREDAY,CONTRACTSTARTDATE,";
                SQL += " CONTRACTENDDATE,POSITION,ISROTATION,ISPRODUCTTYPE,";
                SQL += " ISLABOR,BUILDINGTYPE,WORKSTARTTIME,WORKENDTIME,WORKMEETTIME,";
                SQL += " WORKROTATIONTIME,WORKLUANCHTIME,WORKRESTTIME,WORKEDUTIME,";
                SQL += " WORKETCTIME,ISCONTRACT,ISWEM,ISWEMDATA,ISCOMMITTEE,ISSKELETON,";
                SQL += " ISSKELETONDATE,ISSPACEPROGRAM,ISSPACEPROGRAMDATE,ISEARPROGRAM,";
                SQL += " ISEARPROGRAMDATE,ISSTRESS,ISSTRESSDATE,ISBRAINTEST,";
                SQL += " ISBRAINTESTDATE,ISSPECIAL,ISSPECIALDATA,ISDELETED,";
                SQL += " MODIFIED,'1',CREATED,'1','',TERMINATEDATE,";
                SQL += " VISITPLACE,'" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_CONTRACT ";
                SQL += "WHERE OSHA_SITE_ID = 44873 "; //선안
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }

            // 감자상회
            SQL = "SELECT * FROM HIC_OSHA_CONTRACT ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND OSHA_SITE_ID=" + strLtdno2 + " ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                SQL = "INSERT INTO HIC_OSHA_CONTRACT (ESTIMATE_ID,OSHA_SITE_ID,CONTRACTDATE,SPECIALCONTRACT,WORKERTOTALCOUNT,";
                SQL += " WORKERWHITEMALECOUNT,WORKERWHITEFEMALECOUNT,WORKERBLUEMALECOUNT,";
                SQL += " WORKERBLUEFEMALECOUNT,MANAGEWORKERCOUNT,";
                SQL += " MANAGEDOCTOR,MANAGEDOCTORSTARTDATE,MANAGEDOCTORCOUNT,";
                SQL += " MANAGENURSE,MANAGENURSESTARTDATE,MANAGENURSECOUNT,";
                SQL += " MANAGEENGINEER,MANAGEENGINEERSTARTDATE,MANAGEENGINEERCOUNT,";
                SQL += " VISITWEEK,VISITDAY,COMMISSION,DECLAREDAY,CONTRACTSTARTDATE,";
                SQL += " CONTRACTENDDATE,POSITION,ISROTATION,ISPRODUCTTYPE,";
                SQL += " ISLABOR,BUILDINGTYPE,WORKSTARTTIME,WORKENDTIME,WORKMEETTIME,";
                SQL += " WORKROTATIONTIME,WORKLUANCHTIME,WORKRESTTIME,WORKEDUTIME,";
                SQL += " WORKETCTIME,ISCONTRACT,ISWEM,ISWEMDATA,ISCOMMITTEE,ISSKELETON,";
                SQL += " ISSKELETONDATE,ISSPACEPROGRAM,ISSPACEPROGRAMDATE,ISEARPROGRAM,";
                SQL += " ISEARPROGRAMDATE,ISSTRESS,ISSTRESSDATE,ISBRAINTEST,";
                SQL += " ISBRAINTESTDATE,ISSPECIAL,ISSPECIALDATA,ISDELETED,";
                SQL += " MODIFIED,MODIFIEDUSER,CREATED,CREATEDUSER,REMARK,TERMINATEDATE,";
                SQL += " VISITPLACE,SWLICENSE) ";
                SQL += "SELECT ESTIMATE_ID,'" + strLtdno2 + "',CONTRACTDATE,SPECIALCONTRACT,WORKERTOTALCOUNT,";
                SQL += " WORKERWHITEMALECOUNT,WORKERWHITEFEMALECOUNT,WORKERBLUEMALECOUNT,";
                SQL += " WORKERBLUEFEMALECOUNT,MANAGEWORKERCOUNT,";
                SQL += " '2',MANAGEDOCTORSTARTDATE,MANAGEDOCTORCOUNT,";
                SQL += " '3',MANAGENURSESTARTDATE,MANAGENURSECOUNT,";
                SQL += " '4',MANAGEENGINEERSTARTDATE,MANAGEENGINEERCOUNT,";
                SQL += " VISITWEEK,VISITDAY,COMMISSION,DECLAREDAY,CONTRACTSTARTDATE,";
                SQL += " CONTRACTENDDATE,POSITION,ISROTATION,ISPRODUCTTYPE,";
                SQL += " ISLABOR,BUILDINGTYPE,WORKSTARTTIME,WORKENDTIME,WORKMEETTIME,";
                SQL += " WORKROTATIONTIME,WORKLUANCHTIME,WORKRESTTIME,WORKEDUTIME,";
                SQL += " WORKETCTIME,ISCONTRACT,ISWEM,ISWEMDATA,ISCOMMITTEE,ISSKELETON,";
                SQL += " ISSKELETONDATE,ISSPACEPROGRAM,ISSPACEPROGRAMDATE,ISEARPROGRAM,";
                SQL += " ISEARPROGRAMDATE,ISSTRESS,ISSTRESSDATE,ISBRAINTEST,";
                SQL += " ISBRAINTESTDATE,ISSPECIAL,ISSPECIALDATA,ISDELETED,";
                SQL += " MODIFIED,'1',CREATED,'1','',TERMINATEDATE,";
                SQL += " VISITPLACE,'" + txtLicno.Text.Trim() + "' ";
                SQL += " FROM HIC_OSHA_CONTRACT ";
                SQL += "WHERE OSHA_SITE_ID = 44909 "; //선화이엔지
                SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
                SQL += "  AND ISDELETED='N' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }

        private void copy_HIC_LTD_RESULT2()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수

            SQL = "DELETE FROM HIC_LTD_RESULT2 ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID IN (" + strLtdno1 + "," + strLtdno2 + ") ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            // 연습회사
            SQL = "INSERT INTO HIC_LTD_RESULT2 (SITEID,ID,BUSE,NAME,BIRTH,YEAR,JINDATE,";
            SQL += " HOSNAME,AGE,SEX,RESULT,JOBSABUN,ENTTIME,SWLICENSE) ";
            SQL += "SELECT '" + strLtdno1 + "',ID,BUSE,NAME,BIRTH,YEAR,JINDATE,";
            SQL += " HOSNAME,AGE,SEX,RESULT,'1',ENTTIME,'" + txtLicno.Text.Trim() + "' ";
            SQL += " FROM HIC_LTD_RESULT2 ";
            SQL += "WHERE SITEID = 44873 "; //선안
            SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
            SQL += "  AND ID IN (SELECT ID FROM HIC_SITE_WORKER ";
            SQL += "              WHERE SITEID=" + strLtdno1 + " ";
            SQL += "                AND SWLICENSE='" + txtLicno.Text.Trim() + "') "; 
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            // 감자상회
            SQL = "INSERT INTO HIC_LTD_RESULT2 (SITEID,ID,BUSE,NAME,BIRTH,YEAR,JINDATE,";
            SQL += " HOSNAME,AGE,SEX,RESULT,JOBSABUN,ENTTIME,SWLICENSE) ";
            SQL += "SELECT '" + strLtdno2 + "',ID,BUSE,NAME,BIRTH,YEAR,JINDATE,";
            SQL += " HOSNAME,AGE,SEX,RESULT,'1',ENTTIME,'" + txtLicno.Text.Trim() + "' ";
            SQL += " FROM HIC_LTD_RESULT2 ";
            SQL += "WHERE SITEID = 44909 "; //선화이엔지
            SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
            SQL += "  AND ID IN (SELECT ID FROM HIC_SITE_WORKER ";
            SQL += "              WHERE SITEID=" + strLtdno2 + " ";
            SQL += "                AND SWLICENSE='" + txtLicno.Text.Trim() + "') ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

        }

        private void copy_HIC_LTD_RESULT3()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수

            SQL = "DELETE FROM HIC_LTD_RESULT3 ";
            SQL += "WHERE SWLICENSE='" + txtLicno.Text.Trim() + "' ";
            SQL += "  AND SITEID IN (" + strLtdno1 + "," + strLtdno2 + ") ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            // 연습회사
            SQL = "INSERT INTO HIC_LTD_RESULT3 (SITEID,YEAR,JONG,BANGI,ID,GONGJENG,NAME,";
            SQL += " BIRTH,JINDATE,SEX,AGE,GUNSOK,YUHE,JIPYO,GGUBUN,SOGEN,SAHU,UPMU,";
            SQL += " JOBSABUN,ENTTIME,SWLICENSE) ";
            SQL += "SELECT '" + strLtdno1 + "',YEAR,JONG,BANGI,ID,GONGJENG,NAME,";
            SQL += " BIRTH,JINDATE,SEX,AGE,GUNSOK,YUHE,JIPYO,GGUBUN,SOGEN,SAHU,UPMU,";
            SQL += " '1',ENTTIME,'" + txtLicno.Text.Trim() + "' ";
            SQL += " FROM HIC_LTD_RESULT3 ";
            SQL += "WHERE SITEID = 44873 "; //선안
            SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
            SQL += "  AND ID IN (SELECT ID FROM HIC_SITE_WORKER ";
            SQL += "              WHERE SITEID=" + strLtdno1 + " ";
            SQL += "                AND SWLICENSE='" + txtLicno.Text.Trim() + "') ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

            // 감자상회
            SQL = "INSERT INTO HIC_LTD_RESULT3 (SITEID,YEAR,JONG,BANGI,ID,GONGJENG,NAME,";
            SQL += " BIRTH,JINDATE,SEX,AGE,GUNSOK,YUHE,JIPYO,GGUBUN,SOGEN,SAHU,UPMU,";
            SQL += " JOBSABUN,ENTTIME,SWLICENSE) ";
            SQL += "SELECT '" + strLtdno2 + "',YEAR,JONG,BANGI,ID,GONGJENG,NAME,";
            SQL += " BIRTH,JINDATE,SEX,AGE,GUNSOK,YUHE,JIPYO,GGUBUN,SOGEN,SAHU,UPMU,";
            SQL += " '1',ENTTIME,'" + txtLicno.Text.Trim() + "' ";
            SQL += " FROM HIC_LTD_RESULT3 ";
            SQL += "WHERE SITEID = 44909 "; //선화이엔지
            SQL += "  AND SWLICENSE='9555-88P5-8P33' "; //대한
            SQL += "  AND ID IN (SELECT ID FROM HIC_SITE_WORKER ";
            SQL += "              WHERE SITEID=" + strLtdno2 + " ";
            SQL += "                AND SWLICENSE='" + txtLicno.Text.Trim() + "') ";
            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

        }

        private void BtnSearch_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            bool SqlErr;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "ALTER TABLE LICMST ADD SMTP_Setup VARCHAR(200)";
                SqlErr = clsDbMySql.ExecuteNonQuery(SQL);

                if (SqlErr == false)
                {
                    ComFunc.MsgBox("테이블 칼럼추가 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("테이블 칼럼 추가되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void 전체자료삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            string strLIC = "";
            string strList1 = "";
            string strTable = "";
            long nTableCnt = 0;

            //정식사이트는 삭제 불가
            strLIC = txtLicno.Text.Trim();
            if (strLIC == "9555-88P5-8P33") { ComFunc.MsgBox("이 라이선스는 정식사용 Data임으로 삭제 불가함."); return; }
            if (strLIC == "") { ComFunc.MsgBox("라이선스번호가 공란입니다."); return; }

            if (ComFunc.MsgBoxQ(strLIC + " 라이선스의 전체 자료를 삭제하시겠습니까") == DialogResult.No) return;

            //전체 삭제할 테이블 목록을 설정
            strList1 = "";
            strList1 += "HIC_USERS{}";
            strList1 += "HIC_CODES{}";
            strList1 += "HIC_LTD{}";
            strList1 += "HIC_OSHA_SITE{}";
            strList1 += "HIC_LTD_RESULT2{}";
            strList1 += "HIC_LTD_RESULT3{}";
            strList1 += "HIC_OSHA_CONTRACT{}";
            strList1 += "HIC_OSHA_CONTRACT_MANAGER{}";
            strList1 += "HIC_OSHA_ESTIMATE{}";
            strList1 += "HIC_SITE_WORKER{}";
            strList1 += "HIC_OSHA_REPORT_DOCTOR{}";
            strList1 += "HIC_OSHA_REPORT_NURSE{}";
            strList1 += "HIC_OSHA_REPORT_ENGINEER{}";
            strList1 += "HIC_OSHA_HEALTHCHECK{}";
            strList1 += "HIC_OSHA_CARD19{}";
            strList1 += "HIC_MACROWORD{}";
            
            nTableCnt = VB.L(strList1, "{}");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (int i = 1; i <= nTableCnt; i++)
                {
                    strTable = VB.Pstr(strList1, "{}", i);
                    if (strTable != "")
                    {
                        SQL =  "DELETE FROM " + strTable + " ";
                        SQL += " WHERE SWLICENSE = '" + strLIC + "' ";
                        if (strTable=="HIC_USERS") SQL += " AND USERID<>'1' "; //관리자비번 삭제금지
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(strTable + " 삭제 실패", "알림");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                ComFunc.MsgBox("삭제되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void 연습자료만삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            string strLIC = "";
            string strLTD = "";
            string strList1 = "";
            string strTable = "";
            long nTableCnt = 0;

            //정식사이트는 삭제 불가
            strLIC = txtLicno.Text.Trim();
            if (strLIC == "9555-88P5-8P33") { ComFunc.MsgBox("이 라이선스는 정식사용 Data임으로 삭제 불가함."); return; }
            if (strLIC == "") { ComFunc.MsgBox("라이선스번호가 공란입니다."); return; }

            // 연습 거래처 목록을 읽어 변수에 저장
            SQL = "SELECT CODE FROM HIC_LTD WHERE SWLICENSE='" + txtLicno.Text.Trim() + "'";
            SQL += " AND SANGHO IN ('연습회사','감자상회') ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            strLTD = "";
            if (dt.Rows.Count > 0)
            {
                for (int i=0;i< dt.Rows.Count; i++)
                {
                    strLTD += "'" + dt.Rows[i]["CODE"].ToString() + "',";
                }
                if (VB.Right(strLTD, 1) == ",") strLTD = VB.Left(strLTD, VB.Len(strLTD) - 1);
            }

            //연습자료 회사정보가 없으면
            if (strLTD == "")
            {
                ComFunc.MsgBox("삭제할 연습자료가 없습니다.", "알림");
                Cursor.Current = Cursors.Default;
                return;
            }

            //연습자료 삭제할 테이블 목록을 설정
            strList1 = "";
            strList1 += "HIC_USERS{}";
            strList1 += "HIC_LTD{}";
            strList1 += "HIC_OSHA_SITE{}";
            strList1 += "HIC_LTD_RESULT2{}";
            strList1 += "HIC_LTD_RESULT3{}";
            strList1 += "HIC_OSHA_CONTRACT{}";
            strList1 += "HIC_OSHA_ESTIMATE{}";
            strList1 += "HIC_SITE_WORKER{}";
            strList1 += "HIC_OSHA_REPORT_DOCTOR{}";
            strList1 += "HIC_OSHA_REPORT_NURSE{}";
            strList1 += "HIC_OSHA_REPORT_ENGINEER{}";
            strList1 += "HIC_OSHA_HEALTHCHECK{}";
            strList1 += "HIC_OSHA_CARD19{}";

            nTableCnt = VB.L(strList1, "{}");

            if (ComFunc.MsgBoxQ(strLIC + " 라이선스의 연습 자료를 삭제하시겠습니까") == DialogResult.No) return;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (int i = 1; i <= nTableCnt; i++)
                {
                    strTable = VB.Pstr(strList1, "{}", i);
                    if (strTable != "")
                    {
                        SQL = "DELETE FROM " + strTable + " ";
                        SQL += " WHERE SWLICENSE = '" + strLIC + "' ";
                        if (strTable == "HIC_USERS") SQL += " AND USERID IN ('2','3') ";
                        if (strTable == "HIC_LTD") SQL += " AND CODE IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_SITE") SQL += " AND ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_ESTIMATE") SQL += " AND OSHA_SITE_ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_CONTRACT") SQL += " AND OSHA_SITE_ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_LTD_RESULT2") SQL += " AND SITEID IN (" + strLTD + ") ";
                        if (strTable == "HIC_LTD_RESULT3") SQL += " AND SITEID IN (" + strLTD + ") ";
                        if (strTable == "HIC_SITE_WORKER") SQL += " AND SITEID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_REPORT_DOCTOR") SQL += " AND SITE_ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_REPORT_NURSE") SQL += " AND SITE_ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_REPORT_ENGINEER") SQL += " AND SITE_ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_HEALTHCHECK") SQL += " AND SITE_ID IN (" + strLTD + ") ";
                        if (strTable == "HIC_OSHA_CARD19") SQL += " AND SITE_ID IN (" + strLTD + ") ";

                        //AND 조건이 누락되면 실행금지
                        if (VB.InStr(SQL," AND ")==0)
                        {
                            ComFunc.MsgBox(strTable + " AND 조건이 누락되어 삭제 실패", "알림");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(strTable + " 연습자료 삭제 실패", "알림");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                SQL = "DELETE FROM HIC_USERS ";
                SQL += "WHERE SWLICENSE = '" + strLIC + "' ";
                SQL += "  AND USERID IN ('2','3','4') ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("HIC_USERS 연습자료 삭제 실패", "알림");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ComFunc.MsgBox("연습자료가 삭제되었습니다.", "알림");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
