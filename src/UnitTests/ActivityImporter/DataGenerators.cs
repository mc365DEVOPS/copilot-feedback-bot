﻿using ActivityImporter.Engine.ActivityAPI;
using ActivityImporter.Engine.ActivityAPI.Models;

namespace UnitTests.ActivityImporter;


internal class DataGenerators
{

    // For generating random data
    protected static Random random = new Random();

    /// <summary>
    /// Generate testing random object
    /// </summary>
    public static SharePointAuditLogContent GetRandomSharePointLog(int lookupsMax)
    {
        var log = new SharePointAuditLogContent();
        log.Workload = ActivityImportConstants.WORKLOAD_SP;
        log.Id = Guid.NewGuid();
        log.UserId = GetRandomLookup(lookupsMax, LookupType.UserID);
        log.Operation = GetRandomLookup(lookupsMax, LookupType.Operation);
        log.SourceFileName = GetRandomLookup(lookupsMax, LookupType.SourceFileName);
        log.SourceFileExtension = GetRandomLookup(lookupsMax, LookupType.SourceFileExtension);

        // URL
        log.ObjectId = "https://sharepoint/sites/site/files/file" + GetRandomString(50) + "/" + log.SourceFileExtension;
        int randomSeconds = random.Next(1, 999999);
        log.CreationTime = DateTime.Now.AddSeconds(randomSeconds - randomSeconds * 2);
        log.SiteUrl = GetRandomLookup(lookupsMax, LookupType.SiteURL);
        log.ItemType = GetRandomLookup(lookupsMax, LookupType.Type);

        return log;
    }
    public static SharePointAuditLogContent GetRandomSharePointLog()
    {
        return GetRandomSharePointLog(0);
    }

    public static string GetRandomLookup(int maxLookups, LookupType type)
    {
        string prefix = string.Empty;

        // Set prefix
        switch (type)
        {
            case LookupType.UserID:
                prefix = "User";
                break;
            case LookupType.Operation:
                prefix = "Operation";
                break;
            case LookupType.SourceFileName:
                prefix = "File";
                break;
            case LookupType.SourceFileExtension:
                prefix = "ext";
                break;
            case LookupType.Type:
                prefix = "Type";
                break;
            case LookupType.PageTitle:
                prefix = "Page Title";
                break;
            case LookupType.SiteURL:
                prefix = "https://sharepoint/sites/site";
                break;
            case LookupType.FileURL:
                prefix = "https://sharepoint/sites/site"; // More added below
                break;
            case LookupType.Browser:
                prefix = "Browser";
                break;
            case LookupType.OS:
                prefix = "OS";
                break;
            case LookupType.County:
                prefix = "Province";
                break;
            case LookupType.Country:
                prefix = "Country";
                break;
            case LookupType.City:
                prefix = "City";
                break;
            case LookupType.WebTitle:
                prefix = "Web";
                break;
            default:
                throw new NotSupportedException("No idea what type");
        }

        string uniqueLookup = $"{prefix} {GetRandomString(5)}";

        // Exceptions
        if (type == LookupType.FileURL)
        {
            uniqueLookup = $"/file-{GetRandomString(5)}.{GetRandomLookup(maxLookups, LookupType.SourceFileExtension)}";
        }

        if (maxLookups == 0)
        {
            return uniqueLookup;
        }
        else if (maxLookups > 0)
        {
            // From cache
            List<string> lookupCache = LookupCaches.Value.Where(c => c.Key == type).SingleOrDefault().Value;

            // Build cache if not already populated
            if (lookupCache.Count < maxLookups)
            {
                lookupCache.Add(uniqueLookup);
                return uniqueLookup;
            }
            else
            {
                // Return random 
                return lookupCache[random.Next(0, maxLookups)];
            }
        }
        else
        {
            throw new InvalidOperationException("Unexpected max lookup count");
        }
    }

    // Lists of random lookups
    static Lazy<Dictionary<LookupType, List<string>>> LookupCaches = new Lazy<Dictionary<LookupType, List<string>>>(() =>
    {
        Dictionary<LookupType, List<string>> caches = new Dictionary<LookupType, List<string>>();

        // Add each type
        foreach (LookupType t in (LookupType[])Enum.GetValues(typeof(LookupType)))
        {
            caches.Add(t, new List<string>());
        }


        return caches;
    });

    public enum LookupType
    {
        Unknown,
        UserID,
        Operation,
        SourceFileName,
        SourceFileExtension,
        Type,
        PageTitle,
        SiteURL,
        FileURL,
        Country,
        County,
        City,
        Browser,
        OS,
        WebTitle
    }

    public static string GetRandomString(int length = 16)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
