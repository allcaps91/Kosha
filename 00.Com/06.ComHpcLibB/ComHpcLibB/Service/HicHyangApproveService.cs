namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicHyangApproveService
    {

        private HicHyangApproveRepository hicHyangApproveRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicHyangApproveService()
        {
            this.hicHyangApproveRepository = new HicHyangApproveRepository();
        }

        public int Insert(HIC_HYANG_APPROVE item)
        {
            return hicHyangApproveRepository.Insert(item);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBDate(string strSDate, long nSabun)
        {
            return hicHyangApproveRepository.GetItembyBDate(strSDate, nSabun);
        }

        public List<HIC_HYANG_APPROVE> GetItembyWrtNo(long nWRTNO, string strBDate, long nPano)
        {
            return hicHyangApproveRepository.GetItembyWrtNo(nWRTNO, strBDate, nPano);
        }

        public int GetCountbyBDate(string strBDate, string strGubun = "")
        {
            return hicHyangApproveRepository.GetCountbyBDate(strBDate, strGubun);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBDate(string fstrBDate)
        {
            return hicHyangApproveRepository.GetItembyBDate(fstrBDate);
        }

        public HIC_HYANG_APPROVE GetItembyWrtnoBDate(long nWRTNO, string argBDate, string argSuCode = "", string argDept = "")
        {
            return hicHyangApproveRepository.GetItembyWrtnoBDate(nWRTNO, argBDate, argSuCode, argDept);
        }

        public List<HIC_HYANG_APPROVE> GetItembyWrtNoBDate(long nWRTNO, string strBDate, string strDept, string strSuCode)
        {
            return hicHyangApproveRepository.GetItembyWrtNoBDate(nWRTNO, strBDate, strDept, strSuCode);
        }

        public int UpdateDrSabunPproveTimebyRowId(string idNumber, string strROWID)
        {
            return hicHyangApproveRepository.UpdateDrSabunPproveTimebyRowId(idNumber, strROWID);
        }

        public int UpdateDrSabunbyRowId(string strDrSabun, string strApproveTime, string strCertNo, string strROWID)
        {
            return hicHyangApproveRepository.UpdateDrSabunbyRowId(strDrSabun, strApproveTime, strCertNo, strROWID);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBdatePaNo(string strBDate, long nPano)
        {
            return hicHyangApproveRepository.GetItembyBdatePaNo(strBDate, nPano);
        }

        public List<HIC_HYANG_APPROVE> GetItembyBDateDeptCode(string strBDate, string strJob, string strDeptCode)
        {
            return hicHyangApproveRepository.GetItembyBDateDeptCode(strBDate, strJob, strDeptCode);
        }

        public List<HIC_HYANG_APPROVE> GetItemListbyWrtNo(long argWrtNo, string argDept)
        {
            return hicHyangApproveRepository.GetItemListbyWrtNo(argWrtNo, argDept);
        }

        public int UpdatebyRowId(string rOWID)
        {
            return hicHyangApproveRepository.UpdatebyRowId(rOWID);
        }

        public HIC_HYANG_APPROVE GetItembyWrtNoSucode(long argWrtNo, string strSuCode)
        {
            return hicHyangApproveRepository.GetItembyWrtNoSucode(argWrtNo, strSuCode);
        }

        public int InsertSelect(string strROWID)
        {
            return hicHyangApproveRepository.InsertSelect(strROWID);
        }

        public int UpdateItembyRowId(double nQty, double nOldQty, string idNumber, string strROWID, string strApproveTime)
        {
            return hicHyangApproveRepository.UpdateItembyRowId(nQty, nOldQty, idNumber, strROWID, strApproveTime);
        }

        public List<HIC_HYANG_APPROVE> GetSucodeDrSabunQtybyWrtNo(long argWrtNo)
        {
            return hicHyangApproveRepository.GetSucodeDrSabunQtybyWrtNo(argWrtNo);
        }

        public List<HIC_HYANG_APPROVE> GetItembyWrtNoDeptCode(long argWrtNo, string strDeptCode)
        {
            return hicHyangApproveRepository.GetItembyWrtNoDeptCode(argWrtNo, strDeptCode);
        }

        public int UpdateDelDatebyRowId(string argRowId)
        {
            return hicHyangApproveRepository.UpdateDelDatebyRowId(argRowId);
        }

        public int InsertSelectbyRowId(string strROWID)
        {
            return hicHyangApproveRepository.InsertSelectbyRowId(strROWID);
        }

        public int UpdateAppTimebyRowId(string strGubun, string strAppTime, long nQty, string idNumber, string strROWID)
        {
            return hicHyangApproveRepository.UpdateAppTimebyRowId(strGubun, strAppTime, nQty, idNumber, strROWID);
        }

        public int UpdateGbSiteEntQtyByBdatePtnoSucode(string strGbSite, long nQty, string strBdate, string strPtno, string strSucode)
        {
            return hicHyangApproveRepository.UpdateGbSiteEntQtyByBdatePtnoSucode(strGbSite, nQty, strBdate, strPtno, strSucode);
        }

        public List<HIC_HYANG_APPROVE> GetItembySDateDeptCodeSite(string strSDate, string strDeptCode, string strGBSite)
        {
            return hicHyangApproveRepository.GetItembySDateDeptCodeSite(strSDate, strDeptCode, strGBSite);
        }

        public bool UpDateItemByRowid(string strGb, string strGbSite, int nEntQty, string strJuso, string rOWID)
        {
            try
            {
                hicHyangApproveRepository.UpDateItemByRowid(strGb, strGbSite, nEntQty, strJuso, rOWID);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateDelDatebySDateWrtnoSucode(string argDate, long argWrtno, string argDept, string argSuCode)
        {
            try
            {
                hicHyangApproveRepository.UpdateDelDatebySDateWrtnoSucode(argDate, argWrtno, argDept, argSuCode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int UpdateEntqty2(double argEntqty, long argWrtno, string argPano, string argSucode)
        {
            return hicHyangApproveRepository.UpdateEntqty2(argEntqty, argWrtno, argPano, argSucode);
        }

        public int DeleteHicHyangApprove(string argBdate, long argWrtno)
        {
            return hicHyangApproveRepository.DeleteHicHyangApprove(argBdate, argWrtno);
        }

        public int UpdateOcsSendTime(string argBdate)
        {
            return hicHyangApproveRepository.UpdateOcsSendTime(argBdate);
        }

        public List<HIC_HYANG_APPROVE> GetItemCountByBDate(string argBDATE)
        {
            return hicHyangApproveRepository.GetItemCountByBDate(argBDATE);
        }

        public List<HIC_HYANG_APPROVE> GetRowidSucodeByItems(string argBdate, long argWrtno, long argPano, string argPtno, string argSname)
        {
            return hicHyangApproveRepository.GetRowidSucodeByItems(argBdate, argWrtno, argPano, argPtno, argSname);
        }

        public int UpdateOcsSendTimeByItems(string argBdate, long argWrtno, long argPano, string argPtno, string argSname)
        {
            return hicHyangApproveRepository.UpdateOcsSendTimeByItems(argBdate, argWrtno, argPano, argPtno, argSname);
        }
        public List<HIC_HYANG_APPROVE> GetItemByBdateSucode(string argBDATE, string argSucode)
        {
            return hicHyangApproveRepository.GetItemByBdateSucode(argBDATE, argSucode);
        }
        public int GetCountByPtNoBdate(string argPtno, string strSdate)
        {
            return hicHyangApproveRepository.GetCountByPtNoBdate(argPtno, strSdate);
        }

    }
}
