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
    public partial class FrmExcelUpload1 : Form
    {
        public FrmExcelUpload1()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int nYear = 0;
            string strTitle = "";
            nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYear.ToString());
                nYear--;
            }
            cboYear.SelectedIndex = 0;

            cboBangi.Items.Add("전체");
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");
            cboBangi.SelectedIndex = 0;

            btnJob1.Enabled = true;
            btnJob2.Enabled = false;
            btnJob3.Enabled = false;
            btnJob4.Enabled = false;

            SS1_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            for (int i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                strTitle = SS1_Sheet1.ColumnHeader.Cells[0, i].Value.ToString();
                SSConv_Sheet1.Cells[i, 0].Text = strTitle;
                SSConv_Sheet1.Cells[i, 1].Value = (i + 1);
            }
        }

        private void btnJob1_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            int nBlankLine = 0;
            int nRowCount = 0;
            bool bBlank = false;

            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                SSExcel.ActiveSheet.RowCount = 0;
                SSExcel.ActiveSheet.OpenExcel(strFileName, 0);
                btnJob4.Enabled = true;

                // 공란만 있는 Row는 제거
                for (int i = 0; i < SSExcel_Sheet1.RowCount; i++)
                {
                    bBlank = true;
                    for(int j=0;j<SSExcel_Sheet1.ColumnCount; j++)
                    {
                        if (SSExcel_Sheet1.Cells[i, j].Text!="")
                        {
                            bBlank = false;
                            nRowCount = i;
                            break;
                        }
                    }
                    if (bBlank == true)
                    {
                        nBlankLine++;
                        if (nBlankLine>5)
                        {
                            SSExcel_Sheet1.RowCount = nRowCount+1;
                            break;
                        }
                    }
                }
            }
        }

        private void btnJob4_Click(object sender, EventArgs e)
        {
            string strData = "";
            int nCol = 0;

            for (int i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                SSConv_Sheet1.Cells[i, 2].Value = "";
            }

            for (int i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                nCol = Int32.Parse(SSConv_Sheet1.Cells[i, 1].Value.ToString());
                if (nCol > 0)
                {
                    strData = SSExcel_Sheet1.Cells[1, nCol - 1].Text.ToString();
                    SSConv_Sheet1.Cells[i, 2].Text = strData;
                }
            }
            btnJob2.Enabled = true;
        }

        private void FrmExcelUpload1_Load(object sender, EventArgs e)
        {

        }

        private void BtnSearchSite_Click(object sender, EventArgs e)
        {
            SiteListForm form = new SiteListForm();

            HC_SITE_VIEW siteView = form.Search(TxtSiteIdOrName.Text);
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
                TxtSiteIdOrName.Text = siteView.ID.ToString() + "." + siteView.NAME;
            }
        }

        // 표준서식으로 변환
        private void btnJob2_Click(object sender, EventArgs e)
        {
            string strData = "";
            int nCol = 0;
            int nRow = 0;
            int nBlankLines = 0;
            bool isBlankLine = false;

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = SSExcel_Sheet1.RowCount;

            for (int i = 1; i < SSExcel_Sheet1.RowCount;i++)
            {
                isBlankLine = true;
                for (int j = 0; j < SSConv_Sheet1.RowCount; j++)
                {
                    if (SSExcel_Sheet1.Cells[i,j].Text.ToString() != "")
                    {
                        isBlankLine = false;
                        break;
                    }
                }

                // 공란줄은 제외
                if (isBlankLine == false)
                {
                    for (int j= 0; j < SSConv_Sheet1.RowCount; j++)
                    {
                        nCol = Int32.Parse(SSConv_Sheet1.Cells[j, 1].Value.ToString());
                        if (nCol > 0)
                        {
                            strData = SSExcel_Sheet1.Cells[i, nCol - 1].Text.ToString();
                            SS1_Sheet1.Cells[nRow, j].Text = strData;
                        }
                    }
                    nRow++;
                }
            }
            SS1_Sheet1.RowCount = nRow;
            btnJob3.Enabled = true;
        }
    }
}
