namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasPatientRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public BasPatientRepository()
        {
        }

        public IList<BAS_PATIENT> GetPatientByJuminNo(string argJumin1, string argJumin2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,SNAME,ZIPCODE1,ZIPCODE2,ZIPCODE3,JUSO,BUILDNO,GKIHO,SEX     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND JUMIN1 = :JUMIN1                                        ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3                                        ");
            parameter.AppendSql("   AND SUBSTR(PANO,1,1) <> '9'                                 ");
            parameter.AppendSql("   AND (SNAME IS NULL OR  SNAME <> '¿Ã¡ﬂ√≠∆Æ')                 ");

            parameter.Add("JUMIN1", argJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", argJumin2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<BAS_PATIENT>(parameter);
        }

        public BAS_PATIENT GetPaNobySName(string argSName, string argBirth, string argHPhone)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND SNAME  = :SNAME                                     ");
            }
            if (!argBirth.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND JUMIN1 = :JUMIN1                                    ");
            }
            if (!argHPhone.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND HPHONE = :HPHONE                                    ");
            }

            if (!argSName.IsNullOrEmpty())
            {
                parameter.Add("SNAME", argSName);
            }
            if (!argBirth.IsNullOrEmpty())
            {
                parameter.Add("JUMIN1", argBirth, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }
            if (!argHPhone.IsNullOrEmpty())
            {
                parameter.Add("HPHONE", argHPhone);
            }

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public BAS_PATIENT GetPaNoRowIdbyPaNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Pano, ROWID RID                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                 ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");

            parameter.Add("PANO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public string GetJuminbyPaNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JUMIN1                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                 ");
            parameter.AppendSql(" WHERE PANO = :PANO                            ");

            parameter.Add("PANO", strPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string GetPaNobyJumin1(string fstrJumin1, string fstrJumin2, string fstrJumin3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                         ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                                                ");
            parameter.AppendSql("   AND ((Jumin3 IS NULL AND JUMIN2 = :JUMIN2) OR JUMIN3 = :JUMIN3)     ");

            parameter.Add("JUMIN1", fstrJumin1);
            parameter.Add("JUMIN2", fstrJumin2);
            parameter.Add("JUMIN3", fstrJumin3);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateSNameHphoneTelbyRowId(string strSname, string strHPhone, string strTel, string strROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.BAS_PATIENT SET     ");
            parameter.AppendSql("       SNAME  = :SNAME                 ");
            parameter.AppendSql("     , HPHONE = :HPHONE                ");
            parameter.AppendSql("     , TEL    = :TEL                   ");
            parameter.AppendSql(" WHERE ROWID  = :RID                   ");

            parameter.Add("SNAME", strSname);
            parameter.Add("HPHONE", strHPhone);
            parameter.Add("TEL", strTel);
            parameter.Add("RID", strROWID);

            return ExecuteNonQuery(parameter);
        }

        public BAS_PATIENT GetPaNoRowIdbyJumin1Jumin3(string strJumin1, string strJumin3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Pano, ROWID RID                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                 ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                        ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3                        ");

            parameter.Add("JUMIN1", strJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", strJumin3);

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public BAS_PATIENT GetItembyJumin1Jumin3NotInSName(string strJumin1, string strJumin2, List<string> b04_NOT_PATIENT)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SNAME, TEL, PANO, HPHONE, SEX, JUSO, ROADDETAIL           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                 ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                        ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3                        ");
            if (b04_NOT_PATIENT.Count > 0)
            {
                parameter.AppendSql("   AND SNAME NOT IN (:SNAME)               ");
            }

            parameter.Add("JUMIN1", strJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", strJumin2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (b04_NOT_PATIENT.Count > 0)
            {
                parameter.AddInStatement("SNAME", b04_NOT_PATIENT);
            }

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public List<BAS_PATIENT> GetListByJuminNo(string argJumin1, string argJumin2, List<string> b04_NOT_PATIENT)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO,SNAME,SEX,JUMIN1,JUMIN2,STARTDATE,LASTDATE,ZIPCODE1,ZIPCODE2                                   ");
            parameter.AppendSql("      ,JUSO, JICODE, TEL, SABUN, EMBPRT, BI, PNAME, GWANGE, KIHO, GKIHO, DEPTCODE, DRCODE, GBSPC, GBGAMEK  ");
            parameter.AppendSql("      ,JINILSU, JINAMT, TUYAKGWA, TUYAKMONTH, TUYAKJULDATE, TUYAKILSU, BOHUN, REMARK, RELIGION             ");
            parameter.AppendSql("      ,GBMSG, XRAYBARCODE, ARSCHK, BUNUP, BIRTH, GBBIRTH, EMAIL, GBINFOR, JIKUP, HPHONE, GBJUGER           ");
            parameter.AppendSql("      ,GBSMS, GBJUSO, BICHK, HPHONE2, JUSAMSG, EKGMSG, BIDATE, MISSINGCALL, AIFLU, TEL_CONFIRM             ");
            parameter.AppendSql("      ,GBSMS_DRUG, GBINFO_DETAIL, GBINFOR2, ROAD, ROADDONG, JUMIN3, GBFOREIGNER, ENAME, CASHYN             ");
            parameter.AppendSql("      ,GB_VIP, GB_VIP_REMARK, GB_VIP_SABUN, GB_VIP_DATE, ROADDETAIL, GB_VIP2, GB_VIP2_REAMRK               ");
            parameter.AppendSql("      ,GB_SVIP, WEBSEND, WEBSENDDATE, GBMERS, OBST, ZIPCODE3, BUILDNO, PT_REMARK, TEMPLE, C_NAME           ");
            parameter.AppendSql("      ,GBCOUNTRY, GBGAMEKC, SNAME2, PNAME2, INJ_SMS1, INJ_SMS2, EPASSNO, EUNIQNO                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                                                             ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                                                                                    ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN2                                                                                    ");
            parameter.AppendSql("   AND SNAME NOT IN (:SNAME)                                                                               ");

            parameter.Add("JUMIN1", argJumin1);
            parameter.Add("JUMIN2", argJumin2);
            parameter.AddInStatement("SNAME", b04_NOT_PATIENT);

            return ExecuteReader<BAS_PATIENT>(parameter);
        }

        public BAS_PATIENT GetCountbyJumin1Jumin3(string strJumin1, string strJumin3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, ZIPCODE1, ZIPCODE2, JUSO, GKIHO, SEX           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                     ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                                            ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3                                            ");

            parameter.Add("JUMIN1", strJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", strJumin3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public int Insert(BAS_PATIENT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.BAS_PATIENT        ");
            parameter.AppendSql("       (PANO                               ");
            parameter.AppendSql("     , SNAME                               ");
            parameter.AppendSql("     , SEX                                 ");
            parameter.AppendSql("     , JUMIN1                              ");
            parameter.AppendSql("     , JUMIN2                              ");
            parameter.AppendSql("     , STARTDATE                           ");
            parameter.AppendSql("     , LASTDATE                            ");
            parameter.AppendSql("     , ZIPCODE1                            ");
            parameter.AppendSql("     , ZIPCODE2                            ");
            parameter.AppendSql("     , JUSO                                ");
            parameter.AppendSql("     , JICODE                              ");
            parameter.AppendSql("     , TEL                                 ");
            parameter.AppendSql("     , HPHONE                              ");
            parameter.AppendSql("     , EMBPRT                              ");
            parameter.AppendSql("     , BI                                  ");
            parameter.AppendSql("     , PNAME                               ");
            parameter.AppendSql("     , GWANGE                              ");
            parameter.AppendSql("     , KIHO                                ");
            parameter.AppendSql("     , GKIHO                               ");
            parameter.AppendSql("     , DEPTCODE                            ");
            parameter.AppendSql("     , DRCODE                              ");
            parameter.AppendSql("     , GBSPC                               ");
            parameter.AppendSql("     , GBGAMEK                             ");
            parameter.AppendSql("     , BOHUN                               ");
            parameter.AppendSql("     , REMARK                              ");
            parameter.AppendSql("     , SABUN                               ");
            parameter.AppendSql("     , BUNUP                               ");
            parameter.AppendSql("     , BIRTH                               ");
            parameter.AppendSql("     , GBBIRTH                             ");
            parameter.AppendSql("     , EMAIL                               ");
            parameter.AppendSql("     , GBINFOR                             ");
            parameter.AppendSql("     , GBJUSO                              ");
            parameter.AppendSql("     , GBSMS                               ");
            parameter.AppendSql("     , HPHONE2                             ");
            parameter.AppendSql("     , JUMIN3)                             ");
            parameter.AppendSql("VALUES                                     ");
            parameter.AppendSql("       (:PANO                              ");
            parameter.AppendSql("     , :SNAME                              ");
            parameter.AppendSql("     , :SEX                                ");
            parameter.AppendSql("     , :JUMIN1                             ");
            parameter.AppendSql("     , :JUMIN2                             ");
            parameter.AppendSql("     , TO_DATE(:STARTDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("     , TO_DATE(:LASTDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("     , :ZIPCODE1                           ");
            parameter.AppendSql("     , :ZIPCODE2                           ");
            parameter.AppendSql("     , :JUSO                               ");
            parameter.AppendSql("     , :JICODE                             ");
            parameter.AppendSql("     , :TEL                                ");
            parameter.AppendSql("     , :HPHONE                             ");
            parameter.AppendSql("     , :EMBPRT                             ");
            parameter.AppendSql("     , :BI                                 ");
            parameter.AppendSql("     , :PNAME                              ");
            parameter.AppendSql("     , :GWANGE                             ");
            parameter.AppendSql("     , :KIHO                               ");
            parameter.AppendSql("     , :GKIHO                              ");
            parameter.AppendSql("     , :DEPTCODE                           ");
            parameter.AppendSql("     , :DRCODE                             ");
            parameter.AppendSql("     , :GBSPC                              ");
            parameter.AppendSql("     , :GBGAMEK                            ");
            parameter.AppendSql("     , :BOHUN                              ");
            parameter.AppendSql("     , :REMARK                             ");
            parameter.AppendSql("     , :SABUN                              ");
            parameter.AppendSql("     , :BUNUP                              ");
            parameter.AppendSql("     , TO_DATE(:BIRTH, 'YYYY-MM-DD')       ");
            parameter.AppendSql("     , :GBBIRTH                            ");
            parameter.AppendSql("     , :EMAIL                              ");
            parameter.AppendSql("     , :GBINFOR                            ");
            parameter.AppendSql("     , :GBJUSO                             ");
            parameter.AppendSql("     , :GBSMS                              ");
            parameter.AppendSql("     , :HPHONE2                            ");
            parameter.AppendSql("     , :JUMIN3)                            ");

            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("JUMIN1", item.JUMIN1);
            parameter.Add("JUMIN2", item.JUMIN2);
            parameter.Add("STARTDATE", item.STARTDATE);
            parameter.Add("LASTDATE", item.LASTDATE);
            parameter.Add("ZIPCODE1", item.ZIPCODE1);
            parameter.Add("ZIPCODE2", item.ZIPCODE2);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("JICODE", item.JICODE);
            parameter.Add("TEL", item.TEL);
            parameter.Add("HPHONE", item.HPHONE);
            parameter.Add("EMBPRT", item.EMBPRT);
            parameter.Add("BI", item.BI);
            parameter.Add("PNAME", item.PNAME);
            parameter.Add("GWANGE", item.GWANGE);
            parameter.Add("KIHO", item.KIHO);
            parameter.Add("GKIHO", item.GKIHO);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("DRCODE", item.DRCODE);
            parameter.Add("GBSPC", item.GBSPC);
            parameter.Add("GBGAMEK", item.GBGAMEK);
            parameter.Add("BOHUN", item.BOHUN);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("SABUN", item.SABUN);
            parameter.Add("BUNUP", item.BUNUP);
            parameter.Add("BIRTH", item.BIRTH);
            parameter.Add("GBBIRTH", item.GBBIRTH);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("GBINFOR", item.GBINFOR);
            parameter.Add("GBJUSO", item.GBJUSO);
            parameter.Add("GBSMS", item.GBSMS);
            parameter.Add("HPHONE2", item.HPHONE2);
            parameter.Add("JUMIN3", item.JUMIN3);

            return ExecuteNonQuery(parameter);
        }

        public BAS_PATIENT GetPaNobyJumin1Jumin3(string strJumin1, string strJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT         ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN2                ");

            parameter.Add("JUMIN1", strJumin1);
            parameter.Add("JUMIN2", strJumin2);

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public string  GetPaNobyJumin1Jumin2(string strJumin1, string strJumin2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PANO                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT         ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN2                ");

            parameter.Add("JUMIN1", strJumin1);
            parameter.Add("JUMIN2", strJumin2);

            return ExecuteScalar <string>(parameter);
        }

        public string GetPaNobyPaNo(string strPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT         ");
            parameter.AppendSql(" WHERE PANO = :PANO                    ");

            parameter.Add("PANO", strPtNo);

            return ExecuteScalar<string>(parameter);
        }

        public BAS_PATIENT GetItembyJumin1Jumin3(string argJumin1, string argJumin3)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Pano,SName, ZipCode1,ZipCode2, Juso, Gkiho,Sex      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                             ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                                    ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3                                    ");

            parameter.Add("JUMIN1", argJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", argJumin3);

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }

        public string GetPaNobyJumin1Jumin2Jumin3(string strJumin1, string strJumin2, string strJumin3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                 ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                                        ");
            parameter.AppendSql("   AND JUMIN3 IS NULL                                          ");
            parameter.AppendSql("   AND (JUMIN2 = :JUMIN2                                       ");
            parameter.AppendSql("    OR  JUMIN3 = :JUMIN3)                                      ");

            parameter.Add("JUMIN1", strJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN2", strJumin2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", strJumin3);

            return ExecuteScalar<string>(parameter);
        }

        public string GetPaNOByJuminNo(string strJumin1, string strJumin2, string strJumin3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO                                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                                 ");
            parameter.AppendSql(" WHERE JUMIN1 = :JUMIN1                                        ");
            parameter.AppendSql("   AND JUMIN2 = :JUMIN2                                        ");
            parameter.AppendSql("   AND JUMIN3 = :JUMIN3                                        ");

            parameter.Add("JUMIN1", strJumin1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN2", strJumin2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JUMIN3", strJumin3);

            return ExecuteScalar<string>(parameter);
        }


        public BAS_PATIENT GetItembyPano(string strPano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO,SNAME,SEX,JUMIN1,JUMIN2,STARTDATE,LASTDATE,ZIPCODE1,ZIPCODE2                                   ");
            parameter.AppendSql("      ,JUSO, JICODE, TEL, SABUN, EMBPRT, BI, PNAME, GWANGE, KIHO, GKIHO, DEPTCODE, DRCODE, GBSPC, GBGAMEK  ");
            parameter.AppendSql("      ,JINILSU, JINAMT, TUYAKGWA, TUYAKMONTH, TUYAKJULDATE, TUYAKILSU, BOHUN, REMARK, RELIGION             ");
            parameter.AppendSql("      ,GBMSG, XRAYBARCODE, ARSCHK, BUNUP, BIRTH, GBBIRTH, EMAIL, GBINFOR, JIKUP, HPHONE, GBJUGER           ");
            parameter.AppendSql("      ,GBSMS, GBJUSO, BICHK, HPHONE2, JUSAMSG, EKGMSG, BIDATE, MISSINGCALL, AIFLU, TEL_CONFIRM             ");
            parameter.AppendSql("      ,GBSMS_DRUG, GBINFO_DETAIL, GBINFOR2, ROAD, ROADDONG, JUMIN3, GBFOREIGNER, ENAME, CASHYN             ");
            parameter.AppendSql("      ,GB_VIP, GB_VIP_REMARK, GB_VIP_SABUN, GB_VIP_DATE, ROADDETAIL, GB_VIP2, GB_VIP2_REAMRK               ");
            parameter.AppendSql("      ,GB_SVIP, WEBSEND, WEBSENDDATE, GBMERS, OBST, ZIPCODE3, BUILDNO, PT_REMARK, TEMPLE, C_NAME           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PATIENT                     ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");

            parameter.Add("PANO", strPano);

            return ExecuteReaderSingle<BAS_PATIENT>(parameter);
        }


    }
}
