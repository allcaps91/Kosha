using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB; //DB연결
using ComPmpaLibB;
using System.Data;
using ComBase;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

/// <summary>
/// Description : BAS모듈
/// Author : 박병규
/// Create Date : 2018.4.11
/// </summary>
/// <history>
/// </history>
/// <seealso cref="무인수납.bas"/> 

namespace ComPmpaLibB
{
    public class clsAutoAcct
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public static frmAutoInfomation frmAutoInfomationX = null;

        //DataTable DtAuto = new DataTable();
        //DataTable DtSub = new DataTable();
        //
        //string SQL = "";
        //string SqlErr = "";
        //int intRowCnt = 0;
        //string rtnVal = "";

        clsOumsadChk cOC = new clsOumsadChk();

        /// <summary>
        /// Description : 무인수납장비_고유번호체크
        /// Author : 박병규
        /// Create Date : 2018.04.11
        /// <param name="pDbCon"></param>
        /// <param name="ArgValue"></param>
        /// <seealso cref="무인수납.bas : 무인수납장비_고유번호체크"/>
        /// </summary>

        public string Check_Machine_No(PsmhDb pDbCon, string ArgValue)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";            

            string rtnVal = "";
            string strTemp = VB.Replace(ArgValue, "-", "");
            string strTemp2 = "OK";
            string strJumin2 = "";

            //strTemp = string.Format("{0:D8}", Convert.ToInt64(strTemp));

            sb.Clear();
            sb.AppendLine(" SELECT Pano, SName, JUMIN1, ");
            sb.AppendLine("        Jumin2, Jumin3 ");
            sb.AppendLine("   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ");
            sb.AppendLine("  WHERE 1       = 1 ");

