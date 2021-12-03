
namespace ComHpcLibB.Repository
{

    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력하세요
    /// </summary>
    public class HicEmrResultRepository : BaseRepository
    {

        public HicEmrResultRepository()
        {

        }

        public int Insert(HIC_EMR_RESULT item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO ADMIN.HIC_EMR_RESULT(                            ");
            parameter.AppendSql(" JOBDATE, TABLE_NAME, GUBUN, WRTNO, SEQNO, DRNO, PRTRESULT         ");
            parameter.AppendSql(" ,HASHDATA, CERTDATA, ENTSABUN, ENTDATE, FILENAME)                 ");
            parameter.AppendSql("VALUES(                                                            ");
            parameter.AppendSql(" :JOBDATE, :TABLE_NAME, :GUBUN, :WRTNO, :SEQNO, :DRNO, ''          ");
            parameter.AppendSql(" , '', '', :ENTSABUN, SYSDATE, :FILENAME)                          ");


            parameter.Add("JOBDATE", item.JOBDATE);
            parameter.Add("TABLE_NAME", item.TABLE_NAME);
            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("WRTNO", item.WRTNO);
            parameter.Add("SEQNO", item.SEQNO);
            parameter.Add("DRNO", item.DRNO);
            parameter.Add("ENTSABUN", item.ENTSABUN);
            parameter.Add("FILENAME", item.FILENAME);
            
            return ExecuteNonQuery(parameter);
        }


        public int UpdateFileNameFTPByRID(string argFileNameFTP, string argROWID)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE ADMIN.HIC_EMR_RESULT SET              ");
            parameter.AppendSql(" FILENAME = :FILENAMEFTP                           ");
            parameter.AppendSql(" WHERE ROWID  = :ROWID                            ");

            parameter.Add("FILENAMEFTP", argFileNameFTP);
            parameter.Add("ROWID", argROWID);

            return ExecuteNonQuery(parameter);
        }


        public HIC_EMR_RESULT GetItemByWrtnoGubunSeqno(long argWrtno, string argGubun, string argSeqno)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT ROWID RID, JOBDATE              ");
            parameter.AppendSql("  FROM ADMIN.HIC_EMR_RESULT      ");
            parameter.AppendSql(" WHERE 1 = 1                           ");
            parameter.AppendSql(" AND WRTNO = :WRTNO                    ");
            parameter.AppendSql(" AND GUBUN = :GUBUN                    ");
            parameter.AppendSql(" AND SEQNO = :SEQNO                    ");


            parameter.Add("WRTNO", argWrtno);
            parameter.Add("GUBUN", argGubun);
            parameter.Add("SEQNO", argSeqno);

            return ExecuteReaderSingle<HIC_EMR_RESULT>(parameter);
        }

    }
}
