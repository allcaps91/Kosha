namespace ComHpcLibB.Dto
{
    using ComBase.Mvc;
    using System;
    using System.Text;


    /// <summary>
    /// 
    /// </summary>
    public class EMR_CONSENT : BaseDto
    {

        /// <summary>
        /// 서식지 번호
        /// </summary>
        public long FORMNO { get; set; }

        /// <summary>
        /// 업데이트 번호
        /// </summary>
        public long UPDATENO { get; set; }

        /// <summary>
        /// 서식지이름
        /// </summary>
        public string FORMNAME { get; set; }

        /// <summary>
        /// 작성자
        /// </summary>
        public string CREATEDUSER { get; set; }

        /// <summary>
        /// 작성일시
        /// </summary>
        public DateTime? CREATED { get; set; }

        /// <summary>
        /// 전자동의서 시퀀스(EMRNO)
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 작성자여부
        /// </summary>
        public string IS_WRITE { get; set; }

        /// <summary>
        /// 등록번호
        /// </summary>
        public string PTNO { get; set; }

        /// <summary>
        /// 수검자명
        /// </summary>
        public string SNAME { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string HPHONE { get; set; }

        /// <summary>
        /// 현작업내용(공정)
        /// </summary>
        public string GONGJENG { get; set; }

        /// <summary>
        /// 현직부서명
        /// </summary>
        public string BUSE { get; set; }

        /// <summary>
        /// 부서명
        /// </summary>
        public string BUSENAME { get; set; }

        /// <summary>
        /// 입사일자
        /// </summary>
        public string IPSADATE { get; set; }

        /// <summary>
        /// 주소
        /// </summary>
        public string JUSO { get; set; }

        /// <summary>
        /// 유해인자명
        /// </summary>
        public string UCODENAMES { get; set; }

        /// <summary>
        /// 현직전입일자
        /// </summary>
        public string BUSEIPSA { get; set; }

        /// <summary>
        /// 폭로기간
        /// </summary>
        public string P_GIGAN { get; set; }

        /// <summary>
        /// 주민등록번호
        /// </summary>
        public string JUMINNO { get; set; }

        /// <summary>
        /// 주민등록번호 앞자리
        /// </summary>
        public string JUMINNO1 { get; set; }

        /// <summary>
        /// 주민등록번호 뒷자리
        /// </summary>
        public string JUMINNO2 { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        public string SEX { get; set; }

        /// <summary>
        /// 나이
        /// </summary>
        public int AGE { get; set; }

        /// <summary>
        /// 동의일자
        /// </summary>
        public string AGREEDATE { get; set; }

        /// <summary>
        /// 검진일자
        /// </summary>
        public string EXDATE { get; set; }

        /// <summary>
        /// 검진등록일자
        /// </summary>
        public string BDATE { get; set; }

        /// <summary>
        /// 진료과목
        /// </summary>
        public string DEPT { get; set; }

        /// <summary>
        /// 진료과목명
        /// </summary>
        public string DEPTNAME { get; set; }

        /// <summary>
        /// 사업장명
        /// </summary>
        public string LTDNAME { get; set; }

        /// <summary>
        /// 진료의사 사번
        /// </summary>
        public string DRNO { get; set; }

        /// <summary>
        /// 진료의사 성명
        /// </summary>
        public string DRNAME { get; set; }

        /// <summary>
        /// ASA 점수
        /// </summary>
        public string ASA { get; set; }

        /// <summary>
        /// 기존동의서 서식번호 코드
        /// </summary>
        public string FORMCODE { get; set; }

        /// <summary>
        /// 동의서삭제일자
        /// </summary>
        public string DELDATE { get; set; }

        /// <summary>
        /// 위내시경일자
        /// </summary>
        public string STOMACHDATE { get; set; }

        /// <summary>
        /// 대장내시경일자
        /// </summary>
        public string COLONDATE { get; set; }

        /// <summary>
        /// 검진접수번호
        /// </summary>
        public long WRTNO { get; set; }

        /// <summary>
        /// 건강검진문진 Data
        /// </summary>
        public StringBuilder MUNDATA { get; set; }

        /// <summary>
        /// 검사명
        /// </summary>
        public string EXNAME { get; set; }

        /// <summary>
        /// 일반검진 여부
        /// </summary>
        public string GJJONG11_YN { get; set; }

        /// <summary>
        /// 암검진 여부
        /// </summary>
        public string GJJONG31_YN { get; set; }

        /// <summary>
        /// 1일폭로시간
        /// </summary>
        public string P_GIGAN_1DAY { get; set; }

        /// <summary>
        /// 작업공정1
        /// </summary>
        public string WORK_GONGJENG1 { get; set; }
        /// <summary>
        /// 근무년수1
        /// </summary>
        public string WORK_YEAR1 { get; set; }
        /// <summary>
        /// 근무기간1
        /// </summary>
        public string WORK_DYAS1 { get; set; }

        /// <summary>
        /// 작업공정1
        /// </summary>
        public string WORK_GONGJENG2 { get; set; }
        /// <summary>
        /// 근무년수1
        /// </summary>
        public string WORK_YEAR2 { get; set; }
        /// <summary>
        /// 근무기간1
        /// </summary>
        public string WORK_DYAS2 { get; set; }

        /// <summary>
        /// 표적장기
        /// </summary>
        public string PYOJANGGI { get; set; }
        /// <summary>
        /// 진찰소견
        /// </summary>
        public string JINSOGEN { get; set; }
        /// <summary>
        /// 내시경 수면여부
        /// </summary>
        public string ENDO { get; set; }

        public long EMRNO { get; set; }


        public string DRGUBUN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EMR_CONSENT()
        {
        }
    }
}
