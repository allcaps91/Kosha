using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using ComBase;
using ComLibB;


namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmMedOpdIlbo.cs
    /// Description     : 진료시간,인원및진료스케쥴메세지전달
    /// Author          : 유진호
    /// Create Date     : 2018-01-12
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmIlbo
    /// </history>
    /// </summary>
    public partial class frmMedOpdIlbo : Form, MainFormMessage
    {
        string mPara1 = "";

        ComFunc CF = new ComFunc();        
        int FnRow = 0;
        int FnCol = 0;
        string FstrFLAG = "";
        string FstrROWID = "";
        
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
        
        public frmMedOpdIlbo()
        {
            InitializeComponent();
        }

        public frmMedOpdIlbo(string strFlag)
        {
            InitializeComponent();
            FstrFLAG = strFlag;
        }
        
        public frmMedOpdIlbo(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmMedOpdIlbo(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }
        
        private void frmMedOpdIlbo_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssList_Sheet1.Columns[18].Visible = false;
            ssList_Sheet1.Columns[19].Visible = false;

            ssMessage_Sheet1.Columns[7].Visible = false;
            ssMessage_Sheet1.Columns[8].Visible = false;
            ssMessage_Sheet1.Columns[9].Visible = false;

            ComFunc.ReadSysDate(clsDB.DbCon);

            panPopUp.Visible = false;

            dtpFDate.Text = clsPublic.GstrSysDate;

            ComboSet();
        }


        private void frmMedOpdIlbo_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmMedOpdIlbo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }


        #region // 버튼이벤트

        private void btnChk_Click(object sender, EventArgs e)
        {
            btnChkClick();
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            btnSearchClick();            
        }
                
        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            btnSaveClick();
            btnSearchClick();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 외래일지 현황(일보)" + "/n/n/n/n";
            strHead2 = "/l/f2" + "작업일자 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + VB.Space(20) + "인쇄일자 : " + SysDate;

            ssList_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssList_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssList_Sheet1.PrintInfo.Margin.Left = 35;
            ssList_Sheet1.PrintInfo.Margin.Right = 0;
            ssList_Sheet1.PrintInfo.Margin.Top = 35;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = false;
            ssList_Sheet1.PrintInfo.UseMax = false;
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList.PrintSheet(0);
        }
        
        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (btnSave2Click() == true)
            {
                btnExit2.PerformClick();
            }
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            FnRow = 0;
            FnCol = 0;

            panPopUp.Visible = false;

            FstrROWID = "";
        }

        #endregion

        private void ComboSet()
        {


            if (clsType.User.JobGroup == "JOB013057")
            {
                FstrFLAG = "TOP";
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            cboDept.Items.Clear();

            try
            {

                SQL = SQL + "SELECT DEPTCODE, DEPTNAMEK FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";

                if (FstrFLAG != "TOP" && clsOpdNr.GstrEmrDoct != "")
                {
                    SQL = SQL + " WHERE DEPTCODE IN ( SELECT DrDept1 FROM BAS_DOCTOR WHERE DrCode IN (" + clsOpdNr.GstrEmrDoct + ") )";

                }
                SQL = SQL + " ORDER BY decode(DEPTCODE,'UR',1) , PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {

                    if (FstrFLAG == "TOP" || clsOpdNr.GstrEmrDoct == "")
                    {
                        cboDept.Items.Add("**.전체");

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        }

                        cboDept.Items.Add("JU.주사실");
                        cboDept.Items.Add("SI.심전도실");
                        cboDept.Items.Add("ED.내시경실");
                    }
                    else
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnChkClick()
        {
            string strMsg = "";

            if (VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) == "**")
            {
                if (ComFunc.MsgBoxQ("전체과는 점검시간이 다소 걸립니다. 계소 하시겠습니까?", "작업확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            
            strMsg = DATA_READ_RESERVED();

            if (strMsg != "")
            {
                ComFunc.MsgBox("외래 예약자 점검 오류 목록 " + ComNum.VBLF + ComNum.VBLF + strMsg);
            }

            CheckDualData();


            btnSearch.Enabled = true;
            btnSave.Enabled = false;
        }
        
        private void CheckDualData()
        {

            //일지가 중복으로 표시 되었을 때 하나 지워주는 부분
            //2019-04-08

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO KOSMOS_PMPA.NUR_OPDILJI_DEL( ";
            SQL += ComNum.VBLF + "            ACTDATE, DEPT, DRCODE, ANSABUN1, ANSABUN2, SININWON, GUINWON, ";
            SQL += ComNum.VBLF + "            ILBAN, IPWON, AMTIME1, AMTIME2, AMCNT1, AMCNT2, PMTIME1, PMTIME2, PMCNT1, PMCNT2, ";
            SQL += ComNum.VBLF + "            ANNAME, ENTTIME, ENTSABUN, NTTIME1, NTTIME2, NTCNT1, NTCNT2, NTNAME, DELDATE, DELSABUN) ";
            SQL += ComNum.VBLF + " SELECT ";
            SQL += ComNum.VBLF + "   ACTDATE, DEPT, DRCODE, ANSABUN1, ANSABUN2, SININWON, GUINWON, ";
            SQL += ComNum.VBLF + "   ILBAN, IPWON, AMTIME1, AMTIME2, AMCNT1, AMCNT2, PMTIME1, PMTIME2, PMCNT1, PMCNT2, ";
            SQL += ComNum.VBLF + "   ANNAME, ENTTIME, ENTSABUN, NTTIME1, NTTIME2, NTCNT1, NTCNT2, NTNAME, SYSDATE, " + clsType.User.Sabun;
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_OPDILJI ";
            SQL += ComNum.VBLF + " WHERE ROWID IN ( ";
            SQL += ComNum.VBLF + " SELECT ROWID2 FROM( ";
            SQL += ComNum.VBLF + " SELECT MAX(ROWID) ROWID2, ACTDATE, DEPT, DRCODE, SUM(1) CNT ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_OPDILJI ";
            SQL += ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + " GROUP BY ACTDATE, DEPT, DRCODE ";
            SQL += ComNum.VBLF + " HAVING SUM(1) > 1)) ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("일지 중복 내용 정리 오류 발생-1");
                Cursor.Current = Cursors.Default;
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + " DELETE KOSMOS_PMPA.NUR_OPDILJI ";
            SQL += ComNum.VBLF + " WHERE ROWID IN ( ";
            SQL += ComNum.VBLF + " SELECT ROWID2 FROM( ";
            SQL += ComNum.VBLF + " SELECT MAX(ROWID) ROWID2, ACTDATE, DEPT, DRCODE, SUM(1) CNT ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_OPDILJI ";
            SQL += ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + " GROUP BY ACTDATE, DEPT, DRCODE ";
            SQL += ComNum.VBLF + " HAVING SUM(1) > 1)) ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("일지 중복 내용 정리 오류 발생-2");
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void btnSearchClick()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            
            DATA_READ1(ssList_Sheet1, ssColor_Sheet1);
            DATA_READ2(ssMessage_Sheet1);

            btnSave.Enabled = true;
        }

        private void DATA_READ1(FarPoint.Win.Spread.SheetView Spd, FarPoint.Win.Spread.SheetView Spd2)
        {
            int i = 0;
            int nTAmCnt1 = 0;
            int nTAmCnt2 = 0;
            int nTPmCnt1 = 0;
            int nTPmCnt2 = 0;
            int nTNtCnt1 = 0;
            int nTNtCnt2 = 0;
            int nTICnt = 0;

            int[] nTemp = new int[7];
                        
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Spd.RowCount = 0;

            try
            {
                
                SQL = SQL + " SELECT A.DEPT, A.DRCODE, B.DRNAME, A.AMTIME1, A.AMTIME2, A.AMCNT1, A.AMCNT2,  ";                
                SQL = SQL + ComNum.VBLF + "  A.PMTIME1, A.PMTIME2, A.PMCNT1, A.PMCNT2,   ";
                SQL = SQL + ComNum.VBLF + "  A.NTTIME1, A.NTTIME2, A.NTCNT1, A.NTCNT2, A.NTNAME,   ";
                SQL = SQL + ComNum.VBLF + "  A.IPWON , A.ANNAME, A.ROWID,   ";
                SQL = SQL + ComNum.VBLF + "  A.ANSABUN1, A.ANSABUN2, A.SININWON, A.GUINWON, C.GBJIN, C.GBJin2, C.GBJIN3   ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_OPDILJI A, "+ ComNum.DB_PMPA +"BAS_DOCTOR B, "+ ComNum.DB_PMPA + "BAS_SCHEDULE C, " + ComNum.DB_PMPA + "BAS_CLINICDEPT D ";
                SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                if (FstrFLAG != "TOP" && clsOpdNr.GstrEmrDoct != "")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.DRCODE IN ( " + clsOpdNr.GstrEmrDoct + " )";
                }
                //외래팀장 점진 소화기내과과장 안보이게처라요청
                if (clsType.User.IdNumber == "23767")
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.DRCODE not in ('0116') ";
                }
                SQL = SQL + ComNum.VBLF + " AND B.DRCODE  not in ( '0104','1402','1403','1405' ,'1407')  ";
                SQL = SQL + ComNum.VBLF + " AND A.DRCODE = B.DRCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.ACTDATE = C.SCHDATE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.DRCODE = C.DRCODE(+)";
                SQL = SQL + ComNum.VBLF + " AND A.DEPT = D.DEPTCODE";

                if(VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) != "**") SQL = SQL + ComNum.VBLF + " AND A.DEPT = '" + VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) + "' ";

                SQL = SQL + ComNum.VBLF + " ORDER BY D.PRINTRANKING, decode(a.drcode,'1102',1,'1104',2,'1114',3,'1113', 4, '1108',5 ,'1111', 6,'1120',7 ,'1119',8, '1107', 9, '1125', 10, b.PrintRanking ) ";                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.RowCount = Spd.RowCount + 1;
                    Spd.SetRowHeight(-1, ComNum.SPDROWHT);

                    // Col = 0
                    Spd.Cells[i, 0].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                    // Col = 1
                    if (dt.Rows[i]["DEPT"].ToString().Trim()=="JU")
                    {
                        Spd.Cells[i, 1].Text = "주사실";
                    }
                    else if (dt.Rows[i]["DEPT"].ToString().Trim() == "ED")
                    {
                        Spd.Cells[i, 1].Text = "내시경";
                    }
                    else if (dt.Rows[i]["DEPT"].ToString().Trim() == "SI")
                    {
                        Spd.Cells[i, 1].Text = "심전도";
                    }
                    else if (dt.Rows[i]["DEPT"].ToString().Trim() == "OC")
                    {
                        Spd.Cells[i, 1].Text = "비만클리닉";
                    }
                    else
                    {
                        Spd.Cells[i, 1].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    }
                    // Col = 2
                    Spd.Cells[i, 2].Text = dt.Rows[i]["AmTime1"].ToString().Trim();

                    if (dt.Rows[i]["GBJIN"].ToString().Trim() == "1")
                    {
                        Spd.Cells[i, 2].BackColor = Spd2.Cells[0, 0].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN"].ToString().Trim() == "2")
                    {
                        Spd.Cells[i, 2].BackColor = Spd2.Cells[0, 1].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN"].ToString().Trim() == "3")
                    {
                        Spd.Cells[i, 2].BackColor = Spd2.Cells[0, 2].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN"].ToString().Trim() == "4")
                    {
                        Spd.Cells[i, 2].BackColor = Spd2.Cells[0, 3].BackColor;
                    }
                    // Col = 3
                    Spd.Cells[i, 3].Text = dt.Rows[i]["AmTime2"].ToString().Trim();

                    if (dt.Rows[i]["GBJIN"].ToString().Trim() == "1")
                    {
                        Spd.Cells[i, 3].BackColor = Spd2.Cells[0, 0].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN"].ToString().Trim() == "2")
                    {
                        Spd.Cells[i, 3].BackColor = Spd2.Cells[0, 1].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN"].ToString().Trim() == "3")
                    {
                        Spd.Cells[i, 3].BackColor = Spd2.Cells[0, 2].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN"].ToString().Trim() == "4")
                    {
                        Spd.Cells[i, 3].BackColor = Spd2.Cells[0, 3].BackColor;
                    }

                    for(int j=0;j<7;j++)
                    {
                        nTemp[j] = 0;
                    }
                    // Col = 4
                    if (dt.Rows[i]["AMCNT1"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 4].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 4].Text = dt.Rows[i]["AMCNT1"].ToString().Trim();
                        nTemp[0]= Convert.ToInt32(VB.Val(dt.Rows[i]["AMCNT1"].ToString()));
                        nTAmCnt1 = nTAmCnt1 + nTemp[0];
                    }
                    // Col = 5
                    if (dt.Rows[i]["AMCNT2"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 5].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 5].Text = dt.Rows[i]["AMCNT2"].ToString().Trim();
                        nTemp[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["AMCNT2"].ToString()));
                        nTAmCnt2 = nTAmCnt2 + nTemp[1];
                    }

                    // Col = 6
                    Spd.Cells[i, 6].Text = (nTemp[0] + nTemp[1]).ToString() ;
                    // Col = 8
                    Spd.Cells[i, 8].Text = dt.Rows[i]["PmTime1"].ToString().Trim();

                    if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "1")
                    {
                        Spd.Cells[i, 8].BackColor = Spd2.Cells[0, 0].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "2")
                    {
                        Spd.Cells[i, 8].BackColor = Spd2.Cells[0, 1].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "3")
                    {
                        Spd.Cells[i, 8].BackColor = Spd2.Cells[0, 2].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "4")
                    {
                        Spd.Cells[i, 8].BackColor = Spd2.Cells[0, 3].BackColor;
                    }

                    // Col = 9
                    Spd.Cells[i, 9].Text = dt.Rows[i]["PmTime2"].ToString().Trim();

                    if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "1")
                    {
                        Spd.Cells[i, 9].BackColor = Spd2.Cells[0, 0].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "2")
                    {
                        Spd.Cells[i, 9].BackColor = Spd2.Cells[0, 1].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "3")
                    {
                        Spd.Cells[i, 9].BackColor = Spd2.Cells[0, 2].BackColor;
                    }
                    else if (dt.Rows[i]["GBJIN2"].ToString().Trim() == "4")
                    {
                        Spd.Cells[i, 9].BackColor = Spd2.Cells[0, 3].BackColor;
                    }

                    // Col = 10
                    if (dt.Rows[i]["PMCNT1"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 10].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 10].Text = dt.Rows[i]["PMCNT1"].ToString().Trim();
                        nTemp[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["PMCNT1"].ToString()));
                        nTPmCnt1 = nTPmCnt1 + nTemp[2];
                    }
                    // Col = 11
                    if (dt.Rows[i]["PMCNT2"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 11].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 11].Text = dt.Rows[i]["PMCNT2"].ToString().Trim();
                        nTemp[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["PMCNT2"].ToString()));
                        nTPmCnt2 = nTPmCnt2 + nTemp[3];
                    }
                    // Col = 12 
                    Spd.Cells[i, 12].Text = (nTemp[2] +nTemp[3]).ToString();

                    // Col = 14
                    Spd.Cells[i, 14].Text = dt.Rows[i]["NTTIME1"].ToString().Trim();

                    if (dt.Rows[i]["GBJIN3"].ToString().Trim() == "1")
                    {
                        Spd.Cells[i, 14].BackColor = Spd2.Cells[0, 7].BackColor;
                    }
                    else 
                    {
                        Spd.Cells[i, 14].BackColor = Spd2.Cells[0, 8].BackColor;
                    }

                    // Col = 15
                    Spd.Cells[i, 15].Text = dt.Rows[i]["NTTIME2"].ToString().Trim();

                    if (dt.Rows[i]["GBJIN3"].ToString().Trim() == "1")
                    {
                        Spd.Cells[i, 15].BackColor = Spd2.Cells[0, 7].BackColor;
                    }
                    else
                    {
                        Spd.Cells[i, 15].BackColor = Spd2.Cells[0, 8].BackColor;
                    }

                    // Col = 16
                    if (dt.Rows[i]["NTCNT1"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 16].Text = "";
                    }
                    else
                    {                    
                        Spd.Cells[i, 16].Text = dt.Rows[i]["NTCNT1"].ToString().Trim();
                        nTemp[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["NTCNT1"].ToString()));
                        nTNtCnt1 = nTNtCnt1 + nTemp[4];
                    }
                    // Col = 17
                    if (dt.Rows[i]["NTCNT2"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 17].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 17].Text = dt.Rows[i]["NTCNT2"].ToString().Trim();
                        nTemp[5] = Convert.ToInt32(VB.Val(dt.Rows[i]["NTCNT2"].ToString()));
                        nTNtCnt2 = nTNtCnt2 + nTemp[5];
                    }
                    // Col = 18
                    Spd.Cells[i, 18].Text = (nTemp[4] + nTemp[5]).ToString() ;

                    // Col = 20
                    if (dt.Rows[i]["IPWON"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 20].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 20].Text = dt.Rows[i]["IPWON"].ToString().Trim();
                        nTemp[6] = Convert.ToInt32(VB.Val(dt.Rows[i]["IPWON"].ToString()));
                        nTICnt = nTICnt + nTemp[6];

                    }

                    // Col = 21
                    Spd.Cells[i, 21].Text = READ_NURNAME(dt.Rows[i]["ANSABUN1"].ToString().Trim());//성명
                    if(dt.Rows[i]["ANSABUN2"].ToString().Trim() != "")
                    {
                        Spd.Cells[i, 21].Text = Spd.Cells[i, 21].Text +  "," + READ_NURNAME(dt.Rows[i]["ANSABUN2"].ToString().Trim());
                    }

                    // Col = 22
                    if (dt.Rows[i]["SININWON"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 22].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 22].Text = dt.Rows[i]["SININWON"].ToString().Trim();
                    }
                    // Col = 23
                    if (dt.Rows[i]["GUINWON"].ToString().Trim() == "0")
                    {
                        Spd.Cells[i, 23].Text = "";
                    }
                    else
                    {
                        Spd.Cells[i, 23].Text = dt.Rows[i]["GUINWON"].ToString().Trim();
                    }
                    // Col = 24
                    Spd.Cells[i, 24].Text = Convert.ToString(Convert.ToUInt32(VB.Val(dt.Rows[i]["SININWON"].ToString().Trim()) + VB.Val(dt.Rows[i]["GUINWON"].ToString().Trim())));
                    // Col = 25
                    Spd.Cells[i, 25].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    // Col = 26
                    Spd.Cells[i, 26].Text = dt.Rows[i]["DEPT"].ToString().Trim();

                    //'기획과 김상효샘 요청
                    if (VB.Right(dt.Rows[i]["DRCODE"].ToString().Trim(), 2) == "99")
                    {
                        Spd.Cells[i, 0, i, Spd.ColumnCount - 1].BackColor = Color.FromArgb(210, 233, 255);
                        Spd.Cells[i, 0, i, Spd.ColumnCount - 1].ForeColor = Color.FromArgb(0,0,0);

                        //'과별 최종시간 및 카운트 계산
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT MIN(A.AMTIME1) MinT1, MAX(A.AMTIME2) MaxT1, SUM(A.AMCNT1) CntT1, SUM(A.AMCNT2) CntT2, ";
                        SQL = SQL + ComNum.VBLF + "  MIN (A.PMTIME1) MinT2, MAX(A.PMTIME2) MaxT2, SUM(A.PMCNT1) CntT3, SUM(A.PMCNT2) CntT4, ";
                        SQL = SQL + ComNum.VBLF + "  MIN (A.NTTIME1) MinT3, MAX(A.NTTIME2) MaxT3, SUM(A.NTCNT1) CntT5, SUM(A.NTCNT2) CntT6,  ";
                        SQL = SQL + ComNum.VBLF + " SUM(A.IPWON) CntT7, SUM(A.SININWON) CntT8, SUM(A.GUINWON) CntT9";
                        SQL = SQL + ComNum.VBLF + " FROM NUR_OPDILJI A, BAS_DOCTOR B, BAS_SCHEDULE C";
                        SQL = SQL + ComNum.VBLF + " WHERE A.ACTDATE = TO_DATE('" + VB.Trim(dtpFDate.Text) + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + " AND A.DRCODE = B.DRCODE(+)     AND A.ACTDATE = C.SCHDATE(+)";
                        SQL = SQL + ComNum.VBLF + " AND A.DRCODE = C.DRCODE(+)    AND A.DEPT = '" + dt.Rows[i]["DEPT"].ToString().Trim() + "' ";
                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (dt1 == null)
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            Spd.Cells[i, 2].Text = dt1.Rows[0]["MinT1"].ToString().Trim();
                            Spd.Cells[i, 3].Text = dt1.Rows[0]["MaxT1"].ToString().Trim();
                            Spd.Cells[i, 4].Text = dt1.Rows[0]["CntT1"].ToString().Trim();
                            Spd.Cells[i, 5].Text = dt1.Rows[0]["CntT2"].ToString().Trim();
                            Spd.Cells[i, 6].Text = Convert.ToString(Convert.ToInt32(VB.Val(dt1.Rows[0]["CntT1"].ToString().Trim()) + VB.Val(dt1.Rows[0]["CntT2"].ToString().Trim())));

                            Spd.Cells[i, 8].Text = dt1.Rows[0]["MinT2"].ToString().Trim();
                            Spd.Cells[i, 9].Text = dt1.Rows[0]["MaxT2"].ToString().Trim();
                            Spd.Cells[i, 10].Text = dt1.Rows[0]["CntT3"].ToString().Trim();
                            Spd.Cells[i, 11].Text = dt1.Rows[0]["CntT4"].ToString().Trim();
                            Spd.Cells[i, 12].Text = Convert.ToString(Convert.ToInt32(VB.Val(dt1.Rows[0]["CntT3"].ToString().Trim()) + VB.Val(dt1.Rows[0]["CntT4"].ToString().Trim())));

                            Spd.Cells[i, 14].Text = dt1.Rows[0]["MinT3"].ToString().Trim();
                            Spd.Cells[i, 15].Text = dt1.Rows[0]["MaxT3"].ToString().Trim();
                            Spd.Cells[i, 16].Text = dt1.Rows[0]["CntT5"].ToString().Trim();
                            Spd.Cells[i, 17].Text = dt1.Rows[0]["CntT6"].ToString().Trim();
                            Spd.Cells[i, 18].Text = Convert.ToString(Convert.ToInt32(VB.Val(dt1.Rows[0]["CntT5"].ToString().Trim()) + VB.Val(dt1.Rows[0]["CntT6"].ToString().Trim())));

                            Spd.Cells[i, 20].Text = dt1.Rows[0]["CntT7"].ToString().Trim();
                            Spd.Cells[i, 22].Text = dt1.Rows[0]["CntT8"].ToString().Trim();
                            Spd.Cells[i, 23].Text = dt1.Rows[0]["CntT9"].ToString().Trim();
                            Spd.Cells[i, 24].Text = Convert.ToString(Convert.ToInt32(VB.Val(dt1.Rows[0]["CntT8"].ToString().Trim()) + VB.Val(dt1.Rows[0]["CntT9"].ToString().Trim())));

                        }
                        dt1.Dispose();
                        dt1 = null;                        
                    }                 
                }
                dt.Dispose();
                dt = null;


                //합계
                Spd.RowCount = Spd.RowCount + 1;

                Spd.Cells[Spd.RowCount - 1, 1].Text = "합계";
                Spd.Cells[Spd.RowCount - 1, 4].Text = nTAmCnt1.ToString();
                Spd.Cells[Spd.RowCount - 1, 5].Text = nTAmCnt2.ToString();
                Spd.Cells[Spd.RowCount - 1, 10].Text = nTPmCnt1.ToString();
                Spd.Cells[Spd.RowCount - 1, 11].Text = nTPmCnt2.ToString();
                Spd.Cells[Spd.RowCount - 1, 16].Text = nTNtCnt1.ToString();
                Spd.Cells[Spd.RowCount - 1, 17].Text = nTNtCnt2.ToString();
                Spd.Cells[Spd.RowCount - 1, 20].Text = nTICnt.ToString();
                
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void DATA_READ2(FarPoint.Win.Spread.SheetView Spd)
        {
            
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            Spd.RowCount = 0;
            
            try
            {
                
                SQL = "";
                SQL = SQL + " SELECT A.DRCODE, B.DRNAME, A.STIME, A.ETIME, A.REMARK, A.ROWID,  ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.SCHDATE,'YYYY-MM-DD') SCHDATE,A.GBMSG    ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + " WHERE A.SCHDATE = TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND A.DRCODE = B.DRCODE(+)";
                if (FstrFLAG != "TOP" && clsOpdNr.GstrEmrDoct != "") SQL = SQL + ComNum.VBLF + "   AND A.DRCODE IN ( " + clsOpdNr.GstrEmrDoct + " )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                Spd.RowCount = dt.Rows.Count + 20;
                Spd.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Spd.Cells[i, 1].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        Spd.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        Spd.Cells[i, 3].Text = dt.Rows[i]["SCHDATE"].ToString().Trim();
                        Spd.Cells[i, 4].Text = dt.Rows[i]["STIME"].ToString().Trim();
                        Spd.Cells[i, 5].Text = dt.Rows[i]["ETIME"].ToString().Trim();
                        Spd.Cells[i, 6].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        Spd.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        Spd.Cells[i, 8].Text = dt.Rows[i]["STIME"].ToString().Trim();
                        Spd.Cells[i, 9].Text = dt.Rows[i]["ETIME"].ToString().Trim();
                        Spd.Cells[i, 10].Text = dt.Rows[i]["GBMSG"].ToString().Trim();

                    }
                }
                        
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
        
        //간호사이름
        private string READ_NURNAME(string argSabun)
        {
            
            DataTable dt = null;            
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = SQL + " SELECT KORNAME FROM " + ComNum.DB_ERP + "INSA_MST  ";
                SQL = SQL + ComNum.VBLF + "  WHERE SABUN IN ('" + argSabun.PadLeft(5,'0') +  "') ";
                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KorName"].ToString().Trim();

                }
                else
                {
                    SQL = "";
                    SQL = SQL + " SELECT Name FROM " + ComNum.DB_PMPA + "NUR_CODE  ";
                    SQL = SQL + ComNum.VBLF + "  WHERE CODE = '" + argSabun + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (dt.Rows.Count > 0)
                    {
                        rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = "";
                    }                    
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 외래 예약자 일일점검
        /// </summary>
        /// <returns></returns>
        private string DATA_READ_RESERVED()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDate = "";
            string strMsg = "";

            strDate = dtpFDate.Text.Trim();

            try
            {

                SQL = "";
                SQL = SQL + "SELECT Pano,DeptCode,SNAME,TO_CHAR(BDate,'YYYY-MM-DD') BDate,DrCode,GbSpc ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL = SQL + ComNum.VBLF + "   WHERE ACTDATE =TO_DATE('" + strDate + "' ,'YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "    AND PANO <>'81000004'";
                SQL = SQL + ComNum.VBLF + "    AND reserved ='1' ";
                SQL = SQL + ComNum.VBLF + "    AND Jin NOT IN ('2','8','U','G')";
                SQL = SQL + ComNum.VBLF + "    AND DeptCode NOT IN ('ER','HR','TO','R6')";

                if (VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) != "**") SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + VB.Left(cboDept.SelectedItem.ToString().Trim(), 2) + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY DEPTCODE,PANO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = SQL + "SELECT PTNO ";
                        SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_OORDER a";
                        SQL = SQL + ComNum.VBLF + "   WHERE BDate =TO_DATE('" + strDate + "' ,'YYYY-MM-DD')  ";
                        SQL = SQL + ComNum.VBLF + "     AND A.PTNO ='" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "     AND A.ORDERCODE IS NOT NULL  ";

                        SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                        if (dt2.Rows.Count == 0)
                        {
                            strMsg = strMsg + ComNum.VBLF + dt.Rows[i]["Pano"].ToString().Trim();
                            strMsg = strMsg + " " + dt.Rows[i]["SName"].ToString().Trim();
                            strMsg = strMsg + " " + dt.Rows[i]["DeptCode"].ToString().Trim();
                        }

                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return strMsg;
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            string strDept;
            string strDrCode;
            string strAMTime1;
            string strAMTime2;
            int nAMCnt1;
            int nAMCnt2;

            string strPMTime1;
            string strPMTime2;
            int nPMCnt1;
            int nPMCnt2;

            string strNTTime1;
            string strNTTime2;
            int nNTCnt1;
            int nNTCnt2;

            int nIpwon;
            string strAnName;
            string strROWID;

            string strDrCode_1;
            string strSchDate;
            string strSTIME;
            string strETime;
            string strSTime2;
            string strETime2;
            string strRemark;

            string strMsg = "";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //외래 진료실적
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strDrCode = ssList_Sheet1.Cells[i, 0].Text.Trim();

                    if (strDrCode != "")
                    {
                        if (VB.Right(strDrCode, 2) != "99") //'과는 합계에서 제외
                        {
                            strAMTime1 = ssList_Sheet1.Cells[i, 2].Text.Trim();
                            if (strAMTime1 == "00:00") strAMTime1 = "";

                            strAMTime2 = ssList_Sheet1.Cells[i, 3].Text.Trim();
                            if (strAMTime2 == "00:00") strAMTime2 = "";

                            nAMCnt1 = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 4].Text.Trim()));
                            nAMCnt2 = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 5].Text.Trim()));


                            strPMTime1 = ssList_Sheet1.Cells[i, 8].Text.Trim();
                            if (strPMTime1 == "00:00") strPMTime1 = "";

                            strPMTime2 = ssList_Sheet1.Cells[i, 9].Text.Trim();
                            if (strPMTime2 == "00:00") strPMTime2 = "";

                            nPMCnt1 = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 10].Text.Trim()));
                            nPMCnt2 = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 11].Text.Trim()));


                            strNTTime1 = ssList_Sheet1.Cells[i, 14].Text.Trim();
                            if (strNTTime1 == "00:00") strNTTime1 = "";

                            strNTTime2 = ssList_Sheet1.Cells[i, 15].Text.Trim();
                            if (strNTTime2 == "00:00") strNTTime2 = "";

                            nNTCnt1 = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 16].Text.Trim()));
                            nNTCnt2 = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 17].Text.Trim()));

                            nIpwon = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 20].Text.Trim()));
                            strAnName = ssList_Sheet1.Cells[i, 21].Text.Trim();
                            strROWID = ssList_Sheet1.Cells[i, 25].Text.Trim();
                            strDept = ssList_Sheet1.Cells[i, 26].Text.Trim();


                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_OPDILJI SET ";
                            SQL = SQL + ComNum.VBLF + " SININWON = '" + (nAMCnt1 + nPMCnt1 + nNTCnt1) + "' , ";
                            SQL = SQL + ComNum.VBLF + " GUINWON = '" + (nAMCnt2 + nPMCnt2 + nNTCnt2) + "', ";
                            SQL = SQL + ComNum.VBLF + " IPWON = '" + nIpwon + "', ";
                            SQL = SQL + ComNum.VBLF + " AMTIME1 = '" + strAMTime1 + "', ";
                            SQL = SQL + ComNum.VBLF + " AMTIME2 = '" + strAMTime2 + "', ";
                            SQL = SQL + ComNum.VBLF + " AMCNT1 = '" + nAMCnt1 + "', ";
                            SQL = SQL + ComNum.VBLF + " AMCNT2 = '" + nAMCnt2 + "', ";
                            SQL = SQL + ComNum.VBLF + " PMTIME1 = '" + strPMTime1 + "', ";
                            SQL = SQL + ComNum.VBLF + " PMTIME2 = '" + strPMTime2 + "', ";
                            SQL = SQL + ComNum.VBLF + " PMCNT1 = '" + nPMCnt1 + "',";
                            SQL = SQL + ComNum.VBLF + " PMCNT2 = '" + nPMCnt2 + "',";
                            SQL = SQL + ComNum.VBLF + " NTTIME1 = '" + strNTTime1 + "', ";
                            SQL = SQL + ComNum.VBLF + " NTTIME2 = '" + strNTTime2 + "', ";
                            SQL = SQL + ComNum.VBLF + " NTCNT1 = '" + nNTCnt1 + "', ";
                            SQL = SQL + ComNum.VBLF + " NTCNT2 = '" + nNTCnt2 + "',";
                            SQL = SQL + ComNum.VBLF + " ANNAME = '" + strAnName + "', ";
                            SQL = SQL + ComNum.VBLF + " ENTTIME = SYSDATE,  ";
                            SQL = SQL + ComNum.VBLF + " ENTSABUN = '" + clsType.User.Sabun + "' "; //editedit // TODO: yunjoyon 간호
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }                            
                        }                        
                    }                    
                }



                //의사별 기타 스케쥴

                for (i = 0; i < ssMessage_Sheet1.RowCount; i++)
                {
                    strDrCode_1 = ssMessage_Sheet1.Cells[i, 1].Text.Trim();
                    strSchDate = ssMessage_Sheet1.Cells[i, 3].Text.Trim();

                    strSTIME = ssMessage_Sheet1.Cells[i, 4].Text.Trim();
                    if (strSTIME == "00:00") strSTIME = "";

                    strETime = ssMessage_Sheet1.Cells[i, 5].Text.Trim();
                    if (strETime == "00:00") strETime = "";

                    strRemark = VB.Left(ssMessage_Sheet1.Cells[i, 6].Text.Trim(), 30);

                    strROWID = ssMessage_Sheet1.Cells[i, 7].Text.Trim();
                    

                    strSTime2 = ssMessage_Sheet1.Cells[i, 8].Text.Trim();
                    if (strSTime2 == "00:00") strSTime2 = "";

                    strETime2 = ssMessage_Sheet1.Cells[i, 9].Text.Trim();
                    if (strETime2 == "00:00") strETime2 = "";

                    if (Convert.ToBoolean(ssMessage_Sheet1.Cells[i, 0].Value) == true)
                    {                        
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC  ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }
                    else
                    {

                        if (Convert.ToBoolean(ssMessage_Sheet1.Cells[i, 10].Value) == true)
                        {
                            strMsg = "1";
                        }
                        else
                        {
                            strMsg = "0";
                        }
                        
                        if (strROWID == "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC (DRCODE, SCHDATE, STIME, ETIME, REMARK,GBMSG) VALUES ( ";
                            SQL = SQL + ComNum.VBLF + " '" + strDrCode_1 + "', TO_DATE('" + strSchDate + "','YYYY-MM-DD') , ";
                            SQL = SQL + ComNum.VBLF + " '" + strSTIME + "','" + strETime + "', '" + strRemark + "','" + strMsg + "') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC SET ";
                            SQL = SQL + ComNum.VBLF + " SCHDATE = TO_DATE('" + strSchDate + "','YYYY-MM-DD') , ";
                            SQL = SQL + ComNum.VBLF + " STIME = '" + strSTIME + "', ";
                            SQL = SQL + ComNum.VBLF + " ETIME = '" + strETime + "', ";
                            SQL = SQL + ComNum.VBLF + " REMARK = '" + strRemark + "', ";
                            SQL = SQL + ComNum.VBLF + " GBMSG = '" + strMsg + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }
                }

                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }            
        }

        private bool btnSave2Click()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
                        
            string strSabun1 = "";
            string strSabun2 = "";

            if (lbGunName1.Text != "")
            {
                if (txtGunSabun1.Text != "")
                {
                    strSabun1 = txtGunSabun1.Text.PadLeft(5, '0');
                }
            }
            if (lbGunName2.Text != "")
            {
                if (txtGunSabun2.Text != "")
                {
                    strSabun2 = txtGunSabun2.Text.PadLeft(5, '0');
                }
            }
            

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //update
                if (FstrROWID != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "NUR_OPDILJI SET ";
                    SQL = SQL + ComNum.VBLF + " ANSABUN1 = '" + strSabun1 + "', ";
                    SQL = SQL + ComNum.VBLF + " ANSABUN2 = '" + strSabun2 + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }

                ssList_Sheet1.Cells[FnRow, FnCol].Text = lbGunName1.Text + (lbGunName2.Text != "" ? "," + lbGunName2.Text : "");

                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void ssMessage_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0 || e.Row < 0) return;

            ssList_Sheet1.Rows.Get(e.Row).BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int nRow = 0;
            //string strROWID = "";
            string strDrCode = "";
            string strDrName = "";

            FstrROWID = "";

            if (e.RowHeader == true || e.ColumnHeader == true) return;
            
            if (e.Column == 21)
            {
                FstrROWID = ssList_Sheet1.Cells[e.Row, 25].Text.Trim();

                if(FstrROWID =="")
                {
                    MessageBox.Show("근무자 변경은 근무 시간을 저장한 후 가능합니다");
                    return;
                }

                GunMuClear();

                READ_GunMuSabun(FstrROWID);

                FnRow = e.Row;
                FnCol = e.Column;

                panPopUp.Visible = true;

            }

            if (e.Column != 1) return;

            strDrCode = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
            strDrName = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();

            if (strDrCode == "") return;

            nRow = ssMessage_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;

            ssMessage_Sheet1.Cells[nRow, 1].Text = strDrCode;
            ssMessage_Sheet1.Cells[nRow, 2].Text = strDrName;
            ssMessage_Sheet1.Cells[nRow, 3].Text = clsPublic.GstrSysDate;
            ssMessage_Sheet1.Cells[nRow, 4].Text = clsPublic.GstrSysTime;
            ssMessage_Sheet1.Cells[nRow, 5].Text = "17:00";

        }

        private void GunMuClear()
        {
            txtGunSabun1.Text = "";
            txtGunSabun2.Text = "";
            lbGunName1.Text = "";
            lbGunName2.Text = "";
            //groupBox2.Visible =false;
            //groupBox2.Enabled = false;
        }

        private void READ_GunMuSabun(string argRowid)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "  ANSABUN1, ANSABUN2  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_OPDILJI ";
                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + argRowid + "'  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    txtGunSabun1.Text = dt.Rows[0]["ANSABUN1"].ToString().Trim();
                    txtGunSabun2.Text = dt.Rows[0]["ANSABUN2"].ToString().Trim();
                    lbGunName1.Text = READ_NURNAME(dt.Rows[0]["ANSABUN1"].ToString().Trim()) ;
                    lbGunName2.Text = READ_NURNAME(dt.Rows[0]["ANSABUN2"].ToString().Trim());

                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtGunSabun1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                lbGunName1.Text = READ_NURNAME(txtGunSabun1.Text.PadLeft(5,'0'));
            }
        }

        private void txtGunSabun2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lbGunName2.Text = READ_NURNAME(txtGunSabun2.Text.PadLeft(5, '0'));
            }
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
                   
        }
    }
}
