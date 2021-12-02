using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcBloodExam_Sub : Form
    {
        long fnPano = 0;
        long FnDrno = 0;
        long fnWRTNO_OLD = 0;

        string fstrRePrt = "";
        string fstrJepdate = "";

        HIC_JEPSU_RES_ETC_PATIENT nHJREP = null;

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();

        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;
        HicJepsuService hicJepsuService = null;
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;
        HicResEtcService hicResEtcService = null;



        public frmHcBloodExam_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcBloodExam_Sub(HIC_JEPSU_RES_ETC_PATIENT argnHJREP, string argRePrt)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJREP = argnHJREP;
            fstrRePrt = argRePrt;
        }
        private void SetControl()
        {

            nHJREP = new HIC_JEPSU_RES_ETC_PATIENT();

            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
            hicJepsuService = new HicJepsuService();
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();
            hicResEtcService = new HicResEtcService();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Clear();       //Sheet Clear
            Result_Print_Sub1();

            Result_Print_Update();

            //HIC_RES_SPECIAL 인쇄일자, 사번을 업데이트
            //hicResSpecialService.UpdatePRINTbyWrtNo(nHJREP.WRTNO, clsType.User.IdNumber.To<long>());

            //HIC_JEPSU 인쇄일자, 사번을 업데이트
            //hicJepsuService.UpdatePRINTbyWrtNo(nHJREP.WRTNO, clsType.User.IdNumber.To<long>());

            ComFunc.Delay(1500);
            this.Close();
        }
        private void Result_Print_Clear()
        {

            SS1.ActiveSheet.Cells[3, 4].Text = "";
            SS1.ActiveSheet.Cells[5, 1].Text = "";
            SS1.ActiveSheet.Cells[5, 2].Text = "";
            SS1.ActiveSheet.Cells[5, 5].Text = "";

            SS1.ActiveSheet.Cells[6, 1].Text = "";
            SS1.ActiveSheet.Cells[6, 5].Text = "";

            SS1.ActiveSheet.Cells[7, 1].Text = "";
            SS1.ActiveSheet.Cells[7, 5].Text = "";

            SS1.ActiveSheet.Cells[8, 1].Text = "";
            SS1.ActiveSheet.Cells[8, 2].Text = "";

            for (int i = 2; i < 30; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = "";
            }

            SS1.ActiveSheet.Cells[31, 2].Text = "";
            SS1.ActiveSheet.Cells[31, 5].Text = "";
        }

        private void Result_Print_Sub1()
        {

            string strUcodes = "";
            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strData = "";
            string strTemp = "";
            string strTemp1 = "";
            string strTemp2 = "";
            string strList = "";
            string strSex = "";
            string strInfo = "";

            HIC_JEPSU_RES_ETC_PATIENT item = hicJepsuResEtcPatientService.GetItemByWrtnoGubun(nHJREP.WRTNO, "1");
            if(!item.IsNullOrEmpty())
            {
                strSex = item.SEX.Trim();
                strJumin = item.JUMIN;
                strLtdCode = item.LTDCODE.ToString();
                strJepDate = item.JEPDATE;
                fstrJepdate = item.JEPDATE;
                fnPano = item.PANO;

                SS1.ActiveSheet.Cells[3, 4].Text = "성명: " + item.SNAME.Trim();
                if(!item.SABUN.IsNullOrEmpty())
                {
                    if (item.SABUN != "0")
                    {
                        SS1.ActiveSheet.Cells[3, 4].Text = "사번: " + item.SABUN.Trim();
                    }
                }

                //1페이지
                SS1.ActiveSheet.Cells[5, 1].Text = item.SNAME.Trim();
                SS1.ActiveSheet.Cells[5, 2].Text = " 주민등록번호 : " + VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                SS1.ActiveSheet.Cells[5, 5].Text = VB.IIf(strSex == "M", "남", "여").ToString();

                SS1.ActiveSheet.Cells[6, 1].Text = item.PTNO;
                SS1.ActiveSheet.Cells[6, 2].Text = " 사 업 장 : " + hb.READ_Ltd_Name(strLtdCode);

                SS1.ActiveSheet.Cells[7, 1].Text = item.JUSO1 + " " + item.JUSO2;
                SS1.ActiveSheet.Cells[7, 5].Text = item.BUSENAME;

                SS1.ActiveSheet.Cells[8, 1].Text = item.JEPDATE;
                SS1.ActiveSheet.Cells[8, 2].Text = " 전 화 번 호 : " + item.TEL;

                SS1.ActiveSheet.Cells[31, 2].Text = " 면 허 번 호 : " + item.PANJENGDRNO; FnDrno = item.PANJENGDRNO;
                SS1.ActiveSheet.Cells[31, 5].Text = hb.READ_License_DrName(item.PANJENGDRNO) + " (인)";

                SS1.ActiveSheet.Cells[12, 0].Text = item.SOGEN;

                //2페이지에 사용
                strInfo = VB.Space(2) + "등록번호: " + item.PTNO + VB.Space(5) + "이름: " + item.SNAME + VB.Space(5) + "나이: " + item.AGE + "세" + VB.Space(5);
                strInfo += VB.IIf(strSex == "M", "성별 : 남", "성별 : 여").ToString() + VB.Space(5) + "건진일: " + item.JEPDATE;


                //2페이지
                for (int i = 1; i <= 6; i++)
                {
                   
                    FpSpread SS2 = (Controls.Find("SS2" + i.ToString(), true)[0] as FpSpread);
                    SS2.ActiveSheet.Cells[1, 0].Text = strInfo;

                    SS_Draw_Line_All(SS2, 1);
                }
            }
           
        }
        private void Result_Print_Sub2()
        {
            long nSS = 0;
            long nRow = 0;
            long nRead = 0;

            string strYear = "";
            string strYear_Old = "";

            string strExamBun_New = "";
            string strExamBun_Old = "";

            string strExCode = "";
            string strResCode = "";
            string strResult = "";
            string strGjYear = "";
            string strData = "";



            List<HIC_JEPSU> list = hicJepsuService.GetWrtnoYearByPanoJepdateJong(fnPano, fstrJepdate, "62");
            if(!list.IsNullOrEmpty())
            {
                fnWRTNO_OLD = list[0].WRTNO;
                strYear_Old = list[0].GJYEAR;
            }


            //금회의 검사결과를 READ
            List<HIC_RESULT_EXCODE_JEPSU> list1 = hicResultExcodeJepsuService.GetItemByWrtNo2(nHJREP.WRTNO);
            if (list1.IsNullOrEmpty())
            {
                nRead = list1.Count;
                nRow = 2;

                for (int i = 0; i < list1.Count; i++)
                {

                    if (nRow >= 49)
                    {
                        nSS += 1;
                        nRow = 2;

                    }

                    strYear = list1[i].GJYEAR;
                    if (list1[i].EXAMBUN.IsNullOrEmpty())
                    {
                        strExamBun_New = "zz";
                    }
                    else
                    {
                        strExamBun_New = list1[i].EXAMBUN;
                    }

                    if((strExamBun_New != strExamBun_Old) || (strExamBun_New.IsNullOrEmpty() && strExamBun_Old.IsNullOrEmpty()))
                    {
                        if (!strExamBun_Old.IsNullOrEmpty())
                        {
                            //Call SS_Draw_Line_Top_Bottom(SS3(nSS), -1, nRow)
                        }

                        strExamBun_Old = strExamBun_New;
                        nRow += 2;
                        
                        










                    }

                    strExCode = list1[i].EXCODE;
                    strResCode = list1[i].RESCODE;
                    strResult = list1[i].RESULT;

                    //A,C형간염항체(Anti HCV IgG)
                    if (strExCode =="E508" || strExCode == "E511")
                    {
                        if(strResult == "01")
                        {
                            strResult = "음성";
                        }
                        else if (strResult == "02")
                        {
                            strResult = "양성";
                        }
                        else if (VB.UCase(VB.Left(strResult,3)) == "NON")
                        {
                            strResult = "음성";
                        }
                        else if (VB.UCase(VB.Left(strResult, 3)) == "REA")
                        {
                            strResult = "양성";
                        }
                        else if (VB.UCase(VB.Left(strResult, 3)) == "BOR")
                        {
                            strResult = "경계치";
                        }
                        else
                        {
                            strResult = strResult;
                        }

                    }

                    if (!strResCode.IsNullOrEmpty()) { strResult = hb.READ_ResultName(strResCode, strResult).Trim(); }
                    if(strExCode =="A258" || strExCode == "E921")
                    {
                        strResult = hb.READ_ResultName("004", strResult).Trim();
                    }

                    nRow = nRow + 1;














                }


            }
        }


        private void Result_Print_Update()
        {

            string strUpdateOK = "";

            HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(nHJREP.WRTNO, "1");
            if(item.IsNullOrEmpty())
            {
                strUpdateOK = "OK";
            }
            else
            {
                if(item.GBPRINT != "Y") { strUpdateOK = "OK"; }
            }

            if(strUpdateOK=="OK")
            {
                hicResEtcService.UpdateByWrtnoGubun(nHJREP.WRTNO, clsType.User.IdNumber.To<long>(), clsPublic.GstrSysDate, "1");

                hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(nHJREP.WRTNO, clsType.User.IdNumber.To<long>());

            }
        }


            private void SS_Draw_Line_All(Control argSS, int argRow)
        {
            //argSS.

            //argSS.SetCellBorder ArgCol, argRow, ArgCol, argRow, SS_BORDER_TYPE_LEFT, &HFFFFFFFF, SS_BORDER_STYLE_SOLID
            //argSS.SetCellBorder ArgCol, argRow, ArgCol, argRow, SS_BORDER_TYPE_TOP, &HFFFFFFFF, SS_BORDER_STYLE_SOLID
            //argSS.SetCellBorder ArgCol, argRow, ArgCol, argRow, SS_BORDER_TYPE_RIGHT, &HFFFFFFFF, SS_BORDER_STYLE_SOLID
            //argSS.SetCellBorder ArgCol, argRow, ArgCol, argRow, SS_BORDER_TYPE_BOTTOM, &HFFFFFFFF, SS_BORDER_STYLE_SOLID

            //FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), 
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), 
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), 
            //    new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);




        }
        private void SS_Draw_Line_Left_Right(Control argSS, int argRow)
        {
        }

        private void SS_Draw_Line_Top_Bottom(Control argSS, int argRow)
        {
        }
    }
}
