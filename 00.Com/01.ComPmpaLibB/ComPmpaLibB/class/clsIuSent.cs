using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Data;

namespace ComPmpaLibB
{
    public class clsIuSent
    {
        //string SQL = "";
        //string SqlErr = ""; //에러문 받는 변수

        public DataTable sel_ViewSuga_Read(PsmhDb pDbCon, string argCode, string argSugbA)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBN,SUCODE,SUNEXT,BUN,NU,SUGBA,SUGBB,SUGBD,SUGBF,SUGBG,SUGBH,                         ";
                SQL += ComNum.VBLF + "        SUGBI,SUGBJ,SUGBK,SUGBL,SUGBM,SUGBN,SUGBO,SUGBP,SUGBQ,SUGBR,SUGBS,SUGBT,SUGBU,SUGBV,  ";
                SQL += ComNum.VBLF + "        SUGBW,SUGBX,SUGBSS,SUGBBI,SUQTY,IAMT,TAMT,BAMT,OLDIAMT,OLDTAMT,                       ";
                SQL += ComNum.VBLF + "        OLDBAMT,DAYMAX,TOTMAX,SUNAMEK,SUNAMEE,SUNAMEG,UNIT,DAICODE,HCODE,                     ";
                SQL += ComNum.VBLF + "        DECODE(SUGBC,'','0',SUGBC)   SUGBC,  DECODE(SUGBE,'0','',' ','',SUGBE)   SUGBE,       ";
                SQL += ComNum.VBLF + "        DECODE(SUGBY,'','0',SUGBY)   SUGBY,  DECODE(SUGBZ,'','0',SUGBZ)   SUGBZ,              ";
                SQL += ComNum.VBLF + "        DECODE(SUGBAA,'','0',SUGBAA) SUGBAA, DECODE(SUGBAB,'','0',SUGBAB) SUGBAB,             ";
                SQL += ComNum.VBLF + "        DECODE(SUGBAC,'','0',SUGBAC) SUGBAC, DECODE(SUGBAD,'','0',SUGBAD) SUGBAD,             ";
                SQL += ComNum.VBLF + "        TO_CHAR(SUDATE, 'YYYY-MM-DD') SUDATE,                                                 ";
                SQL += ComNum.VBLF + "        TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,                                                ";
                SQL += ComNum.VBLF + "        TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,                                                ";
                SQL += ComNum.VBLF + "        TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,                                                ";
                SQL += ComNum.VBLF + "        TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,                                                ";
                SQL += ComNum.VBLF + "        BAMT3,TAMT3,IAMT3,BAMT4,TAMT4,IAMT4,BAMT5,TAMT5,IAMT5,WONCODE,                        ";
                SQL += ComNum.VBLF + "        WONAMT,GBWON1,GBWON2,GBWON3,EDIJONG,BCODE,SUHAM,EDIDATE,OLDJONG,OLDBCODE,OLDGESU,     ";
                SQL += ComNum.VBLF + "        EDIDATE3,EDIJONG3,BCODE3,GESU3,EDIDATE4,EDIJONG4,BCODE4,GESU4,EDIDATE5,EDIJONG5,      ";
                SQL += ComNum.VBLF + "        BCODE5,GESU5,NURCODE,GBYEYAK,ANTICLASS,GBANTI,GBGOJI,GBGANJANG,GBRARE,GBBONE,         ";
                SQL += ComNum.VBLF + "        GBANTICAN,GBPPI,GBDEMENTIA,GBDIA,GBDRUG,GBOCSF,GBWONF,GBGABA,GBDRUGNO,GBNS,GBSELECT,  ";
                SQL += ComNum.VBLF + "        GBOCSDRUG,DTLBUN,DRGCODE,DRG100,DRGF,DRGADD,DRGOPEN,DRGOGADD,GBOPROOM,GBTAX,DECODE(SUGBAE,'','0',SUGBAE) SUGBAE,DECODE(SUGBAG,'','0',SUGBAG) SUGBAG,   ";
                SQL += ComNum.VBLF + "        GBTB,GBTAHPSUGA,TROWID,NROWID                                                                    ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VIEW_SUGA_CODE                                                  ";
                SQL += ComNum.VBLF + "  WHERE Sucode = '" + argCode + "'                                                            ";
                if (string.Compare(argSugbA, "1") > 0)
                {
                    SQL += ComNum.VBLF + "   AND GBN = 'H' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "   AND GBN = 'T' ";
                }
                SQL += ComNum.VBLF + "   AND DELDATE IS NULL ";
                SQL += ComNum.VBLF + "   AND (BAMT <> 0 or OLDBAMT <> 0 or sugbg='6' ) order by bamt desc";
                //SQL += ComNum.VBLF + "   AND IAMT <> 0 ";
                //SQL += ComNum.VBLF + "   AND TAMT <> 0 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable sel_Suga_Read_SugbA(PsmhDb pDbCon, string argCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Sucode, BUN, SUGBA, TO_CHAR(DelDate,'YYYY-MM-DD') DelDate  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUT t,                      ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN n                       ";
                SQL += ComNum.VBLF + "  WHERE t.Sucode = '" + argCode + "'                          ";
                SQL += ComNum.VBLF + "    AND T.SuNext = n.SuNext(+)                                ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }
        
