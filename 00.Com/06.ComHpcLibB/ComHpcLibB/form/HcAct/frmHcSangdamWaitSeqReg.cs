using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSangdamWaitSeqReg.cs
/// Description     : 상담대기순번 등록
/// Author          : 이상훈
/// Create Date     : 2019-09-04
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcAct04_New.frm(Frm상담대기순번등록_New)" />

namespace ComHpcLibB
{
    public partial class frmHcSangdamWaitSeqReg : Form
    {
        HicSangdamNewService hicSangdamNewService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicJepsuService hicJepsuService = null;
        HicDoctorService hicDoctorService = null;
        BasScheduleService basScheduleService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnWrtNo = 0;
        long FnPaNo = 0;
        string FstrROWID = "";
        int FnIndex = 0;

        public frmHcSangdamWaitSeqReg(long nPaNo, long nWrtNo)
        {
            InitializeComponent();

            FnPaNo = nPaNo;
            FnWrtNo = nWrtNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicSangdamNewService = new HicSangdamNewService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicJepsuService = new HicJepsuService();
            hicDoctorService = new HicDoctorService();
            basScheduleService = new BasScheduleService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eActivated);
            this.btnInsert1.Click += new EventHandler(eBtnClick);
            this.btnInsert2.Click += new EventHandler(eBtnClick);
            this.btnInsert3.Click += new EventHandler(eBtnClick);
            this.btnInsert4.Click += new EventHandler(eBtnClick);
            this.btnInsert5.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnChange.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS3.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS4.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.SS5.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
        }

