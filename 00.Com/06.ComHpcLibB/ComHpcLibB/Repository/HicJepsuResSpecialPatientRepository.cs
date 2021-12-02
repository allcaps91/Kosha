
namespace ComHpcLibB.Repository
{


    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicJepsuResSpecialPatientRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicJepsuResSpecialPatientRepository()
        {
        }


        public List<HIC_JEPSU_RES_SPECIAL_PATIENT> GetItemByJepdate(string strFdate, string strTdate)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT A.WRTNO,A.SNAME,TO_CHAR(A.JEPDATE,'YYYY-MM-DD') JEPDATE, A.LTDCODE, A.TEL           ");
            parameter.AppendSql(" , B.HNAME, B.GbSPC, C.JUMIN, A.SEXAMS                                                     ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_JEPSU A, KOSMOS_PMPA.HIC_RES_SPECIAL B, KOSMOS_PMPA.HIC_PATIENT C    ");
            parameter.AppendSql(" WHERE  1 = 1                                                                              ");
            parameter.AppendSql(" AND A.WRTNO = B.WRTNO                                                                     ");
            parameter.AppendSql(" AND A.PANO = C.PANO                                                                       ");
            parameter.AppendSql(" AND (A.SEXAMS LIKE '%J134%' OR A.SEXAMS LIKE '%J225%')                                    ");
            parameter.AppendSql(" AND A.JEPDATE >= :FDATE                                                                   ");
            parameter.AppendSql(" AND A.JEPDATE <= :TDATE                                                                   ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                                                     ");

            parameter.Add("FDATE", strFdate);
            parameter.Add("TDATE", strTdate);

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_PATIENT>(parameter);
        }


        public List<HIC_JEPSU_RES_SPECIAL_PATIENT> GetItemByJepdateGbspc(string strFdate, string strTdate, long nWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql(" SELECT A.WRTNO,A.SNAME,TO_CHAR(A.JEPDATE,'YYYY-MM-DD') JEPDATE,b.HNAME,B.GbSpc,C.JUMIN    ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_JEPSU A, KOSMOS_PMPA.HIC_RES_SPECIAL B, KOSMOS_PMPA.HIC_PATIENT C    ");
            parameter.AppendSql(" WHERE  1 = 1                                                                              ");
            parameter.AppendSql(" AND A.WRTNO = B.WRTNO                                                                     ");
            parameter.AppendSql(" AND A.PANO = C.PANO                                                                       ");
            parameter.AppendSql(" AND B.GBSPC IN ('10', '16')                                                               ");
            parameter.AppendSql(" AND A.JEPDATE >= :FDATE                                                                   ");
            parameter.AppendSql(" AND A.JEPDATE <= :TDATE                                                                   ");
            parameter.AppendSql(" AND A.DELDATE IS NULL                                                                     ");
            if (nWrtno > 0)
            {
                parameter.AppendSql(" AND A.WRTNO = :WRTNO                                                                  ");
            }

            parameter.Add("FDATE", strFdate);
            parameter.Add("TDATE", strTdate);
            //parameter.Add("GBSPC", strGbspc, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            if (nWrtno > 0)
            {
                parameter.Add("WRTNO", nWrtno);
            }

            return ExecuteReader<HIC_JEPSU_RES_SPECIAL_PATIENT>(parameter);
        }

        public HIC_JEPSU_RES_SPECIAL_PATIENT GetItembyWrtno(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT B.UCODES,B.SEXAMS,B.SNAME,B.SEX,B.AGE,C.SABUN,B.JUSO1,B.JUSO2,B.GJJONG                 ");
            parameter.AppendSql(" ,TO_CHAR(B.IPSADATE,'YYYY-MM-DD') IPSADATE,B.TEL,B.LTDCODE,C.BUSENAME,C.HPHONE                ");
            parameter.AppendSql(" ,A.GONGJENG,B.JIKJONG,TO_CHAR(B.BUSEIPSA,'YYYY-MM-DD') JENIPDATE                              ");
            parameter.AppendSql(" ,TO_CHAR(B.JEPDATE,'YYYY-MM-DD') JEPDATE                                                      ");
            parameter.AppendSql(" ,TO_CHAR(A.PANJENGDATE,'YYYY-MM-DD') PANJENGDATE                                              ");
            parameter.AppendSql(" ,B.MAILCODE,A.PGIGAN_YY,A.PGIGAN_MM,A.PTIME                                                   ");
            parameter.AppendSql(" ,A.OLDGONG1,A.OLDMCODE1,OLDMYEAR1,A.OLDDAYTIME1                                               ");
            parameter.AppendSql(" ,A.OLDGONG2,A.OLDMCODE2,OLDMYEAR2,A.OLDDAYTIME2                                               ");
            parameter.AppendSql(" ,A.OLDGONG3,A.OLDMCODE3,OLDMYEAR3,A.OLDDAYTIME3                                               ");
            parameter.AppendSql(" ,A.OLDMYEAR1,A.OLDMYEAR2,A.OLDMYEAR3,A.OLDMYEAR4,A.OLDMYEAR5,A.OLDETCMSYM                     ");
            parameter.AppendSql(" ,A.MUN_OLDMSYM,A.MUN_GAJOK,A.MUN_GIINSUNG,A.JIN_NEURO,A.JIN_HEAD                              ");
            parameter.AppendSql(" ,A.JIN_SKIN,A.JIN_CHEST,A.GBSPC,A.JENGSANG,A.DENTSOGEN,A.DENTDOCT                             ");
            parameter.AppendSql(" ,A.XRAYGBN,A.XRAYNO,A.XRAYNO2,B.PANO,C.JUMIN,C.JISA,A.PANJENGDRNO,  B.GBJUSO                  ");
            parameter.AppendSql(" FROM HIC_RES_SPECIAL A,HIC_JEPSU B,HIC_PATIENT C                                              ");
            parameter.AppendSql(" WHERE A.WRTNO = :WRTNO                                                                        ");
            parameter.AppendSql(" AND A.WRTNO = B.WRTNO(+)                                                                      ");
            parameter.AppendSql(" AND B.PANO  = C.PANO(+)                                                                       ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteReaderSingle<HIC_JEPSU_RES_SPECIAL_PATIENT>(parameter);
        }

    }

}
