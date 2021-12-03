namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    public class HicJepsuSunapRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSunapRepository()
        {

        }

        public List<HIC_JEPSU_SUNAP> GetItembyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT to_char(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.WRTNO        ");
            parameter.AppendSql("     , a.GJJONG, a.GJYEAR, a.GJBANGI, SUM(b.TOTAMT) TOTAMT     ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a                                 ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAP b                                 ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                          ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                       ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                    ");
            parameter.AppendSql(" GROUP BY a.JEPDATE, a.WRTNO, a.GJJONG, a.GJYEAR, a.GJBANGI    ");
            parameter.AppendSql(" ORDER BY a.JEPDATE, a.WRTNO, a.GJJONG, a.GJYEAR, a.GJBANGI    ");

            parameter.Add("PANO", nPano);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetWrtNoCodebyPtNo(string argPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO, b.CODE                     ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU    a          ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAPDTL b          ");
            parameter.AppendSql(" WHERE a.PTNO = :PTNO                      ");
            parameter.AppendSql("   AND a.GJJONG IN ('31','35')             ");
            parameter.AppendSql("   AND a.JEPDATE >= TRUNC(SYSDATE - 30)    ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                ");
            parameter.AppendSql(" ORDER BY a.WRTNO DESC                     ");

            parameter.Add("PTNO", argPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetItembyWrtNoCode(long nWRTNO, string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT A.WRTNO, A.JEPDATE, B.GBSELF        ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU    a          ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAPDTL b          ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                    ");
            parameter.AppendSql("   AND B.CODE = :CODE                      ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                   ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetItembySuDateMirNo(string strFDate, string strTDate, string strJong, string strJonggum, long nMirNo, string strSunap, string strBo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.GjJong,a.Pano,b.SName,a.SeqNo seqno,a.WRTNO,b.gjchasu,b.jonggumyn                     ");
            parameter.AppendSql("     , TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate,b.LtdCode                                       ");
            parameter.AppendSql("     , c.SunapAmt,c.TotAmt,c.JohapAmt                                                          ");
            parameter.AppendSql("     , c.LtdAmt,c.BoninAmt,c.MisuAmt                                                           ");
            parameter.AppendSql("     , c.HalinAmt,c.BogenAmt,b.UCodes,a.JobSabun                                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.SuDate >= TO_DATE(:FDATE','YYYY-MM-DD')                                               ");
            parameter.AppendSql("   AND a.SuDate <= TO_DATE(:TDATE','YYYY-MM-DD')                                               ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                    ");
            if (strSunap == "Y")
            {
                parameter.AppendSql("   AND a.SunapAmt <> 0                                                                     ");
            }           
            if (strJonggum == "Y")
            {
                parameter.AppendSql("   and b.jonggumyn = '1'                                                                   ");
            }
            if (strJong == "1")
            {
                parameter.AppendSql("   AND b.MIRNO1 = :MIRNO                                                                   ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("   AND b.MIRNO2 = :MIRNO                                                                   ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql("   AND b.MIRNO3 = :MIRNO                                                                   ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("   AND b.MIRNO5 = :MIRNO                                                                   ");
            }
            if (strBo == "Y")
            {
                parameter.AppendSql("   AND b.MURYOAM = 'Y'                                                                     ");
                parameter.AppendSql("   AND b.MURYOGBN > '0'                                                                    ");
            }
            parameter.AppendSql("ORDER BY b.GjJong,b.JepDate,a.Pano,a.SeqNo,a.WRTNO                                             ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("MIRNO", nMirNo);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetGongDanItem(List<string> jongSQL, string FstrCOLNM, string fstrJongSQL, string dtpFDate, string dtpTDate, string fstrJong, string cboJong, string txtBogun, string txtKiho, bool chkW_Am, string cboGbn, string cboGbnLen, string cboDent, bool rdoChasu2, bool rdoChasu3)
        {
            MParameter parameter = CreateParameter();

            if (fstrJong == "4")
            {
                parameter.AppendSql("SELECT             A.Mirno3, A.PANO, A.WRTNO, A.KIHO,A.GJJONG, SUM(B.JOHAPAMT) JOHAPAMT,SUM(B.LTDAMT) LTDAMT,A.DENTAMT                     ");
            }
            else if (fstrJong == "6")
            {
                parameter.AppendSql("SELECT             A.Mirno2, A.PANO, A.WRTNO, A.KIHO,A.GJJONG, SUM(B.JOHAPAMT) JOHAPAMT,SUM(B.LTDAMT) LTDAMT,A.DENTAMT                     ");
            }
            else
            {
                parameter.AppendSql("SELECT             A.Mirno1, A.PANO, A.WRTNO, A.KIHO,A.GJJONG, SUM(B.JOHAPAMT) JOHAPAMT,SUM(B.LTDAMT) LTDAMT,A.DENTAMT                     ");
            }
            
            parameter.AppendSql("                     , MIN(TO_CHAR(A.JEPDATE,'YYYY-MM-DD')) MINDATE                                                                                ");
            parameter.AppendSql("                     , MAX(TO_CHAR(A.JEPDATE,'YYYY-MM-DD')) MAXDATE                                                                                ");

            parameter.AppendSql("FROM                   HIC_JEPSU A,HIC_SUNAP B                                                                                                     ");
            parameter.AppendSql("WHERE                  A.JEPDATE>=TO_DATE(:FDATE,'YYYY-MM-DD')                                                                                     ");
            parameter.AppendSql("   AND                 A.JEPDATE<=TO_DATE(:TDATE,'YYYY-MM-DD')                                                                                     ");
            parameter.AppendSql("   AND                 A.DELDATE IS NULL                                                                                                           ");
            parameter.AppendSql("   AND                 A.PANO <> 999                                                                                                               ");

            if (fstrJongSQL != "")
            {
                parameter.AppendSql("   AND             A.GJJONG IN (:JONGSQL)                                                                                                      ");
            }
            else
            {
                if (fstrJong != "6")
                {
                    parameter.AppendSql("   AND         A.GJJONG IN (SELECT CODE FROM HIC_EXJONG WHERE MISUJONG = :STRJONG )                                                          ");
                }
            }
            parameter.AppendSql("   AND                 A.WRTNO=B.WRTNO(+)                                                                                                          ");
            parameter.AppendSql("   AND                 B.JOHAPAMT <> 0                                                                                                             ");
            
            if (fstrJong == "4")
            {
                parameter.AppendSql("   AND                 A.Mirno3 >= 1                                                                                                           ");
            }
            else if (fstrJong == "6")
            {
                parameter.AppendSql("   AND                 A.Mirno2 >= 1                                                                                                           ");
            }
            else
            {
                parameter.AppendSql("   AND                 A.Mirno1 >= 1                                                                                                           ");
            }                                                                                                       



            if (fstrJong != "6")
            {
                parameter.AppendSql("   AND               (A.MISUNO2 IS NULL OR A.MISUNO2 = 0)                                                                                      ");
            }
            else
            {
                parameter.AppendSql("   AND               (A.MISUNO3 IS NULL OR A.MISUNO3 = 0)                                                                                      ");
                parameter.AppendSql("   AND               A.GJCHASU ='1'                                                                                                            ");
                parameter.AppendSql("   AND               A.GBDENTAL ='Y'                                                                                                           ");
            }



            if (cboJong == "1")
            {
                if (rdoChasu2 == true)
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('13','43')                                                                                                   ");
                }
                else if (rdoChasu3 == true)
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('18','46')                                                                                                   ");
                }
                else
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('13','18','43','46')                                                                                         ");
                }

                if (cboDent == "직장")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('7','8')                                                                                          ");
                }
                else if (cboDent == "공교")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('5','6')                                                                                          ");
                }
                else if (cboDent == "지역")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('1','2','3')                                                                                      ");
                }
                else if (cboDent == "급여")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('9')                                                                                              ");
                }
            }


            else if (cboJong == "2")
            {
                if (rdoChasu2 == true)
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('12','42')                                                                                                   ");
                }
                else if (rdoChasu3 == true)
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('17','45')                                                                                                   ");
                }
                else
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('12','17','42','45')                                                                                         ");
                }

                if (cboDent == "직장")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('7','8')                                                                                          ");
                }
                else if (cboDent == "공교")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('5','6')                                                                                          ");
                }
                else if (cboDent == "지역")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('1','2','3')                                                                                      ");
                }
            }


            else if (cboJong == "3")
            {
                if (rdoChasu2 == true)
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('11','41')                                                                                                   ");
                }
                else if (rdoChasu3 == true)
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('16','44')                                                                                                   ");
                }
                else
                {
                    parameter.AppendSql("   AND           A.GJJONG IN ('11','16','41','44')                                                                                         ");
                }

                if (cboDent == "직장")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('7','8')                                                                                          ");
                }
                else if (cboDent == "공교")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('5','6')                                                                                          ");
                }
                else if (cboDent == "지역")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('1','2','3')                                                                                      ");
                }
                else if (cboDent == "급여")
                {
                    parameter.AppendSql("   AND           SUBSTR(A.GKIHO,1,1) IN ('9')                                                                                              ");
                }
            }
            else if (cboJong == "4")
            {
                parameter.AppendSql("   AND               A.GJJONG IN ('31','35')                                                                                                   ");
                if (txtBogun != "")
                {
                    parameter.AppendSql("   AND           A.BOGUNSO = :BOGUSO                                                                                                        ");
                }
                if (txtKiho != "")
                {
                    parameter.AppendSql("   AND           A.KIHO = :KIHO                                                                                                            ");
                }

                if (chkW_Am == true)
                {
                    parameter.AppendSql("   AND           ( SUBSTR(A.GBAM,1,1) + SUBSTR(A.GBAM,3,1) + SUBSTR(A.GBAM,5,1) + SUBSTR(A.GBAM,7,1) +                                     ");
                    parameter.AppendSql("                   SUBSTR(GBAM,9,1) + SUBSTR(GBAM,11,1) ) =1                                                                               ");
                    parameter.AppendSql("   AND             SUBSTR(A.GBAM,11,1) =1                                                                                                  ");
                }
            }
            else if (cboJong == "6")
            {
                if (cboGbnLen == "1")
                {
                    if (rdoChasu2 == true)
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('13','43')                                                                                                ");
                    }
                    else if (rdoChasu3 == true)
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('18','46')                                                                                                ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('13','18','43','46')                                                                                      ");
                    }
                }
                else if (cboGbnLen == "2")
                {
                    if (rdoChasu2 == true)
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('12','42')                                                                                                ");
                    }
                    else if (rdoChasu3 == true)
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('17','45')                                                                                                ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('12','17','42','45')                                                                                      ");
                    }
                }
                else if (cboGbnLen == "3")
                {
                    if (rdoChasu2 == true)
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('11','41')                                                                                                ");
                    }
                    else if (rdoChasu3 == true)
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('16','44')                                                                                                ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND          A.GJJONG IN ('11','16','41','44')                                                                                      ");
                    }
                }

                if (cboDent == "직장")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('7','8')                                                                                        ");
                }
                else if (cboDent == "공교")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('5','6')                                                                                        ");
                }
                else if (cboDent == "지역")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('1','2','3')                                                                                    ");
                }
                else if (cboDent == "급여")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('9')                                                                                            ");
                }
            }
            else
            {
                if (cboDent == "직장")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('7','8')                                                                                        ");
                    if (rdoChasu2 == true)
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('11','41')                                                                                                 ");
                    }
                    else if (rdoChasu3 == true)
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('16','44')                                                                                                 ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('11','16','41','44')                                                                                       ");
                    }
                }
                else if (cboDent == "공교")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('5','6')                                                                                        ");
                    if (rdoChasu2 == true)
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('12','42')                                                                                                 ");
                    }
                    else if (rdoChasu3 == true)
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('17','45')                                                                                                 ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('12','17','42','45')                                                                                       ");
                    }
                }
                else if (cboDent == "지역")
                {
                    parameter.AppendSql("   AND             SUBSTR(A.GKIHO,1,1) IN ('1','2','3')                                                                                    ");
                    if (rdoChasu2 == true)
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('13','43')                                                                                                 ");
                    }
                    else if (rdoChasu3 == true)
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('18','46')                                                                                                 ");
                    }
                    else
                    {
                        parameter.AppendSql("   AND         A.GJJONG IN ('13','18','43','46')                                                                                       ");
                    }
                }
            }


            if (fstrJong == "4")
            {
                parameter.AppendSql("GROUP BY               A.Mirno3,A.PANO,A.WRTNO,A.KIHO,A.GJJONG,A.DENTAMT                                                                       ");
                parameter.AppendSql("ORDER BY               A.Mirno3,A.PANO,A.WRTNO,A.KIHO                                                                                          ");
            }
            else if (fstrJong == "6")
            {
                parameter.AppendSql("GROUP BY               A.Mirno2,A.PANO,A.WRTNO,A.KIHO,A.GJJONG,A.DENTAMT                                                                       ");
                parameter.AppendSql("ORDER BY               A.Mirno2,A.PANO,A.WRTNO,A.KIHO                                                                                          ");
            }
            else
            {
                parameter.AppendSql("GROUP BY               A.Mirno1,A.PANO,A.WRTNO,A.KIHO,A.GJJONG,A.DENTAMT                                                                       ");
                parameter.AppendSql("ORDER BY               A.Mirno1,A.PANO,A.WRTNO,A.KIHO                                                                                          ");
            }
            
            parameter.Add("FDATE", dtpFDate);
            parameter.Add("TDATE", dtpTDate);

            if (fstrJongSQL != "")
            {
                parameter.AddInStatement("JONGSQL", jongSQL);
            }
            else
            {
                if (fstrJong != "6")
                {
                    parameter.Add("STRJONG", fstrJong);
                }
            }

            if (cboJong == "4")
            {
                if (txtBogun != "")
                {
                    parameter.Add("BOGUSO", txtBogun);
                }
                if (txtKiho != "")
                {
                    parameter.Add("KIHO", txtKiho);
                }
            }

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetInWonCash(string fstrFDate, string fstrTDate, List<string> WRTNO, long nMisuNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                 A.PANO,A.WRTNO,SUM(B.JOHAPAMT) JOHAPAMT,SUM(B.LTDAMT) LTDAMT,SUM(B.BOGENAMT) BOGENAMT                           ");

            parameter.AppendSql("FROM                   HIC_JEPSU A, HIC_SUNAP B                                                                                        ");

            parameter.AppendSql("WHERE                  A.JEPDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                                      ");
            parameter.AppendSql("       AND             A.JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                                                      ");
            parameter.AppendSql("       AND             A.WRTNO IN (:WRTNO)                                                                                             ");
            parameter.AppendSql("       AND             A.PANO <> 999                                                                                                   ");
            parameter.AppendSql("       AND             A.DELDATE IS NULL                                                                                               ");
            parameter.AppendSql("       AND             A.WRTNO=B.WRTNO(+)                                                                                              ");
            parameter.AppendSql("       AND             B.MISUNO4 = :MISUNO                                                                                             ");

            parameter.AppendSql("GROUP BY               A.PANO,A.WRTNO                                                                                                  ");
            parameter.AppendSql("ORDER BY               A.PANO                                                                                                          ");

            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);
            parameter.AddInStatement("WRTNO", WRTNO);
            parameter.Add("MISUNO", nMisuNo);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetInWonMoney(bool chkBogen, bool chkW_Am, string txtBogun, string txtKiho, string txtLtdCode1, bool chkGub, string dtpFDate, string dtpTDate)
        {
            MParameter parameter = CreateParameter();

            if (chkBogen == true)
            {
                parameter.AppendSql("SELECT             A.MIRNO4 MIRNO, A.BOGUNSO, A.PANO, A.WRTNO, A.GJJONG                             ");
            }                                                                                                                            
            else                                                                                                                         
            {                                                                                                                            
                parameter.AppendSql("SELECT             A.MIRNO5 MIRNO, A.BOGUNSO, A.PANO, A.WRTNO, A.GJJONG                             ");
            }

            parameter.AppendSql("                       , SUM(B.JOHAPAMT) JOHAPAMT, SUM(B.LTDAMT) LTDAMT, SUM(B.BOGENAMT) BOGENAMT       ");
            parameter.AppendSql("                       , MIN(TO_CHAR(A.JEPDATE, 'YYYY-MM-DD')) MINDATE                                  ");
            parameter.AppendSql("                       , MAX(TO_CHAR(A.JEPDATE,'YYYY-MM-DD')) MAXDATE                                   ");


            parameter.AppendSql("FROM                   HIC_JEPSU A, HIC_SUNAP B                                                         ");


            parameter.AppendSql("WHERE                  A.JEPDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("       AND             A.JEPDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                                       ");
            parameter.AppendSql("       AND             A.DELDATE IS NULL                                                                ");
            parameter.AppendSql("       AND             A.PANO <> 999                                                                    ");    // 전산실 연습 제외
            parameter.AppendSql("       AND             A.WRTNO=B.WRTNO(+)                                                               ");
            parameter.AppendSql("       AND             A.GJJONG IN('31','35')                                                           ");    // 보건소 미청구
            parameter.AppendSql("       AND             (A.MISUNO4 IS NULL OR A.MISUNO4 = 0)                                             ");    // 보건소 부담액이 있는것

            if(chkW_Am == true)
            {
                parameter.AppendSql("   AND             (SUBSTR(A.GBAM,1,1) + SUBSTR(A.GBAM,3,1) + SUBSTR(A.GBAM,5,1) +                  ");
                parameter.AppendSql("                   SUBSTR(A.GBAM,7,1) + SUBSTR(GBAM,9,1) + SUBSTR(GBAM,11,1)) = 1                   ");
                parameter.AppendSql("   AND             SUBSTR(A.GBAM,11,1) = 1                                                          ");
            }

            if(txtBogun != "")
            {
                parameter.AppendSql("   AND             A.BOGUNSO =:BOGEN                                                                ");
            }                                                                                                                            
                                                                                                                                         
            if (txtKiho != "")                                                                                                           
            {                                                                                                                            
                parameter.AppendSql("   AND             A.KIHO =:KIHO                                                                    ");
            }
            if(txtLtdCode1 != "")
            {
                parameter.AppendSql("   AND             A.KIHO =:LTDCODE1 OR A.LTDCODE = :LTDCODE1                                       ");
            }

            // 의료급여암
            if(chkGub == true)
            {
                parameter.AppendSql("   AND             A.GUBDAESANG ='Y'                                                                ");
                parameter.AppendSql("   AND             A.MIRNO5 > 0                                                                     ");    // 의료급여암
            }

            // 보건소암
            if(chkBogen == true)
            {
                parameter.AppendSql("   AND             MURYOAM ='Y'                                                                     ");
                parameter.AppendSql("   AND             MURYOGBN > '0'                                                                   ");
                parameter.AppendSql("   AND             A.MIRNO4 > 0                                                                     ");    // 보건소로 청구한것만
            }

            if(chkBogen == true)
            {
                parameter.AppendSql("   GROUP BY        A.MIRNO4,A.BOGUNSO,A.PANO,A.WRTNO,A.GJJONG                                       ");
                parameter.AppendSql("   ORDER BY        A.MIRNO4,A.BOGUNSO,A.PANO,A.WRTNO                                                ");
            }
            else
            {
                parameter.AppendSql("   GROUP BY        A.MIRNO5,A.BOGUNSO,A.PANO,A.WRTNO,A.GJJONG                                       ");
                parameter.AppendSql("   ORDER BY        A.MIRNO5,A.BOGUNSO,A.PANO,A.WRTNO                                                ");
            }



            parameter.Add("FDATE", dtpFDate);
            parameter.Add("TDATE", dtpTDate);

            if (txtBogun != "")
            {
                parameter.Add("BOGEN", txtBogun);
            }
            if (txtKiho != "")
            {
                parameter.Add("KIHO", txtKiho);
            }
            if (txtLtdCode1 != "")
            {
                parameter.Add("LTDCODE1", txtLtdCode1);
            }

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetSumTotAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT SUM(a.TOTAMT) TOTAMT, B.MIRNO1                                                      ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT SUM(a.TOTAMT) TOTAMT, B.MIRNO2                                                      ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT SUM(a.TOTAMT) TOTAMT, B.MIRNO3                                                      ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT SUM(a.TOTAMT) TOTAMT, B.MIRNO4                                                      ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(a.TOTAMT) TOTAMT, B.MIRNO5                                                      ");
            }

            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                      ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO1                                                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO2                                                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO3                                                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO4                                                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO5                                                                         ");
            }

            parameter.Add("WRTNO", wRTNO2);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetLtdAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT SUM(a.LTDAMT) LTDAMT, B.MIRNO1                                                      ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT SUM(a.LTDAMT) LTDAMT, B.MIRNO2                                                      ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT SUM(a.LTDAMT) LTDAMT, B.MIRNO3                                                      ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT SUM(a.LTDAMT) LTDAMT, B.MIRNO4                                                      ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(a.LTDAMT) LTDAMT, B.MIRNO5                                                      ");
            }

            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                      ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO1                                                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO2                                                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO3                                                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO4                                                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO5                                                                         ");
            }

            parameter.Add("WRTNO", wRTNO2);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }
        
        public HIC_JEPSU_SUNAP GetJohapAmtMirNo1byWrtNo2(long wRTNO2, string strGbJong, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, B.MIRNO1                                                  ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, B.MIRNO2                                                  ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, B.MIRNO3                                                  ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, B.MIRNO4                                                  ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, B.MIRNO5                                                  ");
            }
            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                      ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO1                                                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO2                                                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO3                                                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO4                                                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO5                                                                         ");
            }

            parameter.Add("WRTNO", wRTNO2);
            parameter.Add("GJJONG", strGbJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetBoninAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            { 
                parameter.AppendSql("SELECT SUM(a.BONINAMT) BONINAMT, b.MIRNO1                                                  ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT SUM(a.BONINAMT) BONINAMT, b.MIRNO2                                                  ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT SUM(a.BONINAMT) BONINAMT, b.MIRNO3                                                  ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT SUM(a.BONINAMT) BONINAMT, b.MIRNO4                                                  ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(a.BONINAMT) BONINAMT, b.MIRNO5                                                  ");
            }
            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                      ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO1                                                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO2                                                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO3                                                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO4                                                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO5                                                                         ");
            }

            parameter.Add("WRTNO", wRTNO2);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetSumJohapAmtMirNo3byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO1                                                  ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO2                                                  ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO3                                                  ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO4                                                  ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO5                                                  ");
            }

            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                      ");
            if (strGubun == "1")
            { 
                parameter.AppendSql(" GROUP BY b.MIRNO1                                                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO2                                                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO3                                                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO4                                                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO5                                                                         ");
            }

            parameter.Add("WRTNO", wRTNO2);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetSumJohapAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            MParameter parameter = CreateParameter();

            if (strGubun == "1")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO1                                                  ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO2                                                  ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO3                                                  ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO4                                                  ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql("SELECT SUM(a.JOHAPAMT) JOHAPAMT, b.MIRNO5                                                  ");
            }
            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql(" WHERE a.WRTNO = b.WRTNO(+)                                                                    ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                      ");
            if (strGubun == "1")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO1                                                                         ");
            }
            else if (strGubun == "2")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO2                                                                         ");
            }
            else if (strGubun == "3")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO3                                                                         ");
            }
            else if (strGubun == "4")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO4                                                                         ");
            }
            else if (strGubun == "5")
            {
                parameter.AppendSql(" GROUP BY b.MIRNO5                                                                         ");
            }

            parameter.Add("WRTNO", wRTNO2);
            parameter.Add("GJJONG", strGjJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetItembySuDateLtdCodeMirNo(string strFDate, string strTDate, long nLtdCode, string strJong, string strJonggum, long nMirNo, string strSunap, string strBo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.GjJong,a.Pano,b.SName,a.SeqNo seqno,a.WRTNO,b.gjchasu,b.jonggumyn                     ");
            parameter.AppendSql("     , TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate,b.LtdCode                                       ");
            parameter.AppendSql("     , c.SunapAmt,c.TotAmt,c.JohapAmt                                                          ");
            parameter.AppendSql("     , c.LtdAmt,c.BoninAmt,c.MisuAmt                                                           ");
            parameter.AppendSql("     , c.HalinAmt,c.BogenAmt,b.UCodes,a.JobSabun                                               ");
            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                        ");
            parameter.AppendSql("     , (select a.wrtno,max(a.seqno) seqno,sum(a.SunapAmt) sunapamt                             ");
            parameter.AppendSql("             , sum(a.TotAmt) totamt,sum(a.JohapAmt)  johapamt,sum(a.LtdAmt) ltdamt             ");
            parameter.AppendSql("             , sum(a.BoninAmt) boninamt,sum(a.MisuAmt) misuamt,sum( a.HalinAmt) halinamt       ");
            parameter.AppendSql("             , sum(a.BogenAmt) BogenAmt                                                        ");
            parameter.AppendSql("          from ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b                                ");
            parameter.AppendSql("         WHERE a.SuDate >= TO_DATE(:FDATE,'YYYY-MM-DD')                                        ");
            parameter.AppendSql("           AND a.SuDate <= TO_DATE(:TDATE,'YYYY-MM-DD')                                        ");
            parameter.AppendSql("           AND a.wrtno = b.wrtno(+)                                                            ");
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.AppendSql("           AND b.LTDCODE = :LTDCODE                                                        ");
            }
            parameter.AppendSql("         GROUP BY a.WRTNO ) c                                                                  ");
            parameter.AppendSql("WHERE a.SuDate >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                 ");
            parameter.AppendSql("  AND a.SuDate <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                 ");
            if (strSunap == "Y")
            {
                parameter.AppendSql("   AND a.SunapAmt <> 0                                                                     ");
            }
            parameter.AppendSql("  AND a.WRTNO = b.WRTNO(+)                                                                     ");
            parameter.AppendSql("  and a.wrtno = c.wrtno                                                                        ");
            parameter.AppendSql("  and a.seqno = c.seqno                                                                        ");
            parameter.AppendSql("  and c.totamt > 0                                                                             ");
            if (strJonggum == "Y")
            {
                parameter.AppendSql("   and b.jonggumyn = '1'                                                                   ");
            }
            if (strJong == "1")
            {
                parameter.AppendSql("   AND b.MIRNO1 = :MIRNO                                                                   ");
            }
            else if (strJong == "3")
            {
                parameter.AppendSql("   AND b.MIRNO2 = :MIRNO                                                                   ");
            }
            else if (strJong == "4")
            {
                parameter.AppendSql("   AND b.MIRNO3 = :MIRNO                                                                   ");
            }
            else if (strJong == "E")
            {
                parameter.AppendSql("   AND b.MIRNO5 = :MIRNO                                                                   ");
            }
            if (strBo == "Y")
            {
                parameter.AppendSql("   AND b.MURYOAM = 'Y'                                                                     ");
                parameter.AppendSql("   AND b.MURYOGBN > '0'                                                                    ");
            }
            parameter.AppendSql("ORDER BY b.GjJong,b.JepDate,a.Pano,a.SeqNo,a.WRTNO                                             ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);
            parameter.Add("MIRNO", nMirNo);
            if (!nLtdCode.IsNullOrEmpty() && nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public int GetCountbyWrtNoCode(long nWRTNO, string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU    a          ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAPDTL b          ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                    ");
            parameter.AppendSql("   AND B.CODE = :CODE                      ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                   ");

            parameter.Add("WRTNO", nWRTNO);
            parameter.Add("CODE", strCode, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetJohapAmtbyJepDAteMirNo1(string fRDATE, long argMirno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT GjJong,SUM(b.JohapAmt) JOHAPAMT                         ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU    a                              ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAPDTL b                              ");
            parameter.AppendSql(" WHERE a.Wrtno = b.Wrtno(+)                                    ");
            parameter.AppendSql("   AND  a.JepDate >= to_date(:FRDATE, 'YYYY-MM-DD')            ");
            parameter.AppendSql("   AND  a.JepDate <= TRUNC(SYSDATE)                            ");
            parameter.AppendSql("   AND  a.MIRNO1 = :MIRNO1                                     ");
            parameter.AppendSql("   AND  a.DelDate IS NULL                                      ");
            parameter.AppendSql(" GROUP BY GjJong                                               ");

            parameter.Add("FRDATE", fRDATE);
            parameter.Add("MIRNO1", argMirno);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public HIC_JEPSU_SUNAP GetItembyJepDateWrtNo(long argWRTNO, string strJepDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.GjJong,a.Pano,b.SName,a.SeqNo seqno,a.WRTNO,b.gjchasu,b.jonggumyn                 ");
            parameter.AppendSql("      , TO_CHAR(b.JepDate,'YYYY-MM-DD') JepDate,b.LtdCode, c.SunapAmt,c.TotAmt,c.JohapAmt  ");
            parameter.AppendSql("      , c.BogenAmt, c.LtdAmt,c.BoninAmt,c.MisuAmt, c.HalinAmt,b.UCodes,a.JobSabun          ");
            parameter.AppendSql("      , b.BOGUNSO, b.muryoam                                                               ");
            parameter.AppendSql("   FROM ADMIN.HIC_SUNAP a, ADMIN.HIC_JEPSU b,                                  ");
            parameter.AppendSql("        (select a.wrtno,max(a.seqno) seqno,sum(a.SunapAmt) sunapamt                        ");
            parameter.AppendSql("              , sum(a.TotAmt) totamt,sum(a.JohapAmt)  johapamt,sum(a.LtdAmt) ltdamt        ");
            parameter.AppendSql("              , sum(a.BoninAmt) boninamt,sum(a.MisuAmt) misuamt,sum( a.HalinAmt) halinamt  ");
            parameter.AppendSql("              , sum(a.bogenamt) bogenamt                                                   ");
            parameter.AppendSql("            from ADMIN.HIC_SUNAP a, ADMIN.hic_jepsu b                          ");
            parameter.AppendSql("           WHERE b.Jepdate >= to_date(:JEPDATE, 'YYYY-MM-DD')                              ");
            parameter.AppendSql("             and a.WRTNO = b.wrtno(+)                                                      ");
            parameter.AppendSql("             and a.WRTNO = :WRTNO                                                          ");
            parameter.AppendSql("           group by a.wrtno                                                                ");
            parameter.AppendSql("         ) c                                                                               ");
            parameter.AppendSql(" WHERE b.Jepdate >= to_date(:JEPDATE, 'YYYY-MM-DD')                                        ");
            parameter.AppendSql("   and  a.WRTNO=b.WRTNO(+)                                                                 ");
            parameter.AppendSql("   and a.wrtno =c.wrtno                                                                    ");
            parameter.AppendSql("   and a.seqno = c.seqno                                                                   ");
            parameter.AppendSql("   and c.totamt > 0                                                                        ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                                                    ");
            parameter.AppendSql(" ORDER BY b.GjJong,b.JepDate,a.Pano,a.SeqNo,a.WRTNO                                        ");

            parameter.Add("WRTNO", argWRTNO);
            parameter.Add("JEPDATE", strJepDate);

            return ExecuteReaderSingle<HIC_JEPSU_SUNAP>(parameter);
        }

        public int GetCountWrtNo(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT COUNT('X') CNT                                              ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU    a                                  ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAPDTL b                                  ");
            parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                            ");
            parameter.AppendSql("   AND a.Wrtno = b.Wrtno(+)                                        ");
            parameter.AppendSql("   AND b.Code IN ( SELECT CODE FROM HIC_CODE WHERE GUBUN ='98' )   ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteScalar<int>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetCodebyPtNo(string argPtNo)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.CODE                          ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU_WORK a    ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAPDTL_WORK b ");
            parameter.AppendSql(" WHERE a.PTNO = :PTNO                  ");
            parameter.AppendSql("   AND a.GJJONG IN ('31','35')         ");
            parameter.AppendSql("   AND a.PANO   = b.PANO(+)            ");
            parameter.AppendSql("   AND a.GJJONG = b.GJJONG             ");

            parameter.Add("PTNO", argPtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetListSum(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero, string strSunap)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FC_HIC_GJJONG_NAME(b.GJJONG, b.UCODES) AS GJNAME, a.PANO,b.SNAME                                    ");
            parameter.AppendSql("      ,a.SEQNO, a.WRTNO, b.GJCHASU, b.KIHO, b.PTNO, b.REMARK                                               ");
            parameter.AppendSql("      ,TO_CHAR(b.JEPDATE, 'MM/DD') JEPDATE_MMDD, b.GJJONG                                                  ");
            parameter.AppendSql("      ,TO_CHAR(a.SUDATE, 'MM/DD') SUDATE_MMDD, b.LTDCODE, c.LTDAMT                                         ");
            parameter.AppendSql("      ,c.SUNAPAMT AS SUNAPAMT1, c.SUNAPAMT2, c.TOTAMT, c.JOHAPAMT                                          ");
            parameter.AppendSql("      ,c.BONINAMT, c.MISUAMT, c.HALINAMT, b.UCODES, a.JOBSABUN                                             ");
            parameter.AppendSql("      ,DECODE(b.JONGGUMYN, '1', 'Y', '') AS JONGGUMYN, a.BOGENAMT                                          ");
            parameter.AppendSql("      ,ADMIN.FC_HC_PATIENT_JUMINNO(b.PTNO) AS JUMINNO                                                ");
            parameter.AppendSql("      ,ADMIN.FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME                                                    ");
            parameter.AppendSql("      ,ADMIN.FC_INSA_MST_KORNAME(a.JOBSABUN) JOBNAME                                                  ");
            parameter.AppendSql("      ,CASE WHEN a.SUNAPAMT2 > 0 THEN '◎' ELSE '' END AS GBCARD                                            ");
            parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a                                                                             ");
            parameter.AppendSql("     , ADMIN.HIC_JEPSU b                                                                             ");
            parameter.AppendSql("     ,(                                                                                                    ");
            parameter.AppendSql("       SELECT a.WRTNO, MAX(a.SEQNO) AS SEQNO, SUM(a.SUNAPAMT) AS SUNAPAMT, SUM(a.SUNAPAMT2) AS SUNAPAMT2   ");
            parameter.AppendSql("             ,SUM(a.TOTAMT) AS TOTAMT, SUM(a.JOHAPAMT) AS JOHAPAMT, SUM(a.LTDAMT) AS LTDAMT                ");
            parameter.AppendSql("             ,SUM(a.BOGENAMT) AS BOGENAMT, SUM(a.BONINAMT) AS BONINAMT, SUM(a.MISUAMT) AS MISUAMT          ");
            parameter.AppendSql("             ,SUM(a.HALINAMT) AS HALINAMT                                                                  ");
            parameter.AppendSql("         FROM HIC_SUNAP A, HIC_JEPSU B                                                                     ");
            parameter.AppendSql("        WHERE a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("          AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                     ");
            parameter.AppendSql("          AND a.WRTNO = b.WRTNO(+)                                                                         ");
            parameter.AppendSql("          AND a.WRTNO > 0                                                                                  ");
            parameter.AppendSql("        GROUP BY a.WRTNO                                                                                   ");
            parameter.AppendSql("      ) c                                                                                                  ");
            if (strSunap != "0")
            {
                parameter.AppendSql("     , ADMIN.INSA_MST d                                                                           ");
            }
            parameter.AppendSql(" WHERE 1 = 1                                                                                               ");
            parameter.AppendSql("   AND A.PANO NOT IN (991,992,993,994,995,996,997,998,999)                                                 ");
            parameter.AppendSql("   AND a.SUDATE >= TO_DATE(:FDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND a.SUDATE <= TO_DATE(:TDATE,'YYYY-MM-DD')                                                            ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                                ");
            parameter.AppendSql("   AND a.WRTNO = c.WRTNO                                                                                   ");
            parameter.AppendSql("   AND a.SEQNO = c.SEQNO                                                                                   ");
            parameter.AppendSql("   AND c.TOTAMT > 0                                                                                        ");
            if (strSunap == "1")    //수납구분(건진센터)
            {
                parameter.AppendSql("   AND a.jobsabun = d.sabun(+)                                                                         ");
                parameter.AppendSql("   AND (D.buse not in ('077402','077400','077401','077404','077403')                                   ");
                parameter.AppendSql("    OR a.JobSabun = 111 OR a.JobSabun = 222)                                                           ");
            }
            else if (strSunap == "2")   //수납구분(원무과)
            {
                parameter.AppendSql("   AND a.jobsabun = d.sabun(+)                                                                         ");
                parameter.AppendSql("   AND D.buse in ('077402','077400','077401','077404','077403')                                        ");
                parameter.AppendSql("   AND a.JobSabun NOT IN (111,222)                                                                     ");
            }

            if (fbZero)
            {
                parameter.AppendSql("   AND a.SUNAPAMT > 0                                                                                  ");
            }

            if (!fstrJong.Equals("**"))
            {
                parameter.AppendSql("   AND b.GJJONG = :GJJONG                                                                              ");
            }

            if (fstrJob.Equals("2"))
            {
                parameter.AppendSql("   AND CHUNGGUYN IS NULL                                                                               ");
            }
            else if (fstrJob.Equals("3"))
            {
                parameter.AppendSql("   AND CHUNGGUYN ='Y'                                                                                  ");
            }

            if (fnLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                                                                               ");
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                                                                            ");
            }

            parameter.AppendSql(" ORDER BY b.GJJONG, b.JEPDATE, a.PANO, a.WRTNO, a.SeqNo                                                    ");

            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);

            if (!fstrJong.Equals("**"))
            {
                parameter.Add("GJJONG", fstrJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (fnLtdCode > 0)
            {
                parameter.Add("LTDCODE", fnLtdCode);
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", fstrSName);
            }

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetListAll(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero, string strSunap)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT FC_HIC_GJJONG_NAME(b.GJJONG, b.UCODES) AS GJNAME, a.PANO,b.SNAME    ");
            parameter.AppendSql("      ,a.SEQNO, a.WRTNO, b.GJCHASU, b.KIHO, b.PTNO, b.REMARK               ");
            parameter.AppendSql("      ,TO_CHAR(b.JEPDATE, 'MM/DD') JEPDATE_MMDD, b.GJJONG                  ");
            parameter.AppendSql("      ,TO_CHAR(a.SUDATE, 'MM/DD') SUDATE_MMDD, b.LTDCODE, a.LTDAMT         ");
            parameter.AppendSql("      ,a.SUNAPAMT AS SUNAPAMT1, a.SUNAPAMT2, a.TOTAMT, a.JOHAPAMT          ");
            parameter.AppendSql("      ,a.BONINAMT, a.MISUAMT, a.HALINAMT, b.UCODES, a.JOBSABUN             ");
            parameter.AppendSql("      ,DECODE(b.JONGGUMYN, '1', 'Y', '') AS JONGGUMYN, a.BOGENAMT          ");
            parameter.AppendSql("      ,ADMIN.FC_HC_PATIENT_JUMINNO(b.PTNO) AS JUMINNO                ");
            parameter.AppendSql("      ,ADMIN.FC_HIC_LTDNAME(b.LTDCODE) AS LTDNAME                    ");
            parameter.AppendSql("      ,ADMIN.FC_INSA_MST_KORNAME(a.JOBSABUN) JOBNAME                  ");
            parameter.AppendSql("      ,CASE WHEN a.SUNAPAMT2 > 0 THEN '◎' ELSE '' END AS GBCARD            ");
            if (strSunap == "0")    //수납구분(전체)
            {
                parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a                                         ");
                parameter.AppendSql("     , ADMIN.HIC_JEPSU b                                         ");
            }
            else
            {
                parameter.AppendSql("  FROM ADMIN.HIC_SUNAP a                                         ");
                parameter.AppendSql("     , ADMIN.HIC_JEPSU b                                         ");
                parameter.AppendSql("     , ADMIN.INSA_MST   d                                         ");
            }
            parameter.AppendSql(" WHERE 1 = 1                                                               ");
            parameter.AppendSql("   AND A.PANO NOT IN (991,992,993,994,995,996,997,998,999)                 ");
            parameter.AppendSql("   AND a.SUDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND a.SUDATE <= TO_DATE(:TDATE, 'YYYY-MM-DD')                           ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                ");
            if (strSunap == "1")    //수납구분(건진센터)
            {
                parameter.AppendSql("   AND a.jobsabun = d.sabun(+)                                         ");
                parameter.AppendSql("   AND (d.buse not in ('077402','077400','077401','077404','077403')   ");
                parameter.AppendSql("    OR a.JobSabun = 111 OR a.JobSabun = 222)                           ");
            }
            else if (strSunap == "2")    //수납구분(원무과)
            {
                parameter.AppendSql("   AND a.jobsabun = d.sabun(+)                                         ");
                parameter.AppendSql("   AND d.buse in ('077402','077400','077401','077404','077403')        ");
                parameter.AppendSql("   AND a.JobSabun NOT IN (111, 222)                                    ");
            }
            if (fbZero)
            {
                parameter.AppendSql("   AND a.SUNAPAMT <> 0                                                 ");
            }

            if (!fstrJong.Equals("**"))
            {
                parameter.AppendSql("   AND b.GJJONG = :GJJONG                                              ");
            }

            if (fstrJob.Equals("2"))
            {
                parameter.AppendSql("   AND CHUNGGUYN IS NULL                                               ");
            }
            else if (fstrJob.Equals("3"))
            {
                parameter.AppendSql("   AND CHUNGGUYN ='Y'                                                  ");
            }

            if (fnLtdCode > 0)
            {
                parameter.AppendSql("   AND LTDCODE =:LTDCODE                                               ");
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND b.SNAME LIKE :SNAME                                            ");
            }

            parameter.AppendSql(" ORDER BY b.GJJONG, b.JEPDATE, a.PANO, a.WRTNO, a.SEQNO                    ");

            parameter.Add("FDATE", fstrFDate);
            parameter.Add("TDATE", fstrTDate);

            if (!fstrJong.Equals("**"))
            {
                parameter.Add("GJJONG", fstrJong, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            }

            if (fnLtdCode > 0)
            {
                parameter.Add("LTDCODE", fnLtdCode);
            }

            if (!fstrSName.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("SNAME", fstrSName);
            }

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }

        public List<HIC_JEPSU_SUNAP> GetUnionItembyPaNo(long nPano)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT '' GUBUN, to_char(a.JEPDATE,'YYYY-MM-DD') JEPDATE, a.WRTNO                          ");
            parameter.AppendSql("     , a.GJJONG, a.GJYEAR, a.GJBANGI, SUM(b.TOTAMT) TOTAMT, SECOND_SAYU                    ");
            parameter.AppendSql("     , a.JUSO1, a.JUSO2                                                                    ");
            parameter.AppendSql("  FROM ADMIN.HIC_JEPSU a                                                             ");
            parameter.AppendSql("     , ADMIN.HIC_SUNAP b                                                             ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                                      ");
            parameter.AppendSql("   AND (a.DelDate IS NULL OR a.DelDate ='')                                                ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql(" GROUP BY a.JEPDATE, a.WRTNO, a.GJJONG, a.GJYEAR, a.GJBANGI,SECOND_SAYU, a.JUSO1, a.JUSO2  ");
            parameter.AppendSql(" UNION ALL                                                                                 ");
            parameter.AppendSql("SELECT '종검' GUBUN, to_char(a.SDATE,'YYYY-MM-DD') JEPDATE, a.WRTNO                        ");
            parameter.AppendSql("     , a.GJJONG,'' GJYEAR, '' GJBANGI, SUM(b.TOTAMT) TOTAMT, '' SECOND_SAYU                ");
            parameter.AppendSql("     , a.JUSO1, a.JUSO2                                                                    ");
            parameter.AppendSql("  FROM ADMIN.HEA_JEPSU a                                                             ");
            parameter.AppendSql("     , ADMIN.HEA_SUNAP b                                                             ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                                      ");
            parameter.AppendSql("   AND (a.DelDate IS NULL OR a.DelDate ='')                                                ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO(+)                                                                ");
            parameter.AppendSql(" GROUP BY a.SDATE, a.WRTNO, a.GJJONG, a.JUSO1, a.JUSO2                                     ");
            parameter.AppendSql(" ORDER BY JEPDATE DESC, WRTNO, GJJONG                                                      ");

            parameter.Add("PANO", nPano);

            return ExecuteReader<HIC_JEPSU_SUNAP>(parameter);
        }
    }
}
