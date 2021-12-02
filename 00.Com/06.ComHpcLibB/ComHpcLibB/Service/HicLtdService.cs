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
                MessageBox.Show("������ڵ带 �Է��ϼ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if ((item.JUMIN != null && item.JUMIN != "") && (item.JUMIN.Length != 14 || VB.Mid(item.JUMIN, 7, 1) != "-"))
            {
                MessageBox.Show("�ֹε�Ϲ�ȣ�� YYMMDD-1234567 ���·� �Է��ϼ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (VB.Left(item.KIHO, 5) == "5" && (item.ARMY_HSP == null || item.ARMY_HSP == ""))
            {
                MessageBox.Show("�������� �����ϼ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (item.MAILCODE == null || item.MAILCODE.Length != 5)
            {
                MessageBox.Show("�����ȣ�� 5�ڸ��� �����մϴ�.", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //���ڼ��ݰ�꼭 üũ

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
        /// ���ǰ��� ����� ���
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
