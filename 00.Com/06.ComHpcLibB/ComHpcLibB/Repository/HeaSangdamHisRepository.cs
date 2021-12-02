namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaSangdamHisRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaSangdamHisRepository()
        {
        }

        public int InsertSangdamHis(HEA_SANGDAM_HIS item)
        {
            MParameter parameter = CreateParameter();
            if (item.ENTGUBUN.IsNullOrEmpty())
            {
                parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SANGDAM_HIS                                            ");
                parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO)                    ");
                parameter.AppendSql("VALUES                                                                             ");
                parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO)             ");
            }
            else
            {
                parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SANGDAM_HIS                                            ");
                parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO, ENTGUBUN)          ");
                parameter.AppendSql("VALUES                                                                             ");
                parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO, :ENTGUBUN)  ");
            }
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("WAITNO", item.WAITNO);
            if (!item.ENTGUBUN.IsNullOrEmpty())
            {
                parameter.Add("ENTGUBUN", item.ENTGUBUN);
            }

            return ExecuteNonQuery(parameter);
        }

        public int InsertSangdam(HEA_SANGDAM_HIS item)
        {
            MParameter parameter = CreateParameter();
            
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_SANGDAM_WAIT                                           ");
            parameter.AppendSql("       (WRTNO, SNAME, SEX, AGE, GJJONG, GUBUN, ENTTIME, WAITNO)                    ");
            parameter.AppendSql("VALUES                                                                             ");
            parameter.AppendSql("       (:WRTNO, :SNAME, :SEX, :AGE, :GJJONG, :GUBUN, SYSDATE, :WAITNO)             ");            

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("GJJONG", item.GJJONG);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("WAITNO", item.WAITNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
