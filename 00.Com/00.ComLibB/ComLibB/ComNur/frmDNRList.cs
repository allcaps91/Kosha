using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComLibB;


namespace ComLibB
{
    public partial class frmDNRList : Form, MainFormMessage
    {
        #region //MainFormMessage

        string fstrWard = "";

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

        public frmDNRList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setCtrlData();
            setEvent();
        }

        public frmDNRList()
        {
            InitializeComponent();
            setCtrlData();
            setEvent();
        }

        public frmDNRList(string argWard)
        {
            InitializeComponent();
            fstrWard = argWard;
            setCtrlData();
            setEvent();
        }

        //기본값 세팅
        private void setCtrlData()
        {
            ComFunc cf = new ComFunc();
            TxtSDate.Text = cf.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -5);
            txtEDate.Text = clsPublic.GstrSysDate;

            if (fstrWard == "")
            {
                fstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }
        }

        private void setCtrlInit()
        {
        }

        private void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                eGetData();
            }
        }

        private void eFormActivated(object sender, EventArgs e)
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

        private void eFormClosed(object sender, FormClosedEventArgs e)
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

        private void eGetData()
        {
            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;

            string strSDate = "";
            string strEDate = "";

            string strINTIME = "";
            string strPANO = "";

            int i = 0;
            int nRead = 0;

            strSDate = TxtSDate.Text.Trim();
            strEDate = txtEDate.Text.Trim();

            SS1.ActiveSheet.RowCount = 0;

            if (fstrWard == "")
            {
                MessageBox.Show("병동 구분이 공란입니다. 병동에서만 사용이 가능한 프로그램입니다.");
                return;
            }

            SQL = " SELECT TO_CHAR(INTIME, 'YYYY-MM-DD HH24:MI') INTIME, PANO, CHECK_ER_SABUN, ";
            SQL += ComNum.VBLF + "   CHECK_WARD1_SABUN, IPDNO, TO_CHAR(TODATE,'YYYY-MM-DD HH24:MI') TODATE, TOWARD ";
            SQL += ComNum.VBLF + "   FROM ADMIN.NUR_ER_CHECKLIST_TRANS ";
            SQL += ComNum.VBLF + "  WHERE INTIME >= TO_DATE('" + strSDate + " 00:00', 'YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "    AND INTIME <= TO_DATE('" + strEDate + " 23:59', 'YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "    AND (TOWARD IS NULL OR TOWARD = '" + fstrWard + "') ";
            SQL += ComNum.VBLF + "  ORDER BY INTIME ASC, PANO ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nRead = dt.Rows.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (i = 0; i < nRead; i++)
                {
                    strINTIME = dt.Rows[i]["INTIME"].ToString().Trim();
                    strPANO = dt.Rows[i]["PANO"].ToString().Trim();

                    SS1.ActiveSheet.Cells[i, 1].Text = strINTIME;
                    SS1.ActiveSheet.Cells[i, 2].Text = "";
                    SS1.ActiveSheet.Cells[i, 3].Text = ComFunc.SetAutoZero(strPANO, 8);
                    SS1.ActiveSheet.Cells[i, 4].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());
                    SS1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["CHECK_ER_SABUN"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 6].Text = ReadTransSub(strINTIME, strPANO);
                    SS1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["TODATE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["TOWARD"].ToString().Trim();
                }
            }
            else
            {
                MessageBox.Show("조회된 내용이 없습니다.");
            }

            dt.Dispose();
            dt = null;

            return;
        }

        private string ReadTransSub(string argINTIME, string argPANO)
        {
            string SQL = "";
            string SqlErr = "";

            string strCheckName = "";
            string strQTY = "";
            string strUNIT = "";

            int i = 0;

            string strTemp = "";
            DataTable dt = null;

            SQL = " SELECT CHECKNAME, QTY, UNIT ";
            SQL += ComNum.VBLF + "   FROM ADMIN.NUR_ER_CHECKLIST_TRANS_SUB ";
            SQL += ComNum.VBLF + "  WHERE INTIME = TO_DATE('" + argINTIME + "', 'YYYY-MM-DD HH24:MI') ";
            SQL += ComNum.VBLF + "    AND PANO = '" + argPANO + "' ";
            SQL += ComNum.VBLF + "    AND CHECK_ER = 'Y' ";
            SQL += ComNum.VBLF + "  ORDER BY SEQNO ASC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return "ERROR";
            }

            if (dt.Rows.Count > 0)
            {
                strTemp = "";
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strCheckName = dt.Rows[i]["CHECKNAME"].ToString().Trim();
                    strQTY = dt.Rows[i]["QTY"].ToString().Trim();
                    strUNIT = dt.Rows[i]["UNIT"].ToString().Trim();

                    strTemp += strCheckName;
                    if (strQTY != "")
                    {
                        strTemp += " " + strQTY + strUNIT;
                    }

                    strTemp += ", ";
                }

            }

            dt.Dispose();
            dt = null;

            return strTemp;
        }


    }
}