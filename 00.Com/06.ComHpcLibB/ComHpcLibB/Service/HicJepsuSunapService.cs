namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
    using System.Windows.Forms;

    public class HicJepsuSunapService 
    {
        private HicJepsuSunapRepository hicJepsuSunapRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicJepsuSunapService()
        {
            this.hicJepsuSunapRepository = new HicJepsuSunapRepository();
        }

        public List<HIC_JEPSU_SUNAP> GetItembyPaNo(long nPano)
        {
            return hicJepsuSunapRepository.GetItembyPaNo(nPano);
        }

        public List<HIC_JEPSU_SUNAP> GetListAll(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero, string strSunap)
        {
            return hicJepsuSunapRepository.GetListAll(fstrFDate, fstrTDate, fstrJong, fstrJob, fnLtdCode, fstrSName, fbZero, strSunap);
        }

        public List<HIC_JEPSU_SUNAP> GetListSum(string fstrFDate, string fstrTDate, string fstrJong, string fstrJob, long fnLtdCode, string fstrSName, bool fbZero, string strSunap)
        {
            return hicJepsuSunapRepository.GetListSum(fstrFDate, fstrTDate, fstrJong, fstrJob, fnLtdCode, fstrSName, fbZero, strSunap);
        }

        public List<HIC_JEPSU_SUNAP> GetCodebyPtNo(string argPtNo)
        {
            return hicJepsuSunapRepository.GetCodebyPtNo(argPtNo);
        }

        public List<HIC_JEPSU_SUNAP> GetWrtNoCodebyPtNo(string argPtNo)
        {
            return hicJepsuSunapRepository.GetWrtNoCodebyPtNo(argPtNo);
        }

        public List<HIC_JEPSU_SUNAP> GetUnionItembyPaNo(long nPano)
        {
            return hicJepsuSunapRepository.GetUnionItembyPaNo(nPano);
        }

        public int GetCountWrtNo(long nWRTNO)
        {
            return hicJepsuSunapRepository.GetCountWrtNo(nWRTNO);
        }

        public HIC_JEPSU_SUNAP GetItembyJepDateWrtNo(long argWRTNO, string strJepDate)
        {
            return hicJepsuSunapRepository.GetItembyJepDateWrtNo(argWRTNO, strJepDate);
        }

        public List<HIC_JEPSU_SUNAP> GetJohapAmtbyJepDAteMirNo1(string fRDATE, long argMirno)
        {
            return hicJepsuSunapRepository.GetJohapAmtbyJepDAteMirNo1(fRDATE, argMirno);
        }

        public HIC_JEPSU_SUNAP GetItembyWrtNoCode(long nWRTNO, string strCode)
        {
            return hicJepsuSunapRepository.GetItembyWrtNoCode(nWRTNO, strCode);
        }

        public int GetCountbyWrtNoCode(long nWRTNO, string strCode)
        {
            return hicJepsuSunapRepository.GetCountbyWrtNoCode(nWRTNO, strCode);
        }

        public List<HIC_JEPSU_SUNAP> GetItembySuDateLtdCodeMirNo(string strFDate, string strTDate, long nLtdCode, string strJong, string strJonggum, long nMirNo, string strSunap, string strBo)
        {
            return hicJepsuSunapRepository.GetItembySuDateLtdCodeMirNo(strFDate, strTDate, nLtdCode, strJong, strJonggum, nMirNo, strSunap, strBo);
        }

        public List<HIC_JEPSU_SUNAP> GetItembySuDateMirNo(string strFDate, string strTDate, string strJong, string strJonggum, long nMirNo, string strSunap, string strBo)
        {
            return hicJepsuSunapRepository.GetItembySuDateMirNo(strFDate, strTDate, strJong, strJonggum, nMirNo, strSunap, strBo);
        }

        public HIC_JEPSU_SUNAP GetSumJohapAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            return hicJepsuSunapRepository.GetSumJohapAmtMirNo1byWrtNo2(wRTNO2, strGjJong, strGubun);
        }

        public HIC_JEPSU_SUNAP GetSumJohapAmtMirNo3byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            return hicJepsuSunapRepository.GetSumJohapAmtMirNo3byWrtNo2(wRTNO2, strGjJong, strGubun);
        }

        public HIC_JEPSU_SUNAP GetBoninAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            return hicJepsuSunapRepository.GetBoninAmtMirNo1byWrtNo2(wRTNO2, strGjJong, strGubun);
        }

        public HIC_JEPSU_SUNAP GetJohapAmtMirNo1byWrtNo2(long wRTNO2, string strGbJong, string strGubun)
        {
            return hicJepsuSunapRepository.GetJohapAmtMirNo1byWrtNo2(wRTNO2, strGbJong, strGubun);
        }

        public HIC_JEPSU_SUNAP GetLtdAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            return hicJepsuSunapRepository.GetLtdAmtMirNo1byWrtNo2(wRTNO2, strGjJong, strGubun);
        }

        public HIC_JEPSU_SUNAP GetSumTotAmtMirNo1byWrtNo2(long wRTNO2, string strGjJong, string strGubun)
        {
            return hicJepsuSunapRepository.GetSumTotAmtMirNo1byWrtNo2(wRTNO2, strGjJong, strGubun);
        }

        public List<HIC_JEPSU_SUNAP> GetInWonMoney(bool ChkBogen, bool ChkW_Am, string TxtBogun, string TxtKiho, string TxtLtdCode1, bool ChkGub, string dtpFDate, string dtpTDate)
        {
            return hicJepsuSunapRepository.GetInWonMoney(ChkBogen, ChkW_Am, TxtBogun, TxtKiho, TxtLtdCode1, ChkGub, dtpFDate, dtpTDate);
        }

        public List<HIC_JEPSU_SUNAP> GetInWonCash(string fstrFDate, string fstrTDate, List<string> WRTNO, long nMisuNo)
        {
            return hicJepsuSunapRepository.GetInWonCash(fstrFDate, fstrTDate, WRTNO, nMisuNo);
        }

        public List<HIC_JEPSU_SUNAP> GetGongDanItem(List<string> jongSQL, string FstrCOLNM, string fstrJongSQL, string dtpFDate, string dtpTDate, string fstrJong, string cboJong, string TxtBogun, string TxtKiho, bool ChkW_Am, string cboGbn, string cboGbnLen, string cboDent, bool rdoChasu2, bool rdoChasu3)
        {
            return hicJepsuSunapRepository.GetGongDanItem(jongSQL, FstrCOLNM, fstrJongSQL, dtpFDate, dtpTDate, fstrJong, cboJong, TxtBogun, TxtKiho, ChkW_Am, cboGbn, cboGbnLen, cboDent, rdoChasu2, rdoChasu3);
        }
    }
}
