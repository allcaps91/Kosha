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
/// File Name       : frmHcNightMunjin.cs
/// Description     : 야간작업 문진표
/// Author          : 이상훈
/// Create Date     : 2020-06-17
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm야간작업문진표(Frm야간작업문진표.frm)" />

namespace ComHpcLibB
{
    public partial class frmHcNightMunjin : Form
    {
        HicMunjinNightService hicMunjinNightService = null;
        HicJepsuService hicJepsuService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();
        clsHcFunc hf = new clsHcFunc();

        long FnAge;
        long FnPano;
        long FnWRTNO;
        long FnWrtno1;
        string FstrJepDate;
        string FstrChasu;
        string FstrPtno;
        string FstrROWID;
        bool FbAutoDisplay;
        string FstrID;

        public frmHcNightMunjin(string strID, long nWrtNo)
        {
            InitializeComponent();
            FstrID = strID;
            FnWRTNO = nWrtNo;
            SetEvent();
        }

        public frmHcNightMunjin()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicMunjinNightService = new HicMunjinNightService();
            hicJepsuService = new HicJepsuService();
            hicSunapdtlService = new HicSunapdtlService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicIeMunjinNewService = new HicIeMunjinNewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave1.Click += new EventHandler(eBtnClick);
            this.btnSave2.Click += new EventHandler(eBtnClick);
            this.btnCancel1.Click += new EventHandler(eBtnClick);
            this.btnCancel2.Click += new EventHandler(eBtnClick);

            this.txtSName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.txtMun10.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun11.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun12.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun13.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun14.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun15.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun16.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.txtMun20.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun21.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun22.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun23.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun24.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun25.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun26.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun27.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun28.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun29.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun210.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun211.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun212.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun213.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun214.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun215.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun216.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun217.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.txtMun30.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun31.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun32.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun33.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun34.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun35.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun36.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun37.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.txtMun40.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun41.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun42.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun43.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun44.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun45.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.txtMun50.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun51.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun52.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun53.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun54.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMun55.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.txtSum1.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtSum2.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtSum3.KeyPress += new KeyPressEventHandler(eKeyPress);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            
            this.txtMun10.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun11.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun12.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun13.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun14.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun15.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun16.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtMun20.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun21.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun22.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun23.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun24.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun25.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun26.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun27.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun28.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun29.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun210.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun211.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun212.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun213.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun214.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun215.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun216.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun217.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtMun30.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun31.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun32.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun33.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun34.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun35.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun36.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun37.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtMun40.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun41.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun42.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun43.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun44.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun45.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtMun50.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun51.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun52.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun53.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun54.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtMun55.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtMun10.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun11.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun12.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun13.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun14.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun15.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun16.LostFocus += new EventHandler(eTxtLostFocus);

            this.txtMun20.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun21.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun22.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun23.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun24.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun25.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun26.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun27.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun28.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun29.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun210.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun211.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun212.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun213.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun214.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun215.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun216.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun217.LostFocus += new EventHandler(eTxtLostFocus);

            this.txtMun30.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun31.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun32.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun33.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun34.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun35.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun36.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun37.LostFocus += new EventHandler(eTxtLostFocus);

            this.txtMun40.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun41.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun42.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun43.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun44.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun45.LostFocus += new EventHandler(eTxtLostFocus);

            this.txtMun50.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun51.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun52.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun53.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun54.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtMun55.LostFocus += new EventHandler(eTxtLostFocus);


            this.timer1.Tick += new EventHandler(timer1Tick);
        }

        void timer1Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            fn_Display_Munjin();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYear = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            hf.SET_자료사전_VALUE();

            nYear = int.Parse(VB.Left(clsPublic.GstrSysDate, 4));

