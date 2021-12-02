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
    /// File Name       : frmComSupXraySET08.cs
    /// Description     : 영상의학과 콜시간상세 등록폼
    /// Author          : 윤조연
    /// Create Date     : 2018-01-22
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuilgi\frmAngio3.frm(FrmAngio3) >> frmComSupXraySET08.cs 폼이름 재정의" />
    public partial class frmComSupXraySET08 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead xreadSql = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSup.cBasPatient cBasPatient = null;
        clsComSupXraySQL.cXray_Ilgi_Angio_Sub cXray_Ilgi_Angio_Sub = null;

        string gROWID = "";

        #endregion

        public frmComSupXraySET08()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {            
            screen_clear();
            
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
            this.btnDelete.Click += new EventHandler(eBtnSave);

            //this.txtPano.KeyDown += new KeyEventHandler(eTxtEvent);

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
                cxraySpd.sSpd_enmXraySET08(ssList, cxraySpd.sSpdenmXraySET08, cxraySpd.nSpdenmXraySET08, 10, 0);

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
            else if (sender == this.btnCancel)
            {
                screen_clear();                
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
                eSave(clsDB.DbCon, "저장");
            }
            else if (sender == this.btnDelete)
            {
                //
                eSave(clsDB.DbCon, "삭제");
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
            gROWID = "";

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
                gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.ROWID].Text.Trim();

                if (gROWID !="")
                {
                    lblSts.Text = "수정";
                    lblSts.BackColor = System.Drawing.Color.LightGreen;
                }
                
                txtSName.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.SName].Text.Trim();
                dtpSDate.Text =  VB.Left(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.SDate].Text.Trim(),10);
                dtpSTime.Text = VB.Right(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.SDate].Text.Trim(),5);
                dtpEDate.Text = VB.Left(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.EDate].Text.Trim(),10);
                dtpETime.Text = VB.Right(o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.EDate].Text.Trim(),5);
                txtRemark.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXraySET08.Remark].Text.Trim();
                
                btnSave.Enabled = true;
                btnDelete.Enabled = true;

            }

        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtPano)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        read_pano_info(clsDB.DbCon, txtPano.Text.Trim());
            //    }
            //}

        }

        void eSave(PsmhDb pDbCon,string argJob)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            #region //값 체크
            if (argJob == "삭제" && gROWID =="")
            {
                ComFunc.MsgBox("대상을 선택후 작업하세요..");
                return;
            }
            if (dtpSTime.Text.Trim() =="00:00" || dtpETime.Text.Trim() == "00:00")
            {
                ComFunc.MsgBox("검사 시간을 넣고 작업하세요..");
                return;
            }
            if (dtpSDate.Text.Trim() == "" || dtpEDate.Text.Trim() == "")
            {
                ComFunc.MsgBox("검사일자를 넣고 작업하세요..");
                return;
            }

            #endregion

            clsDB.setBeginTran(pDbCon);

            try
            {
                #region //변수설정
                cXray_Ilgi_Angio_Sub = new clsComSupXraySQL.cXray_Ilgi_Angio_Sub();
                
                cXray_Ilgi_Angio_Sub.BDate = dtpDate.Text.Trim();
                cXray_Ilgi_Angio_Sub.Call_Time1 = dtpSDate.Text.Trim() + " " + dtpSTime.Text.Trim();
                cXray_Ilgi_Angio_Sub.Call_Time2 = dtpTDate.Text.Trim() + " " + dtpETime.Text.Trim();
                cXray_Ilgi_Angio_Sub.ROWID = gROWID;
                cXray_Ilgi_Angio_Sub.Code = "X001";
                cXray_Ilgi_Angio_Sub.DamName = txtSName.Text.Trim();
                cXray_Ilgi_Angio_Sub.Remark = txtRemark.Text.Trim();
                #endregion

                if (argJob =="저장")
                {
                    cXray_Ilgi_Angio_Sub.Job = "00";

                    if (cXray_Ilgi_Angio_Sub.ROWID != "")
                    {
                        SqlErr = cxraySql.up_XRAY_ILGI_ANGIO_SUB(pDbCon, cXray_Ilgi_Angio_Sub, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cxraySql.ins_XRAY_ILGI_ANGIO_SUB(pDbCon, cXray_Ilgi_Angio_Sub, ref intRowAffected);
                    }

                }
                else if (argJob == "삭제")
                {
                    cXray_Ilgi_Angio_Sub.Job = "01";

                    if (cXray_Ilgi_Angio_Sub.ROWID != "")
                    {
                        SqlErr = cxraySql.up_XRAY_ILGI_ANGIO_SUB(pDbCon, cXray_Ilgi_Angio_Sub, ref intRowAffected);
                    }
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);
                }

                screen_clear();
                screen_display();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                return;
            }


        }

        void read_pano_info(PsmhDb pDbCon, string argPano)
        {
            cBasPatient = sup.sel_Bas_Patient_cls(pDbCon, argPano);

            //txtPano.Text = argPano;
            //txtSName.Text = cBasPatient.SName;
            //txtJumin.Text = cBasPatient.JuminFull;
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void screen_clear()
        {
            //
            read_sysdate();

            gROWID = "";
            //btnSave.Enabled = false;
            btnDelete.Enabled = false;

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
                    if (((DateTimePicker)ctl).Name == "dtpSDate" || ((DateTimePicker)ctl).Name == "dtpEDate")
                    {
                        ((DateTimePicker)ctl).Text = "";
                    }

                }

            }

            dtpSTime.Text = "00:00";
            dtpETime.Text = "00:00";
            lblSts.Text = "신규";
            lblSts.BackColor = System.Drawing.Color.Transparent;

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argFDate, string argTDate)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
           
            cXray_Ilgi_Angio_Sub = new clsComSupXraySQL.cXray_Ilgi_Angio_Sub();
            cXray_Ilgi_Angio_Sub.Job = "01";
            cXray_Ilgi_Angio_Sub.Date1 = argFDate;
            cXray_Ilgi_Angio_Sub.Date2 = argTDate;
            cXray_Ilgi_Angio_Sub.Code = "X";

            dt = cxraySql.sel_XRAY_ILGI_ANGIO_SUB(pDbCon, cXray_Ilgi_Angio_Sub);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET08.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET08.SName].Text = dt.Rows[i]["DamName"].ToString().Trim();                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET08.SDate].Text = dt.Rows[i]["Call_Time1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET08.EDate].Text = dt.Rows[i]["Call_Time2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET08.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXraySET08.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
          
        }

    }
}
