
namespace ComHpcLibB.Repository
{

    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicJinWordRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicJinWordRepository()
        {

        }


        public HIC_JIN_WORD GetItemByGubunJdate(string argGUBUN, string argJdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT WORD1, WORD2                               ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_JIN_WORD                     ");
            parameter.AppendSql(" WHERE JDATE   <= TO_DATE(:JDATE,'YYYY-MM-DD')     ");
            parameter.AppendSql(" AND GUBUN = :GUBUN                                ");
            parameter.AppendSql(" ORDER BY JDATE DESC                               ");


            parameter.Add("GUBUN", argGUBUN);
            parameter.Add("JDATE", argJdate);

            return ExecuteReaderSingle<HIC_JIN_WORD>(parameter);
        }


    }
}
