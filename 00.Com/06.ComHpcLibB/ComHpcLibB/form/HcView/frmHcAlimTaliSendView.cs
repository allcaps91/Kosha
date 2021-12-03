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
using ComBase.Controls;
using ComLibB;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcAlimTaliSendView.cs
/// Description     : 알림톡 전송내역
/// Author          : 이상훈
/// Create Date     : 2020-07-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm알림톡전송내역.frm(Frm알림톡전송내역)" />

namespace ComHpcLibB
{
    public partial class frmHcAlimTaliSendView : Form
    {
        EtcAlimTalkService etcAlimTalkService = null;
        ComHpcLibBService comHpcLibBService = null;
        EtcAlimTalkTemplateService etcAlimTalkTemplateService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsAlimTalk Alim = new clsAlimTalk();

        public frmHcAlimTaliSendView()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            etcAlimTalkService = new EtcAlimTalkService();
            etcAlimTalkTemplateService = new EtcAlimTalkTemplateService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.txtHphone.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            List<COMHPC> list = comHpcLibBService.GetDeptName();

            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체");

            for (int i = 0; i < list.Count; i++)
            {
                cboDept.Items.Add(list[i].DEPTCODE + "." + list[i].DEPTNAMEK);
            }

            cboDept.SelectedIndex = 0;

            cboTempCD.Items.Clear();
            cboTempCD.Items.Add("**.전체");

            List<ETC_ALIMTALK_TEMPLATE> list2 = etcAlimTalkTemplateService.GetTempCdTitleRowId();

            for (int i = 0; i < list2.Count; i++)
            {
                cboTempCD.Items.Add(list2[i].TITLE + VB.Space(100)+ "." + list2[i].TEMPCD);
            }

            cboTempCD.SelectedIndex = 0;

            cboSuccess.Items.Clear();
            cboSuccess.Items.Add("*.전체");
            cboSuccess.Items.Add("1.성공");
            cboSuccess.Items.Add("2.실패");

            cboSuccess.SelectedIndex = 0;

            fn_Screen_Clear();

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
        }

        void fn_Screen_Clear()
        {
            txtSname.Text = "";
            txtHphone.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                string strRepType = "";
                string strRepCode = "";
                string strFrDate = "";
                string strToDate = "";
                string strDeptName = "";
                string strTempCd = "";
                string strReportCode = "";
                string strSName = "";
                string strPhone = "";
                int nRead = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = cf.DATE_ADD(clsDB.DbCon, dtpToDate.Text,1);

                strDeptName = VB.Left(cboDept.Text, 2);
                strTempCd = VB.Pstr(cboTempCD.Text, ".", 2);
                if (VB.Left(cboTempCD.Text,2) == "**")
                {
                    strTempCd = "**";
                }
                strReportCode = VB.Left(cboSuccess.Text, 1);
                strSName = txtSname.Text;
                strPhone = txtHphone.Text;

                sp.Spread_All_Clear(ssList);

                Application.DoEvents();

                Cursor.Current = Cursors.WaitCursor;

                List<ETC_ALIMTALK> list = etcAlimTalkService.GetItembyJobDate(strFrDate, strToDate, strDeptName, strTempCd, strReportCode, strSName, strPhone);

                if (list.Count == 0)
                {
                    return;
                }
                else
                {
                    nRead = list.Count;
                    ssList.ActiveSheet.RowCount = nRead;
                    progressBar1.Maximum = nRead;
                    for (int i = 0; i < nRead; i++)
                    {
                        strRepType = list[i].REPORT_TYPE;
                        strRepCode = list[i].REPORT_CODE;

                        ssList.ActiveSheet.Cells[i, 0].Text = list[i].JOBDATE.ToString();
                        ssList.ActiveSheet.Cells[i, 1].Text = list[i].PANO;
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].DEPTNAME;
                        ssList.ActiveSheet.Cells[i, 3].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[i, 4].Text = list[i].HPHONE;

                        if (list[i].ENTSABUN == 25420)
                        {
                            ssList.ActiveSheet.Cells[i, 5].Text = "자동전송";
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[i, 5].Text = hb.READ_HIC_InsaName(list[i].ENTSABUN.ToString()).Trim();
                        }
                        ssList.ActiveSheet.Cells[i, 6].Text = strRepType;
                        ssList.ActiveSheet.Cells[i, 7].Text = Alim.Read_AlimTalk_Send_Result(strRepType, strRepCode);
                        if (strRepCode != "0000" && !strRepCode.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[i, 8].Text = Alim.Read_AlimTalk_Send_Result("SMS", list[i].RESEND_REPORT_CODE);
                        }
                        ssList.ActiveSheet.Cells[i, 9].Text = Alim.READ_TEMPLATE_NAME(list[i].TEMPCD);
                        ssList.ActiveSheet.Cells[i, 10].Text = list[i].RDATE.ToString();
                        ssList.ActiveSheet.Cells[i, 11].Text = list[i].RETTEL;
                        ssList.ActiveSheet.Cells[i, 12].Text = list[i].QUESTLINK;

                        progressBar1.Value = i + 1;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtHphone)
            {
                if (e.KeyChar == 13)
                {
                    txtHphone.Text = string.Format("{0:000-0000-0000}", txtHphone.Text);
                }
            }
        }
    }
}
