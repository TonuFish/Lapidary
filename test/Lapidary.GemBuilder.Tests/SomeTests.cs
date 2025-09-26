using Lapidary.GemBuilder.Tests.Fixtures;

namespace Lapidary.GemBuilder.Tests;

[Collection(nameof(GemStoneCollection))]
public sealed class SomeTests
{
	private readonly GemStoneFixture _fixture;

	public SomeTests(GemStoneFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public void Placeholder()
	{
		var expected = FFI.NewLargeInteger(_fixture.Session, 765);
		var result = FFI.Execute(_fixture.Session, "765"u8);
		Assert.Equal(result, expected);
	}
}
