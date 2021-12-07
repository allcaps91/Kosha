namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Service;
    using HC.OSHA.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicOshaPanjengRepository : BaseRepository
    {
        public void Delete(string YEAR, long SITE_ID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_PANJEONG   ");
            parameter.AppendSql("   WHERE SITE_ID = :SITE_ID     ");
            parameter.AppendSql("   AND YEAR = :YEAR   ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", SITE_ID);
            parameter.Add("YEAR", YEAR);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            ExecuteNonQuery(parameter);
        }

        public void Insert(HIC_OSHA_PANJEONG dto)
        {
            
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_PANJEONG  ");
            parameter.AppendSql("(                              ");
            parameter.AppendSql("    WORKER_ID                  ");
            parameter.AppendSql("  , SITE_ID                    ");
            parameter.AppendSql("  , SITENAME                   ");
            parameter.AppendSql("  , YEAR                       ");
            parameter.AppendSql("  , PANO                       ");
            parameter.AppendSql("  , NAME                       ");
            parameter.AppendSql("  , SEX                        ");
            parameter.AppendSql("  , AGE                        ");
            parameter.AppendSql("  , JOBYEAR                    ");
            parameter.AppendSql("  , PANJEONG                   ");
            parameter.AppendSql("  , ISSPECIAL                  ");
            parameter.AppendSql("  , OPINION                    ");
            parameter.AppendSql("  , RESULT                     ");
            parameter.AppendSql("  , GRADE                      ");
            parameter.AppendSql("  , TASK                       ");
            parameter.AppendSql("  , INJA                       ");
            parameter.AppendSql("  , JIPYO                      ");
            parameter.AppendSql("  , CREATED                    ");
            parameter.AppendSql("  , SWLICENSE                  ");
            parameter.AppendSql(") VALUES (                     ");
            parameter.AppendSql("    :WORKER_ID                 ");
            parameter.AppendSql("  , :SITE_ID                   ");
            parameter.AppendSql("  , :SITENAME                  ");
            parameter.AppendSql("  , :YEAR                      ");
            parameter.AppendSql("  , :PANO                      ");
            parameter.AppendSql("  , :NAME                      ");
            parameter.AppendSql("  , :SEX                       ");
            parameter.AppendSql("  , :AGE                       ");
            parameter.AppendSql("  , :JOBYEAR                   ");
            parameter.AppendSql("  , :PANJEONG                  ");
            parameter.AppendSql("  , :ISSPECIAL                 ");
            parameter.AppendSql("  , :OPINION                   ");
            parameter.AppendSql("  , :RESULT                    ");
            parameter.AppendSql("  , :GRADE                     ");
            parameter.AppendSql("  , :TASK                      ");
            parameter.AppendSql("  , :INJA                      ");
            parameter.AppendSql("  , :JIPYO                     ");
            parameter.AppendSql("  , SYSTIMESTAMP               ");
            parameter.AppendSql("  , :SWLICENSE                 ");
            parameter.AppendSql(")                              ");

            parameter.Add("WORKER_ID", dto.WORKER_ID);
            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("SITENAME", dto.SITENAME);
            parameter.Add("YEAR", dto.YEAR);
            parameter.Add("PANO", dto.PANO);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("SEX", dto.SEX);
            parameter.Add("AGE", dto.AGE);
            parameter.Add("JOBYEAR", dto.JOBYEAR);
            parameter.Add("PANJEONG", dto.PANJEONG);
            parameter.Add("ISSPECIAL", dto.ISSPECIAL);
            parameter.Add("OPINION", dto.OPINION);
            parameter.Add("RESULT", dto.RESULT);
            parameter.Add("GRADE", dto.GRADE);
            parameter.Add("TASK", dto.TASK);
            parameter.Add("INJA", dto.INJA);
            parameter.Add("JIPYO", dto.JIPYO);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
    }
}
