using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanXrayPracticianView.cs
/// Description     : 방사선 종사자 판정 조회화면
/// Author          : 이상훈
/// Create Date     : 2019-11-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm방사선조회.frm(HcPan106)" />

namespace HC_Pan
{
    public partial class frmHcPanXrayPracticianView : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcCombo Combo = new clsHcCombo();

        long FnWrtNo;
        long FnWrtno1;
        long FnWrtno2;
        string FstrSex;
        long FnPano;
        string FstrGjJong;

        int nOldCNT = 0;
        //string strExamCode = "";
        List<string> strExamCode = new List<string>();
        string strAllWRTNO = "";
        string strJepDate = "";
        string strExPan = "";

        int nREAD;

        int nHyelH = 0;
        int nHyelL = 0;
        int nHeight = 0;
        int nWeight = 0;
        int nResult = 0;

        string strExCode = "";
        string strResult = "";
        string strResCode = "";
        string strResultType = "";
        string strGbCodeUse = "";
        string strResName = "";
        string strRemark = "";
        string strOldJepsuDate = "";
        string strOldJepDate = "";

        public frmHcPanXrayPracticianView(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_Screen_Clear();
            Combo.ComboPanjeng2_SET(cboPanjeng);  //특수종합판정

            if (!FnWrtNo.IsNullOrEmpty())
            {
                txtWrtNo.Text = FnWrtNo.To<string>();
                fn_Screen_Display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void fn_Screen_Clear()
        {
            txtPlace.Text = "";
            txtRemark.Text = "";
            txtMuch.Text = "";
            txtJung.Text = "";
            txtEye.Text = "";
            txtSkin.Text = "";
            txtEtc.Text = "";
            txtTerm.Text = "";
            rdo1.Checked = false;
            rdo2.Checked = false;
            txtXJong.Text = "";
            txtPanDrNo1.Text = "";
            lblDrName1.Text = "";
            rdoGubun1.Checked = false;
            rdoGubun2.Checked = false;
            txtSogenRemark.Text = "";
            txtJochiRemark.Text = "";
            txtPanDrNo.Text = "";
            txtPanDrNo.Text = "";
            txtPanjeng.Text = "";
            cboPanjeng.SelectedIndex = -1;
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            string strGjYear = "";
            string strGjChasu = "";
            string strGjBangi = "";
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strResName = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strIpsadate = "";
            string strJepDate = "";
            string strGjJong = "";
            long nLicense = 0;
            string strDrname = "";
            string strExPan = "";

            FnWrtno1 = long.Parse(txtWrtNo.Text);

            //인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWrtno1);

            if (list == null)
            {
                MessageBox.Show("접수번호 : " + FnWrtno1 + " 번 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrSex = list.SEX;
            FnPano = list.PANO;
            strGjYear = list.GJYEAR;
            strGjChasu = list.GJCHASU;
            strGjBangi = list.GJBANGI;
            strJepDate = list.JEPDATE;
            FstrGjJong = list.GJJONG;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

            //검사결과 1,2차를 모두 표시하기
            List<HIC_JEPSU> list2 = hicJepsuService.GetItembyPaNoGjYearGjBangiJepDate(FnPano, strGjYear, strGjBangi, strJepDate);

            FnWrtno1 = 0;
            FnWrtno2 = 0;

            if (list2.Count >= 1)
            {
                FnWrtno1 = list2[0].WRTNO;  //1차
            }

            if (list2.Count >= 2)
            {
                FnWrtno2 = list2[1].WRTNO;  //2차
            }

            fn_Screen_Munjin1_Display();
            hm.ExamResult_RePanjeng(FnWrtno1, FstrSex, strJepDate, ""); //검사결과를 재판정
            //Screen_Exam_Items_display //검사항목을 Display
            List<HIC_RESULT_EXCODE> list3 = hicResultExCodeService.GetItembyWrtNo1WrtNo2PaNo(FnWrtno1, FnWrtno2, FnPano);

            nREAD = list3.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list3[i].EXCODE;                 //검사코드
                strResult = list3[i].RESULT;                 //검사실 결과값
                strResCode = list3[i].RESCODE;               //결과값 코드
                strResultType = list3[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list3[i].GBCODEUSE;           //결과값코드 사용여부

                //SS2에 검사실 결과값을 DISPLAY    
                nRow += 1;
                if (nRow > SS2.ActiveSheet.RowCount)
                {
                    SS2.ActiveSheet.RowCount = nRow;
                }
                
                SS2.ActiveSheet.Cells[nRow - 1, 0].Text = " " + list3[i].HNAME;
                SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strResult;

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        SS2.ActiveSheet.Cells[nRow - 1, 1].Text = strResName;
                    }
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, strJepDate, FstrSex, list3[i].MIN_M, list3[i].MAX_M, list3[i].MIN_F, list3[i].MAX_F);
                if (strNomal == "~")
                {
                    strNomal = "";
                }
                else if (strNomal != "" && VB.Right(strNomal, 1) == "~")
                {
                    strNomal = VB.Left(strNomal, strNomal.Length - 1);
                }

                SS2.ActiveSheet.Cells[i, 4].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strExCode;
                SS2.ActiveSheet.Cells[i, 8].Text = strResult;   //정상값 점검용
                strExPan = list3[i].PANJENG;
                SS2.ActiveSheet.Cells[i, 2].Text = strExPan;
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222);  //정상
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                        break;
                }
            }

            SS2.ActiveSheet.RowCount = nRow;

            fn_OLD_Result_Display(FnPano, strJepDate, FstrSex); //종전결과 2개를 Display
        }