            cboYear.Items.Clear();
            for (int i = 0; i <= 4; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYear));
                nYear -= 1;
            }

            cboYear.SelectedIndex = 0;

            FnWrtno1 = 0;
            lblMsg1.Text = "";
            lblMsg2.Text = "";

            txtSName.Text = "";
            txtWrtNo.Text = "";

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            fn_Screen_Clear();

            FbAutoDisplay = false;
            if (FstrID == "야간작업문진표")
            {
                txtWrtNo.Text = FnWRTNO.ToString();
                FbAutoDisplay = true;
                timer1.Enabled = true;
                ComFunc.Delay(1500);
            }

            ssList_Sheet1.Rows.Get(-1).Height = 21F;

            this.ActiveControl = txtMun10;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel1 || sender == btnCancel2)
            {
                if (FbAutoDisplay == true)
                {
                    this.Close();
                    return;
                }
                else
                {
                    fn_Screen_Clear();
                }
            }
            else if (sender == btnSave1 || sender == btnSave2)
            {
                long nMun = 0;
                string strDAT1 = "";
                long nJemsu1 = 0;
                string strPAN1 = "";
                string strDAT2 = "";
                long nJemsu2 = 0;
                string strPAN2 = "";
                string strDAT3 = "";
                long nJemsu3 = 0;
                string strPAN3 = "";
                int result = 0;
                string strGubun = "";
                string strDAT4 = "";
                string strDAT5 = "";

                //1차문진
                if (tabControl1.SelectedTab == tab11)
                {
                    //입력자료 오류 점검
                    for (int i = 0; i <= 6; i++)
                    {
                        TextBox txtMun1 = (Controls.Find("txtMun1" + i.ToString(), true)[0] as TextBox);
                        nMun = long.Parse(txtMun1.Text);
                        if (nMun < 1 || nMun > 5)
                        {
                            MessageBox.Show("1차문진 " + i + 1 + "항목 입력값 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        strDAT1 += VB.Left(txtMun1.Text + " ", 1);
                    }
                    nJemsu1 = long.Parse(txtSum1.Text);
                    strPAN1 = VB.Left(lblPan1.Text, 1);

                    //위장관질환(공통) 입력자료 오류 점검
                    for (int i = 0; i <= 5; i++)
                    {
                        TextBox txtMun4 = (Controls.Find("txtMun4" + i.ToString(), true)[0] as TextBox);
                        strDAT4 += VB.Left(txtMun4.Text + " ", 1);
                    }
                    //유방암(여성) 입력자료 오류 점검
                    for (int i = 0; i <= 5; i++)
                    {
                        TextBox txtMun5 = (Controls.Find("txtMun5" + i.ToString(), true)[0] as TextBox);
                        strDAT5 += VB.Left(txtMun5.Text + " ", 1);
                    }
                }
                else
                {
                    //수면의질 입력자료 오류 점검
                    for (int i = 0; i <= 17; i++)
                    {
                        TextBox txtMun2 = (Controls.Find("txtMun2" + i.ToString(), true)[0] as TextBox);
                        nMun = long.Parse(txtMun2.Text);
                        if (i == 0 || i == 2)
                        {
                            if (nMun < 1 || nMun > 24)
                            {
                                MessageBox.Show("2차문진(수면의질) " + i + 1 + "항목 입력값 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else if (nMun < 1 || nMun > 4)
                        {
                            MessageBox.Show("2차문진(수면의질) " + i + 1 + "항목 입력값 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        strDAT2 += string.Format("{0:#0}", int.Parse(txtMun2.Text)) + ",";
                    }
                    fn_Display_SleepQualty();
                    nJemsu2 = long.Parse(txtSum2.Text);
                    strPAN2 = VB.Left(lblPan2.Text, 1);

                    //주간졸림 입력자료 오류 점검
                    for (int i = 0; i <= 7; i++)
                    {
                        TextBox txtMun3 = (Controls.Find("txtMun3" + i.ToString(), true)[0] as TextBox);
                        nMun = long.Parse(txtMun3.Text);
                        if (nMun < 1 || nMun > 4)
                        {
                            MessageBox.Show("2차문진(주간졸림) " + i + 1 + "항목 입력값 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        strDAT3 += VB.Left(txtMun3.Text + " ", 1);
                    }
                    nJemsu3 = long.Parse(txtSum3.Text);
                    strPAN3 = VB.Left(lblPan3.Text, 1);
                }

                if (tabControl1.SelectedTab == tab11)
                {
                    strGubun = "0";
                }
                else if (tabControl1.SelectedTab == tab12)
                {
                    strGubun = "1";
                }

                //clsDB.setBeginTran(clsDB.DbCon);

                HIC_MUNJIN_NIGHT item = new HIC_MUNJIN_NIGHT();

                item.WRTNO = FnWRTNO;
                item.ITEM1_DATA = strDAT1;
                item.ITEM1_JEMSU = nJemsu1;
                item.ITEM1_PANJENG = strPAN1;
                item.ITEM2_DATA = strDAT2;
                item.ITEM2_JEMSU = nJemsu2;
                item.ITEM2_PANJENG = strPAN2;
                item.ITEM3_DATA = strDAT3;
                item.ITEM3_JEMSU = nJemsu3;
                item.ITEM3_PANJENG = strPAN3;
                item.ITEM4_DATA = strDAT4;
                item.ITEM5_DATA = strDAT5;
                item.ENTSABUN = long.Parse(clsType.User.IdNumber);

                result = hicMunjinNightService.SavebyWrtNo(item, strGubun);

                //if (result < 0)
                //{
                //    MessageBox.Show("야간작업 문진 DB에 저장 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    return;
                //}

                //clsDB.setCommitTran(clsDB.DbCon);

                if (FbAutoDisplay == true)
                {
                    this.Close();
                    return;
                }
                else
                {
                    fn_Screen_Clear();
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nHeight = 0;
                string strJob = "";
                string strList = "";
                int nRow = 0;
                string strOK = "";
                string strJong = "";
                string strROWID = "";
                string strFrDate = "";
                string strToDate = "";
                string strSName = "";

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                strSName = txtSName.Text.Trim();

                btnSearch.Enabled = false;

                Cursor.Current = Cursors.WaitCursor;

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 50;
                //Application.DoEvents();
                if (rdoJob1.Checked == true)
                {
                    strJob = "new";
                }
                else
                {
                    strJob = "";
                }

                //신규접수 및 접수수정 자료를 SELECT
                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetNightItembyJepDateSName(strFrDate, strToDate, strSName, FstrID);

                nREAD = list.Count;                
                ssList.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    //야간작업 대상인지 점검
                    HIC_SUNAPDTL list2 = hicSunapdtlService.GetRowIdbyWrtNoCode(list[i].WRTNO, clsHcVariable.G36_NIGHT_CODE);

                    strOK = "OK";
                    if (list2.IsNullOrEmpty())
                    {
                        strOK = "";
                    }
                    else
                    { 
                        strROWID = list2.RID;

                        if (strROWID.IsNullOrEmpty())
                        {
                            strOK = "";
                        }
                    }
                    strJong = list[i].GJJONG;

                    //야간작업 문진표가 있는지 점검
                    if (strOK == "OK")
                    {
                        strROWID = hicMunjinNightService.GetCountMunjinbyIemunNo(list[i].WRTNO);
                        if (rdoJob1.Checked == true && !strROWID.IsNullOrEmpty())
                        {
                            strOK = "";
                        }
                        if (rdoJob2.Checked == true && strROWID.IsNullOrEmpty())
                        {
                            strOK = "";
                        }
                    }

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_GjJong_Name(list[i].GJJONG);                  //검진종류
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME;                                        //성명
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].WRTNO.ToString();                             //접수번호
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].JEPDATE;                                      //접수일자
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].SEX;                                          //성별
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list[i].JUMIN2)).ToString();  //나이
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].PANO.ToString();                              //검진번호
                    }
                    progressBar1.Value = i + 1;
                }

                ssList.ActiveSheet.RowCount = nRow;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
            }
        }

        /// <summary>
        /// Display_수면의질_Jemsu
        /// </summary>
        void fn_Display_SleepQualty()
        {
            long nJemsu = 0;
            long nSum = 0;
            long nNo = 0;
            long nTime1 = 0;
            long nTime2 = 0;
            long nRate = 0;

            //1.주관적 수면의 질(질문6, 항목15)
            switch (txtMun214.Text)
            {
                case "2":
                    nJemsu += 1;    //대체로 좋다
                    break;
                case "3":
                    nJemsu += 2;    //대체로 나쁘다
                    break;
                case "4":
                    nJemsu += 3;    //아주 나쁘다
                    break;
                default:
                    break;
            }
            //2-1: 수면 잠복(질문2, 항목2)
            nSum = 0;
            switch (txtMun21.Text)
            {
                case "2":
                    nSum += 1;    //16-30분
                    break;
                case "3":
                    nSum += 2;    //31-60분
                    break;
                case "4":
                    nSum += 3;    //60분이상
                    break;
                default:
                    break;
            }
            //2-2:수면 잠복(질문5-1,항목5)
            switch (txtMun24.Text)
            {
                case "2":
                    nSum += 1;    //주1회미만
                    break;
                case "3":
                    nSum += 2;    //주1-2회
                    break;
                case "4":
                    nSum += 3;    //주3회이상
                    break;
                default:
                    break;
            }
            //2-3.수면 잠복(질문2와 질문5-1 합산 평가)
            if (nSum >= 1 && nSum <= 2)
            {
                nJemsu += 1;
            }
            else if (nSum >= 3 && nSum <= 4)
            {
                nJemsu += 2;
            }
            else if (nSum >= 5)
            {
                nJemsu += 3;
            }
            //3. 수면 기간(질문4,항목4)
            switch (txtMun23.Text)
            {
                case "2":
                    nJemsu += 1;    //6-7시간
                    break;
                case "3":
                    nJemsu += 2;    //5=6시간
                    break;
                case "4":
                    nJemsu += 3;    //5시간이하
                    break;
                default:
                    break;
            }
            //4. 습관적 수면 효과
            switch (txtMun21.Text)
            {
                case "1":
                    nTime1 = 10;
                    break;
                case "2":
                    nTime1 = 20;
                    break;
                case "3":
                    nTime1 = 45;
                    break;
                case "4":
                    nTime1 = 60;
                    break;
                default:
                    break;
            }
            switch (txtMun23.Text)
            {
                case "1":
                    nTime2 = 420;
                    break;
                case "2":
                    nTime2 = 360;
                    break;
                case "3":
                    nTime2 = 300;
                    break;
                case "4":
                    nTime2 = 240;
                    break;
                default:
                    break;
            }
            if (nTime1 > 0 && nTime2 > 0)
            {
                nRate = cf.FIX_N(nTime2 / (nTime1 + nTime2) * 100);
                if (nRate >= 85)
                {
                    nJemsu += 0;
                }
                else if (nRate >= 75)
                {
                    nJemsu += 1;
                }
                else if (nRate >= 65)
                {
                    nJemsu += 2;
                }
                else
                {
                    nJemsu += 3;
                }
            }

            //5.수면 방해
            nSum = 0;
            for (int i = 5; i <= 13; i++)
            {
                TextBox txtMun2 = (Controls.Find("txtMun2" + i.ToString(), true)[0] as TextBox);
                switch (txtMun2.Text)
                {
                    case "2":
                        nJemsu += 1;    //6-7시간
                        break;
                    case "3":
                        nJemsu += 2;    //5=6시간
                        break;
                    case "4":
                        nJemsu += 3;    //5시간이하
                        break;
                    default:
                        break;
                }
            }
            if (nSum >= 1 && nSum <= 9)
            {
                nJemsu += 1;
            }
            else if (nSum >= 10 && nSum <= 18)
            {
                nJemsu += 2;
            }
            else if (nSum >= 19)
            {
                nJemsu += 3;
            }
            //6.수면 약물 이용(질문7,항목16)
            switch (txtMun215.Text)
            {
                case "2":
                    nJemsu += 1;
                    break;
                case "3":
                    nJemsu += 2;
                    break;
                case "4":
                    nJemsu += 3;
                    break;
                default:
                    break;
            }
            //7-1.낮 시간 기능장애(질문8,항목17)
            nSum = 0;
            switch (txtMun216.Text)
            {
                case "2":
                    nSum += 1;
                    break;
                case "3":
                    nSum += 2;
                    break;
                case "4":
                    nSum += 3;
                    break;
                default:
                    break;
            }
            //7-2.낮 시간 기능장애(질문9,항목18)
            switch (txtMun217.Text)
            {
                case "2":
                    nSum += 1;
                    break;
                case "3":
                    nSum += 2;
                    break;
                case "4":
                    nSum += 3;
                    break;
                default:
                    break;
            }
            //7-3.낮 시간 기능장애(질문8,9 합산 평가)
            if (nSum >= 1 && nSum <= 2)
            {
                nJemsu += 1;
            }
            else if (nSum >= 3 && nSum <= 4)
            {
                nJemsu += 2;
            }
            else if (nSum >= 5)
            {
                nJemsu += +3;
            }

            txtSum2.Text = string.Format("{0:##0}", nJemsu);
            if (nJemsu <= 5)
            {
                lblPan2.Text = "1.수면의 질에 문제없음";
            }
            else
            {
                lblPan2.Text = "2.수면의 질이 좋지 않음";
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtWrtNo)
                {
                    int nRead = 0;
                    long nAge = 0;

                    if (txtWrtNo.Text.IsNullOrEmpty()) return;

                    if (hicSunapdtlService.GetCountbyWrtNOInCode(long.Parse(txtWrtNo.Text), clsHcVariable.G36_NIGHT_CODE) == 0)
                    {
                        MessageBox.Show("야간작업 문진 대상자가 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    FnWRTNO = long.Parse(txtWrtNo.Text);
                    fn_Display_Munjin();
                }
                else
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                txtWrtNo.Text = ssList.ActiveSheet.Cells[e.Row, 2].Text;
                FnWRTNO = long.Parse(ssList.ActiveSheet.Cells[e.Row, 2].Text);

                fn_Display_Munjin();
            }
        }

        /// <summary>
        /// 선택한 환자를 문진표 화면에 표시
        /// </summary>
        void fn_Display_Munjin()
        {
            int nREAD = 0;
            string strChasu = "";
            string strMsg = "";
            string strChar = "";
            string strJong = "";
            string strPtNo = "";
            string strDAT1 = "";
            long nJemsu1 = 0;
            string strPAN1 = "";
            string strDAT2 = "";
            long nJemsu2 = 0;
            string strPAN2 = "";
            string strDAT3 = "";
            long nJemsu3 = 0;
            string strPAN3 = "";
            string strDAT4 = "";
            string strDAT5 = "";


            //인적사항을 표시함

            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수 자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            strMsg = "[성명:" + list.SNAME + "]";
            strMsg += " 접수번호:" + FnWRTNO;
            strMsg += " 접수일자:" + list.JEPDATE;
            strMsg += " 성별:" + list.SEX + "]";
            lblMsg1.Text = strMsg;
            lblMsg2.Text = strMsg;

            FstrJepDate = list.JEPDATE;
            FnAge = list.AGE;
            FnPano = list.PANO;
            FstrChasu = list.GJCHASU;
            strJong = list.GJJONG;
            FstrPtno = list.PTNO;

            if (FstrChasu != "1" && FstrChasu != "2")
            {
                switch (strJong)
                {
                    case "16":
                    case "17":
                    case "19":
                    case "28":
                    case "29":
                    case "44":
                    case "45":
                        FstrChasu = "2";
                        break;
                    default:
                        FstrChasu = "1";
                        break;
                }
            }

            if (FstrChasu == "1")
            {
                tab11.Visible = true;
                tab12.Visible = false;
                tabControl1.SelectedTab = tab11;
            }
            else
            {
                tab11.Visible = false;
                tab12.Visible = true;
                tabControl1.SelectedTab = tab12;
            }

            //문진 자료를 Display
            HIC_MUNJIN_NIGHT list2 = hicMunjinNightService.GetItembyWrtNo(FnWRTNO);

            if (list2.IsNullOrEmpty())
            {
                btnSave1.Enabled = true;
                btnSave2.Enabled = true;
                btnCancel1.Enabled = true;
                btnCancel2.Enabled = true;
                if (FstrChasu == "1")
                {
                    txtMun10.Focus();
                    fn_NightMunjin_first_Display();
                }
                else
                {
                    txtMun30.Focus();
                }
                return;
            }

            if (FstrChasu == "1")
            {
                strDAT1 = list2.ITEM1_DATA;
                nJemsu1 = list2.ITEM1_JEMSU;
                strPAN1 = list2.ITEM1_PANJENG;
                for (int i = 0; i <= 6; i++)
                {
                    strChar = VB.Mid(strDAT1, i + 1, 1);
                    TextBox txtMun1 = (Controls.Find("txtMun1" + i.ToString(), true)[0] as TextBox);
                    txtMun1.Text = strChar;
                    SS1.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS1.ActiveSheet.Cells[i, 2].Text = (int.Parse(strChar) - 1).ToString();
                        SS1.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Panjeng("1", strChar);
                    }
                }

                txtSum1.Text = nJemsu1.ToString();
                lblPan1.Text = hf.Munjin_Night_Panjeng("1", strPAN1);
                txtMun10.Focus();

                strDAT4 = list2.ITEM4_DATA;
                for (int i = 0; i <= 5; i++)
                {
                    strChar = VB.Mid(strDAT4, i + 1, 1);
                    TextBox txtMun4 = (Controls.Find("txtMun4" + i.ToString(), true)[0] as TextBox);
                    txtMun4.Text = strChar;
                    SS4.ActiveSheet.Cells[i, 1].Text = strChar;
                }
                txtMun40.Focus();

                strDAT5 = list2.ITEM5_DATA;
                for (int i = 0; i <= 5; i++)
                {
                    strChar = VB.Mid(strDAT5, i + 1, 1);
                    TextBox txtMun5 = (Controls.Find("txtMun5" + i.ToString(), true)[0] as TextBox);
                    txtMun5.Text = strChar;
                    SS5.ActiveSheet.Cells[i, 1].Text = strChar;
                }
                txtMun50.Focus();
            }
            else
            {
                //수면의 질
                strDAT2 = list2.ITEM2_DATA;
                nJemsu2 = list2.ITEM2_JEMSU;
                strPAN2 = list2.ITEM2_PANJENG;
                for (int i = 0; i <= 17; i++)
                {
                    strChar = VB.Pstr(strDAT2, ",", i+1);
                    TextBox txtMun2 = (Controls.Find("txtMun2" + i.ToString(), true)[0] as TextBox);
                    txtMun2.Text = string.Format("{0:##}", strChar);
                    SS2.ActiveSheet.Cells[i, 1].Text = string.Format("{0:##}", strChar);
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = (long.Parse(strChar) - 1).ToString();
                        SS2.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value2(i, strChar);
                    }
                }
                txtSum2.Text = nJemsu2.ToString();
                lblPan2.Text = hf.Munjin_Night_Panjeng("2", strPAN2);
                //주간졸림증
                strDAT3 = list2.ITEM3_DATA;
                nJemsu3 = list2.ITEM3_JEMSU;
                strPAN3 = list2.ITEM3_PANJENG;
                for (int i = 0; i <= 7; i++)
                {
                    strChar = VB.Mid(strDAT3, i+1, 1);
                    TextBox txtMun3 = (Controls.Find("txtMun3" + i.ToString(), true)[0] as TextBox);
                    txtMun3.Text = strChar;
                    SS3.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS3.ActiveSheet.Cells[i, 2].Text = (long.Parse(strChar) - 1).ToString();
                        SS3.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value3(i, strChar);
                    }
                }
                txtSum3.Text = nJemsu3.ToString();
                lblPan3.Text = hf.Munjin_Night_Panjeng("3", strChar);
                txtMun30.Focus();
            }

            btnSave1.Enabled = true;
            btnSave2.Enabled = true;
            btnCancel1.Enabled = true;
            btnCancel2.Enabled = true;
        }

        /// <summary>
        /// NightMunjin_1차_Display
        /// </summary>
        void fn_NightMunjin_first_Display()
        {
            int nRow = 0;
            string strMunRes = "";
            string strTemp = "";
            string strSysDate = "";
            string strJepDate = "";
            string strMun = "";

            strSysDate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            strJepDate = Convert.ToDateTime(FstrJepDate).AddDays(2).ToShortDateString();

            HIC_IE_MUNJIN_NEW list = hicIeMunjinNewService.GetCountbyPtNoMunDate(FstrPtno, strSysDate, strJepDate);

            if (list.IsNullOrEmpty()) return;

            strMunRes = list.MUNJINRES;

            strTemp = VB.Pstr(VB.Pstr(VB.Pstr(strMunRes, "{<*>}tbl_night{*}", 2), "{*}", 2), "{<*>}", 1);
            nRow = 1;
            for (int i = 14; i <= 20; i++)
            {
                TextBox txtMun1 = (Controls.Find("txtMun1" + (i - 14).ToString(), true)[0] as TextBox);
                txtMun1.Text = VB.Pstr(VB.Pstr(strTemp, "{}", i + 1), ",", 2);
                eTxtLostFocus(txtMun1, new EventArgs());
            }

            //위장관질환(공통)
            for (int i = 21; i <= 26; i++)
            {
                TextBox txtMun4 = (Controls.Find("txtMun4" + (i - 21).ToString(), true)[0] as TextBox);
                txtMun4.Text = VB.Pstr(VB.Pstr(strTemp, "{}", i + 1), ",", 2);
                eTxtLostFocus(txtMun4, new EventArgs());
            }

            //유방암(여성)
            txtMun50.Text = VB.Pstr(VB.Pstr(strTemp, "{}", 28), ",", 2);
            txtMun55.Text = VB.Pstr(VB.Pstr(strTemp, "{}", 30), ",", 2);

            strMun = VB.Pstr(VB.Pstr(strTemp, "{}", 29), ",,", 2);
            if (!strMun.IsNullOrEmpty())
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (VB.Pstr(strMun, "|", i) == "N")
                    {
                        TextBox txtMun5 = (Controls.Find("txtMun5" + (i).ToString(), true)[0] as TextBox);
                        txtMun5.Text = "1";
                        eTxtLostFocus(txtMun5, new EventArgs());
                    }
                    else
                    {
                        TextBox txtMun5 = (Controls.Find("txtMun5" + (i).ToString(), true)[0] as TextBox);
                        txtMun5.Text = "2";
                        eTxtLostFocus(txtMun5, new EventArgs());
                    }

                }
            }
            lblInternet.Visible = true;
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtMun10 || sender == txtMun11 || sender == txtMun12 || sender == txtMun13 || sender == txtMun14 || sender == txtMun15 ||
                sender == txtMun16)
            {
                for (int i = 0; i <= 6; i++)
                {
                    TextBox txtMun1 = (Controls.Find("txtMun1" + i.ToString(), true)[0] as TextBox);
                    txtMun1.ImeMode = ImeMode.Hangul;
                    txtMun1.SelectionStart = 0;
                    txtMun1.SelectionLength = txtMun1.Text.Length;
                }
            }
            else if (sender == txtMun20 || sender == txtMun21 || sender == txtMun22 || sender == txtMun23 || sender == txtMun24 || sender == txtMun25 ||
                     sender == txtMun26 || sender == txtMun27 || sender == txtMun28 || sender == txtMun29 || sender == txtMun210 || sender == txtMun211 ||
                     sender == txtMun212 || sender == txtMun213 || sender == txtMun214 || sender == txtMun215 || sender == txtMun216 || sender == txtMun217)
            {
                for (int i = 0; i <= 17; i++)
                {
                    TextBox txtMun2 = (Controls.Find("txtMun2" + i.ToString(), true)[0] as TextBox);
                    txtMun2.ImeMode = ImeMode.Hangul;
                    txtMun2.SelectionStart = 0;
                    txtMun2.SelectionLength = txtMun2.Text.Length;

                    lblMsg2.Text = "";
                    switch (i)
                    {
                        case 0:
                            lblMsg2.Text = "잠자리에 든 시간은?(01~24)";
                            break;
                        case 2:
                            lblMsg2.Text = "일어난 시간은?(01~24)";
                            break;
                        default:
                            break;
                    }
                }

            }
            else if (sender == txtMun30 || sender == txtMun31 || sender == txtMun32 || sender == txtMun33 || sender == txtMun34 || sender == txtMun35 ||
                     sender == txtMun36 || sender == txtMun37)
            {
                for (int i = 0; i <= 7; i++)
                {
                    TextBox txtMun3 = (Controls.Find("txtMun3" + i.ToString(), true)[0] as TextBox);
                    txtMun3.ImeMode = ImeMode.Hangul;
                    txtMun3.SelectionStart = 0;
                    txtMun3.SelectionLength = txtMun3.Text.Length;
                }
            }
            else if (sender == txtMun40 || sender == txtMun41 || sender == txtMun42 || sender == txtMun43 || sender == txtMun44 || sender == txtMun45 )
            {
                for (int i = 0; i <= 5; i++)
                {
                    TextBox txtMun4 = (Controls.Find("txtMun4" + i.ToString(), true)[0] as TextBox);
                    txtMun4.ImeMode = ImeMode.Hangul;
                    txtMun4.SelectionStart = 0;
                    txtMun4.SelectionLength = txtMun4.Text.Length;
                }
            }

            else if(sender == txtMun50 || sender == txtMun51 || sender == txtMun52 || sender == txtMun53 || sender == txtMun54 || sender == txtMun55)
            {
                for (int i = 0; i <= 5; i++)
                {
                    TextBox txtMun5 = (Controls.Find("txtMun5" + i.ToString(), true)[0] as TextBox);
                    txtMun5.ImeMode = ImeMode.Hangul;
                    txtMun5.SelectionStart = 0;
                    txtMun5.SelectionLength = txtMun5.Text.Length;
                }
            }

        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            long nJemsu = 0;

            if (sender == txtMun10 || sender == txtMun11 || sender == txtMun12 || sender == txtMun13 || sender == txtMun14 || sender == txtMun15 ||
                sender == txtMun16)
            {
                for (int i = 0; i <= 6; i++)
                {
                    TextBox txtMun1 = (Controls.Find("txtMun1" + i.ToString(), true)[0] as TextBox);
                    SS1.ActiveSheet.Cells[i, 1].Text = txtMun1.Text;

                    if (!txtMun1.Text.IsNullOrEmpty())
                    {
                        if (long.Parse(txtMun1.Text) == 0)
                        {
                            SS1.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else if (long.Parse(txtMun1.Text) > 5)
                        {
                            txtMun1.Text = "";
                            txtMun1.Focus();
                            SS1.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[i, 2].Text = (long.Parse(txtMun1.Text) - 1).ToString();
                            SS1.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value1(i + 1, txtMun1.Text);
                        }
                    }

                    nJemsu = 0;
                    for (int j = 0; j <= 6; j++)
                    {
                        if (!SS1.ActiveSheet.Cells[j, 2].Text.IsNullOrEmpty())
                        {
                            nJemsu += long.Parse(SS1.ActiveSheet.Cells[j, 2].Text);
                        }
                    }
                    txtSum1.Text = nJemsu.ToString();
                    if (nJemsu <= 7)
                    {
                        lblPan1.Text = "1.정상";
                    }
                    else if (nJemsu <= 14)
                    {
                        lblPan1.Text = "2.경미한 불면증";
                    }
                    else if (nJemsu <= 21)
                    {
                        lblPan1.Text = "3.중증도 불면증";
                    }
                    else
                    {
                        lblPan1.Text = "4.심한 불면증";
                    }
                }
            }
            else if (sender == txtMun20 || sender == txtMun21 || sender == txtMun22 || sender == txtMun23 || sender == txtMun24 || sender == txtMun25 ||
                     sender == txtMun26 || sender == txtMun27 || sender == txtMun28 || sender == txtMun29 || sender == txtMun210 || sender == txtMun211 ||
                     sender == txtMun212 || sender == txtMun213 || sender == txtMun214 || sender == txtMun215 || sender == txtMun216 || sender == txtMun217)
            {
                for (int i = 0; i <= 17; i++)
                {
                    TextBox txtMun2 = (Controls.Find("txtMun2" + i.ToString(), true)[0] as TextBox);
                    SS2.ActiveSheet.Cells[i, 1].Text = txtMun2.Text;

                    if (!txtMun2.Text.IsNullOrEmpty())
                    {

                        if (long.Parse(txtMun2.Text) == 0)
                        {
                            SS2.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else if (i == 0 || i == 2)
                        {
                            if (long.Parse(txtMun2.Text) > 24)
                            {
                                txtMun2.Text = "";
                                txtMun2.Focus();
                                SS2.ActiveSheet.Cells[i, 2].Text = "";
                            }
                            else
                            {
                                SS2.ActiveSheet.Cells[i, 2].Text = (long.Parse(txtMun2.Text) - 1).ToString();
                                SS2.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value2(i + 1, txtMun2.Text);
                            }
                        }
                        else if (long.Parse(txtMun2.Text) > 4)
                        {
                            txtMun2.Text = "";
                            txtMun2.Focus();
                            SS2.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            SS2.ActiveSheet.Cells[i, 2].Text = (long.Parse(txtMun2.Text) - 1).ToString();
                            SS2.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value2(i + 1, txtMun2.Text);
                        }


                        fn_Display_SleepQualty();
                    }
                }
            }
            else if (sender == txtMun30 || sender == txtMun31 || sender == txtMun32 || sender == txtMun33 || sender == txtMun34 || sender == txtMun35 ||
                     sender == txtMun36 || sender == txtMun37)
            {
                for (int i = 0; i <= 7; i++)
                {
                    TextBox txtMun3 = (Controls.Find("txtMun3" + i.ToString(), true)[0] as TextBox);

                    SS3.ActiveSheet.Cells[i, 1].Text = txtMun3.Text;

                    if (!txtMun3.Text.IsNullOrEmpty())
                    {
                        if (long.Parse(txtMun3.Text) == 0)
                        {
                            SS3.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else if (long.Parse(txtMun3.Text) > 4)
                        {
                            txtMun3.Text = "";
                            txtMun3.Focus();
                            SS3.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            SS3.ActiveSheet.Cells[i, 2].Text = (long.Parse(txtMun3.Text) - 1).ToString();
                            SS3.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value3(i + 1, txtMun3.Text);
                        }
                    }
                }
                nJemsu = 0;
                for (int i = 0; i <= 7; i++)
                {
                    if (!SS3.ActiveSheet.Cells[i, 2].Text.IsNullOrEmpty())
                    {
                        nJemsu += long.Parse(SS3.ActiveSheet.Cells[i, 2].Text);
                    }
                }
                txtSum3.Text = nJemsu.ToString();
                if (nJemsu <= 9)
                {
                    lblPan3.Text = "1.정상";
                }
                else if (nJemsu <= 14)
                {
                    lblPan3.Text = "2.중증도 주간졸림증";
                }
                else
                {
                    lblPan3.Text = "3.심한 주간졸림증";
                }
            }
            else if (sender == txtMun40 || sender == txtMun41 || sender == txtMun42 || sender == txtMun43 || sender == txtMun44 || sender == txtMun45 )
            {
                for (int i = 0; i <= 5; i++)
                {
                    TextBox txtMun4 = (Controls.Find("txtMun4" + i.ToString(), true)[0] as TextBox);
                    SS4.ActiveSheet.Cells[i, 1].Text = txtMun4.Text;

                    if (!txtMun4.Text.IsNullOrEmpty())
                    {
                        if (long.Parse(txtMun4.Text) == 0)
                        {
                            SS4.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else if (long.Parse(txtMun4.Text) > 7)
                        {
                            txtMun4.Text = "";
                            txtMun4.Focus();
                            SS4.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            SS4.ActiveSheet.Cells[i, 2].Text = hf.Munjin_Night_Value4(i + 1, txtMun4.Text);
                        }
                    }
                }
            }
            else if (sender == txtMun50 || sender == txtMun51 || sender == txtMun52 || sender == txtMun53 || sender == txtMun54 || sender == txtMun55)
            {
                for (int i = 0; i <= 5; i++)
                {
                    TextBox txtMun5 = (Controls.Find("txtMun5" + i.ToString(), true)[0] as TextBox);
                    SS5.ActiveSheet.Cells[i, 1].Text = txtMun5.Text;

                    if (!txtMun5.Text.IsNullOrEmpty())
                    {
                        if (long.Parse(txtMun5.Text) == 0)
                        {
                            SS5.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else if (long.Parse(txtMun5.Text) > 5)
                        {
                            txtMun5.Text = "";
                            txtMun5.Focus();
                            SS5.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            SS5.ActiveSheet.Cells[i, 2].Text = hf.Munjin_Night_Value5(i + 1, txtMun5.Text);
                        }
                    }
                }
            }
        }

        void fn_Screen_Clear()
        {
            lblMsg1.Text = "";

            tab11.Visible = true;
            tab12.Visible = true;
            lblInternet.Visible = false;

            //1차문진
            for (int i = 0; i <= 6; i++)
            {
                TextBox txtMun1 = (Controls.Find("txtMun1" + i.ToString(), true)[0] as TextBox);
                txtMun1.Text = "";
            }
            txtSum1.Text = "";
            lblPan1.Text = "";
            for (int i = 0; i <= 6; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = "";
                SS1.ActiveSheet.Cells[i, 2].Text = "";
                SS1.ActiveSheet.Cells[i, 3].Text = "";
            }
            //수면의질
            for (int i = 0; i <= 17; i++)
            {
                TextBox txtMun2 = (Controls.Find("txtMun2" + i.ToString(), true)[0] as TextBox);
                txtMun2.Text = "";
            }
            lblPan2.Text = "";
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                SS2.ActiveSheet.Cells[i, 1].Text = "";
                SS2.ActiveSheet.Cells[i, 2].Text = "";
                SS2.ActiveSheet.Cells[i, 3].Text = "";
            }
            //주간졸림
            for (int i = 0; i <= 6; i++)
            {
                TextBox txtMun3 = (Controls.Find("txtMun3" + i.ToString(), true)[0] as TextBox);
                txtMun3.Text = "";
            }
            txtSum3.Text = "";
            lblPan3.Text = "";
            for (int i = 0; i <= 7; i++)
            {
                SS3.ActiveSheet.Cells[i, 1].Text = "";
                SS3.ActiveSheet.Cells[i, 2].Text = "";
                SS3.ActiveSheet.Cells[i, 3].Text = "";
            }

            for (int i = 0; i <= 5; i++)
            {
                TextBox txtMun4 = (Controls.Find("txtMun4" + i.ToString(), true)[0] as TextBox);
                txtMun4.Text = "";
            }
            for (int i = 0; i <= 5; i++)
            {
                SS4.ActiveSheet.Cells[i, 1].Text = "";
                SS4.ActiveSheet.Cells[i, 2].Text = "";
            }

            for (int i = 0; i <= 5; i++)
            {
                TextBox txtMun5 = (Controls.Find("txtMun5" + i.ToString(), true)[0] as TextBox);
                txtMun5.Text = "";
            }
            for (int i = 0; i <= 5; i++)
            {
                SS5.ActiveSheet.Cells[i, 1].Text = "";
                SS5.ActiveSheet.Cells[i, 2].Text = "";
            }

            FstrROWID = "";

            btnSave1.Enabled = false;
            btnCancel1.Enabled = false;

            btnSave2.Enabled = false;
            btnCancel2.Enabled = false;
        }
    }
}
