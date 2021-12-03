namespace HC_Measurement.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC_Measurement.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class HicChukEstimateRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukEstimateRepository()
        {
        }

        public List<HIC_CHUK_ESTIMATE> GetItemByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.SEQNO");
            parameter.AppendSql("     , A.SUCODE");
            parameter.AppendSql("     , B.SUNAME");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_GCODE('15', B.CHKCODE) AS CHKWAY_NM");
            parameter.AppendSql("     , ADMIN.FC_HIC_CODE_GCODE1('15', B.CHKCODE) AS ANALWAY_NM");
            parameter.AppendSql("     , A.QTY");
            parameter.AppendSql("     , A.PRICE");
            parameter.AppendSql("     , A.GBHALIN");
            parameter.AppendSql("     , A.HALINAMT");
            parameter.AppendSql("     , A.AMT");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.JOBSABUN");
            parameter.AppendSql("     , A.ENTTIME");
            parameter.AppendSql("     , A.MCODE");
            parameter.AppendSql("     , A.MCODE_NM");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("     , DECODE(A.DELDATE, '', 'N', 'Y') AS IsDelete");
            parameter.AppendSql("  FROM HIC_CHUK_ESTIMATE_DTL A");
            parameter.AppendSql("     , HIC_CHK_SUGA B");
            parameter.AppendSql(" WHERE A.WRTNO =:WRTNO ");
            parameter.AppendSql("   AND A.SUCODE = B.SUCODE ");
            //parameter.AppendSql("   AND A.DELDATE IS NULL ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_CHUK_ESTIMATE>(parameter);
        }

        public void InSert(HIC_CHUK_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO HIC_CHUK_ESTIMATE_DTL");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , SUCODE");
            parameter.AppendSql("  , QTY");
            parameter.AppendSql("  , PRICE");
            parameter.AppendSql("  , GBHALIN");
            parameter.AppendSql("  , HALINAMT");
            parameter.AppendSql("  , AMT");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , JOBSABUN");
            parameter.AppendSql("  , MCODE");
            parameter.AppendSql("  , MCODE_NM");
            parameter.AppendSql("  , ENTTIME");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :SEQNO");
            parameter.AppendSql("  , :SUCODE");
            parameter.AppendSql("  , :QTY");
            parameter.AppendSql("  , :PRICE");
            parameter.AppendSql("  , :GBHALIN");
            parameter.AppendSql("  , :HALINAMT");
            parameter.AppendSql("  , :AMT");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :JOBSABUN");
            parameter.AppendSql("  , :MCODE");
            parameter.AppendSql("  , :MCODE_NM");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("SUCODE", dto.SUCODE);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("PRICE", dto.PRICE);
            parameter.Add("GBHALIN", dto.GBHALIN);
            parameter.Add("HALINAMT", dto.HALINAMT);
            parameter.Add("AMT", dto.AMT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("MCODE", dto.MCODE);
            parameter.Add("MCODE_NM", dto.MCODE_NM);

            ExecuteNonQuery(parameter);
        }

        public void MinusAmt(HIC_CHUK_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUK_ESTIMATE (");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , TOTAMT");
            parameter.AppendSql("  , BASEAMT");
            parameter.AppendSql("  , CHARGEAMT");
            parameter.AppendSql("  , HALINAMT");
            parameter.AppendSql("  , AMT");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , JOBSABUN");
            parameter.AppendSql("  , PER");
            parameter.AppendSql("  , HALINAMT1");
            parameter.AppendSql("  , HALINAMT2");
            parameter.AppendSql("  , ENTTIME )");
            parameter.AppendSql(" SELECT ");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO+1");
            parameter.AppendSql("  , TOTAMT*-1");
            parameter.AppendSql("  , BASEAMT*-1");
            parameter.AppendSql("  , CHARGEAMT*-1");
            parameter.AppendSql("  , HALINAMT*-1");
            parameter.AppendSql("  , AMT*-1");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , :JOBSABUN");
            parameter.AppendSql("  , PER*-1");
            parameter.AppendSql("  , HALINAMT1*-1");
            parameter.AppendSql("  , HALINAMT2*-1");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("   FROM HIC_CHUK_ESTIMATE ");
            parameter.AppendSql("  WHERE ROWID =:RID ");

            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void UpdateSendMail(string strLtdMgr, string strLtdAddr, string strRid)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUK_ESTIMATE");
            parameter.AppendSql("   SET SENDTIME = SYSDATE");
            parameter.AppendSql("     , LTD_MANAGER = :LTD_MANAGER");
            parameter.AppendSql("     , LTD_EMAILA_DDRESS = :LTD_EMAILA_DDRESS");
            parameter.AppendSql(" WHERE ROWID = :RID");

            parameter.Add("LTD_MANAGER", strLtdMgr);
            parameter.Add("LTD_EMAILA_DDRESS", strLtdAddr);
            parameter.Add("RID", strRid);

            ExecuteNonQuery(parameter);
        }

        public HIC_CHUK_ESTIMATE GetSendInfoByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SENDTIME, 'yyyy-MM-dd HH24:MI') SENDTIME");
            parameter.AppendSql("     , TO_CHAR(PRINTDATE, 'yyyy-MM-dd HH24:MI') PRINTDATE");
            parameter.AppendSql("  FROM HIC_CHUK_ESTIMATE A");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO");
            parameter.AppendSql(" ORDER By SEQNO DESC");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_CHUK_ESTIMATE>(parameter);
        }

        public void MinusSumAmt(HIC_CHUK_ESTIMATE hCE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUK_ESTIMATE (");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , TOTAMT");
            parameter.AppendSql("  , BASEAMT");
            parameter.AppendSql("  , CHARGEAMT");
            parameter.AppendSql("  , HALINAMT");
            parameter.AppendSql("  , AMT");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , JOBSABUN");
            parameter.AppendSql("  , ENTTIME ");
            parameter.AppendSql("  , PER");
            parameter.AppendSql("  , HALINAMT1");
            parameter.AppendSql("  , HALINAMT2");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("   :WRTNO");
            parameter.AppendSql("  ,:SEQNO");
            parameter.AppendSql("  ,:TOTAMT");
            parameter.AppendSql("  ,:BASEAMT");
            parameter.AppendSql("  ,:CHARGEAMT");
            parameter.AppendSql("  ,:HALINAMT");
            parameter.AppendSql("  ,:AMT");
            parameter.AppendSql("  ,:REMARK");
            parameter.AppendSql("  ,:JOBSABUN");
            parameter.AppendSql("  ,SYSDATE ");
            parameter.AppendSql("  , :PER");
            parameter.AppendSql("  , :HALINAMT1");
            parameter.AppendSql("  , :HALINAMT2");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", hCE.WRTNO);
            parameter.Add("SEQNO", hCE.SEQNO);
            parameter.Add("TOTAMT", hCE.TOTAMT);
            parameter.Add("BASEAMT", hCE.BASEAMT);
            parameter.Add("CHARGEAMT", hCE.CHARGEAMT);
            parameter.Add("HALINAMT", hCE.HALINAMT);
            parameter.Add("AMT", hCE.AMT);
            parameter.Add("REMARK", hCE.REMARK);
            parameter.Add("JOBSABUN", hCE.JOBSABUN);
            parameter.Add("PER", hCE.PER);
            parameter.Add("HALINAMT1", hCE.HALINAMT1);
            parameter.Add("HALINAMT2", hCE.HALINAMT2);

            ExecuteNonQuery(parameter);
        }

        public void DeleteAll(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            //parameter.AppendSql("UPDATE ADMIN.HIC_CHUK_ESTIMATE_DTL");
            //parameter.AppendSql("   SET DELDATE = SYSDATE");
            //parameter.AppendSql(" WHERE WRTNO = :WRTNO");

            //완전삭제해야함 다시 띄워볼경우 삭제흔적 보임
            parameter.AppendSql("DELETE ADMIN.HIC_CHUK_ESTIMATE_DTL");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public long GetEstAmtSumByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(AMT) AMT");
            parameter.AppendSql("  FROM HIC_CHUK_ESTIMATE ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public long GetMaxSeqNoByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(SEQNO) SEQNO");
            parameter.AppendSql("  FROM HIC_CHUK_ESTIMATE ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public string GetRowidEstAmtByWrtno(long fnWRTNO, long nSeqno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID");
            parameter.AppendSql("  FROM HIC_CHUK_ESTIMATE ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO");
            parameter.AppendSql("   AND SEQNO = :SEQNO");

            parameter.Add("WRTNO", fnWRTNO);
            parameter.Add("SEQNO", nSeqno);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertAmt(HIC_CHUK_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUK_ESTIMATE");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , SEQNO");
            parameter.AppendSql("  , TOTAMT");
            parameter.AppendSql("  , BASEAMT");
            parameter.AppendSql("  , CHARGEAMT");
            parameter.AppendSql("  , HALINAMT");
            parameter.AppendSql("  , AMT");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , JOBSABUN");
            parameter.AppendSql("  , ENTTIME");
            parameter.AppendSql("  , PER");
            parameter.AppendSql("  , HALINAMT1");
            parameter.AppendSql("  , HALINAMT2");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :SEQNO");
            parameter.AppendSql("  , :TOTAMT");
            parameter.AppendSql("  , :BASEAMT");
            parameter.AppendSql("  , :CHARGEAMT");
            parameter.AppendSql("  , :HALINAMT");
            parameter.AppendSql("  , :AMT");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :JOBSABUN");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("  , :PER");
            parameter.AppendSql("  , :HALINAMT1");
            parameter.AppendSql("  , :HALINAMT2");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("TOTAMT", dto.TOTAMT);
            parameter.Add("BASEAMT", dto.BASEAMT);
            parameter.Add("CHARGEAMT", dto.CHARGEAMT);
            parameter.Add("HALINAMT", dto.HALINAMT);
            parameter.Add("AMT", dto.AMT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("PER", dto.PER);
            parameter.Add("HALINAMT1", dto.HALINAMT1);
            parameter.Add("HALINAMT2", dto.HALINAMT2);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HIC_CHUK_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUK_ESTIMATE_DTL");
            parameter.AppendSql("   SET DELDATE = SYSDATE");
            parameter.AppendSql(" WHERE ROWID = :RID");

            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void UpDate(HIC_CHUK_ESTIMATE dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUK_ESTIMATE_DTL");
            parameter.AppendSql("   SET SEQNO = :SEQNO");
            parameter.AppendSql("     , SUCODE = :SUCODE");
            parameter.AppendSql("     , QTY = :QTY");
            parameter.AppendSql("     , PRICE = :PRICE");
            parameter.AppendSql("     , GBHALIN = :GBHALIN");
            parameter.AppendSql("     , HALINAMT = :HALINAMT");
            parameter.AppendSql("     , AMT = :AMT");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql("     , JOBSABUN = :JOBSABUN");
            parameter.AppendSql("     , DELDATE = :DELDATE");
            parameter.AppendSql("     , MCODE = :MCODE");
            parameter.AppendSql("     , MCODE_NM = :MCODE_NM");
            parameter.AppendSql("     , ENTTIME = SYSDATE");
            parameter.AppendSql(" WHERE ROWID = :RID");

            parameter.Add("SEQNO", dto.SEQNO);
            parameter.Add("SUCODE", dto.SUCODE);
            parameter.Add("QTY", dto.QTY);
            parameter.Add("PRICE", dto.PRICE);
            parameter.Add("GBHALIN", dto.GBHALIN);
            parameter.Add("HALINAMT", dto.HALINAMT);
            parameter.Add("AMT", dto.AMT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("JOBSABUN", dto.JOBSABUN);
            parameter.Add("DELDATE", dto.DELDATE);
            parameter.Add("MCODE", dto.MCODE);
            parameter.Add("MCODE_NM", dto.MCODE_NM);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public HIC_CHUK_ESTIMATE GetSumEstAmtByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(A.TOTAMT) AS TOTAMT");
            parameter.AppendSql("     , SUM(A.BASEAMT) AS BASEAMT");
            parameter.AppendSql("     , SUM(A.CHARGEAMT) AS CHARGEAMT");
            parameter.AppendSql("     , SUM(A.HALINAMT) AS HALINAMT");
            parameter.AppendSql("     , SUM(A.AMT) AS AMT");
            parameter.AppendSql("     , SUM(A.PER) AS PER");
            parameter.AppendSql("     , SUM(A.HALINAMT1) AS HALINAMT1");
            parameter.AppendSql("     , SUM(A.HALINAMT2) AS HALINAMT2");
            parameter.AppendSql("  FROM HIC_CHUK_ESTIMATE A");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_CHUK_ESTIMATE>(parameter);
        }
    }
}
