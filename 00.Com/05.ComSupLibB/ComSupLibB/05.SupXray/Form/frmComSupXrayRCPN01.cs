using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayRCPN01.cs
    /// Description     : 영상의학과 CD복사신청 관리
    /// Author          : 윤조연
    /// Create Date     : 2018-01-09
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 두개의 폼을 통합
    /// </history>
    /// <seealso cref= "\xray\xuplist\frm방사선복사신청.frm,frm방사선복사신청2.frm(Frm방사선복사신청,Frm방사선복사신청2) >> frmComSupXrayRCPN01.cs 폼이름 재정의" />
    public partial class frmComSupXrayRCPN01 : Form,MainFormMessage
    {
        public delegate void Input_CdCopyOrder(clsComSupXraySQL.cOCS_ORDER cOCS_ORDER);
        public static event Input_CdCopyOrder InputCdCopyOrder;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead xreadSql = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery Query = new clsQuery();

        clsComSup.cBasPatient cBasPatient = null;
        clsComSupXraySQL.cXrayCdCopy cXrayCdCopy = null;
        clsComSupXray.cXray_CdCopy_MST cXray_CdCopy_MST = null;
        clsComSupXraySQL.cOCS_ORDER cOCS_ORDER = null;

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

        public frmComSupXrayRCPN01(clsComSupXray.cXray_CdCopy_MST argCls)
        {
            InitializeComponent();
            cXray_CdCopy_MST = argCls;
            setEvent();
        }

        public frmComSupXrayRCPN01(MainFormMessage pform, clsComSupXray.cXray_CdCopy_MST argCls)
        {
            InitializeComponent();

            this.mCallForm = pform;
            cXray_CdCopy_MST = argCls;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            setCombo();

            setAuth();

            txtPano.Select();

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSave);

            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);

            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtExamSabun.KeyDown += new KeyEventHandler(eTxtKeyDown);
            //this.cboDeptCode.SelectedIndexChanged += new EventHandler(eCboSelected);
            //this.cboWardCode.SelectedIndexChanged += new EventHandler(eCboSelected);


        }

        void setAuth()
        {
            // 00.권한은 처방발생 및 등록 폼에서만 하세요 기본은 01
            if (cXray_CdCopy_MST.Job =="00")
            {
                btnSave.Visible = true;
                panSave.Visible = true;
            }
            else if (cXray_CdCopy_MST.Job == "01")
            {
                btnSave.Visible = false;
                panSave.Visible = false;
            }

            DataTable dt = sup.sel_Ocs_Doctor(clsDB.DbCon, " Sabun ", "", "**", cXray_CdCopy_MST.DrCode, "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cXray_CdCopy_MST.staff = dt.Rows[0]["Sabun"].ToString().Trim();            
            }

            if (cXray_CdCopy_MST.Pano!="")
            {
                txtPano.Text = cXray_CdCopy_MST.Pano;
                
                read_pano_info(clsDB.DbCon, txtPano.Text);
                screen_display();
                screen_display2();
            }
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
                //시트
                cxraySpd.sSpd_enmXrayCDCopy01(ssList, cxraySpd.sSpdenmXrayCDCopy01, cxraySpd.nSpdenmXrayCDCopy01, 5, 0);
                cxraySpd.sSpd_enmXrayCDCopy02(ssList2, cxraySpd.sSpdenmXrayCDCopy02, cxraySpd.nSpdenmXrayCDCopy02, 5, 0);
                cxraySpd.sSpd_enmXrayCDCopy03(ssList3, cxraySpd.sSpdenmXrayCDCopy03, cxraySpd.nSpdenmXrayCDCopy03, 5, 0);
                
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                
                screen_clear();

                setCtrlData();

                //screen_display();

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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnCancel)
            {
                screen_clear("Part1");
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
                screen_display2();
            }

        }

        void eBtnSave(object sender, EventArgs e)
        {

            if (sender == this.btnSave)
            {
                if (eSave(clsDB.DbCon) == true)
                {
                    //screen_clear("Part1");
                    ssList_Sheet1.RowCount = 0;
                    ssList2_Sheet1.RowCount = 0;
                    screen_display();
                    screen_display2();
                }
            }

        }

        void eBtnPrint(object sender, EventArgs e)
        {

        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtPano.Text.Trim();
                    if (strPano != "")
                    {
                        screen_clear("Part1");

                        txtPano.Text = ComFunc.SetAutoZero(strPano, ComNum.LENPTNO);
                        strPano = txtPano.Text;
                        read_pano_info(clsDB.DbCon, strPano);
                        screen_display();
                        screen_display2();

                    }
                }
            }
            else if (sender == this.txtExamSabun)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strSabun = txtExamSabun.Text.Trim();
                    if (strSabun != "")
                    {
                        txtExamName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun);                        
                    }
                }
            }                
        }

        void eDtpTxtChange(object sender, EventArgs e)
        {
            //dtpTDate.Text = dtpFDate.Text.Trim();
        }

        void eCboSelected(object sender, EventArgs e)
        {
            //DataTable dt = null;

            //if (sender == this.cboDeptCode)
            //{
            //    dt = sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDeptCode.SelectedItem.ToString(), 2), "", false, true, true);

            //    if (ComFunc.isDataTableNull(dt) == false)
            //    {
            //        method.setCombo_View(this.cboDrCode, dt, clsParam.enmComParamComboType.NULL);
            //    }
            //}
            
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {            
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;
            
            if (e.ColumnHeader == true)
            {
                if (sender == this.ssList)
                {
                    if (e.Column == (int)clsComSupXraySpd.enmXrayCDCopy01.chk)
                    {
                        e.Cancel = true;

                        if (o.ActiveSheet.ColumnHeader.Cells[0, 0].Value.Equals("True"))
                        {
                            o.ActiveSheet.ColumnHeader.Cells[0, 0].Value = "False";
                        }

                        else
                        {
                            o.ActiveSheet.ColumnHeader.Cells[0, 0].Value = "True";
                        }
                       
                        //if (o.ActiveSheet.ColumnHeader.Cells[0, 0].Text == "True")                        
                        if (o.ActiveSheet.ColumnHeader.Cells[0, 0].Value.Equals("False"))
                        {
                            //o.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "False";
                            //methodSpd.setSpdCellChk_All(ssList, 0, false);
                            for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                            {
                                if (o.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].CellType.ToString() == "CheckBoxCellType")
                                {
                                    o.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text = "False";
                                }
                            }
                        }
                        else
                        {
                            //if(o.ActiveSheet.ColumnHeader.Cells[0, 0].Text == "True")
                            //{
                            //    o.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "False";
                            //}
                            //o.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "True";
                            for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                            {
                                if (o.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].CellType.ToString() == "CheckBoxCellType")
                                {
                                    o.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text = "True";
                                }
                            }
                            //methodSpd.setSpdCellChk_All(ssList, 0, true);
                        }
                    }
                }
                return;
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

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
            }
            else if (sender == this.ssList2)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                else
                {
                    cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
                    cXrayCdCopy.Job = "04";
                    cXrayCdCopy.Pano = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.Pano].Text.Trim();
                    cXrayCdCopy.BDate = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.BDate].Text.Trim();
                    cXrayCdCopy.SName = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.SName].Text.Trim();
                    cXrayCdCopy.WardCode = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.WardCode].Text.Trim();
                    if (o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.EntSabun].Text.Trim()!="")
                    {
                        cXrayCdCopy.EntSabun = Convert.ToInt32(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.EntSabun].Text.Trim());
                    }
                    
                    if (o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.CdGubun].Text.Trim() =="CD")
                    {
                        cXrayCdCopy.CdGubun = "1";
                    }
                    else if (o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy02.CdGubun].Text.Trim() == "DVD")
                    {
                        cXrayCdCopy.CdGubun = "2";
                    }

                    screen_display3(cXrayCdCopy);

                    for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                    {
                        o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                    }

                    o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;

                }
            }

        }

        void eSpreadButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (sender == this.ssList)
            {
                if (e.Column == (int)clsComSupXraySpd.enmXrayCDCopy01.chk)
                {
                    if (o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text == "True")
                    {
                        o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
                    }
                }
                //사이즈 체크
                lblInfo.Text = size_gesan(ssList, "1");                
            }
        }

        bool eSave(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string strPano = txtPano.Text.Trim();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            #region //값 체크
            //값 체크
            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호 공란!!");
                return false;
            }
            if (txtExamSabun.Text.Trim() == "" || txtExamName.Text.Trim() == ""  )
            {
                ComFunc.MsgBox("등록자사번 공란!!");
                return false;
            }
            if (txtQty.Text.Trim() == "")
            {
                ComFunc.MsgBox("CD-COPY할 영상을 선택후 작업하세요!!");
                return false;
            }

            //당일 복사신청 체크
            //cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
            cXrayCdCopy.Job = "06";
            cXrayCdCopy.Pano = txtPano.Text.Trim();
            cXrayCdCopy.BDate = cpublic.strSysDate;
            
            dt = cxraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                ComFunc.MsgBox("당일 CD복사 신청완료 내역이 있습니다.." + "\r\n" + "추가로 복사하실려면 영상의학과 상의하십시오");
                return false;   
            }

            //당일 오더 체크            
            cXrayCdCopy.Pano = txtPano.Text.Trim();
            cXrayCdCopy.BDate = cpublic.strSysDate;
            cXrayCdCopy.GbIO = cXray_CdCopy_MST.IO;
            cXrayCdCopy.GbER = cXray_CdCopy_MST.ER;
            cXrayCdCopy.CdGubun = "1";
            cXrayCdCopy.CdQty = 0;
            if (txtQty.Text.Trim() !="")
            {
                cXrayCdCopy.CdQty =Convert.ToInt32(txtQty.Text);
            }            
            if (optDVD.Checked == true)
            {
                cXrayCdCopy.CdGubun = "2";
            }

            string strChk = cxraySql.Read_CDCOPY_Order(pDbCon, cXrayCdCopy);

            if (strChk !="OK" && strChk != "OK2")
            {
                return false;
            }

            #endregion

            clsDB.setBeginTran(pDbCon);
                       
            try
            { 
                for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    #region //1건 체크된것 저장


                    #region  //시디복사 변수 class 초기화
                    cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
                    cXrayCdCopy.Job = "02";
                    cXrayCdCopy.GbIO = cXray_CdCopy_MST.IO;
                    cXrayCdCopy.Pano = strPano;
                    cXrayCdCopy.SName = txtSName.Text.Trim();
                    cXrayCdCopy.BDate = cpublic.strSysDate;
                    cXrayCdCopy.SeekDate = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.SeekDate].Text.Trim();
                    cXrayCdCopy.PacsNo = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.PacsNo].Text.Trim();

                    cXrayCdCopy.XJong = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XJong2].Text.Trim();
                    cXrayCdCopy.XCode = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XCode].Text.Trim();
                    cXrayCdCopy.XSubCode = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XSubCode].Text.Trim();
                    cXrayCdCopy.XName = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XName].Text.Trim();
                    cXrayCdCopy.DeptCode = cXray_CdCopy_MST.DeptCode;
                    cXrayCdCopy.DrCode = cXray_CdCopy_MST.DrCode;
                    cXrayCdCopy.WardCode = cXray_CdCopy_MST.WardCode;
                    cXrayCdCopy.RoomCode = cXray_CdCopy_MST.RoomCode;                    
                    cXrayCdCopy.CdGubun = "1";
                    if (optDVD.Checked ==true)
                    {
                        cXrayCdCopy.CdGubun = "2";
                    }
                    cXrayCdCopy.CdQty = 0;
                    if (txtQty.Text.Trim() != "")
                    {
                        cXrayCdCopy.CdQty = Convert.ToInt32(txtQty.Text);
                    }                    
                    cXrayCdCopy.EntSabun =  Convert.ToInt32(txtExamSabun.Text.Trim());                    

                    cXrayCdCopy.ROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.Copy_ROWID].Text.Trim();
                    #endregion


                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text == "True")
                    {
                        
                        //기존접수 체크
                        dt = cxraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);

                        if (ComFunc.isDataTableNull(dt) == false)
                        {
                            cXrayCdCopy.Job = "03";
                            SqlErr = cxraySql.up_Xray_CdCopy(pDbCon, cXrayCdCopy, ref intRowAffected);
                        }
                        else
                        {
                            SqlErr = cxraySql.ins_Xray_CdCopy(pDbCon, cXrayCdCopy, ref intRowAffected);
                        }


                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                    }
                    else
                    {
                        if (cXrayCdCopy.ROWID !="")
                        {
                            cXrayCdCopy.Job = "04";
                            cXrayCdCopy.EntSabun = Convert.ToInt32(clsType.User.Sabun); //삭제는 로그인한 사번
                            SqlErr = cxraySql.up_Xray_CdCopy(pDbCon, cXrayCdCopy, ref intRowAffected);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                    }

                    #endregion
                }

                #region //복사신청 오더생성 - TODO 오더화면에서 발생하도록
                //오더생성
                if (SqlErr == "" && strChk =="OK")
                {                    
                    cOCS_ORDER = new clsComSupXraySQL.cOCS_ORDER();
                    cOCS_ORDER.GbIO = cXrayCdCopy.GbIO;                    
                    cOCS_ORDER.PTNO = cXrayCdCopy.Pano;
                    cOCS_ORDER.BDATE = cXrayCdCopy.BDate;
                    cOCS_ORDER.DEPTCODE = cXray_CdCopy_MST.DeptCode;
                    cOCS_ORDER.DRCODE = cXray_CdCopy_MST.DrCode;
                    cOCS_ORDER.SEQNO = "99";
                    if (cXrayCdCopy.CdGubun == "1")
                    {
                        cOCS_ORDER.ORDERCODE = "00604635";
                        cOCS_ORDER.SUCODE = "XCDC";
                    }
                    else if (cXrayCdCopy.CdGubun == "2")
                    {
                        cOCS_ORDER.ORDERCODE = "00604637";
                        cOCS_ORDER.SUCODE = "XDVDC";
                    }
                    
                    cOCS_ORDER.BUN = "65";
                    cOCS_ORDER.SLIPNO = "0060";
                    cOCS_ORDER.RealQTY = cXrayCdCopy.CdQty.ToString();
                    cOCS_ORDER.QTY = cXrayCdCopy.CdQty;
                    cOCS_ORDER.RealNal = 1;
                    cOCS_ORDER.NAL = 1;
                    cOCS_ORDER.GBDIV = "1";
                    cOCS_ORDER.DOSCODE = "";
                    cOCS_ORDER.GBBOTH = "0";
                    cOCS_ORDER.GBINFO = "";
                    cOCS_ORDER.GBER = "";
                    cOCS_ORDER.GBSELF = "2";
                    cOCS_ORDER.GBSPC = "";
                    cOCS_ORDER.BI = cXray_CdCopy_MST.Bi;
                    cOCS_ORDER.REMARK = "xauto";                    
                    cOCS_ORDER.GBSUNAP = "0";
                    cOCS_ORDER.TUYAKNO = "0";
                    cOCS_ORDER.ORDERNO =Convert.ToInt32(ComQuery.GetSequencesNo(pDbCon, "KOSMOS_OCS", "SEQ_ORDERNO")); //오더넘버생성
                    cOCS_ORDER.MULTI = "";
                    cOCS_ORDER.MULTIREMARK = "";
                    cOCS_ORDER.DUR = "";
                    cOCS_ORDER.RESV = "";
                    cOCS_ORDER.SCODESAYU = "";
                    cOCS_ORDER.SCODEREMARK = "";
                    cOCS_ORDER.GBSEND = "";
                    if (cOCS_ORDER.GbIO =="I")
                    {
                        cOCS_ORDER.GBSEND = "*";

                        if (cXray_CdCopy_MST.staff == "")
                        {
                            clsPublic.GstrCDSendChk = ""; //CD오더생성 체크
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox("STAFF ID 가 누락되었습니다." + ComNum.VBLF + "저장할 수 없습니다.");
                            clsDB.SaveSqlErrLog("STAFF ID 가 누락되었습니다." + ComNum.VBLF + "저장할 수 없습니다.", SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return false;
                        }
                    }

                    cOCS_ORDER.Sabun = cXray_CdCopy_MST.staff;
                    cOCS_ORDER.StaffID = cXray_CdCopy_MST.staff;

                    cOCS_ORDER.WardCode = cXray_CdCopy_MST.WardCode;
                    cOCS_ORDER.RoomCode = cXray_CdCopy_MST.RoomCode;
                    cOCS_ORDER.GbStatus = " ";
                    cOCS_ORDER.GbPRN = " ";

                    // 전산업무의뢰서 2020-1667 처방전달
                    //InputCdCopyOrder(cOCS_ORDER);

                    SqlErr = cxraySql.ins_OCS_ORDER(pDbCon, cOCS_ORDER, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        clsPublic.GstrCDSendChk = ""; //CD오더생성 체크
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                }
                #endregion

                clsPublic.GstrCDSendChk = "OK"; //CD오더생성 체크
                clsDB.setCommitTran(pDbCon);
                return true;
            }
            catch (Exception ex)
            {
                clsPublic.GstrCDSendChk = ""; //CD오더생성 체크
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }                

        void setCombo()
        {
            //과
            //method.setCombo_View(this.cboDeptCode, comSql.sel_BAS_CLINICDEPT_COMBO(clsDB.DbCon), clsParam.enmComParamComboType.NULL);

            

        }

        void read_pano_info(PsmhDb pDbCon, string argPano)
        {            
            txtPano.Text = argPano;
            cBasPatient = sup.sel_Bas_Patient_cls(pDbCon, argPano);
            txtSName.Text = cBasPatient.SName;
            txtAge.Text = cBasPatient.Age + "/" + cBasPatient.Sex;
            
        }               

        string size_gesan(FarPoint.Win.Spread.FpSpread Spd, string job = "") //optional 기능
        {
            string s = "";
            int nCDQty = 0;
            double nCdTemp1 = 0;
            double nCdTemp2 = 0;
            double nDbFile = 0;

            long nSizeSum = 0;
            
            for (int i = 0; i < Spd.ActiveSheet.RowCount; i++)
            {
                if (Spd.ActiveSheet.Cells[i,(int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text == "True")
                {
                    nDbFile += (Convert.ToDouble(Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.Size].Text) * 0.000001);                    
                }
            }

            nDbFile += 50; // DB사이즈에 50메가 추가함

            nSizeSum =  Convert.ToInt32(Math.Round(nDbFile));

            if (nSizeSum > 0)
            {
                nCdTemp1 = nSizeSum / 600;
                nCdTemp2 = ( nSizeSum % 600);
            }

            if (nCdTemp1 < 1)
            {
                nCDQty = 1;
            }
            else
            {
                nCDQty = Convert.ToInt32(nCdTemp1);
                if (nCdTemp2 >0 )
                {
                    nCDQty += 1;
                }                
            }

            if (optDVD.Checked == true)
            {
                nCDQty = 1;
            }
            else
            {
                if (nSizeSum > 600)
                {
                    if(job == "1")
                    {
                        if (ComFunc.MsgBoxQ("영상 사이즈가 600M가 이상입니다.. DVD신청으로 변경하시겠습니까??", "CD,DVD선택", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            nCDQty = 1;
                            optDVD.Checked = true;
                        }
                    }
                    
                }
            }

            txtQty.Text = nCDQty.ToString();

            if (optDVD.Checked == true)
            {
                s = "영상크기:" + nSizeSum.ToString() + "MB" + "\r\n" + "DVD:" + nCDQty.ToString() + " 장 필요함";
            }
            else
            {
                s = "영상크기:" + nSizeSum.ToString() + "MB" + "\r\n" + "CD:" + nCDQty.ToString() + " 장 필요함";
            }

            return s;
        }

        void clear_Sheet()
        {
            for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
            {
                ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text = "";
                ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].BackColor = Color.White;
            }

        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            clear_Sheet();

            optCD.Checked = true;
            optDVD.Checked = false;

            lblInfo.Text = "";

            ssList3.ActiveSheet.RowCount = 0;

            if (Job == "ALL")
            {
                Control[] controls = ComFunc.GetAllControls(this);

                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        ((TextBox)ctl).Text = "";

                    }
                    else if (ctl is ComboBox)
                    {
                        ((ComboBox)ctl).Text = "";

                    }
                }
            }
            else if (Job == "Part1")
            {
                ssList2.ActiveSheet.RowCount = 0;

                //screen_display();

                Control[] controls = ComFunc.GetAllControls(this);

                foreach (Control ctl in controls)
                {
                    if (ctl is TextBox)
                    {
                        //if (((TextBox)ctl).Name !="txtPano" && ((TextBox)ctl).Name != "txtSName" && ((TextBox)ctl).Name != "txtAge")
                        //{
                        ((TextBox)ctl).Text = "";
                        //}


                    }
                    else if (ctl is ComboBox)
                    {
                        ((ComboBox)ctl).Text = "";

                    }
                }

            }

            txtPano.Select();
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList,txtPano.Text.Trim());
        }

        void screen_display2()
        {
            GetData2(clsDB.DbCon, ssList2, txtPano.Text.Trim());
        }

        void screen_display3(clsComSupXraySQL.cXrayCdCopy argCls)
        {
            GetData3(clsDB.DbCon, ssList3, argCls);
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argPano)
        {
            if (argPano == "")
            {
                return;
            }

            int i = 0;
            string strXName = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            read_sysdate();

            screen_clear();

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
            cXrayCdCopy.Job = "02";
            cXrayCdCopy.Pano = argPano;
            cXrayCdCopy.BDate = cpublic.strSysDate;

            dt = cxraySql.sel_XRAY_DETAIL_ENDO(pDbCon, argPano);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strXName = dt.Rows[i]["OrderName"].ToString().Trim();
                    if (strXName == "")
                    {
                        strXName = dt.Rows[i]["XName"].ToString().Trim();
                    }
                    strXName += " " + dt.Rows[i]["Remark"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text = "";

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.SeekDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XJong].Text = dt.Rows[i]["FC_XJong2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XJong2].Text = dt.Rows[i]["XJong"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.ReadNo].Text = dt.Rows[i]["ExInfo"].ToString().Trim();
                    if (dt.Rows[i]["ExInfo"].ToString().Trim() != "")
                    {
                        if (Convert.ToInt32(dt.Rows[i]["ExInfo"].ToString().Trim()) > 1000)
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.STS01].Text = "◎";
                        }
                    }

                    sup.setColStyle_Text(Spd, i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk, false, false, true); //txt재정의            
                    if (dt.Rows[i]["PacsStudyID"].ToString().Trim() != "")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.STS02].Text = "▦";
                        methodSpd.setColStyle(Spd, i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk, clsSpread.enmSpdType.CheckBox);                        
                    }
                    

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XName].Text = strXName;

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.PacsNo].Text = dt.Rows[i]["PacsNo"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.ReadNo].Text = dt.Rows[i]["ExInfo"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.PacsUid].Text = dt.Rows[i]["PacsStudyID"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.OrderDate].Text = dt.Rows[i]["OrderDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.JepTime].Text = dt.Rows[i]["JepsuTime"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XSendDate].Text = dt.Rows[i]["XSendDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.XSubCode].Text = dt.Rows[i]["XSubCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.Size].Value = cxraySql.Read_Pacs_Size(pDbCon, argPano, dt.Rows[i]["PacsNo"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    //xray_cdcopy rowid
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.Copy_ROWID].Text = "";
                    cXrayCdCopy.PacsNo = dt.Rows[i]["PacsNo"].ToString().Trim();
                    dt2 = cxraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);
                    if (ComFunc.isDataTableNull(dt2) == false)
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.chk].Text = "1";
                        Spd.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.LightGreen;
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy01.Copy_ROWID].Text = dt2.Rows[0]["ROWID"].ToString().Trim();
                        
                    }

                }
                                
                //사이즈 체크
                lblInfo.Text = size_gesan(ssList);

                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            #endregion

            #region //당일복사 신청정보
            //당일복사 신청정보
            cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
            cXrayCdCopy.Job = "05";
            cXrayCdCopy.Pano = argPano;
            cXrayCdCopy.BDate = cpublic.strSysDate;

            dt = cxraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);

            lblSts.Text = "당일 복사신청 내역없음!!";

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                string s = "";

                if (dt.Rows[0]["CdGubun"].ToString().Trim() == "1")
                {
                    optCD.Checked = true;
                    s += dt.Rows[0]["BDate"].ToString().Trim() + " 등록번호 : " + argPano + " 성명 : " + dt.Rows[0]["SName"].ToString().Trim();
                    s += " 영상갯수 : " + dt.Rows[0]["CNT"].ToString().Trim() + "   시디[CD](" + dt.Rows[0]["CdQty"].ToString().Trim() + ") 장 등록됨";

                }
                else if (dt.Rows[0]["CdGubun"].ToString().Trim() == "2")
                {
                    optDVD.Checked = true;

                    s += dt.Rows[0]["BDate"].ToString().Trim() + " 등록번호 : " + argPano + " 성명 : " + dt.Rows[0]["SName"].ToString().Trim();
                    s += " 영상갯수 : " + dt.Rows[0]["CNT"].ToString().Trim() + "   시디[DVD](" + dt.Rows[0]["CdQty"].ToString().Trim() + ") 장 등록됨";
                }

                lblSts.Text = "당일 복사신청 내역 : " + s;

            }

            dt.Dispose();
            dt = null;

            #endregion
        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argPano)
        {
            int i = 0;
            DataTable dt = null;

            read_sysdate();

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
            cXrayCdCopy.Job = "03";
            cXrayCdCopy.Pano = argPano;
            cXrayCdCopy.BDate = cpublic.strSysDate;

            dt = cxraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.CdQty].Text = dt.Rows[i]["CdQty"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.Qty].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.WardCode].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.EntSabun].Text = dt.Rows[i]["EntSabun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.CdMake].Text = dt.Rows[i]["CdMake"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.CdMakeTime].Text = dt.Rows[i]["CopyTime"].ToString().Trim();
                    if (dt.Rows[i]["CdGubun"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.CdGubun].Text = "CD";
                    }
                    else if (dt.Rows[i]["CdGubun"].ToString().Trim() == "2")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy02.CdGubun].Text = "DVD";
                    }
                                       
                }

                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            #endregion

        }

        void GetData3(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, clsComSupXraySQL.cXrayCdCopy argCls)
        {
            int i = 0;
            DataTable dt = null;

            read_sysdate();

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;                       

            dt = cxraySql.sel_XRAY_CDCOPY(pDbCon, argCls);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.SeekDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.XJong].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.XCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.XName].Text = dt.Rows[i]["XName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCDCopy03.EndTime].Text = dt.Rows[i]["EntTime"].ToString().Trim();

                }

                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            #endregion

        }

    }
}
