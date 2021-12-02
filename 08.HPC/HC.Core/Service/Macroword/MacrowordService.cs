using HC.Core.Dto;
using HC.Core.Repository;
using System.Collections.Generic;

namespace HC.Core.Service
{
    public class MacrowordService
    {
        public MacrowordRepository MacrowordRepository { get; }
        private DataSyncRepository dataSyncRepository;
        public MacrowordService()
        {
            this.MacrowordRepository = new MacrowordRepository();
        }

        public MacrowordDto Save(MacrowordDto dto)
        {
            if (dto.ID > 0)
            {
             
                return this.MacrowordRepository.Update(dto);
            }
            else
            {
                return this.MacrowordRepository.Insert(dto);
            }
        }

        public List<MacrowordDto> FindAll(string formName, string controlId)
        {
            return this.MacrowordRepository.FindAll(formName, controlId);
        }


        public List<MacrowordDto> FindAll(string formName, string controlId, string title)
        {
            return this.MacrowordRepository.FindAll(formName, controlId, title);
        }
    }
}