        void fn_Screen_Munjin1_Display()
        {
            long nLicense = 0;

            //건강검진 문진표 및 결과를  READ

            COMHPC list = comHpcLibBService.GetItemHicXMunjinbyWrtNo(FnWrtno1);

            if (list == null)
            {
                MessageBox.Show("접수번호 " + FnWrtno1 + " 는 결과 및 판정이 등록 안됨!", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //문진
            if (list.XP1 == "Y")
            {
                rdo1.Checked = true;
            }
            else if (list.XP1 == "N")
            {
                rdo2.Checked = true;
            }

            if (list.JINGBN == "Y")
            {
                rdoGubun1.Checked = true;
            }
            else if (list.JINGBN == "N")
            {
                rdoGubun2.Checked = true;
            }

            if (!list.PAN.IsNullOrEmpty())
            {
                cboPanjeng.SelectedIndex = int.Parse(VB.Left(list.PAN, 1));
            }
            else
            {
                cboPanjeng.SelectedIndex = 0;
            }

            txtSogenRemark.Text = list.SOGEN;
            txtJochiRemark.Text = list.JOCHI;

            txtXJong.Text = list.XPJONG;
            txtRemark.Text = list.XPLACE;
            txtPlace.Text = list.XREMARK;
            txtTerm.Text = list.XTERM;
            txtXTerm.Text = list.XTERM1;
            txtMuch.Text = list.XMUCH;
            txtJung.Text = list.XJUNGSAN;
            txtMun1.Text = list.MUN1;
            txtEye.Text = list.JUNGSAN1;
            txtSkin.Text = list.JUNGSAN2;
            txtEtc.Text = list.JUNGSAN3;
            txtPanDrNo1.Text = list.MUNDRNO;

            //판정
            txtPanjeng.Text = list.PANJENG;
            if (!list.PANJENGDATE.ToString().IsNullOrEmpty())
            {
                txtPanDate.Text = list.PANJENGDATE.ToString();
            }
            else
            {
                txtPanDate.Text = clsPublic.GstrSysDate;
            }
            nLicense = list.PANJENGDRNO;               //의사면허번호
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            if (nLicense > 0)
            {
                txtPanDrNo.Text = nLicense.ToString();
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
            }
            else
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
                lblDrName.Text = hb.READ_License_DrName(clsHcVariable.GnHicLicense);
            }
        }

        void fn_OLD_Result_Display(long argPano, string argJepDate, string argSex)
        {
            //1차검사 종전 접수번호를 읽음
            List<HIC_JEPSU> list = hicJepsuService.GetItembyPaNoJepDateWrtNo(argPano, argJepDate, FnWrtno1, FnWrtno2);

            nOldCNT = list.Count;
            strAllWRTNO = "";
            if (nOldCNT > 2) nOldCNT = 2;
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.To<string>() + ",";
                strJepDate = list[i].JEPDATE;
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 5).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                fn_OLD_Result_Display_SUB(list[i].WRTNO, strExamCode, argSex, i);
                if (i >= 1) break;
            }
        }

        /// <summary>
        /// 종전 1회 검사의 결과를 Display
        /// </summary>
        void fn_OLD_Result_Display_SUB(long nWrtNo, List<string> argExamCode, string argSex, int index)
        {
            int nRow = 0;

            ///TODO : 이상훈(2019.10.31) clsHcMain.cs GET_HIC_JepsuDate Method 확인 필요
            //판정결과를 strRemark에 보관
            strOldJepsuDate = hm.GET_HIC_JepsuDate(nWrtNo);

            //검사항목 및 결과를 READ
            List<HIC_RESULT_EXCODE> list = hicResultExCodeService.GetItembyOnlyWrtNo(nWrtNo);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE;                 //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부

                //해당검사코드가 있는 Row를 찾음
                nRow = 0;
                for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                {
                    if (SS2.ActiveSheet.Cells[j, 7].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                if (nRow > 0)
                {
                    SS2.ActiveSheet.Cells[nRow, index + 5].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS2.ActiveSheet.Cells[nRow, index + 5].Text = strResName;
                            if (strResName.Length > 7)
                            {
                                strRemark += "▷" + list[i].HNAME + ": ";
                                strRemark += strResName + "\r\n";
                            }
                        }
                        else if (strResult.Length > 7)
                        {
                            strResult += "▷" + list[i].HNAME + ": ";
                            strRemark += strResult + "\r\n";
                        }
                        SS2.ActiveSheet.Cells[nRow, index + 9].Text = strResult;   //정상값 점검용
                        strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, strOldJepDate, "");
                        //판정결과별 바탕색상을 다르게 표시함
                        switch (strExPan)
                        {
                            case "B":
                                SS2.ActiveSheet.Cells[nRow, index + 5].BackColor = Color.FromArgb(250, 210, 222);   //정상B
                                break;
                            case "R":
                                SS2.ActiveSheet.Cells[nRow, index + 5].BackColor = Color.FromArgb(250, 170, 170);   //질환의심(R)
                                break;
                            default:
                                SS2.ActiveSheet.Cells[nRow, index + 5].BackColor = Color.FromArgb(190, 250, 220);   //정상A 또는 기타
                                break;
                        }
                    }
                }
            }
        }
    }
}
