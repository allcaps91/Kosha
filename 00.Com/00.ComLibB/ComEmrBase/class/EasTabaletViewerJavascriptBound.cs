using CefSharp;

namespace ComEmrBase
{
    public class EasTabaletViewerJavascriptBound
    {
        private frmEasTabletViewer tabletViewer = null;
        public EasViewerJavascriptBound EasViewerBound { get; set; }
        
        public EasTabaletViewerJavascriptBound(frmEasTabletViewer viewer)
        {
            this.tabletViewer = viewer;
        }
        /// <summary>
        /// 캔버스 추가
        /// </summary>
        /// <param name="position"></param>
        public void SetAddCanvas(string canvasId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("addBlankPage('"+ canvasId + "');");
            }
        }
        /// <summary>
        /// 캔버스 삭제
        /// </summary>
        /// <param name="position"></param>
        public void SetDeleteCanvas(string canvasId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("DeleteDirectCanvas('" + canvasId + "');");
            }
        }
        /// <summary>
        /// 캔버스 선택
        /// </summary>
        /// <param name="position"></param>
        public void SetActiveCanvas(string canvasId)
        {
            if (tabletViewer.Browser.CanExecuteJavascriptInMainFrame && tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("SetActiveCanvas('" + canvasId + "');");
            }
        }
        /// <summary>
        /// 이미지 추가
        /// </summary>
        /// <param name="position"></param>
        public void SetCreateIamge(string canvasId, string imageUrl, string imagePath)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("createIamge('" + canvasId + "','" + imageUrl + "','" + imagePath + "');");
            }
        }
        /// <summary>
        /// 스크롤 동기화
        /// </summary>
        /// <param name="position"></param>
        public void SetScroll(int position)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.SetViewerScroll(position);
            }

        }
        public void SetClear()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("clear();");
            }
        }
        public void SetUndo()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("undo();");
            }

        }
        public void SetRedo()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("redo();");
            }

        }
        public void SetTabletScroll(int position)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("syncScroll(" + position + ");");
            }
        }
        public void SetPcCanvasDraw(string canvasData)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.SetCanvasDraw(canvasData);
            }
        }
        /// <summary>
        /// Canvas 동기화
        /// </summary>
        /// <param name="canvasData"></param>
        public void SetCanvasDraw(string canvasData)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("syncCanvas(" + canvasData + ");");
            }            
        }
        /// <summary>
        /// 태블릿모니터의 서명 캔버스
        /// </summary>
        /// <param name="canvasData"></param>
        public void SetSignatureCanvasDraw(string canvasData)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.SetSignatureCanvasDraw(canvasData);
            }
        }

        public void setPatientSignatureCanvasToMonitor(string canvasData)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.SetPatientSignatureDarw(canvasData);
            }
        }

        //환자서명 동기화
        public void SetPatientSignatureDarw(string canvasData)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("syncPatientCanvas(" + canvasData + ");");
            }
        }
        public void MakePatientSignToMonitor(string signId, string signature, string signName)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.MakePatientSign(signId, signature, signName);

            }
        }
        public void MakeDoctorImageSignToMonitor(string signId,  string signName)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.MakeDoctorSignImage(signId,  signName);

            }
        }
        public void MakePatientSign(string signId, string signature, string signName)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("makeSignImage('" + signId + "', '" + signature + "', true, false, '" + signName + "' );");

            }
         
        }
        public void MakeDoctorSignImage(string signId,  string signName)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("makeDoctorSignImage('" + signId + "', '" + signName + "', '' );");
                
            }

        }

        
        /// <summary>
        /// 태블릿 뷰어 JSON 동기화
        /// </summary>
        /// <param name="json"></param>
        public void SetTabletJsonData(string json)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("syncControlJson(" + json + ");");
            }
        }
        public void SetWinformJsonData(string json)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.SetViewerJsonData(json);

            }
        }
        /// <summary>
        /// 환자서명창 열기
        /// </summary>
        public void OpenSignByPatient()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("OpenSignByPatient();");
            }
             

        }

        public void SetFormDataId(string formDataId, string easFormDataHistoryId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("setFormDataId(" + formDataId + ", "+ easFormDataHistoryId + ");");
            }
        }
        public void SetFormDataIdToMonitor(string formDataId, string easFormDataHistoryId)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.SetFormDataId(formDataId, easFormDataHistoryId);

            }
           
        }
        public void shortMoveClick()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("moveClick();");
            }

        }
        public void shortPencilClick()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("showPencil();");
            }

        }
        public void newShortPencilClick()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("newShortPencil();");
            }

        }
        /// <summary>
        /// 모니터의 펜 그리기 상태로 변경
        /// </summary>
        public void NewShortPencilClickToMonitor()
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.newShortPencilClick();

            }
        }

        /// <summary>
        /// 모니터의 지우개 상태로 변경
        /// </summary>
        public void shortEraserClickToMonitor()
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.shortEraserClick();

            }
        }
        public void shortEraserClick()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("ShowEraser();");
            }

        }

        public void OpenSignToMonitor(string signId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("openSign('" + signId + "');");
            }

        }
        public void OpenSign(string signId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("openSign('" + signId + "');");
            }
        }
        public void OpenModalImageSignatureToMonitor(string signId)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.OpenModalImageSignature(signId);

            }
        }
        public void OpenModalImageSignature(string signId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("openModalImageSignature('" + signId + "');");

            }

        }

        public void CloseTabletSignModal()
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("hideSignatureModal('');");
            
            }
        }
        public void CloseSignModal()
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.CloseViewerSignModal();

            }
        }
        public void DeleteSignImageToMonitor(string signId)
        {
            if (EasViewerBound != null)
            {
                EasViewerBound.DeleteSignImage(signId);

            }
        }
        public void DeleteSignImage(string signId)
        {
            if (tabletViewer.Browser.IsBrowserInitialized)
            {
                tabletViewer.Browser.ExecuteScriptAsync("deleteSignImageFromWinfrom('" + signId + "');");
            }
        }

      
    }
}
