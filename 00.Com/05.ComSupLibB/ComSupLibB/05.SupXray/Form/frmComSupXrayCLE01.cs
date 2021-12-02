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
    /// File Name       : frmComSupXrayCLE01.cs
    /// Description     : 영상의학 영상전달취소 작업(다른환자의 영상이 전달된 경우 OCS 영상 취소)
    /// Author          : 윤조연
    /// Create Date     : 2017-12-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuagfa\frmImageCancel.frm(FrmImageCancel) >> frmComSupXrayCLE01.cs 폼이름 재정의" />
    public partial class frmComSupXrayCLE01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsSpread methodSpd = new clsSpread();
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();        
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXray cxray = new clsComSupXray();

        clsComSupXraySQL.cXrayPacsSend2 cXrayPacsSend2 = null;

        long gMgrNo = 0;
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

        public frmComSupXrayCLE01()
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

            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnDelete.Click += new EventHandler(eBtnSave);            

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

                //screen_display();

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
            if (sender == this.btnDelete)
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
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtPano.Text.Trim();
                    if (strPano != "") read_pano_info(clsDB.DbCon, ComFunc.SetAutoZero(strPano, ComNum.LENPTNO));

                    txtPacsNo.Select();
                }
            }
        }

        void eSave(PsmhDb pDbCon)
        {
            string s = "PACS 관리용 Tool에서 영상을 먼저 삭제하십시오" + ComNum.VBLF;
            s += "영상취소를 하면 방사선 OCS의 재료입력이 취소되고 ," + ComNum.VBLF;
            s += "해당오더가 미접수 상태로 변경이 됩니다.." + ComNum.VBLF;
            s += "정말로 영상취소를 하시겠습니까?";
            if (ComFunc.MsgBoxQ(s, "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strUpCols = "";            
            
            read_sysdate();

            clsDB.setBeginTran(pDbCon);

            try
            {
                #region //DB 처리작업
                //재료입력을 삭제함
                SqlErr = cxraySql.del_Xray_Use(pDbCon, gMgrNo, ref intRowAffected);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }

                if (SqlErr == "")
                {
                    //PACS 전송용 Table에 INSERT(접수취소)
                    cXrayPacsSend2 = new clsComSupXraySQL.cXrayPacsSend2();
                    cXrayPacsSend2.Job ="00";
                    cXrayPacsSend2.SendGbn = "2";
                    cXrayPacsSend2.ROWID = gROWID;
                    cXrayPacsSend2.INPS = clsType.User.IdNumber;
                    SqlErr = cxraySql.ins_XRAY_PACSSEND(pDbCon, cXrayPacsSend2, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }

                if (SqlErr == "")
                {
                    //방사선 오더를 미접수 상태로 변환함
                    strUpCols = " GbReserved='1',GbEnd='',MgrNo='',PacsNo='',PacsStudyID='' ";
                    SqlErr = cxraySql.up_Xray_Detail(pDbCon,"", gROWID, "", strUpCols, "", ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
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

        void read_pano_info(PsmhDb pDbCon, string argPano)
        {

            txtPano.Text = argPano;
            txtSName.Text = fun.Read_Patient(pDbCon, argPano, "2");

        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            btnDelete.Enabled = false;

            if (Job == "ALL")
            {
                txtPano.Text = "";
                txtCnt.Text = "";
                gMgrNo = 0;
                gROWID = "";
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
            if (txtPano.Text.Trim() == "" || txtPacsNo.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호 및 PACSNO 넣고 조회하십시오!!");
                txtPano.Focus();
                return;
            }
            if (txtPacsNo.TextLength !=12)
            {
                ComFunc.MsgBox("PACS번호 12자리를 입력하세요!!");
                return;
            }

            GetData(clsDB.DbCon, txtPano.Text.Trim(),txtPacsNo.Text.Trim());

            screen_clear();

            if (txtCnt.Text !="")
            {
                btnDelete.Enabled = true;
            }            

        }

        void GetData(PsmhDb pDbCon,  string argPano,string argPacsNo)
        {
                        
            string xName = string.Empty;
            DataTable dt = null;
                        
            Cursor.Current = Cursors.WaitCursor;

            txtCnt.Text = "";
                 
            dt = cxraySql.sel_XRAY_DETAIL(pDbCon,"00", argPano,argPacsNo,"");

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                txtCnt.Text = dt.Rows.Count.ToString() + "건";
                if (dt.Rows[0]["MgrNo"].ToString().Trim()!="")
                {
                    gMgrNo =  Convert.ToInt32(dt.Rows[0]["MgrNo"].ToString().Trim());
                }                
                gROWID = dt.Rows[0]["ROWID"].ToString().Trim();

            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

    }
}
