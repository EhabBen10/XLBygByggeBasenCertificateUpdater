using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Responses.PostChanges;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Moq;
using RestSharp;
using Xunit;

namespace CertificateUpdater.Services.Test.Services;
public sealed class PostChangesServiceUnitTest
{
	private readonly Mock<IClient<BaseSettings>> _clientMock = new();
	private readonly Mock<ICredentialProvider> _credentialProviderMock = new();
	private readonly Mock<IResponse<PostChangesResponse>> _postChangesReponseMock = new();
	private IPostChangesService _uut;
	private readonly CancellationToken _token = new();
	private readonly ICollection<CertificationChange> _certificationChanges = new List<CertificationChange>();
	private readonly string _testAspect4UserName = "aspect4UserName";
	private readonly string _testAspect4Password = "aspect4TestPassword";

	public PostChangesServiceUnitTest()
	{
		_uut = new PostChangesService(_clientMock.Object, _credentialProviderMock.Object);
	}

	[Fact]
	public void Construcor_ClientNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new PostChangesService(null!, _credentialProviderMock.Object));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("restClient", result.Message);
	}

	[Fact]
	public void Construcor_CredentialProviderNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new PostChangesService(_clientMock.Object, null!));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("credentialProvider", result.Message);
	}

	[Fact]
	public async void GetProductBatch_TunnrsIsEmpty_NullValueErrorIsReturned()
	{
		// Arrange
		_credentialProviderMock
			.Setup(x => x.GetAspect4Password()).Returns(_testAspect4Password);

		_credentialProviderMock
			.Setup(x => x.GetAspect4Username()).Returns(_testAspect4UserName);

		_clientMock
			.Setup(x => x
				.PostAsync<PostChangesResponse>(
					It.IsAny<RestRequest>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(_postChangesReponseMock.Object);

		// Act
		var result = await _uut.PostChangeBatch(_certificationChanges, _token);

		// Assert 
		Assert.False(result.IsFailure);
		Assert.True(result.IsSuccess);

		_clientMock
			.Verify(x => x.PostAsync<PostChangesResponse>(It.IsAny<RestRequest>(), _token), Times.Once);
	}
}