            if (strTemp.Length < 9)
                sb.AppendLine("AND PANO    = '" + strTemp + "' ");
            else
            {
                sb.AppendLine("AND Jumin1  = '" + VB.Left(strTemp, 6) + "' ");
                sb.AppendLine("AND (Jumin2 = '" + VB.Right(strTemp, 7) + "' OR Jumin3 = '" + clsAES.AES(VB.Right(strTemp, 7)) + "'  ) ");
            }

            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (DtAuto.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    strJumin2 = clsAES.DeAES(DtAuto.Rows[0]["JUMIN3"].ToString().Trim());
                else
                    strJumin2 = DtAuto.Rows[0]["JUMIN2"].ToString().Trim();

                if (strJumin2.Length != 7)
                    strTemp2 = "NO";
                else
                {
                    switch (strJumin2)
                    {
                        case "0000000":
                            strTemp2 = "NO";
                            break;
                        case "1000000":
                            strTemp2 = "NO";
                            break;
                        case "1000001":
                            strTemp2 = "NO";
                            break;
                        case "2000000":
                            strTemp2 = "NO";
                            break;
                        case "2000001":
                            strTemp2 = "NO";
                            break;
                        case "3000000":
                            strTemp2 = "NO";
                            break;
                        case "3000001":
                            strTemp2 = "NO";
                            break;
                        case "4000000":
                            strTemp2 = "NO";
                            break;
                        case "5000000":
                            strTemp2 = "NO";
                            break;
                        case "6000000":
                            strTemp2 = "NO";
                            break;
                        case "7000000":
                            strTemp2 = "NO";
                            break;
                        case "8000000":
                            strTemp2 = "NO";
                            break;
                        case "9000000":
                            strTemp2 = "NO";
                            break;
                    }
                }

                if (strTemp2 == "NO")
                {
                    if (VB.Left(strTemp, 6) != "810000")
                    {
                        DtAuto.Dispose();
                        DtAuto = null;
                        clsAuto.GstrAREquipExceptionCheck = "주민번호 점검";
                        return rtnVal;
                    }
                }

                rtnVal = "OK";
                clsAuto.GstrAR_Pano = DtAuto.Rows[0]["PANO"].ToString().Trim();
                clsAuto.GstrAR_SName = DtAuto.Rows[0]["SName"].ToString().Trim();

                if (DtAuto.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    clsAuto.GstrAR_Jumin = DtAuto.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(DtAuto.Rows[0]["JUMIN3"].ToString().Trim());
                else
                    clsAuto.GstrAR_Jumin = DtAuto.Rows[0]["JUMIN1"].ToString().Trim() + DtAuto.Rows[0]["JUMIN2"].ToString().Trim();
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 무인수납장비위치설정
        /// Author : 박웅규
        /// Create Date : 2018.08.11
        /// <seealso cref="무인수납.bas : 무인수납장비위치설정"/>
        /// </summary>
        public void Check_Machine_Location()
        {
            clsAuto.GstrAREquipUse = "";
            clsAuto.GstrAREquipUsePlace = "";

            string strOK = "";
            string strValue = "";  //무인수납장비위치설정


            FileInfo fi = new FileInfo(@"c:\cmc\무인수납.ini");
            if (fi.Exists == true)
            {
                string strPart = "";
                StreamReader file = new System.IO.StreamReader(@"c:\cmc\무인수납.ini");
                strPart = file.ReadLine();
                if (strPart.Trim() != "")
                {
                    strOK = "OK";
                    strValue = strPart.Trim();
                }
                file.Close();
            }
            fi = null;

            if (strOK == "OK")
            {
                clsAuto.GstrAREquipUse = "OK";

                //추후 사용예정

                clsPublic.GnJobSabun = (long)VB.Val(strValue);
                clsPublic.GstrJobName = "무인수납*";
                clsPublic.GstrJobPart = strValue;

                switch(clsPublic.GnJobSabun)
                {
                    case 4349:
                        clsAuto.GstrAREquipUsePlace = "9";         //전산실연습
                        break;
                    case 5000:
                        clsAuto.GstrAREquipUsePlace = "9";         //소아과사용
                        break;
                    case 5001:
                        clsAuto.GstrAREquipUsePlace = "9";         //소아과사용
                        break;
                    case 5002:
                        clsAuto.GstrAREquipUsePlace = "2";         //정형외과사용
                        break;
                    case 5003:
                    case 5004:
                    case 5050:
                        clsAuto.GstrAREquipUsePlace = "8";         //본관1층
                        break;
                    case 5005:
                    case 5006:
                        clsAuto.GstrAREquipUsePlace = "8";         //본관2층
                        break;
                    //case 5050: //ERROR : 중복코딩이 되어있음
                    //    clsKioskPb.GstrAREquipUsePlace = "9";         //본관2층
                    //    break;
                    default:
                        clsAuto.GstrAREquipUsePlace = "9";
                        clsAuto.GstrAREquipUse = "";
                        break;
                }
            }

            //오류표시관련
            clsAuto.GstrAREquipErrorSign = "";

            fi = new FileInfo(@"C:\무인수납log.ini");
            if (fi.Exists == true)
            {
                clsAuto.GstrAREquipErrorSign = "OK";
            }
            fi = null;

            ////==> 무인수납 정보 강제 세팅
            //clsAuto.GstrAREquipUsePlace = "9";
            //clsAuto.GstrAREquipUse = "OK";
            ////<== 무인수납 정보 강제 세팅
        }

        /// <summary>
        /// Description : 무인수납장비장애체크
        /// Author : 박병규
        /// Create Date : 2018.06.07
        /// <param name="pDbCon"></param>
        /// <seealso cref="무인수납.bas : 무인수납장비장애체크"/>
        /// </summary>

        public string Check_Machine_Obstacle(PsmhDb pDbCon)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";
            clsAuto.GstrAREquipObstacle = "";

            //무인수납장비 수납 전체 사용여부
            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE         ");
            sb.AppendLine("  WHERE 1             = 1                        ");
            sb.AppendLine("    AND Gubun         = '무인수납장비장애구분'   ");
            sb.AppendLine("    AND CODE          = '001'                    ");
            sb.AppendLine("    AND TRIM(NAME)    = 'Y'                      ");
            sb.AppendLine("    AND (DELDATE IS NULL OR DELDATE ='')         ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count == 0)
            {
                rtnVal = "OK";
                clsAuto.GstrAREquipObstacle = "OK";
            }

            DtAuto.Dispose();
            DtAuto = null;

            //무인수납장비 수납 의료급여 자격조회 에러발생시 사용여부
            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE         ");
            sb.AppendLine("  WHERE 1             = 1                        ");
            sb.AppendLine("    AND Gubun         = '무인수납장비장애구분'   ");
            sb.AppendLine("    AND CODE          = '002'                    ");
            sb.AppendLine("    AND TRIM(NAME)    = 'Y'                      ");
            sb.AppendLine("    AND ( DELDATE IS NULL OR DELDATE ='')        ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count == 0)
            {
                rtnVal = "OK2";
                clsAuto.GstrAREquipObstacle = "OK2";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 물리치료무인수납장애체크
        /// Author : 박병규
        /// Create Date : 2018.06.07
        /// <param name="pDbCon"></param>
        /// <seealso cref="무인수납.bas : 물리치료무인수납장애체크"/>
        /// </summary>

        public string Check_PT_Machine_Obstacle(PsmhDb pDbCon)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";
            clsAuto.GstrPhysicalTherapyReceive = "";

            //무인수납장비 수납 전체 사용여부
            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE             ");
            sb.AppendLine("  WHERE 1             = 1                            ");
            sb.AppendLine("    AND Gubun         = '물리치료무인수납가능여부'   ");
            sb.AppendLine("    AND CODE          = '001'                        ");
            sb.AppendLine("    AND TRIM(NAME)    = 'Y'                          ");
            sb.AppendLine("    AND (DELDATE IS NULL OR DELDATE ='')             ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count == 0)
            {
                rtnVal = "OK";
                clsAuto.GstrPhysicalTherapyReceive = "OK";
            }

            DtAuto.Dispose();
            DtAuto = null;

            //무인수납장비 수납 의료급여 자격조회 에러발생시 사용여부
            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE             ");
            sb.AppendLine("  WHERE 1             = 1                            ");
            sb.AppendLine("    AND Gubun         = '물리치료무인수납가능여부'   ");
            sb.AppendLine("    AND CODE          = '002'                        ");
            sb.AppendLine("    AND TRIM(NAME)    = 'Y'                          ");
            sb.AppendLine("    AND ( DELDATE IS NULL OR DELDATE ='')            ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count == 0)
            {
                rtnVal = "OK2";
                clsAuto.GstrPhysicalTherapyReceive = "OK2";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : OCS 연동시 무인장비예외처리
        /// Author : 박병규
        /// Create Date : 2018.06.07
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgBdate"></param>
        /// <seealso cref="무인수납.bas : READ_OCS_무인장비예외처리_CHK"/>
        /// </summary>

        public string Check_Read_OCS_Exception(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgBdate)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string strJin = "";
            string strJinDtl = "";
            string strBi = "";
            string strRes = "";
            string strDrCode = "";
            string strMCode = "";
            string strVCode = "";
            string strGelCode = "";
            string strGam = "";
            string strDementia = "";
            string strSex = "";
            int nAge = 0;
            string strJumin = "";
            long nAmt1  = 0;
            string rtnVal = "";
            
            sb.Clear();
            sb.AppendLine(" SELECT a.Jin,a.JinDtl,a.Reserved,                               ");
            sb.AppendLine("        a.DrCode,a.MCode,a.VCode,                                ");
            sb.AppendLine("        a.GelCode,a.GbGameK,a.GbDementia,                        ");
            sb.AppendLine("        a.Bi,a.Age,a.Sex,                                        ");
            sb.AppendLine("        b.Jumin1,b.Jumin2,b.Jumin3,                              ");
            sb.AppendLine("        a.Amt1                                                   ");
            sb.AppendLine("   FROM KOSMOS_PMPA.OPD_MASTER a,                                ");
            sb.AppendLine("        KOSMOS_PMPA.BAS_PATIENT b                                ");
            sb.AppendLine("  WHERE 1             = 1                                        ");
            sb.AppendLine("    AND a.Pano        = b.Pano(+)                                ");
            sb.AppendLine("    AND a.PANO        = '" + ArgPtno + "'                        ");
            sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                        ");
            sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.ACTDATE     = TRUNC(SYSDATE)                           "); //당일접수건만
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                strJin = DtAuto.Rows[0]["JIN"].ToString().Trim();
                strJinDtl = DtAuto.Rows[0]["JinDtl"].ToString().Trim();
                strBi = DtAuto.Rows[0]["Bi"].ToString().Trim(); 
                strRes = DtAuto.Rows[0]["Reserved"].ToString().Trim(); 
                strDrCode = DtAuto.Rows[0]["DrCode"].ToString().Trim();
                strMCode = DtAuto.Rows[0]["MCode"].ToString().Trim();
                strVCode = DtAuto.Rows[0]["VCode"].ToString().Trim();
                strGelCode = DtAuto.Rows[0]["GelCode"].ToString().Trim();
                strGam = DtAuto.Rows[0]["GbGameK"].ToString().Trim(); 
                strDementia = DtAuto.Rows[0]["GbDementia"].ToString().Trim();
                strSex = DtAuto.Rows[0]["Sex"].ToString().Trim(); 
                nAge = Convert.ToInt32(DtAuto.Rows[0]["Age"].ToString().Trim()); 
                nAmt1 = Convert.ToInt64(DtAuto.Rows[0]["Amt1"].ToString().Trim());


                if (DtAuto.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    strJumin = DtAuto.Rows[0]["Jumin1"].ToString().Trim() + clsAES.DeAES(DtAuto.Rows[0]["JUMIN3"].ToString().Trim());
                else
                    strJumin = DtAuto.Rows[0]["Jumin1"].ToString().Trim() + DtAuto.Rows[0]["Jumin2"].ToString().Trim();

                rtnVal = Check_Read_Exception("ORDER", strJin, strJinDtl, strRes, ArgPtno, clsPublic.GstrSysDate, ArgDept, strDrCode, strBi, nAge, strMCode, strVCode, strGelCode, strGam, "", "", strDementia, strJumin, nAmt1);

            }
            else
            {
                rtnVal = "NO";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : OCS 연동시 무인장비예외처리
        /// Author : 박병규
        /// Create Date : 2018.06.07
        /// <param name="pDbCon"></param>
        /// <seealso cref="무인수납.bas : Read_무인장비예외처리_CHK1"/>
        /// </summary>

        public string Check_Read_Exception(string ArgChk, string ArgJin, string ArgJinDtl, string ArgRes, string ArgPtno, string ArgActdate, string ArgDept, string ArgDrCode, string ArgBi, int ArgAge, string ArgMCode, string ArgVCode, string ArgGelCode, string ArgGam, string ArgRdate, string ArgRTime, string ArgDementia, string ArgJumin, long ArgJinAmt1)
        {
            
            DataTable DtAuto = new DataTable();
            DataTable DtSub = new DataTable();            
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            ComFunc cF = new ComFunc();
            string rtnVal = "";
            string strTemp = "";
            string strTemp2 = "";
            string strMsg = "";
            int nBuCount = 0;
            int nBuCount3 = 0;
            string strChoJae = "";


            clsAuto.GstrAREquipExceptionCheck = "";
            clsAuto.GstrAREquipQualification = "";
            clsAuto.GstrAR_RDate = "";
            clsAuto.GstrAREquipCheckSentence = "무인수납 불가";

            clsAuto.GnAR_DRUG_OUT = 0;
            clsAuto.GnAR_DRUG_ATC = 0;

            if (clsAuto.GstrOfficeCheck == "휴일" || clsAuto.GstrOfficeCheck == "일요일")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "무인수납사용시간아님";
                return rtnVal;
            }
            else if (clsAuto.GstrOfficeCheck == "토요일")
            {
                if (clsType.User.IdNumber == "5005")
                {
                    if (string.Compare(clsPublic.GstrSysTime, "17:30") > 0)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "무인수납사용시간아님";
                        return rtnVal;
                    }
                    else
                    {
                        if (string.Compare(clsPublic.GstrSysTime, clsAuto.GstrAREquipEndTime) > 0)
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "무인수납사용시간아님";
                            return rtnVal;
                        }
                    }

                }
            }
            else if (clsAuto.GstrOfficeCheck == "평일")
            {
                if (clsType.User.IdNumber == "5005")
                {
                    if (clsAuto.GstrAREquipEndTime != "" && string.Compare(clsPublic.GstrSysTime, "17:30") > 0)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "무인수납사용시간아님";
                        return rtnVal;
                    }
                }
                else
                {
                    if (clsAuto.GstrAREquipEndTime != "" && string.Compare(clsPublic.GstrSysTime, clsAuto.GstrAREquipEndTime) > 0)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "무인수납사용시간아님";
                        return rtnVal;
                    }
                }
            }

            //무인수납장비 사용여부, 점검체크
            strTemp = Check_Machine_Obstacle(clsDB.DbCon);

            if (strTemp == "OK")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "무인수납장애서비스점검중";
                return rtnVal;
            }
            else if (strTemp2 == "OK2")
            {
                clsAuto.GstrAREquipQualification = "OK";
            }

            //물리치료 무인수납 장애체크
            strTemp = Check_PT_Machine_Obstacle(clsDB.DbCon);

            if (strTemp == "OK")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "물리치료무인수납장애점검중";
                return rtnVal;
            }
            else if (strTemp2 == "OK2")
            {
                clsAuto.GstrAREquipQualification = "OK";
            }

            strTemp = "";

            if (ArgChk == "")
            {
                sb.Clear();
                sb.AppendLine(" SELECT Remark, TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI') EntTime ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OLOCK                                  ");
                sb.AppendLine("  WHERE 1         = 1                                         ");
                sb.AppendLine("    AND PTNO      = '" + ArgPtno + "'                         ");
                sb.AppendLine("    AND Remark    <>'수납중입니다(auto)'                      ");
                sb.AppendLine("    AND TRUNC(ENTDATE) = TRUNC(SYSDATE)                       ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "처방수정 및 입력중";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //신생아 주민번호 체크
            if (ArgJumin != "")
            {
                switch (VB.Right(ArgJumin, 7))
                {
                    case "0000000":
                    case "1000000":
                    case "1000001":
                    case "2000000":
                    case "2000001":
                    case "3000000":
                    case "3000001":
                    case "4000000":
                    case "5000000":
                    case "6000000":
                    case "7000000":
                    case "8000000":
                    case "9000000":

                        if (VB.Left(ArgPtno, 6) != "810000")
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "주민번호뒷자리오류";
                            return rtnVal;
                        }

                        break;
                }
            }

            if (string.Compare(VB.Left(ArgBi, 1),  "3") < 0)
            {
                if (VB.Left(ArgDept, 1) == "M" && VB.Right(ArgDrCode, 2) != "99")
                {
                    if (ArgJin == "E")
                    {
                        if (cF.Check_Dept_Certified(clsDB.DbCon, ArgDrCode, ArgActdate) == false)
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "비인증전문의 전화접수";
                            return rtnVal;
                        }
                    }

                    if (cF.Check_Dept_Certified(clsDB.DbCon, ArgDrCode, ArgActdate) == false)
                    {
                        if (Convert.ToInt32(ArgJinAmt1) > 0)
                        {
                            strMsg = cF.Check_Dept_ReceiveHis(clsDB.DbCon, ArgPtno, ArgActdate, "비인증", ArgDept, ArgDrCode, Convert.ToInt32(ArgJinAmt1));

                            if (strMsg != "")
                            {
                                rtnVal = "NO";
                                clsAuto.GstrAREquipExceptionCheck = "비인증전문의 진료";
                                return rtnVal;
                            }
                        }

                        if (Convert.ToInt32(ArgJinAmt1) > 0)
                        {
                            strMsg = cF.Check_Dept_ReceiveHis(clsDB.DbCon, ArgPtno, ArgActdate, "인증", ArgDept, ArgDrCode, Convert.ToInt32(ArgJinAmt1));

                            if (strMsg != "")
                            {
                                rtnVal = "NO";
                                clsAuto.GstrAREquipExceptionCheck = "비인증전문의 진료";
                                return rtnVal;
                            }
                        }
                    }
                    else
                    {
                        strMsg = cF.Check_Dept_ReceiveHis(clsDB.DbCon, ArgPtno, ArgActdate, "비인증", ArgDept, ArgDrCode, Convert.ToInt32(ArgJinAmt1));

                        if (strMsg != "")
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "비인증전문의 진료";
                            return rtnVal;
                        }
                    }
                }
            }

            if (ArgRes == "1")
            {
                sb.Clear();
                sb.AppendLine(" SELECT TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3, DEPTCODE      ");
                sb.AppendLine("   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                             ");
                sb.AppendLine("  WHERE 1                 = 1                                    ");
                sb.AppendLine("    AND PANO              = '" + ArgPtno + "'                    ");
                sb.AppendLine("    AND DEPTCODE          = '" + ArgDept + "'                    ");
                sb.AppendLine("    AND DATE1             < TO_DATE('2014-08-01','YYYY-MM-DD')   "); //8/1일 이전건
                sb.AppendLine("    AND TRUNC(TRANSDATE)  =TRUNC(SYSDATE)                        "); //당일 접수전환
                sb.AppendLine("    AND (RETDATE IS NULL OR RETDATE = '')                        "); //환불 안된것
                sb.AppendLine("    AND AMT2              > 0                                    "); //선택금액 발생건
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "선택진료8/1단가확인";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //의료질평가지원금 긴급예외추가
            if (ArgRes == "1" && ArgBi != "21" && ArgBi != "22")
            {
                sb.Clear();
                sb.AppendLine(" SELECT TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3, DEPTCODE  ");
                sb.AppendLine("   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                         ");
                sb.AppendLine("  WHERE 1         = 1                                        ");
                sb.AppendLine("    AND PANO      = '" + ArgPtno + "'                        ");
                sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "'                        ");
                sb.AppendLine("    AND DATE1     < TO_DATE('2015-09-01','YYYY-MM-DD')       "); //9/1일 이전건
                sb.AppendLine("    AND TRUNC(TRANSDATE) = TRUNC(SYSDATE)                    "); //당일 접수전환
                sb.AppendLine("    AND (RETDATE IS NULL OR RETDATE = '')                    "); //환불 안된것
                sb.AppendLine("    AND AMT1 > 0                                             "); //금액 발생건
                sb.AppendLine("    AND ( JIWON = '' OR JIWON IS NULL )                      ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    sb.Clear();
                    sb.AppendLine(" SELECT ROWID                                                    ");
                    sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER                                    ");
                    sb.AppendLine("  WHERE 1         = 1                                            ");
                    sb.AppendLine("    AND PTNO      = '" + ArgPtno + "'                            ");
                    sb.AppendLine("    AND BDATE     = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
                    sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "'                            ");
                    sb.AppendLine("    AND GBSunap   ='0'                                           ");  //미수납만
                    sb.AppendLine("    AND SUBSTR(SuCode,1,2) <> '$$'                               ");  //보호자 내원코드 점검
                    sb.AppendLine("    AND SUCODE IS NOT NULL                                       ");
                    sb.AppendLine("    AND BUN NOT IN ('11','12')                                   ");
                    SqlErr = clsDB.GetDataTableEx(ref DtSub, sb.ToString(), clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtSub.Dispose();
                        DtSub = null;
                        DtAuto.Dispose();
                        DtAuto = null;
                        return rtnVal;
                    }

                    if (DtSub.Rows.Count == 0)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "9월1일이전예약";
                        DtSub.Dispose();
                        DtSub = null;
                        return rtnVal;
                    }

                    DtSub.Dispose();
                    DtSub = null;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //B항 수가항목 체크
            strTemp = "";

            for (int nX = 1; nX <= 3; nX++)
            {
                strTemp = nX.ToString();

                if (nX == 1) { strTemp = ""; }

                strMsg = cF.Check_SugbB(clsDB.DbCon, "ETC_B_SUCHK" + strTemp, ArgPtno, ArgDept);

                if (strMsg != "")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = strMsg;
                    return rtnVal;
                }
            }

            if (ComFunc.CHK_Practitioner_RegularDeptReceive(clsDB.DbCon, ArgPtno, ArgDrCode) == true)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "진료의사본과접수";
                return rtnVal;
            }

            //대리접수인데 소아6세 미만 제외
            if (ArgJin == "5" && ArgAge < 6)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "소아 만6세미만 대리접수";
                return rtnVal;
            }

