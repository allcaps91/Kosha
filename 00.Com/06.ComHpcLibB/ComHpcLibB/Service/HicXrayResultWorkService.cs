
namespace ComHpcLibB
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicXrayResultWorkService
    {
        private HicXrayResultWorkRepository hicXrayResultWorkRepository;

        public HicXrayResultWorkService()
        {
            this.hicXrayResultWorkRepository = new HicXrayResultWorkRepository();
        }


        public List<HIC_XRAY_RESULT_WORK> GetListByItem(string argDate, string argPano)
        {
            return hicXrayResultWorkRepository.GetListByItem(argDate, argPano);
        }

        public IList<HIC_XRAY_RESULT_WORK> GetChulListByItem(string argDate, long argLtdCode)
        {
            return hicXrayResultWorkRepository.GetChulListByItem(argDate, argLtdCode);
        }


    }
}
