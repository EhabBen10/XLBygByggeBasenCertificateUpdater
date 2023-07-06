using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using Xunit;

namespace CertificateUpdater.Services.Test.Finders;
public sealed class RelevantTunnrFinderUnitTest
{
	private readonly IRelevantTunnrFinder _uut = new RelevantTunnrFinder();
	private readonly ICollection<CatChange> catChanges = new List<CatChange>();

	[Fact]
	public void FindRelevantTunnrs_EmptyInputCollection_ResultListIsEmpty()
	{
		// Arrange


		// Act
		var result = _uut.FindRelevantTunnrs(catChanges);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void FindRelevantTunnrs_NonEmptyInputCollectionEmneIdNotDefined_ResultListIsEmpty()
	{
		// Arrange

		catChanges.Add(new CatChange()
		{
			CreatedAt = new DateTime(24, 12, 1),
			EmneId = 0,
			Tunnr = 0
		});
		// Act
		var result = _uut.FindRelevantTunnrs(catChanges);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void FindRelevantTunnrs_NonEmptyInputCollectionEmneIdDefined_ResultListIsContainsChange()
	{
		// Arrange

		catChanges.Add(new CatChange()
		{
			CreatedAt = new DateTime(24, 12, 1),
			EmneId = 37,
			Tunnr = 12
		});
		// Act
		var result = _uut.FindRelevantTunnrs(catChanges);

		// Assert
		Assert.NotEmpty(result);
		Assert.Equal(12, result.First());
	}

	[Fact]
	public void FindRelevantTunnrs_NonEmptyInputCollectionEmneIdDefinedAddingTheSameTwice_ResultListIsContainsChange()
	{
		// Arrange

		catChanges.Add(new()
		{
			CreatedAt = new DateTime(24, 12, 1),
			EmneId = 37,
			Tunnr = 12
		});
		catChanges.Add(new()
		{
			CreatedAt = new DateTime(24, 12, 1),
			EmneId = 37,
			Tunnr = 12
		});
		// Act
		var result = _uut.FindRelevantTunnrs(catChanges);

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result);
		Assert.Equal(12, result.First());
	}

	[Fact]
	public void FindRelevantTunnrs_NonEmptyInputCollectionEmneIdDefinedAddingTwoDifferentTunnrs_ResultListIsContainsChange()
	{
		// Arrange

		catChanges.Add(new()
		{
			CreatedAt = new DateTime(24, 12, 1),
			EmneId = 37,
			Tunnr = 12
		});
		catChanges.Add(new()
		{
			CreatedAt = new DateTime(24, 12, 1),
			EmneId = 37,
			Tunnr = 11
		});
		// Act
		var result = _uut.FindRelevantTunnrs(catChanges);

		// Assert
		Assert.NotEmpty(result);
		Assert.Equal(2, result.Count);
	}
}
