namespace ComSupLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComSupLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayResultnewRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayResultnewRepository()
        {
        }

        public XRAY_RESULTNEW GetItemByPtnoXCode(string argPtno, string argOrderCode, string argSeekDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT RESULT,XDRCODE1                             ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_RESULTNEW                  ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND PANO =:PANO                                 ");

            if (argOrderCode.Equals("US-CADU1") || argOrderCode.Equals("US-CADU2"))
            {
                parameter.AppendSql("   AND XCODE IN ('US-CADU1', 'US-CADU2')       ");
            }
            else
            {
                parameter.AppendSql("   AND XCODE =:XCD                             ");
            }

            parameter.AppendSql("   AND SEEKDATE =TO_DATE(:SEEKDATE, 'YYYY-MM-DD')  ");


            parameter.Add("PANO", argPtno);

            if (argOrderCode.Equals("US-CADU1") || argOrderCode.Equals("US-CADU2"))
            {
                
            }
            else
            {
                parameter.Add("XCD", argOrderCode.PadRight(8));
            }

            parameter.Add("SEEKDATE", argSeekDate);

            return ExecuteReaderSingle<XRAY_RESULTNEW>(parameter);
        }
    }
}
