using ComBase;
using ComBase.Controls;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSchoolBmiCal.cs
/// Description     : 비만도 계산
/// Author          : 이상훈
/// Create Date     : 2020-03-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm비만도계산.frm(Frm비만도계산)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolBmiCal : Form
    {
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        public frmHcSchoolBmiCal()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnClear.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.cboSex.KeyPress += new KeyPressEventHandler(eComboxKeyPress);
            this.txtHeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtWeight.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtJumin.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            cboSex.Items.Clear();
            cboSex.Items.Add("M.남자");
            cboSex.Items.Add("F.여자");
            cboSex.SelectedIndex = 0;

            txtHeight.Text = "";
            txtWeight.Text = "";
            txtJumin.Text = "";
            cboSex.Text = "";
            lblBiman.Text = "";
            lblChejil.Text = "";
            lblAge.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnClear)
            {
                txtHeight.Text = "";
                txtWeight.Text = "";
                txtJumin.Text = "";
                cboSex.Text = "";
                lblBiman.Text = "";
                lblChejil.Text = "";
                lblAge.Text = "";
                txtHeight.Focus(); 
            }
            else if (sender == btnOK)
            {
                long nAge = 0;
                string strJumin = "";

                ComFunc.ReadSysDate(clsDB.DbCon);

                if (txtHeight.Text.Trim().IsNullOrEmpty())
                {
                    MessageBox.Show("키가 공란", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtWeight.Text.Trim().IsNullOrEmpty())
                {
                    MessageBox.Show("몸무게가 공란", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtJumin.Text.Trim().IsNullOrEmpty())
                {
                    MessageBox.Show("주민등록이 공란", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cboSex.Text.Trim().IsNullOrEmpty())
                {
                    MessageBox.Show("성별이 공란", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (VB.Mid(txtJumin.Text, 7, 1).IsNullOrEmpty())
                {
                    MessageBox.Show("주민번호 오류(ex:123456-1234567)", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strJumin = VB.Left(txtJumin.Text, 6) + VB.Mid(txtJumin.Text, 8, txtJumin.Text.Length - 7);
                nAge = clsVbfunc.AGE_YEAR_GESAN2(strJumin, clsPublic.GstrSysDate);
                lblAge.Text = nAge + "세";

                lblBiman.Text = "비만도 : " + hm.CALC_Biman_Rate(double.Parse(txtWeight.Text), double.Parse(txtHeight.Text), VB.Left(cboSex.Text, 1));
                lblChejil.Text = "체질량 : " + hf.CALC_Chejil_Rate(double.Parse(txtWeight.Text), double.Parse(txtHeight.Text), nAge, VB.Left(cboSex.Text, 1));
            }
        }

        void eComboxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == cboSex)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtHeight)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtWeight)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtJumin)
                {
                    switch (VB.Mid(txtJumin.Text, 8, 1))
                    {
                        case "1":
                        case "3":
                            cboSex.SelectedIndex = 0;
                            break;
                        default:
                        case "2":
                        case "4":
                            cboSex.SelectedIndex = 1;
                            break;
                    }
                    SendKeys.Send("{TAB}");
                }
            }
        }
    }
}
