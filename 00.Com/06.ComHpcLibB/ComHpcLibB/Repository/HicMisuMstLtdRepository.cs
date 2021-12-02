namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicMisuMstLtdRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicMisuMstLtdRepository()
        {
        }

        public List<HIC_MISU_MST_LTD> GetBillList(string strFDate, string strTDate, string strMJong)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                    A.LTDCODE,A.WRTNO,A.GJONG,A.MISUAMT                                                      ");
            parameter.AppendSql("                        , B.SAUPNO,B.UPTAE,B.JONGMOK,B.DAEPYO,B.SANGHO,B.JUSO,B.JUSODETAIL,B.JSAUPNO,B.DLTD        ");

            parameter.AppendSql("FROM                      HIC_MISU_MST A,HIC_LTD B                                                                 ");

            parameter.AppendSql("WHERE                     A.BDATE>=TO_DATE( :FDATE, 'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND                    A.BDATE<=TO_DATE( :TDATE, 'YYYY-MM-DD')                                                  ");
            parameter.AppendSql("   AND                    (A.GBTAX IS NULL OR A.GBTAX='N' OR A.GBTAX = ' ')                                        ");  // 지로영수증 발급된것은 제외
            parameter.AppendSql("   AND                    B.SAUPNO > ' '                                                                           ");  // 사업자등록번호가 공란이 아닌 것

            switch (strMJong)
            {
                case "1":
                    parameter.AppendSql("   AND            A.GJONG <=  '80'                                                                         ");  // 건강검진
                    break;
                case "2":
                    parameter.AppendSql("   AND            A.GJONG =  '82'                                                                         ");  // 보건관리대행
                    break;
                case "3":
                    parameter.AppendSql("   AND            A.GJONG =  '81'                                                                         ");  // 작업환경측정
                    break;
                case "4":
                    parameter.AppendSql("   AND            A.GJONG =  '83'                                                                         ");  // 종합건진
                    break;
                case "5":
                    parameter.AppendSql("   AND            A.MISUJONG= '4'                                                                          ");  // 개인미수
                    break;
                case "6":
                    parameter.AppendSql("   AND            A.GJONG =  '82'                                                                         ");  // 보건관리대행
                    break;
            }

            parameter.AppendSql("   AND                    A.MISUJONG IN ('1','4')                                                                  ");  // 회사미수, 개인미수
            parameter.AppendSql("   AND                    A.LTDCODE = B.CODE(+)                                                                    ");  
            parameter.AppendSql("ORDER BY                  B.NAME,A.WRTNO,A.LTDCODE                                                                 ");

            parameter.Add("FDATE", strFDate);
            parameter.Add("TDATE", strTDate);

            return ExecuteReader<HIC_MISU_MST_LTD>(parameter);
        }

        public int UpdateBill(long nWrtno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE     HIC_MISU_MST SET GbTAX='Y'       ");
            parameter.AppendSql("WHERE      WRTNO = :WRTNO                   ");

            parameter.Add("WRTNO", nWrtno);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_MISU_MST_LTD> GetActingItem(long DLTD)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT                    SAUPNO,SANGHO,DAEPYO,UPTAE,JONGMOK,JUSO,JUSODETAIL,JSAUPNO       ");

            parameter.AppendSql("FROM                      HIC_LTD                                                          ");

            parameter.AppendSql("WHERE                     CODE = :DLTD                                                     ");

            parameter.Add("DLTD", DLTD);

            return ExecuteReader<HIC_MISU_MST_LTD>(parameter);
        }
    }
}
