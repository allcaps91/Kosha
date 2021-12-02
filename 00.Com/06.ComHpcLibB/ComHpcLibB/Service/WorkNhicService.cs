namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;


    /// <summary>
    /// 
    /// </summary>
    public class WorkNhicService
    {
        
        private WorkNhicRepository workNhicRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public WorkNhicService()
        {
			this.workNhicRepository = new WorkNhicRepository();
        }

        public WORK_NHIC GetOneItemByNewData(string argGbn)
        {
            return workNhicRepository.GetOneItemByNewData(argGbn);
        }

        public WORK_NHIC GetNhicInfo(string argGbn, string argJuminNo, string argSName, string argYear, string argGbSTS)
        {
            return workNhicRepository.GetNhicInfo(argGbn, argJuminNo, argSName, argYear, argGbSTS);
        }

        public WORK_NHIC GetNhicInfoByTime(string argGbn, string argJuminNo, string argSName, string argYear, string argGbSTS, string argTimeGb)
        {
            return workNhicRepository.GetNhicInfoByTime(argGbn, argJuminNo, argSName, argYear, argGbSTS, argTimeGb);
        }

        public int InsertData(WORK_NHIC item)
        {
            return workNhicRepository.InsertData(item);
        }

        public int DeleteDataAllByJuminNo(string argJumin)
        {
            return workNhicRepository.DeleteDataAllByJuminNo(argJumin);
        }

        public void UpDateError(string argRid)
        {
            workNhicRepository.UpDateError(argRid);
        }

        public void UpdateNewTarget(string argRid)
        {
            workNhicRepository.UpdateNewTarget(argRid);
        }

        public WORK_NHIC GetItembyJuminSName(string argJumin, string argSname)
        {
            return workNhicRepository.GetItembyJuminSName(argJumin, argSname);
        }

        public WORK_NHIC GetSNamebyJumin2Year(string argJumin, string argYear)
        {
            return workNhicRepository.GetSNamebyJumin2Year(argJumin, argYear);
        }

        public WORK_NHIC GetSNamebyJumin2(string argJumin, string argYear)
        {
            return workNhicRepository.GetSNamebyJumin2(argJumin, argYear);
        }

        public WORK_NHIC GetItembyJumin2(string argJumin2)
        {
            return workNhicRepository.GetItembyJumin2(argJumin2);
        }

        public List<WORK_NHIC> GetItembyJumin2SName(string strJumin, string strSName, string strLifeGubun)
        {
            return workNhicRepository.GetItembyJumin2SName(strJumin, strSName, strLifeGubun);
        }

        public WORK_NHIC GetNhicInfo_Am(string argGbn, string argJuminNo, string argSName, string argYear, string argGbSTS, string argLife)
        {
            return workNhicRepository.GetNhicInfo_Am(argGbn, argJuminNo, argSName, argYear, argGbSTS, argLife);
        }

        public string GetRowidByJumin2Rtime(string argJumin)
        {
            return workNhicRepository.GetRowidByJumin2Rtime(argJumin);
        }
    }
}
