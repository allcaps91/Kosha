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
    public class HicSpcPanjengRepository : BaseRepository
    {        
        /// <summary>
        /// 
        /// </summary>
        public HicSpcPanjengRepository()
        {
        }

        public string Read_HicCode2_SCode(long argWrtNo, string argSogenCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SAHUCODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO          ");
            parameter.AppendSql("   AND SOGENCODE = :SOGENCODE      ");

            parameter.Add("WRTNO", argWrtNo);
            parameter.Add("SOGENCODE", argSogenCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public IList<HIC_SPC_PANJENG> Read_Spc_Panjeng(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT WRTNO,JEPDATE,LTDCODE,PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE            ");
            parameter.AppendSql("      ,JOCHICODE,WORKYN,SAHUCODE,MSYMCODE,MBUN,UCODE,SOGENREMARK,JOCHIREMARK,REEXAM     ");
            parameter.AppendSql("      ,ENTSABUN,ENTTIME,DELDATE,ORCODE,ORSAYUCODE,SAHUREMARK,GJCHASU,LTDCODE2,PYOJANGGI ");
            parameter.AppendSql("      ,ROWID AS RID                                                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                                      ");
            parameter.AppendSql(" WHERE 1 = 1                                                                            ");
            parameter.AppendSql("   AND WRTNO  =:WRTNO                                                                   ");
            parameter.AppendSql("   AND ( DELDATE IS NULL OR DELDATE ='' )                                               ");

            parameter.Add("WRTNO", argWRTNO);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo(long fnWrtno, long fnWrtno2, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                     ");
            if (strGubun == "")
            {
                parameter.AppendSql("   AND MCode <> 'ZZZ'                                                  ");
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                    ");
            }
            else
            {
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8')                            ");
            }
            parameter.AppendSql("   AND Deldate is Null                                                     ");
            if (fnWrtno2 > 0)
            {
                parameter.AppendSql(" UNION ALL                                                             ");
                parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                     ");
                parameter.AppendSql(" WHERE WRTNO  = :WRTNO2                                                ");
                if (strGubun == "")
                {
                    parameter.AppendSql("   AND MCode <> 'ZZZ'                                              ");
                    parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                ");
                }
                else
                {
                    parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8')                        ");
                }
                parameter.AppendSql("   AND Deldate is Null                                                 ");
            }
            parameter.AppendSql(" GROUP BY  MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
            if (strGubun == "")
            {
                parameter.AppendSql(" ORDER BY 2,1,3,4,5                                                    ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY 1,2,3,4,5                                                    ");
            }

            parameter.Add("WRTNO", fnWrtno);
            if (fnWrtno2 > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public HIC_SPC_PANJENG GetAllbyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE,  WRTNO, JEPDATE     ");
            parameter.AppendSql("     , LTDCODE, PANJENGDRNO, MCODE, PANJENG                                ");
            parameter.AppendSql("     , SOGENCODE, JOCHICODE, WORKYN, SAHUCODE, MSYMCODE, MBUN, UCODE       ");
            parameter.AppendSql("     , SOGENREMARK, JOCHIREMARK, REEXAM, ENTSABUN, ENTTIME, DELDATE        ");
            parameter.AppendSql("     , ORCODE, ORSAYUCODE, SAHUREMARK, GJCHASU, LTDCODE2, PYOJANGGI        ");
            parameter.AppendSql("     , ROWID                                                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                    "); 

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<HIC_SPC_PANJENG>(parameter);
        }

        public void UpDateDelDateByWrtnoMCodeIN(long wRTNO, List<string> lstUCodes_Del)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG      ");
            parameter.AppendSql("   SET DELDATE = TRUNC(SYSDATE)         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                  ");
            parameter.AppendSql("   AND MCODE IN (:MCODE)                ");

            parameter.Add("WRTNO", wRTNO);
            parameter.AddInStatement("MCODE", lstUCodes_Del);

            ExecuteNonQuery(parameter);
        }

        public int UpdatePanjengSogenRemarkbyJepDate(string strFrDate, string strToDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG SET             ");
            parameter.AppendSql("       PANJENG = '8'                               ");
            parameter.AppendSql("     , SOGENREMARK = '기간내 미실시'               ");
            parameter.AppendSql(" WHERE JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')   ");
            parameter.AppendSql("   AND PANJENG = '7'                               ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SPC_PANJENG> GetPanjengbyWrtNoMCodeNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENG     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG             ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND DELDATE IS NULL                         ");
            parameter.AppendSql("   AND MCODE NOT IN ('ZZZ')                    ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public string GetPanjengDatebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG         ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')    ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int GetPanRbyWrtNo(long wRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(Panjeng,'7',1,0)) PAN_R      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG             ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", wRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_SPC_PANJENG GetD1D2bywrtNo(long nWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SUM(DECODE(PANJENG, '5', 1, 0)) D1      ");
            parameter.AppendSql("     , SUM(DECODE(PANJENG, '6', 1, 0)) D2      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG             ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND Panjeng IN ('5','6')                    ");

            parameter.Add("WRTNO", nWrtNo);

            return ExecuteReaderSingle<HIC_SPC_PANJENG>(parameter);
        }

        public void UpdatePanjengInfobyWrtNo(long fnWRTNO, string strDate, long nDrNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG SET                      ");
            if (nDrNO > 0)
            {
                parameter.AppendSql("     PANJENGDATE = TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')   ");
            }
            else
            {
                parameter.AppendSql("     PANJENGDATE = ''   ");
            }

            parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                     ");

            if (nDrNO > 0)
            {
                parameter.Add("PANJENGDATE", strDate);
            }

            parameter.Add("PANJENGDRNO", nDrNO);
            parameter.Add("WRTNO", fnWRTNO);

            ExecuteNonQuery(parameter);
        }

        public string GetRowidByWrtnoMCode(long argWrtno, string argMCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                     ");
            parameter.AppendSql("   AND MCODE  = :MCODE                     ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')    ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("MCODE", argMCode);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateWrtNobyFWrtNo(long nWrtNo, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG SET     ");
            parameter.AppendSql("       WRTNO = :WRTNO                      ");
            parameter.AppendSql(" WHERE WRTNO = :FWRTNO                     ");

            parameter.Add("WRTNO", nWrtNo);
            parameter.Add("FWRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateAllbyWrtNoMCodePyojanggi(string fstrSaveGbn, HIC_SPC_PANJENG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG SET                         ");
            if (fstrSaveGbn == "저장")
            {
                parameter.AppendSql("       PANJENGDATE =TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')    ");
                parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            }
            else
            {
                parameter.AppendSql("       PANJENGDATE = ''                                    ");
                parameter.AppendSql("     , PANJENGDRNO = 0                                     ");
            }
            parameter.AppendSql("     , PANJENG     = :PANJENG                                  ");
            parameter.AppendSql("     , SOGENCODE   = :SOGENCODE                                ");
            parameter.AppendSql("     , JOCHICODE   = :JOCHICODE                                ");
            parameter.AppendSql("     , WORKYN      = :WORKYN                                   ");
            parameter.AppendSql("     , SAHUCODE    = :SAHUCODE                                 ");
            parameter.AppendSql("     , SAHUREMARK  = :SAHUREMARK                               ");
            parameter.AppendSql("     , UCODE       = :UCODE                                    ");
            parameter.AppendSql("     , SOGENREMARK = :SOGENREMARK                              ");
            parameter.AppendSql("     , JOCHIREMARK = :JOCHIREMARK                              ");
            parameter.AppendSql("     , REEXAM      = :REEXAM                                   ");
            parameter.AppendSql("     , ORCODE      = :ORCODE                                   ");
            parameter.AppendSql("     , ORSAYUCODE  = :ORSAYUCODE                               ");
            parameter.AppendSql("     , PYOJANGGI   = :PYOJANGGI                                ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN                                 ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE                                   ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO                                    ");
            parameter.AppendSql("   AND MCODE       = :MCODE                                    ");
            parameter.AppendSql("   AND PYOJANGGI   = :PYOJANGGI                                ");

            parameter.Add("WRTNO",       item.WRTNO);
            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("PANJENG",     item.PANJENG    );
            parameter.Add("SOGENCODE",   item.SOGENCODE  );
            parameter.Add("JOCHICODE",   item.JOCHICODE  );
            parameter.Add("WORKYN",      item.WORKYN     );
            parameter.Add("SAHUCODE",    item.SAHUCODE   );
            parameter.Add("SAHUREMARK",  item.SAHUREMARK );
            parameter.Add("UCODE",       item.UCODE      );
            parameter.Add("SOGENREMARK", item.SOGENREMARK);
            parameter.Add("JOCHIREMARK", item.JOCHIREMARK);
            parameter.Add("REEXAM",      item.REEXAM     );
            parameter.Add("ORCODE",      item.ORCODE     );
            parameter.Add("ORSAYUCODE",  item.ORSAYUCODE );
            parameter.Add("PYOJANGGI",   item.PYOJANGGI  );
            parameter.Add("ENTSABUN",    item.ENTSABUN   );
            parameter.Add("MCODE",       item.MCODE      );

            return ExecuteNonQuery(parameter);
        }

        public int UpdateAllbyWrtNoMCodePyojanggi1(string fstrSaveGbn, HIC_SPC_PANJENG item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG SET                         ");
            if (fstrSaveGbn == "저장")
            {
                parameter.AppendSql("       PANJENGDATE =TO_DATE(:PANJENGDATE, 'YYYY-MM-DD')    ");
                parameter.AppendSql("     , PANJENGDRNO = :PANJENGDRNO                          ");
            }
            else
            {
                parameter.AppendSql("       PANJENGDATE = ''                                    ");
                parameter.AppendSql("     , PANJENGDRNO = 0                                     ");
            }
            parameter.AppendSql("     , PANJENG     = :PANJENG                                  ");
            parameter.AppendSql("     , SOGENCODE   = :SOGENCODE                                ");
            parameter.AppendSql("     , JOCHICODE   = :JOCHICODE                                ");
            parameter.AppendSql("     , WORKYN      = :WORKYN                                   ");
            parameter.AppendSql("     , SAHUCODE    = :SAHUCODE                                 ");
            parameter.AppendSql("     , SAHUREMARK  = :SAHUREMARK                               ");
            parameter.AppendSql("     , UCODE       = :UCODE                                    ");
            parameter.AppendSql("     , SOGENREMARK = :SOGENREMARK                              ");
            parameter.AppendSql("     , JOCHIREMARK = :JOCHIREMARK                              ");
            parameter.AppendSql("     , REEXAM      = :REEXAM                                   ");
            parameter.AppendSql("     , ORCODE      = :ORCODE                                   ");
            parameter.AppendSql("     , ORSAYUCODE  = :ORSAYUCODE                               ");
            parameter.AppendSql("     , PYOJANGGI   = :PYOJANGGI                                ");
            parameter.AppendSql("     , ENTSABUN    = :ENTSABUN                                 ");
            parameter.AppendSql("     , ENTTIME     = SYSDATE                                   ");
            parameter.AppendSql(" WHERE ROWID       = :RID                                    ");

            parameter.Add("PANJENGDATE", item.PANJENGDATE);
            parameter.Add("PANJENGDRNO", item.PANJENGDRNO);
            parameter.Add("PANJENG", item.PANJENG);
            parameter.Add("SOGENCODE", item.SOGENCODE);
            parameter.Add("JOCHICODE", item.JOCHICODE);
            parameter.Add("WORKYN", item.WORKYN);
            parameter.Add("SAHUCODE", item.SAHUCODE);
            parameter.Add("SAHUREMARK", item.SAHUREMARK);
            parameter.Add("UCODE", item.UCODE);
            parameter.Add("SOGENREMARK", item.SOGENREMARK);
            parameter.Add("JOCHIREMARK", item.JOCHIREMARK);
            parameter.Add("REEXAM", item.REEXAM);
            parameter.Add("ORCODE", item.ORCODE);
            parameter.Add("ORSAYUCODE", item.ORSAYUCODE);
            parameter.Add("PYOJANGGI", item.PYOJANGGI);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("RID", item.RID);

            return ExecuteNonQuery(parameter);
        }

        public int InsertPanjeng(long fnWrtNo, string strCode, string strUCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPC_PANJENG        ");
            parameter.AppendSql("       (WRTNO, JEPDATE, LTDCODE, MCODE, UCODE) ");
            parameter.AppendSql("SELECT WRTNO, JepDate, LtdCode, :MCODE, :UCODE ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU                   ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");

            parameter.Add("WRTNO", fnWrtNo);
            parameter.Add("MCODE", strCode);
            parameter.Add("UCODE", strUCode);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SPC_PANJENG> GetAllbyWrtNoList(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE,'YYYY-MM-DD') PANJENGDATE,MCODE,PANJENG ");
            parameter.AppendSql("     , SOGENCODE,JOCHICODE,WORKYN,SAHUCODE,MSYMCODE,MBUN,UCODE     ");
            parameter.AppendSql("     , SOGENREMARK,JOCHIREMARK,REEXAM,WRTNO,ROWID  AS RID          ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                 ");
            if (!fnWrtNo.IsNullOrEmpty() && fnWrtNo > 0)
            {
                parameter.AppendSql(" WHERE WRTNO = :WRTNO                                          ");
            }
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                            ");
            parameter.AppendSql(" ORDER BY MCODE                                                    ");

            if (!fnWrtNo.IsNullOrEmpty() && fnWrtNo > 0)
            {
                parameter.Add("WRTNO", fnWrtNo);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetMCodebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MCODE, ROWID                                                        ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                      ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                    ");
            parameter.AppendSql("   AND(PanjengDate IS NULL OR PanjengDate = '')                            ");
            parameter.AppendSql(" ORDER BY MCode                                                            ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public int UpdateDelDatebyWrtNo(long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG         ");
            parameter.AppendSql("   SET DELDATE = TRUNC(SYSDATE)            ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO                    ");

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNoUnionWrtNo2(long fnWRTNO, long fnWrtno2, string strTemp)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                     ");
            if (strTemp == "OK")
            {
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8')                            ");
            }
            parameter.AppendSql("   AND Deldate is Null                                                     ");
            if (fnWrtno2 > 0)
            {
                parameter.AppendSql(" UNION ALL                                                             ");
                parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                     ");
                parameter.AppendSql(" WHERE WRTNO  = :WRTNO2                                                ");
                if (strTemp == "OK")
                {
                    parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8')                        ");
                }
                parameter.AppendSql("   AND Deldate is Null                                                 ");
            }
            parameter.AppendSql(" GROUP BY  MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
            parameter.AppendSql(" ORDER BY 1,2,3,4,5                                                        ");

            parameter.Add("WRTNO", fnWRTNO);
            if (fnWrtno2 > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNoWrtNo3(long fnWRTNO, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                     ");
            parameter.AppendSql("   AND MCode <> 'ZZZ'                                                      ");
            parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                        ");
            parameter.AppendSql("   AND Deldate is Null                                                     ");
            if (fnWrtno2 > 0)
            {
                parameter.AppendSql(" UNION ALL                                                             ");
                parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                     ");
                parameter.AppendSql(" WHERE WRTNO  = :WRTNO2                                                ");
                parameter.AppendSql("   AND MCode <> 'ZZZ'                                                  ");
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                    ");
                parameter.AppendSql("   AND Deldate is Null                                                 ");
            }
            parameter.AppendSql(" GROUP BY  MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
            parameter.AppendSql(" ORDER BY 2,1,3,4,5                                                        ");

            parameter.Add("WRTNO", fnWRTNO);
            if (fnWrtno2 > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GeResulttItembyWrtNoWrtNo2(long fnWRTNO, long fnWrtno2, string strTemp)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                     ");
            parameter.AppendSql("   AND MCode <> 'ZZZ'                                                      ");
            if (strTemp == "OK")
            {   
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                    ");
            }
            parameter.AppendSql("   AND Deldate is Null                                                     ");
            if (fnWrtno2 > 0)
            {
                parameter.AppendSql(" UNION ALL                                                             ");
                parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                     ");
                parameter.AppendSql(" WHERE WRTNO  = :WRTNO2                                                ");
                parameter.AppendSql("   AND MCode <> 'ZZZ'                                                  ");
                if (strTemp == "OK")
                {
                    parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                ");
                }
                parameter.AppendSql("   AND Deldate is Null                                                 ");
            }
            parameter.AppendSql(" GROUP BY  MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
            parameter.AppendSql(" ORDER BY 2,1,3,4,5                                                        ");

            parameter.Add("WRTNO", fnWRTNO);
            if (fnWrtno2 > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetPanjengSahuCodebyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Panjeng, SahuCode, COUNT(*) CNT         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG             ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                          ");
            parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','9','A')    ");
            parameter.AppendSql("   AND DelDate IS NULL                         ");
            parameter.AppendSql(" GROUP BY Panjeng,SahuCode                     ");
            parameter.AppendSql(" ORDER BY Panjeng,SahuCode                     ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public long GetPanjengDrNobyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO                         ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG         ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')    ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<long>(parameter);
        }

        public int SelectInsert(string strCode, long gnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPC_PANJENG    ");
            parameter.AppendSql("       (WRTNO, JEPDATE, LTDCODE, MCODE)    ");
            parameter.AppendSql("SELECT WRTNO, JEPDATE, LTDCODE, :MCODE     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU               ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO                      ");

            parameter.Add("MCODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("WRTNO", gnWRTNO);

            return ExecuteNonQuery(parameter);
        }

        public int UpdateDelDatebyRowId(string strRowId)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG         ");
            parameter.AppendSql("   SET DELDATE = TRUNC(SYSDATE)            ");
            parameter.AppendSql(" WHERE ROWID = :RID                        ");

            parameter.Add("RID", strRowId);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo1WrtNo2(long fnWRTNO, long fnWrtno1, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,MCode,Panjeng         ");
            parameter.AppendSql("     , SogenCode,JochiCode,WorkYN,SahuCode,MsymCode,MBun,UCode             ");
            parameter.AppendSql("     , SogenRemark,JochiRemark,ROWID                                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            if (fnWrtno1 > 0 && fnWrtno2 > 0)
            {
                parameter.AppendSql(" WHERE WRTNO IN (:WRTNO1, :WRTNO2)                                     ");
            }
            else
            {
                parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                  ");
            }
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                    ");
            parameter.AppendSql(" ORDER BY MCode                                                            ");

            parameter.Add("WRTNO", fnWRTNO);
            if (fnWrtno1 > 0 && fnWrtno2 > 0)
            {
                parameter.Add("WRTNO1", fnWrtno1);
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public HIC_SPC_PANJENG GetItembyRowId(string fstrPROWID)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PANJENGDATE, 'YYYY-MM-DD') PANJENGDATE,PANJENGDRNO,MCODE,PANJENG,SOGENCODE      ");
            parameter.AppendSql("     , JOCHICODE,WORKYN,SAHUCODE,SAHUREMARK,MSYMCODE,MBUN,UCODE,SOGENREMARK,JOCHIREMARK        ");
            parameter.AppendSql("     , REEXAM,ORCODE,ORSAYUCODE                                                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                                             ");
            parameter.AppendSql(" WHERE ROWID = :RID                                                                            ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                                        ");

            parameter.Add("RID", fstrPROWID);

            return ExecuteReaderSingle<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtno1Wrtno2(long fnWrtno1, long fnWrtno2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(PanjengDate,'YYYY-MM-DD') PanjengDate,MCode,Panjeng ");
            parameter.AppendSql("     , SogenCode,JochiCode,WorkYN,SahuCode,MsymCode,MBun,UCode     ");
            parameter.AppendSql("     , SogenRemark,JochiRemark,ROWID RID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                 ");
            if (fnWrtno1 > 0 && fnWrtno2 > 0)
            {
                parameter.AppendSql(" WHERE WRTNO IN (:WRTNO1, :WRTNO2)                             ");
            }
            else
            {
                parameter.AppendSql(" WHERE WRTNO = :WRTNO1                                         ");
            }
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                            ");
            parameter.AppendSql(" ORDER BY MCode                                                    ");

            if (fnWrtno1 > 0 && fnWrtno2 > 0)
            {
                parameter.Add("WRTNO1", fnWrtno1);
                parameter.Add("WRTNO2", fnWrtno2);
            }
            else
            {
                parameter.Add("WRTNO1", fnWrtno1);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENG, MCODE                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                     ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            if (nWRTNO != 0)
            {
                parameter.AppendSql("   AND WRTNO = :WRTNO                              ");
            }
            parameter.AppendSql("   AND MCode NOT IN ('ZZZ')                            ");
            parameter.AppendSql("   AND Panjeng IN ('3','5','6','A')                    "); //C1,D1,D2,DN     
            parameter.AppendSql(" ORDER BY Panjeng                                      ");

            if (nWRTNO != 0)
            {
                parameter.Add("WRTNO", nWRTNO);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNo2(long nWrtno2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Panjeng,COUNT(*) CNT            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG     ");
            parameter.AppendSql(" WHERE WRTNO = :WRTNO2                 ");
            parameter.AppendSql("   AND MCode NOT IN ('ZZZ')            ");
            parameter.AppendSql(" GROUP BY Panjeng                      ");
            parameter.AppendSql(" ORDER BY Panjeng                      ");

            parameter.Add("WRTNO2", nWrtno2);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetPanjengbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT WRTNO, MCODE, PANJENG, SOGENREMARK, JOCHIREMARK     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                         ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO                                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE = '')                   ");
            parameter.AppendSql("   AND Panjeng IN ('5','6','A')                            ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public long GetPanjengDrNobyWrtNo2(long fnWrtno2)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANJENGDRNO                 ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO          ");

            parameter.Add("WRTNO", fnWrtno2);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_SPC_PANJENG> GetItembyWrtNoWrtNo2(long fnWRTNO, long fnWrtno2, string strGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark     ");            
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                         ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                                     ");            
            if (strGubun == "")
            {
                parameter.AppendSql("   AND MCode <> 'ZZZ'                                                  ");
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                    ");
            }
            else
            {
                parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8')                            ");
            }
            parameter.AppendSql("   AND Deldate is Null                                                     ");
            if (fnWrtno2 > 0)
            {
                parameter.AppendSql(" UNION ALL                                                             ");
                parameter.AppendSql("SELECT MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                     ");
                parameter.AppendSql(" WHERE WRTNO  = :WRTNO2                                                ");
                parameter.AppendSql("   AND MCode <> 'ZZZ'                                                  ");
                if (strGubun == "")
                {
                    parameter.AppendSql("   AND Panjeng IN ('3','4','5','6','7','8','9','A')                ");
                }
                parameter.AppendSql("   AND Deldate is Null                                                 ");
            }
            parameter.AppendSql(" GROUP BY  MCode,SogenCode,Panjeng,WorkYN,SahuCode,JochiRemark,SogenRemark ");
            if (strGubun == "")
            {
                parameter.AppendSql(" ORDER BY 2,1,3,4,5                                                    ");
            }
            else
            {
                parameter.AppendSql(" ORDER BY 1,2,3,4,5                                                    ");
            }

            parameter.Add("WRTNO", fnWRTNO);
            if (fnWrtno2 > 0)
            {
                parameter.Add("WRTNO2", fnWrtno2);
            }

            return ExecuteReader<HIC_SPC_PANJENG>(parameter);
        }

        public int GetCountbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT                                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                                     ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO                                                                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                                ");
            parameter.AppendSql("   AND (PanjengDrNo IS NULL OR PanjengDrNo = 0)                                        ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyWrtNo2(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('x') CNT                                                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG                                                     ");
            parameter.AppendSql(" WHERE WRTNO  =:WRTNO                                                                  ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')                                                ");
            //parameter.AppendSql("   AND (PanjengDrNo IS NULL OR PanjengDrNo = 0)                                        ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public int ChasuUpDate(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG         ");
            parameter.AppendSql("   SET GJCHASU = '1'                       ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND WRTNO =:WRTNO                      ");
            parameter.AppendSql("   AND (GjChasu='' OR GjChasu IS NULL)     ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public string Read_Spc_Panjeng_YN(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG     ");
            parameter.AppendSql(" WHERE WRTNO     = :WRTNO              ");
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE ='')");
            parameter.AppendSql("   AND GJCHASU = '2'                   ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }

        public int Insert(HIC_SPC_PANJENG item2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_SPC_PANJENG (       ");
            parameter.AppendSql("       WRTNO,JEPDATE,LtdCode,MCode,UCode        ");
            parameter.AppendSql(") VALUES (                                      ");
            parameter.AppendSql("      :WRTNO,TO_DATE(:JEPDATE,'YYYY-MM-DD'),:LtdCode,:MCode,:UCode    ");
            parameter.AppendSql(")  ");

            #region Query 변수대입
            parameter.Add("WRTNO",   item2.WRTNO);
            parameter.Add("JepDate", item2.JEPDATE);
            parameter.Add("LtdCode", item2.LTDCODE);
            parameter.Add("MCode",   item2.MCODE);
            parameter.Add("UCode",   item2.UCODE);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public string FindRid(long argWRTNO, string strMCode = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_SPC_PANJENG ");
            parameter.AppendSql(" WHERE WRTNO   = :WRTNO            ");
            if (!string.IsNullOrEmpty(strMCode))
            {
                parameter.AppendSql("   AND MCODE   = :MCODE            ");
            }
            
            parameter.Add("WRTNO", argWRTNO);

            if (!string.IsNullOrEmpty(strMCode))
            { 
                parameter.Add("MCODE", strMCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            return ExecuteScalar<string>(parameter);
        }

        public int OneDelUpDate(string gstrSysDate, string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG                                 ");
            parameter.AppendSql("   SET DelDate     = TO_DATE('" + gstrSysDate + "','YYYY-MM-DD')   ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("   AND ROWID =:RID                                                ");

            #region Query 변수대입
            parameter.Add("RID", rOWID);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int All_Del_UpDate(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_SPC_PANJENG ");
            parameter.AppendSql("   SET DelDate     = SYSDATE       ");
            parameter.AppendSql(" WHERE WRTNO       = :WRTNO        ");
            parameter.AppendSql("   AND DELDATE IS NULL             ");

            #region Query 변수대입
            parameter.Add("WRTNO", argWRTNO);
            #endregion
            return ExecuteNonQuery(parameter);
        }
    }
}
