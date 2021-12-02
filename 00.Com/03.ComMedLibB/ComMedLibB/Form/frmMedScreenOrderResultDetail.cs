using System;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedScreenOrderResultDetail : Form
    {
        private clsDur.ResultInfo RsInfo;     //심평원점검결과

        public frmMedScreenOrderResultDetail(clsDur.ResultInfo argRsInfo)
        {
            InitializeComponent();

            RsInfo = argRsInfo;
        }

        private void frmMedScreenOrderResultDetail_Load(object sender, EventArgs e)
        {
            ssResultDetail.ActiveSheet.Cells[0, 1].Text = RsInfo.strExamType;           //점검구분
            ssResultDetail.ActiveSheet.Cells[0, 5].Text = RsInfo.m_nLevel;              //점검등급
            ssResultDetail.ActiveSheet.Cells[1, 1].Text = RsInfo.m_strMessage;          //점검내용
            ssResultDetail.ActiveSheet.Cells[2, 1].Text = RsInfo.m_strMedcNMA;          //입력약품
            ssResultDetail.ActiveSheet.Cells[3, 1].Text = RsInfo.m_strMedcNMB;          //복용약품
            ssResultDetail.ActiveSheet.Cells[4, 1].Text = RsInfo.m_strNotice;           //부작용정보
            ssResultDetail.ActiveSheet.Cells[5, 1].Text = RsInfo.m_strReasonCD;         //중복처방사유코드    
            ssResultDetail.ActiveSheet.Cells[5, 3].Text = RsInfo.m_strReason;           //중복처방사유

            ssResultDetail.ActiveSheet.Cells[6, 1].Text = RsInfo.m_strDpPrscYYMMDD;     //중복처방일자
            ssResultDetail.ActiveSheet.Cells[7, 1].Text = RsInfo.m_strDpPrscHMMSS;      //중복처방시간
            ssResultDetail.ActiveSheet.Cells[8, 1].Text = RsInfo.m_strDpPrscAdminName;  //중복처방기관명
            ssResultDetail.ActiveSheet.Cells[9, 1].Text = RsInfo.m_strDpPrscTel;        //중복처방기관전화
            ssResultDetail.ActiveSheet.Cells[10, 1].Text = RsInfo.m_strDpPrscName;      //중복처방의사명

            ssResultDetail.ActiveSheet.Cells[6, 5].Text = RsInfo.m_strDpMakeYYMMDD;     //중복조제일자
            ssResultDetail.ActiveSheet.Cells[7, 5].Text = RsInfo.m_strDpMakeHMMSS;      //중복조제시간
            ssResultDetail.ActiveSheet.Cells[8, 5].Text = RsInfo.m_strDpMakeAdminName;  //중복조제기관명
            ssResultDetail.ActiveSheet.Cells[9, 5].Text = RsInfo.m_strDpMakeTel;        //중복조제기관전화
            ssResultDetail.ActiveSheet.Cells[10, 5].Text = RsInfo.m_strDpMakeName;      //중복조제약사명

            ssResultDetail.ActiveSheet.Cells[11, 1].Text = RsInfo.m_strMedcCDB;         //약품코드
            ssResultDetail.ActiveSheet.Cells[12, 1].Text = RsInfo.m_strMedcNMB;         //약품명
            ssResultDetail.ActiveSheet.Cells[13, 1].Text = RsInfo.m_strGnlNMCDB;        //성분코드
            ssResultDetail.ActiveSheet.Cells[14, 1].Text = RsInfo.m_strGnlNMB;          //성분명

            ssResultDetail.ActiveSheet.Cells[12, 5].Text = RsInfo.m_fDDMqtyFreqB;        //1회투약
            ssResultDetail.ActiveSheet.Cells[13, 5].Text = RsInfo.m_fDDTotalMqtyB;       //1일회수
            ssResultDetail.ActiveSheet.Cells[14, 5].Text = RsInfo.m_nMdcnExecFreqB;      //총투여일
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
