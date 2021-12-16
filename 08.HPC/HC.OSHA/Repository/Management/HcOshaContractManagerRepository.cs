namespace HC.OSHA.Repository
{
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Mvc;
    using HC.OSHA.Dto;
    using HC.OSHA.Model;
    using HC_Core.Service;

    /// <summary>
    /// 사업장 담당자 관리
    /// </summary>
    public class HcOshaContractManagerRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public List<HC_OSHA_CONTRACT_MANAGER_MODEL> FindContractManager(long estimageId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HIC_OSHA_CONTRACT_MANAGER B   ");
            parameter.AppendSql("WHERE B.ISDELETED = 'N'                     ");
            parameter.AppendSql("AND B.ESTIMATE_ID = :ESTIMATE_ID            ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE                ");
            parameter.Add("ESTIMATE_ID", estimageId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_OSHA_CONTRACT_MANAGER_MODEL>(parameter);
        }
        public List<HC_OSHA_CONTRACT_MANAGER_MODEL> FindContractManagerByRole(long estimageId, string worker_role)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HIC_OSHA_CONTRACT_MANAGER B    ");
            parameter.AppendSql("WHERE B.ISDELETED = 'N'                      ");
            parameter.AppendSql("AND B.ESTIMATE_ID = :ESTIMATE_ID             ");
            parameter.AppendSql("AND B.WORKER_ROLE = :WORKER_ROLE             ");
            parameter.AppendSql("AND B.SWLICENSE = :SWLICENSE                 ");
            parameter.Add("ESTIMATE_ID", estimageId);
            parameter.Add("WORKER_ROLE", worker_role);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReader<HC_OSHA_CONTRACT_MANAGER_MODEL>(parameter);
        }
        public HC_OSHA_CONTRACT_MANAGER FindOne(long id)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT * FROM HIC_OSHA_CONTRACT_MANAGER WHERE ID = :ID ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteReaderSingle<HC_OSHA_CONTRACT_MANAGER>(parameter);
        }
        public HC_OSHA_CONTRACT_MANAGER Insert(HC_OSHA_CONTRACT_MANAGER dto)
        {
            MParameter parameter = CreateParameter();
            dto.ID = GetSequenceNextVal("HC_OSHA_SHARED_ID_SEQ");
            parameter.AppendSql("INSERT INTO HIC_OSHA_CONTRACT_MANAGER ");
            parameter.AppendSql("(ID,ESTIMATE_ID,WORKER_ROLE,NAME,DEPT,TEL,HP,EMAIL,ISDELETED,SWLICENSE) ");
            parameter.AppendSql("VALUES (:ID,:ESTIMATE_ID,:WORKER_ROLE,:NAME,:DEPT,:TEL,:HP,:EMAIL,");
            parameter.AppendSql("        'N',:SWLICENSE) ");
            parameter.Add("ID", dto.ID);
            parameter.Add("ESTIMATE_ID", dto.ESTIMATE_ID);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("DEPT", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("HP", dto.HP);
            parameter.Add("EMAIL", dto.EMAIL);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            DataSyncService.Instance.Insert("HIC_OSHA_CONTRACT_MANAGER", dto.ID);
            return FindOne(dto.ID);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_OSHA_CONTRACT_MANAGER  ");
            parameter.AppendSql("WHERE ID = :ID                         ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", id);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Delete("HIC_OSHA_CONTRACT_MANAGER", id);
        }

        public void Update(HC_OSHA_CONTRACT_MANAGER dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_OSHA_CONTRACT_MANAGER       ");
            parameter.AppendSql("SET    WORKER_ROLE = :WORKER_ROLE      ");
            parameter.AppendSql("      ,NAME = :NAME                    ");
            parameter.AppendSql("      ,DEPT = :DEPT                    ");
            parameter.AppendSql("      ,TEL = :TEL                      ");
            parameter.AppendSql("      ,HP = :HP                        ");
            parameter.AppendSql("      ,EMAIL = :EMAIL                  ");
            parameter.AppendSql("WHERE ID = :ID                         ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("ID", dto.ID);
            parameter.Add("WORKER_ROLE", dto.WORKER_ROLE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("DEPT", dto.DEPT);
            parameter.Add("TEL", dto.TEL);
            parameter.Add("HP", dto.HP);
            parameter.Add("EMAIL", dto.EMAIL);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);

            DataSyncService.Instance.Update("HIC_OSHA_CONTRACT_MANAGER", dto.ID);
        }
    }
}

