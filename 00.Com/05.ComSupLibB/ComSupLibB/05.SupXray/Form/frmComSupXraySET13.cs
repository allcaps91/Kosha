using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET13.cs
    /// Description     : 영상의학과 심장초음파 판독 지원폼1
    /// Author          : 윤조연
    /// Create Date     : 2018-02-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xuec\FrmEC03.frm(FrmECResult) >> frmComSupXraySET13.cs 폼이름 재정의" />
    public partial class frmComSupXraySET13 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComEmrSQL cEMR = new clsComEmrSQL();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSupXraySQL.cXrayDetail cXrayDetail = null;
        clsComSupXrayRead.cXray_Read cXray_Read = null;
        clsComSupXrayRead.cXray_Read_Delegate cXray_Read_Delegate = null; //델리게이트

        public delegate void SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls);
        public event SendMsg rSendMsg;

        //델리게이트 
        frmComSupXRYSET01 frmComSupXRYSET01x = null;    //상용구 입력폼
        frmComSupXraySET03 frmComSupXraySET03x = null;  //상용결과 입력폼
        frmComSupXraySET10 frmComSupXraySET10x = null;  //개인별 판독내역        

        string gGubun = "";
        //string gJob = "";
        //string gSabun = "";

        #endregion

        public frmComSupXraySET13(clsComSupXrayRead.cXray_Read argCls)
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

            optGubun1.Checked = true;
            gGubun = "1";    
            
            //2018-08-03 안정수, Echo 요청으로 추가
            if(txtResult0.Text == "")
            {
                txtResult0.Select();
            }

            if (cXray_Read.Pano != "")
            {
                txtPano.Text = cXray_Read.Pano;
                if (txtPano.Text != "")
                {
                    txtSName.Text = fun.Read_Patient(clsDB.DbCon, txtPano.Text, "2");
                }
            }

            txtResult16.ImeMode = ImeMode.Alpha;
            txtResult.ImeMode = ImeMode.Alpha;

            groupBox7.Visible = false;
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

            this.txtResult0.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult1.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult2.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult3.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult4.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult5.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult6.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult7.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult8.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult9.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult10.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult11.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult12.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult13.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult14.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult15.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult17.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult18.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult19.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult20.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult21.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult22.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult23.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult24.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult25.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult26.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult27.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult28.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult29.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult30.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult31.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult32.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult33.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtResult34.KeyDown += new KeyEventHandler(eTxtEvent);

            //this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);

            //2018-07-31 안정수, 도형자c 요청으로 FInding에도 특수문자 입력되도록하기위하여 구분자 추가함
            this.optGubun0.CheckedChanged += new System.EventHandler(eRdoCheckEvent);
            this.optGubun1.CheckedChanged += new System.EventHandler(eRdoCheckEvent);

            //2018-08-03 안정수, Echo방 요청으로 결과입력창에서도 단축키 사용가능하도록 추가
            this.txtResult16.KeyUp += new KeyEventHandler(eTxtKeyUp);
            this.txtResult.KeyUp += new KeyEventHandler(eTxtKeyUp);           

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

                if(gGubun == "0")
                {
                    txtResult16.Paste(s);
                }

                else if (gGubun == "1")
                {
                    txtResult.Paste(s);
                }
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
            if (sender == this.txtResult0 || sender == this.txtResult1 || sender == this.txtResult2 || sender == this.txtResult3 || sender == this.txtResult4 || sender == this.txtResult5 || sender == this.txtResult6 || sender == this.txtResult7 || sender == this.txtResult8 || sender == this.txtResult9 || sender == this.txtResult10 || sender == this.txtResult11 || sender == this.txtResult12 || sender == this.txtResult13 || sender == this.txtResult14 || sender == this.txtResult15 || sender == this.txtResult17 || sender == this.txtResult18 || sender == this.txtResult19 || sender == this.txtResult20 || sender == this.txtResult21 || sender == this.txtResult22 || sender == this.txtResult23 || sender == this.txtResult24 || sender == this.txtResult25 || sender == this.txtResult26 || sender == this.txtResult27 || sender == this.txtResult28 || sender == this.txtResult29 || sender == this.txtResult30 || sender == this.txtResult31 || sender == this.txtResult32 || sender == this.txtResult33 || sender == this.txtResult34) 
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBSA.Text = "";
                    if (txtResult0.Text.Trim() !="" && txtResult1.Text.Trim() !="")
                    {
                        double nTemp = 0;
                        nTemp =  (Math.Pow(Convert.ToDouble(txtResult0.Text.Trim()), 0.725) * Math.Pow(Convert.ToDouble(txtResult1.Text.Trim()), 0.425)) * 0.007184 ;
                        txtBSA.Text = VB.Format(nTemp, "0.00");
                    }
                    
                    #region //엔터시 포커스
                    if (sender == this.txtResult0)
                    {
                        txtResult1.Focus();
                    }
                    else if (sender == this.txtResult1)
                    {
                        txtResult2.Focus();
                    }
                    else if (sender == this.txtResult2)
                    {
                        txtResult3.Focus();
                    }
                    else if (sender == this.txtResult3)
                    {
                        txtResult4.Focus();
                    }
                    else if (sender == this.txtResult4)
                    {
                        txtResult5.Focus();
                    }
                    else if (sender == this.txtResult5)
                    {
                        txtResult6.Focus();
                    }
                    else if (sender == this.txtResult6)
                    {
                        txtResult7.Focus();
                    }
                    else if (sender == this.txtResult7)
                    {
                        txtResult8.Focus();
                    }
                    else if (sender == this.txtResult8)
                    {
                        txtResult10.Focus();
                    }
                    //else if (sender == this.txtResult9)
                    //{
                    //    txtResult10.Focus();
                    //}
                    //else if (sender == this.txtResult10)
                    //{
                    //    txtResult11.Focus();
                    //}
                    //else if (sender == this.txtResult11)
                    //{
                    //    txtResult12.Focus();
                    //}
                    //else if (sender == this.txtResult12)
                    //{
                    //    txtResult13.Focus();
                    //}
                    else if (sender == this.txtResult10)
                    {
                        txtResult13.Focus();
                    }
                    else if (sender == this.txtResult13)
                    {
                        txtResult14.Focus();
                    }
                    else if (sender == this.txtResult14)
                    {
                        txtResult15.Focus();
                    }
                    else if (sender == this.txtResult15)
                    {
                        txtResult17.Focus();
                    }
                    else if (sender == this.txtResult17)
                    {
                        txtResult18.Focus();
                    }
                    else if (sender == this.txtResult18)
                    {
                        txtResult19.Focus();
                    }
                    else if (sender == this.txtResult19)
                    {
                        txtResult20.Focus();
                    }
                    else if (sender == this.txtResult20)
                    {
                        txtResult21.Focus();
                    }
                    else if (sender == this.txtResult21)
                    {
                        txtResult22.Focus();
                    }
                    else if (sender == this.txtResult22)
                    {
                        txtResult23.Focus();
                    }
                    else if (sender == this.txtResult23)
                    {
                        txtResult24.Focus();
                    }
                    else if (sender == this.txtResult24)
                    {
                        txtResult25.Focus();
                    }
                    else if (sender == this.txtResult25)
                    {
                        txtResult26.Focus();
                    }
                    else if (sender == this.txtResult26)
                    {
                        txtResult27.Focus();
                    }
                    else if (sender == this.txtResult27)
                    {
                        txtResult28.Focus();
                    }
                    else if (sender == this.txtResult28)
                    {
                        txtResult29.Focus();
                    }
                    else if (sender == this.txtResult29)
                    {
                        txtResult30.Focus();
                    }
                    else if (sender == this.txtResult30)
                    {
                        txtResult31.Focus();
                    }
                    else if (sender == this.txtResult31)
                    {
                        txtResult32.Focus();
                    }
                    else if (sender == this.txtResult32)
                    {
                        txtResult33.Focus();
                    }
                    else if (sender == this.txtResult33)
                    {
                        txtResult34.Focus();
                    }
                    else if (sender == this.txtResult34)
                    {
                        groupBox7.Visible = false;
                        txtResult16.Focus();
                        txtResult16.Select(0, 0);
                    }

                    #endregion

                }
            }

        }

        void eRdoCheckEvent(object sender, EventArgs e)
        {
            if (sender == this.optGubun0)
            {
                gGubun = "0";
            }

            else if (sender == this.optGubun1)
            {
                gGubun = "1";
            }
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            if (sender == this.txtResult)
            {
                #region //기능키 체크     

                optGubun1.Checked = true;        

                string s = string.Empty;

                if (e.KeyCode == Keys.F1)
                {
                    s = "F1";
                }
                else if (e.KeyCode == Keys.F2)
                {
                    s = "F2";
                }
                else if (e.KeyCode == Keys.F3)
                {
                    s = "F3";
                }
                else if (e.KeyCode == Keys.F4)
                {
                    s = "F4";
                }
                else if (e.KeyCode == Keys.F5)
                {
                    s = "F5";
                }
                else if (e.KeyCode == Keys.F6)
                {
                    s = "F6";
                }
                else if (e.KeyCode == Keys.F7)
                {
                    s = "F7";
                }
                else if (e.KeyCode == Keys.F8)
                {
                    s = "F8";
                }
                else if (e.KeyCode == Keys.F9)
                {
                    s = "F9";
                }
                else if (e.KeyCode == Keys.F10)
                {
                    s = "F10";
                }
                #endregion

                if (s != "")
                {
                    DataTable dt = sup.sel_Resultward(clsDB.DbCon, Convert.ToInt32(clsType.User.IdNumber), s);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        txtResult.Paste(dt.Rows[0]["WardName"].ToString().Trim()); //현재커서로 붙여넣기                  
                    }
                }
            }
            
            else if (sender == this.txtResult16)
            {
               
                    #region //기능키 체크                    

                    string s = string.Empty;
                    optGubun0.Checked = true;

                    if (e.KeyCode == Keys.F1)
                    {
                        s = "F1";
                    }
                    else if (e.KeyCode == Keys.F2)
                    {
                        s = "F2";
                    }
                    else if (e.KeyCode == Keys.F3)
                    {
                        s = "F3";
                    }
                    else if (e.KeyCode == Keys.F4)
                    {
                        s = "F4";
                    }
                    else if (e.KeyCode == Keys.F5)
                    {
                        s = "F5";
                    }
                    else if (e.KeyCode == Keys.F6)
                    {
                        s = "F6";
                    }
                    else if (e.KeyCode == Keys.F7)
                    {
                        s = "F7";
                    }
                    else if (e.KeyCode == Keys.F8)
                    {
                        s = "F8";
                    }
                    else if (e.KeyCode == Keys.F9)
                    {
                        s = "F9";
                    }
                    else if (e.KeyCode == Keys.F10)
                    {
                        s = "F10";
                    }
                    #endregion

                    if (s != "")
                    {
                        DataTable dt = sup.sel_Resultward(clsDB.DbCon, Convert.ToInt32(clsType.User.IdNumber), s);
                        if (ComFunc.isDataTableNull(dt) == false)
                        {
                            txtResult16.Paste(dt.Rows[0]["WardName"].ToString().Trim()); //현재커서로 붙여넣기                  
                        }
                    }
                
            }
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
                if(cXray_Read.STS01 =="")
                {
                    setResultEC("00", cXray_Read.Remark);
                }
                else
                {
                    setResultEC("02", cXray_Read.Remark);
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
                //clsVbEmr.EXECUTE_TextEmrView(cXray_Read.Pano, clsType.User.IdNumber);
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

        void setDelegate(string argJob,string argResult,string argResultEC)
        {
            if (rSendMsg == null)
            {
                return;
            }            
          
            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = argJob;

            if (argJob == "01" || argJob =="00")
            {
                cXray_Read_Delegate.ResultEC = argResultEC;
            }

            cXray_Read_Delegate.Sogen = argResult;

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            this.Hide();
        }

        void setBase()
        {

            //txtResult16.Text = "1. ECG : NSR" + "\r\n" + "2. No RWMA" + "\r\n" + "3. Normal dimension of LA, LV, RA, RV" + "\r\n" + "4. Valvular morphology and function : normal " + "\r\n" + "5. Normal LV systolic function" + "\r\n" + "6. Normal LV diastolic function" + "\r\n" + "7. No evidence of pericardial effusion" + "\r\n" + "8. No evidence of shunt" + "\r\n" + "9. No evidence of Mass";
            txtResult16.Text = "1. ECG : NSR"  + "\r\n" + "2. Normal dimension of LA, LV, RA, RV" + "\r\n" + "3. Valvular morphology and function : normal " + "\r\n" + "4. Normal LV systolic function" + "\r\n" + "5. Normal LV diastolic function" + "\r\n" + "6. No evidence of pericardial effusion" + "\r\n" + "7. No evidence of shunt" + "\r\n" + "8. No evidence of Mass";
        }






        void setResultEC(string argJob,string argResult)
        {
            string s = string.Empty;
            string s3 = string.Empty;
            string sEC = string.Empty;  //ResultEC를 담기 위한 변수

            int i = 0;
            
            #region //입력값을 변수에 담기
            if (txtResult0.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult0.Text.Trim() + "#$";
            }
            if (txtResult1.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult1.Text.Trim() + "#$";
            }
            if (txtResult2.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult2.Text.Trim() + "#$";
            }
            if (txtResult3.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult3.Text.Trim() + "#$";
            }
            if (txtResult4.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult4.Text.Trim() + "#$";
            }
            if (txtResult5.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult5.Text.Trim() + "#$";
            }
            if (txtResult6.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult6.Text.Trim() + "#$";
            }
            if (txtResult7.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult7.Text.Trim() + "#$";
            }
            if (txtResult8.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult8.Text.Trim() + "#$";
            }
            if (txtResult9.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult9.Text.Trim() + "#$";
            }
            if (txtResult10.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult10.Text.Trim() + "#$";
            }
            if (txtResult11.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult11.Text.Trim() + "#$";
            }
            if (txtResult12.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult12.Text.Trim() + "#$";
            }
            if (txtResult13.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult13.Text.Trim() + "#$";
            }
            if (txtResult14.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult14.Text.Trim() + "#$";
            }
            if (txtResult15.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult15.Text.Trim() + "#$";
            }
            if (txtResult16.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult16.Text.Trim() + "#$";
            }

            if (txtResult17.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult17.Text.Trim() + "#$";
            }
            if (txtResult18.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult18.Text.Trim() + "#$";
            }
            if (txtResult19.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult19.Text.Trim() + "#$";
            }
            if (txtResult20.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult20.Text.Trim() + "#$";
            }
            if (txtResult21.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult21.Text.Trim() + "#$";
            }
            if (txtResult22.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult22.Text.Trim() + "#$";
            }
            if (txtResult23.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult23.Text.Trim() + "#$";
            }
            if (txtResult24.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult24.Text.Trim() + "#$";
            }
            if (txtResult25.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult25.Text.Trim() + "#$";
            }
            if (txtResult26.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult26.Text.Trim() + "#$";
            }
            if (txtResult27.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult27.Text.Trim() + "#$";
            }
            if (txtResult28.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult28.Text.Trim() + "#$";
            }
            if (txtResult29.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult29.Text.Trim() + "#$";
            }
            if (txtResult30.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult30.Text.Trim() + "#$";
            }
            if (txtResult31.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult31.Text.Trim() + "#$";
            }
            if (txtResult32.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult32.Text.Trim() + "#$";
            }
            if (txtResult33.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult33.Text.Trim() + "#$";
            }
            if (txtResult34.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult34.Text.Trim() + "#$";
            }
            if (txtResult35.Text.Trim() == "")
            {
                s += " " + "#$";
            }
            else
            {
                s += txtResult35.Text.Trim() + "#$";
            }
        


            s += txtResult.Text.Trim() == "" ? " " + "#$" : txtResult.Text.Trim() + "#$";
            s += txtBSA.Text.Trim() == "" ? " " + "#$" : txtBSA.Text.Trim() + "#$";
            s += txtBPRE.Text.Trim() == "" ? " " + "#$" : txtBPRE.Text.Trim() + "#$";
            s += txtPulse.Text.Trim() == "" ? " " + "#$" : txtPulse.Text.Trim() + "#$" + "$%";
            #endregion

            if (argJob == "00")
            {
                string strDrRemark = "";

                strDrRemark = txtClinical.Text.Trim();
                if (txtClinical.Text.Trim() =="내용없음.")
                {
                    strDrRemark = "";
                }
                sEC = s;
                
                s3 = cRead.Xray_Read_EC(s, strDrRemark);
                setDelegate("00", s3, sEC);
            }
            else if (argJob == "01" || argJob =="02")
            {
                string[] s2 = new string[41];
                int j = 1;
                int k = 1;
                int l = 0;

                

                if (argJob =="02")
                {
                    //s = s;
                }
                else
                {
                    s = argResult.Trim();
                }

                if (s.Length > 10)
                {
                    cXray_Read.Gubun3 = "E"; //자료가져오기 구분
                }

                for ( i = 1; i <= s.Length; i++)
                {
                    if (VB.Mid(s, i, 2) == "#$")
                    {
                        s2[k - 1] = VB.Mid(s, j, l) == "#$" ? "" : VB.Mid(s, j, l);

                        if (cXray_Read.Gubun3 == "E")
                        {
                            if (k <= 36)
                            {
                                #region //기본값 1
                                if (k == 1)
                                {
                                    txtResult0.Text = s2[k - 1];
                                }
                                else if (k == 2)
                                {
                                    txtResult1.Text = s2[k - 1];
                                }
                                else if (k == 3)
                                {
                                    txtResult2.Text = s2[k - 1];
                                }
                                else if (k == 4)
                                {
                                    txtResult3.Text = s2[k - 1];
                                }
                                else if (k == 5)
                                {
                                    txtResult4.Text = s2[k - 1];
                                }
                                else if (k == 6)
                                {
                                    txtResult5.Text = s2[k - 1];
                                }                                
                                else if (k == 7)
                                {
                                    txtResult6.Text = s2[k - 1];
                                }
                                else if (k == 8)
                                {
                                    txtResult7.Text = s2[k - 1];
                                }
                                else if (k == 9)
                                {
                                    txtResult8.Text = s2[k - 1];
                                }
                                else if (k == 10)
                                {
                                    txtResult9.Text = s2[k - 1];
                                }
                                else if (k == 11)
                                {
                                    txtResult10.Text = s2[k - 1];
                                }
                                else if (k == 12)
                                {
                                    txtResult11.Text = s2[k - 1];
                                }
                                else if (k == 13)
                                {
                                    txtResult12.Text = s2[k - 1];
                                }
                                else if (k == 14)
                                {
                                    txtResult13.Text = s2[k - 1];
                                }
                                else if (k == 15)
                                {
                                    txtResult14.Text = s2[k - 1];
                                }
                                else if (k == 16)
                                {
                                    txtResult15.Text = s2[k - 1];
                                }
                                else if (k == 17)
                                {
                                    txtResult16.Text = s2[k - 1];
                                }
                                else if (k == 18)
                                {
                                    txtResult17.Text = s2[k - 1];
                                }
                                else if (k == 19)
                                {
                                    txtResult18.Text = s2[k - 1];
                                }
                                else if (k == 20)
                                {
                                    txtResult19.Text = s2[k - 1];
                                }
                                else if (k == 21)
                                {
                                    txtResult20.Text = s2[k - 1];
                                }
                                else if (k == 22)
                                {
                                    txtResult21.Text = s2[k - 1];
                                }
                                else if (k == 23)
                                {
                                    txtResult22.Text = s2[k - 1];
                                }
                                else if (k == 24)
                                {
                                    txtResult23.Text = s2[k - 1];
                                }
                                else if (k == 25)
                                {
                                    txtResult24.Text = s2[k - 1];
                                }
                                else if (k == 26)
                                {
                                    txtResult25.Text = s2[k - 1];
                                }
                                else if (k == 27)
                                {
                                    txtResult26.Text = s2[k - 1];
                                }
                                else if (k == 28)
                                {
                                    txtResult27.Text = s2[k - 1];
                                }
                                else if (k == 29)
                                {
                                    txtResult28.Text = s2[k - 1];
                                }
                                else if (k == 30)
                                {
                                    txtResult29.Text = s2[k - 1];
                                }
                                else if (k == 31)
                                {
                                    txtResult30.Text = s2[k - 1];
                                }
                                else if (k == 32)
                                {
                                    txtResult31.Text = s2[k - 1];
                                }
                                else if (k == 33)
                                {
                                    txtResult32.Text = s2[k - 1];
                                }
                                else if (k == 34)
                                {
                                    txtResult33.Text = s2[k - 1];
                                }
                                else if (k == 35)
                                {
                                    txtResult34.Text = s2[k - 1];
                                }
                                else if (k == 36)
                                {
                                    txtResult35.Text = s2[k - 1];
                                }
                                #endregion
                            }
                            else if (k == 37)
                            {
                                txtResult.Text = s2[k - 1];
                            }
                            else if (k == 38)
                            {
                                txtBSA.Text = s2[k - 1];
                            }
                            else if (k == 39)
                            {
                                txtBPRE.Text = s2[k - 1];
                            }
                            else if (k == 40)
                            {
                                txtPulse.Text = s2[k - 1];
                            }
                        }

                        i = i + 2;
                        j = i;
                        k++;
                        l = 0;

                    }
                    else if (VB.Mid(s, i, 2) == "$%")
                    {
                        l = s.Length;
                    }

                    l++;
                }

                if (argJob == "01" || argJob == "02" && cXray_Read.Gubun3 == "E")
                {
                    sEC = s;
                    s = cRead.Xray_Read_EC(s, "");
                    setDelegate("01", s, sEC);
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

            txtResult17.Text = "1";
            txtResult18.Text = "1";
            txtResult19.Text = "1";
            txtResult20.Text = "1";
            txtResult21.Text = "1";
            txtResult22.Text = "1";
            txtResult23.Text = "1";
            txtResult24.Text = "1";
            txtResult25.Text = "1";
            txtResult26.Text = "1";
            txtResult27.Text = "1";
            txtResult28.Text = "1";
            txtResult29.Text = "1";
            txtResult30.Text = "1";
            txtResult31.Text = "1";
            txtResult32.Text = "1";
            txtResult33.Text = "1";
            txtResult34.Text = "1";
        }

        void screen_display()
        {
            //기본표시
            setBase();
            
            //            
            GetData(clsDB.DbCon);
        }                

        void GetData(PsmhDb pDbCon)
        {
            //int i = 0;
            string s = string.Empty;
            DataTable dt = null;
            
            Cursor.Current = Cursors.WaitCursor;

            #region //변수세팅 및 의사참고사항
            cXrayDetail = new clsComSupXraySQL.cXrayDetail();
            cXrayDetail.Job = "06";
            cXrayDetail.Pano = cXray_Read.Pano;
            cXrayDetail.XJong = cXray_Read.XJong;
            cXrayDetail.SeekDate = cXray_Read.SeekDate;
            #endregion

            txtClinical.Text = cRead.Xray_Detail_DrRemark(pDbCon, cXrayDetail);
            
            //입원대상이면 키,몸무게 체크
            if(cXray_Read.GbIO == "입원")
            {
                dt = cSQL.sel_IPD_NEW_MASTER(pDbCon, "02", cXray_Read.Pano);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    txtResult0.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                    txtResult1.Text = dt.Rows[0]["WEIGHT"].ToString().Trim();
                }
                else
                {
                    dt = cEMR.sel_EMRXML(pDbCon, "00", cXray_Read.Pano, "");

                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        txtResult0.Text = dt.Rows[0]["IT10"].ToString().Trim();
                        txtResult1.Text = dt.Rows[0]["IT11"].ToString().Trim();
                    }

                }
            }

            dt = cEMR.sel_EMRXML(pDbCon, "00", cXray_Read.Pano, "1");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                txtBPRE.Text = dt.Rows[0]["IT02"].ToString().Trim() + "/" + dt.Rows[0]["IT03"].ToString().Trim();
                txtPulse.Text = dt.Rows[0]["IT04"].ToString().Trim();
            }

            //기존판독 내용 있으면
            if (cXray_Read.Remark.Trim().Length > 40)
            {
                setResultEC("01",cXray_Read.Remark);
                
            }    


            Cursor.Current = Cursors.Default;

        }

        private void btnRegional_Click(object sender, EventArgs e)
        {
            if(groupBox7.Visible == false)
            {
                groupBox7.Visible = true;
            }
            else
            {
                groupBox7.Visible = false;
                    
            }
        }
    }
}
