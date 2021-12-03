using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 인쇄정보
/// Author : 박병규
/// Create Date : 2017.05.31
/// </summary>
/// <history>
/// </history>


namespace ComLibB
{
    public partial class frmSetPrintInfo : Form
    {
        clsQuery CQ = null;
        ComFunc CF = null;
        clsSpread SPR = null;

        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";
        int intRowCnt = 0;

        string strPtno = "";
        string strRemark = "";
        string strGbn = "";

        public frmSetPrintInfo()
        {
            InitializeComponent();
            setParam();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);

            this.txtName.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtPage.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtJumin1.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtJumin2.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtHphone.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboGwange.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);
            this.cboGwange.SelectedIndexChanged += new EventHandler(eCtl_SelectedIndexChanged);
            this.cboUse.KeyPress += new KeyPressEventHandler(eCtl_KeyPress);

            btnSet10.Click += (eCtl_Click);
            btnSet11.Click += (eCtl_Click);
            btnSet12.Click += (eCtl_Click);
            btnSet20.Click += (eCtl_Click);
            btnSet21.Click += (eCtl_Click);
            btnSet22.Click += (eCtl_Click);

            this.btnPrintTemp.Click += new EventHandler(eCtl_Click);
            this.btnPrint.Click += new EventHandler(eCtl_Click);
            this.btnExit.Click += new EventHandler(eCtl_Click);

        }

        private void eCtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == this.cboGwange)
            {
                if (cboGwange.Text.Trim() == "본인")
                {
                    //2018.06.28 박병규 : 추가
                    ssList_CellClick(ssList, null);
                }
            }
        }

        private void eCtl_Click(object sender, EventArgs e)
        {
            if (sender == this.btnPrintTemp)
            {
                clsPublic.GstrRetValue = "OK^^" + Convert.ToString(VB.Val(txtPage.Text.Trim())) + "^^";
                this.Close();
            }
            else if (sender == this.btnPrint)
                Print_Process(clsDB.DbCon);
            else if (sender == this.btnExit)
            {
                clsPublic.GstrRetValue = "";
                this.Close();
            }
            else if (sender == btnSet10 || sender == btnSet11 || sender == btnSet12)
            {
                cboGwange.Text = ((Button)(sender)).Text;
            }
            else if (sender == btnSet20 || sender == btnSet21 || sender == btnSet22)
            {
                cboUse.Text = ((Button)(sender)).Text;
            }
        }

        private void Print_Process(PsmhDb pDbCon)
        {
            if (strPtno == "") { MessageBox.Show("발급자 등록번호 공란!!", "알림"); ssList.Focus(); return; };

            if (txtName.Text.Trim() == "") { MessageBox.Show("발급자 성명 공란!!", "알림"); txtName.Focus(); return; };
            if (txtJumin1.Text.Trim() == "") { MessageBox.Show("발급자 주민번호(앞) 공란!!", "알림"); txtJumin1.Focus(); return; };
            if (txtJumin2.Text.Trim() == "") { MessageBox.Show("발급자 주민번호(뒤) 공란!!", "알림"); txtJumin2.Focus(); return; };
            if (txtHphone.Text.Trim() == "") { MessageBox.Show("발급자 연락처 공란!!", "알림"); txtHphone.Focus(); return; };
            if (cboGwange.Text.Trim() == "") { MessageBox.Show("발급자 관계 공란!!", "알림"); cboGwange.Focus(); return; };
            if (cboUse.Text.Trim() == "") { MessageBox.Show("발급자 용도 공란!!", "알림"); cboUse.Focus(); return; };
            if (VB.Val(txtPage.Text.Trim()) == 0) { MessageBox.Show("인쇄매수 에러!!", "알림"); txtPage.Focus(); return; };

            clsPublic.GstrRetValue = "OK^^" + Convert.ToString(VB.Val(txtPage.Text)) + "^^" + cboGwange.Text + "^^";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO  ADMIN.ETC_PRINT_HIS";
                SQL += ComNum.VBLF + "        (ACTDATE, PANO, USE1, USE2, REMARK, ENTPART,";
                SQL += ComNum.VBLF + "         ENTDATE, PJUMIN1, PJUMIN2, PJUMIN3, PSNAME, PTEL, GUBUN)";
                SQL += ComNum.VBLF + " VALUES (";
                SQL += ComNum.VBLF + "         TRUNC(SYSDATE),";
                SQL += ComNum.VBLF + "         '" + strPtno + "',";
                SQL += ComNum.VBLF + "         '" + cboGwange.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         '" + cboUse.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         '" + strRemark + "',";
                SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ",";
                SQL += ComNum.VBLF + "         SYSDATE,";
                SQL += ComNum.VBLF + "         '" + txtJumin1.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         '" + VB.Left(txtJumin2.Text.Trim(), 1) + "******',";
                SQL += ComNum.VBLF + "         '" + clsAES.AES(txtJumin2.Text.Trim()) + "',";
                SQL += ComNum.VBLF + "         '" + txtName.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         '" + txtHphone.Text.Trim() + "',";
                SQL += ComNum.VBLF + "         '" + strGbn + "') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                //MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            this.Close();
        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtName && e.KeyChar == (Char)13)
                txtPage.Focus();
            else if (sender == this.txtPage && e.KeyChar == (Char)13)
                txtJumin1.Focus();
            else if (sender == this.txtJumin1 && e.KeyChar == (Char)13)
                txtJumin2.Focus();
            else if (sender == this.txtJumin2 && e.KeyChar == (Char)13)
                txtHphone.Focus();
            else if (sender == this.txtHphone && e.KeyChar == (Char)13)
                cboGwange.Focus();
            else if (sender == this.cboGwange && e.KeyChar == (Char)13)
                cboUse.Focus();
            else if (sender == this.cboUse && e.KeyChar == (Char)13)
                btnPrint.Focus();
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            CQ = new clsQuery();
            CF = new ComFunc();
            SPR = new clsSpread();

          //  if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
         //   ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //Form Clear
            SPR.Spread_Clear(ssList, 0, 0);
            ComFunc.SetAllControlClear(pnlBody);

            string[] strGwange = { "", "본인", "보호자", "기타" };
            cboGwange.Items.AddRange(strGwange);
            cboGwange.SelectedIndex = 0;

            string[] strUse = { "보험회사", "기타" };
            cboUse.Items.AddRange(strUse);
            cboUse.SelectedIndex = 0;


            if (VB.Pstr(clsPublic.GstrRetValue, "^^", 1).Trim() != "")
            {
                strPtno = VB.Pstr(clsPublic.GstrRetValue, "^^", 1).Trim();
                strRemark = VB.Pstr(clsPublic.GstrRetValue, "^^", 2).Trim() + " " + VB.Pstr(clsPublic.GstrRetValue, "^^", 3).Trim();
                strGbn = VB.Pstr(clsPublic.GstrRetValue, "^^", 4).Trim();

                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(pnlBody);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SNAME, JUMIN1, JUMIN2, JUMIN3, HPHONE";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND PANO = '" + strPtno + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Cells[0, 0].Text = strPtno;
                    ssList_Sheet1.Cells[0, 1].Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[0, 2].Text = Dt.Rows[0]["JUMIN1"].ToString().Trim();
                    ssList_Sheet1.Cells[0, 3].Text = clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
                    ssList_Sheet1.Cells[0, 4].Text = Dt.Rows[0]["HPHONE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                Cursor.Current = Cursors.Default;

                //2018.06.28 박병규 : 선택해서 사용할수있도록 재요청
                //ssList_CellClick(ssList, null);
            }
            else
            {
                strRemark = "";
            }

            txtPage.Text = "1";
        }


        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            strPtno = ssList_Sheet1.Cells[0, 0].Text.Trim();
            txtName.Text = ssList_Sheet1.Cells[0, 1].Text.Trim();
            txtJumin1.Text = ssList_Sheet1.Cells[0, 2].Text.Trim();
            txtJumin2.Text = ssList_Sheet1.Cells[0, 3].Text.Trim();
            txtHphone.Text = ssList_Sheet1.Cells[0, 4].Text.Trim();

            txtPage.Text = "1";
            cboGwange.Text = "본인";
            cboUse.Text = "보험회사";
            txtName.Focus();
            
        }


    }
}
