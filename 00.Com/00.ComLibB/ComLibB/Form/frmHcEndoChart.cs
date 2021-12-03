using ComDbB;
using ComBase;
using ComEmrBase;
using System.Windows.Forms;
using System;
using System.Data;


namespace ComLibB
{
    public partial class frmHcEndoChart : Form, MainFormMessage
    {
        EmrPatient pAcp = null;
        frmEmrChartNew frmEmrChartNewX = null;

        
        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        public frmHcEndoChart()
        {
            InitializeComponent();
        }

        public frmHcEndoChart(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            DtpSDate.Text = strSysDate;
            SS1_Sheet1.Rows.Count = 0;

            btnSearch.PerformClick();

        }


        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, DEPTCODE, PANO, SNAME, SEX || '/' || AGE SEXAGE";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE = TO_DATE('" + DtpSDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE IN('HR', 'TO') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY SNAME ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SEXAGE"].ToString().Trim();
                    }

                }

                dt.Dispose();
                dt = null;

                return;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private void ChartPopup(string strPANO, string strDEPT, string strBDATE, string strFormNo)
        {

            pAcp = clsEmrChart.ClearPatient();
            pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPANO, "O", strBDATE, strDEPT);

            EmrForm bundleForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, strFormNo);

            frmEmrChartNewX = new frmEmrChartNew(bundleForm.FmFORMNO.ToString(), bundleForm.FmUPDATENO.ToString(), pAcp, "0", "W");
            frmEmrChartNewX.StartPosition = FormStartPosition.CenterParent;
            frmEmrChartNewX.FormClosed += FrmEmrChartNewX_FormClosed;
            frmEmrChartNewX.Show(this);
        }

        private void FrmEmrChartNewX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBDATE = "";
            string strDEPT = "";
            string strPANO = "";
            string strFORMNO = "";

            if (e.Column < 5)
            {
                return;
            }

            switch (e.Column)
            {
                case 5:
                    strFORMNO = "2431";
                    break;
                case 6:
                    strFORMNO = "2429";
                    break;
                case 7:
                    strFORMNO = "2433";
                    break;
            }

            strBDATE = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();
            strDEPT = SS1_Sheet1.Cells[e.Row, 1].Text.Trim();
            strPANO = SS1_Sheet1.Cells[e.Row, 2].Text.Trim();

            if (strBDATE != "" && strDEPT != "" && strPANO != "" && strFORMNO != "")
            {
                ChartPopup(strPANO,  strDEPT, strBDATE, strFORMNO);
            }

            btnSearch.PerformClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
