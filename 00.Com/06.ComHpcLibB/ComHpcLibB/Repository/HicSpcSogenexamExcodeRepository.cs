namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicSpcSogenexamExcodeRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcSogenexamExcodeRepository()
        {
        }

        public List<HIC_SPC_SOGENEXAM_EXCODE> GetItembySogenCode(string fstrCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.EXCODE, b.HNAME, b.YNAME, a.HANG                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_SOGENEXAM a, KOSMOS_PMPA.HIC_EXCODE b   ");
            parameter.AppendSql(" WHERE a.SOGENCODE = :SOGENCODE                                    ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                        ");
            parameter.AppendSql(" ORDER BY a.EXCODE                                                 ");

            parameter.Add("WRTNO", fstrCode);

            return ExecuteReader<HIC_SPC_SOGENEXAM_EXCODE>(parameter);
        }
    }
}
