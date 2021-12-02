using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET09.cs
    /// Description     : 영상의학과 한글명칭 영문으로 변환코드 등록폼
    /// Author          : 윤조연
    /// Create Date     : 2018-01-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\XuMain\XuMain25.frm(FrmEngCodeEntry) >> frmComSupXraySET09.cs 폼이름 재정의" />
    public partial class frmComSupXraySET09 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cxreadSql = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSupXraySQL.cBas_Z300Font cBas_Z300Font = null;

        #endregion

        public frmComSupXraySET09()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;            

            screen_clear();

            //btnSave2.Enabled = true;

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);


            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnSave.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);

            this.txtHan.KeyDown += new KeyEventHandler(eTxtEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //            
                cxraySpd.sSpd_enmXraySET09(ssList, cxraySpd.sSpdenmXraySET09, cxraySpd.nSpdenmXraySET09, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                //
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
            if (sender == this.btnSearch)
            {
                //
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon,"등록");
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

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (sender == this.ssList)
            {
                //gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayContrast.ROWID].Text.Trim();
                
            }

        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            if (sender == this.txtHan)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave.Enabled = true;
                }
            }

        }

        void eSave(PsmhDb pDbCon, string argJob, int argRow = -1)
        {
            int i = 0;
            int nstartRow = 0;
            int nLastRow = -1;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수                        


            if (ComFunc.MsgBoxQ("변경한것을 " + argJob + "처리 하시겠습니까 ?", "등록확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            read_sysdate();

            if (argRow == -1)
            {
                nLastRow = ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
            }
            else
            {
                nstartRow = argRow;
                nLastRow = argRow + 1;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                for (i = nstartRow; i < nLastRow; i++)
                {
                    
                    #region // 변수세팅 및 자료 저장

                    cBas_Z300Font = new clsComSupXraySQL.cBas_Z300Font();

                    cBas_Z300Font.Z300CODE = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.Han].Text.Trim();
                    cBas_Z300Font.ENGNAME = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.Eng].Text.Trim().ToUpper();
                    cBas_Z300Font.ENGNAME_old = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.Eng_Old].Text.Trim();
                    cBas_Z300Font.ROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.ROWID].Text.Trim();

                    if (cBas_Z300Font.ENGNAME != cBas_Z300Font.ENGNAME_old)
                    {
                        if (argJob == "등록")
                        {
                            SqlErr = cxraySql.up_BAS_Z300FONT(pDbCon, cBas_Z300Font,ref intRowAffected);

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


            //조회            
            screen_display();
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, txtHan.Text.Trim());
        }

        void screen_clear()
        {
            //
            read_sysdate();

            btnSave.Enabled = false;
                     
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
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }
                else if (ctl is DateTimePicker)
                {
                    if (((DateTimePicker)ctl).Name == "dtpDate")
                    {
                        ((DateTimePicker)ctl).Text = "";
                    }

                }

            }


        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argHan)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            
            #region // class 초기화 , 변수 설정
            cBas_Z300Font = new clsComSupXraySQL.cBas_Z300Font();
            cBas_Z300Font.Search = argHan;

            #endregion

            dt = cxraySql.sel_BAS_Z300FONT(pDbCon, cBas_Z300Font);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.Han].Text = dt.Rows[i]["Z300Code"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.Eng].Text = dt.Rows[i]["EngName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.Eng_Old].Text = dt.Rows[i]["EngName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET09.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            
        }


    }
}
