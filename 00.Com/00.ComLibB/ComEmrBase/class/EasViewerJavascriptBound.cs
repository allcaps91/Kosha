using CefSharp;

namespace ComEmrBase
{
    /// <summary>
    /// 
    /// </summary>
    public class EasViewerJavascriptBound
    {
        public bool IsSigned { get; set; }
        public frmEasViewer viewer  { get; set; }
        public EasTabaletViewerJavascriptBound EasTabletViewerBound { get; set; }
        public string FormDataId { get; set; }
        public EasViewerJavascriptBound(frmEasViewer viewer)
        {
            this.viewer = viewer;
        }

        
        /// <summary>
        ///  winform-login.js 에서 호출
        /// </summary>
        /// <param name="userId"></param>
        public void SetSigned(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                IsSigned = false;
            }
            else
            {
                IsSigned = true;
                viewer.LoadUrl();
            }
        }

        /// <summary>
        /// 웹에서  프린트 준비 완료 호출
        /// </summary>
        public void SetPrintPageLoadCompleted()
        {
            viewer.Print();
        }
        /// <summary>
        /// 캔버스 추가
        /// </summary>
        /// <param name="position"></param>
        public void SetAddCanvas(string canvasId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetAddCanvas(canvasId);
            }
        }
        /// <summary>
        /// 캔버스 삭제
        /// </summary>
        /// <param name="position"></param>
        public void SetDeleteCanvas(string canvasId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetDeleteCanvas(canvasId);
            }
        }
        /// <summary>
        /// 캔버스 선택
        /// </summary>
        /// <param name="position"></param>
        public void SetActiveCanvas(string canvasId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetActiveCanvas(canvasId);
            }
        }
        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="position"></param>
        public void SetCreateIamge(string canvasId, string imageUrl, string imagePath)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetCreateIamge(canvasId, imageUrl, imagePath);
            }
        }
        /// <summary>
        /// write.js 스크롤 동기화
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void SetScroll(int position)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetTabletScroll(position);
            }
             
        }
        public void SetViewerScroll(int position)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("syncScroll(" + position + ");");
            }

        }
        public void SetClear()
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetClear();
            }

        }
        public void SetUndo()
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetUndo();
            }

        }
        public void SetRedo()
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetRedo();
            }

        }
        public void SetTabletCanvasDraw(string canvasData)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetCanvasDraw(canvasData);
            }             
        }
        /// <summary>
        /// Canvas 동기화
        /// </summary>
        /// <param name="canvasData"></param>
        public void SetCanvasDraw(string canvasData)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("syncCanvas(" + canvasData + ");");
            }
        }
        public void SetSignatureCanvasDraw(string canvasData)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("syncSignatureCanvas(" + canvasData + ");");
            }
        }
        public void SetWinformJsonData(string json)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetTabletJsonData(json);

            }
        }
        /// <summary>
        ///  뷰어 JSON 동기화
        /// </summary>
        /// <param name="json"></param>
        public void SetViewerJsonData(string json)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("syncControlJson(" + json + ");");

            }
        }
        public void setPatientSignatureCanvasToTablet(string canvasData)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetPatientSignatureDarw(canvasData);
            }
        }
        //환자서명 동기화
        public void SetPatientSignatureDarw(string canvasData)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("syncPatientCanvas(" + canvasData + ");");

            }
            
        }

        public void SetFormDataId(string formDataId, string easFormDataHistoryId)
        {
            FormDataId = formDataId;
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("setFormDataId(" + formDataId + "," + easFormDataHistoryId + ");");

            }
        }

        public void SetFormDataIdToTablet(string formDataId, string easFormDataHistoryId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.SetFormDataId(formDataId, easFormDataHistoryId);

            }
        }
        /// <summary>
        /// 태블릿모니터 서명창 열기
        /// </summary>
        /// <param name="signId"></param>
        public void OpenSignToTablet(string signId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.OpenSign(signId);

            }
        }
        public void OpenSign(string signId)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("openSign('" + signId + "');");

            }

        }
        public void OpenModalImageSignatureToTablet(string signId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.OpenModalImageSignature(signId);

            }
        }
        public void OpenModalImageSignature(string signId)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("openModalImageSignature('" + signId + "');");

            }

        }
        public void CloseSignModal()
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.CloseTabletSignModal();

            }
        }
        public void CloseViewerSignModal()
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("hideSignatureModal('');");

            }
        }
        /// <summary>
        /// 태블릿모니터의 펜 그리기 상태로 변경
        /// </summary>
        public void ShortPencilClick()
        {
            //shortPencilClick()
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.shortPencilClick();

            }
        }

        /// <summary>
        /// 태블릿모니터의 펜 그리기 상태로 변경
        /// </summary>
        public void NewShortPencilClickToTablet()
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.newShortPencilClick();

            }
        }
        public void newShortPencilClick()
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("newShortPencil();");
            }

        }

        /// <summary>
        /// 태블릿모니터의 지우개 상태로 변경
        /// </summary>
        public void shortEraserClickToTablet()
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.shortEraserClick();

            }
        }
        public void shortEraserClick()
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("ShowEraser();");
            }

        }

        /// <summary>
        /// 태블릿모니터의 펜을 이동가능 상태로 변경
        /// </summary>
        public void ShortMoveClick()
        {
            //shortPencilClick()
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.shortMoveClick();

            }
        }
        public void DeleteSignImageToTablet(string signId)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.DeleteSignImage(signId);

            }
        }
        public void DeleteSignImage(string signId)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("deleteSignImageFromWinfrom('" + signId + "');");
            }
        }


        public void MakePatientSignToTablet(string signId, string signature, string signName)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.MakePatientSign(signId, signature, signName);

            }
        }

        public void MakeDoctorImageSignToTablet(string signId, string signName)
        {
            if (EasTabletViewerBound != null)
            {
                EasTabletViewerBound.MakeDoctorSignImage(signId, signName);

            }
        }

        public void MakePatientSign(string signId, string signature, string signName)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("makeSignImage('" + signId + "', '" + signature + "', true, false, '" + signName + "' );");

            }
        }
        public void MakeDoctorSignImage(string signId, string signName)
        {
            if (viewer.Browser.IsBrowserInitialized)
            {
                viewer.Browser.ExecuteScriptAsync("makeDoctorSignImage('" + signId + "', '" + signName + "'  '');");

            }
        }
        
    }
}
