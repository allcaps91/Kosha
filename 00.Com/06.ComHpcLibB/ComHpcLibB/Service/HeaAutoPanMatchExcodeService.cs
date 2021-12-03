namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;

    public class HeaAutoPanMatchExcodeService
    {
        private HeaAutoPanMatchExcodeRepository heaAutoPanMatchExcodeRepository;

        public List<HEA_AUTOPAN_MATCH_EXCODE> GetItembyWrtNo(string argWrtNo)
        {
            return heaAutoPanMatchExcodeRepository.GetItembyWrtNo(argWrtNo);
        }
    }
}
