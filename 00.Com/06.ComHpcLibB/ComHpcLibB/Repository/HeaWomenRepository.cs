namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HeaWomenRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HeaWomenRepository()
        {
        }
        
        public HEA_WOMEN Read_Women_Reference(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,MIN_ONE,MAX_ONE,MIN_TWO,MAX_TWO   ");
            parameter.AppendSql("     , ROWID RID                               ");
            parameter.AppendSql("  FROM ADMIN.HEA_WOMEN                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReaderSingle<HEA_WOMEN>(parameter);
        }

        public int Merge_Women_Reference(string MIN_ONE, string MAX_ONE, string MIN_TWO, string MAX_TWO, long WRTNO, string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO ADMIN.HEA_WOMEN a     ");
            parameter.AppendSql("using dual d                           ");
            parameter.AppendSql("   on (a.WRTNO = :WRTNO)               ");
            parameter.AppendSql(" when matched then                     ");
            parameter.AppendSql("  update set                           ");
            if (CODE == "E908")
            {
                parameter.AppendSql("         MIN_ONE = :MIN_ONE        ");
                parameter.AppendSql("       , MAX_ONE = :MAX_ONE        ");
            }
            else if (CODE == "E909")
            {
                parameter.AppendSql("         MIN_TWO = :MIN_TWO        ");
                parameter.AppendSql("       , MAX_TWO = :MAX_TWO        ");
            }
            parameter.AppendSql("    when not matched then              ");
            parameter.AppendSql("  insert                               ");
            if (CODE == "E908")
            {
                parameter.AppendSql("        (MIN_ONE                   ");
                parameter.AppendSql("       , MAX_ONE                   ");
                parameter.AppendSql("       , WRTNO)                    ");
                parameter.AppendSql("  values                           ");
                parameter.AppendSql("        (:MIN_ONE                  ");
                parameter.AppendSql("       , :MAX_ONE                  ");
                parameter.AppendSql("       , :WRTNO)                   ");
            }
            else if (CODE == "E909")
            {
                parameter.AppendSql("        (MIN_TWO                   ");
                parameter.AppendSql("       , MAX_TWO                   ");
                parameter.AppendSql("       , WRTNO)                    ");
                parameter.AppendSql("  values                           ");
                parameter.AppendSql("        (:MIN_TWO                  ");
                parameter.AppendSql("       , :MAX_TWO                  ");
                parameter.AppendSql("       , :WRTNO)                   ");
            }

            if (CODE == "E908")
            {
                parameter.Add("MIN_ONE", MIN_ONE);
                parameter.Add("MAX_ONE", MAX_ONE);
            }
            else if (CODE == "E909")
            {
                parameter.Add("MIN_TWO", MIN_TWO);
                parameter.Add("MAX_TWO", MAX_TWO);
            }
            parameter.Add("WRTNO", WRTNO);

            return ExecuteNonQuery(parameter);
        }
    }
}
