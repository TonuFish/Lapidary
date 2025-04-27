using System.Numerics;
using Lapidary.GemBuilder.Converters;
using Lapidary.GemBuilder.Converters.Special;

namespace Lapidary.GemBuilder;

#pragma warning disable CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
public readonly ref partial struct GemObject
#pragma warning restore CS0660, S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
{
    public bool GetBoolean()
    {
        var converter = GetStructInformation(typeof(bool));
        return converter.ConvertToStruct<bool>(this);
    }

    public T GetClass<T>() where T : class
    {
        var converter = GetClassInformation(typeof(T));
        if (!converter.CanConvertToClass)
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        return converter.ConvertToClass<T>(this);
    }

    public DateOnly GetDate()
    {
        var converter = GetStructInformation(typeof(DateOnly));
        return converter.ConvertToStruct<DateOnly>(this);
    }

    public DateTime GetDateTime()
    {
        var converter = GetStructInformation(typeof(DateTime));
        return converter.ConvertToStruct<DateTime>(this);
    }

    public T GetNumber<T>() where T : INumber<T>
    {
        var converter = GetNumberInformation();
        if (!converter.CanConvertToNumber)
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        // For when you just want a number and can trust yourself to not lose precision.
        return converter.ConvertToNumber<T>(this);
    }

    public string GetString()
    {
        var converter = GetClassInformation(typeof(string));
        return converter.ConvertToClass<string>(this);
    }

    public T GetStruct<T>() where T : struct
    {
        var converter = GetStructInformation(typeof(T));
        if (!converter.CanConvertToStruct)
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        return converter.ConvertToStruct<T>(this);
    }

    public TimeOnly GetTime()
    {
        var converter = GetStructInformation(typeof(TimeOnly));
        return converter.ConvertToStruct<TimeOnly>(this);
    }

    #region Get Class Information

    internal ILapidaryConverter GetClassInformation(Type targetType)
    {
        var type = GetConverterFromOop();
        if (type is not null && type.ConversionType != targetType)
        {
            // TODO: Found from default list, but isn't correct
            ThrowHelper.GenericExceptionToDetailLater();
        }

        if (!FFI.TryGetObjectInfo(Session, Oop, out var info))
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        var converter = Session.GetClassConverter(targetType, info.objClass);
        if (converter is null)
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        return converter;
    }

    internal ILapidaryConverter GetNumberInformation()
    {
        var type = GetConverterFromOop();
        if (type is not null)
        {
            // TODO: *VERY* not right, but later.
            return type;
        }

        if (!FFI.TryGetObjectInfo(Session, Oop, out var info))
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        var converter = Session.GetNumberConverter(info.objClass);
        if (converter is null)
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        return converter;
    }

    internal ILapidaryConverter GetStructInformation(Type targetType)
    {
        var type = GetConverterFromOop();
        if (type is not null && type.ConversionType != targetType)
        {
            // TODO: Found from default list, but isn't correct
            ThrowHelper.GenericExceptionToDetailLater();
        }

        if (!FFI.TryGetObjectInfo(Session, Oop, out var info))
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        var converter = Session.GetStructConverter(targetType, info.objClass);
        if (converter is null)
        {
            ThrowHelper.GenericExceptionToDetailLater();
        }

        return converter;
    }

    #region Hideous

    // TODO: Remove hack
    private static readonly IllegalConverter _illegalConverter = new();
    private static readonly NilConverter _nilConverter = new();
    private static readonly BooleanConverter _booleanConverter = new();
    private static readonly SmallDoubleConverter _smallDoubleConverter = new();
    private static readonly SmallIntegerConverter _smallIntegerConverter = new();

    internal ILapidaryConverter? GetConverterFromOop()
    {
        // TODO: Lookup, not hack
        // TODO: Dodgy type handling... Illegal/unhandler special types, oop 0, etc.
        // TODO: Widths
        if ((Oop & ReservedOops.OOP_TAG_POM_OOP) != 0UL)
        {
            return Oop == ReservedOops.OOP_ILLEGAL ? _illegalConverter : null;
        }

        return (Oop & ReservedOops.OOP_RAM_TAG_MASK) switch
        {
            ReservedOops.OOP_TAG_SMALLDOUBLE => _smallDoubleConverter,
            ReservedOops.OOP_TAG_SMALLINT => _smallIntegerConverter,
            ReservedOops.OOP_TAG_SPECIAL => (Oop & ReservedOops.OOP_SPECIAL_CLASS_MASK) switch
            {
                ReservedOops.SpecialClassTags.Boolean => _booleanConverter,
                ReservedOops.SpecialClassTags.Undefined => _nilConverter,
                ReservedOops.SpecialClassTags.Character => null,
                ReservedOops.SpecialClassTags.JisCharacter => null,
                ReservedOops.SpecialClassTags.SmallFraction => null,
                ReservedOops.SpecialClassTags.SmallScaledDecimal => null,
                ReservedOops.SpecialClassTags.SmallDateAndTime => null,
                ReservedOops.SpecialClassTags.SmallTime => null,
                ReservedOops.SpecialClassTags.SmallDate => null,
                _ => null, // TODO: Consider if unhandled specials should throw instead, probably.
            },
            _ => null, // TODO: Throw, the only way this branch would be hit is with oop 0, which <should> never happen.
        };
    }

    #endregion Hideous

    #endregion Get Class Information
}
