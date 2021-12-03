using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewBIPatient.cs
    /// Description     : 보호 환자 조회
    /// Author          : 안정수
    /// Create Date     : 2017-10-17
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oiguide\oiguide06.frm(FrmBIPatient) => frmPmpaViewBIPatient.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\oiguide06.frm(FrmBIPatient)
    /// </seealso>
    /// </summary>

    public partial class frmPmpaViewBIPatient : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        string StrScreenFlag = "";
        int nNu = 0;
        string[] strBis = new string[100];
        int nSelect = 0;
        string[] strshow = new string[2001];

        public frmPmpaViewBIPatient()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

            this.txtSname.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumin1.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumin2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txtSname.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtJumin1.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtJumin2.LostFocus += new EventHandler(eControl_LostFocus);

            this.txtSname.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtJumin1.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtJumin2.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            tabControl1.TabIndex = 0;

            btnCancel.Visible = false;
            btnCancel2.Visible = false;


        }

        void tabControl1_Click(object sender, EventArgs e)
        {
            CS.Spread_All_Clear(ssOpd);

            txtSname.Text = "";
            txtJumin1.Text = "";
            txtJumin2.Text = "";

            if (tabItem1.IsSelected == true)
            {
                txtSname.Focus();
            }
            else if (tabItem2.IsSelected == true)
            {
                txtJumin1.Focus();
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if (sender == this.txtSname)
            {
                txtSname.ImeMode = ImeMode.Hangul;
            }

            else if (sender == this.txtJumin2)
            {
                txtJumin1.ImeMode = ImeMode.Alpha;

                txtJumin2.SelectionStart = 0;
                txtJumin2.SelectionLength = txtJumin2.Text.Length;
            }

            else if (sender == this.txtJumin1)
            {
                txtJumin1.ImeMode = ImeMode.Hangul;

                txtJumin1.SelectionStart = 0;
                txtJumin1.SelectionLength = txtJumin1.Text.Length;
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtSname)
            {
                txtSname.ImeMode = ImeMode.Hangul;
            }

            else if (sender == this.txtJumin2)
            {
                txtJumin1.ImeMode = ImeMode.Alpha;
            }
            else if (sender == this.txtJumin1)
            {
                txtJumin1.ImeMode = ImeMode.Hangul;
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtSname)
            {
                string strSname = "";
                int i = 0;
                string strFDate = "";

                strSname = txtSname.Text.Trim();

                if (e.KeyChar == 13)
                {
                    if (ComFunc.LenH(txtSname.Text) >= 4)
                    {
                        Bi_Select_Patient();
                    }
                    txtSname.SelectionStart = 0;
                    txtSname.SelectionLength = txtSname.Text.Length;
                }
            }

            else if (sender == this.txtJumin2)
            {
                if (e.KeyChar == 13)
                {
                    if (txtJumin2.Text.Length > 1)
                    {
                        Bi_Select_Patient();
                    }
                    txtJumin2.SelectionStart = 0;
                    txtJumin2.SelectionLength = txtJumin1.Text.Length;
                }
            }

            else if (sender == this.txtJumin1)
            {
                if (e.KeyChar == 13)
                {
                    if (txtJumin1.Text.Length >= 6)
                    {
                        //Call ChartNo_Display
                    }
                    txtJumin1.SelectionStart = 0;
                    txtJumin1.SelectionLength = txtJumin1.Text.Length;

                    txtJumin2.Focus();
                }
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                Bi_Select_Patient();
            }

            else if (sender == this.btnCancel)
            {
                if (tabItem1.IsSelected == true)
                {
                    ComFunc.SetAllControlClear(panel1);
                }

                else
                {
                    ComFunc.SetAllControlClear(panel5);
                }
            }
        }

        void Bi_Select_Patient()
        {
            int i = 0;
            string strFDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  A.PANO, A.SNAME, A.SEX, A.JUMIN1, A.JUMIN2, A.PNAME,";
            SQL += ComNum.VBLF + "  B.DEPTCODE, TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_SHEET B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (tabItem1.IsSelected == true)
            {
                SQL += ComNum.VBLF + "  AND SNAME LIKE '" + txtSname.Text + "' ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND JUMIN1 = '" + txtJumin1.Text + "'";
                SQL += ComNum.VBLF + "  AND JUMIN2 = '" + txtJumin2.Text + "'";
            }
            SQL += ComNum.VBLF + "      AND BI IN ('21','22','23')";
            SQL += ComNum.VBLF + "      AND B.GUBUN = '2'";
            SQL += ComNum.VBLF + "      AND B.PANO = A.PANO(+)";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssOpd_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssOpd_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                    if (dt.Rows[i]["SEX"].ToString().Trim() == "M")
                    {
                        ssOpd_Sheet1.Cells[i, 2].Text = "남";
                    }
                    else
                    {
                        ssOpd_Sheet1.Cells[i, 2].Text = "여";
                    }
                    ssOpd_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + dt.Rows[i]["JUMIN2"].ToString().Trim();
                    ssOpd_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PNAME"].ToString().Trim();
                    ssOpd_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    strFDate = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    ssOpd_Sheet1.Cells[i, 6].Text = strFDate;
                    ssOpd_Sheet1.Cells[i, 7].Text = (Convert.ToInt32(VB.Left(strFDate, 4)) + 1) + VB.Right(strFDate, 6);

                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }


    }
}
