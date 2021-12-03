using ComBase;
using ComDbB;
using ComLibB.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmCertPoolOrderMain : Form
    {
        bool bolSTOP = false;
        int FnCntPic = 0;
        int FnTimerCnt = 0;
        public frmCertPoolOrderMain()
        {
            InitializeComponent();
        }

        private void frmCertPoolOrderMain_Load(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            txtEmrData.Text = "";
            txtHashData.Text = "";
            txtSignData.Text = "";
            txtCertData.Text = "";

            lblSDate.Text = "";
            lblFrDate.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            lblSDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);
            LblTitle.Text = "";
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

                //병동 OCS
                AUTO_CERT_OCS_IORDER(clsDB.DbCon);
                //병동 구두 의사처방
                AUTO_CERT_OCS_IORDER_VERBAL(clsDB.DbCon);
                //외래 OCS
                AUTO_CERT_OCS_OORDER(clsDB.DbCon);


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

        private void AUTO_CERT_OCS_IORDER(PsmhDb pDbCon)
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
            long nOrderNo;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = "SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE,                                         \r";
                SQL += "      BUN, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT, GBER,                                     \r";
                SQL += "      GBPRN, GBDIV, GBBOTH, GBACT, GBSEND, GBPOSITION, GBSTATUS, NURSEID,                                                                    \r";
                SQL += "      TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK, ACTDATE,                                       \r";
                SQL += "      GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, POWDER,                                                                                \r";
                SQL += "      VERBC,PRN_INS_GBN,PRN_INS_UNIT, TO_CHAR(PRN_INS_SDATE,'YYYY-MM-DD') PRN_INS_SDATE,TO_CHAR(PRN_INS_EDATE,'YYYY-MM-DD') PRN_INS_EDATE ,  \r";
                SQL += "      POWDER_SAYU,PRN_INS_MAX,ASA , ROWID                                                                                                    \r";
                SQL += "      , TUYEOPOINT, TUYEOTIME                                                                                                                \r";
                SQL += " FROM KOSMOS_OCS.OCS_IORDER                                                                                                                  \r";
                SQL += "WHERE ENTDATE BETWEEN TRUNC(SYSDATE - 30) AND TRUNC(SYSDATE + 1)                                                                             \r";
                //SQL += "WHERE ENTDATE BETWEEN TO_DATE('2019-02-01','YYYY-MM-DD') AND TO_DATE('2019-02-10','YYYY-MM-DD') \r"; //'김해수 19년 1월부터 테스트
                SQL += "  AND (CERTNO IS NULL OR CERTNO = 0 )                                                                                                        \r";
                
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
                        strDrSabun = dt.Rows[i]["DRCODE"].ToString().Trim();
                        nOrderNo = long.Parse(dt.Rows[i]["ORDERNO"].ToString().Trim());

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["ptno"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["bdate"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["STAFFID"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SLIPNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SUCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["Contents"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BCONTENTS"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REALQTY"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REALNAL"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DOSCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBINFO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSELF"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSPC"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBNGT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBER"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBPRN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBDIV"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBBOTH"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBACT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSEND"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBPOSITION"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSTATUS"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["NURSEID"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["WARDCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ROOMCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BI"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ACTDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBGROUP"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBPORT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERSITE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["MULTI"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["MULTIREMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["POWDER"].ToString().Trim() + "|";

                        #region 혈종관련 데이터 추가 2021-07-30
                        strEMRData += dt.Rows[i]["TUYEOPOINT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                        #endregion
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_IORDER, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_IORDER, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_IORDER;

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

                        Application.DoEvents();
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

        private void AUTO_CERT_OCS_IORDER_VERBAL(PsmhDb pDbCon)
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
            long nOrderNo;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL += " SELECT PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, DRCODE, STAFFID, SLIPNO, ORDERCODE, SUCODE   \r";
                SQL += "      , BUN, CONTENTS, BCONTENTS, REALQTY, QTY, REALNAL, NAL, DOSCODE, GBINFO, GBSELF, GBSPC, GBNGT     \r";
                SQL += "      , GBER, GBPRN, GBDIV, GBBOTH, GBACT, GBSEND, GBPOSITION, GBSTATUS, NURSEID                        \r";
                SQL += "      , TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE, WARDCODE, ROOMCODE, BI, ORDERNO, REMARK          \r";
                SQL += "      , TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, GBGROUP, GBPORT, ORDERSITE, MULTI, MULTIREMARK, POWDER   \r";
                SQL += "      , VERBC,PRN_INS_GBN,PRN_INS_UNIT, TO_CHAR(PRN_INS_SDATE,'YYYY-MM-DD') PRN_INS_SDATE               \r";
                SQL += "      , TO_CHAR(PRN_INS_EDATE,'YYYY-MM-DD') PRN_INS_EDATE                                               \r";
                SQL += "      , POWDER_SAYU,PRN_INS_MAX,ASA ,DRORDERVIEW, ROWID                                                 \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                                                           \r";
                SQL += "  WHERE ENTDATE BETWEEN TRUNC(SYSDATE - 30) AND TRUNC(SYSDATE + 1)                                      \r";                
                SQL += "    AND CERTNO2 IS NULL                                                                                 \r";
                SQL += "    AND VerbC ='C'                                                                                      \r";

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
                        strDrSabun = dt.Rows[i]["DRCODE"].ToString().Trim();
                        nOrderNo = long.Parse(dt.Rows[i]["ORDERNO"].ToString().Trim());

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["ptno"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["bdate"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["STAFFID"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SLIPNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SUCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["Contents"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BCONTENTS"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REALQTY"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REALNAL"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DOSCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBINFO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSELF"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSPC"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBNGT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBER"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBPRN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBDIV"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBBOTH"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBACT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSEND"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBPOSITION"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSTATUS"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["NURSEID"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["WARDCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ROOMCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BI"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ACTDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBGROUP"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBPORT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERSITE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["MULTI"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["MULTIREMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["POWDER"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DRORDERVIEW"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["VERBC"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["PRN_INS_GBN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["PRN_INS_UNIT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["PRN_INS_SDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["PRN_INS_EDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["POWDER_SAYU"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["PRN_INS_MAX"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ASA"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);

                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_IORDER, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_IORDER, clsCertWork.CERTDATE2, CERTDATE, clsCertWork.CERTNO2, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_IORDER;

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

                        Application.DoEvents();
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

        private void AUTO_CERT_OCS_OORDER(PsmhDb pDbCon)
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
            string strDrCode = string.Empty;
            string strDrSabun = string.Empty;
            string strPano = string.Empty;
            long nOrderNo;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = " SELECT  PTNO ,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ,DEPTCODE,SEQNO,ORDERCODE,SUCODE,BUN,SLIPNO,REALQTY,QTY,NAL,GBDIV,DOSCODE,GBBOTH, ";
                SQL += "        GBINFO,GBER,GBSELF,GBSPC,BI,DRCODE,REMARK, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE ,GBSUNAP,TUYAKNO,ORDERNO,MULTI,MULTIREMARK,DUR,RESV, ";
                SQL += "        SCODESAYU,SCODEREMARK,GBSEND,AUTO_SEND,RES ,GBSPC_NO,WRTNO,ROWID ";
                SQL += "        , TUYEOPOINT, TUYEOTIME ";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER  ";
                SQL += "  WHERE BDATE >= TRUNC(SYSDATE - 100) ";
                //SQL += "    AND ENTDATE BETWEEN TO_DATE('2019-02-01','YYYY-MM-DD') AND TO_DATE('2019-02-10','YYYY-MM-DD')";     //'김해수 19년 1월부터 테스트
                SQL += "    AND ENTDATE BETWEEN TRUNC(SYSDATE - 100) AND TRUNC(SYSDATE + 1) "; //2021-08-28 김해수 100일로 수정 작업
                SQL += "    AND (CERTNO IS NULL OR Certno = 0 ) ";
                SQL += "    AND DRCODE IS NOT NULL ";
                SQL += "    AND ROWNUM < 20000";

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
                        strDrCode = dt.Rows[i]["DRCODE"].ToString().Trim();
                        nOrderNo = long.Parse(dt.Rows[i]["ORDERNO"].ToString().Trim());

                        // 처방의사 사번
                        strDrSabun = clsVbfunc.GetOCSDrCodeSabun(pDbCon, strDrCode);

                        #region // EMRDATA
                        strEMRData = dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SEQNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SUCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SLIPNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REALQTY"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBDIV"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DOSCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBBOTH"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBINFO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBER"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSELF"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSPC"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BI"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["REMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSUNAP"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["TUYAKNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ORDERNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["MULTI"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["MULTIREMARK"].ToString().Trim() + "|";

                        strEMRData += dt.Rows[i]["DUR"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["RESV"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SCODESAYU"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SCODEREMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSEND"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["AUTO_SEND"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["RES"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSPC_NO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["WRTNO"].ToString().Trim() + "|";

                        #region 혈종관련 데이터 추가 2021-07-30
                        strEMRData += dt.Rows[i]["TUYEOPOINT"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                        #endregion
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);

                        try 
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_OORDER, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                // 업데이트
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_OORDER, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_OORDER;

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

                        Application.DoEvents();
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

        private void frmCertPoolOrderMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
