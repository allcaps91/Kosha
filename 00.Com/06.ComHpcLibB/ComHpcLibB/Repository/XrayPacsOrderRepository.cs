namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class XrayPacsOrderRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public XrayPacsOrderRepository()
        {
        }


        public int Insert(XRAY_PACS_ORDER item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.XRAY_PACS_ORDER(                                               ");
            parameter.AppendSql(" QUEUEID,FALG,WORKTIME,PATID,ACDESSIONNO                                       ");
            parameter.AppendSql(" ,EVENTTYPE,EXAMDATE,EXAMTIME,ExamRoom,EXAMCODE,EXAMNAME                       ");
            parameter.AppendSql(" ,ORDERDOC,ORDERFROM,PATNAME,PATBIRTHDAY                                       ");
            parameter.AppendSql(" ,PATSEX,PATDEPT,PATTYPE,HISORDERID,WARD,ROOM,OPERATOR)                        ");
            parameter.AppendSql("VALUES                                                                         ");
            parameter.AppendSql(" (:QUEUEID, 'N', SYSDATE, :PATID, :ACDESSIONNO                                 ");
            parameter.AppendSql(" ,:EVENTTYPE,:EXAMDATE,'',:EXAMROOM,:EXAMCODE,:EXAMNAME                 ");
            parameter.AppendSql(" ,:ORDERDOC,:ORDERFROM,:PATNAME,:PATBIRTHDAY                                   ");
            parameter.AppendSql(" ,:PATSEX,:PATDEPT,:PATTYPE,:HISORDERID,:WARD,:ROOM,:OPERATOR)                 ");

            parameter.Add("QUEUEID", item.QUEUEID);
            parameter.Add("PATID" , item.PATID);
            parameter.Add("ACDESSIONNO" , item.ACDESSIONNO);
            parameter.Add("EVENTTYPE" , item.EVENTTYPE);
            parameter.Add("EXAMDATE" , item.EXAMDATE);
            parameter.Add("EXAMROOM" , item.EXAMROOM);
            parameter.Add("EXAMCODE" , item.EXAMCODE);
            parameter.Add("EXAMNAME" , item.EXAMNAME);
            parameter.Add("ORDERDOC" , item.ORDERDOC);
            parameter.Add("ORDERFROM" , item.ORDERFROM);
            parameter.Add("PATNAME" , item.PATNAME);
            parameter.Add("PATBIRTHDAY" , item.PATBIRTHDAY);
            parameter.Add("PATSEX" , item.PATSEX);
            parameter.Add("PATDEPT" , item.PATDEPT);
            parameter.Add("PATTYPE", item.PATTYPE);
            parameter.Add("HISORDERID" , item.HISORDERID);
            parameter.Add("WARD" , item.WARD);
            parameter.Add("ROOM" , item.ROOM);
            parameter.Add("OPERATOR" , item.OPERATOR);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyPatIdAcdessionNoExamDate(string strPtNo, string strXrayno, string strExamDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM ADMIN.XRAY_PACS_ORDER                            ");
            parameter.AppendSql(" WHERE PATID       = :PATID                            ");
            parameter.AppendSql("   AND ACDESSIONNO = :ACDESSIONNO                      ");
            parameter.AppendSql("   AND EXAMDATE    = :EXAMDATE                         ");

            parameter.Add("PATID", strPtNo);
            parameter.Add("ACDESSIONNO", strXrayno);
            parameter.Add("EXAMDATE", strExamDate);

            return ExecuteScalar<int>(parameter);
        }
    }
}
