using System;
using System.Runtime.CompilerServices;

namespace Lapidary.Samples;

internal static class Program
{
	private static void Main()
	{
		ReworkExample.Foo();
	}

	[ModuleInitializer]
	internal static void Init()
	{
		OnModuleLoad.EnsureGemBuilderFilesExist(AppContext.BaseDirectory);
	}
}
