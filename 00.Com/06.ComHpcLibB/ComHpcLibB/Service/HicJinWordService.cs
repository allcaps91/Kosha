
namespace ComHpcLibB.Service
{
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicJinWordService
    {

        private HicJinWordRepository hicJinWordRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicJinWordService()
        {
            this.hicJinWordRepository = new HicJinWordRepository();
        }

        public HIC_JIN_WORD GetItemByGubunJdate(string strGubun, string strJDate)
        {
            return hicJinWordRepository.GetItemByGubunJdate(strGubun, strJDate);

        }
    }
}
