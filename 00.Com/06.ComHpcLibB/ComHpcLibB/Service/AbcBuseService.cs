namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using ComHpcLibB.Model;
    using ComHpcLibB.Repository;


    /// <summary>
    /// 
    /// </summary>
    public class AbcBuseService
    {
        
        private AbcBuseRepository abcBuseRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public AbcBuseService()
        {
			this.abcBuseRepository = new AbcBuseRepository();
        }

        public List<ABC_BUSE> BuseList()
        {
            return abcBuseRepository.BuseList();
        }
    }
}
