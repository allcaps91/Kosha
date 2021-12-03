using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayCODE05.cs
    /// Description     : 영상의학과 기초코드관리 - 기본사용량 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xucode\xucode03.frm(FrmUse) >> frmComSupXrayCODE05.cs 폼이름 재정의" />
    public partial class frmComSupXrayCODE05 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = null;     
        clsComSupXray cxray = null;
        clsComSupXraySpd xraySpd = null;
        clsComSupXraySQL xraySql = null;
        clsComSupSpd supSpd = null;
        clsMethod method = null;
        clsQuery Query = null;

        #endregion

        public frmComSupXrayCODE05()
        {
            InitializeComponent();

            setEvent();

        }
        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;


            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            setCombo();

            screen_clear();

        }

        void setCombo()
        {
            DataTable dt =  xraySql.sel_Xray_Class_Combo(clsDB.DbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboJong, dt, clsParam.enmComParamComboType.None);

                cboJong.SelectedIndex = -1;
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }
        }

        //권한체크
        void setAuth()
        {


        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);

            this.txtMCode.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.cboJong.SelectedIndexChanged += new EventHandler(eCboEvent);

            this.expandableSplitter1.Click += ExpandableSplitter1_Click;

            this.ssList1.ComboSelChange += new EditorNotifyEventHandler(eSpreadCboClick);            
            this.ssList1.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList1.EditChange += ssList1_EditChange;
            this.ssList2.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList3.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

        }

        void ssList1_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            ssList1.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUse.Change].Text = "Y";
        }

        void eCboEvent(object sender, EventArgs e)
        {   
            //
            try
            {
                if (cboJong != null && cboJong.SelectedItem.ToString() != "")
                {
                    GetData3(clsDB.DbCon, ssList2, cboJong.SelectedItem.ToString());
                }
            }
            catch
            {

            }
        }

        void eSpreadCboClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == this.ssList1)
            {
                if(e.Column == (int)clsComSupXraySpd.enmXrayUse.Gubun2)
                {
                    ssList1.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUse.Gubun2Name].Text = xraySql.read_Xray_Use_Gubun(clsDB.DbCon, ssList1.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUse.Gubun2].Text);
                }
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            int nRow=0;

            if (e.Row < 0 || e.Column < 0) return;

            if (sender == this.ssList2)
            {
                txtMCode.Text = ssList2.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayMCode.MCode].Text.Trim();
                txtMName.Text = ssList2.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayMCode.MCode].Text.Trim();

                GetData4(clsDB.DbCon, ssList1, ssList2.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayMCode.MCode].Text.Trim());
            }
            else if (sender == this.ssList3)
            {
                if (txtMCode.Text.Trim() !="")
                {
                    nRow = ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
                    ssList1.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayUse.MCode].Text = ssList3.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUseMCode.MCode].Text.Trim();
                    ssList1.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayUse.MName].Text = ssList3.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUseMCode.MName].Text.Trim();

                    ssList1.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayUse.Change].Text = "Y"; //수정
                }
                                
            }         
          
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            
            if (e.Row < 0 || e.Column < 0) return;

            if (sender == this.ssList1)
            {
                if (e.Column == (int)clsComSupXraySpd.enmXrayUse.Gubun2)
                {
                    ssList1.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUse.Gubun2Name].Text = xraySql.read_Xray_Use_Gubun(clsDB.DbCon, ssList1.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUse.Gubun2].Text);
                }

                if (e.Column == (int)clsComSupXraySpd.enmXrayUse.Agree || e.Column == (int)clsComSupXraySpd.enmXrayUse.chk)
                {
                    ssList1.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayUse.Change].Text = "Y"; //수정
                }

                    
            }

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {

                cpublic = new clsPublic(); //공용함수        
                cxray = new clsComSupXray();
                xraySpd = new clsComSupXraySpd();
                xraySql = new clsComSupXraySQL();
                supSpd = new clsComSupSpd();
                method = new clsMethod();
                Query = new clsQuery();

                xraySpd.sSpd_XrayUse(ssList1, xraySpd.sSpdXrayUse, xraySpd.nSpdXrayUse, 10, 0);

                xraySpd.sSpd_XrayUseJong(ssList2, xraySpd.sSpdXrayUseJong, xraySpd.nSpdXrayUseJong, 10, 0);
                xraySpd.sSpd_XrayUseMCode(ssList3, xraySpd.sSpdXrayUseMCode, xraySpd.nSpdXrayUseMCode, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                screen_display();
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnCancel)
            {
                screen_clear();
            }            
            else if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon,ssList1, "저장");
            }
            else if (sender == this.btnDelete)
            {
                //
                eSave(clsDB.DbCon,ssList1, "삭제");
            }
            else if (sender == this.btnSearch1)
            {
                //
                GetData2(clsDB.DbCon, ssList3);
            }
            
            txtMCode.Focus();

        }                

        void eSave(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string Job)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strMCode = txtMCode.Text.Trim().ToUpper();

            if (strMCode == "") return;
            if (txtMName.Text.Trim() == "") return;


            //확인 메시지
            if (Job == "삭제")
            {                
                if (ComFunc.MsgBoxQ("해당코드를 정말로 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }
                        
            clsDB.setBeginTran(pDbCon);

            try
            {
                for (int i = 0; i <= Spd.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {

                    #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅

                    xraySql.argstr = new string[Enum.GetValues(typeof(clsComSupXraySQL.enum_XrayBasUse)).Length];
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Change] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Change].Text.Trim();
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.XCode] = strMCode;
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Gubun2] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Gubun2].Text.Trim();
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.MName] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Gubun2Name].Text.Trim();
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.MCode] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.MCode].Text.Trim();
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Qty] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Qty].Text.Trim();
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Agree] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Agree].Text.Trim() == "True" ? "1" : "";
                    xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.ROWID] = Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.ROWID].Text.Trim();

                    #endregion

                    if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Change] == "Y")
                    {
                        #region //저장
                        if (Job == "저장")
                        {
                            //체크
                            if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Gubun2] == "")
                            {
                                MessageBox.Show("성인구분을 확인하세요!!");
                                return;
                            }

                            if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.Qty] == "")
                            {
                                MessageBox.Show("수량을 확인하세요!!");
                                return;
                            }


                            if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.ROWID] != "")
                            {
                                SqlErr = xraySql.up_Xray_BasUse(pDbCon, xraySql.argstr, ref intRowAffected);
                            }
                            else
                            {
                                SqlErr = xraySql.ins_Xray_BasUse(pDbCon, xraySql.argstr, ref intRowAffected);
                            }

                        }
                        #endregion

                        #region //삭제
                        else if (Job == "삭제")
                        {
                            if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayBasUse.ROWID] != "")
                            {
                                SqlErr = xraySql.del_Xray_BasUse(pDbCon, xraySql.argstr, ref intRowAffected);
                            }
                            else
                            {
                                SqlErr = "삭제할 ROWID 없음!!";
                            }

                        }
                        #endregion

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                    }

                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                }
                    
            }
            catch(Exception ex)
            {                
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                return;
            }

            
            screen_clear();

            screen_display();

        }

        void ExpandableSplitter1_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;

            if (ex.Expanded == true)
            {
                panel3.Size = new System.Drawing.Size(882, 20);
            }
            else
            {
                panel3.Size = new System.Drawing.Size(882, 280);
            }
        }

        void screen_clear()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");


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

            //cboJong.SelectedIndex = -1;

            ssList1.ActiveSheet.RowCount = 0;

            btnSave.Enabled = true;


        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList1);
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
            DataTable dt = null;
            
            Spd.ActiveSheet.RowCount = 0;
            if (cboJong.SelectedItem == null)
            {
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            ssList2.Enabled = false;

            dt = xraySql.sel_Xray_MCode(pDbCon, "", cboJong.SelectedItem.ToString());

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {                    
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.MCode].Text = dt.Rows[i]["MCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.MName].Text = dt.Rows[i]["MName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.JCode].Text = dt.Rows[i]["Jepcode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.JGubun].Text = dt.Rows[i]["GbMCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.Unit].Text = dt.Rows[i]["Unit"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayMCode.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            ssList2.Enabled = true;
        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            ssList2.Enabled = false;

            dt = xraySql.sel_Xray_MCode(pDbCon, "", "", "GbMCode,MCode");

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUseMCode.MCode].Text = dt.Rows[i]["MCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUseMCode.MName].Text = dt.Rows[i]["MName"].ToString().Trim();
                    
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            ssList2.Enabled = true;
        }

        void GetData3(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argClass)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            dt = xraySql.sel_Xray_Code(pDbCon, "", argClass);
            
            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUseJong.Jong].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUseJong.JongName].Text = dt.Rows[i]["XName"].ToString().Trim();
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;

        }

        void GetData4(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd,string argCode)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
                        
            if (argCode == "") return;

            dt = xraySql.sel_Xray_Code(pDbCon, argCode, "");

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["DelDate"].ToString().Trim()!="")
                {
                    MessageBox.Show("삭제코드!!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("코드 오류!!");
                return;
            }

            dt = xraySql.sel_Xray_BasUse(pDbCon, argCode);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count+5;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Change].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Gubun2].Text = dt.Rows[i]["GbXCode2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Gubun2Name].Text = xraySql.read_Xray_Use_Gubun(pDbCon, dt.Rows[i]["GbXCode2"].ToString().Trim());                   
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.MCode].Text = dt.Rows[i]["MCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.MName].Text = dt.Rows[i]["MName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Qty].Text = dt.Rows[i]["QTY"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.Agree].Text = dt.Rows[i]["Agree"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayUse.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }

            }

            #endregion

        }
        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtMCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strMcode = txtMCode.Text.Trim();
                    txtMName.Text = strMcode;
                    if (strMcode != "")
                    {
                        GetData4(clsDB.DbCon, ssList1, strMcode);
                    }
                }
            }
        }
    }
}
