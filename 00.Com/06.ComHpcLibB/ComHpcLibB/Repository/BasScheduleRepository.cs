namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class BasScheduleRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public BasScheduleRepository()
        {
        }

        public BAS_SCHEDULE Read_Schedule(string SCHDATE, string DRCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE                                      ");
            parameter.AppendSql("     , TO_CHAR(SchDate, 'DD') ILJA, GbJin, GbJin2  ");
            parameter.AppendSql("     , GbJin3, GbDay, ROWID                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE                    ");
            parameter.AppendSql(" WHERE SCHDATE = TO_DATE(:SCHDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND DRCODE  = :DRCODE                           ");

            parameter.Add("SCHDATE", SCHDATE);
            parameter.Add("DRCODE", DRCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_SCHEDULE>(parameter);
        }

        public List<BAS_SCHEDULE> GetItembySchDate(string fstrJepDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE, GBJIN, GBJIN2                                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE                                        ");
            parameter.AppendSql(" WHERE SCHDATE = TO_DATE(:SCHDATE, 'YYYY-MM-DD')                       ");
            parameter.AppendSql(" AND DRCODE IN ('1402','1405','1407','3114','3115')                    ");
            parameter.AppendSql(" ORDER BY DECODE(DRCODE, '1405',1,'1407',2,'1402',3,'3114',4,'3115',5) ");

            parameter.Add("SCHDATE", fstrJepDate);

            return ExecuteReader<BAS_SCHEDULE>(parameter);
        }

        public BAS_SCHEDULE GetGbJinGbJin2bySchDateDrCode(string argDate, string strDrCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBJIN, GBJIN2                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE                    ");
            parameter.AppendSql(" WHERE DRCODE  = :DRCODE                           ");
            parameter.AppendSql("   AND SCHDATE = TO_DATE(:SCHDATE, 'YYYY-MM-DD')   ");

            parameter.Add("SCHDATE", argDate);
            parameter.Add("DRCODE", strDrCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_SCHEDULE>(parameter);
        }

        public BAS_SCHEDULE GetGbJinbyDrCode(string dRCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GbJin, GbJin2               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE    ");
            parameter.AppendSql(" WHERE DRCODE  = :DRCODE           ");
            parameter.AppendSql("   AND SCHDATE = TRUNC(SYSDATE)    ");

            parameter.Add("DRCODE", dRCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<BAS_SCHEDULE>(parameter);
        }

        public List<BAS_SCHEDULE> GetItembySchDateDrcode(string strFDate, string strTDate, string strDrcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE                                      ");
            parameter.AppendSql("     , TO_CHAR(SCHDATE, 'DD') ILJA, GBJIN, GBJIN2  ");
            parameter.AppendSql("     , GBJIN3, GBDAY                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE                    ");
            parameter.AppendSql(" WHERE  1=1                                        ");
            parameter.AppendSql("   AND SCHDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND SCHDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')    ");

            parameter.AppendSql("   AND DRCODE = (:DRCODE)                          ");
            parameter.AppendSql(" ORDER BY DRCODE, SCHDATE                          ");


            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("DRCODE", strDrcode);

            return ExecuteReader<BAS_SCHEDULE>(parameter);
        }

        public List<BAS_SCHEDULE> GetItembySchDateDrcodes(string strFDate, string strTDate, List<string> strDrcodes)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE                                      ");
            parameter.AppendSql("     , TO_CHAR(SCHDATE, 'DD') ILJA, GBJIN, GBJIN2  ");
            parameter.AppendSql("     , GBJIN3, GBDAY                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE                    ");
            parameter.AppendSql(" WHERE  1=1                                        ");
            parameter.AppendSql("   AND SCHDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND SCHDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')    ");

            parameter.AppendSql("   AND DRCODE  IN (:DRCODE)                        ");
            //parameter.AppendSql(" AND DRCODE  IN ('1109','1402','7706','1405','7114','7115','1407','7709','7710','1404','1409')");
            parameter.AppendSql(" ORDER BY DRCODE, SCHDATE                          ");


            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.AddInStatement("DRCODE", strDrcodes);

            return ExecuteReader<BAS_SCHEDULE>(parameter);
        }

        public int Delete (string strDrCode, string strFDATE, string strTDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.BAS_SCHEDULE                    ");
            parameter.AppendSql(" WHERE DRCODE = :DRCODE                            ");
            parameter.AppendSql("   AND SCHDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND SCHDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')    ");

            #region Query 변수대입
            parameter.Add("DRCODE", strDrCode);
            parameter.Add("FDATE", strFDATE);
            parameter.Add("TDATE", strTDATE);

            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int Insert(string strDrCode, string strDate, string strDay, string strGbn, string strGbn2, string strGbn3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_PMPA.BAS_SCHEDULE (                ");
            parameter.AppendSql("        DRCODE, SCHDATE, GBDAY, GBJINSEND              ");
            parameter.AppendSql("        ,GBJIN, GBJIN2, GBJIN3)                        ");
            parameter.AppendSql(" VALUES (                                              ");
            parameter.AppendSql("        :DRCODE, :SCHDATE, :GBDATE, ' '                ");
            parameter.AppendSql("        ,:GBJIN, :GBJIN2, :GBJIN3)                     ");

            #region Query 변수대입
            parameter.Add("DRCODE", strDrCode);
            parameter.Add("SCHDATE", strDate);
            parameter.Add("GBDAY", strDay);
            parameter.Add("GBJIN", strGbn);
            parameter.Add("GBJIN2", strGbn2);
            parameter.Add("GBJIN3", strGbn3);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int Update(string strDayGbn, string strGbn, string strGbn2, string strGbn3, string strROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE KOSMOS_PMPA.BAS_SCHEDULE                            ");
            parameter.AppendSql("   SET GBDAY       =:GBDAY                                 ");
            parameter.AppendSql("      ,GBJIN       =:GBJIN                                 ");
            parameter.AppendSql("      ,GBJINEND    =''                                     ");
            parameter.AppendSql("      ,GBJIN2      =:GBJIN2                                ");
            parameter.AppendSql("      ,GBJIN3      =:GBJIN3                                ");
            parameter.AppendSql(" WHERE ROWID       =:ROWID                                 ");

            #region Query 변수대입

            parameter.Add("GBDAY", strDayGbn);
            parameter.Add("GBJIN", strGbn);
            parameter.Add("GBJIN2", strGbn2);
            parameter.Add("GBJIN3", strGbn3);
            parameter.Add("ROWID", strROWID);
            #endregion

            return ExecuteNonQuery(parameter);
        }





        public int Read_Schedule_CNT(string strFDATE, string strTDATE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_SCHEDULE                    ");
            parameter.AppendSql(" WHERE 1=1                                         ");
            parameter.AppendSql(" AND SCHDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')      ");
            parameter.AppendSql(" AND SCHDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')      ");

            parameter.Add("FDATE", strFDATE);
            parameter.Add("TDATE", strTDATE);

            return ExecuteScalar<int>(parameter);
        }
    }
}
