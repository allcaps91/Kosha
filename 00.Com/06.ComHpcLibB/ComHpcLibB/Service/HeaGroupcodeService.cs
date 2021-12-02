namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase.Mvc;
    using ComBase.Controls;
    using Microsoft.VisualBasic;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HeaGroupcodeService
    {
        
        private HeaGroupcodeRepository heaGroupcodeRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaGroupcodeService()
        {
			this.heaGroupcodeRepository = new HeaGroupcodeRepository();
        }

        
        public List<HEA_GROUPCODE> Read_Hea_GroupCode(string strExams1)
        {
            return heaGroupcodeRepository.Read_Hea_GroupCode(strExams1);
        }

        public HEA_GROUPCODE GetYNamebyWrtNo(long nWRTNO)
        {
            return heaGroupcodeRepository.GetYNamebyWrtNo(nWRTNO);
        }

        public List<HEA_GROUPCODE> GetCodeNameByLikeName(string argName)
        {
            return heaGroupcodeRepository.GetCodeNameByLikeName(argName);
        }

        public HEA_GROUPCODE GetItemByCode(string argJong)
        {
            return heaGroupcodeRepository.GetItemByCode(argJong);
        }

        public HEA_GROUPCODE GetItemByCodeDeldate(string argJong)
        {
            return heaGroupcodeRepository.GetItemByCodeDeldate(argJong);
        }

        public List<HEA_GROUPCODE> GetListByJongLtdCode(string argJong, long argLtdCode, bool bDel)
        {
            return heaGroupcodeRepository.GetListByJongLtdCode(argJong, argLtdCode, bDel);
        }

        public int Insert(HEA_GROUPCODE item)
        {
            return heaGroupcodeRepository.Insert(item);
        }

        public int UpDate(HEA_GROUPCODE item)
        {
            return heaGroupcodeRepository.UpDate(item);
        }

        public int Delete(string argRowid)
        {
            return heaGroupcodeRepository.Delete(argRowid);
        }

        public string CheckData(HEA_GROUPCODE item)
        {
            string rtnVal = string.Empty;

            if (item.CODE.Length != 5) { rtnVal = "묶음코드가 5자리가 아닙니다."; }
            if (item.NAME.Trim() == "") { rtnVal = "묶음코드명이 공란입니다."; }
            if (item.YNAME.Trim() == "") { rtnVal = "묶음약어가 공란입니다."; }
            if (item.YNAME.Length > 10) { rtnVal = "묶음약어 10자를 초과합니다."; }
            if (item.JONG.Trim() == "") { rtnVal = "종검종류가 공란입니다."; }
            if (item.BURATE.Trim() == "") { rtnVal = "부담방법이 공란입니다."; }
            if (item.GBSEX.Trim() == "") { rtnVal = "성별 구분이 공란입니다."; }
            if (item.OLDAMT != 0 && item.SUDATE == null) { rtnVal = "종전금액은 있으나 변경일자가 공란"; }

            //회사검진 Check
            if ((string.Compare(item.JONG, "21") >= 0 && string.Compare(item.JONG, "29") <= 0) || item.JONG == "69") 
            {
                if (item.LTDCODE == 0) { rtnVal = "계약회사코드가 공란입니다."; }
                if (item.SDATE == null) { rtnVal = "계약일자가 공란입니다."; }
                if (item.JONG != "69")
                {
                    if (!Information.IsNumeric(item.CODE)) { rtnVal = "회사검진은 코드 10001~99999만 가능합니다."; }
                    if (item.CODE.To<long>() < 10001 || item.CODE.To<long>() > 99999) { rtnVal = "회사검진은 10001~99999만 코드로 사용이 가능합니다."; }
                }
            }
            else
            { 
                if (item.LTDCODE > 0) { rtnVal = "계약회사코드가 공란이 아닙니다."; }

                if (string.Compare(item.JONG.Trim(), "11") >= 0 && string.Compare(item.JONG.Trim(), "19") <= 0)
                {
                    if (VB.Left(item.CODE, 1) != "A") { rtnVal = "개인종검은 A0001~A9999까지 사용이 가능함"; }
                }
                else if (string.Compare(item.JONG.Trim(), "31") >= 0 && string.Compare(item.JONG.Trim(), "39") <= 0)
                {
                    if (VB.Left(item.CODE, 1) != "C") { rtnVal = "정밀종검은 C0001~C9999까지 사용이 가능함"; }
                }
                else if (string.Compare(item.JONG.Trim(), "41") >= 0 && string.Compare(item.JONG.Trim(), "49") <= 0)
                {
                    if (VB.Left(item.CODE, 1) != "D") { rtnVal = "숙박종검은 D0001~D9999까지 사용이 가능함"; }
                }
                else if (item.JONG.Trim() == "**")
                {
                    if (VB.Left(item.CODE, 1) != "Z") { rtnVal = "선택검사는 Z0001~Z9999까지 사용이 가능함"; }
                    if (item.GBSELECT == "N") { rtnVal = "선택검사를 클릭 않함"; }
                }
                else
                {
                    rtnVal = "종검종류 선택 오류입니다.";
                }
            }

            return rtnVal;
        }

        public IList<HEA_GROUPCODE> GetListByItem(long nLtdCode, string strSex, string strDate)
        {
            return heaGroupcodeRepository.GetListByItem(nLtdCode, strSex, strDate);
        }

        public List<HEA_GROUPCODE> GetListByAll(string argCode)
        {
            return heaGroupcodeRepository.GetListByAll(argCode);
        }

        public string GetGbSelectByCode(string gRPCODE)
        {
            return heaGroupcodeRepository.GetGbSelectByCode(gRPCODE);
        }

        public long GetLongServiceWorkerTotalAmtByWrtno(long fnWRTNO)
        {
            return heaGroupcodeRepository.GetLongServiceWorkerTotalAmtByWrtno(fnWRTNO);
        }
        public string GetJongByCode(string argGroupCode)
        {
            return heaGroupcodeRepository.GetJongByCode(argGroupCode);
        }

    }
}
