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

            if (item.CODE.Length != 5) { rtnVal = "?????????? 5?????? ????????."; }
            if (item.NAME.Trim() == "") { rtnVal = "???????????? ??????????."; }
            if (item.YNAME.Trim() == "") { rtnVal = "?????????? ??????????."; }
            if (item.YNAME.Length > 10) { rtnVal = "???????? 10???? ??????????."; }
            if (item.JONG.Trim() == "") { rtnVal = "?????????? ??????????."; }
            if (item.BURATE.Trim() == "") { rtnVal = "?????????? ??????????."; }
            if (item.GBSEX.Trim() == "") { rtnVal = "???? ?????? ??????????."; }
            if (item.OLDAMT != 0 && item.SUDATE == null) { rtnVal = "?????????? ?????? ?????????? ????"; }

            //???????? Check
            if ((string.Compare(item.JONG, "21") >= 0 && string.Compare(item.JONG, "29") <= 0) || item.JONG == "69") 
            {
                if (item.LTDCODE == 0) { rtnVal = "?????????????? ??????????."; }
                if (item.SDATE == null) { rtnVal = "?????????? ??????????."; }
                if (item.JONG != "69")
                {
                    if (!Information.IsNumeric(item.CODE)) { rtnVal = "?????????? ???? 10001~99999?? ??????????."; }
                    if (item.CODE.To<long>() < 10001 || item.CODE.To<long>() > 99999) { rtnVal = "?????????? 10001~99999?? ?????? ?????? ??????????."; }
                }
            }
            else
            { 
                if (item.LTDCODE > 0) { rtnVal = "?????????????? ?????? ????????."; }

                if (string.Compare(item.JONG.Trim(), "11") >= 0 && string.Compare(item.JONG.Trim(), "19") <= 0)
                {
                    if (VB.Left(item.CODE, 1) != "A") { rtnVal = "?????????? A0001~A9999???? ?????? ??????"; }
                }
                else if (string.Compare(item.JONG.Trim(), "31") >= 0 && string.Compare(item.JONG.Trim(), "39") <= 0)
                {
                    if (VB.Left(item.CODE, 1) != "C") { rtnVal = "?????????? C0001~C9999???? ?????? ??????"; }
                }
                else if (string.Compare(item.JONG.Trim(), "41") >= 0 && string.Compare(item.JONG.Trim(), "49") <= 0)
                {
                    if (VB.Left(item.CODE, 1) != "D") { rtnVal = "?????????? D0001~D9999???? ?????? ??????"; }
                }
                else if (item.JONG.Trim() == "**")
                {
                    if (VB.Left(item.CODE, 1) != "Z") { rtnVal = "?????????? Z0001~Z9999???? ?????? ??????"; }
                    if (item.GBSELECT == "N") { rtnVal = "?????????? ???? ????"; }
                }
                else
                {
                    rtnVal = "???????? ???? ??????????.";
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
