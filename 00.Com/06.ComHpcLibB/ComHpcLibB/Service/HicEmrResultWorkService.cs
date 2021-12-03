
namespace ComHpcLibB
{

    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicEmrResultWorkService
    {
        private HicEmrResultWorkRepository hicEmrResultWorkRepository;

        public HicEmrResultWorkService()
        {
            this.hicEmrResultWorkRepository = new HicEmrResultWorkRepository();
        }

        public HIC_EMR_RESULT_WORK GetItemByWrtnoGubun(long argWrtno, string argGubun)
        {
            return hicEmrResultWorkRepository.GetItemByWrtnoGubun(argWrtno, argGubun);
        }

        public int Insert(HIC_EMR_RESULT_WORK item)
        {
            return hicEmrResultWorkRepository.Insert(item);
        }

        public int UpdateFileNameByRID(string argFileName, string argROWID)
        {
            return hicEmrResultWorkRepository.UpdateFileNameByRID(argFileName, argROWID);
        }

        public HIC_EMR_RESULT_WORK GetPrtResultByWrtnoGubun(long argWrtno, string argGubun)
        {
            return hicEmrResultWorkRepository.GetPrtResultByWrtnoGubun(argWrtno, argGubun);
        }

    }
}
