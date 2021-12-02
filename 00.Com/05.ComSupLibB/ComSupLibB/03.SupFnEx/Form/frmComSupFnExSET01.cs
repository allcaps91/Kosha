using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupEXSET01.cs
    /// Description     : 기능검사 ABR 스케쥴 등록 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-18
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 ABR 스케쥴 조회 명단폼 frmComSupFnExSET01.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\ekg\.frm() >> frmComSupFnExSET01.cs 폼이름 재정의" />
    public partial class frmComSupFnExSET01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        ComFunc fun = new ComFunc();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSupFnExSpd fnexSpd = new clsComSupFnExSpd();
        clsComSupFnExSQL fnexSql = new clsComSupFnExSQL();

        string gPart = ""; //01:ABR, 02:외과 유방

        string gROWID = "";

        #endregion

        public frmComSupFnExSET01()
        {
            InitializeComponent();
            gPart = "02";
            setEvent();

        }

        public frmComSupFnExSET01(string argPart)
        {
            InitializeComponent();
            gPart = argPart;
            setEvent();

        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;


            //생성자에 따라 타이틀 표시
            if (gPart == "01")
            {
                lblTitle.Text = "ABR 스케쥴 조회";
                
            }
            else if (gPart == "02")
            {
                lblTitle.Text = "외과 유방검사 스케쥴 조회";
                
            }
            else
            {
                lblTitle.Text = "ABR 스케쥴 조회";
                
            }

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnSearch1.Click += new EventHandler(eBtnEvent);            
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnNew.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.txtPano.KeyDown += eTxtKeyDown;
            this.dtpFDate.TextChanged += eDtpTxtChange;


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
                fnexSpd.sSpd_AbrEnt(ssList, fnexSpd.sSpdAbrEnt, fnexSpd.nSpdAbrEnt, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                screen_clear();

                setCtrlData();

                //기본 조회
                screen_display();
            }


        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSave)
            {
                //저장
                eSave(clsDB.DbCon,gPart);

            }
            else if (sender == this.btnDelete)
            {
                //삭제
                eDelete(clsDB.DbCon);
            }
            else if (sender == this.btnNew)
            {
                //신규
                screen_clear("1");
                btnSave.Enabled = true;
                panel5.Enabled = true;
                txtPano.Focus();
            }
            else if (sender == this.btnSearch1)
            {
                screen_display();
            }


        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtPano.Text.Trim();
                    if(strPano !="") read_pano_info(clsDB.DbCon, ComFunc.SetAutoZero(strPano,ComNum.LENPTNO));
                }

            }

        }

        void eDtpTxtChange(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text.Trim();
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            gROWID = "";

            screen_clear("1");

            if (e.Row < 0 || e.Column < 0) return;                     
            
            gROWID = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupFnExSpd.enmAbrEnt.ROWID].Text.Trim();

            if (gROWID!="")
            {
                read_one_data_display(clsDB.DbCon,gPart, gROWID);
                btnSave.Enabled = true;
                panel5.Enabled = true;
            }            

        }

        void eSave(PsmhDb pDbCon,string argPart)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strPano = ComFunc.SetAutoZero(txtPano.Text.Trim(), ComNum.LENPTNO);
            string strExDate = dtpDate.Text.Trim() + "" + dtpTime.Text.Trim();
            string strExName = txtExName.Text.Trim();
            string strDrptcode = "";


            if (strExDate == "" || strExName == "")
            {
                ComFunc.MsgBox("데이타 공란이 있습니다.. 확인하세요!!", "값공란");
                return;
            }
            if (argPart == "02")
            {
                strDrptcode = "GS";
            }
            read_sysdate();

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon); 

            try
            {
                //자료있으면 삭제                        
                if (gROWID != "")
                {
                    SqlErr = fnexSql.del_EXAM_ABR_SCHEDULE(pDbCon, gROWID,  ref intRowAffected);
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
                    return;
                }

                //저장
                SqlErr = fnexSql.ins_EXAM_ABR_SCHEDULE(pDbCon,argPart, strPano, strExDate, strExName, Convert.ToInt32(clsType.User.IdNumber), strDrptcode, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
                    return;
                }

                if (SqlErr == "")
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

            //조회            
            screen_display();

        }

        void eDelete(PsmhDb pDbCon)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int i = 0;
            string strROWID = "";
                                    
            if (ComFunc.MsgBoxQ("선택한 대상을 삭제 하시겠습니까??", "삭제처리", MessageBoxDefaultButton.Button2) == DialogResult.No) return;


            read_sysdate();

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            for (i = 0; i <= ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                if (ssList_Sheet1.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.Chk].Text.Trim() == "True")
                {
                    strROWID = ssList_Sheet1.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.ROWID].Text.Trim();

                    if (strROWID != "")
                    {
                        SqlErr = fnexSql.del_EXAM_ABR_SCHEDULE(pDbCon, strROWID,  ref intRowAffected);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장                            
                        return;
                    }
                }
            }

            clsDB.setCommitTran(pDbCon);

            screen_clear();

            //조회            
            screen_display();

        }

        string read_bas_Jumin(PsmhDb pDbCon, string argPano)
        {
            DataTable dt = sup.sel_Bas_Patient(pDbCon, argPano);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                return  dt.Rows[0]["JuminFull"].ToString().Trim();                
            }
            else
            {
                return "";
            }                       
        }

        void read_one_data_display(PsmhDb pDbCon,string argPart, string argROWID)
        {
            DataTable dt = fnexSql.sel_ExamABRSch(pDbCon, argPart, "", "", argROWID);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                read_pano_info(pDbCon, dt.Rows[0]["Pano"].ToString().Trim());

                txtPano.Text = dt.Rows[0]["Pano"].ToString().Trim();
                dtpDate.Text = dt.Rows[0]["RDate2"].ToString().Trim();
                dtpTime.Text = dt.Rows[0]["RDate3"].ToString().Trim();
                
                txtExName.Text = dt.Rows[0]["ExamName"].ToString().Trim();

            }
        }

        void read_pano_info(PsmhDb pDbCon,string argPano)
        {
            string strJumin = read_bas_Jumin(pDbCon, argPano);
            strJumin = strJumin.Replace("-", "");
            txtSName.Text = fun.Read_Patient(pDbCon, argPano, "2");
            txtAge.Text = ComFunc.AgeCalc(pDbCon, strJumin).ToString();
            txtSex.Text = ComFunc.SexCheck(strJumin, "1");

        }

        void screen_clear(string Job="")
        {
            read_sysdate();

            btnSave.Enabled = false;
            panel5.Enabled = false;

            gROWID = "";

            if (Job =="")
            {
                dtpFDate.Text = cpublic.strSysDate;
                dtpTDate.Text = Convert.ToDateTime(cpublic.strSysDate).AddDays(10).ToShortDateString();                
            }

            dtpDate.Text = "";
            dtpTime.Text = "";

            txtPano.Text = "";
            txtSName.Text = "";
            txtSex.Text = "";
            txtAge.Text = "";
            txtExName.Text = "";


        }        
        
        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argPart, string argFDate, string argTDate)
        {
            int i = 0;            
            DataTable dt = null;
            string strJumin = "";


            read_sysdate();

            screen_clear("1");

            Spd.ActiveSheet.RowCount = 0;
                     
            dt = fnexSql.sel_ExamABRSch(pDbCon,argPart, argFDate, argTDate);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
               
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.Chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.ExDate].Text = dt.Rows[i]["RDate2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.ExTime].Text = dt.Rows[i]["RDate3"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.SName].Text = clsVbfunc.GetPatientName(pDbCon, dt.Rows[i]["Pano"].ToString().Trim());

                    strJumin = read_bas_Jumin(pDbCon, dt.Rows[i]["Pano"].ToString().Trim());
                    strJumin = strJumin.Replace("-", "");                   
               
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.SexAge].Text = ComFunc.SexCheck(strJumin, "1") + "/" + ComFunc.AgeCalc(pDbCon, strJumin).ToString();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.ExName].Text = dt.Rows[i]["ExamName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.EntSabun].Text = dt.Rows[i]["WriteSabun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.EntName].Text = fun.Read_SabunName(pDbCon, dt.Rows[i]["WriteSabun"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmAbrEnt.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
            }

            dt.Dispose();
            dt = null;
            
            Cursor.Current = Cursors.Default;

            #endregion


        }        

        void screen_display()
        {
            GetData( clsDB.DbCon, ssList, gPart, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }


    }
}
