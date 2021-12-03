namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    public class HicResultSunapdtlRepository : BaseRepository
    {

        public HicResultSunapdtlRepository()
        {
        }

        public List<HIC_RESULT_SUNAPDTL> GetItembyWrtnoExcodeIN(long argWrtno, List<string> lstExcode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT ExCode,Result FROM ADMIN.HIC_RESULT A, ADMIN.HIC_SUNAPDTL B");
            parameter.AppendSql("  WHERE A.WRTNO = :WRTNO                                                       ");
            parameter.AppendSql("    AND A.WRTNO = B.WRTNO                                                      ");
            parameter.AppendSql("    AND A.GROUPCODE = B.CODE                                                   ");
            parameter.AppendSql("    AND ExCode IN (:EXCODE)                                                    ");
            parameter.AppendSql("    AND B.GBSELF IN ('','1','01')                                              ");

            parameter.Add("WRTNO", argWrtno);
            parameter.AddInStatement("EXCODE", lstExcode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_SUNAPDTL>(parameter);
        }

        public List<HIC_RESULT_SUNAPDTL> GetCodebyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.Code                                                                 ");
            parameter.AppendSql("   FROM ADMIN.HIC_RESULT A, ADMIN.HIC_SUNAPDTL B                   ");
            parameter.AppendSql("  WHERE A.WRTNO = :WRTNO                                                       ");
            parameter.AppendSql("    AND A.WRTNO = B.WRTNO                                                      ");
            parameter.AppendSql("    AND A.GROUPCODE = B.CODE                                                   ");
            parameter.AppendSql("    AND ExCode IN (:EXCODE)                                                    ");
            parameter.AppendSql("    AND B.GBSELF IN ('','1','01')                                              ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_RESULT_SUNAPDTL>(parameter);
        }

        public int GetCountbyWrtNoExCode(long nWRTNO, string eXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT COUNT('X') CNT                                                         ");
            parameter.AppendSql("   FROM ADMIN.HIC_RESULT A, ADMIN.HIC_SUNAPDTL B                   ");
            parameter.AppendSql("  WHERE a.WRTNO = :WRTNO                                                       ");
            parameter.AppendSql("    AND a.GbSelf IN ('2','3','6')                                              "); //조합부담이 없으면
            parameter.AppendSql("    AND a.wrtno  = b.wrtno(+)                                                  ");
            parameter.AppendSql("    AND a.Code   = b.GroupCode                                                 ");
            parameter.AppendSql("    AND b.EXCODE = :EXCODE                                                     ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", eXCODE);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_RESULT_SUNAPDTL> GetCodebyWrtNoExCode(long nWRTNO, string eXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.CODE                                                                 ");
            parameter.AppendSql("   FROM ADMIN.HIC_RESULT A, ADMIN.HIC_SUNAPDTL B                   ");
            parameter.AppendSql("  WHERE a.WRTNO = :WRTNO                                                       ");
            parameter.AppendSql("    AND a.GbSelf = '1'                                                         ");
            parameter.AppendSql("    AND a.wrtno = b.wrtno(+)                                                   ");
            parameter.AppendSql("    AND a.Code = b.GroupCode                                                   ");
            parameter.AppendSql("    AND b.EXCODE = :EXCODE                                                     ");
            if (eXCODE == "TX27")
            {
                parameter.AppendSql("    AND A.CODE IN ('9265')                                                 ");
            }
            else if (eXCODE == "TX26")
            {
                parameter.AppendSql("    AND A.CODE IN ('9270')                                                 ");
            }
            else if (eXCODE == "A163")
            {
                parameter.AppendSql("    AND A.CODE IN ('3132')                                                 ");
            }
            parameter.AppendSql("    AND b.RESULT NOT IN (' ','.','미실시')                                     ");
            parameter.AppendSql("  GROUP by a.CODE                                                              ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", eXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_SUNAPDTL>(parameter);
        }

        public List<HIC_RESULT_SUNAPDTL> GetItembyWrtNoExCode(long nWRTNO, string eXCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT a.Code  FROM ADMIN.HIC_SUNAPDTL A, ADMIN.HIC_RESULT B      ");
            parameter.AppendSql("  WHERE A.WRTNO  = :WRTNO                                                      ");
            parameter.AppendSql("    AND a.GbSelf = '2'                                                         ");
            parameter.AppendSql("    AND a.WRTNO  = b.WRTNO                                                     ");
            parameter.AppendSql("    AND a.CODE   = b.GroupCode                                                 ");
            parameter.AppendSql("    AND b.EXCODE = :EXCODE                                                     ");
            if (eXCODE == "TX27")
            {
                parameter.AppendSql("    AND A.CODE IN ('9265')                                                 ");
            }
            else if (eXCODE == "TX26")
            {
                parameter.AppendSql("    AND A.CODE IN ('9270')                                                 ");
            }
            parameter.AppendSql("    AND b.RESULT NOT IN(' ', '.', '미실시')                                    ");
            parameter.AppendSql("  GROUP BY a.Code                                                              ");
 
            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("EXCODE", eXCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_RESULT_SUNAPDTL>(parameter);
        }

        public List<HIC_RESULT_SUNAPDTL> GetExCodeResultGbSelfbyWrtNo(long argWRTNO, string argLtd, string strOK3)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT ExCode,Result                                                          ");
            parameter.AppendSql("   FROM ADMIN.HIC_RESULT A, ADMIN.HIC_SUNAPDTL B                   ");
            parameter.AppendSql("  WHERE A.WRTNO = :WRTNO                                                       ");
            parameter.AppendSql("    AND A.WRTNO = B.WRTNO(+)                                                   ");
            parameter.AppendSql("    AND a.GroupCode = b.Code(+)                                                ");
            parameter.AppendSql("    AND b.GbSelf NOT IN ('2','3')                                              ");
            if (argLtd == "1")
            {
                parameter.AppendSql("    AND ( b.GbSelf IS NULL OR b.GbSelf <> '2' )                            ");
            }
            if (strOK3 == "OK")
            {
                parameter.AppendSql(" UNION ALL                                                                 ");
                parameter.AppendSql(" SELECT a.ExCode,a.Result, b.Gbself                                        ");
                parameter.AppendSql("   FROM ADMIN.HIC_RESULT a, ADMIN.HIC_SUNAPDTL b               ");
                parameter.AppendSql("  WHERE a.WRTNO = b.WRTNO(+)                                               ");
                parameter.AppendSql("    AND a.GroupCode = b.Code(+)                                            ");
                parameter.AppendSql("    AND a.WRTNO = :WRTNO                                                   ");
                parameter.AppendSql("    AND A.EXCODE IN('A123','A241','A242','C404')                           ");
                parameter.AppendSql("  GROUP BY a.excode, a.result, b.gbself                                    ");
            }

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_RESULT_SUNAPDTL>(parameter);
        }
    }
}
