using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsSET02.cs
    /// Description     : 내시경 병력 및 약물 히스토리 입력폼
    /// Author          : 윤조연
    /// Create Date     : 2017-09-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\ocs\endo\endo_new\Frm내시경입력.frm(Frm내시경입력) >> frmComSupEndsSET02.cs 폼이름 재정의" />
    /// 
    public partial class frmComSupEndsSET02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupEndsSpd endsSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL endsSql = new clsComSupEndsSQL();

        clsComSupEndsSQL.cEndoAddHis cEndoAddHis = null;

        string gPano = "";
        string gRDate = "";

        #endregion

        public frmComSupEndsSET02(string argPano, string argRDate)
        {
            InitializeComponent();

            gPano = argPano;
            gRDate = argRDate;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            lblSTS.Text = titleSTS("신규");
            lblSTS.BackColor = System.Drawing.Color.White;

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnSave);
            
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
                //endsSpd.sSpd_enmTelview(ssList, endsSpd.sSpdTelview, endsSpd.nSpdTelview, 1, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();

                screen_display();
                
            }

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
            
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSave(PsmhDb pDbCon)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            DataTable dt = null;

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                #region //클래스 생성 및 변수세팅
                cEndoAddHis = new clsComSupEndsSQL.cEndoAddHis();
                cEndoAddHis.Ptno = gPano;
                cEndoAddHis.RDate = gRDate;
                cEndoAddHis.OLD_0 = chkOld0.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_1 = chkOld1.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_2 = chkOld2.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_3 = chkOld3.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_4 = chkOld4.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_5 = chkOld5.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_6 = chkOld6.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_7 = chkOld7.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_8 = chkOld8.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_9 = chkOld9.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_10 = chkOld10.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_11 = chkOld11.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_12 = chkOld12.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_131 = chkOld13.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_132 = txtOld13.Text.Trim();
                cEndoAddHis.OLD_14 = chkOld14.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.OLD_15 = ComFunc.QuotConv(txtOldEtc.Text.Trim());

                cEndoAddHis.DRUG_0 = chkDrug0.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_1 = chkDrug1.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_2 = chkDrug2.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_3 = chkDrug3.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_4 = chkDrug4.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_5 = chkDrug5.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_6 = chkDrug6.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_7 = chkDrug7.Checked.ToString() == "True" ? "1" : "";
                cEndoAddHis.DRUG_8 = txtDrugEtc.Text.Trim();
                cEndoAddHis.DRUG_9 = ComFunc.QuotConv(txtDrugA.Text.Trim());
                cEndoAddHis.DRUG_10 = ComFunc.QuotConv(txtDrugB.Text.Trim());

                cEndoAddHis.EntSabun = clsType.User.Sabun;

                #endregion

                #region //자료저장
                dt = endsSql.sel_ENDO_ADD_HIS(pDbCon,cEndoAddHis);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    cEndoAddHis.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                if (cEndoAddHis.ROWID != "")
                {
                    //갱신
                    SqlErr = endsSql.up_ENDO_ADD_HIS(pDbCon,cEndoAddHis,  ref intRowAffected);
                }
                else
                {
                    SqlErr = endsSql.ins_ENDO_ADD_HIS(pDbCon,cEndoAddHis, ref intRowAffected);
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);
                    this.Close();
                    //ComFunc.MsgBox("저장하였습니다.");
                }
                #endregion
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
            
        }

        string titleSTS(string argSTS)
        {
            if (argSTS=="신규")
            {
                return argSTS + " " + gPano + " [예약일 : " + gRDate + " ]"; 
            }
            else if (argSTS == "수정")
            {
                return argSTS + " " + gPano + " [예약일 : " + gRDate + " ]";
            }
            else
            {
                return "대상자료 없음";
            }
        }

        void screen_display()
        {
            GetData(clsDB.DbCon,gPano, gRDate);
        }

        void screen_clear()
        {
            //
            read_sysdate();

            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    if(((CheckBox)ctl).Name == "chkOld0" || ((CheckBox)ctl).Name == "chkDrug0")
                    {
                        ((CheckBox)ctl).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    
                }                

            }


        }

        void read_sysdate()
        {
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, string argPano, string argDate)
        {            
            DataTable dt = null;
            
            screen_clear();

            #region //병력 히스토리 관련 변수 세팅
            cEndoAddHis = new clsComSupEndsSQL.cEndoAddHis();
            cEndoAddHis.Ptno = argPano;
            cEndoAddHis.RDate = argDate;
            #endregion

            //쿼리실행      
            dt = endsSql.sel_ENDO_ADD_HIS(pDbCon, cEndoAddHis);

            #region //데이터셋 읽어 자료 표시
            
            if (ComFunc.isDataTableNull(dt) == false)
            {                
                lblSTS.Text = titleSTS("수정");

                cEndoAddHis.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                #region //데이타 표시
                //병력
                if (dt.Rows[0]["GB_OLD"].ToString().Trim()=="1")
                {
                    chkOld0.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD1"].ToString().Trim() == "1")
                {
                    chkOld1.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD2"].ToString().Trim() == "1")
                {
                    chkOld2.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD3"].ToString().Trim() == "1")
                {
                    chkOld3.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD4"].ToString().Trim() == "1")
                {
                    chkOld4.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD5"].ToString().Trim() == "1")
                {
                    chkOld5.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD6"].ToString().Trim() == "1")
                {
                    chkOld6.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD7"].ToString().Trim() == "1")
                {
                    chkOld7.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD8"].ToString().Trim() == "1")
                {
                    chkOld8.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD9"].ToString().Trim() == "1")
                {
                    chkOld9.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD10"].ToString().Trim() == "1")
                {
                    chkOld10.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD11"].ToString().Trim() == "1")
                {
                    chkOld11.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD12"].ToString().Trim() == "1")
                {
                    chkOld12.Checked = true;
                }
                if (dt.Rows[0]["GB_OLD13"].ToString().Trim() == "1")
                {
                    chkOld13.Checked = true;
                }
                txtOld13.Text = dt.Rows[0]["GB_OLD13_1"].ToString().Trim();                
                if (dt.Rows[0]["GB_OLD14"].ToString().Trim() == "1")
                {
                    chkOld14.Checked = true;
                }
                txtOldEtc.Text = dt.Rows[0]["GB_OLD15_1"].ToString().Trim();

                //약물
                if (dt.Rows[0]["GB_DRUG"].ToString().Trim() == "1")
                {
                    chkDrug0.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG1"].ToString().Trim() == "1")
                {
                    chkDrug1.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG2"].ToString().Trim() == "1")
                {
                    chkDrug2.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG3"].ToString().Trim() == "1")
                {
                    chkDrug3.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG4"].ToString().Trim() == "1")
                {
                    chkDrug4.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG5"].ToString().Trim() == "1")
                {
                    chkDrug5.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG6"].ToString().Trim() == "1")
                {
                    chkDrug6.Checked = true;
                }
                if (dt.Rows[0]["GB_DRUG7"].ToString().Trim() == "1")
                {
                    chkDrug7.Checked = true;
                }
                txtDrugEtc.Text = dt.Rows[0]["GB_DRUG8_1"].ToString().Trim();

                txtDrugA.Text = dt.Rows[0]["GB_DRUG_STOP1"].ToString().Trim();
                txtDrugB.Text = dt.Rows[0]["GB_DRUG_STOP2"].ToString().Trim();

                #endregion

            }
            else
            {
                if(dt.Rows.Count ==0)
                {
                    lblSTS.Text = titleSTS("신규");
                }
                else
                {
                    lblSTS.Text = titleSTS("");
                }
            }                       

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

    }
}
