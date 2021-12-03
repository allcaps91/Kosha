namespace HC.Core.Repository
{
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Model;
    using HC.Core.Service;


    /// <summary>
    /// 일반검진 접수
    /// </summary>
    public class HealthCareReceiptRepository : BaseRepository
    {
        public List<HealthCareReciptModel> FindAll(string pano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("  SELECT d.Name SiteName, d.code as SiteId, a.WRTNO,a.PANO,a.SName as Name,TO_CHAR(a.JepDate, 'YYYY-MM-DD') JepDate,a.GjJong,a.ltdcode, a.ErFlag,    ");
            parameter.AppendSql("  a.UCodes,a.Sex,a.Age, a.Tel, c.Name ExName, a.BuseName, to_char(IpsaDate,'YYYY-MM-DD') IpsaDate  ");
            parameter.AppendSql("  FROM HIC_JEPSU a, HIC_EXJONG C, HIC_LTD D    ");
            parameter.AppendSql("  WHERE  a.DelDate IS NULL   ");
            parameter.AppendSql("  AND A.PANO = :PANO   ");
            //parameter.AppendSql("  AND GjJong IN('11', '14', '23')   ");
            parameter.AppendSql("  AND GjJong IN('11', '14', '23', '22','24','28')   ");
            parameter.AppendSql("  AND a.GjJong = c.Code(+)   ");
            parameter.AppendSql("  AND a.LtdCode = d.Code(+)   ");
            parameter.AppendSql("  ORDER BY JEPDATE DESC   ");
            parameter.Add("PANO", pano);

            return ExecuteReader<HealthCareReciptModel>(parameter);
        }
    }
}
