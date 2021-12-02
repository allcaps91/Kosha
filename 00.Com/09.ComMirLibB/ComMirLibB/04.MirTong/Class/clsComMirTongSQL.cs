using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ComBase;
using ComDbB;


namespace ComMirLibB.MirTong
{
    public class clsComMirTongSQL
    {
        string SQL = string.Empty;

        /// <summary>
        /// 의사별 보험종류별 삭감액 통계
        /// </summary>
        /// <param name="pDbCon">DB커넥션</param>
        /// <param name="strStartDate">시작시간</param>
        /// <param name="strEndDate">끝</param>
        /// <param name="strIO">입원,외래(I,O)</param>
        /// <returns></returns>
        public DataTable sel_DoctSakAmt(PsmhDb pDbCon, string strStartDate, string strEndDate, string strIO = "")
        {
            DataTable dt = null;            
        
            SQL = "";           
            SQL += " SELECT B.PrintRanking                                                                                              \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(A.DeptCode)                                                AS DEPTNAME \r \n";
            SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)														  AS DRNAME \r\n";
            SQL += "      , SUM(DECODE(A.Johap, 1, A.JAmt, 0)) BohumAmt                                                                 \r\n";
            SQL += "      , SUM(DECODE(A.Johap, 5, A.JAmt, 0)) BohoAmt                                                                  \r\n";
            SQL += "      , SUM(DECODE(A.Johap, 6, A.JAmt, 0)) SanAmt                                                                   \r\n";
            SQL += "      , SUM(DECODE(a.johap,1,a.jamt,0) + DECODE(a.Johap,5,a.JAmt,0) + DECODE(a.Johap,6,a.JAmt,0)) Amt               \r\n";
            SQL += "        FROM " + ComNum.DB_PMPA + "SAK_SIMSADOCT A                                                                  \r\n";
            SQL += "      ,      " + ComNum.DB_PMPA + "Bas_ClinicDept B                                                                 \r\n";
            SQL += "        WHERE 1 = 1                                                                                                 \r\n";
            SQL += "        AND A.TDATE >= " + ComFunc.covSqlDate(strStartDate, "YYYY-MM-DD", false);
            SQL += "        AND A.TDATE <= " + ComFunc.covSqlDate(strEndDate, "YYYY-MM-DD", false);
            if (strIO == "O")
            {
                SQL += "    AND A.IpdOpd = 'O'                                                                                         \r\n";
            }
            else if (strIO == "I")
            {
                SQL += "    AND A.IpdOpd = 'I'                                                                                         \r\n";
            }
            SQL += "        AND A.DeptCode = B.DeptCode(+)                                                                             \r\n";
            SQL += "        GROUP BY B.PrintRanking, A.DeptCode, A.DrCode                                                              \r\n";
            SQL += "        ORDER BY B.PrintRanking, A.DeptCode, A.DrCode                                                              \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_DoctSakDTL(PsmhDb pDbCon, Dictionary<string, string> dicFlag)
        {
            DataTable dt = null;

            if (dicFlag["보험구분"] == "자보")
            {
                SQL = "";
                SQL = " SELECT DISTINCT a.DeptCode                                                      " + ComNum.VBLF;
                SQL += "      , a.DrCode                                                                " + ComNum.VBLF;
                SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) DrName  " + ComNum.VBLF;
                SQL += "      , b.Pano												                    " + ComNum.VBLF;
                SQL += "      , '52' Bi                                                                 " + ComNum.VBLF;    //자보
                SQL += "      , b.Sname                                                                 " + ComNum.VBLF;
                SQL += "      , b.ILLCode1                                                              " + ComNum.VBLF;
                SQL += "      , b.YYMM                                                                  " + ComNum.VBLF;
                SQL += "      , a.JCode                                                                 " + ComNum.VBLF;
                SQL += "      , c.Bun                                                                   " + ComNum.VBLF;
                SQL += "      , c.SuNext                                                                " + ComNum.VBLF;
                SQL += "      , c.Price                                                                 " + ComNum.VBLF;
                SQL += "      , c.Qty                                                                   " + ComNum.VBLF;
                SQL += "      , c.Nal                                                                   " + ComNum.VBLF;
                SQL += "      , a.JAMT                                                                  " + ComNum.VBLF;
                SQL += "      , d.SuNameK                                                               " + ComNum.VBLF;
                SQL += "      , a.YYMM                                                                  " + ComNum.VBLF;
                SQL += "      , DECODE(c.GbGisul,'1',c.Amt*(100+b.RateGasan)/100,c.Amt) Amt             " + ComNum.VBLF;
                SQL += "      , A.WRTNO                                                                 " + ComNum.VBLF;
                SQL += "      , 'SAK_SIMSADOCT' AS Tables                                                " + ComNum.VBLF;
                SQL += "      , a.ROWID                                                                 " + ComNum.VBLF;
                SQL += "      , a.Remark                                                                " + ComNum.VBLF;
                SQL += "      , b.JUMIN1                                                                " + ComNum.VBLF;
                SQL += "      , b.JUMIN2                                                                " + ComNum.VBLF;
                SQL += "        FROM " + ComNum.DB_PMPA + "SAK_SIMSADOCT a                              " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "MIR_TAID b                                   " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "MIR_TADTL c                                  " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "BAS_SUN d                                    " + ComNum.VBLF;
                if (dicFlag["작업구분"] == "진료월")
                {
                    SQL += "        WHERE a.YYMM >=  '" + dicFlag["진료시작기간"] + "'                                           " + ComNum.VBLF;
                    SQL += "        AND a.YYMM <=  '" + dicFlag["진료종료기간"] + "'                                             " + ComNum.VBLF;
                }
                else if (dicFlag["작업구분"] == "통보월")
                {
                    SQL += "        WHERE a.TDate >=  TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                    SQL += "          AND a.TDate <=  TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                }
                else if (dicFlag["작업구분"] == "EDI접수월")
                {
                    SQL += "   ,  ( SELECT YYMM, JEPNO FROM KOSMOS_PMPA.EDI_TAJEPSU                                               " + ComNum.VBLF;
                    SQL += "          WHERE JEPDate >= TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                    SQL += "            AND JEPDate <= TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                    SQL += "        GROUP BY YYMM, JEPNO                                                                          " + ComNum.VBLF;
                    SQL += "       ) E                                                                                            " + ComNum.VBLF;
                    SQL += "       WHERE A.YYMM = E.YYMM                                                                          " + ComNum.VBLF;
                    SQL += "        AND A.JEPNO = E.JEPNO                                                                         " + ComNum.VBLF;
                }

                SQL += "       AND a.Johap = '7'                                                                         " + ComNum.VBLF;
                if (dicFlag["입원구분"] == "외래")
                {
                    SQL += "       AND a.IpdOpd='O'                                                                         " + ComNum.VBLF;
                }
                if (dicFlag["입원구분"] == "입원")
                {
                    SQL += "       AND a.IpdOpd='I'                                                                         " + ComNum.VBLF;
                }
                if (dicFlag["진료과코드"] != "**")
                {
                    SQL += "       AND a.DeptCode='" + dicFlag["진료과코드"] + "'                                               " + ComNum.VBLF;
                }
                if (dicFlag["의사코드"] != "****")
                {
                    SQL += "       AND a.DrCode='" + dicFlag["의사코드"] + "'                                               " + ComNum.VBLF;
                }
                if (dicFlag["EDI항코드"] != "**")
                {
                    SQL += "       AND a.Edihang='" + dicFlag["EDI항코드"] + "'                                               " + ComNum.VBLF;
                }
                if (dicFlag["접수번호"] != "")
                {
                    SQL += "       AND a.JEPNO='" + dicFlag["접수번호"] + "'                                               " + ComNum.VBLF;
                }

                SQL += "       AND a.WRTNO=b.WRTNO(+)                                                                    " + ComNum.VBLF;
                SQL += "       AND a.WRTNO=c.WRTNO(+)                                                                        " + ComNum.VBLF;
                SQL += "       AND a.JulNo=c.EdiSeq(+)                                                                        " + ComNum.VBLF;
                SQL += "       AND c.SuNext=d.SuNext(+)                                                                        " + ComNum.VBLF;
                SQL += "       ORDER BY a.DeptCode,a.DrCode,b.Pano,b.YYMM,c.Bun,c.SuNext                                       " + ComNum.VBLF;
            }
            else if (dicFlag["보험구분"] == "산재")
            {
                SQL = "";
                SQL = " SELECT DISTINCT a.DeptCode                                                      " + ComNum.VBLF;
                SQL += "      , a.DrCode                                                                " + ComNum.VBLF;
                SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) DrName  " + ComNum.VBLF;
                SQL += "      , b.Pano												                    " + ComNum.VBLF;
                SQL += "      , '31' Bi                                                                 " + ComNum.VBLF;    //산재
                SQL += "      , b.Sname                                                                 " + ComNum.VBLF;
                SQL += "      , b.ILLCode1                                                              " + ComNum.VBLF;
                SQL += "      , b.YYMM                                                                  " + ComNum.VBLF;
                SQL += "      , a.JCode                                                                 " + ComNum.VBLF;
                SQL += "      , c.Bun                                                                   " + ComNum.VBLF;
                SQL += "      , c.SuNext                                                                " + ComNum.VBLF;
                SQL += "      , c.Price                                                                 " + ComNum.VBLF;
                SQL += "      , c.Qty                                                                   " + ComNum.VBLF;
                SQL += "      , c.Nal                                                                   " + ComNum.VBLF;
                SQL += "      , a.JAMT                                                                  " + ComNum.VBLF;
                SQL += "      , d.SuNameK                                                               " + ComNum.VBLF;
                SQL += "      , a.YYMM                                                                  " + ComNum.VBLF;
                SQL += "      , DECODE(c.GbGisul,'1',c.Amt*(100+b.RateGasan)/100,c.Amt) Amt             " + ComNum.VBLF;
                SQL += "      , A.WRTNO                                                                 " + ComNum.VBLF;
                SQL += "      , 'SAK_SIMSADOCT' AS Tables                                               " + ComNum.VBLF;
                SQL += "      , a.ROWID                                                                 " + ComNum.VBLF;
                SQL += "      , a.Remark                                                                " + ComNum.VBLF;
                SQL += "      , b.JUMIN1                                                                " + ComNum.VBLF;
                SQL += "      , b.JUMIN2                                                                " + ComNum.VBLF;
                SQL += "        FROM " + ComNum.DB_PMPA + "SAK_SIMSADOCT a                              " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "MIR_SANID b                                   " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "MIR_SANDTL c                                  " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "BAS_SUN d                                    " + ComNum.VBLF;
                if (dicFlag["작업구분"] == "진료월")
                {
                    SQL += "        WHERE a.YYMM >=  '" + dicFlag["진료시작기간"] + "'                                           " + ComNum.VBLF;
                    SQL += "        AND a.YYMM <=  '" + dicFlag["진료종료기간"] + "'                                             " + ComNum.VBLF;
                }
                else if (dicFlag["작업구분"] == "통보월")
                {
                    SQL += "        WHERE a.TDate >=  TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                    SQL += "          AND a.TDate <=  TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                }
                else if (dicFlag["작업구분"] == "EDI접수월")
                {
                    SQL += "   ,  ( SELECT YYMM, JEPNO FROM KOSMOS_PMPA.EDI_SANJEPSU                                               " + ComNum.VBLF;
                    SQL += "          WHERE JEPDate >= TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                    SQL += "            AND JEPDate <= TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                    SQL += "        GROUP BY YYMM, JEPNO                                                                          " + ComNum.VBLF;
                    SQL += "       ) E                                                                                            " + ComNum.VBLF;
                    SQL += "       WHERE A.YYMM = E.YYMM                                                                          " + ComNum.VBLF;
                    SQL += "        AND A.JEPNO = E.JEPNO                                                                         " + ComNum.VBLF;
                }

