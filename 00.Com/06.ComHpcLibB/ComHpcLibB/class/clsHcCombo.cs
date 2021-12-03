
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsHcCombo
    {
        HicOrgancodeService hicOrgancodeService = new HicOrgancodeService();

        public string ComboLiver_SET(ComboBox cboLiver)
        {
            string rtnVal = "";

            cboLiver.Items.Clear();
            cboLiver.Items.Add("");
            cboLiver.Items.Add("1.향체 있음");
            cboLiver.Items.Add("2.향체 없음");
            cboLiver.Items.Add("3.B형간염보유자의심");
            cboLiver.Items.Add("4.한정보류");
            cboLiver.SelectedIndex = 0;

            return rtnVal;
        }

        public string ComboPanjeng1_SET(ComboBox cboPanjeng)
        {
            string rtnVal = "";

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("");
            cboPanjeng.Items.Add("1.정상A");
            cboPanjeng.Items.Add("2.정상B");
            cboPanjeng.Items.Add("3.질환의심(R)");
            cboPanjeng.Items.Add("5.정상B+질환의심");
            cboPanjeng.SelectedIndex = 0;

            return rtnVal;
        }

        public string ComboSahu_SET(ComboBox cboSahu)
        {
            string rtnVal = "";

            cboSahu.Items.Clear();
            cboSahu.Items.Add("");
            cboSahu.Items.Add("1.근로금지 및 제한");
            cboSahu.Items.Add("2.작업전환");
            cboSahu.Items.Add("3.근로시간 단축");
            cboSahu.Items.Add("4.근로중 치료");
            cboSahu.Items.Add("5.추적검사");
            cboSahu.Items.Add("6.보호구 착용");
            cboSahu.Items.Add("7.기타");
            cboSahu.SelectedIndex = 0;

            return rtnVal;
        }

        public string ComboPanjengNew1_SET(ComboBox cboPanjeng)
        {
            string rtnVal = "";

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("");
            cboPanjeng.Items.Add("1.정상A");
            cboPanjeng.SelectedIndex = 0;

            return rtnVal;
        }

        public string ComboPanjeng2_SET(ComboBox cboPanjeng)
        {
            string rtnVal = "";

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("");
            cboPanjeng.Items.Add("1.A (정상)");
            cboPanjeng.Items.Add("2.B (정상)");
            cboPanjeng.Items.Add("3.C1(직업병 요관찰자)");
            cboPanjeng.Items.Add("4.C2(일반질병 요관찰자)");
            cboPanjeng.Items.Add("5.D1(직업병유소견자)");
            cboPanjeng.Items.Add("6.D2(일반질병유소견자)");
            cboPanjeng.Items.Add("9.CN(야간작업)");
            cboPanjeng.Items.Add("A.D2(일반질병유소견자)");
            cboPanjeng.Items.Add("7.R (2차건진대상자)");
            cboPanjeng.Items.Add("8.U (미판정자)");
            cboPanjeng.SelectedIndex = 0;

            return rtnVal;
        }

        public string ComboSPanjeng_SET(ComboBox cboPanjeng)
        {
            string rtnVal = "";

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("");
            cboPanjeng.Items.Add("1.A (정상)");
            cboPanjeng.Items.Add("2.B (정상B)");
            cboPanjeng.Items.Add("3.C1(건강주의)");
            cboPanjeng.Items.Add("4.C2(건강주의)");
            cboPanjeng.Items.Add("5.D1(직업병)");
            cboPanjeng.Items.Add("6.D2(일반질병)");
            cboPanjeng.Items.Add("9.CN(야간작업)");
            cboPanjeng.Items.Add("A.DN(야간작업)");
            cboPanjeng.Items.Add("7.R (재검)");
            cboPanjeng.Items.Add("8.U (미판정자)");
            cboPanjeng.SelectedIndex = 0;

            return rtnVal;
        }
        public string ComboSPanjeng_SET_NEW(ComboBox cboPanjeng, string ArgJONG)
        {
            string rtnVal = "";

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("");
            cboPanjeng.Items.Add("1.A (정상)");
            switch (ArgJONG)
            {
                case "11": case "12": case "14": case "16": case "17": case "19": case "41": case "42": case "44": case "45":
                default:
                    cboPanjeng.Items.Add("2.B (정상B)");
                    break;
            }
            
            cboPanjeng.Items.Add("3.C1(건강주의)");
            cboPanjeng.Items.Add("4.C2(건강주의)");
            cboPanjeng.Items.Add("5.D1(직업병)");
            cboPanjeng.Items.Add("6.D2(일반질병)");
            cboPanjeng.Items.Add("9.CN(야간작업)");
            cboPanjeng.Items.Add("A.DN(야간작업)");
            cboPanjeng.Items.Add("7.R (재검)");
            cboPanjeng.Items.Add("8.U (미판정자)");
            cboPanjeng.SelectedIndex = 0;

            return rtnVal;
        }

        public string ComboEtcJil_SET(ComboBox cboEtcJil)
        {
            string rtnVal = "";

            cboEtcJil.Items.Clear();
            cboEtcJil.Items.Add("");
            cboEtcJil.Items.Add("A.특정감염성 질환");
            cboEtcJil.Items.Add("B.바이러스 및 기생충성 질환");
            cboEtcJil.Items.Add("C.악성신생물");
            cboEtcJil.Items.Add("D.양성신생물 및 혈액질환과 연역장해");
            cboEtcJil.Items.Add("E.내분비, 영양 및 대사질환");
            cboEtcJil.Items.Add("F.정신 및 행동장해");
            cboEtcJil.Items.Add("G.신경계의 질환");
            cboEtcJil.Items.Add("H.눈, 눈부속기와 귀 및 유양돌기의 질환");
            cboEtcJil.Items.Add("I.순환기계의 질환");
            cboEtcJil.Items.Add("J.호흡기계의 질환");
            cboEtcJil.Items.Add("K.소화기계의 질환");
            cboEtcJil.Items.Add("L.피부 및 피하조직의 질환");
            cboEtcJil.Items.Add("M   근골격계 및 결합조직의 질환");
            cboEtcJil.Items.Add("N.비뇨생식기계의 질환");
            cboEtcJil.Items.Add("O.임신, 출산 및 산욕");
            cboEtcJil.Items.Add("P.주산기에 기원한 특정병태");
            cboEtcJil.Items.Add("Q.선천성기형, 변형 및 염색체 이상");
            cboEtcJil.Items.Add("R.기타증상·징후와 임상검사의 이상소견");
            cboEtcJil.Items.Add("S.손상");
            cboEtcJil.Items.Add("T.다발성 및 기타 손상, 중독 및 그 결과");
            cboEtcJil.Items.Add("V.운수사고");
            cboEtcJil.Items.Add("W.불의의 손상의 기타 외인");
            cboEtcJil.Items.Add("X.고온장해 및 자해");
            cboEtcJil.Items.Add("Y.가해, 치료의 합병증 및 후유증");
            cboEtcJil.Items.Add("Z.건강상태에 영향을 주는 원인");
            cboEtcJil.SelectedIndex = 0;

            return rtnVal;
        }

        /// <summary>
        /// 표적장기코드 Add
        /// </summary>
        /// <param name="ComboOrgan"></param>
        /// <param name="chk"></param>
        public string ComboOrgan_SET(ComboBox cboOrgan)
        {
            string rtnVal = "";

            cboOrgan.Items.Clear();
            cboOrgan.Items.Add("");

            List<HIC_ORGANCODE> list = hicOrgancodeService.GetListAll("1", "");

            cboOrgan.SetItems(list, "CODE", "NAME", "", "", AddComboBoxPosition.Top);
            cboOrgan.SelectedIndex = 0;

            return rtnVal;
        }
    }


    
}
