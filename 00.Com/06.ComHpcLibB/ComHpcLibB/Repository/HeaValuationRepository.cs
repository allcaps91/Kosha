namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaValuationRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaValuationRepository()
        {
        }

        public HEA_VALUATION GetAllbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID, WRTNO, SMOKE, ACTIVE, GAJOK1             ");
            parameter.AppendSql("     , GAJOK2, GAJOK3, JANGI, JIHWAN, ENTSABUN, GBSTS  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_VALUATION                       ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HEA_VALUATION>(parameter);
        }

        public int Update(HEA_VALUATION item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_VALUATION SET ");
            parameter.AppendSql("     , SMOKE    = :SMOKE             ");
            parameter.AppendSql("     , ACTIVE   = :ACTIVE            ");
            parameter.AppendSql("     , GAJOK1   = :GAJOK1            ");
            parameter.AppendSql("     , GAJOK2   = :GAJOK2            ");
            parameter.AppendSql("     , GAJOK3   = :GAJOK3            ");
            parameter.AppendSql("     , JANGI    = :JANGI             ");
            parameter.AppendSql("     , JIHWAN   = :JIHWAN            ");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN          ");
            parameter.AppendSql(" WHERE ROWID    = :ROWID             ");

            parameter.Add("SMOKE", item.SMOKE);
            parameter.Add("ACTIVE", item.ACTIVE);
            parameter.Add("GAJOK1", item.GAJOK1);
            parameter.Add("GAJOK2", item.GAJOK2);
            parameter.Add("GAJOK3", item.GAJOK3);
            parameter.Add("JANGI", item.JANGI);
            parameter.Add("JIHWAN", item.JIHWAN);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("ROWID", item.ROWID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HEA_VALUATION item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_VALUATION                                                          ");
            parameter.AppendSql("       (WRTNO, SMOKE, ACTIVE, GAJOK1, GAJOK2, GAJOK3, JANGI, JIHWAN, ENTSABUN, GBSTS)          ");
            parameter.AppendSql("VALUES                                                                                         ");
            parameter.AppendSql("       (:WRTNO, :SMOKE, :ACTIVE, :GAJOK1, :GAJOK2, :GAJOK3, :JANGI, :JIHWAN, :ENTSABUN, 'Y')   ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SMOKE", item.SMOKE);
            parameter.Add("ACTIVE", item.ACTIVE);
            parameter.Add("GAJOK1", item.GAJOK1);
            parameter.Add("GAJOK2", item.GAJOK2);
            parameter.Add("GAJOK3", item.GAJOK3);
            parameter.Add("JANGI", item.JANGI);
            parameter.Add("JIHWAN", item.JIHWAN);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public int Delete(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HEA_VALUATION WHERE WRTNO = :WRTNO  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
