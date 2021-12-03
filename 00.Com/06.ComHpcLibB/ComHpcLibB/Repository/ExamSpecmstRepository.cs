namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class ExamSpecmstRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public ExamSpecmstRepository()
        {
        }

        public int InsertData(EXAM_SPECMST item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO KOSMOS_OCS.EXAM_SPECMST (                                         ");
            parameter.AppendSql("        SPECNO, PANO, BI, SNAME, IPDOPD, AGE, AGEMM, SEX, DEPTCODE             ");
            parameter.AppendSql("       ,WARD, ROOM, DRCODE, DRCOMMENT, STRT, SPECCODE, TUBE, WORKSTS           ");
            parameter.AppendSql("       ,BDATE, BLOODDATE, STATUS, EMR, GB_GWAEXAM, HICNO                       ");
            parameter.AppendSql("       ,RECEIVEDATE, ORDERDATE, SENDDATE, ORDERNO                              ");
            parameter.AppendSql(" ) VALUES (                                                                    ");
            parameter.AppendSql("       :SPECNO, :PANO, :BI, :SNAME, :IPDOPD, :AGE, :AGEMM, :SEX, :DEPTCODE     ");
            parameter.AppendSql("      ,:WARD, :ROOM, :DRCODE, :DRCOMMENT, :STRT, :SPECCODE, :TUBE, :WORKSTS    ");
            parameter.AppendSql("      ,:BDATE, :BLOODDATE, :STATUS, :EMR, :GB_GWAEXAM, :HICNO                  ");
            if (!item.RECEIVEDATE.IsNullOrEmpty())
            {
                parameter.AppendSql(" ,:RECEIVEDATE ");
            }

            if (!item.ORDERDATE.IsNullOrEmpty())
            {
                parameter.AppendSql(" ,:ORDERDATE ");
            }

            if (!item.SENDDATE.IsNullOrEmpty())
            {
                parameter.AppendSql(" ,:SENDDATE ");
            }

            if (item.ORDERNO > 0)
            {
                parameter.AppendSql(" ,:ORDERNO ");
            }

            parameter.AppendSql("  ) ");

            #region Query 변수대입
            parameter.Add("SPECNO",      item.SPECNO);
            parameter.Add("PANO",        item.PANO);
            parameter.Add("BI",          item.BI);
            parameter.Add("SNAME",       item.SNAME);      
            parameter.Add("IPDOPD",      item.IPDOPD);     
            parameter.Add("AGE",         item.AGE);        
            parameter.Add("AGEMM",       item.AGEMM);      
            parameter.Add("SEX",         item.SEX);        
            parameter.Add("DEPTCODE",    item.DEPTCODE);   
            parameter.Add("WARD",        item.WARD);       
            parameter.Add("ROOM",        item.ROOM);       
            parameter.Add("DRCODE",      item.DRCODE);     
            parameter.Add("DRCOMMENT",   item.DRCOMMENT);  
            parameter.Add("STRT",        item.STRT);       
            parameter.Add("SPECCODE",    item.SPECCODE);   
            parameter.Add("TUBE",        item.TUBE);       
            parameter.Add("WORKSTS",     item.WORKSTS);    
            parameter.Add("BDATE",       item.BDATE);      
            parameter.Add("BLOODDATE",   item.BLOODDATE);
            if (!item.RECEIVEDATE.IsNullOrEmpty())
            {
                parameter.Add("RECEIVEDATE", item.RECEIVEDATE);
            }

            parameter.Add("STATUS",      item.STATUS);     
            parameter.Add("EMR",         item.EMR);

            if (!item.ORDERDATE.IsNullOrEmpty())
            {
                parameter.Add("ORDERDATE", item.ORDERDATE);
            }

            if (!item.SENDDATE.IsNullOrEmpty())
            {
                parameter.Add("SENDDATE", item.SENDDATE);
            }
              
            parameter.Add("GB_GWAEXAM",  item.GB_GWAEXAM); 
            parameter.Add("HICNO",       item.HICNO);
            if (item.ORDERNO > 0)
            {
                parameter.Add("ORDERNO", item.ORDERNO);
            }
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public List<EXAM_SPECMST> GetItembySpecNo(string strSpecNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SPECNO, SNAME, DEPTCODE, ROOM, DRCODE, WORKSTS, SPECCODE, TUBE     ");
            parameter.AppendSql("      , STRT, AGE, SEX, TO_CHAR(BLOODDATE, 'HH24:MI') BDATE                ");
            parameter.AppendSql("      , TO_CHAR(BDATE, 'YYYY-MM-DD') BBDATE                                ");
            parameter.AppendSql("      , PANO, IPDOPD, WARD, BI, HICNO                                      ");
            parameter.AppendSql("   FROM KOSMOS_OCS.EXAM_SPECMST                                            ");
            parameter.AppendSql("  WHERE SPECNO = :SPECNO                                                   ");

            parameter.Add("SPECNO", strSpecNo);

            return ExecuteReader<EXAM_SPECMST>(parameter);
        }

        public string GetResultbyPtNoMsCode(string argPTNO, string strMSCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT b.RESULT                           ");
            parameter.AppendSql("   FROM KOSMOS_OCS.EXAM_SPECMST a          ");
            parameter.AppendSql("      , KOSMOS_OCS.EXAM_RESULTC b          ");
            parameter.AppendSql("  WHERE a.PANO = :PANO                     ");
            parameter.AppendSql("    AND b.Result IS NOT NULL               ");
            parameter.AppendSql("    AND a.Specno = b.Specno                ");
            parameter.AppendSql("    AND a.Pano = b.Pano                    ");
            parameter.AppendSql("    AND b.MASTERCODE = :MASTERCODE         ");
            parameter.AppendSql("    AND b.STATUS = 'V'                     ");
            parameter.AppendSql("  ORDER BY a.BDate DESC                    ");

            parameter.Add("PANO", argPTNO);
            parameter.Add("MASTERCODE", strMSCode);

            return ExecuteScalar<string>(parameter);
        }

        public List<EXAM_SPECMST> GetSpecNoByHicNo(long wRTNO, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT SPECNO FROM KOSMOS_OCS.EXAM_SPECMST    ");
            parameter.AppendSql("  WHERE PANO =:PANO                            ");
            parameter.AppendSql("    AND HICNO =:HICNO                          ");

            parameter.Add("PANO", argPtno);
            parameter.Add("HICNO", wRTNO);

            return ExecuteReader<EXAM_SPECMST>(parameter);
        }

        public int UpDateSendFlag(string fstrSpecNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" UPDATE KOSMOS_OCS.EXAM_SPECMST SET SENDFLAG = 'OK'  ");
            parameter.AppendSql("        WHERE SPECNO =:SPECNO                        ");
            
            #region Query 변수대입
            parameter.Add("SPECNO", fstrSpecNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public List<EXAM_SPECMST> GetListByHicList(string argFDate, string argTDate, string argBi, string argSName, string argSendFlag, string argJobFlag)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT /* INDEX(EXAM_SPECMST INDEX_EXAMSPECMST4) */                            ");
            parameter.AppendSql("      SPECNO, PANO, SNAME, AGE || '/' || SEX AS AGESEX, DEPTCODE               ");
            parameter.AppendSql("     ,DECODE(SENDFLAG, 'OK', '전송', '') AS SENDFLAG                           ");
            parameter.AppendSql("     ,'' ROOMCODE, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) AS DRNAME, WORKSTS, SPECCODE   ");
            parameter.AppendSql("     ,DECODE(STATUS, '04', '일부완료', '검사완료') AS STATUS, ANATNO           ");
            parameter.AppendSql("     ,TO_CHAR(RESULTDATE, 'YYYY-MM-DD HH24:MI') RESULTDATE, HICNO              ");
            parameter.AppendSql("     ,TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE                                       ");
            parameter.AppendSql("     ,KOSMOS_OCS.FC_EXAM_RESULTC_MSTNAME(SPECNO) AS EXNAME                     ");
            parameter.AppendSql("  FROM KOSMOS_OCS.EXAM_SPECMST                                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                                   ");
            parameter.AppendSql("  AND BDATE >= TO_DATE(:BFDATE,'YYYY-MM-DD')                                   ");
            parameter.AppendSql("  AND BDATE <  TO_DATE(:BTDATE,'YYYY-MM-DD')                                   ");
            if (argJobFlag.Equals("A"))
            {
                parameter.AppendSql("  AND STATUS ='05'                                                         ");
                parameter.AppendSql("  AND ANATNO IS NOT NULL                                                   ");
            }
            else
            {
                parameter.AppendSql("  AND STATUS IN ('04','05')                                                ");
            }
            
            parameter.AppendSql("  AND BI=:BI                                                                   "); //종합건진
            if (!argSName.IsNullOrEmpty())
            {
                parameter.AppendSql("  AND SNAME LIKE :SNAME                                        ");
            }

            if (argSendFlag.Equals("NO"))
            {
                parameter.AppendSql("  AND (SENDFLAG IS NULL OR SENDFLAG <> 'OK')                   ");
            }
            else if (argSendFlag.Equals("OK"))
            {
                parameter.AppendSql("  AND SENDFLAG = 'OK'                                          ");
            }
            parameter.AppendSql("  ORDER BY PANO, SPECNO                                            ");

            parameter.Add("BFDATE", argFDate);
            parameter.Add("BTDATE", argTDate);
            parameter.Add("BI", argBi);

            if (!argSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }

            return ExecuteReader<EXAM_SPECMST>(parameter);
        }



        public int UpdateHicnoByPtno(long nWrtno, string strPtno, string strDeptcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_OCS.EXAM_SPECMST     ");
            parameter.AppendSql("   SET HICNO   = '0'               ");
            parameter.AppendSql(" WHERE PANO  = :PTNO               ");
            parameter.AppendSql(" AND HICNO  = :WRTNO               ");
            parameter.AppendSql(" AND DEPTCODE  = :DEPTCODE         ");

            parameter.Add("WRTNO", nWrtno);
            parameter.Add("PTNO", strPtno);
            parameter.Add("DEPTCODE", strDeptcode);

            return ExecuteNonQuery(parameter);
        }



        
    }
}