                SQL += "       AND a.Johap = '6'                                                                         " + ComNum.VBLF;
                if (dicFlag["입원구분"] == "외래")
                {
                    SQL += "       AND a.IpdOpd='O'                                                                         " + ComNum.VBLF;
                }
                if (dicFlag["입원구분"] == "입원")
                {
                    SQL += "       AND a.IpdOpd='I'                                                                         " + ComNum.VBLF;
                }
                if (dicFlag["진료과코드"] != "**")
                {
                    SQL += "       AND a.DeptCode='" + dicFlag["진료과코드"] + "'                                               " + ComNum.VBLF;
                }
                if (dicFlag["의사코드"] != "****")
                {
                    SQL += "       AND a.DrCode='" + dicFlag["의사코드"] + "'                                               " + ComNum.VBLF;
                }
                if (dicFlag["EDI항코드"] != "**")
                {
                    SQL += "       AND a.Edihang='" + dicFlag["EDI항코드"] + "'                                               " + ComNum.VBLF;
                }
                if (dicFlag["접수번호"] != "")
                {
                    SQL += "       AND a.JEPNO='" + dicFlag["접수번호"] + "'                                               " + ComNum.VBLF;
                }

                SQL += "       AND a.WRTNO=b.WRTNO(+)                                                                    " + ComNum.VBLF;
                SQL += "       AND a.WRTNO=c.WRTNO(+)                                                                        " + ComNum.VBLF;
                SQL += "       AND a.JulNo=c.EdiSeq(+)                                                                        " + ComNum.VBLF;
                SQL += "       AND c.SuNext=d.SuNext(+)                                                                        " + ComNum.VBLF;
                SQL += "       ORDER BY a.DeptCode,a.DrCode,b.Pano,b.YYMM,c.Bun,c.SuNext                                       " + ComNum.VBLF;
            }
            //보험,보호,보호+외래
            else
            {
                SQL = "";
                SQL = " SELECT  a.DeptCode  " + ComNum.VBLF;
                SQL += "      , a.DrCode            " + ComNum.VBLF;
                SQL += "      , b.Pano				" + ComNum.VBLF;
                SQL += "      , b.Bi                " + ComNum.VBLF;
                SQL += "      , b.Sname          " + ComNum.VBLF;
                SQL += "      , b.ILLCode1       " + ComNum.VBLF;
                SQL += "      , b.YYMM           " + ComNum.VBLF;
                SQL += "      , a.JCode          " + ComNum.VBLF;
                SQL += "      , c.Bun            " + ComNum.VBLF;
                SQL += "      , c.SuNext         " + ComNum.VBLF;
                SQL += "      , c.Price          " + ComNum.VBLF;
                SQL += "      , c.Qty            " + ComNum.VBLF;
                SQL += "      , c.Nal            " + ComNum.VBLF;
                SQL += "      , a.JAMT           " + ComNum.VBLF;
                SQL += "      , d.SuNameK        " + ComNum.VBLF;
                SQL += "      , a.YYMM           " + ComNum.VBLF;
                SQL += "      , DECODE(c.GbGisul,'1',c.Amt*(100+b.RateGasan)/100,c.Amt) Amt   " + ComNum.VBLF;
                SQL += "      , A.WRTNO         " + ComNum.VBLF;
                SQL += "      , 'SAK_SIMSADOCT' AS Tables     " + ComNum.VBLF;
                SQL += "      , a.ROWID     " + ComNum.VBLF;
                SQL += "      , a.Remark    " + ComNum.VBLF;
                SQL += "      , b.JUMIN1                                                                " + ComNum.VBLF;
                SQL += "      , b.JUMIN2                                                                " + ComNum.VBLF;
                SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) DrName  " + ComNum.VBLF;
                SQL += "        FROM " + ComNum.DB_PMPA + "SAK_SIMSADOCT a      " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "MIR_INSID b    " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "MIR_INSDTL c   " + ComNum.VBLF;
                SQL += "      ,      " + ComNum.DB_PMPA + "BAS_SUN d     " + ComNum.VBLF;
                if (dicFlag["작업구분"] == "진료월")
                {
                    SQL += "        WHERE a.YYMM >=  '" + dicFlag["진료시작기간"] + "'    " + ComNum.VBLF;
                    SQL += "        AND a.YYMM <=  '" + dicFlag["진료종료기간"] + "'      " + ComNum.VBLF;
                }
                else if (dicFlag["작업구분"] == "통보월")
                {
                    SQL += "        WHERE a.TDate >=  TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD') " + ComNum.VBLF;
                    SQL += "          AND a.TDate <=  TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD') " + ComNum.VBLF;
                }
                else if (dicFlag["작업구분"] == "EDI접수월")
                {
                    SQL += "   ,  ( SELECT YYMM, JEPNO FROM KOSMOS_PMPA.EDI_JEPSU    " + ComNum.VBLF;
                    SQL += "          WHERE JEPDate >= TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')  " + ComNum.VBLF;
                    SQL += "            AND JEPDate <= TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')   " + ComNum.VBLF;
                    SQL += "        GROUP BY YYMM, JEPNO          " + ComNum.VBLF;
                    SQL += "       ) E                            " + ComNum.VBLF;
                    SQL += "       WHERE A.YYMM = E.YYMM          " + ComNum.VBLF;
                    SQL += "        AND A.JEPNO = E.JEPNO         " + ComNum.VBLF;
                }

