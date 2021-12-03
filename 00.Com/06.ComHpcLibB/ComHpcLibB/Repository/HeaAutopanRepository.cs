namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using Oracle.ManagedDataAccess.Client;

    /// <summary>
    /// 
    /// </summary>
    public class HeaAutopanRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public HeaAutopanRepository()
        {
        }

        public List<HEA_AUTOPAN> GetItembyText(string argText)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, RANKING, TEXT, ROWID, GRPNO  ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN             ");
            parameter.AppendSql(" WHERE TEXT LIKE :TEXT                     ");
            parameter.AppendSql(" ORDER BY RANKING ASC, WRTNO ASC           ");

            parameter.AddLikeStatement("TEXT", argText);

            return ExecuteReader<HEA_AUTOPAN>(parameter);
        }

        public int Insert(HEA_AUTOPAN item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO ADMIN.HEA_AUTOPAN                       ");
            parameter.AppendSql("        (WRTNO, RANKING, WRITEDATE, WRITESABUN, TEXT)      ");
            parameter.AppendSql(" VALUES                                                    ");
            parameter.AppendSql("        (:WRTNO, :RANKING, SYSDATE, :WRITESABUN, :TEXT)    ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("RANKING", item.RANKING);
            parameter.Add("WRITESABUN", item.WRITESABUN);
            parameter.Add("TEXT", item.TEXT);

            return ExecuteNonQuery(parameter);
        }

        public List<HEA_AUTOPAN> GetWrtNo()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO                   ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN ");
            parameter.AppendSql(" ORDER BY RANKING ASC          ");

            return ExecuteReader<HEA_AUTOPAN>(parameter);
        }

        public Dictionary<string, object> Delete(long WRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.commandType = CommandType.StoredProcedure;
            parameter.AppendSql("ADMIN.PC_DELETE_HIC_AUTOPAN");

            parameter.AddProcIn("IN_WRTNO", WRTNO);            

            return ExecuteProc(parameter);
        }

        public int UpdateGrpNobyWrtNo(string strWrtNo, string strGrpNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE ADMIN.HEA_AUTOPAN SET    ");
            parameter.AppendSql("        GRPNO  = GRPNO                 ");
            parameter.AppendSql("  WHERE WRTNO  = :WRTNO                ");

            parameter.Add("GRPNO", strGrpNo);
            parameter.Add("WRTNO", strWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public string GetTextbyWrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TEXT                            ");
            parameter.AppendSql("  FROM ADMIN.HEA_AUTOPAN         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");

            parameter.AddLikeStatement("WRTNO", nWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int Update(HEA_AUTOPAN item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE ADMIN.HEA_AUTOPAN SET    ");
            parameter.AppendSql("        WRITEDATE  = SYSDATE           ");
            parameter.AppendSql("      , WRITESABUN = :WRITESABUN       ");
            parameter.AppendSql("      , TEXT       = :TEXT             ");
            parameter.AppendSql("  WHERE WRTNO      = :WRTNO            ");

            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("WRITESABUN", item.WRITESABUN);
            parameter.Add("TEXT", item.TEXT);

            return ExecuteNonQuery(parameter);
        }

        public string GetSeqHeaAutoPan()
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SEQ_HEAAUTOPAN.NEXTVAL WRTNO FROM DUAL  ");

            return ExecuteScalar<string>(parameter);
        }
    }
}
