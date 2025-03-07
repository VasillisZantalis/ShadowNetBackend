namespace ShadowNetBackend.Common;

public enum UserRank : short
{
    Administrator = 1,
    Commander = 2,
    Agent = 3,
    Operative = 4,
    WitnessProtectionHandler = 5,
    Witness = 6
}

public enum MissionStatus
{
    Planned, Ongoing, Completed, Failed
}

public enum RiskLevel
{
    Low, Medium, High, Critical
}

public enum RelocationStatus
{
    Pending, InProgress, Completed
}

public enum EncryptionType
{
    AES, RSA
}

public enum ClearanceLevel
{
    None, Confidential, Secret, TopSecret
}

public enum SortingOrder
{
    Asc, Desc
}
