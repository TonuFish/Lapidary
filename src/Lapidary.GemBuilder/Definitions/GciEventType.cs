namespace Lapidary.GemBuilder.Definitions;

public enum GciEventType
{
    gciSigNone,
    gciSigAbort,
    gciSigCommittedObjs,
    gciSigFinishTrans,
    gciSignalFromSession,
    gciSigLostOt,
    gciSigLostSession,
}
