using ComLibB;
using ComSupLibB.Com;
using System.Windows.Forms;
using System;
using System.Data;
using System.Drawing;
using FarPoint.Win.Spread;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExVIEW02.cs
    /// Description     : 혈액감염관리
   
    /// Author          : 김홍록
    /// Create Date     : 2017-07-03
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exmain\EXMAIN38.frm" />
    public partial class frmComSupLbExVIEW02 : Form, MainFormMessage
    {

        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsParam param = new clsParam();

        DateTime sysdate;

        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        #endregion

        /// <summary>생성자</summary>
        public frmComSupLbExVIEW02()
        {
            InitializeComponent();
            setEvent();           
        }

        public frmComSupLbExVIEW02(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setEvent()
        {
            this.Load                                       += new EventHandler(eFormLoad);

            this.Activated                                  += new EventHandler(eFormActivated);
            this.FormClosed                                 += new FormClosedEventHandler(eFormClosed);

            this.btnSearch.Click                            += new EventHandler(eBtnSearch);
            this.btnExit.Click                              += new EventHandler(eBtnEvent);
            this.btnClear.Click                             += new EventHandler(eBtnEvent);
                                            
            this.btnPrint.Click                             += new EventHandler(eBtnPrint);                

            this.dtpFDate.KeyPress                          += new KeyPressEventHandler(eCtrlKeyPress);
            this.dtpTDate.KeyPress                          += new KeyPressEventHandler(eCtrlKeyPress);

            this.rdo_ALL.Click                              += new EventHandler(eRdoClick);
            this.rdo_INF.Click                              += new EventHandler(eRdoClick);

            this.ssMain.CellDoubleClick                     += new CellClickEventHandler(eSpreadDClick);
                       
        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlSpread();
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnClear)
            {
                this.ssMain.ActiveSheet.Rows.Count = 0;                
            }
            
        }

        void eRdoClick(object sender, EventArgs e)
        {
            setCtrlSpread();
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpreadPrint();
        }


        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpread();
        }

        void eCtrlKeyPress(object sender, KeyPressEventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, e.Column);
                return;
            }
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate.AddDays(-1);
            this.dtpTDate.Value = sysdate;

        }

        void setCtrlSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;

            strHeader = spread.setSpdPrint_String("혈액감염 등록 리스트 ", new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += spread.setSpdPrint_String(" ", new Font("굴림체", 11, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            string strINF = this.rdo_INF.Checked == true ? "감염" : "전체";

            strHeader += spread.setSpdPrint_String("▶ 조회기간 : " + this.dtpFDate.Text + "~"+ this.dtpTDate.Text + " ▶ 조회조건 : " + strINF, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
            strFoot = spread.setSpdPrint_String("포항성모병원 진단검사의학과", new Font("굴림체", 14, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
            clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false, 0.85F);

            spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);
        }

        void setCtrlSpread()
        {
            DataSet ds = null;

            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.ToString("yyyy-MM-dd");

            if (IsCtrlSpreadError(strFDate, strTDate) == true)
            {
                return;
            }

            ds = lbExSQL.sel_EXAM_INFECT(clsDB.DbCon, strFDate, strTDate, this.rdo_INF.Checked);

            setSpdStyle(this.ssMain, ds, lbExSQL.sSel_EXAM_INFECT, lbExSQL.nSel_EXAM_INFECT);
        }

        bool IsCtrlSpreadError(string strFDate, string strTDate)
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

            if (nDate > clsMethod.nDateGap)
            {
                ComFunc.MsgBox("조회일자는 " + clsMethod.nDateGap.ToString() + "일을 넘을 수 없습니다.");
                this.dtpFDate.Focus();
                b = true;
            }

            return b;
        }



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
            spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_EXAM_INFECT.SPECNO, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_EXAM_INFECT.PANO, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, (int)clsComSupLbExSQL.enmSel_EXAM_INFECT.SNAME, AutoFilterMode.EnhancedContextMenu, true);



        }


    }
}
