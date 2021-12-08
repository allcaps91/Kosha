using ComBase;
using ComBase.Mvc;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC_OSHA.Model.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Repository
{
    public class ChargeRepository : BaseRepository
    {
        private string MISU_MST = "HIC_MISU_MST";//HIC_MISU_MST
        private string MISU_MST_SLIP = "HIC_MISU_SLIP";//HIC_MISU_SLIP
        private string MISU_SEQ = "SEQ_HIC_MISUNO"; //"HIC_MISU_MST_SEQ";//SEQ_HIC_MISUNO
        private string MISU_HISTORY = "HIC_MISU_HISTORY";//HIC_MISU_HISTORY

        private OshaPriceRepository oshaPriceRepository;

        public  ChargeRepository()
        {
            oshaPriceRepository = new OshaPriceRepository();
        }
        public void ChageTable()
        {
            MISU_MST = "HIC_MISU_MST";
            MISU_MST_SLIP = "HIC_MISU_SLIP";
            MISU_SEQ = "SEQ_HIC_MISUNO";
            MISU_HISTORY= "HIC_MISU_HISTORY";
        }

        public void UpdateChargePrice(string startDate, string endDate, long siteId, long misuAmt)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("             SELECT B.ID FROM  HIC_OSHA_VISIT A                    ");
            parameter.AppendSql(" INNER JOIN HIC_OSHA_VISIT_PRICE B                                 ");
            parameter.AppendSql(" ON A.ID = B.VISIT_ID                                              ");
            parameter.AppendSql(" WHERE A.ISDELETED = 'N'                                           ");
            parameter.AppendSql(" AND A.SITE_ID = :SITE_ID                                          ");
            parameter.AppendSql(" AND B.ISDELETED = 'N'                                             ");
            parameter.AppendSql(" AND A.VISITDATETIME >= TO_DATE(:startDate, 'YYYY-MM-DD')          ");
            parameter.AppendSql(" AND A.VISITDATETIME <= TO_DATE(:endDate, 'YYYY-MM-DD')            ");
            parameter.AppendSql(" AND SWLICENSE = :SWLICENSE ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            List<OSHA_VISIT_PRICE> list = ExecuteReader<OSHA_VISIT_PRICE>(parameter);

            List<OSHA_PRICE> childPrice = oshaPriceRepository.FindAllByParent(siteId);
            long totalPrice = misuAmt;
            foreach (OSHA_PRICE child in childPrice)
            {
                totalPrice += child.TOTALPRICE;
            }

            foreach (OSHA_VISIT_PRICE price in list)
            {
                parameter = CreateParameter();
                parameter.AppendSql("   UPDATE HIC_OSHA_VISIT_PRICE    ");
                parameter.AppendSql("   SET CHARGEPRICE = :misuAmt    ");
                parameter.AppendSql("   WHERE ID = :ID ");
                parameter.AppendSql("     AND SWLICENSE = :SWLICENSE ");
                parameter.Add("misuAmt", totalPrice);
                parameter.Add("ID", price.ID);
                parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
                ExecuteNonQuery(parameter);
            }
        }

        /// <summary>
        /// 선청구로 수수료 발생된 데이타는 삭제
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="siteId"></param>
        public void DeleteVisitPreCharge(string startDate, string endDate, long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.ID, B.VISIT_ID ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_VISIT_PRICE B ");
            parameter.AppendSql("             ON A.ID = B.VISIT_ID ");
            parameter.AppendSql(" WHERE A.ISDELETED = 'N' ");
            parameter.AppendSql("   AND A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND B.ISDELETED = 'N' ");
            parameter.AppendSql("   AND A.ISPRECHARGE = 'Y' ");
            parameter.AppendSql("   AND A.VISITDATETIME >= TO_DATE(:startDate, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND A.VISITDATETIME <= TO_DATE(:endDate, 'YYYY-MM-DD') ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("SITE_ID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            List<OSHA_VISIT_PRICE> list = ExecuteReader<OSHA_VISIT_PRICE>(parameter);

            foreach (OSHA_VISIT_PRICE price in list)
            {
                parameter = CreateParameter();
                parameter.AppendSql("UPDATE HIC_OSHA_VISIT   ");
                parameter.AppendSql("  SET ISDELETED='Y' ");
                parameter.AppendSql("WHERE ID = :VISIT_ID ");
                parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");
                parameter.Add("VISIT_ID", price.VISIT_ID);
                parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
                ExecuteNonQuery(parameter);

                parameter = CreateParameter();
                parameter.AppendSql("   UPDATE HIC_OSHA_VISIT_PRICE    ");
                parameter.AppendSql("   SET ISDELETED='Y' ");
                parameter.AppendSql("   WHERE ID = :ID ");
                parameter.AppendSql("     AND SWLICENSE = :SWLICENSE ");
                parameter.Add("ID", price.ID);
                parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
                ExecuteNonQuery(parameter);
            }
        }
        public long GetVisitCount(string startDate, string endDate, long siteId)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT(*) AS VISITCOUNT ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_VISIT_PRICE B ");
            parameter.AppendSql("             ON A.ID = B.VISIT_ID ");
            parameter.AppendSql("  WHERE A.ISDELETED = 'N' ");
            parameter.AppendSql("    AND B.ISDELETED = 'N' ");
            parameter.AppendSql("    AND A.ISFEE = 'Y' ");
            parameter.AppendSql("    AND A.VISITDATETIME >= :startDate ");
            parameter.AppendSql("    AND A.VISITDATETIME <= :endDate ");
            parameter.AppendSql("    AND A.SITE_ID = :siteId ");
            parameter.AppendSql("    AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("    AND B.SWLICENSE = :SWLICENSE ");
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("siteId", siteId);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteScalar<long>(parameter);
        }
        public long Save(HC_MISU_MST dto)
        {
            dto.WRTNO =  GetSequenceNextVal(MISU_SEQ);

            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO "+ MISU_MST + " (WRTNO,LtdCode,MisuJong,BDate,GJong,                ");
            parameter.AppendSql("GbEnd,MisuGbn,MisuAmt,IpgumAmt,GamAmt,SakAmt,BanAmt,JanAmt,                    ");
            parameter.AppendSql("GiroNo,DamName,Remark,MirGbn,MirChaAmt,GbMisuBuild,YYMM_Jin, pummok,Cno,       ");
            parameter.AppendSql("EntDate,EntSabun,SWLICENSE) VALUES (  ");
            parameter.AppendSql("  :WRTNO,         ");
            parameter.AppendSql("  :LTDCODE,       ");
            parameter.AppendSql("  :MISUJONG,      ");
            parameter.AppendSql("  :BDATE,         ");
            parameter.AppendSql("  :GJONG,         ");
            parameter.AppendSql("  :GBEND,         ");
            parameter.AppendSql("  :MISUGBN,       ");
            parameter.AppendSql("  :MISUAMT,       ");
            parameter.AppendSql("  :IPGUMAMT,      ");
            parameter.AppendSql("  :GAMAMT,        ");
            parameter.AppendSql("  :SAKAMT,        ");
            parameter.AppendSql("  :BANAMT,        ");
            parameter.AppendSql("  :JANAMT,        ");
            parameter.AppendSql("  :GIRONO,        ");
            parameter.AppendSql("  :DANNAME,       ");
            parameter.AppendSql("  :REMARK,        ");
            parameter.AppendSql("  :MIRGBN,        ");
            parameter.AppendSql("  :MIRCHAAMT,     ");
            parameter.AppendSql("  :GBMISUBUILD,   ");
            parameter.AppendSql("  :YYMM_JIN,      ");
            parameter.AppendSql("  :PUMMOK,        ");
            parameter.AppendSql("  :CNO,           ");
            parameter.AppendSql("   SYSDATE,       ");
            parameter.AppendSql("  :ENTSABUN,      ");
            parameter.AppendSql("  :SWLICENSE      ");
            parameter.AppendSql(")                 ");
            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("LTDCODE", dto.LTDCODE);
            parameter.Add("MISUJONG", dto.MISUJONG);
            parameter.Add("BDATE", dto.BDATE);
            parameter.Add("GJONG", dto.GJONG);
            parameter.Add("GBEND", dto.GBEND);
            parameter.Add("MISUGBN", dto.MISUGBN);
            parameter.Add("MISUAMT", dto.MISUAMT);
            parameter.Add("IPGUMAMT", dto.IPGUMAMT);
            parameter.Add("GAMAMT", dto.GAMAMT);
            parameter.Add("SAKAMT", dto.SAKAMT);
            parameter.Add("BANAMT", dto.BANAMT);
            parameter.Add("JANAMT", dto.JANAMT);
            parameter.Add("GIRONO", dto.GIRONO);
            parameter.Add("DANNAME", dto.DANNAME);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("MIRGBN", dto.MIRGBN);
            parameter.Add("MIRCHAAMT", dto.MIRCHAAMT);
            parameter.Add("GBMISUBUILD", dto.GBMISUBUILD);
            parameter.Add("YYMM_JIN", dto.YYMM_JIN);
            parameter.Add("PUMMOK", dto.PUMMOK);
            parameter.Add("CNO", dto.CNO);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
            return dto.WRTNO;
        }

        public void SaveSlip(HC_MISU_SLIP dto)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT INTO "+ MISU_MST_SLIP + " (WRTNO,BDATE,LTDCODE,GEACODE, ");
            parameter.AppendSql("            SLIPAMT, REMARK,ENTDATE ,ENTSABUN,SWLICENSE ");
            parameter.AppendSql(") VALUES (                  ");
            parameter.AppendSql("  :WRTNO,                   ");
            parameter.AppendSql("  :BDATE,                   ");
            parameter.AppendSql("  :LTDCODE,                 ");
            parameter.AppendSql("  :GEACODE,                 ");
            parameter.AppendSql("  :SLIPAMT,                 ");
            parameter.AppendSql("  :REMARK,                  ");
            parameter.AppendSql("  SYSDATE,                  ");
            parameter.AppendSql("  :ENTSABUN,                ");
            parameter.AppendSql("  :SWLICENSE                ");
            parameter.AppendSql("  ) ");
            parameter.Add("WRTNO", dto.WRTNO);
            parameter.Add("BDATE", dto.BDATE);
            parameter.Add("LTDCODE", dto.LTDCODE);
            parameter.Add("GEACODE", dto.GEACODE);
            parameter.Add("SLIPAMT", dto.SLIPAMT);
            parameter.Add("REMARK", dto.REMARK);
            parameter.Add("ENTSABUN", dto.ENTSABUN);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }

        public void SaveHC_MISU_HISTORY(long wrtNo, string jobGbn)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("  INSERT INTO "+ MISU_HISTORY + " (JobDate,JobSabun,JobGbn,WRTNO,BDate,LtdCode, ");
            parameter.AppendSql(" GeaCode,SlipAmt,Remark,EntDate,EntSabun,SWLICENSE)                           ");
            parameter.AppendSql(" SELECT SYSDATE,  "+ CommonService.Instance.Session.UserId +",'"+ jobGbn + "',WRTNO,BDate,LtdCode, ");
            parameter.AppendSql("        GeaCode,SlipAmt,Remark,EntDate,EntSabun,SWLICENSE   ");
            parameter.AppendSql("   FROM " + MISU_MST_SLIP + "   ");
            parameter.AppendSql("  WHERE WRTNO= :WRTNO ");
            parameter.AppendSql("  AND SWLICENSE = :SWLICENSE ");

            parameter.Add("WRTNO", wrtNo);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }

        public long FindMaxCno(string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(CNO) as CNO  FROM HIC_MISU_MST   ");
            parameter.AppendSql("WHERE BDate>=TO_DATE(:startDate,'YYYY-MM-DD')   ");
            parameter.AppendSql("AND BDate<=TO_DATE(:endDate,'YYYY-MM-DD')   ");
            parameter.AppendSql("AND MisuJong='1'   ");
            parameter.AppendSql("AND GJong='82' ");
            parameter.AppendSql("AND SWLICENSE = :SWLICENSE ");

            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteScalar<long>(parameter);
        }

        public void DeleteMisuMst(long wrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   DELETE "+MISU_MST+" WHERE WRTNO= :WRTNO ");
            parameter.AppendSql("AND SWLICENSE = :SWLICENSE ");
            parameter.Add("WRTNO", wrtNo);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }
        public void DeleteMisuSlip(long wrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("   DELETE " + MISU_MST_SLIP + " WHERE WRTNO= :WRTNO ");
            parameter.AppendSql("     AND SWLICENSE = :SWLICENSE ");
            parameter.Add("WRTNO", wrtNo);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            ExecuteNonQuery(parameter);
        }

        /// <summary>
        /// 분기청구 발생된 금액 합계
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public long GetQuarterTotalPrice(long siteId, string startDate, string endDate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT B.TOTALPRICE ");
            parameter.AppendSql("  FROM HIC_OSHA_VISIT A ");
            parameter.AppendSql("       INNER JOIN HIC_OSHA_VISIT_PRICE B ");
            parameter.AppendSql("             ON A.ID = B.VISIT_ID ");
            parameter.AppendSql(" WHERE A.ISDELETED = 'N' ");
            parameter.AppendSql("   AND B.ISDELETED = 'N' ");
            parameter.AppendSql("   AND A.ISPRECHARGE = 'N' ");
            parameter.AppendSql("   AND A.SITE_ID = :SITE_ID ");
            parameter.AppendSql("   AND VISITDATETIME >= :startDate ");
            parameter.AppendSql("   AND VISITDATETIME <= :endDate ");
            parameter.AppendSql("   AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("   AND B.SWLICENSE = :SWLICENSE ");

            parameter.Add("SITE_ID", siteId);
            parameter.Add("startDate", startDate);
            parameter.Add("endDate", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);
            return ExecuteScalar<long>(parameter);
        }

        public List<ChargeSiteModel> FindByYear(string startDate, string endDate, bool isParent, bool isQuaterCharge)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.SITEID,A.PARENTSITE_ID,A.SITENAME,A.ESTIMATE_ID,A.CONTRACTDATE,");
            parameter.AppendSql("       A.UNITPRICE,A.TOTALPRICE,A.DAMNAME,A.WRTNO,A.BDATE,A.RANK ");
            parameter.AppendSql("  FROM ( ");
            parameter.AppendSql("       SELECT A.ID as SITEID,A.PARENTSITE_ID,B.NAME as SITENAME,D.ESTIMATE_ID,");
            parameter.AppendSql("              D.CONTRACTDATE,E.UNITPRICE,E.TOTALPRICE,F.NAME as DAMNAME,");
            parameter.AppendSql("              (SELECT MAX(WRTNO) FROM HIC_MISU_MST ");
            parameter.AppendSql("                WHERE Bdate >= TO_DATE(:START_DATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("                  AND Bdate <= TO_DATE(:END_DATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("                  AND MISUJONG = '1' ");
            parameter.AppendSql("                  AND GJONG = '82' ");
            parameter.AppendSql("                  AND LTDCODE = A.ID ");
            parameter.AppendSql("                  AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("               ) AS WRTNO, ");
            parameter.AppendSql("              (SELECT To_CHAR(MAX(BDATE), 'YYYY-MM-DD') FROM HIC_MISU_MST ");
            parameter.AppendSql("                 WHERE Bdate >= TO_DATE(:START_DATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("                   AND Bdate <= TO_DATE(:END_DATE, 'YYYY-MM-DD') ");
            parameter.AppendSql("                   AND MISUJONG = '1' ");
            parameter.AppendSql("                   AND GJONG = '82' ");
            parameter.AppendSql("                   AND LTDCODE = A.ID ");
            parameter.AppendSql("                   AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("               ) AS BDATE, ");
            parameter.AppendSql("               ROW_NUMBER() OVER (PARTITION BY A.ID ORDER BY D.CONTRACTDATE DESC) AS RANK ");
            parameter.AppendSql("          FROM HIC_OSHA_SITE A  ");
            parameter.AppendSql("          INNER JOIN HC_SITE_VIEW B ");
            parameter.AppendSql("                  ON A.ID = B.ID ");
            //승진이엔지(43607) 국고 Y로 되어 있어서 주석처리함
            //     parameter.AppendSql("   AND B.GBGUKGO = 'N'     ");
            parameter.AppendSql("          INNER JOIN HIC_OSHA_ESTIMATE C ");
            parameter.AppendSql("                  ON C.OSHA_SITE_ID = A.ID ");
            parameter.AppendSql("          INNER JOIN HIC_OSHA_CONTRACT D ");
            parameter.AppendSql("                  ON d.estimate_id = C.Id ");
            parameter.AppendSql("                 AND D.ISCONTRACT = 'Y' ");
            parameter.AppendSql("          INNER JOIN (SELECT * FROM HIC_OSHA_PRICE ");
            parameter.AppendSql("                       WHERE ID IN (SELECT MAX(ID) FROM HIC_OSHA_PRICE ");
            parameter.AppendSql("                             WHERE ISDELETED ='N' AND SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("                             GROUP BY ESTIMATE_ID)) E ");
            parameter.AppendSql("                  ON E.ESTIMATE_ID = C.ID ");
            parameter.AppendSql("          LEFT OUTER JOIN HIC_USERS F ");
            parameter.AppendSql("                       ON F.USERID = D.MANAGEENGINEER ");
            parameter.AppendSql("         WHERE 1 = 1 ");
            parameter.AppendSql("           AND A.SWLICENSE = :SWLICENSE ");
            parameter.AppendSql("           and D.CONTRACTDATE <= :END_DATE ");
            parameter.AppendSql("           AND A.ISACTIVE='Y' ");
            parameter.AppendSql("           AND D.ISCONTRACT='Y' ");
    
            if (isParent)
            {
                parameter.AppendSql("   AND A.ISPARENTCHARGE = 'Y'  ");
            }
            else
            {
                parameter.AppendSql("   AND A.PARENTSITE_ID IS NOT NULL ");
                parameter.AppendSql("   AND A.ISPARENTCHARGE = 'N' ");

            }
            if(isQuaterCharge){
                parameter.AppendSql("   AND A.ISQUARTERCHARGE = 'Y' ");
            }
            else
            {
                parameter.AppendSql("   AND A.ISQUARTERCHARGE <> 'Y' ");
            }

            parameter.AppendSql("  ) A                                                                                    ");
            parameter.AppendSql(" WHERE RANK = 1                                                                          ");
            parameter.AppendSql("ORDER BY A.SITENAME, A.SITEID                                                            ");

            parameter.Add("START_DATE", startDate);
            parameter.Add("END_DATE", endDate);
            parameter.Add("SWLICENSE", clsType.HosInfo.SwLicense);

            return ExecuteReader<ChargeSiteModel>(parameter);

        }
    }
}
