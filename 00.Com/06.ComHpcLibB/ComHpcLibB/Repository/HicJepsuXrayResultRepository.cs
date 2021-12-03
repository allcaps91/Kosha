namespace ComHpcLibB.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuXrayResultRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuXrayResultRepository()
        {
        }

        public List<HIC_JEPSU_XRAY_RESULT> GetListbyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT B.GBREAD, B.XRAYNO        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a                                 ");
            parameter.AppendSql("     , ADMIN.HIC_XRAY_RESULT b                           ");
            parameter.AppendSql(" WHERE  1= 1                                                   ");
            parameter.AppendSql(" AND a.PANO = :PANO                                            ");
            parameter.AppendSql(" AND a.DELDATE IS NULL                                         ");
            parameter.AppendSql("  AND a.PANO = b.PANO(+)                                       ");
            parameter.AppendSql(" GROUP BY B.GBREAD, B.XRAYNO                                   ");

            parameter.Add("PANO", nPano);

            return ExecuteReader<HIC_JEPSU_XRAY_RESULT>(parameter);
        }

        public List<HIC_JEPSU_XRAY_RESULT> GetListbyXrayNo(string strXRAYNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(B.JepDate,'YYYY-MM-DD') JepDate,B.XrayNo,       ");
            parameter.AppendSql(" B.Result2,B.Result4,A.WRTNO,A.GjJong                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a                                 ");
            parameter.AppendSql("     , ADMIN.HIC_XRAY_RESULT b                           ");
            parameter.AppendSql(" WHERE  1= 1                                                   ");
            parameter.AppendSql(" AND a.XRAYNO = :XRAYNO                                        ");
            parameter.AppendSql(" AND a.DELDATE IS NULL                                         ");
            parameter.AppendSql(" AND a.XRAYNO = b.XRAYNO(+)                                    ");

            parameter.Add("XRAYNO", strXRAYNO);

            return ExecuteReader<HIC_JEPSU_XRAY_RESULT>(parameter);
        }
    }
}
