
namespace ComHpcLibB.Repository
{

    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicEmrResultWorkRepository : BaseRepository
    {

        public HicEmrResultWorkRepository()
        {

        }

        public HIC_EMR_RESULT_WORK GetItemByWrtnoGubun(long argWrtno, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID, JOBDATE                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EMR_RESULT_WORK         ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                            ");
            parameter.AppendSql(" AND GUBUN = :GUBUN                            ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("GUBUN", argGubun);

            return ExecuteReaderSingle<HIC_EMR_RESULT_WORK>(parameter);
        }

        public int Insert(HIC_EMR_RESULT_WORK item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_EMR_RESULT_WORK(                               ");
            parameter.AppendSql(" JOBDATE, GUBUN, WRTNO, DRNO, ENTSABUN, ENTDATE, FILENAME)                 ");
            parameter.AppendSql("VALUES(                                                                    ");
            parameter.AppendSql(" :JOBDATE, :GUBUN, :WRTNO, :DRNO, :ENTSABUN, SYSDATE, :FILENAME)           ");

            parameter.Add("JOBDATE", item.JOBDATE);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("DRNO", item.DRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("ENTDATE", item.ENTDATE);
            parameter.Add("FILENAME", item.FILENAME);

            return ExecuteNonQuery(parameter);
        }


        public int UpdateFileNameByRID(string argFileName, string argROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_EMR_RESULT_WORK SET             ");
            parameter.AppendSql(" FILENAME = :FILENAME                                  ");
            parameter.AppendSql(" WHERE ROWID  = :ROWID                                 ");

            parameter.Add("FILENAME", argFileName);
            parameter.Add("ROWID", argROWID);

            return ExecuteNonQuery(parameter);
        }

        public HIC_EMR_RESULT_WORK GetPrtResultByWrtnoGubun(long argWrtno, string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT PRTRESULT                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_EMR_RESULT_WORK         ");
            parameter.AppendSql(" WHERE 1 = 1                                   ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                            ");
            parameter.AppendSql(" AND GUBUN = :GUBUN                            ");
            parameter.AppendSql(" AND JOBDATE = TRUNC(SYSDATE)                  ");

            parameter.Add("WRTNO", argWrtno);
            parameter.Add("GUBUN", argGubun);

            return ExecuteReaderSingle<HIC_EMR_RESULT_WORK>(parameter);
        }
    }
}
