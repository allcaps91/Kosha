namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class HicLtdTaxRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicLtdTaxRepository()
        {
        }

        public IList<HIC_LTD_TAX> ViewData(string strKeyWord)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT LTDCODE,BUSE,DAMNAME,DAMJIK,TEL,HPHONE,EMAIL,REMARK,ROWID AS RID  ");
            parameter.AppendSql("  FROM ADMIN.HIC_LTD_TAX                                     ");
            parameter.AppendSql(" WHERE 1 = 1                                                       ");
            parameter.AppendSql("  AND LTDCODE  =:LTDCODE                                           ");

            parameter.Add("LTDCODE", strKeyWord);

            return ExecuteReader<HIC_LTD_TAX>(parameter);
        }

        public int Delete_Tax_One(string rOWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE ADMIN.HIC_LTD_TAX    ");
            parameter.AppendSql(" WHERE ROWID =:RID                ");

            #region Query 변수대입
            parameter.Add("RID", rOWID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_LTD_TAX> GetDamDangJa(string txtLtdCode, string gJONG)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT         DAMNAME,TEL,HPHONE,EMAIL   ");
            parameter.AppendSql("  FROM         ADMIN.HIC_LTD_TAX    ");
            parameter.AppendSql(" WHERE         LTDCODE = :LTDCODE         ");

            if (gJONG == "83")
            {
                parameter.AppendSql(" AND       BUSE='1'                   ");
            }
            else if (gJONG == "81")
            {
                parameter.AppendSql(" AND       BUSE='3'                   ");
            }
            else if (gJONG == "82")
            {
                parameter.AppendSql(" AND       BUSE='4'                   ");
            }
            else
            {
                parameter.AppendSql(" AND       BUSE='2'                   ");
            }

            parameter.Add("LTDCODE", txtLtdCode);

            return ExecuteReader<HIC_LTD_TAX>(parameter);
        }

        public List<HIC_LTD_TAX> GetTaxDamDang(long DLTD, long LTDCODE, string GJONG)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT            DAMNAME,TEL,HPHONE,EMAIL         ");
            parameter.AppendSql("FROM              ADMIN.HIC_LTD_TAX          ");
            
            if(DLTD > 0)
            {
                parameter.AppendSql("WHERE         LTDCODE = :DLTD                  ");
            }
            else
            {
                parameter.AppendSql("WHERE         LTDCODE = :LTDCODE               ");
            }

            if(GJONG == "83")
            {
                parameter.AppendSql("AND           BUSE='1'                         ");  // 종검
            }
            else if(GJONG == "81")
            {
                parameter.AppendSql("AND           BUSE='3'                         ");  // 측정
            }
            else if (GJONG == "82")
            {
                parameter.AppendSql("AND           BUSE='4'                         ");  // 보건관리대행
            }
            else
            {
                parameter.AppendSql("AND           BUSE='2'                         ");  // 일반건진
            }



            if (DLTD > 0)
            {
                parameter.Add("DLTD", DLTD);
            }
            else
            {
                parameter.Add("LTDCODE", LTDCODE);
            }
            return ExecuteReader<HIC_LTD_TAX>(parameter);
        }

        public int UpDate(HIC_LTD_TAX item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_LTD_TAX     ");
            parameter.AppendSql("   SET BUSE    = :BUSE             ");
            parameter.AppendSql("      ,DAMNAME = :DAMNAME          ");
            parameter.AppendSql("      ,DAMJIK  = :DAMJIK           ");
            parameter.AppendSql("      ,TEL     = :TEL              ");
            parameter.AppendSql("      ,HPHONE  = :HPHONE           ");
            parameter.AppendSql("      ,EMAIL   = :EMAIL            ");
            parameter.AppendSql("      ,REMARK  = :REMARK           ");
            parameter.AppendSql("      ,JOBSABUN= :JOBSABUN         ");
            parameter.AppendSql("      ,ENTTIME = SYSDATE           ");
            parameter.AppendSql(" WHERE ROWID   = :RID              ");

            #region Query 변수대입
            parameter.Add("BUSE",     item.BUSE);
            parameter.Add("DAMNAME",  item.DAMNAME);
            parameter.Add("DAMJIK",   item.DAMJIK);
            parameter.Add("TEL",      item.TEL);
            parameter.Add("HPHONE",   item.HPHONE);
            parameter.Add("EMAIL",    item.EMAIL);
            parameter.Add("REMARK",   item.REMARK);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            parameter.Add("RID",      item.RID);
            #endregion

            return ExecuteNonQuery(parameter);
        }

        public int Insert(HIC_LTD_TAX item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" INSERT INTO ADMIN.HIC_LTD_TAX (                                                 ");
            parameter.AppendSql("        LTDCODE,BUSE,DAMNAME,DAMJIK,TEL,HPHONE,EMAIL,REMARK,JOBSABUN,ENTTIME           ");
            parameter.AppendSql(") VALUES (                                                                             ");
            parameter.AppendSql("       :LTDCODE,:BUSE,:DAMNAME,:DAMJIK,:TEL,:HPHONE,:EMAIL,:REMARK,:JOBSABUN,SYSDATE  )");

            #region Query 변수대입
            parameter.Add("LTDCODE",  item.LTDCODE);
            parameter.Add("BUSE",     item.BUSE);
            parameter.Add("DAMNAME",  item.DAMNAME);
            parameter.Add("DAMJIK",   item.DAMJIK);
            parameter.Add("TEL",      item.TEL);
            parameter.Add("HPHONE",   item.HPHONE);
            parameter.Add("EMAIL",    item.EMAIL);
            parameter.Add("REMARK",   item.REMARK);
            parameter.Add("JOBSABUN", item.JOBSABUN);
            #endregion

            return ExecuteNonQuery(parameter);
        }
    }
}
