using ComLibB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupLbEx
{

    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupLbExMIC01.cs
    /// Description     : MIC검사리스트
    /// Author          : 김홍록
    /// Create Date     : 2017-06-09
    /// Update History  : 
    /// </summary>
    /// <history>
    /// 2017.06.09.김홍록:심사팀 김순옥 팀장님과 협의 하에 출력버튼을 삭제함.
    /// 2017.06.09.김홍록:미래 내원일자에 대해서도 관리하지 않기로 함.
    /// </history>
    /// <seealso cref="\mir\miretc\miretc84.frm"/>
    public partial class frmComSupLbExVIEW04 : Form
    {

        DateTime sysdate;

        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();
     
        /// <summary>생성자</summary>
        public frmComSupLbExVIEW04()
        {
            InitializeComponent();
            setEvent();
            
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Move += new EventHandler(eFormMove);
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnExit.Click += new EventHandler(eBtnClose);            
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

        void eFormMove(object sender, EventArgs e)
        {

            Point p = new Point();

            p.X = this.Location.X + this.userPtInfo.Location.X;
            p.Y = this.Location.Y + this.userPtInfo.Location.Y + this.userPtInfo.Height * 4;

            this.userPtInfo.pPSMH_LPoint = p;

        }

        void setCtrl()
        {
            setCtrlDate();
            setCtrlSpread(false);
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpread(true);
        }

        void eBtnClose(object sender, EventArgs e)
        {
            this.Close();            
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate.AddDays(-1);
            this.dtpTDate.Value = sysdate;
        }

        void setCtrlSpread(bool isData)
        {
            DataSet ds = null;

            if (isData == true)
            {
                clsParam.enmComParamGBIO gbio = clsParam.enmComParamGBIO.ALL;

                if (this.rdoIOAll.Checked == true)
                {
                    gbio = clsParam.enmComParamGBIO.ALL;
                }
                else if (this.rdoI.Checked == true)
                {
                    gbio = clsParam.enmComParamGBIO.I;
                }
                else if (this.rdoO.Checked == true)
                {
                    gbio = clsParam.enmComParamGBIO.O;
                }

                clsComSupLbExSQL.enmSel_EXAM_RESULTC_MicResultOptient miOption = clsComSupLbExSQL.enmSel_EXAM_RESULTC_MicResultOptient.GROWTH;

                if (this.rdoGrowthAll.Checked == true)
                {
                    miOption = clsComSupLbExSQL.enmSel_EXAM_RESULTC_MicResultOptient.ALL;
                }
                else if (this.rdoGrowth.Checked == true)
                {
                    miOption = clsComSupLbExSQL.enmSel_EXAM_RESULTC_MicResultOptient.GROWTH;
                }
                else if (this.rdoNoGrowh.Checked == true)
                {
                    miOption = clsComSupLbExSQL.enmSel_EXAM_RESULTC_MicResultOptient.NO_GROWTH;
                }

                string sPtno = "";

                sPtno = userPtInfo.txtSearch_PtInfo.Text;


                ds = lbExSQL.sel_EXAM_RESULTC_MicResult(clsDB.DbCon, this.dtpFDate.Value.ToString("yyyy-MM-dd"), this.dtpTDate.Value.ToString("yyyy-MM-dd"), sPtno, miOption, gbio);
            }

            setSpdStyle(this.ssMain, ds, lbExSQL.sSpd_Sel_EXAM_RESULTC_MicResult, lbExSQL.nSpd_Sel_EXAM_RESULTC_MicResult);

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
            //spread.setSpdFilter(spd, 0, AutoFilterMode.EnhancedContextMenu, true);
            //spread.setSpdSort(spd, 0, true);

        }

    }
}
