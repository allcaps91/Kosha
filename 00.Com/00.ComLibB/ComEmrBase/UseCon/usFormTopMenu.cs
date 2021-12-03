using ComBase; //기본 클래스
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public  partial class usFormTopMenu : UserControl
    {
        /// <summary>
        /// 시간을 설정할 경우
        /// </summary>
        /// <param name="mkText"></param>
        public event SetTimeCheckShow rSetTimeCheckShow;
        public delegate void SetTimeCheckShow(ComboBox mkText);

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        public event SetSave rSetSave;
        public delegate void SetSave(string strFrDate, string strFrTime);

        /// <summary>
        /// 임시저장
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        public event SetSaveTemp rSetSaveTemp;
        public delegate void SetSaveTemp(string strFrDate, string strFrTime);

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        public event SetDel rSetDel;
        public delegate void SetDel(string strFrDate, string strFrTime);

        /// <summary>
        /// 화면 정리
        /// </summary>
        public event SetClear rSetClear;
        public delegate void SetClear();

        /// <summary>
        /// 프린터
        /// </summary>
        public event SetPrint rSetPrint;
        public delegate void SetPrint();

        /// <summary>
        /// 프린터 빈서식지
        /// </summary>
        public event SetPrintNull rSetPrintNull;
        public delegate void SetPrintNull();

        /// <summary>
        /// 미검수, 검수완료
        /// </summary>
        public event SetComplete rSetComplete;
        public delegate void SetComplete();

        /// <summary>
        /// 권한(코딩)
        /// </summary>
        public event SetCoading rSetCoading;
        public delegate void SetCoading();

        /// <summary>
        /// 변경이력
        /// </summary>
        public event SetHisSearch rSetHisSearch;
        public delegate void SetHisSearch();

        /// <summary>
        /// 폼을 닫기
        /// </summary>
        public event EventClosed rEventClosed;
        public delegate void EventClosed();


        //private usTimeSet usTimeSet;

        //차트 일자 수정을 못하게
        public void setDisableChartDate()
        {
            dtMedFrDate.Enabled = false;
            txtMedFrTime.Enabled = false;
        }

        //차트 일자 수정 가능
        public void setEnableChartDate()
        {
            dtMedFrDate.Enabled = false;
            txtMedFrTime.Enabled = false;
        }

        public usFormTopMenu()
        {
            InitializeComponent();
        }

        private void txtMedFrTime_DoubleClick(object sender, EventArgs e)
        {
            rSetTimeCheckShow(txtMedFrTime);
        }

        private void usFormTopMenu_Load(object sender, EventArgs e)
        {
            //ComFunc.SetDefaultButtonBg(this);
            //ComFunc.SetFormSkin(this);
            if(clsType.User.AuAPRINTIN == "1")
            {
                mbtnPrintNull.Top = mbtnClear.Top;
                mbtnPrintNull.Left = mbtnClear.Left;

                mbtnComplete.Top = mbtnPrint.Top;
                mbtnComplete.Left = mbtnPrint.Left;

                mbtnPrint.Top = mbtnDelete.Top;
                mbtnPrint.Left = mbtnDelete.Left;
            }
            else
            {
                mbtnComplete.Top = mbtnClear.Top;
                mbtnComplete.Left = mbtnClear.Left;

                //mbtnClear.Width = 68;
                //mbtnComplete.Width = mbtnClear.Width;

                mbtnPrintNull.Top = mbtnPrint.Top;
                mbtnPrintNull.Left = mbtnPrint.Left;
            }

            //mbtnPrint.Top = mbtnDelete.Top;
            //mbtnPrint.Left = mbtnDelete.Left;

            mbtnAuthority.Top = mbtnSaveTemp.Top;
            mbtnAuthority.Left = mbtnSaveTemp.Left;

            if (clsType.User.AuAMANAGE == "1")
            {
                mbtnHisSearch.Top = mbtnExit.Top;
                mbtnHisSearch.Left = mbtnExit.Left;

                mbtnExit.Left += mbtnHisSearch.Width;
            }

            clsEmrFunc.SetTimeComboBox(txtMedFrTime);
            //clsEmrFunc.SetTimeComboBox(txtMedFrTime);
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            rSetPrint();
        }


        private void mbtnPrintNull_Click(object sender, EventArgs e)
        {
            if (rSetPrintNull == null)
                return;

            rSetPrintNull();
        }

        private void mbtnClear_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("Clear하시겠습니까?", "Clear",MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            rSetClear();
        }

        private void mbtnSave_Click(object sender, EventArgs e)
        {
            string strFrDate = "";
            string strFrTime = "";

            strFrDate = VB.Format(dtMedFrDate.Value, "yyyyMMdd");
            strFrTime = VB.Replace(VB.Trim(txtMedFrTime.Text + ""), ":", "") + "00";

            if (strFrTime.Trim() == "")
            {
                ComFunc.MsgBox("작성시간을 입력하셔야 합니다.");
                return;
            }
            rSetSave(strFrDate, strFrTime);
        }

        private void mbtnSaveTemp_Click(object sender, EventArgs e)
        {
            if (rSetSaveTemp == null)
                return;

            string strFrDate = "";
            string strFrTime = "";

            strFrDate = VB.Format(dtMedFrDate.Value, "yyyyMMdd");
            strFrTime = VB.Replace(VB.Trim(txtMedFrTime.Text + ""), ":", "") + "00";

            if (strFrTime.Trim() == "")
            {
                ComFunc.MsgBox("작성시간을 입력하셔야 합니다.");
                return;
            }
            rSetSaveTemp(strFrDate, strFrTime);
        }

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            string strFrDate = "";
            string strFrTime = "";

            strFrDate = VB.Format(dtMedFrDate.Value, "yyyyMMdd");
            strFrTime = VB.Replace(VB.Trim(txtMedFrTime.Text + ""), ":", "") + "00";

            rSetDel(strFrDate, strFrTime);
        }

        private void mbtnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void mbtnTime_Click(object sender, EventArgs e)
        {
            rSetTimeCheckShow(txtMedFrTime);
        }

        private void mbtnComplete_Click(object sender, EventArgs e)
        {
            rSetComplete();
        }

        private void mbtnAuthority_Click(object sender, EventArgs e)
        {
            rSetCoading();
        }

        private void mbtnHisSearch_Click(object sender, EventArgs e)
        {
            rSetHisSearch();
        }
    }
}
