using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository.StatusReport
{
    public class HealthCheckMemoRepository : BaseRepository
    {

        public HIC_OSHA_PATIENT_MEMO FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT  *  ");
            parameter.AppendSql("  FROM HIC_OSHA_PATIENT_MEMO A");
            parameter.AppendSql(" WHERE A.ID = :ID");

            parameter.Add("ID", id);
            HIC_OSHA_PATIENT_MEMO dto = ExecuteReaderSingle<HIC_OSHA_PATIENT_MEMO>(parameter);
            return dto;
        }

        public List<HIC_OSHA_PATIENT_MEMO> FindAll(string workerId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT  A.*, to_char(A.Created, 'YYYY-MM-dd') as WRITEDATE, B.name as WriteName  ");
            parameter.AppendSql("  FROM HIC_OSHA_PATIENT_MEMO A ");
            parameter.AppendSql("  INNER JOIN HIC_USERS B ");
            parameter.AppendSql("  ON A.CREATEDUSER = B.USERID ");
            
            parameter.AppendSql(" WHERE A.WORKER_ID = :WORKER_ID");
            parameter.AppendSql(" ORDER BY A.CREATED  DESC ");
            parameter.Add("WORKER_ID", workerId);
            return  ExecuteReader<HIC_OSHA_PATIENT_MEMO>(parameter);
        }

        public List<HIC_OSHA_PATIENT_REMARK> FindRemarkAll(string workerId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.ID                                         ");
            parameter.AppendSql("     , A.SITE_ID                                    ");
            parameter.AppendSql("     , A.WORKER_ID                                  ");
            parameter.AppendSql("     , A.REMARK                                     ");
            parameter.AppendSql("     , TO_CHAR(A.CREATED, 'YYYY-MM-dd') AS WRITEDATE");
            parameter.AppendSql("     , A.CREATEDUSER                                ");
            parameter.AppendSql("     , B.NAME AS WRITENAME                          ");
            parameter.AppendSql("  FROM HIC_OSHA_PATIENT_REMARK A                    ");
            parameter.AppendSql("  INNER JOIN HIC_USERS B                            ");
            parameter.AppendSql("          ON A.CREATEDUSER = B.USERID               ");
            parameter.AppendSql(" WHERE A.WORKER_ID = :WORKER_ID                     ");
            parameter.AppendSql("ORDER BY A.CREATED  DESC                            ");

            parameter.Add("WORKER_ID", workerId);
            return ExecuteReader<HIC_OSHA_PATIENT_REMARK>(parameter);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_PATIENT_MEMO   ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_PATIENT_MEMO", id);
        }

        public HIC_OSHA_PATIENT_MEMO Insert(HIC_OSHA_PATIENT_MEMO item)
        {
            item.ID = GetSequenceNextVal("HIC_OSHA_PATIENT_MEMO_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_PATIENT_MEMO  ");
            parameter.AppendSql("(                                  ");
            parameter.AppendSql("    ID                             ");
            parameter.AppendSql("  , WORKER_ID                      ");
            parameter.AppendSql("  , MEMO                           ");
            parameter.AppendSql("  , CREATED                        ");
            parameter.AppendSql("  , CREATEDUSER                    ");
            parameter.AppendSql(") VALUES (                         ");
            parameter.AppendSql("    :ID                            ");
            parameter.AppendSql("  , :WORKER_ID                     ");
            parameter.AppendSql("  , :MEMO                          ");
            parameter.AppendSql("  , SYSTIMESTAMP                   ");
            parameter.AppendSql("  , :CREATEDUSER                   ");
            parameter.AppendSql(")                                  ");

            parameter.Add("ID", item.ID);
            parameter.Add("WORKER_ID", item.WORKER_ID);
            parameter.Add("MEMO", item.MEMO);
          
            if (item.CREATEDUSER.IsNullOrEmpty())
            {
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("CREATEDUSER", item.CREATEDUSER);
            }

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_PATIENT_MEMO", item.ID);

            return FindOne(item.ID);
        }

        internal HIC_OSHA_PATIENT_REMARK InsertRemark(HIC_OSHA_PATIENT_REMARK item)
        {
            item.ID = GetSequenceNextVal("HIC_OSHA_PATIENT_REMARK_SEQ");

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_PATIENT_REMARK");
            parameter.AppendSql("(                                  ");
            parameter.AppendSql("    ID                             ");
            parameter.AppendSql("  , SITE_ID                        ");
            parameter.AppendSql("  , WORKER_ID                      ");
            parameter.AppendSql("  , REMARK                         ");
            parameter.AppendSql("  , CREATED                        ");
            parameter.AppendSql("  , CREATEDUSER                    ");
            parameter.AppendSql(") VALUES (                         ");
            parameter.AppendSql("    :ID                            ");
            parameter.AppendSql("  , :SITE_ID                       ");
            parameter.AppendSql("  , :WORKER_ID                     ");
            parameter.AppendSql("  , :REMARK                        ");
            parameter.AppendSql("  , SYSTIMESTAMP                   ");
            parameter.AppendSql("  , :CREATEDUSER                   ");
            parameter.AppendSql(")                                  ");

            parameter.Add("ID", item.ID);
            parameter.Add("SITE_ID", item.SITE_ID);
            parameter.Add("WORKER_ID", item.WORKER_ID);
            parameter.Add("REMARK", item.REMARK);

            if (item.CREATEDUSER.IsNullOrEmpty())
            {
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("CREATEDUSER", item.CREATEDUSER);
            }

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_PATIENT_REMARK", item.ID);

            return FindOneRemark(item.ID);
        }

        public HIC_OSHA_PATIENT_REMARK FindOneRemark(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                           ");
            parameter.AppendSql("  FROM HIC_OSHA_PATIENT_REMARK A   ");
            parameter.AppendSql(" WHERE A.ID = :ID                  ");

            parameter.Add("ID", id);
            HIC_OSHA_PATIENT_REMARK dto = ExecuteReaderSingle<HIC_OSHA_PATIENT_REMARK>(parameter);
            return dto;
        }

        public void DeleteRemark(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_PATIENT_REMARK   ");
            parameter.AppendSql("WHERE ID = :ID             ");
            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Delete("HIC_OSHA_PATIENT_REMARK", id);
        }

        internal void DeleteRemark(HIC_OSHA_PATIENT_REMARK memoDto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_PATIENT_REMARK     ");
            parameter.AppendSql("   SET REMARK      = ''            ");
            parameter.AppendSql("     , CREATEDUSER = :CREATEDUSER  ");
            parameter.AppendSql(" WHERE ID = :ID                    ");

            parameter.Add("ID", memoDto.ID);
            parameter.Add("CREATEDUSER", memoDto.WORKER_ID);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_OSHA_PATIENT_MEMO", memoDto.ID);
        }

        //public HIC_OSHA_PATIENT_MEMO Update(HIC_OSHA_PATIENT_MEMO dto)
        //{
        //    MParameter parameter = CreateParameter();
        //    parameter.AppendSql("UPDATE HIC_OSHA_PATIENT_MEMO");
        //    parameter.AppendSql("    SET ");
        //    parameter.AppendSql("       MEMO = :MEMO");
        //    parameter.AppendSql("WHERE ID = :ID                     ");

        //    parameter.Add("ID", dto.ID);
        //    parameter.Add("MEMO", dto.MEMO);

        //    ExecuteNonQuery(parameter);
        //    DataSyncService.Instance.Update("HIC_OSHA_PATIENT_MEMO", dto.ID);
        //    return FindOne(dto.ID);

        //}
    }
}
