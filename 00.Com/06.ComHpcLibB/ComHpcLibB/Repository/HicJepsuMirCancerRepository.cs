namespace ComHpcLibB.Repository
{ 
using System;
using System.Collections.Generic;
using ComBase.Mvc;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;

/// <summary>
/// 
/// </summary>
public class HicJepsuMirCancerRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuMirCancerRepository()
        {
        }
        public List<HIC_JEPSU_MIR_CANCER> GetListByItems(string ArgYear, string ArgFDATE, string ArgTDATE, string ArgJohap, string ArgJong, string ArgLtdcode)
        {

            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT b.Mirno,TO_CHAR(b.JepDate,'YYYY-MM-DD') ChungDate, a.WRTNO,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate, a.GjJong        ");
            parameter.AppendSql(" ,a.GjChasu, a.Sname, a.Age, a.Sex, a.Gkiho,a.LtdCode,a.kiho                                                               ");
            parameter.AppendSql(" FROM HIC_JEPSU a, HIC_MIR_CANCER b                                                                                        ");
            parameter.AppendSql(" WHERE a.JepDate>=TO_DATE('" + ArgFDATE + "','YYYY-MM-DD')                                                                 ");
            parameter.AppendSql(" AND a.JepDate<=TO_DATE('" + ArgTDATE + "','YYYY-MM-DD')                                                                   ");
            parameter.AppendSql(" AND a.GjJong='31'                                                                                                         ");
            parameter.AppendSql(" AND a.DelDate IS NULL                                                                                                     ");
            parameter.AppendSql(" AND a.GjYear='" + ArgYear + "'                                                                                            ");
            if (ArgJohap== "사업장")
            {
                parameter.AppendSql(" AND SUBSTR(a.Gkiho,1,1) IN ('7','8')                                                                                  ");
            }
            else if (ArgJohap == "공무원")
            {
                parameter.AppendSql(" AND SUBSTR(a.Gkiho,1,1) IN ('5','6')                                                                                  ");
            }
            else if (ArgJohap == "성인병")
            {
                parameter.AppendSql(" AND SUBSTR(a.Gkiho,1,1) IN ('1','2','3','4')                                                                          ");
            }

            if (ArgJong == "4")
            {
                parameter.AppendSql(" AND a.Mirno3 = b.Mirno(+)                                                                                             ");
            }
            else if (ArgJong == "5")
            {
                parameter.AppendSql(" AND a.Mirno4 = b.Mirno(+)                                                                                             ");
            }

            else if (ArgJong == "E")
            {
                parameter.AppendSql(" AND a.Mirno5 = b.Mirno(+)                                                                                             ");
            }

            if (ArgLtdcode != "")
            {
                parameter.AppendSql(" AND A.LTDCODE = '"+ ArgLtdcode+ "'                                                                                    ");
            }

            parameter.AppendSql(" ORDER BY B.MIRNO, A.WRTNO                                                                                                 ");

            //parameter.Add("WRTNO", ArgWRTNO);
            //parameter.Add("FDATE", ArgFDATE);
            //parameter.Add("TDATE", ArgTDATE);
            //parameter.Add("GUBUN", ArgGUBUN);
            //parameter.Add("BUSE", ArgBUSE);

            return ExecuteReader<HIC_JEPSU_MIR_CANCER>(parameter);
        }

    }
}
