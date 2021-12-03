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
    /// File Name       : frmComSupXrayCLE02.cs
    /// Description     : 영상의학 검진 PACS오더 누락 등록
    /// Author          : 윤조연
    /// Create Date     : 2017-12-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuagfa\Frm검진오더누락등록.frm(Frm검진오더누락등록) >> frmComSupXrayCLE02.cs 폼이름 재정의" />
    public partial class frmComSupXrayCLE02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsSpread methodSpd = new clsSpread();
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXraySend cxraySend = new clsComSupXraySend();
        clsComSupXray cxray = new clsComSupXray();
                
        clsComSupXrayRead.cHic_Xray_Result cHic_Xray_Result = null;
        clsComSupXraySQL.cXray_Pacs_Order cXray_Pacs_Order = null;

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

        public frmComSupXrayCLE02()
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
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtPano.Text.Trim();
                    if (strPano != "") read_pano_info(clsDB.DbCon, strPano);

                    txtPacsNo.Select();
                }
            }
        }

        void eSave(PsmhDb pDbCon)
        {

            if (txtPano.Text.Trim() == "" || txtPacsNo.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호 및 PACSNO 넣고 조회하십시오!!");
                txtPano.Focus();
                return;
            }

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
            DataTable dt = null;

            string strUpCols = "";
            string strWheres = "";
            string strPano = "";
            long nHPano = 0;
            string strXCode = "";
            string strJepDate = "";
            //string strExamName = "";
            string strPacsNo = "";
            read_sysdate();


            //체크
            cHic_Xray_Result = new clsComSupXrayRead.cHic_Xray_Result();
            cHic_Xray_Result.Job = "01";
            cHic_Xray_Result.HPANO = 0;
            if (txtPano.Text.Trim() != "")
            {
                cHic_Xray_Result.HPANO = Convert.ToInt32(txtPano.Text.Trim());
                strPano = txtPano.Text.Trim();
            }
            cHic_Xray_Result.XRAYNO = txtPacsNo.Text.Trim();

            dt = cRead.sel_HIC_XRAY_RESULT(pDbCon, cHic_Xray_Result);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                strPano = "H" + dt.Rows[0]["Pano"].ToString().Trim();
                nHPano =Convert.ToInt32(dt.Rows[0]["Pano"].ToString().Trim());
                strXCode =  dt.Rows[0]["XCode"].ToString().Trim();
                strJepDate = dt.Rows[0]["JepDate"].ToString().Trim();
                strPacsNo = dt.Rows[0]["XrayNo"].ToString().Trim();
            }
            else
            {
                ComFunc.MsgBox(cHic_Xray_Result.XRAYNO + "가 건진 방사선 오더에 없습니다..");
                return;
            }

            cXray_Pacs_Order = new clsComSupXraySQL.cXray_Pacs_Order();
            cXray_Pacs_Order.Job = "00";
            cXray_Pacs_Order.Patid = strPano;
            cXray_Pacs_Order.HISORDERID = txtPacsNo.Text.Trim();

            dt = cxraySql.sel_XRAY_PACS_ORDER(pDbCon, cXray_Pacs_Order);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (dt.Rows[0]["ACDESSIONNO"].ToString().Trim()!="")
                {
                    ComFunc.MsgBox("이미 PACS에 오더가 존재함..");
                    return;
                }                
            }                       

            //팍스넘버 날짜(YYYYMMDD+0000)
            string strQUEUEID = clsPublic.GstrSysDate.Replace("-", "").Trim() + clsPublic.GstrSysTime.Replace(":", "").Trim() +  VB.Right(txtPacsNo.Text.Trim(),4);

            clsDB.setBeginTran(pDbCon);

            try
            {
                #region // 일반건진 접수정보를 인피니트 PACS에 전송
                    
                if (SqlErr == "")
                {
                    //
                    strUpCols = " GbORDER_SEND = 'Y' ";
                    strWheres = " AND XRayNo='" + txtPacsNo.Text.Trim() + "' ";
                    SqlErr = cRead.up_HIC_XRAY_RESULT(pDbCon, "", txtPano.Text.Trim(), strUpCols, strWheres, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                        return;
                    }
                }
                            
                //HIC_PACS_SEND
                if (SqlErr == "")
                {
                    if (cxraySend.HIC_XRAY_PACS_ADT_ORDER_INSERT(pDbCon, strJepDate, strQUEUEID, strPacsNo, strXCode, nHPano, txtPano.Text.Trim(),"PRID4") == false)
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
            txtSName.Text = "";
            DataTable dt = sup.sel_HIC_PATIENT(pDbCon, Convert.ToUInt32(argPano));
            if (ComFunc.isDataTableNull(dt) == false)
            {
                txtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                btnSave.Enabled = true;             
            }
            else
            {
                ComFunc.MsgBox("검진등록번호가 아닙니다.");
                txtPano.Select();
            }           

        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            btnSave.Enabled = false;

            if (Job == "ALL")
            {
                txtPano.Text = "";              
              
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
                 

            //GetData(clsDB.DbCon, txtPano.Text.Trim(), txtPacsNo.Text.Trim());

            //screen_clear();
                        

        }

        void GetData(PsmhDb pDbCon, string argPano, string argPacsNo)
        {

            //string xName = string.Empty;
            //DataTable dt = null;

            //Cursor.Current = Cursors.WaitCursor;


            //dt = cxraySql.sel_XRAY_DETAIL(pDbCon, "00", argPano, argPacsNo);

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
