using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmTransRecordingInfo : Form
    {
        EmrPatient pAcp = null;

        /// <summary>
        /// Emr폼
        /// </summary>
        Form form = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pForm">부모 폼</param>
        public frmTransRecordingInfo(Form pForm, EmrPatient emrPatient)
        {
            form = pForm;
            pAcp = emrPatient;
            InitializeComponent();
        }

        private void frmTransRecordingInfo_Load(object sender, EventArgs e)
        {
            READ_DIAGNOSIS();
            READ_NUR_DIAGNOSIS();
            Get_OpInfo();

            READ_ALLERGY();
            READ_INFECTION();
            READ_NURSING();

            rdoFall2.Checked = true;

            string SysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).ToShortDateString();

            rdoFall2.Checked = true;
            if (READ_WARNING_FALL(pAcp.ptNo, SysDate, "", pAcp.age) != "")
            {
                rdoFall1.Checked = true;
            }

            rdoScale2.Checked = true;
            if (READ_WARNING_BRADEN(pAcp.ptNo, SysDate, "", pAcp.age, pAcp.ward) != "")
            {
                rdoScale1.Checked = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void READ_DIAGNOSIS()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT B.ILLNAMEE, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IILLS A INNER JOIN KOSMOS_PMPA.BAS_ILLS B  ";
                SQL = SQL + ComNum.VBLF + "     ON A.ILLCODE = B.ILLCODE ";
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.ENTDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SEQNO ASC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtDiag.Text = dt.Rows[0]["ILLNAMEE"].ToString().Trim();
                }
                else
                {
                    txtDiag.Text = READ_NUR_DIAGNOSIS();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_NUR_DIAGNOSIS()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT DIAGNOSIS ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY INDATE ASC ";


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void Get_OpInfo()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            txtOPTitle.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT OPCODE, OPTITLE, OPDATE, FLAG ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.ORAN_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(OPDATE) >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND  (OPETIME IS NOT NULL OR FLAG = '2') ";
                SQL = SQL + ComNum.VBLF + "  AND OPBUN NOT IN ('A','B')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                SQL = SQL + ComNum.VBLF + "    ORDER BY OPDATE DESC ";

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtOPTitle.Text = dt.Rows[0]["OPTITLE"].ToString().Trim();
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

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_ALLERGY()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            string strALLERGY = "";

            strALLERGY = "";

            txtAllMemo.Text = "";
            rdoAll1.Checked = false;
            rdoAll2.Checked = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_ALLERGY_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    strALLERGY = dt.Rows[i]["REMARK"].ToString().Trim();
                }
                else if (dt.Rows.Count > 1)
                {
                    strALLERGY = strALLERGY + "," + dt.Rows[i]["REMARK"].ToString().Trim();
                }
                else
                {
                    strALLERGY = "없음";
                }

                dt.Dispose();
                dt = null;

                if (strALLERGY == "없음")
                {
                    rdoAll1.Checked = true;
                }
                else
                {
                    rdoAll2.Checked = true;
                    txtAllMemo.Text = strALLERGY;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_INFECTION()
        {
            rdoExfect1.Checked = false;
            rdoExfect2.Checked = false;
            txtExfectMemo.Text = "";

            OracleDataReader reader = null;
            StringBuilder strINFECT = new StringBuilder();

            try
            {

                string SQL = FormPatInfoQuery.Query_FormPatInfo_INFECT(pAcp);

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                }

                if (reader.HasRows && reader.Read())
                {
                    string strInfect = reader.GetValue(0).ToString().Trim();
                    if (strInfect.Substring(0, 1).Equals("1"))
                    {
                        strINFECT.Append("혈액주의, ");
                    }
                    else if (strInfect.Substring(1, 1).Equals("1"))
                    {
                        strINFECT.Append("접촉주의, ");
                    }
                    else if (strInfect.Substring(2, 1).Equals("1"))
                    {
                        strINFECT.Append("접촉(격리)주의, ");
                    }
                    else if (strInfect.Substring(3, 1).Equals("1"))
                    {
                        strINFECT.Append("공기주의, ");
                    }
                    else if (strInfect.Substring(4, 1).Equals("1"))
                    {
                        strINFECT.Append("비말주의, ");
                    }
                    else if (strInfect.Substring(5, 1).Equals("1"))
                    {
                        strINFECT.Append("보호격리, ");
                    }
                    else if (strInfect.Substring(6, 1).Equals("1"))
                    {
                        strINFECT.Append("해외경유자, ");
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }


            rdoExfect1.Checked = false;
            rdoExfect2.Checked = false;
            txtExfectMemo.Text = "";

            if (strINFECT.Length == 0)
            {
                rdoExfect1.Checked = true;
            }
            else
            {
                rdoExfect2.Checked = true;
              

                txtExfectMemo.Text = strINFECT.ToString().Trim() + " 감염";
            }
        }

        private void READ_NURSING()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string SysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            string strInDate = "";
            string strOutDate = "";
            string strMainDIs = "";
            string strIpwonDong = "";
            string strWriteDate = "";


            txtPatient.Text = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (pAcp.ward == "ER")
                {
                    SQL = "";
                    SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it11') A, ";
                    SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta1') B";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).AddDays(-1).ToString("yyyyMMdd") + "'";
                    SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + pAcp.medFrDate + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 1836";
                    SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";
                    SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE DESC ";
                    SQL = SQL + ComNum.VBLF + "";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                        strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                        strIpwonDong = dt.Rows[0]["B"].ToString().Trim();

                        txtPatient.Text = "주증상 : " + strMainDIs;

                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = " SELECT TO_CHAR(INDATE,'YYYYMMDD') INDATE, OUTDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND INDATE < TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND (OUTDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "','YYYY-MM-DD') OR (JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND OUTDATE IS NULL))";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                    strOutDate = VB.Replace(dt.Rows[0]["OUTDATE"].ToString().Trim(), "-", "");
                }

                dt.Dispose();
                dt = null;

                if (strInDate == "")
                {
                    ComFunc.MsgBox("내원정보가 없습니다.", "확인");
                    return;
                }

                if (string.IsNullOrWhiteSpace(strOutDate))
                {
                    strOutDate = SysDate;
                }

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it32') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta1') B";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 2311";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it34') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta1') B";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 2294";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it34') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta1') B";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 2295";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it34') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta1') B";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 2356";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it34') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta1') B";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 2305";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it6') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta7') B1,";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta8') B2,";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta9') B3";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 1799";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B1"].ToString().Trim();
                    strIpwonDong = strIpwonDong + ComNum.VBLF + dt.Rows[0]["B2"].ToString().Trim();
                    strIpwonDong = strIpwonDong + ComNum.VBLF + dt.Rows[0]["B3"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = " SELECT TO_DATE(A.CHARTDATE  || A.CHARTTIME,'YYYY-MM-DD HH24:MI:SS') CHARTDATE,  extractValue(B.chartxml, '//it6') A, ";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta7') B1,";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta8') B2,";
                SQL = SQL + ComNum.VBLF + " extractValue(B.chartxml, '//ta9') B3";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + "  WHERE A.CHARTDATE >= '" + strInDate + "'";
                SQL = SQL + ComNum.VBLF + "     AND A.CHARTDATE <= '" + strOutDate + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.PTNO = '" + pAcp.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "     AND A.FORMNO = 2069";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strWriteDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strMainDIs = dt.Rows[0]["A"].ToString().Trim();
                    strIpwonDong = dt.Rows[0]["B1"].ToString().Trim();
                    strIpwonDong = strIpwonDong + ComNum.VBLF + dt.Rows[0]["B2"].ToString().Trim();
                    strIpwonDong = strIpwonDong + ComNum.VBLF + dt.Rows[0]["B3"].ToString().Trim();

                    txtPatient.Text = "주증상 : " + strMainDIs;
                    txtPatient.Text = txtPatient.Text + ComNum.VBLF + "입원동기 : " + strIpwonDong;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_WARNING_FALL(string argPTNO, string argDate, string ArgIPDNO, string argAge, string ArgSTS = "")
        {
            string strFall = "";
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //ArgSTS 외래일경우 "외래" 값들어감
                //외래일경우 다시 쿼리
                if (ArgSTS == "외래" || ArgIPDNO == "")
                {
                    SQL = "";
                    SQL = "SELECT IPDNO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ArgIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }

                strFall = "";

                SQL = "";
                SQL = " SELECT WARDCODE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                            strFall = "OK";
                            break;
                        case "NR":
                        case "IQ":
                            strFall = "OK";
                            break;
                        default:
                            break;
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 70)
                    {
                        strFall = "OK";
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 7)
                    {
                        strFall = "OK";
                    }

                    if (VB.IsNumeric(argAge) == false)
                    {
                        argAge = dt.Rows[0]["AGE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (strFall == "OK")
                {
                    rtnVal = "낙상위험";
                    return rtnVal;
                }

                SQL = "";
                SQL = "  SELECT PANO, TOTAL, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + " AND TOTAL >= 51";
                SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                if (VB.Val(argAge) < 18)
                {
                    SQL = "  SELECT PANO, TOTAL, AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "   AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND TOTAL >= 12 ";
                    SQL = SQL + ComNum.VBLF + "   AND ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";
                }

                dt.Dispose();
                dt = null;

                if (strFall == "")
                {
                    SQL = "";
                    SQL = " SELECT IPDNO ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_FALL_WARNING";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND (WARNING1 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR WARNING2 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR WARNING3 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR WARNING4 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR WARNING5 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_01 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_02 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_03 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_04 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_05 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_06 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_07 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_08 = '1'";
                    SQL = SQL + ComNum.VBLF + "                  OR DRUG_08_ETC <> '')";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strFall = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strFall == "OK")
                {
                    rtnVal = "낙상위험";
                }

                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_WARNING_BRADEN(string argPTNO, string argDate, string ArgIPDNO, string argAge, string argWARD, string ArgDate2 = "")
        {
            string strBraden = "";
            string strGUBUN = "";
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ArgIPDNO == "")
                {
                    SQL = "";
                    SQL = "SELECT IPDNO, WARDCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ArgIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                        argWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    }
                    else
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (VB.IsNumeric(argAge) == false)
                {
                    SQL = "";
                    SQL = "SELECT AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        argAge = dt.Rows[0]["AGE"].ToString().Trim();
                    }
                    else
                    {
                        argAge = "";
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (argAge == "")
                {
                    return rtnVal;
                }

                if (argWARD == "NR" || argWARD == "ND" || argWARD == "IQ")
                {
                    strGUBUN = "신생아";
                }
                else if (VB.Val(argAge) < 5)
                {
                    strGUBUN = "소아";
                }
                else
                {
                    strGUBUN = "";
                }

                //if (argPTNO == "08619351")
                //{
                //    argPTNO = argPTNO;
                //}

                if (strGUBUN == "")
                {
                    SQL = "";
                    SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";

                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    }

                    SQL = SQL + ComNum.VBLF + "     AND A.TOTAL <= 18";
                    SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18 || VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else if (strGUBUN == "소아")
                {
                    SQL = "";
                    SQL = "SELECT TOTAL ";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_CHILD ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "    AND PANO = '" + argPTNO + "' ";

                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    }

                    SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                else if (strGUBUN == "신생아")
                {
                    SQL = "";
                    SQL = "SELECT TOTAL ";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_BABY ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";

                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    }

                    SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 20)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strBraden == "")
                {
                    SQL = "";
                    SQL = " SELECT *";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_WARNING ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND ( ";
                    SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                    SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                    SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                    SQL = SQL + ComNum.VBLF + "      )";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strBraden = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strBraden == "OK")
                {
                    rtnVal = "욕창위험";
                }

                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Control[] obj = form.Controls.Find("it1", true);
            if(obj.Length > 0)
            {
                obj[0].Text = txtDiag.Text.Trim();
            }

            obj = form.Controls.Find("it2", true);
            if (obj.Length > 0)
            {
                obj[0].Text = txtOPTitle.Text.Trim();
            }

            obj = form.Controls.Find("ir3", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoFall1.Checked;
            }

            obj = form.Controls.Find("ir4", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoFall2.Checked;
            }

            obj = form.Controls.Find("ir5", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoScale1.Checked;
            }

            obj = form.Controls.Find("ir6", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoScale2.Checked;
            }


            obj = form.Controls.Find("ir2", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoAll1.Checked;
            }

            obj = form.Controls.Find("ir1", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoAll2.Checked;
                if(rdoAll2.Checked)
                {
                    obj = form.Controls.Find("it13", true);
                    if (obj.Length > 0)
                    {
                        (obj[0]).Text = txtAllMemo.Text.Trim();
                    }
                }
            }
            

            obj = form.Controls.Find("ir7", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoExfect1.Checked;
            }

            obj = form.Controls.Find("ir8", true);
            if (obj.Length > 0)
            {
                (obj[0] as RadioButton).Checked = rdoExfect2.Checked;
                if (rdoExfect2.Checked)
                {
                    obj = form.Controls.Find("it14", true);
                    if (obj.Length > 0)
                    {
                        (obj[0]).Text = txtExfectMemo.Text.Trim();
                    }
                }
            }

            obj = form.Controls.Find("ta1", true);
            if (obj.Length > 0)
            {
                obj[0].Text = txtPatient.Text.Trim();
            }

            this.Close();
        }
    }
}