        /// <summary>
        /// 재원심사 Slip 조회 개별식 Query (수가를 낱개로 모두 풀어서 보여줌)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="nTrsNo"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="strFNu"></param>
        /// <param name="strTNu"></param>
        /// <param name="strSort"></param>
        /// <param name="nSumZero"></param>
        /// <returns></returns>
        public DataTable sel_Ipd_New_Slip_Screen(PsmhDb pDbCon, long nTrsNo, string strFDate, string strTDate, string strFNu, string strTNu, string strSort, bool nSumZero, bool nER)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'yyyy-mm-dd') BDate,a.BCode                                                 ";
                SQL += ComNum.VBLF + "       ,A.Sucode,A.Bun,A.Nu,Qty,BaseAmt,GbSpc,b.SUGBS,b.SUGBP                                     ";
                SQL += ComNum.VBLF + "       ,DECODE(GbGisul,'0','',' ','',GbGisul) GBGISUL                                             ";
                //SQL += ComNum.VBLF + "       ,DECODE(GbSelf,'0','',' ','',GBSELF) GBSELF,GbChild,DeptCode                               ";
                SQL += ComNum.VBLF + "       ,GBSELF                                                                                    ";
                SQL += ComNum.VBLF + "       ,GbChild,DeptCode                                                                          ";
                SQL += ComNum.VBLF + "       ,DECODE(GbNgt,'0','',' ','',GBNGT) GBNGT,DrCode,GbSlip                            ";
                SQL += ComNum.VBLF + "       ,GbHost,a.Sunext,ADMIN.FC_BAS_SUN_SUNAMEK(a.Sunext) SunameK                           ";
                SQL += ComNum.VBLF + "       ,b.SUGBQ,t.SUGBF                                                                ";
                SQL += ComNum.VBLF + "       ,SUM(Nal) Nal,SUM(Amt1) Amt1,SUM(Amt2) Amt2                                                ";
                SQL += ComNum.VBLF + "       ,nvl(DIV,0) DIV , T.SUGBC                                                        ";
                SQL += ComNum.VBLF + "       ,DECODE(a.GBSUGBS,'0','',' ', '',a.GBSUGBS) GBSUGBS                                        ";
                SQL += ComNum.VBLF + "       ,DECODE(A.GBER,'0','',' ','',A.GBER) AS GBER                                               ";
                SQL += ComNum.VBLF + "       ,a.GBSGADD,b.SUGBAA,nvl( a.GBSUGBAB,0) GBSUGBAB,nvl(a.GBSUGBAC,0) GBSUGBAC,nvl(a.GBSUGBAD,0) GBSUGBAD  ";
                SQL += ComNum.VBLF + "       ,DECODE(t.SUGBG,'','0',t.SUGBG) SUGBG                                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBY,'','0',b.SUGBY) SUGBY                                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBZ,'','0',b.SUGBZ) SUGBZ                                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBAB,'','0',b.SUGBAB) SUGBAB                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBAC,'','0',b.SUGBAC) SUGBAC                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBAD,'','0',b.SUGBAD) SUGBAD                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(a.OPGUBUN,'','0',a.OPGUBUN) OPGUBUN                                                ";
                SQL += ComNum.VBLF + "       ,DECODE(a.HIGHRISK,'','0',a.HIGHRISK) HIGHRISK                                             ";
                SQL += ComNum.VBLF + "       ,b.BCODE S_BCODE, a.ROWID AROWID                                                           ";
                SQL += ComNum.VBLF + "       ,TO_CHAR(a.ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE                                          ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_MED + "FC_INSA_MST_KORNAME(a.PART) PART                                     ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', a.Sunext) Color   ";
                SQL += ComNum.VBLF + "       ,nvl(a.GBNGT2,0)  GBNGT2                                                                              ";
                SQL += ComNum.VBLF + "       ,nvl(a.POWDER,0)  POWDER                                                                              ";
                SQL += ComNum.VBLF + "       ,nvl(a.asadd,0)  asadd                                                                              ";
                SQL += ComNum.VBLF + "       ,nvl(a.GBHU,0)  GBHU                                                                              ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                                                     ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b,                                                          ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT T                                                           ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO  = " + nTrsNo + "                                                                 ";
                SQL += ComNum.VBLF + "    AND a.Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                                       ";
                SQL += ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                                       ";
                SQL += ComNum.VBLF + "    AND a.Nu    >= '" + strFNu + "'                                                               ";
                SQL += ComNum.VBLF + "    AND a.Nu    <= '" + strTNu + "'                                                               ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT NOT IN ('BBBBBB')                                                                ";
                SQL += ComNum.VBLF + "    AND (b.WONCODE NOT IN ('1118') OR b.WONCODE IS NULL )                                         "; //간호행위제외
                SQL += ComNum.VBLF + "    AND a.Sucode = t.Sucode                                                                       ";
                SQL += ComNum.VBLF + "    AND t.Sunext = b.Sunext(+)                                                                    ";

              
                if (nSumZero)
                {
                    if (clsPmpaType.TIT.GBHU =="Y")

                    {//처방일 기준 합이 0인 수가는 안보여줌
                      //  SQL += ComNum.VBLF + "    AND a.GBHU ='1'                                                                           ";
                        SQL += ComNum.VBLF + "    AND (a.SUNEXT, a.BDATE) NOT IN (                                                          ";
                        SQL += ComNum.VBLF + "          SELECT SUNEXT, BDATE                                                                ";
                        SQL += ComNum.VBLF + "            From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                           ";
                        SQL += ComNum.VBLF + "           WHERE TRSNO = " + nTrsNo + "                                                       ";
                        SQL += ComNum.VBLF + "             AND a.GBHU ='1'                                                                  ";
                        SQL += ComNum.VBLF + "           GROUP By SUNEXT, BDATE                                                             ";
                        SQL += ComNum.VBLF + "          HAVING SUM(QTY*NAL) = 0      )                                                      ";
                    }
                    else
                    {
                        //처방일 기준 합이 0인 수가는 안보여줌
                        SQL += ComNum.VBLF + "    AND (a.SUNEXT, a.BDATE) NOT IN (                                                          ";
                        SQL += ComNum.VBLF + "          SELECT SUNEXT, BDATE                                                                ";
                        SQL += ComNum.VBLF + "            From " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                           ";
                        SQL += ComNum.VBLF + "           WHERE TRSNO = " + nTrsNo + "                                                       ";
                        SQL += ComNum.VBLF + "           GROUP By SUNEXT, BDATE                                                             ";
                        SQL += ComNum.VBLF + "          HAVING SUM(QTY*NAL) = 0      )                                                      ";
                    }

                }