            //소아과 소아6세 미만 제외
            if (ArgJin != "8" && ArgJin != "U" && ArgJin != "S" && ArgJin != "T" && ArgJin != "R" && ArgAge < 6)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "소아 만6세미만 소아접수R";
                return rtnVal;
            }

            //FM/DT는 일반자격 가능
            if (ArgDept != "DT" && ArgDept != "FM")
            {
                if (VB.Left(ArgBi, 1) != "1")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "무인수납은 건강보험자격만가능";
                    return rtnVal;
                }
            }
            else
            {
                if (VB.Left(ArgBi, 1) != "1" && ArgBi != "51")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "FM/DT무인수납은 건보,일반자격만가능";
                    return rtnVal;
                }
            }

            if (ArgBi == "21" || ArgBi == "22" || clsAuto.GstrAREquipQualification == "OK")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "의료급여점검";
                return rtnVal;
            }

            //정신과 원내조제의 경우 접수구분에 F003 확인
            if (ArgDept == "NP")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,                                  "); 
                sb.AppendLine("        b.SName                                                      "); 
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a,                                     "); 
                sb.AppendLine("        KOSMOS_PMPA.BAS_PATIENT b                                    "); 
                sb.AppendLine("  WHERE 1             = 1                                            "); 
                sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "'                            "); 
                sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   "); 
                sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                            "); 
                sb.AppendLine("    AND a.GBSunap     = '0'                                          "); //미수납만
                sb.AppendLine("    AND a.SuCode IN ( '##14' )                                       "); 
                sb.AppendLine("    AND a.Ptno        = b.Pano                                       "); 
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    if (ArgBi == "21" || ArgBi == "22")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "NP의료급여-##14코드점검";
                        DtAuto.Dispose();
                        DtAuto = null;
                        return rtnVal;
                    }

                    if (ArgVCode.Trim() != "F003")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "NP원내조제-F003코드점검";
                        DtAuto.Dispose();
                        DtAuto = null;
                        return rtnVal;
                    }
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //건강보험 기관기호가 의료급여일시 수납창구 확인
            if (ArgPtno != "")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PANO                                                       ");
                sb.AppendLine("   FROM KOSMOS_PMPA.opd_master a,                                    ");
                sb.AppendLine("        KOSMOS_PMPA.bas_patient b                                    ");
                sb.AppendLine("  WHERE 1             = 1                                            ");
                sb.AppendLine("    AND A.pano        = '" + ArgPtno + "'                            ");
                sb.AppendLine("    AND A.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
                sb.AppendLine("    AND A.reserved    = '1'                                          ");
                sb.AppendLine("    AND B.BI like '1%'                                               ");
                sb.AppendLine("    AND B.kiho        <> '00000000000'                               ");
                sb.AppendLine("    AND a.pano        = b.pano                                       ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "건강보험 예약자 자격 변경(보호)확인";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            if (ArgBi == "31" || ArgBi == "52" || ArgBi == "33" || ArgBi == "55")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "산재,교통점검";
                return rtnVal;
            }

            if (ArgVCode == "F003")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "F003점검";
                return rtnVal;
            }

            //차상위 E,F I,J 대상 체크
            if (ArgMCode == "E000" || ArgMCode == "F000" && (ArgJin != "I" && ArgJin != "J"))
            {
                if (clsAuto.M3_HIC_13_Cha2_AutoCHK(clsDB.DbCon, ArgPtno, ArgBi, ArgDept, clsPublic.GstrSysDate, ArgMCode, ArgVCode) == "OK")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "차상위환자I,J점검";
                    return rtnVal;
                }
            }

            //접수2제외,신생아,진단서,후불,결핵쿠폰
            if (ArgJin == "2" || ArgJin == "3" || ArgJin == "4" || ArgJin == "1" || ArgJin == "L")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "(" + ArgJin + ") 접수2,후불,결핵,전화..점검";
                return rtnVal;
            }

            strTemp = "";

            //전화예약 수납가능구분
            if (ArgJin == "E")
            {
                if (ArgVCode != "" || ArgMCode != "" || (ArgJinDtl != "" && ArgJinDtl != "05"))
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "전화예약점검ETC";
                    return rtnVal;
                }

                if (clsAuto.CHK_AR_TelephoneBooking(clsDB.DbCon, ArgPtno, ArgActdate, ArgDept) != "OK")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "전화예약점검C";
                    return rtnVal;
                }
            }

            strTemp = "";

            //초음파급여 오더체크
            sb.Clear();
            sb.AppendLine(" SELECT SUCODE,SUM(QTY*NAL)                                   ");
            sb.AppendLine("        FROM KOSMOS_OCS.OCS_OORDER                            ");
            sb.AppendLine("  WHERE 1        = 1                                          ");
            sb.AppendLine("    AND Ptno     = '" + ArgPtno + "'                          ");
            sb.AppendLine("    AND BDate    = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') "); 
            sb.AppendLine("    AND DeptCode = '" + ArgDept + "'                          ");
            sb.AppendLine("    AND GbSunap  = '0'                                        ");
            sb.AppendLine("    AND Nal      >  0                                         ");
            sb.AppendLine("    AND BUN IN ('49')                                         ");
            sb.AppendLine("  GROUP BY SUCODE                                             ");
            sb.AppendLine(" HAVING SUM(QTY*NAL) > 0                                      ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (ArgMCode == "V000" || ArgVCode == "V193")
                {
                }
                else
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "초음파급여코드점검";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }
            }

            DtAuto.Dispose();
            DtAuto = null;

            //XCDC(의무기록사본) 오더체크
            sb.Clear();
            sb.AppendLine(" SELECT SUCODE,SUM(QTY*NAL)                                      ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER                                    ");
            sb.AppendLine("  WHERE 1        = 1                                             ");
            sb.AppendLine("    AND Ptno     = '" + ArgPtno + "'                             ");
            sb.AppendLine("    AND BDate    = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')    ");
            sb.AppendLine("    AND DeptCode = '" + ArgDept + "'                             ");
            sb.AppendLine("    AND GbSunap  = '0'                                           ");
            sb.AppendLine("    AND Nal      >  0                                            ");
            sb.AppendLine("    AND (BUN IN ('75') OR SUCODE ='XCDC' OR SUCODE = 'XDVDC')    ");
            sb.AppendLine("  GROUP BY SUCODE                                                ");
            sb.AppendLine(" HAVING SUM(QTY*NAL) > 0                                         ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "제증명오더점검(의무기록사본)";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;


            if (ArgGelCode == "H023" || ArgGelCode == "H027" || ArgGelCode == "H122" || ArgGelCode == "H128")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "계약처점검";
                return rtnVal;
            }

            if (ArgDept == "ER")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "응급실점검";
                return rtnVal;
            }


            if (ArgDept == "HD")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "인공신장접수점검";
                return rtnVal;
            }

            //오더없음 체크
            if (ArgJin != "8" && ArgJin != "U" && ArgJin != "G" && ArgJin != "T")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,                                  ");
                sb.AppendLine("        b.SName                                                      ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a,                                     ");
                sb.AppendLine("        KOSMOS_PMPA.BAS_PATIENT b                                    ");
                sb.AppendLine("  WHERE 1             = 1                                            ");
                sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "'                            ");
                sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
                sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                            ");
                sb.AppendLine("    AND a.GBSunap     ='0'                                           "); //미수납만
                sb.AppendLine("    AND a.SuCode IS NOT NULL                                         "); //수가만   
                sb.AppendLine("    AND a.OrderCode   != 'PT######'                                  ");
                sb.AppendLine("    AND a.OrderCode   != 'NSA'                                       ");
                sb.AppendLine("    AND a.Ptno        = b.Pano                                       ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count == 0 && ArgRdate == "")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "오더발생없음";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName                           ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b           ");
            sb.AppendLine("  WHERE 1             = 1                                            ");
            sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "'                            ");
            sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
            sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                            ");
            sb.AppendLine("    AND a.GBSunap     = '0'                                          ");//미수납만
            sb.AppendLine("    AND a.NAL         < 0                                            ");
            sb.AppendLine("    AND a.SuCode IS NOT NULL                                         ");//수가만  
            sb.AppendLine("    AND a.Ptno        = b.Pano                                       ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "-오더발생점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //부가세 수가체크
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName                           "); 
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b           "); 
            sb.AppendLine("  WHERE 1             = 1                                            "); 
            sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "'                            "); 
            sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   "); 
            sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                            "); 
            sb.AppendLine("    AND a.GBSunap     = '0'                                          "); //미수납만
            sb.AppendLine("    AND a.GbTax       = '1'                                          "); 
            sb.AppendLine("    AND a.SuCode IS NOT NULL                                         "); //수가만
            sb.AppendLine("    AND a.Ptno        = b.Pano                                       "); 
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "부가세수가발생";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //당일과 약처방전 발생한후 추가 약처방있을경우 제외
            sb.Clear();
            sb.AppendLine(" SELECT Pano                                                     ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OUTDRUGMST                                ");
            sb.AppendLine("  WHERE SlipDate  = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
            sb.AppendLine("    AND Pano      = '" + ArgPtno + "'                            ");
            sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "'                            ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);                    

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName                           ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b           ");
                sb.AppendLine("  WHERE a.Ptno        = b.Pano                                       ");
                sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "'                            ");
                sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
                sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                            ");
                sb.AppendLine("    AND a.GBSunap     = '0'                                  "); //미수납만
                sb.AppendLine("    AND a.NAL         > 0                                            ");
                sb.AppendLine("    AND a.Bun IN ('11','12','20')                            "); //약만
                sb.AppendLine("    AND a.SuCode IS NOT NULL                                 "); //수가만
                SqlErr = clsDB.GetDataTableEx(ref DtSub, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return rtnVal;
                }

                if (DtSub.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "당일 약처방전발생후 약오더발생";
                    DtSub.Dispose();
                    DtSub = null;
                    return rtnVal;
                }

                DtSub.Dispose();
                DtSub = null;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //예방접종 코드 발생하면 인적정보 있어야함
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName                           ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b           ");
            sb.AppendLine("  WHERE a.Ptno        = b.Pano                                       ");
            sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "'                            ");
            sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD')   ");
            sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "'                            ");
            sb.AppendLine("    AND a.GBSunap     ='0'                                           ");  //미수납만
            sb.AppendLine("    AND a.SuCode IN ( SELECT JEPCODE FROM KOSMOS_OCS.ETC_JUSACODE WHERE (DELDATE IS NULL  OR DELDATE ='') )  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                sb.Clear();
                sb.AppendLine(" SELECT PANO                                 ");
                sb.AppendLine("   FROM KOSMOS_PMPA.VACCINE_TPATIENT         ");
                sb.AppendLine("  WHERE PANO          = '" + ArgPtno + "'    ");
                sb.AppendLine("    AND LAST_UPDATE   >= '" + VB.Replace(ArgActdate, "-", "").Trim() + "' ");
                SqlErr = clsDB.GetDataTableEx(ref DtSub, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    return rtnVal;
                }

                if (DtSub.Rows.Count == 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "예방접종기초정보점검";
                    DtSub.Dispose();
                    DtSub = null;
                    return rtnVal;
                }

                DtSub.Dispose();
                DtSub = null;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //필수예방접종 코드 발생하면 제외
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno        = b.Pano ");
            sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSunap     = '0' ");  //미수납만
            sb.AppendLine("    AND a.SuCode IN ( SELECT SuCode FROM KOSMOS_PMPA.BAS_VACC_MST WHERE  GUBUN ='1'  AND  (DELDATE IS NULL  OR DELDATE ='') )  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "필수예방접종점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //중증코드 점검
            if (ArgVCode == "V193")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.SuCode ='@V193' ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count == 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "접수V193-@V193점검";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //보호자 내원코드 점검
            if (ArgJinDtl == "05")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.SuCode = '$$42' ");  //보호자 내원코드 점검
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count == 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "보호자내원 코드 누락 점검($$42)";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //보호자 내원코드 점검
            if (ArgJinDtl != "05")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.SuCode = '$$42' ");  //보호자 내원코드 점검
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0 && ArgJinAmt1 > 0 )
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "진찰료수납후 보호자내원 코드 발생 점검($$42)";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }


            //$$ 특정코드 무인수납 점검
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno=b.Pano ");
            sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
            if (ArgJinDtl == "05")
            {
                sb.AppendLine("    AND a.SuCode IN ('$$20','$$35','$$33','$$34','$$52') ");
            }
            else
            {
                sb.AppendLine("    AND a.SuCode IN ('$$20','$$35','$$33','$$34','$$52') ");
            }
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "$$특정기호 코드 점검(" + DtAuto.Rows[0]["SUCODE"].ToString().Trim()  + ")";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //@F010 결핵지원금 점검
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno=b.Pano ");
            sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
            sb.AppendLine("    AND a.SuCode IN ('@F010','@F015','@F016') ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "@F코드점검(" + DtAuto.Rows[0]["SUCODE"].ToString().Trim()  + ")";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            if (ArgDept == "FM" && ArgDrCode == "1404")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.GbFM ='1' ");   //분리수납건
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "FM-분리수납점검";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //$$40, $$41 점검
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno=b.Pano ");
            sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
            sb.AppendLine("    AND a.SuCode IN ('$$40','$$41') ");  //보험총액43,일반자격51 수납요청
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count == 1)
            {
                strTemp = DtAuto.Rows[0]["SUCODE"].ToString().Trim();

                if (strTemp == "$$40" && ArgBi != "43")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "접수구분변경-43종아님($$40)";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }
                else if (strTemp == "$$41" && ArgBi != "51")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "접수구분변경-51종아님($$41)";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }
            }
            else if (DtAuto.Rows.Count > 1)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "접수구분변경_수가확인($$40,$$41)";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //당일 ER오더있으면 제외됨
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_iORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno=b.Pano ");
            sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.GbIOE='E' "); //ER 오더
            sb.AppendLine("    AND a.GbAct ='*' ");  //미수납만
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "당일응급실오더발생";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //중증화상코드 점검
            if (ArgVCode == "V247" || ArgVCode == "V248" || ArgVCode == "V249" || ArgVCode == "V250")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.SuCode IN ('@V247','@V248','@V249','@V250' ) ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count == 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "접수화상-V코드점검";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //결핵접수체크
            if (VB.Left(ArgBi, 1) == "1" && ArgVCode != "EV01" && ArgVCode != "V001")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.SuCode IN ('@V206','@V246','@V000' ) ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "접수기본-결핵수가발생점검";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //상병특례 + 희귀난치 체크

            if (VB.Left(ArgBi, 1) == "1")
            {
                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND TRIM(a.SuCode) IN (  SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE (GBRARE <> 'Y' OR GBRARE IS NULL)  AND SUBSTR(SUNEXT,1,2) = '@V'  AND SUNEXT NOT IN ('@V193','@V194','@V247','@V248','@V249','@V250','@V252') ) ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                    nBuCount = 1;

                DtAuto.Dispose();
                DtAuto = null;

                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND TRIM(a.SuCode) IN (  SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE GBRARE = 'Y' AND SUBSTR(SUNEXT,1,2) = '@V'  AND SUNEXT NOT IN ('@V247','@V248','@V249','@V250','@V252') ) ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                    nBuCount3 = 1;

                DtAuto.Dispose();
                DtAuto = null;

                sb.Clear();
                sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
                sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
                sb.AppendLine("  WHERE a.Ptno=b.Pano ");
                sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
                sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
                sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
                sb.AppendLine("    AND a.SuCode IN ('@V193','@V252') ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count == 0)
                {
                    if (ArgJin != "F" && ArgJin != "G" && ArgJin != "A" && ArgJin != "C" && ArgAge >= 6)
                    {
                        if (ArgMCode != "C000" && ArgMCode == "E000" && ArgMCode == "F000")
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "상병특례수가발생-접수점검";
                            DtAuto.Dispose();
                            DtAuto = null;
                            return rtnVal;
                        }
                    }
                }

                DtAuto.Dispose();
                DtAuto = null;

                if (ArgMCode == "H000")
                {
                    if (ArgJin != "F" && ArgJin != "G" && ArgJin != "S" && ArgJin != "T" && ArgJin != "A" && ArgJin != "C" && ArgJin != "6")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치H접수구분점검";
                        return rtnVal;
                    }
                    else if (nBuCount3 == 0)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치H코드점검";
                        return rtnVal;
                    }
                    else if (nBuCount >= 1)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치H-상병특례코드발생점검";
                        return rtnVal;
                    }
                    else if (nBuCount >= 1 && nBuCount3 >= 1)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치H코드+상병특례코드발생점검";
                        return rtnVal;
                    }
                }
                else if (ArgMCode == "")
                {
                    if (nBuCount == 0 && (ArgJin == "F" || ArgJin == "G" || ArgJin == "S" || ArgJin == "T" || ArgJin == "C"))
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치상병특례수가점검1";
                        return rtnVal;
                    }
                    else if (nBuCount >= 1 && ArgJin != "F" && ArgJin != "G" && ArgJin != "T" && ArgJin != "T" && ArgJin != "C")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치상병특례수가점검2";
                        return rtnVal;
                    }
                    else if (nBuCount3 >= 1)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치상병특례수가발생점검";
                        return rtnVal;
                    }
                }

                //등록 희귀난치질환 점검
                if (ArgMCode == "V000")
                {
                    if (ArgJin != "F" && ArgJin != "G" && ArgJin != "S" && ArgJin != "T" && ArgJin != "A" && ArgJin != "C" && ArgJin != "6" && ArgJin != "9" && ArgJin != "8")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치V-접수구분점검";
                        return rtnVal;
                    }
                    else if (nBuCount3 == 0)
                    {
                        if (ArgDept != "HD")
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "희귀난치V점검";
                            return rtnVal;
                        }

                    }
                    else if (nBuCount >= 1)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치V-상병특례발생점검";
                        return rtnVal;
                    }
                    else if (nBuCount >= 1 && nBuCount3 >= 1)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "희귀난치V+상병특례발생점검";
                        return rtnVal;
                    }
                }
                else if (ArgMCode == "")
                {
                    if (nBuCount >= 1 && ArgJin != "F" && ArgJin != "G" && ArgJin != "S" && ArgJin != "T" && ArgJin != "C")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "상병특례발생점검A";
                        return rtnVal;
                    }
                }
            }

            //협력병원 예외적용
            sb.Clear();
            sb.AppendLine(" SELECT  ROWID  FROM KOSMOS_PMPA.ETC_RETURN ");
            sb.AppendLine("  WHERE PANO ='" + ArgPtno + "' ");
            sb.AppendLine("    AND H_CODE IN ( SELECT CODE FROM KOSMOS_PMPA.ETC_RETURN_CODE WHERE GUBUN ='01' AND H_GUBUN ='Y' ) ");
            sb.AppendLine("    AND ACTDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "협력병의원의뢰환자점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //예약자 자격점검
            strTemp = "";
            int nAge = 0;

            if (ArgRes == "1" && ArgActdate == clsPublic.GstrSysDate)
            {
                strTemp = clsAuto.READ_OPD_NHIC_Qualification_2(clsDB.DbCon, clsPublic.GstrSysDate, ArgPtno, ArgBi, ArgDept, "", "1");

                if (strTemp != "")
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "예약자자격점검";
                    return rtnVal;
                }

                if (ArgAge == 5 && VB.Left(ArgBi, 1) == "1")
                {
                    nAge = ComFunc.AgeCalcEx(ArgJumin, ArgActdate);

                    if (nAge > ArgAge && nAge == 6)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "예약자나이(소아)체크";
                        return rtnVal;
                    }
                }
            }

            //접수된것 감액을 실제감액 점검
            switch (ArgGam)
            {
                case "11":
                case "12":
                case "13":
                case "14":
                case "21":
                case "22":
                case "23":
                case "24":
                case "26":
                case "27":

                    if (clsAuto.Gam_Pano_Search_2(clsDB.DbCon, ArgGam, VB.Left(ArgJumin, 6), VB.Right(ArgJumin, 7)) != "OK")
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "감액대상체크";
                        return rtnVal;
                    }

                    break;
            }

            if (ArgRdate.Trim() != "")
            {
                if (cF.DATE_HUIL_CHECK(clsDB.DbCon, ArgRdate) == true)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "공휴일예약점검";
                    return rtnVal;
                }
            }

            //의료급여 승인조 점검
            if (ArgBi == "21" || ArgBi == "22")
            {
                sb.Clear();
                sb.AppendLine(" SELECT SUM(AMT)AMT, PART FROM CARD_APPROV_BI ");
                sb.AppendLine("  WHERE BDATE     = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND PANO      = '" + ArgPtno + "' ");
                sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "' ");
                sb.AppendLine("    AND GbBun     = '1' ");
                sb.AppendLine("  GROUP BY PART ");
                sb.AppendLine(" HAVING SUM(AMT) <> 0 ");
                sb.AppendLine("  UNION ALL ");
                sb.AppendLine(" SELECT SUM(AMT1)AMT, PART FROM CARD_APPROV_BI ");
                sb.AppendLine("  WHERE BDATE     = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND PANO      = '" + ArgPtno + "' ");
                sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "' ");
                sb.AppendLine("    AND GbBun     = '2' ");
                sb.AppendLine("  GROUP BY PART ");
                sb.AppendLine("  HAVING SUM(AMT1) <> 0 ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    if (DtAuto.Rows[0]["PART"].ToString().Trim() != clsType.User.IdNumber.Trim())
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "의료급여-금액발생작업조점검";
                        DtAuto.Dispose();
                        DtAuto = null;
                        return rtnVal;
                    }
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            //접수비 점검
            //의료급여 1,2  차상위 E,F 치매검사
            if ((ArgBi == "21") || (ArgBi == "22" && ArgJin != "2" && ArgMCode != "B099") || ((ArgBi == "11" || ArgBi == "12" || ArgBi == "13") && (ArgMCode == "E000" || ArgMCode == "F000" || ArgDementia == "Y")) )
            {
                if (VB.Left(ArgBi, 1) == "1" && (ArgMCode == "E000" || ArgMCode == "F000" || ArgDementia == "Y"))
                {

                }
                else
                {
                    sb.Clear();
                    sb.AppendLine(" SELECT NVL(SUM(NAL), 0) CNT FROM KOSMOS_PMPA.OPD_SLIP");
                    sb.AppendLine("  WHERE PANO      = '" + ArgPtno + "' ");
                    sb.AppendLine("    AND BDATE     = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
                    sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "' ");
                    sb.AppendLine("    AND Bi        = '" + ArgBi + "'");
                    sb.AppendLine("    AND SUNEXT IN (SELECT SUNEXT FROM KOSMOS_PMPA.BAS_ACCOUNT_CONV ");
                    sb.AppendLine("                    WHERE GUBUN   = '1' ");
                    sb.AppendLine("                      AND SDATE   <= TO_DATE('" + ArgActdate + "','YYYY-MM-DD')) ");
                    SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtAuto.Dispose();
                        DtAuto = null;
                        return rtnVal;
                    }


                    if (Convert.ToInt32(DtAuto.Rows[0]["CNT"].ToString()) == 0)
                    {
                        rtnVal = "NO";
                        clsAuto.GstrAREquipExceptionCheck = "후불진찰료점검";
                        DtAuto.Dispose();
                        DtAuto = null;
                        return rtnVal;
                    }

                    DtAuto.Dispose();
                    DtAuto = null;
                }
            }

            //B형간염바이러스제 점검
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno        = b.Pano ");
            sb.AppendLine("    AND a.Ptno        = '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE       = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE    = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSELF      = '0' ");  //급여만 해당됨
            sb.AppendLine("    AND a.GBSunap     = '0' ");  //미수납만
            sb.AppendLine("    AND TRIM(a.SUCODE) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine("                             WHERE GUBUN = 'ETC_B형간염약제체크'  ");
            sb.AppendLine("                               AND (DELDATE ='' OR DELDATE IS NULL ) ) ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "B형간염바이러스제체크";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //고가약관리 점검(뇌하수체 호르몬제, 알부민주사,혈우병약제?
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno=b.Pano ");
            sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSELF ='0' ");  //급여만 해당됨
            sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
            sb.AppendLine("    AND TRIM(a.SUCODE) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine("                             WHERE GUBUN ='ETC_고가약제관리'  ");
            sb.AppendLine("                               AND (DELDATE ='' OR DELDATE IS NULL ) ) ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "고가약제관리체크";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;


            //항암제
            sb.Clear();
            sb.AppendLine(" SELECT a.PTNO,a.DEPTCODE,a.SuCode,b.SName ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Ptno=b.Pano ");
            sb.AppendLine("    AND a.Ptno= '" + ArgPtno + "' ");
            sb.AppendLine("    AND a.BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND a.DEPTCODE = '" + ArgDept + "' ");
            sb.AppendLine("    AND a.GBSELF ='0' ");  //급여만 해당됨
            sb.AppendLine("    AND a.GBSunap ='0' ");  //미수납만
            sb.AppendLine("    AND TRIM(a.SUCODE) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine("                             WHERE GUBUN ='ETC_특수항암제체크'  ");
            sb.AppendLine("                               AND (DELDATE ='' OR DELDATE IS NULL ) ) ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "특수항암제체크";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //내시경 예약오더 및 당일대체 체크
            sb.Clear();
            sb.AppendLine(" SELECT  ROWID  FROM KOSMOS_OCS.OCS_OORDER ");
            sb.AppendLine("  WHERE PTNO ='" + ArgPtno + "' ");
            sb.AppendLine("    AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("    AND RES ='1' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "당일내시경예약오더점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;


            //내시경예약선수금 있다면 
            sb.Clear();
            sb.AppendLine(" SELECT ROWID  FROM KOSMOS_PMPA.OPD_RESERVED_EXAM ");
            sb.AppendLine("  WHERE PANO ='" + ArgPtno + "' ");
            sb.AppendLine("    AND GbEnd ='N' ");
            sb.AppendLine("    AND TransDate IS NULL ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("    AND GUBUN ='01' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "내시경선수금대상자";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //당일 대체 했다면
            sb.Clear();
            sb.AppendLine(" SELECT ROWID  FROM KOSMOS_PMPA.OPD_RESERVED_EXAM ");
            sb.AppendLine("  WHERE PANO ='" + ArgPtno + "' ");
            sb.AppendLine("    AND TRUNC(TRANSDATE) =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("    AND GUBUN ='01' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "당일내시경대체자점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //치과완전틀니
            if (ArgDept == "DT" && ArgJinDtl == "02")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "완전틀니대상점검";
                return rtnVal;
            }


            sb.Clear();
            sb.AppendLine(" SELECT ROWID  FROM KOSMOS_OCS.OCS_OORDER ");
            sb.AppendLine("  WHERE PTNO ='" + ArgPtno + "' ");
            sb.AppendLine("    AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("    AND TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE DTLBUN ='4004')  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "치과틀니수가점검4004";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            sb.Clear();
            sb.AppendLine(" SELECT ROWID  FROM KOSMOS_OCS.OCS_OORDER ");
            sb.AppendLine("  WHERE PTNO ='" + ArgPtno + "' ");
            sb.AppendLine("    AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("    AND TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE DTLBUN ='4003')  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "치과임플란트수가점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //장루,요루
            if (ArgJinDtl == "01")
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "장루,요루대상점검";
                return rtnVal;
            }

            sb.Clear();
            sb.AppendLine(" SELECT ROWID  FROM KOSMOS_OCS.OCS_OORDER ");
            sb.AppendLine("  WHERE PTNO = '" + ArgPtno + "' ");
            sb.AppendLine("    AND BDATE = TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("    AND TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE DTLBUN ='1001')  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "장루요루 수가점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //치매
            //if (ArgDementia == "Y")
            //{
            //    rtnVal = "NO";
            //    clsAuto.GstrAREquipExceptionCheck = "치매대상점검";
            //    return rtnVal;
            //}

            //sb.Clear();
            //sb.AppendLine(" SELECT  ROWID  FROM KOSMOS_OCS.OCS_OORDER ");
            //sb.AppendLine(" WHERE PTNO ='" + ArgPtno + "' ");
            //sb.AppendLine("  AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            //sb.AppendLine("  AND DEPTCODE ='" + ArgDept + "' ");
            //sb.AppendLine("  AND TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE GBDEMENTIA ='Y' )  ");
            //SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    DtAuto.Dispose();
            //    DtAuto = null;
            //    return rtnVal;
            //}


            //if (DtAuto.Rows.Count > 0)
            //{
            //    rtnVal = "NO";
            //    clsAuto.GstrAREquipExceptionCheck = "치매약제점검";
            //    DtAuto.Dispose();
            //    DtAuto = null;
            //    return rtnVal;
            //}

            //DtAuto.Dispose();
            //DtAuto = null;

            //안저
            sb.Clear();
            sb.AppendLine(" SELECT  ROWID  FROM KOSMOS_PMPA.OPD_MASTER ");
            sb.AppendLine(" WHERE PANO ='" + ArgPtno + "' ");
            sb.AppendLine("  AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("  AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("  AND GBOT ='Y'  "); // 안과검진
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "안과검진점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            // 2018-01-03 조산및저체중아(@F016) 임산부외래(@F015)
            sb.Clear();
            sb.AppendLine(" SELECT  ROWID  FROM KOSMOS_PMPA.OPD_MASTER ");
            sb.AppendLine(" WHERE PANO ='" + ArgPtno + "' ");
            sb.AppendLine("  AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("  AND DEPTCODE ='" + ArgDept + "' ");
            sb.AppendLine("  AND JINDTL in ('22','25')  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
                clsAuto.GstrAREquipExceptionCheck = "조산및저체중아(@F016) 임산부외래(@F015)점검";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            //개인미수금잔액 있는분 (미수구분 11,13,14는 제외함) 2015-02-10
            sb.Clear();
            sb.AppendLine("SELECT  JAmt FROM  KOSMOS_PMPA.Misu_GAINMST ");
            sb.AppendLine(" WHERE Pano  = '" + ArgPtno + "'");
            sb.AppendLine("   AND JAmt > 0 ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }


            if (DtAuto.Rows.Count > 0)
            {
                sb.Clear();
                sb.AppendLine("SELECT MISUDTL FROM KOSMOS_PMPA.Misu_GAINSLIP ");
                sb.AppendLine(" WHERE Pano  = '" + ArgPtno + "'");
                sb.AppendLine("   AND GUBUN1 = '1' ");
                sb.AppendLine("   AND SUBSTR(MISUDTL,4,2) NOT IN ('11','13','14','15') ");
                sb.AppendLine("   AND FLAG <> '*' ");
                SqlErr = clsDB.GetDataTableEx(ref DtSub, sb.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtSub.Rows.Count > 0)
                {
                    rtnVal = "NO";
                    clsAuto.GstrAREquipExceptionCheck = "개인미수금점검";
                    DtSub.Dispose();
                    DtSub = null;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;

                }
            }

            DtAuto.Dispose();
            DtAuto = null;


            //2014-11-21 만성질환대상자 무인수납체크
            sb.Clear();
            sb.AppendLine("SELECT CHOJAE,Amt1 FROM  KOSMOS_PMPA.OPD_MASTER ");
            sb.AppendLine(" WHERE PANO ='" + ArgPtno + "' ");
            sb.AppendLine("  AND BDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD') ");
            sb.AppendLine("  AND DEPTCODE ='" + ArgDept + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                clsPmpaPb.GnDrugNal = 0;
                strChoJae = DtAuto.Rows[0]["chojae"].ToString().Trim();

                if (strChoJae == "1")
                {
                    strTemp = cOC.Check_ChronicIll(clsDB.DbCon, ArgPtno, ArgDept, ArgActdate);

                    if (strTemp != "")
                    {
                        strTemp2 = VB.Pstr(strTemp, "@@", 1);

                        if (string.Compare(cF.DATE_ADD(clsDB.DbCon, strTemp2, clsPmpaPb.GnDrugNal + 90 ), ArgActdate ) > 0)
                        {
                            rtnVal = "NO";
                            clsAuto.GstrAREquipExceptionCheck = "만성질환자 재진료점검";
                            DtAuto.Dispose();
                            DtAuto = null;
                            return rtnVal;
                        }
                    }
                }
            }

            DtAuto.Dispose();
            DtAuto = null;


            if (rtnVal == "")
                clsAuto.GstrAR_RDate = ArgRdate + " " + ArgRTime;

            if (rtnVal != "NO" && clsAuto.GstrAREquipExceptionCheck == "")
                clsAuto.GstrAREquipCheckSentence = "무인수납 가능";


            return rtnVal;
        }


        /// <summary>
        /// Description : 무인수납장비_Lock
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <param name="pDbCon"></param>
        /// <param name="ArgValue">등록번호</param>
        /// <seealso cref="무인수납.bas : CHK_무인수납_LOCK"/>
        /// </summary>

        public string Check_Machine_Lock(PsmhDb pDbCon, string ArgValue)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";
            int intRowCnt = 0;

            string rtnVal = "OK";

            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_OCS.OCS_OLOCK ");
            sb.AppendLine(" WHERE PTNO = '" + ArgValue + "' ");
            sb.AppendLine("   AND TRUNC(ENTDATE) = TRUNC(SYSDATE) ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "NO";
            }
            else
            {
                rtnVal = "OK";

                clsDB.setBeginTran(pDbCon);

                try
                {
                    sb.Clear();
                    sb.AppendLine("INSERT INTO KOSMOS_OCS.OCS_OLOCK(PTNO,REMARK,ENTDATE) VALUES ( ");
                    sb.AppendLine(" '" + ArgValue + "', '수납중입니다(auto)', SysDate) ");
                    SqlErr = clsDB.ExecuteNonQuery(sb.ToString(), ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (intRowCnt == 0)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("Check_Machine_Lock 오류발생.", "알림");
                        return rtnVal;
                    }

                    clsDB.setCommitTran(pDbCon);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, sb.ToString(), pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : CHK_무인수납_History
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgJin"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDr"></param>
        /// <param name="ArgMcode"></param>
        /// <param name="ArgVcode"></param>
        /// <param name="ArgMsg"></param>
        /// <seealso cref="무인수납.bas : CHK_무인수납_History"/>
        /// </summary>

        public void Check_Machine_His(PsmhDb pDbCon, string ArgGubun, string ArgPtno, string ArgJin, string ArgBi, string ArgDept, string ArgDr, string ArgMcode, string ArgVcode, string ArgMsg)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";
            int intRowCnt = 0;

            ComFunc cF = new ComFunc();
            clsDB.setBeginTran(pDbCon);
            
            try
            {
                sb.Clear();
                sb.AppendLine(" INSERT INTO KOSMOS_PMPA.ETC_SUNAP_AHIS ");
                sb.AppendLine("        (BDATE,PANO,GUBUN, ");
                sb.AppendLine("         DEPTCODE,DRCODE,JIN, ");
                sb.AppendLine("         BI,MCODE,VCODE, ");
                sb.AppendLine("         MSG,ENTSABUN,ENTDATE )");
                sb.AppendLine(" VALUES (TRUNC(SYSDATE), ");
                sb.AppendLine("         '" + ArgPtno + "', ");
                sb.AppendLine("         '" + ArgGubun + "', ");
                sb.AppendLine("         '" + ArgDept + "', ");
                sb.AppendLine("         '" + ArgDr + "', ");
                sb.AppendLine("         '" + ArgJin + "', ");
                sb.AppendLine("         '" + ArgBi + "', ");
                sb.AppendLine("         '" + ArgMcode  + "', ");
                sb.AppendLine("         '" + ArgVcode + "', ");
                sb.AppendLine("         '" + cF.Quotation_Change(ArgMsg) + "', ");
                sb.AppendLine("          " + clsPublic.GnJobSabun + ", ");
                sb.AppendLine("         SYSDATE  )  ");
                SqlErr = clsDB.ExecuteNonQuery(sb.ToString(), ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    cF = null;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (intRowCnt == 0)
                {
                    cF = null;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("Check_Machine_His 오류발생.", "알림");
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                cF = null;
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, sb.ToString(), pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgBi">등록번호</param>
        /// <param name="ArgDept">등록번호</param>
        /// <param name="ArgBDate">등록번호</param>
        /// <param name="ArgMCode">등록번호</param>
        /// <param name="ArgVCode">등록번호</param>
        /// <seealso cref="무인수납.bas : M3_HIC_13_차상위2_무인CHK"/>
        /// </summary>

        public string Check_Machine_M3_HIC(PsmhDb pDbCon, string ArgPtno, string ArgBi, string ArgDept, string ArgBDate, string ArgMCode, string ArgVCode)
        {
            DataTable DtAuto = new DataTable();            
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";
            
            string rtnVal = "";
            string strDrugBunup2 = "";
            string strBunup = "";
            string strBunup2 = "";
            string strJupsuCode2 = "OK";
            string strGemsa2 = "";
            int nBuCount = 0;
            int nBuCount2 = 0;
            int nReadM3 = 0;
            string strBun = "";
            string strSucode = "";
            string strSelf = "";
            string strSunap = "";

            if (VB.Left(ArgBi, 1) != "1") { return rtnVal; }
            if (ArgMCode != "E000" && ArgMCode != "F000") { return rtnVal; }

            sb.Clear();
            sb.AppendLine(" SELECT SUCODE,BUN,GBSUNAP,GbSelf ");
            sb.AppendLine("   FROM KOSMOS_OCS.OCS_OORDER ");
            sb.AppendLine("  WHERE PTNO      = '" + ArgPtno + "' ");
            sb.AppendLine("    AND BDATE     = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            nReadM3 = DtAuto.Rows.Count;

            for (int i = 0; i < nReadM3; i++)
            {
                strBun = DtAuto.Rows[i]["BUN"].ToString().Trim();
                strSucode = DtAuto.Rows[i]["SuCode"].ToString().Trim();
                strSelf = DtAuto.Rows[i]["GbSelf"].ToString().Trim();
                strSunap = DtAuto.Rows[i]["GbSunap"].ToString().Trim();

                if (strSucode != "")
                {
                    switch (strSucode)
                    {
                        case "@V001":
                        case "@V003":
                        case "@V005":
                        case "@V117":
                        case "@V027":
                        case "@V009":
                        case "@V012":
                        case "@V013":
                        case "@V014":
                        case "@V015":
                        case "@V191":
                        case "@V192":
                        case "@V193":
                        case "@V194":
                            nBuCount += 1;
                            break;
                    }

                    if (string.Compare(strBun, "80") < 0 && (VB.Left(strSucode, 2) != "$$" && VB.Left(strSucode, 2) != "##" && VB.Left(strSucode, 2) != "@V") && (strBun != "11" && strBun != "12" && strBun != "20"))
                    {
                        strGemsa2 = "OK";
                    }

                    //미수납만 점검
                    if ( strSunap == "0")
                    {
                        if (ArgVCode == "EV00" || ArgVCode == "V206" || ArgVCode == "V231" || ArgVCode == "V246")
                        {
                            if (clsAuto.READ_RareIncurableVCode_2(pDbCon, strSucode) == "OK") { nBuCount = nBuCount + 1; }
                        }

                        if (strSucode == "@V193" || strSucode == "@V194")
                            nBuCount2 = 1;

                        if (strBun == "11" || strBun == "12" || strBun == "20")
                            strBunup2 = "OK";
                        else if ((strSucode == "E7660" || strSucode == "E7660S" || strSucode == "E7630" || strSucode == "E7630S") && (strSelf == "0" || strSelf == ""))
                            strDrugBunup2 = "OK"; //원내조제
                        else if (strSucode == "O9991")
                            strDrugBunup2 = "OK"; //원내조제(HD환자 O9991이 수가코드 발생시 원내조제에 들어감
                        else if (strBun == "02" || strBun == "01" || strBun == "75") //진찰료, 증명료
                            strJupsuCode2 = "OK";
                        else if (strSucode == "AY100") //가정간호 기본방문료가 진찰료임.
                            strJupsuCode2 = "OK";
                    }
                }
            }

            DtAuto.Dispose();
            DtAuto = null;

            sb.Clear();
            sb.AppendLine(" SELECT NVL(SUM(QTY*NAL), 0) CNT FROM KOSMOS_PMPA.OPD_SLIP ");
            sb.AppendLine("  WHERE PANO      = '" + ArgPtno + "' ");
            sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "' ");
            sb.AppendLine("    AND BDATE     = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ");
            sb.AppendLine("    AND BUN IN    ('11','12') ");
            sb.AppendLine("    AND GBBUNUP   = '0' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtAuto.Rows[0]["CNT"].ToString()) > 0)
                    strBun = "Y";
                else
                    strBun = "N";
            }

            DtAuto.Dispose();
            DtAuto = null;

            if (nBuCount == 0) { return rtnVal; }

            if (strBunup2 == "OK")
                rtnVal = "OK";
            else if (strJupsuCode2 == "OK" && strDrugBunup2 == "" && strGemsa2 == "")
                rtnVal = "OK";
            else if (strJupsuCode2 == "OK" && strDrugBunup2 == "OK")
                rtnVal = "OK";
            else if (strJupsuCode2 == "OK" && strGemsa2 == "OK")
                rtnVal = "OK";
            else
                rtnVal = "OK";

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <param name="ArgGubun"></param>
        /// <seealso cref="무인수납.bas : 무인장비수납_SHELL_CHK"/>
        /// </summary>
        public void Check_Machine_Shell(string ArgGubun)
        {
            if (clsAuto.GstrAREquipUse != "OK") { return; }

            IntPtr hWnd = FindWindow(null, ArgGubun);

            //이미 폼이 떠있는지 확인한다.
            if (!hWnd.Equals(IntPtr.Zero))
            {
                // 떠있는 화면 종료
                SendMessage(hWnd, 0x0010, 0, 0);
            }
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <param name="ArgGubun"></param>
        /// <param name="ArgMsg"></param>
        /// <param name="ArgMsg2"></param>
        /// <seealso cref="무인수납.bas : 무인수납장비정보창_POP"/>
        /// </summary>
        public string Check_Machine_PopUp(string ArgGubun, string ArgMsg, string ArgMsg2)
        {
            string rtnVal = "";
            string strFile = "";
            if (clsAuto.GstrAREquipUse != "OK") { return rtnVal; }

            strFile = @"C:\PSMHEXE\PSMHAutoUpdate.ini";
            if (File.Exists(strFile) == false)
            {
                Check_Machine_Shell("무인수납장비정보창");

                VB.Shell(@"c:\cmc\exe\oumsad_msg.exe " + " " + ArgGubun + "^^" + ArgMsg + "^^" + ArgMsg2, "NormalFocus");
            }


            return rtnVal;
        }

        public string Check_Machine_PopUp_New(string ArgGubun, string ArgMsg, string ArgMsg2)
        {
            string rtnVal = "";

            if (clsAuto.GstrAR_PopUpVariable == "")
            {
                clsAuto.GstrAR_PopUpVariable = ArgGubun + "^^" + ArgMsg + "^^" + ArgMsg2;
            }

            if (clsAutoAcct.frmAutoInfomationX == null)
            {
                clsAutoAcct.frmAutoInfomationX = new frmAutoInfomation();
                clsAutoAcct.frmAutoInfomationX.rEventClosed += clsAutoAcct.frmAutoInfomationX_rEventClosed;
                clsAutoAcct.frmAutoInfomationX.StartPosition = FormStartPosition.CenterParent;
                clsAutoAcct.frmAutoInfomationX.WindowState = FormWindowState.Maximized;
            }

            clsAutoAcct.frmAutoInfomationX.ShowDialog();

            //frmAutoInfomation frm = new frmAutoInfomation();
            //frm.StartPosition = FormStartPosition.CenterParent;
            //frm.WindowState = FormWindowState.Maximized;
            //frm.ShowDialog();

            clsAuto.GstrAR_PopUpVariable = "";            
            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_OPD_NHIC_자격점검_2"/>
        /// </summary>
        public string Read_Opd_NHIC(PsmhDb pDbCon, string ArgPtno, string ArgBi, string ArgDept, string ArgDate, string ArgType, string ArgGubun)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            clsVbfunc cVF = new clsVbfunc();
            string rtnVal = "";

            if (ArgBi == "21" || ArgBi == "22") { return rtnVal; }

            string str_NHIC_MCODE = "";
            string strNHIC = "";
            string strM_Jin = "";
            string strM_Bi = "";
            string strM_MCode = "";
            string strM_VCode = "";
            string strRegs1 = "";
            string strRegs2 = "";
            string strRegs3 = "";
            string strRegs4 = "";
            string strRegs1_2 = "";
            string strRegs2_2 = "";
            string strRegs3_2 = "";
            string strRegs4_2 = "";
            string strSangSil = "";
            string strOK = "OK";
            string strOK2 = "";
            int j = 0;

            if (ArgGubun == "1")
            {
                strOK = "";

                sb.Clear();
                sb.AppendLine(" SELECT Pano,Bi,MCode,VCode,Jin ");
                sb.AppendLine("   FROM KOSMOS_PMPA.OPD_MASTER ");
                sb.AppendLine("  WHERE PANO      = '" + ArgPtno + "' ");
                sb.AppendLine("    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ");
                sb.AppendLine("    AND DEPTCODE  = '" + ArgDept + "' ");
                sb.AppendLine("    AND RESERVED  = '1' ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    strOK = "OK";

                    strM_Jin = DtAuto.Rows[0]["Jin"].ToString().Trim();
                    strM_Bi = DtAuto.Rows[0]["Bi"].ToString().Trim();
                    strM_MCode = DtAuto.Rows[0]["MCode"].ToString().Trim();
                    strM_VCode = DtAuto.Rows[0]["VCode"].ToString().Trim();
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            if (strOK != "OK") { return rtnVal; }

            sb.Clear();
            sb.AppendLine(" SELECT PANO,M2_JAGEK,M2_CDATE,M2_SUJIN_NAME,              ");
            sb.AppendLine(" M2_SEDAE_NAME,M2_KIHO,M2_GKIHO,M2_SANGSIL,                ");
            sb.AppendLine(" M2_BONIN,M2_GJAN_AMT,M2_CHULGUK,M2_JANG_DATE,             ");
            sb.AppendLine(" M2_SHOSPITAL1,M2_SHOSPITAL2,M2_SHOSPITAL3,                ");
            sb.AppendLine(" M2_SHOSPITAL4,M2_SHOSPITAL_NAME1,M2_SHOSPITAL_NAME2,      ");
            sb.AppendLine(" M2_SHOSPITAL_NAME3,M2_SHOSPITAL_NAME4,JOB_STS,            ");
            sb.AppendLine(" M2_DISREG1,M2_DISREG2,M2_DISREG3,M2_DISREG4,              ");
            sb.AppendLine(" TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, M2_REMAMT              ");
            sb.AppendLine(" FROM KOSMOS_PMPA.OPD_NHIC                                 ");
            sb.AppendLine("  WHERE PANO ='" + ArgPtno + "'                            ");
            sb.AppendLine("    AND ACTDATE =TO_DATE('" + ArgDate + "','YYYY-MM-DD')   ");
            sb.AppendLine("    AND JOB_STS ='2'                                       ");
            sb.AppendLine("    AND DEPTCODE ='" + ArgDept + "'                        ");
            sb.AppendLine("   ORDER BY SENDTIME DESC                                  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                strSangSil = DtAuto.Rows[0]["M2_SANGSIL"].ToString().Trim();

                //희귀난치대상자 H000
                if (strRegs1 == DtAuto.Rows[0]["M2_DISREG1"].ToString().Trim())
                {
                    str_NHIC_MCODE += VB.Left(strRegs1, 1) + "^^";

                    strRegs1_2 = VB.Left(strRegs1, 4) + "@@";
                    strRegs1_2 += VB.Mid(strRegs1, 5, 8).Trim() + "@@";
                    strRegs1_2 += cVF.Date_Format(VB.Mid(strRegs1, 5, 8)) + "@@";
                    strRegs1_2 += VB.Mid(strRegs1, 13, 8);

                    for (int i = 21; i <= 45; i = i + 5)
                    {
                        strRegs1_2 += VB.Mid(strRegs1, i, 5) + "@@";
                        j += 1;
                    }
                }

                //산정특례(희귀)등록대상자 V000
                strRegs2 = DtAuto.Rows[0]["M2_DISREG2"].ToString().Trim();

                if (strRegs2 != "")
                {
                    str_NHIC_MCODE += VB.Left(strRegs2, 1) + "^^";
                    strRegs2_2 = VB.Left(strRegs2, 4).Trim() + "@@";
                    strRegs2_2 += VB.Mid(strRegs2, 20, 8).Trim() + "@@";
                    strRegs2_2 += cVF.Date_Format(VB.Mid(strRegs2, 20, 8)).Trim() + "@@";
                    strRegs2_2 += VB.Mid(strRegs2, 28, 8).Trim() + "@@";
                    strRegs2_2 += VB.Mid(strRegs2, 5, 15).Trim() + "@@";
                }

                //차상위대상자 C000,E000,F000
                strRegs3 = DtAuto.Rows[0]["M2_DISREG3"].ToString().Trim();

                if (strRegs3 != "")
                {
                    str_NHIC_MCODE += VB.Left(strRegs3, 1) + "^^";
                    strRegs3_2 = VB.Left(strRegs3, 4).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3, 5, 8).Trim() + "@@";
                    strRegs3_2 += cVF.Date_Format(VB.Mid(strRegs3, 5, 8)).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3, 13, 8).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3, 21, 1).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3, 1, 1).Trim() + "@@";
                }

                //중증암환자
                strRegs4 = DtAuto.Rows[0]["M2_DISREG4"].ToString().Trim();

                if (strRegs4 != "")
                {
                    strRegs4_2 = VB.Left(strRegs4, 4).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4, 20, 8).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4, 28, 8).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4, 36, 5).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4, 5, 15).Trim() + "@@";
                }

                if (str_NHIC_MCODE != "")
                {
                    for (int i = 1; i < VB.I(str_NHIC_MCODE, "^^"); i++)
                    {
                        strNHIC += VB.Pstr(str_NHIC_MCODE, "^^", i) + "000 ";
                    }
                }


                //차상위,희귀 자격체크
                if (str_NHIC_MCODE == "")
                {
                    if (VB.Left(strM_MCode, 1) != str_NHIC_MCODE)
                    {
                        rtnVal += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + '\r' + '\r';
                        rtnVal += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + '\r';
                    }
                }
                else
                {
                    if ((VB.I(str_NHIC_MCODE, "^^")) - 1 == 1)
                    {
                        if (VB.Left(strM_MCode, 1) != VB.TR(str_NHIC_MCODE, "^^", ""))
                        {
                            rtnVal += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + '\r' + '\r';
                            rtnVal += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + '\r';
                        }
                    }
                    else
                    {
                        if ( strM_MCode == "")
                        {
                            //OPD_MASTER 자격이 없다면
                            rtnVal += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + '\r' + '\r';
                            rtnVal += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + '\r';
                        }
                        else
                        {
                            strOK2 = "";

                            for (int i = 1; i <=  VB.I(str_NHIC_MCODE, "^^") -1; i++)
                            {
                                if (VB.Left(strM_MCode, 1) == VB.Pstr(str_NHIC_MCODE, "^^", i).Trim())
                                    strOK2 = "OK";
                            }


                            if (strOK2 == "")
                            {
                                //2건이상 자격인데 OPD_MASTER 자격이 없는경우
                                rtnVal += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + '\r' + '\r';
                                rtnVal += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + '\r';
                            }
                            else
                            {
                                //2건이상 자격인데 OPD_MASTER 자격이 다를경우
                                rtnVal += "예약환자 자격점검 [ 예약당시 자격과 현재접수 자격 비교 ]" + '\r' + '\r';
                                rtnVal += "예약당시[" + strM_MCode + "] 자격과 현재자격[" + strNHIC + "]이 다릅니다..자격확인후 처리하십시오.." + '\r';
                            }
                        }
                    }
                }

                //중증코드 체크 F003 의약분업코드는 제외
                if (strM_VCode != "F003" && strM_VCode != VB.Left(strRegs4, 4))
                {
                    rtnVal += "예약당시 중증코드[" + strM_VCode + "] 와 자격조회후 중증코드[" + VB.Left(strRegs4, 4) + "] 가 불일치합니다..";
                }

                if (strSangSil != "" && string.Compare(strSangSil, VB.Replace(clsPublic.GstrSysDate, "-", "")) < 0)
                {
                    if (rtnVal != "")
                    {
                        rtnVal += '\r' + '\r';
                        rtnVal += "자격상실자입니다..반드시 접수확인하세요 !!";
                    }
                    else
                    {
                        rtnVal = "자격상실자입니다..반드시 접수확인하세요 !!";
                    }
                }
            }
            else
            {
                rtnVal = "당일 예약자인데 당일 자격조회 자료가 없습니다..자격조회후 다시 수납하세요";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : Gam_Pano_Search_2"/>
        /// </summary>
        public string Read_Gam_Ptno(PsmhDb pDbCon, string ArgGam, string ArgJumin1, string ArgJumin2)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "NO";

            sb.Clear();
            sb.AppendLine(" SELECT gamjumin FROM KOSMOS_PMPA.BAS_GAMF ");
            sb.AppendLine("  WHERE GAMJUMIN3 = '" + clsAES.AES(ArgJumin1 + ArgJumin2).Trim() + "' ");
            sb.AppendLine("    AND GAMCODE ='" + ArgGam + "'");
            sb.AppendLine("    AND (GAMEND >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') OR GAMEND IS NULL) ");
            sb.AppendLine("  union all ");
            sb.AppendLine("   SELECT code ");
            sb.AppendLine("    FROM  KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine("     WHERE  GUBUN ='원무강제퇴사자감액' ");
            sb.AppendLine("     AND  TRIM(CODE) = '" + ArgJumin1 + ArgJumin2 + "' ");
            sb.AppendLine("  ORDER BY 1  DESC  ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "OK";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_희귀난치VCode_2"/>
        /// </summary>
        public string Read_Vcode(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "NO";

            sb.Clear();
            sb.AppendLine(" SELECT SuNext FROM KOSMOS_PMPA.BAS_SUN ");
            sb.AppendLine("  WHERE GBRARE = 'Y'  ");
            sb.AppendLine("    AND  SuNext = '" + ArgCode + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "OK";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_무인수납_BI_CHK"/>
        /// </summary>
        public string Check_Machine_Bi(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";

            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine(" WHERE GUBUN ='원무_무인수납_허용보험' ");
            sb.AppendLine("  AND TRIM(CODE)='" + ArgCode + "' ");
            sb.AppendLine("  AND (DELDATE IS NULL OR DELDATE ='') ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "OK";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_무인수납_오더과별표시_CHK"/>
        /// </summary>
        public string Check_Machine_Limit(PsmhDb pDbCon, string ArgDept)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";

            sb.Clear();
            sb.AppendLine(" SELECT ROWID FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine("  WHERE GUBUN ='원무_무인수납_오더허용과' ");
            sb.AppendLine("    AND TRIM(CODE)='" + ArgDept + "' ");
            sb.AppendLine("    AND (DELDATE IS NULL OR DELDATE ='') ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "OK";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_무인수납_종료시간_SET"/>
        /// </summary>
        public void Check_Machine_CloseTime(PsmhDb pDbCon)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            clsAuto.GstrAREquipEndTime = "18:00";
            clsAuto.GstrAREquipEndTime_DayOff = "13:00";
            clsAuto.GstrAROperateStartTime = "08:30";
            clsAuto.GstrAROperateEndTime = "17:30";
            clsAuto.GstrAROperateStartTime_DayOff = "08:30";
            clsAuto.GstrAROperateEndTime_DayOff = "12:50";

            sb.Clear();
            sb.AppendLine(" SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine(" WHERE GUBUN ='무인수납_종료시간_평일' ");
            sb.AppendLine("  AND (DELDATE IS NULL OR DELDATE ='') ");
            sb.AppendLine("  AND CODE = '" + clsType.User.IdNumber + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto = null;
                return;
            }

            if (DtAuto.Rows.Count > 0)
            {
                clsAuto.GstrAREquipEndTime = DtAuto.Rows[0]["NAME"].ToString().Trim();
            }

            DtAuto.Dispose();
            DtAuto = null;

            sb.Clear();
            sb.AppendLine(" SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine(" WHERE GUBUN ='무인수납_종료시간_토요일' ");
            sb.AppendLine("  AND (DELDATE IS NULL OR DELDATE ='') ");
            sb.AppendLine("  AND CODE = '" + clsType.User.IdNumber + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto = null;
                return;
            }

            if (DtAuto.Rows.Count > 0)
            {
                clsAuto.GstrAREquipEndTime_DayOff = DtAuto.Rows[0]["NAME"].ToString().Trim();
            }

            DtAuto.Dispose();
            DtAuto = null;


            sb.Clear();
            sb.AppendLine(" SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine(" WHERE GUBUN ='무인수납_운영시간_평일' ");
            sb.AppendLine("  AND (DELDATE IS NULL OR DELDATE ='') ");
            sb.AppendLine("  AND CODE = '" + clsType.User.IdNumber + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto = null;
                return;
            }

            if (DtAuto.Rows.Count > 0)
            {
                clsAuto.GstrAROperateStartTime = VB.Pstr(DtAuto.Rows[0]["NAME"].ToString().Trim(), "-", 1);
                clsAuto.GstrAROperateEndTime = VB.Pstr(DtAuto.Rows[0]["NAME"].ToString().Trim(), "-", 2);
            }

            DtAuto.Dispose();
            DtAuto = null;


            sb.Clear();
            sb.AppendLine(" SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE ");
            sb.AppendLine(" WHERE GUBUN ='무인수납_운영시간_토요일' ");
            sb.AppendLine("  AND (DELDATE IS NULL OR DELDATE ='') ");
            sb.AppendLine("  AND CODE = '" + clsType.User.IdNumber + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto = null;
                return;
            }

            if (DtAuto.Rows.Count > 0)
            {
                clsAuto.GstrAROperateStartTime_DayOff = VB.Pstr(DtAuto.Rows[0]["NAME"].ToString().Trim(), "-", 1);
                clsAuto.GstrAROperateEndTime_DayOff = VB.Pstr(DtAuto.Rows[0]["NAME"].ToString().Trim(), "-", 2);
            }

            DtAuto.Dispose();
            DtAuto = null;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : 무인수납_전화예약_CHK"/>
        /// </summary>
        public string Check_Machine_TelResv(PsmhDb pDbCon, string ArgPtno, string ArgActDate, string ArgDept)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "OK";
            string strTemp = "";
            string strTemp2 = "";

            sb.Clear();
            sb.AppendLine(" SELECT ROWID ");
            sb.AppendLine(" FROM KOSMOS_PMPA.OPD_MASTER ");
            sb.AppendLine("  WHERE Pano= '" + ArgPtno + "' ");
            sb.AppendLine("   AND ACTDATE <TO_DATE('" + ArgActDate + "','YYYY-MM-DD') ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count == 0)
            {
                rtnVal = "NO";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;


            sb.Clear();
            sb.AppendLine(" SELECT M2_GKiho,M2_DISREG3,M2_SANGSIL ");
            sb.AppendLine(" FROM KOSMOS_PMPA.OPD_NHIC ");
            sb.AppendLine("  WHERE Pano= '" + ArgPtno + "' ");
            sb.AppendLine("   AND actDATE =TO_DATE('" + ArgActDate + "','YYYY-MM-DD') ");
            sb.AppendLine("   AND DEPTCODE = '" + ArgDept + "' ");
            sb.AppendLine("   AND Job_STS ='2' ");
            sb.AppendLine("   AND ReqType ='M1' ");
            sb.AppendLine("   AND M2_Jagek NOT IN ('7','8') ");
            sb.AppendLine(" ORDER BY SendTime DESC ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                strTemp = DtAuto.Rows[0]["M2_GKIHO"].ToString().Trim();
                strTemp2 = DtAuto.Rows[0]["M2_DISREG3"].ToString().Trim();
            }

            if (VB.Mid(strTemp2, 21, 1) == "1" && VB.Mid(strTemp2, 1, 1) == "C")
            {
                rtnVal = "NO";
                return rtnVal;
            }
            else if (VB.Mid(strTemp2, 21, 1) == "2" && VB.Mid(strTemp2, 1, 1) == "E")
            {
                rtnVal = "NO";
                return rtnVal;
            }
            //2019-05-13 add  무자격자 예외처리
            else if (DtAuto.Rows[0]["M2_SANGSIL"].ToString().Trim()  != "")
            {
                rtnVal = "NO";
                return rtnVal;
            }
            else if (VB.Mid(strTemp2, 21, 1) == "2" && VB.Mid(strTemp2, 1, 1) == "F")
            {
                rtnVal = "NO";
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            sb.Clear();
            sb.AppendLine(" SELECT b.GKiho ");
            sb.AppendLine(" FROM KOSMOS_PMPA.OPD_TELRESV a, KOSMOS_PMPA.BAS_PATIENT b ");
            sb.AppendLine("  WHERE a.Pano=b.Pano ");
            sb.AppendLine("   AND a.Pano= '" + ArgPtno + "' ");
            sb.AppendLine("   AND a.RDATE =TO_DATE('" + ArgActDate + "','YYYY-MM-DD') ");
            sb.AppendLine("   AND a.DEPTCODE = '" + ArgDept + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (strTemp != VB.Replace(DtAuto.Rows[0]["GKIHO"].ToString(), "-", ""))
                {
                    rtnVal = "NO";
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }
            }
            else
            {
                rtnVal = "NO";
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : RTN_물리치료스케쥴체크"/>
        /// </summary>

        public int RTN_PT_Schedule(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            int rtnVal = 0;

            sb.Clear();
            sb.AppendLine(" SELECT Bi From KOSMOS_PMPA.BAS_PATIENT Where Pano = '" + ArgPtno + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (VB.Left(DtAuto.Rows[0]["Bi"].ToString(), 1) == "2" || VB.Left(DtAuto.Rows[0]["Bi"].ToString(), 1) == "1" || VB.Left(DtAuto.Rows[0]["Bi"].ToString(), 1) == "5")
                {
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }
            }

            DtAuto.Dispose();
            DtAuto = null;

            sb.Clear();
            sb.AppendLine(" SELECT nvl(MAX( NAL - RNAL), 0) MINRNAL FROM KOSMOS_PMPA.ETC_PTSCH ");
            sb.AppendLine(" WHERE PANO = '" + ArgPtno + "'");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = Convert.ToInt32(DtAuto.Rows[0]["MINRNAL"].ToString());
            }

            DtAuto.Dispose();
            DtAuto = null;


            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_PT_SCH_Dept"/>
        /// </summary>

        public string Read_PT_SCH_Dept(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";
            string strBdate = "";

            sb.Clear();
            sb.AppendLine(" SELECT MAX(BDate) MBDate From KOSMOS_PMPA.ETC_PTSCH ");
            sb.AppendLine(" Where Pano= '" + ArgPtno + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                strBdate = DtAuto.Rows[0]["MBDate"].ToString();
            }

            DtAuto.Dispose();
            DtAuto = null;

            if (strBdate != "")
            {
                sb.Clear();
                sb.AppendLine(" SELECT DEPTCODE From KOSMOS_PMPA.ETC_PTSCH ");
                sb.AppendLine(" Where Pano= '" + ArgPtno + "' ");
                sb.AppendLine("   AND BDate =TO_DATE('" + strBdate + "','YYYY-MM-DD') ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = DtAuto.Rows[0]["DEPTCODE"].ToString() + "^^" + strBdate;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : CHK_언어인지치료수가여부"/>
        /// </summary>

        public bool Check_PT_Suga(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            bool rtnVal = false;
            string strBdate = "";

            sb.Clear();
            sb.AppendLine(" SELECT MAX(BDate) MBDate From KOSMOS_PMPA.ETC_PTSCH ");
            sb.AppendLine(" Where Pano= '" + ArgPtno + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                strBdate = DtAuto.Rows[0]["MBDate"].ToString();
            }

            DtAuto.Dispose();
            DtAuto = null;

            if (strBdate != "")
            {
                sb.Clear();
                sb.AppendLine(" SELECT SUCODE From KOSMOS_PMPA.ETC_PTSCH ");
                sb.AppendLine(" Where Pano= '" + ArgPtno + "' ");
                sb.AppendLine("   AND BDate =TO_DATE('" + strBdate + "','YYYY-MM-DD') ");
                sb.AppendLine("   AND (TRIM(SUCODE) IN ( SELECT TRIM(SUNEXT) FROM KOSMOS_PMPA.BAS_SUN WHERE SUNAMEK LIKE '%인지%' ) ");
                sb.AppendLine("    OR SUCODE LIKE 'SPEECH%') ");
                SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtAuto.Dispose();
                    DtAuto = null;
                    return rtnVal;
                }

                if (DtAuto.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                DtAuto.Dispose();
                DtAuto = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.06.25
        /// <seealso cref="무인수납.bas : READ_NHIC"/>
        /// </summary>

        public string Read_NHIC(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";

            sb.Clear();
            sb.AppendLine(" SELECT M2_DISREG1,M2_DISREG2,M2_DISREG3,M2_DISREG9 ");
            sb.AppendLine("  FROM KOSMOS_PMPA.OPD_NHIC ");
            sb.AppendLine(" Where Pano ='" + ArgPtno + "' ");
            sb.AppendLine("   AND BDate=TO_DATE('" + ArgDate + "','YYYY-MM-DD') ");
            sb.AppendLine("   AND JOB_STS = '2' ");
            sb.AppendLine(" ORDER By BDate DESC ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (DtAuto.Rows[0]["M2_DISREG1"].ToString().Trim() != "")
                    rtnVal = "H000";
                else if (DtAuto.Rows[0]["M2_DISREG2"].ToString().Trim() != "")
                    rtnVal = "V000";
                else if (DtAuto.Rows[0]["M2_DISREG9"].ToString().Trim() != "")
                    rtnVal = "V000";
                else if (DtAuto.Rows[0]["M2_DISREG3"].ToString().Trim() != "")
                    rtnVal = "C000";
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// CHK_당일물리치료접수여부
        /// Author : 박웅규
        /// Create Date : 2018.09.30
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// <seealso cref="OUMSAD_CHK.bas : CHK_당일물리치료접수여부"/>
        /// <returns></returns>
        public string ChkTodayPtJupsu(PsmhDb pDbCon, string ArgPtno, string ArgBdate)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";

            sb.Clear();
            sb.AppendLine(" SELECT DEPTCODE,Bi From KOSMOS_PMPA.OPD_MASTER ");
            sb.AppendLine(" Where Pano = '" + ArgPtno + "' ");
            sb.AppendLine("   AND BDate =TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ");
            sb.AppendLine("   AND JIN IN ('8','U','G','T') ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = DtAuto.Rows[0]["DEPTCODE"].ToString().Trim();
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// CHK_당일물리치료수납여부
        /// Author : 박웅규
        /// Create Date : 2018.09.30
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgDept"></param>
        /// <seealso cref="OUMSAD_CHK.bas : CHK_당일물리치료수납여부"/>
        /// <returns></returns>
        public string ChkTodayPtSunap(PsmhDb pDbCon, string ArgPtno, string ArgBdate, string ArgDept)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            string rtnVal = "";

            sb.Clear();
            sb.AppendLine("SELECT ROWID From KOSMOS_PMPA.OPD_SLIP ");
            sb.AppendLine(" Where Pano = '" + ArgPtno + "' ");
            sb.AppendLine("   AND BDate =TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ");
            sb.AppendLine("   AND DeptCode = '" + ArgDept + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = DtAuto.Rows[0]["ROWID"].ToString().Trim();
            }

            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        /// <summary>
        /// CHK_당일자격조회여부
        /// Author : 박웅규
        /// Create Date : 2018.09.30
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="argDATE"></param>
        /// <seealso cref="OUMSAD_CHK.bas : CHK_당일자격조회여부"/>
        /// <returns></returns>
        public bool ChkTodayBiChk(PsmhDb pDbCon, string ArgPtno, string argDATE)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";

            bool rtnVal = false;
            string strYYMM = "";

            strYYMM = argDATE.Replace("-","");
            strYYMM = VB.Right(strYYMM, 6);

            sb.Clear();
            sb.AppendLine(" SELECT BICHK From KOSMOS_PMPA.BAS_PATIENT ");
            sb.AppendLine("  Where Pano = '" + ArgPtno + "' ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                if (DtAuto.Rows[0]["BICHK"].ToString().Trim() == strYYMM)
                {
                    rtnVal = true;
                }
            }
            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }


        /// <summary>
        /// DRUG_처방전유무Check_NEW
        /// Author : 박웅규
        /// Create Date : 2018.09.30
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgDept"></param>
        /// <seealso cref="Drug_out_atc.bas : DRUG_처방전유무Check_NEW"/>
        /// <returns></returns>
        public string DRUG_OUT_CHECK_NEW(PsmhDb pDbCon, string ArgBDate, string ArgPano, string ArgDept)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";
            string rtnVal = "";

            sb.Clear();
            sb.AppendLine(" SELECT SlipNo, COUNT(*) CNT ");
            sb.AppendLine("  FROM KOSMOS_OCS.OCS_OUTDRUGMST ");
            sb.AppendLine(" WHERE Pano='" + ArgPano +"' ");
            sb.AppendLine("   AND SlipDate=TO_DATE('" + ArgBDate +"','YYYY-MM-DD') ");
            sb.AppendLine("   AND DeptCode = '" + ArgDept  +"' ");
            sb.AppendLine("   AND Flag <> 'D' ");
            sb.AppendLine(" GROUP BY SlipNo ");
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = "OK";
            }
            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }
        
        public string check_SPCHJUPSU(PsmhDb pDbCon, string strPano)
        {
            DataTable DtAuto = new DataTable();
            StringBuilder sb = new StringBuilder();
            string SqlErr = "";
            string rtnVal = "";
            
            sb.Clear();
            sb.AppendLine(" SELECT SPCH From KOSMOS_PMPA.ETC_PTMASTER ");
            sb.AppendLine(" Where Pano = '" + strPano + "'            ");
            sb.AppendLine(" ORDER By STARTDATE DESC                   ");
            
            SqlErr = clsDB.GetDataTableEx(ref DtAuto, sb.ToString(), pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, sb.ToString(), pDbCon); //에러로그 저장
                DtAuto.Dispose();
                DtAuto = null;
                return rtnVal;
            }

            if (DtAuto.Rows.Count > 0)
            {
                rtnVal = DtAuto.Rows[0]["SPCH"].ToString().Trim();
            }
            DtAuto.Dispose();
            DtAuto = null;

            return rtnVal;
        }

        public static void frmAutoInfomationX_rEventClosed()
        {
            frmAutoInfomationX.Visible = false;
        }

        public static void LogWrite(string str)
        {
            string FilePath = Environment.CurrentDirectory + @"\Log\MobileSnedLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = Environment.CurrentDirectory + @"\Log";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        
    }
}
