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
    /// File Name       : frmComSupXraySET11.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 기본환경설정폼
    /// Author          : 윤조연
    /// Create Date     : 2018-02-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\xuread04.frm(FrmJobSet) >> frmComSupXraySET11.cs 폼이름 재정의" />
    public partial class frmComSupXraySET11 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();        
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();

        clsComSupXraySQL.cEtc_PCSet cEtc_PCSet = null;

        #endregion

        public frmComSupXraySET11()
        {
            InitializeComponent();
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
            lblSName.Text = sup.read_bas_user(clsDB.DbCon, clsType.User.Sabun);
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);


            //this.btnCancel.Click += new EventHandler(eBtnClick);

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnSet.Click += new EventHandler(eBtnSave);
            //this.btnSave1.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);


            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssOrdList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);            
            ////this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);                        
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);

            //this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            this.optLinkREAD_Y.Click += new EventHandler(eOptEvent);
            this.optLinkREAD_N.Click += new EventHandler(eOptEvent);
            this.optLinkRESULT_Y.Click += new EventHandler(eOptEvent);
            this.optLinkRESULT_N.Click += new EventHandler(eOptEvent);

            //this.txtMCode.KeyDown += new KeyEventHandler(eTxtEvent);
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
                //            
                //cxraySpd.sSpd_enmXraySET10(ssList, cxraySpd.sSpdenmXraySET10, cxraySpd.nSpdenmXraySET10, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                setAuth();

                //
                screen_display();
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            
        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtMCode)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        //GetData2(txtMCode.Text.Trim().ToUpper());
            //    }
            //}

        }

        void ExpandableSplitter1_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;

            //if (ex.Expanded == true)
            //{
            //    panel3.Size = new System.Drawing.Size(882, 20);
            //}
            //else
            //{
            //    panel3.Size = new System.Drawing.Size(882, 280);
            //}
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

        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSet)
            {
                //
                eSave(clsDB.DbCon, "저장");
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

        void eOptEvent(object sender, EventArgs e)
        {
            RadioButton o = (RadioButton)sender;
                        
            if (VB.Left(o.Name, 7) == "optLink")
            {
                if (optLinkREAD_Y.Checked == true || optLinkRESULT_Y.Checked ==true)
                {
                    optLink_Y.Checked = true;
                }
                else if (optLinkREAD_N.Checked == true && optLinkRESULT_N.Checked == true)
                {
                    optLink_N.Checked = true;
                }
            }

        }

        void eSave(PsmhDb pDbCon, string Job)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수       

            if (Job == "저장")
            {
                #region //변수세팅 
                if (optSetA0.Checked == true)
                {
                    cEtc_PCSet.FLAG_PRT = "0";
                }
                else if (optSetA1.Checked == true)
                {
                    cEtc_PCSet.FLAG_PRT = "1";
                }

                if (optSetB0.Checked == true)
                {
                    cEtc_PCSet.FLAG_CAN = "0";
                }
                else if (optSetB1.Checked == true)
                {
                    cEtc_PCSet.FLAG_CAN = "1";
                }

                if (optSetC1.Checked == true)
                {
                    cEtc_PCSet.FLAG_SAVE = "1";
                }
                else if (optSetC2.Checked == true)
                {
                    cEtc_PCSet.FLAG_SAVE = "2";
                }
                else if (optSetC3.Checked == true)
                {
                    cEtc_PCSet.FLAG_SAVE = "3";
                }
                if (optView1.Checked == true)
                {
                    cEtc_PCSet.FLAG_VIEW = "Y";
                }
                else
                {
                    cEtc_PCSet.FLAG_VIEW = "N";
                }
                if (optPView1.Checked == true)
                {
                    cEtc_PCSet.FLAG_PACSVIEW = "Y";
                }
                else
                {
                    cEtc_PCSet.FLAG_PACSVIEW = "N";
                }
                //인피니트 링크관련
                if (optLink_Y.Checked == true)
                {
                    cEtc_PCSet.FLAG_LINK_YN = "Y";
                }
                else
                {
                    cEtc_PCSet.FLAG_LINK_YN = "N";
                }
                if (optLinkREAD_Y.Checked == true)
                {
                    cEtc_PCSet.FLAG_LINK_READ = "Y";
                }
                else
                {
                    cEtc_PCSet.FLAG_LINK_READ = "N";
                }
                if (optLinkRESULT_Y.Checked == true)
                {
                    cEtc_PCSet.FLAG_LINK_RESULT = "Y";
                }
                else
                {
                    cEtc_PCSet.FLAG_LINK_RESULT = "N";
                }

                #endregion

                clsDB.setBeginTran(pDbCon);

                try
                {
                    if (cEtc_PCSet.ROWID =="")
                    {
                        SqlErr = cxraySql.ins_ETC_PCSET(pDbCon, cEtc_PCSet, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cxraySql.up_ETC_PCSET(pDbCon, cEtc_PCSet, ref intRowAffected);
                    }                    

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                    else if (SqlErr == "")
                    {
                        clsDB.setCommitTran(pDbCon);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                }

            }
        }        

        void setXrayBase(clsComSupXraySQL.cEtc_PCSet argCls)
        {
            optSetA0.Checked = true;
            if (argCls.FLAG_PRT =="1")
            {
                optSetA1.Checked = true;
            }

            optSetB0.Checked = true;
            if (argCls.FLAG_CAN == "1")
            {
                optSetB1.Checked = true;
            }

            optSetC3.Checked = true;
            if (argCls.FLAG_SAVE == "1")
            {
                optSetC1.Checked = true;
            }
            else if (argCls.FLAG_SAVE == "2")
            {
                optSetC2.Checked = true;
            }
            optView2.Checked = true;
            if (argCls.FLAG_VIEW =="1")                
            {
                optView1.Checked = true;
            }
            else if (argCls.FLAG_VIEW == "0")
            {
                optView2.Checked = true;
            }
            optPView2.Checked = true;
            if (argCls.FLAG_PACSVIEW == "Y")
            {
                optPView1.Checked = true;
            }
            else if (argCls.FLAG_PACSVIEW == "N")
            {
                optPView2.Checked = true;
            }
            optLink_N.Checked = true;
            if (argCls.FLAG_LINK_YN == "Y")
            {
                optLink_Y.Checked = true;
            }
            else if (argCls.FLAG_LINK_YN == "N")
            {
                optLink_N.Checked = true;
            }
            optLinkREAD_N.Checked = true;
            if (argCls.FLAG_LINK_READ == "Y")
            {
                optLinkREAD_Y.Checked = true;
            }
            else if (argCls.FLAG_LINK_READ == "N")
            {
                optLinkREAD_N.Checked = true;
            }
            optLinkRESULT_N.Checked = true;
            if (argCls.FLAG_LINK_RESULT == "Y")
            {
                optLinkRESULT_Y.Checked = true;
            }
            else if (argCls.FLAG_LINK_RESULT == "N")
            {
                optLinkRESULT_N.Checked = true;
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


        }

        void screen_display()
        {
            GetData(clsDB.DbCon);
            setXrayBase(cEtc_PCSet);
        }

        void GetData(PsmhDb pDbCon)
        {
                                    
            Cursor.Current = Cursors.WaitCursor;

            #region //변수세팅
            cEtc_PCSet = new clsComSupXraySQL.cEtc_PCSet();                        
            #endregion

            cxraySql.set_XRayBase(pDbCon, cEtc_PCSet);
                        

            Cursor.Current = Cursors.Default;

        }

    }
}
