
namespace ComHpcLibB.Repository
{

    using System;
    using System.Collections.Generic;
    using ComBase.Mvc;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicJepsuPatientLtdRepository : BaseRepository
    {
        public HicJepsuPatientLtdRepository()
        {
        }

        public HIC_JEPSU_PATIENT_LTD GetItemByWrtno(long nWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql(" SELECT B.SNAME, B.JUMIN, C.SANGHO, B.HPHONE,a.LtdCode, b.ENAME                        ");
            parameter.AppendSql(" FROM KOSMOS_PMPA.HIC_JEPSU A, KOSMOS_PMPA.HIC_PATIENT B, KOSMOS_PMPA.HIC_LTD C        ");
            parameter.AppendSql(" WHERE A.PANO = B.PANO                                                                 ");
            parameter.AppendSql(" AND A.WRTNO = :WRTNO                                                                  ");
            parameter.AppendSql(" AND a.LTDCODE = C.CODE(+)                                                             ");

            parameter.Add("WRTNO", nWRTNO);

            return ExecuteReaderSingle<HIC_JEPSU_PATIENT_LTD>(parameter);
        }
    }
}
