﻿namespace Lapidary.GemBuilder.Definitions;

internal static class Version
{
    private const string FullVersionIdentifier = "3.7.0-64";

    /// <summary>
    /// Full name of the GciTs DLL.
    /// </summary>
    internal const string GciTsDLL = $"libgcits-{FullVersionIdentifier}.dll";

    /// <summary>
    /// Full name of the SSL support DLL.
    /// </summary>
    internal const string SslDLL = $"libssl-{FullVersionIdentifier}.dll";
}
