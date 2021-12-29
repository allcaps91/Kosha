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

namespace HC_OSHA.form.Etc
{
    /// <summary>
    /// 뇌심현괄 위험도 평가
    /// </summary>
    public partial class frmBrainRiskEvalution : CommonForm, ISelectSite
    {

        private HealthCareService healthCareService;
        private BrainRiskEvalutionService brainRiskEvalutionService;
        public frmBrainRiskEvalution()
        {
            InitializeComponent();
            healthCareService = new HealthCareService();
            brainRiskEvalutionService = new BrainRiskEvalutionService();
        }

        private void panSearch_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmBrainRiskEvalution_Load(object sender, EventArgs e)
        {
            int nYear = Int32.Parse(DateTime.Now.ToString("yyyy"));
            for (int i = 0; i < 10; i++)
            {
                CboYear.Items.Add(nYear.ToString());
                nYear--;
            }
            CboYear.SelectedIndex = 0;

            SSList.Initialize(new SpreadOption() { IsRowSelectColor = true });
            SSList.AddColumnText("소속", nameof(BranRiskEvalutionModel.COL1), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false});
            SSList.AddColumnText("성명", nameof(BranRiskEvalutionModel.COL2), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("생년월일", nameof(BranRiskEvalutionModel.COL3), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("검진일", nameof(BranRiskEvalutionModel.COL4), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("검진병원", nameof(BranRiskEvalutionModel.COL5), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("나이", nameof(BranRiskEvalutionModel.COL6), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("성별", nameof(BranRiskEvalutionModel.COL7), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("신장", nameof(BranRiskEvalutionModel.COL8), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("체중", nameof(BranRiskEvalutionModel.COL9), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("BMI", nameof(BranRiskEvalutionModel.COL10), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("허리둘레", nameof(BranRiskEvalutionModel.COL11), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("수축기혈압", nameof(BranRiskEvalutionModel.COL12), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("이완기혈압", nameof(BranRiskEvalutionModel.COL13), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("혈당", nameof(BranRiskEvalutionModel.COL14), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("당화혈색소", nameof(BranRiskEvalutionModel.COL15), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("총콜레스테롤", nameof(BranRiskEvalutionModel.COL16), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("HDL", nameof(BranRiskEvalutionModel.COL17), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("LDL", nameof(BranRiskEvalutionModel.COL18), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("중성지방", nameof(BranRiskEvalutionModel.COL19), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("단백뇨", nameof(BranRiskEvalutionModel.COL20), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("크레아티닌", nameof(BranRiskEvalutionModel.COL21), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("사구체여과율(GFR)", nameof(BranRiskEvalutionModel.COL22), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("흉부-X선", nameof(BranRiskEvalutionModel.COL23), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("심전도", nameof(BranRiskEvalutionModel.COL24), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("안저", nameof(BranRiskEvalutionModel.COL25), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("좌심실비대", nameof(BranRiskEvalutionModel.COL26), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("맥파전달속도", nameof(BranRiskEvalutionModel.COL27), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("발목위팔혈압지수", nameof(BranRiskEvalutionModel.COL28), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("고혈압성망막", nameof(BranRiskEvalutionModel.COL29), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("연령", nameof(BranRiskEvalutionModel.COL30), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("혈당", nameof(BranRiskEvalutionModel.COL31), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("비만,허리", nameof(BranRiskEvalutionModel.COL32), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("HDL", nameof(BranRiskEvalutionModel.COL33), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("콜레스테롤4종", nameof(BranRiskEvalutionModel.COL34), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("신장3종", nameof(BranRiskEvalutionModel.COL35), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("흡연", nameof(BranRiskEvalutionModel.COL36), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("가족력", nameof(BranRiskEvalutionModel.COL37), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("임시", nameof(BranRiskEvalutionModel.COL38), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false, });
            //SSList.AddColumnText("임시", nameof(BranRiskEvalutionModel.COL39), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false });
            //SSList.AddColumnText("죽상동맥경화", nameof(BranRiskEvalutionModel.COL40), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false });
            //SSList.AddColumnText("고혈압성망막", nameof(BranRiskEvalutionModel.COL41), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false });
            //SSList.AddColumnText("당뇨", nameof(BranRiskEvalutionModel.COL42), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("뇌졸증", nameof(BranRiskEvalutionModel.COL43), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("심장질환", nameof(BranRiskEvalutionModel.COL44), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("말초혈관질환", nameof(BranRiskEvalutionModel.COL45), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false });
            //SSList.AddColumnText("신장질환", nameof(BranRiskEvalutionModel.COL46), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false });
            //SSList.AddColumnText("만성콩판병", nameof(BranRiskEvalutionModel.COL47), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, IsVisivle = false });
            SSList.AddColumnText("10년이내심뇌혈관발병확률", nameof(BranRiskEvalutionModel.COL48), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("심뇌혈관나이", nameof(BranRiskEvalutionModel.COL49), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("심뇌혈관발생위험", nameof(BranRiskEvalutionModel.COL50), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("검진결과평가", nameof(BranRiskEvalutionModel.COL51), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("1단계", nameof(BranRiskEvalutionModel.COL52), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("2단계", nameof(BranRiskEvalutionModel.COL53), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("발병위험도평가", nameof(BranRiskEvalutionModel.COL54), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("업무적합성판정", nameof(BranRiskEvalutionModel.COL55), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("개선의견", nameof(BranRiskEvalutionModel.COL56), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("최종평가", nameof(BranRiskEvalutionModel.COL58), 63, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });

            SSList.SetDataSource(new List<BranRiskEvalutionModel>());
            
        }

        public void Select(ISiteModel siteModel)
        {
            base.SelectedSite = siteModel;

            
            if (SelectedSite != null)
            {
                if (SelectedSite.ID > 0)
                {
                    LblSite.Text = siteModel.NAME;
                    Search();
                }
            
            }
            
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int i = 0;
            string strResult = "";

            string year = CboYear.GetValue();
            long siteId = SelectedSite.ID;

            SSList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM HIC_LTD_RESULT2 ";
                SQL = SQL + ComNum.VBLF + " WHERE SWLicense='" + clsType.HosInfo.SwLicense + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SITEID = " + siteId + " ";
                SQL = SQL + ComNum.VBLF + "   AND YEAR = '" + year + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Name,ID ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    SSList_Sheet1.RowCount = dt.Rows.Count;
                    SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strResult = dt.Rows[i]["RESULT"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BUSE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BIRTH"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["JINDATE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["HOSNAME"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        for (int j=1;j<27;j++)
                        {
                            SSList_Sheet1.Cells[i, j+6].Text = VB.Pstr(strResult, "{$}", j);
                        }
                    }
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

        //사용안함: 2021-12-27 LYJ
        private void Search()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (SelectedSite != null)
                {
                    string year = CboYear.GetValue();
                    long siteId = SelectedSite.ID;
                    //List<BranRiskEvalutionModel> list = brainRiskEvalutionService.FindData(SelectedSite.NAME, siteId, year, startDate, endDate, isMore);
                    //SSList.SetDataSource(list);
                }
            }
            catch (Exception ex)
            {
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            SSList.ExportExcel();
        }
    }
}
