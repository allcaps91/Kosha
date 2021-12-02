namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class EtcJupmstService
    {
        
        private EtcJupmstRepository etcJupmstRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public EtcJupmstService()
        {
			this.etcJupmstRepository = new EtcJupmstRepository();
        }

        public int Insert_Etc_JupMst(ETC_JUPMST item)
        {
            return etcJupmstRepository.Insert_Etc_JupMst(item);
        }

        public string GetRowIdbyPaNo(string fstrPano, string fstrBDate, string fstrDeptCode)
        {
            return etcJupmstRepository.GetRowIdbyPaNo(fstrPano, fstrBDate, fstrDeptCode);
        }

        public List<ETC_JUPMST> GetItemEtcJupMst()
        {
            return etcJupmstRepository.GetItemEtcJupMst();
        }

        public List<ETC_JUPMST> GetItemStress()
        {
            return etcJupmstRepository.GetItemStress();
        }

        public List<ETC_JUPMST> GetItembyRdate()
        {
            return etcJupmstRepository.GetItembyRdate();
        }

        public ETC_JUPMST GetImageGbnbyRowId(string strROWID)
        {
            return etcJupmstRepository.GetImageGbnbyRowId(strROWID);
        }

        public int GetCountbyRowId(string strRowId)
        {
            return etcJupmstRepository.GetCountbyRowId(strRowId);
        }

        public List<ETC_JUPMST> GetStress_SogenbyPtNo(string fstrPtno, string fstrJepDate)
        {
            return etcJupmstRepository.GetStress_SogenbyPtNo(fstrPtno, fstrJepDate);
        }

        public List<ETC_JUPMST> GetItembyPtNoJepDate(string fstrPtno, string fstrJepDate)
        {
            return etcJupmstRepository.GetItembyPtNoJepDate(fstrPtno, fstrJepDate);
        }

        public ETC_JUPMST GetIetmbyRowId(string argRowId)
        {
            return etcJupmstRepository.GetIetmbyRowId(argRowId);
        }

        public ETC_JUPMST GetImagebyRowId(string argRowId)
        {
            return etcJupmstRepository.GetImagebyRowId(argRowId);
        }

        public List<ETC_JUPMST> GetRowIdbyPtNoBDate(string fstrPtno, string fstrJepDate, string strOrderCodeYN, string strGubun)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDate(fstrPtno, fstrJepDate, strOrderCodeYN, strGubun);
        }

        public int GetCountbyPtNo(string fstrPtno, string fstrJepDate, string strGubun, string strTeamPanoYN)
        {
            return etcJupmstRepository.GetCountbyPtNo(fstrPtno, fstrJepDate, strGubun, strTeamPanoYN);
        }

        public string GetRowidByPtNoBDateOrderCode(string argPtno, string argOrderCode, string argDate, string argDept)
        {
            return etcJupmstRepository.GetRowidByPtNoBDateOrderCode(argPtno, argOrderCode, argDate, argDept);
        }

        public int UpDate_Etc_JupMst(ETC_JUPMST dJUPMST)
        {
            return etcJupmstRepository.UpDate_Etc_JupMst(dJUPMST);
        }

        public int UpDate_Etc_JupMst_Del(string argRowid)
        {
            return etcJupmstRepository.UpDate_Etc_JupMst_Del(argRowid);
        }

        public int UpdateImageGbJobSogenbyRowId(string strImage_Gbn, string strGbJob, string strSogen, string fstrROWID)
        {
            return etcJupmstRepository.UpdateImageGbJobSogenbyRowId(strImage_Gbn, strGbJob, strSogen, fstrROWID);
        }

        public int UpdateGbFtpFilePathbyWorId(string strGbFtp, string strFilePath, string fstrROWID)
        {
            return etcJupmstRepository.UpdateImageGbJobSogenbyRowId(strGbFtp, strFilePath, fstrROWID);
        }

        public ETC_JUPMST GetRowIdbyPtNoBDateDeptcode(string fstrPano, string fstrBDate, string fstrDeptCode)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDateDeptcode(fstrPano, fstrBDate, fstrDeptCode);
        }

        public string GetRowIdbyPtNoBDateDeptCode(string fstrPtno, string fstrJepDate, string strGubun)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDateDeptCode(fstrPtno, fstrJepDate, strGubun);
        }

        public int UpdateGbJobbyPtNoRDate(string strPtNo, string strRDate, string strEDATE)
        {
            return etcJupmstRepository.UpdateGbJobbyPtNoRDate(strPtNo, strRDate, strEDATE);
        }

        public List<ETC_JUPMST> GetRowIdbyPtNoBDateOnlyDeptCode(string fstrPtno, string fstrJepDate, string strOrderCodeYN, string strGubun, string strDeptCode)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDateOnlyDeptCode(fstrPtno, fstrJepDate, strOrderCodeYN, strGubun, strDeptCode);
        }

        public ETC_JUPMST GetRowIdbyPtNoBDate(string pTNO, string jEPDATE)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDate(pTNO, jEPDATE);
        }

        public ETC_JUPMST GetRowIdbyPtNoBDateOrerCode(string pTNO, string jEPDATE)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDateOrerCode(pTNO, jEPDATE);
        }

        public List<ETC_JUPMST> GetRowIdbyPtNoBDateList(string pTNO, string jEPDATE)
        {
            return etcJupmstRepository.GetRowIdbyPtNoBDateList(pTNO, jEPDATE);
        }

        public int GetCountbyPtNoBDate(string fstrPano, string fstrJepDate, string strGubun)
        {
            return etcJupmstRepository.GetCountbyPtNoBDate(fstrPano, fstrJepDate, strGubun);
        }
    }
}
