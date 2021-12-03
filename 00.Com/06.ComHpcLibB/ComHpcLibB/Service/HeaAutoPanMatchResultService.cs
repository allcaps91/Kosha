namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;

    public class HeaAutoPanMatchResultService
    {
        private HeaAutoPanMatchResultRepository heaAutoPanMatchResultRepository;

        public List<HEA_AUTOPAN_MATCH_RESULT> GetItembyWrtno(string argWrtNo, string argJepNo)
        {
            return heaAutoPanMatchResultRepository.GetItembyWrtno(argWrtNo, argJepNo);
        }
    }
}
