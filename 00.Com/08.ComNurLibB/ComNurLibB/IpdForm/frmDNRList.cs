using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComNurLibB
{
    public partial class frmDNRList : Form, MainFormMessage
    {
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

        string gsWard = "";

        public frmDNRList(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmDNRList()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        private void setCtrlData()
        {
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

        private void ComboWard_SET()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboWard.Items.Clear();

                if (dt.Rows.Count > 0)
                {
                    cboWard.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                }

                cboWard.Items.Add("HD");

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = 0;

                if (gsWard != "")
                {
                    cboWard.SelectedIndex = cboWard.Items.IndexOf(gsWard);
                    cboWard.Enabled = false;
                }

                Cursor.Current = Cursors.Default;


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
            }
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
                gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");


                ComboWard_SET();
                //ComFunc.ComboFind(cboWard, "T", 0);


                TxtEDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                if (cboWard.Text.Trim() == "HD")
                {
                    TxtSDATE.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                }
                else
                {
                    TxtSDATE.Value = TxtEDATE.Value.AddDays(-10);
                }
                SS1.ActiveSheet.SetColumnVisible(11, false);


                eGetData();

            }
        }

        private void eSpreadClick(object sender, EventArgs e)
        {
            if (sender == SS1)
            {
                
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

            string strFrDate = "";
            string strToDate = "";

            int i = 0;
            int nRead = 0;

            strFrDate = TxtSDATE.Text.Trim();
            strToDate = TxtEDATE.Text.Trim();

            SS1.ActiveSheet.RowCount = 0;

            SQL = " SELECT B.WARDCODE, B.ROOMCODE, A.PANO, B.SNAME, B.AGE, ";
            SQL += ComNum.VBLF + "    B.SEX, B.DEPTCODE, (SELECT DRNAME FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DRCODE = B.DRCODE AND ROWNUM = 1) DRNAME, ";
            SQL += ComNum.VBLF + "    TO_CHAR(B.IPWONTIME, 'YYYY-MM-DD') IPWONDATE, TO_CHAR(A.DDATE, 'YYYY-MM-DD') DDATE, A.DWARD, ";
            SQL += ComNum.VBLF + "    (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN3 = A.DSABUN AND ROWNUM = 1) KORNAME, A.IPDNO ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_DNR A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
            if (cboWard.Text.Trim() == "HD")
            {
                SQL += ComNum.VBLF + "  WHERE A.PANO IN ";
                SQL += ComNum.VBLF + "        ( SELECT PANO FROM  TONG_HD_DAILY ";
                SQL += ComNum.VBLF + "           WHERE TDATE >= TO_DATE('" + strFrDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "             AND TDATE <= TO_DATE('" + strToDate + "','YYYY-MM-DD') )";

            }
            else
            {
                SQL += ComNum.VBLF + "  WHERE A.DDATE >= TO_DATE('" + strFrDate + "', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.DDATE <= TO_DATE('" + strToDate + "', 'YYYY-MM-DD') ";
                if (cboWard.Text.Trim() == "전체")
                {
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND B.WARDCODE = '" + cboWard.Text.Trim() + "' ";
                }
            }
            SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO ";
            if (chkToi.Checked == false)
            {
                SQL += ComNum.VBLF + "    AND B.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') ";
            }
            SQL += ComNum.VBLF + "    ORDER BY WARDCODE, ROOMCODE, A.PANO ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                btnView.Enabled = true;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nRead = dt.Rows.Count;

                SS1.ActiveSheet.RowCount = nRead;

                for (i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["SEX"].ToString().Trim() + "/" + dt.Rows[i]["AGE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["IPWONDATE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["DWARD"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                    SS1.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strIPDNO = "";
            string strWard = "";
            if (e.Row < 0 )
            {
                return;
            }

            strIPDNO = SS1_Sheet1.Cells[e.Row, 11].Text.Trim();
            strWard = SS1_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (strIPDNO != "")
            {
                frmDNR frmDNRX = new frmDNR(strIPDNO, strWard);
                frmDNRX.StartPosition = FormStartPosition.CenterParent;
                frmDNRX.ShowDialog();
                clsNurse.setClearMemory(frmDNRX);
                btnView.PerformClick();
            }
        }
    }
}