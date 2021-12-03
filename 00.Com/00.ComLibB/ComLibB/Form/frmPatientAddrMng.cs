using ComBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmPatientAddrMng : Form
    {
        string strPano = "";

        public frmPatientAddrMng()
        {
            InitializeComponent();
        }

        public frmPatientAddrMng(string strPano)
        {
            InitializeComponent();
            this.strPano = strPano;
        }

        private void frmPatientAddrMng_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if (strPano == "") return;
            txtPtno.Text = strPano;
            GetSearchData();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("주소및 번호를 수정 하시겠습니까?", "환자 정보 수정", MessageBoxDefaultButton.Button1) == DialogResult.No) return;

            if (Save_Data() == false) return;

            Close();
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_PATIENT_HIS ( ";
                SQL = SQL + ComNum.VBLF + " PANO,SNAME,SEX,JUMIN1,JUMIN2,STARTDATE,LASTDATE,ZIPCODE1,ZIPCODE2,";
                SQL = SQL + ComNum.VBLF + " JUSO,JICODE,TEL,SABUN,EMBPRT,BI,PNAME,GWANGE,KIHO,GKIHO ,DEPTCODE,DRCODE ,";
                SQL = SQL + ComNum.VBLF + " GBSPC,GBGAMEK,JINILSU,JINAMT,TUYAKGWA,TUYAKMONTH,TUYAKJULDATE,TUYAKILSU, ";
                SQL = SQL + ComNum.VBLF + " BOHUN,REMARK,RELIGION,GBMSG,XRAYBARCODE,ARSCHK,BUNUP,BIRTH,GBBIRTH,EMAIL, ";
                SQL = SQL + ComNum.VBLF + " GBINFOR,JIKUP,HPHONE,GBJUGER,GBSMS,GBJUSO,BICHK,HPHONE2,JUSAMSG,EKGMSG,BIDATE,MISSINGCALL,AIFLU, ";
                SQL = SQL + ComNum.VBLF + "  TEL_CONFIRM,GBSMS_DRUG,GBINFO_DETAIL,GBINFOR2,ROAD,  ";
                SQL = SQL + ComNum.VBLF + " ROADDONG,JUMIN3,GBFOREIGNER,ENAME,CASHYN,GB_VIP,GB_VIP_REMARK,GB_VIP_SABUN,GB_VIP_DATE,ROADDETAIL,GB_VIP2,GB_VIP2_REAMRK,GB_SVIP, ";
                SQL = SQL + ComNum.VBLF + " WEBSEND,WEBSENDDATE,GBMERS,OBST,ZIPCODE3,BUILDNO,PT_REMARK,TEMPLE,C_NAME,GBCOUNTRY,GBGAMEKC,SNAME2,PNAME2,CHGDATE,CHGIDNUMBER) ";

                SQL = SQL + ComNum.VBLF + " SELECT PANO,SNAME,SEX,JUMIN1,JUMIN2,STARTDATE,LASTDATE,ZIPCODE1,ZIPCODE2,";
                SQL = SQL + ComNum.VBLF + " JUSO,JICODE,TEL,SABUN,EMBPRT,BI,PNAME,GWANGE,KIHO,GKIHO ,DEPTCODE,DRCODE ,";
                SQL = SQL + ComNum.VBLF + " GBSPC,GBGAMEK,JINILSU,JINAMT,TUYAKGWA,TUYAKMONTH,TUYAKJULDATE,TUYAKILSU, ";
                SQL = SQL + ComNum.VBLF + " BOHUN,REMARK,RELIGION,GBMSG,XRAYBARCODE,ARSCHK,BUNUP,BIRTH,GBBIRTH,EMAIL, ";
                SQL = SQL + ComNum.VBLF + " GBINFOR,JIKUP,HPHONE,GBJUGER,GBSMS,GBJUSO,BICHK,HPHONE2,JUSAMSG,EKGMSG,BIDATE,MISSINGCALL,AIFLU, ";
                SQL = SQL + ComNum.VBLF + " TEL_CONFIRM,GBSMS_DRUG,GBINFO_DETAIL,GBINFOR2,ROAD,  ";
                SQL = SQL + ComNum.VBLF + " ROADDONG,JUMIN3,GBFOREIGNER,ENAME,CASHYN,GB_VIP,GB_VIP_REMARK,GB_VIP_SABUN,GB_VIP_DATE,ROADDETAIL,GB_VIP2,GB_VIP2_REAMRK,GB_SVIP, ";
                SQL = SQL + ComNum.VBLF + " WEBSEND,WEBSENDDATE,GBMERS,OBST,ZIPCODE3,BUILDNO,PT_REMARK,TEMPLE,C_NAME,GBCOUNTRY,GBGAMEKC,SNAME2,PNAME2,SYSDATE, " + "'" + clsType.User.Sabun + "' AS IDNUMBER";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "    WHERE PANO = '" + strPano + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                #region 환자인적사항 변경 내역 백업
                ComFunc CF1 = new ComFunc();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                #endregion


                SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT SET ";
                if (txtBuildNo.Text.Trim() == "")
                {
                    SQL += ComNum.VBLF + "    ZipCode1  = '" + VB.Left(txtPostCode.Text, 3) + "', ";
                    SQL += ComNum.VBLF + "    ZipCode2  = '" + VB.Right(txtPostCode.Text, 3) + "', ";
                    SQL += ComNum.VBLF + "    Juso      = '" + txtJuso2.Text.Trim() + "'";
                    SQL += ComNum.VBLF + "    HPHONE    = '" + txtHPhone.Text.Trim() + "'";

                    dict.Add("ZIPCODE1", VB.Left(txtPostCode.Text, 3));
                    dict.Add("ZIPCODE2", VB.Right(txtPostCode.Text, 3));
                    dict.Add("JUSO", txtJuso2.Text.Trim());
                    dict.Add("HPHONE", txtHPhone.Text.Trim());
                    CF1.INSERT_BAS_PATIENT_HIS(txtPtno.Text.Trim(), dict);

                }
                else
                {
                    SQL += ComNum.VBLF + "    ZipCode1   = '', ";
                    SQL += ComNum.VBLF + "    ZipCode2   = '', ";
                    SQL += ComNum.VBLF + "    ZipCode3   = '" + txtPostCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    RoadDetail = '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    Juso       = '" + txtJuso1.Text.Trim() + " " + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    BuildNo    = '" + txtBuildNo.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "    HPHONE     = '" + txtHPhone.Text.Trim() + "'";

                    dict.Add("ZIPCODE3", txtPostCode.Text.Trim());
                    dict.Add("ROADDETAIL", txtJuso2.Text.Trim());
                    dict.Add("JUSO", txtJuso1.Text.Trim());
                    dict.Add("BUILDNO", txtBuildNo.Text.Trim());
                    dict.Add("HPHONE", txtHPhone.Text.Trim());
                    CF1.INSERT_BAS_PATIENT_HIS(txtPtno.Text.Trim(), dict);

                }
                SQL += ComNum.VBLF + "  WHERE Pano      = '" + txtPtno.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT BuildNo, RoadDetail, SNAME, JUSO, ZIPCODE1, ZIPCODE2, ZIPCODE3, HPHONE ";
                SQL += ComNum.VBLF + "FROM ADMIN.BAS_PATIENT";
                SQL += ComNum.VBLF + "WHERE PANO ='" + txtPtno.Text + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                txtBuildNo.Text = dt.Rows[0]["BuildNo"].ToString().Trim();
                txtHPhone.Text  = dt.Rows[0]["HPHONE"].ToString().Trim();

                txtPostCode.Text = txtBuildNo.Text.Trim() == "" ?
                dt.Rows[0]["ZIPCODE1"].ToString().Trim() + dt.Rows[0]["ZIPCODE2"].ToString().Trim() :
                dt.Rows[0]["ZIPCODE3"].ToString().Trim();

                if (txtBuildNo.Text.Trim() != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ZIPNAME1 || ' ' || ZIPNAME2 || ' ' || ZIPNAME3 AS HEADJUSO,";
                    SQL += ComNum.VBLF + "        ROADNAME, BUILDNAME, BUN1, BUN2 ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ZIPS_ROAD ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND BUILDNO = '" + txtBuildNo.Text + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        dt2.Dispose();
                        dt2 = null;
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        txtJuso1.Text = dt2.Rows[0]["HEADJUSO"].ToString().Trim() + " ";
                        txtJuso1.Text += dt2.Rows[0]["ROADNAME"].ToString().Trim() + " ";
                        txtJuso1.Text += dt2.Rows[0]["BUN1"].ToString().Trim() + " ";

                        if (VB.Val(dt2.Rows[0]["BUN2"].ToString().Trim()) > 0)
                        {
                            txtJuso1.Text += "-" + dt2.Rows[0]["BUN2"].ToString().Trim() + " ";
                        }

                        if (dt2.Rows[0]["BUILDNAME"].ToString().Trim() != "")
                        {
                            txtJuso1.Text += dt2.Rows[0]["BUILDNAME"].ToString().Trim();
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;

                    txtJuso2.Text = dt.Rows[0]["ROADDETAIL"].ToString().Trim();
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT MailJuso, GbDel ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND MailCode = '" + txtPostCode.Text + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("주소검색시 오류발생");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        txtJuso1.Text = dt2.Rows[0]["MailJuso"].ToString().Trim();
                        if (dt2.Rows[0]["GbDel"].ToString().Trim() == "*")
                        {
                            ComFunc.MsgBox("삭제된 우편번호를 가지고 있습니다. 주소를 확인하시기 바랍니다.", "주소확인");
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT Juso ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND PANO = '" + txtPtno.Text + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("주소검색시 오류발생");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt2.Rows.Count > 0)
                    {
                        txtJuso2.Text = dt2.Rows[0]["Juso"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                txtSNAME.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            frmSearchRoadAdd frm = new frmSearchRoadAdd();
            frm.rSetGstrValue += new frmSearchRoadAdd.SetGstrValue(ePost_value);
            frm.ShowDialog();
            frm.Dispose();
            frm = null;

            txtJuso2.Focus();
        }

        private void ePost_value(string strValue)
        {
            if (strValue == "") return;

            txtPostCode.Text = VB.Left(VB.Pstr(strValue, "|", 1), 3);
            txtPostCode.Text += VB.Mid(VB.Pstr(strValue, "|", 1), 4, 2);
            txtJuso1.Text = VB.Pstr(strValue, "|", 2).Trim();
            txtJuso2.Text = "";

            txtBuildNo.Text = VB.Pstr(strValue, "|", 5).Trim();
            txtJuso2.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
