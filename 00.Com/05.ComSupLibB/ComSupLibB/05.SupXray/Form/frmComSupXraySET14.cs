using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET14.cs
    /// Description     : 영상의학과 심장초음파 판독 지원폼2
    /// Author          : 윤조연
    /// Create Date     : 2018-02-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xuec\FrmEC03_2.frm(FrmECResult2) >> frmComSupXraySET14.cs 폼이름 재정의" />
    public partial class frmComSupXraySET14 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        //clsComSupXraySQL.cXrayDetail cXrayDetail = null;
        clsComSupXrayRead.cXray_Read cXray_Read = null;        
        clsComSupXrayRead.cXray_Read_Delegate cXray_Read_Delegate = null; //델리게이트

        public delegate void SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls);
        public event SendMsg rSendMsg;

        //델리게이트 
        frmComSupXRYSET01 frmComSupXRYSET01x = null;    //상용구 입력폼
        frmComSupXraySET03 frmComSupXraySET03x = null;  //상용결과 입력폼
        frmComSupXraySET10 frmComSupXraySET10x = null;  //개인별 판독내역        

        //string gJob = "";
        //string gSabun = "";

        #endregion

        public frmComSupXraySET14(clsComSupXrayRead.cXray_Read argCls)
        {
            InitializeComponent();
            cXray_Read = argCls;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            screen_clear();

        } 

        //권한체크
        void setAuth()
        {

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);

            this.btnSearch8.Click += new EventHandler(eBtnSearch);
            this.btnSearch9.Click += new EventHandler(eBtnSearch);
            this.btnSearch10.Click += new EventHandler(eBtnSearch);

            this.btnEMR.Click += new EventHandler(eBtnSearch);
            this.btnResult.Click += new EventHandler(eBtnSearch);

            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);


            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssOrdList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ss.CellClick += new CellClickEventHandler(eSpreadClick);
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);                        
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.txtResult0.KeyDown += new KeyEventHandler(eTxtEvent);
            

            //this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);


        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                setAuth();

                //
                screen_display();

            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ss)
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

                string s = o.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();

                //txtResult.Text += s;
                txtResult.Paste(s);

            }

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }



        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnOK)
            {
                if (cXray_Read.STS01 == "")
                {
                    setResultEC2("00", cXray_Read.Remark);
                }
                else
                {
                    setResultEC2("02", cXray_Read.Remark);
                }

                this.Close();
                return;
            }

        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch8)
            {
                //상용단어 등록
                //frmComSupXRYSET01 f = new frmComSupXRYSET01("XRAY", Convert.ToInt32(clsType.User.Sabun));                                
                if (frmComSupXRYSET01x == null)
                {
                    frmComSupXRYSET01x = new frmComSupXRYSET01("XRAY", Convert.ToInt32(clsType.User.Sabun));
                    frmComSupXRYSET01x.rSendMsg += new frmComSupXRYSET01.SendMsg(frmComSupXRYSET01x_SendMsg);
                }
                else
                {
                    frmComSupXRYSET01x = null;
                    frmComSupXRYSET01x = new frmComSupXRYSET01("XRAY", Convert.ToInt32(clsType.User.Sabun));
                    frmComSupXRYSET01x.rSendMsg += new frmComSupXRYSET01.SendMsg(frmComSupXRYSET01x_SendMsg);
                }

                frmComSupXRYSET01x.ShowDialog();
                sup.setClearMemory(frmComSupXRYSET01x);

                txtResult.Focus();

            }
            else if (sender == this.btnSearch9)
            {
                //상용 판독결과 등록
                //frmComSupXraySET03 f = new frmComSupXraySET03("", Convert.ToUInt32(clsType.User.Sabun));
                //frmComSupXraySET03 f = new frmComSupXraySET03("", 33781); //유윤정

                if (frmComSupXraySET03x == null)
                {
                    frmComSupXraySET03x = new frmComSupXraySET03("XRAY", "", Convert.ToInt32(clsType.User.Sabun));
                    frmComSupXraySET03x.rSendMsg += new frmComSupXraySET03.SendMsg(frmComSupXraySET03x_SendMsg);
                }
                else
                {
                    frmComSupXraySET03x = null;
                    frmComSupXraySET03x = new frmComSupXraySET03("XRAY", "", Convert.ToInt32(clsType.User.Sabun));
                    frmComSupXraySET03x.rSendMsg += new frmComSupXraySET03.SendMsg(frmComSupXraySET03x_SendMsg);
                }

                frmComSupXraySET03x.ShowDialog();
                sup.setClearMemory(frmComSupXraySET03x);

                txtResult.Focus();

            }
            else if (sender == this.btnSearch10)
            {
                try
                {
                    if (cXray_Read.Pano == "")
                    {
                        ComFunc.MsgBox("대상을 선택후 작업하세요!!");
                        return;
                    }

                    //상용 판독결과 등록                
                    //frmComSupXraySET10 f = new frmComSupXraySET10("00",cXray_Read.Pano); 
                    if (frmComSupXraySET10x == null)
                    {
                        frmComSupXraySET10x = new frmComSupXraySET10("00", cXray_Read.Pano);
                        frmComSupXraySET10x.rSendMsg += new frmComSupXraySET10.SendMsg(frmComSupXraySET10x_SendMsg);
                    }
                    else
                    {
                        frmComSupXraySET10x = null;
                        frmComSupXraySET10x = new frmComSupXraySET10("00", cXray_Read.Pano);
                        frmComSupXraySET10x.rSendMsg += new frmComSupXraySET10.SendMsg(frmComSupXraySET10x_SendMsg);
                    }

                    frmComSupXraySET10x.ShowDialog();
                    sup.setClearMemory(frmComSupXraySET10x);

                    //txtResult.Focus();
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                }

            }
            else if (sender == this.btnEMR)
            {
                //EMR 뷰어                
                clsVbEmr.EXECUTE_NewTextEmrView(cXray_Read.Pano);
            }
            else if (sender == this.btnResult)
            {
                frmViewResult f = new frmViewResult(cXray_Read.Pano);
                f.ShowDialog();
                sup.setClearMemory(f);
            }
        }

        #region //델리게이트 

        void frmComSupXRYSET01x_SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls)
        {
            txtResult.Text += argCls.Sogen;
        }

        void frmComSupXraySET03x_SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls)
        {
            string strRemark = txtResult.Text;

            if (argCls.Job == "01") //결과변경
            {
                txtResult.Text = argCls.Sogen;
            }
            else if (argCls.Job == "02" || argCls.Job == "03") //결과추가 , 범위추가
            {
                if (argCls.sPos == "01") //현재커서
                {
                    //txtResult.Text += argCls.Sogen + strRemark;                    
                    txtResult.Paste(argCls.Sogen);
                }
                else if (argCls.sPos == "02") //처음
                {
                    txtResult.Text = argCls.Sogen + strRemark;
                }
                else if (argCls.sPos == "03") //마지막
                {
                    txtResult.Text += argCls.Sogen;
                }
            }

            txtResult.Select(txtResult.Text.Length, 0);
        }

        void frmComSupXraySET10x_SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls)
        {
            string strRemark = txtResult.Text;

            if (argCls.Job == "01") //결과변경
            {
                txtResult.Text = argCls.Sogen;
            }
            else if (argCls.Job == "02" || argCls.Job == "03") //결과추가 , 범위추가
            {
                if (argCls.sPos == "01") //현재커서
                {
                    //txtResult.Text += argCls.Sogen + strRemark;                    
                    txtResult.Paste(argCls.Sogen);
                }
                else if (argCls.sPos == "02") //처음
                {
                    txtResult.Text = argCls.Sogen + strRemark;
                }
                else if (argCls.sPos == "03") //마지막
                {
                    txtResult.Text += argCls.Sogen;
                }
            }

            txtResult.Select(txtResult.Text.Length, 0);
        }

        #endregion

        void eBtnSave(object sender, EventArgs e)
        {
            //if (sender == this.btnSave1)
            //{
            //    //
            //    eSave(clsDB.DbCon, "저장");
            //}         
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

        void eSave(PsmhDb pDbCon, string Job)
        {
        }

        void setDelegate(string argJob, string argResult, string[] argResultEC)
        {
            if (rSendMsg == null)
            {
                return;
            }
            int i = 0;
            string s = string.Empty;

            for ( i = 0; i < 8; i++)
            {
                s += argResultEC[i];
            }

            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = argJob;
            if (argJob == "01")
            {

            }
            cXray_Read_Delegate.ResultEC = s;
           
            cXray_Read_Delegate.Sogen = argResult;

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            this.Hide();
        }

        void setBase()
        {
            
        }

        void setResultEC2(string argJob, string argResult)
        {
            string[] s = new string[8];
            string s3 = string.Empty;

            int i = 0;


            #region //입력값을 변수에 담기

            if (chkV1.Checked == true)
            {
                s[0] += "Y^&^";
            }
            else
            {
                s[0] += "N^&^";
            }
            if (chkV2.Checked == true)
            {
                s[0] += "Y^&^";
            }
            else
            {
                s[0] += "N^&^";
            }
            if (chkV3.Checked == true)
            {
                s[0] += "Y^&^";
            }
            else
            {
                s[0] += "N^&^";
            }
            if (chkV4.Checked == true)
            {
                s[0] += "Y^&^";
            }
            else
            {
                s[0] += "N^&^";
            }
            if (chkV5.Checked == true)
            {
                s[0] += "Y^&^";
            }
            else
            {
                s[0] += "N^&^";
            }
            if (chkV6.Checked == true)
            {
                s[0] += "Y^&^";
            }
            else
            {
                s[0] += "N^&^";
            }

            for (i = 0; i < 6; i++)
            {
                if (i == 5)
                {
                    s[i + 1] += ssStudy.ActiveSheet.Cells[i + 2, 2].Text.Trim() + "^&^";
                    s[i + 1] += ssStudy.ActiveSheet.Cells[i + 2, 6].Text.Trim() + "^&^";
                }
                else
                {
                    for (int j = 0; j < 6; j++)
                    {
                        s[i + 1] += ssStudy.ActiveSheet.Cells[i + 2, j + 2].Text.Trim() + "^&^";
                    }
                }
            }

            s[7] = txtResult.Text.Trim();


            #endregion

            if (argJob == "00")
            {                                
                s3 = cRead.Xray_Read_EC2(s, "");
                setDelegate("00", s3, s);
            }
            else if (argJob == "01" || argJob == "02")
            {
                if (argJob == "02")
                {
                    s3 += s[0];
                    s3 += s[1];
                    s3 += s[2];
                    s3 += s[3];
                    s3 += s[4];
                    s3 += s[5];
                    s3 += s[6];
                    s3 += s[7];
                }
                else
                {
                    s3 = argResult.Trim();
                }
                
                
                if (s3.Length > 10)
                {
                    cXray_Read.Gubun3 = "E"; //자료가져오기 구분
                }

                //기존값 표시
                s = new string[8];

                #region //값변환
                for (i = 1; i <= 6; i++)
                {
                    s[0] += clsComSup.setP(s3, "^&^", i) + "^&^";
                }
                for (i = 7; i <= 12; i++)
                {
                    s[1] += clsComSup.setP(s3, "^&^", i) + "^&^";
                }
                for (i = 13; i <= 18; i++)
                {
                    s[2] += clsComSup.setP(s3, "^&^", i) + "^&^";
                }
                for (i = 19; i <= 24; i++)
                {
                    s[3] += clsComSup.setP(s3, "^&^", i) + "^&^";
                }
                for (i = 25; i <= 30; i++)
                {
                    s[4] += clsComSup.setP(s3, "^&^", i) + "^&^";
                }
                for (i = 31; i <= 36; i++)
                {
                    s[5] += clsComSup.setP(s3, "^&^", i) + "^&^";
                }

                s[6] = clsComSup.setP(s3, "^&^", 37) + "^&^" + clsComSup.setP(s3, "^&^", 38) + "^&^";
                s[7] = clsComSup.setP(s3, "^&^", 39) + "^&^";

                #endregion

                #region //값표시
                if (clsComSup.setP(s[0],"^&^",1) == "Y")
                {
                    chkV1.Checked = true;
                }
                else
                {
                    chkV1.Checked = false;
                }
                if (clsComSup.setP(s[0], "^&^", 2) == "Y")
                {
                    chkV2.Checked = true;
                }
                else
                {
                    chkV2.Checked = false;
                }
                if (clsComSup.setP(s[0], "^&^", 3) == "Y")
                {
                    chkV3.Checked = true;
                }
                else
                {
                    chkV3.Checked = false;
                }
                if (clsComSup.setP(s[0], "^&^", 4) == "Y")
                {
                    chkV4.Checked = true;
                }
                else
                {
                    chkV4.Checked = false;
                }
                if (clsComSup.setP(s[0], "^&^", 5) == "Y")
                {
                    chkV5.Checked = true;
                }
                else
                {
                    chkV5.Checked = false;
                }
                if (clsComSup.setP(s[0], "^&^", 6) == "Y")
                {
                    chkV6.Checked = true;
                }
                else
                {
                    chkV6.Checked = false;
                }

                for ( i = 0; i < 6; i++)
                {
                    if (i == 5)
                    {
                        ssStudy.ActiveSheet.Cells[i + 2,  2].Text = clsComSup.setP(s[i + 1], "^&^", 1);
                        ssStudy.ActiveSheet.Cells[i + 2, 6].Text = clsComSup.setP(s[i + 1], "^&^", 2);
                    }
                    else
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            ssStudy.ActiveSheet.Cells[i + 2, j + 2].Text = clsComSup.setP(s[i + 1], "^&^", j + 1);
                        }
                    }
                }

                txtResult.Text = s[7].Replace("^&^","");

                #endregion

                if (argJob == "02" && cXray_Read.Gubun3 == "E")
                {
                    //기존값 표시
                    s3 = cRead.Xray_Read_EC2(s, "");                                        
                    setDelegate("01", s3, s);                    
                    
                }
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
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    //((RadioButton)ctl).Checked = false;
                }


            }

            ss_clear(ssStudy);

        }

        void ss_clear(FpSpread Spd)
        {
            for (int i = 2; i < Spd.ActiveSheet.RowCount-1; i++)
            {
                for (int j = 2; j < Spd.ActiveSheet.ColumnCount; j++)
                {
                    if ( i == Spd.ActiveSheet.RowCount - 2)
                    {
                        Spd.ActiveSheet.Cells[i, j].Text = "무";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, j].Text = "";
                    }                    
                    
                }                
            }

            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, 2].Text = "";
            Spd.ActiveSheet.Cells[Spd.ActiveSheet.RowCount - 1, 6].Text = "";

        }

        void screen_display()
        {
            //기본표시
            //setBase();

            //            
            GetData(clsDB.DbCon);
        }

        void GetData(PsmhDb pDbCon)
        {
            //int i = 0;
            string s = string.Empty;
            
            //Cursor.Current = Cursors.WaitCursor;


            //기존판독 내용 있으면
            if (cXray_Read.Remark.Trim().Length > 40)
            {
                setResultEC2("01", cXray_Read.Remark);
                //s = cRead.Xray_Read_EC2(s, "");

            }


            //Cursor.Current = Cursors.Default;

        }


    }
}
