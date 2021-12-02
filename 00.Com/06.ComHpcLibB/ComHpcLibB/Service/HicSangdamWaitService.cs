namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicSangdamWaitService
    {
        
        private HicSangdamWaitRepository hicSangdamWaitRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicSangdamWaitService()
        {
			this.hicSangdamWaitRepository = new HicSangdamWaitRepository();
        }

        public string GetNextRoomByWrtNo(long argWrtNo, string argRoom = "")
        {
            return hicSangdamWaitRepository.GetNextRoomByWrtNo(argWrtNo, argRoom);
        }

        public int Update_Patient_Call(long argPano, string argRoom)
        {
            return hicSangdamWaitRepository.Update_Patient_Call(argPano, argRoom);
        }

        public int Delete_Hic_Sangdam_Wait(long argPano, string strGubun = "")
        {
            return hicSangdamWaitRepository.Delete_Hic_Sangdam_Wait(argPano, strGubun);
        }

        public long GetMaxWaitNoByRoom(string argRoom)
        {
            return hicSangdamWaitRepository.GetMaxWaitNoByRoom(argRoom);
        }

        public int Insert_Hic_Sangdam_Wait(long nWrtNo, string strSName, string strSex, long nAge, string strGjJong, string strRoom, long nWait, long argPano, string strNextRoom)
        {
            return hicSangdamWaitRepository.Insert_Hic_Sangdam_Wait(nWrtNo, strSName, strSex, nAge, strGjJong, strRoom, nWait, argPano, strNextRoom);
        }

        public int GetWaitCountbyRoomCd(string strRoom)
        {
            return hicSangdamWaitRepository.GetWaitCountbyRoomCd(strRoom);
        }

        public HIC_SANGDAM_WAIT GetNextRoomGubunByWrtNo(long fWrtNo)
        {
            return hicSangdamWaitRepository.GetNextRoomGubunByWrtNo(fWrtNo);
        }

        public string GetRoomNameByRoomCd(string argRoom)
        {
            return hicSangdamWaitRepository.GetRoomNameByRoomCd(argRoom);
        }

        public int GetMaxWaitNobyRoomCd(string argRoom)
        {
            return hicSangdamWaitRepository.GetMaxWaitNobyRoomCd(argRoom);
        }

        public HIC_SANGDAM_WAIT GetNextRoomByWrtNoGubun(long fnWRTNO, string strGubun)
        {
            return hicSangdamWaitRepository.GetNextRoomByWrtNoGubun(fnWRTNO, strGubun);
        }

        public int GetCountbyWrtNoGubun(string gstrDrRoom, long nWrtNo)
        {
            return hicSangdamWaitRepository.GetCountbyWrtNoGubun(gstrDrRoom, nWrtNo);
        }

        public int UpdateCallTimeDisplaybyOnlyWrtNo(long nWrtNo)
        {
            return hicSangdamWaitRepository.UpdateCallTimeDisplaybyOnlyWrtNo(nWrtNo);
        }

        public string GetGubunbyPaNoWrtNo(long fnPaNo, long fnWrtNo)
        {
            throw new NotImplementedException();
        }

        public long GetMaxWaitNobyGubun(string strGubun, string strTemp = "")
        {
            return hicSangdamWaitRepository.GetMaxWaitNobyGubun(strGubun, strTemp);
        }

        public int UpdateWaitNobyPaNo(HIC_SANGDAM_WAIT item)
        {
            return hicSangdamWaitRepository.UpdateWaitNobyPaNo(item);
        }

        public int Delete_JepsuCancel()
        {
            return hicSangdamWaitRepository.Delete_JepsuCancel();
        }

        public List<HIC_SANGDAM_WAIT> GetItem()
        {
            return hicSangdamWaitRepository.GetItem();
        }

        public List<HIC_SANGDAM_WAIT> GetItem_GbCallNull()
        {
            return hicSangdamWaitRepository.GetItem_GbCallNull();
        }

        public string GetINextRoombyWrtNo(long fnWRTNO)
        {
            return hicSangdamWaitRepository.GetINextRoombyWrtNo(fnWRTNO);
        }

        public int UpdateGbCallbyWrtNo(string gstrDrRoom, long fnWRTNO)
        {
            return hicSangdamWaitRepository.UpdateGbCallbyWrtNo(gstrDrRoom, fnWRTNO);
        }

        public long GetWaitbyGubunEntTime(string strRoom)
        {
            return hicSangdamWaitRepository.GetWaitbyGubunEntTime(strRoom);
        }

        public int Insert(HIC_SANGDAM_WAIT item)
        {
            return hicSangdamWaitRepository.Insert(item);
        }

        public int DeletebyGubun(string gstrDrRoom)
        {
            return hicSangdamWaitRepository.DeletebyGubun(gstrDrRoom);
        }

        public int InsertCall(string gstrDrRoom)
        {
            return hicSangdamWaitRepository.InsertCall(gstrDrRoom);
        }

        public List<HIC_SANGDAM_WAIT> GetNextRoombyWrtNo(long fnWRTNO)
        {
            return hicSangdamWaitRepository.GetNextRoombyWrtNo(fnWRTNO);
        }

        public int GetCountbyWrtNo(long nWrtNo)
        {
            return hicSangdamWaitRepository.GetCountbyWrtNo(nWrtNo);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNobyWrtNo(long nWrtNo)
        {
            return hicSangdamWaitRepository.GetWrtNobyWrtNo(nWrtNo);
        }

        public int UpdateCallTimeDisplaybyWrtNo(List<string> strWrtNo, string strGubun = "")
        {
            return hicSangdamWaitRepository.UpdateCallTimeDisplaybyWrtNo(strWrtNo, strGubun);
        }

        public int UpdateCallTimebyWrtNo(string strSysDate, long nWrtNo)
        {
            return hicSangdamWaitRepository.UpdateCallTimebyWrtNo(strSysDate, nWrtNo);
        }

        public int UpdateGbCallCallTimeGubunbyWrtNo(string gstrDrRoom, long fnWRTNO)
        {
            return hicSangdamWaitRepository.UpdateGbCallCallTimeGubunbyWrtNo(gstrDrRoom, fnWRTNO);
        }

        public int DeletebyPaNo(long fnPano)
        {
            return hicSangdamWaitRepository.DeletebyPaNo(fnPano);
        }

        public int UpdateGbCallGubunbyWrtNo(string gstrDrRoom, long fnWRTNO)
        {
            return hicSangdamWaitRepository.UpdateGbCallGubunbyWrtNo(gstrDrRoom, fnWRTNO);
        }

        public int GetCountbyOnlyWrtNo(long wRTNO)
        {
            return hicSangdamWaitRepository.GetCountbyOnlyWrtNo(wRTNO);
        }

        public string GetGubunbyPaNoWrtNo(long gnWRTNO)
        {
            return hicSangdamWaitRepository.GetGubunbyPaNoWrtNo(gnWRTNO);
        }

        public List<HIC_SANGDAM_WAIT> GetItembyGubunEntTime(string gstrDrRoom, string strFrDate, string strToDate)
        {
            return hicSangdamWaitRepository.GetItembyGubunEntTime(gstrDrRoom, strFrDate, strToDate);
        }

        public HIC_SANGDAM_WAIT GetMaxWaitNobyGubunWaitNo(string gstrDrRoom)
        {
            return hicSangdamWaitRepository.GetMaxWaitNobyGubunWaitNo(gstrDrRoom);
        }

        public int UpdateWaitNobyWrtNo(int nWaitNo, long wRTNO, string gstrDrRoom, string strFrDate1, string strToDate1)
        {
            return hicSangdamWaitRepository.UpdateWaitNobyWrtNo(nWaitNo, wRTNO, gstrDrRoom, strFrDate1, strToDate1);
        }

        public int UpdateWaitNoGubunbyWrtNo(string gstrDrRoom, long gnWRTNO)
        {
            return hicSangdamWaitRepository.UpdateWaitNoGubunbyWrtNo(gstrDrRoom, gnWRTNO);
        }

        public string GetGubunbyWrtNo(long gnWRTNO)
        {
            return hicSangdamWaitRepository.GetGubunbyWrtNo(gnWRTNO);
        }

        public HIC_SANGDAM_WAIT GetMaxWaitNobyDrRoom(string gstrDrRoom)
        {
            return hicSangdamWaitRepository.GetMaxWaitNobyDrRoom(gstrDrRoom);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNoWaitNobyGubunEntTime(string gstrDrRoom, string strFrDate, string strToDate)
        {
            return hicSangdamWaitRepository.GetWrtNoWaitNobyGubunEntTime(gstrDrRoom, strFrDate, strToDate);
        }

        public int UpdateWaitNobyWrtnoGubunEntTime(int nWaitNo, long wRTNO, string gstrDrRoom, string strFrDate1, string strToDate1)
        {
            return hicSangdamWaitRepository.UpdateWaitNobyWrtnoGubunEntTime(nWaitNo, wRTNO, gstrDrRoom, strFrDate1, strToDate1);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNobyGubunNotWrtNo(string gstrDrRoom, long nWrtNo)
        {
            return hicSangdamWaitRepository.GetWrtNobyGubunNotWrtNo(gstrDrRoom, nWrtNo);
        }

        public List<HIC_SANGDAM_WAIT> GetNextRoombyWrtNoInGubun(long fnWRTNO, List<string> strGubun)
        {
            return hicSangdamWaitRepository.GetNextRoombyWrtNoInGubun(fnWRTNO, strGubun);
        }

        public int DeletebyWrtNo(long wRTNO)
        {
            return hicSangdamWaitRepository.DeletebyWrtNo(wRTNO);
        }

        public HIC_SANGDAM_WAIT GetWaitNobyGubun(string gstrDrRoom)
        {
            return hicSangdamWaitRepository.GetWaitNobyGubun(gstrDrRoom);
        }

        public List<HIC_SANGDAM_WAIT> GetWrtNoWaitNo(string gstrDrRoom, string strFrDate, string strToDate)
        {
            return hicSangdamWaitRepository.GetWrtNoWaitNo(gstrDrRoom, strFrDate, strToDate);
        }

        public int InserWrtNoSNameGubunt(HIC_SANGDAM_WAIT item)
        {
            return hicSangdamWaitRepository.InserWrtNoSNameGubunt(item);
        }

        public string GetNextRoomByWrtNoInGubun(long fnWRTNO, string[] strGubun)
        {
            return hicSangdamWaitRepository.GetNextRoomByWrtNoInGubun(fnWRTNO, strGubun);
        }

        public int GetCountbyGubunSName(string fstrRoom, string strName)
        {
            return hicSangdamWaitRepository.GetCountbyGubunSName(fstrRoom, strName);
        }

        public int DeletebyGubunSName(string fstrRoom, string strSName)
        {
            return hicSangdamWaitRepository.DeletebyGubunSName(fstrRoom, strSName);
        }

        public int UpdateCallTimebyWrtNoGubun(long nWrtNo, string fstrWaitRoom)
        {
            return hicSangdamWaitRepository.UpdateCallTimebyWrtNoGubun(nWrtNo, fstrWaitRoom);
        }

        public int DeletebyEntTime(string strDate)
        {
            return hicSangdamWaitRepository.DeletebyEntTime(strDate);
        }

        public HIC_SANGDAM_WAIT Read_Exam_Wait(string argDate, string hCROOM)//, string strJepsuGubun)
        {
            return hicSangdamWaitRepository.Read_Exam_Wait(argDate, hCROOM);//, strJepsuGubun);
        }

        public List<HIC_SANGDAM_WAIT> Read_Now_Wait(string fstrRoom)
        {
            return hicSangdamWaitRepository.Read_Now_Wait(fstrRoom);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_Wait_RegList(long argWrtNo, string strPart)
        {
            return hicSangdamWaitRepository.Read_Sangdam_Wait_RegList(argWrtNo, strPart);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_EtcRoomReg(long argWrtNo, string strPart)
        {
            return hicSangdamWaitRepository.Read_Sangdam_EtcRoomReg(argWrtNo, strPart);
        }

        public int Delete_Sangdam_PreData(long argWrtNo, string strPart)
        {
            return hicSangdamWaitRepository.Delete_Sangdam_PreData(argWrtNo, strPart);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_View(long argWrtNo)
        {
            return hicSangdamWaitRepository.Read_Sangdam_View(argWrtNo);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_View2(string strPart)
        {
            return hicSangdamWaitRepository.Read_Sangdam_View2(strPart);
        }

        public int Insert_Sangdam_Wait3(HIC_SANGDAM_WAIT item)
        {
            return hicSangdamWaitRepository.Insert_Sangdam_Wait3(item);
        }

        public HIC_SANGDAM_WAIT Read_Sangdam_WrtNoCnt(string strRoom)
        {
            return hicSangdamWaitRepository.Read_Sangdam_WrtNoCnt(strRoom);
        }

        public List<HIC_SANGDAM_WAIT> Read_Sangdam_Wait_List(string strPart)
        {
            return hicSangdamWaitRepository.Read_Sangdam_Wait_List(strPart);
        }

        public List<HIC_SANGDAM_WAIT> Read_Sangdam_View3(string strPart)
        {
            return hicSangdamWaitRepository.Read_Sangdam_View3(strPart);
        }

        public List<HIC_SANGDAM_WAIT> Read_Sangdam_Wait_List(string strPart, string v)
        {
            return hicSangdamWaitRepository.Read_Sangdam_Wait_List(strPart);
        }

        public List<HIC_SANGDAM_NEW> GetOnlyNextRoomByWrtNo(long fnWRTNO)
        {
            return hicSangdamWaitRepository.GetOnlyNextRoomByWrtNo(fnWRTNO);
        }
    }
}
