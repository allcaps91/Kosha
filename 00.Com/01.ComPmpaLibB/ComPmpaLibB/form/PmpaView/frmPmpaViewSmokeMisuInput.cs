using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSmokeMisuInput.cs
    /// Description     : 금연처방미수 일괄입금 처리
    /// Author          : 김해수
    /// Create Date     : 2018-11-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= "\misu\Frm금연처방미수일괄입금.frm(Frm금연처방미수일괄입금) >> frmPmpaViewSmokeMisuInput.cs 폼이름 재정의" />
    /// 
    public partial class frmPmpaViewSmokeMisuInput : Form
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();
        clsComPmpaSQL cSQL = new clsComPmpaSQL();
        clsComPmpaSpd MagamSpd = new clsComPmpaSpd();
        clsPmpaMisu cPM = new clsPmpaMisu();
        clsPmpaFunc cPF = new clsPmpaFunc();
        int Chk = 0;

        #endregion

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

        public frmPmpaViewSmokeMisuInput()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSmokeMisuInput(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSave);

            ////명단 더블클릭 이벤트
            this.ssList1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList1.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
        
            this.cboGbn.SelectedIndexChanged += new EventHandler(eCtl_SelectedIndexChanged);


        }

        void setCombo()
        {

        }

        void setTxtTip()
        {

        }

        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //설정세팅
            //}
        }

        private void eCtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == this.cboGbn)
            {
                if (cboGbn.SelectedIndex == 0)
                {
                    txtRemark.Text = "필수예방접종(입금)";
                    TxtM.Visible = false;
                }
                else if(cboGbn.SelectedIndex == 1 )
                {
                    txtRemark.Text = "회사접종(입금)";
                    TxtM.Visible = true;

                }
                else if (cboGbn.SelectedIndex == 2)
                {
                    txtRemark.Text = "금연처방(입금)";
                    TxtM.Visible = false;
                }
            }

          
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
                MagamSpd.sSpd_PmpaViewSmokeMisuInput(ssList1, MagamSpd.senmPmpaViewSmokeMisuInput, MagamSpd.nenmPmpaViewSmokeMisuInput, 1, 0);

                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();
            }

            cboGbn.Items.Clear();
            cboGbn.Items.Add("13.필수예방접종");
            cboGbn.Items.Add("14.회사예방접종");
            cboGbn.Items.Add("15.금연처방");
            cboGbn.SelectedIndex = 0;
            txtRemark.Text = "필수예방접종(입금)";
        }

        void eFormResize(object sender, EventArgs e)
        {
            
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

        void eSpdChange(object sender, EventArgs e)
        {
            
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 1) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right) return; 

            if (sender == this.ssList1)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            string strChk = "";
            int i = 0;

            if(Chk == 0)
            {
                if (e.Row == 0 && e.Column == 0)
                {
                    for (i = 0; i < ssList1.ActiveSheet.RowCount - 1; i++)
                    {
                        strChk = ssList1.ActiveSheet.Cells[i, 0].Text;

                        if (strChk == "1" || strChk == "Ture")
                        {

                        }
                        else
                        {
                            ssList1.ActiveSheet.Cells[i, 0].Text = "1";
                        }
                    }
                }
                Chk = 1;
            }
            else if(Chk ==1)
            {
                if (e.Row == 0 && e.Column == 0)
                {
                    for (i = 0; i < ssList1.ActiveSheet.RowCount - 1; i++)
                    {
                        strChk = ssList1.ActiveSheet.Cells[i, 0].Text;

                        if (strChk == "" || strChk == "False")
                        {

                        }
                        else
                        {
                            ssList1.ActiveSheet.Cells[i, 0].Text = "";
                        }
                    }
                }
                Chk = 0;
            }

            ChkCell(e.Column);
            sumAMT();
        }

        void eSpreadButtonClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            //체크이외의 배경색 흰색으로 변경
            ChkCell(e.Column);

            //선택 셀 배경색 변경
            if (e.Column == (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk)
            {
                if (ssList1.ActiveSheet.Cells[e.Row, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk].Text == "True")
                {
                    ssList1_Sheet1.Rows.Get(e.Row).BackColor = System.Drawing.Color.FromArgb(183, 240, 177);
                }
                else
                {
                    ssList1_Sheet1.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
                }
            }
            //선택된 합계 계산후 띄워주기
            sumAMT();
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
                screen_clear("Clear");
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon,ssList1);
            }
        }

        void screen_clear(string Job = "")
        {
            if (Job == "")
            {
                lblP_Sel.Text = "";
                //txtRemark.Text = "금연처방(입금)";

                //dtpDate.Text = VB.Left(cpublic.strSysDate, 8) + "01";
                dtpDate.Text = clsPublic.GstrSysDate;
                dtpDate.Text = VB.Left(dtpDate.Text, 8) + "01";
                dtpDate1.Text = clsPublic.GstrSysDate;
                ssList1.ActiveSheet.RowCount = 0;
                chk1.Visible = false;

                ////스프레드 범위 클리어
                //CS.Spread_Clear_Range(ssList1, 0, 0, ssList1.ActiveSheet.RowCount, ssList1.ActiveSheet.ColumnCount);
            }
            else if (Job == "Clear")
            {
                ssList1.ActiveSheet.RowCount = 0;
                // 화면상의 정렬표시 Clear ( Sort 정렬 아이콘 클리어)
                ssList1.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList1.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }
        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList1);
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            DataTable dt = null;
            int i = 0;
            long nTotAmt = 0;
            int check = 0;
            int nRow = 0;

            if(chk2.Checked == true)
            {
                check = 1;
            }
            if (TxtM.Text.Trim() != "" && VB.Left(cboGbn.Text, 2) =="14" )
            {
                check = 1;
            }
            
            lblP_Sel.Text = "";

            dt = cSQL.sel_SmokemisuInput_1(pDbCon, dtpDate.Text, dtpDate1.Text, check, VB.Left(cboGbn.Text,2), TxtM.Text.Trim());

            FarPoint.Win.Spread.CellType.TextCellType TCT = new FarPoint.Win.Spread.CellType.TextCellType();

            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow += 1;
                    if (nRow > Spd.ActiveSheet.RowCount)
                    {
                        Spd.ActiveSheet.RowCount = nRow;
                    }

                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Pano].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.BDate].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.GUBUN1].Text = dt.Rows[i]["GUBUN1"].ToString().Trim() + "." + READ_Misu_DTLGBN("A", dt.Rows[i]["GUBUN1"].ToString().Trim());
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.GUBUN2].Text = dt.Rows[i]["GUBUN2"].ToString().Trim() + "." + READ_Misu_DTLGBN("B", dt.Rows[i]["GUBUN2"].ToString().Trim());
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.OPDIPD].Text = dt.Rows[i]["OPDIPD"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.DEPTCODE].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.MisuDTL22].Text = dt.Rows[i]["MISUDTL22"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.AMT].Text = VB.Format(Convert.ToInt32(VB.Val(dt.Rows[i]["AMT"].ToString().Trim())), "###,###,##0");
                    nTotAmt = nTotAmt + Convert.ToInt32(VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.MisuDTL].Text = dt.Rows[i]["MISUDTL"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.GUBUN_1].Text = dt.Rows[i]["GUBUN1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.GUBUN_2].Text = dt.Rows[i]["PoBun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    if (VB.Left(cboGbn.Text, 2) == "14")
                    {
                        Spd.ActiveSheet.Cells[nRow - 1, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();

                    }


                    if (dt.Rows[i]["GUBUN1"].ToString().Trim() == "2")
                    {
                        CS.setSpdCellColor(ssList1, i, 0, i, 9, System.Drawing.Color.FromArgb(255, 255, 128));


                        //Define ceslls as type STARTIC

                        Spd.ActiveSheet.Cells[nRow - 1, 0].CellType = TCT;
                        Spd.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    }

                    if (dt.Rows[i]["Chk"].ToString().Trim() == "1")
                    {
                        CS.setSpdCellColor(ssList1, i, 0, i, 9, System.Drawing.Color.FromArgb(128, 255, 128));

                        Spd.ActiveSheet.Cells[nRow - 1, 0].CellType = TCT;
                        Spd.ActiveSheet.Cells[nRow - 1, 0].Text = "";
                    }
                }
            }

            nRow += 1;
            Spd.ActiveSheet.RowCount = nRow;
            Spd.ActiveSheet.Cells[nRow -1,0].CellType = TCT;
            Spd.ActiveSheet.Cells[nRow -1,0].Text = "";
            Spd.ActiveSheet.Cells[nRow -1,7].Text = "금액합";
            Spd.ActiveSheet.Cells[nRow -1,9].Text = VB.Format(nTotAmt, "###,###,##0");

            dt.Dispose();
            dt = null;
        }

        void eSave(PsmhDb pDbCon,FpSpread spd)
        {
            int i = 0;
            string strPano = "", strPobun = "";
            string strRemark = "", strROWID = "", strDept = "", strIO = "", strBuse = "", strMisuGbn = "";
            double nAmt = 0;

            if (ComFunc.MsgBoxQ("선택한 것을 일괄입금 작업을 하시겠습니까?","확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            strRemark = txtRemark.Text.Trim();

            for (i = 0; i < spd.ActiveSheet.RowCount; i++)
            {
                if(spd.ActiveSheet.Cells[i,(int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk].Text == "True")
                {
                    strPano = spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Pano].Text;
                    strIO = spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.OPDIPD].Text;
                    strDept = spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.DEPTCODE].Text;
                    strMisuGbn = VB.Left(spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.MisuDTL22].Text, 2);
                    nAmt = VB.Val(VB.Replace(spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.AMT].Text,",",""));
                    strBuse = VB.Left(spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.GUBUN_2].Text, 1);
                    strPobun = spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.PoBun].Text;

                    strROWID = spd.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.ROWID].Text;

                    Misu_Mst_Insert_SEL(pDbCon, strIO, strPano, strDept, nAmt, strBuse, strMisuGbn, strPobun, strRemark, strROWID);
                }
            }
            //btnSearch Click
            screen_clear("Clear");
            screen_display();

        }

        string READ_Misu_DTLGBN(string ArgJob, string ArgCode)
        {
           string rtnVal = "";

            if(ArgJob == "A")
            {
                switch (ArgCode)
                {
                    case "1":
                        rtnVal = "발생";
                        break;
                    case "2":
                        rtnVal = "입금";
                        break;
                    case "3":
                        rtnVal = "반송";
                        break;
                    case "4":
                        rtnVal = "감액";
                        break;
                    case "5":
                        rtnVal = "삭감";
                        break;
                    case "9":
                        rtnVal = "참고";
                        break;
                }
            }else if(ArgJob == "B")
            {
                switch (ArgCode)
                {
                    case "1":
                        rtnVal = "외래";
                        break;
                    case "2":
                        rtnVal = "응급실";
                        break;
                    case "3":
                        rtnVal = "입원";
                        break;
                    case "4":
                        rtnVal = "심사계";
                        break;
                    case "5":
                        rtnVal = "원무과";
                        break;
                }
            }else if(ArgJob == "C")
            {
                switch (ArgCode)
                {
                    case "01":
                        rtnVal = "가퇴원미수";
                        break;
                    case "02":
                        rtnVal = "업무착오미수";
                        break;
                    case "03":
                        rtnVal = "탈원미수";
                        break;
                    case "04":
                        rtnVal = "지불각서";
                        break;
                    case "05":
                        rtnVal = "응급미수";
                        break;
                    case "06":
                        rtnVal = "외래미수";
                        break;
                    case "07":
                        rtnVal = "심사청구미수";
                        break;
                    case "10":
                        rtnVal = "기타";
                        break;
                    case "11":
                        rtnVal = "기관미수";
                        break;
                    case "12":
                        rtnVal = "입원정밀";
                        break;
                    case "13":
                        rtnVal = "필수예방접종";
                        break;
                }
            }

            return rtnVal;


        }

        void sumAMT()
        {
            long nTotAmt = 0;
            int nSCnt = 0;

            for (int i = 0; i < ssList1.ActiveSheet.RowCount - 1; i++)
            {

                if (ssList1.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk].Text == "True" || ssList1.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk].Text == "1")
                {                    
                    nTotAmt += Convert.ToInt32(VB.Replace(VB.TR(ssList1.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.AMT].Text, ",", ""),",",""));
                    nSCnt = nSCnt + 1;
                }
            }

            lblP_Sel.Text = nSCnt + "건 선택금액 합 : " + VB.Format(nTotAmt, "###,###,##0");
        }

        void ChkCell(int selCol)
        {
            int i = 0;

            for (i = 0; i < ssList1.ActiveSheet.RowCount; i++)
            {
                if (ssList1.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk].Text == "True" || ssList1.ActiveSheet.Cells[i, (int)clsComPmpaSpd.PmpaViewSmokeMisuInput.Chk].Text == "1")
                {
                    CS.setSpdCellColor(ssList1, i, 0, i, 9, System.Drawing.Color.FromArgb(183, 240, 177));
                }
                else
                {
                    CS.setSpdCellColor(ssList1, i, 0, i, 9, System.Drawing.Color.White);
                }
            }
        }

        void Misu_Mst_Insert_SEL(PsmhDb pDbCon, string ArgIO, string ArgPano, string ArgDept, double ArgAmt, string ArgBuse, string ArgMisuGbn, string ArgPoBun, string ArgRemark, string ArgRowid)
        {
            string SqlErr = ""; //에러문 받는 변수
            string SQL = "";
            int intRowAffected = 0;

            double nAmt1 = 0, nAmt2 = 0;
            string strMisu = "", strROWID = "", strPobun = "";

            DataTable dt = null;

            dt = cSQL.sel_SmokemisuInput_2(pDbCon, ArgPano);

            if (dt.Rows.Count == 1)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                nAmt1 = Convert.ToInt32(VB.Val(dt.Rows[0]["MAmt"].ToString().Trim()));
                nAmt2 = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt"].ToString().Trim()));
            }
            else
            {
                strROWID = "";
                nAmt1 = 0;
                nAmt2 = 0;
            }

            dt.Dispose();
            dt = null;

            nAmt2 = nAmt2 + ArgAmt;

            strPobun = ArgPoBun;

            clsDB.setBeginTran(pDbCon);

            try
            {
                if (ArgRowid != "")//기존발생 미수 처리표시
                {
                    SqlErr = cSQL.udt_SmokeMisuUpdate_1(pDbCon, ArgRowid, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("개인별 미수마스터 등록 오류발생!!", "RollBack");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }

                SqlErr = cSQL.udt_SmokeMisuUpdate_2(pDbCon, ArgPano, nAmt1, nAmt2, strROWID, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("개인별 미수마스터 등록 오류발생!!", "RollBack");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
                   
                //Misu_Dtl_Data_Insert_SEL
                strMisu = "";
                strMisu = ArgIO.Trim();
                strMisu = strMisu + VB.Left(ArgDept.Trim() + "  ", 2);
                strMisu = strMisu + VB.Left(ArgMisuGbn.Trim() + "  ", 2);
                strMisu = strMisu + "000000000";
                strMisu = strMisu + VB.Left("" + VB.Space(8), 8);
                strMisu = strMisu + VB.Left("" + VB.Space(8), 8);

                  
                SqlErr = cSQL.Int_SmokeMisuInsert_1(pDbCon, ArgPano, ArgBuse, ArgAmt, ArgRemark, strMisu, strPobun, clsPublic.GstrSysDate, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    ComFunc.MsgBox("Table Misu_GAINSLIP 입력 Error !", "RollBack");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                //Misu_Dtl_Data_Update_SEL
                if (nAmt1 - nAmt2 == 0)
                {
                    SqlErr = cSQL.udt_SmokeMisuUpdate_3(pDbCon, ArgPano, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        ComFunc.MsgBox("Table MISU_GAINSLIP Update Error !", "RollBack");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }

                clsDB.setCommitTran(pDbCon);
        
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

      
    }
}