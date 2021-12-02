namespace HC_Main.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using HC_Main.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HeaJepsuResvExamRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuResvExamRepository()
        {
        }

        public List<HEA_JEPSU_RESV_EXAM> GetListBySDateCfmYN(string argFDate, string argTDate, int argCFM, string argSName, int nSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT TO_CHAR(b.RTIME, 'YYYY-MM-DD') RTIME, a.WRTNO, a.PANO, a.SNAME, b.GBEXAM, b.EXAMNAME   ");
            parameter.AppendSql("       ,TO_CHAR(b.TONGBODATE,'YYYY-MM-DD') TONGBODATE, b.TONGBOSABUN                           ");
            parameter.AppendSql("       ,KOSMOS_OCS.FC_INSA_MST_KORNAME(b.TONGBOSABUN) TONGBONAME,b.CONFIRM                     ");
            parameter.AppendSql("       ,TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, b.ENTSABUN , b.ENTTIME, b.EXCODE                  ");
            parameter.AppendSql("       ,a.GJJONG, a.AGE || '/' || a.SEX AS A_SEX, b.ROWID AS RID, a.PTNO                       ");
            parameter.AppendSql("       ,KOSMOS_PMPA.FC_HC_PATIENT_HPHONE(a.PTNO) AS HPHONE                                     ");
            parameter.AppendSql("   FROM KOSMOS_PMPA.HEA_JEPSU a                                                                ");
            parameter.AppendSql("       ,KOSMOS_PMPA.HEA_RESV_EXAM b                                                            ");
            parameter.AppendSql("  WHERE a.SDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                               ");
            parameter.AppendSql("    AND a.SDATE <  TO_DATE(:TDATE, 'YYYY-MM-DD')                                               ");
            parameter.AppendSql("    AND b.PANO = a.PANO                                                                        ");
            parameter.AppendSql("    AND a.SDATE = b.SDATE                                                                      ");
            parameter.AppendSql("    AND a.DELDATE IS NULL                                                                      ");
            parameter.AppendSql("    AND b.DELDATE IS NULL                                                                      ");
            parameter.AppendSql("    AND a.SDATE != TRUNC(b.RTIME)                                                              ");
            if (argSName != "")
            {
                parameter.AppendSql("    AND a.SNAME LIKE :SNAME                                                                ");
            }

            if (argCFM == 1)
            {
                parameter.AppendSql("    AND b.CONFIRM = '1' ");
                
            }
            else if (argCFM == 2)
            {
                parameter.AppendSql("    AND (b.CONFIRM = '0' OR b.CONFIRM IS NULL) ");
            }

            if (nSort == 1)
            {
                parameter.AppendSql("     ORDER BY b.RTIME, a.WRTNO, a.SDATE ");
            }
            else
            {
                parameter.AppendSql("     ORDER BY a.SDATE, a.WRTNO, b.RTIME ");
            }

            parameter.Add("FDATE", argFDate);
            parameter.Add("TDATE", argTDate);

            if (argSName != "")
            {
                parameter.AddLikeStatement("SNAME", argSName);
            }

            return ExecuteReader<HEA_JEPSU_RESV_EXAM>(parameter);
        }


        public void Insert(HEA_JEPSU_RESV_EXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HEA_RESV_EXAM (                        ");
            parameter.AppendSql("       RTIME,PANO,SNAME,GBEXAM,EXAMNAME,SDATE                  ");
            parameter.AppendSql("      ,ENTSABUN,ENTTIME,EXCODE,AMPM                            ");
            parameter.AppendSql(" ) VALUES (                                                    ");
            parameter.AppendSql("       TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI'),:PANO             ");
            parameter.AppendSql("      ,:SNAME,:GBEXAM,:EXAMNAME,TO_DATE(:SDATE, 'YYYY-MM-DD')  ");
            parameter.AppendSql("      ,:ENTSABUN,SYSDATE,:EXCODE,:AMPM                         ");
            parameter.AppendSql(" )                                                             ");

            parameter.Add("RTIME", code.RTIME);
            parameter.Add("PANO", code.PANO);
            parameter.Add("SNAME", code.SNAME);
            parameter.Add("GBEXAM", code.GBEXAM);
            parameter.Add("EXAMNAME", code.EXAMNAME);
            parameter.Add("SDATE", code.SDATE);
            parameter.Add("ENTSABUN", code.ENTSABUN);
            parameter.Add("EXCODE", code.EXCODE);
            parameter.Add("AMPM", code.AMPM);

            ExecuteNonQuery(parameter);
        }



        public void Update(HEA_JEPSU_RESV_EXAM code)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HEA_RESV_EXAM SET                   ");

            if (!code.TONGBODATE.IsNullOrEmpty() || code.CONFIRM == "Y")
            {
                parameter.AppendSql("  TONGBODATE =TO_DATE(:TONGBODATE,'YYYY-MM-DD')    ");
                parameter.AppendSql(" ,TONGBOSABUN =:TONGBOSABUN                        ");
            }
            else
            {
                parameter.AppendSql("  TONGBODATE = ''                          ");
                parameter.AppendSql(" ,TONGBOSABUN = 0                          ");
            }

            if (code.CONFIRM == "Y")
            {
                parameter.AppendSql("     ,CONFIRM = '1'                        ");
            }
            else
            {
                parameter.AppendSql("     ,CONFIRM = ''                         ");
            }

            //if (!code.RTIME.IsNullOrEmpty()) { parameter.AppendSql(" ,RTIME =TO_DATE(:RTIME, 'YYYY-MM-DD HH24:MI') "); }
            //if (!code.SNAME.IsNullOrEmpty()) { parameter.AppendSql(" ,SNAME =:SNAME     "); }
            //if (!code.AMPM.IsNullOrEmpty()) { parameter.AppendSql(" ,AMPM =:AMPM     "); }

            parameter.AppendSql(" WHERE ROWID =:RID                             ");

            if (!code.TONGBODATE.IsNullOrEmpty() || code.CONFIRM == "Y")
            {
                parameter.Add("TONGBODATE", code.TONGBODATE.Substring(0, 10));
                parameter.Add("TONGBOSABUN", clsType.User.IdNumber.To<long>());
            }

            //if (!code.RTIME.IsNullOrEmpty()) { parameter.Add("RTIME", code.RTIME); }
            //if (!code.SNAME.IsNullOrEmpty()) { parameter.Add("SNAME", code.SNAME); }
            //if (!code.AMPM.IsNullOrEmpty()) { parameter.Add("AMPM", code.AMPM); }

            parameter.Add("RID", code.RID);

            ExecuteNonQuery(parameter);
        }

    }
}
