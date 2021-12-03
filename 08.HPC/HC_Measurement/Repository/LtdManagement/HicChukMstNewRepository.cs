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
    public class HicChukMstNewRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicChukMstNewRepository()
        {
        }

        public HIC_CHUKMST_NEW GetItemByWrtno(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.CHKYEAR");
            parameter.AppendSql("     , A.BANGI");
            parameter.AppendSql("     , A.LTDCODE");
            parameter.AppendSql("     , A.SDATE");
            parameter.AppendSql("     , A.EDATE");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.T_LIMIT");
            parameter.AppendSql("     , A.T5_LIMIT");
            parameter.AppendSql("     , A.TO_ACCUM");
            parameter.AppendSql("     , A.T5_ACCUM");
            parameter.AppendSql("     , A.LTD_MANAGER");
            parameter.AppendSql("     , A.LTD_GRADE");
            parameter.AppendSql("     , A.LTD_HPHONE");
            parameter.AppendSql("     , A.LTD_EMAIL");
            parameter.AppendSql("     , A.GBSUPPORT");
            parameter.AppendSql("     , A.GBSTS");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.DELSABUN");
            parameter.AppendSql("     , A.LTD_FAX");
            parameter.AppendSql("     , A.GBNEW");
            parameter.AppendSql("     , A.GBWAY");
            parameter.AppendSql("     , A.ILSU");
            parameter.AppendSql("     , A.BDATE");
            parameter.AppendSql("     , A.INWON");
            parameter.AppendSql("     , A.INWON_S");
            parameter.AppendSql("     , A.INWON_H");
            parameter.AppendSql("     , A.GBDAY");
            parameter.AppendSql("     , A.GBSHIFT");
            parameter.AppendSql("     , A.DAYTIME");
            parameter.AppendSql("     , A.SHIFTGRPCNT");
            parameter.AppendSql("     , A.SHIFTQUARTER");
            parameter.AppendSql("     , A.SHIFTTIME");
            parameter.AppendSql("     , A.WORKTIME1");
            parameter.AppendSql("     , A.WORKTIME2");
            parameter.AppendSql("     , A.WORKTIME3");
            parameter.AppendSql("     , A.WORKTIME4");
            parameter.AppendSql("     , A.MEALTIME1");
            parameter.AppendSql("     , A.MEALTIME2");
            parameter.AppendSql("     , A.WORKTIME11");
            parameter.AppendSql("     , A.WORKTIME22");
            parameter.AppendSql("     , A.WORKTIME33");
            parameter.AppendSql("     , A.WORKTIME44");
            parameter.AppendSql("     , A.MEALTIME11");
            parameter.AppendSql("     , A.MEALTIME22");
            parameter.AppendSql("     , A.GBOVERTIME");
            parameter.AppendSql("     , A.OVERTIME");
            parameter.AppendSql("     , A.GBESTIMATE");
            parameter.AppendSql("     , A.GBCORRECT");
            parameter.AppendSql("     , A.GBSAMPLE");
            parameter.AppendSql("     , A.GBUCODE1");
            parameter.AppendSql("     , A.GBUCODE2");
            parameter.AppendSql("     , A.GBUCODE3");
            parameter.AppendSql("     , A.GBUCODE4");
            parameter.AppendSql("     , A.GBUCODE5");
            parameter.AppendSql("     , A.GBUCODE6");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.GBCHROMIUM");
            parameter.AppendSql("     , A.GBEST");
            parameter.AppendSql("     , A.EST_DATE");
            parameter.AppendSql("     , A.EST_SABUN");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.EST_SABUN) EST_JOBNAME");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LTDNAME");
            parameter.AppendSql("     , A.GBTEMP");
            parameter.AppendSql("     , A.LTDSEQNO");
            parameter.AppendSql("     , A.LTDGONGNAME");
            parameter.AppendSql("     , A.CYCLE_PROCS_NEW_CHANGE_YN");
            parameter.AppendSql("     , A.CYCLE_PROCS_NEW_CHANGE_DATE");
            parameter.AppendSql("     , A.CYCLE_PROCS_WEM_RESULT");
            parameter.AppendSql("     , A.CYCLE_CRNGN_RDMTR_OVER_YN");
            parameter.AppendSql("     , A.CYCLE_CHMCLS_RDMTR_OVER_YN");
            parameter.AppendSql("     , A.CYCLE_FUTR_WEM_CYCLE");
            parameter.AppendSql("     , A.CYCLE_FUTR_WEM_PLAN_DATE");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.WRTNO =:WRTNO ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReaderSingle<HIC_CHUKMST_NEW>(parameter);
        }

        public int GetChkCountByLtdCode(long lTDCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(A.LTDCODE) FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.LTDCODE =:LTDCODE ");
            parameter.AppendSql("   AND A.DELDATE IS NULL ");
            
            parameter.Add("LTDCODE", lTDCODE);

            return ExecuteScalar<int>(parameter);
        }

        public long GetT5LimitByBangiYear(string bANGI, string cHKYEAR, bool bDel)
        {
            //국고누적
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(A.LTDCODE) FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.BANGI =:BANGI ");
            parameter.AppendSql("   AND A.CHKYEAR =:CHKYEAR ");
            parameter.AppendSql("   AND A.GBSUPPORT ='1' ");
            if (bDel) { parameter.AppendSql("   AND A.DELDATE IS NULL "); }

            parameter.Add("BANGI", bANGI);
            parameter.Add("CHKYEAR", cHKYEAR);

            return ExecuteScalar<long>(parameter);
        }

        public long GetT5AccumByBangiYear(string bANGI, string cHKYEAR, bool bDel)
        {
            //5인이상 누적
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(A.LTDCODE) FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.BANGI =:BANGI ");
            parameter.AppendSql("   AND A.CHKYEAR =:CHKYEAR ");
            parameter.AppendSql("   AND A.INWON >= 5 ");
            if (bDel) { parameter.AppendSql("   AND A.DELDATE IS NULL "); }

            parameter.Add("BANGI", bANGI);
            parameter.Add("CHKYEAR", cHKYEAR);

            return ExecuteScalar<long>(parameter);
        }

        public long GetTotAccumByBangiYear(string bANGI, string cHKYEAR, bool bDel)
        {
            //총누적
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(A.LTDCODE) FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.BANGI =:BANGI ");
            parameter.AppendSql("   AND A.CHKYEAR =:CHKYEAR ");
            if (bDel) { parameter.AppendSql("   AND A.DELDATE IS NULL "); }

            parameter.Add("BANGI", bANGI);
            parameter.Add("CHKYEAR", cHKYEAR);

            return ExecuteScalar<long>(parameter);
        }

        public long GetMaxLtdSeqNoByLtdCode(long lTDCODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(A.LTDSEQNO) FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.LTDCODE =:LTDCODE ");

            parameter.Add("LTDCODE", lTDCODE);

            return ExecuteScalar<long>(parameter);
        }

        public void UpDateEstInfoDel(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKMST_NEW");
            parameter.AppendSql("   SET EST_DATE = '' ");
            parameter.AppendSql("     , GBEST = 'N'");
            parameter.AppendSql("     , EST_SABUN = ''");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO ");

            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public List<HIC_CHUKMST_NEW> GetListEstimateByResultSTS(string strFDate, string strTDate, string strKeyward)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.CHKYEAR");
            parameter.AppendSql("     , DECODE(A.BANGI, '1', '상반기', '하반기') AS BANGI");
            parameter.AppendSql("     , A.LTDCODE");
            parameter.AppendSql("     , A.SDATE");
            parameter.AppendSql("     , A.EDATE");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.LTD_MANAGER");
            parameter.AppendSql("     , A.LTD_GRADE");
            parameter.AppendSql("     , A.LTD_HPHONE");
            parameter.AppendSql("     , A.LTD_EMAIL");
            parameter.AppendSql("     , A.GBSUPPORT");
            parameter.AppendSql("     , A.GBSTS");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.DELSABUN");
            parameter.AppendSql("     , A.BDATE");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LTDNAME");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND A.SDATE >=TO_DATE(:SDATE, 'YYYY-MM-DD') AND A.EDATE <=TO_DATE(:EDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND A.DELDATE IS NULL ");
            
            if (!strKeyward.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LIKE :LTDNAME ");
            }

            parameter.Add("SDATE", strFDate);
            parameter.Add("EDATE", strTDate);

            if (!strKeyward.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("LTDNAME", strKeyward);
            }


            return ExecuteReader<HIC_CHUKMST_NEW>(parameter);
        }

        public void UpDateEstInfo(long nWRTNO, long nSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKMST_NEW");
            parameter.AppendSql("   SET EST_DATE = SYSDATE");
            parameter.AppendSql("     , GBEST = 'Y'");
            parameter.AppendSql("     , EST_SABUN = :EST_SABUN");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO ");

            parameter.Add("EST_SABUN", nSabun);
            parameter.Add("WRTNO", nWRTNO);

            ExecuteNonQuery(parameter);
        }

        public long GetInWonByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.INWON FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE A.WRTNO =:WRTNO ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_CHUKMST_NEW> GetListEstimate(string strGbn, string strFDate, string strTDate, string strKeyward)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.CHKYEAR");
            parameter.AppendSql("     , DECODE(A.BANGI, '1', '상반기', '하반기') AS BANGI");
            parameter.AppendSql("     , A.LTDCODE");
            parameter.AppendSql("     , A.SDATE");
            parameter.AppendSql("     , A.EDATE");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.T_LIMIT");
            parameter.AppendSql("     , A.T5_LIMIT");
            parameter.AppendSql("     , A.TO_ACCUM");
            parameter.AppendSql("     , A.T5_ACCUM");
            parameter.AppendSql("     , A.LTD_MANAGER");
            parameter.AppendSql("     , A.LTD_GRADE");
            parameter.AppendSql("     , A.LTD_HPHONE");
            parameter.AppendSql("     , A.LTD_EMAIL");
            parameter.AppendSql("     , A.GBSUPPORT");
            parameter.AppendSql("     , A.GBSTS");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.DELSABUN");
            parameter.AppendSql("     , A.LTD_FAX");
            parameter.AppendSql("     , A.GBNEW");
            parameter.AppendSql("     , A.GBWAY");
            parameter.AppendSql("     , A.ILSU");
            parameter.AppendSql("     , A.BDATE");
            parameter.AppendSql("     , A.INWON");
            parameter.AppendSql("     , A.INWON_S");
            parameter.AppendSql("     , A.INWON_H");
            parameter.AppendSql("     , A.GBDAY");
            parameter.AppendSql("     , A.GBSHIFT");
            parameter.AppendSql("     , A.DAYTIME");
            parameter.AppendSql("     , A.SHIFTGRPCNT");
            parameter.AppendSql("     , A.SHIFTQUARTER");
            parameter.AppendSql("     , A.SHIFTTIME");
            parameter.AppendSql("     , A.WORKTIME1");
            parameter.AppendSql("     , A.WORKTIME2");
            parameter.AppendSql("     , A.WORKTIME3");
            parameter.AppendSql("     , A.WORKTIME4");
            parameter.AppendSql("     , A.MEALTIME1");
            parameter.AppendSql("     , A.MEALTIME2");
            parameter.AppendSql("     , A.WORKTIME11");
            parameter.AppendSql("     , A.WORKTIME22");
            parameter.AppendSql("     , A.WORKTIME33");
            parameter.AppendSql("     , A.WORKTIME44");
            parameter.AppendSql("     , A.MEALTIME11");
            parameter.AppendSql("     , A.MEALTIME22");
            parameter.AppendSql("     , A.GBOVERTIME");
            parameter.AppendSql("     , A.OVERTIME");
            parameter.AppendSql("     , A.GBESTIMATE");
            parameter.AppendSql("     , A.GBCORRECT");
            parameter.AppendSql("     , A.GBSAMPLE");
            parameter.AppendSql("     , A.GBUCODE1");
            parameter.AppendSql("     , A.GBUCODE2");
            parameter.AppendSql("     , A.GBUCODE3");
            parameter.AppendSql("     , A.GBUCODE4");
            parameter.AppendSql("     , A.GBUCODE5");
            parameter.AppendSql("     , A.GBUCODE6");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.GBCHROMIUM");
            parameter.AppendSql("     , A.GBEST");
            parameter.AppendSql("     , A.EST_DATE");
            parameter.AppendSql("     , A.EST_SABUN");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.EST_SABUN) EST_JOBNAME");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LTDNAME");
            parameter.AppendSql("     , A.GBTEMP");
            parameter.AppendSql("     , A.LTDSEQNO");
            parameter.AppendSql("     , A.LTDGONGNAME");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE 1 = 1 ");
            parameter.AppendSql("   AND A.BDATE >=TO_DATE(:SDATE, 'YYYY-MM-DD') AND A.BDATE <=TO_DATE(:EDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND A.DELDATE IS NULL ");
            parameter.AppendSql("   AND A.GBEST =:GBEST ");
            if (!strKeyward.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LIKE :LTDNAME ");
            }

            parameter.Add("SDATE", strFDate);
            parameter.Add("EDATE", strTDate);

            if (!strKeyward.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("LTDNAME", strKeyward);
            }

            parameter.Add("GBEST", strGbn);

            return ExecuteReader<HIC_CHUKMST_NEW>(parameter);
        }

        public List<HIC_CHUKMST_NEW> GetListByDateGubun(string argStartDate, string argLastDate, string argGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.CHKYEAR");
            parameter.AppendSql("     , A.BANGI");
            parameter.AppendSql("     , A.LTDCODE");
            parameter.AppendSql("     , A.SDATE");
            parameter.AppendSql("     , A.EDATE");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , A.T_LIMIT");
            parameter.AppendSql("     , A.T5_LIMIT");
            parameter.AppendSql("     , A.TO_ACCUM");
            parameter.AppendSql("     , A.T5_ACCUM");
            parameter.AppendSql("     , A.LTD_MANAGER");
            parameter.AppendSql("     , A.LTD_GRADE");
            parameter.AppendSql("     , A.LTD_HPHONE");
            parameter.AppendSql("     , A.LTD_EMAIL");
            parameter.AppendSql("     , A.GBSUPPORT");
            parameter.AppendSql("     , A.GBSTS");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.DELSABUN");
            parameter.AppendSql("     , A.LTD_FAX");
            parameter.AppendSql("     , A.GBNEW");
            parameter.AppendSql("     , A.GBWAY");
            parameter.AppendSql("     , A.ILSU");
            parameter.AppendSql("     , A.BDATE");
            parameter.AppendSql("     , A.INWON");
            parameter.AppendSql("     , A.INWON_S");
            parameter.AppendSql("     , A.INWON_H");
            parameter.AppendSql("     , A.GBDAY");
            parameter.AppendSql("     , A.GBSHIFT");
            parameter.AppendSql("     , A.DAYTIME");
            parameter.AppendSql("     , A.SHIFTGRPCNT");
            parameter.AppendSql("     , A.SHIFTQUARTER");
            parameter.AppendSql("     , A.SHIFTTIME");
            parameter.AppendSql("     , A.WORKTIME1");
            parameter.AppendSql("     , A.WORKTIME2");
            parameter.AppendSql("     , A.WORKTIME3");
            parameter.AppendSql("     , A.WORKTIME4");
            parameter.AppendSql("     , A.MEALTIME1");
            parameter.AppendSql("     , A.MEALTIME2");
            parameter.AppendSql("     , A.WORKTIME11");
            parameter.AppendSql("     , A.WORKTIME22");
            parameter.AppendSql("     , A.WORKTIME33");
            parameter.AppendSql("     , A.WORKTIME44");
            parameter.AppendSql("     , A.MEALTIME11");
            parameter.AppendSql("     , A.MEALTIME22");
            parameter.AppendSql("     , A.GBOVERTIME");
            parameter.AppendSql("     , A.OVERTIME");
            parameter.AppendSql("     , A.GBESTIMATE");
            parameter.AppendSql("     , A.GBCORRECT");
            parameter.AppendSql("     , A.GBSAMPLE");
            parameter.AppendSql("     , A.GBUCODE1");
            parameter.AppendSql("     , A.GBUCODE2");
            parameter.AppendSql("     , A.GBUCODE3");
            parameter.AppendSql("     , A.GBUCODE4");
            parameter.AppendSql("     , A.GBUCODE5");
            parameter.AppendSql("     , A.GBUCODE6");
            parameter.AppendSql("     , A.REMARK");
            parameter.AppendSql("     , A.GBCHROMIUM");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LTDNAME");
            parameter.AppendSql("     , A.GBEST");
            parameter.AppendSql("     , A.EST_DATE");
            parameter.AppendSql("     , A.EST_SABUN");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.EST_SABUN) EST_JOBNAME");
            parameter.AppendSql("     , A.GBTEMP");
            parameter.AppendSql("     , A.LTDSEQNO");
            parameter.AppendSql("     , A.LTDGONGNAME");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE 1 = 1 ");
            if (argGubun == "1")
            {
                parameter.AppendSql("    AND A.SDATE >=TO_DATE(:SDATE, 'YYYY-MM-DD') AND A.EDATE <=TO_DATE(:EDATE, 'YYYY-MM-DD') ");
            }
            else
            {
                parameter.AppendSql("   AND A.CHK_DATE >=TO_DATE(:BFDATE, 'YYYY-MM-DD') AND A.CHK_DATE <=TO_DATE(:BTDATE, 'YYYY-MM-DD')  ");
            }

            if (argGubun == "1")
            {
                parameter.Add("SDATE", argStartDate);
                parameter.Add("EDATE", argLastDate);
            }
            else
            {
                parameter.Add("BFDATE", argStartDate);
                parameter.Add("BTDATE", argLastDate);
            }

            return ExecuteReader<HIC_CHUKMST_NEW>(parameter);
        }

        public List<HIC_CHUKMST_NEW> GetItemAll(string strKeyWard, bool bDel, string strBangi, string strGjYear)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO");
            parameter.AppendSql("     , A.CHKYEAR");
            parameter.AppendSql("     , A.GBSTS");
            parameter.AppendSql("     , DECODE(A.GBSTS, '0', '예비조사대상', '2', '예비조사완료', '3', '환경측정중', '5','환경측정완료', 'D', '삭제', '') GBSTSNAME");
            parameter.AppendSql("     , DECODE(A.BANGI, '1', '상반기', '2', '하반기', '') BANGI");
            parameter.AppendSql("     , A.LTDCODE");
            parameter.AppendSql("     , ADMIN.FC_HIC_LTDNAME(A.LTDCODE) LTDNAME"); 
            parameter.AppendSql("     , A.SDATE ");
            parameter.AppendSql("     , A.EDATE ");
            parameter.AppendSql("     , A.ENTDATE");
            parameter.AppendSql("     , A.ENTSABUN");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.ENTSABUN) ENTNAME");
            parameter.AppendSql("     , A.T_LIMIT");
            parameter.AppendSql("     , A.T5_LIMIT");
            parameter.AppendSql("     , A.TO_ACCUM");
            parameter.AppendSql("     , A.T5_ACCUM");
            parameter.AppendSql("     , A.LTD_MANAGER");
            parameter.AppendSql("     , A.LTD_GRADE");
            parameter.AppendSql("     , A.LTD_HPHONE");
            parameter.AppendSql("     , A.LTD_EMAIL");
            parameter.AppendSql("     , DECODE(A.GBSUPPORT, '1', '적용', '미적용') GBSUPPORT");
            parameter.AppendSql("     , A.GBSTS");
            parameter.AppendSql("     , A.DELDATE");
            parameter.AppendSql("     , A.DELSABUN");
            parameter.AppendSql("     , A.GBEST");
            parameter.AppendSql("     , A.EST_DATE");
            parameter.AppendSql("     , A.EST_SABUN");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.EST_SABUN) EST_JOBNAME");
            parameter.AppendSql("     , ADMIN.FC_INSA_MST_KORNAME(A.DELSABUN) DELNAME");
            parameter.AppendSql("     , A.GBTEMP");
            parameter.AppendSql("     , A.LTDSEQNO");
            parameter.AppendSql("     , A.LTDGONGNAME");
            parameter.AppendSql("     , A.ROWID AS RID");
            parameter.AppendSql("  FROM ADMIN.HIC_CHUKMST_NEW A");
            parameter.AppendSql(" WHERE 1 = 1 ");
            if (!bDel) { parameter.AppendSql("   AND DELDATE IS NULL"); }
            if (!strBangi.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.BANGI =:BANGI ");
            }

            if (!strGjYear.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND A.CHKYEAR =:CHKYEAR ");
            }

            if (!strBangi.IsNullOrEmpty())
            {
                parameter.Add("BANGI", strBangi);
            }
            
            parameter.Add("CHKYEAR", strGjYear);

            return ExecuteReader<HIC_CHUKMST_NEW>(parameter);
        }

        public void InSert(HIC_CHUKMST_NEW dto)
        {
            long id =  GetSequenceNextVal("ADMIN.HIC_WEM_ID_SITE_MANAGER_SEQ");
            dto.WRTNO = id;

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO ADMIN.HIC_CHUKMST_NEW ");
            parameter.AppendSql("(");
            parameter.AppendSql("    WRTNO");
            parameter.AppendSql("  , CHKYEAR");
            parameter.AppendSql("  , BANGI");
            parameter.AppendSql("  , LTDCODE");
            parameter.AppendSql("  , SDATE");
            parameter.AppendSql("  , EDATE");
            parameter.AppendSql("  , ENTDATE");
            parameter.AppendSql("  , ENTSABUN");
            parameter.AppendSql("  , T_LIMIT");
            parameter.AppendSql("  , T5_LIMIT");
            parameter.AppendSql("  , TO_ACCUM");
            parameter.AppendSql("  , T5_ACCUM");
            parameter.AppendSql("  , LTD_MANAGER");
            parameter.AppendSql("  , LTD_GRADE");
            parameter.AppendSql("  , LTD_HPHONE");
            parameter.AppendSql("  , LTD_EMAIL");
            parameter.AppendSql("  , GBSUPPORT");
            parameter.AppendSql("  , GBSTS");
            parameter.AppendSql("  , LTD_FAX");
            parameter.AppendSql("  , GBNEW");
            parameter.AppendSql("  , GBWAY");
            parameter.AppendSql("  , ILSU");
            parameter.AppendSql("  , BDATE");
            parameter.AppendSql("  , INWON");
            parameter.AppendSql("  , INWON_S");
            parameter.AppendSql("  , INWON_H");
            parameter.AppendSql("  , GBDAY");
            parameter.AppendSql("  , GBSHIFT");
            parameter.AppendSql("  , DAYTIME");
            parameter.AppendSql("  , SHIFTGRPCNT");
            parameter.AppendSql("  , SHIFTQUARTER");
            parameter.AppendSql("  , SHIFTTIME");
            parameter.AppendSql("  , WORKTIME1");
            parameter.AppendSql("  , WORKTIME2");
            parameter.AppendSql("  , WORKTIME3");
            parameter.AppendSql("  , WORKTIME4");
            parameter.AppendSql("  , MEALTIME1");
            parameter.AppendSql("  , MEALTIME2");
            parameter.AppendSql("  , WORKTIME11");
            parameter.AppendSql("  , WORKTIME22");
            parameter.AppendSql("  , WORKTIME33");
            parameter.AppendSql("  , WORKTIME44");
            parameter.AppendSql("  , MEALTIME11");
            parameter.AppendSql("  , MEALTIME22");
            parameter.AppendSql("  , GBOVERTIME");
            parameter.AppendSql("  , OVERTIME");
            parameter.AppendSql("  , GBESTIMATE");
            parameter.AppendSql("  , GBCORRECT");
            parameter.AppendSql("  , GBSAMPLE");
            parameter.AppendSql("  , GBUCODE1");
            parameter.AppendSql("  , GBUCODE2");
            parameter.AppendSql("  , GBUCODE3");
            parameter.AppendSql("  , GBUCODE4");
            parameter.AppendSql("  , GBUCODE5");
            parameter.AppendSql("  , GBUCODE6");
            parameter.AppendSql("  , REMARK");
            parameter.AppendSql("  , GBCHROMIUM");
            parameter.AppendSql("  , GBEST");
            parameter.AppendSql("  , GBTEMP");
            parameter.AppendSql("  , LTDSEQNO");
            parameter.AppendSql("  , LTDGONGNAME");
            parameter.AppendSql("  , CYCLE_PROCS_NEW_CHANGE_YN");
            parameter.AppendSql("  , CYCLE_PROCS_NEW_CHANGE_DATE");
            parameter.AppendSql("  , CYCLE_PROCS_WEM_RESULT");
            parameter.AppendSql("  , CYCLE_CRNGN_RDMTR_OVER_YN");
            parameter.AppendSql("  , CYCLE_CHMCLS_RDMTR_OVER_YN");
            parameter.AppendSql("  , CYCLE_FUTR_WEM_CYCLE");
            parameter.AppendSql("  , CYCLE_FUTR_WEM_PLAN_DATE");
            parameter.AppendSql(") VALUES ( ");
            parameter.AppendSql("    :WRTNO");
            parameter.AppendSql("  , :CHKYEAR");
            parameter.AppendSql("  , :BANGI");
            parameter.AppendSql("  , :LTDCODE");
            parameter.AppendSql("  , TO_DATE(:SDATE, 'YYYY-MM-DD')");
            parameter.AppendSql("  , TO_DATE(:EDATE, 'YYYY-MM-DD')");
            parameter.AppendSql("  , SYSDATE");
            parameter.AppendSql("  , :ENTSABUN");
            parameter.AppendSql("  , :T_LIMIT");
            parameter.AppendSql("  , :T5_LIMIT");
            parameter.AppendSql("  , :TO_ACCUM");
            parameter.AppendSql("  , :T5_ACCUM");
            parameter.AppendSql("  , :LTD_MANAGER");
            parameter.AppendSql("  , :LTD_GRADE");
            parameter.AppendSql("  , :LTD_HPHONE");
            parameter.AppendSql("  , :LTD_EMAIL");
            parameter.AppendSql("  , :GBSUPPORT");
            parameter.AppendSql("  , :GBSTS");
            parameter.AppendSql("  , :LTD_FAX");
            parameter.AppendSql("  , :GBNEW");
            parameter.AppendSql("  , :GBWAY");
            parameter.AppendSql("  , :ILSU");
            parameter.AppendSql("  , TO_DATE(:BDATE, 'YYYY-MM-DD')");
            parameter.AppendSql("  , :INWON");
            parameter.AppendSql("  , :INWON_S");
            parameter.AppendSql("  , :INWON_H");
            parameter.AppendSql("  , :GBDAY");
            parameter.AppendSql("  , :GBSHIFT");
            parameter.AppendSql("  , :DAYTIME");
            parameter.AppendSql("  , :SHIFTGRPCNT");
            parameter.AppendSql("  , :SHIFTQUARTER");
            parameter.AppendSql("  , :SHIFTTIME");
            parameter.AppendSql("  , :WORKTIME1");
            parameter.AppendSql("  , :WORKTIME2");
            parameter.AppendSql("  , :WORKTIME3");
            parameter.AppendSql("  , :WORKTIME4");
            parameter.AppendSql("  , :MEALTIME1");
            parameter.AppendSql("  , :MEALTIME2");
            parameter.AppendSql("  , :WORKTIME11");
            parameter.AppendSql("  , :WORKTIME22");
            parameter.AppendSql("  , :WORKTIME33");
            parameter.AppendSql("  , :WORKTIME44");
            parameter.AppendSql("  , :MEALTIME11");
            parameter.AppendSql("  , :MEALTIME22");
            parameter.AppendSql("  , :GBOVERTIME");
            parameter.AppendSql("  , :OVERTIME");
            parameter.AppendSql("  , :GBESTIMATE");
            parameter.AppendSql("  , :GBCORRECT");
            parameter.AppendSql("  , :GBSAMPLE");
            parameter.AppendSql("  , :GBUCODE1");
            parameter.AppendSql("  , :GBUCODE2");
            parameter.AppendSql("  , :GBUCODE3");
            parameter.AppendSql("  , :GBUCODE4");
            parameter.AppendSql("  , :GBUCODE5");
            parameter.AppendSql("  , :GBUCODE6");
            parameter.AppendSql("  , :REMARK");
            parameter.AppendSql("  , :GBCHROMIUM");
            parameter.AppendSql("  , :GBEST");
            parameter.AppendSql("  , :GBTEMP");
            parameter.AppendSql("  , :LTDSEQNO");
            parameter.AppendSql("  , :LTDGONGNAME");
            parameter.AppendSql("  , :CYCLE_PROCS_NEW_CHANGE_YN");
            parameter.AppendSql("  , :CYCLE_PROCS_NEW_CHANGE_DATE");
            parameter.AppendSql("  , :CYCLE_PROCS_WEM_RESULT");
            parameter.AppendSql("  , :CYCLE_CRNGN_RDMTR_OVER_YN");
            parameter.AppendSql("  , :CYCLE_CHMCLS_RDMTR_OVER_YN");
            parameter.AppendSql("  , :CYCLE_FUTR_WEM_CYCLE");
            parameter.AppendSql("  , :CYCLE_FUTR_WEM_PLAN_DATE");
            parameter.AppendSql(") ");

            parameter.Add("WRTNO",          dto.WRTNO);
            parameter.Add("CHKYEAR",        dto.CHKYEAR);
            parameter.Add("BANGI",          dto.BANGI);
            parameter.Add("LTDCODE",        dto.LTDCODE);
            parameter.Add("SDATE",          dto.SDATE);
            parameter.Add("EDATE",          dto.EDATE);
            parameter.Add("ENTSABUN",       dto.ENTSABUN);
            parameter.Add("T_LIMIT",        dto.T_LIMIT);
            parameter.Add("T5_LIMIT",       dto.T5_LIMIT);
            parameter.Add("TO_ACCUM",       dto.TO_ACCUM);
            parameter.Add("T5_ACCUM",       dto.T5_ACCUM);
            parameter.Add("LTD_MANAGER",    dto.LTD_MANAGER);
            parameter.Add("LTD_GRADE",      dto.LTD_GRADE);
            parameter.Add("LTD_HPHONE",     dto.LTD_HPHONE);
            parameter.Add("LTD_EMAIL",      dto.LTD_EMAIL);
            parameter.Add("GBSUPPORT",      dto.GBSUPPORT);
            parameter.Add("GBSTS",          dto.GBSTS);
            parameter.Add("LTD_FAX",        dto.LTD_FAX);
            parameter.Add("GBNEW",          dto.GBNEW);
            parameter.Add("GBWAY",          dto.GBWAY);
            parameter.Add("ILSU",           dto.ILSU);
            parameter.Add("BDATE",          dto.BDATE);
            parameter.Add("INWON",          dto.INWON);
            parameter.Add("INWON_S",        dto.INWON_S);
            parameter.Add("INWON_H",        dto.INWON_H);
            parameter.Add("GBDAY",          dto.GBDAY);
            parameter.Add("GBSHIFT",        dto.GBSHIFT);
            parameter.Add("DAYTIME",        dto.DAYTIME);
            parameter.Add("SHIFTGRPCNT",    dto.SHIFTGRPCNT);
            parameter.Add("SHIFTQUARTER",   dto.SHIFTQUARTER);
            parameter.Add("SHIFTTIME",      dto.SHIFTTIME);
            parameter.Add("WORKTIME1",      dto.WORKTIME1);
            parameter.Add("WORKTIME2",      dto.WORKTIME2);
            parameter.Add("WORKTIME3",      dto.WORKTIME3);
            parameter.Add("WORKTIME4",      dto.WORKTIME4);
            parameter.Add("MEALTIME1",      dto.MEALTIME1);
            parameter.Add("MEALTIME2",      dto.MEALTIME2);
            parameter.Add("WORKTIME11",     dto.WORKTIME11);
            parameter.Add("WORKTIME22",     dto.WORKTIME22);
            parameter.Add("WORKTIME33",     dto.WORKTIME33);
            parameter.Add("WORKTIME44",     dto.WORKTIME44);
            parameter.Add("MEALTIME11",     dto.MEALTIME11);
            parameter.Add("MEALTIME22",     dto.MEALTIME22);
            parameter.Add("GBOVERTIME",     dto.GBOVERTIME);
            parameter.Add("OVERTIME",       dto.OVERTIME);
            parameter.Add("GBESTIMATE",     dto.GBESTIMATE);
            parameter.Add("GBCORRECT",      dto.GBCORRECT);
            parameter.Add("GBSAMPLE",       dto.GBSAMPLE);
            parameter.Add("GBUCODE1",       dto.GBUCODE1);
            parameter.Add("GBUCODE2",       dto.GBUCODE2);
            parameter.Add("GBUCODE3",       dto.GBUCODE3);
            parameter.Add("GBUCODE4",       dto.GBUCODE4);
            parameter.Add("GBUCODE5",       dto.GBUCODE5);
            parameter.Add("GBUCODE6",       dto.GBUCODE6);
            parameter.Add("REMARK",         dto.REMARK);
            parameter.Add("GBCHROMIUM",     dto.GBCHROMIUM);
            parameter.Add("GBEST",          dto.GBEST);
            parameter.Add("GBTEMP",         dto.GBTEMP);
            parameter.Add("LTDSEQNO",       dto.LTDSEQNO);
            parameter.Add("LTDGONGNAME",    dto.LTDGONGNAME);
            parameter.Add("CYCLE_PROCS_NEW_CHANGE_YN", dto.CYCLE_PROCS_NEW_CHANGE_YN);
            parameter.Add("CYCLE_PROCS_NEW_CHANGE_DATE", dto.CYCLE_PROCS_NEW_CHANGE_DATE);
            parameter.Add("CYCLE_PROCS_WEM_RESULT", dto.CYCLE_PROCS_WEM_RESULT);
            parameter.Add("CYCLE_CRNGN_RDMTR_OVER_YN", dto.CYCLE_CRNGN_RDMTR_OVER_YN);
            parameter.Add("CYCLE_CHMCLS_RDMTR_OVER_YN", dto.CYCLE_CHMCLS_RDMTR_OVER_YN);
            parameter.Add("CYCLE_FUTR_WEM_CYCLE", dto.CYCLE_FUTR_WEM_CYCLE);
            parameter.Add("CYCLE_FUTR_WEM_PLAN_DATE", dto.CYCLE_FUTR_WEM_PLAN_DATE);


            ExecuteNonQuery(parameter);
        }

        public void UpDate(HIC_CHUKMST_NEW dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKMST_NEW");
            parameter.AppendSql("   SET CHKYEAR = :CHKYEAR");
            parameter.AppendSql("     , BANGI = :BANGI");
            parameter.AppendSql("     , LTDCODE = :LTDCODE");
            parameter.AppendSql("     , SDATE = TO_DATE(:SDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("     , EDATE = TO_DATE(:EDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("     , ENTDATE = SYSDATE");
            parameter.AppendSql("     , ENTSABUN = :ENTSABUN");
            parameter.AppendSql("     , T_LIMIT = :T_LIMIT");
            parameter.AppendSql("     , T5_LIMIT = :T5_LIMIT");
            parameter.AppendSql("     , TO_ACCUM = :TO_ACCUM");
            parameter.AppendSql("     , T5_ACCUM = :T5_ACCUM");
            parameter.AppendSql("     , LTD_MANAGER = :LTD_MANAGER");
            parameter.AppendSql("     , LTD_GRADE = :LTD_GRADE");
            parameter.AppendSql("     , LTD_HPHONE = :LTD_HPHONE");
            parameter.AppendSql("     , LTD_EMAIL = :LTD_EMAIL");
            parameter.AppendSql("     , GBSUPPORT = :GBSUPPORT");
            parameter.AppendSql("     , GBSTS = :GBSTS");
            parameter.AppendSql("     , LTD_FAX = :LTD_FAX");
            parameter.AppendSql("     , GBNEW = :GBNEW");
            parameter.AppendSql("     , GBWAY = :GBWAY");
            parameter.AppendSql("     , ILSU = :ILSU");
            parameter.AppendSql("     , BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("     , INWON = :INWON");
            parameter.AppendSql("     , INWON_S = :INWON_S");
            parameter.AppendSql("     , INWON_H = :INWON_H");
            parameter.AppendSql("     , GBDAY = :GBDAY");
            parameter.AppendSql("     , GBSHIFT = :GBSHIFT");
            parameter.AppendSql("     , DAYTIME = :DAYTIME");
            parameter.AppendSql("     , SHIFTGRPCNT = :SHIFTGRPCNT");
            parameter.AppendSql("     , SHIFTQUARTER = :SHIFTQUARTER");
            parameter.AppendSql("     , SHIFTTIME = :SHIFTTIME");
            parameter.AppendSql("     , WORKTIME1 = :WORKTIME1");
            parameter.AppendSql("     , WORKTIME2 = :WORKTIME2");
            parameter.AppendSql("     , WORKTIME3 = :WORKTIME3");
            parameter.AppendSql("     , WORKTIME4 = :WORKTIME4");
            parameter.AppendSql("     , MEALTIME1 = :MEALTIME1");
            parameter.AppendSql("     , MEALTIME2 = :MEALTIME2");
            parameter.AppendSql("     , WORKTIME11 = :WORKTIME11");
            parameter.AppendSql("     , WORKTIME22 = :WORKTIME22");
            parameter.AppendSql("     , WORKTIME33 = :WORKTIME33");
            parameter.AppendSql("     , WORKTIME44 = :WORKTIME44");
            parameter.AppendSql("     , MEALTIME11 = :MEALTIME11");
            parameter.AppendSql("     , MEALTIME22 = :MEALTIME22");
            parameter.AppendSql("     , GBOVERTIME = :GBOVERTIME");
            parameter.AppendSql("     , OVERTIME = :OVERTIME");
            parameter.AppendSql("     , GBESTIMATE = :GBESTIMATE");
            parameter.AppendSql("     , GBCORRECT = :GBCORRECT");
            parameter.AppendSql("     , GBSAMPLE = :GBSAMPLE");
            parameter.AppendSql("     , GBUCODE1 = :GBUCODE1");
            parameter.AppendSql("     , GBUCODE2 = :GBUCODE2");
            parameter.AppendSql("     , GBUCODE3 = :GBUCODE3");
            parameter.AppendSql("     , GBUCODE4 = :GBUCODE4");
            parameter.AppendSql("     , GBUCODE5 = :GBUCODE5");
            parameter.AppendSql("     , GBUCODE6 = :GBUCODE6");
            parameter.AppendSql("     , REMARK = :REMARK");
            parameter.AppendSql("     , GBCHROMIUM = :GBCHROMIUM");
            parameter.AppendSql("     , GBTEMP = :GBTEMP");
            parameter.AppendSql("     , LTDSEQNO = :LTDSEQNO");
            parameter.AppendSql("     , LTDGONGNAME = :LTDGONGNAME");
            parameter.AppendSql("     , CYCLE_PROCS_NEW_CHANGE_YN = :CYCLE_PROCS_NEW_CHANGE_YN");
            parameter.AppendSql("     , CYCLE_PROCS_NEW_CHANGE_DATE = :CYCLE_PROCS_NEW_CHANGE_DATE");
            parameter.AppendSql("     , CYCLE_PROCS_WEM_RESULT = :CYCLE_PROCS_WEM_RESULT");
            parameter.AppendSql("     , CYCLE_CRNGN_RDMTR_OVER_YN = :CYCLE_CRNGN_RDMTR_OVER_YN");
            parameter.AppendSql("     , CYCLE_CHMCLS_RDMTR_OVER_YN = :CYCLE_CHMCLS_RDMTR_OVER_YN");
            parameter.AppendSql("     , CYCLE_FUTR_WEM_CYCLE = :CYCLE_FUTR_WEM_CYCLE");
            parameter.AppendSql("     , CYCLE_FUTR_WEM_PLAN_DATE = :CYCLE_FUTR_WEM_PLAN_DATE");

            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("CHKYEAR", dto.CHKYEAR);
            parameter.Add("BANGI", dto.BANGI);
            parameter.Add("LTDCODE", dto.LTDCODE);
            parameter.Add("SDATE", dto.SDATE);
            parameter.Add("EDATE", dto.EDATE);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("T_LIMIT", dto.T_LIMIT);
            parameter.Add("T5_LIMIT", dto.T5_LIMIT);
            parameter.Add("TO_ACCUM", dto.TO_ACCUM);
            parameter.Add("T5_ACCUM", dto.T5_ACCUM);
            parameter.Add("LTD_MANAGER", dto.LTD_MANAGER);
            parameter.Add("LTD_GRADE", dto.LTD_GRADE);
            parameter.Add("LTD_HPHONE", dto.LTD_HPHONE);
            parameter.Add("LTD_EMAIL", dto.LTD_EMAIL);
            parameter.Add("GBSUPPORT", dto.GBSUPPORT);
            parameter.Add("GBSTS", dto.GBSTS);
            parameter.Add("LTD_FAX", dto.LTD_FAX);
            parameter.Add("GBNEW", dto.GBNEW);
            parameter.Add("GBWAY", dto.GBWAY);
            parameter.Add("ILSU", dto.ILSU);
            parameter.Add("BDATE", dto.BDATE);
            parameter.Add("INWON", dto.INWON);
            parameter.Add("INWON_S", dto.INWON_S);
            parameter.Add("INWON_H", dto.INWON_H);
            parameter.Add("GBDAY", dto.GBDAY);
            parameter.Add("GBSHIFT", dto.GBSHIFT);
            parameter.Add("DAYTIME", dto.DAYTIME);
            parameter.Add("SHIFTGRPCNT", dto.SHIFTGRPCNT);
            parameter.Add("SHIFTQUARTER", dto.SHIFTQUARTER);
            parameter.Add("SHIFTTIME", dto.SHIFTTIME);
            parameter.Add("WORKTIME1", dto.WORKTIME1);
            parameter.Add("WORKTIME2", dto.WORKTIME2);
            parameter.Add("WORKTIME3", dto.WORKTIME3);
            parameter.Add("WORKTIME4", dto.WORKTIME4);
            parameter.Add("MEALTIME1", dto.MEALTIME1);
            parameter.Add("MEALTIME2", dto.MEALTIME2);
            parameter.Add("WORKTIME11", dto.WORKTIME11);
            parameter.Add("WORKTIME22", dto.WORKTIME22);
            parameter.Add("WORKTIME33", dto.WORKTIME33);
            parameter.Add("WORKTIME44", dto.WORKTIME44);
            parameter.Add("MEALTIME11", dto.MEALTIME11);
            parameter.Add("MEALTIME22", dto.MEALTIME22);
            parameter.Add("GBOVERTIME", dto.GBOVERTIME);
            parameter.Add("OVERTIME", dto.OVERTIME);
            parameter.Add("GBESTIMATE", dto.GBESTIMATE);
            parameter.Add("GBCORRECT", dto.GBCORRECT);
            parameter.Add("GBSAMPLE", dto.GBSAMPLE);
            parameter.Add("GBUCODE1", dto.GBUCODE1);
            parameter.Add("GBUCODE2", dto.GBUCODE2);
            parameter.Add("GBUCODE3", dto.GBUCODE3);
            parameter.Add("GBUCODE4", dto.GBUCODE4);
            parameter.Add("GBUCODE5", dto.GBUCODE5);
            parameter.Add("GBUCODE6", dto.GBUCODE6);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("GBCHROMIUM", dto.GBCHROMIUM);
            parameter.Add("GBTEMP", dto.GBTEMP);
            parameter.Add("LTDSEQNO", dto.LTDSEQNO);
            parameter.Add("LTDGONGNAME", dto.LTDGONGNAME);
            parameter.Add("CYCLE_PROCS_NEW_CHANGE_YN", dto.CYCLE_PROCS_NEW_CHANGE_YN);
            parameter.Add("CYCLE_PROCS_NEW_CHANGE_DATE", dto.CYCLE_PROCS_NEW_CHANGE_DATE);
            parameter.Add("CYCLE_PROCS_WEM_RESULT", dto.CYCLE_PROCS_WEM_RESULT);
            parameter.Add("CYCLE_CRNGN_RDMTR_OVER_YN", dto.CYCLE_CRNGN_RDMTR_OVER_YN);
            parameter.Add("CYCLE_CHMCLS_RDMTR_OVER_YN", dto.CYCLE_CHMCLS_RDMTR_OVER_YN);
            parameter.Add("CYCLE_FUTR_WEM_CYCLE", dto.CYCLE_FUTR_WEM_CYCLE);
            parameter.Add("CYCLE_FUTR_WEM_PLAN_DATE", dto.CYCLE_FUTR_WEM_PLAN_DATE);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }

        public void Delete(HIC_CHUKMST_NEW dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("UPDATE ADMIN.HIC_CHUKMST_NEW");
            parameter.AppendSql("   SET DELDATE = SYSDATE");
            parameter.AppendSql("     , DELSABUN = :DELSABUN");
            parameter.AppendSql(" WHERE ROWID = :RID ");

            parameter.Add("DELSABUN", dto.DELSABUN);
            parameter.Add("RID", dto.RID);

            ExecuteNonQuery(parameter);
        }
    }
}
