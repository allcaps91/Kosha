using ComBase; //기본 클래스
using ComBase.Controls;
using ComBase.Mvc;
using ComLibB;
using ComSupLibB.Com;
using ComSupLibB.Dto;
using ComSupLibB.Service;
using ComSupLibB.SupXray;
using System;
using System.Windows.Forms;

namespace ComSupLibB.SupFnEx
{
    public partial class frmComSupFnExSET02 : BaseForm
    {
        /// <summary>
        /// Class Name      : ComSupLibB.SupFnEx
        /// File Name       : frmComSupFnExSET02.cs
        /// Description     : 종검판독결과 입력
        /// Author          : 김경동
        /// Create Date     : 2019-09-19
        /// Update History  :  
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "ekg\Frm종검판독결과입력.frm(Frm종검판독결과입력) >> frmComSupFnExSET02.cs 폼이름 재정의" /> 
        /// 

        #region Declare Variable
        string FstrExName    = string.Empty;     //검사명
        string FstrSName     = string.Empty;     //수검자명
        string FstrSex       = string.Empty;     //성별
        string FstrPtno      = string.Empty;     //등록번호
        string FstrDept      = string.Empty;     //진료과
        string FstrSDate     = string.Empty;     //검사일자
        string FstrBDate     = string.Empty;     //처방일자
        string FstrOrderCode = string.Empty;     //오더코드
        string FstrDrName    = string.Empty;     //판독의사 성명
        string FstrExCode    = string.Empty;     //검진검사 코드

        long   FnWRTNO       = 0;                //접수번호

        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;
        HicResultService hicResultService = null;
        HeaResultService heaResultService = null;
        XrayResultnewService xrayResultnewService = null;
        clsComSup sup = null;
        frmComSupXRYSET01 frmComSupXRYSET01x = null;    //상용구 입력폼

        frmEmrViewer emrViewer = null;
        #endregion

        public frmComSupFnExSET02()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        /// <summary>
        /// 종검 초음파 스케쥴창 연동시 ..
        /// </summary>
        /// <param name="ExName">   검사명   </param>
        /// <param name="SName">    수검자명 </param>
        /// <param name="Ptno">     등록번호 </param>
        /// <param name="Dept">     진료과   </param>
        /// <param name="SDate">    검사일자 </param>
        /// <param name="BDate">    처방일자 </param>
        /// <param name="OrderCD">  오더코드 </param>
        public frmComSupFnExSET02(string ExName, string SName, string Ptno, string Dept, string SDate, string BDate, string OrderCD)
        {
            InitializeComponent();

            SetEvent();
            SetControl();

            FstrExName = ExName;
            FstrSName = SName;
            FstrPtno = Ptno;
            FstrDept = Dept;
            FstrSDate = SDate;
            FstrBDate = BDate;
            FstrOrderCode = OrderCD;
        }

        /// <summary>
        /// 변수 선언에 대한 설명 ...
        /// 컨트롤 Initialize 작업 및 객체 생성, 컨트롤 Option 처리
        /// 폼 전역 변수들을 생성자에서 호출하는 함수(SetControl)에서 선언하는 것을 지향.
        /// 반대로 폼 상단에서 폼 전역변수를 선언하게 되면 강한참조.
        /// ex) HicJepsuService hicJepsuService = new HicJepsuService(); => 강한참조
        /// </summary>
        private void SetControl()
        {
            hicJepsuService = new HicJepsuService();
            heaJepsuService = new HeaJepsuService();
            hicResultService = new HicResultService();
            heaResultService = new HeaResultService();
            xrayResultnewService = new XrayResultnewService();
            sup = new clsComSup();
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.FormClosed         += new FormClosedEventHandler(eFromClosed);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.btnResultNew.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                UpDate_Data();
            }
            else if (sender == btnResultNew)
            {
                //상용단어 등록
                if (frmComSupXRYSET01x == null)
                {
                    frmComSupXRYSET01x = new frmComSupXRYSET01("TO", Convert.ToInt32(clsType.User.Sabun));
                    frmComSupXRYSET01x.rSendMsg += new frmComSupXRYSET01.SendMsg(frmComSupXRYSET01x_SendMsg);
                }
                else
                {
                    frmComSupXRYSET01x = null;
                    frmComSupXRYSET01x = new frmComSupXRYSET01("TO", Convert.ToInt32(clsType.User.Sabun));
                    frmComSupXRYSET01x.rSendMsg += new frmComSupXRYSET01.SendMsg(frmComSupXRYSET01x_SendMsg);
                }

                frmComSupXRYSET01x.ShowDialog();
                sup.setClearMemory(frmComSupXRYSET01x);

                txtResult.Focus();
            }
        }

