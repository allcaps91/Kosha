namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using System.Windows.Forms;


    /// <summary>
    /// 
    /// </summary>
    public class XrayDetailService
    {
        
        private XrayDetailRepository xrayDetailRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public XrayDetailService()
        {
			this.xrayDetailRepository = new XrayDetailRepository();
        }

        public void InsertData(XRAY_DETAIL dXRay)
        {
            xrayDetailRepository.InsertData(dXRay);
        }

        public void UpDateData(XRAY_DETAIL dXRay)
        {
            xrayDetailRepository.UpDateData(dXRay);
        }

        public string GetXrayNoByPanoSeekDateXCode(string argPano, string argDate, string argXCode, string argDept)
        {
            return xrayDetailRepository.GetXrayNoByPanoSeekDateXCode(argPano, argDate, argXCode, argDept);
        }

        public int GetCountbyPtNo(string strPtNo, string jEPDATE)
        {
            return xrayDetailRepository.GetCountbyPtNo(strPtNo, jEPDATE);
        }

        public string GetRowidByPanoXCodeBDateDept(string argPano, string argXCode, string argBDate, string argDept)
        {
            return xrayDetailRepository.GetRowidByPanoXCodeBDateDept(argPano, argXCode, argBDate, argDept);
        }

        public int UpDate_XrayDetail_Del(string argRowid)
        {
            return xrayDetailRepository.UpDate_XrayDetail_Del(argRowid);
        }

        public int UpDate_GBRESERVED(string argRowid, string argGbReserved)
        {
            return xrayDetailRepository.UpDate_GBRESERVED(argRowid, argGbReserved);
        }
        public bool InsertHis(string argRowid)
        {
            try
            {
                xrayDetailRepository.InsertHis(argRowid);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        
    }
}
