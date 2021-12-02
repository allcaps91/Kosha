using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayCODE04.cs
    /// Description     : 영상의학과 기초코드 - 재료코드 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xucode\xucode02.frm(MaterialMaster) >> frmComSupXrayCODE04.cs 폼이름 재정의" />
    public partial class frmComSupXrayCODE04 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        #endregion

        public frmComSupXrayCODE04()
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
            DataTable dt = Query.Get_BasBcode(clsDB.DbCon, "XRAY_JCLASS", "", " Code || '.' || Name CodeName ");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboClass, dt, clsParam.enmComParamComboType.None);
                method.setCombo_View(this.cboClass2, dt, clsParam.enmComParamComboType.None);
                
                cboClass.SelectedIndex = -1;
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

            this.txtMCode.KeyDown += new KeyEventHandler(eTxtEvent);
            this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);


            //this.cboClass.SelectedIndexChanged += new EventHandler(eCboEvent);

            this.expandableSplitter1.Click += ExpandableSplitter1_Click;

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
                       
        }                

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            string strMCode = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayMCode.MCode].Text.Trim();

            GetData2(clsDB.DbCon, strMCode);
                        
        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            if (sender == this.txtMCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    GetData2(clsDB.DbCon, txtMCode.Text.Trim().ToUpper());
                }
            }
            else if (sender == this.txtJepCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    GetData2(clsDB.DbCon, txtMCode.Text.Trim().ToUpper());
                }
            }

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

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //            
                xraySpd.sSpd_XrayMCode(ssList, xraySpd.sSpdXrayMCode, xraySpd.nSpdXrayMCode, 10, 0);

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
            else if (sender == this.btnCancel)
            {
                screen_clear();
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
            else if (sender == this.btnSearch1)
            {
                //
                GetData(clsDB.DbCon, ssList);
            }
            

            txtMCode.Focus();

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

            strTitle = "재료 코드 LIST";

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

            string strMCode = txtMCode.Text.Trim().ToUpper();

            if (strMCode == "") return;
            if (txtMName.Text.Trim() == "") return;
            if (cboClass.SelectedItem.ToString() == "") return;

            //확인 메시지
            if (Job == "삭제")
            {                
                if (ComFunc.MsgBoxQ("해당코드를 정말로 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅

            xraySql.argstr = new string[Enum.GetValues(typeof(clsComSupXraySQL.enum_XrayMCode)).Length];
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.MCdoe] = strMCode;
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.MName] = txtMName.Text.Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.GbMCode] = clsComSup.setP(cboClass.SelectedItem.ToString(), ".", 1).Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.JepCode] = txtJepCode.Text.Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.Unit] = txtUnit.Text.Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.PrintRanking] = txtPrint.Text.Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.Qty] = "1";

            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.ROWID] = "";

            #endregion
                       
            clsDB.setBeginTran(pDbCon); 

            try
            {
                if (Job == "저장")
                {
                    dt = xraySql.sel_Xray_Code(pDbCon, strMCode, "");
                    if (ComFunc.isDataTableNull(dt) == false) xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.ROWID] = dt.Rows[0]["ROWID"].ToString().Trim();

                    if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayMCode.ROWID] != "")
                    {
                        SqlErr = xraySql.up_Xray_MCode(pDbCon, xraySql.argstr, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = xraySql.ins_Xray_MCode(pDbCon, xraySql.argstr, ref intRowAffected);
                    }

                }
                else if (Job == "삭제")
                {
                    SqlErr = xraySql.del_Xray_MCode(pDbCon, strMCode, ref intRowAffected);
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

            cboClass.SelectedIndex = -1;

            lblJName.Text = "";

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

            dt = xraySql.sel_Xray_MCode(pDbCon, "", cboClass2.SelectedItem.ToString());

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
            ssList.Enabled = true;
        }

        void GetData2(PsmhDb pDbCon, string argMCode)
        {
            screen_clear();

            txtMCode.Text = argMCode;

            DataTable dt = xraySql.sel_Xray_MCode(pDbCon, argMCode, "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                txtMCode.Text = argMCode;
                txtMName.Text = dt.Rows[0]["MName"].ToString().Trim();
                txtJepCode.Text = dt.Rows[0]["JepCode"].ToString().Trim();
                
                txtUnit.Text = dt.Rows[0]["Unit"].ToString().Trim();
                txtPrint.Text = dt.Rows[0]["PrintRanking"].ToString().Trim();
                btnDelete.Enabled = true;

                dt = Query.Get_BasBcode(pDbCon, "XRAY_JCLASS", dt.Rows[0]["GbMCode"].ToString().Trim(), " Code || '.' || Name CodeName ");
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    cboClass.Text = dt.Rows[0]["CodeName"].ToString().Trim();
                }
                if (txtJepCode.Text.Trim() != "")
                {
                    dt = xraySql.sel_Ord_Jep(pDbCon, txtJepCode.Text);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        lblJName.Text = dt.Rows[0]["JepName"].ToString().Trim();
                    }
                }
                
            }
            else
            {
                MessageBox.Show("신규코드입니다..");
                txtMCode.Text = argMCode;
                btnDelete.Enabled = false;

            }
        }

        void GetData3(PsmhDb pDbCon, string argJCode)
        {            
            txtJepCode.Text = argJCode;
            lblJName.Text = "";

            if (argJCode == "") return;

            DataTable dt = xraySql.sel_Ord_Jep(pDbCon, argJCode);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                txtJepCode.Text = argJCode;
                lblJName.Text = dt.Rows[0]["Jepname"].ToString().Trim();                
            }
            else
            {
                MessageBox.Show("신규코드입니다..");
                txtMCode.Text = argJCode;
               

            }
        }
    }
}
