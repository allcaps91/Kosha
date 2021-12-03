namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicPrivacyAcceptRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicPrivacyAcceptRepository()
        {
        }

        public HIC_PRIVACY_ACCEPT GetIetmByPtnoYear(string argPtno, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(ENTDATE, 'YYYY-MM-DD') AS ENTDATE, FILENAME     ");
            parameter.AppendSql("  FROM ADMIN.HIC_PRIVACY_ACCEPT                          ");
            parameter.AppendSql(" WHERE 1 =1                                                    ");
            parameter.AppendSql(" AND PTNO = :PTNO                                              ");
            parameter.AppendSql(" AND GJYEAR = :YEAR                                              ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("YEAR", argYear);

            return ExecuteReaderSingle<HIC_PRIVACY_ACCEPT>(parameter);
        }

        public void Insert(HIC_PRIVACY_ACCEPT nHC)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_PRIVACY_ACCEPT (GJYEAR, PTNO, SNAME, FILENAME, ENTDATE         ");
            parameter.AppendSql(" ) VALUES (                                                                    ");
            parameter.AppendSql(" :GJYEAR, :PTNO, :SNAME, :FILENAME, SYSDATE)                                   ");

            parameter.Add("GJYEAR", nHC.GJYEAR);
            parameter.Add("PTNO", nHC.PTNO);
            parameter.Add("SNAME", nHC.SNAME);
            parameter.Add("FILENAME", nHC.FILENAME);

            ExecuteNonQuery(parameter);
        }


    }
}
