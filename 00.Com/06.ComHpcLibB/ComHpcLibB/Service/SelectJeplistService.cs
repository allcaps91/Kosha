namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    
    /// <summary>
    /// 
    /// </summary>
    public class SelectJeplistService
    {   
        private SelectJeplistRepository selectJeplistRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public SelectJeplistService()
        {
			this.selectJeplistRepository = new SelectJeplistRepository();
        }

        public List<Select_JepList> Read_Sangdam_View4(string SDate, string HeaPart)
        {
            return selectJeplistRepository.Read_Sangdam_View4(SDate, HeaPart);
        }

        public List<Select_JepList> Read_Sangdam_View4_Hic(string SDate, string HeaPart)
        {
            return selectJeplistRepository.Read_Sangdam_View4_Hic(SDate, HeaPart);
        }

    }
}
