namespace ComEmrBase
{
    public interface EmrChartForm
    {
        double SaveDataMsg(string strFlag); //저장 이벤트
        bool DelDataMsg(); //삭제 이벤트
        void ClearFormMsg(); //Clear 이벤트
        void SetUserFormMsg(double dblMACRONO); //사용자별 상용 템플릿 세팅 이벤트
        bool SaveUserFormMsg(double dblMACRONO); //사용자별 상용 템플릿 저장 이벤트
        void SetChartHisMsg(string strEmrNo, string strOldGb); //이전내역 뿌리기
        int PrintFormMsg(string strPRINTFLAG); //Print 이벤트
        int ToImageFormMsg(string strPRINTFLAG); //Print 이벤트
        
    }
}
