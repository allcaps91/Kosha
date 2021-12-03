namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB.Model;
    using ComBase.Controls;


    /// <summary>
    /// 
    /// </summary>
    public class HicSunapService
    {

        private HicSunapRepository hicSunapRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicSunapService()
        {
            this.hicSunapRepository = new HicSunapRepository();
        }

        public HIC_SUNAP Read_Hic_Sunap(long argWrtNo)
        {
            return hicSunapRepository.Read_Hic_Sunap(argWrtNo);
        }

        public long GetSunapAmtbyWrtNo(long argWRTNO)
        {
            return hicSunapRepository.GetSunapAmtbyWrtNo(argWRTNO);
        }

        public IList<HIC_SUNAP> GetlistsByGWRTNO(long nGWRTNO)
        {
            return hicSunapRepository.GetlistsByGWRTNO(nGWRTNO);
        }

        public long GetMaxSeqbyWrtNo(long nWrtNo)
        {
            return hicSunapRepository.GetMaxSeqbyWrtNo(nWrtNo);
        }

        public long GetHeaMaxSeqbyWrtNo(long nWrtNo)
        {
            return hicSunapRepository.GetHeaMaxSeqbyWrtNo(nWrtNo);
        }

        public int InsertSelectbyWrtNOSeqNo(long nWrtNo, long nSeq, long nJobSabun, long fnWrtNo, long nOLD_Seq, string strDate)
        {
            return hicSunapRepository.InsertSelectbyWrtNOSeqNo(nWrtNo, nSeq, nJobSabun, fnWrtNo, nOLD_Seq, strDate);
        }

        public HIC_SUNAP GetHicSunapAmtByWRTNO(long nWRTNO)
        {
            return hicSunapRepository.GetHicSunapAmtByWRTNO(nWRTNO);
        }

        public HIC_SUNAP GetHeaSunapAmtByWRTNO(long nWRTNO)
        {
            return hicSunapRepository.GetHeaSunapAmtByWRTNO(nWRTNO);
        }

        public HIC_SUNAP GetItemByWrtno(long nWRTNO)
        {
            return hicSunapRepository.GetItemByWrtno(nWRTNO);
        }

        public HIC_SUNAP GetSumAmtByGroupCode(List<READ_SUNAP_ITEM> suInfo, string argBuRate, long argHalinAmt, string strJong, string strHalinCode)
        {
            HIC_SUNAP item = new HIC_SUNAP();
            int nSelf = 0;
            int nJoRate = 0;
            int nLtdRate = 0;
            int nBogenRate = 0;
            int nBonRate = 0;
            long nAmt = 0, nAmAmt1 = 0, nAmAmt2 = 0;
            long nTotAmt = 0;
            long nJohapAmt = 0;
            long nLtdAmt = 0;
            long nBogenAmt = 0;
            long nBoninAmt = 0;

            for (int i = 0; i < suInfo.Count; i++)
            {
                //체크 표시 제외
                if (suInfo[i].RowStatus != ComBase.Mvc.RowStatus.Delete)
                {
                    nSelf = suInfo[i].GBSELF.To<int>();
                    if (nSelf == 0)
                    {
                        nSelf = argBuRate.To<int>();
                        //suInfo[i].GBSELF = argBuRate;
                    }

                    //TODO : 외래본인부담율처럼 테이블로 관리할것
                    //부담율별 요율 설정
                    switch (nSelf)
                    {
                        case 1: nJoRate = 100; nLtdRate = 0; nBogenRate = 0; nBonRate = 0; break; //조합100%  
                        case 2: nJoRate = 0; nLtdRate = 100; nBogenRate = 0; nBonRate = 0; break; //회사100%  
                        case 3: nJoRate = 0; nLtdRate = 0; nBogenRate = 0; nBonRate = 100; break; //본인100%  
                        case 4: nJoRate = 50; nLtdRate = 0; nBogenRate = 0; nBonRate = 50; break; //조합,본인50%
                        case 5: nJoRate = 50; nLtdRate = 50; nBogenRate = 0; nBonRate = 0; break; //조합,회사50%
                        case 6: nJoRate = 0; nLtdRate = 50; nBogenRate = 0; nBonRate = 50; break; //회사,본인50%
                        case 7: nJoRate = 80; nLtdRate = 0; nBogenRate = 0; nBonRate = 20; break; //조합80%,본인20%
                        case 8: nJoRate = 80; nLtdRate = 20; nBogenRate = 0; nBonRate = 0; break; //조합80%,회사20%
                        case 9: nJoRate = 90; nLtdRate = 0; nBogenRate = 0; nBonRate = 10; break; //조합90%,본인10%
                        case 10: nJoRate = 90; nLtdRate = 10; nBogenRate = 0; nBonRate = 0; break; //조합90%,회사10%
                        case 11: nJoRate = 0; nLtdRate = 0; nBogenRate = 100; nBonRate = 0; break; //보건소100%
                        case 12: nJoRate = 90; nLtdRate = 0; nBogenRate = 10; nBonRate = 0; break; //조합90%,보건소10%
                        default: nJoRate = 0; nLtdRate = 0; nBogenRate = 0; nBonRate = 100; break; //기타 본인 100%
                    }

                    nAmt = suInfo[i].AMT;

                    //금액을 계산 암검진은 부담률 적용
                    //조합 90%계산
                    if ((strJong == "31" || strJong == "35") && nJoRate == 90)
                    {
                        nAmAmt1 = 0; nAmAmt2 = 0;
                        nAmAmt1 = (long)Math.Round(nAmt * nJoRate / 100.0 / 10, MidpointRounding.AwayFromZero) * 10;
                        nAmAmt2 = nAmt - nAmAmt1;
                        nTotAmt = nTotAmt + nAmt;
                        if (nJoRate != 0) { nJohapAmt = nJohapAmt + nAmAmt1; }
                        if (nLtdRate != 0) { nLtdAmt = nLtdAmt + nAmAmt2; }
                        if (nBonRate != 0) { nBoninAmt = nBoninAmt + nAmAmt2; }
                        if (nBogenRate != 0) { nBogenAmt = nBogenAmt + nAmAmt2; }
                    }
                    else if (nJoRate == 0 && nLtdRate > 0 && nBonRate > 0)
                    {
                        nAmAmt1 = 0; nAmAmt2 = 0;
                        nAmAmt1 = (long)Math.Round(nAmt * nLtdRate / 100.0 / 10, MidpointRounding.AwayFromZero) * 10;
                        nAmAmt2 = nAmt - nAmAmt1;
                        nTotAmt = nTotAmt + nAmt;

                        if (nLtdRate != 0) { nLtdAmt = nLtdAmt + nAmAmt2; }
                        if (nBonRate != 0) { nBoninAmt = nBoninAmt + nAmAmt1; }
                    }
                    else
                    {
                        nTotAmt = nTotAmt + nAmt;
                        if (nJoRate != 0) { nJohapAmt = nJohapAmt + (long)Math.Truncate(nAmt * nJoRate / 100.0); }
                        if (nLtdRate != 0) { nLtdAmt = nLtdAmt + (long)Math.Truncate(nAmt * nLtdRate / 100.0); }
                        if (nBonRate != 0) { nBoninAmt = nBoninAmt + (long)Math.Truncate(nAmt * nBonRate / 100.0); }
                        if (nBogenRate != 0) { nBogenAmt = nBogenAmt + (long)Math.Truncate(nAmt * nBogenRate / 100.0); }
                    }
                }
            }

            //2020-08-31 추가(감액코드 03,04 추가)
            if (nLtdAmt > 0 && argHalinAmt > 0 && strHalinCode == "01")
            {
                nLtdAmt = nLtdAmt - argHalinAmt;
            }
            else if (nBoninAmt > 0 && argHalinAmt > 0 && strHalinCode == "03")
            {
                nBoninAmt = nBoninAmt - argHalinAmt;
            }
            else if (nLtdAmt > 0 && argHalinAmt > 0 && strHalinCode == "04")
            {
                nLtdAmt = nLtdAmt - argHalinAmt;
            }
            else
            {
                if (argHalinAmt > 0) { nBoninAmt -= argHalinAmt; }
                if (nBoninAmt < 0)
                {
                    nBoninAmt = 0;
                    argHalinAmt = 0;
                }
            }

            item.TOTAMT = nTotAmt;
            item.JOHAPAMT = nJohapAmt;
            item.LTDAMT = nLtdAmt;
            item.BOGENAMT = nBogenAmt;
            item.BONINAMT = nBoninAmt;
            item.HALINAMT = argHalinAmt;

            return item;
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicSunapRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public void InsertMinusSunapData(HIC_SUNAP item)
        {
            hicSunapRepository.InsertMinusSunapData(item);
        }

        public void InsertMinusSunapData_Hea(HIC_SUNAP item)
        {
            hicSunapRepository.InsertMinusSunapData_Hea(item);
        }

        public void Insert(HIC_SUNAP hSP)
        {
            hicSunapRepository.Insert(hSP);
        }

        public string GetHalinGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            return hicSunapRepository.GetHalinGyeByWrtnoSeqNo(nWRTNO, nSeqNo);
        }

        public string GetMisuGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            return hicSunapRepository.GetMisuGyeByWrtnoSeqNo(nWRTNO, nSeqNo);
        }

        public bool InsertSelectBySuDatePano(long wRTNO, string jEPDATE, long pANO, string argJong = "")
        {
            try
            {
                hicSunapRepository.InsertSelectBySuDatePano(wRTNO, jEPDATE, pANO, argJong);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_SUNAP GetHicSunapByWRTNO(long nWRTNO)
        {
            return hicSunapRepository.GetHicSunapByWRTNO(nWRTNO);
        }

        public bool UpdateTotAmtJhpAmtbyRowid(HIC_SUNAP item)
        {
            try
            {
                hicSunapRepository.UpdateTotAmtJhpAmtbyRowid(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_SUNAP GetSumHeaAmtByGroupCode(List<READ_SUNAP_ITEM> suInfo, string argBuRate, long argLtdCode, long argHalinAmt, string strJong, string strHalinCode)
        {
            HIC_SUNAP item = new HIC_SUNAP();
            int nSelf = 0;
            int nLtdRate = 0;
            int nBonRate = 0;
            long nAmt = 0;
            long nTotAmt = 0;
            long nLtdAmt = 0;
            long nBoninAmt = 0;
            long nHalinAmt = argHalinAmt;

            for (int i = 0; i < suInfo.Count; i++)
            {
                //체크 표시 제외
                if (suInfo[i].RowStatus != ComBase.Mvc.RowStatus.Delete)
                {
                    nSelf = suInfo[i].GBSELF.To<int>();
                    if (nSelf == 0) { nSelf = argBuRate.To<int>(); }

                    //부담율별 요율 설정
                    switch (nSelf)
                    {
                        case 1: nLtdRate = 0; nBonRate = 100; break;  //본인100%  
                        case 2: nLtdRate = 100; nBonRate = 0; break;  //회사100%  
                        case 3: nLtdRate = 50; nBonRate = 50; break;  //회사50%, 본인50%
                        case 4: nLtdRate = 0; nBonRate = 0; break;  //회사, 본인 일부부담
                        default: nLtdRate = 0; nBonRate = 100; break;  //기타 본인 100%
                    }

                    nAmt = suInfo[i].AMT;
                    nTotAmt = nTotAmt + nAmt;

                    if (nLtdRate != 0) { nLtdAmt = nLtdAmt + (long)Math.Truncate(nAmt * nLtdRate / 100.0); }
                    if (nBonRate != 0) { nBoninAmt = nBoninAmt + (long)Math.Truncate(nAmt * nBonRate / 100.0); }
                    
                    //2021-06-14
                    if (nSelf ==4)
                    {
                        nBoninAmt = nBoninAmt + suInfo[i].BONINAMT;
                        nLtdAmt = nLtdAmt + suInfo[i].LTDAMT;
                    }
                    else 
                    {
                        //suInfo[i].LTDAMT = nLtdAmt;
                        //suInfo[i].BONINAMT = nBoninAmt;
                        suInfo[i].LTDAMT = 0;
                        suInfo[i].BONINAMT = 0;
                        if (nLtdRate != 0) { suInfo[i].LTDAMT =  (long)Math.Truncate(nAmt * nLtdRate / 100.0); }
                        if (nBonRate != 0) { suInfo[i].BONINAMT =  (long)Math.Truncate(nAmt * nBonRate / 100.0); }
                    }
                    
                }
            }

            item.TOTAMT = nTotAmt;
            item.LTDAMT = nLtdAmt;
            item.HALINAMT = nHalinAmt;
            item.BONINAMT = (nBoninAmt - nHalinAmt) < 0 ? 0 : (nBoninAmt - nHalinAmt);

            return item;
        }

        public HIC_SUNAP GetJohapAmtBogenAmtbyWrtNo(long wRTNO)
        {
            return hicSunapRepository.GetJohapAmtBogenAmtbyWrtNo(wRTNO);
        }

        public string GetHeaHalinGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            return hicSunapRepository.GetHeaHalinGyeByWrtnoSeqNo(nWRTNO, nSeqNo);
        }

        public string GetHeaMisuGyeByWrtnoSeqNo(long nWRTNO, long nSeqNo)
        {
            return hicSunapRepository.GetHeaMisuGyeByWrtnoSeqNo(nWRTNO, nSeqNo);
        }

        public long GetTotAmtbyWrtNo2(long wRTNO2)
        {
            return hicSunapRepository.GetTotAmtbyWrtNo2(wRTNO2);
        }

        public void InsertHea(HIC_SUNAP hSP)
        {
            hicSunapRepository.InsertHea(hSP);
        }

        public long GetHeaSumMisuAmtByWrtnoSuDate(long fnWRTNO)
        {
            return hicSunapRepository.GetHeaSumMisuAmtByWrtnoSuDate(fnWRTNO);
        }

        public int UpdateMirNobyWrtNo(long nWRTNO, long nMirno, string sGubun)
        {
            return hicSunapRepository.UpdateMirNobyWrtNo(nWRTNO, nMirno, sGubun);
        }

        public long GetBogenAmtbyWrtNo(long nWRTNO)
        {
            return hicSunapRepository.GetBogenAmtbyWrtNo(nWRTNO);
        }

        public long GetJohapAmtbyMirNo(long fnMirNo, string strJong)
        {
            return hicSunapRepository.GetJohapAmtbyMirNo(fnMirNo, strJong);
        }

        public long GetLtdAmtbyMirNo(long fnMirNo)
        {
            return hicSunapRepository.GetLtdAmtbyMirNo(fnMirNo);
        }

        public int UpdateMirNo0byWrtNo(long nWRTNO, string strGubun)
        {
            return hicSunapRepository.UpdateMirNo0byWrtNo(nWRTNO, strGubun);
        }

        public int UpdatebyMirNo1(long nMirno)
        {
            return hicSunapRepository.UpdatebyMirNo1(nMirno);
        }

        public int UpdateMirno2byMirNo2(long nMirno)
        {
            return hicSunapRepository.UpdateMirno2byMirNo2(nMirno);
        }

        public int UpdateMirNo3byMirNo3(long nMirno)
        {
            return hicSunapRepository.UpdateMirNo3byMirNo3(nMirno);
        }

        public int UpdateMirNo5byMirNo5(long nMirno)
        {
            return hicSunapRepository.UpdateMirNo5byMirNo5(nMirno);
        }

        public int UpdateGongDan(long nMisuNo, List<string> WRTNO)
        {
            return hicSunapRepository.UpdateGongDan(nMisuNo, WRTNO);
        }

        public int CompanySuNapUpdate(long nMisuNo, string strFDate, string strTDate, string strltdcode)
        {
            return hicSunapRepository.CompanySuNapUpdate(nMisuNo, strFDate, strTDate, strltdcode);
        }

        public int GuGangGongDan(long nMisuNo, List<string> wrtno, string fstrJong)
        {
            return hicSunapRepository.GuGangGongDan(nMisuNo, wrtno, fstrJong);
        }
    }
}
