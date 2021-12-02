using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsSET01.cs
    /// Description     : 내시경 상용구 등록 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-10-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\\endormk\ENDORM03.frm(frmAllResult) >> frmComSupEndsSET01.cs 폼이름 재정의" />
    /// 
    public partial class frmComSupEndsSET01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery Query = new clsQuery();
        clsComSupEndsSQL cendsSQL = new clsComSupEndsSQL();
        clsComSupEndsSpd cendsSpd = new clsComSupEndsSpd();

        clsComSupEndsSQL.cEndo_SResult cEndo_SResult = null;

        string gJong = "";
        string gJongSub = "";
        string gROWID = "";

        #endregion

        public frmComSupEndsSET01()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            read_jong_set("1");
            setCombo(pDbCon);

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);

            //this.btnSearch1.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnDelete.Click += new EventHandler(eBtnSave);
                        

            //시트관련 이벤트
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);

            this.cboJong.SelectedIndexChanged += new EventHandler(eCboSelChanged);
            this.cboJongSub.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            //this.dtpFDate.ValueChanged += new EventHandler(eDtpValueChanged);

            //this.txtPtno.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.txtNrSabun.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.chkAll.CheckedChanged += new EventHandler(eChkChange);
           
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
                cendsSpd.sSpd_enmEndsSet(ssList, cendsSpd.sSpdEndsSet, cendsSpd.nSpdEndsSet, 1, 0);
                

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            


                //툴팁
                //ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
                //ssList.TextTipDelay = 1000;


                screen_clear();

                setCtrlData(clsDB.DbCon);

                //screen_display();
                optEveryone.Checked = true;
                
            }

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnNew)
            {
                screen_clear();
                txtTilte.ReadOnly = false;
                txtTilte.Select();
                

            }

        }

        void eBtnSearch(object sender, EventArgs e)
        {
            //if (sender == this.btnSearch1)
            //{
            //    screen_display();
            //}

        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon,"저장");
            }
            else if (sender == this.btnDelete)
            {
                eSave(clsDB.DbCon,"삭제");
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
       
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

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

            if (sender == this.ssList)
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }

                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;

                screen_clear();

                string s1 = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmEndsSet.Jong].Text.Trim();
                string s2 = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmEndsSet.JongSub].Text.Trim();
                string s3 = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmEndsSet.Title].Text.Trim();
                gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmEndsSet.ROWID].Text.Trim();

                read_jong_set(s1);

                screen_display2(s1, s2, s3);


            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            //FpSpread o = (FpSpread)sender;

            //if (e.Row < 0 || e.Column < 0) return;

            ////마우스 우클릭
            //if (e.Button == MouseButtons.Right)
            //{
            //    return;
            //}

            //if (e.ColumnHeader == true)
            //{
            //    clsSpread.gSpdSortRow(o, e.Column);
            //    return;
            //}
            //else if (sender == this.ssList)
            //{
               

            //    for (int i = 0; i < o.ActiveSheet.RowCount; i++)
            //    {
            //        o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
            //    }

            //    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;

            //    //gPtno = o.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsSet02A.Ptno].Text.Trim();
                

            //    //if (gPtno != "")
            //    //{
            //    //    screen_display2(gPtno);
            //    //}
            
            //}
        }        
        
        void eSave(PsmhDb pDbCon,string argJob)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            #region //값체크
            if (chkAll.Checked == false && (optPartA1.Checked == false && optPartA2.Checked == false && optPartA3.Checked == false && optPartA4.Checked == false && optPartA5.Checked == false))
            {
                ComFunc.MsgBox("세부구분을 선택후 작업하세요!!(상용결과전체 혹은,, 구분 개별선택!!)");
                return;
            }
            if (txtTilte.Text.Trim() =="")
            {
                ComFunc.MsgBox("상용결과 제목이 공란입니다..!!)");
                txtTilte.Select();
                return;
            }

            #endregion

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                #region //클래스 생성 및 변수세팅
                cEndo_SResult = new clsComSupEndsSQL.cEndo_SResult();
                cEndo_SResult.GbJob = clsComSup.setP(cboJong.SelectedItem.ToString(), ".", 1).Trim();
                cEndo_SResult.GbGubun = gJongSub;
                if (chkAll.Checked == true)
                {
                    cEndo_SResult.GbGubun = "A";
                }
                else
                {
                    if (optPartA1.Checked == true)
                    {
                        cEndo_SResult.GbGubun = "1";
                    }
                    else if (optPartA2.Checked == true)
                    {
                        cEndo_SResult.GbGubun = "2";
                    }
                    else if (optPartA3.Checked == true)
                    {
                        cEndo_SResult.GbGubun = "3";
                    }
                    else if (optPartA4.Checked == true)
                    {
                        cEndo_SResult.GbGubun = "4";
                    }

                }

                cEndo_SResult.RemarkName = ComFunc.QuotConv(txtTilte.Text.Trim());

                if (cEndo_SResult.GbJob == "3") //대장
                {
                    cEndo_SResult.Remark1 = ComFunc.QuotConv(txtPartA1.Text.Trim());
                    cEndo_SResult.Remark2 = ComFunc.QuotConv(txtPartA4.Text.Trim());
                    cEndo_SResult.Remark3 = ComFunc.QuotConv(txtPartA5.Text.Trim());
                    cEndo_SResult.Remark4 = ComFunc.QuotConv(txtPartA2.Text.Trim());
                    cEndo_SResult.Remark5 = ComFunc.QuotConv(txtPartA3.Text.Trim());
                }
                else
                {
                    cEndo_SResult.Remark1 = ComFunc.QuotConv(txtPartA1.Text.Trim());
                    cEndo_SResult.Remark2 = ComFunc.QuotConv(txtPartA2.Text.Trim());
                    cEndo_SResult.Remark3 = ComFunc.QuotConv(txtPartA3.Text.Trim());
                    cEndo_SResult.Remark4 = ComFunc.QuotConv(txtPartA4.Text.Trim());
                    cEndo_SResult.Remark5 = ComFunc.QuotConv(txtPartA5.Text.Trim());
                }

                cEndo_SResult.ROWID = gROWID;

                #endregion

                #region //기존데이타 체크
                if (argJob == "저장")
                { 
                    DataTable dt = cendsSQL.sel_ENDO_SRESULT(pDbCon,cEndo_SResult.GbJob, cEndo_SResult.GbGubun, cEndo_SResult.RemarkName);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("같은 제목으로 이미 존재합니다.. \r\n제목을 변경하거나 삭제후 작업하십시오!!");
                        txtTilte.Select();
                        return;
                    }
                }
                #endregion

                #region //자료저장 //삭제
                if (argJob =="저장")
                {
                    if (cEndo_SResult.ROWID != "")
                    {
                        //갱신
                        //SqlErr = cendsSQL.up_ENDO_sRESULT(cEndo_SResult, DT, ref intRowAffected);
                    }
                    else
                    {
                        if (optPerson.Checked == true)
                        {
                            SqlErr = cendsSQL.ins_ENDO_sRESULT_PER(pDbCon, cEndo_SResult, ref intRowAffected);
                        }
                        else
                        {
                            SqlErr = cendsSQL.ins_ENDO_sRESULT(pDbCon, cEndo_SResult, ref intRowAffected);
                        }

                    }
                    
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else
                    {
                        clsDB.setCommitTran(pDbCon);
                        ComFunc.MsgBox("저장하였습니다.");
                        screen_clear();
                    }
                }                
                else if (argJob == "삭제")
                {
                    if (cEndo_SResult.ROWID != "")
                    {
                        //삭제
                        SqlErr = cendsSQL.del_ENDO_sRESULT(pDbCon,cEndo_SResult, ref intRowAffected);
                        
                    }
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    else
                    {
                        clsDB.setCommitTran(pDbCon);
                        ComFunc.MsgBox("삭제하였습니다.");
                        screen_clear();
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            screen_clear();

            eCboSelChanged(cboJongSub, null);

        }
        
        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtPtno)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        lblSName.Text = "";
            //        if (txtPtno.Text.Trim() != "")
            //        {
            //            string strPtno = ComFunc.SetAutoZero(txtPtno.Text.Trim(), ComNum.LENPTNO);
            //            txtPtno.Text = strPtno;
            //            lblSName.Text = clsVbfunc.GetPatientName(strPtno);
            //        }
            //    }
            //}
            
        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            string s = "";
            string s1 = "";
            
            ComboBox o = (ComboBox)sender;
            DataTable dt = null;
            
            //조회
            try
            {
                if (o.SelectedItem.ToString() != null)
                {
                    if (sender == this.cboJong)
                    {                       
                        #region //종류체크
                        if (clsComSup.setP(o.SelectedItem.ToString(), ".", 1) == "1")
                        {
                            s = "C#_ENDO_상용구제목_기관지";
                            read_jong_set("1");
                        }
                        else if (clsComSup.setP(o.SelectedItem.ToString(), ".", 1) == "2")
                        {
                            s = "C#_ENDO_상용구제목_위";
                            read_jong_set("2");
                        }
                        else if (clsComSup.setP(o.SelectedItem.ToString(), ".", 1) == "3")
                        {
                            s = "C#_ENDO_상용구제목_장";
                            read_jong_set("3");
                        }
                        else if (clsComSup.setP(o.SelectedItem.ToString(), ".", 1) == "4")
                        {
                            s = "C#_ENDO_상용구제목_ERCP";
                            read_jong_set("4");
                        }
                        #endregion

                        ssList.ActiveSheet.RowCount = 0;

                        //상용구제목
                        dt = Query.Get_BasBcode( clsDB.DbCon, s, "", " Code || '.' || Name Names ", "", " Sort ASC, Code ASC ");
                        if (ComFunc.isDataTableNull(dt) == false)
                        {
                            method.setCombo_View(this.cboJongSub, dt, clsParam.enmComParamComboType.NULL);
                        }

                      
                        
                    }
                    else if (sender == this.cboJongSub)
                    {                        
                        s = clsComSup.setP(this.cboJong.SelectedItem.ToString(), ".", 1).Trim();
                        s1 = clsComSup.setP(o.SelectedItem.ToString(), ".", 1).Trim();

                        if (s1 !="")
                        {                            
                            screen_display(s, s1);
                        }
                        else
                        {
                            screen_clear();
                        }
                        


                    }                    
                }
            }
            catch
            {

            }

        }

        void eChkChange(object sender,EventArgs e)
        {
            if (sender == this.chkAll)
            {
                screen_clear("A1");
            }
     
        }

        void setCombo(PsmhDb pDbCon)
        {
            DataTable dt = null;
            
            //상용구제목
            dt = Query.Get_BasBcode(pDbCon, "C#_ENDO_상용구종류", "", " Code || '.' || Name Names ", "", " Sort ASC, Code ASC ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboJong, dt, clsParam.enmComParamComboType.None);
            }
        }

        void read_jong_set(string argJong)
        {
            gbInput.Visible = true;
            optPartA2.Visible = true;
            optPartA3.Visible = true;
            optPartA5.Visible = true;

            txtPartA5.Visible = true;

            lblsmall.Visible = false;
            lbllarge.Visible = false;
            lblrectum.Visible = false;
            

            #region // 종류별로 항목 설정
            if (argJong =="1") //기관지
            {
                optPartA1.Text = "Vocal cord";
                optPartA2.Text = "Carina";
                optPartA3.Text = "Bronchi";
                optPartA4.Text = "Endosopic Procedure";
                optPartA5.Text = "";
                optPartA5.Visible = false;

                txtPartA1.MaxLength = 250;
                txtPartA1.MaxLength = 100;
                txtPartA1.MaxLength = 500;
                txtPartA1.MaxLength = 100;
                txtPartA5.MaxLength = 0;
                txtPartA5.Visible = false;                                                              
            }
            else if (argJong == "2") //위
            {
                optPartA1.Text = "Esophagus";
                optPartA2.Text = "Stomach";
                optPartA3.Text = "Duodenum";
                optPartA4.Text = "Endosopic Diagnosis";
                optPartA5.Text = "Endosopic Procedure";                

                txtPartA1.MaxLength = 300;
                txtPartA1.MaxLength = 550;
                txtPartA1.MaxLength = 180;
                txtPartA1.MaxLength = 480;
                txtPartA5.MaxLength = 100;                
            }
            else if (argJong == "3") //장
            {
                optPartA1.Text = "Endosopic Findings";
                optPartA2.Text = "large Intestinal";
                optPartA2.Visible = false;
                optPartA3.Text = "rectum";
                optPartA3.Visible = false;
                optPartA4.Text = "Endosopic Diagnosis";
                optPartA5.Text = "Endosopic Procedure";                

                txtPartA1.MaxLength = 600;
                txtPartA1.MaxLength = 600;
                txtPartA1.MaxLength = 600;
                txtPartA1.MaxLength = 360;
                txtPartA5.MaxLength = 100;

                lblsmall.Visible = true;
                lbllarge.Visible = true;
                lblrectum.Visible = true;

            }
            else if (argJong == "4") //ERCP
            {
                optPartA1.Text = "ERCP Findings";
                optPartA2.Text = "Diagnosis";
                optPartA3.Text = "Plan Tx";
                optPartA4.Text = "Endosopic Procedure";
                optPartA5.Text = "";
                optPartA5.Visible = false;

                txtPartA1.MaxLength = 300;
                txtPartA1.MaxLength = 360;
                txtPartA1.MaxLength = 180;
                txtPartA1.MaxLength = 100;
                txtPartA5.MaxLength = 0;
                txtPartA5.Visible = false;
            }
            else
            {
                gbInput.Visible = false;
            }
            #endregion
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            txtTilte.ReadOnly = true; 

            Control[] controls = null;

            if (Job =="")
            {
                gJong = "";
                gJongSub = "";
                gROWID = "";

                //콘트롤 값 clear
                controls = ComFunc.GetAllControls(this);
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
                }
            }
            else if(Job =="A1")
            {
                //콘트롤 값 clear
                controls = ComFunc.GetAllControls(this);
                foreach (Control ctl in controls)
                {                    
 
                    if (ctl is CheckBox)
                    {
                        if (((CheckBox)ctl).Name == "chkAll")
                        {
                            //((CheckBox)ctl).Checked = true;
                        }
                        
                    }
                }
            }
            optPartA1.Checked = false;
            optPartA2.Checked = false;
            optPartA3.Checked = false;
            optPartA4.Checked = false;
            optPartA5.Checked = false;


        }
        
        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
               
        void screen_display(string argJong, string argJongSub)
        {
            GetData(clsDB.DbCon, ssList, argJong, argJongSub);
        }

        void screen_display2(string argJong, string argJongSub, string argTitle)
        {
            GetData2(clsDB.DbCon, argJong, argJongSub, argTitle);
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argJong, string argJongSub)
        {
            int i = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            Spd.ActiveSheet.RowCount = 0;

            if(optPerson.Checked == true)
            {
                dt = cendsSQL.sel_ENDO_SRESULT_PER(pDbCon, argJong, argJongSub, "");
            }
            else
            {
                dt = cendsSQL.sel_ENDO_SRESULT(pDbCon, argJong, argJongSub, "");
            }
            

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {  
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmEndsSet.Jong].Text = dt.Rows[i]["GbJob"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmEndsSet.JongSub].Text = dt.Rows[i]["GbGubun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmEndsSet.Title].Text = dt.Rows[i]["RemarkName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmEndsSet.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }               
            }

            #endregion

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

        }

        void GetData2(PsmhDb pDbCon, string argJong, string argJongSub,string argTitle)
        {           
            DataTable dt = null;

            #region //세부종류 설정
            if (argJong =="3")
            {
                if (argJongSub == "A")
                {
                    chkAll.Checked = true;
                }
                else if (argJongSub == "1")
                {
                    optPartA1.Checked = true;
                }
                else if (argJongSub == "2")
                {
                    optPartA4.Checked = true;
                }
                else if (argJongSub == "3")
                {
                    optPartA5.Checked = true;
                }                
            }
            else
            {
                if (argJongSub == "A")
                {
                    chkAll.Checked = true;
                }
                else if (argJongSub == "1")
                {
                    optPartA1.Checked = true;
                }
                else if (argJongSub == "2")
                {
                    optPartA2.Checked = true;
                }
                else if (argJongSub == "3")
                {
                    optPartA3.Checked = true;
                }
                else if (argJongSub == "4")
                {
                    optPartA4.Checked = true;
                }
                else if (argJongSub == "5")
                {
                    optPartA5.Checked = true;
                }
            }
            
            #endregion

            dt = cendsSQL.sel_ENDO_SRESULT( pDbCon, argJong, argJongSub, argTitle);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                gJong = dt.Rows[0]["GbJob"].ToString().Trim();
                gJongSub = dt.Rows[0]["GbGubun"].ToString().Trim();
                gROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                txtTilte.Text = dt.Rows[0]["RemarkName"].ToString().Trim();
                if (gJong == "3")
                {
                    txtPartA1.Text = dt.Rows[0]["Remark1"].ToString().Trim();
                    txtPartA2.Text = dt.Rows[0]["Remark4"].ToString().Trim();
                    txtPartA3.Text = dt.Rows[0]["Remark5"].ToString().Trim();
                    txtPartA4.Text = dt.Rows[0]["Remark2"].ToString().Trim();
                    txtPartA5.Text = dt.Rows[0]["Remark3"].ToString().Trim();
                }
                else
                {
                    txtPartA1.Text = dt.Rows[0]["Remark1"].ToString().Trim();
                    txtPartA2.Text = dt.Rows[0]["Remark2"].ToString().Trim();
                    txtPartA3.Text = dt.Rows[0]["Remark3"].ToString().Trim();
                    txtPartA4.Text = dt.Rows[0]["Remark4"].ToString().Trim();
                    txtPartA5.Text = dt.Rows[0]["Remark5"].ToString().Trim();
                }
            }

            #endregion

            dt.Dispose();
            dt = null;
            
        }

    }
}
