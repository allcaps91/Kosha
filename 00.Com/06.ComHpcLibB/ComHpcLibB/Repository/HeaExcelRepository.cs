namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaExcelRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaExcelRepository()
        {
        }

        public int Update(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HEA_EXCEL SET     ");
            parameter.AppendSql("        MEMO         = :MEMO          ");
            parameter.AppendSql("      , AES_JUMIN    = :AES_JUMIN     ");
            parameter.AppendSql("      , BIRTH        = :BIRTH         ");
            if (!item.SNAME.IsNullOrEmpty())
            {
                parameter.AppendSql("      , SNAME        = :SNAME      ");
            }
            parameter.AppendSql("      , HPHONE       = :HPHONE        ");
            if (!item.TEL.IsNullOrEmpty())
            {
                parameter.AppendSql("      , TEL          = :TEL       ");
            }
            parameter.AppendSql("      , UPDATETIME   = SYSDATE        ");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER  ");
            parameter.AppendSql("  WHERE ROWID     = :RID              ");

            parameter.Add("MEMO", item.MEMO);
            parameter.Add("AES_JUMIN", item.AES_JUMIN);
            parameter.Add("BIRTH", item.BIRTH);
            if (!item.SNAME.IsNullOrEmpty())
            {
                parameter.Add("SNAME", item.SNAME);
            }
            parameter.Add("HPHONE", item.HPHONE);
            if (!item.TEL.IsNullOrEmpty())
            {
                parameter.Add("TEL", item.TEL);
            }
            parameter.Add("MODIFIEDUSER", item.MODIFIEDUSER);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public string GetMemobyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MEMO                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL   ");
            parameter.AppendSql(" WHERE ROWID = :RID            ");

            parameter.Add("RID", strROWID);

            return ExecuteScalar<string>(parameter);
        }

        public List<HEA_EXCEL> GetListByYear(string argYYMM)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT LTDCODE, KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE) LTDNAME ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL                                ");
            parameter.AppendSql(" WHERE YEAR = :YEAR                                         ");
            parameter.AppendSql(" GROUP BY KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE), LTDCODE      ");
            parameter.AppendSql(" ORDER BY KOSMOS_PMPA.FC_HIC_LTDNAME(LTDCODE), LTDCODE      ");

            parameter.Add("YEAR", argYYMM, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HEA_EXCEL>(parameter);
        }

        public void UpDate(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HEA_EXCEL          ");
            parameter.AppendSql("    SET LTDBUSE       = :LTDBUSE       ");
            parameter.AppendSql("      , JNAME         = :JNAME         ");
            parameter.AppendSql("      , REL           = :REL           ");
            parameter.AppendSql("      , SNAME         = :SNAME         ");
            parameter.AppendSql("      , AES_JUMIN     = :AES_JUMIN     ");
            parameter.AppendSql("      , HPHONE        = :HPHONE        ");
            parameter.AppendSql("      , GJTYPE        = :GJTYPE        ");
            parameter.AppendSql("      , HDATE         = :HDATE         ");
            parameter.AppendSql("      , AMPM          = :AMPM          ");
            parameter.AppendSql("      , GBNHIC        = :GBNHIC        ");
            parameter.AppendSql("      , GBLTDADDEXAM  = :GBLTDADDEXAM  ");
            parameter.AppendSql("      , LTDADDEXAM    = :LTDADDEXAM    ");
            parameter.AppendSql("      , BONINADDEXAM  = :BONINADDEXAM  ");
            parameter.AppendSql("      , MCODES        = :MCODES        ");
            parameter.AppendSql("      , HOSPITAL      = :HOSPITAL      ");
            parameter.AppendSql("      , BIRTH         = :BIRTH         ");
            parameter.AppendSql("      , REMARK        = :REMARK        ");
            parameter.AppendSql("      , RDATE         = :RDATE         ");
            parameter.AppendSql("      , PTNO          = :PTNO          ");
            parameter.AppendSql("      , GBSAMU        = :GBSAMU        ");
            parameter.AppendSql("      , GBNIGHT       = :GBNIGHT       ");
            parameter.AppendSql("      , TEL           = :TEL           ");
            parameter.AppendSql("      , LTDSABUN      = :LTDSABUN      ");
            parameter.AppendSql("      , JIKNAME       = :JIKNAME       ");
            parameter.AppendSql("      , IPSADATE      = :IPSADATE      ");
            parameter.AppendSql("      , GKIHO         = :GKIHO         ");
            parameter.AppendSql("      , JUSO          = :JUSO          ");
            parameter.AppendSql("      , NHICINFO      = :NHICINFO      ");
            parameter.AppendSql("      , UPDATETIME    = SYSDATE        ");
            parameter.AppendSql("      , MODIFIEDUSER  = :MODIFIEDUSER  "); 
            parameter.AppendSql(" WHERE ROWID          = :RID           ");

            #region º¯¼ö ¸ÅÄª
            parameter.Add("LTDBUSE",        item.LTDBUSE);
            parameter.Add("JNAME",          item.JNAME);
            parameter.Add("REL",            item.REL);
            parameter.Add("SNAME",          item.SNAME);
            parameter.Add("AES_JUMIN",      item.AES_JUMIN);
            parameter.Add("HPHONE",         item.HPHONE);
            parameter.Add("GJTYPE",         item.GJTYPE);
            parameter.Add("HDATE",          item.HDATE);
            parameter.Add("AMPM",           item.AMPM);
            parameter.Add("GBNHIC",         item.GBNHIC);
            parameter.Add("GBLTDADDEXAM",   item.GBLTDADDEXAM);
            parameter.Add("LTDADDEXAM",     item.LTDADDEXAM);
            parameter.Add("BONINADDEXAM",   item.BONINADDEXAM);
            parameter.Add("MCODES",         item.MCODES);
            parameter.Add("HOSPITAL",       item.HOSPITAL);
            parameter.Add("BIRTH",          item.BIRTH);
            parameter.Add("REMARK",         item.REMARK);
            parameter.Add("RDATE",          item.RDATE);
            parameter.Add("PTNO",           item.PTNO);
            parameter.Add("GBSAMU",         item.GBSAMU);
            parameter.Add("GBNIGHT",        item.GBNIGHT);
            parameter.Add("TEL",            item.TEL);
            parameter.Add("LTDSABUN",       item.LTDSABUN);
            parameter.Add("JIKNAME",        item.JIKNAME);
            parameter.Add("IPSADATE",       item.IPSADATE);
            parameter.Add("GKIHO",          item.GKIHO);
            parameter.Add("JUSO",           item.JUSO);
            parameter.Add("NHICINFO",       item.NHICINFO);
            parameter.Add("MODIFIEDUSER",   item.MODIFIEDUSER); 
            parameter.Add("RID",            item.RID);
            #endregion

            ExecuteNonQuery(parameter);
        }

        public void UpdateExcel(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HEA_EXCEL          ");
            parameter.AppendSql("    SET AES_JUMIN     =:AES_JUMIN      ");
            parameter.AppendSql("      , RDATE         =:RDATE          ");
            parameter.AppendSql("      , SDATE         =:SDATE          ");
            parameter.AppendSql("      , PANO          =:PANO           ");
            parameter.AppendSql("      , PTNO          =:PTNO           ");
            parameter.AppendSql("      , UPDATETIME    = SYSDATE        ");
            parameter.AppendSql("      , MODIFIEDUSER  = :MODIFIEDUSER  ");
            parameter.AppendSql("  WHERE ROWID         =:RID            ");

            #region º¯¼ö ¸ÅÄª
            
            parameter.Add("AES_JUMIN", item.AES_JUMIN);
            parameter.Add("RDATE", item.RDATE);
            parameter.Add("SDATE", item.SDATE);
            parameter.Add("PANO", item.PANO);
            parameter.Add("PTNO", item.PTNO);
            parameter.Add("MODIFIEDUSER", item.MODIFIEDUSER);
            parameter.Add("RID", item.RID);
            #endregion

            ExecuteNonQuery(parameter);
        }

        public HEA_EXCEL GetItemByJuminYear(string argJumin, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YEAR, LTDCODE, LTDBUSE, LTDSABUN, JIKNAME, JNAME, REL                   ");
            parameter.AppendSql("     , SNAME, PANO, AES_JUMIN, SEX, AGE, HPHONE, TEL, GJTYPE, GKIHO            ");
            parameter.AppendSql("     , IPSADATE, GBSAMU, GBNIGHT, GBNONHIC, HOSPITAL, MCODES                   ");
            parameter.AppendSql("     , GBLTDADDEXAM, BONINADDEXAM, LTDADDEXAM, GAJOKADDEXAM, JUSO              ");
            parameter.AppendSql("     , HDATE, AMPM, RDATE, SDATE, REMARK, BIRTH, GBNHIC, GBJUMIN, PTNO         ");
            parameter.AppendSql("     , NHICINFO, ENTSABUN, ENTDATE, MEMO, LTDCODE2, PANO2                      ");
            parameter.AppendSql("     , TO_CHAR(UPDATETIME, 'YYYY-MM-DD HH24:MI:SS') UPDATETIME, MODIFIEDUSER   ");
            parameter.AppendSql("     , ROWID AS RID   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL                                                   ");
            parameter.AppendSql(" WHERE AES_JUMIN = :JUMIN                                                      ");
            parameter.AppendSql("   AND YEAR = :YEAR                                                            ");

            parameter.Add("JUMIN", argJumin);
            parameter.Add("YEAR", argYear);

            return ExecuteReaderSingle<HEA_EXCEL>(parameter);
        }

        public HEA_EXCEL GetItemByLtdsabunYear(string argLtdsabun, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YEAR, LTDCODE, LTDBUSE, LTDSABUN, JIKNAME, JNAME, REL                   ");
            parameter.AppendSql("     , SNAME, PANO, AES_JUMIN, SEX, AGE, HPHONE, TEL, GJTYPE, GKIHO            ");
            parameter.AppendSql("     , IPSADATE, GBSAMU, GBNIGHT, GBNONHIC, HOSPITAL, MCODES                   ");
            parameter.AppendSql("     , GBLTDADDEXAM, BONINADDEXAM, LTDADDEXAM, GAJOKADDEXAM, JUSO              ");
            parameter.AppendSql("     , HDATE, AMPM, RDATE, SDATE, REMARK, BIRTH, GBNHIC, GBJUMIN, PTNO         ");
            parameter.AppendSql("     , NHICINFO, ENTSABUN, ENTDATE, MEMO, LTDCODE2, PANO2                      ");
            parameter.AppendSql("     , TO_CHAR(UPDATETIME, 'YYYY-MM-DD HH24:MI:SS') UPDATETIME, MODIFIEDUSER   ");
            parameter.AppendSql("     , ROWID AS RID   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL                                                   ");
            parameter.AppendSql(" WHERE LTDSABUN = :LTDSABUN                                                      ");
            parameter.AppendSql("   AND YEAR = :YEAR                                                            ");

            parameter.Add("LTDSABUN", argLtdsabun);
            parameter.Add("YEAR", argYear);

            return ExecuteReaderSingle<HEA_EXCEL>(parameter);
        }

        public void Delete(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" DELETE KOSMOS_PMPA.HEA_EXCEL WHERE ROWID =:RID  ");
            parameter.Add("RID", item.RID);
            ExecuteNonQuery(parameter);
        }

        public void Insert(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.HEA_EXCEL (                                                           ");
            parameter.AppendSql("        YEAR,LTDCODE,LTDBUSE,LTDSABUN,JIKNAME,JNAME,REL,SNAME,PANO,AES_JUMIN                   ");
            parameter.AppendSql("       ,SEX,AGE,HPHONE,TEL,GJTYPE,GKIHO,IPSADATE,GBSAMU,GBNIGHT,GBNHIC,GBLTDADDEXAM            ");
            parameter.AppendSql("       ,LTDADDEXAM,BONINADDEXAM,JUSO,HDATE,AMPM,HOSPITAL,MCODES,REMARK,BIRTH,PTNO              ");
            parameter.AppendSql("       ,ENTSABUN,ENTDATE                                                                       ");
            parameter.AppendSql(") VALUES (                                                                                     ");
            parameter.AppendSql("       :YEAR,:LTDCODE,:LTDBUSE,:LTDSABUN,:JIKNAME,:JNAME,:REL,:SNAME,:PANO,:AES_JUMIN          ");
            parameter.AppendSql("       ,:SEX,:AGE,:HPHONE,:TEL,:GJTYPE,:GKIHO,:IPSADATE,:GBSAMU,:GBNIGHT,:GBNHIC,:GBLTDADDEXAM ");
            parameter.AppendSql("       ,:LTDADDEXAM,:BONINADDEXAM,:JUSO,:HDATE,:AMPM,:HOSPITAL,:MCODES,:REMARK,:BIRTH,:PTNO    ");
            parameter.AppendSql("       ,:ENTSABUN,SYSDATE                                                                      ");
            parameter.AppendSql(")                                                                                              ");

            parameter.Add("YEAR",           item.YEAR);
            parameter.Add("LTDCODE",        item.LTDCODE);
            parameter.Add("LTDBUSE",        item.LTDBUSE);
            parameter.Add("LTDSABUN",       item.LTDSABUN);
            parameter.Add("JIKNAME",        item.JIKNAME);
            parameter.Add("JNAME",          item.JNAME);
            parameter.Add("REL",            item.REL);
            parameter.Add("SNAME",          item.SNAME);
            parameter.Add("AES_JUMIN",      item.AES_JUMIN);
            parameter.Add("SEX",            item.SEX);
            parameter.Add("AGE",            item.AGE);
            parameter.Add("HPHONE",         item.HPHONE);
            parameter.Add("TEL",            item.TEL);
            parameter.Add("GJTYPE",         item.GJTYPE);
            parameter.Add("GKIHO",          item.GKIHO);
            parameter.Add("IPSADATE",       item.IPSADATE);
            parameter.Add("GBSAMU",         item.GBSAMU);
            parameter.Add("GBNIGHT",        item.GBNIGHT);
            parameter.Add("GBNHIC",         item.GBNHIC);
            parameter.Add("GBLTDADDEXAM",   item.GBLTDADDEXAM);
            parameter.Add("LTDADDEXAM",     item.LTDADDEXAM);
            parameter.Add("BONINADDEXAM",   item.BONINADDEXAM);
            parameter.Add("JUSO",           item.JUSO);
            parameter.Add("HDATE",          item.HDATE);
            parameter.Add("AMPM",           item.AMPM);
            parameter.Add("HOSPITAL",       item.HOSPITAL);
            parameter.Add("MCODES",         item.MCODES);
            parameter.Add("REMARK",         item.REMARK);
            parameter.Add("BIRTH",          item.BIRTH);
            parameter.Add("PTNO",           item.PTNO);
            parameter.Add("ENTSABUN",       item.ENTSABUN);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidbyItem(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID AS RID                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL                               ");
            parameter.AppendSql(" WHERE YEAR = :YEAR                                        ");

            if (!item.BIRTH.IsNullOrEmpty()) { parameter.AppendSql("  AND BIRTH = :BIRTH               "); }
            if (!item.AES_JUMIN.IsNullOrEmpty()) { parameter.AppendSql("  AND AES_JUMIN = :JUMIN       "); }
            if (!item.LTDSABUN.IsNullOrEmpty()) { parameter.AppendSql("  AND LTDSABUN = :LTDSABUN      "); }

            parameter.AppendSql("  AND SNAME = :SNAME                                       ");
            parameter.AppendSql("  AND LTDCODE = :LTDCODE                                   ");

            parameter.Add("YEAR", item.YEAR);

            if (!item.BIRTH.IsNullOrEmpty()) { parameter.Add("BIRTH", item.BIRTH); }
            if (!item.AES_JUMIN.IsNullOrEmpty()) { parameter.Add("AES_JUMIN", item.AES_JUMIN); }
            if (!item.LTDSABUN.IsNullOrEmpty()) { parameter.Add("LTDSABUN", item.LTDSABUN); }

            parameter.Add("SNAME", item.SNAME);
            parameter.Add("LTDCODE", item.LTDCODE);

            return ExecuteScalar<string>(parameter);
        }

        public int Update_RDate(HEA_EXCEL item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_PMPA.HEA_EXCEL SET                      ");
            parameter.AppendSql("        RDATE        = TO_DATE(:RDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("      , SEX          = :SEX                            ");
            parameter.AppendSql("      , AGE          = :AGE                            ");
            parameter.AppendSql("      , UPDATETIME   = SYSDATE                         ");
            parameter.AppendSql("      , MODIFIEDUSER = :MODIFIEDUSER                   ");
            parameter.AppendSql("  WHERE ROWID = :RID                                   ");

            parameter.Add("RDATE", item.RDATE);
            parameter.Add("SEX", item.SEX, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("AGE", item.AGE);
            parameter.Add("MODIFIEDUSER", item.MODIFIEDUSER); 
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public HEA_EXCEL GetAllbyRowId(string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YEAR, LTDCODE, LTDBUSE, LTDSABUN, JIKNAME, JNAME, REL                   ");
            parameter.AppendSql("     , SNAME, PANO, AES_JUMIN, SEX, AGE, HPHONE, TEL, GJTYPE, GKIHO            ");
            parameter.AppendSql("     , IPSADATE, GBSAMU, GBNIGHT, GBNONHIC, HOSPITAL, MCODES                   ");
            parameter.AppendSql("     , GBLTDADDEXAM, BONINADDEXAM, LTDADDEXAM, GAJOKADDEXAM, JUSO              ");
            parameter.AppendSql("     , HDATE, AMPM, RDATE, SDATE, REMARK, BIRTH, GBNHIC, GBJUMIN, PTNO         ");
            parameter.AppendSql("     , NHICINFO, ENTSABUN, ENTDATE, MEMO, LTDCODE2, PANO2                      ");
            parameter.AppendSql("     , TO_CHAR(UPDATETIME, 'YYYY-MM-DD HH24:MI:SS') UPDATETIME, MODIFIEDUSER   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL                                                   ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                            ");

            parameter.Add("RID", strROWID);

            return ExecuteReaderSingle<HEA_EXCEL>(parameter);
        }

        public List<HEA_EXCEL> GetRowIdbyLtdCode(string strYear, string strLtdCode)
        {
            long nLtdCode = 0;

            nLtdCode = long.Parse(VB.Pstr(strLtdCode, ".", 1));

            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL   ");
            parameter.AppendSql(" WHERE YEAR = :YEAR            ");
            if (VB.Left(strLtdCode, 5) != "*****")
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE  ");
            }

            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteReader<HEA_EXCEL>(parameter);
        }

        public List<HEA_EXCEL> GetAll(string strYear, long nLtd, string strSname, string strBirth, string strNoRsv)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT YEAR, LTDCODE, LTDBUSE, LTDSABUN, JIKNAME, JNAME, REL                   ");
            parameter.AppendSql("     , SNAME, PANO, AES_JUMIN, SEX, AGE, HPHONE, TEL, GJTYPE                   ");
            parameter.AppendSql("     , GKIHO, IPSADATE, GBSAMU, GBNIGHT, GBNONHIC, HOSPITAL                    ");
            parameter.AppendSql("     , MCODES, GBLTDADDEXAM, BONINADDEXAM, LTDADDEXAM                          ");
            parameter.AppendSql("     , GAJOKADDEXAM, JUSO, HDATE, AMPM, RDATE, SDATE, REMARK                   ");
            parameter.AppendSql("     , BIRTH, GBNHIC, GBJUMIN, PTNO, NHICINFO, ENTSABUN                        ");
            parameter.AppendSql("     , TO_CHAR(ENTDATE, 'MM/DD') ENTDATE, MEMO, LTDCODE2, PANO2                ");
            parameter.AppendSql("     , TO_CHAR(UPDATETIME, 'YYYY-MM-DD HH24:MI:SS') UPDATETIME, MODIFIEDUSER   ");
            parameter.AppendSql("     , TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI:SS') ENTDATE1, ROWID AS RID        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_EXCEL                                                   ");
            parameter.AppendSql(" WHERE YEAR = :YEAR                                                            ");
            if (nLtd > 0)
            {
                parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                  ");
            }
            if (strSname != "")
            {
                parameter.AppendSql("   AND SNAME like :SNAME                                                   ");
            }
            if (strBirth != "")
            {
                parameter.AppendSql("   AND BIRTH = :BIRTH                                                      ");
            }
            if (strNoRsv == "Y")
            {
                parameter.AppendSql("   AND RDATE IS NULL                                                       ");
            }
            parameter.AppendSql(" ORDER BY SNAME, BIRTH                                                         ");

            parameter.Add("YEAR", strYear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (nLtd != 0)
            {
                parameter.Add("LTDCODE", nLtd);
            }

            if (strSname != "")
            {
                parameter.Add("SNAME", strSname);
            }
                
            if (strBirth != "")
            { 
                parameter.Add("BIRTH", strBirth, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HEA_EXCEL>(parameter);
        }
    }
}
