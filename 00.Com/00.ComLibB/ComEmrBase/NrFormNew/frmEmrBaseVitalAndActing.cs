using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseVitalAndActing : Form, EmrChartForm, FormEmrMessage
    {

        frmEmrNrNursingActNew frmEmrNrNursingActNewX = null;
        frmEmrVitalSign frmEmrVitalSignX = null;

        #region 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "1761";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        //private mtsPanel15.TransparentPanel panEditLock = null;

        #endregion

        #region EmrChartForm
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        public double SaveDataMsg(string strFlag)
        {
            //TODO
            //double dblEmrNo = pSaveData(strFlag);
            //return dblEmrNo;
            return 0;
        }

        public bool DelDataMsg()
        {
            //TODO
            //return pDelData();
            return false;
        }

        public void ClearFormMsg()
        {
            //TODO
            mstrEmrNo = "0";
            //pClearForm();
        }
        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            return rtnVal;
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0; // clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion


        #region //FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {
            if (mEmrCallForm != null && tabChart.SelectedIndex == 0)
            {
                mEmrCallForm.MsgSave("0");
            }
        }
        public void MsgDelete()
        {


        }
        public void MsgClear()
        {
        }
        public void MsgPrint()
        {

        }
        #endregion

        /// <summary>
        /// 서브폼 클리어
        /// </summary>
        public void SubFormClear()
        {
            if (frmEmrNrNursingActNewX != null)
            {
                frmEmrNrNursingActNewX.Close();
                frmEmrNrNursingActNewX.Dispose();
            }

            if (frmEmrVitalSignX != null)
            {
                frmEmrVitalSignX.Close();
                frmEmrVitalSignX.Dispose();
            }
        }


        public frmEmrBaseVitalAndActing()
        {
            InitializeComponent();
        }

        public frmEmrBaseVitalAndActing(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        public frmEmrBaseVitalAndActing(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm, Panel panParent)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        private void frmEmrBaseVitalAndActing_Load(object sender, EventArgs e)
        {
            #region //간호활동기록
            double FORMNO_W = 1575;
            double UPDATENO_W = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, FORMNO_W);
            EmrForm fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, FORMNO_W.ToString(), UPDATENO_W.ToString());

            frmEmrNrNursingActNewX = new frmEmrNrNursingActNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), p, "0", "W");
            frmEmrNrNursingActNewX.TopLevel = false;
            this.Controls.Add(frmEmrNrNursingActNewX);
            frmEmrNrNursingActNewX.Parent = tabAct;
            frmEmrNrNursingActNewX.Text = fWrite.FmFORMNAME;
            frmEmrNrNursingActNewX.ControlBox = false;
            frmEmrNrNursingActNewX.FormBorderStyle = FormBorderStyle.None;
            frmEmrNrNursingActNewX.Top = 0;
            frmEmrNrNursingActNewX.Left = 0;
            frmEmrNrNursingActNewX.Dock = DockStyle.Fill;
            frmEmrNrNursingActNewX.Show();
            #endregion //간호활동기록

            #region //임상관찰기록지
            FORMNO_W = 3150;
            UPDATENO_W = clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, FORMNO_W);
            fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, FORMNO_W.ToString(), UPDATENO_W.ToString());

            frmEmrVitalSignX = new frmEmrVitalSign(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), p, "0", "W", this);
            frmEmrVitalSignX.TopLevel = false;
            this.Controls.Add(frmEmrVitalSignX);
            frmEmrVitalSignX.Parent = tabVital;
            frmEmrVitalSignX.Text = fWrite.FmFORMNAME;
            frmEmrVitalSignX.ControlBox = false;
            frmEmrVitalSignX.FormBorderStyle = FormBorderStyle.None;
            frmEmrVitalSignX.Top = 0;
            frmEmrVitalSignX.Left = 0;
            frmEmrVitalSignX.Dock = DockStyle.Fill;
            frmEmrVitalSignX.Show();
            #endregion //임상관찰기록지

            tabChart.SelectedIndex = clsEmrPublic.VitalTabIdx;
        }

        private void frmEmrBaseVitalAndActing_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrNrNursingActNewX != null)
            {
                frmEmrNrNursingActNewX.Dispose();
                frmEmrNrNursingActNewX = null;
            }
            if (frmEmrVitalSignX != null)
            {
                frmEmrVitalSignX.Close();
                frmEmrVitalSignX.Dispose();
                frmEmrVitalSignX = null;
            }
        }

        private void mbtnUpTop_Click(object sender, EventArgs e)
        {
            panTop.Height = 30;
        }

        private void mbtnDownTop_Click(object sender, EventArgs e)
        {
            panTop.Height = 60;
        }

        /// <summary>
        /// 환자 정보 세팅
        /// </summary>
        private void SetPatInfo()
        {
            //lblGun.Text = "";
            //lblPrio.Text = "";
            //lblHD.Text = "";
            //lblICUD.Text = "";
            //ssDise_Sheet1.RowCount = 0;

            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            if (p == null) return;

            //lblHD.Text = (VB.DateDiff("d", Convert.ToDateTime(ComFunc.FormatStrToDate(p.medFrDate, "D")), Convert.ToDateTime(strCurDate)) + 1).ToString();
            //TODO
            //GetDataICUD(strCurDate);
            //GetDataDiag(p.inOutCls, p.medFrDate, p.medEndDate, p.medDeptCd);

        }

        /// <summary>
        /// 중증도 세팅
        /// </summary>
        private void SetGunPrio()
        {
            //동국대 경주병원만
            lblGun.Text = "";
            lblPrio.Text = "";

            return;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수



            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT * FROM ICU_Priority ";
            SQL = SQL + ComNum.VBLF + "    WHERE PATIENT_NO = '" + p.ptNo + "'  ";
            //SQL = SQL + ComNum.VBLF + "      AND ORDER_DATE = TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    if (VB.Val(dt.Rows[i]["P1"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "1";
                    }
                    else if (VB.Val(dt.Rows[i]["P2"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "2";
                    }
                    else if (VB.Val(dt.Rows[i]["P3"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "3";
                    }
                    else if (VB.Val(dt.Rows[i]["P4a"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "4a";
                    }
                    else if (VB.Val(dt.Rows[i]["P4b"].ToString().Trim()) != 0)
                    {
                        lblPrio.Text = "4b";
                    }
                }
            }
            dt.Dispose();
            dt = null;

            SQL = " SELECT   BUN FROM MED_OCS.NR_BUN_TONG  ";
            SQL = SQL + ComNum.VBLF + " WHERE    PATIENT_NO    = '" + p.ptNo + "'  ";
            SQL = SQL + ComNum.VBLF + " AND  ACT_DATE    = (";
            SQL = SQL + ComNum.VBLF + "                    SELECT   MAX(ACT_DATE) FROM MED_OCS.NR_BUN_TONG  ";
            SQL = SQL + ComNum.VBLF + "                    WHERE  ACT_DATE    >= TO_DATE('" + ComFunc.FormatStrToDate(p.medFrDate, "D") + "','YYYY-MM-DD') ";
            //SQL = SQL + ComNum.VBLF + "                    AND  ACT_DATE    < TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "                    AND    PATIENT_NO    = '" + p.ptNo + "' )  ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                lblGun.Text = dt.Rows[0]["BUN"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 중증도 세팅
        /// </summary>
        private void SetPatInfoDay()
        {
            lblGun.Text = "";
        }

        private void tabChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsEmrPublic.VitalTabIdx = tabChart.SelectedIndex;
        }
    }
}