                if (nER)
                {
                  
                    //처방일 기준 합이 0인 수가는 안보여줌
                    SQL += ComNum.VBLF + "    AND a.SUNEXT IN (                                                          ";
                    SQL += ComNum.VBLF + "          SELECT SUNEXT                                                                ";
                    SQL += ComNum.VBLF + "            From " + ComNum.DB_PMPA + "BAS_SUN                                           ";
                    SQL += ComNum.VBLF + "           WHERE SUGBAA in ('A','1','2','3')           )                                          ";
                    

                }


                SQL += ComNum.VBLF + "  GROUP BY A.BDate,a.BCode,A.Sucode,A.Bun,A.Nu,Qty,BaseAmt,GbSpc,b.SUGBS,b.SUGBP, GbGisul,        ";
                //SQL += ComNum.VBLF + "           GbGisul,DECODE(GbSelf,'0','',' ','',GBSELF),GbChild,DeptCode,                          ";
                SQL += ComNum.VBLF + "           GbSelf,                                                                                ";
                SQL += ComNum.VBLF + "           GbChild,DeptCode,                                                                      ";
                SQL += ComNum.VBLF + "           DECODE(GbNgt,'0','',' ','',GBNGT),DrCode,                                     ";
                SQL += ComNum.VBLF + "           GbSlip,GbHost,a.SuNext,ADMIN.FC_BAS_SUN_SUNAMEK(a.Sunext),                        ";
                SQL += ComNum.VBLF + "           b.SUGBQ,t.SUGBF,                                                            ";
                SQL += ComNum.VBLF + "           nvl(DIV,0), T.SUGBC,                                                         ";
                SQL += ComNum.VBLF + "           DECODE(a.GBSUGBS,'0','',' ', '',a.GBSUGBS),                                            ";
                SQL += ComNum.VBLF + "           DECODE(A.GBER,'0','',' ','',A.GBER),                                                   ";
                SQL += ComNum.VBLF + "           a.GBSGADD,b.SUGBAA,nvl( a.GBSUGBAB,0) ,nvl(a.GBSUGBAC,0) ,nvl(a.GBSUGBAD,0) ,          ";
                SQL += ComNum.VBLF + "           DECODE(t.SUGBG,'','0',t.SUGBG),                                                        ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBY,'','0',b.SUGBY),                                                        ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBZ,'','0',b.SUGBZ),                                                        ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBAB,'','0',b.SUGBAB),                                                      ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBAC,'','0',b.SUGBAC),                                                      ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBAD,'','0',b.SUGBAD),                                                      ";
                SQL += ComNum.VBLF + "           DECODE(a.OPGUBUN,'','0',a.OPGUBUN),                                                    ";
                SQL += ComNum.VBLF + "           DECODE(a.HIGHRISK,'','0',a.HIGHRISK),                                                  ";
                SQL += ComNum.VBLF + "           b.BCODE, a.ROWID,                                                                      ";
                SQL += ComNum.VBLF + "           TO_CHAR(a.ENTDATE, 'YYYY-MM-DD HH24:MI'),                                              ";
                SQL += ComNum.VBLF + "           " + ComNum.DB_MED + "FC_INSA_MST_KORNAME(a.PART),                                      ";
                SQL += ComNum.VBLF + "           " + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', a.Sunext),     ";
                SQL += ComNum.VBLF + "           nvl(a.GBNGT2,0),                                                                              ";
                SQL += ComNum.VBLF + "           nvl(a.powder,0),                                                                              ";
                SQL += ComNum.VBLF + "           nvl(a.asadd,0),                                                                              ";
                SQL += ComNum.VBLF + "           nvl(a.GBHU, 0)                                                                              ";
                