        private void UpDate_Data()
        {
            int result = 0;
            string strResult = string.Empty;

            if (txtResult.Text.Trim() == "") { return; }

            strResult = txtPanResult.Text + ComNum.VBLF + "※판정결과 ; " + txtResult.Text.Trim();

            //2014-08-05 종검 요청으로 판정의사를 추가함(LYJ)
            if (FstrDrName != "")
            {
                strResult = strResult + ComNum.VBLF + "※판정의사 ; " + FstrDrName;
            }
            strResult = strResult.Replace(ComNum.VBLF + ComNum.VBLF, ComNum.VBLF);

            if (FstrDept.Equals("TO"))
            {
                result = heaResultService.UpDate(strResult, clsType.User.IdNumber.To<long>(), FnWRTNO, FstrExCode);
            }
            else
            {
                result = hicResultService.UpDate(strResult, clsType.User.IdNumber.To<long>(), FnWRTNO, FstrExCode);
            }

            if (result <= 0)
            {
                MessageBox.Show("저장에 실패하였습니다.", "오류");
            }

            this.Close();

        }

        void frmComSupXRYSET01x_SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls)
        {
            txtResult.Text += argCls.Sogen;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            string strExCode = string.Empty;

            if (!FstrExName.IsNullOrEmpty()) { txtExName.Text = FstrExName; }
            if (!FstrSName.IsNullOrEmpty())  { txtSName.Text = FstrSName; }
            if (!FstrPtno.IsNullOrEmpty())   { txtPtno.Text = FstrPtno; }
            if (!FstrSDate.IsNullOrEmpty())  { dtpSDate.Text = FstrSDate.ToString(); }

            //종검 접수정보 읽기
            if (FstrDept.Equals("TO"))
            {
                HEA_JEPSU heaItem = heaJepsuService.GetItemByPtnoSDate(FstrPtno, FstrBDate);

                if (heaItem != null)
                {
                    txtSAge.Text = heaItem.AGE.To<string>() + "/" + heaItem.SEX;
                    dtpJDate.Text = heaItem.SDATE.ToString();
                    FnWRTNO = heaItem.WRTNO;
                }
            }
            //일반검진 접수정보 읽기
            else
            {
                HIC_JEPSU hicItem = hicJepsuService.GetItemByPtnoSDate(FstrPtno, FstrBDate);

                if (hicItem != null)
                {
                    txtSAge.Text = hicItem.AGE.To<string>() + "/" + hicItem.SEX;
                    dtpJDate.Text = hicItem.JEPDATE.ToString();
                    FnWRTNO = hicItem.WRTNO;
                }
            }

            //EMR 열기
            emrViewer = new frmEmrViewer(FstrPtno);
            emrViewer.StartPosition = FormStartPosition.CenterParent;
            emrViewer.Show(this);

            //판독문 읽기
            XRAY_RESULTNEW xItem = xrayResultnewService.GetItemByPtnoXCode(FstrPtno, FstrOrderCode, dtpSDate.Text);

            if (xItem != null)
            {
                txtPanResult.Text = xItem.RESULT;
                FstrDrName = clsVbfunc.GetInSaName(clsDB.DbCon, xItem.XDRCODE1.To<string>());
            }

            //오더코드로 종검검사코드 찾기 ... 
            //TX84       심장초음파      US22
            //TX44       24시간홀터      E6545
            //TX89       운동부하심전도  E6543
            //TX68       경동맥초음파 US-CADU1 (심장초음파가 있으면 US-CADU2)
            //TZ16       뇌혈류초음파    USTCD

            switch (FstrOrderCode)
            {
                case "US22":     strExCode = "TX84"; break;
                case "E6545":    strExCode = "TX44"; break;
                case "E6543":    strExCode = "TX89"; break;
                case "US-CADU1": strExCode = "TX68"; break;
                case "US-CADU2": strExCode = "TX68"; break;
                case "USTCD":    strExCode = "TZ16"; break;
                default:
                    break;
            }

            FstrExCode = strExCode;

            string strResult = string.Empty;

            if (FstrDept.Equals("TO"))
            {
                strResult = heaResultService.GetResultByWrtnoExCD(FnWRTNO, strExCode);
            }
            else
            {
                strResult = hicResultService.GetResultByWrtnoExCD(FnWRTNO, strExCode);
            }

            if (VB.InStr(strResult, "※판정결과 ;") > 0)
            {
                txtResult.Text = VB.STRCUT(strResult, "※판정결과 ;", "").Trim();
                if (VB.InStr(strResult, "※판정의사 ;") > 0)
                {
                    txtResult.Text = VB.STRCUT(txtResult.Text, "", "※판정의사 ;").Trim();
                    txtResult.Text = txtResult.Text.Replace(ComNum.VBLF, "");
                }
            }
            else
            {
                txtResult.Text = strResult;
            }

        }

        private void eFromClosed(object sender, FormClosedEventArgs e)
        {
            if(emrViewer != null)
            {
                emrViewer.Dispose();
                emrViewer = null;
            }
        }
    }
}
