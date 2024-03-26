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
			var katalogsWithCertification = product.KatalogData
				.Where(katalog => Enum.IsDefined(typeof(CertificationEnum), katalog.EmneId))
				.ToList();
			CertificationChange certificationChange = new()
			{
				CompanyName = product.CompanyName,
				DBNr = product.DBNr,
				ProductText = product.ProductText,
				SupplierNr = product.SupplierNr,
				ProductGroupId = product.ProductGroupId,
			};
			if (katalogsWithCertification.Any())
			{
				foreach (var katalog in katalogsWithCertification)
				{
					CertificationEnum certificationEnum = (CertificationEnum)katalog.EmneId;

					switch (katalog.EmneId)
					{
						case (int)CertificationEnum.EPD:
							certificationChange.hasEPD = katalog.isValid;
							break;
						case (int)CertificationEnum.IndeKlima:
							certificationChange.hasIndeKlima = katalog.isValid;
							break;
						case (int)CertificationEnum.FSC:
							certificationChange.hasFSC = katalog.isValid;
							break;
						case (int)CertificationEnum.Svanemærke:
							certificationChange.hasSvanemærke = katalog.isValid;
							break;
						case (int)CertificationEnum.Cradle2Cradle:
							certificationChange.hasC2C = katalog.isValid;
							break;
						case (int)CertificationEnum.DerBlaueEngel:
							certificationChange.hasDBE = katalog.isValid;
							break;
						case (int)CertificationEnum.EUTR:
							certificationChange.hasEUTR = katalog.isValid;
							break;
						case (int)CertificationEnum.AstmaOgAllergi:
							certificationChange.hasAstmaOgAllergi = katalog.isValid;
							break;
						case (int)CertificationEnum.BREEAM:
							certificationChange.hasBREEAM = katalog.isValid;
							break;
						case (int)CertificationEnum.LEED:
							certificationChange.hasLEED = katalog.isValid;
							break;
						case (int)CertificationEnum.SITAC:
							certificationChange.hasSITAC = katalog.isValid;
							break;
						case (int)CertificationEnum.NEMKO:
							certificationChange.hasNEMKO = katalog.isValid;
							break;
						case (int)CertificationEnum.M1:
							certificationChange.hasM1 = katalog.isValid;
							break;
						case (int)CertificationEnum.GlobalCompact:
							certificationChange.hasGlobalCompact = katalog.isValid;
							break;
						case (int)CertificationEnum.AllergyUK:
							certificationChange.hasALUK = katalog.isValid;
							break;
						case (int)CertificationEnum.GEV_EMICODE:
							certificationChange.hasGEV_EMICODE = katalog.isValid;
							break;
						case (int)CertificationEnum.SvanemærkeByggeri:
							certificationChange.hasSvanemærkeByggeri = katalog.isValid;
							break;
						case (int)CertificationEnum.PEFC:
							certificationChange.hasPEFC = katalog.isValid;
							break;
						case (int)CertificationEnum.EUBlomsten:
							certificationChange.hasEUBlomsten = katalog.isValid;
							break;
					}
				}
			}
			if (product.DGNBDocuments.Any())
			{
				foreach (var dgnbDocument in product.DGNBDocuments)
				{
					certificationChange.DGNBQualityStep.Add(Convert.ToString(dgnbDocument.IndicatorNumber) + ":" + Convert.ToString(dgnbDocument.IndicatorStep));
				}
			}
			if (certificationChange.DGNBQualityStep.Any() || katalogsWithCertification.Any())
			{
				certificationChanges.Add(certificationChange);

			}
		}
		return certificationChanges;
	}
}
