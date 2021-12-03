using ComBase;
using ComDbB;
using ComSupLibB.Com;
using ComSupLibB.SupEnds;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayCLE04.cs
    /// Description     : 영상의학 환자성명 오류 UPDATE
    /// Author          : 윤조연
    /// Create Date     : 2017-12-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuagfa\Xuagfa06.frm(FrmEtc01) >> frmComSupXrayCLE04.cs 폼이름 재정의" />
    public partial class frmComSupXrayCLE04 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsSpread methodSpd = new clsSpread();
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupEndsSQL cendsSql = new clsComSupEndsSQL();
        clsComSupXraySend cxraySend = new clsComSupXraySend();
        clsComSupXray cxray = new clsComSupXray();

        clsComSupXraySQL.cXrayPacsSend2 cXrayPacsSend2 = null;
        //clsComSupXraySend.cXrayPacsOrder cXrayPacsOrder = null;
        clsComSup.cBasPatient cBasPatient = null;


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

        public frmComSupXrayCLE04()
        {
            InitializeComponent();
            setEvent();
        }
               
        void setCtrlData()
        {
            dtpFDate.Text = "2002-10-01";
            dtpTDate.Text = cpublic.strSysDate;
        }

        void setCtrlInit()
        {
            txtErrPano.Focus();
            txtErrPano.Select(); 
        }
                
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);

            this.txtErrPano.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);

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
                //cendsSpd.sSpd_enmComSupEndsCLE01A(ssList, cendsSpd.sSpdenmComSupEndsCLE01A, cendsSpd.nSpdenmComSupEndsCLE01A, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            


                screen_clear("ALL");

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

            }

        }

        void eFormResize(object sender, EventArgs e)
        {
            setCtrlProgress();
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
                eSave(clsDB.DbCon, txtErrPano.Text.Trim(), txtErrSName.Text.Trim(), dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
                //eSave(clsDB.DbCon, txtErrPano.Text.Trim(), txtName.Text.Trim(), dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
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

        void eCboSelChanged(object sender, EventArgs e)
        {
            ComboBox o = (ComboBox)sender;

            //조회
            try
            {
                if (o.SelectedItem.ToString() != null)
                {
                    //screen_display();

                }

            }
            catch
            {

            }

        }

        void eOptClick(object sender, EventArgs e)
        {
            //조회
            screen_display();
        }

        void eDtpValueChanged(object sender, EventArgs e)
        {
            //조회
            //screen_display();
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtErrPano)            
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = ComFunc.SetAutoZero(txtErrPano.Text.Trim(),ComNum.LENPTNO);
                    if (strPano != "") read_pano_info(clsDB.DbCon, "ERR", strPano);

                    if(txtErrSName.Text != "")
                    {                        
                        txtPano.Focus();
                        txtPano.Select();
                    }
                    else
                    {
                        ComFunc.MsgBox("등록번호가 오류입니다.");
                        txtErrPano.Focus();
                        txtErrPano.Select();
                    }
                }
            }
            
            else if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = ComFunc.SetAutoZero(txtPano.Text.Trim(), ComNum.LENPTNO);
                    if (strPano != "") read_pano_info(clsDB.DbCon, "NEW", strPano);

                    if (txtSName.Text != "")
                    {
                        btnSearch.Focus();                        
                    }
                    else
                    {
                        ComFunc.MsgBox("등록번호가 오류입니다.");
                        txtPano.Focus();
                        txtPano.Select();
                    }

                }
            }
        }

        void eSave(PsmhDb pDbCon, string argPano, string argSName, string argDate1, string argDate2)
        {
            if (read_data_check() ==false)
            {
                return;
            }

            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;
            string strUpCols = "";
            string strWheres = "";
            string strhicXrayno = "";
            string strPacsNo = "";
            string strXCode = "";
            string strJepDate = "";
            long nHPano = 0;
            string strROWID = "";

            read_sysdate();


            string s = "";
            s += "정말로 작업을 하시겠습니까?";
            if (ComFunc.MsgBoxQ(s, "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
                        
            //2018-11-16 안정수 추가
            if(txtErrPano.Text == txtPano.Text && txtErrSName.Text == txtSName.Text)
            {
                ComFunc.MsgBox("오류내역의 인적사항과 변경할 내역이 동일합니다.");
                return;
            }

            cBasPatient = sup.sel_Bas_Patient_cls(pDbCon, txtPano.Text.Trim());

            

            try
            {
                clsDB.setBeginTran(pDbCon);

                #region // 등록번호 성명 오류 갱신 작업


                #region 1.XRAY_DETAIL PART

                dt = cxraySql.sel_XRAY_DETAIL_ErrUpDate(pDbCon, argPano, argSName, argDate1, argDate2, "01");

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    for ( i = 0; i < dt.Rows.Count; i++)
                    {
                        strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                        strUpCols = " Pano = '" + txtPano.Text.Trim() + "' , SName = '" + txtSName.Text.Trim() + "' ";
                        SqlErr = cxraySql.up_Xray_Detail(pDbCon,"", strROWID, "", strUpCols, "", ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                        //2018-09-06 안정수, SqlErr 조건잘못되있어서 주석처리함 
                        //if (SqlErr != "")
                        //{
                        strWheres = " AND PANO = '" + txtErrPano.Text + "' AND SName ='" + argSName + "' ";
                        SqlErr = cRead.up_XRAY_RESULTNEW(pDbCon, "", txtErrPano.Text.Trim(), strUpCols, strWheres, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                        //}

                        //if (SqlErr != "")
                        //{
                        strUpCols = " PatID = '" + txtPano.Text.Trim() + "'  ";
                        SqlErr = cxraySql.up_DORDER(pDbCon, "", txtErrPano.Text.Trim(), strUpCols, "", ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                        //}

                        //if (SqlErr != "")
                        //{
                        SqlErr = cxraySql.ins_XRAY_PACSSEND(pDbCon, "01", "3", strROWID, clsType.User.IdNumber, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                        //}

                    }

                }

                #endregion

                #region 2.HIC_XRAY_RESULT PART

                dt = cxraySql.sel_HIC_XRAY_RESULT_ErrUpDate(pDbCon, argPano, argSName, argDate1, argDate2);

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strROWID = dt.Rows[i]["ROWID"].ToString().Trim();
                        strPacsNo = dt.Rows[i]["XRAYNO"].ToString().Trim();
                        strXCode = dt.Rows[i]["XCode"].ToString().Trim();
                        strJepDate = dt.Rows[i]["JepDate"].ToString().Trim();
                        if (dt.Rows[i]["Pano"].ToString().Trim()!="")
                        {
                            nHPano = Convert.ToInt32(dt.Rows[i]["Pano"].ToString().Trim());
                        }                        
                        strhicXrayno = cpublic.strSysDate.Replace("-","") + ComFunc.SetAutoZero(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_PMPA", "SEQ_HIC_XRAYNO").ToString(),5);

                        strUpCols = " Ptno = '" + txtPano.Text.Trim() + "' , SName = '" + txtSName.Text.Trim() + "' , XrayNo = '" + strhicXrayno+ "' , XrayNo2 = '" + dt.Rows[i]["XrayNo"].ToString().Trim() + "'  ";
                        SqlErr = cRead.up_HIC_XRAY_RESULT(pDbCon, strROWID, "", strUpCols, "", ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                        //HIC_PACS_SEND
                        //if (SqlErr == "")
                        //{
                        if (cxraySend.HIC_XRAY_PACS_ADT_ORDER_INSERT(pDbCon,strJepDate, strhicXrayno, strPacsNo, strXCode, nHPano,  txtPano.Text.Trim(), "HDX") == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        } 
                        //}                        

                    }

                }

                #endregion

                #region 3.ENDO_JUPMST PART

                dt = cxraySql.sel_ENDO_JUPMST_ErrUpDate(pDbCon, argPano, argSName, argDate1, argDate2);

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                        strUpCols = " Ptno = '" + txtPano.Text.Trim() + "' , SName = '" + txtSName.Text.Trim() + "',PACSSEND ='*' ";
                        SqlErr = cendsSql.up_ENDO_JUPMST(pDbCon, strROWID, "", strUpCols, "", ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                        //if (SqlErr != "")
                        //{
                        #region 변수값 세팅

                        cXrayPacsSend2 = new clsComSupXraySQL.cXrayPacsSend2();
                        cXrayPacsSend2.Job = "01";
                        cXrayPacsSend2.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
                        cXrayPacsSend2.SendGbn = "3";
                        cXrayPacsSend2.Pano = txtPano.Text.Trim();
                        cXrayPacsSend2.SName = txtSName.Text.Trim();
                        cXrayPacsSend2.Sex = dt.Rows[i]["Sex"].ToString().Trim();
                        cXrayPacsSend2.Age = cBasPatient.Age.ToString();                            
                        cXrayPacsSend2.IpdOpd = dt.Rows[i]["IpdIpd"].ToString().Trim();
                        cXrayPacsSend2.DeptCode = dt.Rows[i]["DeptCode"].ToString().Trim();
                        cXrayPacsSend2.DrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                        cXrayPacsSend2.WardCode = dt.Rows[i]["WardCode"].ToString().Trim();
                        cXrayPacsSend2.RoomCode = dt.Rows[i]["RoomCode"].ToString().Trim();
                        cXrayPacsSend2.XJong = "D";
                        cXrayPacsSend2.XSubCode = "";
                        cXrayPacsSend2.XCode = dt.Rows[i]["OrderCode"].ToString().Trim();
                        cXrayPacsSend2.OrderCode = dt.Rows[i]["OrderCode"].ToString().Trim();
                        cXrayPacsSend2.SeekDate = dt.Rows[i]["RDate2"].ToString().Trim();
                        cXrayPacsSend2.Remark = "";
                        cXrayPacsSend2.XrayRoom = "";
                        cXrayPacsSend2.DrRemark = "";
                        cXrayPacsSend2.INPS = clsType.User.IdNumber;

                        #endregion

                        SqlErr = cxraySql.ins_XRAY_PACSSEND(pDbCon, cXrayPacsSend2, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                        //}

                    }

                }

                #endregion

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("작업완료");
                    screen_display();
                    screen_clear("ALL");
                }

                //if (SqlErr == "")
                //{
                //    //strUpCols = " Pano = '" + txtPano.Text.Trim() + "' ";
                //    //SqlErr = cxraySql.up_XRAY_RESULTNEW(pDbCon, "", txtErrPano.Text.Trim(), strUpCols, "", ref intRowAffected);
                //    if (SqlErr != "")
                //    {
                //        clsDB.setRollbackTran(pDbCon);
                //        ComFunc.MsgBox(SqlErr);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                //        return;
                //    }
                //}



                //ADT
                //if (SqlErr == "")
                //{
                //    ////방사선 오더를 미접수 상태로 변환함
                //    //strUpCols = " GbReserved='1',GbEnd='',MgrNo='',PacsNo='',PacsStudyID='' ";
                //    ////SqlErr = cxraySql.up_Xray_Detail(pDbCon, gROWID, "", strUpCols, "", ref intRowAffected);
                //    //if (SqlErr != "")
                //    //{
                //    //    clsDB.setRollbackTran(pDbCon);
                //    //    ComFunc.MsgBox(SqlErr);
                //    //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                //    //    return;
                //    //}
                //}
                #endregion



            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        void ePrint()
        {
            //clsSpread SPR = new clsSpread();
            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;

            //string strTitle = "";
            //string strHeader = "";
            //string strFooter = "";
            //bool PrePrint = true;


            //#region //시트 히든

            ////ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            //#endregion

            //strTitle = "내시경 결과입력 LIST " + "(" + dtpDate.Text.Trim() + ")";

            //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            //SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            //#region //시트 히든 복원

            ////ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            //#endregion

        }

        void setCombo()
        {

        }

        void setCtrlProgress()
        {
            //Point p = new Point();

            //p.X = (this.panheader2.Size.Width / 2) - 90;
            //if (p.X < 0)
            //{
            //    p.X = 0;
            //}
            //p.Y = this.Progress.Location.Y;

            //this.Progress.Location = p;

        }

        void read_pano_info(PsmhDb pDbCon, string argJob, string argPano)
        {
            DataTable dt = null;

            if (argJob == "ERR")
            {
                txtErrPano.Text = argPano;
                //txtErrSName.Text = fun.Read_Patient(pDbCon, argPano, "2");

                dt = cxraySql.sel_Xray_Detail_Name(pDbCon, argPano, false);

                if (dt.Rows.Count > 0)
                {
                    txtErrSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    dt = cxraySql.sel_Patient_Name(pDbCon, argPano, false);

                    if (dt.Rows.Count > 0)
                    {
                        txtErrSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

            }

            else if (argJob == "NEW")
            {
                txtPano.Text = argPano;
                txtSName.Text = fun.Read_Patient(pDbCon, argPano, "2");
            }


        }

        bool read_data_check()
        {
            if (txtErrPano.Text.Trim() == "" || txtErrSName.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호와 성명을 넣고 작업하세요!!");
                txtPano.Focus();
                return false;
            }
            else if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("변경될 등록번호를 넣고 작업하세요!!");
                txtPano.Focus();
                return false;
            }
            else
            {
                return true;
            }
            
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();
            

            if (Job == "ALL")
            {
                txtPano.Text = "";
                btnSave.Enabled = false;
                lblCnt.Text = "";

            }


        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            if (read_data_check() == false)
            {
                return;
            }

            GetData(clsDB.DbCon, txtErrPano.Text.Trim(), txtErrSName.Text.Trim(),dtpFDate.Text.Trim(),dtpTDate.Text.Trim());
            
        }

        void GetData(PsmhDb pDbCon, string argPano, string argSName,string argDate1,string argDate2)
        {
            long nCnt = 0;
            string xName = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            #region XRAY_DETAIL 체크
            dt = cxraySql.sel_XRAY_DETAIL_ErrUpDate(pDbCon, argPano, argSName,argDate1,argDate2, "00"); 
               
            if (dt.Rows.Count > 0)
            { 
                nCnt += dt.Rows.Count;
            }
            
            dt.Dispose();
            dt = null;
            #endregion

            #region HIC_XRAY_RESULT 체크
            dt = cxraySql.sel_HIC_XRAY_RESULT_ErrUpDate(pDbCon, argPano, argSName, argDate1, argDate2);

            if (dt.Rows.Count > 0)
            {
                nCnt += dt.Rows.Count;
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region ENDO_JUPMST 체크
            dt = cxraySql.sel_ENDO_JUPMST_ErrUpDate(pDbCon, argPano, argSName, argDate1, argDate2);

            if (dt.Rows.Count > 0)
            {
                nCnt += dt.Rows.Count;
            }

            dt.Dispose();
            dt = null;
            #endregion
            
            if (nCnt >0)
            {
                lblCnt.Text = nCnt + "건";
                //txtPano.Text = txtErrPano.Text;                

                txtPano.Text = txtErrPano.Text;

                btnSave.Enabled = true;
                btnSave.Focus();
                btnSave.Select();
            }
            else
            {
                ComFunc.MsgBox("변경할 자료가 없습니다");
            }

            Cursor.Current = Cursors.Default;
            

        }

    }
}
