using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.SupXray;
using System.Threading;
using FarPoint.Win.Spread;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupNoExecute2.cs
    /// Description     : 심사용 미시행 검사 조회 및 처리 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-06-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 miretc04.frm(FrmNoExecute2) 폼 frmComSupNoExecute2.cs 으로 변경함
    /// </history>
    /// <seealso cref= "mir\miretc\miretc04.frm >> frmComSupNoExecute2.cs 폼이름 재정의" />
    public partial class frmComSupNoExecute2 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsComSupSpd csupspd = new clsComSupSpd();
        clsSpread cSpd = new clsSpread();
        clsComSupXray cxray = new clsComSupXray();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsComSupSpd.sPatInfo pInfo = new clsComSupSpd.sPatInfo(); //시트선택시 환자정보
        string[] argsql = null; //쿼리에 사용될 변수 배열값

        string gstrER = string.Empty;
        string gstrPano = string.Empty;
        string strJohap = "0";
        string strChkExam = "0";

        int nRow = 0;
        FarPoint.Win.Spread.FpSpread spd;
        Thread thread;

        #endregion

        enum enmsql { Pano, FDate, TDate, DeptCode, DrCode,ChkStation, ChkExam1, ChkExam2, ChkExam3, ChkExam8, ChkExam9, ChkExam10, ChkER };

        public frmComSupNoExecute2()
        {
            InitializeComponent();

            setEvent();

        }

        public frmComSupNoExecute2(string strPano)
        {
            InitializeComponent();

            gstrPano = strPano;

            setEvent();
        }

        void setCtrlData(PsmhDb pDbCon)
        {
            txtPano.Text = gstrPano;

            read_sysdate();

            dtpFDate.Text = VB.DateAdd("d", -10, cpublic.strSysDate).ToShortDateString() ;
            dtpTDate.Text = cpublic.strSysDate;
            
            if (gstrER == "ER")
            {
                gstrPano = "";
                txtPano.Text = "";
                dtpFDate.Text =  VB.DateAdd("d", -1, cpublic.strSysDate).ToShortDateString() ;
                chkEr.Visible = true;
                chkEr.Checked = true;
                
            }

        }

        void screen_clear()
        {
            txtPano.Text = "";
            chkEr.Visible = true;
            chkEr.Checked = true;
            
        }
        void screen_display()
        {
            //
            GetData_th(ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim(), "O");

            //환자공통정보 표시 clear                  
            pInfo = new clsComSupSpd.sPatInfo();
        }
        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnPrint2.Click += new EventHandler(eBtnEvent);

            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.cboDept.SelectedIndexChanged += cboDept_SelectedIndexChanged;

            this.txtPano.LostFocus += new EventHandler(eTxtLostFous);
            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);


        }

        void eFormLoad(object sender, EventArgs e)
        {

            //
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {

                //
                csupspd.sSpd_MirNoExecuteMain(ssList, csupspd.sSpdMirNoExecutoMain, csupspd.nSpdMirNoExecutoMain, 1, 0);

                //            
                csupspd.sSpd_NoExecuteResv(ssResv, csupspd.sSpdNoExecuteResv, csupspd.nSpdNoExecuteResv, 1, 0);

                //            
                csupspd.sSpd_ExamOrder(ssExam, csupspd.sSpdExamOrder, csupspd.nSpdExamOrder, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                
                //
                setCombo(clsDB.DbCon);

                //
                screen_clear();

                //
                setCtrlData(clsDB.DbCon);
                
                this.StartPosition = FormStartPosition.CenterParent;
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch)
            {
                //자료조회
                Cursor.Current = Cursors.WaitCursor;
                GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim(), "O");
                Cursor.Current = Cursors.Default;
                //screen_display();
            }
            else if (sender == this.btnPrint)
            {
                //자료출력
                sPrint("인쇄");
            }
            else if (sender == this.btnPrint2)
            {
                //자료출력
                sPrint("선택인쇄");
            }


        }

        void eSpreadClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmMirNoExecuteMain.Pano].Text.Trim();
            string strBDate = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmMirNoExecuteMain.BDate].Text.Trim();
            string strBun = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmMirNoExecuteMain.Gubun].Text.Trim();

            FpSpread o = (FpSpread)sender;

            if (sender == this.ssList)
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

                ReadResvInfo(clsDB.DbCon, ssResv, strPano);

                ssExam.ActiveSheet.RowCount = 0;
                if (strBun.CompareTo("52") >= 0 && strBun.CompareTo("64") <= 0)
                {
                    ReadExamOrderView(clsDB.DbCon, ssExam, strPano, strBDate);
                }
            }

            //ReadResvInfo(clsDB.DbCon, ssResv, strPano);

            //ssExam.ActiveSheet.RowCount = 0;
            //if (strBun.CompareTo("52") >=0 && strBun.CompareTo("64")<=0)
            //{
            //    ReadExamOrderView(clsDB.DbCon, ssExam, strPano, strBDate);
            //}
        }

        void eSpreadDClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.Pano].Text.Trim();
            string strDept = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Text.Trim();

            //FpSpread o = (FpSpread)sender;

            //if (e.Button == MouseButtons.Right)
            //{
            //    return;
            //}

            //if (sender == this.ssList)
            //{
            //    if (e.ColumnHeader == true)
            //    {
            //        clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
            //        return;
            //    }
            //    if (e.RowHeader == true)
            //    {
            //        return;
            //    }
            //}
        }
        
        void eTxtLostFous(object sender, EventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (txtPano.Text.Trim() != "")
                {
                    txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
                }
            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtPano.Text.Trim() != "")
                    {
                        txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
                    }
                }
            }
        }

        void ReadExamOrderView(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd, string argPano,string argBDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";

            spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                dt = sel_ExamOrder(pDbCon, argPano, argBDate, 0, true);

                #region //데이터셋 읽어 자료 표시
                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.ActiveSheet.RowCount = dt.Rows.Count;
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.JDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.Bi].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.OrderNo].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.SpecNo].Text = dt.Rows[i]["SpecNo"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.RDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.MasterCode].Text = dt.Rows[i]["MasterCode"].ToString().Trim();
                        //spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirExamOrder.ExamName].Text = dt.Rows[i]["SName"].ToString().Trim();

                        Application.DoEvents();

                    }
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                #endregion

            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
            }
        }

        void ReadResvInfo(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd,string argPano)
        {            
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            
            spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                dt = sel_opdResv(pDbCon, argPano);

                #region //데이터셋 읽어 자료 표시
                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");                    
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.ActiveSheet.RowCount = dt.Rows.Count;
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.RDate].Text = dt.Rows[i]["Date3"].ToString().Trim();

                    }
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                #endregion

            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장                
            }


        }         

        void setCombo(PsmhDb pDbCon)
        {
                        
            DataTable dt = null;
            
            //
            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체");
            cboDept.SelectedIndex = 0;


            Application.DoEvents();

            cboDept.Items.Clear();


            dt = comSql.sel_BAS_CLINICDEPT_COMBO(pDbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDept, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }


            cboDept.SelectedIndex = 0;

            cboDoct.Items.Clear();
            cboDoct.Items.Add("****.전체");
            cboDoct.SelectedIndex = 0;

                       

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd, string argFDate, string argTDate, string GbIO)
        {            
            int nRow = 0;

            
            
            DataTable dt = null;

            spd.Enabled = false;

            spd.ActiveSheet.RowCount = 0;
            ssResv.ActiveSheet.RowCount = 0;
            ssExam.ActiveSheet.RowCount = 0;

            nRow = 0;                   

            #region //미시행 관련 쿼리 배열 변수 세팅

            argsql = new string[Enum.GetValues(typeof(enmsql)).Length];            
            argsql[(int)enmsql.FDate] = argFDate;
            argsql[(int)enmsql.TDate] = argTDate;
            argsql[(int)enmsql.DeptCode] = cboDept.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.DrCode] = cboDoct.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.Pano] = txtPano.Text.Trim();
    
            argsql[(int)enmsql.ChkExam1] = chkExam1.Checked.ToString();
            argsql[(int)enmsql.ChkExam2] = chkExam2.Checked.ToString();
            argsql[(int)enmsql.ChkExam3] = chkExam3.Checked.ToString();
            argsql[(int)enmsql.ChkExam8] = chkExam8.Checked.ToString();
            argsql[(int)enmsql.ChkExam9] = chkExam9.Checked.ToString();
            argsql[(int)enmsql.ChkExam10] = chkExam10.Checked.ToString();

            argsql[(int)enmsql.ChkER] = chkEr.Checked.ToString();
            argsql[(int)enmsql.ChkStation] = chkStation.Checked.ToString();

            if(rdojohapAll.Checked == true)
            {
                strJohap = "0";
            }
            else if (rdojohap1.Checked == true)
            {
                strJohap = "1";
            }
            else if (rdojohap2.Checked == true)
            {
                strJohap = "2";
            }

            if (chkExam1.Checked == true)
            {
                strChkExam = "0";
            }
            else if (chkExam2.Checked == true)
            {
                strChkExam = "1";
            }
            else if (chkExam3.Checked == true)
            {
                strChkExam = "2";
            }

            #endregion

            if (chkExam1.Checked == true || chkExam2.Checked == true || chkExam3.Checked == true)
            {

                //미시행 XRAY 관련 쿼리
                dt = sel_Tot_NoExecute(pDbCon, "XRAY",argsql,strJohap , strChkExam);                

                //데이터셋 읽어 자료 표시
                setSpdData(pDbCon, spd, dt, "XRAY", argsql, ref nRow);
                
            }                      

            //혈액검사
            if (chkExam8.Checked == true)
            {

                //미시행 XRAY 관련 쿼리
                dt = sel_Tot_NoExecute(pDbCon, "EXAM",argsql, strJohap, strChkExam);

                //데이터셋 읽어 자료 표시
                setSpdData(pDbCon, spd, dt, "EXAM", argsql, ref nRow);

            }

            if (chkExam9.Checked == true)
            {

                //미시행 XRAY 관련 쿼리
                dt = sel_Tot_NoExecute(pDbCon, "EKG", argsql, strJohap, strChkExam);

                //데이터셋 읽어 자료 표시
                setSpdData(pDbCon, spd, dt, "EKG", argsql, ref nRow);

            }

            if (chkExam10.Checked == true)
            {

                //미시행 XRAY 관련 쿼리
                dt = sel_Tot_NoExecute(pDbCon, "PSYCH", argsql, strJohap, strChkExam);

                //데이터셋 읽어 자료 표시
                setSpdData(pDbCon, spd, dt, "PSYCH", argsql, ref nRow);

            }


            spd.ActiveSheet.RowCount = nRow;

            ssList.Enabled = true;


        }              

        string ReadSts1(string argsts)
        {
            if (argsts == "0")
            {
                return "미확인";
            }
            else if (argsts == "1")
            {
                return "접수";
            }
            else if (argsts == "2")
            {
                return "예약";
            }
            else if (argsts == "3")
            {
                return "촬영준비";
            }
            else if (argsts == "7")
            {
                return "완료";
            }
            else if (argsts == "D")
            {
                return "수납취소";
            }
            else if (argsts == "S")
            {
                return "호명";
            }
            else
            {
                return "";
            }

        }
        //내시경구분
        string ReadSts2(string argsts, string strRDate)
        {
            if (argsts == "1")
            {
                if (strRDate != "")
                {
                    if (strRDate.CompareTo(cpublic.strSysDate) >= 0)
                    {
                        return "예약";
                    }
                    else
                    {
                        return "예약부도";
                    }

                }
                else
                {
                    return "접수";
                }

            }
            else if (argsts == "2")
            {
                return "미접수";
            }
            else if (argsts == "*")
            {
                return "취소";
            }
            else if (argsts == "7")
            {
                return "완료";
            }
            else
            {
                return "";
            }

        }

        //기능검사
        string ReadSts3(string argsts)
        {
            if (argsts == "1")
            {
                return "미접수";

            }
            else if (argsts == "2")
            {
                return "예약";
            }
            else if (argsts == "3")
            {
                return "접수";
            }
            else if (argsts == "9")
            {
                return "취소";
            }
            else
            {
                return "";
            }

        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDept.SelectedItem.ToString(), 2), "",false);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDoct, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

        }

        void sPrint(string Gbn)
        {
            string[] strhead = new string[2];
            string[] strfont = new string[2];

            //
            read_sysdate();

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strhead[0] = "/c/f1/n" + "미시행 검사 내역" + "/f1/n";
            strhead[1] = "/n/l/f2" + "조회기간 : " + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + " /l/f2" + "  출력시간 : " + cpublic.strSysDate + " " +cpublic.strSysTime + " /n";



            if (Gbn == "인쇄")
            {
                cSpd.setColStyle(ssList, -1, (int)clsComSupSpd.enmMirNoExecuteMain.check01, clsSpread.enmSpdType.Hide);
                
            }
            else if (Gbn == "선택인쇄")
            {
                for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmMirNoExecuteMain.check01].Text == "" )
                    {
                        ssList.ActiveSheet.Rows[i].Visible = false;
                    }                    
                }
            }
            
            csupspd.SPREAD_PRINT(ssList_Sheet1, ssList, strhead, strfont, 10, 10, 2, true);

            //시트초기화
            csupspd.sSpd_MirNoExecuteMain(ssList, csupspd.sSpdMirNoExecutoMain, csupspd.nSpdMirNoExecutoMain, 1, 0);

        }

        void setSpdData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd, DataTable dt, string Gbn, string[] arg, ref int nRow)
        {
            DataTable dt1 = null;
            string strOK = "";
            string TDate = "";

            spd.ActiveSheet.RowCount = nRow;
            spd.ActiveSheet.SetRowHeight(-1, 24);
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            for (int i = 0; i < dt.Rows.Count; i++)
            {                
                strOK = "OK";
                dt1 = null;

                if (Gbn == "XRAY")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        dt1 = sel_XrayDetail(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                            {
                                strOK = "OK";
                            }
                            else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                            {
                                strOK = "OK";
                            }
                        }
                        else
                        {
                            strOK = "OK";
                        }

                    }
                }
                else if (Gbn == "EXAM")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                        {
                            dt1 = sel_ExamOrder(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                                {
                                    strOK = "OK";
                                }
                                else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                }
                else if (Gbn == "EKG")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                        {
                            dt1 = sel_EKGOrder(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                                {
                                    strOK = "OK";
                                }
                                else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                }
                else if (Gbn == "PSYCH")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                        {
                            dt1 = sel_PSYCHOrder(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                                {
                                    strOK = "OK";
                                }
                                else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                }


                if (strOK == "OK")
                {
                    if (spd.ActiveSheet.RowCount <= nRow) spd.ActiveSheet.RowCount = nRow + 1;

                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.check01].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Gubun].Value = dt.Rows[i]["Bun"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.DrCode].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.DrName].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.ExCode].Value = dt.Rows[i]["SuCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.ExName].Value = cxray.BAS_SUN_READ(pDbCon, dt.Rows[i]["SuCode"].ToString().Trim());
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Johap].Value = dt.Rows[i]["Bi"].ToString().Trim();

                    if (dt.Rows[i]["OrderNo"].ToString().Trim() != "" && dt.Rows[i]["OrderNo"].ToString().Trim() != "0")
                    {
                        dt1 = sel_XrayDetail(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));

                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            if (dt1 != null) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["JDate"].ToString().Trim();

                            //if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                            if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(TDate) <= 0)
                            {
                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = VB.Format(Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim()), "###,###,###,##0");
                            }
                            else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                            {
                                //if (dt1.Rows[0]["JDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) > 0)
                                if (dt1.Rows[0]["JDate"].ToString().Trim().CompareTo(TDate) > 0)
                                {
                                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                                }

                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = VB.Format(Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim()), "###,###,###,##0");
                            }

                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["RDate"].ToString().Trim();

                            //if (dt1.Rows[0]["RDate"].ToString().Trim() == cpublic.strSysDate)
                            //{
                            //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                            //}
                            //else if (dt1.Rows[0]["RDate"].ToString().Trim() != "" && dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) < 0)
                            //{
                            //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                            //}

                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = VB.Format(Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim()), "###,###,###,##0");
                        }

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt.Rows[i]["BDate"].ToString().Trim();

                        //EXAM 접수일자는 EXAM_SPECMST 로 처리
                        if (Gbn == "EXAM")
                        {
                            dt1 = sel_ExamSpecMst(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = "";
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["RECEIVEDATE"].ToString().Trim();
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["JEPDATE"].ToString().Trim();

                        }
                        else if (Gbn == "EKG")
                        {
                            dt1 = sel_EKGOrder(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = "";
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["JDATE"].ToString().Trim();
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["RDATE"].ToString().Trim();

                        }
                    }
                    else
                    {
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = VB.Format(Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim()), "###,###,###,##0");
                    }

                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt1].Value = VB.Format(Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim()), "###,###,###,##0");

                    nRow++;
                }

            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 외래 SLIP 기준 영상의학과, 혈액, 진검 미시행 쿼리 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        DataTable sel_Tot_NoExecute(PsmhDb pDbCon, string Gbn, string[] arg, string argjohap, string argExam)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT                                                                                      ";
            SQL = SQL + "\r\n" + " TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.BI,                                   ";
            SQL = SQL + "\r\n" + " A.BUN, A.PANO, B.SNAME,A.DEPTCODE,  A.SUCODE, A.ORDERNO,  SUM(A.AMT1) amt    ";
            SQL = SQL + "\r\n" + " FROM " + ComNum.DB_PMPA + "OPD_SLIP a , " + ComNum.DB_PMPA + "BAS_PATIENT b  ";            
            SQL = SQL + "\r\n" + "  WHERE 1=1 ";
            SQL = SQL + "\r\n" + "   AND A.ACTDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')    ";
            SQL = SQL + "\r\n" + "   AND A.ACTDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD')    ";
            SQL = SQL + "\r\n" + "   AND A.BDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')      ";
            SQL = SQL + "\r\n" + "   AND A.BDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD')      ";
            SQL = SQL + "\r\n" + "   AND A.PANO = B.PANO(+)                                                     ";
            
            if (Gbn == "XRAY")
            {
                if(argExam == "0")
                {
                    SQL = SQL + "\r\n" + "   AND (A.BUN >='65'  AND BUN <='73' OR A.BUN ='49')   "; //영상의학
                }
                else if(argExam == "1")
                {
                    SQL = SQL + "\r\n" + "   AND (A.BUN ='71' OR A.BUN = '49')   "; //초음파
                }
                else if(argExam == "2")
                {
                    SQL = SQL + "\r\n" + "   AND (A.BUN ='72' OR A.BUN = '73')  "; //특수촬영
                }
            }
            else if (Gbn == "EXAM")
            {
                SQL = SQL + "\r\n" + "   AND A.BUN >='52'  AND BUN <='64'   "; //혈액,진검
            }
            else if (Gbn == "EKG")
            {
                SQL = SQL + "\r\n" + "   AND A.SUCODE IN ('E6541','F6001','F6001A','E7123','6MWT')"; //심전도실
            }
            else if (Gbn == "PSYCH")
            {
                SQL = SQL + "\r\n" + "    AND (A.BUN ='27') "; //심리검사
            }
            SQL = SQL + "\r\n" + "   AND A.OKDATE IS NULL   ";
            switch (argjohap)
            {
                case "0": SQL = SQL + "\r\n" + "   AND A.BI IN ('11','12','13','21','22')   "; break;
                case "1": SQL = SQL + "\r\n" + "   AND A.BI IN ('11','12','13')   "; break;
                case "2": SQL = SQL + "\r\n" + "   AND A.BI IN ('21','22')   "; break;
            }
            
            SQL = SQL + "\r\n" + "   AND A.GBSELF ='0'   ";
            SQL = SQL + "\r\n" + "   AND A.YYMM IS NULL   ";


            if (arg[(int)enmsql.Pano] != "") SQL = SQL + "\r\n" + " AND a.PANO = '" + arg[(int)enmsql.Pano] + "' ";
            if (VB.Left(arg[(int)enmsql.DeptCode], 2) != "**") SQL = SQL + ComNum.VBLF + " AND a.DEPTCODE='" + VB.Left(arg[(int)enmsql.DeptCode], 2) + "'  ";
            if (VB.Left(arg[(int)enmsql.DrCode], 4) != "****") SQL = SQL + ComNum.VBLF + " AND a.DrCode='" + VB.Left(arg[(int)enmsql.DrCode], 4) + "'  ";


            if (VB.Left(arg[(int)enmsql.ChkER], 4) == "True")
            {
                SQL = SQL + "\r\n" + " AND a.DeptCode  NOT IN ('ER')  ";
            }


            //SQL = SQL + "\r\n" + "  GROUP BY TO_CHAR(A.BDATE,'YYYY-MM-DD'),A.BUN, A.PANO, B.SNAME, A.DEPTCODE, A.SUCODE , A.ORDERNO ";
            SQL = SQL + "\r\n" + "  GROUP BY A.BDATE, A.BUN, A.PANO, B.SNAME, A.DEPTCODE, A.SUCODE , A.ORDERNO, A.Bi ";
            SQL = SQL + "\r\n" + "   HAVING SUM(A.AMT1) <> 0 ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        DataTable sel_opdResv(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  PANO,SName,DeptCode,DrCode,TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') DATE3                                          \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW                                                                  \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                                                                  \r\n";
            SQL += "   AND (RETDATE IS NULL OR RETDATE  ='')                                                                        \r\n";
            SQL += "   AND (TRANSDATE IS NULL OR TRANSDATE=''  OR TRUNC(TRANSDATE) >=TRUNC(SYSDATE) )                               \r\n";
            SQL += "    ORDER BY DATE3                                                                                              \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        DataTable sel_XrayDetail(PsmhDb pDbCon, string strPano, string strBDate, long nOrderno)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  TO_CHAR(SeekDATE,'YYYY-MM-DD') JDATE                                                                          \r\n";
            SQL += "  ,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + strPano + "'                                                                                  \r\n";
            SQL += "   AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')                                                          \r\n";
            SQL += "   AND ORDERNO =" + nOrderno + "                                                                                \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }

        DataTable sel_ExamOrder(PsmhDb pDbCon, string strPano, string strBDate, long nOrderno=0 ,bool opt=false)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
                        
            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  PANO, BI,DEPTCODE, ORDERNO, SPECNO, CANCEL,MASTERCODE                                                         \r\n";
            SQL += "  ,TO_CHAR(JDATE,'YYYY-MM-DD') JDATE                                                                            \r\n";
            SQL += "  ,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                                                                            \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ORDER                                                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + strPano + "'                                                                                  \r\n";
            SQL += "   AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')                                                          \r\n";
            if (opt== true)
            {
                SQL += "   AND DeptCode NOT IN ('TO','HR')                                                                          \r\n";
                SQL += "   AND IPDOPD ='O'                                                                                          \r\n";
            }
            else
            {
                SQL += "   AND ORDERNO =" + nOrderno + "                                                                            \r\n";
            }            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }

        DataTable sel_EKGOrder(PsmhDb pDbCon, string strPano, string strBDate, long nOrderno = 0, bool opt = false)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  PTNO,DEPTCODE, ORDERNO                                                         \r\n";
            SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD') JDATE                                                                            \r\n";
            SQL += "  ,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                                                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND PTno ='" + strPano + "'                                                                                  \r\n";
            SQL += "   AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')                                                          \r\n";
            SQL += "   AND ORDERNO =" + nOrderno + "                                                                            \r\n";
            

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }
        DataTable sel_PSYCHOrder(PsmhDb pDbCon, string strPano, string strBDate, long nOrderno = 0, bool opt = false)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  PTNO,DEPTCODE, ORDERNO                                                         \r\n";
            SQL += "  ,TO_CHAR(BDATE,'YYYY-MM-DD') JDATE                                                                            \r\n";
            SQL += "  ,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                                                                            \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "ETC_JUPMST                                                                         \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND PTno ='" + strPano + "'                                                                                  \r\n";
            SQL += "   AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')                                                          \r\n";
            SQL += "   AND ORDERNO =" + nOrderno + "                                                                            \r\n";


            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }
        DataTable sel_ExamSpecMst(PsmhDb pDbCon, string strPano, string strBDate, long nOrderno)
        {
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                         \r\n";
            SQL += "  TO_CHAR(RECEIVEDATE,'YYYY-MM-DD') RECEIVEDATE  , TO_CHAR(BDATE,'YYYY-MM-DD') JEPDATE                                                            \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECMST                                                                       \r\n";
            SQL += "  WHERE 1 = 1                                                                                                   \r\n";
            SQL += "   AND Pano ='" + strPano + "'                                                                                  \r\n";
            SQL += "   AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')                                                          \r\n";
            SQL += "   AND ORDERNO =" + nOrderno + "                                                                                \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt1;
        }

        void GetData_th(FarPoint.Win.Spread.FpSpread Spd, string argFDate, string argTDate, string GbIO)
        {
            Cursor.Current = Cursors.WaitCursor;

            Spd.ActiveSheet.RowCount = 0;
            
            #region //미시행 관련 쿼리 배열 변수 세팅

            argsql = new string[Enum.GetValues(typeof(enmsql)).Length];
            argsql[(int)enmsql.FDate] = argFDate;
            argsql[(int)enmsql.TDate] = argTDate;
            argsql[(int)enmsql.DeptCode] = cboDept.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.DrCode] = cboDoct.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.Pano] = txtPano.Text.Trim();

            argsql[(int)enmsql.ChkExam1] = chkExam1.Checked.ToString();
            argsql[(int)enmsql.ChkExam2] = chkExam2.Checked.ToString();
            argsql[(int)enmsql.ChkExam3] = chkExam3.Checked.ToString();
            argsql[(int)enmsql.ChkExam8] = chkExam8.Checked.ToString();
            argsql[(int)enmsql.ChkExam9] = chkExam9.Checked.ToString();
            argsql[(int)enmsql.ChkExam10] = chkExam10.Checked.ToString();

            argsql[(int)enmsql.ChkER] = chkEr.Checked.ToString();
            argsql[(int)enmsql.ChkStation] = chkStation.Checked.ToString();

            #endregion

            spd = ssList;

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;
                       

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;

            spd.ActiveSheet.RowCount = 0;
            nRow = 0;

            if (chkExam1.Checked == true || chkExam2.Checked == true || chkExam3.Checked == true)
            {
                dt = sel_Tot_NoExecute(clsDB.DbCon, "XRAY", argsql, strJohap, strChkExam);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "XRAY", argsql);
            }
            if (chkExam8.Checked == true)
            {
                dt = sel_Tot_NoExecute(clsDB.DbCon, "EXAM", argsql, strJohap, strChkExam);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "EXAM", argsql);
            }
            if (chkExam9.Checked == true)
            {
                dt = sel_Tot_NoExecute(clsDB.DbCon, "EKG", argsql, strJohap, strChkExam);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "EKG", argsql);
            }
            if (chkExam10.Checked == true)
            {
                dt = sel_Tot_NoExecute(clsDB.DbCon, "PSYCH", argsql, strJohap, strChkExam);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "PSYCH", argsql);
            }

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt, string Gbn, string[] arg);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt, string Gbn, string[] arg)
        {
            string strOK = "";

            DataTable dt1 = null;

            #region // 데이타 표시
       
            spd.ActiveSheet.RowCount = nRow;
            spd.ActiveSheet.SetRowHeight(-1, 24);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strOK = "OK";
                dt1 = null;

                if (Gbn == "XRAY")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        dt1 = sel_XrayDetail(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                            {
                                strOK = "OK";
                            }
                            else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                            {
                                strOK = "OK";
                            }
                        }
                        else
                        {
                            strOK = "OK";
                        }

                    }
                }
                else if (Gbn == "EXAM")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                        {
                            dt1 = sel_ExamOrder(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                                {
                                    strOK = "OK";
                                }
                                else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                }
                else if (Gbn == "EKG")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                        {
                            dt1 = sel_EKGOrder(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                                {
                                    strOK = "OK";
                                }
                                else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                }
                else if (Gbn == "PSYCH")
                {
                    if (arg[(int)enmsql.ChkStation] == "True")
                    {
                        strOK = "NO";

                        if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                        {
                            dt1 = sel_PSYCHOrder(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                                {
                                    strOK = "OK";
                                }
                                else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                                {
                                    strOK = "OK";
                                }
                            }
                            else
                            {
                                strOK = "OK";
                            }

                        }
                        else
                        {
                            strOK = "OK";
                        }
                    }
                }
                if (strOK == "OK")
                {
                    if (spd.ActiveSheet.RowCount <= nRow) spd.ActiveSheet.RowCount = nRow + 1;

                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.check01].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Gubun].Value = dt.Rows[i]["Bun"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.DrCode].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.DrName].Value = "";
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.ExCode].Value = dt.Rows[i]["SuCode"].ToString().Trim();                    
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.ExName].Value = cxray.BAS_SUN_READ(clsDB.DbCon, dt.Rows[i]["SuCode"].ToString().Trim());
                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();


                    if (dt.Rows[i]["OrderNo"].ToString().Trim() != "")
                    {
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            if (dt1 != null) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["JDate"].ToString().Trim();

                            if (dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) <= 0)
                            {
                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = dt.Rows[i]["AMT"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["RDate"].ToString().Trim() == "")
                            {
                                if (dt1.Rows[0]["JDate"].ToString().Trim().CompareTo(arg[(int)enmsql.TDate]) < 0)
                                {
                                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                                }

                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = dt.Rows[i]["AMT"].ToString().Trim();
                            }

                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["RDate"].ToString().Trim();
                            if (dt1.Rows[0]["RDate"].ToString().Trim() == cpublic.strSysDate)
                            {
                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                            }
                            else if (dt1.Rows[0]["RDate"].ToString().Trim() != "" && dt1.Rows[0]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) < 0)
                            {
                                spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                            }

                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt2].Value = Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim());
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt.Rows[i]["BDate"].ToString().Trim();


                        //EXAM 접수일자는 EXAM_SPECMST 로 처리
                        if (Gbn == "EXAM")
                        {
                            dt1 = sel_ExamSpecMst(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = "";
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["RECEIVEDATE"].ToString().Trim();
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["JEPDATE"].ToString().Trim();

                        }
                        else if (Gbn == "EKG")
                        {
                            dt1 = sel_EKGOrder(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = "";
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["JDATE"].ToString().Trim();
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["RDATE"].ToString().Trim();

                        }
                        else if (Gbn == "PSYCH")
                        {
                            dt1 = sel_PSYCHOrder(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), dt.Rows[i]["BDate"].ToString().Trim(), Convert.ToInt32(dt.Rows[i]["OrderNo"].ToString()));
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = "";
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.JepDate].Value = dt1.Rows[0]["JDATE"].ToString().Trim();
                            if (dt1 != null && dt1.Rows.Count > 0) spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.RDate].Value = dt1.Rows[0]["RDATE"].ToString().Trim();

                        }
                    }

                    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmMirNoExecuteMain.Amt1].Value = dt.Rows[i]["AMT"].ToString().Trim();

                    nRow++;
                }

            }

            dt.Dispose();
            dt = null;
         

            #endregion

           
        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            //this.Progress.Visible = b;
            //this.Progress.IsRunning = b;
        }


        #endregion

    }
}
