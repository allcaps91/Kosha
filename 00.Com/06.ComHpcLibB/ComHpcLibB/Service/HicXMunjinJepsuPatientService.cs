
namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicXMunjinJepsuPatientService
    {
        private HicXMunjinJepsuPatientRepository hicXMunjinJepsuPatientRepository;
        public HicXMunjinJepsuPatientService()
        {
            this.hicXMunjinJepsuPatientRepository = new HicXMunjinJepsuPatientRepository();
        }
        public List<HIC_X_MUNJIN_JEPSU_PATIENT> GetListByItems(string argFDate, string argTDate, string argSname, string argWrtno, string argLtdcode, string argRePrt, string argSort, string argGubun)
        {
            return hicXMunjinJepsuPatientRepository.GetListByItems(argFDate, argTDate, argSname, argWrtno, argLtdcode, argRePrt, argSort, argGubun);
        }

        public HIC_X_MUNJIN_JEPSU_PATIENT GetAllItemsByWrtno(long argWrtno)
        {
            return hicXMunjinJepsuPatientRepository.GetAllItemsByWrtno(argWrtno);
        }

        

    }
}
