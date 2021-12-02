using ComBase;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using ComBase.Controls;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHicPcSet.cs
/// Description     : 계측항목 Setting
/// Author          : 이상훈
/// Create Date     : 2019-08-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSet1.frm(FrmPCSet)" />

namespace HC_Act
{
    public partial class frmHicPcSet : Form
    {
        HicCodeService hicCodeService = null;
        HeaCodeService heaCodeService = null;
        BasPcconfigService basPcconfigService = null;
        HicWaitRoomService hicWaitRoomService = null;

        clsHcFunc hc = new clsHcFunc();

        string FstrBuse;

        public frmHicPcSet()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            heaCodeService = new HeaCodeService();
            basPcconfigService = new BasPcconfigService();
            hicWaitRoomService = new HicWaitRoomService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.rdoBuse1.Click += new EventHandler(eRdoClick);
            this.rdoBuse2.Click += new EventHandler(eRdoClick);
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (chkModify.Checked == true)
            {
                for (int i = 1; i <= 10; i++)
                {
                    ComboBox cboExam = (Controls.Find("cboExam" + i.ToString(), true)[0] as ComboBox);
                    cboExam.Items.Clear();
                }

                if (sender == rdoBuse1)
                {
                    pnlHcSet.Visible = false;
                    pnlHaSet.Visible = true;
                    
                    //List<HIC_CODE> listCode = hicCodeService.Hic_Part_Jepsu("A9", "CODE"); //종검
                    List<HEA_CODE> listCode = heaCodeService.Hea_Part_Jepsu("11"); //종검



                    //'**' AS CODE, '선택안함' as Name

                    cboExam1.Items.Add("**.선택안함");
                    cboExam1.SetItems(listCode, "NAME", "CODE");
                    cboExam2.Items.Add("**.선택안함");
                    cboExam2.SetItems(listCode, "NAME", "CODE");
                    cboExam3.Items.Add("**.선택안함");
                    cboExam3.SetItems(listCode, "NAME", "CODE");
                    cboExam4.Items.Add("**.선택안함");
                    cboExam4.SetItems(listCode, "NAME", "CODE");
                    cboExam5.Items.Add("**.선택안함");
                    cboExam5.SetItems(listCode, "NAME", "CODE");
                    cboExam6.Items.Add("**.선택안함");
                    cboExam6.SetItems(listCode, "NAME", "CODE");
                    cboExam7.Items.Add("**.선택안함");
                    cboExam7.SetItems(listCode, "NAME", "CODE");
                    cboExam8.Items.Add("**.선택안함");
                    cboExam8.SetItems(listCode, "NAME", "CODE");
                    cboExam9.Items.Add("**.선택안함");
                    cboExam9.SetItems(listCode, "NAME", "CODE");
                    cboExam10.Items.Add("**.선택안함");
                    cboExam10.SetItems(listCode, "NAME", "CODE");
                }
                else if (sender == rdoBuse2)
                {
                    pnlHcSet.Visible = true;
                    pnlHaSet.Visible = false;
                    chkAct.Checked = false;
                    chkMonitor.Checked = false;
                    chkInbody.Checked = false;
                    chkHcWait.Checked = false;

                    List<HIC_CODE> listCode = hicCodeService.Hic_Part_Jepsu("72", "SORT"); //일검

                    cboExam1.Items.Add("**.선택안함");
                    cboExam1.SetItems(listCode, "NAME", "CODE");
                    cboExam2.Items.Add("**.선택안함");
                    cboExam2.SetItems(listCode, "NAME", "CODE");
                    cboExam3.Items.Add("**.선택안함");
                    cboExam3.SetItems(listCode, "NAME", "CODE");
                    cboExam4.Items.Add("**.선택안함");
                    cboExam4.SetItems(listCode, "NAME", "CODE");
                    cboExam5.Items.Add("**.선택안함");
                    cboExam5.SetItems(listCode, "NAME", "CODE");
                    cboExam6.Items.Add("**.선택안함");
                    cboExam6.SetItems(listCode, "NAME", "CODE");
                    cboExam7.Items.Add("**.선택안함");
                    cboExam7.SetItems(listCode, "NAME", "CODE");
                    cboExam8.Items.Add("**.선택안함");
                    cboExam8.SetItems(listCode, "NAME", "CODE");
                    cboExam9.Items.Add("**.선택안함");
                    cboExam9.SetItems(listCode, "NAME", "CODE");
                    cboExam10.Items.Add("**.선택안함");
                    cboExam10.SetItems(listCode, "NAME", "CODE");
                }

                for (int i = 1; i <= 10; i++)
                {
                    ComboBox cboExam = (Controls.Find("cboExam" + i.ToString(), true)[0] as ComboBox);
                    cboExam.SelectedIndex = 0;
                }
            }
            else
            {
                if (sender == rdoBuse1)
                {
                    pnlHcSet.Visible = false;
                    pnlHaSet.Visible = true;
                    chkHcWait.Checked = false;
                }
                else if (sender == rdoBuse2)
                {
                    pnlHcSet.Visible = true;
                    pnlHaSet.Visible = false;
                    chkAct.Checked = false;
                    chkMonitor.Checked = false;
                    chkInbody.Checked = false;
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsCompuInfo.SetComputerInfo();
            fn_Form_Load();            
        }

        void fn_Form_Load()
        {
            FstrBuse = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");

            if (!FstrBuse.IsNullOrEmpty())
            {   
                if (FstrBuse == "1")
                {
                    rdoBuse1.Checked = true;
                    rdoBuse2.Checked = false;

                    pnlHcSet.Visible = true;
                    pnlHaSet.Visible = true;
                }
                else if (FstrBuse == "2")
                {
                    rdoBuse1.Checked = false;
                    rdoBuse2.Checked = true;

                    pnlHcSet.Visible = true;
                    pnlHaSet.Visible = false;
                    chkAct.Checked = false;
                    chkMonitor.Checked = false;
                    chkInbody.Checked = false;
                    chkHcWait.Checked = false;
                }
                else
                {
                    rdoBuse1.Checked = false;
                    rdoBuse2.Checked = false;

                    pnlHcSet.Visible = false;
                    pnlHaSet.Visible = false;
                    chkAct.Checked = false;
                    chkMonitor.Checked = false;
                    chkInbody.Checked = false;
                    chkHcWait.Checked = false;
                }
            }

            for (int i = 1; i <= 10; i++)
            {
                ComboBox cboExam = (Controls.Find("cboExam" + i.ToString(), true)[0] as ComboBox);
                cboExam.Items.Clear();
            }

            if (rdoBuse1.Checked == true)
            {
                //List<HIC_CODE> listCode = hicCodeService.Hic_Part_Jepsu("A9", "CODE"); //종검
                List<HEA_CODE> listCode = heaCodeService.Hea_Part_Jepsu("11"); //종검

                cboExam1.Items.Add("**.선택안함");
                cboExam1.SetItems(listCode, "NAME", "CODE");
                cboExam2.Items.Add("**.선택안함");
                cboExam2.SetItems(listCode, "NAME", "CODE");
                cboExam3.Items.Add("**.선택안함");
                cboExam3.SetItems(listCode, "NAME", "CODE");
                cboExam4.Items.Add("**.선택안함");
                cboExam4.SetItems(listCode, "NAME", "CODE");
                cboExam5.Items.Add("**.선택안함");
                cboExam5.SetItems(listCode, "NAME", "CODE");
                cboExam6.Items.Add("**.선택안함");
                cboExam6.SetItems(listCode, "NAME", "CODE");
                cboExam7.Items.Add("**.선택안함");
                cboExam7.SetItems(listCode, "NAME", "CODE");
                cboExam8.Items.Add("**.선택안함");
                cboExam8.SetItems(listCode, "NAME", "CODE");
                cboExam9.Items.Add("**.선택안함");
                cboExam9.SetItems(listCode, "NAME", "CODE");
                cboExam10.Items.Add("**.선택안함");
                cboExam10.SetItems(listCode, "NAME", "CODE");
            }
            else
            {
                List<HIC_CODE> listCode = hicCodeService.Hic_Part_Jepsu("72", "SORT"); //일검

                cboExam1.Items.Add("**.선택안함");
                cboExam1.SetItems(listCode, "NAME", "CODE");
                cboExam2.Items.Add("**.선택안함");
                cboExam2.SetItems(listCode, "NAME", "CODE");
                cboExam3.Items.Add("**.선택안함");
                cboExam3.SetItems(listCode, "NAME", "CODE");
                cboExam4.Items.Add("**.선택안함");
                cboExam4.SetItems(listCode, "NAME", "CODE");
                cboExam5.Items.Add("**.선택안함");
                cboExam5.SetItems(listCode, "NAME", "CODE");
                cboExam6.Items.Add("**.선택안함");
                cboExam6.SetItems(listCode, "NAME", "CODE");
                cboExam7.Items.Add("**.선택안함");
                cboExam7.SetItems(listCode, "NAME", "CODE");
                cboExam8.Items.Add("**.선택안함");
                cboExam8.SetItems(listCode, "NAME", "CODE");
                cboExam9.Items.Add("**.선택안함");
                cboExam9.SetItems(listCode, "NAME", "CODE");
                cboExam10.Items.Add("**.선택안함");
                cboExam10.SetItems(listCode, "NAME", "CODE");
            }

            //검사항목 ComboBox 10개 한꺼번에 매칭
            List<BAS_PCCONFIG> Configlst = basPcconfigService.GetConfig(clsCompuInfo.gstrCOMIP);
            for (int k = 0; k < Configlst.Count; k++)                
            {
                ComboBox cboExam = (Controls.Find("cboExam" + (k + 1).ToString(), true)[0] as ComboBox);

                for (int j = 0; j < cboExam.Items.Count ; j++) 
                {
                    cboExam.SelectedIndex = j;
                    if (VB.Pstr(cboExam.Text, ".", 1) == Configlst[k].CODE)
                    {
                        cboExam.SelectedIndex = j;
                        break;
                    }
                }
            }

            BAS_PCCONFIG Config_PftSNlst = basPcconfigService.GetConfig_PFTSN(clsCompuInfo.gstrCOMIP);
            //select* FROM KOSMOS_PMPA.BAS_PCCONFIG
            // WHERE GUBUN = '폐활량장비S/N'

            if (Config_PftSNlst != null)
            {
                clsHcVariable.GstrPFTSN = Config_PftSNlst.VALUEV;   //폐활량장비S/N
            }

            clsHcVariable.GstrHeaAutoActingYN = "";
            clsHcVariable.GstrHeaMonitorOffYN = "";
            clsHcVariable.GstrHeaInbodySendYN = "";

            clsHcVariable.GstrHicPart = basPcconfigService.GetConfig_Code(clsCompuInfo.gstrCOMIP, "검진센터부서");
            clsHcVariable.GstrHicPartName = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "검진센터부서");
            clsHcVariable.GstrHeaAutoActingYN = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "자동액팅PC여부");
            clsHcVariable.GstrHeaMonitorOffYN = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "대기순번모니터끄기여부");
            clsHcVariable.GstrHeaInbodySendYN = basPcconfigService.GetConfig_Check(clsCompuInfo.gstrCOMIP, "InBody전송PC여부");

