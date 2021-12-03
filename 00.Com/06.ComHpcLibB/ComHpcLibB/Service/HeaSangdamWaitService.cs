namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HeaSangdamWaitService
    {   
        private HeaSangdamWaitRepository heaSangdamWaitRepository;

        /// <summary>
        /// 
        /// </summary>
        public HeaSangdamWaitService()
        {
			this.heaSangdamWaitRepository = new HeaSangdamWaitRepository();
        }

        public Dictionary<string, object> Sangdam_Wait_Update(long WRTNO, string ROOMCODE)
        {
            return heaSangdamWaitRepository.Sangdam_Wait_Update(WRTNO, ROOMCODE);
        }

        //public int Update_Patient_Call(long WRTNO, string ROOMCODE)
        public int Update_Patient_Call(long WRTNO, List<string> ROOMCODE)
        {
            return heaSangdamWaitRepository.Update_Patient_Call(WRTNO, ROOMCODE);
        }
        
        public List<HEA_SANGDAM_WAIT> Read_Now_Wait(string ROOMCODE)
        {
            return heaSangdamWaitRepository.Read_Now_Wait(ROOMCODE);
        }
        
        public List<HEA_SANGDAM_WAIT> Etc_ExamRoom_RegConfirm(long WRTNO, string ROOMCODE)
        {
            return heaSangdamWaitRepository.Etc_ExamRoom_RegConfirm(WRTNO, ROOMCODE);
        }

        public int Delete_Sangdam_Wait(long WRTNO, string ROOMCODE)
        {
            return heaSangdamWaitRepository.Delete_Sangdam_Wait(WRTNO, ROOMCODE);
        }
        
        public List<HEA_SANGDAM_WAIT> Read_Sangdam_GbCall(long WRTNO, string ROOMCODE)
        {
            return heaSangdamWaitRepository.Read_Sangdam_GbCall(WRTNO, ROOMCODE);
        }

        public int Update_Sangdam_GbCall(long WRTNO, string[] strJongSQL)
        {
            return heaSangdamWaitRepository.Update_Sangdam_GbCall(WRTNO, strJongSQL);
        }

        public int Update_Sangdam_CallTime(long WRTNO, string ROOMCODE)
        {
            return heaSangdamWaitRepository.Update_Sangdam_CallTime(WRTNO, ROOMCODE);
        }

        public int Insert_Sangdam_Wait(HEA_SANGDAM_WAIT item)
        {
            return heaSangdamWaitRepository.Insert_Sangdam_Wait(item);
        }

        public int Insert_Sangdam_Wait2(HEA_SANGDAM_WAIT item)
        {
            return heaSangdamWaitRepository.Insert_Sangdam_Wait2(item);
        }

        public int Insert_Sangdam_Wait3(HEA_SANGDAM_WAIT item)
        {
            return heaSangdamWaitRepository.Insert_Sangdam_Wait3(item);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_WaitNo(string ROOMCODE)
        {
            return heaSangdamWaitRepository.Read_Sangdam_WaitNo(ROOMCODE);
        }

        public int GetRowIdbyWrtNo(long nWrtNo, string strFrDate, string strToDate, string[] strEndo_Room)
        {
            return heaSangdamWaitRepository.GetRowIdbyWrtNo(nWrtNo, strFrDate, strToDate, strEndo_Room);
        }

        public HEA_SANGDAM_WAIT GetGBCallbyWrtNo(long nWrtNo, string strFrDate, string strToDate, List<string> strEndo_Room)
        {
            return heaSangdamWaitRepository.GetGBCallbyWrtNo(nWrtNo, strFrDate, strToDate, strEndo_Room);
        }

        public List<HEA_SANGDAM_WAIT> Read_Sangdam_Wait_List(string strPart, string strGubun)
        {
            return heaSangdamWaitRepository.Read_Sangdam_Wait_List(strPart, strGubun);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_Wait_RegList(long nWrtNo, string strPart)
        {
            return heaSangdamWaitRepository.Read_Sangdam_Wait_RegList(nWrtNo, strPart);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_EtcRoomReg(long nWrtNo, string strPart)
        {
            return heaSangdamWaitRepository.Read_Sangdam_EtcRoomReg(nWrtNo, strPart);
        }

        public int Delete_Sangdam_PreData(long WRTNO, string ROOMCODE)
        {
            return heaSangdamWaitRepository.Delete_Sangdam_PreData(WRTNO, ROOMCODE);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_View(long nWrtNo)
        {
            return heaSangdamWaitRepository.Read_Sangdam_View(nWrtNo);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_View2(string strPart)
        {
            return heaSangdamWaitRepository.Read_Sangdam_View2(strPart);
        }

        public HEA_SANGDAM_WAIT Read_Sangdam_WrtNoCnt(string strPart)
        {
            return heaSangdamWaitRepository.Read_Sangdam_WrtNoCnt(strPart);
        }
                
        public List<HEA_SANGDAM_WAIT> Read_Sangdam_View3(string strRoomCd)
        {
            return heaSangdamWaitRepository.Read_Sangdam_View3(strRoomCd);
        }

        public HEA_SANGDAM_WAIT GetGbCallbyWrtNoEntTime(long nWrtNo, string strFrDate, string strToDate)
        {
            return heaSangdamWaitRepository.GetGbCallbyWrtNoEntTime(nWrtNo, strFrDate, strToDate);
        }

        public List<HEA_SANGDAM_WAIT> GetSangdamWaitInfobyEndoRoom(string eNDO_ROOM, string strSysDate)
        {
            return heaSangdamWaitRepository.GetSangdamWaitInfobyEndoRoom(eNDO_ROOM, strSysDate);
        }

        public long GetWaitNobyEndoRoom(string eNDO_ROOM, string sSysDate)
        {
            return heaSangdamWaitRepository.GetWaitNobyEndoRoom(eNDO_ROOM, sSysDate);
        }

        public int InsertSangdamWait(HEA_SANGDAM_WAIT item)
        {
            return heaSangdamWaitRepository.InsertSangdamWait(item);
        }

        public int InsertSangdamWait_GbEndo(HEA_SANGDAM_WAIT item)
        {
            return heaSangdamWaitRepository.InsertSangdamWait_GbEndo(item);
        }

        public int DeleteSangdamWait(long nWRTNO)
        {
            return heaSangdamWaitRepository.DeleteSangdamWait(nWRTNO);
        }

        public HEA_SANGDAM_WAIT GetItembyWrtNo(long nWRTNO)
        {
            return heaSangdamWaitRepository.GetItembyWrtNo(nWRTNO);
        }

        public int UpdateSangdamWaitNo(long nWait, long nWRTNO, string eNDO_ROOM)
        {
            return heaSangdamWaitRepository.UpdateSangdamWaitNo(nWait, nWRTNO, eNDO_ROOM);
        }

        public int UpdateWaitNo(long nWait, string strRowId)
        {
            return heaSangdamWaitRepository.UpdateWaitNo(nWait, strRowId);
        }

        public HEA_SANGDAM_WAIT GetCountWaitbyEndoRoom(string strPtNo, string eNDO_ROOM)
        {
            return heaSangdamWaitRepository.GetCountWaitbyEndoRoom(strPtNo, eNDO_ROOM);
        }

        public int UpdateSangdam_GbEndo(HEA_SANGDAM_WAIT item)
        {
            return heaSangdamWaitRepository.UpdateSangdam_GbEndo(item);
        }

        public List<HEA_SANGDAM_WAIT> GetItembyRoomCd(string eNDO_ROOM)
        {
            return heaSangdamWaitRepository.GetItembyRoomCd(eNDO_ROOM);
        }

        public int Update_CallTimeGbCall(string strRowId)
        {
            return heaSangdamWaitRepository.Update_CallTimeGbCall(strRowId);
        }

        public int GetRowIdbyEndoRoom(long fnWRTNO, string dENT_ROOM)
        {
            return heaSangdamWaitRepository.GetRowIdbyEndoRoom(fnWRTNO, dENT_ROOM);
        }

        public List<HEA_SANGDAM_WAIT> GetcountbyWrtNoRoom(long fnWRTNO, string dENT_ROOM)
        {
            return heaSangdamWaitRepository.GetcountbyWrtNoRoom(fnWRTNO, dENT_ROOM);
        }

        public string GetGBCallbyWrtNoGubun(long fnWRTNO, string strGubun)
        {
            return heaSangdamWaitRepository.GetGBCallbyWrtNoGubun(fnWRTNO, strGubun);
        }

        public HEA_SANGDAM_WAIT GetGBCallbyWrtNoInGubun(long nWRTNO, string[] strGubun)
        {
            return heaSangdamWaitRepository.GetGBCallbyWrtNoInGubun(nWRTNO, strGubun);
        }

        public List<HEA_SANGDAM_WAIT> GetWrtNobyWrtNo(long nWrtNo)
        {
            return heaSangdamWaitRepository.GetWrtNobyWrtNo(nWrtNo);
        }

        public int UpdateCallTimebyWrtNo(string strWRTNO)
        {
            return heaSangdamWaitRepository.UpdateCallTimebyWrtNo(strWRTNO);
        }

        public List<HEA_SANGDAM_WAIT> GetItembyGubunEntTime(string strGubun, string strFrDate, string strToDate)
        {
            return heaSangdamWaitRepository.GetItembyGubunEntTime(strGubun, strFrDate, strToDate);
        }

        public int UpdateGbCallbyWrtNoGubun(string strGbCall, long nWRTNO, string strGubun)
        {
            return heaSangdamWaitRepository.UpdateGbCallbyWrtNoGubun(strGbCall, nWRTNO, strGubun);
        }

        public int GetCountbyWrtNoGubun(long nWrtNo, List<string> strACT_SET_Data)
        {
            return heaSangdamWaitRepository.GetCountbyWrtNoGubun(nWrtNo, strACT_SET_Data);
        }

        public int GetCountbyEntTimeGubun(string strFrDate, string strToDate, string strRoom)
        {
            return heaSangdamWaitRepository.GetCountbyEntTimeGubun(strFrDate, strToDate, strRoom);
        }

        public HEA_SANGDAM_WAIT GetGbCallGbDentbyEntTimeGubun(string strFrDate, string strToDate, string strRoom, long nWrtNo)
        {
            return heaSangdamWaitRepository.GetGbCallGbDentbyEntTimeGubun(strFrDate, strToDate, strRoom, nWrtNo);
        }
    }
}
