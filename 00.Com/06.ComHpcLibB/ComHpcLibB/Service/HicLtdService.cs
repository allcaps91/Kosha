namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class HicLtdService
    {

        private HicLtdRepository hicLtdRepository;
        private HcOshaSiteRepository hcOshaSiteRepository;
        /// <summary>
        /// 
        /// </summary>
        public HicLtdService()
        {
            this.hicLtdRepository = new HicLtdRepository();
            hcOshaSiteRepository = new HcOshaSiteRepository();
        }

        public List<HIC_LTD> ViewLtd(string keyWords, string searchOption = "")
        {
            return hicLtdRepository.ViewLtd(keyWords, searchOption);
        }

        public HIC_LTD FindOne(string v)
        {
            return hicLtdRepository.FindOne(v);
        }

        public int Delete(HIC_LTD item)
        {
            return hicLtdRepository.Delete(item);
        }

        public Dictionary<string, object> SelHicCount(long cODE)
        {
            return hicLtdRepository.SelHicCount(cODE);
        }

        public Dictionary<string, object> SelHeaCount(long cODE)
        {
            return hicLtdRepository.SelHeaCount(cODE);
        }

        public Dictionary<string, object> SelMisuCount(long cODE)
        {
            return hicLtdRepository.SelMisuCount(cODE);
        }

        public int Delete_Tax_All(long argCode)
        {
            return hicLtdRepository.Delete_Tax_All(argCode);
        }

        public long GetHicCount(long cODE)
        {
            return hicLtdRepository.GetHicCount(cODE);
        }

        public long GetHeaCount(long cODE)
        {
            return hicLtdRepository.GetHeaCount(cODE);
        }

        public long GetMisuCount(long cODE)
        {
            return hicLtdRepository.GetMisuCount(cODE);
        }

        public int Insert(HIC_LTD item)
        {
            return hicLtdRepository.Insert(item);
        }

        public int Update(HIC_LTD item)
        {
            return hicLtdRepository.Update(item);
        }

        public HIC_LTD GetCountLtdCode(long strLtdCode)
        {
            return hicLtdRepository.GetCountLtdCode(strLtdCode);
        }

        public string GetSpcRemarkByCode(long strLtdCode)
        {
            return hicLtdRepository.GetSpcRemarkByCode(strLtdCode);
        }

        public List<HIC_LTD> GetItemData(bool rdoSort1, bool rdoSort3, string TxtViewCode)
        {
            return hicLtdRepository.GetItemData(rdoSort1, rdoSort3, TxtViewCode);
        }

        public bool DataCheck(HIC_LTD item)
        {
            if (item.CODE == 0)
            {
                MessageBox.Show("사업장코드를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if ((item.JUMIN != null && item.JUMIN != "") && (item.JUMIN.Length != 14 || VB.Mid(item.JUMIN, 7, 1) != "-"))
            {
                MessageBox.Show("주민등록번호를 YYMMDD-1234567 형태로 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (VB.Left(item.KIHO, 5) == "5" && (item.ARMY_HSP == null || item.ARMY_HSP == ""))
            {
                MessageBox.Show("군병원을 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (item.MAILCODE == null || item.MAILCODE.Length != 5)
            {
                MessageBox.Show("우편번호는 5자리만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //전자세금계산서 체크

            return true;
        }

        public List<HIC_LTD> GetAct(string gstrRetValue)
        {
            return hicLtdRepository.GetAct(gstrRetValue);
        }

        public List<HIC_LTD> GetListGaResv()
        {
            return hicLtdRepository.GetListGaResv();
        }

        public List<HIC_LTD> Read_Hic_Ltd(string strName)
        {
            return hicLtdRepository.Read_Hic_Ltd(strName);
        }

        public string READ_Ltd_One_Name(string strCode)
        {
            return hicLtdRepository.READ_Ltd_One_Name(strCode);
        }

        public int Delete_Tax_One(string rOWID)
        {
            throw new NotImplementedException();
        }

        public string GetHaRemarkbyLtdCode(long v)
        {
            return hicLtdRepository.GetHaRemarkbyLtdCode(v);
        }
      
        public string GetRemarkbyLtdCode(long v)
        {
            return hicLtdRepository.GetRemarkbyLtdCode(v);
        }
        public int UpdateGaResv(long argLtdCode)
        {
            return hicLtdRepository.UpdateGaResv(argLtdCode);
        }

        public HIC_LTD GetItembyCode(string strCode)
        {
            return hicLtdRepository.GetItembyCode(strCode);
        }

        public string GetChulNotSayuByLtdCode(long nLtdCode)
        {
            return hicLtdRepository.GetChulNotSayuByLtdCode(nLtdCode);
        }

        public List<HIC_LTD> ViewChukLtd(string argFindKey, string strGubun)
        {
            return hicLtdRepository.ViewChukLtd(argFindKey, strGubun);
        }

        public HIC_LTD GetAllbyCode(string fstrLtdCode)
        {
            return hicLtdRepository.GetAllbyCode(fstrLtdCode);
        }

        public int UpdateSpcRemarkByLtdCode(string strRemark, long argLtdCode)
        {
            return hicLtdRepository.UpdateSpcRemarkByLtdCode(strRemark, argLtdCode);
        }

        public HIC_LTD GetIetmbyCode(long fstrLtdCode)
        {
            return hicLtdRepository.GetIetmbyCode(fstrLtdCode);
        }

        public List<HIC_LTD> GetBusiness(double TxtLtdCode)
        {
            return hicLtdRepository.GetBusiness(TxtLtdCode);
        }

        public List<HIC_LTD> GetCodeName(string strBogenso)
        {
            return hicLtdRepository.GetCodeName(strBogenso);
        }

        public string GetHTelByLtdCode(long nLtdCode)
        {
            return hicLtdRepository.GetHTelByLtdCode(nLtdCode);
        }

        /// <summary>
        /// 보건관리 사업장 등록
        /// </summary>
        /// <param name="view"></param>
        public bool SaveOsha(long ltdCode)
        {
            bool result = false;
            HC_OSHA_SITE site = hcOshaSiteRepository.FindById(ltdCode);
            if (site == null)
            {
                HC_SITE_VIEW view = new HC_SITE_VIEW();
                view.ID = ltdCode;
                hcOshaSiteRepository.Insert(view);
                result = true;
            }
            else
            {
                hcOshaSiteRepository.ActiveOsha(site.ID);
            }
            return result;
        }

        public int GetMisuTaxUpdate(MISU_TAX item)
        {
            return hcOshaSiteRepository.GetMisuTaxUpdate(item);
        }

        public void InactiveOsha(long ltdCode)
        {
            hcOshaSiteRepository.InactiveOsha(ltdCode);
        }

        public HIC_LTD GetMailCodebyCode(string strLtdCode)
        {
            return hicLtdRepository.GetMailCodebyCode(strLtdCode);
        }

        public string GetFaxNoByLtdCode(long nLtdCode)
        {
            return hicLtdRepository.GetFaxNoByLtdCode(nLtdCode);
        }

        public string GetNamebyCode(string argCode)
        {
            return hicLtdRepository.GetNamebyCode(argCode);
        }

        public string GetGbResvByCode(long nLtdCode)
        {
            return hicLtdRepository.GetGbResvByCode(nLtdCode);
        }

        public string GetJusoByCode(long nLtdCode)
        {
            return hicLtdRepository.GetJusoByCode(nLtdCode);
        }

        public List<HIC_LTD> GetBusinessItem(string TxtLtdCode)
        {
            return hicLtdRepository.GetBusinessItem(TxtLtdCode);
        }
        
    }
}
