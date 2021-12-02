using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static ComBase.clsSpread;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : frmComSupLbExPrt02.cs
    /// Title or Description : 검사결과지 출력
    /// Author : 김홍록
    /// Create Date : 2017-06-02
    /// Update History : 
    /// </summary>
    public partial class frmComSupLbExPRT2 : Form
    {

        clsComSupLbExSQL lbExSql = new clsComSupLbExSQL();
        DataTable gDt = null;
        clsDB ClsDb = new clsDB();
        clsSpread ClsSpread = new clsSpread();
        string gStrSpecNo;
        int nHeaderCnt = 4;
        int nMaxRowSize = 30;
        int nRowHeight = 15;

        public frmComSupLbExPRT2()
        {
            InitializeComponent();
        }

        public frmComSupLbExPRT2(string strSpecNo, clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print_PB enmPrintType = clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print_PB.PB)
        {
            InitializeComponent();

            gStrSpecNo = strSpecNo;
            clearFrom();

            setEvent();

            displaySS(strSpecNo, enmPrintType);
        }

        public void setSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;

            if (this.gDt != null && this.gDt.Rows.Count > 0)
            {

                strHeader = ClsSpread.setSpdPrint_String(this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.WORKSTS_NAME.ToString()].ToString()
                                            , new Font("굴림체", 16, FontStyle.Bold)
                                            , clsSpread.enmSpdHAlign.Center
                                            , false
                                            , true);

                strHeader += "/n";
                strHeader += "/l                                                                           (" + " /p " + "/" + " /pc " + ")";

                SpdPrint_Margin setMargin = new SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                SpdPrint_Option setOption = new SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, true, false, false, false);

                ClsSpread.setSpdPrint(this.ssMain, false, setMargin, setOption, strHeader, strFoot, Centering.Horizontal);
            }

        }

        void setEvent()
        {
            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            setSpreadPrint();
        }

        void clearFrom()
        {

            this.lblTitleSub0.Text = string.Empty;

            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = string.Empty;
                }

            }

        }

        void displaySS(string strSpecNo, clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print_PB isPB)
        {

            //1. 전체 데이터
            gDt = lbExSql.sel_EXAM_RESULTC_Print(clsDB.DbCon, strSpecNo, isPB, true);

            string strFootType = string.Empty;
            int nMaxCol = this.ssMain.ActiveSheet.ColumnCount;

            if (gDt != null && gDt.Rows.Count > 0)
            {
                this.ssMain.AllowCellOverflow = true;

                setHeaderLabel();
                setHeaderFootTitle();

                this.ssMain.ActiveSheet.AddRows(nHeaderCnt, gDt.Rows.Count);

                for (int i = 0; i < gDt.Rows.Count; i++)
                {
                    strFootType = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.FOOTTYPE.ToString()].ToString();

                    if (int.Parse(strFootType) == (int)clsComSupLbExSQL.enmFootNoteType.RESULT)
                    {
                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 0].Text = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.EXAMNAME.ToString()].ToString();
                        this.ssMain.ActiveSheet.Rows.Get(nHeaderCnt + i).Height = nRowHeight;

                        this.ssMain.ActiveSheet.AddSpanCell(nHeaderCnt + i, 0, 1, 2);

                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 2].Text = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULT.ToString()].ToString();

                        //2017.12.14.김홍록 : REFER 부분에 대한 처리

                        if (string.IsNullOrEmpty(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_VALUE.ToString()].ToString().Trim()) == true &&
                                string.IsNullOrEmpty(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.UNIT.ToString()].ToString().Trim()) == true &&
                                string.IsNullOrEmpty(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_RAG.ToString()].ToString().Trim()) == true)
                        {
                            this.ssMain.ActiveSheet.AddSpanCell(nHeaderCnt + i, 2, 1, 5);
                        }

                        else if (string.IsNullOrEmpty(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_VALUE.ToString()].ToString().Trim()) == true &&
                            string.IsNullOrEmpty(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.UNIT.ToString()].ToString().Trim()) == true)


                        {
                            this.ssMain.ActiveSheet.AddSpanCell(nHeaderCnt + i, 2, 1, 4);
                        }
                        //2017.12.14.김홍록 : REFER 부분에 대한 처리
                        else if (string.IsNullOrEmpty(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_VALUE.ToString()].ToString().Trim()) == true)
                        {
                            this.ssMain.ActiveSheet.AddSpanCell(nHeaderCnt + i, 2, 1, 3);
                        }
                        else
                        {
                            this.ssMain.ActiveSheet.AddSpanCell(nHeaderCnt + i, 2, 1, 2);
                        }

                        //2017.12.14.김홍록 : REFER 부분에 대한 처리
                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 4].Text = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_VALUE.ToString()].ToString();
                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 5].Text = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.UNIT.ToString()].ToString();
                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 6].Text = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.REFER_RAG.ToString()].ToString();
                    }
                    else if (int.Parse(strFootType) == (int)clsComSupLbExSQL.enmFootNoteType.FOOTNOTE)
                    {
                        this.ssMain.ActiveSheet.Rows.Get(nHeaderCnt + i).Height = nRowHeight;

                        this.ssMain.ActiveSheet.AddSpanCell(nHeaderCnt + i, 0, 1, 5);
                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 0].Text = "    " + this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULT.ToString()].ToString();
                        this.ssMain.ActiveSheet.Cells[nHeaderCnt + i, 5].Text = "";
                    }

                    if ((i + 1) % 3 == 0)
                    {
                        FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None)
                                                                                                 , new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None)
                                                                                                 , new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None)
                                                                                                 , new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine)
                                                                                                 , new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

                        this.ssMain.ActiveSheet.Rows.Get(nHeaderCnt + i).Border = complexBorder8;
                    }

                }

                if (gDt.Rows.Count < nMaxRowSize)
                {
                    FarPoint.Win.LineBorder LineBorder = new FarPoint.Win.LineBorder(Color.Black, 1, false, true, false, false);

                    this.ssMain.ActiveSheet.AddRows(nHeaderCnt + gDt.Rows.Count, nMaxRowSize - gDt.Rows.Count);

                    this.ssMain.ActiveSheet.Cells[nHeaderCnt + gDt.Rows.Count, 0, nHeaderCnt + gDt.Rows.Count, this.ssMain.ActiveSheet.Columns.Count - 1].Border = LineBorder;

                    for (int i = 0; i < nMaxRowSize - gDt.Rows.Count; i++)
                    {
                        this.ssMain.ActiveSheet.Rows.Get(nHeaderCnt + gDt.Rows.Count + i).Height = nRowHeight;
                    }
                }

            }
            else
            {
                ComFunc.MsgBox("검사결과가 존재 하지 않습니다.");
            }
        }

        void setHeaderFootTitle()
        {
            this.ssMain.ActiveSheet.Cells[0, 1].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.PANO.ToString()].ToString();
            this.ssMain.ActiveSheet.Cells[0, 3].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SNAME.ToString()].ToString();
            this.ssMain.ActiveSheet.Cells[0, 6].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SEX_AGE.ToString()].ToString();

            this.ssMain.ActiveSheet.Cells[1, 1].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.WARD_NAME.ToString()].ToString();
            this.ssMain.ActiveSheet.Cells[1, 3].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DEPT_NAME.ToString()].ToString();
            this.ssMain.ActiveSheet.Cells[1, 6].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DR_NAME.ToString()].ToString();

            this.ssMain.ActiveSheet.Cells[5, 1].Text = this.gStrSpecNo;
            this.ssMain.ActiveSheet.Cells[5, 3].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.BLOODDATE.ToString()].ToString();
             
            //2019-04-04 안정수, 조건 추가 
            if (this.gDt.Rows.Count == 1)
            {
                this.ssMain.ActiveSheet.Cells[5, 5].Text = "보고일시:" + this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString();
            }
            else if (this.gDt.Rows.Count > 1)
            {
                for(int i = 0; i < this.gDt.Rows.Count; i++)
                {
                    if(this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString() != "")
                    {
                        this.ssMain.ActiveSheet.Cells[5, 5].Text = "보고일시:" + this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString();
                        break;
                    }
                }
            }

            this.ssMain.ActiveSheet.Cells[6, 1].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SPEC_NAME.ToString()].ToString();
            this.ssMain.ActiveSheet.Cells[6, 3].Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RECEIVEDATE.ToString()].ToString();


            string strUSER = this.gDt.Rows[this.gDt.Rows.Count - 1][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULT_NAME.ToString()].ToString().Trim();

            //2018-12-20 안정수, 이미경j 요청으로 CLO 검사이면서 최선택과장이 resultsabun으로 들어갔을 경우 Dr:최선택으로 표기되도록  
            if(this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.EXAMNAME.ToString()].ToString() == "CLO TEST" && strUSER == "최선택")
            {
                this.ssMain.ActiveSheet.Cells[6, 5].Text = "검사자:" + strUSER + " Dr:" + strUSER;
            }
            else
            {
                this.ssMain.ActiveSheet.Cells[6, 5].Text = "검사자:" + strUSER + " Dr:양성문";
            }
        }

        void setHeaderLabel()
        {
            this.lblTitleSub0.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.WORKSTS_NAME.ToString()].ToString();
            this.txtPano.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.PANO.ToString()].ToString();
            this.txtSname.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SNAME.ToString()].ToString();
            this.txtSex_Age.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SEX_AGE.ToString()].ToString();
            this.txtWard_Name.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.WARD_NAME.ToString()].ToString();
            this.txtDept_Name.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DEPT_NAME.ToString()].ToString();
            this.txtDr_Name.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.DR_NAME.ToString()].ToString();

            this.txtSpecNo.Text = gStrSpecNo;
            this.txtBloodDate.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.BLOODDATE.ToString()].ToString();

            //this.txtResultDate.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString();
            //2019-04-04 안정수 조건 추가
            if (this.gDt.Rows.Count == 1)
            {
                this.txtResultDate.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString();
            }
            else if (this.gDt.Rows.Count > 1)
            {
                for (int i = 0; i < this.gDt.Rows.Count; i++)
                {
                    if (this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString() != "")
                    {
                        this.txtResultDate.Text = this.gDt.Rows[i][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RESULTDATE.ToString()].ToString();
                        break;
                    }
                }
            }

            this.txtSpecName.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.SPEC_NAME.ToString()].ToString();
            this.txtReceiveDate.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.RECEIVEDATE.ToString()].ToString();
            this.txtExamUser.Text = this.gDt.Rows[0][clsComSupLbExSQL.enmSel_EXAM_RESULTC_Print.EXAM_USER.ToString()].ToString();

        }
    }
}
