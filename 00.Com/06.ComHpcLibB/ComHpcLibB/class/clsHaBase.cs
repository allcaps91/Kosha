using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComDbB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsHaBase
    {
        HicPatientService hicPatientService = null;
        HeaExjongService heaExjongService = null;
        HicExjongService hicExjongService = null;
        HicLtdService hicLtdService = null;
        HicCodeService hicCodeService = null;
        HeaGroupcodeService heaGroupcodeService = null;
        HeaResvExamService heaResvExamService = null;
        HicRescodeService hicRescodeService = null;
        HeaGamcodeService heaGamcodeService = null;
        BasGamfService basGamfService = null;
        BasMailnewService basMailnewService = null;
        HicResultService hicResultService = null;
        HicResultwardService hicResultwardService = null;
        BasScheduleService basScheduleService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicBcodeService hicBcodeService = null;
        DoctorInfoService doctorInfoService = null;
        HicDoctorService hicDoctorService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicMcodeService hicMcodeService = null;
        HicGroupcodeService hicGroupcodeService = null;
        HicJohapcodeService hicJohapcodeService = null;
        HicSpcScodeService hicSpcScodeService = null;
        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;
        BasPcconfigService basPcconfigService = null;
        HicSunapService hicSunapService = null;
        HicSunapdtlService hicSunapdtlService = null;
        ExamDisplayService examDisplayService = null;
        HicResultHisService hicResultHisService = null;
        HicCharttransPrintService hicCharttransPrintService = null;
        OcsDoctorService ocsDoctorService = null;
        HeaResultService heaResultService = null;
        HeaResultHisService heaResultHisService = null;
        HicExcodeService hicExcodeService = null;
        InsaMstService insaMstService = null;
        BasBcodeService basBcodeService = null;
        HeaSunapdtlService heaSunapdtlService = null;
        HeaEkgResultService heaEkgResultService = null;
        HicResEtcService hicResEtcService = null;

        ComFunc CF = new ComFunc();

        public clsHaBase()
        {
            hicPatientService = new HicPatientService();
            heaExjongService = new HeaExjongService();
            hicExjongService = new HicExjongService();
            hicLtdService = new HicLtdService();
            hicCodeService = new HicCodeService();
            heaGroupcodeService = new HeaGroupcodeService();
            heaResvExamService = new HeaResvExamService();
            hicRescodeService = new HicRescodeService();
            heaGamcodeService = new HeaGamcodeService();
            basGamfService = new BasGamfService();
            basMailnewService = new BasMailnewService();
            hicResultService = new HicResultService();
            hicResultwardService = new HicResultwardService();
            basScheduleService = new BasScheduleService();
            comHpcLibBService = new ComHpcLibBService();
            hicBcodeService = new HicBcodeService();
            doctorInfoService = new DoctorInfoService();
            hicDoctorService = new HicDoctorService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicMcodeService = new HicMcodeService();
            hicGroupcodeService = new HicGroupcodeService();
            hicJohapcodeService = new HicJohapcodeService();
            hicSpcScodeService = new HicSpcScodeService();
            hicJepsuService = new HicJepsuService();
            heaJepsuService = new HeaJepsuService();
            basPcconfigService = new BasPcconfigService();
            hicSunapService = new HicSunapService();
            hicSunapdtlService = new HicSunapdtlService();
            examDisplayService = new ExamDisplayService();
            hicResultHisService = new HicResultHisService();
            hicCharttransPrintService = new HicCharttransPrintService();
            ocsDoctorService = new OcsDoctorService();
            heaResultService = new HeaResultService();
            heaResultHisService = new HeaResultHisService();
            hicExcodeService = new HicExcodeService();
            insaMstService = new InsaMstService();
            basBcodeService = new BasBcodeService();
            heaSunapdtlService = new HeaSunapdtlService();
            heaEkgResultService = new HeaEkgResultService();
            hicResEtcService = new HicResEtcService();
        }

        /// <summary>
        /// 신환번호 부여 및 환자마스타에 등록
        /// </summary>
        /// <returns></returns>
        public long New_PatientNo_Create()
        {
            long nNewPano = 0;
            long rtnVal = 0;

            nNewPano = hicPatientService.Read_HicPano();

            if (nNewPano == 0)
            {
                MessageBox.Show("신규번호 부여시 오류가 발생함", "신규번호 생성오류!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rtnVal = nNewPano;

            return rtnVal;
        }

        /// <summary>
        /// 접수번호 부여
        /// </summary>
        /// <returns></returns>
        public long Read_New_JepsuNo()
        {
            long nNewWrtNo = 0;
            long rtnVal = 0;

            //nNewWrtNo = hicPatientService.Read_HeaWrtNo();
            nNewWrtNo = hicPatientService.Read_HicWrtNo();

            if (nNewWrtNo == 0)
            {
                MessageBox.Show("신규접수번호 부여시 오류가 발생함", "신규번호 생성오류!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rtnVal = nNewWrtNo;

            return rtnVal;
        }

        public long Read_New_JepsuGWrtNo()
        {
            long nNewWrtNo = 0;
            long rtnVal = 0;

            nNewWrtNo = hicPatientService.Read_HicGWrtNo();

            if (nNewWrtNo == 0)
            {
                MessageBox.Show("신규접수번호 부여시 오류가 발생함", "신규번호 생성오류!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rtnVal = nNewWrtNo;

            return rtnVal;
        }



        public long READ_New_MisuNo()
        {
            long nNewWrtNo = 0;
            long rtnVal = 0;

            nNewWrtNo = hicPatientService.Read_MisuNo();

            if (nNewWrtNo == 0)
            {
                MessageBox.Show("신규접수번호 부여시 오류가 발생함", "신규번호 생성오류!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rtnVal = nNewWrtNo;

            return rtnVal;
        }

        /// <summary>
        /// 접수번호 부여
        /// </summary>
        /// <returns></returns>
        public long READ_NewHEA_JepsuNo()
        {
            long nNewWrtNo = 0;
            long rtnVal = 0;

            nNewWrtNo = hicPatientService.Read_HeaWrtno();

            if (nNewWrtNo == 0)
            {
                MessageBox.Show("신규접수번호 부여시 오류가 발생함", "신규번호 생성오류!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            rtnVal = nNewWrtNo;

            return rtnVal;
        }

        public void READ_HIC_Doctor(long argSabun)
        {
            clsHcVariable.GstrHicDrName = "";   //판정의 성명
            clsHcVariable.GnHicLicense = 0;     //판정의 면허번호
            clsHcVariable.GstrIpsaDay = "";     //판정의 입사일자
            clsHcVariable.GstrReDay = "";       //판정의 퇴사일자
            string strSabun = string.Format("{0:00000}", argSabun);

            DOCTOR_INFO list = doctorInfoService.Read_Hic_Doctor_Info(strSabun, argSabun);

            if (!list.IsNullOrEmpty())
            {
                clsHcVariable.GstrHicDrName = list.DRNAME.Trim();       //판정의 성명
                clsHcVariable.GnHicLicense = list.LICENCE.To<long>();   //판정의 면허번호
                clsHcVariable.GstrIpsaDay = list.IPSADAY.Trim();        //판정의 입사일자
                clsHcVariable.GstrReDay = list.TOIDAY;                  //판정의 퇴사일자
                clsHcVariable.GstrDrRoom = list.ROOM;                   //판정의 상담실번호
                clsHcVariable.GstrDrGbPan = list.PAN;                   //일반판정여부
                clsHcVariable.GstrDrGbDent = list.GBDENT;               //구강판정여부

                if (clsHcVariable.GstrReDay.IsNullOrEmpty())
                {
                    clsHcVariable.GstrReDay = list.REDAY;               //판정의 퇴사일자
                }
            }
        }

        public string READ_License_DrName(long argLicence)
        {
            string rtnVal = "";

            if (argLicence == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicDoctorService.Read_License_DrName(argLicence);

            if (argLicence == 1809) rtnVal = "이홍주";

            return rtnVal;
        }

        public void READ_HIC_DRCODE(long ArgSabun)
        {
            if (ArgSabun == 0) return;

            HIC_DOCTOR list = hicDoctorService.Read_Hic_DrCode(ArgSabun);

            clsHcVariable.GstrHicDrName = list.DRNAME.Trim();       //판정의 성명
            clsHcVariable.GnHicLicense = long.Parse(list.LICENCE);  //판정의 면허번호
        }

        public string READ_HIC_DRCODE2(long ArgSabun)
        {
            string rtnVal = "";

            if (ArgSabun == 0) return rtnVal;

            HIC_DOCTOR list = hicDoctorService.Read_Hic_DrCode(ArgSabun);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.DRNAME;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        public string READ_HIC_DRCODE3(long ArgSabun)
        {
            string rtnVal = "";

            if (ArgSabun == 0) return "";

            HIC_DOCTOR list = hicDoctorService.Read_Hic_DrCode3(ArgSabun);

            clsHcVariable.GstrHicDrName = list.DRNAME.Trim();       //판정의 성명
            clsHcVariable.GnHicLicense1 = list.DRBUNHO;             //판정의 면허번호

            rtnVal = clsHcVariable.GstrHicDrName;

            return rtnVal;
        }

        public void READ_HIC_DrSabun(string argDrLicence)
        {
            if (argDrLicence == "") return;

            clsHcVariable.GnHicSabun = hicDoctorService.Read_Hic_DrSabun(argDrLicence); //판정의 사원번호
        }

        /// <summary>
        /// 건강진단 종류를 Read
        /// </summary>
        /// <param name="comboNational"></param>
        public void ComboNational_AddItem(ComboBox comboNational)
        {
            List<HIC_CODE> list = hicCodeService.FindOne("A5");
            comboNational.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
        }

        /// <summary>
        /// 건강진단 종류를 READ
        /// </summary>
        /// <param name="ComboJong"></param>
        /// <param name="chk"></param>
        public void ComboJong_AddItem(ComboBox ComboJong, bool chk = false)
        {
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(chk);
            ComboJong.SetItems(list, "NAME", "CODE", "", "");
            ComboJong.SelectedIndex = 0;
        }

        /// <summary>
        /// 건강진단 종류를 READ
        /// </summary>
        /// <param name="comboBox"></param>
        public void ComboJong2_AddItem(ComboBox comboBox)
        {
            List<HEA_EXJONG> list = heaExjongService.Read_Hea_ExJong();
            comboBox.SetItems(list, "NAME", "CODE", "", "");

            comboBox.SelectedIndex = 0;
        }

        public void ComboJong_Set(ComboBox ComboJong)
        {
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);
            ComboJong.SetItems(list, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            ComboJong.SelectedIndex = 0;
        }

        public string LtdName_2_Code(string sName)
        {
            string rtnVal = "";
            string strData = "";
            string strName = "";

            strName = sName;

            //회사명이 공란이면 NULL을 Return
            if (strName == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            //회사명이 숫자이면 변환 안함
            if (VB.IsNumeric(strName))
            {
                rtnVal = strName;
                return rtnVal;
            }

            List<HIC_LTD> list = hicLtdService.Read_Hic_Ltd(strName);

            if (list.Count == 1)
            {
                rtnVal = list[0].CODE.ToString();
                return rtnVal;
            }
            else if (list.Count == 0)
            {
                MessageBox.Show(strName + "으로 시작되는 사업장이 등록 안됨", "검색오류", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                rtnVal = "";
                return rtnVal;
            }

            strData = "";
            for (int i = 0; i < list.Count; i++)
            {
                strData += list[i].CODE + ".";
                strData += list[i].NAME + "." + "\r\n";
            }
            MessageBox.Show("2건 이상", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return rtnVal;
        }

        /// <summary>
        /// READ_HIC_CODE / READ_Hea_CODE / READ_HeaName 통합
        /// </summary>
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_HIC_CODE(string strGubun, string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            HIC_CODE list = hicCodeService.Read_Hic_Code(strGubun, strCode);

            if (list.IsNullOrEmpty())
            {
                rtnVal = "";
            }
            else
            {
                rtnVal = list.NAME;
            }

            return rtnVal;
        }

        public string READ_HicName(string strGubun, string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            HIC_CODE list = hicCodeService.Read_Hic_Code(strGubun, strCode);

            if (list.IsNullOrEmpty())
            {
                rtnVal = "";
            }
            else
            {
                rtnVal = list.NAME;
            }

            return rtnVal;
        }

        public string READ_HIC_Name2(string strGubun, string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicCodeService.Read_Hic_Name2(strGubun, strCode);

            return rtnVal;
        }

        public string READ_HicCode2_GCode(string argGubun, string argCode)
        {
            string rtnVal = "";

            if (argGubun.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicCodeService.Read_HicCode2_GCode(argGubun, argCode);

            return rtnVal;
        }

        /// <summary>
        /// 기초코드에서 특검전송용으로 조치자동 선택
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argCode1"></param>
        /// <param name="argCode2"></param>
        /// <returns></returns>
        public string READ_HicCode2_GCodeNew1(string argGubun, string argCode1, string argCode2)
        {
            string rtnVal = "";

            if (argCode1.IsNullOrEmpty() || argCode2.IsNullOrEmpty())
            {
                rtnVal = hicCodeService.Read_HicCode2_GCodeNew1(argGubun, argCode1, argCode2);
            }

            return rtnVal;
        }

        public string READ_Combo_HicDoctor(ComboBox argCombo)
        {
            string rtnVal = "";

            List<HIC_CODE> list = hicCodeService.Read_Combo_HisDoctor();
            argCombo.SetItems(list, "SNAME", "DRCODE", "", "", AddComboBoxPosition.Top);

            argCombo.SelectedIndex = 0;

            return rtnVal;
        }

        public string READ_Combo_HicDoctor2(ComboBox argCombo)
        {
            string rtnVal = "";

            List<HIC_DOCTOR> list = hicDoctorService.Read_Combo_HisDoctor("");
            argCombo.SetItems(list, "DRNAME", "LICENCE", "전체", "**", AddComboBoxPosition.Top);
            argCombo.SelectedIndex = 0;

            //List<HIC_DOCTOR> list2 = hicDoctorService.Read_Combo_HisDoctor("ALL");
            //argCombo.SetItems(list2, "DRNAME", "LICENCE", "전체", "**", AddComboBoxPosition.Top);
            //argCombo.SelectedIndex = 0;

            return rtnVal;
        }

        public string READ_HicCode2_SCode(long argWrtNo, string argSogenCode)
        {
            string rtnVal = "";

            if (argWrtNo == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicSpcPanjengService.Read_HicCode2_SCode(argWrtNo, argSogenCode);

            return rtnVal;
        }

        public string READ_Hic_Jilbyung(string argGbn, string argPan, string argSogen, string argSogen2)
        {
            string rtnVal = "";

            if (argPan.IsNullOrEmpty())
            {
                return rtnVal;
            }

            rtnVal = hicCodeService.Read_Hic_Jibung(argGbn, argPan, argSogen, argSogen);

            return rtnVal;
        }

        public string READ_SCode2_GCode(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                return rtnVal;
            }

            HIC_SCODE list = hicCodeService.Read_SCode2_GCode(argCode);

            if (list != null)
            {
                rtnVal = list.CODE + "@@" + list.JCODE + "@@" + list.CHASU;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        //READ_HeaName => READ_HIC_CODE 와 통합

        public string READ_Res_ComboSet(string argGubun, ComboBox argCombo)
        {
            string rtnVal = "";

            List<HIC_RESCODE> list = hicRescodeService.Read_Res_ComboSet(argGubun);
            argCombo.SetItems(list, "NAME", "", " ", "", AddComboBoxPosition.Top);
            argCombo.SelectedIndex = 0;

            return rtnVal;
        }

        /// <summary>
        /// 묶음코드명 조회
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_GCodeName(string strCode)
        {
            string rtnVal = "";
            string strExams = "";
            string strExams1 = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            strExams = "";
            strExams1 = "";

            //종검 묶음코드명 조회
            for (int i = 0; i < strCode.Length; i++)
            {
                if (strCode.Substring(i, 1) != ",")
                {
                    strExams1 += strExams + ",";
                }
                else
                {
                    strExams1 += strExams + ",";
                    strExams = "";
                }
            }

            strExams1 = VB.Left(strExams1.Trim(), strExams1.Trim().Length - 1);

            List<HEA_GROUPCODE> list = heaGroupcodeService.Read_Hea_GroupCode(strExams1);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    rtnVal += list[i].NAME;
                }
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// 건진종류명을 READ
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_GjJong_Name(string strCode)
        {
            string rtnVal = string.Empty;

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }
            rtnVal = hicExjongService.Read_Hic_ExJong_Name(strCode.Trim());
            //rtnVal = heaExjongService.Read_ExJong_Name(strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 건진종류명을 READ
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_HIC_GjJong_Name(string strCode)
        {
            string rtnVal = string.Empty;

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicExjongService.Read_Hic_ExJong_Name(strCode.Trim());

            return rtnVal;
        }

        public string Read_Exam_Schedule(long nPano, string strSDate, List<GROUPCODE_EXAM_DISPLAY> lstGED)
        {
            string rtnVal = "";

            string strCode = "";
            string strRowid = "";

            if (lstGED.IsNullOrEmpty() || lstGED.Count == 0)
            {
                return rtnVal;
            }

            for (int i = 0; i < lstGED.Count; i++)
            {
                strCode = lstGED[i].EXCODE;

                strRowid = heaResvExamService.Read_Hes_Resv_Exam(strSDate, nPano, strCode);

                if (strRowid.IsNullOrEmpty())
                {
                    rtnVal = "OK";
                    break;
                }
            }

            return rtnVal;
        }

        public string Read_Exam_Schedule_One(long nPano, string strSDate, string strExCode)
        {
            string rtnVal = "";

            string strRowid = "";

            strRowid = heaResvExamService.Read_Hes_Resv_Exam(strSDate, nPano, strExCode);

            if (strRowid.IsNullOrEmpty())
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        /// <summary>
        /// 결과값코드 Read => READ_Res_Name(HaAct.vbp)
        /// </summary>
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_Res_Name(string strGubun, string strCode)
        {
            string rtnVal = "";

            string strSName = "";

            strCode = string.Format("{0:00}", strCode);

            if (strGubun.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            List<HIC_RESCODE> list = hicRescodeService.Read_HIc_ResCode(strGubun, strCode, "");

            if (strCode.IsNullOrEmpty())   //구분별로 모든 코드를 조회
            {
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strSName += list[i].NAME + ",";
                    }
                    strSName = VB.Left(strSName, strSName.Length - 1);
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "";
                }
            }
            else
            {
                if (list.Count > 0) //원하는 코드만 조회
                {
                    rtnVal = list[0].NAME;
                }
                else
                {
                    rtnVal = "";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 결과값코드 Read => READ_Res_Name(HcAct.vbp)
        /// </summary>
        /// <param name="strGubun"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_Res_Name2(string argGubun, string argCode)
        {
            string rtnVal = "";
            int nlen = 0;
            string strSName = "";
            string strTemp = "";
            string strBun = "";
            string strGubun = "";
            string strCode = "";

            if (strGubun.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            nlen = strCode.Length / 2;
            //???????????????????????????????
            //for (int i = 1; i < nlen; i++)
            //{
            //    if (VB.Mid(strCode, (i * 2) - 1, 2) == "10")
            //    {
            //    }
            //}

            if (strCode.IsNullOrEmpty())
            {
                if (strCode.Length > 1 && strCode != "10")
                {
                    strBun = "IN";
                    for (int i = 0; i < strCode.Length; i++)
                    {
                        strTemp += string.Format("0:00", strCode) + ",";
                    }
                    strTemp = VB.Left(strTemp, strTemp.Length - 1);
                    strCode = strTemp;
                }
                else
                {
                    strBun = "";
                    strCode = string.Format("{0:00}", argCode);
                }
            }

            List<HIC_RESCODE> list = hicRescodeService.Read_HIc_ResCode(strGubun, strCode, strBun);

            if (strCode.IsNullOrEmpty())   //구분별로 모든 코드를 조회
            {
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strSName += list[i].NAME + ",";
                    }
                    strSName = VB.Left(strSName, strSName.Length - 1);
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "";
                }
            }
            else
            {
                if (list.Count > 0) //원하는 코드만 조회
                {
                    if (argCode.Length > 1 && argCode != "10")
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            strSName += list[i].NAME + ",";
                        }
                        strSName = VB.Left(strSName, strSName.Length - 1);
                        rtnVal = strSName.Trim();
                    }
                    else
                    {
                        rtnVal = list[0].NAME;
                    }
                }
                else
                {
                    rtnVal = "";
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 결과값코드 멀티 Read
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_Muti_Res_Name(string argGubun, string argCode)
        {
            string rtnVal = "";
            string strName = "";
            string strTemp = "";

            if (argGubun.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            strName = "";
            if (argCode.Length > 0)
            {
                for (int i = 0; i < argCode.Length; i++)
                {
                    List<HIC_RESCODE> list = hicRescodeService.Read_HIc_ResCode(argGubun, argCode, "");

                    if (list.Count > 0) //원하는 코드만 조회
                    {
                        strTemp += list[0].NAME + ", ";
                    }
                    else
                    {
                        strTemp = "";
                    }
                }

                rtnVal = strTemp;
            }
            return rtnVal;
        }

        /// <summary>
        /// 결과값코드 Read
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_MultiRes_Name(string argGubun, string argCode)
        {
            string rtnVal = "";
            string strName = "";
            string strCode = "";
            string strTemp = "";

            if (argGubun.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            strCode = string.Format("{0:00}", argCode);

            List<HIC_RESCODE> list = hicRescodeService.Read_HIc_ResCode(argGubun, strCode, "");

            if (argCode.IsNullOrEmpty())   //구분별로 모든 코드를 조회
            {
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strName += list[i].NAME + ",";
                    }
                    strName = VB.Left(strName, strName.Length - 1);
                    strTemp = strName.Trim();
                }
                else
                {
                    strTemp = "";
                }
                rtnVal = strTemp;
            }
            else
            {
                if (list.Count > 0)
                {
                    rtnVal = list[0].NAME.Trim();
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 종검종류명을 READ
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_GjJong_HeaName(string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = heaExjongService.Read_Hea_ExJong_Name(strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 취급물질명을 READ
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_MCode_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicMcodeService.Read_MCode_Name(argCode);

            return rtnVal;
        }

        /// <summary>
        /// 취급물질명을 READ하여 검진주기를 조회
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_MCode_Jugi(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicMcodeService.Read_MCode_Jugi(argCode);

            switch (rtnVal)
            {
                case "6":
                    rtnVal = "1";
                    break;
                case "12":
                    rtnVal = "2";
                    break;
                case "24":
                    rtnVal = "3";
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 그룹코드명 READ
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_Group_Name(string argCode)
        {
            string rtnVal = "";

            rtnVal = hicGroupcodeService.Read_Group_Name(argCode);

            return rtnVal;
        }

        /// <summary>
        /// 조합명칭을 READ
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_Johap_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicJohapcodeService.Read_Johap_Name(argCode);

            return rtnVal;
        }

        /// <summary>
        /// 주민번호로 건진번호 찾기
        /// </summary>
        /// <param name="argJumin"></param>
        /// <returns></returns>
        public long READ_Jumin_Pano(string argJumin)
        {
            long rtnVal = 0;
            string strJumin = "";

            if (argJumin.IsNullOrEmpty())
            {
                rtnVal = 0;
                return rtnVal;
            }

            strJumin = clsAES.DeAES(argJumin);

            rtnVal = hicPatientService.Read_Jumin_HicPano(strJumin);

            return rtnVal;
        }

        /// <summary>
        /// 회사명칭을 READ   READ_Ltd_Name2() Merge
        /// READ_Ltd_Name() => READ_Ltd_One_Name()
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_Ltd_One_Name(string strCode)
        {
            string rtnVal = "";

            if (VB.Val(strCode) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicLtdService.READ_Ltd_One_Name(strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 회사명칭을 READ
        /// READ_Ltd_Name() => READ_Ltd_One_Name()
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_Ltd_Name(string strCode)
        {
            string rtnVal = "";

            if (VB.Val(strCode) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            HIC_LTD list = hicLtdService.FindOne(strCode.Trim());

            if (list != null)
            {
                rtnVal = list.NAME;
                clsHcVariable.GstrLtdJuso = list.JUSO + " " + list.JUSODETAIL;
                clsHcVariable.GstrLtdJuso1 = list.SANGHO + " ";
                clsHcVariable.GstrLtdJuso2 = list.SANGHO + " " + list.NAME;
                clsHcVariable.GstrLtdMailcode = VB.Mid(list.MAILCODE, 1, 1) + " " + VB.Mid(list.MAILCODE, 2, 1) + " " +
                                                VB.Mid(list.MAILCODE, 3, 1) + "-" + VB.Mid(list.MAILCODE, 4, 1) + " " +
                                                VB.Mid(list.MAILCODE, 5, 1) + "-" + VB.Mid(list.MAILCODE, 6, 1) + " ";
                clsHcVariable.GstrKiho = list.KIHO;
                clsHcVariable.GnInwon = Convert.ToInt32(list.INWON);
                clsHcVariable.GstrTel = list.TEL;
            }
            else
            {
                rtnVal = "";
                clsHcVariable.GstrLtdJuso = "";
                clsHcVariable.GstrLtdJuso1 = "";
                clsHcVariable.GstrLtdJuso2 = "";
                clsHcVariable.GstrLtdMailcode = "";
                clsHcVariable.GstrKiho = "";
                clsHcVariable.GnInwon = 0;
                clsHcVariable.GstrTel = "";
            }
            return rtnVal;
        }

        public string READ_AmtCode_Jong(string strCode)
        {
            string rtnVal = "";

            switch (VB.Left(strCode, 1))
            {
                case "A":
                    rtnVal = "1차검사";
                    break;
                case "B":
                    rtnVal = "2차검사";
                    break;
                case "C":
                    rtnVal = "유해인자";
                    break;
                case "D":
                    rtnVal = "암검진";
                    break;
                case "Z":
                    rtnVal = "기타검진";
                    break;
                default:
                    rtnVal = "▶오류◀";
                    break;
            }
            return rtnVal;
        }

        public string READ_Bogunso_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicCodeService.Read_Hic_Name2("25", argCode);

            return rtnVal;
        }





        /// <summary>
        /// 종합검진 검사결과 조회순서/인쇄순서 그룹명
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_HeaSORT_Name(string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicCodeService.Read_Hic_CodeName(strCode.Trim());

            return rtnVal;
        }

        public string READ_Sunap_SelfName(string strGbSelf)
        {
            string rtnVal = "";
            string sGbSelf = "";

            sGbSelf = string.Format("{0:00}", strGbSelf);

            //IList<HIC_CODE> Ilist = hicCodeService.FindCodeIn("B4", sGbSelf);
            List<HIC_CODE> list = hicCodeService.FindCodeIn("B4", sGbSelf);
            rtnVal = list[0].NAME;

            return rtnVal;
        }

        /// <summary>
        /// 결과코드명 READ  READ_HeaResultName() Merge
        /// </summary>
        /// <param name="strGBN"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_ResultName(string strGBN, string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicRescodeService.Read_Hic_ResCodeName(strGBN, strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 검사(방사선) 코드명칭을 읽음
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string Read_ExCode_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicExcodeService.GetHNmaebyCode(argCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 시작시간(hh:mm)부터 종료시간(hh:mm)까지의 소요시간(분)을 계산
        /// </summary>
        /// <param name="strSTime"></param>
        /// <param name="strETime"></param>
        /// <returns></returns>
        public double Soyo_Time_Gesan(string strSTime, string strETime)
        {
            double rtnVal = 0;
            double nBUn1 = 0;
            double nBUn2 = 0;

            nBUn1 = VB.Val(VB.Left(strSTime, 2)) * 60 + VB.Val(VB.Right(strSTime, 2));
            nBUn2 = VB.Val(VB.Left(strETime, 2)) * 60 + VB.Val(VB.Right(strETime, 2));

            rtnVal = nBUn2 - nBUn1;

            return rtnVal;
        }

        public string READ_GbSuga_Name(string strGBN)
        {
            string rtnVal = "";

            switch (strGBN)
            {
                case "1":
                    rtnVal = "보험80%";
                    break;
                case "2":
                    rtnVal = "보험100%";
                    break;
                case "3":
                    rtnVal = "보험125%";
                    break;
                case "4":
                    rtnVal = "특검차액";
                    break;
                case "5":
                    rtnVal = "임의수가";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// READ_GAMGYE_NAME() Merge
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string READ_GAMGYE(string strCode)
        {
            string rtnVal = "";

            rtnVal = heaGamcodeService.Read_Hea_GamName(strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 특수 소견코드명을 READ
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_SpcSCode_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicSpcScodeService.Read_Spc_Scode_Name(argCode);

            return rtnVal;
        }

        public string READ_SpcPanjeng_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            switch (argCode)
            {
                case "1":
                    rtnVal = "1.A(정상)";
                    break;
                case "2":
                    rtnVal = "2.B(정상B)";
                    break;
                case "3":
                    rtnVal = "3.C1(건강주의)";
                    break;
                case "4":
                    rtnVal = "4.C2(건강주의)";
                    break;
                case "5":
                    rtnVal = "5.D1(직업병)";
                    break;
                case "6":
                    rtnVal = "6.D2(일반질병)";
                    break;
                case "7":
                    rtnVal = "7.R(재검)";
                    break;
                case "8":
                    rtnVal = "8.U(미판정자)";
                    break;
                case "9":
                    rtnVal = "9.CN(야간작업)";
                    break;
                case "A":
                    rtnVal = "A.DN(야간작업)";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }

        public string Jisa2_Code2Name(string argJisaName)
        {
            string rtnVal = "";

            rtnVal = hicCodeService.Read_JisaCode(argJisaName);

            return rtnVal;
        }



        public string READ_GAM_OPD(string strJumin)
        {
            string rtnVal = "";
            string strTemp = "";

            BAS_GAMF list = basGamfService.Read_Gam_Opd(strJumin);

            if (list != null)
            {
                strTemp = list.GAMMESSAGE;

                if (list.GAMEND != null)
                {
                    strTemp += " [퇴사일자:" + list.GAMEND + "]";
                }

                rtnVal = strTemp;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// Mail Address Read
        /// </summary>
        /// <returns></returns>
        public string READ_MAIL_Name(string strCode)
        {
            string rtnVal = "";

            rtnVal = basMailnewService.Read_MailName(strCode.Trim());

            return rtnVal;
        }

        public string Read_Result_Data(long nWrtNo, string strCode)
        {
            string rtnVal = "";

            HIC_RESULT list = hicResultService.Read_Result_Data(strCode.Trim());

            if (list != null)
            {
                rtnVal = list.RESULT;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        public string Read_JilJong_Name(string strCode)
        {
            string rtnVal = "";

            if (strCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicResultwardService.Read_WardName(strCode.Trim());

            return rtnVal;
        }

        /// <summary>
        /// 인사마스타에서 직원이름 찾기(이름의 공백을 제거함)
        /// 기존 VB는 long type => string 으로 parameter 던질것
        /// </summary>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public string READ_JikwonName(string strSabun)
        {
            string rtnVal = "";

            if (strSabun == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            //INSA_MST에서 직원명 조회
            rtnVal = comHpcLibBService.Read_JikWonName(string.Format("{0:00000}", strSabun));

            return rtnVal;
        }

        /// <summary>
        /// 주민번호 복호화(암호화된번호, 기존번호)
        /// </summary>
        /// <param name="strJumin1"></param>
        /// <param name="strJumin2"></param>
        /// <returns></returns>
        public string Read_Jumin_Decrypt(string strJumin1, string strJumin2)
        {
            string rtnVal = "";

            if (strJumin1 == "")
            {
                rtnVal = strJumin2.Trim();
            }
            else
            {
                rtnVal = clsAES.DeAES(strJumin1.Trim());
            }

            return rtnVal;
        }

        /// <summary>
        /// 종합검진 검사결과 판정(L=Low,H=High,"":Nomal)
        /// </summary>
        /// <param name="strExCode"></param>
        /// <param name="strResult"></param>
        /// <param name="strNormal"></param>
        /// <returns></returns>
        public string Result_Panjeng(string strExCode, string strResult, string strNormal)
        {
            string rtnVal = "";
            double nMinValue = 0;
            double nMaxValue = 0;
            double nResult = 0;
            double nLowRes = 0;
            double nHighRes = 0;

            if (strResult.IsNullOrEmpty() || strNormal.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            if (VB.L(strNormal, "~") < 2)
            {
                rtnVal = "";
                return rtnVal;
            }

            nMinValue = string.Format(VB.Pstr(strNormal, "~", 1)).To<double>();
            nMaxValue = string.Format(VB.Pstr(strNormal, "~", 2)).To<double>();

            if (nMinValue == 0 && nMaxValue == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            nResult = VB.Val(string.Format(strResult));
            nResult = VB.Val(VB.Replace(VB.Replace(strResult, ">", ""), "<", ""));



            switch (strExCode.Trim())
            {
                case "A271":
                case "A272":
                    nLowRes = string.Format(VB.Pstr(strResult, "-", 1)).To<double>();
                    nHighRes = string.Format(VB.Pstr(strResult, "-", 2)).To<double>();

                    if (nMinValue > nLowRes)
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else if (nMaxValue < nHighRes)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                case "A241":
                    if (nResult > nMaxValue)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                case "TU42":
                    if (nMinValue > nResult)
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else if (nResult > nMaxValue)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }

                //case "TX26":
                //    nResult = VB.Val(VB.TR(strResult, ">", ""));

                //    if (nResult > nMaxValue)
                //    {
                //        rtnVal = "H";
                //        return rtnVal;
                //    }
                //    else
                //    {
                //        rtnVal = "";
                //        return rtnVal;
                //    }
                //case "TU42":
                //    nResult = VB.Val(VB.TR(strResult, ">", ""));

                //    if (nResult > nMaxValue)
                //    {
                //        rtnVal = "H";
                //        return rtnVal;
                //    }
                //    else
                //    {
                //        rtnVal = "";
                //        return rtnVal;
                //    }
                //case "E903":
                //    nResult = VB.Val(VB.TR(strResult, ">", ""));

                //    if (nResult > nMaxValue)
                //    {
                //        rtnVal = "H";
                //        return rtnVal;
                //    }
                //    else
                //    {
                //        rtnVal = "";
                //        return rtnVal;
                //    }
                default:
                    rtnVal = "";
                    break;
            }

            //소변 및 대변 검사
            switch (strResult)
            {
                case "음성":
                case "-":
                    rtnVal = "";
                    return rtnVal;
                case "양성":
                    rtnVal = "L";
                    return rtnVal;
                case "+-":
                    if (strExCode.Trim() == "LU46" && strExCode.Trim() == "A259")
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                default:
                    rtnVal = "";
                    break;
            }

            rtnVal = "";
            if (nResult < nMinValue)
            {
                rtnVal = "L";
            }
            else if (nResult > nMaxValue)
            {
                rtnVal = "H";
            }
            else
            {
                rtnVal = "";    //Nomal 또는 점검불능
            }

            return rtnVal;
        }

        /// <summary>
        /// 종합검진 검사결과 판정NEW(B로 표시)
        /// </summary>
        /// <param name="strExcode"></param>
        /// <param name="strREsult"></param>
        /// <param name="strNormal"></param>
        /// <returns></returns>
        public string Result_Panjeng_New(string strExCode, string strResult, string strNormal)
        {
            string rtnVal = "";
            double nMinValue = 0;
            double nMaxValue = 0;
            double nResult = 0;
            double nLowRes = 0;
            double nHighRes = 0;

            if (strResult.IsNullOrEmpty() || strNormal == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            if (VB.L(strNormal, "~") < 2)
            {
                rtnVal = "";
                return rtnVal;
            }

            nMinValue = VB.Val(string.Format(VB.Pstr(strNormal, "~", 1)));
            nMaxValue = VB.Val(string.Format(VB.Pstr(strNormal, "~", 2)));

            if (nMinValue == 0 && nMaxValue == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            nResult = VB.Val(string.Format(strResult));
            nResult = VB.Val(VB.TR(VB.TR(strResult, ">", ""), "<", ""));

            if (strExCode == "TX26")
            {
                nResult = VB.Val(VB.Replace(VB.Replace(strResult, ">", ""), "<", ""));
            }

            switch (strExCode)
            {
                case "A271":
                case "A272":
                    nLowRes = VB.Val(string.Format(VB.Pstr(strResult, "-", 1)));
                    nHighRes = VB.Val(string.Format(VB.Pstr(strResult, "-", 2)));

                    if (nMinValue > nLowRes)
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                    else if (nMaxValue < nHighRes)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else if (VB.L(strResult, "Many") > 1)
                    {
                        rtnVal = "H";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                //case "TX26":
                //    nResult = VB.Val(VB.TR(strResult, ">", ""));

                //    if (nResult > nMaxValue)
                //    {
                //        rtnVal = "H";
                //        return rtnVal;
                //    }
                //    else
                //    {
                //        rtnVal = "";
                //        return rtnVal;
                //    }
                //case "TU42":

                //    nResult = VB.Val(VB.TR(VB.TR(strResult, ">", ""), "<", ""));

                //    if (nResult > nMaxValue)
                //    {
                //        rtnVal = "H";
                //        return rtnVal;
                //    }
                //    else
                //    {
                //        rtnVal = "";
                //        return rtnVal;
                //    }
                //case "E903":
                //    nResult = VB.Val(VB.TR(strResult, ">", ""));

                //    if (nResult > nMaxValue)
                //    {
                //        rtnVal = "H";
                //        return rtnVal;
                //    }
                //    else
                //    {
                //        rtnVal = "";
                //        return rtnVal;
                //    }
                default:
                    rtnVal = "";
                    break;
            }

            //소변 및 대변 검사
            switch (strResult)
            {
                case "음성":
                case "-":
                    rtnVal = "";
                    return rtnVal;
                case "양성":
                    rtnVal = "L";
                    return rtnVal;
                case "+-":
                    if (strExCode.Trim() == "LU46" && strExCode.Trim() == "A259")
                    {
                        rtnVal = "";
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = "L";
                        return rtnVal;
                    }
                default:
                    rtnVal = "";
                    break;
            }

            rtnVal = "";
            if (nResult < nMinValue)
            {
                rtnVal = "B";
            }
            else if (nResult > nMaxValue)
            {
                rtnVal = "B";
            }
            else
            {
                rtnVal = "";    //Nomal 또는 점검불능
            }

            return rtnVal;
        }

        public string Result_Level_Panjeng_New(string strExCode, string strResult, string strNormal, string strLvl1, string strLvl2)
        {
            string rtnVal = "";
            double nMinValue = 0;
            double nMinValue1 = 0;
            double nMinValue2 = 0;
            double nMinValue3 = 0;
            double nMaxValue = 0;
            double nMaxValue1 = 0;
            double nMaxValue2 = 0;
            double nMaxValue3 = 0;
            double nResult = 0;

            if (strResult == "" || strNormal == "" || strResult == ".")
            {
                rtnVal = "";
                return rtnVal;
            }

            //Nomal Value Check
            if (VB.L(strNormal, "~") < 2)
            {
                rtnVal = "";
                return rtnVal;
            }

            nMinValue = VB.Val(string.Format(VB.Pstr(strNormal, "~", 1)));
            nMinValue1 = VB.Val(string.Format(VB.Pstr(strLvl1, "~", 1)));
            nMinValue2 = VB.Val(string.Format(VB.Pstr(strLvl2, "~", 1)));

            nMaxValue = VB.Val(string.Format(VB.Pstr(strNormal, "~", 1)));
            nMaxValue1 = VB.Val(string.Format(VB.Pstr(strLvl1, "~", 1)));
            nMaxValue2 = VB.Val(string.Format(VB.Pstr(strLvl2, "~", 1)));

            if (nMinValue == 0 && nMaxValue == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            nResult = VB.Val(string.Format(strResult));

            rtnVal = "";

            //코드마스타에 등록된 값으로 검사항목별 판정
            if (nResult != 0)
            {
                if (nResult >= nMinValue && nResult <= nMaxValue)
                {
                    rtnVal = "";
                }
                else if (nResult >= nMinValue1 && nResult <= nMaxValue1)
                {
                    rtnVal = "D";
                }
                else if (nResult >= nMinValue2 && nResult <= nMaxValue2)
                {
                    rtnVal = "C";
                }
                else
                {
                    rtnVal = "D";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// SEQ_PACSNO READ
        /// </summary>
        /// <returns></returns>
        public string READ_XRAY_PACSNO()
        {
            string rtnVal = "";
            string strPacsNo = "";

            strPacsNo = clsPublic.GstrSysDate.Replace("-", "");
            string strSeq = comHpcLibBService.Seq_PacsNo();
            if (strSeq.IsNullOrEmpty())
            {
                MessageBox.Show("PACS용 Accession Number 부여시 오류가 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtnVal = "";
            }
            else
            {
                rtnVal = strPacsNo + strSeq.PadLeft(4, '0');
            }

            return rtnVal;
        }

        public string Jisa2_Name(string strJisa)
        {
            string rtnVal = "";

            // 널체크 추가 / 2021.06.21 심명섭
            if (!strJisa.IsNullOrEmpty())
            {
                rtnVal = comHpcLibBService.Read_JisaName(strJisa);

                return rtnVal;
            }
            else
            {
                rtnVal = "";
                return rtnVal;
            }
        }

        /// <summary>
        /// . 없애기
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string Dot_Clear(string strData)
        {
            string rtnVal = "";

            //For i = 1 To Len(argDate)
            //    If Mid(argDate, i, 1) <> "." Then Dot_Clear = Dot_Clear + Mid(argDate, i, 1)
            //Next i

            rtnVal = strData.Replace(".", "");

            return rtnVal;
        }

        /// <summary>
        /// - 없애기
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public string Dash_Clear(string strData)
        {
            string rtnVal = "";

            //For i = 1 To Len(ArgDate)
            //    If Mid(ArgDate, i, 1) <> "-" Then Dash_Clear = Dash_Clear + Mid(ArgDate, i, 1)
            //Next i

            rtnVal = strData.Replace("-", "");

            return rtnVal;
        }

        /// <summary>
        /// , 없애기
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public string Comma_Clear(string strData)
        {
            string rtnVal = "";

            //For i = 1 To Len(ArgDate)
            //    If Mid(ArgDate, i, 1) <> "," Then Comma_Clear = Comma_Clear + Mid(ArgDate, i, 1)
            //Next i

            rtnVal = strData.Replace(",", "");

            return rtnVal;
        }

        public string SpaceLF_Clear(string strData)
        {
            string rtnVal = "";

            //For i = 1 To Len(ArgDate)
            //    If Mid(ArgDate, i, 1) <> Chr$(10) Then SpaceLF_Clear = SpaceLF_Clear + Mid(ArgDate, i, 1)
            //Next i

            rtnVal = strData.Replace("\n", "");
            rtnVal = rtnVal.Replace("\r", "");

            return rtnVal;
        }

        public string READ_JepsuSTS(long argWrtNo)
        {
            string rtnVal = "";

            rtnVal = hicJepsuService.Read_JepsuSts(argWrtNo);

            return rtnVal;
        }

        public string READ_JepsuSTS_Hea(long argWrtNo)
        {
            string rtnVal = "";

            rtnVal = heaJepsuService.Read_JepsuSts(argWrtNo);

            return rtnVal;
        }

        /// <summary>
        /// 삭제(DB 방식으로 변경(HC_Act\frmHcAct.cs의 Read_INI() 참조)
        /// </summary>
        public void READ_HEA_INI()
        {
        }

        public string READ_SangDam_Gubun(string argJong)
        {
            string rtnVal = "";

            if (argJong.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicExjongService.Read_GbSangdam(argJong);

            return rtnVal;
        }

        /// <summary>
        /// 상담acting 코드 유무 확인
        /// </summary>
        /// <param name="WrtNo"></param>
        /// <returns></returns>
        public string READ_SangDam_Acting(long WrtNo)
        {
            string rtnVal = "";
            string strTemp = "";

            if (WrtNo == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            strTemp = hicResultService.Read_Sangdam_Acting(WrtNo);

            if (!strTemp.IsNullOrEmpty())
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        /// <summary>
        /// Read_Hea_Result() Merge
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argExCode"></param>
        /// <returns></returns>
        public string Read_Hic_Result(long argWrtNo, string argExCode)
        {
            string rtnVal = "";

            if (argWrtNo == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            HIC_RESULT list = hicResultService.Read_Result_YN(argWrtNo, argExCode);

            if (list != null)
            {
                rtnVal = list.RESULT;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        /// <summary>
        /// READ_HIC_신용카드승인구분
        /// </summary>
        public void READ_HIC_Credit_Approval_Gubun()
        {
            clsCompuInfo.SetComputerInfo();

            clsHcVariable.GstrCardApprov = basPcconfigService.GetConfig_CardGubun(clsCompuInfo.gstrCOMIP);
            //SELECT VALUEV FROM KOSMOS_PMPA.BAS_PCCONFIG
            // WHERE GUBUN = '카드구분'
            //   AND IPADDRESS = :IPADDRESS
        }

        public double HIC_AGE_GESAN(string argJumin)
        {
            double rtnVal = 0;
            double nYear = 0;
            double nYY = 0;

            nYear = VB.Val(VB.Left(clsPublic.GstrSysDate, 4));
            nYY = VB.Val(VB.Left(argJumin, 2));

            return rtnVal;
        }

        /// <summary>
        /// 검진나이계산 - 만나이로 계산 생일무시함(YYYY-MM-DD Type)
        /// READ_HEA_AGE_GESAN() Merge
        /// </summary>
        /// <param name="argJumin"></param>
        /// <returns></returns>
        public double READ_HIC_AGE_GESAN(string argJumin)
        {
            double rtnVal = 0;

            string strGbn = "";     //구분
            string strJumin = "";   //주민번호
            string strBirth = "";   //생년월일
            double nYear = 0;         //생년일
            double nJYear = 0;        //검진년도

            rtnVal = 999;

            strJumin = argJumin;
            strGbn = VB.Mid(argJumin, 7, 1);
            nJYear = VB.Val(VB.Left(clsPublic.GstrSysDate, 4));

            //생년월일을 YYYY-MM-DD Type으로 변경
            if (strGbn == "1" || strGbn == "2" || strGbn == "5" || strGbn == "6")       // 한국인 남1 녀2, 외국인 남5 녀6
            {
                strBirth = "19" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else if (strGbn == "3" || strGbn == "4" || strGbn == "7" || strGbn == "8")   // 한국인 남3 녀4 , 외국인 남7 녀8
            {
                strBirth = "20" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else if (strGbn == "0" || strGbn == "9")
            {
                strBirth = "18" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else
            {
                rtnVal = 999;
            }

            nYear = VB.Val(VB.Left(strBirth, 4));

            rtnVal = nJYear - nYear;

            return rtnVal;

        }

        public string Read_DrNamebyDrBunho(long pANJENGDRNO8)
        {
            string rtnVal = "";

            if (pANJENGDRNO8.IsNullOrEmpty() || pANJENGDRNO8 == 0) return "";

            rtnVal = ocsDoctorService.GetRedadDrNmaebyDrBunho(pANJENGDRNO8);

            return rtnVal;
        }

        public string Read_DrNamebyInsaDrBunho(long pANJENGDRNO8)
        {
            string rtnVal = "";

            if (pANJENGDRNO8.IsNullOrEmpty() || pANJENGDRNO8 == 0) return "";

            //rtnVal = ocsDoctorService.GetRedadDrNmaebyDrBunho(pANJENGDRNO8);
            rtnVal = insaMstService.GetKornameByMyenBunho(pANJENGDRNO8.ToString());
            return rtnVal;
        }

        public string READ_SPC_GBDENTAL(string argMCode)
        {
            string rtnVal = "";
            string strFlag = "";

            strFlag = hicMcodeService.Read_GbDent(argMCode);

            if (strFlag != null)
            {
                rtnVal = strFlag;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        public string READ_GJJONG_CODE(long argWrtNo)
        {
            string rtnVal = "";

            rtnVal = hicJepsuService.Read_GjJong(argWrtNo);

            return rtnVal;
        }

        /// <summary>
        /// 건진판정의사 휴무일 Check(정상: OK / 휴무: NO) => 사용무
        /// </summary>
        /// <param name="argSabun"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public string READ_HIC_DOCTOR_GBCHUL(long argSabun, string argDate)
        {
            //string rtnVal = "OK";
            string rtnVal = "";
            //string strYYMM = "";
            //string strTP = "";
            //double nDD = 0;

            //strYYMM = VB.Left(argDate, 4) + VB.Mid(argDate, 6, 2);
            //strTP = VB.Right(argDate, 2);
            //nDD = VB.Val(VB.Right(argDate, 2));

            return rtnVal;
        }

        /// <summary>
        /// HIC_1차2차_읽기()
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argGubun"></param>
        /// <returns></returns>
        public long HIC_Read_First_Second(long argWrtNo, string argGubun)
        {
            long rtnVal = 0;

            string strJepDate = "";
            long nPano = 0;
            string strGjYear = "";
            string strGjChasu = "";
            string strChasu = "";
            string strCode2 = "";

            clsHcVariable.GnPano_NEW = 0;
            clsHcVariable.GnWRTNO_NEW1 = 0;
            clsHcVariable.GnWRTNO_NEW2 = 0;

            HIC_JEPSU list = hicJepsuService.Read_Jepsu_Wrtno(argWrtNo);

            if (list != null)
            {
                strJepDate = list.JEPDATE.ToString();
                strGjYear = list.GJYEAR;
                strGjChasu = list.GJCHASU;
                nPano = Convert.ToInt64(list.PANO);
            }
            else
            {
                rtnVal = 0;
                return rtnVal;
            }

            clsHcVariable.GnPano_NEW = nPano;
            clsHcVariable.GnWRTNO_NEW1 = argWrtNo;

            switch (argGubun)
            {
                case "1":
                    strChasu = "2";
                    strCode2 = "2";
                    clsHcVariable.GnWRTNO_NEW2 = hicJepsuService.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
                    if (clsHcVariable.GnWRTNO_NEW2 != 0)
                    {
                        rtnVal = clsHcVariable.GnWRTNO_NEW2;
                    }
                    else
                    {
                        rtnVal = 0;
                    }
                    break;
                case "2":
                    strChasu = "1";
                    strCode2 = "1";
                    clsHcVariable.GnWRTNO_NEW1 = hicJepsuService.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
                    if (clsHcVariable.GnWRTNO_NEW1 != 0)
                    {
                        rtnVal = clsHcVariable.GnWRTNO_NEW1;
                    }
                    else
                    {
                        rtnVal = 0;
                    }
                    break;
                case "3":
                    strChasu = "2";
                    strCode2 = "4";
                    clsHcVariable.GnWRTNO_NEW2 = hicJepsuService.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
                    if (clsHcVariable.GnWRTNO_NEW2 != 0)
                    {
                        rtnVal = clsHcVariable.GnWRTNO_NEW2;
                    }
                    else
                    {
                        rtnVal = 0;
                    }
                    break;
                case "4":
                    strChasu = "1";
                    strCode2 = "3";
                    clsHcVariable.GnWRTNO_NEW1 = hicJepsuService.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
                    if (clsHcVariable.GnWRTNO_NEW1 != 0)
                    {
                        rtnVal = clsHcVariable.GnWRTNO_NEW1;
                    }
                    else
                    {
                        rtnVal = 0;
                    }
                    break;
                case "5":
                    strChasu = "2";
                    strCode2 = "6";
                    clsHcVariable.GnWRTNO_NEW1 = hicJepsuService.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
                    if (clsHcVariable.GnWRTNO_NEW1 != 0)
                    {
                        rtnVal = clsHcVariable.GnWRTNO_NEW1;
                    }
                    else
                    {
                        rtnVal = 0;
                    }
                    break;
                case "6":
                    strChasu = "1";
                    strCode2 = "5";
                    clsHcVariable.GnWRTNO_NEW1 = hicJepsuService.Read_Jepsu_Wrtno2(nPano, strJepDate, strGjYear, strChasu, strCode2);
                    if (clsHcVariable.GnWRTNO_NEW1 != 0)
                    {
                        rtnVal = clsHcVariable.GnWRTNO_NEW1;
                    }
                    else
                    {
                        rtnVal = 0;
                    }
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 의사 당일 스케쥴 조회(상담)
        /// </summary>
        /// <param name="argDrCode"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public string READ_DOCTOR_SCH(string argDrCode, string argDate)
        {
            string rtnVal = "";

            //진료여부를 READ
            BAS_SCHEDULE list = basScheduleService.Read_Schedule(argDate, argDrCode);

            //토요일은 오전근무만 체크
            if (list.GBDAY == "2")
            {
                if (list.GBJIN != "1" && list.GBJIN != "2")
                {
                    rtnVal = "NO";
                }
            }
            else if (list.GBDAY == "3")
            {
                rtnVal = "NO";
            }
            else
            {
                if (list.GBJIN == "1" || list.GBJIN2 == "1")
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "2" || list.GBJIN2 == "2")
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "B" || list.GBJIN2 == "B")   //출장검진
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "C" || list.GBJIN2 == "C")   //채용상담
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "NO";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 의사 당일 스케쥴 조회(상담)
        /// </summary>
        /// <param name="argDrCode"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public string READ_DOCTOR_SCH2(string argDrCode, string argDate)
        {
            string rtnVal = "";

            //진료여부를 READ
            BAS_SCHEDULE list = basScheduleService.Read_Schedule(argDate, argDrCode);

            if (list.IsNullOrEmpty())
            {
                return rtnVal;
            }

            //토요일은 오전근무만 체크
            if (list.GBDAY == "3")
            {
                rtnVal = "NO";
            }
            else
            {
                if (list.GBJIN == "1" || list.GBJIN2 == "1")
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "2" || list.GBJIN2 == "2")
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "4" || list.GBJIN2 == "4")   //휴진
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "B" || list.GBJIN2 == "B")   //출장검진
                {
                    rtnVal = "";
                }
                else if (list.GBJIN == "C" || list.GBJIN2 == "C")   //채용상담
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "NO";
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// READ_SUNAPDTL_계산()
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <returns></returns>
        public string READ_SUNAPDTL_Calculator(long argWrtNo)
        {
            string rtnVal = "";

            clsHcVariable.GnAmt_Misu_BonAmt1 = 0;
            clsHcVariable.GnAmt_Misu_LtdAmt1 = 0;
            clsHcVariable.GnAmt_Misu_JohapAmt1 = 0;
            clsHcVariable.GnAmt_Misu_GamAmt1 = 0;
            clsHcVariable.GnAmt_Misu_BogenAmt1 = 0;

            clsHcVariable.GnAmt_Misu_BonAmt2 = 0;
            clsHcVariable.GnAmt_Misu_LtdAmt2 = 0;
            clsHcVariable.GnAmt_Misu_JohapAmt2 = 0;
            clsHcVariable.GnAmt_Misu_GamAmt2 = 0;
            clsHcVariable.GnAmt_Misu_BogenAmt2 = 0;

            //수납내역을 합산 후 본인부담, 회사부담, 조합부담, 감액내역에 저장
            HIC_SUNAP list = hicSunapService.Read_Hic_Sunap(argWrtNo);

            if (list != null)
            {
                clsHcVariable.GnAmt_Misu_BonAmt1 = list.BONINAMT;
                clsHcVariable.GnAmt_Misu_LtdAmt1 = list.LTDAMT;
                clsHcVariable.GnAmt_Misu_JohapAmt1 = list.JOHAPAMT;
                clsHcVariable.GnAmt_Misu_GamAmt1 = list.HALINAMT;
                clsHcVariable.GnAmt_Misu_BogenAmt1 = list.BOGENAMT;
            }

            //지정된 부담율에 따라 보건소금액과 회사 실 부담액을 산정
            List<HIC_SUNAPDTL> list2 = hicSunapdtlService.Read_GbSelf(argWrtNo);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    rtnVal = "OK";
                    switch (list2[i].GBSELF)
                    {
                        case "2":
                            clsHcVariable.GnAmt_Misu_LtdAmt2 += list2[i].AMT;
                            break;
                        case "5":
                            clsHcVariable.GnAmt_Misu_JohapAmt2 += list2[i].AMT * 0.5;
                            clsHcVariable.GnAmt_Misu_LtdAmt2 += list2[i].AMT * 0.5;
                            break;
                        case "6":
                            clsHcVariable.GnAmt_Misu_LtdAmt2 += list2[i].AMT * 0.5;
                            clsHcVariable.GnAmt_Misu_BonAmt2 += list2[i].AMT * 0.5;
                            break;
                        case "10":
                            clsHcVariable.GnAmt_Misu_JohapAmt2 += list2[i].AMT * 0.9;
                            clsHcVariable.GnAmt_Misu_LtdAmt2 += list2[i].AMT * 0.1;
                            break;
                        case "11":
                            clsHcVariable.GnAmt_Misu_BogenAmt2 += list2[i].AMT;
                            break;
                        case "12":
                            clsHcVariable.GnAmt_Misu_JohapAmt2 += list2[i].AMT * 0.9;
                            clsHcVariable.GnAmt_Misu_BogenAmt2 += list2[i].AMT * 0.1;
                            break;
                        default:
                            break;
                    }
                }
                clsHcVariable.GnAmt_Misu_LtdAmt2 = clsHcVariable.GnAmt_Misu_LtdAmt2 - clsHcVariable.GnAmt_Misu_GamAmt1; //회사부담에 감액제외
            }
            return rtnVal;
        }

        /// <summary>
        /// READ_SUNAPDTL_보건소
        /// </summary>
        /// <param name="argWrtNo"></param>
        public void READ_SUNAPDTL_PublicHealth(long argWrtNo)
        {
            clsHcVariable.GnAmt_Misu_BonAmt1 = 0;
            clsHcVariable.GnAmt_Misu_LtdAmt1 = 0;
            clsHcVariable.GnAmt_Misu_JohapAmt1 = 0;
            clsHcVariable.GnAmt_Misu_GamAmt1 = 0;
            clsHcVariable.GnAmt_Misu_BogenAmt1 = 0;

            //수납내역을 합산 후 본인부담, 회사부담, 조합부담, 감액내역에 저장
            HIC_SUNAP list = hicSunapService.Read_Hic_Sunap(argWrtNo);

            if (list != null)
            {
                clsHcVariable.GnAmt_Misu_BonAmt1 = list.BONINAMT;
                clsHcVariable.GnAmt_Misu_LtdAmt1 = list.LTDAMT;
                clsHcVariable.GnAmt_Misu_JohapAmt1 = list.JOHAPAMT;
                clsHcVariable.GnAmt_Misu_GamAmt1 = list.HALINAMT;
                clsHcVariable.GnAmt_Misu_BogenAmt1 = list.BOGENAMT;
            }
        }

        public void KProcess(string PName)
        {
            //Dim pgm As String
            //Dim wmi As Object
            //Dim processes, process
            //Dim sQuery As String

            //pgm = PName
            //Set wmi = GetObject("winmgmts:")
            //sQuery = "select * from win32_process where name='" & pgm & "'"
            //Set processes = wmi.ExecQuery(sQuery)

            //For Each process In processes
            //        process.Terminate
            //Next

            //Set wmi = Nothing
        }

        public int Smt_Cnt_Chek(long argWrtNo)
        {
            int rtnVal = 0;
            string strTemp = "";
            List<string> strExCode = new List<string>();

            strExCode.Clear();
            strExCode.Add("TX45");
            strExCode.Add("TX46");
            strExCode.Add("TX47");
            strExCode.Add("TX48");
            strExCode.Add("TX49");

            strTemp = hicResultService.Read_ExCode(argWrtNo, strExCode);

            switch (strTemp)
            {
                case "TX45":
                    rtnVal = 1; //1-3개
                    break;
                case "TX46":
                    rtnVal = 2; //4-6개
                    break;
                case "TX47":
                    rtnVal = 3; //7-9개
                    break;
                case "TX48":
                    rtnVal = 4; //10-12개
                    break;
                case "TX49":
                    rtnVal = 5; //13개 이상
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        public int Col_Cnt_Check(long argWrtNo)
        {
            int rtnVal = 0;
            string strTemp = "";
            List<string> strExCode = new List<string>(); //{ "TX70", "TX71", "TX72", "TX73", "TX74" };

            strExCode.Clear();
            strExCode.Add("TX70");
            strExCode.Add("TX71");
            strExCode.Add("TX72");
            strExCode.Add("TX73");
            strExCode.Add("TX74");

            strTemp = hicResultService.Read_ExCode(argWrtNo, strExCode);

            switch (strTemp)
            {
                case "TX70":
                    rtnVal = 1;
                    break;
                case "TX71":
                    rtnVal = 2;
                    break;
                case "TX72":
                    rtnVal = 3;
                    break;
                case "TX73":
                    rtnVal = 4;
                    break;
                case "TX74":
                    rtnVal = 5;
                    break;
                default:
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 자궁경부암-편평상피세포이상
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <returns></returns>
        public double READ_Womb_Check_New(long argWrtNo)
        {
            double rtnVal = 0;

            //hrremark1 9,1 ="1" 일반, 10,1 ="1" 고위험
            rtnVal = VB.Val(hicResultService.Read_CervicalCancer(argWrtNo));

            return rtnVal;
        }

        public string READ_SNAME_WRTNO(long argWrtNo)
        {
            string rtnVal = "";

            rtnVal = hicJepsuService.Read_SName(argWrtNo);

            return rtnVal;
        }

        /// <summary>
        /// 건진종류로 검진통계분류 찾기
        /// 0.분류오류 1.공단검진 2.암검진 3.학생검진 4.기타검진 5.측정 6.대행 7.종검
        /// </summary>
        /// <param name="argGjJong"></param>
        /// <returns></returns>
        public int GET_GjJong_2_TongBun(string argGjJong)
        {
            int rtnVal = 0;
            int nJong = 0;

            if (argGjJong == "" || argGjJong.Length != 2)
            {
                rtnVal = 0;
                return rtnVal;
            }

            nJong = 0;
            switch (argGjJong)
            {
                case "13":
                case "18":
                case "43":
                case "46":
                    nJong = 1;  //공단검진
                    break;
                case "12":
                case "17":
                case "42":
                case "45":
                    nJong = 1;  //공무원
                    break;
                case "81":
                    nJong = 6;  //작업환경측정
                    break;
                case "82":
                    nJong = 7;  //보건관리대행
                    break;
                case "83":
                    nJong = 9;  //종합검진
                    break;
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                    nJong = 2;  //암검진
                    break;
                case "56":
                    nJong = 3;  //학생검진
                    break;
                case "52":
                case "53":
                case "54":
                case "55":
                case "57":
                case "58":
                case "59":
                case "60":
                case "61":
                case "63":
                case "64":
                case "65":
                case "66":
                case "67":
                case "68":
                case "70":
                case "71":
                case "72":
                case "73":
                case "74":
                case "75":
                case "76":
                case "77":
                case "78":
                case "79":
                case "80":
                    nJong = 4;  //기타
                    break;
                default:
                    nJong = 1;  //사업장
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 건진 자료사전 코드명칭 읽기
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public string READ_HIC_BCODE_Name(string argGubun, string argName)
        {
            string rtnVal = "";

            rtnVal = hicBcodeService.Read_Code(argGubun, argName);

            return rtnVal;
        }

        /// <summary>
        /// 건진 자료사전 코드명칭 읽기(코드조건)
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_HIC_BCODE_NamebyCode(string argGubun, string argCode)
        {
            string rtnVal = "";

            rtnVal = hicBcodeService.Read_Code2(argGubun, argCode);

            return rtnVal;
        }

        /// <summary>
        /// 건진 자료사전 모든 자료 읽기
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_HIC_BCODE_Data(string argGubun, string argCode)
        {
            string rtnVal = "";
            string strData = "";

            List<HIC_BCODE> list = hicBcodeService.Read_Code_All(argGubun, argCode);

            for (int i = 0; i < list.Count; i++)
            {
                strData += list[i].NAME + ",";
            }

            rtnVal = strData;

            return rtnVal;
        }

        /// <summary>
        /// 해당 날짜가 휴일인지 Check 함
        /// true: 휴일  false: 휴일이 아님
        /// </summary>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public bool HIC_DATE_HUIL_Check(string argDate)
        {
            bool rtnVal = false;
            string strTemp = "";

            //선거일(임시공휴일인데 근무함)
            //if (argDate == "2014-06-04") return rtnVal;

            strTemp = comHpcLibBService.Read_HolyDay(argDate);

            if (strTemp == "*")
            {
                rtnVal = true;
            }
            else
            {
                rtnVal = false;
            }

            return rtnVal;
        }

        public void GET_Monitor_Setting()
        {
            long currHRes = 0;
            long currVRes = 0;
            long hDC = 0;

            const long HORZRES = 8;
            const long VERTRES = 10;

            //get the system settings
            hDC = clsHcVariable.GetDC(clsHcVariable.GetDesktopWindow());
            currHRes = clsHcVariable.GetDeviceCaps(hDC, HORZRES);
            currVRes = clsHcVariable.GetDeviceCaps(hDC, VERTRES);

            clsHcVariable.GstrMonitorSize = string.Format("{0:#0}", currHRes) + "x" + string.Format("{0:#0}", currVRes);
        }

        /// <summary>
        /// READ_1차접수번호() : 2차 접수번호로 1차 접수번호 찾기
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argJepDate"></param>
        /// <param name="argGjJong"></param>
        /// <param name="argGjYer"></param>
        /// <returns></returns>
        public long READ_FirstJepsuNo(long argPano, string argJepDate, string argGjJong, string argGjYer)
        {
            long rtnVal = 0;
            string strTemp = "";

            switch (argGjJong)
            {
                case "16":
                case "17":
                case "18":
                case "19":
                    strTemp = "'11','12','13','14'";        //일반2차
                    break;
                case "27":
                    strTemp = "'21','22'";                  //일반채용2차
                    break;
                case "28":
                    strTemp = "'22','23','24','25','26'";   //특수2차
                    break;
                case "33":
                    strTemp = "'22','24','25','26'";        //배치전2차
                    break;
                case "44":
                case "45":
                case "46":
                    strTemp = "'41','42','43'";             //생애2차
                    break;
                case "50":
                    strTemp = "'49','51'";                  //방사선종사자2차
                    break;
                default:
                    strTemp = "-1";
                    break;
            }

            rtnVal = hicJepsuService.Read_Jepsu_WrtNo(argPano, argJepDate, argGjYer);

            return rtnVal;
        }

        public string READ_HIC_InsaName(string argSabun)
        {
            string rtnVal = "";
            string strSabun1 = "";
            string strTemp = "";

            if (VB.Val(argSabun) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            strSabun1 = string.Format("{0:#00000}", argSabun);

            strTemp = comHpcLibBService.Read_Hic_Doctor_Name(argSabun);
            if (strTemp == null)
            {
                rtnVal = "";
                switch (argSabun)
                {
                    case "4444":
                        rtnVal = "영양실";
                        break;
                    case "111":
                        rtnVal = "일반건진";
                        break;
                    case "222":
                        rtnVal = "종합건진";
                        break;
                    case "333":
                        rtnVal = "기록실";
                        break;
                    case "555":
                        rtnVal = "예약자부도";
                        break;
                    case "2222":
                        rtnVal = "HD수납";
                        break;
                    case "123":
                        rtnVal = "전화공용";
                        break;
                    case "500":
                        rtnVal = "외래상담용";
                        break;
                    case "4349":
                        rtnVal = "전산정보팀";
                        break;
                    case "04349":
                        rtnVal = "전산정보팀";
                        break;
                    case "6666":
                        rtnVal = "진료의뢰";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                rtnVal = strTemp;
            }

            return rtnVal;
        }

        /// <summary>
        /// 웹결과지 승인번호 6자리 생성
        /// </summary>
        /// <returns></returns>
        public string GET_WebPrt_SendNo()
        {
            string rtnVal = "";
            string strSendNo = "";
            string strTemp = "";

            while (true)
            {
                strSendNo = GET_WebPrt_SendNo_SUB();
                strTemp = comHpcLibBService.Read_WebPrt_Log(strSendNo);

                if (strTemp == null)
                {
                    rtnVal = strSendNo;
                    return rtnVal;
                }
                else if (strTemp == "-1")   ///TODO : 이상훈 (2019.07.29) : 오류 발생에 대한 처리 방법 확인
                {
                    return rtnVal;
                }
                Thread.Sleep(600);
            }
        }

        public string GET_WebPrt_SendNo_SUB()
        {
            string rtnVal = "";
            double nSum = 0;

            nSum = (VB.Val(VB.Left(DateTime.Now.ToString("hh:MM"), 2)) * 10000) + (VB.Val(VB.Right(DateTime.Now.ToString("hh:MM"), 2)) * 10000);
            nSum = nSum + (VB.Val(VB.Mid(DateTime.Now.ToString("hh:MM"), 4, 2)) * 100);
            nSum = nSum + VB.Val(VB.Right(DateTime.Now.ToString("hh:MM"), 2));
            nSum = nSum + (VB.Val(VB.Left(clsPublic.GstrSysDate, 4)) * 100) + (VB.Val(VB.Mid(clsPublic.GstrSysDate, 6, 2)) * 10000) + (VB.Val(VB.Right(clsPublic.GstrSysDate, 2)) * 100);

            rtnVal = VB.Right(string.Format("{0:#000000}", nSum), 6);

            return rtnVal;
        }

        public string IEMunjin_Name_Display(string argRecvForm)
        {
            string rtnVal = "";
            string strResult = "";

            if (VB.InStr(argRecvForm, "12001") > 0) strResult += "공통,";
            if (VB.InStr(argRecvForm, "12003") > 0) strResult += "암,";
            if (VB.InStr(argRecvForm, "12005") > 0) strResult += "구강,";
            if (VB.InStr(argRecvForm, "12006") > 0) strResult += "생애,";
            if (VB.InStr(argRecvForm, "12010") > 0) strResult += "초등,";
            if (VB.InStr(argRecvForm, "12014") > 0) strResult += "구강,";
            if (VB.InStr(argRecvForm, "12020") > 0) strResult += "중고등,";
            if (VB.InStr(argRecvForm, "20002") > 0) strResult += "특수,";
            if (VB.InStr(argRecvForm, "20003") > 0) strResult += "폐활,";
            if (VB.InStr(argRecvForm, "20004") > 0) strResult += "흡연,";
            if (VB.InStr(argRecvForm, "20005") > 0) strResult += "음주,";
            if (VB.InStr(argRecvForm, "20006") > 0) strResult += "운동,";
            if (VB.InStr(argRecvForm, "20007") > 0) strResult += "영양,";
            if (VB.InStr(argRecvForm, "20008") > 0) strResult += "비만,";
            if (VB.InStr(argRecvForm, "20009") > 0) strResult += "우울,";
            if (VB.InStr(argRecvForm, "20010") > 0) strResult += "우울,";
            if (VB.InStr(argRecvForm, "20011") > 0) strResult += "인지,";
            if (VB.InStr(argRecvForm, "20012") > 0) strResult += "정신,";
            if (VB.InStr(argRecvForm, "30001") > 0) strResult += "야간,";
            if (VB.InStr(argRecvForm, "30003") > 0) strResult += "야간,";

            rtnVal = strResult;

            return rtnVal;
        }

        public bool HIC_Huil_GasanDay(string argDate)
        {
            bool rtnVal = false;
            string strYoil = "";
            string strTemp = "";

            //2017-01-01부터 토요일,공휴일 30&가산 적용
            //토요일, 일요일이면 공휴일
            strYoil = CF.READ_YOIL(clsDB.DbCon, argDate);

            if (strYoil == "토요일" || strYoil == "일요일")
            {
                rtnVal = true;
                return rtnVal;
            }

            //BAS_JOB 일요일,공휴일 여부를 읽음
            strTemp = comHpcLibBService.Read_HolyDay(argDate);

            if (strTemp == "*")
            {
                rtnVal = true;
                return rtnVal;
            }

            strTemp = "";
            //건강증진센타 자료사전의 대체휴일 여부을 읽음
            strTemp = hicBcodeService.Read_Code_One("HIC_공단검진임시휴일가산", argDate);

            if (strTemp != null)
            {
                rtnVal = true;
                return rtnVal;
            }
            return rtnVal;
        }

        /// <summary>
        /// 건진년도별 구강금액
        /// </summary>
        /// <param name="argYear"></param>
        public void SET_Dental_Amt(string argYear)
        {
            if (string.Compare(argYear, "2020") >= 0)
            {
                clsHcVariable.GnDentAmt = 7450;
                clsHcVariable.GnDentAddAmt = 2240;
            }
            else if (string.Compare(argYear, "2019") >= 0)
            {
                clsHcVariable.GnDentAmt = 7240;
                clsHcVariable.GnDentAddAmt = 2170;
            }
            else if (string.Compare(argYear, "2018") >= 0)
            {
                clsHcVariable.GnDentAmt = 7060;
                clsHcVariable.GnDentAddAmt = 2120;
            }
            else if (string.Compare(argYear, "2017") >= 0)
            {
                clsHcVariable.GnDentAmt = 6860;
                clsHcVariable.GnDentAddAmt = 2060;
            }
            else if (string.Compare(argYear, "2016") >= 0)
            {
                clsHcVariable.GnDentAmt = 6650;
                clsHcVariable.GnDentAddAmt = 0;
            }
            else if (string.Compare(argYear, "2015") >= 0)
            {
                clsHcVariable.GnDentAmt = 6460;
                clsHcVariable.GnDentAddAmt = 0;
            }
            else if (string.Compare(argYear, "2014") >= 0)
            {
                clsHcVariable.GnDentAmt = 6270;
                clsHcVariable.GnDentAddAmt = 0;
            }
            else if (string.Compare(argYear, "2013") >= 0)
            {
                clsHcVariable.GnDentAmt = 6080;
                clsHcVariable.GnDentAddAmt = 0;
            }
            else
            {
                clsHcVariable.GnDentAmt = 5950;
                clsHcVariable.GnDentAddAmt = 0;
            }
        }

        /// <summary>
        /// 차트인계 목록명을 찾기
        /// </summary>
        /// <param name="argTrList"></param>
        /// <returns></returns>
        public string GET_TrList_Name(string argTrList)
        {
            string rtnVal = "";
            string strTemp = "";

            if (VB.Mid(argTrList, 5, 1) == "Y") strTemp += "문진표,";
            if (VB.Mid(argTrList, 2, 1) == "Y") strTemp += "특수표지,";
            if (VB.Mid(argTrList, 3, 1) == "Y") strTemp += "청력차트,";
            if (VB.Mid(argTrList, 4, 1) == "Y") strTemp += "내시경동의서,";
            if (VB.Mid(argTrList, 6, 1) == "Y") strTemp += "1차,";

            if (VB.Right(strTemp, 1) == ",") strTemp = VB.Left(strTemp, strTemp.Length - 1);

            rtnVal = strTemp;

            return rtnVal;
        }

        public void INSERT_JOB_LOG(PsmhDb pDbCon, string argExe, long argWrtNo, string argJobLog, string argCommit = "")
        {

            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (argCommit != "N")
            {
                clsDB.setBeginTran(pDbCon);
            }

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "HIC_JOB_LOG ( JOBTIME,JOBSABUN,JOBEXE,WRTNO,JOBLOG                         ";
            SQL += ComNum.VBLF + " ) VALUES ( SYSDATE ," + clsType.User.IdNumber + ", '" + argExe + "', " + argWrtNo + ", '" + argJobLog + "' ) ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                if (argCommit != "N")
                {
                    clsDB.setRollbackTran(pDbCon);
                }
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (argCommit != "N")
            {
                clsDB.setCommitTran(pDbCon);
            }
        }

        /// <summary>
        /// READ_DRCODE1() Merge
        /// </summary>
        /// <param name="argSabun"></param>
        public void Read_DrCode(long argSabun)
        {
            if (argSabun == 0) return;

            COMHPC DrInfo = comHpcLibBService.Read_Ocs_Doctor_All(argSabun);

            if (!DrInfo.IsNullOrEmpty())
            {
                clsHcVariable.GstrHicDrName = DrInfo.DRNAME.To<string>("").Trim(); //판정의 성명
                clsHcVariable.GnHicLicense1 = DrInfo.DRBUNHO.To<long>(0); //판정의 면허번호
            }
            else
            {
                //유방촬영 외주판독의사 관련 추가
                //if (argSabun == 99007)
                //{
                //    clsHcVariable.GstrHicDrName = "임효진"; //판정의 성명
                //    clsHcVariable.GnHicLicense1 = 79507; //판정의 면허번호
                //}
                //else if (argSabun == 99012)
                //{
                //    clsHcVariable.GstrHicDrName = "최선형"; //판정의 성명
                //    clsHcVariable.GnHicLicense1 = 72532; //판정의 면허번호
                //}

                BAS_BCODE item = basBcodeService.GetAllByGubunCode("XRAY_외주판독의사", argSabun.ToString());
                if (!item.IsNullOrEmpty())
                {
                    clsHcVariable.GstrHicDrName = item.NAME; //판정의 성명
                    clsHcVariable.GnHicLicense1 = Convert.ToInt32(item.GUBUN2); //판정의 면허번호
                }

                //해부병리의사
                if (argSabun == 55303)
                {
                    clsHcVariable.GstrHicDrName = "김미진"; //판정의 성명
                    clsHcVariable.GnHicLicense1 = 30846; //판정의 면허번호
                }
                if (argSabun == 53784)
                {
                    clsHcVariable.GstrHicDrName = "박미옥"; //판정의 성명
                    clsHcVariable.GnHicLicense1 = 54498; //판정의 면허번호
                }


            }
        }

        /// <summary>
        /// READ_DRCODE1() Merge
        /// </summary>
        /// <param name="argSabun"></param>
        public void Read_DrCode(string argDrCode)
        {
            if (argDrCode.IsNullOrEmpty()) return;

            COMHPC DrInfo = comHpcLibBService.Read_Ocs_Doctor_DrCode(argDrCode);

            if (!DrInfo.IsNullOrEmpty())
            {
                clsHcVariable.GstrHicDrName = DrInfo.DRNAME.To<string>("").Trim(); //판정의 성명
                clsHcVariable.GnHicLicense1 = DrInfo.DRBUNHO.To<long>(0); //판정의 면허번호
            }
        }

        public string Read_SPC_PANJENG(long argWrtNo)
        {
            string rtnVal = "";
            string strTemp = "";

            if (argWrtNo == 0) return rtnVal;

            strTemp = hicSpcPanjengService.Read_Spc_Panjeng_YN(argWrtNo);

            if (strTemp != null)
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        public long Read_WRTNO(string argPano, string argYear)
        {
            long rtnVal = 0;

            if (argPano == "0") return rtnVal;

            rtnVal = hicJepsuService.Read_Jepsu_Wrtno3(argPano, argYear);

            return rtnVal;
        }

        /// <summary>
        /// 2대상 추가검사 자동세팅
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string READ_HICCODE(string argCode)
        {
            string rtnVal = "";
            string strCode = "";
            //string argSQL = "";
            List<string> argSQL = new List<string>();
            string strTemp = "";

            if (argCode == "") return rtnVal;

            for (int i = 0; i < VB.L(argCode, ","); i++)
            {
                if (VB.Pstr(argCode, ",", i) != "")
                {
                    //argSQL += VB.Pstr(argCode, ",", i) + ",";
                    argSQL.Add(VB.Pstr(argCode, ",", i));
                }
            }

            //마지막 컴마를 짜름
            //argSQL = VB.Left(argSQL, argSQL.Length - 1);

            List<HIC_CODE> list = hicCodeService.Read_Hic_Code_All("53", argSQL);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strTemp += list[i].GCODE + ",";
                }
                strTemp = VB.Left(strTemp, strTemp.Length - 1);

                for (int i = 0; i < VB.L(strTemp, ","); i++)
                {
                    if (VB.Pstr(strTemp, ",", i) != "")
                    {
                        strCode += VB.Pstr(strTemp, ",", i) + ",";
                    }
                }

                strCode = VB.Left(strCode, strCode.Length - 1);
                rtnVal = strCode;
            }

            return rtnVal;
        }

        public string READ_BILL_DRCODE(string argSabun)
        {
            string rtnVal = "";

            if (argSabun == "") return rtnVal;

            rtnVal = comHpcLibBService.Read_DrCode(argSabun);

            return rtnVal;
        }

        public string READ_BILL_JUMIN(string argSabun)
        {
            string rtnVal = "";

            if (argSabun == "") return rtnVal;

            rtnVal = comHpcLibBService.GetJumin3byMyen_Bunho(argSabun);

            return rtnVal;
        }

        public string READ_EXID_Name(long argWrtNo, string argXrayNo)
        {
            string rtnVal = "";

            if (argWrtNo == 0) return rtnVal;

            rtnVal = comHpcLibBService.Read_Exid_Name(argXrayNo);

            return rtnVal;
        }

        /// <summary>
        /// 병원번호 마지막에 체크디지트 부여
        /// </summary>
        /// <param name="strPano"></param>
        /// <returns></returns>
        public string PANO_LAST_CHAR(string argPano)
        {
            string rtnVal = "";
            int i = 0;
            int j = 0;
            //double Sum = 0;
            //double na = 0;
            //double mok = 0;
            //double c = 0;
            int Sum = 0;
            int na = 0;
            int mok = 0;
            int c = 0;

            i = 7;
            Sum = 0;
            for (j = 1; j <= 7; j++)
            {
                //Sum += VB.Val(VB.Mid(argPano, j, 1)) * i;
                Sum += Convert.ToInt32(VB.Val(VB.Mid(argPano, j, 1))) * i;
                i -= 1;
            }

            mok = (Sum / 11);
            na = Sum - (mok * 11);
            c = 11 - na;

            if (c == 10 || c == 11)
            {
                c = 0;
            }

            rtnVal = argPano + string.Format("{0:0}", c);

            return rtnVal;
        }

        /// <summary>
        /// 주민등록번호로 BAS_PATIENT의 환자 성명을 읽음
        /// </summary>
        /// <param name="argJumin"></param>
        /// <returns></returns>
        public string READ_JUMIN_SName(string argJumin)
        {
            string rtnVal = "";

            if (argJumin.Length != 13)
            {
                rtnVal = "";
            }

            rtnVal = comHpcLibBService.Read_SName(VB.Left(argJumin, 6), clsAES.AES(VB.Right(argJumin, 7)));

            return rtnVal;
        }

        /// <summary>
        /// 검진나이계산 - 만나이로 계산 생일무시함(YYYY-MM-DD Type)
        /// </summary>
        /// <param name="argJumin"></param>
        /// <returns></returns>
        public double READ_HIC_AGE_GESAN2(string argJumin)
        {
            double rtnVal = 0;

            string strGbn = "";          // 구분
            string strJumin = "";        //주민번호
            string strBirth = "";   //생년월일
            double nYear = 0;       //생년일
            double nJYear = 0;      //검진년도

            rtnVal = 999;
            strJumin = argJumin;
            strGbn = VB.Mid(argJumin, 7, 1);
            nJYear = VB.Val(VB.Left(clsPublic.GstrSysDate, 4));

            //생년월일을 YYYY-MM-DD Type으로 변경
            if (strGbn == "1" || strGbn == "2" || strGbn == "5" || strGbn == "6")       // 한국인 남1 녀2, 외국인 남5 녀6
            {
                strBirth = "19" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else if (strGbn == "3" || strGbn == "4" || strGbn == "7" || strGbn == "8")   // 한국인 남3 녀4 , 외국인 남7 녀8
            {
                strBirth = "20" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else if (strGbn == "0" || strGbn == "9")
            {
                strBirth = "18" + VB.Left(argJumin, 2) + "-" + VB.Mid(argJumin, 3, 2) + "-" + VB.Mid(argJumin, 5, 2);
            }
            else
            {
                rtnVal = 999;
            }

            nYear = VB.Val(VB.Left(strBirth, 4));

            rtnVal = nJYear - nYear;

            return rtnVal;
        }

        /// <summary>
        /// 특정사번 해당일자 휴일(Off)여부 점검
        /// </summary>
        /// <param name="argSabun"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        public bool Check_Sabun_Huil(string argSabun, string argDate)
        {
            bool rtnVal = false;
            string strSaBun = "";
            string strYear = "";
            string strToiDay = "";
            string strGunTae = "";
            string strDate = "";
            int nilsu = 0;
            bool BOK = false;
            string strDrCode = "";

            strYear = VB.Left(argDate, 4);
            strSaBun = string.Format("{0:00000}", argSabun);

            //퇴사일자를 읽어 퇴사일자 이후이면 False 처리
            strToiDay = comHpcLibBService.Read_ToisaDay(strSaBun);

            if (strToiDay != "")
            {
                if (string.Compare(strToiDay, argDate) > 0)
                {
                    rtnVal = false;
                    return rtnVal;
                }
            }

            //해당일자에 휴가,교육,.. 인지 점검
            strGunTae = comHpcLibBService.Read_GunTae(strSaBun, strYear);

            nilsu = CF.DATE_ILSU(clsDB.DbCon, argDate, strYear + "01-01") + 1;
            strDate = VB.Mid(strGunTae, nilsu, 1);
            BOK = false;

            switch (strDate)
            {
                case "A":
                    BOK = true; //휴가 
                    break;
                case "B":
                    BOK = true; //월차 
                    break;
                case "C":
                    BOK = true; //특일
                    break;
                case "D":
                    BOK = true; //교육 
                    break;
                case "E":
                    BOK = true; //출장 
                    break;
                case "F":
                    BOK = true; //병가 
                    break;
                case "G":
                    BOK = true; //분만 
                    break;
                case "H":
                    BOK = true; //피정 
                    break;
                case "I":
                    BOK = true; //훈련 
                    break;
                case "J":
                    BOK = true; //생휴 
                    break;
                case "K":
                    BOK = true; //학회 
                    break;
                case "L":
                    BOK = true; //결근 
                    break;
                case "R":
                    BOK = true; //휴직 
                    break;
                default:
                    break;
            }

            //인사에 휴일이지만 근무스케쥴에 반휴이면 휴일이 아님
            if (BOK == true)
            {
                strDrCode = comHpcLibBService.Read_DrCode(strSaBun);

                BAS_SCHEDULE list = basScheduleService.Read_Schedule(argDate, strDrCode);

                if (list.GBJIN == "1") BOK = false;
                if (list.GBJIN2 == "1") BOK = false;
            }

            rtnVal = BOK;

            return rtnVal;
        }

        public string READ_Sabun_Name(string argSabun)
        {
            string rtnVal = "";
            string strSabun = "";

            if (VB.Val(argSabun) == 0)
            {
                rtnVal = "";
                return rtnVal;
            }

            strSabun = string.Format("{0:00000}", VB.Val(argSabun));

            rtnVal = comHpcLibBService.Read_Insa_Mst(strSabun);

            if (rtnVal == "" || rtnVal == null)
            {
                switch (VB.Val(strSabun))
                {
                    case 4444:
                        rtnVal = "영양실";
                        break;
                    case 111:
                        rtnVal = "일반건진";
                        break;
                    case 222:
                        rtnVal = "종합건진";
                        break;
                    case 333:
                        rtnVal = "기록실";
                        break;
                    case 555:
                        rtnVal = "예약자부도";
                        break;
                    case 2222:
                        rtnVal = "HD수납";
                        break;
                    case 123:
                        rtnVal = "전화공용";
                        break;
                    case 500:
                        rtnVal = "외래상담용";
                        break;
                    case 4349:
                        rtnVal = "전산실";
                        break;
                    default:
                        break;
                }
            }

            return rtnVal;
        }

        public string READ_HIC_OcsDrcode(long argSabun)
        {
            string rtnVal = "";

            if (argSabun == 0)
            {
                return rtnVal;
            }

            rtnVal = comHpcLibBService.Read_Ocs_Doctor(argSabun);

            return rtnVal;
        }

        /// <summary>
        /// 근무부서가 건강증진센타인지 점검
        /// </summary>
        /// <param name="argBuCode"></param>
        /// <returns></returns>
        public bool Check_Center_Buse(string argBuCode)
        {
            bool rtnVal = false;

            string code = hicBcodeService.Read_Check_Center_Buse(argBuCode);

            if (code == "" || code == null)
            {
                rtnVal = false;
            }
            else
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 내시경처방 EMR전송
        /// </summary>
        /// <param name="argSabun"></param>
        /// <returns></returns>
        public string OCS_OORDER_INSERT(long argSabun)
        {
            string rtnVal = "";

            if (argSabun == 0) return rtnVal;

            rtnVal = hicBcodeService.Order_Insert(argSabun);

            return rtnVal;
        }

        public Image SIGNATUREFILE_DBToFile(string strSabun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            IDataReader reader = null;
            OracleCommand cmd = null;

            try
            {
                SQL = "";
                SQL = SQL + "\r\n" + "SELECT SABUN, SIGNATURE ";
                SQL = SQL + "\r\n" + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + "\r\n" + "WHERE TRIM(DRCODE) = '" + strSabun + "'";

                cmd = clsDB.DbCon.Con.CreateCommand();
                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                if (reader == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }

                while (reader.Read())
                {
                    byte[] byteArray = (byte[])reader.GetValue(1);
                    MemoryStream memStream = new MemoryStream(byteArray);
                    rtnVAL = Image.FromStream(memStream);
                }
                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }

        public void Update_Audio_Result(long nWrtNo, string strSex)
        {
            double nResult = 0;
            int nExcludeCnt = 0;    //n제외Cnt
            int nCannotCnt = 0;      //n불가Cnt
            int nRead = 0;
            string strResult = "";

            string sExCode = "";

            string[] strExCodes = { "TH11", "TH12", "TH13", "TH15", "TH21", "TH22", "TH23", "TH25" };

            strResult = "정상";
            nExcludeCnt = 0;
            nCannotCnt = 0;

            //2021-02-16(테이블 수정)
            //HIC_RESULT RsltList = hicResultService.Read_Result_YN(nWrtNo, "TH28");
            HEA_RESULT RsltList = heaResultService.Read_Result_YN(nWrtNo, "TH28");

            if (RsltList == null)
            {
                return;
            }

            List<EXAM_DISPLAY> exam_Dsp_List = examDisplayService.Read_Result(nWrtNo, strExCodes);

            //string[] arrResult = exam_Dsp_List.GetStringArray("RESULT");
            //string[] arrMin_M = exam_Dsp_List.GetStringArray("MIN_M");
            //string[] arrMax_M = exam_Dsp_List.GetStringArray("MAX_M");
            //string[] arrMin_F = exam_Dsp_List.GetStringArray("MIN_F");
            //string[] arrMax_F = exam_Dsp_List.GetStringArray("MAX_F");

            nRead = exam_Dsp_List.Count;

            for (int i = 0; i < nRead; i++)
            {
                if (!exam_Dsp_List[i].RESULT.IsNullOrEmpty())
                {
                    if (exam_Dsp_List[i].RESULT == "본인제외")
                    {
                        nExcludeCnt += 1;
                    }
                    else if (exam_Dsp_List[i].RESULT == "측정불가")
                    {
                        nCannotCnt += 1;
                    }
                    else
                    {
                        nResult = exam_Dsp_List[i].RESULT.To<double>(0);
                        if (strSex == "M")
                        {
                            if (nResult < exam_Dsp_List[i].MIN_M.To<double>(0) || nResult > exam_Dsp_List[i].MAX_M.To<double>(0))
                            {
                                strResult = "정밀검사요함";
                            }
                        }
                        else
                        {
                            if (nResult < exam_Dsp_List[i].MIN_F.To<double>(0) || nResult > exam_Dsp_List[i].MAX_F.To<double>(0))
                            {
                                strResult = "정밀검사요함";
                            }
                        }
                    }
                }
                else //한개라도 결과값이 안들어가 있으면 입력 안됨
                {
                    strResult = "NO";
                }
            }

            if (nExcludeCnt > 1) strResult = "정밀검사요함";
            if (nExcludeCnt > 1) strResult = "본인제외";
            if (nCannotCnt > 1) strResult = "측정불가";

            if (strResult != "NO")
            {
                //History에 INSERT
                sExCode = "TH28";

                //2021-02-16(테이블 수정)
                //int result = hicResultHisService.Result_History_Insert2(clsPublic.GstrJobSabun, strResult, nWrtNo, sExCode);
                int result = heaResultHisService.Result_History_Insert2(long.Parse(clsType.User.IdNumber), strResult, nWrtNo, sExCode);

                if (result < 0)
                {
                    MessageBox.Show("검사결과 History 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //2021-02-16(테이블 수정)
                //int result2 = hicResultHisService.Result_History_Update(clsPublic.GstrJobSabun, strResult, nWrtNo, sExCode);
                int result2 = heaResultHisService.Result_History_Update(strResult, long.Parse(clsType.User.IdNumber), nWrtNo, sExCode);

                if (result2 < 0)
                {
                    MessageBox.Show("검사결과 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        /// <summary>
        /// Update_폐활량검사
        /// </summary>
        /// <param name="nWrtNo"></param>
        public void Update_Lung_Capacity(long nWrtNo, string strSex)
        {
            int nResult = 0;
            int nRead = 0;
            int nExcludeCnt = 0;
            string strResult = "";
            string[] strExCodes = { "ZE12", "ZE05", "ZE06", "ZE07" };
            string sExCode = "";

            strResult = "정상";
            nExcludeCnt = 0;

            sExCode = "ZE13";

            //2021-02-16(테이블 수정)
            //HIC_RESULT RsltList = hicResultService.Read_Result_YN(nWrtNo, sExCode);
            HEA_RESULT RsltList = heaResultService.Read_Result_YN(nWrtNo, sExCode);

            if (RsltList == null)
            {
                return;
            }

            List<EXAM_DISPLAY> exam_Dsp_List = examDisplayService.Read_Result(nWrtNo, strExCodes);

            //string[] arrResult = exam_Dsp_List.GetStringArray("RESULT");
            //string[] arrMin_M = exam_Dsp_List.GetStringArray("MIN_M");
            //string[] arrMax_M = exam_Dsp_List.GetStringArray("MAX_M");
            //string[] arrMin_F = exam_Dsp_List.GetStringArray("MIN_F");
            //string[] arrMax_F = exam_Dsp_List.GetStringArray("MAX_F");

            nRead = exam_Dsp_List.Count;

            for (int i = 0; i < nRead; i++)
            {
                if (!exam_Dsp_List[i].RESULT.IsNullOrEmpty())
                {
                    if (exam_Dsp_List[i].RESULT.To<string>("").Trim() == "본인제외")
                    {
                        nExcludeCnt += 1;
                    }
                    else
                    {
                        nResult = exam_Dsp_List[i].RESULT.To<int>(0);
                        if (strSex == "M")
                        {
                            if (nResult < exam_Dsp_List[i].MIN_M.To<double>(0))
                            {
                                strResult = "추적재검";
                            }
                        }
                        else
                        {
                            if (nResult < exam_Dsp_List[i].MIN_F.To<double>(0))
                            {
                                strResult = "추적재검";
                            }
                        }
                    }
                }
                else //한개라도 결과값이 안들어가 있으면 입력 안됨
                {
                    strResult = "NO";
                }
            }

            if (nExcludeCnt == nRead) strResult = "본인제외";

            if (strResult != "NO")
            {
                //History에 INSERT
                sExCode = "ZE13";

                //2021-02-16(테이블 수정)
                //int result = hicResultHisService.Result_History_Insert2(clsPublic.GstrJobSabun, strResult, nWrtNo, sExCode);
                int result = heaResultHisService.Result_History_Insert2(long.Parse(clsType.User.IdNumber), strResult, nWrtNo, sExCode);

                if (result < 0)
                {
                    MessageBox.Show("검사결과 History 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //2021-02-16(테이블 수정)
                int result2 = heaResultHisService.Result_History_Update(strResult, long.Parse(clsType.User.IdNumber), nWrtNo, sExCode);

                if (result2 < 0)
                {
                    MessageBox.Show("검사결과 저장중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public string READ_CHARTTRANS_PRINT(long argWrtNo)
        {
            string rtnVal = "";

            HIC_CHARTTRANS_PRINT list = hicCharttransPrintService.GetJobTimebyWrtNo(argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = "내원" + "\r\n" + VB.Left(list.JOBTIME.ToString(), 10);
            }

            return rtnVal;
        }

        public string Read_UCODES(long argWrtNo)
        {
            string rtnVal = "";

            rtnVal = hicJepsuService.GetUcodesbyWrtNo(argWrtNo);

            return rtnVal;
        }

        public string READ_SUNAPDTL_FAMILY(long argWrtno)
        {
            string rtnVal = "";
            List<string> argCode = new List<string>();

            argCode.Clear();
            argCode.Add("17774");
            argCode.Add("17775");

            if (!heaSunapdtlService.GetRowidByWrtnoCodeIN(argWrtno, argCode).IsNullOrEmpty())
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        public string READ_ANATNO_STOMACH(string argPtno, string argFDate, string argTDate)
        {
            string rtnVal = "";

            COMHPC item = comHpcLibBService.GetResultByPtnoBdate(argPtno, argFDate, argTDate,"위");
            if (!item.IsNullOrEmpty())
            {
                rtnVal = VB.TR(VB.Trim(item.RESULT1) + ComNum.VBLF + VB.Trim(item.RESULT2), ComNum.VBLF, "");
                rtnVal = VB.Pstr(rtnVal, item.DIAGNOSIS, 1) + ComNum.VBLF + ComNum.VBLF + VB.Pstr(rtnVal, item.DIAGNOSIS, 2);
            }

            return rtnVal;
        }
        public string READ_ANATNO_RECTUM(string argPtno, string argFDate, string argTDate)
        {
            string rtnVal = "";

            COMHPC item = comHpcLibBService.GetResultByPtnoBdate(argPtno, argFDate, argTDate,"대장");
            if (!item.IsNullOrEmpty())
            {
                rtnVal = VB.TR(VB.Trim(item.RESULT1) + ComNum.VBLF + VB.Trim(item.RESULT2), ComNum.VBLF, "");
                rtnVal = VB.Pstr(rtnVal, item.DIAGNOSIS, 1) + ComNum.VBLF + ComNum.VBLF + VB.Pstr(rtnVal, item.DIAGNOSIS, 2);
            }

            return rtnVal;
        }

        public string READ_HEA_EKG_RESULT(long argWrtno)
        {
            string rtnVal = "";

            rtnVal= heaEkgResultService.GetResultbyWrtNo(argWrtno);

            return rtnVal;
        }

        public string READ_XRAY_RESULT(string argPtno, string argFDate, string argTDate, string argCode)
        {
            string rtnVal = "";

            COMHPC item = comHpcLibBService.GetItemByXrayResult(argPtno, argFDate, argTDate, argCode);
            if (!item.IsNullOrEmpty())
            {
                rtnVal = VB.TR(VB.Trim(item.RESULT) + ComNum.VBLF + VB.Trim(item.RESULT1), ComNum.VBLF, "");  
            }

            return rtnVal;
        }

        public void HIC_RES_ETC_INSERT(long nWrtno, string strGubun,string strGjjong)
        {
            int nREAD = 0;
            int result = 0;

            string strJepDate = "";

            HIC_JEPSU item1 = hicJepsuService.GetItembyWrtNo(nWrtno);
            if (!item1.IsNullOrEmpty())
            {
                HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(nWrtno, strGubun);

                if (item.IsNullOrEmpty())
                {
                    result = hicResEtcService.Insert(nWrtno, item1.JEPDATE, strGubun);
                    if (result < 0)
                    {
                        MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}
