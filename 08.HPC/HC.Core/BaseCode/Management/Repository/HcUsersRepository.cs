namespace HC.Core.BaseCode.Management.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using HC.Core.BaseCode.Management.Dto;
    using HC.Core.Common.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcUsersRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HcUsersRepository()
        {
        }
        public HC_USER FindOne(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_USERS WHERE USERID = :USERID                            ");
            parameter.Add("USERID", userId);

           return ExecuteReaderSingle<HC_USER>(parameter);
        }
        public List<HC_USER> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_USERS WHERE ISDELETED = 'N' ORDER BY NAME                            ");

            return ExecuteReader<HC_USER>(parameter);
        }
        public List<HC_USER> FindDoctors()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_USERS WHERE ISDELETED = 'N' AND ROLE = 'DOCTOR' AND ISACTIVE='Y' ORDER BY NAME                            ");

            return ExecuteReader<HC_USER>(parameter);
        }
        public List<HC_USER> FindNurse()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_USERS WHERE ISDELETED = 'N' AND ROLE = 'NURSE' AND ISACTIVE='Y' ORDER BY NAME                            ");
            
            return ExecuteReader<HC_USER>(parameter);
        }
        
        public List<HC_USER> FinnEngineerByOSHA()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_USERS WHERE ISDELETED = 'N' AND ROLE = 'ENGINEER' AND DEPT ='OSHA' AND ISACTIVE='Y' ORDER BY NAME                            ");
            return ExecuteReader<HC_USER>(parameter);
        }

        public List<HC_USER> FindOSHA()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HC_USERS WHERE ISDELETED = 'N'AND DEPT ='OSHA' AND ISACTIVE='Y' ORDER BY NAME                            ");
            return ExecuteReader<HC_USER>(parameter);
        }

        public int Delete(HC_USER user)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_USERS                            ");
            parameter.AppendSql("SET ISDELETED ='Y'                         ");
            parameter.AppendSql("WHERE USERID = :USERID                     ");

            parameter.Add("USERID", user.UserId);
            return ExecuteNonQuery(parameter);
        }
        public void Insert(HC_VIEW_USER dto, string role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HC_USERS                                                         ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  UserId,                                                                   ");
            parameter.AppendSql("  Name,                                                                     ");
            parameter.AppendSql("  Role,                                                                     ");
            parameter.AppendSql("  Dept,                                                                     ");
            parameter.AppendSql("  IsActive,                                                                 ");
            parameter.AppendSql("  ISDELETED,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER                                                               ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :UserId,                                                                  ");
            parameter.AppendSql("  :Name,                                                                    ");
            parameter.AppendSql("  :Role,                                                                    ");
            parameter.AppendSql("  :Dept,                                                                    ");
            parameter.AppendSql("  :IsActive,                                                                ");
            parameter.AppendSql("  :ISDELETED,                                                               ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER                                                             ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("UserId", dto.UserId.Trim());
            parameter.Add("Name", dto.Name.Trim());
            parameter.Add("Role", role);
            parameter.Add("Dept", dto.Dept);
            parameter.Add("IsActive", "Y");
            parameter.Add("ISDELETED", "N");
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);

            ExecuteNonQuery(parameter);
        }

        public void Update(HC_USER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HC_USERS                                                              ");
            parameter.AppendSql("SET                                                                         ");
            parameter.AppendSql("  Name = :Name,                                                             ");
            parameter.AppendSql("  Role = :Role,                                                             ");
            parameter.AppendSql("  Dept = :Dept,                                                             ");
            parameter.AppendSql("  IsActive = :IsActive,                                                     ");
            parameter.AppendSql("  ISDELETED = :ISDELETED,                                                   ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP,                                                  ");
            parameter.AppendSql("  MODIFIEDUSER = :MODIFIEDUSER                                             ");
            parameter.AppendSql("WHERE USERID = :USERID                                                              ");
            parameter.Add("USERID", dto.UserId);
            parameter.Add("Name", dto.Name);
            parameter.Add("Role", dto.Role);
            parameter.Add("Dept", dto.Dept);
            parameter.Add("IsActive", dto.IsActive);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);


            ExecuteNonQuery(parameter);
        }
    }
}