                if (dicFlag["보험구분"] == "보험")
                {
                    SQL += "        AND a.Johap = '1'   " + ComNum.VBLF;
                }
                if (dicFlag["보험구분"] == "보호")
                {
                    SQL += "        AND a.Johap = '5'   " + ComNum.VBLF;
                }
                if (dicFlag["보험구분"] == "보험+보호")
                {
                    SQL += "        AND a.Johap <= '5'  " + ComNum.VBLF;
                }

                if (dicFlag["입원구분"] == "외래")
                {
                    SQL += "       AND a.IpdOpd='O'   " + ComNum.VBLF;
                }
                if (dicFlag["입원구분"] == "입원")
                {
                    SQL += "       AND a.IpdOpd='I'  " + ComNum.VBLF;
                }
                if (dicFlag["진료과코드"] != "**")
                {
                    SQL += "       AND a.DeptCode='" + dicFlag["진료과코드"] + "'   " + ComNum.VBLF;
                }
                if (dicFlag["의사코드"] != "****")
                {
                    SQL += "       AND a.DrCode='" + dicFlag["의사코드"] + "'   " + ComNum.VBLF;
                }
                if (dicFlag["EDI항코드"] != "**")
                {
                    SQL += "       AND a.Edihang='" + dicFlag["EDI항코드"] + "'  " + ComNum.VBLF;
                }
                if (dicFlag["접수번호"] != "")
                {
                    SQL += "       AND a.JEPNO='" + dicFlag["접수번호"] + "'    " + ComNum.VBLF;
                }

