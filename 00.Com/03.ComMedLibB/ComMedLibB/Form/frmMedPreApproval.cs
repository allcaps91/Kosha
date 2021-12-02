using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : frmMedPreApproval.cs
    /// Description     : 심율동전환제세동기거치술(ICD)및심장재동기화치료(CRT)사전승인신청서
    /// Author          : 박창욱
    /// Create Date     : 2017-11-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : 서브폼으로 호출 된 뒤 테스트 필요.
    /// </history>
    /// <seealso cref= "\Ocs\Frm사전승인신청서CRT.frm(Frm사전승인신청서CRT.frm) >> frmMedPreApproval.cs 폼이름 재정의" />	
    public partial class frmMedPreApproval : Form
    {
        public frmMedPreApproval()
        {
            InitializeComponent();
        }

        private void frmMedPreApproval_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = FormStartPosition.CenterScreen;

            ssView_Sheet1.Cells[4, 4].Text = clsVbfunc.GetPatientName(clsDB.DbCon, clsOrdFunction.Pat.PtNo);
            ssView_Sheet1.Cells[4, 6].Text = clsOrdFunction.Pat.Age + "(" + clsOrdFunction.Pat.Sex + ")";
            ssView_Sheet1.Cells[4, 8].Text = clsOrdFunction.Pat.JUMIN;

            ssView_Sheet1.Cells[5, 8].Text = clsVbfunc.GetBiName(clsOrdFunction.Pat.Bi);

            ssView_Sheet1.Cells[25, 3].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsOrdFunction.Pat.DrCode);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
    }
}
