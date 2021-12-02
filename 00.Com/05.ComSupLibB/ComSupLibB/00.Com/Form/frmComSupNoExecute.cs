using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.SupXray;
//using ComNurLibB;
using System.Threading;
using FarPoint.Win.Spread;
using System.Drawing;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupNoExecute.cs
    /// Description     : 미시행 검사 조회 및 처리 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-06-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmNoExecure.frm(FrmNoExecute) 폼 frmComSupNoExecute.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\FrmNoExecute.frm >> frmComSupNoExecute.cs 폼이름 재정의" />
    public partial class frmComSupNoExecute : Form
    {
        #region 클래스 선언 및 etc....

        string SQL = string.Empty;
        clsPublic cpublic = new clsPublic();
        clsComSupSpd csupspd = new clsComSupSpd();
        ComFunc fun = new ComFunc();
        clsQuery Query = new clsQuery();
        clsSpread cSpd = new clsSpread();
        clsComSup csup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsComSup sup = new clsComSup();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();

        clsComSup.cEtc_NoExe_Remark cEtc_NoExe_Remark = null;


        //단축메뉴 배열
        string[] eMnu = null;
        string[,] eMnu_Sub = null;

        string[] argsql = null; //쿼리에 사용될 변수 배열값

        string gstrER = string.Empty;
        string gstrPano = string.Empty;
        string gstrFDate = string.Empty;
        string gstrTDate = string.Empty;
        string gstrDept = string.Empty;

        clsComSupSpd.sPatInfo pInfo = new clsComSupSpd.sPatInfo(); //시트선택시 환자정보

        int nRow = 0;
        Thread thread;
        FpSpread spd;


        enum enmsql { GbIO, Pano, FDate, TDate, DeptCode, DrCode, ChkExam1, ChkExam2, ChkExam3, ChkExam31, ChkExam4, ChkExam5, ChkExam6, ChkExam7, ChkExam8, ChkExam9, ChkExam10, ChkExam11, ChkGum, OptGb1, OptGb2, XJong, ChkER };

        #endregion

        public frmComSupNoExecute()
        {
            InitializeComponent();
            setEvent();
        }

        void setMnu_array(PsmhDb pDbCon)
        {
            DataTable dt = null;
            DataTable dt2 = null;
            int nMaxSub = 0;
            string menuName = "";
            
            dt = sup.sel_Menu_MaxSub(pDbCon, "C#_Menu_NoExeCute_Sub");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                nMaxSub = Convert.ToInt16(dt.Rows[0]["CNT"].ToString());
            }

            dt = Query.Get_BasBcode(pDbCon, "C#_Menu_NoExeCute", "", " Code || '.' || Name CodeName, Code ,Name   ", "", " Sort ASC, Code ASC ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                eMnu = new string[dt.Rows.Count];
                eMnu_Sub = new string[dt.Rows.Count, nMaxSub];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    menuName = dt.Rows[i]["CodeName"].ToString().Trim();

                    eMnu[i] = clsComSup.setP(menuName, ".", 2);

                    dt2 = sup.sel_Menu_Sub(pDbCon, "C#_Menu_NoExeCute_Sub", VB.Mid(menuName, 1, 5), " Code || '.' || Name CodeName, Code,Name   ", "Sort ASC, Code ASC ", 1, 5);
                    if (ComFunc.isDataTableNull(dt2) == false)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            eMnu_Sub[i, j] = dt2.Rows[j]["CodeName"].ToString().Trim();
                        }
                    }
                    else
                    {
                        eMnu_Sub[i, 0] = null;
                    }
                }

            }
            else
            {
                eMnu = null;
            }
        }

        public frmComSupNoExecute(string strER, string strPano ="")
        {
            InitializeComponent();


            gstrPano = strPano;
            gstrER = strER; //응급실에서 사용한다면 strER ="ER"

            setEvent();

        }

        public frmComSupNoExecute(string strER, string strPano ,string argFDate ,string argTDate)
        {
            InitializeComponent();

            gstrPano = strPano;
            gstrER = strER; //응급실에서 사용한다면 strER ="ER"
            gstrFDate = argFDate;
            gstrTDate = argTDate;

            setEvent(); 

        }

        void eFormLoad(object sender, EventArgs e)
        {
            //
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //시트자동 세팅
                csupspd.sSpd_NoExecuteMain(ssList, csupspd.sSpdNoExecutoMain, csupspd.nSpdNoExecutoMain, 1, 0);

                //            
                csupspd.sSpd_NoExecuteResv(ssResv, csupspd.sSpdNoExecuteResv, csupspd.nSpdNoExecuteResv, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                
                //
                screen_clear();

                //
                setCtrlData(clsDB.DbCon);

                if (gstrER == "ER")
                {                    
                    btnSearch.PerformClick();
                }

                //2019-10-22 안정수, 인증기간동안 emr 버튼 안보이도록 추가
                if(String.Compare(cpublic.strSysDate, "2019-10-26") < 0)
                {
                    btnEmr.Visible = false;
                }

                btnGlucose.Visible = false;


            }

        }

        void setCtrlData(PsmhDb pDbCon)
        {

            //
            setCombo();

            txtPano.Text = gstrPano;
            lblSName.Text = read_PanoInfo(pDbCon, txtPano.Text);
            read_sysdate();

            if (gstrFDate !="" && gstrTDate !="")
            {
                dtpFDate.Text = gstrFDate;
                dtpTDate.Text = gstrTDate;
            }
            else
            {
                dtpFDate.Text = cpublic.strSysDate;
                dtpTDate.Text = cpublic.strSysDate;
            }
            
            dtpRDate.Text = VB.DateAdd("d", 1, cpublic.strSysDate).ToShortDateString();

            if (gstrER == "ER")
            {
                //gstrPano = "";
                //txtPano.Text = "";
                dtpFDate.Text =  VB.DateAdd("d", -1, cpublic.strSysDate).ToShortDateString() ;
                chkEr.Visible = true;
                chkEr.Checked = false;
                panER.Location = new System.Drawing.Point(6, 14);
                panER.Visible = true;
            }

            setMnu_array(clsDB.DbCon);

        }

        void screen_clear()
        {
            txtPano.Text = "";
            lblSName.Text = "";
            chkEr.Visible = false;
            chkEr.Checked = false;
            panER.Location = new System.Drawing.Point(140, 14);
            panER.Visible = false;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnSearch1.Click += new EventHandler(eBtnSearch);

            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.btnPrint2.Click += new EventHandler(eBtnPrint);

            this.btnGlucose.Click += new EventHandler(eBtnPopUp);
            this.btnEmr.Click += new EventHandler(eBtnPopUp);
            this.btnMemo.Click += new EventHandler(eBtnPopUp);
            this.btnCsinfo.Click += new EventHandler(eBtnPopUp);

            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnSave1.Click += new EventHandler(eBtnSave);
            this.btnSave2.Click += new EventHandler(eBtnSave);
            

            this.ssList.CellClick +=  new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClicked);

            this.cboDept.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            this.txtPano.LostFocus += new EventHandler(eTxtLostFous);
            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.chkAll.Click += new EventHandler(eChkClick);


        }
        
        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                //자료조회
                screen_display();
            }
            else if (sender == this.btnSearch1)
            {
                //기타결과  
                clsPublic.GstrHelpCode = "";//add                              
                frmViewResult f = new frmViewResult(pInfo.strPano);
                f.ShowDialog();
                sup.setClearMemory(f);
            }

        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                //예약변경
                eSave(clsDB.DbCon, "예약변경");
            }
            else if (sender == this.btnSave1)
            {
                //2019-01-31 의뢰서 135번 진행(김경동)
                //완료처리
                if (ComFunc.MsgBoxQ("검사완료처리를 정말로 진행하시겠습니까. ", "완료처리", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    eSave(clsDB.DbCon, "완료처리");
                }

            }
            else if (sender == this.btnSave2)
            {
                //수동취소
                eSave(clsDB.DbCon, "수동취소");
            }
            
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //자료출력
                ePrint("인쇄");
            }
            else if (sender == this.btnPrint2)
            {
                //자료출력
                ePrint("선택인쇄");
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {
            if (sender == this.btnGlucose)
            {
                //Glucose 검사결과입력
                if (pInfo.strROWID != "" && (pInfo.strExCode == "CR59B" || pInfo.strExCode == "CR59"))
                {
                    #region //Glucose 검사결과입력
                    string strSabun = string.Empty;
                    if (cboDoct.Text == "****.전체")
                    {
                        strSabun = VB.InputBox("사번입력", "Glucose 검사결과입력", clsType.User.IdNumber);
                    }
                    else if (cboDoct.Text != "")
                    {
                        strSabun = clsVbfunc.GetOCSDrCodeSabun(clsDB.DbCon, clsComSup.setP(cboDoct.Text.Trim(),".",1));
                    }
                    else
                    {
                        strSabun = VB.InputBox("사번입력", "Glucose 검사결과입력", clsType.User.IdNumber);
                    }                    
                    
                    if (strSabun != "" && strSabun != "00000")
                    {
                        frmComSupEXRSLT01 f = new frmComSupEXRSLT01(pInfo.strROWID, strSabun);
                        f.ShowDialog();
                        sup.setClearMemory(f);                    
                    }
                    else
                    {
                        ComFunc.MsgBox("의사사번을 체크하십시오!!");
                    }
                    #endregion
                }
                else
                {
                    ComFunc.MsgBox("검사코드 [CR59B , CR59] 가 아닙니다...확인후 작업하세요!! ");
                }
            }
            else if (sender == this.btnEmr)
            {
                //EMR 뷰어                
                clsVbEmr.EXECUTE_TextEmrView(pInfo.strPano, clsType.User.IdNumber);
            }
            else if (sender == this.btnMemo)
            {
                //환자메모
                if (pInfo.strPano != "" && pInfo.strDept != "")
                {
                    frmMemo f = new frmMemo(pInfo.strPano, pInfo.strDept,pInfo.strDrCode);
                    f.ShowDialog();
                    sup.setClearMemory(f);
                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요!! ");
                }

            }
            else if (sender == this.btnCsinfo)
            {
                //고객정보
                frmViewCsinfo f = new frmViewCsinfo(pInfo.strPano);
                f.ShowDialog();
                sup.setClearMemory(f);
            }
        }

        void eSave(PsmhDb pDbCon, string str)
        {
            int i = 0;

            string strRDate = dtpRDate.Text.Trim();
            string strFlag = "";
            string strTable = "";
            string strROWID = "";
            string SqlErr = ""; //에러문 받는 변수
            string strChk = ""; //업데이트전 검사상태체크(ex.예약, 접수, 완료)

            int intRowAffected = 0; //변경된 Row 받는 변수


            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (str == "완료처리" || str == "수동취소" || str == "예약변경")
                {
                    if (strRDate == "" && str == "완료처리")
                    {
                        MessageBox.Show("예약일자를 선정해주세요");
                        return;
                    }

                    for (i = 0; i < ssList.ActiveSheet.RowCount; i++)
                    {
                        strFlag = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteMain.check01].Text.Trim();
                        strTable = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteMain.Table].Text.Trim();
                        strROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Text.Trim();



                        if (strFlag == "True")
                        {
                            //2019-02-13(완료 처리된 기능검사는 변경불가능)
                            if (strTable == "ETC_JUPMST" && gstrER != "ER")
                            //if (strTable == "ETC_JUPMST")
                            { 
                                strChk = CHK_NoExecute_STS(pDbCon, str, strTable, strRDate, strROWID, ref intRowAffected); 
                            }

                            if (strChk == "OK")
                            {
                                MessageBox.Show("검사진행 또는 완료 상태이므로 " + str +" 불가능 합니다.");
                                clsDB.setRollbackTran(pDbCon);
                                return;
                            }

                            if (strTable == "EXAM_ORDER")
                            {
                                SqlErr = up_NoExecute(pDbCon,str, strTable, strRDate, strROWID,  ref intRowAffected);
                            }
                            else if (strTable == "ENDO_JUPMST")
                            {
                                SqlErr = up_NoExecute(pDbCon, str, strTable, strRDate, strROWID,  ref intRowAffected);
                            }
                            else if (strTable == "ETC_JUPMST")
                            {
                                SqlErr = up_NoExecute(pDbCon, str, strTable, strRDate, strROWID,  ref intRowAffected);
                            }
                            else if (strTable == "XRAY_DETAIL")
                            {
                                SqlErr = up_NoExecute(pDbCon, str, strTable, strRDate, strROWID,  ref intRowAffected);
                            }

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                                
                                return;
                            }
                        }

                    }
                }
                if (SqlErr=="")
                {
                    clsDB.setCommitTran(pDbCon);
                }                

                //자료조회
                GetData(pDbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim(), "O");

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
            
            
        }

        void eSpreadClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }


            read_cinfo(ssList, e.Row);


            //마우스 우클릭 메뉴팝업
            if (e.Button == MouseButtons.Right)
            {
                #region //우클릭시 팝업메뉴 생성

                string sMCode = "";
                string sName = "";
                
                if (o.ActiveSheet.RowCount <= 0)
                {
                    return;
                }

                string strPano = o.ActiveSheet.Cells[o.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmNoExecuteMain.Pano].Text.Trim();
                string strSName = o.ActiveSheet.Cells[o.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmNoExecuteMain.SName].Text.Trim();


                //
                if (sender == this.ssList)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ContextMenuStrip menu = new ContextMenuStrip();
                        ToolStripMenuItem[] item = new ToolStripMenuItem[eMnu.Length];
                        ToolStripMenuItem[] submenu = new ToolStripMenuItem[eMnu.Length];

                        for ( i = 0; i < eMnu.Length; i++)
                        {
                            if (i == 0)
                            {
                                submenu[i] = new ToolStripMenuItem(eMnu[i] + "  [ " + strPano + " " + strSName + " ] ");
                            }
                            else
                            {
                                submenu[i] = new ToolStripMenuItem(eMnu[i]);
                                submenu[i].Click += new EventHandler(eMnuSubClick);
                            }

                            for (int j = 0; j < eMnu_Sub.GetLength(1); j++)
                            {
                                if (!string.IsNullOrEmpty(eMnu_Sub[i, j]))
                                {
                                    sMCode = clsComSup.setP(eMnu_Sub[i, j], ".", 1);
                                    sName = eMnu_Sub[i, j].Replace(sMCode + ".", "").Trim();
                                    item[i] = new ToolStripMenuItem(sName, null, eMnuSubClick, sMCode);

                                    submenu[i].DropDownItems.Add(item[i]);
                                }
                                
                            }

                            menu.Items.Add(submenu[i]);

                        }


                        //menu.Show((Control)(sender), s.Location);
                        Point p = new Point();
                        p = new Point(Cursor.Position.X, Cursor.Position.Y);

                        menu.Show(p);

                    }
                    

                }

                #endregion
            }
                        
            //pInfo.strPano = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.Pano].Text.Trim();
            //pInfo.strExCode = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Text.Trim();
            //pInfo.strROWID = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Text.Trim();

            ReadResvInfo(clsDB.DbCon, ssResv, pInfo.strPano);


        }

        void eMnuSubClick(object sender, EventArgs e)
        {          
            string mnuParent = "";
            
            ToolStripMenuItem obj = (ToolStripMenuItem)sender;
            string mnuName = obj.Text.Trim();

            if (obj.OwnerItem != null)
            {
                mnuParent = obj.OwnerItem.ToString().Trim();
            }
            else
            {
                mnuParent = mnuName;
            }

            if (mnuName == "Glucose")
            {
                //Glucose 검사결과입력         
                #region //Glucose 검사결과입력
                if (pInfo.strROWID != "" && (pInfo.strExCode == "CR59B" || pInfo.strExCode == "CR59"))
                {
                    string strSabun = string.Empty;
                    if (cboDoct.Text == "****.전체")
                    {
                        strSabun = VB.InputBox("사번입력", "Glucose 검사결과입력", clsType.User.IdNumber);
                    }
                    else if (cboDoct.Text != "")
                    {
                        strSabun = clsVbfunc.GetOCSDrCodeSabun(clsDB.DbCon, clsComSup.setP(cboDoct.Text.Trim(), ".", 1));
                    }
                    else
                    {
                        strSabun = VB.InputBox("사번입력", "Glucose 검사결과입력", clsType.User.IdNumber);
                    }

                    if (strSabun != "" && strSabun != "00000")
                    {
                        frmComSupEXRSLT01 f = new frmComSupEXRSLT01(pInfo.strROWID, strSabun);
                        f.ShowDialog();
                        sup.setClearMemory(f);
                    }
                    else
                    {
                        ComFunc.MsgBox("의사사번을 체크하십시오!!");
                    }
                }
                else
                {
                    ComFunc.MsgBox("검사코드 [CR59B , CR59] 가 아닙니다...확인후 작업하세요!! ");
                }
                #endregion
            }
            else if (mnuName == "참고사항")
            {
                //환자메모
                if (pInfo.strPano != "" && pInfo.strDept != "")
                {
                    frmMemo f = new frmMemo(pInfo.strPano, pInfo.strDept,pInfo.strDrCode);
                    f.ShowDialog();
                    sup.setClearMemory(f);

                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요!! ");
                }
            }

        }

        void eSpreadBtnClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (e.Row < 0 || e.Column < 0) return;

            FpSpread o = (FpSpread)sender;

            if (sender == this.ssList)
            {
                if (e.Column == (int)clsComSupSpd.enmNoExecuteMain.check01 )
                {
                    if (o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.check01].Text =="True")
                    {
                        o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
                    }
                    
                }
                else if (e.Column == (int)clsComSupSpd.enmNoExecuteMain.MemoSave )
                {

                    cEtc_NoExe_Remark = new clsComSup.cEtc_NoExe_Remark();
                    cEtc_NoExe_Remark.Pano = o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.Pano].Text.Trim();
                    cEtc_NoExe_Remark.TName = o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.Table].Text.Trim();
                    cEtc_NoExe_Remark.TROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Text.Trim();
                    cEtc_NoExe_Remark.Remark =ComFunc.QuotConv(o.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.Memo].Text.Trim());


                    // clsTrans DT = new clsTrans();
                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SqlErr = csup.ins_up_Etc_NoExe_Remark(clsDB.DbCon, cEtc_NoExe_Remark, ref intRowAffected);

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
                        }                        

                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                    }
                    
                    screen_display();
                   
                }

            }

        }

        void eSpreadDClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            if (e.Row <0 || e.Column <0 )
            {
                return;
            }

            string strPano = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.Pano].Text.Trim();
            string strDept = ssList_Sheet1.Cells[e.Row, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Text.Trim();
        }        

        void eChkClick(object sender,EventArgs e)
        {
            int i = 0;
            CheckBox o = (CheckBox)sender;

            if (sender == this.chkAll)
            {
                for ( i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    if (o.Checked == true)
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteMain.check01].Text = "1";
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteMain.check01].Text = "";
                    }                            
                }
            }
        }

        void eTxtLostFous(object sender,EventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (txtPano.Text.Trim() !="")
                {
                    txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
                }
            }
        }

        void eTxtKeyDown(object sender,KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    lblSName.Text = "";

                    if (txtPano.Text.Trim() != "")
                    {
                        txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);
                        lblSName.Text = read_PanoInfo(clsDB.DbCon, txtPano.Text);
                    }
                }                
            }
        }

        void ReadResvInfo(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd,string argPano)
        {            
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            
            spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                dt = csup.sel_Opd_Reserved_New(clsDB.DbCon, argPano);

                #region //데이터셋 읽어 자료 표시
                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");                    
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    spd.ActiveSheet.RowCount = dt.Rows.Count;
                    spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {                        
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteResv.RDate].Text = dt.Rows[i]["Date3"].ToString().Trim();

                    }
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                #endregion

            }
            catch (Exception ex)
            {
                //
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장                
                Cursor.Current = Cursors.Default;
            }


        }
         
        void setCombo()
        {
            int Inx = 0;
            DataTable dt = new DataTable();
            //
            cboPart.Items.Clear();
            cboPart.Items.Add("ALL");


            //
            cboDept.Items.Clear();
            cboDept.Items.Add("**.전체");
            cboDept.SelectedIndex = 0;
                       
            cboDept.Items.Clear();                      
            method.setCombo_View(this.cboDept, comSql.sel_BAS_CLINICDEPT_COMBO(clsDB.DbCon), clsParam.enmComParamComboType.ALL);    
                    
            cboDept.SelectedIndex = 0;


            dt = ComQuery.Select_BAS_PCCONFIG(clsDB.DbCon, clsCompuInfo.gstrCOMIP, "프로그램PC세팅", "외래OCS진료과세팅", "간호사_DeptCode");
            if(dt.Rows.Count > 0)
            {
                gstrDept = dt.Rows[0]["VALUEV"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
             
            //2019-02-11 KMC
            if (gstrDept != "")
            {
                Inx = cboDept.FindString(gstrDept);
                cboDept.SelectedIndex = Inx;

                eCboSelChanged(null, null);
            }
            else
            {
                cboDoct.Items.Clear();
                cboDoct.Items.Add("****.전체");
                cboDoct.SelectedIndex = 0;
            }


            cboBi.Items.Clear();
            cboBi.Items.Add("*.전체");
            cboBi.Items.Add("1.건강보험");
            cboBi.Items.Add("2.의료보험");
            cboBi.Items.Add("3.산재");
            cboBi.Items.Add("4.자보");
            cboBi.SelectedIndex = 0;

        }

        string read_PanoInfo(PsmhDb pDbCon,string argPano)
        {
            if (argPano !="")
            {                
                return fun.Read_Patient(pDbCon, ComFunc.SetAutoZero(argPano, ComNum.LENPTNO), "2");
            }
            else
            {
                return "";
            }
            
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread spd, string argFDate, string argTDate, string GbIO)
        {
            int i = 0;
            int nRow = 0;            
            string strXJong = "",  strOK = "";
            DataTable dt = null;


            spd.Enabled = false;

            spd.ActiveSheet.RowCount = 0;
            ssResv.ActiveSheet.RowCount = 0;
            

            strXJong = "";
            if (chkExam1.Checked == true) strXJong = "'1','2','3','4','5','6','7','8','9','D','K','Q' ";

            if (chkExam2.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'A','B','C','G','H','I'  ";
            if (chkExam3.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'E'  ";
            if (chkExam4.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'F'  ";
            if (chkExam5.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'J'  ";

            #region //미시행 관련 쿼리 배열 변수 세팅

            argsql = new string[Enum.GetValues(typeof(enmsql)).Length];
            argsql[(int)enmsql.GbIO] = GbIO;
            argsql[(int)enmsql.FDate] = argFDate;
            argsql[(int)enmsql.TDate] = argTDate;
            argsql[(int)enmsql.DeptCode] = cboDept.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.DrCode] = cboDoct.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.Pano] = txtPano.Text.Trim();
            argsql[(int)enmsql.XJong] = strXJong;

            argsql[(int)enmsql.ChkExam1] = chkExam1.Checked.ToString();
            argsql[(int)enmsql.ChkExam2] = chkExam2.Checked.ToString();
            argsql[(int)enmsql.ChkExam3] = chkExam3.Checked.ToString();
            argsql[(int)enmsql.ChkExam31] = chkExam31.Checked.ToString();
            argsql[(int)enmsql.ChkExam4] = chkExam4.Checked.ToString();
            argsql[(int)enmsql.ChkExam5] = chkExam5.Checked.ToString();
            argsql[(int)enmsql.ChkExam6] = chkExam6.Checked.ToString();
            argsql[(int)enmsql.ChkExam7] = chkExam7.Checked.ToString();
            argsql[(int)enmsql.ChkExam8] = chkExam8.Checked.ToString();
            argsql[(int)enmsql.ChkExam9] = chkExam9.Checked.ToString();
            argsql[(int)enmsql.ChkExam10] = chkExam10.Checked.ToString();
            argsql[(int)enmsql.ChkExam11] = chkExam11.Checked.ToString();

            argsql[(int)enmsql.ChkGum] = chkGum.Checked.ToString();
            argsql[(int)enmsql.ChkER] = chkEr.Checked.ToString();
            argsql[(int)enmsql.OptGb1] = optGb1.Checked.ToString();
            argsql[(int)enmsql.OptGb2] = optGb2.Checked.ToString();

            #endregion

            if (chkExam1.Checked == true || chkExam2.Checked == true || chkExam3.Checked == true || chkExam4.Checked == true || chkExam5.Checked == true)
            {

                #region //미시행 XRAY 관련 쿼리

                dt = sel_Xray_NoExecute(pDbCon, argsql);

                #endregion

                #region //데이터셋 읽어 자료 표시

                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, 24);

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "")
                        {
                            if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0) strOK = "";
                        }
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {
                                                
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["XCode"].ToString().Trim();

                        if (dt.Rows[i]["OrderName"].ToString().Trim() != "")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {                            
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(pDbCon, dt.Rows[i]["OrderCode"].ToString().Trim(),false);
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = VB.Left(dt.Rows[i]["CDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DelDate].Value = VB.Left(dt.Rows[i]["DelDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts1(dt.Rows[i]["GbSTS"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();
                        if (dt.Rows[i]["Death"].ToString().Trim()=="Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["DrRemark"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value =  dt.Rows[i]["DrRemark"].ToString().Trim();
                        }                                         
                        
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "XRAY_DETAIL";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ChkName].Value = clsVbfunc.GetInSaName(pDbCon, dt.Rows[i]["GSabun"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), "XRAY_DETAIL", dt.Rows[i]["ROWID"].ToString().Trim());
                        

                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion  

            }

            //내시경
            if (chkExam6.Checked == true)
            {

                #region //미시행 내시경 관련 쿼리

                dt = sel_Endo_NoExecute(pDbCon, argsql);

                #endregion                  

                #region //데이터셋 읽어 자료 표시                

                spd.ActiveSheet.RowCount = dt.Rows.Count + nRow;
                spd.ActiveSheet.SetRowHeight(-1, 24);


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)) strOK = "";
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Ptno"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = ""; // dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["XCode"].ToString().Trim();

                        if (dt.Rows[i]["OrderName"].ToString().Trim() != "")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {                            
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(pDbCon, dt.Rows[i]["OrderCode"].ToString().Trim(),false);
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = VB.Left(dt.Rows[i]["JDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }


                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts2(dt.Rows[i]["GbSunap"].ToString().Trim(), dt.Rows[i]["RDate"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();                        
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["DrRemark"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["DrRemark"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "ENDO_JUPMST";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), "ENDO_JUPMST", dt.Rows[i]["ROWID"].ToString().Trim());


                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion

            }


            //기타검사 (기능,심전도,echo
            if (chkExam7.Checked == true || chkExam3.Checked == true || chkExam9.Checked == true || chkExam10.Checked == true || chkExam11.Checked == true)
            {
                #region //미시행 기능검사 관련 쿼리

                dt = sel_Etc_NoExecute(pDbCon, argsql);

                #endregion

                #region //데이터셋 읽어 자료 표시

                spd.ActiveSheet.RowCount = dt.Rows.Count + nRow;
                spd.ActiveSheet.SetRowHeight(-1, 24);


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)) strOK = "";
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Ptno"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["OrderCode"].ToString().Trim();

                        if (dt.Rows[i]["OrderName"].ToString().Trim() != "")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {                         
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(pDbCon, dt.Rows[i]["OrderCode"].ToString().Trim(),false);
                        }

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = VB.Left(dt.Rows[i]["JDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }


                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts3(dt.Rows[i]["GBJOB"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();                        
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["Remark"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["Remark"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "ETC_JUPMST";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(pDbCon, dt.Rows[i]["Ptno"].ToString().Trim(), "ETC_JUPMST", dt.Rows[i]["ROWID"].ToString().Trim());


                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion

            }

            //혈액검사
            if (chkExam8.Checked == true)
            {

                #region //미시행 혈액,진검 관련 쿼리

                dt = sel_Exam_NoExecute(pDbCon, argsql);

                #endregion

                #region //데이터셋 읽어 자료 표시

                spd.ActiveSheet.RowCount = dt.Rows.Count + nRow;
                spd.ActiveSheet.SetRowHeight(-1, 24);


                for (i = 0; i < dt.Rows.Count; i++)
                {                    
                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)) strOK = "";
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["MasterCode"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["EXAMNAME"].ToString().Trim();


                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = ReadSts3(dt.Rows[i]["CDate"].ToString().Trim());

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts3(dt.Rows[i]["GBJOB"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();                        
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["DRCOMMENT"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["DRCOMMENT"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "EXAM_ORDER";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ChkName].Value = dt.Rows[i]["Part2"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(pDbCon, dt.Rows[i]["Pano"].ToString().Trim(), "EXAM_ORDER", dt.Rows[i]["ROWID"].ToString().Trim());


                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion

            }

            spd.ActiveSheet.RowCount = nRow;

            ssList.Enabled = true;


        }

        /// <summary>
        /// 영상의학과 미시행 쿼리 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        DataTable sel_Xray_NoExecute(PsmhDb pDbCon, string[] arg)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT a.Pano, A.SNAME, A.AGE, A.SEX, A.DEPTCODE, A.DRCODE, A.GBSTS, A.ORDERNAME,a.OrderCode,  ";

            SQL = SQL + "\r\n" + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE, ";
            SQL = SQL + "\r\n" + " TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDATE,";
            SQL = SQL + "\r\n" + " TO_CHAR(a.CDate,'YYYY-MM-DD HH24:MI') CDATE,";
            SQL = SQL + "\r\n" + " TO_CHAR(a.DELDate,'YYYY-MM-DD HH24:MI') DELDATE,";
            SQL = SQL + "\r\n" + " TO_CHAR(A.SEEKDATE,'YYYY-MM-DD') SEEDATE,  A.PACSSTUDYID,";
            SQL = SQL + "\r\n" + " a.XJong, a.XCode,  a.GbEnd, A.XRAYROOM, a.EXINFO,";
            SQL = SQL + "\r\n" + " TO_CHAR(A.XSENDDATE,'MM/DD') XSENDDATE, A.ROWID, A.ORDERNO,  A.DRREMARK , a.GSABUN ";
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_NUR_STD_DEATH_CHK(a.Pano) AS DEATH "; //사망체크 function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(a.OrderCode) AS FC_ORDERNAME "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_ETC_NOEXE_REMARK(a.Pano,'XRAY_DETAIL',a.ROWID) AS FC_NOEXE_MEMO "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) AS FC_DrName "; // function
            SQL = SQL + "\r\n" + " FROM " + ComNum.DB_PMPA + "XRAY_DETAIL a ";
            SQL = SQL + "\r\n" + "  WHERE 1=1 ";

            SQL = SQL + "\r\n" + "   AND ( ";
            SQL = SQL + "\r\n" + "          (A.BDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.BDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD'))";
            SQL = SQL + "\r\n" + "         OR (A.RDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.RDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + " 23:59','YYYY-MM-DD HH24:MI')) ";
            SQL = SQL + "\r\n" + "       ) ";


            if (arg[(int)enmsql.Pano] != "") SQL = SQL + "\r\n" + " AND PANO = '" + arg[(int)enmsql.Pano] + "' ";
            if (VB.Left(arg[(int)enmsql.DeptCode], 2) != "**") SQL = SQL + ComNum.VBLF + " AND a.DEPTCODE='" + VB.Left(arg[(int)enmsql.DeptCode], 2) + "'  ";
            if (VB.Left(arg[(int)enmsql.DrCode], 4) != "****") SQL = SQL + ComNum.VBLF + " AND a.DrCode='" + VB.Left(arg[(int)enmsql.DrCode], 4) + "'  ";

            SQL = SQL + "\r\n" + " AND a.XJong IN ( " + arg[(int)enmsql.XJong] + ") ";

            if (VB.Left(arg[(int)enmsql.DeptCode], 2) == "ER")
            {
                SQL = SQL + "\r\n" + " AND a.XCode NOT IN ( 'G0400A','XCDC' ) ";
            }
            else
            {
                SQL = SQL + "\r\n" + " AND a.XCode NOT IN ( 'G0400A' ) ";
            }

            if (arg[(int)enmsql.ChkGum] == "True") SQL = SQL + "\r\n" + " AND a.Gsabun > 0 ";

            if (arg[(int)enmsql.OptGb2] == "True")
            {
                SQL = SQL + "\r\n" + " AND (A.GBSTS IS NULL OR A.GBSTS NOT IN ('7','D') ) "; //미실시
            }
            else if (arg[(int)enmsql.OptGb1] == "True")
            {
                SQL = SQL + "\r\n" + " AND A.GBSTS IN ('7') ";
            }

            SQL = SQL + "\r\n" + " AND a.IPDOPD = '" + arg[(int)enmsql.GbIO] + "' ";
            SQL = SQL + "\r\n" + " AND a.DEPTCODE NOT IN ('HR','TO') ";

            //해당 스테이션 설정대상만 //TODO 윤조연 
            //if (chkStation.Checked == true && GstrEmrViewDoct != "")  SQL = SQL + "  AND a.DrCode IN (" + GstrEmrViewDoct + ") ";

            if (arg[(int)enmsql.ChkER] == "True")
            {
                SQL = SQL + "\r\n" + " AND a.OrderCode IN ('US22ER','ERUS119','00680225','00680226','00680227','00680228','00604635','00604637') ";
                SQL = SQL + "\r\n" + " AND a.Pano IN ( SELECT Pano FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + "\r\n" + "                   WHERE 1=1 ";
                SQL = SQL + "\r\n" + "                    AND ACTDATE>=TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD') ";
                SQL = SQL + "\r\n" + "                    AND ACTDATE<=TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD') ";
                SQL = SQL + "\r\n" + "                    AND DEPTCODE='ER' ";
                SQL = SQL + "\r\n" + "                ) ";
            }

            SQL = SQL + "\r\n" + "  ORDER BY a.Pano,a.XJong,a.XSENDDATE DESC, A.SeekDate DESC ";


            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        /// <summary>
        /// 내시경 미시행 쿼리 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        DataTable sel_Endo_NoExecute(PsmhDb pDbCon, string[] arg)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT a.PTno, A.SNAME,  A.SEX, A.DEPTCODE, A.DRCODE,  ";

            SQL = SQL + "\r\n" + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE, ";
            SQL = SQL + "\r\n" + " TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDATE, A.JDATE,";
            SQL = SQL + "\r\n" + " A.GBJOB, A.GBSUNAP,a.ORDERCODE, ";
            SQL = SQL + "\r\n" + " TO_CHAR(a.ResultDate,'MM/DD') ResultDate, A.SEQNO, A.ROWID, A.ORDERNO, A.REMARK ";
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_NUR_STD_DEATH_CHK(a.Ptno) AS DEATH "; //사망체크 function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(a.ORDERCODE) AS FC_ORDERNAME "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_ETC_NOEXE_REMARK(a.Ptno,'ENDO_JUPMST',a.ROWID) AS FC_NOEXE_MEMO "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) AS FC_DrName "; // function
            SQL = SQL + "\r\n" + " FROM " + ComNum.DB_MED + "ENDO_JUPMST a ";
            SQL = SQL + "\r\n" + "  WHERE 1=1 ";

            SQL = SQL + "\r\n" + "   AND ( ";
            SQL = SQL + "\r\n" + "          (A.BDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.BDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD'))";
            SQL = SQL + "\r\n" + "         OR (A.RDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.RDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD')) ";
            SQL = SQL + "\r\n" + "       ) ";

            SQL = SQL + "\r\n" + "  AND a.RDate =a.BDate ";

            if (arg[(int)enmsql.Pano] != "") SQL = SQL + "\r\n" + " AND PtNO = '" + arg[(int)enmsql.Pano] + "' ";
            if (VB.Left(arg[(int)enmsql.DeptCode], 2) != "**") SQL = SQL + ComNum.VBLF + " AND a.DEPTCODE='" + VB.Left(arg[(int)enmsql.DeptCode], 2) + "'  ";
            if (VB.Left(arg[(int)enmsql.DrCode], 4) != "****") SQL = SQL + ComNum.VBLF + " AND a.DrCode='" + VB.Left(arg[(int)enmsql.DrCode], 4) + "'  ";


            if (arg[(int)enmsql.OptGb2] == "True")
            {
                SQL = SQL + "\r\n" + " AND A.GBSUNAP NOT IN  ('7','*')   "; //미실시
            }
            else if (arg[(int)enmsql.OptGb1] == "True")
            {
                SQL = SQL + "\r\n" + " AND A.GBSUNAP IN ( '7' ) ";
            }

            SQL = SQL + "\r\n" + " AND a.GbSunap <>'*' ";
            SQL = SQL + "\r\n" + " AND a.GBIO = '" + arg[(int)enmsql.GbIO] + "' ";
            SQL = SQL + "\r\n" + " AND a.DEPTCODE NOT IN ('HR','TO') ";

            //해당 스테이션 설정대상만 //TODO 윤조연 
            //if (chkStation.Checked == true && GstrEmrViewDoct != "")  SQL = SQL + "  AND a.DrCode IN (" + GstrEmrViewDoct + ") ";

            if (arg[(int)enmsql.ChkER] == "True")
            {
                SQL = SQL + "\r\n" + " AND a.OrderCode IN ('er') ";
                SQL = SQL + "\r\n" + " AND a.Ptno IN ( SELECT Pano FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + "\r\n" + "                   WHERE 1=1 ";
                SQL = SQL + "\r\n" + "                    AND ACTDATE>=TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD') ";
                SQL = SQL + "\r\n" + "                    AND ACTDATE<=TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD') ";
                SQL = SQL + "\r\n" + "                    AND DEPTCODE='ER' ";
                SQL = SQL + "\r\n" + "                ) ";
            }

            SQL = SQL + "\r\n" + "  ORDER BY a.RDate,a.PTno ";



            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        /// <summary>
        /// 기능검사 미시행 쿼리 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        DataTable sel_Etc_NoExecute(PsmhDb pDbCon, string[] arg)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT a.PTno, A.SNAME, A.AGE, A.SEX, A.DEPTCODE, A.DRCODE,  ";

            SQL = SQL + "\r\n" + " TO_CHAR(a.BDate,'YYYY-MM-DD') BDATE, ";
            SQL = SQL + "\r\n" + " TO_CHAR(a.RDate,'YYYY-MM-DD HH24:MI') RDATE, ";
            SQL = SQL + "\r\n" + " A.REMARK, A.ORDERCODE,'' OrderName, a.GbJob,  ";
            SQL = SQL + "\r\n" + " A.GUBUN, A.ROWID, A.ORDERNO ";
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_NUR_STD_DEATH_CHK(a.Ptno) AS DEATH "; //사망체크 function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(a.OrderCode) AS FC_ORDERNAME "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_ETC_NOEXE_REMARK(a.Ptno,'ETC_JUPMST',a.ROWID) AS FC_NOEXE_MEMO "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) AS FC_DrName "; // function
            SQL = SQL + "\r\n" + " FROM " + ComNum.DB_MED + "ETC_JUPMST a ";
            SQL = SQL + "\r\n" + "  WHERE 1=1 ";

            SQL = SQL + "\r\n" + "   AND ( ";
            SQL = SQL + "\r\n" + "          (A.BDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.BDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD'))";
            SQL = SQL + "\r\n" + "         OR (A.RDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.RDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + " 23:59','YYYY-MM-DD HH24:MI')) ";
            SQL = SQL + "\r\n" + "       ) ";



            if (arg[(int)enmsql.Pano] != "") SQL = SQL + "\r\n" + " AND PtNO = '" + arg[(int)enmsql.Pano] + "' ";
            if (VB.Left(arg[(int)enmsql.DeptCode], 2) != "**") SQL = SQL + ComNum.VBLF + " AND a.DEPTCODE='" + VB.Left(arg[(int)enmsql.DeptCode], 2) + "'  ";
            if (VB.Left(arg[(int)enmsql.DrCode], 4) != "****") SQL = SQL + ComNum.VBLF + " AND a.DrCode='" + VB.Left(arg[(int)enmsql.DrCode], 4) + "'  ";


            if (arg[(int)enmsql.OptGb2] == "True")
            {
                SQL = SQL + "\r\n" + " AND A.GBJOB IN ('1','2' )  "; //미실시
            }
            else if (arg[(int)enmsql.OptGb1] == "True")
            {
                SQL = SQL + "\r\n" + " AND A.GBJOB IN ('3') ";
            }

            if (arg[(int)enmsql.ChkExam7] == "False" && arg[(int)enmsql.ChkExam3] == "True")
            {
                SQL = SQL + "\r\n" + " AND A.BUN IN ( '43' ) ";
            }
            else if (arg[(int)enmsql.ChkExam7] == "True" && arg[(int)enmsql.ChkExam3] == "False")
            {
                SQL = SQL + "\r\n" + " AND A.BUN NOT IN ( '43' ) ";
            }


            if (arg[(int)enmsql.ChkExam9] == "True" && arg[(int)enmsql.ChkExam10] == "True" )
            {
                if(arg[(int)enmsql.ChkExam11] == "True")
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('3','9','10','11','1','4','5','6','7','8','16','26')  ";
                }
                else
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('3','9','10','11','1','4','5','6','7','8','16')  ";
                }
                
            }
            else if (arg[(int)enmsql.ChkExam9] == "True" && arg[(int)enmsql.ChkExam10] == "False" )
            {
                if (arg[(int)enmsql.ChkExam11] == "True")
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('3','9','10','11', '26')  ";
                }
                else
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('3','9','10','11')  ";
                }
              
            }
            else if (arg[(int)enmsql.ChkExam9] == "False" && arg[(int)enmsql.ChkExam10] == "True")
            {
                if (arg[(int)enmsql.ChkExam11] == "True")
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('1','4','5','6','7','8','16','26')  ";
                }
                else
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('1','4','5','6','7','8','16')  ";
                }
             
            }
            else if (arg[(int)enmsql.ChkExam9] == "False" && arg[(int)enmsql.ChkExam10] == "False")
            {
                if (arg[(int)enmsql.ChkExam11] == "True")
                {
                    SQL = SQL + "\r\n" + " AND GUBUN IN ('26')  ";
                }

            }

            if (arg[(int)enmsql.ChkExam31] == "False") SQL = SQL + "\r\n" + " AND a.GBIO = '" + arg[(int)enmsql.GbIO] + "' ";
            SQL = SQL + "\r\n" + " AND a.DEPTCODE NOT IN ('HR','TO') ";

            //해당 스테이션 설정대상만 //TODO 윤조연 
            //if (chkStation.Checked == true && GstrEmrViewDoct != "")  SQL = SQL + "  AND a.DrCode IN (" + GstrEmrViewDoct + ") ";

            if (arg[(int)enmsql.ChkER] == "True")
            {
                SQL = SQL + "\r\n" + " AND a.OrderCode IN ('E6541','US22ER','ERUS119','00680400') ";

            }

            SQL = SQL + "\r\n" + "  ORDER BY a.RDate,a.PTno ";


            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        /// <summary>
        /// 혈액,진검 미시행 쿼리 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        DataTable sel_Exam_NoExecute(PsmhDb pDbCon, string[] arg)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            SQL = " SELECT  a.Pano, A.SNAME, A.AGE, A.SEX, A.DEPTCODE, A.DRCODE, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, TO_CHAR(ACTDATE ,'YYYY-MM-DD') ACTDATE , ";

            SQL = SQL + "\r\n" + " TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(CDATE,'YYYY-MM-DD') CDATE, A.DRCOMMENT, ";
            SQL = SQL + "\r\n" + " TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, ORDERNO, A.MASTERCODE , B.EXAMNAME, A.ROWID  ,A.PART2 ";
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_NUR_STD_DEATH_CHK(a.Pano) AS DEATH "; //사망체크 function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_ETC_NOEXE_REMARK(a.Pano,'EXAM_ORDER',a.ROWID) AS FC_NOEXE_MEMO "; // function
            SQL = SQL + "\r\n" + " ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(a.DrCode) AS FC_DrName "; // function
            SQL = SQL + "\r\n" + " FROM " + ComNum.DB_MED + "EXAM_ORDER a , " + ComNum.DB_MED + "EXAM_MASTER b ";
            SQL = SQL + "\r\n" + "  WHERE 1=1 ";

            SQL = SQL + "\r\n" + "   AND ( ";
            SQL = SQL + "\r\n" + "          (A.BDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.BDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD'))";
            SQL = SQL + "\r\n" + "         OR (A.RDATE >= TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD')   AND A.RDATE <= TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD')) ";
            SQL = SQL + "\r\n" + "       ) ";

            SQL = SQL + "\r\n" + " AND (A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ')   ";
            SQL = SQL + "\r\n" + " AND (a.SDate IS NULL OR a.SDate ='')   ";
            SQL = SQL + "\r\n" + " AND A.MASTERCODE = B.MASTERCODE   ";

            if (arg[(int)enmsql.Pano] != "") SQL = SQL + "\r\n" + " AND PANO = '" + arg[(int)enmsql.Pano] + "' ";
            if (VB.Left(arg[(int)enmsql.DeptCode], 2) != "**") SQL = SQL + ComNum.VBLF + " AND a.DEPTCODE='" + VB.Left(arg[(int)enmsql.DeptCode], 2) + "'  ";
            if (VB.Left(arg[(int)enmsql.DrCode], 4) != "****") SQL = SQL + ComNum.VBLF + " AND a.DrCode='" + VB.Left(arg[(int)enmsql.DrCode], 4) + "'  ";


            if (arg[(int)enmsql.OptGb2] == "True")
            {
                SQL = SQL + "\r\n" + " AND (SPECNO IS NULL or SPECNO ='')  "; //미실시
            }
            else if (arg[(int)enmsql.OptGb1] == "True")
            {
                SQL = SQL + "\r\n" + " AND SPECNO IS NOT NULL ";
            }


            SQL = SQL + "\r\n" + " AND a.IPDOPD = '" + arg[(int)enmsql.GbIO] + "' ";
            SQL = SQL + "\r\n" + " AND a.DEPTCODE NOT IN ('HR','TO') ";

            //해당 스테이션 설정대상만 //TODO 윤조연 
            //if (chkStation.Checked == true && GstrEmrViewDoct != "")  SQL = SQL + "  AND a.DrCode IN (" + GstrEmrViewDoct + ") ";

            if (arg[(int)enmsql.ChkGum] == "True") SQL = SQL + "\r\n" + " AND a.Part2 > 0 ";

            if (arg[(int)enmsql.ChkER] == "True")
            {
                SQL = SQL + "\r\n" + " AND a.MasterCode IN ('er') ";
                SQL = SQL + "\r\n" + " AND a.Pano IN ( SELECT Pano FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + "\r\n" + "                   WHERE 1=1 ";
                SQL = SQL + "\r\n" + "                    AND ACTDATE>=TO_DATE('" + arg[(int)enmsql.FDate] + "','YYYY-MM-DD') ";
                SQL = SQL + "\r\n" + "                    AND ACTDATE<=TO_DATE('" + arg[(int)enmsql.TDate] + "','YYYY-MM-DD') ";
                SQL = SQL + "\r\n" + "                    AND DEPTCODE='ER' ";
                SQL = SQL + "\r\n" + "                ) ";

            }

            SQL = SQL + "\r\n" + "  ORDER BY  a.BDATE, a.PANO,a.DEPTCODE, a.MASTERCODE ";


            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return dt;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;

        }

        string ReadSts1(string argsts)
        {
            if (argsts == "0")
            {
                return "미확인";
            }
            else if (argsts == "1")
            {
                return "접수";
            }
            else if (argsts == "2")
            {
                return "예약";
            }
            else if (argsts == "3")
            {
                return "촬영준비";
            }
            else if (argsts == "7")
            {
                return "완료";
            }
            else if (argsts == "D")
            {
                return "수납취소";
            }
            else if (argsts == "S")
            {
                return "호명";
            }
            else
            {
                return "";
            }

        }
        //내시경구분
        string ReadSts2(string argsts, string strRDate)
        {
            if (argsts == "1")
            {
                if (strRDate != "")
                {
                    if (strRDate.CompareTo(cpublic.strSysDate) >= 0)
                    {
                        return "예약";
                    }
                    else
                    {
                        return "예약부도";
                    }

                }
                else
                {
                    return "접수";
                }

            }
            else if (argsts == "2")
            {
                return "미접수";
            }
            else if (argsts == "*")
            {
                return "취소";
            }
            else if (argsts == "7")
            {
                return "완료";
            }
            else
            {
                return "";
            }

        }

        //기능검사
        string ReadSts3(string argsts)
        {
            if (argsts == "1")
            {
                return "미접수";

            }
            else if (argsts == "2")
            {
                return "예약";
            }
            else if (argsts == "3")
            {
                return "접수";
            }
            else if (argsts == "9")
            {
                return "취소";
            }
            else
            {
                return "";
            }

        }

        void eCboSelChanged(object sender, EventArgs e)
        { 
            DataTable dt = sup.sel_Bas_Doctor_ComBo(clsDB.DbCon, VB.Left(this.cboDept.SelectedItem.ToString(), 2), "",false,true,true);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboDoct, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

        }

        void ePrint(string Gbn)
        {
            

			string[] strhead = new string[2];
            string[] strfont = new string[2];

            //
            read_sysdate();

            strfont[0] = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strfont[1] = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strhead[0] = "/c/f1/n" + "미시행 검사 내역" + "/f1/n";
            strhead[1] = "/n/l/f2" + "조회기간 : " + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + " /l/f2" + "  출력시간 : " + cpublic.strSysDate + " " + cpublic.strSysTime + " /n";

            if (Gbn == "인쇄")
            {
                cSpd.setColStyle(ssList, -1, (int)clsComSupSpd.enmNoExecuteMain.check01, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(ssList, -1, (int)clsComSupSpd.enmNoExecuteMain.DelDate, clsSpread.enmSpdType.Hide);
                cSpd.setColStyle(ssList, -1, (int)clsComSupSpd.enmNoExecuteMain.MemoSave, clsSpread.enmSpdType.Hide);
                
            }
            else if (Gbn == "선택인쇄")
            {
                for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmNoExecuteMain.check01].Text == "")
                    {
                        ssList.ActiveSheet.Rows[i].Visible = false;                        
                    }

                }
            }

            csupspd.SPREAD_PRINT(ssList_Sheet1, ssList, strhead, strfont, 10, 10, 2, true);

            //시트초기화
            csupspd.sSpd_NoExecuteMain(ssList, csupspd.sSpdNoExecutoMain, csupspd.nSpdNoExecutoMain, 1, 0);

            

        }

        void read_cinfo(FpSpread Spd, int row)
        {
            if (row < 0)
            {
                return;
            }

            if (Spd.ActiveSheet.RowCount == 0)
            {
                return;
            }

            //환자공통정보 표시                   
            pInfo = new clsComSupSpd.sPatInfo();
            pInfo.strPano = ssList_Sheet1.Cells[row, (int)clsComSupSpd.enmNoExecuteMain.Pano].Text.Trim();
            pInfo.strDept = ssList_Sheet1.Cells[row, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Text.Trim();
            pInfo.strExCode = ssList_Sheet1.Cells[row, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Text.Trim();
            pInfo.strROWID = ssList_Sheet1.Cells[row, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Text.Trim();
            

        }

        void screen_display()
        {
            //
            GetData_th(ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim(), "O");

            //환자공통정보 표시 clear                  
            pInfo = new clsComSupSpd.sPatInfo();
        }

        string Read_NoExcuteRemark(PsmhDb pDbCon, string argPano,string argTName,string argTRowid)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += "  Remark                                                                    \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "ETC_NOEXE_REMARK                              \r\n";
            SQL += "  WHERE 1 = 1                                                               \r\n";
            SQL += "   AND Pano ='" + argPano + "'                                              \r\n";
            SQL += "   AND Table_Name ='" + argTName + "'                                       \r\n";
            SQL += "   AND Table_ROWID ='" + argTRowid + "'                                     \r\n";
            
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt == null) return "";

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Remark"].ToString().Trim();
                }
                else
                {
                    return "";
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

     
        }

        string up_NoExecute(PsmhDb pDbCon, string argGbn,string argTable, string argRDate, string argRowId, ref int intRowAffected)
        {
            string SQL = "";
            string SqlErr = string.Empty;


            SQL = "";

            if  (argGbn == "완료처리" || argGbn == "수동취소" )
            {
                if (argTable == "EXAM_ORDER" || argTable == "ENDO_JUPMST")
                {
                    return "";
                }
            }

            //기본
            if (argTable=="EXAM_ORDER")
            {
                SQL += " UPDATE " + ComNum.DB_MED + "EXAM_ORDER         SET     \r\n";
            }
            else if (argTable == "ENDO_JUPMST")
            {
                SQL += " UPDATE " + ComNum.DB_MED + "ENDO_JUPMST        SET     \r\n";                
            }
            else if (argTable == "ETC_JUPMST")
            {
                SQL += " UPDATE " + ComNum.DB_MED + "ETC_JUPMST         SET     \r\n";                
            }
            else if (argTable == "XRAY_DETAIL")
            {
                SQL += " UPDATE " + ComNum.DB_PMPA + "XRAY_DETAIL       SET     \r\n";                
            }


            if (argGbn =="예약변경")
            {
                if (argTable == "ENDO_JUPMST")
                {
                    SQL += "    GBSUNAP ='1',                                       \r\n";
                }
                else if (argTable == "ETC_JUPMST")
                {
                    SQL += "    GBJOB ='2',                                         \r\n";
                }
                else if (argTable == "XRAY_DETAIL")
                {
                    SQL += "    GBSTS ='2',                                         \r\n";
                }
                SQL += "     RDate = TO_DATE('" + argRDate + "','YYYY-MM-DD')       \r\n";
            }
            else if (argGbn == "완료처리")
            {
                if (argTable == "ETC_JUPMST")
                {
                    SQL += "    CDate =SYSDATE,                                     \r\n";
                    SQL += "    CSabun ="+ Convert.ToUInt16(clsType.User.IdNumber) + ",                \r\n";
                    SQL += "    GBJOB ='3'                                          \r\n";
                }
                else if (argTable == "XRAY_DETAIL")
                {
                    SQL += "    CDate =SYSDATE,                                     \r\n";
                    SQL += "    CSabun ='" +clsType.User.IdNumber + "',  \r\n";
                    SQL += "    GBSTS ='7',                                         \r\n";
                    SQL += "    GBRESERVED ='7'                                     \r\n";
                }                
            }
            else if (argGbn == "수동취소")
            {
                if (argTable == "ETC_JUPMST")
                {
                    SQL += "    CDate =SYSDATE,                                     \r\n";
                    SQL += "    CSabun =" + Convert.ToUInt16(clsType.User.IdNumber) + ",                \r\n";
                    SQL += "    GBJOB ='9'                                          \r\n";
                }
                else if (argTable == "XRAY_DETAIL")
                {
                    SQL += "    DelDate =SYSDATE,                                   \r\n";
                    SQL += "    CSabun ='" +clsType.User.IdNumber + "',  \r\n";
                    SQL += "    GBSTS ='D',                                         \r\n";
                    SQL += "    cRemark ='스테이션 수동취소'                        \r\n";
                }
            }


            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND ROWID = " + ComFunc.covSqlstr(argRowId, false) ;


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string CHK_NoExecute_STS(PsmhDb pDbCon, string argGbn, string argTable, string argRDate, string argRowId, ref int intRowAffected)
        { 
            string SQL = "";
            string SqlErr = string.Empty;
            string strOK = "";
            DataTable dt = null;


            SQL = "";
            SQL = SQL += " SELECT * FROM " + ComNum.DB_MED + "ETC_JUPMST            \r\n";
            SQL = SQL += "  WHERE 1=1                                               \r\n";
            SQL = SQL += "  AND ROWID  ='" + argRowId + "'                          \r\n";
            //2019-02-15 안정수, gubun2 조건 임시로 제외 
            //SQL = SQL += "  AND (GBJOB IN('3') OR GUBUN2 IS NOT NULL)               \r\n";

            //2019-04-29 안정수, 조건변경
            SQL = SQL += "  AND (GBJOB ='3' OR GUBUN2 = '1' OR ARRDATE IS NOT NULL)  \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }
                if (dt == null) return "";

                if (dt.Rows.Count > 0)
                {
                    strOK = "OK";
                    return strOK;
                }
                else
                {
                    return "";
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

        }

        //스레드 데이타조회
        void GetData_th(FarPoint.Win.Spread.FpSpread Spd, string argFDate, string argTDate, string GbIO)
        {
            string strXJong = "";
                                    
            Cursor.Current = Cursors.WaitCursor;

            Spd.ActiveSheet.RowCount = 0;
            ssResv.ActiveSheet.RowCount = 0;
            

            strXJong = "";
            if (chkExam1.Checked == true) strXJong = "'1','2','3','4','5','6','7','8','9','D','K','Q' ";

            if (chkExam2.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'A','B','C','G','H','I'  ";
            if (chkExam3.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'E'  ";
            if (chkExam4.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'F'  ";
            if (chkExam5.Checked == true) strXJong = strXJong + VB.IIf(strXJong != "", ",", "") + "'J'  ";

            #region //미시행 관련 쿼리 배열 변수 세팅

            argsql = new string[Enum.GetValues(typeof(enmsql)).Length];
            argsql[(int)enmsql.GbIO] = GbIO;
            argsql[(int)enmsql.FDate] = argFDate;
            argsql[(int)enmsql.TDate] = argTDate;
            argsql[(int)enmsql.DeptCode] = cboDept.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.DrCode] = cboDoct.SelectedItem.ToString().Trim();
            argsql[(int)enmsql.Pano] = txtPano.Text.Trim();
            argsql[(int)enmsql.XJong] = strXJong;

            argsql[(int)enmsql.ChkExam1] = chkExam1.Checked.ToString();
            argsql[(int)enmsql.ChkExam2] = chkExam2.Checked.ToString();
            argsql[(int)enmsql.ChkExam3] = chkExam3.Checked.ToString();
            argsql[(int)enmsql.ChkExam31] = chkExam31.Checked.ToString();
            argsql[(int)enmsql.ChkExam4] = chkExam4.Checked.ToString();
            argsql[(int)enmsql.ChkExam5] = chkExam5.Checked.ToString();
            argsql[(int)enmsql.ChkExam6] = chkExam6.Checked.ToString();
            argsql[(int)enmsql.ChkExam7] = chkExam7.Checked.ToString();
            argsql[(int)enmsql.ChkExam8] = chkExam8.Checked.ToString();
            argsql[(int)enmsql.ChkExam9] = chkExam9.Checked.ToString();
            argsql[(int)enmsql.ChkExam10] = chkExam10.Checked.ToString();
            argsql[(int)enmsql.ChkExam11] = chkExam11.Checked.ToString();

            argsql[(int)enmsql.ChkGum] = chkGum.Checked.ToString();
            argsql[(int)enmsql.ChkER] = chkEr.Checked.ToString();
            argsql[(int)enmsql.OptGb1] = optGb1.Checked.ToString();
            argsql[(int)enmsql.OptGb2] = optGb2.Checked.ToString();

            #endregion

            
            spd = ssList;

            //스레드 시작
            thread = new Thread(tProcess);
            thread.Start();

            Cursor.Current = Cursors.Default;
            

        }

        #region //데이타내용 스프레드 표시 스레드 적용

        void tProcess()
        {

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { true });
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));            
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = false));

            DataTable dt = null;

            spd.ActiveSheet.RowCount = 0;
            nRow = 0;

            if (chkExam1.Checked == true || chkExam2.Checked == true || chkExam3.Checked == true || chkExam4.Checked == true || chkExam5.Checked == true)
            {
                dt = sel_Xray_NoExecute(clsDB.DbCon, argsql);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "1");
            }
            if (chkExam6.Checked == true)
            {
                dt = sel_Endo_NoExecute(clsDB.DbCon, argsql);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "2");
            }
            if (chkExam7.Checked == true || chkExam3.Checked == true || chkExam9.Checked == true || chkExam10.Checked == true || chkExam11.Checked == true)
            {
                dt = sel_Etc_NoExecute(clsDB.DbCon, argsql);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "3");
            }
            if (chkExam8.Checked == true)
            {
                dt = sel_Exam_NoExecute(clsDB.DbCon, argsql);
                this.Invoke(new threadSpdTypeDelegate(tShowSpread), spd, dt, "4");
            }

            if (gstrER == "ER")
            {
                if(spd.ActiveSheet.RowCount > 0)
                {
                    for(int i =0; i < spd.ActiveSheet.RowCount; i ++)
                    {
                        if(spd.ActiveSheet.Cells[i, 10].Text == "E6541" )
                        {
                            spd.ActiveSheet.Cells[i, 0].Locked = false;
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[i, 0].Locked = true;
                        }
                    }
                  
                }
                
            }

            this.Invoke(new threadProcessDelegate(trunCircular), new object[] { false });
            this.spd.BeginInvoke(new System.Action(() => this.spd.Enabled = true));            
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void threadSpdTypeDelegate(FarPoint.Win.Spread.FpSpread spd,DataTable dt,string job);
        void tShowSpread(FarPoint.Win.Spread.FpSpread spd,DataTable dt,string job)
        {
            int i = 0;            
            string  strOK = "";                       

            if (job == "1")
            {
                #region //데이터셋 읽어 자료 표시

                if (dt == null || dt.Rows.Count == 0) return;
                
                spd.ActiveSheet.RowCount = dt.Rows.Count;
                spd.ActiveSheet.SetRowHeight(-1, 24);

                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "")
                        {
                            if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0) strOK = "";
                        }
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Gubun].Value = "XRAY";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["XCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["FC_OrderName"].ToString().Trim();
                        //if (dt.Rows[i]["FC_OrderName"].ToString().Trim() != "")
                        //{
                        //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["FC_OrderName"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(dt.Rows[i]["OrderCode"].ToString().Trim());
                        //}

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = VB.Left(dt.Rows[i]["CDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DelDate].Value = VB.Left(dt.Rows[i]["DelDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts1(dt.Rows[i]["GbSTS"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["DrRemark"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["DrRemark"].ToString().Trim();
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "XRAY_DETAIL";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ChkName].Value = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["GSabun"].ToString().Trim());
                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(dt.Rows[i]["Pano"].ToString().Trim(), "XRAY_DETAIL", dt.Rows[i]["ROWID"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = dt.Rows[i]["FC_NOEXE_MEMO"].ToString().Trim();




                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion  
            }

            //내시경
            if (job == "2")
            {
                #region //데이터셋 읽어 자료 표시                

                if (dt == null || dt.Rows.Count == 0) return;
                spd.ActiveSheet.RowCount = dt.Rows.Count + nRow;
                spd.ActiveSheet.SetRowHeight(-1, 24);


                for (i = 0; i < dt.Rows.Count; i++)
                {

                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)) strOK = "";
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Gubun].Value = "ENDO";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Ptno"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = ""; // dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["OrderCode"].ToString().Trim();

                        //if (dt.Rows[i]["OrderName"].ToString().Trim() != "")
                        //{
                        //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["OrderName"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(dt.Rows[i]["OrderCode"].ToString().Trim());
                        //}
                        ///spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(dt.Rows[i]["OrderCode"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["FC_OrderName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = VB.Left(dt.Rows[i]["JDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }


                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts2(dt.Rows[i]["GbSunap"].ToString().Trim(), dt.Rows[i]["RDate"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["DrRemark"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["Remark"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "ENDO_JUPMST";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(dt.Rows[i]["Ptno"].ToString().Trim(), "ENDO_JUPMST", dt.Rows[i]["ROWID"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = dt.Rows[i]["FC_NOEXE_MEMO"].ToString().Trim();


                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion
            }
            
            //기타검사 (기능,심전도,echo
            if (job == "3")
            {
                #region //데이터셋 읽어 자료 표시

                if (dt == null || dt.Rows.Count == 0) return;
                spd.ActiveSheet.RowCount = dt.Rows.Count + nRow;
                spd.ActiveSheet.SetRowHeight(-1, 24);


                for (i = 0; i < dt.Rows.Count; i++)
                {

                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)) strOK = "";
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Gubun].Value = "FUNC";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Ptno"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["OrderCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["FC_OrderName"].ToString().Trim();

                        if(gstrER == "ER")
                        {
                            if(dt.Rows[i]["FC_OrderName"].ToString().Trim() == "Monitoring")
                            {
                                spd.ActiveSheet.Rows[nRow].Visible = false;
                            }
                        }
                        //if (dt.Rows[i]["OrderName"].ToString().Trim() != "")
                        //{
                        //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["OrderName"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = cxray.OCS_XNAME_READ(dt.Rows[i]["OrderCode"].ToString().Trim());
                        //}

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = VB.Left(dt.Rows[i]["JDate"].ToString().Trim(), 10);
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }


                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts3(dt.Rows[i]["GBJOB"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["Remark"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["Remark"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "ETC_JUPMST";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(dt.Rows[i]["Ptno"].ToString().Trim(), "ETC_JUPMST", dt.Rows[i]["ROWID"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = dt.Rows[i]["FC_NOEXE_MEMO"].ToString().Trim();

                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion
            }

            //혈액검사
            if (job == "4")
            {
                #region //데이터셋 읽어 자료 표시

                if (dt == null || dt.Rows.Count == 0) return;
                spd.ActiveSheet.RowCount = dt.Rows.Count + nRow;
                spd.ActiveSheet.SetRowHeight(-1, 24);


                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strOK = "OK";

                    if (chkResv.Checked == true)
                    {
                        if (dt.Rows[i]["RDate"].ToString().Trim().CompareTo(cpublic.strSysDate) > 1) strOK = "";
                    }

                    if (optDetail1.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) > 0 && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)) strOK = "";
                    }
                    else if (optDetail2.Checked == true)
                    {
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) >= 0) strOK = "";
                    }

                    if (strOK == "OK")
                    {

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.check01].Value = "";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Gubun].Value = "EXAM";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.BDate].Value = dt.Rows[i]["BDate"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Pano].Value = dt.Rows[i]["Pano"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.SName].Value = dt.Rows[i]["SName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Age].Value = dt.Rows[i]["Age"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Sex].Value = dt.Rows[i]["Sex"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DeptCode].Value = dt.Rows[i]["DeptCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrCode].Value = dt.Rows[i]["DrCode"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.DrName].Value = dt.Rows[i]["FC_DrName"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExCode].Value = dt.Rows[i]["MasterCode"].ToString().Trim();

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExName].Value = dt.Rows[i]["EXAMNAME"].ToString().Trim();


                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].Value = VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10);
                        if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) == cpublic.strSysDate)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 0].BackColor;
                        }
                        else if (VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10) != "" && VB.Left(dt.Rows[i]["RDate"].ToString().Trim(), 10).CompareTo(cpublic.strSysDate) < 0)
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.RDate].BackColor = ssColor_Sheet1.Cells[0, 1].BackColor;
                        }

                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.JepDate].Value = ReadSts3(dt.Rows[i]["CDate"].ToString().Trim());

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ExamGubun].Value = ReadSts3(dt.Rows[i]["GBJOB"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderNo].Value = dt.Rows[i]["OrderNo"].ToString().Trim();
                        if (dt.Rows[i]["Death"].ToString().Trim() == "Y")
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = "★사망됨! " + dt.Rows[i]["DRCOMMENT"].ToString().Trim();
                        }
                        else
                        {
                            spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.OrderRemark].Value = dt.Rows[i]["DRCOMMENT"].ToString().Trim();
                        }
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Table].Value = "EXAM_ORDER";
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ROWID].Value = dt.Rows[i]["ROWID"].ToString().Trim();
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.ChkName].Value = dt.Rows[i]["Part2"].ToString().Trim();

                        //spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = Read_NoExcuteRemark(dt.Rows[i]["Pano"].ToString().Trim(), "EXAM_ORDER", dt.Rows[i]["ROWID"].ToString().Trim());
                        spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmNoExecuteMain.Memo].Value = dt.Rows[i]["FC_NOEXE_MEMO"].ToString().Trim();

                        nRow++;
                    }

                }

                dt.Dispose();
                dt = null;

                #endregion

            }

        }

        delegate void threadProcessDelegate(bool b);
        void trunCircular(bool b)
        {
            //this.Progress.Visible = b;
            //this.Progress.IsRunning = b;
        }


        #endregion
    }
}
