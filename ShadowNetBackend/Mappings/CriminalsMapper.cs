using MediatR;
using ShadowNetBackend.Features.Criminals;
using ShadowNetBackend.Migrations;

namespace ShadowNetBackend.Mappings;

public static class CriminalsMapper
{
    public static CriminalDto ToCriminalDto(this Criminal criminal)
    {
        return new CriminalDto
        (
            criminal.Id,
            criminal.FirstName,
            criminal.LastName,
            criminal.Alias,
            criminal.DateOfBirth,
            criminal.Nationality,
            criminal.KnownAffiliations,
            criminal.ThreatLevel,
            criminal.IsArmedAndDangerous,
            criminal.LastKnownLocation,
            criminal.LastSpottedDate,
            criminal.UnderSurveillance,
            criminal.SurveillanceNotes,
            criminal.Image != null ? FileHelper.ConvertToBase64(criminal.Image) : null
        );
    }

    public static IEnumerable<CriminalDto> ToCriminalDto(this IEnumerable<Criminal> criminals) => criminals.Select(s => s.ToCriminalDto());

    public static Criminal ToCriminal(this CriminalForCreationDto criminalForCreation)
    {
        return new Criminal
        {
            FirstName = criminalForCreation.FirstName,
            LastName = criminalForCreation.LastName,
            Alias = criminalForCreation.Alias,
            DateOfBirth = criminalForCreation.DateOfBirth,
            Nationality = criminalForCreation.Nationality,
            KnownAffiliations = criminalForCreation.KnownAffiliations,
            ThreatLevel = criminalForCreation.ThreatLevel,
            IsArmedAndDangerous = criminalForCreation.IsArmedAndDangerous,
            LastKnownLocation = criminalForCreation.LastKnownLocation,
            LastSpottedDate = criminalForCreation.LastSpottedDate,
            UnderSurveillance = criminalForCreation.UnderSurveillance,
            SurveillanceNotes = criminalForCreation.SurveillanceNotes,
            Image = criminalForCreation.Image != null ? FileHelper.ConvertFromBase64(criminalForCreation.Image) : null
        };
    }
}
