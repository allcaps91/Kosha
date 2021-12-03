namespace ComHpcLibB.Service
{
    using System.Collections.Generic;
    using ComHpcLibB.Repository;
    using ComHpcLibB.Dto;
    using System;
    using ComBase;
    using System.Windows.Forms;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuWorkService
    {
        
        private HicJepsuWorkRepository hicJepsuWorkRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuWorkService()
        {
			this.hicJepsuWorkRepository = new HicJepsuWorkRepository();
        }

        public IList<HIC_JEPSU_WORK> GetListByItem(string argYear, string argFDate, string argTDate, string argGbChul, string argGjJong, long argLtdCode)
        {
            return hicJepsuWorkRepository.GetListByItem(argYear,  argFDate, argTDate, argGbChul, argGjJong, argLtdCode);
        }

        public bool Save(IList<HIC_JEPSU_WORK> list)
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                foreach (HIC_JEPSU_WORK code in list)
                {
                    if (code.RowStatus == ComBase.Mvc.RowStatus.Insert)
                    {
                        hicJepsuWorkRepository.Insert(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Update)
                    {
                        hicJepsuWorkRepository.Update(code);
                    }
                    else if (code.RowStatus == ComBase.Mvc.RowStatus.Delete)
                    {
                        hicJepsuWorkRepository.Delete_JepsuWork(code);
                        hicJepsuWorkRepository.Delete_SunapWork(code);
                        hicJepsuWorkRepository.Delete_SunapDtlWork(code);
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }
        }

        public List<HIC_JEPSU_WORK> GetItembyJuMin(string strJUMIN, string strJOBDATE)
        {
            return hicJepsuWorkRepository.GetItembyJuMin(strJUMIN, strJOBDATE);
        }

        public List<HIC_JEPSU_WORK> GetItemByPtnoYear(string argPtno, string argYear)
        {
            return hicJepsuWorkRepository.GetItemByPtnoYear(argPtno, argYear);
        }

        public HIC_JEPSU_WORK GetItemByRid(string argRid)
        {
            return hicJepsuWorkRepository.GetItemByRid(argRid);
        }

        public int UpdatePaNobyPaNo(string argPaNo, string argJumin2)
        {
            return hicJepsuWorkRepository.UpdatePaNobyPaNo(argPaNo, argJumin2);
        }

        public int InsertAll(HIC_JEPSU_WORK item)
        {
            return hicJepsuWorkRepository.InsertAll(item);
        }

        public int UpdateAllbyRowId(HIC_JEPSU_WORK item)
        {
            return hicJepsuWorkRepository.UpdateAllbyRowId(item);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDateGjJongLtdCode(string strFrDate, string strToDate, string strGjJong, string strLtdCode)
        {
            return hicJepsuWorkRepository.GetItembyJepDateGjJongLtdCode(strFrDate, strToDate, strGjJong, strLtdCode);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDate(string strFDate, string strTDate)
        {
            return hicJepsuWorkRepository.GetItembyJepDate(strFDate, strTDate);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDateGjJong(string strFDate, string strTDate, List<string> strjong)
        {
            return hicJepsuWorkRepository.GetItembyJepDateGjJong(strFDate, strTDate, strjong);
        }

        public string GetHphonebyPtNoGjJong(string argPtNo, string argGjJong)
        {
            return hicJepsuWorkRepository.GetHphonebyPtNoGjJong(argPtNo, argGjJong);
        }

        public List<HIC_JEPSU_WORK> GetItembyJepDateGjJongLtdCode2(string strFrDate, string strToDate, long nLtdCode)
        {
            return hicJepsuWorkRepository.GetItembyJepDateGjJongLtdCode2(strFrDate, strToDate, nLtdCode);
        }

        public int GetCountbyPaNoSuDate(string strPANO, string strJepDate)
        {
            return hicJepsuWorkRepository.GetCountbyPaNoSuDate(strPANO, strJepDate);
        }

        public int UpdateXrayNo(string strXrayno, string strFDate, string strTDate, string strLtdCode, string strGjYear, long nPano)
        {
            return hicJepsuWorkRepository.UpdateXrayNo(strXrayno, strFDate, strTDate, strLtdCode, strGjYear, nPano);
        }

        public string GetRowidByGJjongPtnoYear(string gJJONG, string pTNO, string gJYEAR)
        {
            return hicJepsuWorkRepository.GetRowidByGJjongPtnoYear(gJJONG, pTNO, gJYEAR);
        }

        public bool DeleteByRowid(string argRowid)
        {
            try
            {
                hicJepsuWorkRepository.DeleteByRowid(argRowid);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            
        }

        public HIC_JEPSU_WORK GetSecondJongItemByPanoGjYear(long nPano, string strYear, string argJob)
        {
            return hicJepsuWorkRepository.GetSecondJongItemByPanoGjYear(nPano, strYear, argJob);
        }

        public List<HIC_JEPSU_WORK> GetListItemByPanoGjYear(long nPano, string strYear, string argJob)
        {
            return hicJepsuWorkRepository.GetListItemByPanoGjYear(nPano, strYear, argJob);
        }

        public List<HIC_JEPSU_WORK> GetListGjNameByPtnoJepDate(string argYear, string argPtno)
        {
            return hicJepsuWorkRepository.GetListGjNameByPtnoJepDate(argYear, argPtno);
        }
    }
}
