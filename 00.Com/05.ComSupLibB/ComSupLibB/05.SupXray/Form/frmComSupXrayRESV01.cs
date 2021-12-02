using ComBase;
using ComDbB;
using ComSupLibB.Com;
using ComSupLibB.SupFnEx;
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
    /// File Name       : frmComSupXrayRESV01.cs
    /// Description     : 영상의학 촬영예약변경 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-12-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\xumain09.frm(FrmYeyakChange) >> frmComSupXrayRESV01.cs 폼이름 재정의" />
    public partial class frmComSupXrayRESV01 : Form, MainFormMessage
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

        frmComSupFnExPOP01 frmComSupFnExPOP01x = null; //예약변경시 달력사용

        string gPart = "";              

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

        public frmComSupXrayRESV01(string argPart)
        {
            InitializeComponent();
            gPart = argPart;
            setEvent(); 
        }

        public frmComSupXrayRESV01(MainFormMessage pform,string argPart)
        {
            InitializeComponent();
            this.mCallForm = pform;
            gPart = argPart;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {            
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

            //this.btnPrint.Click += new EventHandler(eBtnPrint);


            //명단 더블클릭 이벤트
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);                        
            //this.ssList.EditModeOff += new EventHandler(eSpreadEditOff);
            this.ssList.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
            this.ssList.ComboSelChange += new EditorNotifyEventHandler(eSpreadCboSelChange);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);


            this.txtSearch.KeyDown += new KeyEventHandler(eTxtKeyDown);            
            //this.dtpDate.ValueChanged += new EventHandler(eDtpValueChanged);


        }

        void setAuth()
        {
            
            if (gPart == "ALL")
            {
                panTitleSub0.Visible = true;

                
                btnExit.Visible = true;
                panel6.Visible = false;
                panel14.Visible = true;

                //
                ssList.ActiveSheet.Columns[(int)clsComSupXraySpd.enmXrayResv01.XRoomChange].Visible = true;
                this.Size = new Size(980, 590);

            }
            else if (gPart == "00")
            {
                panTitleSub0.Visible = false;

                
                btnExit.Visible = false;
                panel6.Visible = true;
                panel14.Visible = false;

                ssList.ActiveSheet.Columns[(int)clsComSupXraySpd.enmXrayResv01.XRoomChange].Visible = true;

                gPart = "ALL"; //

            }
            else
            {
                panTitleSub0.Visible = true;
                                
                cboJob.Enabled = false;
                btnExit.Visible = true;
                panel6.Visible = false;
                panel14.Visible = true;
                                
            }
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
                cxraySpd.sSpd_enmXrayResv01(ssList, cxraySpd.sSpdenmXrayResv01, cxraySpd.nSpdenmXrayResv01, 3, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                               
                //툴팁
                setTxtTip();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                setAuth();

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
                eSave(clsDB.DbCon, "예약변경");
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {

            //if (sender == this.btnPrint)
            //{
            //    ePrint();
            //}

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

            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                
            }
            else
            {
                if (sender == this.ssList)
                {
                    
                }

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
                if (e.Column ==  (int)clsComSupXraySpd.enmXrayResv01.SeekDate || e.Column == (int)clsComSupXraySpd.enmXrayResv01.SeekTime)
                {                    
                    #region 메뉴 더블클릭시 델리게이트용 폼 팝업

                    string[] argfrm = new string[Enum.GetValues(typeof(clsComSupFnEx.enmfrmComSupFnExPOP01)).Length];
                    argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.Part] = "Xray";
                    argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.STS] = "DClick";
                    argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.RDate] = o.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupXraySpd.enmXrayResv01.SeekDate].Text.Trim();
                    argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.RTime] = ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupXraySpd.enmXrayResv01.SeekTime].Text.Trim();

                    if (frmComSupFnExPOP01x == null)
                    {
                        frmComSupFnExPOP01x = new frmComSupFnExPOP01(argfrm);
                        frmComSupFnExPOP01x.rSendMsg += new frmComSupFnExPOP01.SendMsg(frmComSupFnExPOP01x_SendMsg);
                    }
                    else
                    {
                        frmComSupFnExPOP01x = null;
                        frmComSupFnExPOP01x = new frmComSupFnExPOP01(argfrm);
                        frmComSupFnExPOP01x.rSendMsg += new frmComSupFnExPOP01.SendMsg(frmComSupFnExPOP01x_SendMsg);
                    }

                    #endregion
                    frmComSupFnExPOP01x.ShowDialog();                    
                }

                

            }
        }

        #region //델리게이트
        void frmComSupFnExPOP01x_SendMsg(string[] str)
        {            
            if (str[(int)clsComSupFnEx.enmRDateTime.OK] == "OK" && str[(int)clsComSupFnEx.enmRDateTime.Part] == "Xray")
            {
                if (str[(int)clsComSupFnEx.enmRDateTime.STS] == "DClick") //시트에 값변경
                {
                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupXraySpd.enmXrayResv01.SeekDate].Text = str[(int)clsComSupFnEx.enmRDateTime.RDate];
                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupXraySpd.enmXrayResv01.SeekTime].Text = str[(int)clsComSupFnEx.enmRDateTime.RTime];

                    ssList.ActiveSheet.Rows.Get(ssList.ActiveSheet.ActiveRowIndex).BackColor = System.Drawing.Color.LightGreen;
                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupXraySpd.enmXrayResv01.Change].Text = "Y";

                }
            }
            else
            {
                //ComFunc.MsgBox("예약변경 안됨!!");
            }
        }
        #endregion

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

            if (e.Column == (int)clsComSupXraySpd.enmXrayResv01.chk)
            {
                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
            }

        }

        void eSpreadEditChange(object sender,EditorNotifyEventArgs e )
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (sender == this.ssList)
            {
                if (e.Column == (int)clsComSupXraySpd.enmXrayResv01.SeekDate || e.Column == (int)clsComSupXraySpd.enmXrayResv01.SeekTime)
                {
                    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
                    o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayResv01.Change].Text = "Y";
                }
            }
            
        }

        void eSpreadCboSelChange(object sender, EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (sender == this.ssList)
            {
                
                if (e.Column == (int)clsComSupXraySpd.enmXrayResv01.XRoomChange && o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayResv01.XRoomOld].Text != VB.Left(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayResv01.XRoomChange].Text, 1))
                {
                    //o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayResv01.XRoom].Text = VB.Left(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayResv01.XRoomChange].Text.Trim(), 1);

                    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
                    o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayResv01.Change].Text = "Y";

                }
            }
            
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
            if (sender == this.txtSearch)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtSearch.Text.Trim() != "")
                    {
                        txtSearch.Text = ComFunc.SetAutoZero(txtSearch.Text, ComNum.LENPTNO);
                        screen_display();
                    }
                }
            }
        }

        void eDtpValueChanged(object sender, EventArgs e)
        {
            //조회
            //screen_display();
        }

        void eSave(PsmhDb pDbCon, string argJob, int argRow = -1)
        {
            int i = 0;
            int nstartRow = 0;
            int nLastRow = -1;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strUpCols = "";
            //string strWheres = "";
            int intRowAffected = 0; //변경된 Row 받는 변수
          
            string strDateNew = "";
            string strDateOld = "";
            string strTimeNew = "";
            string strTimeOld = "";
            string strXRoomNew = "";
            string strXRoomOld = "";
       
            string strROWID = "";


            read_sysdate();

            if (sheet_sel_chk_msg(ssList) == false)
            {
                return;
            }

            if (argRow == -1)
            {
                nLastRow = ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
            }
            else
            {
                nstartRow = argRow;
                nLastRow = argRow + 1;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                for (i = nstartRow; i < nLastRow; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.ROWID].Text.Trim() !="" || ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Change].Text.Trim() == "Y" || argRow != -1)
                    {
                        #region // 변수세팅 및 자료 저장

                        strDateNew = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekDate].Text.Trim();
                        strDateOld = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekDateOld].Text.Trim();
                        strTimeNew = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekTime].Text.Trim();
                        strTimeOld = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekTimeOld].Text.Trim();
                        if (ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XRoomChange].Text.Trim() !="")
                        {
                            strXRoomNew =  VB.Left(ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XRoomChange].Text.Trim(),1);
                        }
                        else
                        {
                            strXRoomNew = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XRoom].Text.Trim();
                        }
                        
                        strXRoomOld = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XRoomOld].Text.Trim();


                        strROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.ROWID].Text.Trim();

                        #endregion

                        if (argJob == "예약변경")
                        {
                            #region //예약변경 작업
                                                        
                            if ( strROWID !="" &&  (strDateNew != strDateOld || strTimeNew != strTimeOld || strXRoomNew != strXRoomOld))
                            {                               
                                                                
                                //
                                strUpCols = " GbSTS ='2'  ";
                                strUpCols += " , SeekDate =TO_DATE('" + strDateNew + " " + strTimeNew + "','YYYY-MM-DD HH24:MI') ";
                                strUpCols += " , RDate =TO_DATE('" + strDateNew + " " + strTimeNew + "','YYYY-MM-DD HH24:MI') ";
                                if (strXRoomNew != strXRoomOld)
                                {
                                    strUpCols += " , XrayRoom ='" + strXRoomNew + "' ";
                                }
                                SqlErr = cxraySql.up_Xray_Detail(pDbCon,"", strROWID, "", strUpCols, "", ref intRowAffected);
                              
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }
                                else
                                {
                                    //
                                    SqlErr = cxraySql.ins_XRAY_PACSSEND(pDbCon, "06", "3", strROWID, clsType.User.IdNumber, ref intRowAffected);

                                    if(SqlErr != "")
                                    { 
                                        clsDB.setRollbackTran(pDbCon);
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                        return;
                                    }
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

                    }
                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("작업완료");
                    screen_display();
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
            //clsSpread SPR = new clsSpread();
            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;

            //string strTitle = "";
            //string strHeader = "";
            //string strFooter = "";
            //bool PrePrint = true;


            //#region //시트 히든

            ////ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            //#endregion

            //strTitle = "내시경 예약자 명단 " + "(" + dtpDate.Text.Trim() + ")";

            //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            //SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            //#region //시트 히든 복원

            ////ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            //#endregion

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
            string s = string.Empty;

            if (gPart !="")
            {                
                s = " AND Gubun2 ='" + gPart + "' ";
            }
            else
            {
                ComFunc.MsgBox("폼 권한값 필요!!");
                return;
            }
            
            //작업구분          
            method.setCombo_View(this.cboJob, Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_RESV_PART", "", " Code|| '.' || Name Names ", s, " Sort ASC, Code ASC "), clsParam.enmComParamComboType.None);          

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
                if (Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Change].Text == "Y")
                {
                    sts = true;
                    return sts;
                }
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
                ComFunc.MsgBox("예약변경건이 없습니다..", "확인");
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
            if (Job == "")
            {
                txtSearch.Text = "";               
                
            }
            else if (Job == "A1")
            {
               
            }

            //환자공통정보 표시                   
            cinfo = new clsComSup.SupPInfo();

            txtSearch.Select();

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        public void display_pano_view(string argPano)
        {
            optPano.Checked = true;
            txtSearch.Text = argPano;            
            screen_display();
        }

        void screen_display()
        {
            if (txtSearch.Text.Trim() =="")
            {
                ComFunc.MsgBox("등록번호 혹은 성명을 입력후 조회하십시오!!");
                txtSearch.Select();
                return;
            }

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
            cXrayDetail.Job = "03"; //예약관리
            cXrayDetail.XJong = "*";
            cXrayDetail.RDate = cpublic.strSysDate;
            if (cboJob.Text.Trim() =="002.외과")
            {
                cXrayDetail.XJong = "B";
            }
            else if (cboJob.Text.Trim() == "003.정형외과")
            {
                cXrayDetail.XJong = "I";
            }
            else if (cboJob.Text.Trim() == "004.비뇨기과")
            {
                cXrayDetail.XJong = "A";
            }
            else if (cboJob.Text.Trim() == "005.흉부외과")
            {
                cXrayDetail.XJong = "3";
            }
            if (optPano.Checked == true)
            {
                cXrayDetail.Pano = txtSearch.Text.Trim();
            }
            else if (optSName.Checked == true)
            {
                cXrayDetail.SName = txtSearch.Text.Trim();
            }
            if (chkResv.Checked == true)
            {
                cXrayDetail.GbResvChk = "ALL";
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
            dt = cxraySql.sel_XrayDetail(clsDB.DbCon, cXrayDetail,"", " Pano, SeekDate ");

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

                //public string[] sSpdenmXrayResv01 = { "선택","등록번호","성명","나이","성별"
                //                             ,"검사코드","검사명","촬영일","촬영시간","촬영실"
                //                             ,"I/O","과","의사코드","병동","호실"
                //                             ,"촬영기사","촬영일old","촬영시간old","종류","호실old"
                //                             ,"ROWID"
                //                                };

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strXName = dt.Rows[i]["OrderName"].ToString().Trim();
                    
                    if (strXName != dt.Rows[i]["Remark"].ToString().Trim())
                    {
                        strXName += " " + dt.Rows[i]["Remark"].ToString().Trim();
                    }

                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.chk].Text = "";
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XName].Text = strXName;
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekDate].Text = dt.Rows[i]["SeekDate_Date"].ToString().Trim();
                    sup.setColStyle_Text(spd, i, (int)clsComSupXraySpd.enmXrayResv01.SeekDate, false, false, false, 10);
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekTime].Text = dt.Rows[i]["SeekDate_Time"].ToString().Trim();
                    sup.setColStyle_Text(spd, i, (int)clsComSupXraySpd.enmXrayResv01.SeekTime, false, false, false, 5);
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XRoom].Text = dt.Rows[i]["XrayRoom"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.GbIO].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.WardCode].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.RoomCode].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Exid].Text = dt.Rows[i]["Exid"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekDateOld].Text = dt.Rows[i]["SeekDate_Date"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.SeekTimeOld].Text = dt.Rows[i]["SeekDate_Time"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XRoomOld].Text = dt.Rows[i]["XrayRoom"].ToString().Trim();

                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.XJong].Text = dt.Rows[i]["XJong"].ToString().Trim();
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.Change].Text = "";
                    spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayResv01.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();


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
            //this.Progress.Visible = b;
            //this.Progress.IsRunning = b;
        }


        #endregion


    }
}
