using ComBase;
using ComBase.Mvc;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Core.Repository
{
    public class TaskHistoryRepository : BaseRepository
    {

        /// <summary>
        /// 사용자별 작업 내용 가져오기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TaskHistoryModel> FindAllByUser(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.SITE_ID, B.NAME as SITENAME, A.TASKNAME, A.CREATED, A.CREATEDUSER, C.NAME as USERNAME ");
            parameter.AppendSql("   FROM HIC_OSHA_TASK_HISTORY A ");
            parameter.AppendSql("        INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("              ON A.SITE_ID = B.ID ");
            parameter.AppendSql("        INNER JOIN HIC_USERS C ");
            parameter.AppendSql("              ON A.CREATEDUSER = C.USERID ");
            parameter.AppendSql(" WHERE A.CREATEDUSER = :CREATEDUSER ");
            parameter.AppendSql("   AND A.CREATED >= SYSTIMESTAMP - 7 ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND C.SWLICENSE = :SWLICENSE ");

            parameter.Add("CREATEDUSER", userId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<TaskHistoryModel>(parameter);
           
        }
        public void Update(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE ");
            parameter.AppendSql("  SET MODIFIED = SYSTIMESTAMP ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

        }
        public void Insert(TaskHistory dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_TASK_HISTORY ");
            parameter.AppendSql(" (SITE_ID,TASKNAME,CREATED,CREATEDUSER,SWLICENSE) ");
            parameter.AppendSql("VALUES ");
            parameter.AppendSql(" (:SITE_ID,:TASKNAME,SYSTIMESTAMP,:CREATEDUSER,:SWLICENSE) ");

            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
    }
}
