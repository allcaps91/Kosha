using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsVIEW04.cs
    /// Description     : 내시경관리 
    /// Author          : 윤조연
    /// Create Date     : 2017-09-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endo_new\Frm내시경통합장부.frm(Frm내시경통합장부) >> frmComSupEndsVIEW04.cs 폼이름 재정의" />
    public partial class frmComSupEndsVIEW04 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupEndsSpd cendsSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL cendsSQL = new clsComSupEndsSQL();

        clsComSupEndsSQL.cEndoJupmst cEndoJupmst = null;

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

        public frmComSupEndsVIEW04()
        {
            InitializeComponent();

            setEvent();
        }

        public frmComSupEndsVIEW04(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {



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
            this.btnPrint1.Click += new EventHandler(eBtnPrint);
            this.btnPrint2.Click += new EventHandler(eBtnPrint);
            this.btnSave.Click += new EventHandler(eBtnSave);


            //명단 더블클릭 이벤트
            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
            this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);


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
                cendsSpd.sSpd_enmTotalView(ssList, cendsSpd.sSpdTotalView, cendsSpd.nSpdTotalView, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


                //툴팁
                setTxtTip();


                screen_clear();

                setCtrlData();

                screen_display();

            }

        }

        void eFormResize(object sender, EventArgs e)
        {
            //setCtrlProgress();
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
                eSave(clsDB.DbCon);
            }

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint1)
            {
                ePrint("");
            }
            else if (sender == this.btnPrint2)
            {
                ePrint("선택");
            }

        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (e.ColumnHeader == true && e.Column != 0 )
            {
                clsSpread.gSpdSortRow(o, e.Column);
                return;
            }
            else if (sender == this.ssList)
            {
                //string strPano = o.ActiveSheet.Cells[o.ActiveSheet.ActiveRowIndex, (int)clsSupEndsSpd.enmSupEndsSCH02.Pano].Text.Trim();
                //string strROWID = o.ActiveSheet.Cells[o.ActiveSheet.ActiveRowIndex, (int)clsSupEndsSpd.enmSupEndsSCH02.ROWID].Text.Trim();

                //if (e.Column == (int)clsSupEndsSpd.enmSupEndsSCH02.RWaitDate)  //예약대기일자 변경
                //{
                    
                //}

                if(e.Column == 0)
                {
                    for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if(ssList_Sheet1.Cells[i, 0].Text == "True")
                        {
                            ssList_Sheet1.Cells[i, 0].Text = "False";
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 0].Text = "True";
                        }
                        
                    }
                }


            }
        }

        void eSpreadEditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            FpSpread o = (FpSpread)sender;

            if (e.Column == (int)clsComSupEndsSpd.enmTotalView.RTime || e.Column == (int)clsComSupEndsSpd.enmTotalView.Nurse)
            {
                o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmTotalView.Change].Text = "Y";
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

            e.TipText = ssList.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
            e.ShowTip = true;
        }

        void eSave(PsmhDb pDbCon)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strUpCols = "";
            int nstartRow = 0;
            int nLastRow = -1;
            string strTime = "";
            string strNurse = "";
            string strROWID = "";

            read_sysdate();

            nLastRow = ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                for (i = nstartRow; i < nLastRow; i++)
                {
                    if (ssList_Sheet1.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Change].Text.Trim() == "Y")
                    {
                        #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅 >> 자료저장

                        strTime = ssList_Sheet1.Cells[i, (int)clsComSupEndsSpd.enmTotalView.RTime].Text.Trim();
                        strNurse = ssList_Sheet1.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Nurse].Text.Trim();
                        strROWID = ssList_Sheet1.Cells[i, (int)clsComSupEndsSpd.enmTotalView.ROWID].Text.Trim();

                        if (strTime !="" && VB.Len( strTime) != 11 )
                        {
                            ComFunc.MsgBox("시간형식을 확인하세요!! ex)10:30~11:20 ");
                            break;
                        }

                        //예약변경
                        strUpCols = " GUBUN_TIME = '" + strTime + "' , GUBUN_Nurse ='" + strNurse + "'  ";
                        SqlErr = cendsSQL.up_ENDO_JUPMST(pDbCon, strROWID,"", strUpCols, "", ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        #endregion
                    }
                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

            //조회            
            screen_display();

        }

        void ePrint(string argJob)
        {
            int i = 0;

            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;
            
            string strJong = "";

            if (argJob =="")
            {
                #region //시트 히든

                ssList.ActiveSheet.Columns[(int)clsComSupEndsSpd.enmTotalView.chk].Visible = false;


                #endregion
            }
            else if(argJob =="선택")
            {
                ssList.ActiveSheet.Columns[(int)clsComSupEndsSpd.enmTotalView.chk].Visible = false;

                for (i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, (int) clsComSupEndsSpd.enmTotalView.chk].Text == "")
                    {
                        ssList.ActiveSheet.Rows[i].Visible = false;
                    }
                }
            }

            if (optJong1.Checked ==true)
            {
                strJong = "합병증";
            }
            else if (optJong2.Checked == true)
            {
                strJong = "응급환자";
            }

            strTitle = "내 시 경 장 부 " + "[" + strJong + "]" + "(" + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            if (argJob == "")
            {
                #region //시트 히든 복원

                ssList.ActiveSheet.Columns[(int)clsComSupEndsSpd.enmTotalView.chk].Visible = true;


                #endregion
            }
            else if (argJob == "선택")
            {
                ssList.ActiveSheet.Columns[(int)clsComSupEndsSpd.enmTotalView.chk].Visible = true;
                for (i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.chk].Text == "")
                    {
                        ssList.ActiveSheet.Rows[i].Visible = true;
                    }
                }

            }

        }                

        void screen_clear(string Job = "")
        {
            read_sysdate();

            //txtSearch.Text = "";
            //txtSogen.Text = "";

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string strJob = "";
            string strTPro = "";
                        

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            #region //Biz 쿼리 변수세팅 및 쿼리실행
            cEndoJupmst = new clsComSupEndsSQL.cEndoJupmst();
            cEndoJupmst.STS = "4";
            cEndoJupmst.Job = "*";
            if (optJong0.Checked == true)
            {
                cEndoJupmst.Part2 = "0";
            }
            else if (optJong1.Checked == true)
            {
                cEndoJupmst.Part2 = "1";
            }
            else if (optJong2.Checked == true)
            {
                cEndoJupmst.Part2 = "2";
            }
            else if (optJong3.Checked == true)
            {
                cEndoJupmst.Part2 = "3";
            }
            cEndoJupmst.Gubun = chkEnd.Checked.ToString() == "True" ? "1" : "*";
            cEndoJupmst.Date1 = argSDate;
            cEndoJupmst.Date2 = argTDate;            

            #endregion

            dt = cendsSQL.sel_ENDO_JUPMST(pDbCon, cEndoJupmst,false);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT + 5);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strJob = dt.Rows[i]["GbJob"].ToString().Trim();
                    strTPro = "";

                    #region // 내역담기

                    if(dt.Rows[i]["PRO_BX1"].ToString().Trim()=="Y")
                    {
                        strTPro += "Bx.bottle ";
                    }
                    if (dt.Rows[i]["PRO_BX2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_BX2"].ToString().Trim() + "ea, ";
                    }
                    if (dt.Rows[i]["PRO_PP1"].ToString().Trim() == "Y")
                    {
                        strTPro += "PP ";
                    }
                    if (dt.Rows[i]["PRO_PP2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_PP2"].ToString().Trim() + "ea, ";
                    }

                    if (dt.Rows[i]["PRO_ESD1"].ToString().Trim() == "Y")
                    {
                        strTPro += "ESD, ";
                    }
                    if (dt.Rows[i]["PRO_ESD2"].ToString().Trim() == "Y")
                    {
                        strTPro += "en-bloc, ";
                    }
                    if (dt.Rows[i]["PRO_ESD3_1"].ToString().Trim() == "Y")
                    {
                        strTPro += "piecemeal ";
                    }
                    if (dt.Rows[i]["PRO_ESD3_2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_ESD3_2"].ToString().Trim() + ", " + "\r\n";
                    }

                    if (dt.Rows[i]["PRO_EMR1"].ToString().Trim() == "Y")
                    {
                        strTPro += "EMR, ";
                    }
                    if (dt.Rows[i]["PRO_EMR2"].ToString().Trim() == "Y")
                    {
                        strTPro += "en-bloc, ";
                    }
                    if (dt.Rows[i]["PRO_EMR3_1"].ToString().Trim() == "Y")
                    {
                        strTPro += "piecemeal ";
                    }
                    if (dt.Rows[i]["PRO_EMR3_2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_EMR3_2"].ToString().Trim() + ", " + "\r\n";
                    }

                    if (dt.Rows[i]["PRO_APC"].ToString().Trim() == "Y")
                    {
                        strTPro += "APC, ";
                    }
                    if (dt.Rows[i]["PRO_ELEC"].ToString().Trim() == "Y")
                    {
                        strTPro += "Electrocauterization, ";
                    }

                    if (dt.Rows[i]["PRO_HEMO1"].ToString().Trim() == "Y")
                    {
                        strTPro += "Hemoclip ";
                    }
                    if (dt.Rows[i]["PRO_HEMO2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_HEMO2"].ToString().Trim() + "ea, " + "\r\n";
                    }

                    if (dt.Rows[i]["PRO_EPNA1"].ToString().Trim() == "Y")
                    {
                        strTPro += "EPNA ";
                    }
                    if (dt.Rows[i]["PRO_EPNA2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_EPNA2"].ToString().Trim() + "cc, "  ;
                    }

                    if (dt.Rows[i]["PRO_MBAND"].ToString().Trim() == "Y")
                    {
                        strTPro += "multi-band, ";
                    }

                    if (dt.Rows[i]["PRO_EST"].ToString().Trim() == "Y")
                    {
                        strTPro += "EST ( ";
                    }
                    if (dt.Rows[i]["PRO_EST_STS"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_EST_STS"].ToString().Trim() +") ";
                    }

                    if (strJob =="3")
                    {
                        if (dt.Rows[i]["PRO_BAND1"].ToString().Trim() == "Y")
                        {
                            strTPro += "band ";
                        }
                    }
                    else
                    {
                        if (dt.Rows[i]["PRO_BAND1"].ToString().Trim() == "Y")
                        {
                            strTPro += "Single-band ";
                        }
                    }
                    if (dt.Rows[i]["PRO_BAND2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_BAND2"].ToString().Trim() + "ea, ";
                    }

                    if (dt.Rows[i]["PRO_HIST1"].ToString().Trim() == "Y")
                    {
                        strTPro += "Histoacyl ";
                    }
                    if (dt.Rows[i]["PRO_HIST2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_HIST2"].ToString().Trim() + "ample, ";
                    }

                    if (dt.Rows[i]["PRO_DETA"].ToString().Trim() == "Y")
                    {
                        strTPro += "Detachable snare, ";
                    }

                    if (dt.Rows[i]["PRO_BALL"].ToString().Trim() == "Y")
                    {
                        strTPro += "Ballooon, ";
                    }
                    if (dt.Rows[i]["PRO_BASKET"].ToString().Trim() == "Y")
                    {
                        strTPro += "Basket, ";
                    }

                    if (dt.Rows[i]["PRO_EPBD1"].ToString().Trim() == "Y")
                    {
                        strTPro += "EPBD ";
                    }
                    if (dt.Rows[i]["PRO_EPBD2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_EPBD2"].ToString().Trim() +"mm ";
                    }
                    if (dt.Rows[i]["PRO_EPBD3"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_EPBD3"].ToString().Trim() + "atm ";
                    }
                    if (dt.Rows[i]["PRO_EPBD4"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_EPBD4"].ToString().Trim() + "sec" + "\r\n";
                    }

                    if (dt.Rows[i]["PRO_ENBD1"].ToString().Trim() == "Y")
                    {
                        strTPro += "ENBD ";
                    }
                    if (dt.Rows[i]["PRO_ENBD2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_ENBD2"].ToString().Trim() + "Fr. ";
                    }
                    if (dt.Rows[i]["PRO_ENBD3"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_ENBD3"].ToString().Trim() + "type ";
                    }

                    if (dt.Rows[i]["PRO_ERBD1"].ToString().Trim() == "Y")
                    {
                        strTPro += "ERBD ";
                    }
                    if (dt.Rows[i]["PRO_ERBD2"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_ERBD2"].ToString().Trim() + "Fr. ";
                    }
                    if (dt.Rows[i]["PRO_ERBD3"].ToString().Trim() != "")
                    {
                        strTPro += dt.Rows[i]["PRO_ERBD3"].ToString().Trim() + "type ";
                    }

                    strTPro = strTPro.Trim();

                    #endregion                                        

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.RDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Ptno].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Age].Text = dt.Rows[i]["FC_Age"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.GBIO].Text = dt.Rows[i]["GbIO2"].ToString().Trim();
                    if (dt.Rows[i]["RoomCode"].ToString().Trim() !="")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Buse].Text = dt.Rows[i]["DeptCode"].ToString().Trim() +"(" + dt.Rows[i]["RoomCode"].ToString().Trim() + ")";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Buse].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.RTime].Text = dt.Rows[i]["Gubun_Time"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.DrCode].Text = dt.Rows[i]["ResultDrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.DrName].Text = clsVbfunc.GetInSaName( pDbCon, dt.Rows[i]["ResultDrCode"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Nurse].Text = dt.Rows[i]["Gubun_Nurse"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    dt2 = cendsSQL.sel_ENDO_RESULT(pDbCon, Convert.ToInt32(dt.Rows[i]["SEQNO"].ToString().Trim()));
                    if (ComFunc.isDataTableNull(dt2) == false)
                    {
                        if (strJob =="2")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Diag].Text = dt2.Rows[0]["REMARK4"].ToString().Trim();
                            if (strTPro!="")
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.OrderName].Text = strTPro +  "\r\n" + dt2.Rows[0]["REMARK5"].ToString().Trim();
                            }
                            else
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.OrderName].Text = dt2.Rows[0]["REMARK5"].ToString().Trim();
                            }
                        }
                        else if (strJob == "3")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Diag].Text = dt2.Rows[0]["REMARK2"].ToString().Trim();
                            if (strTPro != "")
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.OrderName].Text = strTPro + "\r\n" + dt2.Rows[0]["REMARK3"].ToString().Trim();
                            }
                            else
                            {
                                Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.OrderName].Text = dt2.Rows[0]["REMARK3"].ToString().Trim();
                            }
                        }
                        else if (strJob == "4")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Diag].Text = dt2.Rows[0]["REMARK2"].ToString().Trim();
                            Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.OrderName].Text = strTPro;
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.Diag].Text = dt2.Rows[0]["REMARK4"].ToString().Trim();
                            Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmTotalView.OrderName].Text = strTPro;
                        }

                    }
                                        
                    Spd.ActiveSheet.Rows.Get(i).Height = Spd.ActiveSheet.Rows[i].GetPreferredHeight();

                }


                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
                
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion

        }
    }

}
