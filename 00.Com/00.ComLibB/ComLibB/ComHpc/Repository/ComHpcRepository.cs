
namespace ComLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComLibB.Dto;

    public class ComHpcRepository :BaseRepository
    {
        public ComHpcRepository()
        {
        }

        public COMHPC GetItembyWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT PANO, SNAME, AGE, SEX, LTDCODE, GbSTS, WRTNO    ");
            parameter.AppendSql("     , TO_CHAR(SDATE, 'YYYY-MM-DD') JEPDATE, GJJONG    ");
            parameter.AppendSql("     , MAILCODE, JUSO1 || JUSO2 as JUSO                ");
            parameter.AppendSql("     , TO_CHAR(RECVDATE,'YYYY-MM-DD') RECVDATE         ");
            parameter.AppendSql("     , TO_CHAR(MAILDATE,'YYYY-MM-DD') MAILDATE         ");
            parameter.AppendSql("     , PTNO, GBEKG, AGE, IEMUNNO                       ");
            parameter.AppendSql("     , PANCODE, PANREMARK, DRSABUN, DRNAME             ");
            parameter.AppendSql("     , TO_CHAR(SDATE, 'YYYY-MM-DD') SDATE              ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU                           ");
            parameter.AppendSql(" WHERE WRTNO  = :WRTNO                                 ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetItembyPaNoSDate(long nPano, string strSDate, string strGbStsYN = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(SDate, 'YYYY-MM-DD') SDate,WRTNO,PanCode,PanRemark,DrName   ");
            parameter.AppendSql("     , SNAME, SEX, AGE, PTNO                                               ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU                                               ");
            parameter.AppendSql(" WHERE PANO = :PANO                                                        ");
            parameter.AppendSql("   AND SDATE < TO_DATE(:SDATE, 'YYYY-MM-DD')                               ");
            parameter.AppendSql("   AND DELDATE IS NULL                                                     ");
            if (strGbStsYN == "Y")
            {
                parameter.AppendSql("   AND GbSTS IN ('3','9')                                              "); //입력완료/판정완료
            }
            parameter.AppendSql(" ORDER BY SDate DESC                                                       ");

            parameter.Add("PANO", nPano);
            parameter.Add("SDATE", strSDate);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItemHeaNoActingbyWrtNo(long fnWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.Part,a.ExCode,b.Excode Code,b.HName,a.Result,a.ResCode,a.Panjeng          ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName,B.HEASORT  ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                    ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
            //parameter.AppendSql(" ORDER BY b.HEASORT, a.ExCode                                                      ");            
            parameter.AppendSql(" ORDER BY b.HEASORT, b.GBSORT, a.EXCODE                                               ");

            parameter.Add("WRTNO", fnWRTNO);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetItembyWrtNoResultHea(long nWRTNO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT b.HeaSORT,a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng                 ");
            parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse,b.HName        ");
            parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                ");
            parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                ");
            parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                       ");
            parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                    ");
            parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                   ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReader<COMHPC>(parameter);
        }

        public COMHPC FindOne(string v)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,SANGHO,NAME,TEL,FAX,EMAIL,MAILCODE,JUSO,SAUPNO,JSAUPNO,UPTAE,JONGMOK,DAEPYO,JUMIN,JISA,KIHO    ");
            parameter.AppendSql("      ,UPJONG,SANKIHO,GWANSE,JIDOWON,BONAME,BOJIK,GYUMOGBN,GESINO,YOUNGUPSO,JUSODETAIL                     ");
            parameter.AppendSql("      ,SELDATE,NEGODATE,MAMT,FAMT,GYEDATE,JEPUM1,JEPUM2,JEPUM3,JEPUM4,JEPUM5                               ");
            parameter.AppendSql("      ,GBGEMJIN,GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,ARMY_HSP,INWON,HAREMARK,GBGARESV  				    ");
            parameter.AppendSql("      ,DELDATE,JEPUMLIST,REMARK,GBSCHOOL,SPCHUNGGU,HEMSNO,HTEL,HEAGAJEPSU1                                 ");
            parameter.AppendSql("      ,HEAGAJEPSU2,HEAGAJEPSU3,HEAGAJEPSU4,TAX_REMARK,BUILDNO,TAX_MAILCODE,TAX_JUSO,TAX_JUSODETAIL 		");
            parameter.AppendSql("      ,DLTD,CHULNOTSAYU,CHREMARK,BOREMARK, NAME AS DNAME, ROWID AS RID	");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD                                                                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("  AND Code  =:Code                                                                                         ");

            parameter.Add("Code", v);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }
        public string Read_Hea_ExJong_Name(string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                        ");
            parameter.AppendSql("  FROM ADMIN.HEA_EXJONG      ");
            parameter.AppendSql(" WHERE CODE =:CODE                 ");

            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Hic_ResCodeName(string GUBUN, string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_RESCODE                 ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                          ");
            parameter.AppendSql("   AND CODE  = :CODE                           ");

            parameter.Add("GUBUN", GUBUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            parameter.Add("CODE", CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }
        public COMHPC GetItemHicHeabyWrtNo(string fstrGubun, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            if (fstrGubun == "HIC")
            {
                parameter.AppendSql("SELECT PANO, WRTNO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(JEPDATE,'YYYY-MM-DD') JEPDATE, GJYEAR, PTNO  ");
                parameter.AppendSql("     , TO_CHAR(IPSADATE,'yyyy-mm-dd') IPSADATE, GJJONG, UCODES, SEXAMS, ERFLAG, WAITREMARK         ");
                parameter.AppendSql("     , SABUN, BUSENAME, GBSUCHEP, JIKJONG, GJCHASU, GJBANGI                                        ");
                parameter.AppendSql("     , JONGGUMYN, GBHEAENDO, IEMUNNO, GBN, CLASS, GBCHK3                                           ");
                parameter.AppendSql("     , TEL, BUSEIPSA, SANGDAMDRNO                                                                  ");
                parameter.AppendSql("  FROM ADMIN.HIC_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");
            }
            else
            {
                parameter.AppendSql("SELECT PANO, WRTNO, SNAME, AGE, SEX, LTDCODE, TO_CHAR(SDATE,'YYYY-MM-DD') JEPDATE, '' GJYEAR, PTNO ");
                parameter.AppendSql("     , '' IPSADATE, GJJONG, '' UCODES, SEXAMS, '' ERFLAG, '' WAITREMARK                            ");
                parameter.AppendSql("     , SABUN, '' BUSENAME, '' GBSUCHEP, '' JIKJONG, '' GJCHASU, '' GJBANGI                         ");
                parameter.AppendSql("     , '' JONGGUMYN, '' GBHEAENDO, IEMUNNO, '' GBN, '' CLASS, '' GBCHK3                            ");
                parameter.AppendSql("     , '' TEL, '' BUSEIPSA, DRSABUN SANGDAMDRNO                                                    ");
                parameter.AppendSql("  FROM ADMIN.HEA_JEPSU                                                                       ");
                parameter.AppendSql(" WHERE WRTNO = :WRTNO                                                                              ");
                parameter.AppendSql("   AND DELDATE IS NULL                                                                             ");
            }

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReaderSingle<COMHPC>(parameter);
        }

        public List<COMHPC> GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(string fstrGubun, long fnWrtNo)
        {
            MParameter parameter = CreateParameter();

            if (fstrGubun == "HIC")
            {
                parameter.AppendSql("SELECT b.part, a.WRTNO, a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID      ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit,b.HEAPART  ");
                parameter.AppendSql("  FROM ADMIN.HIC_RESULT a                                                    ");
                parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
                parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
                parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
                //parameter.AppendSql(" ORDER BY a.PANJENG, b.PART, a.EXCODE                                              ");
                parameter.AppendSql(" ORDER BY a.PART, b.GBSORT, a.EXCODE                                               ");
            }
            else
            {
                parameter.AppendSql("SELECT b.part, a.WRTNO, a.ExCode,b.HName,a.Result,a.ResCode,a.Panjeng,a.ROWID      ");
                parameter.AppendSql("     , b.Min_M,b.Max_M,b.Min_F,b.Max_F,b.ResultType,b.GbCodeUse, b.Unit,b.HEAPART  ");
                parameter.AppendSql("  FROM ADMIN.HEA_RESULT a                                                    ");
                parameter.AppendSql("     , ADMIN.HIC_EXCODE b                                                    ");
                parameter.AppendSql(" WHERE a.WRTNO  = :WRTNO                                                           ");
                parameter.AppendSql("   AND a.EXCODE = b.CODE(+)                                                        ");
                //parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                       ");
                parameter.AppendSql(" ORDER BY b.HeaSORT,a.ExCode                                                       ");
            }

            parameter.Add("WRTNO", fnWrtNo);

            return ExecuteReader<COMHPC>(parameter);
        }
        public string Read_ExJong_Name(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_EXJONG  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Hic_ExJong_Name(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_EXJONG  ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", CODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<string>(parameter);
        }

        public List<COMHPC> GetHeaJepsuitembyPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT *                           ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                ");
            parameter.AppendSql("   AND DELDATE IS NULL             ");
            parameter.AppendSql(" ORDER BY SDATE DESC               ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<COMHPC>(parameter);
        }

        public List<COMHPC> GetHicJepsuitembyPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT JEPDATE, FC_HIC_GJJONG_NAME(GJJONG,UCODES) GJJONG, WRTNO    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU                                       ");
            parameter.AppendSql(" WHERE PTNO = :PTNO                                                ");
            parameter.AppendSql("   AND DELDATE IS NULL                                             ");
            parameter.AppendSql(" ORDER BY WRTNO DESC                                               ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<COMHPC>(parameter);
        }
    }
}
