using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds 
    /// File Name       : clsComSupEnds.cs
    /// Description     : 진료지원 공통 내시경 메인 class
    /// Author          : 윤조연 
    /// Create Date     : 2017-06-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComSupEnds 
    {

        public enum enmEndoViewType { ENDO_GUME_VIEW, ENDO_DRUG_VIEW }; //조회공통폼 구분 

        public enum enm_EndsPart {ALL, ENDO, HEALTH }; //ENDO:내시경실 HEALTH:건진센터

        //public int[,] Endo_Pic = null; //내시경 사진 픽셀정보

    }



}
