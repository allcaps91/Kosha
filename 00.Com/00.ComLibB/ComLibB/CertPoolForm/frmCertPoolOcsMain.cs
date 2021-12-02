using ComBase;
using ComDbB;
using ComLibB.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmCertPoolOcsMain : Form
    {
        bool bolSTOP = false;
        int FnCntPic = 0;
        int FnTimerCnt = 0;
        public frmCertPoolOcsMain()
        {
            InitializeComponent();
        }

        private void frmCertPoolOcsMain_Load(object sender, EventArgs e)
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

                // 컨설트
                AUTO_CERT_OCS_ITRANSFER(clsDB.DbCon);
                // 내시경
                AUTO_CERT_ENDO_JUPMST(clsDB.DbCon);
                // 종합검증
                AUTO_CERT_EXAM_VERIFY(clsDB.DbCon);
                // 마약처방전
                AUTO_CERT_OCS_MAYAK(clsDB.DbCon);
                // 향정처방전
                AUTO_CERT_OCS_HYANG(clsDB.DbCon);
                // 종검 일반건지 향정처방전
                AUTO_CERT_HIC_HYANG(clsDB.DbCon);
                // 건진 향정,마약 오더
                AUTO_CERT_HIC_HYANG_APPROVE(clsDB.DbCon);


                //방사선 판독 2014-04-21
                AUTO_CERT_XRAY_RESULTNEW(clsDB.DbCon);
                // 수술처방 2014-01-01
                AUTO_CERT_ORAN_SLIP(clsDB.DbCon);


                //'진단서는 2013년 06월 01일 부터 전자인증함
                //'의사 라이선스, 사번, 작성일자(발행일자)
                AUTO_CERT_ETC_WONSELU(clsDB.DbCon);      //'진료사실증명서 ○
                AUTO_CERT_OCS_MCCERTIFI01(clsDB.DbCon);  //'일반진단서 ○
                AUTO_CERT_OCS_MCCERTIFI02(clsDB.DbCon);  //'상해진단서 ○
                AUTO_CERT_OCS_MCCERTIFI03(clsDB.DbCon);  //'병사용진단서 ○
                AUTO_CERT_OCS_MCCERTIFI05(clsDB.DbCon);  //'사망진단서 ○
                AUTO_CERT_OCS_MCCERTIFI08(clsDB.DbCon);  //'진료소견서 ○
                AUTO_CERT_OCS_MCCERTIFI12(clsDB.DbCon);  //'진료회송소 ○
                AUTO_CERT_OCS_MCCERTIFI14(clsDB.DbCon);  //'건강진단서 ○
                AUTO_CERT_OCS_MCCERTIFI18(clsDB.DbCon);  //'진료의뢰서 ○
                AUTO_CERT_OCS_MCCERTIFI19(clsDB.DbCon);  //'장애인증명서 ○
                // 2020-04-09 현재 사용안함. 
                //AUTO_CERT_OCS_MCCERTIFI20(clsDB.DbCon);  //'장애진단서 ○
                AUTO_CERT_OCS_MCCERTIFI21(clsDB.DbCon);  //'출생증명서 ○
                AUTO_CERT_OCS_MCCERTIFI22(clsDB.DbCon);  //'사산증명서 ○
                AUTO_CERT_OCS_MCCERTIFI23(clsDB.DbCon);  //'사산증명서 ○
                AUTO_CERT_OCS_MCCERTIFI24(clsDB.DbCon);  //'영문진단서 ○
                AUTO_CERT_OCS_MCCERTIFI25(clsDB.DbCon);  //'근로능력평가용진단서 ○
                AUTO_CERT_OCS_MCCERTIFI26(clsDB.DbCon);  //'의료급여의뢰서 ○
                AUTO_CERT_OCS_MCCERTIFI27(clsDB.DbCon);  //'응급환자진료의뢰서 ○
                AUTO_CERT_OCS_MCCERTIFI28(clsDB.DbCon);  //'포스코 진단서 ○
                AUTO_CERT_OCS_MCCERTIFI29(clsDB.DbCon);  //'근로능력 2015-09-16

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

        /// <summary>
        /// 컨설트 전자인증
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_ITRANSFER(PsmhDb pDbCon)
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

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                SQL = "";
                SQL += "SELECT KOSMOS_OCS.OCS_ITRANSFER.*, ROWID \r";
                SQL += "  FROM KOSMOS_OCS.OCS_ITRANSFER  \r";
                SQL += " WHERE 1=1   \r";
                SQL += "   AND sDATE >= TRUNC(SYSDATE - 10)  \r";
                SQL += "   AND sDATE < TRUNC(SYSDATE)  \r";
                SQL += "   AND GBFLAG ='1' \r";
                SQL += "   AND (GBSEND IS NULL OR GBSEND = ' ') \r";
                SQL += "   AND GBCONFIRM ='*' \r";
                SQL += "   AND (GBDEL IS NULL OR GBDEL <> '*') \r";
                SQL += "   AND (CERTNO IS NULL OR CertNo  = 0 )  \r";
                SQL += " ORDER BY sDATE \r";
                #endregion

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
                        strDrCode = dt.Rows[i]["FRDRCODE"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["FRDEPTCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["frDRCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["TODEPTCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["TODRCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBFLAG"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBCONFIRM"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["INPDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["INPID"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["FRREMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["TOREMARK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BINPID"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBCONFIRM_OLD"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["EDATE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["orderno"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBDEL"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["WARDCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ROOMCODE"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["IPDNO"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["CDate"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["CSABUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["NURSEOK"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBNST"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SMS_SEND"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["SMS_REQ"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBEMSMS"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["EDATE2"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSTS"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GBSEND2"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["ANTI_SUGA"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE1"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE2"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE3"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE4"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["BOHUM"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE5"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE5_SABUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE6"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE6_SABUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE7"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE8"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE7_SABUN"].ToString().Trim() + "|";
                        strEMRData += dt.Rows[i]["KDATE8_SABUN"].ToString().Trim() + "|";
                        #endregion

                        // 처방의사 사번
                        strDrSabun = clsVbfunc.GetOCSDrCodeSabun(pDbCon, strDrCode);
                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {                                
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_ITRANSFER, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_ITRANSFER, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_ITRANSFER;

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

        /// <summary>
        /// 내시경 전자인증
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_ENDO_JUPMST(PsmhDb pDbCon)
        {
            string strOK = "OK";
            string strError = string.Empty;
            string strName = "";
            string strToiday = "";

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
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
            string strSName = string.Empty;
            string strGBIO = string.Empty;
            string strAge = string.Empty;
            string strSex = string.Empty;
            string strWard = string.Empty;
            string strRoomCode = string.Empty;
            string strDeptName = string.Empty;
            string strDrName = string.Empty;
            string strJDate = string.Empty;
            string strRDate = string.Empty;
            string strResultDate = string.Empty;
            string strSeqNUM = string.Empty;
            string strTSName = string.Empty;
            string strTitle = string.Empty;
            string strRDRName = string.Empty;
            string strChiefCom = string.Empty;
            string strClinicalDia = string.Empty;
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                SQL = "";
                SQL += "SELECT A.PTNO, A.SEX , A.SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM, A.ORDERCODE, A.ROWID, \r";
                SQL += "       TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE,  \r";
                SQL += "       TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,  \r";
                SQL += "       TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE, \r";
                SQL += "       A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate,  \r";
                SQL += "       B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6, \r";
                SQL += "       C.REMARKC , C.REMARKX, C.REMARKP, C.REMARKD \r";
                SQL += "  FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B , KOSMOS_OCS.ENDO_REMARK C \r";
                SQL += " WHERE A.RESULTDATE BETWEEN TRUNC(SYSDATE - 30) AND TRUNC(SYSDATE +1) \r";
                SQL += "   AND A.CERTNO IS NULL \r";
                SQL += "   AND A.SEQNO = B.SEQNO \r";
                SQL += "   AND A.PTNO = C.PTNO(+) \r";
                SQL += "   AND A.JDATE = C.JDATE \r";
                SQL += "   AND A.ORDERCODE = C.ORDERCODE \r";
                #endregion

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

                        strPano = dt.Rows[i]["PTNO"].ToString().Trim();
                        strSName = dt.Rows[i]["SNAME"].ToString().Trim();
                        strGBIO = dt.Rows[i]["GBio"].ToString().Trim() == "I" ? "입원" : "외래";
                        strAge = clsVbfunc.AGE_YEAR_GESAN2(dt.Rows[i]["BIRTHDATE"].ToString().Trim(), dt.Rows[i]["JDATE"].ToString().Trim()).ToString();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();
                        strWard = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        strRoomCode = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        strDrName = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        strJDate = dt.Rows[i]["JDATE"].ToString().Trim();
                        strRDate = dt.Rows[i]["RDATE"].ToString().Trim();
                        strResultDate = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                        strSeqNUM = dt.Rows[i]["SEQNUM"].ToString().Trim();

                        if (string.IsNullOrEmpty(strRoomCode) == false)
                        {
                            strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                        }
                        else
                        {
                            strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                        }

                        strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;
                        strRDRName = clsVbfunc.GetInSaName(pDbCon, dt.Rows[i]["Resultdrcode"].ToString().Trim());


                        SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD  \r";
                        SQL = SQL + "  FROM KOSMOS_OCS.ENDO_REMARK  \r";
                        SQL = SQL + " WHERE PTNO = '" + strPano + "'  \r";
                        SQL = SQL + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD') \r";
                        SQL = SQL + "   AND ORDERCODE = '" + dt.Rows[i]["ORDERCODE"].ToString().Trim() + "'  \r";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(pDbCon);
                            //clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return;
                        }
                        
                        strChiefCom = "";
                        strClinicalDia = "";

                        if (dt2.Rows.Count > 0)
                        {
                            strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                            strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                        }
                        dt2.Dispose();
                        dt2 = null;

                        strRowid = dt.Rows[i]["ROWID"].ToString().Trim();
                        strDrSabun = dt.Rows[i]["Resultdrcode"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = strTitle + "|";
                        strEMRData = strEMRData + strChiefCom + "|";
                        strEMRData = strEMRData + strClinicalDia + "|";
                        strTResult = dt.Rows[i]["REMARK1"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        strTResult = dt.Rows[i]["REMARK2"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        strTResult = dt.Rows[i]["REMARK3"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        strTResult = dt.Rows[i]["REMARK4"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        strTResult = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        strTResult = dt.Rows[i]["REMARK6"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        strEMRData = strEMRData + strRDate + "|";
                        strEMRData = strEMRData + strResultDate + "|";
                        strEMRData = strEMRData + strDrName + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Resultdrcode"].ToString().Trim() + " " + strDrName;
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.ENDO_JUPMST, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.ENDO_JUPMST, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = strResultDate != "" ? Convert.ToDateTime(strResultDate).ToShortDateString() : "";  ;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = strPano;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = strSName;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.ENDO_JUPMST;

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

        /// <summary>
        /// 종합검증
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_EXAM_VERIFY(PsmhDb pDbCon)
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
            string strSName = string.Empty;
            string strAgeSex = string.Empty;
            string strWard = string.Empty;
            string strDeptName = string.Empty;
            string strDrName = string.Empty;
            string strJDate = string.Empty;
            string strResultDate = string.Empty;
            string strSDate = string.Empty;
            string strEdate = string.Empty;

            string strRDRName = string.Empty;
            string strRDrBunho = string.Empty;
            string strResultSabun = string.Empty;

            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'자료를 DB에서 READ
                SQL = "";
                SQL += "SELECT PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE,WARD,STATUS, \r";
                SQL += "       TO_CHAR(JDATE,'YYYY-MM-DD') JDATE, \r";
                SQL += "       TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, \r";
                SQL += "       TO_CHAR(RESULTDATE,'YYYY-MM-DD') RESULTDATE, \r";
                SQL += "       TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, \r";
                SQL += "       TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, \r";
                SQL += "       RESULTSABUN,ITEMS1,ITEMS2,DISEASE, \r";
                SQL += "       VERIFY1,VERIFY2,VERIFY3,VERIFY4,VERIFY5,VERIFY6, \r";
                SQL += "       COMMENTS,RECOMMENDATION,PRINT, ROWID   \r";
                SQL += "  FROM KOSMOS_OCS.EXAM_VERIFY  \r";
                SQL += " WHERE RESULTDATE BETWEEN TRUNC(SYSDATE - 30) AND TRUNC(SYSDATE +1)  \r";
                SQL += "   AND STATUS = '3' \r";     //'결과 완료된것
                SQL += "   AND RESULTDATE IS NOT NULL  \r";
                SQL += "   AND CERTNO  IS  NULL  \r";
                SQL += "   AND ROWNUM <= '100'  \r"; //'100건식
                #endregion

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
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strSName = dt.Rows[i]["SNAME"].ToString().Trim();
                        strAgeSex = dt.Rows[i]["AGE"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                        strWard = dt.Rows[i]["Ward"].ToString().Trim();
                        strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        strDrName = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        strJDate = dt.Rows[i]["JDATE"].ToString().Trim();
                        strResultDate = dt.Rows[i]["RESULTDATE"].ToString().Trim();
                        strSDate = dt.Rows[i]["SDATE"].ToString().Trim();
                        strEdate = dt.Rows[i]["EDATE"].ToString().Trim();
                        strResultSabun = dt.Rows[i]["ResultSabun"].ToString().Trim();

                        //'전문의번호 및 성명을 READ
                        switch (strResultSabun)
                        {
                            case "9089":
                                strRDRName = "김성철";
                                strRDrBunho = "301";
                                break;
                            case "18210":
                                strRDRName = "은상진";
                                strRDrBunho = "424";
                                break;
                            default:
                                strRDRName = "의사오류";
                                strRDrBunho = "***";
                                break;
                        }

                        #region // EMRDATA 생성
                        strEMRData = strEMRData + strSName + "|";
                        strEMRData = strEMRData + strAgeSex + "|";
                        strEMRData = strEMRData + strWard + "|";
                        strEMRData = strEMRData + strDeptName + "|";
                        strEMRData = strEMRData + strDrName + "|";

                        strTResult = dt.Rows[i]["ITEMS1"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strTResult = dt.Rows[i]["ITEMS2"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strEMRData = strEMRData + dt.Rows[i]["VERIFY1"].ToString().Trim() == "Y" ? "●" : "○" + "Calibratrion Verification" + "|";
                        strEMRData = strEMRData + dt.Rows[i]["VERIFY4"].ToString().Trim() == "Y" ? "●" : "○" + "Internal Quality Control" + "|";
                        strEMRData = strEMRData + dt.Rows[i]["VERIFY2"].ToString().Trim() == "Y" ? "●" : "○" + "Delta Check Verification" + "|";
                        strEMRData = strEMRData + dt.Rows[i]["VERIFY5"].ToString().Trim() == "Y" ? "●" : "○" + "Panic/Alert Value Verification" + "|";
                        strEMRData = strEMRData + dt.Rows[i]["VERIFY3"].ToString().Trim() == "Y" ? "●" : "○" + "Repeat/Recheck" + "|";


                        if (dt.Rows[i]["VERIFY6"].ToString().Trim() != "")
                        {
                            strEMRData = strEMRData + "●Others" + dt.Rows[i]["VERIFY6"].ToString().Trim() + "|";
                        }
                        else
                        {
                            strEMRData = strEMRData + "○Others" + "|";
                        }

                        strTResult = dt.Rows[i]["COMMENTS"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";
                        
                        strTResult = dt.Rows[i]["RECOMMENDATION"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strEMRData = strEMRData + "보고일 : " + VB.Left(strJDate, 4) + "년 " + VB.Format(VB.Mid(strJDate, 6, 2), "#0") + "월 " + VB.Format(VB.Right(strJDate, 2), "#0") + "일" + "|";
                        strEMRData = strEMRData + "보고자 : 진단검사의학전문의 " + strRDRName + "|";
                        strEMRData = strEMRData + "         전문의 번호( " + strRDrBunho + " ) ";
                        #endregion

                        // 처방의사 사번
                        strDrSabun = clsVbfunc.GetOCSDrCodeSabun(pDbCon, strDrCode);
                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {                                
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.EXAM_VERIFY, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.EXAM_VERIFY, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["JDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["JDATE"].ToString().Trim()).ToShortDateString() : "";                                 
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.EXAM_VERIFY;

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

        /// <summary>
        /// 마약처방전
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MAYAK(PsmhDb pDbCon)
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

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = "";
                SQL += "SELECT PTNO, SNAME , BI,  AGE, SEX, JUMIN, BDATE,  DEPTCODE, DRSABUN, IO, ORDERNO, JUSO,  REMARK1, REMARK2,  \r";
                SQL += "       SUCODE, QTY, REALQTY, NAL ,DOSCODE , ROWID  \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MAYAK  \r";
                SQL += " WHERE ENTDATE BETWEEN TRUNC(SYSDATE -30) AND TRUNC(SYSDATE +1)  \r";
                SQL += "   AND BDATE >= TRUNC(SYSDATE -30) \r";
                SQL += "   AND CERTNO IS NULL  \r";     //'전사서명 않된오더
                SQL += "   AND PRINT = 'Y'  \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();
                        
                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BI"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUMIN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["orderno"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REALQTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DOSCODE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MAYAK, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MAYAK, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToShortDateString() : "";                                 
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MAYAK;

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
        /// <summary>
        /// 향정처방전
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_HYANG(PsmhDb pDbCon)
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

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = "";
                SQL += "SELECT PTNO, SNAME , BI,  AGE, SEX, JUMIN, BDATE,  DEPTCODE, DRSABUN, IO, ORDERNO, JUSO,  REMARK1, REMARK2,  \r";
                SQL += "       SUCODE, QTY, REALQTY, NAL ,DOSCODE , ROWID  \r";
                SQL += "  FROM KOSMOS_OCS.OCS_HYANG  \r";
                SQL += " WHERE ENTDATE BETWEEN TRUNC(SYSDATE -30) AND TRUNC(SYSDATE +1)  \r";
                SQL += "   AND BDATE >= TRUNC(SYSDATE -30) \r";
                SQL += "   AND CERTNO IS NULL  \r";     //'전사서명 않된오더
                SQL += "   AND PRINT = 'Y'  \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BI"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUMIN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["orderno"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REALQTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DOSCODE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_HYANG, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_HYANG, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_HYANG;

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

        /// <summary>
        /// 종검 일반건지 향정처방전
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_HIC_HYANG(PsmhDb pDbCon)
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

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = "";
                SQL += "SELECT PTNO, SNAME , BI,  AGE, SEX, JUMIN, BDATE,  DEPTCODE, DRSABUN, IO, ORDERNO, JUSO,  REMARK1, REMARK2,  \r";
                SQL += "       SUCODE, QTY, REALQTY, NAL ,DOSCODE , ROWID  \r";
                SQL += "  FROM KOSMOS_PMPA.HIC_HYANG  \r";
                SQL += " WHERE ENTDATE BETWEEN TRUNC(SYSDATE -30) AND TRUNC(SYSDATE +1)  \r";
                SQL += "   AND BDATE >= TRUNC(SYSDATE -30) \r";
                SQL += "   AND CERTNO IS NULL  \r";     //'전사서명 않된오더
                SQL += "   AND PRINT ='Y'  \r";
                SQL += "   AND ROWNUM <='100'  \r";     //'100건식
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BI"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUMIN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["orderno"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REALQTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DOSCODE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.HIC_HYANG, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_PMPA, clsCertWork.HIC_HYANG, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.HIC_HYANG;

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

        /// <summary>
        /// 건진 향정,마약 오더
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_HIC_HYANG_APPROVE(PsmhDb pDbCon)
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

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = "";
                SQL += "SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, \r";
                SQL += "       WRTNO,PANO,SNAME,JONG,GBSITE,DEPTCODE,SUCODE,QTY,REALQTY,PTNO,SEX,AGE, \r";
                SQL += "       TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE,DRSABUN, \r";
                SQL += "       TO_CHAR(APPROVETIME,'YYYY-MM-DD HH24:MI') APPROVETIME,GBSLEEP,JUMIN2,ROWID  \r";
                SQL += "  FROM KOSMOS_PMPA.HIC_HYANG_APPROVE  \r";
                SQL += " WHERE SDATE >= TRUNC(SYSDATE - 20)  \r";
                SQL += "   AND DRSABUN IS NOT NULL  \r";
                SQL += "   AND APPROVETIME IS NOT NULL  \r";
                SQL += "   AND CERTNO IS NULL  \r";         //'전사서명 않된오더
                SQL += "   AND ROWNUM <= 100  \r";          //'100건식
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["SDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + string.Format("{0:#0}", VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim())) + "|";
                        strEMRData = strEMRData + string.Format("{0:#0}", VB.Val(dt.Rows[i]["PANO"].ToString().Trim())) + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + dt.Rows[i]["JONG"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBSITE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUCODE"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + string.Format("{0:#0.00}", VB.Val(dt.Rows[i]["QTY"].ToString().Trim())) + "|";
                        strEMRData = strEMRData + string.Format("{0:#0.00}", VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim())) + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + string.Format("{0:#0}", VB.Val(dt.Rows[i]["AGE"].ToString().Trim())) + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + string.Format("{0:#0}", VB.Val(dt.Rows[i]["DRSABUN"].ToString().Trim())) + "|";
                        strEMRData = strEMRData + dt.Rows[i]["APPROVETIME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBSLEEP"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUMIN2"].ToString().Trim() + "|";
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.HIC_HYANG_APPROVE, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_PMPA, clsCertWork.HIC_HYANG_APPROVE, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                //ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.HIC_HYANG_APPROVE;

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

        /// <summary>
        /// 방사선 판독
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_XRAY_RESULTNEW(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'입원 오더는 오더 발생하면 무조건 전자 서명 발생 시킴
                SQL = "";
                SQL += "SELECT WRTNO,PANO,READDATE,SEEKDATE,XJONG,SNAME,SEX,AGE,IPDOPD,DEPTCODE,DRCODE, \r";
                SQL += "       WARDCODE,ROOMCODE,XDRCODE1,XDRCODE2,XDRCODE3,ILLCODE1,ILLCODE2,ILLCODE3,XCODE,  \r";
                SQL += "       XNAME,RESULT,RESULT1,PRTCNT,VIEWCNT,ENTDATE,APPROVE,GBOUT,STIME,ETIME,RESULTEC,  \r";
                SQL += "       RESULTEC1,GBANAT,SENDEMR,SENDEMRNO,GBSPC,PART,ADDENDUM1,ADDENDUM2,ADDDATE,ADDDRCODE,ROWID  \r";
                SQL += "  FROM KOSMOS_PMPA.XRAY_RESULTNEW    \r";
                SQL += " WHERE READDATE >= TRUNC(SYSDATE -30)  \r";
                SQL += "   AND (CERTNO IS NULL OR Certno =0 )  \r";             //'전사서명 않된오더
                SQL += "   AND xDRCODE1 IS NOT NULL  \r";
                SQL += "   AND xDRCODE1 NOT IN ( 99001,99003,99004,99005 ) \r"; //'2015-06-17
                SQL += "   AND XJong <> '7'  \r";                               //'BMD 인증제외 - 입력결과 개인이 함
                #endregion

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
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDrCode = dt.Rows[i]["XDRCODE1"].ToString().Trim();
                        strDrSabun = ComFunc.SetAutoZero(strDrCode.Trim(), 5);

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["WRTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["pano"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["readdate"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEEKDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["XJONG"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IPDOPD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WARDCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["XDRCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["XDRCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["XDRCODE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ILLCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ILLCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ILLCODE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["XCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["XNAME"].ToString().Trim() + "|";

                        strTResult = dt.Rows[i]["RESULT"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strTResult = dt.Rows[i]["RESULT1"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strEMRData = strEMRData + dt.Rows[i]["PRTCNT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["VIEWCNT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["APPROVE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBOUT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["STIME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ETIME"].ToString().Trim() + "|";

                        strTResult = dt.Rows[i]["RESULTEC"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strTResult = dt.Rows[i]["RESULTEC1"].ToString().Trim();
                        strTResult = strTResult.Replace(ComNum.VBLF, ComNum.LF);
                        strEMRData = strEMRData + strTResult + "|";

                        strEMRData = strEMRData + dt.Rows[i]["GBANAT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDEMR"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDEMRNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBSPC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PART"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDENDUM1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDENDUM2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDDRCODE"].ToString().Trim();
                        #endregion

                        //// 처방의사 사번
                        //strDrSabun = clsVbfunc.GetOCSDrCodeSabun(pDbCon, strDrCode);
                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {                                
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.XRAY_RESULTNEW, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_PMPA, clsCertWork.XRAY_RESULTNEW, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["READDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["READDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.XRAY_RESULTNEW;

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

        /// <summary>
        /// 수술처방
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_ORAN_SLIP(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'수술처방발생 후 전송 및 수납된 데이타 인증
                SQL = "";
                SQL += "SELECT WRTNO,PANO,OPDATE,DEPTCODE,DRCODE,IPDOPD,CODEGBN,JEPCODE,SUCODE,SUBUN,  \r";
                SQL += "       GUBUN,QTY,GBSELF,GBSUSUL,GBNGT,GBSUNAP,BUCODE,OPROOM,OPTIMEFROM,SLIPSEND, ENTDATE,  \r";
                SQL += "       ENTSABUN , GUMECONFIRM, DRUG_CHULGO, GBOK, AUTO_SEND, CERTNO, ROWID  \r";
                SQL += "  FROM KOSMOS_PMPA.ORAN_SLIP  \r";
                SQL += " WHERE OPDATE >= TO_DATE('2014-01-01','YYYY-MM-DD') \r";
                SQL += "   AND SLIPSEND >= TRUNC(SYSDATE - 30)  \r";
                SQL += "   AND CERTNO IS NULL  \r";         //'전사서명 않된오더
                SQL += "   AND DRCODE IS NOT NULL  \r";
                #endregion

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
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDrCode = dt.Rows[i]["DRCODE"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["WRTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PANO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IPDOPD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CODEGBN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JEPCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IPDOPD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUCODE"].ToString().Trim() + "|";

                        strEMRData = strEMRData + dt.Rows[i]["SUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["QTY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBSELF"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBSUSUL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBNGT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBSUNAP"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPROOM"].ToString().Trim() + "|";

                        strEMRData = strEMRData + dt.Rows[i]["OPTIMEFROM"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SLIPSEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ENTSABUN"].ToString().Trim();
                        #endregion

                        // 처방의사 사번
                        strDrSabun = clsVbfunc.GetOCSDrCodeSabun(pDbCon, strDrCode);
                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {                                
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.ORAN_SLIP, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_PMPA, clsCertWork.ORAN_SLIP, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["OPDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["OPDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.ORAN_SLIP;

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

        /// <summary>
        /// 진료사실증명서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_ETC_WONSELU(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'수술처방발생 후 전송 및 수납된 데이타 인증
                SQL = "";                
                SQL += "SELECT ACTDATE, PANO, SEQNO, IPDOPD, DEPTCODE, BDATE, ILSU, REMARK, \r";
                SQL += "       JINCODE1, JINCODE2, JINCODE3, JINCODE4, SUSULCODE1, SUSULCODE2, \r";
                SQL += "       SUSULDATE, SUSULCODE3, SUSULCODE4, SUSULDATE2, SUSULDATE3, SUSULDATE4, \r";
                SQL += "       JINDAN, SUSUL, PRT, GUBUN, DRCODE, DELDATE, PRTDATE, CERTNO, REQCNT, ENG_GUBUN, PDUSE, ROWID \r";
                SQL += "  FROM KOSMOS_PMPA.ETC_WONSELU a \r";
                SQL += " WHERE ACTDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                SQL += " ORDER BY ACTDATE \r";
                #endregion

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
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDrSabun = dt.Rows[i]["DRCODE"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["ACTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Pano"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEQNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IPDOPD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bdate"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ILSU"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REMARK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINCODE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINCODE4"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULCODE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULCODE4"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULDATE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSULDATE4"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINDAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUSUL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PRT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DELDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PRTDATE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.ETC_WONSELU, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_PMPA, clsCertWork.ETC_WONSELU, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ACTDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.ETC_WONSELU;

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

        /// <summary>
        /// 일반진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI01(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI01 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGONOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIGO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["USE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEQNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PRTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI01, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI01, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI01;

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

        /// <summary>
        /// 상해진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI02(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI02 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["COMPANY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGONOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["INDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CAUSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PART"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINVIEW"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINSUR"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINADMISS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINACTION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINDINNER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIODDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIODGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIODILSU"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AFTER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["YN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIGO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PART2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI02, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI02, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI02;

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

        /// <summary>
        /// 병사용진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI03(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI03 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OFFICE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OFFICETEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SOLD1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SOLD2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SOLDID"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGONOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DXGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DXDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DXCAUSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DXDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DXSITE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PROFF"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DXVIEW"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["COUSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Normal"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TERM"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AFTER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();
                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI03, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI03, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI03;

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

        /// <summary>
        /// 사망진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI05(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI05 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WORK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FOOTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHPLACE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHOTHERS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHKIND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHREPORT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHSAIN1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHSAIN2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHSAIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHKITA"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIOD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPVIEW"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["opdate"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPDISS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDTIME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDCKIND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDCONTENT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDPLACE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDKITA"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDSTATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDSAYU"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHSAIN4"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIOD1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIOD2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIOD3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PERIOD4"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FOOTGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEATHGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["KINDGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CERTGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI05, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI05, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI05;

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

        /// <summary>
        /// 진료소견서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI08(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI08 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODENAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGONOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IPDOPD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ILSU"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIGO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIGO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FLAG"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ENTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI08, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI08, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI08;

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

        /// <summary>
        /// 진료회송서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI12(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI12 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGONOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBPRINT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["INDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ILSU"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["RTEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBPRINT2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION_ADD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["RET_NEW"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI12, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI12, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI12;

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

        /// <summary>
        /// 건강진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI14(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI14 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";                        
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIGO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["USE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM3_ETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI14, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI14, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI14;

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

        /// <summary>
        /// 진료의뢰서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI18(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI18 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HTEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGONOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TOHOS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TODEPT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TODR"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TODATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REQUEST"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBPRINT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBPREPRINT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI18, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI18, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI18;

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

        /// <summary>
        /// 장애인증명서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI19(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI19 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALCNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALADD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CUSTNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CUSTJUMIN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CUSTADD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TJUMIN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TREL1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TREL2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TCONTENT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TUSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["KORNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBPRINT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CUSTJUMIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TJUMIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI19, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI19, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI19;

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

        /// <summary>
        /// 장애진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI20(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT B.SABUN \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI20 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ADDR"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TPART1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TPART2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TPART3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TORIGIN1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TORIGIN2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TORIGIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TDATE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATES1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATEE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HNAME1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATES2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATEE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HNAME2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRMEMO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GRADE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GRADE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GRADE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAYU"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["RDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRLICNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GBPRINT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["bDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRLICNO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI20, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI20, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI20;

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

        /// <summary>
        /// 출생증명서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI21(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICNO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI21 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Name"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ZIPCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ZIPCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WRITEDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WRITESABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WRITENAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Licno"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FBIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FAGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FJOB"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MBIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MAGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MJOB"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MZIPCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MZIPCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MJUSO1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MJUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BPLACE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BPLACENAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BETC1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BETC2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BDATETIME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BSEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IsDate"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IEDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DSEQ"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNTM"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNTW"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNTD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNTDM"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNTDW"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DCNTDETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SCNT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SCNTL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SCNTD1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SCNTD2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BSTAT1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BSTAT2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BWEIGHT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DMANY"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["FBIRTH2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI21, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI21, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI21;

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

        /// <summary>
        /// 사산증명서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI22(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICNO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI22 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Name"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ZIPCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ZIPCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WRITEDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WRITESABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["WRITENAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Licno"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICNO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JANG1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JPART1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JWONIN1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JANG2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JPART2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JWONIN2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JANG3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JPART3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JWONIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITAL1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITAL2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINTEXT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REPAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REPANDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI22, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI22, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI22;

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

        /// <summary>
        /// 사산증명서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI23(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSE \r";
                SQL += "           AND B.FDATE <= BALDATE \r";
                SQL += "           AND B.TDATE >= BALDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI23 X \r";
                SQL += " WHERE BALDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY BALDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUBIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUAGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUJOB"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MONAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MOBIRTH"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MOAGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MOJOB"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUMOPOSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUMOPOSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUMOJUSO1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUMOJUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAPOSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAPOSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAJUSO1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAJUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAJANG"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAJANGETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SADATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAHH24"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAMI"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SASEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IMMM"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IMBLOOD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DA"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DACHULSAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DACHUL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DACHULMAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DACHULWOMAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DASA"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DASAMAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DASAWOMAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DASAETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAJONG"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAJONGETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SAWON"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IMBYUNG"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["IMBYUNGETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI23, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI23, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BALDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BALDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI23;

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

        /// <summary>
        /// 영문진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI24(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSE \r";
                SQL += "           AND B.FDATE <= BALDATE \r";
                SQL += "           AND B.TDATE >= BALDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI24 X \r";
                SQL += " WHERE BALDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY BALDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGNOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TREATMENT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DURATIONOFTREATMENT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AAA"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DDD"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI24, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI24, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BALDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BALDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI24;

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

        /// <summary>
        /// 근로능력평가용진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI25(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSE \r";
                SQL += "           AND B.FDATE <= BALDATE \r";
                SQL += "           AND B.TDATE >= BALDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI25 X \r";
                SQL += " WHERE BALDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY BALDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO4"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AA"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["KCD1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKTDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKFDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["a"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["KCD2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKTDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKFDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["b"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CURE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["c"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TARGETSICK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TARGETSICKSTEP1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TARGETSICK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TARGETSICKSTEP2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["d"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CUREPLAN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CUREPLANETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGNOSISYESNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGNOSISYESNOETC"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGNOSISDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["e"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITALJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITALTEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKDETAIL1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKDETAIL2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CURE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CURE3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI25, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI25, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BALDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BALDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI25;

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

        /// <summary>
        /// 의료급여의뢰서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI26(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSE \r";
                SQL += "           AND B.FDATE <= BALDATE \r";
                SQL += "           AND B.TDATE >= BALDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI26 X \r";
                SQL += " WHERE BALDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY BALDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ORGANMARK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ORGANNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEJUMIN1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEJUMIN2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUJUMIN1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUJUMIN2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKMARK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKTDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKFDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICKGUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITALJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAGNOSIS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SELECTION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["NONSELECTION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEJUMIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SUJUMIN3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI26, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI26, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BALDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BALDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI26;

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

        /// <summary>
        /// 응급환자진료의뢰서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI27(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                SQL += "           AND B.FDATE <= LSDATE \r";
                SQL += "           AND B.TDATE >= LSDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI27 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PName"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["INDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BEFORE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AFTER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TREATMENT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TREASON"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DOCUMENTS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CARNUMBER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PASSENGER"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["OPINION"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSENO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI27, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI27, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI27;

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

        /// <summary>
        /// 포스코 진단서
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI28(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID, \r";
                SQL += "       (SELECT MAX(B.SABUN) \r";
                SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                SQL += "         WHERE A.SABUN = B.SABUN \r";
                SQL += "           AND A.MYEN_BUNHO = X.LICENSE \r";
                SQL += "           AND B.FDATE <= BALDATE \r";
                SQL += "           AND B.TDATE >= BALDATE \r";
                SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI28 X \r";
                SQL += " WHERE BALDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY BALDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["POSTCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["GUBUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BUSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_01"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_02"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_03"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_04"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_05"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_06"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_07"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_08"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_09"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_10"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM01_REMARK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_01"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_02"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_03"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_04"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_05"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_06"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_07"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM02_REMARK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM03_01"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM03_02"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM03_03"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM03_04"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM03_05"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM03_REMARK"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["HOSPITAL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EXAM_DATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["RESULT_DATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LICENSE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PRT_GB"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PRT_DATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CHK_EXAM02_01"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CHK_EXAM02_02"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CHK_EXAM02_03"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["CHK_EXAM02_04"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI28, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI28, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BALDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["BALDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI28;

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

        /// <summary>
        /// 근로능력
        /// </summary>
        /// <param name="pDbCon"></param>
        private void AUTO_CERT_OCS_MCCERTIFI29(PsmhDb pDbCon)
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
            string strTResult = string.Empty;

            ComFunc.ReadSysDate(clsDB.DbCon);
            string CERTDATE = clsPublic.GstrSysDate.Replace("-", "");
            lblFrDate.Text = string.Concat(clsPublic.GstrSysDate, " ", clsPublic.GstrSysTime);

            try
            {
                #region // QUERY
                //'진단서는 작성되면 무조건 전자서명 발생시킴(2013-06-01 시행)
                SQL = "";
                SQL += "SELECT          \r";
                SQL += "       X.*,     \r";
                SQL += "       X.ROWID  \r";
                //SQL += "       (SELECT B.SABUN \r";
                //SQL += "          FROM KOSMOS_ADM.INSA_MST A, KOSMOS_OCS.OCS_DOCTOR B \r";
                //SQL += "         WHERE A.SABUN = B.SABUN \r";
                //SQL += "           AND A.MYEN_BUNHO = X.LICENSENO \r";
                //SQL += "           AND B.FDATE <= LSDATE \r";
                //SQL += "           AND B.TDATE >= LSDATE \r";
                //SQL += "       ) AS DRSABUN \r";
                SQL += "  FROM KOSMOS_OCS.OCS_MCCERTIFI29 X \r";
                SQL += " WHERE LSDATE BETWEEN TO_DATE('2013-06-01', 'YYYY-MM-DD') AND TRUNC(SYSDATE) \r";
                SQL += "   AND CERTNO IS NULL \r";
                //SQL += "   AND SEND = 'P' \r";
                SQL += " ORDER BY LSDATE \r";
                #endregion

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
                        strDrSabun = dt.Rows[i]["DRSABUN"].ToString().Trim();

                        #region // EMRDATA 생성
                        strEMRData = dt.Rows[i]["MCCLASS"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PTNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["MCNO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEX"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BIRTHDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AGE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ZIPCODE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ZIPCODE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JUSO2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Jumin3"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SENDDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SEND"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["PRTDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["REQCNT"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCDODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["Licno"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DRSABUN"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSDATE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSTEL"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSJUSO"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["LSNAME"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DETAILSICK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["KCD1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAG1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TUYAK1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ETC1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AS1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ASETC1"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SICK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DETAILSICK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["KCD2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["BALDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["JINDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["SDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["EDATE2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DIAG2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TUYAK2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ETC2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["AS2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["ASETC2"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "|";
                        strEMRData = strEMRData + dt.Rows[i]["TEL"].ToString().Trim();

                        #endregion

                        clsCertWork.READ_TOIDAY(pDbCon, strDrSabun, ref strName, ref strToiday);
                        clsDB.setBeginTran(pDbCon);
                        try
                        {                            
                            if (strOK.Equals("OK"))
                            {
                                // 전자인증
                                strError = clsCertWork.CertWorkBacth(pDbCon, strDrSabun, CERTDATE, clsCertWork.OCS_MCCERTIFI29, strPano, strEMRData, ref strHashData, ref strSignData, ref strCertData, ref nCertno);
                                if (strError != "OK")
                                {
                                    strOK = "NO";
                                    strError = strError + " : " + strEMRData;
                                }
                            }

                            if (strOK.Equals("OK"))
                            {
                                if (clsCertWork.UPDATE_OCS_TABLE(pDbCon, ComNum.DB_MED, clsCertWork.OCS_MCCERTIFI29, clsCertWork.CERTDATE, CERTDATE, clsCertWork.CERTNO, nCertno, strRowid) == false)
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
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["LSDATE"].ToString().Trim() != "" ? Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString() : "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = clsCertWork.OCS_MCCERTIFI29;

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

        private void frmCertPoolOcsMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
