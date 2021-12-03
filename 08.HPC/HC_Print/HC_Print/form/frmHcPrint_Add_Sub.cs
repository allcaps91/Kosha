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
    public partial class frmHcPrint_Add_Sub : Form
    {

        long fnDrno = 0;
        string fstrEtcChk = "2";

        HIC_JEPSU_RES_ETC_PATIENT nHJREP = null;
        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();
        clsHcFunc hf = new clsHcFunc();
        clsHcMain hm = new clsHcMain();
        clsHcPrint cHPrt = new clsHcPrint();

        HicJepsuResEtcPatientService hicJepsuResEtcPatientService = null;
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;
        HicResEtcService hicResEtcService = null;
        HicJepsuService hicJepsuService = null;

        public frmHcPrint_Add_Sub()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }
        public frmHcPrint_Add_Sub(HIC_JEPSU_RES_ETC_PATIENT argnHJREP)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            nHJREP = argnHJREP;

        }

        private void SetControl()
        {

            nHJREP = new HIC_JEPSU_RES_ETC_PATIENT();

            hicJepsuResEtcPatientService = new HicJepsuResEtcPatientService();
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();
            hicResEtcService = new HicResEtcService();
            hicJepsuService = new HicJepsuService();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Sub1();
            Result_Print_Sub2();

            Result_Print_Update();

            ComFunc.Delay(1500);
            this.Close();
        }
        private void Result_Print_Sub1()
        {
            long nDrNO = 0;
            string strUCodes = "";
            string strJumin = "";
            string strLtdCode = "";
            string strJepDate = "";
            string strData = "";
            string strTemp = "";
            string strTemp1 = "";
            string strTemp2 = "";
            string strList = "";
            string strSex = "";

            HIC_JEPSU_RES_ETC_PATIENT item = hicJepsuResEtcPatientService.GetItemByWrtnoGubun(nHJREP.WRTNO, "2");

            strJumin = item.JUMIN;
            strLtdCode = VB.Format(item.LTDCODE, "#");
            strJepDate = item.JEPDATE;
            nDrNO = item.PANJENGDRNO;


            SS_Print1.ActiveSheet.Cells[5, 2].Text = item.SNAME;
            SS_Print1.ActiveSheet.Cells[5, 3].Text = "주민등록번호 : " + VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
            strSex = item.SEX;
            if (strSex == "M")
            {
                SS_Print1.ActiveSheet.Cells[5, 6].Text = "남";
            }
            else
            {
                SS_Print1.ActiveSheet.Cells[5, 6].Text = "여";
            }

            SS_Print1.ActiveSheet.Cells[6, 2].Text = item.PTNO;
            SS_Print1.ActiveSheet.Cells[6, 3].Text = "사 업 장 : " + hb.READ_Ltd_Name(item.LTDCODE.ToString());

            SS_Print1.ActiveSheet.Cells[7, 2].Text = item.JUSO1 + " " + item.JUSO2;

            SS_Print1.ActiveSheet.Cells[8, 2].Text = item.JEPDATE;
            SS_Print1.ActiveSheet.Cells[8, 3].Text = "전 화 번 호 : " + item.TEL;

            SS_Print1.ActiveSheet.Cells[31, 3].Text = "     면 허 번 호 :  " + item.PANJENGDRNO;
            SS_Print1.ActiveSheet.Cells[31, 6].Text = hb.READ_License_DrName(item.PANJENGDRNO);
            fnDrno = item.PANJENGDRNO;


            hf.SignImage_Spread_Set(SS_Print1, 31, 7, fnDrno.ToString(), "C", "", "");

            //2페이지(SS_Print2)
            strTemp1 = "";
            if (strSex =="M")
            {
                strTemp1 = "  등록번호  :  " + item.PTNO + VB.Space(7) + "이름  :  " + item.SNAME + VB.Space(7) + "나이  :  " + item.AGE + "세" + VB.Space(7) + "성별  :  남" + VB.Space(7) + "건진일  :  " + strJepDate;
            }
            else
            {
                strTemp1 = "  등록번호  :  " + item.PTNO + VB.Space(7) + "이름  :  " + item.SNAME + VB.Space(7) + "나이  :  " + item.AGE + "세" + VB.Space(7) + "성별  :  여" + VB.Space(7) + "건진일  :  " + strJepDate;
            }

            SS_Print2.ActiveSheet.Cells[1, 1].Text = strTemp1;

            //종합판정소견
            strTemp1 = "";
            SS_Print1.ActiveSheet.Cells[12, 1].Text = item.SOGEN;

            //
            if (!item.PAN.IsNullOrEmpty())
            {
                switch (item.PAN)
                {
                    case "1": strTemp1 += "판정 : A(정상)" + ComNum.VBLF; break;
                    case "2": strTemp1 += "판정 : B(정상)" + ComNum.VBLF; break;
                    case "3": strTemp1 += "판정 : C1(직업병 요관찰자)" + ComNum.VBLF; break;
                    case "4": strTemp1 += "판정 : C2(일반질병 요관찰자)" + ComNum.VBLF; break;
                    case "5": strTemp1 += "판정 : D1(직업병 소견자)" + ComNum.VBLF; break;
                    case "6": strTemp1 += "판정 : D2(일반질병 소견자)" + ComNum.VBLF; break;
                    case "7": strTemp1 += "판정 : R(2차대상자)" + ComNum.VBLF; break;
                    case "8": strTemp1 += "판정 : U(미판정자)" + ComNum.VBLF; break;
                    case "9": strTemp1 += "판정 : CN(야간작업)" + ComNum.VBLF; break;
                    case "A": strTemp1 += "판정 : DN(야간작업)" + ComNum.VBLF; break;
                    default: break;
                }
            }

            if(!item.SOGENREMARK.IsNullOrEmpty())
            {
                strTemp1 += "소견내역 : " + item.SOGENREMARK.Trim() + ComNum.VBLF;
            }
            if (!item.WORKYN.IsNullOrEmpty())
            {
                strTemp1 += "업무적합성 : " + hb.READ_HIC_CODE("13",item.WORKYN) + ComNum.VBLF;
            }
            if (!item.SAHUCODE.IsNullOrEmpty())
            {
                strTemp1 += "사후관리 : " + hm.Sahu_Names_Display(item.SAHUCODE.Trim()) + ComNum.VBLF;
            }
            if (!item.JOCHI.IsNullOrEmpty())
            {
                strTemp1 += "조치내역 : " + item.JOCHI.Trim();
            }
            SS_Print1.ActiveSheet.Cells[12, 1].Text = strTemp1;


            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            cSpd.setSpdPrint(SS_Print1, PrePrint, setMargin, setOption, strHeader, strFooter);

            cHPrt.HIC_CERT_INSERT(SS_Print1, nHJREP.WRTNO, "51", fnDrno);

        }
        private void Result_Print_Sub2()
        {
            int nRow = 0;
            string strNomal = "";
            string strExCode = "";
            string strResCode = "";
            string strResult = "";
            string strGjYear = "";
            string strData = "";
            string strGbCodeUse = "";
            string strResultType = "";
            string strSex = "";
            string strTemp1 = "";
            string strList = "";


            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;




            List<HIC_RESULT_EXCODE_JEPSU> list = hicResultExcodeJepsuService.GetItemByWrtNo1(nHJREP.WRTNO);
            nRow = 4;
            SS_Print2.ActiveSheet.Cells[51, 1].Text = "-2-";

            if (!list.IsNullOrEmpty())
            {
                for (int i = 0; i <= list.Count - 1; i++)
                {

                    //3page
                    if( i == 41)
                    {
                        //출력

                        strTitle = "";
                        setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                        setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

                        cSpd.setSpdPrint(SS_Print2, PrePrint, setMargin, setOption, strHeader, strFooter);

                        cHPrt.HIC_CERT_INSERT(SS_Print2, nHJREP.WRTNO, "53", fnDrno);


                        nRow = 4;
                        for (int j = 5; j < 48; j++)
                        {
                            for (int k = 0; k < 6; k++)
                            {
                                SS_Print2.ActiveSheet.Cells[j, k].Text = "";
                            }
                        }
                    }

                    strSex = list[i].SEX.Trim();
                    strExCode = list[i].EXCODE;
                    strResult = list[i].RESULT;
                    strResCode = list[i].RESCODE;
                    strResultType = list[i].RESULTTYPE;
                    strGbCodeUse = list[i].GBCODEUSE;

                    nRow = nRow + 1;
                    if (nRow > SS_Print2.ActiveSheet.RowCount) { SS_Print2.ActiveSheet.RowCount = nRow; }

                    //간염항원
                    if (strExCode == "E504")
                    {
                        if (strResult == "01")
                        {
                            strResult = "음성(-)";
                        }
                        else if (strResult == "02")
                        {
                            strResult = "양성(+)";
                        }
                    }

                    SS_Print2.ActiveSheet.Cells[nRow, 1].Text = VB.Space(2) + list[i].HNAME.Trim();
                    SS_Print2.ActiveSheet.Cells[nRow, 3].Text = VB.Space(2) + strResult;

                    //비만도는 자동 계산함
                    if (strExCode == "A103")
                    {
                        strResCode = "061";
                    }
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            SS_Print2.ActiveSheet.Cells[nRow, 3].Text = VB.Space(2) + hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    //참고치를 Dispaly
                    if (strSex == "M")
                    {
                        if (!list[i].MIN_M.IsNullOrEmpty() && !list[i].MAX_M.IsNullOrEmpty())
                        {
                            strNomal = list[i].MIN_M.Trim() + "~" + list[i].MAX_M.Trim();
                        }
                        else if (!list[i].MIN_M.IsNullOrEmpty() && list[i].MAX_M.IsNullOrEmpty())
                        {
                            strNomal = list[i].MIN_M.Trim() + "~";
                        }
                        else if (list[i].MIN_M.IsNullOrEmpty() && !list[i].MAX_M.IsNullOrEmpty())
                        {
                            strNomal = "~" + list[i].MAX_M.Trim();
                        }

                    }
                    else
                    {
                        if (!list[i].MIN_F.IsNullOrEmpty() && !list[i].MAX_F.IsNullOrEmpty())
                        {
                            strNomal = list[i].MIN_F.Trim() + "~" + list[i].MAX_F.Trim();
                        }
                        else if (!list[i].MIN_F.IsNullOrEmpty() && list[i].MAX_F.IsNullOrEmpty())
                        {
                            strNomal = list[i].MIN_F.Trim() + "~";
                        }
                        else if (list[i].MIN_F.IsNullOrEmpty() && !list[i].MAX_F.IsNullOrEmpty())
                        {
                            strNomal = "~" + list[i].MAX_F.Trim();
                        }
                    }

                    if (strNomal == "~")
                    {
                        strNomal = "";
                    }

                    if (!list[i].UNIT.IsNullOrEmpty())
                    {
                        SS_Print2.ActiveSheet.Cells[nRow, 2].Text = VB.Space(2) + strNomal + " (" + list[i].UNIT + " )";
                    }
                    else
                    {
                        SS_Print2.ActiveSheet.Cells[nRow, 2].Text = VB.Space(2) + strNomal;
                    }

                    strTemp1 = ""; strList = "";
                    strTemp1 = SS_Print2.ActiveSheet.Cells[nRow, 3].Text;

                    if (!strTemp1.IsNullOrEmpty())
                    {
                        strList = cf.TextBox_2_MultiLine(strTemp1, 34);
                        for (int j = 1; j <= VB.L(strList, "{{@}}"); j++)
                        {
                            if (j > 1)
                            {
                                nRow = nRow + 1;
                            }
                            if (j < 5)
                            {
                                SS_Print2.ActiveSheet.Cells[nRow, 3].Text = VB.Space(2) + VB.Pstr(strList, "{{@}}", j);
                            }
                        }
                        strTemp1 = "";
                    }
                }

                //출력

                strTitle = "";
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);

                cSpd.setSpdPrint(SS_Print2, PrePrint, setMargin, setOption, strHeader, strFooter);

                cHPrt.HIC_CERT_INSERT(SS_Print2, nHJREP.WRTNO, "53", fnDrno);

            }
        }
        private void Result_Print_Update()
        {
            string strUpdateOK = "";

            HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(nHJREP.WRTNO, fstrEtcChk);
            if (item.IsNullOrEmpty())
            {
                strUpdateOK = "OK";
            }
            else
            {
               if(  item.GBPRINT.Trim() != "Y") { strUpdateOK = "OK"; }
            }

            if (strUpdateOK == "OK")
            {
                hicResEtcService.UpdateByWrtnoGubun(nHJREP.WRTNO, clsType.User.IdNumber.To<long>(), clsPublic.GstrSysDate, fstrEtcChk);
            }

            //접수테이블에 통보일자 세팅최초값-갱신안됨
            HIC_JEPSU item1 = hicJepsuService.GetItemByWrtnoGjjong(nHJREP.WRTNO, "69");
            if (!item1.IsNullOrEmpty())
            {
                if(item1.TONGBODATE.IsNullOrEmpty())
                {
                    hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(nHJREP.WRTNO, clsType.User.IdNumber.To<long>());
                }
            }
        }
    }
}
