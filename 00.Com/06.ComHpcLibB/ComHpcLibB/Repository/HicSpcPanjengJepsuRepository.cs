namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcPanjengJepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengJepsuRepository()
        {
        }

        public List<HIC_SPC_PANJENG_JEPSU> GetItembyJepDateWrtnoLtdCode(string strFrDate, string strToDate, long nWrtNo, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO, SUM(DECODE(a.Panjeng,'5',1,0)) D1, SUM(DECODE(a.Panjeng,'6',1,0)) D2   ");
            parameter.AppendSql("     , SUM(DECODE(a.Panjeng,'A',1,0)) DN                                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANJENG a, ADMIN.HIC_JEPSU b                          ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.PanjengDate IS NOT NULL                                                       ");
            parameter.AppendSql("   AND a.Panjeng IN ('5','6','A')                                                      ");
            parameter.AppendSql("   AND a.MCode <> 'ZZZ'                                                                ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.WRTNO=b.WRTNO(+)                                                              ");
            parameter.AppendSql("   AND b.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND b.TongboDate <= TRUNC(SYSDATE-3)                                                ");
            parameter.AppendSql("   AND b.GjJong IN ('11','14','16','19','23','28','41','44')                           ");
            if (nWrtNo != 0)
            {
                parameter.AppendSql("   AND b.WRTNO = :WRTNO                                                            ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND b.LTDCODE = :LTDCODE                                                        ");
            }
            parameter.AppendSql("   GROUP BY a.WRTNO                                                                    ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nWrtNo != 0)
            {
                parameter.Add("WRTNO", nWrtNo);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_SPC_PANJENG_JEPSU>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo(long gnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,MCode,Panjeng ");
            parameter.AppendSql("     , SogenCode,JochiCode,WorkYN,SahuCode,MsymCode,MBun,UCode     ");
            parameter.AppendSql("     , SogenRemark,JochiRemark,ROWID                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_SPC_PANJENG                                 ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                              ");
            parameter.AppendSql("   AND ( DELDATE IS NULL OR DELDATE ='' )                          ");
            parameter.AppendSql(" ORDER BY MCode                                                    ");

            parameter.Add("WRTNO", gnWRTNO);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }
    }
}
