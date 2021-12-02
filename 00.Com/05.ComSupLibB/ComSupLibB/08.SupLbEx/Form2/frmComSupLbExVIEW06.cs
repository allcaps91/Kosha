using ComLibB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExBST01.cs
    /// Description     : BST 병동 불출 현황 관리
    /// Author          : 김홍록
    /// Create Date     : 2017-05-15
    /// Update History  : 
    /// </summary>
    /// <history>
    /// 2017.06.09.김홍록:출력을 안쓰기로 하였음. 간호부 고경자 과장과, 진단검사의학과 업무를 확인 결과 출력을 하지 않고, 바로 조회하고 있음.
    ///                    화면 자체도 필요 없을 것으로 생각되지만 혹 보고 있는 사람이 있을 것으로 생각되어 삭제 하지 않음.
    /// </history>
    /// <seealso cref= "\nurse\nrer\FrmBST.frm, \nurse\nrinfo\FrmBST.frm" />
    public partial class frmComSupLbExVIEW06 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        
        DateTime sysdate;

        /// <summary>생성자</summary>
        public frmComSupLbExVIEW06()
        {
            InitializeComponent();

            setEvent();
            
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClose);
            this.rdoUnfrns.Click += new EventHandler(eRdoClick);
            
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

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == this.rdoUnfrns)
            {
                this.lblDateTitle.Text = "불출일자";
            }
            else if (sender == this.rdoFrns)
            {
                this.lblDateTitle.Text = "수납일자";
            }
        }

        void eBtnClose(object sender, EventArgs e)
        {
            this.Close();
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {


            setCtrlSpread();
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
            lstCbo.Add("ER.응급실");
            lstCbo.Add("HD.인공신장");
            lstCbo.Add("OR.수술실");

            DataTable dt = comSql.sel_BAS_WARD_COMBO(clsDB.DbCon, true);

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

            DataSet ds = null;
            string strWard = string.Empty;
            bool isFrns = this.rdoFrns.Checked == true ? true : false;

            strWard = method.getGubunText(this.cboWard.Text, ".");

            ds = lbExSQL.sel_EXAM_EXBST_Cnt(clsDB.DbCon, this.dtpFDate.Value.ToString("yyyy-MM-dd"), this.dtpTDate.Value.ToString("yyyy-MM-dd"), strWard, isFrns);
            setSpdStyle(this.ssMain, ds, lbExSQL.sSpd_Sel_EXAM_EXBST_Cnt, lbExSQL.nSpd_Sel_EXAM_EXBST_Cnt);

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
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, 0, AutoFilterMode.EnhancedContextMenu,true);
            spread.setSpdSort(spd,0, true);

            // 6. 소계 색
            UnaryComparisonConditionalFormattingRule unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "소계", false);
            unary.BackColor = method.cSpdRowSubTotalColor;
            spd.ActiveSheet.SetConditionalFormatting(-1, 0, unary);

            // 7. 총계 색
            UnaryComparisonConditionalFormattingRule unary1 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "총계", false);
            unary1.BackColor = method.cSpdRowSumColor;
            spd.ActiveSheet.SetConditionalFormatting(-1, 0, unary1);

            //spread.setColAlign(spd, -1, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);




        }
    }
}
