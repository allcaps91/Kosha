namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class ExamOrderRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamOrderRepository()
        {
        }

        public int UpDate(EXAM_ORDER itemSub01, string strSuCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_OCS.EXAM_ORDER                      ");
            parameter.AppendSql("    SET SPECCODE  = :SPECCODE                      ");
            parameter.AppendSql("       ,STRT      = :STRT                          ");
            parameter.AppendSql("       ,DRCOMMENT = :DRCOMMENT                     ");
            parameter.AppendSql("       ,SPECNO    = :SPECNO                        ");
            parameter.AppendSql(" WHERE 1 = 1                                       ");
            parameter.AppendSql("   AND BDATE    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND PANO     = :PANO                            ");
            parameter.AppendSql("   AND IPDOPD   = :IPDOPD                          ");
            parameter.AppendSql("   AND DEPTCODE = :DEPTCODE                        ");
            parameter.AppendSql("   AND RTRIM(MASTERCODE) = :MASTERCODE             ");
            parameter.AppendSql("   AND SPECNO IS NULL                              ");
            parameter.AppendSql("   AND QTY > 0                                     ");
            parameter.AppendSql("   AND ROWNUM = 1                                  ");

            #region Query 변수대입
            parameter.Add("SPECCODE",   itemSub01.SPECCODE);
            parameter.Add("STRT",       itemSub01.STRT);
            parameter.Add("DRCOMMENT",  itemSub01.DRCOMMENT);
            parameter.Add("SPECNO",     itemSub01.SPECNO);
            parameter.Add("BDATE",      itemSub01.BDATE);
            parameter.Add("PANO",       itemSub01.PANO);
            parameter.Add("IPDOPD",     itemSub01.IPDOPD);
            parameter.Add("DEPTCODE",   itemSub01.DEPTCODE);
            parameter.Add("MASTERCODE", itemSub01.MASTERCODE);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public void Insert(EXAM_ORDER item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_OCS.EXAM_ORDER (                           ");
            parameter.AppendSql("        IPDOPD, BDATE , ACTDATE,PANO ,BI , SNAME, AGE, SEX     ");
            parameter.AppendSql("       ,DEPTCODE,MASTERCODE, SPECCODE,DRCODE,QTY, STRT         ");
            parameter.AppendSql(" ) VALUES (                                                    ");
            parameter.AppendSql("       :IPDOPD, :BDATE ,:ACTDATE,:PANO,:BI ,:SNAME, :AGE, :SEX ");
            parameter.AppendSql("      ,:DEPTCODE,:MASTERCODE, :SPECCODE,:DRCODE,:QTY, :STRT    ");
            parameter.AppendSql(" )                                                             ");

            #region Query 변수대입
            parameter.Add("IPDOPD",item.IPDOPD);
            parameter.Add("BDATE",item.BDATE);
            parameter.Add("ACTDATE",item.ACTDATE);
            parameter.Add("PANO",item.PANO);
            parameter.Add("BI",item.BI);
            parameter.Add("SNAME",item.SNAME);
            parameter.Add("AGE",item.AGE);
            parameter.Add("SEX",item.SEX);
            parameter.Add("DEPTCODE",item.DEPTCODE);
            parameter.Add("MASTERCODE",item.MASTERCODE);
            parameter.Add("SPECCODE",item.SPECCODE);
            parameter.Add("DRCODE",item.DRCODE);
            parameter.Add("QTY",item.QTY);
            parameter.Add("STRT",item.STRT);
            #endregion

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByBDatePanoMasterCD(string argBDate, string argPtno, string argBi, string argMasterCD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID                                          ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_ORDER                          ");
            parameter.AppendSql(" WHERE PANO = :PANO                                   ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')          ");
            parameter.AppendSql("   AND BI =:BI                                        ");
            parameter.AppendSql("   AND IPDOPD ='O'                                    "); 
            parameter.AppendSql("   AND MASTERCODE =:MASTERCODE                        ");

            parameter.Add("BDATE", argBDate);
            parameter.Add("PANO", argPtno, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("BI", argBi, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("MASTERCODE", argMasterCD);

            return ExecuteScalar<string>(parameter);
        }
    }
}
