namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayDetailRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayDetailRepository()
        {
        }

        public void InsertData(XRAY_DETAIL item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.XRAY_DETAIL (                                                          ");
            parameter.AppendSql("       ENTERDATE,IPDOPD,GBRESERVED,SEEKDATE,PANO,SNAME,SEX,AGE,DEPTCODE,DRCODE                 ");
            parameter.AppendSql("      ,XJONG,XSUBCODE,XCODE,EXINFO,QTY,REMARK,XRAYROOM,PACSNO,ORDERNAME,ORDERCODE              ");
            parameter.AppendSql("      ,BDATE,GBSPC,GBHIC,HIC_WRTNO                                                             ");
            
            parameter.AppendSql(" ) VALUES (                                                                                    ");
            parameter.AppendSql("       SYSDATE,:IPDOPD,:GBRESERVED,:SEEKDATE,:PANO,:SNAME,:SEX,:AGE,:DEPTCODE,:DRCODE          ");
            parameter.AppendSql("      ,:XJONG,:XSUBCODE,:XCODE,:EXINFO,:QTY,:REMARK,:XRAYROOM,:PACSNO,:ORDERNAME,:ORDERCODE    ");
            parameter.AppendSql("      ,TO_DATE(:BDATE,'YYYY-MM-DD'),:GBSPC,:GBHIC,:HIC_WRTNO                                   ");
            parameter.AppendSql(" )                                                                                             ");

            parameter.Add("IPDOPD", item.IPDOPD);
            parameter.Add("GBRESERVED", item.GBRESERVED);
            parameter.Add("SEEKDATE", item.SEEKDATE);
            parameter.Add("PANO", item.PANO);
            parameter.Add("SNAME", item.SNAME);
            parameter.Add("SEX", item.SEX);
            parameter.Add("AGE", item.AGE);
            parameter.Add("DEPTCODE", item.DEPTCODE);
            parameter.Add("DRCODE", item.DRCODE);
            parameter.Add("XJONG", item.XJONG);
            parameter.Add("XSUBCODE", item.XSUBCODE);
            parameter.Add("XCODE", item.XCODE);
            parameter.Add("EXINFO", item.EXINFO);
            parameter.Add("QTY", item.QTY);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("XRAYROOM", item.XRAYROOM);
            parameter.Add("PACSNO", item.PACSNO);
            parameter.Add("ORDERNAME", item.ORDERNAME);
            parameter.Add("ORDERCODE", item.ORDERCODE);
            parameter.Add("BDATE", item.BDATE.Substring(0, 10));
            parameter.Add("GBSPC", item.GBSPC);
            parameter.Add("GBHIC", item.GBHIC);
            parameter.Add("HIC_WRTNO", item.HIC_WRTNO);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByPanoXCodeBDateDept(string argPano, string argXCode, string argBDate, string argDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID AS RID                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL                     ");
            parameter.AppendSql(" WHERE PANO = :PANO                                ");
            parameter.AppendSql("   AND XCODE = :XCODE                              ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')       ");
            parameter.AppendSql("   AND DEPTCODE =:DEPT                             ");
            parameter.AppendSql("   AND GBSTS <> 'D'                                ");
            //parameter.AppendSql("   AND GBRESERVED = '7'                            ");

            parameter.Add("PANO", argPano, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("XCODE", argXCode, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", argBDate);
            parameter.Add("DEPT", argDept, Oracle.DataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public int GetCountbyPtNo(string strPtNo, string jEPDATE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL                     ");
            parameter.AppendSql(" WHERE PANO     = :PANO                            ");
            parameter.AppendSql("   AND DeptCode IN ('HR','TO')                     ");
            parameter.AppendSql("   AND BDate    = TO_DATE(:BDATE, 'YYYY-MM-DD')    ");
            parameter.AppendSql("   AND XJong    = '1'                              ");
            parameter.AppendSql("   AND XSubCode = '01'                             ");
            parameter.AppendSql("   AND Pacs_End = 'Y'                              ");

            parameter.Add("PANO", strPtNo);
            parameter.Add("BDATE", jEPDATE);

            return ExecuteScalar<int>(parameter);
        }

        public string GetXrayNoByPanoSeekDateXCode(string argPano, string argDate, string argXCode, string argDept)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PACSNO AS XRAYNO                                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.XRAY_DETAIL                             ");
            parameter.AppendSql(" WHERE PANO = :PANO                                        ");
            parameter.AppendSql("   AND TRUNC(SEEKDATE) = TO_DATE(:SEEKDATE, 'YYYY-MM-DD')  ");
            if (argDept == "TO")
            {
                parameter.AppendSql("   AND DEPTCODE IN ('TO')                             ");
            }
            else if (argDept == "HR")
            {
                parameter.AppendSql("   AND DEPTCODE IN ('HR')                             ");
            }
            else
            {
                parameter.AppendSql("   AND DEPTCODE IN ('TO','HR')                             ");
            }
            
            parameter.AppendSql("   AND XCODE = :XCODE                                      ");
            
            parameter.Add("PANO", argPano);
            parameter.Add("SEEKDATE", argDate.Substring(0, 10));
            parameter.Add("XCODE", argXCode);

            return ExecuteScalar<string>(parameter);
        }

        public void UpDateData(XRAY_DETAIL item)
        {
            MParameter parameter = CreateParameter();

            //parameter.AppendSql("UPDATE KOSMOS_PMPA.XRAY_DETAIL SET SEEKDATE = TRUNC(SYSDATE)   ");
            parameter.AppendSql("UPDATE KOSMOS_PMPA.XRAY_DETAIL SET SEEKDATE = TO_DATE(:SEEKDATE, 'YYYY-MM-DD HH24:MI')     ");
            parameter.AppendSql(" WHERE PANO =:PANO                                                                         ");
            parameter.AppendSql("   AND DEPTCODE =:DEPTCODE                                                                 ");
            parameter.AppendSql("   AND XCODE =:XCODE                                                                       ");
            parameter.AppendSql("   AND BDATE = TO_DATE(:BDATE,'YYYY-MM-DD')                                                ");
            parameter.AppendSql("   AND (GBEND IS NULL OR GBEND ='' )                                                       ");

            parameter.Add("PANO", item.PANO, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("DEPTCODE", item.DEPTCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("XCODE", item.XCODE, Oracle.DataAccess.Client.OracleDbType.Char);
            parameter.Add("BDATE", item.BDATE);
            parameter.Add("SEEKDATE", item.SEEKDATE.Value.ToString("yyyy-MM-dd HH:mm"));

            ExecuteNonQuery(parameter);
        }

        public int UpDate_XrayDetail_Del(string argRowid)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.XRAY_DETAIL         ");
            parameter.AppendSql("   SET GBRESERVED = '1'               ");
            parameter.AppendSql("      ,GBSTS = 'D'                     ");
            parameter.AppendSql("      ,DELDATE = SYSDATE               ");
            parameter.AppendSql("      ,CSABUN = '222'                  ");
            parameter.AppendSql("      ,CREMARK  = '종검묶음코드 변경'    ");
            parameter.AppendSql(" WHERE ROWID =:RID                     ");

            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }

        public int UpDate_GBRESERVED(string argRowid, string argGbReserved)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.XRAY_DETAIL         ");
            parameter.AppendSql("   SET GBRESERVED = :GBRESERVED        ");
            parameter.AppendSql(" WHERE ROWID =:RID                     ");
            parameter.AppendSql(" AND GBRESERVED = '6'                  ");

            parameter.Add("GBRESERVED", argGbReserved);
            parameter.Add("RID", argRowid);

            return ExecuteNonQuery(parameter);
        }




        public void InsertHis(string argRowid)
        {
            MParameter parameter = CreateParameter();

 
            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.XRAY_DETAIL (                                                              ");
            parameter.AppendSql(" ENTERDATE, IPDOPD, GBRESERVED, SEEKDATE, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE                  ");
            parameter.AppendSql(" ,WARDCODE, ROOMCODE, XJONG, XSUBCODE, XCODE, EXINFO, QTY, EXMORE, EXID, GBEND, MGRNO, GBPORTABLE  ");
            parameter.AppendSql(" ,REMARK, XRAYROOM, GBNGT, DRREMARK, ORDERNO, ORDERCODE, PACSNO, ORDERNAME, PACSSTUDYID            ");
            parameter.AppendSql(" ,PACS_END, GBREAD, READ_SEND, READ_RECEIVE, READ_FLAG, AGREE, ORDERDATE, SENDDATE                 ");
            parameter.AppendSql(" ,XSENDDATE, PC_BACKDATE, GBPRINT, EXAM_WRTNO, DRDATE, DRWRTNO, STUDY_REF, IMAGE_BDATE             ");
            parameter.AppendSql(" ,GBHIC, HIC_WRTNO, HIC_CODE, BI, CADEX_DEL, BDATE, EMGWRTNO, GBSPC, RDATE, GBSTS, CDATE           ");
            parameter.AppendSql(" ,DELDATE, CSABUN, CREMARK, N_STS, N_REMARK, N_ENTDATE, GB_MANUAL, PICKUPREMARK, CON_DATE          ");
            parameter.AppendSql(" ,GDATE, GSABUN, GBINFO, CVR, CVR_DATE, CVR_GUBUN, CVR_DRSABUN, CVR_SEND, GBER, CVR_CDATE          ");
            parameter.AppendSql(" ,ASA, INPS, INPT_DT, UPPS, UP_DT)                                                                 ");
            parameter.AppendSql(" SELECT ENTERDATE, IPDOPD, GBRESERVED, SEEKDATE, PANO, SNAME, SEX, AGE, DEPTCODE, DRCODE           ");
            parameter.AppendSql(" ,WARDCODE, ROOMCODE, XJONG, XSUBCODE, XCODE, EXINFO, QTY, EXMORE, EXID, GBEND, MGRNO, GBPORTABLE  ");
            parameter.AppendSql(" ,REMARK, XRAYROOM, GBNGT, DRREMARK, ORDERNO, ORDERCODE, PACSNO, ORDERNAME, PACSSTUDYID            ");
            parameter.AppendSql(" ,PACS_END, GBREAD, READ_SEND, READ_RECEIVE, READ_FLAG, AGREE, ORDERDATE, SENDDATE                 ");
            parameter.AppendSql(" ,XSENDDATE, PC_BACKDATE, GBPRINT, EXAM_WRTNO, DRDATE, DRWRTNO, STUDY_REF, IMAGE_BDATE             ");
            parameter.AppendSql(" ,GBHIC, HIC_WRTNO, HIC_CODE, BI, CADEX_DEL, BDATE, EMGWRTNO, GBSPC, RDATE, GBSTS, CDATE           ");
            parameter.AppendSql(" ,DELDATE, CSABUN, CREMARK, N_STS, N_REMARK, N_ENTDATE, GB_MANUAL, PICKUPREMARK, CON_DATE          ");
            parameter.AppendSql(" ,GDATE, GSABUN, GBINFO, CVR, CVR_DATE, CVR_GUBUN, CVR_DRSABUN, CVR_SEND, GBER, CVR_CDATE          ");
            parameter.AppendSql(" ,ASA, INPS, INPT_DT, UPPS, UP_DT                                                                  ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.XRAY_DETAIL                                                                      ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                                ");

            parameter.Add("RID", argRowid);

            ExecuteNonQuery(parameter); ;
        }  
    }
}
