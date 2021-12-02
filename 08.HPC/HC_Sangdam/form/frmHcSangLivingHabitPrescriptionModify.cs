using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Sangdam
/// File Name       : frmHcSangLivingHabitPrescriptionModify.cs
/// Description     : 생활습관개선 상담수정(임시)
/// Author          : 이상훈
/// Create Date     : 2020-01-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm생활습관개선수정.frm(Frm생활습관개선수정)" />

namespace HC_Sangdam
{
    public partial class frmHcSangLivingHabitPrescriptionModify : Form
    {
        HicResBohum1Service hicResBohum1Service = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicPatientService hicPatientService = null;
        HicJepsuPatientService hicJepsuPatientService = null;

        clsSpread sp = new clsSpread();
        clsHaBase hb = new clsHaBase();
        clsHcMain hm = new clsHcMain();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcFunc hc = new clsHcFunc();

        double FnAge;
        long FnWRTNO;
        string FstrGjJong;
        string FstrROWID;

        public frmHcSangLivingHabitPrescriptionModify()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicResBohum1Service = new HicResBohum1Service();
            hicSangdamNewService = new HicSangdamNewService();
            hicPatientService = new HicPatientService();
            hicJepsuPatientService = new HicJepsuPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.Click += new EventHandler(eTxtClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWrtNo.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            txtWrtNo.Text = "";
            FnWRTNO = 0;
            FstrROWID = "";

            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;

            for (int i = 1; i <= 5; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                chkHabit.Checked = false;
            }
            chkDepress.Checked = false;
            chkDementia.Checked = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMenuSave)
            {
                string[] strHabit = new string[5];

                //생활습관 개선필요
                for (int i = 1; i <= 5; i++)
                {
                    CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                    if (chkHabit.Checked == false)
                    {
                        strHabit[i - 1] = "0";
                    }
                    if (chkHabit.Checked == true)
                    {
                        strHabit[i - 1] = "1";
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_RES_BOHUM1 item = new HIC_RES_BOHUM1();

                item.HABIT1 = strHabit[0];
                item.HABIT2 = strHabit[1];
                item.HABIT3 = strHabit[2];
                item.HABIT4 = strHabit[3];
                item.T40_FEEL = "2";
                item.T66_STAT = "2";
                item.HABIT5 = strHabit[4];
                item.WRTNO = FnWRTNO;

                result = hicResBohum1Service.UpdateHabitbyWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("생활습관개선(문진) 저장시 오류 발생", "전산실연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HIC_SANGDAM_NEW item1 = new HIC_SANGDAM_NEW();

                item.HABIT1 = strHabit[0];
                item.HABIT2 = strHabit[1];
                item.HABIT3 = strHabit[2];
                item.HABIT4 = strHabit[3];
                item.T40_FEEL = "2";
                item.T66_STAT = "2";
                item.HABIT5 = strHabit[4];
                item.WRTNO = FnWRTNO;

                result = hicSangdamNewService.UpdateHabitbyWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("생활습관개선(상담) 저장시 오류 발생", "전산실연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtWrtNo)
            {
                txtWrtNo.SelectionStart = 0;
                txtWrtNo.SelectionLength = txtWrtNo.Text.Length;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (txtWrtNo.Text.IsNullOrEmpty())
            {
                return;
            }

            fn_Screen_Display();
        }

        void fn_Screen_Display()
        {
            FnWRTNO = txtWrtNo.Text.To<long>();
            if (FnWRTNO == 0) return;
            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show(FnWRTNO + "접수번호는 삭제된것 입니다. 확인하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담내역이 있는지 점검
            HIC_SANGDAM_NEW list = hicSangdamNewService.GetGbstsRowIdbyWrtNo(FnWRTNO);

            FstrROWID = "";
            if (!list.IsNullOrEmpty())
            {
                FstrROWID = list.RID;
            }

            //Screen_Injek_display
            //인적사항을 Display
            HIC_JEPSU_PATIENT list2 = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);
            if (list2.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FnAge = hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list2.JUMIN2));
            FstrGjJong = list2.GJJONG;

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list2.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list2.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = FnAge + "/" + list2.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list2.LTDCODE.ToString());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list2.JEPDATE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list2.GJJONG);

            //Munjin_Display
            long nTotal = 0;
            string[] str_T40_Feel = new string[2];
            string[] str_T66_Feel = new string[3];
            string[] str_T66_Memory = new string[5];

            HIC_RES_BOHUM1 list3 = hicResBohum1Service.GetItemByWrtno(FnWRTNO);

            //생활습관
            if (list3.HABIT1 == "1")
            {
                chkHabit1.Checked = true;   //음주
            }
            if (list3.HABIT2 == "1")
            {
                chkHabit2.Checked = true;   //흡연
            }
            if (list3.HABIT3 == "1")
            {
                chkHabit3.Checked = true;   //운동
            }
            if (list3.HABIT4 == "1")
            {
                chkHabit4.Checked = true;   //체중
            }
            if (list3.HABIT5 == "1")
            {
                chkHabit5.Checked = true;   //식사
            }

            //생애검진
            if (FstrGjJong == "41" || FstrGjJong == "42" || FstrGjJong == "43" || FstrGjJong == "93" || FstrGjJong == "94")
            {
                chkDementia.Enabled = true;
                chkDepress.Enabled = true;

                str_T40_Feel[1] = list3.T40_FEEL3;
                str_T40_Feel[2] = list3.T40_FEEL4;

                str_T66_Feel[1] = list3.T66_FEEL1;
                str_T66_Feel[2] = list3.T66_FEEL2;
                str_T66_Feel[3] = list3.T66_FEEL3;

                str_T66_Memory[1] = list3.T66_MEMORY1;
                str_T66_Memory[2] = list3.T66_MEMORY2;
                str_T66_Memory[3] = list3.T66_MEMORY3;
                str_T66_Memory[4] = list3.T66_MEMORY4;
                str_T66_Memory[5] = list3.T66_MEMORY5;

                if (FnAge == 40)
                {
                    if (string.Compare(str_T40_Feel[0], "2") > 0 || string.Compare(str_T40_Feel[1], "2") > 0)
                    {
                        chkDepress.Checked = true;
                    }
                }
                else if (FnAge == 66 || FnAge == 70 || FnAge == 74)
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        nTotal += long.Parse(str_T66_Memory[i]) - 1;
                    }

                    if (nTotal > 3)
                    {
                        chkDepress.Checked = true;
                    }
                    if (FnAge == 66)
                    {
                        for (int i = 0; i <= 2; i++)
                        {
                            if (str_T66_Feel[i] == "1")
                            {
                                chkDepress.Checked = true;
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                chkDementia.Enabled = false;
                chkDepress.Enabled = false;
            }

            //SangDam_Display
            HIC_SANGDAM_NEW list4 = hicSangdamNewService.GetHabitbyWrtNo(FnWRTNO);

            //생활습관 개선필요
            if (list4.HABIT1 == "1")
            {
                if (chkHabit1.Checked == false)
                {
                    chkHabit1.Checked = true;
                }
            }
            if (list4.HABIT2 == "1")
            {
                if (chkHabit2.Checked == false)
                {
                    chkHabit2.Checked = true;
                }
            }
            if (list4.HABIT3 == "1")
            {
                if (chkHabit3.Checked == false)
                {
                    chkHabit3.Checked = true;
                }
            }
            if (list4.HABIT4 == "1")
            {
                if (chkHabit4.Checked == false)
                {
                    chkHabit4.Checked = true;
                }
            }
            if (list4.HABIT5 == "1")
            {
                if (chkHabit5.Checked == false)
                {
                    chkHabit5.Checked = true;
                }
            }
        }

        void fn_Screen_Clear()
        {
            txtWrtNo.Text = "";
            FnWRTNO = 0;
            FstrROWID = "";

            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;

            for (int i = 1; i <= 5; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                chkHabit.Checked = false;
            }

            chkDepress.Checked = false;
            chkDementia.Checked = false;
        }
    }
}
