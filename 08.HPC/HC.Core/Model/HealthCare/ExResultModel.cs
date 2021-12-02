namespace HC.Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;


    /// <summary>
    /// 일반건짐 검사결과 모델
    /// </summary>
    public class ExResultModel : BaseDto
    {
        /// <summary>
        /// 키, 신장
        /// </summary>
        public string HEIGHT { get; set; }
        /// <summary>
        /// 몸무게
        /// </summary>
        public string WEIGHT { get; set; }
        /// <summary>
        /// 체질량지수
        /// </summary>
        public string BMI { get; set; }
        /// <summary>
        /// 허리둘레
        /// </summary>
        public string WAITST { get; set; }
        /// <summary>
        /// 수축기 혈압
        /// </summary>
        public string SBP { get; set; }
        /// <summary>
        /// 이완기 혈압
        /// </summary>
        public string DBP { get; set; }
        /// <summary>
        /// 혈당
        /// </summary>
        public string BST { get; set; }
        /// <summary>
        /// 당화혈색소
        /// </summary>
        public string HBA1C { get; set; }
        /// <summary>
        /// 총콜레스테롤
        /// </summary>
        public string TDL { get; set; }
        public string HDL { get; set; }
        public string LDL { get; set; }
        /// <summary>
        /// 중성지방
        /// </summary>
        public string TRI { get; set; }
        /// <summary>
        /// 단백뇨
        /// </summary>
        public string PROTEIN { get; set; }
        public string PROTEIN_RESULT { get; set; }
        public string PROTEIN_RESCODE { get; set; }
        /// <summary>
        /// 크레아티닌
        /// </summary>
        public string CREATININE { get; set; }
        /// <summary>
        /// 사구체여과율
        /// </summary>
        public string GFR { get; set; }
        /// <summary>
        /// 흉부촬영
        /// </summary>
        public string CT { get; set; }
        /// <summary>
        /// 심전도
        /// </summary>
        public string CARDIO { get; set; }
        public string LU48 { get; set; }
        /// <summary>
        /// 안저검사
        /// </summary>
        public string TE15 { get; set; }
        /// <summary>
        /// 청력 왼쪽DB 500
        /// </summary>
        public string TH11 { get; set; }
        /// <summary>
        /// 청력 왼쪽DB 1000
        /// </summary>
        public string TH12 { get; set; }
        /// <summary>
        /// 청력 왼쪽DB 2000
        /// </summary>
        public string TH13 { get; set; }
        /// <summary>
        /// 청력 왼쪽DB 3000
        /// </summary>
        public string TH14 { get; set; }
        /// <summary>
        /// 청력 왼쪽DB 4000
        /// </summary>
        public string TH15 { get; set; }
        /// <summary>
        /// 청력 왼쪽DB 6000
        /// </summary>
        public string TH16 { get; set; }

        /// <summary>
        /// 청력 오른쪽DB 500
        /// </summary>
        public string TH21 { get; set; }
        /// <summary>
        /// 청력 오른쪽DB 1000
        /// </summary>
        public string TH22 { get; set; }
        /// <summary>
        /// 청력 오른쪽DB 2000
        /// </summary>
        public string TH23 { get; set; }
        /// <summary>
        /// 청력 오른쪽DB 3000
        /// </summary>
        public string TH24 { get; set; }
        /// <summary>
        /// 청력 오른쪽DB 4000
        /// </summary>
        public string TH25 { get; set; }
        /// <summary>
        /// 청력 오른쪽DB 6000
        /// </summary>
        public string TH26 { get; set; }

    }
}

