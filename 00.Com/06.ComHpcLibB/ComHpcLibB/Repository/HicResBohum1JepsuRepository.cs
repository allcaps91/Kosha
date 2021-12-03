namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicResBohum1JepsuRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicResBohum1JepsuRepository()
        {
        }

        public HIC_RES_BOHUM1_JEPSU GetItembyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SMOKING1,Habit1,DRINK1,T_STAT21,T_STAT22,T_STAT31,T_STAT32,T_STAT41,T_STAT42,TMUN0047       ");
            parameter.AppendSql("     , TMUN0050,TMUN0053,TMUN0058, T_ACTIVE1,T_ACTIVE2,T_ACTIVE3,PANJENGU1,PANJENGU2               ");
            parameter.AppendSql("     , PANJENGU3,PANJENGU4, TMUN0001,TMUN0002, TMUN0003,TMUN0004,TMUN0005,TMUN0006,TMUN0007        ");
            parameter.AppendSql("     , TMUN0008,TMUN0009, TMUN0010, TMUN0011,TMUN0012, TMUN0103, B.AGE                             "); 
            parameter.AppendSql("     , A.ROWID,HABIT1,HABIT2,HABIT3,HABIT4,HABIT5,T_SMOKE1,T_DRINK1                                ");
            parameter.AppendSql("     , A.OLDBYENG2, A.OLDBYENG5, A.PanjengU1, A.PanjengU2, A.PanjengU3, A.PanjengU4                ");
            parameter.AppendSql("     , A.TMUN0125, A.TMUN0126, A.TMUN0127, A.TMUN0128                                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_RES_BOHUM1 A, ADMIN.HIC_JEPSU B                                       ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                                            ");
            parameter.AppendSql("   AND A.WRTNO = B.WRTNO                                                                           ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_RES_BOHUM1_JEPSU>(parameter);
        }
    }
}