            if (clsHcVariable.GstrHeaAutoActingYN == "Y")
            {
                chkAct.Checked = true;
            }
            else
            {
                chkAct.Checked = false;
            }

            if (clsHcVariable.GstrHeaMonitorOffYN == "Y")
            {
                chkMonitor.Checked = true;
            }
            else
            {
                chkMonitor.Checked = false;
            }
            if (clsHcVariable.GstrHeaInbodySendYN == "Y")
            {
                chkInbody.Checked = true;
            }
            else
            {
                chkInbody.Checked = false;
            }

            HIC_WAIT_ROOM item = hicWaitRoomService.GetItemByRoom("00");
            if (item.AMUSE == "Y") { chkHcWait.Checked = true; }

            txtPFTSN.Text = clsHcVariable.GstrPFTSN;
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
                int FNum = 0;
                string sIpAddress = "";
                string strPrtYN = "";
                string strRec = "";
                string sGubun = "";
                string sCode = "";
                string sValuev = "";
                long nValuen = 0;
                int nDisSeqNo = 0;
                string sInpDate = "";
                string sInpTime = "";
                string sDelGb = "";
                int result = 0;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsDB.setBeginTran(clsDB.DbCon);

                sIpAddress = clsCompuInfo.gstrCOMIP;
                sInpDate = clsPublic.GstrSysDate.Replace("-", "");
                sInpTime = clsPublic.GstrSysTime.Replace(":", "") + "00";
                sDelGb = "0";

