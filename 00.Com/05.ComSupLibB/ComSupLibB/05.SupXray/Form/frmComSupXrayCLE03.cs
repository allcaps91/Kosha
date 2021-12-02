using ComBase;
using ComDbB;
using ComSupLibB.Com;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayCLE03.cs
    /// Description     : 영상의학 이중차트 등록번호 변경작업
    /// Author          : 윤조연
    /// Create Date     : 2017-12-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuagfa\Xuagfa10.frm(FrmChartNoRep) >> frmComSupXrayCLE03.cs 폼이름 재정의" />
    public partial class frmComSupXrayCLE03 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsSpread methodSpd = new clsSpread();
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();

        //clsComSupXraySQL.cXrayPacsSend2 cXrayPacsSend2 = null;
        //clsComSupXraySQL.cHic_Xray_Result cHic_Xray_Result = null;
        //clsComSupXraySQL.cXray_Pacs_Order cXray_Pacs_Order = null;

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

        public frmComSupXrayCLE03()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {

        }

        void setCtrlInit()
        {

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            //this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);

            this.txtOldPano.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtNewPano.KeyDown += new KeyEventHandler(eTxtKeyDown);

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
            //if (sender == this.btnSearch)
            //{
            //    screen_display();
            //}
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon);
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
            if (sender == this.txtOldPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtOldPano.Text.Trim();
                    if (strPano != "") read_pano_info(clsDB.DbCon,"OLD", strPano);

                    
                }
            }
            else if (sender == this.txtNewPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtNewPano.Text.Trim();
                    if (strPano != "") read_pano_info(clsDB.DbCon, "NEW", strPano);


                }
            }
        }

        void eSave(PsmhDb pDbCon)
        {
            if (txtOldPano.Text.Trim() == "" || txtNewPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호를 넣고 조회하십시오!!");
                txtNewPano.Focus();
                return;
            }

            

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;
            string strUpCols = "";            
            int nRead1 = 0;
            int nRead2 = 0;
                       
            read_sysdate();

            //체크
            dt = cxraySql.sel_XRAY_DETAIL(pDbCon, "01", txtOldPano.Text.Trim(), "","");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                nRead1 = dt.Rows.Count;
            }

            dt = cRead.sel_XRAY_RESULTNEW(pDbCon, "00", txtOldPano.Text.Trim(), "", 0);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                nRead2 = dt.Rows.Count;
            }

            if (nRead1 == 0 && nRead2 == 0)
            {
                ComFunc.MsgBox("변경 할 자료가 존재하지 않습니다..");
                return;
            }
            else
            {
                string s = "XRAY_DETAIL에 " + nRead1 + " 개, XRAY_RESULTNEW에 " + nRead2 + " 개 변경 할 자료가 있습니다." + ComNum.VBLF;
                s += "자료를 변경 하시겠습니까?";
                if (ComFunc.MsgBoxQ(s, "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                #region // 일반건진 접수정보를 인피니트 PACS에 전송
                
                if (SqlErr == "")
                {
                    strUpCols = " Pano = '" + txtNewPano.Text.Trim() + "' ";                    
                    SqlErr = cxraySql.up_Xray_Detail(pDbCon,"", "", txtOldPano.Text.Trim(), strUpCols, "", ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }

                    if (SqlErr =="")
                    {
                        strUpCols = " Pano = '" + txtNewPano.Text.Trim() + "' ";
                        SqlErr = cRead.up_XRAY_RESULTNEW(pDbCon, "", txtOldPano.Text.Trim(), strUpCols, "", ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                    }

                }
                               
                #endregion

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("작업완료");
                    screen_display();
                }

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

        void read_pano_info(PsmhDb pDbCon, string argJob , string argPano)
        {
            if (argJob =="OLD")
            {
                txtOldPano.Text = argPano;
                txtOldSName.Text = fun.Read_Patient(pDbCon, argPano, "2");
            }   
            else if (argJob == "NEW")
            {
                txtNewPano.Text = argPano;
                txtNewSName.Text = fun.Read_Patient(pDbCon, argPano, "2");
            }


        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            btnSave.Enabled = false;

            if (Job == "ALL")
            {
                txtNewPano.Text = "";

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


            //GetData(clsDB.DbCon, txtNewPano.Text.Trim(), txtPacsNo.Text.Trim());

            //screen_clear();


        }

        void GetData(PsmhDb pDbCon, string argPano, string argPacsNo)
        {

            //string xName = string.Empty;
            //DataTable dt = null;

            //Cursor.Current = Cursors.WaitCursor;


            //dt = cxraySql.sel_XRAY_DETAIL(pDbCon,"00", argPano, argPacsNo);

            //#region //데이터셋 읽어 자료 표시
            //if (dt == null) return;

            //if (dt.Rows.Count > 0)
            //{

            //    if (dt.Rows[0]["MgrNo"].ToString().Trim() != "")
            //    {
            //        //gMgrNo = Convert.ToInt32(dt.Rows[0]["MgrNo"].ToString().Trim());
            //    }
            //    //gROWID = dt.Rows[0]["ROWID"].ToString().Trim();

            //}

            //dt.Dispose();
            //dt = null;
            //Cursor.Current = Cursors.Default;

            //#endregion


        }
    }
}
