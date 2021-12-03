using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class FrmCalculator : Form
    {
        string strGubun;
        bool mbolMouse = false;

        string SQL;
        DataTable dt = null;
        string SqlErr = ""; //에러문 받는 변수

        public FrmCalculator()
        {
            InitializeComponent();
        }

        public FrmCalculator(string sGubun)
        {
            InitializeComponent();

            strGubun = sGubun;
        }

        public FrmCalculator(string sGubun,bool bolMouse)
        {
            InitializeComponent();

            strGubun = sGubun;
            mbolMouse = bolMouse;
        }

        private void FrmCalculator_Load(object sender, EventArgs e)
        {
            clsOrdFunction.GnCalReturn = 0;
            clsOrdFunction.GstrCalReturn = "";

            fn_Set_Button();

            fn_Setting_Read(clsType.User.Sabun.Trim());

            if (strGubun == "NAL")
            {
                if (clsOrdFunction.GEnvSet_Item03 == "1")
                {
                    chkAll.Checked = true;
                }
                else if (clsOrdFunction.GEnvSet_Item03 == "2")
                {
                    chkAll.Checked = false;
                }
                else
                {
                    chkAll.Checked = false;
                }
            }

            if (strGubun == "QTY" || strGubun == "Contents")
            {
                if (clsOrdFunction.GEnvSet_Item15 == "1")
                {
                    chkAll.Checked = true;
                }
                else if (clsOrdFunction.GEnvSet_Item15 == "2")
                {
                    chkAll.Checked = false;
                }
                else
                {
                    chkAll.Checked = false;
                }
            }

            //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            if (mbolMouse == true)
            {
                for (int i = 0; i < Screen.AllScreens.Length; i++)
                {
                    if (Screen.AllScreens[i].Bounds.X <= Cursor.Position.X
                    && Screen.AllScreens[i].Bounds.X + Screen.AllScreens[i].Bounds.Width > Cursor.Position.X)
                    {
                        if (Screen.AllScreens[i].Bounds.Height < Cursor.Position.Y + this.Height)
                        {
                            this.Location = new Point(Cursor.Position.X, Screen.AllScreens[i].Bounds.Height - this.Height);
                        }
                        else
                        {
                            this.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                        }
                        break;
                    }
                }
                this.StartPosition = FormStartPosition.Manual;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            lblValue.Text = "";
        }

        void fn_Set_Button()
        {
            if(clsOrdFunction.GstrGbJob == "IPD")
            {
                if (strGubun == "NAL")
                {
                    btnPoint.Enabled = false;
                    btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }
                else if (strGubun == "Contents") 
                {
                    btnPoint.Enabled = false;
                    //btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }

                if (clsOrdFunction.GstrMachChk == "OK")
                {
                    btnPoint.Enabled = false;
                    btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }
            }
            else if (clsOrdFunction.GstrGbJob == "ER")
            {
                if (strGubun == "NAL")
                {
                    btnPoint.Enabled = false;
                    btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }
                else if (strGubun == "Contents")
                {
                    //btnPoint.Enabled = false;
                    btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }
            }
            else //OPD
            {
                if (strGubun == "NAL")
                {
                    btnPoint.Enabled = false;
                    btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }
                else if (strGubun == "Contents")
                {
                    btnPoint.Enabled = false;
                    //btnDiv.Enabled = false;
                    btn12.Enabled = false;
                    btn13.Enabled = false;
                    btn15.Enabled = false;
                    btn23.Enabled = false;
                    btn25.Enabled = false;
                    btn23.Enabled = false;
                    btn35.Enabled = false;
                    btn34.Enabled = false;
                    btn14.Enabled = false;
                    btn45.Enabled = false;
                    btn16.Enabled = false;
                    btn17.Enabled = false;
                    btn18.Enabled = false;
                    btn19.Enabled = false;
                }
            }
        }

        void fn_Setting_Read(string sUserId)
        {
            try
            {
                SQL = "";
                SQL += " SELECT ITEM03, ITEM15                  \r";
                SQL += "   FROM ADMIN.OCS_ENVSETTING       \r";
                SQL += "  WHERE USERID = '" + sUserId + "'      \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsOrdFunction.GEnvSet_Item03 = dt.Rows[0]["ITEM03"].ToString().Trim();
                    clsOrdFunction.GEnvSet_Item15 = dt.Rows[0]["ITEM15"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void Calculate_Input(System.Windows.Forms.Button BtnName)
        {
            if (VB.IsNumeric(lblValue.Text) == false && lblValue.Text.IndexOf("..") >= 0)
            {
                lblValue.Text = "";
            }

            lblValue.Text = lblValue.Text.Trim() + BtnName.Text;

            if (strGubun == "NAL")
            {
                if (VB.Val(lblValue.Text) > 999) lblValue.Text = "999";
            }
            lblValue.Tag = lblValue.Text;
        }

        private void Cal_Div_Input(System.Windows.Forms.Button BtnName)
        {
            lblValue.Text = BtnName.Text.Trim();
            lblValue.Tag = BtnName.Tag.ToString();

            //btnOK_Click(btnOK, null);
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn0);
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn1);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn2);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn3);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn4);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn5);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn6);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn7);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn8);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            Calculate_Input(btn9);
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            Calculate_Input(btnPoint);
        }

        private void btn34_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn34);
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {

        }

        private void btn16_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn16);
        }

        private void btn17_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn17);
        }

        private void btn12_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn12);
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn13);
        }

        private void btn23_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn23);
        }

        private void btn14_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn14);
        }

        private void btn18_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn18);
        }

        private void btn15_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn15);
        }

        private void btn25_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn25);
        }

        private void btn35_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn35);
        }

        private void btn45_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn45);
        }

        private void btn19_Click(object sender, EventArgs e)
        {
            Cal_Div_Input(btn19);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lblValue.Text.Trim() == "" || lblValue.Text.Trim() == null) return;
            lblValue.Text = lblValue.Text.Replace("..", "");
            lblValue.Tag = lblValue.Tag.ToString().Replace("..", "");
            if (lblValue.Text.Trim().Substring(lblValue.Text.Trim().Length - 1, 1) == ".")
            {
                lblValue.Text = lblValue.Text.Trim().Substring(0, lblValue.Text.Trim().Length - 1);
            }

            if (VB.IsNumeric(lblValue.Text) == false)
            {
                clsOrdFunction.GnCalReturn = 0;
                clsOrdFunction.GstrCalReturn = "0";
            }

            if (strGubun == "NAL")
            {
                //clsOrdFunction.GnCalReturn = string.Format("{0:N0}", float.Parse(lblValue.Tag.ToString()));
                clsOrdFunction.GnCalReturn = float.Parse(lblValue.Tag.ToString());
                clsOrdFunction.GstrCalReturn = lblValue.Text.Trim();

                if (float.Parse(lblValue.Tag.ToString()) < 1)
                {
                    clsOrdFunction.GnCalReturn = 1;
                    clsOrdFunction.GstrCalReturn = "1";
                }
            }
            else
            {
                //clsOrdFunction.GnCalReturn = string.Format("{0:N2}", float.Parse(lblValue.Tag.ToString()));
                clsOrdFunction.GnCalReturn = float.Parse(lblValue.Tag.ToString());
                clsOrdFunction.GstrCalReturn = lblValue.Text.Trim();
            }

            if (chkAll.Checked == true)
            {
                clsOrdFunction.GnCalLabel = 1;
            }
            else
            {
                clsOrdFunction.GnCalLabel = 0;
            }

            this.Close();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            lblValue.Text = "";
            lblValue.Tag = "";
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            if (lblValue.Text.ToString().Length < 2) lblValue.Text = "";

            if (VB.IsNumeric(lblValue.Text.Trim()) == false) lblValue.Text = "";

            if (lblValue.Text.Trim() == "")
            {
                lblValue.Tag = 0;
                return;
            }
            lblValue.Text = lblValue.Text.Trim().Substring(0, lblValue.Text.Length - 1);
            lblValue.Tag = VB.Val(lblValue.Text);
        }
    }
}