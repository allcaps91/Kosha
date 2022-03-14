using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using HC.Core.Service;
using HC.Core.Common.Interface;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository;
using HC.OSHA.Repository.StatusReport;
using HC.OSHA.Service.StatusReport;
using HC_Core;
using HC_Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace HC_OSHA.form.Visit
{
    public partial class FrmVisitCheck : Form
    {
        public FrmVisitCheck()
        {
            InitializeComponent();
            Screen_Set();
        }

        private void Screen_Set()
        {
            int i = 0;
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));

            if (cboYear.Items.Count == 0)
            {
                for (i = 0; i < 5; i++)
                {
                    cboYear.Items.Add(nYear.ToString());
                    nYear--;
                }
            }
            cboYear.SelectedIndex = 0;

            if (cboVisit.Items.Count == 0)
            {
                cboVisit.Items.Add("의사");
                cboVisit.Items.Add("간호사");
                cboVisit.Items.Add("산업위생");
            }
            cboVisit.SelectedIndex = 1;

        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string strYear = cboYear.Text.Trim();
            string strVisit = cboVisit.Text.Trim();
            string strSiteId = "";
            bool bOK = false;
            int row = 0;
            string strDate = "";

            if (strYear == "" || strVisit == "")
            {
                ComFunc.MsgBox("작업년도, 방문자를 선택하세요");
                return;
            }

            SSList.ActiveSheet.RowCount = 0;

            SQL = "SELECT A.*,B.SANGHO,C.NAME AS UserName ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_CONTRACT A, HIC_LTD B,HIC_USERS C ";
            SQL = SQL + ComNum.VBLF + "WHERE A.CONTRACTENDDATE>='" + cboYear.Text + "-01-01' ";
            SQL = SQL + ComNum.VBLF + "  AND A.CONTRACTSTARTDATE<='" + cboYear.Text + "-12-31' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ISDELETED='N' ";
            SQL = SQL + ComNum.VBLF + "  AND A.OSHA_SITE_ID=B.CODE(+) ";
            if (strVisit=="의사") SQL = SQL + ComNum.VBLF + " AND A.MANAGEDOCTOR=C.USERID(+) ";
            if (strVisit == "간호사") SQL = SQL + ComNum.VBLF + " AND A.MANAGENURSE=C.USERID(+) ";
            if (strVisit == "산업위생") SQL = SQL + ComNum.VBLF + " AND A.MANAGEENGINEER=C.USERID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SANGHO,A.OSHA_SITE_ID ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSiteId = dt.Rows[i]["OSHA_SITE_ID"].ToString().Trim();

                    if (strVisit == "의사")
                    {
                        SQL = "SELECT VISITDATE FROM HIC_OSHA_REPORT_DOCTOR ";
                        SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + strSiteId + " ";
                        SQL = SQL + ComNum.VBLF + "  AND VISITDATE>='" + cboYear.Text + "0101' ";
                        SQL = SQL + ComNum.VBLF + "  AND VISITDATE<='" + cboYear.Text + "1231' ";
                        SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
                        SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE ";
                    }
                    else if (strVisit == "간호사")
                    {
                        SQL = "SELECT VISITDATE FROM HIC_OSHA_REPORT_NURSE ";
                        SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + strSiteId + " ";
                        SQL = SQL + ComNum.VBLF + "  AND VISITDATE>='" + cboYear.Text + "0101' ";
                        SQL = SQL + ComNum.VBLF + "  AND VISITDATE<='" + cboYear.Text + "1231' ";
                        SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
                        SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE ";
                    }
                    else
                    {
                        SQL = "SELECT VISITDATE FROM HIC_OSHA_REPORT_ENGINEER ";
                        SQL = SQL + ComNum.VBLF + "WHERE SITE_ID=" + strSiteId + " ";
                        SQL = SQL + ComNum.VBLF + "  AND VISITDATE>='" + cboYear.Text + "0101' ";
                        SQL = SQL + ComNum.VBLF + "  AND VISITDATE<='" + cboYear.Text + "1231' ";
                        SQL = SQL + ComNum.VBLF + "  AND ISDELETED='N' ";
                        SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY VISITDATE ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    bOK = false;
                    if (chk정상방문.Checked == false) bOK = true;

                    if (bOK == true)
                    {
                        row = SSList.AddRows();

                        SSList.ActiveSheet.Cells[row, 0].Text = strSiteId;
                        SSList.ActiveSheet.Cells[row, 1].Text = dt.Rows[i]["SANGHO"].ToString().Trim();
                        SSList.ActiveSheet.Cells[row, 2].Text = dt.Rows[i]["CONTRACTSTARTDATE"].ToString().Trim();
                        SSList.ActiveSheet.Cells[row, 3].Text = dt.Rows[i]["CONTRACTENDDATE"].ToString().Trim();
                        SSList.ActiveSheet.Cells[row, 4].Text = dt.Rows[i]["UserName"].ToString().Trim();
                        SSList.ActiveSheet.Cells[row, 5].Text = dt.Rows[i]["WORKERTOTALCOUNT"].ToString().Trim();

                        if (strVisit == "의사")
                        {
                           SSList.ActiveSheet.Cells[row, 6].Text = dt.Rows[i]["MANAGEDOCTORCOUNT"].ToString().Trim();
                        }
                        else if (strVisit == "간호사")
                        {
                            SSList.ActiveSheet.Cells[row, 6].Text = dt.Rows[i]["MANAGENURSECOUNT"].ToString().Trim();
                        }  
                        else
                        {
                            SSList.ActiveSheet.Cells[row, 6].Text = dt.Rows[i]["MANAGEENGINEERCOUNT"].ToString().Trim();
                        }
                        // 방문일자를 표시함
                        SSList.ActiveSheet.Cells[row, 7].Text = dt2.Rows.Count.ToString();
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j=0;j< dt2.Rows.Count; j++)
                            {
                                strDate = dt2.Rows[j]["VISITDATE"].ToString().Trim();
                                int col = Int32.Parse(strDate.Substring(4, 2));
                                if (SSList.ActiveSheet.Cells[row, 7 + col].Text == "")
                                {
                                    SSList.ActiveSheet.Cells[row, (7 + col)].Text = strDate.Substring(6, 2);
                                }
                                else
                                {
                                    SSList.ActiveSheet.Cells[row, (7 + col)].Text += "," + strDate.Substring(6, 2);
                                }
                            }
                        }
                        SSList_Sheet1.Rows[row].Height = SSList_Sheet1.Rows[row].GetPreferredHeight();
                    }
                    dt2.Dispose();
                    dt2 = null;
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYear.Text.Trim() + "년 방문 CHECK LIST";
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();
        }
    }
}
