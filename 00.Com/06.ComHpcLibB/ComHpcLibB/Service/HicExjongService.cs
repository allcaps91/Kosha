namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class HicExjongService
    {

        private HicExjongRepository hicExjongRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicExjongService()
        {
            this.hicExjongRepository = new HicExjongRepository();
        }

        public List<HIC_EXJONG> FindAll()
        {
            return hicExjongRepository.FindAll();
        }

        public int Insert(HIC_EXJONG item)
        {
            return hicExjongRepository.Insert(item);
        }

        public List<HIC_EXJONG> GetItemListByBun(string v)
        {
            return hicExjongRepository.GetItemListByBun(v);
        }

        public int Update(HIC_EXJONG item)
        {
            return hicExjongRepository.UpDate(item);
        }

        public int Delete(HIC_EXJONG item)
        {
            return hicExjongRepository.Delete(item);
        }

        public bool DataCheck(HIC_EXJONG item)
        {
            if (item.CODE.Length != 2)
            {
                MessageBox.Show("검진코드를 반드시 2자리 숫자로 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (item.CHASU == null || item.CHASU == "")
            {
                MessageBox.Show("건진차수를 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public List<HIC_EXJONG> Read_ExJong_Add(bool chk)
        {
            return hicExjongRepository.Read_ExJong_Add(chk);
        }

        public string Read_Hic_ExJong_Name(string strCode)
        {
            return hicExjongRepository.Read_Hic_ExJong_Name(strCode);
        }

        public HIC_EXJONG Read_ExJong_CodeName(string v)
        {
            return hicExjongRepository.Read_ExJong_CodeName(v);
        }

        public string Read_GbSangdam(string argJong)
        {
            return hicExjongRepository.Read_GbSangdam(argJong);
        }

        public string GetGbMunjin(string argJONG)
        {
            return hicExjongRepository.GetGbMunjin(argJONG);
        }

        public int GetCount(string argGjJong)
        {
            return hicExjongRepository.GetCount(argGjJong);
        }

        public List<HIC_EXJONG> GetCodeName(List<string> strBun, string strChasu)
        {
            return hicExjongRepository.GetCodeName(strBun, strChasu);
        }

        public string GetGbPrt5byCode(string argJong)
        {
            return hicExjongRepository.GetGbPrt5byCode(argJong);
        }

        public string GetGbSangdambyCode(string argJong)
        {
            return hicExjongRepository.GetGbSangdambyCode(argJong);
        }

        public string GetChasubyCode(string strJong)
        {
            return hicExjongRepository.GetChasubyCode(strJong);
        }

        public string GetBunByGjJong(string grgJONG)
        {
            return hicExjongRepository.GetBunByGjJong(grgJONG);
        }

        public HIC_EXJONG GetItembyCode(string strJong)
        {
            return hicExjongRepository.GetItembyCode(strJong);
        }

        public List<HIC_EXJONG> GetItemList()
        {
            return hicExjongRepository.GetItemList();
        }

        public string GetBuRateByGjJong(string argJong)
        {
            return hicExjongRepository.GetBuRateByGjJong(argJong);
        }

        public HIC_EXJONG GetNamebyCode(string argGjJong)
        {
            return hicExjongRepository.GetNamebyCode(argGjJong);
        }

        public string GetGbMunjinbyCode(string argJong)
        {
            return hicExjongRepository.GetGbMunjinbyCode(argJong);
        }

        public string GetGbNhicByExJong(string argJong)
        {
            return hicExjongRepository.GetGbNhicByExJong(argJong);
        }

        public List<HIC_EXJONG> GetListByCodeName(string strName)
        {
            return hicExjongRepository.GetListByCodeName(strName);
        }

        public List<HIC_EXJONG> GetCode(string fstrJong, string [] jong_1, string [] jong_2, string [] jong_3, string [] jong_4)
        {
            return hicExjongRepository.GetCode(fstrJong, jong_1, jong_2, jong_3, jong_4);
        }
    }
}
