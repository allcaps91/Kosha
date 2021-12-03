namespace ComHpcLibB.Repository
{
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class XrayPacsExamRepository : BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public XrayPacsExamRepository()
        {
        }

        public XRAY_PACS_EXAM GetItembyExamName(string strCode, string strXcode, string strName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT EXAMCODE                                    ");
            parameter.AppendSql(" FROM ADMIN.XRAY_PACS_EXAM                          ");
            parameter.AppendSql(" WHERE 1=1                                         ");
            if (strCode != "")
            {
                parameter.AppendSql(" AND EXAMCODE LIKE :CODE                       ");
            }
            else
            {
                parameter.AppendSql(" AND EXAMCODE LIKE :XCODE                       ");
            }
            if (!strName.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND EXAMNAME = :EXAMNAME                   ");
            }

            if (strCode != "")
            { 
                parameter.AddLikeStatement("CODE", strCode);
            }
            else
            {
                parameter.AddLikeStatement("XCODE", strXcode);
            }
            
            parameter.Add("EXAMNAME", strName);

            return ExecuteReaderSingle<XRAY_PACS_EXAM>(parameter);
        }

        public XRAY_PACS_EXAM GetItemSeqNobyExamName(string strCode, string strXcode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT MAX(EXAMCODE) AS EXAMCODE                   ");
            parameter.AppendSql(" FROM ADMIN.XRAY_PACS_EXAM                          ");
            parameter.AppendSql(" WHERE 1=1                                         ");
            if (!strCode.IsNullOrEmpty())
            {
                parameter.AppendSql(" AND EXAMCODE LIKE :CODE                       ");
            }
            else
            {
                parameter.AppendSql(" AND EXAMCODE LIKE :XCODE                      ");
            }

            if (!strCode.IsNullOrEmpty())
            {
                parameter.AddLikeStatement("CODE", strCode);
            }
            else
            {
                parameter.AddLikeStatement("XCODE", strXcode);
            }

            return ExecuteReaderSingle<XRAY_PACS_EXAM>(parameter);
        }
        

        public int Insert(XRAY_PACS_EXAM item)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("INSERT NTO ADMIN.XRAY_PACS_EXAM(                                                ");
            parameter.AppendSql(" EXAMCODE, EXAMNAME, SHORTNAME, MODALITY, SECTCODE)                            ");
            parameter.AppendSql("VALUES                                                                         ");
            parameter.AppendSql(" (:EXAMCODE, :EXAMNAME, '', :MODALITY, '')                                     ");

            parameter.Add("EXAMCODE", item.EXAMCODE);
            parameter.Add("EXAMNAME", item.EXAMNAME);
            parameter.Add("MODALITY", item.MODALITY);

            return ExecuteNonQuery(parameter);
        }
    }
}
