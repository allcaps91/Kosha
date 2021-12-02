using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdEduManage.cs
    /// Description     : 교육관리 기초코드등록 
    /// Author          : 안정수
    /// Create Date     : 2018-01-2
    /// Update History  :
    /// </summary>
    /// <history>  
    /// 기존 Frm교육관리기초1New.frm(Frm교육관리기초1New) 폼 frmNrstdEduManage.cs 으로 변경함    
    /// </history>    

    /// <seealso cref= "\nurse\nrstd\Frm교육관리기초1New.frm(Frm교육관리기초1New) >> frmNrstdEduManage.cs 폼이름 재정의" />

    public partial class frmNrstdEduManage : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsNrstd CN = new clsNrstd();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        int intRowAffected = 0; //변경된 Row 받는 변수

        #endregion


        #region MainFormMessage InterFace

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

        #endregion

        public frmNrstdEduManage(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdEduManage()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumsu.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtMan.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRemark.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSeqNo.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTime.KeyPress += new KeyPressEventHandler(eControl_KeyPress);            
            this.cboManJong.KeyPress += new KeyPressEventHandler(eControl_KeyPress);          
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);
                Set_Init();
            }
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

        void eBtnClick(object sender, EventArgs e)
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

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }

            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eDelData();
            }

            else if (sender == this.btnCancel)
            {
                btnCancel_Click();
            }

            else if (sender == this.btnNew)
            {
                btnNew_Click();
            }
        }

        void cboTopic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboTopic.SelectedIndex != -1)
            {
                optEduBun1.Checked = true;
            }

            if(cboTopic.SelectedIndex > 0)
            {
                optEduBun1.Checked = true;
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.cboManJong)
            {
                if(e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void Set_Init()
        {
            ssList.ActiveSheet.Columns[7].Visible = false;

            Screen_Display();
            SCREEN_CLEAR();

            clsNrstd.SET_EDU_JONG(cboJong);

            cboManJong.Items.Clear();
            cboManJong.Items.Add(" ");
            cboManJong.Items.Add("01.의사");
            cboManJong.Items.Add("02.전담간호사");
            cboManJong.Items.Add("03.병동간호사");
            cboManJong.Items.Add("04.타부서의뢰");
            cboManJong.Items.Add("05.기타");
            cboManJong.Items.Add("06.간호사");
            cboJong.SelectedIndex = 0;

            cboEduPlace.Items.Clear();
            cboEduPlace.Items.Add(" ");
            cboEduPlace.Items.Add("마리아홀");
            cboEduPlace.Items.Add("마리아홀 회의실");
            cboEduPlace.Items.Add("BLS 센터");
            cboEduPlace.Items.Add("연구동 회의실");
            cboEduPlace.Items.Add("병동 회의실");
            cboEduPlace.Items.Add("전산 교육실");
            cboEduPlace.Items.Add("컨퍼런스 홀 1층");
            cboEduPlace.Items.Add("컨퍼런스 홀 2층");
            cboEduPlace.SelectedIndex = 0;

            cboTopic.Items.Clear();
            cboTopic.Items.Add(" ");
            cboTopic.Items.Add("환자권리와 의무");
            cboTopic.Items.Add("의료윤리");
            cboTopic.Items.Add("소방안전");
            cboTopic.Items.Add("감염");
            cboTopic.Items.Add("질 향상");
            cboTopic.Items.Add("개인 정보 관리");
            cboTopic.Items.Add("CPR");
            cboTopic.Items.Add("진정교육");
            cboTopic.SelectedIndex = 0;
        }

        void btnCancel_Click()
        {
            SCREEN_CLEAR();
            Screen_Display();
        }

        void SCREEN_CLEAR()
        {
            ComFunc.SetAllControlClear(panel1);
            cboTopic.Text = "";
            optEduBun1.Checked = false;
            optEduBun2.Checked = false;            
        }

        void Screen_Display()
        {
            int i = 0;

            //string strTIME = "";
            //string strSECOND = "";

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  FRDATE, TODATE, DECODE(REQUIRE, '0', '필수', '비필수') REQUIRE, JONG, NAME, MAN, PLACE, EDUTIME, EDUTIME_REMARK, ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE Gubun ='2'";
            SQL += ComNum.VBLF + "Order By EntDate DESC";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;

                for(i =0; i < dt.Rows.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = VB.Left(dt.Rows[i]["FRDATE"].ToString().Trim(), 10);
                    ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REQUIRE"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 2].Text = clsNrstd.READ_EDU_JONG(dt.Rows[i]["JONG"].ToString().Trim());
                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["MAN"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["PLACE"].ToString().Trim();

                    ssList.ActiveSheet.Cells[i, 6].Text = clsNrstd.READ_EDU_TIME(dt.Rows[i]["EDUTIME"].ToString().Trim(), dt.Rows[i]["EDUTIME_REMARK"].ToString().Trim());

                    ssList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }
        }

        void eGetData()
        {
            Screen_Display();
        }

        void eSaveData()
        {
            //int i = 0;

            string strSEQNO = "";
            string strTopic = "";
            string strDate1 = "";
            string strDate2 = "";
            string strTIME = "";
            //string strOptTime = "";
            //string strOptPlace = "";
            string strPlace = "";
            string strMan = "";
            string strRemark = "";
            string strJumsu = "";
            string strJong = "";
            string strManJong = "";

            string strROWID = "";

            string strGUBUN1 = "";
            string strBuse = "";
            string strTIME_REMARK = "";
            string strREQUIRE = "";

            strROWID = txtROWID.Text;

            if(strROWID != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ENTSABUN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND ENTSABUN = '" + clsType.User.Sabun + "'";
                SQL += ComNum.VBLF + "      AND ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count < 1)
                {
                    ComFunc.MsgBox("교육을 등록한 본인만 삭제 및 수정이 가능합니다.");
                    dt.Dispose();
                    dt = null;
                    return;
                }

                dt.Dispose();
                dt = null;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strSEQNO = txtSeqNo.Text.Trim();

                strTopic = cboTopic.Text.Trim();

                strDate1 = dtpFDate.Value.ToString("yyyy-MM-dd");
                strDate2 = dtpTDate.Value.ToString("yyyy-MM-dd");

                if (optTime0.Checked == true)
                {
                    strTIME = "10";
                }

                else if (optTime1.Checked == true)
                {
                    strTIME = "20";
                }

                else if (optTime2.Checked == true)
                {
                    strTIME = "30";
                }

                else if (optTime3.Checked == true)
                {
                    strTIME = "60";
                }

                else if (optTime4.Checked == true)
                {
                    strTIME = "90";
                }

                else if (optTime5.Checked == true)
                {
                    strTIME = "120";
                }

                else if (optTime6.Checked == true)
                {
                    strTIME = "480";
                }

                if (optEduBun1.Checked == true)
                {
                    strREQUIRE = "0";
                }

                else if (optEduBun2.Checked == true)
                {
                    strREQUIRE = "1";
                }

                strTIME_REMARK = txtTime.Text.Trim();

                if (cboEduPlace.SelectedItem != null)
                {
                    strPlace = cboEduPlace.SelectedItem.ToString().Trim();
                }

                strMan = txtMan.Text.Trim();
                strRemark = txtRemark.Text.Trim();
                strJumsu = txtJumsu.Text.Trim();

                if (cboJong.SelectedItem != null)
                {
                    strJong = VB.Left(cboJong.SelectedItem.ToString().Trim(), 2);
                }

                if (cboManJong.SelectedItem != null)
                {
                    strManJong = VB.Left(cboManJong.SelectedItem.ToString().Trim(), 2);
                }

                if (optGubun0.Checked == true)
                {
                    strGUBUN1 = "0";
                    strBuse = "";
                }

                else if (optGubun1.Checked == true)
                {
                    strGUBUN1 = "1";
                    strBuse = clsPublic.GstrPassBuse;
                }

                strROWID = txtROWID.Text.Trim();

                if (strROWID == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                    SQL += ComNum.VBLF + "  (GUBUN, CODE, NAME, FRDATE,";
                    SQL += ComNum.VBLF + "   TODATE, JONG, MAN, MANJONG,";
                    SQL += ComNum.VBLF + "   PLACE, JUMSU, ENTDATE, REMARK,";
                    SQL += ComNum.VBLF + "   EDUGUBUN1, ENTSABUN, ENTBUSE,  EDUTIME,";
                    SQL += ComNum.VBLF + "   REQUIRE, EDUTIME_REMARK )";
                    SQL += ComNum.VBLF + "VALUES (";
                    SQL += ComNum.VBLF + " '2'";
                    SQL += ComNum.VBLF + " , '" + strSEQNO + "'";
                    SQL += ComNum.VBLF + "  ,'" + strTopic + "'";
                    SQL += ComNum.VBLF + "  , TO_DATE('" + strDate1 + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  , TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  , '" + strJong + "'";
                    SQL += ComNum.VBLF + "  , '" + strMan + "'";
                    SQL += ComNum.VBLF + "  , '" + strManJong + "'";
                    SQL += ComNum.VBLF + "  , '" + strPlace + "'";
                    SQL += ComNum.VBLF + "  , '" + strJumsu + "'";
                    SQL += ComNum.VBLF + "  , SYSDATE";
                    SQL += ComNum.VBLF + "  , '" + strRemark + "'";
                    SQL += ComNum.VBLF + "  , '" + strGUBUN1 + "'";
                    SQL += ComNum.VBLF + "  , '" + clsType.User.Sabun + "'";
                    SQL += ComNum.VBLF + "  , '" + strBuse + "'";
                    SQL += ComNum.VBLF + "  , '" + strTIME + "'";
                    SQL += ComNum.VBLF + "  , '" + strREQUIRE + "'";
                    SQL += ComNum.VBLF + "  , '" + strTIME_REMARK + "'";
                    SQL += ComNum.VBLF + ")";
                }

                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                    SQL += ComNum.VBLF + "SET";
                    SQL += ComNum.VBLF + "   NAME = '" + strTopic + "'";
                    SQL += ComNum.VBLF + "  ,FrDate= TO_DATE('" + strDate1 + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  ,ToDate= TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  ,EDUTIME = '" + strTIME + "'";
                    SQL += ComNum.VBLF + "  ,EDUTIME_REMARK = '" + strTIME_REMARK + "'";
                    SQL += ComNum.VBLF + "  ,MAN= '" + strMan + "'";
                    SQL += ComNum.VBLF + "  ,ManJong= '" + strManJong + "'";
                    SQL += ComNum.VBLF + "  ,PLACE= '" + strPlace + "'";
                    SQL += ComNum.VBLF + "  ,Jong= '" + strJong + "'";
                    SQL += ComNum.VBLF + "  ,Remark= '" + strRemark + "'";
                    SQL += ComNum.VBLF + "  ,JumSu= '" + strJumsu + "'";
                    SQL += ComNum.VBLF + "  ,REQUIRE= '" + strREQUIRE + "'";
                    SQL += ComNum.VBLF + "  ,EDUGUBUN1 = '" + strGUBUN1 + "'";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            SCREEN_CLEAR();
            Screen_Display();
        }

        void eDelData()
        {
            //int i = 0;
            string strROWID = "";

            if (MessageBox.Show("자료를 정말로 삭제하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            strROWID = txtROWID.Text.Trim();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ENTSABUN";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ENTSABUN = '" + ComFunc.SetAutoZero(clsType.User.IdNumber, 5) + "'";
            SQL += ComNum.VBLF + "      AND ROWID = '" + strROWID + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count < 1)
            {
                ComFunc.MsgBox("교육을 등록한 본인만 삭제 및 수정이 가능합니다.");
                dt.Dispose();
                dt = null;
                return;
            }

            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("삭제하였습니다.");

            SCREEN_CLEAR();
            Screen_Display();
        }

        void btnNew_Click()
        {
            string strSEQNO = "";

            SCREEN_CLEAR();

            strSEQNO = clsNrstd.READ_NUR_EDU_SEQ();

            btnSave.Enabled = true;

            txtSeqNo.Text = strSEQNO;

            cboTopic.Focus();
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int i = 0;
            SCREEN_CLEAR();
            txtROWID.Text = ssList.ActiveSheet.Cells[e.Row, 7].Text;
            

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  GUBUN, JONG, CODE, NAME,";
            SQL += ComNum.VBLF + "  TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE, TO_CHAR(TODATE,'YYYY-MM-DD') TODATE, MAN, MANJONG,";
            SQL += ComNum.VBLF + "  PLACE, JUMSU, ENTDATE, REMARK,";
            SQL += ComNum.VBLF + "  EDUGUBUN1, EDUTIME, EDUTIME_REMARK, REQUIRE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE ROWID ='" + txtROWID.Text + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtSeqNo.Text = dt.Rows[0]["CODE"].ToString().Trim();
                cboJong.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["Jong"].ToString().Trim()));
                cboManJong.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["ManJong"].ToString().Trim()));
                cboTopic.Text = dt.Rows[0]["NAME"].ToString().Trim();
                if (VB.IsDate(dt.Rows[0]["FrDate"].ToString().Trim()) == true)
                {
                    dtpFDate.Value = Convert.ToDateTime(dt.Rows[0]["FrDate"].ToString().Trim());
                }
                if (VB.IsDate(dt.Rows[0]["ToDate"].ToString().Trim()) == true)
                {
                    dtpTDate.Value = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString().Trim());
                }

                if (dt.Rows[0]["REQUIRE"].ToString().Trim() == "0")
                {
                    optEduBun1.Checked = true;
                }

                else if (dt.Rows[0]["REQUIRE"].ToString().Trim() == "1")
                {
                    optEduBun2.Checked = true;
                }

                switch (dt.Rows[0]["EDUTIME"].ToString().Trim())
                {
                    case "10":
                        optTime0.Checked = true;
                        break;

                    case "20":
                        optTime1.Checked = true;
                        break;

                    case "30":
                        optTime2.Checked = true;
                        break;

                    case "60":
                        optTime3.Checked = true;
                        break;

                    case "90":
                        optTime4.Checked = true;
                        break;

                    case "120":
                        optTime5.Checked = true;
                        break;

                    case "480":
                        optTime6.Checked = true;
                        break;
                }


                txtTime.Text = dt.Rows[0]["EDUTIME_REMARK"].ToString().Trim();

                cboEduPlace.Text = dt.Rows[0]["PLACE"].ToString().Trim();
                txtMan.Text = dt.Rows[0]["MAN"].ToString().Trim();
                txtJumsu.Text = dt.Rows[0]["JUMSU"].ToString().Trim();
                txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();

                switch (dt.Rows[0]["EDUGUBUN1"].ToString().Trim())
                {
                    case "0":
                        optGubun0.Checked = true;
                        optGubun1.Checked = false;
                        break;

                    case "1":
                        optGubun0.Checked = false;
                        optGubun1.Checked = true;
                        break;
                }
            }

            dt.Dispose();
            dt = null;

            btnSave.Enabled = true;
            btnDel.Enabled = true;

        }

        
    }
}
