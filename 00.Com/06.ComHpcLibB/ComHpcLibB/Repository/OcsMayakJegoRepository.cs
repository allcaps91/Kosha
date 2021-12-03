namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class OcsMayakJegoRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public OcsMayakJegoRepository()
        {
        }

        public List<OCS_MAYAK_JEGO> GetListByDate(string argDate, string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.SUCODE, B.JEPNAME, B.COVUNIT, C.UNIT, C.HNAME,  A.QTY strQTY , A.ROWID AS RID  ");
            parameter.AppendSql("  FROM ADMIN.OCS_DRUG         A                                                               ");
            parameter.AppendSql("     , ADMIN.DRUG_JEP         B                                                               ");
            parameter.AppendSql("     , ADMIN.OCS_DRUGINFO_NEW C                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                       ");
            parameter.AppendSql("   AND A.BUILDDATE = TO_DATE(:BUILDATE, 'YYYY-MM-DD')                                              ");
            parameter.AppendSql("   AND A.GBN2 IN ( '3')                                                                            "); // 재고
            parameter.AppendSql("   AND A.WARDCODE ='TO'                                                                            ");  // 종검
            parameter.AppendSql("   AND A.SUCODE = B.JEPCODE(+)                                                                     ");
            parameter.AppendSql("   AND A.SUCODE = C.SUNEXT(+)                                                                      ");

            if (!argGbn.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.GBN =:GBN                                                                             ");
            }
            
            parameter.Add("BUILDATE", argDate);

            if (!argGbn.IsNullOrEmpty())
            { 
                parameter.Add("GBN", argGbn, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<OCS_MAYAK_JEGO>(parameter);
        }

        public int InsertOcsDrugSet(OCS_MAYAK_JEGO item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO ADMIN.OCS_DRUG_SET (                                                         ");
            parameter.AppendSql("        JDATE, GBN, WARDCODE, SUCODE, BQTY , QTY , ENTDATE, UNIT                               ");
            parameter.AppendSql(" ) VALUES (                                                                                    ");
            parameter.AppendSql("        TO_DATE(:JDATE,'YYYY-MM-DD'), :GBN, :WARDCODE, :SUCODE, :BQTY , :QTY , SYSDATE, :UNIT )");

            parameter.Add("JDATE", item.JDATE);
            parameter.Add("GBN", item.GBN);
            parameter.Add("WARDCODE", item.WARDCODE);
            parameter.Add("SUCODE", item.SUCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BQTY", item.BQTY);
            parameter.Add("QTY", item.QTY);
            parameter.Add("UNIT", item.UNIT);
            
            return ExecuteNonQuery(parameter);
        }

        public int UpdateOcsDrugSetQtyByRowid(string argQty, string argBQty, string argRowid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE ADMIN.OCS_DRUG_SET SET    ");
            parameter.AppendSql("        QTY   = :QTY                   ");
            parameter.AppendSql("        , BQTY  = :BQTY                  ");
            parameter.AppendSql("  WHERE ROWID = :RID                   ");

            parameter.Add("QTY", argQty);
            parameter.Add("BQTY", argBQty);
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public string GetOcsDrugSetRowidBySucode(string argSuCode, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                            ");
            parameter.AppendSql("  FROM ADMIN.OCS_DRUG_SET                 ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql("   AND SUCODE =:SUCODE                         ");
            parameter.AppendSql("   AND WARDCODE ='TO'                          ");  //종검            
            parameter.AppendSql("   AND JDATE = TO_DATE(:JDATE, 'YYYY-MM-DD')   ");
            
            parameter.Add("SUCODE", argSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JDATE", argDate);

            return ExecuteScalar<string>(parameter);
        }

        public List<OCS_MAYAK_JEGO> GetDayUsedBySucodeDate(string argSucode, string argDate, string argGbn, int nChasu, string argJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.IO,  A.ROOMCODE, A.SUCODE, A.DEPTCODE, A.PTNO, A.SUCODER, A.SNAME,  B.JEPNAME                     ");
            parameter.AppendSql("      ,A.REALQTY * A.NAL AS GREALQTY, A.QTY * A.NAL AS nGQTY , (A.QTY * NAL) - (A.REALQTY * NAL) AS JQTY   ");
            parameter.AppendSql("      ,TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.ENTQTY2, A.ROWID AS RID                                   ");
            parameter.AppendSql("      , A.REALQTY* A.NAL AS REALQTY, A.QTY* A.NAL || '' AS strQTY                                          ");
            parameter.AppendSql("  FROM ADMIN.OCS_DRUG A                                                                               ");
            parameter.AppendSql("      ,ADMIN.DRUG_JEP B                                                                               ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND A.BUILDDATE = TO_DATE(:BUILDATE, 'YYYY-MM-DD')                                                      ");
            parameter.AppendSql("   AND A.GBN2 = :GBN2                                                                                      ");  // 소모
            parameter.AppendSql("   AND A.WARDCODE ='TO'                                                                                    ");  // 종검
            parameter.AppendSql("   AND A.SUCODE = B.JEPCODE(+)                                                                             ");
            parameter.AppendSql("   AND A.SUCODE = :SUCODE                                                                                  ");
            parameter.AppendSql("   AND A.GBN =:GBN                                             ");

            if (nChasu > 0)
            {
                parameter.AppendSql("   AND A.NO1 = :NO1                                            ");
            }
            
            parameter.AppendSql(" ORDER BY A.SNAME, A.PTNO                                          ");

            parameter.Add("BUILDATE", argDate);
            parameter.Add("GBN2", argJob);
            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("GBN", argGbn);

            if (nChasu > 0)
            { 
                parameter.Add("NO1", nChasu);
            }

            return ExecuteReader<OCS_MAYAK_JEGO>(parameter);
        }

        public string GetDrugQtyBySucode(string argSucde, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT SUM(QTY*NAL) AS QTY                             ");
            parameter.AppendSql("  FROM ADMIN.OCS_DRUG                             ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND BUILDDATE = TO_DATE(:BUILDDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND GBN2 = '2'                                      "); //입고(소모)
            parameter.AppendSql("   AND WARDCODE ='TO'                                  ");  //종검            
            parameter.AppendSql("   AND SUCODE =:SUCODE                                 ");
            
            parameter.Add("BUILDDATE", argDate);
            //parameter.Add("SUCODE", argSucde.PadRight(12));
            parameter.Add("SUCODE", argSucde, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<OCS_MAYAK_JEGO> GetDayJegoBySucodeDate(string argSucode, string argDate, string argGbn)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT  GBN2,  A.SUCODE,  B.SUNAMEK , A.QTY strQTY , A.ROWID AS RID  ");
            parameter.AppendSql("   FROM ADMIN.OCS_DRUG A , ADMIN.BAS_SUN B          ");
            parameter.AppendSql("  WHERE A.BUILDDATE =TO_DATE(:BUILDDATE,'YYYY-MM-DD')          ");
            parameter.AppendSql("    AND A.GBN2 IN ('3')                                        ");     //재고
            parameter.AppendSql("    AND A.WARDCODE ='TO'                                       ");     //내시경
            parameter.AppendSql("    AND A.SUCODE = B.SUNEXT                                    ");
            parameter.AppendSql("    AND A.SUCODE = :SUCODE                                     ");
            parameter.AppendSql("    AND A.GBN =:GBN                                            ");

            parameter.Add("SUCODE", argSucode);
            parameter.Add("BUILDDATE", argDate);
            parameter.Add("GBN", argGbn);

            return ExecuteReader<OCS_MAYAK_JEGO>(parameter);
        }

        public OCS_MAYAK_JEGO GetCodeInfoItemBySucode(string argSucode, string argDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.SUCODE , A.BQTY, A.QTY , B.UNITNEW1                ");
            parameter.AppendSql("  FROM ADMIN.OCS_DRUG_SET A,                          ");
            parameter.AppendSql("       ADMIN.BAS_SUN     B                           ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND A.SUCODE = :SUCODE                                  "); 
            parameter.AppendSql("   AND A.JDATE <= TO_DATE(:JDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND A.WARDCODE ='TO'                                    ");  // 종검
            parameter.AppendSql("   AND A.SUCODE = B.SUNEXT(+)                              ");
            parameter.AppendSql(" ORDER BY A.JDATE DESC                                     ");

            parameter.Add("SUCODE", argSucode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("JDATE", argDate);

            return ExecuteReaderSingle<OCS_MAYAK_JEGO>(parameter);
        }


        public int UpdateOcsDrugSetQty(string argQty, string argBuildDate, string argSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE ADMIN.OCS_DRUG SET                        ");
            parameter.AppendSql("        QTY   = :QTY                                   ");
            parameter.AppendSql("        , REALQTY = :QTY                               ");
            parameter.AppendSql("  WHERE BUILDDATE = TO_DATE(:BUILDDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("  AND GBN2 IN ('3')                                    ");
            parameter.AppendSql("  AND WARDCODE = 'TO'                                  ");
            parameter.AppendSql("  AND SUCODE = :SUCODE                                 ");

            parameter.Add("QTY", argQty);
            parameter.Add("BUILDDATE", argBuildDate);
            parameter.Add("SUCODE", argSuCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteNonQuery(parameter);
        }
    }
}
