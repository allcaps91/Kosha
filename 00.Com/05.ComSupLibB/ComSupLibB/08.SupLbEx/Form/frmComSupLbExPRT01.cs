using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System.Data;
using System.Windows.Forms;


namespace ComSupLibB.SupLbEx
{

    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : frmComSupLbExPrt01.cs
    /// Title or Description : 진단검사의학과 출력용
    /// Author : 김홍록
    /// Create Date : 2017-06-07
    /// Update History : 
    /// </summary>
    public partial class frmComSupLbExPRT01 : Form
    {
        string strRowId;

        clsComSupLbEx LbEx = new clsComSupLbEx();
        clsComSupLbExSQL lbExSQL = new clsComSupLbExSQL();
        clsSpread spread = new clsSpread();

        public enum enmType {VRFC,PB,BLOOD};

        public frmComSupLbExPRT01()
        {
            InitializeComponent();
        }

        /// <summary>종합검증 생성자</summary>
        public frmComSupLbExPRT01(enmType prtType, string pRowId = "", string pano = "",  string bDate = "", string specNo = "",bool prePrint = false)
        {
            InitializeComponent();
            this.strRowId = pRowId;

            if (prtType == enmType.VRFC)
            {
                if (setVRFC(pRowId))
                {
                    printSS(this.ss_Vrfc, prePrint);
                }
            }
            else if (prtType == enmType.PB)
            {
                if (setPB(pano, bDate))
                {
                    printSS(this.ss_PB, prePrint);
                }
            }
            else if (prtType == enmType.BLOOD)
            {
                if (setBLOOD(specNo))
                {
                    printSS(this.ss_Blood, prePrint);
                }
            }
        }

        bool setVRFC(string pRowId)
        {

            bool b = true;
            DataTable dt = lbExSQL.sel_EXAM_VERIFY_Print(clsDB.DbCon, pRowId);

            if (dt != null && dt.Rows.Count > 0)
            {
                #region MVC -View

                this.ss_Vrfc.Sheets[0].Cells[2, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.PANO.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[2, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.WARD.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[3, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.SNAME.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[3, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DEPT_NAME.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[4, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.AGE_SEX.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[4, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DR_NAME.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[5, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DISEASE.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[8, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.DISDATE.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[9, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.ITEMS1.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[19, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.ITEMS2.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[30, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY1.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[30, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY2.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[31, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY3.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[31, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY4.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[32, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY5.ToString()].ToString().Trim();
                this.ss_Vrfc.Sheets[0].Cells[32, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.VERIFY6.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[36, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.COMMENTS.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[42, 2].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.RECOMMENDATION.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[49, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.JDATE.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[50, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.RDR_NAME.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[51, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.RDR_BUNHO.ToString()].ToString().Trim();

                this.ss_Vrfc.Sheets[0].Cells[53, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamVerifyPrint.JYYYY.ToString()].ToString().Trim();

                #endregion  
            }
            else
            {
                ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }

            return b;
        }

        bool setPB(string pano, string bDate)
        {
            bool b = true;

            DataTable dt = lbExSQL.sel_EXAM_ORDER_PB(clsDB.DbCon, pano, bDate);

            if (dt != null && dt.Rows.Count > 0)
            {
                #region MVC-View

                ss_PB.ActiveSheet.Cells[1, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.PANO].ToString();
                ss_PB.ActiveSheet.Cells[1, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.WARDCODE].ToString();

                ss_PB.ActiveSheet.Cells[2, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.SNAME].ToString();
                ss_PB.ActiveSheet.Cells[2, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.ROOMCODE].ToString();

                ss_PB.ActiveSheet.Cells[3, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.JUMIN].ToString();
                ss_PB.ActiveSheet.Cells[3, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.DEPTCODE].ToString();

                ss_PB.ActiveSheet.Cells[4, 1].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.BDATE].ToString();
                ss_PB.ActiveSheet.Cells[4, 3].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.DRNAME].ToString();

                ss_PB.ActiveSheet.Cells[9, 0].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.REQUEST1].ToString();
                ss_PB.ActiveSheet.Cells[11, 0].Value = dt.Rows[0][(int)clsComSupLbExSQL.enmSelExamOrderPb.REQUEST2].ToString();

                #endregion  
            }
            else
            {
                ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }
            return b;
        }

        bool setBLOOD(string specNo)
        {
            bool b = true;

            DataTable dt = lbExSQL.sel_EXAM_RESULTC_Blood(clsDB.DbCon, specNo);

            if (dt != null && dt.Rows.Count > 0)
            {
                #region MVC-View

                this.ss_Blood.Sheets[0].Cells[1, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.PANO.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[1, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.SNAME.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[1, 6].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.AGE_SEX.ToString()].ToString().Trim();

                this.ss_Blood.Sheets[0].Cells[2, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.ROOM.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[2, 3].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.DEPT_NAME.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[2, 6].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.DR_NAME.ToString()].ToString().Trim();

                this.ss_Blood.Sheets[0].Cells[3, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.DIAGNOSIS.ToString()].ToString().Trim();

                this.ss_Blood.Sheets[0].Cells[4, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BLOOD_TYPE.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[4, 5].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BLOOD_HIS.ToString()].ToString().Trim();

                //TODO : 2017.05.29.김홍록 : 다른 혈액이 1개 이상 난 경우를 반드시 확인 요망. 

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.ss_Blood.Sheets[0].Cells[8 + i, 0].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BLOOD_NAME.ToString()].ToString().Trim();
                    this.ss_Blood.Sheets[0].Cells[8 + i, 4].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.CNT.ToString()].ToString().Trim();
                }

                this.ss_Blood.Sheets[0].Cells[20, 1].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.BDATE.ToString()].ToString().Trim();
                this.ss_Blood.Sheets[0].Cells[20, 6].Text = dt.Rows[0][clsComSupLbExSQL.enmSelExamResultcBlood.ACT_DATE.ToString()].ToString().Trim();

                #endregion
            }
            else
            {
                ComFunc.MsgBox("조회 조건에 해당하는 데이터가 존재 하지 않습니다.");
                b = false;
            }

            return b;
        }

        void printSS(FpSpread o, bool prePrint)
        {

            string header = string.Empty;
            string foot = string.Empty;

            clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(30,10,10,10,10,10);
            clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                            , PrintType.All, 0, 0, false, false, false, false, false, false, false);

            spread.setSpdPrint(o, prePrint, margin, option, header, foot);
        }

    }
}
