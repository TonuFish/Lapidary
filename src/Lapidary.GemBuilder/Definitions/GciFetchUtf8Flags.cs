namespace Lapidary.GemBuilder.Definitions;

[Flags]
public enum GciFetchUtf8Flags
{
    GCI_UTF8_FetchNormal = 0,

    /// <summary>
    /// Substitute '.' for illegal codepoints.
    /// </summary>
    GCI_UTF8_FilterIllegalCodePoints = 0x1,

    /// <summary>
    /// Return message instead of signalling Exception for illegal codepoints.
    /// </summary>
    GCI_UTF8_NoError = 0x2,
}
