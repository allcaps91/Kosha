using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAmResvSet_New.cs
/// Description     : 일자별 암검진 예약인원 설정
/// Author          : 이상훈
/// Create Date     : 2020-01-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmResvSet_new.frm(HcAm06)" />

namespace ComHpcLibB
{
    public partial class frmHcAmResvSet_New : Form
    {
        HicCancerResv1Service hicCancerResv1Service = null;
        ComHpcLibBService comHpcLibBService = null;

        string[] FstrHolyDay = new string[31];
        string[] FstrYoil = new string[31];

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        public frmHcAmResvSet_New()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCancerResv1Service = new HicCancerResv1Service();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSetting.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            fn_ComMon_Set(cboYYMM, 14);
            fn_Screen_Set();
            eBtnClick(btnSearch, new EventArgs());

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strJDate = "";
                string strWeek = "";
                string strRemark = "";
                string strRemark1 = "";
                string strChange = "";
                string strROWID = "";
                string strFlag = "";
                int nDay = 0;
                int nREAD = 0;
                int nUgi = 0;
                int nGfs = 0;
                int nGfsH = 0;
                int nMammo = 0;
                int nRectum = 0;
                int nSono = 0;
                int nWomb = 0;
                int nUgi1 = 0;
                int nGfs1 = 0;
                int nGfsH1 = 0;
                int nMammo1 = 0;
                int nRectum1 = 0;
                int nSono1 = 0;
                int nWomb1 = 0;
                int nBohum = 0;
                int nBohum1 = 0;
                int nCT = 0;
                int nCT1 = 0;
                int nLungSangdam = 0;
                int nLungSangdam1 = 0;
                int nRow = 0;

                int result = 0;

