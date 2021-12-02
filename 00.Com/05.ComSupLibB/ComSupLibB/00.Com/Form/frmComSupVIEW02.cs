using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupVIEW02.cs
    /// Description     : 컨설트 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-09-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// 
    /// </history>
    /// <seealso cref= "\.frm >> frmComSupVIEW02.cs 폼이름 재정의" />
    public partial class frmComSupVIEW02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        clsComSup sup = new clsComSup();
        clsComSupSpd cSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsComSup.SupPInfo cinfo = new clsComSup.SupPInfo();

        clsComSup.cOcsItransfer cOcsItransfer = null;
                
        string gPano = "";
        string gDept = "";
        string gDrCode = "";
        bool gShow = false;
        int gDay = 0;

        #endregion
                
        public frmComSupVIEW02(string argPano ="",string argDept="",string argDrCode ="", bool bShow = false, int argDay=60)
        {
            InitializeComponent();

            gShow = bShow;
            gPano = argPano;
            gDept = argDept;
            gDrCode = argDrCode;
            gDay = argDay;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            setCombo(pDbCon, gDept);

            dtpFDate.Text =  Convert.ToDateTime(cpublic.strSysDate).AddDays(-15).ToShortDateString();
            dtpTDate.Text = cpublic.strSysDate;

            ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.Return].Visible = false;

            //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.toDept].Visible = false;
            //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.toDrCode].Visible = false;

            txtPtno.Text = gPano;

            if (gShow ==false)
            {
                cboDept.Enabled = false;
                txtPtno.Visible = false;
                lblPtno.Visible = false;
            }
            
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            //버튼 이벤트
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnPrint.Click += new EventHandler(eBtnPrint);
            
            this.btnSearch1.Click += new EventHandler(eBtnSearch); //기본 마스터 조회            
            this.btnSearch2.Click += new EventHandler(eBtnSearch); //기타결과           


            //명단 더블클릭 이벤트
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);            
            this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
                                    
            this.dtpFDate.TextChanged += new EventHandler(eDtpTxtChange);

            this.cboDept.TextChanged += new EventHandler(eCboChange);

            this.optJob1.CheckedChanged += new EventHandler(eOptChange);
            

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
                cSpd.sSpd_ConsultList(ssList, cSpd.sSpdConsultList, cSpd.nSpdConsultList, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData(clsDB.DbCon);

                screen_display();
                screen_clear("A");

            }            

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                //조회                
                screen_display();
                screen_clear("A");
            }     
            else if (sender == this.btnSearch2)
            {
                //기타결과  
                clsPublic.GstrHelpCode = "";//add                              
                frmViewResult f = new frmViewResult(cinfo.strPano);
                f.ShowDialog();

            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            
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

        void eCboChange(object sender, EventArgs e)
        {
            ComboBox o = (ComboBox)sender;
            
            //조회
            try
            {
                if (o.Items.Count > 0)
                {
                    if (sender == this.cboDept)
                    {
                        if (o.SelectedItem.ToString() != null)
                        {
                            method.setCombo_View(cboDr, sup.sel_BAS_DOCTOR(clsDB.DbCon, o.SelectedItem.ToString()), clsParam.enmComParamComboType.ALL);
                        }
                        else
                        {

                        }
                        
                    }
                    
                }

            }
            catch
            {

            }
                        

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;
            string s = "";

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            //
            read_cinfo(o, e.Row);

            for (int i = 0; i < o.ActiveSheet.RowCount; i++)
            {
                o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
            }

            o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;


            
            lblInfo.Text = "";

            s += "등록번호:" + o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.Ptno].Text.Trim();
            s += " 성명:" + o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.SName].Text.Trim();
            s += " (" + o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.Sex].Text.Trim() +") ";

            if (optJob1.Checked == true)
            {
                s += "  [ " + o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.DeptCode].Text.Trim();
                s += o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.FrDrCode].Text.Trim() + " ▶ " + o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.toDrCode].Text.Trim() +" ] ";
            }
            else
            {
                s += "  [ " + o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.FrDrCode].Text.Trim() + " ▶ " +  o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.toDept].Text.Trim();
                s += o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.toDrCode].Text.Trim() + " ] ";
            }

            lblInfo.Text = s;

            txtFrRemark.Text = o.ActiveSheet.Cells[e.Row,(int)clsComSupSpd.enmConsultList.FrREMARK].Text.Trim();
            txtToRemark.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmConsultList.ToREMARK].Text.Trim();


        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {

            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(s, e.Column);
                return;
            }
            else if (sender == this.ssList)
            {                
                //string strRWOID = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsSupFnExSpd.enmSupFnExMain.ROWID].Text.Trim();
            }
        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            //string strTemp = "";

            FpSpread s = (FpSpread)sender;

            if (s.ActiveSheet.RowCount <= -1)
            {
                return;
            }

            if (e.RowHeader == true || e.Row < 0)
            {
                return;
            }                        
           
            try
            {
                e.TipText = ssList.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                e.ShowTip = true;
            }
            catch
            {

            }
           
        }

        void eDtpTxtChange(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text.Trim();
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

            strTitle = " Consult LIST " + "(" + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 30, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
        
        void eOptChange(object sender, EventArgs e)
        {
            RadioButton o = (RadioButton)sender;

            
            if (sender == this.optJob1)
            {
                if (o.Checked == true)
                {
                    ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.Return].Visible = false;

                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.DeptCode].Visible = true;
                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.FrDrCode].Visible = true;

                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.toDept].Visible = false;
                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.toDrCode].Visible = false;
                }
                else
                {
                    ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.Return].Visible = true;

                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.DeptCode].Visible = false;
                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.FrDrCode].Visible = false;

                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.toDept].Visible = true;
                    //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmConsultList.toDrCode].Visible = true;
                }

                screen_display();

                screen_clear("A");

            }
        }

        void setCombo(PsmhDb pDbCon, string argDept ="")
        {
            cboDr.Items.Clear();
            cboDr.Items.Add("****.전체");
            cboDr.SelectedIndex = 0;

            if (argDept=="")
            {
                method.setCombo_View(cboDept, sup.sel_BAS_CLINICDEPT(pDbCon, argDept, " 'II','RD','MD' "), clsParam.enmComParamComboType.ALL);

                cboDr.Items.Clear();
                cboDr.Items.Add("****.전체");
                cboDr.SelectedIndex = 0;

            }
            else
            {
                method.setCombo_View(cboDept, sup.sel_BAS_CLINICDEPT(pDbCon, argDept, " 'II','RD','MD' "), clsParam.enmComParamComboType.None);
            }          
        }

        void read_cinfo(FpSpread Spd, int row)
        {
            if (row < 0)
            {
                return;
            }

            if (Spd.ActiveSheet.RowCount == 0)
            {
                return;
            }

            //환자공통정보 표시                   
            cinfo = new clsComSup.SupPInfo();

            cinfo.strPano = Spd.ActiveSheet.Cells[row, (int)clsComSupSpd.enmConsultList.Ptno].Text.Trim();
            

        }

        void screen_clear(string job="")
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            if (job =="")
            {
                dtpFDate.Text = cpublic.strSysDate;
                dtpTDate.Text = cpublic.strSysDate;
                txtPtno.Text = "";

            }

            lblInfo.Text = "";
            txtFrRemark.Text = "";
            txtToRemark.Text = "";
            


        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;
            //DateTime t1 ;
            //DateTime t2 ;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            #region //변수 담기
            cOcsItransfer = new clsComSup.cOcsItransfer();
            cOcsItransfer.Job = optJob1.Checked == true ? "TO" : "FR";
            cOcsItransfer.STS = "0";
            if (optSTS1.Checked == true)
            {
                cOcsItransfer.STS = "1";
            }
            else if (optSTS2.Checked == true)
            {
                cOcsItransfer.STS = "2";
            }
            cOcsItransfer.Ptno = txtPtno.Text.Trim();
            cOcsItransfer.Date1 = dtpFDate.Text.Trim();
            cOcsItransfer.Date2 = dtpTDate.Text.Trim();
            if (cboDept.Items.Count > 0)
            {
                cOcsItransfer.DeptCode = clsComSup.setP(cboDept.SelectedItem.ToString(),".",1).Trim();
            }
            cOcsItransfer.DrCode = "****";
            if (cboDr.Items.Count > 0)
            {
                cOcsItransfer.DrCode = clsComSup.setP(cboDr.SelectedItem.ToString(),".",1).Trim();
            }
            
            cOcsItransfer.Jewon = "J";        

            #endregion

            //쿼리실행      
            dt = sup.sel_OCS_ITRANSFER(pDbCon, cOcsItransfer);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Time].Text = "";
                    if (dt.Rows[i]["SDate"].ToString().Trim()!="")
                    {
                        
                        //if (dt.Rows[i]["EDate"].ToString().Trim() =="")
                        //{
                        //    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Time].Text = "";
                        //}
                        //else
                        //{

                        //}


                    }
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Room].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.DeptCode].Text = dt.Rows[i]["Dept"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.FrDrCode].Text = dt.Rows[i]["FrDrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Ptno].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim() +"/" + dt.Rows[i]["Age"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.STS].Text = dt.Rows[i]["GbConfirm"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Print].Text = dt.Rows[i]["GbPrint"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.InDate].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.NST].Text = dt.Rows[i]["GbNST"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.GbERSMS].Text = dt.Rows[i]["GbEMSMS"].ToString().Trim();
                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Move].Text = dt.Rows[i]["DrCode"].ToString().Trim() == "1" ? "○" : "X";
                    
                    if (dt.Rows[i]["FC_fall"].ToString().Trim() =="Y")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.fall].Text = "낙";
                    }
                    if (dt.Rows[i]["FC_TeamNo2"].ToString().Trim() !="")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.TeamNo].Text = dt.Rows[i]["FC_TeamNo2"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["FC_TeamNo"].ToString().Trim()=="")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.TeamNo].Text = dt.Rows[i]["FC_Tel"].ToString().Trim();
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.TeamNo].Text = dt.Rows[i]["FC_TeamNo"].ToString().Trim();
                        }                                                
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.NST].Text = dt.Rows[i]["GbNST"].ToString().Trim();
                    if (dt.Rows[i]["Gubun"].ToString().Trim()=="1")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Kekri].Text = "○";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Kekri].Text = "X";
                    }
                    

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.IPDNO].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Bi].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.WardCode].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.infect].Value = sup.Resource2files(dt.Rows[i]["FC_infect"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.toDept].Text = dt.Rows[i]["ToDeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.toDrCode].Text = dt.Rows[i]["ToDrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.Return].Text = dt.Rows[i]["Return"].ToString().Trim()=="*"?"◎":"";

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.ToREMARK].Text = dt.Rows[i]["ToRemark"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.FrREMARK].Text = dt.Rows[i]["FrRemark"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmConsultList.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

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
