using System.Collections.Generic;
using Fint.Event.Model;
using Fint.Sse.Adapter.Services;
using FINT.Model.Utdanning.Elev;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fint.Sse.Adapter.Tests.Services
{
    public class EventHandlerServiceTest
    {
        public EventHandlerServiceTest()
        {
            var json =
                "{\"corrId\":\"c978c986-8d50-496f-8afd-8d27bd68049b\",\"action\":\"health\",\"status\":\"NEW\",\"time\":1481116509260,\"orgId\":\"rogfk.no\",\"source\":\"source\",\"client\":\"client\",\"message\":null,\"data\": \"\"}";
            _evtObj = EventUtil.ToEvent<object>(json);

            _appSettingsMock = new Mock<IOptions<AppSettings>>();
            _httpServiceMock = new Mock<IHttpService>();

            _elevServiceMock = new Mock<IElevService>(); //PwfaService();
            _loggerMock = new Mock<ILogger<EventHandlerService>>();
            _statusServiceMock = new Mock<IEventStatusService>();

            _appSettingsMock.Setup(ap => ap.Value).Returns(new AppSettings
            {
                ResponseEndpoint = "https://example.com/api/response",
                StatusEndpoint = "https://example.com/api/status"
            });
            _httpServiceMock.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<Event<object>>()));
        }

        [Fact]
        public void Given_EventStatus_ADAPTER_REJECTED_Should_DoNothing()
        {
            // Arrange
            _evtObj.Status = Status.ADAPTER_REJECTED;

            SetupStatusServiceMock();
            SetupHandlerService();

            // Act
            _handlerService.HandleEvent(_evtObj);
            // Verify
            _httpServiceMock.Verify(x => x.Post(It.IsAny<string>(), It.IsAny<Event<object>>()), Times.Never());
        }

        [Fact]
        public void Given_EventStatus_ADAPTER_ACCEPTED_Should_PostResponse()
        {
            // Arrange
            _evtObj.Status = Status.ADAPTER_ACCEPTED;
            _evtObj.Action = ElevActions.GET_ALL_ELEV.ToString();
            _evtObj.Data = new List<object>();

            SetupStatusServiceMock();
            SetupHandlerService();

            // Act
            _handlerService.HandleEvent(_evtObj);
            // Verify
            _httpServiceMock.Verify(x => x.Post(It.IsAny<string>(), It.IsAny<Event<object>>()), Times.Once());
        }

        [Fact]
        public void Given_ElevAction_GET_ALL_ELEV_Should_CallGetAllElev()
        {
            // Arrange
            _evtObj.Status = Status.ADAPTER_ACCEPTED;
            _evtObj.Action = ElevActions.GET_ALL_ELEV.ToString();
            _evtObj.Data = new List<object>();

            SetupStatusServiceMock();
            SetupHandlerService();

            // Act
            _handlerService.HandleEvent(_evtObj);
            // Verify
            _elevServiceMock.Verify(x => x.GetAllElev(It.IsAny<Event<object>>()), Times.Once());
        }

        [Fact]
        public void Given_ElevAction_GET_ALL_BASISGRUPPE_Should_CallGetAllBasisgruppe()
        {
            // Arrange
            _evtObj.Status = Status.ADAPTER_ACCEPTED;
            _evtObj.Action = ElevActions.GET_ALL_BASISGRUPPE.ToString();
            _evtObj.Data = new List<object>();

            SetupStatusServiceMock();
            SetupHandlerService();

            // Act
            _handlerService.HandleEvent(_evtObj);
            // Verify
            _elevServiceMock.Verify(x => x.GetAllBasisgruppe(It.IsAny<Event<object>>()), Times.Once());
        }


        private void SetupStatusServiceMock()
        {
            _statusServiceMock.Setup(s => s.VerifyEvent(It.IsAny<Event<object>>())).Returns(() => _evtObj);
        }

        private void SetupHandlerService()
        {
            _handlerService = new EventHandlerService(
                _statusServiceMock.Object,
                _httpServiceMock.Object,
                _elevServiceMock.Object,
                _appSettingsMock.Object,
                _loggerMock.Object
            );
        }

        private readonly Mock<IEventStatusService> _statusServiceMock;
        private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
        private readonly Mock<IHttpService> _httpServiceMock;
        private readonly Mock<IElevService> _elevServiceMock;
        private readonly Mock<ILogger<EventHandlerService>> _loggerMock;
        private EventHandlerService _handlerService;
        private readonly Event<object> _evtObj;
    }
}