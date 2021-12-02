using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupHELP01.cs
    /// Description     : 진단검사의학과 HELP
    /// Author          : 김홍록
    /// Create Date     : 2017-06-17
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "d:\psmh\exam\exinfect\ExInfect90.frm" />
    public partial class frmComSupLbExHELP01 : Form
    {
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();

        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN enmExamSpecodeGubun;
        string gStrHelpNm;
        bool isYn;
        int nRow;
        bool gIsAutoClose;

        bool isSlip = false;
        string gStrPano = string.Empty;
        string gStrSname = string.Empty;
        string gStrBdate = string.Empty;
        string gStrDept = string.Empty;
        object gSender = null;

        public delegate void PSMH_RETURN_VALUE(object sender, string code, string name, string Yname);
        public event PSMH_RETURN_VALUE ePsmhReturnValue;

        public frmComSupLbExHELP01(clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN pEnmExamSpecodeGubun, object pSender
                                , bool isAutoClose = false, string strHelpName = "", bool isYname = false)
        {
            InitializeComponent();

            this.enmExamSpecodeGubun = pEnmExamSpecodeGubun;
            this.gStrHelpNm = strHelpName;
            this.isYn = isYname;
            this.gIsAutoClose = isAutoClose;
            this.gSender = pSender;
            setEvent();
        }

        public frmComSupLbExHELP01(bool isSlip, string strPano, string strSname, string strBdate, string strDept )
        {
            InitializeComponent();

            this.isSlip = isSlip;

            if (isSlip == false)
            {
                this.Close();
            }

            this.gStrPano = strPano;
            this.gStrSname = strSname;
            this.gStrBdate = strBdate;
            this.gStrDept = strDept;

            setEvent();

        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);

            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            
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
            setCtrlTitle();
            setCtrlSpread();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnDelete)
            {
                this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_RETURN_VALUE);
                this.ePsmhReturnValue(this.gSender, "", "", "");

                if (this.gIsAutoClose == true)
                {
                    this.Close();
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, (int)e.Column);
                return;
            }

            nRow = e.Row;
            string strCode;
            string strName;
            string strYName;

            strCode = this.ssMain.ActiveSheet.Cells[nRow, 0].Text;
            strName = this.ssMain.ActiveSheet.Cells[nRow, 1].Text;
            strYName = this.ssMain.ActiveSheet.Cells[nRow, 2].Text;

            this.ePsmhReturnValue += new PSMH_RETURN_VALUE(ePSMH_RETURN_VALUE);

            this.ePsmhReturnValue(this.gSender, strCode, strName, strYName);

            if (this.gIsAutoClose == true)
            {
                this.Close();
            }
        }

        void ePSMH_RETURN_VALUE(object sender, string code, string name, string Yname)
        {

        }

        void setCtrlTitle()
        {
            string strTitle = "";

            if (isSlip == true)
            {
                this.lblTitle.Text = "수납내역(" + this.gStrPano + "/" + this.gStrSname + "/" + this.gStrBdate + "/" + this.gStrDept + ")";
                this.lblTitleSub0.Text = "수납내역리스트";

                return;

            }
            else
            {

                switch (enmExamSpecodeGubun)
                {
                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.WS:
                        strTitle = "WS";
                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.EQU:
                        strTitle = "장비";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.SPEC:
                        strTitle = "검체";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.TUBE:
                        strTitle = "용기";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.VOLUME:
                        strTitle = "채혈량";
                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.COMMENT:
                        strTitle = "비고";
                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.HELP:
                        strTitle = "도움말";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.FOOT:
                        strTitle = "Footer";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.UNIT:
                        strTitle = "단위";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.CANCEL:
                        strTitle = "취소사유";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.RESULT_CHANGE:
                        strTitle = "결과변경사유";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.RESULT_CHANGE_ANAN:
                        strTitle = "병리변경사유";

                        break;

                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.MICRO:
                        strTitle = "미생물";

                        break;
                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.SLIP_SPECIMAN:
                        strTitle = "슬립별검체";
                        break;
                    case clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.OCS_SUBCODE:
                        strTitle = "처방추가정보";
                        break;

                    default:
                        break;
                }


                this.lblTitle.Text = strTitle + " HELP";
                this.lblTitleSub0.Text = strTitle + " 리스트";
            }
        }

        void setCtrlSpread()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DataSet ds = null;
            DataSet dsTmp = null;

            if (this.isSlip == true)
            {
                ds = lbExSQL.sel_OPD_SLIP(clsDB.DbCon, this.gStrPano, this.gStrDept, this.gStrBdate);
                setSpdStyle(this.ssMain, ds, lbExSQL.sSel_OPD_SLIP, lbExSQL.nSel_OPD_SLIP);

                return;

            }

            ds = lbExSQL.sel_EXAM_SPECODE_CODE_HELP(clsDB.DbCon, this.enmExamSpecodeGubun, "", this.gStrHelpNm);

            if (ComFunc.isDataSetNull(ds) == false)
            {
                dsTmp = ds.Clone();
                if (this.isYn == true)
                {
                    foreach (DataRow item in ds.Tables[0].Select("YNAME LIKE '" + this.gStrHelpNm + "%'"))
                    {
                        dsTmp.Tables[0].ImportRow(item);
                    }

                    dsTmp.Tables[0].DefaultView.Sort = "YNAME ASC";

                    setSpdStyle(this.ssMain, dsTmp, lbExSQL.sEXAM_SPECODE_GUBUN, lbExSQL.nEXAM_SPECODE_GUBUN);
                }
                else
                {
                    setSpdStyle(this.ssMain, ds, lbExSQL.sEXAM_SPECODE_GUBUN, lbExSQL.nEXAM_SPECODE_GUBUN);
                }
            }
        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.Rows.Count = 0;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);


            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 30;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            if (this.isSlip == true)
            {
                spread.setColStyle(spd, -1, (int)clsComSupLbExSQL.enmSel_OPD_SLIP.BUN, clsSpread.enmSpdType.Hide); // CODE
                return;
            }

            spread.setColStyle(spd, -1, 3, clsSpread.enmSpdType.Hide);

            if (enmExamSpecodeGubun == clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.OCS_SUBCODE)
            {
                spread.setColStyle(spd, -1, 0, clsSpread.enmSpdType.Hide); // CODE
                spread.setColStyle(spd, -1, 2, clsSpread.enmSpdType.Hide); // YNAME                
            }

            if (enmExamSpecodeGubun == clsComSupLbExSQL.enmEXAM_SPECODE_GUBUN.SUB)
            {
                spread.setColStyle(spd, -1, 3, clsSpread.enmSpdType.View); // YNAME                
            }
            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            // 5. sort, filter
            spread.setSpdFilter(spd, 0, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, 1, AutoFilterMode.EnhancedContextMenu, true);
            spread.setSpdFilter(spd, 3, AutoFilterMode.EnhancedContextMenu, true);

        }

    }
}
