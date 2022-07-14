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

            if (CboYear.Items.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    CboYear.Items.Add(nYear.ToString());
                    nYear--;
                }
            }
            CboYear.SelectedIndex = 1;

            if (cboJong.Items.Count == 0)
            {
                cboJong.Items.Add("전체");
                cboJong.Items.Add("특수");
                cboJong.Items.Add("일반");
                cboJong.Items.Add("배치전");
                cboJong.Items.Add("배치후");
            }
            cboJong.SelectedIndex = 0;

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

            //회사관계자 로그인
            if (clsType.User.LtdUser != "")
            {
                TxtLtdcode.Text = clsType.User.LtdUser + "." + clsType.User.JobName;
                TxtLtdcode.Enabled = false;
                BtnSearchSite.Enabled = false;
                btnDelete.Enabled = false;
            }
            //btnDelete.Enabled = false;
            //if (clsType.User.IdNumber == "1") btnDelete.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;

            string strLtdcode = "";
            string strYear = "";
            string strPanjeng = "";
            string strName = "";
            string strJong = "";

            strLtdcode = VB.Pstr(TxtLtdcode.Text.Trim(), ".", 1);
            strYear = CboYear.Text;
            strPanjeng = cboPanjeng.Text;
            strName = txtName.Text;
            if (strPanjeng == "전체") strPanjeng = "";
            strJong = cboJong.Text;
            if (strJong == "전체") strJong = "";

            if (strLtdcode == "") { ComFunc.MsgBox("회사코드가 공란입니다."); return; }

            SSHealthCheck_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "SELECT ID,GONGJENG,NAME,SEX,AGE,GUNSOK,YUHE,GGUBUN,SOGEN,SAHU,";
                SQL = SQL + ComNum.VBLF + " UPMU,YEAR,JINDATE,JIPYO  ";
                SQL = SQL + ComNum.VBLF + "  FROM HIC_LTD_RESULT3 ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITEID=" + strLtdcode + " ";
                SQL = SQL + ComNum.VBLF + "   AND YEAR='" + strYear + "' ";
                if (strJong != "") SQL = SQL + ComNum.VBLF + "   AND JONG='" + strJong + "' ";
                if (strName != "") SQL = SQL + ComNum.VBLF + "   AND NAME LIKE '%" + strName + "%' ";
                if (strPanjeng != "")
                {
                    if (strPanjeng == "A") SQL = SQL + ComNum.VBLF + "   AND GGUBUN='A' ";
                    if (strPanjeng == "C") SQL = SQL + ComNum.VBLF + "   AND GGUBUN='C' ";
                    if (strPanjeng != "A" && strPanjeng != "C") SQL = SQL + ComNum.VBLF + "   AND GGUBUN LIKE '%" + strPanjeng + "%' ";
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
                        SSHealthCheck_Sheet1.Cells[i, 12].Text = VB.Right(dt.Rows[i]["JINDATE"].ToString().Trim(), 5);
                        SSHealthCheck_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ID"].ToString().Trim();
                        SSHealthCheck_Sheet1.Cells[i, 14].Value = false;
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

        private void panSearch_Paint(object sender, PaintEventArgs e)
        {
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            bool bOk = SSHealthCheck.SaveExcel("c:\\temp\\사후관리소견서.xls", FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            {
                if (bOk == true)
                    System.Diagnostics.Process.Start(@"c:\Temp\");
                    //ComFunc.MsgBox("Temp 폴더에 엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int nSelectCnt = 0;
            string strIDs = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComFunc.MsgBoxQ("정말로 선택한것을 삭제하시겠습니까?", "삭제") == DialogResult.No) return;

            // 선택한 것을 찾음
            nSelectCnt = 0;
            strIDs = "";
            for (int i = 0; i < SSHealthCheck_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(SSHealthCheck_Sheet1.Cells[i, 14].Value))
                {
                    nSelectCnt++;
                    if (strIDs == "")
                    {
                        strIDs = SSHealthCheck_Sheet1.Cells[i, 13].Text.Trim();
                    }
                    else
                    {
                        strIDs += "," + SSHealthCheck_Sheet1.Cells[i, 13].Text.Trim();
                    }
                }
            }

            if (nSelectCnt == 0)
            {
                ComFunc.MsgBox("삭제할 자료를 1건도 선택을 않함", "오류");
                return;
            }

            try
            {
                SQL = "DELETE FROM HIC_LTD_RESULT3 ";
                SQL = SQL + "WHERE ID IN (" + strIDs + ") ";
                SQL = SQL + "  AND SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);

                Search();
            }
            catch (Exception ex)
            {
                MessageUtil.Alert("사후관리소견서 삭제 오류");
            }
        }
    }

}

