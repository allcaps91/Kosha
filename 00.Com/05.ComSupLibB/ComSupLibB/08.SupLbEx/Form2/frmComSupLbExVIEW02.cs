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
    public partial class frmComSupLbExVIEW02 : Form
    {

        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsParam param = new clsParam();

        DateTime sysdate;

        /// <summary>생성자</summary>
        public frmComSupLbExVIEW02()
        {
            InitializeComponent();

            setEvent();
            

        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnClear.Click += new EventHandler(eBtnEvent);
            this.supComPtInfo1.ePSMH_UcSupComPtSearch_VALUE += new UcSupComPtSearch.PSMH_RETURN_VALUE(ePSMH_ReturnValue);

            this.dtpFDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eCtrlKeyPress);

            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
                       
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

            this.dtpFDate.Focus();            
        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlSpread();
        }

        void eFormMove(object sender, EventArgs e)
        {
            Point p = new Point();

            p.X = this.Location.X + this.supComPtInfo1.Location.X;
            p.Y = this.Location.Y + this.supComPtInfo1.Location.Y + this.supComPtInfo1.Height * 3 + 5;

            this.supComPtInfo1.pPSMH_LPoint = p;
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

        void ePSMH_ReturnValue(object sender, string pano, string sname)
        {

        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate.AddDays(-1);
            this.dtpTDate.Value = sysdate;

        }

        void setCtrlSpread()
        {
            DataSet ds = null;

            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.ToString("yyyy-MM-dd");
            string strPano = this.supComPtInfo1.txtSearch_PtInfo.Text;

            if (IsCtrlSpreadError(strFDate, strTDate) == true)
            {
                return;
            }

            ds = lbExSQL.sel_EXAM_INFECT(clsDB.DbCon, strFDate, strTDate, strPano);

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



        }


    }
}
