namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HeaDentalRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaDentalRepository()
        {
        }

        public int Update(HEA_DENTAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_DENTAL      ");
            parameter.AppendSql("   SET GBSTS       = :GBSTS        ");
            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO  ");
            parameter.AppendSql("     , PANJENGDATE = SYSDATE       ");
            parameter.AppendSql("     , USIK        = :USIK         ");
            parameter.AppendSql("     , GYEOLSON    = :GYEOLSON     ");
            parameter.AppendSql("     , CHIEUN      = :CHIEUN       ");
            parameter.AppendSql("     , CHIJU       = :CHIJU        ");
            parameter.AppendSql("     , CHIGUNMAK   = :CHIGUNMAK    ");
            parameter.AppendSql("     , DENTURE     = :DENTURE      ");
            parameter.AppendSql("     , DENTURE_ETC = :DENTURE_ETC  ");
            parameter.AppendSql("     , PANJENG1    = :PANJENG1     ");
            parameter.AppendSql("     , PANJENG2    = :PANJENG2     ");
            parameter.AppendSql("     , PANJENG3    = :PANJENG3     ");
            parameter.AppendSql("     , PANJENG4    = :PANJENG4     ");
            parameter.AppendSql("     , PANJENG5    = :PANJENG5     ");
            parameter.AppendSql("     , PANJENG6    = :PANJENG6     ");
            parameter.AppendSql("     , PANJENG7    = :PANJENG7     ");
            parameter.AppendSql("     , PANJENG8    = :PANJENG8     ");
            parameter.AppendSql("     , PANJENG9    = :PANJENG9     ");
            parameter.AppendSql("     , PANJENG11   = :PANJENG11    ");
            parameter.AppendSql("     , PANJENG12   = :PANJENG12    ");
            parameter.AppendSql("     , PANJENG13   = :PANJENG13    ");
            parameter.AppendSql("     , PANJENG14   = :PANJENG14    ");
            parameter.AppendSql("     , PANJENG15   = :PANJENG15    ");
            parameter.AppendSql("     , PANJENG10   = :PANJENG10    ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO        ");

            parameter.Add("GBSTS",       item.GBSTS, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("USIK",        item.USIK);
            parameter.Add("GYEOLSON",    item.GYEOLSON);
            parameter.Add("CHIEUN",      item.CHIEUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CHIJU",       item.CHIJU, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CHIGUNMAK",   item.CHIGUNMAK, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DENTURE",     item.DENTURE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("DENTURE_ETC", item.DENTURE_ETC);
            parameter.Add("PANJENG1",    item.PANJENG1, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG2",    item.PANJENG2, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG3",    item.PANJENG3, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG4",    item.PANJENG4, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG5",    item.PANJENG5, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG6",    item.PANJENG6, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG7",    item.PANJENG7, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG8",    item.PANJENG8, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG9",    item.PANJENG9, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("PANJENG11",   item.PANJENG11, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG12",   item.PANJENG12, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG13",   item.PANJENG13, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG14",   item.PANJENG14, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG15",   item.PANJENG15, Oracle.ManagedDataAccess.Client.OracleDbType.Char);   
            parameter.Add("PANJENG10",   item.PANJENG10);
            parameter.Add("WRTNO",       item.WRTNO); 

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM ADMIN.HEA_DENTAL                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");
            parameter.AppendSql("   AND GBCHK = 'Y'                                 ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<int>(parameter);
        }

        public HEA_DENTAL GetItemAllbyWrtNo(long fnWRTNO, string strLicence)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO,GBSTS,JEPDATE,PANJENGDRNO,PANJENGDATE,ENTTIME,USIK,GYEOLSON               ");
            parameter.AppendSql("     , CHIEUN,CHIJU,CHIGUNMAK,DENTURE,DENTURE_ETC,PANJENG1,PANJENG2,PANJENG3,PANJENG4  ");
            parameter.AppendSql("     , PANJENG5,PANJENG6,PANJENG7,PANJENG8,PANJENG9,PANJENG10,PANJENG11,PANJENG12      ");
            parameter.AppendSql("     , PANJENG13,PANJENG14,PANJENG15                                                   ");
            parameter.AppendSql("     , ADMIN.FC_HIC_DOCTOR_NAME(:LICENCE) DRNAME                                 ");
            parameter.AppendSql("  FROM ADMIN.HEA_DENTAL                                                          ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                  ");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("LICENCE", strLicence); 

            return ExecuteReaderSingle<HEA_DENTAL>(parameter);
        }

        public int Insert(HEA_DENTAL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HEA_DENTAL                         ");
            parameter.AppendSql("       (WRTNO, JEPDATE, ENTTIME)                           ");
            parameter.AppendSql("VALUES PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql("       (:WRTNO, TO_DATE(:JEPDATE, 'YYYY-MM-DD'), SYSDATE)  ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("JEPDATE", item.JEPDATE);
            return ExecuteNonQuery(parameter);
        }

        public string GetRowIdbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                                       ");
            parameter.AppendSql("  FROM ADMIN.HEA_DENTAL                      ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                              ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<string>(parameter);
        }

        public int GbchkUpdate(long nWrtNo, string strGbchk)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HEA_DENTAL      ");
            parameter.AppendSql("   SET GBCHK = :GBCHK              ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO              ");

            
            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("GBCHK", strGbchk, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
