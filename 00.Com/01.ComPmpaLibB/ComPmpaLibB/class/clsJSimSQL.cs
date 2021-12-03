using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    public class clsJSimSQL
    {
        //string SQL = "";
        //string SqlErr = ""; //에러문 받는 변수
        public static string strJSimView = "";
        public static string strJSimSort = "";

        public DataTable sel_JSimPatList(PsmhDb pDbCon, clsPmpaType.JPatLst JPL)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT /*+ INDEX_DESC(ADMIN.ipd_trans INDEX_IPDTRS0) */ A.IPDNO, A.TRSNO, A.Pano      ";
                SQL += ComNum.VBLF + "        ,A.GBIPD, B.SName, B.WARDCODE, B.RoomCode   ";
                SQL += ComNum.VBLF + "        ,A.DeptCode, A.DrCode,  A.ILSU, A.VCODE, A.OGPDBUN, A.OGPDBUNDTL,A.FCode                      ";
                SQL += ComNum.VBLF + "        ,TO_CHAR(A.InDate,'YYYY-MM-DD') InDate                                                   ";
                SQL += ComNum.VBLF + "        ,TO_CHAR(A.JSIM_LDATE ,'YYYY-MM-DD') JSIM_LDATE ,JSIM_SABUN ,JSIM_SET, B.JSIM_REMARK,a.Ilsu   " ;
                SQL += ComNum.VBLF + "        ,b.ilsu ilsu2,a.DrgCode ";
                SQL += ComNum.VBLF + "        ,DECODE(a.Bohun, '3', '장애', '') Bohun ";
                SQL += ComNum.VBLF + "        ,DECODE(A.GbDRG, 'D', A.GbDRG, '') GBDRG ";
                SQL += ComNum.VBLF + "        ,DECODE(B.OP_JIPYO,'Y',B.OP_JIPYO,'') OP_JIPYO ";
                SQL += ComNum.VBLF + "        ,ADMIN.FC_MIR_IPDID_CHK(A.TRSNO) MIR "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_NUR_MASTER_ROUTDATE(A.PANO, A.IPDNO) ROUTDATE "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_BAS_DOCTOR_DRNAME(A.DrCode) DRNAME "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_IPD_GBSTS_NM(A.GBSTS) GBSTS "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_BI_NM(A.Bi) BI ";
                SQL += ComNum.VBLF + "        ,ADMIN.FC_OCS_IILLS(A.PANO, A.IPDNO, TO_CHAR(A.INDATE, 'YYYY-MM-DD')) ILLS "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_IPD_GUB_NOTICE(A.PANO, A.IPDNO) IPDGUB "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_ORAN_MASTER_YN(A.PANO, TO_CHAR(A.INDATE, 'YYYY-MM-DD')) OPYN ";
                SQL += ComNum.VBLF + "        ,ADMIN.FC_OCS_IILLS_IPDETC(A.PANO, A.IPDNO, TO_CHAR(A.INDATE, 'YYYY-MM-DD'),A.Bi) IPDETC "; 
                SQL += ComNum.VBLF + "        ,ADMIN.FC_INSA_MST_KORNAME(JSIM_SABUN) JSIMSABUN ";
                SQL += ComNum.VBLF + "        ,ADMIN.FC_INSA_MST_KORNAME(JSIM_SET) JSIMSET ";
                SQL += ComNum.VBLF + "        ,ADMIN.FC_OCS_CPNOTE(A.IPDNO) CPNOTE ";
                SQL += ComNum.VBLF + "        ,A.JINDTL,A.Gbilban2 ,ADMIN.FC_NUR_MASTER_ROUTDATE2(A.PANO, A.IPDNO) ROUTDATE2  "; 
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A,        ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B    ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.INDATE >= TO_DATE('2014-10-12','YYYY-MM-DD') ";     //굳이 날짜를 줘야 하는가??
                SQL += ComNum.VBLF + "    AND A.INDATE <= TRUNC(SYSDATE)  ";
                SQL += ComNum.VBLF + "    AND A.ACTDATE IS NULL ";

                if (JPL.Pano != "" && JPL.Pano != null) { SQL += ComNum.VBLF + "    AND A.PANO = '" + JPL.Pano + "' "; }
                if (JPL.Jeawon == "2")
                {   //재원자만
                    SQL += ComNum.VBLF + "     AND A.GBSTS = '0' ";
                }
                else if (JPL.Jeawon == "3")
                {   //퇴원자만
                    SQL += ComNum.VBLF + "     AND A.GBSTS != '0' ";
                }      
                if (JPL.AnOP) { SQL += ComNum.VBLF + "     AND B.OP_JIPYO ='Y' "; }     //수술예방적 항생제
                if (JPL.DRG) { SQL += ComNum.VBLF + "     AND A.GbDRG ='D' "; }         //DRG 환자만
                if (JPL.GbOP)
                {   //수술환자
                    SQL += ComNum.VBLF + "     AND A.Pano IN ( SELECT Pano ";
                    SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL += ComNum.VBLF + "                        WHERE OPDATE >=TO_DATE('" + JPL.OpFDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "                         AND  OPDATE <=TO_DATE('" + JPL.OpTDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "                         AND  IPDOPD ='I' ";
                    SQL += ComNum.VBLF + "                         AND  (OPCANCEL IS NULL OR OPCANCEL ='') ";
                    SQL += ComNum.VBLF + "                    ) ";
                }
                if (JPL.SetSuga)
                {   //수가셋팅
                    SQL += ComNum.VBLF + "     AND A.Pano IN ( SELECT Pano ";
                    SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                    SQL += ComNum.VBLF + "                        WHERE trsno = a.TRSNO  ";
                    SQL += ComNum.VBLF + "                         AND TRIM(SUNEXT) IN ( SELECT TRIM(SuCode) FROM ADMIN.ETC_JSIM_SUCHK WHERE USE ='Y' and SABUN  = '" + JPL.JobSabun + "' ) ";

                    SQL += ComNum.VBLF + "                    ) ";
                }

                if (JPL.Tewon)
                {
                    SQL += ComNum.VBLF + "     AND ADMIN.FC_NUR_MASTER_ROUTDATE(A.PANO, A.IPDNO) IS NOT NULL ";
                }

                if (JPL.MySet)
                {
                    SQL += ComNum.VBLF + "     AND ( ( A.JSIM_SET IS NULL ";
                    if (JPL.SetBi[0] != "") { SQL += ComNum.VBLF + "           AND A.BI IN (" + JPL.SetBi[0] + ") "; }
                    if (JPL.SetDept[0] != "") { SQL += ComNum.VBLF + "         AND A.DEPTCODE IN (" + JPL.SetDept[0] + ") "; }
                    if (JPL.SetWard[0] != "") { SQL += ComNum.VBLF + "         AND B.WARDCODE IN (" + JPL.SetWard[0] + ") "; }
                    if (JPL.SetRoom[0] != "") { SQL += ComNum.VBLF + "         AND B.ROOMCODE IN (" + JPL.SetRoom[0] + ") "; }

                    if (JPL.SetOK[1] != "")
                    { 
                        SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";
                        if (JPL.SetBi[1] != "") { SQL += ComNum.VBLF + "           AND A.BI IN (" + JPL.SetBi[1] + ") "; }
                        if (JPL.SetDept[1] != "") { SQL += ComNum.VBLF + "         AND A.DEPTCODE IN (" + JPL.SetDept[1] + ") "; }
                        if (JPL.SetWard[1] != "") { SQL += ComNum.VBLF + "         AND B.WARDCODE IN (" + JPL.SetWard[1] + ") "; }
                        if (JPL.SetRoom[1] != "") { SQL += ComNum.VBLF + "         AND B.ROOMCODE IN (" + JPL.SetRoom[1] + ") "; }
                    }
                    
                    if (JPL.SetOK[2] != "")
                    { 
                        SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";
                        if (JPL.SetBi[2] != "") { SQL += ComNum.VBLF + "           AND A.BI IN (" + JPL.SetBi[2] + ") "; }
                        if (JPL.SetDept[2] != "") { SQL += ComNum.VBLF + "         AND A.DEPTCODE IN (" + JPL.SetDept[2] + ") "; }
                        if (JPL.SetWard[2] != "") { SQL += ComNum.VBLF + "         AND B.WARDCODE IN (" + JPL.SetWard[2] + ") "; }
                        if (JPL.SetRoom[2] != "") { SQL += ComNum.VBLF + "         AND B.ROOMCODE IN (" + JPL.SetRoom[2] + ") "; }
                    }

                    if (JPL.SetOK[3] != "")
                    { 
                        SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";
                        if (JPL.SetBi[3] != "") { SQL += ComNum.VBLF + "           AND A.BI IN (" + JPL.SetBi[3] + ") "; }
                        if (JPL.SetDept[3] != "") { SQL += ComNum.VBLF + "         AND A.DEPTCODE IN (" + JPL.SetDept[3] + ") "; }
                        if (JPL.SetWard[3] != "") { SQL += ComNum.VBLF + "         AND B.WARDCODE IN (" + JPL.SetWard[3] + ") "; }
                        if (JPL.SetRoom[3] != "") { SQL += ComNum.VBLF + "         AND B.ROOMCODE IN (" + JPL.SetRoom[3] + ") "; }
                    }
                    
                    SQL += ComNum.VBLF + "           )  OR A.JSIM_SET = '" + JPL.JobSabun + "' ) ";
                }
                else
                {
                    if (JPL.Ward != "**")
                    {
                        SQL += ComNum.VBLF + " AND B.WARDCODE = '" + JPL.Ward + "'  ";
                    }

                    if (JPL.Dept != "**")
                    {
                        SQL += ComNum.VBLF + " AND A.DEPTCODE = '" + JPL.Dept + "'  ";
                    }
                }

                if (JPL.Sort.Equals("2"))
                {
                    SQL += ComNum.VBLF + " AND ( JSIM_LDATE < TRUNC(SYSDATE -1)  OR JSIM_LDATE IS NULL ) ";
                }
                else if (JPL.Sort.Equals("3"))
                {
                    SQL += ComNum.VBLF + " AND JSIM_LDATE = TRUNC(SYSDATE -1) ";
                }

                SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO ";
                SQL += ComNum.VBLF + "  ORDER BY B.ROOMCODE  ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_JSim_BasILLs(PsmhDb pDbCon, string strCode)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT IllCode IllCodeK,IllNameK,NOUSE,  TO_CHAR(DDATE,'YYYY-MM-DD') DDATE , ";
                SQL += ComNum.VBLF + "        TO_CHAR(NOUSEDATE,'YYYY-MM-DD') NOUSEDATE,IPDETC ,nvl(kcd8,'') kcd8   ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS                                        ";
                //SQL += ComNum.VBLF + "  WHERE IllCode = '" + strCode + "'                                           ";
                SQL += ComNum.VBLF + "  WHERE illcoded like  '" + strCode + "%'                                           ";
                SQL += ComNum.VBLF + "    AND ILLCLASS ='1'                                                         ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_JSim_BasILL_Like(PsmhDb pDbCon, string strKey)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ILLCODE, ILLNAMEK   ,nvl(kcd8,'') kcd8                                   ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS                        ";
                SQL += ComNum.VBLF + "  WHERE (ILLCODE  LIKE '%" + strKey + "%'                     ";
                SQL += ComNum.VBLF + "     OR UPPER(ILLNAMEK) LIKE '%" + strKey.ToUpper() + "%'     ";
                SQL += ComNum.VBLF + "     OR UPPER(ILLNAMEE) LIKE '%" + strKey.ToUpper() + "%' )   ";
                SQL += ComNum.VBLF + "    AND (NOUSE <>'N' OR NOUSE IS NULL)                        ";
                SQL += ComNum.VBLF + "    AND ILLCLASS ='1'                                         ";
                SQL += ComNum.VBLF + "    AND DDATE IS NULL                                         ";
                if (string.Compare(clsPublic.GstrSysDate, "2016-01-01") < 0)
                {
                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' ) ";
                }
                //else if (VB.Val(FstrOutDate) >= VB.Val("2016-01-01"))
                else if (string.Compare(clsPublic.GstrSysDate, "2016-01-01") >= 0)
                {
                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_JSim_BasILL2(PsmhDb pDbCon, string strKey)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ILLCODE, ILLNAMEK  ,nvl(kcd8,'') kcd8                         ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS                                ";
                //SQL += ComNum.VBLF + "  WHERE ILLCODE  LIKE '" + VB.Left(strKey, strKey.Length - 1) + "%'   ";
                SQL += ComNum.VBLF + "  WHERE ILLCODED  LIKE '" + strKey + "%'                               ";
              //  SQL += ComNum.VBLF + "    AND LENGTH(ILLCODE) <= 6                                          ";
                SQL += ComNum.VBLF + "    AND (NOUSE <>'N' OR NOUSE IS NULL)                                ";
                SQL += ComNum.VBLF + "    AND ILLCLASS ='1'                                                 ";
                SQL += ComNum.VBLF + "    AND DDATE IS NULL                                                 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        //사전심사 - 누적별 합산조회
        public DataTable sel_JSim_Screen_01(PsmhDb pDbCon, string strTrsNo, string strSDate, string strEDate, string strFnu, string strTnu)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sucode, I.Sunext, SunameK, Hcode, BaseAmt, Qty                                           ";
                SQL += ComNum.VBLF + "        ,GbSpc, DECODE(GbNgt,'0','',' ','',GbNgt) GbNgt                                          ";
                SQL += ComNum.VBLF + "        ,DECODE(GbGisul,'1','Y','0','',GbGisul) GbGisul                                          ";
                SQL += ComNum.VBLF + "        ,DECODE(GbSelf,'0','',' ','',GbSelf) GbSelf,GbChild,I.Nu                                 ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',I.Nu) NU2                        ";
                SQL += ComNum.VBLF + "        ,Bun,DECODE(B.SUGBP,'0','',B.SUGBP) SUGBP                                                ";
                SQL += ComNum.VBLF + "        ,DECODE(I.GBSUGBS,'0','',' ','',I.GBSUGBS) GBSUGBS                                       ";
                SQL += ComNum.VBLF + "        ,SUM(Nal) Nalsu, SUM(Amt1) Amtt1, SUM(Amt2) Amtt2                                        ";
                SQL += ComNum.VBLF + "        ,DECODE(I.GBER,'0','',' ','',I.GBER) GBER                                                ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', I.Sunext) Color  ";
                SQL += ComNum.VBLF + "       ,nvl(I.OPGUBUN,0) OPGUBUN                                                                                 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                                                    ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B                                                          ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + strTrsNo + "                                                                 ";
                if (strFnu != "" && strTnu.Equals("00"))
                {
                    SQL += ComNum.VBLF + "   AND NU = '" + strFnu + "'                                  ";
                }
                else if (strFnu != "" && strTnu != "00" && strTnu != "22")
                {
                    SQL += ComNum.VBLF + "    AND Nu >= '" + strFnu + "'                                ";
                    SQL += ComNum.VBLF + "    AND Nu <= '" + strTnu + "'                                ";
                }
                else if (strFnu.Equals("22"))
                {
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','4','5','6','7','8','9')                          ";
                }

                if (strSDate != "")
                {
                    SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')     ";
                }

                if (strEDate != "")
                {
                    SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEDate + "','yyyy-mm-dd')     ";
                }

                SQL += ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )          ";  //간호행위제외
                SQL += ComNum.VBLF + "    AND I.Sunext = B.Sunext                                       ";
                SQL += ComNum.VBLF + "    AND TRIM(i.SUNEXT) NOT IN (                                   ";
                SQL += ComNum.VBLF + "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE          ";
                SQL += ComNum.VBLF + "         WHERE GUBUN ='원무영수제외코드')                         ";  //저가약제 제외코드
                SQL += ComNum.VBLF + "  GROUP BY Sucode, I.Sunext, SunameK, Hcode, BaseAmt, Qty,        ";
                SQL += ComNum.VBLF + "           GbSpc, DECODE(GbNgt,'0','',' ','',GbNgt),              ";
                SQL += ComNum.VBLF + "           DECODE(GbGisul, '1', 'Y', '0', '', GbGisul),           ";
                SQL += ComNum.VBLF + "           DECODE(GbSelf,'0','',' ','',GbSelf), GbChild,I.Nu,     ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',I.Nu),   ";
                SQL += ComNum.VBLF + "           Bun,DECODE(B.SUGBP,'0','',B.SUGBP),                    ";
                SQL += ComNum.VBLF + "           DECODE(I.GBSUGBS,'0','',' ','',I.GBSUGBS),             ";
                SQL += ComNum.VBLF + "           DECODE(I.GBER,'0','',' ','',I.GBER),                   ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', I.Sunext), ";
                SQL += ComNum.VBLF + "           nvl(I.OPGUBUN,0)                                              ";
                SQL += ComNum.VBLF + "  ORDER BY I.Nu, Bun, Sucode , SUNEXT                             ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        //사전심사 - 항목별 상세조회
        public DataTable sel_JSim_Screen_02(PsmhDb pDbCon, string strTrsNo, string strSDate, string strEDate, string strFnu, string strTnu)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                //조회분류별 컬럼명이 변경되므로 BDATE => SUCODE, SUCODE => HCODE Alias 강제 지정함.
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'YY-MM-DD') SUCODE                                                        ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(ACTDATE,'YY-MM-DD') ACTDATE                                                      ";
                SQL += ComNum.VBLF + "       ,I.Nu,SUCODE HCODE,SUNAMEK,BaseAmt,Qty,Nal Nalsu,GbSpc                                    ";
                SQL += ComNum.VBLF + "       ,GbChild,Amt1 Amtt1,Amt2 Amtt2,Part,I.Sunext                                              ";
                SQL += ComNum.VBLF + "       ,DECODE(I.GBSUGBS,'0','',' ','',I.GBSUGBS) GBSUGBS                                        ";
                SQL += ComNum.VBLF + "       ,DECODE(B.SUGBP,'0','',B.SUGBP) SUGBP                                                     ";
                SQL += ComNum.VBLF + "       ,DECODE(I.GBER,'0','',' ','',I.GBER) GBER                                                 ";
                SQL += ComNum.VBLF + "       ,DECODE(GbNgt,'0','',' ','',GbNgt) GBNGT                                                  ";
                SQL += ComNum.VBLF + "       ,DECODE(GbGisul, '1', 'Y', '0', '', GbGisul) GBGISUL                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(GbSelf,'0','',' ','',GbSelf) GBSELF                                               ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',I.Nu) NU2                        ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', I.Sunext) Color  ";
                SQL += ComNum.VBLF + "       ,nvl(I.OPGUBUN,0) OPGUBUN                                                                                ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                                                    ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B                                                          ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + strTrsNo + "                                                                 ";
                if (strFnu != "" && strTnu == "00")
                {
                    SQL += ComNum.VBLF + "   AND NU = '" + strFnu + "'                                          ";
                }
                else if (strFnu != "" && strTnu != "00" && strTnu != "22")
                {
                    SQL += ComNum.VBLF + "    AND NU >= '" + strFnu + "'                                        ";
                    SQL += ComNum.VBLF + "    AND NU <= '" + strTnu + "'                                        ";
                }
                else if (strFnu == "22")
                {
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','4','5','6','7','8','9')                                  ";
                }
                    
                if (strSDate != "")
                {
                    SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')             ";
                }

                if (strEDate != "")
                {
                    SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEDate + "','yyyy-mm-dd')             ";
                }
                
                SQL += ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )                  ";  //간호행위제외
                SQL += ComNum.VBLF + "    AND I.Sunext = B.Sunext                                               ";
                SQL += ComNum.VBLF + "    AND TRIM(i.SUNEXT) NOT IN (                                           ";
                SQL += ComNum.VBLF + "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE                  ";
                SQL += ComNum.VBLF + "         WHERE GUBUN ='원무영수제외코드')                                 ";  //저가약제 제외코드
                SQL += ComNum.VBLF + "  ORDER BY Nu, Sucode, I.Sunext, Bdate                                    ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        //사전심사 - 일자별 상세조회
        public DataTable sel_JSim_Screen_03(PsmhDb pDbCon, string strTrsNo, string strSDate, string strEDate, string strFnu, string strTnu)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                //조회분류별 컬럼명이 변경되므로 BDATE => SUCODE, SUCODE => HCODE Alias 강제 지정함.
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(Bdate, 'YY-MM-DD') SUCODE                                                        ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(ACTDATE,'YY-MM-DD') ACTDATE                                                      ";
                SQL += ComNum.VBLF + "       ,I.Nu,SUCODE HCODE,SUNAMEK,BaseAmt,Qty,Nal Nalsu,GbSpc                                    ";
                SQL += ComNum.VBLF + "       ,GbChild,Amt1 Amtt1,Amt2 Amtt2,Part,I.Sunext                                              ";
                SQL += ComNum.VBLF + "       ,DECODE(I.GBSUGBS,'0','',' ','',I.GBSUGBS) GBSUGBS                                        ";
                SQL += ComNum.VBLF + "       ,DECODE(B.SUGBP,'0','',B.SUGBP) SUGBP                                                     ";
                SQL += ComNum.VBLF + "       ,DECODE(I.GBER,'0','',' ','',I.GBER) GBER                                                 ";
                SQL += ComNum.VBLF + "       ,DECODE(GbNgt,'0','',' ','',GbNgt) GBNGT                                                  ";
                SQL += ComNum.VBLF + "       ,DECODE(GbGisul, '1', 'Y', '0', '', GbGisul) GBGISUL                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(GbSelf,'0','',' ','',GbSelf) GBSELF,I.ROWID                                       ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',I.Nu) NU2                        ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', I.Sunext) Color  ";
                SQL += ComNum.VBLF + "       ,nvl(I.OPGUBUN,0) OPGUBUN                                                                                ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                                                    ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B                                                          ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + strTrsNo + "                                                                 ";
                if (strFnu != "" && strTnu == "00")
                {
                    SQL += ComNum.VBLF + "   AND NU = '" + strFnu + "'                                          ";
                }
                else if (strFnu != "" && strTnu != "00" && strTnu != "22")
                {
                    SQL += ComNum.VBLF + "    AND NU >= '" + strFnu + "'                                        ";
                    SQL += ComNum.VBLF + "    AND NU <= '" + strTnu + "'                                        ";
                }
                else if (strFnu == "22")
                {
                    SQL += ComNum.VBLF + "    AND GBSUGBS IN ('3','4','5','6','7','8','9')                                  ";
                }

                if (strSDate != "")
                {
                    SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')             ";
                }

                if (strEDate != "")
                {
                    SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEDate + "','yyyy-mm-dd')             ";
                }

                SQL += ComNum.VBLF + "    AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )                  ";  //간호행위제외
                SQL += ComNum.VBLF + "    AND I.Sunext = B.Sunext                                               ";
                SQL += ComNum.VBLF + "    AND TRIM(i.SUNEXT) NOT IN (                                           ";
                SQL += ComNum.VBLF + "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE                  ";
                SQL += ComNum.VBLF + "         WHERE GUBUN ='원무영수제외코드')                                 ";  //저가약제 제외코드
                SQL += ComNum.VBLF + "  ORDER BY Nu, Bdate, Sucode, I.Sunext                                    ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        //사전심사 - 누적별 합산조회(1회투여량계산)
        public DataTable sel_JSim_Screen_04(PsmhDb pDbCon, string strTrsNo, string strSDate, string strEDate, string strFnu, string strTnu)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SUCODE, I.SUNEXT, SUNAMEK, HCODE, BASEAMT, Qty, GbSpc,            ";
                SQL += ComNum.VBLF + "        DECODE(GbNgt,'0','',' ','',GbNgt) GbNgt,                          ";
                SQL += ComNum.VBLF + "        DECODE(GbGisul, '1', 'Y', '0', '', GbGisul) GbGisul,              ";
                SQL += ComNum.VBLF + "        DECODE(GbSelf,'0','',' ','',GbSelf) GbSelf, GbChild, Nu, Bun,     ";
                SQL += ComNum.VBLF + "        DECODE(GBSUGBS,'0','',' ','',GBSUGBS) GBSUGBS, B.SUGBP,           ";
                SQL += ComNum.VBLF + "        DECODE(I.GBER,'0','',' ','',I.GBER) GBER,                         ";
                SQL += ComNum.VBLF + "        SUM(Nal) Nalsu, SUM(Amt1) Amtt1, SUM(Amt2) Amtt2,                 ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',I.Nu) NU2, ";
                SQL += ComNum.VBLF + "        nvl(OPGUBUN,0) OPGUBUN                                                         ";
                SQL += ComNum.VBLF + "   FROM                                                                   ";
                SQL += ComNum.VBLF + "      ( SELECT BDATE, SUCODE, SUNEXT,  BASEAMT, SUM(QTY * NAL) QTY ,      ";
                SQL += ComNum.VBLF + "               GBSPC, GBNGT,  GBGISUL, GBSELF,  GBCHILD,  GBER,           ";
                SQL += ComNum.VBLF + "               NU,    BUN,    GBSUGBS, 1 NAL,   SUM(AMT1) AMT1,           ";
                SQL += ComNum.VBLF + "               SUM(AMT2) AMT2,nvl(OPGUBUN,0) OPGUBUN                                     ";
                SQL += ComNum.VBLF + "          FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                         ";
                SQL += ComNum.VBLF + "         WHERE TRSNO = " + strTrsNo + "                                   ";
                if (strFnu != "" && strTnu == "00")
                {
                    SQL += ComNum.VBLF + "       AND NU = '" + strFnu + "'                                          ";
                }
                else if (strFnu != "" && strTnu != "00" && strTnu != "22")
                {
                    SQL += ComNum.VBLF + "       AND NU >= '" + strFnu + "'                                        ";
                    SQL += ComNum.VBLF + "       AND NU <= '" + strTnu + "'                                        ";
                }
                else if (strFnu == "22")
                {
                    SQL += ComNum.VBLF + "       AND GBSUGBS IN ('3','4','5','6','7','8','9')                                  ";
                }

                if (strSDate != "") { SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   "; }
                if (strEDate != "") { SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEDate + "','yyyy-mm-dd')   "; }

                SQL += ComNum.VBLF + "           AND NAL IN ('1', '-1')  ";
                SQL += ComNum.VBLF + "           AND TRIM(SUNEXT) NOT IN ( ";
                SQL += ComNum.VBLF + "               SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN ='원무영수제외코드' ) ";  //저가약제 제외코드
                SQL += ComNum.VBLF + "      GROUP BY BDATE, SUCODE, SUNEXT, BASEAMT,  ";
                SQL += ComNum.VBLF + "               GBSPC, GBNGT,  GBGISUL, GBSELF, GBCHILD, GBER, NU, BUN, GBSUGBS,nvl(OPGUBUN,0)  ";

                SQL += ComNum.VBLF + " UNION ALL ";

                SQL += ComNum.VBLF + "        SELECT BDATE, SUCODE, SUNEXT,  BASEAMT, QTY,              ";
                SQL += ComNum.VBLF + "               GBSPC, GBNGT,  GBGISUL, GBSELF,  GBCHILD,  GBER,   ";
                SQL += ComNum.VBLF + "               NU,    BUN,    GBSUGBS, NAL,     AMT1,     AMT2    ";
                SQL += ComNum.VBLF + "               nvl(OPGUBUN,0) OPGUBUN                                            ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                        ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + strTrsNo + "                                  ";
                if (strFnu != "" && strTnu == "00")
                {
                    SQL += ComNum.VBLF + "       AND NU = '" + strFnu + "'                              ";
                }
                else if (strFnu != "" && strTnu != "00" && strTnu != "22")
                {
                    SQL += ComNum.VBLF + "       AND NU >= '" + strFnu + "'                             ";
                    SQL += ComNum.VBLF + "       AND NU <= '" + strTnu + "'                             ";
                }
                else if (strFnu == "22")
                {
                    SQL += ComNum.VBLF + "       AND GBSUGBS IN ('3','4','5','6','7','8','9')                       ";
                }
                if (strSDate != "") { SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   "; }
                if (strEDate != "") { SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEDate + "','yyyy-mm-dd')   "; }
                SQL += ComNum.VBLF + "   AND NAL NOT IN ('1' ,'-1') ";
                SQL += ComNum.VBLF + " ) I,  ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B                               ";
                SQL += ComNum.VBLF + "  WHERE (B.WONCODE NOT IN('1118') OR B.WONCODE IS NULL )              "; //간호행위제외
                SQL += ComNum.VBLF + "    AND I.Sunext = B.Sunext                                           ";
                SQL += ComNum.VBLF + "    AND TRIM(I.SUNEXT) NOT IN (                                       ";
                SQL += ComNum.VBLF + "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE              ";
                SQL += ComNum.VBLF + "         WHERE GUBUN ='원무영수제외코드')                             "; //저가약제 제외코드 
                SQL += ComNum.VBLF + "    AND I.NU <> '06'                                                  ";
                SQL += ComNum.VBLF + "  GROUP BY SUCODE, I.Sunext, B.SunameK, B.Hcode, I.BaseAmt, I.Qty,    ";
                SQL += ComNum.VBLF + "           GbSpc, GbNgt, GbGisul, GbSelf, GbChild, I.Nu, I.Bun,       ";
                SQL += ComNum.VBLF + "           I.GBSUGBS, B.SUGBP,                                        ";
                SQL += ComNum.VBLF + "           DECODE(I.GBER, '0', '', ' ', '', I.GBER),                  ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',I.Nu), ";
                SQL += ComNum.VBLF + "           nvl(I.OPGUBUN,0) OPGUBUN                                                  ";

                SQL += ComNum.VBLF + " UNION ALL                                                            ";
                //마취분 UNION
                SQL += ComNum.VBLF + " SELECT Sucode, I.Sunext, SunameK, HCODE, BASEAMT, Qty, GbSpc,        ";
                SQL += ComNum.VBLF + "        DECODE(GbNgt,'0','',' ','',GbNgt) GbNgt,                      ";
                SQL += ComNum.VBLF + "        DECODE(GbGisul, '1', 'Y', '0', '', GbGisul) GbGisul,          ";
                SQL += ComNum.VBLF + "        DECODE(GbSelf,'0','',' ','',GbSelf) GbSelf, GbChild, Nu, BUN, ";
                SQL += ComNum.VBLF + "        DECODE(GBSUGBS,'0','',' ','',GBSUGBS) GBSUGBS, B.SUGBP,       ";
                SQL += ComNum.VBLF + "        DECODE(I.GBER,'0','',' ','',I.GBER) GBER,                     ";
                SQL += ComNum.VBLF + "        Nal Nalsu, Amt1 Amtt1, Amt2 Amtt2,                            ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_MED + "FC_BAS_BCODE_NAME('BAS_누적행위명',Nu) NU2, ";
                SQL += ComNum.VBLF + "       nvl(I.OPGUBUN,0) OPGUBUN                                                     ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                         ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B                               ";
                SQL += ComNum.VBLF + "  WHERE I.TRSNO = " + strTrsNo + "                                    ";
                SQL += ComNum.VBLF + "    AND Nu = '06'                                                     ";
                SQL += ComNum.VBLF + "    AND I.Sunext = B.Sunext                                           ";

                if (strSDate != "") { SQL += ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strSDate + "','yyyy-mm-dd')   "; }
                if (strEDate != "") { SQL += ComNum.VBLF + "    AND Bdate <= TO_DATE('" + strEDate + "','yyyy-mm-dd')   "; }
                
                SQL += ComNum.VBLF + "  ORDER BY  Nu, SUCODE , SUNEXT                                     ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        //심사완료 - 정상분만 수가체크
        public void Chk_JSim_OGSuga(PsmhDb pDbCon, string ArgPano, long ArgTrsNo)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID                                               ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                  ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                            ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTrsNo + "                            ";
                SQL += ComNum.VBLF + "    AND SUCODE IN (                                         ";
                SQL += ComNum.VBLF + "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE    ";
                SQL += ComNum.VBLF + "         WHERE GUBUN = 'JSIM_정상분만수가'                  ";
                SQL += ComNum.VBLF + "           AND (DELDATE IS NULL OR DELDATE = ''))           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("정상분만관련 수가코드가 있습니다. 심사완료시 확인하세요!", "확인");
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }
        }

        //심사완료 - 신생아관련 수가체크
        public void Chk_JSim_PDSuga(PsmhDb pDbCon, string ArgPano, long ArgTrsNo)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID                                               ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                  ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                            ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTrsNo + "                            ";
                SQL += ComNum.VBLF + "    AND SUCODE IN (                                         ";
                SQL += ComNum.VBLF + "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE    ";
                SQL += ComNum.VBLF + "         WHERE GUBUN = 'JSIM_신생아수가'                    ";
                SQL += ComNum.VBLF + "           AND (DELDATE IS NULL OR DELDATE = ''))           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("신생아관련 수가코드가 있습니다. 심사완료시 확인하세요!", "확인");
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }
        }

        //인정비급여 관련 메시지 팝업
        public void Chk_JSim_Bigub_SugaSP(PsmhDb pDbCon, string ArgPano, long ArgTrsNo)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT i.Sucode,i.Qty,i.GbSelf ,i.Bun,i.Nu,                      ";
                SQL += ComNum.VBLF + "        SUM(i.Amt1 +i.Amt2) AMT ,B.SUGBS ,B.SUGBP                 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,                     ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN B                           ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                                  ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTrsNo + "                                  ";
                SQL += ComNum.VBLF + "    AND I.Sunext = B.Sunext                                       ";
                SQL += ComNum.VBLF + "    AND nu >= '21'                                                ";
                SQL += ComNum.VBLF + "    AND B.SUGBS <> '1'                                            ";
                SQL += ComNum.VBLF + "    AND B.SUGBP <> '1'                                            ";
                SQL += ComNum.VBLF + "  GROUP BY  i.Sucode,i.Qty,i.GbSelf ,i.Bun,i.Nu ,B.SUGBS ,B.SUGBP ";
                SQL += ComNum.VBLF + " HAVING SUM(i.Amt1 + i.Amt2) > 0                                  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("임의비급여 확인바랍니다!", "확인");
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }
        }

        //F014 관련 청구상병 체크
        public bool Read_Mir_ILLS(PsmhDb pDbCon, string ArgPano, string ArgBi, string ArgInDate, long ArgIpdNo, long ArgTrsNo, string ArgFCode)
        {
            bool rtnVal = true;

            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT RowId, IllCode, IllName, RANK, REMARK, GBILL            \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "MIR_ILLS                          \r\n";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                \r\n";
                SQL += "    AND Bi = '" + ArgBi + "'                                    \r\n";
                SQL += "    AND IpdOpd = 'I'                                            \r\n";
                SQL += "    AND InDate  = TO_DATE('" + ArgInDate + "','YYYY-MM-DD')     \r\n";
                SQL += "    AND (IPDNO IS NULL OR IPDNO = " + ArgIpdNo + ")             \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                                \r\n";
                SQL += "    AND ILLCODE IS NOT NULL                                     \r\n";
                SQL += "  ORDER BY Rank,IllCode                                             ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("청구상병이 없습니다!", "상병확인 요망");
                    return false;
                }
                else
                {
                    //제 1상병만 체크
                    if (READ_BAS_ILLS_IPDETC(pDbCon, dt.Rows[0]["IllCode"].ToString().Trim()) == true)
                    { 
                        if (ArgFCode != "F014")
                        {
                            clsPublic.GstrMsgList = "F014 대상 상병입니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList += "그래도 심사완료 하시겠습니까?";
                            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "장기입원환자 심사체크", MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    { 
                        if (ArgFCode == "F014")
                        {
                            clsPublic.GstrMsgList = "F014 대상 상병아닙니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList += "그래도 심사완료 하시겠습니까?";
                            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "장기입원환자 심사체크", MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return false;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        public bool READ_BAS_ILLS_IPDETC(PsmhDb pDbCon, string ArgILLCode)
        {
            bool rtnVal = false;

            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT IPDETC FROM " + ComNum.DB_PMPA + "BAS_ILLS  ";
                SQL += "  WHERE IllCode = '" + ArgILLCode + "'              ";
                SQL += "    AND ILLCLASS = '1'                              ";
                SQL += "    AND IPDETC = 'Y'                                ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        public bool Return_Process_HU(PsmhDb pDbCon, long ArgTRSNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int intRowAffected = 0;

            int i = 0, nREAD = 0;
            int nRetNal = 0; double nRetQty = 0.0;
            int nIlsu = 0;
            long nRetAmt1 = 0, nRetAmt2 = 0;
            string strRowid = string.Empty;
            string strRetNu = string.Empty;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            bool rtnVal = true;

            //--------------------------------------------------------------------
            //  TRSNO별도 IPD_NEW_SLIP을 읽어 취소처방을    INSERT
            //                            새로운 자격에 INSERT
            //--------------------------------------------------------------------

            try
            {
                SQL = "";
                SQL += " SELECT IPDNO,TrsNo,TO_CHAR(BDATE,'YYYY-MM-DD') BDate,PANO,BI,SuNext,                   \r\n";
                SQL += "        BUN,NU,QTY,BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,                                  \r\n";
                SQL += "        GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,                                 \r\n";
                SQL += "        GBHOST,SEQNO,YYMM, DRGSELF, ORDERNO,                                            \r\n";
                SQL += "        NAL,Amt1,Amt2,                                                                  \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,                                \r\n";
                SQL += "        EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD , POWDER, BCODE,         \r\n"; //2012-11-15 part 추가
                SQL += "        GBSUGBAB, GBSUGBAC, GBSUGBAD, OPGUBUN, HIGHRISK, GBNGT2, asadd                  \r\n ";   //2021-08-05 재원심사 GROUP BY 에서 누락된 컬럼 추가
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a                                            \r\n";
                SQL += "  WHERE TrsNo = " + ArgTRSNO + "   AND trim(PART) not in ('!+')                         \r\n";
                SQL += ComNum.VBLF + "    AND (a.SUNEXT, a.BDATE) NOT IN (                                                          ";
                SQL += ComNum.VBLF + "          SELECT SUNEXT, BDATE                                                                ";
                SQL += ComNum.VBLF + "            From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                           ";
                SQL += ComNum.VBLF + "           WHERE TRSNO =  " + ArgTRSNO + "                                                    ";
                SQL += ComNum.VBLF + "           and  trim(PART) not in ('!+')                                                      ";
                SQL += ComNum.VBLF + "           GROUP By SUNEXT, BDATE                                                             ";
                SQL += ComNum.VBLF + "          HAVING SUM( AMT1 ) = 0      )                                                      ";
               // SQL += "  GROUP BY IPDNO,TrsNo,Pano,Bi,BDate,SuNext,Bun,Nu,Qty,                         \r\n";
               // SQL += "           BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,                 \r\n";
               // SQL += "           DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,SEQNO,YYMM,DRGSELF,ORDERNO,     \r\n";
               // SQL += "           ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,          \r\n";
               // SQL += "           RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD , BCODE      \r\n";
                SQL += "  ORDER BY Bdate, Bun                                                           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        nRetNal = Convert.ToInt32(VB.Val(dt.Rows[i]["NAL"].ToString()));
                        nRetAmt1 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT1"].ToString()));
                        nRetAmt2 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT2"].ToString()));
                        strRetNu = dt.Rows[i]["Nu"].ToString().Trim();

                        if (nRetNal != 0 || nRetAmt1 != 0 || nRetAmt2 != 0)
                        {
                            nRetNal = nRetNal * -1;
                            nRetAmt1 = nRetAmt1 * -1;
                            nRetAmt2 = nRetAmt2 * -1;

                            //----------------------------------------
                            //       보정 SLIP을 INSERT
                            //----------------------------------------
                            SQL = "";
                            SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                         ";
                            SQL += "        (IPDNO,TrsNo,ActDate, Pano, Bi, Bdate, EntDate, Sunext, Bun,                                    ";
                            SQL += "        Nu, Qty, Nal,  BaseAmt, GbSpc, GbNgt, GbGisul, GbSelf,                                          ";
                            SQL += "        GbChild, DeptCode, DrCode, WardCode, Sucode, GbSlip,                                            ";
                            SQL += "        GbHost, Part, Amt1, Amt2, SeqNo, yymm, DRGSELF, ORDERNO,                                        ";
                            SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBSGADD , BCODE,POWDER,GBER,  ";
                            SQL += "        GBSUGBAB, GBSUGBAC, GBSUGBAD, OPGUBUN, HIGHRISK, GBNGT2, asadd) ";   //2021-08-05 재원심사 GROUP BY 에서 누락된 컬럼 추가
                            SQL += " VALUES (                                                                                               ";
                            SQL += "        " + VB.Val(dt.Rows[i]["IPDNO"].ToString()) + ",                                                 ";
                            SQL += "        " + VB.Val(dt.Rows[i]["TrsNo"].ToString()) + ",                                                 ";
                            SQL += "         TRUNC(SYSDATE),'" + clsPmpaType.TIT.Pano + "',           ";
                            SQL += "        '" + dt.Rows[i]["Bi"].ToString() + "',                                                          ";
                            SQL += "        TO_DATE('" + dt.Rows[i]["Bdate"].ToString() + "','YYYY-MM-DD'),                                 ";
                            SQL += "        SYSDATE,                                                                                        ";
                            SQL += "        '" + dt.Rows[i]["Sunext"].ToString() + "',  '" + dt.Rows[i]["Bun"].ToString() + "',             ";
                            SQL += "        '" + dt.Rows[i]["Nu"].ToString() + "',                                                          ";
                            SQL += "         " + VB.Val(dt.Rows[i]["Qty"].ToString()) + ",     '" + nRetNal + "',                           ";
                            SQL += "         " + VB.Val(dt.Rows[i]["BaseAmt"].ToString()) + ", '" + dt.Rows[i]["GbSpc"].ToString() + "',    ";
                            SQL += "        '" + dt.Rows[i]["GbNgt"].ToString() + "',   '" + dt.Rows[i]["GbGisul"].ToString() + "',         ";
                            SQL += "        '" + dt.Rows[i]["GbSelf"].ToString() + "',  '" + dt.Rows[i]["GbChild"].ToString() + "',         ";
                            SQL += "        '" + dt.Rows[i]["DeptCode"].ToString() + "','" + dt.Rows[i]["DrCode"].ToString() + "',          ";
                            SQL += "        '" + dt.Rows[i]["WardCode"].ToString() + "','" + dt.Rows[i]["Sucode"].ToString() + "',          ";
                            SQL += "        '" + dt.Rows[i]["GbSlip"].ToString() + "', '" + dt.Rows[i]["GbHost"].ToString() + "', '!-',     ";
                            SQL += "         " + nRetAmt1 + ", " + nRetAmt2 + ", '" + VB.Val(dt.Rows[i]["SeqNo"].ToString()) + "',          ";
                            SQL += "        '" + dt.Rows[i]["YYMM"].ToString() + "' ,'" + dt.Rows[i]["DRGSELF"].ToString() + "',            ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["ORDERNO"].ToString()) + "',                                             ";
                            SQL += "        '" + dt.Rows[i]["ABCDATE"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["OPER_DEPT"].ToString() + "',                                                   ";
                            SQL += "        '" + dt.Rows[i]["OPER_DCT"].ToString() + "',                                                    ";
                            SQL += "        '" + dt.Rows[i]["ORDER_DEPT"].ToString() + "',                                                  ";
                            SQL += "        '" + dt.Rows[i]["ORDER_DCT"].ToString() + "',                                                   ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["EXAM_WRTNO"].ToString()) + "',                                          ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["RoomCode"].ToString()) + "',                                            ";
                            SQL += "         " + VB.Val(dt.Rows[i]["DIV"].ToString()) + ",                                                  ";
                            SQL += "        '" + dt.Rows[i]["GBSELNOT"].ToString().Trim() + "',                                             ";
                            SQL += "        '" + dt.Rows[i]["GBSUGBS"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["GBSGADD"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["BCODE"].ToString() + "',                                                       ";
                            SQL += "        '" + dt.Rows[i]["POWDER"].ToString() + "',                                                      ";
                            SQL += "        '" + dt.Rows[i]["GBER"].ToString().Trim() + "',                                                 ";
                            SQL += "        '" + dt.Rows[i]["GBSUGBAB"].ToString().Trim() + "',                                             "; //2021-08-05 추가
                            SQL += "        '" + dt.Rows[i]["GBSUGBAC"].ToString().Trim() + "',                                             "; //2021-08-05 추가
                            SQL += "        '" + dt.Rows[i]["GBSUGBAD"].ToString().Trim() + "',                                             "; //2021-08-05 추가
                            SQL += "        '" + dt.Rows[i]["OPGUBUN"].ToString().Trim() + "',                                              "; //2021-08-05 추가
                            SQL += "        '" + dt.Rows[i]["HIGHRISK"].ToString().Trim() + "',                                             "; //2021-08-05 추가
                            SQL += "        '" + dt.Rows[i]["GBNGT2"].ToString().Trim() + "',                                               "; //2021-08-05 추가
                            SQL += "        '" + dt.Rows[i]["asadd"].ToString().Trim() + "'                                                 "; //2021-08-05 추가
                            SQL += "          )                                                                                             ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                return false;
                            }

                        }
                    }
                }

                dt.Dispose();
                dt = null;



                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
        public bool Return_Process_HU_DEL(PsmhDb pDbCon, long ArgTRSNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int intRowAffected = 0;

            int i = 0, nREAD = 0;
            int nRetNal = 0;
            int nIlsu = 0;
            long nRetAmt1 = 0, nRetAmt2 = 0;
            string strRowid = string.Empty;
            string strRetNu = string.Empty;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            bool rtnVal = true;

            //--------------------------------------------------------------------
            //  당일 작업된 호스피스 데이터 삭제
            //--------------------------------------------------------------------

            try
            {
                SQL = "";
                SQL += " SELECT IPDNO,TrsNo,TO_CHAR(BDATE,'YYYY-MM-DD') BDate,PANO,BI,SuNext           \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                      \r\n";
                SQL += "  WHERE TrsNo = " + ArgTRSNO + "                                                \r\n";
                SQL += "    AND trim(PART) in ('!-','!+')  AND actdate=trunc(sysdate)                   \r\n";
                SQL += "  ORDER BY Bdate, Bun                                                           ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {


                    //----------------------------------------
                    //       보정 SLIP을 DEL
                    //----------------------------------------
                    SQL = "";
                    SQL += " DELETE  FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP    \r";
                    SQL += "  WHERE TrsNo = " + ArgTRSNO + "                     \r";
                    SQL += "     AND trim(PART) in ('!-','!+')  AND actdate=trunc(sysdate)       \r";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);




                }

                dt.Dispose();
                dt = null;



                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }
              
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
        public bool Return_Process_HU_UPDATE(PsmhDb pDbCon, long ArgTRSNO)  
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int intRowAffected = 0;

            int i = 0, nREAD = 0;
            int nRetNal = 0;
            int nIlsu = 0;
            long nRetAmt1 = 0, nRetAmt2 = 0;
            string strRowid = string.Empty;
            string strRetNu = string.Empty;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            bool rtnVal = true;

            //--------------------------------------------------------------------
            //  당일 작업된 호스피스 데이터 삭제
            //--------------------------------------------------------------------

            try
            {
                


                    //----------------------------------------
                    //       보정 
                    //----------------------------------------
                    SQL = "";
                    SQL += " UPDATE  " + ComNum.DB_PMPA + "IPD_TRANS            \r";
                    SQL += " SET  GBHU = 'Y'                                    \r";
                    SQL += "  WHERE TrsNo = " + ArgTRSNO + "                     \r";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

        

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
        public bool Create_Process_HU(PsmhDb pDbCon, long ArgTRSNO)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int intRowAffected = 0;

            int i = 0, nREAD = 0;
            int nRetNal = 0;
            double nRetQty = 0.0;
            int nBI = 0;
            long nRetAmt1 = 0, nRetAmt2 = 0;
            string strRowid = string.Empty;
            string strRetNu = string.Empty;
            string strInDate = string.Empty;
            string strOutDate = string.Empty;
            bool rtnVal = true;
            clsPmpaQuery cPQ = new clsPmpaQuery();
            //--------------------------------------------------------------------
            //  TRSNO별도 IPD_NEW_SLIP을 읽어 취소처방을    INSERT
            //                            새로운 자격에 INSERT
            //--------------------------------------------------------------------

            try
            {
                SQL = "";
                SQL += " SELECT IPDNO,TrsNo,TO_CHAR(BDATE,'YYYY-MM-DD') BDate,PANO,BI,trim(SuNext) SuNext,              \r\n";
                SQL += "        BUN,NU,QTY,BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,                                          \r\n";
                SQL += "        GBCHILD,DEPTCODE,DRCODE,WARDCODE,trim(SUCODE) SUCODE,GBSLIP,                            \r\n";
                SQL += "        GBHOST,SEQNO,YYMM, DRGSELF, ORDERNO,                                                    \r\n";
                SQL += "        Nal,Amt1, Amt2,                                                                         \r\n";
                SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,                                        \r\n";
                SQL += "        EXAM_WRTNO,RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD ,POWDER, BCODE,                  \r\n"; //2012-11-15 part 추가
                SQL += "        GBSUGBAB, GBSUGBAC, GBSUGBAD, OPGUBUN, HIGHRISK, GBNGT2, asadd                          \r\n ";   //2021-08-05 재원심사 GROUP BY 에서 누락된 컬럼 추가
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                      \r\n";
                SQL += "  WHERE TrsNo = " + ArgTRSNO + "                                                                \r\n";
                SQL += "  AND Part  = '!-' and actdate=trunc(sysdate)                                                   \r\n";
              //  SQL += "  GROUP BY IPDNO,TrsNo,Pano,Bi,BDate,SuNext,Bun,Nu,Qty,                                       \r\n";
              //  SQL += "           BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,                               \r\n";
              //  SQL += "           DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,SEQNO,YYMM,DRGSELF,ORDERNO,                   \r\n";
              //  SQL += "           ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,                        \r\n";
              //  SQL += "           RoomCode,DIV,GBSELNOT,GBSUGBS,GBER ,GBSGADD , BCODE                                \r\n";
                SQL += "  ORDER BY Bdate, Bun                                                                               ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        nRetNal = Convert.ToInt32(VB.Val(dt.Rows[i]["NAL"].ToString()));
                        nRetQty = Convert.ToDouble(VB.Val(dt.Rows[i]["QTY"].ToString()));
                        nRetAmt1 = Convert.ToInt64(VB.Val(dt.Rows[i] ["AMT1"].ToString()));
                        nRetAmt2 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT2"].ToString()));
                        strRetNu = dt.Rows[i]["Nu"].ToString().Trim();
                        if (nRetNal != 0 || nRetAmt1 != 0 || nRetAmt2 != 0)
                        {
                            nRetNal = nRetNal * -1;
                            nRetAmt1 = 0;
                            nRetAmt2 = 0;

                            cPQ.Read_Suga_Amt_HU(pDbCon, dt.Rows[i]["SUCODE"].ToString(), dt.Rows[i]["SuNext"].ToString(), dt.Rows[i]["BDate"].ToString());

                            if (clsPmpaPb.GstrSugaNewReadOK == "OK")
                            {
                                nRetAmt1 = clsPmpaPb.GnBAmt;
                                nRetAmt2 = clsPmpaPb.GnBAmt;

                                if (nRetAmt1 == 0)
                                {
                                    nRetAmt1 = Convert.ToInt64(VB.Val(dt.Rows[i]["BASEAMT"].ToString()));
                                    nRetAmt2 = Convert.ToInt64(VB.Val(dt.Rows[i]["AMT1"].ToString()) * -1);
                                }
                                                   //병실차액료
                                if (dt.Rows[i]["GbGisul"].ToString() == "1")
                                {
                                    nRetAmt2 = (long)Math.Round( nRetAmt1 * clsPmpaPb.GISUL[Convert.ToInt16(VB.Left(clsPmpaType.TIT.Bi, 1))] / 100.0, 0, MidpointRounding.AwayFromZero);
                                }
                                nRetAmt2 = (long)Math.Truncate(nRetAmt2 * nRetNal * nRetQty);
                            }

                            //----------------------------------------
                            //       보정 SLIP을 INSERT
                            //----------------------------------------
                            SQL = "";
                            SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                         ";
                            SQL += "        (IPDNO,TrsNo,ActDate, Pano, Bi, Bdate, EntDate, Sunext, Bun,                                    ";
                            SQL += "        Nu, Qty, Nal,  BaseAmt, GbSpc, GbNgt, GbGisul, GbSelf,                                          ";
                            SQL += "        GbChild, DeptCode, DrCode, WardCode, Sucode, GbSlip,                                            ";
                            SQL += "        GbHost, Part, Amt1, Amt2, SeqNo, yymm, DRGSELF, ORDERNO,                                        ";
                            SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,                                     ";
                            SQL += "        RoomCode,DIV,GBSELNOT,GBSUGBS,GBSGADD , BCODE,POWDER,GBER,GBHU,                                 ";
                            SQL += "        GBSUGBAB, GBSUGBAC, GBSUGBAD, OPGUBUN, HIGHRISK, GBNGT2, asadd )                                ";      //2021-08-05 재원심사 GROUP BY 누락 컬럼 추가
                            SQL += " VALUES (                                                                                               ";
                            SQL += "        " + VB.Val(dt.Rows[i]["IPDNO"].ToString()) + ",                                                 ";
                            SQL += "        " + VB.Val(dt.Rows[i]["TrsNo"].ToString()) + ",                                                 ";
                            SQL += "         TRUNC(SYSDATE),'" + clsPmpaType.TIT.Pano + "',                                                 ";
                            SQL += "        '" + dt.Rows[i]["Bi"].ToString() + "',                                                          ";
                            SQL += "        TO_DATE('" + dt.Rows[i]["Bdate"].ToString() + "','YYYY-MM-DD'),                                 ";
                            SQL += "        SYSDATE,                                                                                        ";
                            SQL += "        '" + dt.Rows[i]["Sunext"].ToString() + "',  '" + dt.Rows[i]["Bun"].ToString() + "',             ";
                            SQL += "        '" + dt.Rows[i]["Nu"].ToString() + "',                                                          ";
                            SQL += "         " + VB.Val(dt.Rows[i]["Qty"].ToString()) + ",     '" + nRetNal + "',                           ";
                            SQL += "         " + nRetAmt1  + ", '" + dt.Rows[i]["GbSpc"].ToString() + "',                                   ";
                            SQL += "        '" + dt.Rows[i]["GbNgt"].ToString() + "',   '" + dt.Rows[i]["GbGisul"].ToString() + "',         ";
                            SQL += "        '" + dt.Rows[i]["GbSelf"].ToString() + "',  '" + dt.Rows[i]["GbChild"].ToString() + "',         ";
                            SQL += "        '" + dt.Rows[i]["DeptCode"].ToString() + "','" + dt.Rows[i]["DrCode"].ToString() + "',          ";
                            SQL += "        '" + dt.Rows[i]["WardCode"].ToString() + "','" + dt.Rows[i]["Sucode"].ToString() + "',          ";
                            SQL += "        '" + dt.Rows[i]["GbSlip"].ToString() + "', '" + dt.Rows[i]["GbHost"].ToString() + "', '!+',     ";
                            SQL += "         " + nRetAmt2 + ", " + 0 + ", '" + VB.Val(dt.Rows[i]["SeqNo"].ToString()) + "',                 ";
                            SQL += "        '" + dt.Rows[i]["YYMM"].ToString() + "' ,'" + dt.Rows[i]["DRGSELF"].ToString() + "',            ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["ORDERNO"].ToString()) + "',                                             ";
                            SQL += "        '" + dt.Rows[i]["ABCDATE"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["OPER_DEPT"].ToString() + "',                                                   ";
                            SQL += "        '" + dt.Rows[i]["OPER_DCT"].ToString() + "',                                                    ";
                            SQL += "        '" + dt.Rows[i]["ORDER_DEPT"].ToString() + "',                                                  ";
                            SQL += "        '" + dt.Rows[i]["ORDER_DCT"].ToString() + "',                                                   ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["EXAM_WRTNO"].ToString()) + "',                                          ";
                            SQL += "        '" + VB.Val(dt.Rows[i]["RoomCode"].ToString()) + "',                                            ";
                            SQL += "         " + VB.Val(dt.Rows[i]["DIV"].ToString()) + ",                                                  ";
                            SQL += "        '" + dt.Rows[i]["GBSELNOT"].ToString().Trim() + "',                                             ";
                            SQL += "        '" + dt.Rows[i]["GBSUGBS"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["GBSGADD"].ToString() + "',                                                     ";
                            SQL += "        '" + dt.Rows[i]["BCODE"].ToString() + "',                                                       ";
                            SQL += "        '" + dt.Rows[i]["POWDER"].ToString() + "',                                                      ";
                            SQL += "        '" + dt.Rows[i]["GBER"].ToString().Trim() + "',                                                 ";
                            if (clsPmpaPb.GstrSugaNewReadOK == "OK")
                            {
                                SQL += " '1',                                                                                               ";
                            }
                            else
                            {
                                SQL += " '',                                                                                                ";
                            }
                            SQL += "        '" + dt.Rows[i]["GBSUGBAB"].ToString().Trim() + "',                                             ";  //2021-08-05 컬럼추가
                            SQL += "        '" + dt.Rows[i]["GBSUGBAC"].ToString().Trim() + "',                                             ";  //2021-08-05 컬럼추가
                            SQL += "        '" + dt.Rows[i]["GBSUGBAD"].ToString().Trim() + "',                                             ";  //2021-08-05 컬럼추가
                            SQL += "        '" + dt.Rows[i]["OPGUBUN"].ToString().Trim() + "',                                              ";  //2021-08-05 컬럼추가
                            SQL += "        '" + dt.Rows[i]["HIGHRISK"].ToString().Trim() + "',                                             ";  //2021-08-05 컬럼추가
                            SQL += "        '" + dt.Rows[i]["GBNGT2"].ToString().Trim() + "',                                               ";  //2021-08-05 컬럼추가
                            SQL += "        '" + dt.Rows[i]["asadd"].ToString().Trim() + "'                                                 ";  //2021-08-05 컬럼추가
                            SQL += " )";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                return false;

                            }

                        }
                    }
                }

                dt.Dispose();
                dt = null;



                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// 심사완료 작업창에서 퇴원시 의약품관리료 조회 생성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGbn"></param>
        /// <param name="strMode">1.심사완료  2.재원심사</param>
        /// <seealso cref="퇴원의약품관리료_조회생성"/>
        public bool DisCharge_Drug_Manager_Fee(PsmhDb pDbCon, string ArgGbn, FpSpread ss2, string strMode)
        {
            string strTrsChk = string.Empty;
            long nILSU1 = 0, nILSU2 = 0;
            long nBaseAmt = 0, nAMT1 = 0;
            int intRowAffected = 0;
            string strSuCode = "", strSuCode2 = "";
            int nRow = 0;
            bool rtnVal = true;

            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            clsPmpaType.cBas_Add_Arg cBArg = null;                          //수가가산 항목 Flag
            clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();   //EDI 계산 금액과 표준코드값을 받기 위함

            clsPmpaPb cPb = new clsPmpaPb();
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIA = new clsIpdAcct();
            clsIuSentChk cISentChk = new clsIuSentChk();
            clsBasAcct cBAcct = new clsBasAcct();

            clsPmpaPb.GstrPowder = "";
            //건강보험+의료급여(NP제외)
            //자격자른것 퇴원이 아니라 완전 퇴원처리될때 처리함
            //퇴원의약품+내복약조제료(입원퇴원약)
            //입원의약품 관리료

            if (clsPmpaType.IMST.InDate == "" || clsPmpaType.IMST.OutDate == "")
            { 
                if (ArgGbn == "2")
                {
                    ComFunc.MsgBox("작업실패. 입원일자 및 퇴원일자가 없습니다.", "오류");
                    return false;
                }
            }

            if (clsPmpaType.IMST.LastTrs != clsPmpaType.TIT.Trsno && ArgGbn == "2")
            {
                ComFunc.MsgBox("마지막 자격이 아닙니다.", "확인요망!");
                return false;
            }

            if (strMode == "1")
            {
                nRow = 12;

            }
            else
            {
                nRow = 0;
            }

            clsPmpaType.IA.Bi1 = Convert.ToUInt16(VB.Mid(clsPmpaType.TIT.Bi, 1, 1));
            if (clsPmpaType.TIT.Bi == "52" || clsPmpaType.TIT.Bi == "55")
            {
                clsPmpaType.IA.Bi1 = 6;     //자보
            }

            clsPmpaType.IA.Date = clsPmpaPb.GstrSysDate;

            try
            {
                #region 입원약 관리료
                //AL651      입원 의약품관리료(1일당)
                //AL652      입원 의약품관리료(2일당)
                //AL653      입원 의약품관리료(3일당)
                //AL654      입원 의약품관리료(4일당)
                //AL655      입원 의약품관리료(5일당)
                //AL656      입원 의약품관리료(6일당)
                //AL657      입원 의약품관리료(7일당)
                //AL658      입원 의약품관리료(8일당)
                //AL659      입원 의약품관리료(9일당)
                //AL660      입원 의약품관리료(10일당)
                //AL661      입원 의약품관리료(11일당)
                //AL662      입원 의약품관리료(12일당)
                //AL663      입원 의약품관리료(13일당)
                //AL674      입원 의약품관리료 14일분(종합병원)
                //AL675      입원 의약품관리료 15일분(종합병원)

                //AL686      16~30
                //AL687      31~

                //AL676      입원 의약품관리료 16~20일분(종합병원)
                //AL677      입원 의약품관리료 21~25일분(종합병원)
                //AL678      입원 의약품관리료 26~30일분(종합병원)
                //AL679      입원 의약품관리료 31~40일분(종합병원)
                //AL680      입원 의약품관리료 41~50일분(종합병원)
                //AL681      입원 의약품관리료 51~60일분(종합병원)
                //AL682      입원 의약품관리료 61~70일분(종합병원)
                //AL683      입원 의약품관리료 71~80일분(종합병원)
                //AL684      입원 의약품관리료 81~90일분(종합병원)
                //AL685      입원 의약품관리료 91일분이상(종합병원)
                #endregion

                #region //입원기간중 해당코드발생건점검
                SQL = "";
                SQL += " SELECT SUCODE,SUM(NAL*QTY) NCNT                                    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                          \r\n";
                SQL += "  WHERE IPDNO =" + clsPmpaType.TIT.Ipdno + "                        \r\n";
                SQL += "    AND TRIM(SUCODE) IN (                                           \r\n";
                SQL += "        SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE      \r\n";
                SQL += "         WHERE GUBUN = '퇴원의약품관리료코드'                       \r\n";
                SQL += "           AND NAME  = '입원의약품관리료'       )                   \r\n";
                SQL += "  GROUP BY SUCODE                                                   \r\n";
                SQL += " HAVING SUM(NAL*QTY) <> 0                                               ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                ss2.ActiveSheet.Cells[nRow + 1, 0].BackColor = System.Drawing.Color.White;

                if (dt.Rows.Count > 0)
                { 
                    if (VB.Val(dt.Rows[0]["NCNT"].ToString()) != 0)
                    {
                        ss2.ActiveSheet.Cells[nRow + 1, 0].Text = dt.Rows[0]["SuCode"].ToString().Trim() + " 발생";
                        ss2.ActiveSheet.Cells[nRow + 1, 0].BackColor = System.Drawing.Color.LightSalmon;
                    }
                }
                
                dt.Dispose();
                dt = null;
                #endregion

                #region //입원기간중 해당코드발생건점검
                SQL = "";
                SQL += " SELECT SUCODE,SUM(NAL*QTY) NCNT                                    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                          \r\n";
                SQL += "  WHERE IPDNO =" + clsPmpaType.TIT.Ipdno + "                        \r\n";
                SQL += "    AND TRIM(SUCODE) IN (                                           \r\n";
                SQL += "        SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE      \r\n";
                SQL += "         WHERE GUBUN = '퇴원의약품관리료코드'                       \r\n";
                SQL += "           AND NAME IN ('퇴원외래의약품관리료','퇴원내복약조제료')) \r\n";
                SQL += "  GROUP BY SUCODE                                                   \r\n";
                SQL += " HAVING SUM(NAL*QTY) <> 0                                               ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                ss2.ActiveSheet.Cells[nRow + 1, 1].BackColor = System.Drawing.Color.White;

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["NCNT"].ToString()) != 0)
                    {
                        ss2.ActiveSheet.Cells[nRow + 1, 1].Text = dt.Rows[0]["SuCode"].ToString().Trim() + " 발생";
                        ss2.ActiveSheet.Cells[nRow + 1, 1].BackColor = System.Drawing.Color.LightSalmon;
                    }
                }
                
                dt.Dispose();
                dt = null;
                #endregion

                #region 산재일경우 의약품관리료 제외 2013-02-15
                if (clsPmpaType.TIT.Bi != "31" && clsPmpaType.TIT.Bi != "33")
                {
                    SQL = "";
                    SQL += " SELECT BDate,SUM(QTY*NAL) NCnt                                         ";
                    SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                              ";
                    SQL += "  WHERE PANO ='" + clsPmpaType.TIT.Pano + "'                            ";
                    SQL += "    AND BDATE >=TO_DATE('" + clsPmpaType.IMST.InDate + "','YYYY-MM-DD') ";
                    SQL += "    AND BDATE <=TO_DATE('" + clsPmpaType.IMST.OutDate + "','YYYY-MM-DD')";
                    SQL += "    AND NU IN ('04','05')                                               ";  //투약,주사료만
                    SQL += "    AND BUN IN ('11','12','20')                                         ";
                    SQL += "    AND SUBSTR(SUCODE,1,2) NOT IN ('KK')                                ";
                    SQL += "    AND TRIM(SUCODE) NOT IN (                                           ";
                    SQL += "        SELECT TRIM(CODE) FROM " + ComNum.DB_PMPA + "BAS_BCODE          ";
                    SQL += "         WHERE GUBUN ='퇴원의약품관리료코드'                            ";
                    SQL += "           AND NAME ='입원의약품관리료'   )                             ";
                    SQL += "  GROUP BY BDATE                                                        ";
                    SQL += " HAVING SUM(QTY*NAL) <> 0                                               ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }

                    nILSU1 = dt.Rows.Count;

                    ss2.ActiveSheet.Cells[nRow, 0].Text = nILSU1.ToString();

                    dt.Dispose();
                    dt = null;

                    if (nILSU1 > 0)
                    {
                        switch (nILSU1)
                        {
                            case 0: strSuCode = ""; break;
                            case 1: strSuCode = "AL651"; break;
                            case 2: strSuCode = "AL652"; break;
                            case 3: strSuCode = "AL653"; break;
                            case 4: strSuCode = "AL654"; break;
                            case 5: strSuCode = "AL655"; break;
                            case 6: strSuCode = "AL656"; break;
                            case 7: strSuCode = "AL657"; break;
                            case 8: strSuCode = "AL658"; break;
                            case 9: strSuCode = "AL659"; break;
                            case 10: strSuCode = "AL660"; break;
                            case 11: strSuCode = "AL661"; break;
                            case 12: strSuCode = "AL662"; break;
                            case 13: strSuCode = "AL663"; break;
                            case 14: strSuCode = "AL674"; break;
                            case 15: strSuCode = "AL675"; break;
                            default: strSuCode = ""; break;
                        }

                        if (nILSU1 >= 16 && nILSU1 <= 30)
                        {
                            strSuCode = "AL686";
                        }
                        else if (nILSU1 >= 31)
                        {
                            strSuCode = "AL687";
                        }


                        if (ArgGbn == "2" && strSuCode != "")
                        {
                            
                            if (cPF.Suga_Read(pDbCon, strSuCode) == false)
                            {
                                ComFunc.MsgBox(strSuCode + " 수가정보 오류!", "작업불가");
                                return false;
                            }

                            cIA.Move_RS_TO_ISG();                   //수가정보 세팅
                                                        
                            nBaseAmt = clsPmpaType.ISG.BaseAmt;
                            nAMT1    = clsPmpaType.ISG.BaseAmt;

                            #region //소아가산로직 추가 2015-04-21--------------------------------------------
                            //6세미만 소아 내복약조제료 20%가산 가산 2017-07-01 소아가산 변경 (20%->50,30))
                            if (clsPmpaType.ISG.Bun == "13" && VB.Left(strSuCode, 2) == "J1" && clsPmpaType.TIT.Age < 6)
                            {
                                if (string.Compare(clsPmpaType.IMST.OutDate, "2017-07-01") >= 0)
                                {
                                    if (clsPmpaType.TIT.Age == 0)
                                    {
                                        nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.5);
                                    }
                                    else
                                    {
                                        nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.3);
                                    }
                                }
                                else
                                {
                                    nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.2);
                                }
                            }

                            if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                            {
                                if (string.Compare(clsPmpaType.IA.Date, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)   //기술료가산
                                {
                                    nAMT1 = (long)Math.Round(nAMT1 * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    nAMT1 = (long)Math.Round(nAMT1 * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                                }
                            }
                            #endregion

                            if (nAMT1 > 0)
                            {
                                #region Ipd_New_Slip Data Set
                                cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.TIT.Ipdno.ToString();
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.TIT.Trsno.ToString();
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrSysDate;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.TIT.Pano;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.OutDate == "" ? clsPublic.GstrSysDate : clsPmpaType.TIT.OutDate;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = clsPmpaType.ISG.Sunext;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = clsPmpaType.ISG.Bun;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = clsPmpaType.ISG.Nu;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = "1";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = "1";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nBaseAmt.ToString();
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = clsPmpaType.ISG.SugbE;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = clsPmpaType.ISG.Sucode;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = " ";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "4";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = clsType.User.IdNumber;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nAMT1.ToString();
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = clsPmpaType.ISG.Bun + "0";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = clsPmpaType.ISG.Sucode;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = clsPmpaType.ISG.Sunext;
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] = "";
                                cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BCODE] = "";
                                #endregion
                                SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                    return false;
                                }
                            }
                        }   //ArgGbn == "2"
                    }   //nILSU1 > 0
                }   //clsPmpaType.TIT.Bi != "31" && clsPmpaType.TIT.Bi != "33"
                #endregion

                #region ->퇴원약 관리료
                //AL601   외래 의약품관리료 1일분
                //AL602   외래 의약품관리료 2일분
                //AL603   외래 의약품관리료 3일분
                //AL604   외래 의약품관리료 4일분
                //AL605   외래 의약품관리료 5일분
                //AL606   외래 의약품관리료 6일
                //AL607   외래 의약품관리료 7일분
                //AL608   외래 의약품관리료 8일분
                //AL609   외래 의약품관리료 9일분
                //AL610   외래 의약품관리료 10일분
                //AL611   외래 의약품관리료 11일분
                //AL612   외래 의약품관리료 12일분
                //AL613   외래 의약품관리료 13일분
                //AL624   외래 의약품관리료 14일분
                //AL625   외래 의약품관리료 15일분
                //AL626   외래 의약품관리료 16~20일분
                //AL627   외래 의약품관리료 21~25일분(종합병원)
                //AL628   외래 의약품관리료 26~30일분(종합병원)
                //AL629   외래 의약품관리료 31~40일분(종합병원)
                //AL630   외래 의약품관리료 41~50일분(종합병원)
                //AL631   외래 의약품관리료 51~60일분(종합병원)
                //AL632   외래 의약품관리료 61~70일분(종합병원)
                //AL633   외래 의약품관리료 71~80일분(종합병원)
                //AL634   외래 의약품관리료 81~90일분(종합병원)
                //AL635   외래 의약품관리료 91일분이상(종합병원)
                //
                //J1010   퇴원 내복약조제료(라1가) 1일분
                //J1020   퇴원 내복약조제료 2일분
                //J1030   퇴원 내복약조제료 3일분
                //J1040   퇴원 내복약조제료 4일분
                //J1050   퇴원 내복약조제료 5일분
                //J1060   퇴원 내복약조제료 6일분
                //J1070   퇴원 내복약조제료 7일분
                //J1080   퇴원 내복약조제료 8일분
                //J1090   퇴원 내복약조제료 9일분
                //J1100   퇴원 내복약조제료 10일분
                //J1110   퇴원 내복약조제료 11일분
                //J1116   퇴원 내복약조제료 16~20일분까지
                //J1120   퇴원 내복약조제료 12일분
                //J1121   퇴원 내복약조제료 21~25일분까지
                //J1126   퇴원 내복약조제료 26~30일분까지
                //J1130   퇴원 내복약조제료 13일분
                //J1131   퇴원 내복약조제료 31~40일분까지
                //J1140   퇴원 내복약조제료 14일분
                //J1141   퇴원 내복약조제료 41~50일분까지
                //J1150   퇴원 내복약조제료 15일분
                //J1151   퇴원 내복약조제료 51~60일분까지
                //J1161   퇴원 내복약조제료 61~70일분까지
                //J1171   퇴원 내복약조제료 71~80일분까지
                //J1181   퇴원 내복약조제료 81~90일분까지
                //J1191   퇴원 내복약조제료 91일분이상
                #endregion

                #region 퇴원약 발생조회
                SQL = "";
                SQL += " SELECT IPDNO,SUCODE,SUNEXT,SUM(QTY*NAL),SUM(NAL) ORDERNAL                  \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                  \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'                               \r\n";
                SQL +=  "   AND IPDNO =" + clsPmpaType.TIT.Ipdno + "                                \r\n";
                SQL += "    AND ACTDATE =TO_DATE('" + clsPmpaType.IMST.OutDate + "','YYYY-MM-DD')   \r\n";
                SQL += "    AND GBSLIP ='T'                                                         \r\n";
                SQL += "  GROUP BY IPDNO,SUCODE,SUNEXT                                              \r\n";
                SQL += " HAVING Sum(Qty * Nal) <> 0                                                 \r\n";
                SQL += "  ORDER BY ORDERNAL DESC                                                    \r\n";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                ss2.ActiveSheet.Cells[nRow + 1, 1].BackColor = System.Drawing.Color.White;

                if (dt.Rows.Count > 0)
                { 
                    nILSU2 = Convert.ToInt16(VB.Val(dt.Rows[0]["OrderNal"].ToString()));
                }

                ss2.ActiveSheet.Cells[nRow, 1].Text = nILSU2.ToString();
                
                dt.Dispose();
                dt = null;
                #endregion

                #region 퇴원약 발생
                if (nILSU2 > 0)
                {
                    switch (nILSU2)
                    {
                        case 1:     strSuCode = "AL200"; strSuCode2 = "J1010"; break;
                        case 2:     strSuCode = "AL200"; strSuCode2 = "J1020"; break;
                        case 3:     strSuCode = "AL200"; strSuCode2 = "J1030"; break;
                        case 4:     strSuCode = "AL200"; strSuCode2 = "J1040"; break;
                        case 5:     strSuCode = "AL200"; strSuCode2 = "J1050"; break;
                        case 6:     strSuCode = "AL200"; strSuCode2 = "J1060"; break;
                        case 7:     strSuCode = "AL200"; strSuCode2 = "J1070"; break;
                        case 8:     strSuCode = "AL200"; strSuCode2 = "J1080"; break;
                        case 9:     strSuCode = "AL200"; strSuCode2 = "J1090"; break;
                        case 10:    strSuCode = "AL200"; strSuCode2 = "J1100"; break;
                        case 11:    strSuCode = "AL200"; strSuCode2 = "J1110"; break;
                        case 12:    strSuCode = "AL200"; strSuCode2 = "J1120"; break;
                        case 13:    strSuCode = "AL200"; strSuCode2 = "J1130"; break;
                        case 14:    strSuCode = "AL200"; strSuCode2 = "J1140"; break;
                        case 15:    strSuCode = "AL200"; strSuCode2 = "J1150"; break;
                        default:    strSuCode = ""; strSuCode2 = "";           break;
                    }

                    if (nILSU2 >= 16 && nILSU2 <= 20)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1116";
                    }
                    else if (nILSU2 >= 21 && nILSU2 <= 25)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1121";
                    }
                    else if (nILSU2 >= 26 && nILSU2 <= 30)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1126";
                    }
                    else if (nILSU2 >= 31 && nILSU2 <= 40)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1131"; 
                    }
                    else if (nILSU2 >= 41 && nILSU2 <= 50)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1141";
                    }
                    else if (nILSU2 >= 51 && nILSU2 <= 60)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1151";
                    }
                    else if (nILSU2 >= 61 && nILSU2 <= 70)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1161";
                    }
                    else if (nILSU2 >= 71 && nILSU2 <= 80)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1171";
                    }
                    else if (nILSU2 >= 81 && nILSU2 <= 90)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1181";
                    }
                    else if (nILSU2 >= 91)
                    {
                        strSuCode = "AL200"; strSuCode2 = "J1191";
                    }
                }

                if (ArgGbn == "2" && strSuCode != "" && strSuCode2 != "")
                {
                    if (cPF.Suga_Read(pDbCon, strSuCode) == false)
                    {
                        ComFunc.MsgBox(strSuCode + " 수가정보 오류!", "작업불가");
                        return false;
                    }

                    cIA.Move_RS_TO_ISG();                   //수가정보 세팅
                    
                    nBaseAmt = clsPmpaType.ISG.BaseAmt;
                    nAMT1 = clsPmpaType.ISG.BaseAmt;
                    
                    #region //소아가산로직 추가 2015-04-21--------------------------------------------
                    //6세미만 소아 내복약조제료 20%가산 가산 2017-07-01 소아가산 변경 (20%->50,30))
                    if (clsPmpaType.ISG.Bun == "13" && VB.Left(strSuCode, 2) == "J1" && clsPmpaType.TIT.Age < 6)
                    {
                        if (string.Compare(clsPmpaType.IMST.OutDate, "2017-07-01") >= 0)
                        {
                            if (clsPmpaType.TIT.Age == 0)
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.5);
                            }
                            else
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.3);
                            }
                        }
                        else
                        {
                            nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.2);
                        }
                    }

                    if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                    {
                        if (string.Compare(clsPmpaPb.GstrSysDate, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)   //기술료가산
                        {
                            nAMT1 = (long)Math.Round(nAMT1 * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            nAMT1 = (long)Math.Round(nAMT1 * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    #endregion

                    if (nAMT1 > 0)
                    {
                        #region Ipd_New_Slip Data Set
                        cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.TIT.Ipdno.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.TIT.Trsno.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrSysDate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.TIT.Pano;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.OutDate == "" ? clsPublic.GstrSysDate : clsPmpaType.TIT.OutDate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = clsPmpaType.ISG.Sunext;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = clsPmpaType.ISG.Bun;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = clsPmpaType.ISG.Nu;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = "1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = "1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nBaseAmt.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = clsPmpaType.ISG.SugbE;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = clsPmpaType.ISG.Sucode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = " ";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "4";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = clsType.User.IdNumber;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nAMT1.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = clsPmpaType.ISG.Bun + "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = clsPmpaType.ISG.Sucode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = clsPmpaType.ISG.Sunext;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BCODE] = "";
                        #endregion
                        SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return false;
                        }
                    }

                    if (cPF.Suga_Read(pDbCon, strSuCode2) == false)
                    {
                        ComFunc.MsgBox(strSuCode + " 수가정보 오류!", "작업불가");
                        return false; 
                    }

                    cIA.Move_RS_TO_ISG();                   //수가정보 세팅

                    nBaseAmt = clsPmpaType.ISG.BaseAmt;


                    #region 파우더확인
                 
                    SQL = "";
                    SQL += " SELECT b.SUCODE, SUM(b.Qty*b.NAL) POW                                       \r\n";
                    SQL += "   FROM " + ComNum.DB_MED + "ocs_Iorder a , " + ComNum.DB_PMPA + "ipd_new_slip b   \r\n";
                    SQL += "  WHERE a.PtNO = '" + clsPmpaType.TIT.Pano + "'                              \r\n";
                    SQL += "   AND  a.powder ='1'                                                        \r\n";
                    SQL += "   AND  b.ACTDATE =TO_DATE('" + clsPmpaType.IMST.OutDate + "','YYYY-MM-DD')  \r\n";
                    SQL += "    AND b.GBSLIP ='T'                                                        \r\n";
                    SQL += "    AND a.ptno=b.pano                                                        \r\n";
                    SQL += "    AND a.bdate=b.bdate                                                      \r\n";
                    SQL += "    AND a.orderno=b.orderno                                                  \r\n";
                    SQL += "  GROUP BY b.SUCODE  HAVING Sum(b.Qty*b.NAL) <> 0                            \r\n";
                  
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return false;
                    }

                  
                    if (dt.Rows.Count > 0)
                    {
                        clsPmpaPb.GstrPowder = "1";
                    }
                    
                    dt.Dispose();
                    dt = null;
                    #endregion

                    #region //소아가산로직 추가 2015-04-21--------------------------------------------
                    //소아가산 변경 (20%->50,30))  가루약 가산 추가 30%
                    if (clsPmpaPb.GstrPowder == "1" )
                    { 
                        if  (clsPmpaType.ISG.Bun == "13" && VB.Left(strSuCode2, 2) == "J1" )
                        {
                            if (clsPmpaType.TIT.Age == 0)
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.5);
                            }
                            else if (clsPmpaType.TIT.Age < 6 )
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.3);
                            }
                            else
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.3);
                            }
                        }
                    }
                    else
                    {
                        if (clsPmpaType.ISG.Bun == "13" && VB.Left(strSuCode2, 2) == "J1")
                        {
                            if (clsPmpaType.TIT.Age == 0)
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.5);
                            }
                            else if (clsPmpaType.TIT.Age < 6)
                            {
                                nBaseAmt = (long)Math.Truncate(nBaseAmt * 1.3);
                            }
                        }
                    }
              
                    nAMT1 = nBaseAmt;

                    if (string.Compare(clsPmpaType.ISG.SugbE, "0") > 0)
                    {
                        if (string.Compare(clsPmpaPb.GstrSysDate, clsPmpaPb.GISUL_DATE) < 0 && clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] > 0)   //기술료가산
                        {
                            nAMT1 = (long)Math.Round(nAMT1 * clsPmpaPb.OLD_GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            nAMT1 = (long)Math.Round(nAMT1 * clsPmpaPb.GISUL[clsPmpaType.IA.Bi1] / 100.0, 0, MidpointRounding.AwayFromZero);
                        }
                    }
                    

                    #endregion

                    if (nAMT1 > 0)
                    {
                        #region Ipd_New_Slip Data Set
                        cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.TIT.Ipdno.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.TIT.Trsno.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrSysDate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.TIT.Pano;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.OutDate == "" ? clsPublic.GstrSysDate : clsPmpaType.TIT.OutDate;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = clsPmpaType.ISG.Sunext;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = clsPmpaType.ISG.Bun;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = clsPmpaType.ISG.Nu;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = "1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = "1";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nBaseAmt.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = clsPmpaType.ISG.SugbE;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = clsPmpaType.ISG.Sucode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = " ";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "4";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = clsType.User.IdNumber;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nAMT1.ToString();
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = clsPmpaType.ISG.Bun + "0";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = clsPmpaType.ISG.Sucode;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = clsPmpaType.ISG.Sunext;
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BCODE] = "";
                        cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.POWDER] = clsPmpaPb.GstrPowder;
                        
                        #endregion
                        SqlErr = cPF.Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return false;
                        }
                    }

                    clsPmpaPb.GstrPowder = "";
                } 
                #endregion

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 심사완료 및 취소 History 
        /// Author : 김민철
        /// Create Date : 2018.02.26
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgFlag"></param>
        /// <param name="ArgJob"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgSts"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgSName"></param>
        /// <param name="ArgArcDate"></param>
        /// <returns></returns>
        public bool Simsa_History_SAVE(PsmhDb pDbCon, string ArgFlag, string ArgJob, string ArgPano, long ArgIpdNo, long ArgTrsNo, string ArgSts, string ArgBi, string ArgInDate, string ArgOutDate, string ArgSName, string ArgArcDate)
        {
            bool rtnVal = true;

            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            int intRowAffected = 0;
            
            string strSname = clsVbfunc.GetInSaName(pDbCon, clsType.User.IdNumber); ;

            string strVCode = string.Empty;
            string strOGPDBun = string.Empty;
            string strOgPdBun2 = string.Empty;
            string strOGPDBundtl = string.Empty;

            long nAmt50 = 0, nAmt51 = 0, nAMT53 = 0, nAMT54 = 0, nAmt55 = 0;

            try
            {
                //2015-02-03
                SQL = "";
                SQL += " SELECT VCODE,OGPDBUN,OGPDBUN2,OGPDBUNDTL,AMT50,AMT51,AMT53,AMT54,AMT55 ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS                                 ";
                SQL += "  WHERE IPDNO = " + ArgIpdNo + "                                        ";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                                        ";
                SQL += "  ORDER BY GBSTS                                                        ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strVCode = dt.Rows[0]["VCODE"].ToString().Trim();
                    strOGPDBun = dt.Rows[0]["OGPDBUN"].ToString().Trim(); 
                    strOgPdBun2 = dt.Rows[0]["OGPDBUN2"].ToString().Trim(); 
                    strOGPDBundtl = dt.Rows[0]["OGPDBUNDTL"].ToString().Trim(); 
                    
                    nAmt50 = Convert.ToInt64(VB.Val(dt.Rows[0]["AMT50"].ToString()));
                    nAmt51 = Convert.ToInt64(VB.Val(dt.Rows[0]["AMT51"].ToString())); 
                    nAMT53 = Convert.ToInt64(VB.Val(dt.Rows[0]["AMT53"].ToString())); 
                    nAMT54 = Convert.ToInt64(VB.Val(dt.Rows[0]["AMT54"].ToString())); 
                    nAmt55 = Convert.ToInt64(VB.Val(dt.Rows[0]["AMT55"].ToString()));    
                }

                dt.Dispose();
                dt = null;

                #region SQL - Query Build
                SQL = "";
                SQL += " INSERT INTO " + ComNum.DB_PMPA + "IPD_SIMSA_HIS (                                              \r\n";
                SQL += "        IPDNO,TRSNO,PANO,BI,INDATE,OUTDATE,SNAME,GbSTS,Flag,SIMSA_OK,SIMSA_NO,SIMSA_SNAME,      \r\n";
                SQL += "        SIMSA_SABUN,ENTDATE,ArcDate,VCODE,OGPDBUN,OGPDBUN2,OGPDBUNDTL,                          \r\n";
                SQL += "        AMT50,AMT51,AMT53,AMT54,AMT55 ) VALUES (                                                \r\n";
                SQL += "          " + ArgIpdNo + "                                                      --IPDNO         \r\n";
                SQL += "        , " + ArgTrsNo + "                                                      --TRSNO         \r\n";
                SQL += "        ,'" + ArgPano + "'                                                      --PANO          \r\n";
                SQL += "        ,'" + ArgBi + "'                                                        --BI            \r\n";
                SQL += "        ,TO_DATE('" + ArgInDate + "','YYYY-MM-DD')                              --INDATE        \r\n";
                SQL += "        ,TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')                             --OUTDATE       \r\n";
                if (ArgFlag == "Y")
                {
                    SQL += "    ,'" + ArgSName + "'                                                     --SNAME         \r\n";
                    SQL += "    ,'" + ArgSts + "'                                                       --GBSTS         \r\n";
                    SQL += "    ,'Y'                                                                    --FLAG          \r\n";
                }
                else
                {
                    SQL += "    ,'" + ArgSName + "'                                                     --SNAME         \r\n";
                    SQL += "    ,'" + ArgSts + "'                                                       --GBSTS         \r\n";
                    SQL += "    ,''                                                                     --FLAG          \r\n";
                }

                if (ArgJob == "심사완료")
                {
                    SQL += "    , SYSDATE                                                               --SIMSA_OK      \r\n";
                    SQL += "    ,''                                                                     --SIMSA_NO      \r\n";
                }
                else if (ArgJob == "심사취소")
                {
                    SQL += "    ,''                                                                     --SIMSA_OK      \r\n";
                    SQL += "    , SYSDATE                                                               --SIMSA_NO      \r\n";
                }
                SQL += "        ,'" + strSname + "'                                                     --SIMSA_SNAME   \r\n";
                SQL += "        , " + clsType.User.IdNumber + "                                         --SIMSA_SABUN   \r\n";
                SQL += "        , SYSDATE                                                               --ENTDATE       \r\n";
                SQL += "        , TO_DATE('" + ArgArcDate + "','YYYY-MM-DD')                            --ArcDate       \r\n";
                SQL += "        ,'" + strVCode + "'                                                     --VCODE         \r\n";
                SQL += "        ,'" + strOGPDBun + "'                                                   --OGPDBUN       \r\n";
                SQL += "        ,'" + strOgPdBun2 + "'                                                  --OGPDBUN2      \r\n";
                SQL += "        ,'" + strOGPDBundtl + "'                                                --OGPDBUNDTL    \r\n";
                SQL += "        , " + nAmt50 + "                                                        --AMT50         \r\n";
                SQL += "        , " + nAmt51 + "                                                        --AMT51         \r\n";
                SQL += "        , " + nAMT53 + "                                                        --AMT53         \r\n";
                SQL += "        , " + nAMT54 + "                                                        --AMT54         \r\n";
                SQL += "        , " + nAmt55 + "                                                        --AMT55         \r\n";
                SQL += "        )                                                                                           ";
                #endregion
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }

        /// <summary>
        /// Description : 뒷자격 유무 조회
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgInDate"></param>
        /// <returns></returns>
        public DataTable sel_Ipd_NextTrans(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, string ArgInDate)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT Pano,TRSNO,IPDNO,TO_CHAR(INDATE ,'YYYY-MM-DD') INDATE   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS                         \r\n";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                \r\n";
                SQL += "    AND IPDNO = "+ ArgIpdNo + "                                 \r\n";
                SQL += "    AND InDate> TO_DATE('" + ArgInDate + "','YYYY-MM-DD')       \r\n";
                SQL += "    AND GBIPD NOT IN  ('D','9')                                 \r\n"; //삭제,지병 제외
                SQL += "  ORDER BY INDATE                                               \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        /// <summary>
        /// Description : 미전송 OCS 오더를 Check
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgLastInDate"></param>
        /// <returns></returns>
        public bool sel_Ipd_NonTrans_Order(PsmhDb pDbCon, string ArgPano, string ArgInDate, string ArgOutDate, string ArgLastInDate, string ArgLastChk)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int i = 0;
            string strError = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT A.BDATE, A.SUCODE,  A.QTY , A.NAL           ";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER A           ";
                SQL += "  WHERE A.PTNO = '" + ArgPano + "'                  ";

                if (ArgLastChk == "NO")
                { 
                    SQL += "    AND A.BDATE >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD')    ";
                    SQL += "    AND A.BDATE <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')   ";
                }
                else if (ArgLastChk == "OK")
                { 
                    SQL += "    AND A.BDATE >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD')    ";
                    SQL += "    AND A.BDATE < TO_DATE('" + ArgLastInDate + "','YYYY-MM-DD') ";
                }
                else
                { 
                    SQL += "    AND A.BDATE >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD')    ";
                    SQL += "    AND A.BDATE <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')   ";
                }

                SQL += "    AND A.GBSEND IN ('*','Z')                                       ";
                SQL += "    AND (A.GBIOE ='I' OR A.GBIOE IS NULL )                          ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strError += dt.Rows[i]["BDATE"].ToString().Trim() + " ";
                        strError += dt.Rows[i]["SUCODE"].ToString().Trim() + " ";
                        strError += dt.Rows[i]["QTY"].ToString().Trim() + "*";
                        strError += dt.Rows[i]["NAL"].ToString().Trim() + ComNum.VBLF;
                    }

                    if (strError != "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF; 
                        clsPublic.GstrMsgList += " ▷OCS 미전송 오더가 있습니다!! " + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 심사 완료를 할수 없습니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 전산정보팀으로 연락바랍니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += strError + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");

                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 마이너스 수가 체크
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <returns></returns>
        public bool sel_Ipd_NewSlip_Minus(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int i = 0;
            string strError = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT A.BDATE, A.SUNEXT,  B.SUNAMEK,  SUM(A.AMT1) AMT     \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,               \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_sun b                     \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                            \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                            \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                            \r\n";
                SQL += "    AND A.SUNEXT = B.SUNEXT                                 \r\n";
                SQL += "    AND A.BASEAMT <> 0                                      \r\n";
                SQL += "  GROUP BY A.BDATE, A.SUNEXT , B.SUNAMEK                    \r\n";
                SQL += " HAVING SUM(A.QTY * A.NAL) < 0                              \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strError += dt.Rows[i]["BDATE"].ToString().Trim() + " ";
                        strError += dt.Rows[i]["SUNEXT"].ToString().Trim() + " ";
                        strError += dt.Rows[i]["AMT"].ToString().Trim() + " ";
                        strError += dt.Rows[i]["SUNAMEK"].ToString().Trim() + ComNum.VBLF;
                    }

                    if (strError != "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " ▷수가발생 오류가 있습니다.!! " + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 심사 완료를 할수 없습니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 전산정보팀으로 연락바랍니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += strError + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");

                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : S항 수가항목 체크
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <returns></returns>
        public bool sel_Ipd_NewSlip_SugbS(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int i = 0;
            string strError = string.Empty;
            string strNu = string.Empty;
            string strSelf = string.Empty;
            string strBDate = string.Empty;
            string strSugbs = string.Empty;
            string strSuNext = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT A.BDATE, A.SUNEXT, A.NU, A.GBSELF, B.SUNAMEK,               \r\n";
                SQL += "        SUM(A.AMT1) AMT, A.GBSUGBS                                  \r\n";                    
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A,                       \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_SUN b                             \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                                    \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                                    \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                                    \r\n";
                SQL += "    AND A.SUNEXT = B.SUNEXT                                         \r\n";
                SQL += "    AND a.SUNEXT IN (                                               \r\n";
                SQL += "        SELECT SUNEXT FROM " + ComNum.DB_PMPA + "BAS_SUN            \r\n";
                SQL += "         Where SUGBS IN ('3','6','7','8') )                                 \r\n";
                SQL += "  GROUP BY A.BDATE, A.SUNEXT, A.NU, A.GBSELF, B.SUNAMEK, A.GBSUGBS  \r\n";
                SQL += " HAVING SUM(A.QTY * A.NAL) <> 0                                     \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strNu       = dt.Rows[i]["NU"].ToString().Trim();         
                        strSelf     = dt.Rows[i]["GbSelf"].ToString().Trim();   
                        strBDate    = dt.Rows[i]["BDATE"].ToString().Trim();   
                        strSugbs    = dt.Rows[i]["GBSUGBS"].ToString().Trim(); 
                        strSuNext   = dt.Rows[i]["SUNEXT"].ToString().Trim(); 
                            
                        if (string.Compare(strBDate, "2016-09-01") < 0)
                        { 
                            if (strSugbs == "6" || strSugbs == "7")
                            { 
                                strError += strBDate + " " + strSuNext + " S항오류" + ComNum.VBLF;
                            }

                            if (string.Compare(strNu, "21") < 0 && strSelf != "0")
                            { 
                                strError += strBDate + " " + strSuNext + " 누적범위 오류" + ComNum.VBLF;
                            }

                            if (string.Compare(strNu, "21") >= 0 && strSelf == "0")
                            { 
                                strError += strBDate + " " + strSuNext + " 누적범위 오류" + ComNum.VBLF;
                            }
                        }
                        else
                        {
                            if (strSugbs == "6" || strSugbs == "7")
                            { 
                                if (strSelf != "0")
                                { 
                                    strError += strBDate + " " + strSuNext + " Self항 오류" + ComNum.VBLF;
                                }

                                if (string.Compare(strNu, "21") < 0 && strSelf != "0")
                                { 
                                    strError += strBDate + " " + strSuNext + " 누적범위 오류" + ComNum.VBLF;
                                }

                                if (string.Compare(strNu, "21") >= 0 && strSelf == "0")
                                {
                                    strError += strBDate + " " + strSuNext + " 누적범위 오류" + ComNum.VBLF;
                                }
                            }
                            else
                            {
                                if (string.Compare(strNu, "21") < 0 && strSelf != "0")
                                { 
                                    strError += strBDate + " " + strSuNext + " 누적범위 오류" + ComNum.VBLF;
                                }

                                if (string.Compare(strNu, "21") >= 0 && strSelf == "0")
                                { 
                                    strError += strBDate + " " + strSuNext + " 누적범위 오류" + ComNum.VBLF;
                                }
                            }
                        }
                    }

                    if (strError != "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " ▷수가발생 오류가 있습니다.!! " + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 심사 완료를 할수 없습니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 전산정보팀으로 연락바랍니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += strError + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");

                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 컨설트 미완료 수가항목 체크
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgOutDate"></param>
        /// <returns></returns>
        public bool sel_Ipd_NewSlip_Consult(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, string ArgInDate, string ArgOutDate)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int i = 0;
            string strError = string.Empty;
           
            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(ENTDATE,'YYYY-MM-DD') INDATE,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ";
                SQL += "        FRDEPTCODE,TODEPTCODE                                                   ";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_ITRANSFER                                      ";
                SQL += "  WHERE PTNO = '" + ArgPano + "'                                                ";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                                                ";
                //2015-10-05 심사과 이민주쌤 연락 - 컨설트 미확인 오더가 현재 심사하는 자격과 상관없는 일자는 제외
                //현재 심사하는 기간동안만 체크하도록 변경요청함
                SQL += "    AND BDate >=TO_DATE('" + ArgInDate + "','YYYY-MM-DD')                       ";
                if (ArgOutDate != "")
                {
                    SQL += "    AND BDate <=TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')                  ";
                }
                SQL += "    AND (GbConfirm  =' ' OR GbConfirm  ='T' OR GbConfirm IS NULL)               ";  //미완료된것
                SQL += "    AND (GbDel <> '*' OR GbDel IS NULL)                                         ";  //삭제안된것
                SQL += "    AND GbSEND <> '*'                                                           ";  //미전송 제외

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strError += "입원일:"   + dt.Rows[i]["INDATE"].ToString().Trim();
                        strError += " 처방일:"  + dt.Rows[i]["BDATE"].ToString().Trim();
                        strError += " "         + dt.Rows[i]["FRDEPTCODE"].ToString().Trim();
                        strError += "과->> "    + dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                        strError += " 컨설트 미완료건이 있습니다." + ComNum.VBLF;
                    }

                    if (strError != "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " ▷ 컨설트 미처리건이 있습니다.!! " + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 심사 완료를 할수 없습니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += strError + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");

                        rtnVal = false;
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 혈액불출 관련 미전송오더 체크
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgInDate"></param>
        /// <returns></returns>
        public bool sel_Ipd_Work_IpdSlip_Send_Chk(PsmhDb pDbCon, string ArgPano, string ArgInDate)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT SuCode,SUM(Qty*Nal) SQty                                            \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "WORK_IPDSLIP_SEND                             \r\n";
                SQL += "  WHERE Pano = '" + ArgPano + "'                                            \r\n";
                SQL += "    AND IPDOPD IN ('I','K')                                                 \r\n";
                SQL += "    AND SuCode IS NOT NULL                                                  \r\n";
                SQL += "    AND (SENDTIME IS NULL OR SENDTIME = '')                                 \r\n";    //미전송건
                SQL += "    AND BDate  >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD')                 \r\n";    //입원기간
                SQL += "  Group BY SuCode                                                           \r\n";
                SQL += "  Having Sum(Qty*Nal) > 0                                                       ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt16(VB.Val(dt.Rows[0]["SQTY"].ToString())) > 0)
                    {
                        ComFunc.MsgBox("혈액불출 미전송 오더가 있습니다.", "심사완료 불가");
                        dt.Dispose();
                        dt = null;
                        rtnVal = false;
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 의료급여 정신과 단일수가 점검
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <returns></returns>
        public bool sel_Ipd_Gub_Np_Chk(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            string strError = string.Empty;
            int i = 0;

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ,SUCODE,SUM(AMT1+AMT2) SAMT           \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                      \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                                                \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                                                \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                                                \r\n";
                SQL += "    AND GBSELF ='0'                                                             \r\n";  //급여만
                SQL += "    AND SUCODE NOT IN ( 'AR402', 'AR500', 'AR501', 'AR312', 'AR322', 'AR332' )  \r\n";
                SQL += "  GROUP BY  TO_CHAR(BDATE,'YYYY-MM-DD') ,SUCODE                                 \r\n";
                SQL += " HAVING SUM(AMT1+AMT2) <> 0                                                     ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strError += "처방일자:" + dt.Rows[i]["BDATE"].ToString().Trim();
                        strError += "수가코드:" + dt.Rows[i]["SUCODE"].ToString().Trim();
                        strError += "발생금액:" + dt.Rows[i]["SAMT"].ToString().Trim() + ComNum.VBLF;
                    }

                    if (strError != "")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " ▷ 의료급여 정신과 단일수가외에 금액발생 수가 있습니다.! " + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += " 심사 완료를 할수 없습니다." + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += strError + ComNum.VBLF;
                        clsPublic.GstrMsgList += "=============================================" + ComNum.VBLF;

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "오류");

                        rtnVal = false;
                    }
                    
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        /// <summary>
        /// Description : 퇴원일자 보다 큰 처방일자가 있는지 체크함
        /// Author : 김민철
        /// Create Date : 2018.02.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgOutDate"></param>
        /// <returns></returns>
        public bool sel_Ipd_Trans_LastSlip_Chk(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo, string ArgOutDate)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, SUM(AMT1) AMT    ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                  ";
                SQL += "  WHERE BDATE > TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')  ";
                SQL += "    AND PANO = '" + ArgPano + "'                            ";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                            ";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                            ";
                SQL += "    AND TRIM(SUNEXT) NOT IN (                               ";
                SQL += "        SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE    ";
                SQL += "         WHERE GUBUN ='원무영수제외코드' )                  ";  //저가약제 제외코드 2010-11-22
                SQL += "  GROUP BY BDATE                                            ";
                SQL += " HAVING SUM(AMT1) <> 0                                      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(VB.Val(dt.Rows[0]["AMT"].ToString())) != 0)
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "퇴원일자: " + ArgOutDate;
                        clsPublic.GstrMsgList += ComNum.VBLF + "처방일자: " + dt.Rows[0]["BDATE"].ToString().Trim();
                        clsPublic.GstrMsgList += ComNum.VBLF + "DATA를 정리하시고 심사 완료를 해주세요";

                        ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");

                        rtnVal = false;
                    }

                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }
        /// <summary>
         /// Description : 초음파 급여 대상자 Slip 체크
         /// Author : 김민철
         /// Create Date : 2018.02.27
         /// </summary>
         /// <param name="pDbCon"></param>
         /// <param name="ArgPano"></param>
         /// <param name="ArgIpdNo"></param>
         /// <param name="ArgTrsNo"></param>
         /// <returns></returns>
        public bool sel_Ipd_Trans_Exam_Chk(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, string ArgINDate, string ArgOutDate)
        {
            bool rtnVal = true;
            string SQL = ""; string SqlErr = ""; DataTable dt = null;
            int i = 0;
            try
            {
                SQL = "";
                SQL += " SELECT DISTINCT '혈액검사' gubun ,to_char(A.bdate,'yyyy-mm-dd') bdate    ";
                SQL += "   FROM " + ComNum.DB_MED + "Exam_Specmst A, " + ComNum.DB_MED + "EXAM_ORDER  B                ";
                SQL += "  WHERE A.BloodDate >= TO_DATE('" + ArgINDate + "','YYYY-MM-DD')  ";
                SQL += "    AND A.BloodDate <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')  ";
                SQL += "    AND A.Bdate <  TO_DATE('" + ArgINDate + "','YYYY-MM-DD')  ";
                SQL += "    AND A.STATUS not in ('00','06') ";
                SQL += "    AND A.PANO = '" + ArgPano + "'                            ";
                SQL += "    AND (A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ') ";
                SQL += "    AND A.PANO = B.PANO  ";
                SQL += "    AND A.BDATE = B.BDATE  ";
                SQL += "    AND A.ORDERNO = B.ORDERNO                           ";
                SQL += "    AND A.ipdopd='O'                                          ";
                
                SQL += "    union all                                               ";
                SQL += " SELECT DISTINCT '방사선' gubun ,to_char(bdate,'yyyy-mm-dd')  bdate   ";
                SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                  ";
                SQL += "  WHERE SEEKDATE >= TO_DATE('" + ArgINDate + "','YYYY-MM-DD')  ";
                SQL += "    AND SEEKDATE <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')  ";
                SQL += "    AND Bdate < TO_DATE('" + ArgINDate + "','YYYY-MM-DD')  ";
                SQL += "    AND PANO = '" + ArgPano + "'                            ";
                SQL += "    AND ipdopd='O'                                          ";
                SQL += "    AND gbreserved ='7'                                     ";
                
                SQL += "    union all                                               ";
                SQL += " SELECT DISTINCT '특수검사' gubun ,to_char(bdate,'yyyy-mm-dd')  bdate   ";
                SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                  ";
                SQL += "  WHERE RDATE >= TO_DATE('" + ArgINDate + "','YYYY-MM-DD')  ";
                SQL += "    AND RDATE <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')  ";
                SQL += "    AND Bdate < TO_DATE('" + ArgINDate + "','YYYY-MM-DD')  ";
                SQL += "    AND PtNO = '" + ArgPano + "'                            ";
                SQL += "    AND gbio='O'                                          ";
                SQL += "    AND gbftp ='Y'                                     ";
                SQL += "    AND GUBUN IN ('1', '4')                             ";
                
                //  SQL += "    AND pacs_end   is not null                              ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    clsPublic.GstrMsgList = "";
                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        clsPublic.GstrMsgList += "처방구분: " + dt.Rows[i]["gubun"].ToString().Trim() + ComNum.VBLF  ;
                        clsPublic.GstrMsgList += "처방일자: " + dt.Rows[i]["BDATE"].ToString().Trim() + ComNum.VBLF;
                    }
                    ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");

                   
                    
                    rtnVal = false;
                   
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }


        
        public DataTable sel_Ipd_NewSlip_Sono(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo)
        {
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT SUM(NAL*QTY) nQty                       \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP      \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                \r\n";
                SQL += "    AND BUN = '49'                              \r\n"; 
                SQL += " HAVING SUM(AMT1) > 0                           \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
                
                return dt;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// Description : 퇴원당일 수가조정 구분표시
        /// Author : 김민철
        /// Create Date : 2018.08.14
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgROWID"></param>
        /// <seealso cref="FrmSlipScreen_NEW : Read_TODAY_SUCODE_EDIT / Read_TODAY_SUCODE_SIMOKCH 통합"/>
        /// <returns></returns>
        public string Read_TODAY_SUCODE_EDIT(PsmhDb pDbCon, string ArgGubun, long ArgTrsNo, string ArgSuCode, string ArgBDate, string ArgROWID, string ArgTime)
        {
            string rtnVal = "";
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT ROWID                                   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP      \r\n";
                SQL += "  WHERE ACTDATE = TRUNC(SYSDATE)                \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                \r\n";
                SQL += "    AND SUCODE = '" + ArgSuCode + "'            \r\n";
                if (ArgBDate != "") { SQL += "    AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')  \r\n"; }
                if (ArgROWID != "") { SQL += "    AND ROWID = '" + ArgROWID + "'    \r\n"; }
                if (ArgGubun == "EDIT")
                {
                    SQL += " AND PART = '" + clsType.User.IdNumber + "'                        \r\n";
                }
                else if (ArgGubun == "SIM")
                {
                    SQL += " AND EntDate > TO_DATE('" + ArgTime + "','YYYY-MM-DD HH24:MI')       \r\n";
                }
                

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string Read_JSim_Spec(PsmhDb pDbCon, long ArgIpdNo, string ArgSuNext, string ArgInDate)
        {
            string rtnVal = "";
            string SQL = ""; string SqlErr = ""; DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT ROWID                                               \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "MIR_IPDSPEC                   \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND IPDNO = '" + ArgIpdNo + "'                          \r\n";
                SQL += "    AND SUNEXT = '" + ArgSuNext + "'                        \r\n";
                SQL += "    AND INDATE = TO_DATE('" + ArgInDate + "','YYYY-MM-DD')  \r\n";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }   
}
