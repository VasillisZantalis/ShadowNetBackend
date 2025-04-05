using ShadowNetBackend.Features.Criminals;
using ShadowNetBackend.Features.Criminals.Common;
using ShadowNetBackend.Features.Criminals.CreateCriminal;

namespace ShadowNetBackend.Mappings;

public static class CriminalsMapper
{
    public static CriminalResponse ToCriminalResponse(this Criminal criminal)
    {
        return new CriminalResponse
        {
            Id = criminal.Id,
            FirstName = criminal.FirstName,
            LastName = criminal.LastName,
            Alias = criminal.Alias,
            DateOfBirth = criminal.DateOfBirth,
            Nationality = criminal.Nationality,
            KnownAffiliations = criminal.KnownAffiliations,
            ThreatLevel = criminal.ThreatLevel,
            IsArmedAndDangerous = criminal.IsArmedAndDangerous,
            LastKnownLocation = criminal.LastKnownLocation,
            LastSpottedDate = criminal.LastSpottedDate,
            UnderSurveillance = criminal.UnderSurveillance,
            SurveillanceNotes = criminal.SurveillanceNotes,
            Image = criminal.Image != null ? FileHelper.ConvertToBase64(criminal.Image) : null
        };
    }

    public static Criminal ToCriminal(this CreateCriminalCommand criminal)
    {
        return new Criminal
        {
            FirstName = criminal.FirstName,
            LastName = criminal.LastName,
            Alias = criminal.Alias,
            DateOfBirth = criminal.DateOfBirth,
            Nationality = criminal.Nationality,
            KnownAffiliations = criminal.KnownAffiliations,
            ThreatLevel = criminal.ThreatLevel,
            IsArmedAndDangerous = criminal.IsArmedAndDangerous,
            LastKnownLocation = criminal.LastKnownLocation,
            LastSpottedDate = criminal.LastSpottedDate,
            UnderSurveillance = criminal.UnderSurveillance,
            SurveillanceNotes = criminal.SurveillanceNotes,
            Image = criminal.Image != null ? FileHelper.ConvertFromBase64(criminal.Image) : null
        };
    }
}
