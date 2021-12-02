namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicPrivacyAcceptNewRepository : BaseRepository
    {

        public HicPrivacyAcceptNewRepository()
        {
        }

        public HIC_PRIVACY_ACCEPT_NEW GetIetmByPtnoYear(string argPtno, string argYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(ENTDATE, 'YYYY-MM-DD') AS ENTDATE, FILENAME     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_PRIVACY_ACCEPT_NEW                      ");
            parameter.AppendSql(" WHERE 1 =1                                                    ");
            parameter.AppendSql(" AND PTNO = :PTNO                                              ");
            parameter.AppendSql(" AND GJYEAR = :YEAR                                            ");

            parameter.Add("PTNO", argPtno);
            parameter.Add("YEAR", argYear);

            return ExecuteReaderSingle<HIC_PRIVACY_ACCEPT_NEW>(parameter);
        }

        public void Insert(HIC_PRIVACY_ACCEPT_NEW nHC)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO HIC_PRIVACY_ACCEPT_NEW (GJYEAR, PTNO, SNAME, FILENAME, ENTDATE     ");
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
