

namespace ComHpcLibB.Service
{

    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class OcsOorderService
    {
        private OcsOorderRepository ocsOorderRepository;

        public OcsOorderService()
        {
            this.ocsOorderRepository = new OcsOorderRepository();
        }

        public int DeleteOcsOorder(string argPtno, string argBdate, string argDeptcode, long argWrtno)
        {
            return ocsOorderRepository.DeleteOcsOorder(argPtno, argBdate, argDeptcode, argWrtno);
        }


    }
}
