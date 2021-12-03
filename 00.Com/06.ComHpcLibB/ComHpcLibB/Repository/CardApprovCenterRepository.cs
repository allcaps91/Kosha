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
    public class CardApprovCenterRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public CardApprovCenterRepository()
        {
        }

        public List<CARD_APPROV_CENTER> GetItembyActDate(CARD_APPROV_CENTER item, string strGubun, string strCompany, string strPart, string strIO, string strCard, string strBun, string strChk)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(TranDate, 'YYYY-MM-DD') ActDate                                                                         ");
            parameter.AppendSql("     , Pano Pano, HPano                                                                                                ");
            parameter.AppendSql("     , DeptCode DeptCode, SName SName                                                                                  ");
            parameter.AppendSql("     , AccepterName,CardSeqNo,HWrtno                                                                       ");
            parameter.AppendSql("     , TranHeader,CardNo,FullCardNo,FiName,  Period, AccountNo                                                         ");
            parameter.AppendSql("     , TO_CHAR(TranDate, 'YYMMDDHH24MI') TranDate1                                                                     ");
            parameter.AppendSql("     , TO_CHAR(OriginDate, 'YYMMDD') POriginDate                                                                       ");
            parameter.AppendSql("     , case when TranHeader = '1' or TranHeader = '11' then DECODE(InstPeriod, '0', '일반승인', '할부승인')            ");
            parameter.AppendSql("       When TranHeader = '2' or TranHeader = '22' then DECODE(InstPeriod, '0', '일반취소', '할부취소') END  GbRecord   ");
            parameter.AppendSql("     , TO_CHAR(OriginDate, 'MM/DD') OriginDate                                                                         ");
            parameter.AppendSql("     , InstPeriod InstPeriod                                                                                           ");
            parameter.AppendSql("     , TradeAmt TradeAmt                                                                                               ");
            parameter.AppendSql("     , OriginNo OriginNo                                                                                               ");
            parameter.AppendSql("  FROM ADMIN.CARD_APPROV_CENTER A                                                                                ");
            if (strGubun == "0")
            {
                parameter.AppendSql(" WHERE ACTDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                                   ");
                parameter.AppendSql("   AND ACTDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                                   ");
            }
            else if (strGubun == "1")
            {
                parameter.AppendSql(" WHERE PANO = :PANO                                                                                                ");
            }
            else
            {
                if (!item.CARDNO.IsNullOrEmpty())
                {
                    parameter.AppendSql(" WHERE CARDNO LIKE :CARDNO                                                                                     ");
                }
            }
            if (strCompany != "00")
            {
                parameter.AppendSql("   AND FICODE = :FICODE                                                                                            ");
            }
            if (strPart != "")
            {
                parameter.AppendSql("   AND PART = :PART                                                                                                ");
            }
            if (strIO == "0")
            {
                parameter.AppendSql("   AND GBIO  IN( 'H','T' )                                                                                         ");
            }
            else if (strIO == "1")
            {
                parameter.AppendSql("   AND GBIO  = 'H'                                                                                                 ");
            }
            else if (strIO == "2")
            {
                parameter.AppendSql("   AND GBIO  = 'T'                                                                                                 ");
            }
            if (strCard == "0")
            {
                parameter.AppendSql("   AND (GUBUN = '1' OR GUBUN IS NULL)                                                                              ");
            }
            else
            {
                parameter.AppendSql("   AND GUBUN  = '2'                                                                                                ");
            }

            if (strBun == "1")
            {
                parameter.AppendSql("  AND  INPUTMETHOD IN ('S','K')                                                                                    ");
            }
            else if (strBun == "2")
            {
                parameter.AppendSql("  AND  INPUTMETHOD = 'T'                                                                                           "); //단말기 승인
            }
            if (strChk == "OK")
            {
                parameter.AppendSql("  AND  HWrtno = 0 AND HPano NOT IN (41045)                                                                         ");
            }
            if (strGubun == "0")
            {
                if (item.FRDATE != item.TODATE)
                {
                    parameter.AppendSql(" ORDER BY TranDate, Pano, OriginNo                                                                             ");
                }
                else
                {
                    parameter.AppendSql(" ORDER BY pano, OriginNo, TranDate DESC                                                                        ");
                }
            }
            else if (strGubun == "1")
            {
                parameter.AppendSql(" ORDER BY pano, OriginNo, TranDate DESC                                                                            ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY CardNo, OriginNo, TranDate DESC                                                                          ");
            }

            if (strGubun == "0")
            {
                parameter.Add("FRDATE", item.FRDATE);
                parameter.Add("TODATE", item.TODATE);
            }
            if (strGubun == "1")
            {
                parameter.Add("PANO", item.PANO);
            }
            else
            {
                if (!item.CARDNO.IsNullOrEmpty())
                {
                    parameter.AddLikeStatement("CARDNO", item.CARDNO);
                }
            }
            if (strCompany != "00")
            {
                parameter.Add("FICODE", item.FICODE);
            }
            if (strPart != "")
            {
                parameter.Add("PART", item.PART, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<CARD_APPROV_CENTER>(parameter);
        }

        public int UpdateHWrtNobyCardSeqNo(long Idx, long nWRTNO, string argPtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.CARD_APPROV_CENTER  ");
            parameter.AppendSql("   SET HWRTNO = :HWRTNO                ");
            parameter.AppendSql(" WHERE PANO = :PANO                    ");
            parameter.AppendSql("   AND HWRTNO = :IDX                   ");

            parameter.Add("HWRTNO", nWRTNO);
            parameter.Add("PANO", argPtno);
            parameter.Add("IDX", Idx);

            return ExecuteNonQuery(parameter);
        }


        public int UpdateHWrtNobyPano(long argWrtno, string argPtno, long argCardSeqno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.CARD_APPROV_CENTER  ");
            parameter.AppendSql("   SET HWRTNO = :HWRTNO                ");
            parameter.AppendSql(" WHERE PANO = :PANO                    ");
            if (argCardSeqno > 0)
            {
                parameter.AppendSql("   AND (CARDSEQNO = :CARDSEQNO OR HWRTNO = 0 OR HWRTNO IS NULL)    ");
            }
            else
            {
                parameter.AppendSql("   AND ( HWRTNO = 5 OR HWRTNO IS NULL)    ");
            }

            parameter.Add("HWRTNO", argWrtno);
            parameter.Add("PANO", argPtno);
            if (argCardSeqno > 0)
            {
                parameter.Add("CARDSEQNO", argCardSeqno);
            }

            return ExecuteNonQuery(parameter);
        }


        public CARD_APPROV_CENTER GetDataByPanoOrigin(string argPano, string argOrigin)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT FINAME, INSTPERIOD, PERIOD, ORIGINNO, CARDSEQNO, TRADEAMT  ");
            parameter.AppendSql("       ,TO_CHAR(APPROVDATE,'YYYY-MM-DD HH24:MI') APPROVDATE, SNAME ");
            parameter.AppendSql("       ,PANO,ACCEPTERNAME, TRANHEADER, CARDNO, GUBUN1              ");
            parameter.AppendSql("       ,HPANO,HWRTNO                                               ");
            parameter.AppendSql("  FROM ADMIN.CARD_APPROV_CENTER                              ");
            parameter.AppendSql(" WHERE PANO =:PANO                                                 ");
            parameter.AppendSql("   AND ORIGINNO =:ORIGINNO                                         ");

            parameter.Add("PANO", argPano);
            parameter.Add("ORIGINNO", argOrigin);

            return ExecuteReaderSingle<CARD_APPROV_CENTER>(parameter);
        }
        public List<CARD_APPROV_CENTER> GetCardReportData(string argDate, int rdoCheck, string strSaBun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   SELECT ACCEPTERNAME, INPUTMETHOD, ");

            parameter.AppendSql("          SUM(CASE WHEN TRANHEADER ='11' OR TRANHEADER = '1' THEN TRADEAMT ");
            parameter.AppendSql("          WHEN TRANHEADER ='22' OR TRANHEADER = '2' THEN  (-1) * TRADEAMT END) TAMT ");

            parameter.AppendSql("   FROM  ADMIN.CARD_APPROV_CENTER");

            parameter.AppendSql("   WHERE ACTDATE = TO_DATE(:TDATE,'YYYY-MM-DD')  ");
            parameter.AppendSql("          AND TRANHEADER IN ('11','22','1','2')  ");

            if (rdoCheck == 1)
            {
                parameter.AppendSql("          AND GBIO       = 'H'  ");
            }
            else if (rdoCheck == 2)
            {
                parameter.AppendSql("          AND GBIO       = 'T'  ");
            }
            else
            {
                parameter.AppendSql("          AND GBIO       IN ( 'H','T' )  ");
            }

            if (strSaBun != "전체" && strSaBun != "")
            {
                parameter.AppendSql("          AND PART       = :strSaBun  ");
            }

            parameter.AppendSql("          AND PANO       <> '81000004'  ");
            parameter.AppendSql("          AND (GUBUN = '1' OR GUBUN IS NULL)  ");

            parameter.AppendSql("   GROUP BY INPUTMETHOD, ACCEPTERNAME  ");

            if (strSaBun != "전체" && strSaBun != "")
            {
                parameter.Add("strSaBun", strSaBun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            parameter.Add("TDATE", argDate, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<CARD_APPROV_CENTER>(parameter);
        }
    }
}
