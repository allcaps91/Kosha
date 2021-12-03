namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanjengMcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengMcodeRepository()
        {
        }

        public List<HIC_SPC_PANJENG_MCODE> GetItembyWrtNoJepDate(long fnWRTNO, string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.TongBun,a.Panjeng,COUNT(*) CNT                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANJENG a, ADMIN.HIC_MCODE b  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.MCode = b.Code(+)                                     ");
            parameter.AppendSql("   AND a.Deldate IS NULL                                       ");
            parameter.AppendSql("   AND JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql("   AND JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')               ");
            parameter.AppendSql(" GROUP BY b.TongBun,a.Panjeng                                  ");
            parameter.AppendSql(" ORDER BY b.TongBun,a.Panjeng                                  ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteReader<HIC_SPC_PANJENG_MCODE>(parameter);
        }

        public int GetCountbyWrtNoUCode(long wRTNO, string strMCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) CNT                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANJENG a, ADMIN.HIC_MCODE b  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                        ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND b.UCODE = :UCODE                                        ");
            parameter.AppendSql("   AND a.MCODE = b.CODE                                        ");

            parameter.Add("WRTNO", wRTNO);
            parameter.Add("FRDATE", strMCode);

            return ExecuteScalar<int>(parameter);
        }
    }
}
