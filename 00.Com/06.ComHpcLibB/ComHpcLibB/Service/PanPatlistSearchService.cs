namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Model;
    
    
    /// <summary>
    /// 
    /// </summary>
    public class PanPatlistSearchService
    {
        
        private PanPatlistSearchRepository panPatlistSearchRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public PanPatlistSearchService()
        {
			this.panPatlistSearchRepository = new PanPatlistSearchRepository();
        }
    }
}