                SQL += "       AND a.WRTNO=b.WRTNO(+)" + ComNum.VBLF;
                SQL += "       AND a.WRTNO=c.WRTNO(+)" + ComNum.VBLF;
                SQL += "       AND a.JulNo=c.EdiSeq(+)" + ComNum.VBLF;
                SQL += "       AND c.SuNext=d.SuNext(+)" + ComNum.VBLF;

                if (dicFlag["입원구분"] == "외래")
                {
                    SQL += "  UNION ALL" + ComNum.VBLF;
                    SQL += "     SELECT DISTINCT a.DeptCode" + ComNum.VBLF;
                    SQL += "      , a.DrCode" + ComNum.VBLF;
                    SQL += "      , b.Pano" + ComNum.VBLF;
                    SQL += "      , b.Bi" + ComNum.VBLF;
                    SQL += "      , b.Sname" + ComNum.VBLF;
                    SQL += "      , b.ILLCode1" + ComNum.VBLF;
                    SQL += "      , b.YYMM" + ComNum.VBLF;
                    SQL += "      , a.JCode" + ComNum.VBLF;
                    SQL += "      , '99' BUN" + ComNum.VBLF;
                    SQL += "      , c.SuNext" + ComNum.VBLF;
                    SQL += "      , a.BAMT Price" + ComNum.VBLF;
                    SQL += "      , a.Qty" + ComNum.VBLF;
                    SQL += "      , a.Nal" + ComNum.VBLF;
                    SQL += "      , a.JAMT" + ComNum.VBLF;
                    SQL += "      , c.SuNameK" + ComNum.VBLF;
                    SQL += "      , a.YYMM" + ComNum.VBLF;
                    SQL += "      , 0 BAmt" + ComNum.VBLF;
                    SQL += "      , a.WRTNO" + ComNum.VBLF;
                    SQL += "      , 'SAK_SIMSADOCT_OUT' AS Tables  " + ComNum.VBLF;
                    SQL += "      , a.ROWID" + ComNum.VBLF;
                    SQL += "      , a.Remark" + ComNum.VBLF;
                    SQL += "      , b.JUMIN1" + ComNum.VBLF;
                    SQL += "      , b.JUMIN2" + ComNum.VBLF;
                    SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) DrName" + ComNum.VBLF;
                    SQL += "        FROM " + ComNum.DB_PMPA + "SAK_SIMSADOCT_OUT a  " + ComNum.VBLF;
                    SQL += "      ,      " + ComNum.DB_PMPA + "MIR_INSID b         " + ComNum.VBLF;
                    SQL += "      ,      " + ComNum.DB_PMPA + "BAS_SUN c           " + ComNum.VBLF;
                    SQL += "        WHERE A.WRTNO = B.WRTNO             " + ComNum.VBLF;

