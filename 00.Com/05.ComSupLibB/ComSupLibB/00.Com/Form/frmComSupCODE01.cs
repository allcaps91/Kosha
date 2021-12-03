using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupCODE01.cs
    /// Description     : 진료지원 BAS_BCODE 코드 설정 화면 공통
    /// Author          : 윤조연
    /// Create Date     : 2017-08-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// BAS_BCODE 테이블 이용한 공통 진료지원 코드 폼  폼 추가생성함 frmComSupCODE01.cs 
    /// </history>
    /// <seealso cref= "\Ocs\endo\endo_new\Frm내시경물품등록.frm(frm내시경물품등록) >> frmComSupCODE01.cs 폼이름 재정의" />
    public partial class frmComSupCODE01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsQuery Query = new clsQuery();
        clsComSup sup = new clsComSup();        
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();

        clsComSup.cBasBCode cBasBCode = null;

        string gJob = "";

        #endregion
        
        public frmComSupCODE01(string Job)
        {
            InitializeComponent();

            gJob = Job;

            setEvent();
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
                supSpd.sSpd_BCodeMst(ssList, supSpd.sSpdBCodeMst, supSpd.nSpdBCodeMst, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData(clsDB.DbCon);

                setAuth(gJob);

            }

        }
        //권한체크
        void setAuth(string Job)
        {
            string s1 = "";
            string s2 = "";

            cboGubun.DropDownStyle = ComboBoxStyle.DropDownList;

            if (Job =="MASTER")
            {
                cboGubun.DropDownStyle = ComboBoxStyle.DropDown;

                s1 = " '자료사전목록' ";
                s2 = "";
                //s2 = " 'C#_Button1_Ends','C#_Button1_FnEx','C#_Button2_Ends','C#_Button2_FnEx','C#_Menu_Ends','C#_Menu_Ends_Sub','C#_Menu_FnEx','C#_Menu_FnEx_Sub','C#_심장초음파대장_오더코드','C#_영상검사_기능검사제외코드','EKG-검사종류' ";
            }
            else if (Job == "00")
            {
                s1 = " '자료사전목록' ";
                s2 = " 'ENDO_내시경부속물품','ENDO_내시경소독약품' ";
            }
            else if (Job == "HIC")
            {
                s1 = " '자료사전목록' ";
                s2 = " 'HIC_EXAM_MST','HIC_EXAM_MST_SUB' ";
            }
            else if (Job == "XRAY01")
            {
                s1 = " '자료사전목록' ";
                s2 = " 'C#_XRAY_촬영기사' ";
            }
            else if (Job == "XRAY_TONG01")
            {
                s1 = " '자료사전목록' ";
                s2 = " 'XRAY_재료코드01' ";
            }
            else if (Job == "DRUG_CODE")
            {
                s1 = " '자료사전목록' ";
                s2 = " 'C#_약제마스터_코드세팅' ";
            }
            else
            {

            }

            setCombo(clsDB.DbCon, Job,s1, s2);

            setSpdColHide(Job);

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);            
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
                        
            
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDoubleClick);
            this.ssList.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
            this.ssList.ClipboardPasted += new ClipboardPastedEventHandler(eSpreadClipboardPasted);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClicked);


        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch)
            {
                screen_display();
            }
            else if (sender == this.btnCancel)
            {
                screen_clear();
            }
            else if (sender == this.btnPrint)
            {
                ePrint();
            }
            else if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon);
            }
           

        }

        void eCboEvent(object sender, EventArgs e)
        {
            //
            //try
            //{
            //    if (cboClass != null && cboClass.SelectedItem.ToString() != "")
            //    {
            //        setCombo2(clsComSup.setP(cboClass.SelectedItem.ToString(), ".", 1).Trim(), "");
            //    }
            //}
            //catch
            //{

            //}

        }

        void eSpreadDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            FpSpread s = (FpSpread)sender;

            string strCode = s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmBCodeMst.Code].Text.Trim();

            //GetData2(strXCode);

        }

        void eSpreadEditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            ssList.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmBCodeMst.Change].Text = "Y";

        }

        void eSpreadBtnClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (e.Column == (int)clsComSupSpd.enmBCodeMst.Chk)
            {
                if (s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmBCodeMst.Chk].Text == "True")
                {
                    s.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightPink;
                }
                else
                {
                    s.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
                }
            }
        }

        void eSpreadClipboardPasted(object sender,ClipboardPastedEventArgs e)
        {
            if (e.CellRange.Column < 0 || e.CellRange.Row < 0) return;

            //ssList.ActiveSheet.Cells[e.CellRange.Row, (int)clsComSupSpd.enmBCodeMst.Change].Text = "Y";
            ssList.ActiveSheet.Cells[e.CellRange.Row, (int)clsComSupSpd.enmBCodeMst.Change, e.CellRange.Row + e.CellRange.RowCount -1, (int)clsComSupSpd.enmBCodeMst.Change].Text = "Y";
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

            strTitle = "검사 코드 LIST";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void eSave(PsmhDb pDbCon)
        {            
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strGubun = "";
            string strChange = "";
            string strChk = "";
            
            strGubun = cboGubun.SelectedItem.ToString();

            if (strGubun=="")
            {
                ComFunc.MsgBox("작업구분을 선택후 작업하세요!!");
                return;
            }

            // clsTrans DT = new clsTrans(); 
            clsDB.setBeginTran(pDbCon);

            try
            {
                for (int i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    #region class 초기화 및 변수 세팅

                    cBasBCode = new clsComSup.cBasBCode();

                    strChk = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Chk].Text.Trim().ToUpper();
                    strChange = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Change].Text.Trim();
                    cBasBCode.Gubun = strGubun;
                    cBasBCode.EntSabun =  Convert.ToInt32(clsType.User.IdNumber);
                    cBasBCode.Code = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Code].Text.Trim();
                    cBasBCode.Name = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Name].Text.Trim();

                    cBasBCode.JDate = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.JDate].Text.Trim();
                    cBasBCode.DelDate = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.DelDate].Text.Trim();
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Sort].Text.Trim() != "")
                    {
                        cBasBCode.Sort = Convert.ToInt32(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Sort].Text.Trim());
                    }

                    cBasBCode.Part = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Part].Text.Trim();
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Cnt].Text.Trim() != "")
                    {
                        cBasBCode.Cnt = Convert.ToInt32(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Cnt].Text.Trim());
                    }

                    cBasBCode.Gubun2 = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun2].Text.Trim();
                    cBasBCode.Gubun3 = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun3].Text.Trim();
                    //cBasBCode.Gubun4 = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun4].Text.Trim();
                    //cBasBCode.Gubun5 = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun5].Text.Trim();
                    //if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum1].Text.Trim()!="")
                    //{
                    //    cBasBCode.GuNum1 = Convert.ToInt32(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum1].Text.Trim());
                    //}
                    //if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum2].Text.Trim() != "")
                    //{
                    //    cBasBCode.GuNum2 = Convert.ToInt32(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum2].Text.Trim());
                    //}
                    //if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum3].Text.Trim() != "")
                    //{
                    //    cBasBCode.GuNum3 = Convert.ToInt32(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum3].Text.Trim());
                    //}


                    cBasBCode.ROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.ROWID].Text.Trim();

                    #endregion

                    if (strChk == "TRUE" && cBasBCode.ROWID != "")
                    {
                        cBasBCode.DelDate = cpublic.strSysDate + " " + VB.Left(cpublic.strSysTime, 5);

                        SqlErr = sup.up_Bas_BCode(pDbCon, cBasBCode, ref intRowAffected);
                    }
                    else if (strChange == "Y")
                    {
                        if (cBasBCode.ROWID == "")
                        {
                            SqlErr = sup.ins_Bas_BCode(pDbCon, cBasBCode, ref intRowAffected);
                        }
                        else
                        {
                            SqlErr = sup.up_Bas_BCode(pDbCon, cBasBCode, ref intRowAffected);
                        }
                    }
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);
                }
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
                        
            
            screen_clear();
            screen_display();

        }

        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            txtRow.Text = "20";

            read_sysdate();
                                   
        }

        void setCombo(PsmhDb pDbCon, string argJob,string argS1,string argS2)
        {            

            cboGubun.Items.Clear();
            if (argJob =="XRAY01")
            {

            }
            else
            {
                cboGubun.Items.Add("");
            }
            
            if (argJob=="MASTER")
            {
                cboGubun.Items.Add("자료사전목록");
            }
            DataTable dt = sup.sel_BasBCode(pDbCon, argS1, argS2," Code " , "", " Code " );
            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboGubun.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                }                
            }
            
            cboGubun.SelectedIndex = 0;

            if (argJob == "DRUG_CODE")
            {
                cboGubun.SelectedIndex = 1;
            }

        }

        void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //GetData2(txtCode.Text.Trim().ToUpper());
            }

        }

        void setSpdColHide(string argJob)
        {
            if (argJob=="00")
            {
                #region  조건별로 컬럼 히든 및 컬럼 사이즈 

                //히든
                //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.JDate].Visible = false;
                //ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.DelDate].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.EntDate].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.EntSabun].Visible = false;

                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Part].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Cnt].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Cnt].Visible = false;

                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Gubun2].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Gubun3].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Gubun4].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.Gubun5].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.GuNum1].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.GuNum2].Visible = false;
                ssList.ActiveSheet.Columns[(int)clsComSupSpd.enmBCodeMst.GuNum3].Visible = false;
                
                //컬럼사이즈
                ssList.ActiveSheet.Columns.Get((int)clsComSupSpd.enmBCodeMst.Name).Width = 350;

                this.Size = new Size(720, 620);

            #endregion

            }
            else if (argJob == "MASTER")
            {
                //컬럼사이즈
                ssList.ActiveSheet.Columns.Get((int)clsComSupSpd.enmBCodeMst.Code).Width = 150;
            }
            else if (argJob == "HIC")
            {
                //컬럼사이즈
                ssList.ActiveSheet.Columns.Get((int)clsComSupSpd.enmBCodeMst.Code).Width = 150;

                
            string[] sSpdBCodeMst = { "삭제","수정","구분","코드","명칭"
                ,"코드매칭","구분3","구분4","구분5","나이"
                ,"성별","년주기","구분N1","구분N2","구분N3"
                ,"적용일자", "등록일자","등록자", "삭제일자","ROWID" };

            
            int[] nSpdBCodeMst = { 30,30,50,60,250
                                      ,70,50,50,50,50
                                      ,40,50,50,50,50
                                      ,80,80,60,80,60 };

            methodSpd.setHeader(ssList, sSpdBCodeMst, nSpdBCodeMst);

            }

        }

        void screen_display()
        {
            try
            {
                string s = cboGubun.SelectedItem.ToString();

                if (s != "")
                {
                    s = " '" + s + "' ";

                    GetData(ssList, s, Convert.ToInt16(txtRow.Text.Trim()));
                }
                else
                {
                    ComFunc.MsgBox("작업구분을 선택후 조회 하세요!!");
                }
            }
            catch
            {                
                ComFunc.MsgBox("작업구분을 선택후 조회 하세요!!");
                cboGubun.Text = "";
                screen_clear();
            }
            
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");


            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is TextBox)
                {
                    //ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }

            }
            ssList.ActiveSheet.RowCount = 0;
            ssList.ActiveSheet.RowCount = Convert.ToInt16(txtRow.Text);                 
                        
            btnSave.Enabled = true;


        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(FarPoint.Win.Spread.FpSpread Spd,string argGubun,int argRow)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            Spd.Enabled = false;


            dt = sup.sel_BasBCode(clsDB.DbCon, argGubun, "","","","",true);
            
            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count + argRow;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Change].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Name].Text = dt.Rows[i]["Name"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.JDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.DelDate].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.EntSabun].Text = dt.Rows[i]["EntSabun"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Sort].Text = dt.Rows[i]["Sort"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Part].Text = dt.Rows[i]["Part"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Cnt].Text = dt.Rows[i]["Cnt"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun2].Text = dt.Rows[i]["Gubun2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun3].Text = dt.Rows[i]["Gubun3"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun4].Text = dt.Rows[i]["Gubun4"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.Gubun5].Text = dt.Rows[i]["Gubun5"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum1].Text = dt.Rows[i]["GuNum1"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum2].Text = dt.Rows[i]["GuNum2"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.GuNum3].Text = dt.Rows[i]["GuNum3"].ToString().Trim();


                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmBCodeMst.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();


                }

            }
            else
            {
                Spd.ActiveSheet.RowCount =  argRow;
            }

            #endregion

            Cursor.Current = Cursors.Default;
            Spd.Enabled = true;

        }
    }
}