        void eActivated(object sender, EventArgs e)
        {
            long nRoom = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            btnInsert1.Enabled = false;
            btnInsert2.Enabled = false;
            btnInsert3.Enabled = false;
            btnInsert4.Enabled = false;
            btnInsert5.Enabled = false;

            //건진의사 스케쥴 불러오기
            List<HIC_DOCTOR> list = hicDoctorService.GetIDrCode();

            for (int i = 0; i < list.Count; i++)
            {
                BAS_SCHEDULE list2 = basScheduleService.GetGbJinbyDrCode(list[i].DRCODE);

                //오후일과인지 체크
                if (string.Compare(clsPublic.GstrSysTime, "13:00") > 0)
                {
                    if (list2.GBJIN2.Trim() == "1")
                    {
                        switch (list[i].ROOM.Trim())
                        {
                            case "15":
                                nRoom = 0;
                                break;
                            case "16":
                                nRoom = 1;
                                break;
                            case "17":
                                nRoom = 2;
                                break;
                            case "18":
                                nRoom = 3;
                                break;
                            case "19":
                                nRoom = 4;
                                break;
                            default:
                                break;
                        }

                        switch (FnIndex)
                        {
                            case 1:
                                btnInsert1.Enabled = true;
                                break;
                            case 2:
                                btnInsert2.Enabled = true;
                                break;
                            case 3:
                                btnInsert3.Enabled = true;
                                break;
                            case 4:
                                btnInsert4.Enabled = true;
                                break;
                            case 5:
                                btnInsert5.Enabled = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (list2.GBJIN.Trim() == "1")
                    {
                        switch (list[i].ROOM.Trim())
                        {
                            case "15":
                                nRoom = 0;
                                break;
                            case "16":
                                nRoom = 1;
                                break;
                            case "17":
                                nRoom = 2;
                                break;
                            case "18":
                                nRoom = 3;
                                break;
                            case "19":
                                nRoom = 4;
                                break;
                            default:
                                break;
                        }

                        switch (FnIndex)
                        {
                            case 1:
                                btnInsert1.Enabled = true;
                                break;
                            case 2:
                                btnInsert2.Enabled = true;
                                break;
                            case 3:
                                btnInsert3.Enabled = true;
                                break;
                            case 4:
                                btnInsert4.Enabled = true;
                                break;
                            case 5:
                                btnInsert5.Enabled = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            fn_Screen_Display();

            txtWrtNo.Text = "";
            txtSName.Text = "";

            cboGubun.Items.Clear();
            cboGubun.Items.Add("15번방");
            cboGubun.Items.Add("16번방");
            cboGubun.Items.Add("17번방");
            cboGubun.Items.Add("18번방");
            cboGubun.Items.Add("19번방");
            cboGubun.SelectedIndex = -1;
        }

        void fn_Screen_Display()
        {
            int ii = 0;
            int jj = 0;
            int[] nRow = new int[5];
            int[] nJin = new int[5];

            for (int i = 0; i <= 4; i++)
            {
                nRow[i] = 0;
                nJin[i] = 0;
            }

            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);
            sp.Spread_All_Clear(SS4);
            sp.Spread_All_Clear(SS5);

            //접수취소된 대기자는 삭제함
            int result = hicSangdamWaitService.Delete_JepsuCancel();

            pnlInfo.Visible = false;

            //현재까지 상담한 인원수를 구함
            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetItem();

            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].GUBUN)
                {
                    case "15":
                        ii = 0;
                        break;
                    case "16":
                        ii = 1;
                        break;
                    case "17":
                        ii = 2;
                        break;
                    case "18":
                        ii = 3;
                        break;
                    case "19":
                        ii = 4;
                        break;
                    default:
                        break;
                }
                nJin[ii] = nJin[ii] + 1;
            }

            List<HIC_SANGDAM_WAIT> list1 = hicSangdamWaitService.GetItem_GbCallNull();

            for (int i = 0; i < list1.Count; i++)
            {
                switch (list1[i].GUBUN.Trim())
                {
                    case "15":
                        ii = 0;
                        break;
                    case "16":
                        ii = 1;
                        break;
                    case "17":
                        ii = 2;
                        break;
                    case "18":
                        ii = 3;
                        break;
                    case "19":
                        ii = 4;
                        break;
                    default:
                        break;
                }

                nRow[ii] += 1;
                if (ii == 0)
                {
                    if (SS1.ActiveSheet.RowCount < nRow[ii])
                    {
                        SS1.ActiveSheet.RowCount = nRow[ii];
                    }
                    SS1.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].PANO.To<string>();
                    SS1.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].SNAME.Trim();
                    SS1.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].AGE.To<string>() + "/" + list1[i].SEX.To<string>();
                    SS1.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = fn_Read_GjJong_List(list1[i].PANO).To<string>() ;
                }
                else if (ii == 1)
                {
                    if (SS2.ActiveSheet.RowCount < nRow[ii])
                    {
                        SS2.ActiveSheet.RowCount = nRow[ii];
                    }
                    SS2.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].PANO.To<string>();
                    SS2.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].SNAME.Trim();
                    SS2.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].AGE.To<string>() + "/" + list1[i].SEX.To<string>();
                    SS2.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = fn_Read_GjJong_List(list1[i].PANO).To<string>();
                }
                else if (ii == 2)
                {
                    if (SS3.ActiveSheet.RowCount < nRow[ii])
                    {
                        SS3.ActiveSheet.RowCount = nRow[ii];
                    }
                    SS3.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].PANO.To<string>();
                    SS3.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].SNAME.Trim();
                    SS3.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].AGE.To<string>() + "/" + list1[i].SEX.To<string>();
                    SS3.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = fn_Read_GjJong_List(list1[i].PANO).To<string>();
                }
                else if (ii == 3)
                {
                    if (SS4.ActiveSheet.RowCount < nRow[ii])
                    {
                        SS4.ActiveSheet.RowCount = nRow[ii];
                    }
                    SS4.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].PANO.To<string>();
                    SS4.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].SNAME.Trim();
                    SS4.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].AGE.To<string>() + "/" + list1[i].SEX.To<string>();
                    SS4.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = fn_Read_GjJong_List(list1[i].PANO).To<string>();
                }
                else if (ii == 4)
                {
                    if (SS5.ActiveSheet.RowCount < nRow[ii])
                    {
                        SS5.ActiveSheet.RowCount = nRow[ii];
                    }
                    SS5.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].PANO.To<string>();
                    SS5.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].SNAME.Trim();
                    SS5.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = list1[i].AGE.To<string>() + "/" + list1[i].SEX.To<string>();
                    SS5.ActiveSheet.Cells[nRow[ii] - 1, 0].Text = fn_Read_GjJong_List(list1[i].PANO).To<string>();
                }
            }

            btnInsert1.Text += " [" + nRow[0] + "명/" + nJin[0] + "명]";
            SS1.ActiveSheet.RowCount = nRow[0];

            btnInsert2.Text += " [" + nRow[1] + "명/" + nJin[1] + "명]";
            SS2.ActiveSheet.RowCount = nRow[1];

            btnInsert3.Text += " [" + nRow[2] + "명/" + nJin[2] + "명]";
            SS3.ActiveSheet.RowCount = nRow[2];

            btnInsert4.Text += " [" + nRow[3] + "명/" + nJin[3] + "명]";
            SS4.ActiveSheet.RowCount = nRow[3];

            btnInsert5.Text += " [" + nRow[4] + "명/" + nJin[4] + "명]";
            SS5.ActiveSheet.RowCount = nRow[4];
        }

        string fn_Read_GjJong_List(long argPaNo)
        {
            string rtnVal = "";

            List<HIC_JEPSU> lst = hicJepsuService.GetJepsuInfobyPano(argPaNo);

            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    rtnVal += lst[i].GJJONG.Trim() + ",";
                }
            }

            return rtnVal;
        }

        void fn_Insert(int Index)
        {
            string strSName = "";
            string strSex = "";
            long nAge = 0;
            string strGubun = "";
            string strMsg = "";
            string strGjJong = "";
            string strFlag = "";
            long nWait = 0;
            long nWrtNo = 0;

            if (FnPaNo == 0 || FnWrtNo == 0)
            {
                return;
            }

            strGubun = hicSangdamWaitService.GetGubunbyPaNoWrtNo(FnPaNo, FnWrtNo);

            if (strGubun != "")
            {
                if (string.Compare(strGubun, "15") >= 0 && string.Compare(strGubun, "19") <= 0)
                {
                    MessageBox.Show(strGubun + "번방에 이미 등록되어 있습니다!", "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            strFlag = "";
            List<HIC_JEPSU> list = hicJepsuService.GetItembyPaNo(FnPaNo);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strGjJong = list[i].GJJONG;
                    strSName = list[i].GJJONG;
                    strSex = list[i].GJJONG;
                    nAge = list[i].AGE;
                    strGubun = (Index + 15).To<string>();
                    strFlag = hb.READ_SangDam_Gubun(strGjJong);
                    if (strFlag == "Y")
                    {
                        break;
                    }
                }
            }

            if (strFlag == "")
            {
                strMsg = "수검자명: " + strSName;
                strMsg += "검진번호 : " + FnPaNo;
                strMsg += "상담항목이 없습니다!";
                MessageBox.Show(strMsg, "등록불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            nWait = hicSangdamWaitService.GetMaxWaitNobyGubun(strGubun, "N");

            List<HIC_JEPSU> list2 = hicJepsuService.GetJepsuInfobyPano(FnPaNo);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    nWrtNo = list2[i].WRTNO;
                    strGjJong = list2[i].GJJONG.Trim();

                    //상담대상 검진만 등록함
                    if (hb.READ_SangDam_Gubun(strGjJong) == "Y")
                    {
                        int result = hicSangdamWaitService.Insert_Hic_Sangdam_Wait(nWrtNo, strSName, strSex, nAge, strGjJong, strGubun, nWait, FnPaNo, "");

                        if (result < 0)
                        {
                            MessageBox.Show("상담대기 순번등록 중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                }
            }

            fn_Screen_Display();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }            
            else if (sender == btnInsert1)
            {
                FnIndex = 1;
                fn_Insert(1);
            }
            else if (sender == btnInsert2)
            {
                FnIndex = 2;
                fn_Insert(2);
            }
            else if (sender == btnInsert3)
            {
                FnIndex = 3;
                fn_Insert(3);
            }
            else if (sender == btnInsert4)
            {
                FnIndex = 4;
                fn_Insert(4);
            }
            else if (sender == btnInsert5)
            {
                FnIndex = 5;
                fn_Insert(5);
            }
            else if (sender == btnChange)
            {
                string strGubun = "";
                string strTemp = "";
                long nWaitNo = 0;

                switch (FnIndex)
                {
                    case 0:
                        strTemp = "15";
                        break;
                    case 1:
                        strTemp = "16";
                        break;
                    case 2:
                        strTemp = "17";
                        break;
                    case 3:
                        strTemp = "18";
                        break;
                    case 4:
                        strTemp = "19";
                        break;
                    default:
                        break;
                }

                strGubun = VB.Left(cboGubun.Text, 2);

                if (strTemp == strGubun)
                {
                    MessageBox.Show("다른 상담실을 선택하여 주십시오.", "동일상담실", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                nWaitNo = hicSangdamWaitService.GetMaxWaitNobyGubun(strGubun);

                HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                item.GUBUN = strGubun;
                item.WAITNO = nWaitNo;
                item.PANO = FnPaNo;

                int result = hicSangdamWaitService.UpdateWaitNobyPaNo(item);

                if (result < 0)
                {
                    MessageBox.Show("변경시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_Screen_Display();
            }

            else if (sender == btnDelete)
            {
                int result = hicSangdamWaitService.Delete_Hic_Sangdam_Wait(FnPaNo, "Y");

                if (result < 0)
                {
                    MessageBox.Show("삭제시 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fn_Screen_Display();
            }
            else if (sender == btnClose)
            {
                pnlInfo.Visible = false;
                fn_Screen_Display();
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                pnlInfo.Visible = true;

                txtWrtNo.Text = SS1.ActiveSheet.Cells[e.Row, 0].Text;
                txtSName.Text = SS1.ActiveSheet.Cells[e.Row, 1].Text;
                FnPaNo = SS1.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                cboGubun.SelectedIndex = 0;
            }
            else if (sender == SS2)
            {
                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                pnlInfo.Visible = true;

                txtWrtNo.Text = SS2.ActiveSheet.Cells[e.Row, 0].Text;
                txtSName.Text = SS2.ActiveSheet.Cells[e.Row, 1].Text;
                FnPaNo = SS2.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                cboGubun.SelectedIndex = 1;
            }
            else if (sender == SS3)
            {
                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                pnlInfo.Visible = true;

                txtWrtNo.Text = SS3.ActiveSheet.Cells[e.Row, 0].Text;
                txtSName.Text = SS3.ActiveSheet.Cells[e.Row, 1].Text;
                FnPaNo = SS3.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                cboGubun.SelectedIndex = 2;
            }
            else if (sender == SS4)
            {
                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                pnlInfo.Visible = true;

                txtWrtNo.Text = SS4.ActiveSheet.Cells[e.Row, 0].Text;
                txtSName.Text = SS4.ActiveSheet.Cells[e.Row, 1].Text;
                FnPaNo = SS1.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                cboGubun.SelectedIndex = 3;
            }
            else if (sender == SS5)
            {
                if (e.RowHeader == true || e.ColumnHeader == true)
                {
                    return;
                }

                pnlInfo.Visible = true;

                txtWrtNo.Text = SS5.ActiveSheet.Cells[e.Row, 0].Text;
                txtSName.Text = SS5.ActiveSheet.Cells[e.Row, 1].Text;
                FnPaNo = SS5.ActiveSheet.Cells[e.Row, 0].Text.To<long>();

                cboGubun.SelectedIndex = 4;
            }
        }
    }
}
