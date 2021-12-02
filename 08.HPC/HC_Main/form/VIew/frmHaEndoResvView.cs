using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComHpcLibB;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Main
{
    public partial class frmHaEndoResvView : Form
    {
        EndoJupmstOrdercodeService endoJupmstOrdercodeService = null;

        public frmHaEndoResvView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            endoJupmstOrdercodeService = new EndoJupmstOrdercodeService();

            SS1.Initialize(new SpreadOption { RowHeight = 26 });
            SS1.AddColumn("등록번호",   nameof(ENDO_JUPMST_ORDERCODE.PTNO),        84, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsSort = true });
            SS1.AddColumn("수검자명",   nameof(ENDO_JUPMST_ORDERCODE.SNAME),       64, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsSort = true });
            SS1.AddColumn("나이",       nameof(ENDO_JUPMST_ORDERCODE.S_AGE),       44, FpSpreadCellType.TextCellType);
            SS1.AddColumn("ORDER 명",   nameof(ENDO_JUPMST_ORDERCODE.ORDERNAME),  200, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS1.AddColumn("예약일시",   nameof(ENDO_JUPMST_ORDERCODE.RDATE),       72, FpSpreadCellType.TextCellType);
            SS1.AddColumn("진료과",     nameof(ENDO_JUPMST_ORDERCODE.DEPTCODE),    72, FpSpreadCellType.TextCellType);
            SS1.AddColumn("진료의사",   nameof(ENDO_JUPMST_ORDERCODE.DRNAME),      92, FpSpreadCellType.TextCellType);
        }

        private void SetEvent()
        {
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnExit.Click      += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                string strFDate = dtpDate.Text;
                string strTDate = dtpDate.Value.AddDays(1).ToShortDateString();

                Screen_Display(strFDate, strTDate, chkHc.Checked);
            }
        }

        private void Screen_Display(string argFDate, string argTDate, bool chkHc)
        {
            SS1.DataSource = endoJupmstOrdercodeService.GetListByRDate(argFDate, argTDate, chkHc);
        }
    }
}
