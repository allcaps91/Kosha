using ComBase;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrSayu : Form
    {
        /// <summary>
        /// 생성자 열람 사유입력 오더전용
        /// </summary>
        string strGubun = string.Empty;

        public frmEmrSayu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// strGubun = "MTSOORDER" : 오더 열람 사유입력
        /// strGubun = "" : 차트 열람 사유입력
        /// </summary>
        /// <param name="strGubun"></param>
        public frmEmrSayu(string strGubun)
        {
            InitializeComponent();
            this.strGubun = strGubun;
        }

        private void frmEmrSayu_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            txtWhy.Text = "";
            if(strGubun.Equals("MTSOORDER"))
            {
                lblTitle.Text = "오더 열람 사유 입력";
                lblTitleSub0.Text = "오더 열람 사유 입력";
            }
            else
            {
                lblTitle.Text = "차트 열람 사유 입력";
                lblTitleSub0.Text = "차트 열람 사유 입력";
            }

        }

        private void rdoWhy_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.Checked == true)
            {
                rdo.ForeColor = Color.Red;
            }
            else
            {
                rdo.ForeColor = Color.Black;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            clsEmrPublic.mstrSayuRemark = string.Empty;

            foreach(Control ctrl in tableLayoutPanel1.Controls)
            {
                if(ctrl is RadioButton)
                {
                    RadioButton rdo = (RadioButton) ctrl;
                    if (rdo.Checked == true)
                    {
                        switch (VB.Right(rdo.Name, 2))
                        {
                            case "y0":
                                clsEmrPublic.mstrSayu = "002"; //'오더지시
                                break;
                            case "y1":
                                clsEmrPublic.mstrSayu = "003"; //'미비기록작성
                                break;
                            case "y2":
                                clsEmrPublic.mstrSayu = "004"; //'환자진료상담
                                break;
                            case "y3":
                                clsEmrPublic.mstrSayu = "005"; //'진단서/의뢰서/사본발금
                                break;
                            case "y4":
                                clsEmrPublic.mstrSayu = "006"; //'판독용 임상정보 열람
                                break;
                            case "y5":
                                clsEmrPublic.mstrSayu = "007"; //'약제업무
                                break;
                            case "y6":
                                clsEmrPublic.mstrSayu = "008"; //'원무보험
                                break;
                            case "y7":
                                clsEmrPublic.mstrSayu = "009"; //'연구 및 교육
                                break;
                            case "y8":
                                clsEmrPublic.mstrSayu = "010"; //'심사용
                                break;
                            case "y9":
                                clsEmrPublic.mstrSayu = "011"; //'집담회
                                break;
                            case "11":
                                clsEmrPublic.mstrSayu = "012"; //'협진,제한항생제 승인관리(의뢰서2013-729)
                                break;
                            //19-12-13 의료정보팀 이동춘 팀장님 요청으로
                            //외부에서 평가오신분들 용도로 추가함
                            case "12":
                                clsEmrPublic.mstrSayu = "013"; //'환자조사 및 평가용
                                break;
                            case "10":
                                clsEmrPublic.mstrSayu = "999"; //'기타사유
                                clsEmrPublic.mstrSayuRemark = txtWhy.Text.Trim();
                                break;
                        }
                        break;
                    }
                }
            }
            
            if(string.IsNullOrEmpty(clsEmrPublic.mstrSayu))
            {
                ComFunc.MsgBoxEx(this, "사유를 선택 해주세요.");
                return;
            }
            
            if(clsEmrPublic.mstrSayu.Equals("999") && clsEmrPublic.mstrSayuRemark.Length < 10)
            {
                ComFunc.MsgBoxEx(this, "기타사유의 내용을 10자이상 입력해주세요");
                return;
            }

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            clsEmrPublic.mstrSayu = "";
            clsEmrPublic.mstrSayuRemark = "";
            this.Close();
        }

    }
}
