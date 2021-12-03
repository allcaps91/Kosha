using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using ComLibB;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcNewPatient : BaseForm
    {
        public delegate void SetGstrValue(HIC_PATIENT GstrValue);
        public event SetGstrValue rSetGstrValue;

        BasPatientService basPatientService = null;
        HicPatientService hicPatientService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicLtdService hicLtdService = null;

        HIC_PATIENT hPAT = null;
        HIC_LTD hLTD = null;
        clsHcFunc cHF = null;
        clsHaBase cHB = null;

        private string FstrJumin1 = string.Empty;
        private string FstrJumin2 = string.Empty;
        private string FstrJiCode = string.Empty;
        private int FnAge = 0;
        private string FstrSex      = "M";
        private string FstrGKiho    = "";
        private string FstrBuildNo  = "";
        private bool bBASPATIENT_INFO = true;       //외래 환자마스터 존재여부

        public frmHcNewPatient()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        public frmHcNewPatient(string argJumin1, string argJumin2)
        {
            InitializeComponent();
            SetControl();
            SetEvents();

            FstrJumin1 = argJumin1;
            FstrJumin2 = argJumin2;

            if (VB.Left(FstrJumin2, 1) == "1" || VB.Left(FstrJumin2, 1) == "3" || VB.Left(FstrJumin2, 1) == "5" || VB.Left(FstrJumin2, 1) == "7" || VB.Left(FstrJumin2, 1) == "9")
            {
                FstrSex = "M";
            }
            else if (VB.Left(FstrJumin2, 1) == "2" || VB.Left(FstrJumin2, 1) == "4" || VB.Left(FstrJumin2, 1) == "6" || VB.Left(FstrJumin2, 1) == "8" || VB.Left(FstrJumin2, 1) == "9")
            {
                FstrSex = "F";
            }
        }

        private void SetEvents()
        {
            this.Load               += new EventHandler(eFormLoad);

            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click   += new EventHandler(eBtnClick);
            this.btnHelp_Mail.Click += new EventHandler(eBtnClick);
            this.btnLtdJuso.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtJumin1.KeyDown  += new KeyEventHandler(eKeyDown);
            this.txtJumin2.KeyDown  += new KeyEventHandler(eKeyDown);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (sender == txtJumin2)
            {
                if (cHF.JuminNo_Check_New(txtJumin1.Text, txtJumin2.Text).Equals("ERROR"))
                {
                    MessageBox.Show("주민번호가 올바르지 않습니다.", "확인");
                    txtJumin2.Focus();
                    return;
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                if (!panMain.RequiredValidate())
                {
                    MessageBox.Show("필수 입력항목이 누락되었습니다.");
                    return;
                }

                if (ComFunc.LenH(txtJuso1.Text) > 70)
                {
                    MessageBox.Show("주소 길이를 확인해주세요.", "오류");
                    return;
                }


                //DataCheck Module
                if (!Check_Data_Input())
                {
                    return;
                }

                if (rSetGstrValue.IsNullOrEmpty())
                {
                    this.Close();
                }
                else
                {

                    txtLtdName.Text = VB.Pstr(txtLtdName.Text, ".", 1);
                    //생성된 정보를 PASS
                    HIC_PATIENT item = panMain.GetData<HIC_PATIENT>();

                    item.JUMIN = item.CT_JUMIN1 + VB.Left(item.CT_JUMIN2, 1) + "******";
                    item.JUMIN2 = clsAES.AES(item.CT_JUMIN1 + item.CT_JUMIN2);

                    if (!bBASPATIENT_INFO)  //외래환자 정보가 없다면
                    {
                        if (!BAS_PATIENT_INSERT(item))
                        {
                            MessageBox.Show("병원등록번호 생성 시 오류!", "ERROR");
                            return;
                        }
                    }

                    if (item.PTNO.IsNullOrEmpty())
                    {
                        MessageBox.Show("병원등록번호 생성 후 검진번호 연동 시 오류!", "병원번호 없음");
                        return;
                    }

                    item.SEX = FstrSex;
                    item.BUILDNO = FstrBuildNo;
                    item.GKIHO = FstrGKiho;
                    //item.P_AGE = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, DateTime.Now.ToShortDateString());
                    item.P_AGE = Convert.ToInt32(cHB.READ_HIC_AGE_GESAN2(txtJumin1.Text + txtJumin2.Text));
                    item.LTDCODE = VB.Pstr(txtLtdName.Text,".",1).To<long>(0);

                    if (!HIC_PATIENT_INSERT(item))
                    {
                        MessageBox.Show("검진번호 생성 시 오류!", "ERROR");
                        return;
                    }
                    
                    rSetGstrValue(item);
                    this.Close();
                    return;
                }
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnHelp_Mail)
            {
                Post_Code_Help();
            }
            else if ( sender == btnLtdJuso)
            {
                Ltd_Juso();
            }
        }

        private bool Check_Data_Input()
        {
            bool rtnVal = true;

            clsHcFunc cHF = new clsHcFunc();

            if (txtJumin1.Text.Trim().Length != 6 || txtJumin2.Text.Trim().Length != 7)
            {
                MessageBox.Show("주민등록번호를 확인하세요.", "오류");
                return false;
            }

            if (txtTel.Text.Trim().Length > 14)
            {
                MessageBox.Show("전화번호는 14자까지만 가능함.", "오류");
                return false;
            }

            if (txtHphone.Text.Trim().Length > 15)
            {
                MessageBox.Show("휴대폰번호는 15자까지만 가능함.", "오류");
                return false;
            }

            if (cHF.JuminNo_Check_New(txtJumin1.Text, txtJumin2.Text).Equals("ERROR"))
            {
                MessageBox.Show("주민번호가 올바르지 않습니다.", "확인");
                txtJumin2.Focus();
                cHF = null;
                return false;
            }

            cHF = null;

            return rtnVal;
        }

        private bool HIC_PATIENT_INSERT(HIC_PATIENT item)
        {
            bool rtnVal = true;

            long nPANO = hicPatientService.GetPanobyJumin(item.JUMIN2);

            if (nPANO > 0)
            {
                txtPano.Text = nPANO.To<string>();
                return rtnVal;
            }
            else
            {
                item.PANO = comHpcLibBService.Seq_GetHicPatientNo();

                int result = comHpcLibBService.InsertHicPatient(item);
                if (result <= 0)
                {
                    return false;
                }

                txtPano.Text = item.PANO.To<string>();
            }

            return rtnVal;
        }

        private bool BAS_PATIENT_INSERT(HIC_PATIENT item)
        {
            bool rtnVal = true;

            clsHaBase cHS = new clsHaBase();

            BAS_PATIENT Bitem = new BAS_PATIENT
            {
                PANO = cHS.PANO_LAST_CHAR(comHpcLibBService.Seq_GetBasPatientNo()),       //신규번호 생성필요
                SNAME = item.SNAME,                     SEX = FstrSex,
                JUMIN1 = item.CT_JUMIN1,                JUMIN2 = VB.Left(item.CT_JUMIN2, 1) + "******",
                STARTDATE = DateTime.Now.ToShortDateString(),               
                LASTDATE = DateTime.Now.ToShortDateString(),
                ZIPCODE1 = VB.Left(item.MAILCODE, 3),   ZIPCODE2 = VB.Mid(item.MAILCODE, 4, 2),
                JUSO = item.JUSO2,                      JICODE = FstrJiCode.IsNullOrEmpty() ? "01" : FstrJiCode,
                TEL = item.TEL,                         HPHONE = item.HPHONE,
                EMBPRT = "",        BI = "51",          PNAME = item.SNAME,
                GWANGE = "1",       KIHO = "",          GKIHO = FstrGKiho,
                DEPTCODE = "HR",    DRCODE = "7101",    GBSPC = "0",
                GBGAMEK = "00",     BOHUN = "",         REMARK = "",
                SABUN = "",         BUNUP = "",         BIRTH = null,
                GBBIRTH = "",                           EMAIL = item.EMAIL,
                GBINFOR = "",       GBJUSO = "",        GBSMS = "",
                HPHONE2 = "",
                JUMIN3 = clsAES.AES(item.CT_JUMIN2),
                BUILDNO = FstrBuildNo
            };

            if (Bitem.PANO.IsNullOrEmpty()) { return false; }   //병원번호 생성시 실패하면 false

            item.PTNO = Bitem.PANO;
            txtPano.Text = Bitem.PANO;

            int result = comHpcLibBService.InsertBasPatientByHicPatient(Bitem);
            if (result <= 0)
            {
                return false;
            }

            return rtnVal;
        }

        private void Post_Code_Help()
        {
            clsHcVariable.GstrValue = "";
            clsPublic.GstrMsgList = "";

            frmSearchRoadWeb frm = new frmSearchRoadWeb();
            frm.rSetGstrValue += new frmSearchRoadWeb.SetGstrValue(ePost_value);
            frm.ShowDialog();

            if (!clsHcVariable.GstrValue.IsNullOrEmpty())
            {
                txtMail.Text = VB.Left(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 3);
                txtMail.Text += VB.Mid(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 4, 2);
                txtJuso1.Text = VB.Pstr(clsHcVariable.GstrValue, "|", 2).Trim();
                txtJuso2.Text = "";

                FstrBuildNo = VB.Pstr(clsHcVariable.GstrValue, "|", 5).Trim();
                FstrJiCode = VB.Pstr(clsHcVariable.GstrValue, "|", 3).Trim();
                txtJuso2.Focus();
            }
            else
            {
                FstrBuildNo = "";
                FstrJiCode = "";
                txtJuso2.Focus();
            }
        }

        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!hLTD.IsNullOrEmpty() && hLTD.CODE > 0)
            {
                txtLtdName.Text = hLTD.CODE.To<string>();
                txtLtdName.Text += "." + hLTD.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            hLTD = item;
        }

        private void ePost_value(string GstrValue)
        {
            clsHcVariable.GstrValue = GstrValue;
        }

        private void Ltd_Juso()
        {
            string strLtdCode = VB.Pstr(txtLtdName.Text, ".", 1);

            HIC_LTD item = hicLtdService.GetAllbyCode(strLtdCode);
            if (!item.IsNullOrEmpty())
            {
                txtMail.Text = item.MAILCODE;
                txtJuso1.Text = item.JUSO;
                txtJuso2.Text = item.JUSODETAIL;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            panMain.Initialize();

            if (!FstrJumin1.IsNullOrEmpty()) { txtJumin1.Text = FstrJumin1; }
            if (!FstrJumin2.IsNullOrEmpty()) { txtJumin2.Text = FstrJumin2; }

            txtPtno.Text = "0";
            txtPano.Text = "0";
            txtLtdName.Text = "";

            //환자마스타가 없으면 외래의 환자마스타를 읽음
            IList<BAS_PATIENT> list = basPatientService.GetPatientByJuminNo(txtJumin1.Text, clsAES.AES(txtJumin2.Text));
            if (list.Count >= 2)
            {
                MessageBox.Show("외래 챠트번호가 2건 이상입니다.", "확인필요");
            }

            if (list.Count > 0)
            {
                txtPtno.Text    = list[0].PANO;
                txtSName.Text   = list[0].SNAME;
                txtMail.Text    = list[0].ZIPCODE3;
                txtJuso1.Text   = clsVbfunc.GetRoadJuSo(clsDB.DbCon, list[0].BUILDNO);
                txtJuso2.Text   = list[0].JUSO;
                FnAge           = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, DateTime.Now.ToShortDateString());
                FstrSex         = list[0].SEX;
                if (!list[0].GKIHO.IsNullOrEmpty())
                {
                    FstrGKiho = list[0].GKIHO.Replace("-", "");
                }
                FstrBuildNo     = list[0].BUILDNO;
            }
            else
            {
                bBASPATIENT_INFO = false;   //외래환자 마스터 없음
            }
        }

        private void SetControl()
        {
            hPAT    = new HIC_PATIENT();
            cHF     = new clsHcFunc();
            hLTD    = new HIC_LTD();
            cHB     = new clsHaBase();

            basPatientService = new BasPatientService();
            hicPatientService = new HicPatientService();
            comHpcLibBService = new ComHpcLibBService();
            hicLtdService = new HicLtdService();

            panMain.AddRequiredControl(txtJumin1);
            panMain.AddRequiredControl(txtJumin2);
            panMain.AddRequiredControl(txtSName);
            panMain.AddRequiredControl(txtMail);
            panMain.AddRequiredControl(txtHphone);

            panMain.SetEnterKey();
        }
    }
}
