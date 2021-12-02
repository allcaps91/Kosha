using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExVIEW07.cs
    /// Description     : 검체번호별 상세정보
    /// Author          : 김홍록
    /// Create Date     : 2017-07-03
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exmain\FrmCVListView.frm" />
    public partial class frmComSupLbExVIEW07 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
        clsParam param = new clsParam();

        string gStrSpecNo = string.Empty;
        Thread thread;
        DataTable gDt;

        int nGHR10 = 0;
        int nGMaste = 0;

        enum enmSSMain {SPECNO,PANO, SEX_AGE, SNAME , WARD_NAME , DEPT_NAME , DR_NAME , BLOODDATE , RECEIVEDATE , MST_STATUS };
        enum enmSSResult { EXAMNAME, RESULT, REFER, PANIC, DELTA, STATUS, UNIT, REFER_RAG, RESULTDATE, RESULT_NAME, MASTERCODE, SUBCODE};

        /// <summary>생성자</summary>
        /// <param name="strPano">환자번호</param>
        /// <param name="strFdate">조회시작일자</param>
        public frmComSupLbExVIEW07( string strSpecNo)
        {
            InitializeComponent();

            this.gStrSpecNo = strSpecNo;
            setEvent();           
        }

        void setEvent()
        {
            this.Load               += new System.EventHandler(eFormLoad);
            this.btnSearch.Click    += new EventHandler(eBtnSearch);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnPrint.Click     += new EventHandler(eBtnPrint);
            
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
            setCtrlSpread();
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

        void setCtrlSpread()
        {            
            thread = new Thread(() => threadSetCtrlSpread());
            thread.Start();
        }

        void threadSetCtrlSpread()
        {
            gDt = null;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);

            gDt = lbExSQL.sel_EXAM_RESULTC_Print(clsDB.DbCon, this.gStrSpecNo,clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print_PB.ALL,false);

            if (ComFunc.isDataTableNull(gDt) == false)
            {
                this.Invoke(new delegateSetSSMain(setSSMain), new object[]   { gDt });
                this.Invoke(new delegateSetSSResult(setResult), new object[] { gDt });

            }
            else
            {
                ComFunc.MsgBox("조회내역이 존재 하지 않습니다.");
            }

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

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

        delegate void delegateSetSSMain(DataTable dt);
        void setSSMain(DataTable dt)
        {
            this.ssMain.ActiveSheet.Rows.Count = 0;
            this.ssMain.ActiveSheet.Rows.Count = 1;

            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.SPECNO].Text = this.gStrSpecNo;
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.PANO].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.PANO].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.SEX_AGE].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SEX_AGE].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.SNAME].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SNAME].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.WARD_NAME].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.WARD_NAME].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.DEPT_NAME].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DEPT_NAME].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.DR_NAME].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DR_NAME].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.BLOODDATE].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.BLOODDATE].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.RECEIVEDATE].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RECEIVEDATE].ToString();
            this.ssMain.ActiveSheet.Cells[0, (int)enmSSMain.MST_STATUS].Text = dt.Rows[0][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.MST_STATUS].ToString();
        }

        delegate void delegateSetSSResult(DataTable dt);
        void setResult(DataTable dt)
        {
            string strMasterCode;

            this.ssResult.ActiveSheet.Rows.Count = 0;
            this.ssResult.ActiveSheet.Rows.Count = dt.Rows.Count;

            UnaryComparisonConditionalFormattingRule unary = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "V", false);
            unary.BackColor = param.cPaleGreen;
            ssResult.ActiveSheet.SetConditionalFormatting(-1, (int)enmSSResult.STATUS, unary);

            UnaryComparisonConditionalFormattingRule unary2 = new UnaryComparisonConditionalFormattingRule(UnaryComparisonOperator.EqualTo, "H", false);
            unary2.BackColor = param.cPaleGreen;
            ssResult.ActiveSheet.SetConditionalFormatting(-1, (int)enmSSResult.STATUS, unary2);


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.EXAMNAME].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.EXAMNAME].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.RESULT].Text   = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULT].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.REFER].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.PANIC].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.PANIC].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.DELTA].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DELTA].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.STATUS].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.STATUS].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.UNIT].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.UNIT].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.REFER_RAG].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_RAG].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.RESULTDATE].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE].ToString();
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.RESULT_NAME].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULT_NAME].ToString();

                strMasterCode = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.MASTERCODE].ToString();
                if (strMasterCode == clsParam.gPB)
                {
                    nGHR10 += 1;
                }
                else
                {
                    nGMaste += 1;
                }
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.MASTERCODE].Text = 
                this.ssResult.ActiveSheet.Cells[i, (int)enmSSResult.SUBCODE].Text = dt.Rows[i][(int)clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SUBCODE].ToString();
            }
        }

        void setCtrlSpreadPrint()
        {
            if (ComFunc.isDataTableNull(this.gDt) == false)
            {
                if (this.nGHR10 > 0)
                {
                    frmComSupLbExPRT2 f = new frmComSupLbExPRT2(this.gStrSpecNo, clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print_PB.PB);
                    f.setSpreadPrint();
                }

                if (this.nGMaste > 0)
                {
                    frmComSupLbExPRT2 f = new frmComSupLbExPRT2(this.gStrSpecNo, clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print_PB.NONE);
                    f.setSpreadPrint();
                }
                
            }
            else
            {
                ComFunc.MsgBox("출력할 데이터가 존재 하지 않습니다.");
            }
        }
    }
}
