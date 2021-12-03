using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComDbB;
using ComLibB.Properties;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmCertPoolEmrMain : Form
    {
        bool bolSTOP = false;
        int FnCntPic = 0;
        int FnTimerCnt = 0;
        public frmCertPoolEmrMain()
        {
            InitializeComponent();
        }

        private void frmCertPoolEmrMain_Load(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            txtEmrData.Text = "";
            txtHashData.Text = "";
            txtSignData.Text = "";
            txtCertData.Text = "";

            lblSDate.Text = "";
            lblFrDate.Text = "";
            LblTitle.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            lblSDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            bolSTOP = false;
            timer1.Enabled = true;
            timer2.Enabled = true;            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bolSTOP = true;
            LblTitle.Text = "전자인증 작업중지!!";

            timer1.Enabled = false;
            timer2.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            btnStop.PerformClick();

            this.Close();
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            ComFunc.ReadSysDate(clsDB.DbCon);
            FnTimerCnt = FnTimerCnt + 1;

            LblTitle.Text = "작업 대기중입니다...(" + FnTimerCnt + ")";

            if (FnTimerCnt >= 60)
            {
                timer1.Enabled = false;

                LblTitle.Text = "전자인증 작업중입니다.";

                FnTimerCnt = 0;
                ssView_Sheet1.RowCount = 0;

                //EMR 전자인증
                //AUTO_CERT_OLD_EMRXML(clsDB.DbCon);
                //투약기록지 전자인증
                //AUTO_CERT_OLD_EMRXML_TUYAK(clsDB.DbCon);
                //신규 EMR 전자인증

                AUTO_CERT_AEMRCHARTMST(clsDB.DbCon);
                //신규 EMR HIS 전자인증
                AUTO_CERT_AEMRCHARTMSTHIS(clsDB.DbCon);
                //전자동의서 전자인증
                //AUTO_CERT_AEASFORMDATA(clsDB.DbCon);


                if (chkMsg.Checked == true)
                {
                    ComFunc.MsgBox("전체인증후 메시지창 표시!!  작업대기중 !!");
                }

                if (bolSTOP == false)
                {
                    timer1.Enabled = true;
                }
            }
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            FnCntPic = FnCntPic + 1;
            if (FnCntPic > 5)
            {
                FnCntPic = 1;
            }

            if (FnCntPic == 1) pictureBox1.Image = Resources.pic_0;
            else if (FnCntPic == 2) pictureBox1.Image = Resources.pic_1;
            else if (FnCntPic == 3) pictureBox1.Image = Resources.pic_2;
            else if (FnCntPic == 4) pictureBox1.Image = Resources.pic_3;
            else if (FnCntPic == 5) pictureBox1.Image = Resources.pic_4;

            Application.DoEvents();
        }

        private void AUTO_CERT_OLD_EMRXML(PsmhDb pDbCon)
        {
            string strOK = "OK";
            string strError = string.Empty;
            string strName = "";
            string strToiday = "";
            
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double nCertno = 0;
            string strEMRData = string.Empty;
            string strHashData = string.Empty;
            string strSignData = string.Empty;
            string strCertData = string.Empty;
            string strRowid = string.Empty;
            string strDrSabun = string.Empty;
            string strPano = string.Empty;
            
            long nDay = 0;
            long nDay2 = 16;
            string strSDate = string.Empty;
            
            ComFunc.ReadSysDate(pDbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                for (nDay = 1; nDay <= 16; nDay++)
                {
                    nDay2 = nDay2 - 1;

                    if (nDay2 < -1) break;

                    strSDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(nDay2 * -1).ToString("yyyyMMdd");

                    //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                    SQL = "";
                    SQL += "SELECT A.EMRNO , B.PTNO, B.USEID, B.ROWID,  B.CHARTDATE,  B.FORMNO, B.CERTNO  \r";
                    SQL += "  FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B  \r";
                    if (chkDate.Checked == true)
                    {
                        SQL += "    WHERE  A.WRITEDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'  \r";
                        SQL += "      AND  A.WRITEDATE <= '" + dtpToDate.Value.ToString("yyyyMMdd") + "'  \r";
                    }
                    else
                    {
                        if (strSDate != "")
                        {
                            SQL += "    WHERE  A.WRITEDATE = '" + strSDate + "' \r";
                        }
                        else
                        {
                            SQL += "    WHERE  A.WRITEDATE >= TO_CHAR(SYSDATE - 15, 'YYYYMMDD')  \r";
                            SQL += "      AND  A.WRITEDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')  \r";
                        }
                    }
                    SQL += "     AND A.EMRNO = B.EMRNO  \r";
                    SQL += "   ORDER BY A.WRITEDATE  \r";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        //clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strOK = "OK";
                            strError = "";
                            strName = "";
                            strToiday = "";

                            //사용자 중지
                            if (bolSTOP == true)
                            {
                                return;
                            }

                            strRowid = dt.Rows[i]["ROWID"].ToString().Trim();
                            strPano = dt.Rows[i]["PTNO"].ToString().Trim();
                            strDrSabun = dt.Rows[i]["USEID"].ToString().Trim();

                            //'인증된것 제외함
                            if (VB.Val(dt.Rows[i]["CertNo"].ToString().Trim()) != 0)
                            {
                                strOK = "NO";
                            }

                            if (strOK == "OK")
                            {                                
                                clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                                clsDB.setBeginTran(pDbCon);

                                try
                                {                                    
                                    strEMRData = GetOldEmrXMLData(clsDB.DbCon, dt.Rows[i]["EMRNO"].ToString().Trim());
                                    if (string.IsNullOrEmpty(strEMRData) == true)
                                    {
                                        strOK = "NO";
                                        strError = "데이타삭제됨 : " + dt.Rows[i]["EMRNO"].ToString().Trim();
                                    }

                                    if (strOK.Equals("OK"))
                                    {
                                        // 전자인증
                                        strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.EMRXML, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                        if (strError != "OK")
                                        {
                                            strOK = "NO";
                                            strError = strError + " : " + strEMRData;
                                        }
                                    }

                                    if (strOK.Equals("OK"))
                                    {
                                        if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.EMRXML, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
                                        {
                                            strOK = "NO";
                                            strError = string.Concat("DB등록 오류", strEMRData);
                                        }
                                    }

                                    txtEmrData.Text = strEMRData;
                                    txtHashData.Text = strHashData;
                                    txtSignData.Text = strSignData;
                                    txtCertData.Text = strCertData;

                                    if (strOK.Equals("OK"))
                                    {
                                        clsDB.setCommitTran(pDbCon);                                        
                                    }
                                    else
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = GetOldEmrFormName(pDbCon, dt.Rows[i]["FORMNO"].ToString().Trim());
                                        
                                        if (strToiday != "")
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName + "[퇴사:" + strToiday + "]";
                                        }
                                        else
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName;
                                        }
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strError;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    dt.Dispose();
                                    dt = null;
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(ex.Message);
                                }
                            }

                            Application.DoEvents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;                
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void AUTO_CERT_OLD_EMRXML_TUYAK(PsmhDb pDbCon)
        {
            string strOK = "OK";
            string strError = string.Empty;
            string strName = "";
            string strToiday = "";
            string strCertPass = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double nCertno = 0;
            string strEMRData = string.Empty;
            string strHashData = string.Empty;
            string strSignData = string.Empty;
            string strCertData = string.Empty;
            string strRowid = string.Empty;
            string strDrSabun = string.Empty;
            string strPano = string.Empty;

            long nDay = 0;
            long nDay2 = 16;
            string strSDate = string.Empty;

            ComFunc.ReadSysDate(pDbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                for (nDay = 1; nDay <= 16; nDay++)
                {
                    nDay2 = nDay2 - 1;

                    if (nDay2 < -1) break;

                    strSDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(nDay2 * -1).ToString("yyyyMMdd");

                    //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                    SQL = "";
                    SQL += " SELECT EMRNO, PTNO, USEID, ROWID,  CHARTDATE,  FORMNO, CERTNO,  \r";
                    SQL += "        IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10 \r";
                    SQL += "   FROM KOSMOS_EMR.EMRXML_TUYAK   \r";
                    if (chkDate.Checked == true)
                    {
                        SQL += "     WHERE  WRITEDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'  \r";
                        SQL += "       AND  WRITEDATE <= '" + dtpToDate.Value.ToString("yyyyMMdd") + "'  \r";
                    }
                    else
                    {
                        if (strSDate != "")
                        {
                            SQL += "     WHERE  WRITEDATE = '" + strSDate + "' \r";
                        }
                        else
                        {
                            SQL += "     WHERE  WRITEDATE >= TO_CHAR(SYSDATE - 15, 'YYYYMMDD')  \r";
                            SQL += "       AND  WRITEDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')  \r";
                        }
                    }
                    SQL += "   ORDER BY WRITEDATE  \r";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        //clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strOK = "OK";
                            strError = "";
                            strName = "";
                            strToiday = "";

                            //사용자 중지
                            if (bolSTOP == true)
                            {
                                return;
                            }

                            strRowid = dt.Rows[i]["ROWID"].ToString().Trim();
                            strPano = dt.Rows[i]["PTNO"].ToString().Trim();
                            strDrSabun = dt.Rows[i]["USEID"].ToString().Trim();

                            //'인증된것 제외함
                            if (VB.Val(dt.Rows[i]["CertNo"].ToString().Trim()) != 0)
                            {
                                strOK = "NO";
                            }

                            if (strOK == "OK")
                            {
                                clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                                clsDB.setBeginTran(pDbCon);

                                try
                                {                                    
                                    strEMRData = "처방일자:" + dt.Rows[i]["IT1"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "처방코드:" + dt.Rows[i]["IT2"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "처방명:" + dt.Rows[i]["IT3"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "용법및검체:" + dt.Rows[i]["IT4"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "횟수:" + dt.Rows[i]["IT5"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "일투량:" + dt.Rows[i]["IT6"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "비고:" + dt.Rows[i]["IT7"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "일련번호:" + dt.Rows[i]["IT8"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "ACTING:" + dt.Rows[i]["IT9"].ToString().Trim() + " |";
                                    strEMRData = strEMRData + "간호사참고사항:" + dt.Rows[i]["IT10"].ToString().Trim();

                                    if (string.IsNullOrEmpty(strEMRData) == true)
                                    {
                                        strOK = "NO";
                                        strError = "데이타삭제됨 : " + dt.Rows[i]["EMRNO"].ToString().Trim();
                                    }

                                    if (strOK.Equals("OK"))
                                    {
                                        // 전자인증
                                        strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.EMRXML, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                        if (strError != "OK")
                                        {
                                            strOK = "NO";
                                            strError = strError + " : " + strEMRData;
                                        }
                                    }

                                    if (strOK.Equals("OK"))
                                    {
                                        if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.EMRXML, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
                                        {
                                            strOK = "NO";
                                            strError = string.Concat("DB등록 오류", strEMRData);
                                        }
                                    }

                                    txtEmrData.Text = strEMRData;
                                    txtHashData.Text = strHashData;
                                    txtSignData.Text = strSignData;
                                    txtCertData.Text = strCertData;

                                    if (strOK.Equals("OK"))
                                    {
                                        clsDB.setCommitTran(pDbCon);
                                    }
                                    else
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = GetOldEmrFormName(pDbCon, dt.Rows[i]["FORMNO"].ToString().Trim());

                                        if (strToiday != "")
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName + "[퇴사:" + strToiday + "]";
                                        }
                                        else
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName;
                                        }
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strError;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    dt.Dispose();
                                    dt = null;
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(ex.Message);
                                }
                            }

                            Application.DoEvents();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private string GetOldEmrXMLData(PsmhDb pDbCon, string strEmrNo)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += "SELECT                              \r";
                SQL += "    CHARTXML                        \r";
                SQL += " FROM KOSMOS_EMR.EMRXML             \r";
                SQL += "WHERE EMRNO = '" + strEmrNo + "'    \r";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                
                if (SqlErr != "")
                {                    
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["CHARTXML"].ToString();
                    rtnVal = rtnVal.Substring(0, rtnVal.Length > 3000 ? 3000 : rtnVal.Length);
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private string GetOldEmrFormName(PsmhDb pDbCon, string strFormNo)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += "SELECT FORMNAME FROM KOSMOS_EMR.EMRFORM  \r";
                SQL += " WHERE FORMNO = '" + strFormNo + "'    \r";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["FORMNAME"].ToString();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        public static string GetNewEmrJsonData(PsmhDb pDbCon, string strFormNo, string strEmrNo)
        {
            string rtnVal = "";
            OracleDataReader reader = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string quote = "\"";

            try
            {
                if (strFormNo.Equals("1568"))
                {
                    // 1568 : 마취기록지
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNO, \r";
                    SQL += "    '{' || '\"ITEMCD\"' || ':\"' || B.TITLE || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.CONTROLVALUE || '\"}' AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRANCHART B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                    SQL += ") \r";
                    SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNO \r";
                }
                else if (strFormNo.Equals("965") || strFormNo.Equals("2049") || strFormNo.Equals("2137"))
                {
                    // 965 : 간호기록지
                    // 2049 : 응급 간호기록지(ER)
                    // 2137 : 회복실 간호기록지(RE)
                    SQL = "";
                    SQL += "SELECT 'WARDCODE : ' || WARDCODE || ' | ' || \r";
                    SQL += "       'ROOMCODE : ' || ROOMCODE || ' | ' || \r";
                    SQL += "       'PROBLEMCODE : ' || PROBLEMCODE || ' | ' || \r";
                    SQL += "       'PROBLEM : ' || PROBLEM || ' | ' || \r";
                    SQL += "       'TYPE : ' || TYPE || ' | ' || \r";
                    SQL += "       'NRRECODE : ' || NRRECODE AS JSON \r";
                    SQL += " FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRNURSRECORD B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                }
                else if (strFormNo.Equals("1575"))
                {
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNO, \r";
                    SQL += "    TO_CLOB('{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.ITEMVALUE || '\", ' || '\"ITEMVALUE1\"' || ':\"' || B.ITEMVALUE1 || '\", ITEMVALUE2\"' || ':\"' || B.ITEMVALUE2 || '\"}') AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B \r";
                    SQL += "   ON A.EMRNO = B.EMRNO \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "  AND B.ITEMCD > CHR(0) \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                    SQL += "ORDER BY B.DSPSEQ \r";
                    SQL += ") \r";
                    SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNO \r";
                }
                else
                {
                    #region 4천바이트 넘는게 있는지 확인
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS(                                                                                     \r";
                    SQL += "SELECT  LENGTHB(B.ITEMVALUE) SIZE1                                                                      \r";
                    SQL += "    ,   LENGTHB(B.ITEMVALUE1) SIZE2                                                                     \r";
                    SQL += "    ,   LENGTHB(B.ITEMVALUE2) SIZE3                                                                     \r";
                    SQL += "    ,   B.ITEMCD                                                                                        \r";
                    SQL += "    ,   B.ITEMVALUE                                                                                     \r";
                    SQL += "    ,   B.ITEMVALUE1                                                                                    \r";
                    SQL += "    ,   B.ITEMVALUE2                                                                                    \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A                                                                          \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B                                                                    \r";
                    SQL += "   ON A.EMRNO = B.EMRNO                                                                                 \r";
                    SQL += "  AND A.EMRNOHIS = B.EMRNOHIS                                                                           \r";
                    SQL += "  AND B.ITEMCD > CHR(0)                                                                                 \r";
                    SQL += "WHERE A.EMRNO = " + strEmrNo + "                                                                        \r";
                    SQL += ")                                                                                                       \r";
                    SQL += "  SELECT                                                                                                \r";
                    SQL += "          ITEMCD                                                                                        \r";
                    SQL += "    ,     ITEMVALUE                                                                                     \r";
                    SQL += "    ,     ITEMVALUE1                                                                                    \r";
                    SQL += "    ,     ITEMVALUE2                                                                                    \r";
                    SQL += "    FROM TEMP_TABLE                                                                                     \r";
                    SQL += "   WHERE (NVL(SIZE1, 0) + NVL(SIZE2, 0)  + NVL(SIZE3, 0)) >= 3500                                       \r";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }
                    #endregion


                    if (reader.HasRows)
                    {
                        StringBuilder builder = new StringBuilder();
                        while (reader.Read())
                        {
                            builder.Append("{").Append(quote).Append("ITEMCD").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(0).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(1).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE1").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(2).ToString().Trim()).Append(quote).Append(", ");

                            builder.Append(quote).Append("ITEMVALUE2").Append(quote).Append(":");
                            builder.Append(quote).Append(reader.GetValue(3).ToString().Trim()).Append(quote).Append("}");
                        }

                        rtnVal = builder.ToString().Trim();

                    }
                    else
                    {
                        reader.Dispose();
                        reader = null;

                        SQL = "";
                        SQL += "WITH TEMP_TABLE AS( \r";
                        SQL += "SELECT \r";
                        SQL += "    A.EMRNO, \r";
                        SQL += "    TO_CLOB('{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.ITEMVALUE || '\"}') AS ITEM \r";
                        SQL += "FROM KOSMOS_EMR.AEMRCHARTMST A \r";
                        SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROW B \r";
                        SQL += "   ON A.EMRNO = B.EMRNO \r";
                        SQL += "  AND A.EMRNOHIS = B.EMRNOHIS \r";
                        SQL += "  AND B.ITEMCD > CHR(0) \r";
                        SQL += "WHERE A.EMRNO = " + strEmrNo + "    \r";
                        SQL += "ORDER BY B.DSPSEQ \r";
                        SQL += ") \r";
                        SQL += "SELECT WM_CONCAT(ITEM) JSON \r";
                        SQL += "  FROM TEMP_TABLE \r";
                        SQL += " GROUP BY EMRNO \r";
                    }
                }

                if (rtnVal.IsNullOrEmpty())
                {
                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        return rtnVal;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        //rtnVal = dt.Rows[0]["JSON"].ToString().Trim();
                        rtnVal = reader.GetValue(0).ToString().Trim();
                        //rtnVal = rtnVal.Substring(0, rtnVal.Length > 3000 ? 3000 : rtnVal.Length);
                    }
                }

                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }

            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private string GetNewEmrHisJsonData(PsmhDb pDbCon, string strFormNo, string strEmrNoHis)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (strFormNo.Equals("1568"))
                {
                    // 1568 : 마취기록지
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNOHIS, \r";
                    SQL += "    '{' || '\"ITEMCD\"' || ':\"' || B.TITLE || '\", ' || '\"ITEMVALUE\"' || ':\"' || B.CONTROLVALUE || '\"}' AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMSTHIS A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRANCHART B \r";
                    SQL += "    ON A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "WHERE A.EMRNOHIS = '" + strEmrNoHis + "'    \r";
                    SQL += ") \r";
                    SQL += "SELECT EMRNOHIS \r";
                    SQL += "     , WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNOHIS \r";
                }
                else if (strFormNo.Equals("965") || strFormNo.Equals("2049") || strFormNo.Equals("2137"))
                {
                    // 965 : 간호기록지
                    // 2049 : 응급 간호기록지(ER)
                    // 2137 : 회복실 간호기록지(RE)
                    SQL = "";
                    SQL += "SELECT A.EMRNO, \r";
                    SQL += "       'WARDCODE : ' || WARDCODE || ' | ' || \r";
                    SQL += "       'ROOMCODE : ' || ROOMCODE || ' | ' || \r";
                    SQL += "       'PROBLEMCODE : ' || PROBLEMCODE || ' | ' || \r";
                    SQL += "       'PROBLEM : ' || PROBLEM || ' | ' || \r";
                    SQL += "       'TYPE : ' || TYPE || ' | ' || \r";
                    SQL += "       'NRRECODE : ' || NRRECODE AS JSON \r";
                    SQL += " FROM KOSMOS_EMR.AEMRCHARTMSTHIS A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRNURSRECORDHIS B \r";
                    SQL += "   ON A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "WHERE A.EMRNOHIS = '" + strEmrNoHis + "'    \r";
                }
                else
                {
                    SQL = "";
                    SQL += "WITH TEMP_TABLE AS( \r";
                    SQL += "SELECT \r";
                    SQL += "    A.EMRNOHIS, \r";
                    SQL += "    '{' || '\"ITEMCD\"' || ':\"' || B.ITEMCD || '\", ' || '\"ITEMVALUE\"' || ':\"' || SUBSTRB(B.ITEMVALUE, 0, 3950) || '\"}' AS ITEM \r";
                    SQL += "FROM KOSMOS_EMR.AEMRCHARTMSTHIS A \r";
                    SQL += "INNER JOIN KOSMOS_EMR.AEMRCHARTROWHIS B \r";
                    SQL += "    ON A.EMRNOHIS = B.EMRNOHIS \r";
                    SQL += "WHERE A.EMRNOHIS = '" + strEmrNoHis + "'    \r";
                    SQL += "ORDER BY B.DSPSEQ \r";
                    SQL += ") \r";
                    SQL += "SELECT EMRNOHIS \r";
                    SQL += "     , WM_CONCAT(ITEM) JSON \r";
                    SQL += "  FROM TEMP_TABLE \r";
                    SQL += " GROUP BY EMRNOHIS \r";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["JSON"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        private string GetNewEmrFormName(PsmhDb pDbCon, string strFormNo, string strUpdateNo)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += "SELECT FORMNAME FROM KOSMOS_EMR.AEMRFORM  \r";
                SQL += " WHERE FORMNO = '" + strFormNo + "'    \r";
                SQL += "   AND UPDATENO = '" + strUpdateNo + "'    \r";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["FORMNAME"].ToString();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        private void AUTO_CERT_AEMRCHARTMST(PsmhDb pDbCon)
        {
            string strOK = "OK";
            string strError = string.Empty;
            string strName = "";
            string strToiday = "";

            int i = 0;
            OracleDataReader reader = null;

            double nCertno = 0;
            string strEMRData = string.Empty;
            string strHashData = string.Empty;
            string strSignData = string.Empty;
            string strCertData = string.Empty;
            string strRowid = string.Empty;
            string strDrSabun = string.Empty;
            string strPano = string.Empty;

            lblFrDate.Text = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().ToString("yyyy-MM-dd HH:mm");
            MParameter parameter = new MParameter();

            try
            {
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                parameter.AppendSql("SELECT /*+ INDEX(KOSMOS_EMR.AEMRCHARTMST AEMRCHARTMSTHIS_IDX_MIBI) */                  ");
                parameter.AppendSql("       A.EMRNO, A.PTNO, A.CHARTUSEID, A.ROWID,                                         ");
                parameter.AppendSql("       A.CHARTDATE, A.WRITEDATE, A.FORMNO, A.UPDATENO, A.CERTNO                        ");
                parameter.AppendSql("       , TO_CHAR(SYSDATE, 'YYYYMMDD') AS NOWDATE                                       ");
                parameter.AppendSql("  FROM KOSMOS_EMR.AEMRCHARTMST A                                                       ");
                parameter.AppendSql("  LEFT OUTER JOIN KOSMOS_PMPA.BAS_PATIENT  B                                            ");
                parameter.AppendSql("    ON A.PTNO = B.PANO                                                                 ");
                parameter.AppendSql(" WHERE 1 = 1                                                                           ");
                parameter.AppendSql("   AND A.CHARTUSEID <> '합계'                                                           ");
                parameter.AppendSql("   AND A.SAVECERT = '1' --인증저장 일경우만                                               ");
                parameter.AppendSql("   AND A.CERTNO IS NULL -- 인증번호 없을경우만                                            ");
                parameter.AppendSql("   AND A.WRITEDATE >= '20200422'                                                       ");
                parameter.AppendSql("   AND A.PTNO NOT IN                                                                   ");
                parameter.AppendSql("   (                                                                                   ");                                                                
                parameter.AppendSql("       '81000001', '81000002', '81000003', '81000004', '81000005',                     ");
                parameter.AppendSql("       '81000006', '81000007', '81000008', '81000009', '81000010',                     ");
                parameter.AppendSql("       '81000011', '81000012', '81000013', '81000014', '81000015',                     ");
                parameter.AppendSql("       '81000016', '81000017', '81000018', '81000019', '81000020'                      ");
                parameter.AppendSql("   )                                                                                   ");

                if (chkDate.Checked == true)
                {
                    parameter.AppendSql("   AND A.WRITEDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'               ");
                    parameter.AppendSql("   AND A.WRITEDATE <= '" + dtpToDate.Value.ToString("yyyyMMdd") + "'               ");
                }
                else
                {
                    parameter.AppendSql("   AND A.WRITEDATE >= TO_CHAR(SYSDATE - 15, 'YYYYMMDD')                            ");
                    parameter.AppendSql("   AND A.WRITEDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                                 ");
                }
                parameter.AppendSql("   AND EXISTS                                                                          ");
                parameter.AppendSql("   (                                                                                   ");
                parameter.AppendSql("   		SELECT 1                                                                    ");
                parameter.AppendSql("             FROM KOSMOS_PMPA.BAS_PATIENT                                              ");
                parameter.AppendSql("   		 WHERE PANO = A.PTNO    	                                                ");
                parameter.AppendSql("   )                                                                                   ");

                parameter.AppendSql("   AND EXISTS                                                                          ");
                parameter.AppendSql("   (                                                                                   ");
                parameter.AppendSql("   		SELECT 1                                                                    ");
                parameter.AppendSql("             FROM KOSMOS_ERP.HR_EMP_BASIS  SUB                                            ");
                parameter.AppendSql("   		 WHERE SUB.EMP_ID = A.CHARTUSEID    	                                        ");
                parameter.AppendSql("   		   AND SUB.RETIRE_YMD IS NULL    	                                        ");
                parameter.AppendSql("   )                                                                                   ");

                parameter.AppendSql("   AND ROWNUM <= 10000                                                                 ");

                parameter.AppendSql(" GROUP BY A.EMRNO, A.PTNO, A.CHARTUSEID, A.ROWID,                                      ");
                parameter.AppendSql("          A.CHARTDATE, A.WRITEDATE , A.FORMNO, A.UPDATENO, A.CERTNO                    ");
                parameter.AppendSql(" ORDER BY A.WRITEDATE                                                                  ");

                List<Dictionary<string, object>> dt = clsDB.ExecuteReader(parameter, clsDB.DbCon);

                if (dt.Count > 0)
                {
                    for(i = 0; i < dt.Count; i++)
                    {
                        strOK = "OK";
                        strError = "";
                        strName = "";
                        strToiday = "";

                        //사용자 중지
                        if (bolSTOP == true)
                        {
                            return;
                        }

                        strRowid = dt[i]["ROWID"].ToString().Trim();
                        strPano = dt[i]["PTNO"].ToString().Trim();
                        strDrSabun = dt[i]["CHARTUSEID"].ToString().Trim();

                        ////'인증된것 제외함
                        if (VB.Val(dt[i]["CERTNO"].ToString().Trim()) != 0)
                        {
                            strOK = "NO";
                        }

                        if (strOK == "OK")
                        {
                            clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                            clsDB.setBeginTran(pDbCon);

                            try
                            {
                                string FORMNO = dt[i]["FORMNO"].ToString().Trim();
                                string UPDATENO = dt[i]["UPDATENO"].ToString().Trim();
                                string EMRNO = dt[i]["EMRNO"].ToString().Trim();

                                strEMRData = GetNewEmrJsonData(clsDB.DbCon, FORMNO, EMRNO);
                                if (string.IsNullOrEmpty(strEMRData) == true)
                                {
                                    strOK = "NO";
                                    strError = "데이타삭제됨 : " + EMRNO;
                                }

                                string TBL_CERTDATE = dt[i]["NOWDATE"].ToString().Trim();

                                if (strOK.Equals("OK"))
                                {
                                    // 작성일 기준 전자인증 테이블 적용
                                    //CERTDATE = reader.GetValue(5).ToString().Trim();

                                    // 전자인증
                                    strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, TBL_CERTDATE, clsCertWork.AEMRCHARTMST, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                    if (strError != "OK")
                                    {
                                        strOK = "NO";
                                        strError = strError + " : " + strEMRData;
                                    }
                                }

                                if (strOK.Equals("OK"))
                                {
                                    if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.AEMRCHARTMST, clsCertWork.CERTDATE, TBL_CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
                                    {
                                        strOK = "NO";
                                        strError = string.Concat("DB등록 오류", strEMRData);
                                    }
                                }

                                txtEmrData.Text = strEMRData;
                                txtHashData.Text = strHashData;
                                txtSignData.Text = strSignData;
                                txtCertData.Text = strCertData;

                                if (strOK.Equals("OK"))
                                {
                                    clsDB.setCommitTran(pDbCon);                                        
                                }
                                else
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt[i]["CHARTDATE"].ToString().Trim();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt[i]["PTNO"].ToString().Trim();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = EMRNO;
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = GetNewEmrFormName(pDbCon, FORMNO, UPDATENO);

                                    if (strToiday != "")
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName + "[퇴사:" + strToiday + "]";
                                    }
                                    else
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName;
                                    }
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strError;
                                }
                            }
                            catch (Exception ex)
                            {
                                //dt.Dispose();
                                //dt = null;
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                            }
                        }

                        Application.DoEvents();
                    }
                }

                dt.Clear();
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                    reader = null;
                }
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void AUTO_CERT_AEMRCHARTMSTHIS(PsmhDb pDbCon)
        {
            string strOK = "OK";
            string strError = string.Empty;
            string strName = "";
            string strToiday = "";

            int i = 0;
            double nCertno = 0;
            string strEMRData = string.Empty;
            string strHashData = string.Empty;
            string strSignData = string.Empty;
            string strCertData = string.Empty;
            string strRowid = string.Empty;
            string strDrSabun = string.Empty;
            string strPano = string.Empty;

            string strSDate = string.Empty;

            ComFunc.ReadSysDate(pDbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            MParameter parameter = new MParameter();

            try
            {
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                parameter.AppendSql("SELECT /*+ INDEX(KOSMOS_EMR.AEMRCHARTMSTHIS AEMRCHARTMSTHIS_IDX4) */       "); 
                parameter.AppendSql("       A.EMRNOHIS, A.PTNO, A.CHARTUSEID, A.ROWID,                          "); 
                parameter.AppendSql("       A.CHARTDATE, A.WRITEDATE, A.FORMNO, A.UPDATENO, A.CERTNO            "); 
                parameter.AppendSql("       , TO_CHAR(SYSDATE, 'YYYYMMDD') AS NOWDATE                             "); 
                parameter.AppendSql("  FROM KOSMOS_EMR.AEMRCHARTMSTHIS A                                        ");
                parameter.AppendSql("  LEFT OUTER JOIN KOSMOS_PMPA.BAS_PATIENT  B                                            ");
                parameter.AppendSql("    ON A.PTNO = B.PANO                                                             ");
                parameter.AppendSql("WHERE 1 = 1                                                                "); 
                parameter.AppendSql("    AND A.CHARTUSEID <> '합계'                                              "); 
                parameter.AppendSql("    AND A.SAVECERT = '1' -- 인증저장                                        "); 
                parameter.AppendSql("    AND A.CERTNO IS NULL -- 인증번호 없을경우만                               ");
                parameter.AppendSql("    AND A.WRITEDATE >= '20200422'                                          ");
                parameter.AppendSql("    AND A.PTNO NOT IN                                                      ");
                parameter.AppendSql("    (                                                                      ");
                parameter.AppendSql("    '81000001', '81000002', '81000003', '81000004', '81000005',            "); 
                parameter.AppendSql("    '81000006', '81000007', '81000008', '81000009', '81000010',            "); 
                parameter.AppendSql("    '81000011', '81000012', '81000013', '81000014', '81000015',            "); 
                parameter.AppendSql("    '81000016', '81000017', '81000018', '81000019', '81000020'             ");
                parameter.AppendSql("    )                                                                      ");

                if (chkDate.Checked == true)
                {
                    parameter.AppendSql("    AND A.WRITEDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'  "); 
                    parameter.AppendSql("    AND A.WRITEDATE <= '" + dtpToDate.Value.ToString("yyyyMMdd") + "'  ");
                }
                else
                {
                    parameter.AppendSql("    AND A.WRITEDATE >= TO_CHAR(SYSDATE - 15, 'YYYYMMDD')               "); 
                    parameter.AppendSql("    AND A.WRITEDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')                    ");
                }

                parameter.AppendSql("   AND EXISTS                                                                          ");
                parameter.AppendSql("   (                                                                                   ");
                parameter.AppendSql("   		SELECT 1                                                                    ");
                parameter.AppendSql("              FROM KOSMOS_PMPA.BAS_PATIENT                                             ");
                parameter.AppendSql("   		 WHERE PANO = A.PTNO    	                                                ");
                parameter.AppendSql("   )                                                                                   ");

                parameter.AppendSql("   AND EXISTS                                                                          ");
                parameter.AppendSql("   (                                                                                   ");
                parameter.AppendSql("   		SELECT 1                                                                    ");
                parameter.AppendSql("             FROM KOSMOS_ERP.HR_EMP_BASIS  SUB                                            ");
                parameter.AppendSql("   		 WHERE SUB.EMP_ID = A.CHARTUSEID    	                                        ");
                parameter.AppendSql("   		   AND SUB.RETIRE_YMD IS NULL    	                                        ");
                parameter.AppendSql("   )                                                                                   ");


                parameter.AppendSql("   AND ROWNUM <= 10000                                                                   ");

                parameter.AppendSql(" GROUP BY A.EMRNOHIS, A.PTNO, A.CHARTUSEID, A.ROWID,                       ");
                parameter.AppendSql("          A.CHARTDATE, A.WRITEDATE , A.FORMNO, A.UPDATENO, A.CERTNO        ");
                parameter.AppendSql(" ORDER BY A.WRITEDATE  ");

                List<Dictionary<string, object>> dt = clsDB.ExecuteReader(parameter, clsDB.DbCon);

                if (dt.Count > 0)
                {
                    for (i = 0; i < dt.Count; i++)
                    {
                        strOK = "OK";
                        strError = "";
                        strName = "";
                        strToiday = "";

                        //사용자 중지
                        if (bolSTOP == true)
                        {
                            return;
                        }

                        strRowid = dt[i]["ROWID"].ToString().Trim();
                        strPano = dt[i]["PTNO"].ToString().Trim();
                        strDrSabun = dt[i]["CHARTUSEID"].ToString().Trim();

                        //'인증된것 제외함
                        if (VB.Val(dt[i]["CERTNO"].ToString().Trim()) != 0)
                        {
                            strOK = "NO";
                        }

                        if (strOK == "OK")
                        {
                            clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                            clsDB.setBeginTran(pDbCon);

                            try
                            {
                                strEMRData = GetNewEmrHisJsonData(clsDB.DbCon, dt[i]["FORMNO"].ToString().Trim(), dt[i]["EMRNOHIS"].ToString().Trim());
                                if (string.IsNullOrEmpty(strEMRData) == true)
                                {
                                    strOK = "NO";
                                    strError = "데이타삭제됨 : " + dt[i]["EMRNOHIS"].ToString().Trim();
                                }

                                if (strOK.Equals("OK"))
                                {
                                    // 작성일 기준 전자인증 테이블 적용
                                    CERTDATE = dt[i]["NOWDATE"].ToString().Trim();
                                    // 전자인증
                                    strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.AEMRCHARTMSTHIS, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                    if (strError != "OK")
                                    {
                                        strOK = "NO";
                                        strError = strError + " : " + strEMRData;
                                    }
                                }

                                if (strOK.Equals("OK"))
                                {
                                    if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.AEMRCHARTMSTHIS, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
                                    {
                                        strOK = "NO";
                                        strError = string.Concat("DB등록 오류", strEMRData);
                                    }
                                }

                                txtEmrData.Text = strEMRData;
                                txtHashData.Text = strHashData;
                                txtSignData.Text = strSignData;
                                txtCertData.Text = strCertData;

                                if (strOK.Equals("OK"))
                                {
                                    clsDB.setCommitTran(pDbCon);
                                }
                                else
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt[i]["CHARTDATE"].ToString().Trim();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt[i]["PTNO"].ToString().Trim();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt[i]["EMRNOHIS"].ToString().Trim();
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = GetNewEmrFormName(pDbCon, dt[i]["FORMNO"].ToString().Trim(), dt[i]["UPDATENO"].ToString().Trim());

                                    if (strToiday != "")
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName + "[퇴사:" + strToiday + "]";
                                    }
                                    else
                                    {
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName;
                                    }
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strError;
                                }
                            }
                            catch (Exception ex)
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(ex.Message);
                            }
                        }

                        Application.DoEvents();
                    }
                }

                dt.Clear();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 전자동의서 전자인증
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_AEASFORMDATA(PsmhDb pDbCon)
        {
            string strOK = "OK";
            string strError = string.Empty;
            string strName = "";
            string strToiday = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double nCertno = 0;
            string strJSONData = string.Empty;
            string strHashData = string.Empty;
            string strSignData = string.Empty;
            string strCertData = string.Empty;
            string strRowid = string.Empty;
            string strDrSabun = string.Empty;
            string strPano = string.Empty;

            long nDay = 0;
            long nDay2 = 16;
            string strSDate = string.Empty;

            ComFunc.ReadSysDate(pDbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);
            try
            {
                for (nDay = 1; nDay <= 16; nDay++)
                {
                    nDay2 = nDay2 - 1;

                    if (nDay2 < -1) break;

                    strSDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(nDay2 * -1).ToString("yyyy-MM-dd");

                    SQL = "";
                    SQL += "SELECT A.ID, A.PTNO, A.MODIFIED, A.MODIFIEDUSER, A.JSON, A.ROWID, C.FORMNAME, A.CERTDATE, A.CERTNO  \r";
                    SQL += "  FROM KOSMOS_EMR.AEASFORMDATA A \r";
                    SQL += " INNER JOIN KOSMOS_EMR.AEASFORMCONTENT B \r";
                    SQL += "    ON A.EASFORMCONTENT = B.ID \r";
                    SQL += " INNER JOIN KOSMOS_EMR.AEMRFORM C \r";
                    SQL += "    ON C.FORMNO = B.FORMNO \r";
                    SQL += "   AND C.UPDATENO = B.UPDATENO \r";
                    SQL += " WHERE A.ISDELETED = 'N' \r";
                    SQL += "   AND A.CERTNO IS NULL  \r";
                    SQL += "   AND ROWNUM <= 10000                                                      \r";
                    SQL += "   AND A.CHARTDATE >= '20200422'                                           \r";

                    if (chkDate.Checked == true)
                    {
                        SQL += "    AND TRUNC(A.MODIFIED) >= TO_DATE('" + dtpFrDate.Value.ToString("yyyyMMdd") + "', 'YYYY-MM-DD')  \r";
                        SQL += "    AND TRUNC(A.MODIFIED) <= TO_DATE('" + dtpToDate.Value.ToString("yyyyMMdd") + "', 'YYYY-MM-DD')  \r";
                    }
                    else
                    {
                        if (strSDate != "")
                        {
                            SQL += "    AND TRUNC(A.MODIFIED) = TO_DATE('" + strSDate + "', 'YYYY-MM-DD') \r";
                        }
                        else
                        {
                            SQL += "    AND TRUNC(A.MODIFIED) >= TRUNC(SYSDATE - 15)    \r";
                            SQL += "    AND TRUNC(A.MODIFIED) <= TRUNC(SYSDATE)         \r";                            
                        }
                    }                    
                    SQL += "   ORDER BY A.MODIFIED  \r";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(pDbCon);
                        //clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strOK = "OK";
                            strError = "";
                            strName = "";
                            strToiday = "";
                            strJSONData = "";

                            //사용자 중지
                            if (bolSTOP == true)
                            {
                                return;
                            }

                            strRowid = dt.Rows[i]["ROWID"].ToString().Trim();
                            strPano = dt.Rows[i]["PTNO"].ToString().Trim();
                            strDrSabun = dt.Rows[i]["MODIFIEDUSER"].ToString().Trim();

                            //'인증된것 제외함
                            if (VB.Val(dt.Rows[i]["CERTNO"].ToString().Trim()) != 0)
                            {
                                strOK = "NO";
                            }

                            if (strOK == "OK")
                            {
                                clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                                clsDB.setBeginTran(pDbCon);

                                try
                                {
                                    strJSONData = dt.Rows[i]["JSON"].ToString().Trim();
                                    //strJSONData = strJSONData.SubStrByte(3999).Trim();
                                    if (string.IsNullOrEmpty(strJSONData) == true)
                                    {
                                        strOK = "NO";
                                        strError = "데이타삭제됨 : " + dt.Rows[i]["EMRNO"].ToString().Trim();
                                    }

                                    if (strOK.Equals("OK"))
                                    {
                                        // 전자인증
                                        strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.AEASFORMDATA, strPano, strJSONData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                        if (strError != "OK")
                                        {
                                            strOK = "NO";
                                            strError = strError + " : " + strJSONData;
                                        }
                                    }

                                    if (strOK.Equals("OK"))
                                    {
                                        if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_EMR, clsCertWork.AEASFORMDATA, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
                                        {
                                            strOK = "NO";
                                            strError = string.Concat("DB등록 오류", strJSONData);
                                        }
                                    }

                                    txtEmrData.Text = strJSONData;
                                    txtHashData.Text = strHashData;
                                    txtSignData.Text = strSignData;
                                    txtCertData.Text = strCertData;

                                    if (strOK.Equals("OK"))
                                    {
                                        clsDB.setCommitTran(pDbCon);                                        
                                    }
                                    else
                                    {
                                        clsDB.setRollbackTran(pDbCon);
                                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["MODIFIED"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ID"].ToString().Trim();
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["FORMNAME"].ToString().Trim();

                                        if (strToiday != "")
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName + "[퇴사:" + strToiday + "]";
                                        }
                                        else
                                        {
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = strName;
                                        }
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = strError;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (dt != null)
                                    {
                                        dt.Dispose();
                                        dt = null;
                                    }
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(ex.Message);
                                }
                            }

                            Application.DoEvents();
                        }
                    }

                    dt.Dispose();
                    dt = null;
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
    }
}
