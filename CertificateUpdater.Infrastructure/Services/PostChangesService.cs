using System.Xml;
using System.Xml.Serialization;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.RequestBodies;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Responses.PostChanges;
using CertificateUpdater.Services.Settings;
using RestSharp;
using RestSharp.Authenticators;
using Twilio.TwiML;

namespace CertificateUpdater.Services.Services;

public sealed class PostChangesService : IPostChangesService
{
	IClient<BaseSettings> RestClient { get; set; }
	ICredentialProvider CredentialProvider { get; set; }


	public PostChangesService(IClient<BaseSettings> restClient, ICredentialProvider credentialProvider)
	{
		RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
		CredentialProvider = credentialProvider ?? throw new ArgumentNullException(nameof(credentialProvider));
	}

	public async Task PostChangeBatch(ICollection<CertificationChange> changes, CancellationToken cancellationToken)
	{
		var request = new RestRequest("http://ditas02:1043/xzu_a4ws/rest/v1/query/XZU_DB_update/POST");
		var requestBody = new PostChangeBatchBody
		{
			Changes = changes,
		};

		string xml;
		var serializer = new XmlSerializer(typeof(PostChangeBatchBody));
		await using (var stringWriter = new Utf8StringWriter())
		{
			await using XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Async = true });
			serializer.Serialize(writer, requestBody);
			xml = stringWriter.ToString();
		}
		request.AddHeader("Accept", "application/xml");
		request.AddParameter("application/xml", xml, ParameterType.RequestBody);
		request.Authenticator = new HttpBasicAuthenticator(CredentialProvider.GetAspect4Username(), CredentialProvider.GetAspect4Password());
		var response = await RestClient.PostAsync<PostChangesResponse>(request, cancellationToken);
	}
}
