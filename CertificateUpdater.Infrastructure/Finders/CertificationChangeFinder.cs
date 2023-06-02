using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Enum;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Finders;
public class CertificationChangeFinder : ICertificationChangeFinder
{
	public ICollection<CertificationChange> FindCertificationChanges(ICollection<Product> products)
	{
		ICollection<CertificationChange> certificationChanges = new List<CertificationChange>();
		foreach (Product product in products)
		{
			CertificationChange certificationChange = new()
			{
				CompanyName = product.CompanyName,
				DBNr = product.DBNr,
				ProductText = product.ProductText,
				SupplierNr = product.SupplierNr
			};

			foreach (var katalog in product.KatalogData)
			{
				switch (katalog.EmneId)
				{
					case (int)CertificationEnum.DGNB:
						certificationChange.hasDGNB = katalog.isValid;
						break;
					case (int)CertificationEnum.Svanemærke:
						certificationChange.hasSvanemærke = katalog.isValid;
						break;
					case (int)CertificationEnum.SvanemærkeByggeri:
						certificationChange.hasSvanemærkeByggeri = katalog.isValid;
						break;
					case (int)CertificationEnum.BREEAM:
						certificationChange.hasBREEAM = katalog.isValid;
						break;
					case (int)CertificationEnum.Cradle2Cradle:
						certificationChange.hasC2C = katalog.isValid;
						break;
					case (int)CertificationEnum.DerBlaueEngel:
						certificationChange.hasDBE = katalog.isValid;
						break;
					case (int)CertificationEnum.FSC:
						certificationChange.hasFSC = katalog.isValid;
						break;
					case (int)CertificationEnum.PEFC:
						certificationChange.hasPEFC = katalog.isValid;
						break;
					case (int)CertificationEnum.IndeKlima:
						certificationChange.hasIndeKlima = katalog.isValid;
						break;
					case (int)CertificationEnum.SvanemærkeEUBlomse:
						certificationChange.hasEUBlomst = katalog.isValid;
						break;
					case (int)CertificationEnum.AstmaOgAllergi:
						certificationChange.hasAstmaOgAllergi = katalog.isValid;
						break;
					case (int)CertificationEnum.EPD:
						certificationChange.hasEPD = katalog.isValid;
						break;
					case (int)CertificationEnum.AllergyUK:
						certificationChange.hasALUK = katalog.isValid;
						break;
					case (int)CertificationEnum.LEED:
						certificationChange.hasLEED = katalog.isValid;
						break;
					default:
						break;
				}
			}
			certificationChanges.Add(certificationChange);
		}
		return certificationChanges;
	}

}
