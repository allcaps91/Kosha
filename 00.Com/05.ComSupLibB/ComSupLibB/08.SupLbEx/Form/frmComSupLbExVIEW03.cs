using ComLibB;
using ComSupLibB.Com;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;
using System.Threading;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExVIEW03.cs
    /// Description     : CVR 대상자 명단
    /// Author          : 김홍록
    /// Create Date     : 2017-07-03
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exmain\FrmCVListView.frm" />
    public partial class frmComSupLbExVIEW03 : Form
    {

        bool isInit = true;
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsParam param = new clsParam();

        DateTime sysdate;

        string gStrSabun = string.Empty;
        string gStrDRCode = string.Empty;

        Thread thread;

        /// <summary>생성자</summary>
        /// <param name="strPano">환자번호</param>
        /// <param name="strFdate">조회시작일자</param>
        public frmComSupLbExVIEW03( string strSabun, string strDRCode)
        {
            InitializeComponent();

            this.gStrSabun = strSabun;
            this.gStrDRCode = strDRCode;

            if (string.IsNullOrEmpty(gStrDRCode) == true)
            {
                if (isDr() == false)
                {
                    this.Close();
                }
            }

            setEvent();           
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.supComPtInfo1.ePSMH_UcSupComPtSearch_VALUE += new UcSupComPtSearch.PSMH_RETURN_VALUE(ePSMH_ReturnValue);
            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssMain.CellClick += new CellClickEventHandler(eSpreadClick);

            this.rdoAll.Click += new EventHandler(eRdoClick);
            this.rdoConfirm.Click += new EventHandler(eRdoClick);
            this.rdoNone.Click += new EventHandler(eRdoClick);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.cboWS.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);                       
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
                this.dtpFDate.Focus();
            }
        }

        bool isDr()
        {
            bool b = true;
            if (true)
            {
                DataTable dt = comSql.sel_OCS_DOCTOR_DRCODE(clsDB.DbCon, this.gStrSabun);

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (dt.Rows[0][(int)clsComSQL.enmSel_OCS_DOCTOR_DRCODE.GBOUT].ToString() =="Y")
                    {
                        ComFunc.MsgBox("사용 중지 된 의사 정보입니다.");
                        return false;
                    }
                    else
                    {
                        this.gStrDRCode = dt.Rows[0][(int)clsComSQL.enmSel_OCS_DOCTOR_DRCODE.DRCODE].ToString();
                    }
                }
                else
                {
                    ComFunc.MsgBox("의사정보가 존재 하지 않습니다.");
                    return false;
                }
            }

            return b;
        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlCombo();
            setCtrlSpread();
        }

        void eFormMove(object sender, EventArgs e)
        {
            Point p = new Point();

            p.X = this.Location.X + this.supComPtInfo1.Location.X;
            p.Y = this.Location.Y + this.supComPtInfo1.Location.Y + this.supComPtInfo1.Height * 3 + 5;

            this.supComPtInfo1.pPSMH_LPoint = p;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnPrint)
            {
                this.ssMain.ActiveSheet.Rows.Count = 0;                
            }
            
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpread();
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            setCtrlSpreadPrint();

        }



        void eCtrlKeyPress(object sender, KeyPressEventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        void eRdoClick(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            string strChkDate = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV.CHKDATE].Text;
            string strRowId = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV.ROWID].Text;

            int intRowAffected = 0;
            int chkRow = 0;

            // clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            string SqlErr = string.Empty;
            string SQL = string.Empty;


            if (string.IsNullOrEmpty(strChkDate) == true)
            {
                lbExSQL.UP_EXAM_RESULTC_CV(clsDB.DbCon, strRowId, this.gStrSabun, ref intRowAffected);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                chkRow += 1;
                if (chkRow > 0)
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    //ComFunc.MsgBox(chkRow.ToString() + " 건을 저장 하였습니다.");
                    //setCtrl();
                }
            }
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, e.Column);
                return;
            }

        }

        void ePSMH_ReturnValue(object sender, string pano, string sname)
        {
            if (isInit == false)
            {
                setCtrlSpread();

            }

            this.isInit = false;
            
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate.AddMonths(-1);
            this.dtpTDate.Value = sysdate;

        }

        void setCtrlCombo()
        {
            DataTable dt = comSql.sel_BAS_BCODE_COMBO(clsDB.DbCon, "EXAM_검체번호현황_WS");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                method.setCombo_View(this.cboWS, dt, clsParam.enmComParamComboType.ALL);
            }
            else
            {
                ComFunc.MsgBox("조회된 값이 존재하지 않습니다.");
            }

            method.setCombo_View(this.cboWS, dt, clsParam.enmComParamComboType.ALL);
            
        }

        void setCtrlSpread()
        {            
            string strPano = this.supComPtInfo1.txtSearch_PtInfo.Text;
            string strWs = method.getGubunText(this.cboWS.Text, ".");

            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.ToString("yyyy-MM-dd");

            clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV_STATUE chkStatus = clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV_STATUE.ALL;
            if (this.rdoConfirm.Checked == true)
            {
                chkStatus = clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV_STATUE.CONFIRM;
            }
            else if (this.rdoNone.Checked == true)
            {
                chkStatus = clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV_STATUE.NONE;
            }

            if (isCtrlSpreadError(strFDate,strTDate) == true)
            {
                return;
            }

            thread = new Thread(() => threadSetCtrlSpread(strFDate,strTDate,strWs,strPano,chkStatus));
            thread.Start();
               


        }

        bool isCtrlSpreadError(string strFDate, string strTDate)
        {
            bool b = false;

            if (string.IsNullOrEmpty(strFDate) == true)
            {
                ComFunc.MsgBox("시작일자를 입력하세요");
                b = true;
            }
            if (string.IsNullOrEmpty(strTDate) == true)
            {
                ComFunc.MsgBox("종료일자를 입력하세요");
                b = true;
            }

            int nDate = method.getDate_Gap(Convert.ToDateTime(strFDate), Convert.ToDateTime(strTDate));

            if (nDate > 31)
            {
                ComFunc.MsgBox("조회일자는 31 일을 넘을 수 없습니다.");
                this.dtpFDate.Focus();
                b = true;
            }


            return b;
        }

        void threadSetCtrlSpread(string strFDate, string strTDate, string strWs, string strPano, clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV_STATUE chkStatus )
        {
            DataSet ds = null;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds = lbExSQL.sel_EXAM_RESULTC_CV(clsDB.DbCon, this.gStrDRCode, strFDate,strTDate,strWs,strPano,chkStatus);

            this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, lbExSQL.sSel_EXAM_RESULTC_CV, lbExSQL.nSel_EXAM_RESULTC_CV });
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
            spread.setColStyle(spd, -1,(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV.ROWID, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV.CHKDATE, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_CV.CHK, clsSpread.enmSpdType.CheckBox);

            // 4. 정렬
            spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);




        }

        void setCtrlSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;

            string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {

                strHeader = spread.setSpdPrint_String("혈액 감염 등록 내역", new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += spread.setSpdPrint_String("조회기간:" + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += spread.setSpdPrint_String("출력시간:" + s + "       PAGE : /p / /pc", new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strFoot = "/c/f1포항성모병원 임상병리과";

                clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, false, false, false, true);

                spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);

            }
        }


    }
}
