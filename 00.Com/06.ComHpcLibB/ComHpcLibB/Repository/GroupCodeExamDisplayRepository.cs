
namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class GroupCodeExamDisplayRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public GroupCodeExamDisplayRepository()
        {
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetListByGWrtno(long nGWRTNO, long nWRTNO)
        {
            //TODO : 종별 금액 정립할것
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.GWRTNO, A.WRTNO, A.GROUPCODE, A.EXCODE AS EXCODE  ");
            parameter.AppendSql("     , B.HNAME AS EXNAME, B.AMT2 AMT                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EXAM_LIST A,                        ");
            parameter.AppendSql("       KOSMOS_PMPA.HIC_EXCODE B                            ");
            parameter.AppendSql(" WHERE 1 = 1                                               ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE                                   ");
            parameter.AppendSql("   AND A.GWRTNO  =:GWRTNO                                  ");
            if (nWRTNO > 0)
            {
                parameter.AppendSql("   AND A.WRTNO  =:WRTNO                                ");
            }
            parameter.AppendSql(" ORDER BY A.WRTNO, A.GROUPCODE, A.EXCODE                   ");

            parameter.Add("GWRTNO", nGWRTNO);

            if (nWRTNO > 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetListExcodeByGrpCodeList(List<string> lstgrpCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXCODE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM               ");
            parameter.AppendSql(" WHERE GROUPCODE IN (:GROUPCODES)              ");
            parameter.AppendSql(" GROUP By EXCODE                               ");

            parameter.AddInStatement("GROUPCODES", lstgrpCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaExamListByGroupCode(string gRPCODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT '' AS GROUPCODE, A.EXCODE AS EXCODE, B.HNAME AS EXNAME, B.RESCODE, B.PART, B.HEAPART        ");
            parameter.AppendSql("      ,KOSMOS_PMPA.FC_HEA_CODE_NM('13', A.EXCODE) AS ETCEXAM                                       ");
            parameter.AppendSql("      ,DECODE(a.ExCode,'TX20','0','TX22','0','TX23','0','TX32','0','TX41','0','TX64','0','1') GUBUN");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM A                                                                 ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE B                                                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                       ");
            parameter.AppendSql("   AND A.GROUPCODE =:GROUPCODE                                                                     ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                                                        ");
            parameter.AppendSql("   AND B.PART != '5'                                                                               "); //액팅코드 제외
            parameter.AppendSql(" GROUP BY A.EXCODE,B.PART,B.RESCODE,B.HNAME,B.HEAPART                                              ");
            parameter.AppendSql(" ORDER BY GUBUN, A.EXCODE                                                                          ");

            parameter.Add("GROUPCODE", gRPCODE);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaListExcodeByGrpCodeList(List<string> lstgrpCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXCODE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM               ");
            parameter.AppendSql(" WHERE GROUPCODE IN (:GROUPCODES)              ");
            parameter.AppendSql(" GROUP By EXCODE                               ");

            parameter.AddInStatement("GROUPCODES", lstgrpCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetExamListByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.GROUPCODE, A.EXCODE, B.HNAME AS EXNAME, B.AMT2 AMT                ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME (A.GROUPCODE)  AS GROUPCODENAME   ");
            parameter.AppendSql("     , B.ENTPART, B.RESCODE, B.PART                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM A                                         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE B                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND A.GROUPCODE IN (                                                    ");
            parameter.AppendSql("       SELECT CODE FROM HIC_SUNAPDTL WHERE WRTNO =:WRTNO )                 ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                                ");
            parameter.AppendSql("   AND B.DELDATE IS NULL                                                   ");
            parameter.AppendSql("   AND B.PART != '9'                                                       ");
            parameter.AppendSql(" ORDER BY A.GROUPCODE, A.EXCODE                                            ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaListByGroupCode(List<string> suInfo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT '' AS GROUPCODE, A.EXCODE AS EXCODE, B.HNAME AS EXNAME, B.RESCODE, B.PART, B.HEAPART        ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HEA_CODE_NM('13', A.EXCODE) AS ETCEXAM                                       ");
            parameter.AppendSql("     ,DECODE(a.ExCode,'TX20','0','TX22','0','TX23','0','TX32','0','TX41','0','TX64','0','1') GUBUN ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_GROUPEXAM A                                                                 ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE B                                                                    ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                       ");
            parameter.AppendSql("   AND A.GROUPCODE IN (:GROUPCODE)                                                                 ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                                                        ");
            parameter.AppendSql(" GROUP BY A.EXCODE,B.PART,B.RESCODE,B.HNAME,B.HEAPART                                              ");
            parameter.AppendSql(" ORDER BY GUBUN, A.EXCODE                                                                          ");

            parameter.AddInStatement("GROUPCODE", suInfo);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHicListByGroupCode(List<string> suInfo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.GROUPCODE, A.EXCODE, B.HNAME AS EXNAME, B.AMT2 AMT                ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME (A.GROUPCODE)  AS GROUPCODENAME   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_GROUPEXAM A                                         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXCODE B                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND A.GROUPCODE IN (:GROUPCODE)                                         ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                                ");
            parameter.AppendSql(" GROUP BY A.GROUPCODE, A.EXCODE, B.HNAME, B.AMT2                           ");
            parameter.AppendSql(" ORDER BY A.GROUPCODE, A.EXCODE                                            ");

            parameter.AddInStatement("GROUPCODE", suInfo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaListByWrtno(object argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO, '' AS GROUPCODE, A.EXCODE AS EXCODE            ");
            parameter.AppendSql("     , B.HNAME AS EXNAME, B.RESCODE, B.PART, B.HEAPART         ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HEA_CODE_NM('13', A.EXCODE) AS ETCEXAM   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_RESULT A,                               ");
            parameter.AppendSql("       KOSMOS_PMPA.HIC_EXCODE B                                ");
            parameter.AppendSql(" WHERE 1 = 1                                                   ");
            parameter.AppendSql("   AND A.WRTNO  =:WRTNO                                        ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE(+)                                    ");
            parameter.AppendSql(" ORDER BY A.EXCODE                                             ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHicListByWrtno(long argWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO, A.GROUPCODE, A.EXCODE AS EXCODE                            ");
            parameter.AppendSql("     , B.HNAME AS EXNAME, B.AMT2 AMT                                       ");
            parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_GROUPCODE_NAME (A.GROUPCODE)  AS GROUPCODENAME   ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_RESULT A,                                           ");
            parameter.AppendSql("       KOSMOS_PMPA.HIC_EXCODE B                                            ");
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND A.EXCODE = B.CODE                                                   ");
            parameter.AppendSql("   AND A.WRTNO  =:WRTNO                                                    ");
            parameter.AppendSql(" ORDER BY A.WRTNO, A.GROUPCODE, A.EXCODE                                   ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<GROUPCODE_EXAM_DISPLAY>(parameter);
        }
    }
}
