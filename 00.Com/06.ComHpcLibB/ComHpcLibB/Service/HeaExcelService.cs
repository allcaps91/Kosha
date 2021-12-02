namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase.Mvc;
    using ComBase;


    /// <summary>
    /// 
    /// </summary>
    public class HeaExcelService
    {
        
        private HeaExcelRepository heaExcelRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HeaExcelService()
        {
			this.heaExcelRepository = new HeaExcelRepository();
        }

        public int Update(HEA_EXCEL item)
        {
            return heaExcelRepository.Update(item);
        }

        public List<HEA_EXCEL> GetAll(string strYear, long nLtd, string strSname, string strBirth, string strNoRsv)
        {
            return heaExcelRepository.GetAll(strYear, nLtd, strSname, strBirth, strNoRsv);
        }

        public List<HEA_EXCEL> GetRowIdbyLtdCode(string strYear, string strLtdCode)
        {
            return heaExcelRepository.GetRowIdbyLtdCode(strYear, strLtdCode);
        }

        public HEA_EXCEL GetAllbyRowId(string strROWID)
        {
            return heaExcelRepository.GetAllbyRowId(strROWID);
        }

        public int Update_RDate(HEA_EXCEL item)
        {
            return heaExcelRepository.Update_RDate(item);
        }

        public string GetMemobyRowId(string strROWID)
        {
            return heaExcelRepository.GetMemobyRowId(strROWID);
        }

        public List<HEA_EXCEL> GetListByYear(string argYYMM)
        {
            return heaExcelRepository.GetListByYear(argYYMM);
        }

        public bool Save(HEA_EXCEL item)
        {
            //엑셀저장은 신규등록만 가능함
            try
            {
                if (item.RowStatus == RowStatus.None || item.RowStatus == RowStatus.Insert)
                {
                    //신규저장
                    heaExcelRepository.Insert(item);
                }
                else
                {
                    if (item.RowStatus == RowStatus.Delete)
                    {
                        //Data 삭제
                        heaExcelRepository.Delete(item);
                    }
                    else if (item.RowStatus == RowStatus.Update)
                    {
                        //Data UpDate
                        heaExcelRepository.UpDate(item);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public string GetRowidbyItem(HEA_EXCEL item)
        {
            return heaExcelRepository.GetRowidbyItem(item);
        }

        public HEA_EXCEL GetItemByJuminYear(string argJumin, string argYear)
        {
            return heaExcelRepository.GetItemByJuminYear(argJumin, argYear);
        }

        public HEA_EXCEL GetItemByLtdsabunYear(string argLtdsabun, string argYear)
        {
            return heaExcelRepository.GetItemByLtdsabunYear(argLtdsabun, argYear);
        }

        public bool UpdateExcel(HEA_EXCEL item)
        {
            try
            {
                heaExcelRepository.UpdateExcel(item);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}