                    if (dicFlag["작업구분"] == "진료월")
                    {
                        SQL += "        AND a.YYMM >=  '" + dicFlag["진료시작기간"] + "' " + ComNum.VBLF;
                        SQL += "        AND a.YYMM <=  '" + dicFlag["진료종료기간"] + "'    " + ComNum.VBLF;
                    }
                    //통보월 EDI접수월
                    else
                    {
                        SQL += "        AND a.TDate >=  TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')   " + ComNum.VBLF;
                        SQL += "        AND a.TDate <=  TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')  " + ComNum.VBLF;
                    }

                    if (dicFlag["진료과코드"] != "**")
                    {
                        SQL += "       AND a.DeptCode='" + dicFlag["진료과코드"] + "'   " + ComNum.VBLF;
                    }
                    if (dicFlag["의사코드"] != "****")
                    {
                        SQL += "       AND a.DrCode='" + dicFlag["의사코드"] + "'   " + ComNum.VBLF;
                    }
                    if (dicFlag["접수번호"] != "")
                    {
                        SQL += "       AND a.JEPNO='" + dicFlag["접수번호"] + "' " + ComNum.VBLF;
                    }

                    SQL += "       AND A.SUNEXT = C.SUNEXT     " + ComNum.VBLF;
                    SQL += "       ORDER BY 1,2,3,6,4,9,10      " + ComNum.VBLF;
                }
            }

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable GetSuNameK_SimSADoctOut(PsmhDb pDbCon, DataTable dt, Dictionary<string, string> dicFlag, string strDrCode, string strDeptCode, string strWrtno)
        {
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL = " SELECT DISTINCT B.SUNAMEK                                                      " + ComNum.VBLF;
            SQL += "      FROM SAK_SIMSADOCT_OUT A                                                " + ComNum.VBLF;
            SQL += "      , BAS_SUN B												                   " + ComNum.VBLF;
            if (dicFlag["작업구분"] == "진료월")
            {
                SQL += "        WHERE a.YYMM >=  '" + dicFlag["진료시작기간"] + "'                                           " + ComNum.VBLF;
                SQL += "        AND a.YYMM <=  '" + dicFlag["진료종료기간"] + "'                                             " + ComNum.VBLF;
            }
            else
            {
                SQL += "        WHERE a.TDate >=  TO_DATE('" + dicFlag["통보시작기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
                SQL += "          AND a.TDate <=  TO_DATE('" + dicFlag["통보종료기간"] + "','YYYY-MM-DD')                    " + ComNum.VBLF;
            }
            SQL += "       AND A.DRCODE ='" + strDrCode + "'                    " + ComNum.VBLF;
            SQL += "       AND A.DeptCode='" + strDeptCode + "'                    " + ComNum.VBLF;
            SQL += "       AND A.WRTNO = '" + strWrtno + "'                    " + ComNum.VBLF;
            SQL += "       AND A.SUNEXT =B.SUNEXT                    " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            return dt;
        }

        public string up_SimsaDoctRemark(PsmhDb pDbCon, string strTable, string strROWID, string strRemark, ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "   UPDATE " + ComNum.DB_PMPA + strTable + "    \r\n";
            SQL += "     SET  REMARK   = '" + strRemark +  "'     \r\n";
            SQL += "     WHERE ROWID = '" + strROWID + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
    }
}
