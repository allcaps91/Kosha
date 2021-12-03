using ComBase; //기본 클래스
using ComBase.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComEmrBase
{
    /// <summary>
    /// 서식지에 환자정보 연동관련 쿼리
    /// </summary>
    public class FormPatInfoQuery
    {
        /// <summary>
        /// MO 기록지 오더이름 삭제
        /// </summary>
        /// <param name="pAcp">EMR 환자정보</param>
        public static List<Dictionary<string, object>> Query_MO_ORDER_NAME_LIST(EmrPatient pAcp)
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("SELECT ORDERNAME                                  ");
            mParameter.AppendSql("  ,    SEQNO                                      ");
            mParameter.AppendSql("  FROM ADMIN.EMR_ORDERNAME_MAAPING           ");
            mParameter.AppendSql("WHERE PTNO      =  :PTNO                          ");
            mParameter.AppendSql("  AND MEDFRDATE =  :MEDFRDATE                     ");

            mParameter.Add("PTNO", pAcp.ptNo);
            mParameter.Add("MEDFRDATE", pAcp.medFrDate);

            return clsDB.ExecuteReader(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// MO 기록지 오더이름 삭제
        /// </summary>
        /// <param name="pAcp">EMR 환자정보</param>
        public static int Query_MO_ORDER_NAME_DEL(EmrPatient pAcp)
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("DELETE ADMIN.EMR_ORDERNAME_MAAPING                       ");
            mParameter.AppendSql("WHERE PTNO      =  :PTNO                                      ");
            mParameter.AppendSql("  AND MEDFRDATE =  :MEDFRDATE                                 ");
            mParameter.AppendSql("  AND NOT EXISTS                                              ");
            mParameter.AppendSql("  (                                                           ");
            mParameter.AppendSql("      SELECT 1                                                ");
            mParameter.AppendSql("        FROM ADMIN.AEMRCHARTMST                          ");
            mParameter.AppendSql("      WHERE PTNO      =  :PTNO                                ");
            mParameter.AppendSql("        AND MEDFRDATE =  :MEDFRDATE                           ");
            mParameter.AppendSql("        AND FORMNO    = 3552                                  ");
            mParameter.AppendSql("  )                                                           ");

            mParameter.Add("PTNO", pAcp.ptNo);
            mParameter.Add("MEDFRDATE", pAcp.medFrDate);

            return clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// MO 기록지 오더이름 저장
        /// </summary>
        /// <param name="pAcp">EMR 환자정보</param>
        public static int Query_MO_ORDER_NAME_SAVE(EmrPatient pAcp, string ORDERNAME, int SEQNO)
        {
            MParameter mParameter = new MParameter();
            mParameter.AppendSql("INSERT INTO ADMIN.EMR_ORDERNAME_MAAPING      ");
            mParameter.AppendSql("(                                                 ");
            mParameter.AppendSql("      PTNO                                        ");
            mParameter.AppendSql("  ,   MEDFRDATE                                   ");
            mParameter.AppendSql("  ,   ORDERNAME                                   ");
            mParameter.AppendSql("  ,   SEQNO                                       ");
            mParameter.AppendSql(")                                                 ");

            mParameter.AppendSql("VALUES                                            ");
            mParameter.AppendSql("(                                                 ");
            mParameter.AppendSql("      :PTNO                                       ");
            mParameter.AppendSql("  ,   :MEDFRDATE                                  ");
            mParameter.AppendSql("  ,   :ORDERNAME                                  ");
            mParameter.AppendSql("  ,   :SEQNO                                      ");
            mParameter.AppendSql(")                                                 ");

            mParameter.Add("PTNO",        pAcp.ptNo);
            mParameter.Add("MEDFRDATE",   pAcp.medFrDate);
            mParameter.Add("ORDERNAME",   ORDERNAME);
            mParameter.Add("SEQNO", SEQNO, OracleDbType.Int32);

            return clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon);
        }

        /// <summary>
        /// ENDO 당일 검사 가져오기
        /// </summary>
        /// <param name="Itemcd"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_ENDO_NAME(string PTNO)
        {
            StringBuilder SQL = new StringBuilder();
            #region 쿼리
            SQL.AppendLine("SELECT LISTAGG(B.ORDERNAME,', ') WITHIN GROUP(ORDER BY A.SEQNO) AS ORDERNAME    ");
            SQL.AppendLine("FROM ADMIN.ENDO_JUPMST A                                                   ");
            SQL.AppendLine(" INNER JOIN ADMIN.OCS_ORDERCODE B                                          ");
            SQL.AppendLine("   ON A.ORDERCODE = B.ORDERCODE                                                 ");
            SQL.AppendLine(" WHERE A.BDATE = TRUNC(SYSDATE)                                                 ");
            SQL.AppendLine("   AND A.PTNO ='" + PTNO + "'                                                   ");
            SQL.AppendLine("   AND A.GBSUNAP <> '*'                                                         ");
            #endregion

            return SQL.ToString().Trim();
        }

        /// <summary>
        /// 아이템 코드 => 기초코드 확인
        /// </summary>
        /// <param name="Itemcd"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_ItemSugaMaaping(string FormNo, string Ward, string Itemcd, string Value, string OrderGbn = "")
        {
            StringBuilder SQL = new StringBuilder();
            #region 쿼리
            SQL.AppendLine(" SELECT 1");
            SQL.AppendLine("   FROM ADMIN.AEMRSUGAMAPPING A ");
            SQL.AppendLine("  WHERE FORMNO      =  " + FormNo);
            SQL.AppendLine("    AND ITEMCD      = '" + Itemcd   + "'");
            SQL.AppendLine("    AND ITEMVALUE   = '" + Value    + "'");
            SQL.AppendLine("    AND WARD        = '" + Ward    + "'");
            #endregion

            return SQL.ToString().Trim();
        }

        /// <summary>
        /// 환자의 주진단 Query
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_OutDrugCheck(EmrPatient AcpEmr)
        {

            StringBuilder SQL = new StringBuilder();
            #region 쿼리
            SQL.AppendLine(" SELECT R.ITEMVALUE");
            SQL.AppendLine("  FROM ADMIN.AEMRCHARTMST A ");
            SQL.AppendLine("    INNER JOIN ADMIN.AEMRCHARTROW R");
            SQL.AppendLine("       ON A.EMRNO = R.EMRNO");
            SQL.AppendLine("      AND A.EMRNOHIS = R.EMRNOHIS");
            SQL.AppendLine("      AND R.ITEMNO = 'I0000035338'");
            SQL.AppendLine(" WHERE FORMNO IN(966, 1832)");
            SQL.AppendLine("   AND MEDFRDATE = '" + AcpEmr.medFrDate + "'");
            SQL.AppendLine("   AND PTNO      = '" + AcpEmr.ptNo      + "'");
            #endregion

            return SQL.ToString().Trim();
        }

        public static string Query_FormPatInfo_PoaException(string Code)
        {
            StringBuilder SQL = new StringBuilder();
            #region 쿼리
            SQL.AppendLine(" SELECT 1 AS CNT");
            SQL.AppendLine(" FROM DUAL");
            SQL.AppendLine(" WHERE EXISTS");
            SQL.AppendLine(" (");
            SQL.AppendLine(" SELECT 1");
            SQL.AppendLine("  FROM ADMIN.AEMRBASCD ");
            SQL.AppendLine(" WHERE BSNSCLS = '의무기록실'");
            SQL.AppendLine("   AND UNITCLS  ='POA 예외코드'");
            SQL.AppendLine("   AND BASCD = '" + Code + "'");
            SQL.AppendLine(" )");
            #endregion

            return SQL.ToString().Trim();
        }

        public static string Query_FormPatInfo_IsBlood(string Pano, string Date)
        {
            StringBuilder SQL = new StringBuilder();
            #region 쿼리
            SQL.AppendLine(" SELECT 1 AS CNT");
            SQL.AppendLine(" FROM DUAL");
            SQL.AppendLine(" WHERE EXISTS");
            SQL.AppendLine(" (");
            SQL.AppendLine(" SELECT 1");
            SQL.AppendLine("  FROM ADMIN.EXAM_BLOODTRANS A");
            SQL.AppendLine("    INNER JOIN ADMIN.EXAM_BLOOD_IO B");
            SQL.AppendLine("       ON A.PANO = B.PANO");
            SQL.AppendLine("      AND A.BLOODNO = B.BLOODNO");
            SQL.AppendLine("    INNER JOIN ADMIN.EXAM_BLOODCROSSM C");
            SQL.AppendLine("       ON A.PANO = C.PANO");
            SQL.AppendLine("      AND A.BLOODNO = C.BLOODNO");
            SQL.AppendLine("      AND C.GBSTATUS <> '3'");
            SQL.AppendLine(" WHERE A.PANO = '" + Pano + "'");
            SQL.AppendLine("   AND A.GBJOB IN ('3','4')");
            SQL.AppendLine("   AND A.BDATE = TO_DATE('" + Date + "', 'YYYY-MM-DD')");
            SQL.AppendLine(" )");
            #endregion

            return SQL.ToString().Trim();
        }


        /// <summary>
        /// 소속 부서명 가져오기
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_SabunBuseName()
        {
            StringBuilder SQL = new StringBuilder();
            #region 쿼리
            SQL.Clear();
            SQL.AppendLine(" SELECT NAME");
            SQL.AppendLine("   FROM " + ComNum.DB_PMPA + "BAS_BUSE");
            SQL.AppendLine("  WHERE BUCODE = '" + clsType.User.BuseCode + "'");
            #endregion

            return SQL.ToString().Trim();
        }

        /// <summary>
        /// 수혈기록지 EMRNO => 혈액번호 + 종류 가져오기
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_BloodComponent(string EmrNo)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + " SELECT MAPPING1";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "EMR_DATA_MAPPING A";
            SQL += ComNum.VBLF + "  WHERE EMRNO = " + EmrNo;
            SQL += ComNum.VBLF + "    AND FORMNO = 1965";

            return SQL;
        }


        /// <summary>
        ///  물리치료실 재평가 => 초기 기록지 번호 가져오기
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_PTGetFormNo(string strFormName)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + " SELECT FORMNO";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRFORM A";
            SQL += ComNum.VBLF + "  WHERE GRPFORMNO = 1031";
            SQL += ComNum.VBLF + "    AND FORMNAME = '" + strFormName.Replace("재평가 기록지", "초기 평가지") + "'";
            SQL += ComNum.VBLF + "    AND ROWNUM = 1";

            return SQL;
        }

        /// <summary>
        ///  물리치료실 초기, 재평가 기록지 일자 경과 확인
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_Is_PTReCord_Day(string strBdate, string strPano, string strInOutCls, string strDeptCode)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendLine( " WITH CHARTLIST AS ");
            SQL.AppendLine( " (");
            SQL.AppendLine( "   SELECT MAX(CHARTDATE) CHARTDATE, MAX(FORMNAME) FORMNAME");
            SQL.AppendLine( "     FROM ");
            SQL.AppendLine( "     (");
            SQL.AppendLine( "       SELECT MAX(CHARTDATE) CHARTDATE, CASE WHEN INSTR(MAX(B.FORMNAME), '(신규)') = 0 THEN MAX(B.FORMNAME) || '(신규)' ELSE MAX(B.FORMNAME) END FORMNAME");
            SQL.AppendLine( "         FROM " + ComNum.DB_EMR + "EMRXMLMST A");
            SQL.AppendLine( "           INNER JOIN ADMIN.AEMRFORM B");
            SQL.AppendLine( "              ON B.GRPFORMNO = 1031");
            SQL.AppendLine( "             AND (FORMNAME LIKE '%재평가%' OR FORMNAME LIKE '%초기%')");
            SQL.AppendLine( "             AND A.FORMNO  = B.FORMNO ");
            SQL.AppendLine( "             AND B.OLDGB = '1'");
            SQL.AppendLine( "        WHERE A.PTNO = '" + strPano + "'");
            SQL.AppendLine( "          AND A.INOUTCLS   = '" + strInOutCls + "'");
            SQL.AppendLine( "          AND A.MEDDEPTCD  = '" + strDeptCode + "'");
            SQL.AppendLine( "          AND A.CHARTDATE  >= '" + DateTime.ParseExact(strBdate, "yyyyMMdd", null).AddYears(-1).ToString("yyyyMMdd") + "'");
            SQL.AppendLine( "        GROUP BY A.FORMNO, B.FORMNAME");
            SQL.AppendLine( "       UNION ALL");
            SQL.AppendLine( "       SELECT MAX(CHARTDATE) CHARTDATE, CASE WHEN INSTR(MAX(B.FORMNAME), '(신규)') = 0 THEN MAX(B.FORMNAME) || '(신규)' ELSE MAX(B.FORMNAME) END FORMNAME");
            SQL.AppendLine( "         FROM " + ComNum.DB_EMR + "AEMRCHARTMST A");
            SQL.AppendLine( "           INNER JOIN ADMIN.AEMRFORM B");
            SQL.AppendLine( "              ON B.GRPFORMNO = 1031");
            SQL.AppendLine( "             AND (FORMNAME LIKE '%재평가%' OR FORMNAME LIKE '%초기%')");
            SQL.AppendLine( "             AND A.FORMNO  = B.FORMNO ");
            SQL.AppendLine( "             AND A.UPDATENO  = B.UPDATENO ");
            SQL.AppendLine( "        WHERE A.PTNO = '" + strPano + "'");
            SQL.AppendLine( "          AND A.INOUTCLS   = '" + strInOutCls + "'");
            SQL.AppendLine( "          AND A.MEDDEPTCD  = '" + strDeptCode + "'");
            SQL.AppendLine( "          AND A.CHARTDATE  >= '" + DateTime.ParseExact(strBdate, "yyyyMMdd", null).AddYears(-1).ToString("yyyyMMdd") + "'");
            SQL.AppendLine( "        GROUP BY A.FORMNO, B.FORMNAME");
            SQL.AppendLine( "     )");
            SQL.AppendLine( "   GROUP BY REPLACE(FORMNAME, '초기 평가지', '재평가 기록지')");
            SQL.AppendLine( " )");
            SQL.AppendLine( " SELECT CHARTDATE, FORMNAME");
            SQL.AppendLine( "   FROM CHARTLIST CH");
            SQL.AppendLine( "  WHERE EXISTS ");
            SQL.AppendLine( "  ( ");
            SQL.AppendLine( "   SELECT 1                                                                         ");
            SQL.AppendLine( "     FROM ADMIN.AEMRCHARTMST A                                                ");
            SQL.AppendLine( "       INNER JOIN ADMIN.AEMRFORM B                                            ");
            SQL.AppendLine( "          ON B.GRPFORMNO = 1031                                                    ");
            SQL.AppendLine( "         AND A.FORMNO = B.FORMNO                                                   ");
            SQL.AppendLine( "         AND A.UPDATENO = B.UPDATENO                                               ");
            SQL.AppendLine( "    WHERE A.PTNO = '" + strPano + "'");
            SQL.AppendLine( "      AND A.INOUTCLS = '" + strInOutCls + "'");
            SQL.AppendLine( "      AND A.MEDDEPTCD = '" + strDeptCode + "'");
            SQL.AppendLine( "      AND A.CHARTDATE < CH.CHARTDATE                                                   ");
            SQL.AppendLine( "      AND B.FORMNAME = REPLACE(CH.FORMNAME, '초기 평가지', '재평가 기록지') ");
            SQL.AppendLine("  ) ");

            SQL.AppendLine( "  OR ");
            SQL.AppendLine( "  EXISTS ");
            SQL.AppendLine( "  ( ");
            SQL.AppendLine( "   SELECT 1                                                                         ");
            SQL.AppendLine( "     FROM DUAL                                              ");
            SQL.AppendLine( "    WHERE CH.FORMNAME LIKE '%평가%'                                            ");
            SQL.AppendLine( "      AND TRUNC(SYSDATE) - TO_DATE(CH.CHARTDATE, 'YYYYMMDD') >= 30 ");
            SQL.AppendLine( "      AND TRUNC(SYSDATE) - TO_DATE(CH.CHARTDATE, 'YYYYMMDD') <= 45  ");
            SQL.AppendLine("  ) ");

            SQL.AppendLine( "  OR ");
            SQL.AppendLine( "  EXISTS ");
            SQL.AppendLine( "  ( ");
            SQL.AppendLine( "   SELECT 1                                                                         ");
            SQL.AppendLine( "     FROM ADMIN.AEMRCHARTMST A                                                ");
            SQL.AppendLine( "        LEFT OUTER JOIN ADMIN.AEMRFORM B                                            ");
            SQL.AppendLine( "          ON B.GRPFORMNO = 1031                                                    ");
            SQL.AppendLine( "    WHERE A.PTNO = '" + strPano + "'");
            SQL.AppendLine( "      AND A.INOUTCLS = '" + strInOutCls + "'");
            SQL.AppendLine( "      AND A.MEDDEPTCD = '" + strDeptCode + "'");
            SQL.AppendLine( "      AND B.FORMNAME = REPLACE(CH.FORMNAME, '초기 평가지', '재평가 기록지') ");
            SQL.AppendLine( "      AND B.FORMNO IS NULL  ");
            SQL.AppendLine("  ) ");

            return SQL.ToString().Trim();
        }

        /// <summary>
        /// 혈액형 조회
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_BloodType(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT ABO";
            SQL = SQL + ComNum.VBLF + "	 FROM ADMIN.EXAM_BLOOD_MASTER";
            SQL = SQL + ComNum.VBLF + "	 WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "	   AND MODIFYDATE = (SELECT MAX(MODIFYDATE) FROM ADMIN.EXAM_BLOOD_MASTER WHERE PANO = '" + emrPatient.ptNo + "')";

            return SQL;
        }

        /// <summary>
        /// 완화의료 간호사 상담기록지(경과상담) - 초기 기록지에서 데이터 끌고오기
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_Set_FormPatInfo_GETHUDATE(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT EXTRACTVALUE(chartxml, '//dt2') as DATE1, EXTRACTVALUE(chartxml, '//dt3') as DATE2";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRXML";
            SQL += ComNum.VBLF + " WHERE MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2555";

            SQL += ComNum.VBLF + "UNION ALL";
            SQL += ComNum.VBLF + "SELECT DT1.ITEMVALUE AS DATE1, DT2.ITEMVALUE AS DATE2";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";

            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW DT1";
            SQL += ComNum.VBLF + "       ON A.EMRNO = DT1.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = DT1.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND DT1.ITEMCD = 'I0000036126' -- 말기진단";

            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW DT2";
            SQL += ComNum.VBLF + "       ON A.EMRNO = DT2.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = DT2.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND DT2.ITEMCD = 'I0000036140' -- 완화의료 등록";

            SQL += ComNum.VBLF + " WHERE MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2555";

            SQL += ComNum.VBLF + "UNION ALL";
            SQL += ComNum.VBLF + "SELECT DT1.ITEMVALUE AS DATE1, DT2.ITEMVALUE AS DATE2";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";

            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW DT1";
            SQL += ComNum.VBLF + "       ON A.EMRNO = DT1.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = DT1.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND DT1.ITEMCD = 'I0000036126' -- 말기진단";

            SQL += ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW DT2";
            SQL += ComNum.VBLF + "       ON A.EMRNO = DT2.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = DT2.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND DT2.ITEMCD = 'I0000036140' -- 완화의료 등록";

            SQL += ComNum.VBLF + " WHERE MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 3529";

            return SQL;
        }

        /// <summary>
        /// 중심정맥관 유지일 가져오기(내원 일자 중에서 가장 최근에 쓴것 기준)
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_CVCUseDay(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT MAX(B.ITEMVALUE) ITEMVALUE";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000037613'";
            SQL += ComNum.VBLF + "      AND B.ITEMVALUE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2643 -- 중심도관 삽입 및 교체시 Bundle 기록지";
            SQL += ComNum.VBLF + "   AND (CHARTDATE || CHARTTIME) = ";
            SQL += ComNum.VBLF + "   (";
            SQL += ComNum.VBLF + "   SELECT MAX(CHARTDATE || CHARTTIME)";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL += ComNum.VBLF + "    WHERE PTNO = A.PTNO";
            SQL += ComNum.VBLF + "      AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "      AND FORMNO = A.FORMNO";
            SQL += ComNum.VBLF + "   )";
            return SQL;
        }


        /// <summary>
        /// 유치도뇨관 가장 최근 삽입일 가져오기
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_VentilatorInsertDay(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT (MAX(REPLACE(B.ITEMVALUE, '-', '') || A.CHARTTIME))";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000037645'";
            SQL += ComNum.VBLF + "      AND B.ITEMVALUE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2640 -- 유치도뇨관 삽입 BUNDLE(신규)";
            SQL += ComNum.VBLF + "   AND (CHARTDATE || CHARTTIME) = ";
            SQL += ComNum.VBLF + "   (";
            SQL += ComNum.VBLF + "   SELECT MAX(CHARTDATE || CHARTTIME)";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL += ComNum.VBLF + "    WHERE PTNO = A.PTNO";
            SQL += ComNum.VBLF + "      AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "      AND FORMNO = A.FORMNO";
            SQL += ComNum.VBLF + "   )";
            return SQL;
        }

        /// <summary>
        /// 폐렴 Bundle 가장 최근 삽입일 가져오기
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_PneumoniaInsertDay(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT (MAX(REPLACE(B.ITEMVALUE, '-', '') || A.CHARTTIME))";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000037660'";
            SQL += ComNum.VBLF + "      AND B.ITEMVALUE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2639 -- 인공호흡기 관련 폐렴예방 bundle(신규)";
            SQL += ComNum.VBLF + "   AND (CHARTDATE || CHARTTIME) = ";
            SQL += ComNum.VBLF + "   (";
            SQL += ComNum.VBLF + "   SELECT MAX(CHARTDATE || CHARTTIME)";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL += ComNum.VBLF + "    WHERE PTNO = A.PTNO";
            SQL += ComNum.VBLF + "      AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "      AND FORMNO = A.FORMNO";
            SQL += ComNum.VBLF + "   )";
            return SQL;
        }

        /// <summary>
        /// 유치도뇨관 가장 최근 Size 가져오기
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_VentilatorInsertSize(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT B.ITEMVALUE";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000031188'";
            SQL += ComNum.VBLF + "      AND B.ITEMVALUE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2640 -- 유치도뇨관 삽입 BUNDLE(신규)";
            SQL += ComNum.VBLF + "   AND (CHARTDATE || CHARTTIME) = ";
            SQL += ComNum.VBLF + "   (";
            SQL += ComNum.VBLF + "   SELECT MAX(CHARTDATE || CHARTTIME)";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL += ComNum.VBLF + "    WHERE PTNO = A.PTNO";
            SQL += ComNum.VBLF + "      AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "      AND FORMNO = A.FORMNO";
            SQL += ComNum.VBLF + "   )";
            return SQL;
        }

        /// <summary>
        /// 폐렴 Bundle 가장 최근 Size 가져오기
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_PneumoniaInsertSize(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT B.ITEMVALUE";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000037661'";
            SQL += ComNum.VBLF + "      AND B.ITEMVALUE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2639 -- 인공호흡기 관련 폐렴예방 bundle(신규)";
            SQL += ComNum.VBLF + "   AND (CHARTDATE || CHARTTIME) = ";
            SQL += ComNum.VBLF + "   (";
            SQL += ComNum.VBLF + "   SELECT MAX(CHARTDATE || CHARTTIME)";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL += ComNum.VBLF + "    WHERE PTNO = A.PTNO";
            SQL += ComNum.VBLF + "      AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "      AND FORMNO = A.FORMNO";
            SQL += ComNum.VBLF + "   )";
            return SQL;
        }

        /// <summary>
        /// 유치도뇨관 가장 최근 제거일 가져오기
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_VentilatorDelDay(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "WITH CHART_MAX_DATE AS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "SELECT MAX(CHARTDATE || RPAD(CHARTTIME, 6, '0')) AS MAX_DATE";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000037521'";
            SQL += ComNum.VBLF + "      AND (UPPER(B.ITEMVALUE) LIKE '%REMOVE%' OR UPPER(B.ITEMVALUE) LIKE '%AT%' OR B.ITEMVALUE = '제거' OR B.ITEMVALUE ='교환' OR B.ITEMVALUE ='교체')";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 1575 -- 기본간호활동";

            #region 유지번들, 유지일에 제거,REMOVE라고 썻을경우 도 적용되게
            SQL += ComNum.VBLF + "UNION ALL";
            SQL += ComNum.VBLF + "SELECT MAX(CHARTDATE || RPAD(CHARTTIME, 6, '0')) AS MAX_DATE";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO    = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO = 'I0000037742' --유지일";
            SQL += ComNum.VBLF + "      AND (UPPER(B.ITEMVALUE) LIKE '%REMOVE%' OR B.ITEMVALUE = '제거')";
            SQL += ComNum.VBLF + " WHERE PTNO      = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2641 -- 유지관리 BUNDLE";
            #endregion
            SQL += ComNum.VBLF + ")";
            SQL += ComNum.VBLF + "SELECT MAX_DATE";
            SQL += ComNum.VBLF + "  FROM CHART_MAX_DATE";
            SQL += ComNum.VBLF + " WHERE MAX_DATE IS NOT NULL";

            return SQL;
        }

        /// <summary>
        /// 내원내역안에 사망 했으면 일시
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_DeathDate(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT TO_CHAR(DDATE, 'YYYY-MM-DD') DDATE, DTIME";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_STD_DEATH";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + emrPatient.acpNoIn;
            return SQL;
        }

        /// <summary>
        /// 내원내역안에 가장 최근 기록지
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_MaxChartDate(EmrPatient emrPatient, string strFormNo)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT MAX(CHARTDATE)";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = " + strFormNo;
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            return SQL;
        }

        /// <summary>
        /// 내원내역안에 기록지 작성했는지 확인
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_IsWrite(EmrPatient emrPatient, EmrForm emrForm)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT 1 AS CNT";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "SELECT 1";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = " + emrForm.FmFORMNO;
            SQL += ComNum.VBLF + "   AND UPDATENO = " + emrForm.FmUPDATENO;
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + ")";
            return SQL;
        }

        /// <summary>
        /// 내원내역안에 기록지 작성했는지 확인
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_IsWrite(EmrPatient emrPatient, EmrForm emrForm, string ChartDate)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT 1 AS CNT";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "SELECT 1";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = " + emrForm.FmFORMNO;
            SQL += ComNum.VBLF + "   AND UPDATENO = " + emrForm.FmUPDATENO;
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND CHARTDATE = '" + ChartDate + "'";
            SQL += ComNum.VBLF + ")";
            return SQL;
        }

        /// <summary>
        /// 해당 내원날짜의 가장 최근 RRS 점수 계산
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_RRSSCORE(EmrPatient pAcp)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT ITEMNO, B.ITEMVALUE";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "      ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND B.ITEMNO IN('I0000014815', 'I0000002018', 'I0000002009', 'I0000001811', 'I0000037778') -- Pulse(PR) 맥박수, 수축기 혈압(SBP), 호흡수(RR), BT(체온), 의식수준(Avpu Score)";
            SQL += ComNum.VBLF + "     AND B.ITEMVALUE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE A.PTNO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + "   AND A.FORMNO IN(3150, 2135, 1935, 2431, 1969)";
            SQL += ComNum.VBLF + "   AND A.MEDFRDATE = '" + pAcp.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND (CHARTDATE || RPAD(CHARTTIME, 6, '0')) = ";
            SQL += ComNum.VBLF + "       (SELECT MAX(CHARTDATE || RPAD(CHARTTIME, 6, '0'))";
            SQL += ComNum.VBLF + "          FROM ADMIN.AEMRCHARTMST AA";
            SQL += ComNum.VBLF + "            INNER JOIN ADMIN.AEMRCHARTROW B             ";
            SQL += ComNum.VBLF + "               ON AA.EMRNO = B.EMRNO                         ";
            SQL += ComNum.VBLF + "              AND AA.EMRNOHIS = B.EMRNOHIS                      ";
            SQL += ComNum.VBLF + "              AND B.ITEMCD = 'I0000002009'                     ";
            SQL += ComNum.VBLF + "              AND B.ITEMVALUE > CHR(0)                         ";
            SQL += ComNum.VBLF + "            INNER JOIN ADMIN.AEMRCHARTROW B2             ";
            SQL += ComNum.VBLF + "               ON AA.EMRNO = B2.EMRNO                         ";
            SQL += ComNum.VBLF + "              AND AA.EMRNOHIS = B2.EMRNOHIS                      ";
            SQL += ComNum.VBLF + "              AND B2.ITEMCD = 'I0000024733'   --구분            ";
            SQL += ComNum.VBLF + "              AND NVL(B2.ITEMVALUE, ' ') <> '퇴원'                     ";
            SQL += ComNum.VBLF + "         WHERE PTNO = A.PTNO ";
            SQL += ComNum.VBLF + "           AND FORMNO IN(3150, 2135, 1935, 2431, 1969)";
            SQL += ComNum.VBLF + "           AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "       )";

            return SQL;
        }

        /// <summary>
        /// 욕창 분류
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_READ_DETAIL_EVAL_BRADEN_NEW(EmrPatient pAcp)
        {
            string SQL = " SELECT * ";
            SQL += ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_EVAL ";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + pAcp.acpNoIn;

            return SQL;
        }

        /// <summary>
        /// 욕창 분류
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_NUR_BRADEN_WARNING(EmrPatient pAcp)
        {
            string SQL = " SELECT *";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_WARNING ";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + pAcp.acpNoIn;
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + pAcp.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ( ";
            SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
            SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
            SQL = SQL + ComNum.VBLF + "      )";

            return SQL;
        }

        /// <summary>
        /// 욕창 점수 관련(기본)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_DETAIL_BRADEN(EmrPatient pAcp)
        {
            string SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE A";
            SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + pAcp.acpNoIn;
            SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + pAcp.ptNo + "' ";

            //if (ArgDate2.Length > 0)
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
            //    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
            //}
            //else
            //{
            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TRUNC(SYSDATE)";
            //}


            SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
            SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
            SQL = SQL + ComNum.VBLF + "  SELECT * FROM ADMIN.NUR_BRADEN_SCALE";
            SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TRUNC(SYSDATE)";
            SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + pAcp.acpNoIn;
            SQL = SQL + ComNum.VBLF + "       AND PANO = '" + pAcp.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
            SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
            SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

            return SQL;
        }

        /// <summary>
        /// 욕창 점수 관련(소아)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_DETAIL_BRADEN2(EmrPatient pAcp)
        {
            string SQL = " SELECT TOTAL";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE_CHILD";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + pAcp.acpNoIn;
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + pAcp.ptNo + "' ";

            //if (ArgDate2.Length > 0)
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
            //    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
            //}
            //else
            //{
            SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TRUNC(SYSDATE)";
            //}


            SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

            return SQL;
        }

        /// <summary>
        /// 욕창 점수 관련(신생아)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_DETAIL_BRADEN_BABY(EmrPatient pAcp)
        {
            string SQL = " SELECT TOTAL";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE_BABY";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + pAcp.acpNoIn;
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + pAcp.ptNo + "' ";

            //if (ArgDate2.Length > 0)
            //{
            //    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
            //    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
            //}
            //else
            //{
            SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TRUNC(SYSDATE)";
            //}


            SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

            return SQL;
        }


        /// <summary>
        /// 낙상 점수 관련(기본)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_NUR_FALLMORSE_SCALE(EmrPatient pAcp)
        {
            string SQL = "  SELECT PANO, TOTAL ";
            SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLMORSE_SCALE";
            SQL += ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + " AND IPDNO = " + pAcp.acpNoIn;
            SQL += ComNum.VBLF + " AND TOTAL >= 51";
            SQL += ComNum.VBLF + "     AND ROWID = (";
            SQL += ComNum.VBLF + "   SELECT ROWID FROM (";
            SQL += ComNum.VBLF + "  SELECT * FROM ADMIN.NUR_FALLMORSE_SCALE";
            SQL += ComNum.VBLF + "  WHERE ACTDATE = TRUNC(SYSDATE)";
            SQL += ComNum.VBLF + "       AND IPDNO = " + pAcp.acpNoIn;
            SQL += ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), (ACTDATE || ACTTIME) DESC)";
            SQL += ComNum.VBLF + "  Where ROWNUM = 1)";
            SQL += ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

            return SQL;
        }

        /// <summary>
        /// 낙상 점수 관련(18세 이전)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_NUR_FALLMORSE_SCALE18(EmrPatient pAcp)
        {
            string SQL = "  SELECT PANO, TOTAL ";
            SQL += ComNum.VBLF + " FROM ADMIN.NUR_FALLHUMPDUMP_SCALE";
            SQL += ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "'";
            SQL += ComNum.VBLF + " AND IPDNO = " + pAcp.acpNoIn;
            SQL += ComNum.VBLF + " AND (TOTAL >= 12 OR AGE < 7)";
            SQL += ComNum.VBLF + "     AND ROWID = (";
            SQL += ComNum.VBLF + "   SELECT ROWID FROM (";
            SQL += ComNum.VBLF + "  SELECT * FROM ADMIN.NUR_FALLHUMPDUMP_SCALE";
            SQL += ComNum.VBLF + "  WHERE ACTDATE = TRUNC(SYSDATE)";
            SQL += ComNum.VBLF + "       AND IPDNO = " + pAcp.acpNoIn;
            SQL += ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
            SQL += ComNum.VBLF + "  Where ROWNUM = 1)";
            SQL += ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

            return SQL;
        }

        /// <summary>
        /// 낙상 점수 관련 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_NUR_FALL_WARNING(EmrPatient pAcp)
        {
            string SQL = " SELECT * ";
            SQL += ComNum.VBLF + "  FROM ADMIN.NUR_FALL_WARNING";
            SQL += ComNum.VBLF + " WHERE IPDNO = " + pAcp.acpNoIn;
            SQL += ComNum.VBLF + "   AND (WARNING1 = '1'";
            SQL += ComNum.VBLF + "                  OR WARNING2 = '1'";
            SQL += ComNum.VBLF + "                  OR WARNING3 = '1'";
            SQL += ComNum.VBLF + "                  OR WARNING4 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_01 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_02 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_03 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_04 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_05 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_06 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_07 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_08 = '1'";
            SQL += ComNum.VBLF + "                  OR DRUG_08_ETC <> '')";

            return SQL;
        }

        /// <summary>
        /// 간호사가 등록한 진단명 가져오기.
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_NUR_DIAGNOSIS(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL = " SELECT DIAGNOSIS";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.NUR_MASTER";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + emrPatient.acpNoIn;
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + emrPatient.ptNo + "'";

            return SQL;
        }

        /// <summary>
        /// PT오더 쿼리
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <param name="OrderDate">오더 날짜</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_PTOrder(EmrPatient emrPatient, string OrderDate)
        {
            string SQL = string.Empty;
            SQL = " SELECT TO_CHAR(A.ACTDATE, 'YYYY-MM-DD') ACTDATE , A.SUCODE, ORDERNAME, TO_CHAR(CTIME, 'YYYY-MM-DD HH24:MI') CTIME";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ETC_PTORDER A";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_IORDER I";
            SQL = SQL + ComNum.VBLF + "       ON I.BDATE  = A.ACTDATE";
            SQL = SQL + ComNum.VBLF + "      AND I.SUCODE = A.SUCODE";
            SQL = SQL + ComNum.VBLF + "      AND I.PTNO  = A.PANO";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.OCS_ORDERCODE B";
            SQL = SQL + ComNum.VBLF + "       ON B.ORDERCODE = I.ORDERCODE";
            SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE = TO_DATE('" + OrderDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND GBIO = 'I'";
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + " GROUP BY A.ACTDATE, CTIME, A.SUCODE, ORDERNAME";

            return SQL;
        }

        /// <summary>
        /// 해당 내원내역에 작성한 간호정보 조사지에서 해당 아이템을 가져온다(신장, 체중)
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_BodyInfo(EmrPatient emrPatient, string ItemCd)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT B.ITEMVALUE";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "      ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND B.ITEMNO  = '" + ItemCd + "'";
            SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND A.FORMNO IN(2294, 2295, 2311, 2356) --소아용, 산모용, 산모, 기본";
            SQL += ComNum.VBLF + "   AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";

            return SQL;
        }

        /// <summary>
        /// 해당 내원날짜의 가장 최근 V/S 값 가져오기
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_LastVS(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT ITEMNO, B.ITEMVALUE";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "      ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND B.ITEMNO IN('I0000001809', 'I0000008708', 'I0000001811', 'I0000002009', 'I0000002018', 'I0000001765', 'I0000014815')";
            SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND A.FORMNO IN(3150, 2135, 1935, 2431, 1969)";
            SQL += ComNum.VBLF + "   AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND (A.CHARTDATE || CHARTTIME) = ";
            SQL += ComNum.VBLF + "       (SELECT MAX(CHARTDATE ||CHARTTIME)";
            SQL += ComNum.VBLF + "          FROM ADMIN.AEMRCHARTMST ";
            SQL += ComNum.VBLF + "         WHERE PTNO = A.PTNO ";
            SQL += ComNum.VBLF + "           AND FORMNO IN(3150, 2135, 1935, 2431, 1969)";
            SQL += ComNum.VBLF + "           AND MEDFRDATE = A.MEDFRDATE";
            SQL += ComNum.VBLF + "       )";

            return SQL;
        }


        /// <summary>
        /// 해당 날짜 V/S 값 가져오기
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <param name="strDate1">조회 시작일</param>
        /// <param name="strDate2">조회 끝나는날</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_VS(EmrPatient emrPatient, string strDate1, string strDate2)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT A.BDATE, B.ITEMVALUE, D.BASNAME";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "   INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "      ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "     AND B.ITEMNO IN('I0000008708', 'I0000001811', 'I0000002009', 'I0000002018', 'I0000014815')";
            SQL += ComNum.VBLF + "   INNER JOIN  " + ComNum.DB_EMR + "AEMRBASCD D";
            SQL += ComNum.VBLF + "      ON D.BSNSCLS = '기록지관리'";
            SQL += ComNum.VBLF + "     AND D.UNITCLS = '임상관찰'";
            SQL += ComNum.VBLF + "     AND D.BASCD = B.ITEMNO";
            SQL += ComNum.VBLF + "     AND APLFRDATE > CHR(0)";
            SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND A.FORMNO = 3150";
            SQL += ComNum.VBLF + "   AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND A.CHARTDATE >= '" + strDate1.Replace("-", "") + "'";
            if (string.IsNullOrWhiteSpace(strDate2) == false)
            {
                SQL += ComNum.VBLF + "   AND A.CHARTDATE <= '" + strDate2.Replace("-", "") + "'";
            }

            return SQL;
        }


        /// <summary>
        /// 해당 내원 내역에 기록지들 중에 해당 내용이 중복되는 것이 있는지
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <param name="strPTTime">치료시간</param>
        /// <returns></returns>
        public static string Query_FormPatInfo_PT_ReCord_Write(EmrPatient emrPatient, string strPTTime)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT 1 AS CNT";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "  SELECT 1";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "     INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
            SQL += ComNum.VBLF + "        ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "       AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "       AND B.ITEMNO IN('I0000032877')";
            SQL += ComNum.VBLF + "       AND B.ITEMVALUE = '" + strPTTime + "'";
            SQL += ComNum.VBLF + "   WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "     AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "     AND A.FORMNO IN (2165, 2166, 2167, 2168, 2169)";
            SQL += ComNum.VBLF + ")";

            return SQL;
        }

        /// <summary>
        /// 해당 내원 내역에 작성한 기록지 쿼리
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_Mst(EmrPatient emrPatient, EmrForm emrForm)
        {
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "SELECT EMRNO, FORMNO, UPDATENO, CHARTDATE, CHARTTIME, CHARTUSEID, B.NAME AS USENAME";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "   INNER JOIN  " + ComNum.DB_EMR + "EMR_USERT B";
            SQL += ComNum.VBLF + "      ON A.CHARTUSEID = B.USERID";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = " + emrForm.FmFORMNO;
            SQL += ComNum.VBLF + "   AND UPDATENO = " + emrForm.FmUPDATENO;
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + " ORDER BY TRIM(CHARTDATE || CHARTTIME) DESC";
            return SQL;
        }

        /// <summary>
        /// Admission Note [HU]의 주진단, 현병력을 가져온다.
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_2751(EmrPatient emrPatient)
        {
            string SQL = " SELECT B.ITEMTYPE, B.ITEMNO, B.ITEMCD, B.ITEMVALUE";
            SQL += ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN ADMIN.AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO IN ('I0000011878', 'I0000014826') -- 주진단, 현병력 ";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2751";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";

            return SQL;
        }

        /// <summary>
        /// 환자 보험유형 조회
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_BI(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            
            if (emrPatient.inOutCls.Equals("O"))
            {
                SQL = " SELECT Bi FROM ADMIN.OPD_MASTER";
                SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo;
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + emrPatient.medDeptCd + "'";
                SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }
            else
            {
                SQL = " SELECT Bi FROM ADMIN.IPD_NEW_MASTER";
                SQL += ComNum.VBLF + " WHERE PANO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + emrPatient.medDeptCd + "'";
                SQL += ComNum.VBLF + "   AND INDATE >=  TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd 00:00") + "', 'YYYY-MM-DD HH24:MI')";
                SQL += ComNum.VBLF + "   AND INDATE <=  TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd 23:59") + "', 'YYYY-MM-DD HH24:MI')";
            }


            return SQL;
        }


        /// <summary>
        /// 검사결과 + 혈액형 조회
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_LabResult2(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT  LTRIM(M.ExamName) AS EXAMNAME, R.Result, TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') as RESULTDATE ";
            SQL = SQL + ComNum.VBLF + "	 FROM ADMIN.Exam_ResultC R";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.Exam_Master M";
            SQL = SQL + ComNum.VBLF + "		  ON R.SubCode = M.MasterCode";
            SQL = SQL + ComNum.VBLF + "		  AND TRIM(M.EXAMNAME) IN('Hb', 'Hct', 'Na', 'K', 'GOT', 'GPT', 'PT', 'aPTT') ";
            SQL = SQL + ComNum.VBLF + "	WHERE SpecNo IN (SELECT SpecNo";
            SQL = SQL + ComNum.VBLF + "				       FROM ADMIN.Exam_Specmst";
            SQL = SQL + ComNum.VBLF + "				      WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "				        AND BDATE = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "				        AND STATUS = '05'";
            SQL = SQL + ComNum.VBLF + "				    )";
            SQL = SQL + ComNum.VBLF + "	  AND REFER > CHR(0)";
            SQL = SQL + ComNum.VBLF + "	  AND EXAMNAME > CHR(0)";
            SQL = SQL + ComNum.VBLF + "   AND R.RESULTDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";

            if (emrPatient.medEndDate != "")
            {
                SQL = SQL + ComNum.VBLF + "   AND R.RESULTDATE <= TO_DATE('" + DateTime.ParseExact(emrPatient.medEndDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }

            SQL = SQL + ComNum.VBLF + "UNION ALL";
            SQL = SQL + ComNum.VBLF + "SELECT 'Blood Type', ABO, ''";
            SQL = SQL + ComNum.VBLF + "	 FROM ADMIN.EXAM_BLOOD_MASTER";
            SQL = SQL + ComNum.VBLF + "	 WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "	   AND MODIFYDATE = (SELECT MAX(MODIFYDATE) FROM ADMIN.EXAM_BLOOD_MASTER WHERE PANO = '" + emrPatient.ptNo + "')";

            return SQL;
        }

        /// <summary>
        /// 간호정보조사지(신규) 기록지에서 알러지, 가족력,과거병력, 최근투약
        /// 정보를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_New2311Value(EmrPatient emrPatient)
        {
            string SQL = " SELECT B.ITEMTYPE, B.ITEMNO, B.ITEMCD, B.ITEMVALUE";
            SQL += ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A";
            SQL += ComNum.VBLF + "    INNER JOIN ADMIN.AEMRCHARTROW B";
            SQL += ComNum.VBLF + "       ON A.EMRNO = B.EMRNO";
            SQL += ComNum.VBLF + "      AND A.EMRNOHIS = B.EMRNOHIS";
            SQL += ComNum.VBLF + "      AND B.ITEMNO IN (";
            SQL += ComNum.VBLF + "       'I0000000002', 'I0000000418', -- 신장, 체중 ";
            SQL += ComNum.VBLF + "       'I0000000979', 'I0000000457', 'I0000000347', -- 결혼상태, 직업, 종교 ";
            SQL += ComNum.VBLF + "       'I0000034274', 'I0000034276', -- 알러지 ";
            SQL += ComNum.VBLF + "       'I0000034279', 'I0000035257', ";
            SQL += ComNum.VBLF + "       'I0000035258', 'I0000034277', ";
            SQL += ComNum.VBLF + "       'I0000034784', 'I0000034785', 'I0000034786', --가족병력";
            SQL += ComNum.VBLF + "       'I0000035304', 'I0000035274', 'I0000035275', 'I0000035276', 'I0000035277', 'I0000035278', 'I0000035279', 'I0000035280',";
            SQL += ComNum.VBLF + "       'I0000034374', 'I0000034375', 'I0000034376', --과거병력";
            SQL += ComNum.VBLF + "       'I0000034768', 'I0000034769', 'I0000034770', 'I0000034771', 'I0000034772', 'I0000034773', 'I0000034774',";
            SQL += ComNum.VBLF + "       'I0000034383',";
            SQL += ComNum.VBLF + "       'I0000034777', 'I0000034778', 'I0000034776', --최근투약";
            SQL += ComNum.VBLF + "       'I0000034279', 'I0000035257', 'I0000035258', 'I0000034277', --알레르기";
            SQL += ComNum.VBLF + "       'I0000033574'";
            SQL += ComNum.VBLF + "       )";
            SQL += ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "   AND FORMNO = 2311";
            SQL += ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";

            return SQL;
        }

        /// <summary>
        /// ER경유 입원환자 점검
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_ER_IPWON(EmrPatient emrPatient)
        {
            string SQL = " SELECT PANO";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.IPD_NEW_MASTER";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND TRUNC(INDATE) = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "  AND AMSET7 IN ('3','4','5')";

            return SQL;
        }

        /// <summary>
        /// ER내원 후 병동 입원환자 출발시간 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_ER_TRANS_TIME(EmrPatient emrPatient)
        {
            string SQL = " SELECT TO_CHAR(FRDATE, 'YYYY-MM-DD HH24:MI') FRDATE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_ER_CHECKLIST_TRANS";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND FRDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + " 00:00', 'YYYY-MM-DD HH24:MI')";
            SQL = SQL + ComNum.VBLF + "ORDER BY FRDATE asc";

            return SQL;
        }

        /// <summary>
        /// 가장 최근에 작성한 기록지번호(EMRNO)를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_OLDLASTEMRNO(EmrPatient emrPatient, string FormNo)
        {
            string SQL = "SELECT EMRNO";
            SQL += ComNum.VBLF + "FROM ADMIN.EMRXMLMST";
            SQL += ComNum.VBLF + "WHERE PTNO   = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND CHARTDATE >= '20090101'";
            SQL += ComNum.VBLF + "  AND FORMNO = " + FormNo;
            SQL += ComNum.VBLF + "ORDER BY (CHARTDATE || CHARTTIME) DESC";

            return SQL;
        }


        /// <summary>
        /// 가장 최근에 작성한 기록지번호(EMRNO)를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_NEWLASTEMRNO(EmrPatient emrPatient, string FormNo)
        {
            if (emrPatient == null) return string.Empty;

            string SQL = "SELECT MAX(EMRNO) EMRNO";
            SQL += ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST";
            SQL += ComNum.VBLF + "WHERE PTNO   = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND CHARTDATE >= TO_CHAR(SYSDATE - 30, 'YYYYMMDD')";
            SQL += ComNum.VBLF + "  AND FORMNO = " + FormNo;

            return SQL;
        }


        /// <summary>
        /// 당일입원자 검사처방
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_IpdLab(EmrPatient emrPatient, string SearchDate)
        {
            string SQL = "SELECT A.ORDERCODE, B.ORDERNAMES ,B.ORDERNAME , TO_CHAR(A.BDATE, 'YYYY-MM-DD') as BDATE, A.REALQTY, A.QTY,A.NAL";
            SQL += ComNum.VBLF + "FROM ADMIN.OCS_IORDER A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_ORDERCODE B";
            SQL += ComNum.VBLF + "     ON A.ORDERCODE = B.ORDERCODE";
            SQL += ComNum.VBLF + "    AND A.SLIPNO = B.SLIPNO";
            SQL += ComNum.VBLF + "WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + DateTime.ParseExact(SearchDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND A.GBSTATUS = ' '";
            SQL += ComNum.VBLF + "  AND A.BUN IN ('44','52','53','54','55','56','57','58','59','60','61','62','63','64','65','66','67','68','69','70','71','72','73')";

            return SQL;
        }

        /// <summary>
        /// 당일입원자 약처방
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_IpdMed(EmrPatient emrPatient, string SearchDate)
        {
            string SQL = "SELECT A.ORDERCODE, B.ORDERNAMES ,B.ORDERNAME , TO_CHAR(A.BDATE, 'YYYY-MM-DD') as BDATE, A.REALQTY, A.QTY,A.NAL";
            SQL += ComNum.VBLF + "FROM ADMIN.OCS_IORDER A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_ORDERCODE B";
            SQL += ComNum.VBLF + "     ON A.ORDERCODE = B.ORDERCODE";
            SQL += ComNum.VBLF + "    AND A.SLIPNO = B.SLIPNO";
            SQL += ComNum.VBLF + "WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + DateTime.ParseExact(SearchDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND A.GBSTATUS = ' '";
            SQL += ComNum.VBLF + "  AND A.BUN IN('11', '12', '20')";

            return SQL;
        }


        /// <summary>
        /// 당일외래자 검사처방
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_OpdLab(EmrPatient emrPatient, string SearchDate)
        {
            string SQL = "SELECT A.ORDERCODE, B.ORDERNAMES ,B.ORDERNAME , TO_CHAR(A.BDATE, 'YYYY-MM-DD') as BDATE, A.REALQTY, A.QTY,A.NAL";
            SQL += ComNum.VBLF + "FROM ADMIN.OCS_OORDER A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_ORDERCODE B";
            SQL += ComNum.VBLF + "     ON A.ORDERCODE = B.ORDERCODE";
            SQL += ComNum.VBLF + "    AND A.SLIPNO = B.SLIPNO";
            SQL += ComNum.VBLF + "WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + DateTime.ParseExact(SearchDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND A.SEQNO > 0";
            SQL += ComNum.VBLF + "  AND A.NAL > 0";
            SQL += ComNum.VBLF + "  AND A.BUN IN ('44','52','53','54','55','56','57','58','59','60','61','62','63','64','65','66','67','68','69','70','71','72','73')";

            return SQL;
        }

        /// <summary>
        /// 당일외래자 약처방
        /// </summary>
        /// <param name="emrPatient"></param>
        public static string Set_FormPatInfo_OpdMed(EmrPatient emrPatient, string SearchDate)
        {
            string SQL = "SELECT A.ORDERCODE, B.ORDERNAMES ,B.ORDERNAME , TO_CHAR(A.BDATE, 'YYYY-MM-DD') as BDATE, A.REALQTY, A.QTY,A.NAL";
            SQL += ComNum.VBLF + "FROM ADMIN.OCS_OORDER A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_ORDERCODE B";
            SQL += ComNum.VBLF + "     ON A.ORDERCODE = B.ORDERCODE";
            SQL += ComNum.VBLF + "    AND A.SLIPNO = B.SLIPNO";
            SQL += ComNum.VBLF + "WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + DateTime.ParseExact(SearchDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND A.SEQNO > 0";
            SQL += ComNum.VBLF + "  AND A.NAL > 0";
            SQL += ComNum.VBLF + "  AND A.BUN IN('11', '12', '20')";

            return SQL;
        }


        /// <summary>
        /// 피부사정 기록지 점수 쿼리
        /// </summary>
        /// <param name="emrPatient"></param>
        /// <param name="ChartDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_BRADEN(EmrPatient emrPatient, string ChartDate)
        {
            string SQL = " SELECT JUMSU1, JUMSU2, JUMSU3, JUMSU4, JUMSU5, JUMSU6, 0 JUMSU7, ACTDATE, ENTDATE, TOTAL, 'BRADEN' GUBUN ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_BRADEN_SCALE";
            SQL = SQL + ComNum.VBLF + "WHERE ACTDATE = TO_DATE('" + ChartDate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "AND PANO = '" + emrPatient.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "Union";
            SQL = SQL + ComNum.VBLF + "SELECT JUMSU3, JUMSU4, JUMSU2, JUMSU1, JUMSU6, JUMSU5, JUMSU7, ACTDATE, ENTDATE, TOTAL, 'CHILD' GUBUN ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_BRADEN_SCALE_CHILD";
            SQL = SQL + ComNum.VBLF + "WHERE ACTDATE = TO_DATE('" + ChartDate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "AND PANO = '" + emrPatient.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "UNION";
            SQL = SQL + ComNum.VBLF + "SELECT JUMSU4, JUMSU5, JUMSU3, JUMSU2, JUMSU7, JUMSU6, JUMSU8, ACTDATE, ENTDATE, TOTAL, 'BABY' GUBUN ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_BRADEN_SCALE_BABY";
            SQL = SQL + ComNum.VBLF + "WHERE ACTDATE = TO_DATE('" + ChartDate + "', 'YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "AND PANO = '" + emrPatient.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY ACTDATE DESC";

            return SQL;
        }

        /// <summary>
        /// 환자의 당일 상병명 가져오기
        /// 사용우무 판별불가(19-11-19)
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_RODisease(EmrPatient emrPatient)
        {

            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT A.ILLNAMEE, B.RO";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_EILLS B";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN ADMIN.BAS_ILLS A";
            SQL = SQL + ComNum.VBLF + "	   ON B.ILLCODE = A.ILLCODE";
            SQL = SQL + ComNum.VBLF + "   AND A.GBER = '*'";
            SQL = SQL + ComNum.VBLF + "WHERE B.PTNO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "	 AND B.BDATE = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";

            return SQL;
        }

        /// <summary>
        /// 해당 서식지에 해당하는 아이템값의 데이터를 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_GetItemValue(EmrPatient emrPatient, string FormNo, string ItemCD)
        {

            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT B.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN ADMIN.AEMRCHARTROW B";
            SQL = SQL + ComNum.VBLF + "	   ON A.EMRNO = B.EMRNO";
            SQL = SQL + ComNum.VBLF + "	  AND A.EMRNOHIS = B.EMRNOHIS";
            SQL = SQL + ComNum.VBLF + "   AND B.ITEMCD = '" + ItemCD + "'";
            SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + FormNo;

            return SQL;
        }

        /// <summary>
        /// 해당(이전) 서식지에 해당하는 아이템값의 데이터를 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_OldGetItemValue(EmrPatient emrPatient, string FormNo, string ItemCD)
        {

            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT extractValue(B.chartxml, '//" + ItemCD + "') Item";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXMLMST A";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN ADMIN.EMRXML B";
            SQL = SQL + ComNum.VBLF + "	    ON A.EMRNO = B.EMRNO";
            SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + FormNo;

            return SQL;
         }

        /// <summary>
        /// 환자의 당일 상병명 가져오기
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_NowDisease(EmrPatient emrPatient)
        {

            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT A.ILLNAMEE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_IILLS B";
            SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_ILLS A";
            SQL = SQL + ComNum.VBLF + "	   ON B.ILLCODE = A.ILLCODE";
            SQL = SQL + ComNum.VBLF + "   AND A.IllClass = '1'                   ";
            SQL = SQL + ComNum.VBLF + "   AND (A.NOUSE <> 'N' OR A.NOUSE IS NULL)  ";
            SQL = SQL + ComNum.VBLF + "   AND A.DDATE IS NULL                    ";
            SQL = SQL + ComNum.VBLF + "WHERE B.PTNO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "	 AND B.BDATE = TRUNC(SYSDATE)";
            SQL = SQL + ComNum.VBLF + "ORDER BY MAIN, SEQNO";

            return SQL;
        }

        /// <summary>
        /// 병동 전화번호
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_Ward_CallNumber(EmrPatient emrPatient)
        {
            string SQL = " SELECT TEL";
            SQL += ComNum.VBLF + " FROM ADMIN.NUR_TEAM T";
            SQL += ComNum.VBLF + "   INNER JOIN ADMIN.NUR_TEAM_ROOMCODE R";
            SQL += ComNum.VBLF + "      ON R.ROOMCODE = " + emrPatient.room;
            SQL += ComNum.VBLF + "     AND R.WARDCODE = T.WARDCODE";
            SQL += ComNum.VBLF + "     AND R.TEAM = T.TEAM";
            return SQL;
        }

        /// <summary>
        /// ER내원 후 병동 입원환자 출발시간 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_Er_Trans(EmrPatient emrPatient)
        {
            DateTime dtpEndDate = DateTime.ParseExact(emrPatient.medEndDate.Length == 0 ? ComQuery.CurrentDateTime(clsDB.DbCon, "D") : emrPatient.medEndDate, "yyyyMMdd", null);

            string SQL = " SELECT TO_CHAR(FRDATE, 'HH24:MI') FRDATE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.NUR_ER_CHECKLIST_TRANS";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND FRDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + " 00:00', 'YYYY-MM-DD HH24:MI')";
            SQL = SQL + ComNum.VBLF + "  AND FRDATE <= TO_DATE('" + dtpEndDate.ToShortDateString() + " 23:59', 'YYYY-MM-DD HH24:MI')";

            return SQL;
        }


        /// <summary>
        /// 사생활 보호 UPDATE 쿼리
        /// </summary>
        /// <param name="emrPatient"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_Secret(EmrPatient emrPatient)
        {
            string SQL = "UPDATE ADMIN.IPD_NEW_MASTER";
            SQL += ComNum.VBLF + "SET";
            SQL += ComNum.VBLF + "SECRET = '1',";
            SQL += ComNum.VBLF + "SECRET_SABUN = " + clsType.User.IdNumber + ",";
            SQL += ComNum.VBLF + "SECRETINDATE = SYSDATE";
            SQL += ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND TRUNC(INDATE) = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";

            return SQL;
        }


        /// <summary>
        /// 환자의 알러지 정보를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_ALLERGY(EmrPatient emrPatient)
        {
            string SQL = " SELECT REMARK, CODE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_ALLERGY_MST";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND CODE IN('100', '005', '003') -- 연동, 음식(작성), 기타(작성)";
            //SQL = SQL + ComNum.VBLF + "  AND ENTDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 00:00', 'YYYY-MM-DD HH24:MI')";
            //if (!string.IsNullOrEmpty(emrPatient.medEndDate))
            //{
            //    SQL = SQL + ComNum.VBLF + "  AND ENTDATE <= TO_DATE('" + DateTime.ParseExact(emrPatient.medEndDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 23:59', 'YYYY-MM-DD HH24:MI')";
            //}

            return SQL;
        }

        /// <summary>
        /// 환자의 감염정보를 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_INFECT(EmrPatient emrPatient)
        {
            string SQL = " SELECT ADMIN.FC_EXAM_INFECT_MASTER_IMG_EX(A.PANO, " + "TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD'))";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_PATIENT A";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";

            return SQL;
        }

        /// <summary>
        /// 입원 기간내에 가장 최근 BST 가져온다.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_BST(EmrPatient emrPatient)
        {
            string SQL = " SELECT extractValue(chartxml, '//ta2') AS TA2";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "   AND FORMNO = 1572";
            SQL = SQL + ComNum.VBLF + " ORDER BY (CHARTDATE || CHARTTIME) DESC";

            return SQL;
        }

        /// <summary>
        /// 입원 기간내에 수술, 시술명 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_OP_IFNO(EmrPatient emrPatient)
        {
            string SQL = "SELECT DIAGNOSIS, OPTITLE";
            SQL += ComNum.VBLF + "FROM ADMIN.ORAN_MASTER  ";
            SQL += ComNum.VBLF + "WHERE OPDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
            if (emrPatient.medEndDate.Length > 0)
            {
                SQL += ComNum.VBLF + "  AND OPDATE <= TO_DATE('" + DateTime.ParseExact(emrPatient.medEndDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
            }
            SQL += ComNum.VBLF + "  AND PANO = '" + emrPatient.ptNo + "'";

            return SQL;
        }

        /// <summary>
        /// 입원 기간내에 외래 예약한 경우 예약 시간 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_OPD_RESERVED(EmrPatient emrPatient)
        {
            string SQL = "SELECT P.DEPTNAMEK, DATE3, A.DRCODE, B.DRNAME";
            SQL += ComNum.VBLF + "FROM ADMIN.OPD_RESERVED_NEW A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_DOCTOR B";
            SQL += ComNum.VBLF + "     ON A.DRCODE = B.DRCODE";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_CLINICDEPT P";
            SQL += ComNum.VBLF + "     ON P.DEPTCODE = B.DEPTCODE";
            SQL += ComNum.VBLF + "WHERE DATE3 >= SYSDATE ";  //작성일자 기준으로 앞날짜
            //SQL += ComNum.VBLF + "WHERE DATE3 >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + " 00:00', 'YYYY-MM-DD HH24:MI')";
            //if (emrPatient.medEndDate.Length > 0)
            //{
            //    SQL += ComNum.VBLF + "  AND DATE3 <= TO_DATE('" + DateTime.ParseExact(emrPatient.medEndDate, "yyyyMMdd", null).ToShortDateString() + " 00:00', 'YYYY-MM-DD HH24:MI')";
            //}
            SQL += ComNum.VBLF + "  AND PANO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND RETDATE IS NULL";
            SQL += ComNum.VBLF + "ORDER BY DATE3";

            return SQL;
        }

        /// <summary>
        /// 입원한 환자 정보 가져오는 함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_Admission(EmrPatient emrPatient)
        {
            string SQL = string.Empty;
            if (emrPatient == null)
                return SQL;
                    
            SQL = "SELECT (B.JUSO || ' ' || B.JUSO2) JUSO, C.TEL, C.HPHONE, A.INDATE, A.WARDCODE, A.ROOMCODE";
            SQL += ComNum.VBLF + "FROM ADMIN.IPD_NEW_MASTER A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.VIEW_PATIENT_JUSO B";
            SQL += ComNum.VBLF + "     ON B.PANO = A.PANO";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_PATIENT C";
            SQL += ComNum.VBLF + "     ON C.PANO = A.PANO";
            SQL += ComNum.VBLF + "WHERE A.PANO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND TRUNC(INDATE) = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";

            return SQL;
        }

        /// <summary>
        /// 입원기간 내에 전과 하였을경우 처음 입원한 과장 정보 가져오는 함수.
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="emrPatient"></param>
        public static string Query_FormPatInfo_GetInDrName(EmrPatient emrPatient, string strSysDate)
        {
            #region 기존쿼리 19-11-16
            //string SQL = "SELECT C.DEPTNAMEK, B.DRNAME";
            //SQL += ComNum.VBLF + "FROM ";
            //SQL += ComNum.VBLF + "(";
            //SQL += ComNum.VBLF + "SELECT FRDEPT, FRDOCTOR";
            //SQL += ComNum.VBLF + "  FROM ADMIN.IPD_TRANSFOR";
            //SQL += ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            //SQL += ComNum.VBLF + "  AND TRSDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS')";

            //if (emrPatient.medEndDate.Length > 0)
            //{
            //    SQL += ComNum.VBLF + "  AND TRSDATE <= TO_DATE('" + DateTime.ParseExact(emrPatient.medEndDate, "yyyyMMdd", null).ToShortDateString() + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')";
            //}
            //else
            //{
            //    SQL += ComNum.VBLF + "  AND TRSDATE <= TO_DATE('" + DateTime.ParseExact(strSysDate, "yyyyMMdd", null).ToShortDateString() + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')";
            //}

            //SQL += ComNum.VBLF + "UNION ALL ";
            //SQL += ComNum.VBLF + "SELECT IDEPTCODE AS FRDEPT, IDRCODE AS FRDOCTOR";
            //SQL += ComNum.VBLF + "FROM ADMIN.IPD_NEW_MASTER P";
            //SQL += ComNum.VBLF + "  INNER JOIN ADMIN.MID_IPWON_DRCODE_CHECK B";
            //SQL += ComNum.VBLF + "     ON B.IPDNO = P.IPDNO";
            //SQL += ComNum.VBLF + "     AND B.PANO = P.PANO";
            //SQL += ComNum.VBLF + "WHERE P.PANO = '" + emrPatient.ptNo + "'";
            //SQL += ComNum.VBLF + "  AND TRUNC(P.INDATE) = TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
            //SQL += ComNum.VBLF + "  AND P.DEPTCODE = B.IDEPTCODE";
            //SQL += ComNum.VBLF + ") A";
            //SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_DOCTOR B";
            //SQL += ComNum.VBLF + "     ON A.FRDOCTOR = B.DRCODE";
            //SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_CLINICDEPT C";
            //SQL += ComNum.VBLF + "     ON A.FRDEPT = C.DEPTCODE";
            #endregion

            #region 신규쿼리 - 기존에 ㅅ용하던 뷰에 맞춤
            string SQL = "SELECT C.DEPTNAMEK, B.DRNAME";
            SQL += ComNum.VBLF + "FROM ADMIN.VIEW_TRANSFOR A";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.OCS_DOCTOR B";
            SQL += ComNum.VBLF + "     ON A.FRDOCTOR = B.DRCODE";
            SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_CLINICDEPT C";
            SQL += ComNum.VBLF + "     ON A.FRDEPT = C.DEPTCODE";
            
            SQL += ComNum.VBLF + "WHERE PANO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND TRSDATE >= TO_DATE('" + DateTime.ParseExact(emrPatient.medFrDate, "yyyyMMdd", null).ToShortDateString() + " 00:00:00', 'YYYY-MM-DD HH24:MI:SS')";

            if (emrPatient.medEndDate.Length > 0)
            {
                SQL += ComNum.VBLF + "  AND TRSDATE <= TO_DATE('" + DateTime.ParseExact(emrPatient.medEndDate, "yyyyMMdd", null).ToShortDateString() + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND TRSDATE <= TO_DATE('" + DateTime.ParseExact(strSysDate, "yyyyMMdd", null).ToShortDateString() + " 23:59:59', 'YYYY-MM-DD HH24:MI:SS')";
            }

            #endregion

            return SQL;
        }

        /// <summary>
        /// 응급 환자의 내원동기 등을 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_ErPatientInfo(EmrPatient emrPatient)
        {
            string SQL = "SELECT";
            SQL += ComNum.VBLF + "INSTS, -- 내원동기            "      ;
            SQL += ComNum.VBLF + "OPINFO,-- 과거병력 및 수술력     "    ;
            SQL += ComNum.VBLF + "ik91,  -- 보호자 1인상주        "     ;
            SQL += ComNum.VBLF + "ik92,  -- 도난방지            "      ;
            SQL += ComNum.VBLF + "ik93,  -- 낙상방지            "      ;
            SQL += ComNum.VBLF + "IK94,  -- 화재예방 및 비상시 안내  "   ;
            SQL += ComNum.VBLF + "IK95,  -- 금연               "      ;
            SQL += ComNum.VBLF + "IK96,  -- 소아유괴방지          "     ;
            SQL += ComNum.VBLF + "IK100, -- 보조기구(틀니,보청기)   "    ;
            SQL += ComNum.VBLF + "ik97,  --     //기타         "      ;
            SQL += ComNum.VBLF + "IT135  -- 기타 내용           ";
            SQL += ComNum.VBLF + "FROM ADMIN.NUR_ER_PATIENTADD";
            SQL += ComNum.VBLF + "WHERE JDATE   = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "  AND PANO    = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "ORDER BY JDATE DESC, INTIME DESC";
            return SQL;
        }


        /// <summary>
        /// 해당 환자의 KTAS 중증도 분류 일시를 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_Triage(EmrPatient emrPatient)
        {
            string SQL = "SELECT PTMIKTDT, PTMIKTTM, LPAD(PTMIKTS, 5, 'KTAS') PTMIKTS  -- 일자, 시간, 중증도분류 결과";
            SQL += ComNum.VBLF + "FROM ADMIN.NUR_ER_KTAS";
            SQL += ComNum.VBLF + "WHERE PTMIINDT = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "  AND PTMIIDNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND SEQNO = 1";
            SQL += ComNum.VBLF + "ORDER BY PTMIINTM DESC";
            return SQL;
        }

        /// <summary>
        /// 응급 환자 진료 내역을 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_VisitReason(EmrPatient emrPatient)
        {
            string SQL =         "SELECT B.NAME AS PTMIDGKD, --내원사유(질병여부)             ";
            SQL += ComNum.VBLF + "D.NAME AS PTMIARCF, --질병외(의도성여부)             ";
            SQL += ComNum.VBLF + "E.NAME AS PTMIARCS, --질병외(손상기전)               ";
            SQL += ComNum.VBLF + "C.NAME AS PTMITAIP, --교통사고당사자                 ";
            SQL += ComNum.VBLF + "PTMITSBT, --교통사고보장구 - 안전밸드        ";
            SQL += ComNum.VBLF + "PTMITSCS, --교통사고보장구 - 이동용좌석      ";
            SQL += ComNum.VBLF + "PTMITSFA, --교통사고보장구 - 전면에어백      ";
            SQL += ComNum.VBLF + "PTMITSSA, --교통사고보장구 - 측면에어백      ";
            SQL += ComNum.VBLF + "PTMITSHM, --교통사고보장구 - 헬맷           ";
            SQL += ComNum.VBLF + "PTMITSPT, --교통사고보장구 - 무릎및 관절보호대";
            SQL += ComNum.VBLF + "PTMITSNO, --교통사고보장구 - 전혀 착용 않함  ";
            SQL += ComNum.VBLF + "PTMITSUR, --교통사고보장구 - 비해당         ";
            SQL += ComNum.VBLF + "PTMITSUK, --교통사고보장구 - 미상           ";
            SQL += ComNum.VBLF + "PTMIEMRT, --응급진료 코드 (귀가/전원/입원/사망/기타/미상)           ";
            SQL += ComNum.VBLF + "F.NAME AS EMRTRESULT, --응급진료 텍스트(귀가/전원/입원/사망/기타/미상)           ";
            SQL += ComNum.VBLF + "P.AREA    --최종진료구역           ";
            SQL += ComNum.VBLF + "FROM ADMIN.NUR_ER_EMIHPTMI A";
            SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.NUR_ER_PATIENT P --진료결과";
            SQL += ComNum.VBLF + "    ON  TO_CHAR(P.JDATE, 'YYYYMMDD') = A.PTMIINDT";
            SQL += ComNum.VBLF + "    AND P.PANO = A.PTMIIDNO";
            SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_BCODE B --내원사유 질병여부";
            SQL += ComNum.VBLF + "    ON  B.GUBUN='EMI_내원사유(질병여부)' ";
            SQL += ComNum.VBLF + "    AND B.CODE = LTRIM(A.PTMIDGKD, '0')";
            SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_BCODE C --EMI_교통사고손상당사자";
            SQL += ComNum.VBLF + "    ON  C.GUBUN='EMI_교통사고손상당사자'";
            SQL += ComNum.VBLF + "    AND C.CODE = LTRIM(A.PTMITAIP, '0')";
            SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_BCODE D --EMI_내원사유(의도성여부)";
            SQL += ComNum.VBLF + "    ON  D.GUBUN='EMI_내원사유(의도성여부)' ";
            SQL += ComNum.VBLF + "    AND D.CODE = LTRIM(PTMIARCF, '0')";
            SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_BCODE E --EMI_내원사유(손상기전)";
            SQL += ComNum.VBLF + "    ON  E.GUBUN='EMI_내원사유(손상기전)'";
            SQL += ComNum.VBLF + "    AND E.CODE = LTRIM(PTMIARCS, '0')";
            SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_BCODE F --EMI_응급진료결과";
            SQL += ComNum.VBLF + "    ON  F.GUBUN= 'EMI_응급진료결과'";
            SQL += ComNum.VBLF + "    AND F.CODE = A.PTMIEMRT";
            SQL += ComNum.VBLF + "WHERE PTMIINDT = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "  AND PTMIIDNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND SEQNO =";
            SQL += ComNum.VBLF + " (SELECT MAX(SEQNO)";
            SQL += ComNum.VBLF + "    FROM ADMIN.NUR_ER_EMIHPTMI";
            SQL += ComNum.VBLF + "   WHERE PTMIIDNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "     AND PTMIINDT = '" + emrPatient.medFrDate + "')";
            SQL += ComNum.VBLF + "   ORDER BY PTMIINTM DESC";
            return SQL;
        }

        /// <summary>
        /// 해당 환자의 입퇴원 요약지 기록지 갯수를 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_DischargeSummary(EmrPatient emrPatient)
        {

            string SQL = "SELECT 1 AS CNT";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS(";

            SQL += ComNum.VBLF + "SELECT 1";
            SQL += ComNum.VBLF + "FROM ADMIN.EMRXMLMST";
            SQL += ComNum.VBLF + "WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "  AND FORMNO = 1647";

            #region 신규 EMR
            SQL += ComNum.VBLF + "UNION ALL";
            SQL += ComNum.VBLF + "SELECT 1";
            SQL += ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST";
            SQL += ComNum.VBLF + "WHERE PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "  AND MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "  AND FORMNO = 1647";
            #endregion
            SQL += ComNum.VBLF + ")";

            return SQL;
        }

        /// <summary>
        /// ER환자의 가장 최근 접수시간을 가져온다.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_ERInDate(EmrPatient emrPatient)
        {
            string SQL = " SELECT INTIME";
            SQL += ComNum.VBLF + "  From ADMIN.NUR_ER_PATIENT";
            SQL += ComNum.VBLF + " WHERE INTIME >= TO_DATE('" + emrPatient.medFrDate + " 00:00','YYYY-MM-DD HH24:MI')";
            SQL += ComNum.VBLF + "   AND INTIME <= TO_DATE('" + emrPatient.medFrDate + " 23:59','YYYY-MM-DD HH24:MI')";
            SQL += ComNum.VBLF + "   AND PANO = '" + emrPatient.ptNo + "' ";
            SQL += ComNum.VBLF + " ORDER BY INTIME DESC";

            return SQL;
        }


        /// <summary>
        /// 환자의 처방정보를 가지고 온다
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_Order()
        {
            string rtnVal = string.Empty;
            string SQL = string.Empty;





            return rtnVal;
        }

        /// <summary>
        /// 환자의 주증상 및 현병력1 Query
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_MainSymptoms(EmrPatient emrPatient, EmrForm pForm)
        {
            string SQL = string.Empty;
          
            SQL = "SELECT  'OLD' GBN, extractValue(chartxml, '//ta1000') as chiefComplaint,";
            SQL += ComNum.VBLF + " 		   extractValue(chartxml, '//ta1001') as presentIllness     ";
            SQL += ComNum.VBLF + " 		   ,'' as physicalEx     ";
            SQL += ComNum.VBLF + " 		   , CHARTDATE, CHARTTIME     ";
            SQL += ComNum.VBLF + "     FROM ADMIN.EMRXML A                                                ";
            SQL += ComNum.VBLF + "     WHERE EMRNO = (                                              ";
            SQL += ComNum.VBLF + "        SELECT MAX(EMRNO) FROM ADMIN.EMRXML A                           ";
            SQL += ComNum.VBLF + "        INNER JOIN ADMIN.AEMRFORM B                                      ";
            SQL += ComNum.VBLF + "           ON A.FORMNO = B.FORMNO                                    ";
            SQL += ComNum.VBLF + "          AND B.UPDATENO > 0                              ";
            SQL += ComNum.VBLF + "          AND B.OLDGB = '1'    ";
            SQL += ComNum.VBLF + "          AND B.GRPFORMNO = 1002";
            SQL += ComNum.VBLF + "        WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "          AND A.FORMNO <> 1875                                    ";
            SQL += ComNum.VBLF + "          AND A.FORMNO <> 1931                                    ";
            SQL += ComNum.VBLF + "          AND A.FORMNO <> 1974                                    ";
            SQL += ComNum.VBLF + "          AND A.FORMNO <> 1983                                    ";
            SQL += ComNum.VBLF + "          AND A.FORMNO <> 2001                                    ";
            //19-08-19 데이터를 못가져와서 추가함 
            SQL += ComNum.VBLF + "          AND A.FORMNO <> 2593                                    ";
            SQL += ComNum.VBLF + "          AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + " 	)                                                               ";
         
            SQL += ComNum.VBLF + "UNION ALL";
            //SQL += ComNum.VBLF + "SELECT 'NEW' GBN, C.ITEMVALUE AS chiefComplaint, C2.ITEMVALUE AS presentIllness, C3.ITEMVALUE AS physicalEx";
            SQL += ComNum.VBLF + "SELECT 'NEW' GBN, C.ITEMVALUE AS chiefComplaint";
            SQL += ComNum.VBLF + "      , (SELECT LISTAGG(C2.ITEMVALUE, '\r\n') WITHIN GROUP(ORDER BY C2.ITEMINDEX)";
            SQL += ComNum.VBLF + "           FROM ADMIN.AEMRCHARTROW C2   ";
            SQL += ComNum.VBLF + "          WHERE C2.EMRNO = A.EMRNO";
            SQL += ComNum.VBLF + "            AND C2.EMRNOHIS = A.EMRNOHIS";
            SQL += ComNum.VBLF + "            AND C2.ITEMNO IN('I0000014826') -- Present IIIness    ";
            SQL += ComNum.VBLF + "      ) AS presentIllness";
            SQL += ComNum.VBLF + " 		, C3.ITEMVALUE AS physicalEx                         ";
            SQL += ComNum.VBLF + " 		, CHARTDATE, CHARTTIME                                  ";
            SQL += ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A                            ";
            SQL += ComNum.VBLF + "    INNER JOIN ADMIN.AEMRFORM B                         ";
            SQL += ComNum.VBLF + "       ON A.FORMNO = B.FORMNO                               ";
            SQL += ComNum.VBLF + "      AND A.UPDATENO = B.UPDATENO                           ";
            SQL += ComNum.VBLF + "      AND B.GRPFORMNO = 1002 --입원기록지                       ";
            SQL += ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.AEMRCHARTROW C                     ";
            SQL += ComNum.VBLF + "       ON C.EMRNO = A.EMRNO";
            SQL += ComNum.VBLF + "      AND C.EMRNOHIS = A.EMRNOHIS                            ";
            SQL += ComNum.VBLF + "      AND C.ITEMCD IN('I0000011878', 'I0000001749') -- Chief Complaints, Present IIIness       ";
            SQL += ComNum.VBLF + "      AND LENGTH(TRIM(C.ITEMVALUE)) > 0                      ";
            //SQL += ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.AEMRCHARTROW C2                     ";
            //SQL += ComNum.VBLF + "       ON C2.EMRNO = A.EMRNO";
            //SQL += ComNum.VBLF + "      AND C2.EMRNOHIS = A.EMRNOHIS                            ";
            //SQL += ComNum.VBLF + "      AND C2.ITEMNO IN('I0000014826') -- Present IIIness       ";
            //SQL += ComNum.VBLF + "      AND LENGTH(TRIM(C2.ITEMVALUE)) > 0                      ";
            SQL += ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.AEMRCHARTROW C3                     ";
            SQL += ComNum.VBLF + "       ON C3.EMRNO = A.EMRNO";
            SQL += ComNum.VBLF + "      AND C3.EMRNOHIS = A.EMRNOHIS                            ";
            SQL += ComNum.VBLF + "      AND C3.ITEMCD IN('I0000014706') -- physicalEx      ";
            SQL += ComNum.VBLF + " WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "	 AND A.INOUTCLS IN('I', 'O')";
            SQL += ComNum.VBLF + "   AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + "   AND A.EMRNO = (                                              ";
            SQL += ComNum.VBLF + "        SELECT MAX(EMRNO)                            ";
            SQL += ComNum.VBLF + "           FROM ADMIN.AEMRCHARTMST A                           ";
            SQL += ComNum.VBLF + "             INNER JOIN ADMIN.AEMRFORM B                         ";
            SQL += ComNum.VBLF + "                ON A.FORMNO = B.FORMNO                               ";
            SQL += ComNum.VBLF + "               AND A.UPDATENO = B.UPDATENO                           ";
            SQL += ComNum.VBLF + "               AND B.GRPFORMNO = 1002 -- 입원기록지                       ";
            SQL += ComNum.VBLF + "               AND B.FORMNO NOT IN(2238, 2554, 2752, 2593)  -- 폐렴지표서식, 완화의료 초기";
            SQL += ComNum.VBLF + "           WHERE A.PTNO = '" + emrPatient.ptNo + "'";
            SQL += ComNum.VBLF + "             AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
            SQL += ComNum.VBLF + " 	              )   ";
            SQL += ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";

            return SQL;
        }


        /// <summary>
        /// 환자의 주증상 및 현병력2 Query
        /// Query_FormPatInfo_MainSymptoms이 데이터 없을경우 2로 가져옴.
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_MainSymptoms2(EmrPatient emrPatient, EmrForm emrForm)
        {
            string SQL = string.Empty;
            if (emrPatient.medDeptCd.Equals("EN"))
            {
                SQL += ComNum.VBLF + "SELECT 'OLD' AS GBN,";
                SQL += ComNum.VBLF + "       extractValue(chartxml, '//it1')  as chiefComplaint,";
                SQL += ComNum.VBLF + " 		 extractValue(chartxml, '//it2')  as presentIllness,";
                SQL += ComNum.VBLF + " 		 extractValue(chartxml, '//ta1') as physicalEx     ";

                SQL += ComNum.VBLF + "     FROM ADMIN.EMRXML A                                                ";
                SQL += ComNum.VBLF + "     WHERE EMRNO = (                                              ";
                SQL += ComNum.VBLF + "        SELECT MAX(EMRNO) FROM ADMIN.EMRXML A                           ";
                SQL += ComNum.VBLF + "        INNER JOIN ADMIN.EMRFORM B                                      ";
                SQL += ComNum.VBLF + "           ON A.FORMNO = B.FORMNO                                    ";
                SQL += ComNum.VBLF + "        WHERE A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "		    AND B.FORMNO = 1943                                    ";
                SQL += ComNum.VBLF + "          AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
                SQL += ComNum.VBLF + " 	)    ";
            }
            else if (emrPatient.medDeptCd.Equals("CS"))
            {
                SQL += ComNum.VBLF + "SELECT 'OLD' AS GBN,";
                SQL += ComNum.VBLF + "       extractValue(chartxml, '//it7')  as chiefComplaint,";
                SQL += ComNum.VBLF + " 		 extractValue(chartxml, '//ta1')  as presentIllness";

                SQL += ComNum.VBLF + "     FROM ADMIN.EMRXML A                                                ";
                SQL += ComNum.VBLF + "     WHERE EMRNO = (                                              ";
                SQL += ComNum.VBLF + "        SELECT MAX(EMRNO) FROM ADMIN.EMRXML A                           ";
                SQL += ComNum.VBLF + "        INNER JOIN ADMIN.EMRFORM B                                      ";
                SQL += ComNum.VBLF + "           ON A.FORMNO = B.FORMNO                                    ";
                SQL += ComNum.VBLF + "        WHERE A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "		    AND B.FORMNO = 1765                                    ";
                SQL += ComNum.VBLF + "          AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
                SQL += ComNum.VBLF + " 	)    ";
            }
            else
            {
                SQL += ComNum.VBLF + "SELECT 'OLD' AS GBN,";
                SQL += ComNum.VBLF + "       extractValue(chartxml, '//ta1') as chiefComplaint,";
                SQL += ComNum.VBLF + " 		 extractValue(chartxml, '//ta2') as presentIllness     ";

                SQL += ComNum.VBLF + "     FROM ADMIN.EMRXML A                                                ";
                SQL += ComNum.VBLF + "     WHERE EMRNO = (                                              ";
                SQL += ComNum.VBLF + "        SELECT MAX(EMRNO) FROM ADMIN.EMRXML A                           ";
                SQL += ComNum.VBLF + "        INNER JOIN ADMIN.EMRFORM B                                      ";
                SQL += ComNum.VBLF + "           ON A.FORMNO = B.FORMNO                                    ";
                SQL += ComNum.VBLF + "        WHERE A.PTNO = '" + emrPatient.ptNo + "'";
                SQL += ComNum.VBLF + "		    AND B.GRPFORMNO = 12                                    ";
                SQL += ComNum.VBLF + "          AND A.FORMNO <> 1875                                    ";
                SQL += ComNum.VBLF + "          AND A.FORMNO <> 1931                                    ";
                SQL += ComNum.VBLF + "          AND A.FORMNO <> 1974                                    ";
                SQL += ComNum.VBLF + "          AND A.FORMNO <> 1983                                    ";
                SQL += ComNum.VBLF + "          AND A.FORMNO <> 2001                                    ";
                //19-08-19 데이터를 못가져와서 추가함 
                SQL += ComNum.VBLF + "          AND A.FORMNO <> 2593                                    ";
                SQL += ComNum.VBLF + "          AND A.MEDFRDATE = '" + emrPatient.medFrDate + "'";
                SQL += ComNum.VBLF + " 	)    ";
            }

            return SQL;
        }



        /// <summary>
        /// 환자의 주진단 Query
        /// </summary>
        /// <returns></returns>
        public static string Query_FormPatInfo_MainDisease(string strPtno, string strInDate, string strOutDate)
        {

            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "DISTINCT A.ILLCODED,	A.ILLCODE, A.ILLNAMEK, A.ILLNAMEE,";
            SQL = SQL + ComNum.VBLF + "	  TO_CHAR(B.BDATE,'YYYYMMDD') as ORDDATE";
            SQL = SQL + ComNum.VBLF + "	, CASE WHEN EXISTS(SELECT 1 FROM ADMIN.AEMRBASCD WHERE BSNSCLS = '의무기록실' AND UNITCLS  ='POA 예외코드' AND BASCD = A.ILLCODED) THEN 'E' ELSE 'Y' END POA";
            SQL = SQL + ComNum.VBLF + "	, B.SEQNO";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_IILLS B";
            SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_ILLS A";
            SQL = SQL + ComNum.VBLF + "	   ON B.ILLCODE = A.ILLCODE";
            SQL = SQL + ComNum.VBLF + "   AND A.IllClass = '1'                   ";
            SQL = SQL + ComNum.VBLF + "   AND (A.NOUSE <> 'N' OR A.NOUSE IS NULL)  ";
            SQL = SQL + ComNum.VBLF + "   AND A.DDATE IS NULL                    ";
            SQL = SQL + ComNum.VBLF + "WHERE B.PTNO = '" + strPtno + "'";
            SQL = SQL + ComNum.VBLF + "	 AND B.BDATE >= TO_DATE('" + strInDate + "', 'YYYYMMDD')";

            if (strOutDate != "")
            {
                SQL = SQL + ComNum.VBLF + "	 AND B.BDATE <= TO_DATE('" + strOutDate + "', 'YYYYMMDD')";
            }

            SQL = SQL + ComNum.VBLF + "	 AND A.ILLCODE IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SEQNO";

            return SQL;
        }
             

        /// <summary>
        /// 수술 및 처치명 Query
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_OpName(string strPtno, string strInDate, string strOutDate)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(OPDATE,'YYYY/MM/DD') as OPDATE, PANO as PTNO, IPDOPD as INOUTCLS, OPTITLE, DIAGNOSIS, OPDOCT1 as OPDDOCNAME";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ORAN_MASTER";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPtno + "'" ;
            if (strInDate != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND OPDATE >= TO_DATE('" + DateTime.ParseExact(strInDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }
            
            if (strOutDate != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND OPDATE <= TO_DATE('" + DateTime.ParseExact(strOutDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }
                                                                                                                      
            SQL = SQL + ComNum.VBLF + "  AND SWARDTIME > CHR(0)";
            SQL = SQL + ComNum.VBLF + "  AND EORTIME > CHR(0)";
            SQL = SQL + ComNum.VBLF + "  AND OPSTIME > CHR(0)";
            SQL = SQL + ComNum.VBLF + "ORDER BY OPDATE ASC";

            return SQL;
        }


        /// <summary>
        /// 전과정보 Query
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_IpdTrans(string strPtno, string strInDate, string strOutDate)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT A.DRNAME, B.TODEPT, TO_CHAR(TRSDATE,'YYYY-MM-DD') AS TRSDATE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.OCS_DOCTOR A";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN ADMIN.IPD_TRANSFOR B";
            SQL = SQL + ComNum.VBLF + "     ON A.DRCODE = B.TODOCTOR";
            SQL = SQL + ComNum.VBLF + "    AND B.PANO = '" + strPtno + "'";
            if (strInDate != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND B.TRSDATE >= TO_DATE('" + DateTime.ParseExact(strInDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }

            
            SQL = SQL + ComNum.VBLF + "    AND B.FRDEPT <> B.TODEPT";

            if (strOutDate != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND B.TRSDATE <= TO_DATE('" + DateTime.ParseExact(strOutDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
            }
            
            SQL = SQL + ComNum.VBLF + "ORDER BY B.TRSDATE ASC";

            return SQL;
        }


        /// <summary>
        /// 경과요약 및 검사결과 - (퇴원자 방사선소견  리스트) Query
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_TestResult(string strPtno, string strInDate, string strOutDate)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT WRTNO, PANO as PTNO, TO_CHAR(SEEKDATE,'YYYY-MM-DD') as READDATE, XNAME, REPLACE(REPLACE(RESULT,chr(10),' '),chr(13),' ') as RESULT ";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.XRAY_RESULTNEW";
            SQL = SQL + ComNum.VBLF + "WHERE WRTNO = (";
            SQL = SQL + ComNum.VBLF + "     SELECT MAX(WRTNO)";
            SQL = SQL + ComNum.VBLF + "     FROM ADMIN.XRAY_RESULTNEW";
            SQL = SQL + ComNum.VBLF + "     WHERE  PANO = '" + strPtno + "'";
            SQL = SQL + ComNum.VBLF + "       AND TO_CHAR(SEEKDATE,'YYYYMMDD') >= '" + strInDate + "'";

            if (strOutDate != "")
            {
                SQL = SQL + ComNum.VBLF + "       AND TO_CHAR(SEEKDATE,'YYYYMMDD') <= '" + strOutDate + "'";
            }
            
            SQL = SQL + ComNum.VBLF + "       AND APPROVE ='Y'";
            SQL = SQL + ComNum.VBLF + ")";

            return SQL;
        }


        /// <summary>
        /// LAB Query
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_LabResult(string strPtno, string strInDate, string strOutDate)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + "SELECT  R.Refer, R.Result,  R.Unit, LTRIM(M.ExamName) AS EXAMNAME, TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') as RESULTDATE ";
            SQL = SQL + ComNum.VBLF + "				 FROM ADMIN.Exam_ResultC R";
            SQL = SQL + ComNum.VBLF + "				   LEFT OUTER JOIN ADMIN.Exam_Master M";
            SQL = SQL + ComNum.VBLF + "				     ON R.SubCode = M.MasterCode";
            SQL = SQL + ComNum.VBLF + "				  WHERE SpecNo IN (SELECT SpecNo";
            SQL = SQL + ComNum.VBLF + "				                     FROM ADMIN.Exam_Specmst";
            SQL = SQL + ComNum.VBLF + "				                     WHERE PANO = '" + strPtno + "'";                                                               
            SQL = SQL + ComNum.VBLF + "				                     AND BDate  >= TRUNC(SYSDATE-750)  ";
            SQL = SQL + ComNum.VBLF + "				                      AND WorkSTS NOT IN ('A','T')     ";
            SQL = SQL + ComNum.VBLF + "				                      AND Status IN ('04','14','05')   ";
            SQL = SQL + ComNum.VBLF + "				                      AND BI NOT IN ('61','62')        ";
            SQL = SQL + ComNum.VBLF + "				                      AND ( HicNo IS NULL OR HicNo =0 )";
            SQL = SQL + ComNum.VBLF + "				                      AND STATUS = '05'";
            SQL = SQL + ComNum.VBLF + "				    )";
            SQL = SQL + ComNum.VBLF + "				    AND REFER IS NOT NULL";
            SQL = SQL + ComNum.VBLF + "				    AND TO_CHAR(R.RESULTDATE,'YYYYMMDD') >= '" + strInDate + "'";

            if (strOutDate != "")
            {
                SQL = SQL + ComNum.VBLF + "				    AND TO_CHAR(R.RESULTDATE,'YYYYMMDD') <= '" + strOutDate + "'";
            }

            return SQL;
        }

        /// <summary>
        /// LAB Query
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_LabResultNew(EmrPatient AcpEmr)
        {
            string SQL = string.Empty; 
            SQL = SQL + ComNum.VBLF + "WITH LAB_RESULT AS";
            SQL = SQL + ComNum.VBLF + "(";
            SQL = SQL + ComNum.VBLF + "SELECT  R.Refer, R.Result,  R.Unit, LTRIM(M.ExamName) AS EXAMNAME, TO_CHAR(R.RESULTDATE,'YYYY-MM-DD') as RESULTDATE, R.RESULTDATE AS MRESULTDATE ";
            SQL = SQL + ComNum.VBLF + "				 FROM ADMIN.Exam_ResultC R";
            SQL = SQL + ComNum.VBLF + "				   LEFT OUTER JOIN ADMIN.Exam_Master M";
            SQL = SQL + ComNum.VBLF + "				     ON R.SubCode = M.MasterCode";
            SQL = SQL + ComNum.VBLF + "				  WHERE SpecNo IN (SELECT SpecNo";
            SQL = SQL + ComNum.VBLF + "				                     FROM ADMIN.Exam_Specmst";
            SQL = SQL + ComNum.VBLF + "				                     WHERE PANO = '" + AcpEmr.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "				                     AND BDate  >= TRUNC(SYSDATE-750)  ";
            SQL = SQL + ComNum.VBLF + "				                      AND WorkSTS NOT IN ('A','T')     ";
            SQL = SQL + ComNum.VBLF + "				                      AND Status IN ('04','14','05')   ";
            SQL = SQL + ComNum.VBLF + "				                      AND BI NOT IN ('61','62')        ";
            SQL = SQL + ComNum.VBLF + "				                      AND ( HicNo IS NULL OR HicNo =0 )";
            SQL = SQL + ComNum.VBLF + "				                      AND STATUS = '05'";
            SQL = SQL + ComNum.VBLF + "				    )";
            SQL = SQL + ComNum.VBLF + "				    AND REFER IS NOT NULL";
            if (AcpEmr.medFrDate != "")
            {
                SQL = SQL + ComNum.VBLF + "				    AND R.RESULTDATE >= TO_DATE('" + DateTime.ParseExact(AcpEmr.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 00:00', 'YYYY-MM-DD HH24:MI')";
            }
            
            if (AcpEmr.medEndDate != "")
            {
                SQL = SQL + ComNum.VBLF + "				    AND R.RESULTDATE <= TO_DATE('" + DateTime.ParseExact(AcpEmr.medEndDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + " 23:59', 'YYYY-MM-DD HH24:MI')";
            }
            SQL = SQL + ComNum.VBLF + ")";

            SQL = SQL + ComNum.VBLF + "SELECT REFER, RESULT,  UNIT, EXAMNAME, RESULTDATE ";
            SQL = SQL + ComNum.VBLF + "  FROM LAB_RESULT";
            SQL = SQL + ComNum.VBLF + " WHERE MRESULTDATE >= ";
            SQL = SQL + ComNum.VBLF + " ( ";
            SQL = SQL + ComNum.VBLF + " SELECT MAX(MRESULTDATE) - 3 ";
            SQL = SQL + ComNum.VBLF + "  FROM LAB_RESULT";
            SQL = SQL + ComNum.VBLF + " )";

            return SQL;
        }


        /// <summary>
        /// 퇴원약 Query
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <returns></returns>
        public static string Query_FormPatInfo_MedicineDischarge(string strPtno, string strInDate, string strOutDate)
        {
            string SQL = string.Empty;
            SQL = SQL + ComNum.VBLF + " SELECT (SELECT SUNAMEK FROM ADMIN.BAS_SUN WHERE TRIM(A.SUCODE) = TRIM(SUNEXT)) AS SUNAMEK, B.ORDERNAMES ,B.ORDERNAME , A.REALQTY, A.QTY, A.REALNAL, A.NAL";
            SQL = SQL + ComNum.VBLF + "	FROM ADMIN.OCS_IORDER A";
            SQL = SQL + ComNum.VBLF + "	  INNER JOIN ADMIN.OCS_ORDERCODE B";
            SQL = SQL + ComNum.VBLF + "      ON A.ORDERCODE = B.ORDERCODE";
            SQL = SQL + ComNum.VBLF + "     AND A.SLIPNO   = B.SLIPNO";
            //SQL = SQL + ComNum.VBLF + "	  INNER JOIN ADMIN.BAS_SUN C";
            //SQL = SQL + ComNum.VBLF + "      ON TRIM(A.SUCODE) = TRIM(C.SUNEXT)";
            SQL = SQL + ComNum.VBLF + " WHERE A.PTNO = '" + strPtno +  "'";
            SQL = SQL + ComNum.VBLF + "   AND A.GBSTATUS = ' '";
            SQL = SQL + ComNum.VBLF + "   AND A.GBTFLAG = 'T'";
            SQL = SQL + ComNum.VBLF + "   AND A.SUCODE <> 'JAGA'";
            SQL = SQL + ComNum.VBLF + "   AND TO_CHAR(A.BDATE, 'YYYYMMDD') >= '" + strInDate + "'";
            if(strOutDate.Length > 0)
            {
                SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(A.BDATE, 'YYYYMMDD') <= '" + strOutDate + "'";
            }
            SQL = SQL + ComNum.VBLF + "    AND A.BUN IN ('11', '12' , '20')";

            return SQL;
        }
    }
}
