using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


/// <summary>
/// Class Name      : HC_Print
/// File Name       : frmHcPrint_Dental.cs
/// Description     : 혈액종합판정 결과지출력
/// Author          : 김경동
/// Create Date     : 2021-07-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= " Frm혈액종합판정결과지.frm(Frm혈액종합판정)" />
/// 

namespace HC_Print
{

    public partial class frmHaPrint_HanSuWon : Form
    {
        List<string> strExcode = new List<string>();

        string strOK = "";
        string fstrLtd = "";
        string fstrFDate ="";
        string fstrTDate ="";

        HeaJepsuResultPatientService heaJepsuResultPatientService = null;
        XrayResultnewService xrayResultnewService = null;
        HeaResultService heaResultService = null;
        HicExcodeService hicExcodeService = null;

        ComFunc CF = null;
        clsHaBase cHB = null;

        public frmHaPrint_HanSuWon()
        {
            InitializeComponent();
            SetEvent();
            SetControl();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        private void SetControl()
        {

            cHB = new clsHaBase();
            CF = new ComFunc();


            heaJepsuResultPatientService = new HeaJepsuResultPatientService();
            xrayResultnewService = new XrayResultnewService();
            heaResultService = new HeaResultService();
            hicExcodeService = new HicExcodeService();
        }




        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(-60).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            cboLTD.Items.Clear();
            cboLTD.Items.Add("0951.한국수력원자력(주)방사선보건");
            cboLTD.Items.Add("3858.한전케이피에스(주)울진사업처");
            cboLTD.Items.Add("2177.포스코건설");
        }


        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
                btnExcel.Enabled = true;

            }
            else if ( sender == btnExcel)
            {
                Spread_Save();
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display()
        {
            long nGubun = 0;

            if (cboLTD.Text.IsNullOrEmpty())
            {
                MessageBox.Show("회사를 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                return;
            }

            fstrLtd = VB.Pstr(cboLTD.Text, ".", 1);
            fstrFDate = dtpFDate.Text;
            fstrTDate = dtpTDate.Text;

            nGubun = tabMain.SelectedTabIndex;



            switch (nGubun)
            {
                case 0: Display_Result_01(); break;   //위내시경 
                case 1: Display_Result_02(); break;   //복부초음파
                case 2: Display_Result_03(); break;   //대장내시경
                case 3: Display_Result_04(); break;   //유방촬영
                case 4: Display_Result_05(); break;   //유방초음파
                case 5: Display_Result_06(); break;   //하복부초음파
                case 6: Display_Result_07(); break;   //PapsMear
                case 7: Display_Result_08(); break;   //심장정밀검사
                case 8: Display_Result_09(); break;   //흉부CT
                case 9: Display_Result_10(); break;   //안저검사
                case 10: Display_Result_11(); break;   //갑상성초음파
                case 11: Display_Result_12(); break;   //전립선초음파
                case 12: Display_Result_13(); break;   //안압검사


            default: break;
            }


            //Display_Result_01();    //위내시경
            //Display_Result_02();    //복부초음파
            //Display_Result_03();    //대장내시경
            //Display_Result_04();    //유방촬영
            //Display_Result_05();    //유방초음파
            //Display_Result_06();    //하복부초음파
            //Display_Result_07();    //PapsMear
            //Display_Result_08();    //심장정밀검사
            //Display_Result_09();    //흉부CT
            //Display_Result_10();    //안저검사
            //Display_Result_11();    //갑상성초음파
            //Display_Result_12();    //전립선초음파
            //Display_Result_13();    //안압검사

        }


        private void Spread_Save()
        {
            long nGubun = 0;
            string strTitle = "";
            string strDate = "";
            string strFileName = "";


            nGubun = tabMain.SelectedTabIndex;

            switch (nGubun)
            {
                case 0: strTitle = "위내시경"; break;   
                case 1: strTitle = "복부초음파"; break; 
                case 2: strTitle = "대장내시경"; break; 
                case 3: strTitle = "유방촬영"; break;   
                case 4: strTitle = "유방초음파"; break;   
                case 5: strTitle = "하복부초음파"; break;   
                case 6: strTitle = "PapsMear"; break;   
                case 7: strTitle = "심장정밀검사"; break;   
                case 8: strTitle = "흉부CT"; break;   
                case 9: strTitle = "안저검사"; break;   
                case 10: strTitle = "갑상선초음파"; break;  
                case 11: strTitle = "전립선초음파"; break;  
                case 12: strTitle = "안압"; break;  
                default: break;
            }

            strDate = cHB.READ_Ltd_Name(fstrLtd) + "(" + fstrLtd + ")" + VB.Replace(DateTime.Now.ToShortDateString(), "-", "") + "_" + strTitle;

            DirectoryInfo Dir = new DirectoryInfo(@"c:\WORK");
            if (!Dir.Exists)
            {
                Dir.Create();
            }


            strFileName = @"c:\WORK\"+ strDate + ".xls";

            SS1.SaveExcel(strFileName, "");

            MessageBox.Show(@"엑셀파일 저장 완료", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }
        private void Display_Result_01()
        {
            //위내시경
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();

            strExcode.Clear();
            strExcode.Add("TX20");
            strExcode.Add("TX23");

            SS1.ActiveSheet.RowCount = 0;
            List <HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if(list.Count >0)
            {
                SS1.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";
                    
                    if(cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if(strOK =="OK" )
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS1.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS1.ActiveSheet.Cells[i, 1].Text = "";
                        SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if(clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS1.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS1.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS1.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS1.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS1.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS1.ActiveSheet.Cells[i, 8].Text = "";
                        SS1.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS1.ActiveSheet.Cells[i, 10].Text = "";
                        SS1.ActiveSheet.Cells[i, 11].Text =  list[i].SDATE.Trim();
                        SS1.ActiveSheet.Cells[i, 12].Text =  list[i].PANDATE.Trim();
                        SS1.ActiveSheet.Cells[i, 13].Text = "";
                        SS1.ActiveSheet.Cells[i, 14].Text =  list[i].RESULT.Trim();
                        SS1.ActiveSheet.Cells[i, 15].Text =  list[i].PANREMARK.Trim();
                        SS1.ActiveSheet.Cells[i, 16].Text = "";
                        SS1.ActiveSheet.Cells[i, 17].Text = "";
                        SS1.ActiveSheet.Cells[i, 18].Text = cHB.READ_ANATNO_STOMACH(strPtno, CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), -1), CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), 1));
                        SS1.ActiveSheet.Cells[i, 19].Text = "";
                        SS1.ActiveSheet.Cells[i, 20].Text = "";

                    }
                }
            }
        }

        private void Display_Result_02()
        {
            //복부초음파
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("TX27");

            SS2.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS2.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS2.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS2.ActiveSheet.Cells[i, 1].Text = "";
                        SS2.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS2.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS2.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS2.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS2.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS2.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS2.ActiveSheet.Cells[i, 8].Text = "";
                        SS2.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS2.ActiveSheet.Cells[i, 10].Text = "";
                        SS2.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS2.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS2.ActiveSheet.Cells[i, 13].Text = "";
                        SS2.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS2.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS2.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_03()
        {
            //대장내시경
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("TX32");
            strExcode.Add("TX64");

            SS3.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS3.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS3.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS3.ActiveSheet.Cells[i, 1].Text = "";
                        SS3.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS3.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS3.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS3.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS3.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS3.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS3.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS3.ActiveSheet.Cells[i, 8].Text = "";
                        SS3.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS3.ActiveSheet.Cells[i, 10].Text = "";
                        SS3.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS3.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS3.ActiveSheet.Cells[i, 13].Text = "";
                        SS3.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS3.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS3.ActiveSheet.Cells[i, 16].Text = "";
                        SS3.ActiveSheet.Cells[i, 17].Text = "";
                        SS3.ActiveSheet.Cells[i, 18].Text = cHB.READ_ANATNO_RECTUM(strPtno, CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), -1), CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), 1));
                        SS3.ActiveSheet.Cells[i, 19].Text = "";
                        SS3.ActiveSheet.Cells[i, 20].Text = "";
                    }
                }
            }
        }

        private void Display_Result_04()
        {
            //유방촬영
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("TX29");
            strExcode.Add("TX91");

            SS4.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    SS4.ActiveSheet.RowCount = list.Count;
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS4.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS4.ActiveSheet.Cells[i, 1].Text = "";
                        SS4.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS4.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS4.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS4.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS4.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS4.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS4.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS4.ActiveSheet.Cells[i, 8].Text = "";
                        SS4.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS4.ActiveSheet.Cells[i, 10].Text = "";
                        SS4.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS4.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS4.ActiveSheet.Cells[i, 13].Text = "";
                        SS4.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS4.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS4.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_05()
        {
            //유방초음파
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("TX83");

            SS5.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS5.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {
                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS5.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS5.ActiveSheet.Cells[i, 1].Text = "";
                        SS5.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS5.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS5.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS5.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS5.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS5.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS5.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS5.ActiveSheet.Cells[i, 8].Text = "";
                        SS5.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS5.ActiveSheet.Cells[i, 10].Text = "";
                        SS5.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS5.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS5.ActiveSheet.Cells[i, 13].Text = "";
                        SS5.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS5.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS5.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_06()
        {
            //하복부초음파
            string strPtno = "";
            string strSDate = "";
            string strResult = "";
            string strXrayCode = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("TX50");
            strExcode.Add("TX98");

            SS6.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS6.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";
                    strXrayCode = "";
                    strResult = "";
                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {
                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        strXrayCode = hicExcodeService.Read_XrayCode("TX98");
                        strResult = cHB.READ_XRAY_RESULT(strPtno, CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), -1), CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), 1), strXrayCode);

                        SS6.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS6.ActiveSheet.Cells[i, 1].Text = "";
                        SS6.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS6.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS6.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS6.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS6.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS6.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS6.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS6.ActiveSheet.Cells[i, 8].Text = "";
                        SS6.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS6.ActiveSheet.Cells[i, 10].Text = "";
                        SS6.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS6.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS6.ActiveSheet.Cells[i, 13].Text = list[i].RESULT.Trim();
                        SS6.ActiveSheet.Cells[i, 14].Text = strResult;
                        SS6.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS6.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_07()
        {
            //PapsMear
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("A161");

            SS7.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS7.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS7.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS7.ActiveSheet.Cells[i, 1].Text = "";
                        SS7.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS7.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS7.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS7.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS7.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS7.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS7.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS7.ActiveSheet.Cells[i, 8].Text = "";
                        SS7.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS7.ActiveSheet.Cells[i, 10].Text = "";
                        SS7.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS7.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS7.ActiveSheet.Cells[i, 13].Text = "";
                        SS7.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS7.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS7.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_08()
        {
            //심장초음파
            string strPtno = "";
            string strSDate = "";
            string strResult = "";
            string strXrayCode = "";
            string strGubun = "";

            strExcode.Clear();
            strExcode.Clear();
            strExcode.Add("A151");
            strExcode.Add("TX84");
            strExcode.Add("TX89");
            strExcode.Add("TX67");
            strExcode.Add("TX76");

            SS8.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS8.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS8.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS8.ActiveSheet.Cells[i, 1].Text = "";
                        SS8.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS8.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS8.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS8.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS8.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS8.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS8.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS8.ActiveSheet.Cells[i, 8].Text = "";
                        SS8.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS8.ActiveSheet.Cells[i, 10].Text = "";
                        SS8.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS8.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        List<HEA_JEPSU_RESULT_PATIENT> list2 = heaJepsuResultPatientService.GetResultBySdateWrtnoLtdcodeExcode(fstrFDate, fstrTDate, list[i].WRTNO, fstrLtd.To<long>(), strExcode);
                        if( list2.Count >0 )
                        {
                            
                            for (int j = 0; j < list2.Count; j++)
                            {
                                strResult = "";
                                strXrayCode = "";
                               
                                if (list2[j].EXCODE =="A151")
                                {
                                    strGubun = list2[j].EXCODE;
                                    strResult = cHB.READ_HEA_EKG_RESULT(list[i].WRTNO);
                                }
                                else if (list2[j].EXCODE == "TX84"|| list2[j].EXCODE == "TX89" || list2[j].EXCODE == "TX67"|| list2[j].EXCODE == "TX76")
                                {
                                    strGubun = list2[j].EXCODE;
                                    strXrayCode = hicExcodeService.Read_XrayCode(list2[j].EXCODE);
                                    strResult = cHB.READ_XRAY_RESULT(strPtno, CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), -1), CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(),1), strXrayCode);
                                }

                                if(strGubun == "A151")
                                {
                                    SS8.ActiveSheet.Cells[i, 16].Text = list2[j].RESULT.Trim();
                                    SS8.ActiveSheet.Cells[i, 17].Text = strResult;
                                }
                                else if (strGubun == "TX84")
                                {
                                    SS8.ActiveSheet.Cells[i, 18].Text = list2[j].RESULT.Trim();
                                    SS8.ActiveSheet.Cells[i, 19].Text = strResult;
                                }
                                else if (strGubun == "TX89")
                                {
                                    SS8.ActiveSheet.Cells[i, 14].Text = list2[j].RESULT.Trim();
                                    SS8.ActiveSheet.Cells[i, 15].Text = strResult;
                                }
                                else if (strGubun == "TX67" || strGubun == "TX76")
                                {
                                    SS8.ActiveSheet.Cells[i, 20].Text = list2[j].RESULT.Trim();
                                    SS8.ActiveSheet.Cells[i, 21].Text = strResult;
                                }
                            }
                        }
                        

                    }
                }
            }
        }

        private void Display_Result_09()
        {
            //흉부CT
            string strPtno = "";
            string strSDate = "";
            string strResult = "";
            string strXrayCode = "";

            strExcode.Clear();

            strExcode.Clear();
            strExcode.Add("TX88");

            SS9.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS9.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";
                    strXrayCode = "";
                    strResult = "";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        strXrayCode = hicExcodeService.Read_XrayCode(list[i].EXCODE);
                        strResult = cHB.READ_XRAY_RESULT(strPtno, CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), -1), CF.DATE_ADD(clsDB.DbCon, list[i].SDATE.Trim(), 1), strXrayCode);

                        SS9.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS9.ActiveSheet.Cells[i, 1].Text = "";
                        SS9.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS9.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS9.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS9.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS9.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS9.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS9.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS9.ActiveSheet.Cells[i, 8].Text = "";
                        SS9.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS9.ActiveSheet.Cells[i, 10].Text = "";
                        SS9.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS9.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS9.ActiveSheet.Cells[i, 13].Text = "";
                        SS9.ActiveSheet.Cells[i, 14].Text = strResult;
                        SS9.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS9.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_10()
        {
            //안저검사
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();

            strExcode.Clear();
            strExcode.Add("TE15");

            SS10.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS10.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS10.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS10.ActiveSheet.Cells[i, 1].Text = "";
                        SS10.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS10.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS10.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS10.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS10.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS10.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS10.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS10.ActiveSheet.Cells[i, 8].Text = "";
                        SS10.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS10.ActiveSheet.Cells[i, 10].Text = "";
                        SS10.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS10.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS10.ActiveSheet.Cells[i, 13].Text = "";
                        SS10.ActiveSheet.Cells[i, 14].Text = "";

                        //RESULT
                        List<XRAY_RESULTNEW> list2 = xrayResultnewService.GetItembyPaNoSeekDateXCode(list[i].PTNO.Trim(), list[i].SDATE.Trim(), "E6671");
                        if (list2.Count > 0)
                        {
                            for (int j = 0; j < list2.Count; j++)
                            {
                                SS10.ActiveSheet.Cells[i, 14].Text = list2[0].RESULT + list2[0].RESULT1;
                            }
                        }

                        SS10.ActiveSheet.Cells[i, 15].Text = "";
                        SS10.ActiveSheet.Cells[i, 16].Text = list[i].PANREMARK;
                    }
                }
            }

        }

        private void Display_Result_11()
        {
            //갑상선초음파
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();

            strExcode.Clear();
            strExcode.Add("TX99");

            SS11.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS11.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS11.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS11.ActiveSheet.Cells[i, 1].Text = "";
                        SS11.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS11.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS11.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS11.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS11.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS11.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS11.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS11.ActiveSheet.Cells[i, 8].Text = "";
                        SS11.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS11.ActiveSheet.Cells[i, 10].Text = "";
                        SS11.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS11.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS11.ActiveSheet.Cells[i, 13].Text = "";
                        SS11.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS11.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS11.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_12()
        {
            //전십선초음파
            string strPtno = "";
            string strSDate = "";

            strExcode.Clear();

            strExcode.Clear();
            strExcode.Add("ZE17");

            SS12.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS12.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS12.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS12.ActiveSheet.Cells[i, 1].Text = "";
                        SS12.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS12.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS12.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS12.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS12.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS12.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS12.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS12.ActiveSheet.Cells[i, 8].Text = "";
                        SS12.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS12.ActiveSheet.Cells[i, 10].Text = "";
                        SS12.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS12.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();
                        SS12.ActiveSheet.Cells[i, 13].Text = "";
                        SS12.ActiveSheet.Cells[i, 14].Text = list[i].RESULT.Trim();
                        SS12.ActiveSheet.Cells[i, 15].Text = list[i].PANREMARK.Trim();
                        SS12.ActiveSheet.Cells[i, 16].Text = "";
                    }
                }
            }
        }

        private void Display_Result_13()
        {
            //안압
            string strPtno = "";
            string strSDate = "";
            string strResult = "";

            strExcode.Clear();

            strExcode.Clear();
            strExcode.Add("TE43");

            SS13.ActiveSheet.RowCount = 0;
            List<HEA_JEPSU_RESULT_PATIENT> list = heaJepsuResultPatientService.GetListBySdateLtdCodeExcode(fstrFDate, fstrTDate, fstrLtd.To<long>(), strExcode);
            if (list.Count > 0)
            {
                SS13.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    if (cHB.READ_SUNAPDTL_FAMILY(list[i].WRTNO) == "OK") { strOK = ""; }

                    if (strOK == "OK")
                    {

                        strPtno = list[i].PTNO.Trim();
                        strSDate = list[i].SDATE.Trim();

                        SS13.ActiveSheet.Cells[i, 0].Text = "(재)포항성모병원";
                        SS13.ActiveSheet.Cells[i, 1].Text = "";
                        SS13.ActiveSheet.Cells[i, 2].Text = list[i].SNAME.Trim();
                        if (clsHcVariable.B01_VIEW_JUMIN == true)
                        {
                            SS13.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 7);
                        }
                        else
                        {
                            SS13.ActiveSheet.Cells[i, 3].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Mid(list[i].JUMIN, 7, 1) + "******";
                        }
                        SS13.ActiveSheet.Cells[i, 4].Text = list[i].PTNO.Trim();
                        SS13.ActiveSheet.Cells[i, 5].Text = list[i].AGE.Trim();
                        SS13.ActiveSheet.Cells[i, 6].Text = list[i].SEX.Trim();
                        SS13.ActiveSheet.Cells[i, 7].Text = "TO";
                        SS13.ActiveSheet.Cells[i, 8].Text = "";
                        SS13.ActiveSheet.Cells[i, 9].Text = list[i].DRNAME.Trim();
                        SS13.ActiveSheet.Cells[i, 10].Text = "";
                        SS13.ActiveSheet.Cells[i, 11].Text = list[i].SDATE.Trim();
                        SS13.ActiveSheet.Cells[i, 12].Text = list[i].PANDATE.Trim();

                        //RESULT
                        strResult = "";
                        List<HEA_RESULT> list2 = heaResultService.GetExCodeResultbyWrtNo(list[i].WRTNO);
                        if (list2.Count > 0)
                        {
                            for (int j = 0; j < list2.Count; j++)
                            {
                                if ( list2[j].EXCODE.Trim() == "TE43")
                                {
                                    strResult = "안압검사(좌) : " + list2[j].RESULT.Trim();
                                }
                                else if (list2[j].EXCODE.Trim() == "TE44")
                                {
                                    strResult = strResult + ComNum.VBLF + ComNum.VBLF + "안압검사(우) : " + list2[j].RESULT.Trim();
                                }

                            }
                        }
                        SS13.ActiveSheet.Cells[i, 13].Text = strResult;
                        SS13.ActiveSheet.Cells[i, 14].Text = "";
                    }
                }
            }
        }


    }
}