                if (strSort.Equals("1"))
                {
                    SQL += ComNum.VBLF + "  ORDER BY a.Nu,                                                                              ";
                    SQL += ComNum.VBLF + "  DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0),                                ";
                    SQL += ComNum.VBLF + "        a.Sucode,a.SuNext,a.Bdate,a.Qty,a.Bun                                                 ";
                }
                else if (strSort.Equals("2"))
                {
                    SQL += ComNum.VBLF + "  ORDER BY a.Nu, a.bdate ,                                                                    ";
                    SQL += ComNum.VBLF + "  DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0),                                ";
                    SQL += ComNum.VBLF + "        a.Sucode,a.SuNext,a.Qty,a.Bun                                                         ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  ORDER BY  A.Nu, A.Bun, A.Sucode                                                             ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// 재원심사 Slip 조회 합계식 Query
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="nTrsNo"></param>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="strFNu"></param>
        /// <param name="strTNu"></param>
        /// <param name="strSort"></param>
        /// <returns></returns>
        public DataTable sel_Ipd_New_Slip_Screen_SUM(PsmhDb pDbCon, long nTrsNo, string strFDate, string strTDate, string strFNu, string strTNu, string strSort ,bool nSumZero, bool nER)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDate,'yyyy-mm-dd') BDate,a.BCode                                                 ";
                SQL += ComNum.VBLF + "       ,A.Sucode,A.Bun,A.Nu,Qty,BaseAmt,GbSpc,b.SUGBS,b.SUGBP                                     ";
                SQL += ComNum.VBLF + "       ,DECODE(GbGisul,'0','',' ','',GbGisul) GBGISUL                                             ";
                //SQL += ComNum.VBLF + "       ,DECODE(GbSelf,'0','',' ','',GBSELF) GBSELF,GbChild,DeptCode                               ";
                SQL += ComNum.VBLF + "       ,GBSELF,GbChild,DeptCode                                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(GbNgt,'0','',' ','',GBNGT) GBNGT,DrCode,GbSlip                            ";
                SQL += ComNum.VBLF + "       ,GbHost,a.Sunext,ADMIN.FC_BAS_SUN_SUNAMEK(a.Sunext) SunameK                           ";
                SQL += ComNum.VBLF + "       ,b.SUGBQ,t.SUGBF                                                                ";
                SQL += ComNum.VBLF + "       ,SUM(Nal) Nal,SUM(Amt1) Amt1,SUM(Amt2) Amt2                                                ";
                SQL += ComNum.VBLF + "       ,nvl(a.DIV,0) DIV , T.SUGBC                                                        ";
                SQL += ComNum.VBLF + "       ,DECODE(a.GBSUGBS,'0','',' ', '',a.GBSUGBS) GBSUGBS                                        ";
                SQL += ComNum.VBLF + "       ,DECODE(A.GBER,'0','',' ','',A.GBER) AS GBER                                               ";
                SQL += ComNum.VBLF + "       ,a.GBSGADD,b.SUGBAA,nvl(a.GBSUGBAB,0) GBSUGBAB,nvl(a.GBSUGBAC,0) GBSUGBAC,nvl(a.GBSUGBAD,0) GBSUGBAD                                       ";
                SQL += ComNum.VBLF + "       ,DECODE(t.SUGBG,'','0',t.SUGBG) SUGBG                                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBY,'','0',b.SUGBY) SUGBY                                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBZ,'','0',b.SUGBZ) SUGBZ                                                      ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBAB,'','0',b.SUGBAB) SUGBAB                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBAC,'','0',b.SUGBAC) SUGBAC                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(b.SUGBAD,'','0',b.SUGBAD) SUGBAD                                                   ";
                SQL += ComNum.VBLF + "       ,DECODE(a.OPGUBUN,'','0',a.OPGUBUN) OPGUBUN                                                ";
                SQL += ComNum.VBLF + "       ,DECODE(a.HIGHRISK,'','0',a.HIGHRISK) HIGHRISK                                             ";
                SQL += ComNum.VBLF + "       ,b.BCODE S_BCODE, '' AROWID, '' ENTDATE, '' PART                                           ";
                SQL += ComNum.VBLF + "       ," + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', a.Sunext) Color   ";
                SQL += ComNum.VBLF + "       ,nvl(a.GBNGT2,0) GBNGT2                                                                                  ";
                SQL += ComNum.VBLF + "       ,nvl(a.powder,0) powder                                                                                  ";
                SQL += ComNum.VBLF + "       ,nvl(a.asadd,0) asadd                                                                                  ";
                SQL += ComNum.VBLF + "       ,nvl(a.GBHU,0) GBHU                                                                                  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a,                                                     ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b,                                                          ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUT T                                                           ";
                SQL += ComNum.VBLF + "  WHERE a.TRSNO  = " + nTrsNo + "                                                                 ";
                SQL += ComNum.VBLF + "    AND a.Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                                       ";
                SQL += ComNum.VBLF + "    AND a.Bdate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                                       ";
                SQL += ComNum.VBLF + "    AND a.Nu    >= '" + strFNu + "'                                                               ";
                SQL += ComNum.VBLF + "    AND a.Nu    <= '" + strTNu + "'                                                               ";
                SQL += ComNum.VBLF + "    AND a.SUNEXT NOT IN ('BBBBBB')                                                                ";
                SQL += ComNum.VBLF + "    AND (b.WONCODE NOT IN ('1118') OR b.WONCODE IS NULL )                                         "; //간호행위제외
                SQL += ComNum.VBLF + "    AND a.Sucode = t.Sucode                                                                       ";
                if (nER)
                {

                    //처방일 기준 합이 0인 수가는 안보여줌
                    SQL += ComNum.VBLF + "    AND a.SUNEXT IN (                                                          ";
                    SQL += ComNum.VBLF + "          SELECT SUNEXT                                                                ";
                    SQL += ComNum.VBLF + "            From " + ComNum.DB_PMPA + "BAS_SUN                                           ";
                    SQL += ComNum.VBLF + "           WHERE SUGBAA in  ('A','1','2','3')           )                                             ";


                }
                SQL += ComNum.VBLF + "    AND t.Sunext = b.Sunext(+)                                                                    ";
                SQL += ComNum.VBLF + "  GROUP BY A.BDate,a.BCode,A.Sucode,A.Bun,A.Nu,Qty,BaseAmt,GbSpc,b.SUGBS,b.SUGBP,                 ";
                //SQL += ComNum.VBLF + "           GbGisul,DECODE(GbSelf,'0','',' ','',GBSELF),GbChild,DeptCode,                          ";
                SQL += ComNum.VBLF + "           GbGisul,GBSELF,GbChild,DeptCode,                                                       ";//WardCode, 
                SQL += ComNum.VBLF + "           DECODE(GbNgt,'0','',' ','',GBNGT),DrCode,                                    ";
                SQL += ComNum.VBLF + "           GbSlip,GbHost,a.SuNext,ADMIN.FC_BAS_SUN_SUNAMEK(a.Sunext),                        ";
                SQL += ComNum.VBLF + "           b.SUGBQ,t.SUGBF,                                                            ";           //A.ROOMCODE,  
                SQL += ComNum.VBLF + "           nvl(a.DIV,0), T.SUGBC,                                                         ";
                SQL += ComNum.VBLF + "           DECODE(a.GBSUGBS,'0','',' ', '',a.GBSUGBS),                                            ";
                SQL += ComNum.VBLF + "           DECODE(A.GBER,'0','',' ','',A.GBER),                                                   ";
                SQL += ComNum.VBLF + "           a.GBSGADD,b.SUGBAA,nvl(a.GBSUGBAB,0), nvl(a.GBSUGBAC,0),nvl(a.GBSUGBAD,0) ,            ";
                SQL += ComNum.VBLF + "           DECODE(t.SUGBG,'','0',t.SUGBG),                                                        ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBY,'','0',b.SUGBY),                                                        ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBZ,'','0',b.SUGBZ),                                                        ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBAB,'','0',b.SUGBAB),                                                      ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBAC,'','0',b.SUGBAC),                                                      ";
                SQL += ComNum.VBLF + "           DECODE(b.SUGBAD,'','0',b.SUGBAD),                                                      ";
                SQL += ComNum.VBLF + "           DECODE(a.OPGUBUN,'','0',a.OPGUBUN),                                                    ";
                SQL += ComNum.VBLF + "           DECODE(a.HIGHRISK,'','0',a.HIGHRISK),                                                  ";
                SQL += ComNum.VBLF + "           b.BCODE,                                                                               ";
                SQL += ComNum.VBLF + "           " + ComNum.DB_PMPA + "FC_MIR_COLOR_SET('" + clsType.User.IdNumber + "', a.Sunext),     ";
                SQL += ComNum.VBLF + "           nvl(a.GBNGT2,0) ,                                                                             ";
                SQL += ComNum.VBLF + "           nvl(a.powder,0) ,                                                                                   ";
                SQL += ComNum.VBLF + "           nvl(a.asadd,0) ,                                                                                   ";
                SQL += ComNum.VBLF + "           nvl(a.GBHU,0)                                                                                   ";
                // SQL += ComNum.VBLF + "   HAVING (SUM(NAL) <> 0 OR SUM(Amt1) <> 0 OR SUM(Amt2) <> 0)                                     ";

              

