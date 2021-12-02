using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmPrintPreView : Form
    {
        FarPoint.Win.Spread.PrintInfo Spd;
        string GstrPageCnt = "";
        string GstrPrintCnt = "";
        string GstrPrint_YN = "";

        public frmPrintPreView()
        {
            InitializeComponent();
        }

        void frmPrintPreView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            lbl.Text = "100";
            txtPage.Text = "1/" + GstrPageCnt;
            txtFPage.Text = "1";
            txtTPage.Text = GstrPageCnt;
            txtCnt.Text = GstrPrintCnt;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            GstrPrint_YN = "NO";
            this.Close();
        }

        void btnBig_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt16(lbl.Text) >= 300)
            {
                return;
            }
            //ssSpreadPreview.PageViewType = 2
            lbl.Text = Convert.ToString(Convert.ToInt16(lbl.Text) + 10);

            if(Convert.ToInt16(lbl.Text) > 300)
            {
                lbl.Text = "300";
            }
            //ssSpreadPreview.PageViewPercentage = Val(txtRate.Text)
        }

        void btnSmall_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(lbl.Text) <= 50)
            {
                return;
            }
            //ssSpreadPreview.PageViewType = 2
            lbl.Text = Convert.ToString(Convert.ToInt16(lbl.Text) - 10);

            if (Convert.ToInt16(lbl.Text) < 50)
            {
                lbl.Text = "50";
            }
            //ssSpreadPreview.PageViewPercentage = Val(txtRate.Text)
        }

        void btnNextPage_Click(object sender, EventArgs e)
        {
            //ssSpreadPreview.PageCurrent = ssSpreadPreview.PageCurrent + ssSpreadPreview.PagesPerScreen
        }

        void btnPrePage_Click(object sender, EventArgs e)
        {
            //ssSpreadPreview.PageCurrent = ssSpreadPreview.PageCurrent - ssSpreadPreview.PagesPerScreen
        }

        void btnSetting_Click(object sender, EventArgs e)
        {
            //comPrint.Action = 5
        }

        void txtCnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnPrint.Focus();
            }
        }

        void txtCnt_Leave(object sender, EventArgs e)
        {
            if (!VB.IsNumeric(txtCnt.Text))
            {
                txtCnt.Text = "1";
            }
            if(txtCnt.Text != "1")
            {
                txtCnt.Text = "1";
            }
        }

        void txtFPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txtTPage.Focus();
            }
        }

        void txtFPage_Leave(object sender, EventArgs e)
        {
            if (!VB.IsNumeric(txtFPage.Text))
            {
                txtFPage.Text = "1";
            }
            if (Convert.ToInt16(txtFPage.Text) < 1)
            {
                txtFPage.Text = "1";
            }
            if(String.Compare(txtFPage.Text, txtTPage.Text) > 0)
            {
                txtFPage.Text = txtTPage.Text;
            }
        }

        void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(Convert.ToInt16(txtRate.Text) < 50)
                {
                    txtRate.Text = "50";
                }

                //ssSpreadPreview.PageViewType = 2
                //ssSpreadPreview.PageViewPercentage = Val(txtRate.Text)

            }
        }

        void txtTPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txtCnt.Focus();
            }
        }

        void txtTPage_Leave(object sender, EventArgs e)
        {
            if (!VB.IsNumeric(txtTPage.Text))
            {
                //TxtTpage.Text = GSSPrintPreView.PrintPageCount
            }

            // If Val(TxtTpage.Text) > GSSPrintPreView.PrintPageCount
            // TxtTpage.Text = GSSPrintPreView.PrintPageCount
            if (String.Compare(txtTPage.Text, txtFPage.Text) < 0)
            {
                txtTPage.Text = txtFPage.Text;
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            int i = 0;

            //With GSSPrintPreView
            //.PrintPageStart = Val(TxtFpage.Text)
            //.PrintPageEnd = Val(TxtTpage.Text)
            //.PrintType = PrintTypePageRange

            for(i = 0; i < Convert.ToInt16(txtCnt.Text); i++)
            {
                //If.PrintSmartPrint = True Then
                //'.PrintOrientation = 2
                //.Action = SS_ACTION_SMARTPRINT
                //Else
                //    .PrintSheet
                //End If
            }
            //.PrintPageStart = 1
            //.PrintPageEnd = 9999
            // End With

            GstrPrint_YN = "Y";
            // Set GSSPrintPreView = Nothing
            txtCnt.Text = "1";
            this.Close();

        }
    }
}
