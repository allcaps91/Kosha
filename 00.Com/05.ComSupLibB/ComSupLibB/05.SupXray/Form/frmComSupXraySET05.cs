using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET05.cs
    /// Description     : 영상의학과 이상결과보고 등록폼
    /// Author          : 윤조연
    /// Create Date     : 2017-12-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\Frm이상검사결과등록.frm(Frm이상검사결과등록) >> frmComSupXraySET05.cs 폼이름 재정의" />
    public partial class frmComSupXraySET05 : Form, MainFormMessage
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

        clsComSupXraySQL.cXrayDetail cXrayDetail = null;

        string gRowid = "";

        //스레드 여부        
        Thread thread;
        FpSpread spd;


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

        public frmComSupXraySET05()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupXraySET05(MainFormMessage pform)
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
                cxraySpd.sSpd_enmXrayCVR01(ssList, cxraySpd.sSpdenmXrayCVR01, cxraySpd.nSpdenmXrayCVR01, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //툴팁
                setTxtTip();

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
                eSave(clsDB.DbCon, "CVR등록");
                screen_display();
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

                lblInfo.Text = "";
                string s = "성명 : " + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.SName].Text.Trim() + " ";
                s += " " + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.Sex].Text.Trim() + "/";
                s += "" + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.Age].Text.Trim() + "";
                s += "( 등록번호 : " + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.Pano].Text.Trim() + " ) ";
                s += "검사일자:" + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.SeekDate].Text.Trim() + " ";
                s += " 검사명:" + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.XName].Text.Trim() + " ";

                lblInfo.Text = s;

                gRowid = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCVR01.ROWID].Text.Trim();

                if (gRowid !="")
                {
                    btnSave.Enabled = true;
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
            string strUpCols = "";
            string strWheres = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (gRowid =="")
            {
                return;
            }            

            read_sysdate();


            clsDB.setBeginTran(pDbCon);

            try
            {              
                
                if (argJob == "CVR등록")
                {
                    #region //예약변경 작업

                    if (gRowid != "" )
                    {
                        //
                        strUpCols = " CVR ='Y', CVR_Gubun='1', CVR_DATE = SYSDATE  ";
                        strWheres = " AND (CVR_DATE IS NULL OR CVR_DATE ='') ";

                        SqlErr = cxraySql.up_Xray_Detail(pDbCon,"", gRowid, "", strUpCols, strWheres, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

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
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "이상검사결과 LIST " + "(" + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

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

            cboJong.Items.Clear();
            cboJong.Items.Add("2.단순");
            cboJong.Items.Add("3.SONO");
            cboJong.Items.Add("4.CT");
            cboJong.Items.Add("5.MRI");
            cboJong.SelectedIndex = 0;
        }

        void setCtrlProgress()
        {
            Point p = new Point();

            p.X = (this.Size.Width / 2) - 90;
            if (p.X < 0)
            {
                p.X = 0;
            }
            p.Y = this.Progress.Location.Y;

            this.Progress.Location = p;

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
            read_sysdate();

            lblInfo.Text = "";
            gRowid = "";
            btnSave.Enabled = false;

            if (Job == "")
            {                

            }
            else if (Job == "A1")
            {

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
            //if (txtPano.Text.Trim() == "" && txtSName.Text.Trim() == "")
            //{
            //    ComFunc.MsgBox("등록번호 혹은 성명을 입력후 조회하십시오!!");
            //    txtPano.Select();
            //    return;
            //}

            //메인쿼리 및 데이타 표시
            GetData_th(ssList);

            screen_clear("A1");

        }

        void setSNameChk(PsmhDb pDbCon)
        {
            //DataTable dt = null;
            //cEndoJupmst.sNames_Job = "01";
            //dt = cendsSQL.sel_ENDO_JUPMST(pDbCon, cEndoJupmst.RDate);

            //cEndoJupmst.sNames_Name = "";

            //if (dt == null) return;

            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        cEndoJupmst.sNames_Name += dt.Rows[i]["SName"].ToString().Trim() + ",";
            //    }
            //}

            //dt.Dispose();
            //dt = null;
        }

        void GetData_th(FarPoint.Win.Spread.FpSpread Spd)
        {

            Spd.ActiveSheet.RowCount = 0;

            #region //Biz 쿼리 변수세팅 및 쿼리실행

            cXrayDetail = new clsComSupXraySQL.cXrayDetail();
            cXrayDetail.Job = "04"; //이상결과 관리 CVR
            cXrayDetail.XJong = clsComSup.setP(cboJong.SelectedItem.ToString(), ".", 1);
            cXrayDetail.Date1 = dtpFDate.Text.Trim();
            cXrayDetail.Date2 = dtpTDate.Text.Trim();
            cXrayDetail.GbCVR_Gubun1 = "비대상";
            if (optCVR2.Checked == true)
            {
                cXrayDetail.GbCVR_Gubun1 = "대상";
            }
            cXrayDetail.GbCVR_Gubun2 = "촬영자";
            if (optGubun2.Checked == true)
            {
                cXrayDetail.GbCVR_Gubun2 = "판독의";
            }
            
            spd = ssList;


            #endregion

            //동명이인관련 체크
            setSNameChk(clsDB.DbCon);

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;
            //cEndoJupmst.sNames_Job = "00";
            dt = cxraySql.sel_XrayDetail(clsDB.DbCon, cXrayDetail, "", " Pano, SeekDate ");

            this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt);

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd, DataTable dt);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd, DataTable dt)
        {
            string strXName = "";
            spd.ActiveSheet.RowCount = 0;

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                              
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strXName = dt.Rows[i]["OrderName"].ToString().Trim();
                    //if (strXName == "")
                    //{
                    //    strXName = dt.Rows[i]["XName"].ToString().Trim();
                    //}
                    //if (strXName != dt.Rows[i]["Remark"].ToString().Trim())
                    //{
                    //    strXName += " " + dt.Rows[i]["Remark"].ToString().Trim();
                    //}


                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.chk].Text = "";
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.GbIO].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.SeekDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.XCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.XName].Text = strXName;
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.Exid].Text = dt.Rows[i]["Exid"].ToString().Trim();

                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.DrName].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR].Text = dt.Rows[i]["CVR"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Date].Text = dt.Rows[i]["CVR_Date"].ToString().Trim();

                    if (optCVR2.Checked == true)
                    {
                        if (optGubun1.Checked == true)
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Name1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["Exid"].ToString().Trim());
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Name2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_CDate].Text = dt.Rows[i]["CVR_CDate"].ToString().Trim();
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Gubun].Text = "문자전송";
                        }
                        else if (optGubun2.Checked == true)
                        {
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Name1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["CVR_DRSABUN"].ToString().Trim());
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Name2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Send].Text = dt.Rows[i]["CVR_Send"].ToString().Trim();
                            spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.CVR_Gubun].Text = "유선보고";
                        }
                    }
                                        
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCVR01.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    
                }

                // 화면상의 정렬표시 Clear
                spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;


        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            this.Progress.Visible = b;
            this.Progress.IsRunning = b;
        }


        #endregion



    }
}
