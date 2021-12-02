namespace HC_Main.Service
{
    using System.Collections.Generic;
    using HC_Main.Repository;
    using HC_Main.Dto;
    using System;
    using ComBase;
    using System.Windows.Forms;


    /// <summary>
    /// 
    /// </summary>
    public class HeaTicketService
    {
        
        private HeaTicketRepository heaTicketRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaTicketService()
        {
			this.heaTicketRepository = new HeaTicketRepository();
        }

        public HEA_TICKET GetItemByTicketNo(long argTicketNo)
        {
            return heaTicketRepository.GetItemByTicketNo(argTicketNo);
        }

        public bool UpDate(HEA_TICKET iHaTCK)
        {
            try
            {
                heaTicketRepository.UpDate(iHaTCK);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
