using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET06.cs
    /// Description     : 영상의학과 일지 등록폼
    /// Author          : 윤조연
    /// Create Date     : 2018-01-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuilgi\frmilgi2_1.frm(Frmilgi2_1) >> frmComSupXraySET06.cs 폼이름 재정의" />
    public partial class frmComSupXraySET06 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsComSup.SupPInfo cinfo = new clsComSup.SupPInfo();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSQL comSql = new clsComSQL();
        clsComSup sup = new clsComSup();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXray xray = new clsComSupXray();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        ComFunc CF = new ComFunc();

        clsComSupXraySQL.cXray_Ilgi cXray_Ilgi = null;

        string gROWID = "";

        long[] nView = null;
        long[] nSumView = null;

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
        #endregion //MainFormMessage

        public frmComSupXraySET06()
        {
            InitializeComponent();
            setEvent();            
        }

        public frmComSupXraySET06(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //DataTable dt = null;

            setCombo();

            dtpFDate.Text = Convert.ToDateTime(cpublic.strSysDate).AddDays(-1).ToShortDateString();
                        

        }

        void setCtrlInit()
        {
            //clsCompuInfo.SetComputerInfo();
            //DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            //if (ComFunc.isDataTableNull(dt) == false)
            //{
            //    //파트설정로드
            //    if (dt.Rows[0]["VALUEV"].ToString() == clsComSupEnds.enm_EndsPart.ENDO.ToString())
            //    {
            //        partType = clsComSupEnds.enm_EndsPart.ENDO;

            //    }
            //    else if (dt.Rows[0]["VALUEV"].ToString() == clsComSupEnds.enm_EndsPart.HEALTH.ToString())
            //    {
            //        partType = clsComSupEnds.enm_EndsPart.HEALTH;
            //    }
            //    else
            //    {
            //        partType = clsComSupEnds.enm_EndsPart.ALL;
            //    }
            //}

            //setPart();
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);


            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSet.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);

            this.btnPrint.Click += new EventHandler(eBtnPrint);


            //명단 더블클릭 이벤트
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            //this.ssList.EditModeOff += new EventHandler(eSpreadEditOff);
            this.ssList.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);


            //this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.txtSName.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.dtpDate.ValueChanged += new EventHandler(eDtpValueChanged);


        }

        void setTxtTip()
        {
            //툴팁
            ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList.TextTipDelay = 500;
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

                //            
                //cxraySpd.sSpd_enmXrayCVR01(ssList, cxraySpd.sSpdenmXrayCVR01, cxraySpd.nSpdenmXrayCVR01, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //툴팁
                setTxtTip();

                nView = new long[51];
                nSumView = new long[51];

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                //screen_display();

            }

        }

        void eFormResize(object sender, EventArgs e)
        {
            setCtrlProgress();
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
            else if (sender == this.btnCancel)
            {
                screen_clear();
                ss_clear(ssList);
            }
            else if (sender == this.btnSet)
            {
                setSheetBase(ssList);
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                if (clsType.User.IdNumber == "35258")
                {
                    ComFunc.MsgBox("해당부서에서만 등록가능합니다.");
                    return;
                }
                else
                {
                    eSave(clsDB.DbCon, "저장");
                    screen_display();
                }
                
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {


            if (sender == this.btnPrint)
            {
                ePrint();
            }

        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            if (e.ColumnHeader == true)
            {
                return;
            }

            FpSpread s = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {

            }
            else
            {
                //
                //read_cinfo(s, e.Row);

                //알러지정보
                //if (e.Column != (int)clsSupEndsSpd.enmSupEndsSCH01A.RDate && e.Column != (int)clsSupEndsSpd.enmSupEndsSCH01A.RTime)
                //{
                //    //string strAllegy = sup.READ_ALLERGY_POPUP(cinfo.strPano, cinfo.strSName);
                //    //if (strAllegy != "")
                //    //{
                //    //    ComFunc.MsgBox(strAllegy, "환자의 알러지 정보");
                //    //    //환자공통 정보              
                //    //    if (cinfo.strPano != "") conPatInfo1.SetDisPlay(cinfo.strEmrNo, cinfo.strIO, cinfo.strBDate, cinfo.strPano, cinfo.strDept);
                //    //    frmComSupSTS01x.display_pano_view(cinfo.strPano);

                //    //    frmComSupEndsVIEW02x.screen_display_one(e.Column == (int)clsSupEndsSpd.enmSupEndsSCH01A.SName ? "B" : "A", gTab, cinfo.strROWID);

                //    //    readEndoRemark(cinfo.strROWID);

                //    //}
                //}

                //
                //read_cinfo(s, e.Row);


                //TODO 윤조연 환자공통 정보
                //if (cinfo.strPano != "") conPatInfo1.SetDisPlay(cinfo.strEmrNo, cinfo.strIO, cinfo.strBDate, cinfo.strPano, cinfo.strDept);


            }


        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;
            //DataTable dt = null;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }                               
                     
            }
        }

        void eSpreadSelChanged(object sender, SelectionChangedEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            //
            //read_cinfo(o, e.Range.Row);


        }

        void eSpreadButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //
            //read_cinfo(o, e.Row);

            //if (e.Column == (int)clsComSupXraySpd.enmXrayResv01.chk)
            //{
            //    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
            //}
            //else
            //{
            //    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
            //}

        }

        void eSpreadEditChange(object sender, EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;


        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (s.ActiveSheet.RowCount <= 0)
            {
                return;
            }

            if (e.RowHeader == true || e.Column < 1)
            {
                return;
            }
            else
            {
                e.TipText = ssList.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                e.ShowTip = true;
            }

        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            ComboBox o = (ComboBox)sender;
            //조회
            try
            {
                //if (sender == this.cboDept)
                //{
                //    this.cboDoct.Items.Clear();
                //    if (o.SelectedItem.ToString() != null && o.SelectedItem.ToString() != "**.전체")
                //    {
                //        method.setCombo_View(this.cboDoct, sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDept.SelectedItem.ToString(), 2), "", false, true, true), clsParam.enmComParamComboType.ALL);
                //    }

                //}

            }
            catch
            {

            }

        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtPano)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        if (txtPano.Text.Trim() != "")
            //        {
            //            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
            //            screen_display();
            //        }
            //    }
            //}           

        }

        void eDtpValueChanged(object sender, EventArgs e)
        {
            //조회
            //screen_display();
        }

        void eSave(PsmhDb pDbCon, string argJob)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
                        
            read_sysdate();

            clsDB.setBeginTran(pDbCon);

            try
            {

                if (argJob == "저장")
                {
                    #region //일지저장 작업

                    cXray_Ilgi = new clsComSupXraySQL.cXray_Ilgi();
                    cXray_Ilgi.BDATE = dtpFDate.Text.Trim();
                    cXray_Ilgi.ROWID = gROWID;
                    cXray_Ilgi.WORK4 = ssList.ActiveSheet.Cells[46, 4].Text.Trim(); //타이피스트 근무현황 
                    cXray_Ilgi.WORK = ssList.ActiveSheet.Cells[47, 4].Text.Trim();  //영상의학과 근무현황
                    cXray_Ilgi.ITEM = ssList.ActiveSheet.Cells[48, 4].Text.Trim();  //장비고장수리 유무
                    if (cXray_Ilgi.ROWID != "")
                    {
                        SqlErr = cxraySql.up_XRAY_ILGI(pDbCon, cXray_Ilgi, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cxraySql.ins_XRAY_ILGI(pDbCon, cXray_Ilgi, ref intRowAffected);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }

                    #endregion
                }
                else
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("작업구분 오류");
                    return;
                }
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("작업완료");
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            //strTitle = "";
            
            //strHeader = SPR.setSpdPrint_String(strTitle, new Font("", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("맑은고딕", 10, FontStyle.Regular), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 55, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            #endregion

        }

        void read_cinfo(FpSpread Spd, int row)
        {
            //환자공통정보 표시                   
            cinfo = new clsComSup.SupPInfo();

            //cinfo.strIO = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.GbIO].Text.Trim();
            //cinfo.strBDate = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.BDate].Text.Trim(); ;
            //cinfo.strRDate = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.RDate].Text.Trim(); ;
            //cinfo.strPano = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.Pano].Text.Trim(); ;
            //cinfo.strSName = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.SName].Text.Trim(); ;
            //cinfo.strDept = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.DeptCode].Text.Trim();
            //cinfo.strGubun = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.Gubun].Text.Trim();
            //cinfo.strDrName = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.RDrName].Text.Trim();
            //cinfo.strFall = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.Fall].Text.Trim();
            //cinfo.strRoom = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.RoomCode].Text.Trim();
            //cinfo.strMemo = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.Remark].Text.Trim();
            //cinfo.strSend = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.PacsSend].Text.Trim();
            //cinfo.strROWID = Spd.ActiveSheet.Cells[row, (int)clsSupEndsSpd.enmSupEndsSCH01A.ROWID].Text.Trim();



        }

        void setCombo()
        {
            //검사종류
            //method.setCombo_View(this.cboJong, Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_접수종류", "", " Code|| '.' || Name Names ", ""), clsParam.enmComParamComboType.ALL);
                       
        }

        void setCtrlProgress()
        {
            //Point p = new Point();

            //p.X = (this.Size.Width / 2) - 90;
            //if (p.X < 0)
            //{
            //    p.X = 0;
            //}
            //p.Y = this.Progress.Location.Y;

            //this.Progress.Location = p;

        }

        void setSheetBase(FpSpread Spd)
        {
            Spd.ActiveSheet.Cells[45, 4].Text = "특이사항 없습니다.";
            Spd.ActiveSheet.Cells[46, 4].Text = "특이사항 없습니다.";
            Spd.ActiveSheet.Cells[47, 4].Text = "특이사항 없습니다.";
        }

        bool sheet_sel_chk(FpSpread Spd)
        {
            bool sts = false;

            for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                //if (Spd.ActiveSheet.Cells[i, (int)clsSupEndsSpd.enmSupEndsSCH01A.Chk].Text == "True")
                //{
                //    sts = true;
                //    return sts;
                //}
            }

            return sts;
        }

        bool sheet_sel_chk_msg(FpSpread Spd)
        {
            if (sheet_sel_chk(Spd))
            {
                return true;
            }
            else
            {
                ComFunc.MsgBox("대상을 선택후 작업하세요", "선택확인");
                return false;
            }
        }

        bool read_trs_use_chk(string argParent, string argTitle)
        {
            if (argParent == "단축버튼▶")
            {
                return false;
            }
            else if (argParent == "접수등록")
            {
                return false;
            }
            else if (argParent == "도착작업")
            {
                return false;
            }
            else
            {
                if (argTitle == "접수등록")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        void screen_clear(string Job = "")
        {
            
            for (int i = 0; i < 50; i++)
            {
                nView[i] = 0;
                nSumView[i] = 0;
            }

            read_sysdate();
                        
            
            btnSave.Enabled = false;

            if (Job == "")
            {

            }
            else if (Job == "A1")
            {

            }           

        }

        void ss_clear(FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;

            for (i = 1; i <= 12; i++)
            {
                Spd.ActiveSheet.Cells[10, i].Text = "";
            }            

            for ( i = 13; i <= 31; i++)
            {
                Spd.ActiveSheet.Cells[i, 4].Text = "";
                Spd.ActiveSheet.Cells[i, 6].Text = "";

                Spd.ActiveSheet.Cells[i, 11].Text = "";
                Spd.ActiveSheet.Cells[i, 13].Text = "";
            }

            for (i = 36; i <= 44; i++)
            {   
                Spd.ActiveSheet.Cells[i, 3].Text = "";
                Spd.ActiveSheet.Cells[i, 5].Text = "";
                Spd.ActiveSheet.Cells[i, 8].Text = "";
                Spd.ActiveSheet.Cells[i, 10].Text = "";
                Spd.ActiveSheet.Cells[i, 11].Text = "";
                Spd.ActiveSheet.Cells[i, 13].Text = "";
            }
            for ( i = 46; i <= 48; i++)
            {
                Spd.ActiveSheet.Cells[i, 4].Text = "";
            }
            

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {        
            screen_clear();
            ss_clear(ssList);

            string strIO = "";
            if (optIO1.Checked == true)
            {
                strIO = "I";
            }
            else if (optIO2.Checked == true)
            {
                strIO = "O";
            }
            else 
            {
                strIO = "ALL";
            }

            //메인쿼리 및 데이타 표시
            GetData(clsDB.DbCon, ssList,strIO,dtpFDate.Text.Trim());

            btnSave.Enabled = true;

        }
                
        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argIO,string argDate)
        {
            int i = 0;
            DataTable dt = null;

            int nRow = 0;
            string strJong = "";
            string strXSub = "";
            string strXRoom = "";
            
            long nCnt1 = 0;
         
            long[] nSum= new long[6];

            string strStartDate = VB.Left(argDate, 8) + "01";

            Cursor.Current = Cursors.WaitCursor;


            #region //xray_detail 

            dt = cxraySql.sel_XRAY_DETAIL_Code_Ilgi(pDbCon,"00", argIO, argDate, argDate);
            
            if (dt != null && dt.Rows.Count > 0)
            {              
                for (i = 0; i < dt.Rows.Count; i++)
                {                    
                    strJong = dt.Rows[i]["XJONG"].ToString().Trim();
                    strXSub = dt.Rows[i]["XSUBCODE"].ToString().Trim();
                    strXRoom = dt.Rows[i]["XrayRoom"].ToString().Trim();
                    nCnt1 = 0;
                    if (dt.Rows[i]["CNT1"].ToString().Trim() !="")
                    {
                        nCnt1 = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                    }
                    if (strJong =="1")
                    {
                        nView[37] += nCnt1; //일반촬영
                    }
                    else if (strJong == "2")
                    {
                        nView[39] += nCnt1;

                        #region //xjong 2 sub
                        if (strXSub =="02")
                        {

                        }
                        else if (strXSub == "05")
                        {
                            nView[5] += nCnt1; //Ba-Enema
                        }
                        else if (strXSub == "11")
                        {
                            nView[6] += nCnt1; //I.V.P
                        }
                        else if (strXSub == "34")
                        {
                            if (dt.Rows[i]["DeptCode"].ToString().Trim() =="2")
                            {
                                nView[8] += nCnt1; //Mammo 본관
                            }
                            else if (dt.Rows[i]["DeptCode"].ToString().Trim() == "1")
                            {
                                nView[9] += nCnt1; //Mammo 센터
                            }

                        }
                        else if (strXSub == "20")
                        {
                            nView[13] += nCnt1; //Athro
                        }
                        else if (strXSub == "22")
                        {
                            nView[14] += nCnt1; //Myels
                        }
                        else if (strXSub == "17")
                        {
                            nView[15] += nCnt1; //H.S.G
                        }
                        else if (strXSub == "37")
                        {
                            nView[16] += nCnt1; //ERCP
                        }
                        else if (strXSub == "31")
                        {
                            nView[17] += nCnt1; //VCUG
                        }
                        else if (strXSub == "01")
                        {
                            nView[18] += nCnt1; //esophago
                        }
                        else if (strXSub == "23" || strXSub == "24" || strXSub == "28")
                        {
                            nView[19] += nCnt1; //Angio
                        }
                        else if (strXSub == "36" || strXSub == "64" || strXSub == "65")
                        {
                            nView[20] += nCnt1; //Veno
                        }
                        else if (strXSub == "04")
                        {
                            nView[21] += nCnt1; //S.B.S
                        }
                        else if (strXSub == "12")
                        {
                            nView[22] += nCnt1; //D.I.P
                        }
                        else if (strXSub == "14")
                        {
                            nView[23] += nCnt1; //R.G.P
                        }
                        else if (strXSub == "18")
                        {
                            nView[24] += nCnt1; //Fistulo
                        }
                        else if (strXSub == "33")
                        {
                            nView[25] += nCnt1; //Fluoro
                        }
                        else if (strXSub == "09")
                        {
                            nView[26] += nCnt1; //O.P.C
                        }
                        else if (strXSub == "41")
                        {
                            nView[27] += nCnt1; //Ba-redution
                        }
                        else if (strXSub == "15")
                        {
                            nView[28] += nCnt1; //Cysto
                        }
                        else if (strXSub == "16")
                        {
                            nView[29] += nCnt1; //urethro
                        }
                        #endregion
                    }
                    else if (strJong == "3")
                    {
                        nView[39] += nCnt1; //SONO

                        if (strXSub == "32")
                        {
                            nView[30] += nCnt1; //mammo 2013-02-21
                        }
                        else
                        {
                            if (dt.Rows[i]["DeptCode"].ToString().Trim() == "2")
                            {
                                nView[11] += nCnt1; //
                            }
                            else if (dt.Rows[i]["DeptCode"].ToString().Trim() == "1")
                            {
                                nView[12] += nCnt1; //
                            }
                        }
                    }
                    else if (strJong == "4")
                    {
                        nView[39] += nCnt1; //CT
                        if (strXRoom =="C")
                        {
                            nView[1] += nCnt1; // 128ch
                        }
                        else if (strXRoom =="D") 
                        {
                            nView[41] += nCnt1; // 64ch
                        }

                        //2018-06-07 안정수, 윤만식 팀장 요청으로 CT-Drive 추가
                        else 
                        {
                            nView[43] += nCnt1; //Drive
                        }
                    }
                    else if (strJong == "5")
                    {
                        nView[39] += nCnt1; //MRI
                        if (strXRoom == "L")
                        {
                            nView[2] += nCnt1; //
                        }
                        else
                        {
                            nView[42] += nCnt1; //
                        }
                    }
                    else if (strJong == "6")
                    {
                        nView[39] += nCnt1; //RI
                        nView[7] += nCnt1; //RI
                    }
                    else if (strJong == "7")
                    {
                        nView[39] += nCnt1; //BMD
                        if (strXRoom == "B")
                        {
                            nView[10] += nCnt1; //
                        }
                        else
                        {
                            nView[50] += nCnt1; //
                        }
                    }
                    else
                    {

                    }
                    
                }
            }

            #endregion

            #region //xray_detail ugi 

            dt = cxraySql.sel_XRAY_DETAIL_Code_Ilgi(pDbCon, "01", argIO, argDate, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 = 0;
                    if (dt.Rows[i]["CNT1"].ToString().Trim() != "")
                    {
                        nCnt1 = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                    }

                    nView[39] += nCnt1; //
                    if (dt.Rows[i]["DeptCode"].ToString().Trim()=="2")
                    {
                        nView[3] += nCnt1; //U.G.I 본관
                    }
                    else if (dt.Rows[i]["DeptCode"].ToString().Trim() == "1")
                    {
                        nView[4] += nCnt1; //U.G.I 센터
                    }

                }
            }
            #endregion

            #region //xray_detail G2702

            dt = cxraySql.sel_XRAY_DETAIL_Code_Ilgi(pDbCon, "02", argIO, argDate, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 = 0;
                    if (dt.Rows[i]["CNT1"].ToString().Trim() != "")
                    {
                        nCnt1 = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                    }

                    nView[39] += nCnt1; //
                    nView[8] += nCnt1; //Mammo


                }
            }
            #endregion

            #region //xray_detail 01~ 당일까지

            dt = cxraySql.sel_XRAY_DETAIL_Code_Ilgi(pDbCon, "00", argIO, strStartDate, argDate);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strJong = dt.Rows[i]["XJONG"].ToString().Trim();
                    strXSub = dt.Rows[i]["XSUBCODE"].ToString().Trim();
                    strXRoom = dt.Rows[i]["XrayRoom"].ToString().Trim();
                    nCnt1 = 0;
                    if (dt.Rows[i]["CNT1"].ToString().Trim() != "")
                    {
                        nCnt1 = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                    }
                    if (strJong == "1")
                    {
                        nSumView[37] += nCnt1; //일반촬영
                    }
                    else if (strJong == "2")
                    {
                        nSumView[39] += nCnt1;

                        #region //xjong 2 sub
                        if (strXSub == "02")
                        {

                        }
                        else if (strXSub == "05")
                        {
                            nSumView[5] += nCnt1; //Ba-Enema
                        }
                        else if (strXSub == "11")
                        {
                            nSumView[6] += nCnt1; //I.V.P
                        }
                        else if (strXSub == "34")
                        {
                            if (dt.Rows[i]["DeptCode"].ToString().Trim() == "2")
                            {
                                nSumView[8] += nCnt1; //Mammo 본관
                            }
                            else if (dt.Rows[i]["DeptCode"].ToString().Trim() == "1")
                            {
                                nSumView[9] += nCnt1; //Mammo 센터
                            }

                        }
                        else if (strXSub == "20")
                        {
                            nSumView[13] += nCnt1; //Athro
                        }
                        else if (strXSub == "22")
                        {
                            nSumView[14] += nCnt1; //Myels
                        }
                        else if (strXSub == "17")
                        {
                            nSumView[15] += nCnt1; //H.S.G
                        }
                        else if (strXSub == "37")
                        {
                            nSumView[16] += nCnt1; //ERCP
                        }
                        else if (strXSub == "31")
                        {
                            nSumView[17] += nCnt1; //VCUG
                        }
                        else if (strXSub == "01")
                        {
                            nSumView[18] += nCnt1; //esophago
                        }
                        else if (strXSub == "23" || strXSub == "24" || strXSub == "28")
                        {
                            nSumView[19] += nCnt1; //Angio
                        }
                        else if (strXSub == "36" || strXSub == "64" || strXSub == "65")
                        {
                            nSumView[20] += nCnt1; //Veno
                        }
                        else if (strXSub == "04")
                        {
                            nSumView[21] += nCnt1; //S.B.S
                        }
                        else if (strXSub == "12")
                        {
                            nSumView[22] += nCnt1; //D.I.P
                        }
                        else if (strXSub == "14")
                        {
                            nSumView[23] += nCnt1; //R.G.P
                        }
                        else if (strXSub == "18")
                        {
                            nSumView[24] += nCnt1; //Fistulo
                        }
                        else if (strXSub == "33")
                        {
                            nSumView[25] += nCnt1; //Fluoro
                        }
                        else if (strXSub == "09")
                        {
                            nSumView[26] += nCnt1; //O.P.C
                        }
                        else if (strXSub == "41")
                        {
                            nSumView[27] += nCnt1; //Ba-redution
                        }
                        else if (strXSub == "15")
                        {
                            nSumView[28] += nCnt1; //Cysto
                        }
                        else if (strXSub == "16")
                        {
                            nSumView[29] += nCnt1; //urethro
                        }
                        #endregion
                    }
                    else if (strJong == "3")
                    {
                        nSumView[39] += nCnt1; //SONO

                        if (strXSub == "32")
                        {
                            nSumView[30] += nCnt1; //mammo 2013-02-21
                        }
                        else
                        {
                            if (dt.Rows[i]["DeptCode"].ToString().Trim() == "2")
                            {
                                nSumView[11] += nCnt1; //
                            }
                            else if (dt.Rows[i]["DeptCode"].ToString().Trim() == "1")
                            {
                                nSumView[12] += nCnt1; //
                            }
                        }
                    }
                    else if (strJong == "4")
                    {
                        nSumView[39] += nCnt1; //CT
                        if (strXRoom == "C")
                        {
                            nSumView[1] += nCnt1; // 128ch
                        }
                        else if (strXRoom == "D")
                        {
                            nSumView[41] += nCnt1; // 64ch
                        }

                        //2018-06-07 안정수, 윤만식 팀장 요청으로 CT-Drive 추가
                        else
                        {
                            nSumView[43] += nCnt1; // Drive
                        }
                    }
                    else if (strJong == "5")
                    {
                        nSumView[39] += nCnt1; //MRI
                        if (strXRoom == "L")
                        {
                            nSumView[2] += nCnt1; //
                        }
                        else
                        {
                            nSumView[42] += nCnt1; //
                        }
                    }
                    else if (strJong == "6")
                    {
                        nSumView[39] += nCnt1; //RI
                        nSumView[7] += nCnt1; //RI
                    }
                    else if (strJong == "7")
                    {
                        nSumView[39] += nCnt1; //BMD
                        if (strXRoom == "B")
                        {
                            nSumView[10] += nCnt1; //
                        }
                        else
                        {
                            nSumView[50] += nCnt1; //
                        }
                    }
                    else
                    {

                    }

                }
            }

            #endregion

            #region //xray_detail 01~ 당일까지 ugi 

            dt = cxraySql.sel_XRAY_DETAIL_Code_Ilgi(pDbCon, "01", argIO, strStartDate, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 = 0;
                    if (dt.Rows[i]["CNT1"].ToString().Trim() != "")
                    {
                        nCnt1 = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                    }

                    nSumView[39] += nCnt1; //
                    if (dt.Rows[i]["DeptCode"].ToString().Trim() == "2")
                    {
                        nSumView[3] += nCnt1; //U.G.I 본관
                    }
                    else if (dt.Rows[i]["DeptCode"].ToString().Trim() == "1")
                    {
                        nSumView[4] += nCnt1; //U.G.I 센터
                    }

                }
            }
            #endregion

            #region //xray_detail 01~ 당일까지 G2702

            dt = cxraySql.sel_XRAY_DETAIL_Code_Ilgi(pDbCon, "02", argIO, strStartDate, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCnt1 = 0;
                    if (dt.Rows[i]["CNT1"].ToString().Trim() != "")
                    {
                        nCnt1 = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                    }

                    nSumView[39] += nCnt1; //
                    nSumView[8] += nCnt1; //Mammo


                }
            }
            #endregion

            #region //xray_ilgi

            gROWID = "";
            dt = cxraySql.sel_XRAY_ILGI(pDbCon, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {                
                Spd.ActiveSheet.Cells[46, 4].Text = dt.Rows[0]["WORK4"].ToString().Trim();  //타이피스트 근무현황
                Spd.ActiveSheet.Cells[47, 4].Text = dt.Rows[0]["WORK"].ToString().Trim();   //영상의학과 근무현황
                Spd.ActiveSheet.Cells[48, 4].Text = dt.Rows[0]["ITEM"].ToString().Trim();   //장비고장수리 유무

                gROWID = dt.Rows[0]["ROWID"].ToString().Trim();


            }
            #endregion

            #region //xray_resultnew 판독

            dt = cRead.sel_XRAY_RESULTNEW(pDbCon, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                for ( i = 0; i < dt.Rows.Count; i++)
                {
                    nSum[0] = 0;

                    if (dt.Rows[i]["XDrCode1"].ToString().Trim() == "22115")
                    {
                        nRow = 39; //이채경
                    }
                    else if (dt.Rows[i]["XDrCode1"].ToString().Trim() == "28039")
                    {
                        nRow = 38; //이종구
                    }
                    else if (dt.Rows[i]["XDrCode1"].ToString().Trim() == "29377")
                    {
                        nRow = 40; //구관민
                    }
                    else if (dt.Rows[i]["XDrCode1"].ToString().Trim() == "32111")
                    {
                        nRow = 41; //정인희
                    }
                    else if (dt.Rows[i]["XDrCode1"].ToString().Trim() == "33781")
                    {
                        nRow = 36; //유윤정
                    }
                    else if (dt.Rows[i]["XDrCode1"].ToString().Trim() == "33852")
                    {
                        nRow = 37; //정명석
                    }
                    else
                    {
                        nRow = 43; //
                    }
                                        
                    
                    if (dt.Rows[i]["IlbanCnt"].ToString().Trim() != "")
                    {
                        nSum[0] += Convert.ToInt32(dt.Rows[i]["IlbanCnt"].ToString().Trim());
                        nSum[1] += Convert.ToInt32(dt.Rows[i]["IlbanCnt"].ToString().Trim());

                        Spd.ActiveSheet.Cells[nRow, 3].Text = VB.Format(nSum[1], "###,###");
                    }
                    
                    if (dt.Rows[i]["TuksuCnt"].ToString().Trim() != "")
                    {
                        nSum[0] += Convert.ToInt32(dt.Rows[i]["TuksuCnt"].ToString().Trim());
                        nSum[2] += Convert.ToInt32(dt.Rows[i]["TuksuCnt"].ToString().Trim());
                        Spd.ActiveSheet.Cells[nRow, 5].Text = VB.Format(nSum[2], "###,###");
                    }
                    
                    if (dt.Rows[i]["SonoCnt"].ToString().Trim() != "")
                    {
                        nSum[0] += Convert.ToInt32(dt.Rows[i]["SonoCnt"].ToString().Trim());
                        nSum[3] += Convert.ToInt32(dt.Rows[i]["SonoCnt"].ToString().Trim());
                        Spd.ActiveSheet.Cells[nRow, 8].Text = VB.Format(nSum[3], "###,###");
                    }
                    
                    if (dt.Rows[i]["CtCnt"].ToString().Trim() != "")
                    {
                        nSum[0] += Convert.ToInt32(dt.Rows[i]["CtCnt"].ToString().Trim());
                        nSum[4] += Convert.ToInt32(dt.Rows[i]["CtCnt"].ToString().Trim());
                        Spd.ActiveSheet.Cells[nRow, 10].Text = VB.Format(nSum[4], "###,###");
                    }
                    
                    if (dt.Rows[i]["MriCnt"].ToString().Trim() != "")
                    {
                        nSum[0] += Convert.ToInt32(dt.Rows[i]["MriCnt"].ToString().Trim());
                        nSum[5] += Convert.ToInt32(dt.Rows[i]["MriCnt"].ToString().Trim());
                        Spd.ActiveSheet.Cells[nRow, 11].Text = VB.Format(nSum[5], "###,###");
                    }

                    Spd.ActiveSheet.Cells[nRow, 13].Text = VB.Format(nSum[0],"###,###");
                }

                Spd.ActiveSheet.Cells[44, 3].Text = VB.Format(nSum[1],"###,###");
                Spd.ActiveSheet.Cells[44, 5].Text = VB.Format(nSum[2], "###,###");
                Spd.ActiveSheet.Cells[44, 8].Text = VB.Format(nSum[3], "###,###");
                Spd.ActiveSheet.Cells[44, 10].Text = VB.Format(nSum[4], "###,###");
                Spd.ActiveSheet.Cells[44, 11].Text = VB.Format(nSum[5], "###,###");
                Spd.ActiveSheet.Cells[44, 13].Text = VB.Format(nSum[1] + nSum[2] + nSum[3] + nSum[4] + nSum[5],"###,###");
                

            }
            #endregion

            #region // 변수내용 시트 표시

            //상단 날짜표시
            Spd.ActiveSheet.Cells[7, 1].Text = VB.Left(dtpFDate.Text, 4) + "년 " + VB.Mid(dtpFDate.Text, 6, 2) + "월 " + VB.Right(dtpFDate.Text, 2) + "일 " + CF.READ_YOIL(clsDB.DbCon, dtpFDate.Text);
            //상단 요약건수
            Spd.ActiveSheet.Cells[10, 1].Text = " 당일 : " + VB.Format(nView[37], "###,###");
            Spd.ActiveSheet.Cells[10, 3].Text = VB.Format(nSumView[37], "###,###");
            Spd.ActiveSheet.Cells[10, 5].Text = " 당일 : " + VB.Format(nView[39], "###,###");
            Spd.ActiveSheet.Cells[10, 8].Text = VB.Format(nSumView[39], "###,###");
            Spd.ActiveSheet.Cells[10, 10].Text = " 당일 : " + VB.Format(nView[37] + nView[39], "###,###");
            Spd.ActiveSheet.Cells[10, 12].Text = VB.Format(nSumView[37] + nSumView[39], "###,###");

            #region 기본 상세내역1

            //2018-06-08 안정수, 윤만식 팀장요청으로, 64ch 128ch 위치 변경함

            //CT1 (64ch)
            Spd.ActiveSheet.Cells[13, 4].Text = VB.Format(nView[41], "###,###");
            Spd.ActiveSheet.Cells[13, 6].Text = VB.Format(nSumView[41], "###,###");

            //CT2 (128ch)
            Spd.ActiveSheet.Cells[14, 4].Text = VB.Format(nView[1], "###,###");
            Spd.ActiveSheet.Cells[14, 6].Text = VB.Format(nSumView[1], "###,###");
                        
            //2018-06-07 안정수, 윤만식 팀장요청으로 CT-Drive 항목 추가
            //CT3(Drive)
            Spd.ActiveSheet.Cells[15, 4].Text = VB.Format(nView[43], "###,###");
            Spd.ActiveSheet.Cells[15, 6].Text = VB.Format(nSumView[43], "###,###");

            //MR1
            Spd.ActiveSheet.Cells[16, 4].Text = VB.Format(nView[2], "###,###");
            Spd.ActiveSheet.Cells[16, 6].Text = VB.Format(nSumView[2], "###,###");

            //MR2
            Spd.ActiveSheet.Cells[17, 4].Text = VB.Format(nView[42], "###,###");
            Spd.ActiveSheet.Cells[17, 6].Text = VB.Format(nSumView[42], "###,###");

            //UGI
            Spd.ActiveSheet.Cells[18, 4].Text = VB.Format(nView[3], "###,###");
            Spd.ActiveSheet.Cells[18, 6].Text = VB.Format(nSumView[3], "###,###");

            //UGI
            Spd.ActiveSheet.Cells[19, 4].Text = VB.Format(nView[4], "###,###");
            Spd.ActiveSheet.Cells[19, 6].Text = VB.Format(nSumView[4], "###,###");

            //Ba-Enema
            Spd.ActiveSheet.Cells[20, 4].Text = VB.Format(nView[5], "###,###");
            Spd.ActiveSheet.Cells[20, 6].Text = VB.Format(nSumView[5], "###,###");

            //I.V.P
            Spd.ActiveSheet.Cells[21, 4].Text = VB.Format(nView[6], "###,###");
            Spd.ActiveSheet.Cells[21, 6].Text = VB.Format(nSumView[6], "###,###");

            //R.I
            Spd.ActiveSheet.Cells[22, 4].Text = VB.Format(nView[7], "###,###");
            Spd.ActiveSheet.Cells[22, 6].Text = VB.Format(nSumView[7], "###,###");

            //Mammo
            Spd.ActiveSheet.Cells[23, 4].Text = VB.Format(nView[8], "###,###");
            Spd.ActiveSheet.Cells[23, 6].Text = VB.Format(nSumView[8], "###,###");

            //Mammo
            Spd.ActiveSheet.Cells[24, 4].Text = VB.Format(nView[9], "###,###");
            Spd.ActiveSheet.Cells[24, 6].Text = VB.Format(nSumView[9], "###,###");

            //B.M.D
            Spd.ActiveSheet.Cells[25, 4].Text = VB.Format(nView[10], "###,###");
            Spd.ActiveSheet.Cells[25, 6].Text = VB.Format(nSumView[10], "###,###");

            //B.M.D
            Spd.ActiveSheet.Cells[26, 4].Text = VB.Format(nView[50], "###,###");
            Spd.ActiveSheet.Cells[26, 6].Text = VB.Format(nSumView[50], "###,###");

            //SONO
            Spd.ActiveSheet.Cells[27, 4].Text = VB.Format(nView[11], "###,###");
            Spd.ActiveSheet.Cells[27, 6].Text = VB.Format(nSumView[11], "###,###");

            //SONO
            Spd.ActiveSheet.Cells[28, 4].Text = VB.Format(nView[12], "###,###");
            Spd.ActiveSheet.Cells[28, 6].Text = VB.Format(nSumView[12], "###,###");

            //Arthro
            Spd.ActiveSheet.Cells[29, 4].Text = VB.Format(nView[13], "###,###");
            Spd.ActiveSheet.Cells[29, 6].Text = VB.Format(nSumView[13], "###,###");

            //Myels
            Spd.ActiveSheet.Cells[30, 4].Text = VB.Format(nView[14], "###,###");
            Spd.ActiveSheet.Cells[30, 6].Text = VB.Format(nSumView[14], "###,###");

            //ERCP
            Spd.ActiveSheet.Cells[31, 4].Text = VB.Format(nView[16], "###,###");
            Spd.ActiveSheet.Cells[31, 6].Text = VB.Format(nSumView[16], "###,###");
            #endregion

            #region 기본 상세내역2
            
            //VCUG
            Spd.ActiveSheet.Cells[13, 11].Text = VB.Format(nView[17], "###,###");
            Spd.ActiveSheet.Cells[13, 13].Text = VB.Format(nSumView[17], "###,###");

            Spd.ActiveSheet.Cells[14, 11].Text = VB.Format(nView[18], "###,###");
            Spd.ActiveSheet.Cells[14, 13].Text = VB.Format(nSumView[18], "###,###");

            //angio
            Spd.ActiveSheet.Cells[15, 11].Text = VB.Format(nView[19], "###,###");
            Spd.ActiveSheet.Cells[15, 13].Text = VB.Format(nSumView[19], "###,###");

            Spd.ActiveSheet.Cells[16, 11].Text = VB.Format(nView[20], "###,###");
            Spd.ActiveSheet.Cells[16, 13].Text = VB.Format(nSumView[20], "###,###");

            Spd.ActiveSheet.Cells[17, 11].Text = VB.Format(nView[21], "###,###");
            Spd.ActiveSheet.Cells[17, 13].Text = VB.Format(nSumView[21], "###,###");

            Spd.ActiveSheet.Cells[18, 11].Text = VB.Format(nView[22], "###,###");
            Spd.ActiveSheet.Cells[18, 13].Text = VB.Format(nSumView[22], "###,###");

            Spd.ActiveSheet.Cells[19, 11].Text = VB.Format(nView[23], "###,###");
            Spd.ActiveSheet.Cells[19, 13].Text = VB.Format(nSumView[23], "###,###");

            Spd.ActiveSheet.Cells[20, 11].Text = VB.Format(nView[24], "###,###");
            Spd.ActiveSheet.Cells[20, 13].Text = VB.Format(nSumView[24], "###,###");

            Spd.ActiveSheet.Cells[21, 11].Text = VB.Format(nView[25], "###,###");
            Spd.ActiveSheet.Cells[21, 13].Text = VB.Format(nSumView[25], "###,###");

            Spd.ActiveSheet.Cells[22, 11].Text = VB.Format(nView[26], "###,###");
            Spd.ActiveSheet.Cells[22, 13].Text = VB.Format(nSumView[26], "###,###");

            Spd.ActiveSheet.Cells[23, 11].Text = VB.Format(nView[27], "###,###");
            Spd.ActiveSheet.Cells[23, 13].Text = VB.Format(nSumView[27], "###,###");

            Spd.ActiveSheet.Cells[24, 11].Text = VB.Format(nView[28], "###,###");
            Spd.ActiveSheet.Cells[24, 13].Text = VB.Format(nSumView[28], "###,###");

            Spd.ActiveSheet.Cells[25, 11].Text = VB.Format(nView[29], "###,###");
            Spd.ActiveSheet.Cells[25, 13].Text = VB.Format(nSumView[29], "###,###");

            //H.S.G
            Spd.ActiveSheet.Cells[26, 11].Text = VB.Format(nView[15], "###,###");
            Spd.ActiveSheet.Cells[26, 13].Text = VB.Format(nSumView[15], "###,###");

            //MAMMOTOM
            Spd.ActiveSheet.Cells[27, 11].Text = VB.Format(nView[30], "###,###");
            Spd.ActiveSheet.Cells[27, 13].Text = VB.Format(nSumView[30], "###,###"); 
            #endregion


            #endregion


            Cursor.Current = Cursors.Default;            
        }
        
    }
}
