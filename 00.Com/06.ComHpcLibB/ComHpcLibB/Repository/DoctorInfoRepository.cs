namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;    
    
    /// <summary>
    /// 
    /// </summary>
    public class DoctorInfoRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public DoctorInfoRepository()
        {
        }

        public DOCTOR_INFO Read_Hic_Doctor_Info(string sSABUN, long lSabun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.DrName,a.Licence,TO_CHAR(b.IpsaDay,'YYYY-MM-DD') IpsaDay      ");
            parameter.AppendSql("     , TO_CHAR(a.ReDay, 'YYYY-MM-DD') ReDay                            ");
            parameter.AppendSql("     , TO_CHAR(b.ToiDay, 'YYYY-MM-DD') ToiDay, Room, Pan, GbDent       ");
            parameter.AppendSql("  FROM HIC_Doctor          a                                           ");
            parameter.AppendSql("     , ADMIN.INSA_MST b                                           ");
            parameter.AppendSql(" WHERE a.Sabun = :SABUN                                                ");
            parameter.AppendSql("   AND b.Sabun = :LSABUN                                               ");

            parameter.Add("SABUN", sSABUN);
            parameter.Add("LSABUN", lSabun);

            return ExecuteReaderSingle<DOCTOR_INFO>(parameter);
        }
    }
}
