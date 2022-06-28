using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using ComBase.Mvc.Enums;
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

namespace HC_OSHA
{
    public partial class FrmSanjeReport : Form
    {
        public FrmSanjeReport()
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

            cboSanje.Items.Add("*.전체");
            cboSanje.Items.Add("1.사망");
            cboSanje.Items.Add("2.부상");
            cboSanje.Items.Add("3.직업병");
            cboSanje.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strSanje = VB.Pstr(cboSanje.Text, ".", 1);
            if (strSanje == "*") strSanje = "";

            if (cboYear.Text.Trim() == "")
            {
                ComFunc.MsgBox("작업년도를 선택하세요");
                return;
            }

            SSList.ActiveSheet.RowCount = 0;

            SQL = "SELECT A.*,B.ESTIMATE_ID,C.Name AS Sangho ";
            SQL = SQL + ComNum.VBLF + " FROM HIC_OSHA_CARD6 A,HIC_OSHA_CONTRACT B,HIC_LTD C ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACC_DATE>='" + cboYear.Text + "-01-01' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ACC_DATE<='" + cboYear.Text + "-12-31' ";
            if (strSanje != "") SQL = SQL + ComNum.VBLF + "  AND A.IND_ACC_TYPE='" + strSanje + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.ESTIMATE_ID=B.ESTIMATE_ID(+) ";
            SQL = SQL + ComNum.VBLF + "  AND B.OSHA_SITE_ID=C.CODE(+) ";
            //회사관계자 로그인
            if (clsType.User.LtdUser != "") SQL = SQL + ComNum.VBLF + "  AND B.OSHA_SITE_ID='" + clsType.User.LtdUser + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND B.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "  AND C.SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.ACC_DATE,A.NAME ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                SSList.ActiveSheet.RowCount = dt.Rows.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ACC_DATE"].ToString().Trim();
                    if (dt.Rows[i]["IND_ACC_TYPE"].ToString().Trim() == "1") SSList.ActiveSheet.Cells[i, 1].Text = "사망";
                    if (dt.Rows[i]["IND_ACC_TYPE"].ToString().Trim() == "2") SSList.ActiveSheet.Cells[i, 1].Text = "부상";
                    if (dt.Rows[i]["IND_ACC_TYPE"].ToString().Trim() == "3") SSList.ActiveSheet.Cells[i, 1].Text = "직업병";
                    SSList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    if (dt.Rows[i]["JUMIN_NO"].ToString().Trim()!="") SSList.ActiveSheet.Cells[i, 3].Text = clsAES.DeAES(dt.Rows[i]["JUMIN_NO"].ToString().Trim());
                    SSList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Sangho"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["REQUEST_DATE"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["APPROVE_DATE"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["ILLNAME"].ToString().Trim();
                    SSList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                    SSList_Sheet1.Rows[i].Height = SSList_Sheet1.Rows[i].GetPreferredHeight() + 10;
                }
            }
            dt.Dispose();
            dt = null;

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            SpreadPrint sp = new SpreadPrint(SSList, PrintStyle.STANDARD_REPORT);
            sp.Title = cboYear.Text.Trim() + "년 산재 현황 대장";
            //sp.Period = "회사명: " + TxtLtdcode.Text + "          " + "근로자: " + TxtName.Text;
            sp.orientation = PrintOrientation.Landscape;
            sp.Execute();

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSList.SaveExcel("c:\\temp\\산재현황대장.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }

        }
    }
}
