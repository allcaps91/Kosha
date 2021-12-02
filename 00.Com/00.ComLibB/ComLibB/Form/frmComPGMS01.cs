using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary> 상병코드 TREE </summary>
    public partial class frmComPGMS01 : Form
    {
        string SQL = string.Empty;

        clsSpread spread = new clsSpread();
        ComQuery comQuery = new ComQuery();


        enum enmSel_BAS_PROJECT_UC     {    MNGM_NO, RT_MNGM_NO,    OCRR_DT,  RQST_PS, RQST_PSNM,   FORMCD, RQST_CNTS,  WORK_DVSN,        MIN, WORK_DVSN_NM, WORK_STAT, WORK_STAT_NM, RCPN_PRSN, RCPN_PRSN_NM,    RCPN_DT,  CMPL_PS, CMPL_PSNM, REWORK_CNT,    CMPL_DT,   WORK_CLS, WORK_CLS_NM,  CMPL_CNTS,   INPS,   INPT_DY,   UPPS,   UPDT_DY, ROWID };
        string []sSel_BAS_PROJECT_UC = { "관리번호",   "원번호", "작성시간", "작성자",  "작성자", "화면ID",    "내용", "작업구분", "작업(분)",   "작업구분",    "상태",       "상태",  "접수자",     "접수자", "접수시간", "작업자",   "작업자",   "재작업", "완료일시", "작업분류",  "작업분류", "작업내용", "INPS", "INPT_DY", "UPPS", "UPDT_DY", "ROWID" };
        int    []nSel_BAS_PROJECT_UC = {         40,         40,        110,        5,        80,      100,       300,          5,         40,           60,         5,           60,         5,           80,        110,        5,         80,         40,        120,          5,          60,        180,      5,         5,      5,         5,      5 };

        enum enmSel_BAS_PROJECT_UC_STAT { WORK_STAT, CNT };
        string[] sSel_BAS_PROJECT_UC_STAT = { "작업분류", "건수" };
        int[] nSel_BAS_PROJECT_UC_STAT = { 200, 100 };

        enum enmSel_BAS_PROJECT_UC_WORKPS     {  WORK_PS,   WORK_PS_NM,  WORK_DVSN,   WORK_DVSN_NM,    CNT};
        string[] sSel_BAS_PROJECT_UC_WORKPS = { "작업자",     "작업자", "작업구분",     "작업구분", "건수"};
        int[] nSel_BAS_PROJECT_UC_WORKPS    = {        0,          100,          0,            100,   50 };

        enum enmSel_BAS_PROJECT_UC_WORKCLS { WORK_CLS, WORK_CLS_NM, CNT };
        string[] sSel_BAS_PROJECT_UC_WORKCLS = { "작업분류", "작업분류", "건수" };
        int[] nSel_BAS_PROJECT_UC_WORKCLS = { 0, 200, 100 };


        enum enmSel_BAS_PROJECT_UC_FORMCD       { FORMCD,   WORK_DVSN, WORK_DVSN_NM, CNT };
        string[] sSel_BAS_PROJECT_UC_FORMCD =   { "폼명",  "작업분류", "작업분류", "건수" };
        int[] nSel_BAS_PROJECT_UC_FORMCD    =   {    200,           0, 300, 100 };

        DateTime sysdate;

        DataTable gDt;
        DataSet gDs;
        DataRow gDr;
        /// <summary> 상병코드 TREE </summary>
        public frmComPGMS01()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.ssMain.CellClick       += new CellClickEventHandler(eSpreadClick);

            this.btnSave_New.Click      += new EventHandler(eBtnSave);
            this.btnSave.Click          += new EventHandler(eBtnSave);
            this.btnSave_RECP.Click     += new EventHandler(eBtnSave);

            this.btnSearch_User.Click   += new EventHandler(eBtnSearch);
            this.chk_REWORK_CNT.Click   += new EventHandler(eChkBtn);

            this.btnDelete_RECP.Click   += new EventHandler(eBtnDelete);

            this.btnSave_COMMIT.Click   += new EventHandler(eBtnSave);
            this.btnDelete_COMMIT.Click += new EventHandler(eBtnDelete);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            setCtrl();
        }

        void setCtrl()
        {
            setCtrlScreen();
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread(this.ssMain, false);

            setCtrlSpread(this.ss_ADMIN_FORMCD, true);
            setCtrlSpread(this.ss_ADMIN_STAT, true);
            setCtrlSpread(this.ss_ADMIN_WORKPS, true);
            setCtrlSpread(this.ss_ADMIN_WORKCLS, true);
            setCtrlSpread(this.ss_Admin_Detail, true);

            saveNew();
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch_User)
            {
                setCtrlSpread(this.ssMain, false);
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave_New)
            {
                saveNew();
            }
            else if (sender == this.btnSave_RECP)
            {
                save_RECP();
            }
            else if (sender == this.btnSave_COMMIT)
            {
                save_COMMIT();
            }
            else if (sender == this.btnSave)
            {                
                if (isChkSave() == true)
                {
                    clsDB.setBeginTran(clsDB.DbCon);
                    
                    string SqlErr = string.Empty;
                    string SQL = string.Empty;
                    int intRowAffected = 0;
                    int chkRow = 0;

                    this.gDr[(int)enmSel_BAS_PROJECT_UC.FORMCD] = this.txt_FORMCD.Text.Trim();
                    this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_CNTS] = this.txt_RQST_CNTS.Text.Trim();

                    if (this.chk_REWORK_CNT.Checked == false && string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.ROWID].ToString()) == false && string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString()) == false)
                    {
                        SqlErr = up_BAS_PROJECT_UC(false, ref intRowAffected);

                        if (string.IsNullOrEmpty(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                            return;
                        }
                    }
                    else
                    {

                        if (this.chk_REWORK_CNT.Checked == true)
                        {


                            SqlErr = up_BAS_PROJECT_UC(true, ref intRowAffected);

                            if (string.IsNullOrEmpty(SqlErr) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                                return;
                            }

                            SqlErr = ins_BAS_PROJECT_UC_RE(ref intRowAffected);

                            if (string.IsNullOrEmpty(SqlErr) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                                return;
                            }

                            SqlErr = up_BAS_PROJECT_UC2(ref intRowAffected);
                            if (string.IsNullOrEmpty(SqlErr) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                                return;
                            }



                        }
                        else
                        {
                            SqlErr = ins_BAS_PROJECT_UC(ref intRowAffected);
                        }

                        if (string.IsNullOrEmpty(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                            return;
                        }
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    setCtrlSpread(this.ssMain, false);
                    saveNew();
                }
            }           
        }

        void eBtnDelete(object sender, EventArgs e)
        {
            if (sender == this.btnDelete_RECP)
            {
                if (this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("01"))
                {
                    if (ComFunc.MsgBoxQ("고객이 이미 확인한 사항일 수 있습니다. 접수를 취소하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {

                        clsDB.setBeginTran(clsDB.DbCon);

                        string SqlErr = string.Empty;
                        string SQL = string.Empty;
                        int intRowAffected = 0;
                        int chkRow = 0;

                        up_BAS_PROJECT_UC_RECP(true, ref intRowAffected);

                        clsDB.setCommitTran(clsDB.DbCon);
                        setCtrlSpread(this.ssMain, false);
                        saveNew();
                    }

                }
                else if (this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("02"))
                {
                    ComFunc.MsgBox("완료된 사항은 접수 취소를 하실 수 없습니다.");
                }

            }
            else if (sender == this.btnDelete_COMMIT)
            {
                if (this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("02"))
                {
                    if (ComFunc.MsgBoxQ("고객이 이미 확인한 사항일 수 있습니다. 완료를 취소하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        string SqlErr = string.Empty;
                        string SQL = string.Empty;
                        int intRowAffected = 0;
                        int chkRow = 0;

                        up_BAS_PROJECT_UC_COMMIT(true, ref intRowAffected);

                        clsDB.setCommitTran(clsDB.DbCon);
                        setCtrlSpread(this.ssMain, false);
                        saveNew();
                    }

                }
                else
                {
                    ComFunc.MsgBox("완료된 내용만이 취소가 가능합니다.");
                }

            }
        }

        void saveNew()
        {
            setCtrlDr();
            setCtrlText(true);
            setCtrlRead();
        }

        bool isChkSaveRecp()
        {
            bool b = true;

            if (string.IsNullOrEmpty(this.txt_RCPN_PRSN.Text) == true)
            {
                ComFunc.MsgBox("접수자를 입력하세요");
                return false;
            }

            if (this.gDr == null || string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.ROWID].ToString()) == true)
            {
                ComFunc.MsgBox("작업 대상을 선택하세요");
                return false;
            }

            if (!this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("00"))
            {
                ComFunc.MsgBox("미접수 상태만이 접수가 가능합니다.");
                return false;

            }

            return b;
        }

        bool isChkSaveCMPL()
        {
            bool b = true;


            if (this.gDr == null || string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.ROWID].ToString()) == true)
            {
                ComFunc.MsgBox("작업 대상을 선택하세요");
                return false;
            }

            if (!(this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("01") || this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("02")))
            {
                ComFunc.MsgBox("접수 상태만이 완료처리가 가능합니다.");
                return false;
            }

            if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PS].ToString()) == true)
            {
                ComFunc.MsgBox("완료자를 입력하세요.");
                this.txt_CMPL_PS.Select();
                return false;
            }

            if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_DVSN].ToString()) == true)
            {
                ComFunc.MsgBox("분류를 입력하세요.");
                this.cbo_WORK_DVSN_NM.Select();
                return false;

            }

            if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_CLS].ToString()) == true)
            {
                ComFunc.MsgBox("세부 분류를 입력하세요.");
                this.cbo_WORK_CLS_NM.Select();
                return false;

            }

            if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_CNTS].ToString()) == true)
            {
                ComFunc.MsgBox("내용을 입력하세요.");
                this.txt_CMPL_CNTS.Select();
                return false;
            }

            return b;
        }

        void save_RECP()
        {

            if (isChkSaveRecp() == false)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            int chkRow = 0;

            this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN] = this.txt_RCPN_PRSN.Text;

            up_BAS_PROJECT_UC_RECP(false,  ref intRowAffected);

            clsDB.setCommitTran(clsDB.DbCon);
            setCtrlSpread(this.ssMain, false);
            saveNew();

        }

        void save_COMMIT()
        {

            this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PS]    = this.txt_CMPL_PS.Text;
            this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_DVSN]  = getGubunText(this.cbo_WORK_DVSN_NM.Text,".");
            this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_CLS]   = getGubunText(this.cbo_WORK_CLS_NM.Text, ".");
            this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_CNTS]   = this.txt_CMPL_CNTS.Text;

            if (isChkSaveCMPL() == false)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;
            int intRowAffected = 0;
            int chkRow = 0;

            

            up_BAS_PROJECT_UC_COMMIT(false,  ref intRowAffected);

            clsDB.setCommitTran(clsDB.DbCon);
            setCtrlSpread(this.ssMain, false);
            saveNew();

        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            DataSet ds = (DataSet)this.ssMain.DataSource;
            this.gDr = ds.Tables[0].Rows[e.Row];

            setCtrlText(false);
            setCtrlRead();

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        bool isChkSave()
        {
            bool b = true;

            if (string.IsNullOrEmpty(this.txt_FORMCD.Text) == true)
            {
                ComFunc.MsgBox("화면정보를 반드시 입력하세요");
                this.txt_FORMCD.Select();
                return false;
            }

            if (string.IsNullOrEmpty(this.txt_RQST_CNTS.Text) == true)
            {
                ComFunc.MsgBox("내용을 입력하세요");
                this.txt_RQST_CNTS.Select();
                return false;
            }

            if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.ROWID].ToString()) == true )
            {
                if (this.chk_REWORK_CNT.Checked == true)
                {
                    ComFunc.MsgBox("신규 작성 시에는 재오류를 체크 할 수 없습니다.");
                    return false;
                }
            }
            else
            {
                DataSet ds = sel_BAS_PROJECT_UC(this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString());
                this.gDt = ds.Tables[0];

                if (this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].ToString().Equals("00") == false)
                {
                    ComFunc.MsgBox("접수 이상의 상태에서는 데이터를 수정 할 수 없습니다.");
                    return false;
                }
            }

            return b;
        }

        void setCtrlDr()
        {            
            this.gDt = new DataTable();
            this.gDt = this.gDs.Tables[0].Clone();
            this.gDr = this.gDt.NewRow();



            //row = this.gDt.NewRow();
            this.gDr[(int)enmSel_BAS_PROJECT_UC.ROWID] = "";
            this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT] = "00";
            this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PS] = clsType.User.IdNumber;
            this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PSNM] = clsType.User.UserName;
            this.gDr[(int)enmSel_BAS_PROJECT_UC.INPS] = clsType.User.IdNumber;
            this.gDr[(int)enmSel_BAS_PROJECT_UC.UPPS] = clsType.User.IdNumber;

            //this.gDt.Rows.Add(row);
        }

        void setCtrlRead()
        {

            if (this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].Equals("00"))
            {
                //미접수일때만 수정 가능
                this.txt_FORMCD.ReadOnly    = false;
                this.txt_RQST_CNTS.ReadOnly = false;
                this.btnSave.Enabled        = true;
                this.btnSave_New.Enabled    = true;
                this.chk_REWORK_CNT.Enabled = false;
            }
            else
            {
                this.txt_FORMCD.ReadOnly    = true;
                this.txt_RQST_CNTS.ReadOnly = true;
                this.btnSave.Enabled        = false;
                this.btnSave_New.Enabled    = true;

                if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.RT_MNGM_NO].ToString()) == true)
                {
                    this.chk_REWORK_CNT.Enabled = true;
                }
                else
                {
                    this.chk_REWORK_CNT.Enabled = false;
                }

            }
        }

        void setCtrlText(bool isClear)
        {
            if (isClear == true)
            {
                this.txt_FORMCD.Text           = string.Empty;
                this.txt_OCRR_DT.Text          = string.Empty;
                this.txt_RCPN_PRSN.Text        = string.Empty;
                this.txt_RCPN_PRSN_NM.Text     = string.Empty;
                this.txt_RQST_CNTS.Text        = string.Empty;
                this.txt_RQST_PS.Text          = clsType.User.IdNumber;
                this.txt_RQST_PSNM.Text        = clsType.User.UserName;
                this.txt_CMPL_CNTS.Text        = string.Empty;
                this.txt_CMPL_PS.Text          = string.Empty;
                this.txt_CMPL_PSNM.Text        = string.Empty;
                this.txt_WORK_STAT_NM.Text     = string.Empty;
                this.cbo_WORK_CLS_NM.Text      = string.Empty;
                this.txt_REWORK_CNT.Text       = string.Empty;
                this.chk_REWORK_CNT.Checked    = false;

                this.txt_RCPN_PRSN.Text        = clsType.User.IdNumber;
                this.txt_RCPN_PRSN_NM.Text     = clsType.User.UserName;

                this.txt_CMPL_PS.Text          = clsType.User.IdNumber;
                this.txt_CMPL_PSNM.Text        = clsType.User.UserName;

                this.cbo_WORK_CLS_NM.Text      = string.Empty;
                this.cbo_WORK_DVSN_NM.Text     = string.Empty;
                this.txt_CMPL_CNTS.Text        = string.Empty;

                this.txt_FORMCD.ReadOnly       = false;
                this.txt_RQST_CNTS.ReadOnly    = false;
                this.chk_REWORK_CNT.Enabled    = true;

                this.txt_FORMCD.Select();
            }
            else
            {
                this.txt_FORMCD.Text           = this.gDr[(int)enmSel_BAS_PROJECT_UC.FORMCD].ToString();
                this.txt_OCRR_DT.Text          = this.gDr[(int)enmSel_BAS_PROJECT_UC.OCRR_DT].ToString();
                this.txt_RCPN_PRSN.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN].ToString();
                this.txt_RCPN_PRSN_NM.Text     = this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN_NM].ToString();
                this.txt_RQST_CNTS.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_CNTS].ToString();
                this.txt_RQST_PS.Text          = this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PS].ToString();
                this.txt_RQST_PSNM.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PSNM].ToString();
                this.txt_CMPL_CNTS.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_CNTS].ToString();
                this.txt_CMPL_PS.Text          = this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PS].ToString();
                this.txt_CMPL_PSNM.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PSNM].ToString();
                this.txt_WORK_STAT_NM.Text     = this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT_NM].ToString();
                this.cbo_WORK_CLS_NM.Text      = this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_CLS_NM].ToString();
                this.cbo_WORK_DVSN_NM.Text     = this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_DVSN_NM].ToString();

                this.txt_REWORK_CNT.Text       = this.gDr[(int)enmSel_BAS_PROJECT_UC.REWORK_CNT].ToString();
              
                if (Convert.ToInt16(this.gDr[(int)enmSel_BAS_PROJECT_UC.REWORK_CNT].ToString()) > 0)
                {
                    this.chk_REWORK_CNT.Checked = true;
                }

                this.txt_RCPN_PRSN.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN].ToString();
                this.txt_RCPN_PRSN_NM.Text     = this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN_NM].ToString();
                                               
                this.txt_CMPL_PS.Text          = this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PS].ToString();
                this.txt_CMPL_PSNM.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PSNM].ToString();

                this.cbo_WORK_CLS_NM.Text      = this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_CLS_NM].ToString();
                this.txt_CMPL_CNTS.Text        = this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_CNTS].ToString();
            }
        }

        void setCtrlCombo()
        {
            DataTable dt = comQuery.Get_BasBcode(clsDB.DbCon, "BAS_PROJECT_UC.WORK_CLS", "");

            this.cbo_WORK_CLS_NM.Items.Clear();

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.cbo_WORK_CLS_NM.Items.Add(dt.Rows[i]["CODE"].ToString() + "." + dt.Rows[i]["NAME"].ToString());
                }

                if (this.cbo_WORK_CLS_NM.Items.Count > 0)
                {
                    this.cbo_WORK_CLS_NM.SelectedIndex = 0;
                }             
            }

            dt = comQuery.Get_BasBcode(clsDB.DbCon, "BAS_PROJECT_UC.WORK_DVSN", "");

            this.cbo_WORK_DVSN_NM.Items.Clear();

            if (ComFunc.isDataTableNull(dt) == false)
            {

                this.cbo_WORK_DVSN_NM.Items.Add("");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.cbo_WORK_DVSN_NM.Items.Add(dt.Rows[i]["CODE"].ToString() + "." + dt.Rows[i]["NAME"].ToString());
                }

                if (this.cbo_WORK_DVSN_NM.Items.Count > 0)
                {
                    this.cbo_WORK_DVSN_NM.SelectedIndex = 0;
                }
            }

        }

        void setCtrlScreen()
        {
            if (clsType.User.SilBuseCode.Equals("077501") == true)
            {
                this.panAdmin.Visible = true;
                this.splitAdmin.Visible = true;
                this.superTabItem2.Visible = true;
            }
            else
            {
                this.panAdmin.Visible = false;
                this.splitAdmin.Visible = false;
                superTabItem2.Visible = false;
            }
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtp_FOCRR_DT_USER.Value = sysdate.AddDays(-10);
            this.dtp_TOCRR_DT_USER.Value = sysdate;

            this.dtp_FOCRR_DT_ADMIN.Value = sysdate.AddDays(-10);
            this.dtp_TOCRR_DT_ADMIN.Value = sysdate;
        }

        void setCtrlSpread(FpSpread spd, bool isClear)
        {
            if (isClear == true)
            {
                if (spd == this.ssMain || spd == this.ss_Admin_Detail)
                {
                    setSpdStyle(spd, null, sSel_BAS_PROJECT_UC, nSel_BAS_PROJECT_UC);
                }
                else if (spd == this.ss_ADMIN_STAT)
                {
                    setSpdStyle(spd, null, sSel_BAS_PROJECT_UC_STAT, nSel_BAS_PROJECT_UC_STAT);
                }
                else if (spd == this.ss_ADMIN_WORKPS)
                {
                    setSpdStyle(spd, null, sSel_BAS_PROJECT_UC_WORKPS, nSel_BAS_PROJECT_UC_WORKPS);
                }
                else if (spd == this.ss_ADMIN_WORKCLS)
                {
                    setSpdStyle(spd, null, sSel_BAS_PROJECT_UC_WORKCLS, nSel_BAS_PROJECT_UC_WORKCLS);
                }
                else if (spd == this.ss_ADMIN_FORMCD)
                {
                    setSpdStyle(spd, null, sSel_BAS_PROJECT_UC_FORMCD, nSel_BAS_PROJECT_UC_FORMCD);
                }

            }
            else
            {
                if (spd == this.ssMain)
                {
                    this.gDs = sel_BAS_PROJECT_UC();

                    if (ComFunc.isDataSetNull(this.gDs) == true)
                    {
                        setSpdStyle(spd, null, sSel_BAS_PROJECT_UC, nSel_BAS_PROJECT_UC);
                    }
                    else
                    {
                        setSpdStyle(spd, this.gDs, sSel_BAS_PROJECT_UC, nSel_BAS_PROJECT_UC);
                    }
                }

                if (true)
                {

                }
            }            
        }

        string up_BAS_PROJECT_UC_COMMIT(bool isClear, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE KOSMOS_PMPA.BAS_PROJECT_UC               \r\n";
            SQL += "    SET                                          \r\n";

            if (isClear == true)
            {
                SQL += "        WORK_STAT  = '01'                    \r\n";
                SQL += "      , CMPL_PS    = ''                      \r\n";
                SQL += "      , CMPL_DT    = NULL                    \r\n";
                SQL += "      , WORK_DVSN  = NULL                    \r\n";
                SQL += "      , WORK_CLS   = NULL                    \r\n";
                SQL += "      , CMPL_CNTS  = NULL                    \r\n";
            }
            else
            {
                SQL += "        WORK_STAT   = '02'                   \r\n";
                SQL += "      , CMPL_PS     = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_PS].ToString(), false);
                SQL += "      , CMPL_DT     = SYSDATE                    \r\n";
                SQL += "      , WORK_DVSN   = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_DVSN].ToString(), false);
                SQL += "      , WORK_CLS    = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_CLS].ToString(), false);
                SQL += "      , CMPL_CNTS   = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.CMPL_CNTS].ToString(), false);

            }

            SQL += "      , UPDT_DY    = SYSDATE                    \r\n";
            SQL += "      , UPPS       = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN].ToString(), false);
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND MNGM_NO    =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + "\r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            return SqlErr;
        }

        string up_BAS_PROJECT_UC_RECP(bool isClear, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE KOSMOS_PMPA.BAS_PROJECT_UC               \r\n";
            SQL += "    SET                                          \r\n";

            if (isClear == true)
            {
                SQL += "        WORK_STAT  = '00'                    \r\n";
                SQL += "      , RCPN_PRSN  = ''                      \r\n";
                SQL += "      , RCPN_DT    = NULL                    \r\n";
            }
            else
            {
                SQL += "        WORK_STAT  = '01'                   \r\n";
                SQL += "      , RCPN_PRSN  = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN].ToString(), false);
                SQL += "      , RCPN_DT    = SYSDATE                    \r\n";
            }
            
            SQL += "      , UPDT_DY    = SYSDATE                    \r\n";
            SQL += "      , UPPS       = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RCPN_PRSN].ToString(), false);
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND MNGM_NO    =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + "\r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            return SqlErr;
        }

        string up_BAS_PROJECT_UC(bool isReWork, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE KOSMOS_PMPA.BAS_PROJECT_UC              \r\n";

            if (isReWork == true)
            {
                SQL += "    SET REWORK_CNT = (SELECT COUNT(*) + 1 FROM KOSMOS_PMPA.BAS_PROJECT_UC B WHERE RT_MNGM_NO =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + ")            \r\n";
            }
            else
            {
                SQL += "    SET                                          \r\n";
                SQL += "        OCRR_DT    = SYSDATE                     \r\n";
                SQL += "      , FORMCD     = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.FORMCD].ToString(), false);
                SQL += "      , RQST_CNTS  = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_CNTS].ToString(), false);
                SQL += "      , UPDT_DY    = SYSDATE                     \r\n";
            }
            
            SQL += "  WHERE 1=1                                     \r\n";

            if (isReWork == false)
            {
                SQL += "    AND MNGM_NO    =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + "\r\n";
            }
            else
            {
                SQL += "    AND RT_MNGM_NO    =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + " \r\n";
            }

          
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            return SqlErr;
        }

        string up_BAS_PROJECT_UC2( ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE KOSMOS_PMPA.BAS_PROJECT_UC              \r\n";
            SQL += "    SET REWORK_CNT = (SELECT COUNT(*) FROM KOSMOS_PMPA.BAS_PROJECT_UC B WHERE RT_MNGM_NO =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + ")            \r\n";
            SQL += "  WHERE 1=1                                     \r\n";
            SQL += "    AND MNGM_NO    =  " + this.gDr[(int)enmSel_BAS_PROJECT_UC.MNGM_NO].ToString() + "\r\n";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            return SqlErr;
        }

        void eChkBtn(object sender, EventArgs e)
        {
            if (this.chk_REWORK_CNT.Checked == true)
            {
                if (ComFunc.MsgBoxQ("오류가 다시 발생한 경우가 맞습니까?") == DialogResult.Yes)
                {
                    this.btnSave.Enabled = true;
                    this.txt_RQST_CNTS.ReadOnly = false;

                    this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT] = "00";
                    this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PS] = clsType.User.IdNumber;
                    this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PSNM] = clsType.User.UserName;
                    this.gDr[(int)enmSel_BAS_PROJECT_UC.INPS] = clsType.User.IdNumber;
                    this.gDr[(int)enmSel_BAS_PROJECT_UC.UPPS] = clsType.User.IdNumber;

                }
            }
        }

        string ins_BAS_PROJECT_UC_RE(ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO KOSMOS_PMPA.BAS_PROJECT_UC                                                                 \r\n";
            SQL += " (                                                                                                      \r\n";
            SQL += "       MNGM_NO                                                                                          \r\n";
            SQL += "     , RT_MNGM_NO   --원관리번호(재발생시사용)                                                          \r\n";   
            SQL += "     , OCRR_DT      --요청시간                                                                          \r\n";
            SQL += "     , RQST_PS      --요청자                                                                            \r\n"   ;   
            SQL += "     , FORMCD       --요청폼코드                                                                        \r\n" ;
            SQL += "     , WORK_STAT    --요청상태                                                                          \r\n";
            SQL += "     , RQST_CNTS    --요청내용                                                                          \r\n";
            SQL += "     , REWORK_CNT   --재발생                                                                            \r\n"   ;   
            SQL += "     , INPS         --입력자                                                                            \r\n"   ;   
            SQL += "     , INPT_DY      --입력일시                                                                          \r\n";
            SQL += "     , UPPS         --수정자                                                                            \r\n"   ;   
            SQL += "     , UPDT_DY      --수정일시                                                                          \r\n";
            SQL += " )                                                                                                      \r\n";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "       KOSMOS_PMPA.SEQ_BAS_PROJECT_UC.NEXTVAL AS MNGM_NO                                                \r\n";
            SQL += "     , DECODE(A.RT_MNGM_NO, NULL, A.MNGM_NO,A.RT_MNGM_NO) AS RT_MNGM_NO  -- 원관리번호(재발생시사용)   \r\n";
            SQL += "     , SYSDATE                                  \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PS].ToString(), true);
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.FORMCD].ToString(), true);
            SQL += "    , '00'                                                                                            \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_CNTS].ToString(), true);
            SQL += "     , (SELECT COUNT(*) FROM KOSMOS_PMPA.BAS_PROJECT_UC B WHERE RT_MNGM_NO = A.RT_MNGM_NO)           \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.INPS].ToString(), true);
            SQL += "     , SYSDATE                                  \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.UPPS].ToString(), true);
            SQL += "     , SYSDATE                                  \r\n";
            SQL += "  FROM KOSMOS_PMPA.BAS_PROJECT_UC A             \r\n";
            SQL += " WHERE ROWID = " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.ROWID].ToString(), false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            return SqlErr;
        }

        string ins_BAS_PROJECT_UC( ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO KOSMOS_PMPA.BAS_PROJECT_UC         \r\n";
            SQL += " (                                              \r\n";
            SQL += "       MNGM_NO                                  \r\n";
            SQL += "     , RT_MNGM_NO   --원관리번호(재발생시사용)  \r\n";
            SQL += "     , OCRR_DT      --요청시간                  \r\n";
            SQL += "     , RQST_PS      --요청자                    \r\n";
            SQL += "     , FORMCD       --요청폼코드                \r\n";
            SQL += "     , WORK_STAT    --요청상태                  \r\n";
            SQL += "     , RQST_CNTS    --요청내용                  \r\n";
            SQL += "     , REWORK_CNT   --재발생                    \r\n";
            SQL += "     , INPS         --입력자                    \r\n";
            SQL += "     , INPT_DY      --입력일시                  \r\n";
            SQL += "     , UPPS         --수정자                    \r\n";
            SQL += "     , UPDT_DY      --수정일시                  \r\n";
            SQL += " )                                              \r\n";
            SQL += " VALUES                                         \r\n";
            SQL += " (                                              \r\n";
            SQL += "       KOSMOS_PMPA.SEQ_BAS_PROJECT_UC.NEXTVAL   \r\n";

            if (string.IsNullOrEmpty(this.gDr[(int)enmSel_BAS_PROJECT_UC.RT_MNGM_NO].ToString()) == false)
            {
                SQL += "     , " + this.gDr[(int)enmSel_BAS_PROJECT_UC.RT_MNGM_NO].ToString() + "\r\n";
            }
            else
            {
                SQL += "     , '' \r\n";
            }
            
            SQL += "     , SYSDATE                                  \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_PS].ToString(), true);
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.FORMCD].ToString(), true);
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.WORK_STAT].ToString(), true);
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.RQST_CNTS].ToString(), true);
            SQL += "     , 0                                        \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.INPS].ToString(), true);
            SQL += "     , SYSDATE                                  \r\n";
            SQL += "    " + ComFunc.covSqlstr(this.gDr[(int)enmSel_BAS_PROJECT_UC.UPPS].ToString(), true);
            SQL += "     , SYSDATE                                  \r\n";
            SQL += " )                                              \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            return SqlErr;
        }

        DataSet sel_BAS_PROJECT_UC(string strMNGM_NO = "")
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                                                                                \r\n";
            SQL += "         MNGM_NO      											                                    --관리번호                                                                          \r\n";
            SQL += "       , RT_MNGM_NO   											                                    --원관리번호                                                                        \r\n";
            SQL += "       , TO_CHAR(OCRR_DT,'YYYY-MM-DD HH24:MI') 	AS OCRR_DT     	                                    --발생일시                                                                          \r\n";
            SQL += "       , RQST_PS      											                                    --요청자                                                                            \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_USER(RQST_PS) 		AS RQST_PSNM 	                                    --요청자이름                                                                        \r\n";
            SQL += "       , FORMCD       											                                    --프로그램ID                                                                        \r\n";
            SQL += "       , RQST_CNTS    											                                    --요청내용                                                                          \r\n";
            SQL += "       , WORK_DVSN    																				--작업구분 (BCODE : BAS_PROJECT_UC.WORK_DVSN)                                       \r\n";
            SQL += "       , ROUND(                                                                                             \r\n";
            SQL += "                 (TO_DATE(TO_CHAR(CMPL_DT,'YYYY-MM-DD HH24:MI'), 'YYYY-MM-DD HH24:MI')                      \r\n";
            SQL += "                - TO_DATE(TO_CHAR(RCPN_DT,'YYYY-MM-DD HH24:MI'), 'YYYY-MM-DD HH24:MI') )* 24 * 60,0         \r\n";
            SQL += "               )                                                                    AS MIN                  \r\n";
            SQL += "       , DECODE(WORK_DVSN,NULL,'',KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BAS_PROJECT_UC.WORK_DVSN',WORK_DVSN)) 	AS WORK_DVSN_NM --작업상태 (BCODE : BAS_PROJECT_UC.WORK_STAT)                                   \r\n";
            SQL += "       , WORK_STAT    																				--작업상태 (BCODE : BAS_PROJECT_UC.WORK_STAT)                                       \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BAS_PROJECT_UC.WORK_STAT',WORK_STAT) 	AS WORK_STAT_NM --작업상태 (BCODE : BAS_PROJECT_UC.WORK_STAT)                                   \r\n";
            SQL += "       , RCPN_PRSN    											                                    --접수자                                                                                                                \r\n";
            SQL += "       , DECODE(KOSMOS_OCS.FC_BAS_USER(RCPN_PRSN),'', RCPN_PRSN)                	AS RCPN_PRSN_NM                                                                                         \r\n";
            SQL += "       , TO_CHAR(RCPN_DT,'YYYY-MM-DD HH24:MI') 									    AS RCPN_DT      --접수시간                                                                          \r\n";
            SQL += "       , CMPL_PS      											                                    --작업자                                                                                                                \r\n";
            SQL += "       , DECODE(KOSMOS_OCS.FC_BAS_USER(CMPL_PS),'', CMPL_PS)    					AS WORK_PSNM                                                                                            \r\n";
            SQL += "       , REWORK_CNT   											                                    --재발생건수                                                                                                            \r\n";
            SQL += "       , TO_CHAR(CMPL_DT,'YYYY-MM-DD HH24:MI') 									    AS CMPL_DT    	--완료일시                                                                          \r\n";
            SQL += "       , WORK_CLS     																				--작업분류 (BCODE : BAS_PROJECT_UC.WORK_CLS)                                        \r\n";
            SQL += "       , DECODE(WORK_CLS,NULL,'',WORK_CLS || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BAS_PROJECT_UC.WORK_CLS',WORK_CLS)) 	AS WORK_CLS_NM    	--작업상태 (BCODE : BAS_PROJECT_UC.WORK_STAT)                                       \r\n";
            SQL += "       , CMPL_CNTS    --작업내용                                                                                                                                                        \r\n";
            SQL += "       , INPS         --입력자                                                                                                                                                          \r\n";
            SQL += "       , INPT_DY      --입력일시                                                                                                                                                        \r\n";
            SQL += "       , UPPS         --수정자                                                                                                                                                          \r\n";
            SQL += "       , UPDT_DY      --수정일시                                                                                                                                                        \r\n";
            SQL += "       , ROWID        --ROWID                                                                                                                                                           \r\n";
            SQL += "    FROM KOSMOS_PMPA.BAS_PROJECT_UC /** 전산 커뮤니티 관리*/                                                                                                                            \r\n";
            SQL += "  WHERE 1=1                                                                                                                                                                             \r\n";

            if (string.IsNullOrEmpty(strMNGM_NO) == false)
            {
                SQL += "    AND MNGM_NO  = " + strMNGM_NO + "\r\n";
            }
            else
            {
                SQL += "    AND OCRR_DT BETWEEN TO_DATE('" + this.dtp_FOCRR_DT_USER.Value.ToString("yyyy-MM-dd") + " 00:00', 'YYYY-MM-DD HH24:MI')                                                               \r\n";
                SQL += "                    AND TO_DATE('" + this.dtp_TOCRR_DT_USER.Value.ToString("yyyy-MM-dd") + " 23:59', 'YYYY-MM-DD HH24:MI')                                                               \r\n";

                if (clsType.User.SilBuseCode.Equals("077501") == false)
                {
                    SQL += "    AND RQST_PS = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
                }


                SQL += "  ORDER BY MNGM_NO, OCRR_DT                                                                                                                                                                      \r\n";
            
            }
            try                                                                                                                                                                                 
            {                                                                                                                                                                                   
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, clsDB.DbCon);                                                                                                                                  
                                                                                                                                                                                                
                if (SqlErr != "")                                                                                                                                                               
                {                                                                                                                                                                               
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");                                                                                                                               
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        string getGubunText(string s, string gubun) // 12345.생화학 -> 12345를 반환
        {
            string strReturn = "";
            strReturn = s;

            if (strReturn != null && strReturn.Length > 0 && strReturn.IndexOf(gubun) > 0)
            {
                strReturn = strReturn.Substring(0, strReturn.IndexOf(gubun));
                if (strReturn.ToUpper() == "NULL" || strReturn.IndexOf('*') == 0)
                {
                    strReturn = "*";
                }
            }

            return strReturn;
        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 30;
            // 화면상의 정렬표시 Clear
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            spd.TextTipDelay = 500;
            spd.TextTipPolicy = TextTipPolicy.Fixed;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;
            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            if (spd == this.ssMain || spd == this.ss_Admin_Detail)
            {
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.INPS, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.INPT_DY, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.RCPN_PRSN, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.ROWID, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.RQST_PS, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.UPDT_DY, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.UPPS, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.WORK_CLS, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.WORK_DVSN, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.CMPL_PS, clsSpread.enmSpdType.Hide);
                spread.setColStyle(spd, -1, (int)enmSel_BAS_PROJECT_UC.WORK_STAT, clsSpread.enmSpdType.Hide);

                Color cSpdCellImpact_Back = Color.FromArgb(255, 60, 60);
                Color cSpdCellImpact_Fore = Color.FromArgb(255, 250, 250);

                UnaryComparisonConditionalFormattingRule unary;
                unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.GreaterThan, 60, false);

                unary.BackColor = cSpdCellImpact_Back;
                unary.ForeColor = cSpdCellImpact_Fore;

                this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_BAS_PROJECT_UC.MIN, unary);

                UnaryComparisonConditionalFormattingRule unary1;
                unary1 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.NotEqualTo, "완료", false);

                unary1.BackColor = cSpdCellImpact_Back;
                unary1.ForeColor = cSpdCellImpact_Fore;

                this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_BAS_PROJECT_UC.WORK_STAT_NM, unary1);

                UnaryComparisonConditionalFormattingRule unary2;
                unary2 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.GreaterThan, 0, false);

                unary2.BackColor = cSpdCellImpact_Back;
                unary2.ForeColor = cSpdCellImpact_Fore;

                this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_BAS_PROJECT_UC.REWORK_CNT, unary2);

                UnaryComparisonConditionalFormattingRule unary3;
                unary3 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.GreaterThan, 0, false);

                unary3.BackColor = cSpdCellImpact_Back;
                unary3.ForeColor = cSpdCellImpact_Fore;

                this.ssMain.ActiveSheet.SetConditionalFormatting(-1, (int)enmSel_BAS_PROJECT_UC.RT_MNGM_NO, unary3);

                spread.setSpdFilter(spd, (int)enmSel_BAS_PROJECT_UC.WORK_STAT   , AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_BAS_PROJECT_UC.WORK_DVSN_NM, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_BAS_PROJECT_UC.CMPL_PSNM, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_BAS_PROJECT_UC.FORMCD      , AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_BAS_PROJECT_UC.WORK_DVSN_NM, AutoFilterMode.EnhancedContextMenu, true);
                spread.setSpdFilter(spd, (int)enmSel_BAS_PROJECT_UC.RQST_PSNM   , AutoFilterMode.EnhancedContextMenu, true);





            }
        }
    }
}
