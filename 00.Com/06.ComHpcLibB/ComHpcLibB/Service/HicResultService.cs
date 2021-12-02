namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicResultService
    {

        private HicResultRepository hicResultRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicResultService()
        {
            this.hicResultRepository = new HicResultRepository();
        }

        public List<HIC_RESULT> Get_Results(long nWrtNo)
        {
            return hicResultRepository.FindAll(nWrtNo);
        }

        public List<HIC_RESULT> Get_Results(List<long> nWrtNo)
        {
            return hicResultRepository.FindAll(nWrtNo);
        }

        public List<HIC_RESULT> Read_Result(string PTNO, string JEPDATE)
        {
            return hicResultRepository.Read_Result(PTNO, JEPDATE);
        }

        public List<HIC_RESULT> Read_Result2(long WRTNO)
        {
            return hicResultRepository.Read_Result2(WRTNO);
        }

        public HIC_RESULT Read_Result3(long WRTNO)
        {
            return hicResultRepository.Read_Result3(WRTNO);
        }

        public List<HIC_RESULT> Read_Result_Acitve(long nWrtNo, List<string> strActPart)
        {
            return hicResultRepository.Read_Result_Acitve(nWrtNo, strActPart);
        }

        public HIC_RESULT Read_Result_YN(long WRTNO, string EXCODE)
        {
            return hicResultRepository.Read_Result_YN(WRTNO, EXCODE);
        }

        public HIC_RESULT Read_Result_Data(string strCode)
        {
            return hicResultRepository.Read_Result_Data(strCode);
        }

        public int PanUpDate(string argPan, string rOWID)
        {
            return hicResultRepository.PanUpDate(argPan, rOWID);
        }

        public string Read_Sangdam_Acting(long wrtNo)
        {
            return hicResultRepository.Read_Sangdam_Acting(wrtNo);
        }

        public string Read_ExCode(long argWrtNo, List<string> strExCode)
        {
            return hicResultRepository.Read_ExCode(argWrtNo, strExCode);
        }

        public HIC_RESULT Read_ExCode2(long argWrtNo, string[] strExCode)
        {
            return hicResultRepository.Read_ExCode2(argWrtNo, strExCode);
        }

        public string Read_CervicalCancer(long argWrtNo)
        {
            return hicResultRepository.Read_CervicalCancer(argWrtNo);
        }

        public int Chk_NonExcute_Result_Count(long argWRTNO)
        {
            return hicResultRepository.Chk_NonExcute_Result_Count(argWRTNO);
        }

        public List<HIC_RESULT> GetExCodebyWrtNo_All(long wRTNO, string[] strExCode)
        {
            return hicResultRepository.GetExCodebyWrtNo_All(wRTNO, strExCode);
        }

        public IList<HIC_RESULT> Read_Result_All(long argWRTNO, string strGubun = "", string[] argCodes = null)
        {
            return hicResultRepository.Read_Result_All(argWRTNO, strGubun, argCodes);
        }

        public int UpDate(HIC_RESULT item3)
        {
            return hicResultRepository.UpDate(item3);
        }

        public IList<HIC_RESULT> GetResultByWrtnosExCodes(string[] strWrtno, string[] strCodes)
        {
            return hicResultRepository.GetResultByWrtnosExCodes(strWrtno, strCodes);
        }

        public IList<HIC_RESULT> GetResultByWrtnoExCodes(long argWRTNO, string[] strCodes)
        {
            return hicResultRepository.GetResultByWrtnoExCodes(argWRTNO, strCodes);
        }

        public int UpdateResultPanjengbyRowId(string strResult, string strNewPan, string strResCode, string idNumber, string strROWID)
        {
            return hicResultRepository.UpdateResultPanjengbyRowId(strResult, strNewPan, strResCode, idNumber, strROWID);
        }

        public int UpdatebyRowId(HIC_RESULT item)
        {
            return hicResultRepository.UpdatebyRowId(item);
        }

        //public int Update_Hic_Result(long fnWRTNO, string strPart)
        //{
        //    throw new NotImplementedException();
        //}

        public int Update_Hic_Result(string strSYS, long idNumber, long fnWRTNO, string[] strExamList)
        {
            return hicResultRepository.Update_Hic_Result(strSYS, idNumber, fnWRTNO, strExamList);
        }

        public string GetXrayNoByWrtno(long fnWRTNO)
        {
            return hicResultRepository.GetXrayNoByWrtno(fnWRTNO);
        }

        public List<HIC_RESULT> GetAllByWrtNo(long fnWRTNO)
        {
            return hicResultRepository.GetAllByWrtNo(fnWRTNO);
        }

        public int Update_ResultbyRowId(string v1, long v2, string strRowId)
        {
            return hicResultRepository.Update_ResultbyRowId(v1, v2, strRowId);
        }

        public int Update_Hic_Result_Complete(string idNumber, string strROWID)
        {
            return hicResultRepository.Update_Hic_Result_Complete(idNumber, strROWID);
        }

        public bool Update_Reset_Acting(long fnWRTNO)
        {
            try
            {
                hicResultRepository.Update_Reset_Acting(fnWRTNO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HIC_RESULT GetCount(string strJong, long nMirNo)
        {
            return hicResultRepository.GetCount(strJong, nMirNo);
        }

        public List<HIC_RESULT> GetWaitCountByPart(string argEntPart)
        {
            return hicResultRepository.GetWaitCountByPart(argEntPart);
        }

        public List<HIC_RESULT> GetExcodebyExCode(long wRTNO)
        {
            return hicResultRepository.GetExcodebyExCode(wRTNO);
        }

        public int GetResultCount(long nWrtNo)
        {
            return hicResultRepository.GetResultCount(nWrtNo);
        }

        public List<HIC_RESULT> GetRowIdRowIdbyWrtNo(long fnWRTNO)
        {
            return hicResultRepository.GetRowIdRowIdbyWrtNo(fnWRTNO);
        }

        public int Update_ResultbyWrtNoExCode(long argWrtNo, string argJong)
        {
            return hicResultRepository.Update_ResultbyWrtNoExCode(argWrtNo, argJong);
        }

        public int GetCountbyPtNo(string sPtNo)
        {
            return hicResultRepository.GetCountbyPtNo(sPtNo);
        }

        public int GetExCodebyWrtNo(long argWRTNO)
        {
            return hicResultRepository.GetExCodebyWrtNo(argWRTNO);
        }

        public int GetCountbyWrtNoExCodeChkNew(long nWrtNo, string strPart, string strChkNew, List<string> strExCode)
        {
            return hicResultRepository.GetCountbyWrtNoExCodeChkNew(nWrtNo, strPart, strChkNew, strExCode);
        }

        public int GetExCodebyWrtNo_Second(long argWRTNO)
        {
            return hicResultRepository.GetExCodebyWrtNo_Second(argWRTNO);
        }

        public int GetResultCount_ACT(long nWRTNO)
        {
            return hicResultRepository.GetResultCount_ACT(nWRTNO);
        }

        public int Update_Auto_Result(string strActValue, long nSabun, long fnWRTNO, string strROWID)
        {
            return hicResultRepository.Update_Auto_Result(strActValue, nSabun, fnWRTNO, strROWID);
        }

        public List<HIC_RESULT> GetJochiListByWrtnoCodeIN(long nWRTNO, string[] lstExam)
        {
            return hicResultRepository.GetExCodebyWrtNo_All(nWRTNO, lstExam);
        }

        public int Read_Result_Acitve_Status(long nWrtNo, string sysdate, long pano, string strEntPart, string strJong)
        {
            return hicResultRepository.Read_Result_Acitve_Status(nWrtNo, sysdate, pano, strEntPart, strJong);
        }

        public string GetRowidByWrtno(long argWRTNO)
        {
            return hicResultRepository.GetRowidByWrtno(argWRTNO);
        }

        public string GetResultByWrtNo(long nWrtNo)
        {
            return hicResultRepository.GetResultByWrtNo(nWrtNo);
        }

        public int Update_Auto_Result_Hea(HIC_RESULT item, string strTemp)
        {
            return hicResultRepository.Update_Auto_Result_Hea(item, strTemp);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtno1(long fnWrtno1)
        {
            return hicResultRepository.GetExCodeResultbyWrtno1(fnWrtno1);
        }

        public HIC_RESULT GetResultCount_Blood(long nWRTNO)
        {
            return hicResultRepository.GetResultCount_Blood(nWRTNO);
        }

        public int UpdateResCode(string strResCode, string strResult, long fnWRTNO)
        {
            return hicResultRepository.UpdateResCode(strResCode, strResult, fnWRTNO);
        }

        public int InsertSelectbyWrtNo(long nWrtNo, long fnWrtNo)
        {
            return hicResultRepository.InsertSelectbyWrtNo(nWrtNo, fnWrtNo);
        }

        public int GetResultCount_Flag(long nWRTNO)
        {
            return hicResultRepository.GetResultCount_Flag(nWRTNO);
        }

        public int Update_Result_Flag(long nSabun, long nWRTNO)
        {
            return hicResultRepository.Update_Result_Flag(nSabun, nWRTNO);
        }

        public List<HIC_RESULT> GetOnlyExCodebyWrtNo(long nWRTNO)
        {
            return hicResultRepository.GetOnlyExCodebyWrtNo(nWRTNO);
        }

        public int UpdateResultbyRowId(HIC_RESULT item)
        {
            return hicResultRepository.UpdateResultbyRowId(item);
        }

        public int GetCountbyWrtNoCode(long nWRTNO, string strCode)
        {
            return hicResultRepository.GetCountbyWrtNoCode(nWRTNO, strCode);
        }

        public int GetResultCount_ChulAutFlag(long nWRTNO)
        {
            return hicResultRepository.GetResultCount_ChulAutFlag(nWRTNO);
        }

        public int Update_Result_ChulAutoFlag(HIC_RESULT item, string[] strCodes)
        {
            return hicResultRepository.Update_Result_ChulAutoFlag(item, strCodes);
        }

        public int GetResultCount_BMD(long nWRTNO)
        {
            return hicResultRepository.GetResultCount_BMD(nWRTNO);
        }

        public int Update_ResultbyWrtNo(HIC_RESULT item, List<string> strExCode, string argReadDate = "")
        {
            return hicResultRepository.Update_ResultbyWrtNo(item, strExCode, argReadDate);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtNo(long fnWRTNO, string strExCode = "")
        {
            return hicResultRepository.GetExCodeResultbyWrtNo(fnWRTNO, strExCode);
        }

        public HIC_RESULT GetResultRowIdbyWrtNo(long fnWRTNO, string strNewCode, string strExCode)
        {
            return hicResultRepository.GetResultRowIdbyWrtNo(fnWRTNO, strNewCode, strExCode);
        }

        public int Update_Auto_Result_Hic(HIC_RESULT item)
        {
            return hicResultRepository.Update_Auto_Result_Hic(item);
        }

        public HIC_RESULT GetResultRowIdbyExCode(long fnWRTNO, string strExCode)
        {
            return hicResultRepository.GetResultRowIdbyExCode(fnWRTNO, strExCode);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtNoExCode(long argHcWRTNO)
        {
            return hicResultRepository.GetExCodeResultbyWrtNoExCode(argHcWRTNO);
        }

        public int UpdateResultbyWrtNo(string idNumber, long fnWRTNO)
        {
            return hicResultRepository.UpdateResultbyWrtNo(idNumber, fnWRTNO);
        }

        public int UpdateActiveResultbyWrtNoExCode(string idNumber, long fnWRTNO)
        {
            return hicResultRepository.UpdateActiveResultbyWrtNoExCode(idNumber, fnWRTNO);
        }

        public int Update_ResultbyWrtNo_Auto(HIC_RESULT item)
        {
            return hicResultRepository.Update_ResultbyWrtNo_Auto(item);
        }

        public string GetResultbyExCode(long wRTNO, string strJong)
        {
            return hicResultRepository.GetResultbyExCode(wRTNO, strJong);
        }

        public List<HIC_RESULT> GetExCodeResultbyWrtNoExCode(long wRTNO, List<string> strCodeList)
        {
            return hicResultRepository.GetExCodeResultbyWrtNoExCode(wRTNO, strCodeList);
        }

        public int GetExCodebyWrtNoExCode(long argWrtNo)
        {
            return hicResultRepository.GetExCodebyWrtNoExCode(argWrtNo);
        }

        public HIC_RESULT GetEntSabunbyWrtNo(long wRTNO)
        {
            return hicResultRepository.GetEntSabunbyWrtNo(wRTNO);
        }

        public int UpdateSangdambyWrtnoExCode(long fnWRTNO, string strExam, string strSangdam)
        {
            return hicResultRepository.UpdateSangdambyWrtnoExCode(fnWRTNO, strExam, strSangdam);
        }

        public string GetResultOnlybyWrtNoExCode(long nWRTNO, string strExCode)
        {
            return hicResultRepository.GetResultOnlybyWrtNoExCode(nWRTNO, strExCode);
        }

        public HIC_RESULT GetResultbyWrtNo(long nWRTNO, List<string> strExcode)
        {
            return hicResultRepository.GetResultbyWrtNo(nWRTNO, strExcode);
        }

        public int UpdateResultEntSabunbyWrtNoExCode(string strResult, long nWRTNO, long nSabun, string strExCode)
        {
            return hicResultRepository.UpdateResultEntSabunbyWrtNoExCode(strResult, nWRTNO, nSabun, strExCode);
        }

        public HIC_RESULT GetResultbyJepsu(string strPano, string strExCode, int nDay)
        {
            return hicResultRepository.GetResultbyJepsu(strPano, strExCode, nDay);
        }

        public int UpdatebyRowId(string strResult, string rOWID)
        {
            return hicResultRepository.UpdatebyRowId(strResult, rOWID);
        }

        public HIC_RESULT GetResultRowIdbyPtNo(string strPano, string[] strCodes, int nDay, string strGubun)
        {
            return hicResultRepository.GetResultRowIdbyPtNo(strPano, strCodes, nDay, strGubun);
        }

        public HIC_RESULT GetResultByWrtNoExCode(long nWRTNO, string[] strCodes)
        {
            return hicResultRepository.GetResultByWrtNoExCode(nWRTNO, strCodes);
        }

        public int UpdateEntSabunbyWrtNo(long nSabun, long fnWRTNO)
        {
            return hicResultRepository.UpdateEntSabunbyWrtNo(nSabun, fnWRTNO);
        }

        public int UpdatebyWrtNo(string argReadTime, string argResult, long nWRTNO, string[] strCodes)
        {
            return hicResultRepository.UpdatebyWrtNo(argReadTime, argResult, nWRTNO, strCodes);
        }

        public int GetCountbyWrtNoExCode(string argWrtNo, string argJepNo)
        {
            return hicResultRepository.GetCountbyWrtNoExCode(argWrtNo, argJepNo);
        }

        public int GetEndoCountbyWrtNoExCode(long nWrtno, string strExCode)
        {
            return hicResultRepository.GetEndoCountbyWrtNoExCode(nWrtno, strExCode);
        }

        public List<HIC_RESULT> GetItembyWrtNo(long nWrtNo)
        {
            return hicResultRepository.GetItembyWrtNo(nWrtNo);
        }

        public int UpdateSangdambyWrtno(long fnWRTNO, string strExam, string strSangDam)
        {
            return hicResultRepository.UpdateSangdambyWrtno(fnWRTNO, strExam, strSangDam);
        }

        public HIC_RESULT GetJepDate(string fstrFDate, string fstrTDate, string fstrLtdCode, string strGjYear)
        {
            return hicResultRepository.GetJepDate(fstrFDate, fstrTDate, fstrLtdCode, strGjYear);
        }

        public int GetCountbyWrtNo(long fnWRTNO, string[] strExCodes)
        {
            return hicResultRepository.GetCountbyWrtNo(fnWRTNO, strExCodes);
        }

        public int GetCountbyWrtNoNotIn(long fnWRTNO, string[] strExCodes)
        {
            return hicResultRepository.GetCountbyWrtNoNotIn(fnWRTNO, strExCodes);
        }

        public int GetCountbyWrtNoExcodes(long fnWRTNO, string[] strExCodes)
        {
            return hicResultRepository.GetCountbyWrtNoExcodes(fnWRTNO, strExCodes);
        }

        public string GetRowidByOneExcodeWrtno(string argExcode, long argwRTNO)
        {
            return hicResultRepository.GetRowidByOneExcodeWrtno(argExcode, argwRTNO);
        }

        public string GetRowidByOneExcodePtnoJepdate(string argExcode, string argPTNO, string argJEPDATE)
        {
            return hicResultRepository.GetRowidByOneExcodePtnoJepdate(argExcode, argPTNO, argJEPDATE);
        }

        public HIC_RESULT GetExCodebyWrtNo_RESULT1(long ArgWRTNO, string ArgExCode, long ArgPP, string ArgGBN)
        {
            return hicResultRepository.GetExCodebyWrtNo_RESULT1(ArgWRTNO, ArgExCode, ArgPP, ArgGBN);
        }

        public HIC_RESULT GetExCodebyWrtNo_RESULT2(long ArgWRTNO, string ArgExCode)
        {
            return hicResultRepository.GetExCodebyWrtNo_RESULT2(ArgWRTNO, ArgExCode);
        }
        public HIC_RESULT GetExCodebyWrtNo_RESULT3(long ArgWRTNO, string ArgExCode)
        {
            return hicResultRepository.GetExCodebyWrtNo_RESULT3(ArgWRTNO, ArgExCode);
        }

        public HIC_RESULT GetExCodebyWrtNo_ExcodeYN(long ArgWRTNO, string ArgExCode, string ArgGUBUN)
        {
            return hicResultRepository.GetExCodebyWrtNo_ExcodeYN(ArgWRTNO, ArgExCode, ArgGUBUN);
        }

        public int GetEntSabunbyWrtNoExCode(long wRTNO, string strPart, string strChkNew, List<string> fstrPartExam)
        {
            return hicResultRepository.GetEntSabunbyWrtNoExCode(wRTNO, strPart, strChkNew, fstrPartExam);
        }

        public int GetItembyWrtNoExCode(long fnWRTNO)
        {
            return hicResultRepository.GetItembyWrtNoExCode(fnWRTNO);
        }

        public int UpdatebyWrtNoExCode(long nSabun, long fnWRTNO)
        {
            return hicResultRepository.UpdatebyWrtNoExCode(nSabun, fnWRTNO);
        }

        public int UpdateGroupCodebyWrtNoCode(string strNew_Group, string strNew_ExCode, long nWrtNo, string strOLD_Group, string strOld_ExCode)
        {
            return hicResultRepository.UpdateGroupCodebyWrtNoCode(strNew_Group, strNew_ExCode, nWrtNo, strOLD_Group, strOld_ExCode);
        }

        public int UpdateResultbyWrtNoExCode(string strResult, long nWRTNO, long nSabun, string strExCode)
        {
            return hicResultRepository.UpdateResultbyWrtNoExCode(strResult, nWRTNO, nSabun, strExCode);
        }

        public int UpdatebyWrtNoExCode(long nWRTNO)
        {
            return hicResultRepository.UpdatebyWrtNoExCode(nWRTNO);
        }

        public int UpdatebyWrtNoSabunPano(string idNumber, string strJepDate, long nPano, string strResult, string[] strExCode)
        {
            return hicResultRepository.UpdatebyWrtNoSabunPano(idNumber, strJepDate, nPano, strResult, strExCode);
        }

        public string GetAllByWrtNo(string argWrtNo, string argJepNo)
        {
            return hicResultRepository.GetAllByWrtNo(argWrtNo, argJepNo);
        }

        public int GetCntbyWrtNoExCode(long fnWrtNo, string[] strExCode)
        {
            return hicResultRepository.GetCntbyWrtNoExCode(fnWrtNo, strExCode);
        }

        public List<HIC_RESULT> GetExCodeResultbyOnlyWrtNo(long nWrtNo)
        {
            return hicResultRepository.GetExCodeResultbyOnlyWrtNo(nWrtNo);
        }

        public HIC_RESULT GetActivebyWrtNo(long fnWRTNO)
        {
            return hicResultRepository.GetActivebyWrtNo(fnWRTNO);
        }

        public string GetRowidStomachByWrtno(long wRTNO)
        {
            return hicResultRepository.GetRowidStomachByWrtno(wRTNO);
        }

        public int UpdateActiveResultbyWrtNo(string idNumber, long fnWRTNO)
        {
            return hicResultRepository.UpdateActiveResultbyWrtNo(idNumber, fnWRTNO);
        }

        public object GetRowidColonByWrtno(long wRTNO)
        {
            return hicResultRepository.GetRowidColonByWrtno(wRTNO);
        }

        public int UpdateResultActivebyWrtNoExCode(long fnWRTNO, string strInsomniaMun, string idNumber, string strExCode)
        {
            return hicResultRepository.UpdateResultActivebyWrtNoExCode(fnWRTNO, strInsomniaMun, idNumber, strExCode);
        }

        public List<HIC_RESULT> GetResultbyWrtNoExCode(long fnWRTNO, List<string> sExCode)
        {
            return hicResultRepository.GetResultbyWrtNoExCode(fnWRTNO, sExCode);
        }

        public List<HIC_RESULT> GetResultbyWrtNoExCodeList(long nWRTNO, List<string> sExCode)
        {
            return hicResultRepository.GetResultbyWrtNoExCodeList(nWRTNO, sExCode);
        }

        public int UpdateBimanbyWrtNoExCode(string strBiman, string idNumber, long nWRTNO, string strExCode)
        {
            return hicResultRepository.UpdateBimanbyWrtNoExCode(strBiman, idNumber, nWRTNO, strExCode);
        }

        public List<HIC_RESULT> GetExCodeLoopbyWrtNo(long argWrtNo)
        {
            return hicResultRepository.GetExCodeLoopbyWrtNo(argWrtNo);
        }

        public int UpdateActivebyWrtNoExCode(string idNumber, long fnWRTNO)
        {
            return hicResultRepository.UpdateActivebyWrtNoExCode(idNumber, fnWRTNO);
        }

        public List<HIC_RESULT> GetResultExCodebyWrtNo(long fnWrtNo)
        {
            return hicResultRepository.GetResultExCodebyWrtNo(fnWrtNo);
        }

        public string Chk_Simli_ExCode(long argWrtno)
        {
            return hicResultRepository.Chk_Simli_ExCode(argWrtno);
        }

        public void InsertDelSelectbyRowid(string argRid)
        {
            hicResultRepository.InsertDelSelectbyRowid(argRid);
        }

        public void DeleteByRowid(string argRid)
        {
            hicResultRepository.DeleteByRowid(argRid);
        }

        public bool InsertData(HIC_RESULT hRES)
        {
            try
            {
                hicResultRepository.InsertData(hRES);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<HIC_RESULT> GetListByWrtnoCodeIN(long argWrtno, List<string> lstExcode, string argDept)
        {
            return hicResultRepository.GetListByWrtnoCodeIN(argWrtno, lstExcode, argDept);
        }

        public int UpdateItembyRowId(string strResult, string strNewPan, string rOWID)
        {
            return hicResultRepository.UpdateItembyRowId(strResult, strNewPan, rOWID);
        }

        public HIC_RESULT GetResultRowIdbyWrtNoExCode(long fnWRTNO, string strExCode)
        {
            return hicResultRepository.GetResultRowIdbyWrtNoExCode(fnWRTNO, strExCode);
        }

        public HIC_RESULT GetResultbyWrtNoPart(long fnWRTNO, string strPart)
        {
            return hicResultRepository.GetResultbyWrtNoPart(fnWRTNO, strPart);
        }

        public int UpdateActivebyPartWrtNo(string strPart, long argWrtNo)
        {
            return hicResultRepository.UpdateActivebyPartWrtNo(strPart, argWrtNo);
        }

        public List<HIC_RESULT> GetSpcResByPtnoSDate(string strPtno, string strSDate)
        {
            return hicResultRepository.GetSpcResByPtnoSDate(strPtno, strSDate);
        }

        public long GetCountByWrtnoInExCodeIn(List<long> lstHicWrtno, List<string> lstExCode)
        {
            return hicResultRepository.GetCountByWrtnoInExCodeIn(lstHicWrtno, lstExCode);
        }

        public int GetCountbyWrtNoExCodeNoResult(long nWRTNO)
        {
            return hicResultRepository.GetCountbyWrtNoExCodeNoResult(nWRTNO);
        }

        public int UpdateResultActiveEntSabunEntTimebyRowId(string idNumber, string strROWID)
        {
            return hicResultRepository.UpdateResultActiveEntSabunEntTimebyRowId(idNumber, strROWID);
        }

        public HIC_RESULT GetExCodeResultbyWrtNoInExCode(long nWrtNo)
        {
            return hicResultRepository.GetExCodeResultbyWrtNoInExCode(nWrtNo);
        }

        public int UpdateTeethbyWrtNoExCode(long nWrtNo, string IdNumber)
        {
            return hicResultRepository.UpdateTeethbyWrtNoExCode(nWrtNo, IdNumber);
        }

        public int GetCountByWrtnoInExCode(long nWrtNo)
        {
            return hicResultRepository.GetCountByWrtnoInExCode(nWrtNo);
        }

        public int UpdateResultEntSabunEntTimeActivebyWrtNoExCode(long nWrtNo, string idNumber)
        {
            return hicResultRepository.UpdateResultEntSabunEntTimeActivebyWrtNoExCode(nWrtNo, idNumber);
        }

        public int GetCountbyWrtNoExCodeNoResult(long nWrtNo, string strExCode)
        {
            return hicResultRepository.GetCountbyWrtNoExCodeNoResult(nWrtNo, strExCode);
        }

        public int UpdatebyWrtNoExCodeTX07(long nWrtNo, string strResult, string idNumber)
        {
            return hicResultRepository.UpdatebyWrtNoExCodeTX07(nWrtNo, strResult, idNumber);
        }

        public HIC_RESULT GetCountbyWrtNoPart(long fnWRTNO, string strPart)
        {
            return hicResultRepository.GetCountbyWrtNoPart(fnWRTNO, strPart);
        }

        public int UpDate_Audio_Auto(HIC_RESULT item1)
        {
            return hicResultRepository.UpDate_Audio_Auto(item1);
        }

        public int UpdatePointbyWrtNoExCode(string idNumber, long fnWRTNO)
        {
            return hicResultRepository.UpdatePointbyWrtNoExCode(idNumber, fnWRTNO);
        }

        public int UpdateResultPanjengResCodeActivebyRowId(string strResult, string strNewPan, string strResCode, string idNumber, string sSysDate, string strROWID)
        {
            return hicResultRepository.UpdateResultPanjengResCodeActivebyRowId(strResult, strNewPan, strResCode, idNumber, sSysDate, strROWID);
        }

        public int UpdateChangeItembyWrtNoExCode(long fnWrtNo, string strExCode, string strResult)
        {
            return hicResultRepository.UpdateChangeItembyWrtNoExCode(fnWrtNo, strExCode, strResult);
        }

        public List<HIC_RESULT> GetWrtNoExCodeResultbyWrtNo(long argWRTNO)
        {
            return hicResultRepository.GetWrtNoExCodeResultbyWrtNo(argWRTNO);
        }

        public List<HIC_RESULT> GetExCodeREsultbyWrtNoExCodeNotLike(long argWrtNo)
        {
            return hicResultRepository.GetExCodeREsultbyWrtNoExCodeNotLike(argWrtNo);
        }

        public List<HIC_RESULT> GetCorrectedEyeSightbyWrtNoExCode(long nWRTNO, List<string> strCodeList2)
        {
            return hicResultRepository.GetCorrectedEyeSightbyWrtNoExCode(nWRTNO, strCodeList2);
        }

        public int GetCountbyWrtNoInExCode(long nWRTNO, List<string> strExCode)
        {
            return hicResultRepository.GetCountbyWrtNoInExCode(nWRTNO, strExCode);
        }

        public List<HIC_RESULT> GetItembyOnlyWrtNo(long nWRTNO)
        {
            return hicResultRepository.GetItembyOnlyWrtNo(nWRTNO);
        }
    }
}
