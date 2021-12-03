using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스



/// <summary>
/// Description : Client PC 설정
/// Author : 박병규
/// Create Date : 2017.05.22
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public partial class frmSetPmpaPC : Form
    {
        public int  GintSelTab;

        ComFunc CF = null;
        clsPmpaFunc clsPF = null;

        public frmSetPmpaPC()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);
            this.txtX.GotFocus += new EventHandler(txtX_GotFocus);
            this.txtY.GotFocus += new EventHandler(txtY_GotFocus);
            this.txtPad.GotFocus += new EventHandler(txtPad_GotFocus);
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CF = new ComFunc();
            clsPF = new clsPmpaFunc();

            DataTable Dt = new DataTable();
            string strCode = "";
            string strCodeName = "";

            if (clsAuto.GstrAREquipUse != "OK")
            {
                if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            }
            frmSetPmpaPC frm = new frmSetPmpaPC();
            ComFunc.Form_Center(frm);

            cboBand.Items.Clear();
            cboBand.SelectedIndex = -1;

            Dt = ComQuery.Set_BaseCode_Foundation(clsDB.DbCon, "BAS_인식밴드출력위치", "");

            cboBand.Items.Add("선택");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strCode = Dt.Rows[i]["CODE"].ToString().Trim();
                strCodeName = Dt.Rows[i]["NAME"].ToString().Trim();

                cboBand.Items.Add(strCode + "." + strCodeName);
            }
            Dt.Dispose();
            Dt = null;


            cboLocation.Items.Clear();
            cboLocation.SelectedIndex = -1;

            Dt = ComQuery.Set_BaseCode_Foundation(clsDB.DbCon, "BAS_프로그램위치", "");

            cboLocation.Items.Add("선택");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                strCode = Dt.Rows[i]["CODE"].ToString().Trim();
                strCodeName = Dt.Rows[i]["NAME"].ToString().Trim();

                cboLocation.Items.Add(strCode + "." + strCodeName);
            }
            Dt.Dispose();
            Dt = null;


            txtX.Text = "";
            txtY.Text = "";
            txtPad.Text = "";

            optCard0.Checked = true;

            Get_Data();
        }

        private void txtX_GotFocus(object sender, EventArgs e)
        {
            txtX.SelectAll();
        }

        private void txtY_GotFocus(object sender, EventArgs e)
        {
            txtY.SelectAll();
        }

        private void txtPad_GotFocus(object sender, EventArgs e)
        {
            txtPad.SelectAll();
        }

        private void Get_Data()
        {
            String strData = "";

            clsPmpaPb.GnPrintXSet = 0;
            clsPmpaPb.GnPrintYSet = 0;
            clsPmpaPb.GstrCreditBand = "0";

            strData = CF.Reg_Get_Setting("BASIC", "BAND");

            for (int i =0; i < cboBand.Items.Count; i++)
            {
                cboBand.SelectedIndex = i;

                if ( cboBand.Text.Trim() == strData)
                    break;
                else
                    cboBand.SelectedIndex = 0;
            }

            strData = CF.Reg_Get_Setting("BASIC", "OUMSAD");

            for (int i = 0; i < cboLocation.Items.Count; i++)
            {
                cboLocation.SelectedIndex = i;

                if (cboLocation.Text.Trim() == strData)
                    break;
                else
                    cboLocation.SelectedIndex = 0;
            }

            txtX.Text = CF.Reg_Get_Setting("PRINT", "PRINTX");
            clsPmpaPb.GnPrintXSet = Convert.ToInt16(VB.Val(txtX.Text));
            txtY.Text = CF.Reg_Get_Setting("PRINT", "PRINTY");
            clsPmpaPb.GnPrintYSet = Convert.ToInt16(VB.Val(txtY.Text));

            txtPad.Text = CF.Reg_Get_Setting("CREDIT", "CONNECT");
            clsPmpaPb.GstrCreditIF = txtPad.Text.Trim();

            strData = CF.Reg_Get_Setting("CREDIT", "COMPANY");
            clsPmpaPb.GstrCreditBand = strData;

            if (strData == "0")
                optCard0.Checked = true;
            else
                optCard0.Checked = false;
        }
        
        private void txtPad_TextChanged(object sender, EventArgs e)
        {
            if (txtPad.Text != "")
                txtPad.Text = txtPad.Text.ToUpper();
        }

        private void txtX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) SendKeys.Send("{TAB}");
        }
        
        private void btnOk_Click(object sender, EventArgs e)

        {
            string strData = "";

            if (optCard0.Checked == true)
                strData = "0";

            CF.Reg_Save_Setting("BASIC", "BAND", cboBand.Text);                      //환자인식밴드위치
            CF.Reg_Save_Setting("BASIC", "OUMSAD", cboLocation.Text);                //접수/수납프로그램 위치
            CF.Reg_Save_Setting("PRINT", "PRINTX", txtX.Text);                       //X Margin
            CF.Reg_Save_Setting("PRINT", "PRINTY", txtY.Text);                       //Y Margin
            CF.Reg_Save_Setting("CREDIT", "CONNECT", txtPad.Text.ToUpper().Trim());  //서명방법
            CF.Reg_Save_Setting("CREDIT", "COMPANY", strData);                       //카드사

            clsPmpaPb.GnPrintXSet = Convert.ToInt16(VB.Val(txtX.Text));
            clsPmpaPb.GnPrintYSet = Convert.ToInt16(VB.Val(txtY.Text));
            clsPmpaPb.GstrCreditIF = txtPad.Text.ToUpper().Trim();
            clsPmpaPb.GstrCreditBand = strData;

            ComFunc.MsgBox("설정이 완료되었습니다.", "완료");

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPad_Leave(object sender, EventArgs e)
        {
            if (txtPad.Text != "")
            {
                txtPad.Text = txtPad.Text.ToUpper();
            }
        }

        private void txtY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) SendKeys.Send("{TAB}");
        }

    }
}
