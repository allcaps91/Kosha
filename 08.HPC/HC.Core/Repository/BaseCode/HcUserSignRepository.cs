namespace HC.Core.Repository
{
    using ComBase.Mvc;
    using HC.Core.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HcUserSignRepository : BaseRepository
    {
        public HIC_USERSIGN FindOne(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT * FROM  HIC_USERSIGN    ");
            parameter.AppendSql("WHERE USERID =  :USERID    ");

            parameter.Add("USERID", userId);
            return ExecuteReaderSingle<HIC_USERSIGN>(parameter);
        }

        public void Insert(HIC_USERSIGN dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_USERSIGN    ");
            parameter.AppendSql("(USERID, IMAGEBASE64)     ");
            parameter.AppendSql("VALUES(:USERID, :IMAGEBASE64 )     ");

            parameter.Add("USERID", dto.USERID);
            parameter.Add("IMAGEBASE64", dto.IMAGEBASE64);
            ExecuteNonQuery(parameter);
        }
        public void Update(HIC_USERSIGN dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE HIC_USERSIGN   ");
            parameter.AppendSql("SET IMAGEBASE64 = :IMAGEBASE64 ");
            parameter.AppendSql("WHERE USERID = :USERID    ");
            parameter.Add("USERID", dto.USERID);
            parameter.Add("IMAGEBASE64", dto.IMAGEBASE64);
            ExecuteNonQuery(parameter);
        }

        public void Delete(string userId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("DELETE FROM HIC_USERSIGN    ");
            parameter.AppendSql("WHERE USERID = :USERID     ");

            parameter.Add("USERID", userId);
            ExecuteNonQuery(parameter);
        }
    }
}
