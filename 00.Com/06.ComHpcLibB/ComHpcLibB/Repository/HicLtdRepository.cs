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
    public class HicLtdRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicLtdRepository()
        {
        }

        public List<HIC_LTD> ViewLtd(string keyWords, string searchOption = "")
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE,SANGHO,NAME,TEL,FAX,EMAIL,MAILCODE,JUSO,SAUPNO,JSAUPNO,UPTAE,JONGMOK,DAEPYO,JUMIN,JISA,KIHO    ");
            parameter.AppendSql("      ,UPJONG,SANKIHO,GWANSE,JIDOWON,BONAME,BOJIK,GYUMOGBN,GESINO,YOUNGUPSO,JUSODETAIL                     ");
            //parameter.AppendSql("      ,SELDATE,NEGODATE,MAMT,FAMT,GYEDATE,JEPUM1,JEPUM2,JEPUM3,JEPUM4,JEPUM5                               ");
            parameter.AppendSql("      ,TO_CHAR(SELDATE,'YYYY-MM-DD') SELDATE,TO_CHAR(NEGODATE,'YYYY-MM-DD') NEGODATE,MAMT,FAMT             ");
            parameter.AppendSql("      ,TO_CHAR(GYEDATE, 'YYYY-MM-DD') GYEDATE,JEPUM1,JEPUM2,JEPUM3,JEPUM4,JEPUM5                           ");
            parameter.AppendSql("      ,GBGEMJIN,GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,ARMY_HSP,INWON,HAREMARK,GBGARESV  				    ");
            parameter.AppendSql("      ,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,JEPUMLIST,REMARK,GBSCHOOL,SPCHUNGGU,HEMSNO,HTEL,HEAGAJEPSU1   ");
            parameter.AppendSql("      ,HEAGAJEPSU2,HEAGAJEPSU3,HEAGAJEPSU4,TAX_REMARK,BUILDNO,TAX_MAILCODE,TAX_JUSO,TAX_JUSODETAIL 		");
            parameter.AppendSql("      ,DLTD,CHULNOTSAYU,CHREMARK,BOREMARK,ROWID AS RID 													");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD                                                                                 ");
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");

            if (!searchOption.IsNullOrEmpty())
            {
                switch (searchOption)
                {
                    case "NAME": parameter.AppendSql("  AND NAME   LIKE :NAME                                                            "); break;
                    case "DAEPYO": parameter.AppendSql("  AND DAEPYO LIKE :DAEPYO                                                          "); break;
                    case "CODE": parameter.AppendSql("  AND CODE      = :CODE                                                            "); break;
                    default:
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyWords))
                {
                    parameter.AppendSql("  AND Name LIKE :Name                                                                             ");
                }
            }

            parameter.AppendSql("   AND DelDate IS NULL                                                                                     ");
            parameter.AppendSql(" ORDER BY Name                                                                                             ");

            if (!searchOption.IsNullOrEmpty())
            {
                switch (searchOption)
                {
                    case "NAME": parameter.AddLikeStatement("NAME", keyWords); break;
                    case "DAEPYO": parameter.AddLikeStatement("DAEPYO", keyWords); break;
                    case "CODE": parameter.Add("CODE", keyWords); break;
                    default:
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyWords))
                {
                    parameter.AddLikeStatement("Name", keyWords);
                }
            }

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public List<HIC_LTD> GetAct(string gstrRetValue)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT             CODE,NAME,SAUPNO,DAEPYO,UPTAE,JONGMOK,TEL,FAX,BONAME,JISA,KIHO              ");
            parameter.AppendSql("                 , TO_CHAR(GYEDATE,'YYYY-MM-DD') GYEDATE                                       ");

            parameter.AppendSql("FROM               ADMIN.HIC_LTD                                                         ");
            parameter.AppendSql("WHERE              DELDATE IS NULL                                                             ");
            parameter.AppendSql("   AND             CODE = :CODE                                                                ");

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public List<HIC_LTD> GetItemData(bool rdoSort1, bool rdoSort3, string txtViewCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT             CODE,NAME,SAUPNO,DAEPYO,UPTAE,JONGMOK,TEL,FAX,BONAME,JISA,KIHO,             ");
            parameter.AppendSql("                   TO_CHAR(GYEDATE,'YYYY-MM-DD') GYEDATE                                       ");

            parameter.AppendSql("FROM               ADMIN.HIC_LTD                                                         ");
            parameter.AppendSql("WHERE              DELDATE IS NULL                                                             ");

            if(rdoSort3 == true && txtViewCode != "")
            {
                parameter.AppendSql("AND            DAEPYO LIKE :TXTVIEWCODE                                                    ");
            }
            else if(txtViewCode != "")
            {
                parameter.AppendSql("AND            NAME LIKE :TXTVIEWCODE                                                      ");
            }

            if(rdoSort1 == true)
            {
                parameter.AppendSql("ORDER BY       NAME                                                                        ");
            }
            else
            {       
                parameter.AppendSql("ORDER BY       DAEPYO, CODE                                                                ");
            }
            

            if (rdoSort3 == true && txtViewCode != "")
            {
                parameter.AddLikeStatement("TXTVIEWCODE", txtViewCode);
            }
            else if (txtViewCode != "")
            {
                parameter.AddLikeStatement("TXTVIEWCODE", txtViewCode);
            }

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public List<HIC_LTD> ViewChukLtd(string keyWords, string searchOption)
        { 
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.CODE");
            parameter.AppendSql("     , A.SANGHO");
            parameter.AppendSql("     , A.NAME");
            parameter.AppendSql("     , A.TEL");
            parameter.AppendSql("     , A.FAX");
            parameter.AppendSql("     , A.EMAIL");
            parameter.AppendSql("     , A.MAILCODE");
            parameter.AppendSql("     , A.JUSO");
            parameter.AppendSql("     , A.SAUPNO");
            parameter.AppendSql("     , A.UPTAE");
            parameter.AppendSql("     , A.JONGMOK");
            parameter.AppendSql("     , A.DAEPYO");
            parameter.AppendSql("     , A.JISA");
            parameter.AppendSql("     , A.KIHO");
            parameter.AppendSql("     , A.UPJONG");
            parameter.AppendSql("     , A.SANKIHO");
            parameter.AppendSql("     , A.BONAME");
            parameter.AppendSql("     , A.GYEDATE");
            parameter.AppendSql("     , A.JUSODETAIL");
            parameter.AppendSql("     , B.LTDSEQNO");
            parameter.AppendSql("     , B.LTDGONGNAME");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD A");
            parameter.AppendSql("     , ADMIN.HIC_CHUKMST_NEW B");
            parameter.AppendSql(" WHERE 1 = 1");
            parameter.AppendSql("   AND A.CODE = B.LTDCODE(+)");
            parameter.AppendSql("   AND A.DELDATE IS NULL");

            if (!searchOption.IsNullOrEmpty())
            {
                switch (searchOption)
                {
                    case "NAME": parameter.AppendSql("  AND A.NAME   LIKE :NAME                                                            "); break;
                    case "DAEPYO": parameter.AppendSql("  AND A.DAEPYO LIKE :DAEPYO                                                          "); break;
                    case "CODE": parameter.AppendSql("  AND A.CODE      = :CODE                                                            "); break;
                    default:
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyWords))
                {
                    parameter.AppendSql("  AND A.Name LIKE :Name                                                                             ");
                }
            }

            parameter.AppendSql("   AND A.DelDate IS NULL                                                                                     ");
            parameter.AppendSql(" GROUP BY ");
            parameter.AppendSql("       A.CODE");
            parameter.AppendSql("     , A.SANGHO");
            parameter.AppendSql("     , A.NAME");
            parameter.AppendSql("     , A.TEL");
            parameter.AppendSql("     , A.FAX");
            parameter.AppendSql("     , A.EMAIL");
            parameter.AppendSql("     , A.MAILCODE");
            parameter.AppendSql("     , A.JUSO");
            parameter.AppendSql("     , A.SAUPNO");
            parameter.AppendSql("     , A.UPTAE");
            parameter.AppendSql("     , A.JONGMOK");
            parameter.AppendSql("     , A.DAEPYO");
            parameter.AppendSql("     , A.JISA");
            parameter.AppendSql("     , A.KIHO");
            parameter.AppendSql("     , A.UPJONG");
            parameter.AppendSql("     , A.SANKIHO");
            parameter.AppendSql("     , A.BONAME");
            parameter.AppendSql("     , A.GYEDATE");
            parameter.AppendSql("     , A.JUSODETAIL");
            parameter.AppendSql("     , B.LTDSEQNO");
            parameter.AppendSql("     , B.LTDGONGNAME");
            parameter.AppendSql(" ORDER BY A.NAME       ");

            if (!searchOption.IsNullOrEmpty())
            {
                switch (searchOption)
                {
                    case "NAME": parameter.AddLikeStatement("NAME", keyWords); break;
                    case "DAEPYO": parameter.AddLikeStatement("DAEPYO", keyWords); break;
                    case "CODE": parameter.Add("CODE", keyWords); break;
                    default:
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyWords))
                {
                    parameter.AddLikeStatement("Name", keyWords);
                }
            }

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public List<HIC_LTD> GetCodeName(string strBogenso)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                     CODE, NAME                  ");
            parameter.AppendSql("FROM                       ADMIN.HIC_LTD         ");
            parameter.AppendSql("WHERE                      KIHO = :BOGENSO             ");

            parameter.Add("BOGENSO", strBogenso, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public string GetSpcRemarkByCode(long strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT SPC_REMARK              ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", strLtdCode);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_LTD> GetListGaResv()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT CODE, SANGHO            ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE 1 = 1                   ");
            parameter.AppendSql("   AND GBGARESV = 'Y'          ");
            parameter.AppendSql("   AND DELDATE IS NULL         ");
            parameter.AppendSql(" ORDER BY Sangho               ");

            return ExecuteReader<HIC_LTD>(parameter);
        }
        

        public List<HIC_LTD> GetBusinessItem(string txtLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         JUSO, JUSODETAIL, SANGHO, DAEPYO, UPTAE, JONGMOK, SAUPNO, MAILCODE, TAX_JUSO, TAX_JUSODETAIL, JSAUPNO   ");
            parameter.AppendSql("  FROM         ADMIN.HIC_LTD                                                                                     ");
            parameter.AppendSql(" WHERE         CODE = :CODE                                                                                            ");

            parameter.Add("CODE", txtLtdCode);

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public string GetChulNotSayuByLtdCode(long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CHULNOTSAYU              ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD      ");
            parameter.AppendSql(" WHERE CODE = :CODE             ");

            parameter.Add("CODE", nLtdCode);

            return ExecuteScalar<string>(parameter);
        }

        public List<HIC_LTD> GetBusiness(double txtLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT                     JUSO,JUSODETAIL,SANGHO,DAEPYO,UPTAE,JONGMOK,SAUPNO,MAILCODE             ");
            parameter.AppendSql("  FROM                     ADMIN.HIC_LTD                                                     ");
            parameter.AppendSql(" WHERE                     CODE = :CODE                                                            ");

            parameter.Add("CODE",       txtLtdCode                  );

            return ExecuteReader<HIC_LTD>(parameter);
        }

        public string GetHTelByLtdCode(long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HTEL                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", nLtdCode);

            return ExecuteScalar<string>(parameter);
        }

        public int UpdateSpcRemarkByLtdCode(string strRemark, long argLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_LTD         ");
            parameter.AppendSql("   SET SPC_REMARK =:SPC_REMARK     ");
            parameter.AppendSql(" WHERE CODE  =:CODE                ");

            parameter.Add("SPC_REMARK", strRemark);
            parameter.Add("CODE", argLtdCode);

            return ExecuteNonQuery(parameter);
        }

        public HIC_LTD GetMailCodebyCode(string strLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAILCODE, JUSO, JUSODETAIL, DAEPYO, TEL, NAME ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD                     ");
            parameter.AppendSql(" WHERE CODE = :CODE                            ");

            parameter.Add("CODE", strLtdCode);

            return ExecuteReaderSingle<HIC_LTD>(parameter);
        }

        public string GetJusoByCode(long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT JUSO || ' ' || JUSODETAIL AS JUSO                   ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", nLtdCode);

            return ExecuteScalar<string>(parameter);
        }

        public string GetGbResvByCode(long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GBGARESV                ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", nLtdCode);

            return ExecuteScalar<string>(parameter);
        }

        public string GetNamebyCode(string argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME                ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD ");
            parameter.AppendSql(" WHERE CODE = :CODE        ");

            parameter.Add("CODE", argCode);

            return ExecuteScalar<string>(parameter);
        }

        public string GetFaxNoByLtdCode(long nLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT FAX                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", nLtdCode);

            return ExecuteScalar<string>(parameter);
        }

        public HIC_LTD GetIetmbyCode(long fstrLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT NAME, JISA, KIHO, SANGHO, SAUPNO, SANKIHO, JUSO, JUSODETAIL ");
            parameter.AppendSql("      , TEL, JONGMOK, JEPUMLIST, DAEPYO, UPJONG, GWANSE, HEMSNO    ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD                                         ");
            parameter.AppendSql(" WHERE CODE = :CODE                                                ");

            parameter.Add("CODE", fstrLtdCode);

            return ExecuteReaderSingle<HIC_LTD>(parameter);
        }

        public HIC_LTD GetAllbyCode(string fstrLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, SANGHO, NAME                          ");
            parameter.AppendSql("     , TEL, FAX, EMAIL                             ");
            parameter.AppendSql("     , MAILCODE, JUSO, SAUPNO                      ");
            parameter.AppendSql("     , UPTAE, JONGMOK, DAEPYO                      ");
            parameter.AppendSql("     , JUMIN, JISA, KIHO                           ");
            parameter.AppendSql("     , UPJONG, SANKIHO, GWANSE                     ");
            parameter.AppendSql("     , JIDOWON, BONAME, BOJIK                      ");
            parameter.AppendSql("     , GYUMOGBN, GESINO, SELDATE                   ");
            parameter.AppendSql("     , GYEDATE, JEPUM1, JEPUM2                     ");
            parameter.AppendSql("     , JEPUM3, JEPUM4, JEPUM5                      ");
            parameter.AppendSql("     , GBGEMJIN, GBCHUKJENG, GBDAEHANG             ");
            parameter.AppendSql("     , GBJONGGUM, GBGUKGO, DELDATE                 ");
            parameter.AppendSql("     , JEPUMLIST, REMARK, ARMY_HSP                 ");
            parameter.AppendSql("     , YOUNGUPSO, UPSONAME, JUSODETAIL             ");
            parameter.AppendSql("     , NEGODATE, MAMT, FAMT                        ");
            parameter.AppendSql("     , INWON, GBSCHOOL, SPCHUNGGU                  ");
            parameter.AppendSql("     , HEMSNO, HAREMARK, HTEL                      ");
            parameter.AppendSql("     , GBGARESV, HEAGAJEPSU1, HEAGAJEPSU2          ");
            parameter.AppendSql("     , HEAGAJEPSU3, HEAGAJEPSU4, SPC_REMARK        ");
            parameter.AppendSql("     , TAX_REMARK, JSAUPNO, BUILDNO                ");
            parameter.AppendSql("     , TAX_MAILCODE, TAX_JUSO, TAX_JUSODETAIL      ");
            parameter.AppendSql("     , DLTD, CODE2, DLTD2                          ");
            parameter.AppendSql("     , CHULNOTSAYU, CHREMARK, BOREMARK             ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD                         ");
            parameter.AppendSql(" WHERE CODE = :CODE                                ");

            parameter.Add("CODE", fstrLtdCode);

            return ExecuteReaderSingle<HIC_LTD>(parameter);
        }

        public HIC_LTD GetItembyCode(string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT Sangho,DaePyo,BoName,BoJik,Juso,JusoDetail,JepumList,Tel,Fax,HTel,Remark,NAME   ");
            parameter.AppendSql(" , CODE, Jongmok                                                                       ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD                                                             ");
            parameter.AppendSql(" WHERE CODE = :CODE                                                                    ");

            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_LTD>(parameter);
        }

        public int UpdateGaResv(long argLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_LTD     ");
            parameter.AppendSql("   SET GBGARESV = 'N'          ");
            parameter.AppendSql(" WHERE CODE  =:LTDCODE      ");

            parameter.Add("CODE", argLtdCode);

            return ExecuteNonQuery(parameter);
        }

        public HIC_LTD GetCountLtdCode(long strLtdCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", strLtdCode);

            return ExecuteScalar<HIC_LTD>(parameter);
        }

        public string GetHaRemarkbyLtdCode(long v)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT HAREMARK                ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", v);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRemarkbyLtdCode(long v)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT REMARK                ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD     ");
            parameter.AppendSql(" WHERE CODE = :CODE            ");

            parameter.Add("CODE", v);

            return ExecuteScalar<string>(parameter);
        }

        public void InsertOSHA(HIC_LTD item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_OSHA_SITE (                                                              ");
        }

        public int Update(HIC_LTD item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_LTD                     ");
            parameter.AppendSql("   SET SANGHO          =:SANGHO                ");
            parameter.AppendSql("      ,NAME            =:NAME                  ");
            parameter.AppendSql("      ,TEL             =:TEL                   ");
            parameter.AppendSql("      ,FAX             =:FAX                   ");
            parameter.AppendSql("      ,EMAIL           =:EMAIL                 ");
            parameter.AppendSql("      ,MAILCODE        =:MAILCODE              ");
            parameter.AppendSql("      ,JUSO            =:JUSO                  ");
            parameter.AppendSql("      ,JUSODETAIL      =:JUSODETAIL            ");
            parameter.AppendSql("      ,BuildNo         =:BuildNo               ");
            parameter.AppendSql("      ,SAUPNO          =:SAUPNO                ");
            parameter.AppendSql("      ,UPTAE           =:UPTAE                 ");
            parameter.AppendSql("      ,JONGMOK         =:JONGMOK               ");
            parameter.AppendSql("      ,DAEPYO          =:DAEPYO                ");
            parameter.AppendSql("      ,JUMIN           =:JUMIN                 ");
            parameter.AppendSql("      ,JISA            =:JISA                  ");
            parameter.AppendSql("      ,KIHO            =:KIHO                  ");
            parameter.AppendSql("      ,UPJONG          =:UPJONG                ");
            parameter.AppendSql("      ,SANKIHO         =:SANKIHO               ");
            parameter.AppendSql("      ,GWANSE          =:GWANSE                ");
            parameter.AppendSql("      ,JIDOWON         =:JIDOWON               ");
            parameter.AppendSql("      ,BONAME          =:BONAME                ");
            parameter.AppendSql("      ,BOJIK           =:BOJIK                 ");
            parameter.AppendSql("      ,GYUMOGBN        =:GYUMOGBN              ");
            parameter.AppendSql("      ,SELDATE         =:SELDATE               ");
            parameter.AppendSql("      ,GyeDate         =:GyeDate               ");
            parameter.AppendSql("      ,GesiNo          =:GesiNo                ");
            parameter.AppendSql("      ,DelDate         =:DelDate               ");
            parameter.AppendSql("      ,JEPUM1          =:JEPUM1                ");
            parameter.AppendSql("      ,JEPUM2          =:JEPUM2                ");
            parameter.AppendSql("      ,JEPUM3          =:JEPUM3                ");
            parameter.AppendSql("      ,JEPUM4          =:JEPUM4                ");
            parameter.AppendSql("      ,JEPUM5          =:JEPUM5                ");
            parameter.AppendSql("      ,GbGemjin        =:GbGemjin              ");
            parameter.AppendSql("      ,GbChukJeng      =:GbChukJeng            ");
            parameter.AppendSql("      ,GbDaeHang       =:GbDaeHang             ");
            parameter.AppendSql("      ,GbJonggum       =:GbJonggum             ");
            parameter.AppendSql("      ,GbGukgo         =:GbGukgo               ");
            parameter.AppendSql("      ,JepumList       =:JepumList             ");
            parameter.AppendSql("      ,Remark          =:Remark                ");
            parameter.AppendSql("      ,ARMY_HSP        =:ARMY_HSP              ");
            parameter.AppendSql("      ,negodate        =:negodate              ");
            parameter.AppendSql("      ,mamt            =:mamt                  ");
            parameter.AppendSql("      ,famt            =:famt                  ");
            parameter.AppendSql("      ,Inwon           =:Inwon                 ");
            parameter.AppendSql("      ,YOUNGUPSO       =:YOUNGUPSO             ");
            parameter.AppendSql("      ,SpchungGu       =:SpchungGu             ");
            parameter.AppendSql("      ,HemsNo          =:HemsNo                ");
            parameter.AppendSql("      ,HaRemark        =:HaRemark              ");
            parameter.AppendSql("      ,GbSchool        =:GbSchool              ");
            parameter.AppendSql("      ,GbGaResv        =:GbGaResv              ");
            parameter.AppendSql("      ,HTEL            =:HTEL                  ");
            parameter.AppendSql("      ,HeaGajepsu1     =:HeaGajepsu1           ");
            parameter.AppendSql("      ,HeaGajepsu2     =:HeaGajepsu2           ");
            parameter.AppendSql("      ,HeaGajepsu3     =:HeaGajepsu3           ");
            parameter.AppendSql("      ,HeaGajepsu4     =:HeaGajepsu4           ");
            parameter.AppendSql("      ,Tax_Remark      =:Tax_Remark            ");
            parameter.AppendSql("      ,Tax_MailCode    =:Tax_MailCode          ");
            parameter.AppendSql("      ,Tax_Juso        =:Tax_Juso              ");
            parameter.AppendSql("      ,TAX_JusoDetail  =:TAX_JusoDetail        ");
            parameter.AppendSql("      ,DLtd            =:DLtd                  ");
            parameter.AppendSql("      ,ChulNotSayu     =:ChulNotSayu           ");
            parameter.AppendSql("      ,JSaupNo         =:JSaupNo               ");
            parameter.AppendSql(" WHERE ROWID           =:RID                   ");

            #region Query 변수대입
            parameter.Add("SANGHO", item.SANGHO);
            parameter.Add("NAME", item.NAME);
            parameter.Add("TEL", item.TEL);
            parameter.Add("FAX", item.FAX);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("MAILCODE", item.MAILCODE);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("JUSODETAIL", item.JUSODETAIL);
            parameter.Add("BuildNo", item.BUILDNO);
            parameter.Add("SAUPNO", item.SAUPNO);
            parameter.Add("UPTAE", item.UPTAE);
            parameter.Add("JONGMOK", item.JONGMOK);
            parameter.Add("DAEPYO", item.DAEPYO);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("JISA", item.JISA);
            parameter.Add("KIHO", item.KIHO);
            parameter.Add("UPJONG", item.UPJONG);
            parameter.Add("SANKIHO", item.SANKIHO);
            parameter.Add("GWANSE", item.GWANSE);
            parameter.Add("JIDOWON", item.JIDOWON);
            parameter.Add("BONAME", item.BONAME);
            parameter.Add("BOJIK", item.BOJIK);
            parameter.Add("GYUMOGBN", item.GYUMOGBN);
            parameter.Add("SELDATE", item.SELDATE);
            parameter.Add("GyeDate", item.GYEDATE);
            parameter.Add("GesiNo", item.GESINO);
            parameter.Add("DelDate", item.DELDATE);
            parameter.Add("JEPUM1", item.JEPUM1);
            parameter.Add("JEPUM2", item.JEPUM2);
            parameter.Add("JEPUM3", item.JEPUM3);
            parameter.Add("JEPUM4", item.JEPUM4);
            parameter.Add("JEPUM5", item.JEPUM5);
            parameter.Add("GbGemjin", item.GBGEMJIN);
            parameter.Add("GbChukJeng", item.GBCHUKJENG);
            parameter.Add("GbDaeHang", item.GBDAEHANG);
            parameter.Add("GbJonggum", item.GBJONGGUM);
            parameter.Add("GbGukgo", item.GBGUKGO);
            parameter.Add("JepumList", item.JEPUMLIST);
            parameter.Add("Remark", item.REMARK);
            parameter.Add("ARMY_HSP", item.ARMY_HSP);
            parameter.Add("negodate", item.NEGODATE);
            parameter.Add("mamt", item.MAMT);
            parameter.Add("famt", item.FAMT);
            parameter.Add("Inwon", item.INWON);
            parameter.Add("YOUNGUPSO", item.YOUNGUPSO);
            parameter.Add("SpchungGu", item.SPCHUNGGU);
            parameter.Add("HemsNo", item.HEMSNO);
            parameter.Add("HaRemark", item.HAREMARK);
            parameter.Add("GbSchool", item.GBSCHOOL);
            parameter.Add("GbGaResv", item.GBGARESV);
            parameter.Add("HTEL", item.HTEL);
            parameter.Add("HeaGajepsu1", item.HEAGAJEPSU1);
            parameter.Add("HeaGajepsu2", item.HEAGAJEPSU2);
            parameter.Add("HeaGajepsu3", item.HEAGAJEPSU3);
            parameter.Add("HeaGajepsu4", item.HEAGAJEPSU4);
            parameter.Add("Tax_Remark", item.TAX_REMARK);
            parameter.Add("Tax_MailCode", item.TAX_MAILCODE);
            parameter.Add("Tax_Juso", item.TAX_JUSO);
            parameter.Add("TAX_JusoDetail", item.TAX_JUSODETAIL);
            parameter.Add("DLtd", item.DLTD);
            parameter.Add("ChulNotSayu", item.CHULNOTSAYU);
            parameter.Add("JSaupNo", item.JSAUPNO);
            parameter.Add("RID", item.RID);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_LTD item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_LTD (                                                              ");
            parameter.AppendSql("       CODE,SANGHO,Name,TEL,FAX,EMAIL,MAILCODE,JUSO,jusodetail,SAUPNO,UPTAE                    ");
            parameter.AppendSql("     , JONGMOK,DAEPYO,JUMIN,Jisa,KIHO,UPJONG,SANKIHO,GWANSE,JIDOWON,BONAME,BOJIK               ");
            parameter.AppendSql("     , GYUMOGBN,SELDATE,GYEDATE,GesiNo,JEPUM1,JEPUM2,JEPUM3,JEPUM4,JEPUM5                      ");
            parameter.AppendSql("     , GBGEMJIN,GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,DELDATE,JEPUMLIST,GbGaResv              ");
            parameter.AppendSql("     , REMARK,ARMY_HSP,YOUNGUPSO,negodate,mamt,famt,Inwon,GbSchool,SpchungGu,HemsNo            ");
            parameter.AppendSql("     , HaRemark,HTEL,HeaGajepsu1,HeaGajepsu2,HeaGajepsu3,HeaGajepsu4,Tax_Remark,JSaupNo        ");
            parameter.AppendSql("     , BuildNo,TAX_MailCode,TAX_Juso,TAX_JusoDetail,DLtd,ChulNotSayu                           ");
            parameter.AppendSql(" ) VALUES (                                                                                    ");
            parameter.AppendSql("      :CODE,:SANGHO,:Name,:TEL,:FAX,:EMAIL,:MAILCODE,:JUSO,:jusodetail,:SAUPNO,:UPTAE          ");
            parameter.AppendSql("     ,:JONGMOK,:DAEPYO,:JUMIN,:Jisa,:KIHO,:UPJONG,:SANKIHO,:GWANSE,:JIDOWON,:BONAME,:BOJIK     ");
            parameter.AppendSql("     ,:GYUMOGBN,:SELDATE,:GYEDATE,:GesiNo,:JEPUM1,:JEPUM2,:JEPUM3,:JEPUM4,:JEPUM5              ");
            parameter.AppendSql("     ,:GBGEMJIN,:GBCHUKJENG,:GBDAEHANG,:GBJONGGUM,:GBGUKGO,:DELDATE,:JEPUMLIST,:GbGaResv       ");
            parameter.AppendSql("     ,:REMARK,:ARMY_HSP,:YOUNGUPSO,:negodate,:mamt,:famt,:Inwon,:GbSchool,:SpchungGu,:HemsNo   ");
            parameter.AppendSql("     ,:HaRemark,:HTEL,:HeaGajepsu1,:HeaGajepsu2,:HeaGajepsu3,:HeaGajepsu4,:Tax_Remark,:JSaupNo ");
            parameter.AppendSql("     ,:BuildNo,:TAX_MailCode,:TAX_Juso,:TAX_JusoDetail,:DLtd,:ChulNotSayu                      ");
            parameter.AppendSql(")  ");

            #region Query 변수대입
            parameter.Add("CODE", item.CODE);
            parameter.Add("SANGHO", item.SANGHO);
            parameter.Add("Name", item.NAME);
            parameter.Add("TEL", item.TEL);
            parameter.Add("FAX", item.FAX);
            parameter.Add("EMAIL", item.EMAIL);
            parameter.Add("MAILCODE", item.MAILCODE);
            parameter.Add("JUSO", item.JUSO);
            parameter.Add("jusodetail", item.JUSODETAIL);
            parameter.Add("SAUPNO", item.SAUPNO);
            parameter.Add("UPTAE", item.UPTAE);
            parameter.Add("JONGMOK", item.JONGMOK);
            parameter.Add("DAEPYO", item.DAEPYO);
            parameter.Add("JUMIN", item.JUMIN);
            parameter.Add("Jisa", item.JISA);
            parameter.Add("KIHO", item.KIHO);
            parameter.Add("UPJONG", item.UPJONG);
            parameter.Add("SANKIHO", item.SANKIHO);
            parameter.Add("GWANSE", item.GWANSE);
            parameter.Add("JIDOWON", item.JIDOWON);
            parameter.Add("BONAME", item.BONAME);
            parameter.Add("BOJIK", item.BOJIK);
            parameter.Add("GYUMOGBN", item.GYUMOGBN);
            parameter.Add("SELDATE", item.SELDATE);
            parameter.Add("GYEDATE", item.GYEDATE);
            parameter.Add("GesiNo", item.GESINO);
            parameter.Add("JEPUM1", item.JEPUM1);
            parameter.Add("JEPUM2", item.JEPUM2);
            parameter.Add("JEPUM3", item.JEPUM3);
            parameter.Add("JEPUM4", item.JEPUM4);
            parameter.Add("JEPUM5", item.JEPUM5);
            parameter.Add("GBGEMJIN", item.GBGEMJIN);
            parameter.Add("GBCHUKJENG", item.GBCHUKJENG);
            parameter.Add("GBDAEHANG", item.GBDAEHANG);
            parameter.Add("GBJONGGUM", item.GBJONGGUM);
            parameter.Add("GBGUKGO", item.GBGUKGO);
            parameter.Add("DELDATE", item.DELDATE);
            parameter.Add("JEPUMLIST", item.JEPUMLIST);
            parameter.Add("GbGaResv", item.GBGARESV);
            parameter.Add("REMARK", item.REMARK);
            parameter.Add("ARMY_HSP", item.ARMY_HSP);
            parameter.Add("YOUNGUPSO", item.YOUNGUPSO);
            parameter.Add("negodate", item.NEGODATE);
            parameter.Add("mamt", item.MAMT);
            parameter.Add("famt", item.FAMT);
            parameter.Add("Inwon", item.INWON);
            parameter.Add("GbSchool", item.GBSCHOOL);
            parameter.Add("SpchungGu", item.SPCHUNGGU);
            parameter.Add("HemsNo", item.HEMSNO);
            parameter.Add("HaRemark", item.HAREMARK);
            parameter.Add("HTEL", item.HTEL);
            parameter.Add("HeaGajepsu1", item.HEAGAJEPSU1);
            parameter.Add("HeaGajepsu2", item.HEAGAJEPSU2);
            parameter.Add("HeaGajepsu3", item.HEAGAJEPSU3);
            parameter.Add("HeaGajepsu4", item.HEAGAJEPSU4);
            parameter.Add("Tax_Remark", item.TAX_REMARK);
            parameter.Add("JSaupNo", item.JSAUPNO);
            parameter.Add("BuildNo", item.BUILDNO);
            parameter.Add("TAX_MailCode", item.TAX_MAILCODE);
            parameter.Add("TAX_Juso", item.TAX_JUSO);
            parameter.Add("TAX_JusoDetail", item.JUSODETAIL);
            parameter.Add("DLtd", item.DLTD);
            parameter.Add("ChulNotSayu", item.CHULNOTSAYU);

            #endregion
            return ExecuteNonQuery(parameter);
        }

        public long GetHicCount(long cODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) as CNT                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU           ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE              ");
            #region Query 변수대입
            parameter.Add("LTDCODE", cODE);
            #endregion
            return ExecuteScalar<long>(parameter);
        }

        public long GetHeaCount(long cODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) as CNT                 ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU           ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE              ");
            #region Query 변수대입
            parameter.Add("LTDCODE", cODE);
            #endregion
            return ExecuteScalar<long>(parameter);
        }

        public long GetMisuCount(long cODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) as CNT                 ");
            parameter.AppendSql("  FROM ADMIN.HIC_MISU_MST        ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE              ");
            #region Query 변수대입
            parameter.Add("LTDCODE", cODE);
            #endregion
            return ExecuteScalar<long>(parameter);
        }

        public int Delete_Tax_All(long argCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_LTD_TAX      ");
            parameter.AppendSql(" WHERE LTDCODE       =:LTDCODE          ");

            #region Query 변수대입
            parameter.Add("LTDCODE", argCode);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public Dictionary<string, object> SelMisuCount(long cODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_MISU_MST        ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE              ");

            #region Query 변수대입
            parameter.Add("LTDCODE", cODE);
            #endregion
            return ExecuteReaderSingle(parameter);
        }

        public Dictionary<string, object> SelHeaCount(long cODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU           ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE              ");

            #region Query 변수대입
            parameter.Add("LTDCODE", cODE);
            #endregion
            return ExecuteReaderSingle(parameter);
        }

        public Dictionary<string, object> SelHicCount(long cODE)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT(*) CNT                        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU               ");
            parameter.AppendSql(" WHERE 1 = 1                               ");
            parameter.AppendSql("   AND LTDCODE =:LTDCODE                  ");

            #region Query 변수대입
            parameter.Add("LTDCODE", cODE);
            #endregion
            return ExecuteReaderSingle(parameter);
        }

        public int Delete(HIC_LTD item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_LTD      ");
            parameter.AppendSql("   SET DelDate     = :DelDate        ");
            parameter.AppendSql(" WHERE ROWID       = :RID          ");

            #region Query 변수대입
            parameter.Add("DelDate", item.DELDATE);
            parameter.Add("RID", item.RID);
            #endregion
            return ExecuteNonQuery(parameter);
        }

        public HIC_LTD FindOne(string v)
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

            return ExecuteReaderSingle<HIC_LTD>(parameter);
        }

        public List<HIC_LTD> Read_Hic_Ltd(string NAME)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                  ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD         ");
            parameter.AppendSql(" WHERE Name LIKE :NAME             ");
            parameter.AppendSql(" ORDER BY NAME, CODE               ");

            parameter.AddLikeStatement("NAME", NAME.Trim());

            return ExecuteReader<HIC_LTD>(parameter);
        }


        public string READ_Ltd_One_Name(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD ");
            parameter.AppendSql(" WHERE CODE = :CODE        ");
            #region Query 변수대입
            parameter.Add("CODE", CODE);
            #endregion
            return ExecuteScalar<string>(parameter);
        }
    }
}
