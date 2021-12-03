using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using HC.Core.Service;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Model;
using HC.OSHA.Repository.StatusReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.OSHA.Service.StatusReport
{
    /// <summary>
    /// 근로자 건강상담 
    /// </summary>
    public class HealthCheckService
    {
        private HcSiteWorkerService hcSiteWorkerService;
        public HealthCheckRepository healthCheckRepository { get; }
        private HcCodeService codeService;
        private HicCodeService hicCodeService = null;
        private HicSpcPanjengService hicSpcPanjengService = null;
        private HicJepsuService hicJepsuService = null;
        private HicResSpecialService hicResSpecialService = null;
        private HicResBohum2Service hicResBohum2Service = null;

        clsHaBase hb = new clsHaBase();
        public HealthCheckService()
        {
            this.healthCheckRepository = new HealthCheckRepository();
            this.hcSiteWorkerService = new HcSiteWorkerService();
            this.hicCodeService = new HicCodeService();
            this.hicSpcPanjengService = new HicSpcPanjengService();
            this.hicJepsuService = new HicJepsuService();
            this.hicResSpecialService = new HicResSpecialService();

            this.hicResBohum2Service = new HicResBohum2Service();
            codeService = new HcCodeService();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        public void Save(HealthCheckDto dto)
        {
            //if(dto.worker_id <= 0)
            //{
            //    //hcSiteWorkerService.Save
            //}

            if (dto.id <= 0)
            {
                healthCheckRepository.Insert(dto);
            }
            else
            {
                healthCheckRepository.Update(dto);
            }


        }

        /// <summary>
        /// 근로자 상담시 근로자 목록
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="name"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public List<HealthCheckWorkerModel> FindAll(long siteId, string name, string dept, string panjeong, bool chkIsManageOsha, long reportId, bool isEnd)
        {
            if (dept == "전체")
            {
                dept = string.Empty;
            }
            if (panjeong == "전체")
            {
                panjeong = string.Empty;
            }
            string isManageOsha = "N";
            if (chkIsManageOsha)
            {
                isManageOsha = "Y";
            }

            string currentData = codeService.CurrentDate.ToString("yyyy-MM-dd");
            DateTime dtm = codeService.CurrentDate;

            List<HealthCheckWorkerModel> list = this.healthCheckRepository.FindWorker(siteId, name, dept, panjeong, isManageOsha, reportId, isEnd);
            foreach (HealthCheckWorkerModel worker in list)
            {
                string jumin = worker.Jumin;
                if(worker.PanName.NotEmpty())
                {
                    string[] split = worker.PanName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                    if (split.Length > 0)
                    {
                        worker.PanName = split[0];
                    }
                }
                
                if (jumin.NotEmpty())
                {
                    if (!worker.Jumin.IsNumeric())
                    {
                        jumin = clsAES.DeAES(worker.Jumin);
                    }

                    if (jumin.Length > 6)
                    {
                        if (jumin.Length == 7)
                        {
                            jumin += "000000";
                        }
                        worker.Jumin = jumin.Substring(0, 6) + "-" + jumin.Substring(6, 1);
                    }

                    worker.Age = ComFunc.AgeCalcEx(jumin, currentData);
                    worker.Gender = ComFunc.SexCheck(jumin, "2");
                    worker.AgeAndGender = worker.Gender + "(" + worker.Age + ")";

                }
                if (worker.IsSpecial == "Y")
                {
                    worker.IsSpecial = "특수";
                }
                else
                {
                    worker.IsSpecial = "일반";
                }
            }

            //List<HealthCheckWorkerModel> list = new List<HealthCheckWorkerModel>();
            //List<HealthCheckWorkerModel> temp = this.healthCheckRepository.FindNewWorker(siteId, name, dept, panjeong, isManageOsha, reportId, isEnd, dtm);
            //foreach (HealthCheckWorkerModel worker in temp)
            //{
            //    ListCheck(worker, list, currentData);
            //}

            return list;
        }

        public void ListCheck(HealthCheckWorkerModel worker, List<HealthCheckWorkerModel> list, string currentData)
        {
            #region 1차검진


            string jumin = worker.Jumin;
            if (jumin.NotEmpty())
            {
                if (!worker.Jumin.IsNumeric())
                {
                    jumin = clsAES.DeAES(worker.Jumin);
                }

                if (jumin.Length > 6)
                {
                    if (jumin.Length == 7)
                    {
                        jumin += "000000";
                    }
                    worker.Jumin = jumin.Substring(0, 6) + "-" + jumin.Substring(6, 1);
                }

                worker.Age = ComFunc.AgeCalcEx(jumin, currentData);
                worker.Gender = ComFunc.SexCheck(jumin, "2");
                worker.AgeAndGender = worker.Gender + "(" + worker.Age + ")";
            }

            if (worker.BROWID.Empty() && worker.PANDRNO == 0)
            {
                if(worker.WRTNO == 0)
                {
                    list.Add(worker);
                }
                
                return;
            }

            string d2Flag = string.Empty;
            if (worker.PANJENGD21.NotEmpty() || worker.PANJENGD22.NotEmpty() || worker.PANJENGD23.NotEmpty())
            {
                d2Flag = "OK";
            }

            //2010 생애부분추가
            //생애1차만 했을경우
            string flag = string.Empty;
            if (worker.BROWID.NotEmpty() && worker.CROWID.Empty() && worker.GJJONG.Equals("41"))
            {
                worker.Flag = "OK";
                worker.PanName = "2차생애미수검";
                worker.Panjeong = GetPangeng(worker);
                list.Add(new HealthCheckWorkerModel
                {
                    IsSpecial = "일반",
                    Worker_ID = worker.Worker_ID,
                    Name = worker.Name,
                    AgeAndGender = worker.AgeAndGender,
                    Dept = worker.Dept,
                    Year = worker.Year,
                    SITEID = worker.SITEID,
                    IsManageOsha = worker.IsManageOsha,
                    REMARK = worker.REMARK,
                    END_DATE = worker.END_DATE,
                    Pano = worker.Pano,
                    Age = worker.Age,
                    Gender = worker.Gender,
                    Jumin = worker.Jumin,
                    Panjeong = worker.Panjeong,
                    PanName = worker.PanName
                });
            }

            //정상소견
            if (worker.PANJENG.NotEmpty() && (worker.PANJENG.Equals("1") || worker.PANJENG.Equals("2")))
            {
                worker.PANJENG = "1";
                worker.PanName = "정상";
                worker.Panjeong = GetPangeng(worker);
                list.Add(new HealthCheckWorkerModel
                {
                    IsSpecial = "일반",
                    Worker_ID = worker.Worker_ID,
                    Name = worker.Name,
                    AgeAndGender = worker.AgeAndGender,
                    Dept = worker.Dept,
                    Year = worker.Year,
                    SITEID = worker.SITEID,
                    IsManageOsha = worker.IsManageOsha,
                    REMARK = worker.REMARK,
                    END_DATE = worker.END_DATE,
                    Pano = worker.Pano,
                    Age = worker.Age,
                    Gender = worker.Gender,
                    Jumin = worker.Jumin,
                    Panjeong = worker.Panjeong,
                    PanName = worker.PanName
                });
            }

            //2011-11-01 건진센터 한흥렬계장 요청사항(의뢰서)
            //D2 일반질병 판정이 있을경우 동일한 질환으로 질환의심R1 판정이 있어도 D2 질환만 표시함
            //질환의심(R1,R2)

            #region 질환의심(R1,R2)

            for (int i = 1; i < 13; i++)
            {
                if (worker.GetPropertieValue("PANJENGR" + i).To<string>("").Equals("1"))
                {
                    if (d2Flag.NotEmpty())
                    {
                        if (GenPanjengD2(i, worker) == "OK")
                        {
                            worker.SetPropertieValue("PANJENGR" + i, string.Empty);
                        }
                    }
                }

                if (worker.GetPropertieValue("PANJENGR" + i).To<string>("").Equals("1"))
                {
                    switch (i - 1)
                    {
                        case 0:
                            worker.PANJENG = "4";
                            worker.PanName = "폐결핵의심";
                            break;
                        case 1:
                            worker.PANJENG = "4";
                            worker.PanName = "기타흉부질환의심";
                            break;
                        case 2:
                            worker.PANJENG = "5";
                            worker.PanName = "고혈압";
                            break;
                        case 3:
                            worker.PANJENG = "4";
                            worker.PanName = "이상지질혈증의심";
                            break;
                        case 4:
                            worker.PANJENG = "4";
                            worker.PanName = "간장질환의심";
                            break;
                        case 5:
                            worker.PANJENG = "5";
                            worker.PanName = "당뇨병";
                            break;
                        case 6:
                            worker.PANJENG = "4";
                            worker.PanName = "신장질환의심";
                            break;
                        case 7:
                            worker.PANJENG = "4";
                            worker.PanName = "빈혈증의심";
                            break;
                        case 8:
                            worker.PANJENG = "4";
                            worker.PanName = "골다공증의심";
                            break;
                        case 9:
                            worker.PANJENG = "4";
                            worker.PanName = "기타질환의심";
                            break;
                        case 10:
                            worker.PANJENG = "4";
                            worker.PanName = "비만";
                            break;
                        case 11:
                            worker.PANJENG = "4";
                            worker.PanName = "난청";
                            break;
                        default:
                            break;
                    }

                    worker.Panjeong = GetPangeng(worker);
                    list.Add(new HealthCheckWorkerModel
                    {
                        IsSpecial = "일반",
                        Worker_ID = worker.Worker_ID,
                        Name = worker.Name,
                        AgeAndGender = worker.AgeAndGender,
                        Dept = worker.Dept,
                        Year = worker.Year,
                        SITEID = worker.SITEID,
                        IsManageOsha = worker.IsManageOsha,
                        REMARK = worker.REMARK,
                        END_DATE = worker.END_DATE,
                        Pano = worker.Pano,
                        Age = worker.Age,
                        Gender = worker.Gender,
                        Jumin = worker.Jumin,
                        Panjeong = worker.Panjeong,
                        PanName = worker.PanName
                    });
                }
            }

            #endregion

            #region 직업병(D1)

            for (int i = 11; i < 14; i++)
            {
                if (worker.GetPropertieValue("PANJENGD" + i).NotEmpty())
                {
                    worker.PanName = fn_Read_GenPanjengD1D2("31", worker.GetPropertieValue("PANJENGD" + i).ToString());
                    HIC_CODE codes = hicCodeService.GetItembyGubunCode2("31", worker.GetPropertieValue("PANJENGD" + i).ToString());
                    if (codes != null)
                    {
                        worker.PANJENG = "6";
                    }
                    worker.Panjeong = GetPangeng(worker);
                    list.Add(new HealthCheckWorkerModel
                    {
                        IsSpecial = "일반",
                        Worker_ID = worker.Worker_ID,
                        Name = worker.Name,
                        AgeAndGender = worker.AgeAndGender,
                        Dept = worker.Dept,
                        Year = worker.Year,
                        SITEID = worker.SITEID,
                        IsManageOsha = worker.IsManageOsha,
                        REMARK = worker.REMARK,
                        END_DATE = worker.END_DATE,
                        Pano = worker.Pano,
                        Age = worker.Age,
                        Gender = worker.Gender,
                        Jumin = worker.Jumin,
                        Panjeong = worker.Panjeong,
                        PanName = worker.PanName
                    });
                }
            }

            #endregion

            #region 일반질환(D2)

            for (int i = 21; i < 24; i++)
            {
                if (worker.GetPropertieValue("PANJENGD" + i).NotEmpty())
                {
                    worker.PanName = fn_Read_GenPanjengD1D2("33", worker.GetPropertieValue("PANJENGD" + i).ToString());
                    HIC_CODE codes = hicCodeService.GetItembyGubunCode2("33", worker.GetPropertieValue("PANJENGD" + i).ToString());
                    if (codes != null)
                    {
                        worker.PANJENG = "7";
                    }
                    worker.Panjeong = GetPangeng(worker);
                    list.Add(new HealthCheckWorkerModel
                    {
                        IsSpecial = "일반",
                        Worker_ID = worker.Worker_ID,
                        Name = worker.Name,
                        AgeAndGender = worker.AgeAndGender,
                        Dept = worker.Dept,
                        Year = worker.Year,
                        SITEID = worker.SITEID,
                        IsManageOsha = worker.IsManageOsha,
                        REMARK = worker.REMARK,
                        END_DATE = worker.END_DATE,
                        Pano = worker.Pano,
                        Age = worker.Age,
                        Gender = worker.Gender,
                        Jumin = worker.Jumin,
                        Panjeong = worker.Panjeong,
                        PanName = worker.PanName
                    });
                }
            }

            #endregion

            #region 유진환(D) 

            for (int i = 1; i < 5; i++)
            {
                if (worker.GetPropertieValue("PANJENGU" + i).To<string>("").Equals("1"))
                {
                    switch (i - 1)
                    {
                        case 0: worker.PanName = "유질환(고혈압)"; break;
                        case 1: worker.PanName = "유질환(당뇨병)"; break;
                        case 2: worker.PanName = "유질환(이상지질혈증)"; break;
                        case 3: worker.PanName = "유질환(폐결핵)"; break;
                    }

                    worker.PANJENG = "8";
                    worker.Panjeong = GetPangeng(worker);
                    list.Add(new HealthCheckWorkerModel
                    {
                        IsSpecial = "일반",
                        Worker_ID = worker.Worker_ID,
                        Name = worker.Name,
                        AgeAndGender = worker.AgeAndGender,
                        Dept = worker.Dept,
                        Year = worker.Year,
                        SITEID = worker.SITEID,
                        IsManageOsha = worker.IsManageOsha,
                        REMARK = worker.REMARK,
                        END_DATE = worker.END_DATE,
                        Pano = worker.Pano,
                        Age = worker.Age,
                        Gender = worker.Gender,
                        Jumin = worker.Jumin,
                        Panjeong = worker.Panjeong,
                        PanName = worker.PanName
                    });
                }
            }

            #endregion

            long Wrtno2 = 0;
            //2차의 접수번호를 찾음
            HIC_JEPSU wrt2 = hicJepsuService.GetWrtnoByPanoJepDateGjJong(worker.Pano.To<long>(0), worker.JEPDATE, worker.Year, "1");
            if (wrt2 != null)
            {
                Wrtno2 = wrt2.WRTNO;
            }

            if (worker.BROWID.NotEmpty() || worker.GJCHASU.To<string>("").Equals("2") || Wrtno2 > 0)
            {
                HIC_RES_SPECIAL item = hicResSpecialService.GetItemByWrtno(worker.WRTNO);
                //생물학적 노출지표(참고치)
                if (worker.UCODES.NotEmpty())
                {
                    string oldData = string.Empty;
                    //특수 1,2차 판정 결과를 읽음
                    List<HIC_SPC_PANJENG> list2 = hicSpcPanjengService.GeResulttItembyWrtNoWrtNo2(worker.WRTNO, Wrtno2, "");

                    var grp = list2.GroupBy(r => new { r.SOGENCODE, r.PANJENG, r.WORKYN })
                        .Select(n => new HIC_SPC_PANJENG
                        {
                            SOGENCODE = n.Key.SOGENCODE,
                            PANJENG = n.Key.PANJENG,
                            WORKYN = n.Key.WORKYN,
                            SOGENREMARK = n.Select(a => a.SOGENREMARK).FirstOrDefault()
                        });

                    foreach (HIC_SPC_PANJENG spc in grp)
                    {
                        HealthCheckWorkerModel newWorker = new HealthCheckWorkerModel
                        {
                            IsSpecial = "특수",
                            Worker_ID = worker.Worker_ID,
                            Name = worker.Name,
                            AgeAndGender = worker.AgeAndGender,
                            Dept = worker.Dept,
                            Year = worker.Year,
                            SITEID = worker.SITEID,
                            IsManageOsha = worker.IsManageOsha,
                            REMARK = worker.REMARK,
                            END_DATE = worker.END_DATE,
                            Pano = worker.Pano,
                            Age = worker.Age,
                            Gender = worker.Gender,
                            Jumin = worker.Jumin,
                            PanName = spc.SOGENREMARK
                        };
                        
                        switch (spc.PANJENG.To<string>("").Trim())
                        {
                            case "1":
                            case "2": newWorker.Panjeong = "A"; break;
                            case "3": newWorker.Panjeong = "C1"; break;
                            case "4": newWorker.Panjeong = "C2"; break;
                            case "5": newWorker.Panjeong = "D1"; break;
                            case "6": newWorker.Panjeong = "D2"; break;
                            case "7":
                            case "8": newWorker.Panjeong = "U"; break;
                            case "9": newWorker.Panjeong = "CN"; break;
                            case "A": newWorker.Panjeong = "DN"; break;
                        }

                        list.Add(newWorker);
                    }
                }
            }
            #endregion
        }

        public string GetPangeng(HealthCheckWorkerModel worker)
        {
            if(!worker.GJJONG.Equals("41"))
            {
                if(worker.PANJENG.Equals("1") || worker.PANJENG.Equals("2"))
                {
                    //worker.PanName = "필요없음";
                }
                else
                {
                    //worker.PanName = worker.SOGEN;
                }
            }
            if (worker.BROWID.NotEmpty() && worker.CROWID.Empty() && worker.GJJONG.To<string>("").Equals("41") && worker.Flag.Equals("OK"))
            {
                worker.PANJENG = "X";
                worker.PanName = "★2차생애상담필요합니다. ";
            }

            string panjeng = string.Empty;
            switch (worker.PANJENG)
            {
                case "1":
                case "2": panjeng = "A"; break;
                case "3":
                case "4": panjeng = "C"; break;
                case "5": panjeng = "확진검사대상"; break;
                case "6": panjeng = "D1"; break;
                case "7":
                case "8": panjeng = "D2"; break;
                case "X": panjeng = "생애2차상담"; break;
            }

            return panjeng;
        }


        /// <summary>
        /// READ_일반판정D1D2
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        string fn_Read_GenPanjengD1D2(string argGbn, string argCode)
        {
            string rtnVal = "";

            HIC_CODE list = hicCodeService.GetItembyGubunCode2(argGbn, argCode);

            if (!list.IsNullOrEmpty())
            {
                if (list.NAME.Trim() == list.NEWNAME.Trim())
                {
                    rtnVal = list.NEWNAME.Trim();
                }
                else
                {
                    rtnVal = list.NEWNAME.Trim() + "(" + list.NAME.Trim() + ")";
                }
            }

            return rtnVal;
        }

        private string GenPanjengD2(int gbn, HealthCheckWorkerModel item)
        {
            string strDPan = "";
            string rtnVal = "NO";

            strDPan = item.PANJENGD21 + "@" + item.PANJENGD22 + "@" + item.PANJENGD23;

            switch (gbn)
            {
                case 1:
                case 2:
                    rtnVal = VB.L(strDPan, "J") > 1 ? "OK" : "NO";
                    break;
                case 4:
                    rtnVal = VB.L(strDPan, "E") > 1 ? "OK" : "NO";
                    break;
                case 5:
                    rtnVal = VB.L(strDPan, "K") > 1 ? "OK" : "NO";
                    break;
                case 8:
                    rtnVal = VB.L(strDPan, "D") > 1 ? "OK" : "NO";
                    break;
                default:
                    break;
            }

            return rtnVal;
        }
    }
}
