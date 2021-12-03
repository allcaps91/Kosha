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
    /// File Name       : frmComSupXrayCODE01.cs
    /// Description     : 영상의학과 기초코드관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xucode\xucode01.frm(CodeMaster) >> frmComSupXrayCODE01.cs 폼이름 재정의" />
    public partial class frmComSupXrayCODE01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();        
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();

        frmComSupXrayHELP01 fXHelp = null; //부서코드 찾기 공통폼

        //classcode 배열
        string[] classCode = null;

        #endregion

        public frmComSupXrayCODE01()
        {
            InitializeComponent();

            setEvent();                       

        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;
            

            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            setCombo(pDbCon);

            screen_clear();

        }

        void setCombo(PsmhDb pDbCon)
        {
            DataTable dt = xraySql.sel_Xray_Class_Combo(pDbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboClass, dt, clsParam.enmComParamComboType.None);
                method.setCombo_View(this.cboClass2, dt, clsParam.enmComParamComboType.ALL);

                classCode = new string[dt.Rows.Count];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    classCode[i] = dt.Rows[i]["ClassCode"].ToString().Trim();
                }

                cboClass.SelectedIndex = -1;
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }
        }
        
        void setCombo2(PsmhDb pDbCon, string argClass, string argSubClass)
        {
            DataTable dt = xraySql.sel_Xray_SubClass(pDbCon,argClass, "",true);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboSubClass, dt, clsParam.enmComParamComboType.None);

                foreach (var item in cboSubClass.Items)
                {
                    if (VB.Left(item.ToString(), 2) == argSubClass)
                    {
                        cboSubClass.SelectedIndex = cboSubClass.Items.IndexOf(item);

                        return;
                    }
                                        
                }
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
            this.btnSearch2.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);

            this.txtCode.KeyDown += TxtCode_KeyDown;

            this.cboClass.SelectedIndexChanged += new EventHandler(eCboEvent);

            this.expandableSplitter1.Click += ExpandableSplitter1_Click;

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            setDelegate();//델리게이트 세팅
        }

        #region 델리게이트 관련

        void setDelegate()
        {
            if (fXHelp == null)
            {
                fXHelp = new frmComSupXrayHELP01();
                fXHelp.rSendMsg += new frmComSupXrayHELP01.SendMsg(fXHelp_SendMsg);
            }
        }

        void fXHelp_SendMsg(string strMsg)
        {
            //txtBuse.Text = strMsg;
            this.TextSet(strMsg);
        }

        public void TextSet(string str)
        {
            txtBuse.Text = str;
        }


        #endregion

        void eCboEvent(object sender,EventArgs e)
        {
            //
            try
            {
                if (cboClass != null && cboClass.SelectedItem.ToString() !="")
                {
                    setCombo2(clsDB.DbCon, clsComSup.setP(cboClass.SelectedItem.ToString(), ".", 1).Trim(), "");
                }
            }
            catch
            {

            }
            
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            string strXCode = ssList.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayCodeMst.XCode].Text.Trim();

            GetData2(clsDB.DbCon, strXCode);
                        
        }

        void TxtCode_KeyDown(object sender, KeyEventArgs e)
        {   
            if (e.KeyCode == Keys.Enter)
            {
                GetData2(clsDB.DbCon, txtCode.Text.Trim().ToUpper());
            }

        }               

        void ExpandableSplitter1_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;

            if (ex.Expanded == true)
            {
                //panel3.Size = new System.Drawing.Size(882, 20);
            }
            else
            {
                //panel3.Size = new System.Drawing.Size(882, 280);
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
                xraySpd.sSpd_XrayCodeMst(ssList, xraySpd.sSpdXrayCodeMst, xraySpd.nSpdXrayCodeMst, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData(clsDB.DbCon);
                
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
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
            else if (sender == this.btnSearch2)
            {
                //                       
                fXHelp.ShowDialog();

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

            strTitle = "검사 코드 LIST" ;

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
         
            string strXCode = txtCode.Text.Trim().ToUpper();

            if (strXCode == "") return;

            //확인 메시지
            if (Job == "삭제")
            {                
                if (ComFunc.MsgBoxQ("해당코드를 정말로 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅

            xraySql.argstr = new string[Enum.GetValues(typeof(clsComSupXraySQL.enum_XrayCode)).Length];
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.XCode] = strXCode;
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.XName] = txtName.Text.Trim();

            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Class] = cboClass.SelectedItem.ToString();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.SubClass] = cboSubClass.SelectedItem.ToString();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Res] = chkRes.Checked == true ? "Y" : "";
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Pacs] = chkPacs.Checked == true ? "Y" : "N";
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Cnt1] = txtBCnt.Text.Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Cnt2] = txtCCnt.Text.Trim();
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Buse] = clsComSup.setP(txtBuse.Text.Trim(),".",1).Trim();
            if (optS0.Checked==true)
            {
                xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.OptBun] = "0";
            }
            else if (optS1.Checked == true)
            {
                xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.OptBun] = "1";
            }
            else if (optS2.Checked == true)
            {
                xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.OptBun] = "2";
            }
            else if (optS3.Checked == true)
            {
                xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.OptBun] = "3";
            }

            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.DelDate] = txtDelDate.Text.Trim();

            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Remark1] = ComFunc.QuotConv(txtRemark1.Text.Trim());
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Remark2] = ComFunc.QuotConv(txtRemark2.Text.Trim());
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Remark3] = ComFunc.QuotConv(txtRemark3.Text.Trim());
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.Remark4] = ComFunc.QuotConv(txtRemark4.Text.Trim());
            xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.ROWID] = "";

            #endregion
                        
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (Job == "저장")
                {
                    dt = xraySql.sel_Xray_Code(pDbCon, strXCode, "");
                    if (ComFunc.isDataTableNull(dt) == false) xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.ROWID] = dt.Rows[0]["ROWID"].ToString().Trim();

                    if (xraySql.argstr[(int)clsComSupXraySQL.enum_XrayCode.ROWID] != "")
                    {
                        SqlErr = xraySql.up_Xray_Code(pDbCon, xraySql.argstr, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = xraySql.ins_Xray_Code(pDbCon, xraySql.argstr, ref intRowAffected);
                    }

                }
                else if (Job == "삭제")
                {
                    SqlErr = xraySql.del_Xray_Code(pDbCon, strXCode, ref intRowAffected);
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
                else  if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }

            }

            cboClass.SelectedIndex = -1;
            cboSubClass.SelectedIndex = -1;

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


            dt = xraySql.sel_Xray_Code_Sub(pDbCon,"",cboClass2.SelectedItem.ToString(), chkDel.Checked);

            
            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.XCode].Text = dt.Rows[i]["XCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.XName].Text = dt.Rows[i]["XName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.SubCode].Text = dt.Rows[i]["SubCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.SubName].Text = dt.Rows[i]["SubName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.SeekGbn].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.Res].Text = dt.Rows[i]["GbReserved"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.Cnt1].Text = dt.Rows[i]["BCnt"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.Cnt2].Text = dt.Rows[i]["CCnt"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.Buse].Text = dt.Rows[i]["BUCODE"].ToString().Trim() + dt.Rows[i]["Name"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.ClassCode].Text = dt.Rows[i]["ClassCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayCodeMst.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    
                }

            }

            #endregion

            Cursor.Current = Cursors.Default;
            ssList.Enabled = true;
        }

        void GetData2(PsmhDb pDbCon, string argXCode)
        {
            DataTable dt = null;
            
            screen_clear();

            txtCode.Text = argXCode;

            dt = xraySql.sel_Xray_Code_Sub(pDbCon, argXCode, "**", false);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                btnDelete.Enabled = true;
                btnSave.Enabled = true;

                txtName.Text = dt.Rows[0]["XName"].ToString().Trim();
                if (dt.Rows[0]["BUCODE"].ToString().Trim() !="")
                {
                    txtBuse.Text = dt.Rows[0]["BUCODE"].ToString().Trim() + "." + dt.Rows[0]["NAME"].ToString().Trim();
                }               

                chkRes.Checked = false;
                if (dt.Rows[0]["GbReserved"].ToString().Trim() == "Y") chkRes.Checked = true;

                chkPacs.Checked = false;
                if (dt.Rows[0]["GbPacs"].ToString().Trim() == "Y") chkPacs.Checked = true;

                txtBCnt.Text = dt.Rows[0]["BCnt"].ToString().Trim();
                txtCCnt.Text = dt.Rows[0]["CCnt"].ToString().Trim();

                txtDelDate.Text = dt.Rows[0]["DelDate"].ToString().Trim();

                cboClass.SelectedIndex = sup.Get2_Conv(classCode, dt.Rows[0]["ClassCode"].ToString().Trim());

                txtRemark1.Text = dt.Rows[0]["Remark1"].ToString().Trim();
                txtRemark2.Text = dt.Rows[0]["Remark2"].ToString().Trim();
                txtRemark3.Text = dt.Rows[0]["Remark3"].ToString().Trim();
                txtRemark4.Text = dt.Rows[0]["Remark4"].ToString().Trim();

                optS0.Checked = true;
                if (dt.Rows[0]["Remark4"].ToString().Trim()=="1")
                {
                    optS1.Checked = true;
                }
                else if (dt.Rows[0]["Remark4"].ToString().Trim() == "2")
                {
                    optS2.Checked = true;
                }
                else if (dt.Rows[0]["Remark4"].ToString().Trim() == "3")
                {
                    optS3.Checked = true;
                }

                //
                setCombo2(pDbCon, dt.Rows[0]["ClassCode"].ToString().Trim(), dt.Rows[0]["SubCode"].ToString().Trim());


            }
            else
            {
                txtName.Focus();

                btnDelete.Enabled = false;          
                btnSave.Enabled = true;

            }
            
        }
    }
}
