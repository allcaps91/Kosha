
namespace ComHpcLibB.Repository
{

    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using System.Collections.Generic;


    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HeaJepsuResultPatientRepository : BaseRepository
    {
        public HeaJepsuResultPatientRepository()
        {
        }


        public List<HEA_JEPSU_RESULT_PATIENT> GetListBySdateLtdCodeExcode(string argFDate, string argTDate, long argLtdCode, List<string> argExcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT A.SNAME,A.PTNO,A.PANREMARK,A.DRNAME,A.AGE,A.SEX,B.JUMIN,C.RESULT               ");
            parameter.AppendSql(" ,TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE,A.WRTNO                                          ");
            parameter.AppendSql(" ,TO_CHAR(A.PANDATE,'YYYY-MM-DD') PANDATE                                              ");
            parameter.AppendSql(" FROM ADMIN.HEA_JEPSU A, ADMIN.HIC_PATIENT B, ADMIN.HEA_RESULT C     ");
            parameter.AppendSql(" WHERE A.SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                         ");
            parameter.AppendSql(" AND A.SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                           ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                                                 ");
            parameter.AppendSql(" AND A.GBSTS = '9'                                                                     ");
            parameter.AppendSql(" AND A.LTDCODE = :LTDCODE                                                              ");
            parameter.AppendSql(" AND A.PANO = B.PANO                                                                   ");
            parameter.AppendSql(" AND A.WRTNO = C.WRTNO                                                                 ");
            parameter.AppendSql(" AND C.EXCODE IN (:EXCODE)                                                             ");
            parameter.AppendSql(" ORDER BY A.SNAME                                                                      ");  

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.AddInStatement("EXCODE", argExcode);

            return ExecuteReader<HEA_JEPSU_RESULT_PATIENT>(parameter);
        }

        public List<HEA_JEPSU_RESULT_PATIENT> GetResultBySdateWrtnoLtdcodeExcode(string argFDate, string argTDate, long argWrtno, long argLtdCode, List<string> argExcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT A.PANREMARK,A.DRNAME,C.RESULT,C.EXCODE                                         ");
            parameter.AppendSql(" FROM ADMIN.HEA_JEPSU A, ADMIN.HIC_PATIENT B, ADMIN.HEA_RESULT C     ");
            parameter.AppendSql(" WHERE A.SDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                         ");
            parameter.AppendSql(" AND A.SDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                           ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                                                 ");
            parameter.AppendSql(" AND A.GBSTS = '9'                                                                     ");
            parameter.AppendSql(" AND A.LTDCODE = :LTDCODE                                                              ");
            parameter.AppendSql(" AND A.PANO = B.PANO                                                                   ");
            parameter.AppendSql(" AND A.WRTNO = C.WRTNO                                                                 ");
            parameter.AppendSql(" AND A.WRTNO = :WRTNO                                                                  ");
            parameter.AppendSql(" AND C.EXCODE IN (:EXCODE)                                                             ");

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);
            parameter.Add("WRTNO", argWrtno);
            parameter.Add("LTDCODE", argLtdCode);
            parameter.AddInStatement("EXCODE", argExcode);

            return ExecuteReader<HEA_JEPSU_RESULT_PATIENT>(parameter);
        }

        




    }
}