                if (nSumZero)
                {
                    if (clsPmpaType.TIT.GBHU == "Y")

                    {//처방일 기준 합이 0인 수가는 안보여줌
                        SQL += ComNum.VBLF + "   HAVING  SUM(Amt1) <> 0                                      ";
                    }
                }
                else
                {
                    SQL += ComNum.VBLF + "   HAVING (SUM(NAL) <> 0 OR SUM(Amt1) <> 0 OR SUM(Amt2) <> 0)                                     ";
                }
                    if (strSort.Equals("1"))
                {
                    SQL += ComNum.VBLF + "  ORDER BY a.Nu,                                                                              ";
                    SQL += ComNum.VBLF + "  DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0),                                ";
                    SQL += ComNum.VBLF + "        a.Sucode,a.SuNext,a.Bdate,a.Qty,a.Bun                                                 ";
                }
                else if (strSort.Equals("2"))
                {
                    SQL += ComNum.VBLF + "  ORDER BY a.Nu, a.bdate ,                                                                    ";
                    SQL += ComNum.VBLF + "  DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0),                                ";
                    SQL += ComNum.VBLF + "        a.Sucode,a.SuNext,a.Qty,a.Bun                                                         ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  ORDER BY  A.Nu, A.Bun, A.Sucode                                                             ";
                }

               
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable sel_Ipd_Simsa_History(PsmhDb pDbCon, string strPano, long nTrsNo)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT IPDNO,TRSNO,PANO,BI,SNAME,FLAG,               ";
                SQL += ComNum.VBLF + "        ADMIN.FC_IPD_GBSTS_NM(GBSTS) GBSTS,    ";
                SQL += ComNum.VBLF + "        TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE,        ";
                SQL += ComNum.VBLF + "        TO_CHAR(INDATE, 'YYYY/MM/DD') INDATE,         ";
                SQL += ComNum.VBLF + "        TO_CHAR(SIMSA_OK,'MM/DD HH24:MI') SIMSA_OK,   ";
                SQL += ComNum.VBLF + "        TO_CHAR(SIMSA_NO,'MM/DD HH24:MI') SIMSA_NO,   ";
                SQL += ComNum.VBLF + "        SIMSA_SNAME , SIMSA_SABUN                     ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS           ";
                SQL += ComNum.VBLF + "  WHERE PANO  = '" + strPano + "'                     ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + nTrsNo + "                        ";
                SQL += ComNum.VBLF + "  ORDER BY EntDate DESC                               ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable sel_Ipd_New_Slip(PsmhDb pDbCon, string ArgRowid)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP     ";
                SQL += ComNum.VBLF + "  WHERE ROWID  = '" + ArgRowid + "'                   ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable sel_IOrder_AccCheck(PsmhDb pDbCon, string ArgRowid, string ArgPano, string ArgDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,A.OrderCode,A.SuCode,A.SlipNo,          \r\n";
                SQL += "        A.RealQty,a.Qty,a.Nal,a.GbInfo,A.DosCode, a.RoomCode,a.Gbioe,a.Ptno,        \r\n";
                SQL += "        a.PickupSabun,A.DeptCode,A.Staffid DrCode,a.DrCode2,A.OrderNo,A.GbSelf,     \r\n";
                SQL += "        A.GbTFlag,A.GbNgt,A.GbNgt2,a.Gbioe,A.GBSTATUS, a.Gbdiv,  A.ORDERSITE,       \r\n";
                SQL += "        A.CONSULT ,B.SugbB,B.SugbC,B.SugbD,B.SugbE,B.SugbG,B.Bun,B.Nu,B.TotMax,     \r\n";
                SQL += "        B.SugbI,B.SugbF, A.BI, A.HIGHRISK, A.ER24, A.GSADD, A.BURNADD, A.OPGUBUN,   \r\n";
                SQL += "        C.SugbX,C.SugbY,C.SugbZ,C.SugbAA,C.SugbAB,C.SugbAC,C.SugbAD, C.GBNS         \r\n";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_IORDER A,                                          \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_SUT B,                                            \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_SUN C                                             \r\n";
                if (ArgRowid != "")

