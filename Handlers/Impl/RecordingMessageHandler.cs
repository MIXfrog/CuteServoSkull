using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TestWebApp.Model;
using TestWebApp.Services;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace TestWebApp.Handlers.Impl
{
    public class RecordingMessageHandler : IHandler
    {
        private readonly IVkApiIntegrationService _vkApiIntegration;
        private readonly Message _message;

        private const string ErrorMessageResponseText = "Не удалось осуществить запись";
        private const string SuccessMessageResponseText = "Запись успешно добавлена";
        private const string SpreadsheetId = "1VfLI2ZENZFSl89QiMkbQ7OjiIZdY4scM6gJbk0bBR68";
        private static readonly string[] ScopeSheets = { SheetsService.Scope.Spreadsheets };
        private const string AppName = "CuteServoSkull";
        private const string ClientSecret = "/app/google-credentials.json";

        public RecordingMessageHandler(IVkApiIntegrationService vkApiIntegration, Message message)
        {
            _vkApiIntegration = vkApiIntegration;
            _message = message;
        }

        public void Handle()
        {
            try
            {
                var recordMessage = _message.Text.Substring(_message.Text.IndexOf(", ") + 1).Split(' ').ToList();

                var service = GetService(GetSheetCredential());

                WriteRecordMessage(service, recordMessage);


                _vkApiIntegration.SendMessage(SuccessMessageResponseText, _message);
            }
            catch(Exception ex)
            {
                _vkApiIntegration.SendMessage($"{ErrorMessageResponseText}: {ex.Message}", _message);
            }
        }

        private static void WriteRecordMessage(SheetsService service, IEnumerable<string> recordMessage)
        {
            var requests = new List<Request>();

            List<CellData> values = new List<CellData>();

            foreach (var val in recordMessage)
            {
                values.Add(new CellData
                {
                    UserEnteredValue = new ExtendedValue
                    {
                        StringValue = val
                    }
                });
            }

            var request = new Request
            {
                AppendCells = new AppendCellsRequest
                {
                    SheetId = 0,
                    Rows = new List<RowData> { new RowData { Values = values } },
                    Fields = "userEnteredValue"
                }
            };

            requests.Add(request);

            var busr = new BatchUpdateSpreadsheetRequest
            {
                Requests = requests
            };

            service.Spreadsheets.BatchUpdate(busr, SpreadsheetId).Execute();
        }

        private static SheetsService GetService(UserCredential credential)
        {
            return new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });
        }

        private static UserCredential GetSheetCredential()
        {
            using (var stream = new FileStream(ClientSecret, FileMode.Open, FileAccess.Read))
            {
                var credParh = Path.Combine(Directory.GetCurrentDirectory(), "sheetsCreds.json");
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    ScopeSheets,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credParh, true)).Result;
            }
        }
    }
}
