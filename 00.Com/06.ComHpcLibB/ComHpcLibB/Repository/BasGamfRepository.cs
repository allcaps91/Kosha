namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class BasGamfRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public BasGamfRepository()
        {
        }

        
        public BAS_GAMF Read_Gam_Opd(string JUMIN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GAMCODE,  GAMMESSAGE                    ");
            parameter.AppendSql("     , TO_CHAR(GamEnd, 'YYYY-MM-DD') GamEnd    ");
            parameter.AppendSql("  FROM ADMIN.BAS_GAMF                    ");
            parameter.AppendSql(" WHERE GamJumin3 = :JUMIN                      ");

            parameter.Add("JUMIN", JUMIN);

            return ExecuteReaderSingle<BAS_GAMF>(parameter);
        }
    }
}
