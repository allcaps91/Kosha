namespace HC_Measurement.Repository
{
    using ComBase.Mvc;
    using HC_Measurement.Dto;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicChkUcodeRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChkUcodeRepository()
        {
        }

        public List<HIC_CHK_UCODE> GetItemAll()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT CODE,NAME,REMARK,SORT,GBUSE,ENTSABUN,ENTDATE,ROWID AS RID  ");
            parameter.AppendSql("  FROM ADMIN.HIC_CHK_UCODE                                   ");
            parameter.AppendSql(" WHERE 1 = 1                                                 ");
            parameter.AppendSql(" ORDER BY SORT                                               ");

            return ExecuteReader<HIC_CHK_UCODE>(parameter);
        }

        public void Insert(HIC_CHK_UCODE dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHK_UCODE");
            parameter.AppendSql("(");
            parameter.AppendSql("    CODE");
            parameter.AppendSql("  , NAME");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , SORT");
            parameter.AppendSql("  , GBUSE");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :CODE");
            parameter.AppendSql("  , :NAME");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :SORT");
            parameter.AppendSql("  , :GBUSE");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql("  , :ENTDATE");
            parameter.AppendSql(") ");

            parameter.Add("CODE", dto.CODE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SORT", dto.SORT);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("ENTDATE", dto.ENTDATE);

            ExecuteNonQuery(parameter);
        }

        public void UpdatebyRowId(HIC_CHK_UCODE dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_CHK_UCODE");
            parameter.AppendSql("   SET CODE = :CODE");
            parameter.AppendSql("     , NAME = :NAME");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql("     , SORT = :SORT");
            parameter.AppendSql("     , GBUSE = :GBUSE");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql("     , ENTDATE = :ENTDATE");
            parameter.AppendSql("WHERE ROWID = :RID");

            parameter.Add("CODE", dto.CODE);
            parameter.Add("NAME", dto.NAME);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("SORT", dto.SORT);
            parameter.Add("GBUSE", dto.GBUSE);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("ENTDATE", dto.ENTDATE);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
