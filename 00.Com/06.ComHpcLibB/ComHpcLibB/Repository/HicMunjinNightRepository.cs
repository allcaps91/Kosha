namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicMunjinNightRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMunjinNightRepository()
        {
        }

        public string GetCountMunjinbyIemunNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_MUNJIN_NIGHT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(HIC_MUNJIN_NIGHT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_MUNJIN_NIGHT                                       ");
            parameter.AppendSql("        (WRTNO, ITEM1_DATA, ITEM1_JEMSU, ITEM1_PANJENG, EntDate, ENTSABUN)     ");
            parameter.AppendSql(" VALUES                                                                        ");
            parameter.AppendSql("        (:WRTNO, :ITEM1_DATA, :ITEM1_JEMSU, :ITEM1_PANJENG, SYSDATE, :ENTSABUN)");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("ITEM1_DATA", item.ITEM1_DATA);
            parameter.Add("ITEM1_JEMSU", item.ITEM1_JEMSU);
            parameter.Add("ITEM1_PANJENG", item.ITEM1_PANJENG);
            parameter.Add("ENTSABUN", item.ENTSABUN);

            return ExecuteNonQuery(parameter);
        }

        public HIC_MUNJIN_NIGHT GetItembyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, ITEM1_DATA, ITEM1_JEMSU, ITEM1_PANJENG   ");
            parameter.AppendSql("     , ITEM2_DATA, ITEM2_JEMSU, ITEM2_PANJENG          ");
            parameter.AppendSql("     , ITEM3_DATA, ITEM3_JEMSU, ITEM3_PANJENG          ");
            parameter.AppendSql("     , ITEM4_DATA, ITEM4_JEMSU, ITEM4_PANJENG          ");
            parameter.AppendSql("     , ITEM5_DATA, ITEM5_JEMSU, ITEM5_PANJENG          ");
            parameter.AppendSql("     , ENTDATE, ENTSABUN                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_MUNJIN_NIGHT                    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                  ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_MUNJIN_NIGHT>(parameter);
        }

        public int SavebyWrtNo(HIC_MUNJIN_NIGHT item, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("MERGE INTO ADMIN.HIC_MUNJIN_NIGHT a                                  ");
            parameter.AppendSql("using dual d                                                               ");
            parameter.AppendSql("   on (a.WRTNO     = :WRTNO)                                               ");
            parameter.AppendSql(" when matched then                                                         ");
            parameter.AppendSql("  update set                                                               ");
            if (strGubun == "0")
            {
                parameter.AppendSql("          ITEM1_DATA    = :ITEM1_DATA                                  ");
                parameter.AppendSql("        , ITEM1_JEMSU   = :ITEM1_JEMSU                                 ");
                parameter.AppendSql("        , ITEM1_PANJENG = :ITEM1_PANJENG                               ");
                parameter.AppendSql("        , ITEM4_DATA    = :ITEM4_DATA                                  ");
                parameter.AppendSql("        , ITEM5_DATA    = :ITEM5_DATA                                  ");
            }
            else if (strGubun == "1")
            {
                parameter.AppendSql("          ITEM2_DATA    = :ITEM2_DATA                                  ");
                parameter.AppendSql("        , ITEM2_JEMSU   = :ITEM2_JEMSU                                 ");
                parameter.AppendSql("        , ITEM2_PANJENG = :ITEM2_PANJENG                               ");
                parameter.AppendSql("        , ITEM3_DATA    = :ITEM3_DATA                                  ");
                parameter.AppendSql("        , ITEM3_JEMSU   = :ITEM3_JEMSU                                 ");
                parameter.AppendSql("        , ITEM3_PANJENG = :ITEM3_PANJENG                               ");
            }
            parameter.AppendSql("        , EntDate   = SYSDATE                                              ");
            parameter.AppendSql("        , ENTSABUN  = :ENTSABUN                                            ");
            parameter.AppendSql("    when not matched then                                                  ");
            parameter.AppendSql("  INSERT                                                                   ");
            parameter.AppendSql("         (WRTNO, ITEM1_DATA, ITEM1_JEMSU, ITEM1_PANJENG                    ");
            parameter.AppendSql("       , ITEM4_DATA, ITEM5_DATA                                            ");
            parameter.AppendSql("       , ITEM2_DATA,ITEM2_JEMSU,ITEM2_PANJENG                              ");
            parameter.AppendSql("       , ITEM3_DATA,ITEM3_JEMSU,ITEM3_PANJENG,ENTDATE,ENTSABUN )           ");

            parameter.AppendSql(" VALUES                                                                    ");
            parameter.AppendSql("         (:WRTNO, :ITEM1_DATA, :ITEM1_JEMSU, :ITEM1_PANJENG                ");
            parameter.AppendSql("       , :ITEM4_DATA, :ITEM5_DATA                                         ");
            parameter.AppendSql("       , :ITEM2_DATA, :ITEM2_JEMSU, :ITEM2_PANJENG                         ");
            parameter.AppendSql("       , :ITEM3_DATA, :ITEM3_JEMSU, :ITEM3_PANJENG, SYSDATE, :ENTSABUN )   ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("ITEM1_DATA", item.ITEM1_DATA);
            parameter.Add("ITEM1_JEMSU", item.ITEM1_JEMSU);
            parameter.Add("ITEM1_PANJENG", item.ITEM1_PANJENG);
            parameter.Add("ITEM2_DATA", item.ITEM2_DATA);
            parameter.Add("ITEM2_JEMSU", item.ITEM2_JEMSU);
            parameter.Add("ITEM2_PANJENG", item.ITEM2_PANJENG);
            parameter.Add("ITEM3_DATA", item.ITEM3_DATA);
            parameter.Add("ITEM3_JEMSU", item.ITEM3_JEMSU);
            parameter.Add("ITEM3_PANJENG", item.ITEM3_PANJENG);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("ITEM4_DATA", item.ITEM4_DATA);
            parameter.Add("ITEM5_DATA", item.ITEM5_DATA);

            return ExecuteNonQuery(parameter);
        }

        public HIC_MUNJIN_NIGHT GetAllbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO   ");
            parameter.AppendSql("     , ITEM1_DATA, ITEM1_JEMSU, ITEM1_PANJENG  ");
            parameter.AppendSql("     , ITEM2_DATA, ITEM2_JEMSU, ITEM2_PANJENG  ");
            parameter.AppendSql("     , ITEM3_DATA, ITEM3_JEMSU, ITEM3_PANJENG  ");
            parameter.AppendSql("     , ITEM4_DATA, ITEM4_JEMSU, ITEM4_PANJENG  ");
            parameter.AppendSql("     , ITEM5_DATA, ITEM5_JEMSU, ITEM5_PANJENG  ");
            parameter.AppendSql("     , ENTDATE, ENTSABUN               ");
            parameter.AppendSql("  FROM ADMIN.HIC_MUNJIN_NIGHT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteReaderSingle<HIC_MUNJIN_NIGHT>(parameter);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_MUNJIN_NIGHT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }


        public int UpDate(HIC_MUNJIN_NIGHT Item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE ADMIN.HIC_MUNJIN_NIGHT               ");
            parameter.AppendSql("    SET ITEM4_PANJENG = :PANJENG1                  ");
            parameter.AppendSql("       ,ITEM5_PANJENG = :PANJENG2                  ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND WRTNO    = :WRTNO                           ");

            #region Query 변수대입
            parameter.Add("WRTNO", Item.WRTNO);
            parameter.Add("PANJENG1", Item.ITEM4_PANJENG);
            parameter.Add("PANJENG2", Item.ITEM5_PANJENG);

            #endregion

            return ExecuteNonQuery(parameter);
        }

        public void DeleteByWrtno(long argWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_MUNJIN_NIGHT    ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.Add("WRTNO", argWrtno);

            ExecuteNonQuery(parameter);
        }
  
    }
}
