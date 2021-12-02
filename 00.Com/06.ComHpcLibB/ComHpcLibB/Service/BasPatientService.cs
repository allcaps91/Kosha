namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasPatientService
    {

        private BasPatientRepository basPatientRepository;

        /// <summary>
        /// 
        /// </summary>
        public BasPatientService()
        {
            this.basPatientRepository = new BasPatientRepository();
        }

        public IList<BAS_PATIENT> GetPatientByJuminNo(string argJumin1, string argJumin2)
        {
            return basPatientRepository.GetPatientByJuminNo(argJumin1, argJumin2);
        }

        public string GetPaNOByJuminNo(string strJumin1, string strJumin2, string strJumin3)
        {
            return basPatientRepository.GetPaNOByJuminNo(strJumin1, strJumin2, strJumin3);
        }

        public string GetPaNobyJumin1Jumin2Jumin3(string strJumin1, string strJumin2, string strJumin3)
        {
            return basPatientRepository.GetPaNobyJumin1Jumin2Jumin3(strJumin1, strJumin2, strJumin3);
        }


        public BAS_PATIENT GetItembyPano(string strPano)
        {
            return basPatientRepository.GetItembyPano(strPano);
        }

        public BAS_PATIENT GetPaNobySName(string argSName, string argBirth, string argHPhone)
        {
            return basPatientRepository.GetPaNobySName(argSName, argBirth, argHPhone);
        }

        public BAS_PATIENT GetItembyJumin1Jumin3(string argJumin1, string argJumin3)
        {
            return basPatientRepository.GetItembyJumin1Jumin3(argJumin1, argJumin3);
        }

        public string GetPaNobyPaNo(string strPtNo)
        {
            return basPatientRepository.GetPaNobyPaNo(strPtNo);
        }

        public BAS_PATIENT GetPaNobyJumin1Jumin3(string strJumin1, string strJumin2)
        {
            return basPatientRepository.GetPaNobyJumin1Jumin3(strJumin1, strJumin2);
        }

        public string GetPaNobyJumin1Jumin2(string strJumin1, string strJumin2)
        {
            return basPatientRepository.GetPaNobyJumin1Jumin2(strJumin1, strJumin2);
        }

        public int Insert(BAS_PATIENT item)
        {
            return basPatientRepository.Insert(item);
        }

        public List<BAS_PATIENT> GetListByJuminNo(string argJumin1, string argJumin2, List<string> b04_NOT_PATIENT)
        {
            return basPatientRepository.GetListByJuminNo(argJumin1, argJumin2, b04_NOT_PATIENT);
        }

        public BAS_PATIENT GetCountbyJumin1Jumin3(string strJumin1, string strJumin3)
        {
            return basPatientRepository.GetCountbyJumin1Jumin3(strJumin1, strJumin3);
        }

        public BAS_PATIENT GetItembyJumin1Jumin3NotInSName(string strJumin1, string strJumin2, List<string> b04_NOT_PATIENT)
        {
            return basPatientRepository.GetItembyJumin1Jumin3NotInSName(strJumin1, strJumin2, b04_NOT_PATIENT);
        }

        public BAS_PATIENT GetPaNoRowIdbyPaNo(string strPtNo)
        {
            return basPatientRepository.GetPaNoRowIdbyPaNo(strPtNo);
        }

        public BAS_PATIENT GetPaNoRowIdbyJumin1Jumin3(string strJumin1, string strJumin3)
        {
            return basPatientRepository.GetPaNoRowIdbyJumin1Jumin3(strJumin1, strJumin3);
        }

        public int UpdateSNameHphoneTelbyRowId(string strSname, string strHPhone, string strTel, string strROWID)
        {
            return basPatientRepository.UpdateSNameHphoneTelbyRowId(strSname, strHPhone, strTel, strROWID);
        }

        public string GetJuminbyPaNo(string strPtNo)
        {
            return basPatientRepository.GetJuminbyPaNo(strPtNo);
        }

        public string GetPaNobyJumin1(string fstrJumin1, string fstrJumin2, string fstrJumin3)
        {
            return basPatientRepository.GetPaNobyJumin1(fstrJumin1, fstrJumin2, fstrJumin3);
        }
    }
}
