using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public class clsEmrChart
    {
        /// <summary>
        /// EMR 환자 정보를 클리어 한다C:\PSMH\00.Com\00.ComLibB\ComEmrBase\class\clsEmrChart.cs
        /// </summary>
        /// <returns></returns>
        public static EmrPatient ClearPatient()
        {
            EmrPatient p = new EmrPatient();

            p.ptNo= string.Empty;
            p.ptName= string.Empty;
            p.ssno1= string.Empty;
            p.ssno2= string.Empty;
            p.birthdate= string.Empty;
            p.sex= string.Empty;
            p.age= string.Empty;
            p.tel= string.Empty;
            p.celphno= string.Empty;
            p.zipcd= string.Empty;
            p.addr= string.Empty;
            p.address= string.Empty;
            p.zipcdLoad= string.Empty;
            p.addrLoad= string.Empty;
            p.addressLoad= string.Empty;

            // 2017-06-09 법정감염병신고땜에 추가함
            p.rdnmAddr= string.Empty;         // 도로명주소
            p.rdnmAddr_dtl= string.Empty;     // 도로명주소 상세
            p.pibo_name= string.Empty;        // 피보험자 이름

            p.acpNo= string.Empty;
            p.inOutCls= string.Empty;
            p.medFrDate= string.Empty;
            //2016-07-17
            p.medIP_INDATE= string.Empty;
            p.medFrTime= string.Empty;
            p.medEndDate= string.Empty;
            p.medEndTime= string.Empty;
            p.medEndexDate= string.Empty;
            p.medDeptCd= string.Empty;
            p.medDrCd= string.Empty;
            p.medDeptKorName= string.Empty;
            p.medDrName= string.Empty;
            p.ward= string.Empty;
            p.room= string.Empty;
            p.remark= string.Empty;

            p.bi= string.Empty;
            p.biname= string.Empty;

            p.formNo = 0;
            p.updateNo = 0;
            p.printyn = "0";
            p.writeName= string.Empty;
            p.MomName= string.Empty;
            p.DadName= string.Empty;
            p.oldYn = "0";

            p.opno= string.Empty;
            p.opdate= string.Empty;
            p.opdegree= string.Empty;
            p.opdept= string.Empty;

            p.nowdeptcd= string.Empty;
            p.nowdrcd= string.Empty;
            p.nowroomno= string.Empty;

            p.wardDate= string.Empty;
            p.wardTime= string.Empty;

            p.cur_Dept= string.Empty;
            p.cur_Grade= string.Empty;

            p.acpNoOut = "0";
            p.acpNoIn = "0";

            return p;
        }

        /// <summary>
        /// EMR 환자 정보를 옮긴다(복사한다)
        /// </summary>
        /// <param name="po"></param>
        /// <returns></returns>
        public static EmrPatient CopyPatient(EmrPatient po)
        {
            EmrPatient p = new EmrPatient();

            p.ptNo = po.ptNo;
            p.ptName = po.ptName;
            p.ssno1 = po.ssno1;
            p.ssno2 = po.ssno2;
            p.birthdate = po.birthdate;
            p.tel = po.tel;
            p.celphno = po.celphno;
            p.zipcd = po.zipcd;
            p.addr = po.addr;
            p.address = po.address;
            p.zipcdLoad = po.zipcdLoad;
            p.addrLoad = po.addrLoad;
            p.addressLoad = po.addressLoad;

            // 2017-06-09 법정감염병신고땜에 추가함
            p.rdnmAddr = po.rdnmAddr;               // 도로명주소
            p.rdnmAddr_dtl = po.rdnmAddr_dtl;       // 도로명주소 상세
            //2017-09-05  피보험자 이름
            p.pibo_name = po.pibo_name;             // 피보험자 이름


            p.acpNo = po.acpNo;
            p.inOutCls = po.inOutCls;
            p.medFrDate = po.medFrDate;

            //2016-07-17 
            p.medIP_INDATE = po.medIP_INDATE;


            p.medFrTime = po.medFrTime;
            p.medEndDate = po.medEndDate;
            p.medEndTime = po.medEndTime;
            p.medEndexDate = po.medEndexDate;
            p.medDeptCd = po.medDeptCd;
            p.medDrCd = po.medDrCd;
            p.medDeptKorName = po.medDeptKorName;
            p.medDrName = po.medDrName;
            p.ward = po.ward;
            p.room = po.room;
            p.remark = po.remark;

            p.bi = po.bi;
            p.biname = po.biname;

            p.formNo = po.formNo;
            p.updateNo = po.updateNo;
            p.printyn = po.printyn;
            p.writeName = po.writeName;
            p.MomName = po.MomName;
            p.DadName = po.DadName;
            p.oldYn = po.oldYn;

            p.opno = po.opno;
            p.opdate = po.opdate;
            p.opdegree = po.opdegree;
            p.opdept = po.opdept;

            p.nowdeptcd = po.nowdeptcd;
            p.nowdrcd = po.nowdrcd;
            p.nowroomno = po.nowroomno;

            p.wardDate = po.wardDate;
            p.wardTime = po.wardTime;

            p.cur_Dept = po.cur_Dept;
            p.cur_Grade = po.cur_Grade;

            p.acpNoOut = po.acpNoOut;
            p.acpNoIn = po.acpNoIn;

            return p;
        }

        /// <summary>
        /// AEMRFORM 정보를 클리어 한다.
        /// </summary>
        /// <returns></returns>
        public static EmrForm ClearEmrForm()
        {
            EmrForm f = new EmrForm();

            f.FmFORMNO = 0;
            f.FmUPDATENO = 0;
            f.FmGRPFORMNO = 0;
            f.FmDISPSEQ = 0;
            f.FmFORMNAME= string.Empty;
            f.FmFORMNAMEPRINT= string.Empty;
            f.FmFORMTYPE= string.Empty;

            f.FmFORMCD= string.Empty;
            f.FmPROGFORMNAME= string.Empty;
            f.FmDOCFORMNAME= string.Empty;
            f.FmINOUTCLS = "0";
            f.FmFORMCNT = 0;

            f.FmUSEGB = "0";
            f.FmUSEDEPT= string.Empty;
            f.FmVIEWAUTH = "0";
            f.FmWRITEAUTH = "0";
            f.FmVISITSDEPT= string.Empty;

            f.FmUSECHECK = 0;
            f.FmMIBICHECK = 0;
            f.FmCERTCHECK = 0;
            f.FmCERTBOTH = 0;
            f.FmCERTNUM = 0;
            f.FmALIGNGB = 0;
            f.FmVIEWGROUP = 0;
            f.FmDOCPRINTHEAD = 0;

            f.FmREGDATE= string.Empty;
            f.FmCONVIMAGE = 0;
            f.FmOLDGB = 0;

            f.FmPRINTTYPE = 0;
            f.FmFLOWITEMCNT = 0;
            f.FmFLOWHEADCNT = 0;

            return f;
        }

        /// <summary>
        /// AEMRFORM 정보를 옮긴다(복사한다)
        /// </summary>
        /// <param name="fo"></param>
        /// <returns></returns>
        public static EmrForm CopyEmrForm(EmrForm fo)
        {
            EmrForm f = new EmrForm();

            f.FmFORMNO = fo.FmFORMNO;
            f.FmUPDATENO = fo.FmUPDATENO;
            f.FmGRPFORMNO = fo.FmGRPFORMNO;
            f.FmDISPSEQ = fo.FmDISPSEQ;
            f.FmFORMNAME = fo.FmFORMNAME;
            f.FmFORMNAMEPRINT = fo.FmFORMNAMEPRINT;
            f.FmFORMTYPE = fo.FmFORMTYPE;

            f.FmFORMCD = fo.FmFORMCD;
            f.FmPROGFORMNAME = fo.FmPROGFORMNAME;
            f.FmDOCFORMNAME = fo.FmDOCFORMNAME;
            f.FmINOUTCLS = fo.FmINOUTCLS;
            f.FmFORMCNT = fo.FmFORMCNT;

            f.FmUSEGB = fo.FmUSEGB;
            f.FmUSEDEPT = fo.FmUSEDEPT;
            f.FmVIEWAUTH = fo.FmVIEWAUTH;
            f.FmWRITEAUTH = fo.FmWRITEAUTH;

            f.FmUSECHECK = fo.FmUSECHECK;
            f.FmMIBICHECK = fo.FmMIBICHECK;
            f.FmCERTCHECK = fo.FmCERTCHECK;
            f.FmCERTBOTH = fo.FmCERTBOTH;
            f.FmCERTNUM = fo.FmCERTNUM;


            f.FmALIGNGB = fo.FmALIGNGB;
            f.FmVIEWGROUP = fo.FmVIEWGROUP;
            f.FmDOCPRINTHEAD = fo.FmDOCPRINTHEAD;

            f.FmREGDATE = fo.FmREGDATE;
            f.FmCONVIMAGE = fo.FmCONVIMAGE;
            f.FmOLDGB = fo.FmOLDGB;

            return f;
        }

        /// <summary>
        /// OCS 접수정보로 조회
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMedDeptCd"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoOcs(PsmhDb pDbCon, string strPTNO, string strInOutCls, string strMedFrDate, string strMedDeptCd = "")
        {
            string SQL= string.Empty;    //Query문
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient p = new EmrPatient();

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    TEL, ";
                SQL = SQL + ComNum.VBLF + "    CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    WARD, ";
                SQL = SQL + ComNum.VBLF + "    ROOM, ";
                SQL = SQL + ComNum.VBLF + "    REMARK, ";
                SQL = SQL + ComNum.VBLF + "    ADDR, ";
                SQL = SQL + ComNum.VBLF + "    acpnoin, ";
                SQL = SQL + ComNum.VBLF + "    BI -- 접수시 BI";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AVIEWACPOCS ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND INOUTCLS = '" + strInOutCls + "'";
                SQL = SQL + ComNum.VBLF + "     AND MEDFRDATE = '" + strMedFrDate.Replace("-","")  + "'";
                if (strMedDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND MEDDEPTCD = '" + strMedDeptCd + "'";
                }
                //SQL = SQL + ComNum.VBLF + "     AND MEDDRCD = '" + p.medDrCd + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }


                DateTime dtpCurDate = Convert.ToDateTime(ComQuery.CurrentDateTime(pDbCon, "S"));

                p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                if (p.acpNo.Trim() == "")
                {
                    p.acpNo = "0";
                }
                p.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                p.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
                p.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                p.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                p.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                p.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
                p.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComQuery.CurrentDateTime(pDbCon, "D")));

 

                #region 0세인데 현재 년도가 아님. 잘못된 날짜
                if (p.age == "0" && dtpCurDate.Year - VB.Val(p.birthdate.Substring(0, 4)) >= 1)
                {
                    p.age = CalculateAge(dtpCurDate, DateTime.ParseExact(p.birthdate, "yyyyMMdd", null)).ToString();
                }
                #endregion


                p.tel = dt.Rows[0]["TEL"].ToString().Trim();
                p.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                p.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                p.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                //TODO 도로명 주소 해결
                //p.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                p.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                p.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                p.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();
                p.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();

                #region //신규데이타일 경우
                //2016-07-17 병동입원일자로 사용함 
                p.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                //2016-07-17  실제로 입원일자를 사용 함 
                p.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();

                p.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                p.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                {
                    p.medEndTime= string.Empty;
                }
                else
                {
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                    }
                    else
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                    }
                }
                p.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                p.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                p.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                p.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                p.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                #endregion //신규데이타일 경우

                p.ward = dt.Rows[0]["WARD"].ToString().Trim();
                p.room = dt.Rows[0]["ROOM"].ToString().Trim();
                p.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                p.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                p.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                p.bi= string.Empty;
                p.biname= string.Empty;
                //p.opdate= string.Empty;
                //p.opdegree= string.Empty;

                p.acpNoOut = "0";
                p.acpNoIn = dt.Rows[0]["acpnoin"].ToString().Trim();
                p.bi = dt.Rows[0]["BI"].ToString().Trim();
                p.biname = clsVbfunc.GetBiName(dt.Rows[0]["BI"].ToString().Trim());

                dt.Dispose();
                dt = null;

                #region DRCODE가 5자리?면 매칭해서 다시 넣는다.
                if (p.medDrCd.Length > 4)
                {
                    SQL = "SELECT DRCODE";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DOCTOR";
                    SQL = SQL + ComNum.VBLF + "WHERE SABUN = '" + p.medDrCd + "'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return null;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return null;
                    }

                    p.medDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
                    dt.Dispose();
                }
                #endregion

                //TODO 환자정보 기타
                //if (p.inOutCls == "I")
                //{
                //    GetMedFrTime(pDbCon, p.ptNo, p.medIP_INDATE, ref p.medFrTime, ref p.medFrDate);
                //}

                //GetRoom(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, ref p.ward, ref p.room);
                //GetPtBi(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, p.medDeptCd, ref p.bi, ref p.biname);
                //GetMOTHER(pDbCon, p.ptNo, p.medIP_INDATE, ref p.MomName, ref p.DadName);


                return p;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        public static int CalculateAge(DateTime dtpCurDate, DateTime dateOfBirth)
        {
            int age = dtpCurDate.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > dtpCurDate.AddYears(-age)) 
                age--;

            return age;
        }

        /// <summary>
        /// 투약 접수정보로 조회
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMedDeptCd"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoTuyak(PsmhDb pDbCon, string strPTNO, string strInOutCls, string strMedFrDate, string strMedDeptCd = "")
        {
            string SQL= string.Empty;    //Query문
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient p = new EmrPatient();

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    TEL, ";
                SQL = SQL + ComNum.VBLF + "    CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    WARD, ";
                SQL = SQL + ComNum.VBLF + "    ROOM, ";
                SQL = SQL + ComNum.VBLF + "    REMARK, ";
                SQL = SQL + ComNum.VBLF + "    ADDR ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AVIEWACPOCS ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND INOUTCLS = '" + strInOutCls + "'";
                SQL = SQL + ComNum.VBLF + "     AND MEDFRDATE = '" + strMedFrDate.Replace("-", "") + "'";
                if (strMedDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND MEDDEPTCD = '" + strMedDeptCd + "'";
                }
                //SQL = SQL + ComNum.VBLF + "     AND MEDDRCD = '" + p.medDrCd + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                if (p.acpNo.Trim() == "")
                {
                    p.acpNo = "0";
                }
                p.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                p.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
                p.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                p.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                p.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                p.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
                p.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComQuery.CurrentDateTime(pDbCon, "D")));

                p.tel = dt.Rows[0]["TEL"].ToString().Trim();
                p.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                p.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                p.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                //TODO 도로명 주소 해결
                //p.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                p.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                p.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                p.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();
                p.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();

                #region //신규데이타일 경우
                //2016-07-17 병동입원일자로 사용함 
                p.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                //2016-07-17  실제로 입원일자를 사용 함 
                p.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();

                p.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                p.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                {
                    p.medEndTime= string.Empty;
                }
                else
                {
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                    }
                    else
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                    }
                }
                p.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                p.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                p.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                p.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                p.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                #endregion //신규데이타일 경우

                p.ward = dt.Rows[0]["WARD"].ToString().Trim();
                //p.room = dt.Rows[0]["ROOM"].ToString().Trim();
                p.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                p.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                p.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                p.ward= string.Empty;
                p.room= string.Empty;
                p.bi= string.Empty;
                p.biname= string.Empty;
                //p.opdate= string.Empty;
                //p.opdegree= string.Empty;

                p.acpNoOut = "0";
                p.acpNoIn = "0";

                dt.Dispose();
                dt = null;

                //TODO 환자정보 기타
                //if (p.inOutCls == "I")
                //{
                //    GetMedFrTime(pDbCon, p.ptNo, p.medIP_INDATE, ref p.medFrTime, ref p.medFrDate);
                //}

                //GetRoom(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, ref p.ward, ref p.room);
                //GetPtBi(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, p.medDeptCd, ref p.bi, ref p.biname);
                //GetMOTHER(pDbCon, p.ptNo, p.medIP_INDATE, ref p.MomName, ref p.DadName);


                return p;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// BIT 접수정보로 조회
        /// </summary>
        /// <param name="strPTNO"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMedDeptCd"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfTret(PsmhDb pDbCon, string strPTNO, string strInOutCls, string strMedFrDate, string strMedDeptCd = "")
        {
            string SQL= string.Empty;    //Query문
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient p = new EmrPatient();

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    PTNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    TEL, ";
                SQL = SQL + ComNum.VBLF + "    CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    WARD, ";
                SQL = SQL + ComNum.VBLF + "    ROOM, ";
                SQL = SQL + ComNum.VBLF + "    REMARK, ";
                SQL = SQL + ComNum.VBLF + "    ADDR ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AVIEWACP ";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "     AND INOUTCLS = '" + strInOutCls + "'";
                SQL = SQL + ComNum.VBLF + "     AND MEDFRDATE = '" + strMedFrDate.Replace("-","") + "'";
                if (strMedDeptCd != "")
                {
                    SQL = SQL + ComNum.VBLF + "     AND MEDDEPTCD = '" + strMedDeptCd + "'";
                }
                //SQL = SQL + ComNum.VBLF + "     AND MEDDRCD = '" + p.medDrCd + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                p.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                p.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
                p.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                p.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                p.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                p.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
                p.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComQuery.CurrentDateTime(pDbCon, "D")));

                p.tel = dt.Rows[0]["TEL"].ToString().Trim();
                p.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                p.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                p.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                //TODO 도로명 주소 해결
                //p.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                p.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                p.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                p.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();

                p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                p.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();

                #region //신규데이타일 경우
                //2016-07-17 병동입원일자로 사용함 
                p.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                //2016-07-17  실제로 입원일자를 사용 함 
                p.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();

                p.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                p.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                {
                    p.medEndTime= string.Empty;
                }
                else
                {
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                    }
                    else
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                    }
                }
                p.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                p.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                p.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                p.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                p.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                #endregion //신규데이타일 경우

                p.ward = dt.Rows[0]["WARD"].ToString().Trim();
                //p.room = dt.Rows[0]["ROOM"].ToString().Trim();
                p.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                p.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                p.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                p.ward= string.Empty;
                p.room= string.Empty;
                p.bi= string.Empty;
                p.biname= string.Empty;
                //p.opdate= string.Empty;
                //p.opdegree= string.Empty;

                p.acpNoOut = "0";
                p.acpNoIn = "0";

                dt.Dispose();
                dt = null;

                //TODO 환자정보 기타
                //if (p.inOutCls == "I")
                //{
                //    GetMedFrTime(pDbCon, p.ptNo, p.medIP_INDATE, ref p.medFrTime, ref p.medFrDate);
                //}

                //GetRoom(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, ref p.ward, ref p.room);
                //GetPtBi(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, p.medDeptCd, ref p.bi, ref p.biname);
                //GetMOTHER(pDbCon, p.ptNo, p.medIP_INDATE, ref p.MomName, ref p.DadName);


                return p;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 환자 정보(접수정보)를 조회한다.
        /// </summary>
        /// <param name="strAcpNo"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfo(PsmhDb pDbCon, string strAcpNo)
        {
            string SQL= string.Empty;    //Query문
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient p = new EmrPatient();

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    T.PATID AS PTNO, ";
                SQL = SQL + ComNum.VBLF + "    P.PATIENT_NAME AS PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    P.BUDAMAME AS BUDAMAME,";
                SQL = SQL + ComNum.VBLF + "    p.JUMIN1 AS SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    p.JUMIN2 AS SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    P.TEL AS TEL, P.PIBO_NAME AS PIBO_NAME, ";
                SQL = SQL + ComNum.VBLF + "    P.HPNUMBER AS CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    (P.POST_CODE1 || P.POST_CODE2) AS ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    '' AS ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(P.BIRTH_DATE, 'YYYYMMDD') AS BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    '' AS ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    '' AS ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    '' AS ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    '' AS ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    T.CLASS AS INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    T.INDATE AS MEDFRDATE, ";

                //2016-07-17 
                //SQL = SQL + ComNum.VBLF + "    T.INDATE AS MEDFRDATE, ";
                // ENTER_DATE 에실제 입원일자가 들어와서 추가 함 실제로 병동 입원일자는 
                // 앞으로  WARD_DATE
                SQL = SQL + ComNum.VBLF + "    T.INDATE AS IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "     CASE WHEN T.CLASS = 'O' THEN T.INDATE ";
                SQL = SQL + ComNum.VBLF + "     ELSE (CASE WHEN T.INDATE <= '20151231' THEN T.INDATE   ";    // 2016-07-16 수정  INTIME
                SQL = SQL + ComNum.VBLF + " 		  ELSE ( SELECT TO_CHAR( IPD.WARD_DATE,'YYYYMMDD') ENTER_DATE  ";
                SQL = SQL + ComNum.VBLF + " 		        FROM IPD_PATIENT_IN_INFORMATION IPD  ";
                SQL = SQL + ComNum.VBLF + " 		        WHERE T.INDATE = TO_CHAR( IPD.ENTER_DATE,'YYYYMMDD')  ";
                SQL = SQL + ComNum.VBLF + " 		            AND T.PATID = IPD.PATIENT_NO  ";
                SQL = SQL + ComNum.VBLF + " 		            AND T.CLASS ='I' )";
                SQL = SQL + ComNum.VBLF + " 		  END ) ";
                SQL = SQL + ComNum.VBLF + "     END AS MEDFRDATE, ";

                SQL = SQL + ComNum.VBLF + "     CASE WHEN T.CLASS = 'O' THEN T.INTIME ";
                SQL = SQL + ComNum.VBLF + "     ELSE (CASE WHEN T.INDATE <= '20151231' THEN T.INTIME  ";  //   INDATE
                SQL = SQL + ComNum.VBLF + " 		  ELSE ( SELECT REPLACE(IPD.ENTER_TIME, ':', '') || '00'  ";
                SQL = SQL + ComNum.VBLF + " 		        FROM IPD_PATIENT_IN_INFORMATION IPD  ";
                SQL = SQL + ComNum.VBLF + " 		        WHERE   T.INDATE = TO_CHAR( IPD.ENTER_DATE,'YYYYMMDD')  ";
                SQL = SQL + ComNum.VBLF + " 		            AND T.PATID = IPD.PATIENT_NO  ";
                SQL = SQL + ComNum.VBLF + " 		            AND T.CLASS ='I' )";
                SQL = SQL + ComNum.VBLF + " 		  END ) ";
                SQL = SQL + ComNum.VBLF + "     END AS MEDFRTIME, ";

                SQL = SQL + ComNum.VBLF + "    T.OUTDATE AS MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    REPLACE(T.OUTTIME, ':', '') AS MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    '' AS MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    T.CLINCODE AS MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    T.DOCCODE AS MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    T.WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    T.WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    '0' AS CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    D.DEPTKORNAME AS MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    U.USENAME AS MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    '' AS WARD, ";
                SQL = SQL + ComNum.VBLF + "    '' AS ROOM, ";
                SQL = SQL + ComNum.VBLF + "    '' AS REMARK, ";
                SQL = SQL + ComNum.VBLF + "    P.G_KIHO, ";
                SQL = SQL + ComNum.VBLF + "    P.JI_CODE, P.JUSO, R.POST_NAME1 || ' ' || R.POST_NAME2 || ' ' || R.POST_NAME3 AS ADDR ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT T ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT P ";
                SQL = SQL + ComNum.VBLF + "    ON P.PATIENT_NO = T.PATID ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN MED_PMPA.BAS_POSTS R ";
                SQL = SQL + ComNum.VBLF + "        ON P.POST_CODE1 = R.POST_CODE1 ";
                SQL = SQL + ComNum.VBLF + "        AND P.POST_CODE2 = R.POST_CODE2 ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWMEDDEPT D ";
                SQL = SQL + ComNum.VBLF + "    ON TRIM(D.MEDDEPTCD) = TRIM(T.CLINCODE) ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
                SQL = SQL + ComNum.VBLF + "    ON U.USEID = T.DOCCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE T.TREATNO = " + VB.Val(strAcpNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                p.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                p.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();

                if (dt.Rows[0]["BUDAMAME"].ToString().Trim() != "")
                {
                    p.ptName = p.ptName + "(" + dt.Rows[0]["BUDAMAME"].ToString().Trim() + ")";
                }

                p.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                p.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                p.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                p.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");

                p.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D")));

                p.tel = dt.Rows[0]["TEL"].ToString().Trim();
                p.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                p.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                p.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                p.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                p.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                p.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                p.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();

                // 2017-06-09 법정감염병신고땜에 추가함
                p.rdnmAddr = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), ""); // 도로명주소
                p.rdnmAddr_dtl = dt.Rows[0]["JUSO"].ToString().Trim();  // 도로명주소 상세
                //2017-09-05 
                p.pibo_name = dt.Rows[0]["PIBO_NAME"].ToString().Trim();  // 피보험자명                

                p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                p.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                //2016-07-17 병동입원일자로 사용함 
                p.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                //2016-07-17  실제로 입원일자를 사용 함 
                p.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();


                p.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                p.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                {
                    p.medEndTime= string.Empty;
                }
                else
                {
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                    }
                    else
                    {
                        p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                    }
                }
                p.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                p.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                p.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                p.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                p.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                //p.ward = dt.Rows[0]["WARD"].ToString().Trim();
                //p.room = dt.Rows[0]["ROOM"].ToString().Trim();
                p.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                p.ward= string.Empty;
                p.room= string.Empty;
                p.bi= string.Empty;
                p.biname= string.Empty;
                p.g_Kiho = dt.Rows[0]["G_KIHO"].ToString().Trim();

                p.opdate= string.Empty;
                p.opdegree= string.Empty;
                p.opdept= string.Empty;

                p.nowdeptcd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                p.nowdrcd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                p.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                p.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                p.nowroomno= string.Empty;
                p.acpNoOut = "0";
                p.acpNoIn = "0";

                dt.Dispose();
                dt = null;
                //TODO 환자 추가 정보
                //if (p.inOutCls == "I")
                //{
                //    GetMedFrTime(pDbCon, p.ptNo, p.medIP_INDATE, ref p.medFrTime, ref p.medFrDate);

                //}
                //GetRoom(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, ref p.ward, ref p.room);
                //GetPtBi(pDbCon, p.inOutCls, p.ptNo, p.medIP_INDATE, p.medDeptCd, ref p.bi, ref p.biname);
                //GetMOTHER(pDbCon, p.ptNo, p.medIP_INDATE, ref p.MomName, ref p.DadName);

                p.nowroomno = p.room;

                p.cur_Dept= string.Empty;
                p.cur_Grade= string.Empty;


                return p;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 엄마 정보 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPTNO"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMonName"></param>
        /// <param name="strDadName"></param>
        public static void GetMOTHER(PsmhDb pDbCon, string strPTNO, string strMedFrDate, ref string strMonName, ref string strDadName)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;

            strMonName= string.Empty;
            strDadName= string.Empty;

            SQL = " SELECT DISTINCT B.PATIENT_NO, B.PATIENT_NAME, R.ITEMVALUE ";
            SQL = SQL + ComNum.VBLF + "FROM MED_OCS.NEWBORN_BABY A  ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_PMPA + "BAS_PATIENT B ";
            SQL = SQL + ComNum.VBLF + "    ON B.PATIENT_NO = A.MOTHER_NO  ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTMST C ";
            SQL = SQL + ComNum.VBLF + "    ON C.PTNO = B.PATIENT_NO ";
            SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = 23 ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
            SQL = SQL + ComNum.VBLF + "    ON R.EMRNO = C.EMRNO ";
            SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD = 'I0000013061' ";
            SQL = SQL + ComNum.VBLF + "WHERE A.BABY_PAT_NO = '" + strPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "    AND A.BIRTH_DAY <= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD') ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strMonName = dt.Rows[0]["PATIENT_NAME"].ToString().Trim();
                strDadName = dt.Rows[0]["ITEMVALUE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 접수시간 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPTNO"></param>
        /// <param name="strMEDFRDATE"></param>
        /// <param name="strMedFrTime"></param>
        /// <param name="strwardMedFrdate"></param>
        public static void GetMedFrTime(PsmhDb pDbCon, string strPTNO, string strMEDFRDATE, ref string strMedFrTime, ref string strwardMedFrdate)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = "SELECT A.ENTER_TIME   , TO_CHAR(WARD_DATE,'YYYYMMDD')  WARD_DATE  ";
            SQL = SQL + ComNum.VBLF + "FROM MED_OCS.IPD_PATIENT_IN_INFORMATION A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ENTER_DATE = TO_DATE('" + ComFunc.FormatStrToDate(strMEDFRDATE, "D") + "', 'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "AND A.PATIENT_NO = '" + strPTNO + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strMedFrTime = dt.Rows[0]["ENTER_TIME"].ToString().Trim().Replace(":", "") + "00";
                strwardMedFrdate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 병실정보 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strINOUTCLS"></param>
        /// <param name="strPTNO"></param>
        /// <param name="strMEDFRDATE"></param>
        /// <param name="strWard"></param>
        /// <param name="strRoom"></param>
        public static void GetRoom(PsmhDb pDbCon, string strINOUTCLS, string strPTNO, string strMEDFRDATE, ref string strWard, ref string strRoom)
        {
            if (strINOUTCLS == "O")
            {
                return;
            }
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            SQL = "SELECT A.ROOM_NO AS ROOM, A.WARD ";
            SQL = SQL + ComNum.VBLF + "FROM MED_OCS.IPD_PATIENT_IN_WARD A ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ENTER_DATE = TO_DATE('" + ComFunc.FormatStrToDate(strMEDFRDATE, "D") + "', 'YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "AND A.PATIENT_NO = '" + strPTNO + "' ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strWard = dt.Rows[0]["WARD"].ToString().Trim();
                strRoom = dt.Rows[0]["ROOM"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 보험 유형 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strINOUTCLS"></param>
        /// <param name="strPTNO"></param>
        /// <param name="strMEDFRDATE"></param>
        /// <param name="strMEDDEPTCD"></param>
        /// <param name="strBI"></param>
        /// <param name="strBINAME"></param>
        public static void GetPtBi(PsmhDb pDbCon, string strINOUTCLS, string strPTNO, string strMEDFRDATE, string strMEDDEPTCD, ref string strBI, ref string strBINAME)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;

            if (strINOUTCLS == "I")
            {
                SQL = "SELECT A.MED_INSURANCE_CLASS AS BI, B.BAS_NAME ";
                SQL = SQL + ComNum.VBLF + "FROM MED_OCS.IPD_PATIENT_INFORMATION A ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN MED_PMPA.BAS_CODE B ";
                SQL = SQL + ComNum.VBLF + "    ON B.BAS_CLASS = A.MED_INSURANCE_CLASS ";
                SQL = SQL + ComNum.VBLF + "    AND B.BAS_GUBUN = 'GISUL' ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ADMISSION_DATE = TO_DATE('" + ComFunc.FormatStrToDate(strMEDFRDATE, "D") + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "AND A.PATIENT_NO = '" + strPTNO + "' ";
            }
            else
            {
                SQL = " SELECT A.BI, B.BAS_NAME ";
                SQL = SQL + ComNum.VBLF + "FROM MED_PMPA.OPD_MAST A ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN MED_PMPA.BAS_CODE B ";
                SQL = SQL + ComNum.VBLF + "    ON B.BAS_CLASS = A.BI ";
                SQL = SQL + ComNum.VBLF + "    AND B.BAS_GUBUN = 'GISUL' ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACT_DATE = TO_DATE('" + ComFunc.FormatStrToDate(strMEDFRDATE, "D") + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "AND A.PATIENT_NO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "AND A.DEPT_CODE = '" + strMEDDEPTCD + "' ";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                strBI = dt.Rows[0]["BI"].ToString().Trim();
                strBINAME = dt.Rows[0]["BAS_NAME"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 신주소 조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strJiCode"></param>
        /// <param name="strADDR"></param>
        /// <param name="strJUSO"></param>
        /// <returns></returns>
        public static string GetNewAddr(PsmhDb pDbCon, string strJiCode, string strADDR, string strJUSO)
        {
            string strAddr= string.Empty;
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt1 = null;

            if (strJiCode != "")
            {
                strAddr= string.Empty;
                SQL = "SELECT * FROM Med_Pmpa.BAS_POSTS_ROAD";
                SQL = SQL + ComNum.VBLF + "WHERE CRITICAL_CODE = '" + strJiCode + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    strAddr = strADDR + " " + strJUSO;
                    return strAddr;
                }
                if (dt1.Rows.Count > 0)
                {
                    strAddr = dt1.Rows[0]["POST_NAME1"].ToString().Trim();
                    strAddr = strAddr + " " + dt1.Rows[0]["POST_NAME2"].ToString().Trim();
                    strAddr = strAddr + " " + dt1.Rows[0]["ROAD_NAME"].ToString().Trim();

                    if ((dt1.Rows[0]["BUILD_CODE1"].ToString().Trim() != "0") && (dt1.Rows[0]["BUILD_CODE2"].ToString().Trim() != "0"))
                    {
                        strAddr = strAddr + " " + dt1.Rows[0]["BUILD_CODE1"].ToString().Trim() + "-" + dt1.Rows[0]["BUILD_CODE2"].ToString().Trim();
                    }
                    else if ((dt1.Rows[0]["BUILD_CODE1"].ToString().Trim() != "0") && (dt1.Rows[0]["BUILD_CODE2"].ToString().Trim() == "0"))
                    {
                        strAddr = strAddr + " " + dt1.Rows[0]["BUILD_CODE1"].ToString().Trim();
                    }
                    else
                    {
                        strAddr = strAddr + " " + dt1.Rows[0]["BUILD_CODE2"].ToString().Trim();
                    }
                    if ((dt1.Rows[0]["BUILD_NAME"].ToString().Trim() != "" && dt1.Rows[0]["BUILD_NAME_ETC"].ToString().Trim() != ""))
                    {
                        strAddr = strAddr + " " + dt1.Rows[0]["BUILD_NAME"].ToString().Trim() + " ";
                    }
                    else if ((dt1.Rows[0]["BUILD_NAME"].ToString().Trim() != "" && dt1.Rows[0]["BUILD_NAME_ETC"].ToString().Trim() == ""))
                    {
                        strAddr = strAddr + " " + dt1.Rows[0]["BUILD_NAME"].ToString().Trim() + " ";
                    }
                    else if ((dt1.Rows[0]["BUILD_NAME"].ToString().Trim() == "" && dt1.Rows[0]["BUILD_NAME_ETC"].ToString().Trim() != ""))
                    {
                        strAddr = strAddr + " " + dt1.Rows[0]["BUILD_NAME_ETC"].ToString().Trim() + " ";
                    }

                    strAddr = strAddr + strJUSO;
                }
                else
                {
                    strAddr = strADDR + " " + strJUSO;
                }
                dt1.Dispose();
                dt1 = null;
            }
            else
            {
                strAddr = strADDR + " " + strJUSO;
            }
            return strAddr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoPtNo(string strPtNo)
        {
            // 안씀.
            return null;
            //string SQL= string.Empty;
            //DataTable dt = null;
            //EmrPatient p = new EmrPatient();

            //try
            //{
            //    SQL= string.Empty;
            //    SQL = SQL + ComNum.VBLF + " SELECT ";
            //    SQL = SQL + ComNum.VBLF + "	    PTNO, PTNAME, SSNO1, SSNO2, BIRTHDATE, TEL, CELPHON, ZIPCD, ADDR, ADDRESS, ";
            //    SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ADDRLOAD, ADDRESSLOAD, ";
            //    SQL = SQL + ComNum.VBLF + "    ACPNO, INOUTCLS, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, MEDENDDEXDATE, ";
            //    SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, MEDDRCD, MEDDEPTKORNAME, MEDDRNAME, WARD, ROOM, ";
            //    SQL = SQL + ComNum.VBLF + "    REMARK ";
            //    SQL = SQL + ComNum.VBLF + "FROM AVIEWEMRACP ";
            //    SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo.Trim() + "'";
            //    SQL = SQL + ComNum.VBLF + "    AND ACPNO = (SELECT MAX(A1.ACPNO) FROM AAACP A1";
            //    SQL = SQL + ComNum.VBLF + "                    WHERE A1.PTNO = '" + strPtNo.Trim() + "'";
            //    SQL = SQL + ComNum.VBLF + "                        AND A1.CNCLYN = '0')";

            //    dt = clsDB.GetDataTableREx(SQL);

            //    if (dt == null)
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        return null;
            //    }
            //    if (dt.Rows.Count == 0)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        return null;
            //    }

            //    p.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
            //    p.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
            //    p.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
            //    p.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
            //    p.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
            //    p.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
            //    p.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(pDbCon, "D"), "D")));

            //    p.tel = dt.Rows[0]["TEL"].ToString().Trim();
            //    p.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
            //    p.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
            //    p.addr = dt.Rows[0]["ADDR"].ToString().Trim();
            //    p.address = dt.Rows[0]["ADDRESS"].ToString().Trim();
            //    p.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
            //    p.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
            //    p.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();

            //    p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
            //    p.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();
            //    p.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
            //    p.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
            //    p.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
            //    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
            //    {
            //        p.medEndTime= string.Empty;
            //    }
            //    else
            //    {
            //        if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
            //        {
            //            p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
            //        }
            //        else
            //        {
            //            p.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
            //        }
            //    }
            //    p.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
            //    p.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
            //    p.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
            //    p.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
            //    p.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
            //    //p.ward = dt.Rows[0]["WARD"].ToString().Trim();
            //    //p.room = dt.Rows[0]["ROOM"].ToString().Trim();
            //    p.remark = dt.Rows[0]["REMARK"].ToString().Trim();
            //    p.ward= string.Empty;
            //    p.room= string.Empty;
            //    p.bi= string.Empty;
            //    p.biname= string.Empty;
            //    p.opdate= string.Empty;
            //    p.opdegree= string.Empty;
            //    p.opdept= string.Empty;

            //    p.nowdeptcd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
            //    p.nowdrcd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
            //    p.nowroomno = 0;

            //    dt.Dispose();
            //    dt = null;

            //    if (p.inOutCls == "I")
            //    {
            //        GetMedFrTime(p.ptNo, p.medFrDate, ref p.medFrTime);
            //    }

            //    GetRoom(p.inOutCls, p.ptNo, p.medFrDate, ref p.ward, ref p.room);
            //    GetPtBi(p.inOutCls, p.ptNo, p.medFrDate, p.medDeptCd, ref p.bi, ref p.biname);
            //    GetMOTHER(p.ptNo, p.medFrDate, ref p.MomName, ref p.DadName);
            //    p.nowroomno = Convert.ToInt32(VB.Val(p.room));

            //    return p;

            //}
            //catch (Exception ex)
            //{
            //    ComFunc.MsgBox(ex.Message);
            //    return null;
            //}
        }
        
        /// <summary>
        /// Scan EMR_TREATT에서  환자접수정보를 가져온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strTreatNo"> EASFORMDATA의 키 </param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoScan(PsmhDb pDbCon, string strTreatNo)
        {
            EmrPatient pAcp = new EmrPatient();
            string strOLDYN = "N";

            try
            {
                pAcp.acpNo = strTreatNo;                
                SetAcpInfo(pDbCon, ref pAcp, strOLDYN);

                return pAcp;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }
        
        /// <summary>
        /// 전자동의서 EASFORMDATA에서  환자접수정보를 가져온다
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFormDataId"> EASFORMDATA의 키 </param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoEas(PsmhDb pDbCon, string strFormDataId)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient pAcp = new EmrPatient();

            string strOLDYN = "N";

            try
            {
                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + " SELECT B.ID AS EMRNO, A.FORMNO, A.UPDATENO, TO_CHAR(C.CREATED, 'YYYYMMDD') AS CHARTDATE, TO_CHAR(C.CREATED, 'HHMMSS') AS CHARTTIME, ";
                SQL = SQL + ComNum.VBLF + " 0 AS ACPNO, C.PTNO, C.INOUTCLS, C.MEDFRDATE, C.MEDFRTIME, '' AS MEDENDDATE, '' AS MEDENDTIME, C.MEDDEPTCD, C.MEDDRCD,      ";
                SQL = SQL + ComNum.VBLF + " A.FORMNAME, A.GRPFORMNO,       ";
                SQL = SQL + ComNum.VBLF + " (SELECT GRPFORMNAME FROM KOSMOS_EMR.AEMRGRPFORM WHERE GRPFORMNO = A.GRPFORMNO) AS GRPFORMNAME, ";
                SQL = SQL + ComNum.VBLF + " (SELECT DEPTKORNAME FROM KOSMOS_EMR.VIEWBMEDDEPT WHERE MEDDEPTCD = C.MEDDEPTCD) AS DEPTKORNAME , D.USENAME AS MEDDRNAME,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(c.modified, 'YYYY-MM-DD HH:MM') as modified, e.useName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRFORM A ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "AEASFORMCONTENT B ";
                SQL = SQL + ComNum.VBLF + " ON A.FORMNO = B.FORMNO ";
                SQL = SQL + ComNum.VBLF + " AND A.UPDATENO = B.UPDATENO ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "AEASFORMDATA C ";
                SQL = SQL + ComNum.VBLF + " ON B.ID = C.EASFORMCONTENT ";
                SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN aviewemruser D ";
                SQL = SQL + ComNum.VBLF + " ON C.MEDDRCD = D.DRCD ";
                SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN aviewemruser E ";
                SQL = SQL + ComNum.VBLF + " ON C.modifiedUser = E.useId ";
                SQL = SQL + ComNum.VBLF + " WHERE C.ID = " + VB.Val(strFormDataId);

                 SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                strOLDYN = "Y"; //acpNo 사용여부 Y이면 사용안함
                pAcp.acpNo = "0";
                pAcp.formNo = (long)VB.Val(dt.Rows[0]["FORMNO"].ToString().Trim());
                pAcp.updateNo = (int)VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
                pAcp.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                pAcp.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                //pAcp.printyn = dt.Rows[0]["PRNTYN"].ToString().Trim();
                //pAcp.opdate = dt.Rows[0]["OPDATE"].ToString().Trim();
                //pAcp.writeName = dt.Rows[0]["CHARTUSENAME"].ToString().Trim();

                //pAcp.opdegree = dt.Rows[0]["OPDEGREE"].ToString().Trim();
                //pAcp.opdept = dt.Rows[0]["OP_DEPT"].ToString().Trim();
                //pAcp.nowdeptcd = dt.Rows[0]["DEPTCDNOW"].ToString().Trim();
                //pAcp.nowdrcd = dt.Rows[0]["DRCDNOW"].ToString().Trim();
                //pAcp.nowroomno = dt.Rows[0]["ROOM_NO"].ToString().Trim();
                pAcp.medEndexDate= string.Empty; //dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                pAcp.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                pAcp.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                pAcp.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                pAcp.medDeptKorName = dt.Rows[0]["DEPTKORNAME"].ToString().Trim();
                pAcp.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                pAcp.writeDate = dt.Rows[0]["modified"].ToString().Trim();
                pAcp.writeName = dt.Rows[0]["useName"].ToString().Trim();

                dt.Dispose();
                dt = null;

                SetAcpInfo(pDbCon, ref pAcp, strOLDYN);

                return pAcp;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }
        
        /// <summary>
        /// EMRXML에서 환자(접수)정보를 가지고 온다.
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoXml(PsmhDb pDbCon, string strEmrNo)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient pAcp = new EmrPatient();

            string strOLDYN = "N";

            try
            {
                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    OLDYN,                 ";
                SQL = SQL + ComNum.VBLF + "    EMRNO,                 ";
                SQL = SQL + ComNum.VBLF + "    ACPNO,                 ";
                SQL = SQL + ComNum.VBLF + "    FORMNO,                ";
                SQL = SQL + ComNum.VBLF + "    UPDATENO,              ";
                SQL = SQL + ComNum.VBLF + "    CHARTDATE,             ";
                SQL = SQL + ComNum.VBLF + "    CHARTTIME,             ";
                SQL = SQL + ComNum.VBLF + "    CHARTUSEID,            ";
                SQL = SQL + ComNum.VBLF + "    WRITEDATE,             ";
                SQL = SQL + ComNum.VBLF + "    WRITETIME,             ";
                SQL = SQL + ComNum.VBLF + "    COMPUSEID,             ";
                SQL = SQL + ComNum.VBLF + "    COMPDATE,              ";
                SQL = SQL + ComNum.VBLF + "    COMPTIME,              ";
                SQL = SQL + ComNum.VBLF + "    PRNTYN,                ";
                SQL = SQL + ComNum.VBLF + "    SAVEGB,                ";
                SQL = SQL + ComNum.VBLF + "    SAVECERT,              ";
                SQL = SQL + ComNum.VBLF + "    FORMGB,                ";
                SQL = SQL + ComNum.VBLF + "    PTNO,                  ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS,              ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE,             ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME,             ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE,            ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME,            ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD,             ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD,               ";
                SQL = SQL + ComNum.VBLF + "    OPDATE,                ";
                SQL = SQL + ComNum.VBLF + "    OPDEGREE,              ";
                SQL = SQL + ComNum.VBLF + "    OP_DEPT,               ";
                SQL = SQL + ComNum.VBLF + "    DEPTCDNOW,             ";
                SQL = SQL + ComNum.VBLF + "    DRCDNOW,               ";
                SQL = SQL + ComNum.VBLF + "    ROOM_NO,               ";
                SQL = SQL + ComNum.VBLF + "    ACPNOOUT,              ";
                SQL = SQL + ComNum.VBLF + "    CURDEPT,               ";
                SQL = SQL + ComNum.VBLF + "    FORMNAME,              ";
                SQL = SQL + ComNum.VBLF + "    GRPFORMNAME,           ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,                ";
                SQL = SQL + ComNum.VBLF + "    CHARTUSENAME,          ";
                SQL = SQL + ComNum.VBLF + "    COMPUSERNAME,          ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME,        ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME              ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AVIEWCHARTMST C";
                //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U ";
                //SQL = SQL + ComNum.VBLF + "    ON C.CHARTUSEID = U.USEID  ";
                SQL = SQL + ComNum.VBLF + "WHERE C.EMRNO = " + VB.Val(strEmrNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                strOLDYN = dt.Rows[0]["OLDYN"].ToString().Trim();

                pAcp.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                pAcp.formNo = (long)VB.Val(dt.Rows[0]["FORMNO"].ToString().Trim());
                pAcp.updateNo = (int)VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
                pAcp.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                pAcp.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                pAcp.printyn = dt.Rows[0]["PRNTYN"].ToString().Trim();
                pAcp.opdate = dt.Rows[0]["OPDATE"].ToString().Trim();
                pAcp.writeName = dt.Rows[0]["CHARTUSENAME"].ToString().Trim();
                pAcp.chartDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                pAcp.chartTime = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                pAcp.writeDate = dt.Rows[0]["WRITEDATE"].ToString().Trim();
                pAcp.writeTime = dt.Rows[0]["WRITETIME"].ToString().Trim();

                pAcp.opdegree = dt.Rows[0]["OPDEGREE"].ToString().Trim();
                pAcp.opdept = dt.Rows[0]["OP_DEPT"].ToString().Trim();
                pAcp.nowdeptcd = dt.Rows[0]["DEPTCDNOW"].ToString().Trim();
                pAcp.nowdrcd = dt.Rows[0]["DRCDNOW"].ToString().Trim();
                pAcp.nowroomno = dt.Rows[0]["ROOM_NO"].ToString().Trim();

                pAcp.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                pAcp.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                pAcp.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                pAcp.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();

                #region //이전데이타일 경우
                if (strOLDYN == "Y")
                {
                    //병동입원일자로 사용함 
                    pAcp.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();
                    //실제로 입원일자를 사용 함 
                    pAcp.medIP_INDATE = dt.Rows[0]["MEDFRDATE"].ToString().Trim(); //IP_INDATE
                    pAcp.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                    pAcp.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                    {
                        pAcp.medEndTime= string.Empty;
                    }
                    else
                    {
                        if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                        {
                            pAcp.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                        }
                        else
                        {
                            pAcp.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                        }
                    }
                    pAcp.medEndexDate= string.Empty; //dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                    pAcp.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                    pAcp.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                    pAcp.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                    pAcp.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                }
                #endregion //이전데이타

                dt.Dispose();
                dt = null;

                //20-05-15 전과 했을경우 처리 추가.
                if (pAcp.inOutCls.Equals("I"))
                {
                    pAcp.medDeptCd = TransIpdDeptCode(pDbCon, pAcp.ptNo, pAcp.medFrDate, pAcp.medDeptCd);
                }
                pAcp = SetEmrPatInfoOcs(pDbCon, pAcp.ptNo, pAcp.inOutCls, pAcp.medFrDate, pAcp.medDeptCd);
                //SetAcpInfo(pDbCon, ref pAcp, strOLDYN);

                //TODO 환자정보 기타
                //if (pAcp.inOutCls == "I")
                //{
                //    GetMedFrTime(pDbCon, pAcp.ptNo, pAcp.medIP_INDATE, ref pAcp.medFrTime, ref pAcp.medFrDate);
                //}

                //GetRoom(pDbCon, pAcp.inOutCls, pAcp.ptNo, pAcp.medIP_INDATE, ref pAcp.ward, ref pAcp.room);
                //GetPtBi(pDbCon, pAcp.inOutCls, pAcp.ptNo, pAcp.medIP_INDATE, pAcp.medDeptCd, ref pAcp.bi, ref pAcp.biname);
                //GetMOTHER(pDbCon, pAcp.ptNo, pAcp.medIP_INDATE, ref pAcp.MomName, ref pAcp.DadName);

                return pAcp;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 전과 했을경우 마지막 입원정보
        /// </summary>
        /// <returns></returns>
        public static string TransIpdDeptCode(PsmhDb pDbCon, string strPano, string FrDate, string DeptCode)
        {
            string rtnVal = DeptCode;
            string SQL = string.Empty;
            OracleDataReader dataReader = null;

            try
            {
                SQL = " SELECT DEPTCODE";
                SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "WHERE PANO = '" + strPano + "'";
                SQL += ComNum.VBLF + "  AND INDATE >= TO_DATE('" + DateTime.ParseExact(FrDate, "yyyyMMdd", null).ToShortDateString() + " 00:00', 'YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "  AND INDATE <= TO_DATE('" + DateTime.ParseExact(FrDate, "yyyyMMdd", null).ToShortDateString() + " 23:59', 'YYYY-MM-DD HH24:MI')";

                string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, pDbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (dataReader.HasRows && dataReader.Read())
                {
                    rtnVal = dataReader.GetValue(0).ToString().Trim();
                }

                dataReader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 내원정보를 세팅한다 : 정상적일 경우
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="strOLDYN"></param>
        public static void SetAcpInfo(PsmhDb pDbCon, ref EmrPatient pAcp, string strOLDYN)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    PTNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    TEL, ";
                SQL = SQL + ComNum.VBLF + "    CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    WARD, ";
                SQL = SQL + ComNum.VBLF + "    ROOM, ";
                SQL = SQL + ComNum.VBLF + "    REMARK, ";
                SQL = SQL + ComNum.VBLF + "    ADDR ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AVIEWACP ";
                if (strOLDYN == "Y")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + pAcp.ptNo + "'";
                    SQL = SQL + ComNum.VBLF + "     AND INOUTCLS = '" + pAcp.inOutCls + "'";
                    SQL = SQL + ComNum.VBLF + "     AND MEDDEPTCD = '" + pAcp.medDeptCd + "'";
                    //SQL = SQL + ComNum.VBLF + "     AND MEDDRCD = '" + pAcp.medDrCd + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + pAcp.acpNo;
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return ;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    SetAcpInfoOcs(pDbCon, ref pAcp, strOLDYN);

                    return ;
                }

                pAcp.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                pAcp.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
                pAcp.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                pAcp.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                pAcp.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                pAcp.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
                pAcp.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComQuery.CurrentDateTime(pDbCon, "D")));

                pAcp.tel = dt.Rows[0]["TEL"].ToString().Trim();
                pAcp.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                pAcp.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                pAcp.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                //TODO 도로명 주소 해결
                //pAcp.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                pAcp.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                pAcp.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                pAcp.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();

                pAcp.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                pAcp.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();

                #region //신규데이타일 경우
                if (strOLDYN == "N")
                {
                    //2016-07-17 병동입원일자로 사용함 
                    pAcp.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                    //2016-07-17  실제로 입원일자를 사용 함 
                    pAcp.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();

                    pAcp.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                    pAcp.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                    {
                        pAcp.medEndTime= string.Empty;
                    }
                    else
                    {
                        if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                        {
                            pAcp.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                        }
                        else
                        {
                            pAcp.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                        }
                    }
                    pAcp.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                    pAcp.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                    pAcp.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                    pAcp.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                    pAcp.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                }
                #endregion //신규데이타일 경우

                pAcp.ward = dt.Rows[0]["WARD"].ToString().Trim();
                //pAcp.room = dt.Rows[0]["ROOM"].ToString().Trim();
                pAcp.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                pAcp.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                pAcp.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                pAcp.ward= string.Empty;
                pAcp.room= string.Empty;
                pAcp.bi= string.Empty;
                pAcp.biname= string.Empty;
                //pAcp.opdate= string.Empty;
                //pAcp.opdegree= string.Empty;
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return ;
            }
        }

        /// <summary>
        /// 내원정보를 세팅한다 : 비정상적일 경우
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="pAcp"></param>
        /// <param name="strOLDYN"></param>
        public static void SetAcpInfoOcs(PsmhDb pDbCon, ref EmrPatient pAcp, string strOLDYN)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    PTNO, ";
                SQL = SQL + ComNum.VBLF + "    PTNAME,  ";
                SQL = SQL + ComNum.VBLF + "    SSNO1, ";
                SQL = SQL + ComNum.VBLF + "    SSNO2, ";
                SQL = SQL + ComNum.VBLF + "    TEL, ";
                SQL = SQL + ComNum.VBLF + "    CELPHON, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCD, ";
                SQL = SQL + ComNum.VBLF + "    ADDR1, ";
                SQL = SQL + ComNum.VBLF + "    BIRTHDATE, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESS, ";
                SQL = SQL + ComNum.VBLF + "    ZIPCDLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ADDRESSLOAD, ";
                SQL = SQL + ComNum.VBLF + "    ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS, ";
                SQL = SQL + ComNum.VBLF + "    IP_INDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDFRTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDTIME, ";
                SQL = SQL + ComNum.VBLF + "    MEDENDDEXDATE, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTCD, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRCD, ";
                SQL = SQL + ComNum.VBLF + "    WARD_DATE, ";
                SQL = SQL + ComNum.VBLF + "    WARD_TIME, ";
                SQL = SQL + ComNum.VBLF + "    CNCLYN, ";
                SQL = SQL + ComNum.VBLF + "    MEDDEPTKORNAME, ";
                SQL = SQL + ComNum.VBLF + "    MEDDRNAME, ";
                SQL = SQL + ComNum.VBLF + "    WARD, ";
                SQL = SQL + ComNum.VBLF + "    ROOM, ";
                SQL = SQL + ComNum.VBLF + "    REMARK, ";
                SQL = SQL + ComNum.VBLF + "    ADDR ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AVIEWACP ";
                if (strOLDYN == "Y")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + pAcp.ptNo + "'";
                    SQL = SQL + ComNum.VBLF + "     AND INOUTCLS = '" + pAcp.inOutCls + "'";
                    SQL = SQL + ComNum.VBLF + "     AND MEDDEPTCD = '" + pAcp.medDeptCd + "'";
                    //SQL = SQL + ComNum.VBLF + "     AND MEDDRCD = '" + pAcp.medDrCd + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + pAcp.acpNo;
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
                    dt = null;
                    return;
                }

                pAcp.ptNo = dt.Rows[0]["PTNO"].ToString().Trim();
                pAcp.ptName = dt.Rows[0]["PTNAME"].ToString().Trim();
                pAcp.ssno1 = dt.Rows[0]["SSNO1"].ToString().Trim();
                pAcp.ssno2 = dt.Rows[0]["SSNO2"].ToString().Trim();
                pAcp.birthdate = dt.Rows[0]["BIRTHDATE"].ToString().Trim();
                pAcp.sex = ComFunc.SexCheck(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), "1");
                pAcp.age = Convert.ToString(ComFunc.AgeCalcX1(dt.Rows[0]["SSNO1"].ToString().Trim() + dt.Rows[0]["SSNO2"].ToString().Trim(), ComQuery.CurrentDateTime(pDbCon, "D")));

                pAcp.tel = dt.Rows[0]["TEL"].ToString().Trim();
                pAcp.celphno = dt.Rows[0]["CELPHON"].ToString().Trim();
                pAcp.zipcd = dt.Rows[0]["ZIPCD"].ToString().Trim();
                pAcp.addr = dt.Rows[0]["ADDR"].ToString().Trim();
                //TODO 도로명 주소 해결
                //pAcp.address = GetNewAddr(pDbCon, dt.Rows[0]["JI_CODE"].ToString().Trim(), dt.Rows[0]["ADDR"].ToString().Trim(), dt.Rows[0]["JUSO"].ToString().Trim());
                pAcp.zipcdLoad = dt.Rows[0]["ZIPCDLOAD"].ToString().Trim();
                pAcp.addrLoad = dt.Rows[0]["ADDRLOAD"].ToString().Trim();
                pAcp.addressLoad = dt.Rows[0]["ADDRESSLOAD"].ToString().Trim();

                pAcp.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();
                pAcp.inOutCls = dt.Rows[0]["INOUTCLS"].ToString().Trim();

                pAcp.medEndDate= string.Empty;

                #region //신규데이타일 경우
                if (strOLDYN == "N")
                {
                    //2016-07-17 병동입원일자로 사용함 
                    pAcp.medFrDate = dt.Rows[0]["MEDFRDATE"].ToString().Trim();

                    //2016-07-17  실제로 입원일자를 사용 함 
                    pAcp.medIP_INDATE = dt.Rows[0]["IP_INDATE"].ToString().Trim();

                    pAcp.medFrTime = dt.Rows[0]["MEDFRTIME"].ToString().Trim();
                    pAcp.medEndDate = dt.Rows[0]["MEDENDDATE"].ToString().Trim();
                    if (dt.Rows[0]["MEDENDTIME"].ToString().Trim() == "000000")
                    {
                        pAcp.medEndTime= string.Empty;
                    }
                    else
                    {
                        if (dt.Rows[0]["MEDENDTIME"].ToString().Trim().Length == 4)
                        {
                            pAcp.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim() + "00";
                        }
                        else
                        {
                            pAcp.medEndTime = dt.Rows[0]["MEDENDTIME"].ToString().Trim();
                        }
                    }
                    pAcp.medEndexDate = dt.Rows[0]["MEDENDDEXDATE"].ToString().Trim();
                    pAcp.medDeptCd = dt.Rows[0]["MEDDEPTCD"].ToString().Trim();
                    pAcp.medDrCd = dt.Rows[0]["MEDDRCD"].ToString().Trim();
                    pAcp.medDeptKorName = dt.Rows[0]["MEDDEPTKORNAME"].ToString().Trim();
                    pAcp.medDrName = dt.Rows[0]["MEDDRNAME"].ToString().Trim();
                }
                #endregion //신규데이타일 경우

                pAcp.ward = dt.Rows[0]["WARD"].ToString().Trim();
                //pAcp.room = dt.Rows[0]["ROOM"].ToString().Trim();
                pAcp.remark = dt.Rows[0]["REMARK"].ToString().Trim();
                pAcp.wardDate = dt.Rows[0]["WARD_DATE"].ToString().Trim();
                pAcp.wardTime = dt.Rows[0]["WARD_TIME"].ToString().Trim();

                pAcp.ward= string.Empty;
                pAcp.room= string.Empty;
                pAcp.bi= string.Empty;
                pAcp.biname= string.Empty;
                //pAcp.opdate= string.Empty;
                //pAcp.opdegree= string.Empty;
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Bit의 접수정보를 조회한다
        /// </summary>
        /// <param name="pAcp"></param>
        /// <returns></returns>
        public static EmrPatient SetEmrPatInfoBit(PsmhDb pDbCon, AcpOrdPatient pAcp)
        {
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrPatient p = new EmrPatient();

            try
            {
                SQL = " SELECT CLASS, INDATE, CLINCODE, DOCCODE  ";
                SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    PATID AS PTNO,";
                SQL = SQL + ComNum.VBLF + "FROM TREATT ";
                SQL = SQL + ComNum.VBLF + "WHERE INDATE = '" + pAcp.In_date.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CLASS = '" + pAcp.InOutCls + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PATID = '" + pAcp.PatientNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + pAcp.ClinicalDept + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DOCCODE = '" + pAcp.ClinicalDoct + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                p.acpNo = dt.Rows[0]["ACPNO"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return p;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Bit 접수번호를 가지고 온다.
        /// </summary>
        /// <param name="pAcp"></param>
        /// <returns></returns>
        public static string GetTreatNoBit(PsmhDb pDbCon, AcpOrdPatient pAcp)
        {
            string rtnVal= string.Empty;
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT CLASS, INDATE, CLINCODE, DOCCODE,  ";
                SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    PATID AS PTNO";
                SQL = SQL + ComNum.VBLF + "FROM TREATT ";
                SQL = SQL + ComNum.VBLF + "WHERE INDATE = '" + pAcp.In_date.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CLASS = '" + pAcp.InOutCls + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PATID = '" + pAcp.PatientNo + "' ";
                if (pAcp.InOutCls == "O")
                {
                    SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + pAcp.ClinicalDept + "' ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    SQL = " SELECT CLASS, INDATE, CLINCODE, DOCCODE,  ";
                    SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
                    SQL = SQL + ComNum.VBLF + "    PATID AS PTNO";
                    SQL = SQL + ComNum.VBLF + "FROM TREATT ";
                    SQL = SQL + ComNum.VBLF + "WHERE INDATE >= '" + (Convert.ToInt32(pAcp.In_date.Replace("-", "")) - 1).ToString() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND INDATE <= '" + pAcp.In_date.Replace("-", "") + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND CLASS = '" + pAcp.InOutCls + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND PATID = '" + pAcp.PatientNo + "' ";
                    if (pAcp.InOutCls == "O")
                    {
                        SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + pAcp.ClinicalDept + "' ";
                    }

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return "";
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }
                }

                rtnVal = dt.Rows[0]["ACPNO"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public static string GetTreatNoBitEx(PsmhDb pDbCon, AcpOrdPatient pAcp)
        {
            string rtnVal = "0";
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            try
            {
                if (pAcp.PatientJupsuTime == null)
                {
                    pAcp.PatientJupsuTime = "12:00";
                }

                if (pAcp.PatientJupsuTime == "")
                {
                    pAcp.PatientJupsuTime = "12:00";
                }

                pAcp.PatientJupsuTime = pAcp.PatientJupsuTime.Replace(":", "");

                pAcp.PatientJupsuTime = VB.Left(pAcp.PatientJupsuTime, 2) + ":" + VB.Mid(pAcp.PatientJupsuTime, 3, 2);


                if (pAcp.InOutCls == "O")
                {
                    clsDB.setBeginTran(pDbCon);
                    
                    try
                    {
                        if (pAcp.InOutCls == "I")
                        {
                            SQL = " SELECT IN_DATE, INDEPT_CODE, IN_TIME ";
                            SQL = SQL + ComNum.VBLF + "FROM MED_PMPA.IPD_MAST  ";
                            SQL = SQL + ComNum.VBLF + "WHERE PATIENT_NO = '" + pAcp.PatientNo + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND IN_DATE = " + ComFunc.ConvOraToDate(ComFunc.FormatStrToDate(pAcp.In_date.Replace("-", ""), "D"), "D") + " ";
                            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                pAcp.PatientJupsuTime = dt.Rows[0]["IN_TIME"].ToString().Trim();
                                if (pAcp.PatientJupsuTime == "")
                                {
                                    pAcp.PatientJupsuTime = "12:00";
                                }
                            }
                            dt.Dispose();
                            dt = null;
                        }

                        SQL = " SELECT CLASS, INDATE, CLINCODE, DOCCODE,  ";
                        SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
                        SQL = SQL + ComNum.VBLF + "    PATID AS PTNO";
                        SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT  ";
                        SQL = SQL + ComNum.VBLF + "WHERE INDATE = '" + pAcp.In_date.Replace("-", "") + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND CLASS = '" + pAcp.InOutCls + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND PATID = '" + pAcp.PatientNo + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + pAcp.ClinicalDept + "' ";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            SQL = " INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, INDATE, CLINCODE, CLASS,  DOCCODE, OLDPATID, REGTIME, INTIME, LOCATION) ";
                            SQL = SQL + ComNum.VBLF + " SELECT MED_EMR.SEQ_TREATNO.NEXTVAL@HAN_OCS MAXKEY , '" + pAcp.PatientNo + "', '" + pAcp.In_date.Replace("-", "") + "', ";
                            SQL = SQL + ComNum.VBLF + "       '" + pAcp.ClinicalDept + "', '" + pAcp.InOutCls + "', '" + pAcp.ClinicalDoct + "',  ";
                            SQL = SQL + ComNum.VBLF + "       '" + pAcp.PatientNo + "', SYSDATE, '" + pAcp.PatientJupsuTime + "', ";
                            SQL = SQL + ComNum.VBLF + "       ''";
                            SQL = SQL + ComNum.VBLF + " FROM MED_EMR.CONFIGT@HAN_OCS  ";
                            SQL = SQL + ComNum.VBLF + " WHERE NOT EXISTS (SELECT 1 FROM KOSMOS_EMR.EMR_TREATT ";
                            SQL = SQL + ComNum.VBLF + "                                WHERE PATID = '" + pAcp.PatientNo + "'  ";
                            SQL = SQL + ComNum.VBLF + "                                AND CLASS = '" + pAcp.InOutCls + "'  ";
                            SQL = SQL + ComNum.VBLF + "                                AND DOCCODE = '" + pAcp.ClinicalDoct + "'  ";
                            SQL = SQL + ComNum.VBLF + "                                AND CLINCODE = '" + pAcp.ClinicalDept + "' AND INDATE = '" + pAcp.In_date.Replace("-", "") + "'  ";
                            SQL = SQL + ComNum.VBLF + "                                AND INTIME = '" + pAcp.PatientJupsuTime + "') ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {
                            if (pAcp.ClinicalDoct != dt.Rows[0]["DOCCODE"].ToString().Trim() && pAcp.InOutCls == "I")
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL = SQL + ComNum.VBLF + "    DOCCODE = '" + pAcp.ClinicalDoct + "',  ";
                                SQL = SQL + ComNum.VBLF + "    LOCATION = '차트수정'";
                                SQL = SQL + ComNum.VBLF + "WHERE INDATE = '" + pAcp.In_date.Replace("-", "") + "' ";
                                SQL = SQL + ComNum.VBLF + "    AND CLASS = '" + pAcp.InOutCls + "' ";
                                SQL = SQL + ComNum.VBLF + "    AND PATID = '" + pAcp.PatientNo + "' ";
                                SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + pAcp.ClinicalDept + "' ";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return rtnVal;
                                }
                            }
                        }
                        dt.Dispose();
                        dt = null;
                        clsDB.setCommitTran(pDbCon);
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                }

                SQL = " SELECT CLASS, INDATE, CLINCODE, DOCCODE,  ";
                SQL = SQL + ComNum.VBLF + "    TREATNO AS ACPNO, ";
                SQL = SQL + ComNum.VBLF + "    PATID AS PTNO";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT ";
                SQL = SQL + ComNum.VBLF + "WHERE INDATE = '" + pAcp.In_date.Replace("-", "") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CLASS = '" + pAcp.InOutCls + "' ";
                SQL = SQL + ComNum.VBLF + "    AND PATID = '" + pAcp.PatientNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + pAcp.ClinicalDept + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["ACPNO"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// AEMRFORM 정보를 가지고 온다.
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUPDATENO"></param>
        /// <returns></returns>
        public static EmrForm SerEmrFormInfo(PsmhDb pDbCon, string strFormNo, string strUPDATENO)
        {

            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrForm f = new EmrForm();

            f = ClearEmrForm();

            try
            {

                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "    FORMNO         ,  UPDATENO       ,  GRPFORMNO      ,  DISPSEQ        ,  FORMNAME       ,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNAMEPRINT  ,  FORMTYPE       ,  FORMCD         ,  PROGFORMNAME   ,  DOCFORMNAME    ,  ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS       ,  FORMCNT        ,  USEDEPT        ,  VIEWAUTH       ,  FLOWGB         ,  ";
                SQL = SQL + ComNum.VBLF + "    WRITEAUTH      ,  USECHECK       ,  MIBICHECK      ,  CERTCHECK      ,  CERTBOTH       ,  ";
                SQL = SQL + ComNum.VBLF + "    CERTNUM        ,  ALIGNGB        ,  VIEWGROUP      ,  DOCPRINTHEAD   , REGDATE         ,";
                SQL = SQL + ComNum.VBLF + "    CONVIMAGE      ,  OLDGB          ,  VISITSDEPT     ,  PRINTTYPE    , ";
                SQL = SQL + ComNum.VBLF + "    FLOWITEMCNT    ,  FLOWHEADCNT    ,  FLOWINPUTSIZE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO   = " + VB.Val(strFormNo);
                SQL = SQL + ComNum.VBLF + "  AND UPDATENO = " + VB.Val(strUPDATENO);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                f.FmFORMNO = (long)VB.Val(dt.Rows[0]["FORMNO"].ToString().Trim());
                f.FmUPDATENO = (int)VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
                f.FmGRPFORMNO = (int)VB.Val(dt.Rows[0]["GRPFORMNO"].ToString().Trim());
                f.FmDISPSEQ = (int)VB.Val(dt.Rows[0]["DISPSEQ"].ToString().Trim());
                f.FmFORMNAME = dt.Rows[0]["FORMNAME"].ToString().Trim();
                f.FmFORMNAMEPRINT = dt.Rows[0]["FORMNAMEPRINT"].ToString().Trim();
                f.FmFORMTYPE = dt.Rows[0]["FORMTYPE"].ToString().Trim();

                f.FmFORMCD = dt.Rows[0]["FORMCD"].ToString().Trim();
                f.FmPROGFORMNAME = dt.Rows[0]["PROGFORMNAME"].ToString().Trim();
                f.FmDOCFORMNAME = dt.Rows[0]["DOCFORMNAME"].ToString().Trim();
                f.FmINOUTCLS = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                f.FmFORMCNT = (int)VB.Val(dt.Rows[0]["FORMCNT"].ToString().Trim());

                f.FmFLOWGB = dt.Rows[0]["FLOWGB"].ToString().Trim();
                f.FmUSEDEPT = dt.Rows[0]["USEDEPT"].ToString().Trim();
                f.FmVIEWAUTH = dt.Rows[0]["VIEWAUTH"].ToString().Trim();
                f.FmWRITEAUTH = dt.Rows[0]["WRITEAUTH"].ToString().Trim();
                f.FmVISITSDEPT = dt.Rows[0]["VISITSDEPT"].ToString().Trim();

                f.FmUSECHECK = (int)VB.Val(dt.Rows[0]["USECHECK"].ToString().Trim());
                f.FmMIBICHECK = (int)VB.Val(dt.Rows[0]["MIBICHECK"].ToString().Trim());
                f.FmCERTCHECK = (int)VB.Val(dt.Rows[0]["CERTCHECK"].ToString().Trim());
                f.FmCERTBOTH = (int)VB.Val(dt.Rows[0]["CERTBOTH"].ToString().Trim());
                f.FmCERTNUM = (int)VB.Val(dt.Rows[0]["CERTNUM"].ToString().Trim());
                f.FmALIGNGB = (int)VB.Val(dt.Rows[0]["ALIGNGB"].ToString().Trim());
                f.FmVIEWGROUP = (int)VB.Val(dt.Rows[0]["VIEWGROUP"].ToString().Trim());
                f.FmDOCPRINTHEAD = (int)VB.Val(dt.Rows[0]["DOCPRINTHEAD"].ToString().Trim());

                f.FmREGDATE = dt.Rows[0]["REGDATE"].ToString().Trim();
                f.FmCONVIMAGE = (int)VB.Val(dt.Rows[0]["CONVIMAGE"].ToString().Trim());
                f.FmOLDGB = (int)VB.Val(dt.Rows[0]["OLDGB"].ToString().Trim());

                f.FmPRINTTYPE = (int)VB.Val(dt.Rows[0]["PRINTTYPE"].ToString().Trim());
                f.FmFLOWITEMCNT = (int)VB.Val(dt.Rows[0]["FLOWITEMCNT"].ToString().Trim());
                f.FmFLOWHEADCNT = (int)VB.Val(dt.Rows[0]["FLOWHEADCNT"].ToString().Trim());
                f.FmFLOWINPUTSIZE = (int)VB.Val(dt.Rows[0]["FLOWINPUTSIZE"].ToString().Trim());
                dt.Dispose();
                dt = null;
                return f;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// FORMNO로 최신 기록지 정보를 가지고 온다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <returns></returns>
        public static EmrForm SerEmrFormUpdateNo(PsmhDb pDbCon, string strFormNo)
        {

            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            EmrForm f = new EmrForm();
            f = ClearEmrForm();

            try
            {

                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "    FORMNO         ,  UPDATENO       ,  GRPFORMNO      ,  DISPSEQ        ,  FORMNAME       ,  ";
                SQL = SQL + ComNum.VBLF + "    FORMNAMEPRINT  ,  FORMTYPE       ,  FORMCD         ,  PROGFORMNAME   ,  DOCFORMNAME    ,  ";
                SQL = SQL + ComNum.VBLF + "    INOUTCLS       ,  FORMCNT        ,  USEDEPT        ,  VIEWAUTH       ,  ";
                SQL = SQL + ComNum.VBLF + "    WRITEAUTH      ,  USECHECK       ,  MIBICHECK      ,  CERTCHECK      ,  CERTBOTH       ,  ";
                SQL = SQL + ComNum.VBLF + "    CERTNUM        ,  ALIGNGB        ,  VIEWGROUP      , DOCPRINTHEAD, REGDATE, CONVIMAGE, OLDGB  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "      WHERE FORMNO = " + VB.Val(strFormNo);
                SQL = SQL + ComNum.VBLF + "        AND UPDATENO = (SELECT MAX(UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "                            WHERE FORMNO = " + VB.Val(strFormNo);
                SQL = SQL + ComNum.VBLF + "                              AND USECHECK = 1 --사용체크 한것";
                SQL = SQL + ComNum.VBLF + "                       )";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return null;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return null;
                }

                f.FmFORMNO = (long)VB.Val(dt.Rows[0]["FORMNO"].ToString().Trim());
                f.FmUPDATENO = (int)VB.Val(dt.Rows[0]["UPDATENO"].ToString().Trim());
                f.FmGRPFORMNO = (int)VB.Val(dt.Rows[0]["GRPFORMNO"].ToString().Trim());
                f.FmDISPSEQ = (int)VB.Val(dt.Rows[0]["DISPSEQ"].ToString().Trim());
                f.FmFORMNAME = dt.Rows[0]["FORMNAME"].ToString().Trim();
                f.FmFORMNAMEPRINT = dt.Rows[0]["FORMNAMEPRINT"].ToString().Trim();
                f.FmFORMTYPE = dt.Rows[0]["FORMTYPE"].ToString().Trim();

                f.FmFORMCD = dt.Rows[0]["FORMCD"].ToString().Trim();
                f.FmPROGFORMNAME = dt.Rows[0]["PROGFORMNAME"].ToString().Trim();
                f.FmDOCFORMNAME = dt.Rows[0]["DOCFORMNAME"].ToString().Trim();
                f.FmINOUTCLS = dt.Rows[0]["INOUTCLS"].ToString().Trim();
                f.FmFORMCNT = (int)VB.Val(dt.Rows[0]["FORMCNT"].ToString().Trim());

                f.FmUSEDEPT = dt.Rows[0]["USEDEPT"].ToString().Trim();
                f.FmVIEWAUTH = dt.Rows[0]["VIEWAUTH"].ToString().Trim();
                f.FmWRITEAUTH = dt.Rows[0]["WRITEAUTH"].ToString().Trim();

                f.FmUSECHECK = (int)VB.Val(dt.Rows[0]["USECHECK"].ToString().Trim());
                f.FmMIBICHECK = (int)VB.Val(dt.Rows[0]["MIBICHECK"].ToString().Trim());
                f.FmCERTCHECK = (int)VB.Val(dt.Rows[0]["CERTCHECK"].ToString().Trim());
                f.FmCERTBOTH = (int)VB.Val(dt.Rows[0]["CERTBOTH"].ToString().Trim());
                f.FmCERTNUM = (int)VB.Val(dt.Rows[0]["CERTNUM"].ToString().Trim());
                f.FmALIGNGB = (int)VB.Val(dt.Rows[0]["ALIGNGB"].ToString().Trim());
                f.FmVIEWGROUP = (int)VB.Val(dt.Rows[0]["VIEWGROUP"].ToString().Trim());
                f.FmDOCPRINTHEAD = (int)VB.Val(dt.Rows[0]["DOCPRINTHEAD"].ToString().Trim());

                f.FmREGDATE = dt.Rows[0]["REGDATE"].ToString().Trim();
                f.FmCONVIMAGE = (int)VB.Val(dt.Rows[0]["CONVIMAGE"].ToString().Trim());
                f.FmOLDGB = (int)VB.Val(dt.Rows[0]["OLDGB"].ToString().Trim());
                dt.Dispose();
                dt = null;
                return f;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// EMR 기록지 폼번호로 미비 그룹 컨버터 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFormNo"></param>
        /// <returns></returns>
        public static string GetEmrGrp(PsmhDb pDbCon, string strFormNo, ref string strMiBiCd)
        {
            string rtnVal = string.Empty;
            //경과기록지 그냥 리턴
            switch (strFormNo)
            {
                case "1647": //입퇴원
                    rtnVal = "A";
                    break;
                //경과기록지
                case "963":
                    rtnVal = "D";
                    break;
                case "2464"://의학적 재평가
                    strMiBiCd = "'D10', 'D08'";
                    rtnVal = "D";
                    break;
                case "2241"://전출
                    strMiBiCd = "'D05', 'D11'";
                    rtnVal = "D";
                    break;
                case "2323"://전입
                    strMiBiCd = "'D05', 'D12'";
                    rtnVal = "D";
                    break;
                default:
                    string SQL = string.Empty;
                    OracleDataReader reader = null;

                    try
                    {
                        SQL = "SELECT GRPFORMNO";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRFORM";
                        SQL = SQL + ComNum.VBLF + "    WHERE FORMNO = " + strFormNo;

                        string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

                        if (SqlErr.Length > 0)
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (reader.HasRows && reader.Read())
                        {
                            switch (reader.GetValue(0).ToString().Trim())
                            {
                                case "1002":
                                    rtnVal = "C";
                                    break;
                                case "1004":
                                    rtnVal = "E";
                                    break;
                            }
                        }

                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                    }
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 이중 챠트 정보 가져오는 함수
        /// </summary>
        /// <param name="strPtno"></param>
        /// <returns></returns>
        public static string GetDoubleChart(string strPtno)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string sqlErr = string.Empty;

            try
            {
                SQL = " SELECT PANO ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "ETC_PANO_HIS ";
                SQL += ComNum.VBLF + " WHERE TO_PANO = '" + strPtno + "'";
                SQL += ComNum.VBLF + " GROUP BY PANO ";

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBox(sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return rtnVal;
                }

                if(reader.HasRows && reader.Read())
                {
                    rtnVal = string.Format("이중({0})", reader.GetValue(0).ToString().Trim());
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 인증 차트 메뉴버튼 막기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="mnu"></param>
        /// <param name="p"></param>
        public static void SetChartHead(PsmhDb pDbCon, usFormTopMenu mnu, EmrPatient p = null)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            string SQL= string.Empty;
            string SqlErr= string.Empty; //에러문 받는 변수
            DataTable dt = null;
            int i = 0;

            mnu.dtMedFrDate.Enabled = true;
            mnu.txtMedFrTime.Enabled = true;
            mnu.mbtnTime.Visible = true;
            mnu.mbtnDelete.Visible = true;

            return;

            //TODO
            //2016-07-18 차트 작성일자, 시간 수정차단 (차단:1 , 해제:0)
            SQL= string.Empty;
            SQL = "SELECT BASCD, BASVAL FROM " + ComNum.DB_EMR + "AEMRBASCD";
            SQL = SQL + ComNum.VBLF + "    WHERE BSNSCLS = '의무기록실'";
            SQL = SQL + ComNum.VBLF + "    AND UNITCLS = '차트수정관리'";
            SQL = SQL + ComNum.VBLF + "    AND BASCD = 'ModifyAll'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

            if (VB.Left(dt.Rows[i]["BASVAL"].ToString().Trim(), 1) == "0")
            {
                mnu.dtMedFrDate.Enabled = true;
                mnu.txtMedFrTime.Enabled = true;
            }
            else
            {
                mnu.dtMedFrDate.Enabled = false;
                mnu.txtMedFrTime.Enabled = false;
                mnu.mbtnTime.Visible = false;

                SQL= string.Empty;
                SQL = SQL + ComNum.VBLF + " SELECT BASCD, REMARK1 FROM " + ComNum.DB_EMR + "AEMRBASCD  ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '의무기록실' ";
                SQL = SQL + ComNum.VBLF + "        AND UNITCLS = '코딩권한관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BASVAL = 1 ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (clsType.User.IdNumber == dt.Rows[i]["BASCD"].ToString().Trim())
                    {
                        mnu.dtMedFrDate.Enabled = true;
                        mnu.txtMedFrTime.Enabled = true;
                        mnu.mbtnTime.Visible = true;
                    }
                }
            }

            //2016-07-27 전체사용자 삭제권한 체크
            SQL= string.Empty;
            SQL = SQL + ComNum.VBLF + "            SELECT BASVAL FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "			              WHERE BSNSCLS = '의무기록실'	 ";
            SQL = SQL + ComNum.VBLF + "			              AND UNITCLS = '차트수정관리'	 ";
            SQL = SQL + ComNum.VBLF + "			              AND BASCD = 'DeleteAll' 	 ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

            if (VB.Left(dt.Rows[i]["BASVAL"].ToString().Trim(), 1) == "0")
            {
                if (clsType.User.AuAWRITE == "1")
                {
                    mnu.mbtnDelete.Visible = true;
                }
            }
            else
            {
                mnu.mbtnDelete.Visible = false;
            }

            dt.Dispose();
            dt = null;

            if (p != null)
            {
                // 2016-10-12 당일외래환자, 재원환자는 삭제권한 활성화
                if (clsType.User.AuAWRITE == "1")
                {
                    if (p.inOutCls == "I")
                    {
                        //dt = clsEmrQuery.GetIpdPtInfo(pDbCon, p.ptNo);
                        //if (dt.Rows.Count >= 1)
                        //{
                        //    mnu.mbtnDelete.Visible = true;
                        //}

                        //dt.Dispose();
                        //dt = null;
                    }
                    else
                    {
                        if (p.medFrDate == VB.Left(strCurDateTime, 8))
                        {
                            mnu.mbtnDelete.Visible = true;
                        }
                    }
                }
            }

            //2016-07-18 코딩사용자 삭제권한 체크
            SQL= string.Empty;
            SQL = SQL + ComNum.VBLF + "	SELECT BASCD, REMARK1,  ";
            SQL = SQL + ComNum.VBLF + "            (SELECT BASVAL FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "			   		              WHERE BSNSCLS = '의무기록실'	 ";
            SQL = SQL + ComNum.VBLF + "					              AND UNITCLS = '차트수정관리'	 ";
            SQL = SQL + ComNum.VBLF + "					              AND BASCD = 'DeleteCoding') AS BASVAL 	 ";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD ";
            SQL = SQL + ComNum.VBLF + "    WHERE BSNSCLS = '의무기록실' ";
            SQL = SQL + ComNum.VBLF + "        AND UNITCLS = '코딩권한관리' ";
            SQL = SQL + ComNum.VBLF + "    	AND BASVAL = 1 ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
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
                if (clsType.User.IdNumber == dt.Rows[i]["BASCD"].ToString().Trim())
                {
                    if (VB.Left(dt.Rows[i]["BASVAL"].ToString().Trim(), 1) == "0")
                    {
                        mnu.mbtnDelete.Visible = true;
                    }
                }
            }

            dt.Dispose();
            dt = null;

        }

        











    }

}
