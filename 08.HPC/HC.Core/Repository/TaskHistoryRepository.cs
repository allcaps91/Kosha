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
            parameter.AppendSql(" SELECT A.SITE_ID, B.NAME as SITENAME, A.TASKNAME, A.CREATED, A.CREATEDUSER, C.NAME as USERNAME FROM HIC_OSHA_TASK_HISTORY A");
            parameter.AppendSql(" INNER JOIN HC_SITE_VIEW B");
            parameter.AppendSql(" ON A.SITE_ID = B.ID");
            parameter.AppendSql(" INNER JOIN HIC_USERS C");
            parameter.AppendSql(" ON A.CREATEDUSER = C.USERID");
            parameter.AppendSql(" WHERE A.CREATEDUSER = :CREATEDUSER");
            parameter.AppendSql(" AND A.CREATED >= SYSTIMESTAMP - 7");

            parameter.Add("CREATEDUSER", userId);

            return ExecuteReader<TaskHistoryModel>(parameter);
           
        }
        public void Update(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_SITE");
            parameter.AppendSql("SET ");
            parameter.AppendSql("    MODIFIED = SYSTIMESTAMP");
            parameter.AppendSql("WHERE ID = :ID ");

            parameter.Add("ID", siteId);
            ExecuteNonQuery(parameter);

        }
        public void Insert(TaskHistory dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_TASK_HISTORY");
            parameter.AppendSql("(");
            parameter.AppendSql("    SITE_ID");
            parameter.AppendSql("  , TASKNAME");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :SITE_ID");
            parameter.AppendSql("  , :TASKNAME");
            parameter.AppendSql("  , SYSTIMESTAMP");
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql(") ");

            parameter.Add("SITE_ID", dto.SITE_ID);
            parameter.Add("TASKNAME", dto.TASKNAME);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

        }

    }
}
