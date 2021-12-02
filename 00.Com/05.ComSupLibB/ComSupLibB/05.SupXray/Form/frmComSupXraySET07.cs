using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET07.cs
    /// Description     : 영상의학과 혈관조영일지 등록폼
    /// Author          : 윤조연
    /// Create Date     : 2018-01-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuilgi\frmAngio2.frm(FrmAngio2) >> frmComSupXraySET07.cs 폼이름 재정의" />
    public partial class frmComSupXraySET07 : Form
    {
        #region 클래스 선언 및 etc....

        ComFunc CF = new ComFunc();
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
                
        clsComSupXraySQL.cXray_Ilgi_Angio cXray_Ilgi_Angio = null;
        clsComSupXraySQL.cXray_Ilgi_Angio_Sub cXray_Ilgi_Angio_Sub = null;

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

        public frmComSupXraySET07()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupXraySET07(MainFormMessage pform)
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
            //this.Load += new System.EventHandler(eFormLoad);
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
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (argJob != "저장")
            {
                return;
            }
            //read_sysdate();

            clsDB.setBeginTran(pDbCon);

            try
            {

                //cXray_Ilgi_Angio = new clsComSupXraySQL.cXray_Ilgi_Angio();
                //cXray_Ilgi_Angio.BDate = dtpFDate.Text.Trim();
                //cXray_Ilgi_Angio.ROWID = gROWID;
                //cXray_Ilgi_Angio.Remark = ssList.ActiveSheet.Cells[58, 3].Text.Trim();  //참고사항
                //cXray_Ilgi_Angio.Call = ssList.ActiveSheet.Cells[59, 3].Text.Trim();    //CALL
                //cXray_Ilgi_Angio.Call2 = ssList.ActiveSheet.Cells[60, 3].Text.Trim();   //CALL2
                //if (cXray_Ilgi_Angio.ROWID != "")
                //{
                //    SqlErr = cxraySql.up_XRAY_ILGI_ANGIO(pDbCon, cXray_Ilgi_Angio, ref intRowAffected);
                //}
                //else
                //{
                //    SqlErr = cxraySql.ins_XRAY_ILGI_ANGIO(pDbCon, cXray_Ilgi_Angio, ref intRowAffected);
                //}

                string STRDATE = dtpFDate.Value.ToString("yyyy-MM-dd");
                string strRemark = ssList.ActiveSheet.Cells[58, 3].Text.Trim();  //참고사항
                string strCall = ssList.ActiveSheet.Cells[59, 3].Text.Trim();    //CALL
                string strCall2 = ssList.ActiveSheet.Cells[60, 3].Text.Trim();   //CALL2
                string strROWID = "";

                int nCnt = 0;
                int nTCnt = 0;
                string strDept = "";
                string strDrName = "";
                string strDamName = "";

                string strCode = "";
                string strChange = "";


                SQL = " SELECT ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.XRAY_ILGI_ANGIO ";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE =TO_DATE('" + STRDATE + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    SQL = " UPDATE KOSMOS_PMPA.XRAY_ILGI_ANGIO SET "  ;
                    SQL = SQL + ComNum.VBLF + " REMARK = '" + strRemark + "', "  ;
                    SQL = SQL + ComNum.VBLF + " CALL = '" + strCall + "', "  ;
                    SQL = SQL + ComNum.VBLF + " CALL2 = '" + strCall2 + "' "  ;
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID ='" + strROWID + "' "  ;
                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }
                else
                {
                    SQL = " INSERT INTO KOSMOS_PMPA.XRAY_ILGI_ANGIO (BDATE,REMARK,CALL,CALL2 )  VALUES (  "  ;
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + STRDATE + "','YYYY-MM-DD') ,'" + strRemark + "', "  ;
                    SQL = SQL + ComNum.VBLF + " '" + strCall + "','" + strCall2 + "' )  "  ;

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }
                dt.Dispose();
                dt = null;

                for (int i = 11; i <= 56; i++)
                {
                    nCnt = 0;
                    nTCnt = 0;
                    strDept = "";
                    strDrName = "";
                    strDamName = "";
                    strCode = "";
                    strChange = "";
                    strROWID = "";

                    nCnt = (int)VB.Val(ssList.ActiveSheet.Cells[i, 7].Text.Trim()); 
                        
                    strDept = ssList.ActiveSheet.Cells[i, 9].Text.Trim();
                    strDrName = ssList.ActiveSheet.Cells[i, 10].Text.Trim();
                    strDamName = ssList.ActiveSheet.Cells[i, 11].Text.Trim();
                    strCode = ssList.ActiveSheet.Cells[i, 13].Text.Trim();
                    strChange = ssList.ActiveSheet.Cells[i, 14].Text.Trim();
                    strROWID = ssList.ActiveSheet.Cells[i, 15].Text.Trim();

                    if (VB.Left(strCode, 1) == "D")
                    {
                        nCnt = (int)VB.Val(ssList.ActiveSheet.Cells[i, 8].Text.Trim()); //사용
                        nTCnt = (int)VB.Val(ssList.ActiveSheet.Cells[i, 10].Text.Trim()); //입고

                        strDept = "";
                        strDrName = "";
                        strDamName = "";
                    }

                    if (strCode != "")
                    {
                        if (strROWID == "")
                        {
                            SQL = " INSERT INTO KOSMOS_PMPA.XRAY_ILGI_ANGIO_SUB (BDATE,CODE,CNT,TCNT,DEPTCODE,DRNAME,DAMNAME,SDATE,ENTDATE )  VALUES (  " ;
                            SQL = SQL + ComNum.VBLF + " TO_DATE('" + STRDATE + "','YYYY-MM-DD') ,'" + strCode + "'," + nCnt + "," + nTCnt + ",  " ;
                            SQL = SQL + ComNum.VBLF + " '" + strDept + "','" + strDrName + "','" + strDamName + "',SYSDATE,SYSDATE )  " ;
                        }
                        else
                        {
                            SQL = " UPDATE KOSMOS_PMPA.XRAY_ILGI_ANGIO_SUB SET " ;
                            SQL = SQL + ComNum.VBLF + " CNT = " + nCnt + ", " ;
                            SQL = SQL + ComNum.VBLF + " TCNT = " + nTCnt + ", " ;
                            SQL = SQL + ComNum.VBLF + " DEPTCODE = '" + strDept + "', " ;
                            SQL = SQL + ComNum.VBLF + " DRNAME = '" + strDrName + "', " ;
                            SQL = SQL + ComNum.VBLF + " DAMNAME = '" + strDamName + "', " ;
                            SQL = SQL + ComNum.VBLF + " ENTDATE = SYSDATE " ;
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' " ;
                            SQL = SQL + ComNum.VBLF + "  AND CODE ='" + strCode + "' " ;
                            SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + STRDATE + "','YYYY-MM-DD') " ;
                        }

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox("작업완료");
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

            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            //strTitle = "이상검사결과 LIST " + "(" + dtpFDate.Text.Trim() + ")";

            //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

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
            Spd.ActiveSheet.Cells[58, 3].Text = "특이사항 없습니다.";
            Spd.ActiveSheet.Cells[59, 3].Text = "특이사항 없습니다.";
            Spd.ActiveSheet.Cells[60, 3].Text = "특이사항 없습니다.";
        }

        void sheet_visible()
        {
            int i = 0;

            //컬럼visible
            for ( i = 13; i <= 15; i++)
            {
                ssList.ActiveSheet.Columns[i].Visible = false;
            }

            //ROW visible
            //심혈관계
            ssList.ActiveSheet.Rows[17].Visible = false;
            ssList.ActiveSheet.Rows[18].Visible = false;
            ssList.ActiveSheet.Rows[20].Visible = false;
            ssList.ActiveSheet.Rows[21].Visible = false;
            ssList.ActiveSheet.Rows[22].Visible = false;

            //뇌혈관계
            ssList.ActiveSheet.Rows[27].Visible = false;
            ssList.ActiveSheet.Rows[31].Visible = false;

            //
            ssList.ActiveSheet.Rows[38].Visible = false;
            ssList.ActiveSheet.Rows[39].Visible = false;

            //기타
            ssList.ActiveSheet.Rows[42].Visible = false;
            ssList.ActiveSheet.Rows[43].Visible = false;

            //조영제
            ssList.ActiveSheet.Rows[54].Visible = false;
            ssList.ActiveSheet.Rows[55].Visible = false;
            ssList.ActiveSheet.Rows[56].Visible = false;

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

        int read_SumCnt(string argJob,string argDate, string argCode)
        {
            DataTable dt = cxraySql.sel_XRAY_ILGI_ANGIO_SUB_Tot(clsDB.DbCon,argJob, argDate, argCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (argJob =="00" || argJob == "01")
                {
                    if (dt.Rows[0]["SumCnt"].ToString().Trim()!="")
                    {
                        return Convert.ToInt32(dt.Rows[0]["SumCnt"].ToString().Trim());
                    }
                    else
                    {
                        return 0;
                    }
                    
                }
                else if (argJob == "02")
                {
                    if (dt.Rows[0]["SumTCnt"].ToString().Trim() != "")
                    {
                        return Convert.ToInt32(dt.Rows[0]["SumTCnt"].ToString().Trim());
                    }
                    else
                    {
                        return 0;
                    }
                    
                }
                else
                {
                    return 0;
                }
                
            }
            else
            {
                return 0;
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


            Spd.ActiveSheet.Cells[8, 3].Text = "";
            Spd.ActiveSheet.Cells[8, 7].Text = "";
            Spd.ActiveSheet.Cells[8, 9].Text = "";
            Spd.ActiveSheet.Cells[8, 10].Text = "";
            Spd.ActiveSheet.Cells[8, 11].Text = "";


            for (i = 11; i <= 43; i++)
            {
                Spd.ActiveSheet.Cells[i, 7].Text = "";
                Spd.ActiveSheet.Cells[i, 8].Text = "";
                Spd.ActiveSheet.Cells[i, 9].Text = "";
                Spd.ActiveSheet.Cells[i, 10].Text = "";
                Spd.ActiveSheet.Cells[i, 11].Text = "";
            }

            for (i = 46; i <= 56; i++)
            {                
                Spd.ActiveSheet.Cells[i, 8].Text = "";
                Spd.ActiveSheet.Cells[i, 9].Text = "";
                Spd.ActiveSheet.Cells[i, 10].Text = "";
                Spd.ActiveSheet.Cells[i, 11].Text = "";
            }
            for (i = 58; i <= 60; i++)
            {
                Spd.ActiveSheet.Cells[i, 3].Text = "";
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
                        
            //메인쿼리 및 데이타 표시
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim());

            btnSave.Enabled = true;
            
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argDate)
        {
            int i = 0;
            DataTable dt = null;

            int nRowA = 10;
            int nRowB = 22;
            int nRowC = 31;
            int nRowD = 45;
            int nRowE = 41;
            int nRowA_Cnt = 0;
            int nRowB_Cnt = 0;
            int nRowC_Cnt = 0;
            int nRowD_Cnt = 0;
            int nRowE_Cnt = 0;

            int nRow = 0;
            int nCnt = 0;
            int nUCnt = 0;

            string strCode = "";


            Cursor.Current = Cursors.WaitCursor;

            #region //2018-06-28 안정수, 윤만식t 요청으로 상단 날짜표기부분 추가
            //상단 날짜표시
            Spd.ActiveSheet.Cells[6, 1].Text = VB.Left(dtpFDate.Text, 4) + "년 " + VB.Mid(dtpFDate.Text, 6, 2) + "월 " + VB.Right(dtpFDate.Text, 2) + "일 " + CF.READ_YOIL(clsDB.DbCon, dtpFDate.Text);
            #endregion

            #region //READ_EXAM_TITLE 

            dt = cxraySql.sel_XRAY_ILGI_ANGIO_CODE(pDbCon);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {                                      
                    strCode = dt.Rows[i]["Code"].ToString().Trim();
                    if (VB.Left(strCode,1) == "A")
                    {
                        nRowA_Cnt++;
                        nRow = nRowA + nRowA_Cnt;
                    }
                    else if (VB.Left(strCode, 1) == "B")
                    {
                        nRowB_Cnt++;
                        nRow = nRowB + nRowB_Cnt;
                    }
                    else if (VB.Left(strCode, 1) == "C")
                    {
                        nRowC_Cnt++;
                        nRow = nRowC + nRowC_Cnt;
                    }
                    else if (VB.Left(strCode, 1) == "D")
                    {
                        nRowD_Cnt++;
                        nRow = nRowD + nRowD_Cnt;
                    }
                    else if (VB.Left(strCode, 1) == "E")
                    {
                        nRowE_Cnt++;
                        nRow = nRowE + nRowE_Cnt;
                    }

                    Spd.ActiveSheet.Cells[nRow, 3].Text = dt.Rows[i]["Name"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, 13].Text = strCode;

                }
            }

            #endregion           

            #region //xray_ilgi_angio

            gROWID = "";
            dt = cxraySql.sel_XRAY_ILGI_ANGIO(pDbCon, argDate);
            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.Cells[58, 3].Text = dt.Rows[0]["REMARK"].ToString().Trim(); //참고사항
                Spd.ActiveSheet.Cells[59, 3].Text = dt.Rows[0]["CALL"].ToString().Trim();   //CALL
                Spd.ActiveSheet.Cells[60, 3].Text = dt.Rows[0]["CALL2"].ToString().Trim();  //CALL2

                gROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                
            }
            else
            {
                ComFunc.MsgBox("해당일자에 자료 저장 내역이 없습니다");
                
            }
            #endregion

            #region //검사코드별 건수 확인

            cXray_Ilgi_Angio_Sub = new clsComSupXraySQL.cXray_Ilgi_Angio_Sub();
            cXray_Ilgi_Angio_Sub.Job = "00";
            cXray_Ilgi_Angio_Sub.BDate = argDate;

            dt = cxraySql.sel_XRAY_ILGI_ANGIO_SUB(pDbCon, cXray_Ilgi_Angio_Sub);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strCode = dt.Rows[i]["Code"].ToString().Trim();

                    for (int j = 0; j < ssList.ActiveSheet.RowCount; j++)
                    {
                        if (VB.Left(strCode,1) !="D" && ssList.ActiveSheet.Cells[j,13].Text.Trim() == strCode)
                        {
                            if (dt.Rows[i]["Cnt"].ToString().Trim()!= "" && dt.Rows[i]["Cnt"].ToString().Trim() != "0")
                            {
                                ssList.ActiveSheet.Cells[j, 7].Text = dt.Rows[i]["Cnt"].ToString().Trim();
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[j, 7].Text = "";
                            }

                            ssList.ActiveSheet.Cells[j, 8].Text = ""; //누계표시

                            ssList.ActiveSheet.Cells[j, 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssList.ActiveSheet.Cells[j, 10].Text = dt.Rows[i]["DrName"].ToString().Trim();
                            ssList.ActiveSheet.Cells[j, 11].Text = dt.Rows[i]["DamName"].ToString().Trim();

                            ssList.ActiveSheet.Cells[j, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        }
                        else if (VB.Left(strCode, 1) == "D" && ssList.ActiveSheet.Cells[j, 13].Text.Trim() == strCode)
                        {
                            if (dt.Rows[i]["Cnt"].ToString().Trim() != "" && dt.Rows[i]["Cnt"].ToString().Trim() != "0")
                            {
                                ssList.ActiveSheet.Cells[j, 8].Text = dt.Rows[i]["Cnt"].ToString().Trim();
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[j, 8].Text = "";
                            }

                            if (dt.Rows[i]["TCnt"].ToString().Trim() != "" && dt.Rows[i]["TCnt"].ToString().Trim() != "0")
                            {
                                ssList.ActiveSheet.Cells[j, 10].Text = dt.Rows[i]["TCnt"].ToString().Trim();
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[j, 10].Text = "";
                            }
                            
                            
                            ssList.ActiveSheet.Cells[j, 15].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        }
                    }

                    //Spd.ActiveSheet.Cells[43, 11].Text = VB.Format(nSum[5], "###,###");
                }


            }
            #endregion

            #region //누계표시
            for (i =11; i <= 43; i++)
            {
                strCode = ssList.ActiveSheet.Cells[i, 13].Text.Trim();
                nCnt = 0;
                if (ssList.ActiveSheet.Cells[i, 7].Text.Trim() !="")
                {
                    nCnt = Convert.ToInt32(ssList.ActiveSheet.Cells[i, 7].Text.Trim());
                }
                if (strCode !="")
                {
                    if ((read_SumCnt("00", argDate, strCode) + nCnt).ToString() == "0")
                    {
                        ssList.ActiveSheet.Cells[i, 8].Text = "";
                    }

                    else
                    {
                        ssList.ActiveSheet.Cells[i, 8].Text = (read_SumCnt("00", argDate, strCode) + nCnt).ToString();
                    }

                    
                }
                
            }

            #endregion

            #region //조영제
            for (i = 46; i <= 56; i++)
            {
                strCode = ssList.ActiveSheet.Cells[i, 13].Text.Trim();
                nCnt = 0;
                nUCnt = 0;
                if (ssList.ActiveSheet.Cells[i, 8].Text.Trim() != "" && ssList.ActiveSheet.Cells[i, 8].Text.Trim() != "0")
                {
                    nCnt = Convert.ToInt32(ssList.ActiveSheet.Cells[i, 8].Text.Trim());
                }
                if (strCode != "")
                {
                    nUCnt = read_SumCnt("01", argDate, strCode) + nCnt;

                    if (nUCnt != 0)
                    {
                        ssList.ActiveSheet.Cells[i, 9].Text = nUCnt.ToString();
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[i, 9].Text = "";
                    }

                    nCnt = 0;
                    if (ssList.ActiveSheet.Cells[i, 10].Text.Trim() != "" && ssList.ActiveSheet.Cells[i, 10].Text.Trim() != "0")
                    {
                        nCnt = Convert.ToInt32(ssList.ActiveSheet.Cells[i, 10].Text.Trim());
                    }

                    if ((read_SumCnt("02", argDate, strCode) + nCnt - nUCnt).ToString() != "0")
                    {
                        ssList.ActiveSheet.Cells[i, 11].Text = (read_SumCnt("02", argDate, strCode) + nCnt - nUCnt).ToString();
                    }
                }

            }


            #endregion

            #region //건수 및 누계

            nRowA_Cnt = 0;
            nRowB_Cnt = 0;
            nRowC_Cnt = 0;            
            nRowE_Cnt = 0;

            for (i = 11; i <= 43; i++)
            {
                strCode = ssList.ActiveSheet.Cells[i, 13].Text.Trim();
                nCnt = 0;
                if (ssList.ActiveSheet.Cells[i, 7].Text.Trim() != "")
                {
                    nCnt = Convert.ToInt32(ssList.ActiveSheet.Cells[i, 7].Text.Trim());
                }
                if (VB.Left(strCode,1) == "A")
                {
                    nRowA_Cnt += nCnt;
                }
                else if (VB.Left(strCode, 1) == "B")
                {
                    nRowB_Cnt += nCnt;
                }
                else if (VB.Left(strCode, 1) == "C")
                {
                    nRowC_Cnt += nCnt;
                }
                else if (VB.Left(strCode, 1) == "E")
                {
                    nRowE_Cnt += nCnt;
                }
            }

            ssList.ActiveSheet.Cells[8, 3].Text = nRowA_Cnt.ToString();
            ssList.ActiveSheet.Cells[8, 7].Text = nRowB_Cnt.ToString();
            ssList.ActiveSheet.Cells[8, 9].Text = nRowC_Cnt.ToString();

            if (nRowA_Cnt + nRowB_Cnt + nRowC_Cnt != 0)
            {
                ssList.ActiveSheet.Cells[8, 10].Text = (nRowA_Cnt+ nRowB_Cnt+ nRowC_Cnt+nRowE_Cnt).ToString();
            }

            nCnt = 0;

            nCnt = read_SumCnt("00", argDate, strCode);

            if ( nCnt + nRowA_Cnt + nRowB_Cnt + nRowC_Cnt + nRowE_Cnt != 0)
            {
                ssList.ActiveSheet.Cells[8, 11].Text = (nCnt + nRowA_Cnt + nRowB_Cnt + nRowC_Cnt + nRowE_Cnt).ToString();
            }


            #endregion

            Cursor.Current = Cursors.Default;

        }

        private void btnSearchCall_Click(object sender, EventArgs e)
        {
            frmComSupXraySET08 frm = new frmComSupXraySET08();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }

        private void frmComSupXraySET07_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //cxraySpd.sSpd_enmXrayCVR01(ssList, cxraySpd.sSpdenmXrayCVR01, cxraySpd.nSpdenmXrayCVR01, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //툴팁
                setTxtTip();

                nView = new long[51];
                nSumView = new long[51];

                sheet_visible();

                screen_clear();

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                btnSearchCall.Visible = true;
                //screen_display();

            }
        }
    }
}
