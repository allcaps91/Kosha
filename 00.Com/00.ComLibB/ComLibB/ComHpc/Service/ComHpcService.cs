namespace ComLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComLibB.Dto;
    using ComLibB.Repository;

    public class ComHpcService
    {
        private ComHpcRepository comHpcRepository;

        public ComHpcService()
        {
            this.comHpcRepository = new ComHpcRepository();
        }

        public COMHPC GetItembyWrtNo(long nWRTNO)
        {
            return comHpcRepository.GetItembyWrtNo(nWRTNO);
        }

        public List<COMHPC> GetItembyPaNoSDate(long nPano, string strSDate, string strGbSts = "")
        {
            return comHpcRepository.GetItembyPaNoSDate(nPano, strSDate, strGbSts);
        }

        public List<COMHPC> GetItemHeaNoActingbyWrtNo(long fnWRTNO)
        {
            return comHpcRepository.GetItemHeaNoActingbyWrtNo(fnWRTNO);
        }
        public List<COMHPC> GetItembyWrtNoResultHea(long nWRTNO)
        {
            return comHpcRepository.GetItembyWrtNoResultHea(nWRTNO);
        }
        public COMHPC FindOne(string v)
        {
            return comHpcRepository.FindOne(v);
        }

        public string Read_Hea_ExJong_Name(string strCode)
        {
            return comHpcRepository.Read_Hea_ExJong_Name(strCode);
        }

        public string Read_Hic_ResCodeName(string strGubun, string strCode)
        {
            return comHpcRepository.Read_Hic_ResCodeName(strGubun, strCode);
        }

        public COMHPC GetItemHicHeabyWrtNo(string fstrGubun, long fnWrtNo)
        {
            return comHpcRepository.GetItemHicHeabyWrtNo(fstrGubun, fnWrtNo);
        }

        public List<COMHPC> GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(string fstrGubun, long fnWrtNo)
        {
            return comHpcRepository.GetItemHicHeabyWrtNoOrderbyPanjengPartExCode(fstrGubun, fnWrtNo);
        }

        public string Read_ExJong_Name(string strCode)
        {
            return comHpcRepository.Read_ExJong_Name(strCode);
        }

        public string Read_Hic_ExJong_Name(string strCode)
        {
            return comHpcRepository.Read_Hic_ExJong_Name(strCode);
        }
        public List<COMHPC> GetHeaJepsuitembyPtno(string argPtno)
        {
            return comHpcRepository.GetHeaJepsuitembyPtno(argPtno);
        }

        public List<COMHPC> GetHicJepsuitembyPtno(string argPtno)
        {
            return comHpcRepository.GetHicJepsuitembyPtno(argPtno);
        }

    }
}
