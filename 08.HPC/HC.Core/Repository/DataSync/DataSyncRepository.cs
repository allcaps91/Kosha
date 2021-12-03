namespace HC.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC.Core.Dto;
    using HC.Core.Model;
    using HC.Core.Service;
    using HC_Core.Model;
    using HC_Core.Model.DataSync;


    /// <summary>
    /// 
    /// </summary>
    public class DataSyncRepository : BaseRepository
    {

        public void CompleteSync(long id)
        {
            DataSyncDto dto = FindOne(id);
            MParameter parameter = CreateParameter();

            //       parameter.AppendSql("UPDATE HIC_OSHA_DATASYNC SET ISSYNC='Y', MESSAGE='' WHERE TABLENAME = :TABLENAME AND TABLEKEY = :TABLEKEY");
            // parameter.Add("TABLENAME", dto.TABLENAME);
            // parameter.Add("TABLEKEY", dto.TABLEKEY);
            parameter.AppendSql("UPDATE HIC_OSHA_DATASYNC SET ISSYNC='Y', MESSAGE=''");
            parameter.AppendSql(" , UPLOADDATE = SYSTIMESTAMP ");
            parameter.AppendSql("   WHERE ID = :ID         ");
            parameter.Add("ID", id);
            ExecuteNonQuery(parameter);
        }
        public void FaildSync(long id, string message)
        {
            DataSyncDto dto = FindOne(id);
            MParameter parameter = CreateParameter();
            //parameter.AppendSql("UPDATE HIC_OSHA_DATASYNC SET ISSYNC='Y', MESSAGE=:MESSAGE WHERE TABLENAME = :TABLENAME AND TABLEKEY = :TABLEKEY");
            //parameter.Add("TABLENAME", dto.TABLENAME);
            //parameter.Add("TABLEKEY", dto.TABLEKEY);
            //parameter.Add("MESSAGE", message);

            parameter.AppendSql("UPDATE HIC_OSHA_DATASYNC SET ISSYNC='N'");
            parameter.AppendSql(" , MESSAGE = :MESSAGE");
            parameter.AppendSql(" , UPLOADDATE = SYSTIMESTAMP ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.Add("ID", id);
            parameter.Add("MESSAGE", message);
            ExecuteNonQuery(parameter);
        }
        public void UpdateNewKey(string tableKey, string newTableKey, string tableName, string createdUser)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_DATASYNC ");
            parameter.AppendSql("SET NEWTABLEKEY = :newTableKey ");
            parameter.AppendSql("WHERE TABLENAME = :tableName ");
            parameter.AppendSql("AND TABLEKEY = :tableKey ");
            parameter.AppendSql("AND CREATEDUSER = :createdUser ");

            parameter.Add("tableKey", tableKey);
            parameter.Add("tableName", tableName);
            parameter.Add("newTableKey", newTableKey);
            parameter.Add("createdUser", createdUser);
            ExecuteNonQuery(parameter);
        }
        public void UpdateNewKey(long id, string newTableKey)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_DATASYNC ");
            parameter.AppendSql("SET NEWTABLEKEY = :newTableKey ");
            parameter.AppendSql("WHERE id = :id ");

            parameter.Add("id", id);
            parameter.Add("newTableKey", newTableKey);
            ExecuteNonQuery(parameter);
        }
        public void UpdateHealthChek(long newReportId, long reportId, long siteId, string isDoctor)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_HEALTHCHECK ");
            parameter.AppendSql("SET REPORT_ID = :NEW_REPORTID ");      
            parameter.AppendSql("WHERE REPORT_ID = :REPORTID ");
            parameter.AppendSql("AND SITE_ID = :SITE_ID ");
            parameter.AppendSql("AND ISDOCTOR = :ISDOCTOR ");

            parameter.Add("NEW_REPORTID", newReportId);
            parameter.Add("REPORTID", reportId);
            parameter.Add("SITE_ID", siteId);
            parameter.Add("ISDOCTOR", isDoctor);
            ExecuteNonQuery(parameter);

        }
        public List<DataSyncDto> GetNotSyncAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_DATASYNC WHERE ISSYNC ='N'   ");
            parameter.AppendSql("AND CREATEDUSER = :CREATEDUSER ");
            parameter.AppendSql(" ORDER BY ID, TABLENAME ");
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
                //parameter.AppendSql("   SELECT* FROM HIC_OSHA_DATASYNC A  ");
            //parameter.AppendSql("   INNER JOIN(SELECT max(ID) AS ID FROM HIC_OSHA_DATASYNC  ");
            //parameter.AppendSql("             WHERE ISSYNC = 'N'  ");
            //parameter.AppendSql("             GROUP BY TABLEKEY, tablename  ");
            //parameter.AppendSql("            ) B  ");
            //parameter.AppendSql("   ON A.Id = B.ID  ");
            return ExecuteReader<DataSyncDto>(parameter);
        }
    
        public void DeleteSync()
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh" || servieName == "ora7")
            {
                MParameter parameter = CreateParameter();
                parameter.AppendSql("DELETE FROM HIC_OSHA_DATASYNC ");

                ExecuteNonQuery(parameter);
            }
      
        }
        public DataSyncDto FindOne(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_DATASYNC ");
            parameter.AppendSql("WHERE ID = :ID");
            parameter.Add("ID", id);

            return ExecuteReaderSingle<DataSyncDto>(parameter);
        }
        public DataSyncDto FindNewTabnleKey(string tableName, string createdUser, string tableKey)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_DATASYNC ");
            parameter.AppendSql("WHERE TABLENAME = :TABLENAME");
            parameter.AppendSql("AND CREATEDUSER = :CREATEDUSER");
            parameter.AppendSql("AND TABLEKEY = :TABLEKEY");
            parameter.AppendSql("AND DMLTYPE = 'I' ");
            parameter.Add("TABLENAME", tableName);
            parameter.Add("CREATEDUSER", createdUser);
            parameter.Add("TABLEKEY", tableKey);

            return ExecuteReaderSingle<DataSyncDto>(parameter);
        }
        public DataSyncDto FindOne(string tableName, string tableKey)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_DATASYNC ");
            parameter.AppendSql("WHERE TABLENAME = :TABLENAME");
            parameter.AppendSql("AND TABLEKEY = :TABLEKEY");
            parameter.Add("TABLENAME", tableName);
            parameter.Add("TABLEKEY", tableKey);

            return ExecuteReaderSingle<DataSyncDto>(parameter);
        }
        public List<DataSyncDto> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_DATASYNC ");            
            return ExecuteReader<DataSyncDto>(parameter);
        }
        public List<DataSyncDto> FindAllByUserId(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_OSHA_DATASYNC  WHERE CREATEDUSER = :USERID");
            parameter.Add("USERID", userId);
            return ExecuteReader<DataSyncDto>(parameter);
        }
      
        public List<DataSyncModel> FinAll(string startDate, string endDate, string isSync)
        {
            startDate += " 00:00:00";
            endDate += " 23:59:59";
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.*, B.NAME as CREATEDUSERNAME, C.COMMENTS FROM HIC_OSHA_DATASYNC A  ");
       //     parameter.AppendSql(" INNER JOIN(SELECT max(ID) AS ID FROM HIC_OSHA_DATASYNC                    ");
       ////     parameter.AppendSql("                        WHERE ISSYNC = 'N'                               ");
       //     parameter.AppendSql("                         GROUP BY TABLEKEY, tablename                  ");
       //     parameter.AppendSql("                   ) BB                                               ");
       //     parameter.AppendSql("        ON A.Id = BB.ID                                             ");
            parameter.AppendSql("INNER JOIN HIC_USERS B                                              ");
            parameter.AppendSql("ON A.CREATEDUSER = B.USERID                                          ");
            parameter.AppendSql("INNER JOIN USER_TAB_COMMENTS C                                       ");
            parameter.AppendSql("ON A.TABLENAME = C.TABLE_NAME                                       ");
            parameter.AppendSql("WHERE 1=1                                  ");
            parameter.AppendSql("AND A.CREATEDUSER = :CREATEDUSER           ");
            parameter.AppendSql("AND A.CREATED >= TO_TIMESTAMP(:STARTDATE, 'YYYY-MM-DD HH24:MI:SS')         ");
            parameter.AppendSql("AND A.CREATED <= TO_TIMESTAMP(:ENDDATE, 'YYYY-MM-DD HH24:MI:SS')         ");
            if (!isSync.IsNullOrEmpty())
            {
                parameter.AppendSql("AND A.ISSYNC = :ISSYNC     ");
            }
            parameter.AppendSql("ORDER BY  A.ID       ");
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            if (!isSync.IsNullOrEmpty())
            {
                parameter.Add("ISSYNC", isSync);
            }
          
            return ExecuteReader<DataSyncModel>(parameter);
        }
        public void Insert(DataSyncDto dto)
        {
          
            dto.ID = GetSequenceNextVal("HIC_OSHA_DATASYNC_ID_SEQ");
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_OSHA_DATASYNC");
            parameter.AppendSql("(");
            parameter.AppendSql("    ID");
            parameter.AppendSql("  , TABLENAME");
            parameter.AppendSql("  , TABLEKEY");
            parameter.AppendSql("  , NEWTABLEKEY");
            parameter.AppendSql("  , DMLTYPE");
            parameter.AppendSql("  , ISSYNC");
            parameter.AppendSql("  , CREATED");
            parameter.AppendSql("  , CREATEDUSER");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :ID");
            parameter.AppendSql("  , :TABLENAME");
            parameter.AppendSql("  , :TABLEKEY");
            parameter.AppendSql("  , :NEWTABLEKEY");
            parameter.AppendSql("  , :DMLTYPE");
            parameter.AppendSql("  , :ISSYNC");
            if (dto.CREATED==null)
            {
                parameter.AppendSql("  , SYSTIMESTAMP");
            }
            else
            {
                parameter.AppendSql("  , :CREATED");
            }
                
            parameter.AppendSql("  , :CREATEDUSER");
            parameter.AppendSql(") ");

            parameter.Add("ID", dto.ID);
            parameter.Add("TABLENAME", dto.TABLENAME);
            parameter.Add("TABLEKEY", dto.TABLEKEY);
            parameter.Add("NEWTABLEKEY", dto.NEWTABLEKEY);
            parameter.Add("DMLTYPE", dto.DMLTYPE);
            parameter.Add("ISSYNC", dto.ISSYNC);
            if (dto.CREATED != null)
            {
                parameter.Add("CREATED", dto.CREATED);
            }
         
            if (dto.CREATEDUSER.IsNullOrEmpty())
            {
                parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            }
            else
            {
                parameter.Add("CREATEDUSER", dto.CREATEDUSER);
            }
            

            ExecuteNonQuery(parameter);

        }

        public List<DbLinkModel> GetTableComments()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("  SELECT table_name as TableName, comments as Description ");
            parameter.AppendSql("  FROM USER_TAB_COMMENTS ");
            parameter.AppendSql("  WHERE comments IS NOT NULL ");
          //  parameter.AppendSql("  AND table_name ='HIC_MACROWORD' ");
            return  ExecuteReader<DbLinkModel>(parameter);
        }
        public bool HasTable(string tableName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TABLE_NAME FROM ALL_TABLES ");
            parameter.AppendSql("WHERE TABLE_NAME = :tableName ");
            parameter.AppendSql("AND OWNER = 'ADMIN' ");
            parameter.Add("tableName", tableName);
            if (ExecuteScalar<string>(parameter) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void TruncateTable(string tableName)
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
            {
                MParameter parameter = CreateParameter();
                parameter.AppendSql("TRUNCATE TABLE " + tableName);

                ExecuteNonQuery(parameter);
            }
               
        }
      
        public void DropTable(string tableName)
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
            {
                MParameter parameter = CreateParameter();
                parameter.AppendSql("DROP TABLE " + tableName + " CASCADE CONSTRAINTS");

                ExecuteNonQuery(parameter);
            }
   
        }
        public void FlashbackTable(string tableName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("FLASHBACK TABLE " + tableName + " To BEFORE DROP");

            ExecuteNonQuery(parameter);
        }

        public void CreateTableByDbLink(string tableName)
        {

            if (HasTable(tableName))
            {
                DropTable(tableName);
            }

            MParameter parameter = CreateParameter();
            parameter.AppendSql("CREATE TABLE " + tableName + " AS  ");
            parameter.AppendSql("SELECT * FROM " + tableName + "@HICLINK  ");

            ExecuteNonQuery(parameter);
        }
        public void CreateWorkerView()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("        CREATE OR REPLACE FORCE VIEW ADMIN.HC_SITE_WORKER_VIEW           ");
            parameter.AppendSql("       (                                                                                  ");
            parameter.AppendSql("          ID,                                                                             ");
            parameter.AppendSql("          PTNO,                                                                           ");
            parameter.AppendSql("          SITEID,                                                                         ");
            parameter.AppendSql("          NAME,                                                                           ");
            parameter.AppendSql("          WORKER_ROLE,                                                                    ");
            parameter.AppendSql("          DEPT,                                                                           ");
            parameter.AppendSql("          TEL,                                                                            ");
            parameter.AppendSql("          HP,                                                                             ");
            parameter.AppendSql("          EMAIL,                                                                          ");
            parameter.AppendSql("          JUMIN,                                                                          ");
            parameter.AppendSql("          PANO,                                                                            ");
            parameter.AppendSql("          ISMANAGEOSHA                                                                            ");
            parameter.AppendSql("       )                                                                                  ");
            parameter.AppendSql("       AS                                                                                 ");
            parameter.AppendSql("          SELECT A.PTNO AS ID,                                                            ");
            parameter.AppendSql("                 A.PTNO,                                                                  ");
            parameter.AppendSql("                 A.LTDCODE,                                                               ");
            parameter.AppendSql("                 A.SNAME,                                                                 ");
            parameter.AppendSql("                 NVL(WORKER_ROLE, 'EMP_ROLE'),                                            ");
            parameter.AppendSql("                 A.BuseName AS DEPT,                                                      ");
            parameter.AppendSql("                 A.TEL,                                                                   ");
            parameter.AppendSql("                 A.HPHONE,                                                                ");
            parameter.AppendSql("                 A.EMAIL,                                                                 ");
            parameter.AppendSql("                 A.JUMIN2,                                                                ");
            parameter.AppendSql("                 A.PANO,                                                                   ");
            parameter.AppendSql("                 NVL (ISMANAGEOSHA, 'N')                                                             ");
            parameter.AppendSql("            FROM HIC_PATIENT A                                                            ");
            parameter.AppendSql("          UNION                                                                           ");
            parameter.AppendSql("          SELECT ID,                                                                      ");
            parameter.AppendSql("                 PTNO,                                                                    ");
            parameter.AppendSql("                 SITEID,                                                                  ");
            parameter.AppendSql("                 NAME,                                                                    ");
            parameter.AppendSql("                 WORKER_ROLE,                                                             ");
            parameter.AppendSql("                 DEPT,                                                                    ");
            parameter.AppendSql("                 TEL,                                                                     ");
            parameter.AppendSql("                 HP,                                                                      ");
            parameter.AppendSql("                 EMAIL,                                                                   ");
            parameter.AppendSql("                 JUMIN,                                                                   ");
            parameter.AppendSql("                 PANO,                                                                     ");
            parameter.AppendSql("                 NVL (ISMANAGEOSHA, 'N')                                                             ");
            parameter.AppendSql("            FROM HIC_SITE_WORKER                                                          ");
            parameter.AppendSql("           WHERE ISDELETED = 'N'              ");
            ExecuteNonQuery(parameter);
        }
        public void CreateSiteView()
        {
            MParameter parameter = CreateParameter();
            // parameter.AppendSql("  DROP VIEW ADMIN.HC_SITE_VIEW ");

            parameter.AppendSql("CREATE OR REPLACE FORCE VIEW HC_SITE_VIEW              ");
            parameter.AppendSql("(                                                      ");
            parameter.AppendSql("   ID,                                                 ");
            parameter.AppendSql("   NAME,                                               ");
            parameter.AppendSql("   TEL,                                                ");
            parameter.AppendSql("   FAX,                                                ");
            parameter.AppendSql("   EMAIL,                                              ");
            parameter.AppendSql("   ADDRESS,                                            ");
            parameter.AppendSql("   BIZNUMBER,                                          ");
            parameter.AppendSql("   BIZTYPE,                                            ");
            parameter.AppendSql("   BizUPJONG,                                          ");
            parameter.AppendSql("   BIZJONG,                                            ");
            parameter.AppendSql("   BIZKIHO,                                            ");
            parameter.AppendSql("   BIZCREATEDATE,                                      ");
            parameter.AppendSql("   INSURANCE,                                          ");
            parameter.AppendSql("   INDUSTRIALNUMBER,                                   ");
            parameter.AppendSql("   BIZJIDOWON,                                         ");
            parameter.AppendSql("   LABOR,                                              ");
            parameter.AppendSql("   CEONAME,                                            ");
            parameter.AppendSql("   GBGUKGO,                                            ");
            parameter.AppendSql("   DELDATE,                                            ");
            parameter.AppendSql("   GBDAEHANG,                                          ");
            parameter.AppendSql("   JEPUMLIST                                           ");
            parameter.AppendSql(")                                                      ");
            parameter.AppendSql("   AS                                                  ");
            parameter.AppendSql("SELECT A.CODE AS ID                                    ");
            parameter.AppendSql("     , A.SANGHO AS NAME                                ");
            parameter.AppendSql("     , A.TEL                                           ");
            parameter.AppendSql("     , A.FAX                                           ");
            parameter.AppendSql("     , A.EMAIL                                         ");
            parameter.AppendSql("     , CONCAT(A.JUSO, A.JUSODETAIL) AS ADDRESS         ");
            parameter.AppendSql("     , A.SAUPNO AS BIZNUMBER                           ");
            parameter.AppendSql("     , A.UPTAE AS BIZTYPE                              ");
            parameter.AppendSql("     , A.UPJONG AS BIZUPJONG                           ");
            parameter.AppendSql("     , A.JONGMOK AS BIZJONG                            ");
            parameter.AppendSql("     , A.SANKIHO AS BIZKIHO                            ");
            parameter.AppendSql("     , A.SELDATE AS BIZCREATEDATE                      ");
            parameter.AppendSql("     , B.NAME AS INSURANCE                             ");
            parameter.AppendSql("     , A.SANKIHO AS INDUSTRIALNUMBER                   ");
            parameter.AppendSql("     , B.NAME AS BIZJIDOWON                            ");
            parameter.AppendSql("     , B.NAME AS LABOR                                 ");
            parameter.AppendSql("     , DAEPYO AS CEONAME                               ");
            parameter.AppendSql("     , A.GBGUKGO AS GBGUKGO                            ");
            parameter.AppendSql("     , A.DELDATE                                       ");
            parameter.AppendSql("     , A.GBDAEHANG                                     ");
            parameter.AppendSql("     , A.JEPUMLIST                                     ");
            parameter.AppendSql("  FROM HIC_LTD A                                       ");
            parameter.AppendSql("  LEFT OUTER JOIN HIC_CODE B                           ");
            parameter.AppendSql("          ON A.JISA = B.CODE                           ");
            parameter.AppendSql("         AND B.GUBUN = '21'                            ");
            //parameter.AppendSql("  LEFT OUTER JOIN HIC_OSHA_CONTRACT C                  ");
            //parameter.AppendSql("          ON A.CODE = C.OSHA_SITE_ID                   ");
            parameter.AppendSql(" WHERE A.GBDAEHANG = 'Y'                               ");
            //parameter.AppendSql("   AND (C.TERMINATEDATE IS NULL OR TO_DATE(C.TERMINATEDATE, 'YYYY-MM-DD') >= SYSDATE)  ");
            //parameter.AppendSql("   UNION ALL                                                                                                     ");
            //parameter.AppendSql("   SELECT A.CODE AS ID,                                                                                          ");
            //parameter.AppendSql("          A.SANGHO AS NAME,                                                                                      ");
            //parameter.AppendSql("          A.TEL,                                                                                                 ");
            //parameter.AppendSql("          A.FAX,                                                                                                 ");
            //parameter.AppendSql("          A.EMAIL,                                                                                               ");
            //parameter.AppendSql("          CONCAT(A.JUSO, A.JUSODETAIL) AS ADDRESS,                                                               ");
            //parameter.AppendSql("         A.SAUPNO AS BizNumber,                                                                                  ");
            //parameter.AppendSql("          A.UPTAE AS BizType,                                                                                    ");
            //parameter.AppendSql("          A.UPJONG AS BizUPJONG,                                                                                 ");
            //parameter.AppendSql("          A.JONGMOK AS BizJong,                                                                                  ");
            //parameter.AppendSql("          A.SANKIHO AS BizKiho,                                                                                  ");
            //parameter.AppendSql("          A.SELDATE AS BizCreateDate,                                                                            ");
            //parameter.AppendSql("          B.NAME AS Insurance,                                                                                   ");
            //parameter.AppendSql("          A.SANKIHO AS IndustrialNumber,                                                                         ");
            //parameter.AppendSql("          B.NAME AS BizJidowon,                                                                                  ");
            //parameter.AppendSql("          B.NAME AS Labor,                                                                                       ");
            //parameter.AppendSql("          DAEPYO AS CeoName,                                                                                     ");
            //parameter.AppendSql("          A.GBGUKGO AS GBGUKGO,                                                                                  ");
            //parameter.AppendSql("          A.DELDATE,                                                                                             ");
            //parameter.AppendSql("          A.gbdaehang                                                                                            ");
            //parameter.AppendSql("     FROM    ADMIN.HIC_LTD A                                                                               ");
            //parameter.AppendSql("          LEFT OUTER JOIN                                                                                        ");
            //parameter.AppendSql("             HIC_CODE B                                                                                          ");
            //parameter.AppendSql("          ON A.JISA = B.CODE AND B.GUBUN = '21'                                                                  ");
            //parameter.AppendSql("    WHERE A.CODE = 42399                                                                              ");

            ExecuteNonQuery(parameter);


        }

        public void UpdateHealthcheckWorker(string workerId, string userId, long healthCheckId)
        {
            MParameter parameter = CreateParameter();
            //신규 근로자 아이디를 가져온다.
            parameter.AppendSql("   select newtablekey From HIC_OSHA_DATASYNC    ");
            parameter.AppendSql("   where tablename = 'HIC_SITE_WORKER'    ");
            parameter.AppendSql("   and createduser = :userId    ");
            parameter.AppendSql("   and tablekey = :tablekey    ");
            parameter.AppendSql("   and dmltype = 'I'    ");
            parameter.AppendSql("   and issync = 'Y'    ");
            parameter.Add("tablekey", workerId);
            parameter.Add("userId", userId);

            string newWorkerId = ExecuteScalar<string>(parameter);

            if (!newWorkerId.IsNullOrEmpty())
            {
                parameter = CreateParameter();
                parameter.AppendSql(" UPDATE HIC_OSHA_HEALTHCHECK ");
                parameter.AppendSql(" SET WORKER_ID = :NEWWORKERID");
                parameter.AppendSql(" WHERE ID = :healthCheckId ");
                parameter.AppendSql(" AND CREATEDUSER = :userId    ");
                parameter.Add("NEWWORKERID", newWorkerId);
                parameter.Add("healthCheckId", healthCheckId );
                parameter.Add("userId", userId);

                ExecuteNonQuery(parameter);

            }
            
        }

        //ora7서버의 신규근로자 새로운 키를 가져와서 
        public void UpdateMemoWorker(string workerId, string userId)
        {
            MParameter parameter = CreateParameter();
            //신규 근로자 아이디를 가져온다.
            parameter.AppendSql("   select newtablekey From HIC_OSHA_DATASYNC    ");
            parameter.AppendSql("   where tablename = 'HIC_SITE_WORKER'    ");
            parameter.AppendSql("   and createduser = :userId    ");
            parameter.AppendSql("   and tablekey = :tablekey    ");
            parameter.AppendSql("   and dmltype = 'I'    ");
            parameter.AppendSql("   and issync = 'Y'    ");
            parameter.Add("tablekey", workerId);
            parameter.Add("userId", userId);

            string newWorkerId = ExecuteScalar<string>(parameter);

            if (!newWorkerId.IsNullOrEmpty())
            {

                //메모
                parameter = CreateParameter();
                parameter.AppendSql(" UPDATE HIC_OSHA_PATIENT_MEMO ");
                parameter.AppendSql(" SET WORKER_ID = :NEWWORKERID");
                parameter.AppendSql(" WHERE WORKER_ID = :workerId ");
                parameter.Add("NEWWORKERID", newWorkerId);
                parameter.Add("workerId", workerId);

                ExecuteNonQuery(parameter);
            }

        }
        public void UpdateNotebookSync()
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
            {
                MParameter parameter = CreateParameter();
                parameter.AppendSql("   UPDATE HIC_OSHA_DATASYNC SET ISSYNC = 'Y'  ");

                ExecuteNonQuery(parameter);
            }
             
        }
        public void UpdatePassword(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("    UPDATE HIC_USERS A  ");
            parameter.AppendSql("    SET A.PASSHASH256 = (SELECT PASSHASH256 FROM BAS_USER WHERE TRIM(IDNUMBER) = :USERID)  ");
            parameter.AppendSql("    WHERE A.USERID = :USERID  ");

            parameter.Add("USERID", userId);

            ExecuteNonQuery(parameter);
        }
        public void UpdatePasswordByNotbook(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("    UPDATE HIC_USERS A  ");
            parameter.AppendSql("    SET A.PASSHASH256 = (SELECT PASSHASH256 FROM BAS_USER@HICLINK  WHERE TRIM(IDNUMBER) = :USERID)  ");
            parameter.AppendSql("    WHERE A.USERID = :USERID  ");

            parameter.Add("USERID", userId);

            ExecuteNonQuery(parameter);
        }
        public List<SequenceModel> GetSequencesSql()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("     SELECT 'CREATE SEQUENCE ' || sequence_name ||        ");
            parameter.AppendSql("     ' INCREMENT BY ' || increment_by || ");
            parameter.AppendSql("     ' START WITH ' || TO_CHAR(last_number) || ' ' || ");
            parameter.AppendSql("     DECODE(cycle_flag, 'N', 'NOCYCLE', 'CYCLE') || '' as sequence_sql  , sequence_name     ");
            parameter.AppendSql("   FROM USER_SEQUENCES       ");
            parameter.AppendSql("   WHERE sequence_name like '%HC_%'       ");
            parameter.AppendSql("   OR sequence_name like '%HIC_OSHA%'   ");
            return ExecuteReader<SequenceModel>(parameter);
        }
        public void CreateSequence(List<SequenceModel> seqList)
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
            {
                MParameter parameter = null;
                foreach (SequenceModel model in seqList)
                {
                    try
                    {
                        parameter = CreateParameter();
                        parameter.AppendSql("DROP SEQUENCE " + model.sequence_name);
                        ExecuteNonQuery(parameter);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }

                    parameter = CreateParameter();
                    parameter.AppendSql(model.sequence_sql);
                    ExecuteNonQuery(parameter);
                   
                }
            }
    
        }
        public void CreateDataSyncSequence()
        {
            MParameter parameter = null;
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE" )
            {
                try
                {
                    parameter = CreateParameter();
                    parameter.AppendSql("DROP SEQUENCE HIC_OSHA_DATASYNC_ID_SEQ");
                    ExecuteNonQuery(parameter);

                    parameter = CreateParameter();
                    parameter.AppendSql("select MAX(ID) as ID from HIC_OSHA_DATASYNC");
                    DataSyncDto dto = ExecuteReaderSingle<DataSyncDto>(parameter);


                    parameter = CreateParameter();
                    parameter.AppendSql("CREATE SEQUENCE HIC_OSHA_DATASYNC_ID_SEQ INCREMENT BY 1 START WITH " + dto.ID + 1 + " NOCYCLE");
                    ExecuteNonQuery(parameter);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
      
        }
        public void Create_HIC_PATIENT()
        {
            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "xe")
            {
                try
                {
                    MParameter parameter = CreateParameter();
                    parameter.AppendSql(" DROP TABLE HIC_PATIENT    ");
                    ExecuteNonQuery(parameter);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                try
                {
                    MParameter parameter = CreateParameter();
                    parameter.AppendSql(" CREATE TABLE HIC_PATIENT    ");
                    parameter.AppendSql(" AS SELECT * FROM HIC_PATIENT@HICLINK     ");
                    parameter.AppendSql(" WHERE rownum = 1            ");
                    ExecuteNonQuery(parameter);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
         
        
        }
        //선택된 사업장별
        public void INSERT_HIC_PATIENT(long site_id)
        {
            try
            {
                string servieName = clsDB.DbCon.Con.ServiceName;
                if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
                {
                    MParameter parameter = CreateParameter();

                    parameter.AppendSql(" INSERT INTO HIC_PATIENT     ");
                    parameter.AppendSql(" SELECT * FROM HIC_PATIENT@HICLINK     ");
                    parameter.AppendSql(" WHERE LTDCODE = :SITE_ID     ");
                    parameter.Add("SITE_ID", site_id);

                    ExecuteNonQuery(parameter);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void INSERT_HIC_JEPSU(long site_id)
        {
            try
            {
                MParameter parameter = CreateParameter();
                parameter.AppendSql(" INSERT INTO HIC_JEPSU     ");
                parameter.AppendSql(" SELECT * FROM HIC_JEPSU@HICLINK     ");
                parameter.AppendSql(" WHERE LTDCODE = :SITE_ID     ");
                parameter.Add("SITE_ID", site_id);

                ExecuteNonQuery(parameter);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        public void INSERT_HIC_DBLINK(long site_id, string tableName)
        {
            try
            {
                MParameter parameter = CreateParameter();
                parameter.AppendSql(" INSERT INTO " + tableName                    );
                parameter.AppendSql("     SELECT B.* from HIC_JEPSU @HICLINK A    ");
                parameter.AppendSql("     INNER JOIN "+ tableName + " @HICLINK B    ");
                parameter.AppendSql("     ON A.WRTNO = B.WRTNO    ");
                parameter.AppendSql("     WHERE A.LTDCODE = :SITE_ID    ");
                parameter.Add("SITE_ID", site_id);

                ExecuteNonQuery(parameter);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
 
        public List<HIC_JEPSU_MODEL> GetHicJepsu(long site_id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT * FROM HIC_JEPSU     ");
            parameter.AppendSql(" WHERE LTDCODE = :SITE_ID    ");
            parameter.Add("SITE_ID", site_id);

            return ExecuteReader<HIC_JEPSU_MODEL>(parameter);
        }


     
    }
}

