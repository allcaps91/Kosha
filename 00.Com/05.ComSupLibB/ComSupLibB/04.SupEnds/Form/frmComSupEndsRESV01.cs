using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsRESV01.cs
    /// Description     : 내시경 가예약 등록
    /// Author          : 윤조연
    /// Create Date     : 2017-08-25 
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 xxx.frm(xxx) 폼 frmComSupEndsRESV01.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\.frm >> frmComSupEndsRESV01.cs 폼이름 재정의" />
    public partial class frmComSupEndsRESV01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsComSup sup = new clsComSup();
        clsQuery Query = new clsQuery();
        clsComSupEndsSpd csupendSpd = new clsComSupEndsSpd();
        clsComSupEndsSQL csupendSQL = new clsComSupEndsSQL();        

        clsComSupEndsSQL.cEndoResvJupmst cEndoResvJupmst = null;

        string gROWID = "";

        #endregion

        public frmComSupEndsRESV01()
        {
            InitializeComponent();

            setEvent();
        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnNew.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.ssList1.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.txtTime.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtSName.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtDrName.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtSearch.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.dtpDate1.KeyDown += new KeyEventHandler(eTxtKeyDown);
            

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch)
            {
                //
                screen_display();
            }
            else if (sender == this.btnNew)
            {
                screen_clear();
                btnSave.Enabled = true;                
                dtpDate.Focus();
            }
            else if (sender == this.btnCancel)
            {
                screen_clear();
            }
            else if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon, "저장");
            }
            else if (sender == this.btnDelete)
            {
                eSave(clsDB.DbCon,"삭제");
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
                //            
                csupendSpd.sSpd_enmSupEndsResv(ssList1, csupendSpd.sSpdenmSupEndsResv, csupendSpd.nSpdenmSupEndsResv, 5, 0);
                ssList2.VerticalScrollBarWidth = 10;

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등           


                screen_clear();

                //
                screen_display();
                screen_display2();
            }
            
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            
            FpSpread s = (FpSpread)sender;                      

            if (e.Row < 0 || e.Column < 0) return;

            
            if (sender == this.ssList1)
            {
                screen_clear();

                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(s, e.Column);
                    return;
                }
                else
                {                    
                    dtpDate1.Text = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.RDate].Text.Trim(); 
                    txtTime.Text = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.RTime].Text.Trim();
                    txtPano.Text = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.Pano].Text.Trim();
                    txtSName.Text = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.SName].Text.Trim();
                    txtDrName.Text = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.DrName].Text.Trim();
                    txtRemark.Text = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.Remark].Text.Trim();

                    gROWID = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsResv.ROWID].Text.Trim();

                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            else if (sender == this.ssList2)
            {
                txtDrName.Text = s.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                txtRemark.Focus();
            }

        }       

        void eSave(PsmhDb pDbCon,string Job)
        {          
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

           
            if (dtpDate1.Text.Trim() == "") return;

            //확인 메시지
            if (Job == "삭제")
            {                
                if (ComFunc.MsgBoxQ("해당대상자를 정말로 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅

            cEndoResvJupmst = new clsComSupEndsSQL.cEndoResvJupmst();            
            cEndoResvJupmst.RDate = dtpDate1.Text.Trim();
            cEndoResvJupmst.RTime = txtTime.Text.Trim();
            cEndoResvJupmst.RDateTime = dtpDate1.Text.Trim() + " " + txtTime.Text.Trim();
            cEndoResvJupmst.DrName = txtDrName.Text.Trim();
            cEndoResvJupmst.SName = txtSName.Text.Trim();
            cEndoResvJupmst.Pano = txtPano.Text.Trim();
            cEndoResvJupmst.Remark = ComFunc.QuotConv(txtRemark.Text.Trim());

            cEndoResvJupmst.EntSabun =  Convert.ToUInt16(clsType.User.IdNumber);

            cEndoResvJupmst.ROWID = gROWID;

            #endregion

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (Job == "저장")
                {
                    if (gROWID == "")
                    {
                        SqlErr = csupendSQL.ins_ENDO_RESV_JUPMST(pDbCon, cEndoResvJupmst,  ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = csupendSQL.up_ENDO_RESV_JUPMST(pDbCon, cEndoResvJupmst,  ref intRowAffected);
                    }


                }
                else if (Job == "삭제")
                {
                    cEndoResvJupmst.DelDate = cpublic.strSysDate + " " + VB.Left(cpublic.strSysTime, 5);
                    SqlErr = csupendSQL.up_ENDO_RESV_JUPMST(pDbCon, cEndoResvJupmst,  ref intRowAffected);
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
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }                        

            screen_clear();
            screen_display();

        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Enter)
            {
                if (sender == this.dtpDate1)
                {
                    txtTime.Focus();
                }
                else if (sender == this.txtTime)
                {
                    txtPano.Focus();
                }
                else if (sender == this.txtPano)
                {
                    txtSName.Focus();
                }
                else if (sender == this.txtSName)
                {
                    txtDrName.Focus();
                }
                else if (sender == this.txtDrName)
                {
                    txtRemark.Focus();
                }
                else if (sender == this.txtSearch)
                {
                    screen_display();
                }

            }
           
        }
                
        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            gROWID = "";
            btnSave.Enabled = false;
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

            }

        }
        
        void screen_display()
        {
            GetData(clsDB.DbCon, ssList1, dtpDate.Text);
        }

        void screen_display2()
        {
            GetData2(clsDB.DbCon, ssList2);
        }

        void GetData(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd, string argSDate)
        {

            int i = 0;           
            DataTable dt = null;
            
            Spd.ActiveSheet.RowCount = 0;
            #region
            cEndoResvJupmst = new clsComSupEndsSQL.cEndoResvJupmst();
            cEndoResvJupmst.RDate = argSDate;
            cEndoResvJupmst.Search = txtSearch.Text.Trim();
            #endregion
            dt = csupendSQL.sel_ENDO_RESV_JUPMST(pDbCon, cEndoResvJupmst);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                               

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.RDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.RTime].Text = dt.Rows[i]["RTime"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.RDateTime].Text = dt.Rows[i]["RDateTime"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.DrName].Text = dt.Rows[i]["RDrName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.EntSabun].Text = dt.Rows[i]["EntSabun"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsResv.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

        void GetData2(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
            Spd.ActiveSheet.RowCount = 0;

            DataTable dt = Query.Get_BasBcode(pDbCon, "C#_ENDO_예약의사명", "", " Name ","", " Code ");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["Name"].ToString().Trim();
                }                
            }

            dt.Dispose();
            dt = null;
      
        }

    }
}
