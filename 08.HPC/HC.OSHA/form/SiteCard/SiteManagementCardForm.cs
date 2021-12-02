using ComBase;
using ComHpcLibB.Model;
using HC.Core.Common.Interface;
using HC.Core.Model;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{

    public partial class SiteManagementCardForm : CommonForm, ISelectSite, ISelectEstimate
    {
        
        private CommonForm activeForm = null;
        public SiteManagementCardForm()
        {
            InitializeComponent();
        }

        private void SiteManagementCardForm_Load(object sender, EventArgs e)
        {
            activeForm = new CardPage_1_Form();
            AddForm(activeForm, panFrame);

            lblSiteName.Text = "";
        }

        private void SSCardList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            //SSCardList.ActiveSheet.Cells[e.Row, 2]
            if(oshaSiteEstimateList1.GetEstimateModel == null)
            {

            }
            else
            {
                if (e.Row == 0)
                { 
                    activeForm = new CardPage_1_Form();
             
                }
                else if (e.Row == 1)
                {
                    activeForm = new CardPage_3_Form();

                }
                else if (e.Row == 2)
                {
                    activeForm = new CardPage_2_Form();
                    
                }
                else if (e.Row == 3)
                {
                    activeForm = new CardPage_4_Form();
                }
                else if (e.Row == 4)
                {
                    //재해자별현황
                    activeForm = new CardPage_5_Form();
                }
                else if (e.Row == 5)
                {
                    //안전보건관리규정 제개정내용
                    activeForm = new CardPage_6_Form();
                }
                else if (e.Row == 6)
                {
                    //위험성평가
                    activeForm = new CardPage_7_Form();
                }
                else if (e.Row == 7)
                {
                    //건강증진운동 / 양성교육
                    activeForm = new CardPage_8_Form();
                }

                else if (e.Row == 8)
                {
                    //안전검사
                    activeForm = new CardPage_9_Form();
                }
                else if (e.Row == 9)
                {
                    //보호구
                    activeForm = new CardPage_10_Form();
                }
                else if (e.Row == 10)
                {
                    //위험물질
                    activeForm = new CardPage_11_Form();
                }

                else if (e.Row == 11)
                {
                    //유해물질
                    activeForm = new CardPage_12_Form();
                }
                else if (e.Row == 12)
                {
                    //사업장 안전보건교육
                    activeForm = new CardPage_13_Form();
                }
                else if (e.Row == 13)
                {
                    //근로자 건강상담
                    activeForm = new CardPage_14_Form();
                }
                else if (e.Row == 14)
                {
                    //위탁업무
                    activeForm = new CardPage_15_Form();
                }
                else if (e.Row == 15)
                {
                    //위탁업무2페이지, 사업장 만족도
                    activeForm = new CardPage_16_Form();
                }
                else if (e.Row == 16)
                {
                    //응급의료체계
                    activeForm = new CardPage_17_Form();
                }

                AddForm(activeForm, panFrame);

                if(base.SelectedSite != null)
                {
                    if(activeForm is ISelectSite)
                    {
                        (activeForm as ISelectSite).Select(base.SelectedSite);
                    }
                }

                if (base.SelectedEstimate!= null)
                {
                    if(activeForm  is ISelectEstimate)
                    {
                        (activeForm as ISelectEstimate).Select(base.SelectedEstimate);
                    }
                }
            }
            
        }

        private void oshaSiteList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            base.SelectedSite = oshaSiteList1.GetSite;
            
            (activeForm as ISelectSite).Select(oshaSiteList1.GetSite);
            oshaSiteEstimateList1.SearhAndDoubleClik(oshaSiteList1.GetSite.ID, false);

            lblSiteName.Text = base.SelectedSite.NAME;
         
        }

        private void oshaSiteEstimateList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
           

                base.SelectedEstimate = oshaSiteEstimateList1.GetEstimateModel;

                (activeForm as ISelectEstimate).Select(oshaSiteEstimateList1.GetEstimateModel);

          
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null)
            {
                return;
            }

            List<CommonForm> list = new List<CommonForm>();
            for (int i = 0; i < SSCardList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(SSCardList.ActiveSheet.Cells[i, 0].Value) == true)
                {
                    if (i == 0)
                    {
                        list.Add(new CardPage_1_Form());
                    }
                    else if (i == 1)
                    {
                        list.Add(new CardPage_3_Form());
                    }
                    else if (i == 2)
                    {
                        list.Add(new CardPage_2_Form());
                    }
                    else if (i == 3)
                    {
                        list.Add(new CardPage_4_Form());
                    }
                    else if (i == 4)
                    {
                        list.Add(new CardPage_5_Form());
                    }
                    else if (i == 5)
                    {
                        list.Add(new CardPage_6_Form());
                    }
                    else if (i == 6)
                    {
                        list.Add(new CardPage_7_Form());
                    }
                    else if (i == 7)
                    {
                        list.Add(new CardPage_8_Form());
                    }
                    else if (i == 8)
                    {
                        list.Add(new CardPage_9_Form());
                    }
                    else if (i == 9)
                    {
                        list.Add(new CardPage_10_Form());
                    }
                    else if (i == 10)
                    {
                        list.Add(new CardPage_11_Form());
                    }
                    else if (i == 11)
                    {
                        list.Add(new CardPage_12_Form());
                    }
                    else if (i == 12)
                    {
                        list.Add(new CardPage_13_Form());
                    }
                    else if (i == 13)
                    {
                        list.Add(new CardPage_14_Form());
                    }
                    else if (i == 14)
                    {
                        list.Add(new CardPage_15_Form());
                    }
                    else if (i == 15)
                    {
                        list.Add(new CardPage_16_Form());
                    }
                    else if (i == 16)
                    {
                        list.Add(new CardPage_17_Form());
                    }
                }
            }

            while(list.Count > 0)
            {
                CommonForm form = list[list.Count - 1];
                form.SelectedSite = base.SelectedSite;
                form.SelectedEstimate = base.SelectedEstimate;

                bool ok = (form as IPrint).NewPrint();

                list.RemoveAt(list.Count - 1);

                DateTime dtm = DateTime.Now.AddMilliseconds(2000);

                while(dtm > DateTime.Now)
                {

                }
            }

            

            //foreach (CommonForm form in list.OrderBy(x => x.Name).ToList())
            //{
            //    Log.Debug(" 소트: " +  form.Name);
            //    form.SelectedSite = base.SelectedSite;
            //    form.SelectedEstimate = base.SelectedEstimate;
                
            //    (form as IPrint).Print();
            //}
        }

        private void SSCardList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader && e.Column == 0)
            {
           

                bool isCheck = Convert.ToBoolean(SSCardList.ActiveSheet.ColumnHeader.Cells[0, 0].Value);
                if (isCheck)
                {
                    SSCardList.ActiveSheet.ColumnHeader.Cells[0, 0].Value  = false;

                    for (int i = 0; i < SSCardList.ActiveSheet.RowCount; i++)
                    {
                        SSCardList.ActiveSheet.Cells[i, 0].Value = false;
                    }
                }
                else
                {
                    SSCardList.ActiveSheet.ColumnHeader.Cells[0, 0].Value = true;
                    for (int i = 0; i < SSCardList.ActiveSheet.RowCount; i++)
                    {
                        SSCardList.ActiveSheet.Cells[i, 0].Value = true;
                    }
                }
             
            }
        }

        public void Select(ISiteModel siteModel)
        {
            oshaSiteList1.SelectSite(siteModel);
        }

        public void Select(IEstimateModel estimateModel)
        {
        }
    }
}

