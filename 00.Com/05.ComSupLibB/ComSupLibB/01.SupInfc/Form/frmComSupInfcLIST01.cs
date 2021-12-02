using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static ComSupLibB.Com.clsParam;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupInfc
    /// File Name       : frmComSupInfcLIST01.cs
    /// Description     : Influenza 검사 (유행성검사) LIST
    /// Author          : 김홍록
    /// Create Date     : 2017-06-19
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\ExInfluenza.frm" />
    public partial class frmComSupInfcLIST01 : Form
    {

        clsInFcSQL inFcSql = new clsInFcSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        DateTime sysdate;

        Thread thread;

        /// <summary>생성자</summary>
        public frmComSupInfcLIST01()
        {
            InitializeComponent();

            setEvent();

        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnViewResult.Click += new EventHandler(eBtnClick);

            this.btnPrint.Click += new EventHandler(eBtnPrintClick);

            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.rdoI.Click += new EventHandler(eRdoClick);
            this.rdoO.Click += new EventHandler(eRdoClick);

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

        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();

        }

        void eBtnSearch(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eBtnPrintClick(object sender, EventArgs e)
        {
            setCtrlSpreadPrint();
        }

        void eRdoClick(object sender, EventArgs e)
        {
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnViewResult)
            {
                //TODO : 2017.06.22.김홍록: 통합검사조회 조회
                ComFunc.MsgBox("TODO : 통합검사조회 호출");

            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {

            string s = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsInFcSQL.enmSel_EXAM_RESULTC_infec.EMR_SCAN_YN].Text;

            if (s == "▦")
            {
                //TODO : 2017.06.22.김홍록: EMR SCANER 호출
                ComFunc.MsgBox("TODO : SCANER 호출");
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
            List<string> lstCbo = new List<string>();
            List<string> lstSqlWhere = new List<string>();

            lstSqlWhere.Add("CS");
            lstSqlWhere.Add("RT");
            lstSqlWhere.Add("PT");
            lstSqlWhere.Add("II");
            lstSqlWhere.Add("AN");

            DataTable dt = comSql.sel_BAS_CLINICDEPT_COMBO(clsDB.DbCon, lstSqlWhere);        

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
            }
            else
            {
                ComFunc.MsgBox("데이터가 존재하지 않습니다.");
            }


        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }


            string strDept = method.getGubunText(this.cboDept.Text, ".");

            strDept = strDept == "*" ? "" : strDept;

            enmComParamGBIO enmGBIO = enmComParamGBIO.ALL;

            if (rdoI.Checked == true)
            {
                enmGBIO = enmComParamGBIO.I;
            }
            else if (rdoO.Checked == true)
            {
                enmGBIO = enmComParamGBIO.O;
            }

            List<string> lstOrderCode = new List<string>(); 

            if (this.chkInflu.Checked)
            {
                lstOrderCode.Add("SI14F");
                lstOrderCode.Add("SI14G");
                lstOrderCode.Add("SI14H");
            }

            if (string.IsNullOrEmpty(this.txtExamCode01.Text) == false)
            {
                lstOrderCode.Add(this.txtExamCode01.Text);
            }

            if (string.IsNullOrEmpty(this.txtExamCode02.Text) == false)
            {
                lstOrderCode.Add(this.txtExamCode02.Text);
            }

            if (string.IsNullOrEmpty(this.txtExamCode03.Text) == false)
            {
                lstOrderCode.Add(this.txtExamCode03.Text);
            }

            if (string.IsNullOrEmpty(this.txtExamCode04.Text) == false)
            {
                lstOrderCode.Add(this.txtExamCode04.Text);
            }

            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.ToString("yyyy-MM-dd");


            if (method.getDate_Gap(Convert.ToDateTime(strFDate), Convert.ToDateTime(strTDate)) > 60)
            {
                ComFunc.MsgBox("일자는 60일을 넘을 수 없습니다.");
                this.dtpFDate.Focus();
                return;
            }


            thread = new Thread(() => threadSetCtrlSpread(strFDate, strTDate, strDept, enmGBIO, lstOrderCode));
            thread.Start();
          
        }

        void threadSetCtrlSpread(string strFDate, string strTDate, string strDept, enmComParamGBIO enmGBIO, List<string> lstOrderCode)
        {
            DataSet ds = null;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = inFcSql.sel_EXAM_RESULTC_infec(clsDB.DbCon, strFDate, strTDate, strDept, enmGBIO, lstOrderCode);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, inFcSql.sSel_EXAM_RESULTC_infec, inFcSql.nSel_EXAM_RESULTC_infec });
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

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

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, (int)clsInFcSQL.enmSel_EXAM_RESULTC_infec.RESULT, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdSort(spd, (int)clsInFcSQL.enmSel_EXAM_RESULTC_infec.PANO, true);
        }

        void setCtrlSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;


            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {

                strHeader = spread.setSpdPrint_String("검사리스트", new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                string s = this.chkInflu.Checked == true ? "Influenza," : "";

                s += string.IsNullOrEmpty(this.txtExamCode01.Text) == false ? method.getGubunText(this.txtExamCode01.Text, ".") + "," : "";
                s += string.IsNullOrEmpty(this.txtExamCode02.Text) == false ? method.getGubunText(this.txtExamCode02.Text, ".") + "," : "";
                s += string.IsNullOrEmpty(this.txtExamCode03.Text) == false ? method.getGubunText(this.txtExamCode03.Text, ".") + "," : "";
                s += string.IsNullOrEmpty(this.txtExamCode04.Text) == false ? method.getGubunText(this.txtExamCode04.Text, ".") + "," : "";

                s = s.Substring(0, s.Length - 1);

                strHeader += spread.setSpdPrint_String("세부항목:" + this.dtpFDate.Value.ToString("yyyy-MM-dd") + " 부터 " + this.dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += spread.setSpdPrint_String("작업기간:" + s + this.dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

                clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);

            }

        }



    }
}
