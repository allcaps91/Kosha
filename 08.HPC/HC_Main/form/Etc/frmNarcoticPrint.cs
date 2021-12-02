using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmNarcoticPrint.cs
/// Description     : 향정마약처방전인쇄
/// Author          : 이상훈
/// Create Date     : 2019-09-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm향정마약처방전인쇄.frm(Frm향정마약처방전인쇄)" />

namespace HC_Main
{
    public partial class frmNarcoticPrint : Form
    {
        BasSunService basSunService = null;
        HicDoctorService hicDoctorService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        ComFunc cf = new ComFunc();

        string FstrPrint = "";

        public frmNarcoticPrint(string sPrint)
        {
            InitializeComponent();
            FstrPrint = sPrint;
            SetEvent();
        }

        void SetEvent()
        {
            basSunService = new BasSunService();
            hicDoctorService = new HicDoctorService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nCNT = 0;
            int nRow = 0;
            string strPRT = "";
            string strYakList = "";
            string strSuCode = "";
            string strDRSABUN = "";
            string strFile = "";
            string strUnit1 = "";
            string strUnit2 = "";
            string strUnit3 = "";
            string strUnit4 = "";
            string strUnit = "";
            string strQty = "";

            strPRT = "";
            strDRSABUN = VB.Pstr(strPRT, "{$}", 15);
            strQty = VB.Pstr(strPRT, "{$}", 16);

            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1.ActiveSheet.Cells[0, 2].Text = VB.Space(10) + VB.Pstr(strPRT, "{$}", 1);
            SS1.ActiveSheet.Cells[1, 1].Text = VB.Pstr(strPRT, "{$}", 2); //병동/병실
            SS1.ActiveSheet.Cells[1, 4].Text = VB.Pstr(strPRT, "{$}", 3); //성명
            SS1.ActiveSheet.Cells[1, 8].Text = VB.Pstr(strPRT, "{$}", 4); //등록번호
            SS1.ActiveSheet.Cells[1, 10].Text = VB.Pstr(strPRT, "{$}", 5); //진료과
            SS1.ActiveSheet.Cells[2, 1].Text = VB.Pstr(strPRT, "{$}", 6); //오더번호
            SS1.ActiveSheet.Cells[2, 4].Text = VB.Pstr(strPRT, "{$}", 7); //성별/나이
            SS1.ActiveSheet.Cells[2, 8].Text = VB.Pstr(strPRT, "{$}", 8); //주민번호
            SS1.ActiveSheet.Cells[3, 1].Text = VB.Pstr(strPRT, "{$}", 9); //주소
            SS1.ActiveSheet.Cells[4, 1].Text = VB.Pstr(strPRT, "{$}", 10); //상병명
            SS1.ActiveSheet.Cells[5, 1].Text = VB.Pstr(strPRT, "{$}", 11); //주요증상
            SS1.ActiveSheet.Cells[6, 1].Text = VB.Pstr(strPRT, "{$}", 12); //처방일
            SS1.ActiveSheet.Cells[6, 8].Text = VB.Pstr(strPRT, "{$}", 13); //검사일

            //처방내역
            strYakList = VB.Pstr(strPRT, "{$}", 14);
            strSuCode = VB.Pstr(strYakList, ",", 1);

            if (strSuCode != "")
            {
                SS1.ActiveSheet.Cells[8, 0].Text = strSuCode;   //약품코드
                SS1.ActiveSheet.Cells[8, 1].Text = fn_Read_Drug_Jep_Name(strSuCode);
                SS1.ActiveSheet.Cells[8, 5].Text = VB.Pstr(strQty, ",", 2);
                SS1.ActiveSheet.Cells[8, 6].Text = "1";
                SS1.ActiveSheet.Cells[8, 7].Text = "1";
                SS1.ActiveSheet.Cells[8, 8].Text = " IV";

                //용량
                BAS_SUN list = basSunService.GetItembySuCode(strSuCode);

                SS1.ActiveSheet.Cells[8, 3].Text = "";
                if (list != null)
                {
                    strUnit1 = list.UNITNEW1.ToString();
                    strUnit2 = list.UNITNEW2.ToString();
                    strUnit3 = list.UNITNEW3.ToString();
                    strUnit4 = list.UNITNEW4.ToString();
                    strUnit = double.Parse(strUnit1) + "/" + double.Parse(strUnit2) + (double.Parse(strUnit4) > 0 ? strUnit4 + "㎖/" : "") + double.Parse(strUnit3);
                    SS1.ActiveSheet.Cells[8, 3].Text = strUnit;
                }

                SS1.ActiveSheet.Cells[9, 5].Text = VB.Pstr(strQty, ",", 1);
            }

            SS1.ActiveSheet.Cells[12, 1].Text = "";
            SS1.ActiveSheet.Cells[12, 2].Text = "";
            SS1.ActiveSheet.Cells[13, 1].Text = "";

            //의사의 성명,면허번호를 읽음
            if (strDRSABUN != "")
            {
                HIC_DOCTOR drList = hicDoctorService.GetIDrNameLicencebyDrSabun(strDRSABUN);

                if (drList != null)
                {
                    SS1.ActiveSheet.Cells[12, 1].Text = drList.DRNAME.Replace(" ", "");
                    SS1.ActiveSheet.Cells[13, 1].Text = drList.LICENCE;
                }

                //의사 사인 이미지 로드
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                SS1_Sheet1.Cells[12, 2].CellType = imgCellType;
                SS1_Sheet1.Cells[12, 2].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                SS1_Sheet1.Cells[12, 2].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strDRSABUN, 5, "0") + ".jpg";

                Image image = hb.SIGNATUREFILE_DBToFile(strDRSABUN);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    SS1_Sheet1.Cells[12, 2].Value = image;
                }
            }
            timer1.Enabled = true;
        }

        string fn_Read_Drug_Jep_Name(string argSuCode)
        {
            string rtnVal = "";
            string strTemp = "";

            strTemp = comHpcLibBService.GetJepNamebySuCode(argSuCode);

            //한글만 표현 특수문자및 영어는 삭제처리
            for (int i = 0; i < strTemp.Length; i++)
            {
                if (string.Compare(VB.Mid(strTemp, 1, i), "33") >= 0 && string.Compare(VB.Mid(strTemp, 1, i), "126") <= 0)
                {
                }
                else
                {
                    rtnVal += VB.Mid(strTemp, i, 1);
                }
            }

            return rtnVal;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnPrint)
            {
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, false, setMargin, setOption, "", "");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            eBtnClick(btnPrint, new EventArgs());

            this.Close();
        }
    }
}
