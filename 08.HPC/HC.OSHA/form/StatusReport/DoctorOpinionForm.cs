using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using ComHpcLibB.Model;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Service.StatusReport;
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

namespace HC_OSHA.form.StatusReport
{
    public partial class DoctorOpinionForm : CommonForm
    {
        public delegate void OnMemoChangedHandle(string memo);
        public event OnMemoChangedHandle MemoChanged;

        private StatusReportDoctorService statusReportDoctorService;
        private MacrowordService macrowordService;
        public StatusReportDoctorDto StatusReportDoctorDto { get; set; }
        private bool bChanged = false;

        public DoctorOpinionForm()
        {
            InitializeComponent();
            macrowordService = new MacrowordService();
            statusReportDoctorService = new StatusReportDoctorService();
        }

        private void DoctorOpinionForm_Load(object sender, EventArgs e)
        {

            TxtOPINION.SetOptions(new TextBoxOption { DataField = nameof(StatusReportDoctorDto.OPINION) });

            //    NumIsEduCount.SetOptions(new NumericUpDownOption { DataField = nameof(NursePerformContentJson.IsEduCount) });

            ssMacroList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            ssMacroList.AddColumnText("상용구", nameof(MacrowordDto.TITLE), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false });
            SearchMacroword();
        }
        private void SearchMacroword()
        {
            List<MacrowordDto> list = macrowordService.FindAll("StatusReportByDoctor", TxtOPINION.Name);
            ssMacroList.SetDataSource(list);
        }
        public void SetDto(StatusReportDoctorDto dto, ISiteModel iSiteModel, IEstimateModel iEstimateModel)
        {
            this.StatusReportDoctorDto = dto;
            TxtOPINION.Text = dto.OPINION;
            this.SelectedSite = iSiteModel;
            this.SelectedEstimate = iEstimateModel;
            Memo_Display();
        }

        public void SetMemo(string strMemo)
        {
            TxtMemo1.Text = strMemo;
        }

        private void BtnMacro_Click(object sender, EventArgs e)
        {
            MacrowordForm form = new MacrowordForm("StatusReportByDoctor", TxtOPINION);
            form.Show();
            SearchMacroword();
        }

        private void ssMacroList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            MacrowordDto dto = ssMacroList.GetRowData(e.Row) as MacrowordDto;
            //  TxtOPINION.Text = dto.CONTENT;
            if (ChkOverWrite.Checked)
            {
                int selectionIndex = TxtOPINION.SelectionStart;
                TxtOPINION.Text = TxtOPINION.Text.Insert(selectionIndex, dto.CONTENT);
                TxtOPINION.SelectionStart = selectionIndex + dto.CONTENT.Length;
            }
            else
            {
                TxtOPINION.Text = dto.CONTENT;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (StatusReportDoctorDto != null)
            {
                if (StatusReportDoctorDto.APPROVE != null)
                {
                    MessageUtil.Info("상태보고서가 완료되어 저장이 불가능합니다.");
                    return;
                }

                if (MessageUtil.Confirm("저장 하시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    this.StatusReportDoctorDto.OPINION = TxtOPINION.Text;
                    statusReportDoctorService.UpdateOpinion(StatusReportDoctorDto);
                    if (Save_Memo(StatusReportDoctorDto.SITE_ID)==true)
                    {
                        MessageUtil.Info("종합의견을 저장하였습니다.");
                    }
                    //if (bChanged == true) MemoChanged(TxtMemo1.Text.Trim());
                }
            }
            else
            {
                MessageUtil.Info("상태보고서가 없습니다");
            }

        }

        //메모 저장
        private bool Save_Memo(long SITE_ID)
        {
            string SQL = "";
            string SqlErr = "";

            int intRowAffected = 0;
            DataTable dt = null;

            //변경되지 않았으면 저장 안함
            if (bChanged == false) return true;

            //기존 메모내역을 읽음
            SQL = "SELECT * FROM HIC_OSHA_MEMO ";
            SQL = SQL + "WHERE SITEID=" + SITE_ID + " ";
            SQL = SQL + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count == 0)
            {
                SQL = "INSERT INTO HIC_OSHA_MEMO (SITEID, MEMO,SWLICENSE) ";
                SQL = SQL + "VALUES (" + SITE_ID + ",'" + TxtMemo1.Text.Trim() + "',";
                SQL = SQL + "'" + clsType.HosInfo.SwLicense + "') ";
            }
            else
            {
                SQL = "UPDATE HIC_OSHA_MEMO SET MEMO='" + TxtMemo1.Text.Trim() + "' ";
                SQL = SQL + "WHERE SITEID=" + SITE_ID + " ";
                SQL = SQL + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            }
            dt.Dispose();
            dt = null;

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("메모 업데이트 오류가 발생함", "알림");
                Cursor.Current = Cursors.Default;
                return false;
            }
            return true;

        }

        //메모 표시
        private void Memo_Display()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //기존 메모내역을 읽음
            SQL = "SELECT * FROM HIC_OSHA_MEMO ";
            SQL = SQL + "WHERE SITEID=" + StatusReportDoctorDto.SITE_ID + " ";
            SQL = SQL + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            TxtMemo1.Text = "";
            if (dt.Rows.Count > 0) TxtMemo1.Text = dt.Rows[0]["MEMO"].ToString().Trim();
            dt.Dispose();
            dt = null;
        }

        private void BtnCard19_Click(object sender, EventArgs e)
        {
            if (base.SelectedSite == null || base.SelectedEstimate == null)
            {
                return;
            }
            if (StatusReportDoctorDto != null)
            {
                if (StatusReportDoctorDto.APPROVE != null)
                {
                    MessageUtil.Info("상태보고서가 완료되어 저장이 불가능합니다.");
                    return;
                }
            }

            CardPage_15_Form form = new CardPage_15_Form();
            form.Show();

            if (base.SelectedSite.ID > 0)
            {
                (form as ISelectSite).Select(base.SelectedSite);
            }

            if (base.SelectedEstimate.ID > 0)
            {
                (form as ISelectEstimate).Select(base.SelectedEstimate);

            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (MessageUtil.Confirm("전체 내용을 지우시겠습니까?", this, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                TxtOPINION.Text = string.Empty;
            }
        }

        private void TxtMemo1_TextChanged(object sender, EventArgs e)
        {
            bChanged = true;
        }

        private void btnMemoRead_Click(object sender, EventArgs e)
        {
            Memo_Display();
        }
    }
}
