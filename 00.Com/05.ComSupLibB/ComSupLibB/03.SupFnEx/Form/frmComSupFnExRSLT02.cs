using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : SupFnEx
    /// File Name       : frmComSupFnExRSLT02.cs
    /// Description     : 치매척도 검사 결과 등록 관리 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-08-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// SFTP 
    /// 
    /// </history>
    /// <seealso cref= "\ocs\dementia\FrmFementia.frm(Dementia01.frm) >> frmSupFnExRSLT05.cs c#에서 폼 추가함" />
    public partial class frmComSupFnExRSLT02 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        ComFunc fun = new ComFunc();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComEmrSQL emrSql = new clsComEmrSQL();
        clsComSupFnExSpd fnexSpd = new clsComSupFnExSpd();
        clsComSupFnExSQL fnexSql = new clsComSupFnExSQL();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSupFnEx fnex = new clsComSupFnEx();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();

        clsComSupFnExSQL.c_Etc_Result_Dementia c_Etc_ResultDementia = null;

        string gROWID = "";

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

        public frmComSupFnExRSLT02()
        {
            InitializeComponent();

            setEvent();
        }

        public frmComSupFnExRSLT02(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;

            setEvent();
        }


        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {

            dtpFDate.Text = cpublic.strSysDate;
            dtpTDate.Text = cpublic.strSysDate;

            txtResultSabun.Text = clsPublic.GstrJobName;

            setCombo(pDbCon);

        }
        
        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);

            this.btnSave.Click += new EventHandler(eBtnEvent);            
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);
            

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);


            //this.txtPano.KeyDown += eTxtKeyDown;

            this.dtpFDate.TextChanged += new EventHandler(eDtpTxtChange);
            this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelected);



        }

        void setAuth()
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon,this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                setAuth();

                //시트
                fnexSpd.sSpd_enmSupFnExRSLT05(ssList, fnexSpd.sSpdenmSupFnExRSLT05, fnexSpd.nSpdenmSupFnExRSLT05, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear("1");

                setCtrlData(clsDB.DbCon);
            }
                        
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

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                //                    
                this.Close();
                return;
            }
            else if (sender == this.btnSearch)
            {
                screen_display();
            }
            else if (sender == this.btnSave) 
            {

                eSave(clsDB.DbCon,"저장",gROWID);
                
            }                   
            else if (sender == this.btnCancel)
            {
                screen_clear();
            }
            else if (sender == this.btnDelete)
            {
                eSave(clsDB.DbCon,"삭제", gROWID);
                               
            }


        }

        void eOptChecked(object sender, EventArgs e)
        {
            //if (sender == this.optJob0 || sender == this.optJob1 || sender == this.optJob2)
            //{
            //    screen_display();
            //}
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {

            gROWID = "";

            FpSpread o = (FpSpread)sender;
            
            if (e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }
           

            if (sender == this.ssList)
            {
                gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmSupFnExRSLT05.ROWID].Text.Trim();
                
                if (e.Column == (int)clsComSupFnExSpd.enmSupFnExRSLT05.Pano)
                {
                    //EMR 뷰어                
                    //clsVbEmr.EXECUTE_TextEmrView(o.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmSupFnExRSLT05.Pano].Text.Trim(), clsType.User.IdNumber);
                    clsVbEmr.EXECUTE_NewTextEmrView(o.ActiveSheet.Cells[e.Row, (int)clsComSupFnExSpd.enmSupFnExRSLT05.Pano].Text.Trim(), clsType.User.IdNumber);
                }
                else
                {     
                    if (gROWID != "")
                    {
                        one_data_display(clsDB.DbCon, gROWID);
                    }
                }                               

            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtPano)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {


            //    }
            //}


        }

        void eDtpTxtChange(object sender, EventArgs e)
        {

        }

        void eCboSelected(object sender, EventArgs e)
        {

            DataTable dt = sup.sel_Bas_Doctor_ComBo(clsDB.DbCon,VB.Left(this.cboDept.SelectedItem.ToString(), 2), "", false);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDoct, dt, clsParam.enmComParamComboType.NULL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

        }

        void eSave(PsmhDb pDbCon, string argJob,string argROWID)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            #region 클래스 초기화 및 변수 세팅

            c_Etc_ResultDementia = new clsComSupFnExSQL.c_Etc_Result_Dementia();

            c_Etc_ResultDementia.Pano = txtPano.Text.Trim();  
            c_Etc_ResultDementia.sName =  txtSName.Text.Trim();
            c_Etc_ResultDementia.Sex =    txtSex.Text.Trim();
            c_Etc_ResultDementia.Age =    Convert.ToInt32(txtAge.Text);
            c_Etc_ResultDementia.BDate =  dtpBDate.Text.Trim();
            c_Etc_ResultDementia.JDate =  dtpJDate.Text.Trim();
            c_Etc_ResultDementia.DeptCode= clsComSup.setP(cboDept.Text.Trim(),".",1);
            c_Etc_ResultDementia.DrCode = clsComSup.setP(cboDoct.Text.Trim(),".",1);
            c_Etc_ResultDementia.GbIO =   cboIO.Text.Trim();
            c_Etc_ResultDementia.RDate =  dtpRDate.Text.Trim();
            c_Etc_ResultDementia.Result = txtResult.Text.Trim().ToUpper();
            c_Etc_ResultDementia.OrderCode = clsComSup.setP(cboOrder.Text.Trim(),".",1);            
            c_Etc_ResultDementia.sabun = Convert.ToUInt32(clsType.User.IdNumber);
            c_Etc_ResultDementia.ROWID =   argROWID;

            #endregion
                       
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (argJob == "저장")
                {

                    if (c_Etc_ResultDementia.ROWID != "" && gROWID != "")
                    {
                        c_Etc_ResultDementia.Gubun = "1";

                        SqlErr = fnexSql.up_ETC_RESULT_DEMENTIA(pDbCon, c_Etc_ResultDementia, ref intRowAffected);
                    }
                    else
                    {
                        c_Etc_ResultDementia.Gubun = "0";
                        SqlErr = fnexSql.ins_ETC_RESULT_DEMENTIA(pDbCon, c_Etc_ResultDementia, ref intRowAffected);
                    }


                }
                else if (argJob == "삭제")
                {
                    if (c_Etc_ResultDementia.ROWID != "" && gROWID != "")
                    {
                        c_Etc_ResultDementia.DDate = "DEL";
                        SqlErr = fnexSql.up_ETC_RESULT_DEMENTIA(pDbCon, c_Etc_ResultDementia, ref intRowAffected); 
                    }

                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
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


            //
            c_Etc_ResultDementia = new clsComSupFnExSQL.c_Etc_Result_Dementia();

            screen_clear();

            screen_display();
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();
                       
            //
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is TextBox)
                {
                    if (((TextBox)ctl).Name != "txtResultSabun" )
                    {
                        ctl.Text = "";
                    }
                }                
                else if (ctl is ComboBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is DateTimePicker)
                {
                    if (((DateTimePicker)ctl).Name !="dtpFDate" && ((DateTimePicker)ctl).Name != "dtpTDate")
                    {
                        ctl.Text = "";
                    }
                                        
                }
            }
            
            
        }

        void setCombo(PsmhDb pDbCon)
        {

            DataTable dt = null;
            
            cboDept.Items.Clear();
            
            dt = comSql.sel_BAS_CLINICDEPT_COMBO(pDbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDept, dt, clsParam.enmComParamComboType.None);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }
            
            cboDept.SelectedIndex = 0;



            cboOrder.Items.Clear();

            dt = fnexSql.sel_OCS_ORDERCODE_dementia(pDbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboOrder, dt, clsParam.enmComParamComboType.NULL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

            cboOrder.SelectedIndex = 0;

        

            cboDoct.Items.Clear();
            cboDoct.Items.Add(" ");
            cboDoct.SelectedIndex = 0;

            cboIO.Items.Clear();            
            cboIO.Items.Add("O");
            cboIO.Items.Add("I");
            cboIO.SelectedIndex = -1;            

        }

        void screen_display()
        {
            string strDept = "ALL";
            string strGubun = "ALL";
            string strSpano = txtSpano.Text.Trim();

            if (optGubun1.Checked ==true)
            {
                strGubun = "0";
            }
            else if (optGubun2.Checked == true)
            {
                strGubun = "1";
            }

            if (optPart1.Checked == true)
            {
                strDept = " 'NP' ";
            }
            else if (optPart2.Checked == true)
            {
                strDept = " 'NE' ";
            }
            else if (optPart3.Checked == true)
            {
                strDept = " 'NS','RM' ";
            }

            GetData(clsDB.DbCon,ssList, strGubun, dtpFDate.Text.Trim(), dtpTDate.Text.Trim(), strDept ,strSpano);

            gROWID = "";
            c_Etc_ResultDementia = new clsComSupFnExSQL.c_Etc_Result_Dementia();

        }

        void one_data_display(PsmhDb pDbCon, string argROWID)
        {

            screen_clear("");

            DataTable dt = fnexSql.sel_ETC_RESULT_DEMENTIA(pDbCon,"", "", "", "",argROWID);                      
            
            if (ComFunc.isDataTableNull(dt) == false)
            {
                
                txtPano.Text = dt.Rows[0]["Ptno"].ToString().Trim();
                txtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                txtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                txtAge.Text = dt.Rows[0]["Age"].ToString().Trim();
                dtpBDate.Text = dt.Rows[0]["BDate"].ToString().Trim();
                dtpJDate.Text = dt.Rows[0]["JDate"].ToString().Trim() == "" ? dtpBDate.Text : dt.Rows[0]["JDate"].ToString().Trim();
                cboDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim() + "." + clsVbfunc.GetBASClinicDeptNameK(pDbCon, dt.Rows[0]["DeptCode"].ToString().Trim());
                cboDoct.Text = dt.Rows[0]["DrCode"].ToString().Trim() + "." + dt.Rows[0]["DrName"].ToString().Trim();
                cboIO.Text = dt.Rows[0]["GbIO"].ToString().Trim();
                dtpRDate.Text = dt.Rows[0]["RDate"].ToString().Trim() == "" ? dtpBDate.Text : dt.Rows[0]["RDate"].ToString().Trim();
                txtResult.Text = dt.Rows[0]["Result"].ToString().Trim().ToUpper();
                txtResultSabun.Text = dt.Rows[0]["ResultSabun"].ToString().Trim();
                cboOrder.Text = dt.Rows[0]["ORDERCODE"].ToString().Trim() + "." + dt.Rows[0]["ORDERNAME"].ToString().Trim();

            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd,string argGubun,  string argFDate, string argTDate, string argDept, string argSpano = "")
        {
            int i = 0;
            int nRow = -1;
            DataTable dt = null;   

            read_sysdate();

            screen_clear();

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

            #region //치매척도검사 쿼리 및 데이터셋 읽어 자료 표시

            dt = fnexSql.sel_ETC_RESULT_DEMENTIA(pDbCon, argGubun, argFDate, argTDate, argDept, "", argSpano);

            if (ComFunc.isDataTableNull(dt) == false)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                                       
                    nRow++;

                    if (nRow >= Spd.ActiveSheet.RowCount)
                    {
                        Spd.ActiveSheet.RowCount = nRow + 1;
                    }
                        
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.Age].Text = dt.Rows[i]["Age"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.gbIO].Text = dt.Rows[i]["GbIO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.ExName].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.OrderNo].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupFnExSpd.enmSupFnExRSLT05.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();


                }

            }

            dt.Dispose();
            dt = null;

         

            #endregion


            Spd.ActiveSheet.RowCount = nRow + 1;

            Cursor.Current = Cursors.Default;

        }

    
    }
}
