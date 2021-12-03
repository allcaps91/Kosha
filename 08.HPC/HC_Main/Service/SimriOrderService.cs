namespace HC_Main.Service
{
    using System.Collections.Generic;
    using HC_Main.Repository;
    using HC_Main.Dto;
    using System;
    using ComHpcLibB.Dto;


    /// <summary>
    /// 
    /// </summary>
    public class SimriOrderService
    {
        
        private SimriOrderRepository simriOrderRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public SimriOrderService()
        {
			this.simriOrderRepository = new SimriOrderRepository();
        }

        public string GetRowidByPanoBDate(string argPtno, string argBDate, double nQty)
        {
            return simriOrderRepository.GetRowidByPanoBDate(argPtno, argBDate, nQty);
        }

        public void UpDateQtyByRowid(double nQty, string argRowid)
        {
            simriOrderRepository.UpDateQtyByRowid(nQty, argRowid);
        }

        public void InsertDataFromHic(HIC_JEPSU item, double nQty)
        {
            simriOrderRepository.InsertDataFromHic(item, nQty);
        }

        public void DeleteByRowid(string argRowid)
        {
            simriOrderRepository.DeleteByRowid(argRowid);
        }
    }
}
