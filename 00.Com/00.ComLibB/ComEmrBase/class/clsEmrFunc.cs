using ComBase; //기본 클래스
using ComBase.Controls;
using ComDbB; //DB연결
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComEmrBase
{
    public class clsEmrFunc
    {
        /// <summary>
        /// 인증서 점검
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        public static void UserCertCheck(PsmhDb pDbCon, Form frm)
        {
            //2021-10-26 임시 해제
            return; 

            if (clsType.User.AuAMANAGE.Equals("1"))
                return;

            StringBuilder SQL = new StringBuilder();
            OracleDataReader reader = null;

            #region 쿼리
            SQL.AppendLine("SELECT BASCD                                                     ");
            SQL.AppendLine("  FROM KOSMOS_PMPA.BAS_BASCD                                     ");
            SQL.AppendLine(" WHERE GRPCDB = '간호EMR 관리'                                    ");
            SQL.AppendLine("    AND GRPCD = '인증점검 제외'                                    ");
            SQL.AppendLine("    AND BASCD = '" + clsType.User.Sabun + "'                     ");
            #endregion

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
            if (SqlErr.NotEmpty())
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon);
                return;
            }

            try
            {
                if (reader.HasRows)
                {
                    reader.Dispose();
                    reader = null;
                    return;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), pDbCon);
            }

         

            //==============================================================================
            //1.API 초기화 : API_INIT
            //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
            //3.인증
            //4.해제
            //인증을 할때마다 반복을 한다..VB에 코딩이 되어 있음.
            //==============================================================================
            string SID = string.Empty;
            string CERTPASS = string.Empty;
            string ErrMsg = string.Empty;

            try
            {

                //1.API 초기화 : API_INIT
                if (clsCertWork.API_INIT("192.168.100.33", "20011", "192.168.100.33", "20011", "hospitalcode_001", 0) == false)
                {
                    ErrMsg = "API 초기화 실패";
                    clsDB.SaveSqlErrLog("전자인증 오류", ErrMsg, clsDB.DbCon);
                    return;
                }

                //2.인증서 ID, Pawword 찾기
                clsCertWork.GetCertIdAndPassword(pDbCon, clsType.User.Sabun, ref SID, ref CERTPASS);
                if (SID.IsNullOrEmpty() || CERTPASS.IsNullOrEmpty())
                {
                    clsType.User.AuAWRITE = "0"; //기록 못하게 막음.
                    clsCertWork.API_RELEASE();
                    clsDB.SaveSqlErrLog("전자인증 오류", ErrMsg, clsDB.DbCon);
                    ErrMsg = "귀하의 전자서명 미등록 또는 갱신 만료일이 지났습니다.\r\n지금부터는 기록이 불가하오니\r\n의료정보팀(8041)로 즉시 연락하여 주시기 바랍니다.";
                    ComFunc.MsgBoxEx(frm, ErrMsg);
                    return;
                }

                //2.인증서 로그인 : ROAMING_NOVIEW_FORM : sid read '주민번호로 통일, CertPass
                if (clsCertWork.ROAMING_NOVIEW_FORM(SID, CERTPASS) == false)
                {
                    clsType.User.AuAWRITE = "0"; //기록 못하게 막음.
                    clsCertWork.API_RELEASE();
                    ErrMsg = "귀하의 전자서명 미등록 또는 갱신 만료일이 지났습니다.\r\n지금부터는 기록이 불가하오니\r\n의료정보팀(8041)로 즉시 연락하여 주시기 바랍니다.";
                    clsDB.SaveSqlErrLog("전자인증 오류", ErrMsg, clsDB.DbCon);
                    ComFunc.MsgBoxEx(frm, ErrMsg);
                    return;
                }

                #region 인증서 유효기간 점검
                DateTime StartDate = new DateTime(9999, 12, 31, 0, 0, 0, 0);
                DateTime EndDate = new DateTime(9999, 12, 31, 0, 0, 0, 0);

                clsCertWork.GETCERT_TERM(SID, ref StartDate, ref EndDate);
                if (StartDate.Year != 9999 && EndDate.Year != 9999)
                {
                    DateTime dtpNow = ComQuery.CurrentDateTime(pDbCon, "S").To<DateTime>();
                    if (EndDate.Date >= dtpNow.Date && (EndDate.Date - dtpNow.Date).TotalDays <= 3)
                    {
                        clsCertWork.API_RELEASE();
                        clsDB.SaveSqlErrLog("전자인증 오류", ErrMsg, clsDB.DbCon);
                        ErrMsg = "병원내 전자서명 갱신 만료일이 3일 남았습니다.\r\n만료일 이후 기록이 불가하오니 의료정보팀(8041)으로\r\n연락바랍니다.";
                        ComFunc.MsgBoxEx(frm, ErrMsg);
                        return;
                    }
                }
                #endregion

                clsCertWork.API_RELEASE();
                return;
            }
            catch (Exception ex)
            {
                clsCertWork.API_RELEASE();
                ErrMsg = ex.Message;
                clsDB.SaveSqlErrLog("전자인증 오류", ErrMsg, clsDB.DbCon);
                return;
            }
        }

        public static bool NowEmrCert(PsmhDb pDbCon, string MedFrDate, string PTNO)
        {
            //2021-08-30 미사용 주석
            return false;

            if (MedFrDate.IsNullOrEmpty() || PTNO.IsNullOrEmpty())
            {
                return false;
            }

            if (PTNO.Length > 3 && PTNO.Left(2).Equals("81"))
            {
                return false;
            }

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            bool rtnVal = false;
            OracleDataReader reader = null;

            #region 쿼리
            SQL.AppendLine("SELECT EMRNO                                                     ");
            SQL.AppendLine("  FROM KOSMOS_EMR.AEMRCHARTMST                                   ");
            SQL.AppendLine(" WHERE PTNO = '" + PTNO + "'                                     "); 
            SQL.AppendLine("    AND MEDFRDATE = '" + MedFrDate + "'                          ");
            if (ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().Hour <= 7)
            {
                SQL.AppendLine("    AND WRITEDATE >= TO_CHAR(SYSDATE - 1, 'YYYYMMDD')        ");
            }
            else
            {
                SQL.AppendLine("    AND WRITEDATE >= TO_CHAR(SYSDATE, 'YYYYMMDD')            ");
            }
            SQL.AppendLine("    AND WRITETIME >= '000000'                                    ");
            SQL.AppendLine("    AND CHARTUSEID <> '합계'                                      "); 
            SQL.AppendLine("    AND SAVECERT = '1' --인증저장 일경우만                          ");
            SQL.AppendLine("    AND CERTNO IS NULL -- 인증번호 없을경우만                       ");
            #endregion

            SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
            if (SqlErr.NotEmpty())
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon);
                return rtnVal;
            }

            try
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        double EmrNo = reader.GetValue(0).To<double>();
                        clsEmrQuery.SaveEmrCert(pDbCon, EmrNo);
                    }
                }

                reader.Dispose();
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), pDbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 오늘이 공휴일인지.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static bool IsNowHolyDay(PsmhDb pDbCon)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            bool rtnVal = false;
            OracleDataReader reader = null;

            SQL.AppendLine("SELECT 1                                               ");
            SQL.AppendLine("FROM DUAL                                              ");
            SQL.AppendLine("WHERE EXISTS                                           ");
            SQL.AppendLine("(                                                      ");
            SQL.AppendLine("    SELECT 1                                           ");
            SQL.AppendLine("      FROM KOSMOS_PMPA.BAS_JOB                         ");
            SQL.AppendLine("     WHERE JOBDATE = TRUNC(SYSDATE)                    ");
            SQL.AppendLine("       AND HOLYDAY = '*'                               ");
            SQL.AppendLine(")                                                      ");

            SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }


        /// <summary>
        /// 해당 내원내역으로 오늘 이후 변환된 내역이 있는지.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="TreatNo"></param>
        /// <returns></returns>
        public static bool IsImageCvt(PsmhDb pDbCon, string TreatNo)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            bool rtnVal = false;
            OracleDataReader reader = null;


            SQL.AppendLine("WITH MAX_CVTDATE AS                                                                        ");
            SQL.AppendLine("(                                                                                          ");
            SQL.AppendLine("    SELECT  MAX(CVTDATE) CVTDATE                                                           ");
            SQL.AppendLine("        ,   MAX(PANO) PANO                                                                 ");
            SQL.AppendLine("        ,   MAX(B.INDATE) INDATE                                                           ");
            SQL.AppendLine("        ,   COALESCE(MAX(B.OUTDATE), TO_CHAR(SYSDATE, 'YYYYMMDD')) OUTDATE                 ");
            SQL.AppendLine("      FROM KOSMOS_EMR.EMR_IMG_CVT_HISTORY A                                                ");
            SQL.AppendLine("        INNER JOIN KOSMOS_EMR.EMR_TREATT B                                                 ");
            SQL.AppendLine("           ON A.TREATNO = B.TREATNO                                                        ");
            SQL.AppendLine("	 WHERE A.TREATNO =  " + TreatNo                                                         );
            SQL.AppendLine("     --  AND A.CVTDATE >= TRUNC(SYSDATE)   	 	                                           ");
            SQL.AppendLine(")                                                                                          ");
            SQL.AppendLine(", RESULT_DATE AS                                                                           ");
            SQL.AppendLine("(                                                                                          ");
            SQL.AppendLine("SELECT MAX(A.RESULTDATE) RESULTDATE                                                        ");
            SQL.AppendLine("FROM KOSMOS_OCS.EXAM_SPECMST A                                                             ");
            SQL.AppendLine("  INNER JOIN MAX_CVTDATE B                                                                 ");
            SQL.AppendLine("     ON A.PANO = B.PANO                                                                    ");
            SQL.AppendLine("WHERE A.BDATE >= TO_DATE(B.INDATE, 'YYYY-MM-DD')                                           ");
            SQL.AppendLine("  AND A.BDATE <= TO_DATE(B.OUTDATE, 'YYYY-MM-DD')                                          ");
            SQL.AppendLine("  AND A.STATUS = '05'                                                                      ");
            SQL.AppendLine("  AND A.ANATNO IS NULL                                                                     ");
            SQL.AppendLine("  AND A.RESULTDATE IS NOT NULL                                                             ");
            SQL.AppendLine("  AND A.RESULTDATE >= B.CVTDATE                                                            ");
            SQL.AppendLine("UNION ALL                                                                                  ");
            SQL.AppendLine("SELECT MAX(A.RESULTDATE) RESULTDATE                                                        ");
            SQL.AppendLine("FROM KOSMOS_OCS.EXAM_ANATMST A                                                             ");
            SQL.AppendLine("  INNER JOIN KOSMOS_OCS.EXAM_SPECMST B                                                     ");
            SQL.AppendLine("     ON A.SPECNO = B.SPECNO                                                                ");
            SQL.AppendLine("  INNER JOIN MAX_CVTDATE C                                                                 ");
            SQL.AppendLine("     ON A.PTNO = C.PANO                                                                    ");
            SQL.AppendLine("WHERE A.BDATE >= TO_DATE(C.INDATE, 'YYYY-MM-DD')                                           ");
            SQL.AppendLine("  AND A.BDATE <= TO_DATE(C.OUTDATE, 'YYYY-MM-DD')                                          ");
            SQL.AppendLine("  AND A.GBJOB = 'V'                                                                        ");
            SQL.AppendLine("  AND A.RESULTDATE IS NOT NULL                                                             ");
            SQL.AppendLine("  AND A.RESULTDATE >= C.CVTDATE                                                            ");
            SQL.AppendLine("UNION ALL                                                                                  ");
            SQL.AppendLine("SELECT MAX(RESULTDATE) RESULTDATE                                                          ");
            SQL.AppendLine("FROM KOSMOS_OCS.EXAM_VERIFY A                                                              ");
            SQL.AppendLine("  INNER JOIN MAX_CVTDATE C                                                                 ");
            SQL.AppendLine("     ON A.PANO = C.PANO                                                                    ");
            SQL.AppendLine("WHERE A.INDATE >= TO_DATE(C.INDATE, 'YYYY-MM-DD')                                          ");
            SQL.AppendLine("  AND A.INDATE <= TO_DATE(C.OUTDATE, 'YYYY-MM-DD')                                         ");
            SQL.AppendLine("  AND A.STATUS = '3'                                                                       ");
            SQL.AppendLine("  AND A.RESULTDATE IS NOT NULL                                                             ");
            SQL.AppendLine("  AND A.RESULTDATE >= C.CVTDATE                                                            ");
            SQL.AppendLine(")                                                                                          ");
            SQL.AppendLine(" SELECT 1                                                                                  ");
            SQL.AppendLine("   FROM RESULT_DATE                                                                        ");
            SQL.AppendLine("  WHERE RESULTDATE IS NOT NULL                                                             ");
            SQL.AppendLine("UNION ALL                                                                                  ");
            SQL.AppendLine(" SELECT 1                                                                                  ");
            SQL.AppendLine("   FROM MAX_CVTDATE                                                                        ");
            SQL.AppendLine("  WHERE CVTDATE IS NULL                                                                    ");
            
            SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// 퇴원약 없음
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void Set_FormPatInfo_OutDrugCheck(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm)
        {

            DataTable dt = null;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_OutDrugCheck(AcpEmr);
            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            Control control = frm.Controls.Find("I0000035338", true).FirstOrDefault();
            string strVal = string.Empty;
            if (dt.Rows.Count > 0)
            {
                strVal = dt.Rows[0]["ITEMVALUE"].ToString().Trim();
            }

            dt.Dispose();

            if (control is CheckBox)
            {
                (control as CheckBox).Checked = strVal.Equals("1");
            }
            else if (control is RadioButton)
            {
                (control as RadioButton).Checked = strVal.Equals("1");
            }
        }

        /// <summary>
        /// 혈액 종류(코드) 정확한지 확인.
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static bool Blood_Code(string Code)
        {
            bool rtnVal = false;
            if (string.IsNullOrWhiteSpace(Code))
                return rtnVal;

            Dictionary<string, string> pstrBlood = new Dictionary<string, string>();
            pstrBlood.Clear();
            pstrBlood.Add("BT021", "P/C(농축적혈구)");
            pstrBlood.Add("BT041", "FFP(신선동결혈장)");
            pstrBlood.Add("BT023", "PLT/C(농축혈소판)");
            pstrBlood.Add("BT011", "W/B(전혈)");
            pstrBlood.Add("BT051", "Cyro(동결침전제제)");
            pstrBlood.Add("BT071", "W/RBC(세척적혈구)");
            pstrBlood.Add("BT101", "WBC/C(농축백혈구)");
            pstrBlood.Add("BT31",  "PRP(혈소판풍부혈장)");
            pstrBlood.Add("BT27",  "ph-P");
            pstrBlood.Add("BT24",  "ph-PLT");
            pstrBlood.Add("BT25",  "ph-WBC");
            pstrBlood.Add("BT26",  "ph-CB");
            pstrBlood.Add("BT081", "F/RBC(백혈구여과제거 적혈구)");

            rtnVal = pstrBlood.ContainsKey(Code);
            return rtnVal;
        }

        /// <summary>
        /// 완화의료 간호사 상담기록지(경과상담) - 초기 기록지에서 데이터 끌고오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        public static void Set_FormPatInfo_GETHUDATE(PsmhDb pDbCon, Form pForm, string strTag, EmrPatient pAcp)
        {
            #region 변수
            string SQL = FormPatInfoQuery.Query_Set_FormPatInfo_GETHUDATE(pAcp);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            string strDate = string.Empty;
            string strDate2 = string.Empty;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBoxEx(pForm, SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strDate  = reader.GetValue(0).ToString().Trim();
                    strDate2 = reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();

                if (string.IsNullOrWhiteSpace(strDate))
                    return;

                string[] strCtrl = strTag.Substring(strTag.IndexOf(":") + 1).Split(',');

                if (strCtrl.Length == 0)
                    return;

                Control control = pForm.Controls.Find(strCtrl[0], true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strDate;
                }

                if (strCtrl.Length < 2)
                    return;

                control = pForm.Controls.Find(strCtrl[1], true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strDate2;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(pForm, ex.Message);
            }

            return;
        }

        /// <summary>
        /// 내원기간안에 사망 했을경우 사망일시
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp">환자정보</param>
        public static void Set_FormPatInfo_DeathDate(PsmhDb pDbCon, Form pForm, string strTag, EmrPatient pAcp)
        {
            #region 변수
            string SQL = FormPatInfoQuery.Query_FormPatInfo_DeathDate(pAcp);
            string SqlErr = string.Empty;

            OracleDataReader reader = null;
            string strDate = string.Empty;
            string strTime = string.Empty;
            #endregion

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBoxEx(pForm, SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strDate = reader.GetValue(0).ToString().Trim();
                    strTime = reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();

                if (string.IsNullOrWhiteSpace(strDate))
                    return;

                string[] strCtrl = strTag.Substring(strTag.IndexOf(":") + 1).Split(',');

                if (strCtrl.Length == 0)
                    return;

                Control control = pForm.Controls.Find(strCtrl[0], true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strDate;
                }

                control = pForm.Controls.Find(strCtrl[1], true).FirstOrDefault();
                if (strCtrl.Length > 1 && control != null)
                {
                    control.Text = strTime;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(pForm, ex.Message);
            }

            return;
        }

        /// <summary>
        /// 해당 패널안에 텍스트를 모두 클리어 한다.
        /// </summary>
        /// <param name="pForm"></param>
        /// <param name="strTag"></param>
        public static void Set_PanelText_Clear(Form pForm, string strTag)
        {
            try
            {
                string strPanel = strTag.Substring(strTag.IndexOf(":") + 1);

                List<Control> controls = pForm.Controls.Find(strPanel, true).FirstOrDefault().Controls.Cast<Control>().Where(d => d is TextBox).ToList();
                if (controls.Count > 0)
                {
                   foreach(Control control in controls)
                    {
                        control.Text = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(pForm, ex.Message);
            }
        }

        /// <summary>
        /// 알레르기 항목에 자동 체크 후 해당 항목(음식, 약물항생제)에 뿌려준다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static void Set_FormPatInfo_AUTO_ALLERGY(PsmhDb pDbCon, Control pForm, EmrPatient pAcp)
        {
            OracleDataReader reader = null;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_ALLERGY(pAcp);

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    Control control = pForm.Controls.Find("I0000034276", true).FirstOrDefault();
                    if (control != null)
                    {
                        if (control is CheckBox)
                        {
                            (control as CheckBox).Checked = true;
                        }
                        else if (control is RadioButton)
                        {
                            (control as RadioButton).Checked = true;
                        }

                        Dictionary<string, string> keyAllergy = new Dictionary<string, string>();
                        keyAllergy.Add("005", "I0000034279_1");
                        keyAllergy.Add("003", "I0000034277_1");
                        keyAllergy.Add("100", "I0000035257_1");
                        while (reader.Read())
                        {
                            string rtnVal = string.Empty;

                            if (keyAllergy.TryGetValue(reader.GetValue(1).ToString().Trim(), out rtnVal))
                            {
                                control = pForm.Controls.Find(rtnVal, true).FirstOrDefault();
                                if (control != null)
                                {
                                    if (control is CheckBox)
                                    {
                                        (control as CheckBox).Checked = true;
                                    }
                                    else if (control is RadioButton)
                                    {
                                        (control as RadioButton).Checked = true;
                                    }
                                }

                                control = pForm.Controls.Find(rtnVal.Replace("_1", "_2"), true).FirstOrDefault();
                                if (control != null)
                                {
                                    control.Text += reader.GetValue(0).ToString().Trim() + ", ";
                                }
                            }
                        }
                    }
                }
                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 수혈기록지 종료시간 자동 연동(구분에 종료 입력했으면 자동으로 측정일자 시간 종료 시간에 연동)
        /// </summary>
        /// <returns></returns>
        public static void Set_Blood_TimeEnd(Form frm, string strTag)
        {
            try
            {
                //[0]: 바이탈 패널, [1]: 종료일자, [2]: 종료시간
                string[] strArr = strTag.Substring(strTag.IndexOf(":") + 1).Split(',');

                Control EndText = null;
                Control control = frm.Controls.Find(strArr[0], true).FirstOrDefault();
                if (control != null)
                {
                    EndText = ComFunc.GetAllControls(control).Where(d => d is TextBox && 
                    d.Name.IndexOf("I0000037557") != -1 &&
                    d.Text.Trim().Equals("종료")).FirstOrDefault();

                    if (EndText == null)
                        return;

                    string ConNo = EndText.Name.Split('_')[1];

                    string strDate = control.Controls.Find("I0000037555_" + ConNo, true).FirstOrDefault().Text.Trim();
                    string strTime = control.Controls.Find("I0000037556_" + ConNo, true).FirstOrDefault().Text.Trim();

                    if (string.IsNullOrWhiteSpace(strDate) == false && string.IsNullOrWhiteSpace(strTime) == false)
                    {
                        control = frm.Controls.Find(strArr[1], true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = strDate;
                        }

                        control = frm.Controls.Find(strArr[2], true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = strTime;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(frm, ex.Message);
                return;
            }
        }

        /// <summary>
        /// 출생정보 기록지 => 간호정보 조사지 연동(분만실, 신생아)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Doc"></param>
        /// <param name="AcpEmr"></param>
        public static void setChartFormValueBaby_New(PsmhDb pDbCon, Control Doc, string strPano, string strChartDate, EmrForm pForm)
        {
            if (pForm.FmOLDGB == 1)
                return;


            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN R.ITEMCD = 'I0000000399_1' THEN 'I0000034207' ";
            SQL = SQL + ComNum.VBLF + "              WHEN R.ITEMCD = 'I0000018981' THEN 'I0000034208' ";
            SQL = SQL + ComNum.VBLF + "              ELSE R.ITEMCD";
            SQL = SQL + ComNum.VBLF + "         END ITEMCD";
            SQL = SQL + ComNum.VBLF + "       , R.ITEMINDEX";
            SQL = SQL + ComNum.VBLF + "       , R.ITEMTYPE";
            SQL = SQL + ComNum.VBLF + "       , R.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
            SQL = SQL + ComNum.VBLF + "       ON R.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "      AND R.EMRNOHIS = A.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "      AND R.ITEMCD IN";
            SQL = SQL + ComNum.VBLF + "      (";
            SQL = SQL + ComNum.VBLF + "      'I0000034298', 'I0000034299', 'I0000034300', 'I0000034301', 'I0000034302', 'I0000034303', -- 산과력";
            SQL = SQL + ComNum.VBLF + "      'I0000016506', 'I0000019980', 'I0000030840', -- 산모 이름, 생년월일, 혈액형";
            SQL = SQL + ComNum.VBLF + "      'I0000000399_1', 'I0000018981',  -- 산모 전화번호, 국적";
            SQL = SQL + ComNum.VBLF + "      'I0000034209', 'I0000034210', 'I0000033870', -- 남편 이름, 생년월일, 혈액형";
            SQL = SQL + ComNum.VBLF + "      'I0000034211', 'I0000034212',   -- 남편 혈액형 전화번호, 국적";
            SQL = SQL + ComNum.VBLF + "      'I0000034298', 'I0000034299', 'I0000034300', 'I0000034301', 'I0000034302', 'I0000034303', -- 산과력 F P, A, AA, SA, L";
            SQL = SQL + ComNum.VBLF + "      'I0000034304', 'I0000034305', 'I0000034306', 'I0000034307', 'I0000034308',  -- 양막파수 무,유, 일, 시, 분";
            SQL = SQL + ComNum.VBLF + "      'I0000034295', 'I0000034294', 'I0000013182'  -- 임신중 약물복용 복용안함, 복용함, 복용약 입력 텍스트";
            //SQL = SQL + ComNum.VBLF + "      'I0000035002', 'I0000035003', 'I0000035004'  -- 임신기간, 주, 일, 모름 -- 불분명 주석";
            SQL = SQL + ComNum.VBLF + "      )";
            SQL = SQL + ComNum.VBLF + "    WHERE A.PTNO = '" + strPano + "'";
            SQL = SQL + ComNum.VBLF + "      AND A.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "      AND A.FORMNO = 2356";

            SQL = SQL + ComNum.VBLF + "UNION ALL ";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "         'I0000016506' AS  ITEMCD";
            SQL = SQL + ComNum.VBLF + "       , -1 AS ITEMINDEX";
            SQL = SQL + ComNum.VBLF + "       , 'TEXT' AS ITEMTYPE";
            SQL = SQL + ComNum.VBLF + "       , SNAME AS ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_PATIENT A";
            SQL = SQL + ComNum.VBLF + "    WHERE A.PANO = '" + strPano + "'";

            SQL = SQL + ComNum.VBLF + "UNION ALL ";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "         'I0000019980' AS  ITEMCD";
            SQL = SQL + ComNum.VBLF + "       , -1 AS ITEMINDEX";
            SQL = SQL + ComNum.VBLF + "       , 'TEXT' AS ITEMTYPE";
            SQL = SQL + ComNum.VBLF + "       , JUMIN1 AS ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_PATIENT A";
            SQL = SQL + ComNum.VBLF + "    WHERE A.PANO = '" + strPano + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Control control = Doc.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true).FirstOrDefault();
                    if (control != null)
                    {
                        if (control is TextBox)
                        {
                            control.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                        else if(control is CheckBox)
                        {
                            (control as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                        }
                        else if (control is RadioButton)
                        {
                            (control as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                        }

                        if (control.Visible == false)
                        {
                            PanelAutoSize(control.Parent, true);
                        }
                    }
                }
            }

            dt.Dispose();
        }

        /// <summary>
        /// 사번 입력시 소속 부서명, 사용자 성명 가져오기
        /// </summary>
        /// <returns></returns>
        public static string Set_GetSabunBuseName(PsmhDb pDbCon)
        {
            try
            {
                StringBuilder SQL = new StringBuilder();
                OracleDataReader reader = null;

                string strBuNmae = string.Empty;

                #region 쿼리
                SQL.Clear();
                SQL.AppendLine(" SELECT NAME");
                SQL.AppendLine("   FROM " + ComNum.DB_PMPA + "BAS_BUSE");
                SQL.AppendLine("  WHERE BUCODE = '" + clsType.User.BuseCode + "'");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    return string.Empty;
                }

                if (reader.HasRows && reader.Read())
                {
                    strBuNmae = reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();
                #endregion
                return strBuNmae;
              
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return string.Empty;
            }
        }


        /// <summary>
        /// 사번 입력시 소속 부서명, 사용자 성명 가져오기
        /// </summary>
        /// <returns></returns>
        public static void Set_GetSabunBuseName(PsmhDb pDbCon, Form frm, string strTag)
        {
            try
            {
                StringBuilder SQL = new StringBuilder();
                OracleDataReader reader = null;

                //[0]: 소속 부서넣을 아이템, [1]: 사용자 명 넣을 아이템
                string[] strArr = strTag.Substring(strTag.IndexOf(":") + 1).Split(',');
                string strStartDate = string.Empty;
                string strStartTime = string.Empty;
                string strSabun = string.Empty;

                string strBuNmae = string.Empty;
                string strKorNmae = string.Empty;

                Control control = frm.Controls.Find(strArr[0], true).FirstOrDefault();
                if (control != null)
                {
                    strSabun = control.Text.Trim();
                    #region 쿼리
                    SQL.Clear();
                    SQL.AppendLine(" SELECT A.EMP_NM, (SELECT NAME FROM KOSMOS_PMPA.BAS_BUSE WHERE TRIM(BUCODE) = TRIM(A.DEPT_CD)) AS NAME");
                    SQL.AppendLine("   FROM KOSMOS_ERP.HR_EMP_BASIS A");
                    SQL.AppendLine("  WHERE EMP_ID = '" + strSabun.PadLeft(5, '0') + "'");
                    SQL.AppendLine("    AND JOBKIND_CD = '41' -- 간호사만 조회되게");

                    string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
                    if (sqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(frm, sqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        strKorNmae = reader.GetValue(0).ToString().Trim();
                        strBuNmae  = reader.GetValue(1).ToString().Trim();
                    }

                    reader.Dispose();
                    #endregion
                    control.Text = strBuNmae;
                }

                control = frm.Controls.Find(strArr[1], true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strKorNmae;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(frm, ex.Message);
                return;
            }
        }

        /// <summary>
        /// 수혈기록지 시작일시로 부터 시간자동 계산
        /// 시작일자 아이템코드,측정일자 아이템코드,시간 아이템코드,구분 아이템코드
        /// </summary>
        /// <returns></returns>
        public static void Set_Blood_TimeCal(Form frm, string strTag)
        {
            try
            {
                //[0]: 시작일자, [1]: 시작시간, [2]: 측정일자, [3]: 측정시간, [4]: 구분
                string[] strArr = strTag.Substring(strTag.IndexOf(":") + 1).Split(',');
                string strStartDate = string.Empty;
                string strStartTime = string.Empty;


                Control control = frm.Controls.Find(strArr[0], true).FirstOrDefault();
                if (control != null)
                {
                    strStartDate = control.Text.Trim();
                }

                control = frm.Controls.Find(strArr[1], true).FirstOrDefault();
                if (control != null)
                {
                    strStartTime = control.Text.Trim();
                }

                //if (VB.IsNumeric(strStartDate + " " + strStartTime) == false)
                //{
                //    ComFunc.MsgBoxEx(frm, "시작 시간을 다시 확인해주세요");
                //    return;
                //}

                DateTime dtpStart = Convert.ToDateTime(strStartDate + " " + strStartTime);

                for (int i = 0; i < 10; i++)
                {
                    //구분
                    control = frm.Controls.Find(strArr[4] + "_" + (i + 1), true).FirstOrDefault();
                    if (control != null)
                    {
                        if (i == 0)
                        {
                            control.Text = "수혈 전";
                        }
                        else if(i == 1)
                        {
                            dtpStart = dtpStart.AddMinutes(15);
                            control.Text = "시작 15분 이내";
                        }
                        else
                        {
                            dtpStart = dtpStart.AddMinutes(30);
                            control.Text = "30분 후";
                        }
                    }

                    //측정일자
                    control = frm.Controls.Find(strArr[2] + "_" + (i + 1), true).FirstOrDefault();
                    if (control != null)
                    {
                        control.Text = dtpStart.ToString("yyyy-MM-dd");
                    }

                    //측정시간
                    control = frm.Controls.Find(strArr[3] + "_" + (i + 1), true).FirstOrDefault();
                    if (control != null)
                    {
                        control.Text = dtpStart.ToString("HH:mm");
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(frm, ex.Message);
                return;
            }
        }

        /// <summary>
        /// 팀장/수간호사/그외 책임간호사 등 메뉴/버튼 설정
        /// </summary>
        /// <returns></returns>
        public static bool GetMenuAuth(Form parent, PsmhDb pDbCon)
        {
            StringBuilder SQL = new StringBuilder();
            OracleDataReader reader = null;
            bool rtnVal = false;

            try
            {
                #region 쿼리
                SQL.Clear();
                SQL.AppendLine("SELECT 1 AS CNT");
                SQL.AppendLine("  FROM DUAL");
                SQL.AppendLine(" WHERE EXISTS");
                SQL.AppendLine(" (");
                SQL.AppendLine("    SELECT 1");
                SQL.AppendLine("      FROM " + ComNum.DB_PMPA + "BAS_BASCD");
                SQL.AppendLine("     WHERE GRPCDB = '간호EMR 관리'");
                SQL.AppendLine("       AND GRPCD  = '신규기록지 열람관리'");
                SQL.AppendLine("       AND BASCD  = '" + clsType.User.IdNumber + "'");
                SQL.AppendLine("     UNION ALL");
                SQL.AppendLine("    SELECT 1");
                SQL.AppendLine("      FROM KOSMOS_ERP.HR_EMP_BASIS ");
                SQL.AppendLine("     WHERE EMP_ID  = '" + clsType.User.Sabun + "'");
                SQL.AppendLine("       AND JOBPOSITION_CD IN('04', '13', '32') -- 부장, 팀장, 수간호사");
                SQL.AppendLine(" )");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(parent, sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();
                #endregion
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString().Trim(), pDbCon);
                ComFunc.MsgBoxEx(parent, ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 해당 내원내역에 가장 최근 V/S값을 뿌려준다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Doc"></param>
        /// <param name="emrPatient"></param>
        public static void Set_FormPatInfo_LastVS(PsmhDb pDbCon, Control Doc, EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            OracleDataReader reader = null;
            Dictionary<string, string> strBP = new Dictionary<string, string>();

            try
            {
                SQL = FormPatInfoQuery.Query_FormPatInfo_LastVS(emrPatient);
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }
              
                if (reader.HasRows)
                {
                    Control control = null;
                    while (reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim().Equals("I0000001765") || reader.GetValue(0).ToString().Trim().Equals("I0000002018"))
                        {
                            strBP.Add(reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim());
                        }

                        control = Doc.Controls.Find(reader.GetValue(0).ToString().Trim(), true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = reader.GetValue(1).ToString().Trim();
                        }
                    }

                    control = Doc.Controls.Find("I0000001809", true).FirstOrDefault();
                    //BP
                    if (control != null)
                    {
                        if (strBP.Count == 2)
                        {
                            string Val = string.Empty;
                            if (strBP.TryGetValue("I0000002018", out Val))
                            {
                                control.Text = Val;

                                if (strBP.TryGetValue("I0000001765", out Val))
                                {
                                    control.Text += "/" + Val;
                                }
                            }
                        }
                    }
                }

                reader.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }


        }

        public static bool IsNurseNA(PsmhDb pDbCon)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = ""; //에러문 받는 변수
            bool rtnVal = false;
            OracleDataReader reader = null;

            #region 간호조무사 대리처방 확인서 출력 위해서.
            SQL.AppendLine("SELECT 1");
            SQL.AppendLine("FROM DUAL");
            SQL.AppendLine("WHERE EXISTS");
            SQL.AppendLine("(");
            SQL.AppendLine("    SELECT 1");
            SQL.AppendLine("    FROM KOSMOS_PMPA.BAS_BASCD");
            SQL.AppendLine("    WHERE GRPCDB = '간호EMR 관리'");
            SQL.AppendLine("      AND GRPCD  = '간호조무사 관리'");
            SQL.AppendLine("      AND BASCD  = '" + clsType.User.Sabun + "'");
            SQL.AppendLine(")");

            SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read() && VB.Val(reader.GetValue(0).ToString().Trim()) > 0)
            {
                rtnVal = true;
            }

            reader.Dispose();
            #endregion

            return rtnVal;
        }

        public static bool IsErCoordinator(PsmhDb pDbCon)
        {
            StringBuilder SQL = new StringBuilder();
            string SqlErr = ""; //에러문 받는 변수
            bool rtnVal = false;
            OracleDataReader reader = null;

            #region 응급실 코디 검수완료 표시 사번 기초코드 연동
            SQL.AppendLine("SELECT 1");
            SQL.AppendLine("FROM DUAL");
            SQL.AppendLine("WHERE EXISTS");
            SQL.AppendLine("(");
            SQL.AppendLine("    SELECT 1");
            SQL.AppendLine("    FROM KOSMOS_PMPA.BAS_BASCD");
            SQL.AppendLine("    WHERE GRPCDB = '간호EMR 관리'");
            SQL.AppendLine("      AND GRPCD  = '응급실 코디 관리'");
            SQL.AppendLine("      AND BASCD  = '" + clsType.User.Sabun + "'");
            SQL.AppendLine(")");

            SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read() && VB.Val(reader.GetValue(0).ToString().Trim()) > 0)
            {
                rtnVal = true;
            }

            reader.Dispose();
            #endregion

            return rtnVal;
        }

        public static void Set_FormPatInfo_2751(PsmhDb pDbCon, Control Doc, EmrPatient emrPatient)
        {
            string SQL = FormPatInfoQuery.Query_FormPatInfo_2751(emrPatient);
            DataTable dt = null;

            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
                return;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Control control = Doc.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true).FirstOrDefault();
                if (control != null)
                {
                    if (control is TextBox)
                    {
                        control.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    }
                }
            }

            dt.Dispose();
        }

        public static void setChartFormValue2465_New(PsmhDb pDbCon, Control Doc, string Ptno, string OpDate, string OpWrtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT OPILL, PREDIAGNOSIS";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.WRTNO = " + OpWrtno;
            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Control control = Doc.Controls.Find("I0000014147", true).FirstOrDefault();
                if (control != null)
                {
                    //진단명
                    control.Text = dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000031507", true).FirstOrDefault();
                if (control != null)
                {
                    //시술명
                    control.Text = dt.Rows[0]["OPILL"].ToString().Trim();
                }
            }

            dt.Dispose();
        }

        public static void setChartFormValue1544_New(PsmhDb pDbCon, Control Doc, string Ptno, string OpDate, string OpWrtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT OPILL, PREDIAGNOSIS";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.WRTNO = " + OpWrtno;
            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Control control = Doc.Controls.Find("I0000014147", true).FirstOrDefault();
                if (control != null)
                {
                    //진단명
                    control.Text = dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000001429", true).FirstOrDefault();
                if (control != null)
                {
                    //수술명
                    control.Text = dt.Rows[0]["OPILL"].ToString().Trim();
                }
            }

            dt.Dispose();
        }

        public static void setChartFormValue2644_New(PsmhDb pDbCon, Control Doc, string Ptno, string OpDate, string OpWrtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT B.OPTITLE, OPSTIME, OPETIME";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.ORAN_MASTER B";
            SQL += ComNum.VBLF + "       ON A.WRTNO = B.WRTNO";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.WRTNO = " + OpWrtno;
            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Control control = Doc.Controls.Find("I0000031507", true).FirstOrDefault();
                if (control != null)
                {
                    //시술명
                    control.Text = dt.Rows[0]["OPTITLE"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000015584", true).FirstOrDefault();
                if (control != null)
                {
                    //도착시간
                    control.Text = dt.Rows[0]["OPSTIME"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000035709", true).FirstOrDefault();
                if (control != null)
                {
                    //퇴실시간
                    control.Text = dt.Rows[0]["OPETIME"].ToString().Trim();
                }
            }

            dt.Dispose();
        }

        /// <summary>
        /// 마취전 평가서
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Doc"></param>
        /// <param name="Ptno"></param>
        /// <param name="OpDate"></param>
        public static void setChartFormValue2610_New(PsmhDb pDbCon, Control Doc, string Ptno, string OpDate, string OpWrtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT A.PREDIAGNOSIS, A.OPILL, B.DIAGNOSIS AS DIAGNOSIS";
            SQL += ComNum.VBLF + "  ,B.GBER, OPTITLE, ASAADD, ANGBN";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.ORAN_MASTER B";
            SQL += ComNum.VBLF + "       ON A.WRTNO = B.WRTNO";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.WRTNO = " + OpWrtno;

            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                string strGetVal = string.Empty;

                #region 마취종류
                Dictionary<string, string> AnGubun = new Dictionary<string, string>
                {
                    { "G", "I0000022530" }, //General
                    { "MASK", "I0000031039" }, //Mask
                    { "MAC", "I0000010854" }, //MAC
                    { "S", "I0000030597" }, //Spinal
                    { "E", "I0000030598" }, //Epidural
                    { "C", "I0000033625" }, //Caudal
                    { "FNB", "I0000033632" }, //FNB
                    { "SNB", "I0000024579" }, //SNB
                    { "A", "I0000033633" }, //BPB(A)
                    { "B", "I0000033634" }, //BPB(I)
                    { "L", "I0000012570" }, //Local
                    { "Z", "I0000001067_1" } //None(기타)
                };
                #endregion

                #region ASA 분류
                Dictionary<string, string> ASAGubun = new Dictionary<string, string>
                {
                    { "1", "I0000011889" }, //1
                    { "2", "I0000011890" }, //2
                    { "3", "I0000011891" }, //3
                    { "4", "I0000011892" }, //4
                    { "5", "I0000011893" }, //5
                    { "6", "I0000011894" }  //6
                };
                #endregion

                Control control = Doc.Controls.Find("I0000031745", true).FirstOrDefault();
                if (control != null)
                {
                    //진단명
                    control.Text = dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000033630", true).FirstOrDefault();
                if (control != null)
                {
                    //수술명
                    control.Text = dt.Rows[0]["OPILL"].ToString().Trim();
                }

                if (dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim().Equals("*"))
                {
                    control = Doc.Controls.Find("I0000011303", true).FirstOrDefault();
                    if (control != null)
                    {
                        ((RadioButton)control).Checked = true;
                    }
                }
                else
                {
                    if (dt.Rows[0]["ASAADD"].ToString().Trim().Length > 0)
                    {
                        #region ASA 분류
                        if (ASAGubun.TryGetValue(dt.Rows[0]["ASAADD"].ToString().Trim(), out strGetVal))
                        {
                            control = Doc.Controls.Find(strGetVal, true).FirstOrDefault();
                            ((RadioButton)control).Checked = true;
                        }
                        #endregion
                    }

                }

                if (dt.Rows[0]["ANGBN"].ToString().Trim().Length > 0)
                {
                    #region 마취종류
                    if(AnGubun.TryGetValue(dt.Rows[0]["ANGBN"].ToString().Trim(), out strGetVal))
                    {
                        control = Doc.Controls.Find(strGetVal, true).FirstOrDefault();
                        ((RadioButton)control).Checked = true;
                    }
                    #endregion
                }

            }

            dt.Dispose();
        }



        /// <summary>
        /// 회복실 기록지2
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Doc"></param>
        /// <param name="Ptno"></param>
        /// <param name="OpDate"></param>
        public static void setChartFormValue2611_New(PsmhDb pDbCon, Control Doc, string Ptno, string OpDate, string OpWrtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT B.DIAGNOSIS AS DIAGNOSIS, OPTITLE, ANGBN";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.ORAN_MASTER B";
            SQL += ComNum.VBLF + "       ON A.WRTNO = B.WRTNO";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.WRTNO = " + OpWrtno;
            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                #region 마취종류
                Dictionary<string, string> AnGubun = new Dictionary<string, string>
                {
                    { "G", "I0000022530" }, //General
                    { "MASK", "I0000031039" }, //Mask
                    { "MAC", "I0000010854" }, //MAC
                    { "S", "I0000030597" }, //Spinal
                    { "E", "I0000030598" }, //Epidural
                    { "C", "I0000033625" }, //Caudal
                    { "FNB", "I0000033632" }, //FNB
                    { "SNB", "I0000024579" }, //SNB
                    { "A", "I0000033633" }, //BPB(A)
                    { "B", "I0000033634" }, //BPB(I)
                    { "L", "I0000012570" }, //Local
                    { "Z", "I0000001067_1" } //None(기타)
                };
                #endregion

                Control control = Doc.Controls.Find("I0000031746", true).FirstOrDefault();
                if (control != null)
                {
                    //진단명
                    control.Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000001429", true).FirstOrDefault();
                if (control != null)
                {
                    //수술명
                    control.Text = dt.Rows[0]["OPTITLE"].ToString().Trim();
                }

                control = Doc.Controls.Find("I0000031507_1", true).FirstOrDefault();
                if (control != null)
                {
                    //시술명
                    control.Text = dt.Rows[0]["OPTITLE"].ToString().Trim();

                    control = Doc.Controls.Find("I0000014147", true).FirstOrDefault();
                    if (control != null)
                    {
                        //진단명
                        control.Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                    }
                }


                string strGetVal = string.Empty;
                if (dt.Rows[0]["ANGBN"].ToString().Trim().Length > 0)
                {
                    #region 마취종류
                    if (AnGubun.TryGetValue(dt.Rows[0]["ANGBN"].ToString().Trim(), out strGetVal))
                    {
                        control = Doc.Controls.Find(strGetVal, true).FirstOrDefault();
                        if (control != null)
                        {
                            ((RadioButton)control).Checked = true;
                        }
                    }
                    #endregion
                }

            }

            dt.Dispose();
        }

        /// <summary>
        /// 회복실 기록지2 입실/퇴실 시간 및 바이탈 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="pAcp"></param>
        public static void setChartFormValue2611_New_Vital(PsmhDb pDbCon, Form frm, EmrPatient pAcp)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT  A.CHARTDATE, A.CHARTTIME, B.ITEMCD, B.ITEMVALUE";
            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO IN";
            SQL += ComNum.VBLF + "      (";
            SQL += ComNum.VBLF + "      'I0000002018', 'I0000001765', 'I0000014815', 'I0000002009', 'I0000001811', 'I0000008708'";
            SQL += ComNum.VBLF + "      )";
            SQL += ComNum.VBLF + "WHERE MEDFRDATE = '" + pAcp.medFrDate + "'";
            SQL += ComNum.VBLF + "  AND FORMNO = 2135";
            SQL += ComNum.VBLF + "  AND PTNO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + "  AND CHARTDATE = TO_CHAR(SYSDATE, 'YYYYMMDD')";
            SQL += ComNum.VBLF + "  AND CHARTTIME IN";
            SQL += ComNum.VBLF + "  (";
            SQL += ComNum.VBLF + "      SELECT MIN(A.CHARTTIME)";
            SQL += ComNum.VBLF + "        FROM KOSMOS_EMR.AEMRCHARTMST A";
            SQL += ComNum.VBLF + "       WHERE MEDFRDATE = '" + pAcp.medFrDate + "'";
            SQL += ComNum.VBLF + "         AND CHARTDATE = TO_CHAR(SYSDATE, 'YYYYMMDD')";
            SQL += ComNum.VBLF + "         AND FORMNO = 2135";
            SQL += ComNum.VBLF + "         AND PTNO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + "      UNION ALL ";
            SQL += ComNum.VBLF + "      SELECT MAX(A.CHARTTIME)";
            SQL += ComNum.VBLF + "        FROM KOSMOS_EMR.AEMRCHARTMST A";
            SQL += ComNum.VBLF + "       WHERE MEDFRDATE = '" + pAcp.medFrDate + "'";
            SQL += ComNum.VBLF + "         AND CHARTDATE = TO_CHAR(SYSDATE, 'YYYYMMDD')";
            SQL += ComNum.VBLF + "         AND FORMNO = 2135";
            SQL += ComNum.VBLF + "         AND PTNO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + "  )";
            SQL += ComNum.VBLF + " ORDER BY (CHARTDATE || CHARTTIME)";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Dictionary<string, string> strBP = new Dictionary<string, string>();

                string strVSTime = dt.Rows[0]["CHARTTIME"].ToString().Trim();                
                int sIndex = 1;

                //입실시간
                Control control = frm.Controls.Find("I0000035708", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = VB.Val(VB.Left(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (strVSTime.Equals(dt.Rows[i]["CHARTTIME"].ToString().Trim()) == false)
                    {
                        #region 입실 BP
                        control = frm.Controls.Find("I0000001809_" + sIndex, true).FirstOrDefault();
                        //BP
                        if (control != null)
                        {
                            if (strBP.Count == 2)
                            {
                                string Val = string.Empty;
                                if (strBP.TryGetValue("I0000002018", out Val))
                                {
                                    control.Text = Val;

                                    if (strBP.TryGetValue("I0000001765", out Val))
                                    {
                                        control.Text += "/" + Val;
                                    }
                                }
                            }
                            strBP.Clear();
                        }
                        #endregion
                        sIndex++;
                        strVSTime = dt.Rows[i]["CHARTTIME"].ToString().Trim();
                        //퇴실시간
                        control = frm.Controls.Find("I0000035709", true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = VB.Val(VB.Left(strVSTime, 4)).ToString("00:00");
                        }
                    }

                    if (dt.Rows[i]["ITEMCD"].ToString().Trim().IndexOf("I0000001765") != -1 || dt.Rows[i]["ITEMCD"].ToString().Trim().IndexOf("I0000002018") != -1)
                    {
                        strBP.Add(dt.Rows[i]["ITEMCD"].ToString().Trim(), dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                    }

                    control = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim() + "_" + sIndex, true).FirstOrDefault();
                    if (control != null)
                    {
                        control.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    }
                }

                #region 퇴실 BP
                control = frm.Controls.Find("I0000001809_" + sIndex, true).FirstOrDefault();
                //BP
                if (control != null)
                {
                    if (strBP.Count == 2)
                    {
                        string Val = string.Empty;
                        if (strBP.TryGetValue("I0000002018", out Val))
                        {
                            control.Text = Val;

                            if (strBP.TryGetValue("I0000001765", out Val))
                            {
                                control.Text += "/" + Val;
                            }
                        }
                    }
                }
                #endregion
            }

            dt.Dispose();
        }

        /// <summary>
        /// 환자 검사결과 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="emrPatient"></param>
        /// <param name="Tag"></param>
        public static void Set_FormPatInfo_ExamResult(PsmhDb pDbCon, Form frm, EmrPatient emrPatient)
        {
            string SQL = FormPatInfoQuery.Query_FormPatInfo_LabResult2(emrPatient);
            OracleDataReader reader = null;
            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            Dictionary<string, string> ConValues = new Dictionary<string, string>
            {
                { "Hb", "I0000001822" },
                { "Hct", "I0000001825" },
                { "PLT", "I0000003609" },
                { "WBC", "I0000002083" },
                { "Blood Type", "I0000011819" },
                { "PT", "I0000007170" },
                { "PTT", "I0000002582" },
                { "GOT", "I0000033617" },
                { "GPT", "I0000033618" }
            };


            if (reader.HasRows)
            {
                string ContrlNm = string.Empty;
                while (reader.Read())
                {
                    if (ConValues.TryGetValue(reader.GetValue(0).ToString().Trim(), out ContrlNm))
                    {
                        Control control = frm.Controls.Find(ContrlNm, true).FirstOrDefault();
                        if (control != null)
                        {
                            control.Text = reader.GetValue(1).ToString().Trim();
                        }
                    }
                }
            }

            reader.Dispose();
        }

        /// <summary>
        /// 특정컨트롤에 문자열 입력
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Tag"></param>
        public static void Set_ControlText(Form frm, Control ct, string Tag)
        {
            string[] Arr = Tag.Split(':');

            if (Arr.Length < 3)
                return;

            Control control = frm.Controls.Find(Arr[1], true).FirstOrDefault();
            if (control != null)
            {
                if (ct is RadioButton)
                {
                    if ((ct as RadioButton).Checked && control is TextBox)
                    {
                        control.Text = Arr[2];
                    }
                }
                else if (ct is CheckBox)
                {
                    if ((ct as CheckBox).Checked && control is TextBox)
                    {
                        control.Text = Arr[2];
                    }
                }
                else
                {
                    if (control is TextBox)
                    {
                        control.Text = Arr[2];
                    }
                }
            }
        }

        /// <summary>
        /// 신규서식지 아이템값 연동
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="emrPatient"></param>
        /// <param name="Tag"></param>
        public static void Set_FormPatInfo_GetItemValue(PsmhDb pDbCon, Form frm, EmrPatient emrPatient, string Tag)
        {
            string[] Arr = Tag.Split(':');

            if (Arr.Length < 4)
                return;

            string FormNo = Arr[1];
            string ItemCD = Arr[2];
            string ControlNm = Arr[3];

            string SQL = FormPatInfoQuery.Query_FormPatInfo_GetItemValue(emrPatient, FormNo, ItemCD);
            OracleDataReader reader = null;
            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                Control control = frm.Controls.Find(ControlNm, true).FirstOrDefault();
                if (control != null)
                {
                    if (control is TextBox)
                    {
                        control.Text = reader.GetValue(0).ToString().Trim();
                    }
                    else if (control is CheckBox)
                    {
                        (control as CheckBox).Checked = reader.GetValue(0).ToString().Trim().Equals("1");
                    }
                    else if (control is RadioButton)
                    {
                        (control as RadioButton).Checked = reader.GetValue(0).ToString().Trim().Equals("1");
                    }
                }
            }

            reader.Dispose();
        }

        /// <summary>
        /// 신규간호정보 조사지 연동 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strFlag"></param>
        public static void setChartFormValue2311_New(PsmhDb pDbCon, Form frm, EmrPatient emrPatient)
        {
            string SQL = FormPatInfoQuery.Query_FormPatInfo_New2311Value(emrPatient);
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            Control control = null;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
                return;

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                control = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true).FirstOrDefault();
                if(control != null)
                {
                    if (control is TextBox)
                    {
                        control.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    }
                    else if (control is CheckBox)
                    {
                        (control as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                    }
                    else if (control is RadioButton)
                    {
                        (control as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                    }
                }
            }

            dt.Dispose();
        }

        /// <summary>
        /// 엔터시 해당 컨트롤로 포커스 이동
        /// </summary>
        /// <param name="Tag"></param>
        public static void Set_NextFocus(Control Pcontrol, string Tag)
        {
            string NextControl = Tag.Substring(Tag.IndexOf(":") + 1);
            Control control = Pcontrol.Controls.Find(NextControl, true).FirstOrDefault();

            if (control == null)
                return;

            control.Focus();
        }


        /// <summary>
        /// 해당환자에게 가장 최근에 쓴 기록지를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_LASTEMRNO(PsmhDb pDbCon, EmrPatient emrPatient, EmrForm emrForm, string FormNo)
        {
            OracleDataReader reader = null;
            if (emrPatient == null)
                return "0";

            string SQL = emrForm.FmOLDGB == 1 ? FormPatInfoQuery.Query_FormPatInfo_OLDLASTEMRNO(emrPatient, FormNo) : FormPatInfoQuery.Query_FormPatInfo_NEWLASTEMRNO(emrPatient, FormNo);

            string rtnVal = string.Empty;

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }
                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 당일외래자 약처방/당일외래자 검사처방
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="emrPatient"></param>
        /// <param name="Tag"></param>
        public static void Set_FormPatInfo_Order(PsmhDb pDbCon, Form frm, EmrPatient emrPatient, string Tag)
        {
            string strDate = frm.Controls.Find("txtSearch", true).FirstOrDefault()?.Text.Trim();

            DataTable dt = null;

            string MedCtrlNm = Tag.Split(':')[1];
            string LabCtrlNm = Tag.Split(':')[2];

            if (frm.Controls.Find(MedCtrlNm, true).Length == 0 || frm.Controls.Find(LabCtrlNm, true).Length == 0)
                return;

            Control MedCtrl = frm.Controls.Find(MedCtrlNm, true).FirstOrDefault();
            Control LabCtrl = frm.Controls.Find(LabCtrlNm, true).FirstOrDefault();

            MedCtrl.Text = "";
            LabCtrl.Text = "";

            StringBuilder OrderData = new StringBuilder();

            string SQL = emrPatient.inOutCls.Equals("O") ? FormPatInfoQuery.Set_FormPatInfo_OpdMed(emrPatient, strDate) : FormPatInfoQuery.Set_FormPatInfo_IpdMed(emrPatient, strDate);

            try
            {
                #region 약
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        OrderData.AppendLine("■ " + dt.Rows[i]["ORDERNAMES"].ToString().Trim() + "   " + dt.Rows[i]["ORDERNAME"].ToString().Trim() + " " +
                            dt.Rows[i]["REALQTY"].ToString().Trim() + "     " +
                            dt.Rows[i]["QTY"].ToString().Trim() + "  " +
                            dt.Rows[i]["NAL"].ToString().Trim());
                    }

                    MedCtrl.Text = OrderData.ToString().Trim();
                }
                dt.Dispose();

                OrderData.Clear();
                #endregion

                #region 검사
                SQL = emrPatient.inOutCls.Equals("O") ? FormPatInfoQuery.Set_FormPatInfo_OpdLab(emrPatient, strDate) : FormPatInfoQuery.Set_FormPatInfo_IpdLab(emrPatient, strDate);
                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        OrderData.AppendLine("■ " + dt.Rows[i]["ORDERNAMES"].ToString().Trim() + "   " + dt.Rows[i]["ORDERNAME"].ToString().Trim());
                    }

                    LabCtrl.Text = OrderData.ToString().Trim();
                }
                dt.Dispose();
                #endregion

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// 여러개의 컨트롤 값이 맞을때 해당 '패널 혹은 컨트롤을' 숨기거나 보여준다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void Set_CheckValVisible(Form frm, string strTag)
        {
            string[] Arr = strTag.Split(':');
            int AllCount = 0;
            int chkCount = 0;

            for (int i = 1; i < Arr.Length - 1; i++)
            {
                if (Arr[i].ToUpper().Equals("TRUE") || Arr[i].ToUpper().Equals("FALSE"))
                {
                    Control control = frm.Controls.Find(Arr[i + 1], true).FirstOrDefault();
                    if (control != null && 
                        (control is CheckBox && (control as CheckBox).Checked.ToString().ToUpper().Equals(Arr[i].ToUpper()) ||
                         control is RadioButton && (control as RadioButton).Checked.ToString().ToUpper().Equals(Arr[i].ToUpper())))
                    {
                        chkCount++;
                    }
                }
                else
                {
                    if (i < Arr.Length)
                    {
                        AllCount++;
                    }
                    continue;
                }
            }

            if (chkCount == AllCount)
            {
                Control control = frm.Controls.Find(Arr[Arr.Length - 1], true).FirstOrDefault();
                if (control != null)
                {
                    control.Visible = Arr[Arr.Length - 2].ToUpper().Equals("TRUE");
                }
            }
        }


        /// <summary>
        /// 텍스트의 내용에 : 없다면 자동으로
        /// : 만들어서 00:00 형태로 만들어준다.
        /// </summary>
        public static void AutoTimeText(Control TxtBox)
        {
            if (TxtBox.Text.IndexOf(":") != -1)
                return;

            if (VB.IsNumeric(TxtBox.Text) && TxtBox.Text.Length >= 3)
            {                
                string Hour = TxtBox.Text.Substring(0, 2);
                string Minute = TxtBox.Text.Substring(2, TxtBox.Text.Length == 3 ? 1 : 2);
                TxtBox.Text = VB.Val(Hour).ToString("00") + ":" + VB.Val(Minute).ToString("00");
            }
        }

        /// <summary>
        /// 외래예약 날짜 넣는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static void Set_FormPatInfo_OPD_RESERVED(PsmhDb pDbCon, Form frm, EmrPatient emrPatient, string Tag)
        {
            OracleDataReader reader = null;

            string SQL = FormPatInfoQuery.Query_FormPatInfo_OPD_RESERVED(emrPatient);
            string[] Ctrl = Tag.Split(':');
            string PPanel = Ctrl[0];
            string PPanel2 = Ctrl[1];

            if (frm.Controls.Find(PPanel, true).Length == 0)
                return;


            Control control = frm.Controls.Find(PPanel, true).FirstOrDefault();

            //해당 패널 안에서 텍스트만 이름순으로 정렬해서 뽑아낸다.
            List<Control> textBoxes = ComFunc.GetAllControls(frm.Controls.Find(PPanel, true).FirstOrDefault()).Where(Ctl => Ctl is TextBox).OrderBy(Ctl => VB.Val(Ctl.Name.Substring(Ctl.Name.LastIndexOf("_") + 1))).ToList();

            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }
                

                if (reader.HasRows)
                {
                    int ArrCnt = 0;
                    while (reader.Read())
                    {
                        if (ArrCnt >= 5 || ArrCnt >= textBoxes.Count)
                            break;

                        string rtnVal = string.Empty;
                        textBoxes[ArrCnt].Text = string.Format("{0}/{1}/{2}", reader.GetValue(0).ToString().Trim(), Convert.ToDateTime(reader.GetValue(1).ToString().Trim()).ToString("yyyy-MM-dd HH:mm"), reader.GetValue(3).ToString().Trim());
                        ArrCnt += 1;
                    }

                    control.Visible = true;
                    PanelAutoSize(textBoxes[ArrCnt - 1].Parent, true);

                }
                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// 환자의 감염정보를 가져오고 해당 항목에 체크표시 한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static void Set_FormPatInfo_INFECT(PsmhDb pDbCon, Form frm, EmrPatient emrPatient, string Tag)
        {
            OracleDataReader reader = null;
            string CtlNm = Tag.Substring(Tag.IndexOf(":") + 1);
            if (frm.Controls.Find(CtlNm, true).Length == 0)
                return;

            Control Panel = frm.Controls.Find(CtlNm, true).FirstOrDefault();

            PanelAutoSize(Panel, true);

            string SQL = FormPatInfoQuery.Query_FormPatInfo_INFECT(emrPatient);
            Dictionary<string, string> InfectLst = new Dictionary<string, string>()
            {
                {"혈액주의", "I0000035367"},   //혈액주의
                {"접촉주의", "I0000035368"},   //접촉주의
                {"접촉(격리)주의", "I0000035368"},   //접촉주의
                {"공기주의", "I0000035369"},   //공기주의
                {"비말주의", "I0000035370"},   //비말주의
                {"보호격리", "I0000035371"},   //보호격리
                {"해외경유자", "I0000035372"}    //해외경유자
            };

            


            try
            {
                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    string strInfect = reader.GetValue(0).ToString().Trim();
                    List<string> strMaaping = new List<string>();
                    string rtnVal = string.Empty;
                    
                    #region 컨트로ㅓㄹ
                    if (strInfect.Substring(0, 1).Equals("1"))
                    {
                        strMaaping.Add("혈액주의");
                    }

                    if (strInfect.Substring(1, 1).Equals("1"))
                    {
                        strMaaping.Add("접촉주의");
                    }
                    if (strInfect.Substring(2, 1).Equals("1"))
                    {
                        strMaaping.Add("접촉(격리)주의");
                    }
                    if (strInfect.Substring(3, 1).Equals("1"))
                    {
                        strMaaping.Add("공기주의");
                    }
                    if (strInfect.Substring(4, 1).Equals("1"))
                    {
                        strMaaping.Add("비말주의");
                    }
                    if (strInfect.Substring(5, 1).Equals("1"))
                    {
                        strMaaping.Add("보호격리");
                    }
                    if (strInfect.Substring(6, 1).Equals("1"))
                    {
                        strMaaping.Add("해외경유자");
                    }
                    #endregion

                    for(int i = 0; i < strMaaping.Count; i++)
                    {
                        if (InfectLst.TryGetValue(strMaaping[i], out rtnVal))
                        {
                            Control control = Panel.Controls.Find(rtnVal, true).FirstOrDefault();
                            if (control != null)
                            {
                                if (control is CheckBox)
                                {
                                    (control as CheckBox).Checked = true;
                                }
                                else if (control is RadioButton)
                                {
                                    (control as RadioButton).Checked = true;
                                }
                            }
                        }
                    }
                }
                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// 곱하기 함수
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="ct"></param>
        /// <param name="Tag"></param>
        public static void Textmultiply(Form frm, string Tag)
        {
            string[] ArrCtrl = Tag.Split(':')[1].Split(',');
            double intTotSum = 0;
            for (int i = 0; i < ArrCtrl.Length; i++)
            {
                Control[] control = frm.Controls.Find(ArrCtrl[i].Trim(), true);
                if (control.Length > 0)
                {
                    //총점
                    if (i == ArrCtrl.Length - 1)
                    {
                        control[0].Text = intTotSum.ToString();
                    }
                    else
                    {
                        if (intTotSum == 0)
                        {
                            intTotSum = VB.Val(control[0].Text.Trim());
                        }
                        else
                        {
                            intTotSum *= VB.Val(control[0].Text.Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 빼기 함수
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Tag"></param>
        /// <param name="Option">"": 소숫점x 아닐경우 소숫점</param>
        public static void TextMinus(Form frm, string Tag, string Option = "")
        {
            string[] ArrCtrl = Tag.Split(':')[1].Split(',');
            double intTotSum = 0;
            for (int i = 0; i < ArrCtrl.Length; i++)
            {
                Control[] control = frm.Controls.Find(ArrCtrl[i].Trim(), true);
                if (control.Length > 0)
                {
                    //총점
                    if (i == ArrCtrl.Length - 1)
                    {
                        control[0].Text = string.IsNullOrWhiteSpace(Option) ? intTotSum.ToString() : intTotSum.ToString("0.0");
                    }
                    else
                    {
                        if (intTotSum == 0)
                        {
                            intTotSum = VB.Val(control[0].Text.Trim());
                        }
                        else
                        {
                            intTotSum -= VB.Val(control[0].Text.Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 컨트롤 여러개 숨기기/보이기
        /// </summary>
        public static void Set_ControlVisible2(Form frm, string Tag)
        {
            string[] Arr = Tag.Replace("\r", "").Replace("\n", "").Split(':');
            Control[] controls = null;
            
            for(int i = 1; i < Arr.Length; i++)
            {
                if (Arr[i].ToUpper().Equals("TRUE") || Arr[i].ToUpper().Equals("FALSE"))
                {                   
                    controls = frm.Controls.Find(Arr[i + 1], true);
                    if (controls.Length > 0)
                    {
                        
                        #region 간호정보 조사지 피부상태 관련 하드코딩
                        if (controls[0].Name.Equals("panNurImg") && Arr[i].ToUpper().Equals("FALSE"))
                        {
                            int chkCnt = 0;
                            Control ctl = frm.Controls.Find("I0000034243", true).FirstOrDefault();
                            if (ctl != null && ctl is RadioButton && (ctl as RadioButton).Checked)
                            {
                                chkCnt++;
                            }
                            ctl = frm.Controls.Find("I0000034245", true).FirstOrDefault();
                            if (ctl != null && ctl is RadioButton && (ctl as RadioButton).Checked)
                            {
                                chkCnt++;
                            }
                            ctl = frm.Controls.Find("I0000034247", true).FirstOrDefault();
                            if (ctl != null && ctl is RadioButton && (ctl as RadioButton).Checked)
                            {
                                chkCnt++;
                            }

                            if (chkCnt != 3)
                                continue;
                        }
                        #endregion

                        controls[0].Visible = Arr[i].ToUpper().Equals("TRUE");
                        if (controls[0] is Panel)
                        {
                            PanelAutoSize(controls[0], Arr[i].ToUpper().Equals("TRUE"));
                        }
                        #region 숨길때 하위 내용 전부 클리어
                        if (controls[0] is Panel && Arr[i].ToUpper().Equals("FALSE"))
                        {
                            foreach (Control Clearcontrol in ComFunc.GetAllControls(controls[0]))
                            {
                                if (Clearcontrol is TextBox)
                                {
                                    Clearcontrol.Text = "";
                                }
                                else if (Clearcontrol is CheckBox)
                                {
                                    (Clearcontrol as CheckBox).Checked = false;
                                }
                                else if (Clearcontrol is RadioButton)
                                {
                                    (Clearcontrol as RadioButton).Checked = false;
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 패널안 컨트롤 숨기고 보여주는 함수
        /// </summary>
        public static void Set_ControlVisible3(Form frm, string Tag)
        {
            string[] Arr = Tag.Replace("\r", "").Replace("\n", "").Split(':');
            Control[] controls = null;

            for (int i = 1; i < Arr.Length; i++)
            {
                if (Arr[i].ToUpper().Equals("TRUE") || Arr[i].ToUpper().Equals("FALSE"))
                {
                    controls = frm.Controls.Find(Arr[i + 1], true);
                    if (controls.Length > 0)
                    {
                        if (controls[0] is Panel)
                        {
                            #region 숨길때 하위 내용 전부 클리어
                            foreach (Control Clearcontrol in ComFunc.GetAllControls(controls[0]))
                            {
                                if (Arr[i].ToUpper().Equals("FALSE"))
                                {
                                    if (Clearcontrol is TextBox)
                                    {
                                        Clearcontrol.Text = "";
                                    }
                                    else if (Clearcontrol is CheckBox)
                                    {
                                        (Clearcontrol as CheckBox).Checked = false;
                                    }
                                    else if (Clearcontrol is RadioButton)
                                    {
                                        (Clearcontrol as RadioButton).Checked = false;
                                    }

                                    Clearcontrol.Visible = false;
                                }
                                else
                                {
                                    Clearcontrol.Visible = true;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 부모 패널안에서 선택한 패널의 순서를 변경
        /// (Max: 맨뒤로, One: 첫번째로, Con: 특정위치로)
        /// Set_PanelSort:부모패널:변경할패널:위의 변경값
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Tag"></param>
        public static void Set_PanelSort(Form frm, string Tag)
        {
            string[] Arr = Tag.Substring(Tag.IndexOf(':') + 1).Split(':');

            string CtOption = Arr[0]; //Add, Remove
            Control Pcontrol = frm.Controls.Find(Arr[1], true).FirstOrDefault(); //부모 패널 명
            Control Ncontrol = Pcontrol.Controls.Find(Arr[2], true).FirstOrDefault(); //현재 패널 명
            string CtOption2 = Arr[3]; //(Max: 맨뒤로, One: 첫번째로, Con: 특정위치로)

            int index = 0;
            if (CtOption2.ToUpper().Equals("MAX"))
            {
                index = 0;
            }
            else if (CtOption2.ToUpper().Equals("ONE"))
            {
                index = Pcontrol.Controls.Count;
            }
            else if (CtOption2.ToUpper().Equals("CON") && Arr.Length > 4)
            {
                index = (int) VB.Val(Arr[4]);
            }


            if (Pcontrol == null)
                return;

            Pcontrol.Visible = true;

            if (CtOption.Equals("Remove"))
            {
                if (Ncontrol.Visible)
                {
                    Pcontrol.Controls.SetChildIndex(Ncontrol, index);
                    PanelAutoSize(Ncontrol, false);
                }
            }
            else
            {
                if (!Ncontrol.Visible)
                {
                    Pcontrol.Controls.SetChildIndex(Ncontrol, index);
                    PanelAutoSize(Ncontrol, true);
                }
            }
           
        }

        /// <summary>
        /// 해당 패널 에서 숨겨진 패널 1개씩 보이고 숨기기
        /// 이름_숫자 형태로 무조건 되어있어야 합니다.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Tag"></param>
        public static void Set_PanelControl(Form frm, string Tag)
        {
            string[] Arr = Tag.Substring(Tag.IndexOf(':') + 1).Split(':');

            string CtOption = Arr[0]; //Add, Remove
            Control control = frm.Controls.Find(Arr[1], true).FirstOrDefault(); //부모 패널 명

            if (control == null)
                return;

            control.Visible = true;

            List<Control> controls = control.Controls.Cast<Control>().Where(c => c.Name.IndexOf("_") != -1 && (CtOption.Equals("Add") ? c.Visible == false : c.Visible)).ToList();

            if (controls.Count == 0)
                return;

            if (CtOption.Equals("Remove"))
            {
                controls = controls.OrderByDescending(c => VB.Val(c.Name.Substring(c.Name.LastIndexOf("_") + 1))).ToList();
            }
            else
            {
                controls = controls.OrderBy(c => VB.Val(c.Name.Substring(c.Name.LastIndexOf("_") + 1))).ToList();
            }

            for (int i = 0; i < controls.Count; i++)
            {
                Control PanCtrl = controls[i];
                if (CtOption.Equals("Add"))
                {
                    if(!PanCtrl.Visible)
                    {
                        PanCtrl.Parent.Controls.SetChildIndex(PanCtrl, i);
                        PanelAutoSize(PanCtrl, true);
                        break;
                    }
                }
                else if(CtOption.Equals("Remove")) 
                {
                    if (PanCtrl.Visible)
                    {
                        PanelAutoSize(PanCtrl, false);
                        //PanCtrl.Parent.Controls.SetChildIndex(PanCtrl, i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 해당 컨트롤 check(라디오, 체크박스)
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="control"></param>
        /// <param name="Tag"></param>
        public static void Set_ControlCheck(Form frm, string Tag)
        {
            string[] Arr = Tag.Split(':');
            Control[] controls = frm.Controls.Find(Arr[2], true);
            bool Checked = Arr[1].ToUpper().Equals("TRUE");

            if (controls.Length > 0)
            {
                if(controls[0] is CheckBox)
                {
                    (controls[0] as CheckBox).Checked = Checked;
                }
                else if(controls[0] is RadioButton)
                {
                    (controls[0] as RadioButton).Checked = Checked;
                }
            }
        }

        /// <summary>
        /// 체크/라디오 버튼 CHECK값으로 VISIBLE 및 순서지정
        /// Set_CheckControlVisible_Order:컨트롤이름:순서번호(시작 순서가 뒤에서 부터해야 맨처음부터 보입니다.)
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="control"></param>
        /// <param name="Tag"></param>
        public static void Set_CheckControlVisible_Order(Form frm, Control control, string Tag)
        {
            string CtlName = Tag.Split(':')[1];
            string Order = Tag.Split(':')[2];

            Control[] controls = frm.Controls.Find(CtlName, true);
            if (control is CheckBox)
            {
                Set_ControlVisible(frm, ":" + (control as CheckBox).Checked.ToString() + ":" + CtlName);
                if (controls.Length > 0 && controls[0] is Panel)
                {
                    PanelAutoSize(controls[0], (control as CheckBox).Checked);

                    controls[0].Parent.Controls.SetChildIndex(controls[0], (int) VB.Val(Order));
                }
            }
            else if (control is RadioButton)
            {
                Set_ControlVisible(frm, ":" + (control as RadioButton).Checked.ToString() + ":" + CtlName);
                if (controls.Length > 0 && controls[0] is Panel)
                {
                    PanelAutoSize(controls[0], (control as RadioButton).Checked);

                    controls[0].Parent.Controls.SetChildIndex(controls[0], (int) VB.Val(Order));
                }
            }
        }

        /// <summary>
        /// 체크/라디오 버튼 CHECK값으로 VISIBLE
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="control"></param>
        /// <param name="Tag"></param>
        public static void Set_CheckControlVisible(Form frm, Control control, string Tag)
        {
            string CtlName = Tag.Substring(Tag.LastIndexOf(":") + 1);
            Control[] controls = frm.Controls.Find(CtlName, true);
            if (control is CheckBox) 
            {
                Set_ControlVisible(frm, ":" + (control as CheckBox).Checked.ToString() + ":" + CtlName);
                if (controls.Length > 0 && controls[0] is Panel)
                {
                    PanelAutoSize(controls[0], (control as CheckBox).Checked);
                }
            }
            else if (control is RadioButton)
            {
                Set_ControlVisible(frm, ":" + (control as RadioButton).Checked.ToString() + ":" + CtlName);
                if (controls.Length > 0 && controls[0] is Panel)
                {
                    PanelAutoSize(controls[0], (control as RadioButton).Checked);
                }
            }
        }
               
        /// <summary>
        /// 컨트롤일경우 그냥 숨기고 패널일경우 숨기고 사이즈 조정.
        /// Set_ControlVisible:True:컨트롤 일경우
        /// 해당 컨트롤만 VISIBLE TRUE FALSE
        /// Set_ControlVisible:TRUE:컨트롤:패널
        /// 해당 컨트롤 VISIBLE TRUE하고
        /// 끝에 패널 VISIBLE 설정 및 부모패널 사이즈 조절
        /// </summary>
        public static void Set_ControlVisible(Form frm, string Tag)
        {
            string[] Arr = Tag.Split(':');
            bool Visible = Arr[1].ToUpper().Equals("TRUE");

            Control[]  controls = frm.Controls.Find(Arr[2], true);
            if(controls.Length > 0)
            {
                if (!controls[0].Visible.Equals(Visible))
                {
                    controls[0].Visible = Visible;

                    #region 숨길때 하위 내용 전부 클리어
                    if (Visible == false)
                    { 
                        foreach (Control Clearcontrol in ComFunc.GetAllControls(controls[0]))
                        {
                            if (Clearcontrol is TextBox)
                            {
                                Clearcontrol.Text = "";
                            }
                            else if (Clearcontrol is CheckBox)
                            {
                                (Clearcontrol as CheckBox).Checked = false;
                            }
                            else if (Clearcontrol is RadioButton)
                            {
                                (Clearcontrol as RadioButton).Checked = false;
                            }
                        }
                    }
                    #endregion

                }
            }

            if (Arr.Length > 3)
            {
                controls = frm.Controls.Find(Arr[3], true);
                if (controls.Length > 0)
                {
                    if (!controls[0].Visible.Equals(Visible))
                    {
                        PanelAutoSize(controls[0], Visible);
                    }
                }
            }
        }

        /// <summary>
        /// ER내원 후 병동 입원환자 출발시간 가져오기
        /// </summary>
        public static void Set_FormPatInfo_Er_Trans(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, Control ct)
        {
            if (ct is TextBox && VB.IsDate(((TextBox)ct).Text))
            {
                string ErTransTime = FormPatInfoFunc.Set_FormPatInfo_Er_Trans(pDbCon, AcpEmr);

                if (!VB.IsDate(ErTransTime))
                    return;

                DateTime dtpIpdFrDate = Convert.ToDateTime(ct.Text);
                DateTime dtpErFrDate = Convert.ToDateTime(ErTransTime);

                if ((dtpIpdFrDate - dtpErFrDate).TotalMinutes > 10)
                {
                    ComFunc.MsgBoxEx(frm, "ER 이송시간 : " + ErTransTime + ComNum.VBLF + 
                                          "병동 입력시간 : " + ct.Text + ComNum.VBLF + 
                                          "10분이 경과했습니다 시간을 다시 확인해주세요.", "경고");
                }
            }
        }

        /// <summary>
        /// 사생활 보호 요청
        /// </summary>
        public static void Set_FormPatInfo_Secret(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, Control ct)
        {
            if (ct is RadioButton && ((RadioButton) ct).Checked)
            {
                if (ComFunc.MsgBoxQEx(frm, "해당 환자의 사생활 보호를 설정하시겠습니까?") == DialogResult.No)
                    return;

                FormPatInfoFunc.Set_FormPatInfo_Secret(pDbCon, AcpEmr);
            }
        }

        /// <summary>
        /// ER정보 연동
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void Set_FormPatInfo_ER(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            Dictionary<string, string> EMRTRESULT = new Dictionary<string, string>(); //
            Control[] controls = null;
            string CtrlName = string.Empty;

            #region 귀가, 전원, 입원 사유
            EMRTRESULT.Add("11", "I0000034164"); //증상이 호전되어 귀가
            EMRTRESULT.Add("12", "I0000034165"); //말기질환으로 귀가(가정간호 등)6
            EMRTRESULT.Add("13", "I0000034166"); //가망 없는 퇴실 (hopeless discharge)
            EMRTRESULT.Add("14", "I0000034167"); //자의 퇴실
            EMRTRESULT.Add("15", "I0000034168"); //외래 방문 후 귀가 또는 입원
            EMRTRESULT.Add("18", "I0000034180"); //기타 (재활원, 일반시설로 돌아간 경우 등)

            EMRTRESULT.Add("21", "I0000034169"); //당장 응급수술 또는 응급처치가 불가능하여 전원
            EMRTRESULT.Add("22", "I0000034170"); //병실이 부족하여 전원
            EMRTRESULT.Add("23", "I0000034171"); //중환자실이 부족하여 전원
            EMRTRESULT.Add("24", "I0000034172"); //전문 응급의료를 요하므로 전원
            EMRTRESULT.Add("25", "I0000034181"); //경증으로 전원
            EMRTRESULT.Add("26", "I0000034182"); //요양병원으로 전원
            EMRTRESULT.Add("27", "I0000034174"); //환자 또는 보호자 사정으로 전원
            EMRTRESULT.Add("28", "I0000035155_1"); //전원사유 - 기타
            //("2", "I0000035155_2"); //기타 사유
            EMRTRESULT.Add("29", "I0000034183"); //회송 (요양병원 간 전원)

            EMRTRESULT.Add("31", "I0000034175"); //병실로 입원
            EMRTRESULT.Add("32", "I0000034176"); //중환자실로 바로 입원
            EMRTRESULT.Add("33", "I0000034177"); //수술(시술)실로 간 후 병실로 입원
            EMRTRESULT.Add("34", "I0000034178"); //수술(시술)실로 간 후 중환자실로 입원
            EMRTRESULT.Add("38", "I0000035164_1"); //진료 결과 - 입원 - 기타(기타 다른곳으로 입원)
            //("3", "I0000035164_2"); //기타 텍스트
            #endregion

            #region 내원사유/교통사고/진료결과 연동
            pairs.Clear();
            pairs.Add("PTMIDGKD", "I0000035126");//-- 내원사유(질병여부)
            pairs.Add("PTMIARCF", "I0000035127");//-- 질병외(의도성여부)
            pairs.Add("PTMIARCS", "I0000035128");//-- 질병외(손상기전)

            pairs.Add("PTMITAIP", "I0000035129");//-- 교통사고당사자
            //pairs.Add("PTMITAIP2", "I0000035130");//-- 교통사고당사자 - 미해당(?)
            pairs.Add("PTMITSBT", "I0000035131");//-- 교통사고보장구-안전밸드
            pairs.Add("PTMITSCS", "I0000035132");//-- 교통사고보장구-이동용좌석
            pairs.Add("PTMITSFA", "I0000035133");//-- 교통사고보장구-전면에어백
            pairs.Add("PTMITSSA", "I0000035134");//-- 교통사고보장구-측면에어백
            pairs.Add("PTMITSHM", "I0000035135");//-- 교통사고보장구-헬맷
            pairs.Add("PTMITSPT", "I0000035136");//-- 교통사고보장구-무릎및 관절보호대
            pairs.Add("PTMITSNO", "I0000035137");//-- 교통사고보장구-전혀 착용 않함
            pairs.Add("PTMITSUR", "I0000035138");//-- 교통사고보장구-비해당
            pairs.Add("PTMITSUK", "I0000035139");//-- 교통사고보장구-미상

            pairs.Add("PTMIEMRT", "I0000035139");  //-- 응급진료 코드 (귀가/전원/입원/사망/기타/미상)
            pairs.Add("EMRTRESULT", "I0000035139");//-- 응급진료 텍스트(귀가/전원/입원/사망/기타/미상) 
            pairs.Add("AREA", "I0000031631");//-- 최종진료구역

            SQL = FormPatInfoQuery.Query_FormPatInfo_VisitReason(AcpEmr);
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SQL, SQL, pDbCon);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    if (dt.Columns[i].ColumnName.Equals("PTMIEMRT"))
                    {
                        switch (VB.Left(dt.Rows[0]["PTMIEMRT"].ToString().Trim(), 1))
                        {
                            case "1":
                                CtrlName = "I0000034163";
                                break;
                            case "2":
                                CtrlName = "I0000035162";
                                break;
                            case "3":
                                CtrlName = "I0000035163";
                                break;
                            default:
                                continue;
                        }

                        controls = frm.Controls.Find(CtrlName, true);
                        if (controls.Length > 0)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                    else if (dt.Columns[i].ColumnName.Equals("EMRTRESULT") && dt.Rows[0]["EMRTRESULT"].ToString().Trim().Length > 0)
                    {
                        if (EMRTRESULT.TryGetValue(dt.Rows[0]["PTMIEMRT"].ToString().Trim(), out CtrlName))
                        {
                            controls = frm.Controls.Find(CtrlName, true);
                            if (controls.Length > 0)
                            {
                                if (controls[0] is CheckBox)
                                {
                                    (controls[0] as CheckBox).Checked = true;
                                }
                            }
                        }

                    }
                    else if (pairs.TryGetValue(dt.Columns[i].ColumnName, out CtrlName))
                    {
                        controls = frm.Controls.Find(CtrlName, true);
                        if (controls.Length > 0)
                        {
                            if (controls[0] is CheckBox)
                            {
                                (controls[0] as CheckBox).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("Y");
                            }
                            else if (controls[0] is RadioButton)
                            {
                                (controls[0] as RadioButton).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("Y");
                            }
                            else if (controls[0] is TextBox)
                            {
                                (controls[0] as TextBox).Text = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim();
                            }
                        }
                    }
                }
            }

            dt.Dispose();
            #endregion

            #region 중증도 분류 시간
            SQL = FormPatInfoQuery.Query_FormPatInfo_Triage(AcpEmr);
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SQL, SQL, pDbCon);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                controls = frm.Controls.Find("I0000034150_1", true); //최초 중증도 분류일자
                if (controls.Length > 0 && dt.Rows[0]["PTMIKTDT"].ToString().Trim().Length > 0)
                {
                    (controls[0] as TextBox).Text = DateTime.ParseExact(dt.Rows[0]["PTMIKTDT"].ToString().Trim(), "yyyyMMdd", null).ToShortDateString();
                }

                controls = frm.Controls.Find("I0000034150_2", true); //최초 중증도 분류시간
                if (controls.Length > 0 && dt.Rows[0]["PTMIKTTM"].ToString().Trim().Length > 0)
                {
                    (controls[0] as TextBox).Text = VB.Val(VB.Left(dt.Rows[0]["PTMIKTTM"].ToString().Trim(), 4)).ToString("00:00");
                }

                controls = frm.Controls.Find("I0000034151", true);  //최초 중증도 분류
                if (controls.Length > 0)
                {
                    (controls[0] as TextBox).Text = dt.Rows[0]["PTMIKTS"].ToString().Trim();
                }
            }
            dt.Dispose();
            #endregion

            #region 내원시 교육 연동(NUR_ER_PATIENTADD)
            pairs.Clear();
            pairs.Add("INSTS", "I0000034158");    //내원동기
            pairs.Add("OPINFO", "I0000034159");   //과거병력 및 수술력
            pairs.Add("IK91", "I0000029075");     //보호자 1인상주
            pairs.Add("IK92", "I0000029755");     //도난방지
            pairs.Add("IK93", "I0000034160");     //낙상방지
            pairs.Add("IK94", "I0000034161");     //화재예방 및 비상시 안내
            pairs.Add("IK95", "I0000035158");     //금연
            pairs.Add("IK96", "I0000034162");     //소아유괴방지
            pairs.Add("IK100", "I0000035160");    //보조기구(틀니,보청기)
            pairs.Add("IK97", "I0000035161_1");   //기타
            pairs.Add("IT135", "I0000035161_2");  //기타 내용

            SQL = FormPatInfoQuery.Query_FormPatInfo_ErPatientInfo(AcpEmr);
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SQL, SQL, pDbCon);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (pairs.TryGetValue(dt.Columns[i].ColumnName.ToUpper(), out CtrlName))
                    {
                        controls = frm.Controls.Find(CtrlName, true);
                        if (controls.Length > 0)
                        {
                            if (controls[0] is CheckBox)
                            {
                                (controls[0] as CheckBox).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().Equals("1");
                            }
                            else if (controls[0] is RadioButton)
                            {
                                (controls[0] as RadioButton).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("TRUE");
                            }
                            else if (controls[0] is TextBox)
                            {
                                (controls[0] as TextBox).Text = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim();
                            }
                        }
                    }
                }
            }
            dt.Dispose();
            #endregion
        }

        /// <summary>
        /// 해당 패널 숨김(보이)고 해당 하는 부모 컨트롤 사이즈 자동 조절
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void Set_AutoPanelVisible(Form frm, Control ct, string strTag)
        {
            if (strTag.Split(':').Length == 2)
                return;

            bool bVisible = (ct is CheckBox) ? ((CheckBox) ct).Checked  :  strTag.Split(':')[1].ToUpper().Equals("TRUE");
            string strCtrl = strTag.Split(':')[2];
            Control[] controls;
            controls = frm.Controls.Find(strCtrl, true);

            if (controls.Length > 0)
            {
                PanelAutoSize(controls[0], bVisible);
            }
        }

        /// <summary>
        /// 패널 보이고(숨기고) 자동 사이즈 조절
        /// </summary>
        /// <param name="control"></param>
        public static void PanelAutoSize(Control control, bool Visible)
        {
            int CtrlHeight = control.Height;
            List<Control> Parents = GetParentList(control);
            control.Visible = Visible;

            #region 숨길때 하위 내용 전부 클리어
            if (Visible == false)
            {
                foreach (Control Clearcontrol in ComFunc.GetAllControls(control))
                {
                    if (Clearcontrol is TextBox)
                    {
                        Clearcontrol.Text = "";
                    }
                    else if (Clearcontrol is CheckBox)
                    {
                        (Clearcontrol as CheckBox).Checked = false;
                    }
                    else if (Clearcontrol is RadioButton)
                    {
                        (Clearcontrol as RadioButton).Checked = false;
                    }
                    else if (Clearcontrol is PictureBox)
                    {

                    }
                }
            }
            #endregion

            foreach (Control ctl in Parents)
            {
                int PanelHeight = 0;
                if (Visible && control.Visible == false && ctl.Visible == false)
                {
                    //ctl.Visible = true;
                }

                foreach (Control ctl2 in ctl.Controls)
                {
                    if (ctl2 is Panel && ctl2.Dock == DockStyle.Top && ctl2.Visible)
                    {
                        PanelHeight += ctl2.Height;
                    }
                }

                if (PanelHeight > 0 && ctl.Height != PanelHeight)
                {
                    ctl.Height = PanelHeight;
                }
            }
        }

        /// <summary>
        /// 부모패널 최종 사이즈
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static List<Control> GetParentList(Control control)
        {
            List<Control> rtnVal = new List<Control>();

            Control sControl = control;

            while (control is Panel)
            {
                control = control.Parent;
                if (control.Name.Equals("panChart"))
                    break;

                if(control is Panel)
                {
                    rtnVal.Add(control);
                }
            };

            return rtnVal;
        }

        public static bool READ_CHART_COMPLETE(PsmhDb pDbCon, EmrPatient emrPatient, EmrForm emrForm)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = string.Empty;

            try
            {
                SQL = " SELECT A.EMRNO";
                if (emrForm.FmOLDGB == 1)
                {
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A";
                }
                else
                {
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A";
                }

                SQL += ComNum.VBLF + " INNER JOIN KOSMOS_EMR.EMRXML_COMPLETE B";
                SQL += ComNum.VBLF + "    ON A.EMRNO = B.EMRNO";
                if (emrForm.FmOLDGB == 0)
                {
                    SQL += ComNum.VBLF + "   AND A.EMRNOHIS = B.EMRNOHIS";
                }
                SQL += ComNum.VBLF + "WHERE A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
                SQL += ComNum.VBLF + "  AND A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "  AND A.FORMNO = 1647";


                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 2021-634 전산업무 의뢰서 처리(입퇴원 요약지 검수완료)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="EmrNo"></param>
        /// <returns></returns>
        public static bool READ_CHART_COMPLETE2(PsmhDb pDbCon, string EmrNo)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = string.Empty;

            try
            {
                SQL += ComNum.VBLF + " WITH EMR_DATA AS                                             ";
                SQL += ComNum.VBLF + " (                                                            ";
                SQL += ComNum.VBLF + " SELECT A.EMRNO, 0 AS EMRNOHIS, A.MEDFRDATE, A.PTNO           ";
                 SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXMLMST A                                ";
                SQL += ComNum.VBLF + "  WHERE A.EMRNO = " + EmrNo + "";

                SQL += ComNum.VBLF + "  UNION ALL                                                   ";
                SQL += ComNum.VBLF + " SELECT A.EMRNO, A.EMRNOHIS, A.MEDFRDATE, A.PTNO              ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_EMR.AEMRCHARTMST A                             ";
                SQL += ComNum.VBLF + "  WHERE A.EMRNO = " + EmrNo + "";
                SQL += ComNum.VBLF + " )                                                            ";

                SQL += ComNum.VBLF + " SELECT 1                                                     ";
                SQL += ComNum.VBLF + "   FROM EMR_DATA A                                            ";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMRXML_COMPLETE B";
                SQL += ComNum.VBLF + "     ON A.EMRNO = B.EMRNO";
                SQL += ComNum.VBLF + "    AND CASE WHEN A.EMRNOHIS = 0 THEN 1                       ";
                SQL += ComNum.VBLF + "             WHEN A.EMRNOHIS = B.EMRNOHIS THEN 1              ";
                SQL += ComNum.VBLF + "        END = 1                                               ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 즐겨보는 기록지에 등록한 기록지가 있는지 체크
        /// 있으면 True: 없으면 False
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public static bool EMR_LIKERecord(PsmhDb pDbCon, string strSabun)
        {
            bool rtnVal = false;
            return rtnVal;

            //OracleDataReader reader = null;

            //string SQL = string.Empty;

            //try
            //{
            //    SQL = "SELECT COUNT(*) CNT";
            //    SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_LIKERECORD";
            //    SQL += ComNum.VBLF + "WHERE USEID = '" + strSabun + "'";

            //    string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            //    if (sqlErr.Length > 0)
            //    {
            //        clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
            //        ComFunc.MsgBox(sqlErr);
            //        return rtnVal;
            //    }

            //    if (reader.HasRows && reader.Read() &&  VB.Val(reader.GetValue(0).ToString().Trim()) > 0)
            //    {
            //        rtnVal = true;
            //    }

            //    reader.Dispose();

            //}
            //catch (Exception ex)
            //{
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
            //    ComFunc.MsgBox(ex.Message);
            //}

            //return rtnVal;
        }


        /// <summary>
        /// 해당 컨트롤 활성화 혹은 비활성화 및 
        /// 패널 안에 있는 컨트롤(체크, 라디오) 일경우 체크해제
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void Set_PanelActive(Form frm, string strTag)
        {
            string strBool = strTag.Split(':')[1];
            string strCtrl = strTag.Split(':')[2];
            Control[] controls;
            controls = frm.Controls.Find(strCtrl, true);
            if (controls.Length > 0)
            {
                controls[0].Enabled = strBool.ToUpper().Equals("TRUE");
                if(controls[0].Enabled == false)
                {
                    foreach(Control ctrl in controls[0].Controls)
                    {
                        if (ctrl is CheckBox)
                        {
                            ((CheckBox)ctrl).Checked = false;
                        }
                        else if(ctrl is RadioButton)
                        {
                            ((RadioButton)ctrl).Checked = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 부진단명 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void Set_FormPatInfo_SubDisease_Eng(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            string strControl = strTag.Split(':')[1];
            int intStart = (int) VB.Val(Regex.Replace(strTag.Split(':')[2], @"[^0-9]", ""));
            int intEnd = (int) VB.Val(Regex.Replace(strTag.Split(':')[3], @"[^0-9]", ""));

            DataTable dt = null;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_MainDisease(AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
            string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            bool bNewForm = VB.Left(strControl, 1).Equals("I");

            List<Control> controls = FormFunc.GetAllControls(frm).Where(d => d is TextBox).ToList();
            List<Control> lstSubDiagCodeText = controls.Where(c => c.Name.IndexOf(bNewForm ?  "I0000036383" : "di") != -1).
                OrderBy(c => int.Parse(c.Name.Substring(bNewForm ? c.Name.LastIndexOf("_") + 1 : 2))).ToList();
            List<Control> lstSubDiagNameText = bNewForm ? controls.Where(c => c.Name.IndexOf("I0000031714") != -1).
                OrderBy(c => int.Parse(c.Name.Substring(c.Name.LastIndexOf("_") + 1))).ToList() : null;
            List<Control> lstSubDiagPoaText = bNewForm ? controls.Where(c => c.Name.IndexOf("I0000036382") != -1).
                OrderBy(c => int.Parse(c.Name.Substring(c.Name.LastIndexOf("_") + 1))).ToList() : null;


            //string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            //if (sqlErr.Length > 0)
            //{
            //    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
            //    ComFunc.MsgBoxEx(frm, sqlErr);
            //    return;
            //}

            if (dt.Rows.Count > 0)
            {
                if (bNewForm == false && lstSubDiagCodeText != null)
                {
                    lstSubDiagCodeText.Remove(lstSubDiagCodeText.Find(d => d.Name.Equals("di1")));
                }

                dt = dt.AsEnumerable().GroupBy(r => r.Field<string>("ILLCODED")).Select(s => s.First()).OrderBy(o => o.Field<string>("ORDDATE")).CopyToDataTable();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i >= (bNewForm ?  20 : 8))
                        break;

                    if (bNewForm)
                    {
                        //if (dt.Rows[i]["ILLCODED"].ToString().Trim().NotEmpty())
                        //{
                        lstSubDiagCodeText[i].Text = dt.Rows[i]["ILLCODED"].ToString().Trim();
                        lstSubDiagNameText[i].Text = dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                        lstSubDiagPoaText[i].Text = dt.Rows[i]["POA"].ToString().Trim();
                        //}
                        
                        //lstSubDiagPoaText[i].Text = dt.Rows[i]["POA"].ToString().Trim().Equals("N") == false ? "Y" : dt.Rows[i]["POA"].ToString().Trim();
                    }
                    else
                    {
                        lstSubDiagCodeText[i].Text = Regex.Replace(dt.Rows[i]["ILLCODED"].ToString().Trim(), @"/([^a-zA-Z0-9])/", "") + "  :" + dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                    }
                    
                }
            }

            dt.Dispose();
        }

        /// <summary>
        /// Text 내용 복사.
        /// </summary>
        /// <param name="strPtno"></param>
        /// <returns></returns>
        public static void Set_Form_TextCopy(Control frm, string strTag)
        {
            if (strTag.IndexOf(":") == -1)
                return;

            if(strTag.Split(':').Length != 3)
                return;


            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                string strCopyText = controls[0].Text.Trim();
                controls = frm.Controls.Find(strTag.Split(':')[2], true);
                if (controls.Length > 0)
                {
                    controls[0].Text = strCopyText;
                }
            }
        }

        public static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //create new image with 16x16 pixel
            using (Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16)))
            {
                for (int j = 0; j < bmpMin.Height; j++)
                {
                    for (int i = 0; i < bmpMin.Width; i++)
                    {
                        //reduce colors to true / false                
                        lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                    }
                }
            }

            return lResult;
        }

        public static bool ImageCompare(Bitmap bmp1, Bitmap bmp2)
        {
            bool cr = true;

            //Test to see if we have the same size of image
            if (bmp1.Size != bmp2.Size)
            {
                cr = false;
            }
            else
            {
                //Convert each image to a byte array
                System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                byte[] btImage1 = new byte[1];
                byte[] btImage2 = new byte[1];

                using (MemoryStream mem = new MemoryStream())
                {
                    using(Bitmap bitmap = new Bitmap(bmp1))
                    {
                        bitmap.Save(mem, ImageFormat.Jpeg);
                        btImage1 = mem.ToArray();
                    }
                }

                using (MemoryStream mem = new MemoryStream())
                {
                    using (Bitmap bitmap = new Bitmap(bmp2))
                    {
                        bitmap.Save(mem, ImageFormat.Jpeg);
                        btImage2 = mem.ToArray();
                    }
                }
                //btImage1 = (byte[])ic.ConvertTo(bmp1, btImage1.GetType());
                //btImage2 = (byte[])ic.ConvertTo(bmp2, btImage2.GetType());

                //Compute a hash for each image
                using (SHA256Managed shaM = new SHA256Managed())
                {
                    byte[] hash1 = shaM.ComputeHash(btImage1);
                    byte[] hash2 = shaM.ComputeHash(btImage2);

                    //Compare the hash values
                    for (int i = 0; i < hash1.Length && i < hash2.Length
                                      && cr == true; i++)
                    {
                        if (hash1[i] != hash2[i])
                        {
                            cr = false;
                            break;
                        }
                    }
                }
            }
            return cr;
        }

        //public static bool ImageCompare(Bitmap bmp1, Bitmap bmp2)
        //{
        //    if (bmp1 == null || bmp2 == null)
        //        return false;

        //    List<bool> iHash1 = GetHash(bmp1);
        //    List<bool> iHash2 = GetHash(bmp2);

        //    //determine the number of equal pixel (x of 256)
        //    int equalElements = iHash1.Zip(iHash2, (i, j) => i == j).Count(eq => eq);

        //    return equalElements.Equals(256);
        //}

        /// <summary>
        /// 유저 권한
        /// </summary>
        /// <returns></returns>

        public static string SET_GRADE()
        {
            string rtnVal = string.Empty;
            switch (clsType.User.BuseCode)
            {
                case "078200":
                case "078201":
                case "077502":
                case "077405":
                    rtnVal = "SIMSA";
                    break;
                case "044201":
                case "044200":
                    rtnVal = "WRITE";
                    break;
                case "055307":
                    rtnVal = "PT";
                    break;
                case "076001":
                    rtnVal = "QI";
                    break;
                case "055101":
                    rtnVal = "XRAY";//     '2019-02-12
                    break;
            }

            #region 부서가 XRAY인데 실제 근무부서는 혈관조영실일경우 다보이게
            if (rtnVal.Equals("XRAY") && GetSilBuse().Equals("100570"))
            {
                rtnVal = string.Empty;
            }
            #endregion

            if (clsType.User.Sabun.Equals("14472"))
            {
                rtnVal = "SIMSA";
            }

            return rtnVal;

        }

        
        /// <summary>
        /// 실제 부서코드 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static string GetSilBuse()
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;

            #region 혈관조영실 하드코딩
            if (clsType.User.IdNumber.Equals("47769") || clsType.User.IdNumber.Equals("50503") || clsType.User.IdNumber.Equals("51028"))
            {
                return "100570";
            }
            #endregion

            string SQL = string.Empty;

            try
            {
                SQL = "SELECT SIL_BUSE ";
                SQL += ComNum.VBLF + "FROM KOSMOS_ADM.INSA_MST";
                SQL += ComNum.VBLF + "WHERE SABUN3 = " + clsType.User.IdNumber;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// 퇴사한 사번 로그인 및 수정권한 주는 프로그램
        /// </summary>
        /// <returns></returns>
        public static bool TOISABUN_CERT_B(PsmhDb pDbCon, string strSabun)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = string.Empty;

            try
            {
                SQL = "SELECT CERT";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML_MOCIFY_CERT_B";
                SQL += ComNum.VBLF + "WHERE SABUN = " + strSabun;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 퇴사한 사번 로그인 및 수정권한 주는 프로그램
        /// </summary>
        /// <returns></returns>
        public static bool TOISABUN_CERT(PsmhDb pDbCon)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = string.Empty;

            try
            {
                SQL = "SELECT CERT";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML_MOCIFY_CERT";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if(reader.HasRows)
                {
                    rtnVal = true;
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 수술/감염/작성내역을 시간을 조회한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pCallForm"></param>
        /// <param name="panChart"></param>
        /// <param name="FindText"></param>
        /// <param name="ssSpd"></param>
        /// <param name="ctlControl"></param>
        /// <param name="strDiagCode"></param>
        public static void OpControl(PsmhDb pDbCon, Form pCallForm, Control panChart, Control parentCtl, FarPoint.Win.Spread.FpSpread ssSpd, EmrPatient emrPatient, EmrForm emrForm)
        {
            string SQL    = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            #region
            ssSpd.Left = parentCtl.Left - 80;
            ssSpd.Top = parentCtl.Top + 90;
            #endregion
            
            if(emrForm.FmFORMNO == 2465)
            {
                if (emrForm.FmOLDGB == 1)
                {
                    SQL = string.Empty;
                    SQL += ComNum.VBLF + "SELECT RDATE, EXNAME";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_INFECT_MASTER ";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "'";
                    SQL += ComNum.VBLF + "   AND ODATE IS NULL ";
                }
                else
                {
                    SQL = string.Empty;
                    SQL += ComNum.VBLF + "SELECT TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE, OPSTIME, OPETIME, WRTNO";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "'";
                    SQL += ComNum.VBLF + "   AND OPDATE >= TO_DATE('" + emrPatient.medFrDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + " ORDER BY OPDATE DESC";
                }
                
            }
            else if(emrForm.FmFORMNO == 2467 ||
                    emrForm.FmFORMNO == 1808 ||
                    emrForm.FmFORMNO == 2636 && emrForm.FmOLDGB == 0)
            {
                SQL = string.Empty;
                if (emrForm.FmOLDGB == 1)
                {
                    SQL += ComNum.VBLF + "SELECT CHARTDATE, CHARTTIME, NAME, A.EMRNO, 0 AS EMRNOHIS";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST A";
                    SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                    SQL += ComNum.VBLF + "      ON A.USEID = B.USERID";
                }
                else
                {
                    SQL += ComNum.VBLF + "SELECT CHARTDATE, CHARTTIME, NAME, A.EMRNO, A.EMRNOHIS, CASE WHEN A.SAVECERT = '0' THEN '임시' ELSE '인증' END SAVECERT";
                    SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                    SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                    SQL += ComNum.VBLF + "      ON A.CHARTUSEID = B.USERID";
                }
               
                SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
                if (emrForm.FmFORMNO == 1808)
                {
                    SQL += ComNum.VBLF + "   AND A.FORMNO = 1544";
                }
                else if (emrForm.FmFORMNO == 2467 || emrForm.FmFORMNO == 2636)
                {
                    SQL += ComNum.VBLF + "   AND A.FORMNO = 2465";
                }
                SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC";
            }
            else if(emrForm.FmFORMNO == 2309 || emrForm.FmFORMNO == 2611)
            {
                SQL = string.Empty;
                SQL += ComNum.VBLF + "SELECT TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE, DIAGNOSIS, OPTITLE, WRTNO ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND OPDATE >= TO_DATE('" + emrPatient.medFrDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ORDER BY OPDATE DESC";
            }
            else if (emrForm.FmFORMNO == 2308 || emrForm.FmFORMNO == 2610 || emrForm.FmFORMNO == 1544)
            {
                SQL = string.Empty;
                SQL += ComNum.VBLF + "SELECT TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE, PREDIAGNOSIS, OPILL, WRTNO";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OPSCHE ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND OPDATE >= TO_DATE('" + emrPatient.medFrDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ORDER BY OPDATE DESC";
            }
            else if (emrForm.FmFORMNO == 2280)
            {
                SQL = "SELECT CHARTDATE, CHARTTIME, NAME, A.EMRNO, A.EMRNOHIS, CASE WHEN A.SAVECERT = '0' THEN '임시' ELSE '인증' END SAVECERT";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                SQL += ComNum.VBLF + "      ON A.CHARTUSEID = B.USERID";
                SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND A.CHARTDATE >= '" + emrPatient.medFrDate + "'";
                if (!string.IsNullOrWhiteSpace(emrPatient.medEndDate))
                {
                    SQL += ComNum.VBLF + "   AND A.CHARTDATE <= '" + emrPatient.medEndDate + "'";
                }
                else
                {
                    SQL += ComNum.VBLF + "   AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')";
                }
                SQL += ComNum.VBLF + "   AND A.FORMNO = 2279";
                SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC";
            }
            else if (emrForm.FmFORMNO == 2323)
            {
                SQL = "SELECT CHARTDATE, CHARTTIME, NAME, A.EMRNO, A.EMRNOHIS, CASE WHEN A.SAVECERT = '0' THEN '임시' ELSE '인증' END SAVECERT";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL += ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMR_USERT B";
                SQL += ComNum.VBLF + "      ON A.CHARTUSEID = B.USERID";
                SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND A.CHARTDATE >= '" + emrPatient.medFrDate + "'";
                if (!string.IsNullOrWhiteSpace(emrPatient.medEndDate))
                {
                    SQL += ComNum.VBLF + "   AND A.CHARTDATE <= '" + emrPatient.medEndDate + "'";
                }
                else
                {
                    SQL += ComNum.VBLF + "   AND A.CHARTDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')";
                }
                SQL += ComNum.VBLF + "   AND A.FORMNO = 2241";
                SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC";
            }
            else
            {
                SQL = string.Empty;
                SQL += ComNum.VBLF + "SELECT TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE, OPSTIME, OPETIME, WRTNO";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND OPDATE >= TO_DATE('" + emrPatient.medFrDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ORDER BY OPDATE DESC";
            }
          
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 1)
            {
                if(emrForm.FmFORMNO == 2463)
                {
                    setChartFormValue9(pDbCon, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), emrForm);
                }
                //시술/검사전 기록지(Receive)
                else if (emrForm.FmFORMNO == 2467 || emrForm.FmFORMNO == 2636 && emrForm.FmOLDGB == 0 )
                {
                    setChartFormValue2467(pDbCon, panChart, dt.Rows[0]["EMRNO"].ToString().Trim(), dt.Rows[0]["EMRNOHIS"].ToString().Trim(), emrForm);
                }
                //PRE-OP CHECKLIST(수술)
                else if (emrForm.FmFORMNO == 1808)
                {
                    setChartFormValue1808(pDbCon, panChart, dt.Rows[0]["EMRNO"].ToString().Trim(), dt.Rows[0]["EMRNOHIS"].ToString().Trim(), emrForm);
                }
                else if (emrForm.FmFORMNO == 2618)
                {
                    setChartFormValue2618(pDbCon, emrForm, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["WRTNO"].ToString().Trim());
                }
                else if (emrForm.FmFORMNO == 1544)
                {
                    setChartFormValue1544_New(pDbCon, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["WRTNO"].ToString().Trim());
                }
                else if (emrForm.FmFORMNO >= 2644 && emrForm.FmFORMNO <= 2646)
                {
                    setChartFormValue2644_New(pDbCon, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["WRTNO"].ToString().Trim());
                }
                else if (emrForm.FmFORMNO == 2308 || emrForm.FmFORMNO == 2610)
                {
                    if (emrForm.FmOLDGB == 1)
                    {
                        setChartFormValue7(panChart, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim(), dt.Rows[0]["OPILL"].ToString().Trim());
                    }
                    else
                    {
                        setChartFormValue2610_New(pDbCon, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["WRTNO"].ToString().Trim());
                    }
                }
                else if(emrForm.FmFORMNO == 2309 || emrForm.FmFORMNO == 2611)
                {
                    if (emrForm.FmOLDGB == 1)
                    {
                        setChartFormValue7(panChart, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["DIAGNOSIS"].ToString().Trim(), dt.Rows[0]["OPTITLE"].ToString().Trim());
                    }
                    else
                    {
                        setChartFormValue2611_New(pDbCon, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["WRTNO"].ToString().Trim());
                    }
                }
                else if (emrForm.FmFORMNO == 2465 && emrForm.FmOLDGB != 1)
                {
                    setChartFormValue2465_New(pDbCon, panChart, emrPatient.ptNo, dt.Rows[0]["OPDATE"].ToString().Trim(), dt.Rows[0]["WRTNO"].ToString().Trim());
                }
                else if (emrForm.FmFORMNO == 2280)
                {
                    setChartFormValue3(pDbCon, pCallForm, emrPatient, 
                        dt.Rows[0]["CHARTDATE"].ToString().Trim(), 
                        emrForm,
                        dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                else if (emrForm.FmFORMNO == 2323)
                {
                    GetTransferHistory2(pDbCon, pCallForm, emrPatient, dt.Rows[0]["CHARTDATE"].ToString().Trim(), emrForm, dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                else if(emrForm.FmFORMNO != 2465)
                {
                    setChartFormValue2(emrForm, panChart, dt.Rows[0]["OPDATE"].ToString().Trim(),
                                                        dt.Rows[0]["OPSTIME"].ToString().Trim(),
                                                        dt.Rows[0]["OPETIME"].ToString().Trim());
                }
            }
            else if (dt.Rows.Count > 1)
            {
                ssSpd.Visible = true;
                ssSpd.ActiveSheet.RowCount = dt.Rows.Count;
                ssSpd.ActiveSheet.SetRowHeight(-1, 20);

                if (emrForm.FmFORMNO == 2465)
                {
                    if (emrForm.FmUPDATENO == 1)
                    {
                        ssSpd.ActiveSheet.Columns[0].Label = "검사일자";
                        ssSpd.ActiveSheet.Columns[1].Label = "감염";
                        ssSpd.ActiveSheet.Columns[2].Label = "";
                    }
                    else
                    {
                        ssSpd.ActiveSheet.Columns[0].Label = "시술일";
                        ssSpd.ActiveSheet.Columns[1].Label = "시작시간";
                        ssSpd.ActiveSheet.Columns[2].Label = "종료시간";
                    }
                    
                }
                else if (emrForm.FmFORMNO == 2467 || 
                         emrForm.FmFORMNO == 1808 ||
                         emrForm.FmFORMNO == 2280 && emrForm.FmOLDGB != 1 ||
                         emrForm.FmFORMNO == 2636 && emrForm.FmOLDGB != 1 || emrForm.FmFORMNO == 2323
                         )
                {
                    ssSpd.ActiveSheet.Columns[0].Label = "차트일자";
                    ssSpd.ActiveSheet.Columns[1].Label = "차트시간";
                    ssSpd.ActiveSheet.Columns[2].Label = "작성자";
                    ssSpd.ActiveSheet.Columns[3].Label = "저장";
                }
                else if (emrForm.FmFORMNO == 2309 || emrForm.FmFORMNO == 2611 || emrForm.FmFORMNO == 2308 || emrForm.FmFORMNO == 2610 || emrForm.FmFORMNO == 1544)
                {
                    ssSpd.ActiveSheet.Columns[0].Label = "수술일";
                    ssSpd.ActiveSheet.Columns[1].Label = "진단명";
                    ssSpd.ActiveSheet.Columns[2].Label = "수술명";
                }
                else
                {
                    ssSpd.ActiveSheet.Columns[0].Label = "수술일";
                    ssSpd.ActiveSheet.Columns[1].Label = "시작시간";
                    ssSpd.ActiveSheet.Columns[2].Label = "종료시간";
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (emrForm.FmFORMNO == 2465)
                    {
                        if (emrForm.FmOLDGB == 1)
                        {
                            ssSpd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                            ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["EXNAME"].ToString().Trim();
                            ssSpd.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            ssSpd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["OPDATE"].ToString().Trim();
                            ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["OPSTIME"].ToString().Trim();
                            ssSpd.ActiveSheet.Cells[i, 1].Tag = dt.Rows[i]["WRTNO"].ToString().Trim();
                            ssSpd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OPETIME"].ToString().Trim();
                        }
                    }
                    else if (emrForm.FmFORMNO == 2467 ||
                             emrForm.FmFORMNO == 1808 || 
                             emrForm.FmFORMNO == 2280 && emrForm.FmOLDGB != 1 || 
                             emrForm.FmFORMNO == 2636 && emrForm.FmOLDGB != 1 || emrForm.FmFORMNO == 2323)
                    {
                        ssSpd.ActiveSheet.Cells[i, 0].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00"); 
                        ssSpd.ActiveSheet.Cells[i, 1].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                        ssSpd.ActiveSheet.Cells[i, 1].Tag = dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 2].Tag = dt.Rows[i]["EMRNO"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SAVECERT"].ToString().Trim();
                    }
                    else if (emrForm.FmFORMNO == 2309 || emrForm.FmFORMNO == 2611)
                    {
                        ssSpd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["OPDATE"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 1].Tag = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OPTITLE"].ToString().Trim();
                    }
                    else if (emrForm.FmFORMNO == 2308 || emrForm.FmFORMNO == 2610 || emrForm.FmFORMNO == 1544)
                    {
                        ssSpd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["OPDATE"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["PREDIAGNOSIS"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 1].Tag = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OPILL"].ToString().Trim();
                    }
                    else
                    {
                        ssSpd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["OPDATE"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["OPSTIME"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 1].Tag  = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssSpd.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OPETIME"].ToString().Trim();
                    }
                }

                ssSpd.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
                ssSpd.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

                ssSpd.Parent = panChart;
                ssSpd.Visible = true;
                ssSpd.BringToFront();
            }

            dt.Dispose();
        }

        /// <summary>
        /// 진단명을 조회한다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="panChart"></param>
        /// <param name="FindText"></param>
        /// <param name="ssSpd"></param>
        /// <param name="ctlControl"></param>
        /// <param name="strDiagCode"></param>
        public static void DiagControl(PsmhDb pDbCon, Control panChart, string FindText, FarPoint.Win.Spread.FpSpread ssSpd, Control ctlControl, string strDiagCode)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ILLCODED, ILLNAMEK, ILLNAMEE";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
            if (strDiagCode == "rdoEng")
            {
                SQL += ComNum.VBLF + "  WHERE UPPER(ILLNAMEE) LIKE '%" + FindText.Replace("'", "`").ToUpper() + "%'";
            }
            else if (strDiagCode == "rdoKor")
            {
                SQL += ComNum.VBLF + "  WHERE UPPER(ILLNAMEK) LIKE '%" + FindText.Replace("'", "`").ToUpper() + "%'";
            }
            else
            {
                SQL += ComNum.VBLF + "  WHERE UPPER(ILLCODE) LIKE '" + FindText.Replace("'", "`").ToUpper() + "%'";
            }

            SQL += ComNum.VBLF + "    AND IllClass = '1'                   ";
            SQL += ComNum.VBLF + "    AND (NOUSE <> 'N' OR NOUSE IS NULL)  ";
            SQL += ComNum.VBLF + "    AND DDATE IS NULL                    ";

            SQL += ComNum.VBLF + "  ORDER BY ILLUPCODE, ";

            if (strDiagCode == "rdoEng")
            {
                SQL += " ILLNAMEE";
            }
            else if (strDiagCode == "rdoKor")
            {
                SQL += " ILLNAMEK";
            }
            else
            {
                SQL += " ILLCODE";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            ssSpd.ActiveSheet.ColumnCount = 2;
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.Multiline = false;
            TypeText.MaxLength = 500;

            FarPoint.Win.Spread.CellType.TextCellType TypeText2 = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText2.Multiline = false;
            TypeText2.MaxLength = 32000;

            ssSpd.ActiveSheet.Columns[0].CellType = TypeText;
            ssSpd.ActiveSheet.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[0].Width = 50;
            ssSpd.ActiveSheet.Columns[0].Locked = true;
            //ssSpd.ActiveSheet.Columns[0].Visible = true;

            ssSpd.ActiveSheet.Columns[1].CellType = TypeText2;
            ssSpd.ActiveSheet.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[1].Width = 300;
            ssSpd.ActiveSheet.Columns[1].Locked = true;
            //ssSpd.ActiveSheet.Columns[1].Visible = true;

            ssSpd.ActiveSheet.RowCount = dt.Rows.Count;
            ssSpd.ActiveSheet.SetRowHeight(-1, 20);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssSpd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ILLCODED"].ToString().Trim();
                if (strDiagCode == "rdoKor")
                {
                    ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim() + "::   " + dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                }
                else
                {
                    ssSpd.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                }
            }

            ssSpd.ActiveSheet.Columns[0].Width = (int)ssSpd.ActiveSheet.Columns[0].GetPreferredWidth();
            dt.Dispose();

            ssSpd.ActiveSheet.SetActiveCell(0, 0);
            int CtrlWidth = 600;
            ssSpd.Width = CtrlWidth;
            ssSpd.ActiveSheet.SetColumnWidth(1, CtrlWidth - (int) ssSpd.ActiveSheet.Columns[0].Width - 21);

            if (ssSpd.ActiveSheet.RowCount > 10)
            {
                ssSpd.Height = 203;
            }
            else
            {
                //ssSpd.ActiveSheet.SetColumnWidth(1, ctlControl.Width - 4);
                switch (ssSpd.ActiveSheet.RowCount)
                {
                    case 1:
                        ssSpd.Height = 24;
                        break;
                    case 2:
                        ssSpd.Height = 44;
                        break;
                    case 3:
                        ssSpd.Height = 64;
                        break;
                    case 4:
                        ssSpd.Height = 84;
                        break;
                    case 5:
                        ssSpd.Height = 104;
                        break;
                    case 6:
                        ssSpd.Height = 124;
                        break;
                    case 7:
                        ssSpd.Height = 143;
                        break;
                    case 8:
                        ssSpd.Height = 163;
                        break;
                    case 9:
                        ssSpd.Height = 184;
                        break;
                    case 10:
                        ssSpd.Height = 203;
                        break;
                }
            }

            SetChildPositionChart(ctlControl, panChart, ssSpd);

            ssSpd.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            ssSpd.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            //ssSpd.Parent = pCallForm;
            ssSpd.Parent = panChart;
            ssSpd.Visible = true;
            ssSpd.BringToFront();
            //ssSpd.Focus();
            //ssSpd.ActiveSheet.SetActiveCell(0, 0);
        }

        // <summary>
        /// 등록된 상용구를 조회한다(F12)
        /// </summary>
        /// <param name="pCallForm"></param>
        /// <param name="strFormNo"></param>
        /// <param name="ssSpd"></param>
        /// <param name="ctlControl"></param>
        public static void DspControlF12(PsmhDb pDbCon, Form pCallForm, Control panChart, DataTable dt, FarPoint.Win.Spread.FpSpread ssSpd, Control ctlControl)
        {
            if (clsEmrPublic.gstrMcrAllFlag == "")
            {
                clsEmrPublic.gstrMcrAllFlag = "3";
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }
            ssSpd.ActiveSheet.ColumnCount = 3;
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.Multiline = false;
            TypeText.MaxLength = 32000;

            FarPoint.Win.Spread.CellType.TextCellType TypeText2 = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText2.Multiline = true;
            TypeText2.MaxLength = 32000;
            TypeText2.WordWrap = true;

            ssSpd.ActiveSheet.Columns[0].CellType = TypeText;
            ssSpd.ActiveSheet.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[0].Width = 39;
            ssSpd.ActiveSheet.Columns[0].Locked = true;
            ssSpd.ActiveSheet.Columns[0].Visible = true;

            ssSpd.ActiveSheet.Columns[1].CellType = TypeText2;
            ssSpd.ActiveSheet.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[1].Width = 300;
            ssSpd.ActiveSheet.Columns[1].Locked = true;
            ssSpd.ActiveSheet.Columns[1].Visible = false;

            ssSpd.ActiveSheet.Columns[2].CellType = TypeText;
            ssSpd.ActiveSheet.Columns[2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[2].Width = 39;
            ssSpd.ActiveSheet.Columns[2].Locked = true;
            ssSpd.ActiveSheet.Columns[2].Visible = false;

            ssSpd.ActiveSheet.RowCount = dt.Rows.Count;
            ssSpd.ActiveSheet.SetRowHeight(-1, 20);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssSpd.ActiveSheet.Cells[i, 0].Text = VB.Trim(dt.Rows[i]["SYSMPNAME"].ToString());
                ssSpd.ActiveSheet.Cells[i, 1].Text = VB.Trim(dt.Rows[i]["SYSMPRMK"].ToString());
            }

            dt.Dispose();

            ssSpd.Width = ctlControl.Width;

            if (ssSpd.ActiveSheet.RowCount > 10)
            {
                ssSpd.Height = 203;
                ssSpd.ActiveSheet.SetColumnWidth(0, ctlControl.Width - 21);
            }
            else
            {
                ssSpd.ActiveSheet.SetColumnWidth(0, ctlControl.Width - 4);
                switch (ssSpd.ActiveSheet.RowCount)
                {
                    case 1:
                        ssSpd.Height = 24;
                        break;
                    case 2:
                        ssSpd.Height = 44;
                        break;
                    case 3:
                        ssSpd.Height = 64;
                        break;
                    case 4:
                        ssSpd.Height = 84;
                        break;
                    case 5:
                        ssSpd.Height = 104;
                        break;
                    case 6:
                        ssSpd.Height = 124;
                        break;
                    case 7:
                        ssSpd.Height = 143;
                        break;
                    case 8:
                        ssSpd.Height = 163;
                        break;
                    case 9:
                        ssSpd.Height = 184;
                        break;
                    case 10:
                        ssSpd.Height = 203;
                        break;
                }
            }

            SetChildPositionChart(ctlControl, panChart, ssSpd);

            ssSpd.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            ssSpd.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            ssSpd.ActiveSheet.Columns[1].Visible = false;
            ssSpd.ActiveSheet.Columns[0].Locked = true;
            //ssSpd.Parent = pCallForm;
            ssSpd.Parent = panChart;
            ssSpd.Visible = true;
            ssSpd.BringToFront();
            //ssSpd.Focus(); //절대 하면 안됨 텍스트 차팅을 못합니다.
        }

        // <summary>
        /// 컨트롤에 등록된 상용구를 조회한다
        /// </summary>
        /// <param name="pCallForm"></param>
        /// <param name="strFormNo"></param>
        /// <param name="ssSpd"></param>
        /// <param name="ctlControl"></param>
        public static void DspControl(PsmhDb pDbCon, Form pCallForm, Control panChart, string strFormNo, FarPoint.Win.Spread.FpSpread ssSpd, Control ctlControl)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            //optEMRMACRO 상용구 자동 보이기
            string strEMRMACRO = "0";
            dt = clsEmrQuery.GetEmrUserOption(pDbCon, clsType.User.IdNumber, "EMROPTION", "EMRMACRO");
            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    strEMRMACRO = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                }
                dt.Dispose();
                dt = null;
            }
            if (strEMRMACRO == "1")
            {
                return;
            }


            string strConIndex = clsXML.IsArryCon(ctlControl);

            if (clsEmrPublic.gstrMcrAllFlag == "")
            {
                clsEmrPublic.gstrMcrAllFlag = "3";
            }

            //컨트롤에 등록된 상요구를 조회한다
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT MACRONO, TITLE, CONTENT";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE ";
            SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "  AND CONTROLID = '" + ctlControl.Name + "'";
            //if (strConIndex == "")
            //{
            //    SQL = SQL + ComNum.VBLF + "  AND (CONTROLIDIDX IS NULL OR CONTROLIDIDX = '')";
            //}
            //else
            //{
            //    SQL = SQL + ComNum.VBLF + "  AND CONTROLIDIDX = '" + strConIndex + "'";
            //}

            if (clsEmrPublic.gstrMcrAllFlag == "1")
            {
                SQL = SQL + ComNum.VBLF + "      AND USEGB = 'ALL'";
                SQL = SQL + ComNum.VBLF + "      AND GRPGB = 'A'";
            }
            else if (clsEmrPublic.gstrMcrAllFlag == "2")
            {
                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + (clsType.User.DrCode.Length > 0 ? clsType.User.DeptCode : clsType.User.BuseCode) + "'";
                SQL = SQL + ComNum.VBLF + "      AND GRPGB = 'D'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "      AND USEGB = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "      AND GRPGB = 'U'";
            }
            SQL = SQL + ComNum.VBLF + " ORDER BY ORDERSEQ, TITLE";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }
            ssSpd.ActiveSheet.ColumnCount = 3;
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.Multiline = false;
            TypeText.MaxLength = 32000;

            FarPoint.Win.Spread.CellType.TextCellType TypeText2 = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText2.Multiline = true;
            TypeText2.MaxLength = 32000;
            TypeText2.WordWrap = true;

            ssSpd.ActiveSheet.Columns[0].CellType = TypeText;
            ssSpd.ActiveSheet.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[0].Width = 39;
            ssSpd.ActiveSheet.Columns[0].Locked = true;
            ssSpd.ActiveSheet.Columns[0].Visible = true;

            ssSpd.ActiveSheet.Columns[1].CellType = TypeText2;
            ssSpd.ActiveSheet.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[1].Width = 300;
            ssSpd.ActiveSheet.Columns[1].Locked = true;
            ssSpd.ActiveSheet.Columns[1].Visible = false;

            ssSpd.ActiveSheet.Columns[2].CellType = TypeText;
            ssSpd.ActiveSheet.Columns[2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssSpd.ActiveSheet.Columns[2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssSpd.ActiveSheet.Columns[2].Width = 39;
            ssSpd.ActiveSheet.Columns[2].Locked = true;
            ssSpd.ActiveSheet.Columns[2].Visible = false;

            ssSpd.ActiveSheet.RowCount = dt.Rows.Count;
            ssSpd.ActiveSheet.SetRowHeight(-1, 20);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssSpd.ActiveSheet.Cells[i, 0].Text = VB.Trim(dt.Rows[i]["TITLE"].ToString());
                ssSpd.ActiveSheet.Cells[i, 1].Text = VB.Trim(dt.Rows[i]["CONTENT"].ToString());
                ssSpd.ActiveSheet.Cells[i, 2].Text = VB.Trim(dt.Rows[i]["MACRONO"].ToString());
            }

            dt.Dispose();
            dt = null;

            ssSpd.Width = ctlControl.Width;

            if (ssSpd.ActiveSheet.RowCount > 10)
            {
                ssSpd.Height = 203;
                ssSpd.ActiveSheet.SetColumnWidth(0, ctlControl.Width - 21);
            }
            else
            {
                ssSpd.ActiveSheet.SetColumnWidth(0, ctlControl.Width - 4);
                switch (ssSpd.ActiveSheet.RowCount)
                {
                    case 1:
                        ssSpd.Height = 24;
                        break;
                    case 2:
                        ssSpd.Height = 44;
                        break;
                    case 3:
                        ssSpd.Height = 64;
                        break;
                    case 4:
                        ssSpd.Height = 84;
                        break;
                    case 5:
                        ssSpd.Height = 104;
                        break;
                    case 6:
                        ssSpd.Height = 124;
                        break;
                    case 7:
                        ssSpd.Height = 143;
                        break;
                    case 8:
                        ssSpd.Height = 163;
                        break;
                    case 9:
                        ssSpd.Height = 184;
                        break;
                    case 10:
                        ssSpd.Height = 203;
                        break;
                }
            }
            
            SetChildPositionChart(ctlControl, panChart, ssSpd);

            ssSpd.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            ssSpd.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            ssSpd.ActiveSheet.Columns[1].Visible = false;
            ssSpd.ActiveSheet.Columns[0].Locked = true;
            //ssSpd.Parent = pCallForm;
            ssSpd.Parent = panChart;
            ssSpd.Visible = true;
            ssSpd.BringToFront();
            //ssSpd.Focus(); //절대 하면 안됨 텍스트 차팅을 못합니다.
        }

        public static void SetChildPositionChart(Control CallControl, Control panChart, Control ChildControl)
        {

            int intTop = 0;
            int intTopHeight = 0;

            ChildControl.Left = SetAdjustPositionChart(CallControl, panChart, "LEFT");

            intTop = SetAdjustPositionChart(CallControl, panChart, "TOP");
            intTopHeight = intTop + CallControl.Height;

            if (intTopHeight >= 796)
            {
                ChildControl.Top = intTop - ChildControl.Height;
            }
            else
            {
                ChildControl.Top = intTop + CallControl.Height;
            }
        }

        private static int SetAdjustPositionChart(Control cControl, Control panChart, string strOption)
        {
            Control pControl = null;

            int rtnVal = 0;

            if (strOption == "LEFT")
            {
                rtnVal = cControl.Left;
            }
            else
            {
                rtnVal = cControl.Top;
            }

            if (cControl.Parent == null)
            {
                return rtnVal;
            }
            if (cControl.Parent == panChart)
            {
                return rtnVal;
            }

            pControl = cControl.Parent;
            rtnVal = rtnVal + SetAdjustPositionChart(pControl, panChart, strOption);
            return rtnVal;
        }

        private static int SetPositionSp(Control cControl, string strOption)
        {
            Control pControl = null;

            int rtnVal = 0;

            if (strOption == "LEFT")
            {
                rtnVal = cControl.Left;
            }
            else
            {
                rtnVal = cControl.Top;
            }

            if (cControl.Parent == null)
            {
                return rtnVal;
            }
            if (cControl.Parent is Form)
            {
                return rtnVal;
            }
            pControl = cControl.Parent;
            rtnVal = rtnVal + SetPositionSp(pControl, strOption);
            return rtnVal;
        }

        /// <summary>
        /// Top Menu의 버튼을 모두 숨긴다.
        /// </summary>
        /// <param name="usFormTopMenu1"></param>
        /// <param name="strBtnName"></param>
        public static void usBtnHide(Control usFormTopMenu1)
        {
            Control[] controls = ComFunc.GetAllControls(usFormTopMenu1);

            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    if (((Button)control).Name != "mbtnTime")
                    {
                        ((Button)control).Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Top Menu의 버튼을 보이게 한다.
        /// </summary>
        /// <param name="usFormTopMenu1"></param>
        /// <param name="strBtnName"></param>
        public static void usBtnShow(Control usFormTopMenu1, string strBtnName)
        {
            Control[] controls = ComFunc.GetAllControls(usFormTopMenu1);

            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    if (((Button)control).Name == strBtnName)
                    {
                        //usFormTopMenu1.Controls.Remove(((Button)control));
                        ((Button)control).Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Top Menu의 버튼을 권한별로 보이게 한다.
        /// </summary>
        /// <param name="usFormTopMenu1"></param>
        /// <param name="strBtnName"></param>
        public static void usBtnShowReg(Control usFormTopMenu1)
        {
            if (clsType.User.AuAPRINTIN == "1")
            {
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnPrint");
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnPrintNull");
            }
            else
            {
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnPrintNull");
            }

            if (clsType.User.AuAWRITE == "1")
            {
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnClear");
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnSave");
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnSaveTemp");
                clsEmrFunc.usBtnShow(usFormTopMenu1, "mbtnDelete");
            }

            if (clsType.User.AuAMANAGE == "1")
            {
                ((usFormTopMenu)usFormTopMenu1).mbtnSave.Visible = false;
                ((usFormTopMenu)usFormTopMenu1).mbtnClear.Visible = false;
                ((usFormTopMenu)usFormTopMenu1).mbtnDelete.Visible = false;
                ((usFormTopMenu)usFormTopMenu1).mbtnAuthority.Visible = true;
                ((usFormTopMenu)usFormTopMenu1).txtMedFrTime.Font = new Font("굴림체", 9, FontStyle.Bold);
                ((usFormTopMenu)usFormTopMenu1).dtMedFrDate.Font = new Font("굴림체", 9, FontStyle.Bold);
            }
        }

        /// <summary>
        /// 버튼을 로드하면서 이름을 바꾼다
        /// </summary>
        /// <param name="usFormTopMenu1"></param>
        /// <param name="strBtnName"></param>
        /// <param name="strBtnNameChg"></param>
        public static void usBtnShowEx(Control usFormTopMenu1, string strBtnName, string strBtnNameChg)
        {
            Control[] controls = ComFunc.GetAllControls(usFormTopMenu1);

            foreach (Control control in controls)
            {
                if (control is Button)
                {
                    if (((Button)control).Name == strBtnName)
                    {
                        //usFormTopMenu1.Controls.Remove(((Button)control));
                        ((Button)control).Visible = true;
                        if (strBtnNameChg != "")
                        {
                            ((Button)control).Name = strBtnNameChg;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 외래, 입원 서식지를 구분한다.
        /// </summary>
        /// <param name="frmFORM"></param>
        /// <param name="strInOutCls"></param>
        /// <returns></returns>
        public static bool isInOutFix(EmrForm frmFORM, string strInOutCls)
        {
            bool rtnVal = true;

            if (frmFORM.FmINOUTCLS == "0")
            {
                return rtnVal;
            }

            if (frmFORM.FmINOUTCLS == "1")
            {
                if (strInOutCls == "O")
                {
                    return rtnVal;
                }
                else
                {
                    ComFunc.MsgBox("외래전용서식입니다." + ComNum.VBLF + "입원환자는 작성하실수 없습니다.");
                    return false;
                }
            }
            else if (frmFORM.FmINOUTCLS == "2")
            {
                if (strInOutCls == "I")
                {
                    return rtnVal;
                }
                else
                {
                    ComFunc.MsgBox("입원전용서식입니다." + ComNum.VBLF + "외래환자는 작성하실수 없습니다.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 콤보박스에 시간을 세팅한다.
        /// </summary>
        /// <param name="cbo"></param>
        public static void SetTimeComboBox(ComboBox cbo)
        {
            cbo.Items.Add("00:00");
            cbo.Items.Add("01:00");
            cbo.Items.Add("02:00");
            cbo.Items.Add("03:00");
            cbo.Items.Add("04:00");
            cbo.Items.Add("05:00");
            cbo.Items.Add("06:00");
            cbo.Items.Add("07:00");
            cbo.Items.Add("08:00");
            cbo.Items.Add("09:00");
            cbo.Items.Add("10:00");
            cbo.Items.Add("11:00");
            cbo.Items.Add("12:00");
            cbo.Items.Add("13:00");
            cbo.Items.Add("14:00");
            cbo.Items.Add("15:00");
            cbo.Items.Add("16:00");
            cbo.Items.Add("17:00");
            cbo.Items.Add("18:00");
            cbo.Items.Add("19:00");
            cbo.Items.Add("20:00");
            cbo.Items.Add("21:00");
            cbo.Items.Add("22:00");
            cbo.Items.Add("23:00");
        }

        /// <summary>
        /// 조회기간의 내원일자를 세팅한다.
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <param name="pTmp"></param>
        /// <param name="dtpFr"></param>
        /// <param name="dtpEnd"></param>
        public static void SetMedFrEndDate(PsmhDb pDbCon, string strEmrNo, EmrPatient pTmp, DateTimePicker dtpFr, DateTimePicker dtpEnd)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strMedFrDate = "";
            string strMedEndDate = "";
            string strCHARTDATE = "";

            string strFormNo = "0";
            string strUpdateNo = "0";

            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D");

            if (VB.Val(strEmrNo) != 0)
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "      P.INDATE AS MEDFRDATE, P.OUTDATE AS MEDENDDATE";
                SQL = SQL + ComNum.VBLF + "      , A.CHARTDATE, A.FORMNO, A.UPDATENO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.EMR_TREATT P";
                SQL = SQL + ComNum.VBLF + "   ON A.ACPNO = P.TREATNO";
                SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("내원내역 조회도중 오류가 발생했습니다.");
                    dt.Dispose();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMedFrDate = ComFunc.FormatStrToDate(dt.Rows[0]["MEDFRDATE"].ToString().Trim(), "D");
                    strMedEndDate = ComFunc.FormatStrToDate(dt.Rows[0]["MEDENDDATE"].ToString().Trim(), "D");
                    strCHARTDATE = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                    strUpdateNo = dt.Rows[0]["UPDATENO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }

            if (VB.Val(strEmrNo) != 0)
            {
                //EmrForm fView = clsEmrChart.ClearEmrForm();
                //fView = clsEmrChart.SerEmrFormInfo(strFormNo, strUpdateNo);

                if (pTmp.inOutCls == "O" && (strFormNo != "769"))
                {
                    dtpFr.Value = Convert.ToDateTime(strMedFrDate);

                    string strChartEndDate = "";
                    if (dtpEnd != null)
                    {
                        SQL = "SELECT ";
                        SQL = SQL + ComNum.VBLF + "      MAX(A.CHARTDATE) AS CHARTDATE";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                        SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + pTmp.acpNo;

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strChartEndDate = ComFunc.FormatStrToDate(dt.Rows[0]["CHARTDATE"].ToString().Trim(), "D");
                        }
                        dt.Dispose();
                        dt = null;
                        if (strChartEndDate != "")
                        {
                            dtpEnd.Value = Convert.ToDateTime(strChartEndDate);
                        }
                        else
                        {
                            dtpEnd.Value = Convert.ToDateTime(strMedFrDate);
                        }
                    }
                }
                else
                {
                    //420	임상관찰기록지(병동) 423	섭취배설기록지 526	간호활동 기록지 646	ICU 기록지     769 임상관찰기록지(주사실)

                    if ((strFormNo == "420") || (strFormNo == "423") || (strFormNo == "526") || (strFormNo == "646") || (strFormNo == "769"))
                    //if (fView.FmVIEWGROUP == 1)
                    {
                        dtpFr.Value = Convert.ToDateTime(strCHARTDATE);
                        if (dtpEnd != null) dtpEnd.Value = Convert.ToDateTime(strCHARTDATE);
                    }
                    else
                    {
                        if (strMedEndDate.Replace("-", "") == "" || strMedEndDate.Replace("-", "") == "99991231" || strMedEndDate.Replace("-", "") == "99981231")
                        {
                            if(strMedFrDate.Length > 0 )
                            {
                                dtpFr.Value = Convert.ToDateTime(strMedFrDate);
                                if (dtpEnd != null) dtpEnd.Value = Convert.ToDateTime(strCurDate);
                            }
                        }
                        else
                        {
                            if (strMedFrDate.Length > 0)
                            {
                                dtpFr.Value = Convert.ToDateTime(strMedFrDate);
                                if (dtpEnd != null) dtpEnd.Value = Convert.ToDateTime(strMedEndDate);
                            }
                        }
                    }
                }

            }
            else
            {
                if (pTmp != null)
                {
                    if (pTmp.medEndDate.Replace("-", "") == "" || pTmp.medEndDate.Replace("-", "") == "99991231" || pTmp.medEndDate.Replace("-", "") == "99981231")
                    {
                        if (pTmp.medFrDate != "")
                        {
                            dtpFr.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(pTmp.medFrDate, "D"));
                        }
                        else
                        {
                            dtpFr.Value = Convert.ToDateTime(strCurDate);
                        }

                        if (dtpEnd != null) dtpEnd.Value = Convert.ToDateTime(strCurDate);
                    }
                    else
                    {
                        if (pTmp.medFrDate != "")
                        {
                            dtpFr.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(pTmp.medFrDate, "D"));
                            if (dtpEnd != null) dtpEnd.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(pTmp.medEndDate, "D"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 차트 시간을 보고 Duty를 반환한다.
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static string DutyGet(string strTime)
        {
            string rtnVal = "";

            if (strTime.Trim() == "")
            {
                return "Day";
            }
            strTime = strTime.Replace(":", "");
            strTime = strTime.Replace("-", "");

            int intTime = Convert.ToInt32(VB.Val(VB.Left(strTime, 4)));

            if (intTime < 800)
            {
                rtnVal = "Night";
            }
            else if (intTime >= 800 && intTime < 1500)
            {
                rtnVal = "Day";
            }
            else if (intTime >= 1500 && intTime < 2200)
            {
                rtnVal = "Evening";
            }
            else
            {
                rtnVal = "Night";
            }
            return rtnVal;
        }

        public static string GetGrpNm(string strGrpNo)
        {
            string rtnVal = "";
            switch (strGrpNo)
            {
                case "1":
                    rtnVal = "입원기록지";
                    break;
                case "2":
                    rtnVal = "경과기록지";
                    break;
                case "3":
                    rtnVal = "수술기록지";
                    break;
                case "4":
                    rtnVal = "퇴원요약지";
                    break;
                case "5":
                    rtnVal = "전과기록지";
                    break;
            }

            return rtnVal;
        }

        public static bool chkVisibe(string strName)
        {
            bool rtnVal = false;

            switch (strName)
            {
                case "유":
                case "있음":
                case "Y":
                    rtnVal = true;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 이미지를 저장할 경로를 설정한다.
        /// </summary>
        public static void CheckImageJobFold(ref string mstrFoldJob, ref string mstrFoldBase, string strFormNo, string strUpdateNo, string strEmrNo, string strItemName)
        {
            //기록지에서 올 경우는 EmrImageTmp\실행파일이름\New\\기록지번호\아이템이름
            //기록지에서 올 경우는 EmrImageTmp\실행파일이름\New\\BaseImage
            //단독으로 올 경우는 EmrImageTmp\실행파일이름\Update\EMRNO\아이템이름
            //단독으로 올 경우는 EmrImageTmp\실행파일이름\Update\BaseImage

            //이미지 번호를 순차적으로 미리 발생을 시킨다.
            //서식지에 미리 이미지번호를 세팅할 변수를 할당한다.
            //기본이미지를 세팅을 한다.
            //개인별, 과별로 기본 이미지를 세팅을 한다.
            //

            string strJobFold = "";
            string strBaseFold = "";

            if (VB.Val(strEmrNo) == 0) //신규등록
            {
                strJobFold = "\\EmrImageTmp\\New\\" +  strFormNo + "_" + strUpdateNo + "_" + strItemName;
                strBaseFold = "\\EmrImageTmp\\New\\" + strFormNo + "_" + strUpdateNo + "_BaseImage";
            }
            else
            {
                strJobFold = "\\EmrImageTmp\\Update\\" + strEmrNo + "_" + strItemName;
                strBaseFold = "\\EmrImageTmp\\Update\\" + strEmrNo + "_BaseImage";
            }

            mstrFoldJob = clsEmrType.EmrSvrInfo.EmrClient + strJobFold;
            mstrFoldBase = clsEmrType.EmrSvrInfo.EmrClient + strBaseFold;

            if (Directory.Exists(mstrFoldJob) == false)
            {
                Directory.CreateDirectory(mstrFoldJob);
            }
            if (Directory.Exists(mstrFoldBase) == false)
            {
                Directory.CreateDirectory(mstrFoldBase);
            }
        }

        /// <summary>
        /// 이미지 작업폴더를 삭제를 한다.
        /// </summary>
        /// <param name="strJobFold"></param>
        /// <param name="strBaseFold"></param>
        public static void DeleteImageJobFold(string strJobFold, string strBaseFold)
        {
            try
            {
                if (Directory.Exists(strJobFold) == true)
                {
                    DirectoryInfo diSource1 = new DirectoryInfo(strJobFold);
                    if (diSource1.GetFiles().Length > 0)
                    {
                        clsScan.DeleteFoldAll(strJobFold);
                    }

                    diSource1 = null;
                    Directory.Delete(strJobFold);
                }
                if (Directory.Exists(strBaseFold) == true)
                {
                    DirectoryInfo diSource1 = new DirectoryInfo(strBaseFold);
                    if (diSource1.GetFiles().Length > 0)
                    {
                        clsScan.DeleteFoldAll(strBaseFold);
                    }

                    diSource1 = null;
                    Directory.Delete(strBaseFold);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 모든 이미지 작업 폴드를 삭제한다
        /// 프로그램 로드시, 종료시에
        /// </summary>
        public static void DeleteImageJobFoldAll()
        {
            string strJobFold1 = "";
            string strBaseFold1 = "";
            string strJobFold2 = "";
            string strBaseFold2 = "";
            string strScanFold = "";
            //기록지 이미지
            strJobFold1 = clsEmrType.EmrSvrInfo.EmrClient + "\\EmrImageTmp\\New";
            strBaseFold1 = clsEmrType.EmrSvrInfo.EmrClient + "\\EmrImageTmp\\New";
            strJobFold2 = clsEmrType.EmrSvrInfo.EmrClient + "\\EmrImageTmp\\Update";
            strBaseFold2 = clsEmrType.EmrSvrInfo.EmrClient + "\\EmrImageTmp\\Update";
            strScanFold = clsEmrType.EmrSvrInfo.EmrClient + "\\ScanTmp";
            try
            {
                if (Directory.Exists(strJobFold1) == true)
                {
                    ComFunc.DeleteDirectory(strJobFold1, true);
                }
            }
            catch { }
            try
            {
                if (Directory.Exists(strBaseFold1) == true)
                {
                    ComFunc.DeleteDirectory(strBaseFold1, true);
                }
            }
            catch { }
            try
            {
                if (Directory.Exists(strJobFold2) == true)
                {
                    ComFunc.DeleteDirectory(strJobFold2, true);
                }
            }
            catch { }
            try
            {
                if (Directory.Exists(strBaseFold2) == true)
                {
                    ComFunc.DeleteDirectory(strBaseFold2, true);
                }
            }
            catch { }
            try
            {
                if (Directory.Exists(strScanFold) == true)
                {
                    ComFunc.DeleteDirectory(strScanFold, true);
                }
            }
            catch { }
        }
        
        /// <summary>
        /// 서식지 폼의 Tag값을 분석해서 이벤트를 처리한다.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ct"></param>
        /// <param name="strTag"></param>
        public static void SetControlEvent(PsmhDb pDbCon, Form frm, Control ct, string strTag, EmrPatient AcpEmr = null, string strChartDate = "")
        {
            string strFunc = VB.Split(strTag, ":")[0];

            if (strFunc == "PanVisible")
            {
                PanVisible(frm, strTag);
            }
            else if (strFunc == "PanVisibleChk")
            {
                PanVisibleChk(frm, ct, strTag);
            }
            else if (strFunc == "PanVisibleTrueFalse")
            {
                PanVisibleTrueFalse(frm, strTag);
            }
            else if (strFunc == "RadioCheck")
            {
                RadioCheck(frm, strTag);
            }
            else if (strFunc == "CheckBoxCheck")
            {
                CheckBoxCheck(frm, strTag);
            }
            else if (strFunc == "PanVisibleCheckBox")
            {
                PanVisibleCheckBox(frm, ct, strTag);
            }
            else if (strFunc == "PanChkAllCheck")
            {
                PanChkAllCheck(frm, ct, strTag);
            }
            else if(strFunc == "PanChkEndoPartCheck")//2021-04-26
            {
                PanChkEndoPartCheck(frm, ct, strTag);
            }
            else if (strFunc == "PanChkCleanCheck")
            {
                PanChkCleanCheck(frm, strTag);
            }
            else if (strFunc == "GetChartHis")
            {
                GetChartHis(pDbCon, frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "GetSetVitalOut")
            {
                GetSetVitalOut(pDbCon, frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "GetSetItemHis")
            {
                GetSetItemHis(pDbCon, frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "GetSetSOAP") //GetSetSOAP:I0000000748,I0000014624,I0000011772,I0000014792,I0000003280
            {
                GetSetSOAP(frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "TimeSet")
            {
                TimeSet(pDbCon, frm, strTag);
            }
            else if (strFunc == "UMLCodesSet")
            {
                UMLCodesSet(frm, ct, strTag);
            }
            else if (strFunc == "PanDeleteClick")
            {
                PanDeleteClick(frm, strTag);
            }
            else if (strFunc == "GetSetPanelItme")
            {
                GetSetPanelItme(pDbCon, frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "GetSetItemLock")
            {
                GetSetItemLock(pDbCon, frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "ItmeLockDis")
            {
                ItmeLockDis(frm, strTag);
            }
            else if (strFunc == "ItemMisMatch")
            {
                ItemMisMatch(pDbCon, frm, ct, strTag, AcpEmr, strChartDate);
            }
            else if (strFunc == "CheckLock") // 지정한 아이템 값이 트루이면 지정한 아이템을 제외한 지정한 패널안의 첵크 박스들은 다 펄스가 된다.
            {
                CheckLock(frm, ct, strTag);
            }
            else if (strFunc == "PainScaleHelp")
            {
                PainScaleHelp(frm, strTag);
            }
            else if (strFunc == "AddSign")
            {
                AddSign(pDbCon, frm, strTag, AcpEmr);
            }
            else if (strFunc == "PneumoniaSum")
            {
                PneumoniaSum(frm, strTag);
            }
            else if (strFunc == "SetMainDiagnosis")
            {
                DiagTextChange(frm, strTag);
            }
            else if (strFunc == "SetMainDiagnosisNew")
            {
                DiagTextChangeNew(frm, strTag);
            }
            else if (strFunc == "GetTransferHistory")
            {
                GetTransferHistory(pDbCon, frm, strChartDate, AcpEmr.ptNo);
            }
            else if (strFunc == "OPD F/U")
            {
                SetAutoOPDFU(frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_OpName")
            {
                Set_FormPatInfo_OpName(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_IpdTrans")
            {
                Set_FormPatInfo_IpdTrans(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_TestResult")
            {
                Set_FormPatInfo_TestResult(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_LabResult")
            {
                Set_FormPatInfo_LabResult(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_LabResultNew")
            {
                Set_FormPatInfo_LabResultNew(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_MedicineDischarge")
            {
                Set_FormPatInfo_MedicineDischarge(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_MedicineDischargeKor")
            {
                Set_FormPatInfo_MedicineDischarge(pDbCon, AcpEmr, frm, strTag, true);
            }
            else if (strFunc == "Set_Form_TextCopy")
            {
                Set_Form_TextCopy(frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_SubDisease_Eng")
            {
                Set_FormPatInfo_SubDisease_Eng(pDbCon, AcpEmr, frm, strTag);
            }
            else if (strFunc == "Set_PanelActive")
            {
                Set_PanelActive(frm, strTag);
            }
            else if (strFunc == "CheckSum")
            {
                CheckSum(frm, strTag);
            }
            else if (strFunc == "CheckSumPlus")
            {
                CheckSumPlus(frm, strTag);
            }
            else if (strFunc == "Set_AutoPanelVisible")
            {
                Set_AutoPanelVisible(frm, ct, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_ER")
            {
                Set_FormPatInfo_ER(pDbCon, AcpEmr, frm);
            }
            else if (strFunc == "Set_FormPatInfo_Secret")
            {
                Set_FormPatInfo_Secret(pDbCon, AcpEmr, frm, ct);
            }
            else if (strFunc == "Set_FormPatInfo_Er_Trans")
            {
                Set_FormPatInfo_Er_Trans(pDbCon, AcpEmr, frm, ct);
            }
            else if (strFunc == "Set_ControlVisible")
            {
                Set_ControlVisible(frm, strTag);
            }
            else if (strFunc == "Set_ControlVisible2")
            {
                Set_ControlVisible2(frm, strTag);
            }
            else if (strFunc == "Set_ControlVisible3")
            {
                Set_ControlVisible3(frm, strTag);
            }
            else if (strFunc == "Set_CheckControlVisible")
            {
                Set_CheckControlVisible(frm, ct, strTag);
            }
            else if (strFunc == "Set_CheckControlVisible_Order")
            {
                Set_CheckControlVisible_Order(frm, ct, strTag);
            }
            else if (strFunc == "Set_ControlCheck")
            {
                Set_ControlCheck(frm, strTag);
            }
            else if (strFunc == "Set_PanelControl")
            {
                Set_PanelControl(frm, strTag);
            }
            else if (strFunc == "Set_PanelSort")
            {
                Set_PanelSort(frm, strTag);
            }
            else if (strFunc == "Textmultiply")
            {
                Textmultiply(frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_INFECT")
            {
                Set_FormPatInfo_INFECT(pDbCon, frm, AcpEmr, strTag);
            }
            //else if (strFunc == "Set_FormPatInfo_OPD_RESERVED")
            //{
            //    Set_FormPatInfo_OPD_RESERVED(pDbCon, frm, AcpEmr, strTag);
            //}
            else if (strFunc == "Set_CheckValVisible")
            {
                Set_CheckValVisible(frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_OpdOrder")
            {
                Set_FormPatInfo_Order(pDbCon, frm, AcpEmr, strTag);
            }
            else if (strFunc == "setChartFormValue2311_New")
            {
                setChartFormValue2311_New(pDbCon, frm, AcpEmr);
            }
            else if (strFunc == "Set_FormPatInfo_GetItemValue")
            {
                Set_FormPatInfo_GetItemValue(pDbCon, frm, AcpEmr, strTag);
            }
            else if (strFunc == "Set_ControlText")
            {
                Set_ControlText(frm, ct, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_ExamResult")
            {
                Set_FormPatInfo_ExamResult(pDbCon, frm, AcpEmr);
            }
            else if (strFunc == "Set_FormPatInfo_2751")
            {
                Set_FormPatInfo_2751(pDbCon, frm, AcpEmr);
            }
            else if (strFunc == "TextSum")
            {
                TextSum(frm, strTag);
            }
            else if (strFunc == "Set_FormPatInfo_LastVS")
            {
                Set_FormPatInfo_LastVS(pDbCon, frm, AcpEmr);
            }
            else if (strFunc == "Set_Blood_TimeCal")
            {
                Set_Blood_TimeCal(frm, strTag);
            }
            else if (strFunc == "Set_Blood_TimeEnd")
            {
                Set_Blood_TimeEnd(frm, strTag);
            }
            else if (strFunc == "Set_GetSabunBuseName")
            {
                Set_GetSabunBuseName(pDbCon, frm, strTag);
            }
            else if (strFunc == "TextMinus")
            {
                TextMinus(frm, strTag);
            }
            else if (strFunc == "TextMinus2")
            {
                TextMinus(frm, strTag, "1");
            }
            else if (strFunc == "Set_PanelText_Clear")
            {
                Set_PanelText_Clear(frm, strTag);
            }
            else if (strFunc == "SetChartFormValue2611_New_Vital")
            {
                setChartFormValue2611_New_Vital(pDbCon, frm, AcpEmr);
            }
            else if (strFunc == "Set_FormPatInfo_DeathDate")
            {
                Set_FormPatInfo_DeathDate(pDbCon, frm, strTag, AcpEmr);
            }
            else if (strFunc == "Set_FormPatInfo_GETHUDATE")
            {
                Set_FormPatInfo_GETHUDATE(pDbCon, frm, strTag, AcpEmr);
            }
        }
            
        /// <summary>
        /// 수술명 가져오기
        /// </summary>
        public static void Set_FormPatInfo_OpName(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_OpName(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
            }
        }

        /// <summary>
        /// 전과정보 가져오기
        /// </summary>
        public static void Set_FormPatInfo_IpdTrans(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_IpdTrans(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
            }
        }

        /// <summary>
        /// 검사결과 가져오기
        /// </summary>
        public static void Set_FormPatInfo_TestResult(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_TestResult(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
            }
        }

        /// <summary>
        /// Lab 가져오기
        /// </summary>
        public static void Set_FormPatInfo_LabResult(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_LabResult(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
            }
        }

        /// <summary>
        /// Lab 가져오기
        /// </summary>
        public static void Set_FormPatInfo_LabResultNew(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_LabResultNew(pDbCon, AcpEmr);
            }
        }

        /// <summary>
        /// 퇴원약 가져오기
        /// </summary>
        public static void Set_FormPatInfo_MedicineDischarge(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag, bool Kor = false)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_MedicineDischarge(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, Kor);

                if (string.IsNullOrWhiteSpace(controls[0].Text) == false)
                {
                    controls = frm.Controls.Find("I0000035338", true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as CheckBox).Checked = false;
                    }
                }
                    
            }

        }

        /// <summary>
        /// 퇴원약 가져오기(한글)
        /// </summary>
        public static void Set_FormPatInfo_MedicineDischargeKor(PsmhDb pDbCon, EmrPatient AcpEmr, Form frm, string strTag)
        {
            Control[] controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = FormPatInfoFunc.Set_FormPatInfo_MedicineDischarge(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, true);
            }
        }

        public static void setChartFormValue3129_1(PsmhDb pDbCon, Form frm, EmrPatient patient, EmrForm pForm)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            string strWARD = string.Empty;
            string strROOM = string.Empty;

            string strValue = string.Empty;

            Control control;

            SQL = " SELECT WARDCODE, ROOM  ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + patient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND INTIME >= TO_DATE('" + patient.medFrDate + " 0000','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   AND INTIME <= TO_DATE('" + patient.medFrDate + " 2359','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   ORDER BY INTIME DESC ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("입원 병동/호실 정보가 없습니다. (간호사가 직접 입력)");
                dt.Dispose();
                return;
            }
            else if (dt.Rows.Count > 0)
            {
                strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                strROOM = dt.Rows[0]["ROOM"].ToString().Trim();

                if (strWARD == "83" && strROOM == "856")
                {
                    strValue = "15. 일반병실 음압격리실로 입원";
                }
                else if (strWARD == "60" && strROOM == "608")
                {
                    strValue = "15. 일반병실 음압격리실로 입원";
                }
                else if (strWARD == "83")
                {
                    strValue = "12. 응급전용병실로 입원";
                }
                else if (strROOM == "330")
                {
                    strValue = "21. 응급전용 중환자실로 입원";
                }
                else if (strROOM == "351" || strROOM == "352" || strROOM == "353" || strROOM == "354")
                {
                    strValue = "23. 일반중환자실의 일반격리실로 입원";
                }
                else if (strROOM == "355")
                {
                    strValue = "24. 일반중환자실의 음압격리실로 입원";
                }
                else if (strROOM == "331" || strROOM == "332" || strROOM == "333" || strROOM == "334" || strROOM == "335")
                {
                    strValue = "25. 응급전용 중환자실의 일반격리실로 입원";
                }
                else if (strROOM == "350")
                {
                    strValue = "28. 그 외 중환자실로 입원";
                }
                else if (strROOM == "408" || strROOM == "409" || strROOM == "410" || strROOM == "412")
                {
                    strValue = "31. 호스피스병동으로 입원";
                }
                else if (strROOM != "0" && strROOM != "")
                {
                    strValue = "11. 일반병실로 입원";
                }

                control = frm.Controls.Find("I0000013184", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strValue;
                }

                if (strValue == "")
                {
                    //ComFunc.MsgBox("입원 병동/호실 정보가 없습니다. (간호사가 직접 입력)");
                }

            }
            dt.Dispose();

            strValue = "";

            SQL = " SELECT PTMIAREA FROM KOSMOS_PMPA.VIEW_ER_EMIHPTMI ";
            SQL = SQL + ComNum.VBLF + " WHERE PTMIIDNO = '" + patient.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND PTMIINDT = '" + patient.medFrDate + "' ";
            SQL = SQL + ComNum.VBLF + " ORDER BY PTMIINTM DESC ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }
            else if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["PTMIAREA"].ToString().Trim())
                {
                    case "1":
                        strValue = "1.일반격리병상";
                        break;
                    case "2":
                        strValue = "2.음압격리병상";
                        break;
                    case "3":
                        strValue = "3.중증응급환자 진료구역";
                        break;
                    case "4":
                        strValue = "4.응급환자 진료구역";
                        break;
                    case "5":
                        strValue = "5.소생실";
                        break;
                    case "6":
                        strValue = "6.처치실";
                        break;
                    case "8":
                        strValue = "8.기타(1~6을 제외한 모든 구역)";
                        break;
                    case "9":
                        strValue = "9.미상(구역을 모를 경우)";
                        break;
                }

                control = frm.Controls.Find("I0000031631", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strValue;
                }
            }

            dt.Dispose();
        }


        public static void setChartFormValue2279_1(PsmhDb pDbCon, Form frm, EmrPatient patient, EmrForm pForm)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            string strValue = string.Empty;

            Control control;
            
            control = frm.Controls.Find("I0000033506", true).FirstOrDefault();
            if (control != null)
            {
                control.Text = "ER";
            }

            control = frm.Controls.Find("I0000033508_1", true).FirstOrDefault();
            if (control != null)
            {
                control.Text = "ER";
            }


            SQL = " SELECT INWARD FROM KOSMOS_PMPA.NUR_MASTER ";
            SQL = SQL + ComNum.VBLF + " WHERE INDATE >= TO_DATE('" + patient.medFrDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + patient.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   ORDER BY INDATE DESC ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strValue = dt.Rows[0]["INWARD"].ToString().Trim();

                control = frm.Controls.Find("I0000033508_2", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strValue;
                }

            }
            dt.Dispose();

        }


        /// <summary>
        /// VITAL 읽기 
        /// 2074
        /// VB setChartFormValue함수 컨버전
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue(PsmhDb pDbCon, Form frm, EmrPatient patient)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            Control[] controls;

            SQL = " SELECT PTMIHIBP HBP, PTMILOBP LBP, PTMIPULS HR, PTMIBRTH RR, PTMIBDHT BT ";
            SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + "WHERE PTMIIDNO = '" + patient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND PTMIINDT = '" + patient.medFrDate + "' ";
            SQL += ComNum.VBLF + "   AND SEQNO =";
            SQL += ComNum.VBLF + " (SELECT MAX(SEQNO)";
            SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + "   WHERE PTMIIDNO = '" + patient.ptNo + "'";
            SQL += ComNum.VBLF + "     AND PTMIINDT = '" + patient.medFrDate + "')";
            SQL += ComNum.VBLF + "   ORDER BY PTMIINTM DESC";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            if (dt.Rows.Count > 0)
            {
                #region it5~10 저장
                controls = frm.Controls.Find("it5", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["HBP"].ToString().Trim();
                }

                controls = frm.Controls.Find("it6", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["LBP"].ToString().Trim();
                }

                controls = frm.Controls.Find("it7", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["HR"].ToString().Trim();
                }

                controls = frm.Controls.Find("it8", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["RR"].ToString().Trim();
                }

                controls = frm.Controls.Find("it10", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["BT"].ToString().Trim();
                }
                #endregion
            }
            dt.Dispose();


            string strInDate = VB.Left(patient.medFrDate, 8);
            strInDate = VB.Left(strInDate, 4) + "-" + VB.Mid(strInDate, 5, 2) + "-" + VB.Right(strInDate, 2);

            string strInTime = VB.Left(patient.medFrTime, 4);
            strInTime = VB.Left(strInTime, 2) + ":" + VB.Right(strInTime, 2);


            SQL = " SELECT  HIBP, LOBP, PULS, BRTH, BDHT";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_PATIENT";
            SQL += ComNum.VBLF + " WHERE PANO = '" + patient.ptNo + "' ";
            SQL += ComNum.VBLF + "   AND INTIME = TO_DATE('" + strInDate + " " + strInTime + "','YYYY-MM-DD HH24:MI') ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                #region it5~10 저장
                controls = frm.Controls.Find("it5", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["HIBP"].ToString().Trim();
                }

                controls = frm.Controls.Find("it6", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["LOBP"].ToString().Trim();
                }

                controls = frm.Controls.Find("it7", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["PULS"].ToString().Trim();
                }

                controls = frm.Controls.Find("it8", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["BRTH"].ToString().Trim();
                }

                controls = frm.Controls.Find("it10", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["BDHT"].ToString().Trim();
                }
                #endregion
            }
            dt.Dispose();
        }

        /// <summary>
        /// VITAL 읽기 
        /// 2737
        /// VB setChartFormValue10함수 컨버전
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue10(PsmhDb pDbCon, Form frm, EmrPatient patient, EmrForm pForm)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (pForm.FmOLDGB == 1)
            {
                #region XML 쿼리
                SQL = " SELECT ";
                SQL += ComNum.VBLF + " extractValue(chartxml, '//it3') IT3, ";
                SQL += ComNum.VBLF + " extractValue(chartxml, '//it5') IT5, ";
                SQL += ComNum.VBLF + " extractValue(chartxml, '//it6') IT6, ";
                SQL += ComNum.VBLF + " extractValue(chartxml, '//it7') IT7, ";
                SQL += ComNum.VBLF + " extractValue(chartxml, '//it8') IT8";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + " WHERE MEDFRDATE = '" + patient.medFrDate + "' ";
                SQL += ComNum.VBLF + "   AND FORMNO = 1550";
                SQL += ComNum.VBLF + "   AND PTNO = '" + patient.ptNo + "'"; ;
                #endregion
            }
            else
            {
                #region 신규 테이블 쿼리
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "       R.ITEMCD, R.ITEMINDEX, R.ITEMTYPE, R.ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "       ON R.EMRNO = A.EMRNO";
                SQL = SQL + ComNum.VBLF + "      AND R.EMRNOHIS = A.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "      AND R.ITEMCD IN";
                SQL = SQL + ComNum.VBLF + "      (";
                SQL = SQL + ComNum.VBLF + "      'I0000035002', 'I0000035003', 'I0000000418', 'I0000000002', -- 임신주수 주, 일, 체중, 신장";
                SQL = SQL + ComNum.VBLF + "      'I0000017712', 'I0000010747', 'I0000018853' -- 머리둘레,가슴둘레, 복부둘레";
                SQL = SQL + ComNum.VBLF + "      )";
                SQL = SQL + ComNum.VBLF + "    WHERE A.PTNO = '" + patient.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "      AND A.MEDFRDATE = '" + patient.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "      AND A.FORMNO = 1550";
                #endregion
            }

            SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            if (dt.Rows.Count > 0)
            {
                if (pForm.FmOLDGB == 1)
                {
                    #region 이전 XML
                    if (elementEmpty(frm, "it1")) setElement(frm, "it1", dt.Rows[0]["it3"].ToString().Trim());
                    if (elementEmpty(frm, "it3")) setElement(frm, "it3", dt.Rows[0]["it5"].ToString().Trim());
                    if (elementEmpty(frm, "it4")) setElement(frm, "it4", dt.Rows[0]["it6"].ToString().Trim());
                    if (elementEmpty(frm, "it5")) setElement(frm, "it5", dt.Rows[0]["it7"].ToString().Trim());
                    if (elementEmpty(frm, "it6")) setElement(frm, "it6", dt.Rows[0]["it8"].ToString().Trim());
                    #endregion

                }
                else
                {
                    #region 신규
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Control control = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true).FirstOrDefault();
                        if (control != null)
                        {
                            if (control is TextBox)
                            {
                                control.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            }
                            else if (control is CheckBox)
                            {
                                (control as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                            }
                            else if (control is RadioButton)
                            {
                                (control as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                            }

                            if (control.Visible == false)
                            {
                                PanelAutoSize(control.Parent, true);
                            }
                        }
                    }
                    #endregion
                }
                
            }
            dt.Dispose();
        }

        /// <summary>
        /// VITAL 
        /// 폼번호: 2605, 2676
        /// VB setChartFormValue2605 컨버전
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue2605(PsmhDb pDbCon, Form frm, EmrPatient emrPatient, EmrForm pForm)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            Control[] controls;

            SQL = " SELECT PTMIRESP, PTMIHIBP, PTMILOBP, PTMIPULS, PTMIBRTH, PTMIBDHT, PTMIVOXS ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + " WHERE PTMIIDNO = '" + emrPatient.ptNo + "' ";
            SQL += ComNum.VBLF + "   AND PTMIINDT = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND SEQNO =";
            SQL += ComNum.VBLF + " (SELECT MAX(SEQNO)";
            SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + "   WHERE PTMIIDNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "     AND PTMIINDT = '" + emrPatient.medFrDate + "')";
            SQL += ComNum.VBLF + "   ORDER BY PTMIINTM DESC";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                string strInDate = VB.Left(emrPatient.medFrDate, 8);
                strInDate = VB.Left(strInDate, 4) + "-" + VB.Mid(strInDate, 5, 2) + "-" + VB.Right(strInDate, 2);

                string strInTime = VB.Left(emrPatient.medFrTime, 4);
                strInTime = VB.Left(strInTime, 2) + ":" + VB.Right(strInTime, 2);


                SQL = " SELECT  RESP, HIBP, LOBP, PULS, BRTH, BDHT, VOXS ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_PATIENT";
                SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "' ";
                SQL += ComNum.VBLF + "   AND INTIME = TO_DATE('" + strInDate + " " + strInTime + "','YYYY-MM-DD HH24:MI') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    #region it4~10 저장
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it4" : "I0000031260", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["RESP"].ToString().Trim();
                    }
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it5" : "I0000002018", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["HIBP"].ToString().Trim();
                    }
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it6" : "I0000001765", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["LOBP"].ToString().Trim();
                    }
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it7" : "I0000014815", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["PULS"].ToString().Trim();
                    }
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it8" : "I0000002009", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["BRTH"].ToString().Trim();
                    }
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it9" : "I0000001811", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["BDHT"].ToString().Trim();
                    }
                    controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it10" : "I0000008708", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt.Rows[0]["VOXS"].ToString().Trim();
                    }
                    #endregion
                }
                dt.Dispose();
                return;
            }

            if (dt.Rows.Count > 0)
            {
                #region it4~10 저장
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it4" : "I0000031260", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMIRESP"].ToString().Trim();
                }
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it5" : "I0000002018", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMIHIBP"].ToString().Trim();
                }
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it6" : "I0000001765", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMILOBP"].ToString().Trim();
                }
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it7" : "I0000014815", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMIPULS"].ToString().Trim();
                }
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it8" : "I0000002009", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMIBRTH"].ToString().Trim();
                }
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it9" : "I0000001811", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMIBDHT"].ToString().Trim();
                }
                controls = frm.Controls.Find(pForm.FmOLDGB == 1 ? "it10" : "I0000008708", true);
                if (controls.Length > 0)
                {
                    controls[0].Text = dt.Rows[0]["PTMIVOXS"].ToString().Trim();
                }
                #endregion
            }
            dt.Dispose();
        }

        /// <summary>
        /// PRE-OP CHECKLIST(수술) 기록지에 PRE-OP CHECKLIST(병동)의 데이터를 가져와서 뿌린다.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue1808(PsmhDb pDbCon, Control control, string EmrNo, string EmrNoHis, EmrForm emrForm)
        {
            if (VB.Val(EmrNo) == 0)
                return;

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL.AppendLine(" SELECT ");

            if (emrForm.FmOLDGB == 1)
            {
                #region 1칸짜리 텍스트
                SQL.AppendLine("extractValue(chartxml, '//it1')   it1   ,");
                SQL.AppendLine("extractValue(chartxml, '//it10')  it10  ,");
                SQL.AppendLine("extractValue(chartxml, '//it11')  it11  ,");
                SQL.AppendLine("extractValue(chartxml, '//it17')  it17  ,");
                SQL.AppendLine("extractValue(chartxml, '//it18')  it18  ,");
                SQL.AppendLine("extractValue(chartxml, '//it189') it189 ,");
                SQL.AppendLine("extractValue(chartxml, '//it19')  it19  ,");
                SQL.AppendLine("extractValue(chartxml, '//it190') it190 ,");
                SQL.AppendLine("extractValue(chartxml, '//it191') it191 ,");
                SQL.AppendLine("extractValue(chartxml, '//it192') it192 ,");
                SQL.AppendLine("extractValue(chartxml, '//it193') it193 ,");
                SQL.AppendLine("extractValue(chartxml, '//it194') it194 ,");
                SQL.AppendLine("extractValue(chartxml, '//it195') it195 ,");
                //SQL.AppendLine("extractValue(chartxml, '//it196') it196 ,");
                SQL.AppendLine("extractValue(chartxml, '//it2') it2     ,");
                SQL.AppendLine("extractValue(chartxml, '//it20')it20    ,");
                SQL.AppendLine("extractValue(chartxml, '//it21')it21    ,");
                SQL.AppendLine("extractValue(chartxml, '//it22')it22    ,");
                SQL.AppendLine("extractValue(chartxml, '//it23')it23    ,");
                SQL.AppendLine("extractValue(chartxml, '//it24')it24    ,");
                SQL.AppendLine("extractValue(chartxml, '//it25')it25    ,");
                //SQL.AppendLine("extractValue(chartxml, '//it26')it26    ,");
                SQL.AppendLine("extractValue(chartxml, '//it27')it27    ,");
                SQL.AppendLine("extractValue(chartxml, '//it29')it29    ,");
                SQL.AppendLine("extractValue(chartxml, '//it3') it3     ,");
                SQL.AppendLine("extractValue(chartxml, '//it30')it30    ,");
                SQL.AppendLine("extractValue(chartxml, '//it31')it31    ,");
                SQL.AppendLine("extractValue(chartxml, '//it4') it4     ,");
                SQL.AppendLine("extractValue(chartxml, '//it5') it5     ,");
                SQL.AppendLine("extractValue(chartxml, '//it7') it7     ,");
                SQL.AppendLine("extractValue(chartxml, '//it8') it8     ,");
                SQL.AppendLine("extractValue(chartxml, '//it9') it9     ,");
                #endregion

                #region Comment
                SQL.AppendLine("extractValue(chartxml, '//ta1') ta1,");
                #endregion

                #region Radio
                SQL.AppendLine("extractValue(chartxml, '//ir1') ir1  ,");
                SQL.AppendLine("extractValue(chartxml, '//ir10') ir10 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir100') ir100,");
                SQL.AppendLine("extractValue(chartxml, '//ir101') ir101,");
                SQL.AppendLine("extractValue(chartxml, '//ir12') ir12 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir13') ir13 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir14') ir14 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir15') ir15 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir16') ir16 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir17') ir17 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir18') ir18 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir19') ir19 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir2') ir2  ,");
                SQL.AppendLine("extractValue(chartxml, '//ir20') ir20 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir21') ir21 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir22') ir22 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir23') ir23 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir24') ir24 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir25') ir25 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir26') ir26 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir27') ir27 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir28') ir28 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir29') ir29 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir3') ir3  ,");
                SQL.AppendLine("extractValue(chartxml, '//ir30') ir30 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir31') ir31 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir32') ir32 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir33') ir33 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir34') ir34 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir35') ir35 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir36') ir36 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir37') ir37 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir38') ir38 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir39') ir39 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir4') ir4  ,");
                SQL.AppendLine("extractValue(chartxml, '//ir40') ir40 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir41') ir41 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir42') ir42 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir43') ir43 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir44') ir44 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir45') ir45 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir46') ir46 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir47') ir47 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir48') ir48 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir49') ir49 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir5') ir5  ,");
                SQL.AppendLine("extractValue(chartxml, '//ir50') ir50 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir51') ir51 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir52') ir52 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir53') ir53 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir54') ir54 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir55') ir55 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir56') ir56 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir58') ir58 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir59') ir59 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir6') ir6  ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir60') ir60 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir61') ir61 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir62') ir62 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir63') ir63 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir64') ir64 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir65') ir65 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir66') ir66 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir67') ir67 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir68') ir68 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir69') ir69 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir7') ir7  ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir70') ir70 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir71') ir71 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir72') ir72 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir73') ir73 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir74') ir74 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir75') ir75 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir76') ir76 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir77') ir77 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir78') ir78 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir79') ir79 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir8') ir8  ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir80') ir80 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir81') ir81 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir82') ir82 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir83') ir83 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir84') ir84 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir85') ir85 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir86') ir86 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir87') ir87 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir88') ir88 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir89') ir89 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir9') ir9  ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir90') ir90 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir91') ir91 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir92') ir92 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir93') ir93 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir94') ir94 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir95') ir95 ,");
                //SQL.AppendLine("extractValue(chartxml, '//ir96') ir96 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir97') ir97 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir98') ir98 ,");
                SQL.AppendLine("extractValue(chartxml, '//ir99') ir99 ,");
                #endregion

                #region Check
                SQL.AppendLine("extractValue(chartxml, '//ik10') ik10,");
                SQL.AppendLine("extractValue(chartxml, '//ik11') ik11,");
                SQL.AppendLine("extractValue(chartxml, '//ik13') ik13,");
                SQL.AppendLine("extractValue(chartxml, '//ik16') ik16,");
                SQL.AppendLine("extractValue(chartxml, '//ik20') ik20,");
                SQL.AppendLine("extractValue(chartxml, '//ik21') ik21,");
                SQL.AppendLine("extractValue(chartxml, '//ik22') ik22,");
                SQL.AppendLine("extractValue(chartxml, '//ik23') ik23,");
                SQL.AppendLine("extractValue(chartxml, '//ik24') ik24,");
                SQL.AppendLine("extractValue(chartxml, '//ik25') ik25,");
                SQL.AppendLine("extractValue(chartxml, '//ik26') ik26,");
                SQL.AppendLine("extractValue(chartxml, '//ik27') ik27,");
                SQL.AppendLine("extractValue(chartxml, '//ik28') ik28,");
                SQL.AppendLine("extractValue(chartxml, '//ik29') ik29,");
                SQL.AppendLine("extractValue(chartxml, '//ik30') ik30,");
                SQL.AppendLine("extractValue(chartxml, '//ik31') ik31,");
                SQL.AppendLine("extractValue(chartxml, '//ik32') ik32,");
                SQL.AppendLine("extractValue(chartxml, '//ik33') ik33,");
                SQL.AppendLine("extractValue(chartxml, '//ik34') ik34,");
                SQL.AppendLine("extractValue(chartxml, '//ik35') ik35,");
                SQL.AppendLine("extractValue(chartxml, '//ik36') ik36,");
                SQL.AppendLine("extractValue(chartxml, '//ik37') ik37,");
                SQL.AppendLine("extractValue(chartxml, '//ik38') ik38,");
                SQL.AppendLine("extractValue(chartxml, '//ik7')  ik7 ,");
                SQL.AppendLine("extractValue(chartxml, '//ik8')  ik8 ,");
                SQL.AppendLine("extractValue(chartxml, '//ik9')  ik9");
                #endregion

                SQL.AppendLine(" FROM KOSMOS_EMR.EMRXML ");
                SQL.AppendLine("WHERE EMRNO = " + EmrNo);

                SQL.AppendLine("  ORDER BY (TRIM(CHARTDATE) || TRIM(CHARTTIME)) DESC ");

            }
            else
            {
                #region 쿼리
                SQL.AppendLine(" ITEMCD, ITEMVALUE ");
                SQL.AppendLine(" FROM KOSMOS_EMR.AEMRCHARTROW A");
                SQL.AppendLine("WHERE A.EMRNO = " + EmrNo);
                SQL.AppendLine("  AND A.EMRNOHIS = " + EmrNoHis);
                SQL.AppendLine("  AND ITEMNO > CHR(0)");

                #region 수술/시술실 라디오 제외
                SQL.AppendLine("  AND ITEMCD NOT IN");
                SQL.AppendLine("  (");
                SQL.AppendLine("    'I0000034611',");
                SQL.AppendLine("    'I0000034612',");
                SQL.AppendLine("    'I0000034613',");
                SQL.AppendLine("    'I0000034618',");
                SQL.AppendLine("    'I0000034619',");
                SQL.AppendLine("    'I0000034620',");
                SQL.AppendLine("    'I0000034624',");
                SQL.AppendLine("    'I0000034625',");
                SQL.AppendLine("    'I0000034626',");
                SQL.AppendLine("    'I0000034631',");
                SQL.AppendLine("    'I0000034632',");
                SQL.AppendLine("    'I0000034633',");
                SQL.AppendLine("    'I0000034637',");
                SQL.AppendLine("    'I0000034638',");
                SQL.AppendLine("    'I0000034639',");
                SQL.AppendLine("    'I0000034643',");
                SQL.AppendLine("    'I0000034644',");
                SQL.AppendLine("    'I0000034645',");
                SQL.AppendLine("    'I0000034649',");
                SQL.AppendLine("    'I0000034650',");
                SQL.AppendLine("    'I0000034651',");
                SQL.AppendLine("    'I0000034660',");
                SQL.AppendLine("    'I0000034661',");
                SQL.AppendLine("    'I0000034662',");
                SQL.AppendLine("    'I0000034668',");
                SQL.AppendLine("    'I0000034669',");
                SQL.AppendLine("    'I0000034670',");
                SQL.AppendLine("    'I0000034678',");
                SQL.AppendLine("    'I0000034679',");
                SQL.AppendLine("    'I0000034680',");
                SQL.AppendLine("    'I0000034686',");
                SQL.AppendLine("    'I0000034687',");
                SQL.AppendLine("    'I0000034688',");
                SQL.AppendLine("    'I0000034692',");
                SQL.AppendLine("    'I0000034693',");
                SQL.AppendLine("    'I0000034694' ");
                SQL.AppendLine("  )");
                #endregion

                #endregion
            }
            //SQL.AppendLine( " WHERE PTNO = '" + emrPatient.ptNo + "'");
            //SQL.AppendLine( "   AND MEDFRDATE = '" + emrPatient.medFrDate + "' ");
            //SQL.AppendLine( "   AND FORMNO = 2465");


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            Control[] controls;

            if (dt.Rows.Count > 0)
            {
                if (emrForm.FmOLDGB == 1)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        controls = control.Controls.Find(dt.Columns[i].ColumnName.ToLower(), true);
                        if (controls.Length == 0)
                            continue;

                        if (controls[0] is RadioButton)
                        {
                            (controls[0] as RadioButton).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("TRUE");
                        }

                        if (controls[0] is CheckBox)
                        {
                            (controls[0] as CheckBox).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("TRUE");
                        }

                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        controls = control.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true);
                        if (controls.Length == 0)
                            continue;

                        if (controls[0] is RadioButton)
                        {
                            (controls[0] as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                        }

                        if (controls[0] is CheckBox)
                        {
                            (controls[0] as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                        }

                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                    }
                }
              
            }
            dt.Dispose();
        }

        /// <summary>
        /// 시술/검사전 기록지(Receive) 기록지에 시술/검사전 기록지(send)의 데이터를 가져와서 뿌린다.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue2467(PsmhDb pDbCon, Control control, string EmrNo, string EmrNoHis, EmrForm emrForm)
        {
            if (VB.Val(EmrNo) == 0)
                return;

            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL.AppendLine(" SELECT ");

            if (emrForm.FmOLDGB == 1)
            {
                #region 1칸짜리 텍스트
                SQL.AppendLine("extractValue(chartxml, '//it1') it1,");
                SQL.AppendLine("extractValue(chartxml, '//it194') it194,");
                SQL.AppendLine("extractValue(chartxml, '//it196') it196,");
                SQL.AppendLine("extractValue(chartxml, '//it2') it2,");
                SQL.AppendLine("extractValue(chartxml, '//it9') it9,");
                SQL.AppendLine("extractValue(chartxml, '//it10') it10,");
                SQL.AppendLine("extractValue(chartxml, '//it7') it7,");
                SQL.AppendLine("extractValue(chartxml, '//it195') it195,");
                SQL.AppendLine("extractValue(chartxml, '//it8') it8,");
                SQL.AppendLine("extractValue(chartxml, '//it11') it11,");
                SQL.AppendLine("extractValue(chartxml, '//it190') it190,");
                SQL.AppendLine("extractValue(chartxml, '//it192') it192,");
                SQL.AppendLine("extractValue(chartxml, '//it191') it191,");
                SQL.AppendLine("extractValue(chartxml, '//it19') it19,");
                SQL.AppendLine("extractValue(chartxml, '//it18') it18,");
                SQL.AppendLine("extractValue(chartxml, '//it20') it20,");
                SQL.AppendLine("extractValue(chartxml, '//it17') it17,");
                SQL.AppendLine("extractValue(chartxml, '//it21') it21,");
                SQL.AppendLine("extractValue(chartxml, '//it22') it22,");
                SQL.AppendLine("extractValue(chartxml, '//it29') it29,");
                SQL.AppendLine("extractValue(chartxml, '//it30') it30,");
                SQL.AppendLine("extractValue(chartxml, '//it189') it189,");
                SQL.AppendLine("extractValue(chartxml, '//it31') it31,");
                SQL.AppendLine("extractValue(chartxml, '//it25') it25,");
                SQL.AppendLine("extractValue(chartxml, '//it27') it27,");
                SQL.AppendLine("extractValue(chartxml, '//it197') it197,");
                #endregion

                #region Comment
                SQL.AppendLine("extractValue(chartxml, '//ta1') ta1,");
                #endregion

                #region 병동 라디오 예, 아니오, 미해당 
                SQL.AppendLine("extractValue(chartxml, '//ir12') ir12,");
                SQL.AppendLine("extractValue(chartxml, '//ir13') ir13,");
                SQL.AppendLine("extractValue(chartxml, '//ir14') ir14,");

                SQL.AppendLine("extractValue(chartxml, '//ir15') ir15,");
                SQL.AppendLine("extractValue(chartxml, '//ir16') ir16,");
                SQL.AppendLine("extractValue(chartxml, '//ir17') ir17,");

                SQL.AppendLine("extractValue(chartxml, '//ir24') ir24,");
                SQL.AppendLine("extractValue(chartxml, '//ir19') ir19,");
                SQL.AppendLine("extractValue(chartxml, '//ir20') ir20,");

                SQL.AppendLine("extractValue(chartxml, '//ir21') ir21,");
                SQL.AppendLine("extractValue(chartxml, '//ir22') ir22,");
                SQL.AppendLine("extractValue(chartxml, '//ir23') ir23,");

                SQL.AppendLine("extractValue(chartxml, '//ir18') ir18,");
                SQL.AppendLine("extractValue(chartxml, '//ir25') ir25,");
                SQL.AppendLine("extractValue(chartxml, '//ir26') ir26,");

                SQL.AppendLine("extractValue(chartxml, '//ir27') ir27,");
                SQL.AppendLine("extractValue(chartxml, '//ir28') ir28,");
                SQL.AppendLine("extractValue(chartxml, '//ir29') ir29,");

                SQL.AppendLine("extractValue(chartxml, '//ir30') ir30,");
                SQL.AppendLine("extractValue(chartxml, '//ir31') ir31,");
                SQL.AppendLine("extractValue(chartxml, '//ir32') ir32,");

                /////////////////////////////////////////////////////

                SQL.AppendLine("extractValue(chartxml, '//ir33') ir33,");
                SQL.AppendLine("extractValue(chartxml, '//ir34') ir34,");
                SQL.AppendLine("extractValue(chartxml, '//ir35') ir35,");

                SQL.AppendLine("extractValue(chartxml, '//ir36') ir36,");
                SQL.AppendLine("extractValue(chartxml, '//ir37') ir37,");
                SQL.AppendLine("extractValue(chartxml, '//ir38') ir38,");

                SQL.AppendLine("extractValue(chartxml, '//ir39') ir39,");
                SQL.AppendLine("extractValue(chartxml, '//ir40') ir40,");
                SQL.AppendLine("extractValue(chartxml, '//ir41') ir41,");

                SQL.AppendLine("extractValue(chartxml, '//ir42') ir42,");
                SQL.AppendLine("extractValue(chartxml, '//ir43') ir43,");
                SQL.AppendLine("extractValue(chartxml, '//ir44') ir44,");

                SQL.AppendLine("extractValue(chartxml, '//ir45') ir45,");
                SQL.AppendLine("extractValue(chartxml, '//ir46') ir46,");
                SQL.AppendLine("extractValue(chartxml, '//ir47') ir47,");

                SQL.AppendLine("extractValue(chartxml, '//ir48') ir48,");
                SQL.AppendLine("extractValue(chartxml, '//ir49') ir49,");
                SQL.AppendLine("extractValue(chartxml, '//ir50') ir50,");

                SQL.AppendLine("extractValue(chartxml, '//ir109') ir109,");
                SQL.AppendLine("extractValue(chartxml, '//ir110') ir110,");
                SQL.AppendLine("extractValue(chartxml, '//ir111') ir111,");
                #endregion

                #region Radio
                SQL.AppendLine("extractValue(chartxml, '//ir1') ir1,");
                SQL.AppendLine("extractValue(chartxml, '//ir2') ir2,");
                SQL.AppendLine("extractValue(chartxml, '//ir99') ir99,");
                SQL.AppendLine("extractValue(chartxml, '//ir3') ir3,");
                SQL.AppendLine("extractValue(chartxml, '//ir120') ir120,");
                SQL.AppendLine("extractValue(chartxml, '//ir8') ir8,");
                SQL.AppendLine("extractValue(chartxml, '//ir9') ir9,");
                SQL.AppendLine("extractValue(chartxml, '//ir10') ir10,");
                SQL.AppendLine("extractValue(chartxml, '//ir119') ir119,");
                SQL.AppendLine("extractValue(chartxml, '//ir101') ir101,");
                SQL.AppendLine("extractValue(chartxml, '//ir102') ir102,");
                SQL.AppendLine("extractValue(chartxml, '//ir103') ir103,");
                SQL.AppendLine("extractValue(chartxml, '//ir105') ir105,");
                SQL.AppendLine("extractValue(chartxml, '//ir106') ir106,");
                SQL.AppendLine("extractValue(chartxml, '//ir115') ir115,");
                SQL.AppendLine("extractValue(chartxml, '//ir4') ir4,");
                SQL.AppendLine("extractValue(chartxml, '//ir5') ir5,");
                SQL.AppendLine("extractValue(chartxml, '//ir104') ir104,");
                SQL.AppendLine("extractValue(chartxml, '//ir107') ir107,");
                SQL.AppendLine("extractValue(chartxml, '//ir108') ir108,");
                SQL.AppendLine("extractValue(chartxml, '//ir116') ir116,");
                SQL.AppendLine("extractValue(chartxml, '//ir117') ir117,");
                SQL.AppendLine("extractValue(chartxml, '//ir118') ir118,");

                SQL.AppendLine("extractValue(chartxml, '//ir51') ir51,");
                SQL.AppendLine("extractValue(chartxml, '//ir52') ir52,");
                SQL.AppendLine("extractValue(chartxml, '//ir53') ir53,");
                SQL.AppendLine("extractValue(chartxml, '//ir54') ir54,");
                SQL.AppendLine("extractValue(chartxml, '//ir55') ir55,");
                SQL.AppendLine("extractValue(chartxml, '//ir56') ir56,");
                SQL.AppendLine("extractValue(chartxml, '//ir6') ir6,");
                SQL.AppendLine("extractValue(chartxml, '//ir7') ir7,");
                #endregion

                #region Check
                SQL.AppendLine("extractValue(chartxml, '//ik7') ik7,");
                SQL.AppendLine("extractValue(chartxml, '//ik8') ik8,");
                SQL.AppendLine("extractValue(chartxml, '//ik39') ik39,");
                SQL.AppendLine("extractValue(chartxml, '//ik9') ik9,");
                SQL.AppendLine("extractValue(chartxml, '//ik10') ik10,");
                SQL.AppendLine("extractValue(chartxml, '//ik11') ik11,");
                SQL.AppendLine("extractValue(chartxml, '//ik40') ik40,");
                SQL.AppendLine("extractValue(chartxml, '//ik41') ik41,");
                SQL.AppendLine("extractValue(chartxml, '//ik48') ik48,");
                SQL.AppendLine("extractValue(chartxml, '//ik44') ik44,");
                SQL.AppendLine("extractValue(chartxml, '//ik24') ik24,");
                SQL.AppendLine("extractValue(chartxml, '//ik25') ik25,");
                SQL.AppendLine("extractValue(chartxml, '//ik26') ik26,");
                SQL.AppendLine("extractValue(chartxml, '//ik27') ik27,");
                SQL.AppendLine("extractValue(chartxml, '//ik28') ik28,");
                SQL.AppendLine("extractValue(chartxml, '//ik29') ik29,");
                SQL.AppendLine("extractValue(chartxml, '//ik30') ik30,");
                SQL.AppendLine("extractValue(chartxml, '//ik35') ik35,");
                SQL.AppendLine("extractValue(chartxml, '//ik36') ik36,");
                SQL.AppendLine("extractValue(chartxml, '//ik37') ik37,");
                SQL.AppendLine("extractValue(chartxml, '//ik38') ik38,");
                SQL.AppendLine("extractValue(chartxml, '//ik21') ik21,");
                SQL.AppendLine("extractValue(chartxml, '//ik22') ik22,");
                SQL.AppendLine("extractValue(chartxml, '//ik23') ik23,");
                SQL.AppendLine("extractValue(chartxml, '//ik42') ik42,");
                SQL.AppendLine("extractValue(chartxml, '//ik43') ik43,");
                SQL.AppendLine("extractValue(chartxml, '//ik45') ik45,");
                SQL.AppendLine("extractValue(chartxml, '//ik46') ik46,");
                SQL.AppendLine("extractValue(chartxml, '//ik47') ik47");
                #endregion

                SQL.AppendLine( " FROM KOSMOS_EMR.EMRXML ");
                SQL.AppendLine("WHERE EMRNO = " + EmrNo);
                SQL.AppendLine("  ORDER BY (TRIM(CHARTDATE) || TRIM(CHARTTIME)) DESC ");
            }
            else
            {
                #region 쿼리
                SQL.AppendLine(" ITEMCD, ITEMVALUE ");
                SQL.AppendLine(" FROM KOSMOS_EMR.AEMRCHARTROW ");
                SQL.AppendLine("WHERE EMRNO = " + EmrNo);
                SQL.AppendLine("  AND EMRNOHIS = " + EmrNoHis);
                
                if (emrForm.FmFORMNO == 2636)
                {
                    SQL.AppendLine("  AND ITEMNO IN ('I0000014147', 'I0000031507')");
                }
                else
                {
                    SQL.AppendLine("  AND ITEMNO > CHR(0)");
                }

                #region 수술/시술실 라디오 제외
                SQL.AppendLine("  AND ITEMCD NOT IN");
                SQL.AppendLine("  (");
                SQL.AppendLine("    'I0000034611',");
                SQL.AppendLine("    'I0000034612',");
                SQL.AppendLine("    'I0000034613',");
                SQL.AppendLine("    'I0000034618',");
                SQL.AppendLine("    'I0000034619',");
                SQL.AppendLine("    'I0000034620',");
                SQL.AppendLine("    'I0000034624',");
                SQL.AppendLine("    'I0000034625',");
                SQL.AppendLine("    'I0000034626',");
                SQL.AppendLine("    'I0000034631',");
                SQL.AppendLine("    'I0000034632',");
                SQL.AppendLine("    'I0000034633',");
                SQL.AppendLine("    'I0000034637',");
                SQL.AppendLine("    'I0000034638',");
                SQL.AppendLine("    'I0000034639',");
                SQL.AppendLine("    'I0000034643',");
                SQL.AppendLine("    'I0000034644',");
                SQL.AppendLine("    'I0000034645',");
                SQL.AppendLine("    'I0000034649',");
                SQL.AppendLine("    'I0000034650',");
                SQL.AppendLine("    'I0000034651',");
                SQL.AppendLine("    'I0000034660',");
                SQL.AppendLine("    'I0000034661',");
                SQL.AppendLine("    'I0000034662',");
                SQL.AppendLine("    'I0000034668',");
                SQL.AppendLine("    'I0000034669',");
                SQL.AppendLine("    'I0000034670',");
                SQL.AppendLine("    'I0000034678',");
                SQL.AppendLine("    'I0000034679',");
                SQL.AppendLine("    'I0000034680',");
                SQL.AppendLine("    'I0000034686',");
                SQL.AppendLine("    'I0000034687',");
                SQL.AppendLine("    'I0000034688',");
                SQL.AppendLine("    'I0000034692',");
                SQL.AppendLine("    'I0000034693',");
                SQL.AppendLine("    'I0000034694',");
                SQL.AppendLine("    'I0000034719',");
                SQL.AppendLine("    'I0000034720',");
                SQL.AppendLine("    'I0000034721',");
                SQL.AppendLine("    'I0000034730',");
                SQL.AppendLine("    'I0000034731',");
                SQL.AppendLine("    'I0000034732' ");
                SQL.AppendLine("  )");
                #endregion

                #endregion
            }

            //SQL.AppendLine( " WHERE PTNO = '" + emrPatient.ptNo + "'");
            //SQL.AppendLine( "   AND MEDFRDATE = '" + emrPatient.medFrDate + "' ");
            //SQL.AppendLine( "   AND FORMNO = 2465");

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            Control[] controls;

            if (dt.Rows.Count > 0)
            {
                if (emrForm.FmOLDGB == 1)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        controls = control.Controls.Find(dt.Columns[i].ColumnName.ToLower(), true);
                        if (controls.Length == 0)
                            continue;

                        if (controls[0] is RadioButton)
                        {
                            (controls[0] as RadioButton).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("TRUE");
                        }

                        if (controls[0] is CheckBox)
                        {
                            (controls[0] as CheckBox).Checked = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim().ToUpper().Equals("TRUE");
                        }

                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Trim();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string strItemCd = dt.Rows[i]["ITEMCD"].ToString().Trim();
                        if (emrForm.FmFORMNO == 2636 && strItemCd.Equals("I0000031507"))
                        {
                            strItemCd = "I0000031507_1";
                        }

                        controls = control.Controls.Find(strItemCd, true);
                        if (controls.Length == 0)
                            continue;

                        if (controls[0] is RadioButton)
                        {
                            (controls[0] as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                        }

                        if (controls[0] is CheckBox)
                        {
                            (controls[0] as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                        }

                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                    }
                }
            }
            dt.Dispose();
        }


        /// <summary>
        ///  신생아출산(분만내역읽기)
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue6(PsmhDb pDbCon, Form frm, string strPano, string strChartDate, EmrForm pForm)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT ";

            if (pForm.FmOLDGB == 1)
            {
                #region 이전 서식지

                #region it
                for (int i = 1; i < 51; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//it" + i + "') IT" + i.ToString("00") + ", ";
                }
                #endregion

                #region ir
                for (int i = 1; i < 15; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//ir" + i + "') IR" + i.ToString("00") + ", ";
                }
                #endregion

                #region ik
                for (int i = 1; i < 18; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//ik" + i + "') IK" + i.ToString("00") + ", ";
                }
                #endregion

                #region ta
                SQL += ComNum.VBLF + "extractValue(chartxml, '//ta1') TA01, ";
                #endregion

                #region dt
                SQL += ComNum.VBLF + "extractValue(chartxml, '//dt1') dt01, ";
                SQL += ComNum.VBLF + "extractValue(chartxml, '//dt2') dt02, ";
                #endregion

                SQL += ComNum.VBLF + " '' TEMP";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + " WHERE CHARTDATE = '" + strChartDate + "' ";
                SQL += ComNum.VBLF + "     AND FORMNO = 2306";
                SQL += ComNum.VBLF + "     AND PTNO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "  ORDER BY CHARTDATE DESC, CHARTTIME DESC ";
                #endregion
            }
            else
            {
                #region 신규 서식지
                SQL += ComNum.VBLF + " B.ITEMCD, B.ITEMVALUE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A ";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
                SQL += ComNum.VBLF + "      ON A.EMRNO = B.EMRNO";
                SQL += ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
                SQL += ComNum.VBLF + "     AND B.ITEMCD > CHR(0)";
                SQL += ComNum.VBLF + " WHERE CHARTDATE = '" + strChartDate + "' ";
                SQL += ComNum.VBLF + "   AND FORMNO = 2306";
                SQL += ComNum.VBLF + "   AND PTNO = '" + strPano + "' ";
                SQL += ComNum.VBLF + "  ORDER BY CHARTDATE DESC, CHARTTIME DESC ";
                #endregion
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            Control[] controls;

            if (dt.Rows.Count > 0)
            {
                if (pForm.FmOLDGB == 1)
                {
                    #region 이전 서식지
                    #region it
                    for (int i = 1; i < 51; i++)
                    {
                        switch (i)
                        {
                            case 22:
                            case 24:
                            case 30:
                            case 31:
                            case 36:
                                break;
                            default:
                                controls = frm.Controls.Find("it" + i, true);
                                if (controls.Length > 0 && controls[0].Text.Length == 0)
                                {
                                    controls[0].Text = dt.Rows[0]["it" + i.ToString("00")].ToString().Trim();
                                }
                                break;
                        }

                    }
                    #endregion

                    #region ir
                    for (int i = 1; i < 15; i++)
                    {
                        controls = frm.Controls.Find("ir" + i, true);
                        if (controls.Length > 0)
                        {
                            if (controls[0] is RadioButton)
                            {
                                (controls[0] as RadioButton).Checked = dt.Rows[0]["ir" + i.ToString("00")].ToString().Trim().ToUpper().Equals("TRUE");
                            }
                        }
                    }
                    #endregion

                    #region ik
                    for (int i = 1; i < 18; i++)
                    {
                        controls = frm.Controls.Find("ik" + i, true);
                        if (controls.Length > 0)
                        {
                            if (controls[0] is CheckBox)
                            {
                                (controls[0] as CheckBox).Checked = dt.Rows[0]["ik" + i.ToString("00")].ToString().Trim().ToUpper().Equals("TRUE");
                            }
                        }
                    }
                    #endregion

                    #region ta
                    controls = frm.Controls.Find("ta1", true);
                    if (controls.Length > 0 && controls[0].Text.Length == 0)
                    {
                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[0]["ta01"].ToString().Trim();
                        }
                    }
                    #endregion

                    #region dt
                    controls = frm.Controls.Find("dt1", true);
                    if (controls.Length > 0 && controls[0].Text.Length == 0)
                    {
                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[0]["dt01"].ToString().Trim();
                        }
                    }

                    controls = frm.Controls.Find("dt2", true);
                    if (controls.Length > 0 && controls[0].Text.Length == 0)
                    {
                        if (controls[0] is TextBox)
                        {
                            controls[0].Text = dt.Rows[0]["dt02"].ToString().Trim();
                        }
                    }
                    #endregion
                    #endregion
                }
                else
                {
                    #region 신규 서식지
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Control control = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true).FirstOrDefault();
                        if (control != null)
                        {
                            if (control is RadioButton)
                            {
                                (control as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                            }
                            else if (control is CheckBox)
                            {
                                (control as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                            }
                            else if (control is TextBox)
                            {
                                control.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            }
                        }
                    }
                    #endregion
                }

            }
            dt.Dispose();
        }


        /// <summary>
        /// 전동기록지
        /// </summary>
        /// <param name="frm"></param>
        public static void setChartFormValue3(PsmhDb pDbCon, Form frm, EmrPatient AcpEmr, string strChartDate, EmrForm pForm, string strEmrNo = "")
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT ";

            if (pForm.FmOLDGB == 1)
            {
                #region XML
                #region it
                for (int i = 1; i < 25; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//it" + i + "') IT" + i.ToString("00") + ", ";
                }
                #endregion

                #region ir
                for (int i = 1; i < 27; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//ir" + i + "') IR" + i.ToString("00") + ", ";
                }
                #endregion

                #region ik
                for (int i = 1; i < 28; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//ik" + i + "') IK" + i.ToString("00") + ", ";
                }
                #endregion

                #region ta
                for (int i = 1; i < 6; i++)
                {
                    if (i > 0 && (i % 5) == 0)
                    {
                        SQL += ComNum.VBLF;
                    }

                    SQL = SQL + "extractValue(chartxml, '//ta" + i + "') TA" + i.ToString("00") + ", ";
                }
                #endregion

                SQL += ComNum.VBLF + " '' TEMP";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML ";
                SQL += ComNum.VBLF + " WHERE CHARTDATE = '" + strChartDate + "' ";
                SQL += ComNum.VBLF + "     AND FORMNO = 2279";
                SQL += ComNum.VBLF + "     AND PTNO = '" + AcpEmr.ptNo + "' ";
                SQL += ComNum.VBLF + "  ORDER BY CHARTDATE DESC, CHARTTIME DESC ";
                #endregion
            }
            else
            {
                if (string.IsNullOrWhiteSpace(strEmrNo))
                {
                    ComFunc.MsgBoxEx(frm, "정상적인 접근이 아닙니다.");
                    return;
                }

                SQL += ComNum.VBLF + "A.CHARTDATE, A.CHARTTIME, U.NAME, B.ITEMCD, B.ITEMVALUE";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
                SQL += ComNum.VBLF + "     ON A.EMRNO    = B.EMRNO";
                SQL += ComNum.VBLF + "    AND A.EMRNOHIS = B.EMRNOHIS";
                SQL += ComNum.VBLF + "    AND B.ITEMCD > CHR(0)";
                SQL += ComNum.VBLF + "    AND B.ITEMCD NOT IN ('I0000033507', 'I0000033510')";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_USERT U";
                SQL += ComNum.VBLF + "     ON U.USERID = A.CHARTUSEID";
                SQL += ComNum.VBLF + "WHERE A.EMRNO = " + strEmrNo;
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            Control[] controls;

            if (dt.Rows.Count > 0)
            {
                if (pForm.FmOLDGB == 1)
                {
                    #region XML
                    #region it
                    for (int i = 1; i < 25; i++)
                    {
                        switch(i)
                        {
                            case 15:
                            case 17:
                            case 21:
                            case 22:
                            case 24:
                                break;
                            default:
                                controls = frm.Controls.Find("it" + i, true);
                                if (controls.Length > 0 && controls[0].Text.Length == 0)
                                {
                                    controls[0].Text = dt.Rows[0]["it" + i.ToString("00")].ToString().Trim();
                                }
                                break;
                        }
                    
                    }
                    #endregion

                    #region ir
                    for (int i = 1; i < 27; i++)
                    {
                        switch (i)
                        {
                            case 10:
                                break;
                            default:
                                controls = frm.Controls.Find("ir" + i, true);
                                if (controls.Length > 0 )
                                {
                                    if(controls[0] is RadioButton)
                                    {
                                        (controls[0] as RadioButton).Checked = dt.Rows[0]["ir" + i.ToString("00")].ToString().Trim().ToUpper().Equals("TRUE");
                                    }
                                
                                }
                                break;
                        }

                    }
                    #endregion

                    #region ik
                    for (int i = 1; i < 28; i++)
                    {
                        switch (i)
                        {
                            case 1:
                            case 24:
                                break;
                            default:
                                controls = frm.Controls.Find("ik" + i, true);
                                if (controls.Length > 0)
                                {
                                    if (controls[0] is CheckBox)
                                    {
                                        (controls[0] as CheckBox).Checked = dt.Rows[0]["ik" + i.ToString("00")].ToString().Trim().ToUpper().Equals("TRUE");
                                    }

                                }
                                break;
                        }

                    }
                    #endregion

                    #region ta
                    for (int i = 1; i < 6; i++)
                    {
                        controls = frm.Controls.Find("ta" + i, true);
                        if (controls.Length > 0 && controls[0].Text.Length == 0)
                        {
                            if (controls[0] is TextBox)
                            {
                                controls[0].Text = dt.Rows[0]["ta" + i.ToString("00")].ToString().Trim();
                            }
                        }

                    }
                    #endregion
                    #endregion
                }
                else
                {
                    ComFunc.MsgBoxEx(frm, string.Format("Send지 작성시간: {0}\r\n작성자: {1}",
                        (dt.Rows[0]["CHARTDATE"].ToString().Trim()).To<int>().ToString("0000-00-00") + " " +  dt.Rows[0]["CHARTTIME"].ToString().Trim().Substring(0, 4).To<int>().ToString("00:00"), 
                        dt.Rows[0]["NAME"].ToString().Trim()));

                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        controls = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true);
                        if (controls.Length > 0)
                        {
                            if (controls[0] is TextBox)
                            {
                                controls[0].Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            }
                            else if (controls[0] is RadioButton)
                            {
                                (controls[0] as RadioButton).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                            }
                            else if (controls[0] is CheckBox)
                            {
                                (controls[0] as CheckBox).Checked = dt.Rows[i]["ITEMVALUE"].ToString().Trim().Equals("1");
                            }
                        }
                    }
                }
            }
            dt.Dispose();
        }

        /// <summary>
        /// 퇴실내역읽기
        /// ER 퇴실기록지
        /// 폼번호: 
        /// "2277"
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void setChartFormValue4(PsmhDb pDbCon, Form frm, EmrPatient AcpEmr)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;

            SQL = " SELECT PTMIIDNO, PTMIINDT, PTMIINTM, PTMIOTDT, PTMIOTTM, PTMIEMRT, THCD";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + " WHERE PTMIIDNO = '" + AcpEmr.ptNo + "' ";
            SQL += ComNum.VBLF + "   AND PTMIINDT = '" + AcpEmr.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND SEQNO =";
            SQL += ComNum.VBLF + " (SELECT MAX(SEQNO)";
            SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + "   WHERE PTMIIDNO = '" + AcpEmr.ptNo + "'";
            SQL += ComNum.VBLF + "     AND PTMIINDT = '" + AcpEmr.medFrDate + "')";
            SQL += ComNum.VBLF + "   ORDER BY PTMIINTM DESC";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            Control[] controls;

            if (dt.Rows.Count > 0)
            {
                controls = frm.Controls.Find("dt1", true);

                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                      controls[0].Text = VB.Val(dt.Rows[0]["PTMIOTDT"].ToString().Trim()).ToString("0000-00-00");
                }

                controls = frm.Controls.Find("it2", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = VB.Val(dt.Rows[0]["PTMIOTTM"].ToString().Trim()).ToString("00:00");
                }

                #region ik1~5 초기화
                controls = frm.Controls.Find("ik1", true);
                if (controls.Length > 0)
                {
                    ((CheckBox)controls[0]).Checked = false;
                }

                controls = frm.Controls.Find("ik2", true);
                if (controls.Length > 0)
                {
                    ((CheckBox)controls[0]).Checked = false;
                }

                controls = frm.Controls.Find("ik3", true);
                if (controls.Length > 0)
                {
                    ((CheckBox)controls[0]).Checked = false;
                }

                controls = frm.Controls.Find("ik4", true);
                if (controls.Length > 0)
                {
                    ((CheckBox)controls[0]).Checked = false;
                }

                controls = frm.Controls.Find("ik5", true);
                if (controls.Length > 0)
                {
                    ((CheckBox)controls[0]).Checked = false;
                }
                #endregion

                if(dt.Rows[0]["PTMIEMRT"].ToString().Trim() == "14")
                {
                    controls = frm.Controls.Find("ik3", true);
                    if (controls.Length > 0)
                    {
                        ((CheckBox)controls[0]).Checked = true;
                    }
                }
                else
                {
                    switch(VB.Left(dt.Rows[0]["PTMIEMRT"].ToString().Trim(), 1))
                    {
                        case "1":
                            controls = frm.Controls.Find("ik2", true);
                            if (controls.Length > 0)
                            {
                                ((CheckBox)controls[0]).Checked = true;
                            }
                            break;
                        case "2":
                            controls = frm.Controls.Find("ik5", true);
                            if (controls.Length > 0)
                            {
                                ((CheckBox)controls[0]).Checked = true;
                            }
                            break;
                        case "3":
                            controls = frm.Controls.Find("ik1", true);
                            if (controls.Length > 0)
                            {
                                ((CheckBox)controls[0]).Checked = true;
                            }
                            break;
                        case "4":
                            controls = frm.Controls.Find("ik4", true);
                            if (controls.Length > 0)
                            {
                                ((CheckBox)controls[0]).Checked = true;
                            }
                            break;
                    }
                }

                SQL = " SELECT HOSPNAME";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_HOSPITAL";
                SQL += ComNum.VBLF + "  WHERE HOSPNUMB = '" + dt.Rows[0]["THCD"].ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if(dt2.Rows.Count > 0)
                {
                    controls = frm.Controls.Find("it3", true);
                    if (controls.Length > 0)
                    {
                        controls[0].Text = dt2.Rows[0]["HOSPNAME"].ToString().Trim();
                    }
                }
                dt2.Dispose();
            }

            dt.Dispose();

            string strInDate = VB.Left(AcpEmr.medFrDate, 8);
            strInDate = VB.Left(strInDate, 4) + "-" + VB.Mid(strInDate, 5, 2) + "-" + VB.Right(strInDate, 2);

            string strInTime = VB.Left(AcpEmr.medFrTime, 4);
            strInTime = VB.Left(strInTime, 2) + ":" + VB.Right(strInTime, 2);

            SQL = "SELECT  APACHE, ISS, NIHSS";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_PATIENT";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + AcpEmr.ptNo + "'";
            SQL += ComNum.VBLF + "    AND INTIME = TO_DATE('" + strInDate + " " + strInTime + "','YYYY-MM-DD HH24:MI') ";


            SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                controls = frm.Controls.Find("it4", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["APACHE"].ToString().Trim();
                }

                controls = frm.Controls.Find("it5", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["ISS"].ToString().Trim();
                }

                controls = frm.Controls.Find("it6", true);
                if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                {
                    controls[0].Text = dt.Rows[0]["NIHSS"].ToString().Trim();
                }
            }
            dt.Dispose();
        }

        /// <summary>
        /// 체크박스 체크시 OPD F/U 자동입력 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void SetAutoOPDFU(Form frm, string strTag)
        {
            if (strTag.IndexOf(":") == -1)
                return;

            Control[] controls;
            controls = frm.Controls.Find(strTag.Split(':')[1], true);
            if (controls.Length > 0)
            {
                controls[0].Text = "OPD F/U";
            }
        }


        /// <summary>
        /// 전출기록읽기 
        /// VB setChartFormValue8함수 컨버전
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void GetTransferHistory(PsmhDb pDbCon, Form frm, string strChartDate, string strPano)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            strChartDate = VB.Val(strChartDate).ToString("0000-00-00");


            int i;



            if (frm is frmEmrChartNew)
            {
                EmrForm pForm = clsEmrChart.SerEmrFormInfo(pDbCon, (frm as frmEmrChartNew).mstrFormNo, (frm as frmEmrChartNew).mstrUpdateNo);
                if (pForm.FmOLDGB == 1)
                {
                    #region XML
                    SQL = "SELECT";
                    SQL += ComNum.VBLF + "extractValue(chartxml, '//ta1') TA01,";
                    SQL += ComNum.VBLF + "extractValue(chartxml, '//ta2') TA02,";
                    SQL += ComNum.VBLF + "extractValue(chartxml, '//ta3') TA03,";
                    SQL += ComNum.VBLF + "extractValue(chartxml, '//ta4') TA04,";
                    SQL += ComNum.VBLF + "extractValue(chartxml, '//ta5') TA05,";
                    SQL += ComNum.VBLF + "extractValue(chartxml, '//ta6') TA06";

                    SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML";
                    SQL += ComNum.VBLF + "WHERE CHARTDATE >= '" + Convert.ToDateTime(strChartDate).AddDays(-2).ToString("yyyyMMdd") + "'";
                    SQL += ComNum.VBLF + "  AND CHARTDATE <= '" + strChartDate.Replace("-", "") + "' ";
                    SQL += ComNum.VBLF + "  AND FORMNO = 2241";
                    SQL += ComNum.VBLF + "  AND PTNO = '" + strPano + "' ";
                    SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC ";
                    #endregion
                }
                else
                {
                    #region 신규
                    SQL = "SELECT ITEMCD, ITEMVALUE";
                    SQL += ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A";
                    SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
                    SQL += ComNum.VBLF + "     ON A.EMRNO = B.EMRNO";
                    SQL += ComNum.VBLF + "    AND A.EMRNOHIS = B.EMRNOHIS";
                    SQL += ComNum.VBLF + "    AND B.ITEMCD IN ('I0000014147', 'I0000032883', 'I0000000972', 'I0000032884', 'I0000032885', 'I0000032888')";
                    SQL += ComNum.VBLF + "WHERE CHARTDATE = (SELECT MAX(CHARTDATE) FROM KOSMOS_EMR.AEMRCHARTMST WHERE PTNO = A.PTNO AND FORMNO = 2241)";
                    //SQL += ComNum.VBLF + "WHERE CHARTDATE = (SELECT MAX(CHARTDATE) FROM KOSMOS_EMR.AEMRCHARTMST WHERE PTNO = A.PTNO AND MEDFRDATE = A.MEDFRDATE AND FORMNO = 2241)";
                    //SQL += ComNum.VBLF + "WHERE CHARTDATE >= '" + Convert.ToDateTime(strChartDate).AddDays(-2).ToString("yyyyMMdd") + "'";
                    //SQL += ComNum.VBLF + "  AND CHARTDATE <= '" + strChartDate.Replace("-", "") + "' ";
                    SQL += ComNum.VBLF + "  AND FORMNO = 2241";
                    SQL += ComNum.VBLF + "  AND PTNO = '" + strPano + "' ";
                    SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC ";
                    #endregion
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (pForm.FmOLDGB == 1)
                    {
                        #region XML
                        Control[] controls;
                        for (i = 1; i < 7; i++)
                        {
                            controls = frm.Controls.Find("ta" + i, true);
                            if (controls.Length > 0 && controls[0].Text.Trim().Length == 0)
                            {
                                controls[0].Text = ChangeWebCharToCshop(dt.Rows[0]["TA" + i.ToString("00")].ToString().Trim());
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 신규
                        Control[] controls;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            string strItem = dt.Rows[i]["ITEMCD"].ToString();
                            if (strItem == "I0000032888")
                            {
                                strItem = "I0000032886";
                            }
                            controls = frm.Controls.Find(strItem, true);
                            if (controls.Length > 0)
                            {
                                controls[0].Text = ChangeWebCharToCshop(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                            }
                        }
                        #endregion
                    }

                }

                dt.Dispose();
            }
        }


        public static void GetTransferHistory2(PsmhDb pDbCon, Form frm, EmrPatient AcpEmr, string strChartDate, EmrForm pForm, string strEmrNo = "")
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;
            strChartDate = VB.Val(strChartDate).ToString("0000-00-00");

            int i;

            if (frm is frmEmrChartNew)
            {
                SQL = "SELECT B.ITEMCD, B.ITEMVALUE";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRCHARTROW B";
                SQL += ComNum.VBLF + "     ON A.EMRNO = B.EMRNO";
                SQL += ComNum.VBLF + "    AND A.EMRNOHIS = B.EMRNOHIS";
                SQL += ComNum.VBLF + "    AND B.ITEMCD IN ('I0000014147', 'I0000032883', 'I0000000972', 'I0000032884', 'I0000032885', 'I0000032888')";
                SQL += ComNum.VBLF + "WHERE A.EMRNO = " + strEmrNo;
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    Control[] controls;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        string strItem = dt.Rows[i]["ITEMCD"].ToString();
                        if (strItem == "I0000032888")
                        {
                            strItem = "I0000032886";
                        }
                        controls = frm.Controls.Find(strItem, true);
                        if (controls.Length > 0)
                        {
                            controls[0].Text = ChangeWebCharToCshop(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                        }
                    }
                }
                dt.Dispose();
            }
        }

        /// <summary>
        /// 퇴원요약지 진단명/부진단명 옮기는 함수
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void DiagTextChange(Form frm, string strTag)
        {
            string[] ArrCtrl = strTag.Split(':')[1].Split(',');

            if(ArrCtrl.Length != 3)
            {
                return;
            }

            //위로 올릴건지 내릴건지 여부 1: 진단 => 부진단 내림, 0: 부진단 => 주진단 옮김
            string strOption = ArrCtrl[2];

            Control[] ctrl = frm.Controls.Find(ArrCtrl[0].Trim(), true);
            Control MainDiag = null;
            Control SubDiag = null;

            //부진단명 컨트롤
            if (ctrl != null && ctrl.Length > 0)
            {
                SubDiag = frm.Controls.Find(ArrCtrl[0].Trim(), true)[0];
            }

            //진단명 컨트롤
            ctrl = frm.Controls.Find(ArrCtrl[1].Trim(), true);
            if (ctrl != null && ctrl.Length > 0)
            {
                MainDiag = frm.Controls.Find(ArrCtrl[1].Trim(), true)[0];
            }

            if (SubDiag == null || MainDiag == null)
            {
                return;
            }

            #region 진단명 => 부진담명으로 내리기
            if (strOption == "1")
            {
                if( SubDiag.Text.Length > 0)
                {
                    return;
                }
                else
                {
                    //현재 메인진단명을 부진단명 텍스트항목으로 옮기고, 주진단명을 삭제한다.
                    SubDiag.Text = MainDiag.Text;
                    MainDiag.Text = "";
                }

                return;
            }
            #endregion

            #region 부진단 => 주진단명으로 옮기기
            //이미 주진단명이 작성되어있다면 
            if (MainDiag.Text.Length > 0)
            {
                return;
            }

            // 부진단명 삭제
            MainDiag.Text = SubDiag.Text;
            SubDiag.Text = "";
            #endregion
        }

        /// <summary>
        /// 퇴원요약지 진단명/부진단명 옮기는 함수
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        public static void DiagTextChangeNew(Form frm, string strTag)
        {
            string[] ArrCtrl = strTag.Split(':')[1].Split(',');

            if (ArrCtrl.Length != 3)
            {
                return;
            }

            //위로 올릴건지 내릴건지 여부 1: 진단 => 부진단 내림, 0: 부진단 => 주진단 옮김
            string strOption = ArrCtrl[2];

            List<Control> MainDiag = frm.Controls.Find(ArrCtrl[1].Trim(), true)[0].Controls.OfType<Control>().OrderBy(c => c.Left).ToList();

            if ((MainDiag[0].Text.Length >  0 && strOption.Equals("0")) ||
                (MainDiag[0].Text.Length == 0 && strOption.Equals("1")))
            {
                return;
            }

            List<Control> SubDiag = frm.Controls.Find(ArrCtrl[0].Trim(), true)[0].Controls.OfType<Control>().OrderBy(c => c.Left).ToList();

            for(int i = 0; i < MainDiag.Count; i++)
            {
                if (strOption.Equals("1"))
                {
                    SubDiag[i].Text = MainDiag[i].Text;
                    MainDiag[i].Text = "";
                }
                else
                {
                    MainDiag[i].Text = SubDiag[i].Text;
                    SubDiag[i].Text = "";
                }
            }

        }

        /// <summary>
        /// 서식지 폼의 아이템에 기본값을 세팅한다.
        /// </summary>
        public static void SetControlInitValue(PsmhDb pDbCon, Form frm, FormXml[] pFormXml, EmrForm pForm, EmrPatient AcpEmr = null, string strChartDate = "")
        {
            for (int i = 0; i < pFormXml.Length; i++)
            {
                Control[] Ctl;
                if (pFormXml[i].strINITVALUE.Trim().IsNullOrEmpty())
                {
                    Ctl = frm.Controls.Find(pFormXml[i].strCONTROLNAME.ToString(), true);
                    if (Ctl != null && Ctl.Length > 0) 
                    {
                        if (Ctl[0] is TextBox && pFormXml[i].strTEXT.Length > 0)
                        {
                            ((TextBox)Ctl[0]).Text = pFormXml[i].strTEXT;
                        }
                        else if (Ctl[0] is CheckBox)
                        {
                            ((CheckBox)Ctl[0]).Checked = pFormXml[i].strCHECKED.ToUpper().Equals("TRUE");
                        }
                        else if (Ctl[0] is RadioButton)
                        {
                            ((RadioButton)Ctl[0]).Checked = pFormXml[i].strCHECKED.ToUpper().Equals("TRUE");
                        }
                    }
                    continue;
                }

                Ctl = frm.Controls.Find(pFormXml[i].strCONTROLNAME.ToString(), true);
                if (Ctl != null)
                {
                    if (Ctl.Length > 0 &&  AcpEmr != null)
                    {
                        Control obj = Ctl[0];

                        if(pFormXml[i].strINITVALUE.Trim().IndexOf("Set_FormPatInfo_GetItemValue") != -1 )
                        {
                            ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_GetItemValue(pDbCon, AcpEmr, pFormXml[i].strINITVALUE.Substring(pFormXml[i].strINITVALUE.IndexOf(":") + 1));
                        }
                        else
                        {
                            switch (pFormXml[i].strINITVALUE.Trim())
                            {
                                case "Set_FormPatInfo_MainSymptoms":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_MainSymptoms(pDbCon, pForm, AcpEmr);
                                    break;
                                case "Set_FormPatInfo_MainDisease_Eng":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_MainDisease(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, "ENG");
                                    break;
                                case "Set_FormPatInfo_MainDisease_Kor":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_MainDisease(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, "KOR");
                                    break;
                                case "Set_FormPatInfo_SubDisease_Eng":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_SubDisease(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, obj.Name);
                                    break;
                                case "Set_FormPatInfo_OpName":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OpName(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
                                    break;
                                case "Set_FormPatInfo_IpdTrans":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_IpdTrans(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
                                    break;
                                case "Set_FormPatInfo_TestResult":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_TestResult(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
                                    break;
                                case "Set_FormPatInfo_LabResult":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_LabResult(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
                                    break;
                                case "Set_FormPatInfo_LabResultNew":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_LabResultNew(pDbCon, AcpEmr);
                                    break;
                                case "Set_FormPatInfo_MedicineDischarge":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_MedicineDischarge(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate);
                                    break;
                                case "Set_FormPatInfo_MedicineDischargeKor":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_MedicineDischarge(pDbCon, AcpEmr.ptNo, AcpEmr.medFrDate, AcpEmr.medEndDate, true);
                                    break;
                                case "Set_FormPatInfo_NowDate":
                                    ((TextBox)obj).Text = ComQuery.CurrentDateTime(pDbCon, "D");
                                    break;
                                //작성일
                                case "Set_FormPatInfo_WriteDate":
                                    ((TextBox)obj).Text = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S")).ToShortDateString();
                                    break;
                                //작성시간
                                case "Set_FormPatInfo_WriteTime":
                                    ((TextBox)obj).Text = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S")).ToString("HH:mm");
                                    break;
                                //작성자
                                case "Set_FormPatInfo_WriteName":
                                    ((TextBox)obj).Text = clsType.User.UserName;
                                    break;
                                //입원일자
                                case "Set_FormPatInfo_ADMISSION_DATE":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_ADMISSION_DATE(pDbCon, AcpEmr, "DATE");
                                    break;
                                //입원시간
                                case "Set_FormPatInfo_ADMISSION_TIME":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_ADMISSION_DATE(pDbCon, AcpEmr, "TIME");
                                    break;
                                //병동
                                case "Set_FormPatInfo_ADMISSION_WARD":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = AcpEmr.ward;
                                    break;
                                //병실
                                case "Set_FormPatInfo_ADMISSION_ROOM":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = AcpEmr.room;
                                    break;
                                //외래 예약시간
                                case "Set_FormPatInfo_OPD_RESERVED":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OPD_RESERVED(pDbCon, AcpEmr);
                                    break;
                                //입원 기간내 수술(진단명)
                                case "Set_FormPatInfo_OP_DIAGNOSIS":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OP_INFO(pDbCon, AcpEmr, "진단");
                                    break;
                                //입원 기간내 수술(시술명)
                                case "Set_FormPatInfo_OP_TITLE":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OP_INFO(pDbCon, AcpEmr, "시술");
                                    break;
                                //가장 최근 BST
                                case "Set_FormPatInfo_BST":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_BST(pDbCon, AcpEmr);
                                    break;
                                //번호
                                case "Set_FormPatInfo_PHONE":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_PHONE(pDbCon, AcpEmr);
                                    break;
                                //주소
                                case "Set_FormPatInfo_JUSO":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_JUSO(pDbCon, AcpEmr);
                                    break;
                                //알러지정보
                                case "Set_FormPatInfo_ALLERGY":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_ALLERGY(pDbCon, AcpEmr);
                                    break;
                                //감염정보
                                case "Set_FormPatInfo_INFECT":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_INFECT(pDbCon, AcpEmr);
                                    break;
                                //입원한 과
                                case "Set_FormPatInfo_DeptCode":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = AcpEmr.medDeptCd;
                                    break;
                                //주치의 성명
                                case "Set_FormPatInfo_DrName":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = AcpEmr.medDrName;
                                    break;
                                //환자 보험정보
                                case "Set_FormPatInfo_BI":
                                    if (AcpEmr == null)
                                        continue;                                    

                                    ((TextBox)obj).Text = AcpEmr.biname.Length > 0 ? AcpEmr.biname : FormPatInfoFunc.Set_FormPatInfo_BI(pDbCon, AcpEmr);
                                    break;
                                #region ER 함수
                                case "Set_FormPatInfo_ERInDate":
                                    string strDate = FormPatInfoFunc.Set_FormPatInfo_ERInDate(pDbCon, AcpEmr);
                                    ((TextBox)obj).Text = strDate.Length > 0 ? Convert.ToDateTime(strDate).ToShortDateString() : "";
                                    break;
                                case "Set_FormPatInfo_ERInTime":
                                    strDate = FormPatInfoFunc.Set_FormPatInfo_ERInDate(pDbCon, AcpEmr);
                                    ((TextBox)obj).Text = strDate.Length > 0 ? Convert.ToDateTime(strDate).ToString("HH:mm") : "";
                                    break;
                                #endregion
                                case "Set_Ward_CallNumber":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_Ward_CallNumber(pDbCon, AcpEmr);
                                    break;
                                case "Set_FormPatInfo_NowDisease":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_NowDisease(pDbCon, AcpEmr);
                                    break;
                                case "Set_FormPatInfo_OldGetItemValue":
                                    #region 이전 기록지 데이터 연동
                                    switch((frm as frmEmrChartNew).mstrFormNo)
                                    {
                                        //완화의료 초기상담
                                        case "2555":
                                            if (obj.Name.Equals("it5"))
                                            {
                                                ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2311", "it23");
                                            }
                                            else if(obj.Name.Equals("ta3"))
                                            {
                                                string strTemp = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2311", "ta1");
                                                if (strTemp.Length > 0)
                                                {
                                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2311", "it23");
                                                }
                                            }
                                            break;
                                        //완화의료 경과 상담
                                        case "2726":
                                            //말기진단일 완화의료등록
                                            if (obj.Name.Equals("dt2") || obj.Name.Equals("dt3"))
                                            {
                                                ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2555", obj.Name);
                                            }
                                            break;
                                        case "1544":
                                        case "1545":
                                        case "1818":
                                        case "1564":
                                        case "2348":
                                        case "2465":

                                            #region 키, 몸무게 비어있을때 각 기록지에서 가져옴
                                            //간호정보 조사지
                                            string strHEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2311", "it45");
                                            string strWEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2311", "it46");

                                            //간호정보 조사지 (소아용)
                                            if (string.IsNullOrWhiteSpace(strHEIGHT))
                                            {
                                                strHEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2294", "it51");
                                                strWEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2294", "it52");
                                            }

                                            //간호정보 조사지 (산모용)
                                            if (string.IsNullOrWhiteSpace(strHEIGHT))
                                            {
                                                strHEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2295", "it96");
                                                strWEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2295", "it45");
                                            }

                                            //간호정보 조사지 (산모)
                                            if (string.IsNullOrWhiteSpace(strHEIGHT))
                                            {
                                                strHEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2356", "it50");
                                                strWEIGHT = FormPatInfoFunc.Set_FormPatInfo_OldGetItemValue(pDbCon, AcpEmr, "2356", "it49");
                                            }
                                            #endregion

                                            #region 키 데이터 있을때
                                            if (string.IsNullOrWhiteSpace(strHEIGHT) == false)
                                            {
                                                switch ((frm as frmEmrChartNew).mstrFormNo)
                                                {
                                                    case "1544":
                                                    case "1545":
                                                    case "2465":
                                                        if (obj.Name.Equals("it9"))
                                                        {
                                                            ((TextBox)obj).Text = strHEIGHT;
                                                        }
                                                        else if (obj.Name.Equals("it10"))
                                                        {
                                                            ((TextBox)obj).Text = strWEIGHT;
                                                        }
                                                        break;
                                                    case "1818":
                                                    case "2348":
                                                        double nState = VB.Val(strWEIGHT) / ((VB.Val(strHEIGHT) / 100) * (VB.Val(strHEIGHT) / 100) * (AcpEmr.sex.Equals("남자") ? 22 : 21));
                                                        ((TextBox)obj).Text = VB.Format(nState, "###%").ToString();
                                                        break;
                                                    case "1564":
                                                        if (obj.Name.Equals("it7"))
                                                        {
                                                            ((TextBox)obj).Text = strHEIGHT;
                                                        }
                                                        else if (obj.Name.Equals("it8"))
                                                        {
                                                            ((TextBox)obj).Text = strWEIGHT;
                                                        }
                                                        break;
                                                }
                                            }
                                            #endregion                                        
                                            break;
                                    }
                                    #endregion
                                    break;
                                case "Set_FormPatInfo_HEIGHT":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_BodyInfo(pDbCon, AcpEmr, "I0000000002");
                                    break;
                                case "Set_FormPatInfo_WEIGHT":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_BodyInfo(pDbCon, AcpEmr, "I0000000418");
                                    break;
                                case "Set_Login_Buse":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_SabunBuseName(pDbCon);
                                    break;

                                case "Set_FormPatInfo_ENDO_NAME":
                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_ENDO_NAME(pDbCon, AcpEmr);
                                    break;

                                case "Set_FormPatInfo_NUR_DIAGNOSIS":
                                    if (AcpEmr == null)
                                        continue;

                                    ((TextBox)obj).Text = FormPatInfoFunc.Set_FormPatInfo_NUR_DIAGNOSIS(pDbCon, AcpEmr);
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 외래 예약한 경우 예약 시간 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="strDrName"></param>
        /// <param name="strDeptCd"></param>
        public static DataTable Set_FormPatInfo_OPD_RESERVED(PsmhDb pDbCon, EmrPatient emrPatient)
        {
            DataTable rtnVal = null;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_OPD_RESERVED(emrPatient);

            try
            {
                string sqlErr = clsDB.GetDataTableREx(ref rtnVal, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 피부사정 점수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        /// <param name="FormNo">기록지번호</param>
        /// <param name="ItemCd">아이템 번호</param>
        /// <returns></returns>
        public static DataTable Set_FormPatInfo_BRADEN(PsmhDb pDbCon, EmrPatient emrPatient, string ChartDate)
        {
            DataTable dt = null;
            string SQL = FormPatInfoQuery.Query_FormPatInfo_BRADEN(emrPatient, ChartDate);

            try
            {
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return dt;
                }

                return dt;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return dt;
        }


        /// <summary>
        /// 체크/라디오 버튼 체크한 값에 더하기 값 반환
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        private static void CheckSumPlus(Form frm, string strTag)
        {
            string[] ArrCtrl = strTag.Split(':')[1].Split(',');
            double intTotSum = 0;
            for (int i = 0; i < ArrCtrl.Length; i++)
            {
                //해당 컨트롤의 Text가 문자열일경우에만
                if (VB.IsNumeric(ArrCtrl[i]) == false)
                {
                    Control[] control = frm.Controls.Find(ArrCtrl[i].Trim(), true);
                    if (control.Length > 0)
                    {
                        bool chk = false;
                        if (control[0] is CheckBox)
                        {
                            chk = ((CheckBox)control[0]).Checked;
                        }
                        else if (control[0] is RadioButton)
                        {
                            chk = ((RadioButton)control[0]).Checked;
                        }

                        if (chk && i < ArrCtrl.Length - 1)
                        {
                            if( VB.IsNumeric(ArrCtrl[i + 1].Trim()))
                            {
                                intTotSum += VB.Val(ArrCtrl[i + 1].Trim());
                            }
                        }

                        //총점
                        if (i == ArrCtrl.Length - 1)
                        {
                            control[0].Text = intTotSum.ToString();
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// 체크/라디오 버튼 체크한 값만 더하기
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        private static void CheckSum(Form frm, string strTag)
        {
            string[] ArrCtrl = strTag.Split(':')[1].Split(',');
            double intTotSum = 0;
            for (int i = 0; i < ArrCtrl.Length; i++)
            {
                Control[] control = frm.Controls.Find(ArrCtrl[i].Trim(), true);
                if (control.Length > 0)
                {
                    bool chk = false;
                    if (control[0] is CheckBox) 
                    {
                        chk = ((CheckBox)control[0]).Checked;
                    }
                    else if(control[0] is RadioButton)
                    {
                        chk = ((RadioButton)control[0]).Checked;
                    }

                    if(chk)
                    {
                        intTotSum += VB.Val(control[0].Text);                        
                    }

                    //총점
                    if (i == ArrCtrl.Length - 1)
                    {
                        control[0].Text = intTotSum.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 텍스트값 더하기
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        private static void TextSum(Form frm, string strTag)
        {
            strTag = strTag.Substring(strTag.IndexOf(":") + 1);
            string[] CountArr = strTag.Split(':');

            for(int i = 0; i < CountArr.Length; i++)
            {
                string[] ArrCtrl = CountArr[i].Split(',');
                int intTotSum = 0;

                for (int j = 0; j < ArrCtrl.Length; j++)
                {
                    Control[] control = frm.Controls.Find(ArrCtrl[j].Trim(), true);
                    if (control != null)
                    {
                        //총점
                        if (j == ArrCtrl.Length - 1)
                        {
                            control[0].Text = intTotSum.ToString();
                        }
                        else
                        {
                            intTotSum += (int)VB.Val(control[0].Text);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 폐렴지표서식 점수계산 버튼 함수.
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        private static void PneumoniaSum(Form frm, string strTag)
        {
            string[] ArrCtrl = strTag.Split(':')[1].Split(',');
            int intTotSum = 0;

            for (int i = 0; i < ArrCtrl.Length; i++)
            {
                Control[] control = frm.Controls.Find(ArrCtrl[i].Trim(), true);
                if (control != null)
                {
                    //총점
                    if (i == ArrCtrl.Length - 1)
                    {
                        control[0].Text = intTotSum.ToString();
                    }
                    else
                    {
                        intTotSum += (int)VB.Val(control[0].Text);
                    }
                }
            }
        }

        /// <summary>
        /// 사용법 -> AddSign:싸인 들어갈 아템이름(피쳐박스):해당 EMRNO
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strTag"></param>
        /// <param name="AcpEmr"></param>
        private static void AddSign(PsmhDb pDbCon, Form frm, string strTag, EmrPatient AcpEmr)
        {
            string stritemCd = VB.Split(strTag, ":")[1];
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            DataTable dt = null;
            int i = 0;

            SQL = "";
            SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN FROM " + ComNum.DB_EMR + "AEMRUSERSIGN";
            SQL = SQL + ComNum.VBLF + "    WHERE USEID = '" + clsType.User.IdNumber + "' ";
            SQL = SQL + ComNum.VBLF + "    ORDER BY FILESEQ ASC";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }
            if (dt.Rows.Count > 0)
            {
                string strImage = "";

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strImage = strImage + dt.Rows[i]["USESIGN"].ToString().Trim();
                }

                Control[] imgItem = frm.Controls.Find(stritemCd, true);

                if (imgItem != null)
                {
                    if (imgItem.Length > 0)
                    {
                        if (imgItem[0] is PictureBox)
                        {

                            byte[] b = Convert.FromBase64String(strImage);
                            MemoryStream stream = new MemoryStream(b);
                            Bitmap image1 = new Bitmap(stream);

                            int intWidth = 100;
                            int intHeight = 24;

                            Bitmap newImage;

                            intWidth = ((PictureBox)imgItem[0]).Width;
                            intHeight = ((PictureBox)imgItem[0]).Height;

                            newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                            Graphics graphics_1 = Graphics.FromImage(newImage);
                            graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                            graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                            graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                            ((PictureBox)imgItem[0]).Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);


                            InsertAddSign(pDbCon, VB.Split(strTag, ":")[2], VB.Split(strTag, ":")[1], clsType.User.IdNumber);

                        }
                    }
                }
            }
            dt.Dispose();
            dt = null;
        }

        private static void InsertAddSign(PsmhDb pDbCon, string strEmrNo, string strItemCd, string strDrCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_EMR + "AEMRDRSIGNADD        ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo + "      ";
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strItemCd + "'     ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRDRSIGNADD       ";
                SQL = SQL + ComNum.VBLF + "(       ";
                SQL = SQL + ComNum.VBLF + "   EMRNO, ITEMCD, DRCODE       ";
                SQL = SQL + ComNum.VBLF + ")        ";
                SQL = SQL + ComNum.VBLF + "VALUES        ";
                SQL = SQL + ComNum.VBLF + "(        ";
                SQL = SQL + ComNum.VBLF + "  " + strEmrNo + "  ,        ";
                SQL = SQL + ComNum.VBLF + " '" + strItemCd + "'   ,        ";
                SQL = SQL + ComNum.VBLF + " '" + strDrCode + "'      ";
                SQL = SQL + ComNum.VBLF + ")       ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        public static void AddSignLoad(PsmhDb pDbCon, PictureBox picSign, string strEmrNo, mtsPanel15.mPanel panGroup)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;

            if (panGroup != null)
            {
                panGroup.Visible = true;
            }

            SQL = "";
            SQL = "SELECT B.USEID, B.FILESEQ, B.FEXTENSION, B.USESIGN        ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRDRSIGNADD  A        ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRUSERSIGN B        ";
            SQL = SQL + ComNum.VBLF + "    ON   A.DRCODE = B.USEID     ";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO = " + strEmrNo + "        ";
            SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = '" + picSign.Name + "'        ";
            SQL = SQL + ComNum.VBLF + "    ORDER BY B.FILESEQ ASC        ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                string strImage = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strImage = strImage + dt.Rows[i]["USESIGN"].ToString().Trim();
                }

                byte[] b = Convert.FromBase64String(strImage);
                MemoryStream stream = new MemoryStream(b);
                Bitmap image1 = new Bitmap(stream);

                int intWidth = 100;
                int intHeight = 24;

                Bitmap newImage;

                intWidth = picSign.Width;
                intHeight = picSign.Height;

                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                picSign.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);

            }
            dt.Dispose();
            dt = null;
        }

        //2016-07-15 유진호 
        //전동기록 전자서명 로드
        public static void GetSignLoad(PsmhDb pDbCon, PictureBox picSign, string strUserID)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;


            SQL = "";
            SQL = "SELECT USEID, FILESEQ, FEXTENSION, USESIGN        ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRUSERSIGN          ";
            SQL = SQL + ComNum.VBLF + " WHERE USEID = '" + strUserID + "'        ";
            SQL = SQL + ComNum.VBLF + " ORDER BY FILESEQ ASC        ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                string strImage = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strImage = strImage + dt.Rows[i]["USESIGN"].ToString().Trim();
                }

                byte[] b = Convert.FromBase64String(strImage);
                MemoryStream stream = new MemoryStream(b);
                Bitmap image1 = new Bitmap(stream);

                int intWidth = 100;
                int intHeight = 24;

                Bitmap newImage;

                intWidth = picSign.Width;
                intHeight = picSign.Height;

                newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                Graphics graphics_1 = Graphics.FromImage(newImage);
                graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);

                picSign.Image = Image.FromHbitmap(newImage.GetHbitmap(), IntPtr.Zero);

            }
            dt.Dispose();
            dt = null;
        }

        private static void PainScaleHelp(Form frm, string strTag)
        {
            frmPainScaleHelp frmPainScaleHelpX = new frmPainScaleHelp();
            frmPainScaleHelpX.TopMost = true;
            frmPainScaleHelpX.ShowDialog(frm);
        }


        //CheckLock:Penal:CheckBox
        private static void CheckLock(Form frm, Control ct, string strTag)
        {
            string strPanel = VB.Split(strTag, ":")[1];
            string strCheckBox = VB.Split(strTag, ":")[2];

            Control[] panel = null;
            Control[] checkbox = null;
            Control[] controls = null;

            panel = frm.Controls.Find(strPanel, true);
            checkbox = frm.Controls.Find(strCheckBox, true);

            if (panel != null && checkbox != null)
            {
                if (panel.Length > 0 && checkbox.Length > 0)
                {
                    if (panel[0] is mtsPanel15.mPanel && checkbox[0] is CheckBox)
                    {
                        if (((CheckBox)checkbox[0]).Checked == true)
                        {
                            controls = ComFunc.GetAllControls(panel[0]);
                            foreach (Control objControl in controls)
                            {
                                if (objControl is CheckBox)
                                {
                                    if (((CheckBox)objControl).Name != strCheckBox)
                                    {
                                        ((CheckBox)objControl).Checked = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // 한 아이탬 코드에 여러 아이탬 값을 가져 올때 사용한다.
        //TextBox만 가능
        //사용법 ItemMisMatch:서식번호:받을 TextBoxItemCode: 가저 올 TextBoxItemCode
        private static void ItemMisMatch(PsmhDb pDbCon, Form frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            int i = 0;
            string[] strFormNo = VB.Split(VB.Split(strTag, ":")[1], ",");
            string stritem = VB.Split(strTag, ":")[2];
            string[] strGetitem = VB.Split(VB.Split(strTag, ":")[3], ",");
            if (AcpEmr == null) return;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO =  (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1";
            SQL = SQL + ComNum.VBLF + "                    WHERE A1.PTNO = '" + AcpEmr.ptNo + "'";
            if (strFormNo[0] == "582")
            {
                SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTDATE <= " + strChartDate;
            }
            SQL = SQL + ComNum.VBLF + "                        AND A1.FORMNO IN (";
            for (i = 0; i < strFormNo.Length; i++)
            {
                if (i == 0)
                {
                    SQL = SQL + VB.Val(strFormNo[i]);
                }
                else
                {
                    SQL = SQL + "," + VB.Val(strFormNo[i]);
                }
            }
            SQL = SQL + ComNum.VBLF + "                        )";
            SQL = SQL + ComNum.VBLF + "                        AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F";
            SQL = SQL + ComNum.VBLF + "                                            WHERE F.FORMNO = A1.FORMNO) ";
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTDATE = ( SELECT MAX(A2.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRCHARTMST A2";
            SQL = SQL + ComNum.VBLF + "                                            WHERE A2.PTNO = '" + AcpEmr.ptNo + "'";
            //SQL = SQL + ComNum.VBLF + "                                            WHERE A2.ACPNO = '" + AcpEmr.acpNo + "'";
            if (strFormNo[0] == "582")
            {
                SQL = SQL + ComNum.VBLF + "                                                AND A2.CHARTDATE <= " + strChartDate;
            }
            SQL = SQL + ComNum.VBLF + "                                                AND A2.FORMNO IN (";

            for (i = 0; i < strFormNo.Length; i++)
            {
                if (i == 0)
                {
                    SQL = SQL + VB.Val(strFormNo[i]);
                }
                else
                {
                    SQL = SQL + "," + VB.Val(strFormNo[i]);
                }
            }
            SQL = SQL + ComNum.VBLF + "                                           ) ) )";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            Control[] item = null;
            item = frm.Controls.Find(stritem, true);

            if (item != null)
            {
                if (item.Length > 0)
                {
                    Control obj = (TextBox)item[0];
                    ((TextBox)obj).Focus();
                    for (i = 0; i < strGetitem.Length; i++)
                    {
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (strGetitem[i].ToString().Trim() == dt.Rows[k]["ITEMCD"].ToString().Trim())
                            {

                                if (item[0] is TextBox)
                                {
                                    ((TextBox)obj).AppendText(dt.Rows[k]["ITEMVALUE"].ToString().Trim() + ComNum.VBLF);
                                }


                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        // ItmeLockDis:p1,p2...
        private static void ItmeLockDis(Form frm, string strTag)
        {
            string[] strPenel = VB.Split(VB.Split(strTag, ":")[1], ",");

            for (int i = 0; i < strPenel.Length; i++)
            {
                Control[] penel = null;
                Control obj = null;

                penel = frm.Controls.Find(strPenel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)penel[0];

                        Control[] controls = null;
                        controls = ComFunc.GetAllControls(obj);
                        foreach (Control objControl in controls)
                        {
                            if (objControl is CheckBox)
                            {
                                ((CheckBox)objControl).Enabled = true;
                            }
                            if (objControl is RadioButton)
                            {
                                ((RadioButton)objControl).Enabled = true;
                            }
                            if (objControl is TextBox)
                            {
                                ((TextBox)objControl).Enabled = true;
                            }
                            if (objControl is ComboBox)
                            {
                                ((ComboBox)objControl).Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private static void GetSetItemLock(PsmhDb pDbCon, Form frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            string strFormNo = VB.Split(strTag, ":")[1];

            if (AcpEmr == null) return;

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO =  (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1";
            SQL = SQL + ComNum.VBLF + "                    WHERE A1.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                        AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F";
            SQL = SQL + ComNum.VBLF + "                                            WHERE F.FORMNO = A1.FORMNO) ";
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTTIME = ( SELECT MAX(A2.CHARTTIME) AS CHARTTIME FROM " + ComNum.DB_EMR + "AEMRCHARTMST A2";
            SQL = SQL + ComNum.VBLF + "                                            WHERE A2.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                                                AND A2.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                                                AND A2.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                                            ) )";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Control[] txtVal = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true);
                if (txtVal != null)
                {
                    if (txtVal.Length > 0)
                    {
                        if (txtVal[0] is TextBox)
                        {
                            Control obj = (TextBox)txtVal[0];
                            ((TextBox)obj).Text = "";
                            ((TextBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            ((TextBox)obj).Enabled = false;
                        }
                        else if (txtVal[0] is ComboBox)
                        {
                            Control obj = (ComboBox)txtVal[0];
                            ((ComboBox)obj).Text = "";
                            ((ComboBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            ((ComboBox)obj).Enabled = false;
                        }
                        else if (txtVal[0] is CheckBox)
                        {
                            Control obj = (CheckBox)txtVal[0];

                            if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                            {
                                ((CheckBox)obj).Checked = true;
                                ((CheckBox)obj).Enabled = false;
                            }
                            else
                            {
                                ((CheckBox)obj).Checked = false;
                                ((CheckBox)obj).Enabled = false;
                            }
                        }
                        else if (txtVal[0] is RadioButton)
                        {
                            Control obj = (RadioButton)txtVal[0];

                            if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                            {
                                ((RadioButton)obj).Checked = true;
                                ((RadioButton)obj).Enabled = false;
                            }
                            else
                            {
                                ((RadioButton)obj).Checked = false;
                                ((RadioButton)obj).Enabled = false;
                            }
                        }
                    }
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        //다른 서식지의 최근 정보를 패널 별로 받아오게 한다.
        //GetSetPanelItme:FormNo:Panel1,Panel2,Panel3.....
        private static void GetSetPanelItme(PsmhDb pDbCon, Form frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            string strFormNo = VB.Split(strTag, ":")[1];
            string[] strPenel = VB.Split(VB.Split(strTag, ":")[2], ",");

            if (AcpEmr == null) return;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO =  (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1";
            SQL = SQL + ComNum.VBLF + "                    WHERE A1.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTTIME = ( SELECT MAX(A2.CHARTTIME) AS CHARTTIME FROM " + ComNum.DB_EMR + "AEMRCHARTMST A2";
            SQL = SQL + ComNum.VBLF + "                                            WHERE A2.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                                                AND A2.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                                                AND A2.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                                            ) )";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            for (int i = 0; i < strPenel.Length; i++)
            {
                Control[] penel = null;

                penel = frm.Controls.Find(strPenel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        Control objPan = (mtsPanel15.mPanel)penel[0];

                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            Control[] txtVal = objPan.Controls.Find(dt.Rows[k]["ITEMCD"].ToString().Trim(), true);
                            if (txtVal != null)
                            {
                                if (txtVal.Length > 0)
                                {
                                    if (txtVal[0] is TextBox)
                                    {
                                        Control obj = (TextBox)txtVal[0];
                                        ((TextBox)obj).Text = "";
                                        ((TextBox)obj).Text = dt.Rows[k]["ITEMVALUE"].ToString().Trim();
                                    }
                                    else if (txtVal[0] is ComboBox)
                                    {
                                        Control obj = (ComboBox)txtVal[0];
                                        ((ComboBox)obj).Text = "";
                                        ((ComboBox)obj).Text = dt.Rows[k]["ITEMVALUE"].ToString().Trim();
                                    }
                                    else if (txtVal[0] is CheckBox)
                                    {
                                        Control obj = (CheckBox)txtVal[0];

                                        if (dt.Rows[k]["ITEMVALUE"].ToString().Trim() == "1")
                                        {
                                            ((CheckBox)obj).Checked = true;
                                        }
                                        else
                                        {
                                            ((CheckBox)obj).Checked = false;
                                        }
                                    }
                                    else if (txtVal[0] is RadioButton)
                                    {
                                        Control obj = (RadioButton)txtVal[0];

                                        if (dt.Rows[k]["ITEMVALUE"].ToString().Trim() == "1")
                                        {
                                            ((RadioButton)obj).Checked = true;
                                        }
                                        else
                                        {
                                            ((RadioButton)obj).Checked = false;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        // 클릭을 하면 지정된 패널이 숨겨지면서 패널안의 내용이 모두 초기화
        private static void PanDeleteClick(Form frm, string strTag)
        {

            string[] strPenel = VB.Split(VB.Split(strTag, ":")[1], ",");

            for (int i = 0; i < strPenel.Length; i++)
            {
                Control[] penel = null;
                Control obj = null;

                penel = frm.Controls.Find(strPenel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)penel[0];

                        Control[] controls = null;
                        controls = ComFunc.GetAllControls(obj);
                        foreach (Control objControl in controls)
                        {
                            if (objControl is CheckBox)
                            {
                                ((CheckBox)objControl).Checked = false;
                            }
                            if (objControl is RadioButton)
                            {
                                ((RadioButton)objControl).Checked = false;
                            }
                            if (objControl is TextBox)
                            {
                                ((TextBox)objControl).Text = "";
                            }
                        }
                        obj.Visible = false;
                    }
                }
            }
        }

        private static void UMLCodesSet(Form frm, Control ct, string strTag)
        {
            string[] strCode = VB.Split(VB.Split(strTag, ":")[1], ",");
            Control[] TxtUMLCodes = null;
            Control[] TxtUMLCodesName = null;
            Control objUMLCodes = null;
            Control objUMLCodesName = null;

            for (int i = 0; i < 3; i++)
            {
                TxtUMLCodes = frm.Controls.Find("I0000030464_" + Convert.ToString(i), true);
                TxtUMLCodesName = frm.Controls.Find("I0000002168_" + Convert.ToString(i), true);

                if (TxtUMLCodes != null && TxtUMLCodesName != null)
                {
                    if (TxtUMLCodes.Length > 0 && TxtUMLCodesName.Length > 0)
                    {
                        objUMLCodes = (TextBox)TxtUMLCodes[0];
                        objUMLCodesName = (TextBox)TxtUMLCodesName[0];

                        if (((CheckBox)ct).Checked == false)
                        {
                            if (objUMLCodes.Text == strCode[0].ToString())
                            {
                                objUMLCodes.Text = "";
                                objUMLCodesName.Text = "";
                                break;
                            }
                        }
                        else
                        {
                            if (objUMLCodes.Text == "")
                            {
                                objUMLCodes.Text = strCode[0].ToString();
                                objUMLCodesName.Text = ((CheckBox)ct).Text.Trim();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static void TimeSet(PsmhDb pDbCon, Form frm, string strTag)
        {
            string[] strTxt = VB.Split(VB.Split(strTag, ":")[1], ",");
            string strCurTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "T"), "T");
            strCurTime = VB.Left(strCurTime, 5);
            Control[] text = null;
            Control obj = null;

            for (int i = strTxt.Length - 1; i > -1; i--)
            {
                text = frm.Controls.Find(strTxt[i].ToString(), true);
                if (text != null)
                {
                    if (text.Length > 0)
                    {
                        obj = (TextBox)text[0];

                        if (obj.Text == "")
                        {
                            ((TextBox)obj).Focus();
                            ((TextBox)obj).AppendText(strCurTime);
                        }
                    }
                }
            }
        }

       
        private static void GetSetSOAP(Control frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            //기존 작성된 것인지 파악해서 드로잉 폼을 띄운다.
            if (clsEmrPublic.frmEmrProgMacroX != null)
            {
                clsEmrPublic.frmEmrProgMacroX.Dispose();
                clsEmrPublic.frmEmrProgMacroX = null;
            }
            clsEmrPublic.frmEmrProgMacroX = new frmEmrProgMacro(frm, VB.Split(strTag, ":")[1]);
            //clsEmrPublic.frmEmrProgMacroX.rEventClosed += new frmEmrProgMacro.EventClosed(frmEmrProgMacro_EventClosed);
            clsEmrPublic.frmEmrProgMacroX.TopMost = true;
            clsEmrPublic.frmEmrProgMacroX.ShowDialog();
        }

        private static void frmEmrProgMacro_EventClosed()
        {
            clsEmrPublic.frmEmrProgMacroX.Dispose();
            clsEmrPublic.frmEmrProgMacroX = null;
        }


        public static void GetSetVitalOut(PsmhDb pDbCon, Control frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            string strFormNo = VB.Split(strTag, ":")[1];

            if (AcpEmr == null) return;


            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            //2016-09-09 각 항목별 데이터 추출
            SQL = SQL + ComNum.VBLF + "SELECT  ";
            SQL = SQL + ComNum.VBLF + " A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, B. ITEMCD,  B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE,  ";
            SQL = SQL + ComNum.VBLF + " CASE WHEN (B.ITEMCD = 'I0000000416') 			  ";//  			-- 체온 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "	            FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "	            ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "	            WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "		  		                  WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "	                                AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "	                                AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "	                                                    WHERE F.FORMNO = A1.FORMNO) ";
            SQL = SQL + ComNum.VBLF + "	                                AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME ";
            SQL = SQL + ComNum.VBLF + "											             FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2 ";
            SQL = SQL + ComNum.VBLF + "	                                                     WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											               AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "	                                                       AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											               AND (R2.ITEMCD = 'I0000000416' AND ITEMVALUE IS NOT NULL)  ) ) ";
            SQL = SQL + ComNum.VBLF + "	            AND ITEMCD = 'I0000000416') ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000001178') 			 ";//  			-- 맥박 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000001178' AND ITEMVALUE IS NOT NULL)  ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000001178') ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000000574') 		";//	  			-- 호흡 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000000574' AND ITEMVALUE IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000000574')			 ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000002018') 			  	 ";//		-- SBP 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000002018' AND ITEMVALUE IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000002018')		 ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000001765') 			 ";//  			-- DBP 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000001765' AND ITEMVALUE IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000001765')			 ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000019079') 			  	 ";//		-- BST 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";

            // 장비가 있는 과는 자기과 BST 데이터 가져고 가고
            // 장비가 없는 과는 마지막 데이터 가져감.
            if (AcpEmr.medDeptCd == "AK" || AcpEmr.medDeptCd == "CP" || AcpEmr.medDeptCd == "CV" || AcpEmr.medDeptCd == "ED" || AcpEmr.medDeptCd == "KH" || AcpEmr.medDeptCd == "PU" || AcpEmr.medDeptCd == "FM")
            {
                SQL = SQL + ComNum.VBLF + "	  		                      WHERE ACPNO = '" + AcpEmr.acpNo + "'  ";
                SQL = SQL + ComNum.VBLF + "	  		                      AND A1.FORMNO = 412  ";
                SQL = SQL + ComNum.VBLF + "	  		                      AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F  ";
                SQL = SQL + ComNum.VBLF + "	  		                                          WHERE F.FORMNO = A1.FORMNO))   ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
                SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
                SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
                SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
                SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
                SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
                SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
                SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
                SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
                SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000019079' AND ITEMVALUE IS NOT NULL) ";
                SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            }
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000019079')						 ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000000418') 			  		 ";//	-- 체중 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000000418' AND ITEMVALUE IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000000418')			 ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000000562') 			  	 ";//		-- 키 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000000562' AND ITEMVALUE IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000000562')		 ";
            SQL = SQL + ComNum.VBLF + "      WHEN (B.ITEMCD = 'I0000001807') 			 ";//  			-- BMI 
            SQL = SQL + ComNum.VBLF + "      THEN (    SELECT ITEMVALUE  ";
            SQL = SQL + ComNum.VBLF + "                FROM " + ComNum.DB_EMR + "AEMRCHARTMST M1 LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R1 ";
            SQL = SQL + ComNum.VBLF + "                ON M1.EMRNO = R1.EMRNO ";
            SQL = SQL + ComNum.VBLF + "                WHERE M1.EMRNO = (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1 ";
            SQL = SQL + ComNum.VBLF + "	  		                      WHERE A1.PTNO = '" + AcpEmr.ptNo + "' AND A1.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F ";
            SQL = SQL + ComNum.VBLF + "                                                      WHERE F.FORMNO = A1.FORMNO)  ";
            SQL = SQL + ComNum.VBLF + "                                  AND A1.CHARTTIME = ( SELECT MAX(M2.CHARTTIME) AS CHARTTIME  ";
            SQL = SQL + ComNum.VBLF + "						  				               FROM " + ComNum.DB_EMR + "AEMRCHARTMST M2, " + ComNum.DB_EMR + "AEMRCHARTROW R2  ";
            SQL = SQL + ComNum.VBLF + "                                                       WHERE M2.EMRNO = R2.EMRNO ";
            SQL = SQL + ComNum.VBLF + "											             AND M2.PTNO = '" + AcpEmr.ptNo + "' AND M2.CHARTDATE = '" + strChartDate + "' ";
            SQL = SQL + ComNum.VBLF + "                                                         AND M2.FORMNO = 412 ";
            SQL = SQL + ComNum.VBLF + "											             AND (R2.ITEMCD = 'I0000001807' AND ITEMVALUE IS NOT NULL) ";
            SQL = SQL + ComNum.VBLF + "                                                     ) ) ";
            SQL = SQL + ComNum.VBLF + "            AND ITEMCD = 'I0000001807')						 ";
            SQL = SQL + ComNum.VBLF + "      ELSE  B.ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + " END AS ITEMVALUE, ";
            SQL = SQL + ComNum.VBLF + " U.USENAME ";

            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO =  (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1";
            SQL = SQL + ComNum.VBLF + "                    WHERE A1.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                        AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F";
            SQL = SQL + ComNum.VBLF + "                                            WHERE F.FORMNO = A1.FORMNO) ";
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTTIME = ( SELECT MAX(A2.CHARTTIME) AS CHARTTIME FROM " + ComNum.DB_EMR + "AEMRCHARTMST A2";
            SQL = SQL + ComNum.VBLF + "                                            WHERE A2.PTNO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "                                                AND A2.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                                                AND A2.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                                            ) )";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Control[] txtVal = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true);
                if (txtVal != null)
                {
                    if (txtVal.Length > 0)
                    {
                        if (txtVal[0] is TextBox)
                        {
                            Control obj = (TextBox)txtVal[0];
                            ((TextBox)obj).Text = "";
                            ((TextBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                        else if (txtVal[0] is ComboBox)
                        {
                            Control obj = (ComboBox)txtVal[0];
                            ((ComboBox)obj).Text = "";
                            ((ComboBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                        else if (txtVal[0] is CheckBox)
                        {
                            Control obj = (CheckBox)txtVal[0];

                            if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                            {
                                ((CheckBox)obj).Checked = true;
                            }
                            else
                            {
                                ((CheckBox)obj).Checked = false;
                            }
                        }
                        else if (txtVal[0] is RadioButton)
                        {
                            Control obj = (RadioButton)txtVal[0];

                            if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                            {
                                ((RadioButton)obj).Checked = true;
                            }
                            else
                            {
                                ((RadioButton)obj).Checked = false;
                            }
                        }

                    }
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        public static void GetSetItemHis(PsmhDb pDbCon, Control frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            string strFormNo = VB.Split(strTag, ":")[1];

            if (AcpEmr == null) return;

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = SQL + ComNum.VBLF + " SELECT A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, A.PRNTYN, ";
            SQL = SQL + ComNum.VBLF + "                B. ITEMCD, B.ITEMNO, B.ITEMINDEX, B.ITEMTYPE, B.ITEMVALUE,";
            SQL = SQL + ComNum.VBLF + "                U.USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "    ON  B.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO =  (SELECT MAX(A1.EMRNO) AS EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTMST A1";
            SQL = SQL + ComNum.VBLF + "                    WHERE A1.ACPNO = " + VB.Val(AcpEmr.acpNo);
            SQL = SQL + ComNum.VBLF + "                        AND A1.CHARTDATE = '" + strChartDate + "'";
            SQL = SQL + ComNum.VBLF + "                        AND A1.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "                        AND A1.UPDATENO = ( SELECT MAX(F.UPDATENO) AS UPDATENO FROM " + ComNum.DB_EMR + "AEMRFORM F";
            SQL = SQL + ComNum.VBLF + "                                            WHERE F.FORMNO = A1.FORMNO) )";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                Control[] controls = null;
                controls = ComFunc.GetAllControls(frm);
                foreach (Control objControl in controls)
                {
                    Control[] txtVal = frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true);
                    if (txtVal != null)
                    {
                        if (txtVal.Length > 0)
                        {
                            if (txtVal[0] is TextBox)
                            {
                                Control obj = (TextBox)txtVal[0];
                                ((TextBox)obj).Text = "";
                                ((TextBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            }
                            else if (txtVal[0] is CheckBox)
                            {
                                Control obj = (CheckBox)txtVal[0];
                                ((CheckBox)obj).Checked = (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1" ? true : false);
                            }
                            else if (txtVal[0] is RadioButton)
                            {
                                Control obj = (RadioButton)txtVal[0];
                                ((RadioButton)obj).Checked = (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1" ? true : false);
                            }
                            else if (txtVal[0] is ComboBox)
                            {
                                Control obj = (ComboBox)txtVal[0];
                                ((ComboBox)obj).Text = "";
                                ((ComboBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                            }
                        }
                    }
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }
             

        //함수:값:해당컨트롤1,해당컨트롤1,
        //PanChkCleanCheck:p1,p2
        private static void PanChkCleanCheck(Form frm, string strTag)
        {
            string[] strPenel = VB.Split(VB.Split(strTag, ":")[1], ",");

            for (int i = 0; i < strPenel.Length; i++)
            {
                Control[]  penel = frm.Controls.Find(strPenel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        foreach (Control objControl in ComFunc.GetAllControls(penel[0]))
                        {
                            if (objControl is CheckBox)
                            {
                                ((CheckBox)objControl).Checked = false;
                            }
                            else if ((objControl is TextBox))
                            {
                                ((TextBox)objControl).Text = "";
                            }
                        }
                    }
                }
            }
        }

        private static void PanChkEndoPartCheck(Form frm, Control ct, string strTag)
        {
            Control[] controls;

            controls = frm.Controls.Find("I0000035144", true);     //퇴실시 환자교육 - 식사/안정
            if (controls != null)
            {
                (controls[0] as CheckBox).Checked = true;
            }

            controls = frm.Controls.Find("I0000035147", true);     //퇴실시 환자교육 - 낙상예방
            if (controls != null)
            {
                (controls[0] as CheckBox).Checked = true;
            }

            controls = frm.Controls.Find("I0000035270", true);     //퇴실시 환자교육 - 검사 후 관리 리플렛 제공
            if (controls != null)
            {
                (controls[0] as CheckBox).Checked = true;
            }
        }

        //함수:값:True Or False,해당컨트롤1
        //PanChkAllCheck:True:panel1
        private static void PanChkAllCheck(Control frm, Control ct, string strTag)
        {
            //bool blnValue = (ct is CheckBox ? ((CheckBox)ct).Checked : ((RadioButton)ct).Checked);
            bool blnValue = strTag.Split(':')[1].ToUpper().Equals("TRUE");
            string[] strPanel = strTag.Split(':')[2].Split(',');

            for (int i = 0; i < strPanel.Length; i++)
            {
                Control[] penel = frm.Controls.Find(strPanel[i].ToString(), true);
                if (penel.Length > 0)
                {
                    foreach (Control objControl in ComFunc.GetAllControls(penel[0]))
                    {
                        if (objControl is CheckBox)
                        {
                            ((CheckBox)objControl).Checked = blnValue;
                        }
                    }
                }
            }
        }

        private static void PanVisibleCheckBox(Form frm, Control ct, string strTag)
        {
            bool blnValue = ((CheckBox)ct).Checked;
            string[] strPanel = strTag.Split(':')[1].Split(',');

            for (int i = 0; i < strPanel.Length; i++)
            {
                Control[] penel = frm.Controls.Find(strPanel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        Control obj = (mtsPanel15.mPanel)penel[0];
                        obj.Visible = blnValue;
                        foreach (Control objControl in ComFunc.GetAllControls(obj))
                        {
                            if (objControl is CheckBox)
                            {
                                ((CheckBox)objControl).Checked = blnValue;
                            }
                            else if (objControl is RadioButton)
                            {
                                if (blnValue == false)
                                {
                                    ((RadioButton)objControl).Checked = blnValue;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 이전차트를 조회해서 세팅을 한다.
        /// </summary>
        public static void GetChartHis(PsmhDb pDbCon, Control frm, Control ct, string strTag, EmrPatient AcpEmr, string strChartDate)
        {
            //폼을 띄우고
            string strPTNO = AcpEmr.ptNo;
            string strACPNO = AcpEmr.acpNo;
            string strFORMNO = (VB.Split(strTag, ":")[2]).ToString();
            string strFORMNAME = (VB.Split(strTag, ":")[1]).ToString();
            string strCHARTDATE = strChartDate;

            frmEmrChartHisList frmEmrChartHisListX = new frmEmrChartHisList(frm, strPTNO, strACPNO, strFORMNO, strFORMNAME, strCHARTDATE);
            frmEmrChartHisListX.TopMost = true;
            frmEmrChartHisListX.ShowDialog(frm);
        }

       
        //함수:해당컨트롤1,해당컨트롤1,
        //PanVisibleChk:p1,p2
        public static void PanVisibleChk(Control frm, Control ct, string strTag)
        {
            bool blnValue = ((CheckBox)ct).Checked;
            string[] strPenel = VB.Split(VB.Split(strTag, ":")[1], ",");

            for (int i = 0; i < strPenel.Length; i++)
            {
                Control[] penel = null;
                Control obj = null;

                penel = frm.Controls.Find(strPenel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)penel[0];

                        if (blnValue == false)
                        {
                            Control[] controls = null;

                            controls = ComFunc.GetAllControls(obj);
                            foreach (Control objControl in controls)
                            {
                                if (objControl is CheckBox)
                                {
                                    ((CheckBox)objControl).Checked = false;
                                }
                                else if ((objControl is TextBox))
                                {
                                    ((TextBox)objControl).Text = "";
                                }
                                else if ((objControl is RadioButton))
                                {
                                    ((RadioButton)objControl).Checked = false;
                                }
                            }
                        }

                        obj.Visible = blnValue;
                    }
                }
            }
        }

        //함수:값:해당컨트롤1,해당컨트롤1,
        //PanelVisible:true:p1,p2
        public static void PanVisible(Control frm, string strTag)
        {
            string strValue = VB.Split(strTag, ":")[1];
            string[] strPenel = VB.Split(VB.Split(strTag, ":")[2], ",");

            for (int i = 0; i < strPenel.Length; i++)
            {
                Control[] penel = null;
                Control obj = null;

                penel = frm.Controls.Find(strPenel[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)penel[0];
                        if (strValue == "false")
                        {
                            Control[] controls = null;
                            obj.Visible = false;
                            controls = ComFunc.GetAllControls(obj);
                            foreach (Control objControl in controls)
                            {
                                if (objControl is CheckBox)
                                {
                                    ((CheckBox)objControl).Checked = false;
                                }

                                if (objControl is RadioButton)
                                {
                                    ((RadioButton)objControl).Checked = false;
                                }
                            }
                            obj.Visible = false;
                        }
                        else
                        {
                            obj.Visible = true;
                        }
                    }
                }
            }
        }

        //함수:값:해당컨트롤1,해당컨트롤1,
        //CheckBoxCheck:true:p1,p2
        public static void CheckBoxCheck(Control frm, string strTag)
        {
            string strValue = VB.Split(strTag, ":")[1];
            string[] strRadio = VB.Split(VB.Split(strTag, ":")[2], ",");

            for (int i = 0; i < strRadio.Length; i++)
            {
                Control[] ContolX = null;
                Control obj = null;

                ContolX = frm.Controls.Find(strRadio[i].ToString(), true);
                if (ContolX != null)
                {
                    if (ContolX.Length > 0)
                    {
                        obj = (CheckBox)ContolX[0];
                        if (strValue == "false")
                        {
                            ((CheckBox)obj).Checked = false;
                        }
                        else
                        {
                            ((CheckBox)obj).Checked = true;
                        }
                    }
                }
            }
        }

        //함수:값:해당컨트롤1,해당컨트롤1,
        //RadioCheck:true:p1,p2
        public static void RadioCheck(Control frm, string strTag)
        {
            string strValue = VB.Split(strTag, ":")[1];
            string[] strContol = VB.Split(VB.Split(strTag, ":")[2], ",");

            for (int i = 0; i < strContol.Length; i++)
            {
                Control[] ContolX = null;
                Control obj = null;

                ContolX = frm.Controls.Find(strContol[i].ToString(), true);
                if (ContolX != null)
                {
                    if (ContolX.Length > 0)
                    {
                        obj = (RadioButton)ContolX[0];
                        if (strValue == "false")
                        {
                            ((RadioButton)obj).Checked = false;
                        }
                        else
                        {
                            ((RadioButton)obj).Checked = true;
                        }
                    }
                }
            }
        }

        //함수:true,false,
        //PanVisibleTrueFalse:p1:p2
        public static void PanVisibleTrueFalse(Control frm, string strTag)
        {
            string[] strTrue = VB.Split(VB.Split(strTag, ":")[1], ",");
            string[] strFalse = VB.Split(VB.Split(strTag, ":")[2], ",");

            for (int i = 0; i < strTrue.Length; i++)
            {
                Control[] penel = null;
                Control obj = null;

                penel = frm.Controls.Find(strTrue[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)penel[0];
                        obj.Visible = true;
                    }
                }
            }
            for (int i = 0; i < strFalse.Length; i++)
            {
                Control[] penel = null;
                Control obj = null;

                penel = frm.Controls.Find(strFalse[i].ToString(), true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (mtsPanel15.mPanel)penel[0];
                        obj.Visible = false;

                        foreach (Control control in ComFunc.GetAllControls(obj))
                        {
                            if (control is RadioButton)
                            {
                                ((RadioButton)control).Checked = false;
                            }

                            if (control is TextBox)
                            {
                                ((TextBox)control).Text = "";
                            }

                            if (control is CheckBox)
                            {
                                ((CheckBox)control).Checked = false;
                            }
                        }
                    }
                }
            }
        }


        public static bool CheckEmrViewer()
        {
            bool rtnVal = false;
            return rtnVal;
            //try
            //{
            //    string strExePath = clsEmrType.EmrSvrInfo.EmrClient + "\\" + "bitIndexing.exe";
            //    //파일이 존재하는지 확인
            //    if (File.Exists(strExePath) == false)
            //    {
            //        clsWinScp.ConWinScp("Ftp", clsType.SvrInfo.strServerIp, clsType.SvrInfo.strUser, clsType.SvrInfo.strPasswd, clsType.SvrInfo.strSvrHomePath, "");
            //        clsWinScp.gTrsResult = clsWinScp.gWinScp.GetFiles(clsType.SvrInfo.strServerPath + "/exenet/bitIndexing.exe", clsEmrType.EmrSvrInfo.EmrClient + "\\exenet\\" + "bitIndexing.exe", false, clsWinScp.gTrsOptions);
            //    }

            //    //실행중인지 확인
            //    bool boolRunning = false;

            //    System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("bitIndexing");
            //    if (ProcessEx.Length > 0)
            //    {
            //        System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("bitIndexing");
            //        System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
            //        foreach (System.Diagnostics.Process Proc in Pro1)
            //        {
            //            if (Proc.Id != CurPro.Id)
            //            {
            //                boolRunning = true;
            //                break;
            //            }
            //        }
            //    }
            //    if (boolRunning == false)
            //    {
            //        // 업데이트 프로그램 백그라운드에서 실행을 한다.
            //        System.Diagnostics.Process program = System.Diagnostics.Process.Start(clsEmrType.EmrSvrInfo.EmrClient + "\\exenet\\" + "bitIndexing.exe", "");
            //        System.Threading.Thread.Sleep(100);
            //    }
            //    return rtnVal;
            //}
            //catch
            //{
            //    return rtnVal;
            //}
        }

        public static bool CheckEmrViewerEx()
        {
            bool rtnVal = false;
            return rtnVal;
            //try
            //{
            //    string strExePath = clsEmrType.EmrSvrInfo.EmrClient + "\\exenet\\" + "bitIndexing.exe";
            //    //파일이 존재하는지 확인
            //    if (File.Exists(strExePath) == false)
            //    {
            //        clsWinScp.ConWinScp("Ftp", clsType.SvrInfo.strServerIp, clsType.SvrInfo.strUser, clsType.SvrInfo.strPasswd, clsType.SvrInfo.strSvrHomePath, "");
            //        clsWinScp.gTrsResult = clsWinScp.gWinScp.GetFiles(clsType.SvrInfo.strServerPath + "/exenet/bitIndexing.exe", clsEmrType.EmrSvrInfo.EmrClient + "\\exenet\\" + "bitIndexing.exe", false, clsWinScp.gTrsOptions);
            //    }

            //    //실행중인지 확인
            //    bool boolRunning = false;

            //    System.Diagnostics.Process[] ProcessEx = System.Diagnostics.Process.GetProcessesByName("bitIndexing");
            //    if (ProcessEx.Length > 0)
            //    {
            //        System.Diagnostics.Process[] Pro1 = System.Diagnostics.Process.GetProcessesByName("bitIndexing");
            //        System.Diagnostics.Process CurPro = System.Diagnostics.Process.GetCurrentProcess();
            //        foreach (System.Diagnostics.Process Proc in Pro1)
            //        {
            //            if (Proc.Id != CurPro.Id)
            //            {
            //                boolRunning = true;
            //                Proc.Kill();
            //                System.Threading.Thread.Sleep(100);
            //                Application.DoEvents();
            //                break;
            //            }
            //        }
            //    }

            //    //bit 프로그램일수 있음으로 죽이고 다시 실행을 한다.
            //    // 업데이트 프로그램 백그라운드에서 실행을 한다.
            //    System.Diagnostics.Process program = System.Diagnostics.Process.Start(clsEmrType.EmrSvrInfo.EmrClient + "\\exenet\\" + "bitIndexing.exe", "");
            //    //for (int i = 0; i < 100; i++)
            //    //{
            //    //    Application.DoEvents();
            //    //}
            //    return rtnVal;
            //}
            //catch
            //{
            //    return rtnVal;
            //}
        }

        /// <summary>
        /// 바이탈 사인이 있으면 그룹을 열어 준다.
        /// </summary>
        /// <param name="pVital"></param>
        /// <param name="rY"></param>
        /// <param name="rN"></param>
        public static void IsExistsVital(Control pVital, RadioButton rY, RadioButton rN)
        {
            bool isVaule = false;

            Control[] controls = ComFunc.GetAllControls(pVital);

            foreach (Control control in controls)
            {
                if (control is TextBox)
                {
                    if (((TextBox)control).Text.Trim() != "")
                    {
                        isVaule = true;
                        break;
                    }
                }
                else if (control is ComboBox)
                {
                    if (((ComboBox)control).Text.Trim() != "")
                    {
                        isVaule = true;
                        break;
                    }
                }
            }

            if (isVaule == true)
            {
                rY.Checked = true;
            }
            else
            {
                rN.Checked = true;
            }
        }

        // 작성자, 작성일 , 작성시간을 불러옴. 코드 용어관리에서 작성자, 작성일 , 작성시간의 아이템 코드를 적확히 검색해서 TextBox를 만들어 두어야함.
        public static void SignSet(PsmhDb pDbCon, string strEMRNO, Form mFrm, string strCHARTDATE, string strCHARTTIME, string strCHARUSENAME, string strCOMPUSENAME = "")
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT A.CHARTDATE, A.CHARTTIME, A.CHARTUSEID, U.USENAME, UC.USENAME AS COMPUSENAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "    ON  U.USEID = A.CHARTUSEID";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AVIEWEMRUSER UC";
            SQL = SQL + ComNum.VBLF + "    ON  UC.USEID = A.COMPUSEID";
            SQL = SQL + ComNum.VBLF + "WHERE A.EMRNO =" + strEMRNO;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            //작성자 이름
            if (strCHARUSENAME != "")
            {
                Control[] txt_USENAME = mFrm.Controls.Find(strCHARUSENAME, true);
                Control obj_USENAME = null;
                if (txt_USENAME != null)
                {
                    if (txt_USENAME.Length > 0)
                    {
                        obj_USENAME = (TextBox)txt_USENAME[0];
                        obj_USENAME.Text = dt.Rows[0]["USENAME"].ToString().Trim();

                    }
                }
            }

            //확인자 이름
            if (strCOMPUSENAME != "")
            {
                Control[] txt_COMPUSENAME = mFrm.Controls.Find(strCOMPUSENAME, true);
                Control obj_COMPUSENAME = null;
                if (txt_COMPUSENAME != null)
                {
                    if (txt_COMPUSENAME.Length > 0)
                    {
                        obj_COMPUSENAME = (TextBox)txt_COMPUSENAME[0];
                        obj_COMPUSENAME.Text = dt.Rows[0]["COMPUSENAME"].ToString().Trim();

                    }
                }
            }

            //작성일
            if (strCHARTDATE != "")
            {
                Control[] txt_CHARTDATE = mFrm.Controls.Find(strCHARTDATE, true);
                Control obj_CHARTDATE = null;
                if (txt_CHARTDATE != null)
                {
                    if (txt_CHARTDATE.Length > 0)
                    {
                        obj_CHARTDATE = (TextBox)txt_CHARTDATE[0];
                        obj_CHARTDATE.Text = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                    }
                }
            }

            //작성시간
            if (strCHARTTIME != "")
            {
                Control[] txt_CHARTTIME = mFrm.Controls.Find(strCHARTTIME, true);
                Control obj_CHARTTIME = null;
                if (txt_CHARTTIME != null)
                {
                    if (txt_CHARTTIME.Length > 0)
                    {
                        obj_CHARTTIME = (TextBox)txt_CHARTTIME[0];
                        obj_CHARTTIME.Text = dt.Rows[0]["CHARTTIME"].ToString().Trim();

                    }
                }
            }


            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        /// <summary>
        /// 서식지에 일반정보를 세팅한다.
        /// </summary>
        /// <param name="p">환자 정보를 넣어준다.</param>
        /// /// <param name="frm">셋팅할 폼을 정한다.</param>
        /// <param name="strAddr">주소를 셋팅할 텍스트박스의 아이템 이름을 넣는다. </param>
        /// <param name="strPonNum">전화번호를 셋팅할 텍스트박스의 아이템 이름을 넣는다.</param>
        public static void GeneralItemSet(EmrPatient p, Form frm, string strAddr, string strTelNum)
        {
            //주소
            if (strAddr != "")
            {
                Control[] txt_Addr = frm.Controls.Find(strAddr, true);
                Control obj_Addr = null;
                if (txt_Addr != null)
                {
                    if (txt_Addr.Length > 0)
                    {
                        obj_Addr = (TextBox)txt_Addr[0];
                        obj_Addr.Text = p.address;

                    }
                }
            }

            //전화번호
            if (strTelNum != "")
            {
                Control[] txt_TelNum = frm.Controls.Find(strTelNum, true);
                Control obj_TelNum = null;
                if (txt_TelNum != null)
                {
                    if (txt_TelNum.Length > 0)
                    {
                        obj_TelNum = (TextBox)txt_TelNum[0];
                        obj_TelNum.Text = (p.tel + ComNum.VBLF + p.celphno).Trim();

                    }
                }
            }

        }

        // 패널 안에 있는 아이템에 값이 있으면 패널을 열어준다.
        public static void PanelVisibleTrue(Form frm)
        {
            Control[] controls = null;
            controls = ComFunc.GetAllControls(frm);
            foreach (Control Control in controls)
            {
                if (Control is mtsPanel15.mPanel)
                {
                    if (((mtsPanel15.mPanel)Control).Name == "IG00002_0" || ((mtsPanel15.mPanel)Control).Name == "IG00002")
                    {
                        continue;
                    }

                    if (((mtsPanel15.mPanel)Control).Visible == false)
                    {
                        string strTest = ((mtsPanel15.mPanel)Control).Name;

                        Control[] panControls = null;
                        panControls = ComFunc.GetAllControls(Control);
                        foreach (Control conItem in panControls)
                        {
                            if (conItem is TextBox)
                            {
                                if (((TextBox)conItem).Text != "")
                                {
                                    Control.Visible = true;
                                    break;
                                }
                            }
                            else if (conItem is RadioButton)
                            {
                                if (((RadioButton)conItem).Checked == true)
                                {
                                    Control.Visible = true;
                                    break;
                                }
                            }
                            else if (conItem is CheckBox)
                            {
                                if (((CheckBox)conItem).Checked == true)
                                {
                                    Control.Visible = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// EMR 상용구 옵션 설정
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="mControl"></param>
        /// <param name="strMacro"></param>
        public static void MacroSpace(PsmhDb pDbCon, Control mControl, string strMacro)
        {
            DataTable dt = null;
            string strText = "";
            string strSPACE = "0";

            dt = clsEmrQuery.GetEmrUserOption(pDbCon, clsType.User.IdNumber, "EMROPTION", "SPACE");
            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    strSPACE = Convert.ToString(VB.Val((dt.Rows[0]["OPTVALUE"].ToString() + "").Trim()));
                }
                dt.Dispose();
                dt = null;
            }

            strText = "";
            ((TextBox)mControl).Focus();
            if (strSPACE == "0")
            {
                strText = mControl.Text;
                mControl.Text = "";
                ((TextBox)mControl).AppendText(strText + " " + strMacro);
            }
            else
            {
                if (((TextBox)mControl).Multiline == true)
                {
                    strText = mControl.Text;
                    mControl.Text = "";
                    ((TextBox)mControl).AppendText(strText + strMacro + ComNum.VBLF);
                }
                else
                {
                    strText = mControl.Text;
                    mControl.Text = "";
                    ((TextBox)mControl).AppendText(strText + strMacro);
                }
            }
        }

        public static int GrpnoToInt(string strGRPNO)
        {
            int rtnVal = 0;

            switch (strGRPNO)
            {
                case "1002":
                    rtnVal = 1;
                    break;
                case "1003":
                    rtnVal = 2;
                    break;
                case "1004":
                    rtnVal = 3;
                    break;
                case "1009":
                    rtnVal = 4;
                    break;
            }
            return rtnVal;
        }

        public static string intToGrpno(int intGRPNO)
        {
            string rtnVal = "0";

            switch (intGRPNO)
            {
                case 1:
                    rtnVal = "1002";
                    break;
                case 2:
                    rtnVal = "1003";
                    break;
                case 3:
                    rtnVal = "1004";
                    break;
                case 4:
                    rtnVal = "1009";
                    break;
            }
            return rtnVal;
        }

        public static void PanelHeight(Form frm, string strPanel, string strItem, int intItemIndex)
        {
            int i = 0;
            int intHeight = 0;

            Control[] mPanel = null;
            Control[] Item = null;

            mPanel = frm.Controls.Find(strPanel, true);

            if (mPanel[0] != null)
            {
                if (mPanel.Length > 0)
                {
                    if (mPanel[0] is mtsPanel15.mPanel)
                    {
                        for (i = 0; i <= intItemIndex; i++)
                        {
                            Item = mPanel[0].Controls.Find(strItem + "_" + Convert.ToString(i), true);
                            if (Item != null)
                            {
                                if (Item.Length > 0)
                                {
                                    if (Item[0] is TextBox)
                                    {
                                        if (((TextBox)Item[0]).Text != "")
                                        {
                                            intHeight = ((TextBox)Item[0]).Location.Y + ((TextBox)Item[0]).Size.Height;
                                            mPanel[0].Height = intHeight;
                                            Control[] panControls = null;

                                            panControls = ComFunc.GetAllControls(mPanel[0]);

                                            foreach (Control conItem in panControls)
                                            {
                                                if (conItem is TextBox)
                                                {
                                                    if (((TextBox)conItem).Text != "")
                                                    {
                                                        conItem.Visible = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 퇴원요약지 확인자 및 요약자 세팅
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strEmrNo"></param>
        public static void SetCompInfoDisch(PsmhDb pDbCon, Form frm, string strEmrNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    C.COMPUSEID, C.COMPDATE, C.COMPTIME, Y.YOYAKUSEID, Y.YOYAKDATE, Y.YOYAKTIME, ";
            SQL = SQL + ComNum.VBLF + "    UC.USENAME AS COMPNAME, UY.USENAME AS YOYAKNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRDISCHYOYAK Y ";
            SQL = SQL + ComNum.VBLF + "    ON Y.EMRNO = C.EMRNO ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER UC ";
            SQL = SQL + ComNum.VBLF + "    ON UC.USEID = C.COMPUSEID ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER UY ";
            SQL = SQL + ComNum.VBLF + "    ON UY.USEID = Y.YOYAKUSEID ";
            SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO =  " + VB.Val(strEmrNo);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string strCOMPNAME = dt.Rows[0]["COMPNAME"].ToString().Trim();
            string strYOYAKNAME = dt.Rows[0]["YOYAKNAME"].ToString().Trim();
            dt.Dispose();
            dt = null;

            Control[] txt_COMPNAME1 = frm.Controls.Find("I0000030613", true);
            if (txt_COMPNAME1 != null)
            {
                if (txt_COMPNAME1.Length > 0)
                {
                    if (((TextBox)txt_COMPNAME1[0]).Text.Trim() == "")
                    {
                        ((TextBox)txt_COMPNAME1[0]).Text = strCOMPNAME;
                    }
                }
            }

            Control[] txt_COMPNAME2 = frm.Controls.Find("I0000030833", true);
            if (txt_COMPNAME2 != null)
            {
                if (txt_COMPNAME2.Length > 0)
                {
                    if (((TextBox)txt_COMPNAME2[0]).Text.Trim() == "")
                    {
                        ((TextBox)txt_COMPNAME2[0]).Text = strCOMPNAME;
                    }
                }
            }

            Control[] txt_YOYAKNAME = frm.Controls.Find("I0000030929", true);
            if (txt_YOYAKNAME != null)
            {
                if (txt_YOYAKNAME.Length > 0)
                {
                    if (((TextBox)txt_YOYAKNAME[0]).Text.Trim() == "")
                    {
                        ((TextBox)txt_YOYAKNAME[0]).Text = strYOYAKNAME;
                    }
                }
            }
        }

        /// <summary>
        /// 수술기록지 확인자 세팅
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strEmrNo"></param>
        public static void SetCompInfoOpRecord(PsmhDb pDbCon, Form frm, string strEmrNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT  ";
            SQL = SQL + ComNum.VBLF + "    C.COMPUSEID, C.COMPDATE, C.COMPTIME, ";
            SQL = SQL + ComNum.VBLF + "    UC.USENAME AS COMPNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER UC ";
            SQL = SQL + ComNum.VBLF + "    ON UC.USEID = C.COMPUSEID ";
            SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO =  " + VB.Val(strEmrNo);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string strCOMPNAME = dt.Rows[0]["COMPNAME"].ToString().Trim();
            dt.Dispose();
            dt = null;

            Control[] txt_COMPNAME1 = frm.Controls.Find("I0000030917", true);
            if (txt_COMPNAME1 != null)
            {
                if (txt_COMPNAME1.Length > 0)
                {
                    ((TextBox)txt_COMPNAME1[0]).Text = strCOMPNAME;
                }
            }

        }

        /// <summary>
        /// 마취 방법 약자->풀네임 변경
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="strEmrNo"></param>
        public static string GetANES(string strANES)
        {
            string strResult = "";

            switch (strANES)
            {
                case "G":
                    strResult = "General Anesthesia";
                    break;
                case "S":
                    strResult = "Spinal Anesthesia";
                    break;
                case "E":
                    strResult = "Epidural Anesthesia";
                    break;
                case "B":
                    strResult = "Brachial Plexus Block";
                    break;
                case "L":
                    strResult = "Local Anesthesia";
                    break;
                case "C":
                    strResult = "Caudal Block";
                    break;
            }

            return strResult;
        }

        // 수술기록지 Drainage 종류 콤보박스 셋팅.
        public static void DrainageCboSet(PsmhDb pDbCon, ComboBox cboItem)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            cboItem.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT TITLE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO = 562";
            SQL = SQL + ComNum.VBLF + "    AND CONTROLID = 'I0000030799_1'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            cboItem.Items.Add("");
            for (i = 0; i < dt.Rows.Count; i++)
            {
                cboItem.Items.Add(dt.Rows[i]["TITLE"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }


        public static bool GetBriefOp(PsmhDb pDbCon, EmrPatient p, string strEndDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            bool bolreturn = false;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT CHARTDATE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "    AND FORMNO = 498";
            SQL = SQL + ComNum.VBLF + "    AND CHARTDATE >= " + p.medFrDate + "";

            if (p.medEndDate == "")
            {
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE <= " + strEndDate;
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "    AND CHARTDATE <= " + p.medEndDate + "";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                bolreturn = false;
            }
            else if (dt.Rows.Count == 0)
            {
                bolreturn = false;

            }
            else
            {
                bolreturn = true;
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
            return bolreturn;
        }

        /// <summary>
        /// 수술 기록지에 기록된 값을 로드할 때 수술 중 특이사항 값이 있으면 유를 첵크 한다.
        /// </summary>
        /// <param name="I0000030577">수술 중 특이사항 TextBox</param>
        /// <param name="optTrue">수술 중 특이사항의 유 radiobutton</param>
        public static void I0000030577Check(TextBox I0000030577, RadioButton optTrue)
        {
            if (I0000030577.Text != "")
            {
                optTrue.Checked = true;
            }
        }

        public static bool ErNewFormCheck(PsmhDb pDbCon, EmrPatient p)
        {
            bool ErNewFormCheck = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT EMRNO";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = '454'";
            SQL = SQL + ComNum.VBLF + "    AND PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.ACPNO = '" + p.acpNo + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return ErNewFormCheck; ;
            }

            if (dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("응급환자분류표 기록이 없습니다." + ComNum.VBLF + "응급환자분류표를 먼저 작성해 주세요!");
            }
            else
            {
                ErNewFormCheck = true;
            }
            dt.Dispose();
            dt = null;
            return ErNewFormCheck;
        }

        //심사실 
        public static void ForSimsaUser(PsmhDb pDbCon, Control frm)
        {
            ForSimsaUserFlow(pDbCon, frm);
        }

        public static void ForSimsaUserFlow(PsmhDb pDbCon, Control frm)
        {
            //심사실 경우 FlowSheet 순정렬, 작성창 없애기
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT BASCD ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '권한관리' ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = 'TOP' ";
            SQL = SQL + ComNum.VBLF + "    AND BASEXNAME = '보험심사팀' ";
            SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + clsType.User.IdNumber + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                Control[] tx = null;
                Control obj = null;
                tx = frm.Controls.Find("panWrite", true);
                if (tx.Length > 0)
                {
                    obj = (Panel)tx[0];
                    ((Panel)obj).Visible = false;
                }

                tx = null;
                obj = null;
                tx = frm.Controls.Find("chkDesc", true);
                if (tx.Length > 0)
                {
                    obj = (CheckBox)tx[0];
                    ((CheckBox)obj).Checked = false;
                }
            }

            dt.Dispose();
            dt = null;


        }


        public static void GetOPdate(PsmhDb pDbCon, TextBox conTxtSDate, TextBox conTxtEDate, EmrPatient Acp)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (Acp.opdate == "" || VB.IsDate(Acp.opdate) == false)
            {
                return;
            }

            SQL = "";
            SQL = "SELECT B.ITEMCD,B.ITEMVALUE       ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A       ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B     ";
            SQL = SQL + ComNum.VBLF + "    ON A.EMRNO = B.EMRNO        ";
            SQL = SQL + ComNum.VBLF + "    AND B.ITEMCD IN ('I0000003210','I0000003213')       ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + Acp.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = '562'        ";
            SQL = SQL + ComNum.VBLF + "    AND A.OPDATE = TO_DATE('" + Acp.opdate + "','YYYY-MM-DD')       ";
            SQL = SQL + ComNum.VBLF + "    AND A.OPDEGREE = " + Acp.opdegree + "      ";
            SQL = SQL + ComNum.VBLF + "    AND A.OP_DEPT = '" + Acp.opdept + "'        ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000003210")
                {
                    conTxtSDate.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }
                else if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000003213")
                {
                    conTxtEDate.Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        private static bool CheckRestrictTerm(PsmhDb pDbCon, Form mForm)
        {
            bool rtnVal = true;
            int i = 0;
            int j = 0;
            int intCheck = 0;
            //컨트롤 ID : 금기약어
            string[] TermCode;
            string[] TermCodeNm;
            string[] TermChkItem;
            string[] TermChkItemRmk;

            TermCode = new string[0];
            TermCodeNm = new string[0];
            TermChkItem = new string[0];
            TermChkItemRmk = new string[0];

            Control[] controls = null;
            controls = ComFunc.GetAllControls(mForm);

            foreach (Control objControl in controls)
            {
                if (objControl is TextBox)
                {
                    if (((TextBox)objControl).Text.Trim() == "") continue;

                    // 초진기록지 당일상병 제외
                    if ((((TextBox)objControl).Name == "I0000028647") || (((TextBox)objControl).Name == "I0000014390")) continue;
                    // 재진기록지 당일상병 제외
                    if ((((TextBox)objControl).Name == "I0000011772") || (((TextBox)objControl).Name == "I0000030015")) continue;


                    string strItemCdoe = objControl.Name.Trim();
                    string strItemText = ((TextBox)objControl).Text.Trim();
                    //단어 단위로 짤라서 배열에 담아서 "."첫번째에 있을 경우 금기약어에 담는다.
                    string[] arryLine = VB.Split(strItemText, "\r\n"); //줄단위로 담기

                    for (i = 0; i < arryLine.Length; i++)
                    {
                        string[] arryText = VB.Split(arryLine[i].ToString().Trim(), " "); //단어 단위로 담기
                        for (j = 0; j < arryText.Length; j++)
                        {
                            if (arryText[j].ToString().Trim() == "q6pm") //q6pm : every 6 hours 잘못 해석
                            {
                                Array.Resize<string>(ref TermCode, TermCode.Length + 1);
                                Array.Resize<string>(ref TermCodeNm, TermCodeNm.Length + 1);
                                Array.Resize<string>(ref TermChkItem, TermChkItem.Length + 1);
                                Array.Resize<string>(ref TermChkItemRmk, TermChkItemRmk.Length + 1);
                                TermCode[TermCode.Length - 1] = strItemCdoe;
                                TermCodeNm[TermCodeNm.Length - 1] = "";
                                TermChkItem[TermChkItem.Length - 1] = "q6pm";
                                TermChkItemRmk[TermChkItemRmk.Length - 1] = "every 6 hours 잘못 해석";
                                intCheck = intCheck + 1;
                            }
                            else if (arryText[j].ToString().Trim() == "qd") // qd pm : 사용불가
                            {
                                if (j < arryText.Length - 1)
                                {
                                    if (arryText[j + 1].ToString().Trim() == "pm")
                                    {
                                        Array.Resize<string>(ref TermCode, TermCode.Length + 1);
                                        Array.Resize<string>(ref TermCodeNm, TermCodeNm.Length + 1);
                                        Array.Resize<string>(ref TermChkItem, TermChkItem.Length + 1);
                                        Array.Resize<string>(ref TermChkItemRmk, TermChkItemRmk.Length + 1);
                                        TermCode[TermCode.Length - 1] = strItemCdoe;
                                        TermCodeNm[TermCodeNm.Length - 1] = "";
                                        TermChkItem[TermChkItem.Length - 1] = "qd pm";
                                        TermChkItemRmk[TermChkItemRmk.Length - 1] = "사용불가";
                                        intCheck = intCheck + 1;
                                    }
                                }
                            }
                            else if (arryText[j].ToString().Trim() == "sub") // sub q : "q" 가 “every" 로 잘못 해석
                            {
                                if (j < arryText.Length - 1)
                                {
                                    if (arryText[j + 1].ToString().Trim() == "q")
                                    {
                                        Array.Resize<string>(ref TermCode, TermCode.Length + 1);
                                        Array.Resize<string>(ref TermCodeNm, TermCodeNm.Length + 1);
                                        Array.Resize<string>(ref TermChkItem, TermChkItem.Length + 1);
                                        Array.Resize<string>(ref TermChkItemRmk, TermChkItemRmk.Length + 1);
                                        TermCode[TermCode.Length - 1] = strItemCdoe;
                                        TermCodeNm[TermCodeNm.Length - 1] = "";
                                        TermChkItem[TermChkItem.Length - 1] = "sub q";
                                        TermChkItemRmk[TermChkItemRmk.Length - 1] = "q 가 every 로 잘못 해석";
                                        intCheck = intCheck + 1;
                                    }
                                }
                            }
                            else if (arryText[j].ToString().Trim() == "m") // m g : mg 로 잘못 해석
                            {
                                if (j < arryText.Length - 1)
                                {
                                    if (arryText[j + 1].ToString().Trim() == "g")
                                    {
                                        Array.Resize<string>(ref TermCode, TermCode.Length + 1);
                                        Array.Resize<string>(ref TermCodeNm, TermCodeNm.Length + 1);
                                        Array.Resize<string>(ref TermChkItem, TermChkItem.Length + 1);
                                        Array.Resize<string>(ref TermChkItemRmk, TermChkItemRmk.Length + 1);
                                        TermCode[TermCode.Length - 1] = strItemCdoe;
                                        TermCodeNm[TermCodeNm.Length - 1] = "";
                                        TermChkItem[TermChkItem.Length - 1] = "m g";
                                        TermChkItemRmk[TermChkItemRmk.Length - 1] = "mg 로 잘못 해석";
                                        intCheck = intCheck + 1;
                                    }
                                }
                            }
                            else if (VB.Left(arryText[j].ToString().Trim(), 1) == ".")  // .5 mg등 : 체크
                            {
                                if (arryText[j].Length >= 2)
                                {
                                    if (VB.IsNumeric(VB.Left(arryText[j].ToString().Trim(), 2)) == true)
                                    {
                                        Array.Resize<string>(ref TermCode, TermCode.Length + 1);
                                        Array.Resize<string>(ref TermCodeNm, TermCodeNm.Length + 1);
                                        Array.Resize<string>(ref TermChkItem, TermChkItem.Length + 1);
                                        Array.Resize<string>(ref TermChkItemRmk, TermChkItemRmk.Length + 1);
                                        TermCode[TermCode.Length - 1] = strItemCdoe;
                                        TermCodeNm[TermCodeNm.Length - 1] = "";
                                        TermChkItem[TermChkItem.Length - 1] = ".5 mg등";
                                        TermChkItemRmk[TermChkItemRmk.Length - 1] = "5 mg 등으로 잘못 해석";
                                        intCheck = intCheck + 1;
                                    }
                                }
                                else
                                {
                                    //에러 : 5 mg 으로 잘못 해석
                                }
                            }

                        }
                    }
                }
            }

            if (intCheck == 0) return true;

            string strItem = "";

            for (i = 0; i < TermCode.Length; i++)
            {
                if (i == TermCode.Length - 1)
                {
                    strItem = strItem + "'" + TermCode[i].ToString().Trim() + "' ";
                }
                else
                {
                    strItem = strItem + "'" + TermCode[i].ToString().Trim() + "', ";
                }
            }

            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";

            SQL = " SELECT ITEMNO, ITEMNAME";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRITEM ";
            SQL = SQL + ComNum.VBLF + "    WHERE ITEMNO IN (" + strItem + ")";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return true;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return true;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                for (j = 0; j < TermCode.Length; j++)
                {
                    if (TermCode[j].Trim() == dt.Rows[i]["ITEMNO"].ToString().Trim())
                    {
                        TermCodeNm[j] = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    }
                }
            }
            dt.Dispose();
            dt = null;

            string strCheckText = "금기약어 내용입니다." + "\r\n " + "\r\n ";

            for (i = 0; i < TermCode.Length; i++)
            {
                if (i == TermCode.Length - 1)
                {
                    strCheckText = strCheckText + "아이템 : " + "(" + TermCodeNm[i].Trim() + ") 내용중  [" + TermChkItem[i].Trim() + "]는 " + TermChkItemRmk[i].Trim();
                }
                else
                {
                    strCheckText = strCheckText + "아이템 : " + "(" + TermCodeNm[i].Trim() + ") 내용중  [" + TermChkItem[i].Trim() + "]는 " + TermChkItemRmk[i].Trim() + "\r\n ";
                }
            }
            rtnVal = false;
            ComFunc.MsgBox(strCheckText);
            return rtnVal;
        }

        public static bool CheckFormMibi(PsmhDb pDbCon, string strFormNo, string strUpdateNo, Form mForm, string strGuBun)
        {
            bool rtnVal = true;
            string strMiBiTitle = "";
            string strGRPFORMNO = "";

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            // 2016-07-04 금지 약서 주석 
            if (CheckRestrictTerm(pDbCon, mForm) == false) return false;

            //return rtnVal;
            //기록지별 작성 권한을 체크한다
            //진료기록지, 간호 기록지 
            //일단 하드 코딩을 한다.
            //1000 : 초진기록지
            //1001 : 재진기록지
            //1002 : 입원기록지
            //1003 : 경과기록지
            //1007 : 수술요약지
            //1009 : 퇴원요약지
            //1010 : 전과기록지
            //1011 : 협의기록지
            SQL = "";
            SQL = "SELECT A.GRPFORMNO      ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM A       ";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "    AND A.UPDATENO = " + VB.Val(strUpdateNo);
            SQL = SQL + ComNum.VBLF + "    AND A.GRPFORMNO IN (1000, 1001, 1002, 1003, 1007, 1009, 1010, 1011 )";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                switch (strFormNo)
                {
                    case "454":
                    case "593":
                    case "636":
                    case "503":
                    case "453":
                    case "570": //금연 교육 표준 서식
                    case "575": //급성 뇌졸중 재활 표준서식
                    case "577": //한글판 수정바델지수 (K-MBI)
                    case "578": //Barthel Index(퇴원시)
                        //return rtnVal;
                        break;
                    default:
                        switch (VB.Left(clsType.User.IdNumber.ToUpper(), 1))
                        {
                            case "A":
                            case "C":
                            case "D":
                                break;
                            default:
                                ComFunc.MsgBox("의사만 작성 가능한 기록지 입니다."
                                    + ComNum.VBLF + "저장할 수 없습니다."
                                    + ComNum.VBLF + "사용자를 확인해 주십시요.");
                                rtnVal = false;
                                return rtnVal;
                                //break;
                        }
                        //return rtnVal;
                        break;
                }
            }
            else
            {
                dt.Dispose();
                dt = null;
            }

            Color InitColor = Color.White;
            Color MibiColor = Color.Red;

            // 2016-11-19 임시저장은 미비체크 제외
            if (strGuBun == "0") // 임시저장 보내기
            {
                return true;
            }

            // 2016-11-19 미비체크 예외폼 체크
            if (MiViException(pDbCon, strFormNo) == true)
            {
                return true;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            
            // 2010-11-14 미비체크 수정Ver
            //--------------------------------------------------------------------------------------------------
            SQL = "";
            SQL = "SELECT A.GRPFORMNO  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM A ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD C        ";
            SQL = SQL + ComNum.VBLF + "      ON A.GRPFORMNO = C.BASCD        ";
            SQL = SQL + ComNum.VBLF + "      AND C.BSNSCLS = '미비관리'        ";
            SQL = SQL + ComNum.VBLF + "      AND C.UNITCLS = '미비그룹'        ";
            SQL = SQL + ComNum.VBLF + "      AND C.MNGCLS = '0'       ";     // 실운영            
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "AND A.PROGFORMNAME = '" + VB.Split(VB.Split(mForm.ToString().Trim(), ".")[1], ",")[0] + "'       ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            strGRPFORMNO = dt.Rows[0]["GRPFORMNO"].ToString().Trim();

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL = SQL = "SELECT BASCD , BASNAME         ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD       ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '미비관리'       ";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '미비체크'       ";
            SQL = SQL + ComNum.VBLF + "    AND REMARK1 LIKE '%" + strGRPFORMNO + "%'       ";            // 실운영
            SQL = SQL + ComNum.VBLF + "    AND MNGCLS = '0'               ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            
            //미비를 체크한다.

            clsType.User.MibiChartFlag = "";

            if (VB.Left(dt.Rows[0]["BASCD"].ToString().Trim(), 8) != "panChart")
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (CheckMibiItem(mForm, dt.Rows[i]["BASCD"].ToString().Trim(), MibiColor) == false)
                    {
                        if (strGRPFORMNO == "1000" || strGRPFORMNO == "1001")
                        {
                            clsType.User.MibiChartFlag = clsType.User.MibiChartFlag + "[" + dt.Rows[i]["BASNAME"].ToString().Trim() + "] ";
                            rtnVal = false;
                        }
                        else
                        {
                            strMiBiTitle = strMiBiTitle + "[" + dt.Rows[i]["BASNAME"].ToString().Trim() + "] ";
                            rtnVal = false;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                rtnVal = CheckMibiItemAll(pDbCon, mForm, ref strMiBiTitle, MibiColor);
            }
            //2016-12-24 
            if (clsType.User.MibiChartFlag == "")
            {
                if (rtnVal == false) ComFunc.MsgBox(strMiBiTitle + ComNum.VBLF + "항목이 미작성 되었습니다. 작성후 다시 저장하십시요.", "미비체크");
            }
            return rtnVal;

        }

        //예외미비폼 걸러내기~~ 
        private static bool MiViException(PsmhDb pDbCon, string strFormNo)
        {
            bool bolReturn = false;
            string strDate = ComQuery.CurrentDateTime(pDbCon, "D");

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "SELECT BASCD      ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD        ";
            SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '미비관리'        ";
            SQL = SQL + ComNum.VBLF + "      AND UNITCLS = '미비예외폼관리'        ";
            SQL = SQL + ComNum.VBLF + "      AND BASCD = '" + strFormNo + "'        ";
            SQL = SQL + ComNum.VBLF + "      AND APLFRDATE <= '" + strDate + "'       ";
            SQL = SQL + ComNum.VBLF + "      AND APLENDDATE >= '" + strDate + "'       ";
            SQL = SQL + ComNum.VBLF + "      AND USECLS = '1'        ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return bolReturn;
            }

            if (dt.Rows.Count > 0)
            {
                bolReturn = true;
            }
            dt.Dispose();
            dt = null;

            return bolReturn;
        }

        //미비가 있는지 체크를 한다
        private static bool CheckMibiItemAll(PsmhDb pDbCon, Form mForm, ref string strMiBiTitle, Color pColor)
        {
            bool rtnVal = false;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Control[] tx = null;

            tx = mForm.Controls.Find("panChart", true);
            if (tx == null)
            {
                return rtnVal;
            }

            mtsPanel15.mPanel panChart = null;

            if (tx.Length > 0)
            {
                panChart = (mtsPanel15.mPanel)tx[0];
            }
            else
            {
                return rtnVal;
            }
            Control[] controlsGroup = null;
            controlsGroup = ComFunc.GetAllControls(panChart);

            foreach (Control control in controlsGroup)
            {
                if (control is mtsPanel15.mPanel)
                {
                    if (ExceptionPanel(VB.Left(((mtsPanel15.mPanel)control).Name, 7)) == true) continue;

                    if (((mtsPanel15.mPanel)control).Visible == true)
                    {
                        mtsPanel15.mPanel pGroup = null;

                        pGroup = ((mtsPanel15.mPanel)control);

                        if (pGroup.Visible == true)
                        {
                            bool CheckValue = false;

                            Control[] controls = null;

                            controls = ComFunc.GetAllControls(pGroup);
                            foreach (Control controlChk in controls)
                            {
                                if (controlChk is TextBox)
                                {
                                    if (((TextBox)controlChk).Text.Trim() != "")
                                    {
                                        if (((TextBox)controlChk).Text.Trim() != "."
                                            && ((TextBox)controlChk).Text.Trim() != ".."
                                            && ((TextBox)controlChk).Text.Trim() != "...")
                                        {
                                            CheckValue = true;
                                            break;
                                        }
                                    }
                                }
                                if (controlChk is ComboBox)
                                {
                                    if (((ComboBox)controlChk).Text.Trim() != "")
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                                else if (controlChk is CheckBox)
                                {
                                    if (((CheckBox)controlChk).Checked == true)
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                                else if (controlChk is RadioButton)
                                {
                                    if (((RadioButton)controlChk).Checked == true)
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                            }
                            if (CheckValue == false)
                            {
                                Cursor.Current = Cursors.WaitCursor;

                                SQL = "";
                                SQL = "SELECT BASNAME        ";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBASCD        ";
                                SQL = SQL + ComNum.VBLF + "WHERE BSNSCLS = '기록지관리'     ";
                                SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '아이템그룹'       ";
                                SQL = SQL + ComNum.VBLF + "    AND BASCD = '" + VB.Left(pGroup.Name, 7) + "'          ";

                                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                }
                                else
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        strMiBiTitle = strMiBiTitle + "[" + dt.Rows[0]["BASNAME"].ToString().Trim() + "] ";
                                        dt.Dispose();
                                        dt = null;
                                    }
                                    else
                                    {
                                        dt.Dispose();
                                        dt = null;

                                        SQL = "SELECT ITEMNAME       ";
                                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEM         ";
                                        SQL = SQL + ComNum.VBLF + "WHERE ITEMNO = '" + VB.Left(pGroup.Name, 11) + "'         ";

                                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                                        if (SqlErr != "")
                                        {
                                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                            Cursor.Current = Cursors.Default;
                                        }
                                        else
                                        {
                                            if (dt.Rows.Count > 0)
                                            {
                                                strMiBiTitle = strMiBiTitle + "[" + dt.Rows[0]["ITEMNAME"].ToString().Trim() + "] ";
                                            }
                                            dt.Dispose();
                                            dt = null;
                                        }
                                    }
                                    Cursor.Current = Cursors.Default;
                                }
                            }
                        }
                    }
                }
            }
            rtnVal = strMiBiTitle.Trim() == "" ? true : false;

            return rtnVal;
        }

        private static bool ExceptionPanel(string strPanelName)
        {
            bool rtnVal = false;

            switch (strPanelName)
            {
                case "IG00251": //Frequency of Operation
                case "IG00255": //Operative Death
                case "IG00252": //ControlText
                case "IG00073": //수술 및 처치명
                case "IG00249": //작성자 싸인
                    rtnVal = true;
                    break;
            }
            return rtnVal;
        }

        private static void SetMibiBackColor(Form mForm, string strConName, Color pColor)
        {
            Control[] tx = null;
            tx = mForm.Controls.Find(strConName, true);
            if (tx != null)
            {
                if (tx.Length > 0)
                {
                    if (tx[0] is Panel)
                    {
                        ((Panel)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is mtsPanel15.mPanel)
                    {
                        ((mtsPanel15.mPanel)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is GroupBox)
                    {
                        ((GroupBox)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is TextBox)
                    {
                        ((TextBox)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is CheckBox)
                    {
                        ((CheckBox)tx[0]).BackColor = pColor;
                    }
                    else if (tx[0] is ComboBox)
                    {
                        ((ComboBox)tx[0]).BackColor = pColor;
                    }
                }
            }
        }

        //미비가 있는지 체크를 한다
        private static bool CheckMibiItem(Form mForm, string strConName, Color pColor)
        {
            bool rtnVal = true;
            bool bolItmeException = false;

            Control[] tx = null;

            bolItmeException = ItmeException(strConName);

            tx = mForm.Controls.Find(strConName, true);
            if (tx == null)
            {
                return rtnVal;
            }

            mtsPanel15.mPanel pGroup = null;

            if (tx.Length > 0)
            {
                pGroup = (mtsPanel15.mPanel)tx[0];
            }
            else
            {
                return rtnVal;
            }

            if (pGroup.Visible == true)
            {
                bool CheckValue = false;

                Control[] controls = null;

                if (bolItmeException == false)
                {
                    controls = ComFunc.GetAllControls(pGroup);
                    foreach (Control control in controls)
                    {
                        if (control.Visible == false) continue;

                        if (control is TextBox)
                        {
                            if (((TextBox)control).Text.Trim() != "")
                            {
                                CheckValue = true;
                                break;
                            }
                        }
                        if (control is ComboBox)
                        {
                            if (((ComboBox)control).Text.Trim() != "")
                            {
                                CheckValue = true;
                                break;
                            }
                        }
                        else if (control is CheckBox)
                        {
                            if (((CheckBox)control).Checked == true)
                            {
                                CheckValue = true;
                                break;
                            }
                        }
                        else if (control is RadioButton)
                        {
                            if (((RadioButton)control).Checked == true)
                            {
                                CheckValue = true;
                                break;
                            }
                        }
                    }
                }
                else // 예외 처리 (무 일때는 넘어가고 유일때만 미비 체크함.)
                {
                    string strI0000002159 = "";
                    string strI0000001195 = "";

                    controls = ComFunc.GetAllControls(pGroup);

                    foreach (Control control in controls)
                    {
                        if (strI0000002159 == "" || strI0000001195 == "")
                        {
                            if (control is RadioButton)
                            {
                                switch (VB.Left(((RadioButton)control).Name, 11))
                                {
                                    case "I0000002159":  // 유
                                        strI0000002159 = ((RadioButton)control).Checked.ToString().Trim();
                                        break;
                                    case "I0000001195":  // 무
                                        strI0000001195 = ((RadioButton)control).Checked.ToString().Trim();
                                        break;
                                }
                            }
                        }
                    }

                    if (strI0000002159 == "True")
                    {
                        if (strConName == "IG00239") //출혈 정도 (하드코딩 기록실 파트장님이 시킴)
                        {
                            CheckValue = true;

                            foreach (Control control in controls)
                            {
                                if (control is TextBox)
                                {
                                    if (((TextBox)control).Text.Trim() == "")
                                    {
                                        CheckValue = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Control control in controls)
                            {
                                if (control is TextBox)
                                {
                                    if (((TextBox)control).Text.Trim() != "")
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                                if (control is ComboBox)
                                {
                                    if (((ComboBox)control).Text.Trim() != "")
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                                else if (control is CheckBox)
                                {
                                    if (((CheckBox)control).Checked == true)
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                                else if (control is RadioButton)
                                {
                                    switch (VB.Left(((RadioButton)control).Name, 11))
                                    {
                                        case "I0000002159":  // 유
                                            continue;
                                        case "I0000001195":  // 무
                                            continue;
                                    }
                                    if (((RadioButton)control).Checked == true)
                                    {
                                        CheckValue = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (strI0000001195 == "True")
                    {
                        CheckValue = true;
                    }
                    else if (strI0000002159 == "False" && strI0000001195 == "False")
                    {
                        CheckValue = false;
                    }
                }
                rtnVal = CheckValue;
            }
            return rtnVal;
        }

        private static bool ItmeException(string strConName)
        {
            bool bolReturn = false;

            switch (strConName)
            {
                case "IG00238": //수술 중 특이사항
                case "IG00239": //출혈 정도
                case "IG00240": //Specimen
                case "IG00241": //Drainage
                case "IG00246": //기타 특이사항
                    bolReturn = true;
                    break;
            }
            return bolReturn;
        }

        /// <summary>
        /// 전자 인증 번호가 없는 사람들은 임시 저장으로 함
        /// </summary>
        /// <param name="strFlag"></param>
        /// <param name="strUserId"></param>
        public static void EmrSaveFlag(PsmhDb pDbCon, ref string strFlag, string strUserId)
        {
            if (strFlag == "0")
            {
                return;
            }

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SERTI_EMRYN        ";
            SQL = SQL + ComNum.VBLF + "FROM MED_OCS.OPDIPD_PASSWORD_DMC        ";
            SQL = SQL + ComNum.VBLF + "WHERE SERTI_EMRYN = 'Y'     ";
            SQL = SQL + ComNum.VBLF + "    AND DR_CODE = '" + strUserId + "'      ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                strFlag = "0";
            }

            dt.Dispose();
            dt = null;

            return;
        }

        /// <summary>
        /// 기록지 수정 가능여부
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="panChartX"></param>
        /// <param name="EditYn"></param>
        public static void SetEditLockEx(Control frm, Panel panChartX, bool EditYn, mtsPanel15.TransparentPanel panEditLock)
        {
            if (EditYn == false)
            {
                if (panEditLock != null)
                {
                    panEditLock.Dispose();
                    panEditLock = null;
                }
            }
            else
            {
                if (panEditLock != null)
                {
                    panEditLock.Dispose();
                    panEditLock = null;
                }
            }
        }

        /// <summary>
        /// 차트 사이즈를 설정을 한다.
        /// </summary>
        /// <param name="panChartX"></param>
        /// <returns></returns>
        private static int CalcChartPanelHeight(Panel panChartX)
        {
            int rtnVal = 0;

            Control[] controls = null;
            controls = ComFunc.GetAllControls(panChartX);

            foreach (Control objControl in controls)
            {
                if (objControl is Panel)
                {
                    if (((Panel)objControl).Visible == true)
                    {
                        if (objControl.Parent == panChartX)
                        {
                            rtnVal = rtnVal + ((Panel)objControl).Height;
                        }
                    }
                }
            }
            return rtnVal;
        }


        /// <summary>
        /// 매핑한 기록지가 있는지 조회한다.
        /// </summary>
        /// <param name="strAcpNo"></param>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static bool MappingCheck(PsmhDb pDbCon, Form Frm, string strAcpNo, string strEmrNo)
        {
            bool bolReturn = false;
            string strFromEmrNo = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (strEmrNo == "0") return bolReturn;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT FROMEMRNO        ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRMAPPING      ";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + strAcpNo + "          ";
            SQL = SQL + ComNum.VBLF + "    AND TOEMRNO = " + strEmrNo + "      ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return true;
            }

            if (dt.Rows.Count > 0)
            {
                strFromEmrNo = dt.Rows[0]["FROMEMRNO"].ToString().Trim();
                dt.Dispose();
                dt = null;
                MappingSet(pDbCon, Frm, strFromEmrNo);
                bolReturn = true;
            }
            else
            {
                dt.Dispose();
                dt = null;
            }
            return bolReturn;
        }

        /// <summary>
        /// 매핑한 기록지의 정보를 가지고 온다.
        /// </summary>
        /// <param name="Frm"></param>
        /// <param name="intRow"></param>
        /// <param name="ssView_Sheet1"></param>
        /// <param name="strFROMEMRNO"></param>
        private static void MappingSet(PsmhDb pDbCon, Form Frm, string strFROMEMRNO)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = "SELECT ITEMCD, ITEMVALUE      ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTROW     ";
            SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strFROMEMRNO + "        ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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

            Control[] txtVal = null;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                txtVal = Frm.Controls.Find(dt.Rows[i]["ITEMCD"].ToString().Trim(), true);
                if (txtVal != null)
                {
                    if (txtVal.Length > 0)
                    {
                        if (txtVal[0] is TextBox)
                        {
                            Control obj = (TextBox)txtVal[0];
                            ((TextBox)obj).Text = "";
                            ((TextBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                        else if (txtVal[0] is ComboBox)
                        {
                            Control obj = (ComboBox)txtVal[0];
                            ((ComboBox)obj).Text = "";
                            ((ComboBox)obj).Text = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                        }
                        else if (txtVal[0] is CheckBox)
                        {
                            Control obj = (CheckBox)txtVal[0];

                            if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                            {
                                ((CheckBox)obj).Checked = true;
                            }
                            else
                            {
                                ((CheckBox)obj).Checked = false;
                            }
                        }
                        else if (txtVal[0] is RadioButton)
                        {
                            Control obj = (RadioButton)txtVal[0];

                            if (dt.Rows[i]["ITEMVALUE"].ToString().Trim() == "1")
                            {
                                ((RadioButton)obj).Checked = true;
                            }
                            else
                            {
                                ((RadioButton)obj).Checked = false;
                            }
                        }
                    }
                }

            }

            txtVal = null;
            txtVal = Frm.Controls.Find("lblEmrNoTR", true);
            if (txtVal != null)
            {
                if (txtVal.Length == 1)
                {
                    if (txtVal[0] is Label)
                    {
                        ((Label)txtVal[0]).Text = strFROMEMRNO;
                    }
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 수술 및 처치명이 비어 있을 때
        /// </summary>
        /// <param name="I0000001434_1"></param>
        /// <param name="I0000030604_1"></param>
        /// <param name="I0000030603_1"></param>
        public static void Conservative_treatment(TextBox I0000001434_1, TextBox I0000030604_1, TextBox I0000030603_1)
        {
            // 수술 및 처치명이 비어 있을 때
            if (I0000001434_1.Text.Trim() == "" && I0000030604_1.Text.Trim() == "" && I0000030603_1.Text.Trim() == "")
            {
                I0000030604_1.Text = "Conservative treatment";
            }
        }

        /// <summary>
        /// 폼에 있는 이미지 드로잉
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ct"></param>
        /// <param name="strTag"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strEmrNo"></param>
        public static void SetImageEvent(Form frm, Control ct, string strTag, string strFormNo, string strUpdateNo, string strMode, string strEmrNo, FormEmrMessage pEmrCallForm)
        {
            //기존 작성된 것인지 파악해서 드로잉 폼을 띄운다.
            if (clsEmrPublic.DrawingImage != null)
            {
                clsEmrPublic.DrawingImage.Dispose();
                clsEmrPublic.DrawingImage = null;
            }
            clsEmrPublic.DrawingImage = new frmEmrImageDrawingNew(strFormNo, strUpdateNo, strEmrNo, strMode, ((PictureBox)ct).Name.ToString(), ct, strTag, pEmrCallForm);
            clsEmrPublic.DrawingImage.rSetSavedImage += new frmEmrImageDrawingNew.SetSavedImage(frmEmrImageDrawing_SetSavedImage);
            clsEmrPublic.DrawingImage.rDeleteImage += new frmEmrImageDrawingNew.DeleteImage(frmEmrImageDrawing_DeleteImage);
            clsEmrPublic.DrawingImage.rEventClosed += new frmEmrImageDrawingNew.EventClosed(frmEmrImageDrawing_EventClosed);
            clsEmrPublic.DrawingImage.StartPosition = FormStartPosition.CenterParent;
            clsEmrPublic.DrawingImage.ShowDialog();
            //DrawingImage.BringToFront();
        }

        /// <summary>
        /// 폼에 있는 이미지 드로잉
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="ct"></param>
        /// <param name="strTag"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="baseImage"></param>
        public static void SetImageEvent(Form frm, Control ct, string strTag, string strFormNo, string strUpdateNo, string strMode, string strEmrNo, Image baseImage, FormEmrMessage pEmrCallForm)
        {
            //기존 작성된 것인지 파악해서 드로잉 폼을 띄운다.
            if (clsEmrPublic.DrawingImage != null)
            {
                clsEmrPublic.DrawingImage.Dispose();
                clsEmrPublic.DrawingImage = null;
            }
            clsEmrPublic.DrawingImage = new frmEmrImageDrawingNew(strFormNo, strUpdateNo, strEmrNo, strMode, ((PictureBox)ct).Name.ToString(), ct, strTag, baseImage, pEmrCallForm);
            clsEmrPublic.DrawingImage.StartPosition = FormStartPosition.CenterParent;
            clsEmrPublic.DrawingImage.rSetSavedImage += new frmEmrImageDrawingNew.SetSavedImage(frmEmrImageDrawing_SetSavedImage);
            clsEmrPublic.DrawingImage.rDeleteImage += new frmEmrImageDrawingNew.DeleteImage(frmEmrImageDrawing_DeleteImage);
            clsEmrPublic.DrawingImage.rEventClosed += new frmEmrImageDrawingNew.EventClosed(frmEmrImageDrawing_EventClosed);
            clsEmrPublic.DrawingImage.ShowDialog(frm);
            //DrawingImage.BringToFront();
        }

        private static void frmEmrImageDrawing_DeleteImage(string strFORMNO, string strUPDATENO, Control pCont, Image basImage)
        {
            try
            {
                ((PictureBox)pCont).Image = null;
                ((PictureBox)pCont).Tag = "";

                if (basImage != null)
                {
                    ((PictureBox)pCont).Image = basImage;
                }

                clsEmrQuery.SetFormInitImageEx(clsDB.DbCon, strFORMNO, strUPDATENO, pCont);
            }
            catch { }
        }

        private static void frmEmrImageDrawing_SetSavedImage(string strFormNo, string strUpdateNo, string strEmrNo, string strItemName,
                                                                        string strFoldJob, string strSaveFlag, string strImageName, Control pCont)
        {
            try
            {
                clsEmrPublic.DrawingImage.Dispose();
                clsEmrPublic.DrawingImage = null;

                //이미지가 있는지 검색해서 화면에 보여준다.
                string strImageNameJpg = strImageName + ".jpg";
                //이미지 사이즈 조정 후 로드
                int intWidth = ((PictureBox)pCont).Width;
                int intHeight = ((PictureBox)pCont).Height;

                using (Bitmap image1 = (Bitmap)Image.FromFile(strFoldJob + "\\" + strImageNameJpg, true))
                {
                    //비율에 맞게 이미지 조정
                    int newWidth = intWidth;
                    int newHeight = intHeight;
                    int originalWidth = image1.Width;
                    int originalHeight = image1.Height;
                    float percentWidth = (float)intWidth / (float)originalWidth;
                    float percentHeight = (float)intHeight / (float)originalHeight;
                    float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                    newWidth = (int)(originalWidth * percent);
                    newHeight = (int)(originalHeight * percent);

                    Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
                    Graphics graphics_1 = Graphics.FromImage(newImage);
                    graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                    graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                    graphics_1.DrawImage(image1, 0, 0, newWidth, newHeight);

                    ((PictureBox)pCont).BackColor = Color.White;
                    ((PictureBox)pCont).SizeMode = PictureBoxSizeMode.Zoom;
                    ((PictureBox)pCont).Image = newImage;
                    ((PictureBox)pCont).Tag = strImageName;
                }

                    #region //이전 루틴
                    //Bitmap newImage = new Bitmap(intWidth, intHeight, PixelFormat.Format24bppRgb);
                    //Graphics graphics_1 = Graphics.FromImage(newImage);
                    //graphics_1.CompositingQuality = CompositingQuality.HighQuality;
                    //graphics_1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //graphics_1.SmoothingMode = SmoothingMode.HighQuality;
                    //graphics_1.DrawImage(image1, 0, 0, intWidth, intHeight);
                    #endregion //이전 루틴
                
            }
            catch { }
        }

        private static void frmEmrImageDrawing_EventClosed()
        {
            clsEmrPublic.DrawingImage.Dispose();
            clsEmrPublic.DrawingImage = null;
        }

        /// <summary>
        /// Web의 줄바꿈을 C#의 줄바꿈으로 변경
        /// </summary>
        /// <param name="strChar"></param>
        /// <returns></returns>
        public static string ChangeWebCharToCshop(string strChar)
        {
            return strChar.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }

        public static void setChartFormValue2(EmrForm emrForm, Control Doc, string arg1, string arg2, string arg3)
        {

            if (emrForm.FmFORMNO == 2264 || emrForm.FmFORMNO == 2242 || emrForm.FmFORMNO == 2265)
            {
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", arg1);
                    if (elementEmpty(Doc, "it1")) setElement(Doc, "it1", arg2);
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "it2", arg3);
                }
                else
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "I0000001433", arg1);
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "I0000015541", arg1);
                    if (elementEmpty(Doc, "it1")) setElement(Doc, "I0000003210", arg2);
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "I0000003213", arg3);
                }
            }
            else if (emrForm.FmFORMNO == 1939 || emrForm.FmFORMNO == 2289 || emrForm.FmFORMNO == 1258 || emrForm.FmFORMNO == 2084 || emrForm.FmFORMNO == 2068 || emrForm.FmFORMNO == 2073)            
            {
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", arg1);
                    if (elementEmpty(Doc, "it5")) setElement(Doc, "it5", arg2);
                    if (elementEmpty(Doc, "it6")) setElement(Doc, "it6", arg3);
                }
                else
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "I0000001433", arg1);
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "I0000015541", arg1);
                    if (elementEmpty(Doc, "it1")) setElement(Doc, "I0000003210", arg2);
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "I0000003213", arg3);
                }
            }
            else if (emrForm.FmFORMNO == 2150 || emrForm.FmFORMNO == 2130)
            {
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", arg1);
                    if (elementEmpty(Doc, "it6")) setElement(Doc, "it6", arg2);
                    if (elementEmpty(Doc, "it7")) setElement(Doc, "it7", arg3);
                }
                else
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "I0000001433", arg1);
                    if (elementEmpty(Doc, "it1")) setElement(Doc, "I0000003210", arg2);
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "I0000003213", arg3);
                }
            }
            else if (emrForm.FmFORMNO == 1947) //사용안함
            {
                
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", arg1);
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "it2", arg2);
                    if (elementEmpty(Doc, "it3")) setElement(Doc, "it3", arg3);
                }
            }
            else if (emrForm.FmFORMNO == 1570) //사용안함
            {
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "it1")) setElement(Doc, "it1", arg1);
                    if (elementEmpty(Doc, "it12")) setElement(Doc, "it12", arg2);
                    if (elementEmpty(Doc, "it13")) setElement(Doc, "it13", arg3);
                }
            }
            else if (emrForm.FmFORMNO == 2236) //사용안함
            {
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", arg1);
                    if (elementEmpty(Doc, "it17")) setElement(Doc, "it17", arg2);
                    if (elementEmpty(Doc, "it18")) setElement(Doc, "it18", arg3);
                }
            }
            else if (emrForm.FmFORMNO == 2144)
            {
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", arg1);
                    if (elementEmpty(Doc, "it122")) setElement(Doc, "it122", arg2);
                    if (elementEmpty(Doc, "it123")) setElement(Doc, "it123", arg3);
                }
            }

            //신규 서식지
            if (emrForm.FmOLDGB == 0)
            {
                setElement(Doc, "I0000001433", arg1);
                setElement(Doc, "I0000003210", arg2);
                setElement(Doc, "I0000003213", arg3);
            }
        }

        public static void setChartFormValue7(Control Doc, string arg1, string arg2, string arg3)
        {
            if (elementEmpty(Doc, "it1"))  setElement(Doc, "it1", arg2);
            if (elementEmpty(Doc, "it2"))  setElement(Doc, "it2", arg3);
        }

        /// <summary>
        /// 혈액투석기록지
        /// </summary>
        /// <param name="argForm"></param>
        /// <param name="Doc"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public static void setChartFormValue1577(Control Doc, string ArgData)
        {
            string[] str = ArgData.Split(("|").ToCharArray());

            setElement(Doc, "it3",  str[0].Trim());// '투석기번호
            setElement(Doc, "it8",  str[1].Trim());// '투석시간
            setElement(Doc, "it10", str[2].Trim());// '투석액
            setElement(Doc, "it11", str[3].Trim());// '투석액온도
            setElement(Doc, "it12", str[4].Trim());// '헤파린초기용량
            setElement(Doc, "it13", str[5].Trim());// '헤파린유지용량
            setElement(Doc, "it24", str[6].Trim());// 'dry
            setElement(Doc, "it27", str[7].Trim());// 'tuf
        }

        /// <summary>
        /// Surgical Safety Checklist 기록지에
        /// 수술처방 데이터 연동
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Doc"></param>
        /// <param name="Ptno"></param>
        /// <param name="OpDate"></param>
        public static void setChartFormValue9(PsmhDb pDbCon, Control Doc, string Ptno, string OpDate, EmrForm emrForm)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT PREDIAGNOSIS, LEFTRIGHT, OPILL, POSITION";
            SQL += ComNum.VBLF + "  ,B.GBER, OPTITLE, ANDOCT1, ANNURSE, OPDOCT1, OPDOCT2, (OPNURSE || '/' || CNURSE) OPCNURSE, DR_STIME";
            SQL += ComNum.VBLF + "  ,GBEXAM1, GBEXAM2, GBEXAM3, GBEXAM4, GBEXAM5, GBEXAM6 ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.ORAN_MASTER B";
            SQL += ComNum.VBLF + "       ON A.WRTNO = B.WRTNO";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
               string strDIAGNOSIS = dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim();
               string strOPILL = dt.Rows[0]["OPILL"].ToString().Trim();
               string strPOSITION = string.Empty;

                string strOPTITLE = dt.Rows[0]["OPTITLE"].ToString().Trim();

                string strANDOCT = dt.Rows[0]["ANDOCT1"].ToString().Trim();
                string strANNurse = dt.Rows[0]["ANNURSE"].ToString().Trim();

                string strOPDOCT = dt.Rows[0]["OPDOCT1"].ToString().Trim();
                string strOPNurse = dt.Rows[0]["OPCNURSE"].ToString().Trim();


                switch (dt.Rows[0]["leftright"].ToString().Trim())
                {
                    case "0": break;//  '0.해당업음
                    case "1": strPOSITION += " (Rt)"; break; // '1.Right(Rt)
                    case "2": strPOSITION += " (Lt)"; break; // '2.Left(Lt)
                    case "3": strPOSITION += " (OD)"; break; // '3.Right(OD)
                    case "4": strPOSITION += " (OS)"; break; // '4.Left(OS)
                    case "5": strPOSITION += " Both"; break; // '5.Both
                }

                //이전 서식
                if (emrForm.FmOLDGB == 1)
                {
                    if (elementEmpty(Doc, "dt1")) setElement(Doc, "dt1", OpDate);
                    if (elementEmpty(Doc, "it1")) setElement(Doc, "it1", clsType.User.UserName);
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "it2", strDIAGNOSIS);
                    if (elementEmpty(Doc, "it3")) setElement(Doc, "it3", strOPILL);
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "it4", strPOSITION);
                }
                //신규 서식
                else
                {
                    strPOSITION = string.IsNullOrWhiteSpace(strPOSITION) ? "None" : strPOSITION;

                    //수술일자
                    if (elementEmpty(Doc, "I0000001434")) setElement(Doc, "I0000001434", OpDate);
                    //진단명
                    if (elementEmpty(Doc, "it2")) setElement(Doc, "I0000014147", strDIAGNOSIS);
                    //수술명
                    if (elementEmpty(Doc, "it3")) setElement(Doc, "I0000001429", strOPTITLE);
                    //수술부위
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "I0000027589", strPOSITION);
                    //완료된 수술명
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "I0000033664", strOPTITLE);
                    //마취의사, 마취간호사
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "I0000033667", strANDOCT);
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "I0000033668", strANNurse);
                    //집도의사, 소독/순회간호사
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "I0000031763", strOPDOCT);
                    if (elementEmpty(Doc, "it4")) setElement(Doc, "I0000033669", strOPNurse);
                }


            }

            dt.Dispose();
        }

        /// <summary>
        /// NEW수술간호기록지(3)
        /// 수술처방 데이터 연동
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Doc"></param>
        /// <param name="Ptno"></param>
        /// <param name="OpDate"></param>
        public static void setChartFormValue2618(PsmhDb pDbCon, EmrForm emrForm, Control Doc, string Ptno, string OpDate, string OpWrtno)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT PREDIAGNOSIS, B.DIAGNOSIS AS OPDIAGNOSIS, LEFTRIGHT, OPILL, POSITION, CNURSE";
            SQL += ComNum.VBLF + "  ,B.GBER, OPTITLE, ANDOCT1, ANNURSE, OPDOCT1, OPDOCT2, (OPNURSE || '/' || CNURSE) OPCNURSE, DR_STIME -- 정규(응급) 구분, 수술명, 마취의사 성명, 마취과간호사 성명, 수술의사성명, 수술보조의사 성명, 수술실간호사 성명";
            SQL += ComNum.VBLF + "  ,OPNURSE, CNURSE";
            SQL += ComNum.VBLF + "  ,EORTIME, OPTIMEFROM, OPTIMETO, OPSTIME, OPETIME, OPPOSITION -- 입구도착, 마취시작, 마취종료(퇴실시각), 수술시작, 수술종료, 수술 Position";
            SQL += ComNum.VBLF + "  ,ANGBN, GBEXAM1, GBEXAM2, GBEXAM3, GBEXAM4, GBEXAM5, GBEXAM6 --마취분류코드 ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.ORAN_MASTER B";
            SQL += ComNum.VBLF + "       ON A.WRTNO = B.WRTNO";
            SQL += ComNum.VBLF + " WHERE A.PANO = '" + Ptno + "' ";
            SQL += ComNum.VBLF + "   AND A.OPDATE = TO_DATE('" + OpDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND A.WRTNO = " + OpWrtno;
            SQL += ComNum.VBLF + " ORDER BY A.OPDATE DESC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                #region 마취종류
                Dictionary<string, string> AnGubun = new Dictionary<string, string>
                {
                    { "G", "I0000022530" }, //General
                    { "MASK", "I0000031039" }, //Mask
                    { "MAC", "I0000010854" }, //MAC
                    { "S", "I0000030597" }, //Spinal
                    { "E", "I0000030598" }, //Epidural
                    { "C", "I0000033625" }, //Caudal
                    { "FNB", "I0000033632" }, //FNB
                    { "SNB", "I0000024579" }, //SNB
                    { "A", "I0000033633" }, //BPB(A)
                    { "B", "I0000033634" }, //BPB(I)
                    { "L", "I0000012570" }, //Local
                    { "Z", "I0000001067_1" } //None(기타)
                };
                #endregion

                #region 수술체위
                Dictionary<string, string> OpGubun = new Dictionary<string, string>
                {
                    { "1", "I0000000757" }, //Supine
                    { "2", "I0000014757" }, //Prone
                    { "3", "I0000014477" }, //Lith
                    { "4", "I0000014448" }, //Lat
                    { "5", "I0000035760" }, //JK
                    { "6", "I0000000690" }  //Sit
                    //{ "07", "I0000033632" }  //Spinal
                };
                #endregion

                string strGbEr = dt.Rows[0]["GBER"].ToString().Trim();
                string strDIAGNOSIS = dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim();
                string strPOSITION = string.Empty;

                string strOPTITLE = dt.Rows[0]["OPTITLE"].ToString().Trim();

                string strANDOCT = dt.Rows[0]["ANDOCT1"].ToString().Trim();
                string strANNurse = dt.Rows[0]["ANNURSE"].ToString().Trim();

                string strOPNurse = dt.Rows[0]["OPCNURSE"].ToString().Trim();

                switch (dt.Rows[0]["leftright"].ToString().Trim())
                {
                    case "0": break;//  '0.해당업음
                    case "1": strPOSITION += " (Rt)"; break; // '1.Right(Rt)
                    case "2": strPOSITION += " (Lt)"; break; // '2.Left(Lt)
                    case "3": strPOSITION += " (OD)"; break; // '3.Right(OD)
                    case "4": strPOSITION += " (OS)"; break; // '4.Left(OS)
                    case "5": strPOSITION += " Both"; break; // '5.Both
                }

                string strGetVal = string.Empty;


                if (emrForm.FmOLDGB == 0)
                {
                    #region 정규/응급
                    Control control = Doc.Controls.Find(strGbEr.Equals("*") ? "I0000011303" : "I0000015141", true).FirstOrDefault();
                    ((RadioButton)control).Checked = true;
                    #endregion

                    #region 수술일자
                    setElement(Doc, "I0000001434", OpDate);
                    #endregion

                    #region 수술전 진단명                
                    setElement(Doc, "I0000031745", strDIAGNOSIS);
                    #endregion

                    #region 수술후 진단명                
                    setElement(Doc, "I0000031746", dt.Rows[0]["OPDIAGNOSIS"].ToString().Trim());
                    #endregion

                    #region 수술명
                    setElement(Doc, "I0000001429", strOPTITLE);
                    #endregion

                    #region 마취종류
                    if (string.IsNullOrWhiteSpace(dt.Rows[0]["ANGBN"].ToString().Trim()) == false && AnGubun.TryGetValue(VB.Val(dt.Rows[0]["ANGBN"].ToString().Trim()).ToString(), out strGetVal))
                    {
                        control = Doc.Controls.Find(strGetVal, true).FirstOrDefault();
                        ((RadioButton)control).Checked = true;
                    }
                    #endregion

                    #region 집도의사 / Assist
                    setElement(Doc, "I0000000477", dt.Rows[0]["OPDOCT1"].ToString().Trim());
                    setElement(Doc, "I0000030523", dt.Rows[0]["OPDOCT2"].ToString().Trim());
                    setElement(Doc, "I0000035754", dt.Rows[0]["OPDOCT1"].ToString().Trim());

                    #endregion

                    #region 수술간호사
                    setElement(Doc, "I0000035751_1", dt.Rows[0]["OPNURSE"].ToString().Trim());
                    setElement(Doc, "I0000035751_3", dt.Rows[0]["OPNURSE"].ToString().Trim());
                    #endregion

                    #region 순환간호사
                    setElement(Doc, "I0000033665_1", dt.Rows[0]["CNURSE"].ToString().Trim());
                    setElement(Doc, "I0000033665_3", dt.Rows[0]["CNURSE"].ToString().Trim());
                    setElement(Doc, "I0000035755", dt.Rows[0]["CNURSE"].ToString().Trim());
                    #endregion

                    #region 마취의 / 마취간호사
                    setElement(Doc, "I0000012078", strANDOCT);
                    setElement(Doc, "I0000033668", strANNurse);
                    #endregion

                    #region 입구도착
                    setElement(Doc, "I0000035756", TimeText(dt.Rows[0]["EORTIME"].ToString().Trim()));
                    #endregion

                    #region 마취시작, 마취종료(퇴실시각)
                    setElement(Doc, "I0000030772", TimeText(dt.Rows[0]["OPTIMEFROM"].ToString().Trim()));
                    setElement(Doc, "I0000030776", TimeText(dt.Rows[0]["OPTIMETO"].ToString().Trim()));
                    setElement(Doc, "I0000035709", TimeText(dt.Rows[0]["OPTIMETO"].ToString().Trim()));
                    #endregion

                    #region 수술시작, 수술종료
                    setElement(Doc, "I0000035758", TimeText(dt.Rows[0]["OPSTIME"].ToString().Trim()));
                    setElement(Doc, "I0000030775", TimeText(dt.Rows[0]["OPETIME"].ToString().Trim()));
                    #endregion

                    #region 수술 체위
                    if (string.IsNullOrWhiteSpace(dt.Rows[0]["OPPOSITION"].ToString().Trim()) == false && OpGubun.TryGetValue(dt.Rows[0]["OPPOSITION"].ToString().Trim(), out strGetVal))
                    {
                        control = Doc.Controls.Find(strGetVal, true).FirstOrDefault();
                        ((CheckBox)control).Checked = true;
                    }
                    #endregion
                }
                else
                {
                    #region 수술일자
                    setElement(Doc, "dt1", OpDate);
                    #endregion

                    #region 수술전 진단명                
                    setElement(Doc, "it1", strDIAGNOSIS);
                    #endregion

                    //#region 수술후 진단명                
                    //setElement(Doc, "it2", strDIAGNOSIS);
                    //#endregion

                    #region 수술명
                    setElement(Doc, "it3", strOPTITLE);
                    #endregion
                }

            }

            dt.Dispose();
        }

        /// <summary>
        /// 0000을 00:00 시:분으로 반환
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static string TimeText(string strVal)
        {
            string rtnVal = string.Empty;

            if (strVal.IndexOf(":") != -1)
                return strVal;

            if (VB.IsNumeric(strVal) && strVal.Length >= 3)
            {
                string Hour = strVal.Substring(0, 2);
                string Minute = strVal.Substring(2, strVal.Length == 3 ? 1 : 2);
                rtnVal = VB.Val(Hour).ToString("00") + ":" + VB.Val(Minute).ToString("00");
            }

            return rtnVal;
        }



        public static bool elementEmpty(Control doc, string ControlName)
        {
            //19-10-02 내용 있어도 무조건 엎어치게 해달라고 요청(의료정보팀)
            return true;
            bool rtnVal = false;
            if(doc.Controls.Find(ControlName, true).Length > 0 && doc.Controls.Find(ControlName, true)[0].Text.Trim().Length == 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        public static void setElement(Control doc, string ControlName, string strValue)
        {
            Control[] controls = doc.Controls.Find(ControlName, true);
            if (controls.Length > 0)
            {
                if (controls[0] is RadioButton)
                {
                   ((RadioButton)controls[0]).Checked = strValue.ToUpper().Equals("1");
                }
                else if (controls[0] is CheckBox)
                {
                    ((CheckBox)controls[0]).Checked = strValue.ToUpper().Equals("1");
                }
                else if (controls[0] is TextBox)
                {
                    controls[0].Text = strValue;
                }
            }
        }

        public static Bitmap CropWhiteBitmap(Bitmap bmp)
        {
            int w = bmp.Width, h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth <= 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight <= 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                Bitmap target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3}", topmost, bottommost, leftmost, rightmost),
                  ex);
            }
        }

        public Bitmap Crop(Bitmap bitmap)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;

            Func<int, bool> IsAllWhiteRow = row =>
            {
                for (int i = 0; i < w; i++)
                {
                    if (bitmap.GetPixel(i, row).R != 255)
                    {
                        return false;
                    }
                }
                return true;
            };

            Func<int, bool> IsAllWhiteColumn = col =>
            {
                for (int i = 0; i < h; i++)
                {
                    if (bitmap.GetPixel(col, i).R != 255)
                    {
                        return false;
                    }
                }
                return true;
            };

            int leftMost = 0;
            for (int col = 0; col < w; col++)
            {
                if (IsAllWhiteColumn(col)) leftMost = col + 1;
                else break;
            }

            int rightMost = w - 1;
            for (int col = rightMost; col > 0; col--)
            {
                if (IsAllWhiteColumn(col)) rightMost = col - 1;
                else break;
            }

            int topMost = 0;
            for (int row = 0; row < h; row++)
            {
                if (IsAllWhiteRow(row)) topMost = row + 1;
                else break;
            }

            int bottomMost = h - 1;
            for (int row = bottomMost; row > 0; row--)
            {
                if (IsAllWhiteRow(row)) bottomMost = row - 1;
                else break;
            }

            if (rightMost == 0 && bottomMost == 0 && leftMost == w && topMost == h)
            {
                return bitmap;
            }

            int croppedWidth = rightMost - leftMost + 1;
            int croppedHeight = bottomMost - topMost + 1;

            try
            {
                Bitmap target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bitmap,
                        new RectangleF(0, 0, croppedWidth, croppedHeight),
                        new RectangleF(leftMost, topMost, croppedWidth, croppedHeight),
                        GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Values are top={0} bottom={1} left={2} right={3}", topMost, bottomMost, leftMost, rightMost), ex);
            }
        }
    }
}