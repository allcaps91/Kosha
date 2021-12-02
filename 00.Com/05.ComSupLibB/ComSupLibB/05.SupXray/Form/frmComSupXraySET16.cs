using System;
using System.Windows.Forms;
using ComBase;
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using ComDbB;
using System.Data;
using FarPoint.Win.Spread;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : SupXray
    /// File Name       : frmComSupXraySET16.cs
    /// Description     : ASA 임의 등록
    /// Author          : 안정수
    /// Create Date     : 2018-04-16
    /// Update History  :     
    /// </summary>
    /// <history>  
    /// 기존 Frm영상ASA조회.frm(Frm영상ASA조회) 폼 frmSupXrayVIEW05.cs에서 ASA 임의등록 부분 분리
    /// </history>

    public partial class frmComSupXraySET16 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread methodSpd = new clsSpread();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsSpread CS = new clsSpread();
        clsPublic cpublic = new clsPublic();
        ComFunc CF = new ComFunc();
        clsComSupXraySQL cxraySQL = new clsComSupXraySQL();

        clsComSupXraySQL.cXray_ASA cXray_ASA = null;
        clsComSupXraySQL.cXrayDetail cXray_Detail = null;

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        string gRowid = "";

        #endregion

        #region MainFormMessage InterFace

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

        #endregion

        public frmComSupXraySET16(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmComSupXraySET16()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupXraySET16(clsComSupXraySQL.cXrayDetail argCls)
        {
            InitializeComponent();
            cXray_Detail = argCls;
            setEvent();
        }

        void setCtrlData()
        {            
            setCombo();
        }

        void setCtrlInit()
        {
            // clsCompuInfo.SetComputerInfo();
            // DataTable dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "프로그램위치", "접수프로그램위치_내시경실");

            // if (ComFunc.isDataTableNull(dt) == false)
            // {
            // //설정세팅
            // }
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnDel.Click += new EventHandler(eBtnSave);

            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);

            this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            this.txtPano.LostFocus += new EventHandler(TxtPanoLostFocus);

            this.txtPano.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);   
            this.dtpDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboGubun.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboDept.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboDoct.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboWard.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.txtExam.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.txtDrug.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);            

            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDbClick);
        }

        void setCombo()
        {           
            #region 병동 세팅

            //병동
            method.setCombo_View(this.cboWard, sup.sel_Bas_Ward(clsDB.DbCon, "", " WardCode", "", true), clsParam.enmComParamComboType.NULL);
            cboWard.Items.Add("HD");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("RA");
            cboWard.Items.Add("OPD");   //심장초음파

            #endregion
            
            //2018-05-28 안정수, 권현선 간호사 요청으로, TO 추가
            #region 진료과 세팅

            method.setCombo_View(this.cboDept, sup.sel_BAS_CLINICDEPT(clsDB.DbCon, "", " 'ER','MD','HR','II','HC','R6','HD','PT','DT','OC','ER','LM','AN','OM','PC','FM' "), clsParam.enmComParamComboType.NULL);


            #endregion

            #region 구분 세팅 < ASA or CT 조영제 >

            cboGubun.Items.Clear();
            cboGubun.Items.Add("");
            cboGubun.Items.Add("01.진정");
            cboGubun.Items.Add("02.조영제");

            #endregion

            btnSave.Enabled = false;
            btnDel.Enabled = false;

            txtPano.Enabled = false;
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
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등				
                                

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

               
                ssList.ActiveSheet.Columns[4].Visible = false;                                      //DRCODE            
                ssList.ActiveSheet.Columns[ssList.ActiveSheet.Columns.Count - 1].Visible = false;   //ROWID            

                screen_clear();

                if(cXray_Detail != null)
                {
                    btnSave.Enabled = true;

                    txtPano.Text = cXray_Detail.Pano;
                    txtSName.Text = CF.Read_Patient(clsDB.DbCon, ComFunc.SetAutoZero(txtPano.Text.Trim(), ComNum.LENPTNO), "2");
                    dtpDate.Text = cXray_Detail.SeekDate;

                    if(cXray_Detail.GbInfo == "01")
                    {
                        cboGubun.Text = "01.진정";
                    }
                    else if(cXray_Detail.GbInfo == "02")
                    {
                        cboGubun.Text = "02.조영제";
                    }

                    cboDept.Text = cXray_Detail.DeptCode;
                    cboWard.Text = cXray_Detail.WardCode;
                    cboDoct.Text = cXray_Detail.DrCode + "." + cXray_Detail.DrName;

                    txtExam.Text = cXray_Detail.OrderName;
                }
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

            else if (sender == this.btnNew)
            {
                btnNew_Click();                
            }

            else if (sender == this.btnView)
            {
                screen_display("1");
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {

            if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                else
                {
                    eSave(clsDB.DbCon,"저장");
                    screen_display();
                }
            }
            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                else
                {
                    eSave(clsDB.DbCon, "삭제");
                    screen_display();
                }
            }

        }   

        void eCboSelChanged(object sender, EventArgs e)
        {                        
            if (sender == this.cboDept)
            {
                if (cboDept.Text.Trim() !="")
                {
                    method.setCombo_View(this.cboDoct, sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDept.SelectedItem.ToString(), 2), "", false, true, true), clsParam.enmComParamComboType.NULL);
                }
                              
            }            
        }

        void TxtPanoLostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (txtPano.Text != "")
                {
                    txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                }
            }
        }

        void eCtrlKeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.txtPano)
            {
                if(txtPano.Text != "" && e.KeyChar == 13)
                {
                    txtSName.Text = CF.Read_Patient(clsDB.DbCon, ComFunc.SetAutoZero(txtPano.Text.Trim(), ComNum.LENPTNO), "2");
                    SendKeys.Send("{TAB}");
                }
            }

            else if (sender == this.dtpDate)
            {
                if(e.KeyChar == 13 || e.KeyChar == 9)
                {
                    cboGubun.Focus();
                }
            }

            else if (sender == this.dtpDate)
            {
                if (e.KeyChar == 13 || e.KeyChar == 9)
                {
                    cboGubun.Focus();
                }
            }

            else if (sender == this.cboGubun)
            {
                if (e.KeyChar == 13 || e.KeyChar == 9)
                {
                    cboDept.Focus();
                }
            }

            else if (sender == this.cboDept)
            {
                if (e.KeyChar == 13 || e.KeyChar == 9)
                {
                    cboDoct.Focus();
                }
            }

            else if (sender == this.cboDoct)
            {
                if (e.KeyChar == 13 || e.KeyChar == 9)
                {
                    cboWard.Focus();
                }
            }

            else if (sender == this.cboWard)
            {
                if (e.KeyChar == 13 || e.KeyChar == 9)
                {
                    txtExam.Focus();
                }
            }

            else if (sender == this.txtExam)
            {
                if (txtExam.Text.Trim() != "" && (e.KeyChar == 13 || e.KeyChar == 9))
                {
                    txtDrug.Focus();
                }
            }

            else if (sender == this.txtDrug)
            {
                if (txtDrug.Text.Trim() != "" && (e.KeyChar == 13 || e.KeyChar == 9))
                {
                    btnSave.Focus();
                }
            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (sender == this.ssList)
            {
                if (ssList.ActiveSheet.Rows.Count == 0)
                {
                    return;
                }

                ssList.ActiveSheet.Columns[0, ssList.ActiveSheet.Columns.Count - 1].Locked = true;

                ssList.ActiveSheet.Cells[0, 0, ssList.ActiveSheet.RowCount - 1, ssList.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                ssList.ActiveSheet.Cells[e.Row, 0, e.Row, ssList.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

                txtPano.Text = ComFunc.SetAutoZero(ssList.ActiveSheet.Cells[e.Row, 0].Text, ComNum.LENPTNO);
                txtSName.Text = CF.Read_Patient(clsDB.DbCon, ComFunc.SetAutoZero(ssList.ActiveSheet.Cells[e.Row, 0].Text, ComNum.LENPTNO), "2");
                dtpDate.Text = ssList.ActiveSheet.Cells[e.Row, 1].Text;

                if(ssList.ActiveSheet.Cells[e.Row, 2].Text == "진정")
                {
                    cboGubun.Text = "01." + ssList.ActiveSheet.Cells[e.Row, 2].Text;
                }

                else if(ssList.ActiveSheet.Cells[e.Row, 2].Text == "조영제")
                {
                    cboGubun.Text = "02." + ssList.ActiveSheet.Cells[e.Row, 2].Text;
                }
                                
                cboDept.Text = ssList.ActiveSheet.Cells[e.Row, 3].Text;
                cboDoct.Text = ssList.ActiveSheet.Cells[e.Row, 4].Text + "." + ssList.ActiveSheet.Cells[e.Row, 5].Text;
                cboWard.Text = ssList.ActiveSheet.Cells[e.Row, 6].Text;
                txtExam.Text = ssList.ActiveSheet.Cells[e.Row, 7].Text;
                txtDrug.Text = ssList.ActiveSheet.Cells[e.Row, 8].Text;
                gRowid = ssList.ActiveSheet.Cells[e.Row, 9].Text;

                btnDel.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        void eSpreadDbClick(object sender, CellClickEventArgs e)
        {
           if(sender == this.ssList)
            {
                if(ssList.ActiveSheet.Rows.Count == 0)
                {
                    return;
                }

                else
                {
                    if (e.RowHeader == true)
                    {
                        return; // 로우헤더 더블클릭후 삭제시 오류로 인해 return 처리 ( 2018-05-15 안정수 ) 

                        string SQL = "";
                        string SqlErr = ""; //에러문 받는 변수
                        int intRowAffected = 0; //변경된 Row 받는 변수

                        read_sysdate();

                        try
                        {
                            clsDB.setBeginTran(clsDB.DbCon);


                            #region //변수설정
                            cXray_ASA = new clsComSupXraySQL.cXray_ASA();
                            cXray_ASA.Pano = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
                            cXray_ASA.BDate = dtpDate.Text.Trim();
                            cXray_ASA.Gubun = VB.Left(cboGubun.Text.Trim(), 2);
                            cXray_ASA.DeptCode = cboDept.Text.Trim().ToUpper();
                            cXray_ASA.DrCode = VB.Left(cboDoct.Text.Trim(), 4);
                            cXray_ASA.WardCode = cboWard.Text.Trim().ToUpper();
                            cXray_ASA.Remark = ComFunc.QuotConv(txtExam.Text.Trim());
                            cXray_ASA.Remark2 = ComFunc.QuotConv(txtDrug.Text.Trim());
                            cXray_ASA.ROWID = gRowid.Trim();
                            #endregion

                            if (gRowid != "")
                            {
                                if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }

                                SqlErr = cxraySQL.del_XRAY_ASA(clsDB.DbCon, cXray_ASA, ref intRowAffected);
                            }


                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                    
                                return;
                            }

                            else
                            {
                                clsDB.setCommitTran(clsDB.DbCon);

                                btnSave.Enabled = false;
                                btnNew.Enabled = true;

                                btnNew_Click();

                                ComFunc.MsgBox("삭제완료");
                                screen_display();
                            }
                        }

                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                    
                            return;
                        }
                    }
                }
            }
        }

        void btnNew_Click()
        {
            txtPano.Text = "";
            txtSName.Text = "";
            txtExam.Text = "";
            txtDrug.Text = "";
            cboDoct.Text = "";

            setCombo();

            btnSave.Enabled = true;
            txtPano.Enabled = true;
            txtPano.Select();
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            if (Job == "")
            {
                txtPano.Select();
                ssList.ActiveSheet.RowCount = 0;
            }            
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display(string argJob = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
                        
            int i = 0;
           
            string sFDate = Convert.ToDateTime(dtpDate.Text.Trim()).AddDays(-1).ToShortDateString();
            string sTDate = dtpDate.Text.Trim();

            ssList.ActiveSheet.RowCount = 1;
            ssList.ActiveSheet.RowCount = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  PANO, BDATE, DEPTCODE, DRCODE, GUBUN"; 
            SQL += ComNum.VBLF + "  ,WARDCODE, REMARK, Remark2, ROWID";
            SQL += ComNum.VBLF + "  ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) FC_DR";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "XRAY_ASA";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (argJob == "1")
            {
                SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + dtpFDate.Text + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + dtpTDate.Text + "', 'YYYY-MM-DD')";
            }
            else
            {
                SQL += ComNum.VBLF + "      AND BDATE >= TO_DATE('" + sFDate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BDATE <= TO_DATE('" + sTDate + "', 'YYYY-MM-DD')";
            }
            SQL += ComNum.VBLF + "ORDER BY PANO, BDATE, DEPTCODE";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {                    
                    ssList.ActiveSheet.RowCount = dt.Rows.Count;

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 1].Text = VB.Left(dt.Rows[i]["BDATE"].ToString().Trim(), 10);

                        if(dt.Rows[i]["GUBUN"].ToString().Trim() == "01")
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "진정";
                        }

                        else if(dt.Rows[i]["GUBUN"].ToString().Trim() == "02")
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "조영제";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "";
                        }                        
                        ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["FC_DR"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Remark2"].ToString().Trim();
                        ssList.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }        

        void eSave(PsmhDb pDbCon,string argJob)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            read_sysdate();

            try
            {
                clsDB.setBeginTran(pDbCon);

                if (argJob =="저장" || argJob == "삭제")
                {
                    #region //변수설정
                    cXray_ASA = new clsComSupXraySQL.cXray_ASA();
                    cXray_ASA.Pano = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
                    cXray_ASA.BDate = dtpDate.Text.Trim(); 
                    cXray_ASA.Gubun = VB.Left(cboGubun.Text.Trim(), 2);
                    cXray_ASA.DeptCode = cboDept.Text.Trim().ToUpper();
                    cXray_ASA.DrCode = VB.Left(cboDoct.Text.Trim(), 4);
                    cXray_ASA.WardCode = cboWard.Text.Trim().ToUpper();
                    cXray_ASA.Remark = ComFunc.QuotConv(txtExam.Text.Trim());
                    cXray_ASA.Remark2 = ComFunc.QuotConv(txtDrug.Text.Trim());
                    cXray_ASA.ROWID = gRowid.Trim();
                    #endregion

                    #region // XRAY_ASA INSERT, UPDATE

                    if (argJob =="저장")
                    {
                        if (gRowid == "")
                        {
                            SqlErr = cxraySQL.ins_XRAY_ASA(pDbCon, cXray_ASA, ref intRowAffected);
                        }
                        else
                        {
                            SqlErr = cxraySQL.up_XRAY_ASA(pDbCon, cXray_ASA, ref intRowAffected);
                        }
                    }
                    else if (argJob == "삭제")
                    {
                        if (gRowid != "")
                        {
                            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }

                            SqlErr = cxraySQL.del_XRAY_ASA(pDbCon, cXray_ASA, ref intRowAffected);
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

                        btnSave.Enabled = false;
                        btnNew.Enabled = true;
                        
                        btnNew_Click();

                        if (argJob =="저장")
                        {
                            ComFunc.MsgBox("저장완료");
                        }
                        else if (argJob == "삭제")
                        {
                            ComFunc.MsgBox("삭제완료");
                        }

                    }
                    #endregion
                }               
                

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                                    
                return;
            }

            
        }

        #region 사용안함

        void ONE_DATA_SET(string ArgJob, string ArgPano, string ArgBDate, string ArgDept, string ArgDrCode,
                          string ArgWARD, string ArgRemark, string ArgRemark2, string ArgROWID)
        {
            int intRowAffected = 0;

            if (ArgJob == "INSERT")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "XRAY_ASA";
                SQL += ComNum.VBLF + "( PANO,BDATE,DEPTCODE,DRCODE,WARDCODE,REMARK,Remark2 )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + "   '" + ArgPano + "'";
                SQL += ComNum.VBLF + "  ,TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  ,'" + ArgDept + "'";
                SQL += ComNum.VBLF + "  ,'" + ArgDrCode + "'";
                SQL += ComNum.VBLF + "  ,'" + ArgWARD + "'";
                SQL += ComNum.VBLF + "  ,'" + ArgRemark + "'";
                SQL += ComNum.VBLF + "  ,'" + ArgRemark2 + "'";
                SQL += ComNum.VBLF + ")";

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
        }
        #endregion
    
    }
}
