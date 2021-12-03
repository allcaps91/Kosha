namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuPatientService
    {
        
        private HicJepsuPatientRepository hicJepsuPatientRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuPatientService()
        {
			this.hicJepsuPatientRepository = new HicJepsuPatientRepository();
        }

        public HIC_JEPSU_PATIENT GetItembyWrtNo(long fnWRTNO)
        {
            return hicJepsuPatientRepository.GetItembyWrtNo(fnWRTNO);
        }

        public HIC_JEPSU_PATIENT GetItemByWrtnoJepdate(long fnWRTNO, string argJepdate)
        {
            return hicJepsuPatientRepository.GetItemByWrtnoJepdate(fnWRTNO, argJepdate);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyWrtNoLtdCode(List<string> strList, long nWrtNo, long nLtdCode, string strSort)
        {
            return hicJepsuPatientRepository.GetItembyWrtNoLtdCode(strList, nWrtNo, nLtdCode, strSort);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateGjYear(string strFrDate, string strToDate, string strGjYear, string strBangi, string strLtdCode, string strTongbo)
        {
            return hicJepsuPatientRepository.GetItembyJepDateGjYear(strFrDate, strToDate, strGjYear, strBangi, strLtdCode, strTongbo);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateGjjongLtdCode(string strDate1, string strDate2, string strJong, long nLtdCode)
        {
            return hicJepsuPatientRepository.GetItembyJepDateGjjongLtdCode(strDate1, strDate2, strJong, nLtdCode);
        }

        public IList<HIC_JEPSU_PATIENT> GetNhicListByDate(string argFDate, string argTDate)
        {
            return hicJepsuPatientRepository.GetNhicListByDate(argFDate, argTDate);
        }

        public List<HIC_JEPSU_PATIENT> GetSecondListByTongboDate(string argFDate, string argTDate, string argView, string argJong, string argSName = "", long argLtdCode = 0, string argHea = "")
        {
            List<string> lstJob =new List<string>();

            switch (argJong)
            {
                case "0": lstJob.Add("11"); lstJob.Add("14"); lstJob.Add("23"); lstJob.Add("24"); break;
                case "1": lstJob.Add("11"); lstJob.Add("23"); break;
                case "2": lstJob.Add("24"); break;
                case "3": lstJob.Add("21"); lstJob.Add("22"); lstJob.Add("24"); lstJob.Add("30"); lstJob.Add("49"); break;
                //case "4": lstJob.Add("21"); lstJob.Add("22"); lstJob.Add("24"); lstJob.Add("30"); lstJob.Add("49"); break;
                //case "5": lstJob.Add("41"); lstJob.Add("42"); lstJob.Add("43"); break;
                //case "6": lstJob.Add("69"); break;
                default: break;
            }

            return hicJepsuPatientRepository.GetSecondListByTongboDate(argFDate, argTDate, argView, lstJob, argSName, argLtdCode, argJong, argHea);
        }

        public List<HIC_JEPSU_PATIENT> GetListByDate(string argFDate, string argTDate, string argJong = "", long nLtdCode = 0)
        {
            return hicJepsuPatientRepository.GetListByDate(argFDate, argTDate, argJong, nLtdCode);
        }

        public HIC_JEPSU_PATIENT GetItembyJuMin(string strJUMIN, string strJOBDATE)
        {
            return hicJepsuPatientRepository.GetItembyJuMin(strJUMIN, strJOBDATE);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyWrtNoJepDateSNameLtdCode(long nWrtNo, string strFrDate, string strToDate, string strResult, string strPart, string strGjJong, string strSchool, string strSName, long nLtdCode, string strHea, string strSort, string strUcode)
        {
            return hicJepsuPatientRepository.GetItembyWrtNoJepDateSNameLtdCode(nWrtNo, strFrDate, strToDate, strResult, strPart, strGjJong, strSchool, strSName, nLtdCode, strHea, strSort, strUcode);
        }

        public List<HIC_JEPSU_PATIENT> GetNightItembyJepDateSName(string strFrDate, string strToDate, string strSName, string fstrID)
        {
            return hicJepsuPatientRepository.GetNightItembyJepDateSName(strFrDate, strToDate, strSName, fstrID);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateLtdCode(string strFrDate, string strToDate, string strGjJong, long nLtdCode)
        {
            return hicJepsuPatientRepository.GetItembyJepDateLtdCode(strFrDate, strToDate, strGjJong, nLtdCode);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateSname(string strFrDate, string strToDate, string strSname)
        {
            return hicJepsuPatientRepository.GetItembyJepDateSname(strFrDate, strToDate, strSname );
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateSDateLtdCode(string strGubun, string strFrDate, string strToDate, long nLtdCode, string strToDate1, string sGjJong)
        {
            return hicJepsuPatientRepository.GetItembyJepDateSDateLtdCode(strGubun, strFrDate, strToDate, nLtdCode, strToDate1, sGjJong);
        }

        public List<HIC_JEPSU_PATIENT> GetSendListItembyJepDate(string strFDate, string strTDate, string strChasu, string strJohap, long nLtdCode)
        {
            return hicJepsuPatientRepository.GetSendListItembyJepDate(strFDate, strTDate, strChasu, strJohap, nLtdCode);
        }

        public int GetCountbyJuminGjYear(string strJumin, string strGjYear, string strJong, string strJepdate)
        {
            return hicJepsuPatientRepository.GetCountbyJuminGjYear(strJumin, strGjYear, strJong, strJepdate);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyWebPrintSend(string strFDate, string gstrSysDate)
        {
            return hicJepsuPatientRepository.GetItembyWebPrintSend(strFDate, gstrSysDate);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyTongBoDate(string strFDate, string strTDate, List<string> strJong)
        {
            return hicJepsuPatientRepository.GetItembyTongBoDate(strFDate, strTDate, strJong);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyRecvTime(string strFDate, string strTDate)
        {
            return hicJepsuPatientRepository.GetItembyRecvTime(strFDate, strTDate);
        }

        public int GetCountbyJumin2(string strRDate, string strJumin2, List<string> B04_NOT_PATIENT)
        {
            return hicJepsuPatientRepository.GetCountbyJumin2(strRDate, strJumin2, B04_NOT_PATIENT);
        }

        public HIC_JEPSU_PATIENT GetLtdCodebyRDateJumin2SName(string strRDate, string strJumin2, List<string> b04_NOT_PATIENT)
        {
            return hicJepsuPatientRepository.GetLtdCodebyRDateJumin2SName(strRDate, strJumin2, b04_NOT_PATIENT);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateMirNo1(string sJepDate, long argMirno)
        {
            return hicJepsuPatientRepository.GetItembyJepDateMirNo1(sJepDate, argMirno);
        }

        public List<HIC_JEPSU_PATIENT> GetFirstMunbyJepDateMirNo1(string sJepDate, long argMirno)
        {
            return hicJepsuPatientRepository.GetFirstMunbyJepDateMirNo1(sJepDate, argMirno);
        }

        public List<HIC_JEPSU_PATIENT> GetLifeItembyJepDateMirno1(string sJepDate, long argMirno)
        {
            return hicJepsuPatientRepository.GetLifeItembyJepDateMirno1(sJepDate, argMirno);
        }

        public List<HIC_JEPSU_PATIENT> GetDentalItembyJepDateMirNo2(string sJepDate, long argMirno)
        {
            return hicJepsuPatientRepository.GetDentalItembyJepDateMirNo2(sJepDate, argMirno);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateMirNoJong(string sJepDate, string strJong, long argMirno)
        {
            return hicJepsuPatientRepository.GetItembyJepDateMirNoJong(sJepDate, strJong, argMirno);
        }

        public List<HIC_JEPSU_PATIENT> GetItembyJepDateMirNo(string strFrDate, string strToDate, string strMirno, string strJong)
        {
            return hicJepsuPatientRepository.GetItembyJepDateMirNo(strFrDate, strToDate, strMirno, strJong);
        }

        public List<HIC_JEPSU_PATIENT> GetListByJepdateJong(string argFDate, string argTDate, string argJong , string argSname, long nLtdCode, string argTongbo, string argGubun1, string argSort )
        {
            return hicJepsuPatientRepository.GetListByJepdateJong(argFDate, argTDate, argJong, argSname, nLtdCode, argTongbo, argGubun1, argSort);
        }
        public List<HIC_JEPSU_PATIENT> GetListByItems(string argFDate, string argTDate, string argSname,string argWrtno, string argLtdCode, string argRePrt, string argSort, string argJong)
        { 
            return hicJepsuPatientRepository.GetListByItems(argFDate, argTDate, argSname, argWrtno, argLtdCode, argRePrt, argSort, argJong);
        }
        public HIC_JEPSU_PATIENT GetEndoItembyWrtNo(long argWrtno)
        {
            return hicJepsuPatientRepository.GetEndoItembyWrtNo(argWrtno);
        }

        
    }
}
