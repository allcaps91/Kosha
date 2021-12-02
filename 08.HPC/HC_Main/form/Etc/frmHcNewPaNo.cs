using ComBase;
using ComHpcLibB;
using ComHpcLibB.Service;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcNewPaNo.cs
/// Description     : 신규번호 부여
/// Author          : 이상훈
/// Create Date     : 2019-10-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain51.frm(FrmNewPano)" />

namespace HC_Main
{
    public partial class frmHcNewPaNo : Form
    {
        HicPatientService hicPatientService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hc = new clsHcFunc();

        long FPaNo;

        public frmHcNewPaNo(long PaNo)
        {
            InitializeComponent();

            FPaNo = PaNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicPatientService = new HicPatientService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.txtJumin1.KeyPress += new KeyPressEventHandler(etxtKeyPress);
            this.txtJumin2.KeyPress += new KeyPressEventHandler(etxtKeyPress);
        }

        void etxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtJumin1)
            {
                if (txtJumin1.Text.Length == 6)
                {
                    if (e.KeyChar == 13)
                    {
                        SendKeys.Send("{TAB}");
                    }
                }
            }
            else if (sender == txtJumin2)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            txtJumin1.Text = "";
            txtJumin2.Text = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string sMsg = "";

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnOK)
            {
                string strJumin = "";

                txtJumin1.Text = txtJumin1.Text.Trim();
                txtJumin2.Text = txtJumin2.Text.Trim();
                if (txtJumin1.Text.Length != 6)
                {
                    MessageBox.Show("주민번호(1)를 정확하게 입력하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtJumin1.Focus();
                    return;
                }
                if (txtJumin2.Text.Length != 6)
                {
                    MessageBox.Show("주민번호(2)를 정확하게 입력하세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtJumin2.Focus();
                    return;
                }

                if (hc.JuminNo_Check_New(txtJumin1.Text.Trim(), txtJumin2.Text.Trim()) == "ERROR")
                {
                    sMsg = "▶주민등록번호 오류◀" + "\r\n\r\n";
                    sMsg += "주민등록번호에 오류가 있습니다." + "\r\n";
                    sMsg += "신환번호를 부여하시겠습니까?";
                    if (MessageBox.Show(sMsg, "선택", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                //건강진단 마스타에 동일한 주민등록번호가 있는지 Check
                FPaNo = hicPatientService.GetPanobyJumin(clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()));

                if (FPaNo != 0)
                {
                    this.Close();
                    return;
                }

                if (MessageBox.Show("새로운 검진 번호를 부여 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    FPaNo = 0;
                    this.Close();
                    return;
                }

                FPaNo = hb.New_PatientNo_Create();

                strJumin = txtJumin1.Text + txtJumin2.Text;

                //신규번호를 건강진단 환자마스타에 INSERT
                int result = hicPatientService.InsertPatient(FPaNo, VB.Left(strJumin, 7) + "******", clsAES.AES(strJumin));

                if (result < 0)
                {
                    MessageBox.Show("신규번호 부여중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Close();
                return;
            }
        }
    }
}
