namespace ComHpcLibB.Repository
{
    using ComBase;
    using ComBase.Mvc;
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;

    /// <summary>
    /// 
    /// </summary>
    public class PatientInfoRepository : BaseRepository
    {   
        /// <summary>
        /// 
        /// </summary>
        public PatientInfoRepository()
        {
        }

        public PATIENT_INFO GetItembyWrtNoPart(long WRTNO, string sHcPart)
        {
            MParameter parameter = CreateParameter();
            if (sHcPart == "1") //종합검진
            {
                parameter.AppendSql("SELECT a.Pano,a.SName,a.Age,a.Sex,a.LtdCode,TO_CHAR(a.SDate,'YYYY-MM-DD') JepDate, a.Ptno  ");
                parameter.AppendSql("     , '' XRAYNO, a.GjJong, b.Jumin, a.ActMemo, a.ExamRemark, a.GongDan, a.GbExam          ");                
                parameter.AppendSql("     , a.GbNaksang, a.SUNAP, a.ROWID RID                                                   ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_JEPSUJONG(b.PTNO, a.SDATE) JEPSUJONG                             ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HEA_JEPSU   a                                                           ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                           ");
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                    ");
                parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                    ");
            }
            else if (sHcPart == "2")    //일반검진
            {
                parameter.AppendSql("SELECT a.Pano,a.SName,a.Age,a.Sex,a.LtdCode,TO_CHAR(a.JepDate,'YYYY-MM-DD') JepDate,a.Ptno ");
                parameter.AppendSql("     , a.XrayNo, a.GjJong, b.Jumin, a.GjYear,a.UCodes,a.GbNaksang,a.ExamRemark,b.Jumin2, a.ROWID RID ");
                parameter.AppendSql("     , KOSMOS_PMPA.FC_HIC_JEPSUJONG(b.PTNO, a.JEPDATE) JEPSUJONG                           ");
                parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU   a                                                           ");
                parameter.AppendSql("     , KOSMOS_PMPA.HIC_PATIENT b                                                           ");
                parameter.AppendSql(" WHERE a.WRTNO = :WRTNO                                                                    ");
                parameter.AppendSql("   AND a.Pano=b.Pano(+)                                                                    ");
            }

            parameter.Add("WRTNO", WRTNO);

            return ExecuteReaderSingle<PATIENT_INFO>(parameter);
        }
    }
}
