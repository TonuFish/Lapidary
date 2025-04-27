namespace Lapidary.GemBuilder.Definitions;

internal enum GciEventType
{
    gciSigNone,
    gciSigAbort,
    gciSigCommittedObjs,
    gciSigFinishTrans,
    gciSignalFromSession,
    gciSigLostOt,
    gciSigLostSession,
}
