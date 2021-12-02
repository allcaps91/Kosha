using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayCODE03.cs
    /// Description     : 영상의학과 기초코드관리 - 소분류 등록관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xucode\xucode05.frm(SubCodeInput) >> frmComSupXrayCODE03.cs 폼이름 재정의" />
    public partial class frmComSupXrayCODE03 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();        
        clsMethod method = new clsMethod();

        #endregion

        public frmComSupXrayCODE03()
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
            DataTable dt = xraySql.sel_Xray_Class_Combo(clsDB.DbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboClass, dt, clsParam.enmComParamComboType.None);
                method.setCombo_View(this.cboClass2, dt, clsParam.enmComParamComboType.None);
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
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSearch.Click += new EventHandler(eBtnEvent);           
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);
            
            this.txtCode.KeyPress += new KeyPressEventHandler(eTxtEvent);

            this.cboClass2.SelectedIndexChanged += new EventHandler(eCboEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            
        }

        void eTxtEvent(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtCode)
            {
                if (e.KeyChar == (char)Keys.Return)
                {
                    GetData2(clsDB.DbCon, cboClass.SelectedItem.ToString() ,txtCode.Text.Trim().ToUpper());
                    txtName.Focus();
                }

            }
        }

        void eCboEvent(object sender, EventArgs e)
        {
            //
            if(sender == this.cboClass2)
            {
                //
                GetData(clsDB.DbCon, ssList);
            }

        }
        
        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            string strCode = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCode3.SubCode].Text.Trim();
            string strClass = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCode3.ClassCode].Text.Trim()+ "." + ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCode3.ClassName].Text.Trim();

            GetData2(clsDB.DbCon, strClass, strCode);
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
                xraySpd.sSpd_XrayCode3(ssList, xraySpd.sSpdXrayCode3, xraySpd.nSpdXrayCode3, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();
                
            }
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }            
            else if (sender == this.btnPrint)
            {
                //
                ePrint();
            }
            else if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon, "저장");
            }
            else if (sender == this.btnDelete)
            {
                //
                eSave(clsDB.DbCon, "삭제");
            }
            else if (sender == this.btnSearch)
            {
                //
                GetData(clsDB.DbCon, ssList);
            }
            

            txtCode.Focus();

        }
        
        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "검사 소분류 LIST";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void eSave(PsmhDb pDbCon, string Job)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strClass = clsComSup.setP(cboClass.SelectedItem.ToString(),".",1).Trim();
            string strSubCode = txtCode.Text.Trim().ToUpper();
            string strSubName = txtName.Text.Trim().ToUpper();

            if (strClass == "") return;
            if (strSubCode == "") return;
            if (strSubName == "") return;

            //확인 메시지
            if (Job == "삭제")
            {                
                if (ComFunc.MsgBoxQ("해당코드를 정말로 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅

            xraySql.argstr = new string[Enum.GetValues(typeof(clsComSupXraySQL.enum_XrayCode3)).Length];
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode3.SubCode] = strSubCode;            
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode3.SubName] = strSubName;
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode3.ClassCode] = strClass;
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode3.ROWID] = "";

            #endregion

            
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (Job == "저장")
                {
                    dt = xraySql.sel_Xray_SubClass(pDbCon, strClass, strSubCode);
                    if (ComFunc.isDataTableNull(dt) == false) xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode3.ROWID] = dt.Rows[0]["ROWID"].ToString().Trim();

                    if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode3.ROWID] != "")
                    {
                        SqlErr = xraySql.up_Xray_SubClass(pDbCon, xraySql.argstr, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = xraySql.ins_Xray_SubClass(pDbCon, xraySql.argstr, ref intRowAffected);
                    }

                }
                else if (Job == "삭제")
                {
                    SqlErr = xraySql.del_Xray_SubClass(pDbCon, strClass, strSubCode, ref intRowAffected);
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
            catch(Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                return;
            }
            
            screen_clear();
            GetData(pDbCon, ssList);

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

            btnDelete.Enabled = false;
            btnSave.Enabled = true;


        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            ssList.Enabled = false;


            dt = xraySql.sel_Xray_SubClass(pDbCon, cboClass2.SelectedItem.ToString());

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCode3.SubCode].Text = dt.Rows[i]["SUBCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCode3.SubName].Text = dt.Rows[i]["SUBNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCode3.ClassCode].Text = dt.Rows[i]["CLASSCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCode3.ClassName].Text = dt.Rows[i]["CLASSNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCode3.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            ssList.Enabled = true;
        }

        void GetData2(PsmhDb pDbCon, string argClass, string argCode)
        {
            screen_clear();

            cboClass.Text = argClass;

            DataTable dt = xraySql.sel_Xray_SubClass(pDbCon, argClass, argCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                txtCode.Text = argCode;
                txtName.Text = dt.Rows[0]["SubName"].ToString().Trim();
                btnDelete.Enabled = true;
            }
            else
            {
                MessageBox.Show("신규코드입니다..");
                txtCode.Text = argCode;
                btnDelete.Enabled = false;

            }
        }
    }
}