                nREAD = SS1.ActiveSheet.RowCount;
                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < nREAD; i++)
                {
                    nRow = i;
                    if ((i + 1) % 2 == 1)
                    {
                        nDay += 1;
                    }
                    else
                    {
                        nRow = i - 1;
                    }

                    strJDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-" + string.Format("{0:00}", nDay);

                    switch (SS1.ActiveSheet.Cells[nRow, 1].Text)
                    {
                        case "일":
                            strWeek = "0";
                            break;
                        case "월":
                            strWeek = "1";
                            break;
                        case "화":
                            strWeek = "2";
                            break;
                        case "수":
                            strWeek = "3";
                            break;
                        case "목":
                            strWeek = "4";
                            break;
                        case "금":
                            strWeek = "5";
                            break;
                        case "토":
                            strWeek = "6";
                            break;
                        default:
                            break;
                    }

                    if ((i + 1) % 2 == 1)
                    {
                        strFlag = "AM";
                        if (SS1.ActiveSheet.Cells[nRow, 2].Text.IsNullOrEmpty())
                        {
                            nUgi = 0;
                        }
                        else
                        {
                            nUgi = SS1.ActiveSheet.Cells[nRow, 2].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 3].Text.IsNullOrEmpty())
                        {
                            nGfs = 0;
                        }
                        else
                        {
                            nGfs = SS1.ActiveSheet.Cells[nRow, 3].Text.To<int>(); 
                        }

                        if (SS1.ActiveSheet.Cells[nRow, 4].Text.IsNullOrEmpty())
                        {
                            nGfsH = 0;
                        }
                        else
                        {
                            nGfsH = SS1.ActiveSheet.Cells[nRow, 4].Text.To<int>(); 
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 5].Text.IsNullOrEmpty())
                        {
                            nMammo = 0;
                        }
                        else
                        {
                            nMammo = SS1.ActiveSheet.Cells[nRow, 5].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 6].Text.IsNullOrEmpty())
                        {
                            nRectum = 0;
                        }
                        else
                        {
                            nRectum = SS1.ActiveSheet.Cells[nRow, 6].Text.To<int>(); 
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 7].Text.IsNullOrEmpty())
                        {
                            nSono = 0;
                        }
                        else
                        {
                            nSono = SS1.ActiveSheet.Cells[nRow, 7].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 8].Text.IsNullOrEmpty())
                        {
                            nWomb = 0;
                        }
                        else
                        {
                            nWomb = SS1.ActiveSheet.Cells[nRow, 8].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 9].Text.IsNullOrEmpty())
                        {
                            nBohum = 0;
                        }
                        else
                        {
                            nBohum = SS1.ActiveSheet.Cells[nRow, 9].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 10].Text.IsNullOrEmpty())
                        {
                            nCT = 0;
                        }
                        else
                        {
                            nCT = SS1.ActiveSheet.Cells[nRow, 10].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 11].Text.IsNullOrEmpty())
                        {
                            nLungSangdam = 0;
                        }
                        else
                        {
                            nLungSangdam = SS1.ActiveSheet.Cells[nRow, 11].Text.To<int>();
                        }
                        strRemark = SS1.ActiveSheet.Cells[nRow, 12].Text.Trim();
                    }
                    else
                    {
                        strFlag = "PM";
                        nRow += 1;
                        if (SS1.ActiveSheet.Cells[nRow, 2].Text.IsNullOrEmpty())
                        {
                            nUgi1 = 0;
                        }
                        else
                        {
                            nUgi1 = SS1.ActiveSheet.Cells[nRow, 2].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 3].Text.IsNullOrEmpty())
                        {
                            nGfs1 = 0;
                        }
                        else
                        {
                            nGfs1 = SS1.ActiveSheet.Cells[nRow, 3].Text.To<int>();
                        }

                        if (SS1.ActiveSheet.Cells[nRow, 4].Text.IsNullOrEmpty())
                        {
                            nGfsH1 = 0;
                        }
                        else
                        {
                            nGfsH1 = SS1.ActiveSheet.Cells[nRow, 4].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 5].Text.IsNullOrEmpty())
                        {
                            nMammo1 = 0;
                        }
                        else
                        {
                            nMammo1 = SS1.ActiveSheet.Cells[nRow, 5].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 6].Text.IsNullOrEmpty())
                        {
                            nRectum1 = 0;
                        }
                        else
                        {
                            nRectum1 = SS1.ActiveSheet.Cells[nRow, 6].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 7].Text.IsNullOrEmpty())
                        {
                            nSono1 = 0;
                        }
                        else
                        {
                            nSono1 = SS1.ActiveSheet.Cells[nRow, 7].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 8].Text.IsNullOrEmpty())
                        {
                            nWomb1 = 0;
                        }
                        else
                        {
                            nWomb1 = SS1.ActiveSheet.Cells[nRow, 8].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 9].Text.IsNullOrEmpty())
                        {
                            nBohum1 = 0;
                        }
                        else
                        {
                            nBohum1 = SS1.ActiveSheet.Cells[nRow, 9].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 10].Text.IsNullOrEmpty())
                        {
                            nCT1 = 0;
                        }
                        else
                        {
                            nCT1 = SS1.ActiveSheet.Cells[nRow, 10].Text.To<int>();
                        }
                        if (SS1.ActiveSheet.Cells[nRow, 11].Text.IsNullOrEmpty())
                        {
                            nLungSangdam1 = 0;
                        }
                        else
                        {
                            nLungSangdam1 = SS1.ActiveSheet.Cells[nRow, 11].Text.To<int>();
                        }
                        strRemark1 = SS1.ActiveSheet.Cells[nRow, 12].Text.Trim();
                    }

                    strROWID = SS1.ActiveSheet.Cells[nRow, 13].Text.Trim();
                    strChange = SS1.ActiveSheet.Cells[nRow, 14].Text.Trim();

                    HIC_CANCER_RESV1 item = new HIC_CANCER_RESV1();

                    item.JOBDATE = strJDate;
                    item.WEEK = strWeek;
                    item.UGI = nUgi;
                    item.GFS = nGfs;
                    item.GFSH = nGfsH;
                    item.MAMMO = nMammo;
                    item.RECTUM = nRectum;
                    item.SONO = nSono;
                    item.WOMB = nWomb;
                    item.WOMB1 = nWomb1;
                    item.UGI1 = nUgi1;
                    item.GFS1 = nGfs1;
                    item.GFSH1 = nGfsH1;
                    item.MAMMO1 = nMammo;
                    item.RECTUM1 = nRectum1;
                    item.SONO1 = nSono1;
                    item.BOHUM = nBohum;
                    item.BOHUM1 = nBohum1;
                    item.CT = nCT;
                    item.CT1 = nCT1;
                    item.REMARK = strRemark;
                    item.ENTSABUN = clsType.User.IdNumber.To<long>();
                    item.LUNG_SANGDAM = nLungSangdam;
                    item.LUNG_SANGDAM1 = nLungSangdam1;
                    item.ROWID = strROWID;

                    if (strROWID.IsNullOrEmpty())
                    {
                        result = hicCancerResv1Service.Insert(item);
                    }
                    else
                    {
                        if (strChange == "Y")
                        {
                            if (strFlag == "AM")
                            {
                                result = hicCancerResv1Service.Update1(item);
                            }
                            else if (strFlag == "PM")
                            {
                                result = hicCancerResv1Service.Update2(item);
                            }
                        }
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("등록 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSetting)
            {
                string strDate = "";
                string strJDate = "";
                string strWeek = "";
                string strHoly = "";
                string strRemark = "";
                string strFDate = "";
                string strTDate = "";
                int nLastDay = 0;
                int nREAD = 0;
                int nUgi = 0;
                int nGfs = 0;
                int nGfsH = 0;
                int nMammo = 0;
                int nRectum = 0;
                int nSono = 0;
                int nWomb = 0;
                int nUgi1 = 0;
                int nGfs1 = 0;
                int nGfsH1 = 0;
                int nMammo1 = 0;
                int nRectum1 = 0;
                int nSono1 = 0;
                int nWomb1 = 0;
                int nBohum = 0;
                int nBohum1 = 0;
                int nCT = 0;
                int nCT1 = 0;
                int nLungSangdam = 0;
                int nLungSangdam1 = 0;
                string strHolyDay = "";
                int result = 0;

                strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
                strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
                nLastDay = int.Parse(VB.Right(strTDate, 2));

                List<HIC_CANCER_RESV1> list = hicCancerResv1Service.GetItembyJobDate(strFDate, strTDate);

                if (list.Count > 0)
                {
                    MessageBox.Show("이미 초기세팅했습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                nREAD = SS1.ActiveSheet.RowCount + 1;
                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < nLastDay; i++)
                {
                    strDate = VB.Left(strFDate, 7) + "-" + string.Format("{0:00}", i + 1);

                    strHolyDay = comHpcLibBService.GetHolyDay(strDate);
                    if (strHolyDay == "*")
                    {
                        strHoly = "휴";
                    }
                    else
                    {
                        strHoly = "";
                    }
                    strJDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-" + string.Format("{0:00}", i + 1);
                    switch (SS1.ActiveSheet.Cells[i, 1].Text.Trim())
                    {
                        case "일":
                            strWeek = "0";
                            break;
                        case "월":
                            strWeek = "1";
                            break;
                        case "화":
                            strWeek = "2";
                            break;
                        case "수":
                            strWeek = "3";
                            break;
                        case "목":
                            strWeek = "4";
                            break;
                        case "금":
                            strWeek = "5";
                            break;
                        case "토":
                            strWeek = "6";
                            break;
                        default:
                            break;
                    }

                    if (strWeek == "0") //일
                    {
                        nUgi = 0;
                        nGfs = 0;   nGfsH = 0; nMammo = 0; nRectum = 0; nSono = 0;    nWomb = 0;  nBohum = 0; nCT = 0;
                        nUgi1 = 0;  nGfs1 = 0; nGfsH1 = 0; nMammo1 = 0; nRectum1 = 0; nSono1 = 0; nWomb1 = 0; nBohum1 = 0; nCT1 = 0;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else if (strWeek == "1") //월
                    {
                        nUgi = 10;   nGfs = 31; nGfsH = 0;   nMammo = 999;  nRectum = 999;  nSono = 999;  nWomb = 999;  nBohum = 999; nCT = 10;
                        nUgi1 = 10; nGfs1 = 10; nGfsH1 = 10; nMammo1 = 999; nRectum1 = 999; nSono1 = 999; nWomb1 = 999; nBohum1 = 999; nCT1 = 10;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else if (strWeek == "2") //화
                    {
                        nUgi = 10;   nGfs = 31; nGfsH = 0;   nMammo = 999;  nRectum = 999;  nSono = 999;  nWomb = 999;  nBohum = 999; nCT = 10;
                        nUgi1 = 10; nGfs1 = 10; nGfsH1 = 10; nMammo1 = 999; nRectum1 = 999; nSono1 = 999; nWomb1 = 999; nBohum1 = 999; nCT1 = 10;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else if (strWeek == "3") //수
                    {
                        nUgi = 10;   nGfs = 31; nGfsH = 0;   nMammo = 999;  nRectum = 999;  nSono = 999;  nWomb = 999;  nBohum = 999; nCT = 10;
                        nUgi1 = 10; nGfs1 = 10; nGfsH1 = 10; nMammo1 = 999; nRectum1 = 999; nSono1 = 999; nWomb1 = 999; nBohum1 = 999; nCT1 = 10;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else if (strWeek == "4") //목
                    {
                        nUgi = 10;   nGfs = 31; nGfsH = 0;   nMammo = 999;  nRectum = 999;  nSono = 999;  nWomb = 999;  nBohum = 999; nCT = 10;
                        nUgi1 = 10; nGfs1 = 10; nGfsH1 = 10; nMammo1 = 999; nRectum1 = 999; nSono1 = 999; nWomb1 = 999; nBohum1 = 999; nCT1 = 10;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else if (strWeek == "5") //금
                    {
                        nUgi = 10;   nGfs = 31; nGfsH = 0;   nMammo = 999;  nRectum = 999;  nSono = 999;  nWomb = 999;  nBohum = 999; nCT = 10;
                        nUgi1 = 10; nGfs1 = 10; nGfsH1 = 10; nMammo1 = 999; nRectum1 = 999; nSono1 = 999; nWomb1 = 999; nBohum1 = 999; nCT1 = 10;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else if (strWeek == "6") //토
                    {
                        nUgi = 10;   nGfs = 31; nGfsH = 10;  nMammo = 999;  nRectum = 999;  nSono = 999;  nWomb = 999;  nBohum = 999; nCT = 10;
                        nUgi1 = 0;  nGfs1 = 0; nGfsH1 = 0;   nMammo1 = 0;   nRectum1 = 0;   nSono1 = 0;   nWomb1 = 0;   nBohum1 = 0; nCT1 = 0;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    else
                    {
                        nUgi = 0;  nGfs = 0;  nGfsH = 0;  nMammo = 0;  nRectum = 0;  nSono = 0;  nWomb = 0;  nBohum = 0;
                        nUgi1 = 0; nGfs1 = 0; nGfsH1 = 0; nMammo1 = 0; nRectum1 = 0; nSono1 = 0; nWomb1 = 0; nBohum1 = 0;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }

                    if (strHoly == "휴")
                    {
                        nUgi = 0;  nGfs = 0;  nGfsH = 0;  nMammo = 0;  nRectum = 0;  nSono = 0;  nWomb = 0;  nBohum = 0;    nCT = 0;
                        nUgi1 = 0; nGfs1 = 0; nGfsH1 = 0; nMammo1 = 0; nRectum1 = 0; nSono1 = 0; nWomb1 = 0; nBohum1 = 0; nCT1 = 0;
                        nLungSangdam = 0; nLungSangdam1 = 0;
                    }
                    strRemark = SS1.ActiveSheet.Cells[i, 12].Text.Trim();

                    HIC_CANCER_RESV1 item = new HIC_CANCER_RESV1();

                    item.JOBDATE = strJDate;
                    item.WEEK = strWeek;
                    item.UGI = nUgi;
                    item.GFS = nGfs;
                    item.GFSH = nGfsH;
                    item.MAMMO = nMammo;
                    item.RECTUM = nRectum;
                    item.SONO = nSono;
                    item.WOMB = nWomb;
                    item.WOMB1 = nWomb1;
                    item.UGI1 = nUgi1;
                    item.GFS1 = nGfs1;
                    item.GFSH1 = nGfsH1;
                    item.MAMMO1 = nMammo;
                    item.RECTUM1 = nRectum1;
                    item.SONO1 = nSono1;
                    item.BOHUM = nBohum;
                    item.BOHUM1 = nBohum1;
                    item.CT = nCT;
                    item.CT1 = nCT1;
                    item.REMARK = strRemark;
                    item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                    item.LUNG_SANGDAM = nLungSangdam;
                    item.LUNG_SANGDAM1 = nLungSangdam1;

                    result = hicCancerResv1Service.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("등록 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                string strFDate = "";
                string strTDate = "";

                strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2) + "-01";
                strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
                fn_Screen_Set();

                List<HIC_CANCER_RESV1> list = hicCancerResv1Service.GetItembyJobDate(strFDate, strTDate);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    //오전세팅
                    SS1.ActiveSheet.Cells[(i * 2), 2].Text = list[i].UGI.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 3].Text = list[i].GFS.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 4].Text = list[i].GFSH.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 5].Text = list[i].MAMMO.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 6].Text = list[i].RECTUM.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 7].Text = list[i].SONO.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 8].Text = list[i].WOMB.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 9].Text = list[i].BOHUM.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 10].Text = list[i].CT.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 11].Text = list[i].LUNG_SANGDAM.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2), 12].Text = list[i].REMARK;
                    SS1.ActiveSheet.Cells[(i * 2), 13].Text = list[i].ROWID;

                    //오후세팅
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 2].Text = list[i].UGI1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 3].Text = list[i].GFS1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 4].Text = list[i].GFSH1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 5].Text = list[i].MAMMO1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 6].Text = list[i].RECTUM1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 7].Text = list[i].SONO1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 8].Text = list[i].WOMB1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 9].Text = list[i].BOHUM1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 10].Text = list[i].CT1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 11].Text = list[i].LUNG_SANGDAM1.To<string>();
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 12].Text = list[i].REMARK1;
                    SS1.ActiveSheet.Cells[(i * 2) + 1, 13].Text = list[i].ROWID;
                }

                btnSetting.Enabled = false;
                if (btnSave.Enabled == true)
                {
                    btnSetting.Enabled = true;
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            SS1.ActiveSheet.Cells[e.Row, 14].Text = "Y";
        }

        void fn_Screen_Set()
        {
            int nREAD = 0;
            int nDay = 0;
            int nLastDay = 0;
            string strGbn = "";
            string strGbName = "";
            string strFDate = "";
            string strTDate = "";
            string strYoil = "";
            string strDate = "";

            if (cboYYMM.Text == "*")
            {
                return;
            }

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 150;
            strFDate = VB.Left(cboYYMM.Text, 4) + "-" + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate += "-01";
            strTDate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);
            nLastDay = int.Parse(VB.Right(strTDate, 2));

            for (int i = 0; i < nLastDay; i++)
            {
                FstrHolyDay[i] = "";
                FstrYoil[i] = " ";
            }

            //일자별 휴일을 SET
            List<COMHPC> list = comHpcLibBService.GetJobDatebyBasJob(strFDate, strTDate);

            for (int i = 0; i < list.Count; i++)
            {
                nDay = list[i].ILJA.To<int>();
                FstrHolyDay[nDay - 1] = "*";
            }

            SS1.ActiveSheet.RowCount = nLastDay * 2;
            for (int i = 0; i < nLastDay; i++)
            {
                SS1.ActiveSheet.Cells[i * 2, 0].Text = VB.Mid(strFDate, 6, 2) + "/" + string.Format("{0:00}", i + 1);
            }

            //일자별 요일을 SET
            for (int i = 0; i < nLastDay; i++)
            {
                strDate = VB.Left(strFDate, 7) + "-" + string.Format("{0:00}", i + 1);
                strYoil = cf.READ_YOIL(clsDB.DbCon, strDate);
                switch (strYoil)
                {
                    case "월요일":
                        FstrYoil[i] = "1";
                        break;
                    case "화요일":
                        FstrYoil[i] = "2";
                        break;
                    case "수요일":
                        FstrYoil[i] = "3";
                        break;
                    case "목요일":
                        FstrYoil[i] = "4";
                        break;
                    case "금요일":
                        FstrYoil[i] = "5";
                        break;
                    case "토요일":
                        FstrYoil[i] = "6";
                        break;
                    case "일요일":
                        FstrYoil[i] = "7";
                        break;
                    default:
                        break;
                }
                if (FstrHolyDay[i] != "*" && FstrYoil[i] == "6")
                {
                    FstrHolyDay[i] = "#";
                }

                SS1.ActiveSheet.Cells[i * 2, 1].Text = VB.Left(strYoil, 1);
                if (strYoil == "토요일")
                {
                    //SS1.ActiveSheet.Cells[i * 2, 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFF0000"));
                    SS1.ActiveSheet.Cells[i * 2, 1].ForeColor = Color.FromArgb(0, 0, 255);
                }
                else if (strYoil == "일요일")
                {
                    //SS1.ActiveSheet.Cells[i * 2, 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFF&"));
                    SS1.ActiveSheet.Cells[i * 2, 1].ForeColor = Color.FromArgb(255, 0, 0);
                }

                //if (FstrHolyDay[i] == "*")
                //{

                //}
                //else if (FstrHolyDay[i] == "#")
                //{

                //}
            }
        }

        void fn_ComMon_Set(ComboBox ArgCombo, int ArgMonthCnt)
        {
            string strYYMM = "";
            int ArgYY = 0;
            int ArgMM = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strYYMM = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2);
            strYYMM = hf.DATE_YYMM_ADD(strYYMM, -6);
            ArgCombo.Items.Clear();
            for (int i = 0; i < ArgMonthCnt; i++)
            {
                ArgCombo.Items.Add(VB.Left(strYYMM, 4) + "년 " + VB.Right(strYYMM, 2) + "월분");
                strYYMM = hf.DATE_YYMM_ADD(strYYMM, 1);
            }

            ArgCombo.SelectedIndex = 0;
        }
    }
}
