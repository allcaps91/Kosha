using System;
using System.Collections.Generic;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;


namespace ComHpcLibB.Repository
{
    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicHemsCodeRepository : BaseRepository
    {
        public List<HIC_HEMS_CODE> GetItembyGubunName(string strJong, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT GUBUN, CODE1, CODE2, CODE3, CODE4, NAME        ");
            parameter.AppendSql("   FROM ADMIN.HIC_HEMS_CODE                      ");
            parameter.AppendSql("  WHERE GUBUN = :GUBUN                                 ");
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql("    AND NAME LIKE :NAME                            ");
            }
            parameter.AppendSql(" ORDER BY CODE1                                        ");

            parameter.Add("GUBUN", strJong);
            if (!strName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("NAME", strName);
            }

            return ExecuteReader<HIC_HEMS_CODE>(parameter);
        }

        public int Update(HIC_HEMS_CODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_HEMS_CODE                  ");
            parameter.AppendSql("       (GUBUN, CODE1, CODE2, CODE3, CODE4, NAME)       ");
            parameter.AppendSql(" VALUES                                                ");
            parameter.AppendSql("       (:GUBUN, :CODE1, :CODE2, :CODE3, :CODE4, :NAME) ");

            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("CODE1", item.CODE1);
            parameter.Add("CODE2", item.CODE2);
            parameter.Add("CODE3", item.CODE3);
            parameter.Add("CODE4", item.CODE4);
            parameter.Add("NAME", item.NAME);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_HEMS_CODE> GetCode2byGubunCode1(string strGubun, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT Code2                                  ");
            parameter.AppendSql("   FROM ADMIN.HIC_HEMS_CODE              ");
            parameter.AppendSql("  WHERE GUBUN = :GUBUN                         "); //HEMS 약속 코드
            parameter.AppendSql("    AND CODE1 = :CODE1                         ");
            parameter.AppendSql("  ORDER BY CODE2                               ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE1", argCode);

            return ExecuteReader<HIC_HEMS_CODE>(parameter);
        }

        public int Delete(HIC_HEMS_CODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_HEMS_CODE    ");
            parameter.AppendSql(" WHERE ROWID = :RID                 ");

            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_HEMS_CODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_HEMS_CODE SET   ");
            parameter.AppendSql("       CODE1 = :CODE1                  ");
            parameter.AppendSql("     , CODE2 = :CODE2                  ");
            parameter.AppendSql("     , CODE3 = :CODE3                  ");
            parameter.AppendSql("     , CODE4 = :CODE4                  ");
            parameter.AppendSql("     , NAME  = :NAME                   ");
            parameter.AppendSql(" WHERE ROWID = :RID                    ");

            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }
    }
}
