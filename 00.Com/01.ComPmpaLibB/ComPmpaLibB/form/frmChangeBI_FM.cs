using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmChangeBI_FM : Form, MainFormMessage
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

        public frmChangeBI_FM(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmChangeBI_FM()
        {
            InitializeComponent();
            setEvent();
        }

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
            this.btnSave.Click += new EventHandler(eBtnClick);

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
            else if(sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSetData();
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
                string strDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
                dateTimePicker1.Text = strDate;
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
            DataTable dt2 = null;
            ComFunc CF = new ComFunc();

            string strPano = "";

            int i = 0;

            SS1_Sheet1.RowCount = 0;

            SQL = " SELECT A.PANO, A.SNAME, A.AGE, A.SEX, A.BI, A.ROWID ROWID1 ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_MASTER A ";
            SQL += ComNum.VBLF + " WHERE BDATE = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "   AND DEPTCODE = 'FM' ";
            SQL += ComNum.VBLF + "   AND BI = '43' ";
            SQL += ComNum.VBLF + " ORDER BY PANO ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                dt.Dispose();
                dt = null;
                return;
            }

            SS1_Sheet1.RowCount = dt.Rows.Count;

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();

                    SQL = " SELECT A.PANO ";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.OPD_SUNAP A ";
                    SQL += ComNum.VBLF + " WHERE ACTDATE = TRUNC(SYSDATE) ";
                    SQL += ComNum.VBLF + "   AND DEPTCODE = 'FM' ";
                    SQL += ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);
                    if (dt2.Rows.Count > 0 )
                    {
                        SS1_Sheet1.Cells[i, 6].Text = "수납내역있음";
                    }
                    dt2.Dispose();
                    dt2 = null;

                    SS1_Sheet1.Cells[i, 1].Text = strPano;
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["AGE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BI"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

        }

        private void eSetData()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            int i = 0;
            string strROWID = "";
            string strBIGO = "";

            string strPANO = "";
            string strSNAME = "";

            try
            {

                clsDB.setBeginTran(clsDB.DbCon);

                for (i = 0; i < SS1_Sheet1.RowCount; i++)
                {

                    strPANO = SS1_Sheet1.Cells[i, 1].Text.Trim();
                    strSNAME = SS1_Sheet1.Cells[i, 2].Text.Trim();
                    strBIGO = SS1_Sheet1.Cells[i, 6].Text.Trim();
                    strROWID = SS1_Sheet1.Cells[i, 7].Text.Trim();


                    if (strROWID != "" && SS1_Sheet1.Cells[i, 0].Text == "True")
                    {
                        if (strBIGO != "")
                        {
                            ComFunc.MsgBox(" ★ 등록번호 : " + strPANO + " (" + strSNAME + ")" + ComNum.VBLF + ComNum.VBLF + "해당 환자는 수납내역이 있습니다. 자격 변경이 불가능합니다.");
                        }
                        else
                        {
                            SQL = " INSERT INTO " + ComNum.DB_PMPA + "OPD_MASTER_HISTORY ";
                            SQL += ComNum.VBLF + " SELECT * FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                            SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }

                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_MASTER ";
                            SQL += ComNum.VBLF + "    SET BI = '51', ";
                            SQL += ComNum.VBLF + "        RESERVED = '0', ";
                            SQL += ComNum.VBLF + "        JIN = '5', ";
                            SQL += ComNum.VBLF + "        MKSJIN = '5' ";
                            SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eGetData();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }

    }
}