                {
                    SQL += " WHERE A.ROWID = '" + ArgRowid + "'                                             \r\n";
                    SQL += "   AND ((a.ordersite ='ERO'  ) OR A.GbStatus  IN ('D-',' ','D+'))               \r\n";
                    SQL += "   AND A.slipNo  != '0102'                                                      \r\n";
                    //2021-03-27 0101전문물리치료도 수가 발생안되게 요청  
                    //SQL += "   AND A.slipNo  not in ( '0102'  ,'0101')                                      \r\n";
                    SQL += "   AND ( A.GbPRN IS NULL OR A.GbPRN = ' '  )                                    \r\n";
                }
                else
                {
                    SQL += " WHERE A.Ptno      =  '" + ArgPano + "'                                         \r\n";
                    SQL += "  AND A.BDate     =  TO_DATE('" + ArgDate + "','YYYY-MM-DD')                    \r\n";
                    SQL += "  AND A.GBSEND = ' '                                                            \r\n";
                    SQL += "  AND (A.ACCSEND IS NULL OR A.ACCSEND = '')                                     \r\n";
                    SQL += "  AND (a.ordersite ='ERO'   OR   A.GbStatus  IN ('D-',' ','D+'))                \r\n";
                    SQL += "  AND A.slipNo  != '0102'                                                       \r\n";
                    //SQL += "   AND A.slipNo  not in ( '0102'  ,'0101')                                      \r\n";
                    SQL += "  AND ( A.GbPRN IS NULL OR A.GbPRN = ' '  )                                     \r\n";
                }
                SQL += "  AND A.SuCode =  B.SuCode                                                          \r\n";
                SQL += "  AND B.SuNext =  C.SuNext                                                          \r\n";
                //혈액오더는 혈액은행에서 출고시 IPD_SLIP에 전송함(2005-05-06) '
                //2014-01-16 수혈금액발생안함 밑에 루틴확인해야함
                SQL += "  AND b.Bun <> '37'                                                                 \r\n"; //수혈 - 시행시 주석풀어야함 - 금액계산안되게 됨
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        public string sel_Read_V_illCode_Chk(PsmhDb pDbCon, string ArgCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID,GbVCode,GbVCode1,GbVCode2 FROM " + ComNum.DB_PMPA + "BAS_ILLS     ";
                SQL += ComNum.VBLF + "  WHERE ILLCODE = '" + ArgCode + "'                   ";
              //  SQL += ComNum.VBLF + "    AND GbVCode = '*'                                 ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["GbVCode1"].ToString().Trim() == "*")
                    {
                        rtnVal = "@(희귀)";
                    }
                    else if (dt.Rows[0]["GbVCode2"].ToString().Trim() == "*")
                    {
                        rtnVal = "@(난치)";
                    }
                    
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string sel_Read_SimDate(PsmhDb pDbCon, long ArgTrsno)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MAX(TO_CHAR(SIMSA_OK,'YYYY-MM-DD HH24:MI')) SIMDATE   ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS                   ";
                SQL += ComNum.VBLF + "  WHERE ENTDATE >= TRUNC(SYSDATE-1)                           ";
                SQL += ComNum.VBLF + "    AND SIMSA_OK IS NOT NULL                                  ";
                SQL += ComNum.VBLF + "    AND TRSNO = " + ArgTrsno + "                              ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SIMDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }
    }
}
