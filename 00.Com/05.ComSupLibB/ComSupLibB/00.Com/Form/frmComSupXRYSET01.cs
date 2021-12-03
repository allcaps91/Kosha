using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupXRYSET01.cs
    /// Description     : 진료지원 사용자 상용문구 사용 폼
    /// Author          : 윤조연
    /// Create Date     : 2017-06-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmResultWard.frm,Xuread05.frm(FrmResultWard,FrmUseWard) 폼 frmComSupXRYSET01.cs 으로 변경함(2개폼 통합예정)
    /// </history>
    /// <seealso cref=  "\FrmResultWard.frm,\xray\xuread\XuRead05.frm >> frmComSupXRYSET01.cs 폼이름 재정의" />
    public partial class frmComSupXRYSET01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupSpd cSpd = new clsComSupSpd();

        clsComSupXrayRead.cXray_Read_Delegate cXray_Read_Delegate = null; //델리게이트

        public delegate void SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls);
        public event SendMsg rSendMsg;

        string gstrJob = "";//작업구분(TO:종검,XRAY:영상)
        long gSabun = 0;

       
        #endregion

        public frmComSupXRYSET01(string argJob,long argSabun)
        {
            InitializeComponent();

            gstrJob = argJob;
            gSabun = argSabun;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            lblDrName.Text = sup.read_bas_user(clsDB.DbCon, gSabun.ToString());

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
                        
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch1.Click += new EventHandler(eBtnSearch);
            this.btnSearch2.Click += new EventHandler(eBtnSearch);
            this.btnSearch3.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);

            //this.btnPrint.Click += new EventHandler(eBtnEvent);

            //명단 더블클릭 이벤트
            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList.MouseUp += new MouseEventHandler(eMouseEvent);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);
            //this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);


            this.ssList.KeyDown += new KeyEventHandler(eTxtKeyDown);
            
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
                cSpd.sSpd_ResultSetUse(ssList, cSpd.sSpdResultSetUse, cSpd.nSpdResultSetUse, 10, 0, gstrJob);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();


                //
                if (gstrJob == "TO")
                {
                    GetDataTO(clsDB.DbCon, ssList);
                }
                else if (gstrJob == "XRAY")
                {
                    btnSearch1.Visible = false;
                    btnSearch2.Visible = false;
                    btnSearch3.Visible = false;
                    GetDataXray(clsDB.DbCon, ssList);
                }

                ssList.Focus();
                ssList.Select();
            }
            
            
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch3)
            {
                //
                eSeq();
            }
            
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon);
                                
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //ePrint();
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;
            string s = string.Empty;


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
                    //clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                else
                {
                    setDelegate(o, e.Row);
                }

            }

        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.ssList )
            {                
                int nRow = 0;

                if (e.KeyCode == Keys.F1)
                {
                    nRow = 1;
                }
                else if (e.KeyCode == Keys.F2)
                {
                    nRow = 2;
                }
                else if (e.KeyCode == Keys.F3)
                {
                    nRow = 3;
                }
                else if (e.KeyCode == Keys.F4)
                {
                    nRow = 4;
                }
                else if (e.KeyCode == Keys.F5)
                {
                    nRow = 5;
                }
                else if (e.KeyCode == Keys.F6)
                {
                    nRow = 6;
                }
                else if (e.KeyCode == Keys.F7)
                {
                    nRow = 7;
                }
                else if (e.KeyCode == Keys.F8)
                {
                    nRow = 8;
                }
                else if (e.KeyCode == Keys.F9)
                {
                    nRow = 9;
                }

                if (nRow > 0)
                {
                    setDelegate(ssList, nRow-1);
                }

                    
            }

        }

        void setDelegate(FpSpread Spd, int argRow)
        {
            if (rSendMsg == null)
            {
                return;
            }
            
            string s = Spd.ActiveSheet.Cells[argRow, (int)clsComSupSpd.enmResultSetUse.Remark].Text.Trim();
            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = "";
            cXray_Read_Delegate.Sogen = s + "\r\n";

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            this.Hide();
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            //txtSearch.Text = "";   
            //dtpFDate.Text =cpublic.strSysDate;  

        }

        void GetDataTO(PsmhDb pDbCon, FpSpread Spd)
        {

            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = sup.sel_Resultward(pDbCon, gSabun,"", true);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count+10;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Remark].Text = dt.Rows[i]["WardName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

        void GetDataXray(PsmhDb pDbCon, FpSpread Spd)
        {

            int i = 0;
            int nRow = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            //쿼리실행      
            dt = sup.sel_Resultward(pDbCon,gSabun,"");

            #region //데이터셋 읽어 자료 표시

            Spd.ActiveSheet.RowCount = 10;

            if (dt == null) return;

            if (dt.Rows.Count == 0)
            {
                for ( i = 0; i < 10; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Key].Text = "F" + (i + 1).ToString();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Code].Text = "F" + (i + 1).ToString();
                }
                

                return;
            }

            if (dt.Rows.Count > 0)
            {
                
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < 10; i++)
                {

                    if (i < dt.Rows.Count)
                    {

                        nRow = Code2Row(dt.Rows[i]["Code"].ToString().Trim());
                        

                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmResultSetUse.Key].Text = dt.Rows[i]["Code"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmResultSetUse.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmResultSetUse.Remark].Text = dt.Rows[i]["WardName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupSpd.enmResultSetUse.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Key].Text = "F" + (i + 1).ToString();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Code].Text = "F" + (i + 1).ToString();
                    }

                    Spd.ActiveSheet.Rows.Get(i).Height = Spd.ActiveSheet.Rows[i].GetPreferredHeight();
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

        void eSeq()
        {

            if (gstrJob != "TO") return;
            
            int nCnt = 0;
            int cRow = 0;

            for (int i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                if (nCnt < Convert.ToInt16(ssList.ActiveSheet.Cells[i,(int)clsComSupSpd.enmResultSetUse.Code].Text))
                {
                    nCnt = Convert.ToInt16(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Code].Text);
                    cRow = i;                    
                }
            }

            ssList.ActiveSheet.Cells[cRow + 1, (int)clsComSupSpd.enmResultSetUse.Code].Text = ComFunc.SetAutoZero((nCnt+1).ToString(),5)  ;

        }

        int Code2Row(string argCode)
        {
            int i = -1;

            if (argCode =="F1")
            {
                i = 0;
            }
            else if (argCode == "F2")
            {
                i = 1;
            }
            else if (argCode == "F3")
            {
                i = 2;
            }
            else if (argCode == "F4")
            {
                i = 3;
            }
            else if (argCode == "F5")
            {
                i = 4;
            }
            else if (argCode == "F6")
            {
                i = 5;
            }
            else if (argCode == "F7")
            {
                i = 6;
            }
            else if (argCode == "F8")
            {
                i = 7;
            }
            else if (argCode == "F9")
            {
                i = 8;
            }
            else if (argCode == "F10")
            {
                i = 9;
            }
            else
            {
                i = -1;
            }

            return i;
        }

        void eSel()
        {
            for (int i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                
            }
        }
        
        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strCODE ="";
            string strWARD ="";
            string strROWID="";
            string strChk = "";

            long nSabun = Convert.ToInt32(clsType.User.Sabun);

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                for (int i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strChk = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Chk].Text.Trim();
                    strCODE = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Code].Text.Trim();
                    strWARD = ComFunc.QuotConv(ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Remark].Text.Trim());
                    strROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.ROWID].Text.Trim();

                    if (strChk == "True" && strROWID != "" && gstrJob == "TO")
                    {
                        //삭제
                        SqlErr = sup.del_RESULTWARD(pDbCon, "HIC_RESULTWARD", strROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        ssList.ActiveSheet.Cells[i, (int)clsComSupSpd.enmResultSetUse.Chk].Text = "";

                    }
                    else if (strChk != "True" && strROWID == "" && strWARD != "" && strCODE != "")
                    {
                        //신규
                        if (gstrJob == "TO")
                        {
                            SqlErr = sup.ins_RESULTWARD(pDbCon, "HIC_RESULTWARD", nSabun, strCODE, strWARD, ref intRowAffected);
                        }
                        else if (gstrJob == "XRAY")
                        {
                            SqlErr = sup.ins_RESULTWARD(pDbCon, "XRAY_RESULTWARD", nSabun, strCODE, strWARD, ref intRowAffected);
                        }
                                                
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }
                    else if (strChk != "True" && strROWID != "")
                    {
                        //갱신               
                        if (gstrJob == "TO")
                        {
                            SqlErr = sup.up_RESULTWARD(pDbCon, "HIC_RESULTWARD", strROWID, strWARD, ref intRowAffected);
                        }
                        else if (gstrJob == "XRAY")
                        {
                            SqlErr = sup.up_RESULTWARD(pDbCon, "XRAY_RESULTWARD", strROWID, strWARD, ref intRowAffected);
                        }

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                }
                if (SqlErr =="")
                {
                    clsDB.setCommitTran(pDbCon);
                }
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
            

            //
            if (gstrJob == "TO")
            {
                GetDataTO(pDbCon, ssList);
            }
            else if (gstrJob == "XRAY")
            {
                GetDataXray(pDbCon, ssList);
            }

        }                        
                        
    }
}
