namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicPatientService
    {
        
        private HicPatientRepository hicPatientRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicPatientService()
        {
			this.hicPatientRepository = new HicPatientRepository();
        }
        
        public long Read_HicPano()
        {
            return hicPatientRepository.Read_HicPano();
        }
        
        public long Read_HicWrtNo()
        {
            return hicPatientRepository.Read_HicWrtNo();
        }

        public long Read_HeaWrtno()
        {
            return hicPatientRepository.Read_HeaWrtNo();
        }

        public long Read_HicGWrtNo()
        {
            return hicPatientRepository.Read_HicGWrtNo();
        }

        public bool SetHcPatInfoByPano(long nPano)
        {
            bool rtnVal = true;

            clsHcType.HCPAT_CLEAR();

            HIC_PATIENT info = hicPatientRepository.GetHcPatInfoByPano(nPano);

            if (info ==  null)
            {
                return false;
            }

            clsHcType.HPI.PANO        = info.PANO;
            clsHcType.HPI.SNAME       = info.SNAME;
            clsHcType.HPI.JUMIN       = info.JUMIN;
            clsHcType.HPI.SEX         = info.SEX;
            clsHcType.HPI.MAILCODE    = info.MAILCODE;
            clsHcType.HPI.JUSO1       = info.JUSO1;
            clsHcType.HPI.JUSO2       = info.JUSO2;
            clsHcType.HPI.TEL         = info.TEL;
            clsHcType.HPI.LTDCODE     = info.LTDCODE;
            clsHcType.HPI.JIKGBN      = info.JIKGBN;
            clsHcType.HPI.JIKJONG     = info.JIKJONG;
            clsHcType.HPI.SABUN       = info.SABUN;
            clsHcType.HPI.BUSENAME    = info.BUSENAME;
            clsHcType.HPI.GONGJENG    = info.GONGJENG;
            clsHcType.HPI.IPSADATE    = info.IPSADATE;
            clsHcType.HPI.BUSEIPSA    = info.BUSEIPSA;
            clsHcType.HPI.JISA        = info.JISA;
            clsHcType.HPI.GKIHO       = info.GKIHO;
            clsHcType.HPI.GBSUCHEP    = info.GBSUCHEP;
            clsHcType.HPI.PTNO        = info.PTNO;
            clsHcType.HPI.REMARK      = info.REMARK;
            clsHcType.HPI.KIHO        = info.KIHO;
            clsHcType.HPI.BOGUNSO     = info.BOGUNSO;
            clsHcType.HPI.YOUNGUPSO   = info.YOUNGUPSO;
            clsHcType.HPI.LIVER2      = info.LIVER2;
            clsHcType.HPI.GUMDAESANG  = info.GUMDAESANG;
            clsHcType.HPI.EMAIL       = info.EMAIL;
            clsHcType.HPI.HPHONE      = info.HPHONE;
            clsHcType.HPI.GBIEMUNJIN  = info.GBIEMUNJIN;
            clsHcType.HPI.GBSMS       = info.GBSMS;
            clsHcType.HPI.TEL_CONFIRM = info.TEL_CONFIRM;
            clsHcType.HPI.JUMIN2      = clsAES.DeAES(info.JUMIN2);
            clsHcType.HPI.GBPRIVACY   = info.GBPRIVACY;
            clsHcType.HPI.BUILDNO     = info.BUILDNO;
            clsHcType.HPI.LTDCODE2    = info.LTDCODE2;
            clsHcType.HPI.BIRTHDAY    = info.BIRTHDAY;
            clsHcType.HPI.GBBIRTH     = info.GBBIRTH;
            clsHcType.HPI.LTDTEL      = info.LTDTEL;
            clsHcType.HPI.GAMCODE     = info.GAMCODE;
            clsHcType.HPI.RELIGION    = info.RELIGION;
            clsHcType.HPI.STARTDATE   = info.STARTDATE;
            clsHcType.HPI.LASTDATE    = info.LASTDATE;
            clsHcType.HPI.JINCOUNT    = info.JINCOUNT;
            clsHcType.HPI.FAMILLY     = info.FAMILLY;
            clsHcType.HPI.GAMCODE2    = info.GAMCODE2;
            clsHcType.HPI.SOSOK       = info.SOSOK;
            clsHcType.HPI.VIPREMARK   = info.VIPREMARK;
            clsHcType.HPI.GBJIKWON    = info.GBJIKWON;

            return rtnVal;
        }

        public HIC_PATIENT GetPaNobyPtNo(string argPtNo)
        {
            return hicPatientRepository.GetPaNobyPtNo(argPtNo);
        }

        public string GetPrivacyNewByPtno(string fstrPtno)
        {
            return hicPatientRepository.GetPrivacyNewByPtno(fstrPtno);
        }

        public string GetLtdNameByJumin2(string strJumin)
        {
            return hicPatientRepository.GetLtdNameByJumin2(strJumin);
        }

        public List<HIC_PATIENT> GetDoubleChartSearch()
        {
            return hicPatientRepository.GetDoubleChartSearch();
        }

        public int UpdatebySNamePaNo(string strSName, long nPano)
        {
            return hicPatientRepository.UpdatebySNamePaNo(strSName, nPano);
        }

        public List<HIC_PATIENT> GetDblChartbyJumin2(string strJumin2)
        {
            return hicPatientRepository.GetDblChartbyJumin2(strJumin2);
        }

        public int InsertHicPatientOverLap(long nPano)
        {
            return hicPatientRepository.InsertHicPatientOverLap(nPano);
        }

        public int DeletebyPano(long nPano)
        {
            return hicPatientRepository.DeletebyPano(nPano);
        }

        public List<HIC_PATIENT> GetItembySomeone(string strJob, string strSName, List<string> b04_NOT_PATIENT, string strGubun)
        {
            return hicPatientRepository.GetItembySomeone(strJob, strSName, b04_NOT_PATIENT, strGubun);
        }

        public bool SetHcPatInfoByPtno(string argPtno)
        {
            bool rtnVal = true;

            clsHcType.HCPAT_CLEAR();

            HIC_PATIENT info = hicPatientRepository.GetHcPatInfoByPtno(argPtno);

            if (info ==  null)
            {
                return false;
            }

            clsHcType.HPI.PANO        = info.PANO;
            clsHcType.HPI.SNAME       = info.SNAME;
            clsHcType.HPI.JUMIN       = info.JUMIN;
            clsHcType.HPI.SEX         = info.SEX;
            clsHcType.HPI.MAILCODE    = info.MAILCODE;
            clsHcType.HPI.JUSO1       = info.JUSO1;
            clsHcType.HPI.JUSO2       = info.JUSO2;
            clsHcType.HPI.TEL         = info.TEL;
            clsHcType.HPI.LTDCODE     = info.LTDCODE;
            clsHcType.HPI.JIKGBN      = info.JIKGBN;
            clsHcType.HPI.JIKJONG     = info.JIKJONG;
            clsHcType.HPI.SABUN       = info.SABUN;
            clsHcType.HPI.BUSENAME    = info.BUSENAME;
            clsHcType.HPI.GONGJENG    = info.GONGJENG;
            clsHcType.HPI.IPSADATE    = info.IPSADATE;
            clsHcType.HPI.BUSEIPSA    = info.BUSEIPSA;
            clsHcType.HPI.JISA        = info.JISA;
            clsHcType.HPI.GKIHO       = info.GKIHO;
            clsHcType.HPI.GBSUCHEP    = info.GBSUCHEP;
            clsHcType.HPI.PTNO        = info.PTNO;
            clsHcType.HPI.REMARK      = info.REMARK;
            clsHcType.HPI.KIHO        = info.KIHO;
            clsHcType.HPI.BOGUNSO     = info.BOGUNSO;
            clsHcType.HPI.YOUNGUPSO   = info.YOUNGUPSO;
            clsHcType.HPI.LIVER2      = info.LIVER2;
            clsHcType.HPI.GUMDAESANG  = info.GUMDAESANG;
            clsHcType.HPI.EMAIL       = info.EMAIL;
            clsHcType.HPI.HPHONE      = info.HPHONE;
            clsHcType.HPI.GBIEMUNJIN  = info.GBIEMUNJIN;
            clsHcType.HPI.GBSMS       = info.GBSMS;
            clsHcType.HPI.TEL_CONFIRM = info.TEL_CONFIRM;
            clsHcType.HPI.JUMIN2      = clsAES.DeAES(info.JUMIN2);
            clsHcType.HPI.GBPRIVACY   = info.GBPRIVACY;
            clsHcType.HPI.BUILDNO     = info.BUILDNO;
            clsHcType.HPI.LTDCODE2    = info.LTDCODE2;
            clsHcType.HPI.BIRTHDAY    = info.BIRTHDAY;
            clsHcType.HPI.GBBIRTH     = info.GBBIRTH;
            clsHcType.HPI.LTDTEL      = info.LTDTEL;
            clsHcType.HPI.GAMCODE     = info.GAMCODE;
            clsHcType.HPI.RELIGION    = info.RELIGION;
            clsHcType.HPI.STARTDATE   = info.STARTDATE;
            clsHcType.HPI.LASTDATE    = info.LASTDATE;
            clsHcType.HPI.JINCOUNT    = info.JINCOUNT;
            clsHcType.HPI.FAMILLY     = info.FAMILLY;
            clsHcType.HPI.GAMCODE2    = info.GAMCODE2;
            clsHcType.HPI.SOSOK       = info.SOSOK;
            clsHcType.HPI.VIPREMARK   = info.VIPREMARK;
            clsHcType.HPI.GBJIKWON    = info.GBJIKWON;

            return rtnVal;
        }

        public long Read_MisuNo()
        {
            return hicPatientRepository.Read_MisuNo();
        }

        public HIC_PATIENT GetItembyPtNo(string fstrPtNo)
        {
            return hicPatientRepository.GetItembyName(fstrPtNo);
        }
        public List<HIC_PATIENT> GetItembySabun(string argSabun)
        {
            return hicPatientRepository.GetItembySabun(argSabun);
        }

        public List<HIC_PATIENT> GetItembyName(string strJob, string strSName)
        {
            return hicPatientRepository.GetItembyName(strJob, strSName);
        }

        public int InsertPatient(long fPaNo, string strJumin, string strJumin2)
        {
            return hicPatientRepository.InsertPatient(fPaNo, strJumin, strJumin2);
        }

        public List<HIC_PATIENT> GetHicListByItem(string strSName, string strPtno, long nLtdCode, string strFDate, string strTDate)
        {
            return hicPatientRepository.GetHicListByItem(strSName, strPtno, nLtdCode, strFDate, strTDate);
        }

        public List<HIC_PATIENT> GetHeaListByItem(string strSName, string strPtno, long nLtdCode, string strFDate, string strTDate)
        {
            return hicPatientRepository.GetHeaListByItem(strSName, strPtno, nLtdCode, strFDate, strTDate);
        }

        public List<HIC_PATIENT> GetDblCharSearch()
        {
            return hicPatientRepository.GetDblCharSearch();
        }

        public HIC_PATIENT GetJusobyPano(long nPano, string argSName = "")
        {
            return hicPatientRepository.GetJusobyPano(nPano, argSName);
        }

        public List<HIC_PATIENT> GetPanobyItem(string sItem, string sGubun)
        {
            return hicPatientRepository.GetPanobyItem(sItem, sGubun);
        }

        public string GetPtnoByJuminNo(string argJumin)
        {
            return hicPatientRepository.GetPtnoByJuminNo(argJumin);
        }

        public string GetPanoByJuminNo(string argJumin)
        {
            return hicPatientRepository.GetPanoByJuminNo(argJumin);
        }

        public long Read_Jumin_HicPano(string argJumin)
        {
            return hicPatientRepository.Read_Jumin_HicPano(argJumin);
        }

        public long GetPanobyPtno(string strPtNo)
        {
            return hicPatientRepository.GetPanobyPtno(strPtNo);
        }

        public HIC_PATIENT GetPatInfoByPtno(string argPtno)
        {
            return hicPatientRepository.GetPatInfoByPtno(argPtno);
        }

        public long GetPanobyJumin(string strJumin)
        {
            return hicPatientRepository.GetPanobyJumin(strJumin);
        }

        public int UpDate(HIC_PATIENT item)
        {
            return hicPatientRepository.UpDate(item);
        }

        public HIC_PATIENT GetItembyJumin2NotInSName(string strJumin, List<string> b04_NOT_PATIENT)
        {
            return hicPatientRepository.GetItembyJumin2NotInSName(strJumin, b04_NOT_PATIENT);
        }

        public HIC_PATIENT GetHphoneTelbyPano(long nPano)
        {
            return hicPatientRepository.GetHphoneTelbyPano(nPano);
        }

        public HIC_PATIENT GetItembyPaNo(long fnPano)
        {
            return hicPatientRepository.GetItembyPaNo(fnPano);
        }

        public int GetCountbyJumin2(string strJumin2)
        {
            return hicPatientRepository.GetCountbyJumin2(strJumin2);
        }

        public long GetPanobyPaNo(long pANO)
        {
            return hicPatientRepository.GetPanobyPaNo(pANO);
        }

        public int UpdatebyPaNo(HIC_PATIENT item3, string strGbSuchup)
        {
            return hicPatientRepository.UpdatebyPaNo(item3, strGbSuchup);
        }

        public HIC_PATIENT GetJumin2byPano(long fnPano)
        {
            return hicPatientRepository.GetJumin2byPano(fnPano);
        }

        public string GetPanobyJumin2(string strJumin2)
        {
            return hicPatientRepository.GetPanobyJumin2(strJumin2);
        }

        public HIC_PATIENT GetPanoPtnoByJumin2SName(string argJUMIN, string argSName)
        {
            return hicPatientRepository.GetPanoPtnoByJumin2SName(argJUMIN, argSName);
        }

        public HIC_PATIENT GetPanoPtnoByLikeJuminSNameLtdCode(string argBIRTH, string argSNAME, long argLTDCODE)
        {
            return hicPatientRepository.GetPanoPtnoByLikeJuminSNameLtdCode(argBIRTH, argSNAME, argLTDCODE);
        }

        public string GetPtnoByPano(long nPano)
        {
            return hicPatientRepository.GetPtnoByPano(nPano);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicPatientRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public HIC_PATIENT GetItembyJumin2(string argJumin2)
        {
            return hicPatientRepository.GetItembyJumin2(argJumin2);
        }

        public int UpdatePtNobyJumin2(string pANO, string argJumin)
        {
            return hicPatientRepository.UpdatePtNobyJumin2(pANO, argJumin);
        }

        public HIC_PATIENT GetLtdCodebyPaNo(string argPaNo)
        {
            return hicPatientRepository.GetLtdCodebyPaNo(argPaNo);
        }

        public int UpdatebyPtno(string strGongjeng, string strBuseName, string strBan, string strHPhone, string strGKiho, string strPtNo)
        {
            return hicPatientRepository.UpdatebyPtno(strGongjeng, strBuseName, strBan, strHPhone, strGKiho, strPtNo);
        }

        public int UpdatePtNobyPaNo(string strPtNo, string strPANO)
        {
            return hicPatientRepository.UpdatePtNobyPaNo(strPtNo, strPANO);
        }

        public int GetCountbyPaNo(string strPANO)
        {
            return hicPatientRepository.GetCountbyPaNo(strPANO);
        }

        public int InsertItem(long fnPano, string argJumin, string argJuminAes, string argSName, string argTel, string argPaNo, string argSex, long argLtdCode )
        {
            return hicPatientRepository.InsertItem(fnPano, argJumin, argJuminAes, argSName, argTel, argPaNo, argSex, argLtdCode);
        }

        public int UpdateLtdCodebyPano(long nLtdCode, long nPano)
        {
            return hicPatientRepository.UpdateLtdCodebyPano(nLtdCode, nPano);
        }

        public HIC_PATIENT GetPaNoLtdCodebytJumin2(string strJumin2)
        {
            return hicPatientRepository.GetPaNoLtdCodebytJumin2(strJumin2);
        }

        public List<HIC_PATIENT> GetCountbyPtNo(string argPtno)
        {
            return hicPatientRepository.GetCountbyPtNo(argPtno);
        }

        public int UpdatePrivacyByPtno(string strPtno)
        {
            return hicPatientRepository.UpdatePrivacyByPtno(strPtno);
        }

        public int UpdateJumin2ByPtno(string strPtno, string strJumin2)
        {
            return hicPatientRepository.UpdateJumin2ByPtno(strPtno, strJumin2);
        }
        public int UpdateWebSendByPtno(string strPtno)
        {
            return hicPatientRepository.UpdateWebSendByPtno(strPtno);
        }
        public bool UpdateItemsByPaNo(HIC_PATIENT iHP)
        {
            try
            {
                hicPatientRepository.UpdateItemsByPaNo(iHP);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string GetSNameByPano(long argPano)
        {
            return hicPatientRepository.GetSNameByPano(argPano);
        }

        public string GetHphonebyWrtNo(long argWrtNo)
        {
            return hicPatientRepository.GetHphonebyWrtNo(argWrtNo);
        }

        public string GetJumin2BySnameJuminLikeLtdCode(string strSName, string argJumin)
        {
            return hicPatientRepository.GetJumin2BySnameJuminLikeLtdCode(strSName, argJumin);
        }

        public HIC_PATIENT GetJumin2PtNobyPaNo(long fnPano)
        {
            return hicPatientRepository.GetJumin2PtNobyPaNo(fnPano);
        }

        public string GetJumin2byPtno(string strPtno)
        {
            return hicPatientRepository.GetJumin2byPtno(strPtno);
        }
    }
}
