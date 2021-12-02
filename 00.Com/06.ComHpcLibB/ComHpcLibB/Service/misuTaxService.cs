namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using ComBase;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class MisuTaxService
    {
        
        private MisuTaxRepository misuTaxRepository;

        public MisuTaxService()
        {
            this.misuTaxRepository = new MisuTaxRepository();
        }

        public List<MISU_TAX> GetNote(long nWrtno)
        {
            return misuTaxRepository.GetNote(nWrtno);
        }

        public int CreateViewBill(string dtpFDate, string dtpTDate, string strTaxNo, string strJong)
        {
            return misuTaxRepository.CreateViewBill(dtpFDate, dtpTDate, strTaxNo, strJong);
        }

        public List<MISU_TAX> GetViewItem(bool rdoSort1, bool rdoSort2)
        {
            return misuTaxRepository.GetViewItem(rdoSort1, rdoSort2);
        }

        public void DropView()
        {
            misuTaxRepository.DropView();
        }

        public int GetSALEEBILL(int val)
        {
            return misuTaxRepository.GetSALEEBILL(val);
        }

        public int UpdateMisuTax(MISU_TAX item)
        {
            return misuTaxRepository.UpdateMisuTax(item);
        }

        public List<MISU_TAX> GetWrtno(string strGjyear)
        {
            return misuTaxRepository.GetWrtno(strGjyear);
        }
        
        public int initBill(MISU_TAX initItem)
        {
            return misuTaxRepository.initBill(initItem);
        }
    }
}
