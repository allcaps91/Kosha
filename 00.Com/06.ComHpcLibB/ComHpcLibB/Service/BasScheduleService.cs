namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class BasScheduleService
    {
        
        private BasScheduleRepository basScheduleRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public BasScheduleService()
        {
			this.basScheduleRepository = new BasScheduleRepository();
        }

        public BAS_SCHEDULE Read_Schedule(string strDate, string strDrCode)
        {
            return basScheduleRepository.Read_Schedule(strDate, strDrCode);
        }

        public BAS_SCHEDULE GetGbJinbyDrCode(string dRCODE)
        {
            return basScheduleRepository.GetGbJinbyDrCode(dRCODE);
        }

        public BAS_SCHEDULE GetGbJinGbJin2bySchDateDrCode(string argDate, string strDrCode)
        {
            return basScheduleRepository.GetGbJinGbJin2bySchDateDrCode(argDate, strDrCode);
        }

        public List<BAS_SCHEDULE> GetItembySchDate(string fstrJepDate)
        {
            return basScheduleRepository.GetItembySchDate(fstrJepDate);
        }

        public List<BAS_SCHEDULE> GetItembySchDateDrcode(string strFDate, string strTDate, string strDrcode)
        {
            return basScheduleRepository.GetItembySchDateDrcode(strFDate, strTDate, strDrcode);
        }

        public List<BAS_SCHEDULE> GetItembySchDateDrcodes(string strFDate, string strTDate, List<string> strDrcodes)
        {
            return basScheduleRepository.GetItembySchDateDrcodes(strFDate, strTDate, strDrcodes);
        }

        public int Update(string strDayGbn, string strGbn, string strGbn2, string strGbn3, string strROWID)
        {
            return basScheduleRepository.Update(strDayGbn, strGbn, strGbn2, strGbn3, strROWID);
        }

        public int Insert(string strDrCode, string strDate, string strDay, string strGbn, string strGbn2, string strGbn3)
        {
            return basScheduleRepository.Insert(strDrCode, strDate, strDay, strGbn, strGbn2, strGbn3);
        }

        public int Delete (string strDrCode, string strFDATE, string strTDATE)
        {
            return basScheduleRepository.Delete(strDrCode, strFDATE , strTDATE);
        }

        
        public int Read_Schedule_CNT(string strFDATE, string strTDATE)
        {
            return basScheduleRepository.Read_Schedule_CNT(strFDATE, strTDATE);
        }

    }
}
