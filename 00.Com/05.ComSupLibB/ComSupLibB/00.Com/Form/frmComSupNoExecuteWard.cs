using ComLibB;
using ComSupLibB.SupLbEx;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.Com
{

    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupNoExecuteWard.cs
    /// Description     : BST 병동 불출 현황 관리
    /// Author          : 김홍록
    /// Create Date     : 2017-06-14
    /// Update History  : 
    /// </summary>
    /// <history>
    /// 2017.06.21.김홍록: 고경자 과장님과 협의 후 출력은 삭제하기로 함. 
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrinfo\FrmNoExecute.frm" />

    public partial class frmComSupNoExecuteWard : Form
    {

        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsComSupSpd csupspd = new clsComSupSpd();
        clsPublic cpublic = new clsPublic();

        string FstrPANO = string.Empty;
      
        DateTime sysdate;

        Thread thread;

        /// <summary>생성자</summary>
        public frmComSupNoExecuteWard()
        {
            InitializeComponent();

            setEvent();
        }

        public frmComSupNoExecuteWard(string strPANO)
        {
            InitializeComponent();

            setEvent();

            FstrPANO = strPANO;
        }


        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);

            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.chkAll.Click += new EventHandler(eChkChange);
            this.chkXRay.Click += new EventHandler(eChkChange);
            this.chkEndo.Click += new EventHandler(eChkChange);
            this.chkETC.Click += new EventHandler(eChkChange);
            this.chkEXAM.Click += new EventHandler(eChkChange);
            this.chkNoBarCode.Click += new EventHandler(eChkChange);

            this.rdoDate.Click += new EventHandler(eRdoClick);
            this.rdoIn.Click += new EventHandler(eRdoClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
            this.userPtInfo.txtSearch_PtInfo.Text = FstrPANO;
        }

        void eFormMove(object sender, EventArgs e)
        {

            Point p = new Point();

            p.X = this.Location.X + this.userPtInfo.Location.X;
            p.Y = this.Location.Y + this.userPtInfo.Location.Y + this.userPtInfo.Height * 7;

            this.userPtInfo.pPSMH_LPoint = p;
        }

        void setCtrl()
        {
            setCtrlCircular(false);
            setCtrlDate();
            setCtrlCombo();
            //setCtrlSpread();

            setSpdStyle(this.ssMain,null, lbExSQL.sSel_IPD_NEW_MASTER_EXAM, lbExSQL.nSel_IPD_NEW_MASTER_EXAM);
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            //string header = string.Empty;
            //string foot = string.Empty;

            //string headerSubTitle = string.Empty;

            //if (this.rdoActing.Checked == true)
            //{
            //    headerSubTitle = "시행처방";
            //}
            //else if (this.rdoNoActing.Checked == true)
            //{
            //    headerSubTitle = "미시행처방";
            //}

            //string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            //header = spread.setSpdPrint_String(headerSubTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            //header += spread.setSpdPrint_String("출력시간:" + s + " Page : /p", new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            //clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(5, 5, 5, 5, 5, 5);
            //clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape
            //                                , PrintType.All, 0, 0, true, true, true, false, false, false, false);

            //spread.setSpdPrint(this.ssMain, true, margin, option, header, foot);
            //string[] strhead = new string[2];
            //string[] strfont = new string[2];

            ////
            //read_sysdate();

            //if (this.rdoActing.Checked == true)
            //{
            //    header = "/c/f1/n" + "시행처방" + "/f1/n";
            //}
            //else if (this.rdoNoActing.Checked == true)
            //{
            //    header = "/c/f1/n" + "미시행 처방" + "/f1/n";
            //}

            //header+= "/n/l/f2" + "조회기간 : " + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + " /l/f2" + "  출력시간 : " + cpublic.strSysDate + " " + cpublic.strSysTime + " /n";

            ////csupspd.SPREAD_PRINT(ssMain_Sheet1, ssMain, strhead, strfont, 10, 10, 2, true);

            //clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(5, 5, 5, 5, 5, 5);
            //clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape
            //                                , PrintType.All, 0, 0, true, true, true, false, false, false, false);

            //spread.setSpdPrint(this.ssMain, true, margin, option, header, foot);


            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            strFont1 = "/l/fn\"굴림체\" /fz\"20\" ";
            if (this.rdoActing.Checked == true)
            {
                strHead1 = "/n" + "시행처방" + "/n";
            }
            else if (this.rdoNoActing.Checked == true)
            {
                strHead1 = "/n" + "미시행 처방" + "/n";
            }
            strFont2 = "/n/fn\"굴림체\" /fb0/fu0/fz\"11\" ";
            
            strHead2 = "/n" + "/n" + "조회기간 : " + dtpFDate.Text.Trim() + "~" + dtpTDate.Text.Trim() + "     Page : /p  " + "/n" + "/n";

            ssMain_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssMain_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssMain_Sheet1.PrintInfo.Margin.Top = 10;
            ssMain_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssMain_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssMain_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssMain_Sheet1.PrintInfo.ShowBorder = true;
            ssMain_Sheet1.PrintInfo.ShowColor = true;
            ssMain_Sheet1.PrintInfo.ShowGrid = true;
            ssMain_Sheet1.PrintInfo.ShowShadows = true;
            ssMain_Sheet1.PrintInfo.UseMax = false;
            ssMain_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssMain_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssMain_Sheet1.PrintInfo.ShowPrintDialog = true;
            ssMain_Sheet1.PrintInfo.Preview = true;
            ssMain.PrintSheet(0);
        }
        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == this.rdoIn)
            {
                this.dtpFDate.Enabled = false;
                this.dtpTDate.Enabled = false;
            }
            else if (sender == this.rdoDate)
            {
                this.dtpFDate.Enabled = true;
                this.dtpTDate.Enabled = true;
            }

        }

        void eChkChange(object sender, EventArgs e)
        {
            if (sender == this.chkAll)
            {

                foreach (CheckBox c in grpExamType.Controls)
                {
                    if (c.Name != "chkAll")
                    {
                        c.Checked = this.chkAll.Checked;
                    }
                }
            }
            else
            {
                bool b = true;

                foreach (CheckBox c in grpExamType.Controls)
                {
                    if (c.Name != "chkAll")
                    {
                        if (c.Checked == false)
                        {
                            b = false;
                        }
                    }

                    this.chkAll.Checked = b;
                }

            }

        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate.AddDays(-7);
            this.dtpTDate.Value = sysdate.AddDays(-1);
        }

        void setCtrlCombo()
        {
            setCtrlComboWard();
            setCtrlComboDept();
        }

        void setCtrlComboDept()
        {
            string strTemp = "";
            string strBuseCode = "";

            List<string> lstCbo = new List<string>();

            lstCbo.Add("ER.응급실");
            DataTable dt = comSql.sel_BAS_WARD_COMBO(clsDB.DbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstCbo.Add(dt.Rows[i]["CODE_NAME"].ToString());
                }

                method.setCombo_View(this.cboWard, lstCbo, clsParam.enmComParamComboType.ALL);

                //TODO : 2017.06.08.김홍록 INI
                //this.cboWard.Text = GstrHelpCode;

                //this.cboWard.Enabled = false;

                
                //2019-05-13 안정수, 작업자사번에 따른 콤보박스자동세팅 로직 추가
                strTemp = clsType.User.BuseCode;
                strBuseCode = comSql.sel_BAS_BUSE_YNAME(clsDB.DbCon, strTemp);

                if(strBuseCode != "")
                {
                    for (int i = 0; i < cboWard.Items.Count; i++)
                    {
                        if (cboWard.Items[i].ToString().Trim().Contains(strBuseCode))
                        {
                            cboWard.SelectedIndex = i;
                            break;
                        }
                    }
                   
                }
            }
            else
            {
                ComFunc.MsgBox("데이터가 존재하지 않습니다.");
            }

            lstCbo.Clear();

        }

        void setCtrlComboWard()
        {
            List<string> lstCbo = new List<string>();

            DataTable dt = comSql.sel_BAS_CLINICDEPT_COMBO(clsDB.DbCon);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstCbo.Add(dt.Rows[i]["CODE_NAME"].ToString());
                }

                method.setCombo_View(this.cboDept, lstCbo, clsParam.enmComParamComboType.ALL);

                //TODO : 2017.06.08.김홍록 INI
                //this.cboWard.Text = GstrHelpCode;

                //this.cboWard.Enabled = false;

                //2019-05-13 안정수, 병동셋팅 추가
                
            }
            else
            {
                ComFunc.MsgBox("데이터가 존재하지 않습니다.");
            }

        }

        void setCtrlSpread()
        {

            int[] nExam = new int[Enum.GetValues(typeof(clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param)).Length];

            nExam[(int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param.ALL] = this.chkAll.Checked == true ? 1 : 0;
            nExam[(int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param.XRAY] = this.chkXRay.Checked == true ? 1 : 0;
            nExam[(int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param.ENDO] = this.chkEndo.Checked == true ? 1 : 0;
            nExam[(int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param.ETC] = this.chkETC.Checked == true ? 1 : 0;
            nExam[(int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param.EXAM] = this.chkEXAM.Checked == true ? 1 : 0;
            nExam[(int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM_Param.BARCODE] = this.chkNoBarCode.Checked == true ? 1 : 0;

            int nActing = 0;

            if (this.rdoActAll.Checked == true)
            {
                nActing = 0;
            }
            else if (this.rdoNoActing.Checked == true)
            {
                nActing = 1;
            }
            else if (this.rdoActing.Checked == true)
            {
                nActing = 2;
            }

            int nSearch = 0;

            if (this.rdoIn.Checked == true)
            {
                nSearch = 0;
            }
            else if (this.rdoDate.Checked == true)
            {
                nSearch = 0;
            }

            string strWard = string.Empty;

            strWard = method.getGubunText(this.cboWard.Text, ".");
            strWard = strWard == "*" ? "" : strWard;

            string strDept = string.Empty;
            strDept = method.getGubunText(this.cboDept.Text, ".");
            strDept = strDept == "*" ? "" : strDept;

            thread = new Thread(() =>threadSetCtrlSpread(nExam,nActing,nSearch,strWard,strDept));

            thread.Start();        
        }

        void threadSetCtrlSpread(int[] nExam,int nActing, int nSearch, string strWard, string strDept)
        {
            DataSet ds = null; 

           
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = lbExSQL.sel_IPD_NEW_MASTER_EXAM(
                clsDB.DbCon
                , strWard
                                            , this.userPtInfo.txtSearch_PtInfo.Text
                                            , strDept
                                            , this.chkOut.Checked
                                            , nActing
                                            , nSearch
                                            , this.dtpFDate.Value.ToString("yyyy-MM-dd")
                                            , this.dtpTDate.Value.ToString("yyyy-MM-dd")
                                            , nExam);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, lbExSQL.sSel_IPD_NEW_MASTER_EXAM, lbExSQL.nSel_IPD_NEW_MASTER_EXAM });

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

            //setSpdStyle(this.ssMain, ds, lbExSQL.sSel_IPD_NEW_MASTER_EXAM, lbExSQL.nSel_IPD_NEW_MASTER_EXAM);
        }

        delegate void delegateSetCtrlCircular(bool b);            
        void setCtrlCircular(bool b)
        {
            if (b == true)
            {
                this.ssMain.Enabled = false;
            }
            else
            {
                this.ssMain.Enabled = true;
            }

            this.circProgress.Visible = b;
            this.circProgress.IsRunning = b;
        }

        delegate void delegateSetSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size);
        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {

            spd.ActiveSheet.Rows.Count = 0;

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;


            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            // 3. 컬럼 스타일 설정.
            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.EXAM_TYPE, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.WARTNO, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.ORDERNO, clsSpread.enmSpdType.Hide);

            // 4. 정렬
            spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.SNAME, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.EXAM_NAME, AutoFilterMode.EnhancedContextMenu, true);

            spread.setSpdSort(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.SNAME, true);
            spread.setSpdSort(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.EXAM_NAME, true);

            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.ROOMCODE);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.PANO);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.SNAME);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.SEX);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.AGE);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.ROOMCODE);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.DRNAME);
            spread.setColMerge(spd, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.DEPTCODE);

            // 6. 삭제 색
            UnaryComparisonConditionalFormattingRule unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "Y", false);
            unary.BackColor = method.cSpdRowSubTotalColor;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)clsComSupLbExSQL.enmSel_IPD_NEW_MASTER_EXAM.CADEX_DEL, unary);




            //spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);




        }


    }
}
