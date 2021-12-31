namespace HC.OSHA.Service
{
    using System.Collections.Generic;
    using HC.OSHA.Repository;
    using HC.OSHA.Model;
    using System.Drawing;
    using ComBase.Controls;
    using HC.Core.BaseCode.MSDS.Service;
    using HC.Core.Model;
    using HC.Core.Service;
    using ComBase;
    using ComHpcLibB.Dto;
    using System;
    using ComHpcLibB;
    using ComBase.Mvc.Utils;


    /// <summary>
    /// 
    /// </summary>
    public class BrainRiskEvalutionService
    {
        private HealthCareService healthCareService;

        public BrainRiskEvalutionService()
        {
            this.healthCareService = new HealthCareService();
        }
     
         
        public List<BranRiskEvalutionModel> FindData(string siteName, long siteId, string year, string startDate, string endDate, bool isMore)
        {
            try
            {
                List<BranRiskEvalutionModel> list = new List<BranRiskEvalutionModel>();

                List<HealthCarePatientModel> patientList = healthCareService.HealthCarePatientRepository.FindAllComplete(siteId, year, startDate, endDate);

                foreach (HealthCarePatientModel patient in patientList)
                {
                    list.Add(GetBrain(patient, siteName, false));
                }

                if (isMore)
                {
                    List<HealthCarePatientModel> patientTotalList = healthCareService.HealthCarePatientRepository.FindAllToalComplete(siteId, startDate, endDate);
                    foreach (HealthCarePatientModel patient in patientTotalList)
                    {
                        bool isSame = false;
                        foreach (BranRiskEvalutionModel model in list)
                        {
                            if (model.COL51 == clsAES.DeAES(patient.JUMIN2))
                            {
                                isSame = true;
                                break;
                            }
                        }
                        if (isSame == false)
                        {
                            list.Add(GetBrain(patient, siteName, true));
                        }
                    }
                }
                return list;
            }
            catch(Exception ex)
            {
                MessageUtil.Alert(ex.Message);
                Log.Error(ex);
            }
            return new List<BranRiskEvalutionModel>(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="siteName"></param>
        /// <param name="isTotal">종합검진여부</param>
        /// <returns></returns>
        private BranRiskEvalutionModel GetBrain(HealthCarePatientModel patient, string siteName, bool isTotal)
        {
            BranRiskEvalutionModel model = new BranRiskEvalutionModel();

            string jumin = clsAES.DeAES(patient.JUMIN2);
            model.COL1 = siteName;

            model.COL2 = patient.SNAME;
            model.COL3 = jumin.Substring(0, 6) + "-" + jumin.Substring(7, 1);
            model.COL4 = patient.JEPDATE;
            model.COL5 = " ABC 병원 ";
            model.COL6 = patient.AGE.ToString();
            model.COL7 = patient.SEX;
            model.COL56 = jumin;

            ExResultModel exResult = null;

            if (isTotal)
            {
                exResult = healthCareService.GetTotalExResult(patient.WRTNO);
            }
            else
            {
                exResult = healthCareService.GetExResult(patient.WRTNO);
            }
         
            model.COL8 = exResult.HEIGHT;
            model.COL9 = exResult.WEIGHT;
            model.COL10 = exResult.BMI;
            model.COL11 = exResult.WAITST;
            model.COL12 = exResult.SBP;
            model.COL13 = exResult.DBP;
            model.COL14 = exResult.BST;
            model.COL15 = exResult.HBA1C;
            model.COL16 = exResult.TDL;
            model.COL17 = exResult.HDL;
            model.COL18 = exResult.LDL;
            model.COL19 = exResult.TRI;
            model.COL20 = exResult.PROTEIN; // 단백뇨
            model.COL21 = exResult.CREATININE;
            model.COL22 = exResult.GFR;
            model.COL23 = exResult.CT;

            if (isTotal)
            {
                decimal tmp = exResult.HEIGHT.To<decimal>(0) / 100;
                model.COL10 = Math.Round((exResult.WEIGHT.To<decimal>(0) / tmp / tmp),1).ToString(); //bmi

                if(exResult.TE15 != "")
                {
                    model.COL25 = exResult.TE15;
                }
            }


            HIC_RES_BOHUM1 exq = healthCareService.GetFirstExaminationQuestionnaire(patient.WRTNO);
            if(exq == null)
            {

            }
            if (patient.SEX == "M" && patient.AGE >= 45)
            {
                model.COL30 = "1";
            }//29 연령
            else if (patient.SEX == "F" && patient.AGE >= 55)
            {
                model.COL30 = "1";
            }

            if (exResult.BST.To<int>(0) >= 126 || exResult.HBA1C.To<double>(0) >= 6.5)
            {
                model.COL31 = "1";
            }

            if (exResult.BMI.To<decimal>(0) > 25)
            {
                model.COL32 = "1";
            }

            if (isTotal)
            {
                //비만허리둘레
                if(model.COL10.To<double>(0) >= 25)
                {
                    model.COL32 = "1";
                }
            }

            if (patient.SEX == "M" && exResult.WAITST.To<decimal>(0) >= 90)
            {
                model.COL32 = "1";
            }
            else if (patient.SEX == "F" && exResult.WAITST.To<decimal>(0) >= 85)
            {
                model.COL32 = "1";
            }


            if (exResult.HDL.To<int>(0) >= 60)
            {
                model.COL33 = "-1";
            }
            if (exResult.HDL.To<int>(0) != 0 && exResult.HDL.To<int>(0) < 40)
            {
                model.COL34 = "1";
            }

            if (exResult.TDL.To<int>(0) >= 220 || exResult.LDL.To<int>(0) >= 150 || exResult.TRI.To<int>(0) >= 200)
            {
                model.COL34 = "1";
            }

            //단백뇨 및 혈청 크레아티닌, 혈청 크레아티닌, 사구체여과율(GFR), 좌심실비대, 발목위팔혈압지수, 맥파전달속도, 고혈압성망막
            if (exResult.PROTEIN_RESULT != null)
            {
                if (exResult.PROTEIN_RESULT.Substring(0, 2) == "03" || exResult.PROTEIN_RESULT.Substring(0, 2) == "04" || exResult.PROTEIN_RESULT.Substring(0, 2) == "05" || exResult.PROTEIN_RESULT.Substring(0, 2) == "06")
                {
                    model.COL35 = "1";
                }

            }

            if (exResult.CREATININE.To<double>(0) >= 1.5 || (exResult.GFR.To<int>(0) != 0 && exResult.GFR.To<int>(0) <= 60))
            {
                //SSList.ActiveSheet.Cells[row, 34].Value = "3";
                model.COL35 = "3";
            }
            if (exq != null)
            {
                //흡연
                if (exq.T_SMOKE1.To<string>("") == "1")
                {
                    model.COL36 = "1";
                }
                else
                {
                    model.COL36 = "";
                }
            }
     

            //심혈관질환 조기발병 가족력
            if (patient.SEX == "M" && patient.AGE >= 55)
            {
                model.COL37 = "1";
            }
            else if (patient.SEX == "F" && patient.AGE >= 65)
            {
                model.COL37 = "1";
            }
            if (exq != null)
            {
                if (exq.T_STAT32 == "1")
                {
                    model.COL42 = "O";
                }
                //뇌졸증
                if (exq.T_STAT01 == "1" || exq.T_STAT02 == "1")
                {
                    //SSList.ActiveSheet.Cells[row, 42].Value = "O";
                    model.COL43 = "O";
                }

                if (exq.T_STAT11 == "1" || exq.T_STAT12 == "1")
                {
                    //SSList.ActiveSheet.Cells[row, 43].Value = "O";
                    model.COL44 = "O";
                }

            }






            if (patient.SNAME == "강선유")
            {
                string xx = "";
            }
            string col51 = string.Empty;
            if (exq != null) {
                //검진결과평가//51번 strResult2  
                decimal sim_result2 = exq.SIM_RESULT2.To<decimal>(0);
                if (!isTotal)
                {
                    model.COL48 = sim_result2.ToString();//10년이내 뇌심혈관 발생 확률
                    model.COL49 = exq.SIM_RESULT3;//심혈관나이
                    model.COL50 = exq.SIM_RESULT1; //심뇌혈관발생위험
                }

            
                if (sim_result2 < 1)
                {
                    model.COL51 = "저위험";
                }
                else if (sim_result2 < 5)
                {
                    model.COL51 = "중등도위험";
                }
                else if (sim_result2 < 10)
                {
                    model.COL51 = "고위험";
                }
                else if (sim_result2 >= 10)
                {
                    model.COL51 = "최고위험";
                }
                col51 = model.COL51;
                if (sim_result2 == 0)
                {
                    if (!isTotal)
                    {
                        model.COL48 = "";
                        model.COL51 = "";
                    }

                }
            }
   
          


                //1단계(52) = strResult
             model.COL52 = GetFirstStep(exResult.SBP.To<int>(0), exResult.DBP.To<int>(0));


            //2단계 발병위험인자 개수
            int sum = model.COL30.To<int>(0) + model.COL31.To<int>(0) + model.COL32.To<int>(0) + model.COL33.To<int>(0) + model.COL34.To<int>(0) + model.COL35.To<int>(0) + model.COL36.To<int>(0) + model.COL37.To<int>(0) + model.COL38.To<int>(0) + model.COL39.To<int>(0);
            model.COL53 = sum.ToString();
            model.COL54 = GetDiseasePan(model.COL52, sum, exResult, exq, model.COL55);


            if (col51 == "최고위험" || model.COL54 == "최고위험")
            {
                model.COL58 = "최고위험";
            }
            else if (col51 == "고위험" || model.COL54 == "고위험")
            {
                model.COL58 = "고위험";
            }
            else if (col51 == "중등도위험" || model.COL54 == "중등도위험")
            {
                model.COL58 = "중등도위험";
            }
            else if (col51 == "저위험" || model.COL54 == "저위험")
            {
                model.COL58 = "저위험";
            }
            else
            {
                model.COL58 = "정상";
            }

            if (model.COL55 == "정상")
            {
                model.COL55 = "통상근무";
            }
            else
            {
                model.COL55 = "조건부근무";
            }

            if (exq != null)
            {
                if (!isTotal)
                {
                    if (exq.SOGEN != null)
                    {
                        model.COL56 = exq.SOGEN.Replace(":2차검사요", "");
                    }
                }
                else
                {
                    //20200403 검진 오류나서 더이상 진행 못함
                    //clsHcMain hcMain = new clsHcMain();
                    //hcMain.Auto_NewPanjeng_First(patient.WRTNO, "", "", "Y");

                    //string xx = clsHcType.HFA.Sogen;
                }

            }



            return model;
        }
        /// <summary>
        /// 발병위험도 평가
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetDiseasePan(string firstStep, int sum, ExResultModel exResult, HIC_RES_BOHUM1 exq, string COL55)
        {
            string DiseasePan = string.Empty;
            if (firstStep == "1기 고혈압")
            {
                if (sum == 0)
                {
                    DiseasePan = "저위험";
                }
                else if (sum <= 1)
                {
                    DiseasePan = "중등도위험";
                }
                else
                {
                    DiseasePan = "고위험";
                }
            }
            else if (firstStep == "2기 고혈압")
            {
                if (sum <= 2)
                {
                    DiseasePan = "중등도위험";
                }
                else
                {
                    DiseasePan = "고위험";
                }
            }
            else if (firstStep == "3기 고혈압")
            {
                DiseasePan = "고위험";
            }
            else
            {
                if (sum <= 0)
                {
                    DiseasePan = "정상";
                }
                else if (sum <= 2)
                {
                    DiseasePan = "저위험";
                }
                else {
                    DiseasePan = "중등도위험";
                }
            }
            if (exResult != null)
            {
                if (exResult.PROTEIN_RESULT != null)
                {
                    if (exq != null)
                    {
                        if (exq.T_STAT32 == "1" && exResult.PROTEIN_RESULT.To<string>("").Substring(0, 2) == "03" || exResult.PROTEIN_RESULT.Substring(0, 2) == "04" || exResult.PROTEIN_RESULT.Substring(0, 2) == "05" || exResult.PROTEIN_RESULT.Substring(0, 2) == "06")
                        {
                            DiseasePan = "최고위험";
                        }
                    }
                 
                }
                if (exq != null)
                {
                    if (exq.T_STAT32 == "1" && exResult.SBP.To<int>(0) >= 160 && exResult.DBP.To<int>(0) >= 100)
                    {
                        DiseasePan = "최고위험";
                    }
                    //총콜레스테롤
                    if (exq.T_STAT32 == "1" && exResult.TDL.To<int>(0) >= 310)
                    {
                        DiseasePan = "최고위험";
                    }
                }
              
            }



            if (exq != null)
            {
                bool isDangNyo = false;
                if (exq.T_STAT32 == "1")
                {
                    isDangNyo = true;
                }

                if (exq.T_STAT01 == "1" || exq.T_STAT02 == "1")
                {
                    isDangNyo = true;
                }
                if (isDangNyo)
                {
                    if (COL55 == "" || COL55 == "저위험")
                    {
                        DiseasePan = "고위험";
                    }
                }
                if (exq.T_STAT01 == "1" || exq.T_STAT02 == "1")
                {
                    DiseasePan = "최고위험";
                }
                if (exq.T_STAT11 == "1" || exq.T_STAT12 == "1")
                {
                    DiseasePan = "최고위험";
                }

            }

        
            return DiseasePan;
        }
        /// <summary>
        /// 1단계
        /// </summary>
        /// <param name="firstStep">1단계</param>
        /// <param name="injaCount">발병위험인자개수</param>
        /// <returns></returns>
        public string GetFirstStep(int sbp, int dbp)
        {
            string firstStep = string.Empty;
            if (sbp >= 180 ||dbp >= 110)
            {
                firstStep = "3기 고혈압";
            }
            else if (sbp >= 160 ||dbp >= 100)
            {
                firstStep = "2기 고혈압";
            }
            else if (sbp >= 140 ||dbp >= 90)
            {
                firstStep = "1기 고혈압";
            }
            else if (sbp >= 135 ||dbp >= 85)
            {
                firstStep = "고혈압전단계";
            }
            else if (sbp > 130 || dbp < 85)
            {
                firstStep = "정상";
            }
            else
            {
                firstStep = "최적";
            }
            return firstStep;
        }
    }
}
