

namespace ComHpcLibB.Service
{

    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicEmrResultService 
    {
        private HicEmrResultRepository hicEmrResultRepository;

        public HicEmrResultService()
        {
            this.hicEmrResultRepository = new HicEmrResultRepository();
        }


        public int Insert(HIC_EMR_RESULT item)
        {
            return hicEmrResultRepository.Insert(item);
        }

        public int UpdateFileNameFTPByRID(string argFileNameFTP, string argROWID)
        {
            return hicEmrResultRepository.UpdateFileNameFTPByRID(argFileNameFTP, argROWID);
        }


        public HIC_EMR_RESULT GetItemByWrtnoGubunSeqno(long argWrtno, string argGubun, string argSeqno)
        {
            return hicEmrResultRepository.GetItemByWrtnoGubunSeqno(argWrtno, argGubun, argSeqno);
        }

    }
}
