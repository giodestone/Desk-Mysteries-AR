/// <summary>
/// Enum which indexes the state that <see cref="AreaScanningManager"/> is in.
/// </summary>
public enum AreaScanningManagerState
{
    FindingPlane,
    WaitingForUserToPlaceObject,
    PlaneTrackedAndObjectPlaced,
    DoNotSearch
}
