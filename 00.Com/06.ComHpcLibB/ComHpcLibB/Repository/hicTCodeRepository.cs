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
    public class HicTCodeRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicTCodeRepository()
        {
        }

        public List<HIC_TCODE> GetItembyGubun(int argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, NAME, JUMSU                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_TCODE               ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql(" ORDER BY Code                             ");

            parameter.Add("GUBUN", argGubun);

            return ExecuteReader<HIC_TCODE>(parameter);
        }
    }
}
