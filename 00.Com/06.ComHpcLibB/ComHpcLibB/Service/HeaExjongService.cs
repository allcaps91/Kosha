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
    public class HeaExjongService
    {   
        private HeaExjongRepository heaExjongRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaExjongService()
        {
			this.heaExjongRepository = new HeaExjongRepository();
        }

        public List<HEA_EXJONG> Read_Hea_ExJong()
        {
            return heaExjongRepository.Read_Hea_ExJong();
        }

        public string Read_ExJong_Name(string strCode)
        {
            return heaExjongRepository.Read_ExJong_Name(strCode);
        }

        public HEA_EXJONG Read_ExJong_CodeName(string v)
        {
            return heaExjongRepository.Read_ExJong_CodeName(v);
        }

        public List<HEA_EXJONG> FindAll()
        {
            return heaExjongRepository.FindAll();
        }

        public int Insert(HEA_EXJONG item)
        {
            return heaExjongRepository.Insert(item);
        }

        public bool DataCheck(HEA_EXJONG item)
        {
            if (item.CODE.Length != 2)
            {
                MessageBox.Show("검진코드를 반드시 2자리 숫자로 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public int Update(HEA_EXJONG item)
        {
            return heaExjongRepository.UpDate(item);
        }

        public int Delete(HEA_EXJONG item)
        {
            return heaExjongRepository.Delete(item);
        }

        public string GetBuRateByGjJong(string argJong)
        {
            return heaExjongRepository.GetBuRateByGjJong(argJong);
        }

        public string Read_Hea_ExJong_Name(string strCode)
        {
            return heaExjongRepository.Read_Hea_ExJong_Name(strCode);
        }
    }
}
