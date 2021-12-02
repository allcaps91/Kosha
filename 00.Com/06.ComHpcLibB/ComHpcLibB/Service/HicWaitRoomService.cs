namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class HicWaitRoomService
    {
        
        private HicWaitRoomRepository hicWaitRoomRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicWaitRoomService()
        {
			this.hicWaitRoomRepository = new HicWaitRoomRepository();
        }

        public string GetRoomNamebyRoom(string strRoom)
        {
            return hicWaitRoomRepository.GetRoomNamebyRoom(strRoom);
        }
        public HIC_WAIT_ROOM GetItemByRoom(string strRoom)
        {
            return hicWaitRoomRepository.GetItemByRoom(strRoom);
        }

        public List<HIC_WAIT_ROOM> GetRoombyRoomCode(List<string> strTemp, string strSysTime)
        {
            return hicWaitRoomRepository.GetRoombyRoomCode(strTemp, strSysTime);
        }

        public List<HIC_WAIT_ROOM> GetAll()
        {
            return hicWaitRoomRepository.GetAll();
        }

        public string GetCountbyRoomCd(string strRoom)
        {
            return hicWaitRoomRepository.GetCountbyRoomCd(strRoom);
        }

        public int InsertAll(HIC_WAIT_ROOM item)
        {
            return hicWaitRoomRepository.InsertAll(item);
        }

        public int Update(HIC_WAIT_ROOM item)
        {
            return hicWaitRoomRepository.Update(item);
        }

        public int UpdatebyRoom(HIC_WAIT_ROOM item)
        {
            return hicWaitRoomRepository.UpdatebyRoom(item);
        }

        public HIC_WAIT_ROOM GetRoomRoomNamebyRoom(string strNextRoom)
        {
            return hicWaitRoomRepository.GetRoomRoomNamebyRoom(strNextRoom);
        }

        public string GetRoomRoomNameByRoomCd(string strCODE)
        {
            return hicWaitRoomRepository.GetRoomRoomNameByRoomCd(strCODE);
        }
    }
}
