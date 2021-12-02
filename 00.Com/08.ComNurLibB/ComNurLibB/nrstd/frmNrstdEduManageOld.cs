using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;

namespace ComNurLibB
{
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdEduManageOld.cs
    /// Description     : 교육관리 기초코드등록 
    /// Author          : 안정수
    /// Create Date     : 2018-01-31
    /// TODO : 저장, 삭제 부분 실제 테스트 필요
    /// Update History  :    
    /// </summary>
    /// <history>  
    /// 기존 Frm교육관리기초1.frm(Frm교육관리기초1) 폼 frmNrstdEduManage.cs 으로 변경함    
    /// </history>    

    /// <seealso cref= "\nurse\nrstd\Frm교육관리기초1.frm(Frm교육관리기초1) >> frmNrstdEduManage.cs 폼이름 재정의" />
    public partial class frmNrstdEduManageOld : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        int intRowAffected = 0;

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

        public frmNrstdEduManageOld(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdEduManageOld()
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

            this.cboManJong.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumsu.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtMan.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPlace.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRemark.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSeqNo.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTime.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTopic.KeyPress += new KeyPressEventHandler(eControl_KeyPress);           
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
                Screen_Display();
                SCREEN_CLEAR();
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
                Screen_Display();
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

            else if (sender == this.btnNew)
            {
                btnNew_Click();
            }

            else if (sender == this.btnCancel)
            {
                btnCancel_Click();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.cboManJong || sender == this.txtFDate || sender == this.txtTDate || 
               sender == this.txtJumsu || sender == this.txtMan || sender == this.txtPlace ||
               sender == this.txtRemark || sender == this.txtSeqNo || sender == this.txtTime ||
               sender == this.txtTopic)
            {
                if(e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void Set_Init()
        {
            //int i = 0;

            ssList.ActiveSheet.Columns[7].Visible = false;

            Screen_Display();
            SCREEN_CLEAR();

            cboJong.Items.Clear();
            cboJong.Items.Add(" ");
            cboJong.Items.Add("01.병동교육");
            cboJong.Items.Add("02.감염교육");
            cboJong.Items.Add("03.QI교육");
            cboJong.Items.Add("04.CS교육");
            cboJong.Items.Add("05.CPR교육");
            cboJong.Items.Add("06.학술강좌");
            cboJong.Items.Add("07.간호부주최 직무교육");
            cboJong.Items.Add("08.전직원교육");
            cboJong.Items.Add("09.특강(간협)");
            cboJong.Items.Add("10.연수교육");
            cboJong.Items.Add("11.10대질환");
            cboJong.Items.Add("12.보수교육");
            cboJong.Items.Add("13.기타Report");
            cboJong.Items.Add("14.강사활동(교육)");
            cboJong.Items.Add("15.프리셉터교육");
            cboJong.Items.Add("16.Cyber 교육");
            cboJong.Items.Add("17.승진자교육");
            cboJong.Items.Add("18.기타");            
            cboJong.SelectedIndex = 0;

            cboManJong.Items.Clear();
            cboManJong.Items.Add(" ");
            cboManJong.Items.Add("01.의사");
            cboManJong.Items.Add("02.전담간호사");
            cboManJong.Items.Add("03.병동간호사");
            cboManJong.Items.Add("04.타부서의뢰");
            cboManJong.Items.Add("05.기타");
            cboManJong.Items.Add("06.간호사");
            cboJong.SelectedIndex = 0;         
        }

        void btnCancel_Click()
        {
            SCREEN_CLEAR();
            Screen_Display();
        }

        void SCREEN_CLEAR()
        {
            ComFunc.SetAllControlClear(panel1);
            cboJong.Text = "";

            int i = 0;

            RadioButton[] OptTime = new RadioButton[5] { optTime0, optTime1, optTime2, optTime3, optTime4 };
            RadioButton[] OptPlace = new RadioButton[2] { optPlace0, optPlace1 };

            for(i = 0; i < OptTime.Length; i++)
            {
                OptTime[i].Checked = false;
            }

            for (i = 0; i < OptPlace.Length; i++)
            {
                OptPlace[i].Checked = false;
            }

            optGubun0.Checked = true;
        }

        void Screen_Display()
        {
            int i = 0;

            //string strTIME = "";
            //string strSECOND = "";

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  GUBUN , JONG, Code, Name,";
            SQL += ComNum.VBLF + "  TO_CHAR(FrDate,'YYYY-MM-DD') FrDate, TO_CHAR(ToDate,'YYYY-MM-DD') ToDate,";
            SQL += ComNum.VBLF + "  OptTime,STIME, MAN,OptPlace, PLACE, JUMSU, ENTDATE,ROWID";
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

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString().Trim();

                    switch (dt.Rows[i]["Jong"].ToString().Trim())
                    {
                        case "01":
                            ssList.ActiveSheet.Cells[i, 1].Text = "병동교육";
                            break;

                        case "02":
                            ssList.ActiveSheet.Cells[i, 1].Text = "감염교육";
                            break;

                        case "03":
                            ssList.ActiveSheet.Cells[i, 1].Text = "QI교육";
                            break;

                        case "04":
                            ssList.ActiveSheet.Cells[i, 1].Text = "CS교육";
                            break;

                        case "05":
                            ssList.ActiveSheet.Cells[i, 1].Text = "CPR교육";
                            break;

                        case "06":
                            ssList.ActiveSheet.Cells[i, 1].Text = "학술강좌";
                            break;

                        case "07":
                            ssList.ActiveSheet.Cells[i, 1].Text = "간호부주최 직무교육";
                            break;

                        case "08":
                            ssList.ActiveSheet.Cells[i, 1].Text = "전직원교육";
                            break;

                        case "09":
                            ssList.ActiveSheet.Cells[i, 1].Text = "특강(간협)";
                            break;

                        case "10":
                            ssList.ActiveSheet.Cells[i, 1].Text = "연수교육";
                            break;

                        case "11":
                            ssList.ActiveSheet.Cells[i, 1].Text = "10대질환";
                            break;

                        case "12":
                            ssList.ActiveSheet.Cells[i, 1].Text = "보수교육";
                            break;

                        case "13":
                            ssList.ActiveSheet.Cells[i, 1].Text = "기타Report";
                            break;

                        case "14":
                            ssList.ActiveSheet.Cells[i, 1].Text = "강사활동(교육) ";
                            break;

                        case "15":
                            ssList.ActiveSheet.Cells[i, 1].Text = "프리셉터교육";
                            break;

                        case "16":
                            ssList.ActiveSheet.Cells[i, 1].Text = "Cyber 교육";
                            break;

                        case "17":
                            ssList.ActiveSheet.Cells[i, 1].Text = "승진자교육";
                            break;

                        default:
                            ssList.ActiveSheet.Cells[i, 1].Text = "기타";
                            break;
                    }
                    
                    ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = VB.Left(dt.Rows[i]["FrDate"].ToString().Trim(), 10);

                    if (dt.Rows[i]["ToDate"].ToString().Trim() != "")
                    {
                        ssList.ActiveSheet.Cells[i, 3].Text = ssList.ActiveSheet.Cells[i, 3].Text + "~" + VB.Left(dt.Rows[i]["ToDate"].ToString().Trim(), 10);
                    }

                    switch (dt.Rows[i]["OptTime"].ToString().Trim())
                    {
                        case "0":
                            ssList.ActiveSheet.Cells[i, 4].Text = "10분";
                            break;

                        case "1":
                            ssList.ActiveSheet.Cells[i, 4].Text = "30분내";
                            break;

                        case "2":
                            ssList.ActiveSheet.Cells[i, 4].Text = "1시간내";
                            break;

                        case "3":
                            ssList.ActiveSheet.Cells[i, 4].Text = "90분";
                            break;

                        case "4":
                            ssList.ActiveSheet.Cells[i, 4].Text = "2시간";
                            break;
                    }

                    if(dt.Rows[i]["STime"].ToString().Trim() != "")
                    {
                        ssList.ActiveSheet.Cells[i, 4].Text = VB.Left(dt.Rows[i]["STime"].ToString().Trim(), 10);
                    }

                    switch (dt.Rows[i]["OptPlace"].ToString().Trim())
                    {
                        case "0":
                            ssList.ActiveSheet.Cells[i, 5].Text = "마리아홀";
                            break;

                        case "1":
                            ssList.ActiveSheet.Cells[i, 5].Text = "466호실";
                            break;
                    }

                    if(dt.Rows[i]["Place"].ToString().Trim() != "")
                    {
                        ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Place"].ToString().Trim();
                    }

                    ssList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["Man"].ToString().Trim();

                    ssList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                ssList.ActiveSheet.Rows[0, ssList.ActiveSheet.Rows.Count - 1].Height = 20;
            }

            dt.Dispose();
            dt = null;
        }

        void btnNew_Click()
        {
            string strSEQNO = "";

            SCREEN_CLEAR();

            strSEQNO = clsNrstd.READ_NUR_EDU_SEQ();

            btnSave.Enabled = true;
            txtTopic.Focus();
        }

        void eSaveData()
        {
            int i = 0;

            string strSEQNO = "";
            string strTopic = "";
            string strDate1 = "";
            string strDate2 = "";
            string strTIME = "";
            string strOptTime = "";
            string strOptPlace = "";
            string strPlace = "";
            string strMan = "";
            string strRemark = "";
            string strJumsu = "";
            string strJong = "";
            string strManJong = "";

            string strROWID = "";

            string strGUBUN1 = "";
            string strBuse = "";           

            strROWID = txtROWID.Text;
            txtROWID.Visible = false;

            if (strROWID != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ENTSABUN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND ENTSABUN = '" + ComFunc.SetAutoZero(clsType.User.IdNumber, 5) + "'";
                SQL += ComNum.VBLF + "      AND ROWID = '" + strROWID + "' ";

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
            }

            clsDB.setBeginTran(clsDB.DbCon);

            strSEQNO = txtSeqNo.Text.Trim();
            strTopic = txtTopic.Text.Trim();
            strDate1 = txtFDate.Text;
            strDate2 = txtTDate.Text;

            RadioButton[] OptTime = new RadioButton[5] { optTime0, optTime1, optTime2, optTime3, optTime4 };
            RadioButton[] OptPlace = new RadioButton[2] { optPlace0, optPlace1 };

            for(i = 0; i < OptTime.Length; i++)
            {
                if(OptTime[i].Checked == true)
                {
                    strOptTime = i.ToString();
                    break;
                }
            }

            strTIME = txtTime.Text.Trim();

            for(i = 0; i < OptPlace.Length; i++)
            {
                if(OptPlace[i].Checked == true)
                {
                    strOptPlace = i.ToString();
                    break;
                }
            }

            strPlace = txtPlace.Text.Trim();
            strMan = txtMan.Text.Trim();
            strRemark = txtRemark.Text.Trim();
            strJumsu = txtJumsu.Text.Trim();
            strJong = VB.Left(cboJong.SelectedItem.ToString().Trim(), 2);
            strManJong = VB.Left(cboManJong.SelectedItem.ToString().Trim(), 2);

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
                SQL += ComNum.VBLF + "   ToDate,OptTime,STIME,Jong,";
                SQL += ComNum.VBLF + "   MAN,ManJong,OptPlace,PLACE,";
                SQL += ComNum.VBLF + "   JUMSU,ENTDATE,Remark,EDUGUBUN1,";
                SQL += ComNum.VBLF + "   ENTSABUN, ENTBUSE )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + " ' 2'";
                SQL += ComNum.VBLF + " , '" + strSEQNO + "'";
                SQL += ComNum.VBLF + "  ,'" + strTopic + "'";
                SQL += ComNum.VBLF + "  , TO_DATE('" + strDate1 + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  , TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  , '" + strOptTime + "'";
                SQL += ComNum.VBLF + "  , '" + strTIME + "'";
                SQL += ComNum.VBLF + "  , '" + strJong + "'";
                SQL += ComNum.VBLF + "  , '" + strMan + "'";
                SQL += ComNum.VBLF + "  , '" + strManJong + "'";
                SQL += ComNum.VBLF + "  , '" + strOptPlace + "'";
                SQL += ComNum.VBLF + "  , '" + strPlace + "'";                
                SQL += ComNum.VBLF + "  , '" + strJumsu + "'";
                SQL += ComNum.VBLF + "  , SYSDATE ";
                SQL += ComNum.VBLF + "  , '" + strRemark + "'";
                SQL += ComNum.VBLF + "  , '" + strGUBUN1 + "'";
                SQL += ComNum.VBLF + "  , '" + ComFunc.SetAutoZero(clsType.User.IdNumber, 5) + "'";
                SQL += ComNum.VBLF + "  , '" + strBuse + "'";                
                SQL += ComNum.VBLF + ")";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + "   Gubun= '2'";
                SQL += ComNum.VBLF + "  ,Code= '" + strSEQNO + "'";
                SQL += ComNum.VBLF + "  ,Name= '" + strTopic + "'";                
                SQL += ComNum.VBLF + "  ,FrDate= TO_DATE('" + strDate1 + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  ,ToDate= TO_DATE('" + strDate2 + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  ,OptTime = '" + strOptTime + "'";
                SQL += ComNum.VBLF + "  ,STIME = '" + strTIME + "'";
                SQL += ComNum.VBLF + "  ,MAN= '" + strMan + "'";
                SQL += ComNum.VBLF + "  ,ManJong= '" + strManJong + "'";
                SQL += ComNum.VBLF + "  ,OptPlace= '" + strOptPlace + "'";
                SQL += ComNum.VBLF + "  ,PLACE= '" + strPlace + "'";
                SQL += ComNum.VBLF + "  ,Jong= '" + strJong + "'";
                SQL += ComNum.VBLF + "  ,Remark= '" + strRemark + "'";
                SQL += ComNum.VBLF + "  ,JumSu= '" + strJumsu + "'";                
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

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");

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

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int i = 0;
            
            txtROWID.Text = ssList.ActiveSheet.Cells[e.Row, 7].Text;

            RadioButton[] OptTime = new RadioButton[5] { optTime0, optTime1, optTime2, optTime3, optTime4 };
            RadioButton[] OptPlace = new RadioButton[2] { optPlace0, optPlace1 };

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  GUBUN,JONG,CODE,NAME,TO_CHAR(FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate,";
            SQL += ComNum.VBLF + "  OptTime,STIME,MAN,ManJong,OptPlace,PLACE,JUMSU,ENTDATE,REMARK, EDUGUBUN1 ";            
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
                txtTopic.Text = dt.Rows[0]["NAME"].ToString().Trim();
                txtFDate.Text = VB.Left(dt.Rows[0]["FrDate"].ToString().Trim(), 10);
                txtTDate.Text = VB.Left(dt.Rows[0]["ToDate"].ToString().Trim(), 10);

                if (dt.Rows[0]["OptTime"].ToString().Trim() != "")
                {
                    OptTime[Convert.ToInt32(VB.Val(dt.Rows[0]["OptTime"].ToString().Trim()))].Checked = true;
                }

                txtTime.Text = dt.Rows[0]["STime"].ToString().Trim();

                if (dt.Rows[0]["OptPlace"].ToString().Trim() != "")
                {
                    OptPlace[Convert.ToInt32(VB.Val(dt.Rows[0]["OptPlace"].ToString().Trim()))].Checked = true;
                }

                
                txtPlace.Text = dt.Rows[0]["PLACE"].ToString().Trim();
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
