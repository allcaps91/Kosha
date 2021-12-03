using ComBase;
using ComBase.Mvc;
using System;
using System.Collections.Generic;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;

namespace ComHpcLibB.Repository
{
    public class HeaJepsuMemoRepository : BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaJepsuMemoRepository()
        {
        }

        public List<HEA_JEPSU_MEMO> GetItembyPaNo(long pANO)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,b.MEMO,TO_CHAR(b.ENTTIME,'YYYY-MM-DD HH24:MI') ENTTIME,b.JOBSABUN,b.ROWID RID   ");
            parameter.AppendSql("     , TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, a.PTNO                                             ");
            parameter.AppendSql("  From ADMIN.HEA_JEPSU a, ADMIN.HEA_MEMO b                                         ");
            parameter.AppendSql(" WHERE a.PANO = :PANO                                                                          ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                                       ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                       ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                                       ");
            parameter.AppendSql(" ORDER BY a.SDATE DESC, b.ENTTIME DESC                                                         ");

            parameter.Add("PANO", pANO);

            return ExecuteReader<HEA_JEPSU_MEMO>(parameter);
        }

        public List<HEA_JEPSU_MEMO> GetItembyPtno(string argPtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT a.WRTNO,b.MEMO,TO_CHAR(b.ENTTIME,'YYYY-MM-DD HH24:MI') ENTTIME,b.JOBSABUN,b.ROWID RID   ");
            parameter.AppendSql("     , TO_CHAR(a.SDATE,'YYYY-MM-DD') SDATE, a.PTNO                                             ");
            parameter.AppendSql("  From ADMIN.HEA_JEPSU a, ADMIN.HEA_MEMO b                                         ");
            parameter.AppendSql(" WHERE a.PTNO = :PTNO                                                                          ");
            parameter.AppendSql("   AND a.WRTNO = b.WRTNO                                                                       ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                                                       ");
            parameter.AppendSql("   AND b.DELDATE IS NULL                                                                       ");
            parameter.AppendSql(" ORDER BY a.SDATE DESC, b.ENTTIME DESC                                                         ");

            parameter.Add("PTNO", argPtno);

            return ExecuteReader<HEA_JEPSU_MEMO>(parameter);
        }
    }
}
