namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class ExamSpecmstResultAnatmstRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public ExamSpecmstResultAnatmstRepository()
        {
        }

        public List<EXAM_SPECMST_RESULT_ANATMST> GetItembyMasterGroup(string strFrDate, string strToDate, List<string> strMasterGrop, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT S.IPDOPD,     R.PANO,     S.DEPTCODE, S.DRCODE                      ");
            parameter.AppendSql("     , A.REMARK1, A.REMARK2, A.REMARK3, A.REMARK4                          ");
            parameter.AppendSql("     , R.MASTERCODE, S.WARD,     S.ROOM                                    ");
            parameter.AppendSql("     , TO_CHAR(S.BDATE, 'YYYY-MM-DD') BDATE,   S.SPECNO                    ");
            parameter.AppendSql("     , S.SPECCODE,   R.RESULTWS, S.BI,       S.SNAME,   S.ANATNO           ");
            parameter.AppendSql("     , TO_CHAR(S.RECEIVEDATE, 'YYYY-MM-DD') RECEIVEDATE                    ");
            parameter.AppendSql("  FROM ADMIN.EXAM_SPECMST S                                           ");
            parameter.AppendSql("     , ADMIN.EXAM_ResultC R                                           ");
            parameter.AppendSql("     , ADMIN.EXAM_ANATMST A                                           ");
            parameter.AppendSql(" WHERE S.RECEIVEDATE BETWEEN TO_DATE(:FRDATE, 'YYYY-MM-DD HH24:MI')        ");
            parameter.AppendSql("                         AND TO_DATE(:TODATE, 'YYYY-MM-DD HH24:MI')        ");
            parameter.AppendSql("   AND R.SPECNO     = S.SPECNO                                             ");
            parameter.AppendSql("   AND R.SPECNO     = A.SPECNO(+)                                          ");
            parameter.AppendSql("   AND S.SEX = 'F'                                                         ");
            if (strGubun == "OK")
            {
                parameter.AppendSql("      AND S.STATUS     NOT IN ('00','05','06')                         ");
            }
            parameter.AppendSql("      AND R.MASTERCODE IN (:MASTERCODE)                                    ");
            parameter.AppendSql("      AND S.DEPTCODE = 'TO'                                                ");
            parameter.AppendSql("ORDER BY S.PANO, S.BDATE, R.SPECNO                                         ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            parameter.AddInStatement("MASTERCODE", strMasterGrop);
            

            return ExecuteReader<EXAM_SPECMST_RESULT_ANATMST>(parameter);
        }
    }
}
