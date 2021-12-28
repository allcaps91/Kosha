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
using HC.Core.Repository;
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
        private HcSiteWorkerService hcSiteWorkerService;
        private HcSiteWorkerRepository hcSiteWorkerRepository;
        private int[] FnCol = new int[1000];

        public FrmExcelUpload1()
        {
            InitializeComponent();
            hcSiteWorkerService = new HcSiteWorkerService();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            string strTitle = "";

            TxtLtdcode.Text = "";

            for (i = 0; i < 1000; i++)
            {
                FnCol[i] = 0;
            }

            btnJob1.Enabled = true;
            btnJob2.Enabled = false;
            btnJob3.Enabled = false;
            btnJob4.Enabled = false;

            SS1_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = 0;
            SSConv_Sheet1.RowCount = SS1_Sheet1.ColumnCount;

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
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

            if (TxtLtdcode.Text.Trim() == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }

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
                    for (int j = 0; j < SSExcel_Sheet1.ColumnCount; j++)
                    {
                        if (SSExcel_Sheet1.Cells[i, j].Text != "")
                        {
                            bBlank = false;
                            nRowCount = i;
                            break;
                        }
                    }
                    if (bBlank == true)
                    {
                        nBlankLine++;
                        if (nBlankLine > 5)
                        {
                            SSExcel_Sheet1.RowCount = nRowCount + 1;
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
                if (SSConv_Sheet1.Cells[i, 1].Text.Trim() != "")
                {
                    nCol = Int32.Parse(SSConv_Sheet1.Cells[i, 1].Value.ToString());
                    if (nCol > 0)
                    {
                        strData = SSExcel_Sheet1.Cells[1, nCol - 1].Text.ToString();
                        SSConv_Sheet1.Cells[i, 2].Text = strData;
                    }

                }
            }
            btnJob2.Enabled = true;
        }

        private void btnJob2_Click(object sender, EventArgs e)
        {
            int i;
            int j;
            string strData = "";
            int nRow = 0;
            bool isBlankLine = false;
            bool bMultyLine = false;
            string strNewData = "";

            //변경값을 변수에 저장
            for (i = 0; i < SSConv_Sheet1.RowCount; i++)
            {
                FnCol[i] = 0;
                if (SSConv_Sheet1.Cells[i, 1].Text.Trim() != "")
                {
                    FnCol[i] = Int32.Parse(SSConv_Sheet1.Cells[i, 1].Value.ToString());
                }
            }

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = SSExcel_Sheet1.RowCount;

            for (i = 1; i < SSExcel_Sheet1.RowCount; i++)
            {
                isBlankLine = true;
                for (j = 0; j < SSConv_Sheet1.RowCount; j++)
                {
                    if (SSExcel_Sheet1.Cells[i, j].Text.ToString() != "")
                    {
                        isBlankLine = false;
                        break;
                    }
                }

                // 공란줄은 제외
                if (isBlankLine == false)
                {
                    // 엑셀파일의 처음 3칼럼 모두 공란이면 연속 Data로 처리
                    bMultyLine = true;
                    if (SSExcel_Sheet1.Cells[i, 0].Text.Trim() != "") bMultyLine = false;
                    if (SSExcel_Sheet1.Cells[i, 1].Text.Trim() != "") bMultyLine = false;

                    for (j = 0; j < SSConv_Sheet1.RowCount; j++)
                    {
                        if (FnCol[j] > 0)
                        {
                            strData = SSExcel_Sheet1.Cells[i, FnCol[j] - 1].Text.ToString();
                            if (strData != "")
                            {
                                if (bMultyLine == true)
                                {
                                    strNewData = SS1_Sheet1.Cells[nRow - 1, j].Text.ToString();
                                    strNewData = strNewData + ComNum.VBLF + strData;
                                    SS1_Sheet1.Cells[nRow - 1, j].Text = strNewData;
                                }
                                else
                                {
                                    SS1_Sheet1.Cells[nRow, j].Text = strData;
                                }
                            }
                        }
                    }
                }
                if (bMultyLine == false) nRow++;
            }
            SS1_Sheet1.RowCount = nRow;

            // 셀의 높이를 조정
            for (i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                SS1_Sheet1.Rows[i].Height = SS1_Sheet1.Rows[i].GetPreferredHeight();
            }

            btnJob3.Enabled = true;
        }

        private void btnJob3_Click(object sender, EventArgs e)
        {
            string strID = "";
            long nLtdCode = 0;
            string strName = "";
            string strBirth = "";
            string strBuse = "";
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            nLtdCode = long.Parse(VB.Pstr(TxtLtdcode.Text, ".", 1));
            HC_SITE_WORKER worker = new HC_SITE_WORKER();

            for (int i = 0; i < SS1_Sheet1.RowCount; i++)
            {
                strName = SS1_Sheet1.Cells[i, 0].Text.ToString();
                strBirth = SS1_Sheet1.Cells[i, 1].Text.ToString();
                if (VB.Len(strBirth) > 6) strBirth = VB.Left(strBirth, 6);
                strBuse = SS1_Sheet1.Cells[i, 2].Text.ToString();

                // 사원이 없으면 신규등록함
                worker.ID = "";
                worker.SITEID = nLtdCode;
                worker.NAME = strName;
                worker.DEPT = strBuse;
                worker.TEL = "";
                worker.ISDELETED = "N";
                worker.JUMIN = strBirth;
                worker.PANO = 0;
                worker.PTNO = "";
                worker.WORKER_ROLE = "EMP_ROLE";
                worker.IPSADATE = "";
                HC_SITE_WORKER saved = hcSiteWorkerRepository.FindOneByBirth(nLtdCode, strName, strBirth);
                if (saved == null)
                {
                    saved = hcSiteWorkerRepository.Insert(worker);
                }
                else
                {
                    saved = hcSiteWorkerRepository.Update(worker);
                    worker.ID = saved.ID;
                }

            }
            ComFunc.MsgBox("서버로 전송 완료", "알림");
            Screen_Set();
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
