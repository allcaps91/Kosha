using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    public partial class frmPersonConditionCheck : Form, MainFormMessage
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
        #endregion

        string fstrWard = "";

        public frmPersonConditionCheck()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPersonConditionCheck(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            setEvent();
        }

        public frmPersonConditionCheck(string strWard)
        {
            InitializeComponent();
            fstrWard = strWard;
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

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            TxtSDATE.Text = strSysDate;

            if (fstrWard == "" && VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "") != "")
            {
                fstrWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");
            }

            ssList_Sheet1.RowCount = 0;

            ComboWard_SET();
            ComFunc.ComboFind(cboWard, "T", 0, fstrWard);

            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.Items.Add("지정");
            cboTeam.SelectedIndex = 0;



            if (ComQuery.NURSE_System_Manager_Check(VB.Val(clsType.User.Sabun)) == true || clsType.User.JobGroup == "JOB017001")
            {
                cboWard.Enabled = true;
            }
            else
            {
                cboWard.Enabled = false;
            }

            btnSearch.PerformClick();


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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSearch)
            {
                //eGetData("75", cboTeam.Text.Trim());
                eGetData(cboWard.Text.Trim(),cboTeam.Text.Trim());
            }
            else if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        private string GetTarget(string argGbn)
        {
            int i = 0;
            string strRtn = "";

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                {
                    if (argGbn == "1")
                    {
                        strRtn += "'" + ssList_Sheet1.Cells[i, 1].Text.Trim() + "',";
                    }
                    else
                    {
                        strRtn += "'" + VB.Left( ssList_Sheet1.Cells[i, 2].Text.Trim(),8) + "',";
                    }
                }
            }
            if (strRtn != "")
            {
                strRtn = VB.Left(strRtn, VB.Len(strRtn) - 1);
            }
            return strRtn;
        }


        private void eGetData(string strWard, string strTeam)
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            string strTemp = "";

            int i = 0;
            int j = 0;
            int nRead = 0;

            //int nRow = 0;

            ssS_Sheet1.RowCount = 0;

            if(rbtn2.Checked == true)
            {
                strTemp = GetTarget("1");
            }
            else if(rbtn3.Checked == true)
            {
                strTemp = GetTarget("2");
            }

            SQL = "  SELECT ROOMCODE ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER M ";
            SQL += ComNum.VBLF + "  WHERE (JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "          OR OUTDATE = TRUNC(SYSDATE)) ";
            if (strWard == "NR" || strWard == "IQ")
            {
                SQL += ComNum.VBLF + "    AND WARDCODE IN ('NR','IQ') ";
            }
            else
            {
                SQL += ComNum.VBLF + "    AND WARDCODE = '" + strWard + " ' ";
            }

            if (cboTeam.Text.Trim() == "지정")
            {
                SQL = SQL + ComNum.VBLF + " AND M.PANO IN (SELECT PTNO FROM KOSMOS_PMPA.NUR_SABUN_PTNO WHERE SABUN = '" + clsType.User.Sabun + "')       ";
            }
            else if (cboTeam.Text.Trim() != "전체")
            {
                SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                SQL = SQL + ComNum.VBLF + " (SELECT * FROM KOSMOS_PMPA.NUR_TEAM_ROOMCODE T";
                SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + strTeam + "')";
            }
            if (rbtn2.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND ROOMCODE IN (" + strTemp + ")";
            }
            else if (rbtn3.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "   AND PANO IN (" + strTemp + ")";
            }


            SQL += ComNum.VBLF + "  GROUP BY ROOMCODE ";
            SQL += ComNum.VBLF + "  ORDER BY ROOMCODE ASC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
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
                nRead = dt.Rows.Count;
                ssS_Sheet1.RowCount = nRead * 5;
                for (i = 0; i < nRead; i++)
                {
                    if ( i %  6 == 0)
                    {
                        ssS_Sheet1.GetRowPageBreak((i + 1) * 5 - 4);
                    }
           
                    SetRowDesign(i + 1);

                    ssS_Sheet1.Cells[(i + 1) * 5 - 4, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();

                    SQL = "  SELECT PANO || ' ' || DEPTCODE top, SNAME || ' ' || AGE || ' ' || SEX bottom ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER M ";
                    SQL += ComNum.VBLF + "  WHERE (JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "          OR OUTDATE = TRUNC(SYSDATE)) ";
                    if (strWard == "NR" || strWard == "IQ")
                    {
                        SQL += ComNum.VBLF + "    AND WARDCODE IN ('NR','IQ') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "    AND WARDCODE = '" + strWard + " ' ";
                    }
                    if (cboTeam.Text.Trim() == "지정")
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.PANO IN (SELECT PTNO FROM KOSMOS_PMPA.NUR_SABUN_PTNO WHERE SABUN = '" + clsType.User.Sabun + "')       ";
                    }
                    else if (cboTeam.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                        SQL = SQL + ComNum.VBLF + " (SELECT * FROM KOSMOS_PMPA.NUR_TEAM_ROOMCODE T";
                        SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                        SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                        SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + strTeam + "')";
                    }
                    SQL += ComNum.VBLF + "   AND ROOMCODE = '" + dt.Rows[i]["ROOMCODE"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "   AND ROWNUM < 6 ";
                    if (rbtn3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "   AND PANO IN (" + strTemp + ")";
                    }
                    SQL += ComNum.VBLF + "  ORDER BY ROOMCODE ASC ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    for (j = 0; j < dt2.Rows.Count; j++)
                    {
                        ssS_Sheet1.Cells[(i + 1) * 5 - 4, (j + 1) * 3 - 1].Text = dt2.Rows[j]["TOP"].ToString().Trim() + ComNum.VBLF + dt2.Rows[j]["bottom"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;

                }
            }

            dt.Dispose();
            dt = null;

            SetFooter(ssS_Sheet1.RowCount);
        }
               
        void ePrint()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            //string strFoot = "";
            string SysDate = "";

            SmartPrintRulesCollection prules = new SmartPrintRulesCollection();

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C " + cboWard.Text.Trim() + " 상주 간병인 및 보호자 건강 모니터링 점검표(" + clsPublic.GstrSysDate + ")";
            strHead2 = "/n " + "*부서책임자는 점검기록지를 점검월로 부터 1년 간 보관하며, 코로나19 방역관련 외부 점검 시 제시할 수 있도록 합니다.";

            ssS_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssS_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2 + ComNum.VBLF;
            ssS_Sheet1.PrintInfo.Margin.Left = 35;
            ssS_Sheet1.PrintInfo.Margin.Right = 0;
            ssS_Sheet1.PrintInfo.Margin.Top = 35;
            ssS_Sheet1.PrintInfo.Margin.Bottom = 30;
            //ssS_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            //ssS_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssS_Sheet1.PrintInfo.ShowBorder = true;
            ssS_Sheet1.PrintInfo.ShowColor = false;
            ssS_Sheet1.PrintInfo.ShowGrid = true;
            ssS_Sheet1.PrintInfo.ShowShadows = false;
            ssS_Sheet1.PrintInfo.UseMax = false;
            ssS_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;

            //ssS_Sheet1.PrintInfo.ZoomFactor = 0.5f;

            ssS.PrintSheet(0);

        }


        private void SetRowDesign(int i)
        {

            #region 스프레드 스타일 정의
            FarPoint.Win.ComplexBorder complexBorder86 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder87 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder88 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder89 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder90 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder91 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder92 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder93 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder94 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder95 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder96 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder97 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder98 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder99 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder100 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder101 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder102 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder103 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder104 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder105 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder106 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder107 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder108 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder109 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder110 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder111 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder112 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder113 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder114 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder115 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder116 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder117 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder118 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder119 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder120 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder121 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder122 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder123 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder124 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder125 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder126 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder127 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder128 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder129 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder130 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder131 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder132 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder133 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder134 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder135 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder136 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder137 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder138 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder139 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder140 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder141 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder142 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder143 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder144 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder145 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder146 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder147 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder148 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder149 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder150 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder151 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder152 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder153 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder154 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder155 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder156 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder157 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder158 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder159 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder160 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder161 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder162 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder163 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder164 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder165 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder166 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder167 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder168 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder169 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder170 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType21 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType22 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            #endregion


            if (i == 0)
            {
                ssS_Sheet1.RowCount = 0;
                return;
            }

            //ssS_Sheet1.RowCount = i * 5;

            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 0).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 0).Border = complexBorder86;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 0).Value = "병실";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 1).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 1).Border = complexBorder87;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 1).Value = "구분";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 2).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 2).Border = complexBorder88;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 2).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 2).Value = "환자1";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 3).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 3).Border = complexBorder89;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 4).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 4).Border = complexBorder90;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 5).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 5).Border = complexBorder91;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 5).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 5).Value = "환자2";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 6).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 6).Border = complexBorder92;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 7).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 7).Border = complexBorder93;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 8).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 8).Border = complexBorder94;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 8).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 8).Value = "환자3";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 9).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 9).Border = complexBorder95;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 10).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 10).Border = complexBorder96;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 11).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 11).Border = complexBorder97;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 11).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 11).Value = "환자4";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 12).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 12).Border = complexBorder98;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 13).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 13).Border = complexBorder99;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 14).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 14).Border = complexBorder100;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 14).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 14).Value = "환자5";
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 15).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 15).Border = complexBorder101;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 16).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 16).Border = complexBorder102;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 5, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 0).Border = complexBorder103;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 0).RowSpan = 4;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 0).Value = "751";
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 1).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 1).Border = complexBorder104;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 1).Value = "환자정보";
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 2).Border = complexBorder105;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 2).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 3).Border = complexBorder106;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 4).Border = complexBorder107;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 5).Border = complexBorder108;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 5).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 6).Border = complexBorder109;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 7).Border = complexBorder110;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 8).Border = complexBorder111;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 8).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 9).Border = complexBorder112;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 10).Border = complexBorder113;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 11).Border = complexBorder114;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 11).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 12).Border = complexBorder115;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 13).Border = complexBorder116;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 14).Border = complexBorder117;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 14).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 15).Border = complexBorder118;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 16).Border = complexBorder119;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 4, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 0).Border = complexBorder120;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 1).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 1).Border = complexBorder121;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 1).Value = "상주보호자성명";
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 2).Border = complexBorder122;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 2).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 3).Border = complexBorder123;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 4).Border = complexBorder124;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 5).Border = complexBorder125;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 5).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 6).Border = complexBorder126;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 7).Border = complexBorder127;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 8).Border = complexBorder128;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 8).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 9).Border = complexBorder129;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 10).Border = complexBorder130;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 11).Border = complexBorder131;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 11).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 12).Border = complexBorder132;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 13).Border = complexBorder133;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 14).Border = complexBorder134;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 14).ColumnSpan = 3;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 15).Border = complexBorder135;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 16).Border = complexBorder136;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 3, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 0).Border = complexBorder137;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 1).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 1).Border = complexBorder138;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 1).RowSpan = 2;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 1).Value = "점검내용";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 2).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 2).Border = complexBorder139;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 2).Value = "발열";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 3).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 3).Border = complexBorder140;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 3).Value = "증상";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 4).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 4).Border = complexBorder141;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 4).Value = "특이사항";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 5).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 5).Border = complexBorder142;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 5).Value = "발열";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 6).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 6).Border = complexBorder143;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 6).Value = "증상";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 7).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 7).Border = complexBorder144;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 7).Value = "특이사항";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 8).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 8).Border = complexBorder145;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 8).Value = "발열";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 9).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 9).Border = complexBorder146;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 9).Value = "증상";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 10).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 10).Border = complexBorder147;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 10).Value = "특이사항";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 11).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 11).Border = complexBorder148;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 11).Value = "발열";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 12).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 12).Border = complexBorder149;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 12).Value = "증상";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 13).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 13).Border = complexBorder150;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 13).Value = "특이사항";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 14).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 14).Border = complexBorder151;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 14).Value = "발열";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 15).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 15).Border = complexBorder152;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 15).Value = "증상";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 16).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 16).Border = complexBorder153;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 16).Value = "특이사항";
            this.ssS_Sheet1.Cells.Get((i * 5) - 2, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 0).Border = complexBorder154;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 1).BackColor = System.Drawing.Color.AliceBlue;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 1).Border = complexBorder155;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 2).Border = complexBorder156;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 3).Border = complexBorder157;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 4).Border = complexBorder158;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 5).Border = complexBorder159;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 6).Border = complexBorder160;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 7).Border = complexBorder161;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 8).Border = complexBorder162;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 9).Border = complexBorder163;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 10).Border = complexBorder164;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 11).Border = complexBorder165;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 12).Border = complexBorder166;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 13).Border = complexBorder167;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 14).Border = complexBorder168;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 15).Border = complexBorder169;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 16).Border = complexBorder170;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get((i * 5) - 1, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            this.ssS_Sheet1.Rows.Get((i * 5) - 5).Height = 23F;
            textCellType17.Multiline = true;
            textCellType17.WordWrap = true;
            this.ssS_Sheet1.Rows.Get((i * 5) - 4).CellType = textCellType17;
            this.ssS_Sheet1.Rows.Get((i * 5) - 4).Height = 40F;
            this.ssS_Sheet1.Rows.Get((i * 5) - 3).Height = 20F;
            this.ssS_Sheet1.Rows.Get((i * 5) - 2).Height = 23F;
            this.ssS_Sheet1.Rows.Get((i * 5) - 1).Height = 22F;

        }

        private void SetFooter(int i)
        {
            //i 스프레드 행 갯수

            #region 스프레드 스타일 정의
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder5 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder6 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder7 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder9 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder10 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder11 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder12 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder13 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder14 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder15 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder16 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder17 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder18 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder19 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder20 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder21 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder22 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder23 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder24 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder25 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder26 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder27 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder28 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder29 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder30 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder31 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder32 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder33 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder34 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder35 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder36 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder37 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.ComplexBorder complexBorder38 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder39 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder40 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder41 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder42 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder43 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder44 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder45 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder46 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder47 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder48 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder49 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder50 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder51 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder52 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.ComplexBorder complexBorder53 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None));
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NoPrinterPrintInfo noPrinterPrintInfo1 = new FarPoint.Win.Spread.NoPrinterPrintInfo();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType(); 
            #endregion

            this.ssS_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssS_Sheet1.RowCount = i + 5;

            this.ssS_Sheet1.Cells.Get(i, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 0).Border = complexBorder1;
            this.ssS_Sheet1.Cells.Get(i, 0).ColumnSpan = 2;
            this.ssS_Sheet1.Cells.Get(i, 0).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 0).Value = "점검방법";
            this.ssS_Sheet1.Cells.Get(i, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 1).Border = complexBorder2;
            this.ssS_Sheet1.Cells.Get(i, 1).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 1).Value = "구분";
            this.ssS_Sheet1.Cells.Get(i, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 2).Border = complexBorder3;
            this.ssS_Sheet1.Cells.Get(i, 2).ColumnSpan = 15;
            this.ssS_Sheet1.Cells.Get(i, 2).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i, 2).Value = " 일정 시간에 확인 후 기록";
            this.ssS_Sheet1.Cells.Get(i, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 3).Border = complexBorder4;
            this.ssS_Sheet1.Cells.Get(i, 3).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 4).Border = complexBorder5;
            this.ssS_Sheet1.Cells.Get(i, 4).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 5).Border = complexBorder6;
            this.ssS_Sheet1.Cells.Get(i, 5).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 6).Border = complexBorder7;
            this.ssS_Sheet1.Cells.Get(i, 6).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 7).Border = complexBorder8;
            this.ssS_Sheet1.Cells.Get(i, 7).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 8).Border = complexBorder9;
            this.ssS_Sheet1.Cells.Get(i, 8).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 9).Border = complexBorder10;
            this.ssS_Sheet1.Cells.Get(i, 9).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 10).Border = complexBorder11;
            this.ssS_Sheet1.Cells.Get(i, 10).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 11).Border = complexBorder12;
            this.ssS_Sheet1.Cells.Get(i, 11).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 12).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 12).Border = complexBorder13;
            this.ssS_Sheet1.Cells.Get(i, 12).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 13).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 13).Border = complexBorder14;
            this.ssS_Sheet1.Cells.Get(i, 13).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 14).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 14).Border = complexBorder15;
            this.ssS_Sheet1.Cells.Get(i, 14).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 15).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 15).Border = complexBorder16;
            this.ssS_Sheet1.Cells.Get(i, 15).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 16).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i, 16).Border = complexBorder17;
            this.ssS_Sheet1.Cells.Get(i, 16).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 0).Border = complexBorder18;
            textCellType1.Multiline = true;
            textCellType1.WordWrap = true;
            this.ssS_Sheet1.Cells.Get(i + 1, 0).CellType = textCellType1;
            this.ssS_Sheet1.Cells.Get(i + 1, 0).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 0).RowSpan = 3;
            this.ssS_Sheet1.Cells.Get(i + 1, 0).Value = "점검\r\n내용";
            this.ssS_Sheet1.Cells.Get(i + 1, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 1).Border = complexBorder19;
            this.ssS_Sheet1.Cells.Get(i + 1, 1).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 1).Value = "발열";
            this.ssS_Sheet1.Cells.Get(i + 1, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 2).Border = complexBorder20;
            this.ssS_Sheet1.Cells.Get(i + 1, 2).ColumnSpan = 13;
            this.ssS_Sheet1.Cells.Get(i + 1, 2).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 2).Value = " 유 : 발열(37.5℃ 이상) 시 직접 기록";
            this.ssS_Sheet1.Cells.Get(i + 1, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 3).Border = complexBorder21;
            this.ssS_Sheet1.Cells.Get(i + 1, 3).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 4).Border = complexBorder22;
            this.ssS_Sheet1.Cells.Get(i + 1, 4).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 5).Border = complexBorder23;
            this.ssS_Sheet1.Cells.Get(i + 1, 5).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 6).Border = complexBorder24;
            this.ssS_Sheet1.Cells.Get(i + 1, 6).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 7).Border = complexBorder25;
            this.ssS_Sheet1.Cells.Get(i + 1, 7).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 8).Border = complexBorder26;
            this.ssS_Sheet1.Cells.Get(i + 1, 8).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 9).Border = complexBorder27;
            this.ssS_Sheet1.Cells.Get(i + 1, 9).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 10).Border = complexBorder28;
            this.ssS_Sheet1.Cells.Get(i + 1, 10).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 11).Border = complexBorder29;
            this.ssS_Sheet1.Cells.Get(i + 1, 11).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 12).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 12).Border = complexBorder30;
            this.ssS_Sheet1.Cells.Get(i + 1, 12).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 13).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 13).Border = complexBorder31;
            this.ssS_Sheet1.Cells.Get(i + 1, 13).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 14).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 14).Border = complexBorder32;
            this.ssS_Sheet1.Cells.Get(i + 1, 14).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 15).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 15).Border = complexBorder33;
            this.ssS_Sheet1.Cells.Get(i + 1, 15).ColumnSpan = 2;
            this.ssS_Sheet1.Cells.Get(i + 1, 15).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 15).Value = " 무 : 없음";
            this.ssS_Sheet1.Cells.Get(i + 1, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 1, 16).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 1, 16).Border = complexBorder34;
            this.ssS_Sheet1.Cells.Get(i + 1, 16).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 1, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 1, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 0).Border = complexBorder35;
            this.ssS_Sheet1.Cells.Get(i + 2, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 1).Border = complexBorder36;
            this.ssS_Sheet1.Cells.Get(i + 2, 1).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 1).Value = "발열이외 증상";
            this.ssS_Sheet1.Cells.Get(i + 2, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 2).Border = complexBorder37;
            textCellType2.Multiline = true;
            textCellType2.WordWrap = true;
            this.ssS_Sheet1.Cells.Get(i + 2, 2).CellType = textCellType2;
            this.ssS_Sheet1.Cells.Get(i + 2, 2).ColumnSpan = 13;
            this.ssS_Sheet1.Cells.Get(i + 2, 2).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 2).Value = " 유 : ①기침 ②호흡곤란 ③오한 ④근육통 ⑤두통 ⑥인후통 ⑦후각,미각손실 \r\n       ⑧소화기증상(오심,구토,설사) ⑨콧물, 코막힘 등";
            this.ssS_Sheet1.Cells.Get(i + 2, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 3).Border = complexBorder38;
            this.ssS_Sheet1.Cells.Get(i + 2, 3).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 4).Border = complexBorder39;
            this.ssS_Sheet1.Cells.Get(i + 2, 4).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 5).Border = complexBorder40;
            this.ssS_Sheet1.Cells.Get(i + 2, 5).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 6).Border = complexBorder41;
            this.ssS_Sheet1.Cells.Get(i + 2, 6).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 7).Border = complexBorder42;
            this.ssS_Sheet1.Cells.Get(i + 2, 7).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 8).Border = complexBorder43;
            this.ssS_Sheet1.Cells.Get(i + 2, 8).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 9).Border = complexBorder44;
            this.ssS_Sheet1.Cells.Get(i + 2, 9).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 10).Border = complexBorder45;
            this.ssS_Sheet1.Cells.Get(i + 2, 10).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 11).Border = complexBorder46;
            this.ssS_Sheet1.Cells.Get(i + 2, 11).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 12).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 12).Border = complexBorder47;
            this.ssS_Sheet1.Cells.Get(i + 2, 12).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 13).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 13).Border = complexBorder48;
            this.ssS_Sheet1.Cells.Get(i + 2, 13).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 14).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 14).Border = complexBorder49;
            this.ssS_Sheet1.Cells.Get(i + 2, 14).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 15).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 15).Border = complexBorder50;
            this.ssS_Sheet1.Cells.Get(i + 2, 15).ColumnSpan = 2;
            this.ssS_Sheet1.Cells.Get(i + 2, 15).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 15).Value = " 무 : 없음";
            this.ssS_Sheet1.Cells.Get(i + 2, 15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 2, 16).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 2, 16).Border = complexBorder51;
            this.ssS_Sheet1.Cells.Get(i + 2, 16).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 2, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 2, 16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 3, 0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 0).Border = complexBorder52;
            this.ssS_Sheet1.Cells.Get(i + 3, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 3, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 3, 1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 1).Border = complexBorder53;
            this.ssS_Sheet1.Cells.Get(i + 3, 1).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 3, 1).Value = "특이사항";
            this.ssS_Sheet1.Cells.Get(i + 3, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Cells.Get(i + 3, 2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 2).ColumnSpan = 13;
            this.ssS_Sheet1.Cells.Get(i + 3, 2).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 2).Value = " 유 : ①최근 2주 이내 해외 여행력 ②최근 2주 이내 확진자 접촉력";
            this.ssS_Sheet1.Cells.Get(i + 3, 3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 3).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 4).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 4).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 5).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 5).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 6).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 7).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 7).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 8).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 8).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 9).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 9).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 10).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 11).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 11).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 12).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 12).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 13).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 13).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 14).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 14).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 15).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 15).ColumnSpan = 2;
            this.ssS_Sheet1.Cells.Get(i + 3, 15).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Cells.Get(i + 3, 15).Value = " 무 : 없음";
            this.ssS_Sheet1.Cells.Get(i + 3, 16).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssS_Sheet1.Cells.Get(i + 3, 16).Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 3, 16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssS_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.ssS_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(0).Label = "병실";
            this.ssS_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(0).Width = 39F;
            this.ssS_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(1).Width = 95F;
            this.ssS_Sheet1.Columns.Get(2).CellType = textCellType4;
            this.ssS_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(2).Label = "구분";
            this.ssS_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(2).Width = 35F;
            this.ssS_Sheet1.Columns.Get(3).CellType = textCellType5;
            this.ssS_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(3).Label = "환자1";
            this.ssS_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(3).Width = 35F;
            this.ssS_Sheet1.Columns.Get(4).CellType = textCellType6;
            this.ssS_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(4).Width = 55F;
            this.ssS_Sheet1.Columns.Get(5).CellType = textCellType7;
            this.ssS_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(5).Width = 35F;
            this.ssS_Sheet1.Columns.Get(6).CellType = textCellType8;
            this.ssS_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(6).Width = 35F;
            this.ssS_Sheet1.Columns.Get(7).CellType = textCellType9;
            this.ssS_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(7).Width = 55F;
            this.ssS_Sheet1.Columns.Get(8).CellType = textCellType10;
            this.ssS_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(8).Width = 35F;
            this.ssS_Sheet1.Columns.Get(9).CellType = textCellType11;
            this.ssS_Sheet1.Columns.Get(9).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(9).Width = 35F;
            this.ssS_Sheet1.Columns.Get(10).CellType = textCellType12;
            this.ssS_Sheet1.Columns.Get(10).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(10).Width = 55F;
            this.ssS_Sheet1.Columns.Get(11).CellType = textCellType13;
            this.ssS_Sheet1.Columns.Get(11).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(11).Width = 35F;
            this.ssS_Sheet1.Columns.Get(12).CellType = textCellType14;
            this.ssS_Sheet1.Columns.Get(12).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(12).Width = 35F;
            this.ssS_Sheet1.Columns.Get(13).CellType = textCellType15;
            this.ssS_Sheet1.Columns.Get(13).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(13).Width = 55F;
            this.ssS_Sheet1.Columns.Get(14).CellType = textCellType16;
            this.ssS_Sheet1.Columns.Get(14).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(14).Width = 35F;
            this.ssS_Sheet1.Columns.Get(15).CellType = textCellType17;
            this.ssS_Sheet1.Columns.Get(15).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(15).Width = 35F;
            this.ssS_Sheet1.Columns.Get(16).CellType = textCellType18;
            this.ssS_Sheet1.Columns.Get(16).Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.ssS_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssS_Sheet1.Columns.Get(16).Width = 55F;
            this.ssS_Sheet1.Rows.Get(i).Height = 29F;
            textCellType19.Multiline = true;
            textCellType19.WordWrap = true;
            this.ssS_Sheet1.Rows.Get(i + 1).CellType = textCellType19;
            this.ssS_Sheet1.Rows.Get(i + 1).Height = 28F;
            this.ssS_Sheet1.Rows.Get(i + 2).Height = 41F;
            this.ssS_Sheet1.Rows.Get(i + 3).Height = 28F;
            this.ssS_Sheet1.Rows.Get(i + 4).Height = 47F;
            this.ssS_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;

            this.ssS_Sheet1.Cells.Get(i + 4, 0).ColumnSpan = 17;
            this.ssS_Sheet1.Cells.Get(i + 4, 0).Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.ssS_Sheet1.Cells.Get(i + 4, 0).ForeColor = System.Drawing.Color.Black;
            this.ssS_Sheet1.Cells.Get(i + 4, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssS_Sheet1.Cells.Get(i + 4, 0).Value = "부서 책임자 혹은 수(책임) 간호사 확인 :                                        ";

        }

        private void rbtn1_CheckedChanged(object sender, EventArgs e)
        {
            ssList_Sheet1.RowCount = 0;
        }

        private void rbtn2_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "SELECT ROOMCODE   ";
                SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE (JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "          OR OUTDATE = TRUNC(SYSDATE)) ";
                if (cboWard.Text.Trim() == "NR")
                {
                    SQL += ComNum.VBLF + "    AND WARDCODE IN ('NR','IQ')";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND WARDCODE = '" + cboWard.Text.Trim() + " ' ";
                }
                
                SQL += ComNum.VBLF + "   GROUP BY ROOMCODE ";
                SQL += ComNum.VBLF + "   ORDER BY ROOMCODE ASC ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
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

        private void rbtn3_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "SELECT ROOMCODE, PANO || '/' || SNAME PTNO ";
                SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE (JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "          OR OUTDATE = TRUNC(SYSDATE)) ";
                if (cboWard.Text.Trim() == "NR")
                {
                    SQL += ComNum.VBLF + "    AND WARDCODE IN ('NR','IQ')";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND WARDCODE = '" + cboWard.Text.Trim() + " ' ";
                }
                SQL += ComNum.VBLF + "   ORDER BY ROOMCODE ASC, SNAME ASC ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
