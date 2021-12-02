using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmXrayAutoActing.cs
/// Description     : 방사선 자동 엑팅
/// Author          : 이상훈
/// Create Date     : 2019-08-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm방사선자동액팅.frm(Frm방사선자동액팅)" />

namespace HC_Act
{
    public partial class frmXrayAutoActing : Form
    {
        HicJepsuResultService hicJepsuResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HicResultService hicResultService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicWaitRoomService hicWaitRoomService = null;

        clsHcAct ha = new clsHcAct();

        int FnTimer = 0;
        long FnWRTNO = 0;
        long FnPano = 0;

        public frmXrayAutoActing()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuResultService = new HicJepsuResultService();
            hicXrayResultService = new HicXrayResultService();
            hicResultService = new HicResultService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicWaitRoomService = new HicWaitRoomService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.chkStop.Click += new EventHandler(eCheckBoxClick);
            this.timer1.Tick += new EventHandler(eTimerTick);
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            if (sender == chkStop)
            {
                if (chkStop.Checked == true)
                {
                    timer1.Enabled = false;
                }
                else
                {
                    timer1.Enabled = true;
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
            FnTimer = 20;
            progressBar1.Value = 0;
            progressBar1.Maximum = 20;
            timer1.Enabled = true;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string strRoom = "";
            string strRoom1 = "";
            string strRoom2 = "";
            string strRoom3 = "";
            string strRoom4 = "";
            string strNextRoom = "";
            string strRemark = "";
            int nWait = 0;
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            string strGjJong = "";
            long nWRTNO = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        void eTimerTick(object sender, EventArgs e)
        {
            FnTimer += 1;
            if (FnTimer > progressBar1.Maximum)
            {
                FnTimer = progressBar1.Maximum;
            }
            progressBar1.Value = FnTimer;
            Application.DoEvents();

            if (FnTimer >= progressBar1.Maximum)
            {
                FnTimer = 0;
                timer1.Enabled = false;
                fn_XRay_Auto_SET(); //자동 완료 SET
                timer1.Enabled = true;
            }
        }

        /// <summary>
        /// 건진 방사선 자동 촬영완료 SET
        /// </summary>
        void fn_XRay_Auto_SET()
        {
            int nREAD = 0;
            string strSname = "";
            string strROWID = "";
            string strOK = "";
            string strData = "";
            string strNextRoom = "";
            string strTemp = "";

            List<HIC_JEPSU_RESULT> lst = hicJepsuResultService.GetItem();

            nREAD = lst.Count;
            FnWRTNO = 0;
            for (int i = 0; i < nREAD; i++)
            {
                FnWRTNO = lst[i].WRTNO;
                FnPano = lst[i].PANO;
                strSname = lst[i].SNAME;
                strROWID = lst[i].RID;

                //영상이 있는지 점검
                if (hicXrayResultService.GetCountbyPaNo(FnPano) > 0)
                {
                    strOK = "OK";
                }

                //촬영완료 SET
                if (strOK == "OK")
                {
                    int result = hicResultService.Update_Hic_Result_Complete(clsType.User.IdNumber, strROWID);

                    if (result < 0)
                    {
                        MessageBox.Show("촬영완료 Setting 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    HIC_SANGDAM_WAIT list = hicSangdamWaitService.GetNextRoomByWrtNoGubun(FnWRTNO, "10");

                    if (list == null)
                    {
                        strNextRoom = "";
                    }
                    else
                    {
                        strNextRoom = list.NEXTROOM;
                        strData = VB.Left(strNextRoom, 2).Trim();
                        if (!strData.IsNullOrEmpty())
                        {
                            if (strData == "30")
                            {
                                strNextRoom = "1번방(시력)";
                            }
                            else if (strData == "31")
                            {
                                strNextRoom = "3번방(혈압)";
                            }
                            else if (strData == "32")
                            {
                                strNextRoom = "4번방(채혈실)";
                            }
                            else if (strData == "33")
                            {
                                strNextRoom = "접수창구제출";
                            }
                            else
                            {
                                strTemp = hicWaitRoomService.GetRoomNamebyRoom(strData);
                                if (!strTemp.IsNullOrEmpty())
                                {
                                    strNextRoom = strData + "." + strTemp;
                                }
                            }
                        }
                    }

                    if (strNextRoom.IsNullOrEmpty())
                    {
                        List1.Items.Add(string.Format("{0:0000000}", FnWRTNO) + " " + strSname);
                    }
                    else
                    {
                        List1.Items.Add(string.Format("{0:0000000}", FnWRTNO) + " " + strSname + " " + strNextRoom);
                        if (ha.WAIT_NextRoom_SET(FnWRTNO, FnPano, "10") == false)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
