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
    /// File Name       : frmComSupXraySET15.cs
    /// Description     : 영상의학과 심장초음파 판독 지원폼13
    /// Author          : 윤조연
    /// Create Date     : 2018-02-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xuec\FrmEC03_3.frm(FrmECResult) >> frmComSupXraySET15.cs 폼이름 재정의" />
    public partial class frmComSupXraySET15 : Form
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

        public frmComSupXraySET15(clsComSupXrayRead.cXray_Read argCls)
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
                    setResultEC3("00", cXray_Read.Remark);
                }
                else
                {
                    setResultEC3("02", cXray_Read.Remark);
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
                clsVbEmr.EXECUTE_TextEmrView(cXray_Read.Pano, clsType.User.IdNumber);
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

        void setDelegate(string argJob, string argResult, string argResultEC)
        {
            if (rSendMsg == null)
            {
                return;
            }
            
            string s = string.Empty;

            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = argJob;

            if (argJob == "01")
            {

            }
            cXray_Read_Delegate.ResultEC = argResultEC;

            cXray_Read_Delegate.Sogen = argResult;

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            this.Hide();
        }

        void setBase()
        {

        }

        void setResultEC3(string argJob, string argResult)
        {
            int i = 0,j=0;
            int nCnt = 0;
            string[] s = new string[10];
            string[] s2 = new string[10];
            string s3 = string.Empty;
            string sEC = string.Empty;      //ResultEC를 담기 위한 변수

            for ( i = 0; i < 10; i++)
            {                                   
                s2[i] = "";                                    
            }

            #region //입력값을 변수에 담기
            //baseline
            for ( i = 5; i < 9; i++)
            {
                s2[0] += ssTilt.ActiveSheet.Cells[3, i].Text.Trim() + "^&^";                
            }
            s3 += s2[0] + ";";            
            //HUT60
            for (i = 4; i < 9; i++)
            {
                for ( j = 5; j < 9; j++)
                {
                    s2[1] += ssTilt.ActiveSheet.Cells[i, j].Text.Trim() + "^&^";                    
                }                
            }
            s3 += s2[1] + ";";            
            //HUT70
            for (i = 9; i < 12; i++)
            {
                for (j = 5; j < 9; j++)
                {
                    s2[2] += ssTilt.ActiveSheet.Cells[i, j].Text.Trim() + "^&^";                    
                }                
            }
            s3 += s2[2] + ";";             
            //supine
            for (i = 12; i < 15; i++)
            {
                for (j = 5; j < 9; j++)
                {
                    s2[3] += ssTilt.ActiveSheet.Cells[i, j].Text.Trim() + "^&^";                    
                }
            }
            s3 += s2[3] + ";";            
            //nig
            for (i = 15; i < 21; i++)
            {
                for (j = 5; j < 9; j++)
                {
                    s2[4] += ssTilt.ActiveSheet.Cells[i, j].Text.Trim() + "^&^";                     
                }
            }
            s3 += s2[4] + ";";            
            //impre
            s2[5] += ssTilt.ActiveSheet.Cells[25, 1].Text.Trim() + "^&^";             
            s2[5] += ssTilt.ActiveSheet.Cells[26, 1].Text.Trim() + "^&^";
            s3 += s2[5] + ";";
            
            //type1
            s2[6] = ssTilt.ActiveSheet.Cells[28, 2].Text.Trim() + "^&^";
            s3 += s2[6] + ";";            
            //type2
            s2[7] = ssTilt.ActiveSheet.Cells[34, 2].Text.Trim() + "^&^";
            s3 += s2[7] + ";";            
            //type3
            s2[8] = ssTilt.ActiveSheet.Cells[40, 2].Text.Trim() + "^&^";
            s3 += s2[8] + ";";
            
            //
            s2[9] = txtConclusion.Text.Trim() + "^&^" + txtResult.Text.Trim() + "^&^";
            s3 += txtConclusion.Text.Trim() + "^&^" + txtResult.Text.Trim() + "^&^";            
            s3 += ";";


            #endregion

            if (argJob == "00")
            {
                sEC = s3;
                //2018-08-30 안정수, Xray_Read_EC3(s, "") -> s2로 변경
                s3 = cRead.Xray_Read_EC3(s2, "");
                setDelegate("00", s3, sEC);
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
                    s3 += s[8];
                    s3 += s[9];
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
                s = new string[10];

                #region //값변환

                s[0] = clsComSup.setP(s3, ";", 1) + ";";
                s[1] = clsComSup.setP(s3, ";", 2) + ";";
                s[2] = clsComSup.setP(s3, ";", 3) + ";";
                s[3] = clsComSup.setP(s3, ";", 4) + ";";
                s[4] = clsComSup.setP(s3, ";", 5) + ";";
                s[5] = clsComSup.setP(s3, ";", 6) + ";";
                s[6] = clsComSup.setP(s3, ";", 7) + ";";
                s[7] = clsComSup.setP(s3, ";", 8) + ";";
                s[8] = clsComSup.setP(s3, ";", 9) + ";";
                s[9] = clsComSup.setP(s3, ";", 10) + ";";

                #endregion

                #region //값표시
                //baseline
                nCnt = 0;
                for (j = 5; j < 9; j++)
                {
                    nCnt++;
                    ssTilt.ActiveSheet.Cells[3, j].Text = clsComSup.setP(s[0].Replace(";", ""),"^&^",nCnt).Trim();
                }
                //HUT60
                nCnt = 0;
                for (i = 4; i < 9; i++)
                {
                    for (j = 5; j < 9; j++)
                    {
                        nCnt++;
                        ssTilt.ActiveSheet.Cells[i, j].Text = clsComSup.setP(s[1].Replace(";", ""), "^&^", nCnt).Trim();
                    }
                }
                //HUT70
                nCnt = 0;
                for (i = 9; i < 12; i++)
                {
                    for (j = 5; j < 9; j++)
                    {
                        nCnt++;
                        ssTilt.ActiveSheet.Cells[i, j].Text = clsComSup.setP(s[2].Replace(";", ""), "^&^", nCnt).Trim();
                    }
                }
                //supine
                nCnt = 0;
                for (i = 12; i < 15; i++)
                {
                    for (j = 5; j < 9; j++)
                    {
                        nCnt++;
                        ssTilt.ActiveSheet.Cells[i, j].Text = clsComSup.setP(s[3].Replace(";", ""), "^&^", nCnt).Trim();
                    }
                }
                //nig
                nCnt = 0;
                for (i = 15; i < 21; i++)
                {
                    for (j = 5; j < 9; j++)
                    {
                        nCnt++;
                        ssTilt.ActiveSheet.Cells[i, j].Text = clsComSup.setP(s[4].Replace(";", ""), "^&^", nCnt).Trim();
                    }
                }

                //impre
                ssTilt.ActiveSheet.Cells[25, 1].Text = clsComSup.setP(s[5].Replace(";", ""), "^&^", 1).Trim();
                ssTilt.ActiveSheet.Cells[26, 1].Text = clsComSup.setP(s[5].Replace(";", ""), "^&^", 2).Trim();
                

                //type1
                ssTilt.ActiveSheet.Cells[28, 2].Text = clsComSup.setP(s[6].Replace(";", ""), "^&^", 1).Trim();
                
                //type2
                ssTilt.ActiveSheet.Cells[34, 2].Text = clsComSup.setP(s[7].Replace(";", ""), "^&^", 1).Trim();
                
                //type3
                ssTilt.ActiveSheet.Cells[40, 2].Text = clsComSup.setP(s[8].Replace(";", ""), "^&^", 1).Trim();
                
                txtConclusion.Text = clsComSup.setP(s[9].Replace(";",""), "^&^", 1).Trim();
                txtResult.Text = clsComSup.setP(s[9].Replace(";", ""), "^&^", 2).Trim();

                #endregion

                if (argJob == "02" && cXray_Read.Gubun3 == "E")
                {
                    sEC = s3;
                    //기존값 표시
                    s3 = cRead.Xray_Read_EC3(s, "");
                    setDelegate("01", s3, sEC);

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

            ss_clear(ssTilt);

        }

        void ss_clear(FpSpread Spd)
        {
            for (int i = 3; i < 21; i++)
            {
                for (int j = 5; j < 9; j++)
                {                    
                    Spd.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            Spd.ActiveSheet.Cells[25, 1].Text = "";
            Spd.ActiveSheet.Cells[26, 1].Text = "";

            Spd.ActiveSheet.Cells[28, 2].Text = "";
            Spd.ActiveSheet.Cells[34, 2].Text = "";
            Spd.ActiveSheet.Cells[40, 2].Text = "";


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
                setResultEC3("01", cXray_Read.Remark);
                //s = cRead.Xray_Read_EC2(s, "");

            }


            //Cursor.Current = Cursors.Default;

        }



    }
}
