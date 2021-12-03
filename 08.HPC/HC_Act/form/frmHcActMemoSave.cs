using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
//using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcActMemoSave.cs
/// Description     : 검사결과 메모입력
/// Author          : 이상훈
/// Create Date     : 2020-10-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "신규" />

namespace HC_Act
{
    public partial class frmHcActMemoSave : Form
    {
        HicMemoService hicMemoService = null;
        HicJepsuPatientService hicJepsuPatientService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWrtNo;

        public frmHcActMemoSave()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcActMemoSave(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicMemoService = new HicMemoService();
            hicJepsuPatientService = new HicJepsuPatientService(); 

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMemoSave.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (!FnWrtNo.IsNullOrEmpty())
            {
                txtWrtNo.Text = FnWrtNo.To<string>();
                eTxtKeyPress(txtWrtNo, new KeyPressEventArgs((char)Keys.Enter));
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMemoSave)
            {
                fn_Hic_Memo_Save();
                fn_Hic_Memo_Screen();
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtWrtNo)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    if (!txtWrtNo.Text.IsNullOrEmpty())
                    {
                        HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

                        if (list.IsNullOrEmpty())
                        {
                            MessageBox.Show(txtWrtNo.Text.Trim() + " 접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        ssPatInfo.ActiveSheet.RowCount = 1;

                        ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
                        ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
                        ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
                        ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
                        ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.To<string>();
                        ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);

                        fn_Hic_Memo_Screen();
                    }
                }
            }
        }

        void fn_Hic_Memo_Screen()
        {
            long nPano = 0;
            int nRow = 0;
            int nRead = 0;

            sp.Spread_All_Clear(SS_ETC);

            nPano = ssPatInfo.ActiveSheet.Cells[0, 0].Text.Trim().To<long>();
            if (nPano == 0) return;

            //참고사항 Display
            List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(nPano);

            nRead = list.Count;
            SS_ETC.ActiveSheet.RowCount = nRead + 5;
            for (int i = 0; i < nRead; i++)
            {
                SS_ETC.ActiveSheet.Cells[i, 1].Text = list[i].ENTTIME.To<string>();
                SS_ETC.ActiveSheet.Cells[i, 2].Text = list[i].MEMO;

                SS_ETC.ActiveSheet.Cells[i, 3].Text = list[i].JOBNAME;//hm.READ_HIC_Doctor_Name(list[i].JOBSABUN.To<string>());
                SS_ETC.ActiveSheet.Cells[i, 4].Text = list[i].RID;
            }
        }

        void fn_Hic_Memo_Save()
        {
            long nPano = 0;
            string strCODE = "";
            string strMEMO = "";
            string strROWID = "";
            string strOK = "";
            string strTime = "";
            int result = 0;

            nPano = ssPatInfo.ActiveSheet.Cells[0, 0].Text.To<long>();
            if (nPano == 0) return;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS_ETC.ActiveSheet.NonEmptyRowCount; i++)
            {
                strOK = SS_ETC.ActiveSheet.Cells[i, 0].Text.Trim();
                strTime = SS_ETC.ActiveSheet.Cells[i, 1].Text.Trim();
                strMEMO = SS_ETC.ActiveSheet.Cells[i, 2].Text.Trim();
                strROWID = SS_ETC.ActiveSheet.Cells[i, 4].Text.Trim();
                if (!strROWID.IsNullOrEmpty())
                {
                    if (strOK == "True")
                    {
                        result = hicMemoService.UpdatebyRowId(strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("수검자 메모 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else if (strTime.IsNullOrEmpty() && !strMEMO.IsNullOrEmpty())
                {
                    HIC_MEMO item = new HIC_MEMO();

                    item.PANO = nPano;
                    item.MEMO = strMEMO;
                    item.JOBSABUN = long.Parse(clsType.User.IdNumber);
                    item.PTNO = string.Format("{0:00000000}", nPano);

                    result = hicMemoService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("수검자 메모 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }
    }
}
