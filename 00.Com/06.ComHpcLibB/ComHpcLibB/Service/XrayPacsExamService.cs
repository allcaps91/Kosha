namespace ComHpcLibB.Service
{
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 
    /// </summary>
    public class XrayPacsExamService
    {
        private XrayPacsExamRepository xrayPacsExamRepository;

        /// <summary>
        /// 
        /// </summary>
        public XrayPacsExamService()
        {
            this.xrayPacsExamRepository = new XrayPacsExamRepository();
        }
        
        public XRAY_PACS_EXAM GetItembyExamName(string strCode, string strXCode, string strName)
        {
            return xrayPacsExamRepository.GetItembyExamName(strCode, strXCode, strName);
        }

        public XRAY_PACS_EXAM GetItemSeqNobyExamName(string strCode, string strXCode)
        {
            return xrayPacsExamRepository.GetItemSeqNobyExamName(strCode, strXCode);
        }

        
        public int Insert(XRAY_PACS_EXAM item)
        {
            return xrayPacsExamRepository.Insert(item);
        }

    }
}
