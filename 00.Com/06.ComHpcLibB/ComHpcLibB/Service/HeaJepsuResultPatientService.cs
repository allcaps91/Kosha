namespace ComHpcLibB.Service
{

    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;

    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HeaJepsuResultPatientService 
    {
        private HeaJepsuResultPatientRepository heaJepsuResultPatientRepository;

        public HeaJepsuResultPatientService()
        {
            this.heaJepsuResultPatientRepository = new HeaJepsuResultPatientRepository();
        }


        public List<HEA_JEPSU_RESULT_PATIENT> GetListBySdateLtdCodeExcode(string argFDate, string argTDate, long argLtdCode, List<string> argExcode)
        {
            return heaJepsuResultPatientRepository.GetListBySdateLtdCodeExcode(argFDate, argTDate, argLtdCode, argExcode);
        }

        public List<HEA_JEPSU_RESULT_PATIENT> GetResultBySdateWrtnoLtdcodeExcode(string argFDate, string argTDate, long argWrtno, long argLtdCode, List<string> argExcode)
        {
            return heaJepsuResultPatientRepository.GetResultBySdateWrtnoLtdcodeExcode(argFDate, argTDate, argWrtno, argLtdCode, argExcode);
        }
    }
}
