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
    public class HicMisuMonthlyRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuMonthlyRepository()
        {
        }

        public List<HIC_MISU_MONTHLY> GetmisuSummary(long txtWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                      YYMM,IWOLAMT,MISUAMT,(IPGUMAMT+ETCIPGUM) IPGUMAMT,GAMAMT,SAKAMT,BANAMT,JANAMT       ");

            parameter.AppendSql("   FROM                        " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY                                              ");

            parameter.AppendSql("   WHERE                       WRTNO = :NWRTNO                                                                     ");
            parameter.AppendSql("   ORDER BY                    YYMM                                                                                ");

            parameter.Add("NWRTNO",         txtWrtNo        );

            return ExecuteReader<HIC_MISU_MONTHLY>(parameter);
        }

        public List<HIC_MISU_MONTHLY> GetmisuTong(long txtWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  MAX(YYMM) YYMM                          ");

            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY  ");

            parameter.AppendSql("   WHERE                   WRTNO = :NWRTNO                         ");

            parameter.Add("NWRTNO",         txtWrtNo            );

            return ExecuteReader<HIC_MISU_MONTHLY>(parameter);
        }
        public List<HIC_MISU_MONTHLY> GetmisuTong_2()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  MAX(YYMM) YYMM                          ");
            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY  ");

            return ExecuteReader<HIC_MISU_MONTHLY>(parameter);
        }

        public List<HIC_MISU_MONTHLY> GetYYMM()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT                  MAX(YYMM) YYMM                          ");
            parameter.AppendSql("   FROM                    " + ComNum.DB_PMPA + "HIC_MISU_MONTHLY  ");
            return ExecuteReader<HIC_MISU_MONTHLY>(parameter);
        }
    }
}
