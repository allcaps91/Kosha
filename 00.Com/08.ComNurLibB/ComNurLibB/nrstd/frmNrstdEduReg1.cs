using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdEduReg1.cs
    /// Description     : 교육관리 기초코드등록
    /// Author          : 안정수
    /// Create Date     : 2018-01-30
    /// Update History  : 
    /// 
    /// </summary>
    /// <history>  
    /// 기존 Frm교육관리등록1New.frm(Frm교육관리등록1New) 폼 frmNrstdEduReg1.cs 으로 변경함
    /// </history>

    /// <seealso cref= "\nurse\nrstd\Frm교육관리등록1New.frm(Frm교육관리등록1New) >> frmNrstdEduReg1.cs 폼이름 재정의" />
    public partial class frmNrstdEduReg1 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;
        //DataTable dt2 = null;
        int intRowAffected = 0; //변경된 Row 받는 변수

        //long FnWRTNO = 0;
        string strBuName = "";

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

        public frmNrstdEduReg1(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdEduReg1()
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

            this.btnView_new.Click += new EventHandler(eBtnClick);            
            this.btnSView.Click += new EventHandler(eBtnClick);

            this.btnSAll.Click += new EventHandler(eBtnClick);
            this.btnUSAll.Click += new EventHandler(eBtnClick);
            this.btnChkOK.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);

            this.chkMyEdu.CheckedChanged += new EventHandler(eControl_CheckedChanged);
            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
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

                //clsSpread spread = new clsSpread();
                //spread.setSpdFilter(ssList1, 4, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
                //spread.setSpdFilter(ssList1, 5, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
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

            else if (sender == this.btnView_new)
            {
                btnView_new_Click();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnSView)
            {
                btnSView_Click();
            }

            else if (sender == this.btnSAll)
            {
                btnSAll_Click();
            }

            else if (sender == this.btnUSAll)
            {
                btnUSAll_Click();
            }

            else if (sender == this.btnChkOK)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnChkOK_Click();
            }

            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnDel_Click();
            }
        }

        void eControl_CheckedChanged(object sender, EventArgs e)
        {
            if(sender == this.chkMyEdu)
            {
                if(chkMyEdu.Checked == true)
                {
                    cboBuse2.SelectedIndex = 0;
                }
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.dtpDate)
            {
                if(e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void Set_Init()
        {
            int i = 0;

            dtpDate.Enabled = true;
            ssList1.Enabled = false;
            ssBuse.Enabled = false;

            ssEdu.ActiveSheet.Columns[3].Visible = false;

            ssList1.ActiveSheet.Columns[7].Visible = false;     //부서코드
            ssList1.ActiveSheet.Columns[8].Visible = false;     //직종
            ssList1.ActiveSheet.Columns[9].Visible = false;     //입사일
            ssList1.ActiveSheet.Columns[10].Visible = false;    //ROWID

            eGetData();

            txtTopic.Text = "";

            clsNrstd.SET_EDU_JONG(cboJong);
            //collapsibleSplitContainer1.Enabled = false;
            txtPInfo.Text = "";

            dtpFDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-60).ToShortDateString();
            dtpTDate.Text = clsPublic.GstrSysDate;

            cboBuse.Items.Clear();
            cboBuse2.Items.Clear();
            cboBuse.Items.Add("전체");
            cboBuse2.Items.Add("전체");

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.BUSE, C.NAME";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "NUR_EDU_CODE B, " + ComNum.DB_PMPA + "BAS_BUSE C";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.SABUN = B.ENTSABUN";
            SQL += ComNum.VBLF + "      AND B.GUBUN = '2'";
            SQL += ComNum.VBLF + "      AND B.EDUGUBUN1 = '1'";
            SQL += ComNum.VBLF + "      AND A.BUSE = C.BUCODE";
            SQL += ComNum.VBLF + "GROUP BY A.BUSE, C.NAME";
            SQL += ComNum.VBLF + "ORDER BY C.NAME";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboBuse.Items.Add(dt.Rows[i]["BUSE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                    cboBuse2.Items.Add(dt.Rows[i]["BUSE"].ToString().Trim() + "." + dt.Rows[i]["NAME"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            cboBuse.Items.Add("100251.정형외과(일반)");
            cboBuse2.Items.Add("100251.정형외과(일반)");
            cboBuse.SelectedIndex = 0;
            cboBuse2.SelectedIndex = 0;

            Screen_Display();
        }

        void btnView_new_Click()
        {
            Screen_Display();
        }

        void Screen_Display()
        {
            int i = 0;

            string strSabun = "";
            string strSABUN2 = "";

            txtPInfo.Text = "";

            //collapsibleSplitContainer1.Enabled = false;
            ssEdu.ActiveSheet.Rows.Count = 0;
            CS.Spread_All_Clear(ssEdu);

            if(cboBuse.SelectedItem.ToString().Trim() != "전체")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SABUN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND BUSE = '" + VB.Left(cboBuse.SelectedItem.ToString().Trim(), 6) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        strSabun = strSabun + "'" + dt.Rows[i]["SABUN"].ToString().Trim() + "',";
                    }

                    strSabun = VB.Mid(strSabun, 1, strSabun.Length - 1);
                }

                dt.Dispose();
                dt = null;
            }

            if(cboBuse2.SelectedItem.ToString().Trim() != "전체")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SABUN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND BUSE = '" + VB.Left(cboBuse2.SelectedItem.ToString().Trim(), 6) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strSABUN2 = strSABUN2 + "'" + dt.Rows[i]["SABUN"].ToString().Trim() + "',";
                    }

                    strSABUN2 = VB.Mid(strSABUN2, 1, strSABUN2.Length - 1);
                }

                dt.Dispose();
                dt = null;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  JONG, NAME, TO_CHAR(FrDate,'YYYY-MM-DD') FRDATE, ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun ='2' ";

            if(txtTopic.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND Name LIKE '%" + txtTopic.Text.Trim() + "%'";
            }

            if(cboJong.SelectedItem.ToString().Trim() != "")
            {
                SQL += ComNum.VBLF + "  AND Jong ='" + VB.Left(cboJong.SelectedItem.ToString().Trim(), 2) + "' ";
            }

            SQL += ComNum.VBLF + "      AND FRDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND FRDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')";

            if(cboBuse.SelectedItem.ToString().Trim() == "전체")
            {
                
            }

            else if(strSabun != "")
            {
                SQL += ComNum.VBLF + "  AND ENTSABUN IN (" + strSabun + ") ";
                SQL += ComNum.VBLF + "  AND EDUGUBUN1 = '1'";
            }

            if(chkMyEdu.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND ENTSABUN = '" + clsType.User.Sabun + "'";
            }

            else
            {
                if(cboBuse2.SelectedItem.ToString().Trim() == "전체")
                {

                }
                else
                {
                    SQL += ComNum.VBLF + "AND ENTSABUN IN (" + strSABUN2 + ") ";
                }
            }

            SQL += ComNum.VBLF + "Order By EntDate desc";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if(dt.Rows.Count > 0)
            {
                ssEdu.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssEdu.ActiveSheet.Cells[i, 0].Text = clsNrstd.READ_EDU_JONG(dt.Rows[i]["Jong"].ToString().Trim());
                    ssEdu.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssEdu.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["FrDate"].ToString().Trim();
                    ssEdu.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nREAD = 0;
            int nCNT = 0;

            string strBuCode = "";

            ssBuse.ActiveSheet.Rows.Count = 0;
            CS.Spread_All_Clear(ssBuse);

            ssBuse.ActiveSheet.Rows.Count = 50;
            CS.Spread_All_Clear(ssBuse);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  a.Buse,b.Name BuName,COUNT(*) CNT ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.IpsaDay<=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'))";
            SQL += ComNum.VBLF + "      AND A.BUSE IN (";
            SQL += ComNum.VBLF + "                      SELECT MATCH_CODE FROM KOSMOS_PMPA.NUR_CODE";
            SQL += ComNum.VBLF + "                       WHERE GUBUN = '2'";
            SQL += ComNum.VBLF + "                         AND GBUSE = 'Y'";
            SQL += ComNum.VBLF + "                         AND MATCH_CODE IS NOT NULL)";
            SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
            SQL += ComNum.VBLF + "GROUP BY a.Buse,b.Name";
            SQL += ComNum.VBLF + "ORDER BY a.Buse";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;

                ssBuse.ActiveSheet.Rows.Count = nREAD;

                for(i = 0; i < nREAD; i++)
                {
                    strBuCode = dt.Rows[i]["Buse"].ToString().Trim();

                    if(VB.Left(strBuCode, 2) == "04")
                    {
                        break;
                    }

                    nCNT = Convert.ToInt32(VB.Val(dt.Rows[i]["CNT"].ToString().Trim()));

                    if(nCNT > 0)
                    {
                        nRow += 1;

                        if(nRow > ssBuse.ActiveSheet.Rows.Count)
                        {
                            ssBuse.ActiveSheet.Rows.Count = nRow;
                        }

                        ssBuse.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["Buse"].ToString().Trim();
                        ssBuse.ActiveSheet.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["BuName"].ToString().Trim();
                    }                    
                }
            }

            dt.Dispose();
            dt = null;

            ssBuse.ActiveSheet.Rows.Count = nRow;
            ssBuse.Enabled = true;
        }

        void btnSView_Click()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;
            int nRow = 0;

            string strBuCode = "";
            string strBuCode1 = "";

            for(i = 0; i < ssBuse.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                if(ssBuse.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strBuCode = strBuCode + ssBuse.ActiveSheet.Cells[i, 1].Text + ",";
                    strBuName = strBuName + ssBuse.ActiveSheet.Cells[i, 2].Text + ",";
                }
            }

            ssList1.Enabled = false;
            ssList1.ActiveSheet.Rows.Count = 0;
            CS.Spread_All_Clear(ssList1);

            nRow = 0;

            for(j = 1; j <= VB.I(strBuCode, ",") - 1; j++)
            {
                strBuCode1 = VB.Pstr(strBuCode1, ",", j);

                if(String.Compare(VB.Pstr(strBuCode, ",", j), "40000") > 0)
                {
                    strBuCode1 = ComFunc.LeftH(VB.Pstr(strBuCode, ",", j), 5) + "%";

                    //참석자들 Display
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND a.Buse like '" + strBuCode1 + "'";
                    SQL += ComNum.VBLF + "      AND a.IpsaDay<=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "      AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'))";
                    SQL += ComNum.VBLF + "      AND a.Sabun < '90000'";
                    SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
                    SQL += ComNum.VBLF + "      AND a.Jik=c.Code(+)";
                    SQL += ComNum.VBLF + "      AND c.Gubun='2' ";  //직책
                    SQL += ComNum.VBLF + "ORDER BY c.Code, a.Sabun, a.KorName";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt.Rows.Count > 0)
                    {
                        nREAD = dt.Rows.Count;
                        ssList1.ActiveSheet.Rows.Count = nREAD + nRow;

                        for(i = 0; i < nREAD; i++)
                        {
                            nRow += 1;

                            ssList1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                            ssList1.ActiveSheet.Cells[nRow - 1, 1].Text = i.ToString();
                            ssList1.ActiveSheet.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["BuseName"].ToString().Trim();
                            ssList1.ActiveSheet.Cells[nRow - 1, 3].Text = " " + dt.Rows[i]["JikName"].ToString().Trim();
                            ssList1.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                            ssList1.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["KorName"].ToString().Trim().Replace(" ", "");
                            
                            ssList1.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["Buse"].ToString().Trim();
                            ssList1.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["Jik"].ToString().Trim();
                            ssList1.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["IpsaDay"].ToString().Trim();

                            //여부를 읽음
                            if(VB.Pstr(txtPInfo.Text, " ", 1) != "")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + "SELECT ";
                                SQL += ComNum.VBLF + "  ROWID";
                                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA  + "NUR_EDU_MST";
                                SQL += ComNum.VBLF + "WHERE 1=1";
                                SQL += ComNum.VBLF + "      AND WRTNO= " + VB.Pstr(txtPInfo.Text, " ", 1) + " ";
                                SQL += ComNum.VBLF + "      AND Sabun='" + dt.Rows[i]["Sabun"].ToString().Trim() + "'";

                                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if(dt1.Rows.Count > 0)
                                {
                                    if(dt1.Rows[0]["ROWID"].ToString().Trim() != "")
                                    {
                                        ssList1.ActiveSheet.Cells[nRow - 1, 6].Text = "▣";
                                        ssList1.ActiveSheet.Cells[nRow - 1, 10].Text = dt1.Rows[0]["ROWID"].ToString().Trim();
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
            }

            ssList1.Enabled = true;
            ssList1.Focus();
        }

        void btnSAll_Click()
        {
            int i = 0;

            for(i = 0; i < ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                ssList1.ActiveSheet.Cells[i, 0].Text = "True";
            }
        }

        void btnUSAll_Click()
        {
            int i = 0;

            for (i = 0; i < ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                ssList1.ActiveSheet.Cells[i, 0].Text = "";
            }
        }

        void btnChkOK_Click()
        {
            int i = 0;

            string strROWID = "";
            string strChk = "";
            string strBuCode = "";

            string strWRTNO = "";
            string strSabun = "";
            string strIpSaDay = "";
            //string strJikJong = "";
            string strTopic = "";
            string strJik = "";
            string strEduDate = "";
            string strEduDate2 = "";
            string strEduTime = "";
            string strMan = "";
            string strPlace = "";
            string strJumsu = "";            
            string strJong = "";
            string strSName = "";
            string strEDUTIME_REMARK = "";
            string strREQUIRE = "";

            if(txtPInfo.Text.Trim() == "")
            {
                ComFunc.MsgBox("교육내역을 먼저 선택하십시오");
                return;
            }

            strWRTNO = VB.Pstr(txtPInfo.Text, " ", 1);

            if(strWRTNO == "")
            {
                ComFunc.MsgBox("다시 작업하세요");
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  JONG, CODE, NAME,";
            SQL += ComNum.VBLF + "  TO_CHAR(FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate, ";
            SQL += ComNum.VBLF + "  EDUTIME, EDUTIME_REMARK, MAN, PLACE, JUMSU, REMARK, ENTDATE, ROWID, REQUIRE , MANJONG";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun ='2'";
            SQL += ComNum.VBLF + "      AND Code ='" + strWRTNO + "'";

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
                ComFunc.MsgBox("교육명을 먼저 등록하세요");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                //교육내용 읽어오기
                strJong = dt.Rows[0]["JONG"].ToString().Trim();
                strTopic = dt.Rows[0]["NAME"].ToString().Trim();
                strEduDate = dt.Rows[0]["FRDATE"].ToString().Trim();
                strEduDate2 = dt.Rows[0]["TODATE"].ToString().Trim();
                strEduTime = dt.Rows[0]["EDUTIME"].ToString().Trim();
                strEDUTIME_REMARK = dt.Rows[0]["EDUTIME_REMARK"].ToString().Trim();
                strMan = dt.Rows[0]["MAN"].ToString().Trim();
                strPlace = dt.Rows[0]["PLACE"].ToString().Trim();
                strJumsu = dt.Rows[0]["JUMSU"].ToString().Trim();
                strREQUIRE = dt.Rows[0]["REQUIRE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            //for(i = 0; i < ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            for (i = 0; i < ssList1.ActiveSheet.RowCount; i++)
                {
                strChk = ssList1.ActiveSheet.Cells[i, 0].Text;
                strSabun = ssList1.ActiveSheet.Cells[i, 4].Text;
                strSName = ssList1.ActiveSheet.Cells[i, 5].Text;
                strBuCode = ssList1.ActiveSheet.Cells[i, 7].Text;
                strJik = ssList1.ActiveSheet.Cells[i, 8].Text;
                strIpSaDay = ssList1.ActiveSheet.Cells[i, 9].Text;
                strROWID = ssList1.ActiveSheet.Cells[i, 10].Text;

                SQL = "";

                if(strROWID == "" && strChk == "True")
                {
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_EDU_MST";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "  WRTNO, SABUN, SNAME, BUCODE, ";
                    SQL += ComNum.VBLF + "  IPSADATE, SDATE, JIKJONG, EDUJONG,";
                    SQL += ComNum.VBLF + "  EDUNAME, FRDATE, TODATE, EDUTIME,";
                    SQL += ComNum.VBLF + "  EDUTIME_REMARK, MAN, PLACE, JUMSU ,";
                    SQL += ComNum.VBLF + "  GUBUN, ENTDATE, CERT, REQUIRE";
                    SQL += ComNum.VBLF + ")";
                    SQL += ComNum.VBLF + "VALUES (";
                    SQL += ComNum.VBLF + "   '" + strWRTNO + "'";
                    SQL += ComNum.VBLF + "  ,'" + strSabun + "'";
                    SQL += ComNum.VBLF + "  ,'" + strSName + "'";
                    SQL += ComNum.VBLF + "  ,'" + strBuCode + "'";
                    SQL += ComNum.VBLF + "  ,'" + strIpSaDay + "'";
                    SQL += ComNum.VBLF + "  ,TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  ,'" + strJik + "'";
                    SQL += ComNum.VBLF + "  ,'" + strJong + "'";
                    SQL += ComNum.VBLF + "  ,'" + strTopic + "'";
                    SQL += ComNum.VBLF + "  ,TO_DATE('" + strEduDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  ,TO_DATE('" + strEduDate2 + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  ,'" + strEduTime + "'";
                    SQL += ComNum.VBLF + "  ,'" + strEDUTIME_REMARK + "'";
                    SQL += ComNum.VBLF + "  ,'" + strMan + "'";
                    SQL += ComNum.VBLF + "  ,'" + strPlace + "'";
                    SQL += ComNum.VBLF + "  , '" + strJumsu + "'";
                    SQL += ComNum.VBLF + "  ,'2'";
                    SQL += ComNum.VBLF + "  , SYSDATE";
                    SQL += ComNum.VBLF + "  , '1'";
                    SQL += ComNum.VBLF + "  ,'" + strREQUIRE + "'";
                    SQL += ComNum.VBLF + ")";                    
                }

                if(SQL != "")
                {
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");

            CS.Spread_All_Clear(ssList1);
            eGetData();

            ssList1.ActiveSheet.Rows.Count = 0;
            ssBuse.Focus();
        }

        void btnDel_Click()
        {
            int i = 0;
            string strROWID = "";
            string strChk = "";
            string strOK = "";

            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            for(i = 0; i < ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                strChk = ssList1.ActiveSheet.Cells[i, 0].Text;
                strOK = ssList1.ActiveSheet.Cells[i, 6].Text;
                strROWID = ssList1.ActiveSheet.Cells[i, 10].Text;

                SQL = "";

                if(strROWID != "" && strChk == "True" && strOK == "▣")
                {
                    SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_EDU_MST";
                    SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "'";
                }

                if(SQL != "")
                {
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("삭제하였습니다.");

            CS.Spread_All_Clear(ssList1);
            eGetData();
        }

        void ssList1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.Column == 1)
            {
                if (ssList1.ActiveSheet.Cells[e.Row, 0].Text != "True")
                {
                    ssList1.ActiveSheet.Cells[e.Row, 0].Text = "True";
                }

                else
                {
                    ssList1.ActiveSheet.Cells[e.Row, 0].Text = "";
                }
            }
            else if(e.Column == 4)
            {
                clsSpread.gSpdSortRow(ssList1, 4);
            }
            else if (e.Column == 5)
            {
                clsSpread.gSpdSortRow(ssList1, 5);
            }

            
        }

        void ssList1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //int i = 0;
            //int nCNT = 0;

            if(e.Column != 0)
            {
                return;
            }

            if(ssList1.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
            {
                ssList1.ActiveSheet.Rows[e.Row].BackColor = Color.RoyalBlue;
            }

            else
            {
                ssList1.ActiveSheet.Rows[e.Row].BackColor = Color.White;
            }
        }

        void ssBuse_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //int i = 0;
            //int nCNT = 0;

            if (e.Column != 0)
            {
                return;
            }

            if (ssBuse.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
            {
                ssBuse.ActiveSheet.Rows[e.Row].BackColor = Color.RoyalBlue;
            }

            else
            {
                ssBuse.ActiveSheet.Rows[e.Row].BackColor = Color.White;
            }

        }

        void ssBuse_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;

            string strBuCode = "";

            strBuCode = ssBuse.ActiveSheet.Cells[e.Row, 1].Text;
            strBuName = ssBuse.ActiveSheet.Cells[e.Row, 2].Text;

            ssList1.Enabled = false;
            ssList1.ActiveSheet.Rows.Count = 50;
            CS.Spread_All_Clear(ssList1);

            //참석자를 Display
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.Buse like '" + strBuCode + "'";
            SQL += ComNum.VBLF + "      AND a.IpsaDay<=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD'))";
            SQL += ComNum.VBLF + "      AND a.Sabun < '90000'";
            SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
            SQL += ComNum.VBLF + "      AND a.Jik=c.Code(+)";
            SQL += ComNum.VBLF + "      AND c.Gubun='2' ";  //직책
            SQL += ComNum.VBLF + "ORDER BY c.Code, a.Sabun, a.KorName";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                ssList1.ActiveSheet.Rows.Count = nREAD;

                for (i = 0; i < nREAD; i++)
                {
                    nRow += 1;

                    ssList1.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    ssList1.ActiveSheet.Cells[nRow - 1, 1].Text = i.ToString();
                    ssList1.ActiveSheet.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["BuseName"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[nRow - 1, 3].Text = " " + dt.Rows[i]["JikName"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["KorName"].ToString().Trim().Replace(" ", "");

                    ssList1.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["Buse"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["Jik"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["IpsaDay"].ToString().Trim();

                    //여부를 읽음
                    if (VB.Pstr(txtPInfo.Text, " ", 1) != "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  SIGN, ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_MST";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND WRTNO= " + VB.Pstr(txtPInfo.Text, " ", 1) + " ";
                        SQL += ComNum.VBLF + "      AND Sabun='" + dt.Rows[i]["Sabun"].ToString().Trim() + "'";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["ROWID"].ToString().Trim() != "")
                            {
                                ssList1.ActiveSheet.Cells[nRow - 1, 6].Text = "▣";
                                ssList1.ActiveSheet.Cells[nRow - 1, 10].Text = dt1.Rows[0]["ROWID"].ToString().Trim();

                                if(dt1.Rows[0]["SIGN"].ToString().Trim() == "1")
                                {
                                    ssList1.ActiveSheet.Rows[nRow - 1].BackColor = Color.CadetBlue;
                                }

                                else
                                {
                                    ssList1.ActiveSheet.Rows[nRow - 1].BackColor = Color.White;
                                }

                            
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }
            }

            dt.Dispose();
            dt = null;

            ssList1.Enabled = true;
            ssList1.Focus();
        }

        void ssEdu_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strROWID = "";
            string strJong = "";
            string strSEQNO = "";
            string strTopic = "";

            strROWID = ssEdu.ActiveSheet.Cells[e.Row, 3].Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  GUBUN,JONG,CODE,NAME,";
            SQL += ComNum.VBLF + "  STIME,MAN,PLACE,JUMSU,ENTDATE,REMARK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE ROWID ='" + strROWID + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                strSEQNO = dt.Rows[0]["CODE"].ToString().Trim();
                strJong = clsNrstd.READ_EDU_JONG(dt.Rows[0]["Jong"].ToString().Trim());
                strTopic = dt.Rows[0]["NAME"].ToString().Trim();

                txtPInfo.Text = strSEQNO + " [" + strJong + "] " + strTopic;
            }

            dt.Dispose();
            dt = null;
            //collapsibleSplitContainer1.Enabled = true;
        }

       
    }
}
