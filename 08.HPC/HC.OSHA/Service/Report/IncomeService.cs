using HC.OSHA.Model;
using HC.OSHA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_OSHA.Service.Report
{
    public class IncomeService
    {
        private IncomeRepository incomeRepository;

        public IncomeService()
        {
            this.incomeRepository = new IncomeRepository();
        }

        public List<IncomeModel> FindAll(long siteId, string startDate, string endDate, bool isHistory)
        {
            //startDate += " 00:00:00";
            //endDate += " 23:59:59";
            long  sumPrice = 0;
            string created = null;
            IncomeModel sumModel = null;
            List<IncomeModel> list = incomeRepository.FindAll(siteId, startDate, endDate, isHistory);
            List<IncomeModel> sumList = new List<IncomeModel>(); //소계
            List<IncomeModel> resultList = new List<IncomeModel>();
            foreach (IncomeModel model in list)
            {
                if (created == null)
                {
                  
                    if (model.IsDeleted == "Y")
                    {
                        //model.TotalPrice =  -model.TotalPrice;
                        model.TotalPrice = 0;

                    }
                    else
                    {
                        sumPrice = model.TotalPrice;
                    }
                    created = model.Created;
                    
                }
                else
                {
                    if(created == model.Created)
                    {
                        if (model.IsDeleted == "Y")
                        {

                            // model.TotalPrice = -model.TotalPrice;
                            model.TotalPrice = 0;
                        }
                        else
                        {
                            sumPrice += model.TotalPrice;
                        }
                    
                    }
                    else
                    {
                        sumModel = new IncomeModel();
                        sumModel.Created = created;
                        sumModel.TotalPrice = sumPrice;
                        sumModel.SiteName = "** 소계 **";
                        sumModel.SiteId = "";

                        sumModel.danga = ConvertNumber(sumModel.UnitPrice);
                        sumModel.income = ConvertNumber(sumModel.TotalPrice);
                        sumModel.inwon = ConvertNumber(sumModel.WorkerCount);

                        resultList.Add(sumModel);
                        sumList.Clear();
                        created = model.Created;
                        sumPrice = model.TotalPrice;
                    }
                }

                model.danga = ConvertNumber(model.UnitPrice);
                model.income = ConvertNumber(model.TotalPrice);
                model.inwon = ConvertNumber(model.WorkerCount);
                resultList.Add(model);
                sumList.Add(model);
            }
            if(sumList.Count >= 1)
            {
                IncomeModel lastSumModel = new IncomeModel();
                foreach (IncomeModel model in sumList)
                {

                    lastSumModel.Created = model.Created;

                    //if (model.TotalPrice > 0)
                    //{
                    //    lastSumModel.TotalPrice += model.TotalPrice;
                    //}
                    if (model.IsDeleted =="N")
                    {
                        lastSumModel.TotalPrice += model.TotalPrice;
                    }
                    lastSumModel.SiteName = "** 소계 **";
                    lastSumModel.SiteId = "";
                  
                }
                lastSumModel.danga = ConvertNumber(lastSumModel.UnitPrice);
                lastSumModel.income = ConvertNumber(lastSumModel.TotalPrice);
                lastSumModel.inwon = ConvertNumber(lastSumModel.WorkerCount);
                resultList.Add(lastSumModel);

            }

            long sumTotalPrice = 0;
            foreach(IncomeModel model in resultList)
            {
                //if(model.SiteId == "")
                //{
                //    if (model.IsDeleted == "N")
                //    {
                //        sumTotalPrice += model.TotalPrice;
                //    }

                //}
                if (model.IsDeleted == "N")
                {
                    sumTotalPrice += model.TotalPrice;
                }
            }
            IncomeModel totalModel = new IncomeModel();
            totalModel.TotalPrice = sumTotalPrice;
            totalModel.income = ConvertNumber(sumTotalPrice);
            totalModel.SiteName = "** 합계 **";
            totalModel.SiteId = "";
            resultList.Add(totalModel);
            return resultList;
        }

        private string ConvertNumber(long number)
        {
            string tmp =    string.Format("{0:#,0}", number);
            if (tmp == "0")
            {
                return "0";
            }
            return tmp;
        }
    }
}
