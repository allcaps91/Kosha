namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class MisuTaxRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public MisuTaxRepository()
        {
        }

        public List<MISU_TAX> GetNote(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("   SELECT * ");

            parameter.AppendSql("   FROM " + ComNum.DB_PMPA + "MISU_TAX");

            parameter.AppendSql("   WHERE      MisuNo = :nWrtno  ");

            parameter.Add("nWrtno", nWrtno);


            return ExecuteReader<MISU_TAX>(parameter);
        }

        public int CreateViewBill(string dtpFDate, string dtpTDate, string strTaxNo, string strJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("CREATE OR REPLACE VIEW         ADMIN.VIEW_TAXCASH_VIEW (BUNAME, LTD, NAME, UPTAE, JONGMOK, GBTRB, GAMT, GBN, BDATE, LTDCODE, NAME2, LTDCODE2, LTD2, UPTAE2, JONGMOK2, WRTNO, TRBNO) AS");
            // 건진센터 계산서
            parameter.AppendSql("SELECT                         '건진센터',A.LTDNO, A.LTDNAME, A.UPTAE, A.JONGMOK,A.GBTRB, AMT, '2', BDATE, A.LTDCODE, A.LTDNAME2, A.LTDCODE2, A.LTDNO2 , A.UPTAE2, A.JONGMOK2, A.WRTNO,A.TRBNO ");
            parameter.AppendSql("FROM                            ADMIN.MISU_TAX A                                                                                                                                         ");
            parameter.AppendSql("WHERE                           A.BDate >= TO_DATE('" + dtpFDate + "', 'YYYY-MM-DD')                                                                                                           ");
            parameter.AppendSql("   AND                          A.BDate <= TO_DATE('" + dtpTDate + "', 'YYYY-MM-DD')                                                                                                           ");
            parameter.AppendSql("   AND                         (A.DELDATE IS NULL OR A.DELDATE  = '')                                                                                                                          ");
            
            if(strTaxNo != "")
            {
                parameter.AppendSql("   AND                      A.LTDNO = '" + strTaxNo + "'                                                                                                                                   ");
            }
            if(strJong != "*")
            {
                parameter.AppendSql("   AND                      A.MJong = '" + strJong + "'                                                                                                                                    ");
            }

            parameter.AppendSql("UNION ALL");

            // 건진센터 계산서 국세청전송(+)

            parameter.AppendSql("SELECT                         '건진센터',A.LTDNO, A.LTDNAME, A.UPTAE, A.JONGMOK,A.GBTRB, AMT, '2', BDATE, A.LTDCODE, A.LTDNAME2, A.LTDCODE2, A.LTDNO2, A.UPTAE2, A.JONGMOK2, A.WRTNO, A.TRBNO ");
            parameter.AppendSql("FROM                            ADMIN.MISU_TAX A                                                                                                                                         ");
            parameter.AppendSql("WHERE                           A.BDate >= TO_DATE('" + dtpFDate + "', 'YYYY-MM-DD')                                                                                                           ");
            parameter.AppendSql("   AND                          A.BDate <= TO_DATE('" + dtpTDate + "', 'YYYY-MM-DD')                                                                                                           ");
            parameter.AppendSql("   AND                          A.DELDATE IS NOT NULL                                                                                                                                          ");
            parameter.AppendSql("   AND                          A.BMDATE IS NOT NULL                                                                                                                                           ");

            if (strTaxNo != "")
            {
                parameter.AppendSql("   AND                      A.LTDNO = '" + strTaxNo + "'                                                                                                                                   ");
            }
            if (strJong != "*")
            {
                parameter.AppendSql("   AND                      A.MJong = '" + strJong + "'                                                                                                                                    ");
            }

            parameter.AppendSql("UNION ALL");

            // 건진센터 계산서 국세청전송(-)

            parameter.AppendSql("SELECT                         '건진센터',A.LTDNO, A.LTDNAME, A.UPTAE, A.JONGMOK,A.GBTRB,(AMT * -1), '2', BMDATE, A.LTDCODE, A.LTDNAME2, A.LTDCODE2, A.LTDNO2, A.UPTAE2, A.JONGMOK2, A.WRTNO, A.TRBNO ");
            parameter.AppendSql("FROM                            ADMIN.MISU_TAX A                                                                                                                                         ");
            parameter.AppendSql("WHERE                           A.BDate >= TO_DATE('" + dtpFDate + "', 'YYYY-MM-DD')                                                                                                           ");
            parameter.AppendSql("   AND                          A.BDate <= TO_DATE('" + dtpTDate + "', 'YYYY-MM-DD')                                                                                                           ");
            parameter.AppendSql("   AND                          A.DELDATE IS NOT NULL                                                                                                                                          ");
            parameter.AppendSql("   AND                          A.BMDATE IS NOT NULL                                                                                                                                           ");

            if (strTaxNo != "")
            {
                parameter.AppendSql("   AND                      A.LTDNO = '" + strTaxNo + "'                                                                                                                                   ");
            }
            if (strJong != "*")
            {
                parameter.AppendSql("   AND                      A.MJong = '" + strJong + "'                                                                                                                                    ");
            }

            return ExecuteNonQuery(parameter);
        }

        public int initBill(MISU_TAX initItem)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO    MISU_TAX                                                                                               ");
            parameter.AppendSql("             ( GJYEAR,WRTNO,BDATE,GBBUSE,MJONG,LTDCODE,LTDNAME,LTDNO,DAEPYONAME                                       ");
            parameter.AppendSql("             , LTDJUSO,UPTAE,JONGMOK,MISUNO,MIRNO,AMT,ENTDATE,ENTSABUN,REMARK,PUMMOK,GBTRB,TRBNO,TRBNO2               ");

            parameter.AppendSql("               ) VALUES (                                                                                             ");

            parameter.AppendSql("               :GJYEAR, :WRTNO, TO_DATE(:BDATE, 'YYYY-MM-DD'), :GBBUSE, :MJONG, :LTDCODE, :LTDNAME, :LTDNO, :DAEPYONAME       ");
            parameter.AppendSql("             , :LTDJUSO, :UPTAE, :JONGMOK, :MISUNO, :MIRNO, :AMT, SYSDATE, :ENTSABUN, :REMARK, :PUMMOK, :GBTRB, :TRBNO, :TRBNO2 ) ");

            parameter.Add("GJYEAR", initItem.GJYEAR);
            parameter.Add("WRTNO", initItem.WRTNO);
            parameter.Add("BDATE", initItem.BDATE);
            parameter.Add("GBBUSE", initItem.GBBUSE);
            parameter.Add("MJONG", initItem.MJONG);
            parameter.Add("LTDCODE", initItem.LTDCODE);
            parameter.Add("LTDNAME", initItem.LTDNAME);
            parameter.Add("LTDNO", initItem.LTDNO);
            parameter.Add("DAEPYONAME", initItem.DAEPYONAME);
            parameter.Add("LTDJUSO", initItem.LTDJUSO);
            parameter.Add("UPTAE", initItem.UPTAE);
            parameter.Add("JONGMOK", initItem.JONGMOK);
            parameter.Add("MISUNO", initItem.MISUNO);
            parameter.Add("MIRNO", initItem.MIRNO);
            parameter.Add("AMT", initItem.AMT);
            parameter.Add("ENTSABUN", initItem.ENTSABUN);
            parameter.Add("REMARK", initItem.REMARK);
            parameter.Add("PUMMOK", initItem.PUMMOK);
            parameter.Add("GBTRB", initItem.GBTRB);
            parameter.Add("TRBNO", initItem.TRBNO);
            parameter.Add("TRBNO2", initItem.TRBNO2);

            return ExecuteNonQuery(parameter);
        }

        public List<MISU_TAX> GetWrtno(string strGjyear)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT     MAX(WRTNO) TAXWRTNO FROM MISU_TAX       ");
            parameter.AppendSql("WHERE      GJYEAR = :GJYEAR                        ");

            parameter.Add("GJYEAR", strGjyear, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            return ExecuteReader<MISU_TAX>(parameter);
        }

        public int UpdateMisuTax(MISU_TAX item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE         MISU_TAX SET                ");
            parameter.AppendSql("               LTDNO2 = :LTDNO             ");
            parameter.AppendSql("             , LTDNAME2 = :LTDNAME         ");
            parameter.AppendSql("             , LTDCODE2 = :LTDCODE         ");
            parameter.AppendSql("             , UPTAE2 = :UPTAE             ");
            parameter.AppendSql("             , JONGMOK2 = :JONGMOK         ");

            parameter.AppendSql("WHERE          WRTNO = :WRTNO              ");
            parameter.AppendSql("   AND         GJYEAR = :GJYEAR            ");

            parameter.Add("LTDNO", item.LTDNO2);
            parameter.Add("LTDNAME", item.LTDNAME2);
            parameter.Add("LTDCODE", item.LTDCODE2);
            parameter.Add("UPTAE", item.UPTAE2);
            parameter.Add("JONGMOK", item.JONGMOK2);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("GJYEAR", item.GJYEAR);

            return ExecuteNonQuery(parameter);
        }

        public int GetSALEEBILL(int val)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT             RESSEQ, LOADSTATUS         ");
            parameter.AppendSql("FROM               ADMIN.TRB_SALEEBILL   ");
            parameter.AppendSql("WHERE              RESSEQ = :VAL              ");
            parameter.AppendSql("   AND             LOADSTATUS <> '1'          ");

            parameter.Add("VAL", val);

            return ExecuteNonQuery(parameter);
        }

        public void DropView()
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DROP VIEW ADMIN.VIEW_TAXCASH_VIEW   ");

            ExecuteNonQuery(parameter);
        }

        public List<MISU_TAX> GetViewItem(bool rdoSort1, bool rdoSort2)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                          LTD, NAME, UPTAE, JONGMOK,GBTRB, GAMT,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, LTDCODE, NAME2, LTDCODE2, LTD2, UPTAE2, JONGMOK2, WRTNO, TRBNO    ");

            parameter.AppendSql("FROM                            ADMIN.VIEW_TAXCASH_VIEW                                                                                                               ");
                                                                 
            if (rdoSort1 == true)
            {
                parameter.AppendSql("ORDER BY                    NAME,LTD                                                                                                                                   ");
            }
            else if(rdoSort2 == true)
            {
                parameter.AppendSql("ORDER BY                    LTD,NAME,BDATE                                                                                                                             ");
            }
            else
            {
                parameter.AppendSql("ORDER BY                    BDATE,NAME                                                                                                                                 ");
            }

            return ExecuteReader<MISU_TAX>(parameter);
        }
    }
}
