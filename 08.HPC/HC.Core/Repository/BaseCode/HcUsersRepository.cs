namespace HC.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using ComBase.Mvc.Enums;
    using HC.Core.Dto;
    using HC.Core.Service;


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
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE USERID = :USERID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("USERID", userId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReaderSingle<HC_USER>(parameter);
        }
        public List<HC_USER> FindAll()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            //parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_USER>(parameter);
        }
        public List<HC_USER> FindDoctors()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            parameter.AppendSql("   AND SUBSTR(JIKMU,4,1)='Y' "); //대행의사
            //parameter.AppendSql("   AND ROLE = 'DOCTOR' ");
            parameter.AppendSql("   AND ISACTIVE='Y' ");
            parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY NAME ");

            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_USER>(parameter);
        }
        public List<HC_USER> FindNurse()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            parameter.AppendSql("   AND SUBSTR(JIKMU,5,1)='Y' "); //대행간호사
            //parameter.AppendSql("   AND ROLE = 'NURSE' ");
            parameter.AppendSql("   AND ISACTIVE='Y' ");
            parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            
            return ExecuteReader<HC_USER>(parameter);
        }
        
        public List<HC_USER> FinnEngineerByOSHA()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            //parameter.AppendSql("   AND ROLE = 'ENGINEER' ");
            //parameter.AppendSql("   AND DEPT ='OSHA' ");
            parameter.AppendSql("   AND SUBSTR(JIKMU,6,1)='Y' "); //대행 산업위생
            parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<HC_USER>(parameter);
        }

        public List<HC_USER> FindWEM()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            //parameter.AppendSql("   AND ROLE = 'ENGINEER' ");
            //parameter.AppendSql("   AND DEPT ='WEM' ");
            parameter.AppendSql("   AND (SUBSTR(JIKMU,7,1)='Y' OR SUBSTR(JIKMU,8,1)='Y') ");
            parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            //parameter.AppendSql("SELECT A.*, B.JUMIN3 from HIC_USERS A                        ");
            //parameter.AppendSql("INNER JOIN  ADMIN.INSA_MST B                        ");
            //parameter.AppendSql("ON A.USERID = trim(B.SABUN)                        ");
            //parameter.AppendSql("WHERE A.ISDELETED = 'N'                        ");
            //parameter.AppendSql("AND A.DEPT = 'WEM'                        ");

            return ExecuteReader<HC_USER>(parameter);
        }
        public List<HC_USER> FindAnaylist()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            parameter.AppendSql("   AND ROLE = 'ANALYST' ");
            //parameter.AppendSql("   AND DEPT ='WEM' ");
            parameter.AppendSql("   AND SUBSTR(JIKMU,8,1)='Y' ");
            parameter.AppendSql("   AND ISACTIVE='Y' ");
            parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql(" ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_USER>(parameter);
        }

        public List<HC_USER> FindOSHA()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            parameter.AppendSql("   AND (USERID='1' OR SUBSTR(JIKMU,4,1)='Y' OR SUBSTR(JIKMU,5,1)='Y' OR SUBSTR(JIKMU,6,1)='Y') ");
            //parameter.AppendSql("   AND DEPT ='OSHA' "); 
            parameter.AppendSql("   AND LTDUSER IS NULL ");
            parameter.AppendSql("   AND ISACTIVE='Y' ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            //관계사 사용자
            if (clsType.User.LtdUser != "") parameter.AppendSql(" AND USERID='" + clsType.User.IdNumber + "' ");
            parameter.AppendSql("  ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_USER>(parameter);
        }

        public HC_USER FindByName(string strSName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_USERS ");
            parameter.AppendSql(" WHERE ISDELETED = 'N' ");
            parameter.AppendSql("   AND NAME LIKE :SNAME ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("  ORDER BY NAME ");
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            parameter.AddLikeStatement("SNAME", strSName);

            return ExecuteReaderSingle<HC_USER>(parameter);
        }

        public int Delete(HC_USER user)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_USERS  ");
            parameter.AppendSql("   SET ISDELETED ='Y' ");
            parameter.AppendSql(" WHERE USERID = :USERID ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");
            parameter.Add("USERID", user.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteNonQuery(parameter);
        }
        public void Insert(HC_VIEW_USER dto, string role)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_USERS                                                         ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  UserId,                                                                   ");
            parameter.AppendSql("  Name,                                                                     ");
            parameter.AppendSql("  Role,                                                                     ");
            parameter.AppendSql("  Dept,                                                                     ");
            parameter.AppendSql("  CERTNO,                                                                     ");
            parameter.AppendSql("  IsActive,                                                                 ");
            parameter.AppendSql("  ISDELETED,                                                                ");
            parameter.AppendSql("  MODIFIED,                                                                 ");
            parameter.AppendSql("  MODIFIEDUSER,                                                             ");
            parameter.AppendSql("  CREATED,                                                                  ");
            parameter.AppendSql("  CREATEDUSER,                                                              ");
            parameter.AppendSql("  SWLICENSE                                                                 ");
            parameter.AppendSql(")                                                                           ");
            parameter.AppendSql("VALUES                                                                      ");
            parameter.AppendSql("(                                                                           ");
            parameter.AppendSql("  :UserId,                                                                  ");
            parameter.AppendSql("  :Name,                                                                    ");
            parameter.AppendSql("  :Role,                                                                    ");
            parameter.AppendSql("  :Dept,                                                                    ");
            parameter.AppendSql("  :CERTNO,                                                                    ");
            parameter.AppendSql("  :IsActive,                                                                ");
            parameter.AppendSql("  :ISDELETED,                                                               ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                ");
            parameter.AppendSql("  :MODIFIEDUSER,                                                            ");
            parameter.AppendSql("  SYSTIMESTAMP,                                                                 ");
            parameter.AppendSql("  :CREATEDUSER,                                                             ");
            parameter.AppendSql("  :SWLICENSE                                                             ");
            parameter.AppendSql(")                                                                           ");
            parameter.Add("UserId", dto.UserId.Trim());
            parameter.Add("Name", dto.Name.Trim());
            parameter.Add("Role", role);
            parameter.Add("Dept", dto.Dept);
            parameter.Add("CERTNO", dto.CERTNO);
            parameter.Add("IsActive", "Y");
            parameter.Add("ISDELETED", "N");
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("CREATEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }

        public void Update(HC_USER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_USERS                   ");
            parameter.AppendSql("   SET Name         = :Name        ");
            parameter.AppendSql("     , Role         = :Role        ");
            parameter.AppendSql("     , Dept         = :Dept        ");
            parameter.AppendSql("     , CERTNO       = :CERTNO      ");
            parameter.AppendSql("     , IsActive     = :IsActive    ");
            parameter.AppendSql("     , ISDELETED    = :ISDELETED   ");
            parameter.AppendSql("     , MODIFIED     = SYSTIMESTAMP ");
            parameter.AppendSql("     , MODIFIEDUSER = :MODIFIEDUSER");
            parameter.AppendSql("     , SEQ_WORD     = :SEQ_WORD    ");
            parameter.AppendSql("WHERE USERID = :USERID             ");
            parameter.AppendSql("   AND SWLICENSE = :SWLICENSE ");

            parameter.Add("USERID", dto.UserId);
            parameter.Add("Name", dto.Name);
            parameter.Add("Role", dto.Role);
            parameter.Add("Dept", dto.Dept);
            parameter.Add("CERTNO", dto.CERTNO);
            parameter.Add("SEQ_WORD", dto.SEQ_WORD);
            parameter.Add("IsActive", dto.IsActive);
            parameter.Add("ISDELETED", dto.ISDELETED);
            parameter.Add("MODIFIEDUSER", CommonService.Instance.Session.UserId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
    }
}
