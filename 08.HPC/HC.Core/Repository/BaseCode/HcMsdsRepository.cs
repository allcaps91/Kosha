namespace HC.Core.BaseCode.MSDS.Repository
{
    using System.Collections.Generic;
    using ComBase.Mvc;
    using HC.Core.BaseCode.MSDS.Dto;
    using HC.Core.Service;


    /// <summary>
    /// 
    /// </summary>
    public class HcMsdsRepository : BaseRepository
    {

        public List<HC_MSDS> FindByName(string name)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_MSDS ");
            parameter.AppendSql("WHERE NAME LIKE :NAME ");
            parameter.AppendSql("ORDER BY NAME");
            parameter.AddLikeStatement("NAME", name);
            return ExecuteReader<HC_MSDS>(parameter);
        }
        public List<HC_MSDS> FindLikeCasno(string casNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_MSDS ");
            parameter.AppendSql("WHERE CASNO LIKE :CASNO ");
            parameter.AppendSql("ORDER BY CASNO");
            parameter.AddLikeStatement("CASNO", casNo);
            return ExecuteReader<HC_MSDS>(parameter);
        }
        public HC_MSDS FindByChemid(string chemid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_MSDS ");
            parameter.AppendSql("WHERE CHEMID = :CHEMID ");
            parameter.AppendSql("ORDER BY CHEMID");
            parameter.Add("CHEMID", chemid);
            return ExecuteReaderSingle<HC_MSDS>(parameter);
        }

        public HC_MSDS FindByCasno(string casNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_MSDS ");
            parameter.AppendSql("WHERE CASNO = :CASNO ");
            parameter.AppendSql("ORDER BY CASNO");
            parameter.Add("CASNO", casNo);
            return ExecuteReaderSingle<HC_MSDS>(parameter);
        }

        public HC_MSDS FindById(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM HIC_MSDS                                                         ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.Add("ID", id);

            return ExecuteReaderSingle<HC_MSDS>(parameter);
        }

        public void Delete(long id)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("Delete FROM HIC_MSDS ");
            parameter.AppendSql("WHERE ID = :ID ");
            parameter.Add("ID", id);

            ExecuteNonQuery(parameter);
        }


        public HC_MSDS Save(HC_MSDS dto)
        {
            long id =  GetSequenceNextVal("HC_MSDS_ID_SEQ");
            dto.ID = id;

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_MSDS ( ");
            parameter.AppendSql("  ID,CHEMID,NAME,CASNO,EXPOSURE_MATERIAL,WEM_MATERIAL,SPECIALHEALTH_MATERIAL,");
            parameter.AppendSql("  MANAGETARGET_MATERIAL,SPECIALMANAGE_MATERIAL,STANDARD_MATERIAL,");
            parameter.AppendSql("  PERMISSION_MATERIAL,PSM_MATERIAL,GHS_PICTURE,MODIFIED,CREATED) ");
            parameter.AppendSql("VALUES ( ");
            parameter.AppendSql("  :ID,:CHEMID,:NAME,:CASNO,:EXPOSURE_MATERIAL,:WEM_MATERIAL,:SPECIALHEALTH_MATERIAL,");
            parameter.AppendSql("  :MANAGETARGET_MATERIAL,:SPECIALMANAGE_MATERIAL,:STANDARD_MATERIAL,");
            parameter.AppendSql("  :PERMISSION_MATERIAL,:PSM_MATERIAL,:GHS_PICTURE,SYSTIMESTAMP,SYSTIMESTAMP ) ");
            parameter.Add("ID", dto.ID);
            parameter.Add("CHEMID", dto.CHEMID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("CASNO", dto.CASNO);
            parameter.Add("EXPOSURE_MATERIAL", dto.EXPOSURE_MATERIAL);
            parameter.Add("WEM_MATERIAL", dto.WEM_MATERIAL);
            parameter.Add("SPECIALHEALTH_MATERIAL", dto.SPECIALHEALTH_MATERIAL);
            parameter.Add("MANAGETARGET_MATERIAL", dto.MANAGETARGET_MATERIAL);
            parameter.Add("SPECIALMANAGE_MATERIAL", dto.SPECIALMANAGE_MATERIAL);
            parameter.Add("STANDARD_MATERIAL", dto.STANDARD_MATERIAL);
            parameter.Add("PERMISSION_MATERIAL", dto.PERMISSION_MATERIAL);
            parameter.Add("PSM_MATERIAL", dto.PSM_MATERIAL);
            parameter.Add("GHS_PICTURE", dto.GHS_PICTURE);
            ExecuteNonQuery(parameter);

            return FindById(id);

        }

        public HC_MSDS Update(HC_MSDS dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_MSDS ");
            parameter.AppendSql("SET ");
            parameter.AppendSql("  ID = :ID, ");
            parameter.AppendSql("  CHEMID = :CHEMID, ");
            parameter.AppendSql("  NAME = :NAME, ");
            parameter.AppendSql("  CASNO = :CASNO, ");
            parameter.AppendSql("  EXPOSURE_MATERIAL = :EXPOSURE_MATERIAL, ");
            parameter.AppendSql("  WEM_MATERIAL = :WEM_MATERIAL, ");
            parameter.AppendSql("  SPECIALHEALTH_MATERIAL = :SPECIALHEALTH_MATERIAL, ");
            parameter.AppendSql("  MANAGETARGET_MATERIAL = :MANAGETARGET_MATERIAL, ");
            parameter.AppendSql("  SPECIALMANAGE_MATERIAL = :SPECIALMANAGE_MATERIAL, ");
            parameter.AppendSql("  STANDARD_MATERIAL = :STANDARD_MATERIAL, ");
            parameter.AppendSql("  PERMISSION_MATERIAL = :PERMISSION_MATERIAL, ");
            parameter.AppendSql("  PSM_MATERIAL = :PSM_MATERIAL, ");
            parameter.AppendSql("  GHS_PICTURE = :GHS_PICTURE, ");
            parameter.AppendSql("  MODIFIED = SYSTIMESTAMP ");
            parameter.AppendSql("  WHERE ID = :ID ");
            parameter.Add("ID", dto.ID);
            parameter.Add("CHEMID", dto.CHEMID);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("CASNO", dto.CASNO);
            parameter.Add("EXPOSURE_MATERIAL", dto.EXPOSURE_MATERIAL);
            parameter.Add("WEM_MATERIAL", dto.WEM_MATERIAL);
            parameter.Add("SPECIALHEALTH_MATERIAL", dto.SPECIALHEALTH_MATERIAL);
            parameter.Add("MANAGETARGET_MATERIAL", dto.MANAGETARGET_MATERIAL);
            parameter.Add("SPECIALMANAGE_MATERIAL", dto.SPECIALMANAGE_MATERIAL);
            parameter.Add("STANDARD_MATERIAL", dto.STANDARD_MATERIAL);
            parameter.Add("PERMISSION_MATERIAL", dto.PERMISSION_MATERIAL);
            parameter.Add("PSM_MATERIAL", dto.PSM_MATERIAL);
            parameter.Add("GHS_PICTURE", dto.GHS_PICTURE);
         
            ExecuteNonQuery(parameter);

            return FindById(dto.ID);

        }
    }
}
