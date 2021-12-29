using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using HC.Core.Common.Interface;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Service;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
{
    public partial class FrmSahusogen : Form
    {
        public FrmSahusogen()
        {
            InitializeComponent();

            Set_Screen();
        }

        private void Set_Screen()
        {
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            txtName.Text = "";
            txtName.Text = "";

            if (CboYear.Items.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    CboYear.Items.Add(nYear.ToString());
                    nYear--;
                }
            }
            CboYear.SelectedIndex = 0;

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("전체");
            cboPanjeng.Items.Add("A");
            cboPanjeng.Items.Add("U");
            cboPanjeng.Items.Add("AAA");
            cboPanjeng.Items.Add("U");
            cboPanjeng.Items.Add("C");
            cboPanjeng.Items.Add("C1");
            cboPanjeng.Items.Add("C2");
            cboPanjeng.Items.Add("D");
            cboPanjeng.Items.Add("D1");
            cboPanjeng.Items.Add("D2");
            cboPanjeng.Items.Add("CN");
            cboPanjeng.Items.Add("DN");
            cboPanjeng.Items.Add("확진검사대상");
            cboPanjeng.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            string strLtdcode = "";
            string strYear = "";
            string strPanjeng = "";
            string strName = "";

            strLtdcode = VB.Pstr(TxtLtdcode.Text.Trim(), ".", 1);
            strYear = CboYear.Text;
            strPanjeng = cboPanjeng.Text;
            strName = txtName.Text;
            if (strPanjeng == "전체") strPanjeng = "";

            if (strLtdcode == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }

            SSHealthCheck_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT GONGJENG,NAME,SEX,AGE,GUNSOK,YUHE,GGUBUN,SOGEN,SAHU,";
                SQL = SQL + ComNum.VBLF + " UPMU,YEAR,JINDATE,JIPYO  ";
                SQL = SQL + ComNum.VBLF + "  FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITEID=" + strLtdcode + " ";
                SQL = SQL + ComNum.VBLF + "   AND YEAR='" + strYear + "' ";
                if (strName != "") SQL = SQL + ComNum.VBLF + "   AND NAME LIKE '%" + strName + "%' ";
                if (strPanjeng != "")
                {
                    if (strPanjeng == "A") SQL = SQL + ComNum.VBLF + "   AND GGUBUN='A' ";
                    if (strPanjeng == "C") SQL = SQL + ComNum.VBLF + "   AND GGUBUN='C' ";
                    if (strPanjeng!="A" && strPanjeng != "C") SQL = SQL + ComNum.VBLF + "   AND GGUBUN LIKE '%" + strPanjeng + "%' ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY NAME,JINDATE,JONG ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SSHealthCheck_Sheet1.RowCount = dt.Rows.Count;
                    SSHealthCheck_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SSHealthCheck_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GONGJENG"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GUNSOK"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 5].Text = dt.Rows[i]["YUHE"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JIPYO"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GGUBUN"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SOGEN"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SAHU"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 10].Text = dt.Rows[i]["UPMU"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 11].Text = dt.Rows[i]["YEAR"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 12].Text = VB.Right(dt.Rows[i]["JINDATE"].ToString().Trim(),5);
                        SSHealthCheck_Sheet1.Rows[i].Height = SSHealthCheck_Sheet1.Rows[i].GetPreferredHeight();
                    }
                }
                else
                {
                    ComFunc.MsgBox("자료가 없습니다.");
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

        private void BtnSearchSite_Click(object sender, EventArgs e)
        {
            SiteListForm form = new SiteListForm();

            TxtLtdcode.Text = "";
            HC_SITE_VIEW siteView = form.Search(TxtLtdcode.Text);
            if (siteView == null)
            {
                DialogResult result = form.ShowDialog();
                siteView = form.SelectedSite;
            }
            else
            {
                form.Close();
            }

            if (siteView != null)
            {
                TxtLtdcode.Text = siteView.ID.ToString() + "." + siteView.NAME;
            }
        }

        private void SSHealthCheck_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }
    }
}
