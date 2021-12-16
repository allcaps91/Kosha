namespace HC.Core.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComBase.Controls;
    using HC_Core.Service;
    using ComBase.Mvc.Utils;
    using HC.Core.Service;
    using HC.Core.Dto;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HcSiteWorkerRepository : BaseRepository
    {
        public HC_SITE_WORKER FindOneByPano(string pano)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT * FROM HC_SITE_WORKER_VIEW WHERE PANO = :PANO            ");
            parameter.Add("PANO", pano);

            return ExecuteReaderSingle<HC_SITE_WORKER>(parameter);
        }
        public HC_SITE_WORKER FindOne(string ID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT * FROM HC_SITE_WORKER_VIEW WHERE ID = :ID            ");
            parameter.Add("ID", ID);

            return ExecuteReaderSingle<HC_SITE_WORKER>(parameter);
        }
        public HC_SITE_WORKER FindByJumin(string jumin)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT * FROM HC_SITE_WORKER_VIEW WHERE JUMIN = :JUMIN            ");
            parameter.Add("JUMIN", jumin);

            return ExecuteReaderSingle<HC_SITE_WORKER>(parameter);
        }
        //public HC_SITE_WORKER FindOne(long id)
        //{
        //    MParameter parameter = CreateParameter();
        //    parameter.AppendSql(" SELECT A.*,                                             ");
        //    parameter.AppendSql(" SUBSTR(JUMIN, 7, 1),                                                 ");
        //    parameter.AppendSql(" CASE                                                ");
        //    parameter.AppendSql(" WHEN SUBSTR(JUMIN, 7, 1) IN('1', '3', '5', '7', '9') THEN 'M'                                                ");
        //    parameter.AppendSql(" WHEN SUBSTR(JUMIN, 7, 1) IN('2', '4', '6', '8', '0') THEN 'F'                                                ");
        //    parameter.AppendSql(" ELSE ''                                                ");
        //    parameter.AppendSql(" END GENDER,                                                ");
        //    parameter.AppendSql(" CASE                                                ");
        //    parameter.AppendSql(" WHEN SUBSTR(JUMIN, 7, 1) IN('1', '2', '5', '6') THEN TO_CHAR(EXTRACT(YEAR FROM SYSDATE) -TO_NUMBER('19' || SUBSTR(JUMIN, 1, 2)))                                                ");
        //    parameter.AppendSql(" WHEN SUBSTR(JUMIN, 7, 1) IN('3', '4', '7', '8') THEN TO_CHAR(EXTRACT(YEAR FROM SYSDATE) -TO_NUMBER('20' || SUBSTR(JUMIN, 1, 2)))                                                ");
        //    parameter.AppendSql(" WHEN SUBSTR(JUMIN, 7, 1) IN('9', '0')           THEN TO_CHAR(EXTRACT(YEAR FROM SYSDATE) -TO_NUMBER('18' || SUBSTR(JUMIN, 1, 2)))                                                ");
        //    parameter.AppendSql(" ELSE '0'                                                ");
        //    parameter.AppendSql(" END AGE                                                ");
        //    parameter.AppendSql(" FROM HC_SITE_WORKER  A                                                 ");
        //    parameter.AppendSql(" WHERE ID = :ID                                                                ");
        //    parameter.AppendSql(" AND ISDELETED = 'N'                                                               ");
           
   
        //    parameter.Add("ID", id);

        //    return ExecuteReaderSingle<HC_SITE_WORKER>(parameter);
        //}
        //public List<HC_SITE_WORKER> FindAll(long siteId, string dept, string workerRole) {
        //    MParameter parameter = CreateParameter();
        //    parameter.AppendSql("SELECT * FROM HC_SITE_WORKER_VIEW                                                  ");
        //    parameter.AppendSql("WHERE SITEID = :SITEID                                                                ");
        //    parameter.AppendSql("AND ISDELETED = 'N'                                                               ");

        //    if(dept != "��ü" && dept != "")
        //    {
        //        parameter.AppendSql("AND DEPT = :DEPT                                                               ");
        //    }
        //    parameter.Add("SITEID", siteId);
        //    if (dept != "��ü" && dept != "")
        //    {
        //        parameter.Add("DEPT", dept);
        //    }
        //    if (!workerRole.IsNullOrEmpty())
        //    {
        //        parameter.AppendSql("AND WORKER_ROLE = :WORKER_ROLE                                                               ");
        //    }
        //    parameter.AppendSql(" ORDER BY NAME                                                              ");



        //    if (!workerRole.IsNullOrEmpty())
        //    {
        //        parameter.Add("WORKER_ROLE", workerRole);
        //    }
        //    return ExecuteReader<HC_SITE_WORKER>(parameter);

        //}
        public List<HC_SITE_WORKER> FindAllGroupByDept(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT DEPT FROM HC_SITE_WORKER_VIEW                                                  ");
            parameter.AppendSql(" WHERE SITEID = :SITEID                                                                ");
       //     parameter.AppendSql(" AND ISDELETED = 'N'                                                               ");
            parameter.AppendSql(" GROUP BY DEPT  ");

            parameter.Add("SITEID", siteId);

            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        public List<HC_SITE_WORKER> FindAllByName(long siteId, string name, string role)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("SELECT * FROM HC_SITE_WORKER_VIEW  ");
            parameter.AppendSql("WHERE SITEID = :SITEID                                                                ");
    //        parameter.AppendSql("AND ISDELETED = 'N'                                                               ");
            parameter.AppendSql("AND NAME = :NAME ");
            parameter.AppendSql("AND WORKER_ROLE = :ROLE ");
            parameter.Add("SITEID", siteId);
            parameter.Add("NAME", name);
            parameter.Add("ROLE", role);

            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        public List<HC_SITE_WORKER> FindAllByName(long siteId, string name)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HC_SITE_WORKER_VIEW  ");
            parameter.AppendSql("WHERE SITEID = :SITEID                                                                ");
       //     parameter.AppendSql("AND ISDELETED = 'N'                                                               ");
            parameter.AppendSql("AND NAME = :NAME   ");
           
            parameter.Add("SITEID", siteId);
            parameter.Add("NAME", name);
         

            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        public List<HC_SITE_WORKER> FindAll(long siteId, string name, string dept, string workerRole)
        {
            MParameter parameter = CreateParameter();
        
            parameter.AppendSql("SELECT * FROM HC_SITE_WORKER_VIEW  ");
            parameter.AppendSql("WHERE SITEID = :SITEID                                                                ");
            if (!workerRole.IsNullOrEmpty())
            {
                parameter.AppendSql("AND WORKER_ROLE = :WORKER_ROLE                                                               ");
            }
            if (!name.IsNullOrEmpty())
            {
                if (name.IsNumeric())
                {
                    parameter.AppendSql("AND PTNO LIKE :NAME ");
                }
                else
                {
                    parameter.AppendSql("AND NAME LIKE :NAME ");
                }
              
            }
            if (!dept.IsNullOrEmpty())
            {
                parameter.AppendSql("AND DEPT = :DEPT ");
            }
            parameter.AppendSql(" ORDER BY NAME                                                           ");
            parameter.Add("SITEID", siteId);
            
            if (!name.IsNullOrEmpty())
            {
                if (name.IsNumeric())
                {
                    parameter.AddLikeStatement("NAME", name);
                }
                else
                {
                    parameter.AddLikeStatement("NAME", name);
                }
                
            }
            if (!dept.IsNullOrEmpty())
            {
                parameter.Add("DEPT", dept);
            }
            if (!workerRole.IsNullOrEmpty())
            {
                parameter.Add("WORKER_ROLE", workerRole);
            }
            //    if (!workerRole.IsNullOrEmpty())
            //    {
            //        parameter.AppendSql("AND WORKER_ROLE = :WORKER_ROLE                                                               ");
            //    }
            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        /// <summary>
        /// WORKER_ROLE �ڵ忡 ���� �ٷ��� ��������
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<HC_SITE_WORKER> FindWorkerRole(long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.* FROM HC_SITE_WORKER_VIEW A ");
            parameter.AppendSql("       INNER JOIN HIC_CODES B ");
            parameter.AppendSql("             ON A.WORKER_ROLE = B.CODE ");
            parameter.AppendSql("             AND B.CODE <> 'EMP_ROLE' AND B.ISDELETED ='N' ");
            parameter.AppendSql(" WHERE A.SITEID = :SITEID ");
            parameter.AppendSql("   AND A.SWLICENSE=:SWLICENSE ");
            parameter.Add("SITEID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        public List<HC_SITE_WORKER> FindWorker(string worker_role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.* FROM HC_SITE_WORKER_VIEW A                                            ");
            parameter.AppendSql("WHERE A.WORKER_ROLE  = :WORKER_ROL                                                 ");

            parameter.Add("WORKER_ROL", worker_role);
            return ExecuteReader<HC_SITE_WORKER>(parameter);
        }

        public List<HC_SITE_WORKER> FindWorkerByRole(long siteId, string worker_role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.* FROM HC_SITE_WORKER_VIEW A ");
            parameter.AppendSql("       INNER JOIN HIC_CODES B ");
            parameter.AppendSql("          ON A.WORKER_ROLE = B.CODE ");
            if (worker_role.NotEmpty())
            {
                parameter.AppendSql("     AND B.CODE =:worker_role ");
            }
            
            parameter.AppendSql("         AND B.ISDELETED ='N' ");
            parameter.AppendSql(" WHERE A.SITEID = :SITEID ");
            parameter.AppendSql("   AND A.SWLICENSE=:SWLICENSE ");

            parameter.Add("SITEID", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            if (worker_role.NotEmpty())
            {
                parameter.Add("worker_role", worker_role);
            }
             
            return ExecuteReader<HC_SITE_WORKER>(parameter);

        }
        public HC_SITE_WORKER Insert(HC_SITE_WORKER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEQ_WORD        ");
            parameter.AppendSql("  FROM HIC_USERS       ");
            parameter.AppendSql(" WHERE USERID = :USERID");
            
            parameter.Add("USERID", CommonService.Instance.Session.UserId);

            string word = ExecuteScalar<string>(parameter);
            if(word.Empty())
            {
                word = "P";
            }

            dto.ID = string.Concat(word, "-", GetSequenceNextVal("HC_SITE_WORKER_ID_SEQ"));
            parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_SITE_WORKER  ");
            parameter.AppendSql("(                            ");
            parameter.AppendSql("  ID,                        ");
            parameter.AppendSql("  SITEID,                    ");
            parameter.AppendSql("  NAME,                      ");
            parameter.AppendSql("  WORKER_ROLE,               ");
            parameter.AppendSql("  DEPT,                      ");
            parameter.AppendSql("  TEL,                       ");
            parameter.AppendSql("  JUMIN,                     ");
            parameter.AppendSql("  HP,                        ");
            parameter.AppendSql("  EMAIL,                     ");
            parameter.AppendSql("  PANO,                      ");
            parameter.AppendSql("  PTNO,                      ");
            parameter.AppendSql("  IPSADATE,                  ");
            parameter.AppendSql("  ISDELETED,                 ");
            parameter.AppendSql("  ISRETIRE,                  ");
            parameter.AppendSql("  MODIFIED,                  ");
            parameter.AppendSql("  MODIFIEDUSER,              ");
            parameter.AppendSql("  CREATED,                   ");
            parameter.AppendSql("  CREATEDUSER                ");
            parameter.AppendSql(")                            ");
            parameter.AppendSql("VALUES                       ");
            parameter.AppendSql("(                            ");
            parameter.AppendSql("  :ID,                       ");
            parameter.AppendSql("  :SITEID,                   ");
            parameter.AppendSql("  :NAME,                     ");
            parameter.AppendSql("  :WORKER_ROLE,              ");
            parameter.AppendSql("  :DEPT,                     ");
            parameter.AppendSql("  :TEL,                      ");
            parameter.AppendSql("  :JUMIN,                    ");
            parameter.AppendSql("  :HP,                       ");
            parameter.AppendSql("  :EMAIL,                    ");
            parameter.AppendSql("  :PANO,                     ");
            parameter.AppendSql("  :PTNO,                     ");
            parameter.AppendSql("  :IPSADATE,                 ");
            parameter.AppendSql("  'N',                       ");
            parameter.AppendSql("  :ISRETIRE,                 ");
            parameter.AppendSql("  SYSTIMESTAMP,              ");
            parameter.AppendSql("  :MODIFIEDUSER,             ");
            parameter.AppendSql("  SYSTIMESTAMP,              ");
            parameter.AppendSql("  :CREATEDUSER               ");
            parameter.AppendSql(")                            ");
            parameter.Add("ID", dto.ID);
            parameter.Add("SITEID", dto.SITEID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("DEPT", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("JUMIN", dto.JUMIN);
            parameter.Add("HP", dto.HP);
            parameter.Add("EMAIL", dto.EMAIL);
            parameter.Add("PANO", dto.PANO);
            parameter.Add("PTNO", dto.PTNO);
            parameter.Add("IPSADATE", dto.IPSADATE);
            parameter.Add("ISRETIRE", dto.ISRETIRE);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Insert("HIC_SITE_WORKER", dto.ID);

            return FindOne(dto.ID);
        }
        public void UpdatePatientWorkerRole(HC_SITE_WORKER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_PATIENT                                                       ");
            parameter.AppendSql("SET WORKER_ROLE = :WORKER_ROLE                                           ");
            parameter.AppendSql(", EMAIL = :EMAIL                                                 ");
            parameter.AppendSql(", BuseName = :BuseName                                           ");
            parameter.AppendSql(", TEL = :TEL                                                    ");
            parameter.AppendSql(", HPHONE = :HPHONE                                            ");
            parameter.AppendSql(",  ISMANAGEOSHA = :ISMANAGEOSHA                                   ");
            parameter.AppendSql("WHERE PTNO = :PTNO                                                ");
            parameter.Add("PTNO", dto.PTNO);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("EMAIL", dto.EMAIL);
            parameter.Add("BuseName", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("HPHONE", dto.HP);
            parameter.Add("ISMANAGEOSHA", dto.ISMANAGEOSHA);
            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Update("HIC_PATIENT", dto.ID);
        }
        public HC_SITE_WORKER Update(HC_SITE_WORKER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_SITE_WORKER                                                       ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  NAME = :NAME,                                                             ");
            parameter.AppendSql("  WORKER_ROLE = :WORKER_ROLE,                                               ");
            parameter.AppendSql("  DEPT = :DEPT,                                                             ");
            parameter.AppendSql("  TEL = :TEL,                                                               ");
            parameter.AppendSql("  JUMIN = :JUMIN,                                                               ");
            parameter.AppendSql("  HP = :HP,                                                                 ");
            parameter.AppendSql("  EMAIL = :EMAIL,                                                           ");
            parameter.AppendSql("  PTNO = :PTNO,                                                           ");
            parameter.AppendSql("  PANO = :PANO,                                                           ");
            parameter.AppendSql("  IPSADATE = :IPSADATE,                                                           ");
            parameter.AppendSql("  ISRETIRE = :ISRETIRE,                                                      ");
            parameter.AppendSql("  ISMANAGEOSHA = :ISMANAGEOSHA,                                                      ");
            
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                              ");
            parameter.AppendSql("WHERE ID = :ID                                                              ");
            parameter.Add("ID", dto.ID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("DEPT", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("JUMIN", dto.JUMIN);
            parameter.Add("HP", dto.HP);
            parameter.Add("EMAIL", dto.EMAIL);
            parameter.Add("PTNO", dto.PTNO);
            parameter.Add("PANO", dto.PANO);
            parameter.Add("IPSADATE", dto.IPSADATE);
            parameter.Add("ISRETIRE", dto.ISRETIRE);
            parameter.Add("ISMANAGEOSHA", dto.ISMANAGEOSHA);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_SITE_WORKER", dto.ID);

            return FindOne(dto.ID);
        }

        public void Delete(string id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE  HIC_SITE_WORKER                                                 ");
            parameter.AppendSql("SET ISDELETED = 'Y',                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                             ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                         ");
            parameter.AppendSql("WHERE ID = :ID                                                         ");
            parameter.Add("ID", id);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);

            DataSyncService.Instance.Delete("HIC_SITE_WORKER", id);

            ExecuteNonQuery(parameter);

        }
    }
}
