namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicMisuMstSlipRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuMstSlipRepository()
        {
        }

        public List<HIC_MISU_MST_SLIP> GetMisuCashSum(string argFDate, string argTDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  B.GJONG, A.GEACODE, SUM(A.SLIPAMT) SLIPAMT              ");

            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_SLIP A, HIC_MISU_MST B   ");

            parameter.AppendSql("   WHERE                   A.BDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                 ");
            parameter.AppendSql("       AND                 A.BDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                 ");
            parameter.AppendSql("       AND                 A.GEACODE IN ('21','55')                                "); // 현금입금, 카드입금
            parameter.AppendSql("       AND                 A.WRTNO = B.WRTNO(+)                                    ");

            parameter.AppendSql("   GROUP BY                B.GJONG, A.GEACODE                                      ");
            parameter.AppendSql("   ORDER BY                B.GJONG, A.GEACODE                                      ");

            parameter.Add("FDATE",      argFDate        );
            parameter.Add("TDATE",      argTDate        );

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);

        }

        public int GetinsertDelHistory(string strROWID, long GnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO     HIC_MISU_HISTORY (                              ");
            parameter.AppendSql("                   JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE     ");
            parameter.AppendSql("                   ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)       ");

            parameter.AppendSql("   SELECT          SYSDATE, :GNJOBSABUN,'4',WRTNO,BDATE,LTDCODE    ");
            parameter.AppendSql("                   ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN        ");

            parameter.AppendSql("   FROM            ADMIN.HIC_MISU_SLIP                       ");

            parameter.AppendSql("   WHERE           ROWID= :STRROWID                                ");
            parameter.AppendSql("   ORDER BY        BDATE DESC                                      ");

            parameter.Add("STRROWID",       strROWID    );
            parameter.Add("GNJOBSABUN",     GnJobSabun  );

            return ExecuteNonQuery(parameter);
        }

        public int HistoryAfter(string strROWID, long GnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE   ");
            parameter.AppendSql("                       ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                       ");

            parameter.AppendSql("   SELECT              SYSDATE, :GNJOBSABUN,'3',WRTNO,BDATE,LTDCODE                    ");
            parameter.AppendSql("                       ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                        ");

            parameter.AppendSql("   FROM                ADMIN.HIC_MISU_SLIP                                       ");

            parameter.AppendSql("   WHERE               ROWID= :STRROWID                                                ");

            parameter.Add("STRROWID",       strROWID        );
            parameter.Add("GNJOBSABUN",     GnJobSabun      );

            return ExecuteNonQuery(parameter);
        }

        public int LtdCodeUpdate(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   UPDATE              HIC_MISU_SLIP SET LTDCODE=:LTDCODE      ");
            parameter.AppendSql("   WHERE               WRTNO= :WRTNO                           ");

            parameter.Add("LTDCODE",    item.LTDCODE        );
            parameter.Add("WRTNO",      item.WRTNO          );

            return ExecuteNonQuery(parameter);
        }
        public List<HIC_MISU_MST_SLIP> Getitem(string strFdate, string strTdate, string strJong, string strDtlJong, string strLtdCode, bool rdoJob1, bool rdoJob2, long txtMisuAmt, string cboName, bool rdoSort, long LtdCode)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("   SELECT              WRTNO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,LTDCODE,GJONG,SUM(GAMAMT+SAKAMT+BANAMT) DAMT                  ");
            parameter.AppendSql("                     , MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,GIRONO,DAMNAME,REMARK      ");
            parameter.AppendSql("                     , ADMIN.FC_HIC_LTDNAME(LTDCODE) LTDNAME                             ");
            parameter.AppendSql("                     , ADMIN.FC_HIC_GJJONG_NAME(GJONG, '') GJNAME                        ");

            parameter.AppendSql("   FROM                ADMIN.HIC_MISU_MST                                                ");

            if (strFdate == "")
            {
                parameter.AppendSql("   WHERE                BDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                                ");
            }
            else
            {
                parameter.AppendSql("   WHERE                BDATE>=TO_DATE(:FDATE,'YYYY-MM-DD')                                ");
                parameter.AppendSql("       AND              BDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                                ");
            }

            if (strJong != "*")
            {
                parameter.AppendSql("       AND              MISUJONG=:STRJONG                                                  ");
            }

            switch (strDtlJong)
            {
                case "1":
                    parameter.AppendSql("       AND      GJONG IN ('13','18','43','46')                                     "); // 성인병
                    break;
                case "2":
                    parameter.AppendSql("       AND      GJONG IN ('12','17','42','45')                                     "); // 공무원
                    break;
                case "3":
                    parameter.AppendSql("       AND      (GJONG <= '30' OR GJONG IN ('41','44'))                            "); // 사업장
                    parameter.AppendSql("       AND      GJONG NOT IN ('12','13','17','18')                                 ");
                    break;
                case "4":
                    parameter.AppendSql("       AND      GJONG >= '31' AND GJONG <= '80'                                    "); // 기타검진
                    break;
                case "5":
                    parameter.AppendSql("       AND      GJong = '81'                                                       "); // 작업환경측정
                    break;
                case "6":
                    parameter.AppendSql("       AND      GJong = '82'                                                       "); // 보건관리대행
                    break;
                case "7":
                    parameter.AppendSql("       AND      GJong = '83'                                                       "); // 종합건진
                    break;
            }

            if (strLtdCode != "") { parameter.AppendSql("           AND      LTDCODE = :LTDCODE                                "); }
            if (rdoJob1 == true) { parameter.AppendSql("           AND      GBEND = 'N'                                     "); }   // 미수자
            if (rdoJob2 == true) { parameter.AppendSql("           AND      GBEND = 'Y'                                     "); }   // 완불자
            if (txtMisuAmt > 0) { parameter.AppendSql("           AND      MISUAMT = :MISUAMT                              "); }
            if (cboName != "") { parameter.AppendSql("           AND      DAMNAME LIKE :CBONAME                           "); }

            parameter.AppendSql("               GROUP BY        WRTNO,BDATE,LTDCODE,GJONG,                                   ");
            parameter.AppendSql("                               MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,GIRONO,DAMNAME,REMARK      ");

            if (rdoSort == true)
            {
                parameter.AppendSql("   ORDER BY         BDATE, WRTNO                                                        ");
            }
            else
            {
                parameter.AppendSql("   ORDER BY         GIRONO, BDATE, WRTNO                                                ");
            }


            if (strFdate == "")
            {
                parameter.Add("TDATE", strTdate);
            }
            else
            {
                parameter.Add("TDATE", strTdate);
                parameter.Add("FDATE", strFdate);
            }

            if (strJong != "*")
            {
                parameter.Add("STRJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (strLtdCode != "") { parameter.Add("LTDCODE", LtdCode); }
            if (txtMisuAmt > 0) { parameter.Add("MISUAMT", txtMisuAmt); }
            if (cboName != "") { parameter.AddLikeStatement("CBONAME", cboName); }

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuSlipDisplay(long nWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  TO_CHAR(BDate,'YYYY-MM-DD') BDate,GeaCode,SlipAmt       ");
            parameter.AppendSql("                         , Remark,ROWID                                            ");
            parameter.AppendSql("   FROM                  " + ComNum.DB_PMPA + "HIC_MISU_SLIP                      ");

            parameter.AppendSql("   WHERE                   WRTNO = :WRTNO                                          ");
            parameter.AppendSql("       AND                 GEACODE >= '21'                                         ");
            parameter.AppendSql("   ORDER BY                BDATE,GEACODE                                           ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuMaster2(long nWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  LTDCODE,MISUJONG,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE                         ");
            parameter.AppendSql("                         , GJONG,MISUGBN,MISUAMT,IPGUMAMT,GAMAMT,PUMMOK                              ");
            parameter.AppendSql("                         , SAKAMT,BANAMT,JANAMT,GIRONO,DAMNAME,REMARK,ROWID                           ");


            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_MST                                         ");

            parameter.AppendSql("   WHERE                   WRTNO = :WRTNO                                                             ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public int GongDanMisuSlipNew(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_SLIP (WRTNO,BDATE,LTDCODE,GEACODE,SLIPAMT                                                    ");
            parameter.AppendSql("                     , REMARK,ENTDATE,ENTSABUN                                                                               ");
            parameter.AppendSql("                       ) VALUES (                                                                                            ");
            parameter.AppendSql("                       :WRTNO, TO_DATE(:BDATE,'YYYY-MM-DD'), :LTDCODE, :GEACODE, :SLIPAMT, :REMARK, SYSDATE, :ENTSABUN)      ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("GEACODE", item.GEACODE);
            parameter.Add("SLIPAMT", item.SLIPAMT);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int GongDanMisuMasterNew(HIC_MISU_MST_SLIP item, string fstrJong)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_MST ( WRTNO,LTDCODE,MISUJONG,JISA,KIHO,BDATE,GJONG                                                                ");
            parameter.AppendSql("                     , GBEND,MISUGBN,MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,MIRCHAAMT,MIRCHAREMARK,MIRCHADATE                               ");
            
            if(fstrJong != "6")
            {
                parameter.AppendSql("                 , DAMNAME,REMARK,PUMMOK, GIRONO, MIRGBN,GBMISUBUILD2,YYMM_JIN                                                                ");
            }
            else
            {
                parameter.AppendSql("                 , DAMNAME,REMARK,PUMMOK, GIRONO, MIRGBN,GBMISUBUILD3,YYMM_JIN                                                                ");

            }
            parameter.AppendSql("                     , ENTDATE,ENTSABUN,GJYEAR                                                                                                    ");



            parameter.AppendSql("                       ) VALUES (                                                                                                                 ");

            parameter.AppendSql("                       :WRTNO, :LTDCODE, :MISUJONG, :JISA, :KIHO, TO_DATE(:BDATE,'YYYY-MM-DD'), :GJONG                                                                  ");
            parameter.AppendSql("                     , :GBEND, :MISUGBN, :MISUAMT, :IPGUMAMT, :GAMAMT, :SAKAMT, :BANAMT, :JANAMT, :MIRCHAAMT, :MIRCHAREMARK, TO_DATE(:MIRCHADATE,'YYYY-MM-DD')          ");
            parameter.AppendSql("                     , :DAMNAME, :REMARK, :PUMMOK, :GIRONO, :MIRGBN, :GBMISUBUILD4, :YYMM_JIN, SYSDATE, :ENTSABUN, :GJYEAR)                       ");



            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("MISUJONG", item.MISUJONG);
            parameter.Add("JISA", item.JISA);
            parameter.Add("KIHO", item.KIHO);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("GJONG", item.GJONG);
            parameter.Add("GBEND", item.GBEND);
            parameter.Add("MISUGBN", item.MISUGBN);
            parameter.Add("MISUAMT", item.MISUAMT);
            parameter.Add("IPGUMAMT", item.IPGUMAMT);
            parameter.Add("GAMAMT", item.GAMAMT);
            parameter.Add("SAKAMT", item.SAKAMT);
            parameter.Add("BANAMT", item.BANAMT);
            parameter.Add("JANAMT", item.JANAMT);
            parameter.Add("MIRCHAAMT", item.MIRCHAAMT);
            parameter.Add("MIRCHAREMARK", item.MIRCHAREMARK);
            parameter.Add("MIRCHADATE", item.MIRCHADATE);
            parameter.Add("DAMNAME", item.DAMNAME);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("PUMMOK", item.PUMMOK);
            parameter.Add("GIRONO", item.GIRONO);
            parameter.Add("MIRGBN", item.MIRGBN);
            parameter.Add("GBMISUBUILD4", item.GBMISUBUILD4);
            parameter.Add("YYMM_JIN", item.YYMM_JIN);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("GJYEAR", item.GJYEAR);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateRowid(string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE                 HIC_MISU_MST SET GBMISUBUILD2='Y'                       ");
            parameter.AppendSql("WHERE                  WHERE ROWID=:ROWID                                      ");

            parameter.Add("ROWID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MISU_MST_SLIP> getMisuNum(string strltdcode, string strJong, string strTDate, double nAmt)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT                     WRTNO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,ROWID           ");
            parameter.AppendSql("FROM                       ADMIN.HIC_MISU_MST                                ");
            parameter.AppendSql("WHERE                      LTDCODE = :LTDCODE                                      ");
            parameter.AppendSql("    AND                    MISUJONG='1'                                            "); // 회사미수

            switch (strJong)
            {
                case "1":
                    parameter.AppendSql("    AND            GJONG IN ('13','18')                                    ");
                    break;
                case "2":
                    parameter.AppendSql("    AND            GJONG IN ('12','17')                                    ");
                    break;
                case "3":
                    parameter.AppendSql("    AND            (GJONG <= '30' AND GJONG NOT IN ('12','13','17','18'))  ");
                    break;
                case "4":
                    parameter.AppendSql("    AND            GJJONG IN ('31')                                        ");
                    break;
                case "5":
                    parameter.AppendSql("    AND            (GJONG >= '31' AND GJONG <= '80')                       ");
                    break;
            }

            parameter.AppendSql("    AND                    BDATE>=TO_DATE(:TDATE,'YYYY-MM-DD')                      ");
            parameter.AppendSql("    AND                    MISUAMT=:NAMT                                            ");
            parameter.AppendSql("    AND                    GBMISUBUILD IS NULL                                      ");
            
            parameter.AppendSql("ORDER BY                   BDATE                                                    ");
            
            parameter.Add("LTDCODE", strltdcode);
            parameter.Add("TDATE", strTDate);
            parameter.Add("NAMT", nAmt);
            
            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public int NewHistoryInsert(long gnJobSabun, long nMisuNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO            HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE   ");
            parameter.AppendSql("                     , GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                        ");

            parameter.AppendSql("SELECT                 SYSDATE, :JOBSABUN, '1', WRTNO, BDATE, LTDCODE                  ");
            parameter.AppendSql("                     , GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                         ");

            parameter.AppendSql("FROM                   ADMIN.HIC_MISU_SLIP                                       ");

            parameter.AppendSql("WHERE                  WRTNO = :WRTNO                                                  ");
            
            
            parameter.Add("JOBSABUN", gnJobSabun);
            parameter.Add("WRTNO", nMisuNo);

            return ExecuteNonQuery(parameter);
        }

        public int GongDanNewHistoryInsert(long gnJobSabun, long nMisuNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO            HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE   ");
            parameter.AppendSql("                     , GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                        ");

            parameter.AppendSql("SELECT                 SYSDATE, :JOBSABUN, '1', WRTNO, BDATE, LTDCODE                  ");
            parameter.AppendSql("                     , GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                         ");

            parameter.AppendSql("FROM                   ADMIN.HIC_MISU_SLIP                                       ");

            parameter.AppendSql("WHERE                  WRTNO = :WRTNO                                                  ");


            parameter.Add("JOBSABUN", gnJobSabun);
            parameter.Add("WRTNO", nMisuNo);

            return ExecuteNonQuery(parameter);
        }

        public int MisuSlipNew(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_SLIP (WRTNO,BDATE,LTDCODE,GEACODE,SLIPAMT                                                    ");
            parameter.AppendSql("                     , REMARK,ENTDATE,ENTSABUN                                                                               ");
            parameter.AppendSql("                       ) VALUES (                                                                                            ");
            parameter.AppendSql("                       :WRTNO, TO_DATE(:BDATE,'YYYY-MM-DD'), :LTDCODE, :GEACODE, :SLIPAMT, :REMARK, SYSDATE, :ENTSABUN)      ");
            
            parameter.Add("WRTNO",      item.WRTNO);
            parameter.Add("BDATE",      item.BDATE);
            parameter.Add("LTDCODE",    item.LTDCODE);
            parameter.Add("GEACODE",    item.GEACODE);
            parameter.Add("SLIPAMT",    item.SLIPAMT);
            parameter.Add("REMARK",     item.REMARK);
            parameter.Add("ENTSABUN",   item.ENTSABUN);
            
            return ExecuteNonQuery(parameter);
        }

        public int MisuMasterNew(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_MST ( WRTNO,LTDCODE,MISUJONG,JISA,KIHO,BDATE,GJONG                                                                ");
            parameter.AppendSql("                     , GBEND,MISUGBN,MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,MIRCHAAMT,MIRCHAREMARK,MIRCHADATE                               ");
            parameter.AppendSql("                     , DAMNAME,REMARK,PUMMOK, GIRONO, MIRGBN,GBMISUBUILD4,YYMM_JIN, ENTDATE,ENTSABUN,GJYEAR                                       ");
            
            parameter.AppendSql("                       ) VALUES (                                                                                                                 ");

            parameter.AppendSql("                       :WRTNO, :LTDCODE, :MISUJONG, :JISA, :KIHO, TO_DATE(:BDATE,'YYYY-MM-DD'), :GJONG                                                                  ");
            parameter.AppendSql("                     , :GBEND, :MISUGBN, :MISUAMT, :IPGUMAMT, :GAMAMT, :SAKAMT, :BANAMT, :JANAMT, :MIRCHAAMT, :MIRCHAREMARK, TO_DATE(:MIRCHADATE,'YYYY-MM-DD')          ");
            parameter.AppendSql("                     , :DAMNAME, :REMARK, :PUMMOK, :GIRONO, :MIRGBN, :GBMISUBUILD4, :YYMM_JIN, SYSDATE, :ENTSABUN, :GJYEAR)                       "                      );



            parameter.Add("WRTNO",          item.WRTNO);
            parameter.Add("LTDCODE",        item.LTDCODE);
            parameter.Add("MISUJONG",       item.MISUJONG);
            parameter.Add("JISA",           item.JISA);
            parameter.Add("KIHO",           item.KIHO);
            parameter.Add("BDATE",          item.BDATE);
            parameter.Add("GJONG",          item.GJONG);
            parameter.Add("GBEND",          item.GBEND);
            parameter.Add("MISUGBN",        item.MISUGBN);
            parameter.Add("MISUAMT",        item.MISUAMT);
            parameter.Add("IPGUMAMT",       item.IPGUMAMT);
            parameter.Add("GAMAMT",         item.GAMAMT);
            parameter.Add("SAKAMT",         item.SAKAMT);
            parameter.Add("BANAMT",         item.BANAMT);
            parameter.Add("JANAMT",         item.JANAMT);
            parameter.Add("MIRCHAAMT",      item.MIRCHAAMT);
            parameter.Add("MIRCHAREMARK",   item.MIRCHAREMARK);
            parameter.Add("MIRCHADATE",     item.MIRCHADATE);
            parameter.Add("DAMNAME",        item.DAMNAME);
            parameter.Add("REMARK",         item.REMARK);
            parameter.Add("PUMMOK",         item.PUMMOK);
            parameter.Add("GIRONO",         item.GIRONO);
            parameter.Add("MIRGBN",         item.MIRGBN);
            parameter.Add("GBMISUBUILD4",   item.GBMISUBUILD4);
            parameter.Add("YYMM_JIN",       item.YYMM_JIN);
            parameter.Add("ENTSABUN",       item.ENTSABUN);
            parameter.Add("GJYEAR",         item.GJYEAR);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetJisaName(string strFdate, string strTdate, string strJong, string strDtlJong, string strJisaCode, bool rdoJob1, bool rdoJob2, string strView, bool rdoSort1)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("       SELECT               WRTNO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,JISA,KIHO,GJONG,SUM(GAMAMT+SAKAMT+BANAMT) DAMT                ");
            parameter.AppendSql("                          , MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,GIRONO,REMARK             ");
            parameter.AppendSql("                          , ADMIN.FC_HIC_GJJONG_NAME(GJONG, '') GJNAME                       ");
            parameter.AppendSql("                          , ADMIN.FC_HIC_CODE_NM('21', JISA) JISACODE                        ");
            parameter.AppendSql("                          , ADMIN.FC_HIC_CODE_NM('18', KIHO) KIHOCODE                        ");


            parameter.AppendSql("       FROM                ADMIN.HIC_MISU_MST                                                ");

            if (strFdate == "")
            {
                parameter.AppendSql("   WHERE                BDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                                    ");
            }
            else
            {
                parameter.AppendSql("   WHERE                BDATE>=TO_DATE(:FDATE,'YYYY-MM-DD')                                    ");
                parameter.AppendSql("       AND              BDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                                    ");
            }

            if (strJong != "*")
            {
                parameter.AppendSql("       AND              MISUJONG=:STRJONG                                                      ");
            }

            switch (strDtlJong)
            {
                case "1":
                    parameter.AppendSql("       AND      GJONG IN ('13','18','43','46')                                         "); // 성인병
                    break;
                case "2":
                    parameter.AppendSql("       AND      GJONG IN ('12','17','42','45')                                         "); // 공무원
                    break;
                case "3":
                    parameter.AppendSql("       AND      (GJONG <= '30' OR GJONG IN ('41','44'))                                "); // 사업장
                    parameter.AppendSql("       AND      GJONG NOT IN ('12','13','17','18')                                     ");
                    break;
                case "4":
                    parameter.AppendSql("       AND      GJONG >= '31' AND GJONG <= '80'                                        "); // 기타검진
                    break;
                case "5":
                    parameter.AppendSql("       AND      GJong = '81'                                                           "); // 작업환경측정
                    break;
                case "6":
                    parameter.AppendSql("       AND      GJong = '82'                                                           "); // 보건관리대행
                    break;
                case "7":
                    parameter.AppendSql("       AND      GJong = '83'                                                           "); // 종합건진
                    break;
            }

            if (strJisaCode != "") { parameter.AppendSql("           AND      JISA = :JISACODE                                   "); }
            if (strView != "") { parameter.AppendSql("           AND      REMARK LIKE :STRVIEW                             "); }
            if (rdoJob1 == true) { parameter.AppendSql("           AND      GBEND = 'N'                                      "); }   // 미수자
            if (rdoJob2 == true) { parameter.AppendSql("           AND      GBEND = 'Y'                                      "); }   // 완불자


            parameter.AppendSql("               GROUP BY        WRTNO,BDATE,JISA,KIHO,GJONG,                                    ");
            parameter.AppendSql("                               MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,GIRONO,REMARK      ");

            if (rdoSort1 == true)
            {
                parameter.AppendSql("           ORDER BY         BDATE, WRTNO                                                   ");
            }
            else
            {
                parameter.AppendSql("           ORDER BY         GIRONO, BDATE, WRTNO                                           ");
            }

            if (strFdate == "")
            {
                parameter.Add("TDATE", strTdate);
            }
            else
            {
                parameter.Add("TDATE", strTdate);
                parameter.Add("FDATE", strFdate);
            }

            if (strJong != "*")
            {
                parameter.Add("STRJONG", strJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (strJisaCode != "") { parameter.Add("JISACODE", strJisaCode); }
            if (strView != "") { parameter.AddLikeStatement("STRVIEW", strView); }

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetchunguNum(string strInput)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  WRTNO                           ");

            parameter.AppendSql("   FROM                    ADMIN.HIC_MISU_MST        ");

            parameter.AppendSql("   WHERE                   GIRONO=:GIRONO                  ");

            parameter.Add("GIRONO", strInput);

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }
        
        public int HistoryInsert_New(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE   ");
            parameter.AppendSql("                       ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                       ");

            parameter.AppendSql("   SELECT              SYSDATE, :GNJOBSABUN,'1',WRTNO,BDATE,LTDCODE                    ");
            parameter.AppendSql("                       ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                        ");

            parameter.AppendSql("   FROM                ADMIN.HIC_MISU_SLIP                                       ");

            parameter.AppendSql("   WHERE               WRTNO= :WRTNO ");
            parameter.AppendSql("                       AND BDATE <= TO_DATE(:BDATE,'YYYY-MM-DD')                       ");
            parameter.AppendSql("                       AND GEACODE = :GEACODE                                          ");
            parameter.AppendSql("                       AND SLIPAMT = :SLIPAMT                                          ");
            parameter.AppendSql("                       AND REMARK = :REMARK                                            ");

            parameter.Add("WRTNO",      item.WRTNO      );
            parameter.Add("BDATE",      item.BDATE      );
            parameter.Add("GEACODE",    item.GEACODE    );
            parameter.Add("SLIPAMT",    item.SLIPAMT    );
            parameter.Add("REMARK",     item.REMARK     );
            parameter.Add("GNJOBSABUN", item.ENTSABUN   );


            return ExecuteNonQuery(parameter);
        }
        
        public int DelListHistory(HIC_MISU_MST_SLIP item, long nWrtno, long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE,  ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                        ");

            parameter.AppendSql("   SELECT              SYSDATE, SYSDATE, :GNJOBSABUN ,'4',WRTNO,BDATE,LTDCODE,         ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                         ");

            parameter.AppendSql("   FROM                " + ComNum.DB_PMPA + "HIC_MISU_SLIP                             ");

            parameter.AppendSql("   WHERE               WRTNO= :NWRTNO                                                  ");

            parameter.Add("GNJOBSABUN",     gnJobSabun  );
            parameter.Add("nWrtno",         nWrtno      );
            
            return ExecuteNonQuery(parameter);
        }

        public int DelMisuSlip(HIC_MISU_MST_SLIP item, long nWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   DELETE         HIC_MISU_SLIP");
            parameter.AppendSql("   WHERE          WRTNO= :nWrtno ");

            parameter.Add("nWrtno", nWrtno);

            return ExecuteNonQuery(parameter);
        }

        public int DelMisuMaster(HIC_MISU_MST_SLIP item, long nWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   DELETE         HIC_MISU_MST");
            parameter.AppendSql("   WHERE          WRTNO= :nWrtno ");
            
            parameter.Add("nWrtno", nWrtno);

            return ExecuteNonQuery(parameter);
        }

        public int MisuUpdate(HIC_MISU_MST_SLIP item2, long gnJobSabun, string strROWID, string TxtDate, double TxtLtdCode, string TxtRemark, long FnMisuAmt)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   UPDATE              HIC_MISU_SLIP SET                       ");
            parameter.AppendSql("                       BDATE=TO_DATE(:TXTDATE, 'YYYY-MM-DD'),  ");
            parameter.AppendSql("                       LTDCODE = :TXTLTDCODE,                  ");
            parameter.AppendSql("                       SLIPAMT = :FNMISUAMT,                   ");
            parameter.AppendSql("                       REMARK = :TXTREMARK,                    ");
            parameter.AppendSql("                       ENTDATE = SYSDATE,                      ");
            parameter.AppendSql("                       ENTSABUN = :GNJOBSABUN                  ");

            parameter.AppendSql("   WHERE               ROWID = :STRROWID                       ");


            parameter.Add("TXTDATE",        TxtDate         );
            parameter.Add("TXTLTDCODE",     TxtLtdCode      );
            parameter.Add("FNMISUAMT",      FnMisuAmt       );
            parameter.Add("TXTREMARK",      TxtRemark       );
            parameter.Add("GNJOBSABUN",     gnJobSabun      );
            parameter.Add("STRROWID",       strROWID        );


            return ExecuteNonQuery(parameter);
        }

        public int MisuUpdate_New(HIC_MISU_MST_SLIP item3, string strROWID, long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE,      ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                            ");

            parameter.AppendSql("   SELECT              SYSDATE, :GNJOBSABUN,'3',WRTNO,BDATE,LTDCODE,                       ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                             ");

            parameter.AppendSql("   FROM                "+ ComNum.DB_PMPA +" HIC_MISU_SLIP                                  ");

            parameter.AppendSql("   WHERE               ROWID= :STRROWID                                                    ");


            parameter.Add("GNJOBSABUN",     gnJobSabun      );
            parameter.Add("STRROWID",       strROWID        );

            return ExecuteNonQuery(parameter);
        }

        public int insertOld(HIC_MISU_MST_SLIP item, string strROWID, long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE,          ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                                ");

            parameter.AppendSql("   SELECT              SYSDATE, :GNJOBSABUN,'2',WRTNO,BDATE,LTDCODE,                           ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                                 ");

            parameter.AppendSql("   FROM                "+ ComNum.DB_PMPA +" HIC_MISU_SLIP                                      ");

            parameter.AppendSql("   WHERE               ROWID= :STRROWID ");


            parameter.Add("GNJOBSABUN",     gnJobSabun      );
            parameter.Add("STRROWID",       strROWID        );

            return ExecuteNonQuery(parameter);
        }

        public int insertNew(HIC_MISU_MST_SLIP hmm, long nWrtno, string txtDate, long fnMisuAmt, long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE,          ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                                ");

            parameter.AppendSql("   SELECT              SYSDATE, :GNJOBSABUN,'1',WRTNO,BDATE,LTDCODE,                           ");
            parameter.AppendSql("                       GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                                 ");

            parameter.AppendSql("   FROM                " + ComNum.DB_PMPA + "HIC_MISU_SLIP                                     ");

            parameter.AppendSql("   WHERE               WRTNO= :NWRTNO                                                          ");
            parameter.AppendSql("                       AND BDATE <= TO_DATE(:TXTDATE,'YYYY-MM-DD')                             ");
            parameter.AppendSql("                       AND GEACODE = '11'                                                      ");
            parameter.AppendSql("                       AND SLIPAMT = :FNMISUAMT                                                ");


            parameter.Add("GNJOBSABUN",         gnJobSabun      );
            parameter.Add("NWRTNO",             nWrtno          );
            parameter.Add("TXTDATE",            txtDate         );
            parameter.Add("FNMISUAMT",          fnMisuAmt       );

            return ExecuteNonQuery(parameter);
        }

        public int GetMisuSlipUpdate(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT              INTO HIC_MISU_SLIP (                                                ");
            parameter.AppendSql("                       WRTNO,BDATE,LTDCODE,GEACODE,SLIPAMT,                                ");
            parameter.AppendSql("                       REMARK,ENTDATE,ENTSABUN                                             ");
            parameter.AppendSql("                       )VALUES(                                                            ");
            parameter.AppendSql("                       :WRTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), :LTDCODE, '11', :SLIPAMT,    ");
            parameter.AppendSql("                       :REMARK, SYSDATE, :ENTSABUN)                                        ");

            parameter.Add("WRTNO",      item.WRTNO          );
            parameter.Add("BDATE",      item.BDATE          );
            parameter.Add("LTDCODE",    item.LTDCODE        );
            parameter.Add("SLIPAMT",    item.SLIPAMT        );
            parameter.Add("REMARK",     item.REMARK         );
            parameter.Add("ENTSABUN",   item.ENTSABUN       );

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuCheck(long nWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                      ROWID                                   ");

            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_SLIP         ");

            parameter.AppendSql("   WHERE                       WRTNO = :NWRTNO                         ");
            parameter.AppendSql("                               AND GeaCode='11'                        "); // 미수발생

            parameter.Add("NWRTNO",         nWrtno      );

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public int GetMisuMaster_Update_2(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   UPDATE              HIC_MISU_MST SET");

            parameter.AppendSql("                       MIRGBN      =:MIRGBN,          ");
            parameter.AppendSql("                       GBEND       =:GBEND,           ");
            parameter.AppendSql("                       YYMM_JIN    =:YYMM_JIN,        ");
            parameter.AppendSql("                       MISUAMT     =:MISUAMT,         ");
            parameter.AppendSql("                       IPGUMAMT    =:IPGUMAMT,        ");
            parameter.AppendSql("                       GAMAMT      =:GAMAMT,          ");
            parameter.AppendSql("                       SAKAMT      =:SAKAMT,          ");
            parameter.AppendSql("                       BANAMT      =:BANAMT,          ");
            parameter.AppendSql("                       JANAMT      =:JANAMT           ");

            parameter.AppendSql("   WHERE               ROWID       =:RID               ");

            parameter.Add("MIRGBN",         item.MIRGBN             );
            parameter.Add("GBEND",          item.GBEND              );
            parameter.Add("YYMM_JIN",       item.YYMM_JIN           );
            parameter.Add("MISUAMT",        item.MISUAMT            );
            parameter.Add("IPGUMAMT",       item.IPGUMAMT           );
            parameter.Add("GAMAMT",         item.GAMAMT             );
            parameter.Add("SAKAMT",         item.SAKAMT             );
            parameter.Add("BANAMT",         item.BANAMT             );
            parameter.Add("JANAMT",         item.JANAMT             );
            parameter.Add("RID",            item.ROWID              );

            return ExecuteNonQuery(parameter);
        }

        public int GetMisuMaster_Update_1(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   UPDATE              HIC_MISU_MST SET LTDCODE=:LTDCODE,              ");
            parameter.AppendSql("                       JISA=:JISA,                                     ");
            parameter.AppendSql("                       KIHO=:KIHO,                                     ");
            parameter.AppendSql("                       MISUJONG=:MISUJONG,                             ");
            parameter.AppendSql("                       BDATE=TO_DATE(:BDATE,'YYYY-MM-DD'),             ");
            parameter.AppendSql("                       GJONG=:GJONG,                                   ");
            parameter.AppendSql("                       GBEND=:GBEND,                                   ");
            parameter.AppendSql("                       PUMMOK=:PUMMOK,                                 ");
            parameter.AppendSql("                       MISUGBN=:MISUGBN,                               ");
            parameter.AppendSql("                       GIRONO=:GIRONO,                                 ");
            parameter.AppendSql("                       CNO=:CNO,                                       ");
            parameter.AppendSql("                       DAMNAME=:DAMNAME,                               ");
            parameter.AppendSql("                       REMARK=:REMARK,                                 ");
            parameter.AppendSql("                       MIRGBN=:MIRGBN,                                 ");
            parameter.AppendSql("                       MIRCHAAMT=:MIRCHAAMT,                           ");
            parameter.AppendSql("                       MIRCHAREMARK=:MIRCHAREMARK,                     ");

            if (!item.MIRCHADATE.IsNullOrEmpty())
            {
                parameter.AppendSql("                   MIRCHADATE=TO_DATE(:MIRCHADATE,'YYYY-MM-DD'),   ");
            }

            parameter.AppendSql("                       YYMM_JIN=:YYMM_JIN,                             ");
            parameter.AppendSql("                       ENTDATE=SYSDATE,                                ");
            parameter.AppendSql("                       ENTSABUN=:ENTSABUN,                             ");
            parameter.AppendSql("                       MISUAMT=:MISUAMT,                               ");
            parameter.AppendSql("                       IPGUMAMT=:IPGUMAMT,                             ");
            parameter.AppendSql("                       GAMAMT=:GAMAMT,                                 ");
            parameter.AppendSql("                       SAKAMT=:SAKAMT,                                 ");
            parameter.AppendSql("                       BANAMT=:BANAMT,                                 ");
            parameter.AppendSql("                       JANAMT=:JANAMT                                  ");

            parameter.AppendSql("   WHERE               ROWID=:RID                                      ");

            parameter.Add("LTDCODE",        item.LTDCODE                                                );
            parameter.Add("JISA",           item.JISA, Oracle.ManagedDataAccess.Client.OracleDbType.Char       );
            parameter.Add("KIHO",           item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char       );
            parameter.Add("MISUJONG",       item.MISUJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char   );
            parameter.Add("BDATE",          item.BDATE                                                  );
            parameter.Add("GJONG",          item.GJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char      );
            parameter.Add("GBEND",          item.GBEND, Oracle.ManagedDataAccess.Client.OracleDbType.Char      );
            parameter.Add("PUMMOK",         item.PUMMOK                                                 );
            parameter.Add("MISUGBN",        item.MISUGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char    );
            parameter.Add("GIRONO",         item.GIRONO                                                 );
            parameter.Add("CNO",            item.CNO                                                    );
            parameter.Add("DAMNAME",        item.DAMNAME                                                );
            parameter.Add("REMARK",         item.REMARK                                                 );
            parameter.Add("MIRGBN",         item.MIRGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char     );
            parameter.Add("MIRCHAAMT",      item.MIRCHAAMT                                              );
            parameter.Add("MIRCHAREMARK",   item.MIRCHAREMARK                                           );

            if (!item.MIRCHADATE.IsNullOrEmpty())
            {
                parameter.Add("MIRCHADATE", item.MIRCHADATE                                             );
            }
            
            parameter.Add("YYMM_JIN",       item.YYMM_JIN, Oracle.ManagedDataAccess.Client.OracleDbType.Char   );
            parameter.Add("ENTSABUN",       item.ENTSABUN                                               );
            parameter.Add("MISUAMT",        item.MISUAMT                                                );
            parameter.Add("IPGUMAMT",       item.IPGUMAMT                                               );
            parameter.Add("GAMAMT",         item.GAMAMT                                                 );
            parameter.Add("SAKAMT",         item.SAKAMT                                                 );
            parameter.Add("BANAMT",         item.BANAMT                                                 );
            parameter.Add("JANAMT",         item.JANAMT                                                 );
            parameter.Add("RID",            item.ROWID                                                  );

            return ExecuteNonQuery(parameter);
        }
        
        public int GetMisuMaster_Update(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("   INSERT INTO             HIC_MISU_MST (                                                                                    ");
            parameter.AppendSql("                           WRTNO,LTDCODE,JISA,KIHO,MISUJONG,PUMMOK,BDATE,GJONG,                                              ");
            parameter.AppendSql("                           GBEND,MISUGBN,MISUAMT,IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT,                                       ");
            parameter.AppendSql("                           GIRONO,DAMNAME,REMARK,MIRGBN,MIRCHAAMT,MIRCHAREMARK,                                              ");
            parameter.AppendSql("                           YYMM_JIN, ENTDATE,ENTSABUN,CNO                                                                    ");
            if (!item.MIRCHADATE.IsNullOrEmpty())
            {
                parameter.AppendSql(" ,MIRCHADATE ");
            }
            parameter.AppendSql("                           ) VALUES (                                                                                        ");
            parameter.AppendSql("                           :WRTNO, :LTDCODE, :JISA, :KIHO, :MISUJONG, :PUMMOK,                                               ");
            parameter.AppendSql("                           TO_DATE(:BDATE,'YYYY-MM-DD'),                                                                     ");
            parameter.AppendSql("                           :GJONG, :GBEND, :MISUGBN, :MISUAMT, :IPGUMAMT,                                                    ");
            parameter.AppendSql("                           :GAMAMT, :SAKAMT, :BANAMT, :JANAMT, :GIRONO,                                                      ");
            parameter.AppendSql("                           :DAMNAME, :REMARK, :MIRGBN, :MIRCHAAMT, :MIRCHAREMARK,                                            ");
            parameter.AppendSql("                           :YYMM_JIN, SYSDATE, :ENTSABUN, :CNO                                                               ");

            if (!item.MIRCHADATE.IsNullOrEmpty())
            {
                parameter.AppendSql("                           ,TO_DATE(:MIRCHADATE,'YYYY-MM-DD')                                                            ");
            }

            parameter.AppendSql("                           )                                                                                                   ");

            parameter.Add("WRTNO",          item.WRTNO                                                      );
            parameter.Add("LTDCODE",        item.LTDCODE                                                    );
            parameter.Add("JISA",           item.JISA, Oracle.ManagedDataAccess.Client.OracleDbType.Char           );
            parameter.Add("KIHO",           item.KIHO, Oracle.ManagedDataAccess.Client.OracleDbType.Char           );
            parameter.Add("MISUJONG",       item.MISUJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char       );
            parameter.Add("PUMMOK",         item.PUMMOK                                                     );
            parameter.Add("BDATE",          item.BDATE                                                      );
            parameter.Add("GJONG",          item.GJONG, Oracle.ManagedDataAccess.Client.OracleDbType.Char          );
            parameter.Add("GBEND",          item.GBEND, Oracle.ManagedDataAccess.Client.OracleDbType.Char          );
            parameter.Add("MISUGBN",        item.MISUGBN, Oracle.ManagedDataAccess.Client.OracleDbType.Char        );
            parameter.Add("MISUAMT",        item.MISUAMT                                                    );
            parameter.Add("IPGUMAMT",       item.IPGUMAMT                                                   );
            parameter.Add("GAMAMT",         item.GAMAMT                                                     );
            parameter.Add("SAKAMT",         item.SAKAMT                                                     );
            parameter.Add("BANAMT",         item.BANAMT                                                     );
            parameter.Add("JANAMT",         item.JANAMT                                                     );
            parameter.Add("GIRONO",         item.GIRONO                                                     );
            parameter.Add("DAMNAME",        item.DAMNAME                                                    );
            parameter.Add("REMARK",         item.REMARK                                                     );
            parameter.Add("MIRGBN",         item.MIRGBN ,Oracle.ManagedDataAccess.Client.OracleDbType.Char         );
            parameter.Add("MIRCHAAMT",      item.MIRCHAAMT                                                  );
            parameter.Add("MIRCHAREMARK",   item.MIRCHAREMARK                                               );
            parameter.Add("YYMM_JIN",       item.YYMM_JIN, Oracle.ManagedDataAccess.Client.OracleDbType.Char       );
            parameter.Add("ENTSABUN",       item.ENTSABUN                                                   );
            parameter.Add("CNO",            item.CNO                                                        );

            if (!item.MIRCHADATE.IsNullOrEmpty())
            {
                parameter.Add("MIRCHADATE", item.MIRCHADATE                                                 );
            }

            return ExecuteNonQuery(parameter);
        }

        public int HistoryInsert(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_SLIP (                                                             ");
            parameter.AppendSql("                       WRTNO, BDATE, LTDCODE, GEACODE, SLIPAMT, REMARK, ENTDATE, ENTSABUN          ");
            parameter.AppendSql("                       ) VALUES (                                                                  ");
            parameter.AppendSql("                       :WRTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), :LTDCODE, :GEACODE,                  ");
            parameter.AppendSql("                       :SLIPAMT, :REMARK, SYSDATE, :ENTSABUN )                                     ");

            parameter.Add("WRTNO",              item.WRTNO                                                 );
            parameter.Add("BDATE",              item.BDATE                                                 );
            parameter.Add("LTDCODE",            item.LTDCODE                                               );
            parameter.Add("GEACODE",            item.GEACODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char   );
            parameter.Add("SLIPAMT",            item.SLIPAMT                                               );
            parameter.Add("REMARK",             item.REMARK                                                );
            parameter.Add("ENTSABUN",           item.ENTSABUN                                              );

            return ExecuteNonQuery(parameter);
        }

        public int HistoryUpdate(HIC_MISU_MST_SLIP item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   UPDATE              HIC_MISU_SLIP SET BDATE=TO_DATE(:BDATE,'YYYY-MM-DD'),       ");
            parameter.AppendSql("                       GEACODE=:GEACODE,                                           ");
            parameter.AppendSql("                       SLIPAMT=:SLIPAMT,                                           ");
            parameter.AppendSql("                       REMARK=:REMARK,                                             ");
            parameter.AppendSql("                       ENTDATE=SYSDATE,                                            ");
            parameter.AppendSql("                       ENTSABUN=:ENTSABUN                                          ");
            parameter.AppendSql("   WHERE               ROWID=:RID                                                  ");

            parameter.Add("BDATE",          item.BDATE                                                  );
            parameter.Add("GEACODE",        item.GEACODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char    );
            parameter.Add("SLIPAMT",        item.SLIPAMT                                                );
            parameter.Add("REMARK",         item.REMARK                                                 );
            parameter.Add("ENTSABUN",       item.ENTSABUN                                               );
            parameter.Add("RID",            item.ROWID                                                  );

            return ExecuteNonQuery(parameter);
        }

        public int HistoryIn(string strROWID, long GnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   INSERT INTO         HIC_MISU_HISTORY (JOBDATE,JOBSABUN,JOBGBN,WRTNO,BDATE,LTDCODE   ");
            parameter.AppendSql("                       ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN)                       ");

            parameter.AppendSql("   SELECT              SYSDATE, :GNJOBSABUN,'2',WRTNO,BDATE,LTDCODE                    ");
            parameter.AppendSql("                       ,GEACODE,SLIPAMT,REMARK,ENTDATE,ENTSABUN                        ");
                
            parameter.AppendSql("   FROM                ADMIN.HIC_MISU_SLIP                                       ");

            parameter.AppendSql("   WHERE               ROWID= :STRROWID                                                ");

            parameter.Add("STRROWID",       strROWID        );
            parameter.Add("GNJOBSABUN",     GnJobSabun      );

            return ExecuteNonQuery(parameter);
        }

        public int GetDelHistory(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   DELETE HIC_MISU_SLIP        ");
            parameter.AppendSql("   WHERE ROWID= :STRROWID      ");

            parameter.Add("STRROWID", strROWID                  );

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuDate(long txtWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  TO_CHAR(BDATE,'YYYY-MM-DD') BDATE       ");

            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_SLIP     ");

            parameter.AppendSql("   WHERE                   WRTNO = :NWRTNO                         ");
            parameter.AppendSql("   ORDER BY                BDATE DESC                              ");

            parameter.Add("NWRTNO",         txtWrtNo            );

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuSlip(long txtWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT              TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,GEACODE,SLIPAMT, REMARK,ROWID         ");

            parameter.AppendSql("   FROM                " + ComNum.DB_PMPA + "HIC_MISU_SLIP                                     ");

            parameter.AppendSql("   WHERE               WRTNO = :NWRTNO                                                         ");
            parameter.AppendSql("   ORDER BY            BDATE, GEACODE                                                          ");

            parameter.Add("NWRTNO",     txtWrtNo            );

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }

        public List<HIC_MISU_MST_SLIP> GetMisuMaster(long txtWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  LTDCODE,JISA,KIHO,MISUJONG,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,GBEND,         ");
            parameter.AppendSql("                           GJONG,MISUGBN,MISUAMT,IPGUMAMT,GAMAMT,PUMMOK,MIRCHAAMT,MIRCHAREMARK,        ");
            parameter.AppendSql("                           SAKAMT,BANAMT,JANAMT,GIRONO,DAMNAME,REMARK,MIRGBN,GBMISUBUILD,              ");
            parameter.AppendSql("                           GBMISUBUILD2,GBMISUBUILD3,GBMISUBUILD4,                                     ");
            parameter.AppendSql("                           TO_CHAR(MIRCHADATE,'YYYY-MM-DD') MIRCHADATE,YYMM_JIN, ROWID                 ");


            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_MST                                          ");

            parameter.AppendSql("   WHERE                   WRTNO = :NWRTNO                                                             ");

            parameter.Add("NWRTNO", txtWrtNo);

            return ExecuteReader<HIC_MISU_MST_SLIP>(parameter);
        }
    }
}
