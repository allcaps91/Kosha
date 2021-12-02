namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class GroupCodeExamDisplayService
    {
        private GroupCodeExamDisplayRepository groupCodeExamDisplayRepository;

        /// <summary>
        /// 
        /// </summary>
        public GroupCodeExamDisplayService()
        {
            this.groupCodeExamDisplayRepository = new GroupCodeExamDisplayRepository();
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetListByGWrtno(long nGWRTNO, long nWRTNO)
        {
            return groupCodeExamDisplayRepository.GetListByGWrtno(nGWRTNO, nWRTNO);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHicListByWrtno(long argWRTNO)
        {
            return groupCodeExamDisplayRepository.GetHicListByWrtno(argWRTNO);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaListByWrtno(long argWRTNO)
        {
            return groupCodeExamDisplayRepository.GetHeaListByWrtno(argWRTNO);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHicListByGroupCode(List<string> suInfo)
        {
            return groupCodeExamDisplayRepository.GetHicListByGroupCode(suInfo);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaListByGroupCode(List<string> suInfo)
        {
            return groupCodeExamDisplayRepository.GetHeaListByGroupCode(suInfo);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetListExcodeByGrpCodeList(List<string> lstgrpCode)
        {
            return groupCodeExamDisplayRepository.GetListExcodeByGrpCodeList(lstgrpCode);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaListExcodeByGrpCodeList(List<string> lstgrpCode)
        {
            return groupCodeExamDisplayRepository.GetHeaListExcodeByGrpCodeList(lstgrpCode);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetExamListByWrtno(long nWRTNO)
        {
            return groupCodeExamDisplayRepository.GetExamListByWrtno(nWRTNO);
        }

        public List<GROUPCODE_EXAM_DISPLAY> GetHeaExamListByGroupCode(string gRPCODE)
        {
            return groupCodeExamDisplayRepository.GetHeaExamListByGroupCode(gRPCODE);
        }
    }
}
