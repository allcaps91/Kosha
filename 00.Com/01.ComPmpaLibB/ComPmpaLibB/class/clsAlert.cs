using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Windows.Forms;

/// <summary>
/// Description : 경고 메세지 모음
/// Author : 박병규
/// Create Date : 2018.01.12
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public class clsAlert
    {

        /// <summary>
        /// Alter MessageBox
        /// </summary>
        /// <param name="ArgGB"></param>
        public void Alert_Msg(string ArgGB)
        {
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();

            if (ArgGB == "PT+") //수가코드 (MM131, MM132, KK061, KK090, LA322) + 물리치료 발생시
            {
                clsPublic.GstrMsgTitle = "알림";
                clsPublic.GstrMsgList = "[MM131, MM132, KK061, KK090, LA322 ] + 물리치료와 동시 발생함." + '\r' + '\r';
                clsPublic.GstrMsgList += "가격이 저렴한 처치료 비급여로 처리 부탁합니다." + '\r';
                clsPublic.GstrMsgList += "(국소주사 비급여시 약제도 비급여 처리요망)" + '\r' + '\r';
            }
            else if (ArgGB == "GAM")//효도감액 정보확인
            {
                clsPublic.GstrMsgTitle = "효도감액 정보확인";
                clsPublic.GstrMsgList = "◆ " + clsPmpaPb.GstrSpecail_Gam + " ◆" + '\r' + '\r';
                clsPublic.GstrMsgList += clsPublic.GstrSysDate.Substring(0, 4) + "년 5월 한달 적용되는 효도감액 대상자임." + '\r';
                clsPublic.GstrMsgList += "수납시(치과제외) 감액율 확인요망!  (본인부담율 총액 50% 적용됨)" + '\r' + '\r';
            }

            else if (ArgGB == "NCOV-P2A")//코로나검사 팝업 정보확인
            {
                clsPublic.GstrMsgTitle = "코로나 재검사 대상자코드가 (NCOV-P2A) 발생";
                //2021-08-26 09:35 송인태주임 요청으로 금액은 메시지에서 뺌.
                //clsPublic.GstrMsgList = "◆접수2로 접수후,검사료 29,400원을 미수 잡아주세요.◆" + '\r' + '\r';
                clsPublic.GstrMsgList = "◆접수2로 접수후,검사료를 미수 잡아주세요.◆" + '\r' + '\r';
                clsPublic.GstrMsgList += " (진찰료 발생안됨)." + '\r';
                clsPublic.GstrMsgList += "미수멘트 : 내원 시(입원 수속시) 돈받아주세요. 코로나 재검사" + '\r' + '\r';
            }

            else if (ArgGB == "PENTAX")//  51종으로 자격을 변경 알림
            {
                clsPublic.GstrMsgTitle = "알림";
                clsPublic.GstrMsgList = " PENTAX 코드가 발생했습니다. " + '\r' + '\r';
                clsPublic.GstrMsgList += " 51종으로 자격을 변경해서 필수예방접종 미수를 발생 시켜주시기 바랍니다 " + '\r';
            }

            else if (ArgGB == "##13")//응급센터 진료 후 퇴실시 전염병환자 코드 발생시, 팝업창이 뜨도록 요청
            {
                clsPublic.GstrMsgTitle = "알림";
                clsPublic.GstrMsgList  = "  ##13 제1종 전염병환자(전염병예방법) 코드 발생 " + '\r' + '\r';
                clsPublic.GstrMsgList += " 원내약은 응급센터안에서 받아가도록 안내 해주시기 바랍니다." + '\r';
            }


            else if (ArgGB == "C4861C")//보건소 지원 수가 임시 사용 승인
            {
                clsPublic.GstrMsgTitle = "보건소 지원 검사확인";
                clsPublic.GstrMsgList = "★★ C4861C 코드는 보건소 지원 검사 코드입니다. ★★" + '\r' + '\r';
                clsPublic.GstrMsgList = "★★ C4861C 단독코드처방시 당일진찰료 받지 않습니다.  ★★" + '\r' + '\r';
                clsPublic.GstrMsgList = "★★ (3.신생아 / 43종.100% 접수해주세요) ★★" + '\r' + '\r';
                clsPublic.GstrMsgList += "C4861C 코드만 따로 기관 미수 잡아주세요. (★미수금액 : 19,750원★) " + '\r';
            }

            else if (ArgGB == "CO19-AZ1" || ArgGB == "CO19-PF1" || ArgGB == "CO19-MO1" || ArgGB == "BOOST_M1")//아스트라제네카 예방접종   2021-07-05 화이자 추가      2021-07-26 모더나 추가  2021-11-03 모더나 부스트샷
            {
                //clsPublic.GstrMsgTitle = "코로나 백신코드 발생(CO19-AZ1)";
                clsPublic.GstrMsgTitle = "코로나 백신코드 발생(" + ArgGB + ")";
                clsPublic.GstrMsgList = "★★ 19,220원 미수잡아주세요 (미수내용 : 보건소 청구건-코로나접종)  ★★" + '\r' + '\r';
                clsPublic.GstrMsgList += ArgGB + " 코드만 따로 미수 잡아주세요. (★미수금액 : 19,220원★) " + '\r';
            }

            else if (ArgGB == "HAVRI-A0")//보건소 지원 수가 임시 사용 승인
            {
                clsPublic.GstrMsgTitle = "보건소 지원 검사확인";
                clsPublic.GstrMsgList = "★★ HAVRI-A0 코드는 보건소 지원 접종코드입니다. ★★" + '\r' + '\r';
                clsPublic.GstrMsgList += "해당코드만 따로 기관 미수 잡아주세요. (★미수금액 : 52,600원 ★) " + '\r';
            }
            else if (ArgGB == "AVA160-0")//보건소 지원 수가 임시 사용 승인
            {
                clsPublic.GstrMsgTitle = "보건소 지원 검사확인";
                clsPublic.GstrMsgList = "★★ AVA160-0 코드는 보건소 지원 접종코드입니다. ★★" + '\r' + '\r';
                clsPublic.GstrMsgList += "해당코드만 따로 기관 미수 잡아주세요. (★미수금액 : 52,600원 ★) " + '\r';
            }
            else if (ArgGB == "DIAX23-0")//보건소 지원 수가 임시 사용 승인
            {
                clsPublic.GstrMsgTitle = "보건소 지원 검사확인";
                clsPublic.GstrMsgList = "★★ DIAX23-0  코드는 보건소 지원 접종코드입니다. ★★" + '\r' + '\r';
                clsPublic.GstrMsgList += "해당코드만 따로 기관 미수 잡아주세요. (★미수금액 :  19,220원 ★) " + '\r';
            }
            else if (ArgGB == "GAM_55")//계약처 + 본인감액률 확인
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "해당 환자는 계약처 진료비 감액대상 환자임." + '\r' + '\r';
                clsPublic.GstrMsgList += "계약처명 : " + CPF.READ_LTDNAME(clsDB.DbCon, clsPmpaType.TOM.GelCode) + '\r';
                clsPublic.GstrMsgList += "진료비 감액율 : " + clsPmpaType.GAM.Gam_Rate + "% 임." + '\r';
                clsPublic.GstrMsgList += "확인후 수납하시기 바랍니다." + '\r' + '\r';
            }
        
            else if (ArgGB == "NOREV")
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "◆ 노레보원정 수납 ◆" + '\r' + '\r';
                clsPublic.GstrMsgList += "일반자격으로 수납하시기 바랍니다." + '\r' + '\r';
            }
            else if (ArgGB == "PAPIL")
            {
                clsPublic.GstrMsgTitle = "유두종백신 주사 접종대상자";
                clsPublic.GstrMsgList = "◆ 만12세 유두종백신접종 대상자임. ◆" + '\r' + '\r';
                clsPublic.GstrMsgList += "유두종백신접종과 추가처방 있을경우," + '\r';
                clsPublic.GstrMsgList += "유두종백신 먼저 수납하시기 바랍니다." + '\r' + '\r';
            }
            else if (ArgGB == "F015")
            {
                clsPublic.GstrMsgTitle = "임신부 본인부담 경감 대상자";
                clsPublic.GstrMsgList = "◆ F015 임산부 대상입니다(본인부담경감) ◆" + '\r' + '\r';
                clsPublic.GstrMsgList += "접수비 후불처리 대상이며," + '\r';
                clsPublic.GstrMsgList += "예약자의 경우 접수비를 재정산 하시기 바랍니다." + '\r' + '\r';
            }
            else if (ArgGB == "B4052A") //배양검사 후불 확인메세지
            {
                clsPublic.GstrMsgTitle = "알림";
                clsPublic.GstrMsgList = "◆ 배양검사 후불 ◆ 환자에게 설명요망!!" + '\r' + '\r';
                clsPublic.GstrMsgList += "전화번호 : " + clsPmpaType.TBP.Tel + '\r';
                clsPublic.GstrMsgList += "휴 대 폰 : " + clsPmpaType.TBP.HPhone + '\r';
                clsPublic.GstrMsgList += "전화번호 및 휴대폰을 확인하시기 바랍니다." + '\r' + '\r';
            }
            else if (ArgGB == "진단서")
            {
                clsPublic.GstrMsgTitle = "경고";
                clsPublic.GstrMsgList = "수진자명 : " + clsPmpaType.TOM.sName + '\r';
                clsPublic.GstrMsgList += "환자구분 : " + clsPmpaType.TOM.Bi + '\r';
                clsPublic.GstrMsgList += "[@@ 진단서 @@] 로 접수했습니다. 처방을 확인후 수납요망!" + '\r';
            }
            else if (ArgGB == "GAM_BONIN")
            {
                clsPublic.GstrMsgTitle = "경고";
                clsPublic.GstrMsgList = "수진자명 : " + clsPmpaType.TOM.sName + '\r';
                clsPublic.GstrMsgList += "다시 접수후 수납요망!" + '\r';
            }
            else if (ArgGB == "TAX")
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "과세대상 처방이 발생하였습니다." + '\r';
                clsPublic.GstrMsgList += "처방 전체가 과세대상일 경우 접수 추가구분에서 [부가세대상]으로 접수하여 주시기 바랍니다." + '\r';
            }
            else if (ArgGB == "TCRC")
            {
                clsPublic.GstrMsgTitle = "확인";
                clsPublic.GstrMsgList = "해당 환자는 협력체결된 병의원에서 의뢰된 환자임." + '\r' + '\r';
                clsPublic.GstrMsgList += "(MRI, SONO 15% 할인 1회 적용)" + '\r';
                clsPublic.GstrMsgList += "당일 진료에 한해 할인적용됨." + '\r';
                clsPublic.GstrMsgList += "진료의뢰센터에 확인후 수납요망." + '\r';
            }

            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_V000_Opd_Msg_01()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "◆상병특례인 수가코드 발생◆" + '\r' + '\r';
            clsPublic.GstrMsgList += "처방오더 → 상병특례환자인 [@V***] 수가코드 발생하였으므로" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (F,G,S,T,C) 중 접수를 해야함." + '\r';
            clsPublic.GstrMsgList += "접수 작업을 다시 하시기 바랍니다." + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_V000_Opd_Msg_02()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "◆희귀.난치성 수가코드 발생◆" + '\r' + '\r';
            clsPublic.GstrMsgList += "처방오더 → 희귀.난치성질환자인 [@V***] 수가코드 발생하였으므로" + '\r';
            clsPublic.GstrMsgList += "희귀.난치성질환자인지 확인 요망" + '\r';
            clsPublic.GstrMsgList += "희귀.난치성 환자가 맞으면 접수취소후 접수시 [V000]으로 접수하시기 바랍니다." + '\r';
            clsPublic.GstrMsgList += "결핵환자(V000,V010,V246,V206,V231)이면 접수취소후 접수시 [V000]+중증코드로 접수하시기 바랍니다." + '\r';  //잠복결핵(V010) 추가(2021-06-28)
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_H000_Opd_Msg_01()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "◆희귀.난치성질환자 대상◆" + '\r' + '\r';
            clsPublic.GstrMsgList += "자격조회 → 희귀.난치성질환자 대상" + '\r';
            clsPublic.GstrMsgList += "희귀.난치성질환자는 접수구분에 (F,G,S,T,A,6) 중에 접수를 해야함." + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_H000_Opd_Msg_02()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "◆희귀.난치성질환자 대상◆" + '\r' + '\r';
            clsPublic.GstrMsgList += "자격조회 → 희귀.난치성질환자 대상" + '\r';
            clsPublic.GstrMsgList += "처방오더 → 희귀.난치성질환자인 [@V***] 수가코드가 없음" + '\r';
            clsPublic.GstrMsgList += "※ 진료과에서 희귀.난치성질환과 관련이 없다고 하면" + '\r';
            clsPublic.GstrMsgList += " → 희귀.난치성과 접수구분을 변경작업을 하시기 바랍니다." + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_H000_Opd_Msg_03()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "◆희귀.난치성질환자 대상◆" + '\r' + '\r';
            clsPublic.GstrMsgList += "자격조회 → 희귀.난치성질환자 대상" + '\r';
            clsPublic.GstrMsgList += "처방오더 → 희귀.난치성질환자인 [@V***] 수가코드가 발생" + '\r';
            clsPublic.GstrMsgList += "진료과 통화후 [@V***] 수가코드를 확인요망 !!" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_H000_Opd_Msg_04()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "◆희귀.난치성질환자 대상◆" + '\r' + '\r';
            clsPublic.GstrMsgList += "자격조회 → 희귀.난치성질환자 대상" + '\r';
            clsPublic.GstrMsgList += "처방오더 → 희귀.난치성 수가코드와 상병특례 수가코드가 동시 발생." + '\r';
            clsPublic.GstrMsgList += "동시에 수납처리를 할수 없음 → 진료과 전화요망!!" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Rare_Opd_Msg_01()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "차상위2종(건강보험)환자 + 희귀난치 !! 꼭 접수 구분변경" + '\r' + '\r';
            clsPublic.GstrMsgList += "접수구분변경을 (T) → (I) 변경요망" + '\r';
            clsPublic.GstrMsgList += "수납후 (I) → (T) 다시 변경해주세요" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Rare_Opd_Msg_02()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "차상위2종(건강보험)환자...!!! 꼭 접수를 취소 하시고 " + '\r' + '\r';
            clsPublic.GstrMsgList += "접수구분에 (I) 로 접수하시기 바랍니다." + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Rare_Opd_Msg_03()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "차상위2종(건강보험)환자 + 희귀난치 !! 꼭 접수 구분변경" + '\r' + '\r';
            clsPublic.GstrMsgList += "접수구분변경을 (T) → (J) 변경요망" + '\r';
            clsPublic.GstrMsgList += "수납후 (J) → (T) 다시 변경해주세요" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Rare_Opd_Msg_04()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "차상위2종(건강보험)환자...!!! 꼭 접수를 취소 하시고 " + '\r' + '\r';
            clsPublic.GstrMsgList += "접수구분에 (J) 로 접수하시기 바랍니다." + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Rare_Opd_Msg_05()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "차상위2종(건강보험)환자...!!! 접수구분이 I (상병1500) 로 접수했습니다. " + '\r' + '\r';
            clsPublic.GstrMsgList += "하지만 오더에 [@V~] 코드가 없습니다..오더를 확인후 접수하십시오" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="opdMessage.bas:READ_OPD_MSG_S11"/>
        public void Alert_Boho_Opd_Msg_01()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종 중증암환자 !! 꼭 접수를 취소 하시고" + '\r';
            clsPublic.GstrMsgList += "중증(암) 구분에(V193,V194) 입력 하시고" + '\r';
            clsPublic.GstrMsgList += "◆ (원외처방 또는 원외처방 + 검사항목) ◆" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (J) 상병1000 으로 접수하시기 바랍니다.." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Boho_Opd_Msg_02()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종 상병특례환자 !! 꼭 접수를 취소 하시고" + '\r';
            clsPublic.GstrMsgList += "◆ (원외처방 또는 원외처방 + 검사항목) ◆" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (J) 상병1000 으로 접수하시기 바랍니다.." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Boho_Opd_Msg_03()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종 중증암환자 !! 꼭 접수를 취소 하시고" + '\r';
            clsPublic.GstrMsgList += "중증(암) 구분에(V193,V194) 입력 하시고" + '\r';
            clsPublic.GstrMsgList += "◆ (진찰만 한 경우) ◆" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (J) 상병1000 으로 접수하시기 바랍니다.." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Boho_Opd_Msg_04()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종 상병특례환자 !! 꼭 접수를 취소 하시고" + '\r';
            clsPublic.GstrMsgList += "◆ (진찰만 한 경우) ◆" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (J) 상병1000 으로 접수하시기 바랍니다.." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Boho_Opd_Msg_05()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종 중증암환자 !! 꼭 접수를 취소 하시고" + '\r';
            clsPublic.GstrMsgList += "중증(암) 구분에(V193,V194) 입력 하시고" + '\r';
            clsPublic.GstrMsgList += "◆ (원외처방 미발생) ◆" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (I) 상병1500 으로 접수하시기 바랍니다.." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Boho_Opd_Msg_06()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종 상병특례환자 !! 꼭 접수를 취소 하시고" + '\r';
            clsPublic.GstrMsgList += "◆ (원외처방 미발생) ◆" + '\r';
            clsPublic.GstrMsgList += "접수구분에 (I) 상병1500 으로 접수하시기 바랍니다.." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Boho_Opd_Msg_07()
        {
            clsPublic.GstrMsgTitle = "경고";
            clsPublic.GstrMsgList = "의료급여 22종. HD환자는 @V수가코드가 있어야 합니다." + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }


        public void Alert_Msg_01()
        {
            clsPublic.GstrMsgTitle = "마약처방 대상자 주소확인";
            clsPublic.GstrMsgList = "마약이 투여된 환자로 반드시 정확한 상세주소가 필요함." + '\r' + '\r';
            clsPublic.GstrMsgList += "1) 정확한 상세주소 입력, " + '\r';
            clsPublic.GstrMsgList += "2) 또는 주소란에 연락처를 입력요망." + '\r';
            clsPublic.GstrMsgList += "3) 무명남일 경우 병원주소 입력요망 (270-1)" + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Msg_02()
        {
            clsPublic.GstrMsgTitle = "알림";
            clsPublic.GstrMsgList = "정확한 휴대폰 번호 입력요망 !" + '\r';
            clsPublic.GstrMsgList += "휴대폰이 없을시 공란." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Msg_04()
        {
            clsPublic.GstrMsgTitle = "수납불가";
            clsPublic.GstrMsgList = "재단 성직자 및 타재단 성직자 감액대상자는" + '\r';
            clsPublic.GstrMsgList += "보호자내원으로 수납할 수 없습니다." + '\r' + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Msg_05()
        {
            clsPublic.GstrMsgTitle = "수납시 - 본인부담확인";
            clsPublic.GstrMsgList = "만 65세이상 노인틀니(임플란트) 수가 발생!!" + '\r' + '\r';
            clsPublic.GstrMsgList += "접수비 발생이 안되며" + '\r';
            clsPublic.GstrMsgList += "반드시 접수(노인틀니) + 수납시 노인틀니(임플란트) 수가만 별도 수납처리요망!!" + '\r' + '\r';
            clsPublic.GstrMsgList += "(틀니)건강보험 30%, 차상위1종(C000) 5%, 차상위2종(E,F000) 15%" + '\r';
            clsPublic.GstrMsgList += "의료급여1종 5%, 의료급여2종 15%" + '\r' + '\r';
            clsPublic.GstrMsgList += "(임플란트)건강보험 30%, 차상위1종(C000) 10%, 차상위2종(E,F000) 20%" + '\r';
            clsPublic.GstrMsgList += "의료급여1종 10%, 의료급여2종 20%" + '\r' + '\r';
            clsPublic.GstrMsgList += "접수상세구분을 틀니 또는 임플란트로 수정후 수납처리요망!!" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_BonRate(clsPmpaType.BonRate cBON)
        {
            clsPublic.GstrMsgTitle = "심사팀 연락요망!";
            clsPublic.GstrMsgList = "자격구분 : " + cBON.BI + '\r' + '\r';
            clsPublic.GstrMsgList += "소아구분(Child)   : " + cBON.CHILD + '\r';
            clsPublic.GstrMsgList += "자격코드(Mcode)   : " + cBON.MCODE + '\r';
            clsPublic.GstrMsgList += "질환코드(Vcode)   : " + cBON.VCODE + '\r';
            clsPublic.GstrMsgList += "면제코드(OgpdBun) : " + cBON.OGPDBUN + '\r';
            clsPublic.GstrMsgList += "특정기호(Fcode)   : " + cBON.FCODE + '\r' + '\r';

            if (cBON.IO == "I")
                clsPublic.GstrMsgList += "입원 본인부담율이 설정되지 않았습니다. 계산불가!" + '\r';
            else
                clsPublic.GstrMsgList += "외래 본인부담율이 설정되지 않았습니다. 계산불가!" + '\r';


            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        public void Alert_Suga(string ArgSuga)
        {
            clsPublic.GstrMsgTitle = "전산실 연락요망!";
            clsPublic.GstrMsgList = "자동계산 되는 수가코드가 없습니다 !!" + '\r' + '\r';
            clsPublic.GstrMsgList += "수가계산이 정확히 되지 않습니다 !!" + '\r';
            clsPublic.GstrMsgList += "수가코드 : " + ArgSuga + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }
    }
}