                result = basPcconfigService.Delete_PcConfig(sIpAddress);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (rdoBuse1.Checked == true)
                {
                    sCode = "1";
                    sValuev = "종합검진";
                }
                else if (rdoBuse2.Checked == true)
                {
                    sCode = "2";
                    sValuev = "일반검진";
                }
                else if (rdoBuse1.Checked == false && rdoBuse2.Checked == false)
                {
                    sCode = "";
                    sValuev = "";
                    MessageBox.Show("부서선택을 하신 후 저장 바랍니다!", "항목미선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                sGubun = "검진센터부서";

                nDisSeqNo += 1;

                BAS_PCCONFIG item = new BAS_PCCONFIG();

                item.IPADDRESS = sIpAddress;
                item.GUBUN = sGubun;
                item.CODE = sCode;
                item.VALUEV = sValuev;
                item.VALUEN = nValuen;
                item.DISSEQNO = nDisSeqNo;
                item.INPDATE = sInpDate;
                item.INPTIME = sInpTime;
                item.DELGB = sDelGb;

                //int result2 = basPcconfigService.Save_PcConfig(item);
                result = basPcconfigService.Save_PcConfig_Test(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                string strRoom = "";
                string strROWID = "";
                strRoom = "00";
                strROWID = hicWaitRoomService.GetCountbyRoomCd(strRoom);
                HIC_WAIT_ROOM item6 = new HIC_WAIT_ROOM();

                item6.ROOM = strRoom;
                item6.ROOMNAME = "부인과검사중";
                item6.AMUSE = "N";
                item6.PMUSE = "N";
                item6.RID = strROWID;

                if (chkHcWait.Checked == true)
                {
                    item6.AMUSE = "Y";
                    item6.PMUSE = "Y";
                }

                int result1 = hicWaitRoomService.Update(item6);

                if (result < 0)
                {
                    MessageBox.Show("부인과검사중 구분 업데이트 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (chkAct.Checked == true)
                {
                    sGubun = "자동액팅PC여부";
                    sCode = "1";
                    sValuev = "Y";

                    BAS_PCCONFIG item1 = new BAS_PCCONFIG();

                    item1.IPADDRESS = sIpAddress;
                    item1.GUBUN = sGubun;
                    item1.CODE = sCode;
                    item1.VALUEV = sValuev;
                    item1.VALUEN = nValuen;
                    item1.DISSEQNO = nDisSeqNo;
                    item1.INPDATE = sInpDate;
                    item1.INPTIME = sInpTime;
                    item1.DELGB = sDelGb;

                    //result= basPcconfigService.Save_PcConfig(item1);
                    result = basPcconfigService.Save_PcConfig_Test(item1);
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (chkMonitor.Checked == true)
                {   
                    sGubun = "대기순번모니터끄기여부";
                    sCode = "2";
                    sValuev = "Y";

                    BAS_PCCONFIG item2 = new BAS_PCCONFIG();

                    item2.IPADDRESS = sIpAddress;
                    item2.GUBUN = sGubun;
                    item2.CODE = sCode;
                    item2.VALUEV = sValuev;
                    item2.VALUEN = nValuen;
                    item2.DISSEQNO = nDisSeqNo;
                    item2.INPDATE = sInpDate;
                    item2.INPTIME = sInpTime;
                    item2.DELGB = sDelGb;

                    //int result = basPcconfigService.Save_PcConfig(item2);
                    result = basPcconfigService.Save_PcConfig_Test(item2);
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (chkInbody.Checked == true)
                {
                    sGubun = "InBody전송PC여부";
                    sCode = "3";
                    sValuev = "Y";

                    BAS_PCCONFIG item3 = new BAS_PCCONFIG();

                    item3.IPADDRESS = sIpAddress;
                    item3.GUBUN = sGubun;
                    item3.CODE = sCode;
                    item3.VALUEV = sValuev;
                    item3.VALUEN = nValuen;
                    item3.DISSEQNO = nDisSeqNo;
                    item3.INPDATE = sInpDate;
                    item3.INPTIME = sInpTime;
                    item3.DELGB = sDelGb;

                    //int result = basPcconfigService.Save_PcConfig(item3);
                    result = basPcconfigService.Save_PcConfig_Test(item3);
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //if (!string.IsNullOrEmpty(txtPFTSN.Text.Trim()))
                //{
                    sGubun = "폐활량장비S/N";
                    sCode = "01";
                    sValuev = txtPFTSN.Text.Trim();

                    BAS_PCCONFIG item4 = new BAS_PCCONFIG();

                    item4.IPADDRESS = sIpAddress;
                    item4.GUBUN = sGubun;
                    item4.CODE = sCode;
                    item4.VALUEV = sValuev;
                    item4.VALUEN = nValuen;
                    item4.DISSEQNO = nDisSeqNo;
                    item4.INPDATE = sInpDate;
                    item4.INPTIME = sInpTime;
                    item4.DELGB = sDelGb;

                    //int result = basPcconfigService.Save_PcConfig(item4);
                    result = basPcconfigService.Save_PcConfig_Test(item4);
                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                //}

                for (int i = 1; i <= 10; i++)
                {
                    ComboBox cbo = (Controls.Find("cboExam" + i.ToString(), true)[0] as ComboBox);

                    if (!cbo.Text.IsNullOrEmpty())
                    {
                        sGubun = "검진센터계측항목";
                        sCode = VB.Pstr(cbo.Text, ".", 1);
                        if (sCode != "**" && !sCode.IsNullOrEmpty())
                        {
                            sValuev = VB.Mid(cbo.Text.Trim(), cbo.Text.IndexOf(".") + 2, cbo.Text.Length);

                            BAS_PCCONFIG item5 = new BAS_PCCONFIG();

                            item5.IPADDRESS = sIpAddress;
                            item5.GUBUN = sGubun;
                            item5.CODE = sCode;
                            item5.VALUEV = sValuev;
                            item5.VALUEN = nValuen;
                            item5.DISSEQNO = nDisSeqNo;
                            item5.INPDATE = sInpDate;
                            item5.INPTIME = sInpTime;
                            item5.DELGB = sDelGb;

                            //int result = basPcconfigService.Save_PcConfig(item5);
                            result = basPcconfigService.Save_PcConfig_Test(item5);
                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("등록중 오류 발생!!!", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else if (sender == btnCancel)
            {
                fn_Form_Load();
            }
        }
    }
}
