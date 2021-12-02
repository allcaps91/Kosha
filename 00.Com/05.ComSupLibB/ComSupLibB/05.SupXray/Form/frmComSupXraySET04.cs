using ComBase;
using ComBase.Mvc.Utils;
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
    /// File Name       : frmComSupXraySET04.cs
    /// Description     : 영상의학과 조영제 부작용 등록폼
    /// Author          : 윤조연
    /// Create Date     : 2017-10-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xumain\Frm조영제부작용.frm(Frm조영제부작용) >> frmComSupXraySET04.cs 폼이름 재정의" />
    public partial class frmComSupXraySET04 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupXrayRead xreadSql = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSup.cBasPatient cBasPatient = null;
        clsComSupXraySQL.cXrayContrast cXrayContrast = null;

        string gROWID = "";

        #endregion

        public frmComSupXraySET04()
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
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnSearch);            
            this.btnSave1.Click += new EventHandler(eBtnSave);
            this.btnDelete.Click += new EventHandler(eBtnSave);
            
            this.txtPano.KeyDown += new KeyEventHandler(eTxtEvent);            

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
                xraySpd.sSpd_XrayContrast(ssList, xraySpd.sSpdXrayContrast, xraySpd.nSpdXrayContrast, 10, 0);

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
            else if (sender == this.btnNew)
            {
                gROWID = "";
                btnSave1.Enabled = true;
            }
            else if (sender == this.btnCancel)
            {
                screen_clear();
                btnNew.Enabled = true;
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
            if (sender == this.btnSave1)
            {
                eSave(clsDB.DbCon, gROWID , "1");
            }
            else if (sender == this.btnDelete)
            {
                if (MessageUtil.Confirm("정말 삭제 하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
                eSave(clsDB.DbCon, gROWID, "2");
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
                gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayContrast.ROWID].Text.Trim();
                txtPano.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayContrast.Pano].Text.Trim();
                dtpDate.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayContrast.BDate].Text.Trim();
                read_pano_info(clsDB.DbCon, txtPano.Text.Trim());
                txtRemark.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayContrast.Remark].Text.Trim();

                btnSave1.Enabled = true;
            }
            
        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    read_pano_info(clsDB.DbCon, txtPano.Text.Trim());
                }
            }

        }

        void eSave(PsmhDb pDbCon, string argROWID, string SDGu)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (txtPano.Text.Trim() =="")
            {
                ComFunc.MsgBox("등록번호 공란입니다..");
                return;
            }


            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅
            cXrayContrast = new clsComSupXraySQL.cXrayContrast();
            cXrayContrast.BDate = dtpDate.Text.Trim();
            cXrayContrast.Pano = txtPano.Text.Trim();
            cXrayContrast.SName = txtSName.Text.Trim();
            cXrayContrast.Remark = ComFunc.QuotConv(txtRemark.Text.Trim());
            cXrayContrast.EntSabun =  Convert.ToInt32(clsType.User.IdNumber);
            cXrayContrast.ROWID = argROWID;   

            #endregion

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                if(SDGu == "1")
                {
                    if (cXrayContrast.ROWID != "")
                    {
                        SqlErr = xraySql.up_XRAY_CONTRAST(pDbCon, cXrayContrast, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = xraySql.ins_XRAY_CONTRAST(pDbCon, cXrayContrast, ref intRowAffected);
                    }
                }
                else if(SDGu == "2")
                {
                    if (cXrayContrast.ROWID != "")
                    {
                        SqlErr = xraySql.del_XRAY_CONTRAST(pDbCon, cXrayContrast, ref intRowAffected);
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

            txtPano.Text = argPano;
            txtSName.Text = cBasPatient.SName;
            txtJumin.Text = cBasPatient.JuminFull;
        }

        void screen_display()
        {
            GetData(clsDB.DbCon,ssList,dtpFDate.Text.Trim(),dtpTDate.Text.Trim(),txtSearch.Text.Trim());
        }

        void screen_clear()
        {
            //
            read_sysdate();

            gROWID = "";
            btnSave1.Enabled = false;

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
                    if (((DateTimePicker)ctl).Name =="dtpDate")
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

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argFDate, string argTDate,string argSearch)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            ssList.Enabled = false;

            #region // class 초기화 , 변수 설정
            cXrayContrast = new clsComSupXraySQL.cXrayContrast();
            cXrayContrast.Date1 = argFDate;
            cXrayContrast.Date2 = argTDate;
            cXrayContrast.Search = argSearch;
            #endregion

            dt = xraySql.sel_XRAY_CONTRAST(pDbCon, cXrayContrast);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayContrast.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();                   
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayContrast.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayContrast.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayContrast.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayContrast.EntSabun].Text = dt.Rows[i]["EntSabun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayContrast.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            ssList.Enabled = true;
        }

    }
}
