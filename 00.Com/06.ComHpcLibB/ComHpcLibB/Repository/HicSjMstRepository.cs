namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicSjMstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicSjMstRepository()
        {
        }

        public HIC_SJ_MST GetItembyGjYearLtdCode(string strGjYear, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(JepDate,'YYYY-MM-DD') JepDate,GbNewLtd,GbTel,Inwon1,Inwon2,Inwon3           ");
            parameter.AppendSql("     , JepName,DamName,JosaRemark,TO_CHAR(RFDate,'YYYY-MM-DD') RFDate                      ");
            parameter.AppendSql("     , TO_CHAR(RTDate,'YYYY-MM-DD') RTDate,Place1,Place2,Place3                            ");
            parameter.AppendSql("     , TO_CHAR(MDate,'YYYY-MM-DD') MDate,GbMail1,GbMail2,GbJunbi1,GbJunbi2,GbJunbi3        ");
            parameter.AppendSql("     , GbJunbi4,OldHospital                                                                ");
            parameter.AppendSql("     , TO_CHAR(OldFDate,'YYYY-MM-DD') OldFDate,TO_CHAR(OldTDate,'YYYY-MM-DD') OldTDate     ");
            parameter.AppendSql("     , OldInwon1,OldInwon2,OldInwon3,ChukHospital                                          ");
            parameter.AppendSql("     , TO_CHAR(ChukDate1,'YYYY-MM-DD') ChukDate1,TO_CHAR(ChukDate2,'YYYY-MM-DD') ChukDate2 ");
            parameter.AppendSql("     , ChukNochul,ChukChogwa,ChukYuhe,Remark1,Remark2,ROWID as RID                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_MST                                                              ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR                                                                   ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                                                                  ");

            parameter.Add("GJYEAR", strGjYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", strCode);

            return ExecuteReaderSingle<HIC_SJ_MST>(parameter);
        }

        public int GetCountbyGjYearLtdCode(string strYear, long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SJ_MST                  ");
            parameter.AppendSql(" WHERE GJYEAR  = :GJYEAR                       ");
            parameter.AppendSql("   AND LTDCODE = :LTDCODE                      ");

            parameter.Add("GJYEAR", strYear, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", nLtdCode);

            return ExecuteScalar<int>(parameter);
        }

        public int Update(HIC_SJ_MST item, string strOut)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SJ_MST SET                                  ");
            parameter.AppendSql("       JEPDATE       = TO_DATE(:JEPDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("     , GBNEWLTD      = :GBNEWLTD                                   ");                    
            parameter.AppendSql("     , GBTEL         = :GBTEL                                      ");
            parameter.AppendSql("     , INWON1        = :INWON1                                     ");
            parameter.AppendSql("     , INWON2        = :INWON2                                     ");
            parameter.AppendSql("     , INWON3        = :INWON3                                     ");
            parameter.AppendSql("     , JEPNAME       = :JEPNAME                                    ");
            parameter.AppendSql("     , DAMNAME       = :DAMNAME                                    ");
            parameter.AppendSql("     , JOSAREMARK    = :JOSAREMARK                                 ");
            parameter.AppendSql("     , RFDATE        = TO_DATE(:RFDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("     , RTDATE        = TO_DATE(:RTDATE, 'YYYY-MM-DD')              ");
            parameter.AppendSql("     , PLACE1        = :PLACE1                                     ");
            parameter.AppendSql("     , PLACE2        = :PLACE2                                     ");
            if (strOut == "Y")
            {
                parameter.AppendSql("     , PLACE3        = :PLACE3                                 ");
            }
            else
            {
                parameter.AppendSql("     , PLACE3        = ''                                      ");
            }
            parameter.AppendSql("     , MDATE        = TO_DATE(:MDATE, 'YYYY-MM-DD')                ");
            parameter.AppendSql("     , GBMAIL1      = :GBMAIL1                                     ");
            parameter.AppendSql("     , GBMAIL2      = :GBMAIL2                                     ");
            parameter.AppendSql("     , GBJUNBI1     = :GBJUNBI1                                    ");
            parameter.AppendSql("     , GBJUNBI2     = :GBJUNBI2                                    ");
            parameter.AppendSql("     , GBJUNBI3     = :GBJUNBI3                                    ");
            parameter.AppendSql("     , GBJUNBI4     = :GBJUNBI4                                    ");
            parameter.AppendSql("     , OLDHOSPITAL  = :OLDHOSPITAL                                 ");
            parameter.AppendSql("     , OLDFDATE     = TO_DATE(:OLDFDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("     , OLDTDATE     = TO_DATE(:OLDTDATE, 'YYYY-MM-DD')             ");
            parameter.AppendSql("     , OLDINWON1    = :OLDINWON1                                   ");
            parameter.AppendSql("     , OLDINWON2    = :OLDINWON2                                   ");
            parameter.AppendSql("     , OLDINWON3    = :OLDINWON3                                   ");
            parameter.AppendSql("     , CHUKHOSPITAL = :CHUKHOSPITAL                                ");
            parameter.AppendSql("     , CHUKDATE1    = TO_DATE(:CHUKDATE1, 'YYYY-MM-DD')            ");
            parameter.AppendSql("     , CHUKDATE2    = TO_DATE(:CHUKDATE2, 'YYYY-MM-DD')            ");
            parameter.AppendSql("     , CHUKNOCHUL   = :CHUKNOCHUL                                  ");
            parameter.AppendSql("     , CHUKCHOGWA   = :CHUKCHOGWA                                  ");
            parameter.AppendSql("     , CHUKYUHE     = :CHUKYUHE                                    ");
            parameter.AppendSql("     , REMARK1      = :REMARK1                                     ");
            parameter.AppendSql("     , REMARK2      = :REMARK2                                     ");
            parameter.AppendSql("     , ENTSABUN     = :ENTSABUN                                    ");
            parameter.AppendSql("     , ENTTIME      = SYSDATE                                      ");
            parameter.AppendSql(" WHERE GJYEAR       = :GJYEAR                                      ");
            parameter.AppendSql("   AND LTDCODE      = :LTDCODE                                     ");

            parameter.Add("GJYEAR", item.GJYEAR, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("JEPDATE", item.JEPDATE);
            parameter.Add("INWON1", item.INWON1);
            parameter.Add("INWON2", item.INWON2);
            parameter.Add("INWON3", item.INWON3);
            parameter.Add("JEPNAME", item.JEPNAME);
            parameter.Add("DAMNAME", item.DAMNAME);
            parameter.Add("JOSAREMARK", item.JOSAREMARK);
            parameter.Add("RFDATE", item.RFDATE);
            parameter.Add("RTDATE", item.RTDATE);
            parameter.Add("MDATE", item.MDATE);
            parameter.Add("OLDFDATE", item.OLDFDATE);
            parameter.Add("OLDTDATE", item.OLDTDATE);
            parameter.Add("OLDINWON1", item.OLDINWON1);
            parameter.Add("OLDINWON2", item.OLDINWON2);
            parameter.Add("OLDINWON3", item.OLDINWON3);
            parameter.Add("CHUKDATE1", item.CHUKDATE1);
            parameter.Add("CHUKDATE2", item.CHUKDATE2);
            parameter.Add("CHUKNOCHUL", item.CHUKNOCHUL);
            parameter.Add("CHUKCHOGWA", item.CHUKCHOGWA);
            parameter.Add("CHUKYUHE", item.CHUKYUHE); 
            parameter.Add("REMARK1", item.REMARK1);
            parameter.Add("REMARK2", item.REMARK2);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("GBNEWLTD", item.GBNEWLTD, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBTEL", item.GBTEL, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("PLACE1", item.PLACE1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("PLACE2", item.PLACE2, Oracle.DataAccess.Client.OracleDbType.Char);
            if (strOut == "Y")
            {
                parameter.Add("PLACE3", item.PLACE3, Oracle.DataAccess.Client.OracleDbType.Char);
            }
            parameter.Add("GBMAIL1", item.GBMAIL1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBMAIL2", item.GBMAIL2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBJUNBI1", item.GBJUNBI1, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBJUNBI2", item.GBJUNBI2, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBJUNBI3", item.GBJUNBI3, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("GBJUNBI4", item.GBJUNBI4, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("OLDHOSPITAL", item.OLDHOSPITAL);
            parameter.Add("CHUKHOSPITAL", item.CHUKHOSPITAL);

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_SJ_MST item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SJ_MST                         ");
            parameter.AppendSql("       (GJYEAR, LTDCODE, JEPDATE)                          ");
            parameter.AppendSql("VALUES                                                     ");
            parameter.AppendSql("       (:GJYEAR, :LTDCODE, TO_DATE(:JEPDATE, 'YYYY-MM-DD'))");

            parameter.Add("GJYEAR", item.GJYEAR, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("LTDCODE", item.LTDCODE);
            parameter.Add("JEPDATE", item.JEPDATE);

            return ExecuteNonQuery(parameter);
        }
    }
}
