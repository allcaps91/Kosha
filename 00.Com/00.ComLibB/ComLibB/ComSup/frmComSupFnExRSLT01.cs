using ComBase; //기본 클래스
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupFnExRSLT01.cs
    /// Description     : 류마티스 생체현미경(NVC) 검사 결과 등록 폼 -> 상단으로 이동
    /// Author          : 윤조연
    /// Create Date     : 2019-10-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// SFTP 
    /// 비트 컨트롤을 닷넷바 메트로 컨트롤 대체함
    /// </history>
    /// <seealso cref= "\ocs\nvc\Frm결과등록.frm(Frm결과등록) >> frmSupFnExRSLT03.cs >> frmComSupFnExRSLT01.cs  c#에서 폼 추가함" />
    public partial class frmComSupFnExRSLT01 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = null; //공용함수  
        ComFunc fun = null;
        clsSpread methodSpd = null;
        clsPrint cp = new clsPrint();

        DevComponents.DotNetBar.Metro.MetroTileItem[] Item1 = null; 

        string SQL = String.Empty;
        string gROWID = "";
        string gFiles = "";
        string gPacs = "";
        string gPano = "";
        string gAuth = "VIEW";

        string gTitle = "NVC Images";                
        string gPrintName = "";

        Int32 PrtX = 0;
        Int32 PrtY = 0;

        #region //단독실행 상단의 변수 
        string NVC_PATH = @"d:\interface\nvc\nvc_temp\";
        string NVC_PATHB = @"d:\interface\nvc\nvc_backup\";
        string NVC_PATH_VIEW = @"c:\cmc\nvc_temp\";
        string NVC_HOST = "/data/NVC/";
        int NVC_IMG_CNT = 10;
        #endregion

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

        public frmComSupFnExRSLT01()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupFnExRSLT01(string auth, string argPano)
        {
            InitializeComponent();            

            if (auth != "")
            {
                gAuth = auth;
            }
            gPano = argPano;
            setEvent();
        }

        public frmComSupFnExRSLT01(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;

            setEvent();
        }

        public frmComSupFnExRSLT01(MainFormMessage pform, string auth)
        {
            InitializeComponent();

            this.mCallForm = pform;

            if (auth != "")
            {
                gAuth = auth;
            }
            
            setEvent();
        }

        public frmComSupFnExRSLT01(MainFormMessage pform, string auth,string argPano)
        {
            InitializeComponent();

            this.mCallForm = pform;

            if (auth != "")
            {
                gAuth = auth;
            }
            gPano = argPano;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {

            dtpFDate.Text =  Convert.ToDateTime(cpublic.strSysDate).AddDays(-10).ToShortDateString();
            dtpTDate.Text = cpublic.strSysDate;

            setCombo();


            optJob0.Checked = true;

            lblFolder.Text = NVC_PATH;
            folder_sts_chk(NVC_PATH_VIEW);
            folder_sts_chk(NVC_PATH);

            if (gPano !="")
            {
                txtSearch.Text = ComFunc.SetAutoZero(gPano,ComNum.LENPTNO);

                screen_display();
            }


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
            this.btnSearch1.Click += new EventHandler(eBtnSearch);
            this.btnSearch2.Click += new EventHandler(eBtnSearch);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnClear.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnDelete.Click += new EventHandler(eBtnSave);
            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.btnDir.Click += new EventHandler(eBtnClick);
            this.btnBase.Click += new EventHandler(eBtnClick);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.optJob0.CheckedChanged += new EventHandler(eOptChecked);
            this.optJob1.CheckedChanged += new EventHandler(eOptChecked);
            this.optJob2.CheckedChanged += new EventHandler(eOptChecked);

            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.dtpFDate.TextChanged += eDtpTxtChange;            

        }

        void setAuth()
        {
            if (gAuth == "VIEW")
            {
                panBtn.Visible = false;
                btnPrint.Visible = false;
                panThum.Visible = false;
                this.Size = new Size(this.Width, this.Height - panThum.Height );

                btnPacs.Visible = false;
            }
            else if (gAuth == "MASTER")
            {
                panBtn.Visible = true;
                btnPrint.Visible = true;
                panThum.Visible = true;

                btnPacs.Visible = false;
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

                cpublic = new clsPublic(); //공용함수                        
                fun = new ComFunc();
                methodSpd = new clsSpread();

                setAuth();

                //시트
                sSpd_enmSupFnExRSLT03(ssList, sSpdenmSupFnExRSLT03, nSpdenmSupFnExRSLT03, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtl(NVC_IMG_CNT);

                screen_clear("1");

                setCtrlData();

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
                folder_sts_chk(NVC_PATH_VIEW);
                this.Close();
                return;
            }
            if (sender == this.btnCancel)
            {
                screen_clear("1");
            }
            else if (sender == this.btnClear)
            {
                screen_clear("2");
            }
            else if (sender == this.btnPacs)
            {
                #region //팍스뷰어
                if (txtPano.Text.Trim() != "")
                {
                    if (gPacs != "")
                    {
                        PACS_VIEW(txtPano.Text.Trim(), gPacs);
                    }
                    else
                    {
                        ComFunc.MsgBox("팍스영상이 없습니다..");
                    }
                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요!!");
                } 
                #endregion
            }
            else if (sender == this.btnDir)
            {
                folder_dialog();
            }
            else if (sender == this.btnBase)
            {
                setCtl_init();
            }

        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                screen_display();
            }
            else if (sender == this.btnSearch2)
            {
                if (gFiles != "")
                {
                    frmComSupFnExVIEW05 f = new frmComSupFnExVIEW05(gFiles);
                    f.ShowDialog();
                    f.Dispose();
                    f = null;
                    clsApi.FlushMemory();

                }
                else
                {
                    ComFunc.MsgBox("사진 이미지가 없습니다..", "확인");
                }
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                //저장
                if (gROWID != "")
                {
                    eSave(clsDB.DbCon, "S", gROWID);
                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요!!", "확인");
                }

            }
            else if (sender == this.btnDelete)
            {
                //삭제
                if (gROWID != "")
                {
                    eSave(clsDB.DbCon, "D", gROWID);
                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요!!", "확인");
                }
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //출력
                if (gROWID != "")
                {
                    gPrintName = "기본프린트";

                    if (groupBox8.Visible == true)
                    {
                        setNVC_clear(ssNvc);
                        if (setNVC(clsDB.DbCon, ssNvc, gROWID) == true)
                        {
                            SpreadPrint(this.ssNvc, false);
                        }
                    }
                    else if ( groupBox9.Visible == true)
                    {
                        setNVC_clear(ssNvc2);
                        if (setNVC(clsDB.DbCon, ssNvc2, gROWID) == true)
                        {
                            SpreadPrint(this.ssNvc2, false);
                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("대상을 선택후 작업하세요!!", "확인");
                }
            }
            
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eOptChecked(object sender, EventArgs e)
        {
            if (sender == this.optJob0 || sender == this.optJob1 || sender == this.optJob2)
            {
                screen_display();
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {

            if (e.ColumnHeader == true && e.RowHeader == true)
            {
                return;
            }

            FpSpread o = (FpSpread)sender;

            gROWID = "";
            gPacs = "";

            screen_clear("1");

            if (e.Row < 0 || e.Column < 0) return;
            

            if (sender == this.ssList)
            {
                gROWID = o.ActiveSheet.Cells[e.Row, (int)enmSupFnExRSLT03.ROWID].Text.Trim();                               
                
                //PACS일 경우
                if (o.ActiveSheet.Cells[e.Row, (int)enmSupFnExRSLT03.New].Text.Trim() == "PACS")
                {
                    gPacs = NVC_Pacsno(clsDB.DbCon, o.ActiveSheet.Cells[e.Row, (int)enmSupFnExRSLT03.Pano].Text.Trim(), o.ActiveSheet.Cells[e.Row, (int)enmSupFnExRSLT03.BDate].Text.Trim());
                    btnPacs.Visible = true;
                    btnSearch2.Visible = false;
                    panpic.Visible = false;

                    #region 2018-10-31 안정수 추가

                    groupBox8.Dock = DockStyle.None;
                    groupBox8.Visible = false;
                    groupBox9.Dock = DockStyle.Fill;
                    groupBox9.Visible = true;

                    #endregion
                }
                //기존이미지 방식일 경우
                else
                {
                    btnPacs.Visible = false;
                    btnSearch2.Visible = true;
                    panpic.Visible = true;

                    #region 2018-10-31 안정수 추가

                    groupBox9.Dock = DockStyle.None;
                    groupBox9.Visible = false;
                    groupBox8.Dock = DockStyle.Fill;
                    groupBox8.Visible = true;

                    #endregion
                }

                if (gROWID != "")
                {
                    read_one_data_display(clsDB.DbCon, gROWID);
                }

            }
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtPano.Text.Trim();
                    //if (strPano != "") read_pano_info(ComFunc.SetAutoZero(strPano, ComNum.LENPTNO), "1");
                }
            }


        }

        void eDtpTxtChange(object sender, EventArgs e)
        {
            //dtpTDate.Text = dtpFDate.Text.Trim();
        }

        void eSave(PsmhDb pDbCon, string argJob, string argROWID)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            //clsTrans DT = new clsTrans();
            //DT.Trans = clsDB.setBeginTran();
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (argJob == "S")
                {
                    #region  //류마티스 생체현미경검사 저장용 변수 class 초기화


                    c_Etc_ResultNVC c_Etc_ResultNVC = new c_Etc_ResultNVC();
                    c_Etc_ResultNVC.Pano = txtPano.Text.Trim();
                    c_Etc_ResultNVC.BDate = dtpOrdDate.Text.Trim();
                    c_Etc_ResultNVC.RDate = dtpExamDate.Text.Trim();

                    if (c_Etc_ResultNVC.BDate.CompareTo(c_Etc_ResultNVC.RDate) > 0)
                    {
                        ComFunc.MsgBox("검사일자 오류!!", "확인");
                        clsDB.setRollbackTran(pDbCon);
                        return;
                    }
                    c_Etc_ResultNVC.GbRaynaud = "";
                    if (optRay0.Checked == true)
                    {
                        c_Etc_ResultNVC.GbRaynaud = "Y";
                    }
                    else if (optRay0.Checked == true)
                    {
                        c_Etc_ResultNVC.GbRaynaud = "N";
                    }
                    if (txtOnset.Text.Trim() != "")
                    {
                        c_Etc_ResultNVC.nOnSet = Convert.ToInt16(txtOnset.Text.Trim());
                    }

                    c_Etc_ResultNVC.Diagnosis = ComFunc.QuotConv(txtDiag.Text.Trim());

                    c_Etc_ResultNVC.Part11 = "";
                    if (opt1R.Checked == true)
                    {
                        c_Etc_ResultNVC.Part11 = "R";
                    }
                    else if (opt1L.Checked == true)
                    {
                        c_Etc_ResultNVC.Part11 = "L";
                    }
                    else if (opt1RL.Checked == true)
                    {
                        c_Etc_ResultNVC.Part11 = "B";
                    }
                    c_Etc_ResultNVC.Part12 = VB.Left(cboFinger1.SelectedItem.ToString(), 1);


                    c_Etc_ResultNVC.Part21 = "";
                    if (opt2R.Checked == true)
                    {
                        c_Etc_ResultNVC.Part21 = "R";
                    }
                    else if (opt2L.Checked == true)
                    {
                        c_Etc_ResultNVC.Part21 = "L";
                    }
                    else if (opt2RL.Checked == true)
                    {
                        c_Etc_ResultNVC.Part21 = "B";
                    }
                    c_Etc_ResultNVC.Part22 = VB.Left(cboFinger2.SelectedItem.ToString(), 1);

                    c_Etc_ResultNVC.Part23 = "";
                    if (optPart0.Checked == true)
                    {
                        c_Etc_ResultNVC.Part23 = "1";
                    }
                    else if (optPart1.Checked == true)
                    {
                        c_Etc_ResultNVC.Part23 = "2";
                    }
                    else if (optPart2.Checked == true)
                    {
                        c_Etc_ResultNVC.Part23 = "3";
                    }

                    //Findings
                    if (groupBox8.Visible == true)
                    {
                        c_Etc_ResultNVC.Find11 = "";
                        if (optFind11.Checked == true)
                        {
                            c_Etc_ResultNVC.Find11 = "1";
                        }
                        else if (optFind12.Checked == true)
                        {
                            c_Etc_ResultNVC.Find11 = "2";
                        }

                        c_Etc_ResultNVC.Find12 = VB.Left(cboFind1.SelectedItem.ToString(), 1);
                        c_Etc_ResultNVC.Find2 = VB.Left(cboFind2.SelectedItem.ToString(), 1);
                        c_Etc_ResultNVC.Find3 = VB.Left(cboFind3.SelectedItem.ToString(), 1);
                        c_Etc_ResultNVC.Find4 = VB.Left(cboFind4.SelectedItem.ToString(), 1);
                        c_Etc_ResultNVC.Find5 = VB.Left(cboFind5.SelectedItem.ToString(), 1);
                        c_Etc_ResultNVC.Find6 = VB.Left(cboFind6.SelectedItem.ToString(), 1);
                    }
                    else if (groupBox9.Visible == true)
                    {
                        if(optF1_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find11 = "0";
                        }
                        else if(optF1_1.Checked == true)                                                
                        {
                            c_Etc_ResultNVC.Find11 = "1";
                        }

                        if (optF2_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find2 = "0";
                        }
                        else if (optF2_1.Checked == true)
                        {
                            c_Etc_ResultNVC.Find2 = "1";
                        }

                        if (optF3_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find3 = "0";
                        }
                        else if (optF3_1.Checked == true)
                        {
                            c_Etc_ResultNVC.Find3 = "1";
                        }

                        if (optF4_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find4 = "0";
                        }
                        else if (optF4_1.Checked == true)
                        {
                            c_Etc_ResultNVC.Find4 = "1";
                        }

                        if (optF5_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find5 = "0";
                        }
                        else if (optF5_1.Checked == true)
                        {
                            c_Etc_ResultNVC.Find5 = "1";
                        }

                        if (optF6_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find6 = "0";
                        }
                        else if (optF6_1.Checked == true)
                        {
                            c_Etc_ResultNVC.Find6 = "1";
                        }

                        if (optF7_0.Checked == true)
                        {
                            c_Etc_ResultNVC.Find7 = "0";
                        }
                        else if (optF7_1.Checked == true)
                        {
                            c_Etc_ResultNVC.Find7 = "1";
                        }
                    }

                    c_Etc_ResultNVC.Conclusions = ComFunc.QuotConv(txtConclusions.Text.Trim());

                    c_Etc_ResultNVC.ROWID = argROWID;

                    #endregion

                    DataTable dt = sel_ETC_RESULT_NVC(pDbCon, "", "", "", c_Etc_ResultNVC.ROWID);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        c_Etc_ResultNVC.ResultTime = dt.Rows[0]["ResultTime"].ToString().Trim();
                    }
                    else
                    {
                        c_Etc_ResultNVC.ResultTime = "";
                    }

                    SqlErr = up_ETC_RESULT_NVC(pDbCon, c_Etc_ResultNVC, ref intRowAffected);

                    #region NVC 파일 FTP 전송

                    string gFilePath = NVC_PATH;

                    string strYYMM = VB.Left(dtpExamDate.Text.Trim().Replace("-", ""), 6);
                    string strHost = NVC_HOST + strYYMM + "/";  //ftp 저장경로
                    string strFile = "";
                    string strFiles = "";
                    string strHostFile = "";
                    string strFileNames = "";



                    System.IO.DirectoryInfo dic = new System.IO.DirectoryInfo(NVC_PATH);
                    System.IO.FileInfo[] fi = dic.GetFiles("*.jpg");

                    if (fi.Length != 0)
                    {
                        Ftpedt FtpedtX = new Ftpedt(); //FTP

                        for (int i = 0; i < fi.Length; i++)
                        {
                            strFile = fi[i].Name.ToString();
                            strFiles = strHost + strFile;
                            strHostFile = strHost + ComQuery.GetSequencesNo(pDbCon, "KOSMOS_OCS", "SEQ_ETC_RESULT_NVC").ToString() + ".jpg";
                            strFileNames += strHostFile + "|";

                            FtpedtX.FtpUpload("192.168.100.31", "oracle", "oracle", gFilePath + strFile, strHostFile, strHost); //TODO 윤조연 FTP 계정 정리


                            #region //정상처리된 원본파일 백업 및 삭제 작업

                            File.Copy(NVC_PATH + strFile, NVC_PATHB + strFile);
                            File.Delete(NVC_PATH + strFile);

                            #endregion

                        }

                        FtpedtX = null;

                        c_Etc_ResultNVC.FileName = strFileNames;

                        SqlErr = up_ETC_RESULT_NVC_FTP_OK(pDbCon, c_Etc_ResultNVC, ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }

                    }
                    else
                    {


                    }


                    #endregion


                }
                else if (argJob == "D")
                {
                    //삭제
                    SqlErr = del_ETC_RESULT_NVC(pDbCon, argROWID, ref intRowAffected);
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


            screen_clear();
            screen_display();

        }

        void ePicDClick(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Metro.MetroTileItem p = (DevComponents.DotNetBar.Metro.MetroTileItem)sender;

            if (p.Text == "")
            {
                p.Text = p.Name;
                p.TileSize = new Size(120, 100);
            }
            else
            {
                p.Text = "";
                p.TileSize = new Size(220, 160);
            }

            metroTilePan1.Refresh();
            panpic.Refresh();

        }

        void setCombo()
        {
            #region COMBO          

            cboFinger1.Items.Clear();
            cboFinger1.Items.Add("");
            cboFinger1.Items.Add("4");
            cboFinger1.Items.Add("5");
            cboFinger1.Items.Add("B");
            cboFinger1.SelectedIndex = 0;

            cboFinger2.Items.Clear();
            cboFinger2.Items.Add("");
            cboFinger2.Items.Add("4");
            cboFinger2.Items.Add("5");
            cboFinger2.Items.Add("B");
            cboFinger2.SelectedIndex = 0;

            cboFind1.Items.Clear();
            cboFind1.Items.Add("");
            cboFind1.Items.Add("1.None");
            cboFind1.Items.Add("2.A few");
            cboFind1.Items.Add("3.Moderate");
            cboFind1.Items.Add("4.Frequent");
            cboFind1.SelectedIndex = 0;

            cboFind2.Items.Clear();
            cboFind2.Items.Add("");
            cboFind2.Items.Add("1.Well preserved");
            cboFind2.Items.Add("2.mild disorganization");
            cboFind2.Items.Add("3.moderate disorganization");
            cboFind2.Items.Add("4.severe disorganization");
            cboFind2.SelectedIndex = 0;

            cboFind3.Items.Clear();
            cboFind3.Items.Add("");
            cboFind3.Items.Add("1.None");
            cboFind3.Items.Add("2.Mild");
            cboFind3.Items.Add("3.Moderate");
            cboFind3.Items.Add("4.Severe");
            cboFind3.SelectedIndex = 0;

            cboFind4.Items.Clear();
            cboFind4.Items.Add("");
            cboFind4.Items.Add("1.None");
            cboFind4.Items.Add("2.Some");
            cboFind4.Items.Add("3.Moderate");
            cboFind4.Items.Add("4.Extensive");
            cboFind4.SelectedIndex = 0;

            cboFind5.Items.Clear();
            cboFind5.Items.Add("");
            cboFind5.Items.Add("1.None");
            cboFind5.Items.Add("2.A few");
            cboFind5.Items.Add("3.Moderate");
            cboFind5.Items.Add("4.Frequent");
            cboFind5.SelectedIndex = 0;

            cboFind6.Items.Clear();
            cboFind6.Items.Add("");
            cboFind6.Items.Add("1.None");
            cboFind6.Items.Add("2.A few");
            cboFind6.Items.Add("3.Moderate");
            cboFind6.Items.Add("4.Frequent");
            cboFind6.SelectedIndex = 0;

            #endregion

        }

        void screen_clear(string Job = "")
        {
            read_sysdate();


            if (Job =="" || Job !="2")
            {
                gROWID = "";
                gPacs = "";
                gFiles = "";

                itemContainer1.TitleText = gTitle;

                //clear
                for (int i = 0; i < Item1.Length; i++)
                {
                    Item1[i].TileStyle.BackgroundImage = null;
                    Item1[i].Text = "";
                }

                //Finding부분 클리어
                optF1_0.Checked = false;
                optF1_1.Checked = false;
                optF2_0.Checked = false;
                optF2_1.Checked = false;
                optF3_0.Checked = false;
                optF3_1.Checked = false;
                optF4_0.Checked = false;
                optF4_1.Checked = false;
                optF5_0.Checked = false;
                optF5_1.Checked = false;
                optF6_0.Checked = false;
                optF6_1.Checked = false;
                optF7_0.Checked = false;
                optF7_1.Checked = false;
            }            

            if (Job == "1")
            {

                txtPano.Text = "";
                txtSName.Text = "";
                txtAge.Text = "";

                dtpExamDate.Text = "";
                dtpOrdDate.Text = "";


                optRay0.Checked = false;
                optRay1.Checked = false;
                txtOnset.Text = "";
                txtDiag.Text = "";

                lblLDate.Text = "";

                opt1R.Checked = false;
                opt1L.Checked = false;
                opt1RL.Checked = false;
                cboFinger1.Text = "";

                opt2R.Checked = false;
                opt2L.Checked = false;
                opt2RL.Checked = false;
                cboFinger2.Text = "";

                optPart0.Checked = false;
                optPart1.Checked = false;
                optPart2.Checked = false;

                optFind11.Checked = false;
                optFind12.Checked = false;
                cboFind1.Text = "";
                cboFind2.Text = "";
                cboFind3.Text = "";
                cboFind4.Text = "";
                cboFind5.Text = "";
                cboFind6.Text = "";

                txtConclusions.Text = "";

                #region //사용안함

                //Control[] controls = ComFunc.GetAllControls(this);

                //foreach (Control ctl in controls)
                //{
                //    if (ctl is RadioButton)
                //    {
                //        if (((RadioButton)ctl).Name.Contains("optJob") == false)
                //        {
                //            ((RadioButton)ctl).Checked = false;
                //        }

                //    }
                //    else if (ctl is TextBox)
                //    {
                //        ((TextBox)ctl).Text = "";

                //    }
                //    else if (ctl is ComboBox)
                //    {
                //        ((ComboBox)ctl).Text = "";

                //    }
                //    else if (ctl is Label)
                //    {
                //        if (((Label)ctl).Name == "lblLDate")
                //        {
                //            ((Label)ctl).Text = "";
                //        }
                //    }
                //    else if (ctl is DateTimePicker)
                //    {
                //        if (((DateTimePicker)ctl).Name != "dtpFDate" && ((DateTimePicker)ctl).Name != "dtpTDate")
                //        {
                //            ((DateTimePicker)ctl).Text = "";
                //        }

                //    }
                //} 
                #endregion
            }
            else if(Job == "2")
            {
                optRay0.Checked = false;
                optRay1.Checked = false;

                opt1R.Checked = false;
                opt1L.Checked = false;
                opt1RL.Checked = false;
                cboFinger1.Text = "";

                opt2R.Checked = false;
                opt2L.Checked = false;
                opt2RL.Checked = false;
                cboFinger2.Text = "";

                optPart0.Checked = false;
                optPart1.Checked = false;
                optPart2.Checked = false;

                optFind11.Checked = false;
                optFind12.Checked = false;
                cboFind1.Text = "";
                cboFind2.Text = "";
                cboFind3.Text = "";
                cboFind4.Text = "";
                cboFind5.Text = "";
                cboFind6.Text = "";

            }


        }

        void setCtl_init()
        {
            opt1R.Checked = true;
            cboFinger1.SelectedIndex = 1;
            cboFind1.SelectedIndex = 1;
            cboFind2.SelectedIndex = 1;
            cboFind3.SelectedIndex = 1;
            cboFind4.SelectedIndex = 1;
            cboFind5.SelectedIndex = 1;
            cboFind6.SelectedIndex = 1;

        }

        void setCtl(int cnt)
        {
            if (Item1 != null)
            {
                itemContainer1.SubItems.RemoveRange(Item1);
                itemContainer1.SubItems.Clear();
                Item1 = null;
            }

            Item1 = new DevComponents.DotNetBar.Metro.MetroTileItem[cnt];

            for (int i = 0; i < cnt; i++)
            {
                Item1[i] = new DevComponents.DotNetBar.Metro.MetroTileItem();
                Item1[i].ImageTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
                Item1[i].Name = "";
                Item1[i].SymbolColor = System.Drawing.Color.Empty;
                Item1[i].TileColor = DevComponents.DotNetBar.Metro.eMetroTileColor.Default;
                Item1[i].TileSize = new System.Drawing.Size(120, 100);
                Item1[i].DoubleClick += new EventHandler(ePicDClick);
                Item1[i].Visible = true;

            }

            itemContainer1.SubItems.AddRange(Item1);


        }

        void setCtl_Visible(int cnt,int cnt2)
        {
            if (Item1 == null)
            {
                return;
            }
            
            for (int i = 0; i < cnt; i++)
            {     
                Item1[i].Visible = false;
            }

            for (int i = 0; i < cnt2; i++)
            {
                Item1[i].Visible = true;
            }

        }

        void folder_dialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = NVC_PATH;
            dialog.ShowDialog();
            lblFolder.Text = dialog.SelectedPath;
        }

        void foder_img_display(string argFolder)
        {

            string strFile = "";

            //tv1.ClearPage();

            System.IO.DirectoryInfo dic = new System.IO.DirectoryInfo(argFolder);
            System.IO.FileInfo[] fi = dic.GetFiles("*.jpg");

            if (fi.Length != 0)
            {

                for (int i = 0; i < fi.Length; i++)
                {
                    strFile = fi[i].Name.ToString();

                    //tv1.AppendPage(argFolder + strFile, 1, 1);

                }
            }

            //tv1.ThumbGen = true;
            //tv1.Redraw = true;

        }

        void read_one_data_display(PsmhDb pDbCon, string argROWID)
        {
            string strPano = "";
            string strBDate = "";
            string strDeptCode = "";
            string strIO = "";

            //tv1.ClearPage();

            DataTable dt = sel_ETC_RESULT_NVC(pDbCon, "", "", "", "", argROWID);


            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                #region 데이타 읽어 값 표시

                strIO = dt.Rows[0]["GbIO"].ToString().Trim();
                strPano = dt.Rows[0]["Ptno"].ToString().Trim();
                strBDate = dt.Rows[0]["BDate2"].ToString().Trim();
                strDeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();

                txtPano.Text = strPano;
                txtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                txtAge.Text = dt.Rows[0]["Age"].ToString().Trim() + "/" + dt.Rows[0]["Sex"].ToString().Trim();
                if (dt.Rows[0]["RDate"].ToString().Trim() != "")
                {
                    dtpExamDate.Text = dt.Rows[0]["RDate"].ToString().Trim();
                }
                else
                {
                    dtpExamDate.Text = strBDate;
                }

                dtpOrdDate.Text = strBDate;

                if (dt.Rows[0]["GbRaynaud"].ToString().Trim() == "Y")
                {
                    optRay0.Checked = true;
                }
                
                txtOnset.Text = dt.Rows[0]["Onset"].ToString().Trim();
                txtDiag.Text = dt.Rows[0]["Diagnosis"].ToString().Trim();

                //검사부위1
                if (dt.Rows[0]["Part11"].ToString().Trim() == "R")
                {
                    opt1R.Checked = true;
                }
                else if (dt.Rows[0]["Part11"].ToString().Trim() == "L")
                {
                    opt1L.Checked = true;
                }
                else if (dt.Rows[0]["Part11"].ToString().Trim() == "B")
                {
                    opt1RL.Checked = true;
                }

                if (dt.Rows[0]["Part12"].ToString().Trim() == "4")
                {
                    cboFinger1.SelectedIndex = 1;
                }
                else if (dt.Rows[0]["Part12"].ToString().Trim() == "5")
                {
                    cboFinger1.SelectedIndex = 2;
                }
                else if (dt.Rows[0]["Part12"].ToString().Trim() == "B")
                {
                    cboFinger1.SelectedIndex = 3;
                }
                else
                {
                    cboFinger1.SelectedIndex = 0;
                }

                //검사부위2
                if (dt.Rows[0]["Part21"].ToString().Trim() == "R")
                {
                    opt2R.Checked = true;
                }
                else if (dt.Rows[0]["Part21"].ToString().Trim() == "L")
                {
                    opt2L.Checked = true;
                }
                else if (dt.Rows[0]["Part21"].ToString().Trim() == "B")
                {
                    opt2RL.Checked = true;
                }

                if (dt.Rows[0]["Part22"].ToString().Trim() == "4")
                {
                    cboFinger2.SelectedIndex = 1;
                }
                else if (dt.Rows[0]["Part22"].ToString().Trim() == "5")
                {
                    cboFinger2.SelectedIndex = 2;
                }
                else if (dt.Rows[0]["Part22"].ToString().Trim() == "B")
                {
                    cboFinger2.SelectedIndex = 3;
                }
                else
                {
                    cboFinger2.SelectedIndex = 0;
                }

                if (dt.Rows[0]["Part23"].ToString().Trim() == "1")
                {
                    optPart0.Checked = true;
                }
                else if (dt.Rows[0]["Part23"].ToString().Trim() == "2")
                {
                    optPart1.Checked = true;
                }
                else if (dt.Rows[0]["Part23"].ToString().Trim() == "3")
                {
                    optPart2.Checked = true;
                }

                //Findings
                //groupBox8 기존(IMG), groupBox9 신규(PACS)
                if (groupBox8.Visible == true)
                {
                    if (dt.Rows[0]["Findings11"].ToString().Trim() == "1")
                    {
                        optFind11.Checked = true;
                    }
                    else if (dt.Rows[0]["Findings11"].ToString().Trim() == "2")
                    {
                        optFind12.Checked = true;
                    }

                    if (optJob0.Checked != true)
                    {
                        if (dt.Rows[0]["Findings12"].ToString() != "")
                        {
                            cboFind1.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Findings12"].ToString());
                            cboFind1.Refresh();
                        }
                        if (dt.Rows[0]["Findings2"].ToString() != "")
                        {
                            cboFind2.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Findings2"].ToString());
                            cboFind2.Refresh();
                        }
                        if (dt.Rows[0]["Findings3"].ToString() != "")
                        {
                            cboFind3.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Findings3"].ToString());
                        }
                        if (dt.Rows[0]["Findings4"].ToString() != "")
                        {
                            cboFind4.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Findings4"].ToString());
                        }
                        if (dt.Rows[0]["Findings5"].ToString() != "")
                        {
                            cboFind5.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Findings5"].ToString());
                        }
                        if (dt.Rows[0]["Findings6"].ToString() != "")
                        {
                            cboFind6.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Findings6"].ToString());
                        }

                    }
                }
                else if (groupBox9.Visible == true)
                {
                    if (dt.Rows[0]["Findings11"].ToString().Trim() == "0")
                    {
                        optF1_0.Checked = true;
                    }
                    else if (dt.Rows[0]["Findings11"].ToString().Trim() == "1")
                    {
                        optF1_1.Checked = true;
                    }

                    if (optJob0.Checked != true)
                    {                        
                        if (dt.Rows[0]["Findings2"].ToString() == "0")
                        {
                            optF2_0.Checked = true;
                        }
                        else if (dt.Rows[0]["Findings2"].ToString().Trim() == "1")
                        {
                            optF2_1.Checked = true;
                        }

                        if (dt.Rows[0]["Findings3"].ToString() == "0")
                        {
                            optF3_0.Checked = true;
                        }
                        else if (dt.Rows[0]["Findings3"].ToString().Trim() == "1")
                        {
                            optF3_1.Checked = true;
                        }

                        if (dt.Rows[0]["Findings4"].ToString() == "0")
                        {
                            optF4_0.Checked = true;
                        }
                        else if (dt.Rows[0]["Findings4"].ToString().Trim() == "1")
                        {
                            optF4_1.Checked = true;
                        }

                        if (dt.Rows[0]["Findings5"].ToString() == "0")
                        {
                            optF5_0.Checked = true;
                        }
                        else if (dt.Rows[0]["Findings5"].ToString().Trim() == "1")
                        {
                            optF5_1.Checked = true;
                        }

                        if (dt.Rows[0]["Findings6"].ToString() == "0")
                        {
                            optF6_0.Checked = true;
                        }
                        else if (dt.Rows[0]["Findings6"].ToString().Trim() == "1")
                        {
                            optF6_1.Checked = true;
                        }

                        if (dt.Rows[0]["Findings7"].ToString() == "0")
                        {
                            optF7_0.Checked = true;
                        }
                        else if (dt.Rows[0]["Findings7"].ToString().Trim() == "1")
                        {
                            optF7_1.Checked = true;
                        }
                    }
                }
                txtConclusions.Text = dt.Rows[0]["Conclusions"].ToString().Trim();

                //미등록상태이면
                if (optJob0.Checked == true)
                {
                    setCtl_init();
                    if (dtpExamDate.Text.Trim().CompareTo("2005-12-15") >= 0)
                    {
                        //foder_img_display(clsSupFnEx.NVC_PATH);
                        itemContainer1.TitleText = gTitle + "(" + folder_display_img_file_dotnetbar(Item1, NVC_PATH) + ")";

                    }

                }
                else
                {
                    gFiles = dt.Rows[0]["FileName"].ToString();
                    if (gFiles != "")
                    {
                        itemContainer1.TitleText = gTitle + "(" + display_nvc_img_file_dotnetbar(Item1, NVC_PATH_VIEW, gFiles, NVC_HOST) + ")";
                    }
                }

                itemContainer1.Refresh();

                #endregion
                                          
                #region //최종검사일 체크
                DataTable dt2 = sel_ETC_RESULT_NVC(pDbCon, "2", strPano, strBDate, "", "");
                if (dt2 == null) return;

                if (dt2.Rows.Count > 0)
                {
                    if (dt2.Rows[0]["RDate"].ToString().Trim() != "")
                    {
                        lblLDate.Text = dt2.Rows[0]["RDate"].ToString().Trim();
                    }
                    else
                    {
                        lblLDate.Text = dt2.Rows[0]["BDate2"].ToString().Trim() + "(미검사)";
                    }
                    lblLDate.Refresh();
                }
                dt2.Dispose();
                dt2 = null; 
                #endregion
            }

            dt.Dispose();
            dt = null;


        }
                
        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, txtSearch.Text.Trim(), dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSearch, string argFDate, string argTDate)
        {
            int i = 0;
            DataTable dt = null;
            string strJob = "";

            if (optJob0.Checked == true)
            {
                strJob = "0";
            }
            else if (optJob1.Checked == true)
            {
                strJob = "1";
            }
            else if (optJob2.Checked == true)
            {
                strJob = "2";
            }

            read_sysdate();

            screen_clear();

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            dt = sel_ETC_RESULT_NVC(pDbCon, strJob, argSearch, argFDate, argTDate);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {

                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.OrdDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.Age].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    if (dt.Rows[i]["New"].ToString().Trim()=="Y")
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.New].Text = "PACS";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.New].Text = "IMG";
                    }
                    Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.BDate].Text = dt.Rows[i]["BDate2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)enmSupFnExRSLT03.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            #endregion


        }

        #region //류마티스 현미경검사 단독실행관련
        
        enum enmSupFnExRSLT03
        {
            OrdDate, Pano, SName, Age, New,BDate, ROWID
        }
        
        string[] sSpdenmSupFnExRSLT03 = { "오더일자", "등록번호", "성명", "나이", "구분","처방일자", "ROWID" };
                
        int[] nSpdenmSupFnExRSLT03 = { 80, 80, 80, 50, 40,80, 60 };
        
        
        void sSpd_enmSupFnExRSLT03(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupFnExRSLT03)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmSupFnExRSLT03.BDate, clsSpread.enmSpdType.Text);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupFnExRSLT03.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든            
            methodSpd.setColStyle(spd, -1, (int)enmSupFnExRSLT03.BDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupFnExRSLT03.ROWID, clsSpread.enmSpdType.Hide);


        }

        void folder_sts_chk(string folderpath, string argExe = "*.jpg")
        {
            //디렉토리 체크후 없으면 생성 있으면 폴더내 파일 삭제후 작업
            DirectoryInfo di = new DirectoryInfo(folderpath);
            if (di.Exists == false)
            {
                di.Create();
            }
            else
            {

                System.IO.FileInfo[] files = di.GetFiles(argExe, SearchOption.AllDirectories);

                foreach (System.IO.FileInfo file in files)
                {
                    //file.Attributes = FileAttributes.Normal; 
                    file.Delete();
                }

                //Directory.Delete(folderpath, true);

            }
        }

        int display_nvc_img_file_dotnetbar(DevComponents.DotNetBar.Metro.MetroTileItem[] Item1, string folderpath, string argFileName, string argNVC_HOST, bool bDown = false)
        {
            string[] sFile = argFileName.Split('|');


            #region 폴더유무 체크 및 생성, 폴더내 파일 삭제...
            if (bDown)
            {
                folder_sts_chk(folderpath);
            }

            #endregion


            //이미지 뷰
            string strFile = "";
            string strYYMM = setP(setP(sFile[0].Trim(), argNVC_HOST, 2), "/", 1);
            string strHost = argNVC_HOST + strYYMM + "/"; //ftp 저장경로


            Ftpedt FtpedtX = new Ftpedt(); //FTP
            FileStream fs = null;


            setCtl_Visible(NVC_IMG_CNT,sFile.Length-1);

            //clear
            for (int i = 0; i < Item1.Length; i++)
            {
                Item1[i].TileStyle.BackgroundImage = null;
                Item1[i].Text = "";
            }

            for (int i = 0; i < sFile.Length - 1; i++)
            {
                strFile = setP(sFile[i], "/", 5).Trim();

                FtpedtX.FtpDownload("192.168.100.31", "oracle", "oracle", folderpath + strFile, strHost + strFile, strHost); //TODO 윤조연 FTP 계정 정리

                fs = new System.IO.FileStream(folderpath + strFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                Item1[i].TileStyle.BackgroundImage = System.Drawing.Image.FromStream(fs);
                //Item1.TileStyle.BackgroundImage = Image.FromFile(folderpath + strFile);
                Item1[i].TileStyle.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.Stretch;
                Item1[i].TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                Item1[i].Text = (i + 1).ToString() + "." + strFile;
                Item1[i].Name = (i + 1).ToString() + "." + strFile;

                fs.Close();

            }

            FtpedtX = null;

            return sFile.Length - 1;

        }
        
        int folder_display_img_file_dotnetbar(DevComponents.DotNetBar.Metro.MetroTileItem[] Item1, string argFolder)
        {
            string strFile = "";

            //clear
            for (int i = 0; i < Item1.Length; i++)
            {
                Item1[i].TileStyle.BackgroundImage = null;
                Item1[i].Text = "";
            }

            FileStream fs = null;

            DirectoryInfo di = new System.IO.DirectoryInfo(argFolder);
            System.IO.FileInfo[] fi = di.GetFiles("*.jpg");

            if (fi.Length != 0)
            {

                for (int i = 0; i < fi.Length; i++)
                {
                    strFile = fi[i].Name.ToString();
                    fs = new System.IO.FileStream(argFolder + strFile, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    Item1[i].TileStyle.BackgroundImage = System.Drawing.Image.FromStream(fs);
                    Item1[i].TileStyle.BackgroundImagePosition = DevComponents.DotNetBar.eStyleBackgroundImage.Stretch;
                    Item1[i].TileStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
                    Item1[i].Text = (i + 1).ToString() + "." + strFile;
                    Item1[i].Name = (i + 1).ToString() + "." + strFile;

                    fs.Close();

                }
            }

            return fi.Length;
        }

        static int setL(string str, string ch)
        {
            string[] c = VB.Split(str, ch);

            try
            {
                return c.Length;
            }
            catch
            {
                return 0;
            }

        }

        string setP(string str, string ch, int n)
        {
            string[] c = VB.Split(str, ch);

            if (c.Length == 0 || c.Length < n) return "";

            try
            {
                return c[n - 1];
            }
            catch
            {
                return "";
            }

        }

        string NVC_Pacsno(PsmhDb pDbCon, string argPano, string argBDate)
        {
            string s = string.Empty;

            DataTable dt = sel_XRAY_DETAIL(pDbCon, argPano, argBDate);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                //s = dt.Rows[0]["PacsStudyId"].ToString().Trim();
                s = dt.Rows[0]["PacsNo"].ToString().Trim();
            }            
            dt.Dispose();
            dt = null;


            return s;
        }

        DataTable sel_ETC_RESULT_NVC(PsmhDb pDbCon, string Job, string argSearch, string argFDate, string argTDate, string argROWID = "")
        {
            DataTable dt = null;
            string SqlErr = "";

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += "    TO_CHAR(BDate,'YYYYMMDD') BDate,Ptno,SName,Age,Sex              \r\n";
            SQL += "    ,TO_CHAR(BDate,'YYYY-MM-DD') BDate2                             \r\n";
            SQL += "    ,TO_CHAR(RDate,'YYYY-MM-DD') RDate                              \r\n";
            SQL += "    ,TO_CHAR(ResultTime,'YYYY-MM-DD HH24:MI') ResultTime            \r\n";
            SQL += "    ,DeptCode,DrCode,GbIO,GbRaynaud,OnSet,Diagnosis                 \r\n";
            SQL += "    ,Part11,Part12,Part21,Part22,Part23                             \r\n";
            SQL += "    ,Findings11,Findings12,Findings2,Findings3,Findings4,Findings5  \r\n";
            SQL += "    ,Findings6,Findings7                                            \r\n";
            SQL += "    ,Conclusions,GbSend,FileName,New                                \r\n";
            SQL += "    ,ROWID                                                          \r\n";

            SQL += "  FROM " + ComNum.DB_MED + "ETC_RESULT_NVC                          \r\n";
            SQL += "   WHERE 1 = 1                                                      \r\n";
            if (Job !="2")
            {
                if (Job == "0")
                {
                    SQL += "     AND RDate IS NULL                                          \r\n";
                }
                else if (Job == "1")
                {
                    SQL += "     AND RDate IS NOT NULL                                      \r\n";
                }
                if (argROWID != "")
                    SQL += "     AND ROWID ='" + argROWID + "'                              \r\n";
                else if (argSearch != "")
                {
                    SQL += "     AND ( PTNO ='" + argSearch + "'                             \r\n";
                    SQL += "         OR SNAME LIKE '%" + argSearch + "%' )                  \r\n";
                }
                else
                {
                    SQL += "     AND BDate >=TO_DATE('" + argFDate + "','YYYY-MM-DD')       \r\n";
                    SQL += "     AND BDate <=TO_DATE('" + argTDate + "','YYYY-MM-DD')       \r\n";
                }
                SQL += "  ORDER BY RDate,Ptno,SName                                         \r\n";
            }
            else
            {
                SQL += "     AND PTNO ='" + argSearch + "'                             \r\n";
                SQL += "     AND BDate <TO_DATE('" + argFDate + "','YYYY-MM-DD')       \r\n";
                SQL += "  ORDER BY BDate DESC                                         \r\n";
            }

            
            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        DataTable sel_XRAY_DETAIL(PsmhDb pDbCon, string argPano, string argBDate)
        {
            DataTable dt = null;
            string SqlErr = "";

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "    PacsNo,PacsStudyId                                      \r\n";
            SQL += "    ,ROWID                                                  \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL                    \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            SQL += "    AND Pano = '" + argPano + "'                           \r\n";
            SQL += "    AND BDate = TO_DATE('" + argBDate + "','YYYY-MM-DD')    \r\n";
            SQL += "    AND XCode = 'E7190'                                     \r\n";
            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
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
        /// 류마티스 현미경검사 결과 등록 테이블 저장
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        string up_ETC_RESULT_NVC(PsmhDb pDbCon, c_Etc_ResultNVC argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ETC_RESULT_NVC   SET           \r\n";

            SQL += "     GbSTS = '1'                                            \r\n";
            SQL += "     ,RDate =TO_DATE('" + argCls.RDate + "','YYYY-MM-DD')   \r\n";
            if (argCls.ResultTime == "")
            {
                SQL += "     ,ResultTime =SYSDATE                               \r\n";
            }
            SQL += "     ,GbRaynaud = '" + argCls.GbRaynaud + "'                \r\n";
            SQL += "     ,Onset = " + argCls.nOnSet + "                         \r\n";
            SQL += "     ,Diagnosis = '" + argCls.Diagnosis + "'                \r\n";
            SQL += "     ,Part11 = '" + argCls.Part11 + "'                      \r\n";
            SQL += "     ,Part12 = '" + argCls.Part12 + "'                      \r\n";
            SQL += "     ,Part21 = '" + argCls.Part21 + "'                      \r\n";
            SQL += "     ,Part22 = '" + argCls.Part22 + "'                      \r\n";
            SQL += "     ,Part23 = '" + argCls.Part23 + "'                      \r\n";
            SQL += "     ,Findings11 = '" + argCls.Find11 + "'                  \r\n";
            SQL += "     ,Findings12 = '" + argCls.Find12 + "'                  \r\n";
            SQL += "     ,Findings2 = '" + argCls.Find2 + "'                    \r\n";
            SQL += "     ,Findings3 = '" + argCls.Find3 + "'                    \r\n";
            SQL += "     ,Findings4 = '" + argCls.Find4 + "'                    \r\n";
            SQL += "     ,Findings5 = '" + argCls.Find5 + "'                    \r\n";
            SQL += "     ,Findings6 = '" + argCls.Find6 + "'                    \r\n";
            if(argCls.Find7 != "")
            {
                SQL += "     ,Findings7 = '" + argCls.Find7 + "'                \r\n";
            }
            SQL += "     ,Conclusions = '" + argCls.Conclusions + "'            \r\n";
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 류마티스 현미경검사 ftp전송 완료시 갱신 
        /// </summary>
        /// <param name="argCls"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        string up_ETC_RESULT_NVC_FTP_OK(PsmhDb pDbCon, c_Etc_ResultNVC argCls, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "ETC_RESULT_NVC   SET           \r\n";
            SQL += "     ResultTime =SYSDATE                                    \r\n";
            SQL += "     ,FileName = '" + argCls.FileName + "'                  \r\n";
            SQL += "     ,GbSend = 'Y'                                          \r\n";
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "  AND ROWID ='" + argCls.ROWID + "'                         \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }
        string del_ETC_RESULT_NVC(PsmhDb pDbCon, string argROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " DELETE FROM " + ComNum.DB_MED + "ETC_RESULT_NVC            \r\n";
            SQL += " WHERE 1=1                                                  \r\n";
            SQL += "  AND ROWID ='" + argROWID + "'                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// 류마티스 현미경 검사결과등록 관련 데이타 변수관련 class
        /// </summary>
        class c_Etc_ResultNVC
        {
            public string Pano = "";
            public string sName = "";
            public string BDate = "";
            public string RDate = "";
            public string STS = "";
            public string ResultTime = "";
            public string GbSend = "";
            public string FileName = "";
            public string GbRaynaud = "";
            public int nOnSet = 0;
            public string Diagnosis = "";

            public string Part11 = "";
            public string Part12 = "";
            public string Part21 = "";
            public string Part22 = "";
            public string Part23 = "";
            public string Find11 = "";
            public string Find12 = "";
            public string Find2 = "";
            public string Find3 = "";
            public string Find4 = "";
            public string Find5 = "";
            public string Find6 = "";
            public string Find7 = "";
            public string Conclusions = "";
            public string ROWID = "";


        }

        void PACS_VIEW(string argPano, string argUid)
        {
            string str = "";
            string strUid = argUid;

            if (strUid != "")
            {
                str += "/h " + strUid + " /hp " + argPano + " /u " + clsType.User.IdNumber + "@" + clsSHA.SHA256(clsType.User.PasswordChar);
            }
            else
            {
                str = "/hp " + argPano + " /u " + clsType.User.IdNumber + "@" + clsSHA.SHA256(clsType.User.PasswordChar);
            }

            if (clsPacs.ChkPacsLogin(clsDB.DbCon, clsType.User.Sabun, "MVIEW") == false)
            {
                ComFunc.MsgBox("MVIEW 권한이 없습니다!!");
                return;
            }
            else
            {
                clsPacs.PACS_Image_View(clsDB.DbCon, argPano, strUid, clsType.User.IdNumber);
            }
        }

        /// <summary>
        /// NVC결과지 출력
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="argROWID"></param>
        /// <returns></returns>
        bool setNVC(PsmhDb pDbCon, FpSpread Spd, string argROWID)
        {
            bool b = true;
            DataTable dt = null;
            string strTemp = "";

            dt = sel_ETC_RESULT_NVC(pDbCon, "", "", "", "", argROWID);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                #region NVC-View

                Spd.ActiveSheet.Rows[31].Visible = false;

                Spd.ActiveSheet.Cells[3, 2].Text = dt.Rows[0]["PtNo"].ToString().Trim();
                Spd.ActiveSheet.Cells[4, 2].Text = dt.Rows[0]["SName"].ToString().Trim();
                Spd.ActiveSheet.Cells[4, 4].Text = dt.Rows[0]["RDate"].ToString().Trim();
                Spd.ActiveSheet.Cells[5, 2].Text = dt.Rows[0]["Age"].ToString().Trim() + "/" + dt.Rows[0]["Sex"].ToString().Trim();

                strTemp = VB.Space(10);
                if (dt.Rows[0]["PtNo"].ToString().Trim() == "Y")
                {
                    strTemp += "유";
                }
                else if (dt.Rows[0]["PtNo"].ToString().Trim() == "N")
                {
                    strTemp += "무";
                }
                //Raynaud Phenomenon
                strTemp += VB.Space(10) + "onset: ";
                strTemp += dt.Rows[0]["Onset"].ToString().Trim() + "세";
                Spd.ActiveSheet.Cells[6, 2].Text = strTemp;

                //진단명
                Spd.ActiveSheet.Cells[7, 2].Text = VB.Space(2) + dt.Rows[0]["Diagnosis"].ToString().Trim();


                //검사부위①
                strTemp = VB.Space(2);
                if (dt.Rows[0]["Part11"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Part11"].ToString().Trim() == "R")
                    {
                        strTemp += "Right ";
                    }
                    else if (dt.Rows[0]["Part11"].ToString().Trim() == "L")
                    {
                        strTemp += "Left ";
                    }
                    else if (dt.Rows[0]["Part11"].ToString().Trim() == "B")
                    {
                        strTemp += "Both ";
                    }

                    if (dt.Rows[0]["Part12"].ToString().Trim() == "4")
                    {
                        strTemp += "4th finger";
                    }
                    else if (dt.Rows[0]["Part12"].ToString().Trim() == "5")
                    {
                        strTemp += "5th finger";
                    }
                    else if (dt.Rows[0]["Part12"].ToString().Trim() == "B")
                    {
                        strTemp += "4,5th finger";
                    }

                    if (dt.Rows[0]["Part21"].ToString().Trim() == "")
                    {
                        Spd.ActiveSheet.Cells[9, 2].Text = strTemp;
                    }
                    else if (dt.Rows[0]["Part21"].ToString().Trim() == "")
                    {
                        Spd.ActiveSheet.Cells[8, 2].Text = strTemp;
                    }
                }

                //검사부위②
                strTemp = VB.Space(2);
                if (dt.Rows[0]["Part21"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Part21"].ToString().Trim() == "R")
                    {
                        strTemp += "Right ";
                    }
                    else if (dt.Rows[0]["Part21"].ToString().Trim() == "L")
                    {
                        strTemp += "Left ";
                    }
                    else if (dt.Rows[0]["Part21"].ToString().Trim() == "B")
                    {
                        strTemp += "Both ";
                    }

                    if (dt.Rows[0]["Part22"].ToString().Trim() == "4")
                    {
                        strTemp += "4th finger";
                    }
                    else if (dt.Rows[0]["Part22"].ToString().Trim() == "5")
                    {
                        strTemp += "5th finger";
                    }
                    else if (dt.Rows[0]["Part22"].ToString().Trim() == "B")
                    {
                        strTemp += "4,5th finger";
                    }

                    Spd.ActiveSheet.Cells[9, 2].Text = strTemp;

                }

                strTemp = VB.Space(2);
                if (dt.Rows[0]["Part23"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Part23"].ToString().Trim() == "1")
                    {
                        strTemp += "(pitting scar)";
                    }
                    else if (dt.Rows[0]["Part23"].ToString().Trim() == "2")
                    {
                        strTemp += "(active ulcer)";
                    }
                    else if (dt.Rows[0]["Part23"].ToString().Trim() == "3")
                    {
                        strTemp += "(gangrene)";
                    }
                    Spd.ActiveSheet.Cells[10, 2].Text = strTemp;
                }

                if (Spd == ssNvc)
                {
                    //Findings-①
                    strTemp = "①Giant capillaries";
                    if (dt.Rows[0]["Findings11"].ToString().Trim() == "1")
                    {
                        strTemp += " (homogeneously enlaged) : ";
                    }
                    else if (dt.Rows[0]["Findings11"].ToString().Trim() == "2")
                    {
                        strTemp += " (irregulary enlaged) : ";
                    }
                    else
                    {
                        strTemp += ": ";
                    }
                    if (dt.Rows[0]["Findings12"].ToString().Trim() == "1")
                    {
                        strTemp += "▶None";
                    }
                    else if (dt.Rows[0]["Findings12"].ToString().Trim() == "2")
                    {
                        strTemp += "▶A few";
                    }
                    else if (dt.Rows[0]["Findings12"].ToString().Trim() == "3")
                    {
                        strTemp += "▶Moderate";
                    }
                    else if (dt.Rows[0]["Findings12"].ToString().Trim() == "4")
                    {
                        strTemp += "▶Frequent";
                    }
                    Spd.ActiveSheet.Cells[17, 1].Text = strTemp;

                    //Findings-②
                    strTemp = "②Architectural arrangement : ";
                    if (dt.Rows[0]["Findings2"].ToString().Trim() == "1")
                    {
                        strTemp += "▶well preserved";
                    }
                    else if (dt.Rows[0]["Findings2"].ToString().Trim() == "2")
                    {
                        strTemp += "▶mild disorganization";
                    }
                    else if (dt.Rows[0]["Findings2"].ToString().Trim() == "3")
                    {
                        strTemp += "▶Moderate disorganization";
                    }
                    else if (dt.Rows[0]["Findings2"].ToString().Trim() == "4")
                    {
                        strTemp += "▶severe disorganization";
                    }
                    Spd.ActiveSheet.Cells[18, 1].Text = strTemp;

                    //Findings-③
                    strTemp = "③Loss of capillaries (<30 over 5mm in the distal row of nailfold) : ";
                    if (dt.Rows[0]["Findings3"].ToString().Trim() == "1")
                    {
                        strTemp += "▶None";
                    }
                    else if (dt.Rows[0]["Findings3"].ToString().Trim() == "2")
                    {
                        strTemp += "▶Mild";
                    }
                    else if (dt.Rows[0]["Findings3"].ToString().Trim() == "3")
                    {
                        strTemp += "▶Moderate";
                    }
                    else if (dt.Rows[0]["Findings3"].ToString().Trim() == "4")
                    {
                        strTemp += "▶Severe";
                    }
                    Spd.ActiveSheet.Cells[19, 1].Text = strTemp;

                    //Findings-④
                    strTemp = "④Avascular area : ";
                    if (dt.Rows[0]["Findings4"].ToString().Trim() == "1")
                    {
                        strTemp += "▶None";
                    }
                    else if (dt.Rows[0]["Findings4"].ToString().Trim() == "2")
                    {
                        strTemp += "▶Some";
                    }
                    else if (dt.Rows[0]["Findings4"].ToString().Trim() == "3")
                    {
                        strTemp += "▶Moderate";
                    }
                    else if (dt.Rows[0]["Findings4"].ToString().Trim() == "4")
                    {
                        strTemp += "▶Extensive";
                    }
                    Spd.ActiveSheet.Cells[20, 1].Text = strTemp;

                    //Findings-⑤
                    strTemp = "⑤Hemorrhage : ";
                    if (dt.Rows[0]["Findings5"].ToString().Trim() == "1")
                    {
                        strTemp += "▶None";
                    }
                    else if (dt.Rows[0]["Findings5"].ToString().Trim() == "2")
                    {
                        strTemp += "▶Some";
                    }
                    else if (dt.Rows[0]["Findings5"].ToString().Trim() == "3")
                    {
                        strTemp += "▶Moderate";
                    }
                    else if (dt.Rows[0]["Findings5"].ToString().Trim() == "4")
                    {
                        strTemp += "▶Frequent";
                    }
                    Spd.ActiveSheet.Cells[21, 1].Text = strTemp;

                    //Findings-⑥
                    strTemp = "⑥Ramified capillaries (Angiogenesis) : ";
                    if (dt.Rows[0]["Findings6"].ToString().Trim() == "1")
                    {
                        strTemp += "▶None";
                    }
                    else if (dt.Rows[0]["Findings6"].ToString().Trim() == "2")
                    {
                        strTemp += "▶A few";
                    }
                    else if (dt.Rows[0]["Findings6"].ToString().Trim() == "3")
                    {
                        strTemp += "▶Moderate";
                    }
                    else if (dt.Rows[0]["Findings6"].ToString().Trim() == "4")
                    {
                        strTemp += "▶Frequent";
                    }
                    Spd.ActiveSheet.Cells[22, 1].Text = strTemp;
                }
                else if (Spd == ssNvc2)
                {
                    //1
                    if(dt.Rows[0]["Findings11"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[17, 4].Text = "있음";
                    }
                    else  if (dt.Rows[0]["Findings11"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[17, 4].Text = "없음";
                    }

                    //2
                    if (dt.Rows[0]["Findings2"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[18, 4].Text = "있음";
                    }
                    else if (dt.Rows[0]["Findings2"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[18, 4].Text = "없음";
                    }

                    //3
                    if (dt.Rows[0]["Findings3"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[19, 4].Text = "있음";
                    }
                    else if (dt.Rows[0]["Findings3"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[19, 4].Text = "없음";
                    }

                    //4
                    if (dt.Rows[0]["Findings4"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[20, 4].Text = "있음";
                    }
                    else if (dt.Rows[0]["Findings4"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[20, 4].Text = "없음";
                    }

                    //5
                    if (dt.Rows[0]["Findings5"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[21, 4].Text = "있음";
                    }
                    else if (dt.Rows[0]["Findings5"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[21, 4].Text = "없음";
                    }

                    //6
                    if (dt.Rows[0]["Findings6"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[22, 4].Text = "있음";
                    }
                    else if (dt.Rows[0]["Findings6"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[22, 4].Text = "없음";
                    }

                    //7
                    if (dt.Rows[0]["Findings7"].ToString().Trim() == "1")
                    {
                        Spd.ActiveSheet.Cells[23, 4].Text = "있음";
                    }
                    else if (dt.Rows[0]["Findings7"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[23, 4].Text = "없음";
                    }
                }

                //Conclusions
                strTemp = dt.Rows[0]["Conclusions"].ToString().Trim();
                if (strTemp != "")
                {
                    for (int i = 1; i < setL(strTemp, "\r\n"); i++)
                    {
                        if (i > 4)
                        {
                            break;
                        }
                        Spd.ActiveSheet.Cells[24 + i, 1].Text = "  " + setP(strTemp, "\r\n", i);
                    }
                }
                
                #endregion                
            }
            else
            {
                ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }

            return b;
        }

        void setNVC_clear(FpSpread Spd)
        {

            Spd.ActiveSheet.Cells[3, 2].Text = "";
            Spd.ActiveSheet.Cells[4, 2].Text = "";
            Spd.ActiveSheet.Cells[4, 4].Text = "";
            Spd.ActiveSheet.Cells[5, 2].Text = "";
                  
            Spd.ActiveSheet.Cells[6, 2].Text = "";

            //진단명
            Spd.ActiveSheet.Cells[7, 2].Text = "";


            //검사부위①  
            Spd.ActiveSheet.Cells[9, 2].Text = "";             
                
            Spd.ActiveSheet.Cells[8, 2].Text = "";
               
         
            //검사부위②
            Spd.ActiveSheet.Cells[9, 2].Text = "";
            Spd.ActiveSheet.Cells[10, 2].Text = "";


            //Findings-①           
            if (Spd == ssNvc)
            {
                Spd.ActiveSheet.Cells[17, 1].Text = "";

                //Findings-②            
                Spd.ActiveSheet.Cells[18, 1].Text = "";

                //Findings-③            
                Spd.ActiveSheet.Cells[19, 1].Text = "";

                //Findings-④

                Spd.ActiveSheet.Cells[20, 1].Text = "";

                //Findings-⑤            
                Spd.ActiveSheet.Cells[21, 1].Text = "";

                //Findings-⑥            
                Spd.ActiveSheet.Cells[22, 1].Text = "";
            }

            if(Spd == ssNvc2)
            {
                //Findings-⑦   
                //Spd.ActiveSheet.Cells[23, 1].Text = "";

                Spd.ActiveSheet.Cells[17, 2].Text = "";
                Spd.ActiveSheet.Cells[18, 2].Text = "";
                Spd.ActiveSheet.Cells[19, 2].Text = "";
                Spd.ActiveSheet.Cells[20, 2].Text = "";
                Spd.ActiveSheet.Cells[21, 2].Text = "";
                Spd.ActiveSheet.Cells[22, 2].Text = "";
                Spd.ActiveSheet.Cells[23, 2].Text = "";
            }

            //Conclusions
      
            for (int i = 1; i < 5; i++)
            {               
                Spd.ActiveSheet.Cells[24 + i, 1].Text = "";
            }
           
        }

        void SpreadPrint(FpSpread o, bool prePrint)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    return;
            //}
            //else
            //{
                string sPrtName = ""; //선택 프린트명
                string bPrtName = ""; //기본 프린트명
                if (gPrintName.ToUpper() == "기본프린트")
                {
                    sPrtName = clsPrint.gGetDefaultPrinter();
                }
                else
                {
                    sPrtName = cp.getPrinter_Chk(gPrintName.ToUpper());
                }

                if (sPrtName != "")
                {
                    bPrtName = clsPrint.gGetDefaultPrinter();
                    clsPrint.gSetDefaultPrinter(sPrtName);

                    string header = string.Empty;
                    string foot = string.Empty;

                    clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(0, 0, PrtY + 0, 0, PrtX + 0, 0);
                    clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                                    , PrintType.All, 0, 0, false, false, false, false, false, false, false);

                    methodSpd.setSpdPrint(o, prePrint, margin, option, header, foot);

                    ComFunc.Delay(500);
                    clsPrint.gSetDefaultPrinter(bPrtName);

                }

            ///}

        }

        #endregion


    }
}
