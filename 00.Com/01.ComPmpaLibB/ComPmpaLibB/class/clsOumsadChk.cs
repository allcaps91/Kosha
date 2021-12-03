using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class clsOumsadChk
    {
        /// <summary>
        /// Description : 퇴원후외래진료여부
        /// Author : 박병규
        /// Create Date : 2017.08.24
        /// <param name="ArgPano"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_퇴원후외래진료여부"/>
        public bool CHECK_TM_OPD(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            if (ArgDate.Trim() == "")
                ArgDate = clsPublic.GstrSysDate;

            string strDate = VB.DateAdd("d", -90, ArgDate).ToString("yyyy-MM-dd");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, DEPTCODE, ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "  WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "    AND PANO       = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND OUTDATE    >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBIPD      <> 'D' ";
            SQL += ComNum.VBLF + "    AND GBSTS      <> '9' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE   = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = false;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "퇴원후 첫 진료입니다.(90일이내)" + '\r';
                clsPublic.GstrMsgList += "초/재진 구분값을 [재진]으로 변경합니다." + '\r';
                clsPublic.GstrMsgList += "퇴원과 : " + DtFunc.Rows[0]["DEPTCODE"].ToString().Trim() + '\r';
                clsPublic.GstrMsgList += "퇴원일 : " + DtFunc.Rows[0]["OUTDATE"].ToString().Trim() + '\r' + '\r';
                clsPublic.GstrMsgList += "(흉부외과 금연치료 접수인 경우 초/재진 구분 확인하시기 바랍니다.)" + '\r';

                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

                rtnVal = true;
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }
        public  string GetClinicDept_Inf(PsmhDb pDbCon, string strDeptCode)
        {
            DataTable DtQ = new DataTable();
            string strVal = "";
         
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strDeptCode.Trim() == "")
                return strVal;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DEPT_INF  FROM ADMIN.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strDeptCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }

                if (DtQ.Rows.Count > 0)
                {
                    if (DtQ.Rows[0]["DEPT_INF"].ToString().Trim()!="")
                    {
                        strVal = " (" + DtQ.Rows[0]["DEPT_INF"].ToString().Trim() + ")";
                    }
                   
                }

                DtQ.Dispose();
                DtQ = null;
            }
            catch (Exception ex)
            {
                if (DtQ != null)
                {
                    DtQ.Dispose();
                    DtQ = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }

            return strVal;
        }
        /// <summary>
        /// Description : 입원중협진과외래진료여부
        /// Author : 박병규
        /// Create Date : 2017.08.24
        /// <param name="ArgPano"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_입원중협진과외래진료여부"/>
        public bool CHECK_IPD_CONSULT_OPD(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            DataTable DtSub = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;
            string strDept = string.Empty;
            string strDate = string.Empty;

            if (ArgDate.Trim() == "")
                ArgDate = clsPublic.GstrSysDate;

            strDate = VB.DateAdd("d", -90, ArgDate).ToString("yyyy-MM-dd");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, ";
            SQL += ComNum.VBLF + "        DEPTCODE, IPDNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "  WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "    AND PANO       = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND OUTDATE    >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBIPD      <> 'D' ";
            SQL += ComNum.VBLF + "    AND GBSTS      <> '9' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = false;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND IPDNO = " + Convert.ToInt32(DtFunc.Rows[0]["IPDNO"].ToString()) + " ";
                SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + DtFunc.Rows[0]["INDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";
                SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE,1,2) = 'C-' ";
                SQL += ComNum.VBLF + "  GROUP By SUCODE ";
                SQL += ComNum.VBLF + "  HAVING SUM(Qty*Nal) > 0 ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = false;
                    return rtnVal;
                }

                if (DtSub.Rows.Count > 0)
                {
                    for (int i = 0; i < DtSub.Rows.Count; i++)
                    {
                        strDept = VB.Pstr(DtSub.Rows[i]["SUCODE"].ToString().Trim(), "-", 2);
                        if (ArgDept.Equals(strDept))
                        {
                            rtnVal = true;
                            break;
                        }
                    }
                }

                DtSub.Dispose();
                DtSub = null;

                if (strDept != "")
                {
                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList = "퇴원후 첫 진료입니다.(90일이내)" + '\r';
                    clsPublic.GstrMsgList += "초/재진 구분값을 [재진]으로 변경합니다." + '\r';
                    clsPublic.GstrMsgList += "퇴원과 : " + DtFunc.Rows[0]["DEPTCODE"].ToString().Trim() + '\r';
                    clsPublic.GstrMsgList += "협진과 : " + strDept + '\r';
                    clsPublic.GstrMsgList += "입원일 : " + DtFunc.Rows[0]["INDATE"].ToString().Trim() + '\r' + '\r';
                    clsPublic.GstrMsgList += "퇴원일 : " + DtFunc.Rows[0]["OUTDATE"].ToString().Trim() + '\r' + '\r';
                    clsPublic.GstrMsgList += "(흉부외과 금연치료 접수인 경우 초/재진 구분 확인하시기 바랍니다.)" + '\r';

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }
        public string READ_doscode_Name(PsmhDb pDbCon, string ArgCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";
            if (ArgCode == null )
            {
                return rtnVal;
            }
            try
            {

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     dosfullcode ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ocs_odosage ";
                SQL = SQL + ComNum.VBLF + "     WHERE doscode = '" + ArgCode.Trim() + "' ";


                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["dosfullcode"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// Description : 전화접수예약체크
        /// Author : 박병규
        /// Create Date : 2017.08.28
        /// <param name="ArgPano"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_전화접수예약체크"/>
        public bool CHECK_TEL_RESV(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = false;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 개인미수금 조회
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="ArgPtno"></param>
        /// <seealso cref="OUMSAD_chk.bas : CHK_개인미수금"/>
        /// </summary>
        public long READ_GAIN_MISU(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUM(CASE WHEN GUBUN1 = '1' THEN AMT WHEN GUBUN1 IN ('3','2','4','5') THEN AMT * -1 END) MISUAMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("개인미수금 조회 오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = Convert.ToInt64(VB.Val(DtFunc.Rows[0]["MISUAMT"].ToString().Trim()));
            else
                rtnVal = 0;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 진료의사본과접수체크
        /// Author : 박병규
        /// Create Date : 2017.11.07
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDr">의사코드</param>
        /// <seealso cref="OUMSAD_CHK.BAS:CHK_진료의사본과접수"/>
        /// <history>
        /// 1. 2018.05.18 박병규 : 의사코드 5107는 제외
        /// </history>
        public bool CHECK_JUPSU_DOCTOR(PsmhDb pDbCon, string ArgPtno, string ArgDr)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            if (ArgDr.Trim() == "5107") { return rtnVal; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT c.Pano ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_DOCTOR a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_DOCTOR b, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "INSA_MST c ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND a.DrCode  = '" + ArgDr + "' ";
            SQL += ComNum.VBLF + "    AND a.DrCode  = b.DrCode(+) ";
            SQL += ComNum.VBLF + "    AND b.Sabun   = c.Sabun ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                if (DtFunc.Rows[0]["PANO"].ToString().Trim() == ArgPtno)
                    rtnVal = true;
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : DRG퇴원당일접수제한
        /// Author : 박병규
        /// Create Date : 2017.11.09
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="OUMSAD_CHK.BAS:CHK_DRG퇴원당일접수제한"/>
        public bool CHECK_DRGOUT_TODAYJUPSU_LIMIT(PsmhDb pDbCon, string ArgPtno, string ArgBi, string ArgDate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Bi, ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND OUTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Bi        = '" + ArgBi + "' ";
            SQL += ComNum.VBLF + "    AND GBSTS     <> '9' ";
            SQL += ComNum.VBLF + "    AND GBDRG     = 'D' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 퇴원당일접수제한
        /// Author : 박병규
        /// Create Date : 2017.11.09
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="OUMSAD_CHK.BAS:CHK_퇴원당일접수제한"/>
        public bool CHECK_OUT_TODAYJUPSU_LIMIT(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Bi, ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND (OUTDATE IS NULL OR OUTDATE = TO_DATE('" + ArgDate + "', 'YYYY-MM-DD')) ";
            SQL += ComNum.VBLF + "    AND GBSTS IN ('0','2','3','4','5','6') ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 당일DRG입원대상
        /// Author : 박병규
        /// Create Date : 2017.11.09
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="OUMSAD_CHK.BAS:CHK_당일DRG입원대상"/>
        public string CHECK_TODAY_DRGIPWON(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DEPTCODE ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_DEPTJEPSU ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBDRG     = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["DEPTCODE"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DEPTCODE ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_RESERVED ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND REDATE    = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBDRG     = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["DEPTCODE"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 전화번호 체크
        /// Author : 박병규
        /// Create Date : 2017.11.09
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="OUMSAD_CHK.BAS:TelNo_Check"/>
        public string CHECK_TELNO(string argTel, string argMail, string argBuildNo = "")
        {
            int Inx = 0;
            string strTelNo = "";
            string rtnVal = "";

            if (argTel.Trim() == "")
            {
                return "";
            }

            //유효숫자만 정리
            strTelNo = "";
            for (Inx = 0; Inx < VB.Len(argTel); Inx++)
            {
                if (String.Compare(VB.Mid(argTel, Inx, 1), "0") >= 0 && String.Compare(VB.Mid(argTel, Inx, 1), "9") <= 0)
                {
                    strTelNo += VB.Mid(argTel, Inx, 1);
                }
            }

            if (VB.Len(strTelNo) < 7)
            {
                rtnVal = "전화번호 오류: 국번호가 2자리입니다.";
            }

            else if (VB.Left(argTel, 3) == "054")
            {
                rtnVal = "경북은 지역번호를 입력하지 마세요";
            }

            else
            {
                //지역번호 체크 해제 : 2018.04.16 이채현 책임
                //if (VB.Left(strTelNo, 3) != "070") //인터넷 전화
                //{
                //    if (argBuildNo != "")
                //    {
                //        if (!(String.Compare(argMail, "360") >= 1 && !(String.Compare(argMail, "402") <= 1)))
                //        {
                //            if (VB.Left(strTelNo, 1) != "0" && argMail != "" && argMail != "000")
                //            {
                //                rtnVal = "타지역은 반드시 DDD번호를 입력하세요.";
                //            }
                //        }
                //    }

                //    else
                //    {
                //        if (!(String.Compare(argMail, "712") >= 1 && !(String.Compare(argMail, "799") <= 1)))
                //        {
                //            if (VB.Left(strTelNo, 1) != "0" && argMail != "" && argMail != "000")
                //            {
                //                rtnVal = "타지역은 반드시 DDD번호를 입력하세요.";
                //            }
                //        }
                //    }
                //}
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 외래 미수납오더확인
        /// Author : 박병규
        /// Create Date : 2017.12.12
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_미수납오더확인"/>
        public string CHECK_NOTSUNAP_ORDER(PsmhDb pDbCon, string ArgPtno, string ArgBdate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DEPTCODE, SUCODE, TO_CHAR(BDate,'YYYY-MM-DD') BDATE ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDate     >= TO_DATE('" + VB.DateAdd("D", -90, ArgBdate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDate     <  TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBSUNAP   = '0' ";
            SQL += ComNum.VBLF + "    AND SUCODE IS NOT NULL ";
            SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE,1,2) <> '$$' ";
            SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE,1,1) <> '@' ";
            SQL += ComNum.VBLF + "    AND SUBSTR(SUCODE,1,2) <> '##' ";
            SQL += ComNum.VBLF + "    AND SUCODE    <> 'NSA' ";
            SQL += ComNum.VBLF + "    AND RES       <> '1' ";
            SQL += ComNum.VBLF + "    AND Seqno     >  0 ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = DtFunc.Rows[0]["DEPTCODE"].ToString().Trim() + "^^";
                rtnVal += DtFunc.Rows[0]["BDATE"].ToString().Trim();
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        public string RTN_ComboEr_Num(PsmhDb pDbCon, string ArgPtno, string ArgBdate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDATE ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDate     =  TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE = 'ER' ";
            SQL += ComNum.VBLF + "    AND ER_NUM = '98' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = DtFunc.Rows[0]["BDATE"].ToString().Trim();
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 당일 외래진료 전화접수여부
        /// Author : 박병규
        /// Create Date : 2017.12.12
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_당일전화접수여부"/>
        public bool CHECK_TODAY_TELJUPSU(PsmhDb pDbCon, string ArgPtno, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.DEPTCODE, b.SName, b.jumin1, ";
            SQL += ComNum.VBLF + "        b.jumin2, b.jumin3, a.Drcode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT b ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.PANO        = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND a.ACTDATE     = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND a.DeptCode    = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND a.JIN         = 'E' ";
            SQL += ComNum.VBLF + "    AND a.PANO        = b.PANO ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 보호자내원오더체크
        /// Author : 박병규
        /// Create Date : 2017.12.12
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_보호자내원오더체크"/>
        public bool CHECK_GUARDIAN_ORDER(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '$$42' ";
            SQL += ComNum.VBLF + "    AND GBSUNAP   = '0' ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 외래특정기호신청여부
        /// Author : 박병규
        /// Create Date : 2017.12.14
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgVcode"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_외래특정기호신청여부"/>
        public bool CHECK_SPECIFIC_REQUEST(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgVcode, string ArgDept)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, TO_CHAR(TDATE,'YYYY-MM-DD') TDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CANCER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND FDATE <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND TDATE >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND GUBUN ='2'  "; //희귀난치
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count == 0)
                rtnVal = false;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 부가세오더체크
        /// Author : 박병규
        /// Create Date : 2017.12.14
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_부가세오더체크"/>
        public bool CHECK_TAX_ORDER(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GbSunap   = '0' ";
            SQL += ComNum.VBLF + "    AND OrderCode != 'PT######' ";
            SQL += ComNum.VBLF + "    AND OrderCode != 'NSA' ";
            SQL += ComNum.VBLF + "    AND Qty       > 0 ";
            SQL += ComNum.VBLF + "    AND GbTax     = '1' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = false;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 인체면역결핍오더체크
        /// Author : 박병규
        /// Create Date : 2017.12.14
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_인체면역결핍오더체크"/>
        public bool CHECK_HIV_ORDER(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(Qty*Nal), 0) nQty, SuCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND OrderCode != 'PT######' ";
            SQL += ComNum.VBLF + "    AND OrderCode != 'NSA' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '@V103' ";
            SQL += ComNum.VBLF + "  Group By Sucode  ";
            SQL += ComNum.VBLF + "  Having SUM(Qty*Nal) > 0 ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 해바라기센터정보
        /// author : 박병규
        /// Create Date : 2017-11-06
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgGbn">1.가정폭력 2.성폭력</param>
        /// <seealso cref="OUMSAD_CHK.bas:CHK_해바라기센터정보"/>
        /// </summary>
        public void CHECK_SUNFLOWER(PsmhDb pDbCon, string ArgPtno, string ArgBdate, string ArgGbn)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            string strFdate = string.Empty;
            string strTdate = string.Empty;

            strFdate = VB.Left(ArgBdate.Trim(), 4) + "-01-01";
            strTdate = ArgBdate;

            clsPmpaPb.GnHTAmt = 500000;
            clsPmpaPb.GnHSAmt = 0;
            clsPmpaPb.GnHCAmt = 0;

            if (ArgGbn == "1")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1+AMT2), 0) SAMT ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL += ComNum.VBLF + "  Where 1         = 1 ";
                SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND BDATE     >= TO_DATE('" + strFdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE     <= TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND SUCODE    = 'Y96J' ";
                SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtQ.Rows.Count > 0)
                    clsPmpaPb.GnHSAmt = Convert.ToInt64(DtQ.Rows[0]["SAMT"].ToString());

                DtQ.Dispose();
                DtQ = null;

                clsPmpaPb.GnHCAmt = clsPmpaPb.GnHTAmt - clsPmpaPb.GnHSAmt;

                clsPublic.GstrMsgTitle = "해바라기센터 지원금 안내(가정폭력)";
                clsPublic.GstrMsgList = "가정폭력 진료비 지원대상금 : " + string.Format("{0:#,##0}", clsPmpaPb.GnHTAmt) + '\r';
                clsPublic.GstrMsgList += "기발생 진료비 : " + string.Format("{0:#,##0}", clsPmpaPb.GnHSAmt) + '\r';
                clsPublic.GstrMsgList += "지원금 잔액   : " + string.Format("{0:#,##0}", clsPmpaPb.GnHCAmt) + '\r';

                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
            }
            else if (ArgGbn == "2")
            {
                clsPublic.GstrMsgTitle = "해바라기센터 접수";
                clsPublic.GstrMsgList = "성폭력대상 전액지원금 대상자임.(전액미수처리)" + '\r';

                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
            }

        }

        /// <summary>
        /// Description : 시설환자 번호 조회
        /// Author : 박병규
        /// Create Date : 2017.12.18
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:RTN_OPD_NHIC_시설기호"/>
        public string CHECK_OPD_NHIC_GKIHO(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtChk = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT M2_GKIHO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "  ORDER By SENDTIME DESC ";
            SqlErr = clsDB.GetDataTable(ref DtChk, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtChk.Dispose();
                DtChk = null;
                return rtnVal;
            }

            if (DtChk.Rows.Count > 0)
                rtnVal = DtChk.Rows[0]["M2_GKIHO"].ToString().Trim();

            DtChk.Dispose();
            DtChk = null;

            return rtnVal;
        }

        public string CHECK_BI_FLAG(string ArgBi)
        {
            string rtnval = "";

            switch (ArgBi)
            {
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                case "21":
                case "22":
                case "23":
                case "24":
                case "25":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                    rtnval = "OK";
                    break;

                default:
                    rtnval = "BI";
                    break;
            }

            return rtnval;
        }


        /// <summary>
        /// Description : 금연치료인센티브대상자
        /// Author : 박병규
        /// Create Date : 2017.12.18
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:CHK_금연치료인센티브대상자"/>
        public bool Check_AntiSmoking_Incentive(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtChk = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;
            string strDate = VB.Left(ArgDate.Trim(), 4) + "-01-01";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(COUNT(*), 0) AS CNT ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDate     >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDate     < TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = 'CS' ";
            SQL += ComNum.VBLF + "    AND JINDTL    = '12' ";
            SqlErr = clsDB.GetDataTableEx(ref DtChk, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtChk.Dispose();
                DtChk = null;
                return rtnVal;
            }

            if (DtChk.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtChk.Rows[0]["CNT"].ToString().Trim()) >= 2)
                    rtnVal = true;
            }

            DtChk.Dispose();
            DtChk = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : FM과 진료예약 시각
        /// Author : 박병규
        /// Create Date : 2017.12.19
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:RTN_FM_RESERVE_TIME"/>
        public string CHECK_FM_RESERVE_TIME(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtChk = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "00:00";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') Date3 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND TRUNC(DATE3)  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND PANO          = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE      = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTable(ref DtChk, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                DtChk.Dispose();
                DtChk = null;
                return rtnVal;
            }

            if (DtChk.Rows.Count > 0)
                rtnVal = VB.Right(DtChk.Rows[0]["Date3"].ToString().Trim(), 5);

            DtChk.Dispose();
            DtChk = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 저가약제 체크
        /// Author : 박병규
        /// Create Date : 2017.12.20
        /// <param name="ArgSuCode"></param>
        /// </summary>
        /// <seealso cref="vbSugaRead_new.BAS:Read_DRUG_SUGA_CHK"/>
        public string Check_LowDrug_Suga(PsmhDb pDbCon, string ArgSuCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            //표준코드로 저가약제인지 체크
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext, BCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SuNext    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND GbDrug    = 'Y'  ";
            SqlErr = clsDB.GetDataTable(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 응급실 미시행검사 체크
        /// Author : 박병규
        /// Create Date : 2017.12.21
        /// <param name="ArgSuCode"></param>
        /// </summary>
        /// <seealso cref="frmSunapMain_new.frm:Check_Er_Order"/>
        public string Check_ER_Order(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            DataTable DtFuncSub = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "OK";
            string strError = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.BUN, A.ORDERCODE, A.SUCODE, ";
            SQL += ComNum.VBLF + "        A.ORDERNO, B.ORDERNAME, B.ORDERNAMES, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.ACTDATE ,'YYYY-MM-DD') ACTDATE, BDATE  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_MED + "OCS_ORDERCODE  B ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND A.BDATE   >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.BDATE   <= TO_DATE('" + VB.DateAdd("D", 2, ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.Ptno    = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND A.GBIOE IN ('E','EI')  ";
            SQL += ComNum.VBLF + "    AND A.Bun     >= '52' "; //뇨검사~
            SQL += ComNum.VBLF + "    AND A.BUN     <= '73' "; //~MRI
            SQL += ComNum.VBLF + "    AND (A.GBSTATUS IS NULL OR A.GBSTATUS NOT IN ('D','D-')) ";
            SQL += ComNum.VBLF + "    AND A.ORDERCODE = B.ORDERCODE ";
            SQL += ComNum.VBLF + "  ORDER BY  BUN ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                for(int i = 0; i < DtFunc.Rows.Count; i++)
                {
                    SQL = "";
                    if (string.Compare(DtFunc.Rows[i]["BUN"].ToString(), "52") >= 0 && string.Compare(DtFunc.Rows[i]["BUN"].ToString(), "64") <= 0) //진단검사의학과
                    {
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_SPECMST ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND STATUS    = '00' ";
                        SQL += ComNum.VBLF + "    AND ORDERNO   = '" + DtFunc.Rows[i]["ORDERNO"].ToString() + "' ";
                        SqlErr = clsDB.GetDataTable(ref DtFuncSub, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtFuncSub.Dispose();
                            DtFuncSub = null;
                            DtFunc.Dispose();
                            DtFunc = null;
                            return rtnVal;
                        }

                        strError = "";
                        if (DtFuncSub.Rows.Count > 0)
                            strError = "Y";

                        DtFuncSub.Dispose();
                        DtFuncSub = null;
                    }
                    else //영상의학과
                    {
                        SQL += ComNum.VBLF + " SELECT PANO ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                        SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND XCODE     = '" + DtFunc.Rows[i]["SUCODE"].ToString() + "' ";
                        SQL += ComNum.VBLF + "    AND GBRESERVED = '7' ";
                        SQL += ComNum.VBLF + "    AND ORDERNO   = '" + DtFunc.Rows[i]["ORDERNO"].ToString() + "' ";
                        SqlErr = clsDB.GetDataTable(ref DtFuncSub, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtFuncSub.Dispose();
                            DtFuncSub = null;
                            DtFunc.Dispose();
                            DtFunc = null;
                            return rtnVal;
                        }

                        strError = "";
                        if (DtFuncSub.Rows.Count == 0)
                            strError = "Y";

                        DtFuncSub.Dispose();
                        DtFuncSub = null;
                    }


                    if (strError == "Y") { rtnVal = ""; }
                }
            }
 
            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        public string Read_GnNPInjAmt_SUGACHK(string ArgSuCode)
        {
            DataTable DtFunc = null;

           
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SuNext    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND GBHJJuSa = 'Y' ";
           
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : F항변경가능코드 체크 : 비급여->급여변경가능
        /// Author : 박병규
        /// Create Date : 2017.12.21
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgGb">1.ocs용 2.원무용</param>
        /// </summary>
        /// <seealso cref="vbSugaRead_new.BAS:Read_F항변경_SUGA_CHK"/>
        public string Check_SugbF_Suga(PsmhDb pDbCon, string ArgSuCode, string ArgGb)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SuNext    = '" + ArgSuCode + "' ";

            if (ArgGb == "1")
                SQL += ComNum.VBLF + "    AND GbOcsF    = 'Y' ";
            else
                SQL += ComNum.VBLF + "    AND GbWonF    = 'Y' ";

            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 
        /// <param name="o"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_수가입력여부"/>
        /// <returns></returns>
        /// </summary>
        public bool Check_Input_Suga(PsmhDb pDbCon, SheetView o, string ArgSuCode, string ArgDate = "")
        {
            int nRow = 0;
            string strCode = "";
            bool rtnVal = false;
            int nREAD = 0;

            for (nRow = 0; nRow <= o.GetLastNonEmptyRow(NonEmptyItemFlag.Data); nRow++)
            {
                if (Convert.ToBoolean(o.Cells[nRow, 0].Value) == false)
                {
                    strCode = o.Cells[nRow, 1].Text.Trim();

                    if (ArgSuCode == "진찰료" && ArgDate != "")
                    {
                        if (Check_Jinchal_Suga(pDbCon, strCode, ArgDate))
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                    }
                    else
                    {
                        if (strCode == ArgSuCode)
                        {
                            nREAD += Convert.ToInt16(o.Cells[nRow, 8 ].Text.Trim());
                          //  rtnVal = true;
                          //  return rtnVal;
                        }
                    }
                }
            }
            if (nREAD > 0)
            {
                rtnVal = true;
                return rtnVal;
            }

            return rtnVal;
        }


        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 
        /// <param name="ArgCode">수가코드</param>
        /// <param name="ArgDate">적용일자</param>
        /// <seealso cref="oumsad_chk.BAS:CHK_진찰료수가체크"/>
        /// <returns></returns>
        /// </summary>
        public bool Check_Jinchal_Suga(PsmhDb pDbCon, string ArgCode, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN     = '1' ";
            SQL += ComNum.VBLF + "    AND SDATE     <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SUNEXT    = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("진찰료수가 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 금연치료 상담비용
        /// Author : 박병규
        /// Create Date : 
        /// <param name="ArgJinCode"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgBdate"></param>
        /// <seealso cref="oumsad_chk.BAS:SET_금연치료상담비용"/>
        /// </summary>
        public void Set_NotSmoke_ConsultFee(string ArgJinCode, string ArgBi, string ArgBdate)
        {
            clsPmpaPb.GnSCAmt1 = 0;//금연치료 상담료 본인부담금
            clsPmpaPb.GnSCAmt2 = 0;//금연치료 상담료 공단부담금

            if (ArgJinCode == "AA176SC")
            {
                if (ArgBi == "21" || ArgBi == "22")
                {
                    clsPmpaPb.GnSCAmt1 = 0;
                    clsPmpaPb.GnSCAmt2 = 22830;
                }
                else
                {
                    clsPmpaPb.GnSCAmt1 = 4500;
                    clsPmpaPb.GnSCAmt2 = 18330;
                }
            }
            else if (ArgJinCode == "AA276SC")
            {
                if (ArgBi == "21" || ArgBi == "22")
                {
                    clsPmpaPb.GnSCAmt1 = 0;
                    clsPmpaPb.GnSCAmt2 = 14290;
                }
                else
                {
                    if (clsPmpaPb.GstrSCInCent == "OK") //인센티브 대상자
                    {
                        clsPmpaPb.GnSCAmt1 = 0;
                        clsPmpaPb.GnSCAmt2 = 14290;
                    }
                    else
                    {
                        clsPmpaPb.GnSCAmt1 = 2700;
                        clsPmpaPb.GnSCAmt2 = 11590;
                    }
                }
            }
            else if (ArgJinCode == "AA156SC")
            {
                if (ArgBi == "21" || ArgBi == "22")
                {
                    clsPmpaPb.GnSCAmt1 = 0;
                    clsPmpaPb.GnSCAmt2 = 22830;
                }
                else
                {
                    clsPmpaPb.GnSCAmt1 = 3000;
                    clsPmpaPb.GnSCAmt2 = 19830;
                }
            }
            else if (ArgJinCode == "AA256SC")
            {
                if (ArgBi == "21" || ArgBi == "22")
                {
                    clsPmpaPb.GnSCAmt1 = 0;
                    clsPmpaPb.GnSCAmt2 = 14290;
                }
                else
                {
                    if (clsPmpaPb.GstrSCInCent == "OK") //인센티브 대상자
                    {
                        clsPmpaPb.GnSCAmt1 = 0;
                        clsPmpaPb.GnSCAmt2 = 14290;
                    }
                    else
                    {
                        clsPmpaPb.GnSCAmt1 = 1800;
                        clsPmpaPb.GnSCAmt2 = 12490;
                    }
                }
            }
        }

        /// <summary>
        /// 처치 공휴가산 제외자 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_단순처치코드"/>
        /// <returns></returns>
        public string Check_SimpleDressing(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OrderCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "'";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     =  TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND OrderCode = '$$46' "; //처치 공휴가산 제외자
            SQL += ComNum.VBLF + "  GROUP By OrderCode ";
            SQL += ComNum.VBLF + " Having SUM(Qty*Nal) > 0 ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("단순처치코드 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = "OK";

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 필수예방접종수가체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSucode"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_필수예방접종수가체크"/>
        /// <returns></returns>
        public bool Check_Vaccination(PsmhDb pDbCon, string ArgSucode, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_VACC_MST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSucode.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN     = '1' ";
            SQL += ComNum.VBLF + "    AND SDate     <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL OR DELDATE = '' ";
            SQL += ComNum.VBLF + "     OR DelDate > TO_DATE('" + ArgDate + "','YYYY-MM-DD')) ";
            SQL += ComNum.VBLF + "    AND GBES      = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("필수예방접종수가체크 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 필수예방접종 나이 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="vbSugaRead_new.BAS:Read_BAS_AGE_CHK"/>
        /// <returns></returns>
        public string Check_Bas_Age(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strBirth = "";
            int nStart = 1999;
            int nYyyy = 0;
            int nBase = 0;
            

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Jumin1, Jumin2, Jumin3 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE Pano = '" + ArgPtno + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("필수예방접종 나이 체크 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                strJumin1 = DtQ.Rows[0]["JUMIN1"].ToString().Trim();
                strJumin2 = clsAES.DeAES(DtQ.Rows[0]["JUMIN3"].ToString().Trim());
            }
            else
            {
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            DtQ.Dispose();
            DtQ = null;

            strBirth = ComFunc.GetBirthDate(strJumin1, strJumin2, "-");
            nYyyy = Convert.ToInt32(VB.Left(strBirth.Trim(), 4));
            nBase = Convert.ToInt32(VB.Left(ArgDate.Trim(), 4));

            if (nYyyy >= (nBase - 12))
                rtnVal = "OK";
            else
                ComFunc.MsgBox("필수예방접종 적용 예외입니다. 확인요망!", "알림");

            //12세 11개월까지 포함(155개월 -> 15.5로 계산)
            if (ComFunc.AgeCalcEx_Zero(strJumin1 + strJumin2, ArgDate) < 15.6)
                rtnVal = "OK";

            return rtnVal;
        }

        /// <summary>
        /// 의료질평가지원대상체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:RTN_의료질평가지원대상"/>
        /// <returns></returns>
        public string Check_HealthCare_Jiwon(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT JIWON ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("의료질평가지원대상 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = DtQ.Rows[0]["JIWON"].ToString().Trim();

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        public string JinCode_Aute_SuNext(PsmhDb pDbCon, string ArgCode, string ArgCode2="")
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = ""; 
            string rtnVal = "";
            if (ArgCode == "") { ArgCode = "1"; }
            if (ArgCode2 != "" && ArgCode2 == "05") { ArgCode = "D"; }
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Gubun='1'  ";
            SQL += ComNum.VBLF + "    AND Code  = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("후불예약진찰료산정중 문제가 생겼습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = DtQ.Rows[0]["SuNext"].ToString().Trim();

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        public string JinCode_Auto_SuNext(PsmhDb pDbCon, string ArgCode, string ArgCode2 = "")
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            if (ArgCode == "") { ArgCode = "1"; }
            if (ArgCode2 != "" && ArgCode2 == "05") { ArgCode = "D"; }
            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Gubun='1'  ";
            SQL += ComNum.VBLF + "    AND Code  = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("후불예약진찰료산정중 문제가 생겼습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = DtQ.Rows[0]["SuNext"].ToString().Trim();

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }


        public void Check_BirthDay(PsmhDb pDbCon, string ArgPtno)
        {
            clsOumsad CPO = new ComPmpaLibB.clsOumsad();
            string strBirthDay = "";

            CPO.READ_BAS_PATIENT(pDbCon, ArgPtno);

            clsPmpaPb.GstrBirthStat = "";

            if (clsPmpaType.TBP.Birth.Trim() != "")
            {
                if (clsPmpaType.TBP.GbBirth == "-")
                    strBirthDay = ComFunc.ToSolar(clsPmpaType.TBP.Birth); //음력을 양력으로계산
                else
                    strBirthDay = clsPmpaType.TBP.Birth;
            }
            else if (clsPmpaType.TBP.Jumin1 != "" && clsPmpaType.TBP.Jumin2 != "")
            {
                switch (VB.Left(clsPmpaType.TBP.Jumin2.Trim(), 1))
                {
                    case "0":
                    case "9":
                        strBirthDay = "18" + VB.Left(clsPmpaType.TBP.Jumin1.Trim(), 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 3, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 5, 2);
                        break;
                    case "1":
                    case "2":
                    case "5":
                    case "6":
                        strBirthDay = "19" + VB.Left(clsPmpaType.TBP.Jumin1.Trim(), 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 3, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 5, 2);
                        break;
                    case "3":
                    case "4":
                    case "7":
                    case "8":
                        strBirthDay = "20" + VB.Left(clsPmpaType.TBP.Jumin1.Trim(), 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 3, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 5, 2);
                        break;
                    default:
                        strBirthDay = "19" + VB.Left(clsPmpaType.TBP.Jumin1.Trim(), 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 3, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1.Trim(), 5, 2);
                        break;
                }
            }

            if (VB.Mid(clsPublic.GstrSysDate.Trim(), 6, 5) == VB.Mid(strBirthDay.Trim(), 6, 5))
                clsPmpaPb.GstrBirthStat = "OK";
        }

        /// <summary>
        /// 응급실보호자 출입증 교부여부
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:RTN_응급실보호자출입증여부"/>
        /// <returns></returns>
        public string Check_ER_Pass(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CHUL ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("응급실보호자 출입증 교부여부 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = DtQ.Rows[0]["CHUL"].ToString().Trim();

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 응급실 사본발급 대상
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:RTN_사본발급대상여부"/>
        /// <returns></returns>
        public bool Check_ER_CopyRequest(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_MED + "OCS_MCCERTIFI_REQUEST ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND LSDate    >= TO_DATE('" + VB.DateAdd("D", -1, ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("응급실 사본발급 대상 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["ROWID"].ToString().Trim() != "")
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 건강보험 상병특례 등록확인
        /// </summary>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgFdate">조회시작일</param>
        /// <param name="ArgTdate">조회종료일</param>
        /// <param name="ArgGB">1.암, 2.희귀난치, 3.중증화상</param>
        /// <seealso cref=""/>
        /// <returns></returns>
        public bool Check_Reg_BasCancer(PsmhDb pDbCon, string ArgPtno, string ArgFdate, string ArgTdate, string ArgGB)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, TO_CHAR(TDATE,'YYYY-MM-DD') TDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CANCER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND FDATE <= TO_DATE('" + ArgFdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND TDATE >= TO_DATE('" + ArgTdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + ArgGB + "' "; 
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgSunext"></param>
        /// <seealso cref=""/>
        /// <returns></returns>
        public bool Check_Spread_Locked(string ArgSunext)
        {
            bool rtnVal = false;

            switch (ArgSunext)
            {
                case "CLX200":
                case "GA300":
                case "GABA3":
                case "GABAP1":

                case "C3520":
                case "C4212":
                case "C4220":
                case "C4230":
                case "C4240":
                case "C4280":
                case "C4300":
                case "C4330":
                case "C3340":

                case "C-LENA":
                case "C-PEMA":
                case "C-ARIT":

                case "LANST":
                case "PANT2":
                case "PANTOL":

                case "NEBE6":
                case "NEW300":

                    rtnVal = true;
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 예약시간 시,분 체크
        /// </summary>
        /// <param name="ArgTime"></param>
        /// <seealso cref="Rtime_Check"/>
        /// <returns></returns>
        public string Check_Rtime(string ArgTime)
        {
            string rtnVal = "OK";
            string strHH = string.Empty;
            string strMI = string.Empty;

            strHH = VB.Left(ArgTime.Trim(), 2);
            strMI = VB.Mid(ArgTime.Trim(), 4, 2);

            if (string.Compare(strHH, "08") < 0 || string.Compare(strHH, "17") > 0) { rtnVal = "NO"; }
            if (string.Compare(strMI, "00") < 0 || string.Compare(strMI, "59") > 0) { rtnVal = "NO"; }

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgDate"></param>
        /// <seealso cref=""/>
        /// <returns></returns>
        public bool Check_Sequence_Clear(string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MAGAM7 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_AUTOMAGAM ";
            SQL += ComNum.VBLF + "  WHERE JOBDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["MAGAM7"].ToString().Trim() == "Y")
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 휴일체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgDate">예약일자</param>
        /// <seealso cref=""/>
        /// <returns></returns>
        public bool Check_HolyDay(PsmhDb pDbCon, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT HolyDay FROM " + ComNum.DB_PMPA + "BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE JobDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["HolyDay"].ToString().Trim() == "*")
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 의료급여_진찰료중복체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_의료급여_진찰료중복체크"/>
        /// <returns></returns>
        public bool Check_Jinchal_Duplicate(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SUNEXT IN (SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV ";
            SQL += ComNum.VBLF + "                    WHERE GUBUN   = '1' ";
            SQL += ComNum.VBLF + "                      AND SDATE   <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')) ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("의료급여_진찰료중복체크 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString()) > 0)
                    rtnVal = false;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 진찰료자동발생여부
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_진찰료자동발생여부"/>
        /// <returns></returns>
        public string Check_AC302_Duplicate(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT decode(max(GBNGT),' ','0',max(GBNGT))   GBNGT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_iORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDATE     <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') + 1  ";
            SQL += ComNum.VBLF + "    AND BDATE     >= TO_DATE('2019-01-01','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND GbAct     = '*'  ";
            SQL += ComNum.VBLF + "    AND GBIOE     ='E'  ";
            SQL += ComNum.VBLF + "    AND ( ORDERSITE NOT IN ( 'NDC')  OR ORDERSITE IS NULL ) ";
            SQL += ComNum.VBLF + "    AND SEDATION   ='1'    ";
            SQL += ComNum.VBLF + "    AND BUN   <> '37'   ";
            SQL += ComNum.VBLF + "    UNION ALL ";
            SQL += ComNum.VBLF + " SELECT '0'   GBNGT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND BDATE     >= TO_DATE('2019-01-01','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND GbSunap  = '0'   ";
            SQL += ComNum.VBLF + "    AND SEDATION   ='1'   ";
           
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("소아진정관리료 자동 산정부분에 문제가 생겼습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {

                rtnVal = DtQ.Rows[0]["GBNGT"].ToString().Trim();
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        public bool Check_Jinchal_Auto(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT JIN, AMT7 ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("진찰료자동발생여부체크 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtQ.Rows[0]["AMT7"].ToString()) > 0)
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 의료질평가수가발생
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgSucode"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_의료질평가수가발생"/>
        /// <returns></returns>
        public int Check_HealthCare_Create(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate, string ArgSucode)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SUNEXT    = '" + ArgSucode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("의료질평가수가발생 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString().Trim()) > 0)
                    rtnVal = Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString().Trim());
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 의료질평가입력여부
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_의료질평가입력여부"/>
        /// <returns></returns>
        public bool Check_HealthCare_Input(SheetView o, string ArgDate)
        {
            string strAU214 = "";
            string strAU312 = "";

            bool rtnVal = false;

            for (int i = 0; i <= o.GetLastNonEmptyRow(NonEmptyItemFlag.Data); i++)
            {
                if (Convert.ToBoolean(o.Cells[i, 0].Value) != true)
                {
                    if (o.Cells[i, 1].Text.Trim() == "AU214")
                        strAU214 = "OK";

                    if (o.Cells[i, 1].Text.Trim() == "AU312")
                        strAU312 = "OK";
                }
            }

            if (strAU214 == "OK" && strAU312 == "OK")
                rtnVal = true;

            return rtnVal;
        }

        /// <summary>
        /// 만성질환상병진료대상자
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_만성질환상병진료대상자"/>
        /// <returns></returns>
        public string Check_ChronicIll(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            DataTable DtQs1 = new DataTable();
            DataTable DtQs2 = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ILLCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OILLS ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "  ORDER BY SEQNO ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("만성질환상병진료대상자 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Read_ChronicIll(DtQ.Rows[0]["ILLCODE"].ToString().Trim()) != "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ILLCODE, TO_CHAR(BDate,'YYYY-MM-DD') BDate ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OILLS ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND Ptno      ='" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND BDate     < TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
                    SQL += ComNum.VBLF + "    AND SUBSTR(ILLCODE,1,3) = '" + VB.Left(DtQ.Rows[0]["ILLCODE"].ToString().Trim(), 3) + "' ";
                    SQL += ComNum.VBLF + "  ORDER By BDate DESC ";
                    SqlErr = clsDB.GetDataTable(ref DtQs1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtQs1.Dispose();
                        DtQs1 = null;
                        return rtnVal;
                    }

                    if (DtQs1.Rows.Count > 0)
                    {
                        clsPmpaPb.GnDrugNal = 0;

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SUCODE, NVL(SUM(NAL), 0) SNAL ";
                        SQL += ComNum.VBLF + "   From " + ComNum.DB_MED + "OCS_OORDER ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND Ptno      = '" + ArgPtno + "' ";
                        SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + DtQs1.Rows[0]["BDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
                        SQL += ComNum.VBLF + "    AND BUN IN ('11','12') ";
                        SQL += ComNum.VBLF + "  GROUP BY SUCODE, Nal ";
                        SQL += ComNum.VBLF + "  ORDER By Nal DESC ";
                        SqlErr = clsDB.GetDataTable(ref DtQs2, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtQs2.Dispose();
                            DtQs2 = null;
                            return rtnVal;
                        }

                        if (DtQs2.Rows.Count > 0)
                        {
                            clsPmpaPb.GnDrugNal = Convert.ToInt32(DtQs2.Rows[0]["SNAL"].ToString());
                            rtnVal = DtQs1.Rows[0]["BDate"].ToString().Trim() + "@@" + DtQ.Rows[0]["ILLCODE"].ToString().Trim();
                        }

                        DtQs2.Dispose();
                        DtQs2 = null;
                    }

                    DtQs1.Dispose();
                    DtQs1 = null;
                }
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 만성질환상병진료대상자
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_만성질환상병진료대상자"/>
        /// <returns></returns>
        public string Read_ChronicIll(string ArgCode)
        {
            string rtnVal = "";

            if (ArgCode == "") { return rtnVal; }

            if (string.Compare(ArgCode, "I10") >= 0 && string.Compare(ArgCode, "I12") <= 0) { rtnVal = "고혈압"; }
            if (ArgCode == "I15") { rtnVal = "고혈압"; }
            if (string.Compare(ArgCode, "E10") >= 0 && string.Compare(ArgCode, "E14") <= 0) { rtnVal = "당뇨병"; }
            if (string.Compare(ArgCode, "F00") >= 0 && string.Compare(ArgCode, "F99") <= 0) { rtnVal = "정신 및 행동장애"; }
            if (string.Compare(ArgCode, "G40") >= 0 && string.Compare(ArgCode, "G41") <= 0) { rtnVal = "정신 및 행동장애"; }
            if (string.Compare(ArgCode, "A15") >= 0 && string.Compare(ArgCode, "A16") <= 0) { rtnVal = "호흡기결핵"; }
            if (ArgCode == "A19") { rtnVal = "호흡기결핵"; }
            if (string.Compare(ArgCode, "I05") >= 0 && string.Compare(ArgCode, "I09") <= 0) { rtnVal = "심장질환"; }
            if (string.Compare(ArgCode, "I20") >= 0 && string.Compare(ArgCode, "I27") <= 0) { rtnVal = "심장질환"; }
            if (string.Compare(ArgCode, "I30") >= 0 && string.Compare(ArgCode, "I52") <= 0) { rtnVal = "심장질환"; }
            if (string.Compare(ArgCode, "I60") >= 0 && string.Compare(ArgCode, "I69") <= 0) { rtnVal = "대뇌혈괄질환"; }
            if (string.Compare(ArgCode, "G00") >= 0 && string.Compare(ArgCode, "G37") <= 0) { rtnVal = "신경계질환"; }
            if (string.Compare(ArgCode, "G43") >= 0 && string.Compare(ArgCode, "G83") <= 0) { rtnVal = "신경계질환"; }
            if (string.Compare(ArgCode, "C00") >= 0 && string.Compare(ArgCode, "C97") <= 0) { rtnVal = "악성신생물"; }
            if (string.Compare(ArgCode, "D00") >= 0 && string.Compare(ArgCode, "D09") <= 0) { rtnVal = "악성신생물"; }
            if (string.Compare(ArgCode, "E00") >= 0 && string.Compare(ArgCode, "E07") <= 0) { rtnVal = "갑상선의장애"; }
            if (ArgCode == "B18") { rtnVal = "간의질환"; }
            if (ArgCode == "B19") { rtnVal = "간의질환"; }
            if (string.Compare(ArgCode, "K70") >= 0 && string.Compare(ArgCode, "K77") <= 0) { rtnVal = "간의질환"; }
            if (ArgCode == "N18") { rtnVal = "만성신부전증"; }

            return rtnVal;
        }

        /// <summary>
        /// 이전건수 체크하여 수납시 동시발생되면 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgSucode"></param>
        /// <seealso cref="oumsad_chk.BAS:RTN_AL200_이전건수"/>
        /// <seealso cref="oumsad_chk.BAS:RTN_AL651_이전건수"/>
        /// <returns></returns>
        public int Rtn_Cost_BeforeCnt(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate, string ArgSucode)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSucode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox(ArgSucode + " 이전건수 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString().Trim());

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 예약일자 동일과 예약확인
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_예약일자_예약확인"/>
        /// <returns></returns>
        public bool Check_Reserved(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(COUNT(*), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND DATE3     >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DATE3     <  TO_DATE('" + VB.DateAdd("D", 1, ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND (TRANSDATE IS NULL OR TRANSDATE = '') ";
            SQL += ComNum.VBLF + "    AND (RETDATE IS NULL OR RETDATE = '') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString().Trim()) > 0)
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgNum"></param>
        /// <seealso cref="oumsad.BAS:HandPhoneNumber_Check"/>
        /// <returns></returns>
        public string Check_Hphone(string ArgNum)
        {
            string strHphone = "";
            string rtnVal = "OK";

            if (ArgNum.Trim() == "") { return rtnVal; }

            for (int i = 1; i <= ArgNum.Trim().Length; i++)
            {
                if (VB.Asc(VB.Mid(ArgNum, i, 1)) >= 48 && VB.Asc(VB.Mid(ArgNum, i, 1)) <= 57)
                    strHphone = strHphone + VB.Mid(ArgNum, i, 1);
            }

            switch(VB.Left(strHphone.Trim(), 3))
            {
                case "010":
                case "011":
                case "016":
                case "017":
                case "018":
                case "019":
                case "070":
                    break;

                default:
                    rtnVal = "NO";
                    break;
            }

            if (strHphone.Length < 10) { rtnVal = "NO"; }

            return rtnVal;
        }

        /// <summary>
        /// 마약처방대상자체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_마약처방대상자체크"/>
        /// <returns></returns>
        public bool Check_High_Order(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.SUCODE, SUM(A.QTY*A.NAL) SQTY ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_ERP + "DRUG_JEP B ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND A.BDATE       = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.PTNO        = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND A.SUCODE      = B.JEPCODE ";
            SQL += ComNum.VBLF + "    AND B.CHENGGU     = '09' ";
            SQL += ComNum.VBLF + "    AND A.DEPTCODE    = 'ER' ";
            SQL += ComNum.VBLF + "  GROUP BY A.SUCODE ";
            SQL += ComNum.VBLF + " Having Sum(a.Qty * a.Nal) > 0 ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

       

        /// <summary>
        /// 주소입력체크
        /// </summary>
        /// <param name="ArgZip1"></param>
        /// <param name="ArgZip2"></param>
        /// <param name="ArgJuso"></param>
        /// <param name="ArgBuildNo"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_주소입력체크"/>
        /// <returns></returns>
        public bool Check_Input_Juso(PsmhDb pDbCon, string ArgZip1, string ArgZip2, string ArgJuso, string ArgBuildNo)
        {
            ComQuery CQ = new ComQuery();
            bool rtnVal = false;

            for (int i = 1; i <= ArgJuso.Length; i++)
            {
                if (VB.IsNumeric(VB.Mid(ArgJuso, i, 1)) == true)
                {
                    rtnVal = true;
                    break;
                }
            }

            if (VB.I(ArgJuso, "주소미상") > 1) { rtnVal = false; }

            if (ArgZip2.Length < 3)
            {
                if (CQ.Read_RoadJuso(pDbCon, ArgBuildNo) == "")
                    rtnVal = false;
            }
            else
            {
                if (CQ.Read_Juso(pDbCon, ArgZip1 + ArgZip2) == "" || ArgJuso == "")
                    rtnVal = false;
            }

            if (ArgZip1 == "" || ArgZip1 == "000" || ArgZip2 == "" || ArgZip2 == "000") { rtnVal = false; }

            return rtnVal;
        }

        /// <summary>
        /// 해당과의 의사코드인지 Check
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDr"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_BAS_DOCTOR"/>
        /// <returns></returns>
        public bool Check_Bas_Doctor(PsmhDb pDbCon, string ArgDept, string ArgDr)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DrCode FROM BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DrCode    = '" + ArgDr + "' ";
            SQL += ComNum.VBLF + "    AND (Tour <> 'Y' OR Tour IS NULL) ";
            SQL += ComNum.VBLF + "    AND (DrDept1 = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "         OR  DrDept2 = '" + ArgDept + "') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당과 의사가 아니거나, 퇴사 의사코드임.");
                rtnVal = false;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 응급실 수납시 야간,공휴체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgNgt"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_HolyDay"/>
        /// <returns></returns>
        public bool Check_Ngt_HolyDay(PsmhDb pDbCon, string ArgDate, string ArgNgt)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = true;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT HolyDay  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND JobDate   = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["HOLYDAY"].ToString().Trim() == "*")
                {
                    if (ArgNgt != "1")
                        ComFunc.MsgBox("진료일이 공휴일임..야간구분(심야/휴일 = 1) 을 확인하시기 바랍니다.");
                }
                else
                {
                    if (clsPublic.GstrTempHoliday != "*" && ArgNgt == "1")
                        ComFunc.MsgBox("휴일이 아닌데 휴일구분[1]이 되었음. 휴일 = 1, 야간 = 2 로 설정후 수납요망");
                }
            }

            DtQ.Dispose();
            DtQ = null;

            if (clsPublic.GstrTempHoliday == "*" && ArgNgt != "1")
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "◆ 공휴가산 확인요망 ◆" + '\r' + '\r';
                clsPublic.GstrMsgList += "무시하고 수납을 계속 진행하시겠습니까?" + '\r' + '\r';
                clsPublic.DiResult = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button1);

                if (clsPublic.DiResult == DialogResult.No)
                    rtnVal = false;
            }

            return rtnVal;
        }

        /// <summary>
        /// 재원여부 Check
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_재원여부"/>
        /// <returns></returns>
        public string Check_Ipd_Master(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            clsPmpaPb.GstrChkMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, AMSET1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ROUTDATE, 'YYYY-MM-DD') ROUTDATE, DEPTCODE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(OUTDATE, 'YYYY-MM-DD') OUTDATE, GBDRG ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND OUTDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND GBSTS = '0' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                clsPmpaPb.GstrChkMsg = DtQ.Rows[0]["INDATE"].ToString().Trim() + "^^";
                clsPmpaPb.GstrChkMsg += DtQ.Rows[0]["OUTDATE"].ToString().Trim() + "^^";
                clsPmpaPb.GstrChkMsg += DtQ.Rows[0]["DEPTCODE"].ToString().Trim() + "^^";
                clsPmpaPb.GstrChkMsg += DtQ.Rows[0]["GBDRG"].ToString().Trim() + "^^";

                rtnVal = "OK";
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// Bas_Bcode 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgCode"></param>
        /// <seealso cref="frmSunapMain_New.frm:Read_Resv_Exam_Suga_Chk"/>
        /// <returns></returns>
        public string Check_Bas_Bcode(PsmhDb pDbCon, string ArgGubun, string ArgCode)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN         = '" + ArgGubun + "' ";
            SQL += ComNum.VBLF + "    AND TRIM(CODE)    = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = "OK";

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// VCODE 비필요내시경
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgSucode"></param>
        /// <seealso cref="vbfunc.bas:READ_V코드비필요내시경_CHK"/>
        /// <seealso cref="vbfunc.bas:READ_V코드필요내시경_CHK"/>
        /// <returns></returns>
        public string Check_Vcode_Endoscope(PsmhDb pDbCon, string ArgGubun, string ArgSucode)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN         = '" + ArgGubun + "' ";
            SQL += ComNum.VBLF + "    AND TRIM(CODE)    = '" + ArgSucode + "'  ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = "OK";

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 마약관련코드 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgSucode"></param>
        /// <seealso cref="oumsad_chk.bas:Read_SUGA_마약CHK"/>
        /// <returns></returns>
        public string Check_High_Suga(PsmhDb pDbCon, string ArgSucode)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT JepCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "DRUG_JEP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND JepCode   = '" + ArgSucode + "' ";
            SQL += ComNum.VBLF + "    AND chenggu   = '09'  "; // 마약코드
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = "OK";

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 마약관련코드 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_마약관련코드"/>
        /// <returns></returns>
        public bool Check_High_Sunap(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND Remark IN ('수납','수납+예약','환불','부분취소') ";
            SQL += ComNum.VBLF + "  ORDER BY ENTDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }


        /// <summary>
        /// 물리치료 수납
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_SUNAP_물리치료"/>
        /// <returns></returns>
        public bool Check_PT_Sunap(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuCode, NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BUN       = '24' ";
            SQL += ComNum.VBLF + "    AND SUCODE LIKE 'MM%' ";
            SQL += ComNum.VBLF + "  GROUP BY SUCODE ";
            SQL += ComNum.VBLF + " HAVING SUM(QTY*NAL)  > 0 ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToDouble(DtQ.Rows[0]["CNT"].ToString()) > 0)
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 관절강내 주사체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="vbSugaRead_new.bas:Read_SUGA_관절강내CHK"/>
        /// <returns></returns>
        public string Check_Steroid_Suga(PsmhDb pDbCon, string ArgSuCode)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT ";
            SQL += ComNum.VBLF + "  WHERE SuNext    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND SugbB     = '9'  "; // 관절강내주사
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = "OK";

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 관절강내 수납
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_SUNAP_관절광내"/>
        /// <returns></returns>
        public bool Check_Steroid_Sunap(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUCODE, NVL(SUM(QTY*NAL), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND (TRIM(SUCODE) IN (  ";
            SQL += ComNum.VBLF + "                          SELECT TRIM(SUNEXT)  FROM " + ComNum.DB_PMPA + "BAS_SUT  ";
            SQL += ComNum.VBLF + "                           WHERE  SugbB = '9' )";
            SQL += ComNum.VBLF + "     OR  SuCode in ('KK090','KK061') ) ";
            SQL += ComNum.VBLF + "  GROUP BY SUCODE ";
            SQL += ComNum.VBLF + " HAVING SUM(QTY*NAL)  > 0 ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToDouble(DtQ.Rows[0]["CNT"].ToString()) > 0)
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        public bool Check_Except_V352_AGE(string ArgPtno, string ArgDiease1)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Rowid ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_ILLS a ";
            SQL += ComNum.VBLF + "  Where 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.ILLCODE        = '" + ArgDiease1 + "' ";
            SQL += ComNum.VBLF + "    And (a.ILLCODE like 'A044%' or a.ILLCODE like 'B008%' or  a.ILLCODE like 'G538%' or a.ILLCODE like 'J41%' )  ";
            SQL += ComNum.VBLF + "    AND a.GbV352 ='*'  ";
         
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["Rowid"].ToString() != "")
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }
        public bool Check_Except_V352_Return(string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Rowid ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "etc_return a ";
            SQL += ComNum.VBLF + "  Where 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.Pano        = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    And a.enttime       >= TO_DATE('" + ArgDate + "','YYYY-MM-DD')-90 ";
            SQL += ComNum.VBLF + "    And a.actDATE       >= TO_DATE('2018-11-01','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["Rowid"].ToString() != "")
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }
        /// <summary>
        /// 당뇨주사제대상V252제외
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_당뇨주사제대상V252제외"/>
        /// <returns></returns>
        public bool Check_Except_V252(string ArgPtno, string ArgDept, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Rowid ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_SLIP a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
            SQL += ComNum.VBLF + "  Where 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.Pano        = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    And a.BDATE       = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    And a.DeptCode    = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    And b.DAICODE     = '396' "; // 당뇨약제
            SQL += ComNum.VBLF + "    And a.Bun         = '20' "; //주사만
            SQL += ComNum.VBLF + "    And a.SuNext      = b.SuNext(+) ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["Rowid"].ToString() != "")
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <seealso cref="oumsad.bas : Read_Yeyak_Exam_Bun_Chk"/>
        public string Check_Yeyak_Exam_Bun()
        {
            string rtnVal = "00";

            string[] strTemp = new string[11];

            for (int i = 0; i <= 10; i++)
                strTemp[i] = "";

            for (int i = 0; i < 999; i++)
            {
                if (string.IsNullOrEmpty(clsPmpaType.SW[i].SuCode) == false && string.IsNullOrEmpty(clsPmpaType.SW[i].SuNext) == false)
                {
                    if (clsPmpaType.SW[i].OrderNo != 0)
                    {
                        switch (clsPmpaType.SW[i].Bun.Trim())
                        {
                            case "48":
                                strTemp[1] = "01"; //내시경
                                break;

                            case "71":
                            case "72":
                            case "73":
                                strTemp[2] = "02"; //방사선
                                break;
                        }
                    }
                }
            }

            if (strTemp[1] != "" && strTemp[2] != "")
                rtnVal = "99";
            else if (strTemp[1] != "")
                rtnVal = "01";
            else if (strTemp[2] != "")
                rtnVal = "02";

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_IPD_NEW_MASTER"/>
        /// <returns></returns>
        public bool Check_Ipd_Master(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND INDATE    >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND OUTDATE   <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND GBSTS     < '7' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        public bool CHK_SANID_GASAN(PsmhDb pDbCon, string ArgPtno, string argBI, string argDrcode, string ArgDate)
        { 
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = ""; 
            bool rtnVal = false;
            if (argBI != "31") { return rtnVal; }
            if (argDrcode != "2501" && argDrcode != "2314") { return rtnVal; }
   
            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "BAS_SANID ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DATE2 >= TO_DATE('2019-05-01','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND JONG = '*' ";
            SQL += ComNum.VBLF + "    AND GBRESULT = '5' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        /// <seealso cref="oumsad.bas : READ_BunupSayu"/>
        public string Read_BunupSayu(string ArgCode)
        {
            string rtnVal = "";

            switch (ArgCode)
            {
                case "":
                case "0":
                    rtnVal = "원외처방전 대상 의약품";
                    break;
                case "1":
                    rtnVal = "예방접종약";
                    break;
                case "2":
                    rtnVal = "진단의약품";
                    break;
                case "3":
                    rtnVal = "결핵치료제";
                    break;
                case "4":
                    rtnVal = "조제실제제";
                    break;
                case "5":
                    rtnVal = "마약";
                    break;
                case "6":
                    rtnVal = "방사선의약품";
                    break;
                case "7":
                    rtnVal = "기계장치이용";
                    break;
                case "8":
                    rtnVal = "시술필요약품";
                    break;
                case "9":
                    rtnVal = "차광,냉장주사제";
                    break;
                case "A":
                    rtnVal = "차광,냉장주사제";
                    break;
                case "B":
                    rtnVal = "항암주사제";
                    break;
                case "*":
                    rtnVal = "원내약품과동시조제";
                    break;
                case "#":
                    rtnVal = "시범기간중원내조제";
                    break;
                case "$":
                    rtnVal = "원내조제와동시조제";
                    break;
                default:
                    rtnVal = "기타 원내조제";
                    break;  
            }

            return rtnVal;
        }

        /// <summary>
        /// 예약조정대상자
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_예약조정대상자"/>
        /// <returns></returns>
        public string Check_Reserved_Adjust(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(Date3,'YYYY-MM-DD') DATE3 ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_RESERVED_NEW ";
            SQL += ComNum.VBLF + "  Where 1             = 1 ";
            SQL += ComNum.VBLF + "    AND Pano          = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE      = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND TRANSDATE IS NOT NULL ";
            SQL += ComNum.VBLF + "    AND TRUNC(DATE2)  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DATE2         > DATE3 ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = DtQ.Rows[0]["DATE3"].ToString().Trim();

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgSeq"></param>
        /// <param name="ArgBi"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_ER_AC101_SLIP"/>
        /// <returns></returns>
        public bool Check_ER_AC101_Slip(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept, long ArgSeq, string ArgBi)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND Bi        = '" + ArgBi + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND SeqNo     =  " + ArgSeq + " ";
            SQL += ComNum.VBLF + "    AND SUNEXT    = 'AC101' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        public string DATE_RESERVED(PsmhDb pDbCon, string ArgPano, string ArgRdate, string ArgBdate, string ArgDept)
        {
            string rtnVal = ArgRdate;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgRdate != "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "  SELECT ";
                SQL += ComNum.VBLF + "  TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI')  DATE3 ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL += ComNum.VBLF + "  WHERE DATE1= TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND PANO= '" + ArgPano + "'";
                SQL += ComNum.VBLF + "  AND DEPTCODE= '" + ArgDept + "'";
                SQL += ComNum.VBLF + "  AND TRanSDATE IS NULL and RETDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (dt.Rows.Count == 1)
                {
                    rtnVal = dt.Rows[0]["DATE3"].ToString().Trim();
                }



                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }


            return rtnVal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgSeq"></param>
        /// <param name="ArgBi"></param>
        /// <seealso cref="oumsad_chk.bas:CHK_ER_PCLR_SLIP"/>
        /// <returns></returns>
        public bool Check_ER_PCLR_Slip(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept, long ArgSeq, string ArgBi, string ArgJea, string ArgPart)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUNEXT, NVL(SUM(QTY*NAL), 0) SQTY ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";

            if (ArgJea != "")
                SQL += ComNum.VBLF + "AND Bi        = '" + ArgBi + "' ";
            else
                SQL += ComNum.VBLF + "AND PART      = '" + ArgPart + "'";

            SQL += ComNum.VBLF + "   AND DEPTCODE   = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "   AND SeqNo      = '" + ArgSeq + "' ";
            SQL += ComNum.VBLF + "   AND SUNEXT     = 'PCLR' ";
            SQL += ComNum.VBLF + " GROUP By SUNEXT ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToDouble(DtQ.Rows[0]["SQTY"].ToString()) > 0)
                    rtnVal = true;
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgDept"></param>
        /// <seealso cref="oumsad.bas:READ_V810_CHACK"/>
        /// <returns></returns>
        public bool Check_V810(PsmhDb pDbCon, string ArgPtno, string ArgDate, string ArgDept)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MSEQNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND GBBUN     = '3' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
                rtnVal = true;

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 
        /// <param name="o"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="oumsad_chk.BAS:CHK_재출력수가입력여부"/>
        /// <returns></returns>
        /// </summary>
        public bool Check_RePrint_Input_Suga(PsmhDb pDbCon, SheetView o, string ArgSuCode, string ArgDate = "")
        {
            int nRow = 0;
            string strCode = "";
            bool rtnVal = false;

            for (nRow = 0; nRow <= o.GetLastNonEmptyRow(NonEmptyItemFlag.Data); nRow++)
            {
                strCode = o.Cells[nRow, 0].Text.Trim();

                if (strCode != "")
                {
                    if (ArgSuCode == "진찰료" && ArgDate != "")
                    {
                        if (Check_Jinchal_Suga(pDbCon, strCode, ArgDate))
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                    }
                    else
                    {
                        if (strCode == ArgSuCode)
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                    }
                }
            }

            return rtnVal;
        }

    }
}
