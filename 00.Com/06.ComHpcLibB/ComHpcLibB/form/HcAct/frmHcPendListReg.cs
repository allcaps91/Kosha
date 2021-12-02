using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPendListReg.cs
/// Description     : 보류대장
/// Author          : 이상훈
/// Create Date     : 2019-08-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm보류대장등록.frm(Frm보류대장등록)" />

namespace ComHpcLibB
{
    public partial class frmHcPendListReg : Form
    {
        HicJepsuService hicJepsuService = null;
        HicPendingService hicPendingService = null;
        HeaJepsuService heaJepsuService = null;

        long FnWrtNo = 0;
        string FstrROWID = "";
        string FstrJepDate = "";
        string FGubun;  //일반검진 : 1 / 종검 : 2

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmHcPendListReg(long nWrtNo, string Gubun)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            FGubun = Gubun;

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicPendingService = new HicPendingService();
            heaJepsuService = new HeaJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.txtExamName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtSayu.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        void fn_Form_Load()
        {
            txtExamName.Text = "";
            txtSayu.Text = "";
            btnDelete.Enabled = false;

            if (FGubun == "1")  //일반검진
            {
                HIC_JEPSU lst = hicJepsuService.GetItembyWrtNo(FnWrtNo);

                FstrJepDate = lst.JEPDATE;
                ssPatInfo.ActiveSheet.Cells[0, 0].Text = lst.WRTNO.To<string>();
                ssPatInfo.ActiveSheet.Cells[0, 1].Text = lst.SNAME;
                ssPatInfo.ActiveSheet.Cells[0, 2].Text = lst.AGE.To<string>() + "/" + lst.SEX;
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(lst.LTDCODE.To<string>());
                ssPatInfo.ActiveSheet.Cells[0, 4].Text = FstrJepDate;
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(lst.GJJONG);
            }
            else if (FGubun == "2") //종검
            {
                HEA_JEPSU lst = heaJepsuService.GetItembyWrtNo(FnWrtNo);

                FstrJepDate = lst.JEPDATE;
                ssPatInfo.ActiveSheet.Cells[0, 0].Text = lst.WRTNO.To<string>();
                ssPatInfo.ActiveSheet.Cells[0, 1].Text = lst.SNAME;
                ssPatInfo.ActiveSheet.Cells[0, 2].Text = lst.AGE.To<string>() + "/" + lst.SEX;
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(lst.LTDCODE.To<string>());
                ssPatInfo.ActiveSheet.Cells[0, 4].Text = FstrJepDate;
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_HeaName(lst.GJJONG);
            }

            HIC_PENDING list2 = hicPendingService.GetIetmbyWrtNo(FnWrtNo, FGubun);

            FstrROWID = "";
            if (list2 != null)
            {
                FstrROWID = list2.RID;
                txtExamName.Text = list2.EXAMNAME;
                txtSayu.Text = list2.SAYU;
                btnDelete.Enabled = true;
            }
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
                HIC_PENDING item = new HIC_PENDING();

                item.JEPDATE = FstrJepDate;
                item.WRTNO = FnWrtNo;
                item.EXAMNAME = txtExamName.Text.Trim();
                item.SAYU = txtSayu.Text.Trim();
                item.ENTSABUN = clsType.User.IdNumber.To<long>();
                item.RID = FstrROWID;
                item.GUBUN = FGubun;


                if (FstrROWID == "")
                {
                    int result = hicPendingService.Insert(item);

                    if (result < 0)
                    {
                        MessageBox.Show("보류대장 입력중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    int result = hicPendingService.Update(item);

                    if (result < 0)
                    {
                        MessageBox.Show("보류대장 갱신중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("저장 완료!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                eBtnClick(btnExit, new EventArgs());
            }
            else if (sender == btnDelete)
            {
                int result = hicPendingService.DeletebyRowId(FstrROWID);

                if (result < 0)
                {
                    MessageBox.Show("보류대장 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Close();
                return;
            }
        }
    }
}
